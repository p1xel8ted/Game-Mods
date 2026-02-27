// Decompiled with JetBrains decompiler
// Type: FollowerTask_DissentListen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class FollowerTask_DissentListen : FollowerTask
{
  private int _dissentingFollowerID;
  private FollowerTask_Dissent _dissenterTask;

  public override FollowerTaskType Type => FollowerTaskType.DissentListen;

  public override FollowerLocation Location => this._dissenterTask.Location;

  public override bool BlockTaskChanges => true;

  private bool argueAgainst => (double) this._brain.Stats.Happiness >= 80.0;

  public FollowerTask_DissentListen(int dissentingFollowerID)
  {
    this._dissentingFollowerID = dissentingFollowerID;
    this._dissenterTask = FollowerBrain.FindBrainByID(this._dissentingFollowerID).CurrentTask as FollowerTask_Dissent;
  }

  protected override int GetSubTaskCode() => this._dissentingFollowerID;

  protected override void OnStart()
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

  protected override void OnComplete()
  {
    if (this._dissenterTask == null)
      return;
    FollowerTask_Dissent dissenterTask = this._dissenterTask;
    dissenterTask.OnFollowerTaskStateChanged = dissenterTask.OnFollowerTaskStateChanged - new FollowerTask.FollowerTaskDelegate(this.OnTaskStateChanged);
  }

  protected override void TaskTick(float deltaGameTime)
  {
  }

  private void OnTaskStateChanged(FollowerTaskState oldState, FollowerTaskState newState)
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
      followerById.StartCoroutine((IEnumerator) this.DelayReaction(followerById));
    else
      this.End();
  }

  private IEnumerator DelayReaction(Follower follower)
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

  protected override Vector3 UpdateDestination(Follower follower)
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
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    Follower dissenter = this.FindDissenter();
    follower.State.facingAngle = Utils.GetAngle(follower.transform.position, dissenter.transform.position);
    double num = (double) follower.SetBodyAnimation("Dissenters/dissenter-listening", true);
    this._dissenterTask.OnDissentBubble += new Action<WorshipperBubble.SPEECH_TYPE>(this.OnDissentBubble);
  }

  public override void OnDoingEnd(Follower follower)
  {
    follower.State.CURRENT_STATE = StateMachine.State.Idle;
    this._dissenterTask.OnDissentBubble -= new Action<WorshipperBubble.SPEECH_TYPE>(this.OnDissentBubble);
  }

  public override void SimDoingEnd(SimFollower simFollower) => base.SimDoingEnd(simFollower);

  protected override void OnEnd() => base.OnEnd();

  private void OnDissentBubble(WorshipperBubble.SPEECH_TYPE bubbleType)
  {
    Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    if (!(bool) (UnityEngine.Object) followerById)
      return;
    followerById.StartCoroutine((IEnumerator) this.DelayBubble(bubbleType, followerById));
  }

  private IEnumerator DelayBubble(WorshipperBubble.SPEECH_TYPE bubbleType, Follower self)
  {
    yield return (object) new WaitForSeconds(UnityEngine.Random.Range(0.2f, 0.4f));
    bubbleType = this.argueAgainst ? WorshipperBubble.SPEECH_TYPE.DISSENTARGUE : bubbleType;
    self.WorshipperBubble.Play(bubbleType);
  }

  private Follower FindDissenter() => FollowerManager.FindFollowerByID(this._dissentingFollowerID);
}
