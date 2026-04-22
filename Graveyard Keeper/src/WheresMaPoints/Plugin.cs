namespace WheresMaPoints;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private const string UserInterfaceSection = "── User Interface ──";
    private const string VisualFeedbackSection = "── Visual Feedback ──";
    private const string AudioFeedbackSection = "── Audio Feedback ──";
    private const string UpdatesSection = "── Updates ──";

    private static readonly Dictionary<string, string> SectionRenames = new()
    {
        ["01. User Interface"]  = UserInterfaceSection,
        ["02. Visual Feedback"] = VisualFeedbackSection,
        ["03. Audio Feedback"]  = AudioFeedbackSection,
    };

    internal static ConfigEntry<bool> ShowPointGainAboveKeeper { get; private set; }
    internal static ConfigEntry<bool> StillPlayCollectAudio { get; private set; }
    internal static ConfigEntry<bool> AlwaysShowXpBar { get; private set; }
    internal static ConfigEntry<bool> CheckForUpdates { get; private set; }
    private static ManualLogSource Log { get; set; }

    private void Awake()
    {
        Log = Logger;
        MigrateRenamedSections();
        InitConfiguration();
        UpdateChecker.Register(Info, CheckForUpdates);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), MyPluginInfo.PLUGIN_GUID);
    }

    // Rewrites legacy numbered section headers to the plain "── Name ──" style so existing
    // user values survive the rename. Idempotent.
    private void MigrateRenamedSections()
    {
        var path = Config.ConfigFilePath;
        if (!File.Exists(path)) return;

        string content;
        try { content = File.ReadAllText(path); }
        catch (Exception ex) { Log.LogWarning($"[Migration] Could not read {path}: {ex.Message}"); return; }

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

        try { File.WriteAllText(path, content); }
        catch (Exception ex) { Log.LogWarning($"[Migration] Could not write {path}: {ex.Message}"); return; }

        Log.LogInfo($"[Migration] Renamed {renamed} legacy config section header(s) to the '── Name ──' style. Existing user values preserved.");
        Config.Reload();
    }

    private void InitConfiguration()
    {
        AlwaysShowXpBar = Config.Bind(UserInterfaceSection, "Always Show XP Bar", true, new ConfigDescription("Display the experience bar constantly, even without active experience gain.", null, new ConfigurationManagerAttributes {Order = 6}));
        ShowPointGainAboveKeeper = Config.Bind(VisualFeedbackSection, "Show Point Gain Above Keeper", true, new ConfigDescription("Display the points earned above the keeper's head.", null, new ConfigurationManagerAttributes {Order = 5}));
        StillPlayCollectAudio = Config.Bind(AudioFeedbackSection, "Still Play Collect Audio", false, new ConfigDescription("Keep playing the collect audio when point gain is displayed above the keeper's head.", null, new ConfigurationManagerAttributes {Order = 4}));

        CheckForUpdates = Config.Bind(UpdatesSection, "Check for Updates", true, new ConfigDescription(
            "Show a notice on the main menu when a newer version of this mod is available on NexusMods. Click the notice to open the mod's page.",
            null,
            new ConfigurationManagerAttributes { Order = 0 }));
    }

}