using CultOfQoL.Core;
using CultOfQoL.Patches.Gameplay;
using CultOfQoL.Patches.Systems;
using CultOfQoL.Patches.UI;

namespace CultOfQoL;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("com.bepis.bepinex.configurationmanager", "18.3")]
public partial class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.cotl.CultOfQoLCollection";
    internal const string PluginName = "The Cult of QoL Collection";
    private const string PluginVer = "2.3.1";

    private const string RestartGameMessage = "You must restart the game for these changes to take effect, as in totally exit to desktop and restart the game.\n\n** indicates a restart is required if the setting is changed.";
    private const string GeneralSection = "01. General";
    private const string MenuCleanupSection = "02. Menu Cleanup";
    private const string GoldenFleeceSection = "03. Golden Fleece";
    private const string PlayerSection = "05. Player Damage";
    //private const string PlayerSpeedSection = "05. Player Speed";

    private const string SavesSection = "06. Saves";

    //private const string ScalingSection = "08. Scale";
    private const string WeatherSection = "07. Weather";
    private const string NotificationsSection = "13. Notifications";

    private const string FollowersSection = "14. Followers";

    // private const string FarmSection = "12. Farm";
    private const string GameSpeedSection = "10. Game Speed";
    private const string CapacitySection = "12. Capacities";

    private const string AutoInteractSection = "11. Auto-Interact (Chests)";

    // private const string PropagandaSection = "16. Propaganda Structure";
    private const string MinesSection = "08. Mines";
    private const string MassSection = "16. Mass Actions";
    private const string StructureSection = "09. Structures";
    private const string TraitsSection = "15. Traits";
    private const string LootSection = "17. Loot";
    private const string PostProcessingSection = "18. Post Processing";
    private const string RitualSection = "19. Rituals";
    private const string GameMechanicsSection = "04. Game Mechanics";
    private const string ResetAllSettingsSection = "20. Reset All Settings";
    internal static ManualLogSource Log { get; private set; }

    // internal static CanvasScaler GameCanvasScaler { get; set; }
    // internal static CanvasScaler DungeonCanvasScaler { get; set; }
    internal static PopupManager PopupManager { get; private set; }

    // Performance optimization: Cache frequently accessed values
    private bool _cachedDirectLoadValue;
    private bool _cachedEnableQuickSaveValue;
    private bool _cachedDisableAdsValue;
    private KeyboardShortcut _cachedDirectLoadSkipKey;
    private KeyboardShortcut _cachedSaveShortcut;
    private UIMainMenuController _cachedUIMainMenuController;
    private Component[] _cachedAdComponents;
    private bool _configCacheValid;
    private static bool _showConfirmationDialog;
    internal static ConfigFile ConfigInstance;

    private void Awake()
    {
        HideBepInEx();
        ConfigInstance = Config;
        Log = Logger;

        PopupManager = gameObject.AddComponent<PopupManager>();

        //General - 01
        EnableLogging = ConfigInstance.Bind(GeneralSection, "Enable Logging", false, new ConfigDescription("Enable/disable logging.", null, new ConfigurationManagerAttributes { Order = 5 }));
        EnableLogging.SettingChanged += (_, _) => ConfigCache.MarkDirty(ConfigCache.Keys.EnableLogging);
        SkipDevIntros = ConfigInstance.Bind(GeneralSection, "Skip Intros", false, new ConfigDescription("Skip splash screens.", null, new ConfigurationManagerAttributes { Order = 3 }));
        SkipCrownVideo = ConfigInstance.Bind(GeneralSection, "Skip Crown Video", false, new ConfigDescription("Skips the video when the lamb gets given the crown.", null, new ConfigurationManagerAttributes { Order = 2 }));
        UnlockTwitchItems = ConfigInstance.Bind(GeneralSection, "Unlock Twitch Items", false, new ConfigDescription("Unlock pre-order DLC, Twitch plush, and Twitch drops. Paid DLC is excluded on purpose.", null, new ConfigurationManagerAttributes { Order = 1 }));

        //Menu Cleanup - 02
        RemoveMenuClutter = ConfigInstance.Bind(MenuCleanupSection, "Remove Extra Menu Buttons", false, new ConfigDescription("Removes credits/road-map/discord buttons from the menus.", null, new ConfigurationManagerAttributes { Order = 6 }));
        RemoveTwitchButton = ConfigInstance.Bind(MenuCleanupSection, "Remove Twitch Buttons", false, new ConfigDescription("Removes twitch buttons from the menus.", null, new ConfigurationManagerAttributes { Order = 5 }));
        DisableAds = ConfigInstance.Bind(MenuCleanupSection, "Disable Ads", false, new ConfigDescription("Disables the new ad 'feature'.", null, new ConfigurationManagerAttributes { Order = 4 }));
        DisableAds.SettingChanged += (_, _) => InvalidateConfigCache();
        RemoveHelpButtonInPauseMenu = ConfigInstance.Bind(MenuCleanupSection, "Remove Help Button In Pause Menu", false, new ConfigDescription("Removes the help button in the pause menu.", null, new ConfigurationManagerAttributes { Order = 3 }));
        RemoveTwitchButtonInPauseMenu = ConfigInstance.Bind(MenuCleanupSection, "Remove Twitch Button In Pause Menu", false, new ConfigDescription("Removes the twitch button in the pause menu.", null, new ConfigurationManagerAttributes { Order = 2 }));
        RemovePhotoModeButtonInPauseMenu = ConfigInstance.Bind(MenuCleanupSection, "Remove Photo Mode Button In Pause Menu", false, new ConfigDescription("Removes the photo mode button in the pause menu.", null, new ConfigurationManagerAttributes { Order = 1 }));
        MainMenuGlitch = ConfigInstance.Bind(MenuCleanupSection, "Main Menu Glitch", true, new ConfigDescription("Controls the sudden dark-mode switch effect.", null, new ConfigurationManagerAttributes { Order = 0 }));
        MainMenuGlitch.SettingChanged += (_, _) => { UpdateMenuGlitch(); };


        //Golden Fleece - 03
        ReverseGoldenFleeceDamageChange = ConfigInstance.Bind(GoldenFleeceSection, "Reverse Golden Fleece Change", false, new ConfigDescription("Reverts the default damage increase to 10% instead of 5%.", null, new ConfigurationManagerAttributes { Order = 4 }));
        // IncreaseGoldenFleeceDamageRate = Config.Bind(GoldenFleeceSection, "Increase Golden Fleece Rate", false, new ConfigDescription("Doubles the damage increase.", null, new ConfigurationManagerAttributes { Order = 3 }));
        FleeceDamageMulti = ConfigInstance.Bind(GoldenFleeceSection, "Fleece Damage Multiplier", 1.0f, new ConfigDescription("The custom damage multiplier to use. Based off the games default 5%.", new AcceptableValueRange<float>(-10f, 10f), new ConfigurationManagerAttributes { Order = 1 }));
        FleeceDamageMulti.SettingChanged += (_, _) => { FleeceDamageMulti.Value = Mathf.Round(FleeceDamageMulti.Value * 4) / 4; };

        //Game Mechanics - 04
        EasyFishing = ConfigInstance.Bind(GameMechanicsSection, "Disable Fishing Mini-Game", false, new ConfigDescription("Fishing mini-game cheese. Just cast and let the mod do the rest.", null, new ConfigurationManagerAttributes { DispName = "Disable Fishing Mini-Game**", Order = 5 }));
        EasyFishing.SettingChanged += (_, _) => ShowRestartMessage();
        DisableGameOver = ConfigInstance.Bind(GameMechanicsSection, "No More Game-Over", false, new ConfigDescription("Disables the game over function when you have 0 followers for consecutive days.", null, new ConfigurationManagerAttributes { Order = 4 }));
        ThriceMultiplyTarotCardLuck = ConfigInstance.Bind(GameMechanicsSection, "3x Tarot Luck", false, new ConfigDescription("Luck changes with game difficulty, this will multiply your luck multiplier by 3 for drawing rarer tarot cards.", null, new ConfigurationManagerAttributes { Order = 3 }));
        RareTarotCardsOnly = ConfigInstance.Bind(GameMechanicsSection, "Rare Tarot Cards Only", false, new ConfigDescription("Only draw rare tarot cards.", null, new ConfigurationManagerAttributes { Order = 2 }));
        SinBossLimit = ConfigInstance.Bind(GameMechanicsSection, "Sin Boss Limit", 3, new ConfigDescription("Bishop kills required to unlock Sin. Default is 3.", new AcceptableValueRange<int>(1, 5), new ConfigurationManagerAttributes { Order = 1 }));


        //Player Settings - 05
        BaseDamageMultiplier = ConfigInstance.Bind(PlayerSection, "Base Damage Multiplier", 1.0f, new ConfigDescription("The base damage multiplier to use.", new AcceptableValueRange<float>(-10, 10), new ConfigurationManagerAttributes { Order = 5 }));
        BaseDamageMultiplier.SettingChanged += (_, _) => { BaseDamageMultiplier.Value = Mathf.Round(BaseDamageMultiplier.Value * 4) / 4; };
        RunSpeedMulti = ConfigInstance.Bind(PlayerSection, "Run Speed Multiplier", 1.0f, new ConfigDescription("How much faster the player runs.", new AcceptableValueRange<float>(-10f, 10f), new ConfigurationManagerAttributes { Order = 7 }));
        RunSpeedMulti.SettingChanged += (_, _) => { RunSpeedMulti.Value = Mathf.Round(RunSpeedMulti.Value * 4) / 4; };
        DisableRunSpeedInDungeons = ConfigInstance.Bind(PlayerSection, "Disable Run Speed In Dungeons", true, new ConfigDescription("Disables the run speed multiplier in dungeons.", null, new ConfigurationManagerAttributes { Order = 6 }));
        DisableRunSpeedInCombat = ConfigInstance.Bind(PlayerSection, "Disable Run Speed In Combat", true, new ConfigDescription("Disables the run speed multiplier in combat.", null, new ConfigurationManagerAttributes { Order = 5 }));
        DodgeSpeedMulti = ConfigInstance.Bind(PlayerSection, "Dodge Speed Multiplier", 1.0f, new ConfigDescription("How much faster the player dodges.", new AcceptableValueRange<float>(-10f, 10f), new ConfigurationManagerAttributes { Order = 3 }));
        DodgeSpeedMulti.SettingChanged += (_, _) => { DodgeSpeedMulti.Value = Mathf.Round(DodgeSpeedMulti.Value * 4) / 4; };
        LungeSpeedMulti = ConfigInstance.Bind(PlayerSection, "Lunge Speed Multiplier", 1.0f, new ConfigDescription("How much faster the player lunges.", new AcceptableValueRange<float>(-10f, 10f), new ConfigurationManagerAttributes { Order = 1 }));
        LungeSpeedMulti.SettingChanged += (_, _) => { LungeSpeedMulti.Value = Mathf.Round(LungeSpeedMulti.Value * 4) / 4; };


        //Saves - 06
        SaveOnQuitToDesktop = ConfigInstance.Bind(SavesSection, "Save On Quit To Desktop", false, new ConfigDescription("Modify the confirmation dialog to save the game when you quit to desktop.", null, new ConfigurationManagerAttributes { Order = 8 }));
        SaveOnQuitToMenu = ConfigInstance.Bind(SavesSection, "Save On Quit To Menu", false, new ConfigDescription("Modify the confirmation dialog to save the game when you quit to menu.", null, new ConfigurationManagerAttributes { Order = 7 }));
        HideNewGameButtons = ConfigInstance.Bind(SavesSection, "Hide New Game Button (s)", false, new ConfigDescription("Hides the new game button if you have at least one save game.", null, new ConfigurationManagerAttributes { Order = 6 }));
        EnableQuickSaveShortcut = ConfigInstance.Bind(SavesSection, "Enable Quick Save Shortcut", false, new ConfigDescription("Enable/disable the quick save keyboard shortcut.", null, new ConfigurationManagerAttributes { Order = 5 }));
        EnableQuickSaveShortcut.SettingChanged += (_, _) => InvalidateConfigCache();
        SaveKeyboardShortcut = ConfigInstance.Bind(SavesSection, "Save Keyboard Shortcut", new KeyboardShortcut(KeyCode.F5), new ConfigDescription("The keyboard shortcut to save the game.", null, new ConfigurationManagerAttributes { Order = 4 }));
        SaveKeyboardShortcut.SettingChanged += (_, _) => InvalidateConfigCache();
        DirectLoadSave = ConfigInstance.Bind(SavesSection, "Direct Load Save", false, new ConfigDescription("Directly load the specified save game instead of showing the save menu.", null, new ConfigurationManagerAttributes { Order = 3 }));
        DirectLoadSave.SettingChanged += (_, _) => InvalidateConfigCache();
        DirectLoadSkipKey = ConfigInstance.Bind(SavesSection, "Direct Load Skip Key", new KeyboardShortcut(KeyCode.LeftShift), new ConfigDescription("The keyboard shortcut to skip the auto-load when loading the game.", null, new ConfigurationManagerAttributes { Order = 2 }));
        DirectLoadSkipKey.SettingChanged += (_, _) => InvalidateConfigCache();
        SaveSlotToLoad = ConfigInstance.Bind(SavesSection, "Save Slot To Load", 1, new ConfigDescription("The save slot to load.", new AcceptableValueList<int>(1, 2, 3), new ConfigurationManagerAttributes { Order = 1 }));
        SaveSlotToLoad.SettingChanged += (_, _) =>
        {
            if (!SaveAndLoad.SaveExist(SaveSlotToLoad.Value))
            {
                L($"The slot you have select doesn't contain a save game.");
                return;
            }

            L($"Save slot to load changed to {SaveSlotToLoad.Value}");
        };


        //Weather - 07
        ChangeWeatherOnPhaseChange = ConfigInstance.Bind(WeatherSection, "Change Weather On Phase Change", false, new ConfigDescription("By default, the game changes weather when you exit a structure, or on a new day. Enabling this makes the weather change on each phase i.e. morning, noon, evening, night.", null, new ConfigurationManagerAttributes { Order = 9 }));
        ChangeWeatherOnPhaseChange.SettingChanged += (_, _) => ConfigCache.MarkDirty(ConfigCache.Keys.ChangeWeatherOnPhaseChange);
        RandomWeatherChangeWhenExitingArea = ConfigInstance.Bind(WeatherSection, "Random Weather Change When Exiting Area", false, new ConfigDescription("When exiting a building/area, the weather will change to a random weather type instead of the previous weather.", null, new ConfigurationManagerAttributes { Order = 8 }));
        RandomWeatherChangeWhenExitingArea.SettingChanged += (_, _) => ConfigCache.MarkDirty(ConfigCache.Keys.RandomWeatherChangeWhenExitingArea);

        LightSnowColor = ConfigInstance.Bind(WeatherSection, "Light Snow Color", new Color(0.016f, 0f, 1f, 0.15f), new ConfigDescription("Control the colour of the screen when there is light snow.", null, new ConfigurationManagerAttributes { Order = 7 }));

        LightWindColor = ConfigInstance.Bind(WeatherSection, "Light Wind Color", new Color(0.016f, 0f, 1f, 0.15f), new ConfigDescription("Control the colour of the screen when there is light wind.", null, new ConfigurationManagerAttributes { Order = 6 }));

        LightRainColor = ConfigInstance.Bind(WeatherSection, "Light Rain Color", new Color(0.016f, 0f, 1f, 0.15f), new ConfigDescription("Control the colour of the screen when there is light rain.", null, new ConfigurationManagerAttributes { Order = 5 }));

        MediumRainColor = ConfigInstance.Bind(WeatherSection, "Medium Rain Color", new Color(0.016f, 0f, 1f, 0.15f), new ConfigDescription("Control the colour of the screen when there is medium rain.", null, new ConfigurationManagerAttributes { Order = 4 }));

        HeavyRainColor = ConfigInstance.Bind(WeatherSection, "Heavy Rain Color", new Color(0.016f, 0f, 1f, 0.45f), new ConfigDescription("Control the colour of the screen when there is heavy rain.", null, new ConfigurationManagerAttributes { Order = 3 }));

        WeatherDropDown = ConfigInstance.Bind(WeatherSection, "Weather Dropdown", Weather.WeatherCombo.HeavyRain, new ConfigDescription("Select the type of weather you want to test to see the effect your chosen colour has.", null, new ConfigurationManagerAttributes { Order = 2 }));

        ConfigInstance.Bind(WeatherSection, "Test Weather", true, new ConfigDescription("Test Weather Color", null, new ConfigurationManagerAttributes { Order = 1, HideDefaultButton = true, CustomDrawer = TestWeather }));


        //Mines - 08
        LumberAndMiningStationsDontAge = ConfigInstance.Bind(MinesSection, "Infinite Lumber & Mining Stations", false, new ConfigDescription("Lumber and mining stations should never run out and collapse. Takes 1st priority.", null, new ConfigurationManagerAttributes { Order = 3 }));
        LumberAndMiningStationsDontAge.SettingChanged += (_, _) => ConfigCache.MarkDirty(ConfigCache.Keys.LumberAndMiningStationsDontAge);

        LumberAndMiningStationsAgeMultiplier = ConfigInstance.Bind(MinesSection, "Lumber & Mining Stations Age Multiplier", 1.0f, new ConfigDescription("How much slower (or faster) the lumber and mining stations age. Default is 1.0f.", new AcceptableValueRange<float>(-10f, 10f), new ConfigurationManagerAttributes { Order = 2 }));
        LumberAndMiningStationsAgeMultiplier.SettingChanged += (_, _) => ConfigCache.MarkDirty(ConfigCache.Keys.LumberAndMiningStationsAgeMultiplier);
        LumberAndMiningStationsAgeMultiplier.SettingChanged += (_, _) => { LumberAndMiningStationsAgeMultiplier.Value = Mathf.Round(LumberAndMiningStationsAgeMultiplier.Value * 4) / 4; };

        //Structures - 09
        TurnOffSpeakersAtNight = ConfigInstance.Bind(StructureSection, "Turn Off Speakers At Night", false, new ConfigDescription("Turns the speakers off, and stops fuel consumption at night time.", null, new ConfigurationManagerAttributes { Order = 9 }));
        TurnOffSpeakersAtNight.SettingChanged += (_, _) => ConfigCache.MarkDirty(ConfigCache.Keys.TurnOffSpeakersAtNight);
        DisablePropagandaSpeakerAudio = ConfigInstance.Bind(StructureSection, "Disable Propaganda Speaker Audio", false, new ConfigDescription("Disables the audio from propaganda speakers.", null, new ConfigurationManagerAttributes { Order = 8 }));
        DisablePropagandaSpeakerAudio.SettingChanged += (_, _) => ConfigCache.MarkDirty(ConfigCache.Keys.DisablePropagandaSpeakerAudio);
        AddExhaustedToHealingBay = ConfigInstance.Bind(StructureSection, "Add Exhausted To Healing Bay", false, new ConfigDescription("Allows you to select exhausted followers for rest and relaxation in the healing bays.", null, new ConfigurationManagerAttributes { Order = 7 }));
        OnlyShowDissenters = ConfigInstance.Bind(StructureSection, "Only Show Dissenters In Prison Menu", false, new ConfigDescription("Only show dissenting followers when interacting with the prison.", null, new ConfigurationManagerAttributes { Order = 6 }));
        AdjustRefineryRequirements = ConfigInstance.Bind(StructureSection, "Adjust Refinery Requirements", false, new ConfigDescription("Where possible, halves the materials needed to convert items in the refinery. Rounds up.", null, new ConfigurationManagerAttributes { Order = 5 }));
        AdjustRefineryRequirements.SettingChanged += (_, _) => ConfigCache.MarkDirty(ConfigCache.Keys.AdjustRefineryRequirements);

        var harvestRange = Mathf.RoundToInt(HarvestTotem.EFFECTIVE_DISTANCE);
        HarvestTotemRange = ConfigInstance.Bind(StructureSection, "Harvest Totem Range", harvestRange, new ConfigDescription($"The range of the harvest totem. Default is {harvestRange}.", new AcceptableValueRange<int>(3, 14), new ConfigurationManagerAttributes { Order = 4 }));
        HarvestTotemRange.SettingChanged += (_, _) => { HarvestTotem.EFFECTIVE_DISTANCE = Mathf.RoundToInt(HarvestTotemRange.Value); };

        var speakerRange = Mathf.RoundToInt(Structures_PropagandaSpeaker.EFFECTIVE_DISTANCE);
        PropagandaSpeakerRange = ConfigInstance.Bind(StructureSection, "Propaganda Speaker Range", speakerRange, new ConfigDescription($"The range of the propaganda speaker. Default is {speakerRange}.", new AcceptableValueRange<int>(1, 20), new ConfigurationManagerAttributes { Order = 3 }));
        PropagandaSpeakerRange.SettingChanged += (_, _) => { Structures_PropagandaSpeaker.EFFECTIVE_DISTANCE = Mathf.RoundToInt(PropagandaSpeakerRange.Value); };

        FarmStationRange = ConfigInstance.Bind(StructureSection, "Farm Station Range", 6, new ConfigDescription("The range of the farm station. Default is 6.", new AcceptableValueRange<int>(1, 20), new ConfigurationManagerAttributes { Order = 2 }));
        FarmPlotSignRange = ConfigInstance.Bind(StructureSection, "Farm Plot Range", 5, new ConfigDescription("The range of the farm plot sign. Default is 5.", new AcceptableValueRange<int>(1, 20), new ConfigurationManagerAttributes { Order = 1 }));
        FarmPlotSignRange = ConfigInstance.Bind(StructureSection, "Farm Plot Sign Range", 5, new ConfigDescription("The range of the farm plot sign. Default is 5.", new AcceptableValueRange<int>(1, 20), new ConfigurationManagerAttributes { Order = 0 }));

        CookedMeatMealsContainBone = ConfigInstance.Bind(StructureSection, "Cooked Meat Meals Contain Bone", false, new ConfigDescription("Meat + fish meals will spawn 1 - 3 bones when cooked.", null, new ConfigurationManagerAttributes { Order = -1 }));
        CookedMeatMealsContainBone.SettingChanged += (_, _) => ConfigCache.MarkDirty(ConfigCache.Keys.CookedMeatMealsContainBone);
        AddSpiderWebsToOfferings = ConfigInstance.Bind(StructureSection, "Add Spider Webs To Offerings", false, new ConfigDescription("Adds Spider Webs to the Offering Shrines default offerings.", null, new ConfigurationManagerAttributes { Order = -2 }));
        AddSpiderWebsToOfferings.SettingChanged += (_, _) => ConfigCache.MarkDirty(ConfigCache.Keys.AddSpiderWebsToOfferings);
        AddCrystalShardsToOfferings = ConfigInstance.Bind(StructureSection, "Add Crystals To Offerings", false, new ConfigDescription("Adds Crystal Shards to the Offering Shrines rare offerings.", null, new ConfigurationManagerAttributes { Order = -3 }));
        AddCrystalShardsToOfferings.SettingChanged += (_, _) => ConfigCache.MarkDirty(ConfigCache.Keys.AddCrystalShardsToOfferings);

        ProduceSpiderWebsFromLumber = ConfigInstance.Bind(StructureSection, "Lumber Stations Produce Spider Webs", false, new ConfigDescription("Lumber stations will produce spider webs from logs collected.", null, new ConfigurationManagerAttributes { Order = -4 }));
        ProduceSpiderWebsFromLumber.SettingChanged += (_, _) => ConfigCache.MarkDirty(ConfigCache.Keys.ProduceSpiderWebsFromLumber);
        SpiderWebsPerLogs = ConfigInstance.Bind(StructureSection, "Spider Webs Per Logs", 5, new ConfigDescription("Number of logs needed to produce 1 spider web.", new AcceptableValueRange<int>(1, 20), new ConfigurationManagerAttributes { Order = -5 }));
        SpiderWebsPerLogs.SettingChanged += (_, _) => ConfigCache.MarkDirty(ConfigCache.Keys.SpiderWebsPerLogs);

        ProduceCrystalShardsFromStone = ConfigInstance.Bind(StructureSection, "Mining Stations Produce Crystal Shards", false, new ConfigDescription("Mining stations will produce crystal shards from stone collected.", null, new ConfigurationManagerAttributes { Order = -6 }));
        ProduceCrystalShardsFromStone.SettingChanged += (_, _) => ConfigCache.MarkDirty(ConfigCache.Keys.ProduceCrystalShardsFromStone);
        CrystalShardsPerStone = ConfigInstance.Bind(StructureSection, "Crystal Shards Per Stone", 5, new ConfigDescription("Number of stone needed to produce 1 crystal shard.", new AcceptableValueRange<int>(1, 20), new ConfigurationManagerAttributes { Order = -7 }));
        CrystalShardsPerStone.SettingChanged += (_, _) => ConfigCache.MarkDirty(ConfigCache.Keys.CrystalShardsPerStone);

        //Speed - 10
        EnableGameSpeedManipulation = ConfigInstance.Bind(GameSpeedSection, "Enable Game Speed Manipulation", false, new ConfigDescription("Use left/right arrows keys to increase/decrease game speed in 0.25 increments. Up arrow to reset to default.", null, new ConfigurationManagerAttributes { Order = 8 }));
        ShortenGameSpeedIncrements = ConfigInstance.Bind(GameSpeedSection, "Shorten Game Speed Increments", false, new ConfigDescription("Increments in steps of 1, instead of 0.25.", null, new ConfigurationManagerAttributes { Order = 7 }));
        ResetTimeScaleKey = ConfigInstance.Bind(GameSpeedSection, "Reset Time Scale Key", new KeyboardShortcut(KeyCode.UpArrow), new ConfigDescription("The keyboard shortcut to reset the game speed to 1.", null, new ConfigurationManagerAttributes { Order = 6 }));
        IncreaseGameSpeedKey = ConfigInstance.Bind(GameSpeedSection, "Increase Game Speed Key", new KeyboardShortcut(KeyCode.RightArrow), new ConfigDescription("The keyboard shortcut to increase the game speed.", null, new ConfigurationManagerAttributes { Order = 5 }));
        DecreaseGameSpeedKey = ConfigInstance.Bind(GameSpeedSection, "Decrease Game Speed Key", new KeyboardShortcut(KeyCode.LeftArrow), new ConfigDescription("The keyboard shortcut to decrease the game speed.", null, new ConfigurationManagerAttributes { Order = 4 }));
        FastCollecting = ConfigInstance.Bind(GameSpeedSection, "Speed Up Collection", false, new ConfigDescription("Increases the rate you can collect from the shrines, and other structures.", null, new ConfigurationManagerAttributes { DispName = "Speed Up Collection**", Order = 3 }));
        FastCollecting.SettingChanged += (_, _) => ShowRestartMessage();

        SlowDownTimeMultiplier = ConfigInstance.Bind(GameSpeedSection, "Slow Down Time Multiplier", 1.0f, new ConfigDescription("The multiplier to use for slow down time. For example, the default value of 2 is making the day twice as long.", new AcceptableValueRange<float>(-10f, 10f), new ConfigurationManagerAttributes { Order = 1 }));
        SlowDownTimeMultiplier.SettingChanged += (_, _) => { SlowDownTimeMultiplier.Value = Mathf.Round(SlowDownTimeMultiplier.Value * 4) / 4; };
        FastRitualSermons = ConfigInstance.Bind(GameSpeedSection, "Fast Rituals & Sermons", false, new ConfigDescription("Speeds up rituals and sermons.", null, new ConfigurationManagerAttributes { Order = 0 }));
        FastRitualSermons.SettingChanged += (_, _) =>
        {
            if (FastRitualSermons.Value) return;
            RitualSermonSpeed.RitualRunning = false;
            GameManager.SetTimeScale(1);
        };

        RitualCooldownTime = ConfigInstance.Bind(
            GameSpeedSection,
            "Ritual Cooldown Time Multiplier",
            1.0f,
            new ConfigDescription(
                "Scales ritual cooldown duration.\n" +
                "• 2.0 = Double cooldown (slower)\n" +
                "• 1.0 = Normal cooldown\n" +
                "• 0.5 = 50% of normal (faster)\n" +
                "• 0.25 = 25% of normal (much faster)\n\n" +
                "Drag left to reduce cooldown time.\n" +
                "Allowed range: 0.1x to 2.0x.\n\n" +
                "<i>Note: Applies only to newly performed rituals.</i>",
                new AcceptableValueRange<float>(0.1f, 2.0f),
                new ConfigurationManagerAttributes { Order = 0 }
            )
        );
        RitualCooldownTime.SettingChanged += (_, _) => { RitualCooldownTime.Value = Mathf.Round(RitualCooldownTime.Value * 4) / 4; };

        //Chest Auto-Interact - 11
        EnableAutoCollect = ConfigInstance.Bind(AutoInteractSection, "Enable Auto Collect", false, new ConfigDescription("Makes chests automatically send you the resources when you're nearby.", null, new ConfigurationManagerAttributes
        {
            Order = 6, DispName = "Enable Auto Collect**"
        }));
        EnableAutoCollect.SettingChanged += (_, _) => ShowRestartMessage();
        AutoCollectFromFarmStationChests = ConfigInstance.Bind(AutoInteractSection, "Auto Collect From Farm Station Chests", false, new ConfigDescription("Automatically collect from farm station chests.", null, new ConfigurationManagerAttributes
        {
            Order = 5
        }));
        TriggerAmount = ConfigInstance.Bind(AutoInteractSection, "Trigger Amount", 5, new ConfigDescription("The amount of resources needed to trigger the auto-interact.", new AcceptableValueRange<int>(1, 100), new ConfigurationManagerAttributes
        {
            Order = 4, ShowRangeAsPercent = false
        }));
        AutoInteractRangeMulti = ConfigInstance.Bind(AutoInteractSection, "Loot Magnet Range Multiplier", 1.0f, new ConfigDescription("Enter a multiplier to use for auto-collect range when using custom range.", new AcceptableValueRange<float>(-10f, 10f), new ConfigurationManagerAttributes
        {
            Order = 1
        }));
        AutoInteractRangeMulti.SettingChanged += (_, _) => { AutoInteractRangeMulti.Value = Mathf.Round(AutoInteractRangeMulti.Value * 4) / 4; };

        //Capacity - 12
        UseMultiplesOf32 = ConfigInstance.Bind(CapacitySection, "Use Multiples of 32", true, new ConfigDescription("Use multiples of 32 for silo capacity.", null, new ConfigurationManagerAttributes
        {
            Order = 7
        }));
        UseMultiplesOf32.SettingChanged += (_, _) => ConfigCache.MarkDirty(ConfigCache.Keys.UseMultiplesOf32);
        SiloCapacityMulti = ConfigInstance.Bind(CapacitySection, "Silo Capacity Multiplier", 1.0f, new ConfigDescription("Enter a multiplier to use for silo capacity when using custom capacity.", new AcceptableValueRange<float>(-10f, 10f), null, new ConfigurationManagerAttributes
        {
            Order = 4
        }));
        SiloCapacityMulti.SettingChanged += (_, _) =>
        {
            SiloCapacityMulti.Value = Mathf.Round(SiloCapacityMulti.Value * 4) / 4;
            ConfigCache.MarkDirty(ConfigCache.Keys.SiloCapacityMulti);
        };
        SoulCapacityMulti = ConfigInstance.Bind(CapacitySection, "Soul Capacity Multiplier", 1.0f, new ConfigDescription("Enter a multiplier to use for soul capacity when using custom capacity.", new AcceptableValueRange<float>(-10f, 10f), null, new ConfigurationManagerAttributes
        {
            Order = 1
        }));
        SoulCapacityMulti.SettingChanged += (_, _) =>
        {
            SoulCapacityMulti.Value = Mathf.Round(SoulCapacityMulti.Value * 4) / 4;
            ConfigCache.MarkDirty(ConfigCache.Keys.SoulCapacityMulti);
        };

        //Notifications - 13
        NotifyOfScarecrowTraps = ConfigInstance.Bind(NotificationsSection, "Notify of Scarecrow Traps", false, new ConfigDescription("Display a notification when the farm scarecrows have caught a trap!", null, new ConfigurationManagerAttributes
        {
            Order = 5
        }));
        NotifyOfNoFuel = ConfigInstance.Bind(NotificationsSection, "Notify of No Fuel", false, new ConfigDescription("Display a notification when a structure has run out of fuel.", null, new ConfigurationManagerAttributes
        {
            Order = 4
        }));
        NotifyOfBedCollapse = ConfigInstance.Bind(NotificationsSection, "Notify of Bed Collapse", false, new ConfigDescription("Display a notification when a bed has collapsed.", null, new ConfigurationManagerAttributes
        {
            Order = 3
        }));
        ShowPhaseNotifications = ConfigInstance.Bind(NotificationsSection, "Phase Notifications", false, new ConfigDescription("Show a notification when the time of day changes.", null, new ConfigurationManagerAttributes
        {
            Order = 2
        }));
        ShowPhaseNotifications.SettingChanged += (_, _) => ConfigCache.MarkDirty(ConfigCache.Keys.ShowPhaseNotifications);
        ShowWeatherChangeNotifications = ConfigInstance.Bind(NotificationsSection, "Weather Change Notifications", false, new ConfigDescription("Show a notification when the weather changes.", null, new ConfigurationManagerAttributes
        {
            Order = 1
        }));
        ShowWeatherChangeNotifications.SettingChanged += (_, _) => ConfigCache.MarkDirty(ConfigCache.Keys.ShowWeatherChangeNotifications);

        //Followers - 14
        GiveFollowersNewNecklaces = ConfigInstance.Bind(FollowersSection, "Give Followers New Necklaces", false, new ConfigDescription("Followers will be able to receive new necklaces, with the old one being returned to you.", null, new ConfigurationManagerAttributes
        {
            Order = 8
        }));
        CleanseIllnessAndExhaustionOnLevelUp = ConfigInstance.Bind(FollowersSection, "Cleanse Illness and Exhaustion", false, new ConfigDescription("When a follower 'levels up', if they are sick or exhausted, the status is cleansed.", null, new ConfigurationManagerAttributes
        {
            Order = 7
        }));
        CollectTitheFromOldFollowers = ConfigInstance.Bind(FollowersSection, "Collect Tithe From Old Followers", false, new ConfigDescription("Enable collecting tithe from the elderly.", null, new ConfigurationManagerAttributes
        {
            Order = 6
        }));
        IntimidateOldFollowers = ConfigInstance.Bind(FollowersSection, "Intimidate Old Followers", false, new ConfigDescription("Enable intimidating the elderly.", null, new ConfigurationManagerAttributes
        {
            Order = 5
        }));
        RemoveLevelLimit = ConfigInstance.Bind(FollowersSection, "Remove Level Limit", false, new ConfigDescription("Removes the level limit for followers. They can now level up infinitely.", null, new ConfigurationManagerAttributes
        {
            Order = 4
        }));
        MakeOldFollowersWork = ConfigInstance.Bind(FollowersSection, "Make Old Followers Work", false, new ConfigDescription("Enable the elderly to work.", null, new ConfigurationManagerAttributes
        {
            DispName = "Make Old Followers Work**",
            Order = 3
        }));
        MakeOldFollowersWork.SettingChanged += (_, _) => ShowRestartMessage();

        MinRangeLifeExpectancy = ConfigInstance.Bind(
            FollowersSection,
            "Minimum Range Life Expectancy", 40,
            new ConfigDescription(
                "The lowest possible life expectancy a follower can have. The game will randomly choose a value between this and the maximum value below. This must be less than the maximum.",
                new AcceptableValueRange<int>(1, 100),
                new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 2 }
            )
        );
        MinRangeLifeExpectancy.SettingChanged += (_, _) => { EnforceRangeSanity(); };

        MaxRangeLifeExpectancy = ConfigInstance.Bind(
            FollowersSection,
            "Maximum Range Life Expectancy", 55,
            new ConfigDescription(
                "The highest possible life expectancy a follower can have. The game will randomly choose a value between the minimum above and this value.",
                new AcceptableValueRange<int>(1, 100),
                new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 0 }
            )
        );
        MaxRangeLifeExpectancy.SettingChanged += (_, _) => { EnforceRangeSanity(); };


        //Traits - 15
        NoNegativeTraits = ConfigInstance.Bind(TraitsSection, "No Negative Traits", false, new ConfigDescription("Negative traits will be replaced based on the configuration here.", null, new ConfigurationManagerAttributes
        {
            Order = 3
        }));
        NoNegativeTraits.SettingChanged += (_, _) => { UpdateNoNegativeTraits(); };
        UseUnlockedTraitsOnly = ConfigInstance.Bind(TraitsSection, "Use Unlocked Traits Only", true, new ConfigDescription("Only use unlocked traits when replacing negative traits.", null, new ConfigurationManagerAttributes
        {
            Order = 2
        }));
        UseUnlockedTraitsOnly.SettingChanged += (_, _) => { GenerateAvailableTraits(); };
        IncludeImmortal = ConfigInstance.Bind(TraitsSection, "Include Immortal", false, new ConfigDescription("Include the Immortal trait when replacing negative traits.", null, new ConfigurationManagerAttributes
        {
            Order = 1
        }));
        IncludeImmortal.SettingChanged += (_, _) => { GenerateAvailableTraits(); };
        IncludeDisciple = ConfigInstance.Bind(TraitsSection, "Include Disciple", false, new ConfigDescription("Include the Disciple trait when replacing negative traits.", null, new ConfigurationManagerAttributes
        {
            Order = 0
        }));
        IncludeDisciple.SettingChanged += (_, _) => { GenerateAvailableTraits(); };

        ShowNotificationsWhenRemovingTraits = ConfigInstance.Bind(TraitsSection, "Show Notifications When Removing Traits", false, new ConfigDescription("Show notifications when removing negative traits.", null, new ConfigurationManagerAttributes
        {
            Order = 0
        }));
        ShowNotificationsWhenAddingTraits = ConfigInstance.Bind(TraitsSection, "Show Notifications When Adding Traits", false, new ConfigDescription("Show notifications when adding positive traits.", null, new ConfigurationManagerAttributes
        {
            Order = 0
        }));


        //Mass Section - 16
        MassFertilize = ConfigInstance.Bind(MassSection, "Mass Fertilize", false, new ConfigDescription("When fertilizing a plot, all farm plots are fertilized at once.", null, new ConfigurationManagerAttributes
        {
            Order = 17
        }));


        MassWater = ConfigInstance.Bind(MassSection, "Mass Water", false, new ConfigDescription("When watering a plot, all farm plots are watered at once.", null, new ConfigurationManagerAttributes
        {
            Order = 16
        }));

        MassBribe = ConfigInstance.Bind(MassSection, "Mass Bribe", false, new ConfigDescription("When bribing a follower, all followers are bribed at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Bribe**", Order = 15
        }));
        MassBribe.SettingChanged += (_, _) => ShowRestartMessage();

        MassBless = ConfigInstance.Bind(MassSection, "Mass Bless", false, new ConfigDescription("When blessing a follower, all followers are blessed at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Bless**", Order = 14
        }));
        MassBless.SettingChanged += (_, _) => ShowRestartMessage();

        MassExtort = ConfigInstance.Bind(MassSection, "Mass Extort", false, new ConfigDescription("When extorting a follower, all followers are extorted at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Extort**", Order = 13
        }));
        MassExtort.SettingChanged += (_, _) => ShowRestartMessage();

        MassPetDog = ConfigInstance.Bind(MassSection, "Mass Pet Dog", false, new ConfigDescription("When petting a a follower, all followers are petted at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Pet Dog**", Order = 12
        }));
        MassPetDog.SettingChanged += (_, _) => ShowRestartMessage();

        MassIntimidate = ConfigInstance.Bind(MassSection, "Mass Intimidate", false, new ConfigDescription("When intimidating a follower, all followers are intimidated at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Intimidate**", Order = 11
        }));
        MassIntimidate.SettingChanged += (_, _) => ShowRestartMessage();

        MassInspire = ConfigInstance.Bind(MassSection, "Mass Inspire", false, new ConfigDescription("When inspiring a follower, all followers are inspired at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Inspire**", Order = 10
        }));
        MassInspire.SettingChanged += (_, _) => ShowRestartMessage();

        MassCollectFromBeds = ConfigInstance.Bind(MassSection, "Mass Collect From Beds", false, new ConfigDescription("When collecting resources from a bed, all beds are collected from at once.", null, new ConfigurationManagerAttributes
        {
            Order = 9
        }));


        MassCollectFromOuthouses = ConfigInstance.Bind(MassSection, "Mass Collect From Outhouses", false, new ConfigDescription("When collecting resources from an outhouse, all outhouses are collected from at once.", null, new ConfigurationManagerAttributes
        {
            Order = 8
        }));


        MassCollectFromOfferingShrines = ConfigInstance.Bind(MassSection, "Mass Collect From Offering Shrines", false, new ConfigDescription("When collecting resources from an offering shrine, all offering shrines are collected from at once.", null, new ConfigurationManagerAttributes
        {
            Order = 7
        }));


        MassCollectFromPassiveShrines = ConfigInstance.Bind(MassSection, "Mass Collect From Passive Shrines", false, new ConfigDescription("When collecting resources from a passive shrine, all passive shrines are collected from at once.", null, new ConfigurationManagerAttributes
        {
            Order = 6
        }));


        MassCollectFromCompost = ConfigInstance.Bind(MassSection, "Mass Collect From Compost", false, new ConfigDescription("When collecting resources from a compost, all composts are collected from at once.", null, new ConfigurationManagerAttributes
        {
            Order = 5
        }));


        MassCollectFromHarvestTotems = ConfigInstance.Bind(MassSection, "Mass Collect From Harvest Totems", false, new ConfigDescription("When collecting resources from a harvest totem, all harvest totems are collected from at once.", null, new ConfigurationManagerAttributes
        {
            Order = 4
        }));


        MassRomance = ConfigInstance.Bind(MassSection, "Mass Romance", false, new ConfigDescription("When romancing a follower, all followers are romanced at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Romance**", Order = 3
        }));
        MassRomance.SettingChanged += (_, _) => ShowRestartMessage();

        MassBully = ConfigInstance.Bind(MassSection, "Mass Bully", false, new ConfigDescription("When bullying a follower, all followers are bullied at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Bully**", Order = 2
        }));
        MassBully.SettingChanged += (_, _) => ShowRestartMessage();

        MassReassure = ConfigInstance.Bind(MassSection, "Mass Reassure", false, new ConfigDescription("When reassuring a follower, all followers are reassured at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Reassure**", Order = 1
        }));
        MassReassure.SettingChanged += (_, _) => ShowRestartMessage();

        MassReeducate = ConfigInstance.Bind(MassSection, "Mass Reeducate", false, new ConfigDescription("When reeducating a follower, all followers are reeducated at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Reeducate**", Order = 0
        }));
        MassReeducate.SettingChanged += (_, _) => ShowRestartMessage();

        //Loot - 17
        AllLootMagnets = ConfigInstance.Bind(LootSection, "All Loot Magnets", false, new ConfigDescription("All loot is magnetized to you.", null, new ConfigurationManagerAttributes
        {
            Order = 0
        }));
        AllLootMagnets.SettingChanged += (_, _) => { UpdateAllMagnets(); };
        MagnetRangeMultiplier = ConfigInstance.Bind(LootSection, "Magnet Range Multiplier", 1.0f, new ConfigDescription("Apply a multiplier to the magnet range.", new AcceptableValueRange<float>(-10f, 10f), new ConfigurationManagerAttributes
        {
            Order = 0
        }));
        MagnetRangeMultiplier.SettingChanged += (_, _) =>
        {
            {
                MagnetRangeMultiplier.Value = MagnetRangeMultiplier.Value * 4f / 4f;
                UpdateCustomMagnet();
            }
        };

        //Post Processing
        VignetteEffect = ConfigInstance.Bind(PostProcessingSection, "Vignette Effect", true, new ConfigDescription("Enable/disable the vignette effect.", null, new ConfigurationManagerAttributes
        {
            Order = 0
        }));
        VignetteEffect.SettingChanged += (_, _) => { PostProcessing.ToggleVignette(); };

        ReverseEnrichmentNerf = ConfigInstance.Bind(RitualSection, "Reverse Enrichment Nerf", false, new ConfigDescription("Reverts the nerf to the Ritual of Enrichment. Enabling this will automatically enable remove follower level cap.", null, new ConfigurationManagerAttributes
        {
            Order = 0
        }));
        ReverseEnrichmentNerf.SettingChanged += (_, _) =>
        {
            if (ReverseEnrichmentNerf.Value && !RemoveLevelLimit.Value)
            {
                Log.LogInfo("Enabling 'Remove Level Limit' as it is required for 'Reverse Enrichment Nerf'.");
                RemoveLevelLimit.Value = true;
            }
        };

        ConfigInstance.Bind(ResetAllSettingsSection, "Reset All Settings", false, new ConfigDescription("Set this to true and save the config file to reset all settings to default.", null, new ConfigurationManagerAttributes
        {
            Order = 0, HideDefaultButton = true, CustomDrawer = ResetAll
        }));

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);

        SceneManager.sceneLoaded += OnSceneLoaded;

        Helpers.PrintModLoaded(PluginName, Logger);


        // if (!SoftDepend.Enabled) return;
        //
        // SoftDepend.AddSettingsMenus();
        // Log.LogInfo("API detected - You can configure mod settings in the settings menu."));
    }

    private static void ResetAll(ConfigEntryBase entry)
    {
        if (_showConfirmationDialog)
        {
            DisplayConfirmationDialog();
        }
        else
        {
            var button = GUILayout.Button("Reset All Settings", GUILayout.ExpandWidth(true));
            if (button)
            {
                _showConfirmationDialog = true;
            }
        }
    }

    private static void DisplayConfirmationDialog()
    {
        GUILayout.Label("Are you sure you want to reset to default settings?");

        GUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Yes", GUILayout.ExpandWidth(true)))
            {
                RecommendedSettingsAction();
                _showConfirmationDialog = false;
            }

            if (GUILayout.Button("No", GUILayout.ExpandWidth(true)))
            {
                _showConfirmationDialog = false;
            }
        }
        GUILayout.EndHorizontal();
    }

    private static void RecommendedSettingsAction()
    {
        // var entries =  BepInEx.Configuration.ConfigFile.Entries.ToList(); //property (get only) is protected
        // protected Dictionary<ConfigDefinition, ConfigEntryBase> Entries { get; } = new Dictionary<ConfigDefinition, ConfigEntryBase>();
        if (AccessTools.PropertyGetter(typeof(ConfigFile), "Entries").Invoke(ConfigInstance, null) is not Dictionary<ConfigDefinition, ConfigEntryBase> entries)
        {
            Log.LogError("[Config] Unable to access config entries for reset.");
            return;
        }

        foreach (var ent in entries.Where(ent => ent.Value.BoxedValue != ent.Value.DefaultValue).Where(ent => ent.Key.Section != ResetAllSettingsSection))
        {
            ent.Value.BoxedValue = ent.Value.DefaultValue;
            Log.LogInfo($"[Config] Resetting {ent.Key} to default value: {ent.Value.DefaultValue}");
        }
    }

    private static void EnforceRangeSanity()
    {
        var min = MinRangeLifeExpectancy.Value;
        var max = MaxRangeLifeExpectancy.Value;

        if (min >= max)
        {
            if (min > 1)
            {
                MinRangeLifeExpectancy.Value = max - 1;
                Log.LogWarning($"[Config] Min was >= Max — adjusted Min to {max - 1}");
            }
            else if (max < 100)
            {
                MaxRangeLifeExpectancy.Value = min + 1;
                Log.LogWarning($"[Config] Min was >= Max — adjusted Max to {min + 1}");
            }
            else
            {
                // Fallback: reset to defaults
                MinRangeLifeExpectancy.Value = 40;
                MaxRangeLifeExpectancy.Value = 55;
                Log.LogWarning("[Config] Min/Max invalid and unresolvable — reset to defaults.");
            }
        }
    }


    private static void TestWeather(ConfigEntryBase entry)
    {
        var button = GUILayout.Button("Test Weather", GUILayout.ExpandWidth(true));
        if (button)
        {
            var weatherInstance = WeatherSystemController.Instance;
            if (!weatherInstance) return;
            var selectedWeather = WeatherDropDown.Value;
            switch (selectedWeather)
            {
                case Weather.WeatherCombo.LightRain:
                    weatherInstance.SetWeather(WeatherSystemController.WeatherType.Raining, WeatherSystemController.WeatherStrength.Light);
                    break;
                case Weather.WeatherCombo.MediumRain:
                    weatherInstance.SetWeather(WeatherSystemController.WeatherType.Raining, WeatherSystemController.WeatherStrength.Medium);
                    break;
                case Weather.WeatherCombo.HeavyRain:
                    weatherInstance.SetWeather(WeatherSystemController.WeatherType.Raining, WeatherSystemController.WeatherStrength.Heavy);
                    break;
                case Weather.WeatherCombo.LightSnow:
                    weatherInstance.SetWeather(WeatherSystemController.WeatherType.Snowing, WeatherSystemController.WeatherStrength.Light);
                    break;
                case Weather.WeatherCombo.LightWind:
                    weatherInstance.SetWeather(WeatherSystemController.WeatherType.Windy, WeatherSystemController.WeatherStrength.Light);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private static void UpdateNoNegativeTraits()
    {
        if (IsNoNegativePresent())
        {
            NoNegativeTraits.Value = false;
            return;
        }

        if (NoNegativeTraits.Value)
        {
            Patches.Followers.NoNegativeTraits.UpdateAllFollowerTraits();
        }
        else
        {
            Patches.Followers.NoNegativeTraits.RestoreOriginalTraits();
        }
    }

    private static void GenerateAvailableTraits()
    {
        if (IsNoNegativePresent()) return;
        Patches.Followers.NoNegativeTraits.GenerateAvailableTraits();
    }

    private static void UpdateNavigationMode()
    {
        var buttons = Resources.FindObjectsOfTypeAll<MMButton>();
        foreach (var button in buttons)
        {
            button.Selectable.navigation = button.Selectable.navigation with { mode = Navigation.Mode.Automatic };
        }
    }

    private static void UpdateAllMagnets()
    {
        if (AllLootMagnets.Value)
        {
            PickUps.UpdateAllPickUps();
        }
        else
        {
            PickUps.RestoreAllPickUps();
        }
    }


    private static void UpdateCustomMagnet()
    {
        if (Mathf.Approximately(MagnetRangeMultiplier.Value, 1.0f)) return;

        PickUps.UpdateAllPickUps();
    }


    private static void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        UpdateMenuGlitch();
        UpdateNavigationMode();
        UpdateAllMagnets();
    }

    private static void UpdateMenuGlitch()
    {
        var mmc = FindObjectOfType<MainMenuController>();
        if (mmc)
        {
            mmc.doIntroGlitch = MainMenuGlitch.Value;
        }
    }

    public static void L(string message)
    {
        // Use cached config value to avoid repeated access
        if (ConfigCache.GetCachedValue(ConfigCache.Keys.EnableLogging, () => EnableLogging.Value))
        {
            Log.LogInfo(message);
        }
    }


    private static void ShowRestartMessage()
    {
        if (!PopupManager.ShowPopup)
        {
            PopupManager.ShowPopupDlg(RestartGameMessage, true);
        }
    }

    private static bool IsNoNegativePresent()
    {
        if (!Patches.Followers.NoNegativeTraits.IsNothingNegativePresent()) return false;
        PopupManager.ShowPopupDlg($"You have the 'Nothing Negative' mod by voidptr installed. Please remove it to use Cult of QoL's No Negative Traits feature.", false);
        return true;
    }

    private static void HideBepInEx()
    {
        BepInEx.Bootstrap.Chainloader.ManagerObject.hideFlags = HideFlags.HideAndDontSave;
        ThreadingHelper.Instance.gameObject.hideFlags = HideFlags.HideAndDontSave;
        DontDestroyOnLoad(BepInEx.Bootstrap.Chainloader.ManagerObject);
        DontDestroyOnLoad(ThreadingHelper.Instance.gameObject);
    }

    private void RefreshConfigCache()
    {
        _cachedDirectLoadValue = DirectLoadSave.Value;
        _cachedEnableQuickSaveValue = EnableQuickSaveShortcut.Value;
        _cachedDisableAdsValue = DisableAds.Value;
        _cachedDirectLoadSkipKey = DirectLoadSkipKey.Value;
        _cachedSaveShortcut = SaveKeyboardShortcut.Value;
        _configCacheValid = true;
    }

    private void InvalidateConfigCache()
    {
        _configCacheValid = false;
    }

    private void Update()
    {
        // Performance optimization: Cache config values to avoid per-frame access
        if (!_configCacheValid)
        {
            RefreshConfigCache();
        }

        // Direct load skip key handling
        if (_cachedDirectLoadValue && (_cachedDirectLoadSkipKey.IsPressed() || _cachedDirectLoadSkipKey.IsUp() || _cachedDirectLoadSkipKey.IsDown()))
        {
            if (!MenuCleanupPatches.SkipAutoLoad)
            {
                Log.LogWarning($"{_cachedDirectLoadSkipKey.MainKey} pressed; skipping auto-load.");
            }

            MenuCleanupPatches.SkipAutoLoad = true;
        }

        // Quick save handling
        if (_cachedEnableQuickSaveValue && _cachedSaveShortcut.IsUp())
        {
            SaveAndLoad.Save();
            NotificationCentre.Instance.PlayGenericNotification("Game Saved!");
        }

        // Ad disabling - cache UI controller and components to avoid repeated queries
        if (_cachedDisableAdsValue)
        {
            if (!_cachedUIMainMenuController)
            {
                _cachedUIMainMenuController = UIMainMenuController;
            }

            if (_cachedUIMainMenuController)
            {
                if (_cachedAdComponents == null)
                {
                    _cachedAdComponents = _cachedUIMainMenuController.ad.GetComponents<Component>();
                }

                // Only disable if not already disabled
                if (_cachedUIMainMenuController.ad.gameObject.activeSelf)
                {
                    foreach (var comp in _cachedAdComponents)
                    {
                        comp.gameObject.SetActive(false);
                    }

                    _cachedUIMainMenuController.ad.gameObject.SetActive(false);
                }
            }
        }
    }
}