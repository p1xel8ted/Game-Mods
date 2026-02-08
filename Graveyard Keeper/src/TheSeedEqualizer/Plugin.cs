namespace TheSeedEqualizer;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.theseedequalizer";
    private const string PluginName = "The Seed Equalizer!";
    private const string PluginVer = "1.3.8";

    internal static ManualLogSource Log { get; private set; }

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
        InitConfiguration();
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        StartupLogger.PrintModLoaded(PluginName, Log);
    }

    private void InitConfiguration()
    {
        ModifyZombieGardens = Config.Bind("01. Zombie Gardens", "Modify Zombie Gardens", true, new ConfigDescription("Enable or disable modifying zombie gardens", null, new ConfigurationManagerAttributes {Order = 20}));
        ModifyZombieVineyards = Config.Bind("01. Zombie Gardens", "Modify Zombie Vineyards", true, new ConfigDescription("Enable or disable modifying zombie vineyards", null, new ConfigurationManagerAttributes {Order = 19}));
        ModifyPlayerGardens = Config.Bind("02. Player Gardens", "Modify Player Gardens", false, new ConfigDescription("Enable or disable modifying player gardens", null, new ConfigurationManagerAttributes {Order = 18}));
        ModifyRefugeeGardens = Config.Bind("03. Refugee Gardens", "Modify Refugee Gardens", true, new ConfigDescription("Enable or disable modifying refugee gardens", null, new ConfigurationManagerAttributes {Order = 17}));
        AddWasteToZombieGardens = Config.Bind("04. Waste", "Add Waste To Zombie Gardens", true, new ConfigDescription("Enable or disable adding waste to zombie gardens", null, new ConfigurationManagerAttributes {Order = 16}));
        AddWasteToZombieVineyards = Config.Bind("04. Waste", "Add Waste To Zombie Vineyards", true, new ConfigDescription("Enable or disable adding waste to zombie vineyards", null, new ConfigurationManagerAttributes {Order = 15}));
        BoostPotentialSeedOutput = Config.Bind("05. All Gardens", "Boost Potential Seed Output", true, new ConfigDescription("Enable or disable boosting potential seed output", null, new ConfigurationManagerAttributes {Order = 14}));
        BoostGrowSpeedWhenRaining = Config.Bind("05. All Gardens", "Boost Grow Speed When Raining", true, new ConfigDescription("Enable or disable boosting grow speed when raining", null, new ConfigurationManagerAttributes {Order = 13}));
    }
}