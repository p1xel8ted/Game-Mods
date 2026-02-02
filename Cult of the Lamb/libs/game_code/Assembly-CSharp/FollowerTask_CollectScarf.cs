// Decompiled with JetBrains decompiler
// Type: FollowerTask_CollectScarf
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerTask_CollectScarf : FollowerTask
{
  public bool claimedScarf;

  public override FollowerTaskType Type => FollowerTaskType.CollectScarf;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockReactTasks => true;

  public override bool BlockSermon => true;

  public override bool BlockSocial => true;

  public override bool BlockTaskChanges => true;

  public override PriorityCategory GetPriorityCategory(
    FollowerRole FollowerRole,
    WorkerPriority WorkerPriority,
    FollowerBrain brain)
  {
    return brain.CanFreeze() ? PriorityCategory.ExtremelyUrgent : PriorityCategory.Ignore;
  }

  public override void ClaimReservations()
  {
    base.ClaimReservations();
    List<Structures_WoolyShack> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_WoolyShack>(FollowerLocation.Base);
    if (structuresOfType.Count <= 0)
      return;
    structuresOfType[0].ReserveScarf();
  }

  public override void ReleaseReservations()
  {
    base.ReleaseReservations();
    if (this.claimedScarf)
      return;
    List<Structures_WoolyShack> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_WoolyShack>(FollowerLocation.Base);
    if (structuresOfType.Count <= 0)
      return;
    structuresOfType[0].UnreserveScarf();
  }

  public override void OnStart()
  {
    base.OnStart();
    this.SetState(FollowerTaskState.GoingTo);
  }

  public override void OnGoingToEnd(Follower follower)
  {
    base.OnGoingToEnd(follower);
    this.SetState(FollowerTaskState.Doing);
  }

  public override void OnDoingBegin(Follower follower)
  {
    base.OnDoingBegin(follower);
    follower.TimedAnimation("Action", 1f, (System.Action) (() =>
    {
      this.claimedScarf = true;
      follower.Brain.Info.Customisation = FollowerCustomisationType.Scarf;
      follower.Brain.CheckChangeState();
      FollowerBrain.SetFollowerCostume(follower.Spine.Skeleton, follower.Brain._directInfoAccess, forceUpdate: true);
      this.End();
    }));
  }

  public override void SimDoingBegin(SimFollower simFollower)
  {
    base.SimDoingBegin(simFollower);
    this.claimedScarf = true;
    simFollower.Brain.Info.Customisation = FollowerCustomisationType.Scarf;
    simFollower.Brain.CheckChangeState();
    this.End();
  }

  public override int GetSubTaskCode() => 0;

  public override void TaskTick(float deltaGameTime)
  {
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    if ((UnityEngine.Object) Interaction_WoolyShack.Instance != (UnityEngine.Object) null)
      return Interaction_WoolyShack.Instance.Trunk.transform.position;
    List<Structures_WoolyShack> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_WoolyShack>(FollowerLocation.Base);
    return structuresOfType.Count <= 0 ? Vector3.zero : structuresOfType[0].Data.Position;
  }
}
