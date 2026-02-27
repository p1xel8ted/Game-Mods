// Decompiled with JetBrains decompiler
// Type: FrogBossIntroRitual
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
public class FrogBossIntroRitual : BaseMonoBehaviour
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
  private AmplifyColorEffect amplifyColorEffect;
  private int camZoom = 10;
  private bool triggered;
  public Color bloodColor = new Color(0.47f, 0.11f, 0.11f, 1f);
  private bool skippable;
  private List<Tween> tweens = new List<Tween>();
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
    this.amplifyColorEffect = Camera.main.GetComponent<AmplifyColorEffect>();
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
    this.frogBoss.Play();
    LetterBox.Instance.HideSkipPrompt();
    MMConversation.mmConversation.Close();
    AmplifyColorEffect component = Camera.main.GetComponent<AmplifyColorEffect>();
    component.LutHighlightTexture = this.originalHighlightLut;
    component.LutTexture = this.originalLut;
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    AudioManager.Instance.PlayOneShot("event:/boss/frog/roar", this.frogBoss.gameObject);
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (this.triggered || !(collision.tag == "Player"))
      return;
    this.StartCoroutine((IEnumerator) this.RitualRoutine());
  }

  private IEnumerator RitualRoutine()
  {
    FrogBossIntroRitual frogBossIntroRitual = this;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.Instance.playerController.speed = 0.0f;
    SimulationManager.Pause();
    HUD_Manager.Instance.HideTopRight();
    frogBossIntroRitual.originalHighlightLut = frogBossIntroRitual.amplifyColorEffect.LutHighlightTexture;
    frogBossIntroRitual.originalLut = frogBossIntroRitual.amplifyColorEffect.LutTexture;
    frogBossIntroRitual.amplifyColorEffect.LutHighlightBlendTexture = (Texture) frogBossIntroRitual.lutTexture;
    frogBossIntroRitual.triggered = true;
    AudioManager.Instance.PlayOneShot("event:/boss/frog/transition_intro_zoom");
    List<ConversationEntry> Entries = new List<ConversationEntry>()
    {
      new ConversationEntry(frogBossIntroRitual.offsetObject, "Conversation_NPC/Story/Dungeon1/Leader2/Boss1"),
      new ConversationEntry(frogBossIntroRitual.offsetObject, "Conversation_NPC/Story/Dungeon1/Leader2/Boss2")
    };
    Entries[0].CharacterName = "NAMES/CultLeaders/Dungeon2";
    Entries[0].Offset = new Vector3(0.0f, 3f, 0.0f);
    Entries[0].Animation = "talk";
    Entries[0].SkeletonData = frogBossIntroRitual.cultLeaderSpine;
    Entries[1].CharacterName = "NAMES/CultLeaders/Dungeon2";
    Entries[1].Offset = new Vector3(0.0f, 3f, 0.0f);
    Entries[1].Animation = "talk";
    Entries[1].SkeletonData = frogBossIntroRitual.cultLeaderSpine;
    foreach (ConversationEntry conversationEntry in Entries)
      conversationEntry.soundPath = "event:/dialogue/dun2_cult_leader_heket/standard_heket";
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) null), false, SetPlayerIdleOnComplete: false, showControlPrompt: false);
    MMConversation.mmConversation.SpeechBubble.ScreenOffset = 350f;
    yield return (object) new WaitForSeconds(1f);
    frogBossIntroRitual.skippable = DataManager.Instance.BossesEncountered.Contains(PlayerFarming.Location);
    if (frogBossIntroRitual.skippable)
      LetterBox.Instance.ShowSkipPrompt();
    while (MMConversation.CURRENT_CONVERSATION != null)
      yield return (object) null;
    UnityEngine.Object.Destroy((UnityEngine.Object) frogBossIntroRitual.offsetObject);
    yield return (object) new WaitForEndOfFrame();
    AudioManager.Instance.PlayOneShot("event:/boss/frog/transition_zoom_back");
    AudioManager.Instance.PlayOneShot("event:/boss/frog/after_intro_grunt");
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(frogBossIntroRitual.cameraTarget, 12f);
    yield return (object) new WaitForSeconds(1f);
    AudioManager.Instance.PlayOneShot("event:/boss/frog/cultist_sequence");
    foreach (Component enemySpine in frogBossIntroRitual.enemySpines)
      enemySpine.GetComponentInChildren<WorshipperBubble>(true).Play(WorshipperBubble.SPEECH_TYPE.BOSSCROWN2, 4.5f, UnityEngine.Random.Range(0.0f, 0.3f));
    foreach (SkeletonAnimation enemySpine in frogBossIntroRitual.enemySpines)
      enemySpine.AnimationState.SetAnimation(0, "dance", true).TrackTime = UnityEngine.Random.Range(0.0f, 0.3f);
    yield return (object) new WaitForSeconds(3f);
    foreach (SkeletonAnimation enemySpine in frogBossIntroRitual.enemySpines)
      enemySpine.AnimationState.SetAnimation(0, "ritual/sacrifice", false).TrackTime = UnityEngine.Random.Range(0.0f, 0.3f);
    yield return (object) new WaitForSeconds(1f);
    frogBossIntroRitual.cultLeaderSpine.AnimationState.SetAnimation(0, "transform", false);
    AudioManager.Instance.PlayOneShot("event:/boss/frog/transform_sequence");
    yield return (object) new WaitForSeconds(0.3f);
    foreach (SkeletonAnimation surroundingSpine in frogBossIntroRitual.surroundingSpines)
      surroundingSpine.AnimationState.SetAnimation(0, "worship", true).TrackTime = UnityEngine.Random.Range(0.0f, 0.4f);
    foreach (GameObject bloodSpray in frogBossIntroRitual.bloodSprays)
    {
      bloodSpray.SetActive(true);
      AudioManager.Instance.PlayOneShot("event:/enemy/impact_squishy", bloodSpray.gameObject);
      float z = Vector3.Angle(bloodSpray.transform.position, frogBossIntroRitual.cultLeader.transform.position);
      BiomeConstants.Instance.EmitBloodSplatter(bloodSpray.transform.position, new Vector3(0.0f, 0.0f, z), Color.black);
    }
    foreach (Renderer renderer in frogBossIntroRitual.blood)
      renderer.material.DOFloat(4f, "_BlotchMultiply", 4f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InSine);
    GameManager.GetInstance().OnConversationNext(frogBossIntroRitual.cameraTarget, 10f);
    frogBossIntroRitual.tweens.Add((Tween) DOTween.To((DOGetter<float>) (() => GameManager.GetInstance().CamFollowTarget.targetDistance), (DOSetter<float>) (x => GameManager.GetInstance().CamFollowTarget.targetDistance = x), 6f, 6f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InSine));
    Camera.main.GetComponent<AmplifyColorEffect>().BlendTo((Texture) frogBossIntroRitual.lutTexture, 6f, (System.Action) null);
    for (int index = 0; index < frogBossIntroRitual.enemySpines.Length; ++index)
      frogBossIntroRitual.StartCoroutine((IEnumerator) frogBossIntroRitual.SpawnSouls(frogBossIntroRitual.enemySpines[index].transform.position));
    yield return (object) new WaitForSeconds(1f);
    frogBossIntroRitual.bloodParticle.SetActive(true);
    yield return (object) new WaitForSeconds(2f);
    frogBossIntroRitual.symbols.material.DOFloat(5f, "_BlotchMultiply", 5f);
    frogBossIntroRitual.BloodPortalEffect.transform.DOScale(new Vector3(5.3f, 2f, 5.3f), 2f);
    CameraManager.instance.ShakeCameraForDuration(1f, 1.5f, 10f);
    BiomeConstants.Instance.ImpactFrameForDuration();
    MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact);
    yield return (object) new WaitForSeconds(4f);
    frogBossIntroRitual.BloodPortalEffect.transform.DOScale(Vector3.zero, 2f);
  }

  private IEnumerator SpawnSouls(Vector3 position)
  {
    FrogBossIntroRitual frogBossIntroRitual = this;
    float delay = 0.3f;
    int ParticleCount = 50;
    for (int i = 0; i < ParticleCount; ++i)
    {
      float time = (float) i / (float) ParticleCount;
      delay *= 1f - frogBossIntroRitual.absorbSoulCurve.Evaluate(time);
      SoulCustomTarget.Create(frogBossIntroRitual.cameraTarget, new Vector3(position.x, position.y, position.z + 1f), Color.red, (System.Action) null, 0.2f, (float) (100.0 * (1.0 + (double) frogBossIntroRitual.absorbSoulCurve.Evaluate(time)))).transform.parent = frogBossIntroRitual.transform;
      yield return (object) new WaitForSeconds(delay);
    }
  }

  private IEnumerator BossTransformed()
  {
    FrogBossIntroRitual frogBossIntroRitual = this;
    frogBossIntroRitual.skippable = false;
    frogBossIntroRitual.cultLeader.SetActive(false);
    frogBossIntroRitual.frogBoss.gameObject.SetActive(true);
    frogBossIntroRitual.frogBoss.Play();
    // ISSUE: reference to a compiler-generated method
    frogBossIntroRitual.distortionObject.DOScale(50f, 5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(frogBossIntroRitual.\u003CBossTransformed\u003Eb__31_0));
    AmplifyColorEffect component = Camera.main.GetComponent<AmplifyColorEffect>();
    component.LutHighlightTexture = frogBossIntroRitual.originalHighlightLut;
    component.LutTexture = frogBossIntroRitual.originalLut;
    CameraManager.instance.StopAllCoroutines();
    yield return (object) new WaitForEndOfFrame();
    CameraManager.instance.ShakeCameraForDuration(1.5f, 2f, 0.3f);
    yield return (object) new WaitForSeconds(0.1f);
    foreach (SkeletonAnimation enemySpine in frogBossIntroRitual.enemySpines)
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
      BiomeConstants.Instance.EmitBloodSplatterGroundParticles(enemySpine.transform.position, Vector3.zero, frogBossIntroRitual.bloodColor);
      enemySpine.gameObject.SetActive(false);
      frogBossIntroRitual.SpawnDeadBody(enemySpine.transform.position, enemySpine.transform.localScale);
    }
    yield return (object) new WaitForSeconds(0.15f);
    foreach (SkeletonAnimation surroundingSpine in frogBossIntroRitual.surroundingSpines)
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
      BiomeConstants.Instance.EmitBloodSplatterGroundParticles(surroundingSpine.transform.position, Vector3.zero, frogBossIntroRitual.bloodColor);
      surroundingSpine.gameObject.SetActive(false);
      frogBossIntroRitual.SpawnDeadBody(surroundingSpine.transform.position, surroundingSpine.transform.localScale);
    }
    yield return (object) new WaitForSeconds(0.15f);
    foreach (LongGrass longGrass in frogBossIntroRitual.surroundingGrass)
      longGrass.StartCoroutine((IEnumerator) longGrass.ShakeGrassRoutine(frogBossIntroRitual.gameObject, 2f));
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
