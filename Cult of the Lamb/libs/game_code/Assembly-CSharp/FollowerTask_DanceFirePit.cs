// Decompiled with JetBrains decompiler
// Type: FollowerTask_DanceFirePit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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

  public override float SatiationChange(float deltaGameTime) => 0.0f;

  public override float RestChange(float deltaGameTime) => 0.0f;

  public override int GetSubTaskCode() => 0;

  public override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  public override void TaskTick(float deltaGameTime)
  {
  }

  public override void OnEnd()
  {
    base.OnEnd();
    this._brain.AddThought(Thought.DancePit, true);
  }

  public override Vector3 UpdateDestination(Follower follower)
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
