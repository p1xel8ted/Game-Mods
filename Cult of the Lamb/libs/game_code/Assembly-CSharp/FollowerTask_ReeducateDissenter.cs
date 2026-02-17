// Decompiled with JetBrains decompiler
// Type: FollowerTask_ReeducateDissenter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class FollowerTask_ReeducateDissenter : FollowerTask
{
  public FollowerBrain targetFollower;
  public float progress;

  public override FollowerTaskType Type => FollowerTaskType.ReeducateDissenter;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockReactTasks => true;

  public override bool BlockTaskChanges => true;

  public FollowerTask_ReeducateDissenter(FollowerBrain targetFollower)
  {
    this.targetFollower = targetFollower;
  }

  public override int GetSubTaskCode() => 0;

  public override void TaskTick(float deltaGameTime)
  {
    if (PlayerFarming.Location == FollowerLocation.Base && (this.targetFollower == null || this.targetFollower.Info == null || this.targetFollower.Info.CursedState != Thought.None))
      return;
    this.End();
  }

  public void StateChange(FollowerTaskState oldState, FollowerTaskState newState)
  {
    if (oldState != FollowerTaskState.GoingTo || newState != FollowerTaskState.Doing)
      return;
    if ((double) Vector3.Distance(this.Brain.LastPosition, this.targetFollower.LastPosition) > 3.0)
    {
      this.End();
    }
    else
    {
      Follower target = (Follower) null;
      Follower enforcer = (Follower) null;
      foreach (Follower follower in FollowerManager.FollowersAtLocation(FollowerLocation.Base))
      {
        if (follower.Brain.Info.ID == this.targetFollower.Info.ID)
          target = follower;
        else if (follower.Brain.Info.ID == this._brain.Info.ID)
          enforcer = follower;
      }
      if ((bool) (Object) target && (bool) (Object) enforcer && enforcer.gameObject.activeInHierarchy)
      {
        enforcer.StartCoroutine((IEnumerator) this.ReeducateIE(target, enforcer));
      }
      else
      {
        this.Reeducate();
        if (this.targetFollower.CurrentTaskType != FollowerTaskType.Imprisoned)
          this.targetFollower.CompleteCurrentTask();
        this._brain.CompleteCurrentTask();
      }
    }
  }

  public override void OnStart()
  {
    base.OnStart();
    this.OnFollowerTaskStateChanged = this.OnFollowerTaskStateChanged + new FollowerTask.FollowerTaskDelegate(this.StateChange);
    if (this.targetFollower != null && this.targetFollower.CurrentTaskType != FollowerTaskType.Imprisoned)
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
    if (this.targetFollower != null && this.targetFollower.CurrentTaskType != FollowerTaskType.Imprisoned)
      this.targetFollower.CompleteCurrentTask();
    Interaction component = (Interaction) follower.GetComponent<interaction_FollowerInteraction>();
    if (!(bool) (Object) component)
      return;
    component.enabled = true;
  }

  public override void SimCleanup(SimFollower simFollower)
  {
    base.SimCleanup(simFollower);
    if (this.targetFollower == null || this.targetFollower.CurrentTaskType == FollowerTaskType.Imprisoned)
      return;
    this.targetFollower.CompleteCurrentTask();
  }

  public IEnumerator ReeducateIE(Follower target, Follower enforcer)
  {
    if (target.State.CURRENT_STATE == StateMachine.State.Dancing)
    {
      enforcer.Brain.CompleteCurrentTask();
    }
    else
    {
      Interaction interaction = (Interaction) enforcer.GetComponent<interaction_FollowerInteraction>();
      if ((bool) (Object) interaction)
        interaction.enabled = false;
      target.FacePosition(enforcer.transform.position);
      target.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
      enforcer.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
      int num1 = 1;
      if (((double) target.Brain.Stats.Reeducation + 7.5) / 100.0 >= 1.0)
        num1 = 3;
      else if (((double) target.Brain.Stats.Reeducation + 7.5) / 100.0 > 0.5)
        num1 = 2;
      if (target.Brain.CurrentTaskType == FollowerTaskType.Imprisoned)
      {
        double num2 = (double) target.SetBodyAnimation("Prison/stocks-reeducate", false);
        target.AddBodyAnimation("Prison/stocks", false, 0.0f);
      }
      else
      {
        double num3 = (double) target.SetBodyAnimation("reeducate-" + num1.ToString(), false);
        target.AddBodyAnimation("idle", true, 0.0f);
      }
      double num4 = (double) enforcer.SetBodyAnimation("Disciple/reeducate", false);
      target.State.facingAngle = Utils.GetAngle(target.transform.position, enforcer.transform.position);
      enforcer.State.facingAngle = Utils.GetAngle(enforcer.transform.position, target.transform.position);
      float time = 7.8f;
      yield return (object) new WaitForSeconds(time / 3f);
      yield return (object) new WaitForSeconds(time / 3f);
      this.Reeducate();
      AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", target.transform.position);
      BiomeConstants.Instance.EmitHeartPickUpVFX(target.transform.position, 0.0f, "red", "burst_big", false);
      yield return (object) new WaitForSeconds(time / 3f);
      if ((bool) (Object) interaction)
        interaction.enabled = true;
      if (target.Brain.CurrentTaskType != FollowerTaskType.Imprisoned)
        target.Brain.CompleteCurrentTask();
      enforcer.Brain.CompleteCurrentTask();
    }
  }

  public void Reeducate()
  {
    this.targetFollower.Stats.Reeducation -= 7.5f;
    if (this.targetFollower.CurrentTaskType == FollowerTaskType.Imprisoned)
      this.targetFollower.Stats.Reeducation -= 7.5f;
    if ((double) this.targetFollower.Stats.Reeducation > 0.0 && (double) this.targetFollower.Stats.Reeducation < 2.0)
      this.targetFollower.Stats.Reeducation = 0.0f;
    this.Brain._directInfoAccess.FollowersReeducatedToday.Add(this.targetFollower.Info.ID);
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    if (this.targetFollower == null || this.targetFollower.Info == null)
    {
      this.Abort();
      return Vector3.zero;
    }
    if (!((Object) follower != (Object) null))
      return this.targetFollower.LastPosition;
    Follower followerById = FollowerManager.FindFollowerByID(this.targetFollower.Info.ID);
    if (!((Object) followerById == (Object) null))
      return followerById.transform.position + Vector3.right * ((double) followerById.transform.position.x < (double) follower.transform.position.x ? 1.5f : -1.5f);
    this.Abort();
    return Vector3.zero;
  }
}
