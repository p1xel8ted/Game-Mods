// Decompiled with JetBrains decompiler
// Type: FollowerTask_ResourceStation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine;
using UnityEngine;

#nullable disable
public class FollowerTask_ResourceStation : FollowerTask
{
  public const float MAX_TREE_DISTANCE = 30f;
  private int _resourceStationID;
  private Structures_LumberjackStation _resourceStation;
  private bool CarryingResource;

  public override FollowerTaskType Type
  {
    get
    {
      return this._resourceStation.Data.LootToDrop != InventoryItem.ITEM_TYPE.LOG ? FollowerTaskType.ClearRubble : FollowerTaskType.ChopTrees;
    }
  }

  public override FollowerLocation Location => this._resourceStation.Data.Location;

  public override int UsingStructureID => this._resourceStationID;

  public override bool BlockSocial => true;

  public override float Priorty => 20f;

  public override PriorityCategory GetPriorityCategory(
    FollowerRole FollowerRole,
    WorkerPriority WorkerPriority,
    FollowerBrain brain)
  {
    switch (this._resourceStation.Data.LootToDrop)
    {
      case InventoryItem.ITEM_TYPE.LOG:
        if (FollowerRole == FollowerRole.Lumberjack)
          return PriorityCategory.WorkPriority;
        break;
      case InventoryItem.ITEM_TYPE.STONE:
        if (FollowerRole == FollowerRole.StoneMiner)
          return PriorityCategory.WorkPriority;
        break;
    }
    return PriorityCategory.Low;
  }

  public FollowerTask_ResourceStation(int resourceStationID)
  {
    this._resourceStationID = resourceStationID;
    this._resourceStation = StructureManager.GetStructureByID<Structures_LumberjackStation>(this._resourceStationID);
  }

  protected override int GetSubTaskCode() => this._resourceStationID;

  public override void ClaimReservations()
  {
    Structures_LumberjackStation structureById = StructureManager.GetStructureByID<Structures_LumberjackStation>(this._resourceStationID);
    if (structureById == null)
      return;
    structureById.ReservedForTask = true;
  }

  public override void ReleaseReservations()
  {
    Structures_LumberjackStation structureById = StructureManager.GetStructureByID<Structures_LumberjackStation>(this._resourceStationID);
    if (structureById == null)
      return;
    structureById.ReservedForTask = false;
  }

  protected override void OnStart()
  {
    TimeManager.OnNewPhaseStarted += new System.Action(this.OnNewPhaseStarted);
    this.SetState(FollowerTaskState.GoingTo);
  }

  protected override void OnArrive()
  {
    if (this.CarryingResource)
    {
      Follower follower = FollowerManager.FindFollowerByID(this._brain.Info.ID);
      if ((UnityEngine.Object) follower != (UnityEngine.Object) null)
      {
        string animation = "Buildings/add-wood";
        switch (this._resourceStation.Data.LootToDrop)
        {
          case InventoryItem.ITEM_TYPE.LOG:
            animation = "Buildings/add-wood";
            break;
          case InventoryItem.ITEM_TYPE.STONE:
            animation = "Buildings/add-stone";
            break;
        }
        follower.TimedAnimation(animation, 0.9166667f, (System.Action) (() =>
        {
          follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");
          this.CarryingResource = false;
          this.DepositResource();
          this.RecalculateDestination();
          this.SetState(FollowerTaskState.GoingTo);
        }));
      }
      else
      {
        this.CarryingResource = false;
        this.DepositResource();
        this.RecalculateDestination();
        this.SetState(FollowerTaskState.GoingTo);
      }
    }
    else
      this.SetState(FollowerTaskState.Idle);
  }

