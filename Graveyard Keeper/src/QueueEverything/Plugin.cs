namespace QueueEverything;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.queueeverything";
    private const string PluginName = "Queue Everything!*";
    private const string PluginVer = "2.1.11";

    internal static ConfigEntry<bool> HalfFireRequirements { get; private set; }
    internal static ConfigEntry<bool> AutoMaxMultiQualCrafts { get; private set; }
    internal static ConfigEntry<bool> AutoMaxNormalCrafts { get; private set; }
    internal static ConfigEntry<bool> AutoSelectHighestQualRecipe { get; private set; }
    internal static ConfigEntry<bool> AutoSelectCraftButtonWithController { get; private set; }
    internal static ConfigEntry<bool> MakeEverythingAuto { get; private set; }
    internal static ConfigEntry<bool> MakeHandTasksAuto { get; private set; }
    internal static ConfigEntry<bool> ForceMultiCraft { get; private set; }
    internal static ConfigEntry<float> FcTimeAdjustment { get; private set; }
    internal static ConfigEntry<bool> Debug { get; private set; }
    internal static bool DebugEnabled;
    internal static bool DebugDialogShown;
    internal static ManualLogSource Log { get; private set; }

    internal static readonly List<WorldGameObject> CurrentlyCrafting = [];

    internal static readonly string[] MultiOutCantQueue =
    [
        "chisel_2_2b", "marble_plate_3"
    ];

    internal static readonly string[] UnSafeCraftDefPartials =
    [
        "1_to_2", "2_to_3", "3_to_4", "4_to_5", "rem_grave", "soul_workbench_craft", "burgers_place", "beer_barrels_place", "remove", "refugee", "upgrade", "fountain", "blockage", "obstacle", "builddesk", "fix", "broken", "elevator", "refugee", "Remove"
    ];

    internal static readonly string[] UnSafeCraftObjects =
    [
        "mf_crematorium_corp", "garden_builddesk", "tree_garden_builddesk", "mf_crematorium", "grave_ground",
        "tile_church_semicircle_2floors", "mf_grindstone_1", "zombie_garden_desk_1", "zombie_garden_desk_2", "zombie_garden_desk_3",
        "zombie_vineyard_desk_1", "zombie_vineyard_desk_2", "zombie_vineyard_desk_3", "graveyard_builddesk", "blockage_H_low", "blockage_V_low",
        "blockage_H_high", "blockage_V_high", "wood_obstacle_v", "refugee_camp_garden_bed", "refugee_camp_garden_bed_1", "refugee_camp_garden_bed_2",
        "refugee_camp_garden_bed_3", "carrot_box", "elevator_top", "zombie_crafting_table", "mf_balsamation", "mf_balsamation_1", "mf_balsamation_2",
        "mf_balsamation_3", "blockage_H_high", "soul_workbench_craft", "grow_desk_planting", "grow_vineyard_planting"
    ];

    internal static bool AlreadyRun { get; set; }
    internal static bool CcAlreadyRun { get; set; }
    internal static bool CraftsStarted { get; set; }
    internal static bool ExhaustlessEnabled { get; set; }
    internal static bool FasterCraftEnabled { get; set; }
    internal static bool FasterCraftReloaded { get; set; }
    internal static float TimeAdjustment { get; set; }

    private void Awake()
    {
        Log = Logger;
        LogHelper.Log = Logger;
        InitConfiguration();
        Lang.Init(Assembly.GetExecutingAssembly(), Log);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
    }

    private void InitConfiguration()
    {
        const string fcGuid = "p1xel8ted.gyk.fastercraftreloaded";
        if (Harmony.HasAnyPatches(fcGuid))
        {
            var config = new ConfigFile(Path.Combine(Paths.ConfigPath, $"{fcGuid}.cfg"), true);
            var cg = new ConfigDefinition("3. Speed Settings", "Craft Speed Multiplier");
            FcTimeAdjustment = config.Bind(cg, 2f);
            Log.LogInfo("Loading FasterCraft Reloaded Config");
        }

        Debug = Config.Bind("00. Advanced", "Debug Logging", false, new ConfigDescription("Enable or disable debug logging.", null, new ConfigurationManagerAttributes {Order = 6}));
        DebugEnabled = Debug.Value;
        Debug.SettingChanged += (_, _) => DebugEnabled = Debug.Value;

        HalfFireRequirements = Config.Bind("01. Options", "Half Fire Requirements", true, new ConfigDescription("Reduce fire requirements by 50%.", null, new ConfigurationManagerAttributes {Order = 15}));

        AutoMaxMultiQualCrafts = Config.Bind("01. Options", "Auto Max Multi-Quality Crafts", true, new ConfigDescription("Automatically choose maximum craft amount multi-quality crafts.", null, new ConfigurationManagerAttributes {Order = 14}));

        AutoMaxNormalCrafts = Config.Bind("01. Options", "Auto Max Normal Crafts", false, new ConfigDescription("Automatically choose maximum craft amount for normal crafts.", null, new ConfigurationManagerAttributes {Order = 13}));

        AutoSelectHighestQualRecipe = Config.Bind("01. Options", "Auto Select Highest Quality Recipe", true, new ConfigDescription("Automatically select the highest quality recipe available.", null, new ConfigurationManagerAttributes {Order = 12}));

        AutoSelectCraftButtonWithController = Config.Bind("01. Options", "Auto Select Craft Button With Controller", true, new ConfigDescription("Automatically select the craft button when using a controller.", null, new ConfigurationManagerAttributes {Order = 11}));

        MakeEverythingAuto = Config.Bind("01. Options", "Make Everything Auto", true, new ConfigDescription("Automate all possible crafts.", null, new ConfigurationManagerAttributes {Order = 10}));

        MakeHandTasksAuto = Config.Bind("01. Options", "Make Hand Tasks Auto", false, new ConfigDescription("Automate manual crafts (i.e. cooking table).", null, new ConfigurationManagerAttributes {Order = 9}));

        ForceMultiCraft = Config.Bind("01. Options", "Force Multi Craft", true, new ConfigDescription("Makes almost all crafting items able to be queued.", null, new ConfigurationManagerAttributes {Order = 7}));
    }

    internal static bool IsUnsafeDefinition(CraftDefinition _craftDefinition)
    {
        var zombieCraft = _craftDefinition.craft_in.Any(craftIn => craftIn.Contains("zombie"));
        var refugeeCraft = _craftDefinition.craft_in.Any(craftIn => craftIn.Contains("refugee"));
        var unsafeOne = UnSafeCraftDefPartials.Any(_craftDefinition.id.Contains);
        var unsafeTwo = !_craftDefinition.icon.Contains("fire") && _craftDefinition.craft_in.Any(craftIn => UnSafeCraftObjects.Contains(craftIn));
        var unsafeThree = MultiOutCantQueue.Any(_craftDefinition.id.Contains);

        if (zombieCraft || refugeeCraft || unsafeOne || unsafeTwo || unsafeThree)
        {
            return true;
        }

        return false;
    }

    internal static void WriteLog(string message, bool error = false)
    {
        if (error)
        {
            LogHelper.Error(message);
        }
        else
        {
            LogHelper.Info(message);
        }
    }

    internal static void ShowDebugWarningOnce()
    {
        if (!DebugEnabled || DebugDialogShown) return;
        DebugDialogShown = true;
        Lang.Reload();
        GUIElements.me.dialog.OpenOK(PluginName, null, Lang.Get("DebugWarning"), true, string.Empty);
    }
}
