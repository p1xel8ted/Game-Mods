using System;
using System.Collections.Generic;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using GYKHelper;
using UnityEngine;

namespace AutoLootHeavies;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("p1xel8ted.gyk.gykhelper", "3.0.1")]
public partial class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.autolootheavies";
    private const string PluginName = "Auto-Loot Heavies!";
    private const string PluginVer = "3.4.7";

    private static ManualLogSource Log { get; set; }

    private const float EnergyRequirement = 3f;
    private static Harmony Harmony { get;  set; }
    private static List<Stockpile> SortedStockpiles { get; } = new();
    private static float LastBubbleTime { get;  set; }
    private static List<WorldGameObject> Objects { get; set; }
    private static ConfigEntry<bool> TeleportToDumpSiteWhenAllStockPilesFull { get;  set; }
    private static ConfigEntry<Vector3> DesignatedTimberLocation { get;  set; }
    private static ConfigEntry<Vector3> DesignatedOreLocation { get;  set; }
    private static ConfigEntry<Vector3> DesignatedStoneLocation { get;  set; }
    private static ConfigEntry<bool> ImmersionMode { get;  set; }
    private static ConfigEntry<bool> Debug { get;  set; }
    private static ConfigEntry<KeyboardShortcut> SetTimberLocationKeybind { get;  set; }
    private static ConfigEntry<KeyboardShortcut> SetOreLocationKeybind { get;  set; }
    private static ConfigEntry<KeyboardShortcut> SetStoneLocationKeybind { get;  set; }

    private static ConfigEntry<bool> ModEnabled { get;  set; }

    private void Awake()
    {
        Harmony = new Harmony(PluginGuid);
        Log = Logger;

        InitConfiguration();

        ApplyPatches(this, null);
    }

    private void InitConfiguration()
    {
        ModEnabled = Config.Bind("1. General", "Enabled", true, new ConfigDescription($"Toggle {PluginName}", null, new ConfigurationManagerAttributes {Order = 10}));
        ModEnabled.SettingChanged += ApplyPatches;

        TeleportToDumpSiteWhenAllStockPilesFull = Config.Bind("2. Features", "Teleport To Dump Site When Full", true, new ConfigDescription("Teleport resources to a designated dump site when all stockpiles are full", null, new ConfigurationManagerAttributes {Order = 9}));
        ImmersionMode = Config.Bind("2. Features", "Immersive Mode", true, new ConfigDescription("Disable immersive mode to remove energy requirements for teleportation", null, new ConfigurationManagerAttributes {Order = 8}));

        DesignatedTimberLocation = Config.Bind("3. Locations", "Designated Timber Location", new Vector3(-3712.003f, 6144f, 1294.643f), new ConfigDescription("Set the designated location for dumping excess timber", null, new ConfigurationManagerAttributes {Order = 7}));
        DesignatedOreLocation = Config.Bind("3. Locations", "Designated Ore Location", new Vector3(-3712.003f, 6144f, 1294.643f), new ConfigDescription("Set the designated location for dumping excess ore", null, new ConfigurationManagerAttributes {Order = 6}));
        DesignatedStoneLocation = Config.Bind("3. Locations", "Designated Stone Location", new Vector3(-3712.003f, 6144f, 1294.643f), new ConfigDescription("Set the designated location for dumping excess stone and marble", null, new ConfigurationManagerAttributes {Order = 5}));

        SetTimberLocationKeybind = Config.Bind("4. Keybinds", "Set Timber Location Keybind", new KeyboardShortcut(KeyCode.Alpha7), new ConfigDescription("Define the keybind for setting the Timber Location", null, new ConfigurationManagerAttributes {Order = 4}));
        SetOreLocationKeybind = Config.Bind("4. Keybinds", "Set Ore Location Keybind", new KeyboardShortcut(KeyCode.Alpha8), new ConfigDescription("Define the keybind for setting the Ore Location", null, new ConfigurationManagerAttributes {Order = 3}));
        SetStoneLocationKeybind = Config.Bind("4. Keybinds", "Set Stone Location Keybind", new KeyboardShortcut(KeyCode.Alpha9), new ConfigDescription("Define the keybind for setting the Stone Location", null, new ConfigurationManagerAttributes {Order = 2}));

        Debug = Config.Bind("5. Advanced", "Debug Logging", false, new ConfigDescription("Toggle debug logging on or off", null, new ConfigurationManagerAttributes {IsAdvanced = true, Order = 1}));
    }


    private static void ApplyPatches(object sender, EventArgs eventArgs)
    {
        if (ModEnabled.Value)
        {
            Actions.WorldGameObjectInteract += WorldGameObjectInteract;
            Log.LogInfo($"Applying patches for {PluginName}");
            Harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
        else
        {
            Actions.WorldGameObjectInteract -= WorldGameObjectInteract;
            Log.LogInfo($"Removing patches for {PluginName}");
            Harmony.UnpatchSelf();
        }
    }
}