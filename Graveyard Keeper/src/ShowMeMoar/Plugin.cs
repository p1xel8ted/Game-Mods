namespace ShowMeMoar;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private const string UltrawideSection       = "── Ultrawide ──";
    private const string ScaleSection           = "── Scale ──";
    private const string PositionsSection       = "── Positions ──";
    private const string ZoomSection            = "── Zoom ──";
    private const string FogSection             = "── Fog ──";
    private const string ColorCorrectionSection = "── Color Correction ──";
    private const string DisplaySection         = "── Display ──";
    private const string HighDpiSection         = "── High-DPI Fix ──";
    private const string UpdatesSection         = "── Updates ──";

    private static readonly Dictionary<string, string> SectionRenames = new()
    {
        [UltrawideSection]         = UltrawideSection,
        [ScaleSection]             = ScaleSection,
        [PositionsSection]         = PositionsSection,
        [ZoomSection]              = ZoomSection,
        [FogSection]               = FogSection,
        [ColorCorrectionSection]  = ColorCorrectionSection,
        [DisplaySection]           = DisplaySection,
        ["── 9. High-DPI Fix ──"] = HighDpiSection,
    };

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

    // High-DPI fix — one-liners read by HighDpiFix and the main-menu prompt.
    internal static ConfigEntry<string> DpiDetectedStatus { get; private set; }
    internal static ConfigEntry<string> DpiAppliedStatus { get; private set; }
    internal static ConfigEntry<bool> ApplyDpiFix { get; private set; }
    internal static ConfigEntry<bool> AskDpiFixAtStartup { get; private set; }
    internal static int CurrentDpi { get; private set; }
    internal static int ScalingPercent { get; private set; }
    internal static HighDpiFix.Host DpiHost { get; private set; }
    // Only offer the fix on real Windows — Wine/Proton and native Linux/macOS ignore the
    // Windows compatibility flag, so prompting there would be user-hostile.
    internal static bool HighDpiDetected => DpiHost == HighDpiFix.Host.Windows && CurrentDpi > 96;

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

        // Detect the host environment first. The fix only applies on real Windows;
        // on Wine/Proton the AppCompat flag is written into a fake registry that wine
        // doesn't consult for DPI awareness, and on native Linux/macOS there's no .exe
        // or registry to target in the first place.
        DpiHost = HighDpiFix.DetectHost();
        if (DpiHost == HighDpiFix.Host.Windows)
        {
            CurrentDpi = HighDpiFix.DetectDpi();
            ScalingPercent = HighDpiFix.DpiToScalingPercent(CurrentDpi);
            Log.LogInfo($"[HighDpiFix] Host=Windows, DPI={CurrentDpi} ({ScalingPercent}% scaling). Fix needed: {HighDpiDetected}");
        }
        else
        {
            CurrentDpi = 96;
            ScalingPercent = 100;
            Log.LogInfo($"[HighDpiFix] Host={DpiHost} — skipping DPI fix entirely (this is a Windows-only workaround).");
        }

        MigrateRenamedSections();
        InitConfiguration();
        // Lang.Init reads GameSettings._cur_lng lazily on every Lang.Get, so it's safe to
        // initialise here before the game has finished loading language settings.
        Lang.Init(Assembly.GetExecutingAssembly(), Log);
        RefreshDpiStatus();
        UpdateChecker.Register(Info, CheckForUpdates);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), MyPluginInfo.PLUGIN_GUID);
    }

    // Rewrites legacy numbered section headers to the plain "── Name ──" style so existing
    // user values survive the rename. Idempotent.
    private void MigrateRenamedSections()
    {
        var path = Config.ConfigFilePath;
        if (!File.Exists(path)) return;

        string content;
        try { content = File.ReadAllText(path); }
        catch (Exception ex) { Log.LogWarning($"[Migration] Could not read {path}: {ex.Message}"); return; }

        var renamed = 0;
        foreach (var kv in SectionRenames)
        {
            var oldHeader = $"[{kv.Key}]";
            var newHeader = $"[{kv.Value}]";
            if (!content.Contains(oldHeader)) continue;
            content = content.Replace(oldHeader, newHeader);
            renamed++;
        }
        if (renamed == 0) return;

        try { File.WriteAllText(path, content); }
        catch (Exception ex) { Log.LogWarning($"[Migration] Could not write {path}: {ex.Message}"); return; }

        Log.LogInfo($"[Migration] Renamed {renamed} legacy config section header(s) to the '── Name ──' style. Existing user values preserved.");
        Config.Reload();
    }

    // Updates the read-only status config entries visible in ConfigurationManager.
    // Called at startup and after every Apply/Remove so users see live state.
    internal static void RefreshDpiStatus()
    {
        if (DpiDetectedStatus != null)
        {
            DpiDetectedStatus.Value = DpiHost switch
            {
                HighDpiFix.Host.Windows => string.Format(
                    Lang.Get(HighDpiDetected ? "DpiStatusWindowsFixRecommended" : "DpiStatusWindowsNoFixNeeded"),
                    ScalingPercent, CurrentDpi),
                HighDpiFix.Host.WineProton => Lang.Get("DpiStatusWineProton"),
                _                          => Lang.Get("DpiStatusNativeNonWindows"),
            };
        }
        if (DpiAppliedStatus != null)
        {
            if (DpiHost != HighDpiFix.Host.Windows)
            {
                DpiAppliedStatus.Value = Lang.Get("DpiAppliedNotApplicable");
            }
            else
            {
                var reg = Lang.Get(HighDpiFix.IsRegistryFlagSet() ? "DpiYes" : "DpiNo");
                var man = Lang.Get(HighDpiFix.IsManifestPresent() ? "DpiYes" : "DpiNo");
                DpiAppliedStatus.Value = string.Format(Lang.Get("DpiAppliedFormat"), reg, man);
            }
        }
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

        Ultrawide = Config.Bind(UltrawideSection, "Ultrawide", ScreenIsUltrawide, new ConfigDescription("Enable or disable ultrawide support. You must restart the game after changing this setting.", null, new ConfigurationManagerAttributes {Order = 7}));

        CraftIconAboveStations = Config.Bind(ScaleSection, "Interaction Bubble Scale", 1f, new ConfigDescription("Changes the scale of the icons that appear above crafting stations and interaction icons.", new AcceptableValueRange<float>(0.1f, 10f), new ConfigurationManagerAttributes {Order = 6}));
        CraftIconAboveStations.SettingChanged += (_, _) =>
        {
            if (!MainGame.game_started) return;
            UpdateCraftIconScale(CraftIconAboveStations.Value);
        };
        HudScale = Config.Bind(ScaleSection, "HUD Scale", 1f, new ConfigDescription("Changes the scale of the HUD.", new AcceptableValueRange<float>(0.1f, 10f), new ConfigurationManagerAttributes {Order = 5}));
        HudScale.SettingChanged += (_, _) =>
        {
            if (!MainGame.game_started) return;
            if (Patches.HUD != null) Patches.HUD.transform.localScale = new Vector3(HudScale.Value, HudScale.Value, 1);
        };

        HorizontalHudPosition = Config.Bind(PositionsSection, "Horizontal HUD Position", 1f, new ConfigDescription("Changes the horizontal position of the HUD.", new AcceptableValueRange<float>(-5, 5), new ConfigurationManagerAttributes {Order = 4}));
        HorizontalHudPosition.SettingChanged += (_, _) =>
        {
            if (!MainGame.game_started) return;
            if (Patches.ScreenSize != null) Patches.ScreenSize.transform.localScale = new Vector3(HorizontalHudPosition.Value, VerticalHudPosition.Value, 1);
        };

        VerticalHudPosition = Config.Bind(PositionsSection, "Vertical HUD Position", 1f, new ConfigDescription("Changes the vertical position of the HUD.", new AcceptableValueRange<float>(-5, 5), new ConfigurationManagerAttributes {Order = 3}));
        VerticalHudPosition.SettingChanged += (_, _) =>
        {
            if (!MainGame.game_started) return;
            if (Patches.ScreenSize != null) Patches.ScreenSize.transform.localScale = new Vector3(HorizontalHudPosition.Value, VerticalHudPosition.Value, 1);
        };


        Zoom = Config.Bind(ZoomSection, "Zoom", 0f, new ConfigDescription("Zoom", new AcceptableValueRange<float>(min + 10, defaultZoom * 2), new ConfigurationManagerAttributes {Order = 2}));
        Zoom.SettingChanged += (_, _) =>
        {
            if (!MainGame.game_started) return;
            Camera.main!.orthographicSize = defaultZoom + Zoom.Value;
        };

        ZoomIn = Config.Bind(ZoomSection, "Zoom In", new KeyboardShortcut(KeyCode.KeypadPlus), new ConfigDescription("Zoom In", null, new ConfigurationManagerAttributes {Order = 1}));
        ZoomOut = Config.Bind(ZoomSection, "Zoom Out", new KeyboardShortcut(KeyCode.KeypadMinus), new ConfigDescription("Zoom Out", null, new ConfigurationManagerAttributes {Order = 0}));

        RemoveFog = Config.Bind(FogSection, "Remove Fog", true, new ConfigDescription("Remove fog from the game.", null, new ConfigurationManagerAttributes {Order = 0}));
        ColorCorrection = Config.Bind(ColorCorrectionSection, "Color Correction", true, new ConfigDescription("Enable or disable color correction.", null, new ConfigurationManagerAttributes {Order = 0}));
        ColorCorrection.SettingChanged += (_, _) => UpdateCC();

        BorderlessWindowed = Config.Bind(DisplaySection, "Borderless Windowed", false, new ConfigDescription("Run the game in borderless windowed mode instead of fullscreen. Restart required.", null, new ConfigurationManagerAttributes {Order = 1}));

        SetVsyncLimitToMaxRefreshRate = Config.Bind(DisplaySection, "Set Vsync Limit To Max Refresh Rate", true, new ConfigDescription("Set Vsync limit to the maximum refresh rate of the monitor. Game default is 60fps with VSYNC enabled.", null, new ConfigurationManagerAttributes {Order = 0}));

        DpiDetectedStatus = Config.Bind(HighDpiSection, "Detected display scaling", "",
            new ConfigDescription(
                "How Windows is scaling your display. 100% means no fix needed; anything higher (common at 4K) benefits from the fix below.",
                null,
                new ConfigurationManagerAttributes { Order = 100, ReadOnly = true, HideDefaultButton = true }));

        DpiAppliedStatus = Config.Bind(HighDpiSection, "Fix status", "",
            new ConfigDescription(
                "Whether the Windows compatibility flag and sidecar manifest are currently in place for Graveyard Keeper.exe.",
                null,
                new ConfigurationManagerAttributes { Order = 99, ReadOnly = true, HideDefaultButton = true }));

        ApplyDpiFix = Config.Bind(HighDpiSection, "Apply high-DPI fix", false,
            new ConfigDescription(
                "Tick to mark Graveyard Keeper as high-DPI aware so it renders sharply at 4K / scaled displays. " +
                "Writes a Windows compatibility flag to your user registry and a sidecar manifest next to the game. " +
                "Restart the game afterwards. Un-tick to remove both.",
                null,
                new ConfigurationManagerAttributes { Order = 98 }));
        ApplyDpiFix.SettingChanged += (_, _) => OnApplyDpiFixToggled();

        AskDpiFixAtStartup = Config.Bind(HighDpiSection, "Offer the fix at startup", true,
            new ConfigDescription(
                "When on, the mod will detect high-DPI scaling at startup and offer to apply the fix via a main-menu dialog. " +
                "Turn off to silence the prompt (useful once you've decided either way).",
                null,
                new ConfigurationManagerAttributes { Order = 97 }));

        CheckForUpdates = Config.Bind(UpdatesSection, "Check for Updates", true,
            new ConfigDescription(
                "Show a notice on the main menu when a newer version of this mod is available on NexusMods. Click the notice to open the mod's page.",
                null,
                new ConfigurationManagerAttributes { Order = 0 }));

        SceneManager.sceneLoaded += (_, _) => UpdateCC();
    }

    // Called from the SettingChanged handler AND from the main-menu dialog Yes/No callbacks.
    // Mirrors the toggle state to the actual Windows flag + manifest files.
    private static bool _applyingDpiFix;
    internal static void OnApplyDpiFixToggled()
    {
        if (_applyingDpiFix) return; // prevent re-entry when we write-back from the dialog path
        _applyingDpiFix = true;
        try
        {
            if (DpiHost != HighDpiFix.Host.Windows)
            {
                ShowDialog(Lang.Get(DpiHost == HighDpiFix.Host.WineProton
                    ? "DpiWineProtonDialog"
                    : "DpiNativeNonWindowsDialog"));
                ApplyDpiFix.Value = false; // rollback — nothing was actually applied
                return;
            }

            if (ApplyDpiFix.Value)
            {
                var result = HighDpiFix.Apply();
                RefreshDpiStatus();
                var key = result.FullSuccess
                    ? "DpiApplyFullSuccess"
                    : result.AnySuccess
                        ? "DpiApplyPartialSuccess"
                        : "DpiApplyFailed";
                ShowDialog(Lang.Get(key));
                if (!result.AnySuccess)
                {
                    ApplyDpiFix.Value = false; // rollback toggle state — nothing was applied
                }
            }
            else
            {
                HighDpiFix.Remove();
                RefreshDpiStatus();
                ShowDialog(Lang.Get("DpiRemoved"));
            }
        }
        finally
        {
            _applyingDpiFix = false;
        }
    }

    private static void ShowDialog(string message)
    {
        try
        {
            if (GUIElements.me?.dialog != null && MainGame.game_started)
            {
                GUIElements.me.dialog.OpenOK(MyPluginInfo.PLUGIN_NAME, null, message, true, string.Empty);
            }
            else
            {
                Log.LogInfo($"[HighDpiFix] {message}");
            }
        }
        catch (Exception ex)
        {
            Log.LogWarning($"[HighDpiFix] Dialog failed: {ex.Message}. Message was: {message}");
        }
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