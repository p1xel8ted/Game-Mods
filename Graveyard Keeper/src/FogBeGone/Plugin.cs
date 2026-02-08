namespace FogBeGone;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.fogbegone";
    private const string PluginName = "Fog, Be Gone!";
    private const string PluginVer = "3.4.8";

    private static ManualLogSource Log { get; set; }
    
    private void Awake()
    {
        Log = Logger;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        GYKHelper.StartupLogger.PrintModLoaded(PluginName, Log);
    }
    
}