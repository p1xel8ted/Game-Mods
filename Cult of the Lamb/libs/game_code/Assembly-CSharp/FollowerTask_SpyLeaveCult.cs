// Decompiled with JetBrains decompiler
// Type: FollowerTask_SpyLeaveCult
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class FollowerTask_SpyLeaveCult : FollowerTask
{
  public Follower _follower;
  public NotificationCentre.NotificationType _leaveNotificationType;

  public override FollowerTaskType Type => FollowerTaskType.LeaveCult;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockTaskChanges => true;

  public FollowerTask_SpyLeaveCult(
    NotificationCentre.NotificationType leaveNotificationType)
  {
    this._leaveNotificationType = leaveNotificationType;
  }

  public override int GetSubTaskCode() => 0;

  public override void OnStart()
  {
    if ((bool) (UnityEngine.Object) this._follower)
    {
      this._follower.SpeedMultiplier = 0.5f;
      this._follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Prison/Unlawful/sneak");
    }
    this.SetState(FollowerTaskState.GoingTo);
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    this._follower = follower;
  }

  public override void OnGoingToBegin(Follower follower) => base.OnGoingToBegin(follower);

  public override void SimSetup(SimFollower simFollower)
  {
    base.SimSetup(simFollower);
    this.SetState(FollowerTaskState.GoingTo);
  }

  public override void TaskTick(float deltaGameTime)
  {
  }

  public override void OnArrive()
  {
    base.OnArrive();
    Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    if ((UnityEngine.Object) followerById != (UnityEngine.Object) null)
      followerById.LeaveWithAnimationSpy();
    else
      this.End();
  }

  public IEnumerator FrameDelay(System.Action callback)
  {
    yield return (object) new WaitForEndOfFrame();
    System.Action action = callback;
    if (action != null)
      action();
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    return (UnityEngine.Object) follower != (UnityEngine.Object) null ? BaseLocationManager.Instance.GetExitPosition(FollowerLocation.Lumberjack) : Vector3.zero;
  }

  public override void OnFinaliseEnd(Follower follower)
  {
    follower.Brain.LeftCult = true;
    follower.Brain.LeftCultWithReason = this._leaveNotificationType;
  }

  public override void SimFinaliseEnd(SimFollower simFollower)
  {
    simFollower.Brain.LeftCult = true;
    simFollower.Brain.LeftCultWithReason = this._leaveNotificationType;
  }
}
