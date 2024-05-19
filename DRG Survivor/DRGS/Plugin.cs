namespace DRGS;

[Harmony]
[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BasePlugin
{
    private const string PluginGuid = "p1xel8ted.drgs.tweaks";
    private const string PluginName = "DRGS Tweaks";
    private const string PluginVersion = "0.1.0";

    private static ConfigEntry<FullScreenMode> FullScreenModeConfig { get; set; }
    private static int MainWidth => Display.displays[DisplayToUse.Value].systemWidth;
    private static int MainHeight => Display.displays[DisplayToUse.Value].systemHeight;
    private static int MaxRefresh => Screen.resolutions.Max(a => a.refreshRate);
    private static ConfigEntry<bool> RunInBackground { get; set; }
    internal static ConfigEntry<float> DropPodTime { get; private set; }
    private static ConfigEntry<bool> MuteInBackground { get; set; }
    private static ConfigEntry<int> FieldOfView { get; set; }
    private static ConfigEntry<int> DisplayToUse { get; set; }
    private static ManualLogSource Logger { get; set; }

    public override void Load()
    {
        Logger = Log;

        FullScreenModeConfig = Config.Bind("01. Display", "Full Screen Mode", FullScreenMode.FullScreenWindow, new ConfigDescription("Set the full screen mode"));
        FullScreenModeConfig.SettingChanged += (_, _) =>
        {
            UpdateDisplay();
        };


        DisplayToUse = Config.Bind("01. Display", "Display To Use", 0, new ConfigDescription("Display to use", new AcceptableValueList<int>(Display.displays.Select((_, i) => i).ToArray())));
        DisplayToUse.SettingChanged += (_, _) =>
        {
            UpdateDisplay();
        };

        FieldOfView = Config.Bind("02. Camera", "Field of View", 10, new ConfigDescription("Increase or decrease the field of view of the camera. Game default is 25. This adds/subtracts to that 25.", new AcceptableValueRange<int>(-24, 50), new ConfigurationManagerAttributes {Order = 100}));
        FieldOfView.SettingChanged += (_, _) =>
        {
            UpdateCamera();
        };
        
        DropPodTime = Config.Bind("03. Drop Pod", "Drop Pod Time", 60f, new ConfigDescription("Time in minutes before the drop pod leaves. 30 second increments.", new AcceptableValueRange<float>(0.5f, 300), new ConfigurationManagerAttributes {Order = 101}));
        DropPodTime.SettingChanged += (_, _) =>
        {
            var currentValue = DropPodTime.Value;
            var newValue = (float)Math.Round(currentValue * 2, MidpointRounding.AwayFromZero) / 2;
            DropPodTime.Value = newValue;
            
            //output to log in hours:minutes:seconds
            var time = TimeSpan.FromMinutes(newValue);
            Log.LogInfo($"Drop Pod Time: {time.Hours:D2}:{time.Minutes:D2}:{time.Seconds:D2}");
            
            Patches.UpdateDropPodTimer();
        };
        
        
        RunInBackground = Config.Bind("04. Misc", "Run In Background", true, new ConfigDescription("Allows the game to run even when not in focus.", null, new ConfigurationManagerAttributes {Order = 99}));
        RunInBackground.SettingChanged += (_, _) =>
        {
            FocusChanged(Application.isFocused);
        };

        MuteInBackground = Config.Bind("04. Misc", "Mute In Background", false, new ConfigDescription("Mutes the game's audio when it is not in focus.", null, new ConfigurationManagerAttributes {Order = 98}));
        MuteInBackground.SettingChanged += (_, _) =>
        {
            FocusChanged(Application.isFocused);
        };
        Application.focusChanged += (Il2CppSystem.Action<bool>) FocusChanged;

        SceneManager.sceneLoaded += (UnityAction<Scene, LoadSceneMode>) SceneManagerOnSceneLoaded;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Log.LogInfo($"Plugin {PluginName} is loaded!");
    }

    private static void FocusChanged(bool focus)
    {
        Application.runInBackground = RunInBackground.Value;
        AudioListener.pause = !focus && MuteInBackground.Value;
    }

    private static void SceneManagerOnSceneLoaded(Scene a, LoadSceneMode l)
    {
        Logger.LogInfo($"Scene loaded: {a.name}");
        UpdateDisplay();
        UpdateCamera();
    }

    private static void UpdateCamera()
    {
        if (!CameraController.instance) return;
        var vCam = CameraController.instance.vCam;
        vCam.m_Lens = vCam.m_Lens with {FieldOfView = 25f + FieldOfView.Value};
    }

    private static void UpdateDisplay()
    {
        Display.displays[DisplayToUse.Value].Activate();
        Screen.SetResolution(MainWidth, MainHeight, FullScreenModeConfig.Value, MaxRefresh);
        Application.targetFrameRate = MaxRefresh;
        Time.fixedDeltaTime = 1f / MaxRefresh;
    }

}