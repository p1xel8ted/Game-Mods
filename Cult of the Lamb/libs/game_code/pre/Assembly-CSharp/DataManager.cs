// Decompiled with JetBrains decompiler
// Type: DataManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using Lamb.UI.DeathScreen;
using Map;
using System;
using System.Collections.Generic;
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
[XmlInclude(typeof (Objectives_BedRest))]
[XmlInclude(typeof (Objectives_RemoveStructure))]
[XmlInclude(typeof (Objectives_ShootDummy))]
[XmlInclude(typeof (Objectives_TalkToFollower))]
[XmlInclude(typeof (Objectives_CookMeal))]
[XmlInclude(typeof (Objectives_PlaceStructure))]
[XmlInclude(typeof (Objectives_PerformRitual))]
[XmlInclude(typeof (Objectives_UnlockUpgrade))]
[XmlInclude(typeof (Objectives_EatMeal))]
[XmlInclude(typeof (Objectives_RecruitCursedFollower))]
[XmlInclude(typeof (Objectives_FindFollower))]
[XmlInclude(typeof (ObjectivesDataFinalized))]
[XmlInclude(typeof (Objectives_Custom.FinalizedData_Custom))]
[XmlInclude(typeof (Objectives_CollectItem.FinalizedData_CollectItem))]
[XmlInclude(typeof (Objectives_BuildStructure.FinalizedData_BuildStructure))]
[XmlInclude(typeof (Objectives_RecruitFollower.FinalizedData_RecruitFollower))]
[XmlInclude(typeof (Objectives_DepositFood.FinalizedData_DepositFood))]
[XmlInclude(typeof (Objectives_KillEnemies.FinalizedData_KillEnemies))]
[XmlInclude(typeof (Objectives_NoDodge.FinalizedData_NoDodge))]
[XmlInclude(typeof (Objectives_NoCurses.FinalizedData_NoCurses))]
[XmlInclude(typeof (Objectives_NoDamage.FinalizedData_NoDamage))]
[XmlInclude(typeof (Objectives_NoHealing.FinalizedData_NoHealing))]
[XmlInclude(typeof (Objectives_BedRest.FinalizedData_BedRest))]
[XmlInclude(typeof (Objectives_RemoveStructure.FinalizedData_RemoveStructure))]
[XmlInclude(typeof (Objectives_ShootDummy.FinalizedData_ShootDummy))]
[XmlInclude(typeof (Objectives_TalkToFollower.FinalizedData_TalkToFollower))]
[XmlInclude(typeof (Objectives_CookMeal.FinalizedData_CookMeal))]
[XmlInclude(typeof (Objectives_PlaceStructure.FinalizedData_PlaceStructure))]
[XmlInclude(typeof (Objectives_PerformRitual.FinalizedData_PerformRitual))]
[XmlInclude(typeof (Objectives_UnlockUpgrade.FinalizedData_UnlockUpgrade))]
[XmlInclude(typeof (Objectives_EatMeal.FinalizedData_EatMeal))]
[XmlInclude(typeof (Objectives_RecruitCursedFollower.FinalizedData_RecruitCursedFollower))]
[XmlInclude(typeof (Objectives_FindFollower.FinalizedData_FindFollower))]
[XmlInclude(typeof (FinalizedNotification))]
[XmlInclude(typeof (FinalizedFaithNotification))]
[XmlInclude(typeof (FinalizedFollowerNotification))]
[XmlInclude(typeof (FinalizedRelationshipNotification))]
[XmlInclude(typeof (FinalizedItemNotification))]
public class DataManager
{
  [XmlIgnore]
  [NonSerialized]
  public MetaData MetaData;
  public bool AllowSaving = true;
  public bool PauseGameTime;
  public bool GameOverEnabled;
  public bool DisplayGameOverWarning;
  public bool InGameOver;
  public bool GameOver;
  public bool DifficultyChosen;
  public bool DifficultyReminded;
  public int playerDeaths;
  public int playerDeathsInARow;
  public int playerDeathsInARowFightingLeader;
  public int dungeonRun = -1;
  public float dungeonRunDuration;
  public List<NodeType> dungeonVisitedRooms = new List<NodeType>()
  {
    NodeType.FirstFloor
  };
  public List<int> FollowersRecruitedInNodes = new List<int>();
  public int FollowersRecruitedThisNode;
  public float TimeInGame;
  public int KillsInGame;
  public int dungeonRunXPOrbs;
  public int ChestRewardCount = -1;
  public bool BaseGoopDoorLocked;
  public string BaseGoopDoorLoc = "";
  public int STATS_FollowersStarvedToDeath;
  public int STATS_Murders;
  public int STATS_Sacrifices;
  public int STATS_NaturalDeaths;
  public int PlayerKillsOnRun;
  public int PlayerStartingBlackSouls;
  public bool GivenFollowerHearts;
  public bool EnabledSpells = true;
  public bool ForceDoctrineStones = true;
  public int DoctrineStoneTotalCount;
  public bool BuildShrineEnabled = true;
  public bool EnabledHealing = true;
  public bool EnabledSword = true;
  public bool BonesEnabled = true;
  public bool XPEnabled = true;
  public bool ShownDodgeTutorial = true;
  public bool ShownInventoryTutorial = true;
  public int ShownDodgeTutorialCount;
  public bool HadInitialDeathCatConversation = true;
  public bool PlayerHasBeenGivenHearts = true;
  public int TotalFirefliesCaught;
  public int TotalSquirrelsCaught;
  public DataManager.OnboardingPhase CurrentOnboardingPhase;
  public bool firstRecruit;
  public int MissionariesCompleted;
  public int PlayerFleece;
  public List<int> UnlockedFleeces = new List<int>();
  public List<ThoughtData> Thoughts = new List<ThoughtData>();
  public bool CanReadMinds = true;
  public bool HappinessEnabled;
  public bool TeachingsEnabled;
  public bool SchedulingEnabled;
  public bool PrayerEnabled;
  public bool PrayerOrdered;
  public bool HasBuiltCookingFire;
  public bool HasBuiltFarmPlot;
  public bool HasBuiltTemple1;
  public bool HasBuiltTemple2;
  public bool HasBuiltTemple3;
  public bool HasBuiltTemple4;
  public bool HasBuiltShrine1 = true;
  public bool HasBuiltShrine2;
  public bool HasBuiltShrine3;
  public bool HasBuiltShrine4;
  public bool PerformedMushroomRitual;
  public bool BuiltMushroomDecoration;
  public bool HasBuiltSurveillance;
  public int TempleDevotionBoxCoinCount;
  public bool CanBuildShrine;
  public int WokeUpEveryoneDay = -1;
  public bool DiedLastRun;
  public UIDeathScreenOverlayController.Results LastRunResults = UIDeathScreenOverlayController.Results.None;
  public float LastFollowerToStarveToDeath;
  public float LastFollowerToStartStarving;
  public float LastFollowerToStartDissenting;
  public float LastFollowerToReachOldAge;
  public float LastFollowerToBecomeIll;
  public float LastFollowerToBecomeIllFromSleepingNearIllFollower;
  public float LastFollowerToPassOut;
  public int LastFollowerPurchasedFromSpider = -1;
  public float TimeSinceFaithHitEmpty = -1f;
  public float TimeSinceLastCrisisOfFaithQuest;
  public int JudgementAmount;
  public float HungerBarCount;
  public float IllnessBarCount;
  public float IllnessBarDynamicMax;
  public float StaticFaith = 55f;
  public float CultFaith;
  public float LastBrainwashed = float.MinValue;
  public float LastHolidayDeclared = float.MinValue;
  public float LastWorkThroughTheNight = float.MinValue;
  public float LastConstruction = float.MinValue;
  public float LastEnlightenment = float.MinValue;
  public float LastFastDeclared = float.MinValue;
  public float LastFeastDeclared = float.MinValue;
  public float LastFishingDeclared = float.MinValue;
  public float LastHalloween = float.MinValue;
  public float LastArcherShot = float.MinValue;
  public float LastSimpleGuardianAttacked = float.MinValue;
  public float LastSimpleGuardianRingProjectiles = float.MinValue;
  public int LastDayTreesAtBase = -1;
  public int PreviousSermonDayIndex = -1;
  public SermonCategory PreviousSermonCategory;
  public int ShrineLevel;
  public bool GivenSermonQuest;
  public bool GivenFaithOfFlockQuest;
  public bool PrayedAtMassiveMonsterShrine;
  public string TwitchSecretKey;
  public string ChannelID;
  public string ChannelName;
  public int TwitchTotemsCompleted;
  public float TwitchNextHHEvent = -1f;
  public List<string> TwitchFollowerViewerIDs = new List<string>();
  public List<string> TwitchFollowerIDs = new List<string>();
  public bool OnboardingFinished;
  public string SaveUniqueID = "";
  public List<string> enemiesEncountered = new List<string>();
  public bool Chain1;
  public bool Chain2;
  public bool Chain3;
  public int DoorRoomChainProgress = -1;
  public int DoorRoomDoorsProgress = -1;
  public int Dungeon1_Layer = 1;
  public int Dungeon2_Layer = 1;
  public int Dungeon3_Layer = 1;
  public int Dungeon4_Layer = 1;
  public bool First_Dungeon_Resurrecting = true;
  public int SpidersCaught;
  public List<MiniBossController.MiniBossData> MiniBossData = new List<MiniBossController.MiniBossData>();
  public List<DataManager.LocationAndLayer> CachePreviousRun = new List<DataManager.LocationAndLayer>();
  public List<FollowerLocation> DiscoveredLocations = new List<FollowerLocation>();
  public List<FollowerLocation> VisitedLocations = new List<FollowerLocation>();
  public List<FollowerLocation> NewLocationFaithReward = new List<FollowerLocation>();
  public List<FollowerLocation> DissentingFolllowerRooms = new List<FollowerLocation>();
  [XmlIgnore]
  [NonSerialized]
  public FollowerLocation CurrentLocation = FollowerLocation.Base;
  public List<DataManager.LocationAndLayer> OpenedDungeonDoors = new List<DataManager.LocationAndLayer>();
  public List<string> KeyPiecesFromLocation = new List<string>();
  public List<FollowerLocation> UsedFollowerDispensers = new List<FollowerLocation>();
  public List<FollowerLocation> UnlockedBossTempleDoor = new List<FollowerLocation>();
  public List<FollowerLocation> UnlockedDungeonDoor = new List<FollowerLocation>();
  public List<FollowerLocation> BossesCompleted = new List<FollowerLocation>();
  public List<FollowerLocation> BossesEncountered = new List<FollowerLocation>();
  public List<FollowerLocation> DoorRoomBossLocksDestroyed = new List<FollowerLocation>();
  public List<FollowerLocation> SignPostsRead = new List<FollowerLocation>();
  public bool ShrineDoor;
  public bool BaseDoorEast;
  public bool BaseDoorNorthEast;
  public bool BaseDoorNorthWest;
  public bool BossForest;
  public bool ForestTempleDoor;
  public List<int> CompletedQuestFollowerIDs = new List<int>();
  public DataManager.CultLevel CurrentCultLevel;
  public List<UnlockManager.UnlockType> MechanicsUnlocked = new List<UnlockManager.UnlockType>();
  public List<SermonsAndRituals.SermonRitualType> UnlockedSermonsAndRituals = new List<SermonsAndRituals.SermonRitualType>();
  public List<StructureBrain.TYPES> UnlockedStructures = new List<StructureBrain.TYPES>();
  public List<StructureBrain.TYPES> HistoryOfStructures = new List<StructureBrain.TYPES>();
  public bool NewBuildings;
  public List<TutorialTopic> RevealedTutorialTopics = new List<TutorialTopic>();
  public List<StructuresData.ResearchObject> CurrentResearch = new List<StructuresData.ResearchObject>();
  public UpgradeTreeNode.TreeTier CurrentUpgradeTreeTier;
  public UpgradeTreeNode.TreeTier CurrentPlayerUpgradeTreeTier;
  public UpgradeSystem.Type MostRecentTreeUpgrade = UpgradeSystem.Type.Building_Temple;
  public UpgradeSystem.Type MostRecentPlayerTreeUpgrade = UpgradeSystem.Type.PUpgrade_Heart_1;
  public List<UpgradeSystem.Type> UnlockedUpgrades = new List<UpgradeSystem.Type>();
  public List<DoctrineUpgradeSystem.DoctrineType> DoctrineUnlockedUpgrades = new List<DoctrineUpgradeSystem.DoctrineType>();
  public List<UpgradeSystem.UpgradeCoolDown> UpgradeCoolDowns = new List<UpgradeSystem.UpgradeCoolDown>();
  public List<FollowerTrait.TraitType> CultTraits = new List<FollowerTrait.TraitType>();
  public List<string> WeaponUnlockedUpgrades = new List<string>();
  public string CultName;
  public bool DungeonBossFight;
  public List<DataManager.LocationSeedsData> LocationSeeds = new List<DataManager.LocationSeedsData>();
  public float TempleStudyXP;
  public int UnlockededASermon;
  public int CurrentDayIndex = 1;
  public int CurrentPhaseIndex;
  public float CurrentGameTime;
  public int[] LastUsedSermonRitualDayIndex = new int[0];
  public int[] ScheduledActivityIndexes = new int[5]
  {
    0,
    0,
    0,
    0,
    2
  };
  public int OverrideScheduledActivity = -1;
  public int[] InstantActivityIndexes = new int[1]{ 8 };
  public bool PlayerEaten;
  public ResurrectionType ResurrectionType;
  public bool FirstTimeResurrecting = true;
  public bool FirstTimeFertilizing = true;
  public bool FirstTimeChop;
  public bool FirstTimeMine;
  public StructureBrain.Categories currentCategory;
  public float TimeSinceLastComplaint;
  public float TimeSinceLastQuest;
  public int DessentingFollowerChoiceQuestionIndex;
  public int HaroConversationIndex;
  public bool HaroConversationCompleted;
  public bool RatauKilled;
  public bool RatauReadLetter;
  public FollowerLocation CurrentFoxLocation = FollowerLocation.None;
  public int CurrentFoxEncounter;
  public List<FollowerLocation> FoxIntroductions = new List<FollowerLocation>();
  public List<FollowerLocation> FoxCompleted = new List<FollowerLocation>();
  public int PlimboStoryProgress;
  public int RatooFishingProgress;
  public bool RatooFishing_FISH_CRAB;
  public bool RatooFishing_FISH_LOBSTER;
  public bool RatooFishing_FISH_OCTOPUS;
  public bool RatooFishing_FISH_SQUID;
  public bool PlayerHasFollowers;
  public int FishCaughtTotal;
  public bool RatooGivenHeart;
  public bool RatooMentionedWrongHeart;
  public bool ShownInitialTempleDoorSeal;
  public bool FirstFollowerSpawnInteraction = true;
  public List<int> DecorationTypesBuilt = new List<int>();
  public List<TarotCards.Card> WeaponSelectionPositions = new List<TarotCards.Card>();
  public bool ShowCultFaith = true;
  public bool ShowCultIllness = true;
  public bool ShowCultHunger = true;
  public bool ShowLoyaltyBars = true;
  public bool IntroDoor1;
  public bool FirstDoctrineStone;
  public bool ShowHaroDoctrineStoneRoom;
  public bool HaroIntroduceDoctrines;
  public bool RatExplainDungeon = true;
  public bool RatauToGiveCurseNextRun;
  public int SozoStoryProgress = -1;
  public bool MidasBankUnlocked;
  public bool MidasBankIntro;
  public bool MidasSacrificeIntro;
  public bool MidasIntro;
  public bool MidasDevotionIntro;
  public bool MidasStatue;
  public float MidasDevotionCost = 1f;
  public int MidasDevotionLastUsed;
  public int MidasFollowerStatueCount;
  public bool RatauShowShrineShop;
  public bool DecorationRoomFirstConvo;
  public bool FirstTarot;
  public bool Tutorial_Night;
  public bool Tutorial_ReturnToDungeon;
  public bool FirstTimeInDungeon;
  public bool AllowBuilding = true;
  public bool CookedFirstFood = true;
  public bool Dungeon1Story1;
  public bool Dungeon1Story2;
  public bool FirstFollowerRescue;
  public bool FirstDungeon1RescueRoom;
  public bool FirstDungeon2RescueRoom;
  public bool FirstDungeon3RescueRoom;
  public bool FirstDungeon4RescueRoom;
  public bool SherpaFirstConvo;
  public bool ResourceRoom1Revealed;
  public bool EncounteredHealingRoom;
  public bool MinimumRandomRoomsEncountered;
  public int MinimumRandomRoomsEncounteredAmount;
  public bool ForneusLore;
  public bool SozoBeforeDeath;
  public bool SozoDead;
  public bool FirstTimeWeaponMarketplace;
  public bool FirstTimeSpiderMarketplace;
  public bool FirstTimeSeedMarketplace;
  public bool ShowFirstDoctrineStone = true;
  public bool RatauGiftMediumCollected;
  public bool CompletedLighthouseCrystalQuest;
  public bool CameFromDeathCatFight;
  public bool OldFollowerSpoken;
  public int CultLeader1_LastRun = -1;
  public int CultLeader1_StoryPosition;
  public int CultLeader2_LastRun = -1;
  public int CultLeader2_StoryPosition;
  public int CultLeader3_LastRun = -1;
  public int CultLeader3_StoryPosition;
  public int CultLeader4_LastRun = -1;
  public int CultLeader4_StoryPosition;
  public int DeathCatConversationLastRun = -999;
  public int DeathCatStory;
  public int DeathCatDead;
  public int DeathCatWon;
  public bool DeathCatBoss1;
  public bool DeathCatBoss2;
  public bool DeathCatBoss3;
  public bool DeathCatBoss4;
  public bool DeathCatRatauKilled;
  public bool DungeonKeyRoomCompleted1;
  public bool DungeonKeyRoomCompleted2;
  public bool DungeonKeyRoomCompleted3;
  public bool DungeonKeyRoomCompleted4;
  public bool RatOutpostIntro;
  public bool FirstMonsterHeart;
  public bool Rat_Tutorial_Bell;
  public bool Goat_First_Meeting;
  public bool Goat_Guardian_Door_Open;
  public bool Key_Shrine_1;
  public bool Key_Shrine_2;
  public bool Key_Shrine_3;
  public bool InTutorial = true;
  public bool UnlockBaseTeleporter = true;
  public bool Tutorial_First_Indoctoring;
  public bool Tutorial_Second_Enter_Base = true;
  public bool Tutorial_Rooms_Completed;
  public bool Tutorial_Enable_Store_Resources;
  public bool Tutorial_Completed;
  public bool Tutorial_Mission_Board;
  public bool Create_Tutorial_Rooms;
  public bool RatauExplainsFollowers;
  public bool RatauExplainsDemo;
  public bool RatauExplainsBiome0;
  public bool RatauExplainsBiome1;
  public bool RatauExplainsBiome0Boss;
  public bool RatauExplainsTeleporter;
  public bool SozoIntro;
  public bool SozoDecorationQuestActive;
  public bool SozoQuestComplete;
  public bool CollectedMenticide;
  public bool TarotIntro;
  public bool HasTarotBuilding = true;
  public bool ForestOfferingRoomCompleted;
  public bool KnucklebonesIntroCompleted;
  public bool KnucklebonesFirstGameRatauStart = true;
  public bool ForestChallengeRoom1Completed;
  public bool ForestRescueWorshipper;
  public bool GetFirstFollower;
  public bool BeatenFirstMiniBoss;
  public bool RatauExplainBuilding;
  public bool PromoteFollowerExplained;
  public bool HasMadeFirstOffering;
  public bool BirdConvo;
  public bool UnlockedHubShore;
  public bool GivenFollowerGift;
  public bool FinalBossSlowWalk = true;
  public int HadNecklaceOnRun;
  public bool ShownDungeon1FinalLeaderEncounter;
  public bool ShownDungeon2FinalLeaderEncounter;
  public bool ShownDungeon3FinalLeaderEncounter;
  public bool ShownDungeon4FinalLeaderEncounter;
  public bool HaroOnbardedHarderDungeon1;
  public bool HaroOnbardedHarderDungeon2;
  public bool HaroOnbardedHarderDungeon3;
  public bool HaroOnbardedHarderDungeon4;
  public bool RevealOfferingChest;
  public bool OnboardedOfferingChest;
  public bool OnboardedHomeless = true;
  public bool OnboardedHomelessAtNight;
  public bool OnboardedEndlessMode;
  public bool OnboardedDeadFollower;
  public bool OnboardedBuildingHouse;
  public bool OnboardedMakingMoreFood;
  public bool OnboardedCleaningBase;
  public bool OnboardedOldFollower;
  public bool OnboardedSickFollower;
  public bool OnboardedStarvingFollower;
  public bool OnboardedDissenter;
  public bool OnboardedFaithOfFlock;
  public bool OnboardedRaiseFaith;
  public bool OnboardedResourceYard;
  public bool OnboardedCrisisOfFaith;
  public bool OnboardedHalloween;
  public bool OnboardedSermon;
  public bool OnboardedBuildFarm;
  public bool OnboardedRefinery;
  public bool OnboardedCultName;
  public bool OnboardedZombie;
  public bool OnboardedLoyalty;
  public bool HasMetChefShop;
  public int CurrentOnboardingFollowerID = -1;
  public int CurrentOnboardingFollowerType = -1;
  public string CurrentOnboardingFollowerTerm;
  public bool HasPerformedRitual;
  public int GivenLoyaltyQuestDay = -1;
  public int LastDaySincePlayerUpgrade = -1;
  public int MealsCooked;
  public bool Dungeon1_1_Key;
  public bool Dungeon1_2_Key;
  public bool Dungeon1_3_Key;
  public bool Dungeon1_4_Key;
  public bool Dungeon2_1_Key;
  public bool Dungeon2_2_Key;
  public bool Dungeon2_3_Key;
  public bool Dungeon2_4_Key;
  public bool Dungeon3_1_Key;
  public bool Dungeon3_2_Key;
  public bool Dungeon3_3_Key;
  public bool Dungeon3_4_Key;
  public bool HadFirstTempleKey;
  public int CurrentKeyPieces;
  public bool GivenFreeDungeonFollower;
  public bool GivenFreeDungeonGold;
  public bool FoxMeeting_0;
  public bool GaveFollowerToFox;
  public bool Ritual_0;
  public bool Ritual_1;
  public bool Lighthouse_FirstConvo;
  public bool Lighthouse_LitFirstConvo;
  public bool Lighthouse_FireOutAgain;
  public bool Lighthouse_QuestGiven;
  public bool Lighthouse_QuestComplete;
  public int LighthouseFuel;
  public bool Lighthouse_Lit;
  public bool ShoreFishFirstConvo;
  public bool ShoreFishFished;
  public bool ShoreTarotShotConvo1;
  public bool ShoreTarotShotConvo2;
  public bool ShoreFlowerShopConvo1;
  public bool SozoFlowerShopConvo1;
  public bool SozoTarotShopConvo1;
  public bool RatauFoundSkin;
  public bool MidasFoundSkin;
  public bool SozoFoundDecoration;
  public bool HorseTown_PaidRespectToHorse;
  public bool HorseTown_JoinCult;
  public bool HorseTown_OpenedChest;
  public bool BlackSoulsEnabled = true;
  public bool PlacedRubble;
  public bool DefeatedExecutioner;
  public int WorldMapCurrentSelection;
  public int RedHeartsTemporarilyRemoved;
  public bool ShownKnuckleboneTutorial;
  public bool Knucklebones_Opponent_Ratau_Won;
  public int ShopKeeperChefState;
  public int ShopKeeperChefEnragedDay;
  public bool Knucklebones_Opponent_0;
  public bool Knucklebones_Opponent_0_FirstConvoRataus;
  public bool Knucklebones_Opponent_0_Won;
  public bool Knucklebones_Opponent_1;
  public bool Knucklebones_Opponent_1_FirstConvoRataus;
  public bool Knucklebones_Opponent_1_Won;
  public bool Knucklebones_Opponent_2;
  public bool Knucklebones_Opponent_2_FirstConvoRataus;
  public bool Knucklebones_Opponent_2_Won;
  public bool DungeonLayer1;
  public bool DungeonLayer2;
  public bool DungeonLayer3;
  public bool DungeonLayer4;
  public bool DungeonLayer5;
  public bool BeatenDungeon1;
  public bool BeatenDungeon2;
  public bool BeatenDungeon3;
  public bool BeatenDungeon4;
  public bool BeatenDeathCat;
  public bool CanFindTarotCards = true;
  public float LuckMultiplier = 1f;
  public float EnemyModifiersChanceMultiplier = 1f;
  public float EnemyHealthMultiplier = 1f;
  public float ProjectileMoveSpeedMultiplier = 1f;
  public float DodgeDistanceMultiplier = 1f;
  public float CurseFeverMultiplier = 1f;
  public bool SpawnPoisonOnAttack;
  public bool EnemiesInNextRoomHaveModifiers;
  public Vector2 CurrentRoomCoordinates;
  public int ResurrectRitualCount;
  public bool EncounteredGambleRoom;
  public int SwordLevel;
  public int DaggerLevel;
  public int AxeLevel;
  public int HammerLevel;
  public int GauntletLevel;
  public int FireballLevel;
  public int EnemyBlastLevel;
  public int MegaSlashLevel;
  public int ProjectileAOELevel;
  public int TentaclesLevel;
  public int VortexLevel;
  public float LastFollowerQuestGivenTime;
  public bool DLC_Pre_Purchase;
  public bool DLC_Cultist_Pack;
  public bool DLC_Plush_Bonus;
  public bool Twitch_Drop_1;
  public bool Twitch_Drop_2;
  public bool Twitch_Drop_3;
  public bool Twitch_Drop_4;
  public bool Twitch_Drop_5;
  public bool DeathCatBeaten;
  public bool HasEncounteredTarot;
  public List<InventoryItem.ITEM_TYPE> RecentRecipes = new List<InventoryItem.ITEM_TYPE>();
  public List<InventoryItem.ITEM_TYPE> RecipesDiscovered = new List<InventoryItem.ITEM_TYPE>();
  private float playerDamageDealt;
  public bool ChefShopDoublePrices;
  public int FollowerShopUses;
  public int sacrificesCompleted;
  public List<InventoryItem.ITEM_TYPE> FoundItems = new List<InventoryItem.ITEM_TYPE>();
  public bool TakenBossDamage;
  public int PoopMealsCreated;
  public bool PrayedAtCrownShrine;
  public bool ShellsGifted_0;
  public bool ShellsGifted_1;
  public bool ShellsGifted_2;
  public bool ShellsGifted_3;
  public bool ShellsGifted_4;
  public int DateLastScreenshot = -1;
  public float PlayerDamageDealtThisRun;
  public float PlayerDamageReceivedThisRun;
  private float playerDamageReceived;
  public bool Options_ScreenShake = true;
  public static System.Random RandomSeed = new System.Random(UnityEngine.Random.Range(int.MinValue, int.MaxValue));
  public static bool UseDataManagerSeed = false;
  public bool PlayerIsASpirit;
  public bool BridgeFixed;
  public bool BuildingTome;
  public bool BeenToDungeon;
  public int FollowerID;
  public int ObjectiveGroupID;
  public List<FollowerInfo> Followers = new List<FollowerInfo>();
  public List<FollowerInfo> Followers_Recruit = new List<FollowerInfo>();
  public List<FollowerInfo> Followers_Dead = new List<FollowerInfo>();
  public List<int> Followers_Dead_IDs = new List<int>();
  public int StructureID;
  public List<StructuresData> BaseStructures = new List<StructuresData>();
  public List<StructuresData> HubStructures = new List<StructuresData>();
  public List<StructuresData> HubShoreStructures = new List<StructuresData>();
  public List<StructuresData> Hub1_MainStructures = new List<StructuresData>();
  public List<StructuresData> Hub1_BerriesStructures = new List<StructuresData>();
  public List<StructuresData> Hub1_ForestStructures = new List<StructuresData>();
  public List<StructuresData> Hub1_RatauInsideStructures = new List<StructuresData>();
  public List<StructuresData> Hub1_RatauOutsideStructures = new List<StructuresData>();
  public List<StructuresData> Hub1_SozoStructures = new List<StructuresData>();
  public List<StructuresData> Hub1_SwampStructures = new List<StructuresData>();
  public List<StructuresData> Dungeon_Logs1Structures = new List<StructuresData>();
  public List<StructuresData> Dungeon_Logs2Structures = new List<StructuresData>();
  public List<StructuresData> Dungeon_Logs3Structures = new List<StructuresData>();
  public List<StructuresData> Dungeon_Food1Structures = new List<StructuresData>();
  public List<StructuresData> Dungeon_Food2Structures = new List<StructuresData>();
  public List<StructuresData> Dungeon_Food3Structures = new List<StructuresData>();
  public List<StructuresData> Dungeon_Stone1Structures = new List<StructuresData>();
  public List<StructuresData> Dungeon_Stone2Structures = new List<StructuresData>();
  public List<StructuresData> Dungeon_Stone3Structures = new List<StructuresData>();
  public List<int> Followers_Imprisoned_IDs = new List<int>();
  public List<int> Followers_Elderly_IDs = new List<int>();
  public List<int> Followers_OnMissionary_IDs = new List<int>();
  public List<int> Followers_Transitioning_IDs = new List<int>();
  public List<int> Followers_Demons_IDs = new List<int>();
  public List<int> Followers_Demons_Types = new List<int>();
  public List<SeasonalEventType> ActiveSeasonalEvents = new List<SeasonalEventType>();
  public List<StructureBrain.TYPES> RevealedStructures = new List<StructureBrain.TYPES>();
  public List<DayObject> DayList = new List<DayObject>();
  public DayObject CurrentDay;
  public List<string> TrackedObjectiveGroupIDs = new List<string>();
  public List<ObjectivesData> Objectives = new List<ObjectivesData>();
  public List<ObjectivesData> CompletedObjectives = new List<ObjectivesData>();
  public List<ObjectivesData> FailedObjectives = new List<ObjectivesData>();
  public List<ObjectivesData> DungeonObjectives = new List<ObjectivesData>();
  public List<StoryData> StoryObjectives = new List<StoryData>();
  public List<ObjectivesDataFinalized> CompletedObjectivesHistory = new List<ObjectivesDataFinalized>();
  public List<ObjectivesDataFinalized> FailedObjectivesHistory = new List<ObjectivesDataFinalized>();
  public List<DataManager.QuestHistoryData> CompletedQuestsHistorys = new List<DataManager.QuestHistoryData>();
  public InventoryItem.ITEM_TYPE SimpleInventoryItem;
  public List<InventoryItem> items = new List<InventoryItem>();
  public int IngredientsCapacityLevel;
  public static List<int> IngredientsCapacity = new List<int>()
  {
    150,
    50,
    100
  };
  public int FoodCapacityLevel;
  public static List<int> FoodCapacity = new List<int>()
  {
    150,
    50,
    100
  };
  public int LogCapacityLevel;
  public static List<int> LogCapacity = new List<int>()
  {
    150,
    50,
    100
  };
  public int StoneCapacityLevel;
  public static List<int> StoneCapacity = new List<int>()
  {
    150,
    50,
    100
  };
  [XmlIgnore]
  [NonSerialized]
  public static Action<string> OnSkinUnlocked;
  public List<string> FollowerSkinsUnlocked = new List<string>()
  {
    "Cat",
    "Dog",
    "Pig",
    "Deer",
    "Fox"
  };
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
    "Rhino",
    "Racoon",
    "Badger",
    "DeerSkull",
    "BatDemon",
    "Crow"
  };
  public List<string> DLCSkins = new List<string>()
  {
    "TwitchPoggers",
    "TwitchDog",
    "TwitchDogAlt",
    "Lion",
    "Penguin",
    "TwitchMouse",
    "TwitchCat",
    "Cthulhu",
    "Bee",
    "Tapir",
    "Turtle",
    "Monkey",
    "Narwal"
  };
  public string[] SpecialEventSkins = new string[3]
  {
    "DeerSkull",
    "BatDemon",
    "Crow"
  };
  public List<StructureEffect> StructureEffects = new List<StructureEffect>();
  public List<string> KilledBosses = new List<string>();
  public static List<InventoryItem.ITEM_TYPE> AllNecklaces = new List<InventoryItem.ITEM_TYPE>()
  {
    InventoryItem.ITEM_TYPE.Necklace_1,
    InventoryItem.ITEM_TYPE.Necklace_2,
    InventoryItem.ITEM_TYPE.Necklace_3,
    InventoryItem.ITEM_TYPE.Necklace_4,
    InventoryItem.ITEM_TYPE.Necklace_5
  };
  public static List<InventoryItem.ITEM_TYPE> AllGifts = new List<InventoryItem.ITEM_TYPE>()
  {
    InventoryItem.ITEM_TYPE.GIFT_SMALL,
    InventoryItem.ITEM_TYPE.GIFT_MEDIUM
  };
  public static Action<EquipmentType> OnWeaponUnlocked;
  public List<EquipmentType> WeaponPool = new List<EquipmentType>();
  public EquipmentType CurrentWeapon = EquipmentType.None;
  public int CurrentWeaponLevel;
  public int CurrentRunWeaponLevel;
  public EquipmentType ForcedStartingWeapon = EquipmentType.None;
  public EquipmentType CurrentCurse = EquipmentType.None;
  public int CurrentCurseLevel;
  public int CurrentRunCurseLevel;
  public EquipmentType ForcedStartingCurse = EquipmentType.None;
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
    TarotCards.Card.Arrows
  };
  public List<TarotCards.TarotCard> PlayerRunTrinkets = new List<TarotCards.TarotCard>();
  public List<TarotCards.Card> PlayerFoundTrinkets = new List<TarotCards.Card>();
  public List<CrownAbilities> CrownAbilitiesUnlocked = new List<CrownAbilities>();
  public static List<BluePrint.BluePrintType> AllBluePrints = new List<BluePrint.BluePrintType>()
  {
    BluePrint.BluePrintType.TREE,
    BluePrint.BluePrintType.STONE,
    BluePrint.BluePrintType.PATH_DIRT
  };
  public List<BluePrint> PlayerBluePrints = new List<BluePrint>();
  public List<InventoryItem.ITEM_TYPE> FishCaught = new List<InventoryItem.ITEM_TYPE>();
  public List<MissionManager.Mission> ActiveMissions = new List<MissionManager.Mission>();
  public List<MissionManager.Mission> AvailableMissions = new List<MissionManager.Mission>();
  public float NewMissionDayTimestamp = -1f;
  public int LastGoldenMissionDay = -1;
  public bool MissionShrineUnlocked;
  public List<BuyEntry> ItemsForSale = new List<BuyEntry>();
  public List<ShopLocationTracker> Shops = new List<ShopLocationTracker>();
  public int LastDayUsedFollowerShop = -1;
  public FollowerInfo FollowerForSale;
  public MidasDonation midasDonation;
  public int LastDayUsedBank = -1;
  public JellyFishInvestment Investment;
  public List<TraderTracker> Traders = new List<TraderTracker>();
  public List<ShrineUsageInfo> ShrineTimerInfo = new List<ShrineUsageInfo>();
  public static List<int> RedHeartShrineCosts = new List<int>()
  {
    50,
    250,
    500,
    1000
  };
  public int RedHeartShrineLevel;
  public int ShrineHeart;
  public int ShrineCurses;
  public int ShrineVoodo;
  public int ShrineAstrology;
  public List<ItemSelector.Category> ItemSelectorCategories = new List<ItemSelector.Category>();
  public List<InventoryItem> itemsDungeon = new List<InventoryItem>();
  public float DUNGEON_TIME;
  public float PLAYER_RUN_DAMAGE_LEVEL;
  public int PLAYER_HEARTS_LEVEL;
  public int PLAYER_DAMAGE_LEVEL;
  public float PLAYER_HEALTH = 6f;
  public float PLAYER_TOTAL_HEALTH = 6f;
  public float PLAYER_BLUE_HEARTS;
  public float PLAYER_BLACK_HEARTS;
  public float PLAYER_SPIRIT_HEARTS;
  public float PLAYER_SPIRIT_TOTAL_HEARTS;
  public float PLAYER_SPECIAL_CHARGE;
  public float PLAYER_SPECIAL_AMMO;
  public float PLAYER_SPECIAL_CHARGE_TARGET = 10f;
  public int PLAYER_ARROW_AMMO = 3;
  public int PLAYER_ARROW_TOTAL_AMMO = 3;
  public int PLAYER_SPIRIT_AMMO;
  public int PLAYER_SPIRIT_TOTAL_AMMO;
  public int PLAYER_REDEAL_TOKEN;
  public int PLAYER_REDEAL_TOKEN_TOTAL;
  public int PLAYER_HEALTH_MODIFIED;
  public int PLAYER_STARTING_HEALTH_CACHED = -1;
  public int Souls;
  public int BlackSouls;
  public int BlackSoulTarget;
  public int FollowerTokens;
  public float DiscipleXP;
  public int DiscipleLevel;
  public int DiscipleAbilityPoints;
  public static List<float> TargetDiscipleXP = new List<float>()
  {
    3f,
    2f,
    3f,
    4f
  };
  public int XP;
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
  public int AbilityPoints;
  public int WeaponAbilityPoints = 100;
  public float Doctrine_PlayerUpgrade_XP;
  public int Doctrine_PlayerUpgrade_Level;
  public float Doctrine_Special_XP;
  public int Doctrine_Special_Level;
  public float Doctrine_WorkWorship_XP;
  public int Doctrine_WorkWorship_Level;
  public float Doctrine_Possessions_XP;
  public int Doctrine_Possessions_Level;
  public float Doctrine_Food_XP;
  public int Doctrine_Food_Level;
  public float Doctrine_Afterlife_XP;
  public int Doctrine_Afterlife_Level;
  public float Doctrine_LawAndOrder_XP;
  public int Doctrine_LawAndOrder_Level;
  public int CompletedDoctrineStones;
  public int DoctrineCurrentCount;
  public int DoctrineTargetCount;
  public int FRUIT_LOW_MEALS_COOKED;
  public int VEGETABLE_LOW_MEALS_COOKED;
  public int VEGETABLE_MEDIUM_MEALS_COOKED;
  public int VEGETABLE_HIGH_MEALS_COOKED;
  public int FISH_LOW_MEALS_COOKED;
  public int FISH_MEDIUM_MEALS_COOKED;
  public int FISH_HIGH_MEALS_COOKED;
  public int MEAT_LOW_COOKED;
  public int MEAT_MEDIUM_COOKED;
  public int MEAT_HIGH_COOKED;
  public int MIXED_LOW_COOKED;
  public int MIXED_MEDIUM_COOKED;
  public int MIXED_HIGH_COOKED;
  public int POOP_MEALS_COOKED;
  public int GRASS_MEALS_COOKED;
  public int FOLLOWER_MEAT_MEALS_COOKED;
  public int DEADLY_MEALS_COOKED;
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
  private int currentweapon;
  private static DataManager instance = (DataManager) null;
  public List<DataManager.EnemyData> EnemiesKilled = new List<DataManager.EnemyData>();
  public Alerts Alerts = new Alerts();
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
    if (this.DiscoveredLocations.Contains(FollowerLocation.Hub1_RatauOutside) && this.DiscoveredLocations.Contains(FollowerLocation.Hub1_Sozo) && this.DiscoveredLocations.Contains(FollowerLocation.HubShore) && this.DiscoveredLocations.Contains(FollowerLocation.Dungeon_Decoration_Shop1) && this.DiscoveredLocations.Contains(FollowerLocation.Dungeon_Location_4))
      AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("FIND_ALL_LOCATIONS"));
    return true;
  }

  public static bool HasKeyPieceFromLocation(FollowerLocation Location, int Layer)
  {
    return DataManager.Instance.KeyPiecesFromLocation.Contains($"{(object) Location}_{(object) Layer}");
  }

  public static void SaveKeyPieceFromLocation(FollowerLocation Location, int Layer)
  {
    if (DataManager.HasKeyPieceFromLocation(Location, Layer))
      return;
    DataManager.Instance.KeyPiecesFromLocation.Add($"{(object) Location}_{(object) Layer}");
  }

  public bool TryRevealTutorialTopic(TutorialTopic topic)
  {
    if (this.RevealedTutorialTopics.Contains(topic) || CheatConsole.HidingUI)
      return false;
    this.RevealedTutorialTopics.Add(topic);
    this.Alerts.Tutorial.AddOnce(topic);
    return true;
  }

  public void SetTutorialVariables()
  {
    DataManager.Instance.AllowSaving = false;
    DataManager.Instance.EnabledHealing = false;
    DataManager.Instance.BuildShrineEnabled = false;
    DataManager.instance.CookedFirstFood = false;
    DataManager.instance.XPEnabled = false;
    DataManager.Instance.InTutorial = false;
    DataManager.Instance.Tutorial_Second_Enter_Base = false;
    DataManager.Instance.AllowBuilding = false;
    DataManager.Instance.ShowLoyaltyBars = false;
    DataManager.Instance.RatExplainDungeon = false;
    DataManager.Instance.ShowCultFaith = false;
    DataManager.Instance.ShowCultHunger = false;
    DataManager.Instance.ShowCultIllness = false;
    DataManager.Instance.UnlockBaseTeleporter = false;
    DataManager.Instance.BonesEnabled = false;
    DataManager.instance.PauseGameTime = true;
    DataManager.instance.ShownDodgeTutorial = false;
    DataManager.instance.ShownInventoryTutorial = false;
    DataManager.instance.HasEncounteredTarot = false;
    DataManager.Instance.CurrentGameTime = 244f;
    DataManager.Instance.HasBuiltShrine1 = false;
    DataManager.Instance.OnboardedHomeless = false;
    DataManager.Instance.ForceDoctrineStones = false;
    DataManager.instance.HadInitialDeathCatConversation = false;
    DataManager.instance.PlayerHasBeenGivenHearts = false;
    DataManager.instance.BaseGoopDoorLocked = true;
    DataManager.Instance.PLAYER_TOTAL_HEALTH = (float) DataManager.Instance.PLAYER_STARTING_HEALTH;
    DataManager.instance.PLAYER_STARTING_HEALTH_CACHED = DataManager.Instance.PLAYER_STARTING_HEALTH;
    DataManager.instance.PLAYER_HEALTH = (float) DataManager.Instance.PLAYER_STARTING_HEALTH;
    DataManager instance = DataManager.instance;
    int num = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
    num = num.ToString().GetStableHashCode();
    string str = num.ToString();
    instance.SaveUniqueID = str;
    if (CheatConsole.IN_DEMO)
      return;
    DataManager.Instance.CanReadMinds = false;
    DataManager.Instance.EnabledSpells = false;
  }

  public float PlayerDamageDealt
  {
    get => this.playerDamageDealt;
    set
    {
      this.playerDamageDealt = value;
      DifficultyManager.LoadCurrentDifficulty();
    }
  }

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

  public static void SetNewRun()
  {
    DataManager.instance.EnabledSword = true;
    DataManager.instance.dungeonRunDuration = Time.time;
    DataManager.instance.dungeonVisitedRooms = new List<NodeType>()
    {
      NodeType.FirstFloor
    };
    DataManager.instance.FollowersRecruitedInNodes = new List<int>();
    DataManager.instance.FollowersRecruitedThisNode = 0;
    DataManager.instance.PlayerKillsOnRun = 0;
    DataManager.instance.PlayerStartingBlackSouls = DataManager.instance.BlackSouls;
    DataManager.instance.dungeonRunXPOrbs = 0;
    ++DataManager.instance.dungeonRun;
    Debug.Log((object) ("Increase dungeon run! " + (object) DataManager.instance.dungeonRun));
    DataManager.instance.GivenFollowerHearts = false;
    DataManager.RandomSeed = new System.Random(UnityEngine.Random.Range(int.MinValue, int.MaxValue));
    DataManager.UseDataManagerSeed = true;
    DataManager.instance.PlayerHasFollowers = DataManager.instance.Followers.Count > 0;
    DataManager.instance.PlayerDamageReceivedThisRun = 0.0f;
    DataManager.instance.PlayerDamageDealtThisRun = 0.0f;
    DataManager.instance.HadNecklaceOnRun = 0;
    DataManager.instance.CurrentWeapon = EquipmentType.None;
    DataManager.Instance.CurrentWeaponLevel = 0;
    DataManager.Instance.CurrentRunWeaponLevel = 0;
    DataManager.instance.CurrentCurse = EquipmentType.None;
    DataManager.Instance.CurrentCurseLevel = 0;
    DataManager.Instance.CurrentRunCurseLevel = 0;
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
    DataManager.instance.BeatenDeathCat = DataManager.instance.DeathCatBeaten;
    DataManager.instance.MinimumRandomRoomsEncountered = DataManager.instance.MinimumRandomRoomsEncounteredAmount > 3;
    DataManager.instance.CanFindTarotCards = DataManager.instance.PlayerFleece != 4;
    PlayerFleeceManager.ResetDamageModifier();
  }

  public static void ResetRunData()
  {
    TrinketManager.RemoveAllTrinkets();
    DataManager.Instance.PlayerEaten = false;
    DataManager.instance.PLAYER_REDEAL_TOKEN = DataManager.instance.PLAYER_REDEAL_TOKEN_TOTAL;
    DataManager.instance.PLAYER_RUN_DAMAGE_LEVEL = 0.0f;
    ResurrectOnHud.ResurrectionType = ResurrectionType.None;
    FaithAmmo.Reload();
    DataManager.Instance.FailedObjectives.Clear();
    DataManager.instance.HadNecklaceOnRun = 0;
    DataManager.Instance.PLAYER_TOTAL_HEALTH = (float) (DataManager.Instance.PLAYER_STARTING_HEALTH + DataManager.Instance.PLAYER_HEARTS_LEVEL + DataManager.instance.PLAYER_HEALTH_MODIFIED);
    DataManager.Instance.PLAYER_STARTING_HEALTH_CACHED = DataManager.Instance.PLAYER_STARTING_HEALTH;
    DataManager.Instance.RedHeartsTemporarilyRemoved = 0;
    HUD_Timer.TimerRunning = false;
    HUD_Timer.Timer = 0.0f;
    DataManager.Instance.PLAYER_HEALTH = DataManager.Instance.PLAYER_TOTAL_HEALTH;
    DataManager.Instance.PLAYER_SPIRIT_HEARTS = DataManager.Instance.PLAYER_SPIRIT_TOTAL_HEARTS = 0.0f;
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
    DataManager.instance.CurrentRoomCoordinates = Vector2.zero;
    DungeonModifier.SetActiveModifier((DungeonModifier) null);
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
    DataManager.instance.Alerts.RunTarotCardAlerts.ClearAll();
    DataManager.Instance.CurrentWeaponLevel = 1;
    DataManager.Instance.CurrentWeapon = EquipmentType.None;
    DataManager.Instance.CurrentCurseLevel = 1;
    DataManager.Instance.CurrentCurse = EquipmentType.None;
  }

  public void ReplaceDeprication(GameManager root)
  {
    foreach (FollowerInfo followerInfo in this.Followers_Dead)
    {
      if (!this.Followers_Dead_IDs.Contains(followerInfo.ID))
        this.Followers_Dead_IDs.Add(followerInfo.ID);
    }
    if (!string.IsNullOrEmpty(DataManager.instance.SaveUniqueID))
      return;
    DataManager.instance.SaveUniqueID = UnityEngine.Random.Range(int.MinValue, int.MaxValue).ToString().GetStableHashCode().ToString();
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
    switch (type)
    {
      case StructureBrain.TYPES.DECORATION_TREE:
      case StructureBrain.TYPES.DECORATION_STONE:
      case StructureBrain.TYPES.PLANK_PATH:
      case StructureBrain.TYPES.DECORATION_LAMB_STATUE:
      case StructureBrain.TYPES.DECORATION_TORCH:
      case StructureBrain.TYPES.DECORATION_FLOWER_BOX_2:
      case StructureBrain.TYPES.DECORATION_FLOWER_BOX_3:
      case StructureBrain.TYPES.DECORATION_SMALL_STONE_CANDLE:
      case StructureBrain.TYPES.DECORATION_FLAG_CROWN:
      case StructureBrain.TYPES.DECORATION_FLAG_SCRIPTURE:
      case StructureBrain.TYPES.DECORATION_WALL_TWIGS:
      case StructureBrain.TYPES.DECORATION_SHRUB:
      case StructureBrain.TYPES.DECORATION_BELL_STATUE:
      case StructureBrain.TYPES.DECORATION_BONE_ARCH:
      case StructureBrain.TYPES.DECORATION_BONE_BARREL:
      case StructureBrain.TYPES.DECORATION_BONE_CANDLE:
      case StructureBrain.TYPES.DECORATION_BONE_FLAG:
      case StructureBrain.TYPES.DECORATION_BONE_LANTERN:
      case StructureBrain.TYPES.DECORATION_BONE_PILLAR:
      case StructureBrain.TYPES.DECORATION_BONE_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_CANDLE_BARREL:
      case StructureBrain.TYPES.DECORATION_POST_BOX:
      case StructureBrain.TYPES.DECORATION_WALL_BONE:
      case StructureBrain.TYPES.DECORATION_WALL_GRASS:
      case StructureBrain.TYPES.DECORATION_FLOWER_BOTTLE:
      case StructureBrain.TYPES.DECORATION_FLOWER_CART:
      case StructureBrain.TYPES.DECORATION_LEAFY_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_MUSHROOM_1:
      case StructureBrain.TYPES.DECORATION_MUSHROOM_2:
      case StructureBrain.TYPES.DECORATION_MUSHROOM_CANDLE_2:
      case StructureBrain.TYPES.DECORATION_MUSHROOM_CANDLE_LARGE:
      case StructureBrain.TYPES.DECORATION_MUSHROOM_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_SPIDER_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_STONE_CANDLE_LAMP:
      case StructureBrain.TYPES.TILE_FLOWERS:
      case StructureBrain.TYPES.TILE_SPOOKYPLANKS:
      case StructureBrain.TYPES.TILE_REDGRASS:
      case StructureBrain.TYPES.TILE_WATER:
      case StructureBrain.TYPES.DECORATION_MONSTERSHRINE:
      case StructureBrain.TYPES.DECORATION_FLOWERPOTWALL:
      case StructureBrain.TYPES.DECORATION_LEAFYLAMPPOST:
      case StructureBrain.TYPES.DECORATION_FLOWERVASE:
      case StructureBrain.TYPES.DECORATION_WATERINGCAN:
      case StructureBrain.TYPES.DECORATION_FLOWER_CART_SMALL:
      case StructureBrain.TYPES.DECORATION_WEEPINGSHRINE:
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_1:
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_2:
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_3:
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_4:
      case StructureBrain.TYPES.DECORATION_BOSS_TROPHY_5:
      case StructureBrain.TYPES.DECORATION_VIDEO:
      case StructureBrain.TYPES.DECORATION_PLUSH:
      case StructureBrain.TYPES.DECORATION_TWITCH_FLAG_CROWN:
      case StructureBrain.TYPES.DECORATION_TWITCH_MUSHROOM_BAG:
      case StructureBrain.TYPES.DECORATION_TWITCH_ROSE_BUSH:
      case StructureBrain.TYPES.DECORATION_TWITCH_STONE_FLAG:
      case StructureBrain.TYPES.DECORATION_TWITCH_STONE_STATUE:
      case StructureBrain.TYPES.DECORATION_TWITCH_WOODEN_GUARDIAN:
      case StructureBrain.TYPES.DECORATION_HALLOWEEN_PUMPKIN:
      case StructureBrain.TYPES.DECORATION_HALLOWEEN_SKULL:
      case StructureBrain.TYPES.DECORATION_HALLOWEEN_CANDLE:
      case StructureBrain.TYPES.DECORATION_HALLOWEEN_TREE:
        return true;
      default:
        return false;
    }
  }

  public static bool GetDecorationsAvailableCategory(FollowerLocation location)
  {
    DataManager.DecorationType decorationType;
    switch (location)
    {
      case FollowerLocation.HubShore:
      case FollowerLocation.Dungeon1_1:
      case FollowerLocation.Boss_1:
        decorationType = DataManager.DecorationType.Dungeon1;
        break;
      case FollowerLocation.Dungeon1_2:
      case FollowerLocation.Boss_2:
      case FollowerLocation.Sozo:
        decorationType = DataManager.DecorationType.Mushroom;
        break;
      case FollowerLocation.Dungeon1_3:
      case FollowerLocation.Boss_3:
      case FollowerLocation.Dungeon_Decoration_Shop1:
        decorationType = DataManager.DecorationType.Crystal;
        break;
      case FollowerLocation.Dungeon1_4:
      case FollowerLocation.Boss_4:
      case FollowerLocation.Dungeon_Location_3:
        decorationType = DataManager.DecorationType.Spider;
        break;
      default:
        Debug.Log((object) ("Couldnt find player farming location: " + (object) location));
        decorationType = DataManager.DecorationType.All;
        break;
    }
    List<StructureBrain.TYPES> typesList = new List<StructureBrain.TYPES>();
    List<StructureBrain.TYPES> listFromCategory = DataManager.instance.GetDecorationListFromCategory(DataManager.DecorationType.All);
    foreach (StructureBrain.TYPES availableDecoration in DataManager.GetAvailableDecorations())
    {
      if ((DataManager.GetDecorationType(availableDecoration) == decorationType || DataManager.GetDecorationType(availableDecoration) == DataManager.DecorationType.Path) && !StructuresData.GetUnlocked(availableDecoration))
        typesList.Add(availableDecoration);
    }
    foreach (StructureBrain.TYPES Types in listFromCategory)
    {
      if (!StructuresData.GetUnlocked(Types))
        typesList.Add(Types);
    }
    if (typesList.Count > 0)
      return true;
    Debug.Log((object) "Couldnt find any unlocked decorations");
    return false;
  }

  public List<StructureBrain.TYPES> GetDecorationListFromLocation(FollowerLocation location)
  {
    switch (location)
    {
      case FollowerLocation.HubShore:
      case FollowerLocation.Dungeon1_1:
      case FollowerLocation.Boss_1:
        return DataManager.Instance.GetDecorationListFromCategory(DataManager.DecorationType.Dungeon1);
      case FollowerLocation.Dungeon1_2:
      case FollowerLocation.Boss_2:
      case FollowerLocation.Sozo:
        return DataManager.Instance.GetDecorationListFromCategory(DataManager.DecorationType.Mushroom);
      case FollowerLocation.Dungeon1_3:
      case FollowerLocation.Boss_3:
      case FollowerLocation.Dungeon_Decoration_Shop1:
        return DataManager.Instance.GetDecorationListFromCategory(DataManager.DecorationType.Crystal);
      case FollowerLocation.Dungeon1_4:
      case FollowerLocation.Boss_4:
      case FollowerLocation.Dungeon_Location_4:
        return DataManager.Instance.GetDecorationListFromCategory(DataManager.DecorationType.Spider);
      default:
        Debug.Log((object) ("Couldnt find player farming location: " + (object) location));
        return DataManager.Instance.GetDecorationListFromCategory(DataManager.DecorationType.All);
    }
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
    return DataManager.DecorationsForType(DataManager.DecorationType.Path).Contains(type) ? DataManager.DecorationType.Path : DataManager.DecorationType.None;
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
          StructureBrain.TYPES.DECORATION_BOSS_TROPHY_5,
          StructureBrain.TYPES.DECORATION_VIDEO
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
          StructureBrain.TYPES.TILE_FLOWERSROCKY
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
          StructureBrain.TYPES.DECORATION_TWITCH_FLAG_CROWN,
          StructureBrain.TYPES.DECORATION_TWITCH_MUSHROOM_BAG,
          StructureBrain.TYPES.DECORATION_TWITCH_ROSE_BUSH,
          StructureBrain.TYPES.DECORATION_TWITCH_STONE_FLAG,
          StructureBrain.TYPES.DECORATION_TWITCH_STONE_STATUE,
          StructureBrain.TYPES.DECORATION_TWITCH_WOODEN_GUARDIAN,
          StructureBrain.TYPES.DECORATION_PLUSH
        };
      case DataManager.DecorationType.Special_Events:
        return new List<StructureBrain.TYPES>()
        {
          StructureBrain.TYPES.DECORATION_HALLOWEEN_CANDLE,
          StructureBrain.TYPES.DECORATION_HALLOWEEN_SKULL,
          StructureBrain.TYPES.DECORATION_HALLOWEEN_TREE,
          StructureBrain.TYPES.DECORATION_HALLOWEEN_PUMPKIN
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
      if (!DataManager.GetFollowerSkinUnlocked(skinAndData.Skin[0].Skin) && !stringList.Contains(skinAndData.Skin[0].Skin) && !DataManager.instance.DLCSkins.Contains(skinAndData.Skin[0].Skin) && !DataManager.instance.SpecialEventSkins.Contains<string>(skinAndData.Skin[0].Skin))
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

  public static bool GetFollowerSkinUnlocked(string skinName)
  {
    return DataManager.Instance.FollowerSkinsUnlocked.Contains(skinName);
  }

  public static void SetFollowerSkinUnlocked(string skinName)
  {
    if (!skinName.Contains("Boss"))
      skinName = skinName.StripNumbers();
    if (DataManager.Instance.FollowerSkinsUnlocked.Contains(skinName))
      return;
    DataManager.Instance.FollowerSkinsUnlocked.Add(skinName);
    Action<string> onSkinUnlocked = DataManager.OnSkinUnlocked;
    if (onSkinUnlocked != null)
      onSkinUnlocked(skinName);
    if (DataManager.CheckIfThereAreSkinsAvailableAll())
      return;
    Debug.Log((object) "Follower Skin Achievement Unlocked");
    AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("ALL_SKINS_UNLOCKED"));
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

  public static int StartingEquipmentLevel
  {
    get
    {
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

  public EquipmentType GetRandomWeaponInPool()
  {
    if (GameManager.CurrentDungeonFloor <= 1 && DataManager.Instance.dungeonRun >= 2)
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
    }
    List<EquipmentType> equipmentTypeList = new List<EquipmentType>((IEnumerable<EquipmentType>) DataManager.Instance.WeaponPool);
    if (equipmentTypeList.Count <= 1)
      return equipmentTypeList[0];
    if (equipmentTypeList.Contains(DataManager.instance.CurrentWeapon))
      equipmentTypeList.Remove(DataManager.instance.CurrentWeapon);
    for (int index = equipmentTypeList.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) EquipmentManager.GetWeaponData(equipmentTypeList[index]) != (UnityEngine.Object) null)
      {
        if (!DataManager.Instance.WeaponPool.Contains(EquipmentManager.GetWeaponData(equipmentTypeList[index]).PrimaryEquipmentType))
          equipmentTypeList.Remove(equipmentTypeList[index]);
        else if (!GameManager.HasUnlockAvailable() && (bool) (UnityEngine.Object) EquipmentManager.GetWeaponData(equipmentTypeList[index]).GetAttachment(AttachmentEffect.Fervour))
          equipmentTypeList.Remove(equipmentTypeList[index]);
      }
      else
        equipmentTypeList.Remove(equipmentTypeList[index]);
    }
    float num1 = 0.0f;
    for (int index = 0; index < equipmentTypeList.Count; ++index)
    {
      float weight = EquipmentManager.GetWeaponData(equipmentTypeList[index]).Weight;
      if (float.IsPositiveInfinity(weight))
        return equipmentTypeList[index];
      if ((double) weight >= 0.0 && !float.IsNaN(weight))
        num1 += weight;
    }
    float num2 = UnityEngine.Random.value;
    float num3 = 0.0f;
    for (int index = 0; index < equipmentTypeList.Count; ++index)
    {
      float weight = EquipmentManager.GetWeaponData(equipmentTypeList[index]).Weight;
      if (!float.IsNaN(weight) && (double) weight > 0.0)
      {
        num3 += weight / (float) equipmentTypeList.Count;
        if ((double) num3 >= (double) num2)
          return equipmentTypeList[index];
      }
    }
    return this.GetRandomWeaponInPool();
  }

  public void AddCurse(EquipmentType curse)
  {
    if (this.CursePool.Contains(curse))
      return;
    this.CursePool.Add(curse);
  }

  public EquipmentType GetRandomCurseInPool()
  {
    if (GameManager.CurrentDungeonFloor <= 1 && DataManager.Instance.dungeonRun >= 2)
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
    if (equipmentTypeList.Contains(DataManager.instance.CurrentCurse))
      equipmentTypeList.Remove(DataManager.instance.CurrentCurse);
    for (int index = equipmentTypeList.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) EquipmentManager.GetCurseData(equipmentTypeList[index]) != (UnityEngine.Object) null)
      {
        if (!DataManager.Instance.CursePool.Contains(EquipmentManager.GetCurseData(equipmentTypeList[index]).PrimaryEquipmentType))
          equipmentTypeList.Remove(equipmentTypeList[index]);
      }
      else
        equipmentTypeList.Remove(equipmentTypeList[index]);
    }
    float num1 = 0.0f;
    for (int index = 0; index < equipmentTypeList.Count; ++index)
    {
      float weight = EquipmentManager.GetCurseData(equipmentTypeList[index]).Weight;
      if (float.IsPositiveInfinity(weight))
        return equipmentTypeList[index];
      if ((double) weight >= 0.0 && !float.IsNaN(weight))
        num1 += weight;
    }
    float num2 = UnityEngine.Random.value;
    float num3 = 0.0f;
    for (int index = 0; index < equipmentTypeList.Count; ++index)
    {
      float weight = EquipmentManager.GetCurseData(equipmentTypeList[index]).Weight;
      if (!float.IsNaN(weight) && (double) weight > 0.0)
      {
        num3 += weight / (float) equipmentTypeList.Count;
        if ((double) num3 >= (double) num2)
          return equipmentTypeList[index];
      }
    }
    return this.GetRandomCurseInPool();
  }

  public static bool UnlockTrinket(TarotCards.Card card)
  {
    if (DataManager.Instance.PlayerFoundTrinkets.Contains(card))
      return false;
    Debug.Log((object) $"Collected Tarots: {(object) DataManager.Instance.PlayerFoundTrinkets.Count}Total Tarot: {(object) DataManager.AllTrinkets.Count}");
    DataManager.Instance.PlayerFoundTrinkets.Add(card);
    if (DataManager.Instance.PlayerFoundTrinkets.Count >= DataManager.AllTrinkets.Count)
      AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("ALL_TAROTS_UNLOCKED"));
    return true;
  }

  public static bool TrinketUnlocked(TarotCards.Card card)
  {
    return !DataManager.Instance.PlayerFoundTrinkets.Contains(card);
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

  public ItemSelector.Category GetItemSelectorCategory(string key)
  {
    if (string.IsNullOrEmpty(key))
      return (ItemSelector.Category) null;
    foreach (ItemSelector.Category selectorCategory in this.ItemSelectorCategories)
    {
      if (selectorCategory.Key == key)
        return selectorCategory;
    }
    ItemSelector.Category selectorCategory1 = new ItemSelector.Category()
    {
      Key = key
    };
    this.ItemSelectorCategories.Add(selectorCategory1);
    return selectorCategory1;
  }

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

  public bool DungeonCompleted(FollowerLocation location)
  {
    return this.BossesCompleted.Contains(location);
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
    }
  }

  public void AddToNotificationHistory(FinalizedNotification finalizedNotification)
  {
    if (this.NotificationHistory.Count > 20)
      this.NotificationHistory.RemoveAt(this.NotificationHistory.Count - 1);
    this.NotificationHistory.Insert(0, finalizedNotification);
  }

  public static bool ActivateCultistDLC()
  {
    if (DataManager.Instance.DLC_Cultist_Pack)
      return false;
    DataManager.Instance.DLC_Cultist_Pack = true;
    for (int index = 0; index < DataManager.CultistDLCSkins.Count; ++index)
      DataManager.SetFollowerSkinUnlocked(DataManager.CultistDLCSkins[index]);
    for (int index = 0; index < DataManager.CultistDLCStructures.Count; ++index)
      StructuresData.CompleteResearch(DataManager.CultistDLCStructures[index]);
    return true;
  }

  public static void DeactivateCultistDLC()
  {
    DataManager.Instance.DLC_Cultist_Pack = false;
    for (int index = 0; index < DataManager.CultistDLCSkins.Count; ++index)
      DataManager.RemoveUnlockedFollowerSkin(DataManager.CultistDLCSkins[index]);
    for (int index = 0; index < DataManager.CultistDLCStructures.Count; ++index)
      DataManager.Instance.UnlockedStructures.Remove(DataManager.CultistDLCStructures[index]);
  }

  public static bool ActivatePrePurchaseDLC()
  {
    if (DataManager.Instance.DLC_Pre_Purchase)
      return false;
    DataManager.Instance.DLC_Pre_Purchase = true;
    DataManager.SetFollowerSkinUnlocked("Cthulhu");
    return true;
  }

  public static bool ActivatePlushBonusDLC()
  {
    if (DataManager.Instance.DLC_Plush_Bonus)
      return false;
    DataManager.Instance.DLC_Plush_Bonus = true;
    StructuresData.CompleteResearch(StructureBrain.TYPES.DECORATION_PLUSH);
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

  [Serializable]
  public class LocationAndLayer
  {
    public FollowerLocation Location;
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

  public class LocationSeedsData
  {
    public FollowerLocation Location;
    public int Seed;
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
    Total,
  }

  public class QuestHistoryData
  {
    public int QuestIndex;
    public float QuestTimestamp;
    public float QuestCooldownDuration;
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
  }

  public delegate void ChangeToolAction(int PrevTool, int NewTool);

  public class EnemyData
  {
    public Enemy EnemyType;
    public int AmountKilled = 1;
  }
}
