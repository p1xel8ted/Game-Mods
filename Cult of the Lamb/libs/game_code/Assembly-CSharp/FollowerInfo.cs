// Decompiled with JetBrains decompiler
// Type: FollowerInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
using MessagePack.Formatters;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

#nullable disable
[MessagePackObject(false)]
public class FollowerInfo
{
  [Key(0)]
  public int ID;
  [Key(1)]
  public string _name;
  [Key(2)]
  public Thought CursedState;
  [Key(3)]
  public bool WorkerBeenGivenOrders = true;
  [Key(4)]
  public float follower_pitch;
  [Key(5)]
  public float follower_vibrato;
  [Key(6)]
  public int SermonCountAfterlife;
  [Key(7)]
  public int SermonCountFood;
  [Key(8)]
  public int SermonCountLawAndOrder;
  [Key(9)]
  public int SermonCountPossession;
  [Key(10)]
  public int SermonCountWorkAndWorship;
  [Key(11)]
  public bool TraitsSet;
  [Key(12)]
  public int SkinCharacter;
  [Key(13)]
  public int SkinVariation;
  [Key(14)]
  public int SkinColour;
  [Key(15)]
  public string SkinName;
  [Key(16 /*0x10*/)]
  public bool ShowingNecklace = true;
  [Key(17)]
  public FollowerHatType Hat;
  [Key(18)]
  public FollowerOutfitType Outfit;
  [Key(19)]
  public FollowerClothingType Clothing;
  [Key(20)]
  public string ClothingVariant;
  [Key(21)]
  public string ClothingPreviousVariant;
  [Key(22)]
  public FollowerCustomisationType Customisation;
  [Key(23)]
  public FollowerSpecialType Special;
  [Key(24)]
  public int ImprisonedDay;
  [Key(25)]
  public InventoryItem.ITEM_TYPE Necklace;
  [Key(26)]
  public bool IsFakeBrain;
  [Key(27)]
  public List<FollowerPet.FollowerPetType> Pets = new List<FollowerPet.FollowerPetType>();
  [Key(28)]
  public List<FollowerPet.DLCPet> DLCPets = new List<FollowerPet.DLCPet>();
  [Key(29)]
  public int DayJoined;
  [Key(30)]
  public int TimesTurnedIntoADemon;
  [Key(31 /*0x1F*/)]
  public int TimesDoneConfessionBooth;
  [Key(32 /*0x20*/)]
  public int Age;
  [Key(33)]
  public int LifeExpectancy;
  [Key(34)]
  public InventoryItem.ITEM_TYPE SacrificialType;
  [Key(35)]
  public bool LeavingCult;
  [Key(36)]
  public int LeftCultDay;
  [Key(37)]
  public bool DiedOfIllness;
  [Key(38)]
  public bool DiedOfInjury;
  [Key(39)]
  public bool DiedOfOldAge;
  [Key(40)]
  public bool DiedOfStarvation;
  [Key(41)]
  public bool FrozeToDeath;
  [Key(42)]
  public bool DiedFromRot;
  [Key(43)]
  public bool DiedFromTwitchChat;
  [Key(44)]
  public bool DiedInPrison;
  [Key(45)]
  public bool DiedFromMurder;
  [Key(46)]
  public bool DiedFromDeadlyDish;
  [Key(47)]
  public bool DiedFromMissionary;
  [Key(48 /*0x30*/)]
  public bool DiedFromLightning;
  [Key(49)]
  public bool DiedFromOverheating;
  [Key(50)]
  public bool BurntToDeath;
  [Key(51)]
  public int TimeOfDeath;
  [Key(52)]
  public int SpouseFollowerID = -1;
  [Key(53)]
  public bool BornInCult;
  [Key(54)]
  public bool RottingUnique;
  [Key(55)]
  public int MurderedBy;
  [Key(56)]
  public string MurderedTerm;
  [Key(57)]
  public int WeatherEventIDBackToWork = -1;
  [Key(58)]
  public bool HasBeenBuried;
  [Key(59)]
  public bool HasReceivedNecklace;
  [Key(60)]
  public int DayBecameMutated = -1;
  [Key(61)]
  public Thought StartingCursedState;
  [Key(62)]
  public FollowerFaction Faction;
  [Key(63 /*0x3F*/)]
  public FollowerRole FollowerRole;
  [Key(64 /*0x40*/)]
  public WorkerPriority WorkerPriority;
  [Key(65)]
  public bool IsDisciple;
  [Key(66)]
  public FollowerTaskType CurrentOverrideTaskType;
  [Key(67)]
  public StructureBrain.TYPES CurrentOverrideStructureType;
  [Key(68)]
  public int OverrideDayIndex;
  [Key(69)]
  public bool OverrideTaskCompleted;
  [Key(70)]
  public List<ThoughtData> Thoughts = new List<ThoughtData>();
  [Key(71)]
  public float Adoration;
  public const float MAX_ADORATION = 100f;
  [Key(72)]
  public int XPLevel = 1;
  [Key(73)]
  public FollowerLocation HomeLocation;
  [Key(74)]
  public FollowerLocation Location;
  [Key(75)]
  public Vector3 LastPosition;
  [Key(76)]
  public FollowerTaskType SavedFollowerTaskType;
  [Key(77)]
  public FollowerLocation SavedFollowerTaskLocation;
  [Key(78)]
  public Vector3 SavedFollowerTaskDestination;
  [Key(79)]
  public int DwellingID;
  [Key(80 /*0x50*/)]
  public int PreviousDwellingID;
  [Key(81)]
  public int DwellingLevel;
  [Key(82)]
  public int DwellingSlot;
  [Key(83)]
  public int RandomSeed;
  [Key(84)]
  public float MissionaryTimestamp;
  [Key(85)]
  public float MissionaryDuration = 2400f;
  [Key(86)]
  public int MissionaryIndex;
  [Key(87)]
  public int MissionaryType;
  [Key(88)]
  public float MissionaryChance;
  [Key(89)]
  public bool MissionaryFinished;
  [Key(90)]
  public bool MissionarySuccessful;
  [Key(91)]
  public InventoryItem[] MissionaryRewards;
  [Key(92)]
  public List<int> FollowersReeducatedToday = new List<int>();
  [Key(93)]
  public int FightPitWinStreak;
  [Key(94)]
  public int FightPitsFought;
  [Key(95)]
  public int FollowersFought;
  [Key(96 /*0x60*/)]
  public int PoemStatus;
  [Key(97)]
  public List<int> Parents = new List<int>();
  [Key(98)]
  public string Parent1Name;
  [Key(99)]
  public string Parent2Name;
  [Key(100)]
  public List<int> Siblings = new List<int>();
  [Key(101)]
  public List<InventoryItem> Inventory = new List<InventoryItem>();
  [Key(102)]
  public int DaysSleptOutside;
  [Key(103)]
  public bool HadFuneral;
  [Key(104)]
  public List<FollowerTrait.TraitType> TargetTraits = new List<FollowerTrait.TraitType>();
  [Key(105)]
  public UITraitManipulatorMenuController.Type TraitManipulateType;
  [Key(106)]
  public List<FollowerTrait.TraitType> Traits = new List<FollowerTrait.TraitType>();
  [Key(107)]
  public int BastardCounter;
  public const float MIN_HP = 0.0f;
  [Key(108)]
  public bool OldAge;
  [Key(109)]
  public bool MarriedToLeader;
  [Key(110)]
  public bool FirstTimeSpeakingToPlayer = true;
  [Key(111)]
  public bool ComplainingAboutNoHouse = true;
  [Key(112 /*0x70*/)]
  public bool ComplainingNeedBetterHouse;
  [Key(113)]
  public bool NudistWinner;
  [Key(114)]
  public bool TaxEnforcer;
  [Key(115)]
  public bool FaithEnforcer;
  [Key(116)]
  public string ViewerID;
  [Key(117)]
  public int Pleasure;
  public const float MAX_PLEASURE = 65f;
  [Key(118)]
  public int TotalPleasure;
  [Key(119)]
  public bool IsSnowman;
  [Key(190)]
  public bool FromDragonNPC;
  [Key(120)]
  public float _maxHP;
  [Key(121)]
  public float _HP;
  public const float DEVOTION_PER_GAME_DAY = 5f;
  [Key(122)]
  public float PrayProgress;
  [Key(124)]
  public int DevotionGiven;
  public const float MIN_HAPPINESS = 0.0f;
  public const float MAX_HAPPINESS = 100f;
  public const float LOW_HAPPINESS_THRESHOLD = 25f;
  public const float CRITICAL_HAPPINESS_THRESHOLD = 10f;
  public const float HAPPINESS_GAME_MINUTES_TO_DEPLETE = 6600f;
  [Key(125)]
  public float _happiness;
  public const float MIN_FAITH = 0.0f;
  public const float MAX_FAITH = 100f;
  public const float LOW_FAITH_THRESHOLD = 30f;
  public const float CRITICAL_FAITH_THRESHOLD = 5f;
  [Key(126)]
  public float _faith;
  public const float MIN_FEARLOVE = 0.0f;
  public const float MAX_FEARLOVE = 100f;
  public const float FEAR_THRESHOLD = 20f;
  public const float LOVE_THRESHOLD = 80f;
  [Key(127 /*0x7F*/)]
  public float _fearLove;
  public const float MIN_SATIATION = 0.0f;
  public const float MAX_SATIATION = 100f;
  public const float EAT_THRESHOLD = 60f;
  public const float HUNGER_THRESHOLD = 30f;
  public const float SATIATION_GAME_MINUTES_TO_DEPLETE = 2400f;
  [Key(128 /*0x80*/)]
  public float _satiation;
  public const float MIN_STARVATION = 0.0f;
  public const float MAX_STARVATION = 75f;
  [Key(129)]
  public float _starvation;
  [Key(130)]
  public bool IsFreezing;
  public const float MIN_BATHROOM = 0.0f;
  public const float MAX_BATHROOM = 30f;
  [Key(131)]
  public float BathroomFillRate = UnityEngine.Random.Range(0.5f, 1.5f);
  public const float BATHRHOOM_THRESHOLD = 15f;
  public const float BATHROOM_GAME_MINUTES_TO_FULL = 180f;
  [Key(132)]
  public float _bathroom;
  [Key(133)]
  public float _targetBathroom;
  public const float MIN_SOCIAL = 0.0f;
  public const float MAX_SOCIAL = 100f;
  public const float SOCIAL_GAME_MINUTES_TO_FULL = 200f;
  [Key(134)]
  public float _social = 100f;
  public const float MIN_VOMIT = 0.0f;
  public const float MAX_VOMIT = 30f;
  public const float VOMIT_GAME_MINUTES_TO_FULL = 320f;
  [Key(135)]
  public float _vomit;
  public const float MIN_REST = 0.0f;
  public const float TIRED_THRESHOLD = 20f;
  public const float MAX_REST = 100f;
  public const float REST_GAME_MINUTES_TO_DEPLETE = 1200f;
  [Key(136)]
  public float _rest;
  public const float MIN_ILLNESS = 0.0f;
  public const float MAX_ILLNESS = 100f;
  public const float ILLNESS_GAME_MINUTES_TO_FULL = 3600f;
  public const float MIN_EXHAUSTION = 0.0f;
  public const float MAX_EXHAUSTION = 100f;
  public const float EXHAUSTION_GAME_MINUTES_TO_REMOVE = 480f;
  public const float MIN_DRUNK = 0.0f;
  public const float MAX_DRUNK = 100f;
  public const float DRUNK_GAME_MINUTES_TO_REMOVE = 480f;
  public const float MIN_INJURED = 0.0f;
  public const float MAX_INJURED = 100f;
  public const float INJURED_GAME_MINUTES_TO_REMOVE = 2400f;
  public const int TOO_OLD_FOR_DAYCARE_AGE = 14;
  public const int ADULTHOOD_AGE = 18;
  public const float MIN_FREEZING = 0.0f;
  public const float MAX_FREEZING = 100f;
  public const float FREEZING_GAME_MINUTES_TO_REMOVE = 1800f;
  public const float MIN_SOAKING = 0.0f;
  public const float MAX_SOAKING = 100f;
  public const float SOAKING_GAME_MINUTES_TO_REMOVE = 240f;
  public const float MIN_OVERHEATING = 0.0f;
  public const float MAX_OVERHEATING = 100f;
  public const float OVERHEATING_GAME_MINUTES_TO_REMOVE = 1200f;
  public const float MIN_AFLAME = 0.0f;
  public const float MAX_AFLAME = 100f;
  public const float AFLAME_GAME_MINUTES_TO_REMOVE = 240f;
  public const float MIN_WARMTH = 0.0f;
  public const float MAX_WARMTH = 100f;
  [Key(137)]
  public float _illness;
  [Key(138)]
  public float _injured;
  [Key(139)]
  public float _exhaustion;
  [Key(140)]
  public float _drunk;
  [Key(141)]
  public float _freezing;
  [Key(142)]
  public float _overheating;
  [Key(143)]
  public float _aflame;
  [Key(144 /*0x90*/)]
  public float _soaking;
  public const float MIN_DISSENT = 0.0f;
  public const float MAX_DISSENT = 100f;
  public const float DISSENTER_THRESHOLD = 80f;
  [Key(145)]
  public float DissentDuration = -1f;
  [Key(146)]
  public float DissentGold;
  [Key(147)]
  public bool GivenDissentWarning;
  [Key(148)]
  public float _dissent;
  [Key(191)]
  public float reeducation;
  public const float MIN_REEDUCATE = 0.0f;
  public const float MAX_REEDUCATE = 100f;
  public const float REEDUCATE_GAME_MINUTES_TO_FULL = 3600f;
  public const float REEDUCATE_PRISON_BONUS = 25f;
  public const float REEDUCATE_INTERACTION_BONUS = 7.5f;
  public const float REEDUCATE_INTERACTION_FOLLOWER_BONUS = 7.5f;
  public const float EXHAUSTION_GAME_MINUTES_TO_FULL = 900f;
  [Key(149)]
  public List<StructureAndTime> ReactionsAndTime = new List<StructureAndTime>();
  [Key(150)]
  public List<TaskAndTime> TaskMemory = new List<TaskAndTime>();
  public const float INTERACTIONCOOLDOWN_DEMAND_DEVOTION = 1f;
  public const float INTERACTIONCOOLDOWN_FAST = 5f;
  public const float INTERACTIONCOOLDOWN_ENERGISING_SPEECH = 2f;
  public const float INTERACTIONCOOLDOWN_MOTIVATIONAL_SPEECH = 2f;
  [Key(151)]
  public int InteractionCoolDownFasting = -1;
  [Key(152)]
  public int InteractionCoolDownEnergizing = -1;
  [Key(153)]
  public int InteractionCoolDownMotivational = -1;
  [Key(154)]
  public int InteractionCoolDemandDevotion = -1;
  [Key(155)]
  public float FastingUntil;
  [Key(156)]
  public int GuaranteedGoodInteractionsUntil;
  [Key(157)]
  public int IncreasedDevotionOutputUntil;
  [Key(158)]
  public int BrainwashedUntil;
  [Key(159)]
  public int MotivatedUntil;
  [Key(160 /*0xA0*/)]
  public int LastBlessing;
  [Key(161)]
  public int LastSermon;
  [Key(162)]
  public float LastVomit;
  [Key(163)]
  public int LastReeducate;
  [Key(164)]
  public int LastHeal;
  [Key(165)]
  public bool PaidTithes;
  [Key(166)]
  public bool ReceivedBlessing;
  [Key(167)]
  public bool ReeducatedAction;
  [Key(168)]
  public bool KissedAction;
  [Key(169)]
  public bool Inspired;
  [Key(170)]
  public bool PetDog;
  [Key(171)]
  public bool Cuddled;
  [Key(172)]
  public bool Intimidated;
  [Key(173)]
  public bool Bribed;
  [Key(174)]
  public bool ScaredTraitInteracted;
  [Key(175)]
  public bool SozoBrainshed;
  [Key(176 /*0xB0*/)]
  public int CuddledAmount;
  [Key(177)]
  public bool BabyIgnored;
  [Key(178)]
  public int WakeUpDay = -1;
  [Key(179)]
  public int ReassuranceCount;
  [Key(180)]
  public int SmoochCount;
  [Key(181)]
  public float MissionaryExhaustion = 1f;
  public const int MaxMissionaryExhaustion = 5;
  [Key(182)]
  public int TaxCollected;
  [Key(183)]
  public bool TaxedToday;
  [Key(184)]
  public bool FaithedToday;
  [Key(185)]
  public float PoemProgress;
  [Key(186)]
  public int CursedStateVariant;
  public const float RELATIONSHIP_THRESHOLD_HATE = -10f;
  public const float RELATIONSHIP_THRESHOLD_FRIEND = 5f;
  public const float RELATIONSHIP_THRESHOLD_LOVE = 10f;
  [Key(187)]
  public List<IDAndRelationship> Relationships = new List<IDAndRelationship>();
  public const int h = 240 /*0xF0*/;
  [Key(188)]
  public int CachedLumber;
  [Key(189)]
  public int CachedLumberjackStationID;
  public static List<string> NameBeginnings = new List<string>()
  {
    ScriptLocalization.FollowerNames_FollowerName_Start._0,
    ScriptLocalization.FollowerNames_FollowerName_Start._1,
    ScriptLocalization.FollowerNames_FollowerName_Start._2,
    ScriptLocalization.FollowerNames_FollowerName_Start._3,
    ScriptLocalization.FollowerNames_FollowerName_Start._4,
    ScriptLocalization.FollowerNames_FollowerName_Start._5,
    ScriptLocalization.FollowerNames_FollowerName_Start._6,
    ScriptLocalization.FollowerNames_FollowerName_Start._7,
    ScriptLocalization.FollowerNames_FollowerName_Start._8,
    ScriptLocalization.FollowerNames_FollowerName_Start._9,
    ScriptLocalization.FollowerNames_FollowerName_Start._10,
    ScriptLocalization.FollowerNames_FollowerName_Start._11,
    ScriptLocalization.FollowerNames_FollowerName_Start._12,
    ScriptLocalization.FollowerNames_FollowerName_Start._13,
    ScriptLocalization.FollowerNames_FollowerName_Start._14,
    ScriptLocalization.FollowerNames_FollowerName_Start._15,
    ScriptLocalization.FollowerNames_FollowerName_Start._15,
    ScriptLocalization.FollowerNames_FollowerName_Start._16,
    ScriptLocalization.FollowerNames_FollowerName_Start._17,
    ScriptLocalization.FollowerNames_FollowerName_Start._18,
    ScriptLocalization.FollowerNames_FollowerName_Start._19,
    ScriptLocalization.FollowerNames_FollowerName_Start._20,
    ScriptLocalization.FollowerNames_FollowerName_Start._21,
    ScriptLocalization.FollowerNames_FollowerName_Start._22,
    ScriptLocalization.FollowerNames_FollowerName_Start._23
  };
  public static List<string> NameMiddles = new List<string>()
  {
    ScriptLocalization.FollowerNames_FollowerName_Mid._0,
    ScriptLocalization.FollowerNames_FollowerName_Start._1,
    ScriptLocalization.FollowerNames_FollowerName_Start._2,
    ScriptLocalization.FollowerNames_FollowerName_Start._3,
    ScriptLocalization.FollowerNames_FollowerName_Start._4,
    ScriptLocalization.FollowerNames_FollowerName_Start._5,
    ScriptLocalization.FollowerNames_FollowerName_Start._6,
    ScriptLocalization.FollowerNames_FollowerName_Start._7,
    ScriptLocalization.FollowerNames_FollowerName_Start._8,
    ScriptLocalization.FollowerNames_FollowerName_Start._9,
    ScriptLocalization.FollowerNames_FollowerName_Start._10
  };
  public static List<string> NameEndings = new List<string>()
  {
    ScriptLocalization.FollowerNames_FollowerName_End._0,
    ScriptLocalization.FollowerNames_FollowerName_Start._1,
    ScriptLocalization.FollowerNames_FollowerName_Start._2,
    ScriptLocalization.FollowerNames_FollowerName_Start._3,
    ScriptLocalization.FollowerNames_FollowerName_Start._4,
    ScriptLocalization.FollowerNames_FollowerName_Start._5,
    ScriptLocalization.FollowerNames_FollowerName_Start._6,
    ScriptLocalization.FollowerNames_FollowerName_Start._7,
    ScriptLocalization.FollowerNames_FollowerName_Start._8,
    ScriptLocalization.FollowerNames_FollowerName_Start._9
  };

