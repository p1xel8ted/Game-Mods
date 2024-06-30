namespace CheatEnabler;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
[BepInDependency("p1xel8ted.sunhaven.keepalive")]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.sunhaven.cheatenabler";
    private const string PluginName = "Cheat Enabler";
    private const string PluginVersion = "0.2.7";
    internal static ManualLogSource LOG { get; private set; }
    internal static ConfigEntry<bool> Debug { get; private set; }
    
    private void Awake()
    {
        Debug = Config.Bind("01. General", "Debug", false, "Enable debug logging");
        LOG = Logger;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        LOG.LogInfo($"Plugin {PluginName} is loaded!");
        DontDestroyOnLoad(this);
    }

    private void OnDisable()
    {
        LOG.LogError($"Plugin {PluginName} was disabled/destroyed! Unless you are exiting the game, please install Keep Alive! - https://www.nexusmods.com/sunhaven/mods/31");
    }

    private void OnDestroy()
    {
        OnDisable();
    }
}