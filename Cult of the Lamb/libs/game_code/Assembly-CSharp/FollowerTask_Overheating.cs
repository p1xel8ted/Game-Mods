// Decompiled with JetBrains decompiler
// Type: FollowerTask_Overheating
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_Overheating : FollowerTask
{
  public const float IDLE_DURATION_GAME_MINUTES_MIN = 10f;
  public const float IDLE_DURATION_GAME_MINUTES_MAX = 20f;
  public float _gameTimeToNextStateUpdate;

  public override FollowerTaskType Type => FollowerTaskType.Overheating;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockReactTasks => true;

  public override bool BlockSermon => true;

  public override int GetSubTaskCode() => 0;

  public override void TaskTick(float deltaGameTime)
  {
    if (this._state != FollowerTaskState.Idle && this._state != FollowerTaskState.Doing)
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
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Overheated/idle");
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Overheated/walk");
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "idle");
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    return TownCentre.RandomCircleFromTownCentre(16f);
  }

  public override float RestChange(float deltaGameTime) => 100f;
}
