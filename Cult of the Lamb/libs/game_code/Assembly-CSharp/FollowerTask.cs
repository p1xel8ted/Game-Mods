// Decompiled with JetBrains decompiler
// Type: FollowerTask
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public abstract class FollowerTask
{
  public float _priority;
  [CompilerGenerated]
  public bool \u003CInitialized\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CAnimateOutFromLocation\u003Ek__BackingField = true;
  public FollowerBrain _brain;
  public FollowerTaskState _state;
  [CompilerGenerated]
  public FollowerTask_ChangeLocation \u003CChangeLocationTask\u003Ek__BackingField;
  public FollowerTask.FollowerTaskDelegate OnFollowerTaskStateChanged;
  public static int NumFollowerTaskTypes = 135;
  public static string[] FollowerTaskNames = new string[FollowerTask.NumFollowerTaskTypes];
  public Vector3? _currentDestination;

  public abstract FollowerTaskType Type { get; }

  public abstract FollowerLocation Location { get; }

  public virtual bool DisablePickUpInteraction => false;

  public virtual bool BlockTaskChanges => false;

  public virtual bool BlockSocial => false;

  public virtual bool BlockSermon => false;

  public virtual bool BlockReactTasks => false;

  public virtual int UsingStructureID => 0;

  public virtual bool ShouldSaveDestination => true;

  public virtual bool BlockThoughts => false;

  public virtual float Priorty => 0.0f;

  public virtual PriorityCategory GetPriorityCategory(
    FollowerRole FollowerRole,
    WorkerPriority WorkerPriority,
    FollowerBrain brain)
  {
    return PriorityCategory.Low;
  }

  public bool Initialized
  {
    get => this.\u003CInitialized\u003Ek__BackingField;
    set => this.\u003CInitialized\u003Ek__BackingField = value;
  }

  public bool AnimateOutFromLocation
  {
    get => this.\u003CAnimateOutFromLocation\u003Ek__BackingField;
    set => this.\u003CAnimateOutFromLocation\u003Ek__BackingField = value;
  }

  public FollowerTaskState State => this._state;

  public FollowerBrain Brain => this._brain;

  public FollowerTask_ChangeLocation ChangeLocationTask
  {
    get => this.\u003CChangeLocationTask\u003Ek__BackingField;
    set => this.\u003CChangeLocationTask\u003Ek__BackingField = value;
  }

  public static bool RequiredFollowerLevel(FollowerRole FollowerRole, FollowerTaskType Type)
  {
    return true;
  }

  static FollowerTask()
  {
    for (int index = 0; index < FollowerTask.NumFollowerTaskTypes; ++index)
      FollowerTask.FollowerTaskNames[index] = ((FollowerTaskType) index).ToString();
  }

  public int GetUniqueTaskCode() => (int) this.Type * 1000000 + this.GetSubTaskCode();

  public abstract int GetSubTaskCode();

  public int SubTaskCode => this.GetSubTaskCode();

  public virtual void ClaimReservations()
  {
  }

  public virtual void ReleaseReservations()
  {
  }

  public void Init(FollowerBrain brain)
  {
    this._brain = brain;
    if (this._brain.SavedFollowerTaskType == this.Type && this._brain.SavedFollowerTaskLocation == this.Location)
    {
      Vector3 followerTaskDestination = this._brain.SavedFollowerTaskDestination;
      this.ClearDestination();
      this._currentDestination = new Vector3?(followerTaskDestination);
    }
    this.Initialized = true;
    this.OnInitialized();
  }

  public virtual void OnInitialized()
  {
  }

  public void Start()
  {
    if (this._brain.Location == this.Location)
    {
      this.OnStart();
    }
    else
    {
      if (this._brain != null && !DataManager.Instance.Followers.Contains(this._brain._directInfoAccess))
        return;
      this.CleanupSetup();
      this.ChangeLocationTask = new FollowerTask_ChangeLocation(this.Location, this.Type, this.AnimateOutFromLocation);
      FollowerTask_ChangeLocation changeLocationTask = this.ChangeLocationTask;
      changeLocationTask.OnFollowerTaskStateChanged = changeLocationTask.OnFollowerTaskStateChanged + new FollowerTask.FollowerTaskDelegate(this.OnLocationTaskStateChanged);
      this.ChangeLocationTask.Init(this._brain);
      Action<FollowerTask, FollowerTask> onTaskChanged = this._brain.OnTaskChanged;
      if (onTaskChanged != null)
        onTaskChanged((FollowerTask) this.ChangeLocationTask, this);
      this.ChangeLocationTask.Start();
    }
  }

  public virtual void OnStart() => this.SetState(FollowerTaskState.Idle);

  public void StartAgain(Follower follower)
  {
    this.SetState(FollowerTaskState.Doing);
    this.OnDoingBegin(follower);
  }

  public virtual void OnNewPhaseStarted()
  {
  }

  public void OnLocationTaskStateChanged(FollowerTaskState oldState, FollowerTaskState newState)
  {
    if (newState != FollowerTaskState.Done)
      return;
    FollowerTask_ChangeLocation changeLocationTask1 = this.ChangeLocationTask;
    FollowerTask_ChangeLocation changeLocationTask2 = this.ChangeLocationTask;
    changeLocationTask2.OnFollowerTaskStateChanged = changeLocationTask2.OnFollowerTaskStateChanged - new FollowerTask.FollowerTaskDelegate(this.OnLocationTaskStateChanged);
    this.ChangeLocationTask = (FollowerTask_ChangeLocation) null;
    Action<FollowerTask, FollowerTask> onTaskChanged = this._brain.OnTaskChanged;
    if (onTaskChanged != null)
      onTaskChanged(this, (FollowerTask) changeLocationTask1);
    this.SetState(FollowerTaskState.WaitingForLocation);
  }

  public void Arrive()
  {
    if (this._currentDestination.HasValue)
      this._brain.LastPosition = this._currentDestination.Value;
    this.OnArrive();
  }

  public virtual void OnArrive() => this.SetState(FollowerTaskState.Doing);

  public void End()
  {
    if (this._brain.CurrentOverrideTaskType == this.Type)
      this._brain.ClearPersonalOverrideTaskProvider();
    this.OnEnd();
  }

  public virtual void OnEnd() => this.SetState(FollowerTaskState.Finalising);

  public void Complete()
  {
    this.OnComplete();
    this.ClearDestination();
    this.SetState(FollowerTaskState.Done);
    this._brain.ContinueToNextTask();
  }

  public virtual void OnComplete()
  {
  }

  public virtual void OnAbort()
  {
  }

  public void Abort()
  {
    this.OnAbort();
    this.ClearDestination();
    this.Complete();
  }

  public void Tick(float deltaGameTime)
  {
    if (this.State == FollowerTaskState.WaitingForLocation && this._brain.Location == this.Location)
      this.Start();
    if (!TimeManager.PauseGameTime)
    {
      this._brain.Stats.Rest += this.RestChange(deltaGameTime);
      this._brain.Stats.Satiation += this.SatiationChange(deltaGameTime) * DifficultyManager.GetHungerDepletionMultiplier();
      this._brain.Stats.Vomit += this.VomitChange(deltaGameTime);
      this._brain.Stats.Social += this.SocialChange(deltaGameTime);
      if ((double) this._brain.Stats.Illness > 0.0)
        this._brain.Stats.Illness += this.IllnessChange(deltaGameTime) * DifficultyManager.GetIllnessDepletionMultiplier();
      if ((double) this._brain.Stats.Exhaustion > 0.0)
        this._brain.Stats.Exhaustion += this.ExhaustionChange(deltaGameTime);
      if ((double) this._brain.Stats.Drunk > 0.0)
        this._brain.Stats.Drunk += this.DrunkChange(deltaGameTime);
      if ((double) this._brain.Stats.Injured > 0.0)
        this._brain.Stats.Injured += this.InjuredChange(deltaGameTime);
      if ((double) this._brain.Stats.Freezing > 0.0)
        this._brain.Stats.Freezing += this.FreezingChange(deltaGameTime) * DifficultyManager.GetFreezingDepletionMultiplier();
      if ((double) this._brain.Stats.Soaking > 0.0)
        this._brain.Stats.Soaking += this.SoakingChange(deltaGameTime);
      if ((double) this._brain.Stats.Overheating > 0.0)
        this._brain.Stats.Overheating += this.OverheatingChange(deltaGameTime);
      if ((double) this._brain.Stats.Aflame > 0.0)
        this._brain.Stats.Aflame += this.AflameChange(deltaGameTime);
      this._brain.Stats.Reeducation += this.ReeducationChange(deltaGameTime) * DifficultyManager.GetDissenterDepletionMultiplier();
    }
    else if ((double) this.SatiationChange(deltaGameTime) > 0.0)
      this._brain.Stats.Satiation += this.SatiationChange(deltaGameTime) * DifficultyManager.GetHungerDepletionMultiplier();
    this.TaskTick(deltaGameTime);
  }

  public void SetState(FollowerTaskState state)
  {
    if (this._state == state)
      return;
    FollowerTaskState state1 = this._state;
    this._state = state;
    FollowerTask.FollowerTaskDelegate taskStateChanged = this.OnFollowerTaskStateChanged;
    if (taskStateChanged == null)
      return;
    taskStateChanged(state1, this._state);
  }

  public virtual float RestChange(float deltaGameTime)
  {
    return (float) -(100.0 * ((double) deltaGameTime / 1200.0));
  }

  public virtual float VomitChange(float deltaGameTime)
  {
    return (double) this._brain.Stats.Illness > 0.0 ? (float) -(30.0 * ((double) deltaGameTime / 320.0)) : 0.0f;
  }

  public virtual float SocialChange(float deltaGameTime)
  {
    return (float) -(100.0 * ((double) deltaGameTime / 200.0));
  }

  public virtual float FreezingChange(float deltaGameTime)
  {
    return this._brain.HasTrait(FollowerTrait.TraitType.FreezeImmune) || SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter ? 0.0f : (float) (100.0 * ((double) deltaGameTime / 1800.0));
  }

  public virtual float SoakingChange(float deltaGameTime)
  {
    return (float) (100.0 * ((double) deltaGameTime / 240.0));
  }

  public virtual float OverheatingChange(float deltaGameTime)
  {
    return (float) (100.0 * ((double) deltaGameTime / 1200.0));
  }

  public virtual float AflameChange(float deltaGameTime)
  {
    return (float) (100.0 * ((double) deltaGameTime / 240.0));
  }

  public virtual float IllnessChange(float deltaGameTime)
  {
    return (float) (100.0 * ((double) deltaGameTime / 3600.0));
  }

  public virtual float ExhaustionChange(float deltaGameTime)
  {
    return (float) (100.0 * ((double) deltaGameTime / 900.0));
  }

  public virtual float DrunkChange(float deltaGameTime)
  {
    return (float) -(100.0 * ((double) deltaGameTime / 480.0));
  }

  public virtual float InjuredChange(float deltaGameTime)
  {
    return (float) -(100.0 * ((double) deltaGameTime / 2400.0));
  }

  public virtual float ReeducationChange(float deltaGameTime)
  {
    return (float) (100.0 * ((double) deltaGameTime / 3600.0) * (1.0 * (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null) || PlayerFarming.Location == FollowerLocation.Base ? 1.0 : 0.699999988079071)));
  }

  public virtual float SatiationChange(float deltaGameTime)
  {
    return this._brain.Info.CursedState != Thought.None && this._brain.Info.CursedState != Thought.BecomeStarving || FollowerBrainStats.Fasting ? 0.0f : (float) -(100.0 * ((double) deltaGameTime / 2400.0) * (1.0 * (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null) || PlayerFarming.Location == FollowerLocation.Base ? 1.0 : 0.699999988079071)));
  }

  public abstract void TaskTick(float deltaGameTime);

  public virtual void ProgressTask()
  {
  }

  public void RecalculateDestination()
  {
    this.ClearDestination();
    if (this.State != FollowerTaskState.None && this.State != FollowerTaskState.GoingTo)
    {
      this.SetState(FollowerTaskState.GoingTo);
    }
    else
    {
      FollowerTask.FollowerTaskDelegate taskStateChanged = this.OnFollowerTaskStateChanged;
      if (taskStateChanged == null)
        return;
      taskStateChanged(FollowerTaskState.None, FollowerTaskState.GoingTo);
    }
  }

  public bool IsNaN(Vector3 check)
  {
    return float.IsNaN(this._currentDestination.Value.magnitude) || float.IsNaN(this._currentDestination.Value.x) || float.IsNaN(this._currentDestination.Value.y) || float.IsNaN(this._currentDestination.Value.z);
  }

  public Vector3 GetDestination(Follower follower)
  {
    if (this._currentDestination.HasValue && this.IsNaN(this._currentDestination.Value))
      this.ClearDestination();
    if (this._currentDestination.HasValue)
    {
      Vector3? currentDestination = this._currentDestination;
      Vector3 zero = Vector3.zero;
      if ((currentDestination.HasValue ? (currentDestination.HasValue ? (currentDestination.GetValueOrDefault() == zero ? 1 : 0) : 1) : 0) == 0)
        goto label_10;
    }
    this._currentDestination = new Vector3?(this.UpdateDestination(follower));
    if (this.IsNaN(this._currentDestination.Value))
    {
      this.ClearDestination();
      return !((UnityEngine.Object) follower == (UnityEngine.Object) null) ? follower.transform.position : Vector3.zero;
    }
    if (this.ShouldSaveDestination)
    {
      this._brain.SavedFollowerTaskType = this.Type;
      this._brain.SavedFollowerTaskLocation = this.Location;
      this._brain.SavedFollowerTaskDestination = this._currentDestination.Value;
    }
label_10:
    return this._currentDestination.Value;
  }

  public void ClearDestination()
  {
    this._currentDestination = new Vector3?();
    this._brain.SavedFollowerTaskType = FollowerTaskType.None;
    this._brain.SavedFollowerTaskLocation = FollowerLocation.None;
    this._brain.SavedFollowerTaskDestination = Vector3.zero;
  }

  public float GetDistanceToDestination()
  {
    float distanceToDestination = 0.0f;
    if (this._currentDestination.HasValue)
      distanceToDestination = Vector3.Distance(this._brain.LastPosition, this._currentDestination.Value);
    return distanceToDestination;
  }

  public abstract Vector3 UpdateDestination(Follower follower);

  public virtual void Setup(Follower follower)
  {
  }

  public virtual void CleanupSetup()
  {
  }

  public virtual void OnIdleBegin(Follower follower)
  {
    follower.ClearPath();
    follower.State.CURRENT_STATE = StateMachine.State.Idle;
  }

  public virtual void OnIdleEnd(Follower follower)
  {
  }

  public virtual void OnGoingToBegin(Follower follower)
  {
    Vector3 destination = this.GetDestination(follower);
    if ((double) Vector3.Distance(follower.transform.position, destination) > 0.30000001192092896)
    {
      follower.GoTo(destination, (System.Action) (() => this.Arrive()));
    }
    else
    {
      follower.ClearPath();
      this.Arrive();
    }
  }

  public virtual void OnGoingToEnd(Follower follower)
  {
  }

  public virtual void OnDoingBegin(Follower follower)
  {
  }

  public virtual void OnDoingEnd(Follower follower)
  {
  }

  public virtual void OnFinaliseBegin(Follower follower) => this.Complete();

  public virtual void OnFinaliseEnd(Follower follower)
  {
  }

  public virtual void Cleanup(Follower follower) => this.ReleaseReservations();

  public virtual void SimSetup(SimFollower simFollower)
  {
  }

  public virtual void SimIdleBegin(SimFollower simFollower)
  {
  }

  public virtual void SimIdleEnd(SimFollower simFollower)
  {
  }

  public virtual void SimGoingToBegin(SimFollower simFollower)
  {
    simFollower.TravelTimeGameMinutes = 10f;
  }

  public virtual void SimGoingToEnd(SimFollower simFollower)
  {
  }

  public virtual void SimDoingBegin(SimFollower simFollower)
  {
  }

  public virtual void SimDoingEnd(SimFollower simFollower)
  {
  }

  public virtual void SimFinaliseBegin(SimFollower simFollower) => this.Complete();

  public virtual void SimFinaliseEnd(SimFollower simFollower)
  {
  }

  public virtual void SimCleanup(SimFollower simFollower) => this.ReleaseReservations();

  public virtual string ToDebugString() => $"{this.Type}, State.{this._state}";

  public static FollowerTaskType GetFollowerTaskFromRole(FollowerRole role)
  {
    switch (role)
    {
      case FollowerRole.Worshipper:
        return FollowerTaskType.Pray;
      case FollowerRole.Lumberjack:
        return FollowerTaskType.ChopTrees;
      case FollowerRole.Farmer:
        return FollowerTaskType.Farm;
      case FollowerRole.StoneMiner:
        return FollowerTaskType.ClearRubble;
      case FollowerRole.Builder:
        return FollowerTaskType.Build;
      case FollowerRole.Forager:
        return FollowerTaskType.Forage;
      case FollowerRole.Chef:
        return FollowerTaskType.Cook;
      case FollowerRole.Janitor:
        return FollowerTaskType.Janitor;
      case FollowerRole.Refiner:
        return FollowerTaskType.Refinery;
      case FollowerRole.Undertaker:
        return FollowerTaskType.Undertaker;
      case FollowerRole.Bartender:
        return FollowerTaskType.Brew;
      case FollowerRole.Medic:
        return FollowerTaskType.Medic;
      case FollowerRole.Rancher:
        return FollowerTaskType.Rancher;
      case FollowerRole.Logistics:
        return FollowerTaskType.Logistics;
      case FollowerRole.Handyman:
        return FollowerTaskType.Handyman;
      case FollowerRole.TraitManipulator:
        return FollowerTaskType.TraitManipulator;
      case FollowerRole.RotstoneMiner:
        return FollowerTaskType.MineRotstone;
      default:
        return FollowerTaskType.None;
    }
  }

  public Vector3 GetCirclePosition(
    FollowerBrain brain,
    Vector3 center,
    float maxDistance,
    List<FollowerBrain> followers)
  {
    int index = followers.IndexOf(brain);
    return this.GetCirclePosition(center, maxDistance, index, followers.Count);
  }

  public Vector3 GetCirclePosition(Vector3 center, float maxDistance, int index, int count)
  {
    if (count <= 12)
    {
      float num = maxDistance;
      float f = (float) ((double) index * (360.0 / (double) count) * (Math.PI / 180.0));
      return center + new Vector3(num * Mathf.Cos(f), num * Mathf.Sin(f));
    }
    int b = 8;
    float num1;
    float f1;
    if (index < b)
    {
      num1 = maxDistance;
      f1 = (float) ((double) index * (360.0 / (double) Mathf.Min(count, b)) * (Math.PI / 180.0));
    }
    else
    {
      num1 = maxDistance + 1f;
      f1 = (float) ((double) (index - b) * (360.0 / (double) (count - b)) * (Math.PI / 180.0));
    }
    return center + new Vector3(num1 * Mathf.Cos(f1), num1 * Mathf.Sin(f1));
  }

  [CompilerGenerated]
  public void \u003COnGoingToBegin\u003Eb__98_0() => this.Arrive();

  public delegate void FollowerTaskDelegate(FollowerTaskState oldState, FollowerTaskState newState);
}
