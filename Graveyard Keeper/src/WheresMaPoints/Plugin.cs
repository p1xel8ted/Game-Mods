namespace WheresMaPoints;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static ConfigEntry<bool> ShowPointGainAboveKeeper { get; private set; }
    internal static ConfigEntry<bool> StillPlayCollectAudio { get; private set; }
    internal static ConfigEntry<bool> AlwaysShowXpBar { get; private set; }
    internal static ConfigEntry<bool> CheckForUpdates { get; private set; }
    private static ManualLogSource Log { get; set; }

    private void Awake()
    {
        Log = Logger;
        InitConfiguration();
        UpdateChecker.Register(Info, CheckForUpdates);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), MyPluginInfo.PLUGIN_GUID);
    }

    private void InitConfiguration()
    {
        AlwaysShowXpBar = Config.Bind("01. User Interface", "Always Show XP Bar", true, new ConfigDescription("Display the experience bar constantly, even without active experience gain.", null, new ConfigurationManagerAttributes {Order = 6}));
        ShowPointGainAboveKeeper = Config.Bind("02. Visual Feedback", "Show Point Gain Above Keeper", true, new ConfigDescription("Display the points earned above the keeper's head.", null, new ConfigurationManagerAttributes {Order = 5}));
        StillPlayCollectAudio = Config.Bind("03. Audio Feedback", "Still Play Collect Audio", false, new ConfigDescription("Keep playing the collect audio when point gain is displayed above the keeper's head.", null, new ConfigurationManagerAttributes {Order = 4}));

        CheckForUpdates = Config.Bind("── Updates ──", "Check for Updates", true, new ConfigDescription(
            "Show a notice on the main menu when a newer version of this mod is available on NexusMods. Click the notice to open the mod's page.",
            null,
            new ConfigurationManagerAttributes { Order = 0 }));
    }

}