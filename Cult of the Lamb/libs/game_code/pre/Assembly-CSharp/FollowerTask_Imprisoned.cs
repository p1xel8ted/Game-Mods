// Decompiled with JetBrains decompiler
// Type: FollowerTask_Imprisoned
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class FollowerTask_Imprisoned : FollowerTask
{
  private Coroutine _dissentBubbleCoroutine;
  private int _prisonID;
  private StructureBrain _prison;

  public override FollowerTaskType Type => FollowerTaskType.Imprisoned;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool DisablePickUpInteraction => true;

  public override int UsingStructureID => this._prisonID;

  public override bool BlockReactTasks => true;

  public override bool BlockTaskChanges => true;

  public override bool BlockSermon => true;

  public FollowerTask_Imprisoned(int prisonID)
  {
    this._prisonID = prisonID;
    this._prison = StructureManager.GetStructureByID<StructureBrain>(this._prisonID);
  }

  protected override int GetSubTaskCode() => this._prisonID;

  protected override void OnStart()
  {
    StructureManager.GetStructureByID<StructureBrain>(this._prisonID);
    this.SetState(FollowerTaskState.GoingTo);
  }

  protected override void OnEnd()
  {
    StructureBrain structureById = StructureManager.GetStructureByID<StructureBrain>(this._prisonID);
    Follower followerById = FollowerManager.FindFollowerByID(structureById.Data.FollowerID);
    if ((UnityEngine.Object) followerById != (UnityEngine.Object) null)
    {
      followerById.Interaction_FollowerInteraction.Interactable = true;
      followerById.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "idle");
    }
    Debug.Log((object) "END!");
    structureById.Data.FollowerID = -1;
    base.OnEnd();
  }

  protected override void OnAbort() => this.OnEnd();

  private void OnReeducationComplete()
  {
    Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    followerById.Brain.Stats.OnReeducationComplete -= new System.Action(this.OnReeducationComplete);
    followerById.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Prison/stocks-ready");
    if (this._dissentBubbleCoroutine != null)
      followerById.StopCoroutine(this._dissentBubbleCoroutine);
    this._dissentBubbleCoroutine = followerById.StartCoroutine((IEnumerator) this.DissentBubbleRoutine(followerById));
  }

  protected override Vector3 UpdateDestination(Follower follower)
  {
    Prison prison = this.FindPrison();
    return !((UnityEngine.Object) prison == (UnityEngine.Object) null) ? prison.PrisonerLocation.position : this._prison.Data.Position;
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    Vector3 vector3 = this.UpdateDestination(follower);
    if (follower.transform.position != vector3)
      follower.transform.position = vector3;
    if (this.State == FollowerTaskState.Doing)
    {
      if ((double) this._brain.Stats.Reeducation <= 0.0)
      {
        follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Prison/stocks-ready");
        if (this._dissentBubbleCoroutine != null)
          follower.StopCoroutine(this._dissentBubbleCoroutine);
        this._dissentBubbleCoroutine = follower.StartCoroutine((IEnumerator) this.DissentBubbleRoutine(follower));
      }
      else
        follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Prison/stocks");
      if (this._brain.Stats.IsStarving)
        follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Prison/stocks-hungry");
    }
    if (!(bool) (UnityEngine.Object) follower)
      return;
    follower.Interaction_FollowerInteraction.Interactable = false;
    follower.Brain.Stats.OnReeducationComplete += new System.Action(this.OnReeducationComplete);
  }

  public override void OnDoingBegin(Follower follower)
  {
    if ((double) this._brain.Stats.Reeducation <= 0.0)
    {
      if (this._dissentBubbleCoroutine != null)
        follower.StopCoroutine(this._dissentBubbleCoroutine);
      this._dissentBubbleCoroutine = follower.StartCoroutine((IEnumerator) this.DissentBubbleRoutine(follower));
      follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Prison/stocks-ready");
    }
    else
      follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Prison/stocks");
    if (!this._brain.Stats.IsStarving)
      return;
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Prison/stocks-hungry");
  }

  public override void OnFinaliseBegin(Follower follower)
  {
    Prison prison = this.FindPrison();
    if ((UnityEngine.Object) prison != (UnityEngine.Object) null)
      follower.GoTo(prison.PrisonerExitLocation.transform.position, new System.Action(((FollowerTask) this).Complete));
    else
      this.Complete();
  }

  public override void OnFinaliseEnd(Follower follower)
  {
    base.OnFinaliseEnd(follower);
    follower.Brain.Stats.OnReeducationComplete -= new System.Action(this.OnReeducationComplete);
    if (this._dissentBubbleCoroutine == null)
      return;
    follower.StopCoroutine(this._dissentBubbleCoroutine);
    this._dissentBubbleCoroutine = (Coroutine) null;
    follower.WorshipperBubble.Close();
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    follower.Brain.Stats.OnReeducationComplete -= new System.Action(this.OnReeducationComplete);
    if (this._dissentBubbleCoroutine == null)
      return;
    follower.StopCoroutine(this._dissentBubbleCoroutine);
    this._dissentBubbleCoroutine = (Coroutine) null;
    follower.WorshipperBubble.Close();
  }

  private Prison FindPrison()
  {
    Prison prison1 = (Prison) null;
    foreach (Prison prison2 in Prison.Prisons)
    {
      if (prison2.StructureInfo.ID == this._prisonID)
      {
        prison1 = prison2;
        break;
      }
    }
    return prison1;
  }

  protected override float RestChange(float deltaGameTime) => 0.0f;

  protected override float ReeducationChange(float deltaGameTime) => 0.0f;

  private IEnumerator DissentBubbleRoutine(Follower follower)
  {
    float bubbleTimer = 0.3f;
    while (true)
    {
      if ((double) (bubbleTimer -= Time.deltaTime) < 0.0)
      {
        follower.WorshipperBubble.Play(WorshipperBubble.SPEECH_TYPE.READY);
        bubbleTimer = (float) (4 + UnityEngine.Random.Range(0, 2));
      }
      yield return (object) null;
    }
  }

  protected override void TaskTick(float deltaGameTime)
  {
    if ((double) this._brain.Stats.Starvation < 75.0 && (double) this._brain.Stats.Illness < 100.0 && !this._brain.DiedOfOldAge)
      return;
    this._brain.DiedOfStarvation = (double) this._brain.Stats.Starvation >= 75.0 && DataManager.Instance.OnboardingFinished;
    this._brain.DiedOfIllness = (double) this._brain.Stats.Illness >= 100.0 && DataManager.Instance.OnboardingFinished;
    this._brain.DiedInPrison = DataManager.Instance.OnboardingFinished;
    Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    if ((bool) (UnityEngine.Object) followerById)
    {
      followerById.DieWithAnimation("Prison/stocks-die", 3f, "Prison/stocks-dead", false, -1, this._brain.DiedOfIllness ? NotificationCentre.NotificationType.DiedFromIllness : NotificationCentre.NotificationType.DiedFromStarvation, new Action<GameObject>(this.SetPrisonID));
    }
    else
    {
      FollowerManager.FindSimFollowerByID(this._brain.Info.ID).Die(NotificationCentre.NotificationType.DiedFromStarvation, this._currentDestination.Value);
      this.SetPrisonID((GameObject) null);
    }
  }

  private void SetPrisonID(GameObject body)
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.FrameDelay((System.Action) (() => this._prison.Data.FollowerID = this._brain.Info.ID)));
  }

  private IEnumerator FrameDelay(System.Action callback)
  {
    yield return (object) new WaitForEndOfFrame();
    System.Action action = callback;
    if (action != null)
      action();
  }
}