  protected override void TaskTick(float deltaGameTime)
  {
    if (this._resourceStation.Data.Exhausted && !this.CarryingResource)
      this.End();
    if (this.State != FollowerTaskState.Idle && this.State != FollowerTaskState.Doing)
      return;
    this._resourceStation.Data.Progress += deltaGameTime * this._brain.Info.ProductivityMultiplier;
    if ((double) this._resourceStation.Data.Progress < (double) this._resourceStation.DURATION_GAME_MINUTES)
      return;
    this._resourceStation.Data.Progress = 0.0f;
    if (this._resourceStation.Data.Inventory.Count >= this._resourceStation.ResourceMax)
      return;
    this.CarryingResource = true;
    Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    if ((UnityEngine.Object) followerById != (UnityEngine.Object) null)
    {
      switch (this._resourceStation.Data.LootToDrop)
      {
        case InventoryItem.ITEM_TYPE.LOG:
          followerById.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Buildings/run-wood");
          break;
        case InventoryItem.ITEM_TYPE.STONE:
          followerById.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Buildings/run-stone");
          break;
      }
    }
    this.RecalculateDestination();
    this.SetState(FollowerTaskState.GoingTo);
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    follower.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
    follower.SimpleAnimator.ResetAnimationsToDefaults();
    follower.SetHat(HatType.None);
  }

  private void DepositResource()
  {
    this._resourceStation.Data.Inventory.Add(new InventoryItem(this._resourceStation.Data.LootToDrop));
    LumberjackStation resourceStation = this.FindResourceStation();
    if ((UnityEngine.Object) resourceStation != (UnityEngine.Object) null)
      resourceStation.DepositItem();
    this._resourceStation.IncreaseAge();
    if (this._resourceStation.Data.Inventory.Count < this._resourceStation.ResourceMax)
      return;
    this.End();
  }

  private void OnNewPhaseStarted() => this.End();

  protected override Vector3 UpdateDestination(Follower follower)
  {
    LumberjackStation resourceStation = this.FindResourceStation();
    if ((UnityEngine.Object) resourceStation == (UnityEngine.Object) null)
      return this._resourceStation.Data.Position;
    return !this.CarryingResource ? resourceStation.FollowerPosition.transform.position : resourceStation.ChestPosition.transform.position;
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    follower.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
    switch (this._resourceStation.Data.LootToDrop)
    {
      case InventoryItem.ITEM_TYPE.LOG:
        follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "chop-wood");
        break;
      case InventoryItem.ITEM_TYPE.STONE:
        follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "mining");
        break;
    }
  }

  public override void OnDoingBegin(Follower follower)
  {
    base.OnDoingBegin(follower);
    switch (this._resourceStation.Data.LootToDrop)
    {
      case InventoryItem.ITEM_TYPE.LOG:
        follower.SetHat(HatType.Lumberjack);
        break;
      case InventoryItem.ITEM_TYPE.STONE:
        follower.SetHat(HatType.Miner);
        break;
    }
  }

  public override void SimDoingBegin(SimFollower simFollower)
  {
    this.SetState(FollowerTaskState.Idle);
  }

  private LumberjackStation FindResourceStation()
  {
    LumberjackStation resourceStation = (LumberjackStation) null;
    foreach (LumberjackStation lumberjackStation in LumberjackStation.LumberjackStations)
    {
      if ((UnityEngine.Object) lumberjackStation != (UnityEngine.Object) null && lumberjackStation.StructureInfo != null && lumberjackStation.StructureInfo.ID == this._resourceStationID)
      {
        resourceStation = lumberjackStation;
        break;
      }
    }
    return resourceStation;
  }

  private void HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
  {
    Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    if ((UnityEngine.Object) followerById == (UnityEngine.Object) null || !(e.Data.Name == "Chop"))
      return;
    switch (this._resourceStation.Data.LootToDrop)
    {
      case InventoryItem.ITEM_TYPE.LOG:
        AudioManager.Instance.PlayOneShot("event:/material/tree_chop", followerById.transform.position);
        break;
      case InventoryItem.ITEM_TYPE.STONE:
        AudioManager.Instance.PlayOneShot("event:/material/stone_impact", followerById.transform.position);
        break;
    }
  }

  public override void OnIdleBegin(Follower follower)
  {
    base.OnIdleBegin(follower);
    follower.SetHat(this._resourceStation.Data.LootToDrop == InventoryItem.ITEM_TYPE.LOG ? HatType.Lumberjack : HatType.Miner);
  }
}
