namespace AnAlchemicalCollection;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.potionpermit.alchemical_collection";
    private const string PluginName = "An Alchemical Collection";
    private const string PluginVersion = "0.2.0";
    private const string Preload = "PreLoad";
    private const string MainMenu = "MainMenu";


    private static readonly Harmony Harmony = new(PluginGuid);

    public static Resolution Resolution = new()
    {
        width = Display.main.systemWidth,
        height = Display.main.systemHeight,
        refreshRate = MaxRefreshRate
    };

    private static readonly List<CinemachineVirtualCamera> VirtualCameras = [];
    private static readonly Dictionary<string, float> OriginalCameraZoomValues = new();
    private static ManualLogSource Log { get; set; }
    public static ConfigEntry<float> RunSpeedMultiplier { get; private set; }
    public static ConfigEntry<bool> EnableRunSpeedMultiplier { get; private set; }
    public static ConfigEntry<float> LeftRightRunSpeedMultiplier { get; private set; }
    public static ConfigEntry<bool> SpeedUpMenuIntro { get; private set; }
    public static ConfigEntry<bool> AutoChangeTool { get; private set; }
    public static ConfigEntry<bool> HalveToolStaminaUsage { get; private set; }
    public static ConfigEntry<bool> SkipLogos { get; private set; }
    private static ConfigEntry<bool> SaveOnExitWithF11 { get; set; }

    public static ConfigEntry<bool> ModifyResolutions { get; private set; }
    public static ConfigEntry<bool> CustomTargetFramerate { get; private set; }
    private static ConfigEntry<int> Width { get; set; }
    private static ConfigEntry<int> Height { get; set; }
    private static ConfigEntry<int> Refresh { get; set; }
    public static ConfigEntry<int> FrameRate { get; private set; }
    private static ConfigEntry<KeyboardShortcut> ExitKeybind { get; set; }
    private static ConfigEntry<KeyboardShortcut> FastTravelKeybind { get; set; }
    private static ConfigEntry<KeyboardShortcut> QuickSaveKeybind { get; set; }
    private static ConfigEntry<KeyboardShortcut> NewsBoardKeybind { get; set; }
    private static ConfigEntry<KeyboardShortcut> ToggleHudKeybind { get; set; }
    internal static ConfigEntry<bool> ModifyCamera { get; private set; }
    private static ConfigEntry<float> CameraZoomValue { get; set; }
    private static ConfigEntry<bool> TimeManipulation { get; set; }
    internal static ConfigEntry<float> TimeMultiplier { get; private set; }
    internal static ConfigEntry<bool> CharacterBob { get; private set; }
    private static ConfigEntry<bool> IncreaseUpdateRate { get; set; }
    private static ConfigEntry<int> IncreaseUpdateRateValue { get; set; }

    private static ConfigEntry<bool> ModifyHudScale { get; set; }
    private static ConfigEntry<float> ModifyHudScaleValue { get; set; }

    internal static ConfigEntry<bool> FasterDialogue { get; private set; }
    internal static ConfigEntry<bool> InstantDialogue { get; private set; }
    internal static ConfigEntry<bool> DialogueTypeWriterSound { get; private set; }
    internal static ConfigEntry<bool> DogBarkingSound { get; private set; }
    internal static ConfigEntry<bool> DockStompingSoundWhenFishing { get; private set; }
    private static ConfigEntry<bool> CleanUpMainMenuConfig { get; set; }
    internal static ConfigEntry<bool> AutoLoadSave { get; private set; }
    internal static ConfigEntry<int> AutoLoadSaveSlot { get; private set; }
    private static ConfigEntry<KeyboardShortcut> SkipAutoLoadMostRecentSaveShortcut { get; set; }
    private static TimePatches TimeInstance { get; set; }
    private static int MaxRefreshRate => Screen.resolutions.Max(a => a.refreshRate);


    private void Awake()
    {
        Log = Logger;
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        TimeInstance = gameObject.AddComponent<TimePatches>();

        // Display Resolution Configuration
        ModifyResolutions = Config.Bind("01. Display Settings", "Enable Custom Resolution", false,
            new ConfigDescription("Toggle the usage of custom resolution settings. You need to restart the game after changing this setting.", null,
                new ConfigurationManagerAttributes {Order = 101}));
        Width = Config.Bind("01. Display Settings", "Custom Width", Display.main.systemWidth,
            new ConfigDescription("Define the custom display width.", null,
                new ConfigurationManagerAttributes {Order = 100}));
        Height = Config.Bind("01. Display Settings", "Custom Height", Display.main.systemHeight,
            new ConfigDescription("Define the custom display height.", null,
                new ConfigurationManagerAttributes {Order = 99}));
        Refresh = Config.Bind("01. Display Settings", "Custom Refresh Rate", Screen.resolutions.Max(a => a.refreshRate),
            new ConfigDescription("Define the custom display refresh rate.", null,
                new ConfigurationManagerAttributes {Order = 98}));
        
        Resolution.height = Height.Value;
        Resolution.width = Width.Value;
        Resolution.refreshRate = Refresh.Value;
        
        CustomTargetFramerate = Config.Bind("01. Display Settings", "Enable Custom Target Frame Rate", false,
            new ConfigDescription("Toggle the usage of custom target frame rate settings. May or may not do anything.",
                null, new ConfigurationManagerAttributes {Order = 97}));
        FrameRate = Config.Bind("01. Display Settings", "Target Frame Rate", Screen.resolutions.Max(a => a.refreshRate),
            new ConfigDescription("Set the target frame rate.", null, new ConfigurationManagerAttributes {Order = 96}));

        // Tool Usage Configuration
        AutoChangeTool = Config.Bind("02. Tools Settings", "Automatic Tool Switching", true,
            new ConfigDescription("Enable the automatic tool switching based on context.", null,
                new ConfigurationManagerAttributes {Order = 90}));
        HalveToolStaminaUsage = Config.Bind("02. Tools Settings", "Halve Stamina Usage", true,
            new ConfigDescription("Enable the halving of stamina usage for tools.", null,
                new ConfigurationManagerAttributes {Order = 89}));

        // Game Intro Configuration
        SkipLogos = Config.Bind("03. Intro Settings", "Skip Intro Logos", true,
            new ConfigDescription("Enable or disable the intro logos.", null,
                new ConfigurationManagerAttributes {Order = 80}));
        SpeedUpMenuIntro = Config.Bind("03. Intro Settings", "Accelerate Menu Intro", true,
            new ConfigDescription("Enable or disable the acceleration of menu intro.", null,
                new ConfigurationManagerAttributes {Order = 79}));

        // Saving Configuration
        SaveOnExitWithF11 = Config.Bind("04. Save Settings", "Save On Quick Exit", true,
            new ConfigDescription("Enable saving the game on quick exit.", null,
                new ConfigurationManagerAttributes {Order = 70}));
        AutoLoadSave = Config.Bind("04. Save Settings", "Auto Load Specified Save", false,
            new ConfigDescription("Enable auto loading the most recent save.", null,
                new ConfigurationManagerAttributes {Order = 69}));
        AutoLoadSaveSlot = Config.Bind("04. Save Settings", "Auto Load Save Slot", 1,
            new ConfigDescription("Set the save slot to auto load.", new AcceptableValueList<int>(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11),
                new ConfigurationManagerAttributes {Order = 68}));
        SkipAutoLoadMostRecentSaveShortcut = Config.Bind("04. Save Settings", "Skip Auto Load Most Recent Save Shortcut", new KeyboardShortcut(KeyCode.LeftShift),
            new ConfigDescription("Set the key to skip the auto load most recent save. Hold down during start-up.", null,
                new ConfigurationManagerAttributes {Order = 67}));

        // Player Speed Configuration
        EnableRunSpeedMultiplier = Config.Bind("05. Player Speed", "Modify Run Speed", true,
            new ConfigDescription("Enable the modification of player run speed.", null,
                new ConfigurationManagerAttributes {Order = 60}));
        RunSpeedMultiplier = Config.Bind("05. Player Speed", "Run Speed Multiplier", 1.5f,
            new ConfigDescription("Set the player run speed multiplier.", null,
                new ConfigurationManagerAttributes {Order = 59}));
        LeftRightRunSpeedMultiplier = Config.Bind("05. Player Speed", "Lateral Run Speed Multiplier", 1.25f,
            new ConfigDescription("Set the lateral run speed multiplier.", null,
                new ConfigurationManagerAttributes {Order = 58}));

        // Keybinding Configuration
        ExitKeybind = Config.Bind("06. Keybinding", "Quick Exit Key", new KeyboardShortcut(KeyCode.F11),
            new ConfigDescription("Set the key for quick exit.", null,
                new ConfigurationManagerAttributes {Order = 50}));
        FastTravelKeybind = Config.Bind("06. Keybinding", "Fast Travel Key", new KeyboardShortcut(KeyCode.F4),
            new ConfigDescription("Set the key for fast travel.", null,
                new ConfigurationManagerAttributes {Order = 49}));
        QuickSaveKeybind = Config.Bind("06. Keybinding", "Quick Save Key", new KeyboardShortcut(KeyCode.F5),
            new ConfigDescription("Set the key for quick save.", null,
                new ConfigurationManagerAttributes {Order = 48}));
        NewsBoardKeybind = Config.Bind("06. Keybinding", "News Board Toggle Key", new KeyboardShortcut(KeyCode.F6),
            new ConfigDescription("Set the key to toggle the news board.", null,
                new ConfigurationManagerAttributes {Order = 47}));
        ToggleHudKeybind = Config.Bind("06. Keybinding", "HUD Toggle Key", new KeyboardShortcut(KeyCode.F7),
            new ConfigDescription("Set the key to toggle the HUD.", null,
                new ConfigurationManagerAttributes {Order = 46}));

        //Camera manipulation
        ModifyCamera = Config.Bind("07. Camera Manipulation", "Enable Camera Manipulation", true,
            new ConfigDescription("Enable camera manipulation.", null,
                new ConfigurationManagerAttributes {Order = 45}));
        ModifyCamera.SettingChanged += (_, _) =>
        {
            UpdateCameraZoom(!ModifyCamera.Value);
        };
        CameraZoomValue = Config.Bind("07. Camera Manipulation", "Camera Zoom", 1f,
            new ConfigDescription("Set the camera zoom.", new AcceptableValueRange<float>(0.1f, 3f),
                new ConfigurationManagerAttributes {ShowRangeAsPercent = false, Order = 44}));
        CameraZoomValue.SettingChanged += (_, _) =>
        {
            UpdateCameraZoom();
        };


        //Scale manipulation
        ModifyHudScale = Config.Bind("08. Scale Manipulation", "Enable Scale Manipulation", true,
            new ConfigDescription("Enable scale manipulation.", null, new ConfigurationManagerAttributes {Order = 43}));
        ModifyHudScale.SettingChanged += (_, _) =>
        {
            ModifyHud(!ModifyHudScale.Value);
        };
        ModifyHudScaleValue = Config.Bind("08. Scale Manipulation", "HUD Scale", 1.0f,
            new ConfigDescription("Set the HUD scale.", new AcceptableValueRange<float>(0.5f, 2.0f),
                new ConfigurationManagerAttributes {ShowRangeAsPercent = false, Order = 42}));
        ModifyHudScaleValue.SettingChanged += (_, _) =>
        {
            ModifyHud();
        };

        //Time manipulation
        TimeManipulation = Config.Bind("09. Time Manipulation", "Enable Time Manipulation", true,
            new ConfigDescription("Enable time manipulation.", null, new ConfigurationManagerAttributes {Order = 43}));
        TimeManipulation.SettingChanged += (_, _) =>
        {
            TimeInstance.enabled = TimeManipulation.Value;
        };
        TimeMultiplier = Config.Bind("09. Time Manipulation", "Time Multiplier", 1.0f,
            new ConfigDescription("Set the time multiplier.", new AcceptableValueRange<float>(1, 10),
                new ConfigurationManagerAttributes {ShowRangeAsPercent = false, Order = 41}));
        TimeMultiplier.SettingChanged += (_, _) =>
        {
            if (!TimeManipulation.Value) return;
            TimeInstance.UpdateValues();
        };


        //Dialog
        FasterDialogue = Config.Bind("10. Dialogue", "Faster Dialogue", true, new ConfigDescription("Enable faster dialogue when talking to NPC's.", null, new ConfigurationManagerAttributes {Order = 30}));
        FasterDialogue.SettingChanged += (_, _) =>
        {
            if (FasterDialogue.Value)
            {
                InstantDialogue.Value = false;
            }
        };

        InstantDialogue = Config.Bind("10. Dialogue", "Instant Dialogue", false, new ConfigDescription("Enable instant dialogue when talking to NPC's.", null, new ConfigurationManagerAttributes {Order = 29}));
        InstantDialogue.SettingChanged += (_, _) =>
        {
            if (InstantDialogue.Value)
            {
                FasterDialogue.Value = false;
            }
        };


        //Sound
        DialogueTypeWriterSound = Config.Bind("11. Sound", "Dialogue TypeWriter Sound", true, new ConfigDescription("Toggle the dialogue typewriter sound when NPC's are 'speaking'.", null, new ConfigurationManagerAttributes {Order = 28}));
        DogBarkingSound = Config.Bind("11. Sound", "Dog Barking Sound", true, new ConfigDescription("Toggle the dog barking sound (and associated animation).", null, new ConfigurationManagerAttributes {Order = 20}));
        DockStompingSoundWhenFishing = Config.Bind("11. Sound", "Dock Stomping Sound When Fishing", true, new ConfigDescription("Toggle the dock stomping sound when fishing. Note, this is tied with the fishing splash sound.", null, new ConfigurationManagerAttributes {Order = 19}));

        //Misc
        CleanUpMainMenuConfig = Config.Bind("12. Misc", "Clean Up Main Menu", true,
            new ConfigDescription("Remove Credits/Discord/Trademark buttons.", null,
                new ConfigurationManagerAttributes {Order = 41}));
        CleanUpMainMenuConfig.SettingChanged += (_, _) =>
        {
            CleanUpMainMenu(!CleanUpMainMenuConfig.Value);
        };
        IncreaseUpdateRate = Config.Bind("12. Misc", "Enable Increase Update Rate", true,
            new ConfigDescription("Enable the increase of the update rate.", null,
                new ConfigurationManagerAttributes {Order = 40}));
        IncreaseUpdateRate.SettingChanged += (_, _) =>
        {
            UpdateFixedDeltaTime();
        };
        IncreaseUpdateRateValue = Config.Bind("12. Misc", "Increase Update Rate",
            Helper.CalculateLowestMultiplierAbove50(MaxRefreshRate),
            new ConfigDescription(
                "Sets the rate the camera and physics update. Can resolve camera judder, but setting too high can cause performance issues. Game default is 50fps. Ideally it should be a multiple of your refresh rate. You may/may not notice a difference.",
                null,
                new ConfigurationManagerAttributes {ShowRangeAsPercent = false, Order = 40}));
        IncreaseUpdateRateValue.SettingChanged += (_, _) =>
        {
            IncreaseUpdateRateValue.Value = Mathf.RoundToInt(IncreaseUpdateRateValue.Value);
            UpdateFixedDeltaTime();
        };
        CharacterBob = Config.Bind("12. Misc", "Character Bob", false,
            new ConfigDescription("Toggle the player and NPC models bobbing up and down when idle. The dog will immediately sit instead of standing and bobbing.", null,
                new ConfigurationManagerAttributes {Order = 39}));
    }


    private void Update()
    {
        if (Input.GetKey(SkipAutoLoadMostRecentSaveShortcut.Value.MainKey) && SceneManager.GetActiveScene().name.Equals(Preload) || SceneManager.GetActiveScene().name.Equals(MainMenu))
        {
            SavePatches.SkipAutoLoad = true;
        }

        if (IsMainMenuActive()) return;

        HandleFastTravel();
        HandleQuickSave();
        HandleNewsBoard();
        HandleToggleHud();
        HandleExit();
    }


    private void OnEnable()
    {
        Harmony.PatchAll(Assembly.GetExecutingAssembly());
        L($"Plugin {PluginName} is loaded!", true);
    }

    private void OnDisable()
    {
        Harmony.UnpatchSelf();
        L($"Plugin {PluginName} is unloaded!");
    }

    private static void UpdateFixedDeltaTime()
    {
        if (!IncreaseUpdateRate.Value) return;
        Time.fixedDeltaTime = 1f / IncreaseUpdateRateValue.Value;
    }

    private static void CleanUpMainMenu(bool restore = false)
    {
        if (!CleanUpMainMenuConfig.Value) return;

        var cleanUpObjects = new[]
        {
            "UI_CAMERA/MAIN_MENU/MAIN_MENU_LAYOUT/Anchor (MiddleCenter)/LIST_CONTAINER/CREDITS_BUTTON",
            "UI_CAMERA/MAIN_MENU/MAIN_MENU_LAYOUT/Anchor (LowerRight)/DISCORD_BUTTON",
            "UI_CAMERA/MAIN_MENU/MAIN_MENU_BG/DEVELOPER_TRADEMARK"
        };

        foreach (var cleanUpObject in cleanUpObjects)
        {
            var cleanUp = GameObject.Find(cleanUpObject);
            if (cleanUp)
            {
                cleanUp.SetActive(restore);
            }
        }
    }


    private static void ModifyHud(bool restore = false)
    {
        if (UIManager.GAME_HUD is null) return;
        UIManager.GAME_HUD.transform.localScale = restore ? new Vector3(1, 1, 1) : new Vector3(ModifyHudScaleValue.Value, ModifyHudScaleValue.Value, 1);
    }

    internal static void UpdateCameraZoom(bool restore = false)
    {
        foreach (var camera in VirtualCameras.Where(a => a is not null))
        {
            var baseZoom = GetOrSetOriginalZoomValue(camera);
            if (restore)
            {
                camera.m_Lens.OrthographicSize = baseZoom;
            }
            else
            {
                camera.m_Lens.OrthographicSize = ModifyCamera.Value
                    ? baseZoom * CameraZoomValue.Value
                    : baseZoom;
            }
        }
    }

    private static float GetOrSetOriginalZoomValue(CinemachineVirtualCamera camera)
    {
        if (OriginalCameraZoomValues.TryGetValue(camera.name, out var zoom))
        {
            return zoom;
        }

        OriginalCameraZoomValues.Add(camera.name, camera.m_Lens.OrthographicSize);
        return camera.m_Lens.OrthographicSize;
    }

    private static void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        RefreshVirtualCameras();
        SetTargetFrameRate();
        SetScreenResolution();
        UpdateFixedDeltaTime();
        ModifyHud();
        UpdateCameraZoom();
        CleanUpMainMenu();
    }

    private static void RefreshVirtualCameras()
    {
        VirtualCameras.Clear();
        VirtualCameras.AddRange(Resources.FindObjectsOfTypeAll<CinemachineVirtualCamera>());

        foreach (var cam in VirtualCameras.Where(cam => !OriginalCameraZoomValues.ContainsKey(cam.name)))
        {
            OriginalCameraZoomValues.Add(cam.name, cam.m_Lens.OrthographicSize);
        }
    }

    private static void SetTargetFrameRate()
    {
        if (CustomTargetFramerate.Value)
        {
            Application.targetFrameRate = FrameRate.Value;
        }
    }

    private static void SetScreenResolution()
    {
        if (ModifyResolutions.Value)
        {
            Screen.SetResolution(Resolution.width, Resolution.height, FullScreenMode.FullScreenWindow, Resolution.refreshRate);
        }
    }

    private static bool IsMainMenuActive()
    {
        return UIManager.MAIN_MENU is not null && UIManager.MAIN_MENU.isActive;
    }

    private void HandleFastTravel()
    {
        if (!FastTravelKeybind.Value.IsUp()) return;
        FastTravelPatches.DoFastTravel = true;
        StartCoroutine(FastTravelIE());
    }

    private static void HandleQuickSave()
    {
        if (!QuickSaveKeybind.Value.IsUp()) return;
        SaveSystemManager.SAVE();
        Helper.ShowNotification("Game Saved!", "Done!");
    }

    private static void HandleNewsBoard()
    {
        if (!NewsBoardKeybind.Value.IsUp() || UIManager.NEWS_BOARD_UI is null) return;
        if (UIManager.NEWS_BOARD_UI.isActive)
        {
            UIManager.NEWS_BOARD_UI.OnRightClick();
        }
        else
        {
            UIManager.NEWS_BOARD_UI.Call();
            UIManager.NEWS_BOARD_UI.RefreshNewsBoard();
        }
    }

    private static void HandleToggleHud()
    {
        if (!ToggleHudKeybind.Value.IsUp() || UIManager.GAME_HUD is null) return;
        if (UIManager.GAME_HUD.active)
        {
            UIManager.GAME_HUD.Hide();
        }
        else
        {
            UIManager.GAME_HUD.Show();
        }

        UIManager.GAME_HUD.topBlackBar.SetActive(false);
        UIManager.GAME_HUD.botBlackBar.SetActive(false);
    }

    private void HandleExit()
    {
        if (ExitKeybind.Value.IsUp())
        {
            StartCoroutine(SaveAndExitIE());
        }
    }


    private static IEnumerator FastTravelIE()
    {
        Helper.ShowNotification("Going home...", "Home!");
        yield return new WaitForSeconds(3f);
        Helper.Teleport(FastTravelID.MC_HOUSE, MapRegion.CITY);
    }

    private static IEnumerator SaveAndExitIE()
    {
        if (SaveOnExitWithF11.Value)
        {
            SaveSystemManager.SAVE();
            Helper.ShowNotification("Game Saved! Exiting...", "Bye!");
        }
        else
        {
            Helper.ShowNotification("Exiting...", "Bye!");
        }

        yield return new WaitForSeconds(2f);
        Application.Quit();
    }

    internal static void L(string message, bool info = false)
    {
        if (info)
        {
            Log.LogInfo(message);
            return;
        }

        Log.LogWarning(message);
    }
}