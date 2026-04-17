namespace TheSeedEqualizer;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    // Section names. Sections render in CM in the order their first Config.Bind call runs,
    // so Advanced is bound first. Legacy "0X. Name" headers get rewritten to these by
    // MigrateRenamedSections() on first launch so existing user customisations are preserved.
    private const string AdvancedSection       = "── 1. Advanced ──";
    private const string PlayerGardensSection  = "── 2. Player Gardens ──";
    private const string ZombieGardensSection  = "── 3. Zombie Gardens ──";
    private const string RefugeeGardensSection = "── 4. Refugee Gardens ──";
    private const string WasteSection          = "── 5. Waste ──";
    private const string AllGardensSection     = "── 6. All Gardens ──";
    private const string UpdatesSection        = "── 7. Updates ──";

    private static readonly Dictionary<string, string> SectionRenames = new()
    {
        ["00. Advanced"]        = AdvancedSection,
        ["01. Zombie Gardens"]  = ZombieGardensSection,
        ["02. Player Gardens"]  = PlayerGardensSection,
        ["03. Refugee Gardens"] = RefugeeGardensSection,
        ["04. Waste"]           = WasteSection,
        ["05. All Gardens"]     = AllGardensSection,
    };

    internal static ManualLogSource Log { get; private set; }

    internal static ConfigEntry<bool> Debug { get; private set; }
    internal static bool DebugEnabled;
    internal static bool DebugDialogShown;

    internal static ConfigEntry<bool> ModifyPlayerGardens { get; private set; }
    internal static ConfigEntry<bool> ModifyZombieGardens { get; private set; }
    internal static ConfigEntry<bool> ModifyZombieVineyards { get; private set; }
    internal static ConfigEntry<bool> ModifyRefugeeGardens { get; private set; }
    internal static ConfigEntry<bool> AddWasteToZombieGardens { get; private set; }
    internal static ConfigEntry<bool> AddWasteToZombieVineyards { get; private set; }
    internal static ConfigEntry<bool> BoostPotentialSeedOutput { get; private set; }
    internal static ConfigEntry<bool> BoostGrowSpeedWhenRaining { get; private set; }
    internal static ConfigEntry<bool> CheckForUpdates { get; private set; }

    private void Awake()
    {
        Log = Logger;
        LogHelper.Log = Logger;
        MigrateRenamedSections();
        InitConfiguration();
        Lang.Init(Assembly.GetExecutingAssembly(), Log);
        UpdateChecker.Register(Info, CheckForUpdates);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), MyPluginInfo.PLUGIN_GUID);
    }

    // Rewrites old "[0X. Name]" headers to the "[── N. Name ──]" form so existing
    // user values survive the section rename. Idempotent — once migrated there are
    // no old headers left for the next launch to match.
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
            new ConfigDescription("Write detailed diagnostic info to the BepInEx log while you play. Turn this on before reporting a bug so the log has the context I need to help.", null,
                new ConfigurationManagerAttributes {Order = 100}));
        DebugEnabled = Debug.Value;
        Debug.SettingChanged += (_, _) => DebugEnabled = Debug.Value;

        // ── 2. Player Gardens ──
        ModifyPlayerGardens = Config.Bind(PlayerGardensSection, "Modify Player Gardens", false,
            new ConfigDescription("On: your own garden plots drop a fairer amount of seeds — at minimum the same number you planted, plus a bonus. Off: vanilla seed yields apply to your gardens.", null,
                new ConfigurationManagerAttributes {Order = 100}));

        // ── 3. Zombie Gardens ──
        ModifyZombieGardens = Config.Bind(ZombieGardensSection, "Modify Zombie Gardens", true,
            new ConfigDescription("On: zombie garden beds return at least as many seeds as you fed in, plus a bonus, so they don't bleed your seed stock dry. Off: vanilla zombie garden yields apply.", null,
                new ConfigurationManagerAttributes {Order = 100}));

        ModifyZombieVineyards = Config.Bind(ZombieGardensSection, "Modify Zombie Vineyards", true,
            new ConfigDescription("On: zombie vineyards return at least as many seeds as you fed in, plus a bonus. Off: vanilla zombie vineyard yields apply.", null,
                new ConfigurationManagerAttributes {Order = 99}));

        // ── 4. Refugee Gardens ──
        ModifyRefugeeGardens = Config.Bind(RefugeeGardensSection, "Modify Refugee Gardens", true,
            new ConfigDescription("On: refugee garden plots return at least as many seeds as you fed in, plus a bonus. Off: vanilla refugee garden yields apply.", null,
                new ConfigurationManagerAttributes {Order = 100}));

        // ── 5. Waste ──
        AddWasteToZombieGardens = Config.Bind(WasteSection, "Add Waste To Zombie Gardens", true,
            new ConfigDescription("On: zombie garden harvests also drop 3–5 crop waste, which is otherwise hard to come by. Off: zombie gardens drop only their normal output.", null,
                new ConfigurationManagerAttributes {Order = 100}));

        AddWasteToZombieVineyards = Config.Bind(WasteSection, "Add Waste To Zombie Vineyards", true,
            new ConfigDescription("On: zombie vineyards also drop 3–5 crop waste per harvest. Off: zombie vineyards drop only their normal output.", null,
                new ConfigurationManagerAttributes {Order = 99}));

        // ── 6. All Gardens ──
        BoostPotentialSeedOutput = Config.Bind(AllGardensSection, "Boost Potential Seed Output", true,
            new ConfigDescription("On: lifts the upper end of every modified garden's seed roll by an extra 2 (so the random max is +4 instead of +2). Off: keeps the smaller +2 bonus.", null,
                new ConfigurationManagerAttributes {Order = 100}));

        BoostGrowSpeedWhenRaining = Config.Bind(AllGardensSection, "Boost Grow Speed When Raining", true,
            new ConfigDescription("On: garden, vineyard and refugee planting crafts grow twice as fast while it's raining. Off: rain has no effect on garden growth speed.", null,
                new ConfigurationManagerAttributes {Order = 99}));

        // ── 7. Updates ──
        CheckForUpdates = Config.Bind(UpdatesSection, "Check for Updates", true,
            new ConfigDescription(
                "Show a notice on the main menu when a newer version of this mod is available on NexusMods. Click the notice to open the mod's page.",
                null,
                new ConfigurationManagerAttributes {Order = 100}));
    }

    internal static void ShowDebugWarningOnce()
    {
        if (!DebugEnabled || DebugDialogShown) return;
        DebugDialogShown = true;
        Lang.Reload();
        GUIElements.me.dialog.OpenOK(MyPluginInfo.PLUGIN_NAME, null, Lang.Get("DebugWarning"), true, string.Empty);
    }
}
