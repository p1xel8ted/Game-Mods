namespace UIScales;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public partial class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.sunhaven.uiscales";
    private const string PluginName = "UI Scales";
    private const string PluginVersion = "0.2.0";

    internal static ConfigEntry<bool> Debug;
    internal static ConfigEntry<bool> ScaleAdjustments { get; private set; }
    private static ConfigEntry<bool> ZoomAdjustments { get; set; }
    internal static ConfigEntry<float> MainMenuUiScale { get; private set; }
    internal static ConfigEntry<float> InGameUiScale { get; private set; }

    // internal static ConfigEntry<float> DateTimeYearScale { get; private set; }
    internal static ConfigEntry<float> ZoomLevel { get; private set; }
    internal static ConfigEntry<float> CheatConsoleScale { get; private set; }
    internal static ConfigEntry<float> PortraitScale { get; private set; }
    internal static ConfigEntry<float> PortraitHorizontalPosition { get; private set; }
    internal static ConfigEntry<bool> Notifications { get; private set; }
    internal static ConfigEntry<bool> CorrectEndOfDayScreen { get; private set; }
    internal static ConfigEntry<KeyboardShortcut> UIKeyboardShortcutIncrease { get; private set; }
    internal static ConfigEntry<KeyboardShortcut> UIKeyboardShortcutDecrease { get; private set; }
    internal static ConfigEntry<KeyboardShortcut> ZoomKeyboardShortcutIncrease { get; private set; }
    internal static ConfigEntry<KeyboardShortcut> ZoomKeyboardShortcutDecrease { get; private set; }
    internal static ManualLogSource LOG { get; private set; }
    [CanBeNull] internal static CanvasScaler UIOneCanvas { get; set; }
    [CanBeNull] internal static CanvasScaler UITwoCanvas { get; set; }
    [CanBeNull] public static CanvasScaler QuantumCanvas { get; set; }
    [CanBeNull] public static CanvasScaler MainMenuCanvas { get; set; }

    internal static Transform Bust { get; set; }
    internal static WriteOnce<float> OriginalPortraitPosition { get; } = new();
    internal static ConfigFile ConfigFile { get; private set; }

    private void Awake()
    {
        ConfigFile = Config;
        SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
        LOG = Logger;
        InitConfig();
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        LOG.LogInfo($"Plugin {PluginName} is loaded!");
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneManagerOnSceneLoaded;
        LOG.LogError($"{PluginName} has been disabled!");
    }


    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneManagerOnSceneLoaded;
        LOG.LogError($"{PluginName} has been destroyed!");
    }

    private void InitConfig()
    {
        // 01. General Settings
        Debug = Config.Bind("01. General Settings", "Debug Mode", false, new ConfigDescription("Toggle debug logging to help diagnose issues.", null, new ConfigurationManagerAttributes {IsAdvanced = true, Order = 999}));

        Notifications = Config.Bind("01. General Settings", "Enable Notifications", true, new ConfigDescription("Enable or disable in-game notifications.", null, new ConfigurationManagerAttributes {Order = 998}));

        CorrectEndOfDayScreen = Config.Bind("01. General Settings", "Correct End of Day Screen", true, new ConfigDescription("Adjust the end-of-day screen if necessary. Test to ensure proper functionality.", null, new ConfigurationManagerAttributes {Order = 997}));

        // 02. User Interface Scale Settings
        ScaleAdjustments = Config.Bind("02. User Interface Scale Settings", "Enable Scale Adjustments", true, new ConfigDescription("Allow modifications to the scale of various UI elements.", null, new ConfigurationManagerAttributes {Order = 996}));
        ScaleAdjustments.SettingChanged += (_, _) =>
        {
            if (!ScaleAdjustments.Value)
            {
                Utils.ResetCanvasScaleFactors();
            }
        };

        InGameUiScale = Config.Bind<float>("02. User Interface Scale Settings", "Game UI Scale", 3, new ConfigDescription("Adjust the scale of the game’s user interface.", new AcceptableValueRange<float>(0.5f, 10f), new ConfigurationManagerAttributes {Order = 995}));
        InGameUiScale.SettingChanged += (_, _) =>
        {
            if (!ScaleAdjustments.Value) return;
            Shared.Utils.ConfigureCanvasScaler(UIOneCanvas, CanvasScaler.ScaleMode.ConstantPixelSize, InGameUiScale.Value);
            Shared.Utils.ConfigureCanvasScaler(UITwoCanvas, CanvasScaler.ScaleMode.ConstantPixelSize, InGameUiScale.Value);
        };

        MainMenuUiScale = Config.Bind<float>("02. User Interface Scale Settings", "Main Menu UI Scale", 2, new ConfigDescription("Adjust the scale of the main menu's user interface.", new AcceptableValueRange<float>(0.5f, 10f), new ConfigurationManagerAttributes {Order = 994}));
        MainMenuUiScale.SettingChanged += (_, _) =>
        {
            if (!ScaleAdjustments.Value) return;
            Shared.Utils.ConfigureCanvasScaler(MainMenuCanvas, CanvasScaler.ScaleMode.ConstantPixelSize, MainMenuUiScale.Value);
        };

        CheatConsoleScale = Config.Bind<float>("02. User Interface Scale Settings", "Cheat Console Scale", 3, new ConfigDescription("Adjust the scale of the cheat console UI.", new AcceptableValueRange<float>(0.5f, 10f), new ConfigurationManagerAttributes {Order = 9933}));
        CheatConsoleScale.SettingChanged += (_, _) =>
        {
            if (!ScaleAdjustments.Value) return;
            Shared.Utils.ConfigureCanvasScaler(QuantumCanvas, CanvasScaler.ScaleMode.ConstantPixelSize, CheatConsoleScale.Value);
        };

        // DateTimeYearScale = Config.Bind<float>("02. User Interface Scale Settings", "Date Time Year Scale", 2, new ConfigDescription("Adjust the scale of the year in the date time display.", new AcceptableValueRange<float>(0.5f, 10f), new ConfigurationManagerAttributes {Order = 992}));
        // DateTimeYearScale.SettingChanged += (_, _) =>
        // {
        //     if (!ScaleAdjustments.Value) return;
        //     ScaleDateTimeYear();
        // };

        UIKeyboardShortcutIncrease = Config.Bind("02. User Interface Scale Settings", "UI Scale Increase", new KeyboardShortcut(KeyCode.Keypad8, KeyCode.LeftControl), new ConfigDescription("Keybind to increase the UI scale.", null, new ConfigurationManagerAttributes {Order = 991}));

        UIKeyboardShortcutDecrease = Config.Bind("02. User Interface Scale Settings", "UI Scale Decrease", new KeyboardShortcut(KeyCode.Keypad2, KeyCode.LeftControl), new ConfigDescription("Keybind to decrease the UI scale.", null, new ConfigurationManagerAttributes {Order = 990}));

        // 03. Character Portrait Settings
        PortraitScale = Config.Bind<float>("03. Character Portrait Settings", "Portrait Scale", 1f, new ConfigDescription("Adjust the size of character portraits in dialogue.", new AcceptableValueRange<float>(0.5f, 10f), new ConfigurationManagerAttributes {Order = 989}));
        PortraitScale.SettingChanged += (_, _) =>
        {
            if (!ScaleAdjustments.Value) return;
            ScalePortrait();
        };

        PortraitHorizontalPosition = Config.Bind<float>("03. Character Portrait Settings", "Portrait Position", 0, new ConfigDescription("Adjust the horizontal position of character portraits in dialogue.", new AcceptableValueRange<float>(-1500f, 1500f), new ConfigurationManagerAttributes {Order = 988}));

        // 04. Zoom Settings
        ZoomAdjustments = Config.Bind("04. Zoom Settings", "Enable Zoom Adjustments", true, new ConfigDescription("Allow modifications to the zoom level.", null, new ConfigurationManagerAttributes {Order = 987}));

        ZoomLevel = Config.Bind<float>("04. Zoom Settings", "Zoom Level", 2, new ConfigDescription("Adjust the zoom level while in game.", new AcceptableValueRange<float>(0.5f, 10f), new ConfigurationManagerAttributes {Order = 986}));
        ZoomLevel.SettingChanged += (_, _) =>
        {
            if (!ZoomAdjustments.Value) return;
            if (Player.Instance is null) return;
            Player.Instance.OverrideCameraZoomLevel = false;
            Player.Instance.SetZoom(ZoomLevel.Value, true);
        };

        ZoomKeyboardShortcutIncrease = Config.Bind("04. Zoom Settings", "Zoom Level Increase", new KeyboardShortcut(KeyCode.Keypad8), new ConfigDescription("Keybind to increase the zoom level.", null, new ConfigurationManagerAttributes {Order = 985}));

        ZoomKeyboardShortcutDecrease = Config.Bind("04. Zoom Settings", "Zoom Level Decrease", new KeyboardShortcut(KeyCode.Keypad2), new ConfigDescription("Keybind to decrease the zoom level.", null, new ConfigurationManagerAttributes {Order = 984}));
    }

    internal static void MovePortrait()
    {
        if (Bust)
        {
            Bust.localPosition = Bust.localPosition with {x = OriginalPortraitPosition.Value + PortraitHorizontalPosition.Value};
        }
    }
    private static void ScalePortrait()
    {
        if (Bust)
        {
            ScaleTransformWithBottomLeftPivot(Bust, new Vector3(PortraitScale.Value, PortraitScale.Value, 1f));
        }
    }

    private static void SceneManagerOnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        UIOneCanvas = GameObject.Find("Manager/UI")?.GetComponent<CanvasScaler>();
        UITwoCanvas = GameObject.Find("Player(Clone)/UI")?.GetComponent<CanvasScaler>();
        QuantumCanvas = GameObject.Find("SharedManager/Quantum Console")?.GetComponent<CanvasScaler>();
        MainMenuCanvas = GameObject.Find("Canvas")?.GetComponent<CanvasScaler>();
        Utils.UpdateCanvasScaleFactors();
    }

    // private static RectTransform YearRect { get; set; }
    // private static RectTransform DateRect { get; set; }
    // private static RectTransform TimeRect { get; set; }
    //
    // private static void ScaleDateTimeYear()
    // {
    //     var newDateTime = GameObject.Find("Manager/UI/NEWTopHUD/Left/DateTimeYear(Clone)");
    //     ScaleTransformWithTopLeftPivot(newDateTime.transform, new Vector3(DateTimeYearScale.Value, DateTimeYearScale.Value, 1));
    // }

    // // Method to scale a transform while keeping its top-left corner fixed
    // public static void ScaleTransformWithTopLeftPivot(Transform targetTransform, Vector3 newScale)
    // {
    //     // Store the initial position and scale
    //     var initialPosition = targetTransform.position;
    //     var initialScale = targetTransform.localScale;
    //
    //     // Adjust initial position to set pivot to top-left
    //     var topLeftOffset = new Vector3(initialScale.x / 2, -initialScale.y / 2, 0);
    //     targetTransform.position -= topLeftOffset;
    //
    //     // Calculate the scale change
    //     var scaleChange = newScale - initialScale;
    //
    //     // Adjust position to keep the top-left corner in place
    //     var positionAdjustment = new Vector3(scaleChange.x / 2, -scaleChange.y / 2, 0);
    //     targetTransform.localScale = newScale;
    //     targetTransform.position = initialPosition - positionAdjustment;
    // }

    internal static void ScaleTransformWithBottomLeftPivot(Transform targetTransform, Vector3 newScale)
    {
        var initialPosition = targetTransform.position;
        var initialScale = targetTransform.localScale;

        var positionAdjustment = new Vector3(
            (initialScale.x - newScale.x) * 0.5f,
            (initialScale.y - newScale.y) * 0.5f,
            0
        );

        targetTransform.localScale = newScale;
        targetTransform.position = initialPosition + positionAdjustment;
    }

    // private static GameObject DateTimeYearParent { get; set; }

    // internal static void PrepareDateTimeYearForCustomScales()
    // {
    //     var parentPanel = GameObject.Find("Manager/UI/NEWTopHUD/Left");
    //     var newParent = new GameObject("DateTimeYear");
    //     var year = parentPanel.transform.Find("Year")?.GetComponent<RectTransform>();
    //     var date = parentPanel.transform.Find("Date")?.GetComponent<RectTransform>();
    //     var time = parentPanel.transform.Find("Time")?.GetComponent<RectTransform>();
    //
    //     DateTimeYearParent = Instantiate(newParent, parentPanel.transform);
    //     if (!DateTimeYearParent.TryGetComponent<RectTransform>(out var rect))
    //     {
    //         rect = DateTimeYearParent.AddComponent<RectTransform>();
    //     }
    //
    //     year.SetParent(DateTimeYearParent.transform);
    //     date.SetParent(DateTimeYearParent.transform);
    //     time.SetParent(DateTimeYearParent.transform);
    //
    //     DateTimeYearParent.SetActive(true);
    //
    //     var rects = DateTimeYearParent.GetComponentsInChildren<RectTransform>();
    //     rects.AddToArray(rect);
    //
    //     foreach (var r in rects)
    //     {
    //         r.anchorMin = new Vector2(0, 1);
    //         r.anchorMax = new Vector2(0, 1);
    //         r.pivot = new Vector2(0, 1);
    //     }
    // }
}