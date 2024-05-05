using Object = UnityEngine.Object;

namespace UIScales
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public partial class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "p1xel8ted.sunhaven.uiscales";
        private const string PluginName = "UI Scales";
        private const string PluginVersion = "0.1.9";

        internal static ConfigEntry<bool> Debug;
        internal static ConfigEntry<bool> ScaleAdjustments { get; private set; }
        private static ConfigEntry<bool> ZoomAdjustments { get; set; }
        internal static ConfigEntry<float> MainMenuUiScale { get; private set; }
        internal static ConfigEntry<float> InGameUiScale { get; private set; }

        internal static ConfigEntry<float> DateTimeYearScale { get; private set; }
        internal static ConfigEntry<float> ZoomLevel { get; private set; }
        internal static ConfigEntry<float> CheatConsoleScale { get; private set; }
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

        private void Awake()
        {
            SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
            LOG = new ManualLogSource(PluginName);
            BepInEx.Logging.Logger.Sources.Add(LOG);
            InitConfig();
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
            LOG.LogInfo($"Plugin {PluginName} is loaded!");
        }

        private void InitConfig()
        {
            Debug = Config.Bind("01. General", "Debug", false, new ConfigDescription("Toggle debug logging.", null, new ConfigurationManagerAttributes {IsAdvanced = true, Order = 999}));

            Notifications = Config.Bind("01. General", "Enable Notifications", true, new ConfigDescription("Enable notifications.", null, new ConfigurationManagerAttributes {Order = 998}));
            CorrectEndOfDayScreen = Config.Bind("01. General", "Correct End of Day Screen", true, new ConfigDescription("Correct the end of day screen if necessary. Test it out.", null, new ConfigurationManagerAttributes {Order = 997}));

            ScaleAdjustments = Config.Bind("01. Scale", "Enable Scale Adjustments", true, new ConfigDescription("Enable scale adjustments.", null, new ConfigurationManagerAttributes {Order = 997}));
            ScaleAdjustments.SettingChanged += (_, _) =>
            {
                if (!ScaleAdjustments.Value)
                {
                    Utils.ResetCanvasScaleFactors();
                }
            };


            CheatConsoleScale = Config.Bind<float>("01. Scale", "Cheat Console Scale", 3, new ConfigDescription("Cheat console UI scale while in game.", new AcceptableValueRange<float>(0.5f, 10f), new ConfigurationManagerAttributes {Order = 996}));
            CheatConsoleScale.Value = Mathf.Max(CheatConsoleScale.Value, 0.5f);
            CheatConsoleScale.SettingChanged += (_, _) =>
            {
                if (!ScaleAdjustments.Value) return;
                Shared.Utils.ConfigureCanvasScaler(QuantumCanvas, CanvasScaler.ScaleMode.ConstantPixelSize, CheatConsoleScale.Value);
            };

            InGameUiScale = Config.Bind<float>("01. Scale", "Game UI Scale", 3, new ConfigDescription("UI scale while in game.", new AcceptableValueRange<float>(0.5f, 10f), new ConfigurationManagerAttributes {Order = 995}));
            InGameUiScale.Value = Mathf.Max(InGameUiScale.Value, 0.5f);
            InGameUiScale.SettingChanged += (_, _) =>
            {
                if (!ScaleAdjustments.Value) return;
                Shared.Utils.ConfigureCanvasScaler(UIOneCanvas, CanvasScaler.ScaleMode.ConstantPixelSize, InGameUiScale.Value);
                Shared.Utils.ConfigureCanvasScaler(UITwoCanvas, CanvasScaler.ScaleMode.ConstantPixelSize, InGameUiScale.Value);
            };

            DateTimeYearScale = Config.Bind<float>("01. Scale", "Date Time Year Scale", 2, new ConfigDescription("UI scale for the year in the date time display.", new AcceptableValueRange<float>(0.5f, 10f), new ConfigurationManagerAttributes {Order = 994}));
            DateTimeYearScale.SettingChanged += (_, _) =>
            {
                if (!ScaleAdjustments.Value) return;
                ScaleDateTimeYear();
            };
            MainMenuUiScale = Config.Bind<float>("01. Scale", "Main Menu UI Scale", 2, new ConfigDescription("UI scale while at the main menu.", new AcceptableValueRange<float>(0.5f, 10f), new ConfigurationManagerAttributes {Order = 994}));
            MainMenuUiScale.Value = Mathf.Max(MainMenuUiScale.Value, 0.5f);
            MainMenuUiScale.SettingChanged += (_, _) =>
            {
                if (!ScaleAdjustments.Value) return;
                Shared.Utils.ConfigureCanvasScaler(MainMenuCanvas, CanvasScaler.ScaleMode.ConstantPixelSize, MainMenuUiScale.Value);
            };
            UIKeyboardShortcutIncrease = Config.Bind("01. Scale", "UI Scale Increase", new KeyboardShortcut(KeyCode.Keypad8, KeyCode.LeftControl), new ConfigDescription("Keybind to increase the UI scale.", null, new ConfigurationManagerAttributes {Order = 993}));
            UIKeyboardShortcutDecrease = Config.Bind("01. Scale", "UI Scale Decrease", new KeyboardShortcut(KeyCode.Keypad2, KeyCode.LeftControl), new ConfigDescription("Keybind to decrease the UI scale.", null, new ConfigurationManagerAttributes {Order = 992}));


            ZoomAdjustments = Config.Bind("02. Zoom", "Enable Zoom Adjustments", true, new ConfigDescription("Enable zoom adjustments.", null, new ConfigurationManagerAttributes {Order = 991}));
            ZoomLevel = Config.Bind<float>("02. Zoom", "Zoom Level", 2, new ConfigDescription("Zoom level while in game.", new AcceptableValueRange<float>(0.5f, 10f), new ConfigurationManagerAttributes {Order = 990}));
            ZoomLevel.Value = Mathf.Max(ZoomLevel.Value, 0.5f);
            ZoomLevel.SettingChanged += (_, _) =>
            {
                if (!ZoomAdjustments.Value) return;
                if (Player.Instance is null) return;
                Player.Instance.OverrideCameraZoomLevel = false;
                Player.Instance.SetZoom(ZoomLevel.Value, true);
            };
            ZoomKeyboardShortcutIncrease = Config.Bind("02. Zoom", "Zoom Level Increase", new KeyboardShortcut(KeyCode.Keypad8), new ConfigDescription("Keybind to increase the zoom level.", null, new ConfigurationManagerAttributes {Order = 104}));
            ZoomKeyboardShortcutDecrease = Config.Bind("02. Zoom", "Zoom Level Decrease", new KeyboardShortcut(KeyCode.Keypad2), new ConfigDescription("Keybind to decrease the zoom level.", null, new ConfigurationManagerAttributes {Order = 103}));
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

        private static void SceneManagerOnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            UIOneCanvas = GameObject.Find("Manager/UI")?.GetComponent<CanvasScaler>();
            UITwoCanvas = GameObject.Find("Player(Clone)/UI")?.GetComponent<CanvasScaler>();
            QuantumCanvas = GameObject.Find("SharedManager/Quantum Console")?.GetComponent<CanvasScaler>();
            MainMenuCanvas = GameObject.Find("Canvas")?.GetComponent<CanvasScaler>();
            Utils.UpdateCanvasScaleFactors();
           // PrepareDateTimeYearForCustomScales();
        }

        private static RectTransform YearRect { get; set; }
        private static RectTransform DateRect { get; set; }
        private static RectTransform TimeRect { get; set; }

        private static void ScaleDateTimeYear()
        {
            if (YearRect != null)
            {
                YearRect.localScale = new Vector3(DateTimeYearScale.Value, DateTimeYearScale.Value, 1f);
            }
            if (DateRect != null)
            {
                DateRect.localScale = new Vector3(DateTimeYearScale.Value, DateTimeYearScale.Value, 1f);
            }
            if (TimeRect != null)
            {
                TimeRect.localScale = new Vector3(DateTimeYearScale.Value, DateTimeYearScale.Value, 1f);
            }
        }

        private static GameObject DateTimeYearParent { get; set; }

        internal static void PrepareDateTimeYearForCustomScales()
        {
            var parentPanel = GameObject.Find("Manager/UI/NEWTopHUD/Left");
            var newParent  = new GameObject("DateTimeYear");
            var year = GameObject.Find("Manager/UI/NEWTopHUD/Left/Year")?.GetComponent<RectTransform>();
            var date = GameObject.Find("Manager/UI/NEWTopHUD/Left/Date")?.GetComponent<RectTransform>();
            var time = GameObject.Find("Manager/UI/NEWTopHUD/Left/Time")?.GetComponent<RectTransform>();
            // if (year is null || date is null || time is null) return;

            DateTimeYearParent = Instantiate(newParent, parentPanel.transform);
            DateTimeYearParent.TryGetComponent<RectTransform>(out var rect);
            rect ??= DateTimeYearParent.AddComponent<RectTransform>();
            
            year.SetParent(DateTimeYearParent.transform);
            date.SetParent(DateTimeYearParent.transform);
            time.SetParent(DateTimeYearParent.transform);

            DateTimeYearParent.SetActive(true);
            UpdateRect(rect);
            
            return;
            
            void UpdateRect(RectTransform rt)
            {
                var oldPivot = rt.pivot;
                var newPivot = new Vector2(0, 1);
                
                var rtRect = rt.rect;
                var pivotOffset = new Vector2((newPivot.x - oldPivot.x) * rtRect.width,
                    (newPivot.y - oldPivot.y) * rtRect.height);
                
                rt.pivot = newPivot; 
                rt.anchoredPosition += pivotOffset; 
                
                ScaleDateTimeYear();
            }
        }
    }
}