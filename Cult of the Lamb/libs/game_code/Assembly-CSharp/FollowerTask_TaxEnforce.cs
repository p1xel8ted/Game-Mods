// Decompiled with JetBrains decompiler
// Type: FollowerTask_TaxEnforce
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class FollowerTask_TaxEnforce : FollowerTask
{
  public FollowerBrain targetFollower;
  public Follower taxEnforcer;
  public Coroutine taxCollectingRoutine;

  public override FollowerTaskType Type => FollowerTaskType.TaxEnforce;

  public override FollowerLocation Location => FollowerLocation.Base;

  public FollowerTask_TaxEnforce(FollowerBrain targetFollower)
  {
    this.targetFollower = targetFollower;
  }

  public override int GetSubTaskCode() => 0;

  public override void TaskTick(float deltaGameTime)
  {
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
        enforcer = this.taxEnforcer = follower;
    }
    if ((bool) (UnityEngine.Object) target && target.Brain.CurrentTaskType != FollowerTaskType.EnforcerManualControl)
      this.End();
    else if ((bool) (UnityEngine.Object) target && (bool) (UnityEngine.Object) enforcer && enforcer.gameObject.activeInHierarchy)
      this.taxCollectingRoutine = enforcer.StartCoroutine((IEnumerator) this.TaxEnforceIE(target, enforcer));
    else
      this.TaxEnforce();
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

  public IEnumerator TaxEnforceIE(Follower target, Follower enforcer)
  {
    Interaction interaction = (Interaction) enforcer.GetComponent<interaction_FollowerInteraction>();
    if ((bool) (UnityEngine.Object) interaction)
      interaction.enabled = false;
    target.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    enforcer.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    Follower follower1 = target;
    int num1 = UnityEngine.Random.Range(1, 4);
    string animName1 = "Conversations/react-mean" + num1.ToString();
    double num2 = (double) follower1.SetBodyAnimation(animName1, true);
    Follower follower2 = enforcer;
    num1 = UnityEngine.Random.Range(1, 4);
    string animName2 = "Conversations/talk-mean" + num1.ToString();
    double num3 = (double) follower2.SetBodyAnimation(animName2, false);
    target.State.facingAngle = Utils.GetAngle(target.transform.position, enforcer.transform.position);
    enforcer.State.facingAngle = Utils.GetAngle(enforcer.transform.position, target.transform.position);
    yield return (object) new WaitForSeconds(1f);
    double num4 = (double) enforcer.SetBodyAnimation("tax-enforcer", false);
    yield return (object) new WaitForSeconds(1.9666667f);
    AudioManager.Instance.PlayOneShot("event:/followers/pop_in", target.transform.position);
    ResourceCustomTarget.Create(enforcer.gameObject, target.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) null);
    ++enforcer.Brain._directInfoAccess.TaxCollected;
    target.Brain._directInfoAccess.TaxedToday = true;
    yield return (object) new WaitForSeconds(1f);
    if ((bool) (UnityEngine.Object) interaction)
      interaction.enabled = true;
    this.targetFollower.AddThought(Thought.FaithEnforced);
    target.Brain.CompleteCurrentTask();
    enforcer.Brain.CompleteCurrentTask();
    this.taxCollectingRoutine = (Coroutine) null;
  }

  public void TaxEnforce()
  {
    this.targetFollower.AddThought(Thought.FaithEnforced);
    this.targetFollower.CompleteCurrentTask();
    this.targetFollower._directInfoAccess.TaxedToday = true;
    this._brain.CompleteCurrentTask();
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    if (this.targetFollower == null || this.targetFollower.Info == null)
    {
      this.Abort();
      return Vector3.zero;
    }
    if (!((UnityEngine.Object) follower != (UnityEngine.Object) null))
      return this.targetFollower.LastPosition;
    Follower followerById = FollowerManager.FindFollowerByID(this.targetFollower.Info.ID);
    if (!((UnityEngine.Object) followerById == (UnityEngine.Object) null))
      return followerById.transform.position + Vector3.right * ((double) followerById.transform.position.x < (double) follower.transform.position.x ? 1.5f : -1.5f);
    this.Abort();
    return Vector3.zero;
  }

  public override void OnAbort()
  {
    base.OnAbort();
    if (this.taxCollectingRoutine == null || !((UnityEngine.Object) this.taxEnforcer != (UnityEngine.Object) null) || !this.taxEnforcer.gameObject.activeInHierarchy)
      return;
    this.taxEnforcer.StopCoroutine(this.taxCollectingRoutine);
    this.taxCollectingRoutine = (Coroutine) null;
    if (this.targetFollower == null || this.targetFollower.CurrentTaskType != FollowerTaskType.EnforcerManualControl)
      return;
    this.targetFollower.CompleteCurrentTask();
  }
}
