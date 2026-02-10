// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.UISliderControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
namespace Rewired.UI.ControlMapper;

[AddComponentMenu("")]
public class UISliderControl : UIControl
{
  public Image iconImage;
  public Slider slider;
  public bool _showIcon;
  public bool _showSlider;

  public bool showIcon
  {
    get => this._showIcon;
    set
    {
      if ((UnityEngine.Object) this.iconImage == (UnityEngine.Object) null)
        return;
      this.iconImage.gameObject.SetActive(value);
      this._showIcon = value;
    }
  }

  public bool showSlider
  {
    get => this._showSlider;
    set
    {
      if ((UnityEngine.Object) this.slider == (UnityEngine.Object) null)
        return;
      this.slider.gameObject.SetActive(value);
      this._showSlider = value;
    }
  }

  public override void SetCancelCallback(Action cancelCallback)
  {
    base.SetCancelCallback(cancelCallback);
    if (cancelCallback == null || (UnityEngine.Object) this.slider == (UnityEngine.Object) null)
      return;
    if (this.slider is ICustomSelectable)
    {
      (this.slider as ICustomSelectable).CancelEvent += (UnityAction) (() => cancelCallback());
    }
    else
    {
      EventTrigger eventTrigger = this.slider.GetComponent<EventTrigger>();
      if ((UnityEngine.Object) eventTrigger == (UnityEngine.Object) null)
        eventTrigger = this.slider.gameObject.AddComponent<EventTrigger>();
      EventTrigger.Entry entry = new EventTrigger.Entry();
      entry.callback = new EventTrigger.TriggerEvent();
      entry.eventID = EventTriggerType.Cancel;
      entry.callback.AddListener((UnityAction<BaseEventData>) (data => cancelCallback()));
      if (eventTrigger.triggers == null)
        eventTrigger.triggers = new List<EventTrigger.Entry>();
      eventTrigger.triggers.Add(entry);
    }
  }
}
