namespace WheresMaPoints;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.wheresmapoints";
    private const string PluginName = "Where's Ma' Points!";
    private const string PluginVer = "0.3.1";

    internal static ConfigEntry<bool> ShowPointGainAboveKeeper { get; private set; }
    internal static ConfigEntry<bool> StillPlayCollectAudio { get; private set; }
    internal static ConfigEntry<bool> AlwaysShowXpBar { get; private set; }
    private static ManualLogSource Log { get; set; }

    private void Awake()
    {
        Log = Logger;
        InitConfiguration();
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        StartupLogger.PrintModLoaded(PluginName, Log);
    }

    private void InitConfiguration()
    {
        AlwaysShowXpBar = Config.Bind("01. User Interface", "Always Show XP Bar", true, new ConfigDescription("Display the experience bar constantly, even without active experience gain.", null, new ConfigurationManagerAttributes {Order = 6}));
        ShowPointGainAboveKeeper = Config.Bind("02. Visual Feedback", "Show Point Gain Above Keeper", true, new ConfigDescription("Display the points earned above the keeper's head.", null, new ConfigurationManagerAttributes {Order = 5}));
        StillPlayCollectAudio = Config.Bind("03. Audio Feedback", "Still Play Collect Audio", false, new ConfigDescription("Keep playing the collect audio when point gain is displayed above the keeper's head.", null, new ConfigurationManagerAttributes {Order = 4}));
    }

}