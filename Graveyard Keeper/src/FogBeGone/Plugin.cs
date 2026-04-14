namespace FogBeGone;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.fogbegone";
    private const string PluginName = "Fog, Be Gone!";
    private const string PluginVer = "3.4.11";

    private static ManualLogSource Log { get; set; }

    internal static ConfigEntry<bool> DisableFog { get; private set; }
    internal static ConfigEntry<bool> DisableWind { get; private set; }
    internal static ConfigEntry<bool> DisableRain { get; private set; }

    internal static bool DisableFogCached;
    internal static bool DisableWindCached;
    internal static bool DisableRainCached;

    private void Awake()
    {
        Log = Logger;
        InitConfiguration();
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
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
    }
}