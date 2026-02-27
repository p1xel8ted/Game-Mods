// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.UISliderControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private bool _showIcon;
  private bool _showSlider;

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
