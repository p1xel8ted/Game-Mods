namespace NoTimeForFishing;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.notimeforfishing";
    private const string PluginName = "No Time For Fishing!";
    private const string PluginVer = "3.2.7";
    private static ManualLogSource Log { get; set; }

    private void Awake()
    {
        Log = Logger;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Shared.StartupLogger.PrintModLoaded(PluginName, Log);
    }
    
}