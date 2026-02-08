namespace AlchemyResearchRedux;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.alchemyresearchredux";
    private const string PluginName = "Alchemy Research Redux";
    private const string PluginVer = "0.1.1";
    internal static ManualLogSource LOG { get; private set; }
    
    private void Awake()
    {
        LOG = Logger;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        GYKHelper.StartupLogger.PrintModLoaded(PluginName, LOG);
    }

}