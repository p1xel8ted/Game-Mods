using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using BepInEx.Bootstrap;
using HarmonyLib;
using MonoMod.Utils;
using UnityEngine;

namespace Shared;

[Harmony]
internal static class StartupLogger
{
    private const string SentinelName = "GYK_ModSummaryLogged";

    [HarmonyWrapSafe] //ctd on some configurations without this; code still runs fine. no idea.
    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlatformSpecific), nameof(PlatformSpecific.FixScreenModeAfterStart))]
    public static void PlatformSpecific_FixScreenModeAfterStart()
    {
        try
        {
            if (GameObject.Find(SentinelName)) return;

            var sentinel = new GameObject(SentinelName);
            UnityEngine.Object.DontDestroyOnLoad(sentinel);

            var version = $"{LazyConsts.VERSION:0.000#}".Replace(",", ".");
            var bepinexVersion = typeof(Chainloader).Assembly.GetName().Version;
            var managerObj = Chainloader.ManagerObject;
            var bepinexManagerHidden = managerObj != null && managerObj.hideFlags.HasFlag(HideFlags.HideAndDontSave);
            var buildGuid = Application.buildGUID;
            var platform = PlatformHelper.Current;
            var store = DetectStorefront();

            var log = BepInEx.Logging.Logger.CreateLogSource("GYK Mods");
            log.LogInfo("==========================================");
            log.LogInfo("  Graveyard Keeper Mod Summary");
            log.LogInfo("==========================================");
            log.LogInfo($"  Game      : ver. {version} (BuildGUID: {buildGuid})");
            log.LogInfo($"  BepInEx   : v{bepinexVersion} (Manager Hidden: {bepinexManagerHidden})");
            log.LogInfo($"  Platform  : {platform}");
            log.LogInfo($"  Storefront: {store}");
            if (!bepinexManagerHidden)
            {
                log.LogWarning("  BepInEx Manager GameObject is NOT hidden — Unity event methods (Awake, Start, Update) will not fire on plugins!");
                log.LogWarning("  To fix: open BepInEx/config/BepInEx.cfg, find [Chainloader] section, set HideManagerGameObject = true");
            }
            log.LogInfo("------------------------------------------");
            log.LogInfo("  Loaded plugins:");

            foreach (var plugin in Chainloader.PluginInfos.Values.OrderBy(p => p.Metadata.Name))
            {
                log.LogInfo($"    {plugin.Metadata.Name} v{plugin.Metadata.Version} | {plugin.Metadata.GUID}");
            }

            log.LogInfo($"------------------------------------------");
            log.LogInfo($"  Total: {Chainloader.PluginInfos.Count} plugins");
            log.LogInfo("==========================================");

            if (!Chainloader.ConfigHideBepInExGOs.Value)
            {
                log.LogWarning("  BepInEx HideManagerGameObject was disabled — enabling it to prevent Unity event methods from failing");
                Chainloader.ConfigHideBepInExGOs.Value = true;
                if (managerObj)
                {
                    managerObj.hideFlags = HideFlags.HideAndDontSave;
                    UnityEngine.Object.DontDestroyOnLoad(managerObj);
                }
            }

            BepInEx.Logging.Logger.Sources.Remove(log);
        }
        catch (System.Exception ex)
        {
            BepInEx.Logging.Logger.CreateLogSource("GYK Mods").LogError($"StartupLogger failed: {ex}");
        }
    }

    private static readonly string[] PiracyFiles =
    [
        // Steam emulators
        "SmartSteamEmu.ini", "steam_emu.ini", "goldberg_emulator.dll",
        "steamclient_loader.dll", "steam_api64_o.dll", "steam_api.cdx", "steam_api64.cdx.dll",
        "steam_interfaces.txt", "local_save.txt", "valve.ini",
        "coldclient.dll", "ColdClientLoader.ini", "steamless.dll", "GreenLuma",
        "SteamFix.dll", "SteamFix64.dll", "LumaEmu.ini", "Lumaplay",

        // Goldberg emulator config
        "account_name.txt", "user_steam_id.txt", "force_listen_port.txt",
        "goldberg_steam_appid.txt",

        // DLC unlockers
        "CreamAPI.dll", "creamapi.dll", "cream_api.ini", "ScreamAPI.dll", "UnlockAll.dll",

        // Proxy loaders
        "Koaloader.dll", "Koaloader64.dll",

        // Online fixes
        "OnlineFix.dll", "OnlineFix.url", "online-fix.me",

        // Scene group markers
        "codex.ini", "codex64.dll", "CODEX",
        "SKIDROW", "SKIDROW.ini",
        "CPY", "PLAZA", "HOODLUM", "EMPRESS", "TENOKE",
        "PROPHET", "REVOLT", "DARKSiDERS", "RAZOR1911",
        "FLT", "FLT.dll", "RUNE", "RUNE.ini",
        "TiNYiSO", "RELOADED", "RLD!", "DOGE", "CHRONOS", "DINOByTES", "I_KnoW",
        "ElAmigos", "FitGirl", "DODI", "xatab", "KaOs", "IGG", "Masquerade",

        // Common crack files
        "3dmgame.dll", "ALI213.dll", "crack.exe", "Crack.nfo",
        "crackfix", "CrackOnly", "gamefix.dll",
        "nosTEAM", "NoSteam", "FCKDRM", "NoDRM", "VALVEEMPRESS",
    ];

    private static string[] GetSearchDirs()
    {
        var root = Directory.GetCurrentDirectory();
        var dirs = new List<string> { root };

        // Unity games store platform DLLs in *_Data/Plugins/ subdirectories
        try
        {
            foreach (var dataDir in Directory.GetDirectories(root, "*_Data"))
            {
                var pluginsDir = Path.Combine(dataDir, "Plugins");
                if (!Directory.Exists(pluginsDir)) continue;
                dirs.Add(pluginsDir);
                dirs.AddRange(Directory.GetDirectories(pluginsDir));
            }
        }
        catch
        {
            // Ignore permission errors
        }

        return dirs.ToArray();
    }

    private static bool FileExistsInAny(string[] dirs, string filename)
    {
        return dirs.Any(d => File.Exists(Path.Combine(d, filename)));
    }

    private static bool DirExistsInAny(string[] dirs, string dirname)
    {
        return dirs.Any(d => Directory.Exists(Path.Combine(d, dirname)));
    }

    private static string DetectStorefront()
    {
        var root = Directory.GetCurrentDirectory();
        var dirs = GetSearchDirs();
        var store = "Unknown";

        if (FileExistsInAny(dirs, "steam_api.dll") ||
            FileExistsInAny(dirs, "steam_api64.dll") ||
            File.Exists(Path.Combine(root, "steam_appid.txt")))
            store = "Steam";
        else if (Directory.GetFiles(root, "goggame-*.info").Any() ||
                 FileExistsInAny(dirs, "galaxy.dll") ||
                 FileExistsInAny(dirs, "Galaxy64.dll") ||
                 FileExistsInAny(dirs, "GalaxyPeer.dll"))
            store = "GOG";
        else if (FileExistsInAny(dirs, "EOSSDK-Win64-Shipping.dll") ||
                 FileExistsInAny(dirs, "EpicOnlineServices.dll") ||
                 Directory.Exists(Path.Combine(root, ".egstore")))
            store = "Epic";
        else if (root.Contains("WindowsApps") ||
                 File.Exists(Path.Combine(root, "appxmanifest.xml")) ||
                 File.Exists(Path.Combine(root, "microsoft.gameconfig")))
            store = "Xbox/Microsoft Store";
        else if (IsProcessRunning("steam")) store = "Steam (process only)";
        else if (IsProcessRunning("GalaxyClient")) store = "GOG (process only)";
        else if (IsProcessRunning("EpicGamesLauncher")) store = "Epic (process only)";
        else if (IsProcessRunning("XboxApp") || IsProcessRunning("GamingServices")) store = "Xbox (process only)";

        var isPirated = PiracyFiles.Any(pirate =>
            FileExistsInAny(dirs, pirate) || DirExistsInAny(dirs, pirate)
        );

        // Goldberg emulator leaves config in steam_settings/
        if (!isPirated && Directory.Exists(Path.Combine(root, "steam_settings")))
        {
            var settingsDir = Path.Combine(root, "steam_settings");
            isPirated = File.Exists(Path.Combine(settingsDir, "force_account_name.txt")) ||
                        File.Exists(Path.Combine(settingsDir, "force_steamid.txt")) ||
                        File.Exists(Path.Combine(settingsDir, "force_language.txt"));
        }

        if (isPirated)
            store += " + Possible Pirated/Cracked Files Found!";

        return store;
    }

    private static bool IsProcessRunning(string name) => Process.GetProcessesByName(name).Length > 0;
}