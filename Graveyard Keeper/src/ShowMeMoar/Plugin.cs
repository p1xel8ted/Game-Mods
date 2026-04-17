namespace ShowMeMoar;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static ConfigEntry<bool> Ultrawide { get; private set; }
    internal static ConfigEntry<KeyboardShortcut> ZoomIn { get; private set; }
    internal static ConfigEntry<KeyboardShortcut> ZoomOut { get; private set; }

    internal const float NativeAspect = 16f / 9f;
    internal static float CurrentAspect => (float) Display.main.systemWidth / Display.main.systemHeight;
    internal static bool ScreenIsUltrawide => CurrentAspect > NativeAspect;
    internal static float ScaleFactor => CurrentAspect / NativeAspect;
    internal static ConfigEntry<float> HudScale { get; private set; }
    internal static ConfigEntry<float> HorizontalHudPosition { get; private set; }
    internal static ConfigEntry<float> VerticalHudPosition { get; private set; }
    internal static ConfigEntry<float> Zoom { get; private set; }
    private static ConfigEntry<float> CraftIconAboveStations { get; set; }

    internal static ConfigEntry<bool> RemoveFog { get; private set; }

    internal static ConfigEntry<bool> BorderlessWindowed { get; private set; }
    internal static ConfigEntry<bool> SetVsyncLimitToMaxRefreshRate { get; private set; }
    internal static ConfigEntry<bool> ColorCorrection { get; private set; }
    internal static ConfigEntry<bool> CheckForUpdates { get; private set; }
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
            var widget = smallFont.GetComponent<UIWidget>();
            if (widget != null) widget.color = new Color(0, 0, 0, 0);
        };
        InitConfiguration();
        UpdateChecker.Register(Info, CheckForUpdates);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), MyPluginInfo.PLUGIN_GUID);
    }

    internal static void OnGameStartedPlaying()
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

        BorderlessWindowed = Config.Bind("8. Display", "Borderless Windowed", false, new ConfigDescription("Run the game in borderless windowed mode instead of fullscreen. Restart required.", null, new ConfigurationManagerAttributes {Order = 1}));

        SetVsyncLimitToMaxRefreshRate = Config.Bind("8. Display", "Set Vsync Limit To Max Refresh Rate", true, new ConfigDescription("Set Vsync limit to the maximum refresh rate of the monitor. Game default is 60fps with VSYNC enabled.", null, new ConfigurationManagerAttributes {Order = 0}));

        CheckForUpdates = Config.Bind("── Updates ──", "Check for Updates", true,
            new ConfigDescription(
                "Show a notice on the main menu when a newer version of this mod is available on NexusMods. Click the notice to open the mod's page.",
                null,
                new ConfigurationManagerAttributes { Order = 0 }));

        SceneManager.sceneLoaded += (_, _) => UpdateCC();
    }

    internal static void UpdateCC()
    {
        var cc = Resources.FindObjectsOfTypeAll<AmplifyColorEffect>();
        foreach (var c in cc)
        {
            c.enabled = ColorCorrection.Value;
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