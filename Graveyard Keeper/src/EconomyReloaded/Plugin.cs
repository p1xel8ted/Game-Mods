namespace EconomyReloaded;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource Log { get; private set; }

    internal static ConfigEntry<bool> Inflation { get; private set; }
    internal static ConfigEntry<bool> Deflation { get; private set; }
    internal static ConfigEntry<bool> CheckForUpdates { get; private set; }

    private void Awake()
    {
        Log = Logger;
        InitConfiguration();
        UpdateChecker.Register(Info, CheckForUpdates);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), MyPluginInfo.PLUGIN_GUID);
    }

    private void InitConfiguration()
    {
        Inflation = Config.Bind("01. Economy", "Inflation", true, new ConfigDescription("Control whether your trades experiences inflation (the more you buy, the more it cost's per unit.", null, new ConfigurationManagerAttributes {Order = 2}));
        Inflation.SettingChanged += (_, _) => Patches.UpdateStaticCost();
        Deflation = Config.Bind("01. Economy", "Deflation", true, new ConfigDescription("Control whether your trades experiences deflation (the more you sell, the less you get per unit.", null, new ConfigurationManagerAttributes {Order = 1}));
        Deflation.SettingChanged += (_, _) => Patches.UpdateStaticCost();

        CheckForUpdates = Config.Bind("── Updates ──", "Check for Updates", true, new ConfigDescription(
            "Show a notice on the main menu when a newer version of this mod is available on NexusMods. Click the notice to open the mod's page.",
            null,
            new ConfigurationManagerAttributes { Order = 0 }));
    }
}
