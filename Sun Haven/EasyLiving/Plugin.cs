namespace EasyLiving;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
[BepInDependency("p1xel8ted.sunhaven.keepalive")]
public partial class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.sunhaven.easyliving";
    private const string PluginName = "Easy Living";
    private const string PluginVersion = "0.1.8";
    private static ConfigEntry<KeyboardShortcut> SaveShortcut { get; set; }
    private static ConfigEntry<bool> EnableSaveShortcut { get; set; }
    public static ConfigEntry<bool> SkipMuseumMissingItemsDialogue { get; private set; }
    private static ConfigEntry<bool> UnityLogging { get; set; }
    public static ConfigEntry<bool> RemoveSocialMediaButtons { get; private set; }
    public static ConfigEntry<bool> AddQuitToDesktopButton { get; private set; }
    public static ConfigEntry<bool> EnableAdjustQuestTrackerHeightView { get; private set; }
    public static ConfigEntry<bool> AutoLoadMostRecentSave { get; private set; }
    private static ConfigEntry<KeyboardShortcut> SkipAutoLoadMostRecentSaveShortcut { get; set; }
    public static ConfigEntry<int> AdjustQuestTrackerHeightView { get; private set; }
    public static ConfigEntry<bool> ApplyMoveSpeedMultiplier { get; private set; }
    public static ConfigEntry<float> MoveSpeedMultiplier { get; private set; }
    private static ConfigEntry<KeyboardShortcut> MoveSpeedMultiplierIncrease { get; set; }
    private static ConfigEntry<KeyboardShortcut> MoveSpeedMultiplierDecrease { get; set; }

    public static ConfigEntry<bool> MaterialsOnlyDefault { get; private set; }
    // public static ConfigEntry<bool> IncreaseWateringCanFillRange { get; private set; }

    public static ConfigEntry<bool> PlayerStartsAwayFromMailbox { get; private set; }
    public static ConfigEntry<bool> Emotes { get; private set; }
    public static ConfigEntry<bool> FastDialogue { get; set; }
    public static ConfigEntry<bool> RemoveDlcAds { get; set; }
    internal static ConfigEntry<bool> ResolutionChange { get; private set; }
    internal static ManualLogSource LOG { get; private set; }
    public static ConfigEntry<bool> RemovePatchNotes { get; set; }
    public static ConfigEntry<bool> RemovePixelSproutStudiosLogo { get; set; }
    public static ConfigEntry<bool> RemoveDlcShopButton { get; set; }
    public static ConfigEntry<bool> RemoveMenuButtonsBorder { get; set; }
    public static ConfigEntry<bool> MuteInBackground { get; set; }

    public static ConfigEntry<bool> QuickContinueButton { get; set; }

    internal static ConfigEntry<bool> QuickBoot { get; set; }

    private void Awake()
    {
        LOG = Logger;

        SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;

        UnityLogging = Config.Bind("01. Debug", "Unity Logging", false, new ConfigDescription("Toggle Unity logging. Useful for debugging.", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = -1 }));
        UnityLogging.SettingChanged += (_, _) => { Debug.unityLogger.logEnabled = UnityLogging.Value; };
        Debug.unityLogger.logEnabled = UnityLogging.Value;

        EnableSaveShortcut = Config.Bind("02. Keyboard Shortcuts", "Enable Quick Save", true, new ConfigDescription("Enable quick saving via the keybind below.", null, new ConfigurationManagerAttributes { Order = 25 }));
        SaveShortcut = Config.Bind("02. Keyboard Shortcuts", "Quick Save", new KeyboardShortcut(KeyCode.F5), new ConfigDescription("Keybind to press to manual save game. Note that it doesn't save location, just progress.", null, new ConfigurationManagerAttributes { Order = 24 }));
        SkipMuseumMissingItemsDialogue = Config.Bind("03. Museum", "Skip Missing Items Dialogue", true, new ConfigDescription("Skip the 'missing items' dialogue when you interact with a museum display.", null, new ConfigurationManagerAttributes { Order = 23 }));

        RemoveMenuButtonsBorder = Config.Bind("04. UI", "Remove Menu Buttons Border", false, new ConfigDescription("Remove the border around the main menu buttons.", null, new ConfigurationManagerAttributes { Order = 22 }));
        RemoveMenuButtonsBorder.SettingChanged += (_, _) => { Patches.UpdateMainMenu(); };

        RemoveSocialMediaButtons = Config.Bind("04. UI", "Remove Social Media Buttons In Main Menu", true, new ConfigDescription("Remove the Discord/Twitter/etc buttons from  the main menu.", null, new ConfigurationManagerAttributes { Order = 21 }));
        RemoveSocialMediaButtons.SettingChanged += (_, _) => { Patches.UpdateMainMenu(); };

        RemoveDlcShopButton = Config.Bind("04. UI", "Remove DLC Shop Button", false, new ConfigDescription("Remove the DLC shop button from the main menu.", null, new ConfigurationManagerAttributes { Order = 20 }));
        RemoveDlcShopButton.SettingChanged += (_, _) => { Patches.UpdateMainMenu(); };

        RemoveDlcAds = Config.Bind("04. UI", "Remove DLC Ads", false, new ConfigDescription("Remove the DLC ads from the main menu.", null, new ConfigurationManagerAttributes { Order = 19 }));
        RemoveDlcAds.SettingChanged += (_, _) => { Patches.UpdateMainMenu(); };

        RemovePatchNotes = Config.Bind("04. UI", "Remove Patch Notes", true, new ConfigDescription("Remove the patch notes from the main menu.", null, new ConfigurationManagerAttributes { Order = 18 }));
        RemovePatchNotes.SettingChanged += (_, _) => { Patches.UpdateMainMenu(); };

        RemovePixelSproutStudiosLogo = Config.Bind("04. UI", "Remove Pixel Sprout Studios Logo", true, new ConfigDescription("Remove the PP Studios logo from the main menu.", null, new ConfigurationManagerAttributes { Order = 17 }));
        RemovePixelSproutStudiosLogo.SettingChanged += (_, _) => { Patches.UpdateMainMenu(); };

        QuickContinueButton = Config.Bind("04. UI", "Quick Continue Button", true, new ConfigDescription("Enable a 'Continue' button in the main menu to load last used save.", null, new ConfigurationManagerAttributes { Order = 16 }));
        QuickContinueButton.SettingChanged += (_, _) =>
        {
            if (Utils.DevOpsContinueExists(Patches.DevOpsContinue))
            {
                LOG.LogWarning("DevOps continue button exists, not creating a new one.");
                QuickContinueButton.Value = false;
                return;
            }

            if (MainMenuController.Instance)
            {
                Patches.CreateContinueButton(MainMenuController.Instance);
            }
        };

        AddQuitToDesktopButton = Config.Bind("04. UI", "Add Quit To Desktop Button", true, new ConfigDescription("Add a 'Quit To Desktop' button to the main menu. Bottom right X.", null, new ConfigurationManagerAttributes { Order = 16 }));
        EnableAdjustQuestTrackerHeightView = Config.Bind("04. UI", "Enable Adjust Quest Tracker Height", true, new ConfigDescription("Enable adjusting the height of the quest tracker.", null, new ConfigurationManagerAttributes { Order = 15 }));
        AdjustQuestTrackerHeightView = Config.Bind("04. UI", "Adjust Quest Tracker Height", Display.main.systemHeight / 3, new ConfigDescription("Adjust the height of the quest tracker.", new AcceptableValueRange<int>(-2000, 2000), new ConfigurationManagerAttributes { Order = 14 }));
        ApplyMoveSpeedMultiplier = Config.Bind("05. Player", "Apply Move Speed Multiplier", true, new ConfigDescription("Apply a multiplier to the players base speed.", null, new ConfigurationManagerAttributes { Order = 13 }));
        MoveSpeedMultiplier = Config.Bind("05. Player", "Move Speed Multiplier", 1.5f, new ConfigDescription("Adjust the player's move speed.", new AcceptableValueRange<float>(1f, 10f), new ConfigurationManagerAttributes { Order = 12 }));
        MoveSpeedMultiplier.SettingChanged += (_, _) => { MoveSpeedMultiplier.Value = Mathf.Clamp(MoveSpeedMultiplier.Value, 1f, 10f); };
        MoveSpeedMultiplierIncrease = Config.Bind("05. Player", "Move Speed Multiplier Increase", new KeyboardShortcut(KeyCode.LeftBracket), new ConfigDescription("Keybind to increase the player's move speed multiplier.", null, new ConfigurationManagerAttributes { Order = 11 }));
        MoveSpeedMultiplierDecrease = Config.Bind("05. Player", "Move Speed Multiplier Decrease", new KeyboardShortcut(KeyCode.RightBracket), new ConfigDescription("Keybind to decrease the player's move speed multiplier.", null, new ConfigurationManagerAttributes { Order = 10 }));
        AutoLoadMostRecentSave = Config.Bind("06. Saves", "Auto Load Most Recent Save", false, new ConfigDescription("Automatically load the most recent save when starting the game.", null, new ConfigurationManagerAttributes { Order = 9 }));
        SkipAutoLoadMostRecentSaveShortcut = Config.Bind("06. Saves", "Skip Auto Load Most Recent Save Shortcut", new KeyboardShortcut(KeyCode.LeftShift), new ConfigDescription("Keybind to hold to skip auto loading the most recent save.", null, new ConfigurationManagerAttributes { Order = 8 }));
        MaterialsOnlyDefault = Config.Bind("07. Crafting", "Materials Only Default", true, new ConfigDescription("Set the default crafting filter to 'Materials Only' when opening a crafting table.", null, new ConfigurationManagerAttributes { Order = 7 }));
        // IncreaseWateringCanFillRange = Config.Bind("08. Farming", "Increase Watering Can Fill Range", true, new ConfigDescription("Increase the watering can fill range.", null, new ConfigurationManagerAttributes {Order = 5}));
        Emotes = Config.Bind("09. Player Settings", "Emotes", true, new ConfigDescription("Enable emotes.", null, new ConfigurationManagerAttributes { Order = 4 }));
        Emotes.SettingChanged += (_, _) => { UpdatePlayerPref(); };
        FastDialogue = Config.Bind("09. Player Settings", "Fast Dialogue", true, new ConfigDescription("According to game code increases dialogue speed, but I have my doubts. Try for yourself.", null, new ConfigurationManagerAttributes { Order = 2 }));
        FastDialogue.SettingChanged += (_, _) => { UpdatePlayerPref(); };
        PlayerStartsAwayFromMailbox = Config.Bind("09. Player Settings", "Player Starts Away From Mailbox", true, new ConfigDescription("Player starts away from mailbox. Don't ask.", null, new ConfigurationManagerAttributes { Order = 1 }));
        ResolutionChange = Config.Bind("10. Display", "Resolution", false, new ConfigDescription("Forces a resolution update on scene changes. This is mainly to fix Steams FPS counter being huge on my setup. It will use the Main display, so if you play on your secondary, disable this.", null, new ConfigurationManagerAttributes { Order = 0 }));

        MuteInBackground = Config.Bind("11. Misc", "Mute In Background", false, new ConfigDescription("Mute the game when not focused.", null, new ConfigurationManagerAttributes { Order = -2 }));
        Application.focusChanged += focused =>
        {
            AudioListener.volume = 1;

            if (MuteInBackground.Value)
            {
                AudioListener.volume = focused ? 1 : 0;
            }
        };

        QuickBoot = Config.Bind("11. Misc", "Quick Boot", true, new ConfigDescription("Enable quick booting to the main menu.", null, new ConfigurationManagerAttributes { Order = -3 }));

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        LOG.LogInfo($"Plugin {PluginName} is loaded!");
    }

    private void OnDisable()
    {
        LOG.LogError($"Plugin {PluginName} was disabled/destroyed! Unless you are exiting the game, please install Keep Alive! - https://www.nexusmods.com/sunhaven/mods/31");
    }

    private void OnDestroy()
    {
        OnDisable();
    }

    private static void UpdatePlayerPref()
    {
        if (PlaySettingsManager.Instance && PlaySettingsManager.PlaySettings)
        {
            PlaySettingsManager.PlaySettings.enableEmotes = Emotes.Value;
            PlaySettingsManager.PlaySettings.fastDialogue = FastDialogue.Value;
        }
    }

    private static void SceneManagerOnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        UpdatePlayerPref();
        var maxRefreshRateRatio = Screen.resolutions
            .OrderByDescending(r => (float)r.refreshRateRatio.value)
            .Select(r => r.refreshRateRatio)
            .FirstOrDefault();

        if (ResolutionChange.Value)
        {
            Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, FullScreenMode.FullScreenWindow, maxRefreshRateRatio);
            LOG.LogInfo($"Screen resolution set to {Display.main.systemWidth}x{Display.main.systemHeight} @ {maxRefreshRateRatio}Hz");
        }

        Time.fixedDeltaTime = (float)(1f / maxRefreshRateRatio.value);

        LOG.LogInfo($"FixedDeltaTime set to {Time.fixedDeltaTime}");
    }
}