// Decompiled with JetBrains decompiler
// Type: Lamb.UI.SettingsMenu.AccessibilitySettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI.MainMenu;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace Lamb.UI.SettingsMenu;

public class AccessibilitySettings : UISubmenuBase
{
  public static System.Action OnWeaponForceChange;
  public static int RotOffAccessibility = Shader.PropertyToID("ROT_OFF_ACCESSIBILITY");
  [Header("Graphics")]
  [SerializeField]
  public UnityEngine.UI.Slider _screenShakeSlider;
  [SerializeField]
  public MMToggle _reduceCameraMotion;
  [SerializeField]
  public UnityEngine.UI.Slider _textScaleSlider;
  [SerializeField]
  public MMToggle _animatedTextToggle;
  [SerializeField]
  public MMToggle _flashingLightsToggle;
  [SerializeField]
  public UnityEngine.UI.Slider _ditherFadeDistance;
  [SerializeField]
  public MMToggle _holdActions;
  [SerializeField]
  public MMSelectable_Toggle _dyslexicToggle;
  [SerializeField]
  public MMToggle _dyslexicFontToggle;
  [SerializeField]
  public MMToggle _romanNumerals;
  [SerializeField]
  public MMToggle _highConstrastFonts;
  [SerializeField]
  public MMToggle _showBuildModeFilter;
  [SerializeField]
  public MMToggle _removeTextStyling;
  [SerializeField]
  public MMToggle _removeLightingEffects;
  [SerializeField]
  public MMToggle _darkModeToggle;
  [SerializeField]
  public MMToggle _coopVisualIndicatorToggle;
  [SerializeField]
  public UnityEngine.UI.Slider _cameraParticles;
  [SerializeField]
  public UnityEngine.UI.Slider _weatherScreenTint;
  [Header("Gameplay Modifiers")]
  [SerializeField]
  public MMToggle _autoCook;
  [SerializeField]
  public MMToggle _autoFish;
  [SerializeField]
  public MMToggle _autoCraft;
  [SerializeField]
  public MMSelectable_Toggle _stopTimeInCrusdeToggle;
  [SerializeField]
  public MMSelectable_HorizontalSelector _movementModeSelector;
  [SerializeField]
  public MMToggle _invertMovement;
  [SerializeField]
  public MMToggle _unlimitedHP;
  [SerializeField]
  public MMToggle _unlimitedFervour;
  [SerializeField]
  public MMSelectable_Toggle _forceWeaponToggleContainer;
  [SerializeField]
  public MMToggle _forceWeapon;
  [SerializeField]
  public MMSelectable_HorizontalSelector _forcedWeapon;
  public List<EquipmentType> weapons = new List<EquipmentType>();

