using System.Diagnostics;
using System.IO;
using System.Linq;
using BepInEx.Logging;
using MonoMod.Utils;
using UnityEngine;

namespace GYKHelper;

internal static class StartupLogger
{
    public static void PrintModLoaded(string plugin, ManualLogSource logger)
    {
        var version = Application.version.Replace("\r", "").Replace("\n", "");
        var buildGuid = Application.buildGUID;
        var platform = PlatformHelper.Current;
        var store = DetectStorefront();

        logger.LogInfo("==========================================");
        logger.LogInfo($"  Plugin Loaded: {plugin}");
        logger.LogInfo($"  Version   : {version} (BuildGUID: {buildGuid})");
        logger.LogInfo($"  Platform  : {platform}");
        logger.LogInfo($"  Storefront: {store}");
        logger.LogInfo("==========================================");
    }

    private static readonly string[] PiracyFiles =
    [
        // Emulators and loaders
        "SmartSteamEmu.ini", "codex.ini", "steam_emu.ini", "goldberg_emulator.dll",
        "steamclient_loader.dll", "steam_api64_o.dll", "steam_api.cdx", "steam_api64.cdx.dll",
        "steam_interfaces.txt", "local_save.txt", "valve.ini", "codex64.dll",
        "coldclient.dll", "ColdClientLoader.ini", "steamless.dll", "GreenLuma",

        // DLC unlockers
        "CreamAPI.dll", "cream_api.ini", "ScreamAPI.dll",

        // Online fixes
        "OnlineFix.dll", "OnlineFix.url", "online-fix.me",

        // Scene groups
        "CODEX", "SKIDROW", "CPY", "PLAZA", "HOODLUM", "EMPRESS", "TENOKE",
        "PROPHET", "REVOLT", "DARKSiDERS", "RAZOR1911", "FLT", "FLT.dll",
        "RUNE", "RUNE.ini", "TiNYiSO", "RELOADED", "RLD!", "DOGE", "BAT", "P2P",
        "ElAmigos", "FitGirl", "DODI", "xatab", "KaOs", "IGG", "Masquerade",

        // Common crack files
        "3dmgame.dll", "ALI213.dll", "crack", "crack.exe", "Crack.nfo",
        "crackfix", "CrackOnly", "fix.exe", "gamefix.dll", "SKIDROW.ini",
        "nosTEAM", "NoSteam", "FCKDRM", "Goldberg", "VALVEEMPRESS",
    ];

    private static string DetectStorefront()
    {
        var dir = Directory.GetCurrentDirectory();
        var store = "Unknown";

        if (File.Exists(Path.Combine(dir, "steam_api.dll")) ||
            File.Exists(Path.Combine(dir, "steam_api64.dll")) ||
            File.Exists(Path.Combine(dir, "steam_appid.txt")) ||
            Directory.Exists(Path.Combine(dir, "steam_settings")))
            store = "Steam";
        else if (Directory.GetFiles(dir, "goggame-*.info").Any() ||
                 File.Exists(Path.Combine(dir, "galaxy.dll")))
            store = "GOG";
        else if (File.Exists(Path.Combine(dir, "EOSSDK-Win64-Shipping.dll")) ||
                 File.Exists(Path.Combine(dir, "EpicOnlineServices.dll")) ||
                 Directory.Exists(Path.Combine(dir, ".egstore")))
            store = "Epic";
        else if (IsProcessRunning("steam")) store = "Steam (process only)";
        else if (IsProcessRunning("GalaxyClient")) store = "GOG (process only)";
        else if (IsProcessRunning("EpicGamesLauncher")) store = "Epic (process only)";

        var isPirated = PiracyFiles.Any(pirate =>
            File.Exists(Path.Combine(dir, pirate)) ||
            Directory.Exists(Path.Combine(dir, pirate))
        );

        if (isPirated)
            store += " + Possible Pirated/Cracked Files Found!";

        return store;
    }

    private static bool IsProcessRunning(string name) =>
        Process.GetProcessesByName(name).Length > 0;
}
