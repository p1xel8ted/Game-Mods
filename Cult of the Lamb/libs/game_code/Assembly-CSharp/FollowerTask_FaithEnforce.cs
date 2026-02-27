// Decompiled with JetBrains decompiler
// Type: FollowerTask_FaithEnforce
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class FollowerTask_FaithEnforce : FollowerTask
{
  public FollowerBrain targetFollower;
  public Follower faithEnforcer;
  public Coroutine faithCollectingRoutine;

  public override FollowerTaskType Type => FollowerTaskType.FaithEnforce;

  public override FollowerLocation Location => FollowerLocation.Base;

  public FollowerTask_FaithEnforce(FollowerBrain targetFollower)
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
      if (this.targetFollower.CurrentTask != null && this.targetFollower.CurrentTask is FollowerTask_ManualControl)
      {
        this.End();
        return;
      }
      this.targetFollower.CurrentTask?.Abort();
      this.targetFollower.HardSwapToTask((FollowerTask) new FollowerTask_EnforcingManualControl(this._brain, this.Type));
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
        enforcer = this.faithEnforcer = follower;
    }
    if ((bool) (UnityEngine.Object) target && target.Brain.CurrentTaskType != FollowerTaskType.EnforcerManualControl)
      this.End();
    else if ((bool) (UnityEngine.Object) target && (bool) (UnityEngine.Object) enforcer && enforcer.gameObject.activeInHierarchy)
      this.faithCollectingRoutine = enforcer.StartCoroutine(this.FaithEnforceIE(target, enforcer));
    else
      this.FaithEnforce();
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    if (this.targetFollower != null)
      this.targetFollower.CompleteCurrentTask();
    Interaction component = (Interaction) follower.GetComponent<interaction_FollowerInteraction>();
    if (!(bool) (UnityEngine.Object) component)
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

  public IEnumerator FaithEnforceIE(Follower target, Follower enforcer)
  {
    Interaction interaction = (Interaction) enforcer.GetComponent<interaction_FollowerInteraction>();
    if ((bool) (UnityEngine.Object) interaction)
      interaction.enabled = false;
    target.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    enforcer.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    double num1 = (double) target.SetBodyAnimation("Conversations/talk-nice" + UnityEngine.Random.Range(1, 4).ToString(), false);
    double num2 = (double) enforcer.SetBodyAnimation("Conversations/talk-nice" + UnityEngine.Random.Range(1, 4).ToString(), false);
    target.State.facingAngle = Utils.GetAngle(target.transform.position, enforcer.transform.position);
    enforcer.State.facingAngle = Utils.GetAngle(enforcer.transform.position, target.transform.position);
    target.Brain._directInfoAccess.FaithedToday = true;
    yield return (object) new WaitForSeconds(2f);
    this.targetFollower.AddThought(Thought.FaithEnforced);
    this.targetFollower.AddAdoration(FollowerBrain.AdorationActions.FaithEnforce, (System.Action) null);
    AudioManager.Instance.PlayOneShot("event:/followers/gain_loyalty", target.transform.position);
    target.Brain.CompleteCurrentTask();
    enforcer.Brain.CompleteCurrentTask();
    if ((bool) (UnityEngine.Object) interaction)
      interaction.enabled = true;
  }

  public void FaithEnforce()
  {
    this.targetFollower.AddThought(Thought.FaithEnforced);
    this.targetFollower.CompleteCurrentTask();
    this.targetFollower._directInfoAccess.FaithedToday = true;
    this._brain.CompleteCurrentTask();
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    if (!((UnityEngine.Object) follower != (UnityEngine.Object) null))
      return this.targetFollower.LastPosition;
    Follower followerById = FollowerManager.FindFollowerByID(this.targetFollower.Info.ID);
    return (UnityEngine.Object) followerById != (UnityEngine.Object) null ? followerById.transform.position + Vector3.right * ((double) followerById.transform.position.x < (double) follower.transform.position.x ? 1.5f : -1.5f) : this._brain.LastPosition;
  }

  public override void OnAbort()
  {
    base.OnAbort();
    if (this.faithCollectingRoutine == null || !((UnityEngine.Object) this.faithEnforcer != (UnityEngine.Object) null) || !this.faithEnforcer.gameObject.activeInHierarchy)
      return;
    this.faithEnforcer.StopCoroutine(this.faithCollectingRoutine);
    this.faithCollectingRoutine = (Coroutine) null;
    if (this.targetFollower == null || this.targetFollower.CurrentTaskType != FollowerTaskType.EnforcerManualControl)
      return;
    this.targetFollower.CompleteCurrentTask();
  }
}
