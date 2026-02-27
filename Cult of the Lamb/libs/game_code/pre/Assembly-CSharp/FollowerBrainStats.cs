// Decompiled with JetBrains decompiler
// Type: FollowerBrainStats
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerBrainStats
{
  private int _followerID;
  private FollowerInfo _info;
  private FollowerBrain followerBrain;
  public static FollowerBrainStats.StatChangedEvent OnLevelUp;
  public static FollowerBrainStats.StatChangedEvent OnMaxHPChanged;
  public static FollowerBrainStats.StatChangedEvent OnHPChanged;
  public static FollowerBrainStats.StatChangedEvent OnDevotionChanged;
  public static FollowerBrainStats.StatChangedEvent OnDevotionGivenChanged;
  public static FollowerBrainStats.StatStateChangedEvent OnHappinessStateChanged;
  public static FollowerBrainStats.StatChangedEvent OnFearLoveChanged;
  public static FollowerBrainStats.StatChangedEvent OnSatiationChanged;
  public static FollowerBrainStats.StatStateChangedEvent OnSatiationStateChanged;
  public static FollowerBrainStats.StatChangedEvent OnStarvationChanged;
  public static FollowerBrainStats.StatStateChangedEvent OnStarvationStateChanged;
  public static FollowerBrainStats.StatChangedEvent OnBathroomChanged;
  public static FollowerBrainStats.StatStateChangedEvent OnBathroomStateChanged;
  public static FollowerBrainStats.StatChangedEvent OnRestChanged;
  public static FollowerBrainStats.StatStateChangedEvent OnRestStateChanged;
  public static FollowerBrainStats.StatChangedEvent OnIllnessChanged;
  public static FollowerBrainStats.StatStateChangedEvent OnIllnessStateChanged;
  public static FollowerBrainStats.StatChangedEvent OnExhaustionChanged;
  public static FollowerBrainStats.StatStateChangedEvent OnExhaustionStateChanged;
  public static FollowerBrainStats.StatChangedEvent OnReeducationChanged;
  public static FollowerBrainStats.StatStateChangedEvent OnReeducationStateChanged;
  public System.Action OnReeducationComplete;
  public static FollowerBrainStats.StatusChangedEvent OnMotivatedChanged;

  public FollowerBrainStats(FollowerInfo info, FollowerBrain followerBrain)
  {
    this._followerID = info.ID;
    this.followerBrain = followerBrain;
    this._info = info;
  }

  public bool WorkerBeenGivenOrders
  {
    get => this._info.WorkerBeenGivenOrders;
    set
    {
      if (this._info.WorkerBeenGivenOrders != value && value)
        this.followerBrain.CheckChangeTask();
      this._info.WorkerBeenGivenOrders = value;
    }
  }

  public float MaxHP
  {
    get => this._info.MaxHP;
    set
    {
      float maxHp = this.MaxHP;
      this._info.MaxHP = value;
      if ((double) maxHp == (double) this.MaxHP)
        return;
      FollowerBrainStats.StatChangedEvent onMaxHpChanged = FollowerBrainStats.OnMaxHPChanged;
      if (onMaxHpChanged == null)
        return;
      onMaxHpChanged(this._followerID, this.MaxHP, maxHp, this.MaxHP - maxHp);
    }
  }

  public float HP
  {
    get => this._info.HP;
    set
    {
      float hp = this.HP;
      this._info.HP = value;
      if ((double) hp == (double) this.HP)
        return;
      FollowerBrainStats.StatChangedEvent onHpChanged = FollowerBrainStats.OnHPChanged;
      if (onHpChanged == null)
        return;
      onHpChanged(this._followerID, this.HP, hp, this.HP - hp);
    }
  }

  public int DevotionGiven
  {
    get => this._info.DevotionGiven;
    set
    {
      int devotionGiven = this.DevotionGiven;
      if (devotionGiven == this.DevotionGiven)
        return;
      FollowerBrainStats.StatChangedEvent devotionGivenChanged = FollowerBrainStats.OnDevotionGivenChanged;
      if (devotionGivenChanged == null)
        return;
      devotionGivenChanged(this._followerID, (float) this.DevotionGiven, (float) devotionGiven, (float) (this.DevotionGiven - devotionGiven));
    }
  }

  public bool HasLevelledUp => (double) this._info.Adoration >= (double) this.MAX_ADORATION;

  public float Adoration
  {
    get => this._info.Adoration;
    set => this._info.Adoration = value;
  }

  public float MAX_ADORATION => 100f;

  public int LastSermon
  {
    get => this._info.LastSermon;
    set => this._info.LastSermon = value;
  }

  public bool HadSermonYesterday
  {
    get => this._info.LastSermon == DataManager.Instance.CurrentDayIndex - 1;
  }

  public bool PaidTithes
  {
    set => this._info.PaidTithes = value;
    get => this._info.PaidTithes;
  }

  public bool ReceivedBlessing
  {
    set => this._info.ReceivedBlessing = value;
    get => this._info.ReceivedBlessing;
  }

  public bool KissedAction
  {
    set => this._info.KissedAction = value;
    get => this._info.KissedAction;
  }

  public bool ReeducatedAction
  {
    set => this._info.ReeducatedAction = value;
    get => this._info.ReeducatedAction;
  }

  public bool Inspired
  {
    set => this._info.Inspired = value;
    get => this._info.Inspired;
  }

  public bool PetDog
  {
    set => this._info.PetDog = value;
    get => this._info.PetDog;
  }

  public bool Intimidated
  {
    set => this._info.Intimidated = value;
    get => this._info.Intimidated;
  }

  public bool Bribed
  {
    set => this._info.Bribed = value;
    get => this._info.Bribed;
  }

  public List<ThoughtData> Thoughts
  {
    get => this._info.Thoughts;
    set => this._info.Thoughts = value;
  }

  public float Happiness => 50f;

  public void OnThoughtsChanged(FollowerBrain brain, float OldValue, float NewValue)
  {
  }

  public float FearLove
  {
    get => this._info.FearLove;
    set
    {
      float fearLove = this.FearLove;
      this._info.FearLove = value;
      if ((double) fearLove == (double) this.FearLove)
        return;
      FollowerBrainStats.StatChangedEvent onFearLoveChanged = FollowerBrainStats.OnFearLoveChanged;
      if (onFearLoveChanged == null)
        return;
      onFearLoveChanged(this._followerID, this.FearLove, fearLove, this.FearLove - fearLove);
    }
  }

  public float Satiation
  {
    get => this._info.Satiation;
    set
    {
      if (FollowerBrainStats.Fasting && (double) value < (double) this.Satiation)
        return;
      if ((double) value <= 0.0 && DataManager.Instance.CookedFirstFood)
      {
        if (this._info.IsStarving && !TimeManager.PauseGameTime)
          this.Starvation += Mathf.Abs(value - 0.0f) * (PlayerFarming.Location == this.followerBrain.Location ? 1f : 0.5f);
      }
      else if ((double) value > (double) this.Satiation)
      {
        this.Starvation = 0.0f;
        this.IsStarving = false;
      }
      float satiation = this.Satiation;
      this._info.Satiation = value;
      if ((double) satiation == (double) this.Satiation)
        return;
      FollowerBrainStats.StatChangedEvent satiationChanged = FollowerBrainStats.OnSatiationChanged;
      if (satiationChanged != null)
        satiationChanged(this._followerID, this.Satiation, satiation, this.Satiation - satiation);
      if ((double) satiation <= 30.0 && (double) this.Satiation > 30.0)
      {
        FollowerBrainStats.StatStateChangedEvent satiationStateChanged = FollowerBrainStats.OnSatiationStateChanged;
        if (satiationStateChanged == null)
          return;
        satiationStateChanged(this._followerID, FollowerStatState.Off, FollowerStatState.On);
      }
      else
      {
        if ((double) satiation <= 30.0 || (double) this.Satiation > 30.0)
          return;
        FollowerBrainStats.StatStateChangedEvent satiationStateChanged = FollowerBrainStats.OnSatiationStateChanged;
        if (satiationStateChanged == null)
          return;
        satiationStateChanged(this._followerID, FollowerStatState.On, FollowerStatState.Off);
      }
    }
  }

  public bool IsStarving
  {
    get => this._info.IsStarving;
    set => this._info.IsStarving = value;
  }

  public float Starvation
  {
    get => this._info.Starvation;
    set
    {
      float starvation = this.Starvation;
      this._info.Starvation = value;
      if ((double) starvation == (double) this.Starvation)
        return;
      FollowerBrainStats.StatChangedEvent starvationChanged = FollowerBrainStats.OnStarvationChanged;
      if (starvationChanged != null)
        starvationChanged(this._followerID, this.Starvation, starvation, this.Starvation - starvation);
      if ((double) starvation < 75.0 && (double) this.Starvation >= 75.0)
      {
        Debug.Log((object) "A ");
        if (this._info.CursedState == Thought.Zombie)
          return;
        Debug.Log((object) "B ");
        if (this._info.DiedOfStarvation)
          return;
        DataManager.Instance.LastFollowerToStarveToDeath = TimeManager.TotalElapsedGameTime;
        this._info.DiedOfStarvation = true;
      }
      else
      {
        if ((double) starvation <= 0.0 || (double) this.Starvation > 0.0)
          return;
        Debug.Log((object) "C ");
        this.IsStarving = false;
        FollowerBrainStats.StatStateChangedEvent starvationStateChanged = FollowerBrainStats.OnStarvationStateChanged;
        if (starvationStateChanged == null)
          return;
        starvationStateChanged(this._followerID, FollowerStatState.Off, FollowerStatState.On);
      }
    }
  }

  public float Bathroom
  {
    get => this._info.Bathroom;
    set
    {
      float bathroom = this.Bathroom;
      this._info.Bathroom = value;
      if ((double) bathroom == (double) this.Bathroom)
        return;
      FollowerBrainStats.StatChangedEvent onBathroomChanged = FollowerBrainStats.OnBathroomChanged;
      if (onBathroomChanged != null)
        onBathroomChanged(this._followerID, this.Bathroom, bathroom, this.Bathroom - bathroom);
      if ((double) bathroom < 30.0 && (double) this.Bathroom >= 30.0)
      {
        FollowerBrainStats.StatStateChangedEvent bathroomStateChanged = FollowerBrainStats.OnBathroomStateChanged;
        if (bathroomStateChanged == null)
          return;
        bathroomStateChanged(this._followerID, FollowerStatState.Urgent, FollowerStatState.On);
      }
      else if ((double) bathroom > 15.0 && (double) this.Bathroom <= 15.0)
      {
        FollowerBrainStats.StatStateChangedEvent bathroomStateChanged = FollowerBrainStats.OnBathroomStateChanged;
        if (bathroomStateChanged == null)
          return;
        bathroomStateChanged(this._followerID, FollowerStatState.Off, FollowerStatState.On);
      }
      else
      {
        if ((double) bathroom > 15.0 || (double) this.Bathroom <= 15.0)
          return;
        FollowerBrainStats.StatStateChangedEvent bathroomStateChanged = FollowerBrainStats.OnBathroomStateChanged;
        if (bathroomStateChanged == null)
          return;
        bathroomStateChanged(this._followerID, FollowerStatState.On, FollowerStatState.Off);
      }
    }
  }

  public float BathroomFillRate
  {
    get => this._info.BathroomFillRate;
    set => this._info.BathroomFillRate = value;
  }

  public float Social
  {
    get => this._info.Social;
    set => this._info.Social = value;
  }

  public float Vomit
  {
    get => this._info.Vomit;
    set => this._info.Vomit = value;
  }

  public float TargetBathroom
  {
    get => this._info.TargetBathroom;
    set => this._info.TargetBathroom = value;
  }

  public float Rest
  {
    get => this._info.Rest;
    set
    {
      if (this.followerBrain.ThoughtExists(Thought.WorkThroughNight) && (double) value < (double) this.Rest)
        return;
      float rest = this.Rest;
      this._info.Rest = value;
      if ((double) rest == (double) this.Rest)
        return;
      FollowerBrainStats.StatChangedEvent onRestChanged = FollowerBrainStats.OnRestChanged;
      if (onRestChanged != null)
        onRestChanged(this._followerID, this.Rest, rest, this.Rest - rest);
      if ((double) rest > 20.0 && (double) this.Rest <= 20.0)
      {
        FollowerBrainStats.StatStateChangedEvent restStateChanged = FollowerBrainStats.OnRestStateChanged;
        if (restStateChanged == null)
          return;
        restStateChanged(this._followerID, FollowerStatState.On, FollowerStatState.Off);
      }
      else if ((double) rest <= 20.0 && (double) this.Rest > 20.0)
      {
        FollowerBrainStats.StatStateChangedEvent restStateChanged = FollowerBrainStats.OnRestStateChanged;
        if (restStateChanged == null)
          return;
        restStateChanged(this._followerID, FollowerStatState.Off, FollowerStatState.On);
      }
      else if ((double) rest > 0.0 && (double) this.Rest <= 0.0)
      {
        FollowerBrainStats.StatStateChangedEvent restStateChanged = FollowerBrainStats.OnRestStateChanged;
        if (restStateChanged == null)
          return;
        restStateChanged(this._followerID, FollowerStatState.Urgent, FollowerStatState.On);
      }
      else
      {
        if ((double) rest > 0.0 || (double) this.Rest <= 0.0)
          return;
        FollowerBrainStats.StatStateChangedEvent restStateChanged = FollowerBrainStats.OnRestStateChanged;
        if (restStateChanged == null)
          return;
        restStateChanged(this._followerID, FollowerStatState.On, FollowerStatState.Urgent);
      }
    }
  }

  public List<StructureAndTime> ReactionsAndTime => this._info.ReactionsAndTime;

  public float LastVomit
  {
    get => this._info.LastVomit;
    set => this._info.LastVomit = value;
  }

  public int LastHeal
  {
    get => this._info.LastHeal;
    set => this._info.LastHeal = value;
  }

  public float Exhaustion
  {
    get => this._info.Exhaustion;
    set
    {
      float exhaustion = this.Exhaustion;
      this._info.Exhaustion = value;
      if ((double) exhaustion == (double) this.Exhaustion)
        return;
      FollowerBrainStats.StatChangedEvent exhaustionChanged = FollowerBrainStats.OnExhaustionChanged;
      if (exhaustionChanged != null)
        exhaustionChanged(this._followerID, this.Exhaustion, exhaustion, this.Exhaustion - exhaustion);
      if ((double) exhaustion > 0.0 && (double) this.Exhaustion <= 0.0)
      {
        FollowerBrainStats.StatStateChangedEvent exhaustionStateChanged = FollowerBrainStats.OnExhaustionStateChanged;
        if (exhaustionStateChanged != null)
          exhaustionStateChanged(this.followerBrain.Info.ID, FollowerStatState.Off, FollowerStatState.On);
      }
      if ((double) exhaustion > 0.0 || (double) this.Exhaustion <= 0.0)
        return;
      FollowerBrainStats.StatStateChangedEvent exhaustionStateChanged1 = FollowerBrainStats.OnExhaustionStateChanged;
      if (exhaustionStateChanged1 == null)
        return;
      exhaustionStateChanged1(this.followerBrain.Info.ID, FollowerStatState.On, FollowerStatState.Off);
    }
  }

  public float Illness
  {
    get => this._info.Illness;
    set
    {
      float illness = this.Illness;
      this._info.Illness = value;
      if ((double) illness == (double) this.Illness)
        return;
      FollowerBrainStats.StatChangedEvent onIllnessChanged = FollowerBrainStats.OnIllnessChanged;
      if (onIllnessChanged != null)
        onIllnessChanged(this._followerID, this.Illness, illness, this.Illness - illness);
      if ((double) illness >= 100.0 || (double) this.Illness < 100.0)
        return;
      this.followerBrain.DiedOfIllness = DataManager.Instance.OnboardingFinished;
    }
  }

  public float DissentGold
  {
    get => this._info.DissentGold;
    set => this._info.DissentGold = value;
  }

  public bool GivenDissentWarning
  {
    get => this._info.GivenDissentWarning;
    set => this._info.GivenDissentWarning = value;
  }

  public float Reeducation
  {
    get => this._info.Reeducation;
    set
    {
      if ((double) value != (double) this._info.Reeducation)
      {
        FollowerBrainStats.StatChangedEvent reeducationChanged = FollowerBrainStats.OnReeducationChanged;
        if (reeducationChanged != null)
          reeducationChanged(this._info.ID, value, this._info.Reeducation, value - this._info.Reeducation);
      }
      if ((double) this._info.Reeducation > 0.0 && (double) value <= 0.0)
      {
        this._info.Reeducation = value;
        System.Action reeducationComplete = this.OnReeducationComplete;
        if (reeducationComplete != null)
          reeducationComplete();
        if (this.followerBrain.Info.CursedState == Thought.Dissenter)
          CultFaithManager.AddThought(Thought.Cult_CureDissenter, this.followerBrain.Info.ID);
        this.followerBrain.RemoveCurseState(Thought.Dissenter);
        ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.CureDissenter, this.followerBrain.Info.ID);
        FollowerBrainStats.OnReeducationStateChanged(this._info.ID, FollowerStatState.Off, FollowerStatState.On);
      }
      this._info.Reeducation = value;
    }
  }

  public int InteractionCoolDownFasting
  {
    get => this._info.InteractionCoolDownFasting;
    set => this._info.InteractionCoolDownFasting = value;
  }

  public int InteractionCoolDownEnergizing
  {
    get => this._info.InteractionCoolDownEnergizing;
    set => this._info.InteractionCoolDownEnergizing = value;
  }

  public int InteractionCoolDownMotivational
  {
    get => this._info.InteractionCoolDownMotivational;
    set => this._info.InteractionCoolDownMotivational = value;
  }

  public int InteractionCoolDemandDevotion
  {
    get => this._info.InteractionCoolDemandDevotion;
    set => this._info.InteractionCoolDemandDevotion = value;
  }

  public static bool Fasting
  {
    get
    {
      return (double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastFastDeclared < 3600.0;
    }
  }

  public static bool BrainWashed
  {
    get
    {
      return (double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastBrainwashed < 3600.0;
    }
  }

  public static bool IsHoliday
  {
    get
    {
      return (double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastHolidayDeclared < 1200.0;
    }
  }

  public static bool IsWorkThroughTheNight
  {
    get
    {
      return (double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastWorkThroughTheNight < 3600.0;
    }
  }

  public static bool IsConstruction
  {
    get
    {
      return (double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastConstruction < 3600.0;
    }
  }

  public static bool IsEnlightened
  {
    get
    {
      return (double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastEnlightenment < 3600.0;
    }
  }

  public static bool IsFishing
  {
    get
    {
      return (double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastFishingDeclared < 3600.0;
    }
  }

  public static bool IsBloodMoon
  {
    get
    {
      return (double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastHalloween < 3600.0;
    }
  }

  public int GuaranteedGoodInteractionsUntil
  {
    get => this._info.GuaranteedGoodInteractionsUntil;
    set => this._info.GuaranteedGoodInteractionsUntil = value;
  }

  public bool GuaranteedGoodInteractions
  {
    get => this._info.GuaranteedGoodInteractionsUntil >= DataManager.Instance.CurrentDayIndex;
  }

  public int IncreasedDevotionOutputUntil
  {
    get => this._info.IncreasedDevotionOutputUntil;
    set => this._info.IncreasedDevotionOutputUntil = value;
  }

  public bool IncreasedDevotionOutput
  {
    get => this._info.IncreasedDevotionOutputUntil >= DataManager.Instance.CurrentDayIndex;
  }

  public void Brainwash(FollowerBrain Brain) => Brain.AddThought(Thought.Brainwashed);

  public int MotivatedUntil
  {
    get => this._info.MotivatedUntil;
    private set
    {
      int num1 = this.Motivated ? 1 : 0;
      this._info.MotivatedUntil = value;
      int num2 = this.Motivated ? 1 : 0;
      if (num1 == num2)
        return;
      FollowerBrainStats.StatusChangedEvent motivatedChanged = FollowerBrainStats.OnMotivatedChanged;
      if (motivatedChanged == null)
        return;
      motivatedChanged(this._followerID);
    }
  }

  public bool Motivated => this._info.MotivatedUntil >= DataManager.Instance.CurrentDayIndex;

  public void Motivate(int durationDays)
  {
    this.MotivatedUntil = Mathf.Max(this.MotivatedUntil, DataManager.Instance.CurrentDayIndex + (durationDays - 1));
  }

  public int LastBlessing
  {
    get => this._info.LastBlessing;
    set => this._info.LastBlessing = value;
  }

  public bool BlessedToday => this._info.LastBlessing == DataManager.Instance.CurrentDayIndex;

  public int CachedLumber
  {
    get => this._info.CachedLumber;
    set => this._info.CachedLumber = value;
  }

  public int CachedLumberjackStationID
  {
    get => this._info.CachedLumberjackStationID;
    set => this._info.CachedLumberjackStationID = value;
  }

  public int GetDevotionGeneration() => 0;

  public delegate void StatChangedEvent(
    int followerID,
    float newValue,
    float oldValue,
    float change);

  public delegate void StatStateChangedEvent(
    int followerId,
    FollowerStatState newState,
    FollowerStatState oldState);

  public delegate void StatusChangedEvent(int followerID);
}
