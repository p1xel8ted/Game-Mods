namespace ThoughtfulReminders;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.thoughtfulreminders";
    private const string PluginName = "Thoughtful Reminders";
    private const string PluginVer = "2.2.11";

    private const string AdvancedSection  = "── 1. Advanced ──";
    private const string RemindersSection = "── 2. Reminders ──";

    private static readonly Dictionary<string, string> SectionRenames = new()
    {
        ["01. General"] = RemindersSection,
    };

    internal static ConfigEntry<bool> Debug { get; private set; }
    internal static bool DebugEnabled;
    internal static bool DebugDialogShown;

    internal static ConfigEntry<bool> EnableEventMessages { get; private set; }
    internal static ConfigEntry<bool> DaysOnlyConfig { get; private set; }
    internal static ConfigEntry<bool> SpeechBubblesConfig { get; private set; }

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
    }
}