  public static FollowerInfo GetInfoByID(int ID, bool includeDead = false)
  {
    FollowerInfo followerInfo;
    if (DataManager.TryGetFollowerInfoByID(in ID, out followerInfo))
      return followerInfo;
    if (includeDead)
    {
      foreach (FollowerInfo infoById in DataManager.Instance.Followers_Dead)
      {
        if (infoById.ID == ID)
          return infoById;
      }
    }
    return (FollowerInfo) null;
  }

  public static FollowerInfo GetInfoByViewerID(string viewerID, bool includeDead = false)
  {
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
    {
      if (follower.ViewerID == viewerID)
        return follower;
    }
    if (includeDead)
    {
      foreach (FollowerInfo infoByViewerId in DataManager.Instance.Followers_Dead)
      {
        if (infoByViewerId.ViewerID == viewerID)
          return infoByViewerId;
      }
    }
    return (FollowerInfo) null;
  }

  [IgnoreMember]
  public string Name
  {
    get
    {
      return HideSaveicon.IsSaving() ? this._name : LocalizeIntegration.Arabic_ReverseNonRTL(this._name);
    }
    set => this._name = value;
  }

  public string GetNameFormatted()
  {
    string str = $"{ScriptLocalization.Interactions.Level} {this.XPLevel.ToNumeral()}";
    if (this.IsDisciple)
      str = " <sprite name=\"icon_Disciple\">";
    return string.Join(" - ", this.Name, str) + (this.MarriedToLeader ? " <sprite name=\"icon_Married\">" : string.Empty);
  }

