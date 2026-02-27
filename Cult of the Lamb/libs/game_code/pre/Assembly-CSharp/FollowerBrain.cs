// Decompiled with JetBrains decompiler
// Type: FollowerBrain
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class FollowerBrain
{
  public FollowerBrainInfo Info;
  public FollowerBrainStats Stats;
  public FollowerInfo _directInfoAccess;
  public FollowerLocation DesiredLocation;
  private bool _followingPlayer;
  private bool _inRitual;
  public NotificationCentre.NotificationType LeftCultWithReason;
  private float bathroomOffset = -1f;
  private string ReturnString;
  private float ModifierTotal;
  private static List<int> MultipleSpouseFaith = new List<int>()
  {
    30,
    15,
    10,
    5,
    3,
    -2,
    -5,
    -7,
    -10
  };
  public Action<float> OnNewThought;
  public System.Action OnAdoration;
  public static Dictionary<FollowerBrain.AdorationActions, int> AdorationsAndActions = new Dictionary<FollowerBrain.AdorationActions, int>()
  {
    {
      FollowerBrain.AdorationActions.Sermon,
      5
    },
    {
      FollowerBrain.AdorationActions.Bless,
      20
    },
    {
      FollowerBrain.AdorationActions.BigGift,
      30
    },
    {
      FollowerBrain.AdorationActions.Quest,
      50
    },
    {
      FollowerBrain.AdorationActions.Gift,
      20
    },
    {
      FollowerBrain.AdorationActions.Necklace,
      25
    },
    {
      FollowerBrain.AdorationActions.Inspire,
      25
    },
    {
      FollowerBrain.AdorationActions.Intimidate,
      15
    },
    {
      FollowerBrain.AdorationActions.Bribe,
      15
    },
    {
      FollowerBrain.AdorationActions.Ritual_AlmsToPoor,
      30
    },
    {
      FollowerBrain.AdorationActions.FaithEnforce,
      5
    },
    {
      FollowerBrain.AdorationActions.HappyFollowerNPC,
      60
    },
    {
      FollowerBrain.AdorationActions.ConfessionBooth,
      75
    },
    {
      FollowerBrain.AdorationActions.FirePit,
      30
    },
    {
      FollowerBrain.AdorationActions.FightPitMercy,
      20
    },
    {
      FollowerBrain.AdorationActions.FightPitDeath,
      50
    },
    {
      FollowerBrain.AdorationActions.AscendedFollower_Lvl2,
      20
    },
    {
      FollowerBrain.AdorationActions.AscendedFollower_Lvl3,
      30
    },
    {
      FollowerBrain.AdorationActions.AscendedFollower_Lvl4,
      40
    },
    {
      FollowerBrain.AdorationActions.AscendedFollower_Lvl5,
      50
    },
    {
      FollowerBrain.AdorationActions.SmoochSpouse,
      10
    },
    {
      FollowerBrain.AdorationActions.AssignEnforcer,
      30
    },
    {
      FollowerBrain.AdorationActions.PetDog,
      5
    },
    {
      FollowerBrain.AdorationActions.SermonLvl1,
      10
    },
    {
      FollowerBrain.AdorationActions.BribeLvl1,
      20
    },
    {
      FollowerBrain.AdorationActions.BlessLvl1,
      35
    },
    {
      FollowerBrain.AdorationActions.InspireLvl1,
      40
    },
    {
      FollowerBrain.AdorationActions.IntimidateLvl1,
      30
    },
    {
      FollowerBrain.AdorationActions.BigGiftLvl1,
      35
    },
    {
      FollowerBrain.AdorationActions.GiftLvl1,
      25
    },
    {
      FollowerBrain.AdorationActions.NecklaceLvl1,
      30
    }
  };
  public System.Action OnGetXP;
  public int SpeakersInRange;
  private Dictionary<int, int> ReactionDictionary = new Dictionary<int, int>();
  public Action<FollowerState, FollowerState> OnStateChanged;
  private FollowerState _currentState;
  public FollowerBrain.PendingTaskData PendingTask;
  public Action<FollowerTask, FollowerTask> OnTaskChanged;
  private FollowerTask _nextTask;
  private FollowerTask _currentTask;
  public System.Action OnBecomeDissenter;
  public static FollowerBrain.DwellingAssignmentChanged OnDwellingAssigned;
  public static FollowerBrain.DwellingAssignmentChanged OnDwellingCleared;
  public static FollowerBrain.DwellingAssignmentChanged OnDwellingAssignedAwaitClaim;
  public static Action<int> OnBrainAdded;
  public static Action<int> OnBrainRemoved;
  private static Dictionary<int, FollowerBrain> _brainsByID = new Dictionary<int, FollowerBrain>();
  public static List<FollowerBrain> AllBrains = new List<FollowerBrain>();

  public List<TaskAndTime> _taskMemory
  {
    get => this._directInfoAccess.TaskMemory;
    set => this._directInfoAccess.TaskMemory = value;
  }

  public FollowerLocation HomeLocation => this._directInfoAccess.HomeLocation;

  public FollowerLocation Location
  {
    get => this._directInfoAccess.Location;
    set => this._directInfoAccess.Location = value;
  }

  public Vector3 LastPosition
  {
    get => this._directInfoAccess.LastPosition;
    set => this._directInfoAccess.LastPosition = value;
  }

  public FollowerTaskType SavedFollowerTaskType
  {
    get => this._directInfoAccess.SavedFollowerTaskType;
    set => this._directInfoAccess.SavedFollowerTaskType = value;
  }

  public FollowerLocation SavedFollowerTaskLocation
  {
    get => this._directInfoAccess.SavedFollowerTaskLocation;
    set => this._directInfoAccess.SavedFollowerTaskLocation = value;
  }

  public Vector3 SavedFollowerTaskDestination
  {
    get => this._directInfoAccess.SavedFollowerTaskDestination;
    set => this._directInfoAccess.SavedFollowerTaskDestination = value;
  }

  public bool FollowingPlayer
  {
    get => this._followingPlayer;
    set
    {
      if (this._followingPlayer == value)
        return;
      this._followingPlayer = value;
      this.CheckChangeState();
    }
  }

  public bool InRitual
  {
    get => this._inRitual;
    set
    {
      if (this._inRitual == value)
        return;
      this._inRitual = value;
      this.CheckChangeState();
    }
  }

  public bool DiedInPrison
  {
    get => this._directInfoAccess.DiedInPrison;
    set => this._directInfoAccess.DiedInPrison = value;
  }

  public bool DiedOfIllness
  {
    get => this._directInfoAccess.DiedOfIllness;
    set => this._directInfoAccess.DiedOfIllness = value;
  }

  public bool DiedOfOldAge
  {
    get => this._directInfoAccess.DiedOfOldAge;
    set => this._directInfoAccess.DiedOfOldAge = value;
  }

  public bool DiedOfStarvation
  {
    get => this._directInfoAccess.DiedOfStarvation;
    set => this._directInfoAccess.DiedOfStarvation = value;
  }

  public bool DiedFromTwitchChat
  {
    get => this._directInfoAccess.DiedFromTwitchChat;
    set => this._directInfoAccess.DiedFromTwitchChat = value;
  }

  public bool DiedFromDeadlyDish
  {
    get => this._directInfoAccess.DiedFromDeadlyDish;
    set => this._directInfoAccess.DiedFromDeadlyDish = value;
  }

  private bool _leavingCult
  {
    get => this._directInfoAccess.LeavingCult;
    set => this._directInfoAccess.LeavingCult = value;
  }

  public bool LeavingCult
  {
    get => this._leavingCult;
    set
    {
      if (this._leavingCult == value)
        return;
      this._leavingCult = value;
      this.CheckChangeTask();
    }
  }

  public bool LeftCult { get; set; }

  private FollowerBrain(FollowerInfo info)
  {
    if (info.HomeLocation == FollowerLocation.Church)
      info.HomeLocation = FollowerLocation.Base;
    this.Info = new FollowerBrainInfo(info, this);
    this.Stats = new FollowerBrainStats(info, this);
    this._directInfoAccess = info;
    this.DesiredLocation = this.Location;
    StructureManager.OnStructureAdded += new StructureManager.StructureChanged(this.OnStructureAdded);
    StructureManager.OnStructureMoved += new StructureManager.StructureChanged(this.OnStructureMoved);
    StructureManager.OnStructureUpgraded += new StructureManager.StructureChanged(this.OnStructureUpgraded);
    StructureManager.OnStructureRemoved += new StructureManager.StructureChanged(this.OnStructureRemoved);
    FollowerBrainStats.OnExhaustionStateChanged += new FollowerBrainStats.StatStateChangedEvent(this.OnExhaustionStateChanged);
    FollowerBrainStats.OnIllnessStateChanged += new FollowerBrainStats.StatStateChangedEvent(this.OnIllnessStateChanged);
    FollowerBrainStats.OnHappinessStateChanged += new FollowerBrainStats.StatStateChangedEvent(this.OnHappinessStateChanged);
    FollowerBrainStats.OnSatiationStateChanged += new FollowerBrainStats.StatStateChangedEvent(this.OnSatiationStateChanged);
    FollowerBrainStats.OnStarvationStateChanged += new FollowerBrainStats.StatStateChangedEvent(this.OnStarvationStateChanged);
    FollowerBrainStats.OnRestStateChanged += new FollowerBrainStats.StatStateChangedEvent(this.OnRestStateChanged);
    FollowerBrainStats.OnMotivatedChanged += new FollowerBrainStats.StatusChangedEvent(this.OnMotivatedChanged);
    FollowerBrainStats.OnStarvationStateChanged += new FollowerBrainStats.StatStateChangedEvent(this.OnStarvationChanged);
    StructuresData.OnResearchBegin += new System.Action(this.OnResearchBegin);
    TimeManager.OnNewPhaseStarted += new System.Action(this.OnNewPhaseStarted);
    TimeManager.OnScheduleChanged += new System.Action(this.OnScheduleChanged);
    TimeManager.OnNewDayStarted += new System.Action(this.OnNewDay);
    if (this._directInfoAccess.TraitsSet)
      return;
    this.AddTrait(FollowerTrait.GetStartingTrait(), false);
    this.AddTrait(FollowerTrait.GetStartingTrait(), false);
    if ((double) UnityEngine.Random.value <= 0.20000000298023224)
      this.AddTrait(FollowerTrait.GetRareTrait(), false);
    else if ((double) UnityEngine.Random.value <= 0.40000000596046448)
      this.AddTrait(FollowerTrait.GetStartingTrait(), false);
    this._directInfoAccess.TraitsSet = true;
  }

  public void Cleanup()
  {
    StructureManager.OnStructureAdded -= new StructureManager.StructureChanged(this.OnStructureAdded);
    StructureManager.OnStructureMoved -= new StructureManager.StructureChanged(this.OnStructureMoved);
    StructureManager.OnStructureUpgraded -= new StructureManager.StructureChanged(this.OnStructureUpgraded);
    StructureManager.OnStructureRemoved -= new StructureManager.StructureChanged(this.OnStructureRemoved);
    FollowerBrainStats.OnIllnessStateChanged -= new FollowerBrainStats.StatStateChangedEvent(this.OnIllnessStateChanged);
    FollowerBrainStats.OnHappinessStateChanged -= new FollowerBrainStats.StatStateChangedEvent(this.OnHappinessStateChanged);
    FollowerBrainStats.OnSatiationStateChanged -= new FollowerBrainStats.StatStateChangedEvent(this.OnSatiationStateChanged);
    FollowerBrainStats.OnRestStateChanged -= new FollowerBrainStats.StatStateChangedEvent(this.OnRestStateChanged);
    FollowerBrainStats.OnMotivatedChanged -= new FollowerBrainStats.StatusChangedEvent(this.OnMotivatedChanged);
    FollowerBrainStats.OnStarvationStateChanged -= new FollowerBrainStats.StatStateChangedEvent(this.OnStarvationChanged);
    FollowerBrainStats.OnStarvationStateChanged -= new FollowerBrainStats.StatStateChangedEvent(this.OnStarvationStateChanged);
    FollowerBrainStats.OnExhaustionStateChanged -= new FollowerBrainStats.StatStateChangedEvent(this.OnExhaustionStateChanged);
    StructuresData.OnResearchBegin -= new System.Action(this.OnResearchBegin);
    TimeManager.OnNewPhaseStarted -= new System.Action(this.OnNewPhaseStarted);
    TimeManager.OnScheduleChanged -= new System.Action(this.OnScheduleChanged);
    TimeManager.OnNewDayStarted -= new System.Action(this.OnNewDay);
    if (this.CurrentTask == null || this.CurrentTask.State == FollowerTaskState.Done)
      return;
    this.CurrentTask.Abort();
  }

  public void ResetStats()
  {
    this._directInfoAccess.ResetStats();
    this.Info.CursedState = Thought.None;
    this.RemoveThought(Thought.Dissenter, true);
    this.RemoveThought(Thought.OldAge, true);
    this.RemoveThought(Thought.BecomeStarving, true);
    this.RemoveThought(Thought.Ill, true);
    this.Stats.Reeducation = 0.0f;
    this.Stats.Illness = 0.0f;
    this.Stats.Starvation = 0.0f;
    this.Stats.Exhaustion = 0.0f;
    this.DiedInPrison = false;
    this.DiedOfIllness = false;
    this.DiedOfOldAge = false;
    this.DiedOfStarvation = false;
    this.DiedFromTwitchChat = false;
    this.DiedFromDeadlyDish = false;
    this._directInfoAccess.TaxEnforcer = false;
    this._directInfoAccess.FaithEnforcer = false;
    this.ClearThought(Thought.OldAge);
    this.ClearThought(Thought.Dissenter);
    this.ClearThought(Thought.Ill);
    this.ClearThought(Thought.BecomeStarving);
  }

  public FollowerOutfit CreateOutfit() => new FollowerOutfit(this._directInfoAccess);

  private void OnNewDay()
  {
    this.Stats.PaidTithes = false;
    this.Stats.ReceivedBlessing = false;
    this.Stats.Bribed = false;
    this._directInfoAccess.TaxedToday = false;
    this._directInfoAccess.FaithedToday = false;
    this.Stats.Inspired = false;
    this.Stats.PetDog = false;
    this.Stats.Intimidated = false;
    this.Stats.ReeducatedAction = false;
    this.Stats.KissedAction = false;
    if (this.CurrentTask == null || !(this.CurrentTask is FollowerTask_OnMissionary) && !(this.CurrentTask is FollowerTask_MissionaryComplete))
      this._directInfoAccess.MissionaryExhaustion = Mathf.Clamp(this._directInfoAccess.MissionaryExhaustion - 0.5f, 1f, (float) int.MaxValue);
    if (this.CurrentTask == null)
    {
      if (!this.HasHome)
      {
        if (this.HasTrait(FollowerTrait.TraitType.Materialistic))
          this.AddThought(Thought.SleptOutisdeMaterialisticTrait);
        else
          this.AddThought(Thought.SleptOutisde);
      }
      else
      {
        StructureBrain.TYPES structureTypeById = StructureManager.GetStructureTypeByID(this.HomeID);
        if (this.HasTrait(FollowerTrait.TraitType.Materialistic))
        {
          switch (structureTypeById)
          {
            case StructureBrain.TYPES.BED:
              this.AddThought(Thought.SleptHouse1MaterialisticTrait);
              break;
            case StructureBrain.TYPES.BED_2:
              this.AddThought(Thought.SleptHouse2MaterialisticTrait);
              break;
            case StructureBrain.TYPES.BED_3:
              this.AddThought(Thought.SleptHouse3MaterialisticTrait);
              break;
          }
        }
        else
        {
          switch (structureTypeById)
          {
            case StructureBrain.TYPES.BED:
              this.AddThought(Thought.SleptHouse1);
              break;
            case StructureBrain.TYPES.BED_2:
              this.AddThought(Thought.SleptHouse2);
              break;
            case StructureBrain.TYPES.BED_3:
              this.AddThought(Thought.SleptHouse3);
              break;
          }
        }
      }
    }
    if (this.Info.Age++ >= this.Info.LifeExpectancy && this.Info.CursedState != Thought.OldAge && this.Info.CursedState != Thought.Zombie && !this.HasTrait(FollowerTrait.TraitType.Immortal) && DataManager.Instance.OnboardedOldFollower)
    {
      if ((double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastFollowerToReachOldAge < 600.0 / (double) DifficultyManager.GetTimeBetweenOldAgeMultiplier())
        return;
      this.MakeOld();
    }
    if (this._directInfoAccess.WakeUpDay != TimeManager.CurrentDay - 1 || FollowerManager.FollowerLocked(this.Info.ID))
      return;
    this.MakeExhausted();
  }

  public void Tick(float deltaGameTime)
  {
    if (this._currentState == null || this._currentState.Type == FollowerStateType.Default)
      this.CheckChangeState();
    if (this.CurrentTask == null)
      this.CheckChangeTask();
    this.UpdateThoughts();
    if ((double) this.Stats.Bathroom < (double) this.Stats.TargetBathroom && !TimeManager.IsNight)
    {
      float num = 0.166666672f;
      this.Stats.Bathroom += deltaGameTime * num * this.Stats.BathroomFillRate;
    }
    if ((double) this.Stats.Reeducation != 0.0 && !this.LeavingCult && this.CurrentTaskType != FollowerTaskType.Imprisoned && this.Info.CursedState == Thought.Dissenter)
    {
      if ((double) this.Stats.Reeducation > 0.0 && !this.Stats.GivenDissentWarning)
      {
        float itemQuantity = (float) Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD);
        this.Stats.DissentGold = Mathf.Floor(UnityEngine.Random.Range(itemQuantity * 0.15f, itemQuantity * 0.4f));
        NotificationCentre.Instance.PlayGenericNotificationNonLocalizedParams("Notifications/DissenterLeavingTomorrow", this.Info.Name, this.Stats.DissentGold.ToString());
        this.Stats.GivenDissentWarning = true;
      }
      if ((double) this.Stats.Reeducation >= 100.0 && PlayerFarming.Location == FollowerLocation.Base && DataManager.Instance.OnboardingFinished)
        this.LeavingCult = true;
    }
    if (this.CurrentTask == null)
      return;
    this.CurrentTask.Tick(deltaGameTime);
  }

  public string GetThoughtString(float SubTextSize)
  {
    this.ReturnString = "";
    foreach (ThoughtData thought in this.Stats.Thoughts)
    {
      this.ModifierTotal = 0.0f;
      int num = -1;
      while (++num < thought.Quantity)
        this.ModifierTotal += num <= 0 ? thought.Modifier : (float) thought.StackModifier;
      this.ReturnString = $"{this.ReturnString}{((double) this.ModifierTotal >= 0.0 ? (object) "<color=green><b>+" : (object) "<color=red><b>")}{(object) this.ModifierTotal}</b></color> {FollowerThoughts.GetLocalisedName(thought.ThoughtType)}{(thought.Quantity > 1 ? (object) $"{$" <size={SubTextSize}>(x"}{(object) thought.Quantity})" : (object) "")}</size>\n{$"<size={SubTextSize}><i>"}{FollowerThoughts.GetLocalisedDescription(thought.ThoughtType)}</i></size>\n\n";
    }
    return this.ReturnString;
  }

  public static void AddMarriageThoughts()
  {
    List<FollowerBrain> followerBrainList = new List<FollowerBrain>();
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.Info.MarriedToLeader)
        followerBrainList.Add(allBrain);
    }
    Debug.Log((object) ("MarriedBrains.Count: " + (object) followerBrainList.Count));
    switch (followerBrainList.Count)
    {
      case 0:
        break;
      case 1:
        followerBrainList[0].AddThought(Thought.MarriedToLeader);
        break;
      default:
        Debug.Log((object) "Add married thoughts!");
        int index1 = -1;
        while (++index1 < followerBrainList.Count)
        {
          int index2 = -1;
          while (++index2 < followerBrainList.Count)
          {
            if (index1 != index2)
            {
              IDAndRelationship relationship = followerBrainList[index1].Info.GetOrCreateRelationship(followerBrainList[index2].Info.ID);
              if (relationship.Relationship > -11)
              {
                relationship.Relationship = -11;
                relationship.CurrentRelationshipState = IDAndRelationship.RelationshipState.Enemies;
              }
            }
          }
          ThoughtData data = FollowerThoughts.GetData(Thought.MultiMarriedToLeader);
          data.Init();
          data.Modifier = (float) FollowerBrain.MultipleSpouseFaith[Mathf.Clamp(followerBrainList.Count, 0, FollowerBrain.MultipleSpouseFaith.Count - 1)];
          followerBrainList[index1].AddThought(data);
        }
        break;
    }
  }

  public void AddThought(ThoughtData thought, bool forced = false)
  {
    if (this.CurrentTask != null && this.CurrentTask.BlockThoughts && !forced)
      return;
    float happiness = this.Stats.Happiness;
    if (this.OnNewThought != null)
      this.OnNewThought(thought.Modifier);
    int index = -1;
    while (++index < this.Stats.Thoughts.Count)
    {
      ThoughtData thought1 = this.Stats.Thoughts[index];
      if (thought1.ThoughtGroup == thought.ThoughtGroup && thought1.Stacking <= 0)
      {
        Debug.Log((object) $"REPLACE! {(object) thought.ThoughtGroup}  {(object) thought.ThoughtType}");
        this.Stats.Thoughts[index] = thought;
        this.Stats.OnThoughtsChanged(this, happiness, this.Stats.Happiness);
        return;
      }
      if (thought1.ThoughtType == thought.ThoughtType && thought1.Stacking > 0)
      {
        if (thought1.Quantity < thought1.Stacking)
        {
          ++thought1.Quantity;
        }
        else
        {
          thought1.CoolDowns.RemoveAt(0);
          thought1.TimeStarted.RemoveAt(0);
        }
        thought1.CoolDowns.Add(thought.Duration);
        thought1.TimeStarted.Add(TimeManager.TotalElapsedGameTime);
        this.Stats.OnThoughtsChanged(this, happiness, this.Stats.Happiness);
        return;
      }
    }
    this.Stats.Thoughts.Add(thought);
    this.Stats.OnThoughtsChanged(this, happiness, this.Stats.Happiness);
  }

  public void AddThought(Thought thought, bool InsetAtZero = false, bool forced = false)
  {
    if (this.CurrentTask != null && this.CurrentTask.BlockThoughts && !forced)
      return;
    float happiness = this.Stats.Happiness;
    ThoughtData data = FollowerThoughts.GetData(thought);
    data.Init();
    if (data != null && this.OnNewThought != null)
      this.OnNewThought(data.Modifier);
    int index = -1;
    while (++index < this.Stats.Thoughts.Count)
    {
      ThoughtData thought1 = this.Stats.Thoughts[index];
      if (thought1.ThoughtGroup == data.ThoughtGroup && thought1.Stacking <= 0)
      {
        this.Stats.Thoughts[index] = data;
        this.Stats.OnThoughtsChanged(this, happiness, this.Stats.Happiness);
        return;
      }
      if (thought1.ThoughtType == thought && thought1.Stacking > 0)
      {
        if (thought1.Quantity < thought1.Stacking)
        {
          ++thought1.Quantity;
        }
        else
        {
          thought1.CoolDowns.RemoveAt(0);
          thought1.TimeStarted.RemoveAt(0);
        }
        thought1.CoolDowns.Add(data.Duration);
        thought1.TimeStarted.Add(TimeManager.TotalElapsedGameTime);
        this.Stats.OnThoughtsChanged(this, happiness, this.Stats.Happiness);
        return;
      }
    }
    if (InsetAtZero)
      this.Stats.Thoughts.Insert(0, data);
    else
      this.Stats.Thoughts.Add(data);
    this.Stats.OnThoughtsChanged(this, happiness, this.Stats.Happiness);
  }

  public bool HasThought(Thought thought)
  {
    foreach (ThoughtData thought1 in this.Stats.Thoughts)
    {
      if (thought1.ThoughtType == thought)
        return true;
    }
    return false;
  }

  private void UpdateThoughts()
  {
    float happiness = this.Stats.Happiness;
    foreach (ThoughtData thought in this.Stats.Thoughts)
    {
      if ((double) thought.Duration != -1.0 && (double) TimeManager.TotalElapsedGameTime - (double) thought.TimeStarted[0] > (double) thought.CoolDowns[0])
      {
        if (thought.Quantity > 1)
        {
          --thought.Quantity;
          thought.CoolDowns.RemoveAt(0);
          thought.TimeStarted.RemoveAt(0);
        }
        else
          this.Stats.Thoughts.Remove(thought);
        this.Stats.OnThoughtsChanged(this, happiness, this.Stats.Happiness);
        switch (thought.ThoughtType)
        {
          case Thought.Brainwashed:
            float num = UnityEngine.Random.value;
            if ((double) num <= 0.20000000298023224)
              this.AddThought(Thought.BrainwashedHangOverLight);
            else if ((double) num <= 0.20000000298023224)
              this.AddThought(Thought.BrainwashedHangOverMild);
            else
              this.AddThought(Thought.BrainwashedHangOverPounding);
            if ((double) UnityEngine.Random.value <= (DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.MushroomBanned) ? 0.5 : 0.699999988079071))
              return;
            DataManager.Instance.LastFollowerToBecomeIll = TimeManager.TotalElapsedGameTime;
            this.ApplyCurseState(Thought.Ill);
            return;
          case Thought.Ill:
            Debug.Log((object) "REMOVE ILL THOUGHT AND DIE");
            this.CheckChangeTask();
            return;
          case Thought.FeelingUnwell:
            if ((double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastFollowerToBecomeIll < 600.0 / (double) DifficultyManager.GetTimeBetweenIllnessMultiplier())
              return;
            DataManager.Instance.LastFollowerToBecomeIll = TimeManager.TotalElapsedGameTime;
            this.ApplyCurseState(Thought.Ill);
            return;
          case Thought.OldAge:
            if (!this.HasTrait(FollowerTrait.TraitType.Immortal))
              this.DiedOfOldAge = DataManager.Instance.OnboardingFinished;
            this.CheckChangeTask();
            return;
          default:
            return;
        }
      }
    }
  }

  public void RemoveThought(Thought Thought, bool RemoveAllStack)
  {
    foreach (ThoughtData thought in this.Stats.Thoughts)
    {
      if (thought.ThoughtType == Thought)
      {
        thought.Duration = 0.0f;
        thought.CoolDowns[0] = 0.0f;
        if (RemoveAllStack)
          thought.Quantity = 1;
      }
    }
  }

  public void ClearThought(Thought thought)
  {
    for (int index = this.Stats.Thoughts.Count - 1; index >= 0; --index)
    {
      if (this.Stats.Thoughts[index].ThoughtType == thought)
        this.Stats.Thoughts.RemoveAt(index);
    }
  }

  public ThoughtData GetThought(Thought Thought)
  {
    foreach (ThoughtData thought in this.Stats.Thoughts)
    {
      if (thought.ThoughtType == Thought)
        return thought;
    }
    return (ThoughtData) null;
  }

  public bool HasTrait(FollowerTrait.TraitType TraitType)
  {
    return this._directInfoAccess.Traits.Contains(TraitType) || DataManager.Instance.CultTraits.Contains(TraitType);
  }

  public void AddTrait(FollowerTrait.TraitType TraitType, bool ShowNotification = true)
  {
    if (this._directInfoAccess.Traits.Contains(TraitType))
      return;
    FollowerTrait.RemoveExclusiveTraits(this, TraitType);
    foreach (int cultTrait in DataManager.Instance.CultTraits)
      FollowerTrait.RemoveExclusiveTraits(this, TraitType);
    switch (TraitType)
    {
      case FollowerTrait.TraitType.DesensitisedToDeath:
        this.RemoveThought(Thought.EnemyDied, true);
        this.RemoveThought(Thought.FriendDied, true);
        this.RemoveThought(Thought.LoverDied, true);
        this.RemoveThought(Thought.StrangerDied, true);
        this.RemoveThought(Thought.GrievedAtUnburiedBody, true);
        this.RemoveThought(Thought.GrievedAtRottenUnburiedBody, true);
        break;
      case FollowerTrait.TraitType.FearOfDeath:
        this.RemoveThought(Thought.EnemyDied, true);
        this.RemoveThought(Thought.FriendDied, true);
        this.RemoveThought(Thought.LoverDied, true);
        this.RemoveThought(Thought.StrangerDied, true);
        this.RemoveThought(Thought.GrievedAtUnburiedBody, true);
        this.RemoveThought(Thought.GrievedAtRottenUnburiedBody, true);
        break;
      case FollowerTrait.TraitType.Cannibal:
        this.RemoveThought(Thought.AteFollowerMeal, true);
        this.RemoveThought(Thought.AteRottenFollowerMeal, true);
        break;
      case FollowerTrait.TraitType.Disciplinarian:
        this.RemoveThought(Thought.InnocentImprisoned, true);
        this.RemoveThought(Thought.InnocentImprisonedSleeping, true);
        break;
      case FollowerTrait.TraitType.Libertarian:
        this.RemoveThought(Thought.InnocentImprisoned, true);
        this.RemoveThought(Thought.InnocentImprisonedSleeping, true);
        break;
      case FollowerTrait.TraitType.SacrificeEnthusiast:
        this.RemoveThought(Thought.CultMemberWasSacrificed, true);
        this.RemoveThought(Thought.CultMemberWasSacrificedAgainstSacrificeTrait, true);
        break;
      case FollowerTrait.TraitType.AgainstSacrifice:
        this.RemoveThought(Thought.CultMemberWasSacrificed, true);
        this.RemoveThought(Thought.CultMemberWasSacrificedSacrificeEnthusiastTrait, true);
        break;
    }
    this._directInfoAccess.Traits.Add(TraitType);
  }

  public void RemoveTrait(FollowerTrait.TraitType TraitType, bool ShowNotification)
  {
    this._directInfoAccess.Traits.Remove(TraitType);
  }

  public bool ThoughtExists(Thought thought)
  {
    foreach (ThoughtData thought1 in this.Stats.Thoughts)
    {
      if (thought1.ThoughtType == thought)
        return true;
    }
    return false;
  }

  public bool ThoughtGroupExists(Thought thought)
  {
    foreach (ThoughtData thought1 in this.Stats.Thoughts)
    {
      if (thought1.ThoughtGroup == thought)
        return true;
    }
    return false;
  }

  public void Die(
    NotificationCentre.NotificationType deathNotificationType)
  {
    Debug.Log((object) "Brain Die");
    this.Cleanup();
    this.ClearDwelling();
    FollowerManager.FollowerDie(this.Info.ID, deathNotificationType);
  }

  public void Leave(
    NotificationCentre.NotificationType leaveNotificationType)
  {
    FollowerManager.FollowerLeave(this.Info.ID, leaveNotificationType);
    Debug.Log((object) "Follower Brain Leave()");
    NotificationCentre.Instance.PlayGenericNotificationNonLocalizedParams("Notifications/DissenterLeftCult", this.Info.Name, this.Stats.DissentGold.ToString());
    this.Cleanup();
    this.ClearDwelling();
    FollowerManager.RemoveFollower(this.Info.ID);
    FollowerManager.RemoveFollowerBrain(this.Info.ID);
  }

  public void AddAdoration(FollowerBrain.AdorationActions Action, System.Action Callback)
  {
    Follower followerById = FollowerManager.FindFollowerByID(this.Info.ID);
    if (this.Info.XPLevel <= 1)
    {
      switch (Action)
      {
        case FollowerBrain.AdorationActions.Sermon:
          Action = FollowerBrain.AdorationActions.SermonLvl1;
          break;
        case FollowerBrain.AdorationActions.Bless:
          Action = FollowerBrain.AdorationActions.BlessLvl1;
          break;
      }
    }
    this.AddAdoration(followerById, Action, Callback);
  }

  public void AddAdoration(
    Follower follower,
    FollowerBrain.AdorationActions Action,
    System.Action Callback)
  {
    if (this.Stats.HasLevelledUp || !DataManager.Instance.ShowLoyaltyBars)
    {
      if (Callback == null)
        return;
      Callback();
    }
    else
    {
      int adorationsAndAction = FollowerBrain.AdorationsAndActions[Action];
      if (this.HasTrait(FollowerTrait.TraitType.Cynical))
        adorationsAndAction = Mathf.RoundToInt((float) adorationsAndAction * 0.85f);
      else if (this.HasTrait(FollowerTrait.TraitType.Gullible))
        adorationsAndAction = Mathf.RoundToInt((float) adorationsAndAction * 1.15f);
      float adoration = this.Stats.Adoration;
      this.Stats.Adoration += (float) adorationsAndAction;
      if ((bool) (UnityEngine.Object) follower)
      {
        follower.AdorationUI.BarController.SetBarSize(adoration / this.Stats.MAX_ADORATION, false, true);
        follower.StartCoroutine((IEnumerator) this.AddAdorationIE(follower, Action, Callback));
      }
      else
      {
        if (Callback == null)
          return;
        Callback();
      }
    }
  }

  private IEnumerator AddAdorationIE(
    Follower follower,
    FollowerBrain.AdorationActions Action,
    System.Action Callback)
  {
    yield return (object) follower.StartCoroutine((IEnumerator) follower.AdorationUI.IncreaseAdorationIE());
    if ((double) this.Stats.Adoration >= (double) this.Stats.MAX_ADORATION)
      yield return (object) follower.StartCoroutine((IEnumerator) follower.UpgradeToDiscipleRoutine());
    System.Action action = Callback;
    if (action != null)
      action();
  }

  public bool GetWillLevelUp(FollowerBrain.AdorationActions Action)
  {
    int adorationsAndAction = FollowerBrain.AdorationsAndActions[Action];
    if (this.HasTrait(FollowerTrait.TraitType.Cynical))
      adorationsAndAction = Mathf.RoundToInt((float) adorationsAndAction * 0.85f);
    else if (this.HasTrait(FollowerTrait.TraitType.Gullible))
      adorationsAndAction = Mathf.RoundToInt((float) adorationsAndAction * 1.15f);
    return (double) this.Stats.Adoration + (double) adorationsAndAction >= (double) this.Stats.MAX_ADORATION;
  }

  public void GetXP(float Delta)
  {
  }

  public void LevelUp()
  {
    System.Action onPromotion = this.Info.OnPromotion;
    if (onPromotion != null)
      onPromotion();
    ++this.Info.XPLevel;
    NotificationCentre.Instance.PlayFollowerNotification(NotificationCentre.NotificationType.LevelUp, this.Info, NotificationFollower.Animation.Happy);
    NotificationCentreScreen.Play(NotificationCentre.NotificationType.LevelUpCentreScreen);
  }

  public float DevotionToGive
  {
    get
    {
      return Mathf.Max(1f, 1f * (float) ((double) ((float) (1.0 + (this.HasTrait(FollowerTrait.TraitType.Faithful) ? 0.15000000596046448 : 0.0) - (this.HasTrait(FollowerTrait.TraitType.Faithless) ? 0.15000000596046448 : 0.0) + (this.HasTrait(FollowerTrait.TraitType.MushroomBanned) ? 0.05000000074505806 : 0.0) + (this.ThoughtExists(Thought.Intimidated) ? 0.10000000149011612 : 0.0) - (this.HasTrait(FollowerTrait.TraitType.Lazy) ? 0.10000000149011612 : 0.0)) + CultFaithManager.CurrentFaith / 500f) + (FollowerBrainStats.IsEnlightened ? 0.25 : 0.0) + (this.Info.Necklace == InventoryItem.ITEM_TYPE.Necklace_1 ? 0.15000000596046448 : 0.0)));
    }
  }

  public float ResourceHarvestingMultiplier
  {
    get
    {
      return Mathf.Max(1f, 1f * (float) (1.0 + (this.Info.Necklace == InventoryItem.ITEM_TYPE.Necklace_4 ? 0.25 : 0.0)));
    }
  }

  public bool CheckForInteraction(FollowerBrain otherBrain)
  {
    if ((double) this.Stats.Social <= 0.0 && (this.CurrentTask == null || !this.CurrentTask.BlockSocial) && this.Info.CursedState == Thought.None && otherBrain.Info.CursedState == Thought.None && (double) otherBrain.Stats.Social <= 0.0 && (otherBrain.CurrentTask == null || !otherBrain.CurrentTask.BlockReactTasks && !otherBrain.CurrentTask.BlockTaskChanges && !otherBrain.CurrentTask.BlockSocial) && otherBrain.CurrentTask != null)
    {
      Debug.Log((object) "=========================================");
      bool blockSocial = otherBrain.CurrentTask.BlockSocial;
      Debug.Log((object) ("otherBrain.CurrentTask.BlockSocial: " + blockSocial.ToString()));
      if (this.CurrentTask != null)
      {
        blockSocial = this.CurrentTask.BlockSocial;
        Debug.Log((object) ("CurrentTask.BlockSocial: " + blockSocial.ToString()));
      }
      Debug.Log((object) $"{(object) this.CurrentTaskType}  {(object) otherBrain.CurrentTaskType}");
      this.HardSwapToTask((FollowerTask) new FollowerTask_Chat(otherBrain.Info.ID, true));
    }
    if (otherBrain.CurrentTask is FollowerTask_Dissent && otherBrain.CurrentTask.State == FollowerTaskState.Doing && (double) TimeManager.TotalElapsedGameTime - (double) StructureAndTime.GetOrAddTime(otherBrain.Info.ID, this, StructureAndTime.IDTypes.Follower) > 360.0 && this.CurrentTaskType != FollowerTaskType.DissentListen && !this.HasTrait(FollowerTrait.TraitType.Zealous))
    {
      StructureAndTime.SetTime(otherBrain.Info.ID, this, StructureAndTime.IDTypes.Follower);
      this.HardSwapToTask((FollowerTask) new FollowerTask_DissentListen(otherBrain.Info.ID));
      return true;
    }
    if (TimeManager.CurrentPhase != DayPhase.Night && this.Info.FaithEnforcer && (double) TimeManager.TotalElapsedGameTime - (double) StructureAndTime.GetOrAddTime(otherBrain.Info.ID, this, StructureAndTime.IDTypes.Follower) > 360.0 && !otherBrain._directInfoAccess.FaithedToday && (double) Vector3.Distance(this.LastPosition, otherBrain.LastPosition) < 5.0 && (this.CurrentTask == null || !(this.CurrentTask is FollowerTask_FaithEnforce)))
    {
      StructureAndTime.SetTime(otherBrain.Info.ID, this, StructureAndTime.IDTypes.Follower);
      this.HardSwapToTask((FollowerTask) new FollowerTask_FaithEnforce(otherBrain));
      return true;
    }
    if (TimeManager.CurrentPhase == DayPhase.Night || !this.Info.TaxEnforcer || (double) TimeManager.TotalElapsedGameTime - (double) StructureAndTime.GetOrAddTime(otherBrain.Info.ID, this, StructureAndTime.IDTypes.Follower) <= 360.0 || otherBrain._directInfoAccess.TaxedToday || (double) Vector3.Distance(this.LastPosition, otherBrain.LastPosition) >= 5.0 || this.CurrentTask != null && this.CurrentTask is FollowerTask_TaxEnforce)
      return false;
    StructureAndTime.SetTime(otherBrain.Info.ID, this, StructureAndTime.IDTypes.Follower);
    this.HardSwapToTask((FollowerTask) new FollowerTask_TaxEnforce(otherBrain));
    return true;
  }

  public bool CheckForSpeakers(Structure structure)
  {
    if (structure.Type == StructureBrain.TYPES.PROPAGANDA_SPEAKER && structure.Structure_Info.Fuel > 0)
    {
      BoxCollider2D boxCollider2D = GameManager.GetInstance().GetComponent<BoxCollider2D>();
      if ((UnityEngine.Object) boxCollider2D == (UnityEngine.Object) null)
      {
        boxCollider2D = GameManager.GetInstance().gameObject.AddComponent<BoxCollider2D>();
        boxCollider2D.isTrigger = true;
      }
      boxCollider2D.size = Vector2.one * Structures_PropagandaSpeaker.EFFECTIVE_DISTANCE;
      boxCollider2D.transform.position = structure.Brain.Data.Position;
      boxCollider2D.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -45f));
      if (boxCollider2D.OverlapPoint((Vector2) this.LastPosition))
        return true;
    }
    return false;
  }

  public bool CheckForInteraction(Structure structure, float CheckDistance)
  {
    StructureBrain brain = structure.Brain;
    if (TimeManager.IsNight || this.CurrentOverrideTaskType != FollowerTaskType.None)
      return false;
    switch (structure.Type)
    {
      case StructureBrain.TYPES.GRAVE:
      case StructureBrain.TYPES.GRAVE2:
        if (brain.Data.FollowerID != -1 && this.CurrentTaskType != FollowerTaskType.BuryBody && (double) this.GetTimeSinceTask(FollowerTaskType.ReactGrave) > 120.0 && (double) TimeManager.TotalElapsedGameTime - (double) StructureAndTime.GetOrAddTime(brain.Data.ID, this, StructureAndTime.IDTypes.Structure) > 1200.0)
        {
          StructureAndTime.SetTime(structure.Structure_Info.ID, this, StructureAndTime.IDTypes.Structure);
          this.HardSwapToTask((FollowerTask) new FollowerTask_ReactGrave(structure.Structure_Info.ID));
          return true;
        }
        break;
      case StructureBrain.TYPES.DEAD_WORSHIPPER:
        if (this.CurrentTaskType != FollowerTaskType.BuryBody && (double) TimeManager.TotalElapsedGameTime - (double) StructureAndTime.GetOrAddTime(brain.Data.ID, this, StructureAndTime.IDTypes.Structure) > 600.0 && (double) this.GetTimeSinceTask(FollowerTaskType.ReactCorpse) > 60.0 && !brain.ReservedForTask)
        {
          StructureAndTime.SetTime(structure.Structure_Info.ID, this, StructureAndTime.IDTypes.Structure);
          this.HardSwapToTask((FollowerTask) new FollowerTask_ReactCorpse(structure.Structure_Info.ID));
          return true;
        }
        break;
      case StructureBrain.TYPES.VOMIT:
        if (this.CurrentTaskType != FollowerTaskType.Ill && this.CurrentTaskType != FollowerTaskType.CleanWaste && (double) this.GetTimeSinceTask(FollowerTaskType.ReactVomit) > 60.0 && (double) TimeManager.TotalElapsedGameTime - (double) StructureAndTime.GetOrAddTime(brain.Data.ID, this, StructureAndTime.IDTypes.Structure) > 600.0)
        {
          StructureAndTime.SetTime(structure.Structure_Info.ID, this, StructureAndTime.IDTypes.Structure);
          this.HardSwapToTask((FollowerTask) new FollowerTask_ReactVomit(structure.Structure_Info.ID));
          return true;
        }
        break;
      case StructureBrain.TYPES.PRISON:
        if (brain.Data.FollowerID != -1 && brain.Data.FollowerID != this._directInfoAccess.ID && this.CurrentTaskType != FollowerTaskType.ReactPrisoner && (double) this.GetTimeSinceTask(FollowerTaskType.ReactPrisoner) > 120.0 && (double) TimeManager.TotalElapsedGameTime - (double) StructureAndTime.GetOrAddTime(brain.Data.ID, this, StructureAndTime.IDTypes.Structure) > 1200.0)
        {
          StructureAndTime.SetTime(structure.Structure_Info.ID, this, StructureAndTime.IDTypes.Structure);
          this.HardSwapToTask((FollowerTask) new FollowerTask_ReactPrisoner(structure.Structure_Info.ID));
          return true;
        }
        break;
      case StructureBrain.TYPES.OUTHOUSE:
        if (brain.IsFull && (double) this.GetTimeSinceTask(FollowerTaskType.ReactOuthouse) > 60.0 && (double) TimeManager.TotalElapsedGameTime - (double) StructureAndTime.GetOrAddTime(brain.Data.ID, this, StructureAndTime.IDTypes.Structure) > 600.0)
        {
          StructureAndTime.SetTime(structure.Structure_Info.ID, this, StructureAndTime.IDTypes.Structure);
          this.HardSwapToTask((FollowerTask) new FollowerTask_ReactOuthouse(structure.Structure_Info.ID));
          return true;
        }
        break;
      case StructureBrain.TYPES.POOP:
        if (structure.Structure_Info.FollowerID != this.Info.ID && this.CurrentTaskType != FollowerTaskType.CleanWaste && (double) this.GetTimeSinceTask(FollowerTaskType.ReactPoop) > 60.0 && (double) TimeManager.TotalElapsedGameTime - (double) StructureAndTime.GetOrAddTime(brain.Data.ID, this, StructureAndTime.IDTypes.Structure) > 600.0)
        {
          StructureAndTime.SetTime(structure.Structure_Info.ID, this, StructureAndTime.IDTypes.Structure);
          this.HardSwapToTask((FollowerTask) new FollowerTask_ReactPoop(structure.Structure_Info.ID));
          return true;
        }
        break;
      case StructureBrain.TYPES.DECORATION_LAMB_STATUE:
      case StructureBrain.TYPES.DECORATION_FLAG_SCRIPTURE:
      case StructureBrain.TYPES.DECORATION_LAMB_FLAG_STATUE:
      case StructureBrain.TYPES.DECORATION_BELL_STATUE:
      case StructureBrain.TYPES.DECORATION_BONE_ARCH:
      case StructureBrain.TYPES.DECORATION_BONE_CANDLE:
      case StructureBrain.TYPES.DECORATION_BONE_FLAG:
      case StructureBrain.TYPES.DECORATION_BONE_LANTERN:
      case StructureBrain.TYPES.DECORATION_BONE_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_CRYSTAL_ROCK:
      case StructureBrain.TYPES.DECORATION_CRYSTAL_STATUE:
      case StructureBrain.TYPES.DECORATION_CRYSTAL_TREE:
      case StructureBrain.TYPES.DECORATION_CRYSTAL_WINDOW:
      case StructureBrain.TYPES.DECORATION_FLOWER_ARCH:
      case StructureBrain.TYPES.DECORATION_FOUNTAIN:
      case StructureBrain.TYPES.DECORATION_BONE_SKULL_BIG:
      case StructureBrain.TYPES.DECORATION_BONE_SKULL_PILE:
      case StructureBrain.TYPES.DECORATION_LEAFY_FLOWER_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_LEAFY_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_MUSHROOM_CANDLE_LARGE:
      case StructureBrain.TYPES.DECORATION_MUSHROOM_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_SPIDER_PILLAR:
      case StructureBrain.TYPES.DECORATION_SPIDER_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_SPIDER_WEB_CROWN_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_STONE_HENGE:
      case StructureBrain.TYPES.DECORATION_POND:
        if ((double) this.GetTimeSinceTask(FollowerTaskType.ReactDecoration) > 120.0 && (double) TimeManager.TotalElapsedGameTime - (double) StructureAndTime.GetOrAddTime(brain.Data.ID, this, StructureAndTime.IDTypes.Structure) > 600.0)
        {
          StructureAndTime.SetTime(structure.Structure_Info.ID, this, StructureAndTime.IDTypes.Structure);
          this.HardSwapToTask((FollowerTask) new FollowerTask_ReactDecorations(structure.Structure_Info.ID));
          return true;
        }
        break;
      case StructureBrain.TYPES.SHRINE_PASSIVE:
      case StructureBrain.TYPES.SHRINE_PASSIVE_II:
      case StructureBrain.TYPES.SHRINE_PASSIVE_III:
        if (((Structures_Shrine_Passive) brain).SoulCount < ((Structures_Shrine_Passive) brain).SoulMax && ((Structures_Shrine_Passive) brain).PrayAvailable(structure.Type) && !brain.ReservedForTask && (double) TimeManager.TotalElapsedGameTime - (double) StructureAndTime.GetOrAddTime(brain.Data.ID, this, StructureAndTime.IDTypes.Structure) > 600.0 && (double) Vector3.Distance(this.LastPosition, brain.Data.Position) < (double) Structures_Shrine_Passive.Range(brain.Data.Type))
        {
          StructureAndTime.SetTime(structure.Structure_Info.ID, this, StructureAndTime.IDTypes.Structure);
          this.HardSwapToTask((FollowerTask) new FollowerTask_PrayPassive(structure.Structure_Info.ID));
          return false;
        }
        break;
      default:
        return false;
    }
    return false;
  }

  public bool CheckForSimInteraction(StructureBrain structureBrain)
  {
    if (structureBrain.Data.Type == StructureBrain.TYPES.PROPAGANDA_SPEAKER && structureBrain.Data.Fuel > 0 && (double) Vector3.Distance(structureBrain.Data.Position, this.LastPosition) < (double) Structures_PropagandaSpeaker.EFFECTIVE_DISTANCE)
      ++this.SpeakersInRange;
    switch (structureBrain.Data.Type)
    {
      case StructureBrain.TYPES.DECORATION_LAMB_STATUE:
      case StructureBrain.TYPES.DECORATION_FLAG_SCRIPTURE:
      case StructureBrain.TYPES.DECORATION_LAMB_FLAG_STATUE:
      case StructureBrain.TYPES.DECORATION_BELL_STATUE:
      case StructureBrain.TYPES.DECORATION_BONE_ARCH:
      case StructureBrain.TYPES.DECORATION_BONE_CANDLE:
      case StructureBrain.TYPES.DECORATION_BONE_FLAG:
      case StructureBrain.TYPES.DECORATION_BONE_LANTERN:
      case StructureBrain.TYPES.DECORATION_BONE_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_CRYSTAL_ROCK:
      case StructureBrain.TYPES.DECORATION_CRYSTAL_STATUE:
      case StructureBrain.TYPES.DECORATION_CRYSTAL_TREE:
      case StructureBrain.TYPES.DECORATION_CRYSTAL_WINDOW:
      case StructureBrain.TYPES.DECORATION_FLOWER_ARCH:
      case StructureBrain.TYPES.DECORATION_FOUNTAIN:
      case StructureBrain.TYPES.DECORATION_BONE_SKULL_BIG:
      case StructureBrain.TYPES.DECORATION_BONE_SKULL_PILE:
      case StructureBrain.TYPES.DECORATION_LEAFY_FLOWER_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_LEAFY_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_MUSHROOM_CANDLE_LARGE:
      case StructureBrain.TYPES.DECORATION_MUSHROOM_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_SPIDER_PILLAR:
      case StructureBrain.TYPES.DECORATION_SPIDER_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_SPIDER_WEB_CROWN_SCULPTURE:
      case StructureBrain.TYPES.DECORATION_STONE_HENGE:
      case StructureBrain.TYPES.DECORATION_POND:
        if ((double) this.GetTimeSinceTask(FollowerTaskType.ReactDecoration) > 120.0 && (double) TimeManager.TotalElapsedGameTime - (double) StructureAndTime.GetOrAddTime(structureBrain.Data.ID, this, StructureAndTime.IDTypes.Structure) > 600.0)
        {
          StructureAndTime.SetTime(structureBrain.Data.ID, this, StructureAndTime.IDTypes.Structure);
          this.HardSwapToTask((FollowerTask) new FollowerTask_ReactDecorations(structureBrain.Data.ID));
          return true;
        }
        break;
      case StructureBrain.TYPES.SHRINE_PASSIVE:
      case StructureBrain.TYPES.SHRINE_PASSIVE_II:
      case StructureBrain.TYPES.SHRINE_PASSIVE_III:
        if (((Structures_Shrine_Passive) structureBrain).SoulCount < ((Structures_Shrine_Passive) structureBrain).SoulMax && ((Structures_Shrine_Passive) structureBrain).PrayAvailable(structureBrain.Data.Type) && !structureBrain.ReservedForTask && (double) TimeManager.TotalElapsedGameTime - (double) StructureAndTime.GetOrAddTime(structureBrain.Data.ID, this, StructureAndTime.IDTypes.Structure) > 600.0 && (double) Vector3.Distance(this.LastPosition, structureBrain.Data.Position) < (double) Structures_Shrine_Passive.Range(structureBrain.Data.Type))
        {
          StructureAndTime.SetTime(structureBrain.Data.ID, this, StructureAndTime.IDTypes.Structure);
          this.HardSwapToTask((FollowerTask) new FollowerTask_PrayPassive(structureBrain.Data.ID));
          return true;
        }
        break;
      default:
        return false;
    }
    return false;
  }

  public Follower.ComplaintType GetMostPressingComplaint()
  {
    if (this.Info.FirstTimeSpeakingToPlayer)
      return Follower.ComplaintType.FirstTimeSpeakingToPlayer;
    Follower.ComplaintType pressingComplaint = Follower.ComplaintType.None;
    using (IEnumerator<Follower.ComplaintType> enumerator = this.GetOrderedComplaints().GetEnumerator())
    {
      if (enumerator.MoveNext())
        pressingComplaint = enumerator.Current;
    }
    return pressingComplaint;
  }

  public IEnumerable<Follower.ComplaintType> GetOrderedComplaints()
  {
    if ((double) this.Stats.Illness > 0.0)
      yield return Follower.ComplaintType.Sick;
    if ((double) this.Stats.Satiation <= 30.0)
      yield return Follower.ComplaintType.Hunger;
    if (!this.HasHome)
      yield return Follower.ComplaintType.Homeless;
    if (this.HasHome && !this.HomeIsCorrectLevel)
      yield return Follower.ComplaintType.NeedBetterHouse;
  }

  private void OnStructureAdded(StructuresData structure)
  {
    if (structure.Type == StructureBrain.TYPES.TEMPLE)
      this.RemoveThought(Thought.NoTemple, true);
    this.CheckChangeTask();
  }

  private void OnStructureMoved(StructuresData structure)
  {
    if (structure.ID != this.CurrentTaskUsingStructureID)
      return;
    this.CurrentTask.Abort();
  }

  private void OnStructureUpgraded(StructuresData structure)
  {
    if (structure.ID != this.CurrentTaskUsingStructureID)
      return;
    this.CurrentTask.Abort();
  }

  private void OnStructureRemoved(StructuresData structure)
  {
    if (structure.ID == this.CurrentTaskUsingStructureID)
      this.CurrentTask.Abort();
    if (structure.ID == this.HomeID)
      this.ClearDwelling();
    this.CheckChangeTask();
  }

  private void OnHappinessStateChanged(
    int followerId,
    FollowerStatState newState,
    FollowerStatState oldState)
  {
  }

  private void OnStarvationChanged(
    int followerId,
    FollowerStatState newState,
    FollowerStatState oldState)
  {
    if (followerId != this.Info.ID || newState == oldState)
      return;
    if (newState == FollowerStatState.On)
    {
      this.AddThought(Thought.BecomeStarving);
    }
    else
    {
      if (newState != FollowerStatState.Off)
        return;
      this.RemoveCurseState(Thought.BecomeStarving);
      this.RemoveThought(Thought.BecomeStarving, true);
      this.AddThought(Thought.NoLongerStarving);
    }
  }

  public void MakeOld()
  {
    if (this.Info.CursedState != Thought.None)
      return;
    this.ApplyCurseState(Thought.OldAge);
    if (this.Info.CursedState != Thought.OldAge)
      return;
    this.Info.OldAge = true;
    DataManager.Instance.LastFollowerToReachOldAge = TimeManager.TotalElapsedGameTime;
  }

  public void MakeExhausted()
  {
    if ((double) this.Stats.Exhaustion > 0.0)
      return;
    this.Stats.Exhaustion = 50f;
    FollowerBrainStats.StatStateChangedEvent exhaustionStateChanged = FollowerBrainStats.OnExhaustionStateChanged;
    if (exhaustionStateChanged != null)
      exhaustionStateChanged(this.Info.ID, FollowerStatState.On, FollowerStatState.Off);
    this.CheckChangeState();
    if (DataManager.Instance.WokeUpEveryoneDay == -3)
    {
      DataManager.Instance.WokeUpEveryoneDay = -2;
      CultFaithManager.AddThought(Thought.Cult_ExhaustedEveryone);
    }
    else
    {
      if (DataManager.Instance.WokeUpEveryoneDay != -1)
        return;
      CultFaithManager.AddThought(Thought.Cult_ExhaustedFollower, this.Info.ID);
    }
  }

  public void MakeSick()
  {
    if (this.Info.CursedState != Thought.None)
      return;
    this.ApplyCurseState(Thought.Ill);
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.Info.ID != this.Info.ID && !DataManager.Instance.Followers_Recruit.Contains(allBrain._directInfoAccess))
      {
        if (allBrain.HasTrait(FollowerTrait.TraitType.FearOfSickPeople))
          allBrain.AddThought(Thought.SomeoneIllFearOfSickTrait, true);
        else if (allBrain.HasTrait(FollowerTrait.TraitType.LoveOfSickPeople))
          allBrain.AddThought(Thought.SomeoneIllLoveOfSickTrait, true);
        if (allBrain.HasTrait(FollowerTrait.TraitType.FearOfSickPeople))
          CultFaithManager.AddThought(Thought.Cult_FearSick, allBrain.Info.ID);
        else if (allBrain.HasTrait(FollowerTrait.TraitType.LoveOfSickPeople))
          CultFaithManager.AddThought(Thought.Cult_LoveSick, allBrain.Info.ID);
      }
    }
    this.Stats.Illness = 50f;
    FollowerBrainStats.StatStateChangedEvent illnessStateChanged = FollowerBrainStats.OnIllnessStateChanged;
    if (illnessStateChanged != null)
      illnessStateChanged(this.Info.ID, FollowerStatState.On, FollowerStatState.Off);
    this.CheckChangeState();
  }

  public void MakeStarve()
  {
    if (this.Info.CursedState != Thought.None || this.HasTrait(FollowerTrait.TraitType.DontStarve))
      return;
    this.ApplyCurseState(Thought.BecomeStarving);
    this.Stats.IsStarving = true;
    this._directInfoAccess.Satiation = 0.0f;
    this.Stats.Starvation = 37.5f;
    FollowerBrainStats.StatStateChangedEvent starvationStateChanged = FollowerBrainStats.OnStarvationStateChanged;
    if (starvationStateChanged != null)
      starvationStateChanged(this.Info.ID, FollowerStatState.On, FollowerStatState.Off);
    NotificationCentre.Instance.PlayFollowerNotification(NotificationCentre.NotificationType.Starving, this.Info, NotificationFollower.Animation.Unhappy);
    this.CheckChangeState();
    this.CheckChangeTask();
  }

  public void MakeDissenter()
  {
    if (this.Info.CursedState != Thought.None)
      return;
    this.ApplyCurseState(Thought.Dissenter);
    this.CheckChangeState();
    FollowerManager.FindFollowerByID(this.Info.ID)?.SetOutfit(this.Info.Outfit, false);
    if (this.Info.CursedState != Thought.Dissenter)
      return;
    FollowerBrainStats.StatStateChangedEvent reeducationStateChanged = FollowerBrainStats.OnReeducationStateChanged;
    if (reeducationStateChanged == null)
      return;
    reeducationStateChanged(this.Info.ID, FollowerStatState.On, FollowerStatState.Off);
  }

  private void OnExhaustionStateChanged(
    int followerId,
    FollowerStatState newState,
    FollowerStatState oldState)
  {
    if (followerId != this.Info.ID)
      return;
    if (newState != oldState)
    {
      switch (newState)
      {
        case FollowerStatState.Off:
          Debug.Log((object) "EXHAUSTION OFF!!!");
          if (DataManager.Instance.WokeUpEveryoneDay == -4)
          {
            CultFaithManager.AddThought(Thought.Cult_NoLongerExhaustedEveryone);
            DataManager.Instance.WokeUpEveryoneDay = -5;
            break;
          }
          if (DataManager.Instance.WokeUpEveryoneDay == -1)
          {
            CultFaithManager.AddThought(Thought.Cult_NoLongerExhaustedFollower, followerId);
            break;
          }
          break;
        case FollowerStatState.On:
          Debug.Log((object) "EXHAUSTION ON!!!");
          break;
      }
    }
    this.CheckChangeState();
  }

  private void OnIllnessStateChanged(
    int followerId,
    FollowerStatState newState,
    FollowerStatState oldState)
  {
    if (followerId != this.Info.ID)
      return;
    if (newState != oldState && newState == FollowerStatState.Off)
    {
      Debug.Log((object) "ILLNESS OFF!!!");
      this.RemoveThought(Thought.Ill, true);
      this.RemoveCurseState(Thought.Ill);
      foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
      {
        allBrain.RemoveThought(Thought.SomeoneIllFearOfSickTrait, false);
        allBrain.RemoveThought(Thought.SomeoneIllLoveOfSickTrait, false);
      }
      NotificationCentre.Instance.PlayFollowerNotification(NotificationCentre.NotificationType.NoLongerIll, this.Info, NotificationFollower.Animation.Happy);
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.FollowerRecoverIllness, followerId);
    }
    this.CheckChangeState();
  }

  private void OnSatiationStateChanged(
    int followerID,
    FollowerStatState newState,
    FollowerStatState oldState)
  {
    if (followerID != this.Info.ID)
      return;
    this.CheckChangeTask();
  }

  private void OnStarvationStateChanged(
    int followerID,
    FollowerStatState newState,
    FollowerStatState oldState)
  {
    if (followerID != this.Info.ID)
      return;
    this.CheckChangeTask();
  }

  private void OnRestStateChanged(
    int followerID,
    FollowerStatState newState,
    FollowerStatState oldState)
  {
    if (followerID != this.Info.ID)
      return;
    this.CheckChangeState();
    this.CheckChangeTask();
  }

  private void OnBathroomStateChanged(
    int followerID,
    FollowerStatState newState,
    FollowerStatState oldState)
  {
    if (followerID != this.Info.ID)
      return;
    this.CheckChangeTask();
  }

  private void OnMotivatedChanged(int followerID)
  {
    if (followerID != this.Info.ID)
      return;
    this.CheckChangeState();
  }

  private void OnResearchBegin() => this.CheckChangeTask();

  private void OnNewPhaseStarted()
  {
    this.CheckChangeTask();
    switch (TimeManager.CurrentPhase)
    {
      case DayPhase.Afternoon:
        this._directInfoAccess.WakeUpDay = -1;
        break;
      case DayPhase.Night:
        if (DataManager.Instance.WokeUpEveryoneDay == -2)
        {
          DataManager.Instance.WokeUpEveryoneDay = -4;
          break;
        }
        if (DataManager.Instance.WokeUpEveryoneDay != -5)
          break;
        DataManager.Instance.WokeUpEveryoneDay = -1;
        break;
    }
  }

  private void OnScheduleChanged() => this.CheckChangeTask();

  public float GetHungerScore()
  {
    float hungerScore = 0.0f;
    if (!FollowerBrainStats.Fasting)
      hungerScore = (float) Mathf.FloorToInt(this.Stats.Starvation + (100f - this.Stats.Satiation)) + (float) this.Info.ID / 1000f;
    return hungerScore;
  }

  public float GetHungerScoreNotStarving()
  {
    float scoreNotStarving = 0.0f;
    if (!this.Stats.IsStarving && !FollowerBrainStats.Fasting && this.CurrentTaskType != FollowerTaskType.EatMeal && this.CurrentTaskType != FollowerTaskType.EatStoredFood)
      scoreNotStarving = (float) Mathf.FloorToInt(this.Stats.Starvation + (100f - this.Stats.Satiation)) + (float) this.Info.ID / 1000f;
    return scoreNotStarving;
  }

  public float GetHungerScoreNoCursedState()
  {
    float scoreNoCursedState = 0.0f;
    if (this.Info.CursedState == Thought.None && !FollowerBrainStats.Fasting && this.CurrentTaskType != FollowerTaskType.EatMeal && this.CurrentTaskType != FollowerTaskType.EatStoredFood)
      scoreNoCursedState = (float) Mathf.FloorToInt(this.Stats.Starvation + (100f - this.Stats.Satiation)) + (float) this.Info.ID / 1000f;
    return scoreNoCursedState;
  }

  public FollowerState CurrentState
  {
    get => this._currentState;
    private set
    {
      FollowerState currentState = this._currentState;
      this._currentState = value;
      Action<FollowerState, FollowerState> onStateChanged = this.OnStateChanged;
      if (onStateChanged == null)
        return;
      onStateChanged(this._currentState, currentState);
    }
  }

  public void CheckChangeState()
  {
    FollowerState followerState = this.GetFollowerState();
    if (followerState == null)
      return;
    this.CurrentState = followerState;
  }

  private FollowerState GetFollowerState()
  {
    if (this.Info.CursedState == Thought.OldAge)
      return (FollowerState) new FollowerState_OldAge();
    if (this.Info.CursedState == Thought.Ill)
      return (FollowerState) new FollowerState_Ill();
    if (this.FollowingPlayer)
      return (FollowerState) new FollowerState_Following();
    if (this.InRitual)
      return (FollowerState) new FollowerState_Ritual();
    if ((double) this.Stats.Exhaustion > 0.0)
      return (FollowerState) new FollowerState_Exhausted();
    return this.Stats.Motivated ? (FollowerState) new FollowerState_Motivated() : (FollowerState) new FollowerState_Default();
  }

  public bool ShouldReconsiderTask { get; set; }

  public FollowerTask CurrentTask
  {
    get => (FollowerTask) this._currentTask?.ChangeLocationTask ?? this._currentTask;
    private set
    {
      if (this._currentTask != null && this._currentTask.State != FollowerTaskState.Done)
        Debug.Log((object) this._currentTask.Type);
      FollowerTask currentTask = this._currentTask;
      this._currentTask = value;
      currentTask?.ReleaseReservations();
      Action<FollowerTask, FollowerTask> onTaskChanged = this.OnTaskChanged;
      if (onTaskChanged != null)
        onTaskChanged(this._currentTask, currentTask);
      if (this._currentTask != null)
        this._currentTask.Start();
      if (currentTask == null)
        return;
      TaskAndTime.SetTaskTime(TimeManager.TotalElapsedGameTime, currentTask.Type, this);
    }
  }

  public FollowerTaskType CurrentTaskType
  {
    get => this.CurrentTask != null ? this.CurrentTask.Type : FollowerTaskType.None;
  }

  public int CurrentTaskUsingStructureID
  {
    get => this.CurrentTask != null ? this.CurrentTask.UsingStructureID : 0;
  }

  public void CheckChangeTask()
  {
    if (this.CurrentTask != null && this.CurrentTask.BlockTaskChanges)
      return;
    this.ShouldReconsiderTask = true;
  }

  public bool BeginReconsider()
  {
    this._currentTask?.ReleaseReservations();
    this._nextTask?.ReleaseReservations();
    FollowerTask personalTask = this.GetPersonalTask(this.HomeLocation);
    if (personalTask != null)
    {
      this._currentTask?.ClaimReservations();
      this._nextTask?.ClaimReservations();
      FollowerTaskType followerTaskType = this.CurrentTaskType != FollowerTaskType.ChangeLocation ? this.CurrentTaskType : ((FollowerTask_ChangeLocation) this.CurrentTask).ParentType;
      if (personalTask.Type != followerTaskType)
        this.TransitionToTask(personalTask);
      this.ShouldReconsiderTask = false;
    }
    else
      this.PendingTask = new FollowerBrain.PendingTaskData();
    return this.ShouldReconsiderTask;
  }

  public int TryClaimExistingTask(List<FollowerTask> availableTasks)
  {
    if (this.CurrentTask != null)
    {
      for (int index = 0; index < availableTasks.Count; ++index)
      {
        FollowerTask availableTask = availableTasks[index];
        if (availableTask != null && availableTask.GetUniqueTaskCode() == this.CurrentTask.GetUniqueTaskCode())
        {
          this.PendingTask.KeepExistingTask = true;
          this.PendingTask.Task = availableTask;
          this.PendingTask.ListIndex = index;
          availableTasks[index] = (FollowerTask) null;
          return index;
        }
      }
    }
    return -1;
  }

  public int ClaimNextAvailableTask(List<FollowerTask> availableTasks)
  {
    for (int index1 = 0; index1 < 5; ++index1)
    {
      for (int index2 = 0; index2 < availableTasks.Count; ++index2)
      {
        FollowerTask availableTask = availableTasks[index2];
        if (availableTask != null && FollowerTask.RequiredFollowerLevel(this.Info.FollowerRole, availableTask.Type) && availableTask.GetPriorityCategory(this.Info.FollowerRole, this.Info.WorkerPriority, this) == (PriorityCategory) index1)
        {
          this.PendingTask.KeepExistingTask = false;
          this.PendingTask.Task = availableTask;
          this.PendingTask.ListIndex = index2;
          availableTasks[index2] = (FollowerTask) null;
          return index2;
        }
      }
    }
    return -1;
  }

  public void EndReconsider()
  {
    this._currentTask?.ClaimReservations();
    this._nextTask?.ClaimReservations();
    if (this.PendingTask.Task != null)
    {
      if (!this.PendingTask.KeepExistingTask)
        this.TransitionToTask(this.PendingTask.Task);
    }
    else
    {
      FollowerTask fallbackTask = this.GetFallbackTask(TimeManager.GetScheduledActivity(this.HomeLocation));
      if (this.CurrentTaskType != fallbackTask.Type)
        this.TransitionToTask(fallbackTask);
    }
    this.ShouldReconsiderTask = false;
  }

  public void CompleteCurrentTask() => this.TransitionToTask((FollowerTask) null);

  public void TransitionToTask(FollowerTask nextTask)
  {
    nextTask?.Init(this);
    if (this.CurrentTask == null)
    {
      this.CurrentTask = nextTask;
    }
    else
    {
      this._nextTask?.ReleaseReservations();
      this._nextTask = nextTask;
      this._nextTask?.ClaimReservations();
      this.CurrentTask.End();
    }
  }

  public void HardSwapToTask(FollowerTask nextTask)
  {
    nextTask?.Init(this);
    if (this.CurrentTask == null)
    {
      this.CurrentTask = nextTask;
    }
    else
    {
      this._nextTask?.ReleaseReservations();
      this._nextTask = nextTask;
      this._nextTask?.ClaimReservations();
      this.CurrentTask.Abort();
    }
  }

  public void ContinueToNextTask()
  {
    FollowerTask nextTask = this._nextTask;
    this._nextTask = (FollowerTask) null;
    nextTask?.ReleaseReservations();
    this.CurrentTask = nextTask;
  }

  public FollowerTaskType CurrentOverrideTaskType
  {
    get => this._directInfoAccess.CurrentOverrideTaskType;
    set => this._directInfoAccess.CurrentOverrideTaskType = value;
  }

  public StructureBrain.TYPES CurrentOverrideStructureType
  {
    get => this._directInfoAccess.CurrentOverrideStructureType;
    set => this._directInfoAccess.CurrentOverrideStructureType = value;
  }

  private int OverrideDayIndex
  {
    get => this._directInfoAccess.OverrideDayIndex;
    set => this._directInfoAccess.OverrideDayIndex = value;
  }

  private bool OverrideTaskCompleted
  {
    get => this._directInfoAccess.OverrideTaskCompleted;
    set => this._directInfoAccess.OverrideTaskCompleted = value;
  }

  public void SetPersonalOverrideTask(
    FollowerTaskType Type,
    StructureBrain.TYPES OverrideStructureType = StructureBrain.TYPES.NONE)
  {
    this.CompleteCurrentTask();
    this.OverrideTaskCompleted = false;
    this.OverrideDayIndex = TimeManager.CurrentDay;
    this.ClearPersonalOverrideTaskProvider();
    this.CurrentOverrideTaskType = Type;
    this.CurrentOverrideStructureType = OverrideStructureType;
    this.CheckChangeTask();
  }

  public void ClearPersonalOverrideTaskProvider()
  {
    this.CurrentOverrideTaskType = FollowerTaskType.None;
  }

  private bool CheckOverrideComplete()
  {
    switch (this.CurrentOverrideTaskType)
    {
      case FollowerTaskType.EatMeal:
        return (double) this.Stats.Satiation > 60.0 && TimeManager.CurrentDay > this.OverrideDayIndex;
      case FollowerTaskType.Sleep:
        return TimeManager.CurrentDay > this.OverrideDayIndex;
      default:
        return false;
    }
  }

  public FollowerTask GetPersonalOverrideTask()
  {
    FollowerTask personalOverrideTask = (FollowerTask) null;
    if (this.CurrentOverrideTaskType != FollowerTaskType.None)
    {
      if (this.CheckOverrideComplete())
      {
        this.ClearPersonalOverrideTaskProvider();
      }
      else
      {
        switch (this.CurrentOverrideTaskType)
        {
          case FollowerTaskType.EatMeal:
            return this.CheckEatTask(true);
          case FollowerTaskType.Sleep:
            return this.Info.CursedState == Thought.Ill && this.CurrentTaskType != FollowerTaskType.ClaimDwelling ? (FollowerTask) new FollowerTask_SleepBedRest() : (FollowerTask) new FollowerTask_Sleep();
          case FollowerTaskType.SleepBedRest:
            return this.CurrentTaskType != FollowerTaskType.ClaimDwelling ? (FollowerTask) new FollowerTask_SleepBedRest() : (FollowerTask) null;
          default:
            return (FollowerTask) null;
        }
      }
    }
    return personalOverrideTask;
  }

  public float GetTimeSinceTask(FollowerTaskType taskType)
  {
    float timeSinceTask = 0.0f;
    if (this.CurrentTaskType != taskType)
      timeSinceTask = TimeManager.TotalElapsedGameTime - TaskAndTime.GetLastTaskTime(taskType, this);
    return timeSinceTask;
  }

  public FollowerTask GetPersonalTask(FollowerLocation location)
  {
    if (this.LeavingCult)
      return (FollowerTask) new FollowerTask_LeaveCult(NotificationCentre.NotificationType.LeaveCultUnhappy);
    if (this.Info.CursedState != Thought.Zombie)
    {
      if (this.DiedOfStarvation)
        return (FollowerTask) new FollowerTask_FindPlaceToDie(NotificationCentre.NotificationType.DiedFromStarvation);
      if (this.DiedOfIllness)
        return (FollowerTask) new FollowerTask_FindPlaceToDie(NotificationCentre.NotificationType.DiedFromIllness);
      if (this.DiedOfOldAge)
        return (FollowerTask) new FollowerTask_FindPlaceToDie(NotificationCentre.NotificationType.DiedFromOldAge);
    }
    ScheduledActivity scheduledActivity = TimeManager.GetScheduledActivity(location);
    if (scheduledActivity == ScheduledActivity.Sleep && this._directInfoAccess.WorkThroughNight)
      scheduledActivity = ScheduledActivity.Work;
    if (scheduledActivity == ScheduledActivity.Work && FollowerBrainStats.IsHoliday)
      scheduledActivity = ScheduledActivity.Leisure;
    FollowerTask stateTask = this.GetStateTask(location);
    if (stateTask != null)
    {
      this.FollowingPlayer = false;
      return stateTask;
    }
    if ((double) this.Stats.Exhaustion > 0.0 && ((double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.LastFollowerToPassOut > 1200.0 || (double) this.Stats.Exhaustion >= 100.0) && (double) UnityEngine.Random.value < 0.02500000037252903)
    {
      DataManager.Instance.LastFollowerToPassOut = TimeManager.TotalElapsedGameTime;
      return (FollowerTask) new FollowerTask_Sleep(true);
    }
    if (this.FollowingPlayer)
      return (FollowerTask) new FollowerTask_FollowPlayer();
    if (this.CurrentTaskType == FollowerTaskType.ManualControl)
      return (FollowerTask) new FollowerTask_ManualControl();
    FollowerTask personalOverrideTask = this.GetPersonalOverrideTask();
    if (personalOverrideTask != null)
      return personalOverrideTask;
    if (!this.Stats.WorkerBeenGivenOrders && this.Info.FollowerRole == FollowerRole.Worker && scheduledActivity != ScheduledActivity.Sleep)
      return (FollowerTask) new FollowerTask_AwaitInstruction();
    if (this.Stats.CachedLumber > 0 && this.CurrentTaskType != FollowerTaskType.Lumberjack)
      return (FollowerTask) new FollowerTask_DepositWood(this.Stats.CachedLumberjackStationID);
    FollowerTask overrideTask = TimeManager.GetOverrideTask(this);
    if (overrideTask != null)
      return overrideTask;
    if (!this.HasHome && (scheduledActivity == ScheduledActivity.Work || scheduledActivity == ScheduledActivity.Sleep || scheduledActivity == ScheduledActivity.Leisure || this.CurrentTaskType == FollowerTaskType.SleepBedRest))
    {
      Dwelling.DwellingAndSlot freeDwellingAndSlot = StructureManager.GetFreeDwellingAndSlot(location, this._directInfoAccess);
      if (freeDwellingAndSlot != null && !StructureManager.GetStructureByID<Structures_Bed>(freeDwellingAndSlot.ID).ReservedForTask)
      {
        StructureManager.GetStructureByID<Structures_Bed>(freeDwellingAndSlot.ID).ReservedForTask = true;
        return (FollowerTask) new FollowerTask_ClaimDwelling(freeDwellingAndSlot);
      }
    }
    if (this.HasHome && (scheduledActivity == ScheduledActivity.Work || scheduledActivity == ScheduledActivity.Sleep || scheduledActivity == ScheduledActivity.Leisure))
    {
      Structures_Bed structureById = StructureManager.GetStructureByID<Structures_Bed>(this._directInfoAccess.DwellingID);
      if (structureById != null)
      {
        if (!structureById.Data.Claimed)
        {
          Dwelling.DwellingAndSlot dwellingAndSlot = new Dwelling.DwellingAndSlot(this._directInfoAccess.DwellingID, this._directInfoAccess.DwellingSlot, this._directInfoAccess.DwellingLevel);
          StructureManager.GetStructureByID<Structures_Bed>(dwellingAndSlot.ID).ReservedForTask = true;
          return (FollowerTask) new FollowerTask_ClaimDwelling(dwellingAndSlot);
        }
        if (structureById.IsCollapsed)
        {
          Dwelling.DwellingAndSlot freeDwellingAndSlot = StructureManager.GetFreeDwellingAndSlot(location, this._directInfoAccess);
          if (freeDwellingAndSlot != null && !StructureManager.GetStructureByID<Structures_Bed>(freeDwellingAndSlot.ID).ReservedForTask)
          {
            this.ClearDwelling();
            StructureManager.GetStructureByID<Structures_Bed>(freeDwellingAndSlot.ID).ReservedForTask = true;
            return (FollowerTask) new FollowerTask_ClaimDwelling(freeDwellingAndSlot);
          }
        }
      }
      else
        this._directInfoAccess.DwellingID = Dwelling.NO_HOME;
    }
    if (this.Info.CursedState != Thought.None && (this.CurrentTask == null || !(this.CurrentTask is FollowerTask_Imprisoned)))
    {
      switch (this.Info.CursedState)
      {
        case Thought.Dissenter:
          if (scheduledActivity != ScheduledActivity.Sleep)
            return (FollowerTask) new FollowerTask_Dissent();
          break;
        case Thought.Ill:
          if ((double) this.Stats.Illness == 0.0)
          {
            this.RemoveThought(Thought.Ill, true);
            this.RemoveCurseState(Thought.Ill);
            return (FollowerTask) null;
          }
          if (scheduledActivity != ScheduledActivity.Sleep)
          {
            if (this._directInfoAccess.CursedStateVariant == 0)
              return (FollowerTask) new FollowerTask_Ill();
            if (this._directInfoAccess.CursedStateVariant == 1)
              return (FollowerTask) new FollowerTask_IllPoopy();
            break;
          }
          break;
        case Thought.BecomeStarving:
          if (scheduledActivity != ScheduledActivity.Sleep)
            return (FollowerTask) new FollowerTask_Starving();
          break;
        case Thought.OldAge:
          if (scheduledActivity != ScheduledActivity.Sleep)
            return (FollowerTask) new FollowerTask_OldAge();
          break;
        case Thought.Zombie:
          if (scheduledActivity != ScheduledActivity.Sleep)
            return (FollowerTask) new FollowerTask_Zombie();
          break;
      }
    }
    return (FollowerTask) null;
  }

  public static List<FollowerTask> GetTopPriorityFollowerTasks(FollowerLocation location)
  {
    ScheduledActivity activity = TimeManager.GetScheduledActivity(location);
    if (FollowerBrainStats.IsHoliday)
      activity = ScheduledActivity.Leisure;
    if (FollowerBrainStats.IsWorkThroughTheNight)
      activity = ScheduledActivity.Work;
    return FollowerBrain.GetTopPriorityFollowerTasks(activity, location);
  }

  public static List<FollowerTask> GetTopPriorityFollowerTasks(
    ScheduledActivity activity,
    FollowerLocation location)
  {
    switch (activity)
    {
      case ScheduledActivity.Work:
        return FollowerBrain.GetDesiredTask_Work(location);
      case ScheduledActivity.Study:
        return FollowerBrain.GetDesiredTask_Study(location);
      case ScheduledActivity.Sleep:
        return FollowerBrain.GetDesiredTask_Sleep();
      case ScheduledActivity.Pray:
        return FollowerBrain.GetDesiredTask_Pray();
      case ScheduledActivity.Leisure:
        return FollowerBrain.GetDesiredTask_Leisure();
      default:
        throw new ArgumentException($"Unrecognised ScheduledActivity.{activity}");
    }
  }

  private FollowerTask GetStateTask(FollowerLocation location)
  {
    if ((double) this.Stats.Bathroom > 15.0 && !TimeManager.IsNight && this.CurrentTaskType != FollowerTaskType.Bathroom && this.CurrentOverrideTaskType == FollowerTaskType.None && this.CurrentTaskType != FollowerTaskType.Sleep)
    {
      if ((double) this.bathroomOffset == -1.0)
        this.bathroomOffset = TimeManager.TotalElapsedGameTime + (float) UnityEngine.Random.Range(0, 240 /*0xF0*/);
      else if ((double) TimeManager.TotalElapsedGameTime > (double) this.bathroomOffset)
      {
        this.bathroomOffset = -1f;
        List<Structures_Outhouse> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_Outhouse>(location);
        if (structuresOfType.Count <= 0)
          return (FollowerTask) new FollowerTask_Bathroom();
        foreach (Structures_Outhouse structuresOuthouse in structuresOfType)
        {
          if (!structuresOuthouse.ReservedForTask && !structuresOuthouse.IsFull)
            return (FollowerTask) new FollowerTask_Bathroom(structuresOuthouse.Data.ID);
        }
        return (FollowerTask) new FollowerTask_Bathroom(structuresOfType[UnityEngine.Random.Range(0, structuresOfType.Count)].Data.ID);
      }
    }
    return this.CheckEatTask() ?? (FollowerTask) null;
  }

  public void ApplyCurseState(Thought Curse, Thought SpecialThought = Thought.None)
  {
    if (this.CurrentTask != null && this.CurrentTask.BlockThoughts || this.Info.CursedState != Thought.None)
      return;
    this.Info.CursedState = Curse;
    switch (Curse)
    {
      case Thought.Dissenter:
        this.RemoveThought(Thought.NoLongerDissenting, true);
        this.AddThought(Thought.Dissenter, true);
        NotificationCentre.Instance.PlayFollowerNotification(NotificationCentre.NotificationType.BecomeDissenter, this.Info, NotificationFollower.Animation.Dissenting);
        this.Stats.Reeducation = 50f;
        this.Stats.GivenDissentWarning = false;
        foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
        {
          if (allBrain != this && allBrain.HasTrait(FollowerTrait.TraitType.Zealous))
            allBrain.AddThought(Thought.SomeoneDissenterZealotTrait);
        }
        System.Action onBecomeDissenter = this.OnBecomeDissenter;
        if (onBecomeDissenter != null)
        {
          onBecomeDissenter();
          break;
        }
        break;
      case Thought.Ill:
        this.AddThought(Thought.Ill, true);
        this.Stats.Illness = 50f;
        if (SpecialThought == Thought.Cult_FollowerBecameIllSleepingNextToIllFollower)
        {
          if (this.HasTrait(FollowerTrait.TraitType.Germophobe))
            CultFaithManager.AddThought(Thought.Cult_FollowerBecameIllSleepingNextToIllFollower_Germophobe, this.Info.ID);
          else if (this.HasTrait(FollowerTrait.TraitType.Coprophiliac))
            CultFaithManager.AddThought(Thought.Cult_FollowerBecameIllSleepingNextToIllFollower_Coprophiliac, this.Info.ID);
          else
            CultFaithManager.AddThought(Thought.Cult_FollowerBecameIllSleepingNextToIllFollower, this.Info.ID);
        }
        else if (this.HasTrait(FollowerTrait.TraitType.Germophobe))
          CultFaithManager.AddThought(Thought.Cult_GermophobeBecameSick, this.Info.ID);
        else if (this.HasTrait(FollowerTrait.TraitType.Coprophiliac))
          CultFaithManager.AddThought(Thought.Cult_CoprophiliacBecameSick, this.Info.ID);
        else
          NotificationCentre.Instance.PlayFollowerNotification(NotificationCentre.NotificationType.BecomeIll, this.Info, NotificationFollower.Animation.Sick);
        this._directInfoAccess.CursedStateVariant = UnityEngine.Random.Range(0, 2);
        break;
      case Thought.BecomeStarving:
        this.AddThought(Thought.BecomeStarving, true);
        break;
      case Thought.OldAge:
        this.AddThought(Thought.OldAge, true);
        DataManager.Instance.Followers_Elderly_IDs.Add(this.Info.ID);
        if (DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.LoveElderly))
        {
          CultFaithManager.AddThought(Thought.Cult_LoveElderly_Trait, this.Info.ID);
          break;
        }
        CultFaithManager.AddThought(Thought.Cult_FollowerReachedOldAge, this.Info.ID);
        break;
      case Thought.Zombie:
        NotificationCentre.Instance.PlayFollowerNotification(NotificationCentre.NotificationType.ZombieSpawned, this.Info, NotificationFollower.Animation.Unhappy);
        break;
    }
    this.CheckChangeState();
    if (this.CurrentTask != null && this.CurrentTask.Type == FollowerTaskType.Imprisoned)
      return;
    this.CompleteCurrentTask();
  }

  public void RemoveCurseState(Thought Thought)
  {
    Thought cursedState = this.Info.CursedState;
    if (this.Info.CursedState == Thought)
      this.Info.CursedState = Thought.None;
    if (!this.ThoughtExists(Thought))
      return;
    switch (Thought)
    {
      case Thought.Dissenter:
        this.RemoveThought(Thought.Dissenter, true);
        this.AddThought(Thought.NoLongerDissenting);
        this.Stats.Reeducation = 0.0f;
        using (List<FollowerBrain>.Enumerator enumerator = FollowerBrain.AllBrains.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            FollowerBrain current = enumerator.Current;
            if (current != this)
              current.RemoveThought(Thought.SomeoneDissenterZealotTrait, false);
          }
          break;
        }
      case Thought.Ill:
        this.Stats.Illness = 0.0f;
        if (cursedState == Thought.Ill)
        {
          FollowerBrainStats.StatStateChangedEvent illnessStateChanged = FollowerBrainStats.OnIllnessStateChanged;
          if (illnessStateChanged != null)
            illnessStateChanged(this.Info.ID, FollowerStatState.Off, FollowerStatState.On);
        }
        this.CompleteCurrentTask();
        break;
    }
    if (!this.ThoughtExists(Thought.OldAge))
      return;
    this.HardSwapToTask((FollowerTask) new FollowerTask_OldAge());
  }

  private FollowerTask GetFallbackTask(ScheduledActivity activity)
  {
    if (this.CurrentTaskType == FollowerTaskType.ManualControl)
      return (FollowerTask) new FollowerTask_ManualControl();
    if (this.Info.CursedState != Thought.None && this._directInfoAccess.WorkThroughNight && activity == ScheduledActivity.Sleep)
      return this.GetPersonalTask(this.Location);
    if (activity == ScheduledActivity.Sleep && !FollowerBrainStats.IsHoliday && this._directInfoAccess.WorkThroughNight)
      activity = ScheduledActivity.Work;
    if (FollowerBrainStats.IsHoliday && activity != ScheduledActivity.Sleep)
      activity = ScheduledActivity.Leisure;
    switch (activity)
    {
      case ScheduledActivity.Work:
        Structures_Shrine structuresShrine1 = (Structures_Shrine) null;
        List<Structures_Shrine> structuresOfType1 = StructureManager.GetAllStructuresOfType<Structures_Shrine>(this.HomeLocation);
        if (structuresOfType1.Count > 0)
          structuresShrine1 = structuresOfType1[0];
        return structuresShrine1 != null && structuresShrine1.Prayers.Count < structuresShrine1.PrayersMax && structuresShrine1.SoulCount < structuresShrine1.SoulMax ? (FollowerTask) new FollowerTask_Pray(structuresShrine1.Data.ID) : (FollowerTask) new FollowerTask_FakeLeisure();
      case ScheduledActivity.Study:
        return (FollowerTask) new FollowerTask_Study(StructureManager.GetAllStructuresOfType<Structures_Temple>(this.HomeLocation)[0].Data.ID);
      case ScheduledActivity.Sleep:
        return this.Info.CursedState == Thought.Ill ? (FollowerTask) new FollowerTask_SleepBedRest() : (FollowerTask) new FollowerTask_Sleep();
      case ScheduledActivity.Pray:
        Structures_Shrine structuresShrine2 = (Structures_Shrine) null;
        List<Structures_Shrine> structuresOfType2 = StructureManager.GetAllStructuresOfType<Structures_Shrine>(this.HomeLocation);
        if (structuresOfType2.Count > 0)
          structuresShrine2 = structuresOfType2[0];
        return structuresShrine2 != null && structuresShrine2.Prayers.Count < structuresShrine2.PrayersMax && structuresShrine2.SoulCount < structuresShrine2.SoulMax ? (FollowerTask) new FollowerTask_Pray(structuresShrine2.Data.ID) : (FollowerTask) new FollowerTask_Idle();
      case ScheduledActivity.Leisure:
        return (FollowerTask) new FollowerTask_FakeLeisure();
      default:
        throw new ArgumentException($"Unrecognised ScheduledActivity.{activity}");
    }
  }

  public void NewRoleSet(FollowerRole followerRole)
  {
    int followerTaskFromRole = (int) FollowerTask.GetFollowerTaskFromRole(followerRole);
    List<FollowerTask> unoccupiedTasksOfType = FollowerBrain.GetAllUnoccupiedTasksOfType((FollowerTaskType) followerTaskFromRole);
    List<FollowerTask> occupiedTasksOfType = FollowerBrain.GetAllOccupiedTasksOfType((FollowerTaskType) followerTaskFromRole);
    this.Info.FollowerRole = followerRole;
    if (unoccupiedTasksOfType.Count > 0 || occupiedTasksOfType.Count <= 0)
      return;
    FollowerTask followerTask = occupiedTasksOfType[UnityEngine.Random.Range(0, occupiedTasksOfType.Count)];
    followerTask.Brain.Info.FollowerRole = FollowerRole.Worker;
    followerTask.Abort();
  }

  public void CancelTargetedMeal(StructureBrain.TYPES mealType)
  {
    int num1 = 0;
    foreach (StructureBrain structureBrain in StructureManager.GetAllStructuresOfType<Structures_Meal>())
    {
      if (structureBrain.Data.Type == mealType)
        ++num1;
    }
    foreach (StructureBrain structureBrain in StructureManager.GetAllStructuresOfType<Structures_FoodStorage>(this.Location))
    {
      foreach (InventoryItem inventoryItem in structureBrain.Data.Inventory)
      {
        if ((InventoryItem.ITEM_TYPE) inventoryItem.type == CookingData.GetMealFromStructureType(mealType))
          ++num1;
      }
    }
    int num2 = 0;
    List<FollowerTask> occupiedTasksOfType = FollowerBrain.GetAllOccupiedTasksOfType(FollowerTaskType.EatMeal);
    occupiedTasksOfType.AddRange((IEnumerable<FollowerTask>) FollowerBrain.GetAllOccupiedTasksOfType(FollowerTaskType.EatStoredFood));
    foreach (FollowerTask followerTask in occupiedTasksOfType)
    {
      if (followerTask.Type == FollowerTaskType.EatMeal && ((FollowerTask_EatMeal) followerTask).MealType == mealType || followerTask.Type == FollowerTaskType.EatStoredFood && ((FollowerTask_EatStoredFood) followerTask)._foodType == CookingData.GetMealFromStructureType(mealType))
        ++num2;
    }
    if (num2 < num1)
      return;
    for (int index = 0; index < occupiedTasksOfType.Count; ++index)
    {
      if ((occupiedTasksOfType[index].Type == FollowerTaskType.EatMeal || occupiedTasksOfType[index].Type == FollowerTaskType.EatStoredFood) && occupiedTasksOfType[index].State != FollowerTaskState.Doing)
      {
        FollowerBrain brain = occupiedTasksOfType[0].Brain;
        occupiedTasksOfType[0].Abort();
        FollowerTask_FakeLeisure nextTask = new FollowerTask_FakeLeisure();
        brain.HardSwapToTask((FollowerTask) nextTask);
        break;
      }
    }
  }

  public static List<FollowerTask> GetDesiredTask_Work(FollowerLocation location)
  {
    SortedList<float, FollowerTask> sortedTasks = new SortedList<float, FollowerTask>((IComparer<float>) new FollowerBrain.DuplicateKeyComparer<float>());
    foreach (StructureBrain structureBrain in StructureManager.StructuresAtLocation(location))
    {
      if (structureBrain is ITaskProvider)
        ((ITaskProvider) structureBrain).GetAvailableTasks(ScheduledActivity.Work, sortedTasks);
    }
    return new List<FollowerTask>((IEnumerable<FollowerTask>) sortedTasks.Values);
  }

  public static bool IsTaskAvailable(FollowerTaskType taskType)
  {
    List<FollowerTask> followerTaskList = new List<FollowerTask>((IEnumerable<FollowerTask>) FollowerBrain.GetDesiredTask_Work(FollowerLocation.Base));
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.CurrentTask != null)
        followerTaskList.Add(allBrain.CurrentTask);
    }
    foreach (FollowerTask followerTask in followerTaskList)
    {
      if (followerTask.Type == taskType)
        return true;
    }
    return false;
  }

  public static List<FollowerTask> GetAllUnoccupiedTasksOfType(FollowerTaskType taskType)
  {
    List<FollowerTask> followerTaskList = new List<FollowerTask>((IEnumerable<FollowerTask>) FollowerBrain.GetDesiredTask_Work(FollowerLocation.Base));
    List<FollowerTask> unoccupiedTasksOfType = new List<FollowerTask>();
    foreach (FollowerTask followerTask in followerTaskList)
    {
      if (followerTask.Type == taskType)
        unoccupiedTasksOfType.Add(followerTask);
    }
    return unoccupiedTasksOfType;
  }

  private static List<FollowerTask> GetAllOccupiedTasksOfType(FollowerTaskType taskType)
  {
    List<FollowerTask> followerTaskList = new List<FollowerTask>((IEnumerable<FollowerTask>) FollowerBrain.GetDesiredTask_Work(FollowerLocation.Base));
    List<FollowerTask> occupiedTasksOfType = new List<FollowerTask>();
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.CurrentTask != null)
        followerTaskList.Add(allBrain.CurrentTask);
    }
    foreach (FollowerTask followerTask in followerTaskList)
    {
      if (followerTask.Type == taskType)
        occupiedTasksOfType.Add(followerTask);
    }
    return occupiedTasksOfType;
  }

  private static List<FollowerTask> GetDesiredTask_Study(FollowerLocation location)
  {
    return new List<FollowerTask>();
  }

  private static List<FollowerTask> GetDesiredTask_Sleep() => new List<FollowerTask>();

  private static List<FollowerTask> GetDesiredTask_Pray() => new List<FollowerTask>();

  private static List<FollowerTask> GetDesiredTask_Leisure() => new List<FollowerTask>();

  private FollowerTask CheckEatTask(bool Force = false)
  {
    if (this.CurrentTaskType == FollowerTaskType.Sleep)
      return (FollowerTask) null;
    InventoryItem.ITEM_TYPE itemType = InventoryItem.ITEM_TYPE.NONE;
    List<Objectives_EatMeal> objectivesEatMealList = new List<Objectives_EatMeal>();
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      if (objective is Objectives_EatMeal)
      {
        FollowerInfo infoById = FollowerInfo.GetInfoByID(((Objectives_EatMeal) objective).TargetFollower);
        if ((infoById != null ? (infoById.Location == FollowerLocation.Base ? 1 : 0) : 0) != 0)
        {
          objectivesEatMealList.Add((Objectives_EatMeal) objective);
          if (((Objectives_EatMeal) objective).TargetFollower == this.Info.ID)
            itemType = CookingData.GetMealFromStructureType(((Objectives_EatMeal) objective).MealType);
        }
      }
    }
    bool flag1 = Force;
    if (!FollowerBrainStats.Fasting)
    {
      if ((double) this.Stats.Starvation > 0.0)
        flag1 = true;
      else if ((double) HungerBar.Count < (double) HungerBar.MAX_HUNGER - 2.0 || DataManager.Instance.MealsCooked <= 1)
      {
        int count = StructureManager.GetAllStructuresOfType<Structures_Meal>(this.Location).Count;
        foreach (StructureBrain structureBrain in StructureManager.GetAllStructuresOfType<Structures_FoodStorage>(this.Location))
        {
          foreach (InventoryItem inventoryItem in structureBrain.Data.Inventory)
          {
            if (inventoryItem.UnreservedQuantity > 0)
              count += inventoryItem.UnreservedQuantity;
          }
        }
        List<FollowerBrain> list = new List<FollowerBrain>((IEnumerable<FollowerBrain>) FollowerBrain.AllBrains).OrderByDescending<FollowerBrain, float>((Func<FollowerBrain, float>) (o => o._directInfoAccess.Starvation)).ThenBy<FollowerBrain, float>((Func<FollowerBrain, float>) (o => o._directInfoAccess.Satiation)).ToList<FollowerBrain>();
        for (int index = 0; index < list.Count; ++index)
        {
          if (list[index] == this && index < count)
            flag1 = true;
        }
      }
    }
    else if (this.Stats.IsStarving)
      flag1 = true;
    if (flag1 || itemType != InventoryItem.ITEM_TYPE.NONE)
    {
      List<Structures_Meal> structuresMealList = new List<Structures_Meal>();
      foreach (Structures_Meal structuresMeal in StructureManager.GetAllStructuresOfType<Structures_Meal>(this.Location))
      {
        if (!structuresMeal.ReservedForTask && !structuresMeal.Data.Rotten && !structuresMeal.Data.Burned && (this.CurrentOverrideStructureType == StructureBrain.TYPES.NONE || structuresMeal.Data.Type == this.CurrentOverrideStructureType) && (this.Info.CursedState != Thought.Zombie || structuresMeal.Data.Type == StructureBrain.TYPES.MEAL_FOLLOWER_MEAT))
        {
          if (itemType != InventoryItem.ITEM_TYPE.NONE && CookingData.GetMealFromStructureType(structuresMeal.Data.Type) != itemType)
          {
            structuresMealList.Add(structuresMeal);
          }
          else
          {
            bool flag2 = true;
            foreach (Objectives_EatMeal objectivesEatMeal in objectivesEatMealList)
            {
              if (objectivesEatMeal.MealType == structuresMeal.Data.Type && this.Info.ID != objectivesEatMeal.TargetFollower)
              {
                flag2 = false;
                break;
              }
            }
            if (flag2)
              return (FollowerTask) new FollowerTask_EatMeal(structuresMeal.Data.ID);
          }
        }
      }
      if (structuresMealList.Count > 0 & flag1)
        return (FollowerTask) new FollowerTask_EatMeal(structuresMealList[0].Data.ID);
      foreach (Structures_FoodStorage structuresFoodStorage in StructureManager.GetAllStructuresOfType<Structures_FoodStorage>(this.Location))
      {
        List<InventoryItem.ITEM_TYPE> itemTypeList = new List<InventoryItem.ITEM_TYPE>();
        foreach (InventoryItem inventoryItem in structuresFoodStorage.Data.Inventory)
        {
          if (inventoryItem.UnreservedQuantity > 0 && (this.CurrentOverrideStructureType == StructureBrain.TYPES.NONE || StructuresData.GetMealStructureType((InventoryItem.ITEM_TYPE) inventoryItem.type) == this.CurrentOverrideStructureType) && (this.Info.CursedState != Thought.Zombie || inventoryItem.type == 133))
          {
            if (itemType != InventoryItem.ITEM_TYPE.NONE && (InventoryItem.ITEM_TYPE) inventoryItem.type != itemType)
            {
              itemTypeList.Add((InventoryItem.ITEM_TYPE) inventoryItem.type);
            }
            else
            {
              bool flag3 = true;
              foreach (Objectives_EatMeal objectivesEatMeal in objectivesEatMealList)
              {
                if (objectivesEatMeal.MealType == CookingData.GetStructureFromMealType((InventoryItem.ITEM_TYPE) inventoryItem.type) && this.Info.ID != objectivesEatMeal.TargetFollower)
                {
                  flag3 = false;
                  break;
                }
              }
              if (flag3)
                return (FollowerTask) new FollowerTask_EatStoredFood(structuresFoodStorage.Data.ID, (InventoryItem.ITEM_TYPE) inventoryItem.type);
            }
          }
        }
        if (itemTypeList.Count > 0 & flag1)
          return (FollowerTask) new FollowerTask_EatStoredFood(structuresFoodStorage.Data.ID, itemTypeList[0]);
      }
      this.CurrentOverrideStructureType = StructureBrain.TYPES.NONE;
    }
    return (FollowerTask) null;
  }

  public bool HasHome => this._directInfoAccess.DwellingID != Dwelling.NO_HOME;

  public bool HomeIsCorrectLevel => this._directInfoAccess.DwellingLevel >= this.Info.XPLevel;

  public int HomeID => this._directInfoAccess.DwellingID;

  public void AssignDwelling(Dwelling.DwellingAndSlot d, int followerID, bool Claimed)
  {
    Structures_Bed structureById = StructureManager.GetStructureByID<Structures_Bed>(d.ID);
    if (structureById == null)
    {
      d = StructureManager.GetFreeDwellingAndSlot(this.Location, this._directInfoAccess);
      structureById = StructureManager.GetStructureByID<Structures_Bed>(d.ID);
    }
    this._directInfoAccess.PreviousDwellingID = this._directInfoAccess.DwellingID;
    this._directInfoAccess.DwellingID = d.ID;
    this._directInfoAccess.DwellingSlot = d.dwellingslot;
    this._directInfoAccess.DwellingLevel = d.dwellingLevel;
    structureById.Data.Claimed = Claimed;
    structureById.Data.FollowerID = followerID;
    if (Claimed)
    {
      FollowerBrain.DwellingAssignmentChanged dwellingAssigned = FollowerBrain.OnDwellingAssigned;
      if (dwellingAssigned == null)
        return;
      dwellingAssigned(this.Info.ID, d);
    }
    else
      FollowerBrain.OnDwellingAssignedAwaitClaim(this.Info.ID, d);
  }

  public void ClearDwelling()
  {
    Dwelling.DwellingAndSlot d = new Dwelling.DwellingAndSlot(this._directInfoAccess.DwellingID, this._directInfoAccess.DwellingSlot, this._directInfoAccess.DwellingLevel);
    Structures_Bed structureById = StructureManager.GetStructureByID<Structures_Bed>(this._directInfoAccess.DwellingID);
    if (structureById != null)
    {
      structureById.Data.FollowerID = -1;
      structureById.Data.Claimed = false;
    }
    this._directInfoAccess.PreviousDwellingID = this._directInfoAccess.DwellingID;
    this._directInfoAccess.DwellingID = Dwelling.NO_HOME;
    this._directInfoAccess.DwellingSlot = 0;
    this._directInfoAccess.DwellingLevel = 0;
    if (d.ID != Dwelling.NO_HOME)
    {
      FollowerBrain.DwellingAssignmentChanged onDwellingCleared = FollowerBrain.OnDwellingCleared;
      if (onDwellingCleared != null)
        onDwellingCleared(this.Info.ID, d);
    }
    if (this.CurrentTask == null || !(this.CurrentTask is FollowerTask_Sleep))
      return;
    this.CurrentTask.Abort();
  }

  public Structures_Bed GetAssignedDwellingStructure()
  {
    return StructureManager.GetStructureByID<Structures_Bed>(this._directInfoAccess.DwellingID);
  }

  public Dwelling.DwellingAndSlot GetDwellingAndSlot()
  {
    return this.HasHome && this._directInfoAccess != null ? new Dwelling.DwellingAndSlot(this._directInfoAccess.DwellingID, this._directInfoAccess.DwellingSlot, this._directInfoAccess.DwellingLevel) : (Dwelling.DwellingAndSlot) null;
  }

  public void SetNewHomeLocation(FollowerLocation location)
  {
    this._directInfoAccess.HomeLocation = location;
    if (!this.HasHome || this.GetAssignedDwellingStructure().Data.Location == this.HomeLocation)
      return;
    this.ClearDwelling();
  }

  public static List<FollowerBrain> AllAvailableFollowerBrains()
  {
    List<FollowerBrain> followerBrainList = new List<FollowerBrain>();
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (!FollowerManager.FollowerLocked(allBrain.Info.ID))
        followerBrainList.Add(allBrain);
    }
    return followerBrainList;
  }

  public static bool CanFollowerGiveQuest(FollowerInfo follower)
  {
    if (FollowerManager.FollowerLocked(follower.ID) || follower.CursedState != Thought.None)
      return false;
    FollowerBrain brain = FollowerBrain.GetOrCreateBrain(follower);
    if (brain != null && brain.CurrentTask != null && brain.CurrentTask.BlockTaskChanges || (double) brain.Stats.Adoration >= (double) brain.Stats.MAX_ADORATION)
      return false;
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      if (objective.Follower == follower.ID)
        return false;
    }
    foreach (ObjectivesData completedObjective in DataManager.Instance.CompletedObjectives)
    {
      if (completedObjective.Follower == follower.ID)
        return false;
    }
    return true;
  }

  public static bool CanContinueToGiveQuest(FollowerInfo follower)
  {
    if (FollowerManager.FollowerLocked(follower.ID) || follower.CursedState != Thought.None)
      return false;
    FollowerBrain brain = FollowerBrain.GetOrCreateBrain(follower);
    return brain == null || (double) brain.Stats.Adoration < (double) brain.Stats.MAX_ADORATION;
  }

  public static FollowerBrain RandomAvailableBrainNoCurseState()
  {
    List<FollowerBrain> followerBrainList = new List<FollowerBrain>();
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (!FollowerManager.FollowerLocked(allBrain.Info.ID) && allBrain.Info.CursedState == Thought.None && TimeManager.CurrentDay - allBrain._directInfoAccess.DayJoined >= 2)
        followerBrainList.Add(allBrain);
    }
    return followerBrainList.Count <= 0 ? (FollowerBrain) null : followerBrainList[UnityEngine.Random.Range(0, followerBrainList.Count)];
  }

  public static FollowerBrain ClosestAvailableBrainNoCurse()
  {
    List<FollowerBrain> followerBrainList = new List<FollowerBrain>();
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (!FollowerManager.FollowerLocked(allBrain.Info.ID) && allBrain.Info.CursedState == Thought.None && allBrain.CurrentTaskType != FollowerTaskType.GetPlayerAttention)
        followerBrainList.Add(allBrain);
    }
    float num1 = float.MaxValue;
    FollowerBrain followerBrain1 = (FollowerBrain) null;
    foreach (FollowerBrain followerBrain2 in followerBrainList)
    {
      float num2 = Vector3.Distance(followerBrain2.LastPosition, PlayerFarming.Instance.transform.position);
      if ((double) num2 < (double) num1)
      {
        num1 = num2;
        followerBrain1 = followerBrain2;
      }
    }
    return followerBrain1;
  }

  public static FollowerBrain AddBrain(FollowerInfo info)
  {
    FollowerBrain followerBrain = new FollowerBrain(info);
    FollowerBrain.AllBrains.Add(followerBrain);
    Action<int> onBrainAdded = FollowerBrain.OnBrainAdded;
    if (onBrainAdded != null)
      onBrainAdded(info.ID);
    return followerBrain;
  }

  public static void RemoveBrain(int ID)
  {
    for (int index = FollowerBrain.AllBrains.Count - 1; index >= 0; --index)
    {
      if (FollowerBrain.AllBrains[index].Info != null && FollowerBrain.AllBrains[index].Info.ID == ID)
      {
        FollowerBrain.AllBrains.RemoveAt(index);
        break;
      }
    }
    Structures_Missionary.RemoveFollower(ID);
    Structures_Prison.RemoveFollower(ID);
    Structures_Demon_Summoner.RemoveFollower(ID);
    Action<int> onBrainRemoved = FollowerBrain.OnBrainRemoved;
    if (onBrainRemoved == null)
      return;
    onBrainRemoved(ID);
  }

  public static int DiscipleCount()
  {
    int num = 0;
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.Stats.HasLevelledUp)
        ++num;
    }
    return num;
  }

  public static FollowerBrain FindBrainByID(int ID)
  {
    for (int index = 0; index < FollowerBrain.AllBrains.Count; ++index)
    {
      if (FollowerBrain.AllBrains[index].Info != null && FollowerBrain.AllBrains[index].Info.ID == ID)
        return FollowerBrain.AllBrains[index];
    }
    return (FollowerBrain) null;
  }

  public static bool FindBrainByLevelExists(int Level)
  {
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.Info.XPLevel >= Level)
        return true;
    }
    return false;
  }

  public static FollowerBrain GetOrCreateBrain(FollowerInfo info)
  {
    return (info != null ? FollowerBrain.FindBrainByID(info.ID) : (FollowerBrain) null) ?? FollowerBrain.AddBrain(info);
  }

  public static List<FollowerBrain> GetBrainsWithinRadius(Vector3 position, float radius)
  {
    List<FollowerBrain> brainsWithinRadius = new List<FollowerBrain>();
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if ((double) Vector3.Distance(allBrain.LastPosition, position) < (double) radius)
        brainsWithinRadius.Add(allBrain);
    }
    return brainsWithinRadius;
  }

  public enum AdorationActions
  {
    Sermon,
    BigGift,
    Gift,
    Necklace,
    Quest,
    Inspire,
    Bribe,
    Intimidate,
    ConfessionBooth,
    Bless,
    Ritual_AlmsToPoor,
    FaithEnforce,
    AscendedFollower_Lvl2,
    HappyFollowerNPC,
    FirePit,
    FightPitMercy,
    FightPitDeath,
    AscendedFollower_Lvl3,
    AscendedFollower_Lvl4,
    AscendedFollower_Lvl5,
    SmoochSpouse,
    AssignEnforcer,
    PetDog,
    BlessLvl1,
    SermonLvl1,
    BribeLvl1,
    InspireLvl1,
    IntimidateLvl1,
    BigGiftLvl1,
    GiftLvl1,
    NecklaceLvl1,
  }

  public struct PendingTaskData
  {
    public bool KeepExistingTask;
    public FollowerTask Task;
    public int ListIndex;
  }

  public delegate void DwellingAssignmentChanged(int followerID, Dwelling.DwellingAndSlot d);

  public class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable
  {
    public int Compare(TKey x, TKey y)
    {
      int num = x.CompareTo((object) y);
      return num == 0 ? -1 : -num;
    }
  }
}
