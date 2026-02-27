// Decompiled with JetBrains decompiler
// Type: FollowerTask_FakeWork
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_FakeWork : FollowerTask
{
  private float _gameTimeToNextStateUpdate;

  public override FollowerTaskType Type => FollowerTaskType.FakeWork;

  public override FollowerLocation Location => this._brain.HomeLocation;

  protected override int GetSubTaskCode() => 0;

  protected override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  protected override void OnArrive() => this.SetState(FollowerTaskState.Idle);

  protected override void TaskTick(float deltaGameTime)
  {
    if (this._state != FollowerTaskState.Idle)
      return;
    this._gameTimeToNextStateUpdate -= deltaGameTime;
    if ((double) this._gameTimeToNextStateUpdate > 0.0)
      return;
    this._brain.CheckChangeTask();
    if ((double) Random.Range(0.0f, 1f) < 0.5)
      this.Wander();
    this._gameTimeToNextStateUpdate = Random.Range(3f, 7f);
  }

  private void Wander()
  {
    this.ClearDestination();
    this.SetState(FollowerTaskState.GoingTo);
  }

  protected override Vector3 UpdateDestination(Follower follower)
  {
    return TownCentre.RandomPositionInCachedTownCentre();
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, (double) Random.value < 0.5 ? "dig" : "sweep-floor");
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    follower.SimpleAnimator.ResetAnimationsToDefaults();
    follower.ResetStateAnimations();
  }
}
