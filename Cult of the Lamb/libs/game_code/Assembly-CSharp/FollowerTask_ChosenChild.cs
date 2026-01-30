// Decompiled with JetBrains decompiler
// Type: FollowerTask_ChosenChild
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_ChosenChild : FollowerTask
{
  public Follower follower;
  public float repathTimer;

  public override FollowerTaskType Type => FollowerTaskType.ChosenChild;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockReactTasks => true;

  public override bool BlockSermon => true;

  public override bool BlockSocial => true;

  public override bool BlockTaskChanges => true;

  public override bool BlockThoughts => true;

  public override int GetSubTaskCode() => 0;

  public override void OnStart() => this.SetState(FollowerTaskState.Idle);

  public override void TaskTick(float deltaGameTime)
  {
    if (DataManager.Instance.CurrentOnboardingFollowerID == this._brain.Info.ID && this.Brain.Location == FollowerLocation.Base)
    {
      this.Brain.HardSwapToTask((FollowerTask) new FollowerTask_GetAttention(Follower.ComplaintType.GiveOnboarding, false));
    }
    else
    {
      if ((double) (this.repathTimer -= deltaGameTime) >= 0.0)
        return;
      this.ClearDestination();
      this.SetState(FollowerTaskState.GoingTo);
    }
  }

  public override void OnAbort()
  {
    base.OnAbort();
    if (!(bool) (Object) this.follower)
      return;
    this.follower.ClearPath();
  }

  public override void OnEnd()
  {
    base.OnEnd();
    if (!(bool) (Object) this.follower)
      return;
    this.follower.ClearPath();
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    return TownCentre.RandomCircleFromTownCentre(7f);
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    this.follower = follower;
    follower.GetComponentInChildren<FollowerPleasureUI>()?.HideCompleted();
    follower.Interaction_FollowerInteraction.Interactable = false;
    follower.HideAllFollowerIcons();
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "meditate-enlightenment");
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "meditate-enlightenment");
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    double num = (double) follower.SetBodyAnimation("meditate-enlightenment", true);
    follower.Brain.CheckChangeState();
    this.SetState(FollowerTaskState.Idle);
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    follower.LockToGround = true;
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "idle");
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");
    follower.UsePathing = true;
    follower.ShowAllFollowerIcons();
    follower.Interaction_FollowerInteraction.Interactable = true;
    follower.UpdateOutfit();
    follower.SetEmotionAnimation();
  }
}
