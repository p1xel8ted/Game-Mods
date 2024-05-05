namespace CheatEnabler
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public partial class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "p1xel8ted.sunhaven.cheatenabler";
        private const string PluginName = "Cheat Enabler";
        private const string PluginVersion = "0.1.7";
        internal static ManualLogSource LOG { get; set; }
        //private static CheatCs CheatCsInstance { get; set; }

        private void Awake()
        {
            LOG = new ManualLogSource(PluginName);
            BepInEx.Logging.Logger.Sources.Add(LOG);
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
            LOG.LogInfo($"Plugin {PluginName} is loaded!");
           // CheatCsInstance = gameObject.AddComponent<CheatCs>();
           // DontDestroyOnLoad(this);
          //  DontDestroyOnLoad(CheatCsInstance);

        }

        private void OnDestroy()
        {
            LOG.LogError("I've been destroyed!");
        }

        private void OnDisable()
        {
            LOG.LogError("I've been disabled!");
        }
    }
}