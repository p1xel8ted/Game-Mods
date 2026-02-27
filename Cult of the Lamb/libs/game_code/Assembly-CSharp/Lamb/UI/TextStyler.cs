// Decompiled with JetBrains decompiler
// Type: Lamb.UI.TextStyler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class TextStyler : MonoBehaviour
{
  [SerializeField]
  public TMP_Text _text;
  public FontStyles _fontStyles;
  public bool _checkWhichFont;
  public bool _usingFontAwesome;

  public void Awake()
  {
    if ((UnityEngine.Object) this._text != (UnityEngine.Object) null)
      this._fontStyles = this._text.fontStyle;
    if (SettingsManager.Settings == null)
      return;
    this.UpdateTextStyling(SettingsManager.Settings.Accessibility.RemoveTextStyling);
  }

  public void OnEnable()
  {
    if (SettingsManager.Settings != null)
      this.UpdateTextStyling(SettingsManager.Settings.Accessibility.RemoveTextStyling);
    Singleton<AccessibilityManager>.Instance.OnRemoveTextStylingChanged += new Action<bool>(this.OnRemoveTextStylingValueChanged);
  }

  public void OnDisable()
  {
    Singleton<AccessibilityManager>.Instance.OnRemoveTextStylingChanged -= new Action<bool>(this.OnRemoveTextStylingValueChanged);
  }

  public void OnRemoveTextStylingValueChanged(bool value) => this.UpdateTextStyling(value);

  public void UpdateTextStyling(bool value)
  {
    if ((UnityEngine.Object) this._text == (UnityEngine.Object) null)
      return;
    if (!this._checkWhichFont && this._text.font.material.name.Contains("Awesome"))
    {
      this._usingFontAwesome = true;
      this._checkWhichFont = true;
    }
    if (this._usingFontAwesome)
      return;
    if (!value)
      this._text.fontStyle = this._fontStyles;
    else
      this._text.fontStyle = FontStyles.Normal;
  }
}
