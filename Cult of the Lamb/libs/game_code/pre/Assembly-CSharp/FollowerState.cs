// Decompiled with JetBrains decompiler
// Type: FollowerState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

#nullable disable
public abstract class FollowerState
{
  public abstract FollowerStateType Type { get; }

  public virtual float XPMultiplierAddition => 0.0f;

  public virtual float MaxSpeed => 2.25f;

  public virtual string OverrideIdleAnim => (string) null;

  public virtual string OverrideWalkAnim => (string) null;

  public virtual void Setup(Follower follower) => this.SetStateAnimations(follower);

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

  public void SetStateAnimations(Follower follower)
  {
    SimpleSpineAnimator.SpineChartacterAnimationData animationData1 = follower.SimpleAnimator.GetAnimationData(StateMachine.State.Idle);
    if (this.OverrideIdleAnim != null && animationData1.Animation.name == animationData1.DefaultAnimation.name)
      follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, this.OverrideIdleAnim);
    SimpleSpineAnimator.SpineChartacterAnimationData animationData2 = follower.SimpleAnimator.GetAnimationData(StateMachine.State.Moving);
    if (this.OverrideWalkAnim == null || !(animationData2.Animation.name == animationData2.DefaultAnimation.name))
      return;
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, this.OverrideWalkAnim);
  }
}
