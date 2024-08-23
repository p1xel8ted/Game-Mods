namespace CultOfQoL;

public partial class Plugin
{
    internal static ConfigEntry<bool> EnableLogging{ get; set;}
    internal static ConfigEntry<bool> EnableQuickSaveShortcut{ get; set;}
    internal static ConfigEntry<KeyboardShortcut> SaveKeyboardShortcut{ get; set;}
    internal static ConfigEntry<bool> SaveOnQuitToDesktop{ get; set;}
    internal static ConfigEntry<bool> SaveOnQuitToMenu{ get; set;}
    internal static ConfigEntry<bool> DirectLoadSave{ get; set;}
    internal static ConfigEntry<int> SaveSlotToLoad{ get; set;}
    internal static ConfigEntry<KeyboardShortcut> DirectLoadSkipKey{ get; set;}
    internal static ConfigEntry<bool> DisableAds{ get; set;}
    internal static ConfigEntry<bool> HideNewGameButtons{ get; set;}
    // internal static ConfigEntry<bool> EnableCustomUiScale{ get; set;}
    // internal static ConfigEntry<float> CustomUiScale{ get; set;}
    // internal static ConfigEntry<int> NotificationsScale{ get; set;}
    internal static ConfigEntry<bool> SkipDevIntros{ get; set;}
    internal static ConfigEntry<bool> SkipCrownVideo{ get; set;}
    internal static ConfigEntry<bool> EasyFishing{ get; set;}
    internal static ConfigEntry<bool> FastCollecting{ get; set;}
    internal static ConfigEntry<bool> RemoveMenuClutter{ get; set;}
    internal static ConfigEntry<bool> RemoveTwitchButton{ get; set;}

    internal static ConfigEntry<bool> AllLootMagnets{ get; set;}
    internal static ConfigEntry<bool> DoubleMagnetRange{ get; set;}
    internal static ConfigEntry<bool> TripleMagnetRange{ get; set;}
    internal static ConfigEntry<bool> UseCustomMagnetRange{ get; set;}
    internal static ConfigEntry<int> CustomMagnetRange{ get; set;}
    
    internal static ConfigEntry<bool> MainMenuGlitch{ get; set;}
    
    internal static ConfigEntry<bool> ReverseGoldenFleeceDamageChange{ get; set;}
    internal static ConfigEntry<bool> IncreaseGoldenFleeceDamageRate{ get; set;}
    internal static ConfigEntry<bool> AdjustRefineryRequirements{ get; set;}
    
    internal static ConfigEntry<bool> NoNegativeTraits{ get; set;}
    internal static ConfigEntry<bool> UseUnlockedTraitsOnly{ get; set;}   
    internal static ConfigEntry<bool> IncludeImmortal{ get; set;}
    internal static ConfigEntry<bool> IncludeDisciple{ get; set;}
    internal static ConfigEntry<bool> ShowNotificationsWhenRemovingTraits{ get; set;}
    internal static ConfigEntry<bool> ShowNotificationsWhenAddingTraits{ get; set;}
    
    internal static ConfigEntry<bool> CleanseIllnessAndExhaustionOnLevelUp{ get; set;}
    internal static ConfigEntry<bool> UnlockTwitchItems{ get; set;}
    internal static ConfigEntry<bool> LumberAndMiningStationsDontAge{ get; set;}
    internal static ConfigEntry<bool> CollectTitheFromOldFollowers{ get; set;}
    internal static ConfigEntry<bool> IntimidateOldFollowers{ get; set;}
    internal static ConfigEntry<bool> EnableGameSpeedManipulation{ get; set;}

    internal static ConfigEntry<bool> DoubleSoulCapacity{ get; set;}
    internal static ConfigEntry<bool> ShortenGameSpeedIncrements{ get; set;}
    internal static ConfigEntry<bool> SlowDownTime{ get; set;}
    internal static ConfigEntry<float> SlowDownTimeMultiplier{ get; set;}
    internal static ConfigEntry<bool> DoubleLifespanInstead{ get; set;}
    internal static ConfigEntry<bool> DisableGameOver{ get; set;}
    
    internal static ConfigEntry<bool> FastRitualSermons{ get; set;}

    internal static ConfigEntry<bool> TurnOffSpeakersAtNight{ get; set;}
    internal static ConfigEntry<bool> ThriceMultiplyTarotCardLuck{ get; set;}
    internal static ConfigEntry<bool> RareTarotCardsOnly{ get; set;}
    internal static ConfigEntry<bool> FiftyPercentIncreaseToLifespanInstead{ get; set;}

    internal static ConfigEntry<bool> EnableAutoInteract{ get; set;}
    internal static ConfigEntry<int> TriggerAmount { get; set;}
    internal static ConfigEntry<bool> IncreaseAutoCollectRange{ get; set;}
    internal static ConfigEntry<bool> UseCustomAutoInteractRange{ get; set;}
    internal static ConfigEntry<float> CustomAutoInteractRangeMulti{ get; set;}
    internal static ConfigEntry<bool> AutoCollectFromFarmStationChests{ get; set;}
    internal static ConfigEntry<bool> AddExhaustedToHealingBay{ get; set;}
    internal static ConfigEntry<bool> NotifyOfScarecrowTraps{ get; set;}
    internal static ConfigEntry<bool> NotifyOfNoFuel{ get; set;}
    internal static ConfigEntry<bool> NotifyOfBedCollapse{ get; set;}

