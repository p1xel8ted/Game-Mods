using BepInEx.Bootstrap;
using CultOfQoL.Core;
using CultOfQoL.Patches.Gameplay;
using CultOfQoL.Patches.Systems;
using CultOfQoL.Patches.UI;

namespace CultOfQoL;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("com.bepis.bepinex.configurationmanager", "18.4.1")]
public partial class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.cotl.CultOfQoLCollection";
    internal const string PluginName = "The Cult of QoL Collection";
    private const string PluginVer = "2.4.3";

    private const string RestartGameMessage = "You must restart the game for these changes to take effect, as in totally exit to desktop and restart the game.\n\n** indicates a restart is required if the setting is changed.";

    private const string BackupSaveMessage = "IMPORTANT: Please back up your save files before enabling this option.\n\nThis feature will attempt to repair missing lore tablets on your next visit to the base. While it should be safe, backing up your saves first is recommended.";

    // Section constants - General first, then alphabetical (Fixes and Reset at end)
    private const string GeneralSection = "── General ──";
    private const string AnimalsSection = "── Animals ──";
    private const string AutoInteractSection = "── Auto-Interact (Chests) ──";
    private const string CapacitySection = "── Capacities ──";
    private const string CollectionSection = "── Collection ──";
    private const string FarmSection = "── Farm ──";
    private const string FollowersSection = "── Followers ──";
    private const string GameMechanicsSection = "── Game Mechanics ──";
    private const string GameSpeedSection = "── Game Speed ──";
    private const string GoldenFleeceSection = "── Golden Fleece ──";
    private const string KnucklebonesSection = "── Knucklebones ──";
    private const string LootSection = "── Loot ──";
    private const string MassActionCostsSection = "── Mass Action Costs ──";
    private const string MassAnimalSection = "── Mass Animal ──";
    private const string MassCollectSection = "── Mass Collect ──";
    private const string MassFarmSection = "── Mass Farm ──";
    private const string MassFollowerSection = "── Mass Follower ──";
    private const string MenuCleanupSection = "── Menu Cleanup ──";
    private const string MinesSection = "── Mines ──";
    private const string NotificationsSection = "── Notifications ──";
    private const string PlayerSection = "── Player ──";
    private const string PostProcessingSection = "── Post Processing ──";
    private const string RitualSection = "── Rituals ──";
    private const string SavesSection = "── Saves ──";
    private const string SoundSection = "── Sound ──";
    private const string StructureSection = "── Structures ──";
    private const string TarotSection = "── Tarot ──";

    private const string WeatherSection = "── Weather ──";

    // Always at end
    private const string FixesSection = "── Fixes ──";
    private const string ResetAllSettingsSection = "── Reset All Settings ──";
    private const string ResetConfigFlag = "ResetConfig_CultOfQoLCollection_2.3.9";
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
    private static ConfigFile _configInstance;

    private void Awake()
    {
        HideBepInEx();
        _configInstance = Config;
        Log = Logger;

        var configResetDone = PlayerPrefs.GetInt(ResetConfigFlag, 0).ToBool();
        var currentVersion = new Version(PluginVer);
        var versionLimit = new Version("2.3.9");
        if (currentVersion >= versionLimit && !configResetDone)
        {
            WriteLog("[Config Reset] Major configuration change detected. Resetting config to defaults.", LogType.Warning);

            var configPath = Config.ConfigFilePath;
            if (File.Exists(configPath))
            {
                File.Delete(configPath);
                WriteLog($"[Config Reset] Deleted config file: {configPath}", LogType.Warning);
            }

            // Clear in-memory config (bindings will use defaults)
            Config.Clear();

            PlayerPrefs.SetInt(ResetConfigFlag, 1);
            PlayerPrefs.Save();
        }


        PopupManager = gameObject.AddComponent<PopupManager>();
        PopupManager.Title = PluginName;

        // ══════════════════════════════════════════════════════════════════════
        // Config sections: General first, then alphabetical
        // (Fixes and Reset All Settings are at the end)
        // ══════════════════════════════════════════════════════════════════════

        // ── General ──
        EnableLogging = _configInstance.Bind(GeneralSection, "Enable Logging", false, new ConfigDescription("Enable/disable logging.", null, new ConfigurationManagerAttributes { Order = 5 }));
        EnableLogging.SettingChanged += (_, _) => ConfigCache.MarkDirty(ConfigCache.Keys.EnableLogging);
        DumpTranslationsKey = _configInstance.Bind(GeneralSection, "Dump Translations Key", new KeyboardShortcut(KeyCode.F9), new ConfigDescription("When Enable Logging is on, press this key to dump all English translations to a text file in BepInEx/plugins/CultOfQoL/.", null, new ConfigurationManagerAttributes { Order = 4, DispName = "    └ Dump Translations Key" }));
        UnlockTwitchItems = _configInstance.Bind(GeneralSection, "Unlock Twitch Items", false, new ConfigDescription("Unlock pre-order DLC, Twitch plush, and Twitch drops. Paid DLC is excluded on purpose.", null, new ConfigurationManagerAttributes { Order = 1 }));

        // ── Animals ──
        DisableAnimalOldAgeDeath = _configInstance.Bind(
            AnimalsSection,
            "Immortal Farm Animals", false,
            new ConfigDescription(
                "Prevents farm animals from dying of old age.",
                null,
                new ConfigurationManagerAttributes { Order = 3 }
            )
        );

        AnimalOldAgeDeathThreshold = _configInstance.Bind(
            AnimalsSection,
            "Animal Old Age Death Threshold", 15,
            new ConfigDescription(
                "The minimum age (in days) before animals can die of old age. Vanilla default is 15.",
                new AcceptableValueRange<int>(15, 100),
                new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 2 }
            )
        );

        AnimalGuaranteedDeathAge = _configInstance.Bind(
            AnimalsSection,
            "Animal Guaranteed Death Age", 100,
            new ConfigDescription(
                "The age at which old age death becomes certain. Daily death chance scales linearly — at half this age, there's a 50% chance per day. Vanilla default is 100.",
                new AcceptableValueRange<int>(50, 1000),
                new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 1 }
            )
        );

        // ── Auto-Interact (Chests) ──
        EnableAutoCollect = _configInstance.Bind(AutoInteractSection, "Enable Auto Collect", false, new ConfigDescription("Makes chests automatically send you the resources when you're nearby.", null, new ConfigurationManagerAttributes
        {
            Order = 6, DispName = "Enable Auto Collect**"
        }));
        EnableAutoCollect.SettingChanged += (_, _) => ShowRestartMessage();
        AutoCollectFromFarmStationChests = _configInstance.Bind(AutoInteractSection, "Auto Collect From Farm Station Chests", false, new ConfigDescription("Automatically collect from farm station chests.", null, new ConfigurationManagerAttributes
        {
            Order = 5, DispName = "    └ Auto Collect From Farm Station Chests"
        }));
        TriggerAmount = _configInstance.Bind(AutoInteractSection, "Trigger Amount", 5, new ConfigDescription("The amount of resources needed to trigger the auto-interact.", new AcceptableValueRange<int>(1, 100), new ConfigurationManagerAttributes
        {
            Order = 4, ShowRangeAsPercent = false, DispName = "    └ Trigger Amount"
        }));
        AutoInteractRangeMulti = _configInstance.Bind(AutoInteractSection, "Loot Magnet Range Multiplier", 1.0f, new ConfigDescription("Enter a multiplier to use for auto-collect range when using custom range.", new AcceptableValueRange<float>(-10f, 10f), new ConfigurationManagerAttributes
        {
            Order = 1, DispName = "    └ Loot Magnet Range Multiplier"
        }));
        AutoInteractRangeMulti.SettingChanged += (_, _) => { AutoInteractRangeMulti.Value = Mathf.Round(AutoInteractRangeMulti.Value * 4) / 4; };

        // ── Capacities ──
        UseMultiplesOf32 = _configInstance.Bind(CapacitySection, "Use Multiples of 32", true, new ConfigDescription("Use multiples of 32 for silo capacity.", null, new ConfigurationManagerAttributes
        {
            Order = 7
        }));
        UseMultiplesOf32.SettingChanged += (_, _) => ConfigCache.MarkDirty(ConfigCache.Keys.UseMultiplesOf32);
        SiloCapacityMulti = _configInstance.Bind(CapacitySection, "Silo Capacity Multiplier", 1.0f, new ConfigDescription("Enter a multiplier to use for silo capacity when using custom capacity.", new AcceptableValueRange<float>(-10f, 10f), null, new ConfigurationManagerAttributes
        {
            Order = 4
        }));
        SiloCapacityMulti.SettingChanged += (_, _) =>
        {
            SiloCapacityMulti.Value = Mathf.Round(SiloCapacityMulti.Value * 4) / 4;
            ConfigCache.MarkDirty(ConfigCache.Keys.SiloCapacityMulti);
        };
        SoulCapacityMulti = _configInstance.Bind(CapacitySection, "Soul Capacity Multiplier", 1.0f, new ConfigDescription("Enter a multiplier to use for soul capacity when using custom capacity.", new AcceptableValueRange<float>(-10f, 10f), null, new ConfigurationManagerAttributes
        {
            Order = 1
        }));
        SoulCapacityMulti.SettingChanged += (_, _) =>
        {
            SoulCapacityMulti.Value = Mathf.Round(SoulCapacityMulti.Value * 4) / 4;
            ConfigCache.MarkDirty(ConfigCache.Keys.SoulCapacityMulti);
        };

        // ── Collection ──
        FastCollecting = _configInstance.Bind(CollectionSection, "Speed Up Collection", false, new ConfigDescription("Speeds up soul drain from shrines (instant) and beds (2x faster). Also removes delays when collecting from resource chests.", null, new ConfigurationManagerAttributes { DispName = "Speed Up Collection**", Order = 2 }));
        FastCollecting.SettingChanged += (_, _) => ShowRestartMessage();
        CollectShrineDevotionInstantly = _configInstance.Bind(CollectionSection, "Collect Shrine Devotion Instantly", false, new ConfigDescription("When collecting devotion from the shrine, collect all instantly instead of holding to collect.", null, new ConfigurationManagerAttributes { Order = 1 }));
        DisableSoulCameraShake = _configInstance.Bind(CollectionSection, "Disable Soul Camera Shake", false, new ConfigDescription("Disables the camera shake caused by devotion orbs and souls hitting the shrine. Does not affect combat camera shake.", null, new ConfigurationManagerAttributes { Order = 0 }));

        // ── Followers ──
        PrioritizeRequestedFollowers = _configInstance.Bind(FollowersSection, "Prioritize Requested Followers", false, new ConfigDescription("Followers with active requests (rituals, missions, mating, etc.) appear at the top of selection lists.", null, new ConfigurationManagerAttributes
        {
            Order = 9
        }));
        GiveFollowersNewNecklaces = _configInstance.Bind(FollowersSection, "Give Followers New Necklaces", false, new ConfigDescription("Followers will be able to receive new necklaces, with the old one being returned to you.", null, new ConfigurationManagerAttributes
        {
            Order = 8
        }));
        CleanseIllnessAndExhaustionOnLevelUp = _configInstance.Bind(FollowersSection, "Cleanse Illness and Exhaustion", false, new ConfigDescription("When a follower 'levels up', if they are sick or exhausted, the status is cleansed.", null, new ConfigurationManagerAttributes
        {
            Order = 7
        }));
        CollectTitheFromOldFollowers = _configInstance.Bind(FollowersSection, "Collect Tithe From Old Followers", false, new ConfigDescription("Enable collecting tithe from the elderly.", null, new ConfigurationManagerAttributes
        {
            Order = 6
        }));
        IntimidateOldFollowers = _configInstance.Bind(FollowersSection, "Intimidate Old Followers", false, new ConfigDescription("Enable intimidating the elderly.", null, new ConfigurationManagerAttributes
        {
            Order = 5
        }));
        UncapLevelBenefits = _configInstance.Bind(FollowersSection, "Uncap Level Benefits", false, new ConfigDescription("Removes the level 10 cap on follower benefits. Productivity, prayer devotion, and sacrifice rewards will scale beyond level 10. (The base game removed the level cap in 1.5.0, but benefits are still capped at level 10.)", null, new ConfigurationManagerAttributes
        {
            Order = 4
        }));
        ElderWorkMode = _configInstance.Bind(FollowersSection, "Elder Work Mode", CultOfQoL.ElderWorkMode.Disabled, new ConfigDescription("Disabled: Elders don't work (vanilla). All Work: Elders perform all tasks. Light Work Only: Elders only do light tasks like worship, cooking, brewing, and research.", null, new ConfigurationManagerAttributes
        {
            DispName = "Elder Work Mode**",
            Order = 3
        }));
        ElderWorkMode.SettingChanged += (_, _) => ShowRestartMessage();
        MinRangeLifeExpectancy = _configInstance.Bind(
            FollowersSection,
            "Minimum Range Life Expectancy", 40,
            new ConfigDescription(
                "The lowest possible life expectancy a follower can have. The game will randomly choose a value between this and the maximum value below. This must be less than the maximum.",
                new AcceptableValueRange<int>(1, 100),
                new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 2 }
            )
        );
        MinRangeLifeExpectancy.SettingChanged += (_, _) => { EnforceRangeSanity(); };
        MaxRangeLifeExpectancy = _configInstance.Bind(
            FollowersSection,
            "Maximum Range Life Expectancy", 55,
            new ConfigDescription(
                "The highest possible life expectancy a follower can have. The game will randomly choose a value between the minimum above and this value.",
                new AcceptableValueRange<int>(1, 100),
                new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 0 }
            )
        );
        MaxRangeLifeExpectancy.SettingChanged += (_, _) => { EnforceRangeSanity(); };

        // ── Game Mechanics ──
        EasyFishing = _configInstance.Bind(GameMechanicsSection, "Easy Fishing", false, new ConfigDescription("Automatically reels in fish without button mashing. Just cast and wait.", null, new ConfigurationManagerAttributes { DispName = "Easy Fishing**", Order = 5 }));
        EasyFishing.SettingChanged += (_, _) => ShowRestartMessage();
        DisableGameOver = _configInstance.Bind(GameMechanicsSection, "No More Game-Over", false, new ConfigDescription("Disables the game over function when you have 0 followers for consecutive days.", null, new ConfigurationManagerAttributes { Order = 4 }));
        SinBossLimit = _configInstance.Bind(GameMechanicsSection, "Sin Boss Limit", 3, new ConfigDescription("Bishop kills required to unlock Sin. Default is 3.", new AcceptableValueRange<int>(1, 5), new ConfigurationManagerAttributes { Order = 1 }));

        // ── Game Speed ──
        EnableGameSpeedManipulation = _configInstance.Bind(GameSpeedSection, "Enable Game Speed Manipulation", false, new ConfigDescription("Use left/right arrows keys to increase/decrease game speed in 0.25 increments. Up arrow to reset to default.", null, new ConfigurationManagerAttributes { Order = 8 }));
        ShortenGameSpeedIncrements = _configInstance.Bind(GameSpeedSection, "Shorten Game Speed Increments", false, new ConfigDescription("When enabled, speed changes in large steps (0.25x, 1x, 2x, 3x, 4x, 5x). When disabled (default), speed changes in fine 0.25x increments (0.25x, 0.5x, 0.75x, 1x, 1.25x... up to 5x).", null, new ConfigurationManagerAttributes { Order = 7, DispName = "    └ Shorten Game Speed Increments" }));
        ResetTimeScaleKey = _configInstance.Bind(GameSpeedSection, "Reset Time Scale Key", new KeyboardShortcut(KeyCode.UpArrow), new ConfigDescription("The keyboard shortcut to reset the game speed to 1.", null, new ConfigurationManagerAttributes { Order = 6, DispName = "    └ Reset Time Scale Key" }));
        IncreaseGameSpeedKey = _configInstance.Bind(GameSpeedSection, "Increase Game Speed Key", new KeyboardShortcut(KeyCode.RightArrow), new ConfigDescription("The keyboard shortcut to increase the game speed.", null, new ConfigurationManagerAttributes { Order = 5, DispName = "    └ Increase Game Speed Key" }));
        DecreaseGameSpeedKey = _configInstance.Bind(GameSpeedSection, "Decrease Game Speed Key", new KeyboardShortcut(KeyCode.LeftArrow), new ConfigDescription("The keyboard shortcut to decrease the game speed.", null, new ConfigurationManagerAttributes { Order = 4, DispName = "    └ Decrease Game Speed Key" }));
        SlowDownTimeMultiplier = _configInstance.Bind(GameSpeedSection, "Slow Down Time Multiplier", 1.0f, new ConfigDescription("The multiplier to use for slow down time. For example, the default value of 2 is making the day twice as long.", new AcceptableValueRange<float>(-10f, 10f), new ConfigurationManagerAttributes { Order = 2 }));
        SlowDownTimeMultiplier.SettingChanged += (_, _) => { SlowDownTimeMultiplier.Value = Mathf.Round(SlowDownTimeMultiplier.Value * 4) / 4; };

        // ── Golden Fleece ──
        ReverseGoldenFleeceDamageChange = _configInstance.Bind(GoldenFleeceSection, "Reverse Golden Fleece Change", false, new ConfigDescription("Reverts the default damage increase to 10% instead of 5%.", null, new ConfigurationManagerAttributes { Order = 4 }));
        FleeceDamageMulti = _configInstance.Bind(GoldenFleeceSection, "Fleece Damage Multiplier", 1.0f, new ConfigDescription("The custom damage multiplier to use. Based off the games default 5%.", new AcceptableValueRange<float>(-10f, 10f), new ConfigurationManagerAttributes { Order = 1 }));
        FleeceDamageMulti.SettingChanged += (_, _) => { FleeceDamageMulti.Value = Mathf.Round(FleeceDamageMulti.Value * 4) / 4; };

        // ── Knucklebones ──
        KnucklebonesSpeedMultiplier = _configInstance.Bind(KnucklebonesSection, "Animation Speed Multiplier", 1.0f, new ConfigDescription("Speed up Knucklebones animations. 1.0 = normal speed, 2.0 = twice as fast, etc.", new AcceptableValueRange<float>(1f, 10f), new ConfigurationManagerAttributes { Order = 1 }));
        KnucklebonesSpeedMultiplier.SettingChanged += (_, _) =>
        {
            KnucklebonesSpeedMultiplier.Value = Mathf.Round(KnucklebonesSpeedMultiplier.Value * 4) / 4;
            ConfigCache.MarkDirty(ConfigCache.Keys.KnucklebonesSpeedMultiplier);
        };

        // ── Loot ──
        AllLootMagnets = _configInstance.Bind(LootSection, "All Loot Magnets", false, new ConfigDescription("All loot is magnetized to you.", null, new ConfigurationManagerAttributes
        {
            Order = 2
        }));
        AllLootMagnets.SettingChanged += (_, _) => { UpdateAllMagnets(); };
        MagnetRangeMultiplier = _configInstance.Bind(LootSection, "Magnet Range Multiplier", 1.0f, new ConfigDescription("Apply a multiplier to the magnet range.", new AcceptableValueRange<float>(-10f, 10f), new ConfigurationManagerAttributes
        {
            Order = 1, DispName = "    └ Magnet Range Multiplier"
        }));
        MagnetRangeMultiplier.SettingChanged += (_, _) =>
        {
            {
                MagnetRangeMultiplier.Value = Mathf.Round(MagnetRangeMultiplier.Value * 4) / 4;
                UpdateCustomMagnet();
            }
        };

        // ── Mass Animal ──
        MassPetAnimals = _configInstance.Bind(MassAnimalSection, "Mass Pet Animals", false, new ConfigDescription("When petting a farm animal, all farm animals are petted at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Pet Animals**", Order = 6
        }));
        MassPetAnimals.SettingChanged += (_, _) => ShowRestartMessage();
        MassCleanAnimals = _configInstance.Bind(MassAnimalSection, "Mass Clean Animals", false, new ConfigDescription("When cleaning a stinky animal, all stinky animals are cleaned at once.", null, new ConfigurationManagerAttributes
        {
            Order = 5
        }));
        MassFeedAnimals = _configInstance.Bind(MassAnimalSection, "Mass Feed Animals", false, new ConfigDescription("When feeding an animal, all hungry animals are fed the same food at once (consumes one item per animal).", null, new ConfigurationManagerAttributes
        {
            Order = 4
        }));
        MassMilkAnimals = _configInstance.Bind(MassAnimalSection, "Mass Milk Animals", false, new ConfigDescription("When milking an animal, all animals ready for milking are milked at once.", null, new ConfigurationManagerAttributes
        {
            Order = 3
        }));
        MassShearAnimals = _configInstance.Bind(MassAnimalSection, "Mass Shear Animals", false, new ConfigDescription("When shearing an animal, all animals ready for shearing are sheared at once.", null, new ConfigurationManagerAttributes
        {
            Order = 2
        }));
        FillTroughToCapacity = _configInstance.Bind(MassAnimalSection, "Fill Trough to Capacity", false, new ConfigDescription("When adding food to a trough, fills it to capacity in one action instead of adding one item at a time.", null, new ConfigurationManagerAttributes
        {
            Order = 1
        }));
        MassFillTroughs = _configInstance.Bind(MassAnimalSection, "Mass Fill Troughs", false, new ConfigDescription("When filling a trough, all non-full troughs are filled with the same food.", null, new ConfigurationManagerAttributes
        {
            Order = 0, DispName = "    └ Mass Fill Troughs"
        }));
        // TODO: Re-enable after testing
        // MassNurture = _configInstance.Bind(MassAnimalSection, "Mass Nurture", false, new ConfigDescription("When nurturing children at one daycare, children at all other daycares are also nurtured.", null, new ConfigurationManagerAttributes
        // {
        //     Order = 0
        // }));

        // ── Mass Action Costs ──
        MassActionCostModeEntry = _configInstance.Bind(MassActionCostsSection, "Cost Mode", CultOfQoL.MassActionCostMode.PerObject, new ConfigDescription("How costs are calculated. Per Mass Action = flat fee regardless of count. Per Object = cost multiplied by number of objects affected.", null, new ConfigurationManagerAttributes
        {
            Order = 5
        }));
        ShowMassActionCostPreview = _configInstance.Bind(MassActionCostsSection, "Show Cost Preview", false, new ConfigDescription("Show the estimated cost in the interaction label when highlighting a mass action. Only visible when Cost Mode is set to Per Object.", null, new ConfigurationManagerAttributes
        {
            Order = 4
        }));
        MassActionGoldCost = _configInstance.Bind(MassActionCostsSection, "Gold Cost", 0f, new ConfigDescription("Gold deducted for a mass action. Set to 0 for free. If you can't afford the total, the mass action is skipped but the original single interaction still works.", new AcceptableValueRange<float>(0f, 50f), new ConfigurationManagerAttributes
        {
            Order = 3
        }));
        MassActionGoldCost.SettingChanged += (_, _) => MassActionGoldCost.Value = RoundToQuarter(MassActionGoldCost.Value);

        MassActionTimeCost = _configInstance.Bind(MassActionCostsSection, "Time Cost (Game Minutes)", 0f, new ConfigDescription("Game minutes that pass for a mass action. Set to 0 for no time cost. 240 minutes = 1 game phase.", new AcceptableValueRange<float>(0f, 120f), new ConfigurationManagerAttributes
        {
            Order = 2
        }));
        MassActionTimeCost.SettingChanged += (_, _) => MassActionTimeCost.Value = RoundToQuarter(MassActionTimeCost.Value);
        MassFaithReduction = _configInstance.Bind(MassActionCostsSection, "Faith Reduction (%)", 0, new ConfigDescription("Reduces faith gained per follower from mass Bless and Inspire. 0 = full faith, 50 = half faith, 100 = no faith. The original single interaction always gives full faith.", new AcceptableValueRange<int>(0, 100), new ConfigurationManagerAttributes
        {
            Order = 1
        }));

        // ── Mass Collect ──
        CollectAllGodTearsAtOnce = _configInstance.Bind(MassCollectSection, "Collect All God Tears At Once", false, new ConfigDescription("When collecting god tears from the shrine, collect all available at once instead of one per interaction.", null, new ConfigurationManagerAttributes
        {
            Order = 7
        }));
        MassCollectFromBeds = _configInstance.Bind(MassCollectSection, "Mass Collect From Beds", false, new ConfigDescription("When collecting souls from a bed, all beds are collected from at once. Also speeds up the per-soul drain to 2x.", null, new ConfigurationManagerAttributes
        {
            Order = 6
        }));
        MassCollectFromOuthouses = _configInstance.Bind(MassCollectSection, "Mass Collect From Outhouses", false, new ConfigDescription("When collecting resources from an outhouse, all outhouses are collected from at once.", null, new ConfigurationManagerAttributes
        {
            Order = 5
        }));
        MassCollectFromOfferingShrines = _configInstance.Bind(MassCollectSection, "Mass Collect From Offering Shrines", false, new ConfigDescription("When collecting resources from an offering shrine, all offering shrines are collected from at once.", null, new ConfigurationManagerAttributes
        {
            Order = 4
        }));
        MassCollectFromPassiveShrines = _configInstance.Bind(MassCollectSection, "Mass Collect From Passive Shrines", false, new ConfigDescription("When collecting resources from a passive shrine, all passive shrines are collected from at once.", null, new ConfigurationManagerAttributes
        {
            Order = 3
        }));
        MassCollectFromCompost = _configInstance.Bind(MassCollectSection, "Mass Collect From Compost", false, new ConfigDescription("When collecting resources from a compost, all composts are collected from at once.", null, new ConfigurationManagerAttributes
        {
            Order = 2
        }));
        MassCollectFromHarvestTotems = _configInstance.Bind(MassCollectSection, "Mass Collect From Harvest Totems", false, new ConfigDescription("When collecting resources from a harvest totem, all harvest totems are collected from at once.", null, new ConfigurationManagerAttributes
        {
            Order = 1
        }));
        MassCleanPoop = _configInstance.Bind(MassCollectSection, "Mass Clean Poop", false, new ConfigDescription("When cleaning a poop pile, all poop piles are cleaned at once.", null, new ConfigurationManagerAttributes
        {
            Order = 0
        }));
        MassCleanVomit = _configInstance.Bind(MassCollectSection, "Mass Clean Vomit", false, new ConfigDescription("When cleaning a vomit puddle, all vomit puddles are cleaned at once.", null, new ConfigurationManagerAttributes
        {
            Order = -1
        }));

        // ── Mass Farm ──
        MassPlantSeeds = _configInstance.Bind(MassFarmSection, "Mass Plant Seeds", false, new ConfigDescription("When planting a seed in a farm plot, all other empty farm plots are planted with the same seed.", null, new ConfigurationManagerAttributes
        {
            Order = 14
        }));
        MassFertilize = _configInstance.Bind(MassFarmSection, "Mass Fertilize", false, new ConfigDescription("When fertilizing a plot, all farm plots are fertilized at once.", null, new ConfigurationManagerAttributes
        {
            Order = 13
        }));
        MassWater = _configInstance.Bind(MassFarmSection, "Mass Water", false, new ConfigDescription("When watering a plot, all farm plots are watered at once.", null, new ConfigurationManagerAttributes
        {
            Order = 12
        }));
        FillToolshedToCapacity = _configInstance.Bind(MassFarmSection, "Fill Carpentry Station to Capacity", false, new ConfigDescription("When depositing materials into a Carpentry Station, fills it to capacity in one action instead of adding one item at a time.", null, new ConfigurationManagerAttributes
        {
            Order = 11
        }));
        MassFillToolsheds = _configInstance.Bind(MassFarmSection, "Mass Fill Carpentry Stations", false, new ConfigDescription("When filling a Carpentry Station, all non-full Carpentry Stations are filled with the same material.", null, new ConfigurationManagerAttributes
        {
            Order = 10, DispName = "    └ Mass Fill Carpentry Stations"
        }));
        FillMedicToCapacity = _configInstance.Bind(MassFarmSection, "Fill Medic Station to Capacity", false, new ConfigDescription("When depositing supplies into a Medic Station, fills it to capacity in one action instead of adding one item at a time.", null, new ConfigurationManagerAttributes
        {
            Order = 9
        }));
        MassFillMedicStations = _configInstance.Bind(MassFarmSection, "Mass Fill Medic Stations", false, new ConfigDescription("When filling a Medic Station, all non-full Medic Stations are filled with the same supply.", null, new ConfigurationManagerAttributes
        {
            Order = 8, DispName = "    └ Mass Fill Medic Stations"
        }));
        FillSeedSiloToCapacity = _configInstance.Bind(MassFarmSection, "Fill Seed Silo to Capacity", false, new ConfigDescription("When depositing seeds into a Seed Silo, fills it to capacity in one action instead of adding one item at a time.", null, new ConfigurationManagerAttributes
        {
            Order = 7
        }));
        MassFillSeedSilos = _configInstance.Bind(MassFarmSection, "Mass Fill Seed Silos", false, new ConfigDescription("When filling a Seed Silo, all non-full Seed Silos are filled with the same seed.", null, new ConfigurationManagerAttributes
        {
            Order = 6, DispName = "    └ Mass Fill Seed Silos"
        }));
        FillFertilizerSiloToCapacity = _configInstance.Bind(MassFarmSection, "Fill Fertiliser Silo to Capacity", false, new ConfigDescription("When depositing fertiliser into a Fertiliser Silo, fills it to capacity in one action instead of adding one item at a time.", null, new ConfigurationManagerAttributes
        {
            Order = 5
        }));
        MassFillFertilizerSilos = _configInstance.Bind(MassFarmSection, "Mass Fill Fertiliser Silos", false, new ConfigDescription("When filling a Fertiliser Silo, all non-full Fertiliser Silos are filled with the same fertiliser.", null, new ConfigurationManagerAttributes
        {
            Order = 4, DispName = "    └ Mass Fill Fertiliser Silos"
        }));
        MassOpenScarecrows = _configInstance.Bind(MassFarmSection, "Mass Open Scarecrows", false, new ConfigDescription("When opening a scarecrow trap, all scarecrow traps with caught birds are opened at once.", null, new ConfigurationManagerAttributes
        {
            Order = 3
        }));
        MassWolfTraps = _configInstance.Bind(MassFarmSection, "Mass Wolf Traps", MassWolfTrapMode.Disabled, new ConfigDescription("Fill Only: Fill all empty traps with the same bait. Collect Only: Collect from all traps with caught wolves. Both: Do both actions.", null, new ConfigurationManagerAttributes
        {
            Order = 2
        }));

        // ── Farm ──
        RotFertilizerDecay = _configInstance.Bind(FarmSection, "Rot Fertilizer Decay", false, new ConfigDescription("When enabled, rot fertilizer warming on farm plots expires after a set number of days instead of lasting forever.", null, new ConfigurationManagerAttributes
        {
            Order = 2
        }));
        RotFertilizerDuration = _configInstance.Bind(FarmSection, "Rot Fertilizer Duration (Days)", 5, new ConfigDescription("Number of days before rot fertilizer warming expires. Crops on expired plots will freeze during winter unless near a Thawing Harvest Totem.", new AcceptableValueRange<int>(1, 30), new ConfigurationManagerAttributes
        {
            Order = 1
        }));

        // ── Mass Follower ──
        MassNotificationThreshold = _configInstance.Bind(MassFollowerSection, "Mass Notification Threshold", 3, new ConfigDescription("When a mass action affects more than this many followers, show a single summary notification instead of one per follower. Set to 0 to always show a summary.", new AcceptableValueRange<int>(0, 50), new ConfigurationManagerAttributes
        {
            Order = 14
        }));
        MassBribe = _configInstance.Bind(MassFollowerSection, "Mass Bribe", false, new ConfigDescription("When bribing a follower, all followers are bribed at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Bribe**", Order = 13
        }));
        MassBribe.SettingChanged += (_, _) => ShowRestartMessage();
        MassBless = _configInstance.Bind(MassFollowerSection, "Mass Bless", false, new ConfigDescription("When blessing a follower, all followers are blessed at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Bless**", Order = 12
        }));
        MassBless.SettingChanged += (_, _) => ShowRestartMessage();
        MassExtort = _configInstance.Bind(MassFollowerSection, "Mass Extort", false, new ConfigDescription("When extorting a follower, all followers are extorted at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Extort**", Order = 11
        }));
        MassExtort.SettingChanged += (_, _) => ShowRestartMessage();
        MassIntimidate = _configInstance.Bind(MassFollowerSection, "Mass Intimidate", false, new ConfigDescription("When intimidating a follower, all followers are intimidated at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Intimidate**", Order = 10
        }));
        MassIntimidate.SettingChanged += (_, _) => ShowRestartMessage();
        MassIntimidateScareAll = _configInstance.Bind(MassFollowerSection, "Mass Intimidate Scare All", false, new ConfigDescription("When Mass Intimidate is enabled, apply the 5% Scared trait roll to all intimidated followers instead of only the original.", null, new ConfigurationManagerAttributes
        {
            Order = 9, DispName = "    └ Mass Intimidate Scare All"
        }));
        MassIntimidateScareAll.SettingChanged += (_, _) => ShowRestartMessage();
        MassInspire = _configInstance.Bind(MassFollowerSection, "Mass Inspire", false, new ConfigDescription("When inspiring a follower, all followers are inspired at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Inspire**", Order = 8
        }));
        MassInspire.SettingChanged += (_, _) => ShowRestartMessage();
        MassRomance = _configInstance.Bind(MassFollowerSection, "Mass Romance", false, new ConfigDescription("When romancing a follower, all followers are romanced at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Romance**", Order = 7
        }));
        MassRomance.SettingChanged += (_, _) => ShowRestartMessage();
        MassBully = _configInstance.Bind(MassFollowerSection, "Mass Bully", false, new ConfigDescription("When bullying a follower, all followers are bullied at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Bully**", Order = 6
        }));
        MassBully.SettingChanged += (_, _) => ShowRestartMessage();
        MassReassure = _configInstance.Bind(MassFollowerSection, "Mass Reassure", false, new ConfigDescription("When reassuring a follower, all followers are reassured at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Reassure**", Order = 5
        }));
        MassReassure.SettingChanged += (_, _) => ShowRestartMessage();
        MassReeducate = _configInstance.Bind(MassFollowerSection, "Mass Reeducate", false, new ConfigDescription("When reeducating a follower, all followers are reeducated at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Reeducate**", Order = 4
        }));
        MassReeducate.SettingChanged += (_, _) => ShowRestartMessage();
        MassLevelUp = _configInstance.Bind(MassFollowerSection, "Mass Level Up", false, new ConfigDescription("When leveling up a follower, all eligible followers are leveled up at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Level Up**", Order = 3
        }));
        MassLevelUp.SettingChanged += (_, _) => ShowRestartMessage();
        MassLevelUpInstantSouls = _configInstance.Bind(MassFollowerSection, "Mass Level Up Instant Souls", true, new ConfigDescription("Instantly collect souls during mass level up instead of having them fly to you.", null, new ConfigurationManagerAttributes
        {
            Order = 2, DispName = "    └ Mass Level Up Instant Souls"
        }));
        MassPetFollower = _configInstance.Bind(MassFollowerSection, "Mass Pet Follower", false, new ConfigDescription("When petting a follower, all eligible followers are petted at once. Which followers qualify depends on the Mass Pet All Followers setting below.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Pet Follower**", Order = 1
        }));
        MassPetFollower.SettingChanged += (_, _) => ShowRestartMessage();
        MassPetAllFollowers = _configInstance.Bind(MassFollowerSection, "Mass Pet All Followers", false, new ConfigDescription("When enabled, mass pet applies to all followers regardless of the Pettable trait. When disabled, only followers with the Pettable trait or Dog/Poppy skin are petted.", null, new ConfigurationManagerAttributes
        {
            Order = 0, DispName = "    └ Mass Pet All Followers"
        }));
        MassSinExtract = _configInstance.Bind(MassFollowerSection, "Mass Sin Extract", false, new ConfigDescription("When extracting sin from a follower, all eligible followers have their sin extracted at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Sin Extract**", Order = -1
        }));
        MassSinExtract.SettingChanged += (_, _) => ShowRestartMessage();

        // ── Menu Cleanup ──
        RemoveMenuClutter = _configInstance.Bind(MenuCleanupSection, "Remove Extra Menu Buttons", false, new ConfigDescription("Removes credits/road-map/discord buttons from the menus.", null, new ConfigurationManagerAttributes { Order = 6 }));
        RemoveTwitchButton = _configInstance.Bind(MenuCleanupSection, "Remove Twitch Buttons", false, new ConfigDescription("Removes twitch buttons from the menus.", null, new ConfigurationManagerAttributes { Order = 5 }));
        DisableAds = _configInstance.Bind(MenuCleanupSection, "Hide Ads", false, new ConfigDescription("Hides the promotional ads from the main menu.", null, new ConfigurationManagerAttributes { Order = 4 }));
        DisableAds.SettingChanged += (_, _) => InvalidateConfigCache();
        RemoveHelpButtonInPauseMenu = _configInstance.Bind(MenuCleanupSection, "Remove Help Button In Pause Menu", false, new ConfigDescription("Removes the help button in the pause menu.", null, new ConfigurationManagerAttributes { Order = 3 }));
        RemoveTwitchButtonInPauseMenu = _configInstance.Bind(MenuCleanupSection, "Remove Twitch Button In Pause Menu", false, new ConfigDescription("Removes the twitch button in the pause menu.", null, new ConfigurationManagerAttributes { Order = 2 }));
        RemovePhotoModeButtonInPauseMenu = _configInstance.Bind(MenuCleanupSection, "Remove Photo Mode Button In Pause Menu", false, new ConfigDescription("Removes the photo mode button in the pause menu.", null, new ConfigurationManagerAttributes { Order = 1 }));
        MainMenuGlitch = _configInstance.Bind(MenuCleanupSection, "Main Menu Glitch", true, new ConfigDescription("Controls the sudden dark-mode switch effect.", null, new ConfigurationManagerAttributes { Order = 0 }));
        MainMenuGlitch.SettingChanged += (_, _) => { UpdateMenuGlitch(); };

        // ── Mines ──
        LumberAndMiningStationsDontAge = _configInstance.Bind(MinesSection, "Infinite Lumber & Mining Stations", false, new ConfigDescription("Lumber and mining stations should never run out and collapse. Takes 1st priority.", null, new ConfigurationManagerAttributes { Order = 3 }));
        LumberAndMiningStationsDontAge.SettingChanged += (_, _) => ConfigCache.MarkDirty(ConfigCache.Keys.LumberAndMiningStationsDontAge);
        LumberAndMiningStationsAgeMultiplier = _configInstance.Bind(MinesSection, "Lumber & Mining Stations Age Multiplier", 1.0f, new ConfigDescription("How much slower (or faster) the lumber and mining stations age. Default is 1.0f.", new AcceptableValueRange<float>(-10f, 10f), new ConfigurationManagerAttributes { Order = 2 }));
        LumberAndMiningStationsAgeMultiplier.SettingChanged += (_, _) => ConfigCache.MarkDirty(ConfigCache.Keys.LumberAndMiningStationsAgeMultiplier);
        LumberAndMiningStationsAgeMultiplier.SettingChanged += (_, _) => { LumberAndMiningStationsAgeMultiplier.Value = Mathf.Round(LumberAndMiningStationsAgeMultiplier.Value * 4) / 4; };

        // ── Notifications ──
        DisableAllNotifications = _configInstance.Bind(NotificationsSection, "Hide All Notifications", false, new ConfigDescription("Hides all in-game notifications. This also prevents custom notifications below from appearing.", null, new ConfigurationManagerAttributes
        {
            Order = 10
        }));
        AllowCriticalNotifications = _configInstance.Bind(NotificationsSection, "Allow Critical Notifications", true, new ConfigDescription("When 'Disable All Notifications' is enabled, still show critical notifications (deaths, weapon destruction, dissenters).", null, new ConfigurationManagerAttributes
        {
            Order = 9, DispName = "    └ Allow Critical Notifications"
        }));
        SuppressNotificationsOnLoad = _configInstance.Bind(NotificationsSection, "Suppress Notifications On Load", false, new ConfigDescription("Suppress individual notifications for a few seconds after loading a save, preventing the flood of status updates. Dynamic status indicators (starving, sick, etc.) are not affected.", null, new ConfigurationManagerAttributes
        {
            Order = 8
        }));
        NotifyOfScarecrowTraps = _configInstance.Bind(NotificationsSection, "Notify of Scarecrow Traps", false, new ConfigDescription("Display a notification when the farm scarecrows have caught a trap!", null, new ConfigurationManagerAttributes
        {
            Order = 5
        }));
        NotifyOfNoFuel = _configInstance.Bind(NotificationsSection, "Notify of No Fuel", false, new ConfigDescription("Display a notification when a structure has run out of fuel.", null, new ConfigurationManagerAttributes
        {
            Order = 4
        }));
        NotifyOfBedCollapse = _configInstance.Bind(NotificationsSection, "Notify of Bed Collapse", false, new ConfigDescription("Display a notification when a bed has collapsed.", null, new ConfigurationManagerAttributes
        {
            Order = 3
        }));
        ShowPhaseNotifications = _configInstance.Bind(NotificationsSection, "Phase Notifications", false, new ConfigDescription("Show a notification when the time of day changes.", null, new ConfigurationManagerAttributes
        {
            Order = 2
        }));
        ShowPhaseNotifications.SettingChanged += (_, _) => ConfigCache.MarkDirty(ConfigCache.Keys.ShowPhaseNotifications);
        ShowWeatherChangeNotifications = _configInstance.Bind(NotificationsSection, "Weather Change Notifications", false, new ConfigDescription("Show a notification when the weather changes.", null, new ConfigurationManagerAttributes
        {
            Order = 1
        }));
        ShowWeatherChangeNotifications.SettingChanged += (_, _) => ConfigCache.MarkDirty(ConfigCache.Keys.ShowWeatherChangeNotifications);

        // ── Player ──
        RunSpeedMulti = _configInstance.Bind(PlayerSection, "Run Speed Multiplier", 1.0f, new ConfigDescription("How much faster the player runs.", new AcceptableValueRange<float>(-10f, 10f), new ConfigurationManagerAttributes { Order = 7 }));
        RunSpeedMulti.SettingChanged += (_, _) => { RunSpeedMulti.Value = Mathf.Round(RunSpeedMulti.Value * 4) / 4; };
        DisableRunSpeedInDungeons = _configInstance.Bind(PlayerSection, "Exclude Dungeons", true, new ConfigDescription("When enabled, the run speed multiplier won't apply in dungeons.", null, new ConfigurationManagerAttributes { Order = 6, DispName = "    └ Exclude Dungeons" }));
        DisableRunSpeedInCombat = _configInstance.Bind(PlayerSection, "Exclude Combat", true, new ConfigDescription("When enabled, the run speed multiplier won't apply during combat.", null, new ConfigurationManagerAttributes { Order = 5, DispName = "    └ Exclude Combat" }));
        BaseDamageMultiplier = _configInstance.Bind(PlayerSection, "Base Damage Multiplier", 1.0f, new ConfigDescription("The base damage multiplier to use.", new AcceptableValueRange<float>(-10, 10), new ConfigurationManagerAttributes { Order = 4 }));
        BaseDamageMultiplier.SettingChanged += (_, _) => { BaseDamageMultiplier.Value = Mathf.Round(BaseDamageMultiplier.Value * 4) / 4; };
        DodgeSpeedMulti = _configInstance.Bind(PlayerSection, "Dodge Speed Multiplier", 1.0f, new ConfigDescription("How much faster the player dodges.", new AcceptableValueRange<float>(-10f, 10f), new ConfigurationManagerAttributes { Order = 3 }));
        DodgeSpeedMulti.SettingChanged += (_, _) => { DodgeSpeedMulti.Value = Mathf.Round(DodgeSpeedMulti.Value * 4) / 4; };
        LungeSpeedMulti = _configInstance.Bind(PlayerSection, "Lunge Speed Multiplier", 1.0f, new ConfigDescription("How much faster the player lunges.", new AcceptableValueRange<float>(-10f, 10f), new ConfigurationManagerAttributes { Order = 1 }));
        LungeSpeedMulti.SettingChanged += (_, _) => { LungeSpeedMulti.Value = Mathf.Round(LungeSpeedMulti.Value * 4) / 4; };

        // ── Post Processing ──
        VignetteEffect = _configInstance.Bind(PostProcessingSection, "Vignette UI Overlay", true, new ConfigDescription("Enable/disable the vignette UI overlay images (separate from the post-processing vignette effect).", null, new ConfigurationManagerAttributes
        {
            Order = 90
        }));
        VignetteEffect.SettingChanged += (_, _) => { PostProcessing.ToggleVignette(); };

        // ── Rituals ──
        FastRitualSermons = _configInstance.Bind(RitualSection, "Fast Rituals & Sermons", false, new ConfigDescription("Speeds up rituals and sermons.", null, new ConfigurationManagerAttributes { Order = 5 }));
        FastRitualSermons.SettingChanged += (_, _) =>
        {
            if (FastRitualSermons.Value) return;
            RitualSermonSpeed.RitualRunning = false;
            GameManager.SetTimeScale(1);
        };
        ReverseEnrichmentNerf = _configInstance.Bind(RitualSection, "Reverse Enrichment Nerf", false, new ConfigDescription("Reverts the nerf to the Ritual of Enrichment. Coins scale with follower level (level * 20 per follower).", null, new ConfigurationManagerAttributes
        {
            Order = 4
        }));
        RitualCooldownTime = _configInstance.Bind(
            RitualSection,
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
                new ConfigurationManagerAttributes { Order = 3 }
            )
        );
        RitualCooldownTime.SettingChanged += (_, _) => { RitualCooldownTime.Value = Mathf.Round(RitualCooldownTime.Value * 4) / 4; };
        RitualCostMultiplier = _configInstance.Bind(
            RitualSection,
            "Ritual Cost Multiplier",
            1.0f,
            new ConfigDescription(
                "Multiplier for ritual material costs. Values above 1 increase costs, below 1 decrease costs. Stacks with in-game discounts. Does not affect follower requirements or doctrine unlocks.",
                new AcceptableValueRange<float>(0.5f, 4.0f),
                new ConfigurationManagerAttributes { Order = 2 }
            )
        );
        RitualCostMultiplier.SettingChanged += (_, _) => { RitualCostMultiplier.Value = Mathf.Round(RitualCostMultiplier.Value * 4) / 4; };

        // ── Saves ──
        SaveOnQuitToDesktop = _configInstance.Bind(SavesSection, "Save On Quit To Desktop", false, new ConfigDescription("Modify the confirmation dialog to save the game when you quit to desktop.", null, new ConfigurationManagerAttributes { Order = 8 }));
        SaveOnQuitToMenu = _configInstance.Bind(SavesSection, "Save On Quit To Menu", false, new ConfigDescription("Modify the confirmation dialog to save the game when you quit to menu.", null, new ConfigurationManagerAttributes { Order = 7 }));
        HideNewGameButtons = _configInstance.Bind(SavesSection, "Hide New Game Button (s)", false, new ConfigDescription("Hides the new game button if you have at least one save game.", null, new ConfigurationManagerAttributes { Order = 6 }));
        EnableQuickSaveShortcut = _configInstance.Bind(SavesSection, "Enable Quick Save Shortcut", false, new ConfigDescription("Enable/disable the quick save keyboard shortcut.", null, new ConfigurationManagerAttributes { Order = 5 }));
        EnableQuickSaveShortcut.SettingChanged += (_, _) => InvalidateConfigCache();
        SaveKeyboardShortcut = _configInstance.Bind(SavesSection, "Save Keyboard Shortcut", new KeyboardShortcut(KeyCode.F5), new ConfigDescription("The keyboard shortcut to save the game.", null, new ConfigurationManagerAttributes { Order = 4, DispName = "    └ Save Keyboard Shortcut" }));
        SaveKeyboardShortcut.SettingChanged += (_, _) => InvalidateConfigCache();
        DirectLoadSave = _configInstance.Bind(SavesSection, "Direct Load Save", false, new ConfigDescription("Directly load the specified save game instead of showing the save menu.", null, new ConfigurationManagerAttributes { Order = 3 }));
        DirectLoadSave.SettingChanged += (_, _) => InvalidateConfigCache();
        DirectLoadSkipKey = _configInstance.Bind(SavesSection, "Direct Load Skip Key", new KeyboardShortcut(KeyCode.LeftShift), new ConfigDescription("The keyboard shortcut to skip the auto-load when loading the game.", null, new ConfigurationManagerAttributes { Order = 2, DispName = "    └ Direct Load Skip Key" }));
        DirectLoadSkipKey.SettingChanged += (_, _) => InvalidateConfigCache();
        SaveSlotToLoad = _configInstance.Bind(SavesSection, "Save Slot To Load", 1, new ConfigDescription("The save slot to load.", new AcceptableValueList<int>(1, 2, 3), new ConfigurationManagerAttributes { Order = 1, DispName = "    └ Save Slot To Load" }));
        SaveSlotToLoad.SettingChanged += (_, _) =>
        {
            if (!SaveAndLoad.SaveExist(SaveSlotToLoad.Value))
            {
                WriteLog("The slot you have select doesn't contain a save game.");
                return;
            }

            WriteLog($"Save slot to load changed to {SaveSlotToLoad.Value}");
        };

        // ── Sound ──
        ResourceChestDepositSounds = _configInstance.Bind(SoundSection, "Resource Chest Deposit Sounds", true, new ConfigDescription("Play sounds when followers deposit resources into chests.", null, new ConfigurationManagerAttributes { Order = 2 }));
        ResourceChestDepositSounds.SettingChanged += (_, _) => ConfigCache.MarkDirty(ConfigCache.Keys.ResourceChestDepositSounds);
        ResourceChestCollectSounds = _configInstance.Bind(SoundSection, "Resource Chest Collect Sounds", true, new ConfigDescription("Play sounds when collecting resources from chests.", null, new ConfigurationManagerAttributes { Order = 1 }));
        ResourceChestCollectSounds.SettingChanged += (_, _) => ConfigCache.MarkDirty(ConfigCache.Keys.ResourceChestCollectSounds);

        // ── Structures ──
        // Shrine Fuel & Warmth
        EnableRotburnAsShrineFuel = _configInstance.Bind(StructureSection, "Rotburn as Shrine Fuel", false, new ConfigDescription("Allow Rotburn (MAGMA_STONE) to be used as fuel for the Shrine brazier.", null, new ConfigurationManagerAttributes { Order = 30 }));
        RotburnShrineFuelWeight = _configInstance.Bind(StructureSection, "Rotburn Fuel Weight", 13, new ConfigDescription("Fuel value when adding Rotburn to shrine. Default matches LOG (13). Vanilla MAGMA_STONE is 14700.", new AcceptableValueRange<int>(1, 100), new ConfigurationManagerAttributes { Order = 29, ShowRangeAsPercent = false, DispName = "    └ Rotburn Fuel Weight" }));
        EnableShrineWarmth = _configInstance.Bind(StructureSection, "Shrine Provides Warmth", true, new ConfigDescription("When the shrine brazier is fully fueled, it provides warmth during winter (20% contribution).", null, new ConfigurationManagerAttributes { Order = 28 }));

        // Furnace Heater Scaling
        FurnaceHeaterScaling = _configInstance.Bind(StructureSection, "Furnace Heater Scaling", false, new ConfigDescription("Each proximity heater increases the furnace's fuel drain during winter. More heaters = faster fuel consumption.", null, new ConfigurationManagerAttributes { Order = 34 }));
        FurnaceHeaterFuelCost = _configInstance.Bind(StructureSection, "Furnace Heater Fuel Cost", 500, new ConfigDescription("Fuel units drained per heater per game phase during winter. 1 Rotburn = 14,700 fuel. At default 500, 5 heaters drain 2,500/phase (~10,000/day).", new AcceptableValueRange<int>(100, 5000), new ConfigurationManagerAttributes { Order = 33, ShowRangeAsPercent = false, DispName = "    └ Heater Fuel Cost" }));

        RefineryPoopToRotPoop = _configInstance.Bind(StructureSection, "Refinery: Poop to Rot Fertilizer", false, new ConfigDescription("Adds a refinery recipe to convert Poop + Rotgrit into Rot Fertilizer (Poop Rotstone).", null, new ConfigurationManagerAttributes { Order = 28 }));
        AdjustRefineryRequirements = _configInstance.Bind(StructureSection, "Adjust Refinery Requirements", false, new ConfigDescription("Where possible, halves the materials needed to convert items in the refinery. Rounds up.", null, new ConfigurationManagerAttributes { Order = 27 }));
        AdjustRefineryRequirements.SettingChanged += (_, _) => ConfigCache.MarkDirty(ConfigCache.Keys.AdjustRefineryRequirements);
        RefineryMassFill = _configInstance.Bind(StructureSection, "Refinery Mass Fill", false, new ConfigDescription("When adding an item to the refinery queue, automatically fill all available slots with that item.", null, new ConfigurationManagerAttributes { Order = 26 }));
        CookingFireMassFill = _configInstance.Bind(StructureSection, "Cooking Fire Mass Fill", false, new ConfigDescription("When adding a meal to the cooking fire queue, automatically fill all available slots with that meal.", null, new ConfigurationManagerAttributes { Order = 25 }));
        KitchenMassFill = _configInstance.Bind(StructureSection, "Kitchen Mass Fill", false, new ConfigDescription("When adding a meal to the follower kitchen queue, automatically fill all available slots with that meal.", null, new ConfigurationManagerAttributes { Order = 24 }));
        PubMassFill = _configInstance.Bind(StructureSection, "Pub Mass Fill", false, new ConfigDescription("When adding a drink to the pub queue, automatically fill all available slots with that drink.", null, new ConfigurationManagerAttributes { Order = 23 }));
        AutoSelectBestMatingPair = _configInstance.Bind(StructureSection, "Auto-Select Best Mating Pair", false, new ConfigDescription("Automatically selects the two followers with the highest mating success chance when the Mating Tent is opened.", null, new ConfigurationManagerAttributes { Order = 20 }));
        AddExhaustedToHealingBay = _configInstance.Bind(StructureSection, "Add Exhausted To Healing Bay", false, new ConfigDescription("Allows you to select exhausted followers for rest and relaxation in the healing bays.", null, new ConfigurationManagerAttributes { Order = 19 }));
        HideHealthyFromHealingBay = _configInstance.Bind(StructureSection, "Hide Healthy From Healing Bay", false, new ConfigDescription("Hides followers that don't need healing from the healing bay selection menu.", null, new ConfigurationManagerAttributes { Order = 18 }));
        OnlyShowDissenters = _configInstance.Bind(StructureSection, "Only Show Dissenters In Prison Menu", false, new ConfigDescription("Only show dissenting followers when interacting with the prison.", null, new ConfigurationManagerAttributes { Order = 17 }));
        ExcludeGrassFromSeedDeposit = _configInstance.Bind(StructureSection, "Exclude Grass From Seed Deposit", false, new ConfigDescription("When using 'Deposit All Seeds' on a seed silo, grass will not be deposited.", null, new ConfigurationManagerAttributes { Order = 16 }));
        TurnOffSpeakersAtNight = _configInstance.Bind(StructureSection, "Turn Off Speakers At Night", false, new ConfigDescription("Turns the speakers off, and stops fuel consumption at night time.", null, new ConfigurationManagerAttributes { Order = 14 }));
        TurnOffSpeakersAtNight.SettingChanged += (_, _) => ConfigCache.MarkDirty(ConfigCache.Keys.TurnOffSpeakersAtNight);
        DisablePropagandaSpeakerAudio = _configInstance.Bind(StructureSection, "Mute Propaganda Speakers", false, new ConfigDescription("Silences the audio from propaganda speakers.", null, new ConfigurationManagerAttributes { Order = 13 }));
        DisablePropagandaSpeakerAudio.SettingChanged += (_, _) => ConfigCache.MarkDirty(ConfigCache.Keys.DisablePropagandaSpeakerAudio);
        var speakerRange = Mathf.RoundToInt(Structures_PropagandaSpeaker.EFFECTIVE_DISTANCE);
        PropagandaSpeakerRange = _configInstance.Bind(StructureSection, "Propaganda Speaker Range", speakerRange, new ConfigDescription($"The range of the propaganda speaker. Default is {speakerRange}.", new AcceptableValueRange<int>(1, 20), new ConfigurationManagerAttributes { Order = 12 }));
        PropagandaSpeakerRange.SettingChanged += (_, _) => { Structures_PropagandaSpeaker.EFFECTIVE_DISTANCE = Mathf.RoundToInt(PropagandaSpeakerRange.Value); };
        var harvestRange = Mathf.RoundToInt(HarvestTotem.EFFECTIVE_DISTANCE);
        HarvestTotemRange = _configInstance.Bind(StructureSection, "Harvest Totem Range", harvestRange, new ConfigDescription($"The range of the harvest totem. Default is {harvestRange}.", new AcceptableValueRange<int>(3, 14), new ConfigurationManagerAttributes { Order = 11 }));
        HarvestTotemRange.SettingChanged += (_, _) => { HarvestTotem.EFFECTIVE_DISTANCE = Mathf.RoundToInt(HarvestTotemRange.Value); };
        FarmStationRange = _configInstance.Bind(StructureSection, "Farm Station Range", 6, new ConfigDescription("The range of the farm station. Default is 6.", new AcceptableValueRange<int>(1, 20), new ConfigurationManagerAttributes { Order = 10 }));
        FarmPlotSignRange = _configInstance.Bind(StructureSection, "Farm Plot Sign Range", 5, new ConfigDescription("The range of the farm plot sign. Default is 5.", new AcceptableValueRange<int>(1, 20), new ConfigurationManagerAttributes { Order = 9 }));
        var lightningRodRangeLvl1 = Mathf.RoundToInt(Interaction_LightningRod.EFFECTIVE_DISTANCE_LVL1);
        var lightningRodRangeLvl2 = Mathf.RoundToInt(Interaction_LightningRod.EFFECTIVE_DISTANCE_LVL2);
        LightningRodRangeLvl1 = _configInstance.Bind(StructureSection, "Lightning Rod Range (Basic)", lightningRodRangeLvl1, new ConfigDescription($"The range of the basic lightning rod. Default is {lightningRodRangeLvl1}.", new AcceptableValueRange<int>(10, 100), new ConfigurationManagerAttributes { Order = 8 }));
        LightningRodRangeLvl1.SettingChanged += (_, _) => { Interaction_LightningRod.EFFECTIVE_DISTANCE_LVL1 = LightningRodRangeLvl1.Value; };
        LightningRodRangeLvl2 = _configInstance.Bind(StructureSection, "Lightning Rod Range (Upgraded)", lightningRodRangeLvl2, new ConfigDescription($"The range of the upgraded lightning rod. Default is {lightningRodRangeLvl2}.", new AcceptableValueRange<int>(10, 100), new ConfigurationManagerAttributes { Order = 7 }));
        LightningRodRangeLvl2.SettingChanged += (_, _) => { Interaction_LightningRod.EFFECTIVE_DISTANCE_LVL2 = LightningRodRangeLvl2.Value; };
        CookedMeatMealsContainBone = _configInstance.Bind(StructureSection, "Cooked Meat Meals Contain Bone", false, new ConfigDescription("Meat + fish meals will spawn 1 - 3 bones when cooked.", null, new ConfigurationManagerAttributes { Order = 6 }));
        CookedMeatMealsContainBone.SettingChanged += (_, _) => ConfigCache.MarkDirty(ConfigCache.Keys.CookedMeatMealsContainBone);
        AddSpiderWebsToOfferings = _configInstance.Bind(StructureSection, "Add Spider Webs To Offerings", false, new ConfigDescription("Adds Spider Webs to the Offering Shrines default offerings.", null, new ConfigurationManagerAttributes { Order = 5 }));
        AddSpiderWebsToOfferings.SettingChanged += (_, _) => ConfigCache.MarkDirty(ConfigCache.Keys.AddSpiderWebsToOfferings);
        AddCrystalShardsToOfferings = _configInstance.Bind(StructureSection, "Add Crystals To Offerings", false, new ConfigDescription("Adds Crystal Shards to the Offering Shrines rare offerings.", null, new ConfigurationManagerAttributes { Order = 4 }));
        AddCrystalShardsToOfferings.SettingChanged += (_, _) => ConfigCache.MarkDirty(ConfigCache.Keys.AddCrystalShardsToOfferings);
        ProduceSpiderWebsFromLumber = _configInstance.Bind(StructureSection, "Lumber Stations Produce Spider Webs", false, new ConfigDescription("Lumber stations will produce spider webs from logs collected.", null, new ConfigurationManagerAttributes { Order = 3 }));
        ProduceSpiderWebsFromLumber.SettingChanged += (_, _) => ConfigCache.MarkDirty(ConfigCache.Keys.ProduceSpiderWebsFromLumber);
        SpiderWebsPerLogs = _configInstance.Bind(StructureSection, "Spider Webs Per Logs", 5, new ConfigDescription("Number of logs needed to produce 1 spider web.", new AcceptableValueRange<int>(1, 20), new ConfigurationManagerAttributes { Order = 2, DispName = "    └ Spider Webs Per Logs" }));
        SpiderWebsPerLogs.SettingChanged += (_, _) => ConfigCache.MarkDirty(ConfigCache.Keys.SpiderWebsPerLogs);
        ProduceCrystalShardsFromStone = _configInstance.Bind(StructureSection, "Mining Stations Produce Crystal Shards", false, new ConfigDescription("Mining stations will produce crystal shards from stone collected.", null, new ConfigurationManagerAttributes { Order = 1 }));
        ProduceCrystalShardsFromStone.SettingChanged += (_, _) => ConfigCache.MarkDirty(ConfigCache.Keys.ProduceCrystalShardsFromStone);
        CrystalShardsPerStone = _configInstance.Bind(StructureSection, "Crystal Shards Per Stone", 5, new ConfigDescription("Number of stone needed to produce 1 crystal shard.", new AcceptableValueRange<int>(1, 20), new ConfigurationManagerAttributes { Order = 0, DispName = "    └ Crystal Shards Per Stone" }));
        CrystalShardsPerStone.SettingChanged += (_, _) => ConfigCache.MarkDirty(ConfigCache.Keys.CrystalShardsPerStone);

        // ── Tarot ──
        ThriceMultiplyTarotCardLuck = _configInstance.Bind(TarotSection, "Increase Tarot Luck", false, new ConfigDescription("Multiply your luck for drawing rarer tarot cards by the value below.", null, new ConfigurationManagerAttributes { Order = 3 }));
        TarotLuckMultiplier = _configInstance.Bind(TarotSection, "Tarot Luck Multiplier", 2.0f, new ConfigDescription("Luck multiplier for rare tarot card draws. Higher = more rare cards.", new AcceptableValueRange<float>(1.0f, 5.0f), new ConfigurationManagerAttributes { Order = 2, DispName = "    └ Tarot Luck Multiplier" }));
        TarotLuckMultiplier.SettingChanged += (_, _) => { TarotLuckMultiplier.Value = Mathf.Round(TarotLuckMultiplier.Value * 2) / 2; }; // 0.5 increments
        RareTarotCardsOnly = _configInstance.Bind(TarotSection, "Rare Tarot Cards Only", false, new ConfigDescription("Only draw rare tarot cards.", null, new ConfigurationManagerAttributes { Order = 1 }));

        // ── Weather ──
        WeatherChangeTrigger = _configInstance.Bind(WeatherSection, "Weather Change Trigger", Patches.Systems.WeatherChangeTrigger.Disabled, new ConfigDescription("When should weather randomly change? Disabled = vanilla (once per day). Location Change = dungeons/fast travel. Phase Change = every phase. Both = location AND phase change.", null, new ConfigurationManagerAttributes { Order = 10 }));
        UnlockAllWeatherTypes = _configInstance.Bind(WeatherSection, "Unlock All Weather Types", false, new ConfigDescription("Allow all weather types (including snow) to appear regardless of season or game progression.", null, new ConfigurationManagerAttributes { Order = 9 }));
        LightSnowColor = _configInstance.Bind(WeatherSection, "Light Snow Color", new Color(0.016f, 0f, 1f, 0.15f), new ConfigDescription("Control the colour of the screen when there is light snow.", null, new ConfigurationManagerAttributes { Order = 7 }));
        LightWindColor = _configInstance.Bind(WeatherSection, "Light Wind Color", new Color(0.016f, 0f, 1f, 0.15f), new ConfigDescription("Control the colour of the screen when there is light wind.", null, new ConfigurationManagerAttributes { Order = 6 }));
        LightRainColor = _configInstance.Bind(WeatherSection, "Light Rain Color", new Color(0.016f, 0f, 1f, 0.15f), new ConfigDescription("Control the colour of the screen when there is light rain.", null, new ConfigurationManagerAttributes { Order = 5 }));
        MediumRainColor = _configInstance.Bind(WeatherSection, "Medium Rain Color", new Color(0.016f, 0f, 1f, 0.15f), new ConfigDescription("Control the colour of the screen when there is medium rain.", null, new ConfigurationManagerAttributes { Order = 4 }));
        HeavyRainColor = _configInstance.Bind(WeatherSection, "Heavy Rain Color", new Color(0.016f, 0f, 1f, 0.45f), new ConfigDescription("Control the colour of the screen when there is heavy rain.", null, new ConfigurationManagerAttributes { Order = 3 }));
        WeatherDropDown = _configInstance.Bind(WeatherSection, "Weather Dropdown", Weather.WeatherCombo.HeavyRain, new ConfigDescription("Select the type of weather you want to test to see the effect your chosen colour has.", null, new ConfigurationManagerAttributes { Order = 2 }));
        _configInstance.Bind(WeatherSection, "Test Weather", true, new ConfigDescription("Click to apply the weather type selected above and preview your custom color settings in-game.", null, new ConfigurationManagerAttributes { Order = 1, HideDefaultButton = true, CustomDrawer = TestWeather }));

        // ══════════════════════════════════════════════════════════════════════
        // Fixes and Reset All Settings are always at the end
        // ══════════════════════════════════════════════════════════════════════

        // ── Fixes ──
        AutoRepairMissingLore = _configInstance.Bind(FixesSection, "Auto Repair Missing Lore", false, new ConfigDescription("Automatically repair missing lore tablets that weren't unlocked due to a previous bug.", null, new ConfigurationManagerAttributes { Order = 1 }));
        AutoRepairMissingLore.SettingChanged += (_, _) =>
        {
            ShowBackupWarning();
            LoreRepairPatches.OnSettingChanged();
        };

        // ── Reset All Settings ──
        _configInstance.Bind(ResetAllSettingsSection, "Reset All Settings", false, new ConfigDescription("Set this to true and save the config file to reset all settings to default.", null, new ConfigurationManagerAttributes
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
        foreach (var ent in _configInstance.Entries
                     .Where(ent => ent.Value.BoxedValue != ent.Value.DefaultValue)
                     .Where(ent => ent.Key.Section != ResetAllSettingsSection))
        {
            ent.Value.BoxedValue = ent.Value.DefaultValue;
            Log.LogInfo($"[Config] Resetting {ent.Key} to default value: {ent.Value.DefaultValue}");
        }
    }

    private static float RoundToQuarter(float value) => (float)(Math.Round(value * 4.0) / 4.0);

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

    public enum LogType
    {
        Error,
        Info,
        Warning
    }

    public static void WriteLog(string message, LogType type = LogType.Info)
    {
        switch (type)
        {
            case LogType.Error:
                Log.LogError(message);
                break;
            case LogType.Info:
                if (ConfigCache.GetCachedValue(ConfigCache.Keys.EnableLogging, () => EnableLogging.Value))
                {
                    Log.LogInfo(message);
                }

                break;
            case LogType.Warning:
                Log.LogWarning(message);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    private static void ShowRestartMessage()
    {
        if (!PopupManager.ShowPopup)
        {
            PopupManager.ShowPopupDlg(RestartGameMessage, true);
        }
    }

    private static void ShowBackupWarning()
    {
        if (!PopupManager.ShowPopup && AutoRepairMissingLore.Value)
        {
            PopupManager.ShowPopupDlg(BackupSaveMessage, true);
        }
    }

    private static void HideBepInEx()
    {
        Chainloader.ManagerObject.hideFlags = HideFlags.HideAndDontSave;
        ThreadingHelper.Instance.gameObject.hideFlags = HideFlags.HideAndDontSave;
        DontDestroyOnLoad(Chainloader.ManagerObject);
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
                WriteLog($"{_cachedDirectLoadSkipKey.MainKey} pressed; skipping auto-load.");
            }

            MenuCleanupPatches.SkipAutoLoad = true;
        }

        // Quick save handling
        if (_cachedEnableQuickSaveValue && _cachedSaveShortcut.IsUp())
        {
            SaveAndLoad.Save();
            NotificationCentre.Instance.PlayGenericNotification("Game Saved!");
        }

        // Translation dump (debug utility, gated behind EnableLogging)
        if (EnableLogging.Value && DumpTranslationsKey.Value.IsUp())
        {
            DumpTranslations();
        }

        // Ad disabling - cache UI controller and components to avoid repeated queries
        if (_cachedDisableAdsValue)
        {
            if (!_cachedUIMainMenuController)
            {
                _cachedUIMainMenuController = UIMainMenuController;
                _cachedAdComponents = null;
            }

            if (_cachedUIMainMenuController)
            {
                _cachedAdComponents ??= _cachedUIMainMenuController.ad.GetComponents<Component>();

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

    private static void DumpTranslations()
    {
        var outputDir = Path.Combine(BepInEx.Paths.PluginPath, "CultOfQoL");
        Directory.CreateDirectory(outputDir);
        var outputPath = Path.Combine(outputDir, "translations_en.txt");

        var count = 0;
        using (var writer = new StreamWriter(outputPath, false, System.Text.Encoding.UTF8))
        {
            foreach (var source in LocalizationManager.Sources)
            {
                var englishIndex = source.GetLanguageIndex("English");
                if (englishIndex < 0) continue;

                foreach (var term in source.mTerms)
                {
                    var translation = term.GetTranslation(englishIndex);
                    if (string.IsNullOrEmpty(translation)) continue;

                    writer.WriteLine($"{term.Term} = {translation}");
                    count++;
                }
            }
        }

        Log.LogInfo($"[TranslationDump] Dumped {count} English translations to {outputPath}");
        NotificationCentre.Instance?.PlayGenericNotification($"Dumped {count} translations to plugins/CultOfQoL/");
    }
}
