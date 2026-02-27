// Decompiled with JetBrains decompiler
// Type: FollowerInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

#nullable disable
public class FollowerInfo
{
  public int ID;
  public string Name;
  public Thought CursedState;
  public bool WorkerBeenGivenOrders = true;
  public float follower_pitch;
  public float follower_vibrato;
  public int SermonCountAfterlife;
  public int SermonCountFood;
  public int SermonCountLawAndOrder;
  public int SermonCountPossession;
  public int SermonCountWorkAndWorship;
  public bool TraitsSet;
  public int SkinCharacter;
  public int SkinVariation;
  public int SkinColour;
  public string SkinName;
  public FollowerOutfitType Outfit;
  public InventoryItem.ITEM_TYPE Necklace;
  public bool IsFakeBrain;
  public List<FollowerPet.FollowerPetType> Pets = new List<FollowerPet.FollowerPetType>();
  public int DayJoined;
  public int Age;
  public int LifeExpectancy;
  public InventoryItem.ITEM_TYPE SacrificialType;
  public bool LeavingCult;
  public bool DiedOfIllness;
  public bool DiedOfOldAge;
  public bool DiedOfStarvation;
  public bool DiedFromTwitchChat;
  public bool DiedInPrison;
  public bool DiedFromDeadlyDish;
  public Thought StartingCursedState;
  public FollowerFaction Faction;
  public FollowerRole FollowerRole;
  public WorkerPriority WorkerPriority;
  public FollowerTaskType CurrentOverrideTaskType;
  public StructureBrain.TYPES CurrentOverrideStructureType;
  public int OverrideDayIndex;
  public bool OverrideTaskCompleted;
  public List<ThoughtData> Thoughts = new List<ThoughtData>();
  public float Adoration;
  public int FollowerLevel;
  public int XPLevel = 1;
  public FollowerLocation HomeLocation;
  public FollowerLocation Location;
  public Vector3 LastPosition;
  public FollowerTaskType SavedFollowerTaskType;
  public FollowerLocation SavedFollowerTaskLocation;
  public Vector3 SavedFollowerTaskDestination;
  public int DwellingID;
  public int PreviousDwellingID;
  public int DwellingLevel;
  public int DwellingSlot;
  public int RandomSeed;
  public float MissionaryTimestamp;
  public float MissionaryDuration = 2400f;
  public int MissionaryIndex;
  public int MissionaryType;
  public float MissionaryChance;
  public bool MissionaryFinished;
  public bool MissionarySuccessful;
  public InventoryItem[] MissionaryRewards;
  public List<InventoryItem> Inventory = new List<InventoryItem>();
  public int DaysSleptOutside;
  public bool HadFuneral;
  public List<FollowerTrait.TraitType> Traits = new List<FollowerTrait.TraitType>();
  public const float MIN_HP = 0.0f;
  public bool OldAge;
  public bool MarriedToLeader;
  public bool FirstTimeSpeakingToPlayer = true;
  public bool ComplainingAboutNoHouse = true;
  public bool ComplainingNeedBetterHouse;
  public bool TaxEnforcer;
  public bool FaithEnforcer;
  public string ViewerID;
  private float _maxHP;
  private float _HP;
  public const float DEVOTION_PER_GAME_DAY = 5f;
  public float PrayProgress;
  public int DevotionGiven;
  public const float MIN_HAPPINESS = 0.0f;
  public const float MAX_HAPPINESS = 100f;
  public const float LOW_HAPPINESS_THRESHOLD = 25f;
  public const float CRITICAL_HAPPINESS_THRESHOLD = 10f;
  public const float HAPPINESS_GAME_MINUTES_TO_DEPLETE = 6600f;
  private float _happiness;
  public const float MIN_FAITH = 0.0f;
  public const float MAX_FAITH = 100f;
  public const float LOW_FAITH_THRESHOLD = 30f;
  public const float CRITICAL_FAITH_THRESHOLD = 5f;
  private float _faith;
  public const float MIN_FEARLOVE = 0.0f;
  public const float MAX_FEARLOVE = 100f;
  public const float FEAR_THRESHOLD = 20f;
  public const float LOVE_THRESHOLD = 80f;
  private float _fearLove;
  public const float MIN_SATIATION = 0.0f;
  public const float MAX_SATIATION = 100f;
  public const float EAT_THRESHOLD = 60f;
  public const float HUNGER_THRESHOLD = 30f;
  public const float SATIATION_GAME_MINUTES_TO_DEPLETE = 2400f;
  private float _satiation;
  public const float MIN_STARVATION = 0.0f;
  public const float MAX_STARVATION = 75f;
  private float _starvation;
  public bool IsStarving;
  public const float MIN_BATHROOM = 0.0f;
  public const float MAX_BATHROOM = 30f;
  public float BathroomFillRate = UnityEngine.Random.Range(0.5f, 1.5f);
  public const float BATHRHOOM_THRESHOLD = 15f;
  public const float BATHROOM_GAME_MINUTES_TO_FULL = 180f;
  private float _bathroom;
  private float _targetBathroom;
  public const float MIN_SOCIAL = 0.0f;
  public const float MAX_SOCIAL = 100f;
  public const float SOCIAL_GAME_MINUTES_TO_FULL = 200f;
  private float _social = 100f;
  public const float MIN_VOMIT = 0.0f;
  public const float MAX_VOMIT = 30f;
  public const float VOMIT_GAME_MINUTES_TO_FULL = 320f;
  private float _vomit;
  public const float MIN_REST = 0.0f;
  public const float TIRED_THRESHOLD = 20f;
  public const float MAX_REST = 100f;
  public const float REST_GAME_MINUTES_TO_DEPLETE = 1200f;
  private float _rest;
  public const float MIN_ILLNESS = 0.0f;
  public const float MAX_ILLNESS = 100f;
  public const float ILLNESS_GAME_MINUTES_TO_FULL = 3600f;
  public const float MIN_EXHAUSTION = 0.0f;
  public const float MAX_EXHAUSTION = 100f;
  public const float EXHAUSTION_GAME_MINUTES_TO_REMOVE = 480f;
  private float _illness;
  private float _exhaustion;
  public const float MIN_DISSENT = 0.0f;
  public const float MAX_DISSENT = 100f;
  public const float DISSENTER_THRESHOLD = 80f;
  public float DissentDuration = -1f;
  public float DissentGold;
  public bool GivenDissentWarning;
  private float _dissent;
  private float reeducation;
  public const float MIN_REEDUCATE = 0.0f;
  public const float MAX_REEDUCATE = 100f;
  public const float REEDUCATE_GAME_MINUTES_TO_FULL = 3600f;
  public const float REEDUCATE_PRISON_BONUS = 25f;
  public const float REEDUCATE_INTERACTION_BONUS = 7.5f;
  public const float EXHAUSTION_GAME_MINUTES_TO_FULL = 900f;
  public List<StructureAndTime> ReactionsAndTime = new List<StructureAndTime>();
  public List<TaskAndTime> TaskMemory = new List<TaskAndTime>();
  public const float INTERACTIONCOOLDOWN_DEMAND_DEVOTION = 1f;
  public const float INTERACTIONCOOLDOWN_FAST = 5f;
  public const float INTERACTIONCOOLDOWN_ENERGISING_SPEECH = 2f;
  public const float INTERACTIONCOOLDOWN_MOTIVATIONAL_SPEECH = 2f;
  public int InteractionCoolDownFasting = -1;
  public int InteractionCoolDownEnergizing = -1;
  public int InteractionCoolDownMotivational = -1;
  public int InteractionCoolDemandDevotion = -1;
  public float FastingUntil;
  public int GuaranteedGoodInteractionsUntil;
  public int IncreasedDevotionOutputUntil;
  public int BrainwashedUntil;
  public int MotivatedUntil;
  public int LastBlessing;
  public int LastSermon;
  public float LastVomit;
  public int LastReeducate;
  public int LastHeal;
  public bool PaidTithes;
  public bool ReceivedBlessing;
  public bool ReeducatedAction;
  public bool KissedAction;
  public bool Inspired;
  public bool PetDog;
  public bool Intimidated;
  public bool Bribed;
  public int WakeUpDay = -1;
  public float MissionaryExhaustion = 1f;
  public const int MaxMissionaryExhaustion = 5;
  public int TaxCollected;
  public bool TaxedToday;
  public bool FaithedToday;
  public int CursedStateVariant;
  public const float RELATIONSHIP_THRESHOLD_HATE = -10f;
  public const float RELATIONSHIP_THRESHOLD_FRIEND = 5f;
  public const float RELATIONSHIP_THRESHOLD_LOVE = 10f;
  public List<IDAndRelationship> Relationships = new List<IDAndRelationship>();
  private const int h = 240 /*0xF0*/;
  public int CachedLumber;
  public int CachedLumberjackStationID;
  private static List<string> NameBeginnings = new List<string>()
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
  private static List<string> NameMiddles = new List<string>()
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
  private static List<string> NameEndings = new List<string>()
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
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
    {
      if (follower.ID == ID)
        return follower;
    }
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