  [IgnoreMember]
  public int MemberDuration => TimeManager.CurrentDay - this.DayJoined;

  public bool HasThought(Thought thought)
  {
    foreach (ThoughtData thought1 in this.Thoughts)
    {
      if (thought1.ThoughtType == thought)
        return true;
    }
    return false;
  }

  [IgnoreMember]
  public int SacrificialValue
  {
    get
    {
      int num1 = 0 + this.XPLevel * 50 + new System.Random(this.RandomSeed).Next(-10, 10) + this.Age;
      int num2 = num1 - 25;
      if (this.CursedState == Thought.OldAge)
        num1 = new System.Random(this.RandomSeed).Next(25, 50);
      if (this.CursedState == Thought.Ill)
        Mathf.CeilToInt((float) num1 * 0.5f);
      return num2 / 2;
    }
  }

  [IgnoreMember]
  public float MaxHP
  {
    get => this._maxHP;
    set
    {
      float maxHp = this.MaxHP;
      this._maxHP = Mathf.Max(value, 0.0f);
      if ((double) this.MaxHP > (double) maxHp)
        this.HP += this.MaxHP - maxHp;
      else
        this.HP = this.HP;
    }
  }

  [IgnoreMember]
  public float HP
  {
    get => this._HP;
    set => this._HP = Mathf.Clamp(value, 0.0f, this.MaxHP);
  }

  [IgnoreMember]
  public float Happiness
  {
    get => this._happiness;
    set => this._happiness = Mathf.Clamp(value, 0.0f, 100f);
  }

  [IgnoreMember]
  public float Faith
  {
    get => this._faith;
    set => this._faith = Mathf.Clamp(value, 0.0f, 100f);
  }

  [IgnoreMember]
  public float FearLove
  {
    get => this._fearLove;
    set => this._fearLove = Mathf.Clamp(value, 0.0f, 100f);
  }

  [IgnoreMember]
  public float Satiation
  {
    get => this._satiation;
    set => this._satiation = Mathf.Clamp(value, 0.0f, 100f);
  }

  [IgnoreMember]
  public float Starvation
  {
    get => this._starvation;
    set => this._starvation = Mathf.Clamp(value, 0.0f, 75f);
  }

  [IgnoreMember]
  public float Bathroom
  {
    get => this._bathroom;
    set => this._bathroom = Mathf.Clamp(value, 0.0f, 30f);
  }

  [IgnoreMember]
  public float TargetBathroom
  {
    get => this._targetBathroom;
    set
    {
      this._targetBathroom = Mathf.Clamp(value, 0.0f, 30f);
      if ((double) this.TargetBathroom >= (double) this.Bathroom)
        return;
      this.Bathroom = this.TargetBathroom;
    }
  }

  [IgnoreMember]
  public float Social
  {
    get => this._social;
    set => this._social = Mathf.Clamp(value, 0.0f, 100f);
  }

  [IgnoreMember]
  public float Vomit
  {
    get => this._vomit;
    set => this._vomit = Mathf.Clamp(value, 0.0f, 30f);
  }

  [IgnoreMember]
  public float Rest
  {
    get => this._rest;
    set => this._rest = Mathf.Clamp(value, 0.0f, 100f);
  }

  public static float ILLNESS_THRESHOLD_TRAIT(FollowerTrait.TraitType Trait)
  {
    if (Trait == FollowerTrait.TraitType.IronStomach)
      return 20f;
    return Trait == FollowerTrait.TraitType.Sickly ? 75f : 50f;
  }

  [IgnoreMember]
  public float Illness
  {
    get => this._illness;
    set => this._illness = Mathf.Clamp(value, 0.0f, 100f);
  }

  [IgnoreMember]
  public float Injured
  {
    get => this._injured;
    set => this._injured = Mathf.Clamp(value, 0.0f, 100f);
  }

  [IgnoreMember]
  public float Exhaustion
  {
    get => this._exhaustion;
    set => this._exhaustion = Mathf.Clamp(value, 0.0f, 100f);
  }

  [IgnoreMember]
  public float Drunk
  {
    get => this._drunk;
    set => this._drunk = Mathf.Clamp(value, 0.0f, 100f);
  }