  public void Start()
  {
    this._textScaleSlider.minValue = 100f;
    this._textScaleSlider.maxValue = 125f;
    this._ditherFadeDistance.minValue = 75f;
    this._ditherFadeDistance.maxValue = 175f;
    this._movementModeSelector.HorizontalSelector.LocalizeContent = true;
    this._movementModeSelector.HorizontalSelector.PrefillContent("UI/Settings/Accessibility/MovementStyle/Default", "UI/Settings/Accessibility/MovementStyle/AlwaysRun");
    this._forcedWeapon.HorizontalSelector.LocalizeContent = true;
    this._forcedWeapon.HorizontalSelector.PrefillContent(this.GetForceWeaponsOptions());
    this._coopVisualIndicatorToggle.GetComponentInParent<MMSelectable_Toggle>().gameObject.SetActive(CoopManager.CoopActive);
    this.Configure(SettingsManager.Settings.Accessibility);
    this._screenShakeSlider.onValueChanged.AddListener(new UnityAction<float>(this.OnScreenshakeSensitivityChanged));
    this._reduceCameraMotion.OnValueChanged += new Action<bool>(this.OnReduceCameraMotionToggled);
    this._textScaleSlider.onValueChanged.AddListener(new UnityAction<float>(this.OnTextScaleSliderValueChanged));
    this._animatedTextToggle.OnValueChanged += new Action<bool>(this.OnAnimatedTextValueChanged);
    this._flashingLightsToggle.OnValueChanged += new Action<bool>(this.OnFlashingLightsValueChanged);
    this._ditherFadeDistance.onValueChanged.AddListener(new UnityAction<float>(this.OnDitherFadeDistanceValueChanged));
    this._holdActions.OnValueChanged += new Action<bool>(this.OnHoldActionsValueChanged);
    this._autoCook.OnValueChanged += new Action<bool>(this.OnAutoCookValueChanged);
    this._autoCraft.OnValueChanged += new Action<bool>(this.OnAutoCraftValueChanged);
    this._autoFish.OnValueChanged += new Action<bool>(this.OnAutoFishValueChanged);
    this._dyslexicFontToggle.OnValueChanged += new Action<bool>(this.OnDyslexicFontValueChanged);
    this._romanNumerals.OnValueChanged += new Action<bool>(this.OnRomanNumeralsValueChanged);
    this._stopTimeInCrusdeToggle.Toggle.OnValueChanged += new Action<bool>(this.OnStopTimeInCrusadeValueChanged);
    this._highConstrastFonts.OnValueChanged += new Action<bool>(this.OnHighContrastFontsValueChanged);
    this._showBuildModeFilter.OnValueChanged += new Action<bool>(this.OnBuildModeFilterValueChanged);
    this._removeTextStyling.OnValueChanged += new Action<bool>(this.OnRemoveTextStylingValueChanged);
    this._removeLightingEffects.OnValueChanged += new Action<bool>(this.OnRemoveLightingEffectsValueChanges);
    this._darkModeToggle.OnValueChanged += new Action<bool>(this.OnDarkModeToggleValueChanged);
    this._movementModeSelector.HorizontalSelector.OnSelectionChanged += new Action<int>(this.OnMovementModeValueChanged);
    this._invertMovement.OnValueChanged += new Action<bool>(this.OnInvertMovementValueChanged);
    this._unlimitedHP.OnValueChanged += new Action<bool>(this.OnUnlimitedHPSettingValueChanged);
    this._unlimitedFervour.OnValueChanged += new Action<bool>(this.OnUnlimitedFervourSettingValueChanged);
    this._coopVisualIndicatorToggle.OnValueChanged += new Action<bool>(this.OnCoopVisualIndicatorsSettingValueChanged);
    this._forceWeapon.OnValueChanged += new Action<bool>(this.OnForceWeaponSettingValueChanged);
    this._forcedWeapon.HorizontalSelector.OnSelectionChanged += new Action<int>(this.OnForcedWeaponSettingValueChanged);
    this._cameraParticles.onValueChanged.AddListener(new UnityAction<float>(this.OnCameraParticlesValueChanged));
    this._weatherScreenTint.onValueChanged.AddListener(new UnityAction<float>(this.OnWeatherScreenTintValueChanged));
  }

  public override void SetActiveStateForMenu(GameObject target, bool state)
  {
    base.SetActiveStateForMenu(target, state);
    this._dyslexicToggle.Interactable = SettingsManager.Settings.Game.Language == "English";
    if (!PlayerFleeceManager.FleecePreventsForcedWeapons() && !LocationManager.IsDungeonActive())
      return;
    this._forceWeapon.Value = false;
    this._forceWeapon.Interactable = false;
    this._forceWeaponToggleContainer.Interactable = false;
    this._forcedWeapon.Interactable = false;
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
    this._autoCraft.Value = accessibilitySettings.AutoCraft;
    this._autoFish.Value = accessibilitySettings.AutoFish;
    this._dyslexicFontToggle.Value = accessibilitySettings.DyslexicFont;
    this._romanNumerals.Value = accessibilitySettings.RomanNumerals;
    this._stopTimeInCrusdeToggle.Toggle.Value = accessibilitySettings.StopTimeInCrusade;
    this._highConstrastFonts.Value = accessibilitySettings.HighContrastText;
    this._showBuildModeFilter.Value = accessibilitySettings.ShowBuildModeFilter;
    this._removeTextStyling.Value = accessibilitySettings.RemoveTextStyling;
    this._removeLightingEffects.Value = accessibilitySettings.RemoveLightingEffects;
    this._darkModeToggle.Value = accessibilitySettings.DarkMode;
    this._movementModeSelector.HorizontalSelector.ContentIndex = accessibilitySettings.MovementMode;
    this._invertMovement.Value = accessibilitySettings.InvertMovement;
    this._unlimitedHP.Value = accessibilitySettings.UnlimitedHP;
    this._unlimitedFervour.Value = accessibilitySettings.UnlimitedFervour;
    this._coopVisualIndicatorToggle.Value = accessibilitySettings.CoopVisualIndicators;
    this._forceWeapon.Value = accessibilitySettings.ForceWeapon;
    this._forcedWeapon.HorizontalSelector.ContentIndex = accessibilitySettings.ForcedWeapon;
    this._cameraParticles.value = accessibilitySettings.CameraParticles * 100f;
    this._weatherScreenTint.value = accessibilitySettings.WeatherScreenTint * 100f;
    if (DataManager.Instance.WeaponPool.Count == 1)
    {
      this._forceWeapon.transform.parent.transform.parent.gameObject.SetActive(false);
      this._forcedWeapon.gameObject.SetActive(false);
    }
    else
    {
      this._forceWeapon.transform.parent.transform.parent.gameObject.SetActive(true);
      this._forcedWeapon.gameObject.SetActive(true);
    }
    foreach (object activeMenu in UIMenuBase.ActiveMenus)
    {
      if (activeMenu.GetType() == typeof (UIMainMenuController))
      {
        this._forceWeapon.transform.parent.transform.parent.gameObject.SetActive(false);
        this._forcedWeapon.gameObject.SetActive(false);
        break;
      }
    }
  }

