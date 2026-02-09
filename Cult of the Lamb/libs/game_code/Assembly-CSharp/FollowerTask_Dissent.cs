// Decompiled with JetBrains decompiler
// Type: FollowerTask_Dissent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerTask_Dissent : FollowerTask
{
  public const float SPEECH_DURATION_GAME_MINUTES_MIN = 45f;
  public const float SPEECH_DURATION_GAME_MINUTES_MAX = 60f;
  public const float IDLE_DURATION_GAME_MINUTES_MIN = 10f;
  public const float IDLE_DURATION_GAME_MINUTES_MAX = 30f;
  public bool _readyForSpeech = true;
  public float _gameTimeToNextStateUpdate;
  public float _speechDurationRemaining;
  public Coroutine _dissentBubbleCoroutine;
  public Action<WorshipperBubble.SPEECH_TYPE> OnDissentBubble;

  public override FollowerTaskType Type => FollowerTaskType.Dissent;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override int GetSubTaskCode() => 0;

  public override void OnArrive() => this.SetState(FollowerTaskState.Idle);

  public override void TaskTick(float deltaGameTime)
  {
    if (this._state == FollowerTaskState.Idle)
    {
      this._gameTimeToNextStateUpdate -= deltaGameTime;
      if ((double) this._gameTimeToNextStateUpdate <= 0.0)
      {
        this.ClearDestination();
        this.SetState(FollowerTaskState.GoingTo);
        this._gameTimeToNextStateUpdate = UnityEngine.Random.Range(10f, 30f);
        this._readyForSpeech = true;
      }
      else
      {
        if (!this._readyForSpeech || (double) UnityEngine.Random.value >= 0.004999999888241291)
          return;
        this.SetState(FollowerTaskState.Doing);
        this._speechDurationRemaining = UnityEngine.Random.Range(45f, 60f);
        this._readyForSpeech = false;
      }
    }
    else
    {
      if (this._state != FollowerTaskState.Doing || (double) (this._speechDurationRemaining -= deltaGameTime) > 0.0)
        return;
      Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
      if ((UnityEngine.Object) followerById != (UnityEngine.Object) null)
        followerById.TimedAnimation("Reactions/react-determined1", 2f, (System.Action) (() => this.SetState(FollowerTaskState.Idle)), false);
      else
        this.SetState(FollowerTaskState.Idle);
    }
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    return TownCentre.RandomPositionInCachedTownCentre();
  }

  public override void OnDoingBegin(Follower follower)
  {
    double num = (double) follower.SetBodyAnimation("Dissenters/dissenter", true);
    this._dissentBubbleCoroutine = follower.StartCoroutine((IEnumerator) this.DissentBubbleRoutine(follower));
  }

  public override void OnDoingEnd(Follower follower)
  {
    double num = (double) follower.SetBodyAnimation(follower.AnimIdle, true);
    if (this._dissentBubbleCoroutine == null)
      return;
    follower.StopCoroutine(this._dissentBubbleCoroutine);
    this._dissentBubbleCoroutine = (Coroutine) null;
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    if (this._dissentBubbleCoroutine != null)
    {
      follower.StopCoroutine(this._dissentBubbleCoroutine);
      this._dissentBubbleCoroutine = (Coroutine) null;
      follower.WorshipperBubble.Close();
    }
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "idle");
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");
  }

  public IEnumerator DissentBubbleRoutine(Follower follower)
  {
    float bubbleTimer = 0.3f;
    while (true)
    {
      if ((double) (bubbleTimer -= Time.deltaTime) < 0.0 && (double) this._speechDurationRemaining > 6.0)
      {
        WorshipperBubble.SPEECH_TYPE Type = (WorshipperBubble.SPEECH_TYPE) (6 + UnityEngine.Random.Range(0, 3));
        follower.WorshipperBubble.Play(Type);
        bubbleTimer = (float) (4 + UnityEngine.Random.Range(0, 2));
        Action<WorshipperBubble.SPEECH_TYPE> onDissentBubble = this.OnDissentBubble;
        if (onDissentBubble != null)
          onDissentBubble(Type);
      }
      yield return (object) null;
    }
  }

  [CompilerGenerated]
  public void \u003CTaskTick\u003Eb__13_0() => this.SetState(FollowerTaskState.Idle);
}
