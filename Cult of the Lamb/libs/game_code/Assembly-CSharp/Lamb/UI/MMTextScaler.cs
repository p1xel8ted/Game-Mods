// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MMTextScaler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof (TextMeshProUGUI))]
public class MMTextScaler : MonoBehaviour
{
  public TextMeshProUGUI _text;
  public float _originalFontSize;
  public float _originalFontSizeMin;
  public float _originalFontSizeMax;

  public void OnEnable()
  {
    Singleton<AccessibilityManager>.Instance.OnTextScaleChanged += new System.Action(this.OnTextScaleChanged);
    if ((UnityEngine.Object) this._text == (UnityEngine.Object) null)
    {
      this._text = this.GetComponent<TextMeshProUGUI>();
      this._originalFontSize = this._text.fontSize;
      this._originalFontSizeMin = this._text.fontSizeMin;
      this._originalFontSizeMax = this._text.fontSizeMax;
    }
    if (SettingsManager.Settings == null || !((UnityEngine.Object) this._text != (UnityEngine.Object) null))
      return;
    this.OnTextScaleChanged();
  }

  public void OnDisable()
  {
    if (Singleton<AccessibilityManager>.Instance == null)
      return;
    Singleton<AccessibilityManager>.Instance.OnTextScaleChanged -= new System.Action(this.OnTextScaleChanged);
  }

  public void OnTextScaleChanged()
  {
    this._text.fontSize = this._originalFontSize * SettingsManager.Settings.Accessibility.TextScale;
    this._text.fontSizeMin = this._originalFontSizeMin * SettingsManager.Settings.Accessibility.TextScale;
    this._text.fontSizeMax = this._originalFontSizeMax * SettingsManager.Settings.Accessibility.TextScale;
  }
}
