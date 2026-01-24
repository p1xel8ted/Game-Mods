// Decompiled with JetBrains decompiler
// Type: FollowerTask_HugFollower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class FollowerTask_HugFollower : FollowerTask
{
  public FollowerBrain targetFollower;
  public Follower leader;
  public Coroutine huggingRoutine;

  public override FollowerTaskType Type => FollowerTaskType.HuggingFollower;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockReactTasks => true;

  public override bool BlockSocial => true;

  public FollowerTask_HugFollower(FollowerBrain targetFollower)
  {
    this.targetFollower = targetFollower;
  }

  public override int GetSubTaskCode() => 0;

  public override void TaskTick(float deltaGameTime)
  {
  }

  public override void OnStart()
  {
    base.OnStart();
    this.OnFollowerTaskStateChanged = this.OnFollowerTaskStateChanged + new FollowerTask.FollowerTaskDelegate(this.StateChange);
    if (this.targetFollower != null)
    {
      this.targetFollower.CurrentTask?.Abort();
      this.targetFollower.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    }
    this.ClearDestination();
    this.SetState(FollowerTaskState.GoingTo);
  }

  public void StateChange(FollowerTaskState oldState, FollowerTaskState newState)
  {
    if (oldState != FollowerTaskState.GoingTo || newState != FollowerTaskState.Doing)
      return;
    Follower target = (Follower) null;
    Follower enforcer = (Follower) null;
    foreach (Follower follower in FollowerManager.FollowersAtLocation(FollowerLocation.Base))
    {
      if (follower.Brain.Info.ID == this.targetFollower.Info.ID)
        target = follower;
      else if (follower.Brain.Info.ID == this._brain.Info.ID)
        enforcer = this.leader = follower;
    }
    if ((bool) (Object) target && (bool) (Object) enforcer && enforcer.gameObject.activeInHierarchy)
    {
      this.huggingRoutine = enforcer.StartCoroutine((IEnumerator) this.HuggingFollowerIE(target, enforcer));
    }
    else
    {
      this.targetFollower.CompleteCurrentTask();
      this.Brain.CompleteCurrentTask();
    }
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    if (this.targetFollower != null)
      this.targetFollower.CompleteCurrentTask();
    Interaction component = (Interaction) follower.GetComponent<interaction_FollowerInteraction>();
    if (!(bool) (Object) component)
      return;
    component.enabled = true;
  }

  public override void SimCleanup(SimFollower simFollower)
  {
    base.SimCleanup(simFollower);
    if (this.targetFollower == null)
      return;
    this.targetFollower.CompleteCurrentTask();
  }

  public IEnumerator HuggingFollowerIE(Follower target, Follower enforcer)
  {
    Interaction interaction = (Interaction) enforcer.GetComponent<interaction_FollowerInteraction>();
    if ((bool) (Object) interaction)
      interaction.enabled = false;
    target.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    enforcer.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    double num1 = (double) target.SetBodyAnimation(target.Brain.Info.CursedState == Thought.Child ? "Hugging/hug-baby" : "Hugging/hug-parent", false);
    double num2 = (double) enforcer.SetBodyAnimation(enforcer.Brain.Info.CursedState == Thought.Child ? "Hugging/hug-baby" : "Hugging/hug-parent", false);
    yield return (object) new WaitForEndOfFrame();
    target.State.facingAngle = Utils.GetAngle(target.transform.position, enforcer.transform.position);
    enforcer.State.facingAngle = Utils.GetAngle(enforcer.transform.position, target.transform.position);
    yield return (object) new WaitForSeconds(0.75f);
    AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", target.transform.position);
    BiomeConstants.Instance.EmitHeartPickUpVFX((target.transform.position + enforcer.transform.position) / 2f, 0.0f, "red", "burst_big");
    yield return (object) new WaitForSeconds(3.41666651f);
    target.Brain.CompleteCurrentTask();
    enforcer.Brain.CompleteCurrentTask();
    if ((bool) (Object) interaction)
      interaction.enabled = true;
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    if (!((Object) follower != (Object) null))
      return this.targetFollower.LastPosition;
    Follower followerById = FollowerManager.FindFollowerByID(this.targetFollower.Info.ID);
    return (Object) followerById != (Object) null ? followerById.transform.position + Vector3.right * ((double) followerById.transform.position.x < (double) follower.transform.position.x ? 1f : -1f) : this._brain.LastPosition;
  }

  public override void OnAbort()
  {
    base.OnAbort();
    if (this.huggingRoutine == null || !((Object) this.leader != (Object) null) || !this.leader.gameObject.activeInHierarchy)
      return;
    this.leader.StopCoroutine(this.huggingRoutine);
    this.huggingRoutine = (Coroutine) null;
    if (this.targetFollower == null)
      return;
    this.targetFollower.CompleteCurrentTask();
  }
}
