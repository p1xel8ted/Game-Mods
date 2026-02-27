// Decompiled with JetBrains decompiler
// Type: FollowerTask_FaithEnforce
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class FollowerTask_FaithEnforce : FollowerTask
{
  private FollowerBrain targetFollower;

  public override FollowerTaskType Type => FollowerTaskType.FaithEnforce;

  public override FollowerLocation Location => FollowerLocation.Base;

  public FollowerTask_FaithEnforce(FollowerBrain targetFollower)
  {
    this.targetFollower = targetFollower;
  }

  protected override int GetSubTaskCode() => 0;

  protected override void TaskTick(float deltaGameTime)
  {
  }

  protected override void OnStart()
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

  private void StateChange(FollowerTaskState oldState, FollowerTaskState newState)
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
        enforcer = follower;
    }
    if ((bool) (UnityEngine.Object) target && (bool) (UnityEngine.Object) enforcer && enforcer.gameObject.activeInHierarchy)
      enforcer.StartCoroutine((IEnumerator) this.FaithEnforceIE(target, enforcer));
    else
      this.FaithEnforce();
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    if (this.targetFollower == null)
      return;
    this.targetFollower.CompleteCurrentTask();
  }

  public override void SimCleanup(SimFollower simFollower)
  {
    base.SimCleanup(simFollower);
    if (this.targetFollower == null)
      return;
    this.targetFollower.CompleteCurrentTask();
  }

  private IEnumerator FaithEnforceIE(Follower target, Follower enforcer)
  {
    target.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    enforcer.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    double num1 = (double) target.SetBodyAnimation("Conversations/talk-nice" + (object) UnityEngine.Random.Range(1, 4), false);
    double num2 = (double) enforcer.SetBodyAnimation("Conversations/talk-nice" + (object) UnityEngine.Random.Range(1, 4), false);
    target.State.facingAngle = Utils.GetAngle(target.transform.position, enforcer.transform.position);
    enforcer.State.facingAngle = Utils.GetAngle(enforcer.transform.position, target.transform.position);
    target.Brain._directInfoAccess.FaithedToday = true;
    yield return (object) new WaitForSeconds(2f);
    this.targetFollower.AddThought(Thought.FaithEnforced);
    this.targetFollower.AddAdoration(FollowerBrain.AdorationActions.FaithEnforce, (System.Action) null);
    AudioManager.Instance.PlayOneShot("event:/followers/gain_loyalty", target.transform.position);
    target.Brain.CompleteCurrentTask();
    enforcer.Brain.CompleteCurrentTask();
  }

  private void FaithEnforce()
  {
    this.targetFollower.AddThought(Thought.FaithEnforced);
    this.targetFollower.CompleteCurrentTask();
    this.targetFollower._directInfoAccess.FaithedToday = true;
    this._brain.CompleteCurrentTask();
  }

  protected override Vector3 UpdateDestination(Follower follower)
  {
    if (!((UnityEngine.Object) follower != (UnityEngine.Object) null))
      return this.targetFollower.LastPosition;
    Follower followerById = FollowerManager.FindFollowerByID(this.targetFollower.Info.ID);
    return (UnityEngine.Object) followerById != (UnityEngine.Object) null ? followerById.transform.position + Vector3.right * ((double) followerById.transform.position.x < (double) follower.transform.position.x ? 1.5f : -1.5f) : this._brain.LastPosition;
  }
}
