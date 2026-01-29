using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using BepInEx.Logging;
using MonoMod.Utils;
using UnityEngine;

namespace Shared;

public static class Helpers
{
    /// <summary>
    /// Case-insensitive contains check for .NET Framework compatibility.
    /// </summary>
    private static bool ContainsIgnoreCase(this string source, string value)
    {
        return source?.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;
    }

    public static void PrintModLoaded(string plugin, ManualLogSource logger)
    {
        var version = Application.version.Replace("\r", "").Replace("\n", "");
        var buildGuid = Application.buildGUID;
        var platform = PlatformHelper.Current;
        var store = StorefrontDetector.DetectStorefront();

        logger.LogInfo("==========================================");
        logger.LogInfo($"  Plugin Loaded: {plugin}");
        logger.LogInfo($"  Version   : {version} (BuildGUID: {buildGuid})");
        logger.LogInfo($"  Platform  : {platform}");
        logger.LogInfo($"  Storefront: {store}");
        logger.LogInfo("==========================================");
    }

    private static class StorefrontDetector
    {
        // List of piracy marker files and folders (expand as needed)
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


        // Simple main detection method
        public static string DetectStorefront()
        {
            var dir = Directory.GetCurrentDirectory();
            var store = "Unknown";

            // 1. Check for Steam markers
            if (File.Exists(Path.Combine(dir, "steam_api.dll")) ||
                File.Exists(Path.Combine(dir, "steam_api64.dll")) ||
                File.Exists(Path.Combine(dir, "steam_appid.txt")) ||
                Directory.Exists(Path.Combine(dir, "steam_settings")))
                store = "Steam";
            // 2. Check for GOG markers
            else if (Directory.GetFiles(dir, "goggame-*.info").Any() ||
                     File.Exists(Path.Combine(dir, "galaxy.dll")))
                store = "GOG";
            // 3. Check for Epic markers
            else if (File.Exists(Path.Combine(dir, "EOSSDK-Win64-Shipping.dll")) ||
                     File.Exists(Path.Combine(dir, "EpicOnlineServices.dll")) ||
                     Directory.Exists(Path.Combine(dir, ".egstore")))
                store = "Epic";
            // 4. Check for running launcher process as backup
            else if (IsProcessRunning("steam")) store = "Steam (process only)";
            else if (IsProcessRunning("GalaxyClient")) store = "GOG (process only)";
            else if (IsProcessRunning("EpicGamesLauncher")) store = "Epic (process only)";

            // 5. Check for piracy markers
            var isPirated = PiracyFiles.Any(pirate =>
                File.Exists(Path.Combine(dir, pirate)) ||
                Directory.Exists(Path.Combine(dir, pirate))
            );

            // 6. Report
            if (isPirated)
                store += " + Possible Pirated/Cracked Files Found!";

            return store;
        }

        // Helper for process detection
        private static bool IsProcessRunning(string name) =>
            Process.GetProcessesByName(name).Length > 0;
    }

    public static bool IsMultiplierActive(float value)
    {
        return !Mathf.Approximately(value, 1.0f);
    }

    internal static List<Follower> AllFollowers => FollowerManager.Followers.SelectMany(followerList => followerList.Value).ToList();

    public static IEnumerator FilterEnumerator(IEnumerator original, Type[] typesToRemove)
    {
        while (original.MoveNext())
        {
            var current = original.Current;
            if (current != null && !typesToRemove.Contains(current.GetType()))
            {
                yield return current;
            }
        }
    }

    /// <summary>
    /// Logs the call stack methods for debugging purposes.
    /// </summary>
    /// <param name="logger">Logger to output the call stack methods.</param>
    /// <param name="skipFrames">Number of frames to skip (default 3: this method, caller, Harmony internals).</param>
    /// <param name="maxFrames">Maximum frames to search (default 10).</param>
    public static void LogCallStack(ManualLogSource logger, int skipFrames = 3, int maxFrames = 10)
    {
        var stackTrace = new StackTrace(false);
        var frameCount = stackTrace.FrameCount;

        for (var i = skipFrames; i < frameCount && i < maxFrames; i++)
        {
            var method = stackTrace.GetFrame(i)?.GetMethod();
            var declaringType = method?.DeclaringType;
            logger.LogWarning($"[Frame {i}] {declaringType?.FullName}.{method?.Name}");
        }
    }

    /// <summary>
    /// Checks the call stack to see if any of the target types are in the call chain.
    /// Returns the first matching type if found, null otherwise.
    /// </summary>
    /// <param name="targetTypes">Collection of types to search for in the call stack.</param>
    /// <param name="skipFrames">Number of frames to skip (default 3: this method, caller, Harmony internals).</param>
    /// <param name="maxFrames">Maximum frames to search (default 10).</param>
    public static Type GetCallingType(ICollection<Type> targetTypes, int skipFrames = 3, int maxFrames = 10)
    {
        var stackTrace = new StackTrace(false);
        var frameCount = stackTrace.FrameCount;

        for (var i = skipFrames; i < frameCount && i < maxFrames; i++)
        {
            var declaringType = stackTrace.GetFrame(i)?.GetMethod()?.DeclaringType;
            if (declaringType != null && targetTypes.Contains(declaringType))
            {
                return declaringType;
            }
        }

        return null;
    }

    /// <summary>
    /// Checks if any frame in the call stack has a type/method name containing the specified strings.
    /// Useful for matching compiler-generated types like coroutine state machines.
    /// </summary>
    /// <param name="typeNameContains">String to search for in the declaring type's full name.</param>
    /// <param name="methodNameContains">Optional string to search for in the method name.</param>
    /// <param name="skipFrames">Number of frames to skip (default 3: this method, caller, Harmony internals).</param>
    /// <param name="maxFrames">Maximum frames to search (default 10).</param>
    public static bool IsCalledFrom(string typeNameContains, string methodNameContains = null, int skipFrames = 3, int maxFrames = 10)
    {
        var stackTrace = new StackTrace(false);
        var frameCount = stackTrace.FrameCount;

        for (var i = skipFrames; i < frameCount && i < maxFrames; i++)
        {
            var method = stackTrace.GetFrame(i)?.GetMethod();
            var declaringType = method?.DeclaringType;

            if (declaringType == null)
            {
                continue;
            }

            var typeMatches = declaringType.FullName.ContainsIgnoreCase(typeNameContains);
            var methodMatches = methodNameContains == null || method.Name.ContainsIgnoreCase(methodNameContains);

            if (typeMatches && methodMatches)
            {
                return true;
            }
        }

        return false;
    }
}