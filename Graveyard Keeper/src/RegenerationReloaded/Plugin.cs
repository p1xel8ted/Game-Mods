namespace RegenerationReloaded;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private const string RegenerationSection = "── Regeneration ──";
    private const string UpdatesSection      = "── Updates ──";

    private static readonly Dictionary<string, string> SectionRenames = new()
    {
        ["01. Regeneration"] = RegenerationSection,
    };

    private static ManualLogSource Log { get; set; }
    internal static ConfigEntry<bool> ShowRegenUpdates { get; private set; }
    internal static ConfigEntry<float> LifeRegen { get; private set; }
    internal static ConfigEntry<float> EnergyRegen { get; private set; }
    internal static ConfigEntry<float> RegenDelay { get; private set; }
    internal static ConfigEntry<bool> CheckForUpdates { get; private set; }

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
        ShowRegenUpdates = Config.Bind(RegenerationSection, "Show Regeneration Updates", true, new ConfigDescription("Display updates when life and energy regenerate.", null, new ConfigurationManagerAttributes {Order = 4}));
        ShowRegenUpdates.SettingChanged += (_, _) => { Patches.ShowRegenUpdates = ShowRegenUpdates.Value; };
        LifeRegen = Config.Bind(RegenerationSection, "Life Regeneration Rate", 2f, new ConfigDescription("Set the rate at which life regenerates per tick. Set to 0 to disable.", new AcceptableValueRange<float>(0f, 10f), new ConfigurationManagerAttributes {Order = 3}));
        LifeRegen.SettingChanged += (_, _) => { Patches.LifeRegen = LifeRegen.Value; };
        EnergyRegen = Config.Bind(RegenerationSection, "Energy Regeneration Rate", 1f, new ConfigDescription("Set the rate at which energy regenerates per tick. Set to 0 to disable.", new AcceptableValueRange<float>(0f, 10f), new ConfigurationManagerAttributes {Order = 2}));
        EnergyRegen.SettingChanged += (_, _) => { Patches.EnergyRegen = EnergyRegen.Value; };
        RegenDelay = Config.Bind(RegenerationSection, "Regeneration Delay", 5f, new ConfigDescription("Set the delay in seconds between each regeneration tick.", new AcceptableValueRange<float>(0f, 10f), new ConfigurationManagerAttributes {Order = 1}));
        RegenDelay.SettingChanged += (_, _) => { Patches.RegenDelay = RegenDelay.Value; };

        CheckForUpdates = Config.Bind(UpdatesSection, "Check for Updates", true, new ConfigDescription(
            "Show a notice on the main menu when a newer version of this mod is available on NexusMods. Click the notice to open the mod's page.",
            null,
            new ConfigurationManagerAttributes { Order = 0 }));
    }

}