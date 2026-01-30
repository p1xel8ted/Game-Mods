// Decompiled with JetBrains decompiler
// Type: FollowerTask_MutatedDead
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_MutatedDead : FollowerTask
{
  public Follower follower;

  public override FollowerTaskType Type => FollowerTaskType.MutatedDead;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockReactTasks => true;

  public override bool BlockSermon => true;

  public override bool BlockTaskChanges => true;

  public override bool BlockThoughts => true;

  public override float Priorty => 1000f;

  public override PriorityCategory GetPriorityCategory(
    FollowerRole FollowerRole,
    WorkerPriority WorkerPriority,
    FollowerBrain brain)
  {
    return PriorityCategory.ExtremelyUrgent;
  }

  public override int GetSubTaskCode() => 0;

  public override float SatiationChange(float deltaGameTime) => 0.0f;

  public override float RestChange(float deltaGameTime) => 0.0f;

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    this.follower = follower;
  }

  public override void OnStart()
  {
    base.OnStart();
    if (PlacementRegion.Instance.GetTileGridTileAtWorldPosition(this.Brain.LastPosition) == null)
      this.SetState(FollowerTaskState.GoingTo);
    else
      this.SetState(FollowerTaskState.Doing);
  }

  public override void TaskTick(float deltaGameTime)
  {
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    if (PlacementRegion.Instance.GetTileGridTileAtWorldPosition(this.Brain.LastPosition) != null)
      return follower.Brain.LastPosition;
    Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(0.0f, -1f));
    LayerMask layerMask = (LayerMask) ((int) new LayerMask() | 1 << LayerMask.NameToLayer("Island"));
    return (Vector3) (Physics2D.Raycast((Vector2) Vector3.zero, direction, 1000f, (int) layerMask).point + -direction * Random.Range(3f, 8f));
  }
}
