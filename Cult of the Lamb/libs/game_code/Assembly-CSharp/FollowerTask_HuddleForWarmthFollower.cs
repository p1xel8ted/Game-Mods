// Decompiled with JetBrains decompiler
// Type: FollowerTask_HuddleForWarmthFollower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_HuddleForWarmthFollower : FollowerTask
{
  public FollowerTask_HuddleForWarmthLeader LeadTask;

  public override FollowerTaskType Type => FollowerTaskType.HuddleForWarmthFollower;

  public override FollowerLocation Location => this.LeadTask.Location;

  public int DancerID => this._brain.Info.ID;

  public override bool BlockReactTasks => true;

  public override bool BlockSocial => true;

  public override int GetSubTaskCode() => this.LeadTask.LeaderID;

  public override void OnStart()
  {
    if (this.LeadTask.State != FollowerTaskState.Done && this.LeadTask.RemainingSlotCount > 0)
    {
      this.LeadTask.JoinCircle(this);
      this.SetState(FollowerTaskState.GoingTo);
    }
    else
      this.End();
  }

  public override void TaskTick(float deltaGameTime)
  {
  }

  public void BecomeLeader(FollowerTask_HuddleForWarmthLeader newCircle)
  {
    if (this.Brain.CurrentTaskType == FollowerTaskType.ManualControl)
      return;
    this._brain.TransitionToTask((FollowerTask) newCircle);
  }

  public void TransferToNewCircle(FollowerTask_HuddleForWarmthLeader newCircle)
  {
    this.LeadTask = newCircle;
    this.LeadTask.JoinCircle(this);
    this.ClearDestination();
    this.SetState(FollowerTaskState.GoingTo);
  }

  public void UndoStateAnimationChanges(Follower follower)
  {
    SimpleSpineAnimator.SpineChartacterAnimationData animationData = follower.SimpleAnimator.GetAnimationData(StateMachine.State.Idle);
    animationData.Animation = animationData.DefaultAnimation;
    follower.ResetStateAnimations();
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    return this.LeadTask.GetCirclePosition(this._brain);
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    if (this.State == FollowerTaskState.Doing)
    {
      string NewAnimation = "Snow/idle-sad";
      follower.State.facingAngle = Utils.GetAngle(follower.transform.position, this.LeadTask.CenterPosition);
      if (SeasonsManager.CurrentWeatherEvent == SeasonsManager.WeatherEvent.Blizzard)
      {
        float num = Random.value;
        if ((double) num < 0.20000000298023224)
          NewAnimation = "Snow/blow-on-hands";
        else if ((double) num < 0.40000000596046448)
          NewAnimation = "Snow/shuffle";
      }
      else
        NewAnimation = !this.LeadTask.TargetingStructure ? ((double) WarmthBar.WarmthNormalized > 0.25 ? "Snow/idle-smile" : "Snow/idle-sad") : $"Furnace/furnace-warm{Random.Range(1, 6)}";
      follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, NewAnimation);
    }
    if (SeasonsManager.CurrentWeatherEvent == SeasonsManager.WeatherEvent.Blizzard)
      return;
    follower.gameObject.AddComponent<Interaction_BackToWork>().Init(follower, true);
  }

  public override void OnDoingBegin(Follower follower)
  {
    string NewAnimation = "Snow/idle-sad";
    follower.State.facingAngle = Utils.GetAngle(follower.transform.position, this.LeadTask.CenterPosition);
    if (SeasonsManager.CurrentWeatherEvent == SeasonsManager.WeatherEvent.Blizzard)
    {
      float num = Random.value;
      if ((double) num < 0.20000000298023224)
        NewAnimation = "Snow/blow-on-hands";
      else if ((double) num < 0.40000000596046448)
        NewAnimation = "Snow/shuffle";
    }
    else
      NewAnimation = !this.LeadTask.TargetingStructure ? ((double) WarmthBar.WarmthNormalized > 0.25 ? "Snow/idle-smile" : "Snow/idle-sad") : $"Furnace/furnace-warm{Random.Range(1, 6)}";
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, NewAnimation);
  }

  public override void Cleanup(Follower follower)
  {
    this._brain.AddThought(Thought.DanceCircleFollowed);
    this.UndoStateAnimationChanges(follower);
    if ((bool) (Object) follower.GetComponent<Interaction_BackToWork>())
      Object.Destroy((Object) follower.GetComponent<Interaction_BackToWork>());
    base.Cleanup(follower);
  }

  public override void SimCleanup(SimFollower simFollower)
  {
    this._brain.AddThought(Thought.DanceCircleFollowed);
    base.SimCleanup(simFollower);
  }
}
