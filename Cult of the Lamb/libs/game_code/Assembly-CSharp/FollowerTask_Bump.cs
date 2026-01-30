// Decompiled with JetBrains decompiler
// Type: FollowerTask_Bump
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_Bump : FollowerTask
{
  public Follower follower;
  public Follower otherFollower;
  public bool isLeader;

  public override FollowerTaskType Type => FollowerTaskType.Bump;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockTaskChanges => true;

  public override bool BlockReactTasks => true;

  public FollowerTask_Bump(Follower otherFollower, bool isLeader = false)
  {
    this.otherFollower = otherFollower;
    this.isLeader = isLeader;
  }

  public override int GetSubTaskCode() => 0;

  public override void OnStart()
  {
    this.follower = FollowerManager.FindFollowerByID(this.Brain.Info.ID);
    if (this.isLeader)
      this.otherFollower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_Bump(this.follower));
    this.SetState(FollowerTaskState.Doing);
  }

  public override void TaskTick(float deltaGameTime)
  {
  }

  public override void OnDoingBegin(Follower follower)
  {
    if ((UnityEngine.Object) this.otherFollower == (UnityEngine.Object) null || (UnityEngine.Object) follower == (UnityEngine.Object) null)
    {
      this.End();
    }
    else
    {
      if (this._brain.HasTrait(FollowerTrait.TraitType.Blind))
        this._brain.AddRandomThoughtFromList(Thought.Blind_1, Thought.Blind_2);
      FollowerBrain.SetTimeSinceLastBump();
      follower.TimedAnimation(this.GetBumpAnimation(), 2.3f, new System.Action(((FollowerTask) this).End));
    }
  }

  public override Vector3 UpdateDestination(Follower follower) => follower.transform.position;

  public string GetBumpAnimation()
  {
    return (double) ((Vector2) (this.otherFollower.transform.position - this.follower.transform.position)).x > 0.0 && this.follower.SimpleAnimator.Dir == -1 ? "Snow/hit-front-fall" : "Snow/hit-back-fall";
  }
}
