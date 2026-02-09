// Decompiled with JetBrains decompiler
// Type: FollowerTask_ReactFollowerSteal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerTask_ReactFollowerSteal : FollowerTask
{
  public FollowerBrain targetFollower;
  public int state;

  public override FollowerTaskType Type => FollowerTaskType.ReactFollowerSteal;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockTaskChanges => true;

  public FollowerTask_ReactFollowerSteal(FollowerBrain targetFollower)
  {
    this.targetFollower = targetFollower;
  }

  public override int GetSubTaskCode() => 0;

  public override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  public override void TaskTick(float deltaGameTime)
  {
    if (this._brain.Location == PlayerFarming.Location)
      return;
    this.End();
  }

  public override void ProgressTask() => this.End();

  public override void OnEnd() => base.OnEnd();

  public override Vector3 UpdateDestination(Follower follower) => follower.transform.position;

  public override void OnDoingBegin(Follower follower)
  {
    if (this._brain.HasTrait(FollowerTrait.TraitType.Bastard) || this._brain.HasTrait(FollowerTrait.TraitType.CriminalHardened))
    {
      follower.TimedAnimation("Reactions/react-laugh", 3.33333325f, (System.Action) (() => this.ProgressTask()));
      this.state = 0;
    }
    else
      this.DoRandomAnimation(follower);
    follower.FacePosition(this.targetFollower.LastPosition);
  }

  public void DoRandomAnimation(Follower follower)
  {
    double num = (double) UnityEngine.Random.value;
    this.state = 1;
    if (num < 0.5)
    {
      string animation = "Conversations/react-hate" + UnityEngine.Random.Range(1, 4).ToString();
      follower.TimedAnimation(animation, 2f, (System.Action) (() => this.ProgressTask()));
    }
    else
    {
      string animation = "Reactions/react-worried" + UnityEngine.Random.Range(1, 3).ToString();
      follower.TimedAnimation(animation, 1.93333328f, (System.Action) (() => this.ProgressTask()));
    }
  }

  [CompilerGenerated]
  public void \u003COnDoingBegin\u003Eb__15_0() => this.ProgressTask();

  [CompilerGenerated]
  public void \u003CDoRandomAnimation\u003Eb__16_0() => this.ProgressTask();

  [CompilerGenerated]
  public void \u003CDoRandomAnimation\u003Eb__16_1() => this.ProgressTask();
}
