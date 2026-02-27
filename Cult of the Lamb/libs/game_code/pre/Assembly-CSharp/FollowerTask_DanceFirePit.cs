// Decompiled with JetBrains decompiler
// Type: FollowerTask_DanceFirePit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_DanceFirePit : FollowerTask
{
  public override FollowerTaskType Type => FollowerTaskType.DanceFirePit;

  public override FollowerLocation Location => FollowerLocation.Church;

  public override int UsingStructureID => 0;

  public override float Priorty => 1000f;

  public override bool BlockReactTasks => true;

  public override bool BlockTaskChanges => true;

  public override bool BlockThoughts => true;

  public FollowerTask_DanceFirePit(int firePitID)
  {
  }

  protected override float SatiationChange(float deltaGameTime) => 0.0f;

  protected override float RestChange(float deltaGameTime) => 0.0f;

  protected override int GetSubTaskCode() => 0;

  protected override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  protected override void TaskTick(float deltaGameTime)
  {
  }

  protected override void OnEnd()
  {
    base.OnEnd();
    this._brain.AddThought(Thought.DancePit, true);
  }

  protected override Vector3 UpdateDestination(Follower follower)
  {
    return Interaction_FireDancePit.Instance.GetDancePosition(follower.Brain.Info.ID);
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    if (this.State == FollowerTaskState.Doing)
    {
      follower.State.facingAngle = Utils.GetAngle(follower.transform.position, Interaction_FireDancePit.Instance.transform.position);
      follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "dance");
    }
    follower.HideStats();
    follower.Interaction_FollowerInteraction.Interactable = false;
  }

  public override void OnDoingBegin(Follower follower)
  {
    follower.State.facingAngle = Utils.GetAngle(follower.transform.position, Interaction_FireDancePit.Instance.transform.position);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "dance");
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    follower.Interaction_FollowerInteraction.Interactable = true;
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "idle");
  }
}
