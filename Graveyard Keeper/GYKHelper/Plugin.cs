using MonoMod.Utils;

namespace GYKHelper;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.gykhelper";
    private const string PluginName = "GYK Helper Library";
    private const string PluginVer = "3.1.1";
    public static ManualLogSource Log { get; private set; }

    internal static ConfigEntry<bool> DisplayDuplicateHarmonyPatches { get; private set; }

    private void Awake()
    {
        InitializeDisableUnityLogging();
        Log = Logger;
        RegisterEventHandlers();
        PatchWithHarmony();
        Logger.LogInfo($"Plugin {PluginName} is loaded! Running game version {LazyConsts.VERSION} on {PlatformHelper.Current}.\nInstalled DLC, Stories: {DLCEngine.IsDLCStoriesAvailable()}, Refugee: {DLCEngine.IsDLCRefugeesAvailable()}, Souls: {DLCEngine.IsDLCSoulsAvailable()} ");
        
        
    }

    private void InitializeDisableUnityLogging()
    {
        Debug.unityLogger.logEnabled = true;
        DisplayDuplicateHarmonyPatches = Config.Bind("1. General", "Display Duplicate Harmony Patches", false, new ConfigDescription("Output duplicate harmony patches to log when clicking on Start Game", null, new ConfigurationManagerAttributes {IsAdvanced = true, Order = 1}));
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