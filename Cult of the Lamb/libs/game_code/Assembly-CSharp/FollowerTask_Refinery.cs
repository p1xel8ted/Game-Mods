// Decompiled with JetBrains decompiler
// Type: FollowerTask_Refinery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_Refinery : FollowerTask
{
  public Structures_Refinery refineryStructure;
  public int refineryID;
  public Follower follower;

  public override FollowerTaskType Type => FollowerTaskType.Refinery;

  public override FollowerLocation Location => this.refineryStructure.Data.Location;

  public override bool BlockSocial => true;

  public override float Priorty => 25f;

  public override PriorityCategory GetPriorityCategory(
    FollowerRole FollowerRole,
    WorkerPriority WorkerPriority,
    FollowerBrain brain)
  {
    return FollowerRole == FollowerRole.Refiner ? PriorityCategory.WorkPriority : PriorityCategory.Low;
  }

  public FollowerTask_Refinery(int refineryID)
  {
    this.refineryID = refineryID;
    this.refineryStructure = StructureManager.GetStructureByID<Structures_Refinery>(refineryID);
  }

  public override int GetSubTaskCode() => this.refineryID;

  public override void ClaimReservations()
  {
    Structures_Refinery structureById = StructureManager.GetStructureByID<Structures_Refinery>(this.refineryID);
    if (structureById == null)
      return;
    structureById.ReservedForTask = true;
  }

  public override void ReleaseReservations()
  {
    Structures_Refinery structureById = StructureManager.GetStructureByID<Structures_Refinery>(this.refineryID);
    if (structureById == null)
      return;
    structureById.ReservedForTask = false;
  }

  public override void OnStart()
  {
    this.SetState(FollowerTaskState.GoingTo);
    this.refineryStructure.Data.FollowerID = this._brain.Info.ID;
  }

  public override void TaskTick(float deltaGameTime)
  {
    if (TimeManager.IsNight && !this.Brain._directInfoAccess.WorkThroughNight || this.refineryStructure.Data.QueuedResources.Count <= 0)
      this.End();
    if (this.State != FollowerTaskState.Doing)
      return;
    if (this.refineryStructure.Data.QueuedResources.Count > 0)
    {
      this.refineryStructure.Data.Progress += deltaGameTime * this._brain.Info.ProductivityMultiplier;
      if ((double) this.refineryStructure.Data.Progress > (double) this.refineryStructure.RefineryDuration(this.refineryStructure.Data.QueuedResources[0]))
      {
        this.refineryStructure.RefineryDeposit();
        if (this.refineryStructure.Data.QueuedResources.Count <= 0)
          this.Complete();
      }
    }
    if (!(bool) (Object) this.follower)
      return;
    this.follower.transform.position = this.UpdateDestination(this.follower);
  }

  public override void OnDoingBegin(Follower follower)
  {
    base.OnDoingBegin(follower);
    follower.FacePosition(this.refineryStructure.Data.Position);
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    double num = (double) follower.SetBodyAnimation("Buildings/refine", true);
    this.follower = follower;
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    Interaction_Refinery refinery = this.FindRefinery();
    return !((Object) refinery != (Object) null) ? this.refineryStructure.Data.Position + new Vector3(0.0f, 1f) : refinery.FollowerPosition.transform.position;
  }

  public Interaction_Refinery FindRefinery()
  {
    foreach (Interaction_Refinery refinery in Interaction_Refinery.Refineries)
    {
      if ((Object) refinery != (Object) null && (Object) refinery.Structure != (Object) null && refinery.Structure.Structure_Info != null && refinery.Structure.Structure_Info.ID == this.refineryID)
        return refinery;
    }
    return (Interaction_Refinery) null;
  }

  public new void Abort() => base.Abort();

  public override void OnAbort() => base.OnAbort();
}
