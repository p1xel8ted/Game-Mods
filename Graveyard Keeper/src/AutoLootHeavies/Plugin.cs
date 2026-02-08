namespace AutoLootHeavies;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public partial class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.autolootheavies";
    private const string PluginName = "Auto-Loot Heavies!";
    private const string PluginVer = "3.5.0";

    private static ManualLogSource Log { get; set; }

    private const float EnergyRequirement = 3f;

    private static List<Stockpile> SortedStockpiles { get; } = [];
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



    private void Awake()
    {
        Log = Logger;
        InitConfiguration();
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        StartupLogger.PrintModLoaded(PluginName, Log);
      
    }

    private void InitConfiguration()
    {
        TeleportToDumpSiteWhenAllStockPilesFull = Config.Bind("1. Features", "Teleport To Dump Site When Full", true, new ConfigDescription("Teleport resources to a designated dump site when all stockpiles are full", null, new ConfigurationManagerAttributes {Order = 9}));
        ImmersionMode = Config.Bind("1. Features", "Immersive Mode", true, new ConfigDescription("Disable immersive mode to remove energy requirements for teleportation", null, new ConfigurationManagerAttributes {Order = 8}));

        DesignatedTimberLocation = Config.Bind("2. Locations", "Designated Timber Location", new Vector3(-3712.003f, 6144f, 1294.643f), new ConfigDescription("Set the designated location for dumping excess timber", null, new ConfigurationManagerAttributes {Order = 7}));
        DesignatedOreLocation = Config.Bind("2. Locations", "Designated Ore Location", new Vector3(-3712.003f, 6144f, 1294.643f), new ConfigDescription("Set the designated location for dumping excess ore", null, new ConfigurationManagerAttributes {Order = 6}));
        DesignatedStoneLocation = Config.Bind("2. Locations", "Designated Stone Location", new Vector3(-3712.003f, 6144f, 1294.643f), new ConfigDescription("Set the designated location for dumping excess stone and marble", null, new ConfigurationManagerAttributes {Order = 5}));

        SetTimberLocationKeybind = Config.Bind("3. Keybinds", "Set Timber Location Keybind", new KeyboardShortcut(KeyCode.Alpha7), new ConfigDescription("Define the keybind for setting the Timber Location", null, new ConfigurationManagerAttributes {Order = 4}));
        SetOreLocationKeybind = Config.Bind("3. Keybinds", "Set Ore Location Keybind", new KeyboardShortcut(KeyCode.Alpha8), new ConfigDescription("Define the keybind for setting the Ore Location", null, new ConfigurationManagerAttributes {Order = 3}));
        SetStoneLocationKeybind = Config.Bind("3. Keybinds", "Set Stone Location Keybind", new KeyboardShortcut(KeyCode.Alpha9), new ConfigDescription("Define the keybind for setting the Stone Location", null, new ConfigurationManagerAttributes {Order = 2}));

        Debug = Config.Bind("4. Advanced", "Debug Logging", false, new ConfigDescription("Toggle debug logging on or off", null, new ConfigurationManagerAttributes {IsAdvanced = true, Order = 1}));
    }



}