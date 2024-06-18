namespace KeepAlive;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.sunhaven.keepalive";
    private const string PluginName = "Keep Alive";
    private const string PluginVersion = "0.0.6";

    
    private void Awake()
    {
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Logger.LogInfo($"Plugin {PluginName} is loaded!");
    }

    private void OnDestroy()
    {
        Logger.LogInfo($"Exit: {PluginName} prevented the death of BepInEx related objects {Patches.Counter} times!");
    }
}