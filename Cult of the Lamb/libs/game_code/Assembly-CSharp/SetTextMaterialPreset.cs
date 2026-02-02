// Decompiled with JetBrains decompiler
// Type: SetTextMaterialPreset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (TextMeshProUGUI))]
public class SetTextMaterialPreset : MonoBehaviour
{
  [SerializeField]
  public Material materialPreset;
  public TextMeshProUGUI _text;

  public void Awake() => this._text = this.GetComponent<TextMeshProUGUI>();

  public void OnEnable()
  {
    Singleton<AccessibilityManager>.Instance.OnDyslexicFontValueChanged += new Action<bool>(this.OnDyslexicFontSettingChanged);
    if (SettingsManager.Settings.Accessibility.DyslexicFont)
      return;
    this.UpdateMaterial();
  }

  public void UpdateMaterial()
  {
    if (!((UnityEngine.Object) this.materialPreset != (UnityEngine.Object) null))
      return;
    this._text.fontSharedMaterial = this.materialPreset;
  }

  public void OnDisable()
  {
    Singleton<AccessibilityManager>.Instance.OnDyslexicFontValueChanged -= new Action<bool>(this.OnDyslexicFontSettingChanged);
  }

  public void OnDyslexicFontSettingChanged(bool state) => this.UpdateMaterial();
}
