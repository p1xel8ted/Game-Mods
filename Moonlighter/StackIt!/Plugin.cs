using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;


namespace StackIt
{
    /// <summary>
    /// This is the main class for the Stack It plugin. It initializes the configuration for the plugin and applies Harmony patches.
    /// </summary>
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "p1xel8ted.moonlighter.stackit";
        private const string PluginName = "StackIt!";
        private const string PluginVersion = "0.1.2";
        private static ConfigFile ConfigInstance { get; set; }

        /// <summary>
        /// Logger used to log messages to the BepInEx log.
        /// </summary>
        public static ManualLogSource LOG { get; private set; }

        /// <summary>
        /// Configuration entry for enabling or disabling the Stack It feature.
        /// </summary>
        internal static ConfigEntry<bool> StackIt { get; private set; }

        /// <summary>
        /// Configuration entry for enabling or disabling custom stack sizes.
        /// </summary>
        internal static ConfigEntry<bool> CustomStackSizes { get; private set; }

        /// <summary>
        /// Configuration entry for setting the maximum stack size.
        /// </summary>
        internal static ConfigEntry<int> MaxStackSize { get; private set; }

        internal static ConfigEntry<bool> Debug { get; private set; }
        private static ConfigEntry<KeyboardShortcut> ConfigRefreshKeybind { get; set; }

        /// <summary>
        /// The Awake method is called when the plugin is loaded. It initializes the configuration and applies Harmony patches.
        /// </summary>
        public void Awake()
        {
            // Initialize logger
            LOG = Logger;
            ConfigInstance = Config;

            // Initialize configuration entries
            Debug = Config.Bind("0. Debug", "Debug", false, new ConfigDescription("Enables debug logging.", null, new ConfigurationManagerAttributes {Order = 99}));

            StackIt = Config.Bind("1. Stack It!", "Stack It!", true, new ConfigDescription("Doubles the max stack size of eligible items.", null, new ConfigurationManagerAttributes {Order = 50}));

            ConfigRefreshKeybind = Config.Bind("1. Keybinds", "Configuration Refresh Keybind", new KeyboardShortcut(KeyCode.K),
                new ConfigDescription("Select the keybind to refresh the configuration after making changes externally.", null, new ConfigurationManagerAttributes {Order = 49}));

            CustomStackSizes = Config.Bind("2. Custom", "Custom Stack Size", false, new ConfigDescription("Allows you to set custom stack sizes for eligible items.", null, new ConfigurationManagerAttributes {Order = 98}));


            MaxStackSize = Config.Bind("2. Custom", "Max Stack Size", 999, new ConfigDescription("The maximum stack size for eligible items.", new AcceptableValueRange<int>(1, 999), new ConfigurationManagerAttributes {Order = 97}));


            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
            LOG.LogInfo($"Plugin {PluginName} is loaded!");
        }

        private void Update()
        {
            if (ConfigRefreshKeybind.Value.IsUp())
            {
                ConfigInstance.Reload();
                if (StackIt.Value)
                {
                    CustomStackSizes.Value = false;
                }
                else if (CustomStackSizes.Value)
                {
                    StackIt.Value = false;
                }

                ItemPatches.ApplyModifications();
            }
        }
    }
}