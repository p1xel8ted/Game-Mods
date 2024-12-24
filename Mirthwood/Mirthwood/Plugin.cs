namespace Mirthwood;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.mirthwood.tweaks";
    private const string PluginName = "Mirthwood Tweaks";
    private const string PluginVersion = "0.1.0";

    internal static ManualLogSource Log { get; private set; }
    private static ConfigEntry<bool> RunInBackground { get; set; }
    private static ConfigEntry<bool> MuteInBackground { get; set; }
    internal static ConfigEntry<bool> OcclusionCulling { get; private set; }
    internal static ConfigEntry<float> NearClipPlane { get; private set; }
    internal static ConfigEntry<int> FarClipPlane { get; private set; }
    internal static ConfigEntry<bool> ExpandHUD { get; private set; }
    internal static ConfigEntry<float> DialogueSpeed { get; private set; }

    internal static ConfigEntry<int> FieldOfView { get; private set; }

    private const float NativeAspect = 16f / 9f;
    internal static bool IsUltraWide => Screen.currentResolution.width / (float)Screen.currentResolution.height > NativeAspect;

    private void Awake()
    {
        Debug.unityLogger.logEnabled = true;

        Log = Logger;

        OcclusionCulling = Config.Bind("01. Graphics", "Occlusion Culling", true, new ConfigDescription("Enable occlusion culling to improve performance. Game default is enabled.", null, new ConfigurationManagerAttributes { Order = 88 }));
        OcclusionCulling.SettingChanged += (_, _) => { Patches.UpdateCameras(); };

        NearClipPlane = Config.Bind("01. Graphics", "Near Clip Plane", 10f, new ConfigDescription("The nearest distance from the camera at which objects are rendered. Lower values allow objects closer to the camera to be visible, but may cause visual artifacts.", new AcceptableValueRange<float>(0.1f, 10f), new ConfigurationManagerAttributes { Order = 87 }));
        NearClipPlane.SettingChanged += (_, _) =>
        {
            NearClipPlane.Value = Mathf.RoundToInt(NearClipPlane.Value * 4) / 4f;
            Patches.UpdateCameras();
        };

        FarClipPlane = Config.Bind("01. Graphics", "Far Clip Plane", 100, new ConfigDescription("The farthest distance from the camera at which objects are rendered. Higher values allow distant objects to remain visible but may affect performance.", new AcceptableValueRange<int>(100, 10000), new ConfigurationManagerAttributes { Order = 86 }));
        FarClipPlane.SettingChanged += (_, _) =>
        {
            Patches.UpdateCameras();
        };

        FieldOfView = Config.Bind("01. Graphics", "Field of View Multiplier", 0, new ConfigDescription("Multiplier for the camera's field of view. Higher values increase the field of view, allowing more to be seen at once.", new AcceptableValueRange<int>(-25, 25), new ConfigurationManagerAttributes { Order = 85 }));
        FieldOfView.SettingChanged += (_, _) =>
        {
            Patches.UpdateCameras();
        };

        ExpandHUD = Config.Bind("02. UI", "Expand HUD", true, new ConfigDescription("Expands the HUD to fill the screen.", null, new ConfigurationManagerAttributes { Order = 85 }));
        ExpandHUD.SettingChanged += (_, _) => Patches.UpdateHUD();

        DialogueSpeed = Config.Bind("02. UI", "Dialogue Speed", 1f, new ConfigDescription("Set the speed of the dialogue text.", new AcceptableValueRange<float>(0.25f, 10f), new ConfigurationManagerAttributes { Order = 84 }));
        DialogueSpeed.SettingChanged += (_, _) => { DialogueSpeed.Value = Mathf.RoundToInt(DialogueSpeed.Value * 4) / 4f; };

        RunInBackground = Config.Bind("03. Misc", "Run In Background", true, new ConfigDescription("Allows the game to run even when not in focus.", null, new ConfigurationManagerAttributes { Order = 83 }));
        RunInBackground.SettingChanged += (_, _) => Application.runInBackground = RunInBackground.Value;

        MuteInBackground = Config.Bind("03. Misc", "Mute In Background", false, new ConfigDescription("Mutes the game's audio when it is not in focus.", null, new ConfigurationManagerAttributes { Order = 82 }));

        SceneManager.sceneLoaded += OnSceneLoaded;
        Application.focusChanged += FocusChanged;

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Log.LogInfo($"Plugin {PluginName} is loaded!");
    }

    private static void FocusChanged(bool focus)
    {
        Application.runInBackground = RunInBackground.Value;
        var audioSources = Resources.FindObjectsOfTypeAll<AudioSource>();
        foreach (var audioSource in audioSources)
        {
            audioSource.mute = !focus && MuteInBackground.Value;
        }
    }


    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.unityLogger.logEnabled = true;
        Application.runInBackground = RunInBackground.Value;
        Patches.UpdateHUD();
        
        OptimizersSetup.Settings.AutoDetectFOVAndScreenChange = true;
        OptimizersSetup.Settings.ProgressiveResponseQuality = 3000;
    }
}