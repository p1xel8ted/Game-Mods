namespace GerryFixer;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.gerryfixer";
    private const string PluginName = "Gerry Fixer...";
    private const string PluginVer = "0.1.7";

    internal static ConfigEntry<bool> Debug;
    internal static ConfigEntry<bool> AttemptToFixCutsceneGerrys { get; private set; }
    internal static ConfigEntry<bool> SpawnTavernCellarGerry { get; private set; }
    internal static ConfigEntry<bool> SpawnTavernMorgueGerry { get; private set; }
    internal static ManualLogSource Log { get; private set; }

    private void Awake()
    {
        Log = Logger;
        InitConfiguration();
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        StartupLogger.PrintModLoaded(PluginName, Log);
    }

    private void InitConfiguration()
    {
        Debug = Config.Bind("01. Advanced", "Debug Logging", false, new ConfigDescription("Toggle debug logging on or off", null, new ConfigurationManagerAttributes {IsAdvanced = true, Order = 4}));

        AttemptToFixCutsceneGerrys = Config.Bind("02. Gerry", "Attempt To Fix Cutscene Gerrys", false, new ConfigDescription("Attempt to fix cutscene freezes/crashes due to Gerry not appearing", null, new ConfigurationManagerAttributes {Order = 3}));

        SpawnTavernCellarGerry = Config.Bind("02. Gerry", "Spawn Tavern Cellar Gerry", false, new ConfigDescription("Choose whether to spawn Gerry in the tavern cellar (if he does not exist)", null, new ConfigurationManagerAttributes {Order = 2}));

        SpawnTavernMorgueGerry = Config.Bind("02. Gerry", "Spawn Tavern Morgue Gerry", false, new ConfigDescription("Choose whether to spawn Gerry in the tavern morgue (if he does not exist)", null, new ConfigurationManagerAttributes {Order = 1}));
    }
}
