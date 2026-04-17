namespace FogBeGone;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
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
        InitConfiguration();
        UpdateChecker.Register(Info, CheckForUpdates);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), MyPluginInfo.PLUGIN_GUID);
    }

    private void InitConfiguration()
    {
        DisableFog = Config.Bind("01. General", "Disable Fog", true,
            new ConfigDescription("Remove fog from outdoor and indoor weather.", null,
                new ConfigurationManagerAttributes { Order = 3 }));
        DisableWind = Config.Bind("01. General", "Disable Wind", false,
            new ConfigDescription("Remove wind weather effects.", null,
                new ConfigurationManagerAttributes { Order = 2 }));
        DisableRain = Config.Bind("01. General", "Disable Rain", false,
            new ConfigDescription("Remove rain weather effects.", null,
                new ConfigurationManagerAttributes { Order = 1 }));

        DisableFogCached = DisableFog.Value;
        DisableWindCached = DisableWind.Value;
        DisableRainCached = DisableRain.Value;

        DisableFog.SettingChanged += (_, _) => DisableFogCached = DisableFog.Value;
        DisableWind.SettingChanged += (_, _) => DisableWindCached = DisableWind.Value;
        DisableRain.SettingChanged += (_, _) => DisableRainCached = DisableRain.Value;

        CheckForUpdates = Config.Bind("── Updates ──", "Check for Updates", true,
            new ConfigDescription(
                "Show a notice on the main menu when a newer version of this mod is available on NexusMods. Click the notice to open the mod's page.",
                null,
                new ConfigurationManagerAttributes { Order = 0 }));
    }
}