  [IgnoreMember]
  public bool IsDrunk => (double) this.Drunk > 0.0;

  [IgnoreMember]
  public float Freezing
  {
    get => this._freezing;
    set => this._freezing = Mathf.Clamp(value, 0.0f, 100f);
  }

  [IgnoreMember]
  public float Overheating
  {
    get => this._overheating;
    set => this._overheating = Mathf.Clamp(value, 0.0f, 100f);
  }

  [IgnoreMember]
  public float Aflame
  {
    get => this._aflame;
    set => this._aflame = Mathf.Clamp(value, 0.0f, 100f);
  }

  [IgnoreMember]
  public float Soaking
  {
    get => this._soaking;
    set => this._soaking = Mathf.Clamp(value, 0.0f, 100f);
  }

  [IgnoreMember]
  public float Dissent
  {
    get => this._dissent;
    set => this._dissent = Mathf.Clamp(value, 0.0f, 100f);
  }

  [IgnoreMember]
  public float Reeducation
  {
    get => this.reeducation;
    set => this.reeducation = Mathf.Clamp(value, 0.0f, 100f);
  }

  [IgnoreMember]
  public ObjectivesData CurrentPlayerQuest
  {
    get
    {
      List<ObjectivesData> currentFollowerQuests = Quests.GetCurrentFollowerQuests(this.ID);
      return currentFollowerQuests.Count <= 0 ? (ObjectivesData) null : currentFollowerQuests[0];
    }
  }

  [IgnoreMember]
  public bool WorkThroughNight
  {
    get
    {
      return this.WakeUpDay == TimeManager.CurrentDay || this.Necklace == InventoryItem.ITEM_TYPE.Necklace_5 || !FollowerBrainStats.ShouldSleep && this.CursedState != Thought.Child || this.Traits.Contains(FollowerTrait.TraitType.Insomniac) || this.IsSnowman || this.Traits.Contains(FollowerTrait.TraitType.Mutated);
    }
  }

  public void ResetStats()
  {
    this.Satiation = 50f;
    this.Rest = 50f;
    this.Illness = 0.0f;
    this.Dissent = 0.0f;
    this.Happiness = 50f;
    this.Starvation = 0.0f;
    this.HadFuneral = false;
  }

  public static FollowerInfo NewCharacter(FollowerLocation location, string ForceSkin = "")
  {
    FollowerInfo followerInfo = new FollowerInfo();
    followerInfo.Name = FollowerInfo.GenerateName();
    followerInfo.Age = UnityEngine.Random.Range(20, 30);
    followerInfo.LifeExpectancy = UnityEngine.Random.Range(40, 55);
    followerInfo.DayJoined = TimeManager.CurrentDay;
    followerInfo.ID = ++DataManager.Instance.FollowerID;
    while (FollowerManager.UniqueFollowerIDs.Contains(followerInfo.ID))
      followerInfo.ID = ++DataManager.Instance.FollowerID;
    followerInfo.HomeLocation = FollowerLocation.Base;
    followerInfo.Location = location;
    followerInfo.RandomSeed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
    followerInfo.XPLevel = 1;
    switch (UnityEngine.Random.Range(0, 2))
    {
      case 0:
        followerInfo.SacrificialType = InventoryItem.ITEM_TYPE.BLACK_GOLD;
        break;
      case 1:
        followerInfo.SacrificialType = InventoryItem.ITEM_TYPE.SOUL;
        break;
    }
    followerInfo.follower_pitch = UnityEngine.Random.Range(0.0f, 1f);
    followerInfo.follower_vibrato = UnityEngine.Random.Range(0.0f, 1f);
    if (ForceSkin.IsNullOrEmpty())
    {
      followerInfo.SkinCharacter = WorshipperData.Instance.GetRandomAvailableSkin();
      followerInfo.SkinVariation = UnityEngine.Random.Range(0, WorshipperData.Instance.Characters[followerInfo.SkinCharacter].Skin.Count);
      followerInfo.SkinName = WorshipperData.Instance.Characters[followerInfo.SkinCharacter].Skin[followerInfo.SkinVariation].Skin;
      followerInfo.SkinColour = UnityEngine.Random.Range(0, WorshipperData.Instance.GetColourData(followerInfo.SkinName).SlotAndColours.Count);
    }
    else
    {
      followerInfo.SkinName = ForceSkin;
      followerInfo.SkinCharacter = WorshipperData.Instance.GetSkinIndexFromName(ForceSkin);
    }
    followerInfo.Rest = 100f;
    followerInfo.Satiation = 100f;
    followerInfo.Faction = (FollowerFaction) UnityEngine.Random.Range(1, 4);
    followerInfo.PrayProgress = (float) int.MaxValue;
    followerInfo.Faith = 50f;
    followerInfo.Happiness = 80f + UnityEngine.Random.Range(-10f, 10f);
    followerInfo.PreviousDwellingID = followerInfo.DwellingID;
    followerInfo.DwellingID = Dwelling.NO_HOME;
    followerInfo.HP = followerInfo.MaxHP = 25f;
    return followerInfo;
  }

  public static string GenerateName()
  {
    string str = "" + FollowerInfo.NameBeginnings[UnityEngine.Random.Range(0, FollowerInfo.NameBeginnings.Count)];
    if ((double) UnityEngine.Random.Range(0.0f, 1f) < 0.5)
      str += FollowerInfo.NameMiddles[UnityEngine.Random.Range(0, FollowerInfo.NameMiddles.Count)].ToLower();
    return str + FollowerInfo.NameEndings[UnityEngine.Random.Range(0, FollowerInfo.NameEndings.Count)].ToLower();
  }

  public int GetDemonLevel()
  {
    return Mathf.Clamp(this.XPLevel, 1, 10) + (this.Necklace == InventoryItem.ITEM_TYPE.Necklace_Demonic ? 2 : 0);
  }

  public string GetDeathText(bool includeName = true, bool includeRichText = true)
  {
    string str = includeName ? $"<color=#FFD201>{this.Name}</color>" : "";
    string deathText;
    if (this.DiedOfIllness)
      deathText = $"{str} {ScriptLocalization.Notifications.DiedFromIllness}";
    else if (this.DiedOfOldAge)
      deathText = $"{str} {ScriptLocalization.Notifications.DiedFromOldAge}";
    else if (this.DiedOfStarvation)
      deathText = $"{str} {ScriptLocalization.Notifications.DiedFromStarvation}";
    else if (this.DiedFromTwitchChat)
      deathText = $"{str} {LocalizationManager.GetTranslation("Notifications/DiedFromTwitchChat")}";
    else if (this.DiedFromDeadlyDish)
      deathText = $"{str} {LocalizationManager.GetTranslation("Notifications/DiedFromDeadlyDish")}";
    else if (this.DiedFromMissionary)
      deathText = $"{str} {LocalizationManager.GetTranslation("Notifications/DiedFromMissionary")}";
    else if (this.DiedFromOverheating)
      deathText = $"{str} {LocalizationManager.GetTranslation("Notifications/DiedFromOverheating")}";
    else if (this.FrozeToDeath)
      deathText = $"{str} {LocalizationManager.GetTranslation("Notifications/FrozeToDeath")}";
    else if (this.DiedFromRot)
      deathText = $"{str} {LocalizationManager.GetTranslation("Notifications/DiedFromRot")}";
    else if (this.DiedFromLightning)
      deathText = $"{str} {LocalizationManager.GetTranslation("Notifications/StruckByLightning")}";
    else if (this.BurntToDeath)
      deathText = $"{str} {LocalizationManager.GetTranslation("Notifications/BurntToDeath")}";
    else if (this.MurderedBy != -1 && FollowerInfo.GetInfoByID(this.MurderedBy, true) != null)
    {
      deathText = $"{str} {string.Format(LocalizationManager.GetTranslation("Notifications/MurderedByFollower"), (object) FollowerInfo.GetInfoByID(this.MurderedBy, true).Name)}";
      if (!string.IsNullOrEmpty(this.MurderedTerm))
        deathText = $"{deathText}<br><size=28>{LocalizationManager.GetTranslation(this.MurderedTerm)}";
      if (!includeRichText)
        deathText = deathText.Replace("<color=#FFD201>", "").Replace("</color><br><size=28>", " ");
    }
    else
      deathText = $"{str} {LocalizationManager.GetTranslation("Notifications/DiedFromMurder")}";
    return deathText;
  }

