namespace NoIntros;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.nointros";
    private const string PluginName = "No Intros!";
    private const string PluginVer = "2.2.7";
    private static ManualLogSource Log { get; set; }

    private void Awake()
    {
        Log = Logger;
        Shared.StartupLogger.PrintModLoaded(PluginName, Log);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
    }
}