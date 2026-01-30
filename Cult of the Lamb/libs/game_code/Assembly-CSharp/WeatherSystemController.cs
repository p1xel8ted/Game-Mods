// Decompiled with JetBrains decompiler
// Type: WeatherSystemController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using MMBiomeGeneration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityFx.Outline;

#nullable disable
public class WeatherSystemController : MonoBehaviour
{
  public static WeatherSystemController Instance;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float chanceForWeather;
  [SerializeField]
  public float RandomWeatherTransitionLength = 16f;
  [SerializeField]
  public WeatherSystemController.WeatherData[] weatherData;
  [SerializeField]
  public Image lightningFlashOverlay;
  [SerializeField]
  public CanvasGroup weatherCanvasGroup;
  [SerializeField]
  public Image WeatherTintOverlay;
  [SerializeField]
  public Texture2D snowGlobalTexture;
  [SerializeField]
  public Texture2D snowNoiseGlobalTexture;
  [SerializeField]
  public float globalSnowNoiseScaleA = 0.22f;
  [SerializeField]
  public float globalSnowNoiseScaleB = 0.41f;
  [SerializeField]
  public float snowTexScale = 0.33f;
  [SerializeField]
  public RawImage blizzardOverlay;
  [SerializeField]
  public GameObject BlizzardCloudPlane1;
  [SerializeField]
  public GameObject BlizzardCloudPlane2;
  [SerializeField]
  public float _MaxOpacity = 1f;
  public Tween blizzardCloudPlane1Tween;
  public Tween blizzardCloudPlane2Tween;
  [SerializeField]
  public bool debugSnow;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float snowDebugOverride;
  public SeasonsManager.Season currentSeasonDebug;
  [SerializeField]
  public OutlineLayerCollection outlineSettings;
  [SerializeField]
  public OutlineSettings normalOutlineSettings;
  [SerializeField]
  public OutlineSettings winterOutlineSettings;
  [SerializeField]
  public float winterIntensityTrigger = 0.5f;
  public WeatherSystemController.WeatherType currentWeatherType;
  public WeatherSystemController.WeatherStrength currentWeatherStrength;
  public WeatherSystemController.WeatherType previousWeatherType;
  public WeatherSystemController.WeatherStrength previousWeatherStrength;
  public WeatherSystemController.WeatherType cachedWeatherType;
  public WeatherSystemController.WeatherStrength cachedWeatherStrength;
  public Tween weatherTransitionTween;
  public WindSystemController windSystem;
  public EventInstance soundLoop;
  public static int SnowNoise = Shader.PropertyToID("_SnowNoise");
  public static int SnowTexture = Shader.PropertyToID("_SnowTexture");
  public static int SnowHeight = Shader.PropertyToID("_SnowHeight");
  public static int SnowSpread = Shader.PropertyToID("_SnowSpread");
  public static int SnowIntensity = Shader.PropertyToID("_Snow_Intensity");
  public static int GlobalSnowNoiseScaleA = Shader.PropertyToID(nameof (GlobalSnowNoiseScaleA));
  public static int GlobalSnowNoiseScaleB = Shader.PropertyToID(nameof (GlobalSnowNoiseScaleB));
  public static int SnowTexScale = Shader.PropertyToID(nameof (SnowTexScale));

  public bool canShowFogFX
  {
    get
    {
      return this.currentWeatherType == WeatherSystemController.WeatherType.Snowing && SeasonsManager.WinterSeverity >= 4 && !SettingsManager.Settings.Game.PerformanceMode;
    }
  }

  public WeatherSystemController.WeatherType CurrentWeatherType => this.currentWeatherType;

  public WeatherSystemController.WeatherStrength CurrentWeatherStrength
  {
    get => this.currentWeatherStrength;
  }

  public bool IsRaining => this.currentWeatherType == WeatherSystemController.WeatherType.Raining;

  public bool IsSnowing => this.currentWeatherType == WeatherSystemController.WeatherType.Snowing;

  public bool IsWindy => this.currentWeatherType == WeatherSystemController.WeatherType.Windy;

  public void Awake()
  {
    WeatherSystemController.Instance = this;
    this.windSystem = this.GetComponent<WindSystemController>();
    this.windSystem.StopWind();
    if (CheatConsole.IN_DEMO)
      TimeManager.OnNewPhaseStarted += new System.Action(this.ChooseWeather);
    else
      TimeManager.OnNewDayStarted += new System.Action(this.ChooseWeather);
    Shader.SetGlobalTexture(WeatherSystemController.SnowTexture, (Texture) this.snowGlobalTexture);
    Shader.SetGlobalTexture(WeatherSystemController.SnowNoise, (Texture) this.snowNoiseGlobalTexture);
    Shader.SetGlobalFloat(WeatherSystemController.SnowIntensity, 0.0f);
    Shader.SetGlobalFloat(WeatherSystemController.GlobalSnowNoiseScaleA, this.globalSnowNoiseScaleA);
    Shader.SetGlobalFloat(WeatherSystemController.GlobalSnowNoiseScaleB, this.globalSnowNoiseScaleB);
    Shader.SetGlobalFloat(WeatherSystemController.SnowTexScale, this.snowTexScale);
  }

  public void ChangeWinterOutlineSettings()
  {
    float globalFloat = Shader.GetGlobalFloat(WeatherSystemController.SnowIntensity);
    if ((double) globalFloat >= (double) this.winterIntensityTrigger)
    {
      this.outlineSettings.GetOrAddLayer(0).OutlineSettings = this.winterOutlineSettings;
      Debug.Log((object) ("Changed outline settings to winter Snow Intensity: " + globalFloat.ToString()));
    }
    else
    {
      this.outlineSettings.GetOrAddLayer(0).OutlineSettings = this.normalOutlineSettings;
      Debug.Log((object) ("Changed outline settings to normal Snow Intensity: " + globalFloat.ToString()));
    }
    if (PlayerFarming.Location != FollowerLocation.Dungeon1_6)
      return;
    this.outlineSettings.GetOrAddLayer(0).OutlineSettings = this.normalOutlineSettings;
  }

  public void DisableWeatherEffect() => this.gameObject.SetActive(false);

  public void EnableWeatherEffect() => this.gameObject.SetActive(true);

  public void HideWeather(bool snap = true, bool useTimeScale = true)
  {
    if (snap)
    {
      this.weatherCanvasGroup.alpha = 0.0f;
      this.HideFogFx(0.0f, useTimeScale);
    }
    else
    {
      this.weatherCanvasGroup.DOFade(0.0f, 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(!useTimeScale);
      this.HideFogFx(0.5f, useTimeScale);
    }
  }

  public void ShowWeather(bool snap = true, bool useTimeScale = true)
  {
    if (snap)
    {
      this.weatherCanvasGroup.alpha = 1f;
      if (!this.canShowFogFX)
        return;
      this.ShowFogFx(0.0f, useTimeScale);
    }
    else
    {
      this.weatherCanvasGroup.DOFade(1f, 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(!useTimeScale);
      if (!this.canShowFogFX)
        return;
      this.ShowFogFx(0.5f, useTimeScale);
    }
  }

  public void Start()
  {
    CameraManager.instance.GetComponentsInChildren<ParticleSystem>();
    foreach (WeatherSystemController.WeatherData weatherData in this.weatherData)
    {
      foreach (WeatherSystemController.ShaderVariable shaderVariable in weatherData.ShaderVariables)
      {
        if ((double) shaderVariable.TargetValue != 0.0)
          Shader.SetGlobalFloat(shaderVariable.ShaderKey, shaderVariable.SetOnStartOnly ? shaderVariable.TargetValue : 0.0f);
        else if (shaderVariable.TargetVector != Vector4.zero)
          Shader.SetGlobalVector(shaderVariable.ShaderKey, shaderVariable.SetOnStartOnly ? shaderVariable.TargetVector : Vector4.zero);
        else if ((UnityEngine.Object) shaderVariable.TargetTexture != (UnityEngine.Object) null)
          Shader.SetGlobalTexture(shaderVariable.ShaderKey, (Texture) shaderVariable.TargetTexture);
      }
      if ((UnityEngine.Object) weatherData.ParticleSystemReference != (UnityEngine.Object) null)
      {
        weatherData.ParticleSystem = weatherData.ParticleSystemReference;
        weatherData.ParticleSystemReference.emission.enabled = false;
      }
      if (weatherData.HasWind)
        this.windSystem.Initialise(weatherData.ParticleSystemReference ?? weatherData.ParticleSystem);
    }
    this.TurnOffOverlays();
    this.StartCoroutine((IEnumerator) this.WaitForPlayer());
  }

  public void TurnOffOverlays()
  {
    if ((UnityEngine.Object) this.WeatherTintOverlay != (UnityEngine.Object) null)
      this.WeatherTintOverlay.enabled = false;
    if ((UnityEngine.Object) this.blizzardOverlay != (UnityEngine.Object) null)
      this.blizzardOverlay.enabled = false;
    if ((UnityEngine.Object) this.BlizzardCloudPlane1 != (UnityEngine.Object) null)
    {
      this.BlizzardCloudPlane1.SetActive(false);
      Renderer component = this.BlizzardCloudPlane1.GetComponent<Renderer>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && (UnityEngine.Object) component.material != (UnityEngine.Object) null)
        component.material.SetFloat("_MaxOpacity", 0.0f);
    }
    if (!((UnityEngine.Object) this.BlizzardCloudPlane2 != (UnityEngine.Object) null))
      return;
    this.BlizzardCloudPlane2.SetActive(false);
    Renderer component1 = this.BlizzardCloudPlane2.GetComponent<Renderer>();
    if (!((UnityEngine.Object) component1 != (UnityEngine.Object) null) || !((UnityEngine.Object) component1.material != (UnityEngine.Object) null))
      return;
    component1.material.SetFloat("_MaxOpacity", 0.0f);
  }

  public IEnumerator WaitForPlayer()
  {
    while ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
      yield return (object) null;
    if (SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter && !this.LoadLocationWeather())
      this.ChooseWeather();
  }

  public void OnDestroy()
  {
    if (CheatConsole.IN_DEMO)
      TimeManager.OnNewPhaseStarted -= new System.Action(this.ChooseWeather);
    else
      TimeManager.OnNewDayStarted -= new System.Action(this.ChooseWeather);
    foreach (WeatherSystemController.WeatherData weatherData in this.weatherData)
      Shader.DisableKeyword(weatherData.ShaderKeyword);
    AudioManager.Instance.StopLoop(this.soundLoop);
    Shader.SetGlobalFloat(WeatherSystemController.SnowIntensity, 0.0f);
    this.ChangeWinterOutlineSettings();
  }

  public void Update()
  {
    if (this.debugSnow)
    {
      Shader.SetGlobalFloat(WeatherSystemController.SnowIntensity, this.snowDebugOverride);
      this.currentSeasonDebug = SeasonsManager.CurrentSeason;
    }
    if (DataManager.Instance.WeatherType == WeatherSystemController.WeatherType.None || DataManager.Instance.WeatherDuration == -1 || (double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.WeatherStartingTime <= (double) DataManager.Instance.WeatherDuration)
      return;
    this.ClearLocationWeather();
  }

  public bool LoadLocationWeather()
  {
    if ((double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.WeatherStartingTime >= (double) DataManager.Instance.WeatherDuration)
      return false;
    this.SetWeather(DataManager.Instance.WeatherType, DataManager.Instance.WeatherStrength, 0.0f);
    return true;
  }

  public void ClearLocationWeather()
  {
    this.StopCurrentWeather(3f);
    SeasonsManager.SetSeasonDefaultWeather(SeasonsManager.CurrentSeason);
  }

  public WeatherSystemController.WeatherData ChooseRandomWeather()
  {
    if ((double) UnityEngine.Random.value > (double) this.chanceForWeather)
      return (WeatherSystemController.WeatherData) null;
    ((IList<WeatherSystemController.WeatherData>) this.weatherData).Shuffle<WeatherSystemController.WeatherData>();
    for (int index = 0; index < 100; ++index)
    {
      foreach (WeatherSystemController.WeatherData weatherData in this.weatherData)
      {
        if ((double) UnityEngine.Random.value <= (double) weatherData.Chance)
          return weatherData;
      }
    }
    return (WeatherSystemController.WeatherData) null;
  }

  public void ChooseWeather()
  {
    if (this.currentWeatherType != WeatherSystemController.WeatherType.None || SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter)
      return;
    WeatherSystemController.WeatherData weatherData = this.ChooseRandomWeather();
    if (weatherData == null)
      return;
    if (!LocationManager.IndoorLocations.Contains(PlayerFarming.Location))
      this.SetWeather(weatherData.WeatherType, weatherData.WeatherStrength, this.RandomWeatherTransitionLength);
    else
      this.SetWeather(weatherData.WeatherType, weatherData.WeatherStrength, 0.0f);
  }

  public void SetWeather(
    WeatherSystemController.WeatherType weatherType,
    WeatherSystemController.WeatherStrength weatherStrength,
    float transitionDuration = 16f,
    bool updateLighting = true,
    bool allowNone = false)
  {
    if (weatherType == WeatherSystemController.WeatherType.None && !allowNone || PlayerFarming.Location == FollowerLocation.Dungeon1_6)
      return;
    Debug.Log((object) $"WeatherSystemController: Setting weather to {weatherType} with strength {weatherStrength}");
    this.TryPlaySnowGroundAudio(weatherType, weatherStrength, transitionDuration);
    this.previousWeatherType = this.currentWeatherType;
    this.previousWeatherStrength = this.currentWeatherStrength;
    this.currentWeatherType = weatherType;
    this.currentWeatherStrength = weatherStrength;
    if (LocationManager.IndoorLocations.Contains(PlayerFarming.Location))
      return;
    this.ShowCurrentWeather(transitionDuration, updateLighting, allowNone);
  }

  public void StopCurrentWeather(float transitionDuration = 2f)
  {
    if ((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null && BiomeGenerator.Instance.weatherType != WeatherSystemController.WeatherType.None && this.currentWeatherType == BiomeGenerator.Instance.weatherType)
      return;
    this.previousWeatherType = this.currentWeatherType;
    this.previousWeatherStrength = this.currentWeatherStrength;
    if (!LocationManager.IndoorLocations.Contains(PlayerFarming.Location))
      this.HideCurrentWeather(transitionDuration);
    this.currentWeatherType = WeatherSystemController.WeatherType.None;
    if (GameManager.IsDungeon(PlayerFarming.Location))
      return;
    DataManager.Instance.WeatherType = WeatherSystemController.WeatherType.None;
  }

  public void ShowCurrentWeather(float transitionDuration = 16f, bool updateLighting = true, bool allowNone = false)
  {
    WeatherSystemController.WeatherData currentData = this.GetWeatherData(this.currentWeatherType, this.currentWeatherStrength);
    WeatherSystemController.WeatherData previoustData = (WeatherSystemController.WeatherData) null;
    if (this.previousWeatherType != this.currentWeatherType)
      previoustData = this.GetWeatherData(this.previousWeatherType, this.previousWeatherStrength);
    ParticleSystem particleSystem1 = currentData?.ParticleSystemReference ?? currentData?.ParticleSystem;
    foreach (WeatherSystemController.WeatherData weatherData in this.weatherData)
    {
      ParticleSystem particleSystem2 = weatherData.ParticleSystemReference ?? weatherData.ParticleSystem;
      if ((UnityEngine.Object) particleSystem2 != (UnityEngine.Object) null && (UnityEngine.Object) particleSystem2 != (UnityEngine.Object) particleSystem1)
      {
        ParticleSystem.EmissionModule emission = particleSystem2.emission;
        if (emission.enabled)
        {
          emission.enabled = false;
          Debug.Log((object) ("WeatherSystemController: Disabled emission for " + particleSystem2.name));
        }
      }
    }
    if ((UnityEngine.Object) particleSystem1 != (UnityEngine.Object) null)
      Debug.Log((object) $"WeatherSystemController: Target system {particleSystem1.name} will be enabled");
    if (this.currentWeatherType == WeatherSystemController.WeatherType.Snowing && this.currentWeatherStrength == WeatherSystemController.WeatherStrength.Extreme && PlayerFarming.Location != FollowerLocation.IntroDungeon)
    {
      AudioManager.Instance.PlayAtmos("event:/atmos/winter/blizzard");
      AudioManager.Instance.AdjustAtmosParameter("blizzardState", SeasonsManager.WinterSeverity >= 5 ? 1f : 0.0f);
    }
    else if (this.currentWeatherType == WeatherSystemController.WeatherType.Snowing)
      AudioManager.Instance.PlayAtmos("event:/atmos/winter/snow");
    else if (this.previousWeatherType == WeatherSystemController.WeatherType.Snowing && this.previousWeatherStrength == WeatherSystemController.WeatherStrength.Extreme)
    {
      if ((UnityEngine.Object) HubManager.Instance != (UnityEngine.Object) null)
        AudioManager.Instance.PlayAtmos(HubManager.Instance.hubAtmosPath);
      else if (GameManager.IsDungeon(PlayerFarming.Location))
        AudioManager.Instance.PlayAtmos(BiomeGenerator.Instance.biomeAtmosPath);
      else
        AudioManager.Instance.PlayAtmos(TimeManager.IsNight ? BiomeBaseManager.Instance.NightBiomeAtmosPath : BiomeBaseManager.Instance.BiomeAtmosPath);
    }
    if (currentData != null)
    {
      if (!GameManager.IsDungeon(PlayerFarming.Location))
      {
        DataManager.Instance.WeatherStartingTime = TimeManager.TotalElapsedGameTime;
        DataManager.Instance.WeatherDuration = currentData.OverrideDuration ? -1 : UnityEngine.Random.Range(240 /*0xF0*/, 480);
        DataManager.Instance.WeatherType = currentData.WeatherType;
        DataManager.Instance.WeatherStrength = currentData.WeatherStrength;
      }
      Shader.EnableKeyword(currentData.ShaderKeyword);
      if ((UnityEngine.Object) this.WeatherTintOverlay != (UnityEngine.Object) null)
      {
        this.WeatherTintOverlay.enabled = true;
        this.WeatherTintOverlay.DOKill();
        Color weatherTint = currentData.WeatherTint;
        weatherTint.a *= SettingsManager.Settings.Accessibility.WeatherScreenTint;
        DOTweenModuleUI.DOColor(this.WeatherTintOverlay, weatherTint, transitionDuration);
        Debug.Log((object) $"WeatherSystemController: Overlay transition to color {weatherTint} for {this.currentWeatherType} {this.currentWeatherStrength}");
      }
      if ((UnityEngine.Object) this.blizzardOverlay != (UnityEngine.Object) null)
      {
        this.blizzardOverlay.DOKill();
        if (!GameManager.IsDungeon(PlayerFarming.Location))
        {
          this.blizzardOverlay.enabled = true;
          Color color = this.blizzardOverlay.color with
          {
            a = currentData.BlizzardOverlayOpacity * SettingsManager.Settings.Accessibility.WeatherScreenTint
          };
          this.blizzardOverlay.DOColor(color, transitionDuration).OnComplete<TweenerCore<Color, Color, ColorOptions>>((TweenCallback) (() =>
          {
            if ((double) currentData.BlizzardOverlayOpacity > 0.0)
              return;
            this.blizzardOverlay.enabled = false;
            Debug.Log((object) $"WeatherSystemController: Blizzard overlay disabled (opacity: {currentData.BlizzardOverlayOpacity}, isNotDungeon: {!GameManager.IsDungeon(PlayerFarming.Location)})");
          }));
          Debug.Log((object) $"WeatherSystemController: Blizzard overlay transition to alpha {color.a} for {this.currentWeatherType} {this.currentWeatherStrength}");
        }
        else
        {
          this.blizzardOverlay.color = this.blizzardOverlay.color with
          {
            a = currentData.BlizzardOverlayOpacity
          };
          Debug.Log((object) $"WeatherSystemController: Blizzard overlay disabled (opacity: {currentData.BlizzardOverlayOpacity}, isNotDungeon: {!GameManager.IsDungeon(PlayerFarming.Location)})");
        }
      }
      if (this.canShowFogFX)
        this.ShowFogFx(transitionDuration);
      else
        this.HideFogFx(transitionDuration);
      if ((UnityEngine.Object) particleSystem1 != (UnityEngine.Object) null)
      {
        ParticleSystem.EmissionModule emission = particleSystem1.emission with
        {
          enabled = true
        };
        Debug.Log((object) $"WeatherSystemController: Enabled emission for {particleSystem1.name} (enabled: {emission.enabled}) - tween will control rateOverTime");
      }
      else
        Debug.LogWarning((object) $"WeatherSystemController: No particle system found for weather {this.currentWeatherType} {this.currentWeatherStrength}");
      float time = 0.0f;
      Tween weatherTransitionTween = this.weatherTransitionTween;
      if (weatherTransitionTween != null)
        weatherTransitionTween.Kill();
      if ((double) transitionDuration <= 0.0)
      {
        this.UpdateHideWeatherTransition(1f, previoustData);
        this.UpdateShowWeatherTransition(1f);
        this.ChangeWinterOutlineSettings();
        AudioManager.Instance.StopLoop(this.soundLoop);
        this.soundLoop = AudioManager.Instance.CreateLoop(currentData.SoundLoop, this.gameObject, true);
        int num = (int) this.soundLoop.setVolume(currentData.Volume);
      }
      else
        this.weatherTransitionTween = (Tween) DOTween.To((DOGetter<float>) (() => time), (DOSetter<float>) (x => time = x), 1f, transitionDuration).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
        {
          this.UpdateHideWeatherTransition(time, previoustData);
          this.UpdateShowWeatherTransition(time);
        })).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
        {
          this.UpdateHideWeatherTransition(1f, previoustData);
          this.UpdateShowWeatherTransition(1f);
          this.ChangeWinterOutlineSettings();
          if (previoustData != null)
            Shader.DisableKeyword(previoustData.ShaderKeyword);
          AudioManager.Instance.StopLoop(this.soundLoop);
          this.soundLoop = AudioManager.Instance.CreateLoop(currentData.SoundLoop, this.gameObject, true);
          int num = (int) this.soundLoop.setVolume(currentData.Volume);
        })).OnKill<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
        {
          if (previoustData != null)
            Shader.DisableKeyword(previoustData.ShaderKeyword);
          AudioManager.Instance.StopLoop(this.soundLoop);
        }));
      if (currentData.HasWind)
        this.windSystem.StartWind(currentData);
      else
        this.windSystem.StopWind();
    }
    if (!updateLighting)
      return;
    LightingManager.Instance.UpdateLighting(true, forceUpdate: true);
  }

  public void HideCurrentWeather(float transitionDuration = 2f)
  {
    if (this.previousWeatherType == WeatherSystemController.WeatherType.Snowing && this.previousWeatherStrength == WeatherSystemController.WeatherStrength.Extreme)
      AudioManager.Instance.StopCurrentAtmos();
    WeatherSystemController.WeatherData currentData = this.GetWeatherData(this.currentWeatherType, this.currentWeatherStrength);
    if (currentData == null)
      return;
    if ((double) transitionDuration == 0.0)
    {
      ParticleSystem particleSystem = currentData.ParticleSystemReference ?? currentData.ParticleSystem;
      if ((UnityEngine.Object) particleSystem != (UnityEngine.Object) null)
        particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
    if ((UnityEngine.Object) this.WeatherTintOverlay != (UnityEngine.Object) null)
      DOTweenModuleUI.DOColor(this.WeatherTintOverlay, this.WeatherTintOverlay.color with
      {
        a = 0.0f
      }, transitionDuration).OnComplete<TweenerCore<Color, Color, ColorOptions>>((TweenCallback) (() =>
      {
        this.WeatherTintOverlay.enabled = false;
        Debug.Log((object) "WeatherSystemController: Disabled weather tint overlay after fade out");
      }));
    if ((UnityEngine.Object) this.blizzardOverlay != (UnityEngine.Object) null)
      this.blizzardOverlay.DOColor(this.blizzardOverlay.color with
      {
        a = 0.0f
      }, transitionDuration).OnComplete<TweenerCore<Color, Color, ColorOptions>>((TweenCallback) (() =>
      {
        this.blizzardOverlay.enabled = false;
        Debug.Log((object) "WeatherSystemController: Disabled blizzard overlay after fade out");
      }));
    this.HideFogFx(transitionDuration);
    float time = 0.0f;
    Tween weatherTransitionTween = this.weatherTransitionTween;
    if (weatherTransitionTween != null)
      weatherTransitionTween.Kill();
    if ((double) transitionDuration <= 0.0)
    {
      this.UpdateHideWeatherTransition(1f, currentData);
      AudioManager.Instance.StopLoop(this.soundLoop);
      Shader.DisableKeyword(currentData.ShaderKeyword);
      this.ChangeWinterOutlineSettings();
      ParticleSystem particleSystem = currentData.ParticleSystemReference ?? currentData.ParticleSystem;
      if ((UnityEngine.Object) particleSystem != (UnityEngine.Object) null)
        particleSystem.emission.enabled = false;
    }
    else
      this.weatherTransitionTween = (Tween) DOTween.To((DOGetter<float>) (() => time), (DOSetter<float>) (x => time = x), 1f, transitionDuration).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => this.UpdateHideWeatherTransition(time, currentData))).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
      {
        this.UpdateHideWeatherTransition(1f, currentData);
        AudioManager.Instance.StopLoop(this.soundLoop);
        Shader.DisableKeyword(currentData.ShaderKeyword);
        this.ChangeWinterOutlineSettings();
        ParticleSystem particleSystem = currentData.ParticleSystemReference ?? currentData.ParticleSystem;
        if (!((UnityEngine.Object) particleSystem != (UnityEngine.Object) null))
          return;
        particleSystem.emission.enabled = false;
      })).OnKill<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
      {
        AudioManager.Instance.StopLoop(this.soundLoop);
        Shader.DisableKeyword(currentData.ShaderKeyword);
        ParticleSystem particleSystem = currentData.ParticleSystemReference ?? currentData.ParticleSystem;
        if (!((UnityEngine.Object) particleSystem != (UnityEngine.Object) null))
          return;
        particleSystem.emission.enabled = false;
      }));
    this.windSystem.StopWind();
  }

  public void UpdateShowWeatherTransition(float time)
  {
    WeatherSystemController.WeatherData weatherData1 = this.GetWeatherData(this.currentWeatherType, this.currentWeatherStrength);
    float b = weatherData1.ParticlesOverTime * SettingsManager.Settings.Accessibility.CameraParticles;
    float a = 0.0f;
    if (this.previousWeatherType != WeatherSystemController.WeatherType.None)
    {
      WeatherSystemController.WeatherData weatherData2 = this.GetWeatherData(this.previousWeatherType, this.previousWeatherStrength);
      if (weatherData2 != null && (UnityEngine.Object) (weatherData2.ParticleSystemReference ?? weatherData2.ParticleSystem) == (UnityEngine.Object) (weatherData1.ParticleSystemReference ?? weatherData1.ParticleSystem))
      {
        a = weatherData2.ParticlesOverTime * SettingsManager.Settings.Accessibility.CameraParticles;
        Debug.Log((object) $"WeatherSystemController: Same system transition - starting from {a:F2}");
      }
    }
    float num = Mathf.Lerp(a, b, time);
    foreach (WeatherSystemController.ShaderVariable shaderVariable in weatherData1.ShaderVariables)
    {
      if ((double) shaderVariable.TargetValue != 0.0 && !shaderVariable.SetOnStartOnly && (double) Shader.GetGlobalFloat(shaderVariable.ShaderKey) != (double) shaderVariable.TargetValue)
      {
        if (shaderVariable.LerpFromCurrent)
          Shader.SetGlobalFloat(shaderVariable.ShaderKey, Mathf.Lerp(Shader.GetGlobalFloat(shaderVariable.ShaderKey), shaderVariable.TargetValue, time));
        else
          Shader.SetGlobalFloat(shaderVariable.ShaderKey, Mathf.Lerp(0.0f, shaderVariable.TargetValue, time));
      }
      else if (shaderVariable.TargetVector != Vector4.zero && !shaderVariable.SetOnStartOnly && Shader.GetGlobalVector(shaderVariable.ShaderKey) != shaderVariable.TargetVector)
      {
        if (shaderVariable.LerpFromCurrent)
          Shader.SetGlobalVector(shaderVariable.ShaderKey, Vector4.Lerp(Shader.GetGlobalVector(shaderVariable.ShaderKey), shaderVariable.TargetVector, time));
        else
          Shader.SetGlobalVector(shaderVariable.ShaderKey, Vector4.Lerp(Vector4.zero, shaderVariable.TargetVector, time));
      }
    }
    ParticleSystem particleSystem = weatherData1.ParticleSystemReference ?? weatherData1.ParticleSystem;
    if (!((UnityEngine.Object) particleSystem != (UnityEngine.Object) null))
      return;
    ParticleSystem.EmissionModule emission = particleSystem.emission with
    {
      rateOverTime = (ParticleSystem.MinMaxCurve) num,
      enabled = true
    };
    particleSystem.Play();
  }

  public void UpdateHideWeatherTransition(
    float time,
    WeatherSystemController.WeatherData weatherData)
  {
    if (weatherData == null)
      return;
    float num1 = Mathf.Lerp(weatherData.ParticlesOverTime * SettingsManager.Settings.Accessibility.CameraParticles, 0.0f, time);
    foreach (WeatherSystemController.ShaderVariable shaderVariable in weatherData.ShaderVariables)
    {
      if ((double) shaderVariable.TargetValue != 0.0 && !shaderVariable.SetOnStartOnly)
        Shader.SetGlobalFloat(shaderVariable.ShaderKey, Mathf.Lerp(shaderVariable.TargetValue, 0.0f, time));
      else if (shaderVariable.TargetVector != Vector4.zero && !shaderVariable.SetOnStartOnly)
        Shader.SetGlobalVector(shaderVariable.ShaderKey, Vector4.Lerp(shaderVariable.TargetVector, Vector4.zero, time));
    }
    ParticleSystem particleSystem = weatherData.ParticleSystemReference ?? weatherData.ParticleSystem;
    if ((UnityEngine.Object) particleSystem != (UnityEngine.Object) null)
      particleSystem.emission.rateOverTime = (ParticleSystem.MinMaxCurve) num1;
    int num2 = (int) this.soundLoop.setVolume(Mathf.Lerp(weatherData.Volume, 0.0f, time));
  }

  public void ShowFogFx(float transitionDuration, bool useTimeScale = true)
  {
    if ((UnityEngine.Object) this.BlizzardCloudPlane1 != (UnityEngine.Object) null)
    {
      this.BlizzardCloudPlane1.SetActive(true);
      Renderer renderer1 = this.BlizzardCloudPlane1.GetComponent<Renderer>();
      if ((UnityEngine.Object) renderer1 != (UnityEngine.Object) null && (UnityEngine.Object) renderer1.material != (UnityEngine.Object) null)
      {
        if (this.blizzardCloudPlane1Tween != null)
          this.blizzardCloudPlane1Tween.Kill();
        this.blizzardCloudPlane1Tween = (Tween) DOTween.To((DOGetter<float>) (() => renderer1.material.GetFloat("_MaxOpacity")), (DOSetter<float>) (x => renderer1.material.SetFloat("_MaxOpacity", x)), this._MaxOpacity, transitionDuration).SetUpdate<TweenerCore<float, float, FloatOptions>>(!useTimeScale);
      }
    }
    if (!((UnityEngine.Object) this.BlizzardCloudPlane2 != (UnityEngine.Object) null))
      return;
    this.BlizzardCloudPlane2.SetActive(true);
    Renderer renderer2 = this.BlizzardCloudPlane2.GetComponent<Renderer>();
    if (!((UnityEngine.Object) renderer2 != (UnityEngine.Object) null) || !((UnityEngine.Object) renderer2.material != (UnityEngine.Object) null))
      return;
    if (this.blizzardCloudPlane2Tween != null)
      this.blizzardCloudPlane2Tween.Kill();
    this.blizzardCloudPlane2Tween = (Tween) DOTween.To((DOGetter<float>) (() => renderer2.material.GetFloat("_MaxOpacity")), (DOSetter<float>) (x => renderer2.material.SetFloat("_MaxOpacity", x)), this._MaxOpacity, transitionDuration).SetUpdate<TweenerCore<float, float, FloatOptions>>(!useTimeScale);
  }

  public void HideFogFx(float transitionDuration, bool useTimeScale = true)
  {
    if ((UnityEngine.Object) this.BlizzardCloudPlane1 != (UnityEngine.Object) null)
    {
      if (this.blizzardCloudPlane1Tween != null)
        this.blizzardCloudPlane1Tween.Kill();
      Renderer renderer1 = this.BlizzardCloudPlane1.GetComponent<Renderer>();
      if ((UnityEngine.Object) renderer1 != (UnityEngine.Object) null && (UnityEngine.Object) renderer1.material != (UnityEngine.Object) null)
        this.blizzardCloudPlane1Tween = (Tween) DOTween.To((DOGetter<float>) (() => renderer1.material.GetFloat("_MaxOpacity")), (DOSetter<float>) (x => renderer1.material.SetFloat("_MaxOpacity", x)), 0.0f, transitionDuration).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => this.BlizzardCloudPlane1.SetActive(false))).SetUpdate<TweenerCore<float, float, FloatOptions>>(!useTimeScale);
      else
        this.BlizzardCloudPlane1.SetActive(false);
    }
    if (!((UnityEngine.Object) this.BlizzardCloudPlane2 != (UnityEngine.Object) null))
      return;
    if (this.blizzardCloudPlane2Tween != null)
      this.blizzardCloudPlane2Tween.Kill();
    Renderer renderer2 = this.BlizzardCloudPlane2.GetComponent<Renderer>();
    if ((UnityEngine.Object) renderer2 != (UnityEngine.Object) null && (UnityEngine.Object) renderer2.material != (UnityEngine.Object) null)
      this.blizzardCloudPlane2Tween = (Tween) DOTween.To((DOGetter<float>) (() => renderer2.material.GetFloat("_MaxOpacity")), (DOSetter<float>) (x => renderer2.material.SetFloat("_MaxOpacity", x)), 0.0f, transitionDuration).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => this.BlizzardCloudPlane2.SetActive(false))).SetUpdate<TweenerCore<float, float, FloatOptions>>(!useTimeScale);
    else
      this.BlizzardCloudPlane2.SetActive(false);
  }

  public void EnteredBuilding() => this.HideCurrentWeather(0.0f);

  public void ExitedBuilding() => this.ShowCurrentWeather(0.0f, false);

  public WeatherSystemController.WeatherData GetWeatherData(
    WeatherSystemController.WeatherType weatherType,
    WeatherSystemController.WeatherStrength weatherStrength)
  {
    foreach (WeatherSystemController.WeatherData weatherData in this.weatherData)
    {
      if (weatherData.WeatherType == weatherType && weatherData.WeatherStrength == weatherStrength)
        return weatherData;
    }
    return (WeatherSystemController.WeatherData) null;
  }

  public void TriggerLightningStrike()
  {
    this.TriggerLightningStrike(PlayerFarming.Instance.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 7.5f);
  }

  public void TriggerLightningStrike(Health enemy)
  {
    this.TriggerLightningStrike(enemy.transform.position);
  }

  public void TriggerLightningStrike(FollowerBrain targetFollower)
  {
    Follower follower = FollowerManager.FindFollowerByID(targetFollower.Info.ID);
    float seconds = 1.5f;
    if ((UnityEngine.Object) follower != (UnityEngine.Object) null)
    {
      if (targetFollower.HasTrait(FollowerTrait.TraitType.LightningEnthusiast))
      {
        seconds = 0.2f;
        if (follower.gameObject.activeInHierarchy)
        {
          targetFollower.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
          AudioManager.Instance.PlayOneShot("event:/dlc/follower/lightning_impact_conductive", follower.transform.position);
          this.TriggerLightningStrike(targetFollower.LastPosition);
          follower.TimedAnimation("Lightning/die", 0.7f, (System.Action) (() => follower.TimedAnimationWithDuration("Reactions/react-happy1", (System.Action) (() => targetFollower.CurrentTask?.Abort()), false)), false);
        }
      }
      else if (!targetFollower.HasTrait(FollowerTrait.TraitType.LightningEnthusiast))
      {
        follower.Die(NotificationCentre.NotificationType.StruckByLightning, dieAnimation: "Lightning/die", deadAnimation: "Lightning/dead");
        AnalyticsLogger.LogEvent(AnalyticsLogger.EventType.None, "Lightning Death", $"{targetFollower._directInfoAccess.Name} {targetFollower._directInfoAccess.SkinName}", DataManager.Instance.Followers.Count.ToString(), "");
      }
      Vector3 pos = follower.transform.position;
      GameManager.GetInstance().WaitForSeconds(seconds, (System.Action) (() =>
      {
        foreach (Follower follower1 in Follower.Followers)
        {
          Follower f = follower1;
          if (!FollowerManager.FollowerLocked(f.Brain.Info.ID) && (double) Vector3.Distance(f.transform.position, pos) < 3.0 && (UnityEngine.Object) f != (UnityEngine.Object) follower)
          {
            float num = UnityEngine.Random.value;
            f.FacePosition(pos);
            f.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
            f.TimedAnimation((double) num < 0.5 ? "Reactions/react-intimidate" : "Reactions/react-scared", (double) num < 0.5 ? 4.866667f : 1.86666667f, (System.Action) (() => f.Brain.CompleteCurrentTask()));
          }
        }
      }));
    }
    else
    {
      targetFollower.Die(NotificationCentre.NotificationType.StruckByLightning);
      AnalyticsLogger.LogEvent(AnalyticsLogger.EventType.None, "Lightning Death", $"{targetFollower._directInfoAccess.Name} {targetFollower._directInfoAccess.SkinName}", DataManager.Instance.Followers.Count.ToString(), "");
    }
    if (!((UnityEngine.Object) HUD_Manager.Instance != (UnityEngine.Object) null))
      return;
    HUD_Manager.Instance.ClearLightningTarget();
  }

  public void TriggerLightningStrike(StructureBrain structure)
  {
    if (structure.Collapse(false, struckByLightning: true))
      NotificationCentre.Instance.PlayGenericNotificationNonLocalizedParams("Notifications/StructureCollapsedFromLightning", $"<color=#FFD201>{StructuresData.LocalizedName(structure.Data.Type)}</color>");
    this.TriggerLightningStrike(structure.Data.Position);
  }

  public void TriggerLightningStrike(Vector3 position)
  {
    BiomeConstants.Instance.EmitLightningStrike(position);
    this.EmitLightningFlash();
    CameraManager.instance.ShakeCameraForDuration(1f, 1.5f, 0.1f);
    if (!((UnityEngine.Object) HUD_Manager.Instance != (UnityEngine.Object) null))
      return;
    HUD_Manager.Instance.ClearLightningTarget();
  }

  public void EmitLightningFlash()
  {
    if (!SettingsManager.Settings.Accessibility.FlashingLights)
      return;
    this.lightningFlashOverlay.gameObject.SetActive(true);
    DG.Tweening.Sequence s = DOTween.Sequence();
    s.Append((Tween) DOTweenModuleUI.DOFade(this.lightningFlashOverlay, 0.5f, 0.0f));
    s.Append((Tween) DOTweenModuleUI.DOFade(this.lightningFlashOverlay, 0.0f, 0.25f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutSine));
  }

  public ParticleSystem GetParticleSystem(WeatherSystemController.WeatherType weatherType)
  {
    foreach (WeatherSystemController.WeatherData weatherData in this.weatherData)
    {
      if (weatherData.WeatherType == weatherType)
        return weatherData.ParticleSystemReference ?? weatherData.ParticleSystem;
    }
    return (ParticleSystem) null;
  }

  public void TryPlaySnowGroundAudio(
    WeatherSystemController.WeatherType weatherType,
    WeatherSystemController.WeatherStrength weatherStrength,
    float transitionDuration)
  {
    if (!SeasonsManager.IsInitialized || weatherType != WeatherSystemController.WeatherType.Snowing || weatherType == this.currentWeatherType && weatherStrength < this.currentWeatherStrength || LocationManager.IndoorLocations.Contains(PlayerFarming.Location) || (double) transitionDuration <= 0.0)
      return;
    AudioManager.Instance.PlayOneShot("event:/dlc/env/winter/start_ground");
  }

  public void ShowBlizzardOverlay(Color targetColor, float transitionDuration = 16f)
  {
    if (!((UnityEngine.Object) this.blizzardOverlay != (UnityEngine.Object) null))
      return;
    this.blizzardOverlay.DOKill();
    this.blizzardOverlay.enabled = true;
    this.blizzardOverlay.DOColor(targetColor, transitionDuration);
  }

  [CompilerGenerated]
  public void \u003CHideFogFx\u003Eb__80_2() => this.BlizzardCloudPlane1.SetActive(false);

  [CompilerGenerated]
  public void \u003CHideFogFx\u003Eb__80_5() => this.BlizzardCloudPlane2.SetActive(false);

  [Serializable]
  public enum WeatherType
  {
    None,
    Raining,
    Windy,
    Snowing,
    Heat,
  }

  [Serializable]
  public enum WeatherStrength
  {
    Dusting = -1, // 0xFFFFFFFF
    Light = 0,
    Medium = 1,
    Heavy = 2,
    Extreme = 3,
  }

  [Serializable]
  public struct ShaderVariable
  {
    public WeatherSystemController.ShaderVariable.variableType VariableType;
    public string ShaderKey;
    public float TargetValue;
    public Vector4 TargetVector;
    public Texture2D TargetTexture;
    public bool SetOnStartOnly;
    public bool LerpFromCurrent;

    public enum variableType
    {
      None,
      Float,
      Texture,
      Vector4,
    }
  }

  [Serializable]
  public class WeatherData
  {
    public WeatherSystemController.WeatherType WeatherType;
    public WeatherSystemController.WeatherStrength WeatherStrength;
    [Range(0.0f, 1f)]
    public float Chance;
    public float ParticlesOverTime;
    public ParticleSystem ParticleSystemReference;
    public Color WeatherTint = new Color(1f, 1f, 1f, 0.15f);
    public float BlizzardOverlayOpacity;
    [Space]
    public string ShaderKeyword;
    public WeatherSystemController.ShaderVariable[] ShaderVariables;
    [Space]
    public string SoundLoop;
    public float Volume;
    public bool HasWind;
    public bool OverrideDuration;
    [CompilerGenerated]
    public ParticleSystem \u003CParticleSystem\u003Ek__BackingField;

    public ParticleSystem ParticleSystem
    {
      get => this.\u003CParticleSystem\u003Ek__BackingField;
      set => this.\u003CParticleSystem\u003Ek__BackingField = value;
    }

    public void SetWeather()
    {
      WeatherSystemController.Instance.ClearLocationWeather();
      WeatherSystemController.Instance.SetWeather(this.WeatherType, this.WeatherStrength);
    }
  }
}
