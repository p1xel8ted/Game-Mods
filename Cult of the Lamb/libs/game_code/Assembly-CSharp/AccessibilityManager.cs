// Decompiled with JetBrains decompiler
// Type: AccessibilityManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using src;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class AccessibilityManager : Singleton<AccessibilityManager>
{
  public const float kTextScaleMin = 1f;
  public const float KTextScaleMax = 1.25f;
  public const string kDitheringFadeDistance = "_GlobalDitherIntensity";
  public const float kMinDitheringFadeDistance = 0.75f;
  public const float kMaxDitheringFadeDistance = 1.75f;
  public const float kWorldTimeScaleMin = 0.25f;
  public const float kWorldTimeScaleMax = 3f;
  public System.Action OnTextScaleChanged;
  public System.Action OnColorblindModeChanged;
  public Action<bool> OnRomanNumeralsChanged;
  public Action<bool> OnHighContrastTextChanged;
  public Action<float> OnCameraParticlesChanged;
  public Action<float> OnWeatherScreenTintChanged;
  public Action<bool> OnHoldActionToggleChanged;
  public Action<bool> OnAutoCookChanged;
  public Action<bool> OnAutoCraftChanged;
  public Action<float> OnWorldTimeScaleChanged;
  public Action<bool> OnStopTimeInCrusadeChanged;
  public Action<bool> OnBuildModeFilterChanged;
  public Action<bool> OnRemoveTextStylingChanged;
  public Action<bool> OnDyslexicFontValueChanged;
  [CompilerGenerated]
  public int \u003CColorblindMode\u003Ek__BackingField;

  public int ColorblindMode
  {
    set => this.\u003CColorblindMode\u003Ek__BackingField = value;
    get => this.\u003CColorblindMode\u003Ek__BackingField;
  }

  public List<string> AllColorblindModes
  {
    get
    {
      return new List<string>()
      {
        "UI/Settings/Graphics/ColorBlindModes/none",
        "UI/Settings/Graphics/ColorBlindModes/Deuteranopia",
        "UI/Settings/Graphics/ColorBlindModes/Protanopia",
        "UI/Settings/Graphics/ColorBlindModes/Tritanopia",
        "UI/Settings/Graphics/ColorBlindModes/Gray L Red",
        "UI/Settings/Graphics/ColorBlindModes/Gray M Green",
        "UI/Settings/Graphics/ColorBlindModes/Gray S Blue",
        "UI/Settings/Graphics/ColorBlindModes/Doge"
      };
    }
  }

  public void UpdateTextScale(float scale)
  {
    SettingsManager.Settings.Accessibility.TextScale = scale;
    System.Action textScaleChanged = this.OnTextScaleChanged;
    if (textScaleChanged == null)
      return;
    textScaleChanged();
  }

  public void DispatchAll()
  {
    System.Action textScaleChanged = this.OnTextScaleChanged;
    if (textScaleChanged == null)
      return;
    textScaleChanged();
  }

  public void SetHighContrastText(bool value)
  {
    Action<bool> contrastTextChanged = this.OnHighContrastTextChanged;
    if (contrastTextChanged == null)
      return;
    contrastTextChanged(value);
  }

  public void SetCameraParticles(float value)
  {
    SettingsManager.Settings.Accessibility.CameraParticles = value;
    Action<float> particlesChanged = this.OnCameraParticlesChanged;
    if (particlesChanged == null)
      return;
    particlesChanged(value);
  }

  public void SetWeatherScreenTint(float value)
  {
    SettingsManager.Settings.Accessibility.WeatherScreenTint = value;
    Action<float> screenTintChanged = this.OnWeatherScreenTintChanged;
    if (screenTintChanged == null)
      return;
    screenTintChanged(value);
  }

  public void SetColorblindMode(int mode)
  {
    this.ColorblindMode = mode;
    System.Action colorblindModeChanged = this.OnColorblindModeChanged;
    if (colorblindModeChanged == null)
      return;
    colorblindModeChanged();
  }

  public static void UpdateTextStyling()
  {
    Singleton<AccessibilityManager>.Instance.UpdateTextScale(SettingsManager.Settings.Accessibility.TextScale);
    TextAnimatorSettings.Animated = SettingsManager.Settings.Accessibility.AnimatedText;
  }

  public static void UpdateDitheringFadeDistance(float ditheringFadeDistance)
  {
    Shader.SetGlobalFloat("_GlobalDitherIntensity", ditheringFadeDistance);
  }

  public void UpdateHoldActionsToggle(bool value)
  {
    Action<bool> actionToggleChanged = this.OnHoldActionToggleChanged;
    if (actionToggleChanged != null)
      actionToggleChanged(value);
    SettingsManager.Settings.Accessibility.HoldActions = value;
  }

  public void UpdateAutoCook(bool value)
  {
    Action<bool> onAutoCookChanged = this.OnAutoCookChanged;
    if (onAutoCookChanged != null)
      onAutoCookChanged(value);
    SettingsManager.Settings.Accessibility.AutoCook = value;
  }

  public void UpdateAutoCraft(bool value)
  {
    Action<bool> autoCraftChanged = this.OnAutoCraftChanged;
    if (autoCraftChanged != null)
      autoCraftChanged(value);
    SettingsManager.Settings.Accessibility.AutoCraft = value;
  }

  public void UpdateRomanNumerals(bool value)
  {
    SettingsManager.Settings.Accessibility.RomanNumerals = value;
    Action<bool> romanNumeralsChanged = this.OnRomanNumeralsChanged;
    if (romanNumeralsChanged == null)
      return;
    romanNumeralsChanged(value);
  }

  public void UpdateWorldTimeScale(float value)
  {
    SettingsManager.Settings.Accessibility.WorldTimeScale = value;
    Action<float> timeScaleChanged = this.OnWorldTimeScaleChanged;
    if (timeScaleChanged == null)
      return;
    timeScaleChanged(value);
  }

  public void UpdateStopTimeInCrusades(bool value)
  {
    SettingsManager.Settings.Accessibility.StopTimeInCrusade = value;
    Action<bool> inCrusadeChanged = this.OnStopTimeInCrusadeChanged;
    if (inCrusadeChanged == null)
      return;
    inCrusadeChanged(value);
  }

  public void UpdateBuildModeFilter(bool value)
  {
    SettingsManager.Settings.Accessibility.ShowBuildModeFilter = value;
    Action<bool> modeFilterChanged = this.OnBuildModeFilterChanged;
    if (modeFilterChanged == null)
      return;
    modeFilterChanged(value);
  }

  public void UpdateRemoveTextStyling(bool value)
  {
    SettingsManager.Settings.Accessibility.RemoveTextStyling = value;
    Action<bool> textStylingChanged = this.OnRemoveTextStylingChanged;
    if (textStylingChanged == null)
      return;
    textStylingChanged(value);
  }

  public void UpdateDyslexicFontSetting(bool value)
  {
    SettingsManager.Settings.Accessibility.DyslexicFont = value;
    Action<bool> fontValueChanged = this.OnDyslexicFontValueChanged;
    if (fontValueChanged == null)
      return;
    fontValueChanged(value);
  }

  public void UpdateLightingEffectsSetting(bool value)
  {
    SettingsManager.Settings.Accessibility.RemoveLightingEffects = value;
    if (!((UnityEngine.Object) LightingManager.Instance != (UnityEngine.Object) null) || DeathCatRoomMarker.IsDeathCatRoom)
      return;
    float durationMultiplier = LightingManager.Instance.transitionDurationMultiplier;
    LightingManager.Instance.transitionDurationMultiplier = 0.0f;
    LightingManager.Instance.UpdateLighting(true);
    LightingManager.Instance.transitionDurationMultiplier = durationMultiplier;
  }
}
