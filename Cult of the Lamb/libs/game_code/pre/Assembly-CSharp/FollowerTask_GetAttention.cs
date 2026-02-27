// Decompiled with JetBrains decompiler
// Type: FollowerTask_GetAttention
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class FollowerTask_GetAttention : FollowerTask
{
  private float _updateDestination;
  private float _giveUpTimer;
  private bool CanGiveUp = true;
  private float startTimeStamp;
  private const float MaxIgnoreTime = 240f;
  private Follower _follower;
  public Follower.ComplaintType ComplaintType;
  private Coroutine _dissentBubbleCoroutine;

  public override FollowerTaskType Type => FollowerTaskType.GetPlayerAttention;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockReactTasks => true;

  public override bool BlockTaskChanges => true;

  public bool AutoInteract { get; private set; }

  public FollowerTask_GetAttention(Follower.ComplaintType ComplaintType, bool CanGiveUp = true)
  {
    this.ComplaintType = ComplaintType;
    this.CanGiveUp = CanGiveUp;
  }

  protected override int GetSubTaskCode() => 0;

  protected override void OnArrive() => this.SetState(FollowerTaskState.Idle);

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Conversations/walkpast-nice");
    this._dissentBubbleCoroutine = follower.StartCoroutine((IEnumerator) this.DissentBubbleRoutine(follower));
    this.startTimeStamp = TimeManager.TotalElapsedGameTime + 240f;
    this._follower = follower;
  }

  protected override void TaskTick(float deltaGameTime)
  {
    if (PlayerFarming.Location != this.Location)
    {
      this.End();
    }
    else
    {
      if (this._state == FollowerTaskState.GoingTo && this._currentDestination.HasValue)
      {
        if ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
          return;
        Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
        if ((UnityEngine.Object) followerById == (UnityEngine.Object) null || (UnityEngine.Object) followerById.transform == (UnityEngine.Object) null)
          return;
        this._updateDestination -= deltaGameTime;
        if ((double) this._updateDestination <= 0.0)
        {
          this.RecalculateDestination();
          this._updateDestination = 0.5f;
        }
      }
      else if (this._state == FollowerTaskState.Idle)
      {
        PlayerFarming instance = PlayerFarming.Instance;
        if ((UnityEngine.Object) instance == (UnityEngine.Object) null)
          return;
        Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
        if ((UnityEngine.Object) followerById == (UnityEngine.Object) null)
          return;
        float num = Vector3.Distance(instance.transform.position, followerById.transform.position);
        if ((double) num > 3.0)
        {
          this.ClearDestination();
          this.SetState(FollowerTaskState.GoingTo);
        }
        else
        {
          followerById.FacePosition(instance.transform.position);
          if ((double) num <= 2.0 && (double) TimeManager.TotalElapsedGameTime > (double) this.startTimeStamp && (double) this.startTimeStamp != -1.0 && this.ComplaintType == Follower.ComplaintType.GiveOnboarding && (UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null && (PlayerFarming.Instance.state.CURRENT_STATE == StateMachine.State.Idle || PlayerFarming.Instance.state.CURRENT_STATE == StateMachine.State.Moving))
            this.AutoInteract = true;
        }
      }
      if (this._dissentBubbleCoroutine == null && (bool) (UnityEngine.Object) this._follower)
        this._dissentBubbleCoroutine = this._follower.WorshipperBubble.StartCoroutine((IEnumerator) this.DissentBubbleRoutine(this._follower));
      if (this._state == FollowerTaskState.Doing || (double) (this._giveUpTimer += deltaGameTime) < 60.0 || !this.CanGiveUp)
        return;
      this.EndInAnger();
    }
  }

  private void EndInAnger()
  {
    Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    if (this._dissentBubbleCoroutine != null)
    {
      followerById.WorshipperBubble.StopCoroutine(this._dissentBubbleCoroutine);
      this._dissentBubbleCoroutine = (Coroutine) null;
      followerById.WorshipperBubble.Close();
    }
    this.SetState(FollowerTaskState.Doing);
    followerById.TimedAnimation("tantrum", 3.2f, new System.Action(((FollowerTask) this).End));
  }

  protected override Vector3 UpdateDestination(Follower follower)
  {
    return (UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null ? PlayerFarming.Instance.transform.position + (follower.transform.position - PlayerFarming.Instance.transform.position).normalized * 2f : follower.transform.position;
  }

  private void UndoStateAnimationChanges(Follower follower)
  {
    SimpleSpineAnimator.SpineChartacterAnimationData animationData = follower.SimpleAnimator.GetAnimationData(StateMachine.State.Moving);
    animationData.Animation = animationData.DefaultAnimation;
    follower.ResetStateAnimations();
  }

  public override void OnIdleBegin(Follower follower)
  {
    base.OnIdleBegin(follower);
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    double num = (double) follower.SetBodyAnimation("attention", true);
  }

  public override void OnIdleEnd(Follower follower)
  {
    base.OnIdleEnd(follower);
    double num = (double) follower.SetBodyAnimation(follower.AnimIdle, true);
  }

  private IEnumerator DissentBubbleRoutine(Follower follower)
  {
    float bubbleTimer = 0.3f;
    while (true)
    {
      if ((double) (bubbleTimer -= Time.deltaTime) < 0.0)
      {
        WorshipperBubble.SPEECH_TYPE Type = WorshipperBubble.SPEECH_TYPE.HELP;
        follower.WorshipperBubble.gameObject.SetActive(true);
        follower.WorshipperBubble.Play(Type);
        bubbleTimer = (float) (4 + UnityEngine.Random.Range(0, 2));
      }
      yield return (object) null;
    }
  }

  protected override void OnComplete()
  {
    TimeManager.TimeSinceLastComplaint = 0.0f;
    this.startTimeStamp = -1f;
    this.AutoInteract = false;
    base.OnComplete();
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    this.UndoStateAnimationChanges(follower);
    if (this._dissentBubbleCoroutine == null)
      return;
    follower.WorshipperBubble.StopCoroutine(this._dissentBubbleCoroutine);
    this._dissentBubbleCoroutine = (Coroutine) null;
    follower.WorshipperBubble.Close();
  }
}
