// Decompiled with JetBrains decompiler
// Type: Lamb.UI.TarotCardItemBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI.Alerts;
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
  protected TarotCardAnimator _tarotCard;
  [SerializeField]
  protected TarotCardAlertBase _alert;
  [SerializeField]
  protected MMSelectable _selectable;
  private TarotCards.TarotCard _card;

  public TarotCards.TarotCard Card => this._card;

  public TarotCards.Card Type => this._card.CardType;

  public RectTransform RectTransform { private set; get; }

  public TarotCardAnimator TarotCard => this._tarotCard;

  public MMSelectable Selectable => this._selectable;

  public TarotCardAlertBase Alert => this._alert;

  private void Awake() => this.RectTransform = this.GetComponent<RectTransform>();

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
