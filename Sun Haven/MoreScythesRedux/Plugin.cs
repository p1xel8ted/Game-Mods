namespace MoreScythesRedux;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
[BepInDependency("p1xel8ted.sunhaven.keepalive")]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.sunhaven.morescythesredux";
    private const string PluginName = "More Scythes Redux";
    private const string PluginVersion = "0.1.5";
    public static ManualLogSource LOG { get; private set; }

    private void Awake()
    {
        SceneManager.sceneLoaded += (_, _) =>
        {
            try
            {
                Database.GetData<ItemData>(ItemHandler.OriginalScytheId, null);
                Database.GetData<ItemData>(ItemHandler.AdamantScytheId, null);
                Database.GetData<ItemData>(ItemHandler.MithrilScytheId, null);
                Database.GetData<ItemData>(ItemHandler.SuniteScytheId, null);
                Database.GetData<ItemData>(ItemHandler.GloriteScytheId, null);

                ItemHandler.CreateScytheItems();
            }
            catch (Exception)
            {
                //fail silently
            }
        };
        
        LOG = Logger;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Logger.LogInfo($"{PluginName} plugin has loaded successfully.");
    }
    
    private void OnDestroy()
    {
        OnDisable();
    }
    
    private void OnDisable()
    {
        LOG.LogError($"Plugin {PluginName} was disabled/destroyed! Unless you are exiting the game, please install Keep Alive! - https://www.nexusmods.com/sunhaven/mods/31");
    }

}