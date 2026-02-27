// Decompiled with JetBrains decompiler
// Type: FollowerTask_ReactGhost
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_ReactGhost : FollowerTask
{
  public override FollowerTaskType Type => FollowerTaskType.ReactGrave;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockTaskChanges => true;

  protected override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  protected override void TaskTick(float deltaGameTime)
  {
    if (this._brain.Location == PlayerFarming.Location)
      return;
    this.End();
  }

  public override void ProgressTask() => this.End();

  public override void OnDoingBegin(Follower follower)
  {
    follower.TimedAnimation("Reactions/react-spooked", 0.7f, (System.Action) (() => follower.TimedAnimation("Reactions/react-worried2", 1.933f, (System.Action) (() => this.ProgressTask()))));
  }

  protected override int GetSubTaskCode() => 0;

  protected override Vector3 UpdateDestination(Follower follower) => follower.transform.position;
}
