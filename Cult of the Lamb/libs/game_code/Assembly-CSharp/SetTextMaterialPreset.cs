// Decompiled with JetBrains decompiler
// Type: SetTextMaterialPreset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
