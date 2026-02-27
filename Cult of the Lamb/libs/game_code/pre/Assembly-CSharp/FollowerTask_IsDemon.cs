// Decompiled with JetBrains decompiler
// Type: FollowerTask_IsDemon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_IsDemon : FollowerTask
{
  public override FollowerTaskType Type => FollowerTaskType.IsDemon;

  public override FollowerLocation Location => FollowerLocation.Demon;

  public override bool ShouldSaveDestination => true;

  public override bool DisablePickUpInteraction => true;

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

  protected override int GetSubTaskCode() => 0;

  protected override float SatiationChange(float deltaGameTime) => 0.0f;

  protected override float RestChange(float deltaGameTime) => 0.0f;

  protected override void TaskTick(float deltaGameTime)
  {
    if (DataManager.Instance.Followers_Demons_IDs.Contains(this._brain.Info.ID))
      return;
    this._brain.HardSwapToTask((FollowerTask) new FollowerTask_Idle());
    if (this._brain.CurrentTaskType == FollowerTaskType.ChangeLocation)
      this._brain.CurrentTask.Arrive();
    this._brain.CompleteCurrentTask();
  }

  protected override Vector3 UpdateDestination(Follower follower) => Vector3.zero;
}
