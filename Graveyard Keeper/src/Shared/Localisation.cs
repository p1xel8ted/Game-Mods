using System.Collections.Generic;
using System.IO;

namespace Shared;

internal static class Lang
{
    private static Dictionary<string, string> _translations = new();
    private static Dictionary<string, string> _fallback = new();
    private static string _langDir;
    private static string _prefix;
    private static ManualLogSource _log;

    internal static void Init(System.Reflection.Assembly modAssembly, ManualLogSource log)
    {
        _log = log;
        _langDir = Path.Combine(Path.GetDirectoryName(modAssembly.Location)!, "lang");
        _prefix = modAssembly.GetName().Name;

        if (!Directory.Exists(_langDir))
        {
            _log?.LogWarning($"[Lang] Lang directory not found: {_langDir}");
        }

        _fallback = LoadFile("en");
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
        _translations = lang == "en" ? _fallback : LoadFile(lang);
    }

    internal static string Get(string key)
    {
        if (_translations.TryGetValue(key, out var value)) return value;
        if (_fallback.TryGetValue(key, out var fallback)) return fallback;
        _log?.LogWarning($"[Lang] Missing key: {key}");
        return key;
    }

    private static Dictionary<string, string> LoadFile(string lang)
    {
        var path = Path.Combine(_langDir, $"{_prefix}.{lang}.json");
        if (!File.Exists(path))
        {
            if (lang != "en")
            {
                _log?.LogInfo($"[Lang] No translation file for '{lang}', falling back to English");
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
                var key = json.Substring(keyStart + 1, keyEnd - keyStart - 1);

                var valStart = json.IndexOf('"', keyEnd + 1);
                var valEnd = FindUnescapedQuote(json, valStart + 1);
                var val = json.Substring(valStart + 1, valEnd - valStart - 1)
                    .Replace("\\\"", "\"")
                    .Replace("\\\\", "\\")
                    .Replace("\\n", "\n");

                dict[key] = val;
                i = valEnd + 1;
            }

            _log?.LogInfo($"[Lang] Loaded {dict.Count} keys from {_prefix}.{lang}.json");
            return dict;
        }
        catch (System.Exception ex)
        {
            _log?.LogError($"[Lang] Failed to read {_prefix}.{lang}.json: {ex.Message}");
            return new Dictionary<string, string>();
        }
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
