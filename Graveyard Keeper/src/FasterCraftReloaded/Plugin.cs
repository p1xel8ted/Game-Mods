namespace FasterCraftReloaded;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.fastercraftreloaded";
    private const string PluginName = "FasterCraft Reloaded";
    private const string PluginVer = "1.4.8";
    
    internal static ConfigEntry<bool> Debug { get; private set; }
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

    private void Awake()
    {
        Log = Logger;
        InitConfiguration();
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        StartupLogger.PrintModLoaded(PluginName, Log);
        
    }
    
    private void InitConfiguration()
    {

        Debug = Config.Bind("01. Advanced", "Debug Logging", false, new ConfigDescription("Toggle debug logging on or off.", null, new ConfigurationManagerAttributes {IsAdvanced = true, Order = 501}));

        CraftSpeedMultiplier = Config.Bind("02. Speed Settings", "Craft Speed Multiplier", 2f, new ConfigDescription("Set the multiplier for crafting speed.", new AcceptableValueRange<float>(1f, 50f), new ConfigurationManagerAttributes {Order = 500}));
        IncreaseBuildAndDestroySpeed = Config.Bind("02. Speed Settings", "Increase Build And Destroy Speed", true, new ConfigDescription("Toggle faster building and destruction speed.", null, new ConfigurationManagerAttributes {Order = 499}));
        BuildAndDestroySpeed = Config.Bind("02. Speed Settings", "Build And Destroy Speed", 4f, new ConfigDescription("Set the multiplier for building and destruction speed.", new AcceptableValueRange<float>(2f, 10f), new ConfigurationManagerAttributes {Order = 498}));
            
        ModifyCompostSpeed = Config.Bind("03. Composting Settings", "Modify Compost Speed", false, new ConfigDescription("Toggle composting speed modification on or off.", null, new ConfigurationManagerAttributes {Order = 497}));
        CompostSpeedMultiplier = Config.Bind("03. Composting Settings", "Compost Speed Multiplier", 2f, new ConfigDescription("Set the multiplier for composting speed.", new AcceptableValueRange<float>(1f, 50f), new ConfigurationManagerAttributes {Order = 496}));
            
        ModifyPlayerGardenSpeed = Config.Bind("04. Garden Settings", "Modify Player Garden Speed", false, new ConfigDescription("Toggle player garden speed modification on or off.", null, new ConfigurationManagerAttributes {Order = 495}));
        PlayerGardenSpeedMultiplier = Config.Bind("04. Garden Settings", "Player Garden Speed Multiplier", 2f, new ConfigDescription("Set the multiplier for player garden speed.", new AcceptableValueRange<float>(1f, 50f), new ConfigurationManagerAttributes {Order = 494}));
        ModifyRefugeeGardenSpeed = Config.Bind("04. Garden Settings", "Modify Refugee Garden Speed", false, new ConfigDescription("Toggle refugee garden speed modification on or off.", null, new ConfigurationManagerAttributes {Order = 493}));
        RefugeeGardenSpeedMultiplier = Config.Bind("04. Garden Settings", "Refugee Garden Speed Multiplier", 2f, new ConfigDescription("Set the multiplier for refugee garden speed.", new AcceptableValueRange<float>(1f, 50f), new ConfigurationManagerAttributes {Order = 492}));
        ModifyZombieGardenSpeed = Config.Bind("04. Garden Settings", "Modify Zombie Garden Speed", false, new ConfigDescription("Toggle zombie garden speed modification on or off.", null, new ConfigurationManagerAttributes {Order = 491}));
        ZombieGardenSpeedMultiplier = Config.Bind("04. Garden Settings", "Zombie Garden Speed Multiplier", 2f, new ConfigDescription("Set the multiplier for zombie garden speed.", new AcceptableValueRange<float>(1f, 50f), new ConfigurationManagerAttributes {Order = 490}));

        ModifyZombieMinesSpeed = Config.Bind("05. Production Settings", "Modify Zombie Mines Speed", false, new ConfigDescription("Toggle zombie mines speed modification on or off.", null, new ConfigurationManagerAttributes {Order = 489}));
        ZombieMinesSpeedMultiplier = Config.Bind("05. Production Settings", "Zombie Mines Speed Multiplier", 2f
            , new ConfigDescription("Set the multiplier for zombie mines speed.", new AcceptableValueRange<float>(1f, 50f), new ConfigurationManagerAttributes {Order = 488}));
        ModifyZombieSawmillSpeed = Config.Bind("05. Production Settings", "Modify Zombie Sawmill Speed", false, new ConfigDescription("Toggle zombie sawmill speed modification on or off.", null, new ConfigurationManagerAttributes {Order = 487}));
        ZombieSawmillSpeedMultiplier = Config.Bind("05. Production Settings", "Zombie Sawmill Speed Multiplier", 2f, new ConfigDescription("Set the multiplier for zombie sawmill speed.", new AcceptableValueRange<float>(1f, 50f), new ConfigurationManagerAttributes {Order = 486}));
        ModifyZombieVineyardSpeed = Config.Bind("05. Production Settings", "Modify Zombie Vineyard Speed", false, new ConfigDescription("Toggle zombie vineyard speed modification on or off.", null, new ConfigurationManagerAttributes {Order = 485}));
        ZombieVineyardSpeedMultiplier = Config.Bind("05. Production Settings", "Zombie Vineyard Speed Multiplier", 2f, new ConfigDescription("Set the multiplier for zombie vineyard speed.", new AcceptableValueRange<float>(1f, 50f), new ConfigurationManagerAttributes {Order = 484}));
    }
}