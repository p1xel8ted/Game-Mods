namespace UIScales;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
[BepInDependency("p1xel8ted.sunhaven.keepalive")]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.sunhaven.uiscales";
    private const string PluginName = "UI Scales";
    private const string PluginVersion = "0.2.1";

    private static ConfigEntry<bool> ZoomAdjustments { get; set; }
    internal static ConfigEntry<float> MainHudScale { get; private set; }
    internal static ConfigEntry<float> ZoomLevel { get; private set; }
    internal static ConfigEntry<bool> Notifications { get; private set; }
    internal static ConfigEntry<KeyboardShortcut> ZoomKeyboardShortcutIncrease { get; private set; }
    internal static ConfigEntry<KeyboardShortcut> ZoomKeyboardShortcutDecrease { get; private set; }

    internal static ManualLogSource LOG { get; set; }
    public static ConfigEntry<float> GenericDialogScale { get; private set; }
    public static ConfigEntry<float> TooltipScale { get; private set; }
    public static ConfigEntry<float> VirtualKeyboardScale { get; private set; }
    public static ConfigEntry<float> ToolbarScale { get; private set; }
    public static ConfigEntry<float> BackpackSettingsScale { get; private set; }
    public static ConfigEntry<float> QuestsScale { get; private set; }
    public static ConfigEntry<float> ItemIconsScale { get; private set; }
    public static ConfigEntry<float> ShopCanvasScale { get; private set; }
    public static ConfigEntry<float> ChatConsoleScale { get; private set; }
    public static ConfigEntry<float> QuestIconScale { get; private set; }
    public static ConfigEntry<float> ChestCanvasScale { get; private set; }
    public static ConfigEntry<float> SnaccoonCanvasScale { get; private set; }
    public static ConfigEntry<float> DoubloonShopCanvasScale { get; private set; }
    public static ConfigEntry<float> CommunityTokenShopCanvasScale { get; private set; }
    private static ConfigEntry<float> AutoFeederCanvasScale { get; set; }
    public static ConfigEntry<float> OptionsMenuScale { get; private set; }
    public static ConfigEntry<float> EverythingElseScale { get; private set; }

    private void Awake()
    {
        SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
        LOG = Logger;
        InitConfig();
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        LOG.LogInfo($"Plugin {PluginName} is loaded!");
    }

    private void Update()
    {
        Utils.UpdateZoomLevel();
    }


    private void InitConfig()
    {
        Notifications = Config.Bind("01. General Settings", "Enable Notifications", true, new ConfigDescription("Enable or disable in-game notifications.", null, new ConfigurationManagerAttributes {Order = 998}));

        MainHudScale = Config.Bind("02. User Interface Scale Settings", "Main HUD", 2f, new ConfigDescription("Weather HUD, Currency HUD - this may effect other things.", new AcceptableValueRange<float>(0.5f, 10f), new ConfigurationManagerAttributes {Order = 995}));
        MainHudScale.SettingChanged += (_, _) =>
        {
            Patches.UpdateAllScalers();
        };

        ChestCanvasScale = Config.Bind("02. User Interface Scale Settings", "Chest Canvas Scale", 2f, new ConfigDescription("Unsure It's called ChestCanvas, but doesn't seem to be linked to anything.", new AcceptableValueRange<float>(0.5f, 10f), new ConfigurationManagerAttributes {Order = 999}));
        ChestCanvasScale.SettingChanged += (_, _) =>
        {
            Patches.UpdateAllScalers();
        };

        SnaccoonCanvasScale = Config.Bind("02. User Interface Scale Settings", "Snaccoon Canvas Scale", 2f, new ConfigDescription("Adjust the scale of the snaccoon canvas.", new AcceptableValueRange<float>(0.5f, 10f), new ConfigurationManagerAttributes {Order = 996}));
        SnaccoonCanvasScale.SettingChanged += (_, _) =>
        {
            Patches.UpdateAllScalers();
        };

        DoubloonShopCanvasScale = Config.Bind("02. User Interface Scale Settings", "Doubloon Shop Canvas Scale", 2f, new ConfigDescription("Adjust the scale of the doubloon shop canvas.", new AcceptableValueRange<float>(0.5f, 10f), new ConfigurationManagerAttributes {Order = 995}));
        DoubloonShopCanvasScale.SettingChanged += (_, _) =>
        {
            Patches.UpdateAllScalers();
        };

        CommunityTokenShopCanvasScale = Config.Bind("02. User Interface Scale Settings", "Community Token Shop Canvas Scale", 2f, new ConfigDescription("Adjust the scale of the community token shop canvas.", new AcceptableValueRange<float>(0.5f, 10f), new ConfigurationManagerAttributes {Order = 994}));
        CommunityTokenShopCanvasScale.SettingChanged += (_, _) =>
        {
            Patches.UpdateAllScalers();
        };

        AutoFeederCanvasScale = Config.Bind("02. User Interface Scale Settings", "Auto Feeder Canvas Scale", 2f, new ConfigDescription("Adjust the scale of the auto feeder canvas.", new AcceptableValueRange<float>(0.5f, 10f), new ConfigurationManagerAttributes {Order = 993}));
        AutoFeederCanvasScale.SettingChanged += (_, _) =>
        {
            Patches.UpdateAllScalers();
        };

        QuestIconScale = Config.Bind("02. User Interface Scale Settings", "Quest Icon Scale", 2f, new ConfigDescription("Unsure", new AcceptableValueRange<float>(0.5f, 10f), new ConfigurationManagerAttributes {Order = 997}));
        QuestIconScale.SettingChanged += (_, _) =>
        {
            Patches.UpdateAllScalers();
        };
        ChatConsoleScale = Config.Bind("02. User Interface Scale Settings", "Chat Scale", 2f, new ConfigDescription("Chat console, cheat window.", new AcceptableValueRange<float>(0.5f, 10f), new ConfigurationManagerAttributes {Order = 996}));
        ChatConsoleScale.SettingChanged += (_, _) =>
        {
            Patches.UpdateAllScalers();
        };

        GenericDialogScale = Config.Bind("02. User Interface Scale Settings", "Generic Dialog Scale", 2f, new ConfigDescription("Chests, NPC Dialog, Crafting UIs", new AcceptableValueRange<float>(0.5f, 10f), new ConfigurationManagerAttributes {Order = 994}));
        GenericDialogScale.SettingChanged += (_, _) =>
        {
            Patches.UpdateAllScalers();
        };
        VirtualKeyboardScale = Config.Bind("02. User Interface Scale Settings", "Virtual Keyboard Scale", 2f, new ConfigDescription("Adjust the scale of the virtual keyboard.", new AcceptableValueRange<float>(0.5f, 10f), new ConfigurationManagerAttributes {Order = 993}));
        VirtualKeyboardScale.SettingChanged += (_, _) =>
        {
            Patches.UpdateAllScalers();
        };
        ToolbarScale = Config.Bind("02. User Interface Scale Settings", "Action Bar Scale", 2f, new ConfigDescription("Actionbar/Toolbar", new AcceptableValueRange<float>(0.5f, 10f), new ConfigurationManagerAttributes {Order = 992}));
        ToolbarScale.SettingChanged += (_, _) =>
        {
            Patches.UpdateAllScalers();
        };
        BackpackSettingsScale = Config.Bind("02. User Interface Scale Settings", "Backpack Scale", 2f, new ConfigDescription("Backpack UI and all the other tabs.", new AcceptableValueRange<float>(0.5f, 10f), new ConfigurationManagerAttributes {Order = 991}));
        BackpackSettingsScale.SettingChanged += (_, _) =>
        {
            Patches.UpdateAllScalers();
        };
        QuestsScale = Config.Bind("02. User Interface Scale Settings", "Quests Scale", 2f, new ConfigDescription("Adjust the scale of the quest tracker UI.", new AcceptableValueRange<float>(0.5f, 10f), new ConfigurationManagerAttributes {Order = 990}));
        QuestsScale.SettingChanged += (_, _) =>
        {
            Patches.UpdateAllScalers();
        };
        ItemIconsScale = Config.Bind("02. User Interface Scale Settings", "Item Icons Scale", 2f, new ConfigDescription("Adjust the scale of item icons.", new AcceptableValueRange<float>(0.5f, 10f), new ConfigurationManagerAttributes {Order = 989}));
        ItemIconsScale.SettingChanged += (_, _) =>
        {
            Patches.UpdateAllScalers();
        };
        TooltipScale = Config.Bind("02. User Interface Scale Settings", "Tooltip Scale", 2f, new ConfigDescription("Adjust the scale of tooltips.", new AcceptableValueRange<float>(0.5f, 10f), new ConfigurationManagerAttributes {Order = 988}));
        TooltipScale.SettingChanged += (_, _) =>
        {
            Patches.UpdateAllScalers();
        };
        OptionsMenuScale = Config.Bind("02. User Interface Scale Settings", "Options Menu Scale", 2f, new ConfigDescription("Adjust the scale of the options menu.", new AcceptableValueRange<float>(0.5f, 10f), new ConfigurationManagerAttributes {Order = 987}));
        OptionsMenuScale.SettingChanged += (_, _) =>
        {
            Patches.UpdateAllScalers();
        };

        ShopCanvasScale = Config.Bind("02. User Interface Scale Settings", "Shop Canvas Scale", 2f, new ConfigDescription("Adjust the scale of the shop canvas.", new AcceptableValueRange<float>(0.5f, 10f), new ConfigurationManagerAttributes {Order = 987}));
        ShopCanvasScale.SettingChanged += (_, _) =>
        {
            Patches.UpdateAllScalers();
        };

        EverythingElseScale = Config.Bind("02. User Interface Scale Settings", "Everything Else Scale", 2f, new ConfigDescription("Adjust the scale of everything else not defined. No idea what it will effect.", new AcceptableValueRange<float>(0.5f, 10f), new ConfigurationManagerAttributes {Order = 986}));
        EverythingElseScale.SettingChanged += (_, _) =>
        {
            Patches.UpdateAllScalers();
        };
            
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


    private void OnDestroy()
    {
        OnDisable();
    }

    private void OnDisable()
    {
        LOG.LogError($"Plugin {PluginName} was disabled/destroyed! Unless you are exiting the game, please install Keep Alive! - https://www.nexusmods.com/sunhaven/mods/31");
    }

    private static void SceneManagerOnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        var adaptiveScalers = Resources.FindObjectsOfTypeAll<AdaptiveUIScale>();
        foreach (var scaler in adaptiveScalers)
        {
            scaler.enabled = false;
        }

        var uiScaler = Resources.FindObjectsOfTypeAll<UIScaler>();
        foreach (var scaler in uiScaler)
        {
            scaler.enabled = false;
        }

        var csAdjustments = Resources.FindObjectsOfTypeAll<CanvasScalerAdjustment>();
        foreach (var scaler in csAdjustments)
        {
            scaler.enabled = false;
        }
    }
}