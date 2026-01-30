// Decompiled with JetBrains decompiler
// Type: WoolhavenYngyaStatue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using I2.Loc;
using Lamb.UI;
using MMTools;
using Spine;
using Spine.Unity;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class WoolhavenYngyaStatue : BaseMonoBehaviour
{
  public static WoolhavenYngyaStatue Instance;
  [SerializeField]
  public ParticleSystem bellBreakVFX;
  [SerializeField]
  public ParticleSystem rotBreakVFX;
  [SerializeField]
  public SkeletonAnimation skeleton;
  [SerializeField]
  public GameObject ghostNPCCircleContainer;
  [SerializeField]
  public SpriteRenderer yngyaStatueSpriteFlowers;
  [SerializeField]
  public GameObject gravesParent;
  [SerializeField]
  public GameObject plaza;
  [SerializeField]
  public GameObject plazaVolumetrics;
  [SerializeField]
  public float activationDistance;
  [SerializeField]
  public Interaction_PurchasableFleece[] fleeces;
  [SerializeField]
  public Interaction_PurchasableFleece ranchingGrave;
  [SerializeField]
  public GameObject lostSouls;
  [CompilerGenerated]
  public bool \u003CUnlockingChain\u003Ek__BackingField;
  public Coroutine onboardRoutine;
  public List<Interaction_PurchasableFleece> _purchasableFleeces = new List<Interaction_PurchasableFleece>();
  public EventInstance ambienceLoopInstance;
  public string[] ambienceLoopLevels = new string[6]
  {
    "event:/dlc/music/yngya_shrine/woolhaven_level_0_loop",
    "event:/dlc/music/yngya_shrine/woolhaven_level_1_loop",
    "event:/dlc/music/yngya_shrine/woolhaven_level_3_loop",
    "event:/dlc/music/yngya_shrine/woolhaven_level_4_loop",
    "event:/dlc/music/yngya_shrine/woolhaven_level_5_loop",
    "event:/dlc/music/yngya_shrine/woolhaven_level_6_loop"
  };
  public string[] shrinePowerUpStingers = new string[6]
  {
    "event:/dlc/music/yngya_shrine/woolhaven_powerup_start_level_0",
    "event:/dlc/music/yngya_shrine/woolhaven_powerup_start_level_1",
    "event:/dlc/music/yngya_shrine/woolhaven_powerup_start_level_2",
    "event:/dlc/music/yngya_shrine/woolhaven_powerup_start_level_3",
    "event:/dlc/music/yngya_shrine/woolhaven_powerup_start_level_4",
    "event:/dlc/music/yngya_shrine/woolhaven_powerup_start_level_5"
  };
  [SerializeField]
  public SimpleSetCamera IntroSetCamera;

  public SkeletonAnimation Skeleton => this.skeleton;

  public bool UnlockingChain
  {
    get => this.\u003CUnlockingChain\u003Ek__BackingField;
    set => this.\u003CUnlockingChain\u003Ek__BackingField = value;
  }

  public bool canAwakeYngya
  {
    get
    {
      return SeasonsManager.Active && !DataManager.Instance.OnboardedYngyaAwoken && DataManager.Instance.OnboardedLambTown;
    }
  }

  public int bellBreakCount => DataManager.GetCompletedJobBoardCount();

  public void Awake()
  {
    WoolhavenYngyaStatue.Instance = this;
    this._purchasableFleeces = ((IEnumerable<Interaction_PurchasableFleece>) this.gravesParent.GetComponentsInChildren<Interaction_PurchasableFleece>()).ToList<Interaction_PurchasableFleece>();
  }

  public void OnEnable()
  {
    this.skeleton?.ClearState();
    this.plaza.SetActive(false);
    this.plazaVolumetrics.SetActive(false);
    this.lostSouls.SetActive(false);
    WoolhavenYngyaStatue.YngyaRotSkin yngyaSkin = WoolhavenYngyaStatue.ExpectedYngyaRotSkinIndex();
    WoolhavenYngyaStatue.YngyaBellSkin bellSkin = this.ExpectedYngyaBellSkinIndex();
    this.SetYngyaSkin(yngyaSkin, bellSkin);
    this.UpdateAmbientLoop(yngyaSkin);
  }

  public void OnDisable() => AudioManager.Instance.StopLoop(this.ambienceLoopInstance);

  public WoolhavenYngyaStatue.YngyaBellSkin ExpectedYngyaBellSkinIndex()
  {
    WoolhavenYngyaStatue.YngyaBellSkin yngyaBellSkin = WoolhavenYngyaStatue.YngyaBellSkin.NoRot;
    if (this.bellBreakCount > 0)
      yngyaBellSkin = (WoolhavenYngyaStatue.YngyaBellSkin) (this.bellBreakCount + 1);
    else if (DataManager.Instance.OnboardedYngyaAwoken)
      yngyaBellSkin = WoolhavenYngyaStatue.YngyaBellSkin.Rot1;
    return yngyaBellSkin;
  }

  public static WoolhavenYngyaStatue.YngyaRotSkin ExpectedYngyaRotSkinIndex()
  {
    WoolhavenYngyaStatue.YngyaRotSkin yngyaRotSkin = WoolhavenYngyaStatue.YngyaRotSkin.NoRot;
    if (DataManager.Instance.BeatenYngya)
      yngyaRotSkin = WoolhavenYngyaStatue.YngyaRotSkin.Flowers;
    else if (DataManager.Instance.WinterServerity > 0)
      yngyaRotSkin = (WoolhavenYngyaStatue.YngyaRotSkin) Mathf.Clamp(DataManager.Instance.WinterServerity, 0, 4);
    else if (DataManager.Instance.OnboardedYngyaAwoken)
      yngyaRotSkin = WoolhavenYngyaStatue.YngyaRotSkin.SlightRot;
    return yngyaRotSkin;
  }

  public static WoolhavenYngyaStatue.YngyaRotSkin GetNextYngyaRotSkinIndex()
  {
    WoolhavenYngyaStatue.YngyaRotSkin yngyaRotSkinIndex = WoolhavenYngyaStatue.YngyaRotSkin.NoRot;
    int num = SeasonsManager.WinterSeverity + 1;
    if (DataManager.Instance.BeatenYngya)
      yngyaRotSkinIndex = WoolhavenYngyaStatue.YngyaRotSkin.Flowers;
    else if (num > 0)
      yngyaRotSkinIndex = (WoolhavenYngyaStatue.YngyaRotSkin) Mathf.Clamp(num, 0, 4);
    else if (DataManager.Instance.OnboardedYngyaAwoken)
      yngyaRotSkinIndex = WoolhavenYngyaStatue.YngyaRotSkin.SlightRot;
    return yngyaRotSkinIndex;
  }

  public void Update()
  {
    if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
      this.TryToPlayFinalSequence();
    if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null) || PlayerFarming.Instance.GoToAndStopping || this.onboardRoutine != null)
      return;
    this.TryToPlayOnboardingSequence();
  }

  public void SetYngyaSkin(
    WoolhavenYngyaStatue.YngyaRotSkin yngyaSkin,
    WoolhavenYngyaStatue.YngyaBellSkin bellSkin)
  {
    Skin newSkin = new Skin("Rot & bell");
    SkeletonData data = this.skeleton.skeleton.Data;
    newSkin.AddSkin(data.FindSkin($"Rot/{(int) yngyaSkin}"));
    newSkin.AddSkin(data.FindSkin($"Bell/{(int) bellSkin}"));
    this.skeleton.skeleton.SetSkin(newSkin);
    this.skeleton.skeleton.SetSlotsToSetupPose();
    this.skeleton.AnimationState.SetAnimation(0, "animation", true);
  }

  public void UpdateAmbientLoop(WoolhavenYngyaStatue.YngyaRotSkin yngyaSkin)
  {
    AudioManager.Instance.StopLoop(this.ambienceLoopInstance);
    if (yngyaSkin < WoolhavenYngyaStatue.YngyaRotSkin.NoRot || yngyaSkin >= (WoolhavenYngyaStatue.YngyaRotSkin) this.ambienceLoopLevels.Length)
      return;
    string soundPath = this.ambienceLoopLevels[(int) yngyaSkin];
    if (yngyaSkin == WoolhavenYngyaStatue.YngyaRotSkin.Flowers)
      soundPath = "event:/dlc/music/yngya_shrine/woolhaven_level_6_loop";
    if (string.IsNullOrEmpty(soundPath))
      return;
    this.ambienceLoopInstance = AudioManager.Instance.CreateLoop(soundPath, this.gameObject, true);
  }

  public static IEnumerator FadeIn()
  {
    bool waitingForFade = true;
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 1f, "", (System.Action) (() => waitingForFade = false));
    while (waitingForFade)
      yield return (object) null;
  }

  public static IEnumerator FadeOut()
  {
    bool waitingForFade = true;
    MMTransition.ResumePlay((System.Action) (() => waitingForFade = false));
    while (waitingForFade)
      yield return (object) null;
  }

  public void TryToPlayOnboardingSequence()
  {
    if (!DataManager.Instance.OnboardedLambTown)
    {
      this.PlayWoolhavenIntro();
    }
    else
    {
      foreach (PlayerFarming player in PlayerFarming.players)
      {
        if (!((UnityEngine.Object) player == (UnityEngine.Object) null) && (double) Vector2.Distance((Vector2) player.transform.position, (Vector2) this.transform.position) <= (double) this.activationDistance)
        {
          if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.SPECIAL_WOOL_RANCHER) <= 0 || DataManager.Instance.OnboardedLambGhostNPCs)
            break;
          this.PlayOnboardLambGhostsNPC();
          break;
        }
      }
    }
  }

  public void PlayUnlockChain() => this.StartCoroutine((IEnumerator) this.UnlockChainSequence());

  public IEnumerator UnlockChainSequence()
  {
    WoolhavenYngyaStatue woolhavenYngyaStatue = this;
    woolhavenYngyaStatue.UnlockingChain = true;
    try
    {
      GameManager gameManager = GameManager.GetInstance();
      bool breakBell = woolhavenYngyaStatue.bellBreakCount == 6;
      yield return (object) WoolhavenYngyaStatue.FadeIn();
      Vector3 vfxPos = woolhavenYngyaStatue.transform.position with
      {
        z = 0.0f
      };
      Vector3 itemSpawnPos = vfxPos;
      itemSpawnPos.y -= 0.5f;
      Vector3 vector3 = vfxPos;
      vector3.y -= 2f;
      if (breakBell)
        PlayerFarming.Instance.transform.position = vector3;
      gameManager.OnConversationNew();
      gameManager.OnConversationNext(woolhavenYngyaStatue.gameObject);
      gameManager.CameraSnapToPosition(woolhavenYngyaStatue.transform.position);
      gameManager.CameraSetOffset(new Vector3(0.0f, 0.0f, -2f));
      yield return (object) woolhavenYngyaStatue.StartCoroutine((IEnumerator) WoolhavenYngyaStatue.FadeOut());
      AudioManager.Instance.PlayOneShot("event:/door/chain_break_sequence", woolhavenYngyaStatue.gameObject);
      GameManager.GetInstance().CameraZoom(8f, 1.5f);
      DOVirtual.DelayedCall(0.5f, (TweenCallback) (() => CameraManager.instance.ShakeCameraForDuration(0.1f, 0.5f, 1.5f)));
      yield return (object) new WaitForSeconds(1.5f);
      PickUp item = (PickUp) null;
      if (breakBell)
      {
        woolhavenYngyaStatue.skeleton.skeleton.SetSlotsToSetupPose();
        woolhavenYngyaStatue.SetYngyaSkin(WoolhavenYngyaStatue.ExpectedYngyaRotSkinIndex(), WoolhavenYngyaStatue.YngyaBellSkin.Broken);
        woolhavenYngyaStatue.UpdateAmbientLoop(WoolhavenYngyaStatue.ExpectedYngyaRotSkinIndex());
        woolhavenYngyaStatue.skeleton.AnimationState.SetAnimation(0, "break-bell", false);
        AudioManager.Instance.PlayOneShot("event:/dlc/dungeon06/trap/flesh_rock/block_petrified_destroy", woolhavenYngyaStatue.gameObject);
        item = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.LEGENDARY_WEAPON_FRAGMENT, 1, itemSpawnPos);
      }
      else
      {
        woolhavenYngyaStatue.SetYngyaSkin(WoolhavenYngyaStatue.ExpectedYngyaRotSkinIndex(), (WoolhavenYngyaStatue.YngyaBellSkin) (woolhavenYngyaStatue.bellBreakCount + 1));
        woolhavenYngyaStatue.UpdateAmbientLoop(WoolhavenYngyaStatue.ExpectedYngyaRotSkinIndex());
        woolhavenYngyaStatue.skeleton.AnimationState.SetAnimation(0, "next-bell", false);
        AudioManager.Instance.PlayOneShot("event:/Stings/DLC_IntroBell", woolhavenYngyaStatue.gameObject);
      }
      woolhavenYngyaStatue.skeleton.AnimationState.AddAnimation(0, "animation", true, 0.0f);
      CameraManager.shakeCamera(20f, 0.5f);
      BiomeConstants.Instance.EmitDisplacementEffect(vfxPos);
      woolhavenYngyaStatue.bellBreakVFX.Play();
      gameManager.CameraZoom(11f, 0.1f);
      if (breakBell)
      {
        yield return (object) new WaitForSeconds(1.5f);
        AudioManager.Instance.PlayOneShot("event:/Stings/DLC_IntroBell", woolhavenYngyaStatue.gameObject);
        AudioManager.Instance.PlayOneShot("event:/Stings/generic_positive", woolhavenYngyaStatue.gameObject);
        yield return (object) new WaitForSeconds(2f);
        woolhavenYngyaStatue.UnlockWoolhavenAchievementIfComplete();
      }
      else
      {
        yield return (object) new WaitForSeconds(2.5f);
        yield return (object) WoolhavenYngyaStatue.FadeIn();
        GameManager.GetInstance().CameraSetOffset(new Vector3(0.0f, 0.0f, 0.0f));
        GameManager.GetInstance().CameraSnapToPosition(PlayerFarming.Instance.transform.position);
        GameManager.GetInstance().OnConversationEnd();
        woolhavenYngyaStatue.UnlockingChain = false;
        yield return (object) WoolhavenYngyaStatue.FadeOut();
      }
      GameManager.GetInstance().CameraSetOffset(new Vector3(0.0f, 0.0f, 0.0f));
      if ((bool) (UnityEngine.Object) item)
      {
        Interaction_BrokenWeapon component = item.GetComponent<Interaction_BrokenWeapon>();
        component.SetWeapon(EquipmentType.Chain_Legendary);
        component.OnInteract(PlayerFarming.Instance.state);
      }
      gameManager = (GameManager) null;
      vfxPos = new Vector3();
      itemSpawnPos = new Vector3();
      item = (PickUp) null;
    }
    finally
    {
      this.UnlockingChain = false;
    }
  }

  public void ShowStatueEyes(bool active)
  {
    foreach (Interaction_PurchasableFleece purchasableFleece in this._purchasableFleeces)
    {
      purchasableFleece.SetColdEyes(active);
      purchasableFleece.SetEyes(active);
    }
  }

  public void PlayYngyaAwoken()
  {
    this.onboardRoutine = this.StartCoroutine((IEnumerator) this.YngyaAwokenIE());
  }

  public static void PlayYngyaAwokenGlobal(bool playWinterSeqeunce = true, bool playIntroConversation = true)
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) WoolhavenYngyaStatue.Instance.YngyaAwokenIE(playWinterSeqeunce, playIntroConversation));
  }

  public IEnumerator YngyaAwokenIE(bool playWinterSequence = true, bool playIntroConversation = true)
  {
    WoolhavenYngyaStatue woolhavenYngyaStatue1 = this;
    yield return (object) new WaitForEndOfFrame();
    Interaction_DLCYngyaShrine shrine = Interaction_DLCYngyaShrine.Instance;
    PlayerFarming playerFarming = PlayerFarming.Instance;
    SimpleSpineFlash spineFlash = woolhavenYngyaStatue1.skeleton.GetComponent<SimpleSpineFlash>();
    MonoSingleton<UIManager>.Instance.ForceBlockPause = true;
    int num1 = (int) AudioManager.Instance.CurrentMusicInstance.setPaused(true);
    TimeManager.PauseGameTime = true;
    woolhavenYngyaStatue1.ShowStatueEyes(false);
    WeatherSystemController.Instance.StopCurrentWeather(1f);
    shrine.awoken.gameObject.SetActive(false);
    GameManager gameManager = GameManager.GetInstance();
    gameManager.OnConversationNew();
    gameManager.OnConversationNext(woolhavenYngyaStatue1.gameObject);
    gameManager.CameraSetOffset(new Vector3(0.0f, 0.0f, -3f));
    if (DataManager.Instance.WinterServerity == 0)
      woolhavenYngyaStatue1.skeleton.skeleton.Skin = woolhavenYngyaStatue1.skeleton.skeleton.Data.FindSkin("Rot/0");
    EventInstance introInstance = AudioManager.Instance.PlayOneShotWithInstance("event:/music/intro/intro_bass");
    playerFarming.GoToAndStop(woolhavenYngyaStatue1.transform.position + new Vector3(0.0f, -0.5f, 0.0f), woolhavenYngyaStatue1.gameObject, DisableCollider: true);
    foreach (GameObject candle in shrine.candles)
      candle.gameObject.SetActive(false);
    CameraFollowTarget camTarget = gameManager.CamFollowTarget;
    BiomeConstants.Instance.DepthOfFieldTween(0.15f, 8.7f, 26f, 0.0f, 0.0f);
    gameManager.CameraZoom(6f, 6f);
    AudioManager.Instance.PlayOneShot("event:/dlc/env/yngya_shrine/woolhaven_powerup_candles_light");
    AudioManager.Instance.PlayOneShot("event:/dlc/env/yngya_shrine/woolhaven_powerup_start");
    woolhavenYngyaStatue1.PlayShrinePowerupStinger(0);
    yield return (object) new WaitForSeconds(1f);
    shrine.candles = ((IEnumerable<GameObject>) shrine.candles).OrderBy<GameObject, float>((Func<GameObject, float>) (x => Vector3.Distance(x.transform.position, new Vector3(0.0f, 28f, 0.0f)))).ToArray<GameObject>();
    GameObject[] gameObjectArray = shrine.candles;
    int index;
    for (index = 0; index < gameObjectArray.Length; ++index)
    {
      gameObjectArray[index].gameObject.SetActive(true);
      yield return (object) new WaitForSeconds(0.02f);
    }
    gameObjectArray = (GameObject[]) null;
    AudioManager.Instance.PlayOneShot("event:/door/chain_break_sequence", woolhavenYngyaStatue1.gameObject);
    DOVirtual.DelayedCall(0.5f, (TweenCallback) (() => CameraManager.instance.ShakeCameraForDuration(0.1f, 0.5f, 1.5f)));
    yield return (object) new WaitForSeconds(1.5f);
    spineFlash.Flash(Color.white, 0.2f);
    BiomeConstants.Instance.ImpactFrameForDuration();
    CameraManager.instance.ShakeCameraForDuration(1.5f, 2f, 0.1f);
    AudioManager.Instance.PlayOneShot("event:/rituals/blood_sacrifice", woolhavenYngyaStatue1.gameObject);
    AudioManager.Instance.PlayOneShot("event:/Stings/DLC_IntroBell", woolhavenYngyaStatue1.gameObject);
    AudioManager.Instance.PlayOneShot("event:/dlc/dungeon06/trap/flesh_rock/block_petrified_destroy", woolhavenYngyaStatue1.gameObject);
    AudioManager.Instance.StopOneShotInstanceEarly(introInstance, STOP_MODE.IMMEDIATE);
    playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    playerFarming.simpleSpineAnimator.Animate("collect-ghosts", 0, false);
    playerFarming.simpleSpineAnimator.AddAnimate("collect-ghosts-loop", 0, true, 0.0f);
    shrine.ritual.gameObject.SetActive(true);
    shrine.awoken.gameObject.SetActive(true);
    gameManager.CameraSetOffset(new Vector3(0.0f, 0.0f, -5f));
    if (DataManager.Instance.WinterServerity == 0)
      woolhavenYngyaStatue1.skeleton.skeleton.Skin = woolhavenYngyaStatue1.skeleton.skeleton.Data.FindSkin("Rot/1");
    BiomeConstants.Instance.SetIceOverlayReveal(1f);
    gameManager.CameraZoom(10f, 0.1f);
    BiomeConstants.Instance.EmitDisplacementEffect(shrine.transform.position);
    WeatherSystemController.Instance.SetWeather(WeatherSystemController.WeatherType.Snowing, WeatherSystemController.WeatherStrength.Extreme);
    woolhavenYngyaStatue1.skeleton.AnimationState.SetAnimation(0, "next-bell", false);
    woolhavenYngyaStatue1.skeleton.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    woolhavenYngyaStatue1.StartCoroutine((IEnumerator) woolhavenYngyaStatue1.IceOverlayRevealIE(0.0f, 1f, 3f));
    woolhavenYngyaStatue1.ShowStatueEyes(true);
    DataManager.Instance.OnboardedYngyaAwoken = true;
    woolhavenYngyaStatue1.plaza.gameObject.SetActive(true);
    woolhavenYngyaStatue1.plaza.transform.DOScale(1f, 1f).From(0.0f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutCubic);
    woolhavenYngyaStatue1.plazaVolumetrics.SetActive(true);
    SpriteRenderer[] plazaVolumetricRenderers = woolhavenYngyaStatue1.plazaVolumetrics.GetComponentsInChildren<SpriteRenderer>();
    foreach (SpriteRenderer target in plazaVolumetricRenderers)
    {
      float a = target.color.a;
      target.color = new Color(target.color.r, target.color.g, target.color.b, 0.0f);
      target.DOFade(a, 2f).SetDelay<TweenerCore<Color, Color, ColorOptions>>(0.0f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutCubic);
    }
    woolhavenYngyaStatue1.lostSouls.SetActive(true);
    for (index = 0; index < 4; ++index)
    {
      BiomeConstants.Instance.EmitDisplacementEffect(shrine.transform.position);
      AudioManager.Instance.PlayOneShot("event:/ui/heartbeat", woolhavenYngyaStatue1.gameObject);
      CameraManager.shakeCamera(3f, 0.0f);
      woolhavenYngyaStatue1.skeleton.AnimationState.SetAnimation(0, "next-bell", false);
      woolhavenYngyaStatue1.skeleton.AnimationState.AddAnimation(0, "animation", true, 0.0f);
      yield return (object) new WaitForSeconds(1.5f);
    }
    shrine.ritual.gameObject.SetActive(false);
    camTarget.enabled = true;
    playerFarming.simpleSpineAnimator.Animate("collect-ghosts-land", 0, false);
    playerFarming.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    WeatherSystemController.Instance.SetWeather(WeatherSystemController.WeatherType.Snowing, WeatherSystemController.WeatherStrength.Dusting, 0.0f);
    yield return (object) new WaitForEndOfFrame();
    int num2 = (int) AudioManager.Instance.CurrentMusicInstance.setPaused(false);
    if (playWinterSequence)
    {
      yield return (object) GameManager.GetInstance().StartCoroutine((IEnumerator) SeasonsManager.FirstWinterIE(SeasonsManager.Season.Winter, (System.Action) null));
      BaseGoopDoor.WoolhavenDoor.SetDoorUp();
    }
    woolhavenYngyaStatue1.StartCoroutine((IEnumerator) woolhavenYngyaStatue1.IceOverlayRevealIE(1f, 0.0f, 1f));
    woolhavenYngyaStatue1.ShowStatueEyes(false);
    Interaction_DLCYngyaShrine.Instance.XPBar.UpdateBar(0.0f);
    TimeManager.PauseGameTime = false;
    MonoSingleton<UIManager>.Instance.ForceBlockPause = false;
    foreach (SpriteRenderer spriteRenderer in plazaVolumetricRenderers)
    {
      WoolhavenYngyaStatue woolhavenYngyaStatue = woolhavenYngyaStatue1;
      SpriteRenderer child = spriteRenderer;
      float startAlpha = child.color.a;
      child.DOFade(0.0f, 1f).From(startAlpha).SetDelay<TweenerCore<Color, Color, ColorOptions>>(0.0f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutCubic).OnComplete<TweenerCore<Color, Color, ColorOptions>>((TweenCallback) (() =>
      {
        child.color = new Color(child.color.r, child.color.g, child.color.b, startAlpha);
        woolhavenYngyaStatue.plazaVolumetrics.SetActive(false);
      }));
    }
    woolhavenYngyaStatue1.plaza.transform.DOScale(0.0f, 1f).From(1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutCubic).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(woolhavenYngyaStatue1.\u003CYngyaAwokenIE\u003Eb__47_2));
    woolhavenYngyaStatue1.lostSouls.SetActive(false);
    yield return (object) new WaitForSeconds(0.5f);
    if (playIntroConversation)
    {
      shrine.introConvo.Spoken = false;
      shrine.introConvo.gameObject.SetActive(true);
      shrine.introConvo.Play();
      shrine.introConvo.Callback.AddListener((UnityAction) (() => ObjectiveManager.Add((ObjectivesData) new Objectives_CollectItem("Objectives/GroupTitles/WarmCult", InventoryItem.ITEM_TYPE.MAGMA_STONE, 5, targetLocation: FollowerLocation.Dungeon1_5), true, true)));
    }
    else
      gameManager.OnConversationEnd();
    gameManager.CameraSetOffset(Vector3.zero);
    WeatherSystemController.Instance.StopCurrentWeather(1f);
    WeatherSystemController.Instance.SetWeather(WeatherSystemController.WeatherType.Snowing, WeatherSystemController.WeatherStrength.Heavy);
  }

  public IEnumerator IceOverlayRevealIE(float from, float to, float duration)
  {
    float elapsed = 0.0f;
    while ((double) elapsed < (double) duration)
    {
      elapsed += Time.deltaTime;
      float t = Mathf.Clamp01(elapsed / duration);
      BiomeConstants.Instance.SetIceOverlayReveal(Mathf.Lerp(from, to, t));
      yield return (object) null;
    }
  }

  public static void PlayWinterIncrementGlobal(bool skipVisuals = false)
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) WoolhavenYngyaStatue.Instance.IncrementWinterIE(skipVisuals));
  }

  public void TestIncrementWinter()
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) WoolhavenYngyaStatue.Instance.IncrementWinterIE(true));
  }

  public IEnumerator IncrementWinterIE(bool skipVisuals = false)
  {
    WoolhavenYngyaStatue woolhavenYngyaStatue = this;
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.InteractYngyaShrine);
    MonoSingleton<UIManager>.Instance.ForceBlockPause = true;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(woolhavenYngyaStatue.gameObject, 7f);
    GameManager.GetInstance().CameraSetOffset(new Vector3(0.0f, 0.0f, -3f));
    if (!skipVisuals)
      yield return (object) woolhavenYngyaStatue.StartCoroutine((IEnumerator) woolhavenYngyaStatue.IncrementWinterVisualsIE());
    BuildingShrine.ShowingDLCTree = true;
    BuildingShrine.AnimateSnowDLCTree = true;
    Interaction_DLCYngyaShrine.Instance.XPBar.UpdateBar(0.0f);
    UIUpgradeTreeMenuController tree;
    if (!UpgradeSystem.GetUnlocked(UpgradeSystem.Type.WinterSystem))
    {
      GameManager.GetInstance().OnConversationNew();
      tree = woolhavenYngyaStatue.ShowWinterUpgradeTree();
      tree.SetLockCancelInput(true);
      while ((UnityEngine.Object) tree != (UnityEngine.Object) null)
        yield return (object) null;
      if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Winter))
        MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Winter, callback: (System.Action) (() => { }));
    }
    else
    {
      GameManager.GetInstance().OnConversationEnd();
      ++SeasonsManager.WinterSeverity;
      if (SeasonsManager.CurrentWeatherEvent != SeasonsManager.WeatherEvent.Blizzard)
        SeasonsManager.SetSeasonDefaultWeather(SeasonsManager.CurrentSeason);
      switch (SeasonsManager.WinterSeverity)
      {
        case 1:
          Interaction_DLCYngyaShrine.Instance.firstWinterConvo.Play();
          break;
        case 2:
          if (SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter)
            SeasonsManager.SetSeason(SeasonsManager.Season.Winter);
          DataManager.Instance.OnboardedBlizzards = true;
          Interaction_DLCYngyaShrine.Instance.secondWinterIncrementConvo.Play();
          break;
        case 3:
          DataManager.Instance.OnboardedRanchingWolves = true;
          DataManager.Instance.WinterMaxSeverity = true;
          Interaction_DLCYngyaShrine.Instance.thirdWinterConvo.Play();
          break;
        case 4:
          if (SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter)
            SeasonsManager.SetSeason(SeasonsManager.Season.Winter);
          DataManager.Instance.OnboardedSnowedUnder = true;
          Interaction_DLCYngyaShrine.Instance.fourthWinterIncrementConvo.Play();
          break;
        case 5:
          if (SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter)
            SeasonsManager.SetSeason(SeasonsManager.Season.Winter);
          Interaction_DLCYngyaShrine.Instance.fifthWinterConvo.Play();
          break;
        default:
          Interaction_DLCYngyaShrine.Instance.timeToFight.Play();
          AudioManager.Instance.StopCurrentMusic();
          break;
      }
      while (MMConversation.isPlaying)
        yield return (object) null;
      for (int index = 0; index < PlayerFarming.playersCount; ++index)
        PlayerFarming.players[index].EndGoToAndStop();
      yield return (object) null;
      yield return (object) null;
      GameManager.GetInstance().OnConversationNew();
      if (!DataManager.Instance.WinterModeActive && (SeasonsManager.WinterSeverity == 0 || SeasonsManager.WinterSeverity == 2 || SeasonsManager.WinterSeverity == 4))
      {
        switch (SeasonsManager.WinterSeverity)
        {
          case 2:
            yield return (object) SeasonsManager.SecondWinterIE();
            Interaction_DLCYngyaShrine.Instance.secondWinterIncrementConvo_B.Play();
            break;
          case 4:
            yield return (object) SeasonsManager.FourthWinterIE();
            Interaction_DLCYngyaShrine.Instance.fourthWinterIncrementConvo_B.Play();
            break;
        }
        while (MMConversation.isPlaying)
          yield return (object) null;
        yield return (object) new WaitForSeconds(0.25f);
        tree = MonoSingleton<UIManager>.Instance.ShowUpgradeTree();
        while ((UnityEngine.Object) tree != (UnityEngine.Object) null)
          yield return (object) null;
        yield return (object) DLCFurnanceSequences.PlayUpgradeSequenceIE();
        yield return (object) new WaitForEndOfFrame();
        while (MMConversation.isPlaying)
          yield return (object) null;
        switch (SeasonsManager.WinterSeverity)
        {
          case 2:
            GameManager.GetInstance().OnConversationNew();
            DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Winter_2);
            MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Winter_2, callback: (System.Action) (() => GameManager.GetInstance().OnConversationEnd()));
            break;
          case 4:
            GameManager.GetInstance().OnConversationNew();
            DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Winter_4);
            MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Winter_4, callback: (System.Action) (() =>
            {
              ObjectiveManager.Add((ObjectivesData) new Objectives_BuildStructure("Objectives/GroupTitles/WarmCult", StructureBrain.TYPES.PROXIMITY_FURNACE), true, true);
              GameManager.GetInstance().OnConversationEnd();
            }));
            break;
        }
        tree = (UIUpgradeTreeMenuController) null;
      }
      else
      {
        yield return (object) new WaitForEndOfFrame();
        while (PlayerFarming.Instance.state.CURRENT_STATE == StateMachine.State.InActive)
          yield return (object) null;
        while (MMConversation.isPlaying)
          yield return (object) null;
        GameManager.GetInstance().OnConversationNew();
        switch (SeasonsManager.WinterSeverity)
        {
          case 1:
            DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Winter_1);
            MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Winter_1, callback: (System.Action) (() => { }));
            break;
          case 3:
            DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Winter_3);
            MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Winter_3, callback: (System.Action) (() => { }));
            break;
          case 5:
            DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Winter_5);
            MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Winter_5, callback: (System.Action) (() => { }));
            break;
        }
        HUD_Winter.Instance.UpdateBarFeatures(true);
        GameManager.GetInstance().OnConversationEnd();
      }
      BuildingShrine.ShowingDLCTree = false;
      BuildingShrine.AnimateSnowDLCTree = false;
      MonoSingleton<UIManager>.Instance.ForceBlockPause = false;
    }
  }

  public void PlayIncrementWinterVisuals()
  {
    this.StartCoroutine((IEnumerator) this.IncrementWinterVisualsIE(true));
  }

  public IEnumerator IncrementWinterVisualsIE(bool setPlayerIdle = false)
  {
    WoolhavenYngyaStatue woolhavenYngyaStatue = this;
    PlayerFarming playerFarming = PlayerFarming.Instance;
    AudioManager.Instance.PlayOneShot("event:/dlc/env/yngya_shrine/woolhaven_powerup_start");
    int stingerIndex = SeasonsManager.WinterSeverity + 1;
    woolhavenYngyaStatue.PlayShrinePowerupStinger(stingerIndex);
    yield return (object) new WaitForSeconds(1f);
    yield return (object) woolhavenYngyaStatue.StartCoroutine((IEnumerator) woolhavenYngyaStatue.ShakeCameraWithRampUp());
    BiomeConstants.Instance.ImpactFrameForDuration();
    CameraManager.instance.ShakeCameraForDuration(1.5f, 2f, 0.1f);
    AudioManager.Instance.PlayOneShot("event:/rituals/blood_sacrifice", woolhavenYngyaStatue.gameObject);
    AudioManager.Instance.PlayOneShot("event:/Stings/DLC_IntroBell", woolhavenYngyaStatue.gameObject);
    WoolhavenYngyaStatue.YngyaRotSkin yngyaRotSkinIndex = WoolhavenYngyaStatue.GetNextYngyaRotSkinIndex();
    woolhavenYngyaStatue.SetYngyaSkin(yngyaRotSkinIndex, woolhavenYngyaStatue.ExpectedYngyaBellSkinIndex());
    woolhavenYngyaStatue.UpdateAmbientLoop(yngyaRotSkinIndex);
    WoolhavenPlazaRotManager.Instance.SetCurrentPlazaRot();
    WoolhavenXPBarRotManager.Instance.SetCurrentXPBarRot();
    Interaction_DLCYngyaShrine.Instance.ritual.gameObject.SetActive(true);
    Interaction_DLCYngyaShrine.Instance.awoken.gameObject.SetActive(true);
    GameManager.GetInstance().CameraSetOffset(new Vector3(0.0f, 0.0f, -5f));
    BiomeConstants.Instance.SetIceOverlayReveal(1f);
    GameManager.GetInstance().CameraZoom(10f, 0.1f);
    BiomeConstants.Instance.EmitDisplacementEffect(Interaction_DLCYngyaShrine.Instance.transform.position);
    WeatherSystemController.WeatherType currentWeatherType = WeatherSystemController.Instance.CurrentWeatherType;
    WeatherSystemController.WeatherStrength currentWeatherStrength = WeatherSystemController.Instance.CurrentWeatherStrength;
    WeatherSystemController.Instance.SetWeather(WeatherSystemController.WeatherType.Snowing, WeatherSystemController.WeatherStrength.Extreme);
    float startValueIn = 0.0f;
    DOTween.To((DOGetter<float>) (() => startValueIn), (DOSetter<float>) (x => BiomeConstants.Instance.SetIceOverlayReveal(x)), 1f, 3f);
    woolhavenYngyaStatue.ShowStatueEyes(true);
    woolhavenYngyaStatue.lostSouls.SetActive(true);
    playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    playerFarming.simpleSpineAnimator.Animate("collect-ghosts", 0, false);
    playerFarming.simpleSpineAnimator.AddAnimate("collect-ghosts-loop", 0, true, 0.0f);
    woolhavenYngyaStatue.plaza.gameObject.SetActive(true);
    woolhavenYngyaStatue.plaza.transform.DOScale(1f, 1f).From(0.0f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutCubic);
    woolhavenYngyaStatue.plazaVolumetrics.SetActive(true);
    SpriteRenderer[] plazaVolumetricRenderers = woolhavenYngyaStatue.plazaVolumetrics.GetComponentsInChildren<SpriteRenderer>();
    foreach (SpriteRenderer target in plazaVolumetricRenderers)
    {
      float a = target.color.a;
      target.color = new Color(target.color.r, target.color.g, target.color.b, 0.0f);
      target.DOFade(a, 2f).SetDelay<TweenerCore<Color, Color, ColorOptions>>(0.0f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutCubic);
    }
    for (int i = 0; i < 4; ++i)
    {
      BiomeConstants.Instance.EmitDisplacementEffect(Interaction_DLCYngyaShrine.Instance.transform.position);
      AudioManager.Instance.PlayOneShot("event:/ui/heartbeat", woolhavenYngyaStatue.gameObject);
      CameraManager.shakeCamera(3f, 0.0f);
      woolhavenYngyaStatue.skeleton.AnimationState.SetAnimation(0, "next-bell", false);
      woolhavenYngyaStatue.skeleton.AnimationState.AddAnimation(0, "animation", true, 0.0f);
      yield return (object) new WaitForSeconds(1.5f);
    }
    Interaction_DLCYngyaShrine.Instance.ritual.gameObject.SetActive(false);
    float startValueOut = 1f;
    DOTween.To((DOGetter<float>) (() => startValueOut), (DOSetter<float>) (x => BiomeConstants.Instance.SetIceOverlayReveal(x)), 0.0f, 1f);
    woolhavenYngyaStatue.ShowStatueEyes(false);
    Interaction_DLCYngyaShrine.Instance.XPBar.UpdateBar(0.0f);
    playerFarming.simpleSpineAnimator.Animate("collect-ghosts-land", 0, false);
    playerFarming.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    woolhavenYngyaStatue.lostSouls.SetActive(false);
    foreach (SpriteRenderer spriteRenderer in plazaVolumetricRenderers)
    {
      SpriteRenderer child = spriteRenderer;
      float startAlpha = child.color.a;
      child.DOFade(0.0f, 1f).From(startAlpha).SetDelay<TweenerCore<Color, Color, ColorOptions>>(0.0f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutCubic).OnComplete<TweenerCore<Color, Color, ColorOptions>>((TweenCallback) (() =>
      {
        child.color = new Color(child.color.r, child.color.g, child.color.b, startAlpha);
        this.plazaVolumetrics.SetActive(false);
      }));
    }
    woolhavenYngyaStatue.plaza.transform.DOScale(0.0f, 1f).From(1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutCubic).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      this.plaza.gameObject.SetActive(false);
      if (!setPlayerIdle)
        return;
      playerFarming.state.CURRENT_STATE = StateMachine.State.Idle;
    }));
    if (currentWeatherType == WeatherSystemController.WeatherType.Snowing && currentWeatherStrength >= WeatherSystemController.WeatherStrength.Heavy)
      WeatherSystemController.Instance.SetWeather(currentWeatherType, currentWeatherStrength);
    else
      WeatherSystemController.Instance.SetWeather(WeatherSystemController.WeatherType.Snowing, WeatherSystemController.WeatherStrength.Heavy);
  }

  public void PlayShrinePowerupStinger(int stingerIndex)
  {
    stingerIndex = Mathf.Clamp(stingerIndex, 0, this.shrinePowerUpStingers.Length - 1);
    if (stingerIndex < 0 || stingerIndex >= this.shrinePowerUpStingers.Length)
      return;
    string shrinePowerUpStinger = this.shrinePowerUpStingers[stingerIndex];
    if (string.IsNullOrEmpty(shrinePowerUpStinger))
      return;
    AudioManager.Instance.PlayOneShot(shrinePowerUpStinger);
  }

  public UIUpgradeTreeMenuController ShowWinterUpgradeTree()
  {
    GameManager.GetInstance().OnConversationNew();
    MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = PlayerFarming.Instance;
    return MonoSingleton<UIManager>.Instance.ShowUpgradeTree((System.Action) (() =>
    {
      UpgradeSystem.UnlockAbility(UpgradeSystem.Type.WinterSystem);
      SimulationManager.UnPause();
      GameManager.GetInstance().OnConversationEnd();
      MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
      MonoSingleton<UIManager>.Instance.ForceBlockPause = false;
    }), UpgradeSystem.Type.WinterSystem);
  }

  public IEnumerator ShakeCameraWithRampUp()
  {
    float t = 0.0f;
    while ((double) (t += Time.deltaTime) < 3.0)
    {
      float t1 = t / 3f;
      CameraManager.instance.ShakeCameraForDuration(Mathf.Lerp(0.0f, 0.5f, t1), Mathf.Lerp(0.0f, 1.5f, t1), 3f, false);
      yield return (object) null;
    }
    CameraManager.instance.Stopshake();
  }

  public void UnlockWoolhavenAchievementIfComplete()
  {
    DLCShrineRoomLocationManager.CheckWoolhavenCompleteAchievement();
  }

  public void PlayWoolhavenIntro()
  {
    this.onboardRoutine = GameManager.GetInstance().StartCoroutine((IEnumerator) this.WoolhavenIntroIE());
  }

  public IEnumerator WoolhavenIntroIE()
  {
    WoolhavenYngyaStatue woolhavenYngyaStatue = this;
    yield return (object) new WaitForEndOfFrame();
    DataManager.Instance.DiscoverLocation(FollowerLocation.LambTown);
    DataManager.Instance.VisitedLocations.Add(FollowerLocation.LambTown);
    GameManager.SetGlobalOcclusionActive(false);
    BaseGoopDoor.WoolhavenDoor.SetDoorUp();
    TimeManager.PauseGameTime = true;
    DataManager.Instance.OnboardedLambTown = true;
    GameManager.GetInstance().OnConversationNew();
    yield return (object) new WaitForSeconds(0.2f);
    BiomeConstants.Instance.DepthOfFieldTween(0.15f, 8.7f, 26f, 0.0f, 0.0f);
    yield return (object) null;
    UIManager.PlayAudio("event:/dlc/env/woolhaven/enter_firsttime_start");
    UIManager.PlayAudio("event:/dlc/music/woolhaven/enter_firsttime_start");
    woolhavenYngyaStatue.IntroSetCamera.Play();
    yield return (object) new WaitForSeconds(2f);
    woolhavenYngyaStatue.IntroSetCamera.Reset(0.1f);
    GameManager.GetInstance().CamFollowTarget.enabled = false;
    GameManager.GetInstance().CamFollowTarget.ClearAllTargets();
    GameManager.GetInstance().CamFollowTarget.transform.DOKill();
    GameManager.GetInstance().CamFollowTarget.ForceSnapTo(new Vector3(-20f, 23f, -5f));
    GameManager.GetInstance().CamFollowTarget.transform.DOKill();
    GameManager.GetInstance().CamFollowTarget.transform.DOMove(new Vector3(-10f, 23f, -5f), 5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear);
    yield return (object) new WaitForSeconds(3f);
    GameManager.GetInstance().CamFollowTarget.ForceSnapTo(new Vector3(22f, 33f, -5f));
    GameManager.GetInstance().CamFollowTarget.transform.DOKill();
    GameManager.GetInstance().CamFollowTarget.transform.DOMove(new Vector3(15f, 30f, -5f), 5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear);
    yield return (object) new WaitForSeconds(3f);
    GameManager.GetInstance().CamFollowTarget.transform.eulerAngles = new Vector3(-45f, 0.0f, 0.0f);
    GameManager.GetInstance().CamFollowTarget.ForceSnapTo(new Vector3(0.0f, 33f, -5f));
    GameManager.GetInstance().CamFollowTarget.transform.DOKill();
    GameManager.GetInstance().CamFollowTarget.transform.DOMove(new Vector3(0.0f, 17f, -10f), 3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    yield return (object) new WaitForSeconds(3f);
    HUD_DisplayName.Instance.Show(LocalizationManager.GetTranslation("NAMES/Places/LambTown"), 3f, HUD_DisplayName.Positions.Centre);
    yield return (object) new WaitForSeconds(3f);
    GameManager.GetInstance().CamFollowTarget.ForceSnapTo(new Vector3(0.0f, 17f, -10f));
    GameManager.GetInstance().CamFollowTarget.transform.DOKill();
    woolhavenYngyaStatue.StartCoroutine((IEnumerator) woolhavenYngyaStatue.ShakeStatues());
    yield return (object) new WaitForSeconds(2f);
    ConversationObject conv = new ConversationObject(new List<ConversationEntry>()
    {
      new ConversationEntry(woolhavenYngyaStatue.fleeces[0].gameObject, "Conversation_NPC/Lambtown/Intro/GraveConvo/1")
    }, (List<MMTools.Response>) null, (System.Action) null);
    int count = 1;
    float increment = 3f;
    for (int i = 0; i < woolhavenYngyaStatue.fleeces.Length; ++i)
    {
      if (i == woolhavenYngyaStatue.fleeces.Length - 1)
        count = 3;
      conv.Entries[0].Speaker = woolhavenYngyaStatue.fleeces[i].gameObject;
      conv.Entries[0].TermToSpeak = $"Conversation_NPC/Lambtown/Intro/GraveConvo/{count}";
      CameraManager.instance.ShakeCameraForDuration(0.3f, 0.6f, 0.2f);
      MMConversation.PlayBark(conv);
      AudioManager.Instance.PlayOneShot("event:/dlc/env/woolhaven/enter_firsttime_statues_dialogue_screenshake");
      ++count;
      if (count > 3)
      {
        count = 1;
        increment -= 0.75f;
      }
      yield return (object) new WaitForSeconds(increment);
    }
    yield return (object) new WaitForSeconds(2f);
    MMConversation.mmConversation.Close();
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().CamFollowTarget.enabled = true;
    GameManager.GetInstance().OnConversationEnd();
    GameManager.SetGlobalOcclusionActive(true);
    yield return (object) woolhavenYngyaStatue.StartCoroutine((IEnumerator) Interaction_DLCYngyaShrine.Instance.DoorDownIE());
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/GoToDLCDungeonFirstTime", Objectives.CustomQuestTypes.GoToDLCDungeonFirstTime), true, true);
    TimeManager.PauseGameTime = false;
  }

  public IEnumerator ShakeStatues()
  {
    List<Vector3> originalPositions = new List<Vector3>();
    for (int index = 0; index < ((IEnumerable<Interaction_PurchasableFleece>) this.fleeces).Count<Interaction_PurchasableFleece>(); ++index)
      originalPositions.Add(this.fleeces[index].GraveStone.transform.position);
    AudioManager.Instance.PlayOneShot("event:/dlc/env/woolhaven/enter_firsttime_statues_shake");
    AudioManager.Instance.PlayOneShot("event:/dlc/music/woolhaven/enter_firsttime_statues_shake");
    float time = 0.0f;
    float emitInterval = 0.2f;
    while ((double) (time += Time.deltaTime) < 21.0)
    {
      float t = time / 21f;
      float num = Mathf.Clamp01(time / 4f);
      if ((double) time >= 4.0)
        num = Mathf.Max(num + UnityEngine.Random.Range(-0.2f, 0.2f), 0.0f);
      for (int index = 0; index < ((IEnumerable<Interaction_PurchasableFleece>) this.fleeces).Count<Interaction_PurchasableFleece>(); ++index)
      {
        this.fleeces[index].GraveStone.transform.position = originalPositions[index] + (Vector3) (UnityEngine.Random.insideUnitCircle * Mathf.Lerp(0.0f, 0.1f, t));
        if ((double) time % (double) emitInterval < (double) Time.deltaTime)
          BiomeConstants.Instance.EmitSmokeInteractionVFX(this.fleeces[index].GraveStone.transform.position, Vector3.one * num);
      }
      yield return (object) null;
    }
  }

  public void PlayOnboardLambGhostsNPC()
  {
    this.onboardRoutine = GameManager.GetInstance().StartCoroutine((IEnumerator) this.OnboardLambGhostsNPC());
  }

  public IEnumerator OnboardLambGhostsNPC()
  {
    WoolhavenYngyaStatue woolhavenYngyaStatue = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(woolhavenYngyaStatue.ranchingGrave.gameObject, 8f);
    GameManager.GetInstance().CameraSetOffset(new Vector3(0.0f, 0.0f, -0.5f));
    woolhavenYngyaStatue.ranchingGrave.SetEyes(false);
    woolhavenYngyaStatue.ranchingGrave.NewBuildingAvailableObject.SetActive(false);
    GameManager.GetInstance().CameraZoom(6f, 3f);
    EventInstance introInstance = AudioManager.Instance.PlayOneShotWithInstance("event:/music/intro/intro_bass");
    yield return (object) new WaitForSeconds(3f);
    DataManager.Instance.OnboardedLambGhostNPCs = true;
    woolhavenYngyaStatue.ranchingGrave.SetState(Interaction_PurchasableFleece.State.CanBury);
    Vector3 position = woolhavenYngyaStatue.ranchingGrave.transform.position;
    position.z -= 0.5f;
    BiomeConstants.Instance.EmitDisplacementEffect(position);
    CameraManager.shakeCamera(20f, 0.5f);
    GameManager.GetInstance().CameraZoom(8f, 0.1f);
    woolhavenYngyaStatue.ranchingGrave.NewBuildingAvailableObject.SetActive(true);
    woolhavenYngyaStatue.ranchingGrave.SetEyes(true);
    AudioManager.Instance.StopOneShotInstanceEarly(introInstance, STOP_MODE.IMMEDIATE);
    AudioManager.Instance.PlayOneShot("event:/upgrade_statue/upgrade_unlock");
    AudioManager.Instance.PlayOneShot("event:/dlc/dungeon06/trap/flesh_rock/block_petrified_destroy", woolhavenYngyaStatue.gameObject);
    yield return (object) new WaitForSeconds(2f);
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/BuryLostGhost", Objectives.CustomQuestTypes.BuryLostGhost), true, true);
    GameManager.GetInstance().CameraSetOffset(new Vector3(0.0f, 0.0f, 0.0f));
    GameManager.GetInstance().OnConversationEnd();
  }

  public static IEnumerator FadeIn(float duration)
  {
    bool waitingForFade = true;
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, duration, "", (System.Action) (() => waitingForFade = false));
    while (waitingForFade)
      yield return (object) null;
  }

  public void OnDrawGizmosSelected()
  {
    Utils.DrawCircleXY(this.transform.position, this.activationDistance, Color.yellow);
  }

  public void TryToPlayFinalSequence()
  {
    if (!DataManager.Instance.BeatenYngya || DataManager.Instance.FinalDLCMap)
      return;
    DataManager.Instance.FinalDLCMap = true;
    this.PlayYngyaFinalSequence();
  }

  public void PlayYngyaFinalSequence()
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.YngyaFinalSequence());
  }

  public IEnumerator YngyaFinalSequence()
  {
    WoolhavenYngyaStatue woolhavenYngyaStatue = this;
    GameManager.SetGlobalOcclusionActive(false);
    PlayerFarming.StopRidingOnAnimals();
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = true;
    int num1 = (int) AudioManager.Instance.CurrentMusicInstance.setPaused(true);
    woolhavenYngyaStatue.ghostNPCCircleContainer.gameObject.SetActive(true);
    woolhavenYngyaStatue.SetYngyaSkin(WoolhavenYngyaStatue.YngyaRotSkin.FullRot, WoolhavenYngyaStatue.YngyaBellSkin.Rot5);
    woolhavenYngyaStatue.UpdateAmbientLoop(WoolhavenYngyaStatue.YngyaRotSkin.FullRot);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(woolhavenYngyaStatue.gameObject);
    GameManager.GetInstance().CameraSetOffset(new Vector3(0.0f, 0.0f, -4f));
    yield return (object) new WaitForSeconds(2f);
    int rotationTiltStrength = 5;
    EventInstance introInstance = AudioManager.Instance.PlayOneShotWithInstance("event:/music/intro/intro_bass");
    AudioManager.Instance.PlayOneShot("event:/dlc/music/yngya_shrine/woolhaven_purify");
    GameManager.GetInstance().CameraZoom(6f, 3f);
    yield return (object) new WaitForSeconds(1f);
    AudioManager.Instance.PlayOneShot("event:/dlc/dungeon06/trap/flesh_rock/block_fleshy_destroy", woolhavenYngyaStatue.gameObject);
    AudioManager.Instance.PlayOneShot("event:/ui/heartbeat", woolhavenYngyaStatue.gameObject);
    CameraManager.shakeCamera(8f, 0.5f);
    CameraManager.instance.CameraRef.transform.Rotate(new Vector3(0.0f, 0.0f, (float) rotationTiltStrength));
    woolhavenYngyaStatue.skeleton.AnimationState.SetAnimation(0, "break-rot-1", false);
    yield return (object) new WaitForSeconds(1f);
    AudioManager.Instance.PlayOneShot("event:/dlc/dungeon06/trap/flesh_rock/block_fleshy_destroy", woolhavenYngyaStatue.gameObject);
    AudioManager.Instance.PlayOneShot("event:/ui/heartbeat", woolhavenYngyaStatue.gameObject);
    CameraManager.shakeCamera(8f, 0.5f);
    CameraManager.instance.CameraRef.transform.Rotate(new Vector3(0.0f, 0.0f, (float) -(rotationTiltStrength * 2)));
    woolhavenYngyaStatue.skeleton.AnimationState.SetAnimation(0, "break-rot-2", false);
    Vector3 vfxPos = woolhavenYngyaStatue.transform.position with
    {
      z = 0.0f
    };
    CameraManager.instance.ShakeCameraForDuration(0.1f, 0.5f, 1f);
    yield return (object) new WaitForSeconds(1f);
    CameraManager.instance.CameraRef.transform.Rotate(new Vector3(0.0f, 0.0f, (float) rotationTiltStrength));
    AudioManager.Instance.PlayOneShot("event:/dlc/dungeon06/trap/flesh_rock/main_death", woolhavenYngyaStatue.gameObject);
    AudioManager.Instance.PlayOneShot("event:/Stings/DLC_IntroBell", woolhavenYngyaStatue.gameObject);
    AudioManager.Instance.StopOneShotInstanceEarly(introInstance, STOP_MODE.IMMEDIATE);
    woolhavenYngyaStatue.rotBreakVFX.Play();
    BiomeConstants.Instance.EmitDisplacementEffect(vfxPos);
    CameraManager.shakeCamera(20f, 0.5f);
    GameManager.GetInstance().CameraZoom(9f, 0.1f);
    woolhavenYngyaStatue.SetYngyaSkin(WoolhavenYngyaStatue.YngyaRotSkin.NoRot, (WoolhavenYngyaStatue.YngyaBellSkin) (woolhavenYngyaStatue.bellBreakCount + 1));
    woolhavenYngyaStatue.UpdateAmbientLoop(WoolhavenYngyaStatue.YngyaRotSkin.NoRot);
    woolhavenYngyaStatue.skeleton.AnimationState.SetAnimation(0, "next-bell", false);
    woolhavenYngyaStatue.skeleton.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    yield return (object) new WaitForSeconds(2f);
    woolhavenYngyaStatue.yngyaStatueSpriteFlowers.gameObject.SetActive(true);
    woolhavenYngyaStatue.yngyaStatueSpriteFlowers.material.SetFloat("_RotReveal", 0.0f);
    AudioManager.Instance.PlayOneShot("event:/comic sfx/earth_break_land", woolhavenYngyaStatue.gameObject);
    CameraManager.instance.ShakeCameraForDuration(0.1f, 0.5f, 2f);
    yield return (object) woolhavenYngyaStatue.yngyaStatueSpriteFlowers.material.DOFloat(1f, "_RotReveal", 3f).From<float, float, FloatOptions>(0.0f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.Linear).WaitForCompletion();
    yield return (object) new WaitForSeconds(1f);
    yield return (object) DLCMap.ClearRotRoutine();
    yield return (object) woolhavenYngyaStatue.StartCoroutine((IEnumerator) WoolhavenYngyaStatue.FadeIn());
    woolhavenYngyaStatue.SetYngyaSkin(WoolhavenYngyaStatue.YngyaRotSkin.Flowers, WoolhavenYngyaStatue.YngyaBellSkin.NoRot);
    woolhavenYngyaStatue.UpdateAmbientLoop(WoolhavenYngyaStatue.YngyaRotSkin.Flowers);
    woolhavenYngyaStatue.ghostNPCCircleContainer.gameObject.SetActive(false);
    woolhavenYngyaStatue.yngyaStatueSpriteFlowers.gameObject.SetActive(false);
    GameManager.GetInstance().OnConversationEnd();
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    int num2 = (int) AudioManager.Instance.CurrentMusicInstance.setPaused(false);
    yield return (object) woolhavenYngyaStatue.StartCoroutine((IEnumerator) WoolhavenYngyaStatue.FadeOut());
    GameManager.SetGlobalOcclusionActive(true);
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
    if (!UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ability_WinterChoice))
      ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/WinterChoice", Objectives.CustomQuestTypes.WinterChoice), true, true);
  }

  [CompilerGenerated]
  public void \u003CYngyaAwokenIE\u003Eb__47_2() => this.plaza.gameObject.SetActive(false);

  public enum YngyaBellSkin
  {
    NoRot,
    Rot1,
    Rot2,
    Rot3,
    Rot4,
    Rot5,
    Repaired,
    Broken,
  }

  public enum YngyaRotSkin
  {
    NoRot,
    SlightRot,
    MediumRot,
    HeavyRot,
    FullRot,
    Flowers,
  }
}
