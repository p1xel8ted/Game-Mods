namespace LongerDays;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.longerdays";
    private const string PluginName = "Longer Days";
    private const string PluginVer = "1.6.7";

    internal const float MadnessSeconds = 1350f;
    internal const float EvenLongerSeconds = 1125f;
    internal const float DoubleLengthSeconds = 900f;
    internal const float DefaultIncreaseSeconds = 675f;

    internal static float Seconds;
    private static ManualLogSource Log { get; set; }
    private static ConfigEntry<float> DayLength { get; set; }

    private void Awake()
    {
        Log = Logger;

        DayLength = Config.Bind("01. Day Length", "Day Length", 675f, new ConfigDescription($"Set the length of a day", new AcceptableValueList<float>(675f, 900f, 1125f, 1350f), new ConfigurationManagerAttributes {Order = 1, CustomDrawer = LengthSlider}));
        Seconds = DayLength.Value;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        StartupLogger.PrintModLoaded(PluginName, Log);
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