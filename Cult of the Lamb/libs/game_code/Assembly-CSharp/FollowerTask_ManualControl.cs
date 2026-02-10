// Decompiled with JetBrains decompiler
// Type: FollowerTask_ManualControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_ManualControl : FollowerTask
{
  public bool CustomDestination;
  public Vector3 Destination;
  public System.Action Callback;

  public override FollowerTaskType Type => FollowerTaskType.ManualControl;

  public override FollowerLocation Location => this._brain.Location;

  public override bool DisablePickUpInteraction => true;

  public override bool BlockTaskChanges => true;

  public override bool BlockReactTasks => true;

  public override bool BlockSocial => true;

  public override int GetSubTaskCode() => 0;

  public override void TaskTick(float deltaGameTime)
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

  public override Vector3 UpdateDestination(Follower follower)
  {
    return !this.CustomDestination ? follower.transform.position : this.Destination;
  }

  public override void OnArrive()
  {
    base.OnArrive();
    System.Action callback = this.Callback;
    if (callback == null)
      return;
    callback();
  }

  public override void OnEnd() => base.OnEnd();

  public override void OnFinaliseBegin(Follower follower) => base.OnFinaliseBegin(follower);

  public override float SatiationChange(float deltaGameTime) => 0.0f;

  public override float RestChange(float deltaGameTime) => 0.0f;

  public override void OnComplete() => base.OnComplete();

  public override void Cleanup(Follower follower) => base.Cleanup(follower);

  public override void SimCleanup(SimFollower simFollower) => base.SimCleanup(simFollower);
}
