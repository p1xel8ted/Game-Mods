using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using UnityEngine;
using UnityEngine.Networking;

namespace Shared;

// Each mod DLL compiles its own copy of this file — static fields are per-assembly.
// Cross-DLL coordination uses a DontDestroyOnLoad sentinel GameObject whose child
// GameObject names carry the wire format (REG|..., OUT|...). No CLR type identity
// is required across assemblies.

// JsonUtility assigns these fields via reflection — compiler doesn't see the writes.
#pragma warning disable CS0649

[Serializable]
internal class UpdateCheckerManifest
{
    public int schema;
    public string generated_at;
    public string generated_by;
    public string game_domain;
    public UpdateCheckerManifestEntry[] mods;
}

[Serializable]
internal class UpdateCheckerManifestEntry
{
    public string plugin_guid;
    public int nexus_mod_id;
    public string nexus_url;
    public string latest_version;
    public int latest_file_id;
    public string latest_file_name;
    public long uploaded_unix;
    public string status;
}

[Serializable]
internal class UpdateCheckerCacheWrapper
{
    public string cached_at;
    public string body;
}

#pragma warning restore CS0649

internal static class UpdateChecker
{
    internal const string SentinelName = "GYK_UpdateChecker";
    internal const string ManifestUrl = "https://raw.githubusercontent.com/p1xel8ted/Game-Mods/main/Graveyard%20Keeper/versions.json";
    internal const int CacheTtlHours = 4;
    internal const string LogSourceName = "GYK_UpdateChecker";

    // Each mod calls this from Plugin.Awake():
    //     UpdateChecker.Register(Info, CheckForUpdates);
    // If enabledToggle is non-null and false, the registration is silently dropped.
    // Exceptions are swallowed — mod loading must never fail because of this.
    public static void Register(PluginInfo info, ConfigEntry<bool> enabledToggle)
    {
        try
        {
            if (info?.Metadata == null) return;
            if (enabledToggle != null && !enabledToggle.Value) return;

            var guid = info.Metadata.GUID;
            var version = info.Metadata.Version != null ? info.Metadata.Version.ToString() : null;
            var name = info.Metadata.Name;
            if (string.IsNullOrEmpty(guid) || string.IsNullOrEmpty(version)) return;

            var sentinel = GameObject.Find(SentinelName);
            var isFirst = !sentinel;
            if (isFirst)
            {
                sentinel = new GameObject(SentinelName);
                UnityEngine.Object.DontDestroyOnLoad(sentinel);
            }

            // Encode the registration as a child GameObject name — cross-assembly-safe.
            var regChild = new GameObject($"REG|{guid}|{version}|{name}");
            regChild.transform.SetParent(sentinel.transform, false);

            if (isFirst)
            {
                sentinel.AddComponent<UpdateCheckerCoordinator>();
            }
        }
        catch (Exception ex)
        {
            BepInEx.Logging.Logger.CreateLogSource(LogSourceName).LogError($"Register failed: {ex}");
        }
    }

    // Reads installed outdated entries from the sentinel — used by UpdateCheckerUI.
    internal static List<OutdatedEntry> GetOutdated()
    {
        var list = new List<OutdatedEntry>();
        var sentinel = GameObject.Find(SentinelName);
        if (!sentinel) return list;

        foreach (Transform child in sentinel.transform)
        {
            var n = child.name;
            if (string.IsNullOrEmpty(n) || !n.StartsWith("OUT|")) continue;
            var parts = n.Split('|');
            // OUT|guid|installedVer|latestVer|url|modName
            if (parts.Length < 6) continue;
            list.Add(new OutdatedEntry
            {
                Guid = parts[1],
                InstalledVersion = parts[2],
                LatestVersion = parts[3],
                NexusUrl = parts[4],
                ModName = parts[5]
            });
        }
        return list;
    }

    internal class OutdatedEntry
    {
        public string Guid;
        public string ModName;
        public string InstalledVersion;
        public string LatestVersion;
        public string NexusUrl;
    }

    internal static string GetCachePath()
    {
        var asmLoc = typeof(UpdateChecker).Assembly.Location;
        if (string.IsNullOrEmpty(asmLoc))
        {
            return Path.Combine(".", "_shared", "gyk-update-cache.json");
        }
        // <ModDir> / <ModName>.dll  →  plugins / <ModDir>  →  plugins
        var modDir = Path.GetDirectoryName(asmLoc);
        var pluginsDir = Path.GetDirectoryName(modDir);
        return Path.Combine(pluginsDir ?? ".", "_shared", "gyk-update-cache.json");
    }

    internal static bool TryReadCache(out string body, out TimeSpan age)
    {
        body = null;
        age = TimeSpan.MaxValue;
        try
        {
            var path = GetCachePath();
            if (!File.Exists(path)) return false;
            var text = File.ReadAllText(path);
            var wrapper = JsonUtility.FromJson<UpdateCheckerCacheWrapper>(text);
            if (wrapper == null || string.IsNullOrEmpty(wrapper.body) || string.IsNullOrEmpty(wrapper.cached_at)) return false;
            if (!DateTime.TryParse(wrapper.cached_at, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var when)) return false;
            age = DateTime.UtcNow - when.ToUniversalTime();
            body = wrapper.body;
            return true;
        }
        catch
        {
            body = null;
            age = TimeSpan.MaxValue;
            return false;
        }
    }

    internal static void WriteCache(string body)
    {
        try
        {
            var path = GetCachePath();
            var dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var wrapper = new UpdateCheckerCacheWrapper
            {
                cached_at = DateTime.UtcNow.ToString("o"),
                body = body
            };
            var text = JsonUtility.ToJson(wrapper);
            var tmp = path + ".tmp";
            File.WriteAllText(tmp, text);
            if (File.Exists(path)) File.Delete(path);
            File.Move(tmp, path);
        }
        catch
        {
            // cache write failure is non-fatal
        }
    }

