// Decompiled with JetBrains decompiler
// Type: Villager_Info
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

#nullable disable
[MessagePackObject(false)]
public class Villager_Info
{
  [Key(0)]
  public int ID;
  [Key(1)]
  public string Name;
  [Key(2)]
  public int SkinVariation;
  [Key(3)]
  public int SkinColour;
  [Key(4)]
  public int Age;
  [Key(5)]
  public float color_r;
  [Key(6)]
  public float color_g;
  [Key(7)]
  public float color_b;
  public int RandomSkin;
  [Key(8)]
  public string SkinName;
  [Key(9)]
  public Villager_Info.Faction MyFaction;
  [Key(10)]
  public WorshipperInfoManager.Outfit Outfit;
  [Key(11)]
  public string WorkPlace;
  [Key(12)]
  public int WorkPlaceSlot;
  [Key(13)]
  public string Dwelling = "-1";
  [Key(14)]
  public int DwellingSlot;
  [Key(15)]
  public bool DwellingClaimed;
  [Key(16 /*0x10*/)]
  public float HP;
  [Key(17)]
  public float TotalHP;
  [Key(18)]
  public bool SleptOutside;
  [Key(19)]
  public Vector3 SleptOutsidePosition;
  public static System.Action OnFaithChanged;
  public float _Faith = 70f;
  [Key(20)]
  public float FearLove = 50f;
  public static float FearThreshold = 20f;
  public static float LoveThreshold = 80f;
  public static float IllnessThreshold = 50f;
  [Key(21)]
  public float Hunger = 100f;
  public static Villager_Info.StatusEffectEvent OnStarveChanged;
  [XmlIgnore]
  [IgnoreMember]
  public Villager_Info.StatusEffectEvent OnStarve;
  [Key(22)]
  public float _Starve;
  public static int StarveDeath = 300;
  public static float LowFaithThreshold = 30f;
  [Key(23)]
  public float Sleep;
  [Key(24)]
  public int DevotionGiven;
  [Key(25)]
  public float Devotion;
  [Key(26)]
  public int Level = 1;
  public static Villager_Info.StatusEffectEvent OnIllnessChanged;
  [XmlIgnore]
  [IgnoreMember]
  public Villager_Info.StatusEffectEvent OnIllness;
  public float _Illness;
  public static Villager_Info.StatusEffectEvent OnDissenterChanged;
  [XmlIgnore]
  [IgnoreMember]
  public Villager_Info.StatusEffectEvent OnDissenter;
  [Key(27)]
  public bool isDissenter;
  public float _Dissentor = 50f;
  public static float DissentorThreshold = 80f;
  public static float RelationshipHateThreshold = -10f;
  public static float RelationshipFriendThreshold = 5f;
  public static float RelationshipLoveThreshold = 10f;
  public static List<int> LevelTargets = new List<int>()
  {
    50,
    100,
    200,
    300,
    400,
    500
  };
  [Key(28)]
  public bool Complaint_House;
  [Key(29)]
  public bool Complaint_Food;
  [Key(30)]
  public int GuaranteedGoodInteractionsUntil = -1;
  [Key(31 /*0x1F*/)]
  public int FastingUntil = -1;
  [Key(32 /*0x20*/)]
  public int IncreasedDevotionOutputUntil = -1;
  [Key(33)]
  public int BrainwashedUntil = -1;
  [Key(34)]
  public int MotivatedUntil = -1;
  [Key(35)]
  public List<IDAndRelationship> Relationships = new List<IDAndRelationship>();

  [IgnoreMember]
  public float Faith
  {
    get => this._Faith;
    set
    {
      float num = Mathf.Clamp(value, 0.0f, 100f);
      if (this.Brainwashed)
        num = 100f;
      if ((double) num == (double) this._Faith)
        return;
      this._Faith = num;
      System.Action onFaithChanged = Villager_Info.OnFaithChanged;
      if (onFaithChanged == null)
        return;
      onFaithChanged();
    }
  }

