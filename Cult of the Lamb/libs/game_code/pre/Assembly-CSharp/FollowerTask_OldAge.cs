// Decompiled with JetBrains decompiler
// Type: FollowerTask_OldAge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_OldAge : FollowerTask
{
  private const float IDLE_DURATION_GAME_MINUTES_MIN = 10f;
  private const float IDLE_DURATION_GAME_MINUTES_MAX = 20f;
  private float _gameTimeToNextStateUpdate;

  public override FollowerTaskType Type => FollowerTaskType.OldAge;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockReactTasks => true;

  protected override int GetSubTaskCode() => 0;

  protected override void OnArrive() => this.SetState(FollowerTaskState.Idle);

  protected override void TaskTick(float deltaGameTime)
  {
    if (this._state != FollowerTaskState.Idle)
      return;
    this._gameTimeToNextStateUpdate -= deltaGameTime;
    if ((double) this._gameTimeToNextStateUpdate > 0.0)
      return;
    this.ClearDestination();
    this.SetState(FollowerTaskState.GoingTo);
    this._gameTimeToNextStateUpdate = Random.Range(10f, 20f);
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Old/idle-old");
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Old/walk-old");
    follower.SetOutfit(FollowerOutfitType.Old, false);
  }

  protected override Vector3 UpdateDestination(Follower follower)
  {
    return TownCentre.RandomCircleFromTownCentre(8f);
  }

  protected override float SatiationChange(float deltaGameTime) => 0.0f;

  protected override float RestChange(float deltaGameTime) => 0.0f;

  protected override float SocialChange(float deltaGameTime) => 0.0f;

  protected override float VomitChange(float deltaGameTime) => 0.0f;
}
