// Decompiled with JetBrains decompiler
// Type: FollowerTask_DanceCircleFollow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_DanceCircleFollow : FollowerTask
{
  public FollowerTask_DanceCircleLead LeadTask;

  public override FollowerTaskType Type => FollowerTaskType.DanceCircleFollow;

  public override FollowerLocation Location => this.LeadTask.Location;

  public int DancerID => this._brain.Info.ID;

  protected override int GetSubTaskCode() => this.LeadTask.LeaderID;

  protected override void OnStart()
  {
    if (this.LeadTask.State != FollowerTaskState.Done && this.LeadTask.RemainingSlotCount > 0)
    {
      this.LeadTask.JoinCircle(this);
      this.SetState(FollowerTaskState.GoingTo);
    }
    else
      this.End();
  }

  protected override void TaskTick(float deltaGameTime)
  {
  }

  public void BecomeLeader(FollowerTask_DanceCircleLead newCircle)
  {
    if (this.Brain.CurrentTaskType == FollowerTaskType.ManualControl)
      return;
    this._brain.TransitionToTask((FollowerTask) newCircle);
  }

  public void TransferToNewCircle(FollowerTask_DanceCircleLead newCircle)
  {
    this.LeadTask = newCircle;
    this.LeadTask.JoinCircle(this);
    this.ClearDestination();
    this.SetState(FollowerTaskState.GoingTo);
  }

  private void UndoStateAnimationChanges(Follower follower)
  {
    SimpleSpineAnimator.SpineChartacterAnimationData animationData = follower.SimpleAnimator.GetAnimationData(StateMachine.State.Idle);
    animationData.Animation = animationData.DefaultAnimation;
    follower.ResetStateAnimations();
  }

  protected override Vector3 UpdateDestination(Follower follower)
  {
    return this.LeadTask.GetCirclePosition(this._brain);
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    if (this.State != FollowerTaskState.Doing)
      return;
    follower.State.facingAngle = Utils.GetAngle(follower.transform.position, this.LeadTask.CenterPosition);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "dance");
  }

  public override void OnDoingBegin(Follower follower)
  {
    follower.State.facingAngle = Utils.GetAngle(follower.transform.position, this.LeadTask.CenterPosition);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "dance");
  }

  public override void Cleanup(Follower follower)
  {
    this._brain.AddThought(Thought.DanceCircleFollowed);
    this.UndoStateAnimationChanges(follower);
    if ((bool) (Object) follower.GetComponent<Interaction_BackToWork>())
      Object.Destroy((Object) follower.GetComponent<Interaction_BackToWork>());
    base.Cleanup(follower);
  }

  public override void SimCleanup(SimFollower simFollower)
  {
    this._brain.AddThought(Thought.DanceCircleFollowed);
    base.SimCleanup(simFollower);
  }
}
