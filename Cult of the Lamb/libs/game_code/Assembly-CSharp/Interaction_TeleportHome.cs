// Decompiled with JetBrains decompiler
// Type: Interaction_TeleportHome
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.DeathScreen;
using MMTools;
using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_TeleportHome : Interaction
{
  public static Interaction_TeleportHome Instance;
  public bool ResurrectionRoom;
  public RunResults RunResults;
  public bool Activating;
  public Transform PlayerPosition;
  public bool Debug_WarpIn;
  public bool TeleportOutOnlyInteractingPlayer;
  public SkeletonAnimation skeletonAnimation;
  public Animator animator;
  public GoopFade goopFade;
  public bool GoViaQuoteScreen;
  public string TeleporterOnSFX = "event:/pentagram/pentagram_on";
  public string TeleporterPlatformStartSFX = "event:/pentagram_platform/pentagram_platform_start";
  public string TeleporterPlatformEndSFX = "event:/pentagram_platform/pentagram_platform_end";
  public string TeleportSegmentSFX = "event:/pentagram/pentagram_teleport_segment";
  public string pentagramDustGathersSFX = "event:/pentagram/pentagram_dust_gathers";
  public string sReturnToBase;
  public string sSummonWorkers;
  public bool CanSummonWorkers;
  public static System.Action<Interaction_TeleportHome> PlayerActivatingStart;
  public static System.Action<Interaction_TeleportHome> PlayerActivatingEnd;
  public float smokeScale = 6f;
  [Header("Materials")]
  [SerializeField]
  public MeshRenderer _portal;
  [SerializeField]
  public Material NormalMaterial;
  [SerializeField]
  public Material D5Material;
  [SerializeField]
  public Material D6Material;
  [SerializeField]
  public SkeletonAnimation moleSpine;
  [CompilerGenerated]
  public Vector3 \u003CCachePosition\u003Ek__BackingField;
  public bool DoDeathScreen = true;
  public bool OnMountainTop;

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sReturnToBase = ScriptLocalization.Interactions.ReturnToBase;
    this.sSummonWorkers = "Summon Workers - Add in loc";
  }

  public override void GetLabel() => this.Label = this.Activating ? "" : this.sReturnToBase;

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.animator.SetBool("isEnabled", true);
    if (!((UnityEngine.Object) this._portal != (UnityEngine.Object) null))
      return;
    switch (PlayerFarming.Location)
    {
      case FollowerLocation.Dungeon1_5:
        if (!((UnityEngine.Object) this.D5Material != (UnityEngine.Object) null))
          break;
        this._portal.material = this.D5Material;
        break;
      case FollowerLocation.Dungeon1_6:
        if (!((UnityEngine.Object) this.D6Material != (UnityEngine.Object) null))
          break;
        this._portal.material = this.D6Material;
        break;
    }
  }

  public Vector3 CachePosition
  {
    get => this.\u003CCachePosition\u003Ek__BackingField;
    set => this.\u003CCachePosition\u003Ek__BackingField = value;
  }

  public void DisableTeleporter()
  {
    this.animator.SetBool("isEnabled", false);
    this.Activating = true;
    Debug.Log((object) ("transform.position: " + this.transform.position.ToString()));
    this.CachePosition = this.transform.position;
    Debug.Log((object) ("CachePosition: " + this.CachePosition.ToString()));
    this.transform.localScale = Vector3.zero;
  }

  public void EnableTeleporter(bool doSpawnVFX = true)
  {
    this.gameObject.SetActive(true);
    this.StartCoroutine((IEnumerator) this.IEnableTeleporter(Vector3.one, doSpawnVFX));
  }

  public void EnableTeleporter(Vector3 scale, bool doSpawnVFX = true)
  {
    this.gameObject.SetActive(true);
    this.StartCoroutine((IEnumerator) this.IEnableTeleporter(scale, doSpawnVFX));
  }

  public IEnumerator IEnableTeleporter(Vector3 scale, bool doSpawnVFX)
  {
    Interaction_TeleportHome interactionTeleportHome = this;
    interactionTeleportHome.Activating = false;
    if (doSpawnVFX)
    {
      interactionTeleportHome.StartCoroutine((IEnumerator) interactionTeleportHome.EmitSmoke());
      interactionTeleportHome.transform.DOMove(interactionTeleportHome.CachePosition, 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuad);
      interactionTeleportHome.transform.DOScale(scale, 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuad);
      yield return (object) new WaitForSeconds(1f);
    }
    interactionTeleportHome.animator.SetBool("isEnabled", true);
    AudioManager.Instance.PlayOneShot(interactionTeleportHome.TeleporterOnSFX, interactionTeleportHome.gameObject);
  }

  public IEnumerator EmitSmoke()
  {
    Interaction_TeleportHome interactionTeleportHome = this;
    AudioManager.Instance.PlayOneShot(interactionTeleportHome.pentagramDustGathersSFX, interactionTeleportHome.transform.position);
    float Timer = 0.0f;
    float emitInterval = 0.1f;
    while ((double) Timer < 1.5)
    {
      if ((double) Timer % (double) emitInterval < (double) Time.deltaTime)
        BiomeConstants.Instance.EmitSmokeInteractionVFX(interactionTeleportHome.transform.position, Vector3.one * interactionTeleportHome.smokeScale);
      Timer += Time.deltaTime;
      yield return (object) null;
    }
  }

  public override void OnDestroy()
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

  public override void OnBecomeCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeCurrent(playerFarming);
    this.animator.SetBool("isReady", true);
    AudioManager.Instance.PlayOneShot(this.TeleporterPlatformStartSFX, this.gameObject);
  }

  public override void OnBecomeNotCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeNotCurrent(playerFarming);
    this.animator.SetBool("isReady", false);
    AudioManager.Instance.PlayOneShot(this.TeleporterPlatformEndSFX, this.gameObject);
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (this.Activating)
      return;
    if ((UnityEngine.Object) this.skeletonAnimation == (UnityEngine.Object) null)
      this.skeletonAnimation = this.playerFarming.Spine;
    if ((UnityEngine.Object) this.skeletonAnimation == (UnityEngine.Object) null)
      return;
    this.skeletonAnimation.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
    if (!((UnityEngine.Object) this.playerFarming != (UnityEngine.Object) null) || this.playerFarming.GoToAndStopping)
      return;
    this.StartCoroutine((IEnumerator) this.BreakDLCLock((System.Action) (() =>
    {
      PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive, PlayerNotToInclude: this.playerFarming);
      if (this.TeleportOutOnlyInteractingPlayer)
        this.playerFarming.GoToAndStop(this.PlayerPosition.position, this.gameObject, GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.DoTeleportOut(this.playerFarming))));
      else if (!this.Debug_WarpIn)
      {
        foreach (PlayerFarming player1 in PlayerFarming.players)
        {
          PlayerFarming player = player1;
          player.GoToAndStop(this.PlayerPosition.position, this.gameObject, GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.DoTeleportOut(player))));
        }
      }
      else
        this.playerFarming.GoToAndStop(this.PlayerPosition.position, this.gameObject, GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.DoTeleportIn())));
    })));
  }

  public IEnumerator BreakDLCLock(System.Action callback)
  {
    if ((UnityEngine.Object) this.moleSpine != (UnityEngine.Object) null && DataManager.Instance.MapLockCountToUnlock != -1 && (PlayerFarming.Location == FollowerLocation.Dungeon1_5 || PlayerFarming.Location == FollowerLocation.Dungeon1_6))
    {
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(this.moleSpine.gameObject, 6f);
      yield return (object) new WaitForSeconds(1f);
      this.moleSpine.gameObject.SetActive(true);
      this.moleSpine.AnimationState.SetAnimation(0, "dig_up", false);
      AudioManager.Instance.PlayOneShot("event:/dlc/env/gofernon/burrow_in", this.moleSpine.transform.position);
      this.moleSpine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
      yield return (object) new WaitForSeconds(1.5f);
      List<ConversationEntry> Entries1 = new List<ConversationEntry>()
      {
        new ConversationEntry(this.moleSpine.gameObject, $"Conversation_NPC/Mole/PathReveal_{DataManager.Instance.MapLockCountToUnlock + 1}/0"),
        new ConversationEntry(this.moleSpine.gameObject, $"Conversation_NPC/Mole/PathReveal_{DataManager.Instance.MapLockCountToUnlock + 1}/1")
      };
      if (DataManager.Instance.MapLockCountToUnlock == 1)
        Entries1.RemoveAt(1);
      else if (DataManager.Instance.MapLockCountToUnlock == 3)
        Entries1.Add(new ConversationEntry(this.moleSpine.gameObject, $"Conversation_NPC/Mole/PathReveal_{DataManager.Instance.MapLockCountToUnlock + 1}/2"));
      foreach (ConversationEntry conversationEntry in Entries1)
      {
        conversationEntry.CharacterName = ScriptLocalization.NAMES.BaseExpansionNPC;
        conversationEntry.Animation = "talk";
        conversationEntry.LoopAnimation = false;
      }
      MMConversation.Play(new ConversationObject(Entries1, (List<MMTools.Response>) null, (System.Action) null), false);
      yield return (object) null;
      while (MMConversation.isPlaying)
        yield return (object) null;
      UIManager.PlayAudio("event:/dlc/music/map/gofernon_node_reveal");
      bool waiting = true;
      DLCMap.BreakDungeonLock(DataManager.Instance.MapLockCountToUnlock, (System.Action) (() => waiting = false));
      while (waiting)
        yield return (object) null;
      if (DataManager.Instance.MapLockCountToUnlock == 0)
      {
        List<ConversationEntry> Entries2 = new List<ConversationEntry>()
        {
          new ConversationEntry(this.moleSpine.gameObject, $"Conversation_NPC/Mole/PathReveal_{DataManager.Instance.MapLockCountToUnlock + 1}/2")
        };
        foreach (ConversationEntry conversationEntry in Entries2)
        {
          conversationEntry.CharacterName = ScriptLocalization.NAMES.BaseExpansionNPC;
          conversationEntry.Animation = "talk";
          conversationEntry.LoopAnimation = false;
        }
        MMConversation.Play(new ConversationObject(Entries2, (List<MMTools.Response>) null, (System.Action) null), false);
        yield return (object) null;
        while (MMConversation.isPlaying)
          yield return (object) null;
      }
      this.moleSpine.AnimationState.SetAnimation(0, "dig_down", false);
      AudioManager.Instance.PlayOneShot("event:/dlc/env/gofernon/burrow_out", this.moleSpine.transform.position);
    }
    System.Action action = callback;
    if (action != null)
      action();
    DataManager.Instance.MapLockCountToUnlock = -1;
  }

  public IEnumerator DoTeleportOut(PlayerFarming player)
  {
    Interaction_TeleportHome interactionTeleportHome = this;
    HUD_Manager.Instance.Hide(false);
    interactionTeleportHome.Activating = true;
    System.Action<Interaction_TeleportHome> playerActivatingStart = Interaction_TeleportHome.PlayerActivatingStart;
    if (playerActivatingStart != null)
      playerActivatingStart(interactionTeleportHome);
    interactionTeleportHome.CheckLegendaryWeaponObjectives();
    player.transform.DOMove(interactionTeleportHome.PlayerPosition.position, 0.25f);
    GameManager.GetInstance().OnConversationNext(interactionTeleportHome.PlayerPosition.gameObject, 8f);
    player.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    AudioManager.Instance.PlayOneShot(interactionTeleportHome.TeleportSegmentSFX, interactionTeleportHome.gameObject);
    interactionTeleportHome.animator.SetTrigger("warpOut");
    player.simpleSpineAnimator.Animate("warp-out-down", 0, false, 0.0f);
    if (!((UnityEngine.Object) interactionTeleportHome.playerFarming != (UnityEngine.Object) player))
    {
      while (true)
      {
        bool flag = false;
        foreach (PlayerFarming player1 in PlayerFarming.players)
        {
          if (player1.GoToAndStopping)
            flag = true;
        }
        if (flag)
          yield return (object) null;
        else
          break;
      }
      interactionTeleportHome.goopFade.FadeIn(1f, 1.4f);
      BiomeConstants.Instance.ChromaticAbberationTween(2f, BiomeConstants.Instance.ChromaticAberrationDefaultValue, 1f);
      yield return (object) new WaitForSeconds(3f);
      BiomeConstants.Instance.ChromaticAbberationTween(0.1f, 1f, BiomeConstants.Instance.ChromaticAberrationDefaultValue);
      if (interactionTeleportHome.DoDeathScreen)
      {
        if ((UnityEngine.Object) UIDeathScreenOverlayController.Instance == (UnityEngine.Object) null)
        {
          UIDeathScreenOverlayController.Results result = interactionTeleportHome.ResurrectionRoom ? UIDeathScreenOverlayController.Results.Killed : UIDeathScreenOverlayController.Results.Completed;
          if (interactionTeleportHome.OnMountainTop && !DataManager.Instance.AwokenMountainDeathsceen)
          {
            DataManager.Instance.AwokenMountainDeathsceen = true;
            result = UIDeathScreenOverlayController.Results.AwokenMountain;
          }
          MonoSingleton<UIManager>.Instance.ShowDeathScreenOverlay(result).Show();
          if (interactionTeleportHome.ResurrectionRoom)
            RespawnRoomManager.Instance.ResetPathFinding();
        }
      }
      else
        interactionTeleportHome.CompleteDoTeleportOut();
    }
  }

  public QuoteScreenController.QuoteTypes GetQuoteType()
  {
    Debug.Log((object) ("GET QUOTE TYPE! " + PlayerFarming.Location.ToString()));
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

  public void CompleteDoTeleportOut()
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

  public IEnumerator DoTeleportIn()
  {
    Interaction_TeleportHome interactionTeleportHome = this;
    interactionTeleportHome.Activating = true;
    System.Action<Interaction_TeleportHome> playerActivatingStart = Interaction_TeleportHome.PlayerActivatingStart;
    if (playerActivatingStart != null)
      playerActivatingStart(interactionTeleportHome);
    GameManager.GetInstance().OnConversationNext(interactionTeleportHome.PlayerPosition.gameObject, 8f);
    interactionTeleportHome.animator.SetTrigger("warpIn");
    interactionTeleportHome.playerFarming.Spine.GetComponent<MeshRenderer>().enabled = false;
    yield return (object) new WaitForSeconds(1f);
    yield return (object) new WaitForEndOfFrame();
    interactionTeleportHome.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    interactionTeleportHome.playerFarming.simpleSpineAnimator.Animate("warp-in-up", 0, false);
    yield return (object) new WaitForSeconds(3f);
    GameManager.GetInstance().OnConversationEnd();
    interactionTeleportHome.Activating = false;
    System.Action<Interaction_TeleportHome> playerActivatingEnd = Interaction_TeleportHome.PlayerActivatingEnd;
    if (playerActivatingEnd != null)
      playerActivatingEnd(interactionTeleportHome);
    interactionTeleportHome.skeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(interactionTeleportHome.HandleAnimationStateEvent);
  }

  public void HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (e.Data.Name == "warp-in-burst_start")
    {
      this.playerFarming.simpleSpineAnimator.SetColor(Color.black);
      this.playerFarming.Spine.GetComponent<MeshRenderer>().enabled = true;
    }
    if (!(e.Data.Name == "warp-in-burst_end"))
      return;
    this.playerFarming.simpleSpineAnimator.SetColor(Color.white);
  }

  public void Start()
  {
    this.UpdateLocalisation();
    Interaction_TeleportHome.Instance = this;
  }

  public void Play()
  {
  }

  public void CheckLegendaryWeaponObjectives()
  {
    if (DungeonSandboxManager.Active)
      return;
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if (player.playerWeapon.IsLegendaryWeapon())
        ObjectiveManager.CompletLegendaryWeaponRunObjective(player.currentWeapon);
    }
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__43_0()
  {
    PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive, PlayerNotToInclude: this.playerFarming);
    if (this.TeleportOutOnlyInteractingPlayer)
      this.playerFarming.GoToAndStop(this.PlayerPosition.position, this.gameObject, GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.DoTeleportOut(this.playerFarming))));
    else if (!this.Debug_WarpIn)
    {
      foreach (PlayerFarming player1 in PlayerFarming.players)
      {
        PlayerFarming player = player1;
        player.GoToAndStop(this.PlayerPosition.position, this.gameObject, GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.DoTeleportOut(player))));
      }
    }
    else
      this.playerFarming.GoToAndStop(this.PlayerPosition.position, this.gameObject, GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.DoTeleportIn())));
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__43_1()
  {
    this.StartCoroutine((IEnumerator) this.DoTeleportOut(this.playerFarming));
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__43_2()
  {
    this.StartCoroutine((IEnumerator) this.DoTeleportIn());
  }
}
