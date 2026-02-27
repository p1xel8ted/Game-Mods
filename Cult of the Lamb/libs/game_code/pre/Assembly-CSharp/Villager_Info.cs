// Decompiled with JetBrains decompiler
// Type: Villager_Info
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

#nullable disable
public class Villager_Info
{
  public int ID;
  public string Name;
  public int SkinVariation;
  public int SkinColour;
  public int Age;
  public float color_r;
  public float color_g;
  public float color_b;
  private int RandomSkin;
  public string SkinName;
  public Villager_Info.Faction MyFaction;
  public WorshipperInfoManager.Outfit Outfit;
  public string WorkPlace;
  public int WorkPlaceSlot;
  public string Dwelling = "-1";
  public int DwellingSlot;
  public bool DwellingClaimed;
  public float HP;
  public float TotalHP;
  public bool SleptOutside;
  public Vector3 SleptOutsidePosition;
  public static System.Action OnFaithChanged;
  private float _Faith = 70f;
  public float FearLove = 50f;
  public static float FearThreshold = 20f;
  public static float LoveThreshold = 80f;
  public static float IllnessThreshold = 50f;
  public float Hunger = 100f;
  public static Villager_Info.StatusEffectEvent OnStarveChanged;
  [XmlIgnore]
  public Villager_Info.StatusEffectEvent OnStarve;
  public float _Starve;
  public static int StarveDeath = 300;
  public static float LowFaithThreshold = 30f;
  public float Sleep;
  public int DevotionGiven;
  public float Devotion;
  public int Level = 1;
  public static Villager_Info.StatusEffectEvent OnIllnessChanged;
  [XmlIgnore]
  public Villager_Info.StatusEffectEvent OnIllness;
  private float _Illness;
  public static Villager_Info.StatusEffectEvent OnDissenterChanged;
  [XmlIgnore]
  public Villager_Info.StatusEffectEvent OnDissenter;
  public bool isDissenter;
  private float _Dissentor = 50f;
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
  public bool Complaint_House;
  public bool Complaint_Food;
  public int GuaranteedGoodInteractionsUntil = -1;
  public int FastingUntil = -1;
  public int IncreasedDevotionOutputUntil = -1;
  public int BrainwashedUntil = -1;
  public int MotivatedUntil = -1;
  public List<IDAndRelationship> Relationships = new List<IDAndRelationship>();

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

  public bool Fasting => this.FastingUntil >= DataManager.Instance.CurrentDayIndex;

  public void DecreaseHunger(float SpeedMultiplier)
  {
    if (!this.Fasting)
      this.Hunger = Mathf.Clamp(this.Hunger - Time.deltaTime * 1f * SpeedMultiplier, -100f, 100f);
    Debug.Log((object) $"{this.Name}  {(object) this.Hunger}");
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
