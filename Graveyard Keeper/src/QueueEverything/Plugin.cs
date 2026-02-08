namespace QueueEverything;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public partial class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.queueeverything";
    private const string PluginName = "Queue Everything!*";
    private const string PluginVer = "2.1.8";

    private static ConfigEntry<bool> HalfFireRequirements { get; set; }
    private static ConfigEntry<bool> AutoMaxMultiQualCrafts { get; set; }
    private static ConfigEntry<bool> AutoMaxNormalCrafts { get; set; }
    private static ConfigEntry<bool> AutoSelectHighestQualRecipe { get; set; }
    private static ConfigEntry<bool> AutoSelectCraftButtonWithController { get; set; }
    private static ConfigEntry<bool> MakeEverythingAuto { get; set; }
    private static ConfigEntry<bool> MakeHandTasksAuto { get; set; }
    private static ConfigEntry<bool> ForceMultiCraft { get; set; }
    private static ConfigEntry<float> FcTimeAdjustment { get; set; }
    private static ConfigEntry<bool> Debug { get; set; }
    private static ManualLogSource Log { get; set; }


    private void Awake()
    {
        Log = Logger;
        InitConfiguration();
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        StartupLogger.PrintModLoaded(PluginName, Log);
    }

    private void InitConfiguration()
    {
        const string fcGuid = "p1xel8ted.gyk.fastercraftreloaded";
        if (Harmony.HasAnyPatches(fcGuid))
        {
            //get fc config
            var config = new ConfigFile(Path.Combine(Paths.ConfigPath, $"{fcGuid}.cfg"), true);
            var cg = new ConfigDefinition("3. Speed Settings", "Craft Speed Multiplier");
            FcTimeAdjustment = config.Bind(cg, 2f);
            Log.LogInfo("Loading FasterCraft Reloaded Config");
        }
        
        HalfFireRequirements = Config.Bind("01. Options", "Half Fire Requirements", true, new ConfigDescription("Reduce fire requirements by 50%.", null, new ConfigurationManagerAttributes {Order = 15}));

        AutoMaxMultiQualCrafts = Config.Bind("01. Options", "Auto Max Multi-Quality Crafts", true, new ConfigDescription("Automatically choose maximum craft amount multi-quality crafts.", null, new ConfigurationManagerAttributes {Order = 14}));

        AutoMaxNormalCrafts = Config.Bind("01. Options", "Auto Max Normal Crafts", false, new ConfigDescription("Automatically choose maximum craft amount for normal crafts.", null, new ConfigurationManagerAttributes {Order = 13}));

        AutoSelectHighestQualRecipe = Config.Bind("01. Options", "Auto Select Highest Quality Recipe", true, new ConfigDescription("Automatically select the highest quality recipe available.", null, new ConfigurationManagerAttributes {Order = 12}));

        AutoSelectCraftButtonWithController = Config.Bind("01. Options", "Auto Select Craft Button With Controller", true, new ConfigDescription("Automatically select the craft button when using a controller.", null, new ConfigurationManagerAttributes {Order = 11}));

        MakeEverythingAuto = Config.Bind("01. Options", "Make Everything Auto", true, new ConfigDescription("Automate all possible crafts.", null, new ConfigurationManagerAttributes {Order = 10}));
 
        MakeHandTasksAuto = Config.Bind("01. Options", "Make Hand Tasks Auto", false, new ConfigDescription("Automate manual crafts (i.e. cooking table).", null, new ConfigurationManagerAttributes {Order = 9}));

        ForceMultiCraft = Config.Bind("01. Options", "Force Multi Craft", true, new ConfigDescription("Makes almost all crafting items able to be queued.", null, new ConfigurationManagerAttributes {Order = 7}));

        Debug = Config.Bind("00. Advanced", "Debug Logging", false, new ConfigDescription("Enable or disable debug logging.", null, new ConfigurationManagerAttributes {IsAdvanced = true, Order = 6}));
    }

    private void Update()
    {
        if (!MainGame.game_started) return;
        if (!MakeEverythingAuto.Value) return;
        if (CraftsStarted) return;

        foreach (var wgo in MainGame.me.world.GetComponentsInChildren<WorldGameObject>(true))
        {
            if (wgo != null && wgo.components.craft.is_crafting && !wgo.has_linked_worker)
            {
                CurrentlyCrafting.Add(wgo);
            }
        }

        CraftsStarted = true;
    }
}