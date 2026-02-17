// Decompiled with JetBrains decompiler
// Type: FollowerTask_AttendTeaching
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_AttendTeaching : FollowerTask
{
  public override FollowerTaskType Type => FollowerTaskType.AttendTeaching;

  public override FollowerLocation Location => FollowerLocation.Church;

  public override bool BlockReactTasks => true;

  public override bool BlockTaskChanges => true;

  public override bool BlockSermon => false;

  public override int GetSubTaskCode() => 0;

  public override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  public override void OnArrive() => this.SetState(FollowerTaskState.Doing);

  public override float SatiationChange(float deltaGameTime) => 0.0f;

  public override void TaskTick(float deltaGameTime)
  {
    if (PlayerFarming.Location != FollowerLocation.Church)
    {
      this.End();
    }
    else
    {
      if (this._brain.Location != this.Location)
        this.Start();
      if (this.State != FollowerTaskState.Idle)
        return;
      FollowerManager.FindFollowerByID(this._brain.Info.ID).transform.position = ChurchFollowerManager.Instance.GetAudienceMemberPosition(this._brain);
    }
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    return ChurchFollowerManager.Instance.GetAudienceMemberPosition(this._brain);
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    follower.UseUnscaledTime = true;
    if (this.State != FollowerTaskState.Doing)
      return;
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    double num = (double) follower.SetBodyAnimation("idle-ritual-up", true);
  }

  public override void OnDoingBegin(Follower follower)
  {
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    double num = (double) follower.SetBodyAnimation("idle-ritual-up", true);
  }

  public override void OnFinaliseBegin(Follower follower)
  {
    double num = (double) follower.SetBodyAnimation("idle", true);
    base.OnFinaliseBegin(follower);
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    follower.UseUnscaledTime = false;
    follower.State.CURRENT_STATE = StateMachine.State.Idle;
    double num = (double) follower.SetBodyAnimation("idle", true);
  }

  public override void SimCleanup(SimFollower simFollower) => base.SimCleanup(simFollower);
}
