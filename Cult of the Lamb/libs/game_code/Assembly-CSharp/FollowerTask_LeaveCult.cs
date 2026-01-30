// Decompiled with JetBrains decompiler
// Type: FollowerTask_LeaveCult
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerTask_LeaveCult : FollowerTask
{
  public NotificationCentre.NotificationType _leaveNotificationType;

  public override FollowerTaskType Type => FollowerTaskType.LeaveCult;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool DisablePickUpInteraction => true;

  public override bool BlockTaskChanges => true;

  public FollowerTask_LeaveCult(
    NotificationCentre.NotificationType leaveNotificationType)
  {
    this._leaveNotificationType = leaveNotificationType;
  }

  public override int GetSubTaskCode() => 0;

  public override void OnStart()
  {
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    follower.Interaction_FollowerInteraction.Interactable = false;
    follower.TimedAnimation("tantrum-big", 6f, (System.Action) (() =>
    {
      this.\u003C\u003En__0(follower);
      this.SetState(FollowerTaskState.GoingTo);
    }));
  }

  public override void OnGoingToBegin(Follower follower)
  {
    base.OnGoingToBegin(follower);
    follower.Interaction_FollowerInteraction.Interactable = false;
  }

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
      followerById.LeaveWithAnimation();
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

  [CompilerGenerated]
  [DebuggerHidden]
  public void \u003C\u003En__0(Follower follower) => this.OnIdleBegin(follower);
}
