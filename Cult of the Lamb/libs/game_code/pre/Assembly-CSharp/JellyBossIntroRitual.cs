// Decompiled with JetBrains decompiler
// Type: JellyBossIntroRitual
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using MMTools;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class JellyBossIntroRitual : BaseMonoBehaviour
{
  [SerializeField]
  private GameObject cultLeader;
  [SerializeField]
  private GameObject cameraTarget;
  [SerializeField]
  private SkeletonAnimation cultLeaderSpine;
  [SerializeField]
  private Renderer[] blood;
  [SerializeField]
  private GameObject bloodParticle;
  [SerializeField]
  private Renderer symbols;
  [SerializeField]
  private AnimationCurve absorbSoulCurve;
  [SerializeField]
  private Texture2D lutTexture;
  [SerializeField]
  private Transform distortionObject;
  [SerializeField]
  private GameObject transformParticles;
  [Space]
  [SerializeField]
  private DeadBodySliding enemyBody;
  [SerializeField]
  private GameObject deathParticlePrefab;
  [SerializeField]
  private GameObject[] bloodSprays;
  [SerializeField]
  private SkeletonAnimation[] enemySpines;
  [SerializeField]
  private SkeletonAnimation[] surroundingSpines;
  [Space]
  [SerializeField]
  private MiniBossController jellyBoss;
  private LongGrass[] surroundingGrass;
  public GameObject BloodPortalEffect;
  private Texture originalLut;
  private Texture originalHighlightLut;
  private int camZoom = 10;
  private bool triggered;
  public Color bloodColor = new Color(0.47f, 0.11f, 0.11f, 1f);
  private bool skippable;
  private List<Tween> tweens = new List<Tween>();
  private Camera mainCamera;
  private AmplifyColorEffect amplifyColorEffect;
  public GameObject offsetObject;

  private void Awake()
  {
    foreach (Renderer renderer in this.blood)
      renderer.material.SetFloat("_BlotchMultiply", 0.0f);
    this.symbols.material.SetFloat("_BlotchMultiply", 0.0f);
    foreach (GameObject bloodSpray in this.bloodSprays)
      bloodSpray.SetActive(false);
    this.surroundingGrass = this.transform.parent.GetComponentsInChildren<LongGrass>();
    this.mainCamera = Camera.main;
    this.amplifyColorEffect = this.mainCamera.GetComponent<AmplifyColorEffect>();
  }

  private void Update()
  {
    if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null) || !InputManager.Gameplay.GetAttackButtonDown() || !this.skippable || MonoSingleton<UIManager>.Instance.MenusBlocked)
      return;
    this.StopAllCoroutines();
    this.gameObject.SetActive(false);
    CameraManager.instance.Stopshake();
    foreach (Tween tween in this.tweens)
      tween.Kill();
    AmplifyColorEffect amplifyColorEffect = this.amplifyColorEffect;
    amplifyColorEffect.LutHighlightTexture = this.originalHighlightLut;
    amplifyColorEffect.LutTexture = this.originalLut;
    MMConversation.mmConversation.Close();
    LetterBox.Instance.HideSkipPrompt();
    this.cultLeaderSpine.AnimationState.SetAnimation(0, "transform", false).TrackTime = 10.2f;
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/roar", this.gameObject);
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.IntroDone());
  }

  private void Start() => this.cultLeaderSpine.AnimationState.SetAnimation(0, "leader/idle", true);

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (this.triggered || !(collision.tag == "Player"))
      return;
    this.StartCoroutine((IEnumerator) this.RitualRoutine());
  }

  private IEnumerator RitualRoutine()
  {
    JellyBossIntroRitual jellyBossIntroRitual = this;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.Instance.playerController.speed = 0.0f;
    SimulationManager.Pause();
    HUD_Manager.Instance.HideTopRight();
    jellyBossIntroRitual.originalHighlightLut = jellyBossIntroRitual.amplifyColorEffect.LutHighlightTexture;
    jellyBossIntroRitual.originalLut = jellyBossIntroRitual.amplifyColorEffect.LutTexture;
    jellyBossIntroRitual.amplifyColorEffect.LutHighlightBlendTexture = (Texture) jellyBossIntroRitual.lutTexture;
    jellyBossIntroRitual.triggered = true;
    AudioManager.Instance.PlayOneShot("event:/boss/frog/transition_intro_zoom");
    MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
    {
      new ConversationEntry(jellyBossIntroRitual.offsetObject, "Conversation_NPC/Story/Dungeon1/Leader3/Boss1"),
      new ConversationEntry(jellyBossIntroRitual.offsetObject, "Conversation_NPC/Story/Dungeon1/Leader3/Boss2")
    }, (List<MMTools.Response>) null, (System.Action) null), false, SetPlayerIdleOnComplete: false, showControlPrompt: false);
    MMConversation.mmConversation.SpeechBubble.ScreenOffset = 350f;
    yield return (object) new WaitForSeconds(1f);
    jellyBossIntroRitual.skippable = DataManager.Instance.BossesEncountered.Contains(PlayerFarming.Location);
    if (jellyBossIntroRitual.skippable)
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
    // ISSUE: reference to a compiler-generated method
    jellyBossIntroRitual.distortionObject.DOScale(50f, 5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(jellyBossIntroRitual.\u003CRitualRoutine\u003Eb__32_2));
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

  private IEnumerator IntroDone()
  {
    this.skippable = false;
    LetterBox.Instance.HideSkipPrompt();
    GameManager.GetInstance().OnConversationNext(this.cultLeader, 16f);
    CameraManager.instance.ShakeCameraForDuration(1.5f, 2f, 0.7f);
    this.cultLeaderSpine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    yield return (object) new WaitForSeconds(1.5f);
    this.jellyBoss.Play();
    yield return (object) new WaitForEndOfFrame();
    GameManager.GetInstance().OnConversationEnd();
    this.jellyBoss.GetComponentInChildren<EnemyTentacleMonster>().BeginPhase1();
    if (!DataManager.Instance.BossesEncountered.Contains(FollowerLocation.Dungeon1_3))
      DataManager.Instance.BossesEncountered.Add(FollowerLocation.Dungeon1_3);
    this.BloodPortalEffect.transform.DOScale(Vector3.zero, 2f);
  }

  private IEnumerator SpawnSouls(Vector3 position)
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

  private void SpawnDeadBody(Vector3 pos, Vector3 scale)
  {
    UnityEngine.Object.Instantiate<GameObject>(this.deathParticlePrefab, pos, Quaternion.identity, this.transform.parent);
    DeadBodySliding deadBodySliding = UnityEngine.Object.Instantiate<DeadBodySliding>(this.enemyBody, pos, Quaternion.identity, this.transform.parent);
    deadBodySliding.Init(this.gameObject, UnityEngine.Random.Range(0.0f, 360f), (float) UnityEngine.Random.Range(500, 1000));
    deadBodySliding.transform.localScale = scale;
  }
}
