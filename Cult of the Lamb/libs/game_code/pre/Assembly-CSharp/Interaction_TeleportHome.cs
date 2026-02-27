// Decompiled with JetBrains decompiler
// Type: Interaction_TeleportHome
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.DeathScreen;
using Map;
using MMTools;
using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_TeleportHome : Interaction
{
  public bool ResurrectionRoom;
  public RunResults RunResults;
  private bool Activating;
  public Transform PlayerPosition;
  public bool Debug_WarpIn;
  private SkeletonAnimation skeletonAnimation;
  public Animator animator;
  public GoopFade goopFade;
  public bool GoViaQuoteScreen;
  private string sReturnToBase;
  private string sSummonWorkers;
  public bool CanSummonWorkers;
  public static System.Action<Interaction_TeleportHome> PlayerActivatingStart;
  public static System.Action<Interaction_TeleportHome> PlayerActivatingEnd;
  public bool DoDeathScreen = true;

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sReturnToBase = GameManager.SandboxDungeonEnabled ? ScriptLocalization.Interactions.NextLayer : ScriptLocalization.Interactions.ReturnToBase;
    this.sSummonWorkers = "Summon Workers - Add in loc";
  }

  public override void GetLabel() => this.Label = this.Activating ? "" : this.sReturnToBase;

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.animator.SetBool("isEnabled", true);
  }

  private new void OnDestroy()
  {
    base.OnDestroy();
    if (!((UnityEngine.Object) this.skeletonAnimation != (UnityEngine.Object) null))
      return;
    this.skeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    if ((UnityEngine.Object) this.skeletonAnimation != (UnityEngine.Object) null)
      this.skeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
    base.OnDisableInteraction();
    this.animator.SetBool("isEnabled", false);
  }

  public override void OnBecomeCurrent()
  {
    base.OnBecomeCurrent();
    this.animator.SetBool("isReady", true);
    if (this.gameObject.activeSelf)
      return;
    AudioManager.Instance.PlayOneShot("pentagram_platform/pentagram_platform_start", this.gameObject);
  }

  private IEnumerator PlaySoundAfterOneSecond()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Interaction_TeleportHome interactionTeleportHome = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      AudioManager.Instance.PlayOneShot("event:/pentagram/pentagram_on", interactionTeleportHome.gameObject);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(1f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public override void OnBecomeNotCurrent()
  {
    base.OnBecomeNotCurrent();
    this.animator.SetBool("isReady", false);
    AudioManager.Instance.PlayOneShot("pentagram_platform/pentagram_platform_end", this.gameObject);
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (this.Activating)
      return;
    if ((UnityEngine.Object) this.skeletonAnimation == (UnityEngine.Object) null)
      this.skeletonAnimation = PlayerFarming.Instance.Spine;
    if ((UnityEngine.Object) this.skeletonAnimation == (UnityEngine.Object) null)
      return;
    this.skeletonAnimation.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
    if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null) || PlayerFarming.Instance.GoToAndStopping)
      return;
    if (!this.Debug_WarpIn)
      PlayerFarming.Instance.GoToAndStop(this.PlayerPosition.position, this.gameObject, GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.DoTeleportOut())));
    else
      PlayerFarming.Instance.GoToAndStop(this.PlayerPosition.position, this.gameObject, GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.DoTeleportIn())));
  }

  private IEnumerator DoTeleportOut()
  {
    Interaction_TeleportHome interactionTeleportHome = this;
    HUD_Manager.Instance.Hide(false);
    interactionTeleportHome.Activating = true;
    System.Action<Interaction_TeleportHome> playerActivatingStart = Interaction_TeleportHome.PlayerActivatingStart;
    if (playerActivatingStart != null)
      playerActivatingStart(interactionTeleportHome);
    PlayerFarming.Instance.transform.DOMove(interactionTeleportHome.PlayerPosition.position, 0.25f);
    GameManager.GetInstance().OnConversationNext(interactionTeleportHome.PlayerPosition.gameObject, 8f);
    interactionTeleportHome.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    AudioManager.Instance.PlayOneShot("event:/pentagram/pentagram_teleport_segment", interactionTeleportHome.gameObject);
    interactionTeleportHome.animator.SetTrigger("warpOut");
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("warp-out-down", 0, false, 0.0f);
    interactionTeleportHome.goopFade.FadeIn(1f, 1.4f);
    BiomeConstants.Instance.ChromaticAbberationTween(2f, BiomeConstants.Instance.ChromaticAberrationDefaultValue, 1f);
    yield return (object) new WaitForSeconds(3f);
    BiomeConstants.Instance.ChromaticAbberationTween(0.1f, 1f, BiomeConstants.Instance.ChromaticAberrationDefaultValue);
    if (interactionTeleportHome.DoDeathScreen)
    {
      if (GameManager.SandboxDungeonEnabled)
      {
        DungeonSandboxManager.Instance.SetDungeonType(FollowerLocation.Dungeon1_4);
        MapManager.Instance.MapGenerated = false;
        UIAdventureMapOverlayController overlayController = MapManager.Instance.ShowMap(true);
        MapManager.Instance.StartCoroutine((IEnumerator) overlayController.NextSandboxLayer());
      }
      else if ((UnityEngine.Object) UIDeathScreenOverlayController.Instance == (UnityEngine.Object) null)
      {
        MonoSingleton<UIManager>.Instance.ShowDeathScreenOverlay(interactionTeleportHome.ResurrectionRoom ? UIDeathScreenOverlayController.Results.Killed : UIDeathScreenOverlayController.Results.Completed).Show();
        if (interactionTeleportHome.ResurrectionRoom)
          RespawnRoomManager.Instance.ResetPathFinding();
      }
    }
    else
      interactionTeleportHome.CompleteDoTeleportOut();
  }

  private QuoteScreenController.QuoteTypes GetQuoteType()
  {
    Debug.Log((object) ("GET QUOTE TYPE! " + (object) PlayerFarming.Location));
    switch (PlayerFarming.Location)
    {
      case FollowerLocation.Dungeon1_1:
        return QuoteScreenController.QuoteTypes.QuoteBoss1;
      case FollowerLocation.Dungeon1_2:
        return QuoteScreenController.QuoteTypes.QuoteBoss2;
      case FollowerLocation.Dungeon1_3:
        return QuoteScreenController.QuoteTypes.QuoteBoss3;
      case FollowerLocation.Dungeon1_4:
        return QuoteScreenController.QuoteTypes.QuoteBoss4;
      case FollowerLocation.Dungeon1_5:
        return QuoteScreenController.QuoteTypes.QuoteBoss5;
      default:
        return QuoteScreenController.QuoteTypes.QuoteBoss5;
    }
  }

  private void CompleteDoTeleportOut()
  {
    if (this.GoViaQuoteScreen)
    {
      QuoteScreenController.Init(new List<QuoteScreenController.QuoteTypes>()
      {
        this.GetQuoteType()
      }, (System.Action) (() => GameManager.ToShip()), (System.Action) (() => GameManager.ToShip()));
      MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, "QuoteScreen", 5f, "", (System.Action) (() => Time.timeScale = 1f));
    }
    else
      GameManager.ToShip();
    this.Activating = false;
    System.Action<Interaction_TeleportHome> playerActivatingEnd = Interaction_TeleportHome.PlayerActivatingEnd;
    if (playerActivatingEnd != null)
      playerActivatingEnd(this);
    this.skeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
  }

  private IEnumerator DoTeleportIn()
  {
    Interaction_TeleportHome interactionTeleportHome = this;
    interactionTeleportHome.Activating = true;
    System.Action<Interaction_TeleportHome> playerActivatingStart = Interaction_TeleportHome.PlayerActivatingStart;
    if (playerActivatingStart != null)
      playerActivatingStart(interactionTeleportHome);
    GameManager.GetInstance().OnConversationNext(interactionTeleportHome.PlayerPosition.gameObject, 8f);
    interactionTeleportHome.animator.SetTrigger("warpIn");
    PlayerFarming.Instance.Spine.GetComponent<MeshRenderer>().enabled = false;
    yield return (object) new WaitForSeconds(1f);
    yield return (object) new WaitForEndOfFrame();
    interactionTeleportHome.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.Instance.simpleSpineAnimator.Animate("warp-in-up", 0, false);
    yield return (object) new WaitForSeconds(3f);
    GameManager.GetInstance().OnConversationEnd();
    interactionTeleportHome.Activating = false;
    System.Action<Interaction_TeleportHome> playerActivatingEnd = Interaction_TeleportHome.PlayerActivatingEnd;
    if (playerActivatingEnd != null)
      playerActivatingEnd(interactionTeleportHome);
    interactionTeleportHome.skeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(interactionTeleportHome.HandleAnimationStateEvent);
  }

  private void HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (e.Data.Name == "warp-in-burst_start")
    {
      PlayerFarming.Instance.simpleSpineAnimator.SetColor(Color.black);
      PlayerFarming.Instance.Spine.GetComponent<MeshRenderer>().enabled = true;
    }
    if (!(e.Data.Name == "warp-in-burst_end"))
      return;
    PlayerFarming.Instance.simpleSpineAnimator.SetColor(Color.white);
  }

  private void Start() => this.UpdateLocalisation();

  public void Play()
  {
  }
}
