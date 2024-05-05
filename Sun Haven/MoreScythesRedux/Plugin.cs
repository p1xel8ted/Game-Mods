namespace MoreScythesRedux;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.sunhaven.morescythesredux";
    private const string PluginName = "More Scythes Redux";
    private const string PluginVersion = "0.1.1";
    public static ManualLogSource LOG { get; private set; }

    private void Awake()
    {
        LOG = new ManualLogSource(PluginName);
        BepInEx.Logging.Logger.Sources.Add(LOG);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Logger.LogInfo($"{PluginName} plugin has loaded successfully.");
    }


}