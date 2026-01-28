// Decompiled with JetBrains decompiler
// Type: ExecutionerBossIntroRitual
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using FMODUnity;
using Lamb.UI;
using MMTools;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class ExecutionerBossIntroRitual : BaseMonoBehaviour
{
  [SerializeField]
  public GameObject cultLeader;
  [SerializeField]
  public GameObject cameraTarget;
  [SerializeField]
  public SkeletonAnimation cultLeaderSpine;
  [SerializeField]
  public Renderer symbols;
  [SerializeField]
  public AnimationCurve absorbSoulCurve;
  [SerializeField]
  public Texture2D lutTexture;
  [SerializeField]
  public Transform distortionObject;
  [Space]
  [SerializeField]
  public MiniBossController executionerBoss;
  [SerializeField]
  public Interaction_SimpleConversation introConvo;
  [SerializeField]
  public Health executionerBossHealth;
  [EventRef]
  public string IntroTransformationSFX = "event:/dlc/dungeon06/enemy/miniboss_executioner/story_intro_transformation";
  [EventRef]
  public string IntroTransformationSkippedSFX = "event:/dlc/dungeon06/enemy/miniboss_executioner/story_intro_transformation_short";
  public EventInstance longIntroInstanceSFX;
  public LongGrass[] surroundingGrass;
  public GameObject BloodPortalEffect;
  public Texture originalLut;
  public Texture originalHighlightLut;
  public int camZoom = 10;
  public bool triggered;
  public Color bloodColor = new Color(0.47f, 0.11f, 0.11f, 1f);
  public bool skippable;
  public bool skipped;
  public List<Tween> tweens = new List<Tween>();
  public Camera mainCamera;
  public AmplifyColorEffect amplifyColorEffect;
  public GameObject offsetObject;

  public void Awake()
  {
    this.symbols.material.SetFloat("_Rotation", -178f);
    this.surroundingGrass = this.transform.parent.GetComponentsInChildren<LongGrass>();
    this.mainCamera = Camera.main;
    this.amplifyColorEffect = this.mainCamera.GetComponent<AmplifyColorEffect>();
    this.executionerBossHealth.untouchable = true;
  }

  public void Update()
  {
    if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null) || this.skipped || !this.skippable || MonoSingleton<UIManager>.Instance.MenusBlocked || !InputManager.Gameplay.GetAttackButtonDown() && !DungeonSandboxManager.Active && !DataManager.Instance.RemovedStoryMomentsActive)
      return;
    this.StopAllCoroutines();
    this.StopAllActiveSFX();
    this.gameObject.SetActive(false);
    CameraManager.instance.Stopshake();
    foreach (Tween tween in this.tweens)
      tween.Kill();
    AmplifyColorEffect amplifyColorEffect = this.amplifyColorEffect;
    amplifyColorEffect.LutHighlightTexture = this.originalHighlightLut;
    amplifyColorEffect.LutTexture = this.originalLut;
    MMConversation.mmConversation?.Close();
    LetterBox.Instance.HideSkipPrompt();
    this.symbols.material.DOFloat(1f, "_Rotation", 3f);
    this.cultLeaderSpine.AnimationState.SetAnimation(0, "roar", false);
    this.cultLeaderSpine.Skeleton.SetSkin("Executioner_Boss");
    this.cultLeaderSpine.Skeleton.SetSlotsToSetupPose();
    AudioManager.Instance.PlayOneShot(this.IntroTransformationSkippedSFX);
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.IntroDone());
    this.skipped = true;
    this.executionerBossHealth.untouchable = false;
    this.executionerBoss.EnemiesToTrack[0].enabled = true;
  }

  public void Start()
  {
  }

  public void OnTriggerEnter2D(Collider2D collision)
  {
    if (this.triggered || this.skipped || !(collision.tag == "Player"))
      return;
    this.StartCoroutine((IEnumerator) this.RitualRoutine());
  }

  public void StopAllActiveSFX()
  {
    AudioManager.Instance.StopOneShotInstanceEarly(this.longIntroInstanceSFX, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
  }

  public IEnumerator RitualRoutine()
  {
    ExecutionerBossIntroRitual executionerBossIntroRitual = this;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.InActive;
    if ((bool) (UnityEngine.Object) MMConversation.mmConversation)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) MMConversation.mmConversation.gameObject);
      MMConversation.ClearConversation();
    }
    SimulationManager.Pause();
    HUD_Manager.Instance.HideTopRight();
    executionerBossIntroRitual.originalHighlightLut = executionerBossIntroRitual.amplifyColorEffect.LutHighlightTexture;
    executionerBossIntroRitual.originalLut = executionerBossIntroRitual.amplifyColorEffect.LutTexture;
    executionerBossIntroRitual.amplifyColorEffect.LutHighlightBlendTexture = (Texture) executionerBossIntroRitual.lutTexture;
    executionerBossIntroRitual.triggered = true;
    AudioManager.Instance.PlayOneShot("event:/boss/frog/transition_intro_zoom");
    if (DungeonSandboxManager.Active || DataManager.Instance.RemovedStoryMomentsActive)
    {
      executionerBossIntroRitual.skippable = true;
    }
    else
    {
      executionerBossIntroRitual.introConvo.Play();
      yield return (object) new WaitForSeconds(1f);
      executionerBossIntroRitual.skippable = DataManager.Instance.BossesEncountered.Contains(PlayerFarming.Location);
      if (executionerBossIntroRitual.skippable && !executionerBossIntroRitual.skipped)
        LetterBox.Instance.ShowSkipPrompt();
      while (MMConversation.CURRENT_CONVERSATION != null)
        yield return (object) null;
      UnityEngine.Object.Destroy((UnityEngine.Object) executionerBossIntroRitual.offsetObject);
      yield return (object) new WaitForEndOfFrame();
      AudioManager.Instance.PlayOneShot("event:/boss/frog/transition_zoom_back");
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(executionerBossIntroRitual.cameraTarget, 12f);
      yield return (object) new WaitForSeconds(1f);
      executionerBossIntroRitual.cultLeaderSpine.AnimationState.SetAnimation(0, "transform", true);
      executionerBossIntroRitual.cultLeaderSpine.AnimationState.AddAnimation(0, "roar", false, 0.0f);
      executionerBossIntroRitual.cultLeaderSpine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
      if (!string.IsNullOrEmpty(executionerBossIntroRitual.IntroTransformationSFX))
        executionerBossIntroRitual.longIntroInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(executionerBossIntroRitual.IntroTransformationSFX, (Transform) null, false);
      float transformAninTotalDuraiton = executionerBossIntroRitual.cultLeaderSpine.skeleton.Data.FindAnimation("transform").Duration;
      yield return (object) new WaitForSeconds(0.3f);
      transformAninTotalDuraiton -= 0.3f;
      GameManager.GetInstance().OnConversationNext(executionerBossIntroRitual.cameraTarget, 10f);
      executionerBossIntroRitual.tweens.Add((Tween) DOTween.To((DOGetter<float>) (() => GameManager.GetInstance().CamFollowTarget.targetDistance), (DOSetter<float>) (x => GameManager.GetInstance().CamFollowTarget.targetDistance = x), 6f, 6f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InSine));
      executionerBossIntroRitual.amplifyColorEffect.BlendTo((Texture) executionerBossIntroRitual.lutTexture, 6f, (System.Action) null);
      executionerBossIntroRitual.symbols.material.DOFloat(1f, "_Rotation", 7f);
      yield return (object) new WaitForSeconds(3.3f);
      transformAninTotalDuraiton -= 3.3f;
      executionerBossIntroRitual.BloodPortalEffect.transform.DOScale(new Vector3(5.3f, 2f, 5.3f), 2f);
      CameraManager.instance.ShakeCameraForDuration(1f, 1.5f, 10f);
      foreach (PlayerFarming player in PlayerFarming.players)
        MMVibrate.RumbleContinuous(1.5f, 1.75f, player);
      yield return (object) new WaitForSeconds(3.5f);
      transformAninTotalDuraiton -= 3.5f;
      CameraManager.instance.StopAllCoroutines();
      yield return (object) new WaitForEndOfFrame();
      GameManager.GetInstance().OnConversationNext(executionerBossIntroRitual.cultLeader, 12f);
      SimulationManager.Pause();
      executionerBossIntroRitual.distortionObject.DOScale(50f, 5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(executionerBossIntroRitual.\u003CRitualRoutine\u003Eb__31_2));
      AmplifyColorEffect amplifyColorEffect = executionerBossIntroRitual.amplifyColorEffect;
      amplifyColorEffect.LutHighlightTexture = executionerBossIntroRitual.originalHighlightLut;
      amplifyColorEffect.LutTexture = executionerBossIntroRitual.originalLut;
      CameraManager.instance.ShakeCameraForDuration(1.5f, 2f, 0.1f);
      BiomeConstants.Instance.ImpactFrameForDuration();
      MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact);
      yield return (object) new WaitForSeconds(0.25f);
      transformAninTotalDuraiton -= 0.25f;
      executionerBossIntroRitual.cultLeaderSpine.Skeleton.SetSkin("Executioner_Boss");
      yield return (object) new WaitForSeconds(0.15f);
      transformAninTotalDuraiton -= 0.15f;
      foreach (LongGrass longGrass in executionerBossIntroRitual.surroundingGrass)
        longGrass.StartCoroutine((IEnumerator) longGrass.ShakeGrassRoutine(executionerBossIntroRitual.gameObject, 2f));
      yield return (object) new WaitForSeconds(transformAninTotalDuraiton);
      foreach (PlayerFarming player in PlayerFarming.players)
        MMVibrate.StopRumble(player);
      executionerBossIntroRitual.StartCoroutine((IEnumerator) executionerBossIntroRitual.IntroDone());
    }
  }

  public IEnumerator IntroDone()
  {
    this.skippable = false;
    LetterBox.Instance.HideSkipPrompt();
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationNext(this.cultLeader, 16f);
    CameraManager.instance.ShakeCameraForDuration(1.5f, 2f, 0.7f);
    foreach (PlayerFarming player in PlayerFarming.players)
      MMVibrate.RumbleContinuous(1.5f, 1.75f, player);
    yield return (object) new WaitForSeconds(0.8f);
    foreach (PlayerFarming player in PlayerFarming.players)
      MMVibrate.StopRumble(player);
    yield return (object) new WaitForSeconds(0.7f);
    this.executionerBoss.Play();
    this.executionerBossHealth.GetComponent<EnemyBruteBoss>().enabled = true;
    this.executionerBossHealth.untouchable = false;
    yield return (object) new WaitForEndOfFrame();
    this.executionerBossHealth.GetComponent<EnemyBruteBoss>().SpawnEnemy();
    GameManager.GetInstance().OnConversationEnd();
    SimulationManager.Pause();
    this.BloodPortalEffect.transform.DOScale(Vector3.zero, 2f);
  }

  public IEnumerator SpawnSouls(Vector3 position)
  {
    ExecutionerBossIntroRitual executionerBossIntroRitual = this;
    float delay = 0.3f;
    int ParticleCount = 50;
    for (int i = 0; i < ParticleCount; ++i)
    {
      float time = (float) i / (float) ParticleCount;
      delay *= 1f - executionerBossIntroRitual.absorbSoulCurve.Evaluate(time);
      SoulCustomTarget.Create(executionerBossIntroRitual.cameraTarget, new Vector3(position.x, position.y, position.z + 1f), Color.red, (System.Action) null, 0.2f, (float) (100.0 * (1.0 + (double) executionerBossIntroRitual.absorbSoulCurve.Evaluate(time)))).transform.parent = executionerBossIntroRitual.transform;
      yield return (object) new WaitForSeconds(delay);
    }
  }

  [CompilerGenerated]
  public void \u003CRitualRoutine\u003Eb__31_2()
  {
    UnityEngine.Object.Destroy((UnityEngine.Object) this.distortionObject.gameObject);
  }
}
