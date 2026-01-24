// Decompiled with JetBrains decompiler
// Type: FollowerTask_OfferingShrine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_OfferingShrine : FollowerTask
{
  public Structures_OfferingShrine OfferingShrineStructure;
  public int refineryID;

  public override FollowerTaskType Type => FollowerTaskType.PassivePray;

  public override FollowerLocation Location => this.OfferingShrineStructure.Data.Location;

  public override PriorityCategory GetPriorityCategory(
    FollowerRole FollowerRole,
    WorkerPriority WorkerPriority,
    FollowerBrain brain)
  {
    switch (FollowerRole)
    {
      case FollowerRole.Worshipper:
      case FollowerRole.Lumberjack:
      case FollowerRole.Monk:
        return PriorityCategory.Low;
      case FollowerRole.Worker:
        return PriorityCategory.WorkPriority;
      default:
        return PriorityCategory.Low;
    }
  }

  public override bool BlockReactTasks => true;

  public FollowerTask_OfferingShrine(int refineryID)
  {
    this.refineryID = refineryID;
    this.OfferingShrineStructure = StructureManager.GetStructureByID<Structures_OfferingShrine>(refineryID);
  }

  public override int GetSubTaskCode() => this.refineryID;

  public override void ClaimReservations()
  {
    Structures_OfferingShrine structureById = StructureManager.GetStructureByID<Structures_OfferingShrine>(this.refineryID);
    if (structureById == null)
      return;
    structureById.ReservedForTask = true;
  }

  public override void ReleaseReservations()
  {
    Structures_OfferingShrine structureById = StructureManager.GetStructureByID<Structures_OfferingShrine>(this.refineryID);
    if (structureById == null)
      return;
    structureById.ReservedForTask = false;
  }

  public override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  public override void TaskTick(float deltaGameTime)
  {
    if (this.State != FollowerTaskState.Doing)
      return;
    if (this.OfferingShrineStructure.Data.Inventory.Count <= 0)
    {
      this.OfferingShrineStructure.Data.Progress += deltaGameTime * this._brain.Info.ProductivityMultiplier;
      if ((double) this.OfferingShrineStructure.Data.Progress <= 30.0)
        return;
      Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
      this.OfferingShrineStructure.Complete((Object) followerById == (Object) null ? Vector3.zero : followerById.transform.position);
      if (this.OfferingShrineStructure.Data.Inventory.Count <= 0)
        return;
      this.Complete();
    }
    else
      this.Complete();
  }

  public override void OnDoingBegin(Follower follower)
  {
    base.OnDoingBegin(follower);
    follower.FacePosition(this.OfferingShrineStructure.Data.Position);
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    double num = (double) follower.SetBodyAnimation("idle-ritual-up", true);
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    return this.OfferingShrineStructure.Data.Position + new Vector3(0.0f, 0.2f);
  }
}
