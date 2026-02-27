// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.UIControlSet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private TMP_Text title;
  private Dictionary<int, UIControl> _controls;

  private Dictionary<int, UIControl> controls
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
