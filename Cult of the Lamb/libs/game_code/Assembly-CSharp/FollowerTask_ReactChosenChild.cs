// Decompiled with JetBrains decompiler
// Type: FollowerTask_ReactChosenChild
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerTask_ReactChosenChild : FollowerTask
{
  public int _followerID;
  public Follower _follower;

  public override FollowerTaskType Type => FollowerTaskType.ReactChosenChild;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockTaskChanges => true;

  public override int UsingStructureID => 0;

  public FollowerTask_ReactChosenChild(int followerID)
  {
    this._followerID = followerID;
    this._follower = FollowerManager.FindFollowerByID(this._followerID);
  }

  public override int GetSubTaskCode() => this._followerID;

  public override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  public override void TaskTick(float deltaGameTime)
  {
    if (this._brain.Location == PlayerFarming.Location)
      return;
    this.End();
  }

  public override void ProgressTask() => this.End();

  public override void OnEnd() => base.OnEnd();

  public override Vector3 UpdateDestination(Follower follower)
  {
    float num = UnityEngine.Random.Range(0.5f, 1.5f);
    float f = (float) (((double) Utils.GetAngle(this._follower.transform.position, follower.transform.position) + (double) UnityEngine.Random.Range(-45, 45)) * (Math.PI / 180.0));
    return this._follower.transform.position + new Vector3(num * Mathf.Cos(f), num * Mathf.Sin(f));
  }

  public override void OnDoingBegin(Follower follower)
  {
    follower.TimedAnimation("bowed-down", 3.23333335f, (System.Action) (() => this.ProgressTask()));
  }

  [CompilerGenerated]
  public void \u003COnDoingBegin\u003Eb__17_0() => this.ProgressTask();
}
