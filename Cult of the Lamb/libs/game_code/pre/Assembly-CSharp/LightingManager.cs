// Decompiled with JetBrains decompiler
// Type: LightingManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMBiomeGeneration;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

#nullable disable
public class LightingManager : BaseMonoBehaviour
{
  public bool debugGUI;
  private CoroutineQueue queue;
  private bool isTransitionActive;
  public bool isTODTransition;
  public bool lerpActive;
  private float timer;
  private float progress;
  private float deltaTimeMult = 1f;
  private static LightingManager _instance;
  public BiomeLightingSettings dawnSettings;
  public BiomeLightingSettings morningSettings;
  public BiomeLightingSettings afternoonSettings;
  public BiomeLightingSettings duskSettings;
  public BiomeLightingSettings nightSettings;
  public float transitionDuration = 5f;
  public AnimationCurve transitionCurve = new AnimationCurve(new Keyframe[2]
  {
    new Keyframe(0.0f, 0.0f),
    new Keyframe(1f, 1f)
  });
  public float transitionDurationMultiplier = 1f;
  private BiomeLightingSettings targetSettings;
  [HideInInspector]
  public BiomeLightingSettings globalOverrideSettings;
  [HideInInspector]
  public BiomeLightingSettings overrideSettings;
  public bool inLeaderEncounter;
  public bool inOverride;
  public bool inGlobalOverride;
  private bool hideShadowsInTransition;
  public float StencilInfluence;
  private Light mainDirLight;
  private AmplifyColorEffect amplifyColor;
  private ScreenSpaceOverlay screenSpaceOverlay;
  public bool overrideNaturalLightRot;
  private const string mainDirLightTag = "MainDirLight";
  public static readonly int overrideInfluenceID = Shader.PropertyToID("_StencilInfluence");
  private float globalStencilInfluence = 0.5f;
  private Texture currentBlendShadowLUT;
  private Texture currentBlendHighlightLUT;
  public PostProcessVolume ppv;
  private AmplifyPostEffect amplifyPostEffect;
  private AnimationCurve nightTransitionCurve = new AnimationCurve(new Keyframe[2]
  {
    new Keyframe(0.0f, 0.0f),
    new Keyframe(0.0f, 0.0f)
  });
  private LightingManager.FinalizedState finalizedState;
  private static readonly int TimeOfDayColor = Shader.PropertyToID("_TimeOfDayColor");
  private static readonly int ItemInWoodsColor = Shader.PropertyToID("_ItemInWoodsColor");
  private static readonly int GlobalFogDist = Shader.PropertyToID("_GlobalFogDist");
  private static readonly int VerticalFogZOffset = Shader.PropertyToID("_VerticalFog_ZOffset");
  private static readonly int VerticalFogGradientSpread = Shader.PropertyToID("_VerticalFog_GradientSpread");
  private static readonly int GlobalHCol = Shader.PropertyToID("_GlobalHCol");
  private static readonly int GlobalSCol = Shader.PropertyToID("_GlobalSCol");

  public static LightingManager Instance => LightingManager._instance;

  public BiomeLightingSettings currentSettings { get; set; }

  private void Awake()
  {
    if ((UnityEngine.Object) LightingManager._instance != (UnityEngine.Object) null && (UnityEngine.Object) LightingManager._instance != (UnityEngine.Object) this)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    else
      LightingManager._instance = this;
    this.Init();
  }

  public void Init()
  {
    this.ppv.profile.TryGetSettings<AmplifyPostEffect>(out this.amplifyPostEffect);
    if (!(bool) (UnityEngine.Object) this.amplifyPostEffect)
      this.amplifyPostEffect = this.ppv.profile.AddSettings<AmplifyPostEffect>();
    this.mainDirLight = GameObject.FindGameObjectWithTag("MainDirLight").GetComponent<Light>();
    this.amplifyColor = Camera.main.GetComponent<AmplifyColorEffect>();
    this.screenSpaceOverlay = this.transform.parent.GetComponentInChildren<ScreenSpaceOverlay>();
    this.screenSpaceOverlay.Init();
  }

  private void Start()
  {
    this.currentSettings = this.SetCurrentLightingSettings();
    this.targetSettings = new BiomeLightingSettings();
    this.queue = new CoroutineQueue((BaseMonoBehaviour) this);
    this.queue.StartLoop();
    this.SetUpTimeOfDay();
  }

  private void OnEnable()
  {
    BiomeGenerator.OnBiomeGenerated += new BiomeGenerator.BiomeAction(this.SetUpTimeOfDay);
    TimeManager.OnNewPhaseStarted += new System.Action(this.TransitionTimeOfDay);
  }

  private void OnDisable()
  {
    this.inOverride = false;
    BiomeGenerator.OnBiomeGenerated -= new BiomeGenerator.BiomeAction(this.SetUpTimeOfDay);
    TimeManager.OnNewPhaseStarted -= new System.Action(this.TransitionTimeOfDay);
  }

  private void OnDestroy()
  {
    this.inOverride = false;
    TimeManager.OnNewPhaseStarted -= new System.Action(this.TransitionTimeOfDay);
    if (!((UnityEngine.Object) this == (UnityEngine.Object) LightingManager._instance))
      return;
    LightingManager._instance = (LightingManager) null;
  }

  public BiomeLightingSettings SetCurrentLightingSettings()
  {
    BiomeLightingSettings instance = ScriptableObject.CreateInstance<BiomeLightingSettings>();
    instance.AmbientColour = RenderSettings.ambientLight;
    instance.DirectionalLightColour = this.mainDirLight.color;
    instance.DirectionalLightIntensity = this.mainDirLight.intensity;
    instance.ShadowStrength = this.mainDirLight.shadowStrength;
    instance.LightRotation = this.mainDirLight.transform.rotation.eulerAngles;
    instance.GlobalHighlightColor = Shader.GetGlobalColor(LightingManager.GlobalHCol);
    instance.GlobalShadowColor = Shader.GetGlobalColor(LightingManager.GlobalSCol);
    instance.LutTexture_Shadow = this.amplifyPostEffect.LutTexture.value;
    instance.LutTexture_Lit = this.amplifyPostEffect.LutHighlightTexture.value;
    instance.StencilInfluence = Shader.GetGlobalFloat(LightingManager.overrideInfluenceID);
    instance.Exposure = this.amplifyPostEffect.Exposure.value;
    instance.FogColor = Shader.GetGlobalColor(LightingManager.ItemInWoodsColor);
    instance.FogDist = (Vector2) Shader.GetGlobalVector(LightingManager.GlobalFogDist);
    instance.FogHeight = Shader.GetGlobalFloat(LightingManager.VerticalFogZOffset);
    instance.FogSpread = Shader.GetGlobalFloat(LightingManager.VerticalFogGradientSpread);
    instance.GodRayColor = Shader.GetGlobalColor(LightingManager.TimeOfDayColor);
    instance.ScreenSpaceOverlayMat = this.screenSpaceOverlay.GetRenderer().sharedMaterials[0];
    instance.overrideLightingProperties = new OverrideLightingProperties();
    this.currentBlendShadowLUT = (UnityEngine.Object) this.amplifyPostEffect.LutBlendTexture.value != (UnityEngine.Object) null ? this.amplifyPostEffect.LutBlendTexture.value : this.amplifyPostEffect.LutTexture.value;
    this.currentBlendHighlightLUT = (UnityEngine.Object) this.amplifyPostEffect.LutHighlightBlendTexture.value != (UnityEngine.Object) null ? this.amplifyPostEffect.LutHighlightBlendTexture.value : this.amplifyPostEffect.LutHighlightTexture.value;
    return instance;
  }

  public Vector3 GetLightRotation(DayPhase dayPhase)
  {
    LightingManager.LightRotationRange lightRotationRange;
    switch (dayPhase)
    {
      case DayPhase.Dawn:
        lightRotationRange.startRot = new Vector3(-20f, -60f, 0.0f);
        lightRotationRange.endRot = new Vector3(-40f, -30f, 0.0f);
        break;
      case DayPhase.Morning:
        lightRotationRange.startRot = new Vector3(-40f, -30f, 0.0f);
        lightRotationRange.endRot = new Vector3(-50f, 0.0f, 0.0f);
        break;
      case DayPhase.Afternoon:
        lightRotationRange.startRot = new Vector3(-50f, 0.0f, 0.0f);
        lightRotationRange.endRot = new Vector3(-40f, 30f, 0.0f);
        break;
      case DayPhase.Dusk:
        lightRotationRange.startRot = new Vector3(-40f, 30f, 0.0f);
        lightRotationRange.endRot = new Vector3(-20f, 60f, 0.0f);
        break;
      case DayPhase.Night:
        lightRotationRange.startRot = new Vector3(-25f, -60f, 0.0f);
        lightRotationRange.endRot = new Vector3(-25f, 60f, 0.0f);
        break;
      default:
        Debug.Log((object) "Oh uh day phase not right in lighting manager");
        lightRotationRange.startRot = Vector3.zero;
        lightRotationRange.endRot = Vector3.zero;
        break;
    }
    return Vector3.Lerp(lightRotationRange.startRot, lightRotationRange.endRot, TimeManager.CurrentPhaseProgress);
  }

  private void LateUpdate()
  {
    if (!((UnityEngine.Object) this.mainDirLight != (UnityEngine.Object) null) || this.overrideNaturalLightRot)
      return;
    this.mainDirLight.transform.rotation = Quaternion.Euler(this.GetLightRotation(TimeManager.CurrentPhase));
    switch (TimeManager.CurrentPhase)
    {
      case DayPhase.Dawn:
        this.mainDirLight.shadowStrength = Mathf.Lerp(0.0f, this.targetSettings.ShadowStrength, Mathf.InverseLerp(0.0f, 0.1f, TimeManager.CurrentPhaseProgress));
        break;
      case DayPhase.Dusk:
        this.mainDirLight.shadowStrength = Mathf.Lerp(this.targetSettings.ShadowStrength, 0.0f, Mathf.InverseLerp(0.9f, 1f, TimeManager.CurrentPhaseProgress));
        break;
      case DayPhase.Night:
        this.mainDirLight.shadowStrength = Mathf.Lerp(0.0f, this.targetSettings.ShadowStrength, Mathf.InverseLerp(0.0f, 0.1f, TimeManager.CurrentPhaseProgress) * (1f - Mathf.InverseLerp(0.9f, 1f, TimeManager.CurrentPhaseProgress)));
        break;
    }
  }

  public void TransitionTimeOfDay()
  {
    if (WeatherController.InsideBuilding)
      this.transitionDurationMultiplier = 0.0f;
    this.UpdateLighting(false);
  }

  public void SetUpTimeOfDay()
  {
    this.transitionDurationMultiplier = 0.0f;
    this.UpdateLighting(false);
    this.transitionDurationMultiplier = 1f;
  }

  public void PrepareLightingSettings()
  {
    if ((UnityEngine.Object) this.targetSettings == (UnityEngine.Object) null)
      this.targetSettings = new BiomeLightingSettings();
    switch (TimeManager.CurrentPhase)
    {
      case DayPhase.Dawn:
        this.targetSettings = this.dawnSettings;
        break;
      case DayPhase.Morning:
        this.targetSettings = this.morningSettings;
        break;
      case DayPhase.Afternoon:
        this.targetSettings = this.afternoonSettings;
        break;
      case DayPhase.Dusk:
        this.targetSettings = this.duskSettings;
        break;
      case DayPhase.Night:
        this.targetSettings = this.nightSettings;
        break;
    }
    if (this.inGlobalOverride && this.globalOverrideSettings.overrideLightingProperties != null)
      this.targetSettings = !this.globalOverrideSettings.overrideLightingProperties.Enabled ? this.globalOverrideSettings : this.MergeLightingSettings(this.targetSettings, this.globalOverrideSettings, this.globalOverrideSettings.overrideLightingProperties);
    if (!this.inOverride || this.overrideSettings.overrideLightingProperties == null)
      return;
    if (this.overrideSettings.overrideLightingProperties.Enabled)
      this.targetSettings = this.MergeLightingSettings(this.targetSettings, this.overrideSettings, this.overrideSettings.overrideLightingProperties);
    else
      this.targetSettings = this.overrideSettings;
  }

  public void UpdateLighting(bool allowInterupt)
  {
    if (allowInterupt)
    {
      if (this.lerpActive)
      {
        Debug.Log((object) "Lerp Active");
        if (!this.isTODTransition)
          this.deltaTimeMult *= -1f;
        else
          this.queue.EnqueueAction(this.TransitionLighting(this.transitionDuration * this.transitionDurationMultiplier, true));
      }
      else
      {
        Debug.Log((object) "Lerp Not Active");
        this.queue.EnqueueAction(this.TransitionLighting(this.transitionDuration * this.transitionDurationMultiplier, true));
      }
    }
    else
    {
      if (this.queue == null)
        return;
      this.queue.EnqueueAction(this.TransitionLighting(this.transitionDuration * this.transitionDurationMultiplier, false));
    }
  }

  public BiomeLightingSettings MergeLightingSettings(
    BiomeLightingSettings currSettings,
    BiomeLightingSettings LightingSettings,
    OverrideLightingProperties overrideLightingProperties)
  {
    BiomeLightingSettings instance = ScriptableObject.CreateInstance<BiomeLightingSettings>();
    instance.UnscaledTime = overrideLightingProperties.UnscaledTime ? LightingSettings.UnscaledTime : currSettings.UnscaledTime;
    instance.AmbientColour = overrideLightingProperties.AmbientColor ? LightingSettings.AmbientColour : currSettings.AmbientColour;
    instance.DirectionalLightColour = overrideLightingProperties.DirectionalLightColor ? LightingSettings.DirectionalLightColour : currSettings.DirectionalLightColour;
    instance.DirectionalLightIntensity = overrideLightingProperties.DirectionalLightIntensity ? LightingSettings.DirectionalLightIntensity : currSettings.DirectionalLightIntensity;
    instance.ShadowStrength = overrideLightingProperties.ShadowStrength ? LightingSettings.ShadowStrength : currSettings.ShadowStrength;
    instance.LightRotation = overrideLightingProperties.LightRotation ? LightingSettings.LightRotation : currSettings.LightRotation;
    instance.GlobalHighlightColor = overrideLightingProperties.GlobalHighlightColor ? LightingSettings.GlobalHighlightColor : currSettings.GlobalHighlightColor;
    instance.GlobalShadowColor = overrideLightingProperties.GlobalShadowColor ? LightingSettings.GlobalShadowColor : currSettings.GlobalShadowColor;
    instance.LutTexture_Shadow = overrideLightingProperties.LUTTextureShadow ? LightingSettings.LutTexture_Shadow : currSettings.LutTexture_Shadow;
    instance.LutTexture_Lit = overrideLightingProperties.LUTTextureLit ? LightingSettings.LutTexture_Lit : currSettings.LutTexture_Lit;
    instance.StencilInfluence = overrideLightingProperties.StencilInfluence ? LightingSettings.StencilInfluence : currSettings.StencilInfluence;
    instance.Exposure = overrideLightingProperties.Exposure ? LightingSettings.Exposure : currSettings.Exposure;
    instance.FogColor = overrideLightingProperties.FogColor ? LightingSettings.FogColor : currSettings.FogColor;
    instance.FogDist = overrideLightingProperties.FogDist ? LightingSettings.FogDist : currSettings.FogDist;
    instance.FogHeight = overrideLightingProperties.FogHeight ? LightingSettings.FogHeight : currSettings.FogHeight;
    instance.FogSpread = overrideLightingProperties.FogSpread ? LightingSettings.FogSpread : currSettings.FogSpread;
    instance.GodRayColor = overrideLightingProperties.GodRayColor ? LightingSettings.GodRayColor : currSettings.GodRayColor;
    instance.ScreenSpaceOverlayMat = overrideLightingProperties.ScreenSpaceOverlayMat ? LightingSettings.ScreenSpaceOverlayMat : currSettings.ScreenSpaceOverlayMat;
    instance.overrideLightingProperties = overrideLightingProperties;
    return instance;
  }

  public IEnumerator TransitionLighting(float transitionDuration, bool allowInterupt)
  {
    this.isTransitionActive = true;
    this.PrepareLightingSettings();
    BiomeLightingSettings currSettings = this.currentSettings;
    BiomeLightingSettings newSettings = this.targetSettings;
    if (currSettings.IsEquivalent(newSettings))
    {
      this.isTransitionActive = false;
    }
    else
    {
      this.isTODTransition = !allowInterupt;
      if (newSettings.overrideLightingProperties.LightRotation)
        this.overrideNaturalLightRot = true;
      this.amplifyPostEffect.LutBlendTexture.value = newSettings.LutTexture_Shadow;
      this.amplifyPostEffect.LutHighlightBlendTexture.value = newSettings.LutTexture_Lit;
      bool transitionScreenSpaceOverlay = false;
      if ((UnityEngine.Object) newSettings.ScreenSpaceOverlayMat != (UnityEngine.Object) currSettings.ScreenSpaceOverlayMat)
      {
        this.screenSpaceOverlay.SetMaterials(currSettings.ScreenSpaceOverlayMat, newSettings.ScreenSpaceOverlayMat);
        transitionScreenSpaceOverlay = true;
      }
      Quaternion initLightRot = this.mainDirLight.transform.rotation;
      this.timer = 0.0f;
      this.lerpActive = true;
      while (this.lerpActive)
      {
        if (newSettings.UnscaledTime || currSettings.UnscaledTime)
        {
          Debug.Log((object) "Using Unscaled Time");
          this.timer += Time.unscaledDeltaTime * this.deltaTimeMult;
        }
        else if (allowInterupt)
        {
          this.timer += Time.deltaTime * this.deltaTimeMult;
        }
        else
        {
          this.deltaTimeMult = 1f;
          this.timer += Time.deltaTime * this.deltaTimeMult;
        }
        float num = (double) transitionDuration != 0.0 ? this.transitionCurve.Evaluate(Mathf.Clamp01(this.timer / transitionDuration)) : 1f;
        RenderSettings.ambientLight = Color.Lerp(currSettings.AmbientColour, newSettings.AmbientColour, num);
        Shader.SetGlobalColor(LightingManager.TimeOfDayColor, Color.Lerp(currSettings.GodRayColor, newSettings.GodRayColor, num));
        Shader.SetGlobalColor(LightingManager.ItemInWoodsColor, Color.Lerp(currSettings.FogColor, newSettings.FogColor, num));
        Shader.SetGlobalVector(LightingManager.GlobalFogDist, (Vector4) Vector2.Lerp(currSettings.FogDist, newSettings.FogDist, num));
        Shader.SetGlobalFloat(LightingManager.VerticalFogZOffset, Mathf.Lerp(currSettings.FogHeight, newSettings.FogHeight, num));
        Shader.SetGlobalFloat(LightingManager.VerticalFogGradientSpread, Mathf.Lerp(currSettings.FogSpread, newSettings.FogSpread, num));
        Shader.SetGlobalColor(LightingManager.GlobalHCol, Color.Lerp(currSettings.GlobalHighlightColor, newSettings.GlobalHighlightColor, num));
        Shader.SetGlobalColor(LightingManager.GlobalSCol, Color.Lerp(currSettings.GlobalShadowColor, newSettings.GlobalShadowColor, num));
        if ((UnityEngine.Object) Camera.main != (UnityEngine.Object) null)
          Camera.main.backgroundColor = Color.Lerp(currSettings.FogColor, newSettings.FogColor, num);
        this.mainDirLight.color = Color.Lerp(currSettings.DirectionalLightColour, newSettings.DirectionalLightColour, num);
        this.mainDirLight.intensity = Mathf.Lerp(currSettings.DirectionalLightIntensity, newSettings.DirectionalLightIntensity, num);
        this.mainDirLight.shadowStrength = Mathf.Lerp(currSettings.ShadowStrength, newSettings.ShadowStrength, num);
        switch (TimeManager.CurrentPhase)
        {
          case DayPhase.Dawn:
            this.mainDirLight.shadowStrength *= Mathf.InverseLerp(0.0f, 0.1f, TimeManager.CurrentPhaseProgress);
            break;
          case DayPhase.Dusk:
            this.mainDirLight.shadowStrength *= 1f - Mathf.InverseLerp(0.9f, 1f, TimeManager.CurrentPhaseProgress);
            break;
          case DayPhase.Night:
            this.mainDirLight.shadowStrength *= Mathf.InverseLerp(0.0f, 0.1f, TimeManager.CurrentPhaseProgress) * (1f - Mathf.InverseLerp(0.9f, 1f, TimeManager.CurrentPhaseProgress));
            break;
        }
        if (!currSettings.overrideLightingProperties.LightRotation)
          initLightRot = Quaternion.Euler(this.GetLightRotation(TimeManager.CurrentPhase));
        Quaternion b = newSettings.overrideLightingProperties.LightRotation ? Quaternion.Euler(newSettings.LightRotation) : Quaternion.Euler(this.GetLightRotation(TimeManager.CurrentPhase));
        this.mainDirLight.transform.rotation = Quaternion.Lerp(initLightRot, b, num);
        this.amplifyPostEffect.BlendAmount.value = num;
        this.globalStencilInfluence = Mathf.Lerp(currSettings.StencilInfluence, newSettings.StencilInfluence, num);
        Shader.SetGlobalFloat(LightingManager.overrideInfluenceID, this.globalStencilInfluence);
        this.amplifyPostEffect.Exposure.value = Mathf.Lerp(currSettings.Exposure, newSettings.Exposure, num);
        if (transitionScreenSpaceOverlay)
          this.screenSpaceOverlay.TransitionMaterial(num);
        if ((double) this.timer < 0.0)
        {
          this.finalizedState = LightingManager.FinalizedState.StateA;
          this.lerpActive = false;
        }
        else if ((double) this.timer > (double) transitionDuration)
        {
          this.finalizedState = LightingManager.FinalizedState.StateB;
          this.lerpActive = false;
        }
        yield return (object) new WaitForEndOfFrame();
      }
      yield return (object) new WaitForEndOfFrame();
      this.currentSettings = this.finalizedState == LightingManager.FinalizedState.StateA & allowInterupt ? currSettings : newSettings;
      this.timer = 0.0f;
      this.deltaTimeMult = 1f;
      this.transitionDurationMultiplier = 1f;
      this.overrideNaturalLightRot = this.currentSettings.overrideLightingProperties.LightRotation;
      this.amplifyPostEffect.BlendAmount.value = 0.0f;
      this.amplifyPostEffect.LutTexture.value = this.currentSettings.LutTexture_Shadow;
      this.amplifyPostEffect.LutBlendTexture.value = (Texture) null;
      this.amplifyPostEffect.LutHighlightTexture.value = this.currentSettings.LutTexture_Lit;
      this.amplifyPostEffect.LutHighlightBlendTexture.value = (Texture) null;
      if (transitionScreenSpaceOverlay)
        this.screenSpaceOverlay.SetMaterials(this.currentSettings.ScreenSpaceOverlayMat, (Material) null);
      this.timer = 0.0f;
      this.deltaTimeMult = 1f;
      if (this.isTODTransition)
        this.isTODTransition = false;
      this.isTransitionActive = false;
    }
  }

  private void OnGUI()
  {
    if (!this.debugGUI)
      return;
    GUI.skin.label.fontSize = 20;
    GUI.color = Color.yellow;
    double num = (double) GUI.HorizontalSlider(new Rect(25f, 325f, 500f, 50f), this.amplifyPostEffect.BlendAmount.value, 0.0f, 1f);
    GUI.Label(new Rect(25f, 350f, 500f, 25f), "Is transition active: " + this.isTransitionActive.ToString());
    GUI.Label(new Rect(25f, 375f, 500f, 25f), "Is transition dir: " + (object) this.deltaTimeMult);
    int y = 400;
    GUI.Label(new Rect(25f, 400f, 500f, 25f), "QUEUE:");
    foreach (IEnumerator action in this.queue.Actions)
    {
      y += 25;
      GUI.Label(new Rect(25f, (float) y, 500f, 25f), action.ToString());
    }
    if (!this.overrideNaturalLightRot && !this.isTransitionActive)
    {
      GUI.Label(new Rect(25f, 500f, 500f, 25f), "Natural Light Rotation: " + (object) this.mainDirLight.transform.rotation);
    }
    else
    {
      GUI.Label(new Rect(25f, 500f, 500f, 25f), "Overwriting Light Rotation: " + (object) this.mainDirLight.transform.rotation);
      GUI.Label(new Rect(25f, 525f, 500f, 25f), "Target Natural Rotation: " + (object) Quaternion.Euler(this.GetLightRotation(TimeManager.CurrentPhase)));
    }
    GUI.Label(new Rect(25f, 575f, 500f, 25f), "is TOD Transition?: " + this.isTODTransition.ToString());
  }

  private enum FinalizedState
  {
    StateA,
    StateB,
  }

  public struct LightRotationRange
  {
    public Vector3 startRot;
    public Vector3 endRot;
  }
}
