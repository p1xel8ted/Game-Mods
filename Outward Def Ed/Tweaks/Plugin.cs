namespace Tweaks;

[BepInPlugin(GUID, NAME, VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource Log = null!;

    private const string GUID = "p1xel8ted.outward_de.tweaks";
    private const string NAME = "Tweaks!";
    private const string VERSION = "0.1.0";

    private readonly static int DisplayWidth = Display.displays[0].systemWidth;
    private readonly static int DisplayHeight = Display.displays[0].systemHeight;
    private readonly static int MaxRefresh = Screen.resolutions.Max(a => a.refreshRate);

    private void Awake()
    {
        Log = Logger;
        SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
  
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), GUID);
        Log.LogInfo($"Loaded {NAME}!");
    }
    private static void SceneManagerOnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Display.displays[0].Activate(DisplayWidth, DisplayHeight, MaxRefresh);
        Screen.SetResolution(DisplayWidth, DisplayHeight, FullScreenMode.FullScreenWindow, MaxRefresh);
        Application.targetFrameRate = MaxRefresh;
        Log.LogInfo($"Scene loaded: {scene.name}");
        Log.LogInfo($"Set resolution to {DisplayWidth}x{DisplayHeight}@{MaxRefresh}Hz");
        Log.LogInfo($"Current FixedDeltaTime in FPS: {1f / Time.fixedDeltaTime}");
        Time.fixedDeltaTime = 1f / Utils.FindLowestFrameRateMultipleAboveFifty(MaxRefresh);
        Log.LogInfo($"New FixedDeltaTime in FPS: {1f / Time.fixedDeltaTime}");
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.F4)) return;
        SaveManager.Instance.Save(true, true);
        Log.LogInfo("Saved!");
        if (Patches.CharacterUI is not null)
        {
            Patches.CharacterUI.ShowInfoNotification("Game Saved!");
        }
    }

}