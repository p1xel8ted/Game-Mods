// Decompiled with JetBrains decompiler
// Type: Interaction_TempleAltar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.AltarMenu;
using Lamb.UI.Rituals;
using Lamb.UI.SermonWheelOverlay;
using MMTools;
using Spine;
using Spine.Unity;
using src.Extensions;
using src.Rituals;
using src.UI.Overlays.TutorialOverlay;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_TempleAltar : Interaction
{
  public static Interaction_TempleAltar Instance;
  public SermonController SermonController;
  [SerializeField]
  public Collider2D Collider;
  public ChurchFollowerManager ChurchFollowerManager;
  public GameObject RitualAvailable;
  public Animator RitualAvailableAnimator;
  public GameObject distortionObject;
  public GameObject SermonAvailableObject;
  public GameObject RitualAvailableObject;
  public GameObject Menu;
  public Sprite AltarNoBook;
  public Sprite AltarShowBook;
  public Sprite SpriteOn;
  public Sprite SpriteOff;
  public Sprite[] SpriteOnStyles;
  public Sprite[] SpriteOffStyles;
  public Sprite SpriteNotPurchased;
  public Material SpriteOnMaterial;
  public Material SpriteOffMaterial;
  public SpriteRenderer spriteRenderer;
  public SpriteRenderer spriteAltarBackRenderer;
  public bool Activated;
  [SerializeField]
  public AnimationCurve controllerRumbleCurve;
  [SerializeField]
  public SinOnboardingMenu sinOnboardingMenu;
  public GameObject Notification;
  public GameObject DoctrineXPPrefab;
  public UIDoctrineBar UIDoctrineBar;
  public static SermonsAndRituals.SermonRitualType CurrentType;
  public UIFollowerXP UIFollowerXPPrefab;
  public GameObject DoctrineUpgradePrefab;
  public bool firstChoice;
  public float barLocalXP;
  public Light RitualLighting;
  public SkeletonAnimation PortalEffect;
  public bool performedSermon;
  public bool previousWasSermon;
  public bool CoopPlayerFrozen;
  public string sPreachSermon;
  public string sAlreadyGivenSermon;
  public string sSacrifice;
  public string sRitual;
  public string sRequiresTemple2;
  public string sRequireMoreFollowers;
  public string sInteract;
  public bool initialInteraction = true;
  [CompilerGenerated]
  public bool \u003CInteracted\u003Ek__BackingField;
  public UpgradeSystem.Type forceRitual = UpgradeSystem.Type.Count;
  public EventInstance SinLoop;
  public SermonCategory SermonCategory;
  public bool SermonsStillAvailable = true;
  public bool playerLeftDuringRitual;
  public EventInstance sermonLoop;
  public bool GivenAnswer;
  public int RewardLevel;
  public static DoctrineUpgradeSystem.DoctrineType DoctrineUnlockType;
  public GameObject RitualCameraPosition;
  public bool HasBuiltTemple2;
  public SimpleSetCamera CloseUpCamera;
  public SimpleSetCamera SimpleSetCamera;
  public SimpleSetCamera RitualCloseSetCamera;
  public GameObject FrontWall;
  public bool Activating;
  public Ritual CurrentRitual;
  public UpgradeSystem.Type RitualType;
  public int fleece;
  public PlayerFarming temporaryPlayerFarming;

  public void SetAltarStyle(int style)
  {
    this.SpriteOn = this.SpriteOnStyles[style];
    this.SpriteOff = this.SpriteOffStyles[style];
    this.ResetAltarSprite();
  }

  public override void Update() => base.Update();

  public void ResetSprite(bool forceBookVisible = false)
  {
    if ((UnityEngine.Object) this.spriteAltarBackRenderer == (UnityEngine.Object) null)
      return;
    this.spriteRenderer.sprite = forceBookVisible || this.Activated ? this.AltarShowBook : this.AltarNoBook;
    this.ResetAltarSprite();
  }

  public void ResetAltarSprite()
  {
    this.spriteAltarBackRenderer.enabled = false;
    this.spriteAltarBackRenderer.sprite = DataManager.Instance.PreviousSermonDayIndex >= TimeManager.CurrentDay ? this.SpriteOff : this.SpriteOn;
    this.spriteAltarBackRenderer.enabled = true;
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.HasSecondaryInteraction = false;
    this.HasBuiltTemple2 = DataManager.Instance.HasBuiltTemple2;
    this.RitualAvailableAnimator.Play("Hidden");
    this.distortionObject.SetActive(false);
    this.ResetSprite(true);
  }

  public bool CheckCanAfford(UpgradeSystem.Type type)
  {
    List<StructuresData.ItemCost> cost = UpgradeSystem.GetCost(type);
    for (int index = 0; index < cost.Count; ++index)
    {
      if (Inventory.GetItemQuantity((int) cost[index].CostItem) < cost[index].CostValue)
        return false;
    }
    return true;
  }

  public void Start()
  {
    Interaction_TempleAltar.Instance = this;
    this.UpdateLocalisation();
    this.ActivateDistance = 1.5f;
    this.Interactable = true;
    this.SecondaryInteractable = true;
    CoopManager.Instance.OnPlayerLeft += new System.Action(this.playerLeft);
    CoopManager.Instance.OnPlayerJoined += new System.Action(this.playerJoined);
  }

  public bool Interacted
  {
    get => this.\u003CInteracted\u003Ek__BackingField;
    set => this.\u003CInteracted\u003Ek__BackingField = value;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sInteract = ScriptLocalization.Interactions.TempleAltar;
  }

  public override void GetLabel() => this.Label = this.Activated ? "" : this.sInteract;

  public override void OnInteract(StateMachine state)
  {
    if (this.Activated)
      return;
    if (!this.Interacted)
    {
      this.Interacted = true;
      SimulationManager.Pause();
    }
    base.OnInteract(state);
    PlayerFarming.Instance.PlayerDoctrineStone.Spine.unscaledTime = true;
    MonoSingleton<UIManager>.Instance.ForceDisableSaving = true;
    this.Activated = true;
    this.HasChanged = true;
    this.OnBecomeNotCurrent(this.playerFarming);
    this.RitualAvailableAnimator.Play("Hidden");
    GameManager.GetInstance().OnConversationNew(false, false, this.playerFarming);
    GameManager.GetInstance().OnConversationNext(this.playerFarming.CameraBone, 8f);
    GameManager.GetInstance().CamFollowTarget.DisablePlayerLook = true;
    CultFaithManager.Instance.BarController.UseQueuing = false;
    this.Collider.enabled = false;
    if (!this.initialInteraction && this.FollowersReadyForLevelUp())
    {
      this.StartCoroutine((IEnumerator) this.BulkLevelFollowers());
    }
    else
    {
      if (!this.initialInteraction && Ritual.FollowerToAttendSermon != null && Ritual.FollowerToAttendSermon.Count > 0)
        GameManager.GetInstance().StartCoroutine((IEnumerator) Interaction_TempleAltar.Instance.FollowersEnterForSermonRoutine(true));
      this.previousWasSermon = false;
      this.StartCoroutine((IEnumerator) this.WaitForPlayersToGetIntoPosition((System.Action) (() =>
      {
        this.Collider.enabled = true;
        this.StartCoroutine((IEnumerator) this.DelayMenu());
        this.initialInteraction = false;
      })));
    }
  }

  public IEnumerator WaitForPlayersToGetIntoPosition(System.Action callback)
  {
    Interaction_TempleAltar interactionTempleAltar = this;
    bool groupAction = true;
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if ((UnityEngine.Object) player != (UnityEngine.Object) interactionTempleAltar.playerFarming && player.state.CURRENT_STATE == StateMachine.State.CustomAnimation && (double) Vector3.Distance(player.transform.position, ChurchFollowerManager.Instance.AltarPosition.position + Vector3.left) < 0.5)
        groupAction = false;
    }
    interactionTempleAltar.playerFarming.GoToAndStop(ChurchFollowerManager.Instance.AltarPosition.position, interactionTempleAltar.gameObject, GoToCallback: new System.Action(interactionTempleAltar.\u003CWaitForPlayersToGetIntoPosition\u003Eb__59_0), maxDuration: 10f, forcePositionOnTimeout: true, groupAction: groupAction, forceAstar: true, forcedOtherPosition: new Vector3?(ChurchFollowerManager.Instance.AltarPosition.position + Vector3.left));
    yield return (object) new WaitForEndOfFrame();
    while (true)
    {
      bool flag = false;
      foreach (PlayerFarming player in PlayerFarming.players)
      {
        if (player.GoToAndStopping)
          flag = true;
      }
      if (flag)
        yield return (object) null;
      else
        break;
    }
    System.Action action = callback;
    if (action != null)
      action();
  }

  public IEnumerator DelayMenu()
  {
    Interaction_TempleAltar interactionTempleAltar = this;
    interactionTempleAltar.spriteRenderer.sprite = interactionTempleAltar.AltarNoBook;
    AudioManager.Instance.PlayOneShot("event:/sermon/book_pickup", interactionTempleAltar.playerFarming.gameObject);
    interactionTempleAltar.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    interactionTempleAltar.playerFarming.simpleSpineAnimator.Animate("build", 0, true);
    yield return (object) new WaitForSeconds(0.2f);
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      player.Spine.UseDeltaTime = false;
      if ((UnityEngine.Object) interactionTempleAltar.playerFarming != (UnityEngine.Object) player && player.state.CURRENT_STATE != StateMachine.State.CustomAnimation)
      {
        player.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
        player.state.LockStateChanges = true;
        player.Spine.AnimationState.SetAnimation(0, "sermons/second-player-start", false);
        player.Spine.AnimationState.AddAnimation(0, "sermons/second-player-loop", true, 0.0f);
      }
    }
    Time.timeScale = 0.0f;
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = true;
    UIAltarMenuController altarMenuController = MonoSingleton<UIManager>.Instance.ShowAltarMenu();
    altarMenuController.OnHidden = altarMenuController.OnHidden + new System.Action(interactionTempleAltar.\u003CDelayMenu\u003Eb__60_0);
    altarMenuController.OnSermonSelected += new System.Action(interactionTempleAltar.\u003CDelayMenu\u003Eb__60_1);
    altarMenuController.OnRitualsSelected += new System.Action(interactionTempleAltar.\u003CDelayMenu\u003Eb__60_2);
    altarMenuController.OnPlayerUpgradesSelected += new System.Action(interactionTempleAltar.\u003CDelayMenu\u003Eb__60_3);
    altarMenuController.OnDoctrineSelected += new System.Action(interactionTempleAltar.\u003CDelayMenu\u003Eb__60_4);
    altarMenuController.OnCultUpgradeSelected += new System.Action(interactionTempleAltar.\u003CDelayMenu\u003Eb__60_5);
    altarMenuController.OnCancel = altarMenuController.OnCancel + new System.Action(interactionTempleAltar.\u003CDelayMenu\u003Eb__60_6);
  }

  public void DoSermon()
  {
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
    if (DataManager.Instance.PreviousSermonDayIndex >= TimeManager.CurrentDay)
    {
      Debug.Log((object) "ALREADY DONE ONE TODAY!");
      if (DataManager.Instance.PleasureEnabled && DataManager.Instance.SinSermonEnabled)
      {
        if (DataManager.Instance.PreviousSinSermonDayIndex < TimeManager.CurrentDay && Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.PLEASURE_POINT) >= 3)
        {
          Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.PLEASURE_POINT, -3);
          DataManager.Instance.PreviousSinSermonDayIndex = TimeManager.CurrentDay;
        }
        else
        {
          this.DoCancel();
          this.CloseAndSpeak("AlreadyGivenSermon");
          return;
        }
      }
      else
      {
        this.DoCancel();
        this.CloseAndSpeak("AlreadyGivenSermon");
        return;
      }
    }
    if (DataManager.Instance.Followers.Count <= 0)
    {
      this.DoCancel();
      this.CloseAndSpeak("NoFollowers");
    }
    else if (Ritual.FollowersAvailableToAttendSermon() <= 0)
    {
      this.DoCancel();
      this.CloseAndSpeak("NoFollowersAvailable");
    }
    else
    {
      GameManager.GetInstance().CameraSetOffset(Vector3.zero);
      this.performedSermon = true;
      this.previousWasSermon = true;
      SimulationManager.UnPause();
      this.SermonController = this.GetComponent<SermonController>();
      this.SermonController.Play(this.state);
    }
  }

  public void DoRitual()
  {
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
    if (Ritual.FollowersAvailableToAttendSermon() <= 0)
    {
      this.DoCancel();
      this.CloseAndSpeak("NoFollowers");
    }
    else if (Ritual.FollowersAvailableToAttendSermon() <= 0)
    {
      this.DoCancel();
      this.CloseAndSpeak("NoFollowersAvail able");
    }
    else
    {
      GameManager.GetInstance().CameraSetOffset(Vector3.left * 2.25f);
      GameManager.GetInstance().OnConversationNext(this.state.gameObject, 6f);
      AudioManager.Instance.PlayOneShot("event:/ritual_sacrifice/start_ritual", this.playerFarming.gameObject);
      this.RitualAvailableAnimator.Play("Hidden");
      this.StartCoroutine((IEnumerator) this.OpenRitualMenuRoutine());
      this.Activated = false;
    }
  }

  public void DoDoctrine()
  {
    this.previousWasSermon = true;
    Time.timeScale = 0.0f;
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
    foreach (PlayerFarming player in PlayerFarming.players)
      player.Spine.UseDeltaTime = false;
    this.Activated = false;
    UIDoctrineMenuController doctrineMenuController = MonoSingleton<UIManager>.Instance.DoctrineMenuTemplate.Instantiate<UIDoctrineMenuController>();
    doctrineMenuController.Show();
    doctrineMenuController.OnHide = doctrineMenuController.OnHide + (System.Action) (() =>
    {
      Time.timeScale = 1f;
      GameManager.GetInstance().CameraSetOffset(Vector3.zero);
      this.OnInteract(this.state);
    });
  }

  public void DoCultUpgrade()
  {
    Time.timeScale = 0.0f;
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
    foreach (PlayerFarming player in PlayerFarming.players)
      player.Spine.UseDeltaTime = false;
    this.Activated = false;
    UICultUpgradesMenuController cultUpgradesMenuInstance = MonoSingleton<UIManager>.Instance.CultUpgradesMenuTemplate.Instantiate<UICultUpgradesMenuController>();
    cultUpgradesMenuInstance.Show();
    UICultUpgradesMenuController upgradesMenuController1 = cultUpgradesMenuInstance;
    upgradesMenuController1.OnHide = upgradesMenuController1.OnHide + (System.Action) (() => cultUpgradesMenuInstance = (UICultUpgradesMenuController) null);
    UICultUpgradesMenuController upgradesMenuController2 = cultUpgradesMenuInstance;
    upgradesMenuController2.OnCancel = upgradesMenuController2.OnCancel + (System.Action) (() =>
    {
      cultUpgradesMenuInstance = (UICultUpgradesMenuController) null;
      this.OnInteract(this.state);
    });
  }

  public void DoPlayerUpgrade()
  {
    this.previousWasSermon = true;
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
    GameManager.GetInstance().CameraSetOffset(Vector3.left * 2.25f);
    GameManager.GetInstance().OnConversationNext(this.state.gameObject, 6f);
    AudioManager.Instance.PlayOneShot("event:/ritual_sacrifice/start_ritual", this.playerFarming.gameObject);
    this.RitualAvailableAnimator.Play("Hidden");
    this.StartCoroutine((IEnumerator) this.OpenPlayerUpgradeRoutine());
    this.Activated = false;
  }

  public IEnumerator OpenPlayerUpgradeRoutine()
  {
    yield return (object) new WaitForSeconds(0.5f);
    this.OpenPlayerUpgradeMenu();
  }

  public void OpenPlayerUpgradeMenu()
  {
    UIPlayerUpgradesMenuController playerUpgradesMenuInstance = MonoSingleton<UIManager>.Instance.ShowPlayerUpgradesMenu();
    UIPlayerUpgradesMenuController upgradesMenuController1 = playerUpgradesMenuInstance;
    upgradesMenuController1.OnCancel = upgradesMenuController1.OnCancel + (System.Action) (() =>
    {
      GameManager.GetInstance().CameraSetOffset(Vector3.zero);
      this.OnInteract(this.state);
    });
    UIPlayerUpgradesMenuController upgradesMenuController2 = playerUpgradesMenuInstance;
    upgradesMenuController2.OnHidden = upgradesMenuController2.OnHidden + (System.Action) (() => playerUpgradesMenuInstance = (UIPlayerUpgradesMenuController) null);
  }

  public void DoCancel()
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.TryOnboardSin((System.Action) (() =>
    {
      MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
      MonoSingleton<UIManager>.Instance.ForceDisableSaving = false;
      PlayerFarming.Instance.PlayerDoctrineStone.Spine.unscaledTime = false;
      SimulationManager.UnPause();
      AudioManager.Instance.StopLoop(this.sermonLoop);
      AudioManager.Instance.PlayOneShot("event:/sermon/book_put_down", this.playerFarming.gameObject);
      GameManager.GetInstance().CameraSetOffset(Vector3.zero);
      Time.timeScale = 1f;
      foreach (PlayerFarming player in PlayerFarming.players)
        player.Spine.UseDeltaTime = true;
      GameManager.GetInstance().CamFollowTarget.DisablePlayerLook = false;
      GameManager.GetInstance().CameraSetOffset(Vector3.zero);
      GameManager.GetInstance().OnConversationEnd();
      CultFaithManager.Instance.BarController.UseQueuing = false;
      this.initialInteraction = true;
      if (Ritual.FollowerToAttendSermon != null)
      {
        foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
        {
          FollowerBrain f = followerBrain;
          if (f != null)
          {
            Follower followerById = FollowerManager.FindFollowerByID(f.Info.ID);
            if ((bool) (UnityEngine.Object) followerById)
            {
              FollowerManager.FindFollowerByID(f.Info.ID).Spine.UseDeltaTime = true;
              followerById.ShowAllFollowerIcons();
              followerById.Spine.UseDeltaTime = true;
              followerById.UseUnscaledTime = false;
              followerById.OverridingOutfit = false;
              if (this.performedSermon)
              {
                int num = 0;
                if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Temple_III))
                  num = 1;
                if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Temple_IV))
                  num = 2;
                for (int index = 0; index < num; ++index)
                {
                  AudioManager.Instance.PlayOneShot("event:/followers/pop_in", followerById.transform.position);
                  ResourceCustomTarget.Create(Interaction_DonationBox.Instance.gameObject, followerById.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) (() => Interaction_DonationBox.Instance.DepositCoin()));
                }
              }
            }
            f.CompleteCurrentTask();
            this.StartCoroutine((IEnumerator) this.DelayCallback(1f, (System.Action) (() =>
            {
              if (f.CurrentTaskType != FollowerTaskType.AttendTeaching)
                return;
              f.CompleteCurrentTask();
            })));
          }
        }
      }
      this.ResetSprite();
      ChurchFollowerManager.Instance.ClearAudienceBrains();
      this.StartCoroutine((IEnumerator) this.MidasStealDonationsIE((System.Action) (() =>
      {
        this.performedSermon = false;
        this.state.CURRENT_STATE = StateMachine.State.Idle;
        this.ResetSprite();
        foreach (PlayerFarming player in PlayerFarming.players)
        {
          PlayerFarming pl = player;
          if ((UnityEngine.Object) this.playerFarming != (UnityEngine.Object) pl)
            pl.CustomAnimationWithCallback("sermons/second-player-stop", false, (System.Action) (() =>
            {
              pl.state.LockStateChanges = false;
              pl.state.CURRENT_STATE = StateMachine.State.Idle;
            }));
        }
        if (Ritual.FollowerToAttendSermon != null && Ritual.FollowerToAttendSermon.Count > 0)
        {
          GameManager.GetInstance().WaitForSeconds(1.5f, (System.Action) (() =>
          {
            this.HasChanged = true;
            this.Activated = false;
          }));
        }
        else
        {
          this.HasChanged = true;
          this.Activated = false;
        }
        if (Ritual.FollowerToAttendSermon == null)
          return;
        Ritual.FollowerToAttendSermon.Clear();
      })));
    })));
  }

  public IEnumerator MidasStealDonationsIE(System.Action callback)
  {
    if (DataManager.Instance.HasMidasHiding && !MidasBaseController.EncounteredMidasInTemple && UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Temple_III) && this.performedSermon)
    {
      MidasBaseController.EncounteredMidasInTemple = true;
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(Interaction_DonationBox.Instance.gameObject, 7f);
      FollowerManager.SpawnedFollower midas = FollowerManager.SpawnCopyFollower(DataManager.Instance.MidasFollowerInfo, ChurchFollowerManager.Instance.DoorPosition.position, (Transform) null, FollowerLocation.Church);
      midas.FollowerBrain.ResetStats();
      bool waiting = true;
      midas.Follower.LockToGround = true;
      midas.Follower.GoTo(Interaction_DonationBox.Instance.transform.position + Vector3.down, (System.Action) (() => waiting = false));
      while (waiting)
        yield return (object) null;
      midas.Follower.Spine.AnimationState.SetAnimation(1, "Jeer/jeer-plotting" + ((double) UnityEngine.Random.value < 0.5 ? "2" : ""), true);
      Interaction_DonationBox.Instance.gameObject.transform.localScale = Vector3.one;
      Interaction_DonationBox.Instance.gameObject.transform.DOKill();
      Interaction_DonationBox.Instance.gameObject.transform.DOPunchScale(Vector3.one * 0.1f, 1f, 5, 0.5f);
      AudioManager.Instance.PlayOneShot("event:/dlc/env/midas/steal_from_temple");
      for (int index = 0; index < 10; ++index)
        ResourceCustomTarget.Create(midas.Follower.gameObject, Interaction_DonationBox.Instance.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) null);
      DataManager.Instance.TempleDevotionBoxCoinCount = 0;
      Interaction_DonationBox.Instance.UpdateGameObjects();
      yield return (object) new WaitForSeconds(2f);
      waiting = true;
      midas.Follower.GoTo(ChurchFollowerManager.Instance.DoorPosition.position, (System.Action) (() => waiting = false));
      yield return (object) new WaitForSeconds(2f);
      FollowerManager.CleanUpCopyFollower(midas);
      GameManager.GetInstance().OnConversationEnd();
      midas = (FollowerManager.SpawnedFollower) null;
    }
    System.Action action = callback;
    if (action != null)
      action();
  }

  public IEnumerator TryOnboardSin(System.Action callback)
  {
    Interaction_TempleAltar interactionTempleAltar = this;
    yield return (object) new WaitForEndOfFrame();
    while (MMConversation.isPlaying)
      yield return (object) null;
    if (DataManager.Instance.BossesCompleted.Count < 3 || DataManager.Instance.PleasureRevealed || Ritual.FollowerToAttendSermon.Count <= 0 || !UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ritual_FirePit))
    {
      System.Action action = callback;
      if (action != null)
        action();
    }
    else
    {
      yield return (object) new WaitForEndOfFrame();
      MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
      MonoSingleton<UIManager>.Instance.ForceBlockPause = true;
      GameManager.GetInstance().OnConversationNew();
      interactionTempleAltar.SinLoop = AudioManager.Instance.CreateLoop("event:/rituals/sins_onboarding_sfx", true, false);
      yield return (object) new WaitForSeconds(0.5f);
      if (interactionTempleAltar._playerFarming.isLamb)
        AudioManager.Instance.PlayOneShot("event:/player/speak_to_follower_noBookPage");
      else
        AudioManager.Instance.PlayOneShot("event:/player/goat_player/goat_speak_to_follower_noBookPage");
      Vector3 spawnPos = Vector3.zero;
      foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
      {
        Follower followerById = FollowerManager.FindFollowerByID(followerBrain.Info.ID);
        if ((bool) (UnityEngine.Object) followerById)
          spawnPos += followerById.transform.position;
      }
      spawnPos /= (float) Ritual.FollowerToAttendSermon.Count;
      float q = 0.0f;
      DOTween.To((DOGetter<float>) (() => q), (DOSetter<float>) (x => q = x), 1f, 1f).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => GameManager.GetInstance().CamFollowTarget.SetOffset(Vector3.Lerp(Vector3.zero, Vector3.forward * 2.25f, q))));
      ChurchFollowerManager.Instance.RedLightingVolume.gameObject.SetActive(true);
      interactionTempleAltar.StartCoroutine((IEnumerator) interactionTempleAltar.ShakeScreen());
      interactionTempleAltar.StartCoroutine((IEnumerator) interactionTempleAltar.Rumble());
      interactionTempleAltar.ResetSprite();
      interactionTempleAltar.StartCoroutine((IEnumerator) interactionTempleAltar.sinOnboardingMenu.SinIntroSequenceIE(new Vector3(0.15f, interactionTempleAltar.playerFarming.CrownBone.position.y, interactionTempleAltar.playerFarming.CrownBone.position.z), spawnPos - Vector3.forward * 5f));
      interactionTempleAltar.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      yield return (object) new WaitForEndOfFrame();
      interactionTempleAltar.playerFarming.state.LockStateChanges = true;
      interactionTempleAltar.playerFarming.Spine.AnimationState.SetAnimation(0, "rituals/sin-onboarding-start", false);
      interactionTempleAltar.playerFarming.Spine.AnimationState.AddAnimation(0, "rituals/sin-onboarding-loop", true, 0.0f);
      yield return (object) new WaitForSeconds(1f);
      List<Follower> followers = new List<Follower>();
      foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
      {
        Follower f = FollowerManager.FindFollowerByID(followerBrain.Info.ID);
        if ((bool) (UnityEngine.Object) f)
        {
          followers.Add(f);
          GameManager.GetInstance().WaitForSeconds(UnityEngine.Random.Range(0.0f, 0.5f), (System.Action) (() =>
          {
            double num = (double) f.SetBodyAnimation("Sin/idle-ritual-sin-lookup", false);
            f.AddBodyAnimation("Sin/idle-ritual-sin-loop", true, 0.0f);
          }));
        }
      }
      yield return (object) new WaitForSeconds(3f);
      foreach (Follower follower1 in followers)
      {
        Follower follower = follower1;
        double num;
        GameManager.GetInstance().WaitForSeconds(UnityEngine.Random.Range(0.0f, 0.5f), (System.Action) (() => num = (double) follower.SetBodyAnimation("Sin/idle-ritual-sin" + UnityEngine.Random.Range(1, 4).ToString(), true)));
      }
      yield return (object) new WaitForSeconds(2f);
      ChurchFollowerManager.Instance.PlayOverlay(ChurchFollowerManager.OverlayType.Sacrifice, "snakes-combined");
      AudioManager.Instance.PlayOneShot("event:/rituals/snakes_hissing");
      MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact, interactionTempleAltar.playerFarming);
      interactionTempleAltar.StartCoroutine((IEnumerator) interactionTempleAltar.SpawnSnakes(spawnPos));
      yield return (object) new WaitForSeconds(0.5f);
      foreach (Follower follower2 in followers)
      {
        Follower follower = follower2;
        double num;
        GameManager.GetInstance().WaitForSeconds(UnityEngine.Random.Range(0.0f, 0.5f), (System.Action) (() => num = (double) follower.SetBodyAnimation("Reactions/react-scared-long", true)));
      }
      yield return (object) new WaitForSeconds(3f);
      interactionTempleAltar.sinOnboardingMenu.Crown.transform.DOKill();
      GameManager.GetInstance().CamFollowTarget.MoveSpeed *= 2f;
      interactionTempleAltar.sinOnboardingMenu.Crown.AnimationState.SetAnimation(0, "moving-up", true);
      q = 0.0f;
      float speed = 10f;
      Quaternion fromRot = GameManager.GetInstance().CamFollowTarget.transform.rotation;
      while ((double) q < 3.0)
      {
        q += Time.deltaTime;
        interactionTempleAltar.sinOnboardingMenu.Crown.transform.position -= Vector3.forward * speed * Time.deltaTime;
        if ((double) q < 1.5)
          GameManager.GetInstance().CamFollowTarget.transform.rotation = Quaternion.Lerp(fromRot, Quaternion.Euler(-60f, 0.0f, 0.0f), q / 1.5f);
        yield return (object) null;
      }
      interactionTempleAltar.sinOnboardingMenu.Crown.AnimationState.SetAnimation(0, "snake-appear", true);
      yield return (object) new WaitForSeconds(2.33f);
      interactionTempleAltar.sinOnboardingMenu.SinBackgroundSpine.AnimationState.SetAnimation(0, "start", false);
      interactionTempleAltar.sinOnboardingMenu.SinBackgroundSpine.AnimationState.AddAnimation(0, "loop", true, 0.0f);
      ChurchFollowerManager.Instance.PlayOverlay(ChurchFollowerManager.OverlayType.Ritual, "sin-start", "sin-loop", true);
      AudioManager.Instance.PlayOneShot("event:/rituals/generic_start_ritual", interactionTempleAltar.playerFarming.gameObject);
      yield return (object) new WaitForSeconds(1.5f);
      GameManager.GetInstance().CamFollowTarget.enabled = false;
      foreach (Follower follower in followers)
      {
        double num = (double) follower.SetBodyAnimation("idle-ritual-up", true);
      }
      DataManager.Instance.PleasureRevealed = true;
      yield return (object) interactionTempleAltar.StartCoroutine((IEnumerator) interactionTempleAltar.sinOnboardingMenu.CrownSequence());
      GameManager.GetInstance().CamFollowTarget.MoveSpeed /= 2f;
      yield return (object) new WaitForSeconds(0.5f);
      MonoSingleton<UIManager>.Instance.ForceBlockPause = false;
      MonoSingleton<UIManager>.Instance.ForceBlockMenus = true;
      DataManager.Instance.PleasureDoctrineEnabled = true;
      interactionTempleAltar.forceRitual = UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ritual_Purge) ? UpgradeSystem.Type.Ritual_Purge : UpgradeSystem.Type.Ritual_Nudism;
      UpgradeSystem.ClearCooldown(interactionTempleAltar.forceRitual);
      interactionTempleAltar.DoRitual();
    }
  }

  public IEnumerator ShakeScreen()
  {
    float t = 0.1f;
    float duration = 10.25f;
    while ((double) t < (double) duration)
    {
      DeviceLightingManager.TransitionLighting(Color.black, Color.red, t / duration);
      t += Time.deltaTime;
      CameraManager.shakeCamera(t / 13f);
      yield return (object) null;
    }
    CameraManager.shakeCamera(0.0f, false);
  }

  public IEnumerator Rumble()
  {
    Interaction_TempleAltar interactionTempleAltar = this;
    float t = 0.1f;
    float duration = 15.25f;
    while ((double) t < (double) duration)
    {
      t += Time.deltaTime;
      MMVibrate.RumbleContinuous(interactionTempleAltar.controllerRumbleCurve.Evaluate(t / duration), interactionTempleAltar.controllerRumbleCurve.Evaluate((float) ((double) t / (double) duration * 2.0)), interactionTempleAltar.playerFarming);
      yield return (object) null;
    }
    MMVibrate.StopRumble(interactionTempleAltar.playerFarming);
  }

  public IEnumerator SpawnSnakes(Vector3 spawnPos)
  {
    for (int i = 0; i < UnityEngine.Random.Range(25, 35); ++i)
    {
      SinSnake.Spawn(spawnPos, (float) UnityEngine.Random.Range(0, 360), ChurchFollowerManager.Instance.transform, UnityEngine.Random.Range(1f, 2f), 2.35f);
      yield return (object) new WaitForSeconds(UnityEngine.Random.Range(0.015f, 0.05f));
    }
  }

  public void CloseAndSpeak(string ConversationEntryTerm)
  {
    MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
    {
      new ConversationEntry(this.gameObject, "FollowerInteractions/" + ConversationEntryTerm, "idle")
    }, (List<MMTools.Response>) null, (System.Action) null));
  }

  public IEnumerator FollowersEnterForSermonRoutine(bool shufflePosition = false)
  {
    if (Ritual.FollowerToAttendSermon == null || Ritual.FollowerToAttendSermon.Count <= 0)
      Ritual.FollowerToAttendSermon = Ritual.GetFollowersAvailableToAttendSermon();
    if (TimeManager.CurrentPhase == DayPhase.Night)
      DataManager.Instance.WokeUpEveryoneDay = TimeManager.CurrentDay;
    for (int i = Ritual.FollowerToAttendSermon.Count - 1; i >= 0; --i)
    {
      if (Ritual.FollowerToAttendSermon.Count > i)
      {
        Follower followerById = FollowerManager.FindFollowerByID(Ritual.FollowerToAttendSermon[i].Info.ID);
        if ((bool) (UnityEngine.Object) followerById)
        {
          Ritual.FollowerToAttendSermon[i].ShouldReconsiderTask = false;
          followerById.HideAllFollowerIcons();
          followerById.Spine.UseDeltaTime = false;
          followerById.UseUnscaledTime = true;
        }
        if (!(Ritual.FollowerToAttendSermon[i].CurrentTask is FollowerTask_AttendTeaching) || shufflePosition)
        {
          if (Ritual.FollowerToAttendSermon[i].CurrentTask != null)
            Ritual.FollowerToAttendSermon[i].CurrentTask.Abort();
          Ritual.FollowerToAttendSermon[i].HardSwapToTask((FollowerTask) new FollowerTask_AttendTeaching());
          Ritual.FollowerToAttendSermon[i].ShouldReconsiderTask = false;
          if (Ritual.FollowerToAttendSermon[i].CurrentTaskType == FollowerTaskType.ChangeLocation)
            Ritual.FollowerToAttendSermon[i].CurrentTask.Arrive();
          if (i < 12)
            yield return (object) new WaitForSecondsRealtime(UnityEngine.Random.Range(0.05f, 0.15f));
        }
      }
    }
    float timer = 0.0f;
    while (!this.FollowersInPosition() && (double) (timer += Time.deltaTime) < 10.0)
      yield return (object) null;
  }

  public bool ReadyForTeaching()
  {
    bool flag = true;
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (followerBrain.ShouldReconsiderTask || followerBrain.CurrentTaskType == FollowerTaskType.ChangeLocation || followerBrain.Location != PlayerFarming.Location || followerBrain.CurrentTaskType == FollowerTaskType.AttendTeaching && followerBrain.CurrentTask.State != FollowerTaskState.GoingTo)
        flag = false;
    }
    return flag;
  }

  public bool FollowersInPosition()
  {
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (followerBrain.CurrentTaskType == FollowerTaskType.ChangeLocation || followerBrain.Location != PlayerFarming.Location || followerBrain.CurrentTaskType == FollowerTaskType.AttendTeaching && followerBrain.CurrentTask.State != FollowerTaskState.Doing)
      {
        if (followerBrain.Location != PlayerFarming.Location)
        {
          followerBrain.HardSwapToTask((FollowerTask) new FollowerTask_AttendTeaching());
          followerBrain.ShouldReconsiderTask = false;
          if (followerBrain.CurrentTaskType == FollowerTaskType.ChangeLocation)
            followerBrain.CurrentTask.Arrive();
        }
        return false;
      }
    }
    return true;
  }

  public IEnumerator TeachSermonRoutine()
  {
    Interaction_TempleAltar interactionTempleAltar = this;
    interactionTempleAltar.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    interactionTempleAltar.playerFarming.simpleSpineAnimator.Animate("build", 0, true);
    AudioManager.Instance.PlayOneShot("event:/sermon/start_sermon", interactionTempleAltar.playerFarming.gameObject);
    AudioManager.Instance.PlayOneShot("event:/building/building_bell_ring", interactionTempleAltar.playerFarming.gameObject);
    interactionTempleAltar.StartCoroutine((IEnumerator) interactionTempleAltar.CentrePlayer());
    GameManager.GetInstance().OnConversationNext(interactionTempleAltar.playerFarming.CameraBone, 12f);
    yield return (object) interactionTempleAltar.StartCoroutine((IEnumerator) interactionTempleAltar.FollowersEnterForSermonRoutine());
    SimulationManager.Pause();
    GameManager.GetInstance().OnConversationNext(interactionTempleAltar.playerFarming.CameraBone, 7f);
    GameManager.GetInstance().CameraSetOffset(new Vector3(0.0f, 0.0f, -0.5f));
    yield return (object) new WaitForSeconds(0.5f);
    interactionTempleAltar.SermonsStillAvailable = false;
    if (DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.Afterlife) < 4)
      interactionTempleAltar.SermonsStillAvailable = true;
    if (DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.Food) < 4)
      interactionTempleAltar.SermonsStillAvailable = true;
    if (DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.Possession) < 4)
      interactionTempleAltar.SermonsStillAvailable = true;
    if (DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.WorkAndWorship) < 4)
      interactionTempleAltar.SermonsStillAvailable = true;
    if (DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.LawAndOrder) < 4)
      interactionTempleAltar.SermonsStillAvailable = true;
    if (DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.Pleasure) < 4 && DataManager.Instance.PleasureDoctrineEnabled)
      interactionTempleAltar.SermonsStillAvailable = true;
    if (DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.Winter) < 4 && SeasonsManager.Active)
      interactionTempleAltar.SermonsStillAvailable = true;
    if (DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.Special) < 3)
    {
      Debug.Log((object) "A");
      interactionTempleAltar.SermonCategory = SermonCategory.Special;
      interactionTempleAltar.SermonsStillAvailable = true;
      interactionTempleAltar.StartCoroutine((IEnumerator) interactionTempleAltar.GatherFollowers());
    }
    else if (interactionTempleAltar.SermonsStillAvailable)
    {
      AudioManager.Instance.PlayOneShot("event:/sermon/sermon_menu_appear", interactionTempleAltar.playerFarming.gameObject);
      SermonCategory finalisedCategory = SermonCategory.None;
      UISermonWheelController sermonWheelController = MonoSingleton<UIManager>.Instance.SermonWheelTemplate.Instantiate<UISermonWheelController>();
      sermonWheelController.OnItemChosen = sermonWheelController.OnItemChosen + (System.Action<SermonCategory>) (chosenCategory =>
      {
        Debug.Log((object) $"Chose category {chosenCategory}".Colour(Color.yellow));
        finalisedCategory = chosenCategory;
      });
      sermonWheelController.OnHide = sermonWheelController.OnHide + (System.Action) (() =>
      {
        if (finalisedCategory == SermonCategory.None)
        {
          this.DoCancel();
          foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
          {
            try
            {
              followerBrain.CurrentTask.End();
            }
            catch (Exception ex)
            {
              Debug.Log((object) followerBrain.Info.Name);
            }
          }
        }
        else
        {
          this.SermonCategory = finalisedCategory;
          this.StartCoroutine((IEnumerator) this.GatherFollowers());
          AudioManager.Instance.PlayOneShot("event:/sermon/select_sermon", this.playerFarming.gameObject);
        }
      });
      sermonWheelController.Show();
    }
    else
    {
      interactionTempleAltar.SermonCategory = SermonCategory.None;
      interactionTempleAltar.StartCoroutine((IEnumerator) interactionTempleAltar.GatherFollowers());
    }
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    AudioManager.Instance.StopLoop(this.sermonLoop);
  }

  public new void OnDestroy()
  {
    CoopManager.Instance.OnPlayerLeft -= new System.Action(this.playerLeft);
    CoopManager.Instance.OnPlayerJoined -= new System.Action(this.playerJoined);
    if (Ritual.FollowerToAttendSermon != null)
      Ritual.FollowerToAttendSermon.Clear();
    AudioManager.Instance.StopLoop(this.sermonLoop);
    AudioManager.Instance.StopLoop(this.SinLoop);
    Ritual.OnEnd -= new System.Action<bool>(this.RitualOnEnd);
    if ((UnityEngine.Object) MonoSingleton<UIManager>.Instance != (UnityEngine.Object) null)
      MonoSingleton<UIManager>.Instance.ForceBlockPause = false;
    base.OnDestroy();
  }

  public void playerLeft() => this.playerLeftDuringRitual = true;

  public void playerJoined() => this.playerLeftDuringRitual = false;

  public void PulseDisplacementObject(Vector3 Position)
  {
    BiomeConstants.Instance.EmitDisplacementEffect(Position);
  }

  public IEnumerator GatherFollowers()
  {
    Interaction_TempleAltar interactionTempleAltar = this;
    GameManager.GetInstance().CameraSetOffset(new Vector3(0.0f, 0.0f, 1f));
    GameManager.GetInstance().OnConversationNext(interactionTempleAltar.playerFarming.CameraBone, 7f);
    interactionTempleAltar.playerFarming.Spine.skeleton.FindBone("ritualring").Rotation += 60f;
    interactionTempleAltar.playerFarming.Spine.skeleton.UpdateWorldTransform();
    interactionTempleAltar.playerFarming.Spine.skeleton.Update(Time.deltaTime);
    interactionTempleAltar.playerFarming.simpleSpineAnimator.Animate("sermons/sermon-start", 0, false);
    interactionTempleAltar.playerFarming.simpleSpineAnimator.AddAnimate("sermons/sermon-loop", 0, true, 0.0f);
    interactionTempleAltar.sermonLoop = AudioManager.Instance.CreateLoop("event:/sermon/preach_loop", interactionTempleAltar.playerFarming.gameObject, true, false);
    yield return (object) new WaitForSeconds(0.6f);
    interactionTempleAltar.PulseDisplacementObject(interactionTempleAltar.state.transform.position);
    yield return (object) new WaitForSeconds(0.4f);
    ChurchFollowerManager.Instance.StartSermonEffect();
    bool askedQuestion = false;
    if (interactionTempleAltar.SermonsStillAvailable)
    {
      askedQuestion = true;
      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(interactionTempleAltar.DoctrineXPPrefab, GameObject.FindWithTag("Canvas").transform);
      interactionTempleAltar.UIDoctrineBar = gameObject.GetComponent<UIDoctrineBar>();
      float xp = DoctrineUpgradeSystem.GetXPBySermon(interactionTempleAltar.SermonCategory);
      float target = DoctrineUpgradeSystem.GetXPTargetBySermon(interactionTempleAltar.SermonCategory);
      float num = 1.5f - (target - xp);
      yield return (object) interactionTempleAltar.StartCoroutine((IEnumerator) interactionTempleAltar.UIDoctrineBar.Show(xp, interactionTempleAltar.SermonCategory));
      GameManager.GetInstance().OnConversationNext(interactionTempleAltar.playerFarming.CameraBone, 11f);
      int studyIncrementCount = Mathf.FloorToInt(DataManager.Instance.TempleStudyXP / 0.1f);
      float delay = 1.5f / (float) studyIncrementCount;
      if (studyIncrementCount > 0)
      {
        DataManager.Instance.TempleStudyXP -= (float) studyIncrementCount * 0.1f;
        for (int i = 0; i < studyIncrementCount; ++i)
        {
          xp += 0.1f;
          if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Temple_SermonEfficiency))
            xp += 0.05f;
          if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Temple_SermonEfficiencyII))
            xp += 0.05f;
          target = DoctrineUpgradeSystem.GetXPTargetBySermon(interactionTempleAltar.SermonCategory);
          SoulCustomTarget.Create(interactionTempleAltar.gameObject, ChurchFollowerManager.Instance.StudySlots[UnityEngine.Random.Range(0, ChurchFollowerManager.Instance.StudySlots.Length)].transform.position, Color.white, new System.Action(interactionTempleAltar.\u003CGatherFollowers\u003Eb__90_1), 0.2f, 500f);
          yield return (object) new WaitForSeconds(delay);
          if ((double) xp >= (double) target)
          {
            yield return (object) new WaitForSeconds(0.5f);
            yield return (object) interactionTempleAltar.StartCoroutine((IEnumerator) interactionTempleAltar.UIDoctrineBar.UpdateSecondBar(xp, 0.5f));
            AudioManager.Instance.PlayOneShot("event:/sermon/upgrade_menu_appear");
            yield return (object) interactionTempleAltar.StartCoroutine((IEnumerator) interactionTempleAltar.UIDoctrineBar.FlashBarRoutine(0.3f, 1f));
            yield return (object) interactionTempleAltar.StartCoroutine((IEnumerator) interactionTempleAltar.UIDoctrineBar.Hide());
            yield return (object) interactionTempleAltar.StartCoroutine((IEnumerator) interactionTempleAltar.AskQuestionRoutine(InventoryItem.ITEM_TYPE.CRYSTAL_DOCTRINE_STONE));
            xp = 0.0f;
            if (DoctrineUpgradeSystem.GetLevelBySermon(interactionTempleAltar.SermonCategory) < 4)
            {
              if (i < studyIncrementCount - 1)
              {
                yield return (object) interactionTempleAltar.StartCoroutine((IEnumerator) interactionTempleAltar.UIDoctrineBar.Show(0.0f, interactionTempleAltar.SermonCategory));
                interactionTempleAltar.barLocalXP = 0.0f;
              }
            }
            else
              break;
          }
        }
        yield return (object) new WaitForSeconds(1f);
        yield return (object) interactionTempleAltar.StartCoroutine((IEnumerator) interactionTempleAltar.UIDoctrineBar.UpdateSecondBar(xp, 0.5f));
        yield return (object) new WaitForSeconds(0.5f);
      }
      int followersCount = Ritual.FollowerToAttendSermon.Count;
      delay = 1.5f / (float) followersCount;
      interactionTempleAltar.barLocalXP = xp;
      int count = 0;
      do
      {
        xp += 0.1f;
        if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Temple_SermonEfficiency))
          xp += 0.05f;
        if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Temple_SermonEfficiencyII))
          xp += 0.05f;
        target = DoctrineUpgradeSystem.GetXPTargetBySermon(interactionTempleAltar.SermonCategory);
        SoulCustomTarget.Create(interactionTempleAltar.gameObject, Ritual.FollowerToAttendSermon[count].LastPosition, Color.white, new System.Action(interactionTempleAltar.\u003CGatherFollowers\u003Eb__90_0), 0.2f, 500f);
        yield return (object) new WaitForSeconds(delay);
        if ((double) xp >= (double) target)
        {
          yield return (object) new WaitForSeconds(0.5f);
          yield return (object) interactionTempleAltar.StartCoroutine((IEnumerator) interactionTempleAltar.UIDoctrineBar.UpdateSecondBar(xp, 0.5f));
          yield return (object) interactionTempleAltar.StartCoroutine((IEnumerator) interactionTempleAltar.UIDoctrineBar.FlashBarRoutine(0.3f, 1f));
          yield return (object) interactionTempleAltar.StartCoroutine((IEnumerator) interactionTempleAltar.UIDoctrineBar.Hide());
          yield return (object) interactionTempleAltar.StartCoroutine((IEnumerator) interactionTempleAltar.AskQuestionRoutine(InventoryItem.ITEM_TYPE.CRYSTAL_DOCTRINE_STONE));
          xp = 0.0f;
          if (DoctrineUpgradeSystem.GetLevelBySermon(interactionTempleAltar.SermonCategory) < 4)
          {
            if (count < followersCount - 1)
            {
              yield return (object) interactionTempleAltar.StartCoroutine((IEnumerator) interactionTempleAltar.UIDoctrineBar.Show(0.0f, interactionTempleAltar.SermonCategory));
              interactionTempleAltar.barLocalXP = 0.0f;
            }
          }
          else
            break;
        }
        ++count;
      }
      while (count < followersCount);
      ChurchFollowerManager.Instance.EndSermonEffect();
      yield return (object) new WaitForSeconds(0.5f);
      yield return (object) interactionTempleAltar.StartCoroutine((IEnumerator) interactionTempleAltar.UIDoctrineBar.UpdateSecondBar(xp, 0.5f));
      yield return (object) new WaitForSeconds(0.5f);
      UnityEngine.Object.Destroy((UnityEngine.Object) interactionTempleAltar.UIDoctrineBar.gameObject);
      DoctrineUpgradeSystem.SetXPBySermon(interactionTempleAltar.SermonCategory, xp);
    }
    else
    {
      yield return (object) new WaitForSeconds(1f);
      ChurchFollowerManager.Instance.EndSermonEffect();
    }
    while (MMConversation.CURRENT_CONVERSATION != null)
      yield return (object) null;
    AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("DELIVER_FIRST_SERMON"));
    interactionTempleAltar.playerFarming.simpleSpineAnimator.Animate("sermons/sermon-stop", 0, false);
    interactionTempleAltar.playerFarming.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/sermon/end_sermon", interactionTempleAltar.playerFarming.gameObject);
    int num1 = (int) interactionTempleAltar.sermonLoop.stop(STOP_MODE.ALLOWFADEOUT);
    AudioManager.Instance.StopLoop(interactionTempleAltar.sermonLoop);
    yield return (object) new WaitForSeconds(0.333333343f);
    interactionTempleAltar.ResetSprite();
    AudioManager.Instance.PlayOneShot("event:/sermon/book_put_down", interactionTempleAltar.playerFarming.gameObject);
    GameManager.GetInstance().OnConversationEnd();
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    GameManager.GetInstance().CamFollowTarget.DisablePlayerLook = false;
    DataManager.Instance.PreviousSermonDayIndex = TimeManager.CurrentDay;
    foreach (PlayerFarming player in PlayerFarming.players)
      player.Spine.UseDeltaTime = true;
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.CurrentTaskType == FollowerTaskType.AttendTeaching)
      {
        if (DataManager.Instance.UnlockededASermon > 1 && interactionTempleAltar.SermonCategory != SermonCategory.None && interactionTempleAltar.SermonCategory == DataManager.Instance.PreviousSermonCategory)
          allBrain.AddThought(Thought.WatchedSermonSameAsYesterday);
        else if (allBrain.HasTrait(FollowerTrait.TraitType.SermonEnthusiast))
          allBrain.AddThought(Thought.WatchedSermonDevotee);
        else
          allBrain.AddThought(Thought.WatchedSermon);
        allBrain.AddAdoration(FollowerBrain.AdorationActions.Sermon, (System.Action) null);
        interactionTempleAltar.StartCoroutine((IEnumerator) interactionTempleAltar.DelayFollowerReaction(allBrain, UnityEngine.Random.Range(0.1f, 0.5f)));
        FollowerManager.FindFollowerByID(allBrain.Info.ID)?.ShowAllFollowerIcons(false);
      }
    }
    DataManager.Instance.PreviousSermonCategory = interactionTempleAltar.SermonCategory;
    interactionTempleAltar.ResetSprite();
    yield return (object) new WaitForSeconds(0.5f);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.GiveSermon);
    interactionTempleAltar.Activated = false;
    yield return (object) new WaitForSeconds(1.5f);
    if (DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.SermonEnthusiast))
      CultFaithManager.AddThought(Thought.Cult_SermonEnthusiast_Trait);
    else
      CultFaithManager.AddThought(Thought.Cult_Sermon);
    if (DataManager.Instance.WokeUpEveryoneDay == TimeManager.CurrentDay && TimeManager.CurrentPhase == DayPhase.Night && FollowerBrainStats.ShouldSleep)
      CultFaithManager.AddThought(Thought.Cult_WokeUpEveryone);
    if (!askedQuestion)
    {
      foreach (PlayerFarming player in PlayerFarming.players)
      {
        player.health.BlueHearts += 2f;
        if (player.isLamb)
          DataManager.Instance.PLAYER_BLUE_HEARTS += 2f;
        else
          DataManager.Instance.COOP_PLAYER_BLUE_HEARTS += 2f;
      }
    }
  }

  public void IncrementXPBar()
  {
    this.barLocalXP += 0.1f;
    this.StartCoroutine((IEnumerator) this.UIDoctrineBar.UpdateFirstBar(this.barLocalXP, 0.1f));
  }

  public IEnumerator AskQuestionRoutine(InventoryItem.ITEM_TYPE currency)
  {
    Interaction_TempleAltar interactionTempleAltar = this;
    if (interactionTempleAltar._playerFarming.isLamb)
      AudioManager.Instance.PlayOneShot("event:/sermon/sermon_speech_bubble", interactionTempleAltar.playerFarming.gameObject);
    else
      AudioManager.Instance.PlayOneShot("event:/sermon/goat_sermon_speech_bubble", interactionTempleAltar.playerFarming.gameObject);
    interactionTempleAltar.GivenAnswer = false;
    interactionTempleAltar.RewardLevel = currency != InventoryItem.ITEM_TYPE.CRYSTAL_DOCTRINE_STONE ? DoctrineUpgradeSystem.GetLevelBySermon(interactionTempleAltar.SermonCategory) + 1 : 5;
    string TermToSpeak = $"DoctrineUpgradeSystem/{interactionTempleAltar.SermonCategory.ToString()}{interactionTempleAltar.RewardLevel.ToString()}";
    if (interactionTempleAltar.SermonCategory == SermonCategory.Special || interactionTempleAltar.RewardLevel >= 5)
      TermToSpeak = " ";
    Debug.Log((object) $"Text: DoctrineUpgradeSystem/{interactionTempleAltar.SermonCategory.ToString()}{interactionTempleAltar.RewardLevel.ToString()}");
    List<ConversationEntry> Entries = new List<ConversationEntry>();
    Entries.Add(new ConversationEntry(interactionTempleAltar.playerFarming.CameraBone, TermToSpeak));
    Entries[0].CharacterName = "DoctrineUpgradeSystem/DoctrinalDecision";
    List<DoctrineResponse> DoctrineResponses = new List<DoctrineResponse>()
    {
      new DoctrineResponse(interactionTempleAltar.SermonCategory, interactionTempleAltar.RewardLevel, true, new System.Action(interactionTempleAltar.\u003CAskQuestionRoutine\u003Eb__94_0)),
      new DoctrineResponse(interactionTempleAltar.SermonCategory, interactionTempleAltar.RewardLevel, false, new System.Action(interactionTempleAltar.\u003CAskQuestionRoutine\u003Eb__94_1))
    };
    if (DoctrineUpgradeSystem.GetSermonReward(interactionTempleAltar.SermonCategory, interactionTempleAltar.RewardLevel, false) == DoctrineUpgradeSystem.DoctrineType.None)
      DoctrineResponses.RemoveAt(1);
    if (interactionTempleAltar.SermonCategory == SermonCategory.Special)
      DoctrineResponses = new List<DoctrineResponse>()
      {
        new DoctrineResponse(interactionTempleAltar.SermonCategory, interactionTempleAltar.RewardLevel, true, new System.Action(interactionTempleAltar.\u003CAskQuestionRoutine\u003Eb__94_2))
      };
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) null, DoctrineResponses), false, false, false, showControlPrompt: false);
    while (!interactionTempleAltar.GivenAnswer)
      yield return (object) null;
    Interaction_TempleAltar.DoctrineUnlockType = DoctrineUpgradeSystem.GetSermonReward(interactionTempleAltar.SermonCategory, interactionTempleAltar.RewardLevel, interactionTempleAltar.firstChoice);
    DoctrineUpgradeSystem.UnlockAbility(Interaction_TempleAltar.DoctrineUnlockType);
    if (currency != InventoryItem.ITEM_TYPE.CRYSTAL_DOCTRINE_STONE)
      DoctrineUpgradeSystem.SetLevelBySermon(interactionTempleAltar.SermonCategory, DoctrineUpgradeSystem.GetLevelBySermon(interactionTempleAltar.SermonCategory) + 1);
    HUD_Manager.Instance.Hide(true, 0);
  }

  public void Reply(bool FirstChoice)
  {
    this.firstChoice = FirstChoice;
    this.GivenAnswer = true;
    Debug.Log((object) ("CALLBACK! " + FirstChoice.ToString()));
  }

  public IEnumerator DelayFollowerReaction(FollowerBrain brain, float Delay)
  {
    yield return (object) new WaitForSecondsRealtime(Delay);
    Follower f = FollowerManager.FindFollowerByID(brain.Info.ID);
    if (!((UnityEngine.Object) f == (UnityEngine.Object) null))
    {
      f.HideAllFollowerIcons();
      f.TimedAnimation("Reactions/react-enlightened1", 1.5f, (System.Action) (() =>
      {
        f.UseUnscaledTime = true;
        f.Brain.CurrentTask.StartAgain(f);
      }), false, false);
      float num = 0.0f;
      foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
        num += allBrain.Stats.Happiness;
      if ((double) num / (100.0 * (double) FollowerBrain.AllBrains.Count) <= 0.20000000298023224)
        NotificationCentre.Instance.PlayGenericNotification(NotificationCentre.NotificationType.LowFaithDonation);
    }
  }

  public IEnumerator CentrePlayer()
  {
    Interaction_TempleAltar interactionTempleAltar = this;
    interactionTempleAltar.state.facingAngle = 270f;
    float Progress = 0.0f;
    Vector3 StartPosition = interactionTempleAltar.state.transform.position;
    while ((double) (Progress += Time.deltaTime) < 0.25)
    {
      interactionTempleAltar.state.transform.position = Vector3.Lerp(StartPosition, ChurchFollowerManager.Instance.AltarPosition.position, Mathf.SmoothStep(0.0f, 1f, Progress / 0.25f));
      yield return (object) null;
    }
  }

  public override void OnSecondaryInteract(StateMachine state)
  {
    if (!UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Temple2) || !this.HasBuiltTemple2)
    {
      this.playerFarming.indicator.PlayShake();
    }
    else
    {
      if (this.Activating)
        return;
      base.OnSecondaryInteract(state);
      this.Activating = true;
      PlayerFarming playerFarming = state.GetComponent<PlayerFarming>();
      this.OnBecomeNotCurrent(playerFarming);
      this.HasChanged = true;
      GameManager.GetInstance().OnConversationNew(false);
      GameManager.GetInstance().OnConversationNext(playerFarming.CameraBone, 8f);
      GameManager.GetInstance().CamFollowTarget.DisablePlayerLook = true;
      playerFarming.GoToAndStop(ChurchFollowerManager.Instance.AltarPosition.gameObject, this.gameObject, GoToCallback: (System.Action) (() =>
      {
        AudioManager.Instance.PlayOneShot("event:/ritual_sacrifice/start_ritual", playerFarming.gameObject);
        this.RitualAvailableAnimator.Play("Hidden");
        this.StartCoroutine((IEnumerator) this.CentrePlayer());
        this.StartCoroutine((IEnumerator) this.OpenRitualMenuRoutine());
      }));
    }
  }

  public IEnumerator OpenRitualMenuRoutine()
  {
    yield return (object) new WaitForSeconds(0.5f);
    this.OpenRitualMenu();
  }

  public void OpenRitualMenu()
  {
    UIRitualsMenuController ritualsMenuController = MonoSingleton<UIManager>.Instance.RitualsMenuTemplate.Instantiate<UIRitualsMenuController>();
    ritualsMenuController.Show(this.forceRitual, this.forceRitual != UpgradeSystem.Type.Count);
    ritualsMenuController.OnRitualSelected += new System.Action<UpgradeSystem.Type>(this.PerformRitual);
    ritualsMenuController.OnCancel = ritualsMenuController.OnCancel + (System.Action) (() =>
    {
      GameManager.GetInstance().CameraSetOffset(Vector3.zero);
      this.OnInteract(this.state);
    });
  }

  public void PerformRitual(UpgradeSystem.Type RitualType)
  {
    CoopManager.Instance.temporaryDisableRemoval = true;
    SimulationManager.UnPause();
    Ritual.FollowerToAttendSermon = Ritual.GetFollowersAvailableToAttendSermon();
    foreach (FollowerBrain brain in Ritual.FollowerToAttendSermon)
      ChurchFollowerManager.Instance.AddBrainToAudience(brain);
    this.RitualType = RitualType;
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    if ((UnityEngine.Object) this.CurrentRitual != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.CurrentRitual);
    Ritual.OnEnd += new System.Action<bool>(this.RitualOnEnd);
    Debug.Log((object) ("Perform ritual: " + RitualType.ToString()));
    ObjectiveManager.SetRitualObjectivesFailLocked();
    switch (RitualType)
    {
      case UpgradeSystem.Type.Ritual_Sacrifice:
        RitualSacrifice ritualSacrifice = this.gameObject.AddComponent<RitualSacrifice>();
        ritualSacrifice.RitualLight = this.RitualLighting;
        ritualSacrifice.Play();
        this.CurrentRitual = (Ritual) ritualSacrifice;
        break;
      case UpgradeSystem.Type.Ritual_Reindoctrinate:
        RitualReindoctrinate ritualReindoctrinate = this.gameObject.AddComponent<RitualReindoctrinate>();
        ritualReindoctrinate.Play();
        this.CurrentRitual = (Ritual) ritualReindoctrinate;
        break;
      case UpgradeSystem.Type.Ritual_ConsumeFollower:
        RitualConsumeFollower ritualConsumeFollower = this.gameObject.AddComponent<RitualConsumeFollower>();
        ritualConsumeFollower.Play();
        this.CurrentRitual = (Ritual) ritualConsumeFollower;
        break;
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful1:
        RitualFlockOfTheFaithful flockOfTheFaithful = this.gameObject.AddComponent<RitualFlockOfTheFaithful>();
        flockOfTheFaithful.ritualLight = this.RitualLighting;
        flockOfTheFaithful.Play();
        this.CurrentRitual = (Ritual) flockOfTheFaithful;
        break;
      case UpgradeSystem.Type.Ritual_UnlockWeapon:
        this.StartCoroutine((IEnumerator) this.DelayCallback(0.5f, (System.Action) (() => (UnityEngine.Object.Instantiate(Resources.Load("Prefabs/UI/UI Unlock Weapon"), GameObject.FindWithTag("Canvas").transform) as GameObject).GetComponent<UIUnlockWeapon>().Init((System.Action) (() =>
        {
          this.CurrentRitual = (Ritual) this.gameObject.AddComponent<RitualUnlockWeapon>();
          this.CurrentRitual.Play();
        }), (System.Action) (() => this.OpenRitualMenu())))));
        break;
      case UpgradeSystem.Type.Ritual_UnlockCurse:
        this.StartCoroutine((IEnumerator) this.DelayCallback(0.5f, (System.Action) (() => (UnityEngine.Object.Instantiate(Resources.Load("Prefabs/UI/UI Unlock Curse"), GameObject.FindWithTag("Canvas").transform) as GameObject).GetComponent<UIUnlockCurse>().Init((System.Action) (() =>
        {
          this.CurrentRitual = (Ritual) this.gameObject.AddComponent<RitualUnlockCurse>();
          this.CurrentRitual.Play();
        }), (System.Action) (() => this.OpenRitualMenu())))));
        break;
      case UpgradeSystem.Type.Ritual_FasterBuilding:
        this.CurrentRitual = (Ritual) this.gameObject.AddComponent<RitualFasterBuilding>();
        this.CurrentRitual.Play();
        break;
      case UpgradeSystem.Type.Ritual_Enlightenment:
        Debug.Log((object) "ENLIGHTENMENT!");
        this.CurrentRitual = (Ritual) this.gameObject.AddComponent<RitualElightenment>();
        this.CurrentRitual.Play();
        break;
      case UpgradeSystem.Type.Ritual_WorkThroughNight:
        this.CurrentRitual = (Ritual) this.gameObject.AddComponent<RitualWorkThroughNight>();
        this.CurrentRitual.Play();
        break;
      case UpgradeSystem.Type.Ritual_Holiday:
        this.CurrentRitual = (Ritual) this.gameObject.AddComponent<RitualWorkHoliday>();
        this.CurrentRitual.Play();
        break;
      case UpgradeSystem.Type.Ritual_AlmsToPoor:
        this.CurrentRitual = (Ritual) this.gameObject.AddComponent<RitualAlmsToPoor>();
        this.CurrentRitual.Play();
        break;
      case UpgradeSystem.Type.Ritual_DonationRitual:
        this.CurrentRitual = (Ritual) this.gameObject.AddComponent<RitualDonation>();
        this.CurrentRitual.Play();
        break;
      case UpgradeSystem.Type.Ritual_Fast:
        this.CurrentRitual = (Ritual) this.gameObject.AddComponent<RitualFast>();
        this.CurrentRitual.Play();
        using (List<FollowerBrain>.Enumerator enumerator = FollowerBrain.AllBrains.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            FollowerBrain current = enumerator.Current;
            current.Stats.Starvation = 0.0f;
            current.Stats.Satiation = 100f;
            int num = UnityEngine.Random.Range(0, 10);
            if (Utils.Between((float) num, 0.0f, 6f))
              current.AddThought(Thought.Fasting);
            else if (Utils.Between((float) num, 6f, 8f))
              current.AddThought(Thought.AngryAboutFasting);
            else if (Utils.Between((float) num, 8f, 10f))
              current.AddThought(Thought.HappyAboutFasting);
          }
          break;
        }
      case UpgradeSystem.Type.Ritual_Feast:
        this.CurrentRitual = (Ritual) this.gameObject.AddComponent<RitualFeast>();
        this.CurrentRitual.Play();
        break;
      case UpgradeSystem.Type.Ritual_HarvestRitual:
        this.CurrentRitual = (Ritual) this.gameObject.AddComponent<RitualHarvest>();
        this.CurrentRitual.Play();
        break;
      case UpgradeSystem.Type.Ritual_FishingRitual:
        this.CurrentRitual = (Ritual) this.gameObject.AddComponent<RitualFishing>();
        this.CurrentRitual.Play();
        break;
      case UpgradeSystem.Type.Ritual_Ressurect:
        RitualRessurect ritualRessurect = this.gameObject.AddComponent<RitualRessurect>();
        ritualRessurect.Play();
        ritualRessurect.RitualLight = this.RitualLighting;
        this.CurrentRitual = (Ritual) ritualRessurect;
        break;
      case UpgradeSystem.Type.Ritual_Funeral:
        RitualFuneral ritualFuneral = this.gameObject.AddComponent<RitualFuneral>();
        ritualFuneral.Play();
        this.CurrentRitual = (Ritual) ritualFuneral;
        break;
      case UpgradeSystem.Type.Ritual_Fightpit:
        RitualFightpit ritualFightpit = this.gameObject.AddComponent<RitualFightpit>();
        ritualFightpit.Play();
        this.CurrentRitual = (Ritual) ritualFightpit;
        break;
      case UpgradeSystem.Type.Ritual_Wedding:
        RitualWedding ritualWedding = this.gameObject.AddComponent<RitualWedding>();
        ritualWedding.Play();
        this.CurrentRitual = (Ritual) ritualWedding;
        break;
      case UpgradeSystem.Type.Ritual_AssignFaithEnforcer:
        RitualFaithEnforcer ritualFaithEnforcer = this.gameObject.AddComponent<RitualFaithEnforcer>();
        ritualFaithEnforcer.Play();
        this.CurrentRitual = (Ritual) ritualFaithEnforcer;
        break;
      case UpgradeSystem.Type.Ritual_AssignTaxCollector:
        RitualTaxEnforcer ritualTaxEnforcer = this.gameObject.AddComponent<RitualTaxEnforcer>();
        ritualTaxEnforcer.Play();
        this.CurrentRitual = (Ritual) ritualTaxEnforcer;
        break;
      case UpgradeSystem.Type.Ritual_Brainwashing:
        RitualBrainwashing ritualBrainwashing = this.gameObject.AddComponent<RitualBrainwashing>();
        ritualBrainwashing.Play();
        this.CurrentRitual = (Ritual) ritualBrainwashing;
        break;
      case UpgradeSystem.Type.Ritual_Ascend:
        RitualAscend ritualAscend = this.gameObject.AddComponent<RitualAscend>();
        ritualAscend.Play();
        this.CurrentRitual = (Ritual) ritualAscend;
        break;
      case UpgradeSystem.Type.Ritual_FirePit:
        this.CurrentRitual = (Ritual) this.gameObject.AddComponent<RitualFirePit>();
        this.CurrentRitual.Play();
        break;
      case UpgradeSystem.Type.Ritual_Halloween:
        RitualHalloween ritualHalloween = this.gameObject.AddComponent<RitualHalloween>();
        ritualHalloween.Play();
        this.CurrentRitual = (Ritual) ritualHalloween;
        break;
      case UpgradeSystem.Type.Ritual_BecomeDisciple:
        this.CurrentRitual = (Ritual) this.gameObject.AddComponent<RitualBecomeDisciple>();
        this.CurrentRitual.Play();
        break;
      case UpgradeSystem.Type.Ritual_Purge:
        this.CurrentRitual = (Ritual) this.gameObject.AddComponent<RitualPurge>();
        this.CurrentRitual.Play();
        break;
      case UpgradeSystem.Type.Ritual_Nudism:
        this.CurrentRitual = (Ritual) this.gameObject.AddComponent<RitualNudism>();
        this.CurrentRitual.Play();
        break;
      case UpgradeSystem.Type.Ritual_Cannibal:
        this.CurrentRitual = (Ritual) this.gameObject.AddComponent<RitualCannibal>();
        this.CurrentRitual.Play();
        break;
      case UpgradeSystem.Type.Ritual_AtoneSin:
        this.CurrentRitual = (Ritual) this.gameObject.AddComponent<RitualAtone>();
        this.CurrentRitual.Play();
        break;
      case UpgradeSystem.Type.Ritual_Snowman:
        this.CurrentRitual = (Ritual) this.gameObject.AddComponent<RitualSnowman>();
        this.CurrentRitual.Play();
        break;
      case UpgradeSystem.Type.Ritual_Midwinter:
        this.CurrentRitual = (Ritual) this.gameObject.AddComponent<RitualMidwinter>();
        this.CurrentRitual.Play();
        break;
      case UpgradeSystem.Type.Ritual_Warmth:
        this.CurrentRitual = (Ritual) this.gameObject.AddComponent<RitualWarmth>();
        this.CurrentRitual.Play();
        break;
      case UpgradeSystem.Type.Ritual_FollowerWedding:
        this.CurrentRitual = (Ritual) this.gameObject.AddComponent<RitualFollowerWedding>();
        this.CurrentRitual.Play();
        break;
      case UpgradeSystem.Type.Ritual_Divorce:
        this.CurrentRitual = (Ritual) this.gameObject.AddComponent<RitualDivorce>();
        this.CurrentRitual.Play();
        break;
      case UpgradeSystem.Type.Ritual_RanchMeat:
        this.CurrentRitual = (Ritual) this.gameObject.AddComponent<Ritual_RanchMeat>();
        this.CurrentRitual.Play();
        break;
      case UpgradeSystem.Type.Ritual_RanchHarvest:
        this.CurrentRitual = (Ritual) this.gameObject.AddComponent<Ritual_RanchHarvest>();
        this.CurrentRitual.Play();
        break;
      case UpgradeSystem.Type.Ritual_ConvertToRot:
        this.CurrentRitual = (Ritual) this.gameObject.AddComponent<RitualConvertToRot>();
        this.CurrentRitual.Play();
        break;
    }
  }

  public IEnumerator DelayCallback(float delay, System.Action callback)
  {
    yield return (object) new WaitForSeconds(delay);
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void UnlockHeartsCallback(bool cancelled)
  {
    Ritual.OnEnd -= new System.Action<bool>(this.UnlockHeartsCallback);
    this.StartCoroutine((IEnumerator) this.UnlockHeartsCallbackRoutine());
  }

  public IEnumerator UnlockHeartsCallbackRoutine()
  {
    yield return (object) new WaitForSeconds(5f);
    GameManager.GetInstance().StartCoroutine((IEnumerator) UpgradeSystem.ListOfUnlocksRoutine());
  }

  public void RitualOnEnd(bool cancelled)
  {
    if (!cancelled && this.forceRitual == UpgradeSystem.Type.Count && this.RitualType != UpgradeSystem.Type.Ritual_HeartsOfTheFaithful1)
    {
      foreach (StructuresData.ItemCost itemCost in UpgradeSystem.GetCost(this.RitualType))
        Inventory.ChangeItemQuantity((int) itemCost.CostItem, -itemCost.CostValue);
    }
    this.forceRitual = UpgradeSystem.Type.Count;
    this.ResetSprite();
    Ritual.OnEnd -= new System.Action<bool>(this.RitualOnEnd);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.CurrentRitual);
    this.Activating = false;
    foreach (PlayerFarming player in PlayerFarming.players)
      player.Spine.UseDeltaTime = true;
    if (!cancelled)
    {
      AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("FIRST_RITUAL"));
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.PerformAnyRitual);
      if (!DataManager.Instance.ShowLoyaltyBars)
        GameManager.GetInstance().StartCoroutine((IEnumerator) this.WaitForConversationToEnd((System.Action) (() =>
        {
          Onboarding.Instance.RatLoyalty.SetActive(true);
          Onboarding.Instance.RatLoyalty.GetComponent<Interaction_SimpleConversation>().Play();
        })));
      switch (this.RitualType)
      {
        case UpgradeSystem.Type.Ritual_Sacrifice:
          this.StartCoroutine((IEnumerator) this.UnlockSacrifices());
          UpgradeSystem.AddCooldown(this.RitualType, 8400f);
          break;
        case UpgradeSystem.Type.Ritual_ConsumeFollower:
          UpgradeSystem.AddCooldown(this.RitualType, 1200f);
          break;
        case UpgradeSystem.Type.Ritual_UnlockWeapon:
          UpgradeSystem.AddCooldown(this.RitualType, 1200f);
          break;
        case UpgradeSystem.Type.Ritual_UnlockCurse:
          UpgradeSystem.AddCooldown(this.RitualType, 1200f);
          break;
        case UpgradeSystem.Type.Ritual_FasterBuilding:
          UpgradeSystem.AddCooldown(this.RitualType, 6000f);
          break;
        case UpgradeSystem.Type.Ritual_Enlightenment:
          UpgradeSystem.AddCooldown(this.RitualType, 6000f);
          break;
        case UpgradeSystem.Type.Ritual_WorkThroughNight:
          UpgradeSystem.AddCooldown(this.RitualType, 6000f);
          UpgradeSystem.AddCooldown(UpgradeSystem.Type.Ritual_Holiday, 3600f);
          if (DataManager.Instance.PleasureEnabled)
          {
            UpgradeSystem.AddCooldown(UpgradeSystem.Type.Ritual_Nudism, 3600f);
            break;
          }
          break;
        case UpgradeSystem.Type.Ritual_Holiday:
          UpgradeSystem.AddCooldown(this.RitualType, 6000f);
          UpgradeSystem.AddCooldown(UpgradeSystem.Type.Ritual_WorkThroughNight, 1200f);
          break;
        case UpgradeSystem.Type.Ritual_AlmsToPoor:
          UpgradeSystem.AddCooldown(this.RitualType, 6000f);
          break;
        case UpgradeSystem.Type.Ritual_DonationRitual:
          UpgradeSystem.AddCooldown(this.RitualType, 6000f);
          break;
        case UpgradeSystem.Type.Ritual_Fast:
          UpgradeSystem.AddCooldown(this.RitualType, 12000f);
          UpgradeSystem.AddCooldown(UpgradeSystem.Type.Ritual_Feast, 6000f);
          break;
        case UpgradeSystem.Type.Ritual_Feast:
          UpgradeSystem.AddCooldown(this.RitualType, 6000f);
          break;
        case UpgradeSystem.Type.Ritual_HarvestRitual:
          UpgradeSystem.AddCooldown(this.RitualType, 6000f);
          break;
        case UpgradeSystem.Type.Ritual_FishingRitual:
          UpgradeSystem.AddCooldown(this.RitualType, 6000f);
          break;
        case UpgradeSystem.Type.Ritual_Ressurect:
          UpgradeSystem.AddCooldown(this.RitualType, 3600f);
          break;
        case UpgradeSystem.Type.Ritual_Funeral:
          UpgradeSystem.AddCooldown(this.RitualType, 1200f);
          break;
        case UpgradeSystem.Type.Ritual_Fightpit:
          UpgradeSystem.AddCooldown(this.RitualType, 6000f);
          break;
        case UpgradeSystem.Type.Ritual_Wedding:
          UpgradeSystem.AddCooldown(this.RitualType, 3600f);
          break;
        case UpgradeSystem.Type.Ritual_AssignFaithEnforcer:
          UpgradeSystem.AddCooldown(this.RitualType, 6000f);
          break;
        case UpgradeSystem.Type.Ritual_AssignTaxCollector:
          UpgradeSystem.AddCooldown(this.RitualType, 6000f);
          break;
        case UpgradeSystem.Type.Ritual_Brainwashing:
          UpgradeSystem.AddCooldown(this.RitualType, 8400f);
          break;
        case UpgradeSystem.Type.Ritual_Ascend:
          UpgradeSystem.AddCooldown(this.RitualType, 6000f);
          break;
        case UpgradeSystem.Type.Ritual_FirePit:
          UpgradeSystem.AddCooldown(this.RitualType, 8400f);
          break;
        case UpgradeSystem.Type.Ritual_Halloween:
          UpgradeSystem.AddCooldown(UpgradeSystem.Type.Ritual_Halloween, 3600f);
          if (DataManager.Instance.PleasureEnabled)
          {
            UpgradeSystem.AddCooldown(UpgradeSystem.Type.Ritual_Nudism, 3600f);
            break;
          }
          break;
        case UpgradeSystem.Type.Ritual_BecomeDisciple:
          UpgradeSystem.AddCooldown(this.RitualType, 1200f);
          break;
        case UpgradeSystem.Type.Ritual_Purge:
          UpgradeSystem.AddCooldown(this.RitualType, 6000f);
          break;
        case UpgradeSystem.Type.Ritual_Nudism:
          UpgradeSystem.AddCooldown(this.RitualType, 6000f);
          UpgradeSystem.AddCooldown(UpgradeSystem.Type.Ritual_WorkThroughNight, 1200f);
          UpgradeSystem.AddCooldown(UpgradeSystem.Type.Ritual_Holiday, 1200f);
          UpgradeSystem.AddCooldown(UpgradeSystem.Type.Ritual_Purge, 1200f);
          UpgradeSystem.AddCooldown(UpgradeSystem.Type.Ritual_Halloween, 1200f);
          break;
        case UpgradeSystem.Type.Ritual_Cannibal:
          UpgradeSystem.AddCooldown(this.RitualType, 4800f);
          break;
        case UpgradeSystem.Type.Ritual_AtoneSin:
          UpgradeSystem.AddCooldown(this.RitualType, 4800f);
          break;
        case UpgradeSystem.Type.Ritual_Snowman:
          UpgradeSystem.AddCooldown(this.RitualType, 6000f);
          break;
        case UpgradeSystem.Type.Ritual_Midwinter:
        case UpgradeSystem.Type.Ritual_Warmth:
        case UpgradeSystem.Type.Ritual_RanchMeat:
        case UpgradeSystem.Type.Ritual_RanchHarvest:
          UpgradeSystem.AddCooldown(this.RitualType, 7200f);
          break;
        case UpgradeSystem.Type.Ritual_FollowerWedding:
          UpgradeSystem.AddCooldown(this.RitualType, 2400f);
          break;
        case UpgradeSystem.Type.Ritual_ConvertToRot:
          UpgradeSystem.AddCooldown(this.RitualType, 1200f);
          break;
      }
    }
    if (this.playerLeftDuringRitual)
    {
      this.StartCoroutine((IEnumerator) this.DelayRemoveCoopDuringRitual());
      this.playerLeftDuringRitual = false;
    }
    else
      CoopManager.Instance.temporaryDisableRemoval = false;
  }

  public IEnumerator DelayRemoveCoopDuringRitual()
  {
    yield return (object) new WaitForSeconds(1f);
    while (PlayerFarming.AnyPlayerGotoAndStopping())
      yield return (object) null;
    CoopManager.RemovePlayerFromMenu();
    CoopManager.Instance.temporaryDisableRemoval = false;
    this.Close();
  }

  public IEnumerator WaitForConversationToEnd(System.Action callback)
  {
    while (LetterBox.IsPlaying)
      yield return (object) null;
    yield return (object) new WaitForSeconds(0.5f);
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void Close()
  {
    this.ResetSprite();
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    GameManager.GetInstance().OnConversationEnd();
    this.Activated = false;
    PlayerFarming.SetStateForAllPlayers();
  }

  public void GetAbilityRoutine(UpgradeSystem.Type Type)
  {
    this.StartCoroutine((IEnumerator) this.GetAbilityRoutineIE(Type));
  }

  public IEnumerator UnlockSacrifices()
  {
    yield return (object) new WaitForSeconds(1f);
    if (DataManager.Instance.sacrificesCompleted == 0)
      AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("FIRST_SACRIFICE"));
    ++DataManager.Instance.sacrificesCompleted;
    yield return (object) new WaitForSeconds(1f);
    if (DataManager.Instance.sacrificesCompleted >= 10)
      AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("SACRIFICE_FOLLOWERS"));
    yield return (object) null;
  }

  public IEnumerator GetAbilityRoutineIE(UpgradeSystem.Type Type)
  {
    Interaction_TempleAltar interactionTempleAltar = this;
    yield return (object) new WaitForEndOfFrame();
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionTempleAltar.playerFarming.CameraBone, 6f);
    interactionTempleAltar.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    interactionTempleAltar.playerFarming.Spine.AnimationState.SetAnimation(0, "gameover-fast", true);
    AudioManager.Instance.PlayOneShot("event:/player/receive_animation_start", interactionTempleAltar.playerFarming.gameObject);
    EventInstance receiveLoop = AudioManager.Instance.CreateLoop("event:/player/receive_animation_loop", interactionTempleAltar.playerFarming.gameObject, true);
    float Progress = 0.0f;
    float Duration = 3.66666675f;
    float StartingZoom = GameManager.GetInstance().CamFollowTarget.distance;
    while ((double) (Progress += Time.deltaTime) < (double) Duration - 0.5)
    {
      GameManager.GetInstance().CameraSetZoom(Mathf.Lerp(StartingZoom, 4f, Progress / Duration));
      if (Time.frameCount % 10 == 0)
        SoulCustomTarget.Create(interactionTempleAltar.playerFarming.CrownBone.gameObject, interactionTempleAltar.transform.position, Color.red, (System.Action) null, 0.2f);
      yield return (object) null;
    }
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationNext(interactionTempleAltar.playerFarming.CameraBone, 4f);
    interactionTempleAltar.playerFarming.Spine.AnimationState.SetAnimation(0, "specials/special-activate", false);
    yield return (object) new WaitForSeconds(1f);
    AudioManager.Instance.PlayOneShot("event:/player/receive_animation_end", interactionTempleAltar.playerFarming.gameObject);
    int num = (int) receiveLoop.stop(STOP_MODE.ALLOWFADEOUT);
    interactionTempleAltar.playerFarming.Spine.AnimationState.SetAnimation(0, "idle", true);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    UITutorialOverlayController TutorialOverlay = (UITutorialOverlayController) null;
    switch (Type)
    {
      case UpgradeSystem.Type.Ability_Eat:
        if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.TheHunger))
        {
          TutorialOverlay = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.TheHunger);
          break;
        }
        break;
      case UpgradeSystem.Type.Ability_Resurrection:
        if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Resurrection))
        {
          TutorialOverlay = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Resurrection);
          break;
        }
        break;
      case UpgradeSystem.Type.Ability_BlackHeart:
        if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.DarknessWithin))
        {
          TutorialOverlay = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.DarknessWithin);
          break;
        }
        break;
      case UpgradeSystem.Type.Ability_TeleportHome:
        if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Omnipresence))
        {
          TutorialOverlay = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Omnipresence);
          break;
        }
        break;
      case UpgradeSystem.Type.Ability_WinterChoice:
        ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.WinterChoice);
        if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.WinterAbility))
        {
          TutorialOverlay = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.WinterAbility);
          break;
        }
        break;
    }
    while ((UnityEngine.Object) TutorialOverlay != (UnityEngine.Object) null)
      yield return (object) null;
    yield return (object) new WaitForSeconds(0.5f);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.MonsterHeart);
    interactionTempleAltar.ResetSprite();
    interactionTempleAltar.OnInteract(interactionTempleAltar.state);
  }

  public void GetFleeceRoutine(int oldFleece, int newFleece)
  {
    this.StartCoroutine((IEnumerator) this.GetFleeceRoutineIE(oldFleece, newFleece));
  }

  public IEnumerator GetFleeceRoutineIE(int oldFleece, int newFleece)
  {
    Interaction_TempleAltar interactionTempleAltar = this;
    PlayerFarming playerFarming = PlayerFarming.players[0];
    interactionTempleAltar.temporaryPlayerFarming = playerFarming;
    interactionTempleAltar.fleece = newFleece;
    playerFarming.simpleSpineAnimator?.SetSkin("Lamb_" + oldFleece.ToString());
    yield return (object) new WaitForEndOfFrame();
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(playerFarming.CameraBone, 6f);
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    playerFarming.Spine.AnimationState.SetAnimation(0, "unlock-fleece", false);
    playerFarming.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/player/receive_animation_start", playerFarming.gameObject);
    EventInstance receiveLoop = AudioManager.Instance.CreateLoop("event:/player/receive_animation_loop", playerFarming.gameObject, true);
    playerFarming.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(interactionTempleAltar.AnimationState_Event);
    yield return (object) new WaitForSeconds(3f);
    AudioManager.Instance.PlayOneShot("event:/player/receive_animation_end", playerFarming.gameObject);
    int num = (int) receiveLoop.stop(STOP_MODE.ALLOWFADEOUT);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.UnlockFleece);
    interactionTempleAltar.ResetSprite();
    interactionTempleAltar.OnInteract(interactionTempleAltar.state);
  }

  public void AnimationState_Event(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == "change-skin"))
      return;
    PlayerFarming playerFarming = this.playerFarming;
    if ((bool) (UnityEngine.Object) this.temporaryPlayerFarming)
    {
      playerFarming = this.temporaryPlayerFarming;
      this.temporaryPlayerFarming = (PlayerFarming) null;
    }
    playerFarming.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.AnimationState_Event);
    playerFarming.simpleSpineAnimator?.SetSkin("Lamb_" + this.fleece.ToString());
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    AudioManager.Instance.PlayOneShot("event:/hearts_of_the_faithful/hearts_appear", playerFarming.gameObject);
    AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", playerFarming.gameObject.transform.position);
  }

  public bool FollowersReadyForLevelUp()
  {
    if (Ritual.FollowerToAttendSermon != null)
    {
      foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
      {
        if (followerBrain.CanLevelUp())
          return true;
      }
    }
    return false;
  }

  public IEnumerator BulkLevelFollowers()
  {
    Interaction_TempleAltar interactionTempleAltar = this;
    int num1 = interactionTempleAltar.previousWasSermon ? 1 : 0;
    GameObject TargetPosition = num1 != 0 ? ChurchFollowerManager.Instance.AltarPosition.gameObject : ChurchFollowerManager.Instance.RitualCenterPosition.gameObject;
    bool waiting = true;
    interactionTempleAltar.playerFarming.GoToAndStop(TargetPosition, GoToCallback: (System.Action) (() => waiting = false));
    Ritual ritual = interactionTempleAltar.gameObject.AddComponent<Ritual>();
    if (num1 == 0)
    {
      yield return (object) interactionTempleAltar.StartCoroutine((IEnumerator) ritual.WaitFollowersFormCircle(putOnHoods: false, zoom: 8f));
      foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
      {
        Follower followerById = FollowerManager.FindFollowerByID(followerBrain.Info.ID);
        if ((UnityEngine.Object) followerById != (UnityEngine.Object) null)
        {
          double num2 = (double) followerById.SetBodyAnimation("Worship/worship", true);
        }
      }
    }
    else
      GameManager.GetInstance().OnConversationNext(Interaction_TempleAltar.Instance.RitualCameraPosition);
    while (waiting)
      yield return (object) null;
    interactionTempleAltar.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) null;
    interactionTempleAltar.playerFarming.simpleSpineAnimator.Animate("sermons/sermon-start-nobook", 0, false);
    interactionTempleAltar.playerFarming.simpleSpineAnimator.AddAnimate("sermons/sermon-loop-nobook", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(0.25f);
    if (interactionTempleAltar._playerFarming.isLamb)
      AudioManager.Instance.PlayOneShot("event:/player/speak_to_follower_noBookPage");
    else
      AudioManager.Instance.PlayOneShot("event:/player/goat_player/goat_speak_to_follower_noBookPage");
    AudioManager.Instance.PlayOneShot("event:/Stings/generic_positive", interactionTempleAltar.transform.position);
    yield return (object) new WaitForSeconds(0.25f);
    bool showXPbar = false;
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (followerBrain.CanLevelUp())
      {
        showXPbar = true;
        break;
      }
    }
    if (showXPbar)
      HUD_Manager.Instance.XPBarTransitions.MoveBackInFunction();
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (followerBrain.CanLevelUp())
      {
        Follower followerById = FollowerManager.FindFollowerByID(followerBrain.Info.ID);
        if ((UnityEngine.Object) followerById != (UnityEngine.Object) null)
        {
          interactionTempleAltar.StartCoroutine((IEnumerator) followerById.GetComponent<interaction_FollowerInteraction>().LevelUpRoutineTemple(interactionTempleAltar.playerFarming));
          yield return (object) new WaitForSeconds(0.5f);
        }
      }
    }
    yield return (object) new WaitForSeconds(2f);
    if (showXPbar)
    {
      yield return (object) new WaitForSeconds(1f);
      HUD_Manager.Instance.XPBarTransitions.MoveBackOutFunction();
    }
    interactionTempleAltar.playerFarming.simpleSpineAnimator.Animate("sermons/sermon-stop-nobook", 0, false);
    interactionTempleAltar.playerFarming.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(1f);
    interactionTempleAltar.Activated = false;
    ritual.CompleteRitual(ignoreNightPenalty: true);
    UnityEngine.Object.Destroy((UnityEngine.Object) ritual);
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__58_0()
  {
    this.Collider.enabled = true;
    this.StartCoroutine((IEnumerator) this.DelayMenu());
    this.initialInteraction = false;
  }

  [CompilerGenerated]
  public void \u003CWaitForPlayersToGetIntoPosition\u003Eb__59_0()
  {
    this.playerFarming.transform.DOMove(ChurchFollowerManager.Instance.AltarPosition.transform.position, 0.1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
  }

  [CompilerGenerated]
  public void \u003CDelayMenu\u003Eb__60_0() => this.playerFarming.Spine.UseDeltaTime = true;

  [CompilerGenerated]
  public void \u003CDelayMenu\u003Eb__60_1() => this.DoSermon();

  [CompilerGenerated]
  public void \u003CDelayMenu\u003Eb__60_2() => this.DoRitual();

  [CompilerGenerated]
  public void \u003CDelayMenu\u003Eb__60_3() => this.DoPlayerUpgrade();

  [CompilerGenerated]
  public void \u003CDelayMenu\u003Eb__60_4() => this.DoDoctrine();

  [CompilerGenerated]
  public void \u003CDelayMenu\u003Eb__60_5() => this.DoCultUpgrade();

  [CompilerGenerated]
  public void \u003CDelayMenu\u003Eb__60_6() => this.DoCancel();

  [CompilerGenerated]
  public void \u003CDoDoctrine\u003Eb__63_0()
  {
    Time.timeScale = 1f;
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    this.OnInteract(this.state);
  }

  [CompilerGenerated]
  public void \u003CDoCancel\u003Eb__68_0()
  {
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
    MonoSingleton<UIManager>.Instance.ForceDisableSaving = false;
    PlayerFarming.Instance.PlayerDoctrineStone.Spine.unscaledTime = false;
    SimulationManager.UnPause();
    AudioManager.Instance.StopLoop(this.sermonLoop);
    AudioManager.Instance.PlayOneShot("event:/sermon/book_put_down", this.playerFarming.gameObject);
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    Time.timeScale = 1f;
    foreach (PlayerFarming player in PlayerFarming.players)
      player.Spine.UseDeltaTime = true;
    GameManager.GetInstance().CamFollowTarget.DisablePlayerLook = false;
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    GameManager.GetInstance().OnConversationEnd();
    CultFaithManager.Instance.BarController.UseQueuing = false;
    this.initialInteraction = true;
    if (Ritual.FollowerToAttendSermon != null)
    {
      foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
      {
        FollowerBrain f = followerBrain;
        if (f != null)
        {
          Follower followerById = FollowerManager.FindFollowerByID(f.Info.ID);
          if ((bool) (UnityEngine.Object) followerById)
          {
            FollowerManager.FindFollowerByID(f.Info.ID).Spine.UseDeltaTime = true;
            followerById.ShowAllFollowerIcons();
            followerById.Spine.UseDeltaTime = true;
            followerById.UseUnscaledTime = false;
            followerById.OverridingOutfit = false;
            if (this.performedSermon)
            {
              int num = 0;
              if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Temple_III))
                num = 1;
              if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Temple_IV))
                num = 2;
              for (int index = 0; index < num; ++index)
              {
                AudioManager.Instance.PlayOneShot("event:/followers/pop_in", followerById.transform.position);
                ResourceCustomTarget.Create(Interaction_DonationBox.Instance.gameObject, followerById.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) (() => Interaction_DonationBox.Instance.DepositCoin()));
              }
            }
          }
          f.CompleteCurrentTask();
          this.StartCoroutine((IEnumerator) this.DelayCallback(1f, (System.Action) (() =>
          {
            if (f.CurrentTaskType != FollowerTaskType.AttendTeaching)
              return;
            f.CompleteCurrentTask();
          })));
        }
      }
    }
    this.ResetSprite();
    ChurchFollowerManager.Instance.ClearAudienceBrains();
    this.StartCoroutine((IEnumerator) this.MidasStealDonationsIE((System.Action) (() =>
    {
      this.performedSermon = false;
      this.state.CURRENT_STATE = StateMachine.State.Idle;
      this.ResetSprite();
      foreach (PlayerFarming player in PlayerFarming.players)
      {
        PlayerFarming pl = player;
        if ((UnityEngine.Object) this.playerFarming != (UnityEngine.Object) pl)
          pl.CustomAnimationWithCallback("sermons/second-player-stop", false, (System.Action) (() =>
          {
            pl.state.LockStateChanges = false;
            pl.state.CURRENT_STATE = StateMachine.State.Idle;
          }));
      }
      if (Ritual.FollowerToAttendSermon != null && Ritual.FollowerToAttendSermon.Count > 0)
      {
        GameManager.GetInstance().WaitForSeconds(1.5f, (System.Action) (() =>
        {
          this.HasChanged = true;
          this.Activated = false;
        }));
      }
      else
      {
        this.HasChanged = true;
        this.Activated = false;
      }
      if (Ritual.FollowerToAttendSermon == null)
        return;
      Ritual.FollowerToAttendSermon.Clear();
    })));
  }

  [CompilerGenerated]
  public void \u003CDoCancel\u003Eb__68_1()
  {
    this.performedSermon = false;
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.ResetSprite();
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      PlayerFarming pl = player;
      if ((UnityEngine.Object) this.playerFarming != (UnityEngine.Object) pl)
        pl.CustomAnimationWithCallback("sermons/second-player-stop", false, (System.Action) (() =>
        {
          pl.state.LockStateChanges = false;
          pl.state.CURRENT_STATE = StateMachine.State.Idle;
        }));
    }
    if (Ritual.FollowerToAttendSermon != null && Ritual.FollowerToAttendSermon.Count > 0)
    {
      GameManager.GetInstance().WaitForSeconds(1.5f, (System.Action) (() =>
      {
        this.HasChanged = true;
        this.Activated = false;
      }));
    }
    else
    {
      this.HasChanged = true;
      this.Activated = false;
    }
    if (Ritual.FollowerToAttendSermon == null)
      return;
    Ritual.FollowerToAttendSermon.Clear();
  }

  [CompilerGenerated]
  public void \u003CDoCancel\u003Eb__68_4()
  {
    this.HasChanged = true;
    this.Activated = false;
  }

  [CompilerGenerated]
  public void \u003CGatherFollowers\u003Eb__90_1() => this.IncrementXPBar();

  [CompilerGenerated]
  public void \u003CGatherFollowers\u003Eb__90_0() => this.IncrementXPBar();

  [CompilerGenerated]
  public void \u003CAskQuestionRoutine\u003Eb__94_0() => this.Reply(true);

  [CompilerGenerated]
  public void \u003CAskQuestionRoutine\u003Eb__94_1() => this.Reply(false);

  [CompilerGenerated]
  public void \u003CAskQuestionRoutine\u003Eb__94_2() => this.Reply(true);

  [CompilerGenerated]
  public void \u003COpenRitualMenu\u003Eb__108_0()
  {
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    this.OnInteract(this.state);
  }

  [CompilerGenerated]
  public void \u003CPerformRitual\u003Eb__111_0()
  {
    (UnityEngine.Object.Instantiate(Resources.Load("Prefabs/UI/UI Unlock Curse"), GameObject.FindWithTag("Canvas").transform) as GameObject).GetComponent<UIUnlockCurse>().Init((System.Action) (() =>
    {
      this.CurrentRitual = (Ritual) this.gameObject.AddComponent<RitualUnlockCurse>();
      this.CurrentRitual.Play();
    }), (System.Action) (() => this.OpenRitualMenu()));
  }

  [CompilerGenerated]
  public void \u003CPerformRitual\u003Eb__111_2()
  {
    this.CurrentRitual = (Ritual) this.gameObject.AddComponent<RitualUnlockCurse>();
    this.CurrentRitual.Play();
  }

  [CompilerGenerated]
  public void \u003CPerformRitual\u003Eb__111_3() => this.OpenRitualMenu();

  [CompilerGenerated]
  public void \u003CPerformRitual\u003Eb__111_1()
  {
    (UnityEngine.Object.Instantiate(Resources.Load("Prefabs/UI/UI Unlock Weapon"), GameObject.FindWithTag("Canvas").transform) as GameObject).GetComponent<UIUnlockWeapon>().Init((System.Action) (() =>
    {
      this.CurrentRitual = (Ritual) this.gameObject.AddComponent<RitualUnlockWeapon>();
      this.CurrentRitual.Play();
    }), (System.Action) (() => this.OpenRitualMenu()));
  }

  [CompilerGenerated]
  public void \u003CPerformRitual\u003Eb__111_4()
  {
    this.CurrentRitual = (Ritual) this.gameObject.AddComponent<RitualUnlockWeapon>();
    this.CurrentRitual.Play();
  }

  [CompilerGenerated]
  public void \u003CPerformRitual\u003Eb__111_5() => this.OpenRitualMenu();
}
