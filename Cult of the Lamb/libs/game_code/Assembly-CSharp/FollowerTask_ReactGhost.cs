// Decompiled with JetBrains decompiler
// Type: FollowerTask_ReactGhost
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_ReactGhost : FollowerTask
{
  public override FollowerTaskType Type => FollowerTaskType.ReactGrave;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockTaskChanges => true;

  public override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  public override void TaskTick(float deltaGameTime)
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

  public override int GetSubTaskCode() => 0;

  public override Vector3 UpdateDestination(Follower follower) => follower.transform.position;
}
