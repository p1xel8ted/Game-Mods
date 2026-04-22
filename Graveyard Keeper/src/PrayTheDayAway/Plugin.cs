namespace PrayTheDayAway;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private const string AdvancedSection      = "── Advanced ──";
    private const string GeneralSection       = "── General ──";
    private const string ModeSection          = "── Mode ──";
    private const string NotificationsSection = "── Notifications ──";
    private const string UpgradesSection      = "── Upgrades ──";
    private const string SpeedSection         = "── Speed ──";
    private const string CheatsSection        = "── Cheats ──";
    private const string UpdatesSection       = "── Updates ──";

    private static readonly Dictionary<string, string> SectionRenames = new()
    {
        ["00. Advanced"]      = AdvancedSection,
        ["01. General"]       = GeneralSection,
        ["02. Mode"]          = ModeSection,
        ["03. Notifications"] = NotificationsSection,
        ["04. Upgrades"]      = UpgradesSection,
        ["04. Speed"]         = SpeedSection,
        ["05. Cheats"]        = CheatsSection,
    };

    internal static ConfigEntry<bool> Debug { get; private set; }
    internal static bool DebugEnabled;
    internal static ManualLogSource Log { get; private set; }

    internal static ConfigEntry<bool> EverydayIsSermonDay { get; private set; }
    internal static ConfigEntry<bool> SermonOverAndOver { get; private set; }
    internal static ConfigEntry<bool> NotifyOnPrayerLoss { get; private set; }
    internal static ConfigEntry<bool> AlternateMode { get; private set; }
    internal static ConfigEntry<bool> NoLossOnDailySermons { get; private set; }
    internal static ConfigEntry<bool> RandomlyUpgradeBasicPrayer { get; private set; }
    internal static ConfigEntry<bool> SpeedUpSermon { get; private set; }
    internal static ConfigEntry<int> SermonSpeed { get; private set; }
    internal static ConfigEntry<bool> CheatModeConfig { get; private set; }
    internal static ConfigEntry<bool> CheckForUpdates { get; private set; }

    private void Awake()
    {
        Log = Logger;
        LogHelper.Log = Logger;
        MigrateRenamedSections();
        InitConfiguration();
        Lang.Init(Assembly.GetExecutingAssembly(), Log);
        UpdateChecker.Register(Info, CheckForUpdates);
        DebugWarningDialog.Register(MyPluginInfo.PLUGIN_NAME, () => DebugEnabled);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), MyPluginInfo.PLUGIN_GUID);
    }

    // Rewrites old numbered section headers to the new "── Name ──" style so existing
    // user values survive the rename. Idempotent.
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

        Log.LogInfo($"[Migration] Renamed {renamed} legacy config section header(s) to the '── Name ──' style. Existing user values preserved.");
        Config.Reload();
    }

    private void InitConfiguration()
    {
        Debug = Config.Bind(AdvancedSection, "Debug Logging", false,
            new ConfigDescription("Write verbose sermon/prayer diagnostics to the BepInEx console. Leave off for normal play.", null,
                new ConfigurationManagerAttributes {Order = 597}));
        DebugEnabled = Debug.Value;
        Debug.SettingChanged += (_, _) => DebugEnabled = Debug.Value;

        EverydayIsSermonDay = Config.Bind(GeneralSection, "Everyday Is Sermon Day", true,
            new ConfigDescription("Sermons can be held every day instead of only on the vanilla sermon day.", null,
                new ConfigurationManagerAttributes {Order = 606}));

        SermonOverAndOver = Config.Bind(GeneralSection, "Sermon Over And Over", false,
            new ConfigDescription("Repeat the same sermon multiple times in one day without the usual once-per-day cooldown.", null,
                new ConfigurationManagerAttributes {Order = 605}));

        AlternateMode = Config.Bind(ModeSection, "Alternate Mode", true,
            new ConfigDescription("Instead of a chance to destroy a prayer item on use, there's a chance to just downgrade it one tier. Softer loss, keeps your prayers usable.", null,
                new ConfigurationManagerAttributes {Order = 603}));

        NoLossOnDailySermons = Config.Bind(ModeSection, "No Loss On Daily Sermons", false,
            new ConfigDescription("Prayer items used on the bonus daily sermons never get lost or downgraded. Only applies when Everyday Is Sermon Day is on.", null,
                new ConfigurationManagerAttributes {Order = 602, DispName = "    └ No Loss On Daily Sermons"}));

        NotifyOnPrayerLoss = Config.Bind(NotificationsSection, "Notify On Prayer Loss", true,
            new ConfigDescription("Pop up a message when a prayer item gets lost or downgraded, so you notice it happened.", null,
                new ConfigurationManagerAttributes {Order = 602}));

        RandomlyUpgradeBasicPrayer = Config.Bind(UpgradesSection, "Randomly Upgrade Basic Prayer", true,
            new ConfigDescription("Give basic prayers a small chance to upgrade to a known starred version when used.", null,
                new ConfigurationManagerAttributes {Order = 601}));

        SpeedUpSermon = Config.Bind(SpeedSection, "Speed Up Sermon", false,
            new ConfigDescription("Fast-forward the sermon sequence. Note: everything in the world runs faster while the sermon is playing out.", null,
                new ConfigurationManagerAttributes {Order = 600}));

        SermonSpeed = Config.Bind(SpeedSection, "Sermon Speed", 5,
            new ConfigDescription("How much faster the sermon plays. Default game speed is 1 — higher values are more aggressive.",
                new AcceptableValueRange<int>(2, 10),
                new ConfigurationManagerAttributes {Order = 599, DispName = "    └ Sermon Speed"}));

        CheatModeConfig = Config.Bind(CheatsSection, "Cheat Mode", false,
            new ConfigDescription("All sermons always succeed and prayer items are never consumed. Overrides every other setting.", null,
                new ConfigurationManagerAttributes {Order = 598}));

        CheckForUpdates = Config.Bind(UpdatesSection, "Check for Updates", true,
            new ConfigDescription(
                "Show a notice on the main menu when a newer version of this mod is available on NexusMods. Click the notice to open the mod's page.",
                null,
                new ConfigurationManagerAttributes { Order = 0 }));
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
}
