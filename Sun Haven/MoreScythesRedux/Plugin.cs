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
        SceneManager.sceneLoaded += (_, _) =>
        {
            ItemHandler.CreateScytheItems();
        };
        LOG = Logger;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Logger.LogInfo($"{PluginName} plugin has loaded successfully.");
    }
    
}