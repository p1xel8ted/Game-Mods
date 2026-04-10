namespace GYKHelper;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.gykhelper";
    private const string PluginName = "GYK Helper";
    private const string PluginVer = "4.0.0";

    private void Awake()
    {
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        StartupLogger.PrintModLoaded(PluginName, Logger);
    }
}