  [IgnoreMember]
  public float Starve
  {
    get => this._Starve;
    set
    {
      if ((double) this._Starve == (double) value)
        return;
      float starve = this._Starve;
      this._Starve = value;
      if ((double) value <= 0.0 && (double) starve > 0.0)
      {
        Villager_Info.StatusEffectEvent onStarve = this.OnStarve;
        if (onStarve != null)
          onStarve(Villager_Info.StatusState.Off);
        Villager_Info.StatusEffectEvent onStarveChanged = Villager_Info.OnStarveChanged;
        if (onStarveChanged != null)
          onStarveChanged(Villager_Info.StatusState.Off);
      }
      if ((double) value > 0.0 && (double) starve <= 0.0)
      {
        Villager_Info.StatusEffectEvent onStarve = this.OnStarve;
        if (onStarve != null)
          onStarve(Villager_Info.StatusState.On);
        Villager_Info.StatusEffectEvent onStarveChanged = Villager_Info.OnStarveChanged;
        if (onStarveChanged != null)
          onStarveChanged(Villager_Info.StatusState.On);
      }
      if ((double) value < (double) Villager_Info.StarveDeath || (double) starve >= (double) Villager_Info.StarveDeath)
        return;
      Villager_Info.StatusEffectEvent onStarve1 = this.OnStarve;
      if (onStarve1 != null)
        onStarve1(Villager_Info.StatusState.Kill);
      Villager_Info.StatusEffectEvent onStarveChanged1 = Villager_Info.OnStarveChanged;
      if (onStarveChanged1 == null)
        return;
      onStarveChanged1(Villager_Info.StatusState.Kill);
    }
  }

  [IgnoreMember]
  public float Illness
  {
    get => this._Illness;
    set
    {
      if ((double) value != (double) this._Illness)
      {
        if ((double) value >= (double) Villager_Info.IllnessThreshold && (double) this._Illness < (double) Villager_Info.IllnessThreshold)
        {
          Debug.Log((object) "Illness on!");
          Villager_Info.StatusEffectEvent onIllness = this.OnIllness;
          if (onIllness != null)
            onIllness(Villager_Info.StatusState.On);
          Villager_Info.StatusEffectEvent onIllnessChanged = Villager_Info.OnIllnessChanged;
          if (onIllnessChanged != null)
            onIllnessChanged(Villager_Info.StatusState.On);
        }
        if ((double) value < (double) Villager_Info.IllnessThreshold && (double) this._Illness >= (double) Villager_Info.IllnessThreshold)
        {
          Debug.Log((object) "Illness off!");
          Villager_Info.StatusEffectEvent onIllness = this.OnIllness;
          if (onIllness != null)
            onIllness(Villager_Info.StatusState.Off);
          Villager_Info.StatusEffectEvent onIllnessChanged = Villager_Info.OnIllnessChanged;
          if (onIllnessChanged != null)
            onIllnessChanged(Villager_Info.StatusState.Off);
        }
        else if ((double) value >= 100.0)
        {
          Debug.Log((object) "Illness kill");
          Villager_Info.StatusEffectEvent onIllness = this.OnIllness;
          if (onIllness != null)
            onIllness(Villager_Info.StatusState.Kill);
          Villager_Info.StatusEffectEvent onIllnessChanged = Villager_Info.OnIllnessChanged;
          if (onIllnessChanged != null)
            onIllnessChanged(Villager_Info.StatusState.Kill);
        }
      }
      this._Illness = Mathf.Clamp(value, 0.0f, 100f);
    }
  }

  [IgnoreMember]
  public float Dissentor
  {
    get => this._Dissentor;
    set
    {
      if ((double) value != (double) this._Dissentor)
      {
        if ((double) value >= (double) Villager_Info.DissentorThreshold && (double) this._Dissentor < (double) Villager_Info.DissentorThreshold)
        {
          this.isDissenter = true;
          Villager_Info.StatusEffectEvent onDissenter = this.OnDissenter;
          if (onDissenter != null)
            onDissenter(Villager_Info.StatusState.On);
          Villager_Info.StatusEffectEvent dissenterChanged = Villager_Info.OnDissenterChanged;
          if (dissenterChanged != null)
            dissenterChanged(Villager_Info.StatusState.On);
        }
        else if ((double) value < (double) Villager_Info.DissentorThreshold && (double) this._Dissentor >= (double) Villager_Info.DissentorThreshold)
        {
          this.isDissenter = false;
          Villager_Info.StatusEffectEvent onDissenter = this.OnDissenter;
          if (onDissenter != null)
            onDissenter(Villager_Info.StatusState.Off);
          Villager_Info.StatusEffectEvent dissenterChanged = Villager_Info.OnDissenterChanged;
          if (dissenterChanged != null)
            dissenterChanged(Villager_Info.StatusState.Off);
        }
      }
      this._Dissentor = Mathf.Clamp(value, 0.0f, 100f);
    }
  }

  public int GetDevotionLimit()
  {
    switch (this.Level)
    {
      case 2:
        return 15;
      case 3:
        return 30;
      case 4:
        return 50;
      default:
        return 5;
    }
  }

  [IgnoreMember]
  public bool Fasting => this.FastingUntil >= DataManager.Instance.CurrentDayIndex;

