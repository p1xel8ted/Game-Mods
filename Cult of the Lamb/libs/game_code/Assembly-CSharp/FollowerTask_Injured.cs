// Decompiled with JetBrains decompiler
// Type: FollowerTask_Injured
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_Injured : FollowerTask
{
  public float _gameTimeToNextStateUpdate;

  public override FollowerTaskType Type => FollowerTaskType.Injured;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockReactTasks => true;

  public override int GetSubTaskCode() => 0;

  public override void OnArrive() => this.SetState(FollowerTaskState.Idle);

  public override void TaskTick(float deltaGameTime)
  {
    if ((double) this._brain.Stats.Injured == 0.0)
    {
      this._brain.RemoveCurseState(Thought.Injured);
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
}
