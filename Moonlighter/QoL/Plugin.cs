using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using KeyCode = UnityEngine.KeyCode;

namespace QoL
{
    /// <summary>
    /// This is the main class for the QoL plugin. It initializes the configuration for the plugin and applies Harmony patches.
    /// </summary>
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "p1xel8ted.moonlighter.qol";
        private const string PluginName = "QoL";
        private const string PluginVersion = "0.1.0";

        /// <summary>
        /// Logger used to log messages to the BepInEx log.
        /// </summary>
        public static ManualLogSource LOG { get; set; }

        /// <summary>
        /// Configuration entry for enabling or disabling intro skipping.
        /// </summary>
        internal static ConfigEntry<bool> SkipIntros { get; private set; }

        /// <summary>
        /// Configuration entry for enabling or disabling direct loading into the game.
        /// </summary>
        internal static ConfigEntry<bool> LoadStraightIntoGame { get; private set; }

        /// <summary>
        /// The Awake method is called when the plugin is loaded. It initializes the configuration and applies Harmony patches.
        /// </summary>
        private static ConfigFile ConfigInstance { get; set; }
        
        internal static ConfigEntry<KeyboardShortcut> ConfigRefreshKeybind { get; private set; }

        public void Awake()
        {
            ConfigInstance = Config;
            LOG = Logger;
            
            SkipIntros = Config.Bind("1. General", "Skip Intros", true, new ConfigDescription("Skips the intros and the game-pad recommended screen.", null,new ConfigurationManagerAttributes{Order = 98}));
            LoadStraightIntoGame = Config.Bind("1. General", "Load Straight Into Game", true, new ConfigDescription("Loads straight into the game upon menu load.", null, new ConfigurationManagerAttributes{Order = 97}));
            ConfigRefreshKeybind = Config.Bind("2. Keybinds", "Configuration Refresh Keybind", new KeyboardShortcut(KeyCode.K),
                new ConfigDescription("Select the keybind to refresh the configuration after making changes externally.", null, new ConfigurationManagerAttributes {Order = 96}));

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
            LOG.LogInfo($"Plugin {PluginName} is loaded!");
        }
        
        private void Update()
        {
            if (ConfigRefreshKeybind.Value.IsUp())
            {
                ConfigInstance.Reload();
            }
        }
    }
}