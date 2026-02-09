// Decompiled with JetBrains decompiler
// Type: FollowerTask_Fisherman
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_Fisherman : FollowerTask
{
  public const int FISH_DURATION_GAME_MINUTES = 40;
  public int _fishingHutID;
  public Structures_FishingHut _fishingHut;
  public float _progress;

  public override FollowerTaskType Type => FollowerTaskType.Fisherman;

  public override FollowerLocation Location => this._fishingHut.Data.Location;

  public override int UsingStructureID => this._fishingHutID;

  public override float Priorty => 20f;

  public override PriorityCategory GetPriorityCategory(
    FollowerRole FollowerRole,
    WorkerPriority WorkerPriority,
    FollowerBrain brain)
  {
    switch (FollowerRole)
    {
      case FollowerRole.Worshipper:
      case FollowerRole.Lumberjack:
      case FollowerRole.Farmer:
      case FollowerRole.Monk:
        return PriorityCategory.Low;
      case FollowerRole.Worker:
        return PriorityCategory.WorkPriority;
      default:
        return PriorityCategory.Low;
    }
  }

  public FollowerTask_Fisherman(int fishingHutID)
  {
    this._fishingHutID = fishingHutID;
    this._fishingHut = StructureManager.GetStructureByID<Structures_FishingHut>(this._fishingHutID);
  }

  public override int GetSubTaskCode() => this._fishingHutID;

  public override void ClaimReservations()
  {
    StructureManager.GetStructureByID<Structures_FishingHut>(this._fishingHutID).ReservedForTask = true;
  }

  public override void ReleaseReservations()
  {
    StructureManager.GetStructureByID<Structures_FishingHut>(this._fishingHutID).ReservedForTask = false;
  }

  public override void OnStart()
  {
    TimeManager.OnNewPhaseStarted += new System.Action(this.OnNewPhaseStarted);
    this.SetState(FollowerTaskState.GoingTo);
  }

  public override void OnArrive() => this.SetState(FollowerTaskState.Idle);

  public override void TaskTick(float deltaGameTime)
  {
    if (this.State != FollowerTaskState.Idle && this.State != FollowerTaskState.Doing)
      return;
    this._progress += deltaGameTime * this._brain.Info.ProductivityMultiplier;
    if ((double) this._progress < 40.0)
      return;
    this._progress = 0.0f;
    Structures_FishingHut structureById = StructureManager.GetStructureByID<Structures_FishingHut>(this._fishingHutID);
    if (structureById.Data.Inventory.Count < 5)
    {
      structureById.Data.Inventory.Add(new InventoryItem(InventoryItem.ITEM_TYPE.FISH));
      if (this._brain.ThoughtExists(Thought.FishingRitual))
        structureById.Data.Inventory.Add(new InventoryItem(InventoryItem.ITEM_TYPE.FISH));
      Debug.Log((object) $"{this._brain.Info.Name} collected fish, cached now = {structureById.Data.Inventory.Count}");
    }
    this.SetState(FollowerTaskState.Doing);
  }

  public new void OnNewPhaseStarted() => this.End();

  public override Vector3 UpdateDestination(Follower follower)
  {
    FishingHut fishingHut = this.FindFishingHut();
    return !((UnityEngine.Object) fishingHut == (UnityEngine.Object) null) ? fishingHut.FollowerPosition.transform.position : follower.transform.position;
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Fishing/fishing");
  }

  public override void OnIdleBegin(Follower follower)
  {
    base.OnIdleBegin(follower);
    follower.TimedAnimation("Fishing/fishing-start", 1.83333337f, (System.Action) (() => follower.State.CURRENT_STATE = StateMachine.State.Idle));
  }

  public override void OnDoingBegin(Follower follower)
  {
    string str = new string[3]{ "small", "medium", "big" }[UnityEngine.Random.Range(0, 3)];
    follower.TimedAnimation("Fishing/fishing-catch-" + str, 2.33333325f, (System.Action) (() =>
    {
      follower.State.CURRENT_STATE = StateMachine.State.Idle;
      this.SetState(FollowerTaskState.Idle);
    }));
  }

  public FishingHut FindFishingHut()
  {
    FishingHut fishingHut1 = (FishingHut) null;
    foreach (FishingHut fishingHut2 in FishingHut.FishingHuts)
    {
      if (fishingHut2.StructureInfo.ID == this._fishingHutID)
      {
        fishingHut1 = fishingHut2;
        break;
      }
    }
    return fishingHut1;
  }

  public override void SimDoingBegin(SimFollower simFollower)
  {
    this.SetState(FollowerTaskState.Idle);
  }
}
