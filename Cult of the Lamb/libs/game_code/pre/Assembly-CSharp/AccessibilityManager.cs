// Decompiled with JetBrains decompiler
// Type: AccessibilityManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class AccessibilityManager : Singleton<AccessibilityManager>
{
  public const float kTextScaleMin = 1f;
  public const float KTextScaleMax = 1.25f;
  private const string kDitheringFadeDistance = "_GlobalDitherIntensity";
  public const float kMinDitheringFadeDistance = 0.75f;
  public const float kMaxDitheringFadeDistance = 1.75f;
  public System.Action OnTextScaleChanged;
  public System.Action OnColorblindModeChanged;
  public Action<bool> OnHoldActionToggleChanged;
  public Action<bool> OnAutoCookChanged;

  public int ColorblindMode { private set; get; }

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
}
