// Decompiled with JetBrains decompiler
// Type: JellyBossIntroRitual
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using MMTools;
using Spine;
using Spine.Unity;
using src.UINavigator;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class JellyBossIntroRitual : BaseMonoBehaviour
{
  [SerializeField]
  public GameObject cultLeader;
  [SerializeField]
  public GameObject cameraTarget;
  [SerializeField]
  public SkeletonAnimation cultLeaderSpine;
  [SerializeField]
  public Renderer[] blood;
  [SerializeField]
  public GameObject bloodParticle;
  [SerializeField]
  public Renderer symbols;
  [SerializeField]
  public AnimationCurve absorbSoulCurve;
  [SerializeField]
  public Texture2D lutTexture;
  [SerializeField]
  public Transform distortionObject;
  [SerializeField]
  public GameObject transformParticles;
  [Space]
  [SerializeField]
  public DeadBodySliding enemyBody;
  [SerializeField]
  public GameObject deathParticlePrefab;
  [SerializeField]
  public GameObject[] bloodSprays;
  [SerializeField]
  public SkeletonAnimation[] enemySpines;
  [SerializeField]
  public SkeletonAnimation[] surroundingSpines;
  public Interaction_WeaponSelectionPodium RelicPodium;
  public SkeletonGraphic SermonOverlay;
  [Space]
  [SerializeField]
  public MiniBossController jellyBoss;
  [SerializeField]
  public Health jellyBossHealth;
  [SerializeField]
  public GameObject[] environmentTraps;
  public LongGrass[] surroundingGrass;
  public GameObject BloodPortalEffect;
  [SerializeField]
  public GameObject healEffect;
  [SerializeField]
  public SkeletonAnimation healEffectSpine;
  public Texture originalLut;
  public Texture originalHighlightLut;
  public int camZoom = 10;
  public bool triggered;
  public PlayerFarming playerBehind;
  public Vector3 playerBehindTargetPosition;
  public Color bloodColor = new Color(0.47f, 0.11f, 0.11f, 1f);
  public bool skippable;
  public bool skipped;
  public List<Tween> tweens = new List<Tween>();
  public Camera mainCamera;
  public AmplifyColorEffect amplifyColorEffect;
  public bool healingKallamar;
  public GameObject offsetObject;

  public string term1
  {
    get
    {
      return GameManager.Layer2 ? "Conversation_NPC/Story/Dungeon3/Leader1/Boss1_Layer2" : "Conversation_NPC/Story/Dungeon1/Leader3/Boss1";
    }
  }

  public void Awake()
  {
    foreach (Renderer renderer in this.blood)
      renderer.material.SetFloat("_BlotchMultiply", 0.0f);
    this.symbols.material.SetFloat("_BlotchMultiply", 0.0f);
    foreach (GameObject bloodSpray in this.bloodSprays)
      bloodSpray.SetActive(false);
    this.surroundingGrass = this.transform.parent.GetComponentsInChildren<LongGrass>();
    this.mainCamera = Camera.main;
    this.amplifyColorEffect = this.mainCamera.GetComponent<AmplifyColorEffect>();
    this.cultLeaderSpine.Skeleton.SetSkin(GameManager.Layer2 ? "Beaten" : "Normal");
    this.jellyBossHealth.untouchable = true;
    foreach (GameObject environmentTrap in this.environmentTraps)
      environmentTrap.SetActive(GameManager.Layer2);
  }

  public void Update()
  {
    if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null) || this.healingKallamar || this.skipped || !this.skippable || MonoSingleton<UIManager>.Instance.MenusBlocked || !InputManager.Gameplay.GetAttackButtonDown() && !DungeonSandboxManager.Active && !DataManager.Instance.RemovedStoryMomentsActive)
      return;
    this.StopAllCoroutines();
    this.gameObject.SetActive(false);
    CameraManager.instance.Stopshake();
    foreach (Tween tween in this.tweens)
      tween.Kill();
    AmplifyColorEffect amplifyColorEffect = this.amplifyColorEffect;
    amplifyColorEffect.LutHighlightTexture = this.originalHighlightLut;
    amplifyColorEffect.LutTexture = this.originalLut;
    MMConversation.mmConversation?.Close();
    LetterBox.Instance.HideSkipPrompt();
    this.cultLeaderSpine.AnimationState.SetAnimation(0, "transform", false).TrackTime = 10.2f;
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/roar", this.gameObject);
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.IntroDone());
    this.skipped = true;
    this.jellyBossHealth.untouchable = false;
    this.jellyBoss.EnemiesToTrack[0].enabled = true;
  }

  public void Start()
  {
    this.healingKallamar = DataManager.Instance.HealingKallamarQuestActive;
    if (DungeonSandboxManager.Active || DataManager.Instance.RemovedStoryMomentsActive || this.healingKallamar)
    {
      this.cultLeaderSpine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
      foreach (Component enemySpine in this.enemySpines)
        enemySpine.gameObject.SetActive(false);
      foreach (Component surroundingSpine in this.surroundingSpines)
        surroundingSpine.gameObject.SetActive(false);
      this.jellyBoss.EnemiesToTrack[0].enabled = false;
      if (this.healingKallamar)
        this.jellyBoss.EnemiesToTrack[0].gameObject.SetActive(false);
    }
    else
      this.cultLeaderSpine.AnimationState.SetAnimation(0, "leader/idle", true);
    this.StartCoroutine((IEnumerator) this.CompleteHealingBishopRoom());
  }

  public void OnTriggerEnter2D(Collider2D collision)
  {
    if (this.triggered || this.skipped || !collision.CompareTag("Player") || MMTransition.IsPlaying)
      return;
    PlayerFarming componentInParent = collision.GetComponentInParent<PlayerFarming>();
    MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = componentInParent;
    if (this.healingKallamar)
    {
      foreach (PlayerFarming player in PlayerFarming.players)
      {
        player.GoToAndStop(Vector3.down * 3f, DisableCollider: true);
        if ((UnityEngine.Object) player != (UnityEngine.Object) componentInParent)
        {
          this.playerBehind = player;
          this.playerBehindTargetPosition = Vector3.down * 6f;
        }
      }
      this.StartCoroutine((IEnumerator) this.HealingIE());
    }
    else
    {
      foreach (PlayerFarming player in PlayerFarming.players)
      {
        if ((UnityEngine.Object) player != (UnityEngine.Object) componentInParent)
        {
          this.playerBehind = player;
          player.GoToAndStop(collision.transform.position, DisableCollider: true);
          this.playerBehindTargetPosition = collision.transform.position + Vector3.right * 1.5f;
        }
      }
      this.StartCoroutine((IEnumerator) this.RitualRoutine());
    }
  }

  public IEnumerator RitualRoutine()
  {
    JellyBossIntroRitual jellyBossIntroRitual = this;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.InActive;
    if ((bool) (UnityEngine.Object) MMConversation.mmConversation)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) MMConversation.mmConversation.gameObject);
      MMConversation.ClearConversation();
    }
    SimulationManager.Pause();
    HUD_Manager.Instance.HideTopRight();
    jellyBossIntroRitual.originalHighlightLut = jellyBossIntroRitual.amplifyColorEffect.LutHighlightTexture;
    jellyBossIntroRitual.originalLut = jellyBossIntroRitual.amplifyColorEffect.LutTexture;
    jellyBossIntroRitual.amplifyColorEffect.LutHighlightBlendTexture = (Texture) jellyBossIntroRitual.lutTexture;
    jellyBossIntroRitual.triggered = true;
    AudioManager.Instance.PlayOneShot("event:/boss/frog/transition_intro_zoom");
    if (DungeonSandboxManager.Active || DataManager.Instance.RemovedStoryMomentsActive)
    {
      jellyBossIntroRitual.skippable = true;
    }
    else
    {
      List<ConversationEntry> Entries = new List<ConversationEntry>()
      {
        new ConversationEntry(jellyBossIntroRitual.offsetObject, jellyBossIntroRitual.term1),
        new ConversationEntry(jellyBossIntroRitual.offsetObject, "Conversation_NPC/Story/Dungeon1/Leader3/Boss2")
      };
      Entries[0].CharacterName = "NAMES/CultLeaders/Dungeon3";
      Entries[0].Offset = new Vector3(0.0f, 2f, 0.0f);
      Entries[0].Animation = "leader/talk";
      Entries[0].SkeletonData = jellyBossIntroRitual.cultLeaderSpine;
      Entries[1].CharacterName = "NAMES/CultLeaders/Dungeon3";
      Entries[1].Offset = new Vector3(0.0f, 2f, 0.0f);
      Entries[1].Animation = "leader/talk";
      Entries[1].SkeletonData = jellyBossIntroRitual.cultLeaderSpine;
      if (GameManager.Layer2)
        Entries.RemoveAt(1);
      MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) null), false, SetPlayerIdleOnComplete: false, showControlPrompt: false);
      MMConversation.mmConversation.SpeechBubble.ScreenOffset = 350f;
      yield return (object) new WaitForSeconds(1f);
      jellyBossIntroRitual.skippable = DataManager.Instance.BossesEncountered.Contains(PlayerFarming.Location);
      if (jellyBossIntroRitual.skippable && !jellyBossIntroRitual.skipped)
        LetterBox.Instance.ShowSkipPrompt();
      while (MMConversation.CURRENT_CONVERSATION != null)
        yield return (object) null;
      UnityEngine.Object.Destroy((UnityEngine.Object) jellyBossIntroRitual.offsetObject);
      yield return (object) new WaitForEndOfFrame();
      AudioManager.Instance.PlayOneShot("event:/boss/frog/transition_zoom_back");
      AudioManager.Instance.PlayOneShot("event:/boss/frog/after_intro_grunt");
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(jellyBossIntroRitual.cameraTarget, 12f);
      yield return (object) new WaitForSeconds(1f);
      AudioManager.Instance.PlayOneShot("event:/boss/frog/cultist_sequence");
      foreach (Component enemySpine in jellyBossIntroRitual.enemySpines)
        enemySpine.GetComponentInChildren<WorshipperBubble>(true).Play(WorshipperBubble.SPEECH_TYPE.BOSSCROWN3, 4.5f, UnityEngine.Random.Range(0.0f, 0.3f));
      foreach (SkeletonAnimation enemySpine in jellyBossIntroRitual.enemySpines)
        enemySpine.AnimationState.SetAnimation(0, "dance", true).TrackTime = UnityEngine.Random.Range(0.0f, 0.3f);
      yield return (object) new WaitForSeconds(3f);
      foreach (SkeletonAnimation enemySpine in jellyBossIntroRitual.enemySpines)
        enemySpine.AnimationState.SetAnimation(0, "ritual/sacrifice", false).TrackTime = UnityEngine.Random.Range(0.0f, 0.3f);
      yield return (object) new WaitForSeconds(1f);
      jellyBossIntroRitual.transformParticles.SetActive(true);
      jellyBossIntroRitual.cultLeaderSpine.AnimationState.SetAnimation(0, "transform", true);
      AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/transform", jellyBossIntroRitual.gameObject);
      yield return (object) new WaitForSeconds(0.3f);
      foreach (SkeletonAnimation surroundingSpine in jellyBossIntroRitual.surroundingSpines)
        surroundingSpine.AnimationState.SetAnimation(0, "worship", true).TrackTime = UnityEngine.Random.Range(0.0f, 0.4f);
      foreach (GameObject bloodSpray in jellyBossIntroRitual.bloodSprays)
      {
        bloodSpray.SetActive(true);
        AudioManager.Instance.PlayOneShot("event:/enemy/impact_squishy", bloodSpray.gameObject);
        float z = Vector3.Angle(bloodSpray.transform.position, jellyBossIntroRitual.cultLeader.transform.position);
        BiomeConstants.Instance.EmitBloodSplatter(bloodSpray.transform.position, new Vector3(0.0f, 0.0f, z), Color.black);
      }
      foreach (Renderer renderer in jellyBossIntroRitual.blood)
        renderer.material.DOFloat(4f, "_BlotchMultiply", 4f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InSine);
      GameManager.GetInstance().OnConversationNext(jellyBossIntroRitual.cameraTarget, 10f);
      jellyBossIntroRitual.tweens.Add((Tween) DOTween.To((DOGetter<float>) (() => GameManager.GetInstance().CamFollowTarget.targetDistance), (DOSetter<float>) (x => GameManager.GetInstance().CamFollowTarget.targetDistance = x), 6f, 6f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InSine));
      jellyBossIntroRitual.amplifyColorEffect.BlendTo((Texture) jellyBossIntroRitual.lutTexture, 6f, (System.Action) null);
      for (int index = 0; index < jellyBossIntroRitual.enemySpines.Length; ++index)
        jellyBossIntroRitual.StartCoroutine((IEnumerator) jellyBossIntroRitual.SpawnSouls(jellyBossIntroRitual.enemySpines[index].transform.position));
      yield return (object) new WaitForSeconds(1f);
      jellyBossIntroRitual.bloodParticle.SetActive(true);
      yield return (object) new WaitForSeconds(2.3f);
      jellyBossIntroRitual.symbols.material.DOFloat(5f, "_BlotchMultiply", 5f);
      jellyBossIntroRitual.BloodPortalEffect.transform.DOScale(new Vector3(5.3f, 2f, 5.3f), 2f);
      CameraManager.instance.ShakeCameraForDuration(1f, 1.5f, 10f);
      yield return (object) new WaitForSeconds(2.2f);
      yield return (object) new WaitForSeconds(1.3f);
      CameraManager.instance.StopAllCoroutines();
      yield return (object) new WaitForEndOfFrame();
      GameManager.GetInstance().OnConversationNext(jellyBossIntroRitual.cultLeader, 12f);
      SimulationManager.Pause();
      jellyBossIntroRitual.distortionObject.DOScale(50f, 5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(jellyBossIntroRitual.\u003CRitualRoutine\u003Eb__44_2));
      AmplifyColorEffect amplifyColorEffect = jellyBossIntroRitual.amplifyColorEffect;
      amplifyColorEffect.LutHighlightTexture = jellyBossIntroRitual.originalHighlightLut;
      amplifyColorEffect.LutTexture = jellyBossIntroRitual.originalLut;
      CameraManager.instance.ShakeCameraForDuration(1.5f, 2f, 0.1f);
      BiomeConstants.Instance.ImpactFrameForDuration();
      MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact);
      yield return (object) new WaitForSeconds(0.1f);
      foreach (SkeletonAnimation enemySpine in jellyBossIntroRitual.enemySpines)
      {
        string[] strArray = new string[3]
        {
          "BloodImpact_0",
          "BloodImpact_1",
          "BloodImpact_2"
        };
        int index = UnityEngine.Random.Range(0, strArray.Length - 1);
        if (strArray[index] != null)
          BiomeConstants.Instance.EmitBloodImpact(enemySpine.transform.position + Vector3.back * 0.5f, (float) UnityEngine.Random.Range(0, 360), "black", strArray[index]);
        BiomeConstants.Instance.EmitBloodSplatterGroundParticles(enemySpine.transform.position, Vector3.zero, jellyBossIntroRitual.bloodColor);
        enemySpine.gameObject.SetActive(false);
        jellyBossIntroRitual.SpawnDeadBody(enemySpine.transform.position, enemySpine.transform.localScale);
      }
      yield return (object) new WaitForSeconds(0.15f);
      foreach (SkeletonAnimation surroundingSpine in jellyBossIntroRitual.surroundingSpines)
      {
        string[] strArray = new string[3]
        {
          "BloodImpact_0",
          "BloodImpact_1",
          "BloodImpact_2"
        };
        int index = UnityEngine.Random.Range(0, strArray.Length - 1);
        if (strArray[index] != null)
          BiomeConstants.Instance.EmitBloodImpact(surroundingSpine.transform.position + Vector3.back * 0.5f, (float) UnityEngine.Random.Range(0, 360), "black", strArray[index]);
        BiomeConstants.Instance.EmitBloodSplatterGroundParticles(surroundingSpine.transform.position, Vector3.zero, jellyBossIntroRitual.bloodColor);
        surroundingSpine.gameObject.SetActive(false);
        jellyBossIntroRitual.SpawnDeadBody(surroundingSpine.transform.position, surroundingSpine.transform.localScale);
      }
      yield return (object) new WaitForSeconds(0.15f);
      foreach (LongGrass longGrass in jellyBossIntroRitual.surroundingGrass)
        longGrass.StartCoroutine((IEnumerator) longGrass.ShakeGrassRoutine(jellyBossIntroRitual.gameObject, 2f));
      yield return (object) new WaitForSeconds(2.7f);
      jellyBossIntroRitual.StartCoroutine((IEnumerator) jellyBossIntroRitual.IntroDone());
    }
  }

  public IEnumerator IntroDone()
  {
    this.skippable = false;
    LetterBox.Instance.HideSkipPrompt();
    GameManager.GetInstance().OnConversationNext(this.cultLeader, 16f);
    CameraManager.instance.ShakeCameraForDuration(1.5f, 2f, 0.7f);
    this.cultLeaderSpine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    if ((UnityEngine.Object) this.playerBehind != (UnityEngine.Object) null)
    {
      if (this.playerBehind.GoToAndStopping)
      {
        this.playerBehind.EndGoToAndStop();
        this.playerBehind.transform.position = this.playerBehindTargetPosition;
      }
      PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive);
    }
    yield return (object) new WaitForSeconds(1.5f);
    this.jellyBoss.Play();
    this.jellyBossHealth.untouchable = false;
    yield return (object) new WaitForEndOfFrame();
    GameManager.GetInstance().OnConversationEnd();
    SimulationManager.Pause();
    this.jellyBoss.GetComponentInChildren<EnemyTentacleMonster>().BeginPhase1();
    if (!DataManager.Instance.BossesEncountered.Contains(FollowerLocation.Dungeon1_3))
      DataManager.Instance.BossesEncountered.Add(FollowerLocation.Dungeon1_3);
    this.BloodPortalEffect.transform.DOScale(Vector3.zero, 2f);
  }

  public IEnumerator SpawnSouls(Vector3 position)
  {
    JellyBossIntroRitual jellyBossIntroRitual = this;
    float delay = 0.3f;
    int ParticleCount = 50;
    for (int i = 0; i < ParticleCount; ++i)
    {
      float time = (float) i / (float) ParticleCount;
      delay *= 1f - jellyBossIntroRitual.absorbSoulCurve.Evaluate(time);
      SoulCustomTarget.Create(jellyBossIntroRitual.cameraTarget, new Vector3(position.x, position.y, position.z + 1f), Color.red, (System.Action) null, 0.2f, (float) (100.0 * (1.0 + (double) jellyBossIntroRitual.absorbSoulCurve.Evaluate(time)))).transform.parent = jellyBossIntroRitual.transform;
      yield return (object) new WaitForSeconds(delay);
    }
  }

  public IEnumerator CompleteHealingBishopRoom()
  {
    if (this.healingKallamar)
    {
      yield return (object) new WaitForEndOfFrame();
      RoomLockController.RoomCompleted(true);
    }
  }

  public void SpawnDeadBody(Vector3 pos, Vector3 scale)
  {
    UnityEngine.Object.Instantiate<GameObject>(this.deathParticlePrefab, pos, Quaternion.identity, this.transform.parent);
    DeadBodySliding deadBodySliding = UnityEngine.Object.Instantiate<DeadBodySliding>(this.enemyBody, pos, Quaternion.identity, this.transform.parent);
    deadBodySliding.Init(this.gameObject, UnityEngine.Random.Range(0.0f, 360f), (float) UnityEngine.Random.Range(500, 1000));
    deadBodySliding.transform.localScale = scale;
  }

  public IEnumerator HealingIE()
  {
    JellyBossIntroRitual jellyBossIntroRitual = this;
    jellyBossIntroRitual.triggered = true;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(jellyBossIntroRitual.gameObject);
    Demon_Spirit demon = (Demon_Spirit) null;
    foreach (GameObject demon1 in Demon_Arrows.Demons)
    {
      Demon_Spirit component = demon1.GetComponent<Demon_Spirit>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.FollowerInfo.ID == 99992)
      {
        demon = component;
        break;
      }
    }
    demon.enabled = false;
    demon.spine.transform.DOMove(Vector3.zero, 3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear);
    yield return (object) new WaitForSeconds(3f);
    GameManager.GetInstance().OnConversationNext(jellyBossIntroRitual.gameObject, 6f);
    if ((UnityEngine.Object) jellyBossIntroRitual.playerBehind != (UnityEngine.Object) null && jellyBossIntroRitual.playerBehind.GoToAndStopping)
      jellyBossIntroRitual.playerBehind.transform.position = jellyBossIntroRitual.playerBehindTargetPosition;
    yield return (object) new WaitForSeconds(1f);
    demon.spine.AnimationState.SetAnimation(0, "transform", false);
    AudioManager.Instance.PlayOneShot("event:/relics/demon_spawn");
    yield return (object) new WaitForSeconds(0.5f);
    FollowerManager.SpawnedFollower follower = FollowerManager.SpawnCopyFollower(FollowerInfo.GetInfoByID(99992), Vector3.zero, jellyBossIntroRitual.transform, FollowerLocation.Dungeon1_1);
    BiomeConstants.Instance.EmitSmokeExplosionVFX(Vector3.zero);
    AudioManager.Instance.PlayOneShot("event:/relics/puff_of_smoke", jellyBossIntroRitual.gameObject);
    BiomeConstants.Instance.ShakeCamera();
    CameraManager.instance.ShakeCameraForDuration(0.0f, 1f, 0.5f);
    demon.gameObject.SetActive(false);
    follower.Follower.OverridingEmotions = true;
    follower.Follower.SetFaceAnimation("Emotions/emotion-unhappy", true);
    double num1 = (double) follower.Follower.SetBodyAnimation("Reactions/react-sad", false);
    follower.Follower.AddBodyAnimation("idle", true, 0.0f);
    yield return (object) new WaitForSeconds(2f);
    List<ConversationEntry> Entries1 = new List<ConversationEntry>()
    {
      new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Kallamar/Healing/Second/0"),
      new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Kallamar/Healing/Second/1"),
      new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Kallamar/Healing/Second/2"),
      new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Kallamar/Healing/Second/3"),
      new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Kallamar/Healing/Second/4"),
      new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Kallamar/Healing/Second/5"),
      new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Kallamar/Healing/Second/6")
    };
    bool accepted = false;
    bool answered = false;
    List<MMTools.Response> Responses = new List<MMTools.Response>()
    {
      new MMTools.Response(ScriptLocalization.UI_Generic.Accept, (System.Action) (() =>
      {
        accepted = true;
        answered = true;
      }), ScriptLocalization.UI_Generic.Accept),
      new MMTools.Response(ScriptLocalization.UI_Generic.Decline, (System.Action) (() =>
      {
        accepted = false;
        answered = true;
      }), ScriptLocalization.UI_Generic.Decline)
    };
    foreach (ConversationEntry conversationEntry in Entries1)
    {
      conversationEntry.CharacterName = follower.FollowerFakeInfo.Name;
      conversationEntry.Animation = "Conversations/talk-nice" + UnityEngine.Random.Range(1, 3).ToString();
      conversationEntry.LoopAnimation = false;
      conversationEntry.soundPath = " ";
    }
    MMConversation.Play(new ConversationObject(Entries1, Responses, (System.Action) null), false);
    while (MMConversation.isPlaying || !answered)
      yield return (object) null;
    if (accepted)
    {
      AudioManager.Instance.PlayOneShot("event:/Stings/bishop_healed", jellyBossIntroRitual.gameObject);
      GameManager.GetInstance().OnConversationNext(jellyBossIntroRitual.gameObject, 5f);
      jellyBossIntroRitual.healEffect.gameObject.SetActive(true);
      jellyBossIntroRitual.healEffectSpine.AnimationState.SetAnimation(0, "animation", false);
      jellyBossIntroRitual.healEffectSpine.AnimationState.AddAnimation(0, "off", true, 0.0f);
      double num2 = (double) follower.Follower.SetBodyAnimation("bishop-heal", false);
      follower.Follower.AddBodyAnimation("idle", true, 0.0f);
      jellyBossIntroRitual.SermonOverlay.gameObject.SetActive(true);
      jellyBossIntroRitual.SermonOverlay.AnimationState.SetAnimation(0, "1", false);
      yield return (object) new WaitForSeconds(1.15f);
      yield return (object) new WaitForSeconds(1.55f);
      DataManager.Instance.KallamarHealed = true;
      follower.FollowerBrain.Info.SkinName = "CultLeader 3 Healed";
      follower.FollowerBrain.Info.SkinCharacter = WorshipperData.Instance.GetSkinIndexFromName(follower.FollowerBrain.Info.SkinName);
      FollowerBrain.SetFollowerCostume(follower.Follower.Spine.Skeleton, follower.FollowerBrain._directInfoAccess);
      BiomeConstants.Instance.EmitSmokeExplosionVFX(Vector3.zero);
      AudioManager.Instance.PlayOneShot("event:/relics/puff_of_smoke", jellyBossIntroRitual.gameObject);
      follower.Follower.Spine.Skeleton.SetSkin(follower.FollowerBrain.Info.SkinName);
      foreach (WorshipperData.SlotAndColor slotAndColour in WorshipperData.Instance.GetColourData(follower.FollowerBrain.Info.SkinName).SlotAndColours[follower.FollowerBrain.Info.SkinColour].SlotAndColours)
      {
        Slot slot = follower.Follower.Spine.Skeleton.FindSlot(slotAndColour.Slot);
        if (slot != null)
          slot.SetColor(slotAndColour.color);
      }
      follower.Follower.Outfit.SetInfo(follower.FollowerBrain._directInfoAccess);
      follower.Follower.Outfit.SetOutfit(follower.Follower.Spine, false);
      follower.FollowerBrain.AddTrait(FollowerTrait.TraitType.BishopOfCult, true);
      FollowerBrain.SetFollowerCostume(follower.Follower.Spine.Skeleton, follower.FollowerBrain.Info.XPLevel, follower.FollowerBrain.Info.SkinName, follower.FollowerBrain.Info.SkinColour, follower.FollowerBrain.Info.Outfit, follower.FollowerBrain.Info.Hat, follower.FollowerBrain.Info.Clothing, follower.FollowerBrain.Info.Customisation, follower.FollowerBrain.Info.Special, follower.FollowerBrain.Info.Necklace, follower.FollowerBrain.Info.ClothingVariant, follower.FollowerBrain._directInfoAccess);
      yield return (object) new WaitForSeconds(2f);
      jellyBossIntroRitual.healEffect.gameObject.SetActive(false);
      List<ConversationEntry> Entries2 = new List<ConversationEntry>()
      {
        new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Kallamar/Healing/Give/0")
      };
      foreach (ConversationEntry conversationEntry in Entries2)
      {
        conversationEntry.CharacterName = follower.FollowerFakeInfo.Name;
        conversationEntry.Animation = "Conversations/talk-nice" + UnityEngine.Random.Range(1, 3).ToString();
        conversationEntry.LoopAnimation = false;
        conversationEntry.soundPath = " ";
      }
      MMConversation.Play(new ConversationObject(Entries2, (List<MMTools.Response>) null, (System.Action) null), false);
      while (MMConversation.isPlaying)
        yield return (object) null;
      GameManager.GetInstance().OnConversationNext(jellyBossIntroRitual.gameObject, 5f);
      double num3 = (double) follower.Follower.SetBodyAnimation("Reactions/react-bow", false);
      yield return (object) new WaitForSeconds(1.93333328f);
    }
    else
    {
      List<ConversationEntry> Entries3 = new List<ConversationEntry>()
      {
        new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Kallamar/Healing/Deny/0")
      };
      foreach (ConversationEntry conversationEntry in Entries3)
      {
        conversationEntry.CharacterName = follower.FollowerFakeInfo.Name;
        conversationEntry.Animation = "Conversations/talk-nice" + UnityEngine.Random.Range(1, 3).ToString();
        conversationEntry.LoopAnimation = false;
        conversationEntry.soundPath = " ";
      }
      MMConversation.Play(new ConversationObject(Entries3, (List<MMTools.Response>) null, (System.Action) null), false);
      while (MMConversation.isPlaying)
        yield return (object) null;
    }
    FollowerManager.CleanUpCopyFollower(follower);
    BiomeConstants.Instance.EmitSmokeExplosionVFX(Vector3.zero);
    AudioManager.Instance.PlayOneShot("event:/relics/puff_of_smoke", jellyBossIntroRitual.gameObject);
    BiomeConstants.Instance.ShakeCamera();
    CameraManager.instance.ShakeCameraForDuration(0.0f, 1f, 0.5f);
    demon.gameObject.SetActive(true);
    demon.transform.position = Vector3.zero;
    demon.enabled = true;
    if (accepted)
      jellyBossIntroRitual.DoRelicUnlock(RelicType.SpawnCombatFollower);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.HealingBishop_Kallamar, 99992);
    DataManager.Instance.GaveKallamarHealingQuest = true;
    yield return (object) new WaitForSeconds(1f);
    DataManager.Instance.KallamarHealQuestCompleted = true;
    GameManager.GetInstance().OnConversationEnd();
    RoomLockController.RoomCompleted();
    jellyBossIntroRitual.SermonOverlay.gameObject.SetActive(false);
  }

  public void DoRelicUnlock(RelicType relicType)
  {
    if (!((UnityEngine.Object) this.RelicPodium != (UnityEngine.Object) null))
      return;
    this.RelicPodium.RemoveIfNotFirstLayer = false;
    this.RelicPodium.ForceSetRelic(relicType);
    this.RelicPodium.Reveal();
  }

  [CompilerGenerated]
  public void \u003CRitualRoutine\u003Eb__44_2()
  {
    UnityEngine.Object.Destroy((UnityEngine.Object) this.distortionObject.gameObject);
  }
}
