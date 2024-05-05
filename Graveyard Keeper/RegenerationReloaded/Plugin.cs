using System;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using GYKHelper;
using HarmonyLib;

namespace RegenerationReloaded;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("p1xel8ted.gyk.gykhelper", "3.0.1")]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.regenerationreloaded";
    private const string PluginName = "Regeneration Reloaded";
    private const string PluginVer = "1.1.5";

    private static ManualLogSource Log { get; set; }
    private static Harmony Harmony { get; set; }
    private static ConfigEntry<bool> ModEnabled { get; set; }
    internal static ConfigEntry<bool> ShowRegenUpdates { get; private set; }
    internal static ConfigEntry<float> LifeRegen { get; private set; }
    internal static ConfigEntry<float> EnergyRegen { get; private set; }
    internal static ConfigEntry<float> RegenDelay { get; private set; }

    private void Awake()
    {
        Log = Logger;
        Harmony = new Harmony(PluginGuid);
        InitConfiguration();
        ApplyPatches(this, null);
    }

    private void InitConfiguration()
    {
        ModEnabled = Config.Bind("1. General", "Enabled", true, new ConfigDescription($"Enable or disable {PluginName}", null, new ConfigurationManagerAttributes {Order = 5}));
        ModEnabled.SettingChanged += ApplyPatches;

        ShowRegenUpdates = Config.Bind("2. Regeneration", "Show Regeneration Updates", true, new ConfigDescription("Display updates when life and energy regenerate.", null, new ConfigurationManagerAttributes {Order = 4}));
        ShowRegenUpdates.SettingChanged += (sender, args) => { Patches.ShowRegenUpdates = ShowRegenUpdates.Value; };
        LifeRegen = Config.Bind("2. Regeneration", "Life Regeneration Rate", 2f, new ConfigDescription("Set the rate at which life regenerates per tick.", new AcceptableValueRange<float>(1f, 10f), new ConfigurationManagerAttributes {Order = 3}));
        LifeRegen.SettingChanged += (sender, args) => { Patches.LifeRegen = LifeRegen.Value; };
        EnergyRegen = Config.Bind("2. Regeneration", "Energy Regeneration Rate", 1f, new ConfigDescription("Set the rate at which energy regenerates per tick.", new AcceptableValueRange<float>(1f, 10f), new ConfigurationManagerAttributes {Order = 2}));
        EnergyRegen.SettingChanged += (sender, args) => { Patches.EnergyRegen = EnergyRegen.Value; };
        RegenDelay = Config.Bind("2. Regeneration", "Regeneration Delay", 5f, new ConfigDescription("Set the delay in seconds between each regeneration tick.", new AcceptableValueRange<float>(0f, 10f), new ConfigurationManagerAttributes {Order = 1}));
        RegenDelay.SettingChanged += (sender, args) => { Patches.RegenDelay = RegenDelay.Value; };
    }

    private static void ApplyPatches(object sender, EventArgs eventArgs)
    {
        if (ModEnabled.Value)
        {
            Log.LogInfo($"Applying patches for {PluginName}");
            Harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
        else
        {
            Log.LogInfo($"Removing patches for {PluginName}");
            Harmony.UnpatchSelf();
        }
    }
}