    internal static bool IsNewer(string installed, string remote)
    {
        if (string.IsNullOrEmpty(remote)) return false;
        // System.Version — disambiguated from UnityEngine.Networking.Version
        if (System.Version.TryParse(installed, out var i) && System.Version.TryParse(remote, out var r)) return r > i;
        // Non-SemVer tag — flag any inequality (rare; logged at call-site).
        return !string.Equals(installed, remote, StringComparison.OrdinalIgnoreCase);
    }

    internal static Dictionary<string, UpdateCheckerManifestEntry> ParseManifest(string body)
    {
        var dict = new Dictionary<string, UpdateCheckerManifestEntry>();
        try
        {
            var parsed = JsonUtility.FromJson<UpdateCheckerManifest>(body);
            if (parsed?.mods == null) return dict;
            foreach (var entry in parsed.mods)
            {
                if (entry == null || string.IsNullOrEmpty(entry.plugin_guid)) continue;
                dict[entry.plugin_guid] = entry;
            }
        }
        catch
        {
            // malformed JSON — return whatever we have
        }
        return dict;
    }
}

internal class UpdateCheckerCoordinator : MonoBehaviour
{
    private static readonly ManualLogSource Log = BepInEx.Logging.Logger.CreateLogSource(UpdateChecker.LogSourceName);
    private static bool _hasRun;

    private IEnumerator Start()
    {
        if (_hasRun) yield break;
        _hasRun = true;

        yield return null; // late-Awake grace window

        string body = null;

        if (UpdateChecker.TryReadCache(out var cached, out var age) && age.TotalHours < UpdateChecker.CacheTtlHours)
        {
            body = cached;
            Log.LogInfo($"Using cache (age: {age.TotalMinutes:F1} min)");
        }
        else if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            // OS reports no network — don't bother with a 10s timeout. Fall back to
            // stale cache if any. Captive portals / DNS holes still reach the fetch path.
            Log.LogInfo("No network (Application.internetReachability reports NotReachable) — falling back to cache if any");
            if (UpdateChecker.TryReadCache(out var stale, out _)) body = stale;
        }
        else
        {
            using var req = UnityWebRequest.Get(UpdateChecker.ManifestUrl);
            req.timeout = 10;
            req.SetRequestHeader("User-Agent", "GYK-UpdateChecker/1.0");
            yield return req.SendWebRequest();

            var ok = req.result == UnityWebRequest.Result.Success;
            if (ok)
            {
                body = req.downloadHandler?.text;
                if (!string.IsNullOrEmpty(body))
                {
                    UpdateChecker.WriteCache(body);
                    Log.LogInfo("Fetched manifest from GitHub");
                }
            }
            else
            {
                Log.LogWarning($"Fetch failed: {req.error} — falling back to cache if any");
                if (UpdateChecker.TryReadCache(out var stale, out _)) body = stale;
            }
        }

        if (string.IsNullOrEmpty(body))
        {
            Log.LogInfo("No manifest data — update check skipped this session");
            yield break;
        }

        var manifest = UpdateChecker.ParseManifest(body);
        DiffAndStore(manifest);
    }

    private void DiffAndStore(Dictionary<string, UpdateCheckerManifestEntry> manifest)
    {
        var sentinel = GameObject.Find(UpdateChecker.SentinelName);
        if (!sentinel) return;

        var alreadyMarked = new HashSet<string>();
        foreach (Transform child in sentinel.transform)
        {
            if (child.name != null && child.name.StartsWith("OUT|"))
            {
                var p = child.name.Split('|');
                if (p.Length >= 2) alreadyMarked.Add(p[1]);
            }
        }

        int outdated = 0, upToDate = 0, unknown = 0, nonSemVer = 0;
        foreach (Transform child in sentinel.transform)
        {
            var n = child.name;
            if (string.IsNullOrEmpty(n) || !n.StartsWith("REG|")) continue;
            var parts = n.Split('|');
            if (parts.Length < 4) continue;
            var guid = parts[1];
            var installedVer = parts[2];
            var modName = parts[3];

            if (!manifest.TryGetValue(guid, out var entry))
            {
                unknown++;
                continue;
            }
            if (!string.Equals(entry.status, "ok", StringComparison.OrdinalIgnoreCase))
            {
                unknown++;
                continue;
            }
            if (!UpdateChecker.IsNewer(installedVer, entry.latest_version))
            {
                upToDate++;
                continue;
            }

            if (!System.Version.TryParse(installedVer, out _) || !System.Version.TryParse(entry.latest_version, out _))
            {
                nonSemVer++;
                Log.LogWarning($"Non-SemVer version compare for {guid}: '{installedVer}' vs '{entry.latest_version}' — string inequality fallback fired");
            }

            outdated++;
            if (alreadyMarked.Contains(guid)) continue;
            var outChild = new GameObject($"OUT|{guid}|{installedVer}|{entry.latest_version}|{entry.nexus_url}|{modName}");
            outChild.transform.SetParent(sentinel.transform, false);
        }

        var nonSemVerNote = nonSemVer > 0 ? $", {nonSemVer} via string fallback" : "";
        Log.LogInfo($"{outdated} outdated, {upToDate} up to date, {unknown} not in manifest{nonSemVerNote}");

        // If the menu is already showing (user reached it before the fetch returned),
        // trigger a re-render so the label appears without waiting for a menu reopen.
        try
        {
            UpdateCheckerUI.RefreshIfMenuOpen();
        }
        catch
        {
            // UI refresh failure is non-fatal
        }
    }
}