  public void DecreaseHunger(float SpeedMultiplier)
  {
    if (!this.Fasting)
      this.Hunger = Mathf.Clamp(this.Hunger - Time.deltaTime * 1f * SpeedMultiplier, -100f, 100f);
    Debug.Log((object) $"{this.Name}  {this.Hunger.ToString()}");
  }

  public void IncreaseDevotion(float SpeedMultiplier)
  {
    this.Devotion += (float) ((double) SpeedMultiplier * (double) Time.deltaTime / 60.0);
    this.Devotion = Mathf.Clamp(this.Devotion, 0.0f, (float) this.GetDevotionLimit());
  }

  public static float TotalFaithNormalised()
  {
    if (DataManager.Instance.Followers.Count <= 0)
      return 0.0f;
    float num = 0.0f;
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
      num += follower.Faith;
    return num / (100f * (float) DataManager.Instance.Followers.Count);
  }

  public void DecreaseCultFaith(float Speed) => this.Faith = 50f;

  public void IncreaseCultFaith(float Speed) => this.Faith = 50f;

  [IgnoreMember]
  public bool Brainwashed => this.BrainwashedUntil >= DataManager.Instance.CurrentDayIndex;

  public void Brainwash(int durationDays)
  {
    int num = this.Brainwashed ? 1 : 0;
    this.BrainwashedUntil = DataManager.Instance.CurrentDayIndex + (durationDays - 1);
    this.Faith = 100f;
  }

  public static FollowerInfo GetVillagerInfoByID(int ID)
  {
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
    {
      if (follower.ID == ID)
        return follower;
    }
    return (FollowerInfo) null;
  }

  public static Villager_Info NewCharacter(string ForceSkin = "")
  {
    Villager_Info villagerInfo = new Villager_Info();
    villagerInfo.Name = Villager_Info.GenerateName();
    villagerInfo.Age = UnityEngine.Random.Range(15, 30);
    villagerInfo.ID = ++DataManager.Instance.FollowerID;
    if (ForceSkin == "")
    {
      int index = UnityEngine.Random.Range(0, WorshipperData.Instance.Characters.Count);
      villagerInfo.SkinVariation = UnityEngine.Random.Range(0, WorshipperData.Instance.Characters[index].Skin.Count);
      villagerInfo.SkinName = WorshipperData.Instance.Characters[index].Skin[villagerInfo.SkinVariation].Skin;
      villagerInfo.SkinColour = UnityEngine.Random.Range(0, WorshipperData.Instance.GetColourData(villagerInfo.SkinName).SlotAndColours.Count);
    }
    else
      villagerInfo.SkinName = ForceSkin;
    villagerInfo.Sleep = (float) UnityEngine.Random.Range(30, 60);
    villagerInfo.Hunger = 100f;
    villagerInfo.MyFaction = (Villager_Info.Faction) UnityEngine.Random.Range(1, 4);
    villagerInfo.Devotion = (float) villagerInfo.GetDevotionLimit();
    villagerInfo.WorkPlace = "-1";
    villagerInfo.Dwelling = "-1";
    villagerInfo.DwellingClaimed = false;
    villagerInfo.HP = villagerInfo.TotalHP = 25f;
    return villagerInfo;
  }

  public bool CanLevelUp() => this.DevotionGiven >= Villager_Info.LevelTargets[this.Level - 1];

  public static string GenerateName()
  {
    string str1 = "";
    List<string> stringList1 = new List<string>()
    {
      "Ja",
      "Jul",
      "Na",
      "No",
      "Gre",
      "Bre",
      "Tre",
      "Mer",
      "Ty",
      "Ar",
      "An",
      "Yar",
      "Fe",
      "Fi",
      "The",
      "Thor"
    };
    string str2 = str1 + stringList1[UnityEngine.Random.Range(0, stringList1.Count)];
    List<string> stringList2 = new List<string>()
    {
      "na",
      "ya",
      "len",
      "lay",
      "no",
      "tha",
      "ka",
      "ki",
      "ko",
      "li",
      "lo"
    };
    if ((double) UnityEngine.Random.Range(0.0f, 1f) < 0.5)
      str2 += stringList2[UnityEngine.Random.Range(0, stringList2.Count)];
    List<string> stringList3 = new List<string>()
    {
      "on",
      "y",
      "an",
      "yen",
      "son",
      "ryn",
      "nor",
      "mar"
    };
    return str2 + stringList3[UnityEngine.Random.Range(0, stringList3.Count)];
  }

  public enum Faction
  {
    Generic,
    Fundamentalist,
    Utopianist,
    Misfit,
  }

  public enum StatusState
  {
    Off,
    On,
    Kill,
  }

  public delegate void StatusEffectEvent(Villager_Info.StatusState State);
}
