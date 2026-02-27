// Decompiled with JetBrains decompiler
// Type: FollowerTask_GetAttention
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerTask_GetAttention : FollowerTask
{
  public float _updateDestination;
  public float _giveUpTimer;
  public bool CanGiveUp = true;
  public float startTimeStamp;
  public const float MaxIgnoreTime = 240f;
  public bool showSpeechBubble = true;
  [CompilerGenerated]
  public bool \u003CAutoInteract\u003Ek__BackingField;
  public Follower _follower;
  public Follower.ComplaintType ComplaintType;
  [CompilerGenerated]
  public string \u003CTerm\u003Ek__BackingField = "";
  public Coroutine _dissentBubbleCoroutine;

  public override FollowerTaskType Type => FollowerTaskType.GetPlayerAttention;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockReactTasks => true;

  public override bool BlockTaskChanges => true;

  public bool AutoInteract
  {
    get => this.\u003CAutoInteract\u003Ek__BackingField;
    set => this.\u003CAutoInteract\u003Ek__BackingField = value;
  }

  public string Term
  {
    get => this.\u003CTerm\u003Ek__BackingField;
    set => this.\u003CTerm\u003Ek__BackingField = value;
  }

  public PlayerFarming PlayerToFollow
  {
    get
    {
      if ((UnityEngine.Object) this._follower != (UnityEngine.Object) null && this._follower.gameObject.activeInHierarchy)
      {
        PlayerFarming closestPlayer = PlayerFarming.FindClosestPlayer(this._follower.transform.position, true, true);
        if ((UnityEngine.Object) closestPlayer != (UnityEngine.Object) null)
          return closestPlayer;
      }
      return PlayerFarming.Instance;
    }
  }

  public FollowerTask_GetAttention(
    Follower.ComplaintType ComplaintType,
    bool CanGiveUp = true,
    string term = "")
  {
    this.ComplaintType = ComplaintType;
    this.CanGiveUp = CanGiveUp;
    this.Term = term;
    SimulationManager.onPause += new SimulationManager.OnPause(this.OnSimulationPause);
    SimulationManager.onUnPause += new SimulationManager.OnUnPause(this.OnSimulationUnPause);
  }

  public void OnSimulationPause()
  {
    if (!(bool) (UnityEngine.Object) this._follower)
      return;
    this._follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "idle");
    this.SetState(FollowerTaskState.Wait);
  }

  public void OnSimulationUnPause() => this.SetState(FollowerTaskState.Idle);

  public override int GetSubTaskCode() => 0;

  public override void OnArrive() => this.SetState(FollowerTaskState.Idle);

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, this.GetMovingAnimation());
    this._dissentBubbleCoroutine = follower.StartCoroutine(this.DissentBubbleRoutine(follower));
    this.startTimeStamp = TimeManager.TotalElapsedGameTime + 240f;
    this._follower = follower;
    if (!Interaction_Daycare.IsInDaycare(follower.Brain.Info.ID))
      return;
    follower.Interaction_FollowerInteraction.Interactable = true;
    Interaction_Daycare.RemoveFromDaycare(follower.Brain.Info.ID);
  }

  public override void TaskTick(float deltaGameTime)
  {
    if (PlayerFarming.Location != this.Location)
      this.End();
    else if (TwitchHelpHinder.Active)
      this.Abort();
    else if (UIDrumMinigameOverlayController.IsPlaying && this._brain.Info.CursedState == Thought.Child)
    {
      this.Abort();
    }
    else
    {
      if (this._state == FollowerTaskState.GoingTo && this._currentDestination.HasValue)
      {
        if ((UnityEngine.Object) this.PlayerToFollow == (UnityEngine.Object) null)
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
        PlayerFarming playerToFollow = this.PlayerToFollow;
        if ((UnityEngine.Object) playerToFollow == (UnityEngine.Object) null)
          return;
        Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
        if ((UnityEngine.Object) followerById == (UnityEngine.Object) null)
          return;
        float num = Vector3.Distance(playerToFollow.transform.position, followerById.transform.position);
        if ((double) num > 3.0)
        {
          this.ClearDestination();
          this.SetState(FollowerTaskState.GoingTo);
        }
        else
        {
          followerById.FacePosition(playerToFollow.transform.position);
          if ((double) num <= 2.0)
          {
            if ((double) TimeManager.TotalElapsedGameTime > (double) this.startTimeStamp && (double) this.startTimeStamp != -1.0 && this.ComplaintType == Follower.ComplaintType.GiveOnboarding && (UnityEngine.Object) this.PlayerToFollow != (UnityEngine.Object) null && (this.PlayerToFollow.state.CURRENT_STATE == StateMachine.State.Idle || this.PlayerToFollow.state.CURRENT_STATE == StateMachine.State.Moving))
              this.AutoInteract = true;
            if (this.ComplaintType == Follower.ComplaintType.ShowTwitchMessage)
              FollowerManager.FindFollowerByID(this._brain.Info.ID)?.ShowTwitchMessage();
          }
        }
      }
      if (this._dissentBubbleCoroutine == null && (bool) (UnityEngine.Object) this._follower && this.showSpeechBubble)
        this._dissentBubbleCoroutine = this._follower.WorshipperBubble.StartCoroutine(this.DissentBubbleRoutine(this._follower));
      if (this._state == FollowerTaskState.Doing || (double) (this._giveUpTimer += deltaGameTime) < 60.0 || !this.CanGiveUp)
        return;
      this.EndInAnger();
    }
  }

  public void EndInAnger()
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

  public override Vector3 UpdateDestination(Follower follower)
  {
    return (UnityEngine.Object) this.PlayerToFollow != (UnityEngine.Object) null ? this.PlayerToFollow.transform.position + (follower.transform.position - this.PlayerToFollow.transform.position).normalized * 2f : follower.transform.position;
  }

  public void UndoStateAnimationChanges(Follower follower)
  {
    SimpleSpineAnimator.SpineChartacterAnimationData animationData = follower.SimpleAnimator.GetAnimationData(StateMachine.State.Moving);
    animationData.Animation = animationData.DefaultAnimation;
    follower.ResetStateAnimations();
  }

  public override void OnIdleBegin(Follower follower)
  {
    base.OnIdleBegin(follower);
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    double num = (double) follower.SetBodyAnimation(this.GetAttentionAnimation(), true);
  }

  public override void OnIdleEnd(Follower follower)
  {
    base.OnIdleEnd(follower);
    double num = (double) follower.SetBodyAnimation(follower.AnimIdle, true);
  }

  public IEnumerator DissentBubbleRoutine(Follower follower)
  {
    float bubbleTimer = 0.3f;
    while (true)
    {
      if ((double) (bubbleTimer -= Time.deltaTime) < 0.0)
      {
        WorshipperBubble.SPEECH_TYPE Type = this.ComplaintType == Follower.ComplaintType.ShowTwitchMessage ? WorshipperBubble.SPEECH_TYPE.TWITCH : WorshipperBubble.SPEECH_TYPE.HELP;
        follower.WorshipperBubble.gameObject.SetActive(true);
        follower.WorshipperBubble.Play(Type);
        bubbleTimer = (float) (4 + UnityEngine.Random.Range(0, 2));
      }
      yield return (object) null;
    }
  }

  public override void OnComplete()
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
    if (this._dissentBubbleCoroutine != null)
    {
      follower.WorshipperBubble.StopCoroutine(this._dissentBubbleCoroutine);
      this._dissentBubbleCoroutine = (Coroutine) null;
      follower.WorshipperBubble.Close();
    }
    SimulationManager.onPause -= new SimulationManager.OnPause(this.OnSimulationPause);
    SimulationManager.onUnPause -= new SimulationManager.OnUnPause(this.OnSimulationUnPause);
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

  public string GetAttentionAnimation()
  {
    string attentionAnimation = "attention";
    if (this.Brain.Info.ID == 100000)
      attentionAnimation = "meditate-enlightenment";
    else if (this.Brain.CanFreeze() && SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter)
      attentionAnimation = "Snow/wave-cold";
    return attentionAnimation;
  }

  public string GetMovingAnimation()
  {
    string movingAnimation = "Conversations/walkpast-nice";
    if (this.Brain.Info.ID == 100000)
      movingAnimation = "meditate-enlightenment";
    else if (this.Brain.CanFreeze() && SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter)
      movingAnimation = "Snow/walkup-cold";
    return movingAnimation;
  }
}
