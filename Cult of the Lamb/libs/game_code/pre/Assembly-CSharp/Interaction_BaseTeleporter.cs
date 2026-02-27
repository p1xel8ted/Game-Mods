// Decompiled with JetBrains decompiler
// Type: Interaction_BaseTeleporter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using MMTools;
using Spine;
using Spine.Unity;
using src.Extensions;
using src.UI.Overlays.EventOverlay;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_BaseTeleporter : Interaction
{
  public Animator animator;
  private SkeletonAnimation skeletonAnimation;
  private bool Used;
  private bool Activating;
  public GameObject distortionObject;
  public GameObject LightingVolume;
  public Material PlayerStencilRT;
  public Material originalMaterial;
  public SimpleSetCamera SimpleSetCamera;
  public GameObject BlackWhiteOverlay;
  public static Interaction_BaseTeleporter Instance;
  public GoopFade goopFade;
  [SerializeField]
  public GameObject newLocation;
  private string sInteract;
  public static System.Action OnPlayerTeleportedIn;

  private void Start()
  {
    this.HasSecondaryInteraction = false;
    this.ActivateDistance = 2f;
    this.UpdateLocalisation();
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    Interaction_BaseTeleporter.Instance = this;
    this.distortionObject.gameObject.SetActive(false);
    if (!((UnityEngine.Object) this.LightingVolume != (UnityEngine.Object) null))
      return;
    this.LightingVolume.gameObject.SetActive(false);
  }

  private new void OnDestroy()
  {
    if (!((UnityEngine.Object) this.skeletonAnimation != (UnityEngine.Object) null))
      return;
    this.skeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
  }

  public override void OnDisableInteraction()
  {
    if ((UnityEngine.Object) this.skeletonAnimation != (UnityEngine.Object) null)
      this.skeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
    base.OnDisableInteraction();
  }

  public override void OnBecomeCurrent()
  {
    base.OnBecomeCurrent();
    this.animator.SetBool("isReady", true);
    AudioManager.Instance.PlayOneShot("event:/pentagram/pentagram_on", this.gameObject);
  }

  public override void OnBecomeNotCurrent()
  {
    base.OnBecomeNotCurrent();
    this.animator.SetBool("isReady", false);
    AudioManager.Instance.PlayOneShot("event:/pentagram/pentagram_off", this.gameObject);
  }

  public override void IndicateHighlighted()
  {
  }

  public override void EndIndicateHighlighted()
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
    if ((UnityEngine.Object) this.skeletonAnimation == (UnityEngine.Object) null)
      this.skeletonAnimation = PlayerFarming.Instance.Spine;
    if ((UnityEngine.Object) this.skeletonAnimation == (UnityEngine.Object) null)
      return;
    this.skeletonAnimation.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
    this.Used = true;
    this.Activating = true;
    GameManager.GetInstance().OnConversationNew(false);
    GameManager.GetInstance().OnConversationNext(this.gameObject, 8f);
    PlayerFarming.Instance.GoToAndStop(this.transform.position, GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.TeleportOut())));
  }

  protected override void Update()
  {
    base.Update();
    if ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
      return;
    this.newLocation.SetActive(!this.Activating && DataManager.Instance.UnlockBaseTeleporter && DataManager.Instance.DiscoveredLocations.Count > 1 && DataManager.Instance.DiscoveredLocations.Count > DataManager.Instance.VisitedLocations.Count);
  }

  private IEnumerator TeleportOut()
  {
    Interaction_BaseTeleporter interactionBaseTeleporter = this;
    if ((UnityEngine.Object) interactionBaseTeleporter.LightingVolume != (UnityEngine.Object) null)
      interactionBaseTeleporter.LightingVolume.gameObject.SetActive(true);
    PlayerFarming.Instance.transform.DOMove(interactionBaseTeleporter.gameObject.transform.position, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine);
    interactionBaseTeleporter.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    AudioManager.Instance.PlayOneShot("event:/pentagram/pentagram_teleport_segment", interactionBaseTeleporter.gameObject);
    interactionBaseTeleporter.animator.SetTrigger("warpOut");
    PlayerFarming.Instance.simpleSpineAnimator.Animate("warp-out-down", 0, false);
    interactionBaseTeleporter.BlackWhiteOverlay.SetActive(true);
    BiomeConstants.Instance.GoopFadeIn(1f, 1.4f);
    BiomeConstants.Instance.ChromaticAbberationTween(2f, BiomeConstants.Instance.ChromaticAberrationDefaultValue, 1f);
    yield return (object) new WaitForSeconds(1.4f);
    interactionBaseTeleporter.PulseDisplacementObject();
    yield return (object) new WaitForSeconds(0.75f);
    BiomeConstants.Instance.ChromaticAbberationTween(0.1f, 0.6f, BiomeConstants.Instance.ChromaticAberrationDefaultValue);
    MMTransition.StopCurrentTransition();
    UIWorldMapMenuController mapMenuController = MonoSingleton<UIManager>.Instance.ShowWorldMap();
    mapMenuController.Show();
    // ISSUE: reference to a compiler-generated method
    mapMenuController.OnCancel = mapMenuController.OnCancel + new System.Action(interactionBaseTeleporter.\u003CTeleportOut\u003Eb__26_0);
    if ((UnityEngine.Object) interactionBaseTeleporter.LightingVolume != (UnityEngine.Object) null)
      interactionBaseTeleporter.LightingVolume.gameObject.SetActive(false);
    if ((UnityEngine.Object) interactionBaseTeleporter.BlackWhiteOverlay != (UnityEngine.Object) null)
      interactionBaseTeleporter.BlackWhiteOverlay.SetActive(false);
  }

  public void PulseDisplacementObject()
  {
    if (this.distortionObject.gameObject.activeSelf)
    {
      this.distortionObject.transform.localScale = Vector3.zero;
      this.distortionObject.transform.DORestart();
      this.distortionObject.transform.DOPlay();
    }
    else
    {
      this.distortionObject.SetActive(true);
      this.distortionObject.transform.localScale = Vector3.zero;
      this.distortionObject.transform.DOScale(9f, 0.9f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.distortionObject.SetActive(false)));
    }
  }

  public void TeleportIn() => this.StartCoroutine((IEnumerator) this.DoTeleportRoutine());

  private IEnumerator DoTeleportRoutine()
  {
    Interaction_BaseTeleporter interactionBaseTeleporter = this;
    HUD_Manager.Instance.Hide(true);
    yield return (object) new WaitForEndOfFrame();
    AudioManager.Instance.PlayOneShot("event:/pentagram/pentagram_teleport_segment", interactionBaseTeleporter.gameObject);
    interactionBaseTeleporter.BlackWhiteOverlay.SetActive(true);
    BiomeConstants.Instance.GoopFadeOut(1f);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    interactionBaseTeleporter.Activating = true;
    PlayerFarming.Instance.transform.position = interactionBaseTeleporter.transform.position;
    GameManager.GetInstance().CameraSnapToPosition(PlayerFarming.Instance.transform.position);
    GameManager.GetInstance().OnConversationNew(false, true);
    GameManager.GetInstance().OnConversationNext(interactionBaseTeleporter.gameObject, 8f);
    interactionBaseTeleporter.animator.SetTrigger("warpIn");
    PlayerFarming.Instance.Spine.GetComponent<MeshRenderer>().enabled = false;
    if ((UnityEngine.Object) interactionBaseTeleporter.skeletonAnimation == (UnityEngine.Object) null)
      interactionBaseTeleporter.skeletonAnimation = PlayerFarming.Instance.Spine;
    interactionBaseTeleporter.skeletonAnimation.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(interactionBaseTeleporter.HandleAnimationStateEvent);
    yield return (object) new WaitForSeconds(1f);
    yield return (object) new WaitForEndOfFrame();
    PlayerFarming.Instance.simpleSpineAnimator.Animate("warp-in-up", 0, false);
    PlayerFarming.Instance.state.facingAngle = PlayerFarming.Instance.state.LookAngle = 270f;
    yield return (object) new WaitForSeconds(0.5f);
    PlayerFarming.Instance.Spine.GetComponent<MeshRenderer>().enabled = true;
    yield return (object) new WaitForSeconds(2.5f);
    if (!CheatConsole.IN_DEMO && Onboarding.CurrentPhase != DataManager.OnboardingPhase.Done && !DataManager.Instance.DifficultyChosen || !CheatConsole.IN_DEMO && Onboarding.CurrentPhase == DataManager.OnboardingPhase.Done && !DataManager.Instance.DifficultyReminded && DifficultyManager.PrimaryDifficulty >= DifficultyManager.Difficulty.Medium && DataManager.Instance.playerDeathsInARow >= 2)
    {
      UIDifficultySelectorOverlayController difficultySelectorInstance = MonoSingleton<UIManager>.Instance.ShowDifficultySelector();
      if ((UnityEngine.Object) difficultySelectorInstance != (UnityEngine.Object) null)
      {
        UIDifficultySelectorOverlayController overlayController = difficultySelectorInstance;
        overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() =>
        {
          if (!DataManager.Instance.DifficultyChosen)
            DataManager.Instance.DifficultyChosen = true;
          else
            DataManager.Instance.DifficultyReminded = true;
          difficultySelectorInstance = (UIDifficultySelectorOverlayController) null;
        });
        difficultySelectorInstance.OnDifficultySelected += (System.Action<int>) (difficulty =>
        {
          DataManager.Instance.MetaData.Difficulty = difficulty;
          DifficultyManager.ForceDifficulty(DataManager.Instance.MetaData.Difficulty);
        });
      }
      while ((UnityEngine.Object) difficultySelectorInstance != (UnityEngine.Object) null)
        yield return (object) null;
    }
    if (DataManager.Instance.OnboardingFinished && SeasonalEventManager.InitialiseEvents())
    {
      SeasonalEventData activeEvent = SeasonalEventManager.GetActiveEvent();
      if ((UnityEngine.Object) activeEvent != (UnityEngine.Object) null)
      {
        UIEventOverlay eventOverlay = activeEvent.EventOverlay.Instantiate<UIEventOverlay>();
        eventOverlay.Show(activeEvent);
        UIEventOverlay uiEventOverlay = eventOverlay;
        uiEventOverlay.OnHidden = uiEventOverlay.OnHidden + (System.Action) (() => eventOverlay = (UIEventOverlay) null);
        while ((UnityEngine.Object) eventOverlay != (UnityEngine.Object) null)
          yield return (object) null;
      }
    }
    GameManager.GetInstance().OnConversationEnd();
    interactionBaseTeleporter.Used = false;
    interactionBaseTeleporter.Activating = false;
    interactionBaseTeleporter.skeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(interactionBaseTeleporter.HandleAnimationStateEvent);
    if ((UnityEngine.Object) interactionBaseTeleporter.BlackWhiteOverlay != (UnityEngine.Object) null)
      interactionBaseTeleporter.BlackWhiteOverlay.SetActive(false);
    System.Action playerTeleportedIn = Interaction_BaseTeleporter.OnPlayerTeleportedIn;
    if (playerTeleportedIn != null)
      playerTeleportedIn();
  }

  private void HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (e.Data.Name == "warp-in-burst_start")
    {
      if ((UnityEngine.Object) this.originalMaterial != (UnityEngine.Object) null && (UnityEngine.Object) this.PlayerStencilRT != (UnityEngine.Object) null)
      {
        PlayerFarming.Instance.Spine.CustomMaterialOverride.Clear();
        PlayerFarming.Instance.Spine.CustomMaterialOverride.Add(this.originalMaterial, this.PlayerStencilRT);
      }
      PlayerFarming.Instance.Spine.GetComponent<MeshRenderer>().enabled = true;
      this.PulseDisplacementObject();
    }
    if (!(e.Data.Name == "warp-in-burst_end"))
      return;
    PlayerFarming.Instance.Spine.CustomMaterialOverride.Clear();
  }

  public override void OnDrawGizmos() => base.OnDrawGizmos();

  public void ActivateRoutine() => this.StartCoroutine((IEnumerator) this.ActivateIE());

  private IEnumerator ActivateIE()
  {
    Interaction_BaseTeleporter interactionBaseTeleporter = this;
    interactionBaseTeleporter.Activating = true;
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionBaseTeleporter.gameObject);
    AudioManager.Instance.PlayOneShot("event:/ui/map_location_pan", interactionBaseTeleporter.gameObject);
    yield return (object) new WaitForSeconds(1f);
    MMVibrate.Haptic(MMVibrate.HapticTypes.SoftImpact);
    CameraManager.instance.ShakeCameraForDuration(0.0f, 2f, 0.5f);
    interactionBaseTeleporter.animator.SetBool("isReady", true);
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", interactionBaseTeleporter.gameObject);
    interactionBaseTeleporter.Activating = false;
    yield return (object) new WaitForSeconds(2.2f);
    interactionBaseTeleporter.animator.SetBool("isReady", false);
    GameManager.GetInstance().OnConversationEnd();
  }
}
