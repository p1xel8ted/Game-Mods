namespace AlchemyResearchRedux;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("p1xel8ted.gyk.gykhelper", "3.1.0")]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.alchemyresearchredux";
    private const string PluginName = "Alchemy Research Redux";
    private const string PluginVer = "0.1.0";
    internal static ManualLogSource LOG { get; private set; }
    
    private void Awake()
    {
        LOG = Logger;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        LOG.LogInfo($"Plugin {PluginName} is loaded!");
    }

}