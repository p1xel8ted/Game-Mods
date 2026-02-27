// Decompiled with JetBrains decompiler
// Type: FollowerTask_AwaitInstruction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class FollowerTask_AwaitInstruction : FollowerTask
{
  private const float SPEECH_DURATION_GAME_MINUTES_MIN = 45f;
  private const float SPEECH_DURATION_GAME_MINUTES_MAX = 55f;
  private const float IDLE_DURATION_GAME_MINUTES_MIN = 20f;
  private const float IDLE_DURATION_GAME_MINUTES_MAX = 30f;
  private bool _readyForSpeech = true;
  private float _gameTimeToNextStateUpdate;
  private float _speechDurationRemaining;
  private Coroutine _dissentBubbleCoroutine;

  public override FollowerTaskType Type => FollowerTaskType.AwaitInstructions;

  public override FollowerLocation Location => this._brain.HomeLocation;

  protected override int GetSubTaskCode() => 0;

  protected override void OnArrive() => this.SetState(FollowerTaskState.Idle);

  protected override void TaskTick(float deltaGameTime)
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

  protected override Vector3 UpdateDestination(Follower follower)
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

  private IEnumerator DissentBubbleRoutine(Follower follower)
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
