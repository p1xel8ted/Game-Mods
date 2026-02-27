// Decompiled with JetBrains decompiler
// Type: FollowerTask_AttendTeaching
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_AttendTeaching : FollowerTask
{
  public override FollowerTaskType Type => FollowerTaskType.AttendTeaching;

  public override FollowerLocation Location => FollowerLocation.Church;

  public override bool BlockReactTasks => true;

  public override bool BlockTaskChanges => true;

  public override bool BlockSermon => false;

  protected override int GetSubTaskCode() => 0;

  protected override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  protected override void OnArrive() => this.SetState(FollowerTaskState.Doing);

  protected override float SatiationChange(float deltaGameTime) => 0.0f;

  protected override void TaskTick(float deltaGameTime)
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

  protected override Vector3 UpdateDestination(Follower follower)
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
