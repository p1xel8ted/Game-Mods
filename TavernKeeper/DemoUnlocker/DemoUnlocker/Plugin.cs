namespace DemoUnlocker;

[Harmony]
[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BasePlugin
{
    private const string PluginGuid = "p1xel8ted.tavernkeeperdemo.unlocker";
    private const string PluginName = "Tavern Keeper Demo Unlocker";
    private const string PluginVersion = "0.1.0";
  
    public override void Load()
    {
        Log.LogInfo($"Plugin {PluginName} is loaded!");
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        UpdateFlags();
    }

    private static void UpdateFlags()
    {
        GameFlags.ENABLE_DEMO_MAINMENU = false;
        GameFlags.ALLOW_ESCAPE_TO_MAINMENU = true;
        GameFlags.ALLOW_SAVELOAD = true;
        GameFlags.FORCE_SHOW_REGIONS_UNLOCKED_IN_MAIN_MENU = true;
        GameFlags.ALL_LEVELS_UNLOCKED = true;
        GameFlags.ENABLE_SHARE_CODES = true;
        GameFlags.ENABLE_TROPHY_CASE = true;
        GameFlags.ALLOW_FIRE_SPARKS = true;
        GameFlags.ALLOW_DIRECTORSTOOLBAR = true;
        GameFlags.DEBUG_ENABLE_DIRECTORS_TOOLBAR_DEVTOOLS = true;
        GameFlags.ALLOW_DEV_CHEATS = true;
        GameFlags.SHOW_HIDDEN_ITEMS_IN_MENU = true;
        GameFlags.AUTO_SAVE_INTERVAL_IN_MINUTES = 10;
        GameFlags.ENABLE_CINEMATIC_REPLAY = true;
        GameFlags.ENABLE_CREDITS = true;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(SaveLoadManager), nameof(SaveLoadManager.LateUpdate))]
    public static void SaveLoadManager_LateUpdate(SaveLoadManager __instance)
    {
        if (Input.GetKeyUp(KeyCode.F5))
        {
            __instance.QuickSave();
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(SceneManager), nameof(SceneManager.Internal_SceneLoaded))]
    public static void SceneManager_Internal_SceneLoaded(Scene scene)
    {
        UpdateFlags();
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameFlags), "get_IsDemoOrBeta")]
    public static void GameFlags_IsDemoOrBeta(ref bool __result)
    {
        __result = false;
    }
}