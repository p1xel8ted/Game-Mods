namespace CultOfQoL;

public partial class Plugin
{
    internal static ConfigEntry<bool> EnableLogging = null!;
    internal static ConfigEntry<bool> EnableQuickSaveShortcut = null!;
    internal static ConfigEntry<KeyboardShortcut> SaveKeyboardShortcut = null!;
    internal static ConfigEntry<bool> SaveOnQuitToDesktop = null!;
    internal static ConfigEntry<bool> SaveOnQuitToMenu = null!;
    internal static ConfigEntry<bool> DirectLoadSave = null!;
    internal static ConfigEntry<int> SaveSlotToLoad = null!;
    internal static ConfigEntry<KeyboardShortcut> DirectLoadSkipKey = null!;
    internal static ConfigEntry<bool> DisableAds = null!;
    internal static ConfigEntry<bool> HideNewGameButtons = null!;
    internal static ConfigEntry<bool> EnableCustomUiScale = null!;
    internal static ConfigEntry<int> CustomUiScale = null!;
    internal static ConfigEntry<int> NotificationsScale = null!;
    internal static ConfigEntry<bool> SkipDevIntros = null!;
    internal static ConfigEntry<bool> SkipCrownVideo = null!;
    internal static ConfigEntry<bool> EasyFishing = null!;
    internal static ConfigEntry<bool> FastCollecting = null!;
    internal static ConfigEntry<bool> RemoveMenuClutter = null!;
    internal static ConfigEntry<bool> RemoveTwitchButton = null!;

    internal static ConfigEntry<bool> AllLootMagnets = null!;
    internal static ConfigEntry<bool> DoubleMagnetRange = null!;
    internal static ConfigEntry<bool> TripleMagnetRange = null!;
    internal static ConfigEntry<bool> UseCustomMagnetRange = null!;
    internal static ConfigEntry<int> CustomMagnetRange = null!;
    
    
    internal static ConfigEntry<bool> ReverseGoldenFleeceDamageChange = null!;
    internal static ConfigEntry<bool> IncreaseGoldenFleeceDamageRate = null!;
    internal static ConfigEntry<bool> AdjustRefineryRequirements = null!;
    
    internal static ConfigEntry<bool> NoNegativeTraits = null!;
    internal static ConfigEntry<bool> UseUnlockedTraitsOnly = null!;   
    internal static ConfigEntry<bool> IncludeImmortal = null!;
    internal static ConfigEntry<bool> IncludeDisciple = null!;
    internal static ConfigEntry<bool> ShowNotificationsWhenRemovingTraits = null!;
    internal static ConfigEntry<bool> ShowNotificationsWhenAddingTraits = null!;
    
    internal static ConfigEntry<bool> CleanseIllnessAndExhaustionOnLevelUp = null!;
    internal static ConfigEntry<bool> UnlockTwitchItems = null!;
    internal static ConfigEntry<bool> LumberAndMiningStationsDontAge = null!;
    internal static ConfigEntry<bool> CollectTitheFromOldFollowers = null!;
    internal static ConfigEntry<bool> IntimidateOldFollowers = null!;
    internal static ConfigEntry<bool> EnableGameSpeedManipulation = null!;

    internal static ConfigEntry<bool> DoubleSoulCapacity = null!;
    internal static ConfigEntry<bool> ShortenGameSpeedIncrements = null!;
    internal static ConfigEntry<bool> SlowDownTime = null!;
    internal static ConfigEntry<float> SlowDownTimeMultiplier = null!;
    internal static ConfigEntry<bool> DoubleLifespanInstead = null!;
    internal static ConfigEntry<bool> DisableGameOver = null!;
    
    internal static ConfigEntry<bool> FastRitualSermons = null!;

    internal static ConfigEntry<bool> TurnOffSpeakersAtNight = null!;
    internal static ConfigEntry<bool> ThriceMultiplyTarotCardLuck = null!;
    internal static ConfigEntry<bool> RareTarotCardsOnly = null!;
    internal static ConfigEntry<bool> FiftyPercentIncreaseToLifespanInstead = null!;

    internal static ConfigEntry<bool> EnableAutoInteract = null!;
    internal static ConfigEntry<int> TriggerAmount = null!;
    internal static ConfigEntry<bool> IncreaseRange = null!;
    internal static ConfigEntry<bool> UseCustomRange = null!;
    internal static ConfigEntry<float> CustomRangeMulti = null!;

    internal static ConfigEntry<bool> AddExhaustedToHealingBay = null!;
    internal static ConfigEntry<bool> NotifyOfScarecrowTraps = null!;
    internal static ConfigEntry<bool> NotifyOfNoFuel = null!;
    internal static ConfigEntry<bool> NotifyOfBedCollapse = null!;

    internal static ConfigEntry<bool> GiveFollowersNewNecklaces = null!;


    internal static ConfigEntry<bool> RandomWeatherChangeWhenExitingArea = null!;
    internal static ConfigEntry<bool> ChangeWeatherOnPhaseChange = null!;
    internal static ConfigEntry<bool> ShowPhaseNotifications = null!;
    internal static ConfigEntry<bool> ShowWeatherChangeNotifications = null!;

    internal static ConfigEntry<float> CustomSoulCapacityMulti = null!;
    internal static ConfigEntry<float> CustomSiloCapacityMulti = null!;
    
    internal static ConfigEntry<bool> DoubleSiloCapacity = null!;
    internal static ConfigEntry<bool> UseCustomSoulCapacity = null!;
    internal static ConfigEntry<bool> UseCustomSiloCapacity = null!;
    internal static ConfigEntry<bool> UseMultiplesOf32 = null!;

    internal static ConfigEntry<bool> EnableBaseDamageMultiplier = null!;
    internal static ConfigEntry<float> BaseDamageMultiplier = null!;

    internal static ConfigEntry<float> CustomDamageMulti = null!;


    internal static ConfigEntry<bool> RemoveHelpButtonInPauseMenu = null!;
    internal static ConfigEntry<bool> RemoveTwitchButtonInPauseMenu = null!;
    internal static ConfigEntry<bool> RemovePhotoModeButtonInPauseMenu = null!;

    internal static ConfigEntry<bool> EnableRunSpeedMulti = null!;
    internal static ConfigEntry<bool> EnableDodgeSpeedMulti = null!;
    internal static ConfigEntry<bool> EnableLungeSpeedMulti = null!;
    internal static ConfigEntry<float> RunSpeedMulti = null!;
    internal static ConfigEntry<float> DodgeSpeedMulti = null!;
    internal static ConfigEntry<float> LungeSpeedMulti = null!;

    internal static ConfigEntry<bool> OnlyShowDissenters = null!;

    internal static ConfigEntry<bool> DisablePropagandaSpeakerAudio = null!;

    internal static ConfigEntry<bool> UseCustomDamageValue = null!;


    internal static ConfigEntry<bool> RemoveLevelLimit = null!;

    internal static WriteOnce<float> LumberFastCollect { get; } = new();
    internal static WriteOnce<float> OtherFastCollect { get; } = new();

    internal static ConfigEntry<bool> MassBribe = null!;
    internal static ConfigEntry<bool> MassFertilize = null!;
    internal static ConfigEntry<bool> MassBless = null!;
    internal static ConfigEntry<bool> MassRomance = null!;
    internal static ConfigEntry<bool> MassBully = null!;
    internal static ConfigEntry<bool> MassReassure = null!;
    internal static ConfigEntry<bool> MassReeducate = null!;
    internal static ConfigEntry<bool> MassExtort = null!;
    internal static ConfigEntry<bool> MassPetDog = null!;
    internal static ConfigEntry<bool> MassIntimidate = null!;
    internal static ConfigEntry<bool> MassInspire = null!;
    internal static ConfigEntry<bool> MassWater = null!;
    internal static ConfigEntry<bool> MassLevelUp = null!;

    internal static ConfigEntry<bool> MassCollectFromBeds = null!;
    internal static ConfigEntry<bool> MassCollectFromOuthouses = null!;
    internal static ConfigEntry<bool> MassCollectFromOfferingShrines = null!;
    internal static ConfigEntry<bool> MassCollectFromPassiveShrines = null!;
    internal static ConfigEntry<bool> MassCollectFromCompost = null!;
    internal static ConfigEntry<bool> MassCollectFromHarvestTotems = null!;

    internal static WriteOnce<float> RunSpeed { get; } = new();
    internal static WriteOnce<float> DodgeSpeed { get; } = new();
    internal static UIMainMenuController? UIMainMenuController = null!;
}