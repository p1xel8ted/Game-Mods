// Decompiled with JetBrains decompiler
// Type: FollowerTask
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public abstract class FollowerTask
{
  public float _priority;
  protected FollowerBrain _brain;
  protected FollowerTaskState _state;
  public FollowerTask.FollowerTaskDelegate OnFollowerTaskStateChanged;
  protected Vector3? _currentDestination;

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

  public bool Initialized { get; private set; }

  public bool AnimateOutFromLocation { get; set; } = true;

  public FollowerTaskState State => this._state;

  public FollowerBrain Brain => this._brain;

  public FollowerTask_ChangeLocation ChangeLocationTask { get; private set; }

  public static bool RequiredFollowerLevel(FollowerRole FollowerRole, FollowerTaskType Type)
  {
    return true;
  }

  public int GetUniqueTaskCode() => (int) this.Type * 1000000 + this.GetSubTaskCode();

  protected abstract int GetSubTaskCode();

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
  }

  public void Start()
  {
    if (this._brain.Location == this.Location)
    {
      this.OnStart();
    }
    else
    {
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

  protected virtual void OnStart() => this.SetState(FollowerTaskState.Idle);

  public void StartAgain(Follower follower)
  {
    this.SetState(FollowerTaskState.Doing);
    this.OnDoingBegin(follower);
  }

  private void OnLocationTaskStateChanged(FollowerTaskState oldState, FollowerTaskState newState)
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

  protected virtual void OnArrive() => this.SetState(FollowerTaskState.Doing);

  public void End()
  {
    if (this._brain.CurrentOverrideTaskType == this.Type)
      this._brain.ClearPersonalOverrideTaskProvider();
    this.OnEnd();
  }

  protected virtual void OnEnd() => this.SetState(FollowerTaskState.Finalising);

  protected void Complete()
  {
    this.OnComplete();
    this.ClearDestination();
    this.SetState(FollowerTaskState.Done);
    this._brain.ContinueToNextTask();
  }

  protected virtual void OnComplete()
  {
  }

  protected virtual void OnAbort()
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
      this._brain.Stats.Reeducation += this.ReeducationChange(deltaGameTime) * DifficultyManager.GetDissenterDepletionMultiplier();
    }
    else if ((double) this.SatiationChange(deltaGameTime) > 0.0)
      this._brain.Stats.Satiation += this.SatiationChange(deltaGameTime) * DifficultyManager.GetHungerDepletionMultiplier();
    this.TaskTick(deltaGameTime);
  }

  protected void SetState(FollowerTaskState state)
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

  protected virtual float RestChange(float deltaGameTime)
  {
    return (float) -(100.0 * ((double) deltaGameTime / 1200.0));
  }

  protected virtual float VomitChange(float deltaGameTime)
  {
    return (double) this._brain.Stats.Illness > 0.0 ? (float) -(30.0 * ((double) deltaGameTime / 320.0)) : 0.0f;
  }

  protected virtual float SocialChange(float deltaGameTime)
  {
    return (float) -(100.0 * ((double) deltaGameTime / 200.0));
  }

  protected virtual float IllnessChange(float deltaGameTime)
  {
    return (float) (100.0 * ((double) deltaGameTime / 3600.0));
  }

  protected virtual float ExhaustionChange(float deltaGameTime)
  {
    return (float) (100.0 * ((double) deltaGameTime / 900.0));
  }

  protected virtual float ReeducationChange(float deltaGameTime)
  {
    return (float) (100.0 * ((double) deltaGameTime / 3600.0) * (1.0 * (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null) || PlayerFarming.Location == FollowerLocation.Base ? 1.0 : 0.699999988079071)));
  }

  protected virtual float SatiationChange(float deltaGameTime)
  {
    return this._brain.Info.CursedState != Thought.None && this._brain.Info.CursedState != Thought.BecomeStarving || FollowerBrainStats.Fasting ? 0.0f : (float) -(100.0 * ((double) deltaGameTime / 2400.0) * (1.0 * (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null) || PlayerFarming.Location == FollowerLocation.Base ? 1.0 : 0.699999988079071)));
  }

  protected abstract void TaskTick(float deltaGameTime);

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

  private bool IsNaN(Vector3 check)
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

  protected abstract Vector3 UpdateDestination(Follower follower);

  public virtual void Setup(Follower follower)
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
      default:
        return FollowerTaskType.None;
    }
  }

  public delegate void FollowerTaskDelegate(FollowerTaskState oldState, FollowerTaskState newState);
}
