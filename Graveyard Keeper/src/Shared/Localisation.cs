using System.Collections.Generic;
using System.IO;

namespace Shared;

internal static class Lang
{
    private static Dictionary<string, string> _translations = new();
    private static Dictionary<string, string> _fallback = new();
    // normalized lang code ("pt_br", "zh_cn", "en") → absolute file path on disk.
    // Lets us match a user's file regardless of how they cased it or whether they
    // used `-` or `_` as the separator — the game's _cur_lng is normalized the
    // same way before lookup, so all four permutations of e.g. "pt-br" resolve
    // to the same file.
    private static Dictionary<string, string> _langFiles = new();
    private static string _langDir;
    private static string _prefix;
    private static string _currentLang;
    private static ManualLogSource _log;

    internal static void Init(System.Reflection.Assembly modAssembly, ManualLogSource log)
    {
        _log = log;
        _langDir = Path.Combine(Path.GetDirectoryName(modAssembly.Location)!, "lang");
        _prefix = modAssembly.GetName().Name;

        IndexLangFiles();

        _fallback = LoadLang("en");
        if (_fallback.Count == 0)
        {
            _log?.LogWarning($"[Lang] No English fallback loaded — translations will return raw keys");
        }

        Reload();
    }

    internal static void Reload()
    {
        var lang = GameSettings._cur_lng;
        if (string.IsNullOrEmpty(lang)) lang = "en";
        if (_currentLang == lang) return;
        _currentLang = lang;
        _translations = Normalize(lang) == "en" ? _fallback : LoadLang(lang);
    }

    internal static string Get(string key)
    {
        // BepInEx plugins Awake() before the game assigns GameSettings._cur_lng,
        // so re-check on first lookup (and on any mid-game language change).
        var current = GameSettings._cur_lng;
        if (string.IsNullOrEmpty(current)) current = "en";
        if (_currentLang != current) Reload();

        if (_translations.TryGetValue(key, out var value)) return value;
        if (_fallback.TryGetValue(key, out var fallback)) return fallback;
        _log?.LogWarning($"[Lang] Missing key: {key}");
        return key;
    }

    // Scan the lang dir once; build the normalized-code → path table. Called once at
    // Init so a mid-session file drop won't be picked up, matching the previous
    // behaviour. Duplicate normalized keys (e.g. both pt-br.json and pt_BR.json on
    // a case-sensitive FS) keep the first one found and log a warning.
    private static void IndexLangFiles()
    {
        _langFiles.Clear();
        if (!Directory.Exists(_langDir))
        {
            _log?.LogWarning($"[Lang] Lang directory not found: {_langDir}");
            return;
        }

        foreach (var path in Directory.GetFiles(_langDir, $"{_prefix}.*.json"))
        {
            var name = Path.GetFileNameWithoutExtension(path);
            var prefix = _prefix + ".";
            if (!name.StartsWith(prefix)) continue;
            var code = name.Substring(prefix.Length);
            var key = Normalize(code);
            if (_langFiles.ContainsKey(key))
            {
                _log?.LogWarning($"[Lang] Duplicate lang file for '{key}': keeping {Path.GetFileName(_langFiles[key])}, ignoring {Path.GetFileName(path)}");
                continue;
            }
            _langFiles[key] = path;
        }
    }

    private static Dictionary<string, string> LoadLang(string lang)
    {
        var key = Normalize(lang);
        if (!_langFiles.TryGetValue(key, out var path))
        {
            if (key != "en")
            {
                _log?.LogInfo($"[Lang] No translation file for '{lang}' (normalized '{key}'), falling back to English");
            }
            return new Dictionary<string, string>();
        }

        try
        {
            var dict = new Dictionary<string, string>();
            var json = File.ReadAllText(path);

            // Minimal JSON parser for flat key-value objects — no dependencies needed
            var i = json.IndexOf('{') + 1;
            while (i < json.Length)
            {
                var keyStart = json.IndexOf('"', i);
                if (keyStart < 0) break;
                var keyEnd = json.IndexOf('"', keyStart + 1);
                var jsonKey = json.Substring(keyStart + 1, keyEnd - keyStart - 1);

                var valStart = json.IndexOf('"', keyEnd + 1);
                var valEnd = FindUnescapedQuote(json, valStart + 1);
                var val = json.Substring(valStart + 1, valEnd - valStart - 1)
                    .Replace("\\\"", "\"")
                    .Replace("\\\\", "\\")
                    .Replace("\\n", "\n");

                dict[jsonKey] = val;
                i = valEnd + 1;
            }

            _log?.LogInfo($"[Lang] Loaded {dict.Count} keys from {Path.GetFileName(path)}");
            return dict;
        }
        catch (System.Exception ex)
        {
            _log?.LogError($"[Lang] Failed to read {Path.GetFileName(path)}: {ex.Message}");
            return new Dictionary<string, string>();
        }
    }

    private static string Normalize(string code)
    {
        return string.IsNullOrEmpty(code) ? "en" : code.ToLowerInvariant().Replace('-', '_');
    }

    private static int FindUnescapedQuote(string s, int start)
    {
        for (var i = start; i < s.Length; i++)
        {
            if (s[i] == '\\') { i++; continue; }
            if (s[i] == '"') return i;
        }
        return s.Length;
    }
}
