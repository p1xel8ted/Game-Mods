// Decompiled with JetBrains decompiler
// Type: DeathCatController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.DeathScreen;
using MMBiomeGeneration;
using MMTools;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class DeathCatController : BaseMonoBehaviour
{
  public const int MAX_FOLLOWERS = 12;
  public static DeathCatController Instance;
  public PlayerFarming playerFarming;
  public PlayerFarming playerBehind;
  public Vector3 playerBehindTargetPosition;
  [SerializeField]
  public UnitObject guardian1;
  [SerializeField]
  public GuardianBossIntro guardian1Intro;
  [SerializeField]
  public List<BaseMonoBehaviour> guardian1Components;
  [SerializeField]
  public UnitObject guardian2;
  [SerializeField]
  public List<BaseMonoBehaviour> guardian2Components;
  [SerializeField]
  public DeathCatClone deathCatClone;
  [SerializeField]
  public GameObject deathCatCloneCamera;
  [SerializeField]
  public EnemyDeathCatBoss deathCatBig;
  [Space]
  [SerializeField]
  public Vector2 fervourTimeDropInterval;
  [SerializeField]
  public GameObject followerToSpawn;
  public Interaction_SimpleConversation conversation0;
  public Interaction_SimpleConversation conversation0_B;
  public Interaction_SimpleConversation conversation0_C;
  public Interaction_SimpleConversation conversation1;
  public Interaction_SimpleConversation conversation2;
  public Interaction_SimpleConversation conversation3;
  public Interaction_SimpleConversation conversation4;
  public Interaction_SimpleConversation conversation5;
  public Interaction_SimpleConversation conversation6_A;
  public Interaction_SimpleConversation conversation6_B;
  [SerializeField]
  public GameObject whiteRoom;
  [SerializeField]
  public GameObject redRoom;
  [SerializeField]
  public GameObject transitionRoom;
  [SerializeField]
  public List<GameObject> torches;
  [SerializeField]
  public List<GameObject> eyes;
  [SerializeField]
  public SpriteRenderer blackFade;
  [SerializeField]
  public GameObject deathCatLighting;
  public SkeletonAnimation Chain1Spine;
  public SkeletonAnimation Chain2Spine;
  public SkeletonAnimation Chain3Spine;
  public SkeletonAnimation BChain1Spine;
  public SkeletonAnimation BChain2Spine;
  public SkeletonAnimation BChain3Spine;
  public SkeletonAnimation BChain4Spine;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Chain1Spine")]
  public string breakAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Chain1Spine")]
  public string backgroundBreak1Animation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Chain1Spine")]
  public string backgroundBreak2Animation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Chain1Spine")]
  public string backgroundBreak3Animation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Chain1Spine")]
  public string backgroundBreak4Animation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Chain1Spine")]
  public string brokenAnimation;
  [Space]
  [SerializeField]
  public GameObject[] eyeBones;
  [SerializeField]
  public EnemyDeathCatEyesManager eyesManager;
  [SerializeField]
  public ParticleSystem transformParticles;
  [Space]
  [SerializeField]
  public GameObject followerChainsParent;
  [SerializeField]
  public GameObject[] cages;
  public CapturedFollowerChain[] followerChains;
  public float timeBetweenFervourDrop;
  [CompilerGenerated]
  public bool \u003CDroppingFervour\u003Ek__BackingField = true;
  public List<FollowerManager.SpawnedFollower> chainedFollowers = new List<FollowerManager.SpawnedFollower>();
  public List<FollowerManager.SpawnedFollower> cagedFollowers = new List<FollowerManager.SpawnedFollower>();
  public int originalFleece;
  public int originalVisualFleece;
  [SerializeField]
  public GameObject distortionObject;
  public bool skippable;
  public Interaction_FollowerSpawn followerSpawn;
  public static int count;

  public bool DroppingFervour
  {
    get => this.\u003CDroppingFervour\u003Ek__BackingField;
    set => this.\u003CDroppingFervour\u003Ek__BackingField = value;
  }

  public void Awake()
  {
    if (!DungeonSandboxManager.Active)
    {
      this.originalFleece = DataManager.Instance.PlayerFleece;
      this.originalVisualFleece = DataManager.Instance.PlayerVisualFleece;
      DataManager.Instance.PlayerFleece = 0;
      DataManager.Instance.PlayerVisualFleece = 0;
    }
    if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
      PlayerFarming.Instance.SetSkin();
    DeathCatController.Instance = this;
    foreach (BaseMonoBehaviour guardian1Component in this.guardian1Components)
    {
      if ((UnityEngine.Object) guardian1Component != (UnityEngine.Object) null)
        guardian1Component.enabled = false;
    }
    foreach (BaseMonoBehaviour guardian2Component in this.guardian2Components)
    {
      if ((UnityEngine.Object) guardian2Component != (UnityEngine.Object) null)
        guardian2Component.enabled = false;
    }
    this.guardian1.GetComponent<Health>().OnDie += new Health.DieAction(this.Guardian1_OnDie);
    this.redRoom.SetActive(false);
    this.transitionRoom.SetActive(false);
    this.whiteRoom.SetActive(true);
    this.followerChains = this.followerChainsParent.transform.GetComponentsInChildren<CapturedFollowerChain>();
    this.InitializeFollowers();
    if (DungeonSandboxManager.Active || DataManager.Instance.RemovedStoryMomentsActive)
      this.SpawnFollowersInCage();
    AudioManager.Instance.PlayMusic("event:/music/death_cat_battle/death_cat_battle");
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.StandardRoom);
    if (!DungeonSandboxManager.Active || !((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null))
      return;
    BiomeGenerator.Instance.biomeMusicPath = "";
    BiomeGenerator.Instance.biomeAtmosPath = "";
  }

  public void OnEnable()
  {
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.OnLanguageChanged);
  }

  public void OnDisable()
  {
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.OnLanguageChanged);
  }

  public void Update()
  {
    if (this.deathCatBig.enabled && this.DroppingFervour && (double) GameManager.GetInstance().CurrentTime > (double) this.timeBetweenFervourDrop)
    {
      for (int index = 0; index < 50; ++index)
      {
        CapturedFollowerChain followerChain = this.followerChains[UnityEngine.Random.Range(0, this.followerChains.Length)];
        if (!followerChain.DroppingFervour)
        {
          followerChain.DropFervour();
          break;
        }
      }
      this.timeBetweenFervourDrop = GameManager.GetInstance().CurrentTime + UnityEngine.Random.Range(this.fervourTimeDropInterval.x, this.fervourTimeDropInterval.y);
    }
    if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null) || !this.skippable || !InputManager.Gameplay.GetAttackButtonDown(this.playerFarming) && !DungeonSandboxManager.Active && !DataManager.Instance.RemovedStoryMomentsActive)
      return;
    if ((UnityEngine.Object) this.playerBehind != (UnityEngine.Object) null && this.playerBehind.GoToAndStopping)
    {
      this.playerBehind.EndGoToAndStop();
      this.playerBehind.transform.position = this.playerBehindTargetPosition;
    }
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.SkipIntro());
  }

  public void LateUpdate() => SimulationManager.Pause();

  public void InitializeFollowers()
  {
    List<FollowerBrain> followerBrainList = new List<FollowerBrain>((IEnumerable<FollowerBrain>) FollowerBrain.AllBrains);
    for (int index = followerBrainList.Count - 1; index >= 0; --index)
    {
      if (DataManager.Instance.Followers_Dead.Contains(followerBrainList[index]._directInfoAccess) || followerBrainList[index].Info.CursedState == Thought.Child)
        followerBrainList.RemoveAt(index);
    }
    if (DungeonSandboxManager.Active)
    {
      for (int index = 0; index < 12; ++index)
      {
        FollowerInfo info = FollowerInfo.NewCharacter(FollowerLocation.Base);
        followerBrainList.Add(FollowerBrain.GetOrCreateBrain(info));
      }
    }
    int num = Mathf.CeilToInt((float) (Mathf.Min(followerBrainList.Count, 12) / 2));
    for (int index = 0; index < followerBrainList.Count && index < 12; ++index)
    {
      if (index < num)
      {
        FollowerManager.SpawnedFollower spawnedFollower = FollowerManager.SpawnCopyFollower(followerBrainList[index]._directInfoAccess, this.cages[0].transform.position + new Vector3(Mathf.Lerp(-1.5f, 2f, (float) index / (float) num), UnityEngine.Random.Range(-0.1f, 0.4f), 0.0f), this.transform, PlayerFarming.Location);
        this.cagedFollowers.Add(spawnedFollower);
        spawnedFollower.Follower.transform.parent = this.cages[0].transform;
        if (index == 3)
          spawnedFollower.Follower.transform.position = this.cages[0].transform.position + new Vector3(Mathf.Lerp(-1.5f, 2f, (float) index / (float) num), -0.1f, 0.0f);
        spawnedFollower.Follower.gameObject.SetActive(false);
      }
      else if (index >= num)
      {
        FollowerManager.SpawnedFollower spawnedFollower = FollowerManager.SpawnCopyFollower(followerBrainList[index]._directInfoAccess, this.cages[1].transform.position + new Vector3(Mathf.Lerp(-1.5f, 2f, (float) (index - num) / (float) num), UnityEngine.Random.Range(-0.1f, 0.4f), 0.0f), this.transform, PlayerFarming.Location);
        this.cagedFollowers.Add(spawnedFollower);
        spawnedFollower.Follower.transform.parent = this.cages[1].transform;
        spawnedFollower.Follower.transform.localScale = new Vector3(-1f, 1f, 1f);
        spawnedFollower.Follower.gameObject.SetActive(false);
      }
    }
  }

  public IEnumerator SkipIntro()
  {
    DeathCatController deathCatController = this;
    deathCatController.skippable = false;
    LetterBox.Instance.HideSkipPrompt();
    Interaction_FinalBossAltar objectOfType = UnityEngine.Object.FindObjectOfType<Interaction_FinalBossAltar>();
    objectOfType.enabled = false;
    if (!DungeonSandboxManager.Active && !DataManager.Instance.RemovedStoryMomentsActive)
      deathCatController.playerFarming.transform.position = objectOfType.transform.position;
    CameraManager.instance.Stopshake();
    if (MMConversation.CURRENT_CONVERSATION != null)
      MMConversation.CURRENT_CONVERSATION.CallBack = (System.Action) null;
    MMConversation.mmConversation.Close();
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    yield return (object) new WaitForEndOfFrame();
    if (!DungeonSandboxManager.Active && !DataManager.Instance.RemovedStoryMomentsActive)
      deathCatController.SpawnFollowersInCage();
    foreach (BaseMonoBehaviour guardian1Component in deathCatController.guardian1Components)
    {
      if ((UnityEngine.Object) guardian1Component != (UnityEngine.Object) null)
        guardian1Component.enabled = true;
    }
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(deathCatController.guardian1.gameObject, 6f);
    deathCatController.ResetPlayerWalking();
    AudioManager.Instance.SetMusicRoomID(3, "deathcat_room_id");
    deathCatController.StartCoroutine((IEnumerator) deathCatController.guardian1Intro.PlayRoutine(false));
  }

  public void SpawnFollowersInCage()
  {
    int num = this.cagedFollowers.Count / 2;
    for (int index = 0; index < this.cagedFollowers.Count; ++index)
    {
      if (index < num)
      {
        FollowerManager.SpawnedFollower cagedFollower = this.cagedFollowers[index];
        cagedFollower.Follower.gameObject.SetActive(true);
        cagedFollower.Follower.OverridingEmotions = true;
        cagedFollower.Follower.AddBodyAnimation("idle", true, 0.0f);
        cagedFollower.Follower.SetFaceAnimation("Emotions/emotion-unhappy", true);
      }
    }
    for (int index = num; index < this.cagedFollowers.Count; ++index)
    {
      FollowerManager.SpawnedFollower cagedFollower = this.cagedFollowers[index];
      cagedFollower.Follower.gameObject.SetActive(true);
      cagedFollower.Follower.OverridingEmotions = true;
      cagedFollower.Follower.AddBodyAnimation("idle", true, 0.0f);
      cagedFollower.Follower.SetFaceAnimation("Emotions/emotion-unhappy", true);
    }
  }

  public void OnDestroy()
  {
    if ((UnityEngine.Object) this.guardian1 != (UnityEngine.Object) null)
      this.guardian1.GetComponent<Health>().OnDie -= new Health.DieAction(this.Guardian1_OnDie);
    foreach (FollowerManager.SpawnedFollower chainedFollower in this.chainedFollowers)
      FollowerManager.CleanUpCopyFollower(chainedFollower);
    foreach (FollowerManager.SpawnedFollower cagedFollower in this.cagedFollowers)
      FollowerManager.CleanUpCopyFollower(cagedFollower);
    if (DungeonSandboxManager.Active)
      return;
    DataManager.Instance.PlayerFleece = this.originalFleece;
    DataManager.Instance.PlayerVisualFleece = this.originalVisualFleece;
  }

  public void Play() => this.StartCoroutine((IEnumerator) this.IntroIE());

  public IEnumerator IntroIE()
  {
    DeathCatController deathCatController = this;
    SimulationManager.Pause();
    deathCatController.playerFarming = PlayerFarming.players[0];
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if ((UnityEngine.Object) player != (UnityEngine.Object) deathCatController.playerFarming && (double) Vector3.Distance(player.transform.position, deathCatController.transform.position) < (double) Vector3.Distance(deathCatController.playerFarming.transform.position, deathCatController.transform.position))
        deathCatController.playerFarming = player;
    }
    deathCatController.conversation0.Play(deathCatController.playerFarming.gameObject);
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if ((UnityEngine.Object) player != (UnityEngine.Object) deathCatController.playerFarming)
      {
        player.GoToAndStop(deathCatController.playerFarming.transform.position + Vector3.right, deathCatController.gameObject, DisableCollider: true);
        deathCatController.playerBehind = player;
        deathCatController.playerBehindTargetPosition = deathCatController.playerFarming.transform.position + Vector3.right;
      }
    }
    deathCatController.skippable = DataManager.Instance.BossesEncountered.Contains(PlayerFarming.Location);
    if (deathCatController.skippable)
      LetterBox.Instance.ShowSkipPrompt();
    while (MMConversation.isPlaying)
      yield return (object) null;
    deathCatController.skippable = false;
    LetterBox.Instance.HideSkipPrompt();
    yield return (object) deathCatController.StartCoroutine((IEnumerator) deathCatController.SpawnFollowersInCageIE());
  }

  public IEnumerator SpawnFollowersInCageIE()
  {
    GameManager.GetInstance().OnConversationNew();
    yield return (object) new WaitForSeconds(0.5f);
    yield return (object) new WaitForEndOfFrame();
    GameManager.GetInstance().OnConversationNext(this.cages[0]);
    List<FollowerBrain> followerBrainList = new List<FollowerBrain>((IEnumerable<FollowerBrain>) FollowerBrain.AllBrains);
    if (DungeonSandboxManager.Active)
    {
      for (int index = 0; index < 12; ++index)
      {
        FollowerInfo info = FollowerInfo.NewCharacter(FollowerLocation.Base);
        followerBrainList.Add(FollowerBrain.GetOrCreateBrain(info));
      }
    }
    int half = this.cagedFollowers.Count / 2;
    int i;
    for (i = 0; i < this.cagedFollowers.Count; ++i)
    {
      if (i < half)
      {
        FollowerManager.SpawnedFollower cagedFollower = this.cagedFollowers[i];
        cagedFollower.Follower.gameObject.SetActive(true);
        AudioManager.Instance.PlayOneShot("event:/followers/pop_in", cagedFollower.Follower.gameObject);
        cagedFollower.Follower.OverridingEmotions = true;
        double num = (double) cagedFollower.Follower.SetBodyAnimation("spawn-in", false);
        cagedFollower.Follower.AddBodyAnimation("Reactions/react-worried" + UnityEngine.Random.Range(1, 3).ToString(), false, 0.0f);
        cagedFollower.Follower.AddBodyAnimation("idle", true, 0.0f);
        cagedFollower.Follower.SetFaceAnimation("Emotions/emotion-unhappy", true);
        yield return (object) new WaitForSeconds(0.05f);
      }
    }
    yield return (object) new WaitForSeconds(2f);
    GameManager.GetInstance().OnConversationNext(this.cages[1]);
    if ((UnityEngine.Object) this.playerBehind != (UnityEngine.Object) null)
    {
      if (this.playerBehind.GoToAndStopping)
        this.playerBehind.EndGoToAndStop();
      this.playerBehind.transform.position = this.playerBehindTargetPosition;
    }
    for (i = half; i < this.cagedFollowers.Count; ++i)
    {
      FollowerManager.SpawnedFollower cagedFollower = this.cagedFollowers[i];
      cagedFollower.Follower.gameObject.SetActive(true);
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", cagedFollower.Follower.gameObject);
      cagedFollower.Follower.OverridingEmotions = true;
      double num = (double) cagedFollower.Follower.SetBodyAnimation("spawn-in", false);
      cagedFollower.Follower.AddBodyAnimation("Reactions/react-worried" + UnityEngine.Random.Range(1, 3).ToString(), false, 0.0f);
      cagedFollower.Follower.AddBodyAnimation("idle", true, 0.0f);
      cagedFollower.Follower.SetFaceAnimation("Emotions/emotion-unhappy", true);
      yield return (object) new WaitForSeconds(0.05f);
    }
    yield return (object) new WaitForSeconds(2f);
    GameManager.GetInstance().OnConversationEnd();
    this.playerFarming.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public void KneelCallback() => this.StartCoroutine((IEnumerator) this.IKneelCallback());

  public IEnumerator IKneelCallback()
  {
    DeathCatController deathCatController = this;
    GameManager.GetInstance().OnConversationNew(false);
    GameManager.GetInstance().OnConversationNext(deathCatController.playerFarming.gameObject, 6f);
    AudioManager.Instance.PlayOneShot("event:/Stings/church_bell", deathCatController.playerFarming.gameObject);
    AudioManager.Instance.PlayOneShot("event:/player/kneel_sequence", deathCatController.playerFarming.gameObject);
    deathCatController.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) null;
    deathCatController.playerFarming.Spine.AnimationState.SetAnimation(0, "final-boss/kneel", true);
    deathCatController.playerFarming.Spine.AnimationState.AddAnimation(0, "final-boss/kneel-loop", true, 0.0f);
    yield return (object) new WaitForSeconds(3f);
    GameManager.GetInstance().OnConversationNext(deathCatController.deathCatClone.gameObject, 14f);
    yield return (object) new WaitForSeconds(0.933333337f);
    MMVibrate.RumbleContinuous(0.5f, 0.75f, deathCatController.playerFarming);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.6f, float.MaxValue);
    deathCatController.deathCatBig.BaseFormSpine.AnimationState.SetAnimation(0, "break-free-crown", false);
    deathCatController.transformParticles.startDelay = 2.6f;
    deathCatController.transformParticles.Play((bool) (UnityEngine.Object) deathCatController.playerFarming.gameObject);
    deathCatController.Chain1Spine.AnimationState.SetAnimation(0, deathCatController.breakAnimation, false);
    deathCatController.Chain2Spine.AnimationState.SetAnimation(0, deathCatController.breakAnimation, false);
    deathCatController.Chain3Spine.AnimationState.SetAnimation(0, deathCatController.breakAnimation, false);
    deathCatController.BChain1Spine.AnimationState.SetAnimation(0, deathCatController.backgroundBreak1Animation, false);
    deathCatController.BChain2Spine.AnimationState.SetAnimation(0, deathCatController.backgroundBreak2Animation, false);
    deathCatController.BChain3Spine.AnimationState.SetAnimation(0, deathCatController.backgroundBreak3Animation, false);
    deathCatController.BChain4Spine.AnimationState.SetAnimation(0, deathCatController.backgroundBreak4Animation, false);
    deathCatController.Chain1Spine.AnimationState.AddAnimation(0, deathCatController.brokenAnimation, true, 0.0f);
    deathCatController.Chain2Spine.AnimationState.AddAnimation(0, deathCatController.brokenAnimation, true, 0.0f);
    deathCatController.Chain3Spine.AnimationState.AddAnimation(0, deathCatController.brokenAnimation, true, 0.0f);
    deathCatController.BChain1Spine.AnimationState.AddAnimation(0, deathCatController.brokenAnimation, true, 0.0f);
    deathCatController.BChain2Spine.AnimationState.AddAnimation(0, deathCatController.brokenAnimation, true, 0.0f);
    deathCatController.BChain3Spine.AnimationState.AddAnimation(0, deathCatController.brokenAnimation, true, 0.0f);
    deathCatController.BChain4Spine.AnimationState.AddAnimation(0, deathCatController.brokenAnimation, true, 0.0f);
    AudioManager.Instance.SetMusicRoomID(3, "deathcat_room_id");
    yield return (object) new WaitForSeconds(2f);
    deathCatController.deathCatBig.BaseFormSpine.gameObject.SetActive(false);
    deathCatController.deathCatClone.gameObject.SetActive(true);
    deathCatController.transformParticles.Stop();
    deathCatController.deathCatClone.Spine.AnimationState.SetAnimation(0, "transform", false);
    deathCatController.deathCatClone.Spine.AnimationState.AddAnimation(0, "kill-player", true, 0.0f);
    deathCatController.deathCatClone.Spine.Skeleton.SetSkin("Crown");
    foreach (FollowerManager.SpawnedFollower cagedFollower in deathCatController.cagedFollowers)
    {
      FollowerManager.SpawnedFollower f = cagedFollower;
      deathCatController.StartCoroutine((IEnumerator) deathCatController.DelayCallback(UnityEngine.Random.Range(0.0f, 0.5f), (System.Action) (() =>
      {
        double num = (double) f.Follower.SetBodyAnimation("Reactions/react-scared-long", false);
        f.Follower.AddBodyAnimation("idle", true, 0.0f);
      })));
    }
    deathCatController.deathCatClone.transform.DOMove(deathCatController.deathCatClone.transform.position + Vector3.down * 3f, 3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine);
    GameManager.GetInstance().OnConversationNext(deathCatController.deathCatCloneCamera.gameObject, 20f);
    yield return (object) new WaitForSeconds(1f);
    deathCatController.playerFarming.Spine.AnimationState.SetAnimation(0, "final-boss/die", false);
    AudioManager.Instance.PlayOneShot("event:/dialogue/death_cat/long_laugh", deathCatController.gameObject);
    CameraManager.instance.Stopshake();
    MMVibrate.StopRumble();
    DOTween.To((DOGetter<float>) (() => GameManager.GetInstance().CamFollowTarget.targetDistance), (DOSetter<float>) (x => GameManager.GetInstance().CamFollowTarget.targetDistance = x), 15f, 5f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InOutSine);
    yield return (object) new WaitForSeconds(6f);
    for (int index = 0; index < DataManager.Instance.Followers_Demons_IDs.Count; ++index)
    {
      FollowerInfo infoById = FollowerInfo.GetInfoByID(DataManager.Instance.Followers_Demons_IDs[index]);
      if (infoById != null)
        FollowerBrain.GetOrCreateBrain(infoById)?.AddThought(Thought.DemonSuccessfulRun);
    }
    DataManager.Instance.Followers_Demons_IDs.Clear();
    DataManager.Instance.Followers_Demons_Types.Clear();
    DataManager.ResetRunData();
    if (DungeonSandboxManager.Active)
    {
      if ((UnityEngine.Object) UIDeathScreenOverlayController.Instance == (UnityEngine.Object) null)
      {
        UIDeathScreenOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowDeathScreenOverlay(UIDeathScreenOverlayController.Results.Completed);
        overlayController.Show();
        overlayController.OnShown = overlayController.OnShown + new System.Action(deathCatController.\u003CIKneelCallback\u003Eb__73_3);
      }
    }
    else
    {
      MMTransition.Play(MMTransition.TransitionType.ChangeSceneAutoResume, MMTransition.Effect.BlackFade, "Credits", 10f, "", (System.Action) null);
      Credits.GoToMainMenu = true;
    }
  }

  public void RefuseToKneel() => this.StartCoroutine((IEnumerator) this.RefuseToKneelRoutine());

  public IEnumerator RefuseToKneelRoutine()
  {
    DeathCatController deathCatController = this;
    DataManager.Instance.BossesEncountered.Add(PlayerFarming.Location);
    AudioManager.Instance.PlayOneShot("event:/player/refuse_kneel_sequence", deathCatController.playerFarming.gameObject);
    GameManager.GetInstance().OnConversationNew(false);
    GameManager.GetInstance().OnConversationNext(deathCatController.playerFarming.gameObject, 6f);
    deathCatController.playerFarming._state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    deathCatController.playerFarming.simpleSpineAnimator.OnSpineEvent += new SimpleSpineAnimator.SpineEvent(deathCatController.OnSpineEvent);
    deathCatController.playerFarming.Spine.AnimationState.SetAnimation(0, "final-boss/refuse", true);
    string str = deathCatController.playerFarming.CurrentWeaponInfo.WeaponData.PrimaryEquipmentType.ToString();
    if (deathCatController.playerFarming.CurrentWeaponInfo.WeaponData.PrimaryEquipmentType == EquipmentType.Gauntlet)
      str = "Guantlets";
    deathCatController.playerFarming.Spine.AnimationState.AddAnimation(0, "final-boss/refuse-" + str, false, 0.0f);
    deathCatController.playerFarming.Spine.AnimationState.AddAnimation(0, "final-boss/idle-" + str, true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/Stings/church_bell", deathCatController.playerFarming.gameObject);
    yield return (object) new WaitForSeconds(2.66666675f);
    GameManager.GetInstance().OnConversationNext(deathCatController.playerFarming.gameObject, 5f);
    yield return (object) new WaitForSeconds(3.66666675f);
    GameManager.GetInstance().OnConversationNext(deathCatController.playerFarming.gameObject, 11f);
    yield return (object) new WaitForSeconds(0.5f);
    deathCatController.playerFarming.simpleSpineAnimator.OnSpineEvent -= new SimpleSpineAnimator.SpineEvent(deathCatController.OnSpineEvent);
    deathCatController.conversation0_C.SetPlayerInactiveOnStart = false;
    deathCatController.conversation0_C.CallOnConversationEnd = false;
    deathCatController.conversation0_C.Play(deathCatController.playerFarming.gameObject);
    yield return (object) new WaitForEndOfFrame();
    while (MMConversation.isPlaying)
      yield return (object) null;
    foreach (BaseMonoBehaviour guardian1Component in deathCatController.guardian1Components)
    {
      if ((UnityEngine.Object) guardian1Component != (UnityEngine.Object) null)
        guardian1Component.enabled = true;
    }
    deathCatController.ResetPlayerWalking();
    deathCatController.StartCoroutine((IEnumerator) deathCatController.guardian1Intro.PlayRoutine(false));
  }

  public void OnSpineEvent(string eventname)
  {
    if (!(eventname == "wings"))
      return;
    CameraManager.instance.ShakeCameraForDuration(1.2f, 1.5f, 0.5f);
    GameManager.GetInstance().OnConversationNext(this.playerFarming.gameObject, 8f);
    this.distortionObject.transform.parent = this.playerFarming.transform;
    this.distortionObject.transform.localPosition = Vector3.zero;
    this.distortionObject.SetActive(true);
    this.distortionObject.transform.DOScale(9f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      this.distortionObject.transform.localScale = Vector3.zero;
      this.distortionObject.SetActive(false);
      this.distortionObject.transform.parent = this.transform;
    }));
  }

  public void SoundTrigger1()
  {
    AudioManager.Instance.PlayOneShot("event:/music/intro/sting_mm_logo");
    HUD_Manager.Instance.Hide(false, 0);
  }

  public void SoundTrigger2()
  {
    this.InitPlayerWalking();
    AudioManager.Instance.PlayOneShot("event:/music/intro/sting_dd_logo");
  }

  public void SoundTrigger3()
  {
    AudioManager.Instance.PlayOneShot("event:/music/intro/sting_bridge");
  }

  public void InitPlayerWalking()
  {
    if (!DataManager.Instance.FinalBossSlowWalk || DungeonSandboxManager.Active || DataManager.Instance.RemovedStoryMomentsActive)
      return;
    DataManager.Instance.FinalBossSlowWalk = false;
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      PlayerFarming.SetMainPlayer(player);
      PlayerFarming.Instance.unitObject.maxSpeed = 0.03f;
      PlayerFarming.Instance.playerController.RunSpeed = 2f;
      PlayerFarming.Instance.playerController.DefaultRunSpeed = 2f;
      PlayerFarming.Instance.simpleSpineAnimator.NorthIdle = PlayerFarming.Instance.simpleSpineAnimator.GetAnimationReference("idle-up-slow");
      PlayerFarming.Instance.simpleSpineAnimator.Idle = PlayerFarming.Instance.simpleSpineAnimator.GetAnimationReference("idle-slow");
      PlayerFarming.Instance.simpleSpineAnimator.DefaultLoop = PlayerFarming.Instance.simpleSpineAnimator.GetAnimationReference("idle-slow");
      PlayerFarming.Instance.simpleSpineAnimator.ChangeStateAnimation(StateMachine.State.Idle, "idle-slow");
      PlayerFarming.Instance.simpleSpineAnimator.NorthMoving = PlayerFarming.Instance.simpleSpineAnimator.GetAnimationReference("run-up-slow");
      PlayerFarming.Instance.simpleSpineAnimator.SouthMoving = PlayerFarming.Instance.simpleSpineAnimator.GetAnimationReference("run-down-slow");
      PlayerFarming.Instance.simpleSpineAnimator.NorthDiagonalMoving = PlayerFarming.Instance.simpleSpineAnimator.GetAnimationReference("run-up-diagonal-slow");
      PlayerFarming.Instance.simpleSpineAnimator.SouthDiagonalMoving = PlayerFarming.Instance.simpleSpineAnimator.GetAnimationReference("run-slow");
      PlayerFarming.Instance.simpleSpineAnimator.ForceDirectionalMovement = true;
      PlayerFarming.Instance.simpleSpineAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run-horizontal-slow");
      PlayerFarming.Instance.simpleSpineAnimator.UpdateIdleAndMoving();
      PlayerFarming.Instance.playerWeapon.enabled = false;
      PlayerFarming.Instance.playerSpells.enabled = false;
      PlayerFarming.Instance.AllowDodging = false;
    }
  }

  public void ResetPlayerWalking()
  {
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      PlayerFarming.SetMainPlayer(player);
      PlayerFarming.Instance.unitObject.maxSpeed = 0.09f;
      PlayerFarming.Instance.playerController.RunSpeed = 5.5f;
      PlayerFarming.Instance.playerController.DefaultRunSpeed = 5.5f;
      PlayerFarming.Instance.simpleSpineAnimator.NorthIdle = PlayerFarming.Instance.simpleSpineAnimator.GetAnimationReference("idle-up");
      PlayerFarming.Instance.simpleSpineAnimator.Idle = PlayerFarming.Instance.simpleSpineAnimator.GetAnimationReference("idle");
      PlayerFarming.Instance.simpleSpineAnimator.DefaultLoop = PlayerFarming.Instance.simpleSpineAnimator.GetAnimationReference("idle");
      PlayerFarming.Instance.simpleSpineAnimator.ChangeStateAnimation(StateMachine.State.Idle, "idle");
      PlayerFarming.Instance.simpleSpineAnimator.SetDefault(StateMachine.State.Idle, "idle");
      PlayerFarming.Instance.simpleSpineAnimator.NorthMoving = PlayerFarming.Instance.simpleSpineAnimator.GetAnimationReference("run-up");
      PlayerFarming.Instance.simpleSpineAnimator.SouthMoving = PlayerFarming.Instance.simpleSpineAnimator.GetAnimationReference("run-down");
      PlayerFarming.Instance.simpleSpineAnimator.NorthDiagonalMoving = PlayerFarming.Instance.simpleSpineAnimator.GetAnimationReference("run-up-diagonal");
      PlayerFarming.Instance.simpleSpineAnimator.SouthDiagonalMoving = PlayerFarming.Instance.simpleSpineAnimator.GetAnimationReference("run");
      PlayerFarming.Instance.simpleSpineAnimator.ForceDirectionalMovement = false;
      PlayerFarming.Instance.simpleSpineAnimator.SetDefault(StateMachine.State.Moving, "run-horizontal");
      PlayerFarming.Instance.simpleSpineAnimator.ResetAnimationsToDefaults();
      PlayerFarming.Instance.simpleSpineAnimator.UpdateIdleAndMoving();
      PlayerFarming.Instance.playerWeapon.enabled = true;
      PlayerFarming.Instance.playerSpells.enabled = true;
      PlayerFarming.Instance.AllowDodging = true;
    }
  }

  public void Guardian1_OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    foreach (BaseMonoBehaviour guardian2Component in this.guardian2Components)
    {
      if ((UnityEngine.Object) guardian2Component != (UnityEngine.Object) null)
        guardian2Component.enabled = true;
    }
  }

  public void DeathCatCloneTransform()
  {
    this.StartCoroutine((IEnumerator) this.DeathCatCloneTransformIE());
  }

  public IEnumerator DeathCatCloneTransformIE()
  {
    DeathCatController deathCatController = this;
    deathCatController.conversation2.Play(deathCatController.playerFarming.gameObject);
    yield return (object) new WaitForEndOfFrame();
    while (MMConversation.isPlaying)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(deathCatController.deathCatClone.gameObject, 14f);
    MMVibrate.RumbleContinuous(0.5f, 0.75f, deathCatController.playerFarming);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.6f, float.MaxValue);
    deathCatController.deathCatBig.BaseFormSpine.AnimationState.SetAnimation(0, "break-free", false);
    deathCatController.transformParticles.startDelay = 2.6f;
    deathCatController.transformParticles.Play((bool) (UnityEngine.Object) deathCatController.playerFarming.gameObject);
    deathCatController.Chain1Spine.AnimationState.SetAnimation(0, deathCatController.breakAnimation, false);
    deathCatController.Chain2Spine.AnimationState.SetAnimation(0, deathCatController.breakAnimation, false);
    deathCatController.Chain3Spine.AnimationState.SetAnimation(0, deathCatController.breakAnimation, false);
    deathCatController.BChain1Spine.AnimationState.SetAnimation(0, deathCatController.backgroundBreak1Animation, false);
    deathCatController.BChain2Spine.AnimationState.SetAnimation(0, deathCatController.backgroundBreak2Animation, false);
    deathCatController.BChain3Spine.AnimationState.SetAnimation(0, deathCatController.backgroundBreak3Animation, false);
    deathCatController.BChain4Spine.AnimationState.SetAnimation(0, deathCatController.backgroundBreak4Animation, false);
    deathCatController.Chain1Spine.AnimationState.AddAnimation(0, deathCatController.brokenAnimation, true, 0.0f);
    deathCatController.Chain2Spine.AnimationState.AddAnimation(0, deathCatController.brokenAnimation, true, 0.0f);
    deathCatController.Chain3Spine.AnimationState.AddAnimation(0, deathCatController.brokenAnimation, true, 0.0f);
    deathCatController.BChain1Spine.AnimationState.AddAnimation(0, deathCatController.brokenAnimation, true, 0.0f);
    deathCatController.BChain2Spine.AnimationState.AddAnimation(0, deathCatController.brokenAnimation, true, 0.0f);
    deathCatController.BChain3Spine.AnimationState.AddAnimation(0, deathCatController.brokenAnimation, true, 0.0f);
    deathCatController.BChain4Spine.AnimationState.AddAnimation(0, deathCatController.brokenAnimation, true, 0.0f);
    AudioManager.Instance.SetMusicRoomID(3, "deathcat_room_id");
    yield return (object) new WaitForSeconds(3.5f);
    deathCatController.deathCatBig.BaseFormSpine.gameObject.SetActive(false);
    deathCatController.deathCatClone.gameObject.SetActive(true);
    deathCatController.transformParticles.Stop();
    deathCatController.deathCatClone.Spine.AnimationState.SetAnimation(0, "transform", false);
    deathCatController.deathCatClone.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    foreach (FollowerManager.SpawnedFollower cagedFollower in deathCatController.cagedFollowers)
    {
      FollowerManager.SpawnedFollower f = cagedFollower;
      deathCatController.StartCoroutine((IEnumerator) deathCatController.DelayCallback(UnityEngine.Random.Range(0.0f, 0.5f), (System.Action) (() =>
      {
        double num = (double) f.Follower.SetBodyAnimation("Reactions/react-scared-long", false);
        f.Follower.AddBodyAnimation("idle", true, 0.0f);
      })));
    }
    deathCatController.deathCatClone.transform.DOMove(deathCatController.deathCatClone.transform.position + Vector3.down * 3f, 3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine);
    GameManager.GetInstance().OnConversationNext(deathCatController.deathCatClone.gameObject, 20f);
    yield return (object) new WaitForSeconds(1f);
    CameraManager.instance.Stopshake();
    MMVibrate.StopRumble();
    yield return (object) new WaitForSeconds(3f);
    HUD_DisplayName.Play(ScriptLocalization.NAMES.DeathNPC, 2, HUD_DisplayName.Positions.Centre, HUD_DisplayName.textBlendMode.DungeonFinal);
    UIBossHUD.Play(deathCatController.deathCatClone.health, ScriptLocalization.NAMES.DeathNPC);
    GameManager.GetInstance().OnConversationEnd();
    GameManager.GetInstance().AddPlayersToCamera();
    GameManager.GetInstance().AddToCamera(deathCatController.deathCatClone.gameObject);
    GameManager.GetInstance().CamFollowTarget.MinZoom = 9f;
    GameManager.GetInstance().CamFollowTarget.MaxZoom = 18f;
    yield return (object) new WaitForSeconds(1f);
    deathCatController.deathCatClone.enabled = true;
  }

  public IEnumerator DelayCallback(float delay, System.Action callback)
  {
    yield return (object) new WaitForSeconds(delay);
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void StartPhase2()
  {
    this.guardian1.gameObject.SetActive(false);
    this.guardian2.gameObject.SetActive(false);
    this.deathCatClone.gameObject.SetActive(false);
    this.ResetPlayerWalking();
    PlayerFarming.Instance.transform.position = Vector3.zero;
    UnityEngine.Object.FindObjectOfType<Interaction_FinalBossAltar>().gameObject.SetActive(false);
    this.GetComponent<Collider2D>().enabled = false;
    this.DeathCatBigTransform();
  }

  public void DeathCatBigTransform()
  {
    this.StartCoroutine((IEnumerator) this.DeathCatBigTransformIE());
  }

  public IEnumerator DeathCatBigTransformIE()
  {
    DeathCatController coroutineSupport = this;
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.NoMusic);
    GameManager.GetInstance().OnConversationNext(coroutineSupport.transitionRoom.gameObject, 24f);
    foreach (GameObject torch in coroutineSupport.torches)
      torch.SetActive(false);
    foreach (GameObject eye in coroutineSupport.eyes)
      eye.SetActive(false);
    foreach (GameObject demon in Demon_Arrows.Demons)
    {
      StateMachine component = demon.GetComponent<StateMachine>();
      if ((bool) (UnityEngine.Object) component)
      {
        component.CURRENT_STATE = StateMachine.State.Idle;
        component.LockStateChanges = true;
      }
    }
    BlackSoulUpdater.Instance.Clear();
    PlayerFarming.ReloadAllFaith();
    UIBossHUD.Hide();
    foreach (PlayerFarming player in PlayerFarming.players)
      player.SetCoopIndicatorVisibility(false);
    coroutineSupport.playerFarming.Spine.gameObject.SetActive(false);
    coroutineSupport.playerFarming.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
    PlayerFarming.PositionAllPlayers(new Vector3(0.0f, 0.0f, 0.0f));
    coroutineSupport.playerFarming.LookToObject = coroutineSupport.deathCatBig.gameObject;
    coroutineSupport.whiteRoom.SetActive(false);
    coroutineSupport.transitionRoom.SetActive(true);
    for (int index = Projectile.Projectiles.Count - 1; index >= 0; --index)
      Projectile.Projectiles[index].DestroyProjectile(true);
    BiomeConstants.Instance.SetAmplifyPostEffect(false);
    coroutineSupport.deathCatBig.Spine.gameObject.SetActive(true);
    coroutineSupport.deathCatBig.BaseFormSpine.gameObject.SetActive(false);
    coroutineSupport.deathCatBig.Spine.AnimationState.SetAnimation(0, "animation", true);
    coroutineSupport.deathCatLighting.SetActive(false);
    yield return (object) new WaitForSeconds(2f);
    int i;
    for (i = 0; i < coroutineSupport.eyes.Count; i += 2)
    {
      coroutineSupport.eyes[i].SetActive(true);
      coroutineSupport.eyes[i + 1].SetActive(true);
      MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact, coroutineSupport.playerFarming, coroutineSupport: (MonoBehaviour) coroutineSupport);
      AudioManager.Instance.PlayOneShot("event:/cooking/fire_start", coroutineSupport.playerFarming.gameObject);
      if (i == 0)
        yield return (object) new WaitForSeconds(1f);
      else
        yield return (object) new WaitForSeconds(0.01f);
    }
    yield return (object) new WaitForSeconds(1f);
    for (i = 0; i < coroutineSupport.torches.Count; i += 2)
    {
      coroutineSupport.torches[i].SetActive(true);
      coroutineSupport.torches[i + 1].SetActive(true);
      MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact, coroutineSupport.playerFarming, coroutineSupport: (MonoBehaviour) coroutineSupport);
      AudioManager.Instance.PlayOneShot("event:/cooking/fire_start", coroutineSupport.playerFarming.gameObject);
      yield return (object) new WaitForSeconds(0.35f);
    }
    yield return (object) new WaitForSeconds(2f);
    MMVibrate.StopRumble();
    coroutineSupport.deathCatBig.Spine.AnimationState.SetAnimation(0, "roar", false);
    coroutineSupport.deathCatBig.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    yield return (object) new WaitForSeconds(1f);
    BiomeConstants.Instance.SetAmplifyPostEffect(true);
    MMVibrate.RumbleContinuous(1.5f, 1.75f, coroutineSupport.playerFarming);
    CameraManager.instance.ShakeCameraForDuration(1.6f, 2f, 2.5f);
    GameManager.GetInstance().OnConversationNext(coroutineSupport.deathCatBig.CinematicBone, 18f);
    coroutineSupport.playerFarming.Spine.gameObject.SetActive(true);
    foreach (PlayerFarming player in PlayerFarming.players)
      player.SetCoopIndicatorVisibility(true);
    coroutineSupport.redRoom.SetActive(true);
    coroutineSupport.blackFade.DOFade(0.0f, 0.5f);
    AudioManager.Instance.PlayMusic("event:/music/death_cat_battle/death_cat_battle");
    AudioManager.Instance.SetMusicRoomID(4, "deathcat_room_id");
    for (int index = 0; index < coroutineSupport.eyes.Count; ++index)
      coroutineSupport.eyes[index].SetActive(false);
    coroutineSupport.deathCatLighting.SetActive(true);
    if (FollowerBrain.AllBrains.Count > 0 || DungeonSandboxManager.Active)
    {
      yield return (object) new WaitForSeconds(2.5f);
      MMVibrate.StopRumble();
      yield return (object) new WaitForSeconds(1.5f);
      coroutineSupport.conversation4.Play(coroutineSupport.playerFarming.gameObject);
      yield return (object) new WaitForEndOfFrame();
      while (MMConversation.isPlaying)
        yield return (object) null;
      yield return (object) new WaitForEndOfFrame();
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(coroutineSupport.transitionRoom.gameObject, 22f);
      coroutineSupport.deathCatBig.Spine.AnimationState.SetAnimation(0, "summon-followers", false);
      coroutineSupport.deathCatBig.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
      int count = coroutineSupport.cagedFollowers.Count;
      MMVibrate.RumbleContinuous(0.35f, 0.5f, coroutineSupport.playerFarming);
      CameraManager.instance.ShakeCameraForDuration(0.3f, 0.6f, 5f);
      for (int index = 0; index < coroutineSupport.followerChains.Length; ++index)
      {
        if (index < count)
          coroutineSupport.StartCoroutine((IEnumerator) coroutineSupport.\u003CDeathCatBigTransformIE\u003Eg__SpawnChainedFollower\u007C90_3(index, UnityEngine.Random.Range(0.0f, 1f)));
      }
      yield return (object) new WaitForSeconds(5f);
      MMVibrate.StopRumble();
      foreach (FollowerManager.SpawnedFollower chainedFollower in coroutineSupport.chainedFollowers)
      {
        if (coroutineSupport.cagedFollowers.Contains(chainedFollower))
          coroutineSupport.cagedFollowers.Remove(chainedFollower);
      }
    }
    GameManager.GetInstance().OnConversationNext(coroutineSupport.deathCatBig.CinematicBone, 14f);
    yield return (object) new WaitForSeconds(2f);
    AudioManager.Instance.PlayOneShot("event:/boss/deathcat/grunt", coroutineSupport.gameObject);
    for (int index = 0; index < 3; ++index)
    {
      if (index == 1)
      {
        GameManager.GetInstance().AddToCamera(coroutineSupport.eyesManager.Eyes[index].gameObject);
        GameManager.GetInstance().CameraSetOffset(Vector3.down * 3f);
      }
      coroutineSupport.eyesManager.Eyes[index].transform.position = coroutineSupport.eyeBones[index].transform.position;
    }
    for (i = 0; i < 3; ++i)
    {
      coroutineSupport.eyesManager.Eyes[i].gameObject.SetActive(true);
      coroutineSupport.eyesManager.Eyes[i].PopParticle.SetActive(true);
      coroutineSupport.eyesManager.Eyes[i].transform.localScale = Vector3.one * 0.6f;
      Vector3 vector3 = new Vector3((float) ((i - 1) * -3), -5f, i == 1 ? -2f : -1f);
      coroutineSupport.eyesManager.Eyes[i].transform.DOMove(coroutineSupport.eyesManager.Eyes[i].transform.position + vector3, 3.5f - (float) i).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine);
      coroutineSupport.eyesManager.Eyes[i].transform.DOScale(0.8f, 3.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine);
      CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
      coroutineSupport.deathCatBig.Spine.Skeleton.SetSkin((i + 1).ToString());
      AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/teleport_away", coroutineSupport.gameObject);
      if (i != 2)
        yield return (object) new WaitForSeconds(1f);
    }
    yield return (object) new WaitForSeconds(2.5f);
    coroutineSupport.eyesManager.Eyes[0].transform.DOMove(new Vector3(coroutineSupport.eyesManager.Eyes[0].transform.position.x, coroutineSupport.eyesManager.Eyes[0].transform.position.y, 2f), 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(coroutineSupport.\u003CDeathCatBigTransformIE\u003Eb__90_0));
    coroutineSupport.eyesManager.Eyes[2].transform.DOMove(new Vector3(coroutineSupport.eyesManager.Eyes[2].transform.position.x, coroutineSupport.eyesManager.Eyes[2].transform.position.y, 2f), 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(coroutineSupport.\u003CDeathCatBigTransformIE\u003Eb__90_1));
    yield return (object) new WaitForSeconds(0.5f);
    coroutineSupport.eyesManager.Eyes[1].transform.DOMove(new Vector3(coroutineSupport.eyesManager.Eyes[1].transform.position.x, coroutineSupport.eyesManager.Eyes[1].transform.position.y, 2f), 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(coroutineSupport.\u003CDeathCatBigTransformIE\u003Eb__90_2));
    yield return (object) new WaitForSeconds(1.5f);
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    GameManager.GetInstance().RemoveAllFromCamera();
    GameManager.GetInstance().AddPlayerToCamera();
    GameManager.GetInstance().OnConversationEnd();
    GameManager.SetGlobalOcclusionActive(true);
    yield return (object) new WaitForEndOfFrame();
    GameManager.GetInstance().AddPlayersToCamera();
    GameManager.GetInstance().AddToCamera(coroutineSupport.deathCatBig.gameObject);
    GameManager.GetInstance().CamFollowTarget.MinZoom = 9f;
    GameManager.GetInstance().CamFollowTarget.MaxZoom = 18f;
    UIBossHUD.Play(coroutineSupport.deathCatBig.health, ScriptLocalization.NAMES.DeathNPC);
    while ((UnityEngine.Object) UIBossHUD.Instance == (UnityEngine.Object) null)
      yield return (object) new WaitForEndOfFrame();
    UIBossHUD.Instance.ForceHealthAmount(0.0f);
    coroutineSupport.timeBetweenFervourDrop = GameManager.GetInstance().CurrentTime + UnityEngine.Random.Range(coroutineSupport.fervourTimeDropInterval.x, coroutineSupport.fervourTimeDropInterval.y);
    Health component1 = coroutineSupport.deathCatBig.GetComponent<Health>();
    component1.enabled = true;
    component1.HP = component1.totalHP;
    coroutineSupport.deathCatBig.enabled = true;
    foreach (GameObject demon in Demon_Arrows.Demons)
    {
      StateMachine component2 = demon.GetComponent<StateMachine>();
      if ((bool) (UnityEngine.Object) component2)
        component2.LockStateChanges = false;
    }
    float time = 0.0f;
    while ((double) time < 2.0)
    {
      float normHealthAmount = time / 2f;
      UIBossHUD.Instance?.ForceHealthAmount(normHealthAmount);
      time += Time.deltaTime;
      yield return (object) null;
    }
  }

  public void DeathCatKilled() => this.StartCoroutine((IEnumerator) this.DeathCatKilledIE());

  public IEnumerator DeathCatKilledIE()
  {
    DeathCatController deathCatController = this;
    HUD_Manager.Instance.Hide(false);
    deathCatController.playerFarming.GoToAndStop(new Vector3(0.0f, 2f, 0.0f), deathCatController.gameObject);
    AudioManager.Instance.SetMusicRoomID(0, "deathcat_room_id");
    if (!DungeonSandboxManager.Active)
    {
      GameObject Follower = UnityEngine.Object.Instantiate<GameObject>(deathCatController.followerToSpawn, new Vector3(0.0f, 8f, 0.0f), Quaternion.identity);
      deathCatController.followerSpawn = Follower.GetComponent<Interaction_FollowerSpawn>();
      deathCatController.followerSpawn.Play("Boss Death Cat", ScriptLocalization.NAMES.DeathNPC);
      deathCatController.followerSpawn.AutomaticallyInteract = false;
      deathCatController.followerSpawn.Interactable = false;
      deathCatController.followerSpawn.DisableOnHighlighted = true;
      deathCatController.followerSpawn.EndIndicateHighlighted(deathCatController.playerFarming);
      DataManager.SetFollowerSkinUnlocked("Boss Death Cat");
      while (LetterBox.IsPlaying)
        yield return (object) null;
      GameManager.GetInstance().CameraSetOffset(Vector3.zero);
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(Follower.gameObject, 4f);
      foreach (ConversationEntry entry in deathCatController.conversation5.Entries)
      {
        entry.Speaker = deathCatController.followerSpawn.Spine.gameObject;
        entry.SkeletonData = deathCatController.followerSpawn.Spine;
        entry.Animation = "unconverted-talk";
        entry.pitchValue = deathCatController.followerSpawn._followerInfo.follower_pitch;
        entry.vibratoValue = deathCatController.followerSpawn._followerInfo.follower_vibrato;
        entry.followerID = deathCatController.followerSpawn._followerInfo.ID;
      }
      yield return (object) new WaitForEndOfFrame();
      deathCatController.conversation5.Play(deathCatController.playerFarming.gameObject);
      yield return (object) new WaitForEndOfFrame();
      GameManager.GetInstance().CamFollowTarget.TargetOffset = new Vector3(0.0f, 0.0f, 0.3f);
      while (LetterBox.IsPlaying)
        yield return (object) null;
      yield return (object) new WaitForEndOfFrame();
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(Follower.gameObject, 4f);
      GameManager.GetInstance().CamFollowTarget.TargetOffset = new Vector3(0.0f, 0.0f, 0.3f);
      bool convert = false;
      foreach (PlayerFarming player in PlayerFarming.players)
      {
        if ((UnityEngine.Object) player != (UnityEngine.Object) deathCatController.playerFarming)
        {
          if ((double) Vector3.Distance(player.transform.position, deathCatController.playerFarming.transform.position) > 7.5)
            player.transform.position = deathCatController.playerFarming.transform.position + Vector3.right;
          else
            player.GoToAndStop(deathCatController.playerFarming.transform.position + Vector3.right);
        }
      }
      GameObject g = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/UI/Choice Indicator"), GameObject.FindWithTag("Canvas").transform) as GameObject;
      ChoiceIndicator choice = g.GetComponent<ChoiceIndicator>();
      choice.Offset = new Vector3(0.0f, -350f);
      choice.Show("Conversation_NPC/DeathCatBossFight/Dead/Spare", "FollowerInteractions/Murder", (System.Action) (() => convert = true), (System.Action) (() => convert = false), Follower.transform.position);
      while ((UnityEngine.Object) g != (UnityEngine.Object) null)
      {
        choice.UpdatePosition(Follower.transform.position);
        yield return (object) null;
      }
      UIManager.PlayAudio("event:/ui/heretics_defeated");
      Interaction_SimpleConversation simpleConversation = !convert ? deathCatController.conversation6_B : deathCatController.conversation6_A;
      simpleConversation.Entries[0].Speaker = deathCatController.followerSpawn.Spine.gameObject;
      simpleConversation.Entries[0].SkeletonData = deathCatController.followerSpawn.Spine;
      simpleConversation.Entries[0].Animation = "unconverted-talk";
      simpleConversation.Entries[0].pitchValue = deathCatController.followerSpawn._followerInfo.follower_pitch;
      simpleConversation.Entries[0].vibratoValue = deathCatController.followerSpawn._followerInfo.follower_vibrato;
      simpleConversation.Entries[0].followerID = deathCatController.followerSpawn._followerInfo.ID;
      simpleConversation.Play(deathCatController.playerFarming.gameObject);
      yield return (object) new WaitForEndOfFrame();
      GameManager.GetInstance().CamFollowTarget.TargetOffset = new Vector3(0.0f, 0.0f, 0.3f);
      while (MMConversation.isPlaying)
        yield return (object) null;
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(deathCatController.followerSpawn.gameObject, 6f);
      Vector3 TargetPosition = Follower.transform.position + Vector3.right * 1.5f;
      deathCatController.playerFarming.GoToAndStop(TargetPosition, deathCatController.followerSpawn.gameObject);
      while (PlayerFarming.Instance.GoToAndStopping)
        yield return (object) null;
      deathCatController.followerSpawn.Spine.GetComponent<SimpleSpineAnimator>().enabled = false;
      if (convert)
        yield return (object) deathCatController.StartCoroutine((IEnumerator) deathCatController.followerSpawn.ConvertFollower());
      else
        yield return (object) deathCatController.StartCoroutine((IEnumerator) deathCatController.MurderDeathCat());
      Follower = (GameObject) null;
      g = (GameObject) null;
      choice = (ChoiceIndicator) null;
    }
    else
      GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(deathCatController.playerFarming.gameObject);
    bool waiting = true;
    deathCatController.playerFarming.GoToAndStop(Vector3.zero, IdleOnEnd: true, GoToCallback: (System.Action) (() =>
    {
      waiting = false;
      this.playerFarming.state.facingAngle = 270f;
      this.playerFarming.state.LookAngle = 270f;
    }), groupAction: true);
    yield return (object) new WaitForEndOfFrame();
    while (waiting)
      yield return (object) null;
    PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive);
    deathCatController.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    AstarPath.active = (AstarPath) null;
    for (int index = 0; index < deathCatController.chainedFollowers.Count; ++index)
      GameManager.GetInstance().AddToCamera(deathCatController.chainedFollowers[index].Follower.gameObject);
    yield return (object) new WaitForSeconds(2f);
    for (int i = 0; i < deathCatController.chainedFollowers.Count; ++i)
    {
      deathCatController.StartCoroutine((IEnumerator) deathCatController.DropFollower(deathCatController.chainedFollowers[i].Follower, i));
      yield return (object) new WaitForSeconds(0.1f);
    }
    yield return (object) new WaitForSeconds(2f);
    yield return (object) new WaitForSeconds(4f);
    DataManager.Instance.DiedLastRun = false;
    DataManager.Instance.LastRunResults = UIDeathScreenOverlayController.Results.None;
    for (int index = 0; index < DataManager.Instance.Followers_Demons_IDs.Count; ++index)
    {
      FollowerInfo infoById = FollowerInfo.GetInfoByID(DataManager.Instance.Followers_Demons_IDs[index]);
      if (infoById != null)
        FollowerBrain.GetOrCreateBrain(infoById)?.AddThought(Thought.DemonSuccessfulRun);
    }
    DataManager.Instance.Followers_Demons_IDs.Clear();
    DataManager.Instance.Followers_Demons_Types.Clear();
    deathCatController.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    deathCatController.playerFarming.Spine.AnimationState.SetAnimation(0, "sermons/sermon-start-nobook", false);
    deathCatController.playerFarming.Spine.AnimationState.AddAnimation(0, "sermons/sermon-loop-nobook", true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/Stings/white_eyes", deathCatController.playerFarming.transform.position);
    AudioManager.Instance.PlayOneShot("event:/player/standard_jump_spin_float", deathCatController.playerFarming.transform.position);
    if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
    {
      if (!PlayerFarming.Instance.isLamb || PlayerFarming.Instance.IsGoat)
        AudioManager.Instance.PlayOneShot("event:/player/goat_player/goat_speak_to_follower_noBookPage");
      else
        AudioManager.Instance.PlayOneShot("event:/player/speak_to_follower_noBookPage");
    }
    MMVibrate.Haptic(MMVibrate.HapticTypes.Success);
    DOTween.To((DOGetter<float>) (() => GameManager.GetInstance().CamFollowTarget.MaxZoom), (DOSetter<float>) (x => GameManager.GetInstance().CamFollowTarget.MaxZoom = x), 3f, 10f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InOutSine);
    DOTween.To((DOGetter<float>) (() => GameManager.GetInstance().CamFollowTarget.MinZoom), (DOSetter<float>) (x => GameManager.GetInstance().CamFollowTarget.MinZoom = x), 1f, 10f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InOutSine);
    DOTween.To((DOGetter<Vector3>) (() => GameManager.GetInstance().CamFollowTarget.TargetOffset), (DOSetter<Vector3>) (x => GameManager.GetInstance().CamFollowTarget.TargetOffset = x), Vector3.forward * -1.2f, 10f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine);
    yield return (object) new WaitForSeconds(10f);
    SimulationManager.UnPause();
    System.Action action = (System.Action) (() => MMTransition.Play(MMTransition.TransitionType.ChangeSceneAutoResume, MMTransition.Effect.BlackFade, "Credits", 0.5f, "", new System.Action(this.Save)));
    if (DungeonSandboxManager.Active)
    {
      Inventory.AddItem(InventoryItem.ITEM_TYPE.GOD_TEAR, 1);
      UIDeathScreenOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowDeathScreenOverlay(UIDeathScreenOverlayController.Results.Completed);
      overlayController.Show();
      overlayController.OnShown = overlayController.OnShown + (System.Action) (() =>
      {
        foreach (FollowerManager.SpawnedFollower chainedFollower in this.chainedFollowers)
          chainedFollower.Follower.gameObject.SetActive(false);
      });
    }
    else
    {
      QuoteScreenController.Init(new List<QuoteScreenController.QuoteTypes>()
      {
        QuoteScreenController.QuoteTypes.QuoteBoss5
      }, action, action);
      MMTransition.Play(MMTransition.TransitionType.ChangeSceneAutoResume, MMTransition.Effect.BlackFade, "QuoteScreen", 5f, "", (System.Action) (() => Time.timeScale = 1f));
    }
  }

  public void Save()
  {
    DataManager.ResetRunData();
    SaveAndLoad.Save();
  }

  public IEnumerator MurderDeathCat()
  {
    this.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) null;
    this.playerFarming.Spine.AnimationState.SetAnimation(0, "murder", false);
    this.playerFarming.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
    {
      if (!PlayerFarming.Instance.isLamb || PlayerFarming.Instance.IsGoat)
        AudioManager.Instance.PlayOneShot("event:/player/goat_player/goat_speak_to_follower_noBookPage");
      else
        AudioManager.Instance.PlayOneShot("event:/player/speak_to_follower_noBookPage");
    }
    AudioManager.Instance.PlayOneShot("event:/player/murder_follower_sequence");
    this.followerSpawn.Spine.AnimationState.SetAnimation(1, "murder", false);
    float Duration = this.followerSpawn.Spine.AnimationState.GetCurrent(1).Animation.Duration;
    GameManager.GetInstance().AddToCamera(this.followerSpawn.gameObject);
    yield return (object) new WaitForSeconds(0.1f);
    this.followerSpawn.Spine.CustomMaterialOverride.Clear();
    this.followerSpawn.Spine.CustomMaterialOverride.Add(this.followerSpawn.NormalMaterial, this.followerSpawn.BW_Material);
    this.playerFarming.Spine.CustomMaterialOverride.Clear();
    this.playerFarming.Spine.CustomMaterialOverride.Add(this.playerFarming.originalMaterial, this.playerFarming.BW_Material);
    HUD_Manager.Instance.ShowBW(0.33f, 0.0f, 1f);
    BiomeConstants.Instance.EmitSmokeInteractionVFX(this.followerSpawn.transform.position, new Vector3(0.5f, 0.5f, 1f));
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    yield return (object) new WaitForSeconds(1.6f);
    BiomeConstants.Instance.EmitSmokeInteractionVFX(this.followerSpawn.transform.position, new Vector3(1f, 1f, 1f));
    CameraManager.instance.ShakeCameraForDuration(0.8f, 1f, 0.3f);
    this.playerFarming.Spine.CustomMaterialOverride.Clear();
    this.followerSpawn.Spine.CustomMaterialOverride.Clear();
    HUD_Manager.Instance.ShowBW(0.33f, 1f, 0.0f);
    yield return (object) new WaitForSeconds((float) ((double) Duration - 0.10000000149011612 - 1.7000000476837158));
    this.followerSpawn.Interactable = false;
    bool Waiting = true;
    DecorationCustomTarget.Create(this.followerSpawn.transform.position, this.playerFarming.transform.position, 0.5f, StructureBrain.TYPES.DECORATION_BOSS_TROPHY_5, (System.Action) (() => Waiting = false));
    while (Waiting)
      yield return (object) null;
  }

  public IEnumerator DoSlowMo()
  {
    yield return (object) new WaitForSeconds(1.33333337f);
    GameManager.SetTimeScale(0.2f);
    GameManager.GetInstance().CameraSetZoom(6f);
    yield return (object) new WaitForSeconds(0.933333337f);
    GameManager.SetTimeScale(1f);
    GameManager.GetInstance().CameraResetTargetZoom();
  }

  public IEnumerator DropFollower(Follower follower, int index)
  {
    double num1 = (double) follower.SetBodyAnimation("FinalBoss/freed", false);
    yield return (object) new WaitForSeconds(0.5f);
    follower.transform.DOMove(new Vector3(follower.transform.position.x, follower.transform.position.y, 0.0f), 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine);
    yield return (object) new WaitForSeconds(0.5f);
    follower.transform.localScale = Vector3.one;
    follower.State.facingAngle = (double) this.playerFarming.transform.position.x > (double) follower.transform.position.x ? 0.0f : 180f;
    follower.State.LookAngle = follower.State.facingAngle;
    yield return (object) new WaitForSeconds(0.166666672f);
    follower.Brain.CurrentState = (FollowerState) new FollowerState_Default();
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run-cheering");
    bool waiting = true;
    follower.GoTo(this.GetFollowerPosition(index), (System.Action) (() =>
    {
      follower.State.facingAngle = (double) this.playerFarming.transform.position.x > (double) follower.transform.position.x ? 0.0f : 180f;
      follower.State.LookAngle = follower.State.facingAngle;
      waiting = false;
      ++DeathCatController.count;
    }));
    while (waiting || DeathCatController.count < this.chainedFollowers.Count - 1)
      yield return (object) null;
    double num2 = (double) follower.SetBodyAnimation("devotion/devotion-start", false);
    follower.AddBodyAnimation("devotion/devotion-collect-loopstart-whiteyes", false, 0.0f);
    follower.AddBodyAnimation("devotion/devotion-collect-loop-whiteyes", true, 0.0f);
  }

  public Vector3 GetFollowerPosition(int index)
  {
    if (this.chainedFollowers.Count <= 12)
    {
      float num = 2f;
      float f = (float) ((double) index * (360.0 / (double) this.chainedFollowers.Count) * (Math.PI / 180.0));
      return this.playerFarming.transform.position + new Vector3(num * Mathf.Cos(f), num * Mathf.Sin(f));
    }
    int b = 8;
    float num1;
    float f1;
    if (index < b)
    {
      num1 = 2f;
      f1 = (float) ((double) index * (360.0 / (double) Mathf.Min(this.chainedFollowers.Count, b)) * (Math.PI / 180.0));
    }
    else
    {
      num1 = 3f;
      f1 = (float) ((double) (index - b) * (360.0 / (double) (this.chainedFollowers.Count - b)) * (Math.PI / 180.0));
    }
    return this.playerFarming.transform.position + new Vector3(num1 * Mathf.Cos(f1), num1 * Mathf.Sin(f1));
  }

  public void OnLanguageChanged()
  {
    UIBossHUD.Instance?.UpdateName(ScriptLocalization.NAMES.DeathNPC);
  }

  [CompilerGenerated]
  public void \u003CIKneelCallback\u003Eb__73_3()
  {
    foreach (FollowerManager.SpawnedFollower cagedFollower in this.cagedFollowers)
      cagedFollower.Follower.gameObject.SetActive(false);
  }

  [CompilerGenerated]
  public void \u003COnSpineEvent\u003Eb__76_0()
  {
    this.distortionObject.transform.localScale = Vector3.zero;
    this.distortionObject.SetActive(false);
    this.distortionObject.transform.parent = this.transform;
  }

  [CompilerGenerated]
  public IEnumerator \u003CDeathCatBigTransformIE\u003Eg__SpawnChainedFollower\u007C90_3(
    int index,
    float delay)
  {
    yield return (object) new WaitForSeconds(delay);
    this.followerChains[index].Init(this.cagedFollowers[index]);
    this.chainedFollowers.Add(this.cagedFollowers[index]);
  }

  [CompilerGenerated]
  public void \u003CDeathCatBigTransformIE\u003Eb__90_0()
  {
    this.eyesManager.Eyes[0].SplashParticle.gameObject.SetActive(true);
    this.eyesManager.Eyes[0].enabled = true;
    this.eyesManager.Eyes[0].spawnTrails = true;
    this.eyesManager.Eyes[0].Spine.gameObject.SetActive(false);
    AudioManager.Instance.PlayOneShot("event:/boss/frog/tongue_impact", this.gameObject);
  }

  [CompilerGenerated]
  public void \u003CDeathCatBigTransformIE\u003Eb__90_1()
  {
    this.eyesManager.Eyes[2].SplashParticle.gameObject.SetActive(true);
    this.eyesManager.Eyes[2].enabled = true;
    this.eyesManager.Eyes[2].spawnTrails = true;
    this.eyesManager.Eyes[2].Spine.gameObject.SetActive(false);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    AudioManager.Instance.PlayOneShot("event:/boss/frog/tongue_impact", this.gameObject);
  }

  [CompilerGenerated]
  public void \u003CDeathCatBigTransformIE\u003Eb__90_2()
  {
    this.eyesManager.Eyes[1].SplashParticle.gameObject.SetActive(true);
    this.eyesManager.Eyes[1].enabled = true;
    this.eyesManager.Eyes[1].spawnTrails = true;
    this.eyesManager.Eyes[1].Spine.gameObject.SetActive(false);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    AudioManager.Instance.PlayOneShot("event:/boss/frog/tongue_impact", this.gameObject);
  }
}
