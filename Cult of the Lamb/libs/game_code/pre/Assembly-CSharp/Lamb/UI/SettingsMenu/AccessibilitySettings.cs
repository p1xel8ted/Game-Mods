// Decompiled with JetBrains decompiler
// Type: Lamb.UI.SettingsMenu.AccessibilitySettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace Lamb.UI.SettingsMenu;

public class AccessibilitySettings : UISubmenuBase
{
  [Header("Accessibility Settings")]
  [SerializeField]
  private UnityEngine.UI.Slider _screenShakeSlider;
  [SerializeField]
  private MMToggle _reduceCameraMotion;
  [SerializeField]
  private UnityEngine.UI.Slider _textScaleSlider;
  [SerializeField]
  private MMToggle _animatedTextToggle;
  [SerializeField]
  private MMToggle _flashingLightsToggle;
  [SerializeField]
  private UnityEngine.UI.Slider _ditherFadeDistance;
  [SerializeField]
  private MMToggle _holdActions;
  [Header("Gameplay Modifiers")]
  [SerializeField]
  private MMToggle _autoCook;
  [SerializeField]
  private MMToggle _autoFish;

  private void Start()
  {
    this._textScaleSlider.minValue = 100f;
    this._textScaleSlider.maxValue = 125f;
    this._ditherFadeDistance.minValue = 75f;
    this._ditherFadeDistance.maxValue = 175f;
    this.Configure(SettingsManager.Settings.Accessibility);
    this._screenShakeSlider.onValueChanged.AddListener(new UnityAction<float>(this.OnScreenshakeSensitivityChanged));
    this._reduceCameraMotion.OnValueChanged += new Action<bool>(this.OnReduceCameraMotionToggled);
    this._textScaleSlider.onValueChanged.AddListener(new UnityAction<float>(this.OnTextScaleSliderValueChanged));
    this._animatedTextToggle.OnValueChanged += new Action<bool>(this.OnAnimatedTextValueChanged);
    this._flashingLightsToggle.OnValueChanged += new Action<bool>(this.OnFlashingLightsValueChanged);
    this._ditherFadeDistance.onValueChanged.AddListener(new UnityAction<float>(this.OnDitherFadeDistanceValueChanged));
    this._holdActions.OnValueChanged += new Action<bool>(this.OnHoldActionsValueChanged);
    this._autoCook.OnValueChanged += new Action<bool>(this.OnAutoCookValueChanged);
    this._autoFish.OnValueChanged += new Action<bool>(this.OnAutoFishValueChanged);
  }

  public void Configure(
    SettingsData.AccessibilitySettings accessibilitySettings)
  {
    this._screenShakeSlider.value = accessibilitySettings.ScreenShake * 100f;
    this._reduceCameraMotion.Value = accessibilitySettings.ReduceCameraMotion;
    this._textScaleSlider.value = accessibilitySettings.TextScale * 100f;
    this._animatedTextToggle.Value = accessibilitySettings.AnimatedText;
    this._flashingLightsToggle.Value = accessibilitySettings.FlashingLights;
    this._ditherFadeDistance.value = accessibilitySettings.DitherFadeDistance * 100f;
    this._holdActions.Value = accessibilitySettings.HoldActions;
    this._autoCook.Value = accessibilitySettings.AutoCook;
    this._autoFish.Value = accessibilitySettings.AutoFish;
  }

  public void Reset()
  {
    SettingsManager.Settings.Accessibility = new SettingsData.AccessibilitySettings();
  }

  private void OnScreenshakeSensitivityChanged(float screenshakeIntensity)
  {
    screenshakeIntensity /= 100f;
    SettingsManager.Settings.Accessibility.ScreenShake = screenshakeIntensity;
    Debug.Log((object) $"AccessibilitySettings - Change screenshake intensity to {screenshakeIntensity}");
  }

  private void OnReduceCameraMotionToggled(bool value)
  {
    SettingsManager.Settings.Accessibility.ReduceCameraMotion = value;
    if ((UnityEngine.Object) CameraFollowTarget.Instance != (UnityEngine.Object) null)
      CameraFollowTarget.Instance.CamWobbleSettings = (float) (1 - value.ToInt());
    Debug.Log((object) $"AccessibilitySettings - Change Reduce Camera motion to {value}");
  }

  private void OnTextScaleSliderValueChanged(float value)
  {
    value /= 100f;
    Singleton<AccessibilityManager>.Instance.UpdateTextScale(value);
    Debug.Log((object) $"AccessibilitySettings - Text Scale changed to {value}");
  }

  private void OnAnimatedTextValueChanged(bool value)
  {
    Debug.Log((object) $"AccessibilitySettings - Animated text value changed to {value}".Colour(Color.yellow));
    SettingsManager.Settings.Accessibility.AnimatedText = value;
    AccessibilityManager.UpdateTextStyling();
  }

  private void OnFlashingLightsValueChanged(bool value)
  {
    Debug.Log((object) $"AccessibilitySettings - Flashing lights value changed to {value}".Colour(Color.yellow));
    SettingsManager.Settings.Accessibility.FlashingLights = value;
  }

  private void OnDitherFadeDistanceValueChanged(float value)
  {
    value /= 100f;
    Debug.Log((object) $"AccessibilitySettings - Dithering Fade value changed to {value}".Colour(Color.yellow));
    SettingsManager.Settings.Accessibility.DitherFadeDistance = value;
    AccessibilityManager.UpdateDitheringFadeDistance(value);
  }

  private void OnHoldActionsValueChanged(bool value)
  {
    Debug.Log((object) $"AccessibilitySettings - Hold Actions value changed to {value}".Colour(Color.yellow));
    Singleton<AccessibilityManager>.Instance.UpdateHoldActionsToggle(value);
  }

  private void OnAutoCookValueChanged(bool value)
  {
    Debug.Log((object) $"AccessibilitySettings - Auto Cook value changed to {value}".Colour(Color.yellow));
    Singleton<AccessibilityManager>.Instance.UpdateAutoCook(value);
  }

  private void OnAutoFishValueChanged(bool value)
  {
    Debug.Log((object) $"AccessibilitySettings - Auto Fish value changed to {value}".Colour(Color.yellow));
    SettingsManager.Settings.Accessibility.AutoFish = value;
  }
}