  public List<FollowerTrait.TraitType> RandomisedTraits(int seed)
  {
    int max = 6;
    UnityEngine.Random.InitState(seed);
    int num = UnityEngine.Random.Range(Mathf.Clamp(this.Traits.Count - 1, 1, max), Mathf.Clamp(this.Traits.Count + 2, 1, max));
    List<FollowerTrait.TraitType> traitTypeList = new List<FollowerTrait.TraitType>();
    if (this.Traits.Contains(FollowerTrait.TraitType.Mutated))
    {
      traitTypeList.Add(FollowerTrait.TraitType.Mutated);
      --num;
    }
    foreach (FollowerTrait.TraitType uniqueTrait in FollowerTrait.UniqueTraits)
    {
      if (this.Traits.Contains(uniqueTrait))
      {
        traitTypeList.Add(uniqueTrait);
        num = Mathf.Max(num - 1, 0);
      }
    }
    foreach (FollowerTrait.TraitType trait in this.Traits)
    {
      if (this.HasTraitFromNecklace(trait) && !traitTypeList.Contains(trait))
      {
        traitTypeList.Add(trait);
        num = Mathf.Max(num - 1, 0);
      }
    }
    for (int index = 0; index < num; ++index)
    {
      FollowerTrait.TraitType traitType;
      bool flag;
      do
      {
        traitType = FollowerTrait.GetStartingTrait();
        if ((double) UnityEngine.Random.value < 0.10000000149011612)
          traitType = FollowerTrait.GetRareTrait();
        flag = false;
        foreach (KeyValuePair<FollowerTrait.TraitType, FollowerTrait.TraitType> exclusiveTrait in FollowerTrait.ExclusiveTraits)
        {
          if (exclusiveTrait.Key == traitType && this.Traits.Contains(exclusiveTrait.Value) || exclusiveTrait.Value == traitType && this.Traits.Contains(exclusiveTrait.Key))
            flag = true;
        }
      }
      while (this.Traits.Contains(traitType) || traitTypeList.Contains(traitType) || FollowerTrait.UniqueTraits.Contains(traitType) || flag);
      traitTypeList.Add(traitType);
    }
    return traitTypeList;
  }

  public bool HasTraitFromNecklace(FollowerTrait.TraitType trait)
  {
    return trait == FollowerTrait.TraitType.Immortal && this.ID != 666 && this.ID != 10009 && this.Necklace == InventoryItem.ITEM_TYPE.Necklace_Gold_Skull;
  }

  public List<FollowerTrait.TraitType> CompletelyNewRandomisedTraits(int seed, int num)
  {
    UnityEngine.Random.InitState(seed);
    List<FollowerTrait.TraitType> traitTypeList = new List<FollowerTrait.TraitType>(num);
    for (int index = 0; index < num; ++index)
    {
      FollowerTrait.TraitType traitType;
      bool flag;
      do
      {
        traitType = FollowerTrait.GetStartingTrait();
        if ((double) UnityEngine.Random.value < 0.10000000149011612)
          traitType = FollowerTrait.GetRareTrait();
        flag = false;
        foreach (KeyValuePair<FollowerTrait.TraitType, FollowerTrait.TraitType> exclusiveTrait in FollowerTrait.ExclusiveTraits)
        {
          if (exclusiveTrait.Key == traitType && this.Traits.Contains(exclusiveTrait.Value) || exclusiveTrait.Value == traitType && this.Traits.Contains(exclusiveTrait.Key))
            flag = true;
        }
      }
      while (this.Traits.Contains(traitType) || traitTypeList.Contains(traitType) || FollowerTrait.UniqueTraits.Contains(traitType) || flag);
      traitTypeList.Add(traitType);
    }
    return traitTypeList;
  }

