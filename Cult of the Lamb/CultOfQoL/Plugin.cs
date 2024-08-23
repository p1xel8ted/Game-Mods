namespace CultOfQoL;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("io.github.xhayper.COTL_API", BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency("com.bepis.bepinex.configurationmanager", "18.3")]
[BepInDependency("dev.mamallama.lcm.dbgasst", "1.2.0")]
public partial class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.cotl.CultOfQoLCollection";
    internal const string PluginName = "The Cult of QoL Collection";
    private const string PluginVer = "2.2.5";

    private const string RestartGameMessage = "You must restart the game for these changes to take effect, as in totally exit to desktop and restart the game.\n\n** indicates a restart is required if the setting is changed.";
    private const string GeneralSection = "01. General";
    private const string MenuCleanupSection = "02. Menu Cleanup";
    private const string GameMechanicsSection = "03. Game Mechanics";
    private const string PlayerDamageSection = "05. Player Damage";
    private const string PlayerSpeedSection = "06. Player Speed";

    private const string SavesSection = "07. Saves";

    // private const string ScalingSection = "08. Scale";
    private const string WeatherSection = "09. Weather";
    private const string NotificationsSection = "10. Notifications";

    private const string FollowersSection = "11. Followers";

    // private const string FarmSection = "12. Farm";
    private const string GameSpeedSection = "13. Game Speed";
    private const string CapacitySection = "14. Capacities";

    private const string AutoInteractSection = "15. Auto-Interact (Chests)";

    // private const string PropagandaSection = "16. Propaganda Structure";
    private const string MinesSection = "17. Mines";
    private const string MassSection = "18. Mass Actions";
    private const string StructureSection = "04. Structures";
    private const string TraitsSection = "19. Traits";

    internal static ManualLogSource Log { get; private set; }

    // internal static CanvasScaler GameCanvasScaler { get; set; }
    // internal static CanvasScaler DungeonCanvasScaler { get; set; }
    internal static PopupManager PopupManager { get; private set; }
    private void Awake()
    {
        HideBepInEx();

        Log = Logger;

        PopupManager = gameObject.AddComponent<PopupManager>();

        //General
        EnableLogging = Config.Bind(GeneralSection, "Enable Logging", false, new ConfigDescription("Enable/disable logging.", null, new ConfigurationManagerAttributes {Order = 4}));
        SkipDevIntros = Config.Bind(GeneralSection, "Skip Intros", true, new ConfigDescription("Skip splash screens.", null, new ConfigurationManagerAttributes {Order = 3}));
        SkipCrownVideo = Config.Bind(GeneralSection, "Skip Crown Video", true, new ConfigDescription("Skips the video when the lamb gets given the crown.", null, new ConfigurationManagerAttributes {Order = 2}));
        UnlockTwitchItems = Config.Bind(GeneralSection, "Unlock Twitch Items", true, new ConfigDescription("Unlock pre-order DLC, Twitch plush, and Twitch drops. Paid DLC is excluded on purpose.", null, new ConfigurationManagerAttributes {Order = 1}));

        //Menu Cleanup
        RemoveMenuClutter = Config.Bind(MenuCleanupSection, "Remove Extra Menu Buttons", true, new ConfigDescription("Removes credits/road-map/discord buttons from the menus.", null, new ConfigurationManagerAttributes {Order = 6}));
        RemoveTwitchButton = Config.Bind(MenuCleanupSection, "Remove Twitch Buttons", true, new ConfigDescription("Removes twitch buttons from the menus.", null, new ConfigurationManagerAttributes {Order = 5}));
        DisableAds = Config.Bind(MenuCleanupSection, "Disable Ads", true, new ConfigDescription("Disables the new ad 'feature'.", null, new ConfigurationManagerAttributes {Order = 4}));
        RemoveHelpButtonInPauseMenu = Config.Bind(MenuCleanupSection, "Remove Help Button In Pause Menu", true, new ConfigDescription("Removes the help button in the pause menu.", null, new ConfigurationManagerAttributes {Order = 3}));
        RemoveTwitchButtonInPauseMenu = Config.Bind(MenuCleanupSection, "Remove Twitch Button In Pause Menu", true, new ConfigDescription("Removes the twitch button in the pause menu.", null, new ConfigurationManagerAttributes {Order = 2}));
        RemovePhotoModeButtonInPauseMenu = Config.Bind(MenuCleanupSection, "Remove Photo Mode Button In Pause Menu", true, new ConfigDescription("Removes the photo mode button in the pause menu.", null, new ConfigurationManagerAttributes {Order = 1}));
        MainMenuGlitch = Config.Bind(MenuCleanupSection, "Main Menu Glitch", false, new ConfigDescription("Disables the sudden dark-mode switch effect.", null, new ConfigurationManagerAttributes {Order = 0}));
        MainMenuGlitch.SettingChanged += (_, _) =>
        {
            UpdateMenuGlitch();
        };
        //Player Damage
        EnableBaseDamageMultiplier = Config.Bind(PlayerDamageSection, "Enable Base Damage Multiplier", false, new ConfigDescription("Enable/disable the base damage multiplier.", null, new ConfigurationManagerAttributes {Order = 6}));
        BaseDamageMultiplier = Config.Bind(PlayerDamageSection, "Base Damage Multiplier", 1.5f, new ConfigDescription("The base damage multiplier to use.", new AcceptableValueRange<float>(1.25f, 100), new ConfigurationManagerAttributes {Order = 5}));
        BaseDamageMultiplier.SettingChanged += (_, _) =>
        {
            BaseDamageMultiplier.Value = Mathf.Round(BaseDamageMultiplier.Value * 4) / 4;
        };
        ReverseGoldenFleeceDamageChange = Config.Bind(GameMechanicsSection, "Reverse Golden Fleece Change", true, new ConfigDescription("Reverts the default damage increase to 10% instead of 5%.", null, new ConfigurationManagerAttributes {Order = 4}));
        IncreaseGoldenFleeceDamageRate = Config.Bind(GameMechanicsSection, "Increase Golden Fleece Rate", true, new ConfigDescription("Doubles the damage increase.", null, new ConfigurationManagerAttributes {Order = 3}));
        UseCustomDamageValue = Config.Bind(GameMechanicsSection, "Use Custom Damage Value", false, new ConfigDescription("Use a custom damage value instead of the default 10%.", null, new ConfigurationManagerAttributes {Order = 2}));
        CustomDamageMulti = Config.Bind(GameMechanicsSection, "Custom Damage Multiplier", 2.0f, new ConfigDescription("The custom damage multiplier to use. Based off the games default 5%.", new AcceptableValueRange<float>(1.25f, 100), new ConfigurationManagerAttributes {Order = 1}));
        CustomDamageMulti.SettingChanged += (_, _) =>
        {
            CustomDamageMulti.Value = Mathf.Round(CustomDamageMulti.Value * 4) / 4;
        };

        //Player Speed
        EnableRunSpeedMulti = Config.Bind(PlayerSpeedSection, "Enable Run Speed Multiplier", true, new ConfigDescription("Enable/disable the run speed multiplier.", null, new ConfigurationManagerAttributes {Order = 8}));
        RunSpeedMulti = Config.Bind(PlayerSpeedSection, "Run Speed Multiplier", 1.5f, new ConfigDescription("How much faster the player runs.", new AcceptableValueRange<float>(1.25f, 100), new ConfigurationManagerAttributes {Order = 7}));
        RunSpeedMulti.SettingChanged += (_, _) =>
        {
            RunSpeedMulti.Value = Mathf.Round(RunSpeedMulti.Value * 4) / 4;
        };
        DisableRunSpeedInDungeons = Config.Bind(PlayerSpeedSection, "Disable Run Speed In Dungeons", true, new ConfigDescription("Disables the run speed multiplier in dungeons.", null, new ConfigurationManagerAttributes {Order = 6}));
        DisableRunSpeedInCombat = Config.Bind(PlayerSpeedSection, "Disable Run Speed In Combat", true, new ConfigDescription("Disables the run speed multiplier in combat.", null, new ConfigurationManagerAttributes {Order = 5}));
        
        EnableDodgeSpeedMulti = Config.Bind(PlayerSpeedSection, "Enable Dodge Speed Multiplier", true, new ConfigDescription("Enable/disable the dodge speed multiplier.", null, new ConfigurationManagerAttributes {Order = 4}));
        DodgeSpeedMulti = Config.Bind(PlayerSpeedSection, "Dodge Speed Multiplier", 1.5f, new ConfigDescription("How much faster the player dodges.", new AcceptableValueRange<float>(1.25f, 100), new ConfigurationManagerAttributes {Order = 3}));
        DodgeSpeedMulti.SettingChanged += (_, _) =>
        {
            DodgeSpeedMulti.Value = Mathf.Round(DodgeSpeedMulti.Value * 4) / 4;
        };
        EnableLungeSpeedMulti = Config.Bind(PlayerSpeedSection, "Enable Lunge Speed Multiplier", true, new ConfigDescription("Enable/disable the lunge speed multiplier.", null, new ConfigurationManagerAttributes {Order = 2}));
        LungeSpeedMulti = Config.Bind(PlayerSpeedSection, "Lunge Speed Multiplier", 1.5f, new ConfigDescription("How much faster the player lunges.", new AcceptableValueRange<float>(1.25f, 100), new ConfigurationManagerAttributes {Order = 1}));
        LungeSpeedMulti.SettingChanged += (_, _) =>
        {
            LungeSpeedMulti.Value = Mathf.Round(LungeSpeedMulti.Value * 4) / 4;
        };
        //Save
        SaveOnQuitToDesktop = Config.Bind(SavesSection, "Save On Quit To Desktop", true, new ConfigDescription("Modify the confirmation dialog to save the game when you quit to desktop.", null, new ConfigurationManagerAttributes {Order = 8}));
        SaveOnQuitToMenu = Config.Bind(SavesSection, "Save On Quit To Menu", true, new ConfigDescription("Modify the confirmation dialog to save the game when you quit to menu.", null, new ConfigurationManagerAttributes {Order = 7}));
        HideNewGameButtons = Config.Bind(SavesSection, "Hide New Game Button (s)", true, new ConfigDescription("Hides the new game button if you have at least one save game.", null, new ConfigurationManagerAttributes {Order = 6}));
        EnableQuickSaveShortcut = Config.Bind(SavesSection, "Enable Quick Save Shortcut", true, new ConfigDescription("Enable/disable the quick save keyboard shortcut.", null, new ConfigurationManagerAttributes {Order = 5}));
        SaveKeyboardShortcut = Config.Bind(SavesSection, "Save Keyboard Shortcut", new KeyboardShortcut(KeyCode.F5), new ConfigDescription("The keyboard shortcut to save the game.", null, new ConfigurationManagerAttributes {Order = 4}));
        DirectLoadSave = Config.Bind(SavesSection, "Direct Load Save", false, new ConfigDescription("Directly load the specified save game instead of showing the save menu.", null, new ConfigurationManagerAttributes {Order = 3}));
        DirectLoadSkipKey = Config.Bind(SavesSection, "Direct Load Skip Key", new KeyboardShortcut(KeyCode.LeftShift), new ConfigDescription("The keyboard shortcut to skip the auto-load when loading the game.", null, new ConfigurationManagerAttributes {Order = 2}));
        SaveSlotToLoad = Config.Bind(SavesSection, "Save Slot To Load", 1, new ConfigDescription("The save slot to load.", new AcceptableValueList<int>(1, 2, 3), new ConfigurationManagerAttributes {Order = 1}));
        SaveSlotToLoad.SettingChanged += (_, _) =>
        {
            if (!SaveAndLoad.SaveExist(SaveSlotToLoad.Value))
            {
                L($"The slot you have select doesn't contain a save game.");
                return;
            }
            L($"Save slot to load changed to {SaveSlotToLoad.Value}");
        };

        //Scale
        // EnableCustomUiScale = Config.Bind(ScalingSection, "Enable Custom UI Scale", false, new ConfigDescription("Enable/disable the custom UI scale.", null, new ConfigurationManagerAttributes {Order = 2}));
        // EnableCustomUiScale.SettingChanged += (_, _) =>
        // {
        //   Scales.ChangeAllScalers();
        // };
        //
        // CustomUiScale = Config.Bind(ScalingSection, "Custom UI Scale", 1f, new ConfigDescription("The custom UI scale to use.", new AcceptableValueRange<float>(0.10f, 101f), new ConfigurationManagerAttributes {Order = 1}));
        // CustomUiScale.SettingChanged += (_, _) =>
        // {
        //     Scales.ChangeAllScalers();
        // };
        //
        // NotificationsScale = Config.Bind(ScalingSection, "Notifications Scale", 100, new ConfigDescription("The scale to use for notifications. This setting is independent of Custom UI Scale", new AcceptableValueRange<int>(1, 101), new ConfigurationManagerAttributes {Order = 0}));

        //Weather
        ChangeWeatherOnPhaseChange = Config.Bind(WeatherSection, "Change Weather On Phase Change", true, new ConfigDescription("By default, the game changes weather when you exit a structure, or on a new day. Enabling this makes the weather change on each phase i.e. morning, noon, evening, night.", null, new ConfigurationManagerAttributes {Order = 2}));
        RandomWeatherChangeWhenExitingArea = Config.Bind(WeatherSection, "Random Weather Change When Exiting Area", true, new ConfigDescription("When exiting a building/area, the weather will change to a random weather type instead of the previous weather.", null, new ConfigurationManagerAttributes {Order = 1}));

        //Game Mechanics
        EasyFishing = Config.Bind(GameMechanicsSection, "Disable Fishing Mini-Game", true, new ConfigDescription("Fishing mini-game cheese. Just cast and let the mod do the rest.", null, new ConfigurationManagerAttributes {DispName = "Disable Fishing Mini-Game**", Order = 3}));
        EasyFishing.SettingChanged += (_, _) => ShowRestartMessage();

        DisableGameOver = Config.Bind(GameMechanicsSection, "No More Game-Over", false, new ConfigDescription("Disables the game over function when you have 0 followers for consecutive days.", null, new ConfigurationManagerAttributes {Order = 2}));
        ThriceMultiplyTarotCardLuck = Config.Bind(GameMechanicsSection, "3x Tarot Luck", true, new ConfigDescription("Luck changes with game difficulty, this will multiply your luck multiplier by 3 for drawing rarer tarot cards.", null, new ConfigurationManagerAttributes {Order = 1}));
        RareTarotCardsOnly = Config.Bind(GameMechanicsSection, "Rare Tarot Cards Only", true, new ConfigDescription("Only draw rare tarot cards.", null, new ConfigurationManagerAttributes {Order = 0}));

        //Mines
        LumberAndMiningStationsDontAge = Config.Bind(MinesSection, "Infinite Lumber & Mining Stations", false, new ConfigDescription("Lumber and mining stations should never run out and collapse. Takes 1st priority.", null, new ConfigurationManagerAttributes {Order = 3}));
        LumberAndMiningStationsDontAge.SettingChanged += (_, _) =>
        {
            if (!LumberAndMiningStationsDontAge.Value) return;
            DoubleLifespanInstead.Value = false;
            FiftyPercentIncreaseToLifespanInstead.Value = false;
        };

        DoubleLifespanInstead = Config.Bind(MinesSection, "Double Life Span Instead", false, new ConfigDescription("Doubles the life span of lumber/mining stations. Takes 2nd priority.", null, new ConfigurationManagerAttributes {Order = 2}));
        DoubleLifespanInstead.SettingChanged += (_, _) =>
        {
            if (!DoubleLifespanInstead.Value) return;
            LumberAndMiningStationsDontAge.Value = false;
            FiftyPercentIncreaseToLifespanInstead.Value = false;
        };

        FiftyPercentIncreaseToLifespanInstead = Config.Bind(MinesSection, "Add 50% to Life Span Instead", true, new ConfigDescription("For when double is too long for your tastes. This will extend their life by 50% instead of 100%. Takes 3rd priority.", null, new ConfigurationManagerAttributes {Order = 1}));
        FiftyPercentIncreaseToLifespanInstead.SettingChanged += (_, _) =>
        {
            if (!FiftyPercentIncreaseToLifespanInstead.Value) return;
            LumberAndMiningStationsDontAge.Value = false;
            DoubleLifespanInstead.Value = false;
        };

        //Structures
        TurnOffSpeakersAtNight = Config.Bind(StructureSection, "Turn Off Speakers At Night", true, new ConfigDescription("Turns the speakers off, and stops fuel consumption at night time.", null, new ConfigurationManagerAttributes {Order = 5}));
        DisablePropagandaSpeakerAudio = Config.Bind(StructureSection, "Disable Propaganda Speaker Audio", true, new ConfigDescription("Disables the audio from propaganda speakers.", null, new ConfigurationManagerAttributes {Order = 4}));
        AddExhaustedToHealingBay = Config.Bind(StructureSection, "Add Exhausted To Healing Bay", true, new ConfigDescription("Allows you to select exhausted followers for rest and relaxation in the healing bays.", null, new ConfigurationManagerAttributes {Order = 3}));
        OnlyShowDissenters = Config.Bind(StructureSection, "Only Show Dissenters In Prison Menu", true, new ConfigDescription("Only show dissenting followers when interacting with the prison.", null, new ConfigurationManagerAttributes {Order = 2}));
        AdjustRefineryRequirements = Config.Bind(StructureSection, "Adjust Refinery Requirements", true, new ConfigDescription("Where possible, halves the materials needed to convert items in the refinery. Rounds up.", null, new ConfigurationManagerAttributes {Order = 1}));

        //Speed
        EnableGameSpeedManipulation = Config.Bind(GameSpeedSection, "Enable Game Speed Manipulation", true, new ConfigDescription("Use left/right arrows keys to increase/decrease game speed in 0.25 increments. Up arrow to reset to default.", null, new ConfigurationManagerAttributes {Order = 5}));
        ShortenGameSpeedIncrements = Config.Bind(GameSpeedSection, "Shorten Game Speed Increments", false, new ConfigDescription("Increments in steps of 1, instead of 0.25.", null, new ConfigurationManagerAttributes {Order = 4}));
        FastCollecting = Config.Bind(GameSpeedSection, "Speed Up Collection", true, new ConfigDescription("Increases the rate you can collect from the shrines, and other structures.", null, new ConfigurationManagerAttributes {DispName = "Speed Up Collection**", Order = 3}));
        FastCollecting.SettingChanged += (_, _) => ShowRestartMessage();
        SlowDownTime = Config.Bind(GameSpeedSection, "Slow Down Time", false, new ConfigDescription("Enables the ability to slow down time. This is different to the increase speed implementation. This will make the days longer, but not slow down animations.", null, new ConfigurationManagerAttributes {Order = 2}));
        SlowDownTimeMultiplier = Config.Bind(GameSpeedSection, "Slow Down Time Multiplier", 2f, new ConfigDescription("The multiplier to use for slow down time. For example, the default value of 2 is making the day twice as long.", new AcceptableValueRange<float>(1.25f, 100f), new ConfigurationManagerAttributes {Order = 1}));
        SlowDownTimeMultiplier.SettingChanged += (_, _) =>
        {
            SlowDownTimeMultiplier.Value = Mathf.Round(SlowDownTimeMultiplier.Value * 4) / 4;
        };
        FastRitualSermons = Config.Bind(GameSpeedSection, "Fast Rituals & Sermons", true, new ConfigDescription("Speeds up rituals and sermons.", null, new ConfigurationManagerAttributes {Order = 0}));
        FastRitualSermons.SettingChanged += (_, _) =>
        {
            if (FastRitualSermons.Value) return;
            RitualSermonSpeed.RitualRunning = false;
            GameManager.SetTimeScale(1);
        };

        //Chest Auto-Interact
        EnableAutoInteract = Config.Bind(AutoInteractSection, "Enable Auto Interact", true, new ConfigDescription("Makes chests automatically send you the resources when you're nearby.", null, new ConfigurationManagerAttributes
        {
            Order = 5
        }));
        AutoCollectFromFarmStationChests = Config.Bind(AutoInteractSection, "Auto Collect From Farm Station Chests", true, new ConfigDescription("Automatically collect from farm station chests.", null, new ConfigurationManagerAttributes
        {
            Order = 4
        }));
        TriggerAmount = Config.Bind(AutoInteractSection, "Trigger Amount", 5, new ConfigDescription("The amount of resources needed to trigger the auto-interact.", new AcceptableValueRange<int>(1, 100), new ConfigurationManagerAttributes
        {
            Order = 4, ShowRangeAsPercent = false
        }));
        IncreaseAutoCollectRange = Config.Bind(AutoInteractSection, "Double Activation Range", true, new ConfigDescription("The default range is 5. This will increase it to 10.", null, new ConfigurationManagerAttributes
        {
            Order = 3
        }));
        UseCustomAutoInteractRange = Config.Bind(AutoInteractSection, "Use Custom Range", false, new ConfigDescription("Use a custom range instead of the default or increased range.", null, new ConfigurationManagerAttributes
        {
            Order = 2
        }));
        CustomAutoInteractRangeMulti = Config.Bind(AutoInteractSection, "Custom Range Multiplier", 2.0f, new ConfigDescription("Enter a multiplier to use for auto-collect range when using custom range.", new AcceptableValueRange<float>(1.25f, 100), new ConfigurationManagerAttributes
        {
            Order = 1
        }));
        CustomAutoInteractRangeMulti.SettingChanged += (_, _) =>
        {
            CustomAutoInteractRangeMulti.Value = Mathf.Round(CustomAutoInteractRangeMulti.Value * 4) / 4;
        };

        //Capacity
        UseMultiplesOf32 = Config.Bind(CapacitySection, "Use Multiples of 32", true, new ConfigDescription("Use multiples of 32 for silo capacity.", null, new ConfigurationManagerAttributes
        {
            Order = 7
        }));
        DoubleSiloCapacity = Config.Bind(CapacitySection, "Double Silo Capacity", true, new ConfigDescription("Doubles the silo capacity of applicable structures.", null, new ConfigurationManagerAttributes
        {
            Order = 6
        }));
        UseCustomSiloCapacity = Config.Bind(CapacitySection, "Use Custom Silo Capacity", false, new ConfigDescription("Use a custom silo capacity instead of the default or increased capacity.", null, new ConfigurationManagerAttributes
        {
            Order = 5
        }));
        CustomSiloCapacityMulti = Config.Bind(CapacitySection, "Custom Silo Capacity Multiplier", 2.0f, new ConfigDescription("Enter a multiplier to use for silo capacity when using custom capacity.", new AcceptableValueRange<float>(1.25f, 1000), null, new ConfigurationManagerAttributes
        {
            Order = 4
        }));
        CustomSiloCapacityMulti.SettingChanged += (_, _) =>
        {
            CustomSiloCapacityMulti.Value = Mathf.Round(CustomSiloCapacityMulti.Value * 4) / 4;
        };
        DoubleSoulCapacity = Config.Bind(CapacitySection, "Double Soul Capacity", true, new ConfigDescription("Doubles the soul capacity of applicable structures.", null, new ConfigurationManagerAttributes
        {
            Order = 3
        }));
        UseCustomSoulCapacity = Config.Bind(CapacitySection, "Use Custom Soul Capacity", false, new ConfigDescription("Use a custom soul capacity instead of the default or doubled capacity.", null, new ConfigurationManagerAttributes
        {
            Order = 2
        }));
        CustomSoulCapacityMulti = Config.Bind(CapacitySection, "Custom Soul Capacity Multiplier", 2.0f, new ConfigDescription("Enter a multiplier to use for soul capacity when using custom capacity.", new AcceptableValueRange<float>(1.25f, 1000f), null, new ConfigurationManagerAttributes
        {
            Order = 1
        }));
        CustomSoulCapacityMulti.SettingChanged += (_, _) =>
        {
            CustomSoulCapacityMulti.Value = Mathf.Round(CustomSoulCapacityMulti.Value * 4) / 4;
        };

        //Notifications
        NotifyOfScarecrowTraps = Config.Bind(NotificationsSection, "Notify of Scarecrow Traps", true, new ConfigDescription("Display a notification when the farm scarecrows have caught a trap!", null, new ConfigurationManagerAttributes
        {
            Order = 5
        }));
        NotifyOfNoFuel = Config.Bind(NotificationsSection, "Notify of No Fuel", true, new ConfigDescription("Display a notification when a structure has run out of fuel.", null, new ConfigurationManagerAttributes
        {
            Order = 4
        }));
        NotifyOfBedCollapse = Config.Bind(NotificationsSection, "Notify of Bed Collapse", true, new ConfigDescription("Display a notification when a bed has collapsed.", null, new ConfigurationManagerAttributes
        {
            Order = 3
        }));
        ShowPhaseNotifications = Config.Bind(NotificationsSection, "Phase Notifications", true, new ConfigDescription("Show a notification when the time of day changes.", null, new ConfigurationManagerAttributes
        {
            Order = 2
        }));
        ShowWeatherChangeNotifications = Config.Bind(NotificationsSection, "Weather Change Notifications", true, new ConfigDescription("Show a notification when the weather changes.", null, new ConfigurationManagerAttributes
        {
            Order = 1
        }));

        //Followers
        GiveFollowersNewNecklaces = Config.Bind(FollowersSection, "Give Followers New Necklaces", true, new ConfigDescription("Followers will be able to receive new necklaces, with the old one being returned to you.", null, new ConfigurationManagerAttributes
        {
            Order = 5
        }));
        CleanseIllnessAndExhaustionOnLevelUp = Config.Bind(FollowersSection, "Cleanse Illness and Exhaustion", true, new ConfigDescription("When a follower 'levels up', if they are sick or exhausted, the status is cleansed.", null, new ConfigurationManagerAttributes
        {
            Order = 4
        }));
        CollectTitheFromOldFollowers = Config.Bind(FollowersSection, "Collect Tithe From Old Followers", true, new ConfigDescription("Enable collecting tithe from the elderly.", null, new ConfigurationManagerAttributes
        {
            Order = 3
        }));
        IntimidateOldFollowers = Config.Bind(FollowersSection, "Intimidate Old Followers", true, new ConfigDescription("Enable intimidating the elderly.", null, new ConfigurationManagerAttributes
        {
            Order = 2
        }));
        RemoveLevelLimit = Config.Bind(FollowersSection, "Remove Level Limit", true, new ConfigDescription("Removes the level limit for followers. They can now level up infinitely.", null, new ConfigurationManagerAttributes
        {
            Order = 1
        }));
        MakeOldFollowersWork = Config.Bind(FollowersSection, "Make Old Followers Work", true, new ConfigDescription("Enable the elderly to work.", null, new ConfigurationManagerAttributes
        {
            Order = 0
        }));

        //Traits
        NoNegativeTraits = Config.Bind(TraitsSection, "No Negative Traits", true, new ConfigDescription("Negative traits will be replaced based on the configuration here.", null, new ConfigurationManagerAttributes
        {
            Order = 3
        }));
        NoNegativeTraits.SettingChanged += (_, _) =>
        {
            UpdateNoNegativeTraits();
        };
        UseUnlockedTraitsOnly = Config.Bind(TraitsSection, "Use Unlocked Traits Only", true, new ConfigDescription("Only use unlocked traits when replacing negative traits.", null, new ConfigurationManagerAttributes
        {
            Order = 2
        }));
        UseUnlockedTraitsOnly.SettingChanged += (_, _) =>
        {
            GenerateAvailableTraits();
        };
        IncludeImmortal = Config.Bind(TraitsSection, "Include Immortal", false, new ConfigDescription("Include the Immortal trait when replacing negative traits.", null, new ConfigurationManagerAttributes
        {
            Order = 1
        }));
        IncludeImmortal.SettingChanged += (_, _) =>
        {
            GenerateAvailableTraits();
        };
        IncludeDisciple = Config.Bind(TraitsSection, "Include Disciple", false, new ConfigDescription("Include the Disciple trait when replacing negative traits.", null, new ConfigurationManagerAttributes
        {
            Order = 0
        }));
        IncludeDisciple.SettingChanged += (_, _) =>
        {
            GenerateAvailableTraits();
        };

        ShowNotificationsWhenRemovingTraits = Config.Bind(TraitsSection, "Show Notifications When Removing Traits", false, new ConfigDescription("Show notifications when removing negative traits.", null, new ConfigurationManagerAttributes
        {
            Order = 0
        }));
        ShowNotificationsWhenAddingTraits = Config.Bind(TraitsSection, "Show Notifications When Adding Traits", false, new ConfigDescription("Show notifications when adding positive traits.", null, new ConfigurationManagerAttributes
        {
            Order = 0
        }));

        //Mass Section
        MassLevelUp = Config.Bind(MassSection, "Mass Level Up", true, new ConfigDescription("When interacting with a follower than can level, all eligible followers will be leveled up.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Level Up**", Order = 18
        }));
        MassLevelUp.SettingChanged += (_, _) => ShowRestartMessage();

        MassFertilize = Config.Bind(MassSection, "Mass Fertilize", true, new ConfigDescription("When fertilizing a plot, all farm plots are fertilized at once.", null, new ConfigurationManagerAttributes
        {
            Order = 17
        }));


        MassWater = Config.Bind(MassSection, "Mass Water", true, new ConfigDescription("When watering a plot, all farm plots are watered at once.", null, new ConfigurationManagerAttributes
        {
            Order = 16
        }));
        // MassHarvest = Config.Bind(MassSection, "Mass Harvest", true, new ConfigDescription("When harvesting a plot, all farm plots are harvested at once.", null, new ConfigurationManagerAttributes
        // {
        //     Order = 15
        // }));

        MassBribe = Config.Bind(MassSection, "Mass Bribe", true, new ConfigDescription("When bribing a follower, all followers are bribed at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Bribe**", Order = 15
        }));
        MassBribe.SettingChanged += (_, _) => ShowRestartMessage();

        MassBless = Config.Bind(MassSection, "Mass Bless", true, new ConfigDescription("When blessing a follower, all followers are blessed at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Bless**", Order = 14
        }));
        MassBless.SettingChanged += (_, _) => ShowRestartMessage();

        MassExtort = Config.Bind(MassSection, "Mass Extort", true, new ConfigDescription("When extorting a follower, all followers are extorted at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Extort**", Order = 13
        }));
        MassExtort.SettingChanged += (_, _) => ShowRestartMessage();

        MassPetDog = Config.Bind(MassSection, "Mass Pet Dog", true, new ConfigDescription("When petting a a follower, all followers are petted at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Pet Dog**", Order = 12
        }));
        MassPetDog.SettingChanged += (_, _) => ShowRestartMessage();

        MassIntimidate = Config.Bind(MassSection, "Mass Intimidate", true, new ConfigDescription("When intimidating a follower, all followers are intimidated at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Intimidate**", Order = 11
        }));
        MassIntimidate.SettingChanged += (_, _) => ShowRestartMessage();

        MassInspire = Config.Bind(MassSection, "Mass Inspire", true, new ConfigDescription("When inspiring a follower, all followers are inspired at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Inspire**", Order = 10
        }));
        MassInspire.SettingChanged += (_, _) => ShowRestartMessage();

        MassCollectFromBeds = Config.Bind(MassSection, "Mass Collect From Beds", true, new ConfigDescription("When collecting resources from a bed, all beds are collected from at once.", null, new ConfigurationManagerAttributes
        {
            Order = 9
        }));


        MassCollectFromOuthouses = Config.Bind(MassSection, "Mass Collect From Outhouses", true, new ConfigDescription("When collecting resources from an outhouse, all outhouses are collected from at once.", null, new ConfigurationManagerAttributes
        {
            Order = 8
        }));


        MassCollectFromOfferingShrines = Config.Bind(MassSection, "Mass Collect From Offering Shrines", true, new ConfigDescription("When collecting resources from an offering shrine, all offering shrines are collected from at once.", null, new ConfigurationManagerAttributes
        {
            Order = 7
        }));


        MassCollectFromPassiveShrines = Config.Bind(MassSection, "Mass Collect From Passive Shrines", true, new ConfigDescription("When collecting resources from a passive shrine, all passive shrines are collected from at once.", null, new ConfigurationManagerAttributes
        {
            Order = 6
        }));


        MassCollectFromCompost = Config.Bind(MassSection, "Mass Collect From Compost", true, new ConfigDescription("When collecting resources from a compost, all composts are collected from at once.", null, new ConfigurationManagerAttributes
        {
            Order = 5
        }));


        MassCollectFromHarvestTotems = Config.Bind(MassSection, "Mass Collect From Harvest Totems", true, new ConfigDescription("When collecting resources from a harvest totem, all harvest totems are collected from at once.", null, new ConfigurationManagerAttributes
        {
            Order = 4
        }));


        MassRomance = Config.Bind(MassSection, "Mass Romance", true, new ConfigDescription("When romancing a follower, all followers are romanced at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Romance**", Order = 3
        }));
        MassRomance.SettingChanged += (_, _) => ShowRestartMessage();

        MassBully = Config.Bind(MassSection, "Mass Bully", true, new ConfigDescription("When bullying a follower, all followers are bullied at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Bully**", Order = 2
        }));
        MassBully.SettingChanged += (_, _) => ShowRestartMessage();

        MassReassure = Config.Bind(MassSection, "Mass Reassure", true, new ConfigDescription("When reassuring a follower, all followers are reassured at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Reassure**", Order = 1
        }));
        MassReassure.SettingChanged += (_, _) => ShowRestartMessage();

        MassReeducate = Config.Bind(MassSection, "Mass Reeducate", true, new ConfigDescription("When reeducating a follower, all followers are reeducated at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Reeducate**", Order = 0
        }));
        MassReeducate.SettingChanged += (_, _) => ShowRestartMessage();

        //Loot
        AllLootMagnets = Config.Bind("20. Loot", "All Loot Magnets", true, new ConfigDescription("All loot is magnetized to you.", null, new ConfigurationManagerAttributes
        {
            Order = 0
        }));
        AllLootMagnets.SettingChanged += (_, _) =>
        {
            UpdateAllMagnets();
        };
        DoubleMagnetRange = Config.Bind("20. Loot", "Double Magnet Range", true, new ConfigDescription("Doubles the magnet range.", null, new ConfigurationManagerAttributes
        {
            Order = 0
        }));
        DoubleMagnetRange.SettingChanged += (_, _) =>
        {
            UpdateDoubleMagnet();
        };

        TripleMagnetRange = Config.Bind("20. Loot", "Triple Magnet Range", false, new ConfigDescription("Triples the magnet range.", null, new ConfigurationManagerAttributes
        {
            Order = 0
        }));
        TripleMagnetRange.SettingChanged += (_, _) =>
        {
            UpdateTripleMagnet();
        };

        UseCustomMagnetRange = Config.Bind("20. Loot", "Use Custom Magnet Range", false, new ConfigDescription("Use a custom magnet range instead of the default or increased range.", null, new ConfigurationManagerAttributes
        {
            Order = 0
        }));
        UseCustomMagnetRange.SettingChanged += (_, _) =>
        {
            UpdatePickups();
        };
        CustomMagnetRange = Config.Bind("20. Loot", "Custom Magnet Range", 7, new ConfigDescription("Quadruples the magnet range.", new AcceptableValueRange<int>(7, 50), new ConfigurationManagerAttributes
        {
            Order = 0
        }));
        CustomMagnetRange.SettingChanged += (_, _) =>
        {
            UpdateCustomMagnet();
        };

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);

        SceneManager.sceneLoaded += OnSceneLoaded;

        Logger.LogInfo($"Plugin {PluginName} is loaded! Running game version {Application.version} on {MonoMod.Utils.PlatformHelper.Current}.");
        // if (!SoftDepend.Enabled) return;
        //
        // SoftDepend.AddSettingsMenus();
        // Log.LogInfo("API detected - You can configure mod settings in the settings menu."));
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
            Patches.NoNegativeTraits.UpdateAllFollowerTraits();
        }
        else
        {
            Patches.NoNegativeTraits.RestoreOriginalTraits();
        }
    }
    private static void GenerateAvailableTraits()
    {
        if (IsNoNegativePresent()) return;
        Patches.NoNegativeTraits.GenerateAvailableTraits();
    }
    private static void UpdateNavigationMode()
    {
        var buttons = Resources.FindObjectsOfTypeAll<MMButton>();
        foreach (var button in buttons)
        {
            button.Selectable.navigation = button.Selectable.navigation with {mode = Navigation.Mode.Automatic};
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
            PickUps.RestoreMagnets();
        }
    }
    private static void UpdateDoubleMagnet()
    {
        if (!DoubleMagnetRange.Value && !TripleMagnetRange.Value && !UseCustomMagnetRange.Value)
        {
            PickUps.RestoreMagnetRange();
        }

        if (DoubleMagnetRange.Value)
        {
            PickUps.UpdateAllPickUps();
            TripleMagnetRange.Value = false;
            UseCustomMagnetRange.Value = false;
        }
    }
    private static void UpdateTripleMagnet()
    {
        if (!DoubleMagnetRange.Value && !TripleMagnetRange.Value && !UseCustomMagnetRange.Value)
        {
            PickUps.RestoreMagnetRange();
        }
        if (TripleMagnetRange.Value)
        {
            PickUps.UpdateAllPickUps();
            DoubleMagnetRange.Value = false;
            UseCustomMagnetRange.Value = false;
        }
    }
    private static void UpdateCustomMagnet()
    {
        if (!UseCustomMagnetRange.Value)
        {
            return;
        }
        UseCustomMagnetRange.Value = true;
        DoubleMagnetRange.Value = false;
        TripleMagnetRange.Value = false;
        PickUps.UpdateAllPickUps();
    }
    private static void UpdatePickups()
    {
        if (!DoubleMagnetRange.Value && !TripleMagnetRange.Value && !UseCustomMagnetRange.Value)
        {
            PickUps.RestoreMagnetRange();
        }
        if (UseCustomMagnetRange.Value)
        {
            PickUps.UpdateAllPickUps();
            DoubleMagnetRange.Value = false;
            TripleMagnetRange.Value = false;
        }
    }
    private static void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        UpdateMenuGlitch();
        //  UpdateScale();
        UpdateNavigationMode();
        // UpdateAllMagnets();
        // UpdateDoubleMagnet();
        // UpdateTripleMagnet();
        // UpdateCustomMagnet();
        // UpdatePickups();
        // UpdateNoNegativeTraits();
        // Scales.ChangeAllScalers();
    }

    private static void UpdateMenuGlitch()
    {
        var mmc = FindObjectOfType<MainMenuController>();
        if (mmc)
        {
            mmc.doIntroGlitch = Plugin.MainMenuGlitch.Value;
        }
    }

    public static void L(string message)
    {
        if (EnableLogging.Value)
        {
            Log.LogInfo(message);
        }
    }


    private static void ShowRestartMessage()
    {
        if (!PopupManager.showPopup)
        {
            PopupManager.ShowPopup(RestartGameMessage, true);
        }
    }

    private static bool IsNoNegativePresent()
    {
        if (!Patches.NoNegativeTraits.IsNothingNegativePresent()) return false;
        PopupManager.ShowPopup($"You have the 'Nothing Negative' mod by voidptr installed. Please remove it to use Cult of QoL's No Negative Traits feature.", false);
        return true;
    }

    private static void HideBepInEx()
    {
        BepInEx.Bootstrap.Chainloader.ManagerObject.hideFlags = HideFlags.HideAndDontSave;
        ThreadingHelper.Instance.gameObject.hideFlags = HideFlags.HideAndDontSave;
        DontDestroyOnLoad(BepInEx.Bootstrap.Chainloader.ManagerObject);
        DontDestroyOnLoad(ThreadingHelper.Instance.gameObject);
    }

}