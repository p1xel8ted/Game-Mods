namespace AlchemyResearchRedux;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private const string AdvancedSection = "── Advanced ──";
    private const string UpdatesSection  = "── Updates ──";

    private static readonly Dictionary<string, string> SectionRenames = new()
    {
        ["00. Advanced"] = AdvancedSection,
    };

    internal static ManualLogSource LOG { get; private set; }
    internal static ConfigEntry<bool> Debug { get; private set; }
    internal static bool DebugEnabled;
    internal static ConfigEntry<bool> CheckForUpdates { get; private set; }

    private void Awake()
    {
        LOG = Logger;
        LogHelper.Log = Logger;
        MigrateRenamedSections();

        Debug = Config.Bind(AdvancedSection, "Debug Logging", false,
            new ConfigDescription("Write verbose alchemy-recipe diagnostics to the BepInEx console. Leave off for normal play.", null,
                new ConfigurationManagerAttributes {Order = 1}));
        DebugEnabled = Debug.Value;
        Debug.SettingChanged += (_, _) => DebugEnabled = Debug.Value;

        DebugWarningDialog.Register(MyPluginInfo.PLUGIN_NAME, () => DebugEnabled);

        CheckForUpdates = Config.Bind(UpdatesSection, "Check for Updates", true,
            new ConfigDescription(
                "Show a notice on the main menu when a newer version of this mod is available on NexusMods. Click the notice to open the mod's page.",
                null,
                new ConfigurationManagerAttributes { Order = 0 }));

        UpdateChecker.Register(Info, CheckForUpdates);
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
            LOG.LogWarning($"[Migration] Could not read {path} for section rename: {ex.Message}");
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
            LOG.LogWarning($"[Migration] Could not write {path} after section rename: {ex.Message}");
            return;
        }

        LOG.LogInfo($"[Migration] Renamed {renamed} legacy config section header(s) to the '── Name ──' style. Existing user values preserved.");
        Config.Reload();
    }
}