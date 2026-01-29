// Decompiled with JetBrains decompiler
// Type: FollowerTask_Starving
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_Starving : FollowerTask
{
  public float _gameTimeToNextStateUpdate;

  public override FollowerTaskType Type => FollowerTaskType.Starving;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockReactTasks => true;

  public override int GetSubTaskCode() => 0;

  public override void OnArrive() => this.SetState(FollowerTaskState.Idle);

  public override void TaskTick(float deltaGameTime)
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

  public override Vector3 UpdateDestination(Follower follower)
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
