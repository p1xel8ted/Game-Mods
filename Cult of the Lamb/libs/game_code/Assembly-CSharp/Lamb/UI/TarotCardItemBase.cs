// Decompiled with JetBrains decompiler
// Type: Lamb.UI.TarotCardItemBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.Alerts;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
namespace Lamb.UI;

[RequireComponent(typeof (RectTransform))]
public class TarotCardItemBase : 
  BaseMonoBehaviour,
  ISelectHandler,
  IEventSystemHandler,
  IDeselectHandler
{
  [SerializeField]
  public TarotCardAnimator _tarotCard;
  [SerializeField]
  public TarotCardAlertBase _alert;
  [SerializeField]
  public MMSelectable _selectable;
  public TarotCards.TarotCard _card;
  [CompilerGenerated]
  public RectTransform \u003CRectTransform\u003Ek__BackingField;

  public TarotCards.TarotCard Card => this._card;

  public TarotCards.Card Type => this._card.CardType;

  public RectTransform RectTransform
  {
    set => this.\u003CRectTransform\u003Ek__BackingField = value;
    get => this.\u003CRectTransform\u003Ek__BackingField;
  }

  public TarotCardAnimator TarotCard => this._tarotCard;

  public MMSelectable Selectable => this._selectable;

  public TarotCardAlertBase Alert => this._alert;

  public void Awake() => this.RectTransform = this.GetComponent<RectTransform>();

  public virtual void Configure(TarotCards.TarotCard card)
  {
    this._card = card;
    this._alert.Configure(card.CardType);
    this._tarotCard.Configure(card);
  }

  public virtual void Configure(TarotCards.Card card)
  {
    this.Configure(new TarotCards.TarotCard(card, 0));
  }

  public virtual void OnSelect(BaseEventData eventData) => this._alert.TryRemoveAlert();

  public virtual void OnDeselect(BaseEventData eventData)
  {
  }
}
