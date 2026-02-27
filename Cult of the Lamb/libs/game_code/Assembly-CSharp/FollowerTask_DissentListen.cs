// Decompiled with JetBrains decompiler
// Type: FollowerTask_DissentListen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class FollowerTask_DissentListen : FollowerTask
{
  public int _dissentingFollowerID;
  public FollowerTask_Dissent _dissenterTask;
  public Coroutine delayedReactionRoutine;

  public override FollowerTaskType Type => FollowerTaskType.DissentListen;

  public override FollowerLocation Location => this._dissenterTask.Location;

  public override bool BlockTaskChanges => true;

  public bool argueAgainst => (double) this._brain.Stats.Happiness >= 80.0;

  public FollowerTask_DissentListen(int dissentingFollowerID)
  {
    this._dissentingFollowerID = dissentingFollowerID;
    this._dissenterTask = FollowerBrain.FindBrainByID(this._dissentingFollowerID).CurrentTask as FollowerTask_Dissent;
  }

  public override int GetSubTaskCode() => this._dissentingFollowerID;

  public override void OnStart()
  {
    if (this._dissenterTask != null)
    {
      FollowerTask_Dissent dissenterTask = this._dissenterTask;
      dissenterTask.OnFollowerTaskStateChanged = dissenterTask.OnFollowerTaskStateChanged + new FollowerTask.FollowerTaskDelegate(this.OnTaskStateChanged);
      this.SetState(FollowerTaskState.GoingTo);
    }
    else
      this.End();
  }

  public override void OnComplete()
  {
    if (this._dissenterTask == null)
      return;
    FollowerTask_Dissent dissenterTask = this._dissenterTask;
    dissenterTask.OnFollowerTaskStateChanged = dissenterTask.OnFollowerTaskStateChanged - new FollowerTask.FollowerTaskDelegate(this.OnTaskStateChanged);
  }

  public override void TaskTick(float deltaGameTime)
  {
  }

  public void OnTaskStateChanged(FollowerTaskState oldState, FollowerTaskState newState)
  {
    if (newState == FollowerTaskState.Doing)
      return;
    if (!this.argueAgainst)
    {
      if (this._brain.HasTrait(FollowerTrait.TraitType.Zealous))
        this._brain.AddThought(Thought.ListenedToDissenterZealotTrait);
      else
        this._brain.AddThought(Thought.ListenedToDissenter);
    }
    Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    if ((UnityEngine.Object) followerById != (UnityEngine.Object) null && followerById.gameObject.activeInHierarchy)
      this.delayedReactionRoutine = followerById.StartCoroutine(this.DelayReaction(followerById));
    else
      this.End();
  }

  public IEnumerator DelayReaction(Follower follower)
  {
    FollowerTask_DissentListen taskDissentListen = this;
    yield return (object) new WaitForSeconds(UnityEngine.Random.Range(0.4f, 0.7f));
    if (taskDissentListen.argueAgainst)
      follower.TimedAnimation("Conversations/react-mean3", 2f, new System.Action(((FollowerTask) taskDissentListen).End), false);
    else if ((double) UnityEngine.Random.value < 0.5)
      follower.TimedAnimation((double) UnityEngine.Random.value < 0.5 ? "Reactions/react-worried1" : "Reactions/react-worried2", 1.9f, new System.Action(((FollowerTask) taskDissentListen).End), false);
    else
      follower.TimedAnimation("Reactions/react-non-believers", 1.9f, new System.Action(((FollowerTask) taskDissentListen).End), false);
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    Follower dissenter = this.FindDissenter();
    if (!(bool) (UnityEngine.Object) dissenter)
      return follower.Brain.LastPosition;
    float num = (float) UnityEngine.Random.Range(2, 3);
    float f = Utils.GetAngle(dissenter.transform.position, follower.transform.position) * ((float) Math.PI / 180f);
    return dissenter.transform.position + new Vector3(num * Mathf.Cos(f), num * Mathf.Sin(f));
  }

  public override void OnDoingBegin(Follower follower)
  {
    Follower dissenter = this.FindDissenter();
    if ((UnityEngine.Object) dissenter != (UnityEngine.Object) null)
    {
      follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
      follower.State.facingAngle = Utils.GetAngle(follower.transform.position, dissenter.transform.position);
      double num = (double) follower.SetBodyAnimation("Dissenters/dissenter-listening", true);
      this._dissenterTask.OnDissentBubble += new Action<WorshipperBubble.SPEECH_TYPE>(this.OnDissentBubble);
    }
    else
      this.End();
  }

  public override void OnDoingEnd(Follower follower)
  {
    follower.State.CURRENT_STATE = StateMachine.State.Idle;
    this._dissenterTask.OnDissentBubble -= new Action<WorshipperBubble.SPEECH_TYPE>(this.OnDissentBubble);
  }

  public override void SimDoingEnd(SimFollower simFollower) => base.SimDoingEnd(simFollower);

  public override void OnEnd()
  {
    base.OnEnd();
    if (this.delayedReactionRoutine == null)
      return;
    FollowerManager.FindFollowerByID(this._brain.Info.ID)?.StopCoroutine(this.delayedReactionRoutine);
  }

  public void OnDissentBubble(WorshipperBubble.SPEECH_TYPE bubbleType)
  {
    Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    if (!(bool) (UnityEngine.Object) followerById)
      return;
    followerById.StartCoroutine(this.DelayBubble(bubbleType, followerById));
  }

  public IEnumerator DelayBubble(WorshipperBubble.SPEECH_TYPE bubbleType, Follower self)
  {
    yield return (object) new WaitForSeconds(UnityEngine.Random.Range(0.2f, 0.4f));
    bubbleType = this.argueAgainst ? WorshipperBubble.SPEECH_TYPE.DISSENTARGUE : bubbleType;
    self.WorshipperBubble.Play(bubbleType);
  }

  public Follower FindDissenter() => FollowerManager.FindFollowerByID(this._dissentingFollowerID);
}
