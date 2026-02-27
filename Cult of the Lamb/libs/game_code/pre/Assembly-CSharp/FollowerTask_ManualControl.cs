// Decompiled with JetBrains decompiler
// Type: FollowerTask_ManualControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_ManualControl : FollowerTask
{
  private bool CustomDestination;
  private Vector3 Destination;
  private System.Action Callback;

  public override FollowerTaskType Type => FollowerTaskType.ManualControl;

  public override FollowerLocation Location => this._brain.Location;

  public override bool DisablePickUpInteraction => true;

  public override bool BlockTaskChanges => true;

  public override bool BlockReactTasks => true;

  protected override int GetSubTaskCode() => 0;

  protected override void TaskTick(float deltaGameTime)
  {
  }

  public void GoToAndStop(Follower Follower, Vector3 Destination, System.Action Callback)
  {
    this.ClearDestination();
    this.CustomDestination = true;
    this.Destination = Destination;
    this.Callback = Callback;
    this.OnGoingToBegin(Follower);
  }

  protected override Vector3 UpdateDestination(Follower follower)
  {
    return !this.CustomDestination ? follower.transform.position : this.Destination;
  }

  protected override void OnArrive()
  {
    base.OnArrive();
    System.Action callback = this.Callback;
    if (callback == null)
      return;
    callback();
  }

  protected override void OnEnd() => base.OnEnd();

  public override void OnFinaliseBegin(Follower follower) => base.OnFinaliseBegin(follower);

  protected override float SatiationChange(float deltaGameTime) => 0.0f;

  protected override float RestChange(float deltaGameTime) => 0.0f;

  protected override void OnComplete() => base.OnComplete();

  public override void Cleanup(Follower follower) => base.Cleanup(follower);

  public override void SimCleanup(SimFollower simFollower) => base.SimCleanup(simFollower);
}
