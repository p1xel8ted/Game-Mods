namespace GYKHelper;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.gykhelper";
    private const string PluginName = "GYK Helper Library";
    private const string PluginVer = "3.0.5";

    public static ManualLogSource Log { get; private set; }
    private static ConfigEntry<bool> UnityLogging { get; set; }
    internal static ConfigEntry<bool> DisplayDuplicateHarmonyPatches { get; private set; }

    private void Awake()
    {
        InitializeDisableUnityLogging();
        Log = Logger;
        RegisterEventHandlers();
        PatchWithHarmony();
        Logger.LogInfo($"Plugin {PluginName} is loaded! Running game version {Application.version} on {MonoMod.Utils.PlatformHelper.Current}.");
    }

    private void InitializeDisableUnityLogging()
    {
        UnityLogging = Config.Bind("1. General", "Unity Logging", false, new ConfigDescription("Toggle Unity Logging", null, new ConfigurationManagerAttributes {IsAdvanced = true, Order = 2}));
        UnityLogging.SettingChanged += (_, _) => Debug.unityLogger.logEnabled = UnityLogging.Value;
        DisplayDuplicateHarmonyPatches = Config.Bind("1. General", "Display Duplicate Harmony Patches", false, new ConfigDescription("Output duplicate harmony patches to log when clicking on Start Game", null, new ConfigurationManagerAttributes {IsAdvanced = true, Order = 1}));
        Debug.unityLogger.logEnabled = UnityLogging.Value;
    }

    private static void RegisterEventHandlers()
    {
        Actions.WorldGameObjectInteractPrefix += Actions.WorldGameObject_Interact;
        Actions.GameStartedPlaying += Actions.CleanGerries;
    }

    private static void PatchWithHarmony()
    {
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
    }
}