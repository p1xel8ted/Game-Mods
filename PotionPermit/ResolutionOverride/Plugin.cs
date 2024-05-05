
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace ResolutionOverride
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "p1xel8ted.potionpermit.resoverride";
        private const string PluginName = "Potion Permit ResolutionOverride";
        private const string PluginVersion = "0.1.2";

        private static readonly Harmony Harmony = new(PluginGuid);
        private static ManualLogSource _logger;

        private static ConfigEntry<int> _width;
        private static ConfigEntry<int> _height;
        private static ConfigEntry<int> _refresh;
        public static ConfigEntry<int> FrameRate;

        public static Resolution Resolution = new()
        {
            width = 3440,
            height = 1440,
            refreshRate = 120
        };

        private void Awake()
        {
            _logger = Logger;
            _width = Config.Bind("Resolution", "Width", Display.main.systemWidth);
            _height = Config.Bind("Resolution", "Height", Display.main.systemHeight);
            _refresh = Config.Bind("Resolution", "Refresh", 120);
            FrameRate = Config.Bind("Resolution", "TargetFrameRate", 120, "Don't know if this actually does anything, but the game sets it to 60 by default.");
            Resolution.width = _width.Value;
            Resolution.height = _height.Value;
            Resolution.refreshRate = _refresh.Value;
        }

        private void OnEnable()
        {
            Harmony.PatchAll(Assembly.GetExecutingAssembly());
            L($"Plugin {PluginName} is loaded!");
        }

        private void OnDisable()
        {
            Harmony.UnpatchSelf();
            L($"Plugin {PluginName} is unloaded!");
        }

        private static void L(string message)
        {
            _logger.LogWarning(message);
        }
    }
}