  public void Reset()
  {
    SettingsManager.Settings.Accessibility = new SettingsData.AccessibilitySettings();
  }

  public void OnCameraParticlesValueChanged(float value)
  {
    value /= 100f;
    SettingsManager.Settings.Accessibility.CameraParticles = value;
    Singleton<AccessibilityManager>.Instance.SetCameraParticles(value);
    Debug.Log((object) $"AccessibilitySettings - Change Camera Particles to {value}");
  }

  public void OnWeatherScreenTintValueChanged(float value)
  {
    value /= 100f;
    SettingsManager.Settings.Accessibility.WeatherScreenTint = value;
    Singleton<AccessibilityManager>.Instance.SetWeatherScreenTint(value);
    Debug.Log((object) $"AccessibilitySettings - Change Weather Screen Tint to {value}");
  }

  public void OnHighContrastFontsValueChanged(bool value)
  {
    SettingsManager.Settings.Accessibility.HighContrastText = value;
    Singleton<AccessibilityManager>.Instance.SetHighContrastText(value);
    Debug.Log((object) $"AccessibilitySettings - Change HighContrastText to {value}");
  }

  public void OnScreenshakeSensitivityChanged(float screenshakeIntensity)
  {
    screenshakeIntensity /= 100f;
    SettingsManager.Settings.Accessibility.ScreenShake = screenshakeIntensity;
    Debug.Log((object) $"AccessibilitySettings - Change screenshake intensity to {screenshakeIntensity}");
  }

  public void OnReduceCameraMotionToggled(bool value)
  {
    SettingsManager.Settings.Accessibility.ReduceCameraMotion = value;
    if ((UnityEngine.Object) CameraFollowTarget.Instance != (UnityEngine.Object) null)
      CameraFollowTarget.Instance.CamWobbleSettings = (float) (1 - value.ToInt());
    Debug.Log((object) $"AccessibilitySettings - Change Reduce Camera motion to {value}");
  }

  public void OnTextScaleSliderValueChanged(float value)
  {
    value /= 100f;
    Singleton<AccessibilityManager>.Instance.UpdateTextScale(value);
    Debug.Log((object) $"AccessibilitySettings - Text Scale changed to {value}");
  }

  public void OnAnimatedTextValueChanged(bool value)
  {
    Debug.Log((object) $"AccessibilitySettings - Animated text value changed to {value}".Colour(Color.yellow));
    SettingsManager.Settings.Accessibility.AnimatedText = value;
    AccessibilityManager.UpdateTextStyling();
  }

  public void OnFlashingLightsValueChanged(bool value)
  {
    Debug.Log((object) $"AccessibilitySettings - Flashing lights value changed to {value}".Colour(Color.yellow));
    SettingsManager.Settings.Accessibility.FlashingLights = value;
    int num = value ? 0 : 1;
    Debug.Log((object) $"AccessibilitySettings - Setting ROT_OFF_ACCESSIBILITY shader global to {num}".Colour(Color.yellow));
    Shader.SetGlobalInt(AccessibilitySettings.RotOffAccessibility, num);
  }

  public void OnDitherFadeDistanceValueChanged(float value)
  {
    value /= 100f;
    Debug.Log((object) $"AccessibilitySettings - Dithering Fade value changed to {value}".Colour(Color.yellow));
    SettingsManager.Settings.Accessibility.DitherFadeDistance = value;
    AccessibilityManager.UpdateDitheringFadeDistance(value);
  }

