// Decompiled with JetBrains decompiler
// Type: FollowerState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public abstract class FollowerState
{
  public FollowerBrain brain;
  public Follower follower;

  public abstract FollowerStateType Type { get; }

  public virtual float XPMultiplierAddition => 0.0f;

  public virtual float MaxSpeed => 2.25f;

  public virtual string OverrideIdleAnim => "idle";

  public virtual string OverrideWalkAnim => (string) null;

  public virtual void Setup(Follower follower)
  {
    this.brain = follower.Brain;
    this.follower = follower;
    this.SetStateAnimations(follower);
  }

  public virtual void Cleanup(Follower follower)
  {
    SimpleSpineAnimator.SpineChartacterAnimationData animationData1 = follower.SimpleAnimator.GetAnimationData(StateMachine.State.Idle);
    if (this.OverrideIdleAnim != null && animationData1.Animation.name == this.OverrideIdleAnim)
      animationData1.Animation = animationData1.DefaultAnimation;
    SimpleSpineAnimator.SpineChartacterAnimationData animationData2 = follower.SimpleAnimator.GetAnimationData(StateMachine.State.Moving);
    if (this.OverrideWalkAnim == null || !(animationData2.Animation.name == this.OverrideWalkAnim))
      return;
    animationData2.Animation = animationData2.DefaultAnimation;
  }

  public void SetStateAnimations(Follower follower, bool forced = false)
  {
    SimpleSpineAnimator.SpineChartacterAnimationData animationData1 = follower.SimpleAnimator.GetAnimationData(StateMachine.State.Idle);
    if (animationData1 != null && this.OverrideIdleAnim != null && animationData1.Animation.name == animationData1.DefaultAnimation.name | forced)
      follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, this.OverrideIdleAnim);
    SimpleSpineAnimator.SpineChartacterAnimationData animationData2 = follower.SimpleAnimator.GetAnimationData(StateMachine.State.Moving);
    if (animationData2 == null || this.OverrideWalkAnim == null || !(animationData2.Animation.name == animationData2.DefaultAnimation.name | forced))
      return;
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, this.OverrideWalkAnim);
  }
}
