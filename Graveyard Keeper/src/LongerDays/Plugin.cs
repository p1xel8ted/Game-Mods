namespace LongerDays;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private const string DayLengthSection = "── Day Length ──";
    private const string UpdatesSection   = "── Updates ──";

    private static readonly Dictionary<string, string> SectionRenames = new()
    {
        ["01. Day Length"] = DayLengthSection,
    };

    internal const float MadnessSeconds = 1350f;
    internal const float EvenLongerSeconds = 1125f;
    internal const float DoubleLengthSeconds = 900f;
    internal const float DefaultIncreaseSeconds = 675f;

    internal static float Seconds;
    private static ManualLogSource Log { get; set; }
    private static ConfigEntry<float> DayLength { get; set; }
    internal static ConfigEntry<bool> CheckForUpdates { get; private set; }

    private void Awake()
    {
        Log = Logger;
        MigrateRenamedSections();

        DayLength = Config.Bind(DayLengthSection, "Day Length", 675f, new ConfigDescription($"Set the length of a day", new AcceptableValueList<float>(675f, 900f, 1125f, 1350f), new ConfigurationManagerAttributes {Order = 1, CustomDrawer = LengthSlider}));
        Seconds = DayLength.Value;

        CheckForUpdates = Config.Bind(UpdatesSection, "Check for Updates", true, new ConfigDescription(
            "Show a notice on the main menu when a newer version of this mod is available on NexusMods. Click the notice to open the mod's page.",
            null,
            new ConfigurationManagerAttributes { Order = 0 }));

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

    private static void LengthSlider(ConfigEntryBase entry)
    {
        GUILayout.Label($"{Patches.GetTimeMulti()}x", GUILayout.Width(60));
        float[] steps = [675f, 900f, 1125f, 1350f];
        var selectedIndex = Mathf.RoundToInt((DayLength.Value - steps[0]) / (steps[steps.Length - 1] - steps[0]) * (steps.Length - 1));
        var newSelectedIndex = Mathf.RoundToInt(GUILayout.HorizontalSlider(selectedIndex, 0, steps.Length - 1, GUILayout.ExpandWidth(true)));
        if (newSelectedIndex == selectedIndex) return;
        DayLength.Value = steps[newSelectedIndex];
        Seconds = DayLength.Value;
    }

}