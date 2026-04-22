namespace FogBeGone;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private const string GeneralSection = "── General ──";
    private const string UpdatesSection = "── Updates ──";

    private static readonly Dictionary<string, string> SectionRenames = new()
    {
        ["01. General"] = GeneralSection,
    };

    private static ManualLogSource Log { get; set; }

    internal static ConfigEntry<bool> DisableFog { get; private set; }
    internal static ConfigEntry<bool> DisableWind { get; private set; }
    internal static ConfigEntry<bool> DisableRain { get; private set; }
    internal static ConfigEntry<bool> CheckForUpdates { get; private set; }

    internal static bool DisableFogCached;
    internal static bool DisableWindCached;
    internal static bool DisableRainCached;

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
        DisableFog = Config.Bind(GeneralSection, "Disable Fog", true,
            new ConfigDescription("Remove fog from outdoor and indoor weather.", null,
                new ConfigurationManagerAttributes { Order = 3 }));
        DisableWind = Config.Bind(GeneralSection, "Disable Wind", false,
            new ConfigDescription("Remove wind weather effects.", null,
                new ConfigurationManagerAttributes { Order = 2 }));
        DisableRain = Config.Bind(GeneralSection, "Disable Rain", false,
            new ConfigDescription("Remove rain weather effects.", null,
                new ConfigurationManagerAttributes { Order = 1 }));

        DisableFogCached = DisableFog.Value;
        DisableWindCached = DisableWind.Value;
        DisableRainCached = DisableRain.Value;

        DisableFog.SettingChanged += (_, _) => DisableFogCached = DisableFog.Value;
        DisableWind.SettingChanged += (_, _) => DisableWindCached = DisableWind.Value;
        DisableRain.SettingChanged += (_, _) => DisableRainCached = DisableRain.Value;

        CheckForUpdates = Config.Bind(UpdatesSection, "Check for Updates", true,
            new ConfigDescription(
                "Show a notice on the main menu when a newer version of this mod is available on NexusMods. Click the notice to open the mod's page.",
                null,
                new ConfigurationManagerAttributes { Order = 0 }));
    }
}