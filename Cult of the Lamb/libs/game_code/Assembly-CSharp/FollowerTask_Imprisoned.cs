// Decompiled with JetBrains decompiler
// Type: FollowerTask_Imprisoned
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerTask_Imprisoned : FollowerTask
{
  public Coroutine _dissentBubbleCoroutine;
  public int _prisonID;
  public StructureBrain _prison;
  public float breakOutTimestamp;
  public bool isDeathInitated;
  public bool isBreakingOut;

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
    this.breakOutTimestamp = TimeManager.CurrentGameTime + (float) UnityEngine.Random.Range(240 /*0xF0*/, 720);
  }

  public override int GetSubTaskCode() => this._prisonID;

  public override void OnStart()
  {
    StructureManager.GetStructureByID<StructureBrain>(this._prisonID);
    this.SetState(FollowerTaskState.GoingTo);
  }

  public override void OnEnd()
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

  public override void OnAbort() => this.OnEnd();

  public void OnReeducationComplete()
  {
    Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    followerById.Brain.Stats.OnReeducationComplete -= new System.Action(this.OnReeducationComplete);
    if (this._brain.Info.ID == 99996 && this._brain._directInfoAccess.SozoBrainshed)
      return;
    followerById.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Prison/stocks-ready");
    if (this._dissentBubbleCoroutine != null)
      followerById.StopCoroutine(this._dissentBubbleCoroutine);
    this._dissentBubbleCoroutine = followerById.StartCoroutine((IEnumerator) this.DissentBubbleRoutine(followerById));
  }

  public override Vector3 UpdateDestination(Follower follower)
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
      if ((double) this._brain.Stats.Reeducation <= 0.0 && (this._brain.Info.ID != 99996 || !this._brain._directInfoAccess.SozoBrainshed))
      {
        follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Prison/stocks-ready");
        if (this._dissentBubbleCoroutine != null)
          follower.StopCoroutine(this._dissentBubbleCoroutine);
        this._dissentBubbleCoroutine = follower.StartCoroutine((IEnumerator) this.DissentBubbleRoutine(follower));
      }
      else
        follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Prison/stocks");
      if (this._brain.Info.CursedState == Thought.BecomeStarving)
        follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Prison/stocks-hungry");
    }
    if (!(bool) (UnityEngine.Object) follower)
      return;
    follower.Interaction_FollowerInteraction.Interactable = false;
    follower.Brain.Stats.OnReeducationComplete -= new System.Action(this.OnReeducationComplete);
    follower.Brain.Stats.OnReeducationComplete += new System.Action(this.OnReeducationComplete);
  }

  public override void OnDoingBegin(Follower follower)
  {
    if ((double) this._brain.Stats.Reeducation <= 0.0 && (this._brain.Info.ID != 99996 || !this._brain._directInfoAccess.SozoBrainshed))
    {
      if (this._dissentBubbleCoroutine != null)
        follower.StopCoroutine(this._dissentBubbleCoroutine);
      this._dissentBubbleCoroutine = follower.StartCoroutine((IEnumerator) this.DissentBubbleRoutine(follower));
      follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Prison/stocks-ready");
    }
    else
      follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Prison/stocks");
    if (this._brain.Info.CursedState != Thought.BecomeStarving)
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
    follower.Interaction_FollowerInteraction.Interactable = true;
    follower.Brain.Stats.OnReeducationComplete -= new System.Action(this.OnReeducationComplete);
    if (this._dissentBubbleCoroutine == null)
      return;
    follower.StopCoroutine(this._dissentBubbleCoroutine);
    this._dissentBubbleCoroutine = (Coroutine) null;
    follower.WorshipperBubble.Close();
  }

  public Prison FindPrison()
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

  public override float RestChange(float deltaGameTime) => 0.0f;

  public override float ReeducationChange(float deltaGameTime) => 0.0f;

  public IEnumerator DissentBubbleRoutine(Follower follower)
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

  public override void TaskTick(float deltaGameTime)
  {
    if (this.isBreakingOut)
      return;
    if (!this.isDeathInitated && ((double) this._brain.Stats.Starvation >= 75.0 || (double) this._brain.Stats.Illness >= 100.0 || this._brain.DiedOfOldAge || this._brain.FrozeToDeath || this._brain.DiedFromOverheating || this._brain.BurntToDeath || SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter && this._brain._directInfoAccess.IsSnowman && this._brain._directInfoAccess.Necklace != InventoryItem.ITEM_TYPE.Necklace_Winter && !this._brain.HasTrait(FollowerTrait.TraitType.InfusibleSnowman)))
    {
      this._brain.DiedOfStarvation = (double) this._brain.Stats.Starvation >= 75.0 && DataManager.Instance.OnboardingFinished;
      this._brain.DiedOfIllness = (double) this._brain.Stats.Illness >= 100.0 && DataManager.Instance.OnboardingFinished;
      this._brain.FrozeToDeath = (double) this._brain.Stats.Freezing >= 100.0 && DataManager.Instance.OnboardingFinished;
      this._brain.DiedFromOverheating = (double) this._brain.Stats.Overheating >= 100.0 && DataManager.Instance.OnboardingFinished;
      this._brain.BurntToDeath = (double) this._brain.Stats.Aflame >= 100.0 && DataManager.Instance.OnboardingFinished;
      this._brain.DiedInPrison = DataManager.Instance.OnboardingFinished;
      Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
      if ((bool) (UnityEngine.Object) followerById)
      {
        string animation = "Prison/stocks-die";
        string deadAnimation = "Prison/stocks-dead";
        NotificationCentre.NotificationType deathNotificationType = NotificationCentre.NotificationType.DiedFromIllness;
        if (this._brain.DiedOfStarvation)
          deathNotificationType = NotificationCentre.NotificationType.DiedFromStarvation;
        else if (this._brain.FrozeToDeath)
        {
          deathNotificationType = NotificationCentre.NotificationType.FrozeToDeath;
          animation = "Prison/stocks-die-frozen";
          deadAnimation = "Prison/stocks-dead-frozen";
        }
        else if (this._brain.DiedFromOverheating)
        {
          deathNotificationType = NotificationCentre.NotificationType.DiedFromOverheating;
          animation = "Prison/stocks-die-overheated";
          deadAnimation = "Prison/stocks-dead-overheated";
        }
        else if (this._brain._directInfoAccess.IsSnowman)
          deathNotificationType = NotificationCentre.NotificationType.MeltedToDeath;
        followerById.DieWithAnimation(animation, deadAnimation, dir: -1, deathNotificationType: deathNotificationType, callback: new Action<GameObject>(this.SetPrisonID));
      }
      else
      {
        FollowerManager.FindSimFollowerByID(this._brain.Info.ID).Die(NotificationCentre.NotificationType.DiedFromStarvation, this._currentDestination.Value);
        this.SetPrisonID((GameObject) null);
      }
      this.isDeathInitated = true;
    }
    if (!this._brain.HasTrait(FollowerTrait.TraitType.Unlawful) || (double) TimeManager.CurrentGameTime <= (double) this.breakOutTimestamp || this._prison == null || PlayerFarming.Location != FollowerLocation.Base || (double) Vector3.Distance(PlayerFarming.Instance.transform.position, this._prison.Data.Position) >= 10.0)
      return;
    this.BreakOut();
  }

  public void SetPrisonID(GameObject body)
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.FrameDelay((System.Action) (() => this._prison.Data.FollowerID = this._brain.Info.ID)));
  }

  public IEnumerator FrameDelay(System.Action callback)
  {
    yield return (object) new WaitForEndOfFrame();
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void BreakOut()
  {
    this.isBreakingOut = true;
    Follower f = FollowerManager.FindFollowerByID(this.Brain.Info.ID);
    if ((UnityEngine.Object) f != (UnityEngine.Object) null)
    {
      this._prison.Data.FollowerID = -1;
      DataManager.Instance.Followers_Imprisoned_IDs.Remove(this._brain.Info.ID);
      f.TimedAnimation("Prison/stocks-breaking-out", 1.6f, (System.Action) (() =>
      {
        AudioManager.Instance.PlayOneShot("event:/material/wood_barrel_break", f.gameObject);
        BiomeConstants.Instance.EmitSmokeExplosionVFX(this._prison.Data.Position);
        this._prison.Collapse(refreshFollowerTasks: false);
        f.TimedAnimation("Prison/Unlawful/stocks-broke-free", 1f, (System.Action) (() =>
        {
          NotificationCentre.Instance.PlayFaithNotification("Notifications/BrokeOutOfPrison", 0.0f, NotificationBase.Flair.None, this._brain.Info.ID);
          this.Complete();
        }), false);
      }));
    }
    else
    {
      this._prison.Data.FollowerID = -1;
      DataManager.Instance.Followers_Imprisoned_IDs.Remove(this._brain.Info.ID);
      NotificationCentre.Instance.PlayFaithNotification("Notifications/BrokeOutOfPrison", 0.0f, NotificationBase.Flair.None, this._brain.Info.ID);
      this.Complete();
    }
  }

  [CompilerGenerated]
  public void \u003CSetPrisonID\u003Eb__36_0()
  {
    this._prison.Data.FollowerID = this._brain.Info.ID;
  }
}
