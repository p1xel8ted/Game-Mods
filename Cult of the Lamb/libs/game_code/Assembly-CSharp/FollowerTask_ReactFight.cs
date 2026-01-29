// Decompiled with JetBrains decompiler
// Type: FollowerTask_ReactFight
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerTask_ReactFight : FollowerTask
{
  public FollowerBrain targetFollower;
  public Coroutine _dissentBubbleCoroutine;
  public Follower follower;
  public bool showSpeechBubble = true;

  public override FollowerTaskType Type => FollowerTaskType.ReactFight;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockTaskChanges => true;

  public FollowerTask_ReactFight(FollowerBrain targetFollower)
  {
    this.targetFollower = targetFollower;
  }

  public override int GetSubTaskCode() => 0;

  public override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    this._dissentBubbleCoroutine = follower.StartCoroutine((IEnumerator) this.DissentBubbleRoutine(follower));
    this.follower = follower;
  }

  public override void TaskTick(float deltaGameTime)
  {
    if (this._brain.Location != PlayerFarming.Location || this.targetFollower.CurrentTaskType != FollowerTaskType.FightFollower || this.targetFollower.CurrentTask.State != FollowerTaskState.Doing)
      this.End();
    if (this._dissentBubbleCoroutine != null || !((UnityEngine.Object) this.follower != (UnityEngine.Object) null) || !this.showSpeechBubble || this.State != FollowerTaskState.Doing || this.targetFollower.CurrentTask.State != FollowerTaskState.Doing)
      return;
    this._dissentBubbleCoroutine = this.follower.WorshipperBubble.StartCoroutine((IEnumerator) this.DissentBubbleRoutine(this.follower));
  }

  public override void OnFinaliseBegin(Follower follower)
  {
    this.StopSpeechBubble(follower);
    follower.TimedAnimation("Reactions/react-laugh", 3.33333325f, (System.Action) (() => this.\u003C\u003En__0(follower)));
  }

  public override void OnAbort()
  {
    base.OnAbort();
    if (!((UnityEngine.Object) this.follower != (UnityEngine.Object) null))
      return;
    this.follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "idle");
    if (this._dissentBubbleCoroutine == null)
      return;
    this.follower.WorshipperBubble.StopCoroutine(this._dissentBubbleCoroutine);
    this._dissentBubbleCoroutine = (Coroutine) null;
    this.follower.WorshipperBubble.Close();
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    Vector3 vector3 = this.targetFollower.LastPosition;
    Follower followerById = FollowerManager.FindFollowerByID(this.targetFollower.Info.ID);
    if ((UnityEngine.Object) followerById != (UnityEngine.Object) null)
      vector3 = followerById.transform.position;
    return vector3 + (Vector3) UnityEngine.Random.insideUnitCircle * UnityEngine.Random.Range(2f, 3f);
  }

  public override void OnDoingBegin(Follower follower)
  {
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "cheer");
    follower.FacePosition(this.targetFollower.LastPosition);
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "idle");
    if (this._dissentBubbleCoroutine == null)
      return;
    follower.WorshipperBubble.StopCoroutine(this._dissentBubbleCoroutine);
    this._dissentBubbleCoroutine = (Coroutine) null;
    follower.WorshipperBubble.Close();
  }

  public IEnumerator DissentBubbleRoutine(Follower follower)
  {
    float bubbleTimer = 0.3f;
    while (true)
    {
      if ((double) (bubbleTimer -= Time.deltaTime) < 0.0)
      {
        WorshipperBubble.SPEECH_TYPE Type = WorshipperBubble.SPEECH_TYPE.ENEMIES;
        follower.WorshipperBubble.gameObject.SetActive(true);
        follower.WorshipperBubble.Play(Type);
        bubbleTimer = (float) (4 + UnityEngine.Random.Range(0, 2));
      }
      yield return (object) null;
    }
  }

  public void StopSpeechBubble(Follower follower)
  {
    if (this._dissentBubbleCoroutine == null)
      return;
    this.showSpeechBubble = false;
    follower.WorshipperBubble.StopCoroutine(this._dissentBubbleCoroutine);
    this._dissentBubbleCoroutine = (Coroutine) null;
    follower.WorshipperBubble.Close();
  }

  [CompilerGenerated]
  [DebuggerHidden]
  public void \u003C\u003En__0(Follower follower) => base.OnFinaliseBegin(follower);
}