  public sealed class FollowerInfoFormatter : 
    IMessagePackFormatter<FollowerInfo>,
    IMessagePackFormatter
  {
    public void Serialize(
      ref MessagePackWriter writer,
      FollowerInfo value,
      MessagePackSerializerOptions options)
    {
      if (value == null)
      {
        writer.WriteNil();
      }
      else
      {
        IFormatterResolver resolver = options.Resolver;
        writer.WriteArrayHeader(192 /*0xC0*/);
        writer.Write(value.ID);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value._name, options);
        resolver.GetFormatterWithVerify<Thought>().Serialize(ref writer, value.CursedState, options);
        writer.Write(value.WorkerBeenGivenOrders);
        writer.Write(value.follower_pitch);
        writer.Write(value.follower_vibrato);
        writer.Write(value.SermonCountAfterlife);
        writer.Write(value.SermonCountFood);
        writer.Write(value.SermonCountLawAndOrder);
        writer.Write(value.SermonCountPossession);
        writer.Write(value.SermonCountWorkAndWorship);
        writer.Write(value.TraitsSet);
        writer.Write(value.SkinCharacter);
        writer.Write(value.SkinVariation);
        writer.Write(value.SkinColour);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.SkinName, options);
        writer.Write(value.ShowingNecklace);
        resolver.GetFormatterWithVerify<FollowerHatType>().Serialize(ref writer, value.Hat, options);
        resolver.GetFormatterWithVerify<FollowerOutfitType>().Serialize(ref writer, value.Outfit, options);
        resolver.GetFormatterWithVerify<FollowerClothingType>().Serialize(ref writer, value.Clothing, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.ClothingVariant, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.ClothingPreviousVariant, options);
        resolver.GetFormatterWithVerify<FollowerCustomisationType>().Serialize(ref writer, value.Customisation, options);
        resolver.GetFormatterWithVerify<FollowerSpecialType>().Serialize(ref writer, value.Special, options);
        writer.Write(value.ImprisonedDay);
        resolver.GetFormatterWithVerify<InventoryItem.ITEM_TYPE>().Serialize(ref writer, value.Necklace, options);
        writer.Write(value.IsFakeBrain);
        resolver.GetFormatterWithVerify<List<FollowerPet.FollowerPetType>>().Serialize(ref writer, value.Pets, options);
        resolver.GetFormatterWithVerify<List<FollowerPet.DLCPet>>().Serialize(ref writer, value.DLCPets, options);
        writer.Write(value.DayJoined);
        writer.Write(value.TimesTurnedIntoADemon);
        writer.Write(value.TimesDoneConfessionBooth);
        writer.Write(value.Age);
        writer.Write(value.LifeExpectancy);
        resolver.GetFormatterWithVerify<InventoryItem.ITEM_TYPE>().Serialize(ref writer, value.SacrificialType, options);
        writer.Write(value.LeavingCult);
        writer.Write(value.LeftCultDay);
        writer.Write(value.DiedOfIllness);
        writer.Write(value.DiedOfInjury);
        writer.Write(value.DiedOfOldAge);
        writer.Write(value.DiedOfStarvation);
        writer.Write(value.FrozeToDeath);
        writer.Write(value.DiedFromRot);
        writer.Write(value.DiedFromTwitchChat);
        writer.Write(value.DiedInPrison);
        writer.Write(value.DiedFromMurder);
        writer.Write(value.DiedFromDeadlyDish);
        writer.Write(value.DiedFromMissionary);
        writer.Write(value.DiedFromLightning);
        writer.Write(value.DiedFromOverheating);
        writer.Write(value.BurntToDeath);
        writer.Write(value.TimeOfDeath);
        writer.Write(value.SpouseFollowerID);
        writer.Write(value.BornInCult);
        writer.Write(value.RottingUnique);
        writer.Write(value.MurderedBy);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.MurderedTerm, options);
        writer.Write(value.WeatherEventIDBackToWork);
        writer.Write(value.HasBeenBuried);
        writer.Write(value.HasReceivedNecklace);
        writer.Write(value.DayBecameMutated);
        resolver.GetFormatterWithVerify<Thought>().Serialize(ref writer, value.StartingCursedState, options);
        resolver.GetFormatterWithVerify<FollowerFaction>().Serialize(ref writer, value.Faction, options);
        resolver.GetFormatterWithVerify<FollowerRole>().Serialize(ref writer, value.FollowerRole, options);
        resolver.GetFormatterWithVerify<WorkerPriority>().Serialize(ref writer, value.WorkerPriority, options);
        writer.Write(value.IsDisciple);
        resolver.GetFormatterWithVerify<FollowerTaskType>().Serialize(ref writer, value.CurrentOverrideTaskType, options);
        resolver.GetFormatterWithVerify<StructureBrain.TYPES>().Serialize(ref writer, value.CurrentOverrideStructureType, options);
        writer.Write(value.OverrideDayIndex);
        writer.Write(value.OverrideTaskCompleted);
        resolver.GetFormatterWithVerify<List<ThoughtData>>().Serialize(ref writer, value.Thoughts, options);
        writer.Write(value.Adoration);
        writer.Write(value.XPLevel);
        resolver.GetFormatterWithVerify<FollowerLocation>().Serialize(ref writer, value.HomeLocation, options);
        resolver.GetFormatterWithVerify<FollowerLocation>().Serialize(ref writer, value.Location, options);
        resolver.GetFormatterWithVerify<Vector3>().Serialize(ref writer, value.LastPosition, options);
        resolver.GetFormatterWithVerify<FollowerTaskType>().Serialize(ref writer, value.SavedFollowerTaskType, options);
        resolver.GetFormatterWithVerify<FollowerLocation>().Serialize(ref writer, value.SavedFollowerTaskLocation, options);
        resolver.GetFormatterWithVerify<Vector3>().Serialize(ref writer, value.SavedFollowerTaskDestination, options);
        writer.Write(value.DwellingID);
        writer.Write(value.PreviousDwellingID);
        writer.Write(value.DwellingLevel);
        writer.Write(value.DwellingSlot);
        writer.Write(value.RandomSeed);
        writer.Write(value.MissionaryTimestamp);
        writer.Write(value.MissionaryDuration);
        writer.Write(value.MissionaryIndex);
        writer.Write(value.MissionaryType);
        writer.Write(value.MissionaryChance);
        writer.Write(value.MissionaryFinished);
        writer.Write(value.MissionarySuccessful);
        resolver.GetFormatterWithVerify<InventoryItem[]>().Serialize(ref writer, value.MissionaryRewards, options);
        resolver.GetFormatterWithVerify<List<int>>().Serialize(ref writer, value.FollowersReeducatedToday, options);
        writer.Write(value.FightPitWinStreak);
        writer.Write(value.FightPitsFought);
        writer.Write(value.FollowersFought);
        writer.Write(value.PoemStatus);
        resolver.GetFormatterWithVerify<List<int>>().Serialize(ref writer, value.Parents, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.Parent1Name, options);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.Parent2Name, options);
        resolver.GetFormatterWithVerify<List<int>>().Serialize(ref writer, value.Siblings, options);
        resolver.GetFormatterWithVerify<List<InventoryItem>>().Serialize(ref writer, value.Inventory, options);
        writer.Write(value.DaysSleptOutside);
        writer.Write(value.HadFuneral);
        resolver.GetFormatterWithVerify<List<FollowerTrait.TraitType>>().Serialize(ref writer, value.TargetTraits, options);
        resolver.GetFormatterWithVerify<UITraitManipulatorMenuController.Type>().Serialize(ref writer, value.TraitManipulateType, options);
        resolver.GetFormatterWithVerify<List<FollowerTrait.TraitType>>().Serialize(ref writer, value.Traits, options);
        writer.Write(value.BastardCounter);
        writer.Write(value.OldAge);
        writer.Write(value.MarriedToLeader);
        writer.Write(value.FirstTimeSpeakingToPlayer);
        writer.Write(value.ComplainingAboutNoHouse);
        writer.Write(value.ComplainingNeedBetterHouse);
        writer.Write(value.NudistWinner);
        writer.Write(value.TaxEnforcer);
        writer.Write(value.FaithEnforcer);
        resolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.ViewerID, options);
        writer.Write(value.Pleasure);
        writer.Write(value.TotalPleasure);
        writer.Write(value.IsSnowman);
        writer.Write(value._maxHP);
        writer.Write(value._HP);
        writer.Write(value.PrayProgress);
        writer.WriteNil();
        writer.Write(value.DevotionGiven);
        writer.Write(value._happiness);
        writer.Write(value._faith);
        writer.Write(value._fearLove);
        writer.Write(value._satiation);
        writer.Write(value._starvation);
        writer.Write(value.IsFreezing);
        writer.Write(value.BathroomFillRate);
        writer.Write(value._bathroom);
        writer.Write(value._targetBathroom);
        writer.Write(value._social);
        writer.Write(value._vomit);
        writer.Write(value._rest);
        writer.Write(value._illness);
        writer.Write(value._injured);
        writer.Write(value._exhaustion);
        writer.Write(value._drunk);
        writer.Write(value._freezing);
        writer.Write(value._overheating);
        writer.Write(value._aflame);
        writer.Write(value._soaking);
        writer.Write(value.DissentDuration);
        writer.Write(value.DissentGold);
        writer.Write(value.GivenDissentWarning);
        writer.Write(value._dissent);
        resolver.GetFormatterWithVerify<List<StructureAndTime>>().Serialize(ref writer, value.ReactionsAndTime, options);
        resolver.GetFormatterWithVerify<List<TaskAndTime>>().Serialize(ref writer, value.TaskMemory, options);
        writer.Write(value.InteractionCoolDownFasting);
        writer.Write(value.InteractionCoolDownEnergizing);
        writer.Write(value.InteractionCoolDownMotivational);
        writer.Write(value.InteractionCoolDemandDevotion);
        writer.Write(value.FastingUntil);
        writer.Write(value.GuaranteedGoodInteractionsUntil);
        writer.Write(value.IncreasedDevotionOutputUntil);
        writer.Write(value.BrainwashedUntil);
        writer.Write(value.MotivatedUntil);
        writer.Write(value.LastBlessing);
        writer.Write(value.LastSermon);
        writer.Write(value.LastVomit);
        writer.Write(value.LastReeducate);
        writer.Write(value.LastHeal);
        writer.Write(value.PaidTithes);
        writer.Write(value.ReceivedBlessing);
        writer.Write(value.ReeducatedAction);
        writer.Write(value.KissedAction);
        writer.Write(value.Inspired);
        writer.Write(value.PetDog);
        writer.Write(value.Cuddled);
        writer.Write(value.Intimidated);
        writer.Write(value.Bribed);
        writer.Write(value.ScaredTraitInteracted);
        writer.Write(value.SozoBrainshed);
        writer.Write(value.CuddledAmount);
        writer.Write(value.BabyIgnored);
        writer.Write(value.WakeUpDay);
        writer.Write(value.ReassuranceCount);
        writer.Write(value.SmoochCount);
        writer.Write(value.MissionaryExhaustion);
        writer.Write(value.TaxCollected);
        writer.Write(value.TaxedToday);
        writer.Write(value.FaithedToday);
        writer.Write(value.PoemProgress);
        writer.Write(value.CursedStateVariant);
        resolver.GetFormatterWithVerify<List<IDAndRelationship>>().Serialize(ref writer, value.Relationships, options);
        writer.Write(value.CachedLumber);
        writer.Write(value.CachedLumberjackStationID);
        writer.Write(value.FromDragonNPC);
        writer.Write(value.reeducation);
      }
    }

    public FollowerInfo Deserialize(
      ref MessagePackReader reader,
      MessagePackSerializerOptions options)
    {
      if (reader.TryReadNil())
        return (FollowerInfo) null;
      options.Security.DepthStep(ref reader);
      IFormatterResolver resolver = options.Resolver;
      int num = reader.ReadArrayHeader();
      FollowerInfo followerInfo = new FollowerInfo();
      for (int index = 0; index < num; ++index)
      {
        switch (index)
        {
          case 0:
            followerInfo.ID = reader.ReadInt32();
            break;
          case 1:
            followerInfo._name = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 2:
            followerInfo.CursedState = resolver.GetFormatterWithVerify<Thought>().Deserialize(ref reader, options);
            break;
          case 3:
            followerInfo.WorkerBeenGivenOrders = reader.ReadBoolean();
            break;
          case 4:
            followerInfo.follower_pitch = reader.ReadSingle();
            break;
          case 5:
            followerInfo.follower_vibrato = reader.ReadSingle();
            break;
          case 6:
            followerInfo.SermonCountAfterlife = reader.ReadInt32();
            break;
          case 7:
            followerInfo.SermonCountFood = reader.ReadInt32();
            break;
          case 8:
            followerInfo.SermonCountLawAndOrder = reader.ReadInt32();
            break;
          case 9:
            followerInfo.SermonCountPossession = reader.ReadInt32();
            break;
          case 10:
            followerInfo.SermonCountWorkAndWorship = reader.ReadInt32();
            break;
          case 11:
            followerInfo.TraitsSet = reader.ReadBoolean();
            break;
          case 12:
            followerInfo.SkinCharacter = reader.ReadInt32();
            break;
          case 13:
            followerInfo.SkinVariation = reader.ReadInt32();
            break;
          case 14:
            followerInfo.SkinColour = reader.ReadInt32();
            break;
          case 15:
            followerInfo.SkinName = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 16 /*0x10*/:
            followerInfo.ShowingNecklace = reader.ReadBoolean();
            break;
          case 17:
            followerInfo.Hat = resolver.GetFormatterWithVerify<FollowerHatType>().Deserialize(ref reader, options);
            break;
          case 18:
            followerInfo.Outfit = resolver.GetFormatterWithVerify<FollowerOutfitType>().Deserialize(ref reader, options);
            break;
          case 19:
            followerInfo.Clothing = resolver.GetFormatterWithVerify<FollowerClothingType>().Deserialize(ref reader, options);
            break;
          case 20:
            followerInfo.ClothingVariant = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 21:
            followerInfo.ClothingPreviousVariant = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 22:
            followerInfo.Customisation = resolver.GetFormatterWithVerify<FollowerCustomisationType>().Deserialize(ref reader, options);
            break;
          case 23:
            followerInfo.Special = resolver.GetFormatterWithVerify<FollowerSpecialType>().Deserialize(ref reader, options);
            break;
          case 24:
            followerInfo.ImprisonedDay = reader.ReadInt32();
            break;
          case 25:
            followerInfo.Necklace = resolver.GetFormatterWithVerify<InventoryItem.ITEM_TYPE>().Deserialize(ref reader, options);
            break;
          case 26:
            followerInfo.IsFakeBrain = reader.ReadBoolean();
            break;
          case 27:
            followerInfo.Pets = resolver.GetFormatterWithVerify<List<FollowerPet.FollowerPetType>>().Deserialize(ref reader, options);
            break;
          case 28:
            followerInfo.DLCPets = resolver.GetFormatterWithVerify<List<FollowerPet.DLCPet>>().Deserialize(ref reader, options);
            break;
          case 29:
            followerInfo.DayJoined = reader.ReadInt32();
            break;
          case 30:
            followerInfo.TimesTurnedIntoADemon = reader.ReadInt32();
            break;
          case 31 /*0x1F*/:
            followerInfo.TimesDoneConfessionBooth = reader.ReadInt32();
            break;
          case 32 /*0x20*/:
            followerInfo.Age = reader.ReadInt32();
            break;
          case 33:
            followerInfo.LifeExpectancy = reader.ReadInt32();
            break;
          case 34:
            followerInfo.SacrificialType = resolver.GetFormatterWithVerify<InventoryItem.ITEM_TYPE>().Deserialize(ref reader, options);
            break;
          case 35:
            followerInfo.LeavingCult = reader.ReadBoolean();
            break;
          case 36:
            followerInfo.LeftCultDay = reader.ReadInt32();
            break;
          case 37:
            followerInfo.DiedOfIllness = reader.ReadBoolean();
            break;
          case 38:
            followerInfo.DiedOfInjury = reader.ReadBoolean();
            break;
          case 39:
            followerInfo.DiedOfOldAge = reader.ReadBoolean();
            break;
          case 40:
            followerInfo.DiedOfStarvation = reader.ReadBoolean();
            break;
          case 41:
            followerInfo.FrozeToDeath = reader.ReadBoolean();
            break;
          case 42:
            followerInfo.DiedFromRot = reader.ReadBoolean();
            break;
          case 43:
            followerInfo.DiedFromTwitchChat = reader.ReadBoolean();
            break;
          case 44:
            followerInfo.DiedInPrison = reader.ReadBoolean();
            break;
          case 45:
            followerInfo.DiedFromMurder = reader.ReadBoolean();
            break;
          case 46:
            followerInfo.DiedFromDeadlyDish = reader.ReadBoolean();
            break;
          case 47:
            followerInfo.DiedFromMissionary = reader.ReadBoolean();
            break;
          case 48 /*0x30*/:
            followerInfo.DiedFromLightning = reader.ReadBoolean();
            break;
          case 49:
            followerInfo.DiedFromOverheating = reader.ReadBoolean();
            break;
          case 50:
            followerInfo.BurntToDeath = reader.ReadBoolean();
            break;
          case 51:
            followerInfo.TimeOfDeath = reader.ReadInt32();
            break;
          case 52:
            followerInfo.SpouseFollowerID = reader.ReadInt32();
            break;
          case 53:
            followerInfo.BornInCult = reader.ReadBoolean();
            break;
          case 54:
            followerInfo.RottingUnique = reader.ReadBoolean();
            break;
          case 55:
            followerInfo.MurderedBy = reader.ReadInt32();
            break;
          case 56:
            followerInfo.MurderedTerm = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 57:
            followerInfo.WeatherEventIDBackToWork = reader.ReadInt32();
            break;
          case 58:
            followerInfo.HasBeenBuried = reader.ReadBoolean();
            break;
          case 59:
            followerInfo.HasReceivedNecklace = reader.ReadBoolean();
            break;
          case 60:
            followerInfo.DayBecameMutated = reader.ReadInt32();
            break;
          case 61:
            followerInfo.StartingCursedState = resolver.GetFormatterWithVerify<Thought>().Deserialize(ref reader, options);
            break;
          case 62:
            followerInfo.Faction = resolver.GetFormatterWithVerify<FollowerFaction>().Deserialize(ref reader, options);
            break;
          case 63 /*0x3F*/:
            followerInfo.FollowerRole = resolver.GetFormatterWithVerify<FollowerRole>().Deserialize(ref reader, options);
            break;
          case 64 /*0x40*/:
            followerInfo.WorkerPriority = resolver.GetFormatterWithVerify<WorkerPriority>().Deserialize(ref reader, options);
            break;
          case 65:
            followerInfo.IsDisciple = reader.ReadBoolean();
            break;
          case 66:
            followerInfo.CurrentOverrideTaskType = resolver.GetFormatterWithVerify<FollowerTaskType>().Deserialize(ref reader, options);
            break;
          case 67:
            followerInfo.CurrentOverrideStructureType = resolver.GetFormatterWithVerify<StructureBrain.TYPES>().Deserialize(ref reader, options);
            break;
          case 68:
            followerInfo.OverrideDayIndex = reader.ReadInt32();
            break;
          case 69:
            followerInfo.OverrideTaskCompleted = reader.ReadBoolean();
            break;
          case 70:
            followerInfo.Thoughts = resolver.GetFormatterWithVerify<List<ThoughtData>>().Deserialize(ref reader, options);
            break;
          case 71:
            followerInfo.Adoration = reader.ReadSingle();
            break;
          case 72:
            followerInfo.XPLevel = reader.ReadInt32();
            break;
          case 73:
            followerInfo.HomeLocation = resolver.GetFormatterWithVerify<FollowerLocation>().Deserialize(ref reader, options);
            break;
          case 74:
            followerInfo.Location = resolver.GetFormatterWithVerify<FollowerLocation>().Deserialize(ref reader, options);
            break;
          case 75:
            followerInfo.LastPosition = resolver.GetFormatterWithVerify<Vector3>().Deserialize(ref reader, options);
            break;
          case 76:
            followerInfo.SavedFollowerTaskType = resolver.GetFormatterWithVerify<FollowerTaskType>().Deserialize(ref reader, options);
            break;
          case 77:
            followerInfo.SavedFollowerTaskLocation = resolver.GetFormatterWithVerify<FollowerLocation>().Deserialize(ref reader, options);
            break;
          case 78:
            followerInfo.SavedFollowerTaskDestination = resolver.GetFormatterWithVerify<Vector3>().Deserialize(ref reader, options);
            break;
          case 79:
            followerInfo.DwellingID = reader.ReadInt32();
            break;
          case 80 /*0x50*/:
            followerInfo.PreviousDwellingID = reader.ReadInt32();
            break;
          case 81:
            followerInfo.DwellingLevel = reader.ReadInt32();
            break;
          case 82:
            followerInfo.DwellingSlot = reader.ReadInt32();
            break;
          case 83:
            followerInfo.RandomSeed = reader.ReadInt32();
            break;
          case 84:
            followerInfo.MissionaryTimestamp = reader.ReadSingle();
            break;
          case 85:
            followerInfo.MissionaryDuration = reader.ReadSingle();
            break;
          case 86:
            followerInfo.MissionaryIndex = reader.ReadInt32();
            break;
          case 87:
            followerInfo.MissionaryType = reader.ReadInt32();
            break;
          case 88:
            followerInfo.MissionaryChance = reader.ReadSingle();
            break;
          case 89:
            followerInfo.MissionaryFinished = reader.ReadBoolean();
            break;
          case 90:
            followerInfo.MissionarySuccessful = reader.ReadBoolean();
            break;
          case 91:
            followerInfo.MissionaryRewards = resolver.GetFormatterWithVerify<InventoryItem[]>().Deserialize(ref reader, options);
            break;
          case 92:
            followerInfo.FollowersReeducatedToday = resolver.GetFormatterWithVerify<List<int>>().Deserialize(ref reader, options);
            break;
          case 93:
            followerInfo.FightPitWinStreak = reader.ReadInt32();
            break;
          case 94:
            followerInfo.FightPitsFought = reader.ReadInt32();
            break;
          case 95:
            followerInfo.FollowersFought = reader.ReadInt32();
            break;
          case 96 /*0x60*/:
            followerInfo.PoemStatus = reader.ReadInt32();
            break;
          case 97:
            followerInfo.Parents = resolver.GetFormatterWithVerify<List<int>>().Deserialize(ref reader, options);
            break;
          case 98:
            followerInfo.Parent1Name = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 99:
            followerInfo.Parent2Name = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 100:
            followerInfo.Siblings = resolver.GetFormatterWithVerify<List<int>>().Deserialize(ref reader, options);
            break;
          case 101:
            followerInfo.Inventory = resolver.GetFormatterWithVerify<List<InventoryItem>>().Deserialize(ref reader, options);
            break;
          case 102:
            followerInfo.DaysSleptOutside = reader.ReadInt32();
            break;
          case 103:
            followerInfo.HadFuneral = reader.ReadBoolean();
            break;
          case 104:
            followerInfo.TargetTraits = resolver.GetFormatterWithVerify<List<FollowerTrait.TraitType>>().Deserialize(ref reader, options);
            break;
          case 105:
            followerInfo.TraitManipulateType = resolver.GetFormatterWithVerify<UITraitManipulatorMenuController.Type>().Deserialize(ref reader, options);
            break;
          case 106:
            followerInfo.Traits = resolver.GetFormatterWithVerify<List<FollowerTrait.TraitType>>().Deserialize(ref reader, options);
            break;
          case 107:
            followerInfo.BastardCounter = reader.ReadInt32();
            break;
          case 108:
            followerInfo.OldAge = reader.ReadBoolean();
            break;
          case 109:
            followerInfo.MarriedToLeader = reader.ReadBoolean();
            break;
          case 110:
            followerInfo.FirstTimeSpeakingToPlayer = reader.ReadBoolean();
            break;
          case 111:
            followerInfo.ComplainingAboutNoHouse = reader.ReadBoolean();
            break;
          case 112 /*0x70*/:
            followerInfo.ComplainingNeedBetterHouse = reader.ReadBoolean();
            break;
          case 113:
            followerInfo.NudistWinner = reader.ReadBoolean();
            break;
          case 114:
            followerInfo.TaxEnforcer = reader.ReadBoolean();
            break;
          case 115:
            followerInfo.FaithEnforcer = reader.ReadBoolean();
            break;
          case 116:
            followerInfo.ViewerID = resolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
            break;
          case 117:
            followerInfo.Pleasure = reader.ReadInt32();
            break;
          case 118:
            followerInfo.TotalPleasure = reader.ReadInt32();
            break;
          case 119:
            followerInfo.IsSnowman = reader.ReadBoolean();
            break;
          case 120:
            followerInfo._maxHP = reader.ReadSingle();
            break;
          case 121:
            followerInfo._HP = reader.ReadSingle();
            break;
          case 122:
            followerInfo.PrayProgress = reader.ReadSingle();
            break;
          case 124:
            followerInfo.DevotionGiven = reader.ReadInt32();
            break;
          case 125:
            followerInfo._happiness = reader.ReadSingle();
            break;
          case 126:
            followerInfo._faith = reader.ReadSingle();
            break;
          case (int) sbyte.MaxValue:
            followerInfo._fearLove = reader.ReadSingle();
            break;
          case 128 /*0x80*/:
            followerInfo._satiation = reader.ReadSingle();
            break;
          case 129:
            followerInfo._starvation = reader.ReadSingle();
            break;
          case 130:
            followerInfo.IsFreezing = reader.ReadBoolean();
            break;
          case 131:
            followerInfo.BathroomFillRate = reader.ReadSingle();
            break;
          case 132:
            followerInfo._bathroom = reader.ReadSingle();
            break;
          case 133:
            followerInfo._targetBathroom = reader.ReadSingle();
            break;
          case 134:
            followerInfo._social = reader.ReadSingle();
            break;
          case 135:
            followerInfo._vomit = reader.ReadSingle();
            break;
          case 136:
            followerInfo._rest = reader.ReadSingle();
            break;
          case 137:
            followerInfo._illness = reader.ReadSingle();
            break;
          case 138:
            followerInfo._injured = reader.ReadSingle();
            break;
          case 139:
            followerInfo._exhaustion = reader.ReadSingle();
            break;
          case 140:
            followerInfo._drunk = reader.ReadSingle();
            break;
          case 141:
            followerInfo._freezing = reader.ReadSingle();
            break;
          case 142:
            followerInfo._overheating = reader.ReadSingle();
            break;
          case 143:
            followerInfo._aflame = reader.ReadSingle();
            break;
          case 144 /*0x90*/:
            followerInfo._soaking = reader.ReadSingle();
            break;
          case 145:
            followerInfo.DissentDuration = reader.ReadSingle();
            break;
          case 146:
            followerInfo.DissentGold = reader.ReadSingle();
            break;
          case 147:
            followerInfo.GivenDissentWarning = reader.ReadBoolean();
            break;
          case 148:
            followerInfo._dissent = reader.ReadSingle();
            break;
          case 149:
            followerInfo.ReactionsAndTime = resolver.GetFormatterWithVerify<List<StructureAndTime>>().Deserialize(ref reader, options);
            break;
          case 150:
            followerInfo.TaskMemory = resolver.GetFormatterWithVerify<List<TaskAndTime>>().Deserialize(ref reader, options);
            break;
          case 151:
            followerInfo.InteractionCoolDownFasting = reader.ReadInt32();
            break;
          case 152:
            followerInfo.InteractionCoolDownEnergizing = reader.ReadInt32();
            break;
          case 153:
            followerInfo.InteractionCoolDownMotivational = reader.ReadInt32();
            break;
          case 154:
            followerInfo.InteractionCoolDemandDevotion = reader.ReadInt32();
            break;
          case 155:
            followerInfo.FastingUntil = reader.ReadSingle();
            break;
          case 156:
            followerInfo.GuaranteedGoodInteractionsUntil = reader.ReadInt32();
            break;
          case 157:
            followerInfo.IncreasedDevotionOutputUntil = reader.ReadInt32();
            break;
          case 158:
            followerInfo.BrainwashedUntil = reader.ReadInt32();
            break;
          case 159:
            followerInfo.MotivatedUntil = reader.ReadInt32();
            break;
          case 160 /*0xA0*/:
            followerInfo.LastBlessing = reader.ReadInt32();
            break;
          case 161:
            followerInfo.LastSermon = reader.ReadInt32();
            break;
          case 162:
            followerInfo.LastVomit = reader.ReadSingle();
            break;
          case 163:
            followerInfo.LastReeducate = reader.ReadInt32();
            break;
          case 164:
            followerInfo.LastHeal = reader.ReadInt32();
            break;
          case 165:
            followerInfo.PaidTithes = reader.ReadBoolean();
            break;
          case 166:
            followerInfo.ReceivedBlessing = reader.ReadBoolean();
            break;
          case 167:
            followerInfo.ReeducatedAction = reader.ReadBoolean();
            break;
          case 168:
            followerInfo.KissedAction = reader.ReadBoolean();
            break;
          case 169:
            followerInfo.Inspired = reader.ReadBoolean();
            break;
          case 170:
            followerInfo.PetDog = reader.ReadBoolean();
            break;
          case 171:
            followerInfo.Cuddled = reader.ReadBoolean();
            break;
          case 172:
            followerInfo.Intimidated = reader.ReadBoolean();
            break;
          case 173:
            followerInfo.Bribed = reader.ReadBoolean();
            break;
          case 174:
            followerInfo.ScaredTraitInteracted = reader.ReadBoolean();
            break;
          case 175:
            followerInfo.SozoBrainshed = reader.ReadBoolean();
            break;
          case 176 /*0xB0*/:
            followerInfo.CuddledAmount = reader.ReadInt32();
            break;
          case 177:
            followerInfo.BabyIgnored = reader.ReadBoolean();
            break;
          case 178:
            followerInfo.WakeUpDay = reader.ReadInt32();
            break;
          case 179:
            followerInfo.ReassuranceCount = reader.ReadInt32();
            break;
          case 180:
            followerInfo.SmoochCount = reader.ReadInt32();
            break;
          case 181:
            followerInfo.MissionaryExhaustion = reader.ReadSingle();
            break;
          case 182:
            followerInfo.TaxCollected = reader.ReadInt32();
            break;
          case 183:
            followerInfo.TaxedToday = reader.ReadBoolean();
            break;
          case 184:
            followerInfo.FaithedToday = reader.ReadBoolean();
            break;
          case 185:
            followerInfo.PoemProgress = reader.ReadSingle();
            break;
          case 186:
            followerInfo.CursedStateVariant = reader.ReadInt32();
            break;
          case 187:
            followerInfo.Relationships = resolver.GetFormatterWithVerify<List<IDAndRelationship>>().Deserialize(ref reader, options);
            break;
          case 188:
            followerInfo.CachedLumber = reader.ReadInt32();
            break;
          case 189:
            followerInfo.CachedLumberjackStationID = reader.ReadInt32();
            break;
          case 190:
            followerInfo.FromDragonNPC = reader.ReadBoolean();
            break;
          case 191:
            followerInfo.reeducation = reader.ReadSingle();
            break;
          default:
            reader.Skip();
            break;
        }
      }
      --reader.Depth;
      return followerInfo;
    }
  }
}
