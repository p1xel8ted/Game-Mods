namespace QueueEverything;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.queueeverything";
    private const string PluginName = "Queue Everything!*";
    private const string PluginVer = "2.1.13";

    private const string AdvancedSection    = "── Advanced ──";
    private const string AutoCraftSection   = "── Auto-Craft ──";
    private const string BalanceSection     = "── Balance ──";
    private const string ConvenienceSection = "── Convenience ──";

    internal static ConfigEntry<bool> EnableAutoCraft { get; private set; }
    internal static Dictionary<CraftCategory, ConfigEntry<bool>> CategoryToggles { get; } = new();

    internal static ConfigEntry<bool> HalfFireRequirements { get; private set; }
    internal static ConfigEntry<bool> HalfCraftOutputs { get; private set; }

    internal static ConfigEntry<bool> AutoMaxMultiQualCrafts { get; private set; }
    internal static ConfigEntry<bool> AutoMaxNormalCrafts { get; private set; }
    internal static ConfigEntry<bool> AutoSelectHighestQualRecipe { get; private set; }
    internal static ConfigEntry<bool> AutoSelectCraftButtonWithController { get; private set; }
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
        "0_to_1", "1_to_2", "2_to_3", "3_to_4", "4_to_5", "upgr_to", "_to_lantern_",
        "rem_grave", "soul_workbench_craft", "burgers_place", "beer_barrels_place",
        "remove", "refugee", "upgrade", "fountain", "blockage", "obstacle",
        "builddesk", "fix", "broken", "elevator", "refugee", "Remove",
        "repair_", "place_tent", "find_zombie"
    ];

    internal static readonly string[] UnSafeCraftObjects =
    [
        "mf_crematorium_corp", "garden_builddesk", "tree_garden_builddesk", "mf_crematorium", "grave_ground",
        "tile_church_semicircle_2floors", "mf_grindstone_1", "zombie_garden_desk_1", "zombie_garden_desk_2", "zombie_garden_desk_3",
        "zombie_vineyard_desk_1", "zombie_vineyard_desk_2", "zombie_vineyard_desk_3", "graveyard_builddesk", "blockage_H_low", "blockage_V_low",
        "blockage_H_high", "blockage_V_high", "wood_obstacle_v", "refugee_camp_garden_bed", "refugee_camp_garden_bed_1", "refugee_camp_garden_bed_2",
        "refugee_camp_garden_bed_3", "carrot_box", "elevator_top", "zombie_crafting_table", "mf_balsamation", "mf_balsamation_1", "mf_balsamation_2",
        "mf_balsamation_3", "soul_workbench_craft", "grow_desk_planting", "grow_vineyard_planting",
        "mf_furnace_0", "mf_furnace_1", "mf_distcube_2_clay",
        "lantern_1_clone", "lantern_2_clone",
        "beegarden_table_broken", "swamp_table_constr", "build_vendor_tent_and_stall", "mill_broken_obj",
        "steep_yellow_blockage_R_o", "steep_marble", "steep_marble_2", "steep_stone",
        "garden_of_stones_place", "lantern_place",
        "crafting_skull", "crafting_skull_2",
        "soul_workbench", "soul_container_place_2", "soul_container_2_place_2", "soul_container_3_place_2",
        "test_obj_2"
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

        Debug = Config.Bind(AdvancedSection, "Debug Logging", false,
            new ConfigDescription("Enable or disable debug logging.", null,
                new ConfigurationManagerAttributes {Order = 10}));
        DebugEnabled = Debug.Value;
        Debug.SettingChanged += (_, _) => DebugEnabled = Debug.Value;

        EnableAutoCraft = Config.Bind(AutoCraftSection, "Enable Auto-Craft", true,
            new ConfigDescription("Master switch for converting hand crafts into queueable auto-crafts. Use the per-workbench toggles below to choose which benches get converted. If this is off, nothing is converted regardless of the child toggles.",
                null, new ConfigurationManagerAttributes {Order = 100}));

        BindCategory(CraftCategory.Alchemy,    "Alchemy",    "Auto-convert alchemy hand crafts at the alchemy workbench, mortar, and survey station.", 99);
        BindCategory(CraftCategory.Cooking,    "Cooking",    "Auto-convert cooking-table and tavern-kitchen crafts.",                                  98);
        BindCategory(CraftCategory.Study,      "Study",      "Auto-convert study desks and book-writing table crafts.",                                97);
        BindCategory(CraftCategory.Metalwork,  "Metalwork",  "Auto-convert anvil, hammer station, and jewelry-bench crafts.",                          96);
        BindCategory(CraftCategory.Morgue,     "Morgue",     "Auto-convert body-preparation, autopsy-table, and packing-table crafts.",                95);
        BindCategory(CraftCategory.Carpentry,  "Carpentry",  "Auto-convert sawhorse, chopping spot, carpenter's workbench, and beam-gantry crafts.",   94);
        BindCategory(CraftCategory.Sermons,    "Sermons",    "Auto-convert sermons at the church pulpit. Note: this removes the energy cost per sermon.", 93);
        BindCategory(CraftCategory.Printing,   "Printing",   "Auto-convert printing press and paper-press crafts.",                                    92);
        BindCategory(CraftCategory.Winemaking, "Winemaking", "Auto-convert vine-press crafts.",                                                        91);
        BindCategory(CraftCategory.Pottery,    "Pottery",    "Auto-convert potter's wheel crafts.",                                                    90);
        BindCategory(CraftCategory.Misc,       "Misc",       "Auto-convert any other hand crafts that don't fall into the categories above (e.g. new benches from a game update).", 89);

        HalfFireRequirements = Config.Bind(BalanceSection, "Half Fire Requirements", true,
            new ConfigDescription("Halve the amount of fuel fires need for their requirements, across every craft that uses fire.",
                null, new ConfigurationManagerAttributes {Order = 100}));

        HalfCraftOutputs = Config.Bind(BalanceSection, "Half Research Point Outputs", true,
            new ConfigDescription("Halves red/green/blue research-point outputs (blood, fat, skin, heart, intestine) on crafts this mod has converted from manual to auto. Vanilla auto-crafts are never touched. Turn this off if you want full yields on auto-crafted recipes.",
                null, new ConfigurationManagerAttributes {Order = 99}));

        AutoMaxMultiQualCrafts = Config.Bind(ConvenienceSection, "Auto Max Multi-Quality Crafts", true,
            new ConfigDescription("Automatically choose maximum craft amount for multi-quality crafts.",
                null, new ConfigurationManagerAttributes {Order = 100}));

        AutoMaxNormalCrafts = Config.Bind(ConvenienceSection, "Auto Max Normal Crafts", false,
            new ConfigDescription("Automatically choose maximum craft amount for normal crafts.",
                null, new ConfigurationManagerAttributes {Order = 99}));

        AutoSelectHighestQualRecipe = Config.Bind(ConvenienceSection, "Auto Select Highest Quality Recipe", true,
            new ConfigDescription("Automatically select the highest quality recipe available.",
                null, new ConfigurationManagerAttributes {Order = 98}));

        AutoSelectCraftButtonWithController = Config.Bind(ConvenienceSection, "Auto Select Craft Button With Controller", true,
            new ConfigDescription("Automatically focus the craft button when using a controller.",
                null, new ConfigurationManagerAttributes {Order = 97}));

        ForceMultiCraft = Config.Bind(ConvenienceSection, "Force Multi Craft", true,
            new ConfigDescription("Makes almost all crafting items able to be queued.",
                null, new ConfigurationManagerAttributes {Order = 96}));

        EnableAutoCraft.SettingChanged      += OnCraftSettingChanged;
        HalfFireRequirements.SettingChanged += OnCraftSettingChanged;
        HalfCraftOutputs.SettingChanged     += OnCraftSettingChanged;
        ForceMultiCraft.SettingChanged      += OnCraftSettingChanged;
        foreach (var entry in CategoryToggles.Values)
        {
            entry.SettingChanged += OnCraftSettingChanged;
        }
        return;

        void BindCategory(CraftCategory category, string label, string description, int order)
        {
            var entry = Config.Bind(AutoCraftSection, label, false,
                new ConfigDescription(description, null,
                    new ConfigurationManagerAttributes {Order = order, DispName = "    └ " + label}));
            CategoryToggles[category] = entry;
        }
    }

    internal static bool IsCategoryEnabled(CraftCategory category)
    {
        if (!EnableAutoCraft.Value)
        {
            return false;
        }

        return CategoryToggles.TryGetValue(category, out var entry) && entry.Value;
    }

    internal static bool AnyAutoCraftCategoryEnabled()
    {
        if (!EnableAutoCraft.Value)
        {
            return false;
        }

        foreach (var entry in CategoryToggles.Values)
        {
            if (entry.Value)
            {
                return true;
            }
        }

        return false;
    }

    private static void OnCraftSettingChanged(object sender, EventArgs e)
    {
        var changedKey = (sender as ConfigEntryBase)?.Definition.Key ?? "unknown";
        if (!CcAlreadyRun || !MainGame.game_started || GameBalance.me == null)
        {
            if (DebugEnabled)
            {
                WriteLog($"[OnCraftSettingChanged] '{changedKey}' changed but mutations not yet applied; deferring until game starts.");
            }
            return;
        }

        if (DebugEnabled)
        {
            WriteLog($"[OnCraftSettingChanged] '{changedKey}' changed — queued for next-frame re-apply.");
        }

        CraftComponentPatches.PendingReapply = true;
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
