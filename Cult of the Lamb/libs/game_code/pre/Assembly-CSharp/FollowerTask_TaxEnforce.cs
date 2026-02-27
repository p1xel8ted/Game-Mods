// Decompiled with JetBrains decompiler
// Type: FollowerTask_TaxEnforce
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class FollowerTask_TaxEnforce : FollowerTask
{
  private FollowerBrain targetFollower;

  public override FollowerTaskType Type => FollowerTaskType.TaxEnforce;

  public override FollowerLocation Location => FollowerLocation.Base;

  public FollowerTask_TaxEnforce(FollowerBrain targetFollower)
  {
    this.targetFollower = targetFollower;
  }

  protected override int GetSubTaskCode() => 0;

  protected override void TaskTick(float deltaGameTime)
  {
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
      enforcer.StartCoroutine((IEnumerator) this.TaxEnforceIE(target, enforcer));
    else
      this.TaxEnforce();
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

  private IEnumerator TaxEnforceIE(Follower target, Follower enforcer)
  {
    target.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    enforcer.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    double num1 = (double) target.SetBodyAnimation("Conversations/react-mean" + (object) UnityEngine.Random.Range(1, 4), true);
    double num2 = (double) enforcer.SetBodyAnimation("Conversations/talk-mean" + (object) UnityEngine.Random.Range(1, 4), false);
    target.State.facingAngle = Utils.GetAngle(target.transform.position, enforcer.transform.position);
    enforcer.State.facingAngle = Utils.GetAngle(enforcer.transform.position, target.transform.position);
    yield return (object) new WaitForSeconds(1f);
    double num3 = (double) enforcer.SetBodyAnimation("tax-enforcer", false);
    yield return (object) new WaitForSeconds(1.9666667f);
    AudioManager.Instance.PlayOneShot("event:/followers/pop_in", target.transform.position);
    ResourceCustomTarget.Create(enforcer.gameObject, target.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) null);
    ++enforcer.Brain._directInfoAccess.TaxCollected;
    target.Brain._directInfoAccess.TaxedToday = true;
    yield return (object) new WaitForSeconds(1f);
    this.targetFollower.AddThought(Thought.FaithEnforced);
    target.Brain.CompleteCurrentTask();
    enforcer.Brain.CompleteCurrentTask();
  }

  private void TaxEnforce()
  {
    this.targetFollower.AddThought(Thought.FaithEnforced);
    this.targetFollower.CompleteCurrentTask();
    this.targetFollower._directInfoAccess.TaxedToday = true;
    this._brain.CompleteCurrentTask();
  }

  protected override Vector3 UpdateDestination(Follower follower)
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
}
