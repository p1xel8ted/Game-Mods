// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.UIControlSet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace Rewired.UI.ControlMapper;

[AddComponentMenu("")]
public class UIControlSet : MonoBehaviour
{
  [SerializeField]
  public TMP_Text title;
  public Dictionary<int, UIControl> _controls;

  public Dictionary<int, UIControl> controls
  {
    get => this._controls ?? (this._controls = new Dictionary<int, UIControl>());
  }

  public void SetTitle(string text)
  {
    if ((UnityEngine.Object) this.title == (UnityEngine.Object) null)
      return;
    this.title.text = text;
  }

  public T GetControl<T>(int uniqueId) where T : UIControl
  {
    UIControl control;
    this.controls.TryGetValue(uniqueId, out control);
    return control as T;
  }

  public UISliderControl CreateSlider(
    GameObject prefab,
    Sprite icon,
    float minValue,
    float maxValue,
    Action<int, float> valueChangedCallback,
    Action<int> cancelCallback)
  {
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
    UISliderControl control = gameObject.GetComponent<UISliderControl>();
    if ((UnityEngine.Object) control == (UnityEngine.Object) null)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) gameObject);
      Debug.LogError((object) "Prefab missing UISliderControl component!");
      return (UISliderControl) null;
    }
    gameObject.transform.SetParent(this.transform, false);
    if ((UnityEngine.Object) control.iconImage != (UnityEngine.Object) null)
      control.iconImage.sprite = icon;
    if ((UnityEngine.Object) control.slider != (UnityEngine.Object) null)
    {
      control.slider.minValue = minValue;
      control.slider.maxValue = maxValue;
      if (valueChangedCallback != null)
        control.slider.onValueChanged.AddListener((UnityAction<float>) (value => valueChangedCallback(control.id, value)));
      if (cancelCallback != null)
        control.SetCancelCallback((Action) (() => cancelCallback(control.id)));
    }
    this.controls.Add(control.id, (UIControl) control);
    return control;
  }
}
