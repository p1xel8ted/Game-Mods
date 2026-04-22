namespace ThoughtfulReminders;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private const string AdvancedSection  = "── Advanced ──";
    private const string RemindersSection = "── Reminders ──";
    private const string UpdatesSection   = "── Updates ──";

    private static readonly Dictionary<string, string> SectionRenames = new()
    {
        ["01. General"]        = RemindersSection,
        ["── 1. Advanced ──"]  = AdvancedSection,
        ["── 2. Reminders ──"] = RemindersSection,
        ["── 3. Updates ──"]   = UpdatesSection,
    };

    internal static ConfigEntry<bool> Debug { get; private set; }
    internal static bool DebugEnabled;

    internal static ConfigEntry<bool> EnableEventMessages { get; private set; }
    internal static ConfigEntry<bool> DaysOnlyConfig { get; private set; }
    internal static ConfigEntry<bool> SpeechBubblesConfig { get; private set; }
    internal static ConfigEntry<float> WakeUpDelay { get; private set; }
    internal static ConfigEntry<bool> CheckForUpdates { get; private set; }

    internal static ManualLogSource Log { get; private set; }

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

    // Rewrites the legacy "[01. General]" header to the new "[── 2. Reminders ──]" form on
    // first launch of the new version so existing user values survive the section rename.
    // Idempotent — once migrated there are no old headers left to match next launch.
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
        // ── 1. Advanced ──
        Debug = Config.Bind(AdvancedSection, "Debug Logging", false,
            new ConfigDescription("Write detailed diagnostic info to the BepInEx log while you play. Turn this on before reporting a bug so the log has the context I need to help.", null,
                new ConfigurationManagerAttributes {Order = 100}));
        DebugEnabled = Debug.Value;
        Debug.SettingChanged += (_, _) => DebugEnabled = Debug.Value;

        // ── 2. Reminders ──
        EnableEventMessages = Config.Bind(RemindersSection, "Event Messages", true,
            new ConfigDescription("On: reminders include the week's featured event alongside the day name (for example 'Lust day — you could drop by the tavern'). Off: only the day name is shown, so reminders stay lore-light.", null,
                new ConfigurationManagerAttributes {Order = 100}));

        DaysOnlyConfig = Config.Bind(RemindersSection, "Days Only", false,
            new ConfigDescription("Override: always show only the day name, even if Event Messages is turned on above. Useful if you prefer the shortest possible reminders without flipping Event Messages off every time.", null,
                new ConfigurationManagerAttributes {Order = 99}));

        SpeechBubblesConfig = Config.Bind(RemindersSection, "Speech Bubbles", true,
            new ConfigDescription("How reminders are displayed on screen. On: your character says the reminder in a thought-style speech bubble. Off: the reminder floats above your head as a small red label instead.", null,
                new ConfigurationManagerAttributes {Order = 90}));

        WakeUpDelay = Config.Bind(RemindersSection, "Wake-Up Delay", 2f,
            new ConfigDescription("How long to wait (in seconds) before showing a reminder after waking up in bed. Lets the wake-up screen finish fading so the message isn't covered up. Set to 0 for instant reminders.",
                new AcceptableValueRange<float>(0f, 10f),
                new ConfigurationManagerAttributes {Order = 80}));

        // ── 3. Updates ──
        CheckForUpdates = Config.Bind(UpdatesSection, "Check for Updates", true,
            new ConfigDescription(
                "Show a notice on the main menu when a newer version of this mod is available on NexusMods. Click the notice to open the mod's page.",
                null,
                new ConfigurationManagerAttributes {Order = 100}));
    }
}