  private FollowerInfo()
  {
  }

  public string GetNameFormatted()
  {
    return string.Join(" - ", this.Name, $"{ScriptLocalization.Interactions.Level} {this.XPLevel.ToNumeral()}") + (this.MarriedToLeader ? " <sprite name=\"icon_Married\">" : string.Empty);
  }

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

  public float HP
  {
    get => this._HP;
    set => this._HP = Mathf.Clamp(value, 0.0f, this.MaxHP);
  }

  public float Happiness
  {
    get => this._happiness;
    set => this._happiness = Mathf.Clamp(value, 0.0f, 100f);
  }

  public float Faith
  {
    get => this._faith;
    set => this._faith = Mathf.Clamp(value, 0.0f, 100f);
  }

  public float FearLove
  {
    get => this._fearLove;
    set => this._fearLove = Mathf.Clamp(value, 0.0f, 100f);
  }

  public float Satiation
  {
    get => this._satiation;
    set => this._satiation = Mathf.Clamp(value, 0.0f, 100f);
  }

  public float Starvation
  {
    get => this._starvation;
    set => this._starvation = Mathf.Clamp(value, 0.0f, 75f);
  }

  public float Bathroom
  {
    get => this._bathroom;
    set => this._bathroom = Mathf.Clamp(value, 0.0f, 30f);
  }

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