    internal static ConfigEntry<bool> GiveFollowersNewNecklaces{ get; set;}


    internal static ConfigEntry<bool> RandomWeatherChangeWhenExitingArea{ get; set;}
    internal static ConfigEntry<bool> ChangeWeatherOnPhaseChange{ get; set;}
    internal static ConfigEntry<bool> ShowPhaseNotifications{ get; set;}
    internal static ConfigEntry<bool> ShowWeatherChangeNotifications{ get; set;}

    internal static ConfigEntry<float> CustomSoulCapacityMulti{ get; set;}
    internal static ConfigEntry<float> CustomSiloCapacityMulti{ get; set;}
    
    internal static ConfigEntry<bool> DoubleSiloCapacity{ get; set;}
    internal static ConfigEntry<bool> UseCustomSoulCapacity{ get; set;}
    internal static ConfigEntry<bool> UseCustomSiloCapacity{ get; set;}
    internal static ConfigEntry<bool> UseMultiplesOf32{ get; set;}

    internal static ConfigEntry<bool> EnableBaseDamageMultiplier{ get; set;}
    internal static ConfigEntry<float> BaseDamageMultiplier{ get; set;}

    internal static ConfigEntry<float> CustomDamageMulti{ get; set;}
    internal static ConfigEntry<bool> DisableRunSpeedInDungeons{ get; set;}
    internal static ConfigEntry<bool> DisableRunSpeedInCombat{ get; set;}

    internal static ConfigEntry<bool> RemoveHelpButtonInPauseMenu{ get; set;}
    internal static ConfigEntry<bool> RemoveTwitchButtonInPauseMenu{ get; set;}
    internal static ConfigEntry<bool> RemovePhotoModeButtonInPauseMenu{ get; set;}

    internal static ConfigEntry<bool> EnableRunSpeedMulti{ get; set;}
    internal static ConfigEntry<bool> EnableDodgeSpeedMulti{ get; set;}
    internal static ConfigEntry<bool> EnableLungeSpeedMulti{ get; set;}
    internal static ConfigEntry<float> RunSpeedMulti{ get; set;}
    internal static ConfigEntry<float> DodgeSpeedMulti{ get; set;}
    internal static ConfigEntry<float> LungeSpeedMulti{ get; set;}

    internal static ConfigEntry<bool> OnlyShowDissenters{ get; set;}

    internal static ConfigEntry<bool> DisablePropagandaSpeakerAudio{ get; set;}

    internal static ConfigEntry<bool> UseCustomDamageValue{ get; set;}


    internal static ConfigEntry<bool> RemoveLevelLimit{ get; set;}

    internal static WriteOnce<float> DefaultLumberFastCollect { get; } = new();
    internal static WriteOnce<float> OtherFastCollect { get; } = new();

    internal static ConfigEntry<bool> MassBribe{ get; set;}
    internal static ConfigEntry<bool> MassFertilize{ get; set;}
    internal static ConfigEntry<bool> MassBless{ get; set;}
    internal static ConfigEntry<bool> MassRomance{ get; set;}
    internal static ConfigEntry<bool> MassBully{ get; set;}
    internal static ConfigEntry<bool> MassReassure{ get; set;}
    internal static ConfigEntry<bool> MassReeducate{ get; set;}
    internal static ConfigEntry<bool> MassExtort{ get; set;}
    internal static ConfigEntry<bool> MassPetDog{ get; set;}
    internal static ConfigEntry<bool> MassIntimidate{ get; set;}
    internal static ConfigEntry<bool> MassInspire{ get; set;}
    internal static ConfigEntry<bool> MassWater{ get; set;}
    
    // internal static ConfigEntry<bool> MassHarvest{ get; set;}
    internal static ConfigEntry<bool> MassLevelUp{ get; set;}

    internal static ConfigEntry<bool> MakeOldFollowersWork{ get; set;}
    
    internal static ConfigEntry<bool> MassCollectFromBeds{ get; set;}
    internal static ConfigEntry<bool> MassCollectFromOuthouses{ get; set;}
    internal static ConfigEntry<bool> MassCollectFromOfferingShrines{ get; set;}
    internal static ConfigEntry<bool> MassCollectFromPassiveShrines{ get; set;}
    internal static ConfigEntry<bool> MassCollectFromCompost{ get; set;}
    internal static ConfigEntry<bool> MassCollectFromHarvestTotems{ get; set;}

    internal static WriteOnce<float> RunSpeed { get; } = new();
    internal static WriteOnce<float> DodgeSpeed { get; } = new();
    internal static UIMainMenuController UIMainMenuController{ get; set;}
}