// Decompiled with JetBrains decompiler
// Type: FollowerTask_Starving
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_Starving : FollowerTask
{
  private float _gameTimeToNextStateUpdate;

  public override FollowerTaskType Type => FollowerTaskType.Starving;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockReactTasks => true;

  protected override int GetSubTaskCode() => 0;

  protected override void OnArrive() => this.SetState(FollowerTaskState.Idle);

  protected override void TaskTick(float deltaGameTime)
  {
    if ((double) this._brain.Stats.Starvation == 0.0)
    {
      this._brain.RemoveCurseState(Thought.BecomeStarving);
      this.End();
    }
    else
    {
      if (this._state != FollowerTaskState.Idle)
        return;
      this._gameTimeToNextStateUpdate -= deltaGameTime;
      if ((double) this._gameTimeToNextStateUpdate > 0.0)
        return;
      this.ClearDestination();
      this.SetState(FollowerTaskState.GoingTo);
      this._gameTimeToNextStateUpdate = Random.Range(4f, 6f);
    }
  }

  protected override Vector3 UpdateDestination(Follower follower)
  {
    return TownCentre.RandomPositionInCachedTownCentre();
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Hungry/idle-hungry");
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Hungry/walk-hungry");
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "idle");
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");
  }
}
