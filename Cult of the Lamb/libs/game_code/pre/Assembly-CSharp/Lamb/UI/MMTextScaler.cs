// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MMTextScaler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof (TextMeshProUGUI))]
public class MMTextScaler : MonoBehaviour
{
  private TextMeshProUGUI _text;
  private float _originalFontSize;
  private float _originalFontSizeMin;
  private float _originalFontSizeMax;

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

  private void OnTextScaleChanged()
  {
    this._text.fontSize = this._originalFontSize * SettingsManager.Settings.Accessibility.TextScale;
    this._text.fontSizeMin = this._originalFontSizeMin * SettingsManager.Settings.Accessibility.TextScale;
    this._text.fontSizeMax = this._originalFontSizeMax * SettingsManager.Settings.Accessibility.TextScale;
  }
}
