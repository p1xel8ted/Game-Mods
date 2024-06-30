namespace AutoTools;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
[BepInDependency("p1xel8ted.sunhaven.keepalive")]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.sunhaven.autotools";
    private const string PluginName = "Auto Tools";
    private const string PluginVersion = "0.0.7";
    private const string CategoryGeneral = "01. General";
    private const string CategoryFarm = "02. Farm";
    private const string CategoryEnemies = "03. Enemies";
    private const string CategorySpecificTools = "04. Specific Tools";
    private const string CategoryDebug = "05. Debug";
    private static ManualLogSource LOG { get; set; }
    internal static ConfigEntry<bool> EnableAutoTool { get; private set; }
    internal static ConfigEntry<bool> EnableAutoToolOnFarmTiles { get; private set; }
    internal static ConfigEntry<bool> EnableAutoPickaxe { get; private set; }
    internal static ConfigEntry<bool> EnableAutoAxe { get; private set; }
    internal static ConfigEntry<bool> EnableAutoScythe { get; private set; }
    internal static ConfigEntry<bool> EnableAutoFishingRod { get; private set; }
    internal static ConfigEntry<bool> EnableAutoWateringCan { get; private set; }
    internal static ConfigEntry<int> WateringCanFillThreshold { get; private set; }
    internal static ConfigEntry<bool> EnableAutoHoe { get; private set; }
    private static ConfigEntry<bool> EnableDebug { get; set; }
    internal static ConfigEntry<int> FishingRodWaterDetectionDistance { get; private set; }
    internal static ConfigEntry<bool> EnableEnemyDetection { get; private set; }
    internal static ConfigEntry<bool> UseCombatRange { get; private set; }
    internal static ConfigEntry<int> CombatRange { get; private set; }

    private void Awake()
    {
        LOG = new ManualLogSource(PluginName);
        BepInEx.Logging.Logger.Sources.Add(LOG);
        WateringCan.onWateringCanEmpty += FindNextWateringCan;
        WateringCan.onFillUpWateringCan += FillWateringCanProper;

        EnableAutoTool = Config.Bind(CategoryGeneral, "Enable AutoTools", true, new ConfigDescription("Enable AutoTools.", null, new ConfigurationManagerAttributes {Order = 27}));
        EnableAutoToolOnFarmTiles = Config.Bind(CategoryFarm, "Enable AutoTools On Farm Tiles", false, new ConfigDescription("Enable AutoTools On Farm Tiles.", null, new ConfigurationManagerAttributes {Order = 26}));
        EnableEnemyDetection = Config.Bind(CategoryEnemies, "Enable Enemy Detection", true, new ConfigDescription("If enabled, tool switching will be disabled when enemies are in the area.", null, new ConfigurationManagerAttributes {Order = 25}));
        UseCombatRange = Config.Bind(CategoryEnemies, "Use Combat Range", false, new ConfigDescription("Use Combat Range for switching instead of based on area.", null, new ConfigurationManagerAttributes {Order = 25}));
        CombatRange = Config.Bind(CategoryEnemies, "Combat Range", 75, new ConfigDescription("Sets how close enemies can get to you before tool switching is disabled. Regardless of this value, if you're in combat, it won't switch tools.", new AcceptableValueRange<int>(5, 100), new ConfigurationManagerAttributes {Order = 25}));
        EnableAutoPickaxe = Config.Bind(CategorySpecificTools, "Enable AutoPickaxe", true, new ConfigDescription("Enable AutoPickaxe.", null, new ConfigurationManagerAttributes {Order = 24}));
        EnableAutoAxe = Config.Bind(CategorySpecificTools, "Enable AutoAxe", true, new ConfigDescription("Enable AutoAxe.", null, new ConfigurationManagerAttributes {Order = 23}));
        EnableAutoScythe = Config.Bind(CategorySpecificTools, "Enable AutoScythe", true, new ConfigDescription("Enable AutoScythe.", null, new ConfigurationManagerAttributes {Order = 22}));
        EnableAutoFishingRod = Config.Bind(CategorySpecificTools, "Enable AutoFishingRod", true, new ConfigDescription("Enable AutoFishingRod.", null, new ConfigurationManagerAttributes {Order = 21}));
        FishingRodWaterDetectionDistance = Config.Bind(CategorySpecificTools, "Fishing Rod Water Detection Distance", 5, new ConfigDescription("Control how far away from water you can be to use the fishing rod. Setting too high will cause swaps when you don't want it to...i.e combat near a water source", new AcceptableValueRange<int>(1, 20), new ConfigurationManagerAttributes {Order = 20}));
        EnableAutoWateringCan = Config.Bind(CategorySpecificTools, "Enable AutoWateringCan", true, new ConfigDescription("Enable AutoWateringCan.", null, new ConfigurationManagerAttributes {Order = 19}));
        WateringCanFillThreshold = Config.Bind(CategorySpecificTools, "Watering Can Fill Threshold", 10, new ConfigDescription("How low the water in the water is able to reach before it takes priority over fishing rods at water sources.", new AcceptableValueRange<int>(0, 100), new ConfigurationManagerAttributes {ShowRangeAsPercent = true, Order = 18}));
        EnableAutoHoe = Config.Bind(CategorySpecificTools, "Enable AutoHoe", true, new ConfigDescription("Enable AutoHoe.", null, new ConfigurationManagerAttributes {Order = 17}));
        EnableDebug = Config.Bind(CategoryDebug, "Enable Debug", false, new ConfigDescription("Enable Debug.", null, new ConfigurationManagerAttributes {Order = 16}));

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        LOG.LogInfo($"Plugin {PluginName} is loaded!");
    }
    
    private static void FillWateringCanProper()
    {
        if (!Player.Instance) return;
        if (Player.Instance.CurrentItem is not WateringCanItem item) return;
        var wc = ((WateringCanData) Utils.GetItemData(item.id)).waterCapacity;
        item.WaterAmount = wc;
    }
    
    private void OnDestroy()
    {
        OnDisable();
    }
    
    private void OnDisable()
    {
        WateringCan.onWateringCanEmpty -= FindNextWateringCan;
        WateringCan.onFillUpWateringCan -= FillWateringCanProper;
        LOG.LogError($"Plugin {PluginName} was disabled/destroyed! Unless you are exiting the game, please install Keep Alive! - https://www.nexusmods.com/sunhaven/mods/31");
    }

    
    internal static void DebugLog(string str)
    {
        if (!EnableDebug.Value) return;

        LOG.LogInfo(str);
    }

    private static void FindNextWateringCan()
    {
        Utilities.Notify(Tools.YourWateringCanIsEmpty, Tools.GetBestWateringCanId(), true);
        Tools.FindBestTool(Tools.Tool.WateringCan);
    }
}