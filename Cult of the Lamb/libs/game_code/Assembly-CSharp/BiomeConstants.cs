// Decompiled with JetBrains decompiler
// Type: BiomeConstants
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using MMBiomeGeneration;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

#nullable disable
public class BiomeConstants : BaseMonoBehaviour
{
  [SerializeField]
  public GoopFade goopFade;
  [SerializeField]
  public PsychedelicFade psychedelicFade;
  [SerializeField]
  public ImpactFrame impactFrame;
  [SerializeField]
  public VFX_CoOpSpawnScreenEffect coOpSpawnScreenEffect;
  [SerializeField]
  public RawImage IceOverlay;
  public Transform UIDisplayCoop;
  public List<ObjectPoolObject> ObjectPoolObjects = new List<ObjectPoolObject>();
  public static GameObject _ObjectPoolParent;
  public static BiomeConstants Instance;
  [FormerlySerializedAs("_uiProgressIndicator")]
  [FormerlySerializedAs("ProgressIndicator")]
  public UIProgressIndicator ProgressIndicatorTemplate;
  public Shader _uberShaderNoDepth;
  public Shader _uberShader;
  public Material grassNormal;
  public Material grassNoDepth;
  public GameObject ShopKeeperChef;
  public EnableRecieveShadowsSpriteRenderer enableRecieve;
  [SerializeField]
  public ParticleSystem dustCloudParticles;
  public ParticleSystem.MinMaxCurve dustCloudParticlesInitSize;
  public ParticleSystem.MinMaxCurve dustCloudParticlesInitLifetime;
  public ParticleSystem footprintParticles;
  public Vector3 lastPlayerPosition;
  public ParticleSystem.MinMaxCurve footprintParticlesInitSize;
  public ParticleSystem.MinMaxCurve footprintParticlesInitLifetime;
  public PostProcessLayer _postProcessLayer;
  public bool swapShaders;
  public Desaturate_StencilRTMaskPPSSettings desaturateStencil;
  public Vignette vignette;
  public float VignetteDefaultValue = 0.3f;
  public ChromaticAberration chromaticAbberration;
  public float ChromaticAberrationDefaultValue = 0.072f;
  public Bloom bloom;
  public DepthOfField depthoffield;
  public float DepthOfFieldDefaultLength;
  public Coroutine depthOfFieldRoutine;
  public AmplifyPostEffect amplifyPostEffect;
  public bool isBloomInitialized;
  public float HDR_bloomIntensity;
  public float HDR_bloomThreshold;
  public float HDR_bloomSoftKnee;
  public float HDR_bloomClamp;
  public float HDR_bloomDirtIntensity;
  public float noHDR_bloomIntensity = 6f;
  public float noHDR_bloomThreshold = 1f;
  public float noHDR_bloomSoftKnee = 0.19f;
  public float noHDR_bloomClamp = 6.5f;
  public float noHDR_bloomDirtIntensity = 8f;
  public const float FlashTick = 0.12f;
  public GameObject LightningIndicator;
  public List<Texture2D> ColorBlindLUT = new List<Texture2D>();
  public AmplifyColorBase amp;
  public bool TrackDepthOfField;
  public ParticleSystem steppingInWaterParticles;
  public ParticleSystem.EmitParams DustCloudParticlesParams;
  public ParticleSystem.EmitParams emitParams;
  public GameObject DisplacementPrefab;
  public GameObject HitImpactPrefab;
  public SimpleParticleVFX PlayerHitImpactPrefab_Ice;
  public SimpleParticleVFX PlayerHitImpactPrefab_Poison;
  public SimpleParticleVFX PlayerHitImpactPrefabDown;
  public SimpleParticleVFX PlayerHitImpactPrefabDownRight;
  public SimpleParticleVFX PlayerHitImpactPrefabUp;
  public SimpleVFX PlayerHitCritical;
  public GameObject blackSoulPrefab;
  public GameObject sinPrefab;
  [SerializeField]
  public ParticleSystem BloodSplatterGround;
  public ParticleSystem.MinMaxCurve BloodSplatterGroundParticlesInitSize;
  public SimpleVFX BloodImpactPrefab;
  public GameObject HitFX_BlockedRedSmall;
  public Vector3 defaultScale = Vector3.one;
  public GameObject HeartPickUpVFX;
  public GameObject BlockImpactShield;
  public GameObject BlockImpact;
  public GameObject BlockImpactElectric;
  public GameObject ProtectorImpactIcon;
  public GameObject HitFXSoulPrefab;
  public SimpleVFX HitFXPrefab;
  [SerializeField]
  public ParticleSystem EmitRubbleHitFX;
  [SerializeField]
  public ParticleSystem EmitRotRubbleHitFX;
  [SerializeField]
  public ParticleSystem EmitGroundSmashVFX;
  public ParticleSystem.MinMaxCurve EmitGroundSmashVFXInitSize;
  [SerializeField]
  public ParticleSystem DeadBodyExplodeVFX;
  [SerializeField]
  public Particle_Chunk particleChunkPrefab;
  [SerializeField]
  public ParticleSystem burnVFX;
  public GameObject GrenadeBulletImpact_A;
  public GameObject SpawnInWhite;
  public GameObject GroundSmash_Medium;
  public GameObject HitFX_Blocked;
  public SimpleVFX GhostTwirlAttack;
  public SimpleVFX SlamVFX;
  public SimpleVFX craterVFX;
  public SimpleVFX groundParticles;
  public SetSpriteshapeMaterial setSpriteshapeMaterial;
  public GameObject PickUpVFX;
  public GameObject SnowImpactVFX;
  public GameObject SmokeExplosionVFX;
  public ParticleSystem interactionSmokeVFX;
  public GameObject ConfettiVFX;
  public ParticleSystem ParticleChunkSystem_Grass;
  public ParticleSystem ParticleChunkSystem_Wood;
  public ParticleSystem ParticleChunkSystem_Clay;
  public ParticleSystem ParticleChunkSystem_Bones;
  public ParticleSystem ParticleChunkSystem_Grass_2;
  public ParticleSystem ParticleChunkSystem_Grass_3;
  public ParticleSystem ParticleChunkSystem_Grass_4;
  public ParticleSystem ParticleChunkSystem_Grass_5;
  public ParticleSystem ParticleChunkSystem_Grass_6;
  public ParticleSystem ParticleChunkSystem_Rot;
  public ParticleSystem ParticleChunkSystem_Stone;
  public ParticleSystem ParticleChunkSystem_Snow_1;
  public ParticleSystem ParticleChunkSystem_Snow_2;
  public ParticleSystem ParticleChunkSystem_Snow_3;
  public ParticleSystem ParticleChunkSystem_Snow_4;
  public ParticleSystem ParticleChunkSystem_Snow_5;
  public ParticleSystem.TextureSheetAnimationModule ParticleChunkTextures;
  public float RandomVariation = 0.5f;
  public PostProcessVolume ppv;
  public Image timeOfDayOverlay;
  public ParticleSystem fireflies;
  public Color dawnColor;
  public Color dayColor;
  public Color duskColor;
  public Color nightColor;
  public float overlayAmount = 0.9f;
  public float overlayAmountDay = 1f;
  public float lerpTime = 10f;
  public float speed = 1f;
  public GameObject DungeonChallengeRatoo;
  public GameObject BlackHeartDamageIcon;
  public GameObject FireHeartEffectIcon;
  public GameObject IceHeartEffectIcon;
  public GameObject TarotCardDamageIcon;
  public GameObject DamageTextIcon;
  public GameObject TarotCardBreakEffect;
  public ParticleSystem lightningStrike;
  public VFXAbilitySequenceData LightningSequenceData;
  public VFXAbilitySequenceData freezeTimeSequenceData;
  public VFXAbilitySequenceData freezeAllSequenceData;
  public VFXAbilitySequenceData burnAllSequenceData;
  public VFXAbilitySequenceData charmAllSequenceData;
  public VFXAbilitySequenceData frostedEnemiesSequenceData;
  public ParticleSystem puffEffect;
  public ParticleSystem pleasureParticle;
  public ParticleSystem winterSuccess;
  public ParticleSystem roarDistortion;

  public void SetIceOverlayReveal(float value)
  {
    value = Mathf.Clamp(value, 0.0f, 1f);
    this.IceOverlay.material.SetFloat("_OverlayReveal", value);
  }

  public bool ShowingIceOverlay()
  {
    return (double) this.IceOverlay.material.GetFloat("_OverlayReveal") > 0.0;
  }

  public void CoOpScreenEffect()
  {
    if ((UnityEngine.Object) this.coOpSpawnScreenEffect == (UnityEngine.Object) null)
      return;
    this.coOpSpawnScreenEffect.Init();
  }

  public void GoopFadeIn(float _Duration = 2f, float _Delay = 0.0f, bool UseDeltaTime = true)
  {
    this.goopFade.gameObject.SetActive(true);
    this.goopFade.FadeIn(_Duration, _Delay, UseDeltaTime);
  }

  public void GoopFadeOut(float _Duration = 2f, float _Delay = 0.0f, bool UseDeltaTime = true)
  {
    this.goopFade.gameObject.SetActive(true);
    this.goopFade.FadeOut(_Duration, _Delay, UseDeltaTime);
  }

  public void PsychedelicFadeIn(
    float _Duration = 2f,
    float _Delay = 0.0f,
    bool UseDeltaTime = true,
    System.Action onComplete = null)
  {
    if (SettingsManager.Settings.Accessibility.RemoveLightingEffects)
      return;
    this.psychedelicFade.gameObject.SetActive(true);
    this.psychedelicFade.FadeIn(_Duration, _Delay, UseDeltaTime, onComplete);
  }

  public void PsychedelicFadeOut(
    float _Duration = 2f,
    float _Delay = 0.0f,
    bool UseDeltaTime = true,
    System.Action onComplete = null)
  {
    this.psychedelicFade.gameObject.SetActive(true);
    this.psychedelicFade.FadeOut(_Duration, _Delay, UseDeltaTime, onComplete);
  }

  public void ImpactFrameForDuration(float _Duration = 0.2f, float _Delay = 0.0f)
  {
    if (!SettingsManager.Settings.Accessibility.FlashingLights)
      return;
    DeviceLightingManager.TransitionLighting(Color.white, Color.black, 0.5f);
    this.impactFrame.gameObject.SetActive(true);
    this.impactFrame.ShowForDuration(_Duration, _Delay);
  }

  public void ImpactFrameForIn()
  {
    if (!SettingsManager.Settings.Accessibility.FlashingLights)
      return;
    this.impactFrame.gameObject.SetActive(true);
    this.impactFrame.Show();
  }

  public void ImpactFrameForOut()
  {
    if (!SettingsManager.Settings.Accessibility.FlashingLights)
      return;
    this.impactFrame.gameObject.SetActive(true);
    this.impactFrame.Hide();
  }

  public static GameObject ObjectPoolParent
  {
    get
    {
      if ((UnityEngine.Object) BiomeConstants._ObjectPoolParent == (UnityEngine.Object) null)
      {
        BiomeConstants._ObjectPoolParent = new GameObject();
        BiomeConstants._ObjectPoolParent.name = "ObjectPool Parent";
      }
      return BiomeConstants._ObjectPoolParent;
    }
  }

  public void EnableRecieveShadows() => this.enableRecieve.UpdateSpriteRenderers();

  public void OnEnable()
  {
    BiomeConstants.Instance = this;
    this.dustCloudParticles?.gameObject.SetActive(true);
    BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
    TimeManager.OnNewPhaseStarted += new System.Action(this.newTimeOfDay);
    ObjectiveManager.OnObjectiveCompleted += new ObjectiveManager.ObjectiveUpdated(this.ObjectiveManager_OnObjectiveCompleted);
    this.StartCoroutine((IEnumerator) this.SwapShaders());
    Camera main = Camera.main;
    this._postProcessLayer = main.GetComponent<PostProcessLayer>();
    main.allowHDR = true;
    Singleton<AccessibilityManager>.Instance.OnColorblindModeChanged += new System.Action(this.OnColourblindModeUpdated);
    GraphicsSettingsUtilities.OnEnvironmentSettingsChanged += new System.Action(this.SwapShadersStart);
  }

  public void SwapShadersStart() => this.StartCoroutine((IEnumerator) this.SwapShaders());

  public IEnumerator SwapShaders()
  {
    this.swapShaders = false;
    while (SettingsManager.Settings == null)
      yield return (object) new WaitForSeconds(0.1f);
    if (SettingsManager.Settings.Graphics.EnvironmentDetail == 0)
      this.swapShaders = true;
    this.grassNormal.shader = !this.swapShaders ? this._uberShader : this._uberShaderNoDepth;
  }

  public void OnDisable()
  {
    if ((UnityEngine.Object) BiomeConstants.Instance != (UnityEngine.Object) null)
    {
      if ((UnityEngine.Object) BiomeConstants.Instance.grassNormal != (UnityEngine.Object) null)
        BiomeConstants.Instance.grassNormal.SetColor("_Color", new Color(1f, 1f, 1f, 1f));
      if ((UnityEngine.Object) BiomeConstants.Instance.grassNoDepth != (UnityEngine.Object) null)
        BiomeConstants.Instance.grassNoDepth.SetColor("_Color", new Color(1f, 1f, 1f, 1f));
    }
    if ((UnityEngine.Object) BiomeConstants.Instance == (UnityEngine.Object) this)
      BiomeConstants.Instance = (BiomeConstants) null;
    GraphicsSettingsUtilities.OnEnvironmentSettingsChanged -= new System.Action(this.SwapShadersStart);
    this.footprintParticles?.Clear();
    this.dustCloudParticles?.Clear();
    this.dustCloudParticles?.gameObject.SetActive(false);
    this.steppingInWaterParticles?.Clear();
    this.BloodSplatterGround?.Clear();
    this.EmitGroundSmashVFX?.Clear();
    this.EmitRubbleHitFX?.Clear();
    this.EmitRotRubbleHitFX?.Clear();
    this.ParticleChunkSystem_Grass?.Clear();
    this.ParticleChunkSystem_Wood?.Clear();
    this.ParticleChunkSystem_Clay?.Clear();
    this.fireflies?.Clear();
    this.ParticleChunkSystem_Grass_2?.Clear();
    this.ParticleChunkSystem_Grass_3?.Clear();
    this.ParticleChunkSystem_Grass_4?.Clear();
    this.ParticleChunkSystem_Grass_5?.Clear();
    this.ParticleChunkSystem_Grass_6?.Clear();
    this.ParticleChunkSystem_Rot?.Clear();
    this.ParticleChunkSystem_Bones?.Clear();
    this.ParticleChunkSystem_Snow_1?.Clear();
    this.ParticleChunkSystem_Snow_2?.Clear();
    this.ParticleChunkSystem_Snow_3?.Clear();
    this.ParticleChunkSystem_Snow_4?.Clear();
    this.ParticleChunkSystem_Snow_5?.Clear();
    this.ParticleChunkSystem_Stone?.Clear();
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
    TimeManager.OnNewPhaseStarted -= new System.Action(this.newTimeOfDay);
    if (Singleton<AccessibilityManager>.Instance != null)
      Singleton<AccessibilityManager>.Instance.OnColorblindModeChanged -= new System.Action(this.OnColourblindModeUpdated);
    ObjectiveManager.OnObjectiveCompleted -= new ObjectiveManager.ObjectiveUpdated(this.ObjectiveManager_OnObjectiveCompleted);
    GraphicsSettingsUtilities.OnEnvironmentSettingsChanged -= new System.Action(this.SwapShadersStart);
    this.ObjectPoolObjects.Clear();
    BiomeConstants._ObjectPoolParent = (GameObject) null;
    if (!((UnityEngine.Object) BiomeConstants.Instance == (UnityEngine.Object) this))
      return;
    BiomeConstants.Instance = (BiomeConstants) null;
  }

  public void OnDestroy()
  {
    this.dustCloudParticles?.gameObject.SetActive(false);
    TimeManager.OnNewPhaseStarted -= new System.Action(this.newTimeOfDay);
  }

  public void Update()
  {
    if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null))
      return;
    this.lastPlayerPosition = PlayerFarming.Instance.transform.position;
  }

  public void BiomeGenerator_OnBiomeChangeRoom()
  {
    this.footprintParticles.Clear();
    this.ParticleChunkSystem_Grass.Clear();
    this.ParticleChunkSystem_Wood.Clear();
    this.ParticleChunkSystem_Clay.Clear();
    this.EmitGroundSmashVFX.Clear();
    this.EmitRubbleHitFX.Clear();
    this.EmitRotRubbleHitFX.Clear();
    this.ParticleChunkSystem_Grass_2.Clear();
    this.ParticleChunkSystem_Grass_3.Clear();
    this.ParticleChunkSystem_Grass_4.Clear();
    this.ParticleChunkSystem_Grass_5.Clear();
    this.ParticleChunkSystem_Grass_6.Clear();
    this.ParticleChunkSystem_Rot.Clear();
    this.ParticleChunkSystem_Bones.Clear();
    this.ParticleChunkSystem_Stone.Clear();
    this.ParticleChunkSystem_Snow_1?.Clear();
    this.ParticleChunkSystem_Snow_2?.Clear();
    this.ParticleChunkSystem_Snow_3?.Clear();
    this.ParticleChunkSystem_Snow_4?.Clear();
    this.ParticleChunkSystem_Snow_5?.Clear();
  }

  public void Start()
  {
    if ((UnityEngine.Object) this.coOpSpawnScreenEffect != (UnityEngine.Object) null)
      this.coOpSpawnScreenEffect.gameObject.SetActive(false);
    this.goopFade.gameObject.SetActive(false);
    this.psychedelicFade.gameObject.SetActive(false);
    this.impactFrame.gameObject.SetActive(false);
    if ((UnityEngine.Object) this.dustCloudParticles != (UnityEngine.Object) null)
    {
      this.dustCloudParticlesInitSize = this.dustCloudParticles.main.startSize;
      this.dustCloudParticlesInitLifetime = this.dustCloudParticles.main.startLifetime;
    }
    if ((UnityEngine.Object) this.footprintParticles != (UnityEngine.Object) null)
    {
      this.footprintParticlesInitSize = this.footprintParticles.main.startSize;
      this.footprintParticlesInitLifetime = this.footprintParticles.main.startLifetime;
    }
    this.CreatePools();
    this.getPostProcessingSettings();
    if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
      this.lastPlayerPosition = PlayerFarming.Instance.transform.position;
    TimeManager.OnNewPhaseStarted += new System.Action(this.newTimeOfDay);
    this.newTimeOfDay();
  }

  public void CreatePools()
  {
    Scene activeScene = SceneManager.GetActiveScene();
    foreach (ObjectPoolObject objectPoolObject in this.ObjectPoolObjects)
    {
      if (objectPoolObject.PoolingLocation == ObjectPoolObject.PoolLocation.Base && activeScene.name == "Base Biome 1" || objectPoolObject.PoolingLocation == ObjectPoolObject.PoolLocation.Both)
        objectPoolObject.gameObject.CreatePool(objectPoolObject.AmountToPool);
      else if (objectPoolObject.PoolingLocation == ObjectPoolObject.PoolLocation.Dungeon && activeScene.name != "Base Biome 1" || objectPoolObject.PoolingLocation == ObjectPoolObject.PoolLocation.Both)
        objectPoolObject.gameObject.CreatePool(objectPoolObject.AmountToPool);
    }
    if (activeScene.name != "Base Biome 1" && activeScene.name != "Midas Cave" && activeScene.name != "Sozo Cave Location" && activeScene.name != "Mushroom Research Site" && activeScene.name != "Hub-Shore" && activeScene.name != "Dungeon Ratau Home")
    {
      this.BloodImpactPrefab.CreatePool<SimpleVFX>(128 /*0x80*/);
      this.PlayerHitImpactPrefabDown.CreatePool<SimpleParticleVFX>(6);
      this.PlayerHitImpactPrefabDownRight.CreatePool<SimpleParticleVFX>(6);
      this.PlayerHitImpactPrefabUp.CreatePool<SimpleParticleVFX>(6);
      this.HitImpactPrefab.CreatePool(12);
      this.PlayerHitCritical.CreatePool<SimpleVFX>(3);
      this.PlayerHitImpactPrefab_Poison.CreatePool<SimpleParticleVFX>(3);
      this.PlayerHitImpactPrefab_Ice.CreatePool<SimpleParticleVFX>(3);
      this.SmokeExplosionVFX.CreatePool(6);
      this.HitFXPrefab.CreatePool<SimpleVFX>(16 /*0x10*/);
      this.particleChunkPrefab.CreatePool<Particle_Chunk>(100);
      this.PickUpVFX.CreatePool(48 /*0x30*/);
      this.HeartPickUpVFX.CreatePool(1);
      this.HitFX_BlockedRedSmall.CreatePool(128 /*0x80*/);
      this.blackSoulPrefab.CreatePool(64 /*0x40*/);
      this.ProgressIndicatorTemplate.CreatePool<UIProgressIndicator>(3);
      this.SpawnInWhite.CreatePool(8);
      this.HitFXSoulPrefab.CreatePool(64 /*0x40*/);
      Explosion.CreateExplosionPool(16 /*0x10*/);
      LightningRingExplosion.CreateExplosionPool(16 /*0x10*/);
      TrapPoison.Preload(32 /*0x20*/);
      TrapPoison.Preload(32 /*0x20*/, true);
      ShowHPBar.Preload(6);
    }
    if (!(activeScene.name == "Base Biome 1"))
      return;
    this.HitFXSoulPrefab.CreatePool(12);
    this.ProgressIndicatorTemplate.CreatePool<UIProgressIndicator>(20);
  }

  public void CreateBossPools()
  {
    ObjectPool.CreatePool(this.GrenadeBulletImpact_A, 40);
    ObjectPool.CreatePool<Particle_Chunk>(this.particleChunkPrefab, 180, true);
    ObjectPool.CreatePool<SimpleParticleVFX>(this.PlayerHitImpactPrefabDown, 16 /*0x10*/, true);
    ObjectPool.CreatePool<SimpleParticleVFX>(this.PlayerHitImpactPrefabDownRight, 16 /*0x10*/, true);
    ObjectPool.CreatePool<SimpleParticleVFX>(this.PlayerHitImpactPrefabUp, 16 /*0x10*/, true);
  }

  public void getPostProcessingSettings()
  {
    this.ppv.profile.TryGetSettings<Desaturate_StencilRTMaskPPSSettings>(out this.desaturateStencil);
    this.ppv.profile.TryGetSettings<Vignette>(out this.vignette);
    this.ppv.profile.TryGetSettings<ChromaticAberration>(out this.chromaticAbberration);
    this.ppv.profile.TryGetSettings<Bloom>(out this.bloom);
    this.ppv.profile.TryGetSettings<DepthOfField>(out this.depthoffield);
    this.ppv.profile.TryGetSettings<AmplifyPostEffect>(out this.amplifyPostEffect);
    this.updatePostProcessingSettings();
  }

  public bool IsFlashLightsActive => SettingsManager.Settings.Accessibility.FlashingLights;

  public void DepthOfFieldTween(
    float Duration,
    float FocusDistance,
    float aperture,
    float FocusLengthStart,
    float FocusLengthEnd)
  {
    if ((UnityEngine.Object) this.depthoffield == (UnityEngine.Object) null)
      this.getPostProcessingItems();
    if ((UnityEngine.Object) this.depthoffield == (UnityEngine.Object) null)
      return;
    this.depthoffield.enabled.value = SettingsManager.Settings.Graphics.DepthOfField;
    this.depthoffield.focusDistance.value = FocusDistance;
    this.depthoffield.aperture.value = aperture;
    if (this.depthOfFieldRoutine != null)
      this.StopCoroutine(this.depthOfFieldRoutine);
    this.depthOfFieldRoutine = this.StartCoroutine((IEnumerator) this.DepthOfFieldRoutine(Duration, FocusLengthStart, FocusLengthEnd));
  }

  public IEnumerator DepthOfFieldRoutine(
    float Duration,
    float FocusLengthStart,
    float FocusLengthEnd)
  {
    float Progress = 0.0f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      this.depthoffield.focalLength.value = Mathf.SmoothStep(FocusLengthStart, FocusLengthEnd, Progress / Duration);
      yield return (object) null;
    }
    this.depthoffield.focalLength.value = FocusLengthEnd;
  }

  public void ChromaticAbberationTween(float Duration, float StartValue, float EndValue)
  {
    if ((UnityEngine.Object) this.chromaticAbberration == (UnityEngine.Object) null)
      this.getPostProcessingItems();
    if ((UnityEngine.Object) this.chromaticAbberration == (UnityEngine.Object) null)
      return;
    this.chromaticAbberration.enabled.value = SettingsManager.Settings.Graphics.ChromaticAberration;
    this.StartCoroutine((IEnumerator) this.ChromaticAbberationRoutine(Duration, StartValue, EndValue));
  }

  public IEnumerator ChromaticAbberationRoutine(float Duration, float StartValue, float EndValue)
  {
    float Progress = 0.0f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      this.chromaticAbberration.intensity.value = Mathf.SmoothStep(StartValue, EndValue, Progress / Duration);
      yield return (object) null;
    }
    this.chromaticAbberration.intensity.value = EndValue;
  }

  public void VignetteTween(float Duration, float StartValue, float EndValue)
  {
    if ((UnityEngine.Object) this.vignette == (UnityEngine.Object) null)
      this.getPostProcessingItems();
    if ((UnityEngine.Object) this.vignette == (UnityEngine.Object) null)
      return;
    this.vignette.enabled.value = SettingsManager.Settings.Graphics.Vignette;
    this.StartCoroutine((IEnumerator) this.VignetteRoutine(Duration, StartValue, EndValue));
  }

  public IEnumerator VignetteRoutine(float Duration, float StartValue, float EndValue)
  {
    float Progress = 0.0f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      this.vignette.intensity.value = Mathf.SmoothStep(StartValue, EndValue, Progress / Duration);
      yield return (object) null;
    }
    this.vignette.intensity.value = EndValue;
  }

  public void SetAmplifyPostEffect(bool enable) => this.amplifyPostEffect.enabled.value = enable;

  public void DesaturationStencilTween(
    float Duration,
    float StartValue,
    float EndValue,
    float BrightnessStart,
    float BrightnessEnd)
  {
    if ((UnityEngine.Object) this.desaturateStencil == (UnityEngine.Object) null)
      this.getPostProcessingItems();
    if ((UnityEngine.Object) this.desaturateStencil == (UnityEngine.Object) null)
      return;
    this.StopCoroutine((IEnumerator) this.DesaturationStencilRoutine(Duration, StartValue, EndValue, BrightnessStart, BrightnessEnd));
    this.desaturateStencil.enabled.value = true;
    this.StartCoroutine((IEnumerator) this.DesaturationStencilRoutine(Duration, StartValue, EndValue, BrightnessStart, BrightnessEnd));
  }

  public IEnumerator DesaturationStencilRoutine(
    float Duration,
    float StartValue,
    float EndValue,
    float BrightnessStart,
    float BrightnessEnd)
  {
    float Progress = 0.0f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      this.desaturateStencil._Intensity.value = Mathf.SmoothStep(StartValue, EndValue, Progress / Duration);
      this.desaturateStencil._Brightness.value = Mathf.SmoothStep(BrightnessStart, BrightnessEnd, Progress / Duration);
      yield return (object) null;
    }
    this.desaturateStencil._Intensity.value = EndValue;
    this.desaturateStencil._Brightness.value = BrightnessEnd;
    if ((double) this.desaturateStencil._Intensity.value == 0.0)
      this.desaturateStencil.enabled.value = false;
  }

  public void DesaturationStencil(
    float norm,
    float StartValue,
    float EndValue,
    float BrightnessStart,
    float BrightnessEnd)
  {
    if ((UnityEngine.Object) this.desaturateStencil == (UnityEngine.Object) null)
      this.getPostProcessingItems();
    if ((UnityEngine.Object) this.desaturateStencil == (UnityEngine.Object) null)
      return;
    this.desaturateStencil._Intensity.value = Mathf.SmoothStep(StartValue, EndValue, norm);
    this.desaturateStencil._Brightness.value = Mathf.SmoothStep(BrightnessStart, BrightnessEnd, norm);
    this.desaturateStencil.enabled.value = (double) this.desaturateStencil._Intensity.value != 0.0;
  }

  public void DisableIndicators()
  {
    foreach (UIProgressIndicator progressIndicator in UIProgressIndicator.ProgressIndicators)
      progressIndicator.Hide(0.0f, 0.0f);
  }

  public void getColorBlind()
  {
    if (Singleton<AccessibilityManager>.Instance.ColorblindMode != 0 && Singleton<AccessibilityManager>.Instance.ColorblindMode - 1 < this.ColorBlindLUT.Count)
    {
      if ((UnityEngine.Object) this.amp == (UnityEngine.Object) null)
        this.amp = Camera.main.gameObject.AddComponent<AmplifyColorBase>();
      this.amp.LutTexture = (Texture) this.ColorBlindLUT[Singleton<AccessibilityManager>.Instance.ColorblindMode - 1];
      this.amp.QualityLevel = AmplifyColor.Quality.Mobile;
    }
    else
    {
      if (!((UnityEngine.Object) this.amp != (UnityEngine.Object) null))
        return;
      UnityEngine.Object.Destroy((UnityEngine.Object) this.amp);
    }
  }

  public void OnColourblindModeUpdated()
  {
    this.getColorBlind();
    this.updatePostProcessing();
  }

  public void getPostProcessingItems()
  {
    this.ppv.profile.TryGetSettings<Vignette>(out this.vignette);
    this.ppv.profile.TryGetSettings<ChromaticAberration>(out this.chromaticAbberration);
    this.ppv.profile.TryGetSettings<Bloom>(out this.bloom);
    this.ppv.profile.TryGetSettings<DepthOfField>(out this.depthoffield);
    this.ppv.profile.TryGetSettings<AmplifyPostEffect>(out this.amplifyPostEffect);
    this.ppv.profile.TryGetSettings<Desaturate_StencilRTMaskPPSSettings>(out this.desaturateStencil);
    this.updatePostProcessing();
  }

  public void updatePostProcessingSettings()
  {
    if ((UnityEngine.Object) this.vignette == (UnityEngine.Object) null || (UnityEngine.Object) this.chromaticAbberration == (UnityEngine.Object) null || (UnityEngine.Object) this.bloom == (UnityEngine.Object) null || (UnityEngine.Object) this.depthoffield == (UnityEngine.Object) null)
      this.getPostProcessingItems();
    if ((UnityEngine.Object) this.vignette == (UnityEngine.Object) null || (UnityEngine.Object) this.chromaticAbberration == (UnityEngine.Object) null || (UnityEngine.Object) this.bloom == (UnityEngine.Object) null || (UnityEngine.Object) this.depthoffield == (UnityEngine.Object) null)
      return;
    Debug.Log((object) "PostProcessingUpdate");
    if (!this.isBloomInitialized && (UnityEngine.Object) this.bloom != (UnityEngine.Object) null)
    {
      this.isBloomInitialized = true;
      this.HDR_bloomIntensity = (float) (ParameterOverride<float>) this.bloom.intensity;
      this.HDR_bloomThreshold = (float) (ParameterOverride<float>) this.bloom.threshold;
      this.HDR_bloomSoftKnee = (float) (ParameterOverride<float>) this.bloom.softKnee;
      this.HDR_bloomClamp = (float) (ParameterOverride<float>) this.bloom.clamp;
      this.HDR_bloomDirtIntensity = (float) (ParameterOverride<float>) this.bloom.dirtIntensity;
    }
    this.bloom.enabled.value = SettingsManager.Settings.Graphics.Bloom;
    this.chromaticAbberration.enabled.value = SettingsManager.Settings.Graphics.ChromaticAberration;
    this.vignette.enabled.value = SettingsManager.Settings.Graphics.Vignette;
    this.depthoffield.enabled.value = SettingsManager.Settings.Graphics.DepthOfField;
    this._postProcessLayer.antialiasingMode = GraphicsSettingsUtilities.AntiAliasingModeFromBool(SettingsManager.Settings.Graphics.AntiAliasing);
    this.TrackDepthOfField = false;
  }

  public IEnumerator TrackDepthOfFieldDistance()
  {
    while ((UnityEngine.Object) this.depthoffield != (UnityEngine.Object) null && this.TrackDepthOfField)
    {
      this.depthoffield.focusDistance.value = Vector3.Distance(Camera.main.transform.position, PlayerFarming.Instance.gameObject.transform.position);
      yield return (object) null;
    }
  }

  public void updatePostProcessing() => this.updatePostProcessingSettings();

  public void EmitSteppingInWaterParticles(Vector3 worldPos, int numParticles = 3)
  {
    if ((UnityEngine.Object) this.steppingInWaterParticles == (UnityEngine.Object) null)
    {
      if (!((UnityEngine.Object) this.gameObject != (UnityEngine.Object) null))
        return;
      Debug.LogWarning((object) "steppingInWaterParticles property is missing!", (UnityEngine.Object) this.gameObject);
    }
    else
    {
      this.emitParams = new ParticleSystem.EmitParams();
      this.emitParams.position = worldPos;
      this.emitParams.applyShapeToPosition = true;
      this.steppingInWaterParticles.Emit(this.emitParams, numParticles);
    }
  }

  public void EmitDustCloudParticles(
    Vector3 worldPos,
    int numParticles = 3,
    float multiplier = 1f,
    bool ignoreTimescale = false)
  {
    if (!this.dustCloudParticles.gameObject.activeSelf || !((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null))
      return;
    if ((UnityEngine.Object) this.dustCloudParticles == (UnityEngine.Object) null)
    {
      if (!((UnityEngine.Object) this.gameObject != (UnityEngine.Object) null))
        return;
      Debug.LogWarning((object) "dustCloudPFX property is missing!", (UnityEngine.Object) this.gameObject);
    }
    else
    {
      this.DustCloudParticlesParams = new ParticleSystem.EmitParams();
      this.DustCloudParticlesParams.position = worldPos;
      this.DustCloudParticlesParams.applyShapeToPosition = true;
      if ((double) multiplier != 1.0)
      {
        this.DustCloudParticlesParams.startSize = UnityEngine.Random.Range(this.dustCloudParticlesInitSize.constantMin, this.dustCloudParticlesInitSize.constantMax) * multiplier;
        this.DustCloudParticlesParams.startLifetime = UnityEngine.Random.Range(this.dustCloudParticlesInitLifetime.constantMin, this.dustCloudParticlesInitLifetime.constantMax) * multiplier;
      }
      this.dustCloudParticles.main.useUnscaledTime = ignoreTimescale;
      this.dustCloudParticles.Emit(this.DustCloudParticlesParams, numParticles);
    }
  }

  public void EmitFootprintsParticles(Vector3 worldPos, Color color, float multiplier = 1f)
  {
    if ((UnityEngine.Object) this.footprintParticles == (UnityEngine.Object) null)
    {
      Debug.LogWarning((object) "footprintParticles property is missing!", (UnityEngine.Object) this.gameObject);
    }
    else
    {
      this.emitParams = new ParticleSystem.EmitParams();
      this.emitParams.position = new Vector3(worldPos.x, worldPos.y, -1f / 1000f);
      this.emitParams.applyShapeToPosition = true;
      this.emitParams.startColor = (Color32) color;
      if ((double) multiplier != 1.0)
        this.emitParams.startSize = this.footprintParticlesInitSize.constant * multiplier;
      this.footprintParticles.Emit(this.emitParams, 1);
    }
  }

  public void EmitDisplacementEffect(Vector3 Position)
  {
    Position.z = -2f;
    UnityEngine.Object.Instantiate<GameObject>(this.DisplacementPrefab, Position, Quaternion.identity);
  }

  public void EmitDisplacementEffectScale(Vector3 Position, float Scale)
  {
    Position.z = -2f;
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.DisplacementPrefab, Position, Quaternion.identity);
    if (!((UnityEngine.Object) gameObject != (UnityEngine.Object) null))
      return;
    foreach (DOTweenAnimation component in gameObject.GetComponents<DOTweenAnimation>())
    {
      DOTweenAnimation t = component;
      if (t.animationType == DOTweenAnimation.AnimationType.Scale)
      {
        original = t.endValueFloat;
        t.DOKillById(t.id);
        gameObject.transform.DOScale(t.endValueFloat * Scale, t.duration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(t.easeType).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => t.endValueFloat = original));
      }
    }
  }

  public void EmitHitImpactEffect(Vector3 Position, float Angle, bool useDeltaTime = true)
  {
    SimpleVFX component1 = this.HitImpactPrefab.Spawn(BiomeConstants.ObjectPoolParent.transform, Position, Quaternion.identity).GetComponent<SimpleVFX>();
    component1.gameObject.SetActive(true);
    if ((UnityEngine.Object) component1.Spine != (UnityEngine.Object) null)
    {
      component1.Spine.UseDeltaTime = useDeltaTime;
      SkeletonRenderer component2 = component1.Spine.GetComponent<SkeletonRenderer>();
      Spine.Slot slot1 = component2.Skeleton.FindSlot("SlashPlayer_0");
      Spine.Slot slot2 = component2.Skeleton.FindSlot("Swipe_1");
      Spine.Slot slot3 = component2.Skeleton.FindSlot("Swipe_2");
      Spine.Slot slot4 = component2.Skeleton.FindSlot("Swipe 3");
      slot1.SetColor(StaticColors.GreenColor);
      slot2.SetColor(StaticColors.GreenColor);
      slot3.SetColor(StaticColors.GreenColor);
      Color greenColor = StaticColors.GreenColor;
      slot4.SetColor(greenColor);
    }
    component1.Play(Position, Angle);
  }

  public void PlayerEmitHitImpactEffect(
    Vector3 Position,
    float Angle,
    bool useDeltaTime = true,
    Color color = default (Color),
    float scale = 1f,
    bool crit = false,
    PlayerFarming playerFarming = null)
  {
    SimpleParticleVFX simpleParticleVfx = (SimpleParticleVFX) null;
    if ((UnityEngine.Object) playerFarming == (UnityEngine.Object) null)
      playerFarming = PlayerFarming.Instance;
    if (playerFarming.CurrentWeaponInfo != null && (bool) (UnityEngine.Object) playerFarming.CurrentWeaponInfo.WeaponData && EquipmentManager.IsPoisonWeapon(playerFarming.CurrentWeaponInfo.WeaponData.EquipmentType))
      simpleParticleVfx = this.PlayerHitImpactPrefab_Poison.Spawn<SimpleParticleVFX>(BiomeConstants.ObjectPoolParent.transform, Position, Quaternion.identity);
    else if (playerFarming.currentCurse == EquipmentType.Tentacles_Ice || playerFarming.currentCurse == EquipmentType.EnemyBlast_Ice || playerFarming.currentCurse == EquipmentType.MegaSlash_Ice)
    {
      simpleParticleVfx = this.PlayerHitImpactPrefab_Ice.Spawn<SimpleParticleVFX>(BiomeConstants.ObjectPoolParent.transform, Position, Quaternion.identity);
    }
    else
    {
      switch (PlayerWeapon.AttackSwipeDirection)
      {
        case PlayerWeapon.AttackSwipeDirections.Down:
          if ((UnityEngine.Object) this.PlayerHitImpactPrefabDown != (UnityEngine.Object) null)
          {
            simpleParticleVfx = this.PlayerHitImpactPrefabDown.Spawn<SimpleParticleVFX>(BiomeConstants.ObjectPoolParent.transform, Position, Quaternion.identity);
            break;
          }
          break;
        case PlayerWeapon.AttackSwipeDirections.DownRight:
          if ((UnityEngine.Object) this.PlayerHitImpactPrefabDownRight != (UnityEngine.Object) null)
          {
            simpleParticleVfx = this.PlayerHitImpactPrefabDownRight.Spawn<SimpleParticleVFX>(BiomeConstants.ObjectPoolParent.transform, Position, Quaternion.identity);
            break;
          }
          break;
        case PlayerWeapon.AttackSwipeDirections.Up:
          if ((UnityEngine.Object) this.PlayerHitImpactPrefabUp != (UnityEngine.Object) null)
          {
            simpleParticleVfx = this.PlayerHitImpactPrefabUp.Spawn<SimpleParticleVFX>(BiomeConstants.ObjectPoolParent.transform, Position, Quaternion.identity);
            break;
          }
          break;
      }
    }
    if ((UnityEngine.Object) simpleParticleVfx == (UnityEngine.Object) null)
    {
      Debug.Log((object) ("Oh no! vfx was not set correctly! " + PlayerWeapon.AttackSwipeDirection.ToString()));
    }
    else
    {
      simpleParticleVfx.transform.localScale = Vector3.one * scale;
      if ((UnityEngine.Object) simpleParticleVfx.particlesToPlay[0] != (UnityEngine.Object) null)
        simpleParticleVfx.particlesToPlay[0].main.useUnscaledTime = useDeltaTime;
      Color red = Color.red;
      simpleParticleVfx.gameObject.SetActive(true);
      simpleParticleVfx.Play(Position, Angle);
      if (!crit)
        return;
      this.PlayerEmitHitCriticalImpactEffect(Position, Angle, useDeltaTime, color, 0.5f);
      AudioManager.Instance.PlayOneShot("event:/weapon/crit_hit", Position);
    }
  }

  public void PlayerEmitHitCriticalImpactEffect(
    Vector3 Position,
    float Angle,
    bool useDeltaTime = true,
    Color color = default (Color),
    float scale = 1f)
  {
    SimpleVFX simpleVfx = this.PlayerHitCritical.Spawn<SimpleVFX>(BiomeConstants.ObjectPoolParent.transform, Position, Quaternion.identity);
    if ((UnityEngine.Object) simpleVfx == (UnityEngine.Object) null)
    {
      Debug.Log((object) ("Oh no! vfx was not set correctly! " + PlayerWeapon.AttackSwipeDirection.ToString()));
    }
    else
    {
      simpleVfx.transform.localScale = Vector3.one * scale;
      if ((UnityEngine.Object) simpleVfx.particlesToPlay[0] != (UnityEngine.Object) null)
        simpleVfx.particlesToPlay[0].main.useUnscaledTime = useDeltaTime;
      simpleVfx.gameObject.SetActive(true);
      simpleVfx.Play(Position, Angle);
    }
  }

  public GameObject BloodSplatterPrefab
  {
    get
    {
      return (UnityEngine.Object) BiomeGenerator.Instance == (UnityEngine.Object) null ? (GameObject) null : BiomeGenerator.Instance.CurrentRoom.generateRoom.BloodSplatterPrefab;
    }
  }

  public BlackSoul SpawnBlackSouls(Vector3 Position, Transform Layer, float Angle, bool simulated = false)
  {
    BlackSoul component = this.blackSoulPrefab.Spawn(Layer, Position, Quaternion.identity).GetComponent<BlackSoul>();
    component.Simulated = simulated;
    component.gameObject.SetActive(true);
    component.SetAngle(Angle);
    return component;
  }

  public BlackSoul SpawnSin(Vector3 Position, Transform Layer, float Angle, bool simulated = false)
  {
    BlackSoul component = this.sinPrefab.Spawn(Layer, Position, Quaternion.identity).GetComponent<BlackSoul>();
    component.Simulated = simulated;
    component.gameObject.SetActive(true);
    component.Sin = true;
    component.SetAngle(Angle);
    return component;
  }

  public void EmitBloodSplatter(Vector3 hitPos, Vector3 direction, Color color)
  {
  }

  public void EmitBloodDieEffect(Vector3 hitPos, Vector3 direction, Color color)
  {
  }

  public void EmitBloodSplatterGroundParticles(
    Vector3 worldPos,
    Vector3 Velocity,
    Color color,
    int numParticles = 1,
    float multiplier = 1f)
  {
    ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
    emitParams.position = worldPos;
    emitParams.applyShapeToPosition = true;
    emitParams.startColor = (Color32) color;
    int count = UnityEngine.Random.Range(1, 3);
    if (!((UnityEngine.Object) this.BloodSplatterPrefab != (UnityEngine.Object) null))
      return;
    this.BloodSplatterPrefab.GetComponent<ParticleSystem>().Emit(emitParams, count);
    this.BloodSplatterPrefab.GetComponent<global::BloodSplatterPrefab>().NewParticle();
  }

  public void EmitBloodImpact(
    Vector3 Position,
    float Angle,
    string skin,
    string animation = null,
    bool useDeltaTime = true)
  {
    SimpleVFX simpleVfx = this.BloodImpactPrefab.Spawn<SimpleVFX>((Transform) null, Position, Quaternion.identity);
    if ((UnityEngine.Object) simpleVfx.Spine != (UnityEngine.Object) null)
      simpleVfx.Spine.UseDeltaTime = useDeltaTime;
    if (animation == null)
      simpleVfx.Play(Position, Angle);
    else
      simpleVfx.Play(Position, Angle, animation);
    if (skin == null)
      skin = "black";
    simpleVfx.Spine.skeleton.SetSkin(skin);
  }

  public void EmitHitFX_BlockedRedSmall(Vector3 Position, Quaternion Angle, Vector3 scale)
  {
    GameObject gameObject = this.HitFX_BlockedRedSmall.Spawn(BiomeConstants.ObjectPoolParent.transform, Position, Angle);
    if (!(scale != Vector3.one))
      return;
    gameObject.transform.localScale = scale;
  }

  public void EmitHeartPickUpVFX(
    Vector3 Position,
    float Angle,
    string skin,
    string animationName,
    bool useDeltaTime = true)
  {
    SimpleVFX component = this.HeartPickUpVFX.Spawn(BiomeConstants.ObjectPoolParent.transform, Position, Quaternion.identity).GetComponent<SimpleVFX>();
    component.gameObject.SetActive(true);
    component.Play(Position, Angle, animationName);
    component.Spine.skeleton.SetSkin(skin);
    component.Spine.UseDeltaTime = useDeltaTime;
    component.Spine.ForceVisible = true;
  }

  public void EmitBlockImpact(
    Vector3 Position,
    float Angle,
    Transform parentTransform = null,
    string animation = null)
  {
    SimpleVFX component1 = this.BlockImpactShield.Spawn(BiomeConstants.ObjectPoolParent.transform, Position, Quaternion.identity).GetComponent<SimpleVFX>();
    SimpleVFX component2 = this.BlockImpact.Spawn(BiomeConstants.ObjectPoolParent.transform, Position, Quaternion.identity).GetComponent<SimpleVFX>();
    if (animation != null)
    {
      component2.Play(Position, Angle);
      component1.Play(Position, 0.0f, animation);
    }
    else
    {
      component2.Play(Position, Angle);
      component1.Play(Position);
      AudioManager.Instance.PlayOneShot("event:/dlc/combat/defended_impact", Position);
    }
    if (!((UnityEngine.Object) parentTransform != (UnityEngine.Object) null))
      return;
    component1.gameObject.transform.SetParent(parentTransform);
    component2.gameObject.transform.SetParent(parentTransform);
  }

  public void EmitBlockElectricImpact(
    Vector3 Position,
    float Angle,
    Transform parentTransform = null,
    string animation = null)
  {
    ParticleSystem component = this.BlockImpactElectric.Spawn(BiomeConstants.ObjectPoolParent.transform, Position, Quaternion.identity).GetComponent<ParticleSystem>();
    component.Play();
    if (!((UnityEngine.Object) parentTransform != (UnityEngine.Object) null))
      return;
    component.gameObject.transform.SetParent(parentTransform);
  }

  public void EmitProtectorImpact(Vector3 Position, float Angle)
  {
    SimpleVFX component = this.ProtectorImpactIcon.Spawn(BiomeConstants.ObjectPoolParent.transform, Position, Quaternion.identity).GetComponent<SimpleVFX>();
    this.BlockImpact.Spawn(BiomeConstants.ObjectPoolParent.transform, Position, Quaternion.identity).GetComponent<SimpleVFX>().Play(Position, Angle);
    Vector3 Position1 = Position;
    component.Play(Position1);
  }

  public void EmitHitVFXSoul(Vector3 Position, Quaternion Angle)
  {
    this.HitFXSoulPrefab.Spawn(BiomeConstants.ObjectPoolParent.transform, Position, Angle).GetComponent<ParticleSystem>().Play();
  }

  public void EmitHitVFX(Vector3 Position, float Angle, string animationName)
  {
    this.HitFXPrefab.Spawn<SimpleVFX>(BiomeConstants.ObjectPoolParent.transform, Position, Quaternion.identity).Play(Position, Angle, animationName);
  }

  public void EmitRubbleHitFXParticles(Vector3 worldPos, float multiplier = 1f)
  {
    this.EmitRubbleHitFX.Emit(new ParticleSystem.EmitParams()
    {
      position = worldPos
    }, 1);
  }

  public void EmitRotRubbleHitFXParticles(Vector3 worldPos, float multiplier = 1f)
  {
    this.EmitRotRubbleHitFX.Emit(new ParticleSystem.EmitParams()
    {
      position = worldPos
    }, 1);
  }

  public void EmitGroundSmashVFXParticles(Vector3 worldPos, float multiplier = 1f)
  {
    ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
    if ((double) multiplier != 1.0)
      emitParams.startSize = UnityEngine.Random.Range(this.EmitGroundSmashVFXInitSize.constantMin, this.EmitGroundSmashVFXInitSize.constantMax) * multiplier;
    emitParams.position = worldPos;
    emitParams.applyShapeToPosition = true;
    this.EmitGroundSmashVFX.Emit(emitParams, 1);
  }

  public void EmitDeadBodyExplodeVFX(Vector3 worldPos)
  {
    if ((UnityEngine.Object) this.DeadBodyExplodeVFX == (UnityEngine.Object) null)
      return;
    this.DeadBodyExplodeVFX.transform.position = worldPos;
    this.DeadBodyExplodeVFX.Play();
  }

  public Particle_Chunk SpawnParticleChunk(Vector3 position)
  {
    return this.particleChunkPrefab.Spawn<Particle_Chunk>(BiomeConstants.ObjectPoolParent.transform, position, Quaternion.identity);
  }

  public ParticleSystem EmitBurnVFX(Transform parent)
  {
    ParticleSystem particleSystem = this.burnVFX.Spawn<ParticleSystem>(parent);
    particleSystem.transform.localPosition = Vector3.zero;
    return particleSystem;
  }

  public void EmitGhostTwirlAttack(Vector3 Position, float Angle)
  {
    Debug.Log((object) nameof (EmitGhostTwirlAttack));
    this.GhostTwirlAttack.gameObject.SetActive(true);
    Vector3 vector3 = new Vector3(1.5f * Mathf.Cos(Angle * ((float) Math.PI / 180f)), 1.5f * Mathf.Sin(Angle * ((float) Math.PI / 180f)));
    this.GhostTwirlAttack.Play(Position + vector3);
    this.SlamVFX.gameObject.SetActive(true);
    this.SlamVFX.Play(Position + vector3);
    this.craterVFX.Play(Position + vector3);
    this.groundParticles.Play(Position + vector3);
    CameraManager.shakeCamera(2f);
  }

  public void EmitHammerEffects(Vector3 Position, float Angle, bool playSFX = true)
  {
    CameraManager.instance.ShakeCameraForDuration(1f, 1.2f, 0.3f);
    Vector3 vector3 = new Vector3(1.5f * Mathf.Cos(Angle * ((float) Math.PI / 180f)), 1.5f * Mathf.Sin(Angle * ((float) Math.PI / 180f)));
    this.SlamVFX.gameObject.SetActive(true);
    this.SlamVFX.transform.localScale = Vector3.one;
    this.craterVFX.transform.localScale = Vector3.one;
    this.groundParticles.transform.localScale = Vector3.one;
    this.SlamVFX.Play(Position + vector3);
    this.craterVFX.Play(Position + vector3);
    this.groundParticles.Play(Position + vector3);
    if (!playSFX)
      return;
    AudioManager.Instance.PlayOneShot("event:/enemy/tunnel_worm/tunnel_worm_burst_out_of_ground", Position + vector3);
  }

  public void EmitHammerEffects(
    Vector3 Position,
    float cameraShakeIntensityMin = 1f,
    float cameraShakeIntensityMax = 1.2f,
    float cameraShakeDuration = 0.3f,
    bool cameraShakeStacking = true,
    float scale = 1f,
    bool playSFX = true)
  {
    CameraManager.instance.ShakeCameraForDuration(cameraShakeIntensityMin, cameraShakeIntensityMax, cameraShakeDuration, cameraShakeStacking);
    Vector3 zero = Vector3.zero;
    this.SlamVFX.transform.localScale = Vector3.one * scale;
    this.craterVFX.transform.localScale = Vector3.one * scale;
    this.groundParticles.transform.localScale = Vector3.one * scale;
    this.SlamVFX.gameObject.SetActive(true);
    this.SlamVFX.Play(Position + zero);
    this.craterVFX.Play(Position + zero);
    this.groundParticles.Play(Position + zero);
    if (!playSFX)
      return;
    AudioManager.Instance.PlayOneShot("event:/enemy/tunnel_worm/tunnel_worm_burst_out_of_ground", Position + zero);
  }

  public void EmitHammerEffectsInstantiated(
    Vector3 Position,
    float cameraShakeIntensityMin = 1f,
    float cameraShakeIntensityMax = 1.2f,
    float cameraShakeDuration = 0.3f,
    bool cameraShakeStacking = true,
    float scale = 1f,
    bool playSFX = true,
    string customSFX = "")
  {
    CameraManager.instance.ShakeCameraForDuration(cameraShakeIntensityMin, cameraShakeIntensityMax, cameraShakeDuration, cameraShakeStacking);
    Vector3 zero = Vector3.zero;
    SimpleVFX simpleVfx1 = this.SlamVFX.Spawn<SimpleVFX>(Position);
    SimpleVFX simpleVfx2 = this.craterVFX.Spawn<SimpleVFX>(Position);
    SimpleVFX simpleVfx3 = this.groundParticles.Spawn<SimpleVFX>(Position);
    simpleVfx1.transform.localScale = Vector3.one * scale;
    simpleVfx2.transform.localScale = Vector3.one * scale;
    simpleVfx3.transform.localScale = Vector3.one * scale;
    simpleVfx1.gameObject.SetActive(true);
    simpleVfx1.Play(Position + zero);
    simpleVfx2.Play(Position + zero);
    simpleVfx3.Play(Position + zero);
    if (!playSFX)
      return;
    AudioManager.Instance.PlayOneShot(string.IsNullOrEmpty(customSFX) ? "event:/enemy/tunnel_worm/tunnel_worm_burst_out_of_ground" : customSFX, Position + zero);
  }

  public void ShakeCamera() => CameraManager.shakeCamera(2f);

  public void EmitSlamVFX(Vector3 Position) => this.SlamVFX.Play(Position);

  public void EmitPickUpVFX(Vector3 Position, string animation = null)
  {
    SimpleVFX component = this.PickUpVFX.Spawn(BiomeConstants.ObjectPoolParent.transform, Position, Quaternion.identity).GetComponent<SimpleVFX>();
    component.gameObject.SetActive(true);
    if (animation == null)
      component.Play(Position);
    else
      component.Play(Position, 0.0f, animation);
  }

  public void EmitSnowImpactVFX(Vector3 Position, float scale = 1f)
  {
    SimpleVFX component = this.SnowImpactVFX.Spawn(BiomeConstants.ObjectPoolParent.transform, Position, Quaternion.identity).GetComponent<SimpleVFX>();
    component.gameObject.SetActive(true);
    component.transform.localScale = new Vector3(scale, scale, scale);
    component.Play(Position);
  }

  public SimpleVFX EmitSmokeExplosionVFX(Vector3 Position)
  {
    SimpleVFX component = this.SmokeExplosionVFX.Spawn(BiomeConstants.ObjectPoolParent.transform, Position, Quaternion.identity).GetComponent<SimpleVFX>();
    component.gameObject.SetActive(true);
    component.transform.localScale = Vector3.one;
    component.Play(Position);
    return component;
  }

  public void EmitSmokeExplosionVFX(Vector3 Position, Color color)
  {
    SimpleVFX component = this.SmokeExplosionVFX.Spawn(BiomeConstants.ObjectPoolParent.transform, Position, Quaternion.identity).GetComponent<SimpleVFX>();
    component.gameObject.SetActive(true);
    foreach (ParticleSystem particleSystem in component.particlesToPlay)
      particleSystem.main.startColor = (ParticleSystem.MinMaxGradient) color;
    component.Play(Position);
  }

  public void EmitSmokeInteractionVFX(Vector3 position, Vector3 scale)
  {
    Debug.DrawLine(position - scale * 0.5f, position + scale * 0.5f, Color.magenta, 5f);
    this.interactionSmokeVFX.transform.localScale = scale;
    this.interactionSmokeVFX.Emit(new ParticleSystem.EmitParams()
    {
      position = position,
      applyShapeToPosition = true
    }, (int) ((double) Mathf.Max(scale.x, scale.y) * 5.0));
  }

  public void EmitSmokeInteractionVFXForDuration(
    float duration,
    float interval,
    Vector3 position,
    Vector3 scale)
  {
    this.StartCoroutine((IEnumerator) this.SmokeInteractionVFXRoutine(duration, interval, position, scale));
  }

  public IEnumerator SmokeInteractionVFXRoutine(
    float duration,
    float interval,
    Vector3 position,
    Vector3 scale)
  {
    for (float elapsed = 0.0f; (double) elapsed < (double) duration; elapsed += interval)
    {
      this.EmitSmokeInteractionVFX(position, scale);
      yield return (object) new WaitForSeconds(interval);
    }
  }

  public void EmitConfettiVFX(Vector3 Position)
  {
    SimpleVFX component = this.ConfettiVFX.Spawn(BiomeConstants.ObjectPoolParent.transform, Position, Quaternion.identity).GetComponent<SimpleVFX>();
    component.gameObject.SetActive(true);
    component.Play(Position);
  }

  public void EmitParticleChunk(
    BiomeConstants.TypeOfParticle type,
    Vector3 worldPos,
    Vector3 Velocity,
    int numParticles = 3,
    float multiplier = 1f)
  {
    Vector3 vector3 = new Vector3(Velocity.x / 4f, Velocity.y / 4f, -2f) + new Vector3(UnityEngine.Random.Range(-this.RandomVariation, this.RandomVariation), UnityEngine.Random.Range(-this.RandomVariation, this.RandomVariation), UnityEngine.Random.Range(-this.RandomVariation, this.RandomVariation) * 2f);
    this.emitParams = new ParticleSystem.EmitParams();
    this.emitParams.position = worldPos;
    this.emitParams.applyShapeToPosition = true;
    this.emitParams.velocity = vector3;
    switch (type)
    {
      case BiomeConstants.TypeOfParticle.wood:
        this.ParticleChunkSystem_Wood.Emit(this.emitParams, numParticles);
        break;
      case BiomeConstants.TypeOfParticle.clay:
        this.ParticleChunkSystem_Clay.Emit(this.emitParams, numParticles);
        break;
      case BiomeConstants.TypeOfParticle.grass:
        this.ParticleChunkSystem_Grass.Emit(this.emitParams, numParticles);
        break;
      case BiomeConstants.TypeOfParticle.stone:
        this.ParticleChunkSystem_Stone.Emit(this.emitParams, numParticles);
        break;
      case BiomeConstants.TypeOfParticle.bone:
        this.ParticleChunkSystem_Bones.Emit(this.emitParams, numParticles);
        break;
      case BiomeConstants.TypeOfParticle.grass_2:
        this.ParticleChunkSystem_Grass_2.Emit(this.emitParams, numParticles);
        break;
      case BiomeConstants.TypeOfParticle.grass_3:
        this.ParticleChunkSystem_Grass_3.Emit(this.emitParams, numParticles);
        break;
      case BiomeConstants.TypeOfParticle.grass_4:
        this.ParticleChunkSystem_Grass_4.Emit(this.emitParams, numParticles);
        break;
      case BiomeConstants.TypeOfParticle.grass_5:
        this.ParticleChunkSystem_Grass_5.Emit(this.emitParams, numParticles);
        break;
      case BiomeConstants.TypeOfParticle.snow_1:
        this.ParticleChunkSystem_Snow_1.Emit(this.emitParams, numParticles);
        break;
      case BiomeConstants.TypeOfParticle.snow_2:
        this.ParticleChunkSystem_Snow_2.Emit(this.emitParams, numParticles);
        break;
      case BiomeConstants.TypeOfParticle.snow_3:
        this.ParticleChunkSystem_Snow_3.Emit(this.emitParams, numParticles);
        break;
      case BiomeConstants.TypeOfParticle.snow_4:
        this.ParticleChunkSystem_Snow_4.Emit(this.emitParams, numParticles);
        break;
      case BiomeConstants.TypeOfParticle.snow_5:
        this.ParticleChunkSystem_Snow_5.Emit(this.emitParams, numParticles);
        break;
      case BiomeConstants.TypeOfParticle.grass_6:
        this.ParticleChunkSystem_Grass_6.Emit(this.emitParams, numParticles);
        break;
      case BiomeConstants.TypeOfParticle.rot:
        this.ParticleChunkSystem_Rot.Emit(this.emitParams, numParticles);
        break;
    }
  }

  public void newTimeOfDay()
  {
    this.fireflies.gameObject.SetActive(false);
    if (TimeManager.CurrentPhase != DayPhase.Night)
      return;
    this.fireflies.gameObject.SetActive(true);
  }

  public IEnumerator LerpTimeOfDay(Color StartColor, Color endColor, float Intensity)
  {
    endColor.a = Intensity;
    for (float timer = 0.0f; (double) timer <= (double) this.lerpTime; timer += 0.005f)
    {
      Shader.SetGlobalColor("_TimeOfDayColor", Color.Lerp(StartColor, endColor, timer));
      yield return (object) null;
    }
  }

  public void ObjectiveManager_OnObjectiveCompleted(ObjectivesData objective)
  {
    if (objective.Type != Objectives.TYPES.NO_CURSES && objective.Type != Objectives.TYPES.NO_DAMAGE && objective.Type != Objectives.TYPES.NO_DODGE)
      return;
    UnityEngine.Object.Instantiate<GameObject>(this.DungeonChallengeRatoo, this.GetRandomPositionForRatooReward(), Quaternion.identity, BiomeGenerator.Instance.CurrentRoom.generateRoom.transform);
  }

  public Vector3 GetRandomPositionForRatooReward()
  {
    Vector3 zero = Vector3.zero;
    float num = 1.5f;
    Interaction_WeaponSelectionPodium[] componentsInChildren = BiomeGenerator.Instance.CurrentRoom.GameObject.GetComponentsInChildren<Interaction_WeaponSelectionPodium>();
    return componentsInChildren == null || componentsInChildren.Length == 0 ? ((double) UnityEngine.Random.value > 0.5 ? Vector3.right : Vector3.left) * num : Vector3.up * num;
  }

  public void ShowHeartEffect(Transform parent, Vector3 offset, Health.HeartEffects heartEffect)
  {
    GameObject icon;
    switch (heartEffect)
    {
      case Health.HeartEffects.Black:
        icon = UnityEngine.Object.Instantiate<GameObject>(this.BlackHeartDamageIcon, this.transform.position, Quaternion.identity);
        break;
      case Health.HeartEffects.Fire:
        icon = UnityEngine.Object.Instantiate<GameObject>(this.FireHeartEffectIcon, this.transform.position, Quaternion.identity);
        break;
      case Health.HeartEffects.Ice:
        icon = UnityEngine.Object.Instantiate<GameObject>(this.IceHeartEffectIcon, this.transform.position, Quaternion.identity);
        break;
      default:
        icon = UnityEngine.Object.Instantiate<GameObject>(this.BlackHeartDamageIcon, this.transform.position, Quaternion.identity);
        break;
    }
    icon.transform.parent = parent;
    icon.transform.localPosition = Vector3.back + offset;
    icon.transform.DOPunchScale(Vector3.one * 0.5f, 0.2f);
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.DelayedCallback(0.75f, (System.Action) (() =>
    {
      if ((bool) (UnityEngine.Object) icon)
      {
        icon.transform.DOLocalMoveZ(0.0f, 0.25f);
        icon.transform.DOScale(0.0f, 0.25f);
      }
      GameManager.GetInstance().StartCoroutine((IEnumerator) this.DelayedCallback(0.25f, (System.Action) (() =>
      {
        if (!(bool) (UnityEngine.Object) icon)
          return;
        UnityEngine.Object.Destroy((UnityEngine.Object) icon.gameObject);
      })));
    })));
  }

  public void ShowTarotCardDamage(Transform parent, Vector3 offset)
  {
    GameObject icon = UnityEngine.Object.Instantiate<GameObject>(BiomeConstants.Instance.TarotCardDamageIcon, this.transform.position, Quaternion.identity);
    icon.transform.parent = parent;
    icon.transform.localPosition = Vector3.back + offset;
    icon.transform.DOPunchScale(Vector3.one * 0.5f, 0.2f);
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.DelayedCallback(0.75f, (System.Action) (() =>
    {
      if ((bool) (UnityEngine.Object) icon)
      {
        icon.transform.DOLocalMoveZ(0.0f, 0.25f);
        icon.transform.DOScale(0.0f, 0.25f);
      }
      GameManager.GetInstance().StartCoroutine((IEnumerator) this.DelayedCallback(0.25f, (System.Action) (() =>
      {
        if (!(bool) (UnityEngine.Object) icon)
          return;
        UnityEngine.Object.Destroy((UnityEngine.Object) icon.gameObject);
      })));
    })));
  }

  public void ShowDestroyTarot(Transform parent)
  {
    GameObject effect = ObjectPool.Spawn(this.TarotCardBreakEffect, parent);
    effect.transform.localPosition = Vector3.zero;
    effect.GetComponent<ParticleSystem>().Play();
    GameManager.GetInstance().WaitForSeconds(2f, (System.Action) (() => effect.Recycle()));
  }

  public void ShowDamageTextIcon(Transform parent, Vector3 offset, float damage)
  {
    GameObject icon = UnityEngine.Object.Instantiate<GameObject>(BiomeConstants.Instance.DamageTextIcon, this.transform.position, Quaternion.identity);
    icon.transform.parent = parent;
    icon.transform.localPosition = Vector3.back + offset;
    icon.transform.DOPunchScale(Vector3.one * 0.5f, 0.2f);
    icon.GetComponentInChildren<TMP_Text>().text = damage.ToString();
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.DelayedCallback(0.75f, (System.Action) (() =>
    {
      if ((bool) (UnityEngine.Object) icon)
      {
        icon.transform.DOLocalMoveZ(0.0f, 0.25f);
        icon.transform.DOScale(0.0f, 0.25f);
      }
      GameManager.GetInstance().StartCoroutine((IEnumerator) this.DelayedCallback(0.25f, (System.Action) (() =>
      {
        if (!(bool) (UnityEngine.Object) icon)
          return;
        UnityEngine.Object.Destroy((UnityEngine.Object) icon.gameObject);
      })));
    })));
  }

  public IEnumerator DelayedCallback(float delay, System.Action callback)
  {
    yield return (object) new WaitForSeconds(delay);
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void EmitLightningStrike(Vector3 targetPosition)
  {
    AudioManager.Instance.PlayOneShot("event:/dlc/atmos/blizzard_lightning_strike", targetPosition);
    this.lightningStrike.gameObject.SetActive(true);
    this.lightningStrike.transform.position = targetPosition;
  }

  public void FreezeTime()
  {
    VFXSequence freezeSequence = this.freezeTimeSequenceData.PlayNewSequence(PlayerFarming.Instance.transform, new Transform[1]
    {
      PlayerFarming.Instance.transform
    });
    PlayerFarming.Instance.playerRelic.FreezeTime(freezeSequence);
  }

  public void FreezeAllEnemies(float duration)
  {
    VFXSequence freezePoisonAllSequence = this.freezeAllSequenceData.PlayNewSequence(PlayerFarming.Instance.transform, new Transform[1]
    {
      PlayerFarming.Instance.transform
    });
    freezePoisonAllSequence.OnImpact += (Action<VFXObject, int>) ((vfxObject1, impactVFXIndex1) =>
    {
      freezePoisonAllSequence.OnImpact -= (Action<VFXObject, int>) ((vfxObject2, impactVFXIndex2) =>
      {
        // ISSUE: unable to decompile the method.
      });
      this.FreezeAllEnemiesInstant(duration);
    });
  }

  public void FreezeAllEnemiesInstant(float duration)
  {
    AudioManager.Instance.PlayOneShot("event:/relics/freeze_relic", this.gameObject);
    AudioManager.Instance.PlayOneShot("event:/player/Curses/goop_charge", this.gameObject);
    AudioManager.Instance.ToggleFilter("freeze", true);
    GameManager.GetInstance().WaitForSeconds(0.2f, (System.Action) (() =>
    {
      for (int index = Health.team2.Count - 1; index >= 0; --index)
      {
        if (!((UnityEngine.Object) Health.team2[index] == (UnityEngine.Object) null) && Health.team2[index].gameObject.activeInHierarchy)
        {
          Health health = Health.team2[index];
          if (!((UnityEngine.Object) health == (UnityEngine.Object) null))
          {
            health.AddIce(duration);
            AudioManager.Instance.PlayOneShot("event:/enemy/spit_gross_projectile", this.gameObject);
            BiomeConstants.Instance.EmitSmokeExplosionVFX(health.transform.position, Color.cyan);
          }
        }
      }
      AudioManager.Instance.ToggleFilter("freeze", false);
    }));
  }

  public void BurnAllEnemies(float minDuration, float maxDuration)
  {
    VFXSequence freezePoisonAllSequence = this.burnAllSequenceData.PlayNewSequence(PlayerFarming.Instance.transform, new Transform[1]
    {
      PlayerFarming.Instance.transform
    });
    freezePoisonAllSequence.OnImpact += (Action<VFXObject, int>) ((vfxObject1, impactVFXIndex1) =>
    {
      freezePoisonAllSequence.OnImpact -= (Action<VFXObject, int>) ((vfxObject2, impactVFXIndex2) =>
      {
        // ISSUE: unable to decompile the method.
      });
      AudioManager.Instance.PlayOneShot("event:/relics/freeze_relic", this.gameObject);
      AudioManager.Instance.PlayOneShot("event:/player/Curses/goop_charge", this.gameObject);
      AudioManager.Instance.ToggleFilter("freeze", true);
      GameManager.GetInstance().WaitForSeconds(0.2f, (System.Action) (() =>
      {
        for (int index = Health.team2.Count - 1; index >= 0; --index)
        {
          if (!((UnityEngine.Object) Health.team2[index] == (UnityEngine.Object) null) && Health.team2[index].gameObject.activeInHierarchy)
          {
            Health health = Health.team2[index];
            if (!((UnityEngine.Object) health == (UnityEngine.Object) null))
            {
              health.AddBurn(PlayerFarming.Instance.gameObject, UnityEngine.Random.Range(minDuration, maxDuration));
              AudioManager.Instance.PlayOneShot("event:/enemy/spit_gross_projectile", this.gameObject);
              BiomeConstants.Instance.EmitSmokeExplosionVFX(health.transform.position, Color.red);
            }
          }
        }
        AudioManager.Instance.ToggleFilter("freeze", false);
      }));
    });
  }

  public void CharmAllEnemies()
  {
    Color smokeColor = new Color(0.902f, 0.694f, 0.278f);
    VFXSequence charmAllSequence = this.charmAllSequenceData.PlayNewSequence(PlayerFarming.Instance.transform, new Transform[1]
    {
      PlayerFarming.Instance.transform
    });
    charmAllSequence.OnImpact += (Action<VFXObject, int>) ((vfxObject1, impactVFXIndex1) =>
    {
      charmAllSequence.OnImpact -= (Action<VFXObject, int>) ((vfxObject2, impactVFXIndex2) =>
      {
        // ISSUE: unable to decompile the method.
      });
      AudioManager.Instance.PlayOneShot("event:/relics/relic_poison", PlayerFarming.Instance.gameObject);
      AudioManager.Instance.PlayOneShot("event:/player/Curses/goop_charge", PlayerFarming.Instance.gameObject);
      AudioManager.Instance.ToggleFilter("freeze", true);
      GameManager.GetInstance().WaitForSeconds(0.2f, (System.Action) (() =>
      {
        for (int index = Health.team2.Count - 1; index >= 0; --index)
        {
          if (!((UnityEngine.Object) Health.team2[index] == (UnityEngine.Object) null) && Health.team2[index].gameObject.activeInHierarchy)
          {
            Health health = Health.team2[index];
            if (!((UnityEngine.Object) health == (UnityEngine.Object) null))
            {
              health.AddCharm();
              AudioManager.Instance.PlayOneShot("event:/enemy/spit_gross_projectile", this.gameObject);
              BiomeConstants.Instance.EmitSmokeExplosionVFX(health.transform.position, smokeColor);
            }
          }
        }
        AudioManager.Instance.ToggleFilter("freeze", false);
      }));
    });
  }

  public void PlayFrostedEnemies(float duration)
  {
    VFXSequence frostedSequence = this.frostedEnemiesSequenceData.PlayNewSequence(PlayerFarming.Instance.transform, new Transform[1]
    {
      BiomeGenerator.Instance.CurrentRoom.generateRoom.transform
    }, BiomeGenerator.Instance.CurrentRoom.generateRoom.transform);
    frostedSequence.OnImpact += (Action<VFXObject, int>) ((vfxObject1, impactVFXIndex1) =>
    {
      frostedSequence.OnImpact -= (Action<VFXObject, int>) ((vfxObject2, impactVFXIndex2) =>
      {
        // ISSUE: unable to decompile the method.
      });
      this.FreezeAllEnemiesInstant(duration);
    });
  }

  public void SpawnPuffEffect(Vector3 position, Transform parent)
  {
    ParticleSystem puffObject = this.puffEffect.Spawn<ParticleSystem>(parent, position);
    GameManager.GetInstance().WaitForSeconds(2f, (System.Action) (() => puffObject.Recycle<ParticleSystem>()));
  }

  public void SpawnCursedPuffEffect(Vector3 position, Transform parent)
  {
    ParticleSystem puffObject = UnityEngine.Object.Instantiate<ParticleSystem>(this.puffEffect, position, Quaternion.identity, parent);
    puffObject.main.startColor = (ParticleSystem.MinMaxGradient) new Color(0.089f, 0.063f, 0.113f, 1f);
    GameManager.GetInstance().WaitForSeconds(2f, (System.Action) (() => UnityEngine.Object.Destroy((UnityEngine.Object) puffObject.gameObject)));
  }

  public void EmitPleasureVFX(Vector3 pos, Transform parent)
  {
    UnityEngine.Object.Instantiate<ParticleSystem>(this.pleasureParticle, pos, Quaternion.identity, parent);
  }

  public void EmitWinterSuccessVFX(Vector3 pos, Transform parent)
  {
    ParticleSystem p = UnityEngine.Object.Instantiate<ParticleSystem>(this.winterSuccess, pos, Quaternion.identity, parent);
    GameManager.GetInstance().WaitForSeconds(2f, (System.Action) (() => UnityEngine.Object.Destroy((UnityEngine.Object) p.gameObject)));
  }

  public void EmitRoarDistortionVFX(Vector3 pos)
  {
    ParticleSystem roarVFX = this.roarDistortion.Spawn<ParticleSystem>(pos);
    DOVirtual.DelayedCall(3f, (TweenCallback) (() => roarVFX.Recycle<ParticleSystem>()));
  }

  public enum TypeOfParticle
  {
    blank,
    wood,
    clay,
    grass,
    stone,
    bone,
    glass,
    grass_2,
    grass_3,
    grass_4,
    grass_5,
    snow_1,
    snow_2,
    snow_3,
    snow_4,
    snow_5,
    grass_6,
    rot,
  }
}
