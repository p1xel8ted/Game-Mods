using System;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using GYKHelper;
using HarmonyLib;

namespace TheSeedEqualizer;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("p1xel8ted.gyk.gykhelper", "3.0.1")]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.theseedequalizer";
    private const string PluginName = "The Seed Equalizer!";
    private const string PluginVer = "1.3.5";

    internal static ManualLogSource Log { get; private set; }
    private static Harmony Harmony { get; set; }

    private static ConfigEntry<bool> ModEnabled { get; set; }
    internal static ConfigEntry<bool> ModifyZombieGardens { get; private set; }
    internal static ConfigEntry<bool> ModifyZombieVineyards { get; private set; }
    internal static ConfigEntry<bool> ModifyPlayerGardens { get; private set; }
    internal static ConfigEntry<bool> ModifyRefugeeGardens { get; private set; }
    internal static ConfigEntry<bool> AddWasteToZombieGardens { get; private set; }
    internal static ConfigEntry<bool> AddWasteToZombieVineyards { get; private set; }
    internal static ConfigEntry<bool> BoostPotentialSeedOutput { get; private set; }
    internal static ConfigEntry<bool> BoostGrowSpeedWhenRaining { get; private set; }

    private void Awake()
    {
        Log = Logger;
        Harmony = new Harmony(PluginGuid);

        InitConfiguration();

        ApplyPatches(this, null);
    }

    private void InitConfiguration()
    {
        ModEnabled = Config.Bind("1. General", "Enabled", true, new ConfigDescription($"Enable or disable {PluginName}", null, new ConfigurationManagerAttributes {Order = 21}));
        ModEnabled.SettingChanged += ApplyPatches;

        ModifyZombieGardens = Config.Bind("2. Gardens", "Modify Zombie Gardens", true, new ConfigDescription("Enable or disable modifying zombie gardens", null, new ConfigurationManagerAttributes {Order = 20}));
        ModifyZombieVineyards = Config.Bind("3. Gardens", "Modify Zombie Vineyards", true, new ConfigDescription("Enable or disable modifying zombie vineyards", null, new ConfigurationManagerAttributes {Order = 19}));
        ModifyPlayerGardens = Config.Bind("4. Gardens", "Modify Player Gardens", false, new ConfigDescription("Enable or disable modifying player gardens", null, new ConfigurationManagerAttributes {Order = 18}));
        ModifyRefugeeGardens = Config.Bind("5. Gardens", "Modify Refugee Gardens", true, new ConfigDescription("Enable or disable modifying refugee gardens", null, new ConfigurationManagerAttributes {Order = 17}));
        AddWasteToZombieGardens = Config.Bind("6. Gardens", "Add Waste To Zombie Gardens", true, new ConfigDescription("Enable or disable adding waste to zombie gardens", null, new ConfigurationManagerAttributes {Order = 16}));
        AddWasteToZombieVineyards = Config.Bind("7. Gardens", "Add Waste To Zombie Vineyards", true, new ConfigDescription("Enable or disable adding waste to zombie vineyards", null, new ConfigurationManagerAttributes {Order = 15}));
        BoostPotentialSeedOutput = Config.Bind("8. Gardens", "Boost Potential Seed Output", true, new ConfigDescription("Enable or disable boosting potential seed output", null, new ConfigurationManagerAttributes {Order = 14}));
        BoostGrowSpeedWhenRaining = Config.Bind("9. Gardens", "Boost Grow Speed When Raining", true, new ConfigDescription("Enable or disable boosting grow speed when raining", null, new ConfigurationManagerAttributes {Order = 13}));
    }
        
    private static void ApplyPatches(object sender, EventArgs eventArgs)
    {
        if (ModEnabled.Value)
        {
            Actions.GameBalanceLoad += Helpers.GameBalancePostfix;
            Log.LogInfo($"Applying patches for {PluginName}");
            Harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
        else
        {
            Actions.GameBalanceLoad -= Helpers.GameBalancePostfix;
            Log.LogInfo($"Removing patches for {PluginName}");
            Harmony.UnpatchSelf();
        }
    }
}