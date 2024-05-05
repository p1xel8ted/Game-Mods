using System;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using GYKHelper;
using HarmonyLib;

namespace FasterCraftReloaded;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("p1xel8ted.gyk.gykhelper", "3.0.1")]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.fastercraftreloaded";
    private const string PluginName = "FasterCraft Reloaded";
    private const string PluginVer = "1.4.5";

    private static ConfigEntry<bool> ModEnabled { get; set; }
    internal static ConfigEntry<bool> Debug { get; set; }
    internal static ConfigEntry<bool> IncreaseBuildAndDestroySpeed { get; private set; }
    internal static ConfigEntry<float> BuildAndDestroySpeed { get; private set; }
    internal static ConfigEntry<float> CraftSpeedMultiplier { get; private set; }
    internal static ConfigEntry<bool> ModifyPlayerGardenSpeed { get; private set; }
    internal static ConfigEntry<float> PlayerGardenSpeedMultiplier { get; private set; }
    internal static ConfigEntry<bool> ModifyZombieGardenSpeed { get; private set; }
    internal static ConfigEntry<float> ZombieGardenSpeedMultiplier { get; private set; }
    internal static ConfigEntry<bool> ModifyRefugeeGardenSpeed { get; private set; }
    internal static ConfigEntry<float> RefugeeGardenSpeedMultiplier { get; private set; }
    internal static ConfigEntry<bool> ModifyZombieVineyardSpeed { get; private set; }
    internal static ConfigEntry<float> ZombieVineyardSpeedMultiplier { get; private set; }
    internal static ConfigEntry<bool> ModifyZombieSawmillSpeed { get; private set; }
    internal static ConfigEntry<float> ZombieSawmillSpeedMultiplier { get; private set; }
    internal static ConfigEntry<bool> ModifyZombieMinesSpeed { get; private set; }
    internal static ConfigEntry<float> ZombieMinesSpeedMultiplier { get; private set; }
    internal static ConfigEntry<bool> ModifyCompostSpeed { get; private set; }
    internal static ConfigEntry<float> CompostSpeedMultiplier { get; private set; }

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
        ModEnabled = Config.Bind("1. General", "Enabled", true, new ConfigDescription($"Toggle {PluginName}", null, new ConfigurationManagerAttributes {Order = 502}));
        ModEnabled.SettingChanged += ApplyPatches;

        Debug = Config.Bind("2. Advanced", "Debug Logging", false, new ConfigDescription("Toggle debug logging on or off.", null, new ConfigurationManagerAttributes {IsAdvanced = true, Order = 501}));

        CraftSpeedMultiplier = Config.Bind("3. Speed Settings", "Craft Speed Multiplier", 2f, new ConfigDescription("Set the multiplier for crafting speed.", new AcceptableValueRange<float>(1f, 50f), new ConfigurationManagerAttributes {Order = 500}));
        IncreaseBuildAndDestroySpeed = Config.Bind("3. Speed Settings", "Increase Build And Destroy Speed", true, new ConfigDescription("Toggle faster building and destruction speed.", null, new ConfigurationManagerAttributes {Order = 499}));
        BuildAndDestroySpeed = Config.Bind("3. Speed Settings", "Build And Destroy Speed", 4f, new ConfigDescription("Set the multiplier for building and destruction speed.", new AcceptableValueRange<float>(2f, 10f), new ConfigurationManagerAttributes {Order = 498}));
            
        ModifyCompostSpeed = Config.Bind("4. Composting Settings", "Modify Compost Speed", false, new ConfigDescription("Toggle composting speed modification on or off.", null, new ConfigurationManagerAttributes {Order = 497}));
        CompostSpeedMultiplier = Config.Bind("4. Composting Settings", "Compost Speed Multiplier", 2f, new ConfigDescription("Set the multiplier for composting speed.", new AcceptableValueRange<float>(1f, 50f), new ConfigurationManagerAttributes {Order = 496}));
            
        ModifyPlayerGardenSpeed = Config.Bind("5. Garden Settings", "Modify Player Garden Speed", false, new ConfigDescription("Toggle player garden speed modification on or off.", null, new ConfigurationManagerAttributes {Order = 495}));
        PlayerGardenSpeedMultiplier = Config.Bind("5. Garden Settings", "Player Garden Speed Multiplier", 2f, new ConfigDescription("Set the multiplier for player garden speed.", new AcceptableValueRange<float>(1f, 50f), new ConfigurationManagerAttributes {Order = 494}));
        ModifyRefugeeGardenSpeed = Config.Bind("5. Garden Settings", "Modify Refugee Garden Speed", false, new ConfigDescription("Toggle refugee garden speed modification on or off.", null, new ConfigurationManagerAttributes {Order = 493}));
        RefugeeGardenSpeedMultiplier = Config.Bind("5. Garden Settings", "Refugee Garden Speed Multiplier", 2f, new ConfigDescription("Set the multiplier for refugee garden speed.", new AcceptableValueRange<float>(1f, 50f), new ConfigurationManagerAttributes {Order = 492}));
        ModifyZombieGardenSpeed = Config.Bind("5. Garden Settings", "Modify Zombie Garden Speed", false, new ConfigDescription("Toggle zombie garden speed modification on or off.", null, new ConfigurationManagerAttributes {Order = 491}));
        ZombieGardenSpeedMultiplier = Config.Bind("5. Garden Settings", "Zombie Garden Speed Multiplier", 2f, new ConfigDescription("Set the multiplier for zombie garden speed.", new AcceptableValueRange<float>(1f, 50f), new ConfigurationManagerAttributes {Order = 490}));

        ModifyZombieMinesSpeed = Config.Bind("6. Production Settings", "Modify Zombie Mines Speed", false, new ConfigDescription("Toggle zombie mines speed modification on or off.", null, new ConfigurationManagerAttributes {Order = 489}));
        ZombieMinesSpeedMultiplier = Config.Bind("6. Production Settings", "Zombie Mines Speed Multiplier", 2f
            , new ConfigDescription("Set the multiplier for zombie mines speed.", new AcceptableValueRange<float>(1f, 50f), new ConfigurationManagerAttributes {Order = 488}));
        ModifyZombieSawmillSpeed = Config.Bind("6. Production Settings", "Modify Zombie Sawmill Speed", false, new ConfigDescription("Toggle zombie sawmill speed modification on or off.", null, new ConfigurationManagerAttributes {Order = 487}));
        ZombieSawmillSpeedMultiplier = Config.Bind("6. Production Settings", "Zombie Sawmill Speed Multiplier", 2f, new ConfigDescription("Set the multiplier for zombie sawmill speed.", new AcceptableValueRange<float>(1f, 50f), new ConfigurationManagerAttributes {Order = 486}));
        ModifyZombieVineyardSpeed = Config.Bind("6. Production Settings", "Modify Zombie Vineyard Speed", false, new ConfigDescription("Toggle zombie vineyard speed modification on or off.", null, new ConfigurationManagerAttributes {Order = 485}));
        ZombieVineyardSpeedMultiplier = Config.Bind("6. Production Settings", "Zombie Vineyard Speed Multiplier", 2f, new ConfigDescription("Set the multiplier for zombie vineyard speed.", new AcceptableValueRange<float>(1f, 50f), new ConfigurationManagerAttributes {Order = 484}));
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