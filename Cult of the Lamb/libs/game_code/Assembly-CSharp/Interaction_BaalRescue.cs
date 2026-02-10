// Decompiled with JetBrains decompiler
// Type: Interaction_BaalRescue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using MMBiomeGeneration;
using MMTools;
using Spine.Unity;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_BaalRescue : Interaction
{
  public static Interaction_BaalRescue Instance;
  public SkeletonAnimation Spine;
  public SkeletonAnimation AymSpine;
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
  public Vector3 doorEntrancePosition;
  public bool isNowFollowing;
  public bool foundTeleporter;
  public SimpleBark roomClearBarks;
  public Interaction_SimpleConversation introConversation;
  public Interaction_SimpleConversation outroConversation;
  public bool facePlayer = true;
  public int barkCount = 3;
  public string lastAnimName;

  public void Start()
  {
    Interaction_BaalRescue.Instance = this;
    this.label = ScriptLocalization.NAMES.Guardian1;
    if (!(bool) (UnityEngine.Object) this.AymSpine)
      return;
    this.AymSpine.gameObject.SetActive(false);
  }

  public new void Update()
  {
    if (!this.facePlayer)
      return;
    if ((double) this.transform.position.x < (double) PlayerFarming.Instance.transform.position.x - 0.10000000149011612)
    {
      this.transform.localScale = new Vector3(-1f, 1f, 1f);
    }
    else
    {
      if ((double) this.transform.position.x <= (double) PlayerFarming.Instance.transform.position.x + 0.10000000149011612)
        return;
      this.transform.localScale = new Vector3(1f, 1f, 1f);
    }
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.rewardGiven)
      return;
    this.facePlayer = false;
    this.transform.localScale = Vector3.one;
    this.Interactable = false;
    this.Label = "";
    this.HasChanged = true;
    if (!this.isNowFollowing)
      this.StartCoroutine((IEnumerator) this.IntroToLostBaal());
    else if (this.foundTeleporter)
      this.StartCoroutine((IEnumerator) this.OutroToLostBaal());
    base.OnInteract(state);
  }

  public IEnumerator IntroToLostBaal()
  {
    Interaction_BaalRescue interactionBaalRescue = this;
    interactionBaalRescue.playerFarming.GoToAndStop(interactionBaalRescue.transform.position + Vector3.down * 1.5f);
    while (interactionBaalRescue.playerFarming.GoToAndStopping)
      yield return (object) null;
    interactionBaalRescue.playerFarming.state.facingAngle = Utils.GetAngle(interactionBaalRescue.playerFarming.transform.position, interactionBaalRescue.transform.position);
    interactionBaalRescue.playerFarming.state.LookAngle = interactionBaalRescue.playerFarming.state.facingAngle;
    Health.team2.Clear();
    Interaction_SimpleConversation currentConversation = interactionBaalRescue.introConversation;
    if ((UnityEngine.Object) currentConversation != (UnityEngine.Object) null)
    {
      currentConversation.Spoken = false;
      currentConversation.Play(interactionBaalRescue.state.gameObject);
      yield return (object) new WaitUntil((Func<bool>) (() => currentConversation.Finished));
    }
    while (MMConversation.CURRENT_CONVERSATION != null)
      yield return (object) null;
    yield return (object) new WaitForSeconds(0.25f);
    if ((bool) (UnityEngine.Object) interactionBaalRescue.companion)
    {
      interactionBaalRescue.companion.init(interactionBaalRescue.state.gameObject);
      interactionBaalRescue.companion.OnRoomEnter += new System.Action<CompanionCrusade>(interactionBaalRescue.RoomEntered);
      interactionBaalRescue.companion.OnRoomClear += new System.Action<CompanionCrusade>(interactionBaalRescue.RoomCleared);
    }
    interactionBaalRescue.isNowFollowing = true;
    RoomLockController.RoomCompleted();
    GameManager.GetInstance().OnConversationEnd();
    interactionBaalRescue.CheckForTeleporter();
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
    if (!((UnityEngine.Object) this.teleporter != (UnityEngine.Object) null))
      return;
    this.StopAllCoroutines();
    this.companion.enabled = false;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.companion);
    this.foundTeleporter = true;
    this.StartCoroutine((IEnumerator) this.MoveToTeleporter());
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
    DOVirtual.DelayedCall(2f, (TweenCallback) (() =>
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

  public IEnumerator MoveToTeleporter()
  {
    Interaction_BaalRescue interactionBaalRescue = this;
    Vector3 position1 = PlayerFarming.Instance.transform.position;
    interactionBaalRescue.doorEntrancePosition = new Vector3(position1.x, position1.y, 0.0f);
    if ((bool) (UnityEngine.Object) interactionBaalRescue.AymSpine)
    {
      interactionBaalRescue.AymSpine.gameObject.SetActive(true);
      interactionBaalRescue.AymSpine.transform.SetParent(interactionBaalRescue.teleporter.transform.parent);
      interactionBaalRescue.AymSpine.transform.localScale = new Vector3(-1f, 1f, 1f);
      Vector3 position2 = interactionBaalRescue.teleporter.transform.position;
      position2.x -= 0.9f;
      position2.y -= 0.075f;
      position2.z = 0.0f;
      interactionBaalRescue.AymSpine.transform.position = position2;
    }
    Vector3 targetPosition = interactionBaalRescue.teleporter.transform.position;
    targetPosition.x += 0.3f;
    targetPosition.y += 0.075f;
    targetPosition.z = 0.0f;
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationNew();
    yield return (object) new WaitForSeconds(0.5f);
    interactionBaalRescue.lastAnimName = "";
    double num1 = (double) interactionBaalRescue.PlayAnimation(interactionBaalRescue.RunAnimation, true);
    interactionBaalRescue.Spine.AnimationState.SetAnimation(0, interactionBaalRescue.RunAnimation, true);
    float num2 = Vector3.Distance(interactionBaalRescue.transform.position, targetPosition) / 3f;
    interactionBaalRescue.transform.DOMove(targetPosition, num2).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear);
    yield return (object) new WaitForSeconds(num2);
    interactionBaalRescue.Spine.AnimationState.SetAnimation(0, interactionBaalRescue.IdleAnimation, true);
    if ((bool) (UnityEngine.Object) interactionBaalRescue.AymSpine)
      interactionBaalRescue.FaceTarget(interactionBaalRescue.AymSpine.transform.position);
    yield return (object) new WaitForSeconds(1f);
    interactionBaalRescue.StartCoroutine((IEnumerator) interactionBaalRescue.OutroToLostBaal());
  }

  public IEnumerator OutroToLostBaal()
  {
    Interaction_BaalRescue interactionBaalRescue = this;
    interactionBaalRescue.playerFarming.GoToAndStop(interactionBaalRescue.transform.position + Vector3.down * 1.5f);
    while (interactionBaalRescue.playerFarming.GoToAndStopping)
      yield return (object) null;
    interactionBaalRescue.facePlayer = false;
    interactionBaalRescue.transform.localScale = new Vector3(1f, 1f, 1f);
    interactionBaalRescue.playerFarming.state.facingAngle = Utils.GetAngle(interactionBaalRescue.playerFarming.transform.position, interactionBaalRescue.transform.position);
    interactionBaalRescue.playerFarming.state.LookAngle = interactionBaalRescue.playerFarming.state.facingAngle;
    Interaction_SimpleConversation currentConversation = interactionBaalRescue.outroConversation;
    if ((UnityEngine.Object) currentConversation != (UnityEngine.Object) null)
    {
      currentConversation.Spoken = false;
      currentConversation.Play();
      MMConversation.mmConversation.SpeechBubble.ScreenOffset = 200f;
      yield return (object) new WaitUntil((Func<bool>) (() => currentConversation.Finished));
    }
    while (MMConversation.CURRENT_CONVERSATION != null)
      yield return (object) null;
    yield return (object) interactionBaalRescue.StartCoroutine((IEnumerator) interactionBaalRescue.GiveHeartRoutine());
    interactionBaalRescue.Label = "";
    interactionBaalRescue.rewardGiven = true;
    interactionBaalRescue.Spine.AnimationState.SetAnimation(0, interactionBaalRescue.RunAnimation, true);
    if ((bool) (UnityEngine.Object) interactionBaalRescue.AymSpine)
      interactionBaalRescue.AymSpine.AnimationState.SetAnimation(0, interactionBaalRescue.RunAnimation, true);
    Vector3 zero = Vector3.zero;
    Vector3 normalized = (interactionBaalRescue.doorEntrancePosition - zero).normalized;
    float num1 = Vector3.Distance(interactionBaalRescue.transform.position, interactionBaalRescue.doorEntrancePosition);
    Vector3 endValue1 = interactionBaalRescue.doorEntrancePosition + normalized * num1;
    double num2 = (double) num1 * 2.0;
    float num3 = 3f;
    double num4 = (double) num3;
    float duration1 = (float) (num2 / num4);
    float seconds = num1 / num3;
    float num5 = (double) normalized.x >= 0.0 ? -1f : 1f;
    float num6 = (double) normalized.x >= 0.0 ? 1f : -1f;
    interactionBaalRescue.transform.DOMove(endValue1, duration1).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear);
    interactionBaalRescue.Spine.skeleton.ScaleX = num5;
    if ((bool) (UnityEngine.Object) interactionBaalRescue.AymSpine)
    {
      float num7 = Vector3.Distance(interactionBaalRescue.AymSpine.transform.position, interactionBaalRescue.doorEntrancePosition);
      Vector3 endValue2 = interactionBaalRescue.doorEntrancePosition + normalized * num7;
      float duration2 = num7 * 2f / num3;
      double num8 = (double) num7 / (double) num3;
      interactionBaalRescue.AymSpine.transform.DOMove(endValue2, duration2).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear);
      interactionBaalRescue.AymSpine.skeleton.ScaleX = num6;
    }
    yield return (object) new WaitForSeconds(seconds);
    Color endValue3 = new Color(0.23f, 0.16f, 0.36f, 1f);
    float num9 = 0.75f;
    interactionBaalRescue.Spine.skeleton.SetColor(Color.white);
    DOTween.To((DOGetter<Color>) (() => this.Spine.skeleton.GetColor()), (DOSetter<Color>) (x => this.Spine.skeleton.SetColor(x)), endValue3, num9);
    if ((bool) (UnityEngine.Object) interactionBaalRescue.AymSpine)
    {
      interactionBaalRescue.AymSpine.skeleton.SetColor(Color.white);
      DOTween.To((DOGetter<Color>) (() => this.AymSpine.skeleton.GetColor()), (DOSetter<Color>) (x => this.AymSpine.skeleton.SetColor(x)), endValue3, num9);
    }
    yield return (object) new WaitForSeconds(num9);
    PlayerFarming.SetStateForAllPlayers();
    GameManager.GetInstance().OnConversationEnd();
    if ((bool) (UnityEngine.Object) interactionBaalRescue.AymSpine)
      UnityEngine.Object.Destroy((UnityEngine.Object) interactionBaalRescue.AymSpine.gameObject);
    UnityEngine.Object.Destroy((UnityEngine.Object) interactionBaalRescue.gameObject);
  }

  public new void OnDestroy()
  {
    this.Spine.skeleton.SetColor(Color.white);
    if ((bool) (UnityEngine.Object) this.AymSpine)
      this.AymSpine.skeleton.SetColor(Color.white);
    base.OnDestroy();
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
    Interaction_BaalRescue interactionBaalRescue = this;
    yield return (object) new WaitForEndOfFrame();
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionBaalRescue.state.gameObject);
    bool waiting = true;
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid");
    FollowerOutfitCustomTarget.Create(interactionBaalRescue.transform.position, interactionBaalRescue.state.gameObject.transform.position, 2f, FollowerClothingType.Winter_3, (System.Action) (() => waiting = false));
    interactionBaalRescue.state.CURRENT_STATE = StateMachine.State.InActive;
    while (waiting)
      yield return (object) null;
    DataManager.Instance.BaalNeedsRescue = false;
    DataManager.Instance.BaalRescued = true;
    yield return (object) new WaitForSeconds(0.5f);
  }

  [CompilerGenerated]
  public void \u003CShowRoomClearedBark\u003Eb__25_0() => this.RoomCleared(this.companion);
}
