namespace NoIntros;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("p1xel8ted.gyk.gykhelper", "3.1.0")]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.nointros";
    private const string PluginName = "No Intros!";
    private const string PluginVer = "2.2.5";
    private static ManualLogSource Log { get; set; }

    private void Awake()
    {
        Log = Logger;
        Log.LogInfo($"Plugin {PluginName} is loaded!");
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
    }
}