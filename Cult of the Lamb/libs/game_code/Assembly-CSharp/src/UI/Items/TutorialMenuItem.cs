// Decompiled with JetBrains decompiler
// Type: src.UI.Items.TutorialMenuItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.Alerts;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

#nullable disable
namespace src.UI.Items;

public class TutorialMenuItem : MonoBehaviour, ISelectHandler, IEventSystemHandler
{
  public Action<TutorialMenuItem> OnTopicChosen;
  [SerializeField]
  public MMButton _button;
  [SerializeField]
  public TutorialTopic _topic;
  [SerializeField]
  public TutorialAlert _alert;

  public MMButton Button => this._button;

  public TutorialTopic Topic => this._topic;

  public void Awake()
  {
    this._button.onClick.AddListener(new UnityAction(this.OnButtonClicked));
    this._alert.Configure(this._topic);
  }

  public void OnButtonClicked()
  {
    Action<TutorialMenuItem> onTopicChosen = this.OnTopicChosen;
    if (onTopicChosen == null)
      return;
    onTopicChosen(this);
  }

  public void OnSelect(BaseEventData eventData) => this._alert.TryRemoveAlert();
}
