using System;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using GYKHelper;
using HarmonyLib;

namespace GiveMeMoar;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("p1xel8ted.gyk.gykhelper", "3.0.1")]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.givememoar";
    private const string PluginName = "Give Me Moar!";
    private const string PluginVer = "1.2.7";

    private static ConfigEntry<bool> ModEnabled { get; set; }
    internal static ConfigEntry<bool> Debug { get; private set; }
    internal static ConfigEntry<bool> MultiplySticks { get; private set; }
    internal static ConfigEntry<float> FaithMultiplier { get; private set; }
    internal static ConfigEntry<float> ResourceMultiplier { get; private set; }
    internal static ConfigEntry<float> GratitudeMultiplier { get; private set; }
    internal static ConfigEntry<float> SinShardMultiplier { get; private set; }
    internal static ConfigEntry<float> DonationMultiplier { get; private set; }
    internal static ConfigEntry<float> BlueTechPointMultiplier { get; private set; }
    internal static ConfigEntry<float> GreenTechPointMultiplier { get; private set; }
    internal static ConfigEntry<float> RedTechPointMultiplier { get; private set; }
    internal static ConfigEntry<float> HappinessMultiplier { get; private set; }
    internal static ManualLogSource Log { get; private set; }
    private static Harmony Harmony { get; set; }


    private void Awake()
    {
        Log = Logger;
        Harmony = new Harmony(PluginGuid);
        InitConfiguration();
        ApplyPatches(this, null);
    }

    private void InitConfiguration()
    {
        ModEnabled = Config.Bind("1. General", "Enabled", true, new ConfigDescription($"Toggle {PluginName}", null, new ConfigurationManagerAttributes {Order = 18}));
        ModEnabled.SettingChanged += ApplyPatches;

        BlueTechPointMultiplier = Config.Bind("2. Multipliers", "Blue Tech Point Multiplier", 1f, new ConfigDescription("Adjust the multiplier for blue tech points", new AcceptableValueRange<float>(1f, 50f), new ConfigurationManagerAttributes {Order = 17}));

        DonationMultiplier = Config.Bind("2. Multipliers", "Donation Multiplier", 1f, new ConfigDescription("Adjust the multiplier for donations", new AcceptableValueRange<float>(1f, 50f), new ConfigurationManagerAttributes {Order = 16}));

        FaithMultiplier = Config.Bind("2. Multipliers", "Faith Multiplier", 1f, new ConfigDescription("Adjust the multiplier for faith", new AcceptableValueRange<float>(1f, 50f), new ConfigurationManagerAttributes {Order = 15}));

        GratitudeMultiplier = Config.Bind("2. Multipliers", "Gratitude Multiplier", 1f, new ConfigDescription("Adjust the multiplier for gratitude", new AcceptableValueRange<float>(1f, 50f), new ConfigurationManagerAttributes {Order = 14}));

        GreenTechPointMultiplier = Config.Bind("2. Multipliers", "Green Tech Point Multiplier", 1f, new ConfigDescription("Adjust the multiplier for green tech points", new AcceptableValueRange<float>(1f, 50f), new ConfigurationManagerAttributes {Order = 13}));

        HappinessMultiplier = Config.Bind("2. Multipliers", "Happiness Multiplier", 1f, new ConfigDescription("Adjust the multiplier for happiness", new AcceptableValueRange<float>(1f, 50f), new ConfigurationManagerAttributes {Order = 12}));

        RedTechPointMultiplier = Config.Bind("2. Multipliers", "Red Tech Point Multiplier", 1f, new ConfigDescription("Adjust the multiplier for red tech points", new AcceptableValueRange<float>(1f, 50f), new ConfigurationManagerAttributes {Order = 11}));

        ResourceMultiplier = Config.Bind("2. Multipliers", "Resource Multiplier", 1f, new ConfigDescription("Adjust the multiplier for resources", new AcceptableValueRange<float>(1f, 50f), new ConfigurationManagerAttributes {Order = 10}));

        SinShardMultiplier = Config.Bind("2. Multipliers", "Sin Shard Multiplier", 1f, new ConfigDescription("Adjust the multiplier for sin shards", new AcceptableValueRange<float>(1f, 50f), new ConfigurationManagerAttributes {Order = 9}));

        MultiplySticks = Config.Bind("3. Miscellaneous", "Multiply Sticks", false, new ConfigDescription("Sticks get multiplied endlessly when used in the garden. Enable this to exclude them.", null, new ConfigurationManagerAttributes {Order = 8}));

        Debug = Config.Bind("4. Advanced", "Debug Logging", false, new ConfigDescription("Toggle debug logging on or off", null, new ConfigurationManagerAttributes {IsAdvanced = true, Order = 7}));
    }


    private static void ApplyPatches(object sender, EventArgs e)
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