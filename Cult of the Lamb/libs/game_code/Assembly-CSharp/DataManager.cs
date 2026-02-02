// Decompiled with JetBrains decompiler
// Type: DataManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Flockade;
using I2.Loc;
using Lamb.UI;
using MessagePack;
using MessagePack.Formatters;
using Newtonsoft.Json;
using src.Data;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using UnityEngine;

#nullable disable
[XmlInclude(typeof (ObjectivesData))]
[XmlInclude(typeof (Objectives_Custom))]
[XmlInclude(typeof (Objectives_CollectItem))]
[XmlInclude(typeof (Objectives_BuildStructure))]
[XmlInclude(typeof (Objectives_RecruitFollower))]
[XmlInclude(typeof (Objectives_DepositFood))]
[XmlInclude(typeof (Objectives_KillEnemies))]
[XmlInclude(typeof (Objectives_NoDodge))]
[XmlInclude(typeof (Objectives_NoCurses))]
[XmlInclude(typeof (Objectives_NoDamage))]
[XmlInclude(typeof (Objectives_NoHealing))]
[XmlInclude(typeof (Objectives_AssignClothing))]
[XmlInclude(typeof (Objectives_BedRest))]
[XmlInclude(typeof (Objectives_RemoveStructure))]
[XmlInclude(typeof (Objectives_ShootDummy))]
[XmlInclude(typeof (Objectives_UseRelic))]
[XmlInclude(typeof (Objectives_TalkToFollower))]
[XmlInclude(typeof (Objectives_CookMeal))]
[XmlInclude(typeof (Objectives_PlaceStructure))]
[XmlInclude(typeof (Objectives_PerformRitual))]
[XmlInclude(typeof (Objectives_UnlockUpgrade))]
[XmlInclude(typeof (Objectives_EatMeal))]
[XmlInclude(typeof (Objectives_RecruitCursedFollower))]
[XmlInclude(typeof (Objectives_FindFollower))]
[XmlInclude(typeof (Objectives_Drink))]
[XmlInclude(typeof (Objectives_Mating))]
[XmlInclude(typeof (Objectives_CraftClothing))]
[XmlInclude(typeof (Objectives_BlizzardOffering))]
[XmlInclude(typeof (Objectives_BuildWinterDecorations))]
[XmlInclude(typeof (Objectives_BuildWinterDecorations.FinalizedData_BuildWinterDecorations))]
[XmlInclude(typeof (Objectives_GetAnimal))]
[XmlInclude(typeof (Objectives_GetAnimal.FinalizedData_GetAnimal))]
[XmlInclude(typeof (Objectives_FinishRace))]
[XmlInclude(typeof (Objectives_FinishRace.FinalizedData_Objectives_FinishRace))]
[XmlInclude(typeof (Objectives_GiveItem))]
[XmlInclude(typeof (Objectives_GiveItem.FinalizedData_GiveItem))]
[XmlInclude(typeof (Objectives_ShowFleece.FinalizedData_ShowFleece))]
[XmlInclude(typeof (Objectives_ShowFleece))]
[XmlInclude(typeof (Objectives_LegendaryWeaponRun.FinalizedData_LegendaryWeaponRun))]
[XmlInclude(typeof (Objectives_LegendaryWeaponRun))]
[XmlInclude(typeof (Objectives_WinFlockadeBet.FinalizedData_WinFlockadeBet))]
[XmlInclude(typeof (Objectives_WinFlockadeBet))]
[XmlInclude(typeof (Objectives_FlowerBaskets.FinalizedData_FlowerBaskets))]
[XmlInclude(typeof (Objectives_WalkAnimal))]
[XmlInclude(typeof (Objectives_WalkAnimal.FinalizedData_WalkAnimal))]
[XmlInclude(typeof (Objectives_FeedAnimal))]
[XmlInclude(typeof (Objectives_FeedAnimal.FinalizedData_FeedAnimal))]
[XmlInclude(typeof (Objectives_FlowerBaskets))]
[XmlInclude(typeof (Objectives_LegendarySwordReturn.FinalizedData_LegendarySwordReturn))]
[XmlInclude(typeof (Objectives_LegendarySwordReturn))]
[XmlInclude(typeof (Objectives_FindChildren.FinalizedData_FindChildren))]
[XmlInclude(typeof (Objectives_FindChildren))]
[XmlInclude(typeof (ObjectivesDataFinalized))]
[XmlInclude(typeof (Objectives_BlizzardOffering.FinalizedData_BlizzardOffering))]
[XmlInclude(typeof (Objectives_Custom.FinalizedData_Custom))]
[XmlInclude(typeof (Objectives_CollectItem.FinalizedData_CollectItem))]
[XmlInclude(typeof (Objectives_Mating.FinalizedData_Mating))]
[XmlInclude(typeof (Objectives_BuildStructure.FinalizedData_BuildStructure))]
[XmlInclude(typeof (Objectives_RecruitFollower.FinalizedData_RecruitFollower))]
[XmlInclude(typeof (Objectives_DepositFood.FinalizedData_DepositFood))]
[XmlInclude(typeof (Objectives_KillEnemies.FinalizedData_KillEnemies))]
[XmlInclude(typeof (Objectives_NoDodge.FinalizedData_NoDodge))]
[XmlInclude(typeof (Objectives_NoCurses.FinalizedData_NoCurses))]
[XmlInclude(typeof (Objectives_NoDamage.FinalizedData_NoDamage))]
[XmlInclude(typeof (Objectives_Drink.FinalizedData_Drink))]
[XmlInclude(typeof (Objectives_AssignClothing.FinalizedData_AssignClothing))]
[XmlInclude(typeof (Objectives_NoHealing.FinalizedData_NoHealing))]
[XmlInclude(typeof (Objectives_BedRest.FinalizedData_BedRest))]
[XmlInclude(typeof (Objectives_RemoveStructure.FinalizedData_RemoveStructure))]
[XmlInclude(typeof (Objectives_ShootDummy.FinalizedData_ShootDummy))]
[XmlInclude(typeof (Objectives_UseRelic.FinalizedData_UseRelic))]
[XmlInclude(typeof (Objectives_TalkToFollower.FinalizedData_TalkToFollower))]
[XmlInclude(typeof (Objectives_CookMeal.FinalizedData_CookMeal))]
[XmlInclude(typeof (Objectives_PlaceStructure.FinalizedData_PlaceStructure))]
[XmlInclude(typeof (Objectives_PerformRitual.FinalizedData_PerformRitual))]
[XmlInclude(typeof (Objectives_UnlockUpgrade.FinalizedData_UnlockUpgrade))]
[XmlInclude(typeof (Objectives_EatMeal.FinalizedData_EatMeal))]
[XmlInclude(typeof (Objectives_RecruitCursedFollower.FinalizedData_RecruitCursedFollower))]
[XmlInclude(typeof (Objectives_FindFollower.FinalizedData_FindFollower))]
[XmlInclude(typeof (Objectives_CraftClothing.FinalizedData_CraftClothing))]
[XmlInclude(typeof (FinalizedNotification))]
[XmlInclude(typeof (FinalizedFaithNotification))]
[XmlInclude(typeof (FinalizedFollowerNotification))]
[XmlInclude(typeof (FinalizedRelationshipNotification))]
[XmlInclude(typeof (FinalizedItemNotification))]
[XmlInclude(typeof (DungeonWorldMapIcon.DLCTemporaryMapNode))]
[MessagePackObject(false)]
public class DataManager
{
  [XmlIgnore]
  [IgnoreMember]
  [NonSerialized]
  public MetaData MetaData;
  [Key(0)]
  public bool AllowSaving = true;
  [Key(1)]
  public bool DisableSaving;
  [Key(2)]
  public bool PauseGameTime;
  [Key(3)]
  public bool GameOverEnabled;
  [Key(4)]
  public bool DisplayGameOverWarning;
  [Key(5)]
  public bool InGameOver;
  [Key(6)]
  public bool GameOver;
  [Key(7)]
  public bool DifficultyChosen;
  [Key(8)]
  public bool DifficultyReminded;
  [Key(9)]
  public bool DisableYngyaShrine;
  [Key(10)]
  public int playerDeaths;
  [Key(11)]
  public int playerDeathsInARow;
  [Key(12)]
  public int playerDeathsInARowFightingLeader;
  [Key(13)]
  public int FightPitRituals;
  [Key(14)]
  public int dungeonRun = -1;
  [Key(15)]
  public float dungeonRunDuration;
  [Key(16 /*0x10*/)]
  public List<Map.NodeType> dungeonVisitedRooms = new List<Map.NodeType>()
  {
    Map.NodeType.FirstFloor
  };
  [Key(17)]
  public List<FollowerLocation> dungeonLocationsVisited = new List<FollowerLocation>();
  [Key(18)]
  public List<int> FollowersRecruitedInNodes = new List<int>();
  [Key(19)]
  public int FollowersRecruitedThisNode;
  [Key(20)]
  public float TimeInGame;
  [Key(21)]
  public int KillsInGame;
  [Key(22)]
  public int dungeonRunXPOrbs;
  [Key(23)]
  public int ChestRewardCount = -1;
  [Key(24)]
  public bool BaseGoopDoorLocked;
  [Key(25)]
  public string BaseGoopDoorLoc = "";
  [Key(26)]
  public int STATS_FollowersStarvedToDeath;
  [Key(27)]
  public int STATS_Murders;
  [Key(28)]
  public int STATS_Sacrifices;
  [Key(29)]
  public int STATS_AnimalSacrifices;
  [Key(30)]
  public int STATS_NaturalDeaths;
  [Key(31 /*0x1F*/)]
  public int PlayerKillsOnRun;
  [Key(32 /*0x20*/)]
  public int PlayerStartingBlackSouls;
  [Key(33)]
  public bool GivenFollowerHearts;
  [Key(34)]
  public bool EnabledSpells = true;
  [Key(35)]
  public bool ForceDoctrineStones = true;
  [Key(36)]
  public int SpaceOutDoctrineStones = -1;
  [Key(37)]
  public int DoctrineStoneTotalCount;
  [Key(38)]
  public bool BuildShrineEnabled = true;
  [Key(39)]
  public bool EnabledHealing = true;
  [Key(40)]
  public bool EnabledSword = true;
  [Key(41)]
  public bool BonesEnabled = true;
  [Key(42)]
  public bool XPEnabled = true;
  [Key(43)]
  public bool ShownDodgeTutorial = true;
  [Key(44)]
  public bool ShownInventoryTutorial = true;
  [Key(45)]
  public int ShownDodgeTutorialCount;
  [Key(46)]
  public bool HadInitialDeathCatConversation = true;
  [Key(47)]
  public bool PlayerHasBeenGivenHearts = true;
  [Key(48 /*0x30*/)]
  public int TotalFirefliesCaught;
  [Key(49)]
  public int TotalSquirrelsCaught;
  [Key(50)]
  public int TotalBirdsCaught;
  [Key(51)]
  public int TotalBodiesHarvested;
  [Key(52)]
  public DataManager.OnboardingPhase CurrentOnboardingPhase;
  [Key(53)]
  public bool firstRecruit;
  [Key(54)]
  public int MissionariesCompleted;
  [Key(55)]
  public int PlayerFleece;
  [Key(56)]
  public int PlayerVisualFleece = -1;
  [Key(57)]
  public List<int> UnlockedFleeces = new List<int>();
  [Key(58)]
  public bool PostGameFleecesOnboarded;
  [Key(59)]
  public bool GoatFleeceOnboarded;
  [Key(60)]
  public bool CowboyFleeceOnboarded;
  [Key(61)]
  public List<ThoughtData> Thoughts = new List<ThoughtData>();
  [Key(62)]
  public bool CanReadMinds = true;
  [Key(63 /*0x3F*/)]
  public bool HappinessEnabled;
  [Key(64 /*0x40*/)]
  public bool TeachingsEnabled;
  [Key(65)]
  public bool SchedulingEnabled;
  [Key(66)]
  public bool PrayerEnabled;
  [Key(67)]
  public bool PrayerOrdered;
  [Key(68)]
  public bool HasBuiltCookingFire;
  [Key(69)]
  public bool HasBuiltFarmPlot;
  [Key(70)]
  public bool HasBuiltTemple1;
  [Key(71)]
  public bool HasBuiltTemple2;
  [Key(72)]
  public bool HasBuiltTemple3;
  [Key(73)]
  public bool HasBuiltTemple4;
  [Key(74)]
  public bool HasBuiltShrine1 = true;
  [Key(75)]
  public bool HasBuiltShrine2;
  [Key(76)]
  public bool HasBuiltShrine3;
  [Key(77)]
  public bool HasBuiltShrine4;
  [Key(78)]
  public bool HasBuiltPleasureShrine;
  [Key(79)]
  public bool HasOnboardedCultLevel;
  [Key(80 /*0x50*/)]
  public bool PerformedMushroomRitual;
  [Key(81)]
  public bool BuiltMushroomDecoration;
  [Key(82)]
  public bool HasBuiltSurveillance;
  [Key(83)]
  public int TempleDevotionBoxCoinCount;
  [Key(84)]
  public bool CanBuildShrine;
  [Key(85)]
  public int DaySinseLastMutatedFollower;
  [Key(86)]
  public bool SeasonsActive;
  [Key(87)]
  public int SeasonTimestamp;
  [Key(88)]
  public SeasonsManager.Season CurrentSeason;
  [Key(89)]
  public SeasonsManager.Season PreviousSeason;
  [Key(90)]
  public SeasonsManager.WeatherEvent CurrentWeatherEvent;
  [Key(91)]
  public int BlizzardEventID;
  [Key(92)]
  public int WeatherEventID;
  [Key(93)]
  public int WintersOccured;
  [Key(94)]
  public int WinterServerity;
  [Key(95)]
  public int NextWinterServerity;
  [Key(96 /*0x60*/)]
  public bool NextPhaseIsWeatherEvent;
  [Key(97)]
  public bool GivenBlizzardObjective;
  [Key(98)]
  public bool OnboardedDLCEntrance;
  [Key(99)]
  public bool OnboardedBaseExpansion;
  [Key(100)]
  public bool OnboardedWolf;
  [Key(101)]
  public bool OnboardedLambTown;
  [Key(102)]
  public bool OnboardedLambGhostNPCs;
  [Key(103)]
  public bool OnboardedYngyaAwoken;
  [Key(104)]
  public bool OnboardedDungeon6;
  [Key(105)]
  public bool OnboardedIntroYngyaShrine;
  [Key(106)]
  public bool OnboardedFindLostSouls;
  [Key(107)]
  public bool OnboardedAddFuelToFurnace;
  [Key(108)]
  public bool RequiresSnowedUnderOnboarded;
  [Key(109)]
  public bool RequiresWolvesOnboarded;
  [Key(110)]
  public bool WinterMaxSeverity;
  [Key(111)]
  public bool RequiresBlizzardOnboarded;
  [Key(112 /*0x70*/)]
  public bool OnboardedRanchingWolves;
  [Key(113)]
  public bool OnboardedBlizzards;
  [Key(114)]
  public bool OnboardedSnowedUnder;
  [Key(115)]
  public bool OnboardedWitheredCrops;
  [Key(116)]
  public bool OnboardedLongNights;
  [Key(117)]
  public bool LongNightActive;
  [Key(118)]
  public bool OnboardedRanching;
  [Key(119)]
  public bool OnboardedSeasons;
  [Key(120)]
  public bool OnboardedDLCBuildMenu;
  [Key(121)]
  public int DLCUpgradeTreeSnowIncrement;
  [Key(122)]
  public bool BuiltFurnace;
  [Key(123)]
  public bool OnboardedLightningShardDungeon;
  [Key(124)]
  public bool OnboardedRotstoneDungeon;
  [Key(1321)]
  public bool OnboardedYewCursedDungeon;
  [Key(125)]
  public bool OnboardedRotstone;
  [Key(126)]
  public bool CollectedRotstone;
  [Key(127 /*0x7F*/)]
  public bool CollectedYewMutated;
  [Key(1343)]
  public bool CollectedLightningShards;
  [Key(128 /*0x80*/)]
  public bool OnboardedMutationRoom;
  [Key(129)]
  public bool OnboardedRotHelobFollowers;
  [Key(130)]
  public bool PlayedFinalYngyaConvo;
  [Key(131)]
  public int YngyaOffering;
  [Key(132)]
  public int YngyaRotOfferingsReceived;
  [Key(133)]
  public bool SpokeToYngyaOnMountainTop;
  [Key(134)]
  public bool ShowIcegoreRoom;
  [Key(135)]
  public bool EncounteredIcegoreRoom;
  [Key(136)]
  public bool SpokenToRatauWinter;
  [Key(137)]
  public bool SpokenToPlimboWinter;
  [Key(138)]
  public bool SpokenToPlimboBlunderbuss;
  [Key(139)]
  public bool SpokenToClauneckWinter;
  [Key(140)]
  public bool SpokenToKudaiiWinter;
  [Key(141)]
  public bool SpokenToChemachWinter;
  [Key(142)]
  public bool SpokenToChemachRot;
  [Key(143)]
  public bool SpokenToKudaiiRot;
  [Key(144 /*0x90*/)]
  public bool SpokenToClauneckRot;
  [Key(145)]
  public List<int> DLCDungeonNodesCompleted = new List<int>();
  [Key(146)]
  public int DLCDungeonNodeCurrent = -1;
  [Key(147)]
  public int DLCKey_1;
  [Key(148)]
  public int DLCKey_2;
  [Key(149)]
  public int DLCKey_3;
  [Key(150)]
  public bool RevealedPostGameDungeon5;
  [Key(151)]
  public bool RevealedPostGameDungeon6;
  [Key(152)]
  public bool RevealDLCDungeonNode;
  [Key(153)]
  public int CurrentDLCDungeonID = -1;
  [Key(154)]
  public DungeonWorldMapIcon.NodeType CurrentDLCNodeType;
  [Key(155)]
  public bool HasYngyaConvo;
  [Key(156)]
  public bool IsMiniBoss = true;
  [Key(157)]
  public bool IsLambGhostRescue;
  [Key(158)]
  public int DLCDungeon5MiniBossIndex;
  [Key(159)]
  public int DLCDungeon6MiniBossIndex;
  [Key(160 /*0xA0*/)]
  public int PuzzleRoomsCompleted;
  [Key(161)]
  public bool RancherSpokeAboutBrokenShop;
  [Key(162)]
  public bool RancherShopFixed;
  [Key(163)]
  public bool RancherOnboardedLightningShards;
  [Key(164)]
  public bool RancherOnboardedHolyYew;
  [Key(165)]
  public bool FlockadeSpokeAboutBrokenShop;
  [Key(166)]
  public bool FlockadeShopFixed;
  [Key(167)]
  public bool FlockadeBlacksmithWon;
  [Key(168)]
  public bool FlockadeDecoWon;
  [Key(169)]
  public bool FlockadeFlockadeWon;
  [Key(170)]
  public bool FlockadeGraveyardWon;
  [Key(171)]
  public bool FlockadeRancherWon;
  [Key(172)]
  public bool FlockadeTarotWon;
  [Key(1367)]
  public int FlockadeBlacksmithWoolWon;
  [Key(1362)]
  public int FlockadeDecoWoolWon;
  [Key(1363)]
  public int FlockadeFlockadeWoolWon;
  [Key(1364)]
  public int FlockadeGraveyardWoolWon;
  [Key(1365)]
  public int FlockadeRancherWoolWon;
  [Key(1366)]
  public int FlockadeTarotWoolWon;
  [Key(173)]
  public bool TarotSpokeAboutBrokenShop;
  [Key(174)]
  public bool TarotShopFixed;
  [Key(175)]
  public bool DecoSpokeAboutBrokenShop;
  [Key(176 /*0xB0*/)]
  public bool DecoShopFixed;
  [Key(177)]
  public bool BlacksmithSpokeAboutBrokenShop;
  [Key(178)]
  public bool BlacksmithShopFixed;
  [Key(179)]
  public bool GraveyardSpokeAboutBrokenShop;
  [Key(180)]
  public bool GraveyardShopFixed;
  [Key(181)]
  public bool OnboardedLambTownGhost7;
  [Key(182)]
  public bool OnboardedLambTownGhost8;
  [Key(183)]
  public bool OnboardedLambTownGhost9;
  [Key(184)]
  public bool OnboardedLambTownGhost10;
  [Key(185)]
  public int NPCsRescued = -1;
  [Key(186)]
  public bool NPCGhostRancherRescued;
  [Key(187)]
  public bool NPCGhostFlockadeRescued;
  [Key(188)]
  public bool NPCGhostTarotRescued;
  [Key(189)]
  public bool NPCGhostDecoRescued;
  [Key(190)]
  public bool NPCGhostBlacksmithRescued;
  [Key(191)]
  public bool NPCGhostGraveyardRescued;
  [Key(192 /*0xC0*/)]
  public bool NPCGhostGeneric7Rescued;
  [Key(193)]
  public bool NPCGhostGeneric8Rescued;
  [Key(194)]
  public bool NPCGhostGeneric9Rescued;
  [Key(195)]
  public bool NPCGhostGeneric10Rescued;
  [Key(1347)]
  public bool NPCGhostGeneric11Rescued;
  [Key(1344)]
  public bool FoundHollowKnightWool;
  [Key(196)]
  public bool RepairedLegendarySword;
  [Key(197)]
  public bool RepairedLegendaryAxe;
  [Key(198)]
  public bool RepairedLegendaryHammer;
  [Key(199)]
  public bool RepairedLegendaryGauntlet;
  [Key(200)]
  public bool RepairedLegendaryBlunderbuss;
  [Key(201)]
  public bool RepairedLegendaryDagger;
  [Key(202)]
  public bool RepairedLegendaryChains;
  [Key(203)]
  public bool OnboardedLegendaryWeapons;
  [Key(204)]
  public bool FindBrokenHammerWeapon;
  [Key(205)]
  public bool GivenBrokenHammerWeaponQuest;
  [Key(206)]
  public bool GaveChosenChildQuest;
  [Key(207)]
  public bool ChosenChildLeftInTheMidasCave;
  [Key(208 /*0xD0*/)]
  public bool FoundLegendarySword;
  [Key(209)]
  public int ChosenChildMeditationQuestDay;
  [Key(210)]
  public bool LegendarySwordHinted;
  [Key(211)]
  public bool LegendaryAxeHinted;
  [Key(212)]
  public bool DeliveredCharybisLetter;
  [Key(213)]
  public bool BringFishermanWoolStarted;
  [Key(214)]
  public int FishermanGaveWoolAmount;
  [Key(215)]
  public bool BroughtFishingRod;
  [Key(216)]
  public bool LegendaryDaggerHinted;
  [Key(217)]
  public bool FoundLegendaryGauntlets;
  [Key(218)]
  public bool LegendaryGauntletsHinted;
  [Key(219)]
  public bool LegendaryBlunderbussHinted;
  [Key(220)]
  public bool LegendaryBlunderbussPlimboEaterEggTalked;
  [Key(1368)]
  public bool FoundLegendaryDagger;
  [Key(1369)]
  public bool FoundLegendaryBlunderbuss;
  [XmlIgnore]
  [IgnoreMember]
  [NonSerialized]
  public bool LegendaryBlunderbussPlimboEasterEggActive;
  [Key(221)]
  public List<EquipmentType> KudaaiLegendaryWeaponsResponses = new List<EquipmentType>();
  [Key(222)]
  public List<EquipmentType> LegendaryWeaponsUnlockOrder = new List<EquipmentType>();
  [Key(223)]
  public bool EncounteredBaseExpansionNPC;
  [Key(224 /*0xE0*/)]
  public int WeatherEventTriggeredDay = -1;
  [Key(225)]
  public float WeatherEventOverTime = -1f;
  [Key(226)]
  public float WeatherEventDurationTime = -1f;
  [Key(227)]
  public int SeasonSpecialEventTriggeredDay = -1;
  [Key(228)]
  public float TimeSinceLastSnowedUnderStructure;
  [Key(229)]
  public float TimeSinceLastLightingStrikedFollower;
  [Key(230)]
  public float TimeSinceLastLightingStrikedStructure;
  [Key(231)]
  public float TimeSinceLastAflamedStructure;
  [Key(232)]
  public float TimeSinceLastAflamedFollower;
  [Key(233)]
  public float TimeSinceLastStolenCoins = -1f;
  [Key(234)]
  public float TimeSinceLastMurderedFollowerFromFollower = -1f;
  [Key(235)]
  public float TimeSinceLastSnowPileSpawned = -1f;
  [Key(236)]
  public float Temperature = 50f;
  [Key(237)]
  public bool FollowerOnboardedWinterComing;
  [Key(238)]
  public bool FollowerOnboardedWinterHere;
  [Key(239)]
  public bool FollowerOnboardedWinterAlmostHere;
  [Key(240 /*0xF0*/)]
  public bool FollowerOnboardedBlizzard;
  [Key(241)]
  public bool FollowerOnboardedFreezing;
  [Key(242)]
  public bool FollowerOnboardedOverheating;
  [Key(243)]
  public bool FollowerOnboardedWoolyShack;
  [Key(244)]
  public bool FollowerOnboardedRanchChoppingBlock;
  [Key(245)]
  public bool FollowerOnboardedAutumnComing;
  [Key(246)]
  public bool FollowerOnboardedTyphoon;
  [Key(247)]
  public bool TriedTailorRequiresRevealingFromBase;
  [Key(248)]
  public bool TailorRequiresRevealingFromBase;
  [Key(249)]
  public int NudeClothingCount;
  [Key(250)]
  public bool SinSermonEnabled;
  [Key(251)]
  public bool PleasureRevealed;
  [Key(252)]
  public bool PleasureDoctrineOnboarded;
  [Key(253)]
  public bool PleasureDoctrineEnabled;
  [Key(254)]
  public bool WinterDoctrineEnabled;
  [Key(255 /*0xFF*/)]
  public int WokeUpEveryoneDay = -1;
  [Key(256 /*0x0100*/)]
  public bool DiedLastRun;
  [Key(257)]
  public Lamb.UI.DeathScreen.UIDeathScreenOverlayController.Results LastRunResults = Lamb.UI.DeathScreen.UIDeathScreenOverlayController.Results.None;
  [Key(258)]
  public float LastFollowerToStarveToDeath;
  [Key(259)]
  public float LastFollowerToFreezeToDeath;
  [Key(260)]
  public float LastFollowerToOverheatToDeath;
  [Key(261)]
  public float LastFollowerToBurnToDeath;
  [Key(262)]
  public float LastFollowerToStartStarving;
  [Key(263)]
  public float LastFollowerToStartDissenting;
  [Key(264)]
  public float LastFollowerToStartFreezing;
  [Key(265)]
  public float LastFollowerToStartSoaking;
  [Key(266)]
  public float LastFollowerToStartOverheating;
  [Key(267)]
  public float LastFollowerToReachOldAge;
  [Key(268)]
  public float LastFollowerToBecomeIll;
  [Key(269)]
  public float LastFollowerToBecomeIllFromSleepingNearIllFollower;
  [Key(270)]
  public float LastFollowerToPassOut;
  [Key(271)]
  public int LastFollowerPurchasedFromSpider = -1;
  [Key(272)]
  public float TimeSinceFaithHitEmpty = -1f;
  [Key(273)]
  public float TimeSinceLastCrisisOfFaithQuest;
  [Key(274)]
  public List<FollowerLocation> PalworldSkinsGivenLocations = new List<FollowerLocation>();
  [Key(275)]
  public List<string> PalworldEggSkinsGiven = new List<string>();
  [Key(276)]
  public int PalworldEggsCollected;
  [Key(1351)]
  public int DragonEggsCollected;
  [Key(277)]
  public bool ForcePalworldEgg = true;
  [Key(278)]
  public int JudgementAmount;
  [Key(279)]
  public float HungerBarCount;
  [Key(280)]
  public float IllnessBarCount;
  [Key(281)]
  public float IllnessBarDynamicMax;
  [Key(282)]
  public float WarmthBarCount;
  [Key(283)]
  public float StaticFaith = 55f;
  [Key(284)]
  public float CultFaith;
  [Key(285)]
  public float LastBrainwashed = float.MinValue;
  [Key(286)]
  public float LastHolidayDeclared = float.MinValue;
  [Key(287)]
  public float LastPurgeDeclared = float.MinValue;
  [Key(288)]
  public float LastNudismDeclared = float.MinValue;
  [Key(289)]
  public float LastWorkThroughTheNight = float.MinValue;
  [Key(290)]
  public float LastConstruction = float.MinValue;
  [Key(291)]
  public float LastEnlightenment = float.MinValue;
  [Key(292)]
  public float LastFastDeclared = float.MinValue;
  [Key(293)]
  public float LastWarmthRitualDeclared = float.MinValue;
  [Key(294)]
  public float LastFeastDeclared = float.MinValue;
  [Key(295)]
  public float LastFishingDeclared = float.MinValue;
  [Key(296)]
  public float LastHalloween = float.MinValue;
  [Key(297)]
  public float LastCNY = float.MinValue;
  [Key(298)]
  public float LastCthulhu = float.MinValue;
  [Key(299)]
  public float LastRanchRitualMeat = float.MinValue;
  [Key(300)]
  public float LastRanchRitualHarvest = float.MinValue;
  [Key(301)]
  public float LastArcherShot = float.MinValue;
  [Key(302)]
  public float LastSimpleGuardianAttacked = float.MinValue;
  [Key(303)]
  public float LastSimpleGuardianRingProjectiles = float.MinValue;
  [Key(304)]
  public float LastSimpleGuardianPatternShot = float.MinValue;
  [Key(305)]
  public int LastDayTreesAtBase = -1;
  [Key(306)]
  public int LastSnowPileAtBase = -1;
  [Key(307)]
  public int PreviousSermonDayIndex = -1;
  public const int SIN_SERMON_COST = 3;
  [Key(308)]
  public int PreviousSinSermonDayIndex = -1;
  [Key(309)]
  public SermonCategory PreviousSermonCategory;
  [Key(310)]
  public int ShrineLevel;
  [Key(311)]
  public int TempleLevel = -1;
  [Key(312)]
  public int TempleBorder;
  [Key(313)]
  public bool TempleUnlockedBorder5;
  [Key(314)]
  public bool TempleUnlockedBorder6;
  [Key(315)]
  public bool GivenSermonQuest;
  [Key(316)]
  public bool GivenFaithOfFlockQuest;
  [Key(317)]
  public bool PrayedAtMassiveMonsterShrine;
  [Key(318)]
  public string TwitchSecretKey;
  [Key(319)]
  public string TwitchToken;
  [Key(320)]
  public string ChannelID;
  [Key(321)]
  public string ChannelName;
  [Key(322)]
  public List<string> ReadTwitchMessages = new List<string>();
  [Key(323)]
  public int TotemContributions;
  [Key(324)]
  public bool TwitchSentFollowers;
  [Key(325)]
  public TwitchSettings TwitchSettings = new TwitchSettings();
  [Key(326)]
  public int TwitchTotemsCompleted;
  [Key(327)]
  public float TwitchNextHHEvent = -1f;
  [Key(328)]
  public List<string> TwitchFollowerViewerIDs = new List<string>();
  [Key(329)]
  public List<string> TwitchFollowerIDs = new List<string>();
  [Key(330)]
  public bool OnboardingFinished;
  [Key(331)]
  public string SaveUniqueID = "";
  [Key(332)]
  public List<string> enemiesEncountered = new List<string>();
  [Key(333)]
  public bool Chain1;
  [Key(334)]
  public bool Chain2;
  [Key(335)]
  public bool Chain3;
  [Key(336)]
  public int DoorRoomChainProgress = -1;
  [Key(337)]
  public int DoorRoomDoorsProgress = -1;
  [Key(338)]
  public int Dungeon1_Layer = 1;
  [Key(339)]
  public int Dungeon2_Layer = 1;
  [Key(340)]
  public int Dungeon3_Layer = 1;
  [Key(341)]
  public int Dungeon4_Layer = 1;
  [Key(342)]
  public int Dungeon5_Layer = 1;
  [Key(343)]
  public int Dungeon6_Layer = 1;
  [Key(344)]
  public bool WinterLoopEnabled = true;
  [Key(345)]
  public int WinterLoopModifiedDay;
  [Key(346)]
  public int ValentinsDayYear;
  [Key(347)]
  public List<string> CheatHistory = new List<string>();
  [Key(348)]
  public bool First_Dungeon_Resurrecting = true;
  [Key(349)]
  public bool PermadeDeathActive;
  [Key(350)]
  public int SpidersCaught;
  [Key(351)]
  public int FrogFollowerCount;
  [Key(352)]
  public bool PhotoCameraLookedAtGallery;
  [Key(353)]
  public bool PhotoUsedCamera;
  public const int SPY_DURATION = 5;
  [Key(354)]
  public List<FollowerClothingType> clothesCrafted = new List<FollowerClothingType>();
  [Key(1352)]
  public StructuresData.Ranchable_Animal bestFriendAnimal;
  [Key(1353)]
  public int bestFriendAnimalLevel;
  [Key(1354)]
  public float bestFriendAnimalAdoration;
  [Key(355)]
  public List<MiniBossController.MiniBossData> MiniBossData = new List<MiniBossController.MiniBossData>();
  [Key(356)]
  public List<DataManager.LocationAndLayer> CachePreviousRun = new List<DataManager.LocationAndLayer>();
  [Key(357)]
  public List<FollowerLocation> DiscoveredLocations = new List<FollowerLocation>();
  [Key(358)]
  public List<FollowerLocation> VisitedLocations = new List<FollowerLocation>();
  [Key(359)]
  public List<FollowerLocation> NewLocationFaithReward = new List<FollowerLocation>();
  [Key(360)]
  public List<FollowerLocation> DissentingFolllowerRooms = new List<FollowerLocation>();
  [IgnoreMember]
  [XmlIgnore]
  [NonSerialized]
  public FollowerLocation CurrentLocation = FollowerLocation.Base;
  [Key(361)]
  public List<DataManager.LocationAndLayer> OpenedDungeonDoors = new List<DataManager.LocationAndLayer>();
  [Key(362)]
  public List<string> KeyPiecesFromLocation = new List<string>();
  [Key(363)]
  public List<FollowerLocation> UsedFollowerDispensers = new List<FollowerLocation>();
  [Key(364)]
  public List<FollowerLocation> UnlockedBossTempleDoor = new List<FollowerLocation>();
  [Key(365)]
  public List<FollowerLocation> UnlockedDungeonDoor = new List<FollowerLocation>();
  [Key(366)]
  public List<FollowerLocation> BossesCompleted = new List<FollowerLocation>();
  [Key(367)]
  public List<FollowerLocation> BossesEncountered = new List<FollowerLocation>();
  [Key(368)]
  public List<FollowerLocation> DoorRoomBossLocksDestroyed = new List<FollowerLocation>();
  [Key(369)]
  public List<FollowerLocation> SignPostsRead = new List<FollowerLocation>();
  [Key(370)]
  public bool ShrineDoor;
  [Key(371)]
  public List<int> JobBoardsClaimedQuests = new List<int>();
  [Key(372)]
  public bool OnboardedRanchingJobBoard;
  [Key(373)]
  public bool CompletedRanchingJobBoard;
  [Key(374)]
  public bool OnboardedFlockadeJobBoard;
  [Key(375)]
  public bool CompletedFlockadeJobBoard;
  [Key(376)]
  public bool OnboardedBlacksmithJobBoard;
  [Key(377)]
  public bool CompletedBlacksmithJobBoard;
  [Key(378)]
  public bool OnboardedTarotJobBoard;
  [Key(379)]
  public bool CompletedTarotJobBoard;
  [Key(380)]
  public bool OnboardedDecoJobBoard;
  [Key(381)]
  public bool CompletedDecoJobBoard;
  [Key(382)]
  public bool OnboardedGraveyardJobBoard;
  [Key(383)]
  public bool CompletedGraveyardJobBoard;
  [Key(384)]
  public bool HasFurnace;
  [Key(385)]
  public bool BaseDoorEast;
  [Key(386)]
  public bool BaseDoorNorthEast;
  [Key(387)]
  public bool BaseDoorNorthWest;
  [Key(388)]
  public bool BossForest;
  [Key(389)]
  public bool ForestTempleDoor;
  [Key(390)]
  public List<int> CompletedQuestFollowerIDs = new List<int>();
  [Key(391)]
  public DataManager.CultLevel CurrentCultLevel;
  [Key(392)]
  public List<UnlockManager.UnlockType> MechanicsUnlocked = new List<UnlockManager.UnlockType>();
  [Key(393)]
  public List<SermonsAndRituals.SermonRitualType> UnlockedSermonsAndRituals = new List<SermonsAndRituals.SermonRitualType>();
  [Key(394)]
  public List<StructureBrain.TYPES> UnlockedStructures = new List<StructureBrain.TYPES>();
  [Key(395)]
  public List<StructureBrain.TYPES> HistoryOfStructures = new List<StructureBrain.TYPES>();
  [Key(396)]
  public Dictionary<StructureBrain.TYPES, int> DayPreviosulyUsedStructures = new Dictionary<StructureBrain.TYPES, int>();
  [Key(397)]
  public bool NewBuildings;
  [Key(398)]
  public List<TutorialTopic> RevealedTutorialTopics = new List<TutorialTopic>();
  [IgnoreMember]
  [NonSerialized]
  public List<TutorialTopic> IgnoreFromQuickStart = new List<TutorialTopic>()
  {
    TutorialTopic.Purgatory,
    TutorialTopic.Twitch,
    TutorialTopic.Twitch_2,
    TutorialTopic.Twitch_3,
    TutorialTopic.Twitch_4,
    TutorialTopic.Omnipresence,
    TutorialTopic.PlayerExhausted,
    TutorialTopic.PlayerStarving,
    TutorialTopic.PlayerHunger,
    TutorialTopic.PlayerSleep,
    TutorialTopic.ShieldWeapon,
    TutorialTopic.Winter,
    TutorialTopic.Freezing,
    TutorialTopic.Furnace,
    TutorialTopic.Winter_1,
    TutorialTopic.Winter_2,
    TutorialTopic.Winter_3,
    TutorialTopic.Winter_4,
    TutorialTopic.Winter_5,
    TutorialTopic.WeatherVane,
    TutorialTopic.WinterOver,
    TutorialTopic.Wool,
    TutorialTopic.MutatedFollower,
    TutorialTopic.WinterAbility,
    TutorialTopic.Midwinter,
    TutorialTopic.JobBoards,
    TutorialTopic.SacrificeTable
  };
  [Key(399)]
  public List<StructuresData.ResearchObject> CurrentResearch = new List<StructuresData.ResearchObject>();
  [Key(400)]
  public Lamb.UI.UpgradeTreeNode.TreeTier CurrentUpgradeTreeTier;
  [Key(401)]
  public Lamb.UI.UpgradeTreeNode.TreeTier DLCCurrentUpgradeTreeTier;
  [Key(402)]
  public Lamb.UI.UpgradeTreeNode.TreeTier CurrentPlayerUpgradeTreeTier;
  [Key(403)]
  public UpgradeSystem.Type MostRecentTreeUpgrade = UpgradeSystem.Type.Building_Temple;
  [Key(404)]
  public UpgradeSystem.Type MostRecentPlayerTreeUpgrade = UpgradeSystem.Type.PUpgrade_Heart_1;
  [Key(405)]
  public List<UpgradeSystem.Type> UnlockedUpgrades = new List<UpgradeSystem.Type>();
  [Key(406)]
  public List<DoctrineUpgradeSystem.DoctrineType> DoctrineUnlockedUpgrades = new List<DoctrineUpgradeSystem.DoctrineType>();
  [Key(407)]
  public List<UpgradeSystem.UpgradeCoolDown> UpgradeCoolDowns = new List<UpgradeSystem.UpgradeCoolDown>();
  [Key(408)]
  public List<FollowerTrait.TraitType> CultTraits = new List<FollowerTrait.TraitType>();
  [Key(409)]
  public List<string> WeaponUnlockedUpgrades = new List<string>();
  [Key(410)]
  public string CultName;
  [Key(411)]
  public string MysticKeeperName = "???";
  [Key(412)]
  public int PlayerTriedToEnterMysticDimensionCount;
  [XmlIgnore]
  [NonSerialized]
  public static string[] MysticShopKeeperSkins = new string[15]
  {
    "Penguin",
    "Lion",
    "Shrimp",
    "Koala",
    "Owl",
    "Volvy",
    "TwitchPoggers",
    "TwitchDog",
    "TwitchDogAlt",
    "TwitchCat",
    "TwitchMouse",
    "StarBunny",
    "Pelican",
    "Kiwi",
    "DogTeddy"
  };
  [XmlIgnore]
  [NonSerialized]
  public static StructureBrain.TYPES[] MysticShopKeeperDecorations = new StructureBrain.TYPES[17]
  {
    StructureBrain.TYPES.DECORATION_DST_BEEFALOSKELETON,
    StructureBrain.TYPES.DECORATION_DST_GLOMMERSTATUE,
    StructureBrain.TYPES.DECORATION_TWITCH_FLAG_CROWN,
    StructureBrain.TYPES.DECORATION_TWITCH_MUSHROOM_BAG,
    StructureBrain.TYPES.DECORATION_TWITCH_ROSE_BUSH,
    StructureBrain.TYPES.DECORATION_TWITCH_STONE_FLAG,
    StructureBrain.TYPES.DECORATION_TWITCH_STONE_STATUE,
    StructureBrain.TYPES.DECORATION_TWITCH_WOODEN_GUARDIAN,
    StructureBrain.TYPES.DECORATION_GNOME1,
    StructureBrain.TYPES.DECORATION_GNOME2,
    StructureBrain.TYPES.DECORATION_GNOME3,
    StructureBrain.TYPES.DECORATION_GOAT_STATUE,
    StructureBrain.TYPES.DECORATION_GOAT_PLANT,
    StructureBrain.TYPES.DECORATION_GOAT_LANTERN,
    StructureBrain.TYPES.DECORATION_FLOWER_BOTTLE,
    StructureBrain.TYPES.DECORATION_MUSHROOM_CANDLE_2,
    StructureBrain.TYPES.DECORATION_LEAFY_SCULPTURE
  };
  [Key(413)]
  public int MysticRewardCount;
  [Key(414)]
  public bool DungeonBossFight;
  [Key(415)]
  public List<DataManager.LocationSeedsData> LocationSeeds = new List<DataManager.LocationSeedsData>();
  [Key(416)]
  public WeatherSystemController.WeatherType WeatherType;
  [Key(417)]
  public WeatherSystemController.WeatherStrength WeatherStrength;
  [Key(418)]
  public float WeatherStartingTime;
  [Key(419)]
  public int WeatherDuration;
  [Key(420)]
  public float TempleStudyXP;
  [Key(421)]
  public int UnlockededASermon;
  [Key(422)]
  public int CurrentDayIndex = 1;
  [Key(423)]
  public int CurrentPhaseIndex;
  [Key(424)]
  public float CurrentGameTime;
  [Key(425)]
  public int[] LastUsedSermonRitualDayIndex = new int[0];
  [Key(426)]
  public int[] ScheduledActivityIndexes = new int[5]
  {
    0,
    0,
    0,
    0,
    2
  };
  [Key(427)]
  public int OverrideScheduledActivity = -1;
  [Key(428)]
  public int[] InstantActivityIndexes = new int[1]{ 8 };
  [Key(429)]
  public bool PlayerEaten;
  [Key(430)]
  public bool PlayerEaten_COOP;
  [Key(431)]
  public ResurrectionType ResurrectionType;
  [Key(432)]
  public bool FirstTimeResurrecting = true;
  [Key(433)]
  public bool FirstTimeFertilizing = true;
  [Key(434)]
  public bool FirstTimeChop;
  [Key(435)]
  public bool FirstTimeMine;
  [Key(436)]
  public bool PlayersShagged;
  [Key(439)]
  public List<DataManager.DungeonCompletedFleeces> DungeonsCompletedWithFleeces = new List<DataManager.DungeonCompletedFleeces>();
  [Key(440)]
  public StructureBrain.Categories currentCategory;
  [Key(441)]
  public float TimeSinceLastComplaint;
  [Key(442)]
  public float TimeSinceLastQuest;
  [Key(443)]
  public int DessentingFollowerChoiceQuestionIndex;
  [Key(444)]
  public int HaroConversationIndex;
  [Key(445)]
  public int SpecialHaroConversationIndex;
  [Key(446)]
  public List<FollowerLocation> HaroSpecialEncounteredLocations = new List<FollowerLocation>();
  [Key(447)]
  public List<FollowerLocation> LeaderSpecialEncounteredLocations = new List<FollowerLocation>();
  [Key(448)]
  public bool SpokenToHaroD6;
  [Key(449)]
  public bool HaroOnboardedWinter;
  [Key(450)]
  public bool HaroConversationCompleted;
  [Key(451)]
  public bool RatauKilled;
  [Key(452)]
  public bool RatauReadLetter;
  [Key(453)]
  public bool RatauIntroWoolhaven;
  [Key(454)]
  public bool RatauStaffQuestGameConvoPlayed;
  [Key(455)]
  public bool RatauStaffQuestWonGame;
  [Key(456)]
  public bool RatauStaffQuestAliveDead;
  [Key(457)]
  public FollowerLocation CurrentFoxLocation = FollowerLocation.None;
  [Key(458)]
  public int CurrentFoxEncounter;
  [Key(459)]
  public int CurrentDLCFoxEncounter;
  [Key(460)]
  public List<FollowerLocation> FoxIntroductions = new List<FollowerLocation>();
  [Key(461)]
  public List<FollowerLocation> FoxCompleted = new List<FollowerLocation>();
  [Key(462)]
  public int PlimboStoryProgress;
  [Key(463)]
  public int RatooFishingProgress;
  [Key(464)]
  public bool RatooFishing_FISH_CRAB;
  [Key(465)]
  public bool RatooFishing_FISH_LOBSTER;
  [Key(466)]
  public bool RatooFishing_FISH_OCTOPUS;
  [Key(467)]
  public bool RatooFishing_FISH_SQUID;
  [Key(468)]
  public bool RatooNeedsRescue;
  [Key(469)]
  public bool RatooRescued;
  [Key(1322)]
  public bool BaalNeedsRescue;
  [Key(1323)]
  public bool BaalRescued;
  [Key(470)]
  public bool PlayerHasFollowers;
  [Key(471)]
  public bool ShowSpecialHaroRoom;
  [Key(472)]
  public bool ShowSpecialMidasRoom;
  [Key(473)]
  public bool ShowSpecialPlimboRoom;
  [Key(474)]
  public bool ShowSpecialKlunkoRoom;
  [Key(475)]
  public bool ShowSpecialLeaderRoom;
  [Key(476)]
  public bool ShowSpecialFishermanRoom;
  [Key(477)]
  public bool ShowSpecialSozoRoom;
  [Key(478)]
  public bool ShowSpecialBaalAndAymRoom;
  [Key(479)]
  public bool ShowSpecialLighthouseKeeperRoom;
  [Key(480)]
  public bool SozoUnlockedMushroomSkin;
  [Key(481)]
  public bool SozoNoLongerBrainwashed;
  [Key(482)]
  public int SozoMushroomRecruitedDay = int.MaxValue;
  [Key(483)]
  public int SozoAteMushroomDay = -1;
  [Key(484)]
  public int SozoMushroomCount;
  [Key(485)]
  public int DrunkDay;
  [Key(486)]
  public int DrunkIncrement;
  [Key(487)]
  public int PoemIncrement;
  [Key(488)]
  public int FishCaughtTotal;
  [Key(489)]
  public int PlayerDeathDay;
  [Key(490)]
  public int DisciplesCreated;
  [Key(491)]
  public float LastDrumCircleTime;
  [Key(492)]
  public bool HasMidasHiding;
  [Key(493)]
  public FollowerInfo MidasFollowerInfo;
  [Key(494)]
  public float TimeSinceMidasStoleGold = -1f;
  [Key(495)]
  public int MidasHiddenDay = -1;
  [Key(496)]
  public bool CompletedMidasFollowerQuest;
  [Key(497)]
  public bool GivenMidasFollowerQuest;
  [Key(498)]
  public List<InventoryItem> MidasStolenGold = new List<InventoryItem>();
  [Key(499)]
  public float LastIceSculptureBuild;
  [Key(500)]
  public float LastAnimalLoverPet;
  [Key(501)]
  public bool RatooGivenHeart;
  [Key(502)]
  public bool RatooMentionedWrongHeart;
  [Key(503)]
  public bool ShownInitialTempleDoorSeal;
  [Key(504)]
  public bool FirstFollowerSpawnInteraction = true;
  [Key(505)]
  public List<int> DecorationTypesBuilt = new List<int>();
  [Key(506)]
  public List<FollowerClothingType> UnlockedClothing = new List<FollowerClothingType>();
  [Key(507)]
  public List<FollowerClothingType> ClothingAssigned = new List<FollowerClothingType>();
  [Key(508)]
  public FollowerClothingType previouslyAssignedClothing;
  [Key(509)]
  public List<DataManager.ClothingVariant> ClothingVariants = new List<DataManager.ClothingVariant>();
  [Key(510)]
  public List<TarotCards.Card> WeaponSelectionPositions = new List<TarotCards.Card>();
  [Key(511 /*0x01FF*/)]
  public int LoreStonesRoomUpTo;
  [Key(512 /*0x0200*/)]
  public bool LoreStonesHaro;
  [Key(513)]
  public bool LoreStonesMoth;
  [Key(514)]
  public bool ShowCultFaith = true;
  [Key(515)]
  public bool ShowCultIllness = true;
  [Key(516)]
  public bool ShowCultHunger = true;
  [Key(517)]
  public bool ShowCultWarmth;
  [Key(518)]
  public bool ShowLoyaltyBars = true;
  [Key(519)]
  public bool SandboxModeEnabled;
  [Key(520)]
  public bool SpawnPubResources;
  [Key(521)]
  public bool EnteredHopRoom;
  [Key(522)]
  public bool EnteredGrapeRoom;
  [Key(523)]
  public bool EnteredCottonRoom;
  [Key(1324)]
  public bool StelleSpecialEncountered;
  [Key(1325)]
  public bool MonchMamaSpecialEncountered;
  [Key(1326)]
  public bool FishermanDLCSpecialEncountered;
  [Key(1327)]
  public bool ShowSpecialStelleRoom;
  [Key(1328)]
  public bool ShowSpecialMonchMamaRoom;
  [Key(1329)]
  public bool ShowSpecialFishermanDLCRoom;
  [Key(1330)]
  public bool InfectedDudeSpecialEncountered;
  [Key(1331)]
  public bool DungeonRancherSpecialEncountered;
  [Key(1332)]
  public bool RefinedResourcesSpecialEncountered;
  [Key(1333)]
  public bool ShowSpecialInfectedDudeRoom;
  [Key(1334)]
  public bool ShowSpecialDungeonRancherRoom;
  [Key(1335)]
  public bool ShowSpecialRefinedResourcesRoom;
  [Key(1336)]
  public bool StripperGaveOutfit;
  [Key(1337)]
  public bool OnboardedRotRoom;
  [Key(1338)]
  public bool ExecutionerRoom1Encountered;
  [Key(1339)]
  public bool ExecutionerRoom2Encountered;
  [Key(1340)]
  public bool CanShowExecutionerRoom1;
  [Key(1341)]
  public bool CanShowExecutionerRoom2;
  [Key(524)]
  public bool IntroDoor1;
  [Key(525)]
  public bool FirstDoctrineStone;
  [Key(526)]
  public bool InitialDoctrineStone = true;
  [Key(527)]
  public bool ShowHaroDoctrineStoneRoom;
  [Key(528)]
  public bool HaroIntroduceDoctrines;
  [Key(529)]
  public bool RatExplainDungeon = true;
  [Key(530)]
  public bool RatauToGiveCurseNextRun;
  [Key(531)]
  public int SozoStoryProgress = -1;
  [Key(532)]
  public bool MidasBankUnlocked;
  [Key(533)]
  public bool MidasBankIntro;
  [Key(534)]
  public bool MidasSacrificeIntro;
  [Key(535)]
  public bool MidasIntro;
  [Key(536)]
  public bool MidasDevotionIntro;
  [Key(537)]
  public bool MidasStatue;
  [Key(538)]
  public float MidasDevotionCost = 1f;
  [Key(539)]
  public int MidasDevotionLastUsed;
  [Key(540)]
  public int MidasFollowerStatueCount;
  [Key(541)]
  public bool RatauShowShrineShop;
  [Key(542)]
  public bool DecorationRoomFirstConvo;
  [Key(543)]
  public bool FirstTarot;
  [Key(544)]
  public bool Tutorial_Night;
  [Key(545)]
  public bool Tutorial_ReturnToDungeon;
  [Key(546)]
  public bool FirstTimeInDungeon;
  [Key(547)]
  public bool AllowBuilding = true;
  [Key(548)]
  public bool CookedFirstFood = true;
  [Key(549)]
  public bool Dungeon1Story1;
  [Key(550)]
  public bool Dungeon1Story2;
  [Key(551)]
  public bool FirstFollowerRescue;
  [Key(552)]
  public bool FirstDungeon1RescueRoom;
  [Key(553)]
  public bool FirstDungeon2RescueRoom;
  [Key(554)]
  public bool FirstDungeon3RescueRoom;
  [Key(555)]
  public bool FirstDungeon4RescueRoom;
  [Key(556)]
  public bool SherpaFirstConvo;
  [Key(557)]
  public bool ResourceRoom1Revealed;
  [Key(558)]
  public bool EncounteredHealingRoom;
  [Key(559)]
  public bool MinimumRandomRoomsEncountered;
  [Key(560)]
  public int MinimumRandomRoomsEncounteredAmount;
  [Key(561)]
  public bool ForneusLore;
  [Key(562)]
  public bool SozoBeforeDeath;
  [Key(563)]
  public bool SozoDead;
  [Key(564)]
  public bool SozoTakenMushroom;
  [Key(565)]
  public bool FirstTimeWeaponMarketplace;
  [Key(566)]
  public bool FirstTimeSpiderMarketplace;
  [Key(567)]
  public bool FirstTimeSeedMarketplace;
  [Key(568)]
  public bool ShowFirstDoctrineStone = true;
  [Key(569)]
  public bool RatauGiftMediumCollected;
  [Key(570)]
  public bool CompletedLighthouseCrystalQuest;
  [Key(571)]
  public bool CameFromDeathCatFight;
  [Key(572)]
  public bool OldFollowerSpoken;
  [Key(573)]
  public bool InjuredFollowerSpoken;
  [Key(574)]
  public bool CanUnlockRelics;
  [Key(575)]
  public bool FoundRelicAtHubShore;
  [Key(576)]
  public bool FoundRelicInFish;
  [Key(577)]
  public bool GivenRelicFishRiddle;
  [Key(578)]
  public bool GivenRelicLighthouseRiddle;
  [Key(579)]
  public bool ForceMarketplaceCat;
  [Key(580)]
  public bool HadInitialMatingTentInteraction;
  [Key(581)]
  public bool ShowMidasKilling;
  [Key(582)]
  public bool GivenMidasSkull;
  [Key(583)]
  public bool CompletedYngyaFightIntro;
  [Key(584)]
  public bool RecruitedRotFollower;
  [Key(1379)]
  public bool FirstRotFollowerAilmentAvoided;
  [Key(1380)]
  public float RemoveBlizzardsBeforeTimestamp;
  [Key(1381)]
  public bool DisableBlizzard1;
  [Key(1382)]
  public bool DisableBlizzard2;
  [Key(585)]
  public bool HasAcceptedPilgrimPart1;
  [Key(586)]
  public int PilgrimPart1TargetDay;
  [Key(587)]
  public bool HasAcceptedPilgrimPart2;
  [Key(588)]
  public int PilgrimPart2TargetDay = int.MaxValue;
  [Key(589)]
  public bool HasAcceptedPilgrimPart3;
  [Key(590)]
  public int PilgrimPart3TargetDay = int.MaxValue;
  [Key(591)]
  public bool IsPilgrimRescue;
  [Key(592)]
  public bool IsJalalaBag;
  [Key(593)]
  public bool FoundJalalaBag;
  [Key(594)]
  public bool GivenRinorLine;
  [Key(595)]
  public bool GivenYarlenLine;
  [Key(596)]
  public FollowerLocation PilgrimTargetLocation = FollowerLocation.Dungeon1_4;
  [Key(597)]
  public int CultLeader1_LastRun = -1;
  [Key(598)]
  public int CultLeader1_StoryPosition;
  [Key(599)]
  public int CultLeader2_LastRun = -1;
  [Key(600)]
  public int CultLeader2_StoryPosition;
  [Key(601)]
  public int CultLeader3_LastRun = -1;
  [Key(602)]
  public int CultLeader3_StoryPosition;
  [Key(603)]
  public int CultLeader4_LastRun = -1;
  [Key(604)]
  public int CultLeader4_StoryPosition;
  [Key(605)]
  public int CultLeader5_LastRun = -1;
  [Key(606)]
  public int CultLeader5_StoryPosition = -1;
  [Key(607)]
  public int CultLeader6_LastRun = -1;
  [Key(608)]
  public int CultLeader6_StoryPosition = -1;
  [Key(609)]
  public bool BeatenDungeon5;
  [Key(610)]
  public bool BeatenDungeon6;
  [Key(611)]
  public int DeathCatConversationLastRun = -999;
  [Key(612)]
  public int DeathCatStory;
  [Key(613)]
  public int DeathCatDead;
  [Key(614)]
  public int DeathCatWon;
  [Key(615)]
  public bool DeathCatBoss1;
  [Key(616)]
  public bool DeathCatBoss2;
  [Key(617)]
  public bool DeathCatBoss3;
  [Key(618)]
  public bool DeathCatBoss4;
  [Key(619)]
  public bool DeathCatRatauKilled;
  [Key(620)]
  public bool DungeonKeyRoomCompleted1;
  [Key(621)]
  public bool DungeonKeyRoomCompleted2;
  [Key(622)]
  public bool DungeonKeyRoomCompleted3;
  [Key(623)]
  public bool DungeonKeyRoomCompleted4;
  [Key(624)]
  public int LambTownLevel;
  [Key(625)]
  public int LambTownWoolGiven;
  [Key(626)]
  public bool RatOutpostIntro;
  [Key(627)]
  public bool FirstMonsterHeart;
  [Key(628)]
  public bool Rat_Tutorial_Bell;
  [Key(629)]
  public bool Goat_First_Meeting;
  [Key(630)]
  public bool Goat_Guardian_Door_Open;
  [Key(631)]
  public bool Key_Shrine_1;
  [Key(632)]
  public bool Key_Shrine_2;
  [Key(633)]
  public bool Key_Shrine_3;
  [Key(634)]
  public bool InTutorial = true;
  [Key(635)]
  public bool UnlockBaseTeleporter = true;
  [Key(636)]
  public bool Tutorial_First_Indoctoring;
  [Key(637)]
  public bool Tutorial_Second_Enter_Base = true;
  [Key(638)]
  public bool Tutorial_Rooms_Completed;
  [Key(639)]
  public bool Tutorial_Enable_Store_Resources;
  [Key(640)]
  public bool Tutorial_Completed;
  [Key(641)]
  public bool Tutorial_Mission_Board;
  [Key(642)]
  public bool Create_Tutorial_Rooms;
  [Key(643)]
  public bool RatauExplainsFollowers;
  [Key(644)]
  public bool RatauExplainsDemo;
  [Key(645)]
  public bool RatauExplainsBiome0;
  [Key(646)]
  public bool RatauExplainsBiome1;
  [Key(647)]
  public bool RatauExplainsBiome0Boss;
  [Key(648)]
  public bool RatauExplainsTeleporter;
  [Key(649)]
  public bool SozoIntro;
  [Key(650)]
  public bool SozoDecorationQuestActive;
  [Key(651)]
  public bool SozoQuestComplete;
  [Key(652)]
  public bool CollectedMenticide;
  [Key(653)]
  public bool TarotIntro;
  [Key(654)]
  public bool HasTarotBuilding = true;
  [Key(655)]
  public bool ForestOfferingRoomCompleted;
  [Key(656)]
  public bool KnucklebonesIntroCompleted;
  [Key(657)]
  public bool KnucklebonesFirstGameRatauStart = true;
  [Key(658)]
  public bool ForestChallengeRoom1Completed;
  [Key(659)]
  public bool ForestRescueWorshipper;
  [Key(660)]
  public bool GetFirstFollower;
  [Key(661)]
  public bool BeatenFirstMiniBoss;
  [Key(662)]
  public bool RatauExplainBuilding;
  [Key(663)]
  public bool PromoteFollowerExplained;
  [Key(664)]
  public bool HasMadeFirstOffering;
  [Key(665)]
  public bool BirdConvo;
  [Key(666)]
  public bool UnlockedHubShore;
  [Key(667)]
  public bool GivenFollowerGift;
  [Key(668)]
  public bool FinalBossSlowWalk = true;
  [Key(669)]
  public int HadNecklaceOnRun;
  [Key(670)]
  public bool HasPerformedPleasureRitual;
  [Key(671)]
  public bool ViolentExtremistFirstTime;
  [Key(672)]
  public List<int> FollowersPlayedKnucklebonesToday = new List<int>();
  [Key(1342)]
  public int EncounteredDungeonRancherCount;
  [Key(1348)]
  public bool DiedToWolfBoss;
  [Key(1349)]
  public bool DiedToYngyaBoss;
  [Key(1350)]
  public bool DragonIntrod;
  [Key(673)]
  public bool ShownDungeon1FinalLeaderEncounter;
  [Key(674)]
  public bool ShownDungeon2FinalLeaderEncounter;
  [Key(675)]
  public bool ShownDungeon3FinalLeaderEncounter;
  [Key(676)]
  public bool ShownDungeon4FinalLeaderEncounter;
  [Key(677)]
  public bool HaroOnbardedHarderDungeon1;
  [Key(678)]
  public bool HaroOnbardedHarderDungeon2;
  [Key(679)]
  public bool HaroOnbardedHarderDungeon3;
  [Key(680)]
  public bool HaroOnbardedHarderDungeon4;
  [Key(681)]
  public bool HaroOnbardedDungeon6;
  [Key(682)]
  public bool HaroOnbardedHarderDungeon1_PostGame;
  [Key(683)]
  public bool HaroOnbardedHarderDungeon2_PostGame;
  [Key(684)]
  public bool HaroOnbardedHarderDungeon3_PostGame;
  [Key(685)]
  public bool HaroOnbardedHarderDungeon4_PostGame;
  [Key(686)]
  public bool RevealOfferingChest;
  [Key(687)]
  public bool OnboardedOfferingChest;
  [Key(688)]
  public bool OnboardedHomeless = true;
  [Key(689)]
  public bool OnboardedHomelessAtNight;
  [Key(690)]
  public bool OnboardedEndlessMode;
  [Key(691)]
  public bool OnboardedDeadFollower;
  [Key(692)]
  public bool OnboardedBuildingHouse;
  [Key(693)]
  public bool OnboardedMakingMoreFood;
  [Key(694)]
  public bool OnboardedCleaningBase;
  [Key(695)]
  public bool OnboardedOldFollower;
  [Key(696)]
  public bool OnboardedSickFollower;
  [Key(697)]
  public bool OnboardedStarvingFollower;
  [Key(698)]
  public bool OnboardedDissenter;
  [Key(699)]
  public bool OnboardedFaithOfFlock;
  [Key(700)]
  public bool OnboardedRaiseFaith;
  [Key(701)]
  public bool OnboardedResourceYard;
  [Key(702)]
  public bool OnboardedCrisisOfFaith;
  [Key(703)]
  public bool OnboardedHalloween;
  [Key(704)]
  public bool OnboardedSermon;
  [Key(705)]
  public bool OnboardedBuildFarm;
  [Key(706)]
  public bool OnboardedRefinery;
  [Key(707)]
  public bool OnboardedCultName;
  [Key(708)]
  public bool OnboardedZombie;
  [Key(709)]
  public bool OnboardedLoyalty;
  [Key(710)]
  public bool OnboardedGodTear;
  [Key(711)]
  public bool OnboardedMysticShop;
  [Key(712)]
  public bool ForeshadowedMysticShop;
  [Key(713)]
  public bool ForeshadowedWolf;
  [Key(714)]
  public bool OnboardedLayer2;
  [Key(715)]
  public bool OnboardedRelics;
  [Key(716)]
  public bool HasMetChefShop;
  [Key(717)]
  public int CurrentOnboardingFollowerID = -1;
  [Key(718)]
  public int CurrentOnboardingFollowerType = -1;
  [Key(719)]
  public string CurrentOnboardingFollowerTerm;
  [Key(720)]
  public bool HasPerformedRitual;
  [Key(721)]
  public bool DeathCatBaalAndAymSecret;
  [Key(722)]
  public bool ShamuraBaalAndAymSecret;
  [Key(723)]
  public bool CanFindLeaderRelic;
  [Key(724)]
  public bool OnboardedDisciple;
  [Key(725)]
  public bool OnboardedPleasure;
  [Key(726)]
  public bool OnboardedFollowerPleasure;
  [Key(727)]
  public List<FollowerLocation> SecretItemsGivenToFollower = new List<FollowerLocation>();
  [Key(728)]
  public bool OnboardedDepositFollowerNPC;
  [Key(729)]
  public bool DepositFinalFollowerNPC;
  [Key(730)]
  public bool TalkedToInfectedNPC;
  [Key(731)]
  public bool CompletedInfectedNPCQuest;
  [Key(732)]
  public List<FollowerTrait.TraitType> DepositFollowerTargetTraits = new List<FollowerTrait.TraitType>();
  [Key(733)]
  public int DepositedFollowerRewardsClaimed;
  [Key(734)]
  public int DepositedWitnessEyesForRelics = -1;
  [Key(735)]
  public bool GaveLeshyHealingQuest;
  [Key(736)]
  public bool GaveHeketHealingQuest;
  [Key(737)]
  public bool GaveKallamarHealingQuest;
  [Key(738)]
  public bool GaveShamuraHealingQuest;
  [Key(739)]
  public bool LeshyHealQuestCompleted;
  [Key(740)]
  public bool HeketHealQuestCompleted;
  [Key(741)]
  public bool KallamarHealQuestCompleted;
  [Key(742)]
  public bool ShamuraHealQuestCompleted;
  [Key(743)]
  public bool LeshyHealed;
  [Key(744)]
  public bool HeketHealed;
  [Key(745)]
  public bool KallamarHealed;
  [Key(746)]
  public bool ShamuraHealed;
  [Key(747)]
  public bool HealingLeshyQuestActive;
  [Key(748)]
  public bool HealingHeketQuestActive;
  [Key(749)]
  public bool HealingKallamarQuestActive;
  [Key(750)]
  public bool HealingShamuraQuestActive;
  [Key(751)]
  public int HealingQuestDay;
  [Key(752)]
  public int LeshyHealingQuestDay;
  [Key(753)]
  public int HeketHealingQuestDay;
  [Key(754)]
  public int KallamarHealingQuestDay;
  [Key(755)]
  public int ShamuraHealingQuestDay;
  [Key(756)]
  public bool BeatenWitnessDungeon1;
  [Key(757)]
  public bool BeatenWitnessDungeon2;
  [Key(758)]
  public bool BeatenWitnessDungeon3;
  [Key(759)]
  public bool BeatenWitnessDungeon4;
  [Key(760)]
  public bool MysticKeeperBeatenLeshy;
  [Key(761)]
  public bool MysticKeeperBeatenHeket;
  [Key(762)]
  public bool MysticKeeperBeatenKallamar;
  [Key(763)]
  public bool MysticKeeperBeatenShamura;
  [Key(764)]
  public bool MysticKeeperBeatenAll;
  [Key(765)]
  public bool MysticKeeperFirstPurchase;
  [Key(1358)]
  public bool MysticKeeperBeatenYngya;
  [Key(766)]
  public bool MysticKeeperOnboardedSin;
  [Key(1359)]
  public bool SpokenToMysticKeeperWinter;
  [Key(767 /*0x02FF*/)]
  public bool ChemachOnboardedSin;
  [Key(768 /*0x0300*/)]
  public bool KlunkoOnboardedTailor1;
  [Key(769)]
  public bool KlunkoOnboardedTailor2;
  [Key(770)]
  public bool AssignedFollowersOutfits;
  [Key(771)]
  public bool BeatenPostGame;
  [Key(772)]
  public int GivenLoyaltyQuestDay = -1;
  [Key(773)]
  public int LastDaySincePlayerUpgrade = -1;
  [Key(774)]
  public int MealsCooked;
  [Key(775)]
  public int DrinksBrewed;
  [Key(776)]
  public int TalismanPiecesReceivedFromMysticShop;
  [Key(777)]
  public bool MysticShopUsed;
  [Key(778)]
  public int CrystalDoctrinesReceivedFromMysticShop;
  [Key(779)]
  public InventoryItem.ITEM_TYPE PreviousMysticShopItem;
  [Key(780)]
  public bool OnboardedCrystalDoctrine;
  [Key(781)]
  public int RanchingAnimalsAdded;
  [Key(782)]
  public int FollowersTrappedInToxicWaste;
  [Key(783)]
  public bool OnboardedWool;
  [Key(784)]
  public bool Dungeon1_1_Key;
  [Key(785)]
  public bool Dungeon1_2_Key;
  [Key(786)]
  public bool Dungeon1_3_Key;
  [Key(787)]
  public bool Dungeon1_4_Key;
  [Key(788)]
  public bool Dungeon2_1_Key;
  [Key(789)]
  public bool Dungeon2_2_Key;
  [Key(790)]
  public bool Dungeon2_3_Key;
  [Key(791)]
  public bool Dungeon2_4_Key;
  [Key(792)]
  public bool Dungeon3_1_Key;
  [Key(793)]
  public bool Dungeon3_2_Key;
  [Key(794)]
  public bool Dungeon3_3_Key;
  [Key(795)]
  public bool Dungeon3_4_Key;
  [Key(796)]
  public bool HadFirstTempleKey;
  [Key(797)]
  public int CurrentKeyPieces;
  [Key(798)]
  public bool GivenFreeDungeonFollower;
  [Key(799)]
  public bool GivenFreeDungeonGold;
  [Key(800)]
  public bool FoxMeeting_0;
  [Key(801)]
  public bool GaveFollowerToFox;
  [Key(802)]
  public bool Ritual_0;
  [Key(803)]
  public bool Ritual_1;
  [Key(804)]
  public int SnowmenCreated;
  [Key(805)]
  public bool Lighthouse_FirstConvo;
  [Key(806)]
  public bool Lighthouse_LitFirstConvo;
  [Key(807)]
  public bool Lighthouse_FireOutAgain;
  [Key(808)]
  public bool Lighthouse_QuestGiven;
  [Key(809)]
  public bool Lighthouse_QuestComplete;
  [Key(810)]
  public int LighthouseFuel;
  [Key(811)]
  public bool Lighthouse_Lit;
  [Key(812)]
  public bool ShoreFishFirstConvo;
  [Key(813)]
  public bool ShoreFishFished;
  [Key(814)]
  public bool ShoreTarotShotConvo1;
  [Key(815)]
  public bool ShoreTarotShotConvo2;
  [Key(816)]
  public bool ShoreFlowerShopConvo1;
  [Key(817)]
  public bool SozoFlowerShopConvo1;
  [Key(818)]
  public bool SozoTarotShopConvo1;
  [Key(819)]
  public EquipmentType ForcingPlayerWeaponDLC = EquipmentType.None;
  [Key(820)]
  public bool RatauFoundSkin;
  [Key(821)]
  public bool MidasFoundSkin;
  [Key(822)]
  public bool SozoFoundDecoration;
  [Key(823)]
  public int MidasTotalGoldStolen;
  [Key(824)]
  public int MidasSpecialEncounter;
  [Key(825)]
  public List<FollowerLocation> MidasSpecialEncounteredLocations = new List<FollowerLocation>();
  [Key(826)]
  public bool MidasBeaten;
  [Key(827)]
  public bool PlimboSpecialEncountered;
  [Key(828)]
  public bool KlunkoSpecialEncountered;
  [Key(829)]
  public bool FishermanSpecialEncountered;
  [Key(830)]
  public bool BaalAndAymSpecialEncounterd;
  [Key(831)]
  public bool LighthouseKeeperSpecialEncountered;
  [Key(832)]
  public bool SozoSpecialEncountered;
  [Key(833)]
  public float OpenedDoorTimestamp;
  [Key(834)]
  public bool SeedMarketPlacePostGame;
  [Key(835)]
  public bool HelobPostGame;
  [Key(836)]
  public bool HorseTown_PaidRespectToHorse;
  [Key(837)]
  public bool HorseTown_JoinCult;
  [Key(838)]
  public bool HorseTown_OpenedChest;
  [Key(839)]
  public bool BlackSoulsEnabled = true;
  [Key(840)]
  public bool PlacedRubble;
  [Key(841)]
  public bool DefeatedExecutioner;
  [Key(842)]
  public bool BeatenWolf;
  [Key(843)]
  public bool BeatenYngya;
  [Key(844)]
  public bool BeatenExecutioner;
  [Key(845)]
  public bool RevealedPostDLC;
  [Key(846)]
  public bool HadFinalYngyaRoomConvo;
  [Key(1370)]
  public bool Dungeon5Harder;
  [Key(1371)]
  public bool Dungeon6Harder;
  [Key(1372)]
  public bool EncounteredSabnock;
  [Key(1373)]
  public bool ForceSinRoom;
  [Key(1374)]
  public bool ForceHeartRoom;
  [Key(1375)]
  public bool ForceDragonRoom;
  [Key(847)]
  public bool HasYngyaMatingQuestAccepted;
  [Key(848)]
  public bool HasYngyaFirePitRitualQuestAccepted;
  [Key(849)]
  public bool HasYngyaFlowerBasketQuestAccepted;
  [Key(850)]
  public bool HasFinishedYngyaFlowerBasketQuest;
  [Key(851)]
  public bool HasAnimalFeedPoopQuest0Accepted;
  [Key(852)]
  public bool HasAnimalFeedPoopQuest1Accepted;
  [Key(853)]
  public bool HasAnimalFeedPoopQuest2Accepted;
  [Key(854)]
  public bool HasWalkPoopedAnimalQuestAccepted;
  [Key(855)]
  public bool HasAnimalFeedMeatQuest0Accepted;
  [Key(856)]
  public bool HasAnimalFeedMeatQuest1Accepted;
  [Key(857)]
  public bool HasAnimalFeedMeatQuest2Accepted;
  [Key(858)]
  public bool HasBuildGoodSnowmanQuestAccepted;
  [Key(859)]
  public bool HasLifeToTheIceRitualQuestAccepted;
  [Key(1361)]
  public bool HasPureBloodMatingQuestAccepted;
  [Key(860)]
  public int ExecutionerFollowerNoteGiverID;
  [Key(861)]
  public bool GiveExecutionerFollower;
  [Key(862)]
  public bool GivenExecutionerFollower;
  [Key(1356)]
  public bool GivenNarayanaFollower;
  [Key(863)]
  public bool ExecutionerRoomRequiresRevealing;
  [Key(864)]
  public bool ExecutionerRoomRevealed;
  [Key(865)]
  public bool ExecutionerRoomRevealedThisRun;
  [Key(866)]
  public bool ExecutionerRoomUnlocked;
  [Key(867)]
  public bool ExecutionerDefeated;
  [Key(868)]
  public bool ExecutionerPardoned;
  [Key(869)]
  public bool ExecutionerDamned;
  [Key(870)]
  public bool ExecutionerSpokenToPlimbo;
  [Key(871)]
  public bool ExecutionerReceivedPlimbosHelp;
  [Key(872)]
  public bool ExecutionerSpokenToMidas;
  [Key(873)]
  public bool ExecutionerReceivedMidasHelp;
  [Key(874)]
  public bool ExecutionerFindNoteInSilkCradle;
  [Key(875)]
  public int ExecutionerPurchases;
  [Key(876)]
  public List<int> RuinedGraveyards = new List<int>();
  [Key(877)]
  public int ExecutionerPardonedDay = -1;
  [Key(878)]
  public int ExecutionerInWoolhavenDay = -1;
  [Key(879)]
  public bool ExecutionerWoolhavenExecuted;
  [Key(880)]
  public bool ExecutionerWoolhavenSaved;
  [Key(881)]
  public bool ExecutionerGivenWeaponFragment;
  [Key(882)]
  public int WorldMapCurrentSelection;
  [Key(883)]
  public bool HasBaalSkin;
  [Key(884)]
  public bool HasReturnedBaal;
  [Key(885)]
  public bool HasAymSkin;
  [Key(886)]
  public bool HasReturnedAym;
  [Key(887)]
  public bool HasReturnedBoth;
  [Key(888)]
  public bool PlayedPostYngyaSequence;
  [Key(889)]
  public bool QuickStartActive;
  [Key(890)]
  public bool RemovedStoryMomentsActive;
  [Key(891)]
  public bool WinterModeActive;
  [Key(892)]
  public bool SurvivalModeActive;
  [Key(893)]
  public bool SurvivalModeFirstSpawn = true;
  [Key(894)]
  public bool SurvivalModeOnboarded;
  [Key(895)]
  public bool SurvivalSleepOnboarded;
  [Key(896)]
  public bool SurvivalHungerOnboarded;
  [Key(897)]
  public bool PlimboRejectedRotEye;
  [Key(898)]
  public float SurvivalMode_Hunger = 50f;
  [Key(899)]
  public float SurvivalMode_Sleep = 100f;
  [Key(900)]
  public int RedHeartsTemporarilyRemoved;
  [Key(901)]
  public bool ShownKnuckleboneTutorial;
  [Key(902)]
  public bool Knucklebones_Opponent_Ratau_Won;
  [Key(903)]
  public float FollowerKnucklebonesMatch;
  [Key(904)]
  public bool NextKnucklbonesLucky;
  [Key(905)]
  public bool FlockadeTutorialShown;
  [Key(906)]
  public bool FlockadeShepherdsTutorialShown;
  [Key(907)]
  public bool FlockadeFirstGameOpponentStarts = true;
  [Key(908)]
  public bool FlockadePlayed;
  [Key(909)]
  public bool HasNewFlockadePieces;
  [Key(910)]
  public int AnimalID;
  [Key(911)]
  public int ShopKeeperChefState;
  [Key(912)]
  public int ShopKeeperChefEnragedDay;
  [Key(913)]
  public bool Knucklebones_Opponent_0;
  [Key(914)]
  public bool Knucklebones_Opponent_0_FirstConvoRataus;
  [Key(915)]
  public bool Knucklebones_Opponent_0_Won;
  [Key(916)]
  public bool Knucklebones_Opponent_1;
  [Key(917)]
  public bool Knucklebones_Opponent_1_FirstConvoRataus;
  [Key(918)]
  public bool Knucklebones_Opponent_1_Won;
  [Key(919)]
  public bool Knucklebones_Opponent_2;
  [Key(920)]
  public bool Knucklebones_Opponent_2_FirstConvoRataus;
  [Key(921)]
  public bool Knucklebones_Opponent_2_Won;
  [Key(922)]
  public bool RefinedElectrifiedRotstone;
  [Key(923)]
  public bool DungeonLayer1;
  [Key(924)]
  public bool DungeonLayer2;
  [Key(925)]
  public bool DungeonLayer3;
  [Key(926)]
  public bool DungeonLayer4;
  [Key(927)]
  public bool DungeonLayer5;
  [Key(928)]
  public bool BeatenDungeon1;
  [Key(929)]
  public bool BeatenDungeon2;
  [Key(930)]
  public bool BeatenDungeon3;
  [Key(931)]
  public bool BeatenDungeon4;
  [Key(932)]
  public bool BeatenDeathCat;
  [Key(933)]
  public bool BeatenLeshyLayer2;
  [Key(934)]
  public bool BeatenHeketLayer2;
  [Key(935)]
  public bool BeatenKallamarLayer2;
  [Key(936)]
  public bool BeatenShamuraLayer2;
  [Key(937)]
  public bool BeatenOneDungeons;
  [Key(938)]
  public bool BeatenTwoDungeons;
  [Key(939)]
  public bool BeatenThreeDungeons;
  [Key(940)]
  public bool BeatenFourDungeons;
  [Key(941)]
  public int Dungeon1GodTears;
  [Key(942)]
  public int Dungeon2GodTears;
  [Key(943)]
  public int Dungeon3GodTears;
  [Key(944)]
  public int Dungeon4GodTears;
  [Key(945)]
  public int DungeonRunsSinceBeatingFirstDungeon;
  [Key(946)]
  public string PreviousMiniBoss;
  [Key(947)]
  public int FishCaughtInsideWhaleToday;
  [Key(948)]
  public List<DungeonSandboxManager.ProgressionSnapshot> SandboxProgression = new List<DungeonSandboxManager.ProgressionSnapshot>();
  [Key(949)]
  public bool OnboardedBossRush;
  [Key(950)]
  public bool CompletedSandbox;
  [Key(1360)]
  public bool RevealedBaseYngyaShrine;
  [Key(951)]
  public bool CanFindTarotCards = true;
  [Key(952)]
  public float LuckMultiplier = 1f;
  [Key(953)]
  public bool NextMissionarySuccessful;
  [Key(954)]
  public float EnemyModifiersChanceMultiplier = 1f;
  [Key(955)]
  public float EnemyHealthMultiplier = 1f;
  [Key(956)]
  public float ProjectileMoveSpeedMultiplier = 1f;
  [Key(957)]
  public float DodgeDistanceMultiplier = 1f;
  [Key(958)]
  public float CurseFeverMultiplier = 1f;
  [Key(959)]
  public bool SpawnPoisonOnAttack;
  [Key(960)]
  public bool EnemiesInNextRoomHaveModifiers;
  [Key(961)]
  public bool EnemiesDropGoldDuringRun;
  [Key(962)]
  public bool NoRollInNextCombatRoom;
  [Key(963)]
  public bool NoHeartDrops;
  [Key(964)]
  public bool EnemiesDropBombOnDeath;
  [Key(965)]
  public Vector2 CurrentRoomCoordinates;
  public const float SpecialAttackDamageMultiplierBaseConst = 1.25f;
  [Key(966)]
  public float SpecialAttackDamageMultiplier = 1.25f;
  [Key(967)]
  public bool NextChestGold;
  [Key(968)]
  public bool SpecialAttacksDisabled;
  [Key(969)]
  public float BossHealthMultiplier = 1f;
  [Key(970)]
  public int ResurrectRitualCount;
  [Key(971)]
  public bool NextRitualFree;
  [Key(972)]
  public bool EncounteredGambleRoom;
  [Key(973)]
  public List<int> LeaderFollowersRecruited = new List<int>();
  [Key(974)]
  public List<int> UniqueFollowersRecruited = new List<int>();
  [Key(975)]
  public int SwordLevel;
  [Key(976)]
  public int DaggerLevel;
  [Key(977)]
  public int AxeLevel;
  [Key(978)]
  public int HammerLevel;
  [Key(979)]
  public int GauntletLevel;
  [Key(980)]
  public int FireballLevel;
  [Key(981)]
  public int EnemyBlastLevel;
  [Key(982)]
  public int MegaSlashLevel;
  [Key(983)]
  public int ProjectileAOELevel;
  [Key(984)]
  public int TentaclesLevel;
  [Key(985)]
  public int VortexLevel;
  [Key(986)]
  public float LastFollowerQuestGivenTime;
  [Key(987)]
  public bool DLC_Pre_Purchase;
  [Key(988)]
  public bool DLC_Cultist_Pack;
  [Key(989)]
  public bool DLC_Heretic_Pack;
  [Key(990)]
  public bool DLC_Sinful_Pack;
  [Key(991)]
  public bool DLC_Pilgrim_Pack;
  [Key(992)]
  public bool MAJOR_DLC;
  [Key(993)]
  public bool DLC_Plush_Bonus;
  [Key(994)]
  public bool PAX_DLC;
  [Key(995)]
  public bool Twitch_Drop_1;
  [Key(996)]
  public bool Twitch_Drop_2;
  [Key(997)]
  public bool Twitch_Drop_3;
  [Key(998)]
  public bool Twitch_Drop_4;
  [Key(999)]
  public bool Twitch_Drop_5;
  [Key(1000)]
  public bool Twitch_Drop_6;
  [Key(1001)]
  public bool Twitch_Drop_7;
  [Key(1002)]
  public bool Twitch_Drop_8;
  [Key(1003)]
  public bool Twitch_Drop_9;
  [Key(1004)]
  public bool Twitch_Drop_10;
  [Key(1005)]
  public bool Twitch_Drop_11;
  [Key(1006)]
  public bool Twitch_Drop_12;
  [Key(1007)]
  public bool Twitch_Drop_13;
  [Key(1008)]
  public bool Twitch_Drop_14;
  [Key(1009)]
  public bool Twitch_Drop_15;
  [Key(1386)]
  public bool Twitch_Drop_16;
  [Key(1387)]
  public bool Twitch_Drop_17;
  [Key(1388)]
  public bool Twitch_Drop_18;
  [Key(1389)]
  public bool Twitch_Drop_19;
  [Key(1390)]
  public bool Twitch_Drop_20;
  [Key(1010)]
  public bool SupportStreamer;
  [Key(1011)]
  public int LandConvoProgress;
  [Key(1012)]
  public int LandResourcesGiven;
  [Key(1013)]
  public int LandPurchased = -1;
  [Key(1014)]
  public bool HasWeatherVane;
  [Key(1383)]
  public int GofernonRotburnProgress;
  [Key(1015)]
  public bool HasWeatherVaneUI;
  [Key(1016)]
  public List<int> ShopsBuilt = new List<int>();
  [Key(1017)]
  public bool InteractedDLCShrine;
  [Key(1018)]
  public int NPCRescueRoomsCompleted;
  [Key(1019)]
  public int RoomVariant;
  [Key(1020)]
  public float TimeSinceLastWolf;
  [Key(1357)]
  public int MapLockCountToUnlock = -1;
  [Key(1021)]
  public List<StructuresData.Ranchable_Animal> BreakingOutAnimals = new List<StructuresData.Ranchable_Animal>();
  [Key(1022)]
  public List<InventoryItem.ITEM_TYPE> DisoveredAnimals = new List<InventoryItem.ITEM_TYPE>();
  [Key(1023 /*0x03FF*/)]
  public StructuresData.Ranchable_Animal[] FollowingPlayerAnimals = new StructuresData.Ranchable_Animal[2];
  [XmlIgnore]
  [IgnoreMember]
  [NonSerialized]
  public List<StructuresData.Ranchable_Animal> DeadAnimalsTemporaryList = new List<StructuresData.Ranchable_Animal>();
  [Key(1024 /*0x0400*/)]
  public List<DataManager.Offering> BlizzardOfferingRequirements = new List<DataManager.Offering>();
  [Key(1025)]
  public List<DataManager.Offering> BlizzardOfferingsGiven;
  [Key(1026)]
  public List<InventoryItem> SacrificeTableInventory = new List<InventoryItem>();
  [Key(1027)]
  public bool BlizzardMonsterActive;
  [Key(1028)]
  public int BlizzardSnowmenGiven;
  [Key(1029)]
  public bool CompletedOfferingThisBlizzard;
  [Key(1030)]
  public bool CompletedBlizzardSecret;
  [Key(1031)]
  public int BlizzardOfferingsCompleted;
  [Key(1032)]
  public bool ForceDammedRelic;
  [Key(1033)]
  public bool ForceBlessedRelic;
  [Key(1034)]
  public bool FirstRelic = true;
  [Key(1035)]
  public bool EndlessModeOnCooldown;
  [Key(1036)]
  public bool EndlessModeSinOncooldown;
  [Key(1037)]
  public float TimeSinceLastStolenFromFollowers = -1f;
  [Key(1038)]
  public float TimeSinceLastFollowerFight = -1f;
  [Key(1039)]
  public float TimeSinceLastFollowerEaten = -1f;
  [Key(1040)]
  public float TimeSinceLastFollowerBump = -1f;
  [Key(1041)]
  public float TimeSinceLastMissionaryFollowerEncounter = -1f;
  [Key(1042)]
  public int DaySinceLastSpecialPoop = 10;
  [Key(1043)]
  public bool followerRecruitWaiting;
  [Key(1044)]
  public int weddingsPerformed;
  [Key(1045)]
  public bool ForceClothesShop;
  [Key(1046)]
  public bool UnlockedTailor;
  [Key(1047)]
  public bool RevealedTailor;
  [Key(1048)]
  public bool TookBopToTailor;
  [Key(1049)]
  public bool LeftBopAtTailor;
  [Key(1050)]
  public bool BerithTalkedWithBop;
  [Key(1051)]
  public int itemsCleaned;
  public static int itemsCleanedNeeded = 200;
  [Key(1052)]
  public int outfitsCreated;
  [Key(1053)]
  public int drinksCreated;
  [Key(1054)]
  public int eggsCracked;
  [Key(1055)]
  public int eggsHatched;
  [Key(1056)]
  public int EggsProduced;
  [Key(1057)]
  public bool HasProducedChosenOne;
  [Key(1058)]
  public bool HasGivenPedigreeFollower;
  [Key(1059)]
  public int pleasurePointsRedeemed;
  [Key(1060)]
  public List<int> PreviousSinPointFollowers = new List<int>();
  public static int MAX_PREV_SIN = 6;
  [Key(1061)]
  public bool pleasurePointsRedeemedFollowerSpoken;
  [Key(1062)]
  public bool pleasurePointsRedeemedTempleFollowerSpoken;
  [Key(1063)]
  public int damnedConversation;
  [Key(1064)]
  public int damnedFightConversation;
  [Key(1065)]
  public bool ForceGoldenEgg;
  [Key(1066)]
  public bool ForceSpecialPoo;
  [Key(1067)]
  public bool ForceAbomination;
  [Key(1068)]
  public bool clickedDLCAd;
  [Key(1069)]
  public bool RevealedDLCMapDoor;
  [Key(1070)]
  public bool RevealedDLCMapHeart;
  [Key(1345)]
  public bool EnabledDLCMapHeart;
  [Key(1346)]
  public bool FinalDLCMap;
  [Key(1391)]
  public bool RevealedWolfNode;
  [Key(1071)]
  public int YngyaHeartRoomEncounters;
  [Key(1392)]
  public RelicType PreviousRelic;
  [Key(1393)]
  public bool FirstDungeon6RescueRoom;
  [Key(1394)]
  public int WoolhavenSkinsPurchased;
  [Key(1072)]
  public bool DeathCatBeaten;
  [Key(1073)]
  public bool HasEncounteredTarot;
  [Key(1074)]
  public List<InventoryItem.ITEM_TYPE> RecentRecipes = new List<InventoryItem.ITEM_TYPE>();
  [Key(1075)]
  public List<InventoryItem.ITEM_TYPE> RecipesDiscovered = new List<InventoryItem.ITEM_TYPE>();
  [Key(1076)]
  public List<int> LoreUnlocked = new List<int>();
  [Key(1077)]
  public bool LoreStonesOnboarded;
  [Key(1078)]
  public bool LoreOnboarded;
  [Key(1079)]
  public bool UpgradeTreeMenuDLCAlert;
  [Key(1080)]
  public List<DataManager.DepositedFollower> DepositedFollowers = new List<DataManager.DepositedFollower>();
  [Key(1081)]
  public float playerDamageDealt;
  [Key(1082)]
  public int PlayerScaleModifier = 1;
  [Key(1083)]
  public bool ChefShopDoublePrices;
  [Key(1084)]
  public int FollowerShopUses;
  [Key(1085)]
  public List<DataManager.WoolhavenFlowerPot> WoolhavenFlowerPots = new List<DataManager.WoolhavenFlowerPot>();
  [Key(1086)]
  public List<int> FullWoolhavenFlowerPots = new List<int>();
  [Key(1087)]
  public int sacrificesCompleted;
  [Key(1088)]
  public List<InventoryItem.ITEM_TYPE> FoundItems = new List<InventoryItem.ITEM_TYPE>();
  [Key(1089)]
  public bool TakenBossDamage;
  [Key(1090)]
  public int PoopMealsCreated;
  [Key(1091)]
  public bool PrayedAtCrownShrine;
  [Key(1092)]
  public bool ShellsGifted_0;
  [Key(1093)]
  public bool ShellsGifted_1;
  [Key(1094)]
  public bool ShellsGifted_2;
  [Key(1095)]
  public bool ShellsGifted_3;
  [Key(1096)]
  public bool ShellsGifted_4;
  [Key(1355)]
  public int LastAnimalToStarveDay = -1;
  [Key(1097)]
  public int LostSoulsBark;
  [IgnoreMember]
  [NonSerialized]
  public Dictionary<FollowerClothingType, int> _revealingOutfits = new Dictionary<FollowerClothingType, int>();
  [Key(1098)]
  public int DateLastScreenshot = -1;
  [Key(1099)]
  public float PlayerDamageDealtThisRun;
  [Key(1100)]
  public float PlayerDamageReceivedThisRun;
  [Key(1101)]
  public float playerDamageReceived;
  [Key(1102)]
  public bool Options_ScreenShake = true;
  public static System.Random RandomSeed = new System.Random();
  public static bool UseDataManagerSeed = false;
  [Key(1384)]
  public List<int> LastDungeonSeeds = new List<int>();
  [Key(1385)]
  public bool FishermanWinterConvo;
  [Key(1103)]
  public bool PlayerIsASpirit;
  [Key(1104)]
  public bool BridgeFixed;
  [Key(1105)]
  public bool BuildingTome;
  [Key(1106)]
  public bool BeenToDungeon;
  [Key(1107)]
  public int FollowerID;
  [Key(1108)]
  public int ObjectiveGroupID;
  [Key(1109)]
  public List<FollowerInfo> Followers = new List<FollowerInfo>();
  [Key(1110)]
  public List<FollowerInfo> Followers_Recruit = new List<FollowerInfo>();
  [Key(1111)]
  public List<FollowerInfo> Followers_Dead = new List<FollowerInfo>();
  [Key(1112)]
  public List<int> Followers_Dead_IDs = new List<int>();
  [Key(1113)]
  public List<FollowerInfo> Followers_Possessed = new List<FollowerInfo>();
  [Key(1114)]
  public List<FollowerInfo> Followers_Dissented = new List<FollowerInfo>();
  [Key(1115)]
  public int EncounteredPossessedEnemyRun;
  [Key(1116)]
  public int StructureID;
  [Key(1117)]
  public List<StructuresData> BaseStructures = new List<StructuresData>();
  [Key(1118)]
  public List<StructuresData> MajorDLCCachedBaseStructures = new List<StructuresData>();
  [Key(1119)]
  public List<StructuresData> HubStructures = new List<StructuresData>();
  [Key(1120)]
  public List<StructuresData> HubShoreStructures = new List<StructuresData>();
  [Key(1121)]
  public List<StructuresData> Hub1_MainStructures = new List<StructuresData>();
  [Key(1122)]
  public List<StructuresData> Hub1_BerriesStructures = new List<StructuresData>();
  [Key(1123)]
  public List<StructuresData> Hub1_ForestStructures = new List<StructuresData>();
  [Key(1124)]
  public List<StructuresData> Hub1_RatauInsideStructures = new List<StructuresData>();
  [Key(1125)]
  public List<StructuresData> Hub1_RatauOutsideStructures = new List<StructuresData>();
  [Key(1126)]
  public List<StructuresData> Hub1_SozoStructures = new List<StructuresData>();
  [Key(1127)]
  public List<StructuresData> Hub1_SwampStructures = new List<StructuresData>();
  [Key(1128)]
  public List<StructuresData> WoolhavenStructures = new List<StructuresData>();
  [Key(1129)]
  public List<StructuresData> Dungeon_Logs1Structures = new List<StructuresData>();
  [Key(1130)]
  public List<StructuresData> Dungeon_Logs2Structures = new List<StructuresData>();
  [Key(1131)]
  public List<StructuresData> Dungeon_Logs3Structures = new List<StructuresData>();
  [Key(1132)]
  public List<StructuresData> Dungeon_Food1Structures = new List<StructuresData>();
  [Key(1133)]
  public List<StructuresData> Dungeon_Food2Structures = new List<StructuresData>();
  [Key(1134)]
  public List<StructuresData> Dungeon_Food3Structures = new List<StructuresData>();
  [Key(1135)]
  public List<StructuresData> Dungeon_Stone1Structures = new List<StructuresData>();
  [Key(1136)]
  public List<StructuresData> Dungeon_Stone2Structures = new List<StructuresData>();
  [Key(1137)]
  public List<StructuresData> Dungeon_Stone3Structures = new List<StructuresData>();
  [Key(1138)]
  public List<int> Followers_TraitManipulating_IDs = new List<int>();
  [Key(1139)]
  public List<int> Followers_Imprisoned_IDs = new List<int>();
  [Key(1140)]
  public List<int> Followers_Elderly_IDs = new List<int>();
  [Key(1141)]
  public List<int> Followers_OnMissionary_IDs = new List<int>();
  [Key(1142)]
  public List<int> Followers_LeftInTheDungeon_IDs = new List<int>();
  [Key(1143)]
  public List<int> Followers_Transitioning_IDs = new List<int>();
  [Key(1144)]
  public List<int> Followers_Demons_IDs = new List<int>();
  [Key(1145)]
  public List<int> Followers_Demons_Types = new List<int>();
  [Key(1146)]
  public float MatingCompletedTimestamp = -1f;
  [Key(1147)]
  public List<SeasonalEventType> ActiveSeasonalEvents = new List<SeasonalEventType>();
  [Key(1148)]
  public List<Vector2> CustomisedFleeceOptions = new List<Vector2>();
  [Key(1149)]
  public List<StructureBrain.TYPES> RevealedStructures = new List<StructureBrain.TYPES>();
  [Key(1150)]
  public List<DayObject> DayList = new List<DayObject>();
  [Key(1151)]
  public DayObject CurrentDay;
  [Key(1152)]
  public List<string> TrackedObjectiveGroupIDs = new List<string>();
  [Key(1153)]
  public List<ObjectivesData> Objectives = new List<ObjectivesData>();
  [Key(1154)]
  public List<ObjectivesData> CompletedObjectives = new List<ObjectivesData>();
  [Key(1155)]
  public List<ObjectivesData> FailedObjectives = new List<ObjectivesData>();
  [Key(1156)]
  public List<ObjectivesData> DungeonObjectives = new List<ObjectivesData>();
  [Key(1157)]
  public List<StoryData> StoryObjectives = new List<StoryData>();
  [Key(1158)]
  public List<ObjectivesDataFinalized> CompletedObjectivesHistory = new List<ObjectivesDataFinalized>();
  [Key(1159)]
  public List<ObjectivesDataFinalized> FailedObjectivesHistory = new List<ObjectivesDataFinalized>();
  [Key(1160)]
  public List<DataManager.QuestHistoryData> CompletedQuestsHistorys = new List<DataManager.QuestHistoryData>();
  [Key(1161)]
  public InventoryItem.ITEM_TYPE SimpleInventoryItem;
  [Key(1162)]
  public List<InventoryItem> items = new List<InventoryItem>();
  [Key(1163)]
  public int IngredientsCapacityLevel;
  public static List<int> IngredientsCapacity = new List<int>()
  {
    150,
    50,
    100
  };
  [Key(1164)]
  public int FoodCapacityLevel;
  public static List<int> FoodCapacity = new List<int>()
  {
    150,
    50,
    100
  };
  [Key(1165)]
  public int LogCapacityLevel;
  public static List<int> LogCapacity = new List<int>()
  {
    150,
    50,
    100
  };
  [Key(1166)]
  public int StoneCapacityLevel;
  public static List<int> StoneCapacity = new List<int>()
  {
    150,
    50,
    100
  };
  [XmlIgnore]
  [IgnoreMember]
  [NonSerialized]
  public static Action<string> OnSkinUnlocked;
  [Key(1167)]
  public List<string> FollowerSkinsUnlocked = new List<string>()
  {
    "Cat",
    "Dog",
    "Pig",
    "Deer",
    "Fox"
  };
  [XmlIgnore]
  [IgnoreMember]
  [NonSerialized]
  public List<string> FollowerSkinsBlacklist = new List<string>()
  {
    "Axolotl",
    "Starfish",
    "Fish",
    "Shrew",
    "Poop",
    "Crab",
    "Snail",
    "MassiveMonster",
    "Nightwolf",
    "Butterfly",
    "Squirrel",
    "CultLeader 1",
    "CultLeader 2",
    "CultLeader 3",
    "CultLeader 4",
    "Boss Baal",
    "Boss Aym",
    "Shrimp",
    "Koala",
    "Owl",
    "Volvy",
    "Snake",
    "Seal",
    "Lemur",
    "Caterpillar",
    "Worm",
    "Sozo",
    "Abomination",
    "Mushroom",
    "CultLeader 1 Healed",
    "CultLeader 2 Healed",
    "CultLeader 3 Healed",
    "CultLeader 4 Healed",
    "Bee",
    "Tapir",
    "Turtle",
    "Monkey",
    "Narwal",
    "Cthulhu",
    "Webber",
    "TwitchMouse",
    "TwitchCat",
    "TwitchPoggers",
    "TwitchDog",
    "TwitchDogAlt",
    "Lion",
    "Penguin",
    "Kiwi",
    "Pelican",
    "DeerSkull",
    "BatDemon",
    "Crow",
    "Moose",
    "Gorilla",
    "Mosquito",
    "Goldfish",
    "Possum",
    "Hammerhead",
    "Llama",
    "Tiger",
    "Sphynx",
    "LadyBug",
    "Panda",
    "Skunk",
    "Anteater",
    "Enchida",
    "Camel",
    "StarBunny",
    "Jalala",
    "Rinor",
    "DogTeddy",
    "PalworldOne",
    "PalworldTwo",
    "PalworldThree",
    "PalworldFour",
    "PalworldFive",
    "Midas",
    "Boss Lamb 1",
    "Boss Lamb 2",
    "Boss Lamb 3",
    "Boss Lamb 4",
    "Orca",
    "IceGore",
    "Mole",
    "Lamb",
    "Boss Dog 1",
    "Boss Dog 2",
    "Boss Dog 3",
    "Boss Dog 4",
    "SnowLeopard",
    "Boss Lamb 1",
    "Boss Lamb 2",
    "Boss Lamb 3",
    "Boss Lamb 4",
    "Yngya",
    "ChosenChild",
    "Executioner",
    "Narayana",
    "Dragon",
    "Snowman/Good_1",
    "Snowman/Good_2",
    "Snowman/Good_3",
    "Fly",
    "Bug",
    "BugTwo",
    "BugThree",
    "BugFour",
    "BugFive",
    "Chameleon",
    "Dragon",
    "DragonTwo",
    "DragonThree",
    "DragonFour",
    "DragonFive",
    "Wombat",
    "Apple_1",
    "Apple_2",
    "Apple_3",
    "Apple_4",
    "Apple_5",
    "Orangutan",
    "Robin",
    "Trout",
    "Beaver",
    "Lobster",
    "Anglerfish",
    "SeaButterfly",
    "Jellyfish",
    "Leech",
    "LizardTongue",
    "Lamb"
  };
  [XmlIgnore]
  [IgnoreMember]
  [NonSerialized]
  public List<string> DLCSkins = new List<string>()
  {
    "TwitchPoggers",
    "TwitchDog",
    "TwitchDogAlt",
    "Lion",
    "Penguin",
    "Kiwi",
    "Pelican",
    "TwitchMouse",
    "TwitchCat",
    "Cthulhu",
    "Bee",
    "Tapir",
    "Turtle",
    "Monkey",
    "Narwal",
    "Moose",
    "Gorilla",
    "Mosquito",
    "Goldfish",
    "Possum",
    "Hammerhead",
    "Llama",
    "Tiger",
    "Sphynx",
    "LadyBug",
    "Panda",
    "Skunk",
    "Anteater",
    "Enchida",
    "Camel",
    "DogTeddy",
    "Anglerfish",
    "SeaButterfly",
    "Jellyfish",
    "Leech",
    "LizardTongue"
  };
  [XmlIgnore]
  [IgnoreMember]
  [NonSerialized]
  public List<string> MajorDLCSkins = new List<string>()
  {
    "Midas",
    "Boss Lamb 1",
    "Boss Lamb 2",
    "Boss Lamb 3",
    "Boss Lamb 4",
    "Orca",
    "IceGore",
    "Mole",
    "Bug",
    "Lamb",
    "PolarBear",
    "Reindeer",
    "Vulture",
    "SnowLeopard",
    "Leopard",
    "Pudding",
    "Midas",
    "Chameleon",
    "Boss Lamb 1",
    "Boss Lamb 2",
    "Boss Lamb 3",
    "Boss Lamb 4",
    "Orca",
    "IceGore",
    "Mole",
    "Lamb",
    "Boss Dog 1",
    "Boss Dog 2",
    "Boss Dog 3",
    "Boss Dog 4",
    "SnowLeopard",
    "Boss Lamb 1",
    "Boss Lamb 2",
    "Boss Lamb 3",
    "Boss Lamb 4",
    "Yngya",
    "Fly",
    "Bug",
    "BugTwo",
    "BugThree",
    "BugFour",
    "BugFive",
    "Chameleon",
    "Dragon",
    "DragonTwo",
    "DragonThree",
    "DragonFour",
    "DragonFive",
    "ChosenChild",
    "Executioner",
    "Wombat",
    "Jackalope",
    "Mammoth",
    "Armadillo",
    "Ibex",
    "Moth",
    "Lizard",
    "Maggot",
    "NakedMoleRat",
    "Hyena",
    "SnowMonkey",
    "Beluga"
  };
  [XmlIgnore]
  [IgnoreMember]
  [NonSerialized]
  public string[] SpecialEventSkins = new string[6]
  {
    "DeerSkull",
    "BatDemon",
    "Crow",
    "StarBunny",
    "Webber",
    "Volvy"
  };
  [XmlIgnore]
  [IgnoreMember]
  [NonSerialized]
  public string[] PalworldSkins = new string[5]
  {
    "PalworldOne",
    "PalworldTwo",
    "PalworldThree",
    "PalworldFour",
    "PalworldFive"
  };
  [XmlIgnore]
  [IgnoreMember]
  [NonSerialized]
  public List<string> AvailableBeforeUnlockSkins = new List<string>()
  {
    "Bug",
    "Mushroom"
  };
  [Key(1168)]
  public List<StructureEffect> StructureEffects = new List<StructureEffect>();
  [Key(1169)]
  public List<string> KilledBosses = new List<string>();
  public static List<InventoryItem.ITEM_TYPE> AllNecklaces = new List<InventoryItem.ITEM_TYPE>()
  {
    InventoryItem.ITEM_TYPE.Necklace_1,
    InventoryItem.ITEM_TYPE.Necklace_2,
    InventoryItem.ITEM_TYPE.Necklace_3,
    InventoryItem.ITEM_TYPE.Necklace_4,
    InventoryItem.ITEM_TYPE.Necklace_5,
    InventoryItem.ITEM_TYPE.Necklace_Missionary,
    InventoryItem.ITEM_TYPE.Necklace_Light,
    InventoryItem.ITEM_TYPE.Necklace_Loyalty,
    InventoryItem.ITEM_TYPE.Necklace_Demonic,
    InventoryItem.ITEM_TYPE.Necklace_Dark,
    InventoryItem.ITEM_TYPE.Necklace_Gold_Skull,
    InventoryItem.ITEM_TYPE.Necklace_Deaths_Door,
    InventoryItem.ITEM_TYPE.Necklace_Winter,
    InventoryItem.ITEM_TYPE.Necklace_Frozen,
    InventoryItem.ITEM_TYPE.Necklace_Weird,
    InventoryItem.ITEM_TYPE.Necklace_Targeted
  };
  public static List<InventoryItem.ITEM_TYPE> AllGifts = new List<InventoryItem.ITEM_TYPE>()
  {
    InventoryItem.ITEM_TYPE.GIFT_SMALL,
    InventoryItem.ITEM_TYPE.GIFT_MEDIUM
  };
  public static Action<EquipmentType> OnWeaponUnlocked;
  [Key(1170)]
  public List<EquipmentType> WeaponPool = new List<EquipmentType>();
  [Key(1171)]
  public string LegendarySwordCustomName;
  [Key(1172)]
  public string LegendaryAxeCustomName;
  [Key(1173)]
  public string LegendaryDaggerCustomName;
  [Key(1174)]
  public string LegendaryHammerCustomName;
  [Key(1175)]
  public string LegendaryGauntletCustomName;
  [Key(1176)]
  public string LegendaryBlunderbussCustomName;
  [Key(1177)]
  public string LegendaryChainCustomName;
  [Key(1178)]
  public List<EquipmentType> LegendaryWeaponsJobBoardCompleted = new List<EquipmentType>();
  [Key(1179)]
  public int CurrentRunWeaponLevel;
  [Key(1180)]
  public EquipmentType ForcedStartingWeapon = EquipmentType.None;
  [Key(1181)]
  public int CurrentRunCurseLevel;
  [Key(1182)]
  public EquipmentType ForcedStartingCurse = EquipmentType.None;
  [Key(1183)]
  public List<RelicType> SpawnedRelicsThisRun = new List<RelicType>();
  public const int WEAPON_LEVEL_DIVIDER = 3;
  public static List<float> WeaponDurabilityChance = new List<float>()
  {
    80f,
    90f,
    98f
  };
  public static List<float> WeaponDurabilityLevels = new List<float>()
  {
    75f,
    50f,
    85f,
    60f,
    100f
  };
  public static Action<TarotCards.Card> OnCurseUnlocked;
  [Key(1184)]
  public List<EquipmentType> CursePool = new List<EquipmentType>();
  public static List<TarotCards.Card> TarotCardBlacklist = new List<TarotCards.Card>()
  {
    TarotCards.Card.Count,
    TarotCards.Card.Vortex,
    TarotCards.Card.Tripleshot,
    TarotCards.Card.MegaSlash,
    TarotCards.Card.Tentacles,
    TarotCards.Card.Fireball,
    TarotCards.Card.EnemyBlast,
    TarotCards.Card.ProjectileAOE,
    TarotCards.Card.Sword,
    TarotCards.Card.Dagger,
    TarotCards.Card.Axe,
    TarotCards.Card.Blunderbuss,
    TarotCards.Card.Hammer,
    TarotCards.Card.InvincibleWhileHealing
  };
  public static List<TarotCards.Card> AllTrinkets = new List<TarotCards.Card>()
  {
    TarotCards.Card.Hearts1,
    TarotCards.Card.Hearts2,
    TarotCards.Card.Hearts3,
    TarotCards.Card.Moon,
    TarotCards.Card.NaturesGift,
    TarotCards.Card.Lovers1,
    TarotCards.Card.Lovers2,
    TarotCards.Card.Sun,
    TarotCards.Card.EyeOfWeakness,
    TarotCards.Card.Telescope,
    TarotCards.Card.HandsOfRage,
    TarotCards.Card.Skull,
    TarotCards.Card.DiseasedHeart,
    TarotCards.Card.Spider,
    TarotCards.Card.AttackRate,
    TarotCards.Card.IncreasedDamage,
    TarotCards.Card.MovementSpeed,
    TarotCards.Card.IncreaseBlackSoulsDrop,
    TarotCards.Card.HealChance,
    TarotCards.Card.NegateDamageChance,
    TarotCards.Card.BombOnRoll,
    TarotCards.Card.GoopOnDamaged,
    TarotCards.Card.GoopOnRoll,
    TarotCards.Card.PoisonImmune,
    TarotCards.Card.DamageOnRoll,
    TarotCards.Card.AmmoEfficient,
    TarotCards.Card.BlackSoulAutoRecharge,
    TarotCards.Card.BlackSoulOnDamage,
    TarotCards.Card.NeptunesCurse,
    TarotCards.Card.HealTwiceAmount,
    TarotCards.Card.DeathsDoor,
    TarotCards.Card.TheDeal,
    TarotCards.Card.RabbitFoot,
    TarotCards.Card.Potion,
    TarotCards.Card.GiftFromBelow,
    TarotCards.Card.Arrows,
    TarotCards.Card.ImmuneToTraps,
    TarotCards.Card.BombOnDamaged,
    TarotCards.Card.WalkThroughBlocks,
    TarotCards.Card.InvincibilityPerRoom,
    TarotCards.Card.TentacleOnDamaged,
    TarotCards.Card.MoreRelics,
    TarotCards.Card.DecreaseRelicCharge,
    TarotCards.Card.AdventureMapFreedom,
    TarotCards.Card.Recycle,
    TarotCards.Card.StrikeBack,
    TarotCards.Card.SurpriseAttack,
    TarotCards.Card.BossHeal,
    TarotCards.Card.Sin,
    TarotCards.Card.ExtraMove,
    TarotCards.Card.ShuffleNode,
    TarotCards.Card.CoopBetterTogether,
    TarotCards.Card.CoopBetterApart,
    TarotCards.Card.CoopBonded,
    TarotCards.Card.CoopGoodTiming,
    TarotCards.Card.CoopExplosive,
    TarotCards.Card.CorruptedBombsAndHealth,
    TarotCards.Card.CorruptedHeavy,
    TarotCards.Card.CorruptedTradeOff,
    TarotCards.Card.CorruptedBlackHeartForRelic,
    TarotCards.Card.CorruptedHealForRelic,
    TarotCards.Card.CorruptedFullCorruption,
    TarotCards.Card.CorruptedPoisonCoins,
    TarotCards.Card.CorruptedRelicCharge,
    TarotCards.Card.CorruptedGoopyTrail,
    TarotCards.Card.NoCorruption,
    TarotCards.Card.FrostHeart,
    TarotCards.Card.FlameHeart,
    TarotCards.Card.EasyMoney,
    TarotCards.Card.HighRoller,
    TarotCards.Card.FrostedEnemies,
    TarotCards.Card.LastChance,
    TarotCards.Card.Joker,
    TarotCards.Card.MutatedResurrectFullHealth,
    TarotCards.Card.MutatedDropRotburn,
    TarotCards.Card.MutatedFreezeOnHit,
    TarotCards.Card.MutatedInvincibility,
    TarotCards.Card.MutatedNegateHit,
    TarotCards.Card.MutatedSpawnRotDemons,
    TarotCards.Card.EmptyFervourCritical,
    TarotCards.Card.KillEnemiesOnResurrect,
    TarotCards.Card.RoomEnterCritter,
    TarotCards.Card.HitKillEnemy,
    TarotCards.Card.SummonGhost,
    TarotCards.Card.HeartTarotDrawn
  };
  [Key(1185)]
  public List<TarotCards.Card> PlayerFoundTrinkets = new List<TarotCards.Card>();
  [Key(1186)]
  public List<CrownAbilities> CrownAbilitiesUnlocked = new List<CrownAbilities>();
  [Key(1187)]
  public List<RelicType> PlayerFoundRelics = new List<RelicType>();
  public static List<BluePrint.BluePrintType> AllBluePrints = new List<BluePrint.BluePrintType>()
  {
    BluePrint.BluePrintType.TREE,
    BluePrint.BluePrintType.STONE,
    BluePrint.BluePrintType.PATH_DIRT
  };
  [Key(1188)]
  public List<BluePrint> PlayerBluePrints = new List<BluePrint>();
  [Key(1189)]
  public List<FlockadePieceType> PlayerFoundPieces = new List<FlockadePieceType>()
  {
    FlockadePieceType.Sword,
    FlockadePieceType.Shield,
    FlockadePieceType.Scribe,
    FlockadePieceType.ScribeRisen,
    FlockadePieceType.SwordRisen,
    FlockadePieceType.ShieldRisen,
    FlockadePieceType.ScribeFallen,
    FlockadePieceType.ShieldFallen,
    FlockadePieceType.SwordFallen
  };
  [Key(1190)]
  public List<InventoryItem.ITEM_TYPE> FishCaught = new List<InventoryItem.ITEM_TYPE>();
  [Key(1191)]
  public List<MissionManager.Mission> ActiveMissions = new List<MissionManager.Mission>();
  [Key(1192)]
  public List<MissionManager.Mission> AvailableMissions = new List<MissionManager.Mission>();
  [Key(1193)]
  public float NewMissionDayTimestamp = -1f;
  [Key(1194)]
  public int LastGoldenMissionDay = -1;
  [Key(1195)]
  public bool MissionShrineUnlocked;
  [Key(1196)]
  public List<BuyEntry> ItemsForSale = new List<BuyEntry>();
  [Key(1197)]
  public List<ShopLocationTracker> Shops = new List<ShopLocationTracker>();
  [Key(1198)]
  public int LastDayUsedFollowerShop = -1;
  [Key(1199)]
  public FollowerInfo FollowerForSale;
  [Key(1200)]
  public MidasDonation midasDonation;
  [Key(1201)]
  public int LastDayUsedBank = -1;
  [Key(1202)]
  public JellyFishInvestment Investment;
  [Key(1203)]
  public List<TraderTracker> Traders = new List<TraderTracker>();
  [Key(1204)]
  public int LastDayUsedFlockadeHint = -1;
  [Key(1205)]
  public FlockadePieceType HintedPieceType;
  [Key(1206)]
  public List<ShrineUsageInfo> ShrineTimerInfo = new List<ShrineUsageInfo>();
  public static List<int> RedHeartShrineCosts = new List<int>()
  {
    50,
    250,
    500,
    1000
  };
  [Key(1207)]
  public int RedHeartShrineLevel;
  [Key(1208)]
  public int ShrineHeart;
  [Key(1209)]
  public int ShrineCurses;
  [Key(1210)]
  public int ShrineVoodo;
  [Key(1211)]
  public int ShrineAstrology;
  [Key(1212)]
  public List<Lamb.UI.ItemSelector.Category> ItemSelectorCategories = new List<Lamb.UI.ItemSelector.Category>();
  [Key(1213)]
  public List<InventoryItem> itemsDungeon = new List<InventoryItem>();
  [Key(1376)]
  public List<InventoryItem> GivenUpWolfFood = new List<InventoryItem>();
  [Key(1377)]
  public bool GivenUpHeartToWolf;
  [Key(1378)]
  public int WoolhavenDecorationCouunt;
  [Key(1214)]
  public float DUNGEON_TIME;
  [Key(1215)]
  public float PLAYER_RUN_DAMAGE_LEVEL;
  [Key(1216)]
  public int PLAYER_HEARTS_LEVEL;
  [Key(1217)]
  public int PLAYER_DAMAGE_LEVEL;
  [Key(1218)]
  public float PLAYER_HEALTH = 6f;
  [Key(1219)]
  public float PLAYER_TOTAL_HEALTH = 6f;
  [Key(1220)]
  public float PLAYER_BLUE_HEARTS;
  [Key(1221)]
  public float PLAYER_BLACK_HEARTS;
  [Key(1222)]
  public float PLAYER_FIRE_HEARTS;
  [Key(1223)]
  public float PLAYER_ICE_HEARTS;
  [Key(1224)]
  public float PLAYER_REMOVED_HEARTS;
  [Key(1225)]
  public float PLAYER_SPIRIT_HEARTS;
  [Key(1226)]
  public float PLAYER_SPIRIT_TOTAL_HEARTS;
  [Key(1227)]
  public bool UnlockedCoopRelicsAndTarots;
  [Key(1228)]
  public bool UnlockedCoopTarots;
  [Key(1229)]
  public bool UnlockedCoopRelics;
  [Key(1230)]
  public bool UnlockedCorruptedRelicsAndTarots;
  [Key(1231)]
  public float COOP_PLAYER_BLUE_HEARTS;
  [Key(1232)]
  public float COOP_PLAYER_BLACK_HEARTS;
  [Key(1233)]
  public float COOP_PLAYER_FIRE_HEARTS;
  [Key(1234)]
  public float COOP_PLAYER_ICE_HEARTS;
  [Key(1235)]
  public float COOP_PLAYER_REMOVED_HEARTS;
  [Key(1236)]
  public float PLAYER_SPECIAL_CHARGE;
  [Key(1237)]
  public float PLAYER_SPECIAL_AMMO;
  [Key(1238)]
  public float PLAYER_SPECIAL_CHARGE_TARGET = 10f;
  [Key(1239)]
  public int PLAYER_ARROW_AMMO = 3;
  [Key(1240)]
  public int PLAYER_ARROW_TOTAL_AMMO = 3;
  [Key(1241)]
  public int PLAYER_SPIRIT_AMMO;
  [Key(1242)]
  public int PLAYER_SPIRIT_TOTAL_AMMO;
  [Key(1243)]
  public int PLAYER_REDEAL_TOKEN;
  [Key(1244)]
  public int PLAYER_REDEAL_TOKEN_TOTAL;
  [Key(1245)]
  public int PLAYER_HEALTH_MODIFIED;
  [Key(1246)]
  public float COOP_PLAYER_SPIRIT_HEARTS;
  [Key(1247)]
  public float COOP_PLAYER_SPIRIT_TOTAL_HEARTS;
  [Key(1248)]
  public int PLAYER_STARTING_HEALTH_CACHED = -1;
  [Key(1249)]
  public int Souls;
  [Key(1250)]
  public int BlackSouls;
  [Key(1251)]
  public int BlackSoulTarget;
  [Key(1252)]
  public int FollowerTokens;
  [Key(1253)]
  public int SpyDay = -1;
  [Key(1254)]
  public int SpyJoinedDay = -1;
  [Key(1255)]
  public int ShrineGhostJuice;
  [Key(1256)]
  public int TotalShrineGhostJuice;
  [Key(1257)]
  public int YngyaMiscConvoIndex;
  [Key(1258)]
  public float ChoreXP;
  [Key(1259)]
  public float ChoreXP_Coop;
  [Key(1260)]
  public float ChoreXP_Coop_Temp_Gained;
  [Key(1261)]
  public int ChoreXPLevel;
  [Key(1262)]
  public int ChoreXPLevel_Coop;
  public static List<float> TargetChoreXP = new List<float>()
  {
    3f,
    5f,
    10f,
    20f,
    30f,
    50f,
    75f,
    100f,
    150f,
    200f
  };
  [Key(1263)]
  public float DiscipleXP;
  [Key(1264)]
  public int DiscipleLevel;
  [Key(1265)]
  public int DiscipleAbilityPoints;
  public static List<float> TargetDiscipleXP = new List<float>()
  {
    3f,
    2f,
    3f,
    4f
  };
  [Key(1266)]
  public int XP;
  [Key(1267)]
  public int Level;
  public const float XPMultiplier = 1.3f;
  public const float XPMultiplierLvlII = 1.5f;
  public const float XPMultiplierLvlIII = 2.5f;
  public const float XPMultiplierLvlIV = 3f;
  public static List<int> TargetXP = new List<int>()
  {
    1,
    Mathf.FloorToInt(13f),
    Mathf.FloorToInt(29.9f),
    Mathf.FloorToInt(45.5f),
    Mathf.FloorToInt(59.8f),
    Mathf.FloorToInt(65f),
    Mathf.FloorToInt(68.8999939f),
    Mathf.FloorToInt(93f),
    Mathf.FloorToInt(93f),
    Mathf.FloorToInt(106.5f),
    Mathf.FloorToInt(106.5f),
    Mathf.FloorToInt(200f),
    Mathf.FloorToInt(200f),
    Mathf.FloorToInt(195f),
    Mathf.FloorToInt(215f),
    Mathf.FloorToInt(215f),
    Mathf.FloorToInt(215f),
    Mathf.FloorToInt(258f),
    Mathf.FloorToInt(258f),
    Mathf.FloorToInt(285f),
    Mathf.FloorToInt(285f),
    Mathf.FloorToInt(315f),
    Mathf.FloorToInt(345f),
    Mathf.FloorToInt(339f),
    Mathf.FloorToInt(363f),
    Mathf.FloorToInt(363f),
    Mathf.FloorToInt(363f),
    Mathf.FloorToInt(363f),
    Mathf.FloorToInt(390f),
    Mathf.FloorToInt(390f),
    Mathf.FloorToInt(390f),
    Mathf.FloorToInt(390f),
    Mathf.FloorToInt(417f),
    Mathf.FloorToInt(441f),
    Mathf.FloorToInt(441f),
    Mathf.FloorToInt(441f),
    Mathf.FloorToInt(441f),
    Mathf.FloorToInt(465f),
    Mathf.FloorToInt(465f),
    Mathf.FloorToInt(465f),
    Mathf.FloorToInt(465f)
  };
  [Key(1268)]
  public int AbilityPoints;
  [Key(1269)]
  public int WeaponAbilityPoints = 100;
  [Key(1270)]
  public int CurrentChallengeModeXP;
  [Key(1271)]
  public int CurrentChallengeModeLevel;
  public static List<int> ChallengeModeTargetXP = new List<int>()
  {
    50,
    65,
    100,
    125,
    150,
    200,
    250,
    300
  };
  [Key(1272)]
  public float Doctrine_Pleasure_XP;
  [Key(1273)]
  public float Doctrine_Winter_XP;
  [Key(1274)]
  public float Doctrine_PlayerUpgrade_XP;
  [Key(1275)]
  public int Doctrine_PlayerUpgrade_Level;
  [Key(1276)]
  public float Doctrine_Special_XP;
  [Key(1277)]
  public int Doctrine_Special_Level;
  [Key(1278)]
  public float Doctrine_WorkWorship_XP;
  [Key(1279 /*0x04FF*/)]
  public int Doctrine_WorkWorship_Level;
  [Key(1280 /*0x0500*/)]
  public float Doctrine_Possessions_XP;
  [Key(1281)]
  public int Doctrine_Possessions_Level;
  [Key(1282)]
  public float Doctrine_Food_XP;
  [Key(1283)]
  public int Doctrine_Food_Level;
  [Key(1284)]
  public float Doctrine_Afterlife_XP;
  [Key(1285)]
  public int Doctrine_Afterlife_Level;
  [Key(1286)]
  public float Doctrine_LawAndOrder_XP;
  [Key(1287)]
  public int Doctrine_LawAndOrder_Level;
  [Key(1288)]
  public int Doctrine_Pleasure_Level;
  [Key(1289)]
  public int Doctrine_Winter_Level;
  [Key(1290)]
  public int CompletedDoctrineStones;
  [Key(1291)]
  public int DoctrineCurrentCount;
  [Key(1292)]
  public int DoctrineTargetCount;
  [Key(1293)]
  public int FRUIT_LOW_MEALS_COOKED;
  [Key(1294)]
  public int VEGETABLE_LOW_MEALS_COOKED;
  [Key(1295)]
  public int VEGETABLE_MEDIUM_MEALS_COOKED;
  [Key(1296)]
  public int VEGETABLE_HIGH_MEALS_COOKED;
  [Key(1297)]
  public int FISH_LOW_MEALS_COOKED;
  [Key(1298)]
  public int FISH_MEDIUM_MEALS_COOKED;
  [Key(1299)]
  public int FISH_HIGH_MEALS_COOKED;
  [Key(1300)]
  public int EGG_MEALS_COOKED;
  [Key(1301)]
  public int MEAT_LOW_COOKED;
  [Key(1302)]
  public int MEAT_MEDIUM_COOKED;
  [Key(1303)]
  public int MEAT_HIGH_COOKED;
  [Key(1304)]
  public int MIXED_LOW_COOKED;
  [Key(1305)]
  public int MIXED_MEDIUM_COOKED;
  [Key(1306)]
  public int MIXED_HIGH_COOKED;
  [Key(1307)]
  public int POOP_MEALS_COOKED;
  [Key(1308)]
  public int GRASS_MEALS_COOKED;
  [Key(1309)]
  public int FOLLOWER_MEAT_MEALS_COOKED;
  [Key(1310)]
  public int DEADLY_MEALS_COOKED;
  [Key(1311)]
  public List<string> ReapedSouls = new List<string>();
  public static List<float> PlayerUpgradeTargetXP = new List<float>()
  {
    0.3f,
    0.4f,
    1.1f,
    1.2f,
    1.5f,
    2f,
    2.6f,
    3.5f,
    5f,
    6f,
    7f,
    8f,
    9f,
    10f
  };
  public static float DoctrineMultiplier = 4f;
  [IgnoreMember]
  public static List<float> DoctrineTargetXP = new List<float>()
  {
    0.5f,
    (float) (0.60000002384185791 * ((double) DataManager.DoctrineMultiplier / 2.0)),
    (float) (1.0 * ((double) DataManager.DoctrineMultiplier / 1.5)),
    1.5f * DataManager.DoctrineMultiplier,
    3f * DataManager.DoctrineMultiplier,
    6f * DataManager.DoctrineMultiplier,
    9f * DataManager.DoctrineMultiplier,
    12f * DataManager.DoctrineMultiplier,
    15f * DataManager.DoctrineMultiplier,
    18f * DataManager.DoctrineMultiplier
  };
  [IgnoreMember]
  public int currentweapon;
  [CompilerGenerated]
  public bool \u003CAwokenMountainDeathsceen\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CIceGoreShown\u003Ek__BackingField;
  [CompilerGenerated]
  public DataManager.GhostLostLambState \u003CghostLostLambState\u003Ek__BackingField;
  public static DataManager instance = (DataManager) null;
  [Key(1314)]
  public List<DataManager.EnemyData> EnemiesKilled = new List<DataManager.EnemyData>();
  [Key(1315)]
  public Alerts Alerts = new Alerts();
  [Key(1316)]
  public List<FinalizedNotification> NotificationHistory = new List<FinalizedNotification>();
  public static List<StructureBrain.TYPES> CultistDLCStructures = new List<StructureBrain.TYPES>()
  {
    StructureBrain.TYPES.TILE_FLOWERS,
    StructureBrain.TYPES.DECORATION_FLOWERPOTWALL,
    StructureBrain.TYPES.DECORATION_LEAFYLAMPPOST,
    StructureBrain.TYPES.DECORATION_FLOWERVASE,
    StructureBrain.TYPES.DECORATION_WATERINGCAN,
    StructureBrain.TYPES.DECORATION_FLOWER_CART_SMALL,
    StructureBrain.TYPES.DECORATION_WEEPINGSHRINE
  };
  public static List<string> CultistDLCSkins = new List<string>()
  {
    "Bee",
    "Tapir",
    "Turtle",
    "Monkey",
    "Narwal"
  };
  public static List<StructureBrain.TYPES> HereticDLCStructures = new List<StructureBrain.TYPES>()
  {
    StructureBrain.TYPES.TILE_OLDFAITH,
    StructureBrain.TYPES.DECORATION_OLDFAITH_CRYSTAL,
    StructureBrain.TYPES.DECORATION_OLDFAITH_FLAG,
    StructureBrain.TYPES.DECORATION_OLDFAITH_FOUNTAIN,
    StructureBrain.TYPES.DECORATION_OLDFAITH_IRONMAIDEN,
    StructureBrain.TYPES.DECORATION_OLDFAITH_SHRINE,
    StructureBrain.TYPES.DECORATION_OLDFAITH_TORCH,
    StructureBrain.TYPES.DECORATION_OLDFAITH_WALL
  };
  public static List<string> HereticDLCSkins = new List<string>()
  {
    "Moose",
    "Gorilla",
    "Mosquito",
    "Goldfish",
    "Possum"
  };
  public static List<StructureBrain.TYPES> SinfulDLCStructures = new List<StructureBrain.TYPES>()
  {
    StructureBrain.TYPES.DECORATION_SINFUL_STATUE,
    StructureBrain.TYPES.DECORATION_SINFUL_CRUCIFIX,
    StructureBrain.TYPES.DECORATION_SINFUL_FLOWERS1,
    StructureBrain.TYPES.DECORATION_SINFUL_FLOWERS2,
    StructureBrain.TYPES.DECORATION_SINFUL_SKULL,
    StructureBrain.TYPES.DECORATION_SINFUL_INCENSE
  };
  public static List<string> SinfulDLCSkins = new List<string>()
  {
    "Hammerhead",
    "Llama",
    "Tiger",
    "Sphynx",
    "LadyBug"
  };
  public static List<StructureBrain.TYPES> PilgrimDLCStructures = new List<StructureBrain.TYPES>()
  {
    StructureBrain.TYPES.DECORATION_PILGRIM_BONSAI,
    StructureBrain.TYPES.DECORATION_PILGRIM_LANTERN,
    StructureBrain.TYPES.DECORATION_PILGRIM_PAGODA,
    StructureBrain.TYPES.DECORATION_PILGRIM_VASE,
    StructureBrain.TYPES.DECORATION_PILGRIM_WALL
  };
  public static List<string> PilgrimDLCSkins = new List<string>()
  {
    "Panda",
    "Skunk",
    "Anteater",
    "Enchida",
    "Camel"
  };
  public static List<StructureBrain.TYPES> MajorDLCStructures = new List<StructureBrain.TYPES>()
  {
    StructureBrain.TYPES.RANCH,
    StructureBrain.TYPES.RANCH_2,
    StructureBrain.TYPES.RANCH_FENCE,
    StructureBrain.TYPES.RANCH_HUTCH,
    StructureBrain.TYPES.RANCH_TROUGH,
    StructureBrain.TYPES.RANCH_CHOPPING_BLOCK,
    StructureBrain.TYPES.WEATHER_VANE,
    StructureBrain.TYPES.LOGISTICS,
    StructureBrain.TYPES.MEDIC,
    StructureBrain.TYPES.VOLCANIC_SPA,
    StructureBrain.TYPES.WOLF_TRAP,
    StructureBrain.TYPES.LIGHTNING_ROD,
    StructureBrain.TYPES.FURNACE_1,
    StructureBrain.TYPES.FURNACE_2,
    StructureBrain.TYPES.PROXIMITY_FURNACE,
    StructureBrain.TYPES.FURNACE_3,
    StructureBrain.TYPES.TOOLSHED,
    StructureBrain.TYPES.TRAIT_MANIPULATOR_1,
    StructureBrain.TYPES.TRAIT_MANIPULATOR_2,
    StructureBrain.TYPES.TRAIT_MANIPULATOR_3,
    StructureBrain.TYPES.DECORATION_DLC_ROT_BOTTLE,
    StructureBrain.TYPES.DECORATION_DLC_ROT_BUCKET,
    StructureBrain.TYPES.DECORATION_DLC_ROT_CAGE1,
    StructureBrain.TYPES.DECORATION_DLC_ROT_CAGE2,
    StructureBrain.TYPES.DECORATION_DLC_ROT_CAULDRON,
    StructureBrain.TYPES.DECORATION_DLC_ROT_DIORAMA,
    StructureBrain.TYPES.DECORATION_DLC_ROT_FIREMACHINE,
    StructureBrain.TYPES.DECORATION_DLC_ROT_FLOOR,
    StructureBrain.TYPES.DECORATION_DLC_ROT_IRONMAIDEN,
    StructureBrain.TYPES.DECORATION_DLC_ROT_LUMP1,
    StructureBrain.TYPES.DECORATION_DLC_ROT_LUMP2,
    StructureBrain.TYPES.DECORATION_DLC_ROT_PILLAR1,
    StructureBrain.TYPES.DECORATION_DLC_ROT_PILLAR2,
    StructureBrain.TYPES.DECORATION_DLC_ROT_STONE1,
    StructureBrain.TYPES.DECORATION_DLC_ROT_STONE2,
    StructureBrain.TYPES.DECORATION_DLC_ROT_TENTACLE,
    StructureBrain.TYPES.DECORATION_DLC_ROT_WALL,
    StructureBrain.TYPES.DECORATION_DLC_WOLF_BULB,
    StructureBrain.TYPES.DECORATION_DLC_WOLF_CULTIST1,
    StructureBrain.TYPES.DECORATION_DLC_WOLF_CULTIST2,
    StructureBrain.TYPES.DECORATION_DLC_WOLF_DIORAMA,
    StructureBrain.TYPES.DECORATION_DLC_WOLF_FIREPIT,
    StructureBrain.TYPES.DECORATION_DLC_WOLF_LAMPPOST,
    StructureBrain.TYPES.DECORATION_DLC_WOLF_PILLAR1,
    StructureBrain.TYPES.DECORATION_DLC_WOLF_PILLAR2,
    StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE1,
    StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE2,
    StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE3,
    StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE4,
    StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE5,
    StructureBrain.TYPES.DECORATION_DLC_WOLF_TESLA,
    StructureBrain.TYPES.DECORATION_DLC_WOLF_TREE1,
    StructureBrain.TYPES.DECORATION_DLC_WOLF_WIRES,
    StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_CHIMNEY_1,
    StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_CHIMNEY_2,
    StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_LAMPPOST,
    StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_CLOCK,
    StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_WALL,
    StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_PLANT,
    StructureBrain.TYPES.DECORATION_DLC_YNGYA_CANDLE,
    StructureBrain.TYPES.DECORATION_DLC_YNGYA_FLOWERBUCKET,
    StructureBrain.TYPES.DECORATION_DLC_YNGYA_STICKBUNDLE,
    StructureBrain.TYPES.DECORATION_DLC_YNGYA_TALLFLOWERS,
    StructureBrain.TYPES.DECORATION_DLC_YNGYA_TREEBUSH,
    StructureBrain.TYPES.DECORATION_DLC_YNGYA_TREEPOT,
    StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BUSH1,
    StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BUSH2,
    StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_FOUNTAIN,
    StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_SMALLSTATUE1,
    StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_SMALLSTATUE2,
    StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_STREETLAMP,
    StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_TREE,
    StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_WALL,
    StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_FLOOR,
    StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_WOLF,
    StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_YNGYA,
    StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_EXECUTIONER,
    StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_JUG,
    StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_RUG,
    StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BARREL1,
    StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BARREL2,
    StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_CANDLE,
    StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BELLS,
    StructureBrain.TYPES.TILE_WATER,
    StructureBrain.TYPES.DECORATION_EASTEREGG_EGG,
    StructureBrain.TYPES.DECORATION_EASTEREGG_HAROSTATUE,
    StructureBrain.TYPES.DECORATION_EASTEREGG_TURUA,
    StructureBrain.TYPES.DECORATION_EASTEREGG_WARRACKA
  };
  [IgnoreMember]
  public FollowerClothingType[] Cultist_DLC_Clothing = new FollowerClothingType[2]
  {
    FollowerClothingType.Cultist_DLC,
    FollowerClothingType.Cultist_DLC2
  };
  [IgnoreMember]
  public FollowerClothingType[] Heretic_DLC_Clothing = new FollowerClothingType[2]
  {
    FollowerClothingType.Heretic_DLC,
    FollowerClothingType.Heretic_DLC2
  };
  [IgnoreMember]
  public FollowerClothingType[] Sinful_DLC_Clothing = new FollowerClothingType[6]
  {
    FollowerClothingType.DLC_1,
    FollowerClothingType.DLC_2,
    FollowerClothingType.DLC_3,
    FollowerClothingType.DLC_4,
    FollowerClothingType.DLC_5,
    FollowerClothingType.DLC_6
  };
  [IgnoreMember]
  public FollowerClothingType[] Major_DLC_Clothing = new FollowerClothingType[12]
  {
    FollowerClothingType.Winter_1,
    FollowerClothingType.Winter_2,
    FollowerClothingType.Winter_3,
    FollowerClothingType.Winter_4,
    FollowerClothingType.Winter_5,
    FollowerClothingType.Winter_6,
    FollowerClothingType.Normal_MajorDLC_1,
    FollowerClothingType.Normal_MajorDLC_2,
    FollowerClothingType.Normal_MajorDLC_3,
    FollowerClothingType.Normal_MajorDLC_4,
    FollowerClothingType.Normal_MajorDLC_5,
    FollowerClothingType.Normal_MajorDLC_6
  };
  [IgnoreMember]
  public FollowerClothingType[] Pilgrim_DLC_Clothing = new FollowerClothingType[2]
  {
    FollowerClothingType.Pilgrim_DLC,
    FollowerClothingType.Pilgrim_DLC2
  };
  [Key(1317)]
  public float blizzardTimeInCurrentSeason = -1f;
  [Key(1318)]
  public float blizzardEndTimeInCurrentSeason = -1f;
  [Key(1319)]
  public float blizzardTimeInCurrentSeason2 = -1f;
  [Key(1320)]
  public float blizzardEndTimeInCurrentSeason2 = -1f;

  [IgnoreMember]
  public bool DoubledHeartsInFireIceHealingPoolRooms => this.BroughtFishingRod;

  [XmlIgnore]
  [IgnoreMember]
  public bool TailorEnabled => DataManager.instance.RevealedTailor;

  [XmlIgnore]
  [IgnoreMember]
  public bool PleasureEnabled => DataManager.instance.HasBuiltPleasureShrine;

  [IgnoreMember]
  public bool WinterDoctrineAvailable
  {
    get
    {
      return DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.Special_EmbraceRot) || DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.Special_RejectRot);
    }
  }

  public static bool TwitchTotemRewardAvailable()
  {
    return DataManager.GetAvailableTwitchTotemDecorations().Count > 0 || DataManager.GetAvailableTwitchTotemSkins().Count > 0;
  }

  public static List<StructureBrain.TYPES> GetAvailableTwitchTotemDecorations()
  {
    List<StructureBrain.TYPES> totemDecorations = new List<StructureBrain.TYPES>(6)
    {
      StructureBrain.TYPES.DECORATION_TWITCH_FLAG_CROWN,
      StructureBrain.TYPES.DECORATION_TWITCH_MUSHROOM_BAG,
      StructureBrain.TYPES.DECORATION_TWITCH_ROSE_BUSH,
      StructureBrain.TYPES.DECORATION_TWITCH_STONE_FLAG,
      StructureBrain.TYPES.DECORATION_TWITCH_STONE_STATUE,
      StructureBrain.TYPES.DECORATION_TWITCH_WOODEN_GUARDIAN
    };
    for (int index = totemDecorations.Count - 1; index >= 0; --index)
    {
      if (StructuresData.GetUnlocked(totemDecorations[index]))
        totemDecorations.RemoveAt(index);
    }
    return totemDecorations;
  }

  public static List<string> GetAvailableTwitchTotemSkins()
  {
    List<string> twitchTotemSkins = new List<string>(2)
    {
      "TwitchCat",
      "TwitchMouse"
    };
    for (int index = twitchTotemSkins.Count - 1; index >= 0; --index)
    {
      if (DataManager.GetFollowerSkinUnlocked(twitchTotemSkins[index]))
        twitchTotemSkins.RemoveAt(index);
    }
    return twitchTotemSkins;
  }

  public void AddEncounteredEnemy(string enemy)
  {
    for (int index = 0; index < enemy.Length; ++index)
    {
      if (enemy[index] == '(')
      {
        enemy = enemy.Remove(index - 1, enemy.Length - index + 1);
        break;
      }
    }
    if (this.enemiesEncountered.Contains(enemy))
      return;
    this.enemiesEncountered.Add(enemy);
  }

  public bool HasEncounteredEnemy(string enemy)
  {
    for (int index = 0; index < enemy.Length; ++index)
    {
      if (enemy[index] == '(')
      {
        enemy = enemy.Remove(index - 1, enemy.Length - index + 1);
        break;
      }
    }
    return this.enemiesEncountered.Contains(enemy);
  }

  public MiniBossController.MiniBossData GetMiniBossData(Enemy enemyType)
  {
    foreach (MiniBossController.MiniBossData miniBossData in this.MiniBossData)
    {
      if (miniBossData.EnemyType == enemyType)
        return miniBossData;
    }
    return (MiniBossController.MiniBossData) null;
  }

  public bool DiscoverLocation(FollowerLocation location)
  {
    this.Alerts.Locations.Add(location);
    if (this.DiscoveredLocations.Contains(location))
      return false;
    this.DiscoveredLocations.Add(location);
    if (location != FollowerLocation.DLC_ShrineRoom && this.DiscoveredLocations.Contains(FollowerLocation.Hub1_RatauOutside) && this.DiscoveredLocations.Contains(FollowerLocation.Hub1_Sozo) && this.DiscoveredLocations.Contains(FollowerLocation.HubShore) && this.DiscoveredLocations.Contains(FollowerLocation.Dungeon_Decoration_Shop1) && this.DiscoveredLocations.Contains(FollowerLocation.Dungeon_Location_4))
      AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("FIND_ALL_LOCATIONS"));
    return true;
  }

  public static bool HasKeyPieceFromLocation(FollowerLocation Location, int Layer)
  {
    return DataManager.Instance.KeyPiecesFromLocation.Contains($"{Location.ToString()}_{Layer.ToString()}");
  }

  public static void SaveKeyPieceFromLocation(FollowerLocation Location, int Layer)
  {
    if (DataManager.HasKeyPieceFromLocation(Location, Layer))
      return;
    DataManager.Instance.KeyPiecesFromLocation.Add($"{Location.ToString()}_{Layer.ToString()}");
  }

  public bool TryRevealTutorialTopic(TutorialTopic topic)
  {
    if (DataManager.Instance.QuickStartActive && !this.IgnoreFromQuickStart.Contains(topic) || this.RevealedTutorialTopics.Contains(topic) || CheatConsole.HidingUI)
      return false;
    this.RevealedTutorialTopics.Add(topic);
    this.Alerts.Tutorial.AddOnce(topic);
    return true;
  }

  public bool ClothesUnlocked(FollowerClothingType type)
  {
    foreach (FollowerClothingType followerClothingType in this.UnlockedClothing)
    {
      if (followerClothingType == type)
        return true;
    }
    return type == FollowerClothingType.None;
  }

  public void AddNewClothes(FollowerClothingType type)
  {
    foreach (FollowerClothingType followerClothingType in this.UnlockedClothing)
    {
      if (followerClothingType == type)
        return;
    }
    this.Alerts.ClothingAlerts.Add(type);
    this.UnlockedClothing.Add(type);
    if (!TailorManager.HasUnlockedAllClothing())
      return;
    AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("ALL_OUTFITS"));
  }

  public void SetClothingColour(FollowerClothingType clothingType, int colour)
  {
    for (int index = 0; index < this.ClothingVariants.Count; ++index)
    {
      if (this.ClothingVariants[index].ClothingType == clothingType)
      {
        this.ClothingVariants[index].Colour = colour;
        return;
      }
    }
    this.ClothingVariants.Add(new DataManager.ClothingVariant()
    {
      ClothingType = clothingType,
      Colour = colour
    });
  }

  public int GetClothingColour(FollowerClothingType clothingType)
  {
    for (int index = 0; index < this.ClothingVariants.Count; ++index)
    {
      if (this.ClothingVariants[index].ClothingType == clothingType)
        return this.ClothingVariants[index].Colour;
    }
    return 0;
  }

  public void SetClothingVariant(FollowerClothingType clothingType, string variant)
  {
    for (int index = 0; index < this.ClothingVariants.Count; ++index)
    {
      if (this.ClothingVariants[index].ClothingType == clothingType)
      {
        this.ClothingVariants[index].Variant = variant;
        return;
      }
    }
    this.ClothingVariants.Add(new DataManager.ClothingVariant()
    {
      ClothingType = clothingType,
      Colour = 0,
      Variant = variant
    });
  }

  public string GetClothingVariant(FollowerClothingType clothingType)
  {
    for (int index = 0; index < this.ClothingVariants.Count; ++index)
    {
      if (this.ClothingVariants[index].ClothingType == clothingType)
        return this.ClothingVariants[index].Variant;
    }
    return "";
  }

  public void SetTutorialVariables()
  {
    DataManager.RandomSeed = new System.Random(UnityEngine.Random.Range(int.MinValue, int.MaxValue));
    DataManager.Instance.AllowSaving = this.QuickStartActive;
    DataManager.Instance.EnabledHealing = this.QuickStartActive;
    DataManager.Instance.BuildShrineEnabled = this.QuickStartActive;
    DataManager.instance.CookedFirstFood = this.QuickStartActive;
    DataManager.instance.XPEnabled = this.QuickStartActive;
    DataManager.Instance.InTutorial = this.QuickStartActive;
    DataManager.Instance.Tutorial_Second_Enter_Base = this.QuickStartActive;
    DataManager.Instance.AllowBuilding = this.QuickStartActive;
    DataManager.Instance.ShowLoyaltyBars = this.QuickStartActive;
    DataManager.Instance.RatExplainDungeon = this.QuickStartActive;
    DataManager.Instance.ShowCultFaith = this.QuickStartActive;
    DataManager.Instance.ShowCultHunger = this.QuickStartActive;
    DataManager.Instance.ShowCultIllness = this.QuickStartActive;
    DataManager.Instance.ShowCultWarmth = false;
    DataManager.Instance.UnlockBaseTeleporter = this.QuickStartActive;
    DataManager.Instance.BonesEnabled = this.QuickStartActive;
    DataManager.instance.PauseGameTime = !this.QuickStartActive;
    DataManager.instance.ShownDodgeTutorial = this.QuickStartActive;
    DataManager.instance.ShownInventoryTutorial = this.QuickStartActive;
    DataManager.instance.HasEncounteredTarot = this.QuickStartActive;
    DataManager.Instance.CurrentGameTime = 244f;
    DataManager.Instance.HasBuiltShrine1 = false;
    DataManager.Instance.OnboardedHomeless = this.QuickStartActive;
    DataManager.Instance.ForceDoctrineStones = this.QuickStartActive;
    DataManager.instance.HadInitialDeathCatConversation = this.QuickStartActive;
    DataManager.instance.PlayerHasBeenGivenHearts = this.QuickStartActive;
    DataManager.instance.BaseGoopDoorLocked = !this.QuickStartActive;
    DataManager.instance.FirstRelic = true;
    DataManager.instance.CanBuildShrine = this.QuickStartActive;
    DataManager.instance.FirstDoctrineStone = this.QuickStartActive;
    DataManager.instance.RevealedBaseYngyaShrine = this.QuickStartActive;
    DataManager.Instance.PLAYER_TOTAL_HEALTH = (float) DataManager.Instance.PLAYER_STARTING_HEALTH;
    DataManager.instance.PLAYER_STARTING_HEALTH_CACHED = DataManager.Instance.PLAYER_STARTING_HEALTH;
    DataManager.instance.PLAYER_HEALTH = (float) DataManager.Instance.PLAYER_STARTING_HEALTH;
    DataManager.instance.SpyDay = UnityEngine.Random.Range(35, 65);
    for (int index = 0; index < PlayerFarming.playersCount; ++index)
    {
      HealthPlayer health = PlayerFarming.players[index].health;
      if ((UnityEngine.Object) health != (UnityEngine.Object) null)
      {
        health.PLAYER_TOTAL_HEALTH = (float) health.PLAYER_STARTING_HEALTH;
        health.PLAYER_STARTING_HEALTH_CACHED = (float) health.PLAYER_STARTING_HEALTH;
        health.PLAYER_HEALTH = (float) health.PLAYER_STARTING_HEALTH;
      }
    }
    DataManager instance = DataManager.instance;
    int num = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
    num = num.ToString().GetStableHashCode();
    string str = num.ToString();
    instance.SaveUniqueID = str;
    GameManager.CurrentDungeonLayer = 1;
    if (DataManager.instance.QuickStartActive)
    {
      DataManager.instance.OnboardedRelics = true;
      DataManager.Instance.FirstRelic = false;
      DataManager.Instance.UnlockedUpgrades.Add(UpgradeSystem.Type.Ritual_HeartsOfTheFaithful1);
      UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Relic_Pack_Default);
      EquipmentManager.UnlockRelics(UpgradeSystem.Type.Relic_Pack_Default);
      DataManager.instance.dungeonRun = 1;
      DataManager.Instance.Followers_Recruit.Add(FollowerInfo.NewCharacter(FollowerLocation.Base));
      this.DiscoverLocation(FollowerLocation.Base);
      this.DiscoverLocation(FollowerLocation.Hub1_RatauOutside);
      DataManager.instance.WeaponPool.Add(EquipmentType.Sword);
      DataManager.instance.WeaponPool.Add(EquipmentType.Axe);
      DataManager.instance.WeaponPool.Add(EquipmentType.Dagger);
      DataManager.instance.WeaponPool.Add(EquipmentType.Gauntlet);
      DataManager.instance.WeaponPool.Add(EquipmentType.Hammer);
      DataManager.instance.WeaponPool.Add(EquipmentType.Blunderbuss);
      DataManager.instance.CursePool.Add(EquipmentType.Fireball);
      DataManager.instance.CursePool.Add(EquipmentType.Tentacles);
      DataManager.instance.CursePool.Add(EquipmentType.ProjectileAOE);
      DataManager.instance.CursePool.Add(EquipmentType.MegaSlash);
      DataManager.instance.CursePool.Add(EquipmentType.EnemyBlast);
      DataManager.Instance.DifficultyChosen = true;
      DataManager.Instance.StaticFaith = 85f;
      DataManager.Instance.HungerBarCount = HungerBar.MAX_HUNGER;
      foreach (TutorialTopic alert in Enum.GetValues(typeof (TutorialTopic)))
      {
        if (Enum.IsDefined(typeof (TutorialTopic), (object) alert) && !this.IgnoreFromQuickStart.Contains(alert) && (!DataManager.Instance.SurvivalModeActive || alert != TutorialTopic.Omnipresence && alert != TutorialTopic.Purgatory))
        {
          this.RevealedTutorialTopics.Add(alert);
          this.Alerts.Tutorial.AddOnce(alert);
        }
      }
    }
    if (DataManager.instance.RemovedStoryMomentsActive)
    {
      DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.DoctrineType.Special_Bonfire);
      DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.DoctrineType.Special_ReadMind);
      DoctrineUpgradeSystem.UnlockAbility(DoctrineUpgradeSystem.DoctrineType.Special_Sacrifice);
    }
    if (DataManager.instance.WinterModeActive)
    {
      Inventory.AddItem(InventoryItem.ITEM_TYPE.LOG, 10);
      Inventory.AddItem(InventoryItem.ITEM_TYPE.STONE, 5);
      Inventory.AddItem(InventoryItem.ITEM_TYPE.MAGMA_STONE, 20);
      Inventory.AddItem(InventoryItem.ITEM_TYPE.BLACK_GOLD, 30);
      UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ability_TeleportHome);
      UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Building_Furnace);
      UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Economy_Refinery);
      UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Building_Temple);
    }
    else if (DataManager.instance.SurvivalModeActive)
    {
      UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ability_TeleportHome);
      Inventory.AddItem(InventoryItem.ITEM_TYPE.BLACK_GOLD, 30);
    }
    if (DataManager.instance.WinterModeActive)
    {
      SeasonsManager.Active = true;
      SeasonsManager.CurrentSeason = SeasonsManager.Season.Winter;
      DataManager.instance.InteractedDLCShrine = true;
      UpgradeSystem.UnlockAbility(UpgradeSystem.Type.WinterSystem);
      UpgradeSystem.UnlockAbility(UpgradeSystem.Type.RanchingSystem);
      UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Building_Ranch);
      DataManager.instance.DiscoveredLocations.Add(FollowerLocation.LambTown);
      DataManager.instance.VisitedLocations.Add(FollowerLocation.LambTown);
      DataManager.instance.OnboardedLambTown = true;
      DataManager.instance.OnboardedDungeon6 = true;
      DataManager.instance.ForeshadowedWolf = true;
      DataManager.instance.OnboardedWolf = true;
      DataManager.instance.OnboardedSeasons = true;
      DataManager.instance.ShowCultWarmth = true;
      DataManager.instance.FollowerOnboardedFreezing = true;
      DataManager.Instance.FollowerOnboardedWinterHere = true;
      DataManager.Instance.FollowerOnboardedWoolyShack = true;
      DataManager.Instance.DLCUpgradeTreeSnowIncrement = 4;
    }
    if (CheatConsole.IN_DEMO)
      return;
    DataManager.Instance.CanReadMinds = DataManager.instance.RemovedStoryMomentsActive;
    DataManager.Instance.RevealOfferingChest = DataManager.instance.RemovedStoryMomentsActive;
    DataManager.Instance.OnboardedOfferingChest = DataManager.instance.RemovedStoryMomentsActive;
    DataManager.Instance.EnabledSpells = this.QuickStartActive;
  }

  public static int GetFlowerPotProgress(int ID)
  {
    if (DataManager.instance == null)
      return 0;
    foreach (DataManager.WoolhavenFlowerPot woolhavenFlowerPot in DataManager.Instance.WoolhavenFlowerPots)
    {
      if (woolhavenFlowerPot.ID == ID)
        return woolhavenFlowerPot.FlowersAdded;
    }
    DataManager.Instance.WoolhavenFlowerPots.Add(new DataManager.WoolhavenFlowerPot()
    {
      ID = ID
    });
    return 0;
  }

  public static int IncrementFlowerPotProgress(int ID)
  {
    for (int index = 0; index < DataManager.Instance.WoolhavenFlowerPots.Count; ++index)
    {
      if (DataManager.Instance.WoolhavenFlowerPots[index].ID == ID)
      {
        ++DataManager.Instance.WoolhavenFlowerPots[index].FlowersAdded;
        break;
      }
    }
    DataManager.Instance.WoolhavenFlowerPots.Add(new DataManager.WoolhavenFlowerPot()
    {
      ID = ID,
      FlowersAdded = 1
    });
    return 0;
  }

  public static int GetNumberOfFullFlowerPots()
  {
    return DataManager.Instance.FullWoolhavenFlowerPots.Count;
  }

  public static void SetFlowerPotAsFull(int ID)
  {
    if (DataManager.Instance.FullWoolhavenFlowerPots.Contains(ID))
      return;
    DataManager.Instance.FullWoolhavenFlowerPots.Add(ID);
    DLCShrineRoomLocationManager.CheckWoolhavenCompleteAchievement();
    ObjectiveManager.CompleteFlowerBasketsObjective();
  }

  public static void ResetFlowerPots()
  {
    DataManager.Instance.FullWoolhavenFlowerPots.Clear();
    DataManager.Instance.WoolhavenFlowerPots.Clear();
  }

  [IgnoreMember]
  public float PlayerDamageDealt
  {
    get => this.playerDamageDealt;
    set
    {
      this.playerDamageDealt = value;
      DifficultyManager.LoadCurrentDifficulty();
    }
  }

  [IgnoreMember]
  public float PlayerDamageReceived
  {
    get => this.playerDamageReceived;
    set
    {
      this.playerDamageReceived = value;
      DifficultyManager.LoadCurrentDifficulty();
    }
  }

  public bool GetVariable(DataManager.Variables variable)
  {
    return (bool) typeof (DataManager).GetField(variable.ToString()).GetValue((object) DataManager.Instance);
  }

  public bool GetVariable(string variablestring)
  {
    return (bool) typeof (DataManager).GetField(variablestring).GetValue((object) DataManager.Instance);
  }

  public int GetVariableInt(string variablestring)
  {
    return (int) typeof (DataManager).GetField(variablestring).GetValue((object) DataManager.Instance);
  }

  public int GetVariableInt(DataManager.Variables variable)
  {
    return (int) typeof (DataManager).GetField(variable.ToString()).GetValue((object) DataManager.Instance);
  }

  public void SetVariableInt(DataManager.Variables variable, int Value)
  {
    typeof (DataManager).GetField(variable.ToString()).SetValue((object) DataManager.Instance, (object) Value);
  }

  public void SetVariable(DataManager.Variables variable, bool Toggle)
  {
    typeof (DataManager).GetField(variable.ToString()).SetValue((object) DataManager.Instance, (object) Toggle);
  }

  public void SetVariable(string variablestring, bool Toggle)
  {
    typeof (DataManager).GetField(variablestring).SetValue((object) DataManager.Instance, (object) Toggle);
  }

  public static void SetNewRun(FollowerLocation location = FollowerLocation.None)
  {
    DataManager.instance.EnabledSword = true;
    DataManager.instance.dungeonRunDuration = Time.time;
    DataManager.instance.dungeonVisitedRooms = new List<Map.NodeType>()
    {
      Map.NodeType.FirstFloor
    };
    if (!DataManager.instance.dungeonLocationsVisited.Contains(PlayerFarming.Location))
      DataManager.instance.dungeonLocationsVisited.Add(PlayerFarming.Location);
    DataManager.instance.FollowersRecruitedInNodes = new List<int>();
    DataManager.instance.FollowersRecruitedThisNode = 0;
    DataManager.instance.PlayerKillsOnRun = 0;
    DataManager.instance.PlayerStartingBlackSouls = DataManager.instance.BlackSouls;
    DataManager.instance.dungeonRunXPOrbs = 0;
    ++DataManager.instance.dungeonRun;
    DataManager.instance.ShrineTimerInfo.Clear();
    if (DataManager.instance.BossesCompleted.Count >= 1)
      ++DataManager.instance.DungeonRunsSinceBeatingFirstDungeon;
    PlayerFarming.SetResetHealthData(true);
    DataManager.Instance.LegendaryBlunderbussPlimboEasterEggActive = false;
    Debug.Log((object) ("Increase dungeon run! " + DataManager.instance.dungeonRun.ToString()));
    DataManager.instance.GivenFollowerHearts = false;
    DataManager.RandomSeed = new System.Random(UnityEngine.Random.Range(int.MinValue, int.MaxValue));
    DataManager.UseDataManagerSeed = true;
    DataManager.instance.PlayerHasFollowers = DataManager.instance.Followers.Count > 0;
    DataManager.instance.PlayerDamageReceivedThisRun = 0.0f;
    DataManager.instance.PlayerDamageDealtThisRun = 0.0f;
    DataManager.instance.SpecialAttackDamageMultiplier = 1.25f;
    DataManager.instance.NextChestGold = false;
    DataManager.instance.SpecialAttacksDisabled = false;
    DataManager.instance.BossHealthMultiplier = 1f;
    DataManager.instance.HadNecklaceOnRun = 0;
    DataManager.instance.CanFindLeaderRelic = false;
    PlayerReturnToBase.Disabled = false;
    Lamb.UI.DeathScreen.UIDeathScreenOverlayController.UsedBossPortal = false;
    DungeonModifier.SetActiveModifier((DungeonModifier) null);
    DataManager.Instance.CurrentRunWeaponLevel = 0;
    DataManager.Instance.CurrentRunCurseLevel = 0;
    if (!DataManager.Instance.OnboardedRelics)
      DataManager.Instance.FirstRelic = true;
    DataManager.instance.HaroConversationCompleted = LocalizationManager.GetTermData($"Conversation_NPC/Haro/Conversation_{DataManager.Instance.HaroConversationIndex}/Line{1}") == null;
    DataManager.instance.DungeonLayer1 = GameManager.CurrentDungeonLayer == 1;
    DataManager.instance.DungeonLayer2 = GameManager.CurrentDungeonLayer == 2;
    DataManager.instance.DungeonLayer3 = GameManager.CurrentDungeonLayer == 3;
    DataManager.instance.DungeonLayer4 = GameManager.CurrentDungeonLayer == 4;
    DataManager.instance.DungeonLayer5 = GameManager.CurrentDungeonLayer == 5;
    DataManager.instance.BeatenDungeon1 = DataManager.instance.DungeonCompleted(FollowerLocation.Dungeon1_1);
    DataManager.instance.BeatenDungeon2 = DataManager.instance.DungeonCompleted(FollowerLocation.Dungeon1_2);
    DataManager.instance.BeatenDungeon3 = DataManager.instance.DungeonCompleted(FollowerLocation.Dungeon1_3);
    DataManager.instance.BeatenDungeon4 = DataManager.instance.DungeonCompleted(FollowerLocation.Dungeon1_4);
    DataManager.instance.BeatenDungeon5 = DataManager.instance.DungeonCompleted(FollowerLocation.Dungeon1_5);
    DataManager.instance.BeatenDungeon6 = DataManager.instance.DungeonCompleted(FollowerLocation.Dungeon1_6);
    DataManager.instance.BeatenOneDungeons = DataManager.instance.BossesCompleted.Count >= 1;
    DataManager.instance.BeatenTwoDungeons = DataManager.instance.BossesCompleted.Count >= 2;
    DataManager.instance.BeatenThreeDungeons = DataManager.instance.BossesCompleted.Count >= 3;
    DataManager.instance.BeatenFourDungeons = DataManager.instance.BossesCompleted.Count >= 4;
    DataManager.instance.BeatenDeathCat = DataManager.instance.DeathCatBeaten;
    DataManager.instance.BeatenWitnessDungeon1 = DataManager.Instance.CheckKilledBosses("Boss Beholder 1");
    DataManager.instance.BeatenWitnessDungeon2 = DataManager.Instance.CheckKilledBosses("Boss Beholder 2");
    DataManager.instance.BeatenWitnessDungeon3 = DataManager.Instance.CheckKilledBosses("Boss Beholder 3");
    DataManager.instance.BeatenWitnessDungeon4 = DataManager.Instance.CheckKilledBosses("Boss Beholder 4");
    DataManager.instance.ForceMarketplaceCat = DataManager.instance.Followers_Demons_Types.Contains(6) || DataManager.instance.Followers_Demons_Types.Contains(7);
    DataManager.instance.CanUnlockRelics = DataManager.instance.BeatenOneDungeons && (DataManager.instance.DungeonRunsSinceBeatingFirstDungeon >= 3 || DataManager.instance.BeatenTwoDungeons);
    DataManager.instance.ShowSpecialMidasRoom = !DataManager.Instance.GivenMidasSkull && (!DataManager.instance.MidasSpecialEncounteredLocations.Contains(location) && DataManager.GetGodTearNotches(location) + 1 >= 1 && location == FollowerLocation.Dungeon1_1 || !DataManager.instance.MidasSpecialEncounteredLocations.Contains(location) && DataManager.GetGodTearNotches(location) + 1 >= 1 && location == FollowerLocation.Dungeon1_3 || !DataManager.instance.MidasSpecialEncounteredLocations.Contains(location) && DataManager.GetGodTearNotches(location) + 1 >= 3 && location == FollowerLocation.Dungeon1_4);
    DataManager.instance.ShowSpecialLeaderRoom = !DataManager.instance.LeaderSpecialEncounteredLocations.Contains(location) && DataManager.GetGodTearNotches(location) + 1 >= 2 && location == FollowerLocation.Dungeon1_1 || !DataManager.instance.LeaderSpecialEncounteredLocations.Contains(location) && DataManager.GetGodTearNotches(location) + 1 >= 1 && location == FollowerLocation.Dungeon1_2 || !DataManager.instance.LeaderSpecialEncounteredLocations.Contains(location) && DataManager.GetGodTearNotches(location) + 1 >= 3 && location == FollowerLocation.Dungeon1_3 || !DataManager.instance.LeaderSpecialEncounteredLocations.Contains(location) && DataManager.GetGodTearNotches(location) + 1 >= 1 && location == FollowerLocation.Dungeon1_4;
    DataManager.instance.ShowSpecialKlunkoRoom = DataManager.GetGodTearNotches(location) + 1 >= 4 && !DataManager.instance.KlunkoSpecialEncountered && location == FollowerLocation.Dungeon1_1;
    DataManager.instance.ShowSpecialPlimboRoom = DataManager.GetGodTearNotches(location) + 1 >= 2 && !DataManager.instance.PlimboSpecialEncountered && location == FollowerLocation.Dungeon1_3;
    DataManager.instance.ShowSpecialFishermanRoom = DataManager.GetGodTearNotches(location) + 1 >= 4 && !DataManager.instance.FishermanSpecialEncountered && location == FollowerLocation.Dungeon1_3;
    DataManager.instance.ShowSpecialLighthouseKeeperRoom = DataManager.GetGodTearNotches(location) + 1 >= 4 && !DataManager.instance.LighthouseKeeperSpecialEncountered && location == FollowerLocation.Dungeon1_2;
    DataManager.instance.ShowSpecialSozoRoom = DataManager.GetGodTearNotches(location) + 1 >= 3 && !DataManager.instance.SozoSpecialEncountered && location == FollowerLocation.Dungeon1_2;
    DataManager.instance.ShowSpecialBaalAndAymRoom = DataManager.instance.HasReturnedBoth && !DataManager.instance.BaalAndAymSpecialEncounterd;
    DataManager.instance.ForceClothesShop = !DataManager.instance.RevealedTailor && (location == FollowerLocation.Dungeon1_4 || GameManager.Layer2);
    if (!DataManager.instance.ForceClothesShop)
      DataManager.instance.ForceClothesShop = !DataManager.instance.LeftBopAtTailor && DataManager.instance.BeatenYngya && (location == FollowerLocation.Dungeon1_5 || location == FollowerLocation.Dungeon1_6);
    DataManager.instance.ForceHeartRoom = DataManager.instance.BeatenYngya && !DataManager.instance.FoundLegendaryDagger && (location == FollowerLocation.Dungeon1_5 || location == FollowerLocation.Dungeon1_6);
    DataManager.instance.ForceSinRoom = DataManager.instance.BeatenYngya && !DataManager.instance.UnlockedClothing.Contains(FollowerClothingType.Normal_MajorDLC_2);
    DataManager.instance.ForceDragonRoom = DataManager.instance.BeatenYngya && DataManager.instance.DragonEggsCollected < Interaction_DragonEgg.DragonSkins.Count;
    DataManager.instance.SpawnPubResources = DataManager.instance.BossesCompleted.Count >= 2 || DataManager.instance.PleasureEnabled;
    DataManager.instance.IsPilgrimRescue = false;
    DataManager.instance.IsJalalaBag = false;
    DataManager.instance.HealingLeshyQuestActive = (DataManager.instance.GaveLeshyHealingQuest || ObjectiveManager.HasCustomObjectiveOfType(global::Objectives.CustomQuestTypes.HealingBishop_Leshy)) && !DataManager.instance.LeshyHealed && DataManager.instance.Followers_Demons_Types.Contains(8) && location == FollowerLocation.Dungeon1_1;
    DataManager.instance.HealingHeketQuestActive = (DataManager.instance.GaveHeketHealingQuest || ObjectiveManager.HasCustomObjectiveOfType(global::Objectives.CustomQuestTypes.HealingBishop_Heket)) && !DataManager.instance.HeketHealed && DataManager.instance.Followers_Demons_Types.Contains(9) && location == FollowerLocation.Dungeon1_2;
    DataManager.instance.HealingKallamarQuestActive = (DataManager.instance.GaveKallamarHealingQuest || ObjectiveManager.HasCustomObjectiveOfType(global::Objectives.CustomQuestTypes.HealingBishop_Kallamar)) && !DataManager.instance.KallamarHealed && DataManager.instance.Followers_Demons_Types.Contains(10) && location == FollowerLocation.Dungeon1_3;
    DataManager.instance.HealingShamuraQuestActive = (DataManager.instance.GaveShamuraHealingQuest || ObjectiveManager.HasCustomObjectiveOfType(global::Objectives.CustomQuestTypes.HealingBishop_Shamura)) && !DataManager.instance.ShamuraHealed && DataManager.instance.Followers_Demons_Types.Contains(11) && location == FollowerLocation.Dungeon1_4;
    DataManager.instance.ForcePalworldEgg = DataManager.Instance.PleasureEnabled && DataManager.instance.PalworldEggsCollected < 4 && !DataManager.instance.PalworldSkinsGivenLocations.Contains(location);
    DataManager.instance.ShowMidasKilling = !DataManager.Instance.GivenMidasSkull && DataManager.instance.MidasFollowerStatueCount >= 4 && location == FollowerLocation.Dungeon1_5 && DataManager.Instance.GetDungeonLayer(location) >= 2;
    DataManager.instance.ShowIcegoreRoom = location == FollowerLocation.Dungeon1_5 && !DataManager.instance.EncounteredIcegoreRoom;
    DataManager.instance.OnboardedMutationRoom = location == FollowerLocation.Dungeon1_5 && !DataManager.instance.BeatenYngya;
    DataManager.instance.CanShowExecutionerRoom1 = location == FollowerLocation.Dungeon1_6 && DataManager.instance.RecruitedRotFollower && !DataManager.instance.ExecutionerRoom1Encountered;
    DataManager.instance.CanShowExecutionerRoom2 = location == FollowerLocation.Dungeon1_6 && DataManager.instance.RecruitedRotFollower && DataManager.instance.ExecutionerRoom1Encountered && !DataManager.instance.ExecutionerRoom2Encountered;
    DataManager.instance.ShowSpecialStelleRoom = location == FollowerLocation.Dungeon1_6 && !DataManager.instance.StelleSpecialEncountered;
    DataManager.instance.ShowSpecialMonchMamaRoom = location == FollowerLocation.Dungeon1_6 && !DataManager.instance.MonchMamaSpecialEncountered && DataManager.instance.StelleSpecialEncountered && DataManager.Instance.GetDungeonLayer(location) >= 2;
    DataManager.instance.ShowSpecialDungeonRancherRoom = !DataManager.instance.ShowSpecialMonchMamaRoom && !DataManager.instance.ShowSpecialDungeonRancherRoom && location == FollowerLocation.Dungeon1_6 && DataManager.Instance.GetDungeonLayer(location) >= 2 && !DataManager.instance.DungeonRancherSpecialEncountered;
    bool flag1 = false;
    DataManager.instance.RatooNeedsRescue = flag1 || DataManager.Instance.CurrentDLCNodeType == DungeonWorldMapIcon.NodeType.Dungeon6_MiniBoss && location == FollowerLocation.Dungeon1_6 && GameManager.CurrentDungeonLayer >= 3 && !DataManager.instance.RatooRescued;
    DataManager.instance.ShowSpecialRefinedResourcesRoom = location == FollowerLocation.Dungeon1_6 && !DataManager.instance.ShowSpecialDungeonRancherRoom && !DataManager.instance.RefinedResourcesSpecialEncountered && DataManager.Instance.GetDungeonLayer(location) >= 3;
    DataManager.instance.ShowSpecialFishermanDLCRoom = !DataManager.instance.ShowSpecialStelleRoom && DataManager.Instance.CurrentDLCNodeType == DungeonWorldMapIcon.NodeType.Dungeon5_MiniBoss && location == FollowerLocation.Dungeon1_5 && !DataManager.instance.FishermanDLCSpecialEncountered;
    bool flag2 = false;
    DataManager.instance.BaalNeedsRescue = location == FollowerLocation.Dungeon1_5 && ((!DataManager.Instance.HasReturnedAym || !DataManager.Instance.HasReturnedBaal || DataManager.Instance.CurrentDLCNodeType != DungeonWorldMapIcon.NodeType.Dungeon5_MiniBoss || DataManager.Instance.GetDungeonLayer(location) < 3 ? 0 : (!DataManager.instance.BaalRescued ? 1 : 0)) | (flag2 ? 1 : 0)) != 0;
    foreach (ObjectivesData objective in DataManager.instance.Objectives)
    {
      if (objective is Objectives_FindFollower objectivesFindFollower && objectivesFindFollower.TargetLocation == location && objectivesFindFollower.TargetFollowerName == "Jalala")
        DataManager.instance.IsPilgrimRescue = true;
      else if (objective is Objectives_Custom objectivesCustom && objectivesCustom.CustomQuestType == global::Objectives.CustomQuestTypes.FindJalalaBag)
        DataManager.instance.IsJalalaBag = true;
    }
    DataManager.instance.MinimumRandomRoomsEncountered = DataManager.instance.MinimumRandomRoomsEncounteredAmount > 3;
    DataManager.instance.CanFindTarotCards = !PlayerFleeceManager.FleecePreventTarotCards();
    PlayerFleeceManager.ResetDamageModifier();
  }

  public static void ResetRunData()
  {
    TrinketManager.RemoveAllTrinkets();
    TrinketManager.RemoveAllEncounteredTrinkets();
    DataManager.Instance.PlayerEaten = false;
    DataManager.Instance.PlayersShagged = false;
    DataManager.Instance.PlayerEaten_COOP = false;
    DataManager.instance.PLAYER_REDEAL_TOKEN = DataManager.instance.PLAYER_REDEAL_TOKEN_TOTAL;
    DataManager.instance.PLAYER_RUN_DAMAGE_LEVEL = 0.0f;
    ResurrectOnHud.ResurrectionType = ResurrectionType.None;
    PlayerFarming.ReloadAllFaith();
    DataManager.Instance.FailedObjectives.Clear();
    DataManager.instance.HadNecklaceOnRun = 0;
    DataManager.Instance.ForcingPlayerWeaponDLC = EquipmentType.None;
    DataManager.Instance.IsLambGhostRescue = false;
    DataManager.Instance.IsMiniBoss = false;
    DataManager.Instance.MapLockCountToUnlock = -1;
    PlayerFarming.SetResetHealthData(true);
    DataManager.Instance.PLAYER_TOTAL_HEALTH = (float) (DataManager.Instance.PLAYER_STARTING_HEALTH + DataManager.Instance.PLAYER_HEARTS_LEVEL + DataManager.instance.PLAYER_HEALTH_MODIFIED);
    DataManager.Instance.PLAYER_STARTING_HEALTH_CACHED = DataManager.Instance.PLAYER_STARTING_HEALTH;
    DataManager.Instance.RedHeartsTemporarilyRemoved = 0;
    HUD_Timer.TimerRunning = false;
    HUD_Timer.Timer = 0.0f;
    DataManager.Instance.PLAYER_HEALTH = DataManager.Instance.PLAYER_TOTAL_HEALTH;
    DataManager.Instance.PLAYER_SPIRIT_HEARTS = DataManager.Instance.PLAYER_SPIRIT_TOTAL_HEARTS = 0.0f;
    DataManager.Instance.COOP_PLAYER_SPIRIT_HEARTS = DataManager.Instance.COOP_PLAYER_SPIRIT_TOTAL_HEARTS = 0.0f;
    DataManager.Instance.PLAYER_BLUE_HEARTS = 0.0f;
    DataManager.instance.PLAYER_BLACK_HEARTS = 0.0f;
    DungeonLayerStatue.ShownDungeonLayer = false;
    DataManager.instance.EnemyModifiersChanceMultiplier = 1f;
    DataManager.instance.EnemyHealthMultiplier = 1f;
    DataManager.instance.CurseFeverMultiplier = 1f;
    DataManager.instance.ProjectileMoveSpeedMultiplier = 1f;
    DataManager.instance.DodgeDistanceMultiplier = 1f;
    DataManager.instance.SpawnPoisonOnAttack = false;
    DataManager.instance.EnemiesInNextRoomHaveModifiers = false;
    DataManager.instance.EnemiesDropGoldDuringRun = false;
    DataManager.instance.NoRollInNextCombatRoom = false;
    DataManager.instance.NoHeartDrops = false;
    DataManager.instance.EnemiesDropBombOnDeath = false;
    DataManager.instance.CurrentRoomCoordinates = Vector2.zero;
    DungeonModifier.SetActiveModifier((DungeonModifier) null);
    DataManager.instance.Alerts.RunTarotCardAlerts.ClearAll();
    DataManager.instance.SpawnedRelicsThisRun.Clear();
    PlayerFarming.ResetRunData();
    for (int index = DataManager.Instance.Objectives.Count - 1; index >= 0; --index)
    {
      if (DataManager.Instance.DungeonObjectives.Contains(DataManager.Instance.Objectives[index]))
        DataManager.Instance.Objectives.Remove(DataManager.Instance.Objectives[index]);
    }
    for (int index = DataManager.Instance.CompletedObjectives.Count - 1; index >= 0; --index)
    {
      if (DataManager.Instance.DungeonObjectives.Contains(DataManager.Instance.CompletedObjectives[index]))
        DataManager.Instance.CompletedObjectives.Remove(DataManager.Instance.CompletedObjectives[index]);
    }
    DataManager.Instance.DungeonObjectives.Clear();
    if (!(bool) (UnityEngine.Object) CoopManager.Instance)
      return;
    CoopManager.Instance.ResetCoopWeapons();
  }

  public void AddNewDungeonSeed(int seed)
  {
    if (this.LastDungeonSeeds.Count >= 5)
      this.LastDungeonSeeds.RemoveAt(4);
    this.LastDungeonSeeds.Add(seed);
  }

  public void ReplaceDeprication(GameManager root)
  {
    if (DataManager.Instance.PlayerVisualFleece == -1)
      DataManager.Instance.PlayerVisualFleece = DataManager.Instance.PlayerFleece;
    if (!UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Knucklebones) && DataManager.Instance.Knucklebones_Opponent_0_Won && DataManager.Instance.Knucklebones_Opponent_1_Won && DataManager.Instance.Knucklebones_Opponent_2_Won)
      UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Building_Knucklebones);
    if (DataManager.instance.OnboardedDisciple && !UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ritual_BecomeDisciple))
      UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_BecomeDisciple);
    if (DataManager.instance.CompletedLighthouseCrystalQuest && !DataManager.instance.FollowerSkinsUnlocked.Contains("Axolotl"))
      DataManager.instance.FollowerSkinsUnlocked.Add("Axolotl");
    foreach (FollowerInfo followerInfo in this.Followers_Dead)
    {
      if (!this.Followers_Dead_IDs.Contains(followerInfo.ID))
        this.Followers_Dead_IDs.Add(followerInfo.ID);
      if (this.Followers_OnMissionary_IDs.Contains(followerInfo.ID))
        this.Followers_OnMissionary_IDs.Remove(followerInfo.ID);
    }
    foreach (FollowerInfo follower in DataManager.instance.Followers)
    {
      if (follower != null)
      {
        if (follower.ID == 99994 || follower.ID == 99995)
        {
          for (int index = 0; index < WorshipperData.Instance.Characters.Count; ++index)
          {
            if (follower.ID == 99994 && WorshipperData.Instance.Characters[index].Title == "Baal")
            {
              follower.SkinCharacter = index;
              break;
            }
            if (follower.ID == 99995 && WorshipperData.Instance.Characters[index].Title == "Aym")
            {
              follower.SkinCharacter = index;
              break;
            }
          }
        }
        else if (follower.ID == 99990 && !follower.Traits.Contains(FollowerTrait.TraitType.Blind))
          follower.Traits.Add(FollowerTrait.TraitType.Blind);
      }
    }
    if (DataManager.instance.SozoAteMushroomDay == int.MaxValue)
    {
      bool flag = false;
      foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
      {
        if (allBrain.Info.SkinName.Contains("Mushroom"))
          flag = true;
      }
      if (!flag)
        DataManager.Instance.SozoAteMushroomDay = 0;
    }
    if (string.IsNullOrEmpty(DataManager.instance.SaveUniqueID))
      DataManager.instance.SaveUniqueID = UnityEngine.Random.Range(int.MinValue, int.MaxValue).ToString().GetStableHashCode().ToString();
    if (this.DeathCatBeaten && !StructuresData.GetUnlocked(StructureBrain.TYPES.DECORATION_BOSS_TROPHY_5) && !this.LeaderFollowersRecruited.Contains(666))
      this.LeaderFollowersRecruited.Add(666);
    try
    {
      for (int index = this.Alerts.CharacterSkinAlerts.Total - 1; index >= 0; --index)
      {
        if (WorshipperData.Instance.GetCharacters(this.Alerts.CharacterSkinAlerts._alerts[index]).Invariant)
          this.Alerts.CharacterSkinAlerts._alerts.RemoveAt(index);
      }
    }
    catch (Exception ex)
    {
    }
    if (!DataManager.Instance.TriedTailorRequiresRevealingFromBase)
    {
      DataManager.Instance.TriedTailorRequiresRevealingFromBase = true;
      if (!DataManager.Instance.UnlockedUpgrades.Contains(UpgradeSystem.Type.TailorSystem) && this.DungeonCompleted(FollowerLocation.Dungeon1_4))
        this.TailorRequiresRevealingFromBase = true;
    }
    for (int index = 0; index < DataManager.instance.BaseStructures.Count; ++index)
    {
      if (DataManager.instance.BaseStructures[index].Type == StructureBrain.TYPES.DECORATION_DST_GLOMMERSTATUE || DataManager.instance.BaseStructures[index].Type == StructureBrain.TYPES.DECORATION_DST_BEEFALOSKELETON)
        DataManager.instance.BaseStructures[index].PrefabPath = StructuresData.GetInfoByType(DataManager.instance.BaseStructures[index].Type, 0).PrefabPath;
    }
    if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_WeaponCritHit) && !DataManager.instance.WeaponPool.Contains(EquipmentType.Blunderbuss_Critical))
      DataManager.Instance.AddWeapon(EquipmentType.Blunderbuss_Critical);
    if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_WeaponFervor) && !DataManager.instance.WeaponPool.Contains(EquipmentType.Blunderbuss_Fervour))
      DataManager.Instance.AddWeapon(EquipmentType.Blunderbuss_Fervour);
    if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_WeaponGodly) && !DataManager.instance.WeaponPool.Contains(EquipmentType.Blunderbuss_Godly))
      DataManager.Instance.AddWeapon(EquipmentType.Blunderbuss_Godly);
    if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_WeaponHeal) && !DataManager.instance.WeaponPool.Contains(EquipmentType.Blunderbuss_Healing))
      DataManager.Instance.AddWeapon(EquipmentType.Blunderbuss_Healing);
    if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_WeaponNecromancy) && !DataManager.instance.WeaponPool.Contains(EquipmentType.Blunderbuss_Nercomancy))
      DataManager.Instance.AddWeapon(EquipmentType.Blunderbuss_Nercomancy);
    if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_WeaponPoison) && !DataManager.instance.WeaponPool.Contains(EquipmentType.Blunderbuss_Poison))
      DataManager.Instance.AddWeapon(EquipmentType.Blunderbuss_Poison);
    if ((DataManager.instance.SurvivalModeActive || DataManager.instance.QuickStartActive) && !DataManager.instance.WeaponPool.Contains(EquipmentType.Blunderbuss))
      DataManager.instance.AddWeapon(EquipmentType.Blunderbuss);
    if (DataManager.instance.WeaponPool.Contains(EquipmentType.Chain))
    {
      if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_WeaponCritHit) && !DataManager.instance.WeaponPool.Contains(EquipmentType.Chain_Critical))
        DataManager.Instance.AddWeapon(EquipmentType.Chain_Critical);
      if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_WeaponFervor) && !DataManager.instance.WeaponPool.Contains(EquipmentType.Chain_Fervour))
        DataManager.Instance.AddWeapon(EquipmentType.Chain_Fervour);
      if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_WeaponGodly) && !DataManager.instance.WeaponPool.Contains(EquipmentType.Chain_Godly))
        DataManager.Instance.AddWeapon(EquipmentType.Chain_Godly);
      if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_WeaponHeal) && !DataManager.instance.WeaponPool.Contains(EquipmentType.Chain_Healing))
        DataManager.Instance.AddWeapon(EquipmentType.Chain_Healing);
      if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_WeaponNecromancy) && !DataManager.instance.WeaponPool.Contains(EquipmentType.Chain_Nercomancy))
        DataManager.Instance.AddWeapon(EquipmentType.Chain_Nercomancy);
      if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_WeaponPoison) && !DataManager.instance.WeaponPool.Contains(EquipmentType.Chain_Poison))
        DataManager.Instance.AddWeapon(EquipmentType.Chain_Poison);
    }
    if (DataManager.instance.QuickStartActive && (!DataManager.instance.WinterModeActive || DataManager.instance.BuiltFurnace))
      DataManager.instance.OnboardingFinished = true;
    if (DataManager.instance.BossesCompleted.Count >= 4)
      DataManager.instance.OnboardingFinished = true;
    if (DataManager.instance.SozoTakenMushroom && DataManager.instance.SozoAteMushroomDay == -1 && Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.SEED_SOZO) <= 0)
    {
      bool flag = false;
      foreach (StructuresData baseStructure in DataManager.instance.BaseStructures)
      {
        if (baseStructure.Type == StructureBrain.TYPES.FARM_PLOT)
        {
          foreach (InventoryItem inventoryItem in baseStructure.Inventory)
          {
            if (inventoryItem.type == 160 /*0xA0*/)
              flag = true;
          }
        }
      }
      if (!flag)
        Inventory.AddItem(InventoryItem.ITEM_TYPE.SEED_SOZO, 1);
    }
    if (DataManager.instance.DisciplesCreated > 0 && !UpgradeSystem.GetUnlocked(UpgradeSystem.Type.DiscipleSystem) && !ObjectiveManager.GroupExists("Objectives/GroupTitles/Disciple"))
      UpgradeSystem.UnlockAbility(UpgradeSystem.Type.DiscipleSystem);
    for (int index1 = 0; index1 < DataManager.instance.Followers.Count; ++index1)
    {
      for (int index2 = 0; index2 < DataManager.instance.Followers.Count; ++index2)
      {
        if (index1 != index2 && (DataManager.instance.Followers[index1].Parents.Contains(DataManager.instance.Followers[index2].ID) || DataManager.instance.Followers[index2].Parents.Contains(DataManager.instance.Followers[index1].ID)))
        {
          IDAndRelationship relationship = FollowerBrain.GetOrCreateBrain(DataManager.instance.Followers[index1]).Info.GetOrCreateRelationship(DataManager.instance.Followers[index2].ID);
          if (relationship.CurrentRelationshipState == IDAndRelationship.RelationshipState.Lovers)
          {
            --relationship.Relationship;
            relationship.CurrentRelationshipState = IDAndRelationship.RelationshipState.Friends;
          }
        }
      }
    }
    if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Relic_Pack_Default) && !DataManager.instance.PlayerFoundRelics.Contains(RelicType.DealDamagePerFollower))
    {
      DataManager.UnlockRelic(RelicType.DealDamagePerFollower);
      DataManager.UnlockRelic(RelicType.HealPerFollower);
      DataManager.UnlockRelic(RelicType.DestroyTarotGainBuff);
    }
    if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Relics_Blessed_1) && !DataManager.instance.PlayerFoundRelics.Contains(RelicType.DealDamagePerFollower_Blessed))
    {
      DataManager.UnlockRelic(RelicType.DealDamagePerFollower_Blessed);
      DataManager.UnlockRelic(RelicType.HealPerFollower_Blessed);
      DataManager.UnlockRelic(RelicType.DestroyTarotGainBuff_Blessed);
    }
    if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Relics_Dammed_1) && !DataManager.instance.PlayerFoundRelics.Contains(RelicType.DealDamagePerFollower_Dammed))
    {
      DataManager.UnlockRelic(RelicType.DealDamagePerFollower_Dammed);
      DataManager.UnlockRelic(RelicType.HealPerFollower_Dammed);
      DataManager.UnlockRelic(RelicType.DestroyTarotGainBuff_Dammed);
    }
    if (!DataManager.Instance.HasAcceptedPilgrimPart1 && FollowerInfo.GetInfoByID(99998, true) != null)
      DataManager.Instance.HasAcceptedPilgrimPart1 = true;
    if (!DataManager.Instance.HasAcceptedPilgrimPart2 && FollowerInfo.GetInfoByID(99997, true) != null && FollowerInfo.GetInfoByID(99999, true) != null)
      DataManager.Instance.HasAcceptedPilgrimPart2 = true;
    if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ritual_FollowerWedding) || UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ritual_Wedding))
      UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Divorce);
    if (DataManager.Instance.BeatenWolf && !this.DiedToYngyaBoss && DataManager.Instance.TotalShrineGhostJuice < 80 /*0x50*/ && Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.YNGYA_GHOST) == 0 && this.DLCDungeonNodesCompleted.Contains(11) && !this.BossesCompleted.Contains(FollowerLocation.Boss_Wolf))
    {
      this.BossesCompleted.Add(FollowerLocation.Boss_Wolf);
      Inventory.AddItem(InventoryItem.ITEM_TYPE.YNGYA_GHOST, 4);
      StructuresData.CompleteResearch(StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_WOLF);
      StructuresData.SetRevealed(StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_WOLF);
      if (DataManager.Instance.GivenUpHeartToWolf)
      {
        ++DataManager.Instance.RedHeartShrineLevel;
        ++DataManager.Instance.PLAYER_HEALTH_MODIFIED;
      }
    }
    int num = 0;
    if (DataManager.instance.UnlockedFleeces.Contains(1))
      ++num;
    if (DataManager.instance.UnlockedFleeces.Contains(2))
      ++num;
    if (DataManager.instance.UnlockedFleeces.Contains(3))
      ++num;
    if (DataManager.instance.UnlockedFleeces.Contains(4))
      ++num;
    if (DataManager.instance.UnlockedFleeces.Contains(5))
      ++num;
    if (DataManager.instance.UnlockedFleeces.Contains(6))
      ++num;
    if (DataManager.instance.UnlockedFleeces.Contains(7))
      ++num;
    if (DataManager.instance.UnlockedFleeces.Contains(8))
      ++num;
    if (DataManager.instance.UnlockedFleeces.Contains(9))
      ++num;
    if (num == 8 && Inventory.KeyPieces == 3 && DataManager.Instance.TalismanPiecesReceivedFromMysticShop >= 12 && DataManager.instance.PlimboStoryProgress >= 11 && DataManager.instance.RatauKilled && DataManager.instance.RatooFishingProgress >= 3 && DataManager.instance.MidasFollowerStatueCount >= 4 && DataManager.instance.SozoStoryProgress >= 4)
    {
      Inventory.KeyPieces = 0;
      Inventory.AddItem(InventoryItem.ITEM_TYPE.TALISMAN, 1);
    }
    if (DataManager.instance.HasProducedChosenOne)
    {
      List<Structures_EggFollower> structuresOfType1 = StructureManager.GetAllStructuresOfType<Structures_EggFollower>();
      bool flag1 = true;
      foreach (Structures_EggFollower structuresEggFollower in structuresOfType1)
      {
        if (structuresEggFollower != null && structuresEggFollower.Data != null && structuresEggFollower.Data.EggInfo != null && structuresEggFollower.Data.EggInfo.Traits != null && structuresEggFollower.Data.EggInfo.Traits.Contains(FollowerTrait.TraitType.ChosenOne))
          flag1 = false;
      }
      if (flag1)
      {
        List<Structures_Hatchery> structuresOfType2 = StructureManager.GetAllStructuresOfType<Structures_Hatchery>();
        bool flag2 = true;
        foreach (Structures_Hatchery structuresHatchery in structuresOfType2)
        {
          if (structuresHatchery != null && structuresHatchery.Data != null && structuresHatchery.Data.EggInfo != null && structuresHatchery.Data.EggInfo.Traits != null && structuresHatchery.Data.EggInfo.Traits.Contains(FollowerTrait.TraitType.ChosenOne))
            flag2 = false;
        }
        if (flag2)
          DataManager.instance.HasProducedChosenOne = false;
      }
    }
    if (DataManager.instance.GaveChosenChildQuest && !DataManager.Instance.ChosenChildLeftInTheMidasCave && !ObjectiveManager.HasCustomObjectiveOfType(global::Objectives.CustomQuestTypes.LegendarySword) && !DataManager.Instance.FollowerSkinsUnlocked.Contains("ChosenChild"))
    {
      Objectives_Custom objective = new Objectives_Custom("Objectives/GroupTitles/Quest", global::Objectives.CustomQuestTypes.LegendarySword, 100000);
      objective.FailLocked = true;
      ObjectiveManager.Add((ObjectivesData) objective, true, true);
    }
    if (!DataManager.Instance.OnboardedBaseExpansion && (DataManager.instance.YngyaOffering >= 2 || DataManager.instance.YngyaOffering < 0))
      DataManager.Instance.OnboardedBaseExpansion = true;
    if (DataManager.Instance.NPCRescueRoomsCompleted >= 1 && Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.SPECIAL_WOOL_RANCHER) <= 0 && !DataManager.instance.NPCGhostRancherRescued)
      Inventory.AddItem(InventoryItem.ITEM_TYPE.SPECIAL_WOOL_RANCHER, 1);
    if (DataManager.Instance.NPCRescueRoomsCompleted >= 2)
    {
      if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.SPECIAL_WOOL_LAMBWAR) <= 0 && !DataManager.instance.NPCGhostFlockadeRescued)
        Inventory.AddItem(InventoryItem.ITEM_TYPE.SPECIAL_WOOL_LAMBWAR, 1);
      if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.SPECIAL_WOOL_TAROT) <= 0 && !DataManager.instance.NPCGhostTarotRescued)
        Inventory.AddItem(InventoryItem.ITEM_TYPE.SPECIAL_WOOL_TAROT, 1);
    }
    if (DataManager.Instance.NPCRescueRoomsCompleted >= 4)
    {
      if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.SPECIAL_WOOL_DECORATION) <= 0 && !DataManager.instance.NPCGhostDecoRescued)
        Inventory.AddItem(InventoryItem.ITEM_TYPE.SPECIAL_WOOL_DECORATION, 1);
      if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.SPECIAL_WOOL_BLACKSMITH) <= 0 && !DataManager.instance.NPCGhostBlacksmithRescued)
        Inventory.AddItem(InventoryItem.ITEM_TYPE.SPECIAL_WOOL_BLACKSMITH, 1);
    }
    if (DataManager.Instance.NPCRescueRoomsCompleted >= 6 && Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.SPECIAL_WOOL_6) <= 0 && !DataManager.instance.NPCGhostGraveyardRescued)
      Inventory.AddItem(InventoryItem.ITEM_TYPE.SPECIAL_WOOL_6, 1);
    if (DataManager.Instance.NPCRescueRoomsCompleted >= 7)
    {
      if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.SPECIAL_WOOL_7) <= 0 && !DataManager.instance.NPCGhostGeneric7Rescued)
        Inventory.AddItem(InventoryItem.ITEM_TYPE.SPECIAL_WOOL_8, 1);
      if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.SPECIAL_WOOL_8) <= 0 && !DataManager.instance.NPCGhostGeneric8Rescued)
        Inventory.AddItem(InventoryItem.ITEM_TYPE.SPECIAL_WOOL_8, 1);
    }
    if (DataManager.Instance.NPCRescueRoomsCompleted >= 9)
    {
      if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.SPECIAL_WOOL_9) <= 0 && !DataManager.instance.NPCGhostGeneric9Rescued)
        Inventory.AddItem(InventoryItem.ITEM_TYPE.SPECIAL_WOOL_9, 1);
      if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.SPECIAL_WOOL_10) <= 0 && !DataManager.instance.NPCGhostGeneric10Rescued)
        Inventory.AddItem(InventoryItem.ITEM_TYPE.SPECIAL_WOOL_10, 1);
    }
    if (DataManager.instance.FoundHollowKnightWool && !DataManager.instance.NPCGhostGeneric11Rescued && Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.SPECIAL_WOOL_11) <= 0)
      Inventory.AddItem(InventoryItem.ITEM_TYPE.SPECIAL_WOOL_11, 1);
    if (DataManager.instance.FullWoolhavenFlowerPots.Count >= 10 && DataManager.instance.HasYngyaFlowerBasketQuestAccepted && !DataManager.instance.HasFinishedYngyaFlowerBasketQuest)
      DataManager.instance.HasFinishedYngyaFlowerBasketQuest = true;
    if (DataManager.instance.GaveLeshyHealingQuest && !DataManager.instance.LeshyHealQuestCompleted && !ObjectiveManager.HasCustomObjectiveOfType(global::Objectives.CustomQuestTypes.HealingBishop_Leshy))
      ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/HealingBishop", global::Objectives.CustomQuestTypes.HealingBishop_Leshy, 99990), true);
    if (DataManager.instance.GaveHeketHealingQuest && !DataManager.instance.HeketHealQuestCompleted && !ObjectiveManager.HasCustomObjectiveOfType(global::Objectives.CustomQuestTypes.HealingBishop_Heket))
      ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/HealingBishop", global::Objectives.CustomQuestTypes.HealingBishop_Heket, 99991), true);
    if (DataManager.instance.GaveKallamarHealingQuest && !DataManager.instance.KallamarHealQuestCompleted && !ObjectiveManager.HasCustomObjectiveOfType(global::Objectives.CustomQuestTypes.HealingBishop_Kallamar))
      ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/HealingBishop", global::Objectives.CustomQuestTypes.HealingBishop_Kallamar, 99992), true);
    if (!DataManager.instance.GaveShamuraHealingQuest || DataManager.instance.ShamuraHealQuestCompleted || ObjectiveManager.HasCustomObjectiveOfType(global::Objectives.CustomQuestTypes.HealingBishop_Shamura))
      return;
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/HealingBishop", global::Objectives.CustomQuestTypes.HealingBishop_Shamura, 99993), true);
  }

  public void AddToCompletedQuestHistory(ObjectivesDataFinalized finalizedData)
  {
    this.CompletedObjectivesHistory.Insert(0, finalizedData);
    ObjectiveManager.MaintainObjectiveHistoryList(ref this.CompletedObjectivesHistory);
  }

  public void AddToFailedQuestHistory(ObjectivesDataFinalized finalizedData)
  {
    this.FailedObjectivesHistory.Insert(0, finalizedData);
    ObjectiveManager.MaintainObjectiveHistoryList(ref this.FailedObjectivesHistory);
  }

  [IgnoreMember]
  public List<InventoryItem> Food
  {
    get
    {
      List<InventoryItem> food = new List<InventoryItem>();
      foreach (InventoryItem inventoryItem in Inventory.items)
      {
        if (inventoryItem.type == 6 || inventoryItem.type == 21)
          food.Add(inventoryItem);
      }
      return food;
    }
  }

  public static WorshipperData.SkinAndData GetSkinAndDataFromString(string FollowerSkin)
  {
    foreach (WorshipperData.SkinAndData character in WorshipperData.Instance.Characters)
    {
      if (character.Skin[0].Skin == FollowerSkin)
        return character;
    }
    return (WorshipperData.SkinAndData) null;
  }

  public static bool CheckAvailableDecorations()
  {
    foreach (StructureBrain.TYPES allStructure in StructuresData.AllStructures)
    {
      if (StructuresData.GetCategory(allStructure) == StructureBrain.Categories.AESTHETIC && !StructuresData.GetUnlocked(allStructure) && !DataManager.OnDecorationBlacklist(allStructure))
        return true;
    }
    return false;
  }

  public static List<StructureBrain.TYPES> GetAvailableDecorations(bool excludeBlackList = true)
  {
    List<StructureBrain.TYPES> availableDecorations = new List<StructureBrain.TYPES>();
    foreach (StructureBrain.TYPES allStructure in StructuresData.AllStructures)
    {
      if (StructuresData.GetCategory(allStructure) == StructureBrain.Categories.AESTHETIC && !StructuresData.GetUnlocked(allStructure) && (!excludeBlackList || excludeBlackList && !DataManager.OnDecorationBlacklist(allStructure)))
        availableDecorations.Add(allStructure);
    }
    return availableDecorations;
  }

  public static bool OnDecorationBlacklist(StructureBrain.TYPES type)
  {
    if (type <= StructureBrain.TYPES.DECORATION_POST_BOX)
    {
      if (type <= StructureBrain.TYPES.DECORATION_TORCH)
      {
        if ((uint) (type - 34) > 1U && type != StructureBrain.TYPES.PLANK_PATH && (uint) (type - 102) > 1U)
          goto label_16;
      }
      else if (type <= StructureBrain.TYPES.DECORATION_SHRUB)
      {
        if ((uint) (type - 107) > 5U && type != StructureBrain.TYPES.DECORATION_SHRUB)
          goto label_16;
      }
      else if ((uint) (type - 182) > 8U && type != StructureBrain.TYPES.DECORATION_POST_BOX)
        goto label_16;
    }
    else if (type <= StructureBrain.TYPES.DECORATION_CNY_TREE)
    {
      if (type <= StructureBrain.TYPES.DECORATION_WALL_GRASS)
      {
        if (type != StructureBrain.TYPES.DECORATION_WALL_BONE && type != StructureBrain.TYPES.DECORATION_WALL_GRASS)
          goto label_16;
      }
      else
      {
        switch (type - 222)
        {
          case StructureBrain.TYPES.NONE:
          case StructureBrain.TYPES.BUILDER:
          case StructureBrain.TYPES.BLACKSMITH:
          case StructureBrain.TYPES.TAVERN:
          case StructureBrain.TYPES.FARM_STATION:
          case StructureBrain.TYPES.KITCHEN:
          case StructureBrain.TYPES.COOKED_FOOD_SILO:
          case StructureBrain.TYPES.CROP:
          case StructureBrain.TYPES.DEFENCE_TOWER:
          case StructureBrain.TYPES.GRASS:
          case StructureBrain.TYPES.GRAVE:
          case StructureBrain.TYPES.DEMOLISH_STRUCTURE:
          case StructureBrain.TYPES.MOVE_STRUCTURE:
          case StructureBrain.TYPES.TAROT_BUILDING:
          case StructureBrain.TYPES.PLANK_PATH:
          case StructureBrain.TYPES.PRISON:
          case StructureBrain.TYPES.GRAVE2:
          case StructureBrain.TYPES.RUBBLE:
          case StructureBrain.TYPES.WEEDS:
          case StructureBrain.TYPES.LUMBERJACK_STATION:
          case StructureBrain.TYPES.LUMBERJACK_STATION_2:
          case StructureBrain.TYPES.RESEARCH_1:
          case StructureBrain.TYPES.RESEARCH_2:
          case StructureBrain.TYPES.ONE:
          case StructureBrain.TYPES.TWO:
          case StructureBrain.TYPES.THREE:
          case StructureBrain.TYPES.SACRIFICIAL_TEMPLE_2:
          case StructureBrain.TYPES.COOKING_FIRE:
          case StructureBrain.TYPES.ALCHEMY_CAULDRON:
          case StructureBrain.TYPES.FOOD_STORAGE:
          case StructureBrain.TYPES.FOOD_STORAGE_2:
          case StructureBrain.TYPES.MATING_TENT:
          case StructureBrain.TYPES.BLOODSTONE_MINE:
          case StructureBrain.TYPES.BLOODSTONE_MINE_2:
          case StructureBrain.TYPES.CONFESSION_BOOTH:
          case StructureBrain.TYPES.DRUM_CIRCLE:
          case StructureBrain.TYPES.ENEMY_TRAP:
          case StructureBrain.TYPES.FISHING_HUT:
          case StructureBrain.TYPES.SECURITY_TURRET:
          case StructureBrain.TYPES.SECURITY_TURRET_2:
          case StructureBrain.TYPES.WITCH_DOCTOR:
          case StructureBrain.TYPES.MAYPOLE:
          case StructureBrain.TYPES.FLOWER_GARDEN:
          case StructureBrain.TYPES.BERRY_BUSH:
          case StructureBrain.TYPES.SLEEPING_BAG:
          case StructureBrain.TYPES.BLOOD_STONE:
          case StructureBrain.TYPES.POOP:
          case StructureBrain.TYPES.OUTPOST_SHRINE:
          case StructureBrain.TYPES.LUMBER_MINE:
          case StructureBrain.TYPES.COMPOST_BIN:
          case StructureBrain.TYPES.BLOODMOON_OFFERING:
          case StructureBrain.TYPES.DECORATION_LAMB_STATUE:
          case StructureBrain.TYPES.DECORATION_TORCH:
          case StructureBrain.TYPES.PUMPKIN_BUSH:
          case StructureBrain.TYPES.SACRIFICIAL_STONE:
            break;
          case StructureBrain.TYPES.BED:
          case StructureBrain.TYPES.PORTAL:
          case StructureBrain.TYPES.SACRIFICIAL_TEMPLE:
          case StructureBrain.TYPES.WOOD_STORE:
          case StructureBrain.TYPES.WHEAT_SILO:
          case StructureBrain.TYPES.NIGHTMARE_MACHINE:
          case StructureBrain.TYPES.MONSTER_HIVE:
          case StructureBrain.TYPES.TREE:
          case StructureBrain.TYPES.BUSH:
          case StructureBrain.TYPES.FIRE:
          case StructureBrain.TYPES.ROCK:
          case StructureBrain.TYPES.FOLLOWER_RECRUIT:
          case StructureBrain.TYPES.SEED_FLOWER:
          case StructureBrain.TYPES.COTTON_BUSH:
          case StructureBrain.TYPES.HEALING_BATH:
          case StructureBrain.TYPES.FIRE_PIT:
          case StructureBrain.TYPES.BUILD_PLOT:
          case StructureBrain.TYPES.SHRINE:
          case StructureBrain.TYPES.BARRACKS:
          case StructureBrain.TYPES.ASTROLOGIST:
          case StructureBrain.TYPES.STORAGE_PIT:
          case StructureBrain.TYPES.BUILD_SITE:
          case StructureBrain.TYPES.ALTAR:
          case StructureBrain.TYPES.PLACEMENT_REGION:
          case StructureBrain.TYPES.DECORATION_TREE:
          case StructureBrain.TYPES.DECORATION_STONE:
          case StructureBrain.TYPES.REPAIRABLE_HEARTS:
          case StructureBrain.TYPES.REPAIRABLE_ASTROLOGY:
          case StructureBrain.TYPES.REPAIRABLE_VOODOO:
          case StructureBrain.TYPES.REPAIRABLE_CURSES:
          case StructureBrain.TYPES.BED_2:
          case StructureBrain.TYPES.BED_3:
          case StructureBrain.TYPES.SHRINE_FUNDAMENTALIST:
          case StructureBrain.TYPES.SHRINE_MISFIT:
          case StructureBrain.TYPES.SHRINE_UTOPIANIST:
          case StructureBrain.TYPES.FARM_PLOT:
          case StructureBrain.TYPES.DEAD_WORSHIPPER:
          case StructureBrain.TYPES.VOMIT:
          case StructureBrain.TYPES.FARM_PLOT_SOZO:
          case StructureBrain.TYPES.MEAL:
          case StructureBrain.TYPES.BODY_PIT:
          case StructureBrain.TYPES.CULT_UPGRADE1:
          case StructureBrain.TYPES.DANCING_FIREPIT:
          case StructureBrain.TYPES.CULT_UPGRADE2:
          case StructureBrain.TYPES.GHOST_CIRCLE:
          case StructureBrain.TYPES.HIPPY_TENT:
          case StructureBrain.TYPES.HUNTERS_HUT:
          case StructureBrain.TYPES.KNUCKLEBONES_ARENA:
          case StructureBrain.TYPES.MEDITATION_MAT:
          case StructureBrain.TYPES.SCARIFICATIONIST:
          case StructureBrain.TYPES.OUTHOUSE:
            goto label_16;
          default:
            if ((uint) (type - 354) <= 11U)
              break;
            goto label_16;
        }
      }
    }
    else if (type <= StructureBrain.TYPES.DECORATION_PALWORLD_TREE)
    {
      if ((uint) (type - 376) > 7U && (uint) (type - 401) > 4U)
        goto label_16;
    }
    else if ((uint) (type - 435) > 1U)
    {
      switch (type - 465)
      {
        case StructureBrain.TYPES.NONE:
        case StructureBrain.TYPES.BUILDER:
        case StructureBrain.TYPES.BED:
        case StructureBrain.TYPES.PORTAL:
        case StructureBrain.TYPES.SACRIFICIAL_TEMPLE:
        case StructureBrain.TYPES.WOOD_STORE:
        case StructureBrain.TYPES.BLACKSMITH:
        case StructureBrain.TYPES.TAVERN:
        case StructureBrain.TYPES.FARM_STATION:
        case StructureBrain.TYPES.WHEAT_SILO:
        case StructureBrain.TYPES.KITCHEN:
        case StructureBrain.TYPES.COOKED_FOOD_SILO:
        case StructureBrain.TYPES.FOLLOWER_RECRUIT:
        case StructureBrain.TYPES.SEED_FLOWER:
        case StructureBrain.TYPES.COTTON_BUSH:
        case StructureBrain.TYPES.FIRE_PIT:
        case StructureBrain.TYPES.BUILD_PLOT:
        case StructureBrain.TYPES.SHRINE:
        case StructureBrain.TYPES.BARRACKS:
        case StructureBrain.TYPES.ASTROLOGIST:
        case StructureBrain.TYPES.STORAGE_PIT:
        case StructureBrain.TYPES.BED_2:
        case StructureBrain.TYPES.BED_3:
        case StructureBrain.TYPES.SHRINE_FUNDAMENTALIST:
        case StructureBrain.TYPES.SHRINE_MISFIT:
        case StructureBrain.TYPES.VOMIT:
        case StructureBrain.TYPES.DEMOLISH_STRUCTURE:
        case StructureBrain.TYPES.MOVE_STRUCTURE:
        case StructureBrain.TYPES.FARM_PLOT_SOZO:
          break;
        default:
          goto label_16;
      }
    }
    return true;
label_16:
    return false;
  }

  public List<StructureBrain.TYPES> GetDecorationListFromLocation(FollowerLocation location)
  {
    List<StructureBrain.TYPES> listFromLocation = new List<StructureBrain.TYPES>();
    switch (location)
    {
      case FollowerLocation.HubShore:
      case FollowerLocation.Dungeon1_1:
      case FollowerLocation.Boss_1:
        listFromLocation = DataManager.Instance.GetDecorationListFromCategory(DataManager.DecorationType.Dungeon1);
        break;
      case FollowerLocation.Dungeon1_2:
      case FollowerLocation.Boss_2:
      case FollowerLocation.Sozo:
        listFromLocation = DataManager.Instance.GetDecorationListFromCategory(DataManager.DecorationType.Mushroom);
        break;
      case FollowerLocation.Dungeon1_3:
      case FollowerLocation.Boss_3:
      case FollowerLocation.Dungeon_Decoration_Shop1:
        listFromLocation = DataManager.Instance.GetDecorationListFromCategory(DataManager.DecorationType.Crystal);
        break;
      case FollowerLocation.Dungeon1_4:
      case FollowerLocation.Boss_4:
      case FollowerLocation.Dungeon_Location_4:
        listFromLocation = DataManager.Instance.GetDecorationListFromCategory(DataManager.DecorationType.Spider);
        break;
      case FollowerLocation.Dungeon1_5:
      case FollowerLocation.Boss_Wolf:
        listFromLocation = DataManager.Instance.GetDecorationListFromCategory(DataManager.DecorationType.Wolf);
        break;
      case FollowerLocation.Dungeon1_6:
      case FollowerLocation.Boss_Yngya:
        listFromLocation = DataManager.Instance.GetDecorationListFromCategory(DataManager.DecorationType.Rot);
        break;
      case FollowerLocation.DLC_ShrineRoom:
        listFromLocation = DataManager.Instance.GetDecorationListFromCategory(DataManager.DecorationType.Woolhaven);
        listFromLocation.AddRange((IEnumerable<StructureBrain.TYPES>) DataManager.Instance.GetDecorationListFromCategory(DataManager.DecorationType.Rot));
        listFromLocation.AddRange((IEnumerable<StructureBrain.TYPES>) DataManager.Instance.GetDecorationListFromCategory(DataManager.DecorationType.Wolf));
        break;
    }
    if (listFromLocation.Count <= 0)
      listFromLocation = DataManager.Instance.GetDecorationListFromCategory(DataManager.DecorationType.All);
    return listFromLocation;
  }

  public List<StructureBrain.TYPES> GetDecorationListFromCategory(
    DataManager.DecorationType category)
  {
    List<StructureBrain.TYPES> availableDecorations = DataManager.GetAvailableDecorations();
    List<StructureBrain.TYPES> typesList = new List<StructureBrain.TYPES>();
    foreach (StructureBrain.TYPES type in availableDecorations)
    {
      if (DataManager.GetDecorationType(type) == category || DataManager.GetDecorationType(type) == DataManager.DecorationType.Path)
        typesList.Add(type);
    }
    return typesList.Count > 0 ? typesList : new List<StructureBrain.TYPES>();
  }

  public static DecorationCost GetDecorationTypeCost(DataManager.DecorationType type)
  {
    DecorationCost decorationTypeCost = new DecorationCost();
    switch (type)
    {
      case DataManager.DecorationType.Dungeon1:
        decorationTypeCost.costAmount = 25;
        decorationTypeCost.costType = InventoryItem.ITEM_TYPE.BLACK_GOLD;
        return decorationTypeCost;
      case DataManager.DecorationType.Mushroom:
        decorationTypeCost.costAmount = 50;
        decorationTypeCost.costType = InventoryItem.ITEM_TYPE.BLACK_GOLD;
        return decorationTypeCost;
      case DataManager.DecorationType.Crystal:
        decorationTypeCost.costAmount = 100;
        decorationTypeCost.costType = InventoryItem.ITEM_TYPE.BLACK_GOLD;
        return decorationTypeCost;
      case DataManager.DecorationType.Spider:
        decorationTypeCost.costAmount = 2;
        decorationTypeCost.costType = InventoryItem.ITEM_TYPE.GOLD_REFINED;
        return decorationTypeCost;
      case DataManager.DecorationType.All:
        decorationTypeCost.costAmount = 25;
        decorationTypeCost.costType = InventoryItem.ITEM_TYPE.BLACK_GOLD;
        return decorationTypeCost;
      case DataManager.DecorationType.Rot:
      case DataManager.DecorationType.Wolf:
      case DataManager.DecorationType.Woolhaven:
        decorationTypeCost.costAmount = 20;
        decorationTypeCost.costType = InventoryItem.ITEM_TYPE.WOOL;
        return decorationTypeCost;
      default:
        decorationTypeCost.costAmount = 50;
        decorationTypeCost.costType = InventoryItem.ITEM_TYPE.BLACK_GOLD;
        return decorationTypeCost;
    }
  }

  public static string GetDecorationLocation(StructureBrain.TYPES type)
  {
    switch (DataManager.GetDecorationType(type))
    {
      case DataManager.DecorationType.Dungeon1:
        return ScriptLocalization.NAMES_Places.Dungeon1_1;
      case DataManager.DecorationType.Mushroom:
        return ScriptLocalization.NAMES_Places.Dungeon1_2;
      case DataManager.DecorationType.Crystal:
        return ScriptLocalization.NAMES_Places.Dungeon1_3;
      case DataManager.DecorationType.Spider:
        return ScriptLocalization.NAMES_Places.Dungeon1_4;
      case DataManager.DecorationType.All:
        return "";
      case DataManager.DecorationType.Path:
        return ScriptLocalization.UI.Paths;
      case DataManager.DecorationType.Rot:
        return LocalizationManager.GetTranslation("NAMES/Places/Dungeon1_6");
      case DataManager.DecorationType.Wolf:
        return LocalizationManager.GetTranslation("NAMES/Places/Dungeon1_5");
      case DataManager.DecorationType.Woolhaven:
        return LocalizationManager.GetTranslation("NAMES/Places/LambTown");
      default:
        return "";
    }
  }

  public static DataManager.DecorationType GetDecorationType(StructureBrain.TYPES type)
  {
    if (DataManager.DecorationsForType(DataManager.DecorationType.All).Contains(type))
      return DataManager.DecorationType.All;
    if (DataManager.DecorationsForType(DataManager.DecorationType.Dungeon1).Contains(type))
      return DataManager.DecorationType.Dungeon1;
    if (DataManager.DecorationsForType(DataManager.DecorationType.Mushroom).Contains(type))
      return DataManager.DecorationType.Mushroom;
    if (DataManager.DecorationsForType(DataManager.DecorationType.Crystal).Contains(type))
      return DataManager.DecorationType.Crystal;
    if (DataManager.DecorationsForType(DataManager.DecorationType.Spider).Contains(type))
      return DataManager.DecorationType.Spider;
    if (DataManager.DecorationsForType(DataManager.DecorationType.Wolf).Contains(type))
      return DataManager.DecorationType.Wolf;
    if (DataManager.DecorationsForType(DataManager.DecorationType.Rot).Contains(type))
      return DataManager.DecorationType.Rot;
    if (DataManager.DecorationsForType(DataManager.DecorationType.Woolhaven).Contains(type))
      return DataManager.DecorationType.Woolhaven;
    return DataManager.DecorationsForType(DataManager.DecorationType.Path).Contains(type) ? DataManager.DecorationType.Path : DataManager.DecorationType.None;
  }

  public static List<StructureBrain.TYPES> MajorDLCDecorationsDisplay(
    DataManager.DecorationMajorDLCGrouping decorationMajorDLCGrouping)
  {
    switch (decorationMajorDLCGrouping)
    {
      case DataManager.DecorationMajorDLCGrouping.Major_DLC:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.DECORATION_DLC_YNGYA_CANDLE,
          StructureBrain.TYPES.DECORATION_DLC_YNGYA_FLOWERBUCKET,
          StructureBrain.TYPES.DECORATION_DLC_YNGYA_STICKBUNDLE,
          StructureBrain.TYPES.DECORATION_DLC_YNGYA_TALLFLOWERS,
          StructureBrain.TYPES.DECORATION_DLC_YNGYA_TREEBUSH,
          StructureBrain.TYPES.DECORATION_DLC_YNGYA_TREEPOT,
          StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_CHIMNEY_1,
          StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_CHIMNEY_2,
          StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_LAMPPOST,
          StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_CLOCK,
          StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_WALL,
          StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_PLANT
        };
      case DataManager.DecorationMajorDLCGrouping.Woolhaven:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BUSH1,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BUSH2,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_FOUNTAIN,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_SMALLSTATUE1,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_SMALLSTATUE2,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_STREETLAMP,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_TREE,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_WALL,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_FLOOR,
          StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_WOLF,
          StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_YNGYA,
          StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_EXECUTIONER,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_JUG,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_RUG,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BARREL1,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BARREL2,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_CANDLE,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BELLS,
          StructureBrain.TYPES.TILE_WATER,
          StructureBrain.TYPES.DECORATION_EASTEREGG_EGG,
          StructureBrain.TYPES.DECORATION_EASTEREGG_HAROSTATUE,
          StructureBrain.TYPES.DECORATION_EASTEREGG_TURUA,
          StructureBrain.TYPES.DECORATION_EASTEREGG_WARRACKA
        };
      case DataManager.DecorationMajorDLCGrouping.Ewefall:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.DECORATION_DLC_WOLF_BULB,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_CULTIST1,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_CULTIST2,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_DIORAMA,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_FIREPIT,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_LAMPPOST,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_PILLAR1,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_PILLAR2,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE1,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE2,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE3,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE4,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE5,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_TESLA,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_TREE1,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_WIRES
        };
      case DataManager.DecorationMajorDLCGrouping.Rot:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.DECORATION_DLC_ROT_BOTTLE,
          StructureBrain.TYPES.DECORATION_DLC_ROT_BUCKET,
          StructureBrain.TYPES.DECORATION_DLC_ROT_CAGE1,
          StructureBrain.TYPES.DECORATION_DLC_ROT_CAGE2,
          StructureBrain.TYPES.DECORATION_DLC_ROT_CAULDRON,
          StructureBrain.TYPES.DECORATION_DLC_ROT_DIORAMA,
          StructureBrain.TYPES.DECORATION_DLC_ROT_FIREMACHINE,
          StructureBrain.TYPES.DECORATION_DLC_ROT_FLOOR,
          StructureBrain.TYPES.DECORATION_DLC_ROT_IRONMAIDEN,
          StructureBrain.TYPES.DECORATION_DLC_ROT_LUMP1,
          StructureBrain.TYPES.DECORATION_DLC_ROT_LUMP2,
          StructureBrain.TYPES.DECORATION_DLC_ROT_PILLAR1,
          StructureBrain.TYPES.DECORATION_DLC_ROT_PILLAR2,
          StructureBrain.TYPES.DECORATION_DLC_ROT_STONE1,
          StructureBrain.TYPES.DECORATION_DLC_ROT_STONE2,
          StructureBrain.TYPES.DECORATION_DLC_ROT_TENTACLE,
          StructureBrain.TYPES.DECORATION_DLC_ROT_WALL
        };
      default:
        return new List<StructureBrain.TYPES>();
    }
  }

  public static List<StructureBrain.TYPES> DecorationsForType(
    DataManager.DecorationType decorationType)
  {
    switch (decorationType)
    {
      case DataManager.DecorationType.Dungeon1:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.FARM_PLOT_SIGN,
          StructureBrain.TYPES.DECORATION_BARROW,
          StructureBrain.TYPES.DECORATION_WALL_STONE,
          StructureBrain.TYPES.DECORATION_TORCH_BIG,
          StructureBrain.TYPES.DECORATION_HAY_BALE,
          StructureBrain.TYPES.DECORATION_HAY_PILE,
          StructureBrain.TYPES.DECORATION_STONE_FLAG,
          StructureBrain.TYPES.DECORATION_BOSS_TROPHY_1
        };
      case DataManager.DecorationType.Mushroom:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.DECORATION_MUSHROOM_1,
          StructureBrain.TYPES.DECORATION_MUSHROOM_CANDLE_1,
          StructureBrain.TYPES.DECORATION_MUSHROOM_SCULPTURE,
          StructureBrain.TYPES.DECORATION_FLAG_MUSHROOM,
          StructureBrain.TYPES.DECORATION_STONE_MUSHROOM,
          StructureBrain.TYPES.DECORATION_PUMPKIN_PILE,
          StructureBrain.TYPES.DECORATION_PUMPKIN_STOOL,
          StructureBrain.TYPES.DECORATION_BOSS_TROPHY_2
        };
      case DataManager.DecorationType.Crystal:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.DECORATION_CRYSTAL_LAMP,
          StructureBrain.TYPES.DECORATION_CRYSTAL_LIGHT,
          StructureBrain.TYPES.DECORATION_CRYSTAL_ROCK,
          StructureBrain.TYPES.DECORATION_CRYSTAL_STATUE,
          StructureBrain.TYPES.DECORATION_CRYSTAL_WINDOW,
          StructureBrain.TYPES.DECORATION_FLAG_CRYSTAL,
          StructureBrain.TYPES.DECORATION_BOSS_TROPHY_3,
          StructureBrain.TYPES.DECORATION_CRYSTAL_TREE
        };
      case DataManager.DecorationType.Spider:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.DECORATION_WALL_SPIDER,
          StructureBrain.TYPES.DECORATION_BONE_SKULL_BIG,
          StructureBrain.TYPES.DECORATION_BONE_SKULL_PILE,
          StructureBrain.TYPES.DECORATION_SPIDER_LANTERN,
          StructureBrain.TYPES.DECORATION_SPIDER_PILLAR,
          StructureBrain.TYPES.DECORATION_SPIDER_TORCH,
          StructureBrain.TYPES.DECORATION_SPIDER_WEB_CROWN_SCULPTURE,
          StructureBrain.TYPES.DECORATION_BOSS_TROPHY_4
        };
      case DataManager.DecorationType.All:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.DECORATION_TWIG_LAMP,
          StructureBrain.TYPES.DECORATION_FOUNTAIN,
          StructureBrain.TYPES.DECORATION_WREATH_STICK,
          StructureBrain.TYPES.DECORATION_STUMP_LAMB_STATUE,
          StructureBrain.TYPES.DECORATION_STONE_HENGE,
          StructureBrain.TYPES.DECORATION_POND,
          StructureBrain.TYPES.DECORATION_LEAFY_FLOWER_SCULPTURE,
          StructureBrain.TYPES.DECORATION_LEAFY_LANTERN,
          StructureBrain.TYPES.DECORATION_LAMB_FLAG_STATUE,
          StructureBrain.TYPES.DECORATION_STONE_CANDLE,
          StructureBrain.TYPES.DECORATION_CANDLE_BARREL,
          StructureBrain.TYPES.DECORATION_FLOWER_ARCH,
          StructureBrain.TYPES.DECORATION_SMALL_STONE_CANDLE,
          StructureBrain.TYPES.DECORATION_FLOWER_BOX_1,
          StructureBrain.TYPES.DECORATION_BONE_ARCH,
          StructureBrain.TYPES.DECORATION_BONE_BARREL,
          StructureBrain.TYPES.DECORATION_BONE_CANDLE,
          StructureBrain.TYPES.DECORATION_BONE_FLAG,
          StructureBrain.TYPES.DECORATION_BONE_LANTERN,
          StructureBrain.TYPES.DECORATION_BONE_PILLAR,
          StructureBrain.TYPES.DECORATION_BONE_SCULPTURE,
          StructureBrain.TYPES.DECORATION_WALL_TWIGS,
          StructureBrain.TYPES.DECORATION_WALL_GRASS,
          StructureBrain.TYPES.DECORATION_TREE,
          StructureBrain.TYPES.DECORATION_STONE,
          StructureBrain.TYPES.DECORATION_FLAG_CROWN,
          StructureBrain.TYPES.DECORATION_WALL_BONE,
          StructureBrain.TYPES.DECORATION_MONSTERSHRINE,
          StructureBrain.TYPES.DECORATION_BOSS_TROPHY_5
        };
      case DataManager.DecorationType.Path:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.TILE_PATH,
          StructureBrain.TYPES.TILE_HAY,
          StructureBrain.TYPES.TILE_BLOOD,
          StructureBrain.TYPES.TILE_ROCKS,
          StructureBrain.TYPES.TILE_BRICKS,
          StructureBrain.TYPES.TILE_PLANKS,
          StructureBrain.TYPES.TILE_GOLD,
          StructureBrain.TYPES.TILE_MOSAIC,
          StructureBrain.TYPES.TILE_FLOWERSROCKY,
          StructureBrain.TYPES.DECORATION_DLC_ROT_FLOOR,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_FLOOR
        };
      case DataManager.DecorationType.DLC:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.DECORATION_FLOWERPOTWALL,
          StructureBrain.TYPES.DECORATION_LEAFYLAMPPOST,
          StructureBrain.TYPES.DECORATION_FLOWERVASE,
          StructureBrain.TYPES.DECORATION_WATERINGCAN,
          StructureBrain.TYPES.DECORATION_FLOWER_CART_SMALL,
          StructureBrain.TYPES.DECORATION_WEEPINGSHRINE,
          StructureBrain.TYPES.TILE_FLOWERS,
          StructureBrain.TYPES.DECORATION_OLDFAITH_CRYSTAL,
          StructureBrain.TYPES.DECORATION_OLDFAITH_FLAG,
          StructureBrain.TYPES.DECORATION_OLDFAITH_FOUNTAIN,
          StructureBrain.TYPES.DECORATION_OLDFAITH_IRONMAIDEN,
          StructureBrain.TYPES.DECORATION_OLDFAITH_SHRINE,
          StructureBrain.TYPES.DECORATION_OLDFAITH_TORCH,
          StructureBrain.TYPES.DECORATION_OLDFAITH_WALL,
          StructureBrain.TYPES.TILE_OLDFAITH,
          StructureBrain.TYPES.DECORATION_TWITCH_FLAG_CROWN,
          StructureBrain.TYPES.DECORATION_TWITCH_MUSHROOM_BAG,
          StructureBrain.TYPES.DECORATION_TWITCH_ROSE_BUSH,
          StructureBrain.TYPES.DECORATION_TWITCH_STONE_FLAG,
          StructureBrain.TYPES.DECORATION_TWITCH_STONE_STATUE,
          StructureBrain.TYPES.DECORATION_TWITCH_WOODEN_GUARDIAN,
          StructureBrain.TYPES.DECORATION_CHRISTMAS_TREE,
          StructureBrain.TYPES.DECORATION_CHRISTMAS_SNOWMAN,
          StructureBrain.TYPES.DECORATION_CHRISTMAS_CANDLE,
          StructureBrain.TYPES.DECORATION_CHRISTMAS_CANDYCANE,
          StructureBrain.TYPES.DECORATION_SINFUL_STATUE,
          StructureBrain.TYPES.DECORATION_SINFUL_CRUCIFIX,
          StructureBrain.TYPES.DECORATION_SINFUL_FLOWERS1,
          StructureBrain.TYPES.DECORATION_SINFUL_FLOWERS2,
          StructureBrain.TYPES.DECORATION_SINFUL_SKULL,
          StructureBrain.TYPES.DECORATION_SINFUL_INCENSE,
          StructureBrain.TYPES.DECORATION_PILGRIM_WALL,
          StructureBrain.TYPES.DECORATION_PILGRIM_BONSAI,
          StructureBrain.TYPES.DECORATION_PILGRIM_LANTERN,
          StructureBrain.TYPES.DECORATION_PILGRIM_PAGODA,
          StructureBrain.TYPES.DECORATION_PILGRIM_VASE,
          StructureBrain.TYPES.DECORATION_PLUSH
        };
      case DataManager.DecorationType.Special_Events:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.DECORATION_HALLOWEEN_CANDLE,
          StructureBrain.TYPES.DECORATION_HALLOWEEN_SKULL,
          StructureBrain.TYPES.DECORATION_HALLOWEEN_TREE,
          StructureBrain.TYPES.DECORATION_HALLOWEEN_PUMPKIN,
          StructureBrain.TYPES.DECORATION_DST_ALCHEMY,
          StructureBrain.TYPES.DECORATION_DST_DEERCLOPS,
          StructureBrain.TYPES.DECORATION_DST_MARBLETREE,
          StructureBrain.TYPES.DECORATION_DST_PIGSTICK,
          StructureBrain.TYPES.DECORATION_DST_SCIENCEMACHINE,
          StructureBrain.TYPES.DECORATION_DST_TREE,
          StructureBrain.TYPES.DECORATION_DST_WALL,
          StructureBrain.TYPES.DECORATION_DST_BEEFALOSKELETON,
          StructureBrain.TYPES.DECORATION_DST_GLOMMERSTATUE,
          StructureBrain.TYPES.DECORATION_PALWORLD_LAMB,
          StructureBrain.TYPES.DECORATION_PALWORLD_LANTERN,
          StructureBrain.TYPES.DECORATION_PALWORLD_PLANT,
          StructureBrain.TYPES.DECORATION_PALWORLD_STATUE,
          StructureBrain.TYPES.DECORATION_PALWORLD_TREE,
          StructureBrain.TYPES.DECORATION_CNY_TREE,
          StructureBrain.TYPES.DECORATION_CNY_DRAGON,
          StructureBrain.TYPES.DECORATION_CNY_LANTERN,
          StructureBrain.TYPES.DECORATION_GNOME1,
          StructureBrain.TYPES.DECORATION_GNOME2,
          StructureBrain.TYPES.DECORATION_GNOME3,
          StructureBrain.TYPES.DECORATION_GOAT_LANTERN,
          StructureBrain.TYPES.DECORATION_GOAT_STATUE,
          StructureBrain.TYPES.DECORATION_GOAT_PLANT,
          StructureBrain.TYPES.DECORATION_STONE_CANDLE_LAMP,
          StructureBrain.TYPES.DECORATION_POST_BOX,
          StructureBrain.TYPES.DECORATION_FLOWER_CART,
          StructureBrain.TYPES.DECORATION_FLOWER_BOTTLE,
          StructureBrain.TYPES.DECORATION_MUSHROOM_CANDLE_2,
          StructureBrain.TYPES.DECORATION_LEAFY_SCULPTURE
        };
      case DataManager.DecorationType.Rot:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.DECORATION_DLC_ROT_BOTTLE,
          StructureBrain.TYPES.DECORATION_DLC_ROT_BUCKET,
          StructureBrain.TYPES.DECORATION_DLC_ROT_CAGE1,
          StructureBrain.TYPES.DECORATION_DLC_ROT_CAGE2,
          StructureBrain.TYPES.DECORATION_DLC_ROT_CAULDRON,
          StructureBrain.TYPES.DECORATION_DLC_ROT_DIORAMA,
          StructureBrain.TYPES.DECORATION_DLC_ROT_FIREMACHINE,
          StructureBrain.TYPES.DECORATION_DLC_ROT_FLOOR,
          StructureBrain.TYPES.DECORATION_DLC_ROT_IRONMAIDEN,
          StructureBrain.TYPES.DECORATION_DLC_ROT_LUMP1,
          StructureBrain.TYPES.DECORATION_DLC_ROT_LUMP2,
          StructureBrain.TYPES.DECORATION_DLC_ROT_PILLAR1,
          StructureBrain.TYPES.DECORATION_DLC_ROT_PILLAR2,
          StructureBrain.TYPES.DECORATION_DLC_ROT_STONE1,
          StructureBrain.TYPES.DECORATION_DLC_ROT_STONE2,
          StructureBrain.TYPES.DECORATION_DLC_ROT_TENTACLE,
          StructureBrain.TYPES.DECORATION_DLC_ROT_WALL
        };
      case DataManager.DecorationType.Wolf:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.DECORATION_DLC_WOLF_BULB,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_CULTIST1,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_CULTIST2,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_DIORAMA,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_FIREPIT,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_LAMPPOST,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_PILLAR1,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_PILLAR2,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE1,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE2,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE3,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE4,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE5,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_TESLA,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_TREE1,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_WIRES,
          StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_CHIMNEY_1,
          StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_CHIMNEY_2,
          StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_LAMPPOST,
          StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_CLOCK,
          StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_WALL,
          StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_PLANT
        };
      case DataManager.DecorationType.Major_DLC:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.DECORATION_DLC_WOLF_BULB,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_CULTIST1,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_CULTIST2,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_DIORAMA,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_FIREPIT,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_LAMPPOST,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_PILLAR1,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_PILLAR2,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE1,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE2,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE3,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE4,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_STATUE5,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_TESLA,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_TREE1,
          StructureBrain.TYPES.DECORATION_DLC_WOLF_WIRES,
          StructureBrain.TYPES.DECORATION_DLC_ROT_BOTTLE,
          StructureBrain.TYPES.DECORATION_DLC_ROT_BUCKET,
          StructureBrain.TYPES.DECORATION_DLC_ROT_CAGE1,
          StructureBrain.TYPES.DECORATION_DLC_ROT_CAGE2,
          StructureBrain.TYPES.DECORATION_DLC_ROT_CAULDRON,
          StructureBrain.TYPES.DECORATION_DLC_ROT_DIORAMA,
          StructureBrain.TYPES.DECORATION_DLC_ROT_FIREMACHINE,
          StructureBrain.TYPES.DECORATION_DLC_ROT_FLOOR,
          StructureBrain.TYPES.DECORATION_DLC_ROT_IRONMAIDEN,
          StructureBrain.TYPES.DECORATION_DLC_ROT_LUMP1,
          StructureBrain.TYPES.DECORATION_DLC_ROT_LUMP2,
          StructureBrain.TYPES.DECORATION_DLC_ROT_PILLAR1,
          StructureBrain.TYPES.DECORATION_DLC_ROT_PILLAR2,
          StructureBrain.TYPES.DECORATION_DLC_ROT_STONE1,
          StructureBrain.TYPES.DECORATION_DLC_ROT_STONE2,
          StructureBrain.TYPES.DECORATION_DLC_ROT_TENTACLE,
          StructureBrain.TYPES.DECORATION_DLC_ROT_WALL,
          StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_CHIMNEY_1,
          StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_CHIMNEY_2,
          StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_LAMPPOST,
          StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_CLOCK,
          StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_WALL,
          StructureBrain.TYPES.DECORATION_DLC_STEAMPUNK_PLANT,
          StructureBrain.TYPES.DECORATION_DLC_YNGYA_CANDLE,
          StructureBrain.TYPES.DECORATION_DLC_YNGYA_FLOWERBUCKET,
          StructureBrain.TYPES.DECORATION_DLC_YNGYA_STICKBUNDLE,
          StructureBrain.TYPES.DECORATION_DLC_YNGYA_TALLFLOWERS,
          StructureBrain.TYPES.DECORATION_DLC_YNGYA_TREEBUSH,
          StructureBrain.TYPES.DECORATION_DLC_YNGYA_TREEPOT,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BUSH1,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BUSH2,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_FOUNTAIN,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_SMALLSTATUE1,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_SMALLSTATUE2,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_STREETLAMP,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_TREE,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_WALL,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_FLOOR,
          StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_WOLF,
          StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_YNGYA,
          StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_EXECUTIONER,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_JUG,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_RUG,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BARREL1,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BARREL2,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_CANDLE,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BELLS,
          StructureBrain.TYPES.TILE_WATER,
          StructureBrain.TYPES.DECORATION_EASTEREGG_EGG,
          StructureBrain.TYPES.DECORATION_EASTEREGG_HAROSTATUE,
          StructureBrain.TYPES.DECORATION_EASTEREGG_TURUA,
          StructureBrain.TYPES.DECORATION_EASTEREGG_WARRACKA
        };
      case DataManager.DecorationType.Woolhaven:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BUSH1,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BUSH2,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_FOUNTAIN,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_SMALLSTATUE1,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_SMALLSTATUE2,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_STREETLAMP,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_TREE,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_WALL,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_FLOOR,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_JUG,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_RUG,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BARREL1,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BARREL2,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_CANDLE,
          StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_BELLS,
          StructureBrain.TYPES.DECORATION_EASTEREGG_EGG,
          StructureBrain.TYPES.DECORATION_EASTEREGG_HAROSTATUE,
          StructureBrain.TYPES.DECORATION_EASTEREGG_TURUA,
          StructureBrain.TYPES.DECORATION_EASTEREGG_WARRACKA
        };
      default:
        return (List<StructureBrain.TYPES>) null;
    }
  }

  public static StructureBrain.TYPES GetRandomLockedDecoration()
  {
    List<StructureBrain.TYPES> typesList = new List<StructureBrain.TYPES>();
    foreach (StructureBrain.TYPES allStructure in StructuresData.AllStructures)
    {
      if (StructuresData.GetCategory(allStructure) == StructureBrain.Categories.AESTHETIC && !StructuresData.GetUnlocked(allStructure) && !DataManager.OnDecorationBlacklist(allStructure))
        typesList.Add(allStructure);
    }
    return typesList.Count > 0 ? typesList[UnityEngine.Random.Range(0, typesList.Count - 1)] : StructureBrain.TYPES.NONE;
  }

  public static bool CheckIfThereAreSkinsAvailable()
  {
    List<string> stringList = new List<string>();
    foreach (WorshipperData.SkinAndData skinAndData in WorshipperData.Instance.GetSkinsFromLocation(WorshipperData.DropLocation.Other))
    {
      if (!skinAndData.Skin[0].Skin.Contains("Boss") && !DataManager.OnBlackList(skinAndData.Skin[0].Skin) && !DataManager.GetFollowerSkinUnlocked(skinAndData.Skin[0].Skin))
        stringList.Add(skinAndData.Skin[0].Skin);
    }
    foreach (WorshipperData.SkinAndData skinAndData in WorshipperData.Instance.GetSkinsFromFollowerLocation(PlayerFarming.Location))
    {
      if (!skinAndData.Skin[0].Skin.Contains("Boss") && !DataManager.OnBlackList(skinAndData.Skin[0].Skin) && !DataManager.GetFollowerSkinUnlocked(skinAndData.Skin[0].Skin))
        stringList.Add(skinAndData.Skin[0].Skin);
    }
    return stringList.Count > 0;
  }

  public static bool CheckIfThereAreSkinsAvailableAll()
  {
    List<string> stringList = new List<string>();
    foreach (WorshipperData.SkinAndData skinAndData in WorshipperData.Instance.GetSkinsAll())
    {
      if (!DataManager.GetFollowerSkinUnlocked(skinAndData.Skin[0].Skin) && !stringList.Contains(skinAndData.Skin[0].Skin) && !DataManager.instance.DLCSkins.Contains(skinAndData.Skin[0].Skin) && !DataManager.instance.SpecialEventSkins.Contains<string>(skinAndData.Skin[0].Skin) && !DataManager.instance.MajorDLCSkins.Contains(skinAndData.Skin[0].Skin))
        stringList.Add(skinAndData.Skin[0].Skin);
    }
    return stringList.Count > 0;
  }

  public static List<string> AvailableSkins()
  {
    List<string> stringList = new List<string>();
    foreach (WorshipperData.SkinAndData character in WorshipperData.Instance.Characters)
    {
      if (!DataManager.OnBlackList(character.Title))
        stringList.Add(character.Title);
    }
    return stringList;
  }

  public static bool OnBlackList(string skin)
  {
    if (skin.Contains("Boss"))
      return true;
    foreach (string str in DataManager.Instance.FollowerSkinsBlacklist)
    {
      if (str == skin)
        return true;
    }
    return false;
  }

  public static bool IsAllJobBoardsComplete => DataManager.GetCompletedJobBoardCount() >= 6;

  public static int GetCompletedJobBoardCount()
  {
    int num = DataManager.Instance.CompletedBlacksmithJobBoard ? 1 : 0;
    bool completedTarotJobBoard = DataManager.Instance.CompletedTarotJobBoard;
    bool completedDecoJobBoard = DataManager.Instance.CompletedDecoJobBoard;
    bool flockadeJobBoard = DataManager.Instance.CompletedFlockadeJobBoard;
    bool graveyardJobBoard = DataManager.Instance.CompletedGraveyardJobBoard;
    bool ranchingJobBoard = DataManager.Instance.CompletedRanchingJobBoard;
    int completedJobBoardCount = 0;
    if (num != 0)
      ++completedJobBoardCount;
    if (completedTarotJobBoard)
      ++completedJobBoardCount;
    if (completedDecoJobBoard)
      ++completedJobBoardCount;
    if (flockadeJobBoard)
      ++completedJobBoardCount;
    if (graveyardJobBoard)
      ++completedJobBoardCount;
    if (ranchingJobBoard)
      ++completedJobBoardCount;
    return completedJobBoardCount;
  }

  public static string GetRandomUnlockedSkin()
  {
    List<string> stringList = new List<string>();
    foreach (string str in DataManager.Instance.FollowerSkinsUnlocked)
    {
      if (!str.Contains("Boss"))
        stringList.Add(str);
    }
    return stringList[UnityEngine.Random.Range(0, stringList.Count)];
  }

  public static string GetRandomSkin()
  {
    List<string> stringList = new List<string>();
    foreach (WorshipperData.SkinAndData skinAndData in WorshipperData.Instance.GetSkinsFromFollowerLocation(PlayerFarming.Location))
    {
      if (!skinAndData.Skin[0].Skin.Contains("Boss") && !DataManager.OnBlackList(skinAndData.Skin[0].Skin))
        stringList.Add(skinAndData.Skin[0].Skin);
    }
    if (stringList.Count == 0)
    {
      foreach (WorshipperData.SkinAndData skinAndData in WorshipperData.Instance.GetSkinsFromLocation(WorshipperData.DropLocation.Other))
      {
        if (!skinAndData.Skin[0].Skin.Contains("Boss") && !DataManager.OnBlackList(skinAndData.Skin[0].Skin))
          stringList.Add(skinAndData.Skin[0].Skin);
      }
    }
    if (stringList.Count > 0)
      return stringList[UnityEngine.Random.Range(0, stringList.Count)];
    Debug.Log((object) "No Skins Found!");
    return "";
  }

  public static string GetRandomLockedSkin()
  {
    List<string> stringList = new List<string>();
    foreach (WorshipperData.SkinAndData skinAndData in WorshipperData.Instance.GetSkinsFromFollowerLocation(PlayerFarming.Location))
    {
      if (!skinAndData.Skin[0].Skin.Contains("Boss") && !DataManager.OnBlackList(skinAndData.Skin[0].Skin) && !DataManager.GetFollowerSkinUnlocked(skinAndData.Skin[0].Skin))
        stringList.Add(skinAndData.Skin[0].Skin);
    }
    if (stringList.Count == 0)
    {
      foreach (WorshipperData.SkinAndData skinAndData in WorshipperData.Instance.GetSkinsFromLocation(WorshipperData.DropLocation.Other))
      {
        if (!skinAndData.Skin[0].Skin.Contains("Boss") && !DataManager.OnBlackList(skinAndData.Skin[0].Skin) && !DataManager.GetFollowerSkinUnlocked(skinAndData.Skin[0].Skin))
          stringList.Add(skinAndData.Skin[0].Skin);
      }
    }
    if (stringList.Count > 0)
      return stringList[UnityEngine.Random.Range(0, stringList.Count)];
    Debug.Log((object) "No Skins Found!");
    return "";
  }

  public static bool TryGetFollowerInfoByID(in int id, out FollowerInfo followerInfo)
  {
    followerInfo = (FollowerInfo) null;
    int num1 = 0;
    int num2 = DataManager.instance.Followers.Count - 1;
    while (num1 <= num2)
    {
      int index = num1 + (num2 - num1) / 2;
      FollowerInfo follower = DataManager.instance.Followers[index];
      if (follower.ID == id)
      {
        followerInfo = follower;
        return true;
      }
      if (follower.ID < id)
        num1 = index + 1;
      else
        num2 = index - 1;
    }
    return false;
  }

  public static bool GetFollowerSkinUnlocked(string skinName)
  {
    return DataManager.Instance.FollowerSkinsUnlocked.Contains(skinName);
  }

  public static void SetFollowerSkinUnlocked(string skinName)
  {
    if (skinName == "Mushroom" && !DataManager.instance.SozoUnlockedMushroomSkin)
      return;
    if (!skinName.Contains("Boss") && !skinName.Contains("CultLeader"))
      skinName = skinName.StripNumbers();
    if (DataManager.Instance.FollowerSkinsUnlocked.Contains(skinName))
      return;
    DataManager.Instance.FollowerSkinsUnlocked.Add(skinName);
    Action<string> onSkinUnlocked = DataManager.OnSkinUnlocked;
    if (onSkinUnlocked != null)
      onSkinUnlocked(skinName);
    if (!DataManager.CheckIfThereAreSkinsAvailableAll())
    {
      Debug.Log((object) "Follower Skin Achievement Unlocked");
      AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("ALL_SKINS_UNLOCKED"));
    }
    TwitchFollowers.SendEnabledSkins();
  }

  public static void RemoveUnlockedFollowerSkin(string skinName)
  {
    if (!DataManager.Instance.FollowerSkinsUnlocked.Contains(skinName))
      return;
    DataManager.Instance.FollowerSkinsUnlocked.Remove(skinName);
  }

  public static void UnlockAllDecorations()
  {
    foreach (StructureBrain.TYPES allStructure in StructuresData.AllStructures)
    {
      if (StructuresData.GetCategory(allStructure) == StructureBrain.Categories.AESTHETIC && !StructuresData.GetUnlocked(allStructure))
      {
        StructuresData.CompleteResearch(allStructure);
        StructuresData.SetRevealed(allStructure);
      }
    }
  }

  public void AddKilledBoss(string BossSkin)
  {
    if (this.KilledBosses.Contains(BossSkin))
      return;
    this.KilledBosses.Add(BossSkin);
  }

  public bool CheckKilledBosses(string BossSkin) => this.KilledBosses.Contains(BossSkin);

  public bool HasBeatenAllBeholderBosses()
  {
    return this.KilledBosses.Contains("Boss Beholder 1") && this.KilledBosses.Contains("Boss Beholder 2") && this.KilledBosses.Contains("Boss Beholder 3") && this.KilledBosses.Contains("Boss Beholder 4") && this.KilledBosses.Contains("Boss Beholder 1_P2") && this.KilledBosses.Contains("Boss Beholder 2_P2") && this.KilledBosses.Contains("Boss Beholder 3_P2") && this.KilledBosses.Contains("Boss Beholder 4_P2");
  }

  public static void CheckAllLegendaryWeaponsUnlocked()
  {
    if (!DataManager.Instance.WeaponPool.Contains(EquipmentType.Axe_Legendary) || !DataManager.Instance.WeaponPool.Contains(EquipmentType.Blunderbuss_Legendary) || !DataManager.Instance.WeaponPool.Contains(EquipmentType.Chain_Legendary) || !DataManager.Instance.WeaponPool.Contains(EquipmentType.Dagger_Legendary) || !DataManager.Instance.WeaponPool.Contains(EquipmentType.Gauntlet_Legendary) || !DataManager.Instance.WeaponPool.Contains(EquipmentType.Hammer_Legendary) || !DataManager.Instance.WeaponPool.Contains(EquipmentType.Sword_Legendary))
      return;
    AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup(AchievementsWrapper.Tags.ALL_LEGENDARY_WEAPONS));
  }

  public void AddWeapon(EquipmentType weapon)
  {
    if (this.WeaponPool.Contains(weapon))
      return;
    this.WeaponPool.Add(weapon);
    Action<EquipmentType> onWeaponUnlocked = DataManager.OnWeaponUnlocked;
    if (onWeaponUnlocked == null)
      return;
    onWeaponUnlocked(weapon);
  }

  public void AddLegendaryWeaponToUnlockQueue(EquipmentType legendaryWeapon)
  {
    if (!EquipmentManager.IsLegendaryWeapon(legendaryWeapon) || this.LegendaryWeaponsUnlockOrder.Contains(legendaryWeapon))
      return;
    this.LegendaryWeaponsUnlockOrder.Add(legendaryWeapon);
  }

  public static int StartingEquipmentLevel
  {
    get
    {
      if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_StartingWeapon_8))
        return 24;
      if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_StartingWeapon_7))
        return 21;
      if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_StartingWeapon_6))
        return 18;
      if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_StartingWeapon_5))
        return 15;
      if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_StartingWeapon_4))
        return 12;
      if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_StartingWeapon_3))
        return 9;
      if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_StartingWeapon_2))
        return 6;
      return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_StartingWeapon_1) ? 3 : 0;
    }
  }

  public EquipmentType GetRandomWeaponInPool(bool skipFirstRoomCheck = false, bool coopPodium = false)
  {
    if (!coopPodium && !skipFirstRoomCheck && GameManager.CurrentDungeonFloor <= 1 && DataManager.Instance.dungeonRun >= 2 && !DungeonSandboxManager.Active && PlayerFarming.Location != FollowerLocation.Boss_5 && (SettingsManager.Settings == null || !SettingsManager.Settings.Accessibility.ForceWeapon || SettingsManager.Settings.Accessibility.ForcedWeapon == -1 || PlayerFleeceManager.FleecePreventsForcedWeapons()))
    {
      if (!DataManager.Instance.WeaponPool.Contains(EquipmentType.Axe))
      {
        PlayerWeapon.FirstTimeUsingWeapon = true;
        return EquipmentType.Axe;
      }
      if (!DataManager.Instance.WeaponPool.Contains(EquipmentType.Dagger))
      {
        PlayerWeapon.FirstTimeUsingWeapon = true;
        return EquipmentType.Dagger;
      }
      if (!DataManager.Instance.WeaponPool.Contains(EquipmentType.Hammer) && DataManager.Instance.BossesCompleted.Count >= 2)
      {
        PlayerWeapon.FirstTimeUsingWeapon = true;
        return EquipmentType.Hammer;
      }
      if (!DataManager.Instance.WeaponPool.Contains(EquipmentType.Gauntlet) && DataManager.Instance.BossesCompleted.Count >= 1)
      {
        PlayerWeapon.FirstTimeUsingWeapon = true;
        return EquipmentType.Gauntlet;
      }
      if (!DataManager.Instance.WeaponPool.Contains(EquipmentType.Blunderbuss) && DataManager.Instance.BossesCompleted.Count >= 3)
      {
        PlayerWeapon.FirstTimeUsingWeapon = true;
        return EquipmentType.Blunderbuss;
      }
      if (!DataManager.Instance.WeaponPool.Contains(EquipmentType.Chain) && PlayerFarming.Location == FollowerLocation.Dungeon1_5)
      {
        PlayerWeapon.FirstTimeUsingWeapon = true;
        return EquipmentType.Chain;
      }
    }
    if (DataManager.instance.PlayerFleece == 676)
    {
      List<EquipmentType> equipmentTypeList = new List<EquipmentType>((IEnumerable<EquipmentType>) DataManager.Instance.WeaponPool);
      for (int index = equipmentTypeList.Count - 1; index >= 0; --index)
      {
        WeaponData weaponData = EquipmentManager.GetWeaponData(equipmentTypeList[index]);
        if ((weaponData != null ? (weaponData.PrimaryEquipmentType != EquipmentType.Blunderbuss ? 1 : 0) : 1) != 0)
          equipmentTypeList.RemoveAt(index);
      }
      if (!UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Blacksmith_Legendary_Blunderbuss))
        equipmentTypeList.Remove(EquipmentType.Blunderbuss_Legendary);
      return equipmentTypeList[UnityEngine.Random.Range(0, equipmentTypeList.Count)];
    }
    List<EquipmentType> equipmentTypeList1 = new List<EquipmentType>((IEnumerable<EquipmentType>) DataManager.Instance.WeaponPool);
    if (equipmentTypeList1.Count <= 1)
      return equipmentTypeList1[0];
    if (!UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Blacksmith_Legendary_Hammer))
      equipmentTypeList1.Remove(EquipmentType.Hammer_Legendary);
    if (!UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Blacksmith_Legendary_Sword))
      equipmentTypeList1.Remove(EquipmentType.Hammer_Legendary);
    if (!UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Blacksmith_Legendary_Dagger))
      equipmentTypeList1.Remove(EquipmentType.Dagger_Legendary);
    if (!UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Blacksmith_Legendary_Gauntlets))
      equipmentTypeList1.Remove(EquipmentType.Gauntlet_Legendary);
    if (!UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Blacksmith_Legendary_Blunderbuss))
      equipmentTypeList1.Remove(EquipmentType.Blunderbuss_Legendary);
    if (!UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Blacksmith_Legendary_Chain))
      equipmentTypeList1.Remove(EquipmentType.Chain_Legendary);
    if (!UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Blacksmith_Legendary_Axe))
      equipmentTypeList1.Remove(EquipmentType.Axe_Legendary);
    foreach (Interaction_LegendaryWeaponSelectionPodium legendaryPodium in Interaction_LegendaryWeaponSelectionPodium.LegendaryPodiums)
      equipmentTypeList1.Remove(legendaryPodium.TypeOfWeapon);
    for (int index = equipmentTypeList1.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) EquipmentManager.GetWeaponData(equipmentTypeList1[index]) != (UnityEngine.Object) null)
      {
        EquipmentType primaryEquipmentType = EquipmentManager.GetWeaponData(equipmentTypeList1[index]).PrimaryEquipmentType;
        if (!DataManager.Instance.WeaponPool.Contains(primaryEquipmentType))
          equipmentTypeList1.Remove(equipmentTypeList1[index]);
        else if ((bool) (UnityEngine.Object) EquipmentManager.GetWeaponData(equipmentTypeList1[index]).GetAttachment(AttachmentEffect.Fervour) && (DungeonSandboxManager.Active || !GameManager.HasUnlockAvailable() && !DataManager.Instance.DeathCatBeaten))
          equipmentTypeList1.Remove(equipmentTypeList1[index]);
        else if (primaryEquipmentType == EquipmentType.Chain && !DataManager.instance.MAJOR_DLC)
          equipmentTypeList1.Remove(equipmentTypeList1[index]);
        else if (primaryEquipmentType == EquipmentType.Sword_Ratau && PlayerFleeceManager.FleeceSwapsCurseForRelic())
          equipmentTypeList1.Remove(equipmentTypeList1[index]);
      }
      else
        equipmentTypeList1.Remove(equipmentTypeList1[index]);
    }
    List<EquipmentType> equipmentTypeList2 = new List<EquipmentType>();
    if (SettingsManager.Settings != null && SettingsManager.Settings.Accessibility.ForceWeapon && SettingsManager.Settings.Accessibility.ForcedWeapon != -1 && !PlayerFleeceManager.FleecePreventsForcedWeapons())
    {
      foreach (EquipmentType weaponType in equipmentTypeList1)
      {
        switch (SettingsManager.Settings.Accessibility.ForcedWeapon)
        {
          case 0:
            if (EquipmentManager.GetWeaponData(weaponType).PrimaryEquipmentType == EquipmentType.Sword)
            {
              equipmentTypeList2.Add(weaponType);
              continue;
            }
            continue;
          case 1:
            if (EquipmentManager.GetWeaponData(weaponType).PrimaryEquipmentType == EquipmentType.Axe)
            {
              equipmentTypeList2.Add(weaponType);
              continue;
            }
            continue;
          case 2:
            if (EquipmentManager.GetWeaponData(weaponType).PrimaryEquipmentType == EquipmentType.Dagger)
            {
              equipmentTypeList2.Add(weaponType);
              continue;
            }
            continue;
          case 3:
            if (EquipmentManager.GetWeaponData(weaponType).PrimaryEquipmentType == EquipmentType.Gauntlet)
            {
              equipmentTypeList2.Add(weaponType);
              continue;
            }
            continue;
          case 4:
            if (EquipmentManager.GetWeaponData(weaponType).PrimaryEquipmentType == EquipmentType.Hammer)
            {
              equipmentTypeList2.Add(weaponType);
              continue;
            }
            continue;
          case 5:
            if (EquipmentManager.GetWeaponData(weaponType).PrimaryEquipmentType == EquipmentType.Blunderbuss)
            {
              equipmentTypeList2.Add(weaponType);
              continue;
            }
            continue;
          case 6:
            if (EquipmentManager.GetWeaponData(weaponType).PrimaryEquipmentType == EquipmentType.Chain)
            {
              equipmentTypeList2.Add(weaponType);
              continue;
            }
            continue;
          default:
            continue;
        }
      }
      if (equipmentTypeList2.Count > 0)
      {
        Debug.Log((object) ("ForcedList Count: " + equipmentTypeList2.Count.ToString()));
        return this.CursePool.Count <= 0 ? EquipmentType.Sword : equipmentTypeList2[UnityEngine.Random.Range(0, equipmentTypeList2.Count)];
      }
    }
    else if (DataManager.Instance.ForcedStartingWeapon != EquipmentType.None && !DungeonSandboxManager.Active && !PlayerFleeceManager.FleecePreventsForcedWeapons())
    {
      int forcedStartingWeapon = (int) DataManager.Instance.ForcedStartingWeapon;
      DataManager.Instance.ForcedStartingWeapon = EquipmentType.None;
      return (EquipmentType) forcedStartingWeapon;
    }
    return this.CursePool.Count <= 0 ? EquipmentType.Sword : equipmentTypeList1[UnityEngine.Random.Range(0, equipmentTypeList1.Count)];
  }

  public void AddCurse(EquipmentType curse)
  {
    if (this.CursePool.Contains(curse))
      return;
    this.CursePool.Add(curse);
  }

  public EquipmentType GetRandomCurseInPool(bool isCoopPodium = false)
  {
    if (!isCoopPodium && GameManager.CurrentDungeonFloor <= 1 && DataManager.Instance.dungeonRun >= 2 && !DungeonSandboxManager.Active && !DataManager.instance.QuickStartActive)
    {
      if (!DataManager.Instance.CursePool.Contains(EquipmentType.Tentacles))
        return EquipmentType.Tentacles;
      if (!DataManager.Instance.CursePool.Contains(EquipmentType.EnemyBlast))
        return EquipmentType.EnemyBlast;
      if (!DataManager.Instance.CursePool.Contains(EquipmentType.ProjectileAOE))
        return EquipmentType.ProjectileAOE;
      if (!DataManager.Instance.CursePool.Contains(EquipmentType.MegaSlash))
        return EquipmentType.MegaSlash;
    }
    List<EquipmentType> equipmentTypeList = new List<EquipmentType>((IEnumerable<EquipmentType>) DataManager.Instance.CursePool);
    if (equipmentTypeList.Count <= 1)
      return equipmentTypeList[0];
    if ((bool) (UnityEngine.Object) PlayerFarming.Instance && equipmentTypeList.Contains(PlayerFarming.Instance.currentCurse))
      equipmentTypeList.Remove(PlayerFarming.Instance.currentCurse);
    for (int index = equipmentTypeList.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) EquipmentManager.GetCurseData(equipmentTypeList[index]) != (UnityEngine.Object) null)
      {
        if (!DataManager.Instance.CursePool.Contains(EquipmentManager.GetCurseData(equipmentTypeList[index]).PrimaryEquipmentType))
          equipmentTypeList.Remove(equipmentTypeList[index]);
        else if (PlayerFleeceManager.FleeceSwapsWeaponForCurse() && equipmentTypeList[index] == EquipmentType.ProjectileAOE_GoopTrail)
          equipmentTypeList.Remove(equipmentTypeList[index]);
      }
      else
        equipmentTypeList.Remove(equipmentTypeList[index]);
    }
    return equipmentTypeList.Count <= 0 ? EquipmentType.Fireball : equipmentTypeList[UnityEngine.Random.Range(0, equipmentTypeList.Count)];
  }

  public static void DungeonCompletedWithFleece(FollowerLocation location, int fleece)
  {
    foreach (DataManager.DungeonCompletedFleeces completedWithFleece in DataManager.Instance.DungeonsCompletedWithFleeces)
    {
      if (completedWithFleece.Location == location)
      {
        if (completedWithFleece.Fleeces.Contains(fleece))
          return;
        completedWithFleece.Fleeces.Add(fleece);
        return;
      }
    }
    DataManager.instance.DungeonsCompletedWithFleeces.Add(new DataManager.DungeonCompletedFleeces()
    {
      Location = location,
      Fleeces = new List<int>(1) { fleece }
    });
  }

  public bool PlayerFoundTrinketsContains(TarotCards.TarotCard card, PlayerFarming playerFarming)
  {
    if ((UnityEngine.Object) playerFarming == (UnityEngine.Object) null)
      playerFarming = PlayerFarming.Instance;
    return playerFarming.FoundTrinkets.Contains(card);
  }

  public void PlayerFoundTrinketsAdd(TarotCards.TarotCard card, PlayerFarming playerFarming)
  {
    if ((UnityEngine.Object) playerFarming == (UnityEngine.Object) null)
      playerFarming = PlayerFarming.Instance;
    if (playerFarming.FoundTrinkets.Contains(card))
      return;
    playerFarming.FoundTrinkets.Add(card);
  }

  public bool PlayerRunTrinketsContains(TarotCards.TarotCard card, PlayerFarming playerFarming)
  {
    if ((UnityEngine.Object) playerFarming == (UnityEngine.Object) null)
      playerFarming = PlayerFarming.Instance;
    return playerFarming.RunTrinkets.Contains(card);
  }

  public void PlayerRunTrinketsAdd(TarotCards.TarotCard card, PlayerFarming playerFarming)
  {
    if ((UnityEngine.Object) playerFarming == (UnityEngine.Object) null)
      playerFarming = PlayerFarming.Instance;
    if (playerFarming.RunTrinkets.Contains(card))
      return;
    playerFarming.RunTrinkets.Add(card);
  }

  public void PlayerTrinketsRemove(TarotCards.TarotCard card, PlayerFarming playerFarming)
  {
    if ((UnityEngine.Object) playerFarming == (UnityEngine.Object) null)
      playerFarming = PlayerFarming.Instance;
    playerFarming.RunTrinkets.Remove(card);
  }

  public static bool UnlockTrinket(TarotCards.Card card)
  {
    if (DataManager.Instance.PlayerFoundTrinkets.Contains(card))
      return false;
    int count = DataManager.Instance.PlayerFoundTrinkets.Count;
    string str1 = count.ToString();
    count = DataManager.AllTrinkets.Count;
    string str2 = count.ToString();
    Debug.Log((object) $"Collected Tarots: {str1}Total Tarot: {str2}");
    DataManager.Instance.PlayerFoundTrinkets.Add(card);
    int num1 = 0;
    for (int index = 0; index < DataManager.Instance.PlayerFoundTrinkets.Count; ++index)
    {
      if (!TarotCards.CoopCards.Contains<TarotCards.Card>(DataManager.Instance.PlayerFoundTrinkets[index]))
        ++num1;
    }
    int num2 = 0;
    for (int index = 0; index < DataManager.AllTrinkets.Count; ++index)
    {
      if (!TarotCards.CoopCards.Contains<TarotCards.Card>(DataManager.AllTrinkets[index]))
        ++num2;
    }
    if (num1 >= num2)
      AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("ALL_TAROTS_UNLOCKED"));
    return true;
  }

  public static bool TrinketUnlocked(TarotCards.Card card)
  {
    return DataManager.Instance.PlayerFoundTrinkets.Contains(card);
  }

  public static int GetTrinketsUnlocked()
  {
    int trinketsUnlocked = 0;
    foreach (TarotCards.Card allTrinket in DataManager.AllTrinkets)
    {
      if (DataManager.TrinketUnlocked(allTrinket))
        ++trinketsUnlocked;
    }
    return trinketsUnlocked;
  }

  public static float GetWeaponDamageMultiplier(int level)
  {
    return (float) (0.12999999523162842 + 0.070000000298023224 * (double) level);
  }

  public static float GetWeaponAttackRateMultiplier(int level) => 0.3f * (float) level;

  public static void UnlockFullyUpgradedWeapon()
  {
  }

  public static void UnlockFullyUpgradedCurse()
  {
  }

  public static void IncreaseCurseLevel(TarotCards.Card curse)
  {
    switch (curse)
    {
      case TarotCards.Card.Fireball:
        ++DataManager.instance.FireballLevel;
        if (DataManager.instance.FireballLevel < 3)
          break;
        DataManager.UnlockFullyUpgradedCurse();
        break;
      case TarotCards.Card.Tentacles:
        ++DataManager.instance.TentaclesLevel;
        if (DataManager.instance.TentaclesLevel < 3)
          break;
        DataManager.UnlockFullyUpgradedCurse();
        break;
      case TarotCards.Card.EnemyBlast:
        ++DataManager.instance.EnemyBlastLevel;
        if (DataManager.instance.EnemyBlastLevel < 3)
          break;
        DataManager.UnlockFullyUpgradedCurse();
        break;
      case TarotCards.Card.ProjectileAOE:
        ++DataManager.instance.ProjectileAOELevel;
        if (DataManager.instance.ProjectileAOELevel < 3)
          break;
        DataManager.UnlockFullyUpgradedCurse();
        break;
      case TarotCards.Card.Vortex:
        ++DataManager.instance.VortexLevel;
        if (DataManager.instance.VortexLevel < 3)
          break;
        DataManager.UnlockFullyUpgradedCurse();
        break;
      case TarotCards.Card.MegaSlash:
        ++DataManager.instance.MegaSlashLevel;
        if (DataManager.instance.MegaSlashLevel < 3)
          break;
        DataManager.UnlockFullyUpgradedCurse();
        break;
    }
  }

  public static string GetCurseUpgradeDescription(TarotCards.Card curse, int upgradeLevel)
  {
    return LocalizationManager.GetTranslation($"UpgradeSystem/{curse.ToString()}/Upgrade/{upgradeLevel}");
  }

  public bool HasBuiltDecoration(StructureBrain.TYPES structureType)
  {
    return this.DecorationTypesBuilt.Contains((int) structureType);
  }

  public void SetBuiltDecoration(StructureBrain.TYPES structureType)
  {
    if (this.HasBuiltDecoration(structureType))
      return;
    this.DecorationTypesBuilt.Add((int) structureType);
  }

  public MissionManager.Mission GetMission(MissionManager.MissionType missionType, int id)
  {
    foreach (MissionManager.Mission activeMission in this.ActiveMissions)
    {
      if (activeMission.MissionType == missionType && activeMission.ID == id)
        return activeMission;
    }
    return (MissionManager.Mission) null;
  }

  public bool ContainsMissionID(MissionManager.MissionType missionType, int id)
  {
    return this.GetMission(missionType, id) != null;
  }

  public void UpdateShop(ShopLocationTracker shop)
  {
    for (int index = 0; index < this.Shops.Count; ++index)
    {
      if (this.Shops[index].location == shop.location)
      {
        this.Shops[index] = shop;
        break;
      }
    }
  }

  public bool CheckShopExists(FollowerLocation location, string ShopKeeperName)
  {
    foreach (ShopLocationTracker shop in this.Shops)
    {
      if (shop.location == location && shop.shopKeeperName == ShopKeeperName)
        return true;
    }
    return false;
  }

  public ShopLocationTracker GetShop(FollowerLocation location, string ShopKeeperName)
  {
    foreach (ShopLocationTracker shop in this.Shops)
    {
      if (shop.location == location && shop.shopKeeperName == ShopKeeperName)
        return shop;
    }
    return (ShopLocationTracker) null;
  }

  public bool CheckInvestmentExist() => this.Investment != null;

  public void CreateInvestment(JellyFishInvestment _Investment) => this.Investment = _Investment;

  public TraderTracker ReturnTrader(FollowerLocation location)
  {
    foreach (TraderTracker trader in this.Traders)
    {
      if (trader.location == location)
        return trader;
    }
    Debug.Log((object) "Couldn't find Trader");
    return (TraderTracker) null;
  }

  public void SetTrader(TraderTracker trader)
  {
    int index = 0;
    foreach (TraderTracker trader1 in this.Traders)
    {
      if (trader1.location != trader.location)
        ++index;
      else
        break;
    }
    this.Traders[index] = trader;
  }

  public Lamb.UI.ItemSelector.Category GetItemSelectorCategory(string key)
  {
    if (string.IsNullOrEmpty(key))
      return (Lamb.UI.ItemSelector.Category) null;
    foreach (Lamb.UI.ItemSelector.Category selectorCategory in this.ItemSelectorCategories)
    {
      if (selectorCategory.Key == key)
        return selectorCategory;
    }
    Lamb.UI.ItemSelector.Category selectorCategory1 = new Lamb.UI.ItemSelector.Category()
    {
      Key = key
    };
    this.ItemSelectorCategories.Add(selectorCategory1);
    return selectorCategory1;
  }

  [IgnoreMember]
  public int PLAYER_STARTING_HEALTH
  {
    get
    {
      switch (DifficultyManager.PrimaryDifficulty)
      {
        case DifficultyManager.Difficulty.Easy:
          return 8;
        case DifficultyManager.Difficulty.Hard:
          return 4;
        case DifficultyManager.Difficulty.ExtraHard:
          return 2;
        default:
          return 6;
      }
    }
  }

  [IgnoreMember]
  public float GetTargetChoreXP
  {
    get
    {
      return DataManager.TargetChoreXP[Mathf.Min(this.ChoreXPLevel, DataManager.TargetChoreXP.Count - 1)];
    }
  }

  public static float GetChoreDuration(PlayerFarming playerFarming)
  {
    return playerFarming.isLamb ? Mathf.Lerp(1.5f, 0.2f, (float) DataManager.Instance.ChoreXPLevel / 9f) : Mathf.Lerp(1.5f, 0.2f, (float) DataManager.Instance.ChoreXPLevel_Coop / 9f);
  }

  public static float AllUnlockedMultiplier => GameManager.HasUnlockAvailable() ? 1f : 3f;

  public static int GetTargetXP(int index)
  {
    return Mathf.FloorToInt((float) DataManager.TargetXP[index] * DataManager.AllUnlockedMultiplier);
  }

  [JsonIgnore]
  [IgnoreMember]
  public int CurrentChallengeModeTargetXP
  {
    get
    {
      return DataManager.ChallengeModeTargetXP[Mathf.Min(this.CurrentChallengeModeLevel, DataManager.ChallengeModeTargetXP.Count - 1)];
    }
  }

  [IgnoreMember]
  public int CURRENT_WEAPON
  {
    get => this.currentweapon;
    set
    {
      if (DataManager.OnChangeTool != null)
        DataManager.OnChangeTool(this.currentweapon, value);
      this.currentweapon = value;
    }
  }

  public static event DataManager.ChangeToolAction OnChangeTool;

  public static DataManager Instance
  {
    get
    {
      if (DataManager.instance == null)
      {
        DataManager dataManager = new DataManager();
      }
      return DataManager.instance;
    }
    set => DataManager.instance = value;
  }

  [IgnoreMember]
  public bool AwokenMountainDeathsceen
  {
    get => this.\u003CAwokenMountainDeathsceen\u003Ek__BackingField;
    set => this.\u003CAwokenMountainDeathsceen\u003Ek__BackingField = value;
  }

  [IgnoreMember]
  public bool IceGoreShown
  {
    get => this.\u003CIceGoreShown\u003Ek__BackingField;
    set => this.\u003CIceGoreShown\u003Ek__BackingField = value;
  }

  [IgnoreMember]
  public DataManager.GhostLostLambState ghostLostLambState
  {
    get => this.\u003CghostLostLambState\u003Ek__BackingField;
    set => this.\u003CghostLostLambState\u003Ek__BackingField = value;
  }

  public DataManager() => DataManager.instance = this;

  public static void ResetData()
  {
    DataManager.instance = new DataManager();
    DataManager.instance.PlayerBluePrints = new List<BluePrint>();
    DataManager.instance.MetaData = MetaData.Default(DataManager.instance);
  }

  public void AddEnemyKilled(Enemy enemy)
  {
    for (int index = 0; index < this.EnemiesKilled.Count; ++index)
    {
      if (this.EnemiesKilled[index].EnemyType == enemy)
      {
        ++this.EnemiesKilled[index].AmountKilled;
        return;
      }
    }
    this.EnemiesKilled.Add(new DataManager.EnemyData()
    {
      EnemyType = enemy
    });
  }

  public int GetEnemiesKilled(Enemy enemy)
  {
    for (int index = 0; index < this.EnemiesKilled.Count; ++index)
    {
      if (this.EnemiesKilled[index].EnemyType == enemy)
        return this.EnemiesKilled[index].AmountKilled;
    }
    return 0;
  }

  public float GetLuckMultiplier()
  {
    return 1f - (float) ((double) this.LuckMultiplier * (double) DifficultyManager.GetLuckMultiplier() / 10.0);
  }

  public bool DungeonCompleted(FollowerLocation location, bool layer2 = false)
  {
    bool flag = this.BossesCompleted.Contains(location);
    if (!layer2)
      return flag;
    this.DungeonCompleted(location);
    switch (location)
    {
      case FollowerLocation.Dungeon1_2:
        return flag && DataManager.instance.BeatenHeketLayer2;
      case FollowerLocation.Dungeon1_3:
        return flag && DataManager.instance.BeatenKallamarLayer2;
      case FollowerLocation.Dungeon1_4:
        return flag && DataManager.instance.BeatenShamuraLayer2;
      default:
        return flag && DataManager.instance.BeatenLeshyLayer2;
    }
  }

  public int GetDungeonLayer(FollowerLocation location)
  {
    switch (location)
    {
      case FollowerLocation.Dungeon1_1:
        return this.Dungeon1_Layer;
      case FollowerLocation.Dungeon1_2:
        return this.Dungeon2_Layer;
      case FollowerLocation.Dungeon1_3:
        return this.Dungeon3_Layer;
      case FollowerLocation.Dungeon1_4:
        return this.Dungeon4_Layer;
      case FollowerLocation.Dungeon1_5:
        return this.Dungeon5_Layer;
      case FollowerLocation.Dungeon1_6:
        return this.Dungeon6_Layer;
      default:
        return 0;
    }
  }

  public FollowerLocation GetCurrentDungeon()
  {
    if (!this.BossesCompleted.Contains(FollowerLocation.Dungeon1_1))
      return FollowerLocation.Dungeon1_1;
    if (!this.BossesCompleted.Contains(FollowerLocation.Dungeon1_2))
      return FollowerLocation.Dungeon1_2;
    if (!this.BossesCompleted.Contains(FollowerLocation.Dungeon1_3))
      return FollowerLocation.Dungeon1_3;
    return !this.BossesCompleted.Contains(FollowerLocation.Dungeon1_4) ? FollowerLocation.Dungeon1_4 : FollowerLocation.Dungeon1_1;
  }

  public int GetDungeonNumber(FollowerLocation location)
  {
    switch (location)
    {
      case FollowerLocation.Dungeon1_1:
        return 1;
      case FollowerLocation.Dungeon1_2:
        return 2;
      case FollowerLocation.Dungeon1_3:
        return 3;
      case FollowerLocation.Dungeon1_4:
      case FollowerLocation.Dungeon1_5:
        return 4;
      default:
        return 1;
    }
  }

  public void AddDungeonLayer(FollowerLocation location)
  {
    switch (location)
    {
      case FollowerLocation.Dungeon1_1:
        ++this.Dungeon1_Layer;
        break;
      case FollowerLocation.Dungeon1_2:
        ++this.Dungeon2_Layer;
        break;
      case FollowerLocation.Dungeon1_3:
        ++this.Dungeon3_Layer;
        break;
      case FollowerLocation.Dungeon1_4:
        ++this.Dungeon4_Layer;
        break;
      case FollowerLocation.Dungeon1_5:
        ++this.Dungeon5_Layer;
        break;
      case FollowerLocation.Dungeon1_6:
        ++this.Dungeon6_Layer;
        break;
    }
  }

  public void SetDungeonLayer(FollowerLocation location, int layer)
  {
    switch (location)
    {
      case FollowerLocation.Dungeon1_1:
        this.Dungeon1_Layer = layer;
        break;
      case FollowerLocation.Dungeon1_2:
        this.Dungeon2_Layer = layer;
        break;
      case FollowerLocation.Dungeon1_3:
        this.Dungeon3_Layer = layer;
        break;
      case FollowerLocation.Dungeon1_4:
        this.Dungeon4_Layer = layer;
        break;
      case FollowerLocation.Dungeon1_5:
        this.Dungeon5_Layer = layer;
        break;
      case FollowerLocation.Dungeon1_6:
        this.Dungeon6_Layer = layer;
        break;
    }
  }

  public void AddToNotificationHistory(FinalizedNotification finalizedNotification)
  {
    if (this.NotificationHistory.Count > 20)
      this.NotificationHistory.RemoveAt(this.NotificationHistory.Count - 1);
    this.NotificationHistory.Insert(0, finalizedNotification);
  }

  public static int GetGodTearNotches(FollowerLocation location)
  {
    switch (location)
    {
      case FollowerLocation.Dungeon1_1:
        return DataManager.Instance.Dungeon1GodTears;
      case FollowerLocation.Dungeon1_2:
        return DataManager.Instance.Dungeon2GodTears;
      case FollowerLocation.Dungeon1_3:
        return DataManager.Instance.Dungeon3GodTears;
      case FollowerLocation.Dungeon1_4:
        return DataManager.Instance.Dungeon4GodTears;
      default:
        return 0;
    }
  }

  public static int GetGodTearNotchesTotal()
  {
    return DataManager.Instance.Dungeon1GodTears + DataManager.Instance.Dungeon2GodTears + DataManager.Instance.Dungeon3GodTears + DataManager.Instance.Dungeon4GodTears;
  }

  public static bool ActivateCultistDLC()
  {
    if (!DataManager.Instance.DLC_Cultist_Pack)
    {
      DataManager.Instance.DLC_Cultist_Pack = true;
      for (int index = 0; index < DataManager.CultistDLCSkins.Count; ++index)
        DataManager.SetFollowerSkinUnlocked(DataManager.CultistDLCSkins[index]);
      for (int index = 0; index < DataManager.CultistDLCStructures.Count; ++index)
        StructuresData.CompleteResearch(DataManager.CultistDLCStructures[index]);
      return true;
    }
    foreach (FollowerClothingType type in DataManager.Instance.Cultist_DLC_Clothing)
    {
      if (!DataManager.instance.UnlockedClothing.Contains(type))
        DataManager.instance.AddNewClothes(type);
    }
    return false;
  }

  public static void DeactivateWoolhavenDLC()
  {
    DataManager.Instance.MAJOR_DLC = false;
    DataManager.instance.YngyaOffering = 0;
    foreach (StructureBrain.TYPES decoration in Interaction_DLCShrine.Decorations)
    {
      if (DataManager.instance.UnlockedStructures.Contains(decoration))
        DataManager.instance.UnlockedStructures.Remove(decoration);
    }
    if (DataManager.instance.UnlockedUpgrades.Contains(UpgradeSystem.Type.Ritual_FollowerWedding))
      DataManager.instance.UnlockedUpgrades.Remove(UpgradeSystem.Type.Ritual_FollowerWedding);
    for (int index = 0; index < DataManager.instance.Followers.Count; ++index)
      DataManager.instance.Followers[index].SpouseFollowerID = -1;
    if ((UnityEngine.Object) WeatherSystemController.Instance != (UnityEngine.Object) null && WeatherSystemController.Instance.CurrentWeatherType == WeatherSystemController.WeatherType.Snowing)
      WeatherSystemController.Instance.StopCurrentWeather(0.0f);
    ObjectiveManager.FailObjective("Objectives/GroupTitles/ExpandBase");
    DataManager.Instance.LandPurchased = -1;
    DataManager.Instance.LandConvoProgress = 0;
    DataManager.Instance.OnboardedBaseExpansion = false;
  }

  public static void DeactivateCultistDLC()
  {
    DataManager.Instance.DLC_Cultist_Pack = false;
    for (int index = 0; index < DataManager.CultistDLCSkins.Count; ++index)
      DataManager.RemoveUnlockedFollowerSkin(DataManager.CultistDLCSkins[index]);
    for (int index = 0; index < DataManager.CultistDLCStructures.Count; ++index)
      DataManager.Instance.UnlockedStructures.Remove(DataManager.CultistDLCStructures[index]);
    foreach (FollowerClothingType followerClothingType in DataManager.Instance.Cultist_DLC_Clothing)
    {
      if (DataManager.instance.UnlockedClothing.Contains(followerClothingType))
        DataManager.instance.UnlockedClothing.Remove(followerClothingType);
    }
  }

  public static bool ActivateHereticDLC()
  {
    if (!DataManager.Instance.DLC_Heretic_Pack)
    {
      DataManager.Instance.DLC_Heretic_Pack = true;
      for (int index = 0; index < DataManager.HereticDLCSkins.Count; ++index)
        DataManager.SetFollowerSkinUnlocked(DataManager.HereticDLCSkins[index]);
      for (int index = 0; index < DataManager.HereticDLCStructures.Count; ++index)
        StructuresData.CompleteResearch(DataManager.HereticDLCStructures[index]);
      if (!DataManager.instance.UnlockedFleeces.Contains(999))
        DataManager.Instance.UnlockedFleeces.Add(999);
      return true;
    }
    foreach (FollowerClothingType type in DataManager.Instance.Heretic_DLC_Clothing)
    {
      if (!DataManager.instance.UnlockedClothing.Contains(type))
        DataManager.instance.AddNewClothes(type);
    }
    return false;
  }

  public static void DeactivateHereticDLC()
  {
    DataManager.Instance.DLC_Heretic_Pack = false;
    for (int index = 0; index < DataManager.HereticDLCSkins.Count; ++index)
      DataManager.RemoveUnlockedFollowerSkin(DataManager.HereticDLCSkins[index]);
    for (int index = 0; index < DataManager.HereticDLCStructures.Count; ++index)
      DataManager.Instance.UnlockedStructures.Remove(DataManager.HereticDLCStructures[index]);
    if (DataManager.instance.UnlockedFleeces.Contains(999))
      DataManager.Instance.UnlockedFleeces.Remove(999);
    if (DataManager.instance.PlayerFleece == 999)
    {
      DataManager.instance.PlayerFleece = 0;
      if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
        PlayerFarming.Instance.simpleSpineAnimator?.SetSkin("Lamb_0");
    }
    foreach (FollowerClothingType followerClothingType in DataManager.Instance.Heretic_DLC_Clothing)
    {
      if (DataManager.instance.UnlockedClothing.Contains(followerClothingType))
        DataManager.instance.UnlockedClothing.Remove(followerClothingType);
    }
  }

  public static bool ActivateSinfulDLC()
  {
    if (DataManager.Instance.DLC_Sinful_Pack)
      return false;
    DataManager.Instance.DLC_Sinful_Pack = true;
    for (int index = 0; index < DataManager.SinfulDLCSkins.Count; ++index)
      DataManager.SetFollowerSkinUnlocked(DataManager.SinfulDLCSkins[index]);
    for (int index = 0; index < DataManager.SinfulDLCStructures.Count; ++index)
      StructuresData.CompleteResearch(DataManager.SinfulDLCStructures[index]);
    if (!DataManager.instance.UnlockedFleeces.Contains(1001))
      DataManager.Instance.UnlockedFleeces.Add(1001);
    foreach (FollowerClothingType type in DataManager.Instance.Sinful_DLC_Clothing)
    {
      if (!DataManager.instance.UnlockedClothing.Contains(type))
        DataManager.instance.AddNewClothes(type);
    }
    return true;
  }

  public static void DeactivateSinfulDLC()
  {
    DataManager.Instance.DLC_Sinful_Pack = false;
    for (int index = 0; index < DataManager.SinfulDLCSkins.Count; ++index)
      DataManager.RemoveUnlockedFollowerSkin(DataManager.SinfulDLCSkins[index]);
    if (DataManager.instance.UnlockedFleeces.Contains(1001))
      DataManager.Instance.UnlockedFleeces.Remove(1001);
    if (DataManager.instance.PlayerFleece == 1001)
    {
      DataManager.instance.PlayerFleece = 0;
      if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
        PlayerFarming.Instance.simpleSpineAnimator?.SetSkin("Lamb_0");
    }
    for (int index = 0; index < DataManager.SinfulDLCStructures.Count; ++index)
      DataManager.Instance.UnlockedStructures.Remove(DataManager.SinfulDLCStructures[index]);
    foreach (FollowerClothingType followerClothingType in DataManager.Instance.Sinful_DLC_Clothing)
    {
      if (DataManager.instance.UnlockedClothing.Contains(followerClothingType))
        DataManager.instance.UnlockedClothing.Remove(followerClothingType);
    }
  }

  public static bool ActivatePilgrimDLC()
  {
    if (DataManager.Instance.DLC_Pilgrim_Pack)
      return false;
    DataManager.Instance.DLC_Pilgrim_Pack = true;
    for (int index = 0; index < DataManager.PilgrimDLCSkins.Count; ++index)
      DataManager.SetFollowerSkinUnlocked(DataManager.PilgrimDLCSkins[index]);
    for (int index = 0; index < DataManager.PilgrimDLCStructures.Count; ++index)
      StructuresData.CompleteResearch(DataManager.PilgrimDLCStructures[index]);
    if (!DataManager.instance.UnlockedFleeces.Contains(1002))
      DataManager.Instance.UnlockedFleeces.Add(1002);
    foreach (FollowerClothingType type in DataManager.Instance.Pilgrim_DLC_Clothing)
    {
      if (!DataManager.instance.UnlockedClothing.Contains(type))
        DataManager.instance.AddNewClothes(type);
    }
    return true;
  }

  public static void DeactivatePilgrimDLC()
  {
    DataManager.Instance.DLC_Pilgrim_Pack = false;
    for (int index = 0; index < DataManager.PilgrimDLCSkins.Count; ++index)
      DataManager.RemoveUnlockedFollowerSkin(DataManager.PilgrimDLCSkins[index]);
    for (int index = 0; index < DataManager.PilgrimDLCStructures.Count; ++index)
      DataManager.Instance.UnlockedStructures.Remove(DataManager.PilgrimDLCStructures[index]);
    foreach (FollowerClothingType followerClothingType in DataManager.Instance.Pilgrim_DLC_Clothing)
    {
      if (DataManager.instance.UnlockedClothing.Contains(followerClothingType))
        DataManager.instance.UnlockedClothing.Remove(followerClothingType);
    }
    if (DataManager.instance.UnlockedFleeces.Contains(1002))
      DataManager.Instance.UnlockedFleeces.Remove(1002);
    if (DataManager.instance.PlayerFleece != 1002)
      return;
    DataManager.instance.PlayerFleece = 0;
    if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null))
      return;
    PlayerFarming.Instance.simpleSpineAnimator?.SetSkin("Lamb_0");
  }

  public static bool ActivatePrePurchaseDLC()
  {
    if (DataManager.Instance.DLC_Pre_Purchase)
      return false;
    DataManager.Instance.DLC_Pre_Purchase = true;
    DataManager.SetFollowerSkinUnlocked("Cthulhu");
    return true;
  }

  public static void DeactivatePrePurchaseDLC()
  {
    DataManager.Instance.DLC_Pre_Purchase = false;
    DataManager.RemoveUnlockedFollowerSkin("Cthulhu");
  }

  public static bool ActivatePlushBonusDLC()
  {
    if (DataManager.Instance.DLC_Plush_Bonus)
      return false;
    DataManager.Instance.DLC_Plush_Bonus = true;
    StructuresData.CompleteResearch(StructureBrain.TYPES.DECORATION_PLUSH);
    return true;
  }

  public static bool ActivatePAXDLC()
  {
    if (DataManager.Instance.PAX_DLC)
      return false;
    DataManager.Instance.PAX_DLC = true;
    DataManager.SetFollowerSkinUnlocked("StarBunny");
    return true;
  }

  public static bool ActivateTwitchDrop1()
  {
    if (DataManager.Instance.Twitch_Drop_1)
      return false;
    DataManager.Instance.Twitch_Drop_1 = true;
    DataManager.SetFollowerSkinUnlocked("TwitchPoggers");
    return true;
  }

  public static bool ActivateTwitchDrop2()
  {
    if (DataManager.Instance.Twitch_Drop_2)
      return false;
    DataManager.Instance.Twitch_Drop_2 = true;
    DataManager.SetFollowerSkinUnlocked("TwitchDog");
    return true;
  }

  public static bool ActivateTwitchDrop3()
  {
    if (DataManager.Instance.Twitch_Drop_3)
      return false;
    DataManager.Instance.Twitch_Drop_3 = true;
    DataManager.SetFollowerSkinUnlocked("TwitchDogAlt");
    return true;
  }

  public static bool ActivateTwitchDrop4()
  {
    if (DataManager.Instance.Twitch_Drop_4)
      return false;
    DataManager.Instance.Twitch_Drop_4 = true;
    DataManager.SetFollowerSkinUnlocked("Lion");
    return true;
  }

  public static bool ActivateTwitchDrop5()
  {
    if (DataManager.Instance.Twitch_Drop_5)
      return false;
    DataManager.Instance.Twitch_Drop_5 = true;
    DataManager.SetFollowerSkinUnlocked("Penguin");
    return true;
  }

  public static bool ActivateTwitchDrop6()
  {
    if (DataManager.Instance.Twitch_Drop_6)
      return false;
    DataManager.Instance.Twitch_Drop_6 = true;
    DataManager.SetFollowerSkinUnlocked("Kiwi");
    return true;
  }

  public static bool ActivateTwitchDrop7()
  {
    if (DataManager.Instance.Twitch_Drop_7)
      return false;
    DataManager.Instance.Twitch_Drop_7 = true;
    DataManager.SetFollowerSkinUnlocked("Pelican");
    return true;
  }

  public static bool ActivateTwitchDrop8()
  {
    if (DataManager.Instance.Twitch_Drop_8)
      return false;
    DataManager.Instance.Twitch_Drop_8 = true;
    StructuresData.CompleteResearch(StructureBrain.TYPES.DECORATION_DST_GLOMMERSTATUE);
    return true;
  }

  public static bool ActivateTwitchDrop9()
  {
    if (DataManager.Instance.Twitch_Drop_9)
      return false;
    DataManager.Instance.Twitch_Drop_9 = true;
    StructuresData.CompleteResearch(StructureBrain.TYPES.DECORATION_DST_BEEFALOSKELETON);
    return true;
  }

  public static bool ActivateTwitchDrop10()
  {
    if (DataManager.Instance.Twitch_Drop_10)
      return false;
    DataManager.Instance.Twitch_Drop_10 = true;
    StructuresData.CompleteResearch(StructureBrain.TYPES.DECORATION_GNOME1);
    return true;
  }

  public static bool ActivateTwitchDrop11()
  {
    if (DataManager.Instance.Twitch_Drop_11)
      return false;
    DataManager.Instance.Twitch_Drop_11 = true;
    StructuresData.CompleteResearch(StructureBrain.TYPES.DECORATION_GNOME2);
    return true;
  }

  public static bool ActivateTwitchDrop12()
  {
    if (DataManager.Instance.Twitch_Drop_12)
      return false;
    DataManager.Instance.Twitch_Drop_12 = true;
    StructuresData.CompleteResearch(StructureBrain.TYPES.DECORATION_GNOME3);
    return true;
  }

  public static bool ActivateTwitchDrop13()
  {
    if (DataManager.Instance.Twitch_Drop_13)
      return false;
    DataManager.Instance.Twitch_Drop_13 = true;
    StructuresData.CompleteResearch(StructureBrain.TYPES.DECORATION_GOAT_LANTERN);
    return true;
  }

  public static bool ActivateTwitchDrop14()
  {
    if (DataManager.Instance.Twitch_Drop_14)
      return false;
    DataManager.Instance.Twitch_Drop_14 = true;
    StructuresData.CompleteResearch(StructureBrain.TYPES.DECORATION_GOAT_PLANT);
    return true;
  }

  public static bool ActivateTwitchDrop15()
  {
    if (DataManager.Instance.Twitch_Drop_15)
      return false;
    DataManager.Instance.Twitch_Drop_15 = true;
    StructuresData.CompleteResearch(StructureBrain.TYPES.DECORATION_GOAT_STATUE);
    return true;
  }

  public static bool ActivateTwitchDrop16()
  {
    if (DataManager.Instance.Twitch_Drop_16)
      return false;
    DataManager.Instance.Twitch_Drop_16 = true;
    DataManager.SetFollowerSkinUnlocked("Anglerfish");
    return true;
  }

  public static bool ActivateTwitchDrop17()
  {
    if (DataManager.Instance.Twitch_Drop_17)
      return false;
    DataManager.Instance.Twitch_Drop_17 = true;
    DataManager.SetFollowerSkinUnlocked("SeaButterfly");
    return true;
  }

  public static bool ActivateTwitchDrop18()
  {
    if (DataManager.Instance.Twitch_Drop_18)
      return false;
    DataManager.Instance.Twitch_Drop_18 = true;
    DataManager.SetFollowerSkinUnlocked("Jellyfish");
    return true;
  }

  public static bool ActivateTwitchDrop19()
  {
    if (DataManager.Instance.Twitch_Drop_19)
      return false;
    DataManager.Instance.Twitch_Drop_19 = true;
    DataManager.SetFollowerSkinUnlocked("Leech");
    return true;
  }

  public static bool ActivateTwitchDrop20()
  {
    if (DataManager.Instance.Twitch_Drop_20)
      return false;
    DataManager.Instance.Twitch_Drop_20 = true;
    DataManager.SetFollowerSkinUnlocked("LizardTongue");
    return true;
  }

  public static bool ActivateSupportStreamer()
  {
    if (DataManager.Instance.SupportStreamer)
      return false;
    DataManager.Instance.SupportStreamer = true;
    DataManager.SetFollowerSkinUnlocked("DogTeddy");
    StructuresData.CompleteResearch(StructureBrain.TYPES.DECORATION_FLOWER_BOTTLE);
    StructuresData.CompleteResearch(StructureBrain.TYPES.DECORATION_MUSHROOM_CANDLE_2);
    StructuresData.CompleteResearch(StructureBrain.TYPES.DECORATION_LEAFY_SCULPTURE);
    return true;
  }

  public static bool ActivateMajorDLC()
  {
    if (DataManager.Instance.MAJOR_DLC)
      return false;
    DataManager.Instance.MAJOR_DLC = true;
    return true;
  }

  public static void UnlockRelic(RelicType relicType)
  {
    if (DataManager.instance.PlayerFoundRelics.Contains(relicType))
      return;
    DataManager.instance.PlayerFoundRelics.Add(relicType);
    if (EquipmentManager.CoopRelics.Contains(relicType) || EquipmentManager.MajorDLCRelics.Contains(relicType))
      return;
    int num1 = 0;
    for (int index = 0; index < DataManager.instance.PlayerFoundRelics.Count; ++index)
    {
      if (!EquipmentManager.CoopRelics.Contains(DataManager.instance.PlayerFoundRelics[index]) && !EquipmentManager.MajorDLCRelics.Contains(DataManager.instance.PlayerFoundRelics[index]))
        ++num1;
    }
    int num2 = 0;
    for (int index = 0; index < EquipmentManager.RelicData.Length; ++index)
    {
      if (!EquipmentManager.CoopRelics.Contains(EquipmentManager.RelicData[index].RelicType) && !EquipmentManager.MajorDLCRelics.Contains(EquipmentManager.RelicData[index].RelicType))
        ++num2;
    }
    if (num1 < num2)
      return;
    AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("ALL_RELICS_UNLOCKED"));
    Debug.Log((object) "ACHIEVEMENT GOT : ALL_RELICS_UNLOCKED");
  }

  public bool HasDeathCatFollower()
  {
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
    {
      if (follower.SkinName == "Boss Death Cat")
        return true;
    }
    return false;
  }

  public int GetRottingFollowers()
  {
    int rottingFollowers = 0;
    foreach (FollowerInfo follower in this.Followers)
    {
      if (follower != null && follower.Traits != null && follower.Traits.Contains(FollowerTrait.TraitType.Mutated))
        ++rottingFollowers;
    }
    return rottingFollowers;
  }

  public static void CompleteDungeonMapNode()
  {
    int currentDlcDungeonId = DataManager.Instance.CurrentDLCDungeonID;
    if (currentDlcDungeonId == -1 || DataManager.Instance.DLCDungeonNodesCompleted.Contains(currentDlcDungeonId))
      return;
    Debug.Log((object) $"Completing dungeon map node for dungeon ID: {currentDlcDungeonId}");
    DataManager.Instance.DLCDungeonNodesCompleted.Add(currentDlcDungeonId);
    DataManager.Instance.DLCDungeonNodeCurrent = currentDlcDungeonId;
    DataManager.Instance.CurrentDLCDungeonID = -1;
  }

  public enum OnboardingPhase
  {
    Off,
    Indoctrinate,
    Shrine,
    Devotion,
    IndoctrinateBerriesAllowed,
    Done,
  }

  public enum Chains
  {
    Chain1,
    Chain2,
    Chain3,
  }

  [MessagePackObject(false)]
  [Serializable]
  public class LocationAndLayer
  {
    [Key(0)]
    public FollowerLocation Location;
    [Key(1)]
    public int Layer;

    public LocationAndLayer()
    {
    }

    public LocationAndLayer(FollowerLocation Location, int Layer)
    {
      this.Location = Location;
      this.Layer = Layer;
    }

    public static bool Contains(
      FollowerLocation Location,
      int Layer,
      List<DataManager.LocationAndLayer> List)
    {
      foreach (DataManager.LocationAndLayer locationAndLayer in List)
      {
        if (locationAndLayer.Location == Location && locationAndLayer.Layer == Layer)
          return true;
      }
      return false;
    }

    public static DataManager.LocationAndLayer ContainsLocation(
      FollowerLocation Location,
      List<DataManager.LocationAndLayer> List)
    {
      foreach (DataManager.LocationAndLayer locationAndLayer in List)
      {
        if (locationAndLayer.Location == Location)
          return locationAndLayer;
      }
      return (DataManager.LocationAndLayer) null;
    }
  }

  public enum CultLevel
  {
    One,
    Two,
    Three,
    Four,
  }

  [MessagePackObject(false)]
  public class LocationSeedsData
  {
    [Key(0)]
    public FollowerLocation Location;
    [Key(1)]
    public int Seed;
  }

  [MessagePackObject(false)]
  [Serializable]
  public struct DungeonCompletedFleeces
  {
    [Key(0)]
    public FollowerLocation Location;
    [Key(1)]
    public List<int> Fleeces;
  }

  [MessagePackObject(false)]
  [Serializable]
  public class ClothingVariant
  {
    [Key(0)]
    public FollowerClothingType ClothingType;
    [Key(1)]
    public int Colour;
    [Key(2)]
    public string Variant;
  }

  public enum Variables
  {
    Rat_Tutorial_Bell,
    Goat_First_Meeting,
    Goat_Guardian_Door_Open,
    Key_Shrine_1,
    Key_Shrine_2,
    Key_Shrine_3,
    InTutorial,
    Tutorial_Rooms_Completed,
    Tutorial_Enable_Store_Resources,
    Tutorial_Completed,
    Create_Tutorial_Rooms,
    RatauExplainsFollowers,
    RatauExplainsDemo,
    SpokenToDeathNPC,
    RatauExplainsTeleporter,
    SozoIntro,
    SozoQuestComplete,
    TarotIntro,
    HasTarotBuilding,
    ForestOfferingRoomCompleted,
    KnucklebonesIntroCompleted,
    ForestChallengeRoom1Completed,
    ForestRescueWorshipper,
    RatauExplainsBiome0,
    RatauExplainsBiome0Boss,
    RatauExplainsBiome1,
    GetFirstFollower,
    RatauExplainBuilding,
    PromoteFollowerExplained,
    FoxMeeting_0,
    FirstMonsterHeart,
    Lighthouse_FirstConvo,
    Lighthouse_LitFirstConvo,
    Lighthouse_FireOutAgain,
    BirdConvo,
    RatOutpostIntro,
    RatExplainDungeon,
    RatExplainTemple,
    HorseTown_PaidRespectToHorse,
    HorseTown_JoinCult,
    HorseTown_OpenedChest,
    Tutorial_Indoctrinate_Begun,
    Tutorial_Indoctrinate_Completed,
    Dungeon1Story1,
    Dungeon1Story2,
    ShowFaith,
    EnabledSpells,
    EnabledHealing,
    FirstFollowerRescue,
    SherpaFirstConvo,
    UnlockedHubShore,
    ResourceRoom1Revealed,
    DecorationRoomFirstConvo,
    FirstTarot,
    Knucklebones_Opponent_Ratau_Won,
    Knucklebones_Opponent_0,
    Knucklebones_Opponent_0_FirstConvoRataus,
    Knucklebones_Opponent_0_Won,
    Knucklebones_Opponent_1_FirstConvoRataus,
    Knucklebones_Opponent_1,
    Knucklebones_Opponent_1_Won,
    Knucklebones_Opponent_2_FirstConvoRataus,
    Knucklebones_Opponent_2,
    Knucklebones_Opponent_2_Won,
    CultLeader1_LastRun,
    CultLeader1_StoryPosition,
    CultLeader2_LastRun,
    CultLeader2_StoryPosition,
    CultLeader3_LastRun,
    CultLeader3_StoryPosition,
    CultLeader4_LastRun,
    CultLeader4_StoryPosition,
    HaroConversationCompleted,
    PlayerHasFollowers,
    DungeonLayer1,
    DungeonLayer2,
    DungeonLayer3,
    DungeonLayer4,
    DungeonLayer5,
    HasBuiltShrine1,
    ShoreFishFirstConvo,
    ShoreFishFished,
    RatauShowShrineShop,
    RatauKilled,
    RatauReadLetter,
    BeatenDungeon1,
    BeatenDungeon2,
    BeatenDungeon3,
    BeatenDungeon4,
    BeatenDeathCat,
    CanFindTarotCards,
    DungeonKeyRoomCompleted1,
    DungeonKeyRoomCompleted2,
    DungeonKeyRoomCompleted3,
    DungeonKeyRoomCompleted4,
    ShoreTarotShotConvo1,
    ShoreTarotShotConvo2,
    ChefShopDoublePrices,
    FollowerShopUses,
    MidasIntro,
    MidasBankUnlocked,
    MidasBankIntro,
    ShoreFlowerShopConvo1,
    MidasDevotionIntro,
    MidasSacrificeIntro,
    EncounteredHealingRoom,
    GivenFreeDungeonFollower,
    BeatenFirstMiniBoss,
    RatooGivenHeart,
    RatooMentionedWrongHeart,
    RatauFoundSkin,
    DiedLastRun,
    RatauToGiveCurseNextRun,
    MidasFoundSkin,
    MinimumRandomRoomsEncountered,
    Tutorial_First_Indoctoring,
    FirstDoctrineStone,
    ForneusLore,
    HaroIntroduceDoctrines,
    SozoBeforeDeath,
    SozoDead,
    FirstDungeon1RescueRoom,
    FirstDungeon2RescueRoom,
    FirstDungeon3RescueRoom,
    FirstDungeon4RescueRoom,
    Lighthouse_QuestGiven,
    Lighthouse_QuestComplete,
    FirstTimeWeaponMarketplace,
    FirstTimeSpiderMarketplace,
    FirstTimeSeedMarketplace,
    ShowHaroDoctrineStoneRoom,
    SozoFoundDecoration,
    RatauGiftMediumCollected,
    SozoFlowerShopConvo1,
    SozoTarotShopConvo1,
    MidasStatue,
    ShowLoyaltyBars,
    CanBuildShrine,
    SandboxModeEnabled,
    OnboardedRelics,
    CanUnlockRelics,
    BeatenOneDungeons,
    BeatenTwoDungeons,
    BeatenThreeDungeons,
    BeatenFourDungeons,
    OnboardedMysticShop,
    OnboardedLayer2,
    BeatenWitnessDungeon1,
    BeatenWitnessDungeon2,
    BeatenWitnessDungeon3,
    BeatenWitnessDungeon4,
    FoundRelicAtHubShore,
    FoundRelicInFish,
    GivenRelicFishRiddle,
    GivenRelicLighthouseRiddle,
    CanFindLeaderRelic,
    PlimboSpecialEncountered,
    KlunkoSpecialEncountered,
    ShowSpecialHaroRoom,
    ShowSpecialMidasRoom,
    ShowSpecialPlimboRoom,
    ShowSpecialKlunkoRoom,
    ShowSpecialLeaderRoom,
    ShowSpecialFishermanRoom,
    ShowSpecialSozoRoom,
    ShowSpecialLighthouseKeeperRoom,
    LighthouseKeeperSpecialEncountered,
    SozoSpecialEncountered,
    FishermanSpecialEncountered,
    BaalAndAymSpecialEncounterd,
    ShowSpecialBaalAndAymRoom,
    MysticKeeperBeatenLeshy,
    MysticKeeperBeatenHeket,
    MysticKeeperBeatenKallamar,
    MysticKeeperBeatenShamura,
    MysticKeeperBeatenAll,
    SeedMarketPlacePostGame,
    HelobPostGame,
    MysticKeeperFirstPurchase,
    ForceMarketplaceCat,
    SurvivalModeActive,
    QuickStartActive,
    ForceClothesShop,
    UnlockedTailor,
    LoreStonesOnboarded,
    LoreStonesHaro,
    LoreStonesMoth,
    OnboardedPleasure,
    RevealedTailor,
    MysticKeeperOnboardedSin,
    ChemachOnboardedSin,
    KlunkoOnboardedTailor1,
    KlunkoOnboardedTailor2,
    AssignedFollowersOutfits,
    SpawnPubResources,
    EnteredHopRoom,
    EnteredGrapeRoom,
    EnteredCottonRoom,
    IsPilgrimRescue,
    IsJalalaBag,
    HealingLeshyQuestActive,
    HealingHeketQuestActive,
    HealingKallamarQuestActive,
    HealingShamuraQuestActive,
    UnlockedCorruptedRelicsAndTarots,
    OnboardedWolf,
    SeasonsActive,
    OnboardedSeasons,
    OnboardedLambTown,
    OnboardedDungeon6,
    OnboardedRanching,
    ForcePalworldEgg,
    RancherSpokeAboutBrokenShop,
    RancherShopFixed,
    EncounteredRancher,
    OnboardedDepositFollowerNPC,
    DepositFinalFollowerNPC,
    ShowMidasKilling,
    CultLeader5_LastRun,
    CultLeader5_StoryPosition,
    CultLeader6_LastRun,
    CultLeader6_StoryPosition,
    BeatenDungeon5,
    BeatenDungeon6,
    FlockadeSpokeAboutBrokenShop,
    FlockadeShopFixed,
    EncounteredFlockade,
    EncounteredBaseExpansionNPC,
    EncounteredTarotShop,
    EncounteredDecoShop,
    EncounteredBlacksmithShop,
    EncounteredGraveyardShop,
    TarotSpokeAboutBrokenShop,
    TarotShopFixed,
    DecoSpokeAboutBrokenShop,
    DecoShopFixed,
    BlacksmithSpokeAboutBrokenShop,
    BlacksmithShopFixed,
    GraveyardSpokeAboutBrokenShop,
    GraveyardShopFixed,
    OnboardedIntroYngyaShrine,
    BeatenWolf,
    BeatenYngya,
    CompletedYngyaFightIntro,
    FindBrokenHammerWeapon,
    ShowIcegoreRoom,
    GaveLegendarySwordChosenChildQuest,
    FoundLegendarySword,
    OnboardedWool,
    FoundLegendaryGauntlets,
    FlockadeOpponent0Won,
    CompletedInfectedNPCQuest,
    TalkedToInfectedNPC,
    OnboardedLambTownGhost7,
    OnboardedLambTownGhost8,
    OnboardedLambTownGhost9,
    OnboardedLambTownGhost10,
    NPCsRescued,
    NPCGhostRancherRescued,
    NPCGhostFlockadeRescued,
    NPCGhostDepositRescued,
    NPCGhostBlacksmithRescued,
    NPCGhostTarotRescued,
    NPCGhostDecoRescued,
    NPCGhostGraveyardRescued,
    NPCGhostGeneric7Rescued,
    NPCGhostGeneric8Rescued,
    NPCGhostGeneric9Rescued,
    NPCGhostGeneric10Rescued,
    FlockadeOpponent1Won,
    FlockadeOpponent2Won,
    FlockadeOpponent3Won,
    SpokenToHaroD6,
    SpokenToRatauWinter,
    SpokenToPlimboWinter,
    SpokenToPlimboBlunderbuss,
    SpokenToClauneckWinter,
    SpokenToKudaiiWinter,
    SpokenToChemachWinter,
    OnboardedRotstone,
    OnboardedFindLostSouls,
    OnboardedRotstoneDungeon,
    OnboardedMutationRoom,
    RancherOnboardedHolyYew,
    OnboardedRotHelobFollowers,
    PlimboRejectedRotEye,
    BlizzardOfferingsGiven,
    BlizzardOfferedThisWinter,
    BeatenExecutioner,
    FlockadeBlacksmithWon,
    FlockadeDecoWon,
    FlockadeFlockadeWon,
    FlockadeGraveyardWon,
    FlockadeRancherWon,
    FlockadeTarotWon,
    OnboardedLightningShardDungeon,
    OnboardedRanchingJobBoard,
    OnboardedFlockadeJobBoard,
    OnboardedBlacksmithJobBoard,
    OnboardedTarotJobBoard,
    OnboardedDecoJobBoard,
    OnboardedGraveyardJobBoard,
    BlizzardOfferingsCompleted,
    CompletedRanchingJobBoard,
    CompletedFlockadeJobBoard,
    CompletedBlacksmithJobBoard,
    CompletedTarotJobBoard,
    CompletedDecoJobBoard,
    CompletedGraveyardJobBoard,
    DungeonBossFight,
    OnboardedLegendaryWeapons,
    RepairedLegendarySword,
    RepairedLegendaryAxe,
    RepairedLegendaryHammer,
    RepairedLegendaryGauntlet,
    RepairedLegendaryBlunderbuss,
    RepairedLegendaryDagger,
    RepairedLegendaryChains,
    RatooNeedsRescue,
    LegendaryBlunderbussPlimboEasterEggActive,
    SpokenToChemachRot,
    SpokenToKudaiiRot,
    SpokenToClauneckRot,
    StelleSpecialEncountered,
    MonchMamaSpecialEncountered,
    FishermanDLCSpecialEncountered,
    ShowSpecialStelleRoom,
    ShowSpecialMonchMamaRoom,
    ShowSpecialFishermanDLCRoom,
    InfectedDudeSpecialEncountered,
    DungeonRancherSpecialEncountered,
    RefinedResourcesSpecialEncountered,
    ShowSpecialInfectedDudeRoom,
    ShowSpecialDungeonRancherRoom,
    ShowSpecialRefinedResourcesRoom,
    StripperGaveOutfit,
    OnboardedRotRoom,
    RevealedDLCMapDoor,
    RevealedDLCMapHeart,
    CanShowExecutionerRoom1,
    CanShowExecutionerRoom2,
    ExecutionerRoom1Encountered,
    ExecutionerRoom2Encountered,
    EncounteredIcegoreRoom,
    BaalNeedsRescue,
    BaalRescued,
    EnabledDLCMapHeart,
    NPCGhostGeneric11Rescued,
    FoundHollowKnightWool,
    FinalDLCMap,
    DragonIntrod,
    WinterServerity,
    MysticKeeperBeatenYngya,
    SpokenToMysticKeeperWinter,
    FlockadeBlacksmithWoolWon,
    FlockadeDecoWoolWon,
    FlockadeFlockadeWoolWon,
    FlockadeGraveyardWoolWon,
    FlockadeRancherWoolWon,
    FlockadeTarotWoolWon,
    LegendarySwordHinted,
    LegendaryDaggerHinted,
    LegendaryGauntletsHinted,
    LegendaryBlunderbussHinted,
    FoundLegendaryDagger,
    FoundLegendaryBlunderbuss,
    EncounteredSabnock,
    ForceSinRoom,
    ForceHeartRoom,
    ForceDragonRoom,
    RevealedWolfNode,
    Total,
  }

  [MessagePackObject(false)]
  [Serializable]
  public class Offering
  {
    [Key(0)]
    public InventoryItem.ITEM_TYPE Type;
    [Key(1)]
    public int Quantity;
  }

  [MessagePackObject(false)]
  [Serializable]
  public class DepositedFollower
  {
    [Key(0)]
    public FollowerInfo FollowerInfo;
    [Key(1)]
    public int DepositedDay;
    [Key(2)]
    public bool Hatched;
  }

  [MessagePackObject(false)]
  [Serializable]
  public class WoolhavenFlowerPot
  {
    [Key(0)]
    public int ID;
    [Key(1)]
    public int FlowersAdded;
  }

  [MessagePackObject(false)]
  public class QuestHistoryData
  {
    [Key(0)]
    public int QuestIndex;
    [Key(1)]
    public float QuestTimestamp;
    [Key(2)]
    public float QuestCooldownDuration;
    [Key(3)]
    public bool IsStory;
  }

  public enum DecorationType
  {
    None,
    Dungeon1,
    Mushroom,
    Crystal,
    Spider,
    All,
    Path,
    DLC,
    Special_Events,
    Rot,
    Wolf,
    Major_DLC,
    Woolhaven,
  }

  public enum DecorationMajorDLCGrouping
  {
    None,
    Major_DLC,
    Woolhaven,
    Ewefall,
    Rot,
  }

  public delegate void ChangeToolAction(int PrevTool, int NewTool);

  public enum GhostLostLambState
  {
    DoNotAppear,
    AppearInBase,
    AppearInLambTown,
    AlreadyInLambTown,
  }

  [MessagePackObject(false)]
  public class EnemyData
  {
    [Key(0)]
    public Enemy EnemyType;
    [Key(1)]
    public int AmountKilled = 1;
  }

  public sealed class DataManagerFormatter : 
    IMessagePackFormatter<DataManager>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      DataManager value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(1395);
        writer.Write(value.AllowSaving);
        writer.Write(value.DisableSaving);
        writer.Write(value.PauseGameTime);
        writer.Write(value.GameOverEnabled);
        writer.Write(value.DisplayGameOverWarning);
        writer.Write(value.InGameOver);
        writer.Write(value.GameOver);
        writer.Write(value.DifficultyChosen);
        writer.Write(value.DifficultyReminded);
        writer.Write(value.DisableYngyaShrine);
        writer.Write(value.playerDeaths);
        writer.Write(value.playerDeathsInARow);
        writer.Write(value.playerDeathsInARowFightingLeader);
        writer.Write(value.FightPitRituals);
        writer.Write(value.dungeonRun);
        writer.Write(value.dungeonRunDuration);
        resolver.GetFormatterWithVerify<List<Map.NodeType>>().Serialize(ref writer, value.dungeonVisitedRooms, options);
        resolver.GetFormatterWithVerify<List<FollowerLocation>>().Serialize(ref writer, value.dungeonLocationsVisited, options);
        resolver.GetFormatterWithVerify<List<int>>().Serialize(ref writer, value.FollowersRecruitedInNodes, options);
        writer.Write(value.FollowersRecruitedThisNode);
        writer.Write(value.TimeInGame);
        writer.Write(value.KillsInGame);
        writer.Write(value.dungeonRunXPOrbs);
        writer.Write(value.ChestRewardCount);
        writer.Write(value.BaseGoopDoorLocked);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.BaseGoopDoorLoc, options);
        writer.Write(value.STATS_FollowersStarvedToDeath);
        writer.Write(value.STATS_Murders);
        writer.Write(value.STATS_Sacrifices);
        writer.Write(value.STATS_AnimalSacrifices);
        writer.Write(value.STATS_NaturalDeaths);
        writer.Write(value.PlayerKillsOnRun);
        writer.Write(value.PlayerStartingBlackSouls);
        writer.Write(value.GivenFollowerHearts);
        writer.Write(value.EnabledSpells);
        writer.Write(value.ForceDoctrineStones);
        writer.Write(value.SpaceOutDoctrineStones);
        writer.Write(value.DoctrineStoneTotalCount);
        writer.Write(value.BuildShrineEnabled);
        writer.Write(value.EnabledHealing);
        writer.Write(value.EnabledSword);
        writer.Write(value.BonesEnabled);
        writer.Write(value.XPEnabled);
        writer.Write(value.ShownDodgeTutorial);
        writer.Write(value.ShownInventoryTutorial);
        writer.Write(value.ShownDodgeTutorialCount);
        writer.Write(value.HadInitialDeathCatConversation);
        writer.Write(value.PlayerHasBeenGivenHearts);
        writer.Write(value.TotalFirefliesCaught);
        writer.Write(value.TotalSquirrelsCaught);
        writer.Write(value.TotalBirdsCaught);
        writer.Write(value.TotalBodiesHarvested);
        resolver.GetFormatterWithVerify<DataManager.OnboardingPhase>().Serialize(ref writer, value.CurrentOnboardingPhase, options);
        writer.Write(value.firstRecruit);
        writer.Write(value.MissionariesCompleted);
        writer.Write(value.PlayerFleece);
        writer.Write(value.PlayerVisualFleece);
        resolver.GetFormatterWithVerify<List<int>>().Serialize(ref writer, value.UnlockedFleeces, options);
        writer.Write(value.PostGameFleecesOnboarded);
        writer.Write(value.GoatFleeceOnboarded);
        writer.Write(value.CowboyFleeceOnboarded);
        resolver.GetFormatterWithVerify<List<ThoughtData>>().Serialize(ref writer, value.Thoughts, options);
        writer.Write(value.CanReadMinds);
        writer.Write(value.HappinessEnabled);
        writer.Write(value.TeachingsEnabled);
        writer.Write(value.SchedulingEnabled);
        writer.Write(value.PrayerEnabled);
        writer.Write(value.PrayerOrdered);
        writer.Write(value.HasBuiltCookingFire);
        writer.Write(value.HasBuiltFarmPlot);
        writer.Write(value.HasBuiltTemple1);
        writer.Write(value.HasBuiltTemple2);
        writer.Write(value.HasBuiltTemple3);
        writer.Write(value.HasBuiltTemple4);
        writer.Write(value.HasBuiltShrine1);
        writer.Write(value.HasBuiltShrine2);
        writer.Write(value.HasBuiltShrine3);
        writer.Write(value.HasBuiltShrine4);
        writer.Write(value.HasBuiltPleasureShrine);
        writer.Write(value.HasOnboardedCultLevel);
        writer.Write(value.PerformedMushroomRitual);
        writer.Write(value.BuiltMushroomDecoration);
        writer.Write(value.HasBuiltSurveillance);
        writer.Write(value.TempleDevotionBoxCoinCount);
        writer.Write(value.CanBuildShrine);
        writer.Write(value.DaySinseLastMutatedFollower);
        writer.Write(value.SeasonsActive);
        writer.Write(value.SeasonTimestamp);
        resolver.GetFormatterWithVerify<SeasonsManager.Season>().Serialize(ref writer, value.CurrentSeason, options);
        resolver.GetFormatterWithVerify<SeasonsManager.Season>().Serialize(ref writer, value.PreviousSeason, options);
        resolver.GetFormatterWithVerify<SeasonsManager.WeatherEvent>().Serialize(ref writer, value.CurrentWeatherEvent, options);
        writer.Write(value.BlizzardEventID);
        writer.Write(value.WeatherEventID);
        writer.Write(value.WintersOccured);
        writer.Write(value.WinterServerity);
        writer.Write(value.NextWinterServerity);
        writer.Write(value.NextPhaseIsWeatherEvent);
        writer.Write(value.GivenBlizzardObjective);
        writer.Write(value.OnboardedDLCEntrance);
        writer.Write(value.OnboardedBaseExpansion);
        writer.Write(value.OnboardedWolf);
        writer.Write(value.OnboardedLambTown);
        writer.Write(value.OnboardedLambGhostNPCs);
        writer.Write(value.OnboardedYngyaAwoken);
        writer.Write(value.OnboardedDungeon6);
        writer.Write(value.OnboardedIntroYngyaShrine);
        writer.Write(value.OnboardedFindLostSouls);
        writer.Write(value.OnboardedAddFuelToFurnace);
        writer.Write(value.RequiresSnowedUnderOnboarded);
        writer.Write(value.RequiresWolvesOnboarded);
        writer.Write(value.WinterMaxSeverity);
        writer.Write(value.RequiresBlizzardOnboarded);
        writer.Write(value.OnboardedRanchingWolves);
        writer.Write(value.OnboardedBlizzards);
        writer.Write(value.OnboardedSnowedUnder);
        writer.Write(value.OnboardedWitheredCrops);
        writer.Write(value.OnboardedLongNights);
        writer.Write(value.LongNightActive);
        writer.Write(value.OnboardedRanching);
        writer.Write(value.OnboardedSeasons);
        writer.Write(value.OnboardedDLCBuildMenu);
        writer.Write(value.DLCUpgradeTreeSnowIncrement);
        writer.Write(value.BuiltFurnace);
        writer.Write(value.OnboardedLightningShardDungeon);
        writer.Write(value.OnboardedRotstoneDungeon);
        writer.Write(value.OnboardedRotstone);
        writer.Write(value.CollectedRotstone);
        writer.Write(value.CollectedYewMutated);
        writer.Write(value.OnboardedMutationRoom);
        writer.Write(value.OnboardedRotHelobFollowers);
        writer.Write(value.PlayedFinalYngyaConvo);
        writer.Write(value.YngyaOffering);
        writer.Write(value.YngyaRotOfferingsReceived);
        writer.Write(value.SpokeToYngyaOnMountainTop);
        writer.Write(value.ShowIcegoreRoom);
        writer.Write(value.EncounteredIcegoreRoom);
        writer.Write(value.SpokenToRatauWinter);
        writer.Write(value.SpokenToPlimboWinter);
        writer.Write(value.SpokenToPlimboBlunderbuss);
        writer.Write(value.SpokenToClauneckWinter);
        writer.Write(value.SpokenToKudaiiWinter);
        writer.Write(value.SpokenToChemachWinter);
        writer.Write(value.SpokenToChemachRot);
        writer.Write(value.SpokenToKudaiiRot);
        writer.Write(value.SpokenToClauneckRot);
        resolver.GetFormatterWithVerify<List<int>>().Serialize(ref writer, value.DLCDungeonNodesCompleted, options);
        writer.Write(value.DLCDungeonNodeCurrent);
        writer.Write(value.DLCKey_1);
        writer.Write(value.DLCKey_2);
        writer.Write(value.DLCKey_3);
        writer.Write(value.RevealedPostGameDungeon5);
        writer.Write(value.RevealedPostGameDungeon6);
        writer.Write(value.RevealDLCDungeonNode);
        writer.Write(value.CurrentDLCDungeonID);
        resolver.GetFormatterWithVerify<DungeonWorldMapIcon.NodeType>().Serialize(ref writer, value.CurrentDLCNodeType, options);
        writer.Write(value.HasYngyaConvo);
        writer.Write(value.IsMiniBoss);
        writer.Write(value.IsLambGhostRescue);
        writer.Write(value.DLCDungeon5MiniBossIndex);
        writer.Write(value.DLCDungeon6MiniBossIndex);
        writer.Write(value.PuzzleRoomsCompleted);
        writer.Write(value.RancherSpokeAboutBrokenShop);
        writer.Write(value.RancherShopFixed);
        writer.Write(value.RancherOnboardedLightningShards);
        writer.Write(value.RancherOnboardedHolyYew);
        writer.Write(value.FlockadeSpokeAboutBrokenShop);
        writer.Write(value.FlockadeShopFixed);
        writer.Write(value.FlockadeBlacksmithWon);
        writer.Write(value.FlockadeDecoWon);
        writer.Write(value.FlockadeFlockadeWon);
        writer.Write(value.FlockadeGraveyardWon);
        writer.Write(value.FlockadeRancherWon);
        writer.Write(value.FlockadeTarotWon);
        writer.Write(value.TarotSpokeAboutBrokenShop);
        writer.Write(value.TarotShopFixed);
        writer.Write(value.DecoSpokeAboutBrokenShop);
        writer.Write(value.DecoShopFixed);
        writer.Write(value.BlacksmithSpokeAboutBrokenShop);
        writer.Write(value.BlacksmithShopFixed);
        writer.Write(value.GraveyardSpokeAboutBrokenShop);
        writer.Write(value.GraveyardShopFixed);
        writer.Write(value.OnboardedLambTownGhost7);
        writer.Write(value.OnboardedLambTownGhost8);
        writer.Write(value.OnboardedLambTownGhost9);
        writer.Write(value.OnboardedLambTownGhost10);
        writer.Write(value.NPCsRescued);
        writer.Write(value.NPCGhostRancherRescued);
        writer.Write(value.NPCGhostFlockadeRescued);
        writer.Write(value.NPCGhostTarotRescued);
        writer.Write(value.NPCGhostDecoRescued);
        writer.Write(value.NPCGhostBlacksmithRescued);
        writer.Write(value.NPCGhostGraveyardRescued);
        writer.Write(value.NPCGhostGeneric7Rescued);
        writer.Write(value.NPCGhostGeneric8Rescued);
        writer.Write(value.NPCGhostGeneric9Rescued);
        writer.Write(value.NPCGhostGeneric10Rescued);
        writer.Write(value.RepairedLegendarySword);
        writer.Write(value.RepairedLegendaryAxe);
        writer.Write(value.RepairedLegendaryHammer);
        writer.Write(value.RepairedLegendaryGauntlet);
        writer.Write(value.RepairedLegendaryBlunderbuss);
        writer.Write(value.RepairedLegendaryDagger);
        writer.Write(value.RepairedLegendaryChains);
        writer.Write(value.OnboardedLegendaryWeapons);
        writer.Write(value.FindBrokenHammerWeapon);
        writer.Write(value.GivenBrokenHammerWeaponQuest);
        writer.Write(value.GaveChosenChildQuest);
        writer.Write(value.ChosenChildLeftInTheMidasCave);
        writer.Write(value.FoundLegendarySword);
        writer.Write(value.ChosenChildMeditationQuestDay);
        writer.Write(value.LegendarySwordHinted);
        writer.Write(value.LegendaryAxeHinted);
        writer.Write(value.DeliveredCharybisLetter);
        writer.Write(value.BringFishermanWoolStarted);
        writer.Write(value.FishermanGaveWoolAmount);
        writer.Write(value.BroughtFishingRod);
        writer.Write(value.LegendaryDaggerHinted);
        writer.Write(value.FoundLegendaryGauntlets);
        writer.Write(value.LegendaryGauntletsHinted);
        writer.Write(value.LegendaryBlunderbussHinted);
        writer.Write(value.LegendaryBlunderbussPlimboEaterEggTalked);
        resolver.GetFormatterWithVerify<List<EquipmentType>>().Serialize(ref writer, value.KudaaiLegendaryWeaponsResponses, options);
        resolver.GetFormatterWithVerify<List<EquipmentType>>().Serialize(ref writer, value.LegendaryWeaponsUnlockOrder, options);
        writer.Write(value.EncounteredBaseExpansionNPC);
        writer.Write(value.WeatherEventTriggeredDay);
        writer.Write(value.WeatherEventOverTime);
        writer.Write(value.WeatherEventDurationTime);
        writer.Write(value.SeasonSpecialEventTriggeredDay);
        writer.Write(value.TimeSinceLastSnowedUnderStructure);
        writer.Write(value.TimeSinceLastLightingStrikedFollower);
        writer.Write(value.TimeSinceLastLightingStrikedStructure);
        writer.Write(value.TimeSinceLastAflamedStructure);
        writer.Write(value.TimeSinceLastAflamedFollower);
        writer.Write(value.TimeSinceLastStolenCoins);
        writer.Write(value.TimeSinceLastMurderedFollowerFromFollower);
        writer.Write(value.TimeSinceLastSnowPileSpawned);
        writer.Write(value.Temperature);
        writer.Write(value.FollowerOnboardedWinterComing);
        writer.Write(value.FollowerOnboardedWinterHere);
        writer.Write(value.FollowerOnboardedWinterAlmostHere);
        writer.Write(value.FollowerOnboardedBlizzard);
        writer.Write(value.FollowerOnboardedFreezing);
        writer.Write(value.FollowerOnboardedOverheating);
        writer.Write(value.FollowerOnboardedWoolyShack);
        writer.Write(value.FollowerOnboardedRanchChoppingBlock);
        writer.Write(value.FollowerOnboardedAutumnComing);
        writer.Write(value.FollowerOnboardedTyphoon);
        writer.Write(value.TriedTailorRequiresRevealingFromBase);
        writer.Write(value.TailorRequiresRevealingFromBase);
        writer.Write(value.NudeClothingCount);
        writer.Write(value.SinSermonEnabled);
        writer.Write(value.PleasureRevealed);
        writer.Write(value.PleasureDoctrineOnboarded);
        writer.Write(value.PleasureDoctrineEnabled);
        writer.Write(value.WinterDoctrineEnabled);
        writer.Write(value.WokeUpEveryoneDay);
        writer.Write(value.DiedLastRun);
        resolver.GetFormatterWithVerify<Lamb.UI.DeathScreen.UIDeathScreenOverlayController.Results>().Serialize(ref writer, value.LastRunResults, options);
        writer.Write(value.LastFollowerToStarveToDeath);
        writer.Write(value.LastFollowerToFreezeToDeath);
        writer.Write(value.LastFollowerToOverheatToDeath);
        writer.Write(value.LastFollowerToBurnToDeath);
        writer.Write(value.LastFollowerToStartStarving);
        writer.Write(value.LastFollowerToStartDissenting);
        writer.Write(value.LastFollowerToStartFreezing);
        writer.Write(value.LastFollowerToStartSoaking);
        writer.Write(value.LastFollowerToStartOverheating);
        writer.Write(value.LastFollowerToReachOldAge);
        writer.Write(value.LastFollowerToBecomeIll);
        writer.Write(value.LastFollowerToBecomeIllFromSleepingNearIllFollower);
        writer.Write(value.LastFollowerToPassOut);
        writer.Write(value.LastFollowerPurchasedFromSpider);
        writer.Write(value.TimeSinceFaithHitEmpty);
        writer.Write(value.TimeSinceLastCrisisOfFaithQuest);
        resolver.GetFormatterWithVerify<List<FollowerLocation>>().Serialize(ref writer, value.PalworldSkinsGivenLocations, options);
        resolver.GetFormatterWithVerify<List<string>>().Serialize(ref writer, value.PalworldEggSkinsGiven, options);
        writer.Write(value.PalworldEggsCollected);
        writer.Write(value.ForcePalworldEgg);
        writer.Write(value.JudgementAmount);
        writer.Write(value.HungerBarCount);
        writer.Write(value.IllnessBarCount);
        writer.Write(value.IllnessBarDynamicMax);
        writer.Write(value.WarmthBarCount);
        writer.Write(value.StaticFaith);
        writer.Write(value.CultFaith);
        writer.Write(value.LastBrainwashed);
        writer.Write(value.LastHolidayDeclared);
        writer.Write(value.LastPurgeDeclared);
        writer.Write(value.LastNudismDeclared);
        writer.Write(value.LastWorkThroughTheNight);
        writer.Write(value.LastConstruction);
        writer.Write(value.LastEnlightenment);
        writer.Write(value.LastFastDeclared);
        writer.Write(value.LastWarmthRitualDeclared);
        writer.Write(value.LastFeastDeclared);
        writer.Write(value.LastFishingDeclared);
        writer.Write(value.LastHalloween);
        writer.Write(value.LastCNY);
        writer.Write(value.LastCthulhu);
        writer.Write(value.LastRanchRitualMeat);
        writer.Write(value.LastRanchRitualHarvest);
        writer.Write(value.LastArcherShot);
        writer.Write(value.LastSimpleGuardianAttacked);
        writer.Write(value.LastSimpleGuardianRingProjectiles);
        writer.Write(value.LastSimpleGuardianPatternShot);
        writer.Write(value.LastDayTreesAtBase);
        writer.Write(value.LastSnowPileAtBase);
        writer.Write(value.PreviousSermonDayIndex);
        writer.Write(value.PreviousSinSermonDayIndex);
        resolver.GetFormatterWithVerify<SermonCategory>().Serialize(ref writer, value.PreviousSermonCategory, options);
        writer.Write(value.ShrineLevel);
        writer.Write(value.TempleLevel);
        writer.Write(value.TempleBorder);
        writer.Write(value.TempleUnlockedBorder5);
        writer.Write(value.TempleUnlockedBorder6);
        writer.Write(value.GivenSermonQuest);
        writer.Write(value.GivenFaithOfFlockQuest);
        writer.Write(value.PrayedAtMassiveMonsterShrine);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.TwitchSecretKey, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.TwitchToken, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.ChannelID, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.ChannelName, options);
        resolver.GetFormatterWithVerify<List<string>>().Serialize(ref writer, value.ReadTwitchMessages, options);
        writer.Write(value.TotemContributions);
        writer.Write(value.TwitchSentFollowers);
        resolver.GetFormatterWithVerify<TwitchSettings>().Serialize(ref writer, value.TwitchSettings, options);
        writer.Write(value.TwitchTotemsCompleted);
        writer.Write(value.TwitchNextHHEvent);
        resolver.GetFormatterWithVerify<List<string>>().Serialize(ref writer, value.TwitchFollowerViewerIDs, options);
        resolver.GetFormatterWithVerify<List<string>>().Serialize(ref writer, value.TwitchFollowerIDs, options);
        writer.Write(value.OnboardingFinished);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.SaveUniqueID, options);
        resolver.GetFormatterWithVerify<List<string>>().Serialize(ref writer, value.enemiesEncountered, options);
        writer.Write(value.Chain1);
        writer.Write(value.Chain2);
        writer.Write(value.Chain3);
        writer.Write(value.DoorRoomChainProgress);
        writer.Write(value.DoorRoomDoorsProgress);
        writer.Write(value.Dungeon1_Layer);
        writer.Write(value.Dungeon2_Layer);
        writer.Write(value.Dungeon3_Layer);
        writer.Write(value.Dungeon4_Layer);
        writer.Write(value.Dungeon5_Layer);
        writer.Write(value.Dungeon6_Layer);
        writer.Write(value.WinterLoopEnabled);
        writer.Write(value.WinterLoopModifiedDay);
        writer.Write(value.ValentinsDayYear);
        resolver.GetFormatterWithVerify<List<string>>().Serialize(ref writer, value.CheatHistory, options);
        writer.Write(value.First_Dungeon_Resurrecting);
        writer.Write(value.PermadeDeathActive);
        writer.Write(value.SpidersCaught);
        writer.Write(value.FrogFollowerCount);
        writer.Write(value.PhotoCameraLookedAtGallery);
        writer.Write(value.PhotoUsedCamera);
        resolver.GetFormatterWithVerify<List<FollowerClothingType>>().Serialize(ref writer, value.clothesCrafted, options);
        resolver.GetFormatterWithVerify<List<MiniBossController.MiniBossData>>().Serialize(ref writer, value.MiniBossData, options);
        resolver.GetFormatterWithVerify<List<DataManager.LocationAndLayer>>().Serialize(ref writer, value.CachePreviousRun, options);
        resolver.GetFormatterWithVerify<List<FollowerLocation>>().Serialize(ref writer, value.DiscoveredLocations, options);
        resolver.GetFormatterWithVerify<List<FollowerLocation>>().Serialize(ref writer, value.VisitedLocations, options);
        resolver.GetFormatterWithVerify<List<FollowerLocation>>().Serialize(ref writer, value.NewLocationFaithReward, options);
        resolver.GetFormatterWithVerify<List<FollowerLocation>>().Serialize(ref writer, value.DissentingFolllowerRooms, options);
        resolver.GetFormatterWithVerify<List<DataManager.LocationAndLayer>>().Serialize(ref writer, value.OpenedDungeonDoors, options);
        resolver.GetFormatterWithVerify<List<string>>().Serialize(ref writer, value.KeyPiecesFromLocation, options);
        resolver.GetFormatterWithVerify<List<FollowerLocation>>().Serialize(ref writer, value.UsedFollowerDispensers, options);
        resolver.GetFormatterWithVerify<List<FollowerLocation>>().Serialize(ref writer, value.UnlockedBossTempleDoor, options);
        resolver.GetFormatterWithVerify<List<FollowerLocation>>().Serialize(ref writer, value.UnlockedDungeonDoor, options);
        resolver.GetFormatterWithVerify<List<FollowerLocation>>().Serialize(ref writer, value.BossesCompleted, options);
        resolver.GetFormatterWithVerify<List<FollowerLocation>>().Serialize(ref writer, value.BossesEncountered, options);
        resolver.GetFormatterWithVerify<List<FollowerLocation>>().Serialize(ref writer, value.DoorRoomBossLocksDestroyed, options);
        resolver.GetFormatterWithVerify<List<FollowerLocation>>().Serialize(ref writer, value.SignPostsRead, options);
        writer.Write(value.ShrineDoor);
        resolver.GetFormatterWithVerify<List<int>>().Serialize(ref writer, value.JobBoardsClaimedQuests, options);
        writer.Write(value.OnboardedRanchingJobBoard);
        writer.Write(value.CompletedRanchingJobBoard);
        writer.Write(value.OnboardedFlockadeJobBoard);
        writer.Write(value.CompletedFlockadeJobBoard);
        writer.Write(value.OnboardedBlacksmithJobBoard);
        writer.Write(value.CompletedBlacksmithJobBoard);
        writer.Write(value.OnboardedTarotJobBoard);
        writer.Write(value.CompletedTarotJobBoard);
        writer.Write(value.OnboardedDecoJobBoard);
        writer.Write(value.CompletedDecoJobBoard);
        writer.Write(value.OnboardedGraveyardJobBoard);
        writer.Write(value.CompletedGraveyardJobBoard);
        writer.Write(value.HasFurnace);
        writer.Write(value.BaseDoorEast);
        writer.Write(value.BaseDoorNorthEast);
        writer.Write(value.BaseDoorNorthWest);
        writer.Write(value.BossForest);
        writer.Write(value.ForestTempleDoor);
        resolver.GetFormatterWithVerify<List<int>>().Serialize(ref writer, value.CompletedQuestFollowerIDs, options);
        resolver.GetFormatterWithVerify<DataManager.CultLevel>().Serialize(ref writer, value.CurrentCultLevel, options);
        resolver.GetFormatterWithVerify<List<UnlockManager.UnlockType>>().Serialize(ref writer, value.MechanicsUnlocked, options);
        resolver.GetFormatterWithVerify<List<SermonsAndRituals.SermonRitualType>>().Serialize(ref writer, value.UnlockedSermonsAndRituals, options);
        resolver.GetFormatterWithVerify<List<StructureBrain.TYPES>>().Serialize(ref writer, value.UnlockedStructures, options);
        resolver.GetFormatterWithVerify<List<StructureBrain.TYPES>>().Serialize(ref writer, value.HistoryOfStructures, options);
        resolver.GetFormatterWithVerify<Dictionary<StructureBrain.TYPES, int>>().Serialize(ref writer, value.DayPreviosulyUsedStructures, options);
        writer.Write(value.NewBuildings);
        resolver.GetFormatterWithVerify<List<TutorialTopic>>().Serialize(ref writer, value.RevealedTutorialTopics, options);
        resolver.GetFormatterWithVerify<List<StructuresData.ResearchObject>>().Serialize(ref writer, value.CurrentResearch, options);
        resolver.GetFormatterWithVerify<Lamb.UI.UpgradeTreeNode.TreeTier>().Serialize(ref writer, value.CurrentUpgradeTreeTier, options);
        resolver.GetFormatterWithVerify<Lamb.UI.UpgradeTreeNode.TreeTier>().Serialize(ref writer, value.DLCCurrentUpgradeTreeTier, options);
        resolver.GetFormatterWithVerify<Lamb.UI.UpgradeTreeNode.TreeTier>().Serialize(ref writer, value.CurrentPlayerUpgradeTreeTier, options);
        resolver.GetFormatterWithVerify<UpgradeSystem.Type>().Serialize(ref writer, value.MostRecentTreeUpgrade, options);
        resolver.GetFormatterWithVerify<UpgradeSystem.Type>().Serialize(ref writer, value.MostRecentPlayerTreeUpgrade, options);
        resolver.GetFormatterWithVerify<List<UpgradeSystem.Type>>().Serialize(ref writer, value.UnlockedUpgrades, options);
        resolver.GetFormatterWithVerify<List<DoctrineUpgradeSystem.DoctrineType>>().Serialize(ref writer, value.DoctrineUnlockedUpgrades, options);
        resolver.GetFormatterWithVerify<List<UpgradeSystem.UpgradeCoolDown>>().Serialize(ref writer, value.UpgradeCoolDowns, options);
        resolver.GetFormatterWithVerify<List<FollowerTrait.TraitType>>().Serialize(ref writer, value.CultTraits, options);
        resolver.GetFormatterWithVerify<List<string>>().Serialize(ref writer, value.WeaponUnlockedUpgrades, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CultName, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.MysticKeeperName, options);
        writer.Write(value.PlayerTriedToEnterMysticDimensionCount);
        writer.Write(value.MysticRewardCount);
        writer.Write(value.DungeonBossFight);
        resolver.GetFormatterWithVerify<List<DataManager.LocationSeedsData>>().Serialize(ref writer, value.LocationSeeds, options);
        resolver.GetFormatterWithVerify<WeatherSystemController.WeatherType>().Serialize(ref writer, value.WeatherType, options);
        resolver.GetFormatterWithVerify<WeatherSystemController.WeatherStrength>().Serialize(ref writer, value.WeatherStrength, options);
        writer.Write(value.WeatherStartingTime);
        writer.Write(value.WeatherDuration);
        writer.Write(value.TempleStudyXP);
        writer.Write(value.UnlockededASermon);
        writer.Write(value.CurrentDayIndex);
        writer.Write(value.CurrentPhaseIndex);
        writer.Write(value.CurrentGameTime);
        resolver.GetFormatterWithVerify<int[]>().Serialize(ref writer, value.LastUsedSermonRitualDayIndex, options);
        resolver.GetFormatterWithVerify<int[]>().Serialize(ref writer, value.ScheduledActivityIndexes, options);
        writer.Write(value.OverrideScheduledActivity);
        resolver.GetFormatterWithVerify<int[]>().Serialize(ref writer, value.InstantActivityIndexes, options);
        writer.Write(value.PlayerEaten);
        writer.Write(value.PlayerEaten_COOP);
        resolver.GetFormatterWithVerify<ResurrectionType>().Serialize(ref writer, value.ResurrectionType, options);
        writer.Write(value.FirstTimeResurrecting);
        writer.Write(value.FirstTimeFertilizing);
        writer.Write(value.FirstTimeChop);
        writer.Write(value.FirstTimeMine);
        writer.Write(value.PlayersShagged);
        writer.WriteNil();
        writer.WriteNil();
        resolver.GetFormatterWithVerify<List<DataManager.DungeonCompletedFleeces>>().Serialize(ref writer, value.DungeonsCompletedWithFleeces, options);
        resolver.GetFormatterWithVerify<StructureBrain.Categories>().Serialize(ref writer, value.currentCategory, options);
        writer.Write(value.TimeSinceLastComplaint);
        writer.Write(value.TimeSinceLastQuest);
        writer.Write(value.DessentingFollowerChoiceQuestionIndex);
        writer.Write(value.HaroConversationIndex);
        writer.Write(value.SpecialHaroConversationIndex);
        resolver.GetFormatterWithVerify<List<FollowerLocation>>().Serialize(ref writer, value.HaroSpecialEncounteredLocations, options);
        resolver.GetFormatterWithVerify<List<FollowerLocation>>().Serialize(ref writer, value.LeaderSpecialEncounteredLocations, options);
        writer.Write(value.SpokenToHaroD6);
        writer.Write(value.HaroOnboardedWinter);
        writer.Write(value.HaroConversationCompleted);
        writer.Write(value.RatauKilled);
        writer.Write(value.RatauReadLetter);
        writer.Write(value.RatauIntroWoolhaven);
        writer.Write(value.RatauStaffQuestGameConvoPlayed);
        writer.Write(value.RatauStaffQuestWonGame);
        writer.Write(value.RatauStaffQuestAliveDead);
        resolver.GetFormatterWithVerify<FollowerLocation>().Serialize(ref writer, value.CurrentFoxLocation, options);
        writer.Write(value.CurrentFoxEncounter);
        writer.Write(value.CurrentDLCFoxEncounter);
        resolver.GetFormatterWithVerify<List<FollowerLocation>>().Serialize(ref writer, value.FoxIntroductions, options);
        resolver.GetFormatterWithVerify<List<FollowerLocation>>().Serialize(ref writer, value.FoxCompleted, options);
        writer.Write(value.PlimboStoryProgress);
        writer.Write(value.RatooFishingProgress);
        writer.Write(value.RatooFishing_FISH_CRAB);
        writer.Write(value.RatooFishing_FISH_LOBSTER);
        writer.Write(value.RatooFishing_FISH_OCTOPUS);
        writer.Write(value.RatooFishing_FISH_SQUID);
        writer.Write(value.RatooNeedsRescue);
        writer.Write(value.RatooRescued);
        writer.Write(value.PlayerHasFollowers);
        writer.Write(value.ShowSpecialHaroRoom);
        writer.Write(value.ShowSpecialMidasRoom);
        writer.Write(value.ShowSpecialPlimboRoom);
        writer.Write(value.ShowSpecialKlunkoRoom);
        writer.Write(value.ShowSpecialLeaderRoom);
        writer.Write(value.ShowSpecialFishermanRoom);
        writer.Write(value.ShowSpecialSozoRoom);
        writer.Write(value.ShowSpecialBaalAndAymRoom);
        writer.Write(value.ShowSpecialLighthouseKeeperRoom);
        writer.Write(value.SozoUnlockedMushroomSkin);
        writer.Write(value.SozoNoLongerBrainwashed);
        writer.Write(value.SozoMushroomRecruitedDay);
        writer.Write(value.SozoAteMushroomDay);
        writer.Write(value.SozoMushroomCount);
        writer.Write(value.DrunkDay);
        writer.Write(value.DrunkIncrement);
        writer.Write(value.PoemIncrement);
        writer.Write(value.FishCaughtTotal);
        writer.Write(value.PlayerDeathDay);
        writer.Write(value.DisciplesCreated);
        writer.Write(value.LastDrumCircleTime);
        writer.Write(value.HasMidasHiding);
        resolver.GetFormatterWithVerify<FollowerInfo>().Serialize(ref writer, value.MidasFollowerInfo, options);
        writer.Write(value.TimeSinceMidasStoleGold);
        writer.Write(value.MidasHiddenDay);
        writer.Write(value.CompletedMidasFollowerQuest);
        writer.Write(value.GivenMidasFollowerQuest);
        resolver.GetFormatterWithVerify<List<InventoryItem>>().Serialize(ref writer, value.MidasStolenGold, options);
        writer.Write(value.LastIceSculptureBuild);
        writer.Write(value.LastAnimalLoverPet);
        writer.Write(value.RatooGivenHeart);
        writer.Write(value.RatooMentionedWrongHeart);
        writer.Write(value.ShownInitialTempleDoorSeal);
        writer.Write(value.FirstFollowerSpawnInteraction);
        resolver.GetFormatterWithVerify<List<int>>().Serialize(ref writer, value.DecorationTypesBuilt, options);
        resolver.GetFormatterWithVerify<List<FollowerClothingType>>().Serialize(ref writer, value.UnlockedClothing, options);
        resolver.GetFormatterWithVerify<List<FollowerClothingType>>().Serialize(ref writer, value.ClothingAssigned, options);
        resolver.GetFormatterWithVerify<FollowerClothingType>().Serialize(ref writer, value.previouslyAssignedClothing, options);
        resolver.GetFormatterWithVerify<List<DataManager.ClothingVariant>>().Serialize(ref writer, value.ClothingVariants, options);
        resolver.GetFormatterWithVerify<List<TarotCards.Card>>().Serialize(ref writer, value.WeaponSelectionPositions, options);
        writer.Write(value.LoreStonesRoomUpTo);
        writer.Write(value.LoreStonesHaro);
        writer.Write(value.LoreStonesMoth);
        writer.Write(value.ShowCultFaith);
        writer.Write(value.ShowCultIllness);
        writer.Write(value.ShowCultHunger);
        writer.Write(value.ShowCultWarmth);
        writer.Write(value.ShowLoyaltyBars);
        writer.Write(value.SandboxModeEnabled);
        writer.Write(value.SpawnPubResources);
        writer.Write(value.EnteredHopRoom);
        writer.Write(value.EnteredGrapeRoom);
        writer.Write(value.EnteredCottonRoom);
        writer.Write(value.IntroDoor1);
        writer.Write(value.FirstDoctrineStone);
        writer.Write(value.InitialDoctrineStone);
        writer.Write(value.ShowHaroDoctrineStoneRoom);
        writer.Write(value.HaroIntroduceDoctrines);
        writer.Write(value.RatExplainDungeon);
        writer.Write(value.RatauToGiveCurseNextRun);
        writer.Write(value.SozoStoryProgress);
        writer.Write(value.MidasBankUnlocked);
        writer.Write(value.MidasBankIntro);
        writer.Write(value.MidasSacrificeIntro);
        writer.Write(value.MidasIntro);
        writer.Write(value.MidasDevotionIntro);
        writer.Write(value.MidasStatue);
        writer.Write(value.MidasDevotionCost);
        writer.Write(value.MidasDevotionLastUsed);
        writer.Write(value.MidasFollowerStatueCount);
        writer.Write(value.RatauShowShrineShop);
        writer.Write(value.DecorationRoomFirstConvo);
        writer.Write(value.FirstTarot);
        writer.Write(value.Tutorial_Night);
        writer.Write(value.Tutorial_ReturnToDungeon);
        writer.Write(value.FirstTimeInDungeon);
        writer.Write(value.AllowBuilding);
        writer.Write(value.CookedFirstFood);
        writer.Write(value.Dungeon1Story1);
        writer.Write(value.Dungeon1Story2);
        writer.Write(value.FirstFollowerRescue);
        writer.Write(value.FirstDungeon1RescueRoom);
        writer.Write(value.FirstDungeon2RescueRoom);
        writer.Write(value.FirstDungeon3RescueRoom);
        writer.Write(value.FirstDungeon4RescueRoom);
        writer.Write(value.SherpaFirstConvo);
        writer.Write(value.ResourceRoom1Revealed);
        writer.Write(value.EncounteredHealingRoom);
        writer.Write(value.MinimumRandomRoomsEncountered);
        writer.Write(value.MinimumRandomRoomsEncounteredAmount);
        writer.Write(value.ForneusLore);
        writer.Write(value.SozoBeforeDeath);
        writer.Write(value.SozoDead);
        writer.Write(value.SozoTakenMushroom);
        writer.Write(value.FirstTimeWeaponMarketplace);
        writer.Write(value.FirstTimeSpiderMarketplace);
        writer.Write(value.FirstTimeSeedMarketplace);
        writer.Write(value.ShowFirstDoctrineStone);
        writer.Write(value.RatauGiftMediumCollected);
        writer.Write(value.CompletedLighthouseCrystalQuest);
        writer.Write(value.CameFromDeathCatFight);
        writer.Write(value.OldFollowerSpoken);
        writer.Write(value.InjuredFollowerSpoken);
        writer.Write(value.CanUnlockRelics);
        writer.Write(value.FoundRelicAtHubShore);
        writer.Write(value.FoundRelicInFish);
        writer.Write(value.GivenRelicFishRiddle);
        writer.Write(value.GivenRelicLighthouseRiddle);
        writer.Write(value.ForceMarketplaceCat);
        writer.Write(value.HadInitialMatingTentInteraction);
        writer.Write(value.ShowMidasKilling);
        writer.Write(value.GivenMidasSkull);
        writer.Write(value.CompletedYngyaFightIntro);
        writer.Write(value.RecruitedRotFollower);
        writer.Write(value.HasAcceptedPilgrimPart1);
        writer.Write(value.PilgrimPart1TargetDay);
        writer.Write(value.HasAcceptedPilgrimPart2);
        writer.Write(value.PilgrimPart2TargetDay);
        writer.Write(value.HasAcceptedPilgrimPart3);
        writer.Write(value.PilgrimPart3TargetDay);
        writer.Write(value.IsPilgrimRescue);
        writer.Write(value.IsJalalaBag);
        writer.Write(value.FoundJalalaBag);
        writer.Write(value.GivenRinorLine);
        writer.Write(value.GivenYarlenLine);
        resolver.GetFormatterWithVerify<FollowerLocation>().Serialize(ref writer, value.PilgrimTargetLocation, options);
        writer.Write(value.CultLeader1_LastRun);
        writer.Write(value.CultLeader1_StoryPosition);
        writer.Write(value.CultLeader2_LastRun);
        writer.Write(value.CultLeader2_StoryPosition);
        writer.Write(value.CultLeader3_LastRun);
        writer.Write(value.CultLeader3_StoryPosition);
        writer.Write(value.CultLeader4_LastRun);
        writer.Write(value.CultLeader4_StoryPosition);
        writer.Write(value.CultLeader5_LastRun);
        writer.Write(value.CultLeader5_StoryPosition);
        writer.Write(value.CultLeader6_LastRun);
        writer.Write(value.CultLeader6_StoryPosition);
        writer.Write(value.BeatenDungeon5);
        writer.Write(value.BeatenDungeon6);
        writer.Write(value.DeathCatConversationLastRun);
        writer.Write(value.DeathCatStory);
        writer.Write(value.DeathCatDead);
        writer.Write(value.DeathCatWon);
        writer.Write(value.DeathCatBoss1);
        writer.Write(value.DeathCatBoss2);
        writer.Write(value.DeathCatBoss3);
        writer.Write(value.DeathCatBoss4);
        writer.Write(value.DeathCatRatauKilled);
        writer.Write(value.DungeonKeyRoomCompleted1);
        writer.Write(value.DungeonKeyRoomCompleted2);
        writer.Write(value.DungeonKeyRoomCompleted3);
        writer.Write(value.DungeonKeyRoomCompleted4);
        writer.Write(value.LambTownLevel);
        writer.Write(value.LambTownWoolGiven);
        writer.Write(value.RatOutpostIntro);
        writer.Write(value.FirstMonsterHeart);
        writer.Write(value.Rat_Tutorial_Bell);
        writer.Write(value.Goat_First_Meeting);
        writer.Write(value.Goat_Guardian_Door_Open);
        writer.Write(value.Key_Shrine_1);
        writer.Write(value.Key_Shrine_2);
        writer.Write(value.Key_Shrine_3);
        writer.Write(value.InTutorial);
        writer.Write(value.UnlockBaseTeleporter);
        writer.Write(value.Tutorial_First_Indoctoring);
        writer.Write(value.Tutorial_Second_Enter_Base);
        writer.Write(value.Tutorial_Rooms_Completed);
        writer.Write(value.Tutorial_Enable_Store_Resources);
        writer.Write(value.Tutorial_Completed);
        writer.Write(value.Tutorial_Mission_Board);
        writer.Write(value.Create_Tutorial_Rooms);
        writer.Write(value.RatauExplainsFollowers);
        writer.Write(value.RatauExplainsDemo);
        writer.Write(value.RatauExplainsBiome0);
        writer.Write(value.RatauExplainsBiome1);
        writer.Write(value.RatauExplainsBiome0Boss);
        writer.Write(value.RatauExplainsTeleporter);
        writer.Write(value.SozoIntro);
        writer.Write(value.SozoDecorationQuestActive);
        writer.Write(value.SozoQuestComplete);
        writer.Write(value.CollectedMenticide);
        writer.Write(value.TarotIntro);
        writer.Write(value.HasTarotBuilding);
        writer.Write(value.ForestOfferingRoomCompleted);
        writer.Write(value.KnucklebonesIntroCompleted);
        writer.Write(value.KnucklebonesFirstGameRatauStart);
        writer.Write(value.ForestChallengeRoom1Completed);
        writer.Write(value.ForestRescueWorshipper);
        writer.Write(value.GetFirstFollower);
        writer.Write(value.BeatenFirstMiniBoss);
        writer.Write(value.RatauExplainBuilding);
        writer.Write(value.PromoteFollowerExplained);
        writer.Write(value.HasMadeFirstOffering);
        writer.Write(value.BirdConvo);
        writer.Write(value.UnlockedHubShore);
        writer.Write(value.GivenFollowerGift);
        writer.Write(value.FinalBossSlowWalk);
        writer.Write(value.HadNecklaceOnRun);
        writer.Write(value.HasPerformedPleasureRitual);
        writer.Write(value.ViolentExtremistFirstTime);
        resolver.GetFormatterWithVerify<List<int>>().Serialize(ref writer, value.FollowersPlayedKnucklebonesToday, options);
        writer.Write(value.ShownDungeon1FinalLeaderEncounter);
        writer.Write(value.ShownDungeon2FinalLeaderEncounter);
        writer.Write(value.ShownDungeon3FinalLeaderEncounter);
        writer.Write(value.ShownDungeon4FinalLeaderEncounter);
        writer.Write(value.HaroOnbardedHarderDungeon1);
        writer.Write(value.HaroOnbardedHarderDungeon2);
        writer.Write(value.HaroOnbardedHarderDungeon3);
        writer.Write(value.HaroOnbardedHarderDungeon4);
        writer.Write(value.HaroOnbardedDungeon6);
        writer.Write(value.HaroOnbardedHarderDungeon1_PostGame);
        writer.Write(value.HaroOnbardedHarderDungeon2_PostGame);
        writer.Write(value.HaroOnbardedHarderDungeon3_PostGame);
        writer.Write(value.HaroOnbardedHarderDungeon4_PostGame);
        writer.Write(value.RevealOfferingChest);
        writer.Write(value.OnboardedOfferingChest);
        writer.Write(value.OnboardedHomeless);
        writer.Write(value.OnboardedHomelessAtNight);
        writer.Write(value.OnboardedEndlessMode);
        writer.Write(value.OnboardedDeadFollower);
        writer.Write(value.OnboardedBuildingHouse);
        writer.Write(value.OnboardedMakingMoreFood);
        writer.Write(value.OnboardedCleaningBase);
        writer.Write(value.OnboardedOldFollower);
        writer.Write(value.OnboardedSickFollower);
        writer.Write(value.OnboardedStarvingFollower);
        writer.Write(value.OnboardedDissenter);
        writer.Write(value.OnboardedFaithOfFlock);
        writer.Write(value.OnboardedRaiseFaith);
        writer.Write(value.OnboardedResourceYard);
        writer.Write(value.OnboardedCrisisOfFaith);
        writer.Write(value.OnboardedHalloween);
        writer.Write(value.OnboardedSermon);
        writer.Write(value.OnboardedBuildFarm);
        writer.Write(value.OnboardedRefinery);
        writer.Write(value.OnboardedCultName);
        writer.Write(value.OnboardedZombie);
        writer.Write(value.OnboardedLoyalty);
        writer.Write(value.OnboardedGodTear);
        writer.Write(value.OnboardedMysticShop);
        writer.Write(value.ForeshadowedMysticShop);
        writer.Write(value.ForeshadowedWolf);
        writer.Write(value.OnboardedLayer2);
        writer.Write(value.OnboardedRelics);
        writer.Write(value.HasMetChefShop);
        writer.Write(value.CurrentOnboardingFollowerID);
        writer.Write(value.CurrentOnboardingFollowerType);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.CurrentOnboardingFollowerTerm, options);
        writer.Write(value.HasPerformedRitual);
        writer.Write(value.DeathCatBaalAndAymSecret);
        writer.Write(value.ShamuraBaalAndAymSecret);
        writer.Write(value.CanFindLeaderRelic);
        writer.Write(value.OnboardedDisciple);
        writer.Write(value.OnboardedPleasure);
        writer.Write(value.OnboardedFollowerPleasure);
        resolver.GetFormatterWithVerify<List<FollowerLocation>>().Serialize(ref writer, value.SecretItemsGivenToFollower, options);
        writer.Write(value.OnboardedDepositFollowerNPC);
        writer.Write(value.DepositFinalFollowerNPC);
        writer.Write(value.TalkedToInfectedNPC);
        writer.Write(value.CompletedInfectedNPCQuest);
        resolver.GetFormatterWithVerify<List<FollowerTrait.TraitType>>().Serialize(ref writer, value.DepositFollowerTargetTraits, options);
        writer.Write(value.DepositedFollowerRewardsClaimed);
        writer.Write(value.DepositedWitnessEyesForRelics);
        writer.Write(value.GaveLeshyHealingQuest);
        writer.Write(value.GaveHeketHealingQuest);
        writer.Write(value.GaveKallamarHealingQuest);
        writer.Write(value.GaveShamuraHealingQuest);
        writer.Write(value.LeshyHealQuestCompleted);
        writer.Write(value.HeketHealQuestCompleted);
        writer.Write(value.KallamarHealQuestCompleted);
        writer.Write(value.ShamuraHealQuestCompleted);
        writer.Write(value.LeshyHealed);
        writer.Write(value.HeketHealed);
        writer.Write(value.KallamarHealed);
        writer.Write(value.ShamuraHealed);
        writer.Write(value.HealingLeshyQuestActive);
        writer.Write(value.HealingHeketQuestActive);
        writer.Write(value.HealingKallamarQuestActive);
        writer.Write(value.HealingShamuraQuestActive);
        writer.Write(value.HealingQuestDay);
        writer.Write(value.LeshyHealingQuestDay);
        writer.Write(value.HeketHealingQuestDay);
        writer.Write(value.KallamarHealingQuestDay);
        writer.Write(value.ShamuraHealingQuestDay);
        writer.Write(value.BeatenWitnessDungeon1);
        writer.Write(value.BeatenWitnessDungeon2);
        writer.Write(value.BeatenWitnessDungeon3);
        writer.Write(value.BeatenWitnessDungeon4);
        writer.Write(value.MysticKeeperBeatenLeshy);
        writer.Write(value.MysticKeeperBeatenHeket);
        writer.Write(value.MysticKeeperBeatenKallamar);
        writer.Write(value.MysticKeeperBeatenShamura);
        writer.Write(value.MysticKeeperBeatenAll);
        writer.Write(value.MysticKeeperFirstPurchase);
        writer.Write(value.MysticKeeperOnboardedSin);
        writer.Write(value.ChemachOnboardedSin);
        writer.Write(value.KlunkoOnboardedTailor1);
        writer.Write(value.KlunkoOnboardedTailor2);
        writer.Write(value.AssignedFollowersOutfits);
        writer.Write(value.BeatenPostGame);
        writer.Write(value.GivenLoyaltyQuestDay);
        writer.Write(value.LastDaySincePlayerUpgrade);
        writer.Write(value.MealsCooked);
        writer.Write(value.DrinksBrewed);
        writer.Write(value.TalismanPiecesReceivedFromMysticShop);
        writer.Write(value.MysticShopUsed);
        writer.Write(value.CrystalDoctrinesReceivedFromMysticShop);
        resolver.GetFormatterWithVerify<InventoryItem.ITEM_TYPE>().Serialize(ref writer, value.PreviousMysticShopItem, options);
        writer.Write(value.OnboardedCrystalDoctrine);
        writer.Write(value.RanchingAnimalsAdded);
        writer.Write(value.FollowersTrappedInToxicWaste);
        writer.Write(value.OnboardedWool);
        writer.Write(value.Dungeon1_1_Key);
        writer.Write(value.Dungeon1_2_Key);
        writer.Write(value.Dungeon1_3_Key);
        writer.Write(value.Dungeon1_4_Key);
        writer.Write(value.Dungeon2_1_Key);
        writer.Write(value.Dungeon2_2_Key);
        writer.Write(value.Dungeon2_3_Key);
        writer.Write(value.Dungeon2_4_Key);
        writer.Write(value.Dungeon3_1_Key);
        writer.Write(value.Dungeon3_2_Key);
        writer.Write(value.Dungeon3_3_Key);
        writer.Write(value.Dungeon3_4_Key);
        writer.Write(value.HadFirstTempleKey);
        writer.Write(value.CurrentKeyPieces);
        writer.Write(value.GivenFreeDungeonFollower);
        writer.Write(value.GivenFreeDungeonGold);
        writer.Write(value.FoxMeeting_0);
        writer.Write(value.GaveFollowerToFox);
        writer.Write(value.Ritual_0);
        writer.Write(value.Ritual_1);
        writer.Write(value.SnowmenCreated);
        writer.Write(value.Lighthouse_FirstConvo);
        writer.Write(value.Lighthouse_LitFirstConvo);
        writer.Write(value.Lighthouse_FireOutAgain);
        writer.Write(value.Lighthouse_QuestGiven);
        writer.Write(value.Lighthouse_QuestComplete);
        writer.Write(value.LighthouseFuel);
        writer.Write(value.Lighthouse_Lit);
        writer.Write(value.ShoreFishFirstConvo);
        writer.Write(value.ShoreFishFished);
        writer.Write(value.ShoreTarotShotConvo1);
        writer.Write(value.ShoreTarotShotConvo2);
        writer.Write(value.ShoreFlowerShopConvo1);
        writer.Write(value.SozoFlowerShopConvo1);
        writer.Write(value.SozoTarotShopConvo1);
        resolver.GetFormatterWithVerify<EquipmentType>().Serialize(ref writer, value.ForcingPlayerWeaponDLC, options);
        writer.Write(value.RatauFoundSkin);
        writer.Write(value.MidasFoundSkin);
        writer.Write(value.SozoFoundDecoration);
        writer.Write(value.MidasTotalGoldStolen);
        writer.Write(value.MidasSpecialEncounter);
        resolver.GetFormatterWithVerify<List<FollowerLocation>>().Serialize(ref writer, value.MidasSpecialEncounteredLocations, options);
        writer.Write(value.MidasBeaten);
        writer.Write(value.PlimboSpecialEncountered);
        writer.Write(value.KlunkoSpecialEncountered);
        writer.Write(value.FishermanSpecialEncountered);
        writer.Write(value.BaalAndAymSpecialEncounterd);
        writer.Write(value.LighthouseKeeperSpecialEncountered);
        writer.Write(value.SozoSpecialEncountered);
        writer.Write(value.OpenedDoorTimestamp);
        writer.Write(value.SeedMarketPlacePostGame);
        writer.Write(value.HelobPostGame);
        writer.Write(value.HorseTown_PaidRespectToHorse);
        writer.Write(value.HorseTown_JoinCult);
        writer.Write(value.HorseTown_OpenedChest);
        writer.Write(value.BlackSoulsEnabled);
        writer.Write(value.PlacedRubble);
        writer.Write(value.DefeatedExecutioner);
        writer.Write(value.BeatenWolf);
        writer.Write(value.BeatenYngya);
        writer.Write(value.BeatenExecutioner);
        writer.Write(value.RevealedPostDLC);
        writer.Write(value.HadFinalYngyaRoomConvo);
        writer.Write(value.HasYngyaMatingQuestAccepted);
        writer.Write(value.HasYngyaFirePitRitualQuestAccepted);
        writer.Write(value.HasYngyaFlowerBasketQuestAccepted);
        writer.Write(value.HasFinishedYngyaFlowerBasketQuest);
        writer.Write(value.HasAnimalFeedPoopQuest0Accepted);
        writer.Write(value.HasAnimalFeedPoopQuest1Accepted);
        writer.Write(value.HasAnimalFeedPoopQuest2Accepted);
        writer.Write(value.HasWalkPoopedAnimalQuestAccepted);
        writer.Write(value.HasAnimalFeedMeatQuest0Accepted);
        writer.Write(value.HasAnimalFeedMeatQuest1Accepted);
        writer.Write(value.HasAnimalFeedMeatQuest2Accepted);
        writer.Write(value.HasBuildGoodSnowmanQuestAccepted);
        writer.Write(value.HasLifeToTheIceRitualQuestAccepted);
        writer.Write(value.ExecutionerFollowerNoteGiverID);
        writer.Write(value.GiveExecutionerFollower);
        writer.Write(value.GivenExecutionerFollower);
        writer.Write(value.ExecutionerRoomRequiresRevealing);
        writer.Write(value.ExecutionerRoomRevealed);
        writer.Write(value.ExecutionerRoomRevealedThisRun);
        writer.Write(value.ExecutionerRoomUnlocked);
        writer.Write(value.ExecutionerDefeated);
        writer.Write(value.ExecutionerPardoned);
        writer.Write(value.ExecutionerDamned);
        writer.Write(value.ExecutionerSpokenToPlimbo);
        writer.Write(value.ExecutionerReceivedPlimbosHelp);
        writer.Write(value.ExecutionerSpokenToMidas);
        writer.Write(value.ExecutionerReceivedMidasHelp);
        writer.Write(value.ExecutionerFindNoteInSilkCradle);
        writer.Write(value.ExecutionerPurchases);
        resolver.GetFormatterWithVerify<List<int>>().Serialize(ref writer, value.RuinedGraveyards, options);
        writer.Write(value.ExecutionerPardonedDay);
        writer.Write(value.ExecutionerInWoolhavenDay);
        writer.Write(value.ExecutionerWoolhavenExecuted);
        writer.Write(value.ExecutionerWoolhavenSaved);
        writer.Write(value.ExecutionerGivenWeaponFragment);
        writer.Write(value.WorldMapCurrentSelection);
        writer.Write(value.HasBaalSkin);
        writer.Write(value.HasReturnedBaal);
        writer.Write(value.HasAymSkin);
        writer.Write(value.HasReturnedAym);
        writer.Write(value.HasReturnedBoth);
        writer.Write(value.PlayedPostYngyaSequence);
        writer.Write(value.QuickStartActive);
        writer.Write(value.RemovedStoryMomentsActive);
        writer.Write(value.WinterModeActive);
        writer.Write(value.SurvivalModeActive);
        writer.Write(value.SurvivalModeFirstSpawn);
        writer.Write(value.SurvivalModeOnboarded);
        writer.Write(value.SurvivalSleepOnboarded);
        writer.Write(value.SurvivalHungerOnboarded);
        writer.Write(value.PlimboRejectedRotEye);
        writer.Write(value.SurvivalMode_Hunger);
        writer.Write(value.SurvivalMode_Sleep);
        writer.Write(value.RedHeartsTemporarilyRemoved);
        writer.Write(value.ShownKnuckleboneTutorial);
        writer.Write(value.Knucklebones_Opponent_Ratau_Won);
        writer.Write(value.FollowerKnucklebonesMatch);
        writer.Write(value.NextKnucklbonesLucky);
        writer.Write(value.FlockadeTutorialShown);
        writer.Write(value.FlockadeShepherdsTutorialShown);
        writer.Write(value.FlockadeFirstGameOpponentStarts);
        writer.Write(value.FlockadePlayed);
        writer.Write(value.HasNewFlockadePieces);
        writer.Write(value.AnimalID);
        writer.Write(value.ShopKeeperChefState);
        writer.Write(value.ShopKeeperChefEnragedDay);
        writer.Write(value.Knucklebones_Opponent_0);
        writer.Write(value.Knucklebones_Opponent_0_FirstConvoRataus);
        writer.Write(value.Knucklebones_Opponent_0_Won);
        writer.Write(value.Knucklebones_Opponent_1);
        writer.Write(value.Knucklebones_Opponent_1_FirstConvoRataus);
        writer.Write(value.Knucklebones_Opponent_1_Won);
        writer.Write(value.Knucklebones_Opponent_2);
        writer.Write(value.Knucklebones_Opponent_2_FirstConvoRataus);
        writer.Write(value.Knucklebones_Opponent_2_Won);
        writer.Write(value.RefinedElectrifiedRotstone);
        writer.Write(value.DungeonLayer1);
        writer.Write(value.DungeonLayer2);
        writer.Write(value.DungeonLayer3);
        writer.Write(value.DungeonLayer4);
        writer.Write(value.DungeonLayer5);
        writer.Write(value.BeatenDungeon1);
        writer.Write(value.BeatenDungeon2);
        writer.Write(value.BeatenDungeon3);
        writer.Write(value.BeatenDungeon4);
        writer.Write(value.BeatenDeathCat);
        writer.Write(value.BeatenLeshyLayer2);
        writer.Write(value.BeatenHeketLayer2);
        writer.Write(value.BeatenKallamarLayer2);
        writer.Write(value.BeatenShamuraLayer2);
        writer.Write(value.BeatenOneDungeons);
        writer.Write(value.BeatenTwoDungeons);
        writer.Write(value.BeatenThreeDungeons);
        writer.Write(value.BeatenFourDungeons);
        writer.Write(value.Dungeon1GodTears);
        writer.Write(value.Dungeon2GodTears);
        writer.Write(value.Dungeon3GodTears);
        writer.Write(value.Dungeon4GodTears);
        writer.Write(value.DungeonRunsSinceBeatingFirstDungeon);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.PreviousMiniBoss, options);
        writer.Write(value.FishCaughtInsideWhaleToday);
        resolver.GetFormatterWithVerify<List<DungeonSandboxManager.ProgressionSnapshot>>().Serialize(ref writer, value.SandboxProgression, options);
        writer.Write(value.OnboardedBossRush);
        writer.Write(value.CompletedSandbox);
        writer.Write(value.CanFindTarotCards);
        writer.Write(value.LuckMultiplier);
        writer.Write(value.NextMissionarySuccessful);
        writer.Write(value.EnemyModifiersChanceMultiplier);
        writer.Write(value.EnemyHealthMultiplier);
        writer.Write(value.ProjectileMoveSpeedMultiplier);
        writer.Write(value.DodgeDistanceMultiplier);
        writer.Write(value.CurseFeverMultiplier);
        writer.Write(value.SpawnPoisonOnAttack);
        writer.Write(value.EnemiesInNextRoomHaveModifiers);
        writer.Write(value.EnemiesDropGoldDuringRun);
        writer.Write(value.NoRollInNextCombatRoom);
        writer.Write(value.NoHeartDrops);
        writer.Write(value.EnemiesDropBombOnDeath);
        resolver.GetFormatterWithVerify<Vector2>().Serialize(ref writer, value.CurrentRoomCoordinates, options);
        writer.Write(value.SpecialAttackDamageMultiplier);
        writer.Write(value.NextChestGold);
        writer.Write(value.SpecialAttacksDisabled);
        writer.Write(value.BossHealthMultiplier);
        writer.Write(value.ResurrectRitualCount);
        writer.Write(value.NextRitualFree);
        writer.Write(value.EncounteredGambleRoom);
        resolver.GetFormatterWithVerify<List<int>>().Serialize(ref writer, value.LeaderFollowersRecruited, options);
        resolver.GetFormatterWithVerify<List<int>>().Serialize(ref writer, value.UniqueFollowersRecruited, options);
        writer.Write(value.SwordLevel);
        writer.Write(value.DaggerLevel);
        writer.Write(value.AxeLevel);
        writer.Write(value.HammerLevel);
        writer.Write(value.GauntletLevel);
        writer.Write(value.FireballLevel);
        writer.Write(value.EnemyBlastLevel);
        writer.Write(value.MegaSlashLevel);
        writer.Write(value.ProjectileAOELevel);
        writer.Write(value.TentaclesLevel);
        writer.Write(value.VortexLevel);
        writer.Write(value.LastFollowerQuestGivenTime);
        writer.Write(value.DLC_Pre_Purchase);
        writer.Write(value.DLC_Cultist_Pack);
        writer.Write(value.DLC_Heretic_Pack);
        writer.Write(value.DLC_Sinful_Pack);
        writer.Write(value.DLC_Pilgrim_Pack);
        writer.Write(value.MAJOR_DLC);
        writer.Write(value.DLC_Plush_Bonus);
        writer.Write(value.PAX_DLC);
        writer.Write(value.Twitch_Drop_1);
        writer.Write(value.Twitch_Drop_2);
        writer.Write(value.Twitch_Drop_3);
        writer.Write(value.Twitch_Drop_4);
        writer.Write(value.Twitch_Drop_5);
        writer.Write(value.Twitch_Drop_6);
        writer.Write(value.Twitch_Drop_7);
        writer.Write(value.Twitch_Drop_8);
        writer.Write(value.Twitch_Drop_9);
        writer.Write(value.Twitch_Drop_10);
        writer.Write(value.Twitch_Drop_11);
        writer.Write(value.Twitch_Drop_12);
        writer.Write(value.Twitch_Drop_13);
        writer.Write(value.Twitch_Drop_14);
        writer.Write(value.Twitch_Drop_15);
        writer.Write(value.SupportStreamer);
        writer.Write(value.LandConvoProgress);
        writer.Write(value.LandResourcesGiven);
        writer.Write(value.LandPurchased);
        writer.Write(value.HasWeatherVane);
        writer.Write(value.HasWeatherVaneUI);
        resolver.GetFormatterWithVerify<List<int>>().Serialize(ref writer, value.ShopsBuilt, options);
        writer.Write(value.InteractedDLCShrine);
        writer.Write(value.NPCRescueRoomsCompleted);
        writer.Write(value.RoomVariant);
        writer.Write(value.TimeSinceLastWolf);
        resolver.GetFormatterWithVerify<List<StructuresData.Ranchable_Animal>>().Serialize(ref writer, value.BreakingOutAnimals, options);
        resolver.GetFormatterWithVerify<List<InventoryItem.ITEM_TYPE>>().Serialize(ref writer, value.DisoveredAnimals, options);
        resolver.GetFormatterWithVerify<StructuresData.Ranchable_Animal[]>().Serialize(ref writer, value.FollowingPlayerAnimals, options);
        resolver.GetFormatterWithVerify<List<DataManager.Offering>>().Serialize(ref writer, value.BlizzardOfferingRequirements, options);
        resolver.GetFormatterWithVerify<List<DataManager.Offering>>().Serialize(ref writer, value.BlizzardOfferingsGiven, options);
        resolver.GetFormatterWithVerify<List<InventoryItem>>().Serialize(ref writer, value.SacrificeTableInventory, options);
        writer.Write(value.BlizzardMonsterActive);
        writer.Write(value.BlizzardSnowmenGiven);
        writer.Write(value.CompletedOfferingThisBlizzard);
        writer.Write(value.CompletedBlizzardSecret);
        writer.Write(value.BlizzardOfferingsCompleted);
        writer.Write(value.ForceDammedRelic);
        writer.Write(value.ForceBlessedRelic);
        writer.Write(value.FirstRelic);
        writer.Write(value.EndlessModeOnCooldown);
        writer.Write(value.EndlessModeSinOncooldown);
        writer.Write(value.TimeSinceLastStolenFromFollowers);
        writer.Write(value.TimeSinceLastFollowerFight);
        writer.Write(value.TimeSinceLastFollowerEaten);
        writer.Write(value.TimeSinceLastFollowerBump);
        writer.Write(value.TimeSinceLastMissionaryFollowerEncounter);
        writer.Write(value.DaySinceLastSpecialPoop);
        writer.Write(value.followerRecruitWaiting);
        writer.Write(value.weddingsPerformed);
        writer.Write(value.ForceClothesShop);
        writer.Write(value.UnlockedTailor);
        writer.Write(value.RevealedTailor);
        writer.Write(value.TookBopToTailor);
        writer.Write(value.LeftBopAtTailor);
        writer.Write(value.BerithTalkedWithBop);
        writer.Write(value.itemsCleaned);
        writer.Write(value.outfitsCreated);
        writer.Write(value.drinksCreated);
        writer.Write(value.eggsCracked);
        writer.Write(value.eggsHatched);
        writer.Write(value.EggsProduced);
        writer.Write(value.HasProducedChosenOne);
        writer.Write(value.HasGivenPedigreeFollower);
        writer.Write(value.pleasurePointsRedeemed);
        resolver.GetFormatterWithVerify<List<int>>().Serialize(ref writer, value.PreviousSinPointFollowers, options);
        writer.Write(value.pleasurePointsRedeemedFollowerSpoken);
        writer.Write(value.pleasurePointsRedeemedTempleFollowerSpoken);
        writer.Write(value.damnedConversation);
        writer.Write(value.damnedFightConversation);
        writer.Write(value.ForceGoldenEgg);
        writer.Write(value.ForceSpecialPoo);
        writer.Write(value.ForceAbomination);
        writer.Write(value.clickedDLCAd);
        writer.Write(value.RevealedDLCMapDoor);
        writer.Write(value.RevealedDLCMapHeart);
        writer.Write(value.YngyaHeartRoomEncounters);
        writer.Write(value.DeathCatBeaten);
        writer.Write(value.HasEncounteredTarot);
        resolver.GetFormatterWithVerify<List<InventoryItem.ITEM_TYPE>>().Serialize(ref writer, value.RecentRecipes, options);
        resolver.GetFormatterWithVerify<List<InventoryItem.ITEM_TYPE>>().Serialize(ref writer, value.RecipesDiscovered, options);
        resolver.GetFormatterWithVerify<List<int>>().Serialize(ref writer, value.LoreUnlocked, options);
        writer.Write(value.LoreStonesOnboarded);
        writer.Write(value.LoreOnboarded);
        writer.Write(value.UpgradeTreeMenuDLCAlert);
        resolver.GetFormatterWithVerify<List<DataManager.DepositedFollower>>().Serialize(ref writer, value.DepositedFollowers, options);
        writer.Write(value.playerDamageDealt);
        writer.Write(value.PlayerScaleModifier);
        writer.Write(value.ChefShopDoublePrices);
        writer.Write(value.FollowerShopUses);
        resolver.GetFormatterWithVerify<List<DataManager.WoolhavenFlowerPot>>().Serialize(ref writer, value.WoolhavenFlowerPots, options);
        resolver.GetFormatterWithVerify<List<int>>().Serialize(ref writer, value.FullWoolhavenFlowerPots, options);
        writer.Write(value.sacrificesCompleted);
        resolver.GetFormatterWithVerify<List<InventoryItem.ITEM_TYPE>>().Serialize(ref writer, value.FoundItems, options);
        writer.Write(value.TakenBossDamage);
        writer.Write(value.PoopMealsCreated);
        writer.Write(value.PrayedAtCrownShrine);
        writer.Write(value.ShellsGifted_0);
        writer.Write(value.ShellsGifted_1);
        writer.Write(value.ShellsGifted_2);
        writer.Write(value.ShellsGifted_3);
        writer.Write(value.ShellsGifted_4);
        writer.Write(value.LostSoulsBark);
        writer.Write(value.DateLastScreenshot);
        writer.Write(value.PlayerDamageDealtThisRun);
        writer.Write(value.PlayerDamageReceivedThisRun);
        writer.Write(value.playerDamageReceived);
        writer.Write(value.Options_ScreenShake);
        writer.Write(value.PlayerIsASpirit);
        writer.Write(value.BridgeFixed);
        writer.Write(value.BuildingTome);
        writer.Write(value.BeenToDungeon);
        writer.Write(value.FollowerID);
        writer.Write(value.ObjectiveGroupID);
        resolver.GetFormatterWithVerify<List<FollowerInfo>>().Serialize(ref writer, value.Followers, options);
        resolver.GetFormatterWithVerify<List<FollowerInfo>>().Serialize(ref writer, value.Followers_Recruit, options);
        resolver.GetFormatterWithVerify<List<FollowerInfo>>().Serialize(ref writer, value.Followers_Dead, options);
        resolver.GetFormatterWithVerify<List<int>>().Serialize(ref writer, value.Followers_Dead_IDs, options);
        resolver.GetFormatterWithVerify<List<FollowerInfo>>().Serialize(ref writer, value.Followers_Possessed, options);
        resolver.GetFormatterWithVerify<List<FollowerInfo>>().Serialize(ref writer, value.Followers_Dissented, options);
        writer.Write(value.EncounteredPossessedEnemyRun);
        writer.Write(value.StructureID);
        resolver.GetFormatterWithVerify<List<StructuresData>>().Serialize(ref writer, value.BaseStructures, options);
        resolver.GetFormatterWithVerify<List<StructuresData>>().Serialize(ref writer, value.MajorDLCCachedBaseStructures, options);
        resolver.GetFormatterWithVerify<List<StructuresData>>().Serialize(ref writer, value.HubStructures, options);
        resolver.GetFormatterWithVerify<List<StructuresData>>().Serialize(ref writer, value.HubShoreStructures, options);
        resolver.GetFormatterWithVerify<List<StructuresData>>().Serialize(ref writer, value.Hub1_MainStructures, options);
        resolver.GetFormatterWithVerify<List<StructuresData>>().Serialize(ref writer, value.Hub1_BerriesStructures, options);
        resolver.GetFormatterWithVerify<List<StructuresData>>().Serialize(ref writer, value.Hub1_ForestStructures, options);
        resolver.GetFormatterWithVerify<List<StructuresData>>().Serialize(ref writer, value.Hub1_RatauInsideStructures, options);
        resolver.GetFormatterWithVerify<List<StructuresData>>().Serialize(ref writer, value.Hub1_RatauOutsideStructures, options);
        resolver.GetFormatterWithVerify<List<StructuresData>>().Serialize(ref writer, value.Hub1_SozoStructures, options);
        resolver.GetFormatterWithVerify<List<StructuresData>>().Serialize(ref writer, value.Hub1_SwampStructures, options);
        resolver.GetFormatterWithVerify<List<StructuresData>>().Serialize(ref writer, value.WoolhavenStructures, options);
        resolver.GetFormatterWithVerify<List<StructuresData>>().Serialize(ref writer, value.Dungeon_Logs1Structures, options);
        resolver.GetFormatterWithVerify<List<StructuresData>>().Serialize(ref writer, value.Dungeon_Logs2Structures, options);
        resolver.GetFormatterWithVerify<List<StructuresData>>().Serialize(ref writer, value.Dungeon_Logs3Structures, options);
        resolver.GetFormatterWithVerify<List<StructuresData>>().Serialize(ref writer, value.Dungeon_Food1Structures, options);
        resolver.GetFormatterWithVerify<List<StructuresData>>().Serialize(ref writer, value.Dungeon_Food2Structures, options);
        resolver.GetFormatterWithVerify<List<StructuresData>>().Serialize(ref writer, value.Dungeon_Food3Structures, options);
        resolver.GetFormatterWithVerify<List<StructuresData>>().Serialize(ref writer, value.Dungeon_Stone1Structures, options);
        resolver.GetFormatterWithVerify<List<StructuresData>>().Serialize(ref writer, value.Dungeon_Stone2Structures, options);
        resolver.GetFormatterWithVerify<List<StructuresData>>().Serialize(ref writer, value.Dungeon_Stone3Structures, options);
        resolver.GetFormatterWithVerify<List<int>>().Serialize(ref writer, value.Followers_TraitManipulating_IDs, options);
        resolver.GetFormatterWithVerify<List<int>>().Serialize(ref writer, value.Followers_Imprisoned_IDs, options);
        resolver.GetFormatterWithVerify<List<int>>().Serialize(ref writer, value.Followers_Elderly_IDs, options);
        resolver.GetFormatterWithVerify<List<int>>().Serialize(ref writer, value.Followers_OnMissionary_IDs, options);
        resolver.GetFormatterWithVerify<List<int>>().Serialize(ref writer, value.Followers_LeftInTheDungeon_IDs, options);
        resolver.GetFormatterWithVerify<List<int>>().Serialize(ref writer, value.Followers_Transitioning_IDs, options);
        resolver.GetFormatterWithVerify<List<int>>().Serialize(ref writer, value.Followers_Demons_IDs, options);
        resolver.GetFormatterWithVerify<List<int>>().Serialize(ref writer, value.Followers_Demons_Types, options);
        writer.Write(value.MatingCompletedTimestamp);
        resolver.GetFormatterWithVerify<List<SeasonalEventType>>().Serialize(ref writer, value.ActiveSeasonalEvents, options);
        resolver.GetFormatterWithVerify<List<Vector2>>().Serialize(ref writer, value.CustomisedFleeceOptions, options);
        resolver.GetFormatterWithVerify<List<StructureBrain.TYPES>>().Serialize(ref writer, value.RevealedStructures, options);
        resolver.GetFormatterWithVerify<List<DayObject>>().Serialize(ref writer, value.DayList, options);
        resolver.GetFormatterWithVerify<DayObject>().Serialize(ref writer, value.CurrentDay, options);
        resolver.GetFormatterWithVerify<List<string>>().Serialize(ref writer, value.TrackedObjectiveGroupIDs, options);
        resolver.GetFormatterWithVerify<List<ObjectivesData>>().Serialize(ref writer, value.Objectives, options);
        resolver.GetFormatterWithVerify<List<ObjectivesData>>().Serialize(ref writer, value.CompletedObjectives, options);
        resolver.GetFormatterWithVerify<List<ObjectivesData>>().Serialize(ref writer, value.FailedObjectives, options);
        resolver.GetFormatterWithVerify<List<ObjectivesData>>().Serialize(ref writer, value.DungeonObjectives, options);
        resolver.GetFormatterWithVerify<List<StoryData>>().Serialize(ref writer, value.StoryObjectives, options);
        resolver.GetFormatterWithVerify<List<ObjectivesDataFinalized>>().Serialize(ref writer, value.CompletedObjectivesHistory, options);
        resolver.GetFormatterWithVerify<List<ObjectivesDataFinalized>>().Serialize(ref writer, value.FailedObjectivesHistory, options);
        resolver.GetFormatterWithVerify<List<DataManager.QuestHistoryData>>().Serialize(ref writer, value.CompletedQuestsHistorys, options);
        resolver.GetFormatterWithVerify<InventoryItem.ITEM_TYPE>().Serialize(ref writer, value.SimpleInventoryItem, options);
        resolver.GetFormatterWithVerify<List<InventoryItem>>().Serialize(ref writer, value.items, options);
        writer.Write(value.IngredientsCapacityLevel);
        writer.Write(value.FoodCapacityLevel);
        writer.Write(value.LogCapacityLevel);
        writer.Write(value.StoneCapacityLevel);
        resolver.GetFormatterWithVerify<List<string>>().Serialize(ref writer, value.FollowerSkinsUnlocked, options);
        resolver.GetFormatterWithVerify<List<StructureEffect>>().Serialize(ref writer, value.StructureEffects, options);
        resolver.GetFormatterWithVerify<List<string>>().Serialize(ref writer, value.KilledBosses, options);
        resolver.GetFormatterWithVerify<List<EquipmentType>>().Serialize(ref writer, value.WeaponPool, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.LegendarySwordCustomName, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.LegendaryAxeCustomName, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.LegendaryDaggerCustomName, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.LegendaryHammerCustomName, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.LegendaryGauntletCustomName, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.LegendaryBlunderbussCustomName, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.LegendaryChainCustomName, options);
        resolver.GetFormatterWithVerify<List<EquipmentType>>().Serialize(ref writer, value.LegendaryWeaponsJobBoardCompleted, options);
        writer.Write(value.CurrentRunWeaponLevel);
        resolver.GetFormatterWithVerify<EquipmentType>().Serialize(ref writer, value.ForcedStartingWeapon, options);
        writer.Write(value.CurrentRunCurseLevel);
        resolver.GetFormatterWithVerify<EquipmentType>().Serialize(ref writer, value.ForcedStartingCurse, options);
        resolver.GetFormatterWithVerify<List<RelicType>>().Serialize(ref writer, value.SpawnedRelicsThisRun, options);
        resolver.GetFormatterWithVerify<List<EquipmentType>>().Serialize(ref writer, value.CursePool, options);
        resolver.GetFormatterWithVerify<List<TarotCards.Card>>().Serialize(ref writer, value.PlayerFoundTrinkets, options);
        resolver.GetFormatterWithVerify<List<CrownAbilities>>().Serialize(ref writer, value.CrownAbilitiesUnlocked, options);
        resolver.GetFormatterWithVerify<List<RelicType>>().Serialize(ref writer, value.PlayerFoundRelics, options);
        resolver.GetFormatterWithVerify<List<BluePrint>>().Serialize(ref writer, value.PlayerBluePrints, options);
        resolver.GetFormatterWithVerify<List<FlockadePieceType>>().Serialize(ref writer, value.PlayerFoundPieces, options);
        resolver.GetFormatterWithVerify<List<InventoryItem.ITEM_TYPE>>().Serialize(ref writer, value.FishCaught, options);
        resolver.GetFormatterWithVerify<List<MissionManager.Mission>>().Serialize(ref writer, value.ActiveMissions, options);
        resolver.GetFormatterWithVerify<List<MissionManager.Mission>>().Serialize(ref writer, value.AvailableMissions, options);
        writer.Write(value.NewMissionDayTimestamp);
        writer.Write(value.LastGoldenMissionDay);
        writer.Write(value.MissionShrineUnlocked);
        resolver.GetFormatterWithVerify<List<BuyEntry>>().Serialize(ref writer, value.ItemsForSale, options);
        resolver.GetFormatterWithVerify<List<ShopLocationTracker>>().Serialize(ref writer, value.Shops, options);
        writer.Write(value.LastDayUsedFollowerShop);
        resolver.GetFormatterWithVerify<FollowerInfo>().Serialize(ref writer, value.FollowerForSale, options);
        resolver.GetFormatterWithVerify<MidasDonation>().Serialize(ref writer, value.midasDonation, options);
        writer.Write(value.LastDayUsedBank);
        resolver.GetFormatterWithVerify<JellyFishInvestment>().Serialize(ref writer, value.Investment, options);
        resolver.GetFormatterWithVerify<List<TraderTracker>>().Serialize(ref writer, value.Traders, options);
        writer.Write(value.LastDayUsedFlockadeHint);
        resolver.GetFormatterWithVerify<FlockadePieceType>().Serialize(ref writer, value.HintedPieceType, options);
        resolver.GetFormatterWithVerify<List<ShrineUsageInfo>>().Serialize(ref writer, value.ShrineTimerInfo, options);
        writer.Write(value.RedHeartShrineLevel);
        writer.Write(value.ShrineHeart);
        writer.Write(value.ShrineCurses);
        writer.Write(value.ShrineVoodo);
        writer.Write(value.ShrineAstrology);
        resolver.GetFormatterWithVerify<List<Lamb.UI.ItemSelector.Category>>().Serialize(ref writer, value.ItemSelectorCategories, options);
        resolver.GetFormatterWithVerify<List<InventoryItem>>().Serialize(ref writer, value.itemsDungeon, options);
        writer.Write(value.DUNGEON_TIME);
        writer.Write(value.PLAYER_RUN_DAMAGE_LEVEL);
        writer.Write(value.PLAYER_HEARTS_LEVEL);
        writer.Write(value.PLAYER_DAMAGE_LEVEL);
        writer.Write(value.PLAYER_HEALTH);
        writer.Write(value.PLAYER_TOTAL_HEALTH);
        writer.Write(value.PLAYER_BLUE_HEARTS);
        writer.Write(value.PLAYER_BLACK_HEARTS);
        writer.Write(value.PLAYER_FIRE_HEARTS);
        writer.Write(value.PLAYER_ICE_HEARTS);
        writer.Write(value.PLAYER_REMOVED_HEARTS);
        writer.Write(value.PLAYER_SPIRIT_HEARTS);
        writer.Write(value.PLAYER_SPIRIT_TOTAL_HEARTS);
        writer.Write(value.UnlockedCoopRelicsAndTarots);
        writer.Write(value.UnlockedCoopTarots);
        writer.Write(value.UnlockedCoopRelics);
        writer.Write(value.UnlockedCorruptedRelicsAndTarots);
        writer.Write(value.COOP_PLAYER_BLUE_HEARTS);
        writer.Write(value.COOP_PLAYER_BLACK_HEARTS);
        writer.Write(value.COOP_PLAYER_FIRE_HEARTS);
        writer.Write(value.COOP_PLAYER_ICE_HEARTS);
        writer.Write(value.COOP_PLAYER_REMOVED_HEARTS);
        writer.Write(value.PLAYER_SPECIAL_CHARGE);
        writer.Write(value.PLAYER_SPECIAL_AMMO);
        writer.Write(value.PLAYER_SPECIAL_CHARGE_TARGET);
        writer.Write(value.PLAYER_ARROW_AMMO);
        writer.Write(value.PLAYER_ARROW_TOTAL_AMMO);
        writer.Write(value.PLAYER_SPIRIT_AMMO);
        writer.Write(value.PLAYER_SPIRIT_TOTAL_AMMO);
        writer.Write(value.PLAYER_REDEAL_TOKEN);
        writer.Write(value.PLAYER_REDEAL_TOKEN_TOTAL);
        writer.Write(value.PLAYER_HEALTH_MODIFIED);
        writer.Write(value.COOP_PLAYER_SPIRIT_HEARTS);
        writer.Write(value.COOP_PLAYER_SPIRIT_TOTAL_HEARTS);
        writer.Write(value.PLAYER_STARTING_HEALTH_CACHED);
        writer.Write(value.Souls);
        writer.Write(value.BlackSouls);
        writer.Write(value.BlackSoulTarget);
        writer.Write(value.FollowerTokens);
        writer.Write(value.SpyDay);
        writer.Write(value.SpyJoinedDay);
        writer.Write(value.ShrineGhostJuice);
        writer.Write(value.TotalShrineGhostJuice);
        writer.Write(value.YngyaMiscConvoIndex);
        writer.Write(value.ChoreXP);
        writer.Write(value.ChoreXP_Coop);
        writer.Write(value.ChoreXP_Coop_Temp_Gained);
        writer.Write(value.ChoreXPLevel);
        writer.Write(value.ChoreXPLevel_Coop);
        writer.Write(value.DiscipleXP);
        writer.Write(value.DiscipleLevel);
        writer.Write(value.DiscipleAbilityPoints);
        writer.Write(value.XP);
        writer.Write(value.Level);
        writer.Write(value.AbilityPoints);
        writer.Write(value.WeaponAbilityPoints);
        writer.Write(value.CurrentChallengeModeXP);
        writer.Write(value.CurrentChallengeModeLevel);
        writer.Write(value.Doctrine_Pleasure_XP);
        writer.Write(value.Doctrine_Winter_XP);
        writer.Write(value.Doctrine_PlayerUpgrade_XP);
        writer.Write(value.Doctrine_PlayerUpgrade_Level);
        writer.Write(value.Doctrine_Special_XP);
        writer.Write(value.Doctrine_Special_Level);
        writer.Write(value.Doctrine_WorkWorship_XP);
        writer.Write(value.Doctrine_WorkWorship_Level);
        writer.Write(value.Doctrine_Possessions_XP);
        writer.Write(value.Doctrine_Possessions_Level);
        writer.Write(value.Doctrine_Food_XP);
        writer.Write(value.Doctrine_Food_Level);
        writer.Write(value.Doctrine_Afterlife_XP);
        writer.Write(value.Doctrine_Afterlife_Level);
        writer.Write(value.Doctrine_LawAndOrder_XP);
        writer.Write(value.Doctrine_LawAndOrder_Level);
        writer.Write(value.Doctrine_Pleasure_Level);
        writer.Write(value.Doctrine_Winter_Level);
        writer.Write(value.CompletedDoctrineStones);
        writer.Write(value.DoctrineCurrentCount);
        writer.Write(value.DoctrineTargetCount);
        writer.Write(value.FRUIT_LOW_MEALS_COOKED);
        writer.Write(value.VEGETABLE_LOW_MEALS_COOKED);
        writer.Write(value.VEGETABLE_MEDIUM_MEALS_COOKED);
        writer.Write(value.VEGETABLE_HIGH_MEALS_COOKED);
        writer.Write(value.FISH_LOW_MEALS_COOKED);
        writer.Write(value.FISH_MEDIUM_MEALS_COOKED);
        writer.Write(value.FISH_HIGH_MEALS_COOKED);
        writer.Write(value.EGG_MEALS_COOKED);
        writer.Write(value.MEAT_LOW_COOKED);
        writer.Write(value.MEAT_MEDIUM_COOKED);
        writer.Write(value.MEAT_HIGH_COOKED);
        writer.Write(value.MIXED_LOW_COOKED);
        writer.Write(value.MIXED_MEDIUM_COOKED);
        writer.Write(value.MIXED_HIGH_COOKED);
        writer.Write(value.POOP_MEALS_COOKED);
        writer.Write(value.GRASS_MEALS_COOKED);
        writer.Write(value.FOLLOWER_MEAT_MEALS_COOKED);
        writer.Write(value.DEADLY_MEALS_COOKED);
        resolver.GetFormatterWithVerify<List<string>>().Serialize(ref writer, value.ReapedSouls, options);
        writer.WriteNil();
        writer.WriteNil();
        resolver.GetFormatterWithVerify<List<DataManager.EnemyData>>().Serialize(ref writer, value.EnemiesKilled, options);
        resolver.GetFormatterWithVerify<Alerts>().Serialize(ref writer, value.Alerts, options);
        resolver.GetFormatterWithVerify<List<FinalizedNotification>>().Serialize(ref writer, value.NotificationHistory, options);
        writer.Write(value.blizzardTimeInCurrentSeason);
        writer.Write(value.blizzardEndTimeInCurrentSeason);
        writer.Write(value.blizzardTimeInCurrentSeason2);
        writer.Write(value.blizzardEndTimeInCurrentSeason2);
        writer.Write(value.OnboardedYewCursedDungeon);
        writer.Write(value.BaalNeedsRescue);
        writer.Write(value.BaalRescued);
        writer.Write(value.StelleSpecialEncountered);
        writer.Write(value.MonchMamaSpecialEncountered);
        writer.Write(value.FishermanDLCSpecialEncountered);
        writer.Write(value.ShowSpecialStelleRoom);
        writer.Write(value.ShowSpecialMonchMamaRoom);
        writer.Write(value.ShowSpecialFishermanDLCRoom);
        writer.Write(value.InfectedDudeSpecialEncountered);
        writer.Write(value.DungeonRancherSpecialEncountered);
        writer.Write(value.RefinedResourcesSpecialEncountered);
        writer.Write(value.ShowSpecialInfectedDudeRoom);
        writer.Write(value.ShowSpecialDungeonRancherRoom);
        writer.Write(value.ShowSpecialRefinedResourcesRoom);
        writer.Write(value.StripperGaveOutfit);
        writer.Write(value.OnboardedRotRoom);
        writer.Write(value.ExecutionerRoom1Encountered);
        writer.Write(value.ExecutionerRoom2Encountered);
        writer.Write(value.CanShowExecutionerRoom1);
        writer.Write(value.CanShowExecutionerRoom2);
        writer.Write(value.EncounteredDungeonRancherCount);
        writer.Write(value.CollectedLightningShards);
        writer.Write(value.FoundHollowKnightWool);
        writer.Write(value.EnabledDLCMapHeart);
        writer.Write(value.FinalDLCMap);
        writer.Write(value.NPCGhostGeneric11Rescued);
        writer.Write(value.DiedToWolfBoss);
        writer.Write(value.DiedToYngyaBoss);
        writer.Write(value.DragonIntrod);
        writer.Write(value.DragonEggsCollected);
        resolver.GetFormatterWithVerify<StructuresData.Ranchable_Animal>().Serialize(ref writer, value.bestFriendAnimal, options);
        writer.Write(value.bestFriendAnimalLevel);
        writer.Write(value.bestFriendAnimalAdoration);
        writer.Write(value.LastAnimalToStarveDay);
        writer.Write(value.GivenNarayanaFollower);
        writer.Write(value.MapLockCountToUnlock);
        writer.Write(value.MysticKeeperBeatenYngya);
        writer.Write(value.SpokenToMysticKeeperWinter);
        writer.Write(value.RevealedBaseYngyaShrine);
        writer.Write(value.HasPureBloodMatingQuestAccepted);
        writer.Write(value.FlockadeDecoWoolWon);
        writer.Write(value.FlockadeFlockadeWoolWon);
        writer.Write(value.FlockadeGraveyardWoolWon);
        writer.Write(value.FlockadeRancherWoolWon);
        writer.Write(value.FlockadeTarotWoolWon);
        writer.Write(value.FlockadeBlacksmithWoolWon);
        writer.Write(value.FoundLegendaryDagger);
        writer.Write(value.FoundLegendaryBlunderbuss);
        writer.Write(value.Dungeon5Harder);
        writer.Write(value.Dungeon6Harder);
        writer.Write(value.EncounteredSabnock);
        writer.Write(value.ForceSinRoom);
        writer.Write(value.ForceHeartRoom);
        writer.Write(value.ForceDragonRoom);
        resolver.GetFormatterWithVerify<List<InventoryItem>>().Serialize(ref writer, value.GivenUpWolfFood, options);
        writer.Write(value.GivenUpHeartToWolf);
        writer.Write(value.WoolhavenDecorationCouunt);
        writer.Write(value.FirstRotFollowerAilmentAvoided);
        writer.Write(value.RemoveBlizzardsBeforeTimestamp);
        writer.Write(value.DisableBlizzard1);
        writer.Write(value.DisableBlizzard2);
        writer.Write(value.GofernonRotburnProgress);
        resolver.GetFormatterWithVerify<List<int>>().Serialize(ref writer, value.LastDungeonSeeds, options);
        writer.Write(value.FishermanWinterConvo);
        writer.Write(value.Twitch_Drop_16);
        writer.Write(value.Twitch_Drop_17);
        writer.Write(value.Twitch_Drop_18);
        writer.Write(value.Twitch_Drop_19);
        writer.Write(value.Twitch_Drop_20);
        writer.Write(value.RevealedWolfNode);
        resolver.GetFormatterWithVerify<RelicType>().Serialize(ref writer, value.PreviousRelic, options);
        writer.Write(value.FirstDungeon6RescueRoom);
        writer.Write(value.WoolhavenSkinsPurchased);
      }
    }

    public DataManager Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (DataManager) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      DataManager dataManager = new DataManager();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            dataManager.AllowSaving = reader.ReadBoolean();
            break;
          case 1:
            dataManager.DisableSaving = reader.ReadBoolean();
            break;
          case 2:
            dataManager.PauseGameTime = reader.ReadBoolean();
            break;
          case 3:
            dataManager.GameOverEnabled = reader.ReadBoolean();
            break;
          case 4:
            dataManager.DisplayGameOverWarning = reader.ReadBoolean();
            break;
          case 5:
            dataManager.InGameOver = reader.ReadBoolean();
            break;
          case 6:
            dataManager.GameOver = reader.ReadBoolean();
            break;
          case 7:
            dataManager.DifficultyChosen = reader.ReadBoolean();
            break;
          case 8:
            dataManager.DifficultyReminded = reader.ReadBoolean();
            break;
          case 9:
            dataManager.DisableYngyaShrine = reader.ReadBoolean();
            break;
          case 10:
            dataManager.playerDeaths = reader.ReadInt32();
            break;
          case 11:
            dataManager.playerDeathsInARow = reader.ReadInt32();
            break;
          case 12:
            dataManager.playerDeathsInARowFightingLeader = reader.ReadInt32();
            break;
          case 13:
            dataManager.FightPitRituals = reader.ReadInt32();
            break;
          case 14:
            dataManager.dungeonRun = reader.ReadInt32();
            break;
          case 15:
            dataManager.dungeonRunDuration = reader.ReadSingle();
            break;
          case 16 /*0x10*/:
            dataManager.dungeonVisitedRooms = resolver.GetFormatterWithVerify<List<Map.NodeType>>().Deserialize(ref reader, options);
            break;
          case 17:
            dataManager.dungeonLocationsVisited = resolver.GetFormatterWithVerify<List<FollowerLocation>>().Deserialize(ref reader, options);
            break;
          case 18:
            dataManager.FollowersRecruitedInNodes = resolver.GetFormatterWithVerify<List<int>>().Deserialize(ref reader, options);
            break;
          case 19:
            dataManager.FollowersRecruitedThisNode = reader.ReadInt32();
            break;
          case 20:
            dataManager.TimeInGame = reader.ReadSingle();
            break;
          case 21:
            dataManager.KillsInGame = reader.ReadInt32();
            break;
          case 22:
            dataManager.dungeonRunXPOrbs = reader.ReadInt32();
            break;
          case 23:
            dataManager.ChestRewardCount = reader.ReadInt32();
            break;
          case 24:
            dataManager.BaseGoopDoorLocked = reader.ReadBoolean();
            break;
          case 25:
            dataManager.BaseGoopDoorLoc = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 26:
            dataManager.STATS_FollowersStarvedToDeath = reader.ReadInt32();
            break;
          case 27:
            dataManager.STATS_Murders = reader.ReadInt32();
            break;
          case 28:
            dataManager.STATS_Sacrifices = reader.ReadInt32();
            break;
          case 29:
            dataManager.STATS_AnimalSacrifices = reader.ReadInt32();
            break;
          case 30:
            dataManager.STATS_NaturalDeaths = reader.ReadInt32();
            break;
          case 31 /*0x1F*/:
            dataManager.PlayerKillsOnRun = reader.ReadInt32();
            break;
          case 32 /*0x20*/:
            dataManager.PlayerStartingBlackSouls = reader.ReadInt32();
            break;
          case 33:
            dataManager.GivenFollowerHearts = reader.ReadBoolean();
            break;
          case 34:
            dataManager.EnabledSpells = reader.ReadBoolean();
            break;
          case 35:
            dataManager.ForceDoctrineStones = reader.ReadBoolean();
            break;
          case 36:
            dataManager.SpaceOutDoctrineStones = reader.ReadInt32();
            break;
          case 37:
            dataManager.DoctrineStoneTotalCount = reader.ReadInt32();
            break;
          case 38:
            dataManager.BuildShrineEnabled = reader.ReadBoolean();
            break;
          case 39:
            dataManager.EnabledHealing = reader.ReadBoolean();
            break;
          case 40:
            dataManager.EnabledSword = reader.ReadBoolean();
            break;
          case 41:
            dataManager.BonesEnabled = reader.ReadBoolean();
            break;
          case 42:
            dataManager.XPEnabled = reader.ReadBoolean();
            break;
          case 43:
            dataManager.ShownDodgeTutorial = reader.ReadBoolean();
            break;
          case 44:
            dataManager.ShownInventoryTutorial = reader.ReadBoolean();
            break;
          case 45:
            dataManager.ShownDodgeTutorialCount = reader.ReadInt32();
            break;
          case 46:
            dataManager.HadInitialDeathCatConversation = reader.ReadBoolean();
            break;
          case 47:
            dataManager.PlayerHasBeenGivenHearts = reader.ReadBoolean();
            break;
          case 48 /*0x30*/:
            dataManager.TotalFirefliesCaught = reader.ReadInt32();
            break;
          case 49:
            dataManager.TotalSquirrelsCaught = reader.ReadInt32();
            break;
          case 50:
            dataManager.TotalBirdsCaught = reader.ReadInt32();
            break;
          case 51:
            dataManager.TotalBodiesHarvested = reader.ReadInt32();
            break;
          case 52:
            dataManager.CurrentOnboardingPhase = resolver.GetFormatterWithVerify<DataManager.OnboardingPhase>().Deserialize(ref reader, options);
            break;
          case 53:
            dataManager.firstRecruit = reader.ReadBoolean();
            break;
          case 54:
            dataManager.MissionariesCompleted = reader.ReadInt32();
            break;
          case 55:
            dataManager.PlayerFleece = reader.ReadInt32();
            break;
          case 56:
            dataManager.PlayerVisualFleece = reader.ReadInt32();
            break;
          case 57:
            dataManager.UnlockedFleeces = resolver.GetFormatterWithVerify<List<int>>().Deserialize(ref reader, options);
            break;
          case 58:
            dataManager.PostGameFleecesOnboarded = reader.ReadBoolean();
            break;
          case 59:
            dataManager.GoatFleeceOnboarded = reader.ReadBoolean();
            break;
          case 60:
            dataManager.CowboyFleeceOnboarded = reader.ReadBoolean();
            break;
          case 61:
            dataManager.Thoughts = resolver.GetFormatterWithVerify<List<ThoughtData>>().Deserialize(ref reader, options);
            break;
          case 62:
            dataManager.CanReadMinds = reader.ReadBoolean();
            break;
          case 63 /*0x3F*/:
            dataManager.HappinessEnabled = reader.ReadBoolean();
            break;
          case 64 /*0x40*/:
            dataManager.TeachingsEnabled = reader.ReadBoolean();
            break;
          case 65:
            dataManager.SchedulingEnabled = reader.ReadBoolean();
            break;
          case 66:
            dataManager.PrayerEnabled = reader.ReadBoolean();
            break;
          case 67:
            dataManager.PrayerOrdered = reader.ReadBoolean();
            break;
          case 68:
            dataManager.HasBuiltCookingFire = reader.ReadBoolean();
            break;
          case 69:
            dataManager.HasBuiltFarmPlot = reader.ReadBoolean();
            break;
          case 70:
            dataManager.HasBuiltTemple1 = reader.ReadBoolean();
            break;
          case 71:
            dataManager.HasBuiltTemple2 = reader.ReadBoolean();
            break;
          case 72:
            dataManager.HasBuiltTemple3 = reader.ReadBoolean();
            break;
          case 73:
            dataManager.HasBuiltTemple4 = reader.ReadBoolean();
            break;
          case 74:
            dataManager.HasBuiltShrine1 = reader.ReadBoolean();
            break;
          case 75:
            dataManager.HasBuiltShrine2 = reader.ReadBoolean();
            break;
          case 76:
            dataManager.HasBuiltShrine3 = reader.ReadBoolean();
            break;
          case 77:
            dataManager.HasBuiltShrine4 = reader.ReadBoolean();
            break;
          case 78:
            dataManager.HasBuiltPleasureShrine = reader.ReadBoolean();
            break;
          case 79:
            dataManager.HasOnboardedCultLevel = reader.ReadBoolean();
            break;
          case 80 /*0x50*/:
            dataManager.PerformedMushroomRitual = reader.ReadBoolean();
            break;
          case 81:
            dataManager.BuiltMushroomDecoration = reader.ReadBoolean();
            break;
          case 82:
            dataManager.HasBuiltSurveillance = reader.ReadBoolean();
            break;
          case 83:
            dataManager.TempleDevotionBoxCoinCount = reader.ReadInt32();
            break;
          case 84:
            dataManager.CanBuildShrine = reader.ReadBoolean();
            break;
          case 85:
            dataManager.DaySinseLastMutatedFollower = reader.ReadInt32();
            break;
          case 86:
            dataManager.SeasonsActive = reader.ReadBoolean();
            break;
          case 87:
            dataManager.SeasonTimestamp = reader.ReadInt32();
            break;
          case 88:
            dataManager.CurrentSeason = resolver.GetFormatterWithVerify<SeasonsManager.Season>().Deserialize(ref reader, options);
            break;
          case 89:
            dataManager.PreviousSeason = resolver.GetFormatterWithVerify<SeasonsManager.Season>().Deserialize(ref reader, options);
            break;
          case 90:
            dataManager.CurrentWeatherEvent = resolver.GetFormatterWithVerify<SeasonsManager.WeatherEvent>().Deserialize(ref reader, options);
            break;
          case 91:
            dataManager.BlizzardEventID = reader.ReadInt32();
            break;
          case 92:
            dataManager.WeatherEventID = reader.ReadInt32();
            break;
          case 93:
            dataManager.WintersOccured = reader.ReadInt32();
            break;
          case 94:
            dataManager.WinterServerity = reader.ReadInt32();
            break;
          case 95:
            dataManager.NextWinterServerity = reader.ReadInt32();
            break;
          case 96 /*0x60*/:
            dataManager.NextPhaseIsWeatherEvent = reader.ReadBoolean();
            break;
          case 97:
            dataManager.GivenBlizzardObjective = reader.ReadBoolean();
            break;
          case 98:
            dataManager.OnboardedDLCEntrance = reader.ReadBoolean();
            break;
          case 99:
            dataManager.OnboardedBaseExpansion = reader.ReadBoolean();
            break;
          case 100:
            dataManager.OnboardedWolf = reader.ReadBoolean();
            break;
          case 101:
            dataManager.OnboardedLambTown = reader.ReadBoolean();
            break;
          case 102:
            dataManager.OnboardedLambGhostNPCs = reader.ReadBoolean();
            break;
          case 103:
            dataManager.OnboardedYngyaAwoken = reader.ReadBoolean();
            break;
          case 104:
            dataManager.OnboardedDungeon6 = reader.ReadBoolean();
            break;
          case 105:
            dataManager.OnboardedIntroYngyaShrine = reader.ReadBoolean();
            break;
          case 106:
            dataManager.OnboardedFindLostSouls = reader.ReadBoolean();
            break;
          case 107:
            dataManager.OnboardedAddFuelToFurnace = reader.ReadBoolean();
            break;
          case 108:
            dataManager.RequiresSnowedUnderOnboarded = reader.ReadBoolean();
            break;
          case 109:
            dataManager.RequiresWolvesOnboarded = reader.ReadBoolean();
            break;
          case 110:
            dataManager.WinterMaxSeverity = reader.ReadBoolean();
            break;
          case 111:
            dataManager.RequiresBlizzardOnboarded = reader.ReadBoolean();
            break;
          case 112 /*0x70*/:
            dataManager.OnboardedRanchingWolves = reader.ReadBoolean();
            break;
          case 113:
            dataManager.OnboardedBlizzards = reader.ReadBoolean();
            break;
          case 114:
            dataManager.OnboardedSnowedUnder = reader.ReadBoolean();
            break;
          case 115:
            dataManager.OnboardedWitheredCrops = reader.ReadBoolean();
            break;
          case 116:
            dataManager.OnboardedLongNights = reader.ReadBoolean();
            break;
          case 117:
            dataManager.LongNightActive = reader.ReadBoolean();
            break;
          case 118:
            dataManager.OnboardedRanching = reader.ReadBoolean();
            break;
          case 119:
            dataManager.OnboardedSeasons = reader.ReadBoolean();
            break;
          case 120:
            dataManager.OnboardedDLCBuildMenu = reader.ReadBoolean();
            break;
          case 121:
            dataManager.DLCUpgradeTreeSnowIncrement = reader.ReadInt32();
            break;
          case 122:
            dataManager.BuiltFurnace = reader.ReadBoolean();
            break;
          case 123:
            dataManager.OnboardedLightningShardDungeon = reader.ReadBoolean();
            break;
          case 124:
            dataManager.OnboardedRotstoneDungeon = reader.ReadBoolean();
            break;
          case 125:
            dataManager.OnboardedRotstone = reader.ReadBoolean();
            break;
          case 126:
            dataManager.CollectedRotstone = reader.ReadBoolean();
            break;
          case (int) sbyte.MaxValue:
            dataManager.CollectedYewMutated = reader.ReadBoolean();
            break;
          case 128 /*0x80*/:
            dataManager.OnboardedMutationRoom = reader.ReadBoolean();
            break;
          case 129:
            dataManager.OnboardedRotHelobFollowers = reader.ReadBoolean();
            break;
          case 130:
            dataManager.PlayedFinalYngyaConvo = reader.ReadBoolean();
            break;
          case 131:
            dataManager.YngyaOffering = reader.ReadInt32();
            break;
          case 132:
            dataManager.YngyaRotOfferingsReceived = reader.ReadInt32();
            break;
          case 133:
            dataManager.SpokeToYngyaOnMountainTop = reader.ReadBoolean();
            break;
          case 134:
            dataManager.ShowIcegoreRoom = reader.ReadBoolean();
            break;
          case 135:
            dataManager.EncounteredIcegoreRoom = reader.ReadBoolean();
            break;
          case 136:
            dataManager.SpokenToRatauWinter = reader.ReadBoolean();
            break;
          case 137:
            dataManager.SpokenToPlimboWinter = reader.ReadBoolean();
            break;
          case 138:
            dataManager.SpokenToPlimboBlunderbuss = reader.ReadBoolean();
            break;
          case 139:
            dataManager.SpokenToClauneckWinter = reader.ReadBoolean();
            break;
          case 140:
            dataManager.SpokenToKudaiiWinter = reader.ReadBoolean();
            break;
          case 141:
            dataManager.SpokenToChemachWinter = reader.ReadBoolean();
            break;
          case 142:
            dataManager.SpokenToChemachRot = reader.ReadBoolean();
            break;
          case 143:
            dataManager.SpokenToKudaiiRot = reader.ReadBoolean();
            break;
          case 144 /*0x90*/:
            dataManager.SpokenToClauneckRot = reader.ReadBoolean();
            break;
          case 145:
            dataManager.DLCDungeonNodesCompleted = resolver.GetFormatterWithVerify<List<int>>().Deserialize(ref reader, options);
            break;
          case 146:
            dataManager.DLCDungeonNodeCurrent = reader.ReadInt32();
            break;
          case 147:
            dataManager.DLCKey_1 = reader.ReadInt32();
            break;
          case 148:
            dataManager.DLCKey_2 = reader.ReadInt32();
            break;
          case 149:
            dataManager.DLCKey_3 = reader.ReadInt32();
            break;
          case 150:
            dataManager.RevealedPostGameDungeon5 = reader.ReadBoolean();
            break;
          case 151:
            dataManager.RevealedPostGameDungeon6 = reader.ReadBoolean();
            break;
          case 152:
            dataManager.RevealDLCDungeonNode = reader.ReadBoolean();
            break;
          case 153:
            dataManager.CurrentDLCDungeonID = reader.ReadInt32();
            break;
          case 154:
            dataManager.CurrentDLCNodeType = resolver.GetFormatterWithVerify<DungeonWorldMapIcon.NodeType>().Deserialize(ref reader, options);
            break;
          case 155:
            dataManager.HasYngyaConvo = reader.ReadBoolean();
            break;
          case 156:
            dataManager.IsMiniBoss = reader.ReadBoolean();
            break;
          case 157:
            dataManager.IsLambGhostRescue = reader.ReadBoolean();
            break;
          case 158:
            dataManager.DLCDungeon5MiniBossIndex = reader.ReadInt32();
            break;
          case 159:
            dataManager.DLCDungeon6MiniBossIndex = reader.ReadInt32();
            break;
          case 160 /*0xA0*/:
            dataManager.PuzzleRoomsCompleted = reader.ReadInt32();
            break;
          case 161:
            dataManager.RancherSpokeAboutBrokenShop = reader.ReadBoolean();
            break;
          case 162:
            dataManager.RancherShopFixed = reader.ReadBoolean();
            break;
          case 163:
            dataManager.RancherOnboardedLightningShards = reader.ReadBoolean();
            break;
          case 164:
            dataManager.RancherOnboardedHolyYew = reader.ReadBoolean();
            break;
          case 165:
            dataManager.FlockadeSpokeAboutBrokenShop = reader.ReadBoolean();
            break;
          case 166:
            dataManager.FlockadeShopFixed = reader.ReadBoolean();
            break;
          case 167:
            dataManager.FlockadeBlacksmithWon = reader.ReadBoolean();
            break;
          case 168:
            dataManager.FlockadeDecoWon = reader.ReadBoolean();
            break;
          case 169:
            dataManager.FlockadeFlockadeWon = reader.ReadBoolean();
            break;
          case 170:
            dataManager.FlockadeGraveyardWon = reader.ReadBoolean();
            break;
          case 171:
            dataManager.FlockadeRancherWon = reader.ReadBoolean();
            break;
          case 172:
            dataManager.FlockadeTarotWon = reader.ReadBoolean();
            break;
          case 173:
            dataManager.TarotSpokeAboutBrokenShop = reader.ReadBoolean();
            break;
          case 174:
            dataManager.TarotShopFixed = reader.ReadBoolean();
            break;
          case 175:
            dataManager.DecoSpokeAboutBrokenShop = reader.ReadBoolean();
            break;
          case 176 /*0xB0*/:
            dataManager.DecoShopFixed = reader.ReadBoolean();
            break;
          case 177:
            dataManager.BlacksmithSpokeAboutBrokenShop = reader.ReadBoolean();
            break;
          case 178:
            dataManager.BlacksmithShopFixed = reader.ReadBoolean();
            break;
          case 179:
            dataManager.GraveyardSpokeAboutBrokenShop = reader.ReadBoolean();
            break;
          case 180:
            dataManager.GraveyardShopFixed = reader.ReadBoolean();
            break;
          case 181:
            dataManager.OnboardedLambTownGhost7 = reader.ReadBoolean();
            break;
          case 182:
            dataManager.OnboardedLambTownGhost8 = reader.ReadBoolean();
            break;
          case 183:
            dataManager.OnboardedLambTownGhost9 = reader.ReadBoolean();
            break;
          case 184:
            dataManager.OnboardedLambTownGhost10 = reader.ReadBoolean();
            break;
          case 185:
            dataManager.NPCsRescued = reader.ReadInt32();
            break;
          case 186:
            dataManager.NPCGhostRancherRescued = reader.ReadBoolean();
            break;
          case 187:
            dataManager.NPCGhostFlockadeRescued = reader.ReadBoolean();
            break;
          case 188:
            dataManager.NPCGhostTarotRescued = reader.ReadBoolean();
            break;
          case 189:
            dataManager.NPCGhostDecoRescued = reader.ReadBoolean();
            break;
          case 190:
            dataManager.NPCGhostBlacksmithRescued = reader.ReadBoolean();
            break;
          case 191:
            dataManager.NPCGhostGraveyardRescued = reader.ReadBoolean();
            break;
          case 192 /*0xC0*/:
            dataManager.NPCGhostGeneric7Rescued = reader.ReadBoolean();
            break;
          case 193:
            dataManager.NPCGhostGeneric8Rescued = reader.ReadBoolean();
            break;
          case 194:
            dataManager.NPCGhostGeneric9Rescued = reader.ReadBoolean();
            break;
          case 195:
            dataManager.NPCGhostGeneric10Rescued = reader.ReadBoolean();
            break;
          case 196:
            dataManager.RepairedLegendarySword = reader.ReadBoolean();
            break;
          case 197:
            dataManager.RepairedLegendaryAxe = reader.ReadBoolean();
            break;
          case 198:
            dataManager.RepairedLegendaryHammer = reader.ReadBoolean();
            break;
          case 199:
            dataManager.RepairedLegendaryGauntlet = reader.ReadBoolean();
            break;
          case 200:
            dataManager.RepairedLegendaryBlunderbuss = reader.ReadBoolean();
            break;
          case 201:
            dataManager.RepairedLegendaryDagger = reader.ReadBoolean();
            break;
          case 202:
            dataManager.RepairedLegendaryChains = reader.ReadBoolean();
            break;
          case 203:
            dataManager.OnboardedLegendaryWeapons = reader.ReadBoolean();
            break;
          case 204:
            dataManager.FindBrokenHammerWeapon = reader.ReadBoolean();
            break;
          case 205:
            dataManager.GivenBrokenHammerWeaponQuest = reader.ReadBoolean();
            break;
          case 206:
            dataManager.GaveChosenChildQuest = reader.ReadBoolean();
            break;
          case 207:
            dataManager.ChosenChildLeftInTheMidasCave = reader.ReadBoolean();
            break;
          case 208 /*0xD0*/:
            dataManager.FoundLegendarySword = reader.ReadBoolean();
            break;
          case 209:
            dataManager.ChosenChildMeditationQuestDay = reader.ReadInt32();
            break;
          case 210:
            dataManager.LegendarySwordHinted = reader.ReadBoolean();
            break;
          case 211:
            dataManager.LegendaryAxeHinted = reader.ReadBoolean();
            break;
          case 212:
            dataManager.DeliveredCharybisLetter = reader.ReadBoolean();
            break;
          case 213:
            dataManager.BringFishermanWoolStarted = reader.ReadBoolean();
            break;
          case 214:
            dataManager.FishermanGaveWoolAmount = reader.ReadInt32();
            break;
          case 215:
            dataManager.BroughtFishingRod = reader.ReadBoolean();
            break;
          case 216:
            dataManager.LegendaryDaggerHinted = reader.ReadBoolean();
            break;
          case 217:
            dataManager.FoundLegendaryGauntlets = reader.ReadBoolean();
            break;
          case 218:
            dataManager.LegendaryGauntletsHinted = reader.ReadBoolean();
            break;
          case 219:
            dataManager.LegendaryBlunderbussHinted = reader.ReadBoolean();
            break;
          case 220:
            dataManager.LegendaryBlunderbussPlimboEaterEggTalked = reader.ReadBoolean();
            break;
          case 221:
            dataManager.KudaaiLegendaryWeaponsResponses = resolver.GetFormatterWithVerify<List<EquipmentType>>().Deserialize(ref reader, options);
            break;
          case 222:
            dataManager.LegendaryWeaponsUnlockOrder = resolver.GetFormatterWithVerify<List<EquipmentType>>().Deserialize(ref reader, options);
            break;
          case 223:
            dataManager.EncounteredBaseExpansionNPC = reader.ReadBoolean();
            break;
          case 224 /*0xE0*/:
            dataManager.WeatherEventTriggeredDay = reader.ReadInt32();
            break;
          case 225:
            dataManager.WeatherEventOverTime = reader.ReadSingle();
            break;
          case 226:
            dataManager.WeatherEventDurationTime = reader.ReadSingle();
            break;
          case 227:
            dataManager.SeasonSpecialEventTriggeredDay = reader.ReadInt32();
            break;
          case 228:
            dataManager.TimeSinceLastSnowedUnderStructure = reader.ReadSingle();
            break;
          case 229:
            dataManager.TimeSinceLastLightingStrikedFollower = reader.ReadSingle();
            break;
          case 230:
            dataManager.TimeSinceLastLightingStrikedStructure = reader.ReadSingle();
            break;
          case 231:
            dataManager.TimeSinceLastAflamedStructure = reader.ReadSingle();
            break;
          case 232:
            dataManager.TimeSinceLastAflamedFollower = reader.ReadSingle();
            break;
          case 233:
            dataManager.TimeSinceLastStolenCoins = reader.ReadSingle();
            break;
          case 234:
            dataManager.TimeSinceLastMurderedFollowerFromFollower = reader.ReadSingle();
            break;
          case 235:
            dataManager.TimeSinceLastSnowPileSpawned = reader.ReadSingle();
            break;
          case 236:
            dataManager.Temperature = reader.ReadSingle();
            break;
          case 237:
            dataManager.FollowerOnboardedWinterComing = reader.ReadBoolean();
            break;
          case 238:
            dataManager.FollowerOnboardedWinterHere = reader.ReadBoolean();
            break;
          case 239:
            dataManager.FollowerOnboardedWinterAlmostHere = reader.ReadBoolean();
            break;
          case 240 /*0xF0*/:
            dataManager.FollowerOnboardedBlizzard = reader.ReadBoolean();
            break;
          case 241:
            dataManager.FollowerOnboardedFreezing = reader.ReadBoolean();
            break;
          case 242:
            dataManager.FollowerOnboardedOverheating = reader.ReadBoolean();
            break;
          case 243:
            dataManager.FollowerOnboardedWoolyShack = reader.ReadBoolean();
            break;
          case 244:
            dataManager.FollowerOnboardedRanchChoppingBlock = reader.ReadBoolean();
            break;
          case 245:
            dataManager.FollowerOnboardedAutumnComing = reader.ReadBoolean();
            break;
          case 246:
            dataManager.FollowerOnboardedTyphoon = reader.ReadBoolean();
            break;
          case 247:
            dataManager.TriedTailorRequiresRevealingFromBase = reader.ReadBoolean();
            break;
          case 248:
            dataManager.TailorRequiresRevealingFromBase = reader.ReadBoolean();
            break;
          case 249:
            dataManager.NudeClothingCount = reader.ReadInt32();
            break;
          case 250:
            dataManager.SinSermonEnabled = reader.ReadBoolean();
            break;
          case 251:
            dataManager.PleasureRevealed = reader.ReadBoolean();
            break;
          case 252:
            dataManager.PleasureDoctrineOnboarded = reader.ReadBoolean();
            break;
          case 253:
            dataManager.PleasureDoctrineEnabled = reader.ReadBoolean();
            break;
          case 254:
            dataManager.WinterDoctrineEnabled = reader.ReadBoolean();
            break;
          case (int) byte.MaxValue:
            dataManager.WokeUpEveryoneDay = reader.ReadInt32();
            break;
          case 256 /*0x0100*/:
            dataManager.DiedLastRun = reader.ReadBoolean();
            break;
          case 257:
            dataManager.LastRunResults = resolver.GetFormatterWithVerify<Lamb.UI.DeathScreen.UIDeathScreenOverlayController.Results>().Deserialize(ref reader, options);
            break;
          case 258:
            dataManager.LastFollowerToStarveToDeath = reader.ReadSingle();
            break;
          case 259:
            dataManager.LastFollowerToFreezeToDeath = reader.ReadSingle();
            break;
          case 260:
            dataManager.LastFollowerToOverheatToDeath = reader.ReadSingle();
            break;
          case 261:
            dataManager.LastFollowerToBurnToDeath = reader.ReadSingle();
            break;
          case 262:
            dataManager.LastFollowerToStartStarving = reader.ReadSingle();
            break;
          case 263:
            dataManager.LastFollowerToStartDissenting = reader.ReadSingle();
            break;
          case 264:
            dataManager.LastFollowerToStartFreezing = reader.ReadSingle();
            break;
          case 265:
            dataManager.LastFollowerToStartSoaking = reader.ReadSingle();
            break;
          case 266:
            dataManager.LastFollowerToStartOverheating = reader.ReadSingle();
            break;
          case 267:
            dataManager.LastFollowerToReachOldAge = reader.ReadSingle();
            break;
          case 268:
            dataManager.LastFollowerToBecomeIll = reader.ReadSingle();
            break;
          case 269:
            dataManager.LastFollowerToBecomeIllFromSleepingNearIllFollower = reader.ReadSingle();
            break;
          case 270:
            dataManager.LastFollowerToPassOut = reader.ReadSingle();
            break;
          case 271:
            dataManager.LastFollowerPurchasedFromSpider = reader.ReadInt32();
            break;
          case 272:
            dataManager.TimeSinceFaithHitEmpty = reader.ReadSingle();
            break;
          case 273:
            dataManager.TimeSinceLastCrisisOfFaithQuest = reader.ReadSingle();
            break;
          case 274:
            dataManager.PalworldSkinsGivenLocations = resolver.GetFormatterWithVerify<List<FollowerLocation>>().Deserialize(ref reader, options);
            break;
          case 275:
            dataManager.PalworldEggSkinsGiven = resolver.GetFormatterWithVerify<List<string>>().Deserialize(ref reader, options);
            break;
          case 276:
            dataManager.PalworldEggsCollected = reader.ReadInt32();
            break;
          case 277:
            dataManager.ForcePalworldEgg = reader.ReadBoolean();
            break;
          case 278:
            dataManager.JudgementAmount = reader.ReadInt32();
            break;
          case 279:
            dataManager.HungerBarCount = reader.ReadSingle();
            break;
          case 280:
            dataManager.IllnessBarCount = reader.ReadSingle();
            break;
          case 281:
            dataManager.IllnessBarDynamicMax = reader.ReadSingle();
            break;
          case 282:
            dataManager.WarmthBarCount = reader.ReadSingle();
            break;
          case 283:
            dataManager.StaticFaith = reader.ReadSingle();
            break;
          case 284:
            dataManager.CultFaith = reader.ReadSingle();
            break;
          case 285:
            dataManager.LastBrainwashed = reader.ReadSingle();
            break;
          case 286:
            dataManager.LastHolidayDeclared = reader.ReadSingle();
            break;
          case 287:
            dataManager.LastPurgeDeclared = reader.ReadSingle();
            break;
          case 288:
            dataManager.LastNudismDeclared = reader.ReadSingle();
            break;
          case 289:
            dataManager.LastWorkThroughTheNight = reader.ReadSingle();
            break;
          case 290:
            dataManager.LastConstruction = reader.ReadSingle();
            break;
          case 291:
            dataManager.LastEnlightenment = reader.ReadSingle();
            break;
          case 292:
            dataManager.LastFastDeclared = reader.ReadSingle();
            break;
          case 293:
            dataManager.LastWarmthRitualDeclared = reader.ReadSingle();
            break;
          case 294:
            dataManager.LastFeastDeclared = reader.ReadSingle();
            break;
          case 295:
            dataManager.LastFishingDeclared = reader.ReadSingle();
            break;
          case 296:
            dataManager.LastHalloween = reader.ReadSingle();
            break;
          case 297:
            dataManager.LastCNY = reader.ReadSingle();
            break;
          case 298:
            dataManager.LastCthulhu = reader.ReadSingle();
            break;
          case 299:
            dataManager.LastRanchRitualMeat = reader.ReadSingle();
            break;
          case 300:
            dataManager.LastRanchRitualHarvest = reader.ReadSingle();
            break;
          case 301:
            dataManager.LastArcherShot = reader.ReadSingle();
            break;
          case 302:
            dataManager.LastSimpleGuardianAttacked = reader.ReadSingle();
            break;
          case 303:
            dataManager.LastSimpleGuardianRingProjectiles = reader.ReadSingle();
            break;
          case 304:
            dataManager.LastSimpleGuardianPatternShot = reader.ReadSingle();
            break;
          case 305:
            dataManager.LastDayTreesAtBase = reader.ReadInt32();
            break;
          case 306:
            dataManager.LastSnowPileAtBase = reader.ReadInt32();
            break;
          case 307:
            dataManager.PreviousSermonDayIndex = reader.ReadInt32();
            break;
          case 308:
            dataManager.PreviousSinSermonDayIndex = reader.ReadInt32();
            break;
          case 309:
            dataManager.PreviousSermonCategory = resolver.GetFormatterWithVerify<SermonCategory>().Deserialize(ref reader, options);
            break;
          case 310:
            dataManager.ShrineLevel = reader.ReadInt32();
            break;
          case 311:
            dataManager.TempleLevel = reader.ReadInt32();
            break;
          case 312:
            dataManager.TempleBorder = reader.ReadInt32();
            break;
          case 313:
            dataManager.TempleUnlockedBorder5 = reader.ReadBoolean();
            break;
          case 314:
            dataManager.TempleUnlockedBorder6 = reader.ReadBoolean();
            break;
          case 315:
            dataManager.GivenSermonQuest = reader.ReadBoolean();
            break;
          case 316:
            dataManager.GivenFaithOfFlockQuest = reader.ReadBoolean();
            break;
          case 317:
            dataManager.PrayedAtMassiveMonsterShrine = reader.ReadBoolean();
            break;
          case 318:
            dataManager.TwitchSecretKey = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 319:
            dataManager.TwitchToken = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 320:
            dataManager.ChannelID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 321:
            dataManager.ChannelName = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 322:
            dataManager.ReadTwitchMessages = resolver.GetFormatterWithVerify<List<string>>().Deserialize(ref reader, options);
            break;
          case 323:
            dataManager.TotemContributions = reader.ReadInt32();
            break;
          case 324:
            dataManager.TwitchSentFollowers = reader.ReadBoolean();
            break;
          case 325:
            dataManager.TwitchSettings = resolver.GetFormatterWithVerify<TwitchSettings>().Deserialize(ref reader, options);
            break;
          case 326:
            dataManager.TwitchTotemsCompleted = reader.ReadInt32();
            break;
          case 327:
            dataManager.TwitchNextHHEvent = reader.ReadSingle();
            break;
          case 328:
            dataManager.TwitchFollowerViewerIDs = resolver.GetFormatterWithVerify<List<string>>().Deserialize(ref reader, options);
            break;
          case 329:
            dataManager.TwitchFollowerIDs = resolver.GetFormatterWithVerify<List<string>>().Deserialize(ref reader, options);
            break;
          case 330:
            dataManager.OnboardingFinished = reader.ReadBoolean();
            break;
          case 331:
            dataManager.SaveUniqueID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 332:
            dataManager.enemiesEncountered = resolver.GetFormatterWithVerify<List<string>>().Deserialize(ref reader, options);
            break;
          case 333:
            dataManager.Chain1 = reader.ReadBoolean();
            break;
          case 334:
            dataManager.Chain2 = reader.ReadBoolean();
            break;
          case 335:
            dataManager.Chain3 = reader.ReadBoolean();
            break;
          case 336:
            dataManager.DoorRoomChainProgress = reader.ReadInt32();
            break;
          case 337:
            dataManager.DoorRoomDoorsProgress = reader.ReadInt32();
            break;
          case 338:
            dataManager.Dungeon1_Layer = reader.ReadInt32();
            break;
          case 339:
            dataManager.Dungeon2_Layer = reader.ReadInt32();
            break;
          case 340:
            dataManager.Dungeon3_Layer = reader.ReadInt32();
            break;
          case 341:
            dataManager.Dungeon4_Layer = reader.ReadInt32();
            break;
          case 342:
            dataManager.Dungeon5_Layer = reader.ReadInt32();
            break;
          case 343:
            dataManager.Dungeon6_Layer = reader.ReadInt32();
            break;
          case 344:
            dataManager.WinterLoopEnabled = reader.ReadBoolean();
            break;
          case 345:
            dataManager.WinterLoopModifiedDay = reader.ReadInt32();
            break;
          case 346:
            dataManager.ValentinsDayYear = reader.ReadInt32();
            break;
          case 347:
            dataManager.CheatHistory = resolver.GetFormatterWithVerify<List<string>>().Deserialize(ref reader, options);
            break;
          case 348:
            dataManager.First_Dungeon_Resurrecting = reader.ReadBoolean();
            break;
          case 349:
            dataManager.PermadeDeathActive = reader.ReadBoolean();
            break;
          case 350:
            dataManager.SpidersCaught = reader.ReadInt32();
            break;
          case 351:
            dataManager.FrogFollowerCount = reader.ReadInt32();
            break;
          case 352:
            dataManager.PhotoCameraLookedAtGallery = reader.ReadBoolean();
            break;
          case 353:
            dataManager.PhotoUsedCamera = reader.ReadBoolean();
            break;
          case 354:
            dataManager.clothesCrafted = resolver.GetFormatterWithVerify<List<FollowerClothingType>>().Deserialize(ref reader, options);
            break;
          case 355:
            dataManager.MiniBossData = resolver.GetFormatterWithVerify<List<MiniBossController.MiniBossData>>().Deserialize(ref reader, options);
            break;
          case 356:
            dataManager.CachePreviousRun = resolver.GetFormatterWithVerify<List<DataManager.LocationAndLayer>>().Deserialize(ref reader, options);
            break;
          case 357:
            dataManager.DiscoveredLocations = resolver.GetFormatterWithVerify<List<FollowerLocation>>().Deserialize(ref reader, options);
            break;
          case 358:
            dataManager.VisitedLocations = resolver.GetFormatterWithVerify<List<FollowerLocation>>().Deserialize(ref reader, options);
            break;
          case 359:
            dataManager.NewLocationFaithReward = resolver.GetFormatterWithVerify<List<FollowerLocation>>().Deserialize(ref reader, options);
            break;
          case 360:
            dataManager.DissentingFolllowerRooms = resolver.GetFormatterWithVerify<List<FollowerLocation>>().Deserialize(ref reader, options);
            break;
          case 361:
            dataManager.OpenedDungeonDoors = resolver.GetFormatterWithVerify<List<DataManager.LocationAndLayer>>().Deserialize(ref reader, options);
            break;
          case 362:
            dataManager.KeyPiecesFromLocation = resolver.GetFormatterWithVerify<List<string>>().Deserialize(ref reader, options);
            break;
          case 363:
            dataManager.UsedFollowerDispensers = resolver.GetFormatterWithVerify<List<FollowerLocation>>().Deserialize(ref reader, options);
            break;
          case 364:
            dataManager.UnlockedBossTempleDoor = resolver.GetFormatterWithVerify<List<FollowerLocation>>().Deserialize(ref reader, options);
            break;
          case 365:
            dataManager.UnlockedDungeonDoor = resolver.GetFormatterWithVerify<List<FollowerLocation>>().Deserialize(ref reader, options);
            break;
          case 366:
            dataManager.BossesCompleted = resolver.GetFormatterWithVerify<List<FollowerLocation>>().Deserialize(ref reader, options);
            break;
          case 367:
            dataManager.BossesEncountered = resolver.GetFormatterWithVerify<List<FollowerLocation>>().Deserialize(ref reader, options);
            break;
          case 368:
            dataManager.DoorRoomBossLocksDestroyed = resolver.GetFormatterWithVerify<List<FollowerLocation>>().Deserialize(ref reader, options);
            break;
          case 369:
            dataManager.SignPostsRead = resolver.GetFormatterWithVerify<List<FollowerLocation>>().Deserialize(ref reader, options);
            break;
          case 370:
            dataManager.ShrineDoor = reader.ReadBoolean();
            break;
          case 371:
            dataManager.JobBoardsClaimedQuests = resolver.GetFormatterWithVerify<List<int>>().Deserialize(ref reader, options);
            break;
          case 372:
            dataManager.OnboardedRanchingJobBoard = reader.ReadBoolean();
            break;
          case 373:
            dataManager.CompletedRanchingJobBoard = reader.ReadBoolean();
            break;
          case 374:
            dataManager.OnboardedFlockadeJobBoard = reader.ReadBoolean();
            break;
          case 375:
            dataManager.CompletedFlockadeJobBoard = reader.ReadBoolean();
            break;
          case 376:
            dataManager.OnboardedBlacksmithJobBoard = reader.ReadBoolean();
            break;
          case 377:
            dataManager.CompletedBlacksmithJobBoard = reader.ReadBoolean();
            break;
          case 378:
            dataManager.OnboardedTarotJobBoard = reader.ReadBoolean();
            break;
          case 379:
            dataManager.CompletedTarotJobBoard = reader.ReadBoolean();
            break;
          case 380:
            dataManager.OnboardedDecoJobBoard = reader.ReadBoolean();
            break;
          case 381:
            dataManager.CompletedDecoJobBoard = reader.ReadBoolean();
            break;
          case 382:
            dataManager.OnboardedGraveyardJobBoard = reader.ReadBoolean();
            break;
          case 383:
            dataManager.CompletedGraveyardJobBoard = reader.ReadBoolean();
            break;
          case 384:
            dataManager.HasFurnace = reader.ReadBoolean();
            break;
          case 385:
            dataManager.BaseDoorEast = reader.ReadBoolean();
            break;
          case 386:
            dataManager.BaseDoorNorthEast = reader.ReadBoolean();
            break;
          case 387:
            dataManager.BaseDoorNorthWest = reader.ReadBoolean();
            break;
          case 388:
            dataManager.BossForest = reader.ReadBoolean();
            break;
          case 389:
            dataManager.ForestTempleDoor = reader.ReadBoolean();
            break;
          case 390:
            dataManager.CompletedQuestFollowerIDs = resolver.GetFormatterWithVerify<List<int>>().Deserialize(ref reader, options);
            break;
          case 391:
            dataManager.CurrentCultLevel = resolver.GetFormatterWithVerify<DataManager.CultLevel>().Deserialize(ref reader, options);
            break;
          case 392:
            dataManager.MechanicsUnlocked = resolver.GetFormatterWithVerify<List<UnlockManager.UnlockType>>().Deserialize(ref reader, options);
            break;
          case 393:
            dataManager.UnlockedSermonsAndRituals = resolver.GetFormatterWithVerify<List<SermonsAndRituals.SermonRitualType>>().Deserialize(ref reader, options);
            break;
          case 394:
            dataManager.UnlockedStructures = resolver.GetFormatterWithVerify<List<StructureBrain.TYPES>>().Deserialize(ref reader, options);
            break;
          case 395:
            dataManager.HistoryOfStructures = resolver.GetFormatterWithVerify<List<StructureBrain.TYPES>>().Deserialize(ref reader, options);
            break;
          case 396:
            dataManager.DayPreviosulyUsedStructures = resolver.GetFormatterWithVerify<Dictionary<StructureBrain.TYPES, int>>().Deserialize(ref reader, options);
            break;
          case 397:
            dataManager.NewBuildings = reader.ReadBoolean();
            break;
          case 398:
            dataManager.RevealedTutorialTopics = resolver.GetFormatterWithVerify<List<TutorialTopic>>().Deserialize(ref reader, options);
            break;
          case 399:
            dataManager.CurrentResearch = resolver.GetFormatterWithVerify<List<StructuresData.ResearchObject>>().Deserialize(ref reader, options);
            break;
          case 400:
            dataManager.CurrentUpgradeTreeTier = resolver.GetFormatterWithVerify<Lamb.UI.UpgradeTreeNode.TreeTier>().Deserialize(ref reader, options);
            break;
          case 401:
            dataManager.DLCCurrentUpgradeTreeTier = resolver.GetFormatterWithVerify<Lamb.UI.UpgradeTreeNode.TreeTier>().Deserialize(ref reader, options);
            break;
          case 402:
            dataManager.CurrentPlayerUpgradeTreeTier = resolver.GetFormatterWithVerify<Lamb.UI.UpgradeTreeNode.TreeTier>().Deserialize(ref reader, options);
            break;
          case 403:
            dataManager.MostRecentTreeUpgrade = resolver.GetFormatterWithVerify<UpgradeSystem.Type>().Deserialize(ref reader, options);
            break;
          case 404:
            dataManager.MostRecentPlayerTreeUpgrade = resolver.GetFormatterWithVerify<UpgradeSystem.Type>().Deserialize(ref reader, options);
            break;
          case 405:
            dataManager.UnlockedUpgrades = resolver.GetFormatterWithVerify<List<UpgradeSystem.Type>>().Deserialize(ref reader, options);
            break;
          case 406:
            dataManager.DoctrineUnlockedUpgrades = resolver.GetFormatterWithVerify<List<DoctrineUpgradeSystem.DoctrineType>>().Deserialize(ref reader, options);
            break;
          case 407:
            dataManager.UpgradeCoolDowns = resolver.GetFormatterWithVerify<List<UpgradeSystem.UpgradeCoolDown>>().Deserialize(ref reader, options);
            break;
          case 408:
            dataManager.CultTraits = resolver.GetFormatterWithVerify<List<FollowerTrait.TraitType>>().Deserialize(ref reader, options);
            break;
          case 409:
            dataManager.WeaponUnlockedUpgrades = resolver.GetFormatterWithVerify<List<string>>().Deserialize(ref reader, options);
            break;
          case 410:
            dataManager.CultName = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 411:
            dataManager.MysticKeeperName = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 412:
            dataManager.PlayerTriedToEnterMysticDimensionCount = reader.ReadInt32();
            break;
          case 413:
            dataManager.MysticRewardCount = reader.ReadInt32();
            break;
          case 414:
            dataManager.DungeonBossFight = reader.ReadBoolean();
            break;
          case 415:
            dataManager.LocationSeeds = resolver.GetFormatterWithVerify<List<DataManager.LocationSeedsData>>().Deserialize(ref reader, options);
            break;
          case 416:
            dataManager.WeatherType = resolver.GetFormatterWithVerify<WeatherSystemController.WeatherType>().Deserialize(ref reader, options);
            break;
          case 417:
            dataManager.WeatherStrength = resolver.GetFormatterWithVerify<WeatherSystemController.WeatherStrength>().Deserialize(ref reader, options);
            break;
          case 418:
            dataManager.WeatherStartingTime = reader.ReadSingle();
            break;
          case 419:
            dataManager.WeatherDuration = reader.ReadInt32();
            break;
          case 420:
            dataManager.TempleStudyXP = reader.ReadSingle();
            break;
          case 421:
            dataManager.UnlockededASermon = reader.ReadInt32();
            break;
          case 422:
            dataManager.CurrentDayIndex = reader.ReadInt32();
            break;
          case 423:
            dataManager.CurrentPhaseIndex = reader.ReadInt32();
            break;
          case 424:
            dataManager.CurrentGameTime = reader.ReadSingle();
            break;
          case 425:
            dataManager.LastUsedSermonRitualDayIndex = resolver.GetFormatterWithVerify<int[]>().Deserialize(ref reader, options);
            break;
          case 426:
            dataManager.ScheduledActivityIndexes = resolver.GetFormatterWithVerify<int[]>().Deserialize(ref reader, options);
            break;
          case 427:
            dataManager.OverrideScheduledActivity = reader.ReadInt32();
            break;
          case 428:
            dataManager.InstantActivityIndexes = resolver.GetFormatterWithVerify<int[]>().Deserialize(ref reader, options);
            break;
          case 429:
            dataManager.PlayerEaten = reader.ReadBoolean();
            break;
          case 430:
            dataManager.PlayerEaten_COOP = reader.ReadBoolean();
            break;
          case 431:
            dataManager.ResurrectionType = resolver.GetFormatterWithVerify<ResurrectionType>().Deserialize(ref reader, options);
            break;
          case 432:
            dataManager.FirstTimeResurrecting = reader.ReadBoolean();
            break;
          case 433:
            dataManager.FirstTimeFertilizing = reader.ReadBoolean();
            break;
          case 434:
            dataManager.FirstTimeChop = reader.ReadBoolean();
            break;
          case 435:
            dataManager.FirstTimeMine = reader.ReadBoolean();
            break;
          case 436:
            dataManager.PlayersShagged = reader.ReadBoolean();
            break;
          case 439:
            dataManager.DungeonsCompletedWithFleeces = resolver.GetFormatterWithVerify<List<DataManager.DungeonCompletedFleeces>>().Deserialize(ref reader, options);
            break;
          case 440:
            dataManager.currentCategory = resolver.GetFormatterWithVerify<StructureBrain.Categories>().Deserialize(ref reader, options);
            break;
          case 441:
            dataManager.TimeSinceLastComplaint = reader.ReadSingle();
            break;
          case 442:
            dataManager.TimeSinceLastQuest = reader.ReadSingle();
            break;
          case 443:
            dataManager.DessentingFollowerChoiceQuestionIndex = reader.ReadInt32();
            break;
          case 444:
            dataManager.HaroConversationIndex = reader.ReadInt32();
            break;
          case 445:
            dataManager.SpecialHaroConversationIndex = reader.ReadInt32();
            break;
          case 446:
            dataManager.HaroSpecialEncounteredLocations = resolver.GetFormatterWithVerify<List<FollowerLocation>>().Deserialize(ref reader, options);
            break;
          case 447:
            dataManager.LeaderSpecialEncounteredLocations = resolver.GetFormatterWithVerify<List<FollowerLocation>>().Deserialize(ref reader, options);
            break;
          case 448:
            dataManager.SpokenToHaroD6 = reader.ReadBoolean();
            break;
          case 449:
            dataManager.HaroOnboardedWinter = reader.ReadBoolean();
            break;
          case 450:
            dataManager.HaroConversationCompleted = reader.ReadBoolean();
            break;
          case 451:
            dataManager.RatauKilled = reader.ReadBoolean();
            break;
          case 452:
            dataManager.RatauReadLetter = reader.ReadBoolean();
            break;
          case 453:
            dataManager.RatauIntroWoolhaven = reader.ReadBoolean();
            break;
          case 454:
            dataManager.RatauStaffQuestGameConvoPlayed = reader.ReadBoolean();
            break;
          case 455:
            dataManager.RatauStaffQuestWonGame = reader.ReadBoolean();
            break;
          case 456:
            dataManager.RatauStaffQuestAliveDead = reader.ReadBoolean();
            break;
          case 457:
            dataManager.CurrentFoxLocation = resolver.GetFormatterWithVerify<FollowerLocation>().Deserialize(ref reader, options);
            break;
          case 458:
            dataManager.CurrentFoxEncounter = reader.ReadInt32();
            break;
          case 459:
            dataManager.CurrentDLCFoxEncounter = reader.ReadInt32();
            break;
          case 460:
            dataManager.FoxIntroductions = resolver.GetFormatterWithVerify<List<FollowerLocation>>().Deserialize(ref reader, options);
            break;
          case 461:
            dataManager.FoxCompleted = resolver.GetFormatterWithVerify<List<FollowerLocation>>().Deserialize(ref reader, options);
            break;
          case 462:
            dataManager.PlimboStoryProgress = reader.ReadInt32();
            break;
          case 463:
            dataManager.RatooFishingProgress = reader.ReadInt32();
            break;
          case 464:
            dataManager.RatooFishing_FISH_CRAB = reader.ReadBoolean();
            break;
          case 465:
            dataManager.RatooFishing_FISH_LOBSTER = reader.ReadBoolean();
            break;
          case 466:
            dataManager.RatooFishing_FISH_OCTOPUS = reader.ReadBoolean();
            break;
          case 467:
            dataManager.RatooFishing_FISH_SQUID = reader.ReadBoolean();
            break;
          case 468:
            dataManager.RatooNeedsRescue = reader.ReadBoolean();
            break;
          case 469:
            dataManager.RatooRescued = reader.ReadBoolean();
            break;
          case 470:
            dataManager.PlayerHasFollowers = reader.ReadBoolean();
            break;
          case 471:
            dataManager.ShowSpecialHaroRoom = reader.ReadBoolean();
            break;
          case 472:
            dataManager.ShowSpecialMidasRoom = reader.ReadBoolean();
            break;
          case 473:
            dataManager.ShowSpecialPlimboRoom = reader.ReadBoolean();
            break;
          case 474:
            dataManager.ShowSpecialKlunkoRoom = reader.ReadBoolean();
            break;
          case 475:
            dataManager.ShowSpecialLeaderRoom = reader.ReadBoolean();
            break;
          case 476:
            dataManager.ShowSpecialFishermanRoom = reader.ReadBoolean();
            break;
          case 477:
            dataManager.ShowSpecialSozoRoom = reader.ReadBoolean();
            break;
          case 478:
            dataManager.ShowSpecialBaalAndAymRoom = reader.ReadBoolean();
            break;
          case 479:
            dataManager.ShowSpecialLighthouseKeeperRoom = reader.ReadBoolean();
            break;
          case 480:
            dataManager.SozoUnlockedMushroomSkin = reader.ReadBoolean();
            break;
          case 481:
            dataManager.SozoNoLongerBrainwashed = reader.ReadBoolean();
            break;
          case 482:
            dataManager.SozoMushroomRecruitedDay = reader.ReadInt32();
            break;
          case 483:
            dataManager.SozoAteMushroomDay = reader.ReadInt32();
            break;
          case 484:
            dataManager.SozoMushroomCount = reader.ReadInt32();
            break;
          case 485:
            dataManager.DrunkDay = reader.ReadInt32();
            break;
          case 486:
            dataManager.DrunkIncrement = reader.ReadInt32();
            break;
          case 487:
            dataManager.PoemIncrement = reader.ReadInt32();
            break;
          case 488:
            dataManager.FishCaughtTotal = reader.ReadInt32();
            break;
          case 489:
            dataManager.PlayerDeathDay = reader.ReadInt32();
            break;
          case 490:
            dataManager.DisciplesCreated = reader.ReadInt32();
            break;
          case 491:
            dataManager.LastDrumCircleTime = reader.ReadSingle();
            break;
          case 492:
            dataManager.HasMidasHiding = reader.ReadBoolean();
            break;
          case 493:
            dataManager.MidasFollowerInfo = resolver.GetFormatterWithVerify<FollowerInfo>().Deserialize(ref reader, options);
            break;
          case 494:
            dataManager.TimeSinceMidasStoleGold = reader.ReadSingle();
            break;
          case 495:
            dataManager.MidasHiddenDay = reader.ReadInt32();
            break;
          case 496:
            dataManager.CompletedMidasFollowerQuest = reader.ReadBoolean();
            break;
          case 497:
            dataManager.GivenMidasFollowerQuest = reader.ReadBoolean();
            break;
          case 498:
            dataManager.MidasStolenGold = resolver.GetFormatterWithVerify<List<InventoryItem>>().Deserialize(ref reader, options);
            break;
          case 499:
            dataManager.LastIceSculptureBuild = reader.ReadSingle();
            break;
          case 500:
            dataManager.LastAnimalLoverPet = reader.ReadSingle();
            break;
          case 501:
            dataManager.RatooGivenHeart = reader.ReadBoolean();
            break;
          case 502:
            dataManager.RatooMentionedWrongHeart = reader.ReadBoolean();
            break;
          case 503:
            dataManager.ShownInitialTempleDoorSeal = reader.ReadBoolean();
            break;
          case 504:
            dataManager.FirstFollowerSpawnInteraction = reader.ReadBoolean();
            break;
          case 505:
            dataManager.DecorationTypesBuilt = resolver.GetFormatterWithVerify<List<int>>().Deserialize(ref reader, options);
            break;
          case 506:
            dataManager.UnlockedClothing = resolver.GetFormatterWithVerify<List<FollowerClothingType>>().Deserialize(ref reader, options);
            break;
          case 507:
            dataManager.ClothingAssigned = resolver.GetFormatterWithVerify<List<FollowerClothingType>>().Deserialize(ref reader, options);
            break;
          case 508:
            dataManager.previouslyAssignedClothing = resolver.GetFormatterWithVerify<FollowerClothingType>().Deserialize(ref reader, options);
            break;
          case 509:
            dataManager.ClothingVariants = resolver.GetFormatterWithVerify<List<DataManager.ClothingVariant>>().Deserialize(ref reader, options);
            break;
          case 510:
            dataManager.WeaponSelectionPositions = resolver.GetFormatterWithVerify<List<TarotCards.Card>>().Deserialize(ref reader, options);
            break;
          case 511 /*0x01FF*/:
            dataManager.LoreStonesRoomUpTo = reader.ReadInt32();
            break;
          case 512 /*0x0200*/:
            dataManager.LoreStonesHaro = reader.ReadBoolean();
            break;
          case 513:
            dataManager.LoreStonesMoth = reader.ReadBoolean();
            break;
          case 514:
            dataManager.ShowCultFaith = reader.ReadBoolean();
            break;
          case 515:
            dataManager.ShowCultIllness = reader.ReadBoolean();
            break;
          case 516:
            dataManager.ShowCultHunger = reader.ReadBoolean();
            break;
          case 517:
            dataManager.ShowCultWarmth = reader.ReadBoolean();
            break;
          case 518:
            dataManager.ShowLoyaltyBars = reader.ReadBoolean();
            break;
          case 519:
            dataManager.SandboxModeEnabled = reader.ReadBoolean();
            break;
          case 520:
            dataManager.SpawnPubResources = reader.ReadBoolean();
            break;
          case 521:
            dataManager.EnteredHopRoom = reader.ReadBoolean();
            break;
          case 522:
            dataManager.EnteredGrapeRoom = reader.ReadBoolean();
            break;
          case 523:
            dataManager.EnteredCottonRoom = reader.ReadBoolean();
            break;
          case 524:
            dataManager.IntroDoor1 = reader.ReadBoolean();
            break;
          case 525:
            dataManager.FirstDoctrineStone = reader.ReadBoolean();
            break;
          case 526:
            dataManager.InitialDoctrineStone = reader.ReadBoolean();
            break;
          case 527:
            dataManager.ShowHaroDoctrineStoneRoom = reader.ReadBoolean();
            break;
          case 528:
            dataManager.HaroIntroduceDoctrines = reader.ReadBoolean();
            break;
          case 529:
            dataManager.RatExplainDungeon = reader.ReadBoolean();
            break;
          case 530:
            dataManager.RatauToGiveCurseNextRun = reader.ReadBoolean();
            break;
          case 531:
            dataManager.SozoStoryProgress = reader.ReadInt32();
            break;
          case 532:
            dataManager.MidasBankUnlocked = reader.ReadBoolean();
            break;
          case 533:
            dataManager.MidasBankIntro = reader.ReadBoolean();
            break;
          case 534:
            dataManager.MidasSacrificeIntro = reader.ReadBoolean();
            break;
          case 535:
            dataManager.MidasIntro = reader.ReadBoolean();
            break;
          case 536:
            dataManager.MidasDevotionIntro = reader.ReadBoolean();
            break;
          case 537:
            dataManager.MidasStatue = reader.ReadBoolean();
            break;
          case 538:
            dataManager.MidasDevotionCost = reader.ReadSingle();
            break;
          case 539:
            dataManager.MidasDevotionLastUsed = reader.ReadInt32();
            break;
          case 540:
            dataManager.MidasFollowerStatueCount = reader.ReadInt32();
            break;
          case 541:
            dataManager.RatauShowShrineShop = reader.ReadBoolean();
            break;
          case 542:
            dataManager.DecorationRoomFirstConvo = reader.ReadBoolean();
            break;
          case 543:
            dataManager.FirstTarot = reader.ReadBoolean();
            break;
          case 544:
            dataManager.Tutorial_Night = reader.ReadBoolean();
            break;
          case 545:
            dataManager.Tutorial_ReturnToDungeon = reader.ReadBoolean();
            break;
          case 546:
            dataManager.FirstTimeInDungeon = reader.ReadBoolean();
            break;
          case 547:
            dataManager.AllowBuilding = reader.ReadBoolean();
            break;
          case 548:
            dataManager.CookedFirstFood = reader.ReadBoolean();
            break;
          case 549:
            dataManager.Dungeon1Story1 = reader.ReadBoolean();
            break;
          case 550:
            dataManager.Dungeon1Story2 = reader.ReadBoolean();
            break;
          case 551:
            dataManager.FirstFollowerRescue = reader.ReadBoolean();
            break;
          case 552:
            dataManager.FirstDungeon1RescueRoom = reader.ReadBoolean();
            break;
          case 553:
            dataManager.FirstDungeon2RescueRoom = reader.ReadBoolean();
            break;
          case 554:
            dataManager.FirstDungeon3RescueRoom = reader.ReadBoolean();
            break;
          case 555:
            dataManager.FirstDungeon4RescueRoom = reader.ReadBoolean();
            break;
          case 556:
            dataManager.SherpaFirstConvo = reader.ReadBoolean();
            break;
          case 557:
            dataManager.ResourceRoom1Revealed = reader.ReadBoolean();
            break;
          case 558:
            dataManager.EncounteredHealingRoom = reader.ReadBoolean();
            break;
          case 559:
            dataManager.MinimumRandomRoomsEncountered = reader.ReadBoolean();
            break;
          case 560:
            dataManager.MinimumRandomRoomsEncounteredAmount = reader.ReadInt32();
            break;
          case 561:
            dataManager.ForneusLore = reader.ReadBoolean();
            break;
          case 562:
            dataManager.SozoBeforeDeath = reader.ReadBoolean();
            break;
          case 563:
            dataManager.SozoDead = reader.ReadBoolean();
            break;
          case 564:
            dataManager.SozoTakenMushroom = reader.ReadBoolean();
            break;
          case 565:
            dataManager.FirstTimeWeaponMarketplace = reader.ReadBoolean();
            break;
          case 566:
            dataManager.FirstTimeSpiderMarketplace = reader.ReadBoolean();
            break;
          case 567:
            dataManager.FirstTimeSeedMarketplace = reader.ReadBoolean();
            break;
          case 568:
            dataManager.ShowFirstDoctrineStone = reader.ReadBoolean();
            break;
          case 569:
            dataManager.RatauGiftMediumCollected = reader.ReadBoolean();
            break;
          case 570:
            dataManager.CompletedLighthouseCrystalQuest = reader.ReadBoolean();
            break;
          case 571:
            dataManager.CameFromDeathCatFight = reader.ReadBoolean();
            break;
          case 572:
            dataManager.OldFollowerSpoken = reader.ReadBoolean();
            break;
          case 573:
            dataManager.InjuredFollowerSpoken = reader.ReadBoolean();
            break;
          case 574:
            dataManager.CanUnlockRelics = reader.ReadBoolean();
            break;
          case 575:
            dataManager.FoundRelicAtHubShore = reader.ReadBoolean();
            break;
          case 576:
            dataManager.FoundRelicInFish = reader.ReadBoolean();
            break;
          case 577:
            dataManager.GivenRelicFishRiddle = reader.ReadBoolean();
            break;
          case 578:
            dataManager.GivenRelicLighthouseRiddle = reader.ReadBoolean();
            break;
          case 579:
            dataManager.ForceMarketplaceCat = reader.ReadBoolean();
            break;
          case 580:
            dataManager.HadInitialMatingTentInteraction = reader.ReadBoolean();
            break;
          case 581:
            dataManager.ShowMidasKilling = reader.ReadBoolean();
            break;
          case 582:
            dataManager.GivenMidasSkull = reader.ReadBoolean();
            break;
          case 583:
            dataManager.CompletedYngyaFightIntro = reader.ReadBoolean();
            break;
          case 584:
            dataManager.RecruitedRotFollower = reader.ReadBoolean();
            break;
          case 585:
            dataManager.HasAcceptedPilgrimPart1 = reader.ReadBoolean();
            break;
          case 586:
            dataManager.PilgrimPart1TargetDay = reader.ReadInt32();
            break;
          case 587:
            dataManager.HasAcceptedPilgrimPart2 = reader.ReadBoolean();
            break;
          case 588:
            dataManager.PilgrimPart2TargetDay = reader.ReadInt32();
            break;
          case 589:
            dataManager.HasAcceptedPilgrimPart3 = reader.ReadBoolean();
            break;
          case 590:
            dataManager.PilgrimPart3TargetDay = reader.ReadInt32();
            break;
          case 591:
            dataManager.IsPilgrimRescue = reader.ReadBoolean();
            break;
          case 592:
            dataManager.IsJalalaBag = reader.ReadBoolean();
            break;
          case 593:
            dataManager.FoundJalalaBag = reader.ReadBoolean();
            break;
          case 594:
            dataManager.GivenRinorLine = reader.ReadBoolean();
            break;
          case 595:
            dataManager.GivenYarlenLine = reader.ReadBoolean();
            break;
          case 596:
            dataManager.PilgrimTargetLocation = resolver.GetFormatterWithVerify<FollowerLocation>().Deserialize(ref reader, options);
            break;
          case 597:
            dataManager.CultLeader1_LastRun = reader.ReadInt32();
            break;
          case 598:
            dataManager.CultLeader1_StoryPosition = reader.ReadInt32();
            break;
          case 599:
            dataManager.CultLeader2_LastRun = reader.ReadInt32();
            break;
          case 600:
            dataManager.CultLeader2_StoryPosition = reader.ReadInt32();
            break;
          case 601:
            dataManager.CultLeader3_LastRun = reader.ReadInt32();
            break;
          case 602:
            dataManager.CultLeader3_StoryPosition = reader.ReadInt32();
            break;
          case 603:
            dataManager.CultLeader4_LastRun = reader.ReadInt32();
            break;
          case 604:
            dataManager.CultLeader4_StoryPosition = reader.ReadInt32();
            break;
          case 605:
            dataManager.CultLeader5_LastRun = reader.ReadInt32();
            break;
          case 606:
            dataManager.CultLeader5_StoryPosition = reader.ReadInt32();
            break;
          case 607:
            dataManager.CultLeader6_LastRun = reader.ReadInt32();
            break;
          case 608:
            dataManager.CultLeader6_StoryPosition = reader.ReadInt32();
            break;
          case 609:
            dataManager.BeatenDungeon5 = reader.ReadBoolean();
            break;
          case 610:
            dataManager.BeatenDungeon6 = reader.ReadBoolean();
            break;
          case 611:
            dataManager.DeathCatConversationLastRun = reader.ReadInt32();
            break;
          case 612:
            dataManager.DeathCatStory = reader.ReadInt32();
            break;
          case 613:
            dataManager.DeathCatDead = reader.ReadInt32();
            break;
          case 614:
            dataManager.DeathCatWon = reader.ReadInt32();
            break;
          case 615:
            dataManager.DeathCatBoss1 = reader.ReadBoolean();
            break;
          case 616:
            dataManager.DeathCatBoss2 = reader.ReadBoolean();
            break;
          case 617:
            dataManager.DeathCatBoss3 = reader.ReadBoolean();
            break;
          case 618:
            dataManager.DeathCatBoss4 = reader.ReadBoolean();
            break;
          case 619:
            dataManager.DeathCatRatauKilled = reader.ReadBoolean();
            break;
          case 620:
            dataManager.DungeonKeyRoomCompleted1 = reader.ReadBoolean();
            break;
          case 621:
            dataManager.DungeonKeyRoomCompleted2 = reader.ReadBoolean();
            break;
          case 622:
            dataManager.DungeonKeyRoomCompleted3 = reader.ReadBoolean();
            break;
          case 623:
            dataManager.DungeonKeyRoomCompleted4 = reader.ReadBoolean();
            break;
          case 624:
            dataManager.LambTownLevel = reader.ReadInt32();
            break;
          case 625:
            dataManager.LambTownWoolGiven = reader.ReadInt32();
            break;
          case 626:
            dataManager.RatOutpostIntro = reader.ReadBoolean();
            break;
          case 627:
            dataManager.FirstMonsterHeart = reader.ReadBoolean();
            break;
          case 628:
            dataManager.Rat_Tutorial_Bell = reader.ReadBoolean();
            break;
          case 629:
            dataManager.Goat_First_Meeting = reader.ReadBoolean();
            break;
          case 630:
            dataManager.Goat_Guardian_Door_Open = reader.ReadBoolean();
            break;
          case 631:
            dataManager.Key_Shrine_1 = reader.ReadBoolean();
            break;
          case 632:
            dataManager.Key_Shrine_2 = reader.ReadBoolean();
            break;
          case 633:
            dataManager.Key_Shrine_3 = reader.ReadBoolean();
            break;
          case 634:
            dataManager.InTutorial = reader.ReadBoolean();
            break;
          case 635:
            dataManager.UnlockBaseTeleporter = reader.ReadBoolean();
            break;
          case 636:
            dataManager.Tutorial_First_Indoctoring = reader.ReadBoolean();
            break;
          case 637:
            dataManager.Tutorial_Second_Enter_Base = reader.ReadBoolean();
            break;
          case 638:
            dataManager.Tutorial_Rooms_Completed = reader.ReadBoolean();
            break;
          case 639:
            dataManager.Tutorial_Enable_Store_Resources = reader.ReadBoolean();
            break;
          case 640:
            dataManager.Tutorial_Completed = reader.ReadBoolean();
            break;
          case 641:
            dataManager.Tutorial_Mission_Board = reader.ReadBoolean();
            break;
          case 642:
            dataManager.Create_Tutorial_Rooms = reader.ReadBoolean();
            break;
          case 643:
            dataManager.RatauExplainsFollowers = reader.ReadBoolean();
            break;
          case 644:
            dataManager.RatauExplainsDemo = reader.ReadBoolean();
            break;
          case 645:
            dataManager.RatauExplainsBiome0 = reader.ReadBoolean();
            break;
          case 646:
            dataManager.RatauExplainsBiome1 = reader.ReadBoolean();
            break;
          case 647:
            dataManager.RatauExplainsBiome0Boss = reader.ReadBoolean();
            break;
          case 648:
            dataManager.RatauExplainsTeleporter = reader.ReadBoolean();
            break;
          case 649:
            dataManager.SozoIntro = reader.ReadBoolean();
            break;
          case 650:
            dataManager.SozoDecorationQuestActive = reader.ReadBoolean();
            break;
          case 651:
            dataManager.SozoQuestComplete = reader.ReadBoolean();
            break;
          case 652:
            dataManager.CollectedMenticide = reader.ReadBoolean();
            break;
          case 653:
            dataManager.TarotIntro = reader.ReadBoolean();
            break;
          case 654:
            dataManager.HasTarotBuilding = reader.ReadBoolean();
            break;
          case 655:
            dataManager.ForestOfferingRoomCompleted = reader.ReadBoolean();
            break;
          case 656:
            dataManager.KnucklebonesIntroCompleted = reader.ReadBoolean();
            break;
          case 657:
            dataManager.KnucklebonesFirstGameRatauStart = reader.ReadBoolean();
            break;
          case 658:
            dataManager.ForestChallengeRoom1Completed = reader.ReadBoolean();
            break;
          case 659:
            dataManager.ForestRescueWorshipper = reader.ReadBoolean();
            break;
          case 660:
            dataManager.GetFirstFollower = reader.ReadBoolean();
            break;
          case 661:
            dataManager.BeatenFirstMiniBoss = reader.ReadBoolean();
            break;
          case 662:
            dataManager.RatauExplainBuilding = reader.ReadBoolean();
            break;
          case 663:
            dataManager.PromoteFollowerExplained = reader.ReadBoolean();
            break;
          case 664:
            dataManager.HasMadeFirstOffering = reader.ReadBoolean();
            break;
          case 665:
            dataManager.BirdConvo = reader.ReadBoolean();
            break;
          case 666:
            dataManager.UnlockedHubShore = reader.ReadBoolean();
            break;
          case 667:
            dataManager.GivenFollowerGift = reader.ReadBoolean();
            break;
          case 668:
            dataManager.FinalBossSlowWalk = reader.ReadBoolean();
            break;
          case 669:
            dataManager.HadNecklaceOnRun = reader.ReadInt32();
            break;
          case 670:
            dataManager.HasPerformedPleasureRitual = reader.ReadBoolean();
            break;
          case 671:
            dataManager.ViolentExtremistFirstTime = reader.ReadBoolean();
            break;
          case 672:
            dataManager.FollowersPlayedKnucklebonesToday = resolver.GetFormatterWithVerify<List<int>>().Deserialize(ref reader, options);
            break;
          case 673:
            dataManager.ShownDungeon1FinalLeaderEncounter = reader.ReadBoolean();
            break;
          case 674:
            dataManager.ShownDungeon2FinalLeaderEncounter = reader.ReadBoolean();
            break;
          case 675:
            dataManager.ShownDungeon3FinalLeaderEncounter = reader.ReadBoolean();
            break;
          case 676:
            dataManager.ShownDungeon4FinalLeaderEncounter = reader.ReadBoolean();
            break;
          case 677:
            dataManager.HaroOnbardedHarderDungeon1 = reader.ReadBoolean();
            break;
          case 678:
            dataManager.HaroOnbardedHarderDungeon2 = reader.ReadBoolean();
            break;
          case 679:
            dataManager.HaroOnbardedHarderDungeon3 = reader.ReadBoolean();
            break;
          case 680:
            dataManager.HaroOnbardedHarderDungeon4 = reader.ReadBoolean();
            break;
          case 681:
            dataManager.HaroOnbardedDungeon6 = reader.ReadBoolean();
            break;
          case 682:
            dataManager.HaroOnbardedHarderDungeon1_PostGame = reader.ReadBoolean();
            break;
          case 683:
            dataManager.HaroOnbardedHarderDungeon2_PostGame = reader.ReadBoolean();
            break;
          case 684:
            dataManager.HaroOnbardedHarderDungeon3_PostGame = reader.ReadBoolean();
            break;
          case 685:
            dataManager.HaroOnbardedHarderDungeon4_PostGame = reader.ReadBoolean();
            break;
          case 686:
            dataManager.RevealOfferingChest = reader.ReadBoolean();
            break;
          case 687:
            dataManager.OnboardedOfferingChest = reader.ReadBoolean();
            break;
          case 688:
            dataManager.OnboardedHomeless = reader.ReadBoolean();
            break;
          case 689:
            dataManager.OnboardedHomelessAtNight = reader.ReadBoolean();
            break;
          case 690:
            dataManager.OnboardedEndlessMode = reader.ReadBoolean();
            break;
          case 691:
            dataManager.OnboardedDeadFollower = reader.ReadBoolean();
            break;
          case 692:
            dataManager.OnboardedBuildingHouse = reader.ReadBoolean();
            break;
          case 693:
            dataManager.OnboardedMakingMoreFood = reader.ReadBoolean();
            break;
          case 694:
            dataManager.OnboardedCleaningBase = reader.ReadBoolean();
            break;
          case 695:
            dataManager.OnboardedOldFollower = reader.ReadBoolean();
            break;
          case 696:
            dataManager.OnboardedSickFollower = reader.ReadBoolean();
            break;
          case 697:
            dataManager.OnboardedStarvingFollower = reader.ReadBoolean();
            break;
          case 698:
            dataManager.OnboardedDissenter = reader.ReadBoolean();
            break;
          case 699:
            dataManager.OnboardedFaithOfFlock = reader.ReadBoolean();
            break;
          case 700:
            dataManager.OnboardedRaiseFaith = reader.ReadBoolean();
            break;
          case 701:
            dataManager.OnboardedResourceYard = reader.ReadBoolean();
            break;
          case 702:
            dataManager.OnboardedCrisisOfFaith = reader.ReadBoolean();
            break;
          case 703:
            dataManager.OnboardedHalloween = reader.ReadBoolean();
            break;
          case 704:
            dataManager.OnboardedSermon = reader.ReadBoolean();
            break;
          case 705:
            dataManager.OnboardedBuildFarm = reader.ReadBoolean();
            break;
          case 706:
            dataManager.OnboardedRefinery = reader.ReadBoolean();
            break;
          case 707:
            dataManager.OnboardedCultName = reader.ReadBoolean();
            break;
          case 708:
            dataManager.OnboardedZombie = reader.ReadBoolean();
            break;
          case 709:
            dataManager.OnboardedLoyalty = reader.ReadBoolean();
            break;
          case 710:
            dataManager.OnboardedGodTear = reader.ReadBoolean();
            break;
          case 711:
            dataManager.OnboardedMysticShop = reader.ReadBoolean();
            break;
          case 712:
            dataManager.ForeshadowedMysticShop = reader.ReadBoolean();
            break;
          case 713:
            dataManager.ForeshadowedWolf = reader.ReadBoolean();
            break;
          case 714:
            dataManager.OnboardedLayer2 = reader.ReadBoolean();
            break;
          case 715:
            dataManager.OnboardedRelics = reader.ReadBoolean();
            break;
          case 716:
            dataManager.HasMetChefShop = reader.ReadBoolean();
            break;
          case 717:
            dataManager.CurrentOnboardingFollowerID = reader.ReadInt32();
            break;
          case 718:
            dataManager.CurrentOnboardingFollowerType = reader.ReadInt32();
            break;
          case 719:
            dataManager.CurrentOnboardingFollowerTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 720:
            dataManager.HasPerformedRitual = reader.ReadBoolean();
            break;
          case 721:
            dataManager.DeathCatBaalAndAymSecret = reader.ReadBoolean();
            break;
          case 722:
            dataManager.ShamuraBaalAndAymSecret = reader.ReadBoolean();
            break;
          case 723:
            dataManager.CanFindLeaderRelic = reader.ReadBoolean();
            break;
          case 724:
            dataManager.OnboardedDisciple = reader.ReadBoolean();
            break;
          case 725:
            dataManager.OnboardedPleasure = reader.ReadBoolean();
            break;
          case 726:
            dataManager.OnboardedFollowerPleasure = reader.ReadBoolean();
            break;
          case 727:
            dataManager.SecretItemsGivenToFollower = resolver.GetFormatterWithVerify<List<FollowerLocation>>().Deserialize(ref reader, options);
            break;
          case 728:
            dataManager.OnboardedDepositFollowerNPC = reader.ReadBoolean();
            break;
          case 729:
            dataManager.DepositFinalFollowerNPC = reader.ReadBoolean();
            break;
          case 730:
            dataManager.TalkedToInfectedNPC = reader.ReadBoolean();
            break;
          case 731:
            dataManager.CompletedInfectedNPCQuest = reader.ReadBoolean();
            break;
          case 732:
            dataManager.DepositFollowerTargetTraits = resolver.GetFormatterWithVerify<List<FollowerTrait.TraitType>>().Deserialize(ref reader, options);
            break;
          case 733:
            dataManager.DepositedFollowerRewardsClaimed = reader.ReadInt32();
            break;
          case 734:
            dataManager.DepositedWitnessEyesForRelics = reader.ReadInt32();
            break;
          case 735:
            dataManager.GaveLeshyHealingQuest = reader.ReadBoolean();
            break;
          case 736:
            dataManager.GaveHeketHealingQuest = reader.ReadBoolean();
            break;
          case 737:
            dataManager.GaveKallamarHealingQuest = reader.ReadBoolean();
            break;
          case 738:
            dataManager.GaveShamuraHealingQuest = reader.ReadBoolean();
            break;
          case 739:
            dataManager.LeshyHealQuestCompleted = reader.ReadBoolean();
            break;
          case 740:
            dataManager.HeketHealQuestCompleted = reader.ReadBoolean();
            break;
          case 741:
            dataManager.KallamarHealQuestCompleted = reader.ReadBoolean();
            break;
          case 742:
            dataManager.ShamuraHealQuestCompleted = reader.ReadBoolean();
            break;
          case 743:
            dataManager.LeshyHealed = reader.ReadBoolean();
            break;
          case 744:
            dataManager.HeketHealed = reader.ReadBoolean();
            break;
          case 745:
            dataManager.KallamarHealed = reader.ReadBoolean();
            break;
          case 746:
            dataManager.ShamuraHealed = reader.ReadBoolean();
            break;
          case 747:
            dataManager.HealingLeshyQuestActive = reader.ReadBoolean();
            break;
          case 748:
            dataManager.HealingHeketQuestActive = reader.ReadBoolean();
            break;
          case 749:
            dataManager.HealingKallamarQuestActive = reader.ReadBoolean();
            break;
          case 750:
            dataManager.HealingShamuraQuestActive = reader.ReadBoolean();
            break;
          case 751:
            dataManager.HealingQuestDay = reader.ReadInt32();
            break;
          case 752:
            dataManager.LeshyHealingQuestDay = reader.ReadInt32();
            break;
          case 753:
            dataManager.HeketHealingQuestDay = reader.ReadInt32();
            break;
          case 754:
            dataManager.KallamarHealingQuestDay = reader.ReadInt32();
            break;
          case 755:
            dataManager.ShamuraHealingQuestDay = reader.ReadInt32();
            break;
          case 756:
            dataManager.BeatenWitnessDungeon1 = reader.ReadBoolean();
            break;
          case 757:
            dataManager.BeatenWitnessDungeon2 = reader.ReadBoolean();
            break;
          case 758:
            dataManager.BeatenWitnessDungeon3 = reader.ReadBoolean();
            break;
          case 759:
            dataManager.BeatenWitnessDungeon4 = reader.ReadBoolean();
            break;
          case 760:
            dataManager.MysticKeeperBeatenLeshy = reader.ReadBoolean();
            break;
          case 761:
            dataManager.MysticKeeperBeatenHeket = reader.ReadBoolean();
            break;
          case 762:
            dataManager.MysticKeeperBeatenKallamar = reader.ReadBoolean();
            break;
          case 763:
            dataManager.MysticKeeperBeatenShamura = reader.ReadBoolean();
            break;
          case 764:
            dataManager.MysticKeeperBeatenAll = reader.ReadBoolean();
            break;
          case 765:
            dataManager.MysticKeeperFirstPurchase = reader.ReadBoolean();
            break;
          case 766:
            dataManager.MysticKeeperOnboardedSin = reader.ReadBoolean();
            break;
          case 767 /*0x02FF*/:
            dataManager.ChemachOnboardedSin = reader.ReadBoolean();
            break;
          case 768 /*0x0300*/:
            dataManager.KlunkoOnboardedTailor1 = reader.ReadBoolean();
            break;
          case 769:
            dataManager.KlunkoOnboardedTailor2 = reader.ReadBoolean();
            break;
          case 770:
            dataManager.AssignedFollowersOutfits = reader.ReadBoolean();
            break;
          case 771:
            dataManager.BeatenPostGame = reader.ReadBoolean();
            break;
          case 772:
            dataManager.GivenLoyaltyQuestDay = reader.ReadInt32();
            break;
          case 773:
            dataManager.LastDaySincePlayerUpgrade = reader.ReadInt32();
            break;
          case 774:
            dataManager.MealsCooked = reader.ReadInt32();
            break;
          case 775:
            dataManager.DrinksBrewed = reader.ReadInt32();
            break;
          case 776:
            dataManager.TalismanPiecesReceivedFromMysticShop = reader.ReadInt32();
            break;
          case 777:
            dataManager.MysticShopUsed = reader.ReadBoolean();
            break;
          case 778:
            dataManager.CrystalDoctrinesReceivedFromMysticShop = reader.ReadInt32();
            break;
          case 779:
            dataManager.PreviousMysticShopItem = resolver.GetFormatterWithVerify<InventoryItem.ITEM_TYPE>().Deserialize(ref reader, options);
            break;
          case 780:
            dataManager.OnboardedCrystalDoctrine = reader.ReadBoolean();
            break;
          case 781:
            dataManager.RanchingAnimalsAdded = reader.ReadInt32();
            break;
          case 782:
            dataManager.FollowersTrappedInToxicWaste = reader.ReadInt32();
            break;
          case 783:
            dataManager.OnboardedWool = reader.ReadBoolean();
            break;
          case 784:
            dataManager.Dungeon1_1_Key = reader.ReadBoolean();
            break;
          case 785:
            dataManager.Dungeon1_2_Key = reader.ReadBoolean();
            break;
          case 786:
            dataManager.Dungeon1_3_Key = reader.ReadBoolean();
            break;
          case 787:
            dataManager.Dungeon1_4_Key = reader.ReadBoolean();
            break;
          case 788:
            dataManager.Dungeon2_1_Key = reader.ReadBoolean();
            break;
          case 789:
            dataManager.Dungeon2_2_Key = reader.ReadBoolean();
            break;
          case 790:
            dataManager.Dungeon2_3_Key = reader.ReadBoolean();
            break;
          case 791:
            dataManager.Dungeon2_4_Key = reader.ReadBoolean();
            break;
          case 792:
            dataManager.Dungeon3_1_Key = reader.ReadBoolean();
            break;
          case 793:
            dataManager.Dungeon3_2_Key = reader.ReadBoolean();
            break;
          case 794:
            dataManager.Dungeon3_3_Key = reader.ReadBoolean();
            break;
          case 795:
            dataManager.Dungeon3_4_Key = reader.ReadBoolean();
            break;
          case 796:
            dataManager.HadFirstTempleKey = reader.ReadBoolean();
            break;
          case 797:
            dataManager.CurrentKeyPieces = reader.ReadInt32();
            break;
          case 798:
            dataManager.GivenFreeDungeonFollower = reader.ReadBoolean();
            break;
          case 799:
            dataManager.GivenFreeDungeonGold = reader.ReadBoolean();
            break;
          case 800:
            dataManager.FoxMeeting_0 = reader.ReadBoolean();
            break;
          case 801:
            dataManager.GaveFollowerToFox = reader.ReadBoolean();
            break;
          case 802:
            dataManager.Ritual_0 = reader.ReadBoolean();
            break;
          case 803:
            dataManager.Ritual_1 = reader.ReadBoolean();
            break;
          case 804:
            dataManager.SnowmenCreated = reader.ReadInt32();
            break;
          case 805:
            dataManager.Lighthouse_FirstConvo = reader.ReadBoolean();
            break;
          case 806:
            dataManager.Lighthouse_LitFirstConvo = reader.ReadBoolean();
            break;
          case 807:
            dataManager.Lighthouse_FireOutAgain = reader.ReadBoolean();
            break;
          case 808:
            dataManager.Lighthouse_QuestGiven = reader.ReadBoolean();
            break;
          case 809:
            dataManager.Lighthouse_QuestComplete = reader.ReadBoolean();
            break;
          case 810:
            dataManager.LighthouseFuel = reader.ReadInt32();
            break;
          case 811:
            dataManager.Lighthouse_Lit = reader.ReadBoolean();
            break;
          case 812:
            dataManager.ShoreFishFirstConvo = reader.ReadBoolean();
            break;
          case 813:
            dataManager.ShoreFishFished = reader.ReadBoolean();
            break;
          case 814:
            dataManager.ShoreTarotShotConvo1 = reader.ReadBoolean();
            break;
          case 815:
            dataManager.ShoreTarotShotConvo2 = reader.ReadBoolean();
            break;
          case 816:
            dataManager.ShoreFlowerShopConvo1 = reader.ReadBoolean();
            break;
          case 817:
            dataManager.SozoFlowerShopConvo1 = reader.ReadBoolean();
            break;
          case 818:
            dataManager.SozoTarotShopConvo1 = reader.ReadBoolean();
            break;
          case 819:
            dataManager.ForcingPlayerWeaponDLC = resolver.GetFormatterWithVerify<EquipmentType>().Deserialize(ref reader, options);
            break;
          case 820:
            dataManager.RatauFoundSkin = reader.ReadBoolean();
            break;
          case 821:
            dataManager.MidasFoundSkin = reader.ReadBoolean();
            break;
          case 822:
            dataManager.SozoFoundDecoration = reader.ReadBoolean();
            break;
          case 823:
            dataManager.MidasTotalGoldStolen = reader.ReadInt32();
            break;
          case 824:
            dataManager.MidasSpecialEncounter = reader.ReadInt32();
            break;
          case 825:
            dataManager.MidasSpecialEncounteredLocations = resolver.GetFormatterWithVerify<List<FollowerLocation>>().Deserialize(ref reader, options);
            break;
          case 826:
            dataManager.MidasBeaten = reader.ReadBoolean();
            break;
          case 827:
            dataManager.PlimboSpecialEncountered = reader.ReadBoolean();
            break;
          case 828:
            dataManager.KlunkoSpecialEncountered = reader.ReadBoolean();
            break;
          case 829:
            dataManager.FishermanSpecialEncountered = reader.ReadBoolean();
            break;
          case 830:
            dataManager.BaalAndAymSpecialEncounterd = reader.ReadBoolean();
            break;
          case 831:
            dataManager.LighthouseKeeperSpecialEncountered = reader.ReadBoolean();
            break;
          case 832:
            dataManager.SozoSpecialEncountered = reader.ReadBoolean();
            break;
          case 833:
            dataManager.OpenedDoorTimestamp = reader.ReadSingle();
            break;
          case 834:
            dataManager.SeedMarketPlacePostGame = reader.ReadBoolean();
            break;
          case 835:
            dataManager.HelobPostGame = reader.ReadBoolean();
            break;
          case 836:
            dataManager.HorseTown_PaidRespectToHorse = reader.ReadBoolean();
            break;
          case 837:
            dataManager.HorseTown_JoinCult = reader.ReadBoolean();
            break;
          case 838:
            dataManager.HorseTown_OpenedChest = reader.ReadBoolean();
            break;
          case 839:
            dataManager.BlackSoulsEnabled = reader.ReadBoolean();
            break;
          case 840:
            dataManager.PlacedRubble = reader.ReadBoolean();
            break;
          case 841:
            dataManager.DefeatedExecutioner = reader.ReadBoolean();
            break;
          case 842:
            dataManager.BeatenWolf = reader.ReadBoolean();
            break;
          case 843:
            dataManager.BeatenYngya = reader.ReadBoolean();
            break;
          case 844:
            dataManager.BeatenExecutioner = reader.ReadBoolean();
            break;
          case 845:
            dataManager.RevealedPostDLC = reader.ReadBoolean();
            break;
          case 846:
            dataManager.HadFinalYngyaRoomConvo = reader.ReadBoolean();
            break;
          case 847:
            dataManager.HasYngyaMatingQuestAccepted = reader.ReadBoolean();
            break;
          case 848:
            dataManager.HasYngyaFirePitRitualQuestAccepted = reader.ReadBoolean();
            break;
          case 849:
            dataManager.HasYngyaFlowerBasketQuestAccepted = reader.ReadBoolean();
            break;
          case 850:
            dataManager.HasFinishedYngyaFlowerBasketQuest = reader.ReadBoolean();
            break;
          case 851:
            dataManager.HasAnimalFeedPoopQuest0Accepted = reader.ReadBoolean();
            break;
          case 852:
            dataManager.HasAnimalFeedPoopQuest1Accepted = reader.ReadBoolean();
            break;
          case 853:
            dataManager.HasAnimalFeedPoopQuest2Accepted = reader.ReadBoolean();
            break;
          case 854:
            dataManager.HasWalkPoopedAnimalQuestAccepted = reader.ReadBoolean();
            break;
          case 855:
            dataManager.HasAnimalFeedMeatQuest0Accepted = reader.ReadBoolean();
            break;
          case 856:
            dataManager.HasAnimalFeedMeatQuest1Accepted = reader.ReadBoolean();
            break;
          case 857:
            dataManager.HasAnimalFeedMeatQuest2Accepted = reader.ReadBoolean();
            break;
          case 858:
            dataManager.HasBuildGoodSnowmanQuestAccepted = reader.ReadBoolean();
            break;
          case 859:
            dataManager.HasLifeToTheIceRitualQuestAccepted = reader.ReadBoolean();
            break;
          case 860:
            dataManager.ExecutionerFollowerNoteGiverID = reader.ReadInt32();
            break;
          case 861:
            dataManager.GiveExecutionerFollower = reader.ReadBoolean();
            break;
          case 862:
            dataManager.GivenExecutionerFollower = reader.ReadBoolean();
            break;
          case 863:
            dataManager.ExecutionerRoomRequiresRevealing = reader.ReadBoolean();
            break;
          case 864:
            dataManager.ExecutionerRoomRevealed = reader.ReadBoolean();
            break;
          case 865:
            dataManager.ExecutionerRoomRevealedThisRun = reader.ReadBoolean();
            break;
          case 866:
            dataManager.ExecutionerRoomUnlocked = reader.ReadBoolean();
            break;
          case 867:
            dataManager.ExecutionerDefeated = reader.ReadBoolean();
            break;
          case 868:
            dataManager.ExecutionerPardoned = reader.ReadBoolean();
            break;
          case 869:
            dataManager.ExecutionerDamned = reader.ReadBoolean();
            break;
          case 870:
            dataManager.ExecutionerSpokenToPlimbo = reader.ReadBoolean();
            break;
          case 871:
            dataManager.ExecutionerReceivedPlimbosHelp = reader.ReadBoolean();
            break;
          case 872:
            dataManager.ExecutionerSpokenToMidas = reader.ReadBoolean();
            break;
          case 873:
            dataManager.ExecutionerReceivedMidasHelp = reader.ReadBoolean();
            break;
          case 874:
            dataManager.ExecutionerFindNoteInSilkCradle = reader.ReadBoolean();
            break;
          case 875:
            dataManager.ExecutionerPurchases = reader.ReadInt32();
            break;
          case 876:
            dataManager.RuinedGraveyards = resolver.GetFormatterWithVerify<List<int>>().Deserialize(ref reader, options);
            break;
          case 877:
            dataManager.ExecutionerPardonedDay = reader.ReadInt32();
            break;
          case 878:
            dataManager.ExecutionerInWoolhavenDay = reader.ReadInt32();
            break;
          case 879:
            dataManager.ExecutionerWoolhavenExecuted = reader.ReadBoolean();
            break;
          case 880:
            dataManager.ExecutionerWoolhavenSaved = reader.ReadBoolean();
            break;
          case 881:
            dataManager.ExecutionerGivenWeaponFragment = reader.ReadBoolean();
            break;
          case 882:
            dataManager.WorldMapCurrentSelection = reader.ReadInt32();
            break;
          case 883:
            dataManager.HasBaalSkin = reader.ReadBoolean();
            break;
          case 884:
            dataManager.HasReturnedBaal = reader.ReadBoolean();
            break;
          case 885:
            dataManager.HasAymSkin = reader.ReadBoolean();
            break;
          case 886:
            dataManager.HasReturnedAym = reader.ReadBoolean();
            break;
          case 887:
            dataManager.HasReturnedBoth = reader.ReadBoolean();
            break;
          case 888:
            dataManager.PlayedPostYngyaSequence = reader.ReadBoolean();
            break;
          case 889:
            dataManager.QuickStartActive = reader.ReadBoolean();
            break;
          case 890:
            dataManager.RemovedStoryMomentsActive = reader.ReadBoolean();
            break;
          case 891:
            dataManager.WinterModeActive = reader.ReadBoolean();
            break;
          case 892:
            dataManager.SurvivalModeActive = reader.ReadBoolean();
            break;
          case 893:
            dataManager.SurvivalModeFirstSpawn = reader.ReadBoolean();
            break;
          case 894:
            dataManager.SurvivalModeOnboarded = reader.ReadBoolean();
            break;
          case 895:
            dataManager.SurvivalSleepOnboarded = reader.ReadBoolean();
            break;
          case 896:
            dataManager.SurvivalHungerOnboarded = reader.ReadBoolean();
            break;
          case 897:
            dataManager.PlimboRejectedRotEye = reader.ReadBoolean();
            break;
          case 898:
            dataManager.SurvivalMode_Hunger = reader.ReadSingle();
            break;
          case 899:
            dataManager.SurvivalMode_Sleep = reader.ReadSingle();
            break;
          case 900:
            dataManager.RedHeartsTemporarilyRemoved = reader.ReadInt32();
            break;
          case 901:
            dataManager.ShownKnuckleboneTutorial = reader.ReadBoolean();
            break;
          case 902:
            dataManager.Knucklebones_Opponent_Ratau_Won = reader.ReadBoolean();
            break;
          case 903:
            dataManager.FollowerKnucklebonesMatch = reader.ReadSingle();
            break;
          case 904:
            dataManager.NextKnucklbonesLucky = reader.ReadBoolean();
            break;
          case 905:
            dataManager.FlockadeTutorialShown = reader.ReadBoolean();
            break;
          case 906:
            dataManager.FlockadeShepherdsTutorialShown = reader.ReadBoolean();
            break;
          case 907:
            dataManager.FlockadeFirstGameOpponentStarts = reader.ReadBoolean();
            break;
          case 908:
            dataManager.FlockadePlayed = reader.ReadBoolean();
            break;
          case 909:
            dataManager.HasNewFlockadePieces = reader.ReadBoolean();
            break;
          case 910:
            dataManager.AnimalID = reader.ReadInt32();
            break;
          case 911:
            dataManager.ShopKeeperChefState = reader.ReadInt32();
            break;
          case 912:
            dataManager.ShopKeeperChefEnragedDay = reader.ReadInt32();
            break;
          case 913:
            dataManager.Knucklebones_Opponent_0 = reader.ReadBoolean();
            break;
          case 914:
            dataManager.Knucklebones_Opponent_0_FirstConvoRataus = reader.ReadBoolean();
            break;
          case 915:
            dataManager.Knucklebones_Opponent_0_Won = reader.ReadBoolean();
            break;
          case 916:
            dataManager.Knucklebones_Opponent_1 = reader.ReadBoolean();
            break;
          case 917:
            dataManager.Knucklebones_Opponent_1_FirstConvoRataus = reader.ReadBoolean();
            break;
          case 918:
            dataManager.Knucklebones_Opponent_1_Won = reader.ReadBoolean();
            break;
          case 919:
            dataManager.Knucklebones_Opponent_2 = reader.ReadBoolean();
            break;
          case 920:
            dataManager.Knucklebones_Opponent_2_FirstConvoRataus = reader.ReadBoolean();
            break;
          case 921:
            dataManager.Knucklebones_Opponent_2_Won = reader.ReadBoolean();
            break;
          case 922:
            dataManager.RefinedElectrifiedRotstone = reader.ReadBoolean();
            break;
          case 923:
            dataManager.DungeonLayer1 = reader.ReadBoolean();
            break;
          case 924:
            dataManager.DungeonLayer2 = reader.ReadBoolean();
            break;
          case 925:
            dataManager.DungeonLayer3 = reader.ReadBoolean();
            break;
          case 926:
            dataManager.DungeonLayer4 = reader.ReadBoolean();
            break;
          case 927:
            dataManager.DungeonLayer5 = reader.ReadBoolean();
            break;
          case 928:
            dataManager.BeatenDungeon1 = reader.ReadBoolean();
            break;
          case 929:
            dataManager.BeatenDungeon2 = reader.ReadBoolean();
            break;
          case 930:
            dataManager.BeatenDungeon3 = reader.ReadBoolean();
            break;
          case 931:
            dataManager.BeatenDungeon4 = reader.ReadBoolean();
            break;
          case 932:
            dataManager.BeatenDeathCat = reader.ReadBoolean();
            break;
          case 933:
            dataManager.BeatenLeshyLayer2 = reader.ReadBoolean();
            break;
          case 934:
            dataManager.BeatenHeketLayer2 = reader.ReadBoolean();
            break;
          case 935:
            dataManager.BeatenKallamarLayer2 = reader.ReadBoolean();
            break;
          case 936:
            dataManager.BeatenShamuraLayer2 = reader.ReadBoolean();
            break;
          case 937:
            dataManager.BeatenOneDungeons = reader.ReadBoolean();
            break;
          case 938:
            dataManager.BeatenTwoDungeons = reader.ReadBoolean();
            break;
          case 939:
            dataManager.BeatenThreeDungeons = reader.ReadBoolean();
            break;
          case 940:
            dataManager.BeatenFourDungeons = reader.ReadBoolean();
            break;
          case 941:
            dataManager.Dungeon1GodTears = reader.ReadInt32();
            break;
          case 942:
            dataManager.Dungeon2GodTears = reader.ReadInt32();
            break;
          case 943:
            dataManager.Dungeon3GodTears = reader.ReadInt32();
            break;
          case 944:
            dataManager.Dungeon4GodTears = reader.ReadInt32();
            break;
          case 945:
            dataManager.DungeonRunsSinceBeatingFirstDungeon = reader.ReadInt32();
            break;
          case 946:
            dataManager.PreviousMiniBoss = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 947:
            dataManager.FishCaughtInsideWhaleToday = reader.ReadInt32();
            break;
          case 948:
            dataManager.SandboxProgression = resolver.GetFormatterWithVerify<List<DungeonSandboxManager.ProgressionSnapshot>>().Deserialize(ref reader, options);
            break;
          case 949:
            dataManager.OnboardedBossRush = reader.ReadBoolean();
            break;
          case 950:
            dataManager.CompletedSandbox = reader.ReadBoolean();
            break;
          case 951:
            dataManager.CanFindTarotCards = reader.ReadBoolean();
            break;
          case 952:
            dataManager.LuckMultiplier = reader.ReadSingle();
            break;
          case 953:
            dataManager.NextMissionarySuccessful = reader.ReadBoolean();
            break;
          case 954:
            dataManager.EnemyModifiersChanceMultiplier = reader.ReadSingle();
            break;
          case 955:
            dataManager.EnemyHealthMultiplier = reader.ReadSingle();
            break;
          case 956:
            dataManager.ProjectileMoveSpeedMultiplier = reader.ReadSingle();
            break;
          case 957:
            dataManager.DodgeDistanceMultiplier = reader.ReadSingle();
            break;
          case 958:
            dataManager.CurseFeverMultiplier = reader.ReadSingle();
            break;
          case 959:
            dataManager.SpawnPoisonOnAttack = reader.ReadBoolean();
            break;
          case 960:
            dataManager.EnemiesInNextRoomHaveModifiers = reader.ReadBoolean();
            break;
          case 961:
            dataManager.EnemiesDropGoldDuringRun = reader.ReadBoolean();
            break;
          case 962:
            dataManager.NoRollInNextCombatRoom = reader.ReadBoolean();
            break;
          case 963:
            dataManager.NoHeartDrops = reader.ReadBoolean();
            break;
          case 964:
            dataManager.EnemiesDropBombOnDeath = reader.ReadBoolean();
            break;
          case 965:
            dataManager.CurrentRoomCoordinates = resolver.GetFormatterWithVerify<Vector2>().Deserialize(ref reader, options);
            break;
          case 966:
            dataManager.SpecialAttackDamageMultiplier = reader.ReadSingle();
            break;
          case 967:
            dataManager.NextChestGold = reader.ReadBoolean();
            break;
          case 968:
            dataManager.SpecialAttacksDisabled = reader.ReadBoolean();
            break;
          case 969:
            dataManager.BossHealthMultiplier = reader.ReadSingle();
            break;
          case 970:
            dataManager.ResurrectRitualCount = reader.ReadInt32();
            break;
          case 971:
            dataManager.NextRitualFree = reader.ReadBoolean();
            break;
          case 972:
            dataManager.EncounteredGambleRoom = reader.ReadBoolean();
            break;
          case 973:
            dataManager.LeaderFollowersRecruited = resolver.GetFormatterWithVerify<List<int>>().Deserialize(ref reader, options);
            break;
          case 974:
            dataManager.UniqueFollowersRecruited = resolver.GetFormatterWithVerify<List<int>>().Deserialize(ref reader, options);
            break;
          case 975:
            dataManager.SwordLevel = reader.ReadInt32();
            break;
          case 976:
            dataManager.DaggerLevel = reader.ReadInt32();
            break;
          case 977:
            dataManager.AxeLevel = reader.ReadInt32();
            break;
          case 978:
            dataManager.HammerLevel = reader.ReadInt32();
            break;
          case 979:
            dataManager.GauntletLevel = reader.ReadInt32();
            break;
          case 980:
            dataManager.FireballLevel = reader.ReadInt32();
            break;
          case 981:
            dataManager.EnemyBlastLevel = reader.ReadInt32();
            break;
          case 982:
            dataManager.MegaSlashLevel = reader.ReadInt32();
            break;
          case 983:
            dataManager.ProjectileAOELevel = reader.ReadInt32();
            break;
          case 984:
            dataManager.TentaclesLevel = reader.ReadInt32();
            break;
          case 985:
            dataManager.VortexLevel = reader.ReadInt32();
            break;
          case 986:
            dataManager.LastFollowerQuestGivenTime = reader.ReadSingle();
            break;
          case 987:
            dataManager.DLC_Pre_Purchase = reader.ReadBoolean();
            break;
          case 988:
            dataManager.DLC_Cultist_Pack = reader.ReadBoolean();
            break;
          case 989:
            dataManager.DLC_Heretic_Pack = reader.ReadBoolean();
            break;
          case 990:
            dataManager.DLC_Sinful_Pack = reader.ReadBoolean();
            break;
          case 991:
            dataManager.DLC_Pilgrim_Pack = reader.ReadBoolean();
            break;
          case 992:
            dataManager.MAJOR_DLC = reader.ReadBoolean();
            break;
          case 993:
            dataManager.DLC_Plush_Bonus = reader.ReadBoolean();
            break;
          case 994:
            dataManager.PAX_DLC = reader.ReadBoolean();
            break;
          case 995:
            dataManager.Twitch_Drop_1 = reader.ReadBoolean();
            break;
          case 996:
            dataManager.Twitch_Drop_2 = reader.ReadBoolean();
            break;
          case 997:
            dataManager.Twitch_Drop_3 = reader.ReadBoolean();
            break;
          case 998:
            dataManager.Twitch_Drop_4 = reader.ReadBoolean();
            break;
          case 999:
            dataManager.Twitch_Drop_5 = reader.ReadBoolean();
            break;
          case 1000:
            dataManager.Twitch_Drop_6 = reader.ReadBoolean();
            break;
          case 1001:
            dataManager.Twitch_Drop_7 = reader.ReadBoolean();
            break;
          case 1002:
            dataManager.Twitch_Drop_8 = reader.ReadBoolean();
            break;
          case 1003:
            dataManager.Twitch_Drop_9 = reader.ReadBoolean();
            break;
          case 1004:
            dataManager.Twitch_Drop_10 = reader.ReadBoolean();
            break;
          case 1005:
            dataManager.Twitch_Drop_11 = reader.ReadBoolean();
            break;
          case 1006:
            dataManager.Twitch_Drop_12 = reader.ReadBoolean();
            break;
          case 1007:
            dataManager.Twitch_Drop_13 = reader.ReadBoolean();
            break;
          case 1008:
            dataManager.Twitch_Drop_14 = reader.ReadBoolean();
            break;
          case 1009:
            dataManager.Twitch_Drop_15 = reader.ReadBoolean();
            break;
          case 1010:
            dataManager.SupportStreamer = reader.ReadBoolean();
            break;
          case 1011:
            dataManager.LandConvoProgress = reader.ReadInt32();
            break;
          case 1012:
            dataManager.LandResourcesGiven = reader.ReadInt32();
            break;
          case 1013:
            dataManager.LandPurchased = reader.ReadInt32();
            break;
          case 1014:
            dataManager.HasWeatherVane = reader.ReadBoolean();
            break;
          case 1015:
            dataManager.HasWeatherVaneUI = reader.ReadBoolean();
            break;
          case 1016:
            dataManager.ShopsBuilt = resolver.GetFormatterWithVerify<List<int>>().Deserialize(ref reader, options);
            break;
          case 1017:
            dataManager.InteractedDLCShrine = reader.ReadBoolean();
            break;
          case 1018:
            dataManager.NPCRescueRoomsCompleted = reader.ReadInt32();
            break;
          case 1019:
            dataManager.RoomVariant = reader.ReadInt32();
            break;
          case 1020:
            dataManager.TimeSinceLastWolf = reader.ReadSingle();
            break;
          case 1021:
            dataManager.BreakingOutAnimals = resolver.GetFormatterWithVerify<List<StructuresData.Ranchable_Animal>>().Deserialize(ref reader, options);
            break;
          case 1022:
            dataManager.DisoveredAnimals = resolver.GetFormatterWithVerify<List<InventoryItem.ITEM_TYPE>>().Deserialize(ref reader, options);
            break;
          case 1023 /*0x03FF*/:
            dataManager.FollowingPlayerAnimals = resolver.GetFormatterWithVerify<StructuresData.Ranchable_Animal[]>().Deserialize(ref reader, options);
            break;
          case 1024 /*0x0400*/:
            dataManager.BlizzardOfferingRequirements = resolver.GetFormatterWithVerify<List<DataManager.Offering>>().Deserialize(ref reader, options);
            break;
          case 1025:
            dataManager.BlizzardOfferingsGiven = resolver.GetFormatterWithVerify<List<DataManager.Offering>>().Deserialize(ref reader, options);
            break;
          case 1026:
            dataManager.SacrificeTableInventory = resolver.GetFormatterWithVerify<List<InventoryItem>>().Deserialize(ref reader, options);
            break;
          case 1027:
            dataManager.BlizzardMonsterActive = reader.ReadBoolean();
            break;
          case 1028:
            dataManager.BlizzardSnowmenGiven = reader.ReadInt32();
            break;
          case 1029:
            dataManager.CompletedOfferingThisBlizzard = reader.ReadBoolean();
            break;
          case 1030:
            dataManager.CompletedBlizzardSecret = reader.ReadBoolean();
            break;
          case 1031:
            dataManager.BlizzardOfferingsCompleted = reader.ReadInt32();
            break;
          case 1032:
            dataManager.ForceDammedRelic = reader.ReadBoolean();
            break;
          case 1033:
            dataManager.ForceBlessedRelic = reader.ReadBoolean();
            break;
          case 1034:
            dataManager.FirstRelic = reader.ReadBoolean();
            break;
          case 1035:
            dataManager.EndlessModeOnCooldown = reader.ReadBoolean();
            break;
          case 1036:
            dataManager.EndlessModeSinOncooldown = reader.ReadBoolean();
            break;
          case 1037:
            dataManager.TimeSinceLastStolenFromFollowers = reader.ReadSingle();
            break;
          case 1038:
            dataManager.TimeSinceLastFollowerFight = reader.ReadSingle();
            break;
          case 1039:
            dataManager.TimeSinceLastFollowerEaten = reader.ReadSingle();
            break;
          case 1040:
            dataManager.TimeSinceLastFollowerBump = reader.ReadSingle();
            break;
          case 1041:
            dataManager.TimeSinceLastMissionaryFollowerEncounter = reader.ReadSingle();
            break;
          case 1042:
            dataManager.DaySinceLastSpecialPoop = reader.ReadInt32();
            break;
          case 1043:
            dataManager.followerRecruitWaiting = reader.ReadBoolean();
            break;
          case 1044:
            dataManager.weddingsPerformed = reader.ReadInt32();
            break;
          case 1045:
            dataManager.ForceClothesShop = reader.ReadBoolean();
            break;
          case 1046:
            dataManager.UnlockedTailor = reader.ReadBoolean();
            break;
          case 1047:
            dataManager.RevealedTailor = reader.ReadBoolean();
            break;
          case 1048:
            dataManager.TookBopToTailor = reader.ReadBoolean();
            break;
          case 1049:
            dataManager.LeftBopAtTailor = reader.ReadBoolean();
            break;
          case 1050:
            dataManager.BerithTalkedWithBop = reader.ReadBoolean();
            break;
          case 1051:
            dataManager.itemsCleaned = reader.ReadInt32();
            break;
          case 1052:
            dataManager.outfitsCreated = reader.ReadInt32();
            break;
          case 1053:
            dataManager.drinksCreated = reader.ReadInt32();
            break;
          case 1054:
            dataManager.eggsCracked = reader.ReadInt32();
            break;
          case 1055:
            dataManager.eggsHatched = reader.ReadInt32();
            break;
          case 1056:
            dataManager.EggsProduced = reader.ReadInt32();
            break;
          case 1057:
            dataManager.HasProducedChosenOne = reader.ReadBoolean();
            break;
          case 1058:
            dataManager.HasGivenPedigreeFollower = reader.ReadBoolean();
            break;
          case 1059:
            dataManager.pleasurePointsRedeemed = reader.ReadInt32();
            break;
          case 1060:
            dataManager.PreviousSinPointFollowers = resolver.GetFormatterWithVerify<List<int>>().Deserialize(ref reader, options);
            break;
          case 1061:
            dataManager.pleasurePointsRedeemedFollowerSpoken = reader.ReadBoolean();
            break;
          case 1062:
            dataManager.pleasurePointsRedeemedTempleFollowerSpoken = reader.ReadBoolean();
            break;
          case 1063:
            dataManager.damnedConversation = reader.ReadInt32();
            break;
          case 1064:
            dataManager.damnedFightConversation = reader.ReadInt32();
            break;
          case 1065:
            dataManager.ForceGoldenEgg = reader.ReadBoolean();
            break;
          case 1066:
            dataManager.ForceSpecialPoo = reader.ReadBoolean();
            break;
          case 1067:
            dataManager.ForceAbomination = reader.ReadBoolean();
            break;
          case 1068:
            dataManager.clickedDLCAd = reader.ReadBoolean();
            break;
          case 1069:
            dataManager.RevealedDLCMapDoor = reader.ReadBoolean();
            break;
          case 1070:
            dataManager.RevealedDLCMapHeart = reader.ReadBoolean();
            break;
          case 1071:
            dataManager.YngyaHeartRoomEncounters = reader.ReadInt32();
            break;
          case 1072:
            dataManager.DeathCatBeaten = reader.ReadBoolean();
            break;
          case 1073:
            dataManager.HasEncounteredTarot = reader.ReadBoolean();
            break;
          case 1074:
            dataManager.RecentRecipes = resolver.GetFormatterWithVerify<List<InventoryItem.ITEM_TYPE>>().Deserialize(ref reader, options);
            break;
          case 1075:
            dataManager.RecipesDiscovered = resolver.GetFormatterWithVerify<List<InventoryItem.ITEM_TYPE>>().Deserialize(ref reader, options);
            break;
          case 1076:
            dataManager.LoreUnlocked = resolver.GetFormatterWithVerify<List<int>>().Deserialize(ref reader, options);
            break;
          case 1077:
            dataManager.LoreStonesOnboarded = reader.ReadBoolean();
            break;
          case 1078:
            dataManager.LoreOnboarded = reader.ReadBoolean();
            break;
          case 1079:
            dataManager.UpgradeTreeMenuDLCAlert = reader.ReadBoolean();
            break;
          case 1080:
            dataManager.DepositedFollowers = resolver.GetFormatterWithVerify<List<DataManager.DepositedFollower>>().Deserialize(ref reader, options);
            break;
          case 1081:
            dataManager.playerDamageDealt = reader.ReadSingle();
            break;
          case 1082:
            dataManager.PlayerScaleModifier = reader.ReadInt32();
            break;
          case 1083:
            dataManager.ChefShopDoublePrices = reader.ReadBoolean();
            break;
          case 1084:
            dataManager.FollowerShopUses = reader.ReadInt32();
            break;
          case 1085:
            dataManager.WoolhavenFlowerPots = resolver.GetFormatterWithVerify<List<DataManager.WoolhavenFlowerPot>>().Deserialize(ref reader, options);
            break;
          case 1086:
            dataManager.FullWoolhavenFlowerPots = resolver.GetFormatterWithVerify<List<int>>().Deserialize(ref reader, options);
            break;
          case 1087:
            dataManager.sacrificesCompleted = reader.ReadInt32();
            break;
          case 1088:
            dataManager.FoundItems = resolver.GetFormatterWithVerify<List<InventoryItem.ITEM_TYPE>>().Deserialize(ref reader, options);
            break;
          case 1089:
            dataManager.TakenBossDamage = reader.ReadBoolean();
            break;
          case 1090:
            dataManager.PoopMealsCreated = reader.ReadInt32();
            break;
          case 1091:
            dataManager.PrayedAtCrownShrine = reader.ReadBoolean();
            break;
          case 1092:
            dataManager.ShellsGifted_0 = reader.ReadBoolean();
            break;
          case 1093:
            dataManager.ShellsGifted_1 = reader.ReadBoolean();
            break;
          case 1094:
            dataManager.ShellsGifted_2 = reader.ReadBoolean();
            break;
          case 1095:
            dataManager.ShellsGifted_3 = reader.ReadBoolean();
            break;
          case 1096:
            dataManager.ShellsGifted_4 = reader.ReadBoolean();
            break;
          case 1097:
            dataManager.LostSoulsBark = reader.ReadInt32();
            break;
          case 1098:
            dataManager.DateLastScreenshot = reader.ReadInt32();
            break;
          case 1099:
            dataManager.PlayerDamageDealtThisRun = reader.ReadSingle();
            break;
          case 1100:
            dataManager.PlayerDamageReceivedThisRun = reader.ReadSingle();
            break;
          case 1101:
            dataManager.playerDamageReceived = reader.ReadSingle();
            break;
          case 1102:
            dataManager.Options_ScreenShake = reader.ReadBoolean();
            break;
          case 1103:
            dataManager.PlayerIsASpirit = reader.ReadBoolean();
            break;
          case 1104:
            dataManager.BridgeFixed = reader.ReadBoolean();
            break;
          case 1105:
            dataManager.BuildingTome = reader.ReadBoolean();
            break;
          case 1106:
            dataManager.BeenToDungeon = reader.ReadBoolean();
            break;
          case 1107:
            dataManager.FollowerID = reader.ReadInt32();
            break;
          case 1108:
            dataManager.ObjectiveGroupID = reader.ReadInt32();
            break;
          case 1109:
            dataManager.Followers = resolver.GetFormatterWithVerify<List<FollowerInfo>>().Deserialize(ref reader, options);
            break;
          case 1110:
            dataManager.Followers_Recruit = resolver.GetFormatterWithVerify<List<FollowerInfo>>().Deserialize(ref reader, options);
            break;
          case 1111:
            dataManager.Followers_Dead = resolver.GetFormatterWithVerify<List<FollowerInfo>>().Deserialize(ref reader, options);
            break;
          case 1112:
            dataManager.Followers_Dead_IDs = resolver.GetFormatterWithVerify<List<int>>().Deserialize(ref reader, options);
            break;
          case 1113:
            dataManager.Followers_Possessed = resolver.GetFormatterWithVerify<List<FollowerInfo>>().Deserialize(ref reader, options);
            break;
          case 1114:
            dataManager.Followers_Dissented = resolver.GetFormatterWithVerify<List<FollowerInfo>>().Deserialize(ref reader, options);
            break;
          case 1115:
            dataManager.EncounteredPossessedEnemyRun = reader.ReadInt32();
            break;
          case 1116:
            dataManager.StructureID = reader.ReadInt32();
            break;
          case 1117:
            dataManager.BaseStructures = resolver.GetFormatterWithVerify<List<StructuresData>>().Deserialize(ref reader, options);
            break;
          case 1118:
            dataManager.MajorDLCCachedBaseStructures = resolver.GetFormatterWithVerify<List<StructuresData>>().Deserialize(ref reader, options);
            break;
          case 1119:
            dataManager.HubStructures = resolver.GetFormatterWithVerify<List<StructuresData>>().Deserialize(ref reader, options);
            break;
          case 1120:
            dataManager.HubShoreStructures = resolver.GetFormatterWithVerify<List<StructuresData>>().Deserialize(ref reader, options);
            break;
          case 1121:
            dataManager.Hub1_MainStructures = resolver.GetFormatterWithVerify<List<StructuresData>>().Deserialize(ref reader, options);
            break;
          case 1122:
            dataManager.Hub1_BerriesStructures = resolver.GetFormatterWithVerify<List<StructuresData>>().Deserialize(ref reader, options);
            break;
          case 1123:
            dataManager.Hub1_ForestStructures = resolver.GetFormatterWithVerify<List<StructuresData>>().Deserialize(ref reader, options);
            break;
          case 1124:
            dataManager.Hub1_RatauInsideStructures = resolver.GetFormatterWithVerify<List<StructuresData>>().Deserialize(ref reader, options);
            break;
          case 1125:
            dataManager.Hub1_RatauOutsideStructures = resolver.GetFormatterWithVerify<List<StructuresData>>().Deserialize(ref reader, options);
            break;
          case 1126:
            dataManager.Hub1_SozoStructures = resolver.GetFormatterWithVerify<List<StructuresData>>().Deserialize(ref reader, options);
            break;
          case 1127:
            dataManager.Hub1_SwampStructures = resolver.GetFormatterWithVerify<List<StructuresData>>().Deserialize(ref reader, options);
            break;
          case 1128:
            dataManager.WoolhavenStructures = resolver.GetFormatterWithVerify<List<StructuresData>>().Deserialize(ref reader, options);
            break;
          case 1129:
            dataManager.Dungeon_Logs1Structures = resolver.GetFormatterWithVerify<List<StructuresData>>().Deserialize(ref reader, options);
            break;
          case 1130:
            dataManager.Dungeon_Logs2Structures = resolver.GetFormatterWithVerify<List<StructuresData>>().Deserialize(ref reader, options);
            break;
          case 1131:
            dataManager.Dungeon_Logs3Structures = resolver.GetFormatterWithVerify<List<StructuresData>>().Deserialize(ref reader, options);
            break;
          case 1132:
            dataManager.Dungeon_Food1Structures = resolver.GetFormatterWithVerify<List<StructuresData>>().Deserialize(ref reader, options);
            break;
          case 1133:
            dataManager.Dungeon_Food2Structures = resolver.GetFormatterWithVerify<List<StructuresData>>().Deserialize(ref reader, options);
            break;
          case 1134:
            dataManager.Dungeon_Food3Structures = resolver.GetFormatterWithVerify<List<StructuresData>>().Deserialize(ref reader, options);
            break;
          case 1135:
            dataManager.Dungeon_Stone1Structures = resolver.GetFormatterWithVerify<List<StructuresData>>().Deserialize(ref reader, options);
            break;
          case 1136:
            dataManager.Dungeon_Stone2Structures = resolver.GetFormatterWithVerify<List<StructuresData>>().Deserialize(ref reader, options);
            break;
          case 1137:
            dataManager.Dungeon_Stone3Structures = resolver.GetFormatterWithVerify<List<StructuresData>>().Deserialize(ref reader, options);
            break;
          case 1138:
            dataManager.Followers_TraitManipulating_IDs = resolver.GetFormatterWithVerify<List<int>>().Deserialize(ref reader, options);
            break;
          case 1139:
            dataManager.Followers_Imprisoned_IDs = resolver.GetFormatterWithVerify<List<int>>().Deserialize(ref reader, options);
            break;
          case 1140:
            dataManager.Followers_Elderly_IDs = resolver.GetFormatterWithVerify<List<int>>().Deserialize(ref reader, options);
            break;
          case 1141:
            dataManager.Followers_OnMissionary_IDs = resolver.GetFormatterWithVerify<List<int>>().Deserialize(ref reader, options);
            break;
          case 1142:
            dataManager.Followers_LeftInTheDungeon_IDs = resolver.GetFormatterWithVerify<List<int>>().Deserialize(ref reader, options);
            break;
          case 1143:
            dataManager.Followers_Transitioning_IDs = resolver.GetFormatterWithVerify<List<int>>().Deserialize(ref reader, options);
            break;
          case 1144:
            dataManager.Followers_Demons_IDs = resolver.GetFormatterWithVerify<List<int>>().Deserialize(ref reader, options);
            break;
          case 1145:
            dataManager.Followers_Demons_Types = resolver.GetFormatterWithVerify<List<int>>().Deserialize(ref reader, options);
            break;
          case 1146:
            dataManager.MatingCompletedTimestamp = reader.ReadSingle();
            break;
          case 1147:
            dataManager.ActiveSeasonalEvents = resolver.GetFormatterWithVerify<List<SeasonalEventType>>().Deserialize(ref reader, options);
            break;
          case 1148:
            dataManager.CustomisedFleeceOptions = resolver.GetFormatterWithVerify<List<Vector2>>().Deserialize(ref reader, options);
            break;
          case 1149:
            dataManager.RevealedStructures = resolver.GetFormatterWithVerify<List<StructureBrain.TYPES>>().Deserialize(ref reader, options);
            break;
          case 1150:
            dataManager.DayList = resolver.GetFormatterWithVerify<List<DayObject>>().Deserialize(ref reader, options);
            break;
          case 1151:
            dataManager.CurrentDay = resolver.GetFormatterWithVerify<DayObject>().Deserialize(ref reader, options);
            break;
          case 1152:
            dataManager.TrackedObjectiveGroupIDs = resolver.GetFormatterWithVerify<List<string>>().Deserialize(ref reader, options);
            break;
          case 1153:
            dataManager.Objectives = resolver.GetFormatterWithVerify<List<ObjectivesData>>().Deserialize(ref reader, options);
            break;
          case 1154:
            dataManager.CompletedObjectives = resolver.GetFormatterWithVerify<List<ObjectivesData>>().Deserialize(ref reader, options);
            break;
          case 1155:
            dataManager.FailedObjectives = resolver.GetFormatterWithVerify<List<ObjectivesData>>().Deserialize(ref reader, options);
            break;
          case 1156:
            dataManager.DungeonObjectives = resolver.GetFormatterWithVerify<List<ObjectivesData>>().Deserialize(ref reader, options);
            break;
          case 1157:
            dataManager.StoryObjectives = resolver.GetFormatterWithVerify<List<StoryData>>().Deserialize(ref reader, options);
            break;
          case 1158:
            dataManager.CompletedObjectivesHistory = resolver.GetFormatterWithVerify<List<ObjectivesDataFinalized>>().Deserialize(ref reader, options);
            break;
          case 1159:
            dataManager.FailedObjectivesHistory = resolver.GetFormatterWithVerify<List<ObjectivesDataFinalized>>().Deserialize(ref reader, options);
            break;
          case 1160:
            dataManager.CompletedQuestsHistorys = resolver.GetFormatterWithVerify<List<DataManager.QuestHistoryData>>().Deserialize(ref reader, options);
            break;
          case 1161:
            dataManager.SimpleInventoryItem = resolver.GetFormatterWithVerify<InventoryItem.ITEM_TYPE>().Deserialize(ref reader, options);
            break;
          case 1162:
            dataManager.items = resolver.GetFormatterWithVerify<List<InventoryItem>>().Deserialize(ref reader, options);
            break;
          case 1163:
            dataManager.IngredientsCapacityLevel = reader.ReadInt32();
            break;
          case 1164:
            dataManager.FoodCapacityLevel = reader.ReadInt32();
            break;
          case 1165:
            dataManager.LogCapacityLevel = reader.ReadInt32();
            break;
          case 1166:
            dataManager.StoneCapacityLevel = reader.ReadInt32();
            break;
          case 1167:
            dataManager.FollowerSkinsUnlocked = resolver.GetFormatterWithVerify<List<string>>().Deserialize(ref reader, options);
            break;
          case 1168:
            dataManager.StructureEffects = resolver.GetFormatterWithVerify<List<StructureEffect>>().Deserialize(ref reader, options);
            break;
          case 1169:
            dataManager.KilledBosses = resolver.GetFormatterWithVerify<List<string>>().Deserialize(ref reader, options);
            break;
          case 1170:
            dataManager.WeaponPool = resolver.GetFormatterWithVerify<List<EquipmentType>>().Deserialize(ref reader, options);
            break;
          case 1171:
            dataManager.LegendarySwordCustomName = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 1172:
            dataManager.LegendaryAxeCustomName = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 1173:
            dataManager.LegendaryDaggerCustomName = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 1174:
            dataManager.LegendaryHammerCustomName = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 1175:
            dataManager.LegendaryGauntletCustomName = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 1176:
            dataManager.LegendaryBlunderbussCustomName = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 1177:
            dataManager.LegendaryChainCustomName = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 1178:
            dataManager.LegendaryWeaponsJobBoardCompleted = resolver.GetFormatterWithVerify<List<EquipmentType>>().Deserialize(ref reader, options);
            break;
          case 1179:
            dataManager.CurrentRunWeaponLevel = reader.ReadInt32();
            break;
          case 1180:
            dataManager.ForcedStartingWeapon = resolver.GetFormatterWithVerify<EquipmentType>().Deserialize(ref reader, options);
            break;
          case 1181:
            dataManager.CurrentRunCurseLevel = reader.ReadInt32();
            break;
          case 1182:
            dataManager.ForcedStartingCurse = resolver.GetFormatterWithVerify<EquipmentType>().Deserialize(ref reader, options);
            break;
          case 1183:
            dataManager.SpawnedRelicsThisRun = resolver.GetFormatterWithVerify<List<RelicType>>().Deserialize(ref reader, options);
            break;
          case 1184:
            dataManager.CursePool = resolver.GetFormatterWithVerify<List<EquipmentType>>().Deserialize(ref reader, options);
            break;
          case 1185:
            dataManager.PlayerFoundTrinkets = resolver.GetFormatterWithVerify<List<TarotCards.Card>>().Deserialize(ref reader, options);
            break;
          case 1186:
            dataManager.CrownAbilitiesUnlocked = resolver.GetFormatterWithVerify<List<CrownAbilities>>().Deserialize(ref reader, options);
            break;
          case 1187:
            dataManager.PlayerFoundRelics = resolver.GetFormatterWithVerify<List<RelicType>>().Deserialize(ref reader, options);
            break;
          case 1188:
            dataManager.PlayerBluePrints = resolver.GetFormatterWithVerify<List<BluePrint>>().Deserialize(ref reader, options);
            break;
          case 1189:
            dataManager.PlayerFoundPieces = resolver.GetFormatterWithVerify<List<FlockadePieceType>>().Deserialize(ref reader, options);
            break;
          case 1190:
            dataManager.FishCaught = resolver.GetFormatterWithVerify<List<InventoryItem.ITEM_TYPE>>().Deserialize(ref reader, options);
            break;
          case 1191:
            dataManager.ActiveMissions = resolver.GetFormatterWithVerify<List<MissionManager.Mission>>().Deserialize(ref reader, options);
            break;
          case 1192:
            dataManager.AvailableMissions = resolver.GetFormatterWithVerify<List<MissionManager.Mission>>().Deserialize(ref reader, options);
            break;
          case 1193:
            dataManager.NewMissionDayTimestamp = reader.ReadSingle();
            break;
          case 1194:
            dataManager.LastGoldenMissionDay = reader.ReadInt32();
            break;
          case 1195:
            dataManager.MissionShrineUnlocked = reader.ReadBoolean();
            break;
          case 1196:
            dataManager.ItemsForSale = resolver.GetFormatterWithVerify<List<BuyEntry>>().Deserialize(ref reader, options);
            break;
          case 1197:
            dataManager.Shops = resolver.GetFormatterWithVerify<List<ShopLocationTracker>>().Deserialize(ref reader, options);
            break;
          case 1198:
            dataManager.LastDayUsedFollowerShop = reader.ReadInt32();
            break;
          case 1199:
            dataManager.FollowerForSale = resolver.GetFormatterWithVerify<FollowerInfo>().Deserialize(ref reader, options);
            break;
          case 1200:
            dataManager.midasDonation = resolver.GetFormatterWithVerify<MidasDonation>().Deserialize(ref reader, options);
            break;
          case 1201:
            dataManager.LastDayUsedBank = reader.ReadInt32();
            break;
          case 1202:
            dataManager.Investment = resolver.GetFormatterWithVerify<JellyFishInvestment>().Deserialize(ref reader, options);
            break;
          case 1203:
            dataManager.Traders = resolver.GetFormatterWithVerify<List<TraderTracker>>().Deserialize(ref reader, options);
            break;
          case 1204:
            dataManager.LastDayUsedFlockadeHint = reader.ReadInt32();
            break;
          case 1205:
            dataManager.HintedPieceType = resolver.GetFormatterWithVerify<FlockadePieceType>().Deserialize(ref reader, options);
            break;
          case 1206:
            dataManager.ShrineTimerInfo = resolver.GetFormatterWithVerify<List<ShrineUsageInfo>>().Deserialize(ref reader, options);
            break;
          case 1207:
            dataManager.RedHeartShrineLevel = reader.ReadInt32();
            break;
          case 1208:
            dataManager.ShrineHeart = reader.ReadInt32();
            break;
          case 1209:
            dataManager.ShrineCurses = reader.ReadInt32();
            break;
          case 1210:
            dataManager.ShrineVoodo = reader.ReadInt32();
            break;
          case 1211:
            dataManager.ShrineAstrology = reader.ReadInt32();
            break;
          case 1212:
            dataManager.ItemSelectorCategories = resolver.GetFormatterWithVerify<List<Lamb.UI.ItemSelector.Category>>().Deserialize(ref reader, options);
            break;
          case 1213:
            dataManager.itemsDungeon = resolver.GetFormatterWithVerify<List<InventoryItem>>().Deserialize(ref reader, options);
            break;
          case 1214:
            dataManager.DUNGEON_TIME = reader.ReadSingle();
            break;
          case 1215:
            dataManager.PLAYER_RUN_DAMAGE_LEVEL = reader.ReadSingle();
            break;
          case 1216:
            dataManager.PLAYER_HEARTS_LEVEL = reader.ReadInt32();
            break;
          case 1217:
            dataManager.PLAYER_DAMAGE_LEVEL = reader.ReadInt32();
            break;
          case 1218:
            dataManager.PLAYER_HEALTH = reader.ReadSingle();
            break;
          case 1219:
            dataManager.PLAYER_TOTAL_HEALTH = reader.ReadSingle();
            break;
          case 1220:
            dataManager.PLAYER_BLUE_HEARTS = reader.ReadSingle();
            break;
          case 1221:
            dataManager.PLAYER_BLACK_HEARTS = reader.ReadSingle();
            break;
          case 1222:
            dataManager.PLAYER_FIRE_HEARTS = reader.ReadSingle();
            break;
          case 1223:
            dataManager.PLAYER_ICE_HEARTS = reader.ReadSingle();
            break;
          case 1224:
            dataManager.PLAYER_REMOVED_HEARTS = reader.ReadSingle();
            break;
          case 1225:
            dataManager.PLAYER_SPIRIT_HEARTS = reader.ReadSingle();
            break;
          case 1226:
            dataManager.PLAYER_SPIRIT_TOTAL_HEARTS = reader.ReadSingle();
            break;
          case 1227:
            dataManager.UnlockedCoopRelicsAndTarots = reader.ReadBoolean();
            break;
          case 1228:
            dataManager.UnlockedCoopTarots = reader.ReadBoolean();
            break;
          case 1229:
            dataManager.UnlockedCoopRelics = reader.ReadBoolean();
            break;
          case 1230:
            dataManager.UnlockedCorruptedRelicsAndTarots = reader.ReadBoolean();
            break;
          case 1231:
            dataManager.COOP_PLAYER_BLUE_HEARTS = reader.ReadSingle();
            break;
          case 1232:
            dataManager.COOP_PLAYER_BLACK_HEARTS = reader.ReadSingle();
            break;
          case 1233:
            dataManager.COOP_PLAYER_FIRE_HEARTS = reader.ReadSingle();
            break;
          case 1234:
            dataManager.COOP_PLAYER_ICE_HEARTS = reader.ReadSingle();
            break;
          case 1235:
            dataManager.COOP_PLAYER_REMOVED_HEARTS = reader.ReadSingle();
            break;
          case 1236:
            dataManager.PLAYER_SPECIAL_CHARGE = reader.ReadSingle();
            break;
          case 1237:
            dataManager.PLAYER_SPECIAL_AMMO = reader.ReadSingle();
            break;
          case 1238:
            dataManager.PLAYER_SPECIAL_CHARGE_TARGET = reader.ReadSingle();
            break;
          case 1239:
            dataManager.PLAYER_ARROW_AMMO = reader.ReadInt32();
            break;
          case 1240:
            dataManager.PLAYER_ARROW_TOTAL_AMMO = reader.ReadInt32();
            break;
          case 1241:
            dataManager.PLAYER_SPIRIT_AMMO = reader.ReadInt32();
            break;
          case 1242:
            dataManager.PLAYER_SPIRIT_TOTAL_AMMO = reader.ReadInt32();
            break;
          case 1243:
            dataManager.PLAYER_REDEAL_TOKEN = reader.ReadInt32();
            break;
          case 1244:
            dataManager.PLAYER_REDEAL_TOKEN_TOTAL = reader.ReadInt32();
            break;
          case 1245:
            dataManager.PLAYER_HEALTH_MODIFIED = reader.ReadInt32();
            break;
          case 1246:
            dataManager.COOP_PLAYER_SPIRIT_HEARTS = reader.ReadSingle();
            break;
          case 1247:
            dataManager.COOP_PLAYER_SPIRIT_TOTAL_HEARTS = reader.ReadSingle();
            break;
          case 1248:
            dataManager.PLAYER_STARTING_HEALTH_CACHED = reader.ReadInt32();
            break;
          case 1249:
            dataManager.Souls = reader.ReadInt32();
            break;
          case 1250:
            dataManager.BlackSouls = reader.ReadInt32();
            break;
          case 1251:
            dataManager.BlackSoulTarget = reader.ReadInt32();
            break;
          case 1252:
            dataManager.FollowerTokens = reader.ReadInt32();
            break;
          case 1253:
            dataManager.SpyDay = reader.ReadInt32();
            break;
          case 1254:
            dataManager.SpyJoinedDay = reader.ReadInt32();
            break;
          case 1255:
            dataManager.ShrineGhostJuice = reader.ReadInt32();
            break;
          case 1256:
            dataManager.TotalShrineGhostJuice = reader.ReadInt32();
            break;
          case 1257:
            dataManager.YngyaMiscConvoIndex = reader.ReadInt32();
            break;
          case 1258:
            dataManager.ChoreXP = reader.ReadSingle();
            break;
          case 1259:
            dataManager.ChoreXP_Coop = reader.ReadSingle();
            break;
          case 1260:
            dataManager.ChoreXP_Coop_Temp_Gained = reader.ReadSingle();
            break;
          case 1261:
            dataManager.ChoreXPLevel = reader.ReadInt32();
            break;
          case 1262:
            dataManager.ChoreXPLevel_Coop = reader.ReadInt32();
            break;
          case 1263:
            dataManager.DiscipleXP = reader.ReadSingle();
            break;
          case 1264:
            dataManager.DiscipleLevel = reader.ReadInt32();
            break;
          case 1265:
            dataManager.DiscipleAbilityPoints = reader.ReadInt32();
            break;
          case 1266:
            dataManager.XP = reader.ReadInt32();
            break;
          case 1267:
            dataManager.Level = reader.ReadInt32();
            break;
          case 1268:
            dataManager.AbilityPoints = reader.ReadInt32();
            break;
          case 1269:
            dataManager.WeaponAbilityPoints = reader.ReadInt32();
            break;
          case 1270:
            dataManager.CurrentChallengeModeXP = reader.ReadInt32();
            break;
          case 1271:
            dataManager.CurrentChallengeModeLevel = reader.ReadInt32();
            break;
          case 1272:
            dataManager.Doctrine_Pleasure_XP = reader.ReadSingle();
            break;
          case 1273:
            dataManager.Doctrine_Winter_XP = reader.ReadSingle();
            break;
          case 1274:
            dataManager.Doctrine_PlayerUpgrade_XP = reader.ReadSingle();
            break;
          case 1275:
            dataManager.Doctrine_PlayerUpgrade_Level = reader.ReadInt32();
            break;
          case 1276:
            dataManager.Doctrine_Special_XP = reader.ReadSingle();
            break;
          case 1277:
            dataManager.Doctrine_Special_Level = reader.ReadInt32();
            break;
          case 1278:
            dataManager.Doctrine_WorkWorship_XP = reader.ReadSingle();
            break;
          case 1279 /*0x04FF*/:
            dataManager.Doctrine_WorkWorship_Level = reader.ReadInt32();
            break;
          case 1280 /*0x0500*/:
            dataManager.Doctrine_Possessions_XP = reader.ReadSingle();
            break;
          case 1281:
            dataManager.Doctrine_Possessions_Level = reader.ReadInt32();
            break;
          case 1282:
            dataManager.Doctrine_Food_XP = reader.ReadSingle();
            break;
          case 1283:
            dataManager.Doctrine_Food_Level = reader.ReadInt32();
            break;
          case 1284:
            dataManager.Doctrine_Afterlife_XP = reader.ReadSingle();
            break;
          case 1285:
            dataManager.Doctrine_Afterlife_Level = reader.ReadInt32();
            break;
          case 1286:
            dataManager.Doctrine_LawAndOrder_XP = reader.ReadSingle();
            break;
          case 1287:
            dataManager.Doctrine_LawAndOrder_Level = reader.ReadInt32();
            break;
          case 1288:
            dataManager.Doctrine_Pleasure_Level = reader.ReadInt32();
            break;
          case 1289:
            dataManager.Doctrine_Winter_Level = reader.ReadInt32();
            break;
          case 1290:
            dataManager.CompletedDoctrineStones = reader.ReadInt32();
            break;
          case 1291:
            dataManager.DoctrineCurrentCount = reader.ReadInt32();
            break;
          case 1292:
            dataManager.DoctrineTargetCount = reader.ReadInt32();
            break;
          case 1293:
            dataManager.FRUIT_LOW_MEALS_COOKED = reader.ReadInt32();
            break;
          case 1294:
            dataManager.VEGETABLE_LOW_MEALS_COOKED = reader.ReadInt32();
            break;
          case 1295:
            dataManager.VEGETABLE_MEDIUM_MEALS_COOKED = reader.ReadInt32();
            break;
          case 1296:
            dataManager.VEGETABLE_HIGH_MEALS_COOKED = reader.ReadInt32();
            break;
          case 1297:
            dataManager.FISH_LOW_MEALS_COOKED = reader.ReadInt32();
            break;
          case 1298:
            dataManager.FISH_MEDIUM_MEALS_COOKED = reader.ReadInt32();
            break;
          case 1299:
            dataManager.FISH_HIGH_MEALS_COOKED = reader.ReadInt32();
            break;
          case 1300:
            dataManager.EGG_MEALS_COOKED = reader.ReadInt32();
            break;
          case 1301:
            dataManager.MEAT_LOW_COOKED = reader.ReadInt32();
            break;
          case 1302:
            dataManager.MEAT_MEDIUM_COOKED = reader.ReadInt32();
            break;
          case 1303:
            dataManager.MEAT_HIGH_COOKED = reader.ReadInt32();
            break;
          case 1304:
            dataManager.MIXED_LOW_COOKED = reader.ReadInt32();
            break;
          case 1305:
            dataManager.MIXED_MEDIUM_COOKED = reader.ReadInt32();
            break;
          case 1306:
            dataManager.MIXED_HIGH_COOKED = reader.ReadInt32();
            break;
          case 1307:
            dataManager.POOP_MEALS_COOKED = reader.ReadInt32();
            break;
          case 1308:
            dataManager.GRASS_MEALS_COOKED = reader.ReadInt32();
            break;
          case 1309:
            dataManager.FOLLOWER_MEAT_MEALS_COOKED = reader.ReadInt32();
            break;
          case 1310:
            dataManager.DEADLY_MEALS_COOKED = reader.ReadInt32();
            break;
          case 1311:
            dataManager.ReapedSouls = resolver.GetFormatterWithVerify<List<string>>().Deserialize(ref reader, options);
            break;
          case 1314:
            dataManager.EnemiesKilled = resolver.GetFormatterWithVerify<List<DataManager.EnemyData>>().Deserialize(ref reader, options);
            break;
          case 1315:
            dataManager.Alerts = resolver.GetFormatterWithVerify<Alerts>().Deserialize(ref reader, options);
            break;
          case 1316:
            dataManager.NotificationHistory = resolver.GetFormatterWithVerify<List<FinalizedNotification>>().Deserialize(ref reader, options);
            break;
          case 1317:
            dataManager.blizzardTimeInCurrentSeason = reader.ReadSingle();
            break;
          case 1318:
            dataManager.blizzardEndTimeInCurrentSeason = reader.ReadSingle();
            break;
          case 1319:
            dataManager.blizzardTimeInCurrentSeason2 = reader.ReadSingle();
            break;
          case 1320:
            dataManager.blizzardEndTimeInCurrentSeason2 = reader.ReadSingle();
            break;
          case 1321:
            dataManager.OnboardedYewCursedDungeon = reader.ReadBoolean();
            break;
          case 1322:
            dataManager.BaalNeedsRescue = reader.ReadBoolean();
            break;
          case 1323:
            dataManager.BaalRescued = reader.ReadBoolean();
            break;
          case 1324:
            dataManager.StelleSpecialEncountered = reader.ReadBoolean();
            break;
          case 1325:
            dataManager.MonchMamaSpecialEncountered = reader.ReadBoolean();
            break;
          case 1326:
            dataManager.FishermanDLCSpecialEncountered = reader.ReadBoolean();
            break;
          case 1327:
            dataManager.ShowSpecialStelleRoom = reader.ReadBoolean();
            break;
          case 1328:
            dataManager.ShowSpecialMonchMamaRoom = reader.ReadBoolean();
            break;
          case 1329:
            dataManager.ShowSpecialFishermanDLCRoom = reader.ReadBoolean();
            break;
          case 1330:
            dataManager.InfectedDudeSpecialEncountered = reader.ReadBoolean();
            break;
          case 1331:
            dataManager.DungeonRancherSpecialEncountered = reader.ReadBoolean();
            break;
          case 1332:
            dataManager.RefinedResourcesSpecialEncountered = reader.ReadBoolean();
            break;
          case 1333:
            dataManager.ShowSpecialInfectedDudeRoom = reader.ReadBoolean();
            break;
          case 1334:
            dataManager.ShowSpecialDungeonRancherRoom = reader.ReadBoolean();
            break;
          case 1335:
            dataManager.ShowSpecialRefinedResourcesRoom = reader.ReadBoolean();
            break;
          case 1336:
            dataManager.StripperGaveOutfit = reader.ReadBoolean();
            break;
          case 1337:
            dataManager.OnboardedRotRoom = reader.ReadBoolean();
            break;
          case 1338:
            dataManager.ExecutionerRoom1Encountered = reader.ReadBoolean();
            break;
          case 1339:
            dataManager.ExecutionerRoom2Encountered = reader.ReadBoolean();
            break;
          case 1340:
            dataManager.CanShowExecutionerRoom1 = reader.ReadBoolean();
            break;
          case 1341:
            dataManager.CanShowExecutionerRoom2 = reader.ReadBoolean();
            break;
          case 1342:
            dataManager.EncounteredDungeonRancherCount = reader.ReadInt32();
            break;
          case 1343:
            dataManager.CollectedLightningShards = reader.ReadBoolean();
            break;
          case 1344:
            dataManager.FoundHollowKnightWool = reader.ReadBoolean();
            break;
          case 1345:
            dataManager.EnabledDLCMapHeart = reader.ReadBoolean();
            break;
          case 1346:
            dataManager.FinalDLCMap = reader.ReadBoolean();
            break;
          case 1347:
            dataManager.NPCGhostGeneric11Rescued = reader.ReadBoolean();
            break;
          case 1348:
            dataManager.DiedToWolfBoss = reader.ReadBoolean();
            break;
          case 1349:
            dataManager.DiedToYngyaBoss = reader.ReadBoolean();
            break;
          case 1350:
            dataManager.DragonIntrod = reader.ReadBoolean();
            break;
          case 1351:
            dataManager.DragonEggsCollected = reader.ReadInt32();
            break;
          case 1352:
            dataManager.bestFriendAnimal = resolver.GetFormatterWithVerify<StructuresData.Ranchable_Animal>().Deserialize(ref reader, options);
            break;
          case 1353:
            dataManager.bestFriendAnimalLevel = reader.ReadInt32();
            break;
          case 1354:
            dataManager.bestFriendAnimalAdoration = reader.ReadSingle();
            break;
          case 1355:
            dataManager.LastAnimalToStarveDay = reader.ReadInt32();
            break;
          case 1356:
            dataManager.GivenNarayanaFollower = reader.ReadBoolean();
            break;
          case 1357:
            dataManager.MapLockCountToUnlock = reader.ReadInt32();
            break;
          case 1358:
            dataManager.MysticKeeperBeatenYngya = reader.ReadBoolean();
            break;
          case 1359:
            dataManager.SpokenToMysticKeeperWinter = reader.ReadBoolean();
            break;
          case 1360:
            dataManager.RevealedBaseYngyaShrine = reader.ReadBoolean();
            break;
          case 1361:
            dataManager.HasPureBloodMatingQuestAccepted = reader.ReadBoolean();
            break;
          case 1362:
            dataManager.FlockadeDecoWoolWon = reader.ReadInt32();
            break;
          case 1363:
            dataManager.FlockadeFlockadeWoolWon = reader.ReadInt32();
            break;
          case 1364:
            dataManager.FlockadeGraveyardWoolWon = reader.ReadInt32();
            break;
          case 1365:
            dataManager.FlockadeRancherWoolWon = reader.ReadInt32();
            break;
          case 1366:
            dataManager.FlockadeTarotWoolWon = reader.ReadInt32();
            break;
          case 1367:
            dataManager.FlockadeBlacksmithWoolWon = reader.ReadInt32();
            break;
          case 1368:
            dataManager.FoundLegendaryDagger = reader.ReadBoolean();
            break;
          case 1369:
            dataManager.FoundLegendaryBlunderbuss = reader.ReadBoolean();
            break;
          case 1370:
            dataManager.Dungeon5Harder = reader.ReadBoolean();
            break;
          case 1371:
            dataManager.Dungeon6Harder = reader.ReadBoolean();
            break;
          case 1372:
            dataManager.EncounteredSabnock = reader.ReadBoolean();
            break;
          case 1373:
            dataManager.ForceSinRoom = reader.ReadBoolean();
            break;
          case 1374:
            dataManager.ForceHeartRoom = reader.ReadBoolean();
            break;
          case 1375:
            dataManager.ForceDragonRoom = reader.ReadBoolean();
            break;
          case 1376:
            dataManager.GivenUpWolfFood = resolver.GetFormatterWithVerify<List<InventoryItem>>().Deserialize(ref reader, options);
            break;
          case 1377:
            dataManager.GivenUpHeartToWolf = reader.ReadBoolean();
            break;
          case 1378:
            dataManager.WoolhavenDecorationCouunt = reader.ReadInt32();
            break;
          case 1379:
            dataManager.FirstRotFollowerAilmentAvoided = reader.ReadBoolean();
            break;
          case 1380:
            dataManager.RemoveBlizzardsBeforeTimestamp = reader.ReadSingle();
            break;
          case 1381:
            dataManager.DisableBlizzard1 = reader.ReadBoolean();
            break;
          case 1382:
            dataManager.DisableBlizzard2 = reader.ReadBoolean();
            break;
          case 1383:
            dataManager.GofernonRotburnProgress = reader.ReadInt32();
            break;
          case 1384:
            dataManager.LastDungeonSeeds = resolver.GetFormatterWithVerify<List<int>>().Deserialize(ref reader, options);
            break;
          case 1385:
            dataManager.FishermanWinterConvo = reader.ReadBoolean();
            break;
          case 1386:
            dataManager.Twitch_Drop_16 = reader.ReadBoolean();
            break;
          case 1387:
            dataManager.Twitch_Drop_17 = reader.ReadBoolean();
            break;
          case 1388:
            dataManager.Twitch_Drop_18 = reader.ReadBoolean();
            break;
          case 1389:
            dataManager.Twitch_Drop_19 = reader.ReadBoolean();
            break;
          case 1390:
            dataManager.Twitch_Drop_20 = reader.ReadBoolean();
            break;
          case 1391:
            dataManager.RevealedWolfNode = reader.ReadBoolean();
            break;
          case 1392:
            dataManager.PreviousRelic = resolver.GetFormatterWithVerify<RelicType>().Deserialize(ref reader, options);
            break;
          case 1393:
            dataManager.FirstDungeon6RescueRoom = reader.ReadBoolean();
            break;
          case 1394:
            dataManager.WoolhavenSkinsPurchased = reader.ReadInt32();
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return dataManager;
    }
  }
}
