// Decompiled with JetBrains decompiler
// Type: DyslexicFontPreferredSizeChanger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class DyslexicFontPreferredSizeChanger : MonoBehaviour
{
  [SerializeField]
  public bool _changePreferredWidth;
  [SerializeField]
  public float _dyslexicFontPreferredWidth;
  [SerializeField]
  public bool _changePreferredHeight;
  [SerializeField]
  public float _dyslexicFontPreferredHeight;
  public LayoutElement _layoutElement;
  public Vector2 _defaultPreferredSize;
  public bool _initialized;

  public void Awake()
  {
    this._layoutElement = this.GetComponent<LayoutElement>();
    if ((UnityEngine.Object) this._layoutElement == (UnityEngine.Object) null)
      return;
    this._defaultPreferredSize = new Vector2(this._layoutElement.preferredWidth, this._layoutElement.preferredHeight);
    this._initialized = true;
    if (SettingsManager.Settings == null)
      return;
    this.UpdatePreferredSize(SettingsManager.Settings.Accessibility.DyslexicFont);
  }

  public void OnEnable()
  {
    if (!this._initialized || Singleton<AccessibilityManager>.Instance == null)
      return;
    Singleton<AccessibilityManager>.Instance.OnDyslexicFontValueChanged += new Action<bool>(this.UpdatePreferredSize);
  }

  public void OnDisable()
  {
    if (!this._initialized || Singleton<AccessibilityManager>.Instance == null)
      return;
    Singleton<AccessibilityManager>.Instance.OnDyslexicFontValueChanged -= new Action<bool>(this.UpdatePreferredSize);
  }

  public void UpdatePreferredSize(bool useDyslexicFont)
  {
    if (this._changePreferredWidth)
      this._layoutElement.preferredWidth = useDyslexicFont ? this._dyslexicFontPreferredWidth : this._defaultPreferredSize.x;
    if (!this._changePreferredHeight)
      return;
    this._layoutElement.preferredHeight = useDyslexicFont ? this._dyslexicFontPreferredHeight : this._defaultPreferredSize.y;
  }
}
