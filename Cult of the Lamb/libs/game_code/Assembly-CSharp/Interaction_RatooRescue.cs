// Decompiled with JetBrains decompiler
// Type: Interaction_RatooRescue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using MMBiomeGeneration;
using MMTools;
using Spine.Unity;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_RatooRescue : Interaction
{
  public static Interaction_RatooRescue Instance;
  public SkeletonAnimation Spine;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string IdleAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string RunAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string RunStopAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string DigDownAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string DigUpAnimation;
  public Interaction_TeleportHome teleporter;
  public CompanionCrusade companion;
  public bool giveReward;
  public bool rewardGiven;
  public bool isNowFollowing;
  public bool foundTeleporter;
  public SimpleBark roomClearBarks;
  public Interaction_SimpleConversation introConversation;
  public Interaction_SimpleConversation outroConversation;
  public int barkCount = 3;
  public string lastAnimName;

  public void Start() => this.label = ScriptLocalization.NAMES.Ratoo;

  public new void OnDestroy() => base.OnDestroy();

  public new void OnDisable() => base.OnDisable();

  public override void OnInteract(StateMachine state)
  {
    if (this.rewardGiven)
      return;
    this.Interactable = false;
    this.Label = "";
    this.HasChanged = true;
    if (!this.isNowFollowing)
    {
      DataManager.Instance.RatooNeedsRescue = false;
      this.StartCoroutine((IEnumerator) this.IntroToLostRatoo());
    }
    else if (this.foundTeleporter)
      this.StartCoroutine((IEnumerator) this.OutroToLostRatoo());
    base.OnInteract(state);
  }

  public IEnumerator IntroToLostRatoo()
  {
    Interaction_RatooRescue interactionRatooRescue = this;
    interactionRatooRescue.playerFarming.GoToAndStop(interactionRatooRescue.transform.position + Vector3.down * 1.5f, maxDuration: 3f, forcePositionOnTimeout: true);
    while (interactionRatooRescue.playerFarming.GoToAndStopping)
      yield return (object) null;
    Health.team2.Clear();
    Interaction_SimpleConversation currentConversation = interactionRatooRescue.introConversation;
    if ((UnityEngine.Object) currentConversation != (UnityEngine.Object) null)
    {
      currentConversation.Spoken = false;
      currentConversation.Play(interactionRatooRescue.state.gameObject);
      yield return (object) new WaitUntil((Func<bool>) (() => currentConversation.Finished));
    }
    while (MMConversation.CURRENT_CONVERSATION != null)
      yield return (object) null;
    yield return (object) new WaitForSeconds(0.25f);
    if ((bool) (UnityEngine.Object) interactionRatooRescue.companion)
    {
      interactionRatooRescue.companion.init(interactionRatooRescue.state.gameObject);
      interactionRatooRescue.companion.OnRoomEnter += new System.Action<CompanionCrusade>(interactionRatooRescue.RoomEntered);
      interactionRatooRescue.companion.OnRoomClear += new System.Action<CompanionCrusade>(interactionRatooRescue.RoomCleared);
    }
    interactionRatooRescue.isNowFollowing = true;
    RoomLockController.RoomCompleted();
    GameManager.GetInstance().OnConversationEnd();
    interactionRatooRescue.CheckForTeleporter();
  }

  public void RoomEntered(CompanionCrusade companion)
  {
    if (this.isNowFollowing)
    {
      this.label = "";
      this.Interactable = false;
    }
    this.CheckForTeleporter();
  }

  public void CheckForTeleporter()
  {
    this.teleporter = UnityEngine.Object.FindObjectOfType<Interaction_TeleportHome>();
    if (!((UnityEngine.Object) this.teleporter != (UnityEngine.Object) null) || RespawnRoomManager.RespawnPlayInProgress)
      return;
    this.StopAllCoroutines();
    this.companion.enabled = false;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.companion);
    this.foundTeleporter = true;
    this.StartCoroutine((IEnumerator) this.DigToTeleporter());
  }

  public void ShowRoomClearedBark()
  {
    Debug.Log((object) ("TESTING BARK " + this.roomClearBarks.Entries.Count.ToString()));
    DOVirtual.DelayedCall(3f, (TweenCallback) (() => this.RoomCleared(this.companion)));
  }

  public void RoomCleared(CompanionCrusade companion)
  {
    if (!((UnityEngine.Object) this.roomClearBarks != (UnityEngine.Object) null) || (double) UnityEngine.Random.value >= 0.75 || this.barkCount <= 0 || BiomeGenerator.Instance.CurrentRoom.IsBoss)
      return;
    --this.barkCount;
    DOVirtual.DelayedCall(3f, (TweenCallback) (() =>
    {
      foreach (ConversationEntry entry in this.roomClearBarks.Entries)
      {
        entry.Animation = companion.lastAnimName;
        entry.LoopAnimation = companion.lastAnimLoop;
        entry.DefaultAnimation = companion.lastAnimName;
      }
      this.roomClearBarks.gameObject.SetActive(true);
    }));
  }

  public IEnumerator DigToTeleporter()
  {
    Interaction_RatooRescue interactionRatooRescue = this;
    GameManager.GetInstance().OnConversationNew();
    yield return (object) new WaitForSeconds(0.5f);
    yield return (object) interactionRatooRescue.PlayAnimationAndWait(interactionRatooRescue.DigDownAnimation);
    interactionRatooRescue.transform.position = new Vector3(interactionRatooRescue.teleporter.transform.position.x, interactionRatooRescue.teleporter.transform.position.y, interactionRatooRescue.teleporter.transform.position.z);
    double num = (double) interactionRatooRescue.PlayAnimation(interactionRatooRescue.DigUpAnimation, false, interactionRatooRescue.IdleAnimation);
    yield return (object) new WaitForSeconds(2f);
    GameManager.GetInstance().OnConversationEnd();
    interactionRatooRescue.Interactable = true;
    interactionRatooRescue.AutomaticallyInteract = true;
    interactionRatooRescue.ActivateDistance = 6f;
    interactionRatooRescue.label = ScriptLocalization.NAMES.Ratoo;
    interactionRatooRescue.foundTeleporter = true;
  }

  public IEnumerator OutroToLostRatoo()
  {
    Interaction_RatooRescue interactionRatooRescue = this;
    interactionRatooRescue.playerFarming.GoToAndStop(interactionRatooRescue.transform.position + Vector3.down * 1.5f, DisableCollider: true, maxDuration: 5f, forcePositionOnTimeout: true);
    while (interactionRatooRescue.playerFarming.GoToAndStopping)
    {
      interactionRatooRescue.FaceTarget(interactionRatooRescue.playerFarming.transform.position);
      yield return (object) null;
    }
    interactionRatooRescue.playerFarming.state.facingAngle = Utils.GetAngle(interactionRatooRescue.playerFarming.transform.position, interactionRatooRescue.transform.position);
    interactionRatooRescue.playerFarming.state.LookAngle = Utils.GetAngle(interactionRatooRescue.playerFarming.transform.position, interactionRatooRescue.transform.position);
    Interaction_SimpleConversation currentConversation = interactionRatooRescue.outroConversation;
    if ((UnityEngine.Object) currentConversation != (UnityEngine.Object) null)
    {
      currentConversation.Spoken = false;
      currentConversation.Play();
      MMConversation.mmConversation.SpeechBubble.ScreenOffset = 200f;
      yield return (object) new WaitUntil((Func<bool>) (() => currentConversation.Finished));
    }
    while (MMConversation.CURRENT_CONVERSATION != null)
      yield return (object) null;
    yield return (object) interactionRatooRescue.StartCoroutine((IEnumerator) interactionRatooRescue.GiveHeartRoutine());
    interactionRatooRescue.Label = "";
    interactionRatooRescue.rewardGiven = true;
    AudioManager.Instance.PlayOneShot("event:/pentagram/pentagram_teleport_segment", interactionRatooRescue.gameObject);
    interactionRatooRescue.teleporter.animator.SetTrigger("warpOut");
    yield return (object) interactionRatooRescue.StartCoroutine((IEnumerator) interactionRatooRescue.PlayAnimationAndWait(interactionRatooRescue.DigDownAnimation));
    PlayerFarming.SetStateForAllPlayers();
    GameManager.GetInstance().OnConversationEnd();
    UnityEngine.Object.Destroy((UnityEngine.Object) interactionRatooRescue.gameObject);
  }

  public void FaceTarget(Vector3 target)
  {
    Vector3 vector3 = target - this.transform.position;
    float num = Mathf.Atan2(vector3.y, vector3.x) * 57.29578f;
    this.Spine.skeleton.ScaleX = (double) num > 90.0 || (double) num < -90.0 ? 1f : -1f;
  }

  public float PlayAnimation(string animName, bool loop, string animAfterToLoop = "")
  {
    if (animName == this.lastAnimName)
      return 0.0f;
    this.lastAnimName = animName;
    Spine.Animation animation = this.Spine.Skeleton.Data.FindAnimation(animName);
    if (animation == null)
      return 0.0f;
    this.Spine.AnimationState.SetAnimation(0, animName, loop);
    if (animAfterToLoop != "")
      this.Spine.AnimationState.AddAnimation(0, animAfterToLoop, true, 0.0f);
    return animation.Duration;
  }

  public IEnumerator PlayAnimationAndWait(string animName)
  {
    if (!(animName == this.lastAnimName))
    {
      this.lastAnimName = animName;
      bool finished = false;
      if (this.Spine.AnimationState.SetAnimation(0, animName, false) != null)
        this.Spine.AnimationState.Complete += (Spine.AnimationState.TrackEntryDelegate) (trackEntry1 =>
        {
          if (!(trackEntry1.Animation.Name == animName))
            return;
          finished = true;
          this.Spine.AnimationState.Complete -= (Spine.AnimationState.TrackEntryDelegate) (trackEntry2 =>
          {
            // ISSUE: unable to decompile the method.
          });
        });
      else
        finished = true;
      yield return (object) new WaitUntil((Func<bool>) (() => finished));
    }
  }

  public IEnumerator GiveHeartRoutine()
  {
    Interaction_RatooRescue interactionRatooRescue = this;
    yield return (object) new WaitForEndOfFrame();
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionRatooRescue.state.gameObject);
    bool waiting = true;
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid");
    FollowerOutfitCustomTarget.Create(interactionRatooRescue.transform.position, interactionRatooRescue.state.gameObject.transform.position, 2f, FollowerClothingType.Winter_2, (System.Action) (() => waiting = false));
    while (waiting)
      yield return (object) null;
    DataManager.Instance.RatooNeedsRescue = false;
    DataManager.Instance.RatooRescued = true;
    yield return (object) new WaitForSeconds(0.5f);
  }

  [CompilerGenerated]
  public void \u003CShowRoomClearedBark\u003Eb__23_0() => this.RoomCleared(this.companion);
}
