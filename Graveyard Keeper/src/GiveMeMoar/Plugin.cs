namespace GiveMeMoar;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.givememoar";
    private const string PluginName = "Give Me Moar!";
    private const string PluginVer = "1.2.14";

    private const string AdvancedSection    = "── 1. Advanced ──";
    private const string MultipliersSection = "── 2. Multipliers ──";
    private const string CategoriesSection  = "── 3. Categories ──";
    private const string CraftingSection    = "── 4. Crafting ──";

    // Maps legacy section headers (from shipped versions) to current headers. Applied once on
    // first launch so existing user values survive the rename. Idempotent.
    private static readonly Dictionary<string, string> SectionRenames = new()
    {
        ["00. Advanced"]         = AdvancedSection,
        ["01. Multipliers"]      = MultipliersSection,
        // "Multiply Sticks" used to live in Miscellaneous; it now belongs in Categories
        // alongside the new per-category toggles.
        ["3. Miscellaneous"]     = CategoriesSection,
        ["── 3. Miscellaneous ──"] = CategoriesSection,
    };

    internal static ConfigEntry<bool> Debug { get; private set; }
    internal static bool DebugEnabled;
    internal static bool DebugDialogShown;

    // ── Multipliers ──
    internal static ConfigEntry<float> ResourceMultiplier { get; private set; }
    internal static ConfigEntry<float> FaithMultiplier { get; private set; }
    internal static ConfigEntry<float> DonationMultiplier { get; private set; }
    internal static ConfigEntry<float> GratitudeMultiplier { get; private set; }
    internal static ConfigEntry<float> SinShardMultiplier { get; private set; }
    internal static ConfigEntry<float> HappinessMultiplier { get; private set; }
    internal static ConfigEntry<float> RedTechPointMultiplier { get; private set; }
    internal static ConfigEntry<float> GreenTechPointMultiplier { get; private set; }
    internal static ConfigEntry<float> BlueTechPointMultiplier { get; private set; }

    // ── Categories ── (gates which item groups the Resource Multiplier touches)
    internal static ConfigEntry<bool> MultiplySticks { get; private set; }
    internal static ConfigEntry<bool> MultiplyCrops { get; private set; }
    internal static ConfigEntry<bool> MultiplySeeds { get; private set; }
    internal static ConfigEntry<bool> MultiplyLogs { get; private set; }
    internal static ConfigEntry<bool> MultiplyOres { get; private set; }
    internal static ConfigEntry<bool> MultiplyBugs { get; private set; }
    internal static ConfigEntry<bool> MultiplyEnemyDrops { get; private set; }
    internal static ConfigEntry<bool> MultiplyMisc { get; private set; }
    internal static ConfigEntry<bool> MultiplyBodyParts { get; private set; }

    // ── Crafting ── (craft-output scaling)
    internal static ConfigEntry<float>  CraftOutputMultiplier { get; private set; }
    internal static ConfigEntry<string> CraftPerStationOverrides { get; private set; }
    internal static ConfigEntry<string> CraftExcludedIds { get; private set; }
    internal static ConfigEntry<bool>   CraftExcludeToolsAndEquipment { get; private set; }
    internal static ConfigEntry<bool>   CraftExcludeProgressionCrafts { get; private set; }

    // Cached parse of CraftPerStationOverrides ("station_id=multi;station_id=multi"). Rebuilt
    // on SettingChanged so the hot path doesn't re-parse the string on every craft.
    internal static Dictionary<string, float> CraftStationOverrideMap { get; private set; } = new();

    internal static ManualLogSource Log { get; private set; }

    private void Awake()
    {
        Log = Logger;
        LogHelper.Log = Logger;
        MigrateRenamedSections();
        InitConfiguration();
        Lang.Init(Assembly.GetExecutingAssembly(), Log);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
    }

    // Rewrites legacy "[01. Multipliers]" style headers to the current "── N. Name ──" style
    // in the .cfg file so existing user values survive. Idempotent — re-running on an already
    // migrated file finds nothing to rename.
    private void MigrateRenamedSections()
    {
        var path = Config.ConfigFilePath;
        if (!File.Exists(path)) return;

        string content;
        try
        {
            content = File.ReadAllText(path);
        }
        catch (Exception ex)
        {
            Log.LogWarning($"[Migration] Could not read {path} for section rename: {ex.Message}");
            return;
        }

        var renamed = 0;
        foreach (var kv in SectionRenames)
        {
            var oldHeader = $"[{kv.Key}]";
            var newHeader = $"[{kv.Value}]";
            if (!content.Contains(oldHeader)) continue;
            content = content.Replace(oldHeader, newHeader);
            renamed++;
        }
        if (renamed == 0) return;

        try
        {
            File.WriteAllText(path, content);
        }
        catch (Exception ex)
        {
            Log.LogWarning($"[Migration] Could not write {path} after section rename: {ex.Message}");
            return;
        }

        Log.LogInfo($"[Migration] Renamed {renamed} legacy config section header(s) to the '── N. Name ──' style. Existing user values preserved.");
        Config.Reload();
    }

    private void InitConfiguration()
    {
        // ── 1. Advanced ──
        Debug = Config.Bind(AdvancedSection, "Debug Logging", false,
            new ConfigDescription(
                "Write detailed diagnostic info to the BepInEx log while you play. Turn this on before reporting a bug so the log has the context I need to help.",
                null,
                new ConfigurationManagerAttributes {Order = 100}));
        DebugEnabled = Debug.Value;
        Debug.SettingChanged += (_, _) => DebugEnabled = Debug.Value;

        // ── 2. Multipliers ──
        ResourceMultiplier = Config.Bind(MultipliersSection, "Resource Multiplier", 1f,
            new ConfigDescription(
                "Multiplies the drop amount for resources you harvest, chop, mine, or collect. Use the toggles in the Categories section to pick which item groups this applies to. Values below 1 reduce drops; 1 is vanilla.",
                new AcceptableValueRange<float>(0.1f, 50f),
                new ConfigurationManagerAttributes {Order = 100}));

        FaithMultiplier = Config.Bind(MultipliersSection, "Faith Multiplier", 1f,
            new ConfigDescription(
                "Multiplies faith earned from sermons at the church pulpit. Higher values let you unlock church upgrades and shop items faster.",
                new AcceptableValueRange<float>(0.1f, 50f),
                new ConfigurationManagerAttributes {Order = 99}));

        DonationMultiplier = Config.Bind(MultipliersSection, "Donation Multiplier", 1f,
            new ConfigDescription(
                "Multiplies coin donations collected from the church after each sermon. Useful if you want sermons to fund your building projects more quickly.",
                new AcceptableValueRange<float>(0.1f, 50f),
                new ConfigurationManagerAttributes {Order = 98}));

        GratitudeMultiplier = Config.Bind(MultipliersSection, "Gratitude Multiplier", 1f,
            new ConfigDescription(
                "Multiplies the gratitude points earned when you release souls at the lighthouse. Higher values speed up your progress through the soul-release tech tree.",
                new AcceptableValueRange<float>(0.1f, 50f),
                new ConfigurationManagerAttributes {Order = 97}));

        SinShardMultiplier = Config.Bind(MultipliersSection, "Sin Shard Multiplier", 1f,
            new ConfigDescription(
                "Multiplies sin shards dropped from corpses during body preparation. Higher values help you cash in at the inquisitor faster.",
                new AcceptableValueRange<float>(0.1f, 50f),
                new ConfigurationManagerAttributes {Order = 96}));

        HappinessMultiplier = Config.Bind(MultipliersSection, "Happiness Multiplier", 1f,
            new ConfigDescription(
                "Multiplies the happiness change applied to refugees in the refugee camp. Values above 1 make the camp reach full happiness quicker; this affects both gains and losses.",
                new AcceptableValueRange<float>(0.1f, 50f),
                new ConfigurationManagerAttributes {Order = 95}));

        RedTechPointMultiplier = Config.Bind(MultipliersSection, "Red Tech Point Multiplier", 1f,
            new ConfigDescription(
                "Multiplies red (crafting) tech points earned from crafts, corpses and other sources. Higher values unlock workbench techs faster.",
                new AcceptableValueRange<float>(0.1f, 50f),
                new ConfigurationManagerAttributes {Order = 94}));

        GreenTechPointMultiplier = Config.Bind(MultipliersSection, "Green Tech Point Multiplier", 1f,
            new ConfigDescription(
                "Multiplies green (nature) tech points earned from gardening, alchemy and other sources. Higher values unlock farming and potion techs faster.",
                new AcceptableValueRange<float>(0.1f, 50f),
                new ConfigurationManagerAttributes {Order = 93}));

        BlueTechPointMultiplier = Config.Bind(MultipliersSection, "Blue Tech Point Multiplier", 1f,
            new ConfigDescription(
                "Multiplies blue (science) tech points earned from study desks, writing and other sources. Higher values unlock study and printing techs faster.",
                new AcceptableValueRange<float>(0.1f, 50f),
                new ConfigurationManagerAttributes {Order = 92}));

        // ── 3. Categories ──
        MultiplySticks = Config.Bind(CategoriesSection, "Multiply Sticks", true,
            new ConfigDescription(
                "On: sticks get multiplied by the Resource Multiplier alongside other resources — great for stocking fires. Off: sticks are excluded and drop at vanilla quantities, useful if high multipliers flood your inventory (deconstructing a garden is the common offender).",
                null,
                new ConfigurationManagerAttributes {Order = 100}));

        MultiplyCrops = Config.Bind(CategoriesSection, "Multiply Crops", true,
            new ConfigDescription(
                "On: harvested crops (wheat, cabbage, carrot, beet, onion, lentils, pumpkin, hops, hemp, grapes, apples, berries, mushrooms, flowers, crop waste, hiccup grass) are multiplied by the Resource Multiplier. Off: crops stay at vanilla amounts.",
                null,
                new ConfigurationManagerAttributes {Order = 99}));

        MultiplySeeds = Config.Bind(CategoriesSection, "Multiply Seeds", false,
            new ConfigDescription(
                "On: seeds you get back from harvesting get multiplied by the Resource Multiplier. Off: seeds stay at vanilla amounts. Off is usually what you want — seeds pile up fast at high multipliers because each harvest already returns more than you planted.",
                null,
                new ConfigurationManagerAttributes {Order = 98}));

        MultiplyLogs = Config.Bind(CategoriesSection, "Multiply Logs", true,
            new ConfigDescription(
                "On: wooden billets, planks, beams and flitches that drop from trees, stumps or deconstructing built objects are multiplied. Off: they stay at vanilla. Sticks are controlled by the Multiply Sticks toggle separately.",
                null,
                new ConfigurationManagerAttributes {Order = 97}));

        MultiplyOres = Config.Bind(CategoriesSection, "Multiply Ores", true,
            new ConfigDescription(
                "On: ore blocks, stone plates, marble plates, clay, coal, sulfur, silver/gold nuggets, graphite, faceted diamonds, sand and lifestone get multiplied. Off: vanilla.",
                null,
                new ConfigurationManagerAttributes {Order = 96}));

        MultiplyBugs = Config.Bind(CategoriesSection, "Multiply Bugs", true,
            new ConfigDescription(
                "On: bees, butterflies, moths and maggots you catch or harvest get multiplied. Off: vanilla.",
                null,
                new ConfigurationManagerAttributes {Order = 95}));

        MultiplyEnemyDrops = Config.Bind(CategoriesSection, "Multiply Enemy Drops", true,
            new ConfigDescription(
                "On: items dropped by enemies — bat wings, slimes, jelly slugs (green/blue/orange/black), spider webs and bloody nails — are multiplied. Off: vanilla.",
                null,
                new ConfigurationManagerAttributes {Order = 94}));

        MultiplyMisc = Config.Bind(CategoriesSection, "Multiply Miscellaneous", false,
            new ConfigDescription(
                "On: honey, beeswax, ash, peat, salt, water and bucket-of-water, alcohol, chicken eggs, jug of milk, and metal scrap get multiplied. Off: vanilla. Off by default because water/alcohol/eggs can quickly unbalance cooking and potion play.",
                null,
                new ConfigurationManagerAttributes {Order = 93}));

        MultiplyBodyParts = Config.Bind(CategoriesSection, "Multiply Body Parts", false,
            new ConfigDescription(
                "On: blood, flesh, fat, skin, bone and skull dropped during body preparation get multiplied. Off: vanilla. Only the listed body parts are touched — organs and other specialised parts are always left alone.",
                null,
                new ConfigurationManagerAttributes {Order = 92}));

        // ── 4. Crafting ──
        CraftOutputMultiplier = Config.Bind(CraftingSection, "Craft Output Multiplier", 1f,
            new ConfigDescription(
                "Multiplies the quantity of items produced by crafts — for example, setting this to 5 makes one log yield five times as many billets at the sawhorse. 1 = vanilla. Research-point outputs (red/green/blue) are never touched; the Tech Point Multipliers handle those separately.",
                new AcceptableValueRange<float>(0.1f, 50f),
                new ConfigurationManagerAttributes {Order = 100}));

        CraftPerStationOverrides = Config.Bind(CraftingSection, "Per-Station Overrides", string.Empty,
            new ConfigDescription(
                "Optional per-station overrides that beat the global Craft Output Multiplier. Format: station_id=multi;station_id=multi — for example 'mf_sawhorse_1=10;mf_anvil_2=3' scales sawhorse crafts by 10× and anvil-II crafts by 3×, while every other station uses the global multiplier.",
                null,
                new ConfigurationManagerAttributes {Order = 99, DispName = "    └ Per-Station Overrides"}));

        CraftExcludedIds = Config.Bind(CraftingSection, "Excluded Craft IDs", string.Empty,
            new ConfigDescription(
                "Optional semicolon-separated list of craft definition IDs to skip on top of the automatic progression deny-list. Useful if you find a specific craft that shouldn't be multiplied (e.g. a DLC recipe). Leave blank unless you know what you're doing.",
                null,
                new ConfigurationManagerAttributes {Order = 90}));

        CraftExcludeToolsAndEquipment = Config.Bind(CraftingSection, "Exclude Tools And Equipment", true,
            new ConfigDescription(
                "On: tools, weapons, armour and sermon scrolls are never multiplied, so one craft still makes one sword. Off: the multiplier applies to these too, which usually produces multiple equipped items per craft (weird but available).",
                null,
                new ConfigurationManagerAttributes {Order = 80}));

        CraftExcludeProgressionCrafts = Config.Bind(CraftingSection, "Exclude Progression Crafts", true,
            new ConfigDescription(
                "On: automatically skips crafts that exist solely to upgrade a station, place an object, repair something, or advance quality tiers (e.g. 'upgrade', '0_to_1'). These would trivialise progression if multiplied. Off: every craft is fair game.",
                null,
                new ConfigurationManagerAttributes {Order = 79}));

        // Craft-output patch reacts to config changes by restoring snapshots and reapplying.
        CraftOutputMultiplier.SettingChanged         += OnCraftOutputSettingChanged;
        CraftPerStationOverrides.SettingChanged      += OnCraftOutputSettingChanged;
        CraftExcludedIds.SettingChanged              += OnCraftOutputSettingChanged;
        CraftExcludeToolsAndEquipment.SettingChanged += OnCraftOutputSettingChanged;
        CraftExcludeProgressionCrafts.SettingChanged += OnCraftOutputSettingChanged;

        RebuildCraftStationOverrideMap();
    }

    private static void OnCraftOutputSettingChanged(object sender, EventArgs e)
    {
        RebuildCraftStationOverrideMap();
        Patches.RequestCraftOutputReapply();
    }

    internal static void RebuildCraftStationOverrideMap()
    {
        var map = new Dictionary<string, float>(StringComparer.Ordinal);
        var raw = CraftPerStationOverrides?.Value;
        if (!string.IsNullOrWhiteSpace(raw))
        {
            var pairs = raw.Split(';');
            foreach (var pair in pairs)
            {
                var trimmed = pair.Trim();
                if (trimmed.Length == 0) continue;
                var eq = trimmed.IndexOf('=');
                if (eq <= 0) continue;
                var key = trimmed.Substring(0, eq).Trim();
                var valPart = trimmed.Substring(eq + 1).Trim();
                if (key.Length == 0) continue;
                if (!float.TryParse(valPart, NumberStyles.Float, CultureInfo.InvariantCulture, out var value)) continue;
                if (value <= 0f) continue;
                map[key] = value;
            }
        }
        CraftStationOverrideMap = map;
        if (DebugEnabled)
        {
            Helpers.Log($"[CraftOverrides] rebuilt map, entries={map.Count} raw='{raw}'");
        }
    }
}
