// Decompiled with JetBrains decompiler
// Type: FollowerTask_ReactChild
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerTask_ReactChild : FollowerTask
{
  public int _followerID;
  public Follower _follower;

  public override FollowerTaskType Type => FollowerTaskType.ReactChild;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockTaskChanges => true;

  public override int UsingStructureID => 0;

  public bool IsPositiveReaction
  {
    get
    {
      return !this.Brain.HasTrait(FollowerTrait.TraitType.Argumentative) && !this.Brain.HasTrait(FollowerTrait.TraitType.Bastard) && !this.Brain.HasTrait(FollowerTrait.TraitType.Celibate);
    }
  }

  public FollowerTask_ReactChild(int followerID)
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

  public override void OnEnd()
  {
    base.OnEnd();
    if (this.IsPositiveReaction)
      this.Brain.AddThought((Thought) UnityEngine.Random.Range(402, 406));
    else
      this.Brain.AddThought((Thought) UnityEngine.Random.Range(406, 410));
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    float num = UnityEngine.Random.Range(0.5f, 1.5f);
    float f = (float) (((double) Utils.GetAngle(this._follower.transform.position, follower.transform.position) + (double) UnityEngine.Random.Range(-45, 45)) * (Math.PI / 180.0));
    return this._follower.transform.position + new Vector3(num * Mathf.Cos(f), num * Mathf.Sin(f));
  }

  public override void OnDoingBegin(Follower follower)
  {
    if (this.IsPositiveReaction)
      follower.TimedAnimation("Reactions/react-admire-outfit", 2.13333344f, (System.Action) (() => this.ProgressTask()));
    else
      follower.TimedAnimation("Conversations/react-mean" + UnityEngine.Random.Range(1, 4).ToString(), 2f, (System.Action) (() => this.ProgressTask()));
  }

  [CompilerGenerated]
  public void \u003COnDoingBegin\u003Eb__19_0() => this.ProgressTask();

  [CompilerGenerated]
  public void \u003COnDoingBegin\u003Eb__19_1() => this.ProgressTask();
}
