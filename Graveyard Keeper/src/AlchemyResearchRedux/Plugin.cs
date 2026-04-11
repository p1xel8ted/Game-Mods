namespace AlchemyResearchRedux;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.alchemyresearchredux";
    private const string PluginName = "Alchemy Research Redux";
    private const string PluginVer = "0.1.3";
    internal static ManualLogSource LOG { get; private set; }
    internal static ConfigEntry<bool> Debug { get; private set; }

    private void Awake()
    {
        LOG = Logger;
        Debug = Config.Bind("00. Advanced", "Debug Logging", false,
            new ConfigDescription("Enable or disable debug logging.", null, new ConfigurationManagerAttributes {IsAdvanced = true, Order = 1}));
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
    }

}