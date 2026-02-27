// Decompiled with JetBrains decompiler
// Type: src.UI.Items.TutorialMenuItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private MMButton _button;
  [SerializeField]
  private TutorialTopic _topic;
  [SerializeField]
  private TutorialAlert _alert;

  public MMButton Button => this._button;

  public TutorialTopic Topic => this._topic;

  private void Awake()
  {
    this._button.onClick.AddListener(new UnityAction(this.OnButtonClicked));
    this._alert.Configure(this._topic);
  }

  private void OnButtonClicked()
  {
    Action<TutorialMenuItem> onTopicChosen = this.OnTopicChosen;
    if (onTopicChosen == null)
      return;
    onTopicChosen(this);
  }

  public void OnSelect(BaseEventData eventData) => this._alert.TryRemoveAlert();
}
