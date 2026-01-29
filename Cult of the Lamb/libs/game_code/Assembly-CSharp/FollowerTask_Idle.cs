// Decompiled with JetBrains decompiler
// Type: FollowerTask_Idle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_Idle : FollowerTask
{
  public float _gameTimeToNextStateUpdate;

  public override FollowerTaskType Type => FollowerTaskType.Idle;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override int GetSubTaskCode() => 0;

  public override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  public override void OnArrive() => this.SetState(FollowerTaskState.Idle);

  public override void TaskTick(float deltaGameTime)
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

  public void Wander()
  {
    this.ClearDestination();
    this.SetState(FollowerTaskState.GoingTo);
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    return TownCentre.RandomPositionInCachedTownCentre();
  }
}
