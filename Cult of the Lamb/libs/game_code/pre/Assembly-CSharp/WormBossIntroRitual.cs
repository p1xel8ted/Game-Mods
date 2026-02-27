// Decompiled with JetBrains decompiler
// Type: WormBossIntroRitual
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using MMTools;
using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class WormBossIntroRitual : BaseMonoBehaviour
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
  private MiniBossController frogBoss;
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
    this.cultLeaderSpine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.LeaderEvent);
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
    this.frogBoss.gameObject.SetActive(true);
    this.frogBoss.Play(true);
    AudioManager.Instance.PlayOneShot("event:/boss/worm/roar", this.frogBoss.gameObject);
    MMConversation.mmConversation.Close();
    LetterBox.Instance.HideSkipPrompt();
    AmplifyColorEffect amplifyColorEffect = this.amplifyColorEffect;
    amplifyColorEffect.LutHighlightTexture = this.originalHighlightLut;
    amplifyColorEffect.LutTexture = this.originalLut;
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (this.triggered || !(collision.tag == "Player"))
      return;
    this.StartCoroutine((IEnumerator) this.RitualRoutine());
  }

  private IEnumerator RitualRoutine()
  {
    WormBossIntroRitual wormBossIntroRitual = this;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.Instance.playerController.speed = 0.0f;
    SimulationManager.Pause();
    HUD_Manager.Instance.HideTopRight();
    wormBossIntroRitual.originalHighlightLut = wormBossIntroRitual.amplifyColorEffect.LutHighlightTexture;
    wormBossIntroRitual.originalLut = wormBossIntroRitual.amplifyColorEffect.LutTexture;
    wormBossIntroRitual.amplifyColorEffect.LutHighlightBlendTexture = (Texture) wormBossIntroRitual.lutTexture;
    wormBossIntroRitual.triggered = true;
    AudioManager.Instance.PlayOneShot("event:/boss/frog/transition_intro_zoom");
    List<ConversationEntry> Entries = new List<ConversationEntry>();
    Entries.Add(new ConversationEntry(wormBossIntroRitual.offsetObject, "Conversation_NPC/Story/Dungeon1/Leader1/Boss1"));
    Entries.Add(new ConversationEntry(wormBossIntroRitual.offsetObject, "Conversation_NPC/Story/Dungeon1/Leader1/Boss2"));
    Entries[0].CharacterName = "NAMES/CultLeaders/Dungeon1";
    Entries[0].Offset = new Vector3(0.0f, 2f, 0.0f);
    Entries[0].soundPath = "event:/dialogue/dun1_cult_leader_leshy/standard_leshy";
    Entries[0].Animation = "talk";
    Entries[0].SkeletonData = wormBossIntroRitual.cultLeaderSpine;
    Entries[1].CharacterName = "NAMES/CultLeaders/Dungeon1";
    Entries[1].Offset = new Vector3(0.0f, 2f, 0.0f);
    Entries[1].soundPath = "event:/dialogue/dun1_cult_leader_leshy/standard_leshy";
    Entries[1].Animation = "talk";
    Entries[1].SkeletonData = wormBossIntroRitual.cultLeaderSpine;
    wormBossIntroRitual.cultLeaderSpine.AnimationState.SetAnimation(0, "talk", true);
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) null), false, SetPlayerIdleOnComplete: false, showControlPrompt: false);
    MMConversation.mmConversation.SpeechBubble.ScreenOffset = 200f;
    yield return (object) new WaitForSeconds(1f);
    wormBossIntroRitual.skippable = DataManager.Instance.BossesEncountered.Contains(PlayerFarming.Location);
    if (wormBossIntroRitual.skippable)
      LetterBox.Instance.ShowSkipPrompt();
    while (MMConversation.CURRENT_CONVERSATION != null)
      yield return (object) null;
    UnityEngine.Object.Destroy((UnityEngine.Object) wormBossIntroRitual.offsetObject);
    yield return (object) new WaitForEndOfFrame();
    AudioManager.Instance.PlayOneShot("event:/boss/frog/transition_zoom_back");
    AudioManager.Instance.PlayOneShot("event:/boss/frog/after_intro_grunt");
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(wormBossIntroRitual.cultLeaderSpine.gameObject, 12f);
    yield return (object) new WaitForSeconds(1f);
    AudioManager.Instance.PlayOneShot("event:/boss/frog/cultist_sequence");
    foreach (Component enemySpine in wormBossIntroRitual.enemySpines)
      enemySpine.GetComponentInChildren<WorshipperBubble>(true).Play(WorshipperBubble.SPEECH_TYPE.BOSSCROWN1, 4.5f, UnityEngine.Random.Range(0.0f, 0.3f));
    foreach (SkeletonAnimation enemySpine in wormBossIntroRitual.enemySpines)
      enemySpine.AnimationState.SetAnimation(0, "ritual/preach2", true).TrackTime = UnityEngine.Random.Range(0.0f, 0.15f);
    yield return (object) new WaitForSeconds(2.8f);
    foreach (SkeletonAnimation enemySpine in wormBossIntroRitual.enemySpines)
      enemySpine.AnimationState.SetAnimation(0, "ritual/sacrifice2", false).TrackTime = UnityEngine.Random.Range(0.0f, 0.15f);
    yield return (object) new WaitForSeconds(1f);
    wormBossIntroRitual.cultLeaderSpine.AnimationState.SetAnimation(0, "transform", false);
    AudioManager.Instance.PlayOneShot("event:/boss/worm/transform");
    yield return (object) new WaitForSeconds(0.3f);
    foreach (SkeletonAnimation surroundingSpine in wormBossIntroRitual.surroundingSpines)
      surroundingSpine.AnimationState.SetAnimation(0, "ritual/preach", true).TrackTime = UnityEngine.Random.Range(0.0f, 0.4f);
    foreach (GameObject bloodSpray in wormBossIntroRitual.bloodSprays)
    {
      bloodSpray.SetActive(true);
      AudioManager.Instance.PlayOneShot("event:/enemy/impact_squishy", bloodSpray.gameObject);
      float z = Vector3.Angle(bloodSpray.transform.position, wormBossIntroRitual.cultLeader.transform.position);
      BiomeConstants.Instance.EmitBloodSplatter(bloodSpray.transform.position, new Vector3(0.0f, 0.0f, z), Color.black);
    }
    foreach (Renderer renderer in wormBossIntroRitual.blood)
      renderer.material.DOFloat(4f, "_BlotchMultiply", 4f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InSine);
    GameManager.GetInstance().OnConversationNext(wormBossIntroRitual.cultLeaderSpine.gameObject, 10f);
    wormBossIntroRitual.tweens.Add((Tween) DOTween.To((DOGetter<float>) (() => GameManager.GetInstance().CamFollowTarget.targetDistance), (DOSetter<float>) (x => GameManager.GetInstance().CamFollowTarget.targetDistance = x), 6f, 6f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InSine));
    wormBossIntroRitual.tweens.Add((Tween) DOTween.To((DOGetter<Vector3>) (() => GameManager.GetInstance().CamFollowTarget.TargetOffset), (DOSetter<Vector3>) (x => GameManager.GetInstance().CamFollowTarget.TargetOffset = x), Vector3.forward * -2f, 6f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine));
    wormBossIntroRitual.amplifyColorEffect.BlendTo((Texture) wormBossIntroRitual.lutTexture, 6f, (System.Action) null);
    for (int index = 0; index < wormBossIntroRitual.enemySpines.Length; ++index)
      wormBossIntroRitual.StartCoroutine((IEnumerator) wormBossIntroRitual.SpawnSouls(wormBossIntroRitual.enemySpines[index].transform.position));
    yield return (object) new WaitForSeconds(1f);
    wormBossIntroRitual.bloodParticle.SetActive(true);
    yield return (object) new WaitForSeconds(2f);
    wormBossIntroRitual.symbols.material.DOFloat(5f, "_BlotchMultiply", 5f);
    wormBossIntroRitual.BloodPortalEffect.transform.DOScale(new Vector3(5.3f, 2f, 5.3f), 2f);
    CameraManager.instance.ShakeCameraForDuration(1f, 1.5f, 10f);
    BiomeConstants.Instance.ImpactFrameForDuration();
    MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact);
    yield return (object) new WaitForSeconds(4f);
    wormBossIntroRitual.BloodPortalEffect.transform.DOScale(Vector3.zero, 2f);
  }

  private IEnumerator SpawnSouls(Vector3 position)
  {
    WormBossIntroRitual wormBossIntroRitual = this;
    float delay = 0.3f;
    int ParticleCount = 50;
    for (int i = 0; i < ParticleCount; ++i)
    {
      float time = (float) i / (float) ParticleCount;
      delay *= 1f - wormBossIntroRitual.absorbSoulCurve.Evaluate(time);
      SoulCustomTarget.Create(wormBossIntroRitual.cameraTarget, new Vector3(position.x, position.y, position.z + 1f), Color.red, (System.Action) null, 0.2f, (float) (100.0 * (1.0 + (double) wormBossIntroRitual.absorbSoulCurve.Evaluate(time)))).transform.parent = wormBossIntroRitual.transform;
      yield return (object) new WaitForSeconds(delay);
    }
  }

  private IEnumerator BossTransformed()
  {
    WormBossIntroRitual wormBossIntroRitual = this;
    wormBossIntroRitual.skippable = false;
    wormBossIntroRitual.cultLeader.SetActive(false);
    wormBossIntroRitual.frogBoss.gameObject.SetActive(true);
    wormBossIntroRitual.frogBoss.Play();
    GameManager.GetInstance().CamFollowTarget.TargetOffset = Vector3.zero;
    // ISSUE: reference to a compiler-generated method
    wormBossIntroRitual.distortionObject.DOScale(50f, 5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(wormBossIntroRitual.\u003CBossTransformed\u003Eb__32_0));
    AmplifyColorEffect amplifyColorEffect = wormBossIntroRitual.amplifyColorEffect;
    amplifyColorEffect.LutHighlightTexture = wormBossIntroRitual.originalHighlightLut;
    amplifyColorEffect.LutTexture = wormBossIntroRitual.originalLut;
    CameraManager.instance.StopAllCoroutines();
    yield return (object) new WaitForEndOfFrame();
    CameraManager.instance.ShakeCameraForDuration(1.5f, 2f, 0.3f);
    yield return (object) new WaitForSeconds(0.1f);
    foreach (SkeletonAnimation enemySpine in wormBossIntroRitual.enemySpines)
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
      BiomeConstants.Instance.EmitBloodSplatterGroundParticles(enemySpine.transform.position, Vector3.zero, wormBossIntroRitual.bloodColor);
      enemySpine.gameObject.SetActive(false);
      wormBossIntroRitual.SpawnDeadBody(enemySpine.transform.position, enemySpine.transform.localScale);
    }
    yield return (object) new WaitForSeconds(0.15f);
    foreach (SkeletonAnimation surroundingSpine in wormBossIntroRitual.surroundingSpines)
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
      BiomeConstants.Instance.EmitBloodSplatterGroundParticles(surroundingSpine.transform.position, Vector3.zero, wormBossIntroRitual.bloodColor);
      surroundingSpine.gameObject.SetActive(false);
      wormBossIntroRitual.SpawnDeadBody(surroundingSpine.transform.position, surroundingSpine.transform.localScale);
    }
    yield return (object) new WaitForSeconds(0.15f);
    foreach (LongGrass longGrass in wormBossIntroRitual.surroundingGrass)
      longGrass.StartCoroutine((IEnumerator) longGrass.ShakeGrassRoutine(wormBossIntroRitual.gameObject, 2f));
  }

  private void LeaderEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == "transform"))
      return;
    this.StartCoroutine((IEnumerator) this.BossTransformed());
  }

  private void SpawnDeadBody(Vector3 pos, Vector3 scale)
  {
    UnityEngine.Object.Instantiate<GameObject>(this.deathParticlePrefab, pos, Quaternion.identity, this.transform.parent);
    DeadBodySliding deadBodySliding = UnityEngine.Object.Instantiate<DeadBodySliding>(this.enemyBody, pos, Quaternion.identity, this.transform.parent);
    deadBodySliding.Init(this.gameObject, UnityEngine.Random.Range(0.0f, 360f), (float) UnityEngine.Random.Range(500, 1000));
    deadBodySliding.transform.localScale = scale;
  }
}
