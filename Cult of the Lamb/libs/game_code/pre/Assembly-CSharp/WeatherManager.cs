// Decompiled with JetBrains decompiler
// Type: WeatherManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class WeatherManager : BaseMonoBehaviour
{
  [Range(0.0f, 2f)]
  public float SnowIntensity;
  private float lastSnowIntensity;
  public Texture2D SnowTexture;
  public ParticleSystem SnowSystem;
  [Range(0.0f, 1f)]
  public float RainIntensity;
  private float lastRainIntensity;
  [Range(0.01f, 1f)]
  public float RippleTiling = 0.2f;
  public float RippleAnimSpeed = 1f;
  public Vector4 RippleWindSpeed = new Vector4(0.01f, 0.08f, 0.012f, 0.01f);
  public float WaterBumpDistance = 50f;
  public Texture2D RainRipples;
  public ParticleSystem RainSystem;
  public ParticleSystem WindSystem;
  public Vector2 windDirection = new Vector2(1f, 0.2f);
  private Vector2 lastWindDirection;
  public float windSpeed = 3f;
  private float lastWindSpeed;
  public float windDensity = 0.1f;
  private float lastWindDensity;
  public float transitionRate = 0.5f;
  public float DebugWindSpeed;
  public Vector2 DebugWindDir = Vector2.zero;
  public float DebugWindDensity;
  public GameObject overlayHolder;
  public Image overlay;
  private Vector2 windTimer = new Vector2(0.0f, 0.0f);
  public EventInstance rainSound;
  public EventInstance windSound;
  public static WeatherManager instance;
  private AnimationCurve curveMinX = new AnimationCurve();
  private AnimationCurve curveMaxX = new AnimationCurve();
  private AnimationCurve curveMinY = new AnimationCurve();
  private AnimationCurve curveMaxY = new AnimationCurve();
  private bool playingRainSound;
  private bool playingWindSound;
  private ParticleSystem.VelocityOverLifetimeModule windVelocity;
  private static readonly int SnowIntensityGlobal = Shader.PropertyToID("_Snow_Intensity");
  private static readonly int SnowTexture1 = Shader.PropertyToID("_SnowTexture");

  private void OnDestroy()
  {
    if ((Object) this.RainSystem != (Object) null)
      this.RainSystem.emission.enabled = false;
    if ((Object) this.WindSystem != (Object) null)
      this.WindSystem.emission.enabled = false;
    Shader.DisableKeyword("_RAINEFFECT_ON");
    AudioManager.Instance.StopLoop(this.rainSound);
    AudioManager.Instance.StopLoop(this.windSound);
  }

  private void OnEnable() => WeatherManager.instance = this;

  private void OnDisable()
  {
    if ((Object) WeatherManager.instance == (Object) this)
      WeatherManager.instance = (WeatherManager) null;
    if ((Object) this.RainSystem != (Object) null)
      this.RainSystem.emission.enabled = false;
    if ((Object) this.WindSystem != (Object) null)
      this.WindSystem.emission.enabled = false;
    Shader.DisableKeyword("_RAINEFFECT_ON");
    AudioManager.Instance.StopLoop(this.rainSound);
    AudioManager.Instance.StopLoop(this.windSound);
  }

  public void Init()
  {
    if ((Object) CameraManager.instance.CameraRef == (Object) null)
    {
      Debug.Log((object) "Warning Camera Main = null Cant set weather");
    }
    else
    {
      foreach (ParticleSystem componentsInChild in CameraManager.instance.CameraRef.GetComponentsInChildren<ParticleSystem>())
      {
        if (componentsInChild.name == "Snow_ParticleSystem")
          this.SnowSystem = componentsInChild;
        else if (componentsInChild.name == "Rain_ParticleSystem")
          this.RainSystem = componentsInChild;
        else if (componentsInChild.name == "Wind_ParticleSystem")
          this.WindSystem = componentsInChild;
      }
      if ((Object) this.overlayHolder != (Object) null)
        this.overlayHolder.SetActive(false);
      if ((Object) this.RainSystem == (Object) null || (Object) this.WindSystem == (Object) null)
        return;
      Shader.SetGlobalFloat("_Rain_Intensity", this.RainIntensity);
      Shader.SetGlobalVector("_Rain_RippleWindSpeed", this.RippleWindSpeed);
      Shader.SetGlobalFloat("_Rain_RippleTiling", this.RippleTiling);
      Shader.SetGlobalFloat("_Rain_RippleAnimSpeed", this.RippleAnimSpeed);
      Shader.SetGlobalFloat("_Rain_WaterBumpDistance", this.WaterBumpDistance);
      if ((bool) (Object) this.RainRipples)
        Shader.SetGlobalTexture("_Rain_Ripples", (Texture) this.RainRipples);
      Shader.SetGlobalVector("_WindDirection", (Vector4) this.windDirection);
      Shader.SetGlobalFloat("_WindSpeed", this.windSpeed);
      Shader.SetGlobalFloat("_WindDensity", this.windDensity);
      this.lastWindDirection = this.windDirection;
      this.lastWindSpeed = this.windSpeed;
      this.lastWindDensity = this.windDensity;
      if ((Object) this.SnowSystem == (Object) null)
        return;
      Shader.SetGlobalFloat(WeatherManager.SnowIntensityGlobal, this.SnowIntensity);
      Shader.SetGlobalTexture(WeatherManager.SnowTexture1, (Texture) this.SnowTexture);
    }
  }

  private void UpdateRainSettings()
  {
    AudioManager.Instance.StopLoop(this.rainSound);
    if ((Object) this.RainSystem == (Object) null)
      return;
    if ((double) this.RainIntensity == 0.0)
    {
      AudioManager.Instance.StopLoop(this.rainSound);
      this.playingRainSound = false;
    }
    else
    {
      if (!this.playingRainSound)
      {
        AudioManager.Instance.StopLoop(this.rainSound);
        this.rainSound = AudioManager.Instance.CreateLoop("event:/atmos/forest/rain", this.gameObject, true);
        this.playingRainSound = true;
      }
      int num = (int) this.rainSound.setVolume(Mathf.Clamp(this.RainIntensity + 0.35f, 0.0f, 1f));
    }
    ParticleSystem.EmissionModule emission = this.RainSystem.emission;
    if (!Mathf.Approximately(this.lastRainIntensity, this.RainIntensity))
    {
      if ((double) this.lastRainIntensity > 0.0 && !emission.enabled)
      {
        if (!this.overlayHolder.activeSelf)
        {
          this.overlayHolder.SetActive(true);
          this.overlay.color = new Color(this.overlay.color.r, this.overlay.color.g, this.overlay.color.b, 0.0f);
          DOTweenModuleUI.DOFade(this.overlay, 0.15f, 3f);
        }
        Shader.EnableKeyword("_RAINEFFECT_ON");
        emission.enabled = true;
      }
      this.lastRainIntensity = Mathf.MoveTowards(this.lastRainIntensity, this.RainIntensity, (float) ((double) this.transitionRate * (double) Time.deltaTime * 0.5));
      Shader.SetGlobalFloat("_Rain_Intensity", this.lastRainIntensity);
      emission.rateOverTime = (ParticleSystem.MinMaxCurve) (300f * this.lastRainIntensity);
    }
    else
    {
      if (!Mathf.Approximately(this.lastRainIntensity, 0.0f) || !emission.enabled)
        return;
      AudioManager.Instance.StopLoop(this.rainSound);
      if (this.overlayHolder.activeSelf)
        DOTweenModuleUI.DOFade(this.overlay, 0.0f, 3f).OnComplete<TweenerCore<Color, Color, ColorOptions>>(new TweenCallback(this.DisableOverlay));
      Shader.DisableKeyword("_RAINEFFECT_ON");
      emission.enabled = false;
    }
  }

  private void DisableOverlay() => this.overlayHolder.SetActive(false);

  private void UpdateSnowSettings()
  {
    if ((Object) this.SnowSystem == (Object) null)
      return;
    ParticleSystem.EmissionModule emission = this.SnowSystem.emission;
    if (!Mathf.Approximately(this.lastSnowIntensity, this.SnowIntensity))
    {
      if ((double) this.lastSnowIntensity > 0.0 && !emission.enabled)
      {
        if (!this.overlayHolder.activeSelf)
        {
          this.overlayHolder.SetActive(true);
          this.overlay.color = new Color(this.overlay.color.r, this.overlay.color.g, this.overlay.color.b, 0.0f);
          DOTweenModuleUI.DOFade(this.overlay, 0.15f, 3f);
        }
        Shader.EnableKeyword("_SNOWEFFECT_ON");
        emission.enabled = true;
      }
      this.lastSnowIntensity = Mathf.MoveTowards(this.lastSnowIntensity, this.SnowIntensity, (float) ((double) this.transitionRate * (double) Time.deltaTime * 0.5));
      Shader.SetGlobalFloat(WeatherManager.SnowIntensityGlobal, this.lastSnowIntensity);
      emission.rateOverTime = (ParticleSystem.MinMaxCurve) (5f * this.lastSnowIntensity);
    }
    else if (Mathf.Approximately(this.lastSnowIntensity, 0.0f) && emission.enabled)
    {
      if (this.overlayHolder.activeSelf)
        DOTweenModuleUI.DOFade(this.overlay, 0.0f, 3f).OnComplete<TweenerCore<Color, Color, ColorOptions>>(new TweenCallback(this.DisableOverlay));
      Shader.DisableKeyword("_SNOWEFFECT_ON");
      emission.enabled = false;
    }
    ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = this.SnowSystem.velocityOverLifetime with
    {
      x = this.windVelocity.x,
      y = this.windVelocity.y
    };
  }

  private void UpdateWindSettings()
  {
    if ((Object) this.WindSystem == (Object) null)
      return;
    if ((double) this.windSpeed <= 3.0)
    {
      AudioManager.Instance.StopLoop(this.windSound);
      this.WindSystem.Stop();
      this.playingWindSound = false;
    }
    else
    {
      if (!this.playingWindSound)
      {
        this.WindSystem.Play();
        this.windSound = AudioManager.Instance.CreateLoop("event:/atmos/forest/heavy_wind", this.gameObject, true);
        this.playingWindSound = true;
      }
      int num = (int) this.windSound.setVolume(this.windSpeed / 10f);
    }
    ParticleSystem.EmissionModule emission = this.WindSystem.emission;
    ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = this.WindSystem.velocityOverLifetime;
    this.curveMinX.keys = WeatherManager.Linear(0.0f, 1f, (float) ((double) this.lastWindDirection.x * (double) this.lastWindSpeed * 1.1000000238418579));
    this.curveMaxX.keys = WeatherManager.Linear(0.0f, 1f, (float) ((double) this.lastWindDirection.x * (double) this.lastWindSpeed * 0.89999997615814209));
    this.curveMinY.keys = WeatherManager.Linear(0.0f, 1f, (float) ((double) this.lastWindDirection.y * (double) this.lastWindSpeed * 1.1000000238418579));
    this.curveMaxY.keys = WeatherManager.Linear(0.0f, 1f, (float) ((double) this.lastWindDirection.y * (double) this.lastWindSpeed * 0.89999997615814209));
    ParticleSystem.MinMaxCurve minMaxCurve1 = new ParticleSystem.MinMaxCurve();
    minMaxCurve1.mode = ParticleSystemCurveMode.TwoCurves;
    minMaxCurve1.curveMin = this.curveMinX;
    minMaxCurve1.curveMax = this.curveMaxX;
    minMaxCurve1.curveMultiplier = 1f;
    ParticleSystem.MinMaxCurve minMaxCurve2 = new ParticleSystem.MinMaxCurve();
    minMaxCurve2.mode = ParticleSystemCurveMode.TwoCurves;
    minMaxCurve2.curveMin = this.curveMinY;
    minMaxCurve2.curveMax = this.curveMaxY;
    minMaxCurve2.curveMultiplier = 1f;
    velocityOverLifetime.x = minMaxCurve1;
    velocityOverLifetime.y = minMaxCurve2;
    this.windVelocity = velocityOverLifetime;
    emission.rateOverTime = (ParticleSystem.MinMaxCurve) (this.lastWindSpeed - 3f);
    if (!Mathf.Approximately(this.lastWindDirection.x, this.windDirection.x))
      this.lastWindDirection.x = Mathf.MoveTowards(this.lastWindDirection.x, this.windDirection.x, this.transitionRate * Time.deltaTime);
    if (!Mathf.Approximately(this.lastWindDirection.y, this.windDirection.y))
      this.lastWindDirection.y = Mathf.MoveTowards(this.lastWindDirection.y, this.windDirection.y, this.transitionRate * Time.deltaTime);
    if (!Mathf.Approximately(this.lastWindSpeed, this.windSpeed))
      this.lastWindSpeed = Mathf.MoveTowards(this.lastWindSpeed, this.windSpeed, this.transitionRate * Time.deltaTime);
    if (!Mathf.Approximately(this.lastWindDensity, this.windDensity))
      this.lastWindDensity = Mathf.MoveTowards(this.lastWindDensity, this.windDensity, this.transitionRate * Time.deltaTime);
    this.windTimer.x += Time.deltaTime * this.lastWindSpeed * this.lastWindDirection.x;
    this.windTimer.y += Time.deltaTime * this.lastWindSpeed * this.lastWindDirection.y;
    Shader.SetGlobalVector("_WindTimer", (Vector4) this.windTimer);
    Shader.SetGlobalFloat("_WindDensity", this.lastWindDensity);
    this.DebugWindSpeed = this.lastWindSpeed;
    this.DebugWindDir = this.lastWindDirection;
    this.DebugWindDensity = this.lastWindDensity;
  }

  private void Start() => this.Init();

  private void Update()
  {
    this.UpdateRainSettings();
    this.UpdateWindSettings();
    this.UpdateSnowSettings();
  }

  private static Keyframe[] Linear(float timeStart, float timeEnd, float value)
  {
    return (double) timeStart == (double) timeEnd ? new Keyframe[1]
    {
      new Keyframe(timeStart, value)
    } : new Keyframe[2]
    {
      new Keyframe(timeStart, value, 0.0f, 0.0f),
      new Keyframe(timeEnd, value, 0.0f, 0.0f)
    };
  }
}