  public float Social
  {
    get => this._social;
    set => this._social = Mathf.Clamp(value, 0.0f, 100f);
  }

  public float Vomit
  {
    get => this._vomit;
    set => this._vomit = Mathf.Clamp(value, 0.0f, 30f);
  }

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

  public float Illness
  {
    get => this._illness;
    set => this._illness = Mathf.Clamp(value, 0.0f, 100f);
  }

  public float Exhaustion
  {
    get => this._exhaustion;
    set => this._exhaustion = Mathf.Clamp(value, 0.0f, 100f);
  }

  public float Dissent
  {
    get => this._dissent;
    set => this._dissent = Mathf.Clamp(value, 0.0f, 100f);
  }

  public float Reeducation
  {
    get => this.reeducation;
    set => this.reeducation = Mathf.Clamp(value, 0.0f, 100f);
  }

  public ObjectivesData CurrentPlayerQuest
  {
    get
    {
      List<ObjectivesData> currentFollowerQuests = Quests.GetCurrentFollowerQuests(this.ID);
      return currentFollowerQuests.Count <= 0 ? (ObjectivesData) null : currentFollowerQuests[0];
    }
  }

  public bool WorkThroughNight
  {
    get
    {
      return this.WakeUpDay == TimeManager.CurrentDay || this.Necklace == InventoryItem.ITEM_TYPE.Necklace_5 || FollowerBrainStats.IsWorkThroughTheNight;
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
    this.IsStarving = false;
  }

  public static FollowerInfo NewCharacter(FollowerLocation location, string ForceSkin = "")
  {
    FollowerInfo followerInfo = new FollowerInfo();
    followerInfo.Name = FollowerInfo.GenerateName();
    followerInfo.Age = UnityEngine.Random.Range(20, 30);
    followerInfo.LifeExpectancy = UnityEngine.Random.Range(40, 55);
    followerInfo.DayJoined = TimeManager.CurrentDay;
    followerInfo.ID = ++DataManager.Instance.FollowerID;
    if (followerInfo.ID == 666)
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
      followerInfo.SkinColour = UnityEngine.Random.Range(0, WorshipperData.Instance.GetColourData(followerInfo.SkinName).StartingSlotAndColours.Count);
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

  public string GetDeathText()
  {
    string str = $"<color=#FFD201>{this.Name}</color>";
    if (this.DiedOfIllness)
      return $"{str} {ScriptLocalization.Notifications.DiedFromIllness}";
    if (this.DiedOfOldAge)
      return $"{str} {ScriptLocalization.Notifications.DiedFromOldAge}";
    if (this.DiedOfStarvation)
      return $"{str} {ScriptLocalization.Notifications.DiedFromStarvation}";
    if (this.DiedFromTwitchChat)
      return $"{str} {LocalizationManager.GetTranslation("Notifications/DiedFromTwitchChat")}";
    return this.DiedFromDeadlyDish ? $"{str} {LocalizationManager.GetTranslation("Notifications/DiedFromDeadlyDish")}" : "";
  }
}
