namespace KeepAlive;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.sunhaven.keepalive";
    private const string PluginName = "Keep Alive";
    private const string PluginVersion = "0.0.5";
    private static ManualLogSource LOG { get; set; }
    
    private void Awake()
    {
        LOG = new ManualLogSource(PluginName);
        BepInEx.Logging.Logger.Sources.Add(LOG);

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        LOG.LogInfo($"Plugin {PluginName} is loaded!");
    }

    private void OnDestroy()
    {
        LOG.LogInfo($"OnDestroy: {PluginName} prevented the death of BepInEx related objects {Patches.Counter} times!");
    }

    private void OnDisable()
    {
        LOG.LogInfo($"OnDisable: {PluginName} prevented the death of BepInEx related objects {Patches.Counter} times!");
    }
}