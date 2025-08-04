namespace CultOfQoL;

public partial class Plugin
{
    private static ConfigEntry<bool> EnableLogging { get; set; }
    private static ConfigEntry<bool> EnableQuickSaveShortcut { get; set; }
    private static ConfigEntry<KeyboardShortcut> SaveKeyboardShortcut { get; set; }
    internal static ConfigEntry<bool> SaveOnQuitToDesktop { get; private set; }
    internal static ConfigEntry<bool> SaveOnQuitToMenu { get; private set; }
    internal static ConfigEntry<bool> DirectLoadSave { get; private set; }
    internal static ConfigEntry<int> SaveSlotToLoad { get; private set; }
    private static ConfigEntry<KeyboardShortcut> DirectLoadSkipKey { get; set; }
    private static ConfigEntry<bool> DisableAds { get; set; }
    internal static ConfigEntry<bool> HideNewGameButtons { get; private set; }

    internal static ConfigEntry<bool> SkipDevIntros { get; private set; }
    internal static ConfigEntry<bool> SkipCrownVideo { get; private set; }
    internal static ConfigEntry<bool> EasyFishing { get; private set; }
    internal static ConfigEntry<bool> FastCollecting { get; private set; }
    internal static ConfigEntry<bool> RemoveMenuClutter { get; private set; }
    internal static ConfigEntry<bool> RemoveTwitchButton { get; private set; }

    internal static ConfigEntry<KeyboardShortcut> ResetTimeScaleKey { get; private set; }
    internal static ConfigEntry<KeyboardShortcut> IncreaseGameSpeedKey { get; private set; }
    internal static ConfigEntry<KeyboardShortcut> DecreaseGameSpeedKey { get; private set; }
    internal static ConfigEntry<bool> AllLootMagnets { get; private set; }
    internal static ConfigEntry<bool> DoubleMagnetRange { get; private set; }
    internal static ConfigEntry<bool> TripleMagnetRange { get; private set; }
    internal static ConfigEntry<bool> UseCustomMagnetRange { get; private set; }
    internal static ConfigEntry<int> CustomMagnetRange { get; private set; }

    private static ConfigEntry<bool> MainMenuGlitch { get; set; }

    internal static ConfigEntry<bool> ReverseGoldenFleeceDamageChange { get; private set; }
    internal static ConfigEntry<bool> IncreaseGoldenFleeceDamageRate { get; private set; }
    internal static ConfigEntry<bool> AdjustRefineryRequirements { get; private set; }

    internal static ConfigEntry<bool> NoNegativeTraits { get; private set; }
    internal static ConfigEntry<bool> UseUnlockedTraitsOnly { get; private set; }
    internal static ConfigEntry<bool> IncludeImmortal { get; private set; }
    internal static ConfigEntry<bool> IncludeDisciple { get; private set; }
    internal static ConfigEntry<bool> ShowNotificationsWhenRemovingTraits { get; private set; }
    internal static ConfigEntry<bool> ShowNotificationsWhenAddingTraits { get; private set; }

    internal static ConfigEntry<bool> CleanseIllnessAndExhaustionOnLevelUp { get; private set; }
    internal static ConfigEntry<bool> UnlockTwitchItems { get; private set; }
    internal static ConfigEntry<bool> LumberAndMiningStationsDontAge { get; private set; }
    internal static ConfigEntry<bool> CollectTitheFromOldFollowers { get; private set; }
    internal static ConfigEntry<bool> IntimidateOldFollowers { get; private set; }
    internal static ConfigEntry<bool> EnableGameSpeedManipulation { get; private set; }

    internal static ConfigEntry<bool> DoubleSoulCapacity { get; private set; }
    internal static ConfigEntry<bool> ShortenGameSpeedIncrements { get; private set; }
    internal static ConfigEntry<bool> SlowDownTime { get; private set; }
    internal static ConfigEntry<float> SlowDownTimeMultiplier { get; private set; }
    internal static ConfigEntry<bool> DoubleLifespanInstead { get; private set; }
    internal static ConfigEntry<bool> DisableGameOver { get; private set; }

    internal static ConfigEntry<bool> FastRitualSermons { get; private set; }

    internal static ConfigEntry<bool> TurnOffSpeakersAtNight { get; private set; }
    internal static ConfigEntry<bool> ThriceMultiplyTarotCardLuck { get; private set; }
    internal static ConfigEntry<bool> RareTarotCardsOnly { get; private set; }
    internal static ConfigEntry<bool> FiftyPercentIncreaseToLifespanInstead { get; private set; }

    internal static ConfigEntry<bool> EnableAutoCollect { get; private set; }
    internal static ConfigEntry<int> TriggerAmount { get; private set; }
    internal static ConfigEntry<bool> IncreaseAutoCollectRange { get; private set; }
    internal static ConfigEntry<bool> UseCustomAutoInteractRange { get; private set; }
    internal static ConfigEntry<float> CustomAutoInteractRangeMulti { get; private set; }
    internal static ConfigEntry<bool> AutoCollectFromFarmStationChests { get; private set; }
    internal static ConfigEntry<bool> AddExhaustedToHealingBay { get; private set; }
    internal static ConfigEntry<bool> NotifyOfScarecrowTraps { get; private set; }
    internal static ConfigEntry<bool> NotifyOfNoFuel { get; private set; }
    internal static ConfigEntry<bool> NotifyOfBedCollapse { get; private set; }
    internal static ConfigEntry<bool> GiveFollowersNewNecklaces { get; private set; }
    internal static ConfigEntry<bool> RandomWeatherChangeWhenExitingArea { get; private set; }
    internal static ConfigEntry<bool> ChangeWeatherOnPhaseChange { get; private set; }
    internal static ConfigEntry<Color> LightSnowColor { get; private set; }
    internal static ConfigEntry<Color> LightWindColor { get; private set; }
    internal static ConfigEntry<Color> LightRainColor { get; private set; }
    internal static ConfigEntry<Color> MediumRainColor { get; private set; }
    internal static ConfigEntry<Color> HeavyRainColor { get; private set; }
    private static ConfigEntry<Weather.WeatherCombo> WeatherDropDown { get; set; }
    internal static ConfigEntry<bool> ShowPhaseNotifications { get; private set; }
    internal static ConfigEntry<bool> ShowWeatherChangeNotifications { get; private set; }

    internal static ConfigEntry<float> CustomSoulCapacityMulti { get; private set; }
    internal static ConfigEntry<float> CustomSiloCapacityMulti { get; private set; }

    internal static ConfigEntry<bool> DoubleSiloCapacity { get; private set; }
    internal static ConfigEntry<bool> UseCustomSoulCapacity { get; private set; }
    internal static ConfigEntry<bool> UseCustomSiloCapacity { get; private set; }
    internal static ConfigEntry<bool> UseMultiplesOf32 { get; private set; }

    internal static ConfigEntry<bool> EnableBaseDamageMultiplier { get; private set; }
    internal static ConfigEntry<float> BaseDamageMultiplier { get; private set; }

    internal static ConfigEntry<float> CustomDamageMulti { get; private set; }
    internal static ConfigEntry<bool> DisableRunSpeedInDungeons { get; private set; }
    internal static ConfigEntry<bool> DisableRunSpeedInCombat { get; private set; }

    internal static ConfigEntry<bool> RemoveHelpButtonInPauseMenu { get; private set; }
    internal static ConfigEntry<bool> RemoveTwitchButtonInPauseMenu { get; private set; }
    internal static ConfigEntry<bool> RemovePhotoModeButtonInPauseMenu { get; private set; }
    internal static ConfigEntry<bool> EnableRunSpeedMulti { get; private set; }
    internal static ConfigEntry<bool> EnableDodgeSpeedMulti { get; private set; }
    internal static ConfigEntry<bool> EnableLungeSpeedMulti { get; private set; }
    internal static ConfigEntry<float> RunSpeedMulti { get; private set; }
    internal static ConfigEntry<float> DodgeSpeedMulti { get; private set; }
    internal static ConfigEntry<float> LungeSpeedMulti { get; private set; }

    internal static ConfigEntry<bool> OnlyShowDissenters { get; private set; }

    internal static ConfigEntry<bool> DisablePropagandaSpeakerAudio { get; private set; }

    internal static ConfigEntry<bool> UseCustomDamageValue { get; private set; }


    internal static ConfigEntry<bool> RemoveLevelLimit { get; private set; }


    internal static ConfigEntry<bool> MassBribe { get; private set; }
    internal static ConfigEntry<bool> MassFertilize { get; private set; }
    internal static ConfigEntry<bool> MassBless { get; private set; }
    internal static ConfigEntry<bool> MassRomance { get; private set; }
    internal static ConfigEntry<bool> MassBully { get; private set; }
    internal static ConfigEntry<bool> MassReassure { get; private set; }
    internal static ConfigEntry<bool> MassReeducate { get; private set; }
    internal static ConfigEntry<bool> MassExtort { get; private set; }
    internal static ConfigEntry<bool> MassPetDog { get; private set; }
    internal static ConfigEntry<bool> MassIntimidate { get; private set; }
    internal static ConfigEntry<bool> MassInspire { get; private set; }
    internal static ConfigEntry<bool> MassWater { get; private set; }

    // internal static ConfigEntry<bool> MassHarvest{ get; private set;}
    //internal static ConfigEntry<bool> MassLevelUp{ get; private set;}

    internal static ConfigEntry<bool> MakeOldFollowersWork { get; private set; }

    internal static ConfigEntry<bool> MassCollectFromBeds { get; private set; }
    internal static ConfigEntry<bool> MassCollectFromOuthouses { get; private set; }
    internal static ConfigEntry<bool> MassCollectFromOfferingShrines { get; private set; }
    internal static ConfigEntry<bool> MassCollectFromPassiveShrines { get; private set; }
    internal static ConfigEntry<bool> MassCollectFromCompost { get; private set; }
    internal static ConfigEntry<bool> MassCollectFromHarvestTotems { get; private set; }

    internal static WriteOnce<float> RunSpeed { get; } = new();
    internal static WriteOnce<float> DodgeSpeed { get; } = new();
    internal static UIMainMenuController UIMainMenuController { get; set; }
    public static ConfigEntry<bool> VignetteEffect { get; private set; }
    public static ConfigEntry<bool> ReverseEnrichmentNerf { get; private set; }
    
    public static ConfigEntry<int> HarvestTotemRange { get; private set; }
    public static ConfigEntry<int> PropagandaSpeakerRange { get; private set; }
    
    public static ConfigEntry<int> FarmStationRange { get; private set; }
    
    public static ConfigEntry<int> FarmPlotSignRange { get; private set; }
    public static ConfigEntry<int> SinBossLimit { get; private set; }
    
    public static ConfigEntry<int> MinRangeLifeExpectancy { get; private set; }
    public static ConfigEntry<int> MaxRangeLifeExpectancy { get; private set; }
    
    public static ConfigEntry<float> RitualCooldownTime { get; private set; }
}