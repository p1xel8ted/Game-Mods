namespace ShowMeMoar;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("p1xel8ted.gyk.gykhelper", "3.0.9")]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.showmemoar";
    private const string PluginName = "Show Me Moar!";
    private const string PluginVer = "0.1.6";
    internal static ConfigEntry<bool> Ultrawide { get; private set; }
    private static ConfigEntry<KeyboardShortcut> ZoomIn { get; set; }
    private static ConfigEntry<KeyboardShortcut> ZoomOut { get; set; }

    internal const float NativeAspect = 16f / 9f;
    internal static float CurrentAspect => (float) Display.main.systemWidth / Display.main.systemHeight;
    internal static bool ScreenIsUltrawide => CurrentAspect > NativeAspect;
    internal static float ScaleFactor => CurrentAspect / NativeAspect;
    internal static ConfigEntry<float> HudScale { get; private set; }
    internal static ConfigEntry<float> HorizontalHudPosition { get; private set; }
    internal static ConfigEntry<float> VerticalHudPosition { get; private set; }
    private static ConfigEntry<float> Zoom { get; set; }
    private static ConfigEntry<float> CraftIconAboveStations { get; set; }

    internal static ConfigEntry<bool> RemoveFog { get; private set; }

    internal static ConfigEntry<bool> SetVsyncLimitToMaxRefreshRate { get; private set; }
    internal static ConfigEntry<bool> ColorCorrection { get; private set; }
    private static GameObject Icons { get; set; }
    internal static ManualLogSource Log { get; private set; }
    
    internal static float MaxRefreshRate => Screen.resolutions.Max(a => a.refreshRate);

    private void Awake()
    {
        Log = Logger;
        SceneManager.sceneLoaded += (_, _) =>
        {
            var smallFont = GameObject.Find("UI Root/Label size calculators/small_font");
            if (smallFont == null) return;
            Log.LogInfo("Hiding small font. We can't disable it as it breaks UI.");
            smallFont.ChangeColor(new Color(0, 0, 0, 0), 0f);
        };
        Actions.GameStartedPlaying += OnGameStartedPlaying;
        InitConfiguration();
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Log.LogInfo($"Plugin {PluginName} is loaded!");
    }

    private static void OnGameStartedPlaying()
    {
        if (!MainGame.game_started) return;

        Patches.ScreenSize = GameObject.Find("UI Root/Screen size panel").transform;
        if (Patches.ScreenSize == null)
        {
            Log.LogError("Screen size panel not found!");
        }

        var setting = Zoom.Value;
        var defaultZoom = GameSettings.current_resolution.y / 2f;
        Camera.main!.orthographicSize = defaultZoom + setting;
    }

    private void InitConfiguration()
    {
        var defaultZoom = Screen.currentResolution.height / 2f;
        var min = 0 - defaultZoom;

        Ultrawide = Config.Bind("2. Ultrawide", "Ultrawide", ScreenIsUltrawide, new ConfigDescription("Enable or disable ultrawide support. You must restart the game after changing this setting.", null, new ConfigurationManagerAttributes {Order = 7}));

        CraftIconAboveStations = Config.Bind("3. Scale", "Interaction Bubble Scale", 1f, new ConfigDescription("Changes the scale of the icons that appear above crafting stations and interaction icons.", new AcceptableValueRange<float>(0.1f, 10f), new ConfigurationManagerAttributes {Order = 6}));
        CraftIconAboveStations.SettingChanged += (_, _) =>
        {
            if (!MainGame.game_started) return;
            UpdateCraftIconScale(CraftIconAboveStations.Value);
        };
        HudScale = Config.Bind("3. Scale", "HUD Scale", 1f, new ConfigDescription("Changes the scale of the HUD.", new AcceptableValueRange<float>(0.1f, 10f), new ConfigurationManagerAttributes {Order = 5}));
        HudScale.SettingChanged += (_, _) =>
        {
            if (!MainGame.game_started) return;
            if (Patches.HUD != null) Patches.HUD.transform.localScale = new Vector3(HudScale.Value, HudScale.Value, 1);
        };

        HorizontalHudPosition = Config.Bind("4. Positions", "Horizontal HUD Position", 1f, new ConfigDescription("Changes the horizontal position of the HUD.", new AcceptableValueRange<float>(-5, 5), new ConfigurationManagerAttributes {Order = 4}));
        HorizontalHudPosition.SettingChanged += (_, _) =>
        {
            if (!MainGame.game_started) return;
            if (Patches.ScreenSize != null) Patches.ScreenSize.transform.localScale = new Vector3(HorizontalHudPosition.Value, VerticalHudPosition.Value, 1);
        };

        VerticalHudPosition = Config.Bind("4. Positions", "Vertical HUD Position", 1f, new ConfigDescription("Changes the vertical position of the HUD.", new AcceptableValueRange<float>(-5, 5), new ConfigurationManagerAttributes {Order = 3}));
        VerticalHudPosition.SettingChanged += (_, _) =>
        {
            if (!MainGame.game_started) return;
            if (Patches.ScreenSize != null) Patches.ScreenSize.transform.localScale = new Vector3(HorizontalHudPosition.Value, VerticalHudPosition.Value, 1);
        };


        Zoom = Config.Bind("5. Zoom", "Zoom", 0f, new ConfigDescription("Zoom", new AcceptableValueRange<float>(min + 10, defaultZoom * 2), new ConfigurationManagerAttributes {Order = 2}));
        Zoom.SettingChanged += (_, _) =>
        {
            if (!MainGame.game_started) return;
            Camera.main!.orthographicSize = defaultZoom + Zoom.Value;
        };

        ZoomIn = Config.Bind("5. Zoom", "Zoom In", new KeyboardShortcut(KeyCode.KeypadPlus), new ConfigDescription("Zoom In", null, new ConfigurationManagerAttributes {Order = 1}));
        ZoomOut = Config.Bind("5. Zoom", "Zoom Out", new KeyboardShortcut(KeyCode.KeypadMinus), new ConfigDescription("Zoom Out", null, new ConfigurationManagerAttributes {Order = 0}));

        RemoveFog = Config.Bind("6. Fog", "Remove Fog", true, new ConfigDescription("Remove fog from the game.", null, new ConfigurationManagerAttributes {Order = 0}));
        ColorCorrection = Config.Bind("7. Color Correction", "Color Correction", true, new ConfigDescription("Enable or disable color correction.", null, new ConfigurationManagerAttributes {Order = 0}));
        ColorCorrection.SettingChanged += (_, _) => UpdateCC();

        SetVsyncLimitToMaxRefreshRate = Config.Bind("8. Vsync", "Set Vsync Limit To Max Refresh Rate", true, new ConfigDescription("Set Vsync limit to the maximum refresh rate of the monitor. Game default is 60fps with VSYNC enabled.", null, new ConfigurationManagerAttributes {Order = 0}));
        SceneManager.sceneLoaded += (_, _) => UpdateCC();
    }

    internal static void UpdateCC()
    {
        Time.fixedDeltaTime = 1f / 180f;
        Plugin.Log.LogWarning($"Fixed Delta in FPS: {1f / Time.fixedDeltaTime}");
        var cc = Resources.FindObjectsOfTypeAll<AmplifyColorEffect>();
        foreach (var c in cc)
        {
            c.enabled = ColorCorrection.Value;
        }
    }

    private void Update()
    {
        if (!MainGame.game_started) return;

        if (ZoomIn.Value.IsPressed())
        {
            Zoom.Value -= 5f;
        }

        if (ZoomOut.Value.IsPressed())
        {
            Zoom.Value += 5f;
        }
    }

    private static void UpdateCraftIconScale(float scale)
    {
        Icons ??= GameObject.Find("UI Root/Interaction bubbles");
        if (Icons != null)
        {
            Icons.transform.localScale = new Vector3(scale, scale, 1);
        }
    }
}