  public void OnHoldActionsValueChanged(bool value)
  {
    Debug.Log((object) $"AccessibilitySettings - Hold Actions value changed to {value}".Colour(Color.yellow));
    Singleton<AccessibilityManager>.Instance.UpdateHoldActionsToggle(value);
  }

  public void OnAutoCraftValueChanged(bool value)
  {
    Debug.Log((object) $"AccessibilitySettings - Auto Craft value changed to {value}".Colour(Color.yellow));
    Singleton<AccessibilityManager>.Instance.UpdateAutoCraft(value);
  }

  public void OnAutoCookValueChanged(bool value)
  {
    Debug.Log((object) $"AccessibilitySettings - Auto Cook value changed to {value}".Colour(Color.yellow));
    Singleton<AccessibilityManager>.Instance.UpdateAutoCook(value);
  }

  public void OnAutoFishValueChanged(bool value)
  {
    Debug.Log((object) $"AccessibilitySettings - Auto Fish value changed to {value}".Colour(Color.yellow));
    SettingsManager.Settings.Accessibility.AutoFish = value;
  }

  public void OnDyslexicFontValueChanged(bool value)
  {
    Debug.Log((object) $"AccessibilitySettings - Dyslexic Font Value changed to {value}".Colour(Color.yellow));
    Singleton<AccessibilityManager>.Instance.UpdateDyslexicFontSetting(value);
    LocalizationManager.LocalizeAll(true);
  }

  public void OnRomanNumeralsValueChanged(bool value)
  {
    Debug.Log((object) $"AccessibilitySettings - Roman Numerals Value changed to {value}".Colour(Color.yellow));
    Singleton<AccessibilityManager>.Instance.UpdateRomanNumerals(value);
  }

  public void OnWorldTimeScaleChanged(float value)
  {
    value /= 100f;
    Debug.Log((object) $"AccessibilitySettings - World Time Scale Value changed to {value}".Colour(Color.yellow));
    Singleton<AccessibilityManager>.Instance.UpdateWorldTimeScale(value);
  }

  public void OnStopTimeInCrusadeValueChanged(bool value)
  {
    Debug.Log((object) $"AccessibilitySettings - Pause Time In Crusade Value changed to {value}".Colour(Color.yellow));
    Singleton<AccessibilityManager>.Instance.UpdateStopTimeInCrusades(value);
  }

  public void OnBuildModeFilterValueChanged(bool value)
  {
    Debug.Log((object) $"AccessibilitySettings - Show Build Mode filter value changed to {value}".Colour(Color.yellow));
    Singleton<AccessibilityManager>.Instance.UpdateBuildModeFilter(value);
  }

  public void OnRemoveTextStylingValueChanged(bool value)
  {
    Debug.Log((object) $"AccessibilitySettings - Remove Text Styling value changed to {value}".Colour(Color.yellow));
    Singleton<AccessibilityManager>.Instance.UpdateRemoveTextStyling(value);
  }

  public void OnRemoveLightingEffectsValueChanges(bool value)
  {
    Debug.Log((object) $"AccessibilitySettings - Remove Text Styling value changed to {value}".Colour(Color.yellow));
    Singleton<AccessibilityManager>.Instance.UpdateLightingEffectsSetting(value);
  }

  public void OnDarkModeToggleValueChanged(bool value)
  {
    Debug.Log((object) $"AccessibilitySettings - Dark Mode value changed to {value}".Colour(Color.yellow));
    SettingsManager.Settings.Accessibility.DarkMode = value;
    GraphicsSettingsUtilities.SetDarkMode();
  }

  public void OnMovementModeValueChanged(int value)
  {
    Debug.Log((object) $"AccessibilitySettings - Movement Mode value changed to {value}".Colour(Color.yellow));
    SettingsManager.Settings.Accessibility.MovementMode = value;
  }

  public void OnInvertMovementValueChanged(bool value)
  {
    Debug.Log((object) $"AccessibilitySettings - Invert Movement value changed to {value}".Colour(Color.yellow));
    SettingsManager.Settings.Accessibility.InvertMovement = value;
  }

  public void OnUnlimitedHPSettingValueChanged(bool value)
  {
    Debug.Log((object) $"AccessibilitySettings - Unlimited HP value changed to {value}".Colour(Color.yellow));
    SettingsManager.Settings.Accessibility.UnlimitedHP = value;
  }

  public void OnUnlimitedFervourSettingValueChanged(bool value)
  {
    Debug.Log((object) $"AccessibilitySettings - Unlimited Fervour value changed to {value}".Colour(Color.yellow));
    SettingsManager.Settings.Accessibility.UnlimitedFervour = value;
  }

  public void OnCoopVisualIndicatorsSettingValueChanged(bool value)
  {
    Debug.Log((object) $"AccessibilitySettings - Coop Visual Indicators changed to {value}".Colour(Color.yellow));
    SettingsManager.Settings.Accessibility.CoopVisualIndicators = value;
  }

  public void OnForceWeaponSettingValueChanged(bool value)
  {
    System.Action weaponForceChange = AccessibilitySettings.OnWeaponForceChange;
    if (weaponForceChange != null)
      weaponForceChange();
    Debug.Log((object) $"AccessibilitySettings - Force Weapon value changed to: {value}".Colour(Color.yellow));
    SettingsManager.Settings.Accessibility.ForceWeapon = value;
  }

  public void OnForcedWeaponSettingValueChanged(int value)
  {
    System.Action weaponForceChange = AccessibilitySettings.OnWeaponForceChange;
    if (weaponForceChange != null)
      weaponForceChange();
    int forceWeaponIndex = this.GetForceWeaponIndex(value);
    Debug.Log((object) $"AccessibilitySettings - Forced Weapon Changed to: {forceWeaponIndex}".Colour(Color.yellow));
    SettingsManager.Settings.Accessibility.ForcedWeapon = forceWeaponIndex;
  }

  public string[] GetForceWeaponsOptions()
  {
    List<string> stringList = new List<string>();
    if (DataManager.Instance != null && DataManager.Instance.WeaponPool.Count > 0)
    {
      if (DataManager.Instance.WeaponPool.Contains(EquipmentType.Sword))
        stringList.Add("TarotCards/Sword/Name");
      if (DataManager.Instance.WeaponPool.Contains(EquipmentType.Axe))
        stringList.Add("TarotCards/Axe/Name");
      if (DataManager.Instance.WeaponPool.Contains(EquipmentType.Dagger))
        stringList.Add("TarotCards/Dagger/Name");
      if (DataManager.Instance.WeaponPool.Contains(EquipmentType.Gauntlet))
        stringList.Add("TarotCards/Gauntlet/Name");
      if (DataManager.Instance.WeaponPool.Contains(EquipmentType.Hammer))
        stringList.Add("TarotCards/Hammer/Name");
      if (DataManager.Instance.WeaponPool.Contains(EquipmentType.Blunderbuss))
        stringList.Add("UpgradeSystem/Blunderbuss/Name");
      if (DataManager.Instance.WeaponPool.Contains(EquipmentType.Chain) && DataManager.Instance.MAJOR_DLC)
        stringList.Add("UpgradeSystem/Chain/Name");
      return stringList.ToArray();
    }
    return new string[7]
    {
      "TarotCards/Sword/Name",
      "TarotCards/Axe/Name",
      "TarotCards/Dagger/Name",
      "TarotCards/Gauntlet/Name",
      "TarotCards/Hammer/Name",
      "UpgradeSystem/Blunderbuss/Name",
      "UpgradeSystem/Chain/Name"
    };
  }

  public int GetForceWeaponIndex(int optionIndex)
  {
    List<int> intList = new List<int>();
    if (DataManager.Instance == null || DataManager.Instance.WeaponPool.Count <= 0)
      return optionIndex;
    if (DataManager.Instance.WeaponPool.Contains(EquipmentType.Sword))
      intList.Add(0);
    if (DataManager.Instance.WeaponPool.Contains(EquipmentType.Axe))
      intList.Add(1);
    if (DataManager.Instance.WeaponPool.Contains(EquipmentType.Dagger))
      intList.Add(2);
    if (DataManager.Instance.WeaponPool.Contains(EquipmentType.Gauntlet))
      intList.Add(3);
    if (DataManager.Instance.WeaponPool.Contains(EquipmentType.Hammer))
      intList.Add(4);
    if (DataManager.Instance.WeaponPool.Contains(EquipmentType.Blunderbuss))
      intList.Add(5);
    if (DataManager.Instance.WeaponPool.Contains(EquipmentType.Chain))
      intList.Add(6);
    return intList[optionIndex];
  }
}
