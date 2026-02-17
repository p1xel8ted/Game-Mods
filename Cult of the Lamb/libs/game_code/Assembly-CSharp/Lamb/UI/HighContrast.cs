// Decompiled with JetBrains decompiler
// Type: Lamb.UI.HighContrast
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.Assets;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class HighContrast : MonoBehaviour
{
  [SerializeField]
  public HighContrastConfiguration _configuration;
  public HighContrastTarget _target;

  public void Awake()
  {
    Singleton<AccessibilityManager>.Instance.OnHighContrastTextChanged += new Action<bool>(this.OnHighContrastSettingChanged);
    SelectableColourProxy component1;
    if (this.TryGetComponent<SelectableColourProxy>(out component1))
    {
      this._target = (HighContrastTarget) new SelectableColorProxyHighContrastTarget(component1, this._configuration);
    }
    else
    {
      Selectable component2;
      if (this.TryGetComponent<Selectable>(out component2))
      {
        if (component2.transition == Selectable.Transition.ColorTint)
          this._target = (HighContrastTarget) new SelectableColorTransitionHighContrastTarget(component2, this._configuration);
        else if (component2.transition == Selectable.Transition.Animation)
          this._target = (HighContrastTarget) new SelectableAnimatedHighContrastTarget(component2, this._configuration);
      }
      else
      {
        TMP_Text component3;
        if (this.TryGetComponent<TMP_Text>(out component3))
          this._target = (HighContrastTarget) new TextHighContrastTarget(component3, this._configuration);
      }
    }
    if (this._target != null)
      this._target.Init();
    if (SettingsManager.Settings == null || SettingsManager.Settings.Accessibility == null || !SettingsManager.Settings.Accessibility.HighContrastText)
      return;
    this.OnHighContrastSettingChanged(true);
  }

  public void OnDestroy()
  {
    Singleton<AccessibilityManager>.Instance.OnHighContrastTextChanged -= new Action<bool>(this.OnHighContrastSettingChanged);
  }

  public void OnHighContrastSettingChanged(bool state)
  {
    if (this._target == null)
      return;
    this._target.Apply(state);
  }
}
