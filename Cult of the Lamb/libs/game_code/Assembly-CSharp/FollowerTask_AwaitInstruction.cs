// Decompiled with JetBrains decompiler
// Type: FollowerTask_AwaitInstruction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class FollowerTask_AwaitInstruction : FollowerTask
{
  public const float SPEECH_DURATION_GAME_MINUTES_MIN = 45f;
  public const float SPEECH_DURATION_GAME_MINUTES_MAX = 55f;
  public const float IDLE_DURATION_GAME_MINUTES_MIN = 20f;
  public const float IDLE_DURATION_GAME_MINUTES_MAX = 30f;
  public bool _readyForSpeech = true;
  public float _gameTimeToNextStateUpdate;
  public float _speechDurationRemaining;
  public Coroutine _dissentBubbleCoroutine;

  public override FollowerTaskType Type => FollowerTaskType.AwaitInstructions;

  public override FollowerLocation Location => this._brain.HomeLocation;

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
        this._gameTimeToNextStateUpdate = Random.Range(20f, 30f);
        this._readyForSpeech = true;
      }
      else
      {
        if (!this._readyForSpeech)
          return;
        this.SetState(FollowerTaskState.Doing);
        this._speechDurationRemaining = Random.Range(45f, 55f);
        this._readyForSpeech = false;
      }
    }
    else
    {
      if (this._state != FollowerTaskState.Doing)
        return;
      Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
      if ((Object) followerById != (Object) null && (Object) PlayerFarming.Instance != (Object) null)
        followerById.FacePosition(PlayerFarming.Instance.transform.position);
      if ((double) (this._speechDurationRemaining -= deltaGameTime) > 0.0)
        return;
      this.SetState(FollowerTaskState.Idle);
    }
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    return follower.transform.position + (Vector3) Random.insideUnitCircle * 3f;
  }

  public override void OnDoingBegin(Follower follower)
  {
    double num = (double) follower.SetBodyAnimation("attention", true);
    this._dissentBubbleCoroutine = follower.StartCoroutine((IEnumerator) this.DissentBubbleRoutine(follower));
  }

  public override void OnDoingEnd(Follower follower)
  {
    double num = (double) follower.SetBodyAnimation(follower.AnimIdle, true);
    if (this._dissentBubbleCoroutine == null)
      return;
    follower.StopCoroutine(this._dissentBubbleCoroutine);
    this._dissentBubbleCoroutine = (Coroutine) null;
    follower.WorshipperBubble.Close();
  }

  public IEnumerator DissentBubbleRoutine(Follower follower)
  {
    float bubbleTimer = 0.3f;
    while (true)
    {
      if ((double) (bubbleTimer -= Time.deltaTime) < 0.0 && (double) this._speechDurationRemaining > 6.0)
      {
        follower.WorshipperBubble.Play(WorshipperBubble.SPEECH_TYPE.HELP);
        bubbleTimer = (float) (4 + Random.Range(0, 2));
      }
      yield return (object) null;
    }
  }
}
