namespace EconomyReloaded;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.economyreloaded";
    private const string PluginName = "Economy Reloaded";
    private const string PluginVer = "1.3.7";
    internal static ManualLogSource Log { get; private set; }

    internal static ConfigEntry<bool> Inflation { get; private set; }
    internal static ConfigEntry<bool> Deflation { get; private set; }

    private void Awake()
    {
        Log = Logger;
        InitConfiguration();
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        StartupLogger.PrintModLoaded(PluginName, Log);
    }

    private void InitConfiguration()
    {
        Inflation = Config.Bind("01. Economy", "Inflation", true, new ConfigDescription("Control whether your trades experiences inflation (the more you buy, the more it cost's per unit.", null, new ConfigurationManagerAttributes {Order = 2}));
        Inflation.SettingChanged += (_, _) => Patches.UpdateStaticCost();
        Deflation = Config.Bind("01. Economy", "Deflation", true, new ConfigDescription("Control whether your trades experiences deflation (the more you sell, the less you get per unit.", null, new ConfigurationManagerAttributes {Order = 1}));
        Deflation.SettingChanged += (_, _) => Patches.UpdateStaticCost();
    }
}
