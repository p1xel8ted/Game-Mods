// Decompiled with JetBrains decompiler
// Type: Interaction_BaseTeleporter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using MMTools;
using Spine;
using Spine.Unity;
using src.Extensions;
using src.Managers;
using src.UI;
using src.UI.Overlays.EventOverlay;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class Interaction_BaseTeleporter : Interaction
{
  public Animator animator;
  public List<SkeletonAnimation> skeletonAnimations = new List<SkeletonAnimation>();
  public bool Used;
  public bool Activating;
  public GameObject LightingVolume;
  public Material[] PlayerStencilRT;
  public Transform displacementTransform;
  public Material originalMaterial;
  public SimpleSetCamera SimpleSetCamera;
  public GameObject BlackWhiteOverlay;
  public static Interaction_BaseTeleporter Instance;
  public GoopFade goopFade;
  [SerializeField]
  public GameObject newLocation;
  public List<AsyncOperationHandle> asyncOperationHandles = new List<AsyncOperationHandle>();
  public string sInteract;
  public static System.Action OnPlayerTeleportedIn;

  public void Start()
  {
    this.HasSecondaryInteraction = false;
    this.ActivateDistance = 2f;
    this.UpdateLocalisation();
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    Interaction_BaseTeleporter.Instance = this;
    if (!((UnityEngine.Object) this.LightingVolume != (UnityEngine.Object) null))
      return;
    this.LightingVolume.gameObject.SetActive(false);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    foreach (SkeletonAnimation skeletonAnimation in this.skeletonAnimations)
    {
      if ((UnityEngine.Object) skeletonAnimation != (UnityEngine.Object) null)
        skeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
    }
    for (int index = 0; index < this.asyncOperationHandles.Count; ++index)
    {
      if (this.asyncOperationHandles[index].IsValid())
        Addressables.Release(this.asyncOperationHandles[index]);
    }
    this.asyncOperationHandles.Clear();
  }

  public override void OnDisableInteraction()
  {
    foreach (SkeletonAnimation skeletonAnimation in this.skeletonAnimations)
    {
      if ((UnityEngine.Object) skeletonAnimation != (UnityEngine.Object) null)
        skeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
    }
    base.OnDisableInteraction();
  }

  public override void OnBecomeCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeCurrent(playerFarming);
    this.animator.SetBool("isReady", true);
    AudioManager.Instance.PlayOneShot("event:/pentagram/pentagram_on", this.gameObject);
  }

  public override void OnBecomeNotCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeNotCurrent(playerFarming);
    this.animator.SetBool("isReady", false);
    AudioManager.Instance.PlayOneShot("event:/pentagram/pentagram_off", this.gameObject);
  }

  public override void IndicateHighlighted(PlayerFarming playerFarming)
  {
  }

  public override void EndIndicateHighlighted(PlayerFarming playerFarming)
  {
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sInteract = ScriptLocalization.Interactions.Teleport;
  }

  public override void GetLabel()
  {
    bool active = TwitchHelpHinder.Active;
    this.Label = ((this.Activating || !DataManager.Instance.UnlockBaseTeleporter ? 1 : (DataManager.Instance.DiscoveredLocations.Count <= 1 ? 1 : 0)) | (active ? 1 : 0)) != 0 ? "" : this.sInteract;
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.Used)
      return;
    base.OnInteract(state);
    if (!this.skeletonAnimations.Contains(this.playerFarming.Spine))
      this.skeletonAnimations.Add(this.playerFarming.Spine);
    foreach (SkeletonAnimation skeletonAnimation in this.skeletonAnimations)
      skeletonAnimation.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
    this.Used = true;
    this.Activating = true;
    GameManager.GetInstance().OnConversationNew(false);
    GameManager.GetInstance().OnConversationNext(this.gameObject, 8f);
    this.playerFarming.GoToAndStop(this.transform.position, GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.TeleportOut())));
  }

  public override void Update()
  {
    base.Update();
    if ((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null)
      return;
    this.newLocation.SetActive(!this.Activating && DataManager.Instance.UnlockBaseTeleporter && DataManager.Instance.DiscoveredLocations.Count > 1 && DataManager.Instance.DiscoveredLocations.Count > DataManager.Instance.VisitedLocations.Count);
  }

  public IEnumerator TeleportOut()
  {
    Interaction_BaseTeleporter interactionBaseTeleporter = this;
    if ((UnityEngine.Object) interactionBaseTeleporter.LightingVolume != (UnityEngine.Object) null)
      interactionBaseTeleporter.LightingVolume.gameObject.SetActive(true);
    interactionBaseTeleporter.playerFarming.transform.DOMove(interactionBaseTeleporter.gameObject.transform.position, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine);
    interactionBaseTeleporter.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    AudioManager.Instance.PlayOneShot("event:/pentagram/pentagram_teleport_segment", interactionBaseTeleporter.gameObject);
    interactionBaseTeleporter.animator.SetTrigger("warpOut");
    interactionBaseTeleporter.playerFarming.simpleSpineAnimator.Animate("warp-out-down", 0, false);
    interactionBaseTeleporter.BlackWhiteOverlay.SetActive(true);
    BiomeConstants.Instance.GoopFadeIn(1f, 1.4f);
    BiomeConstants.Instance.ChromaticAbberationTween(2f, BiomeConstants.Instance.ChromaticAberrationDefaultValue, 1f);
    yield return (object) new WaitForSeconds(1.4f);
    BiomeConstants.Instance.EmitDisplacementEffect(interactionBaseTeleporter.displacementTransform.position);
    yield return (object) new WaitForSeconds(0.75f);
    BiomeConstants.Instance.ChromaticAbberationTween(0.1f, 0.6f, BiomeConstants.Instance.ChromaticAberrationDefaultValue);
    MMTransition.StopCurrentTransition();
    System.Threading.Tasks.Task loadTask = MonoSingleton<UIManager>.Instance.LoadWorldMapAssets();
    yield return (object) new WaitUntil((Func<bool>) (() => loadTask.IsCompleted));
    UIWorldMapMenuController mapMenuController = MonoSingleton<UIManager>.Instance.ShowWorldMap();
    mapMenuController.Show();
    mapMenuController.OnCancel = mapMenuController.OnCancel + (System.Action) (() => this.TeleportIn());
    if ((UnityEngine.Object) interactionBaseTeleporter.LightingVolume != (UnityEngine.Object) null)
      interactionBaseTeleporter.LightingVolume.gameObject.SetActive(false);
    if ((UnityEngine.Object) interactionBaseTeleporter.BlackWhiteOverlay != (UnityEngine.Object) null)
      interactionBaseTeleporter.BlackWhiteOverlay.SetActive(false);
  }

  public void TeleportIn() => this.StartCoroutine((IEnumerator) this.DoTeleportRoutine());

  public IEnumerator DoTeleportRoutine()
  {
    Interaction_BaseTeleporter interactionBaseTeleporter = this;
    Follower followerPrefab = FollowerManager.FollowerPrefab;
    Follower combatFollowerPrefab = FollowerManager.CombatFollowerPrefab;
    FollowerRecruit recruitPrefab = FollowerManager.RecruitPrefab;
    ObjectPool.CreatePool<FollowerRecruit>(FollowerManager.RecruitPrefab, 3);
    HUD_Manager.Instance.Hide(true);
    yield return (object) new WaitForEndOfFrame();
    AudioManager.Instance.PlayOneShot("event:/pentagram/pentagram_teleport_segment", interactionBaseTeleporter.gameObject);
    interactionBaseTeleporter.BlackWhiteOverlay.SetActive(true);
    BiomeConstants.Instance.GoopFadeOut(1f);
    interactionBaseTeleporter.Activating = true;
    for (int i = PlayerFarming.players.Count - 1; i >= 0; --i)
    {
      if (i == 0)
        yield return (object) interactionBaseTeleporter.StartCoroutine((IEnumerator) interactionBaseTeleporter.SpawnPlayerIE(PlayerFarming.players[i], PlayerFarming.players.Count > 1 ? Vector3.left / 2f : Vector3.zero));
      else
        interactionBaseTeleporter.StartCoroutine((IEnumerator) interactionBaseTeleporter.SpawnPlayerIE(PlayerFarming.players[i], Vector3.right / 2f));
    }
    if (!CheatConsole.IN_DEMO && Onboarding.CurrentPhase != DataManager.OnboardingPhase.Done && !DataManager.Instance.DifficultyChosen)
    {
      UIDifficultySelectorOverlayController difficultySelectorInstance = (UIDifficultySelectorOverlayController) null;
      yield return (object) MonoSingleton<UIManager>.Instance.LoadDifficultySelector();
      while ((UnityEngine.Object) difficultySelectorInstance == (UnityEngine.Object) null)
      {
        yield return (object) new WaitForSeconds(0.1f);
        difficultySelectorInstance = MonoSingleton<UIManager>.Instance.ShowDifficultySelector();
        if ((UnityEngine.Object) difficultySelectorInstance != (UnityEngine.Object) null)
        {
          Debug.Log((object) ("difficulty seletor found " + Time.time.ToString()));
          UIDifficultySelectorOverlayController overlayController = difficultySelectorInstance;
          overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() =>
          {
            if (!DataManager.Instance.DifficultyChosen)
              DataManager.Instance.DifficultyChosen = true;
            else
              DataManager.Instance.DifficultyReminded = true;
          });
          difficultySelectorInstance.OnDifficultySelected += (System.Action<int>) (difficulty =>
          {
            DataManager.Instance.MetaData.Difficulty = difficulty;
            DifficultyManager.ForceDifficulty(DataManager.Instance.MetaData.Difficulty);
          });
        }
        else
          Debug.Log((object) ("difficulty selector not found " + Time.time.ToString()));
      }
      yield return (object) difficultySelectorInstance.YieldUntilHidden();
      difficultySelectorInstance = (UIDifficultySelectorOverlayController) null;
    }
    if (DataManager.Instance.OnboardingFinished && SeasonalEventManager.InitialiseEvents())
    {
      SeasonalEventData activeEvent = SeasonalEventManager.GetActiveEvent();
      if ((UnityEngine.Object) activeEvent != (UnityEngine.Object) null && (UnityEngine.Object) activeEvent.EventOverlay != (UnityEngine.Object) null && activeEvent.ShowEventOverlay)
      {
        UIEventOverlay menu = activeEvent.EventOverlay.Instantiate<UIEventOverlay>();
        menu.Show(activeEvent);
        yield return (object) menu.YieldUntilHidden();
      }
    }
    if (DataManager.Instance.DeathCatBeaten && !PersistenceManager.PersistentData.PostGameRevealed)
    {
      UIMenuConfirmationWindow menu = MonoSingleton<UIManager>.Instance.ConfirmationWindowTemplate.Instantiate<UIMenuConfirmationWindow>();
      menu.Show();
      menu.Configure(ScriptLocalization.UI_PostGameUnlock.Header, ScriptLocalization.UI_PostGameUnlock.Description, true);
      yield return (object) menu.YieldUntilHidden();
      PersistenceManager.PersistentData.PostGameRevealed = true;
      PersistenceManager.Save();
    }
    if (DataManager.Instance.DeathCatBeaten && !PersistenceManager.PersistentData.UnlockedSurvivalMode)
    {
      UIMenuConfirmationWindow menu = MonoSingleton<UIManager>.Instance.ConfirmationWindowTemplate.Instantiate<UIMenuConfirmationWindow>();
      menu.Show();
      menu.Configure(ScriptLocalization.UI_PostGameUnlock.Header, LocalizationManager.GetTranslation("UI/SurvivalMode/Description"), true);
      yield return (object) menu.YieldUntilHidden();
      PersistenceManager.PersistentData.UnlockedSurvivalMode = true;
      PersistenceManager.Save();
    }
    if (!DataManager.Instance.CompletedSandbox && DungeonSandboxManager.GetCompletedRunCount() >= 120)
    {
      UIMenuConfirmationWindow menu = MonoSingleton<UIManager>.Instance.ConfirmationWindowTemplate.Instantiate<UIMenuConfirmationWindow>();
      menu.Show();
      menu.Configure(ScriptLocalization.UI_CompletedPurgatory.Header, ScriptLocalization.UI_CompletedPurgatory.Description, true);
      yield return (object) menu.YieldUntilHidden();
      DataManager.Instance.CompletedSandbox = true;
    }
    GameManager.GetInstance()?.OnConversationEnd();
    interactionBaseTeleporter.Used = false;
    interactionBaseTeleporter.Activating = false;
    foreach (SkeletonAnimation skeletonAnimation in interactionBaseTeleporter.skeletonAnimations)
      skeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(interactionBaseTeleporter.HandleAnimationStateEvent);
    if ((UnityEngine.Object) interactionBaseTeleporter.BlackWhiteOverlay != (UnityEngine.Object) null)
      interactionBaseTeleporter.BlackWhiteOverlay.SetActive(false);
    if (CompanionBaseArea.AllCompanions.Count == 0 && interactionBaseTeleporter.playerFarming.isLamb)
      CompanionBaseArea.SpawnCompanionGhosts();
    System.Action playerTeleportedIn = Interaction_BaseTeleporter.OnPlayerTeleportedIn;
    if (playerTeleportedIn != null)
      playerTeleportedIn();
  }

  public IEnumerator SpawnPlayerIE(PlayerFarming playerFarming, Vector3 positionOffset)
  {
    Interaction_BaseTeleporter interactionBaseTeleporter = this;
    playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    playerFarming.state.LockStateChanges = true;
    playerFarming.transform.position = interactionBaseTeleporter.transform.position + positionOffset;
    GameManager.GetInstance().CameraSnapToPosition(playerFarming.transform.position);
    GameManager.GetInstance().OnConversationNew(false);
    GameManager.GetInstance().OnConversationNext(interactionBaseTeleporter.gameObject, 8f);
    interactionBaseTeleporter.animator.SetTrigger("warpIn");
    playerFarming.Spine.GetComponent<MeshRenderer>().enabled = false;
    if (!interactionBaseTeleporter.skeletonAnimations.Contains(playerFarming.Spine))
      interactionBaseTeleporter.skeletonAnimations.Add(playerFarming.Spine);
    foreach (SkeletonAnimation skeletonAnimation in interactionBaseTeleporter.skeletonAnimations)
      skeletonAnimation.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(interactionBaseTeleporter.HandleAnimationStateEvent);
    yield return (object) new WaitForSeconds(1f);
    yield return (object) new WaitForEndOfFrame();
    playerFarming.simpleSpineAnimator.Animate("warp-in-up", 0, false);
    playerFarming.state.facingAngle = playerFarming.state.LookAngle = 270f;
    yield return (object) new WaitForSeconds(0.5f);
    playerFarming.Spine.GetComponent<MeshRenderer>().enabled = true;
    yield return (object) new WaitForSeconds(2.5f);
    if (playerFarming.state.LockStateChanges)
    {
      playerFarming.state.LockStateChanges = false;
      Debug.LogWarning((object) "Warp-in-up aniamtion event wasn't invoked!");
    }
  }

  public void HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (e.Data.Name == "warp-in-burst_start")
    {
      if ((UnityEngine.Object) this.originalMaterial != (UnityEngine.Object) null && this.PlayerStencilRT != null)
      {
        for (int index = 0; index < PlayerFarming.players.Count; ++index)
        {
          if (PlayerFarming.players[index].state.CURRENT_STATE == StateMachine.State.CustomAnimation)
          {
            PlayerFarming.players[index].Spine.CustomMaterialOverride.Clear();
            PlayerFarming.players[index].Spine.CustomMaterialOverride.Add(this.originalMaterial, this.PlayerStencilRT[index]);
            PlayerFarming.players[index].Spine.GetComponent<MeshRenderer>().enabled = true;
          }
        }
      }
      BiomeConstants.Instance.EmitDisplacementEffect(this.displacementTransform.position);
    }
    if (!(e.Data.Name == "warp-in-burst_end"))
      return;
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      player.Spine.CustomMaterialOverride.Clear();
      player.state.LockStateChanges = false;
    }
  }

  public override void OnDrawGizmos() => base.OnDrawGizmos();

  public void ActivateRoutine() => this.StartCoroutine((IEnumerator) this.ActivateIE());

  public IEnumerator ActivateIE()
  {
    Interaction_BaseTeleporter interactionBaseTeleporter = this;
    interactionBaseTeleporter.Activating = true;
    GameManager.GetInstance().OnConversationNew();
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationNext(interactionBaseTeleporter.gameObject);
    AudioManager.Instance.PlayOneShot("event:/ui/map_location_pan", interactionBaseTeleporter.gameObject);
    yield return (object) new WaitForSeconds(1f);
    MMVibrate.Haptic(MMVibrate.HapticTypes.SoftImpact, interactionBaseTeleporter.playerFarming);
    CameraManager.instance.ShakeCameraForDuration(0.0f, 2f, 0.5f);
    interactionBaseTeleporter.animator.SetBool("isReady", true);
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", interactionBaseTeleporter.gameObject);
    interactionBaseTeleporter.Activating = false;
    yield return (object) new WaitForSeconds(2.2f);
    interactionBaseTeleporter.animator.SetBool("isReady", false);
    GameManager.GetInstance().OnConversationEnd();
  }

  public void WakeUpInBase() => this.StartCoroutine((IEnumerator) this.WakeUpInBaseRoutine());

  public IEnumerator WakeUpInBaseRoutine()
  {
    Interaction_BaseTeleporter interactionBaseTeleporter = this;
    HUD_Manager.Instance.Hide(true);
    yield return (object) new WaitForEndOfFrame();
    interactionBaseTeleporter.BlackWhiteOverlay.SetActive(true);
    BiomeConstants.Instance.GoopFadeOut(1f);
    interactionBaseTeleporter.playerFarming.transform.position = TownCentre.Instance.transform.position;
    GameManager.GetInstance().CameraSnapToPosition(interactionBaseTeleporter.playerFarming.transform.position);
    interactionBaseTeleporter.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    interactionBaseTeleporter.playerFarming.CustomAnimation("intro/rebirth2", false);
    interactionBaseTeleporter.Activating = true;
    GameManager.GetInstance().OnConversationNew(false);
    GameManager.GetInstance().OnConversationNext(interactionBaseTeleporter.playerFarming.gameObject, 8f);
    yield return (object) new WaitForSeconds(4f);
    GameManager.GetInstance().OnConversationEnd();
    interactionBaseTeleporter.Used = false;
    interactionBaseTeleporter.Activating = false;
    if ((UnityEngine.Object) interactionBaseTeleporter.BlackWhiteOverlay != (UnityEngine.Object) null)
      interactionBaseTeleporter.BlackWhiteOverlay.SetActive(false);
    System.Action playerTeleportedIn = Interaction_BaseTeleporter.OnPlayerTeleportedIn;
    if (playerTeleportedIn != null)
      playerTeleportedIn();
    interactionBaseTeleporter.playerFarming.state.CURRENT_STATE = StateMachine.State.InActive;
    if (DataManager.Instance.WinterModeActive)
      Onboarding.Instance.RatWinterMode.GetComponent<Interaction_SimpleConversation>().Play();
    else
      Onboarding.Instance.RatSurvivalMode.GetComponent<Interaction_SimpleConversation>().Play();
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__25_0()
  {
    this.StartCoroutine((IEnumerator) this.TeleportOut());
  }
}
