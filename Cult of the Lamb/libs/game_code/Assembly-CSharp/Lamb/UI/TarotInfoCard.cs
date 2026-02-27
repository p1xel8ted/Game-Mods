// Decompiled with JetBrains decompiler
// Type: Lamb.UI.TarotInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using src.UINavigator;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class TarotInfoCard : UIInfoCardBase<TarotCards.TarotCard>
{
  [SerializeField]
  public CanvasGroup _canvasGroup;
  [SerializeField]
  public TarotCardAnimator _tarotCard;
  [SerializeField]
  public TextMeshProUGUI _itemHeader;
  [SerializeField]
  public TextMeshProUGUI _itemLore;
  [SerializeField]
  public TextMeshProUGUI _itemDescription;
  [SerializeField]
  public RectTransform _cardContainer;

  public TarotCardAnimator TarotCard => this._tarotCard;

  public RectTransform CardContainer => this._cardContainer;

  public override void Configure(TarotCards.TarotCard card)
  {
    this._tarotCard.Configure(card);
    this._itemHeader.text = TarotCards.LocalisedName(card.CardType);
    this._itemLore.text = LocalizationManager.GetTranslation($"TarotCards/{card.CardType}/Lore");
    this._itemDescription.text = TarotCards.LocalisedDescription(card.CardType, MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer);
  }

  public override void DoShow(bool instant)
  {
    this._canvasGroup.DOKill();
    this.RectTransform.DOKill();
    this.RectTransform.anchoredPosition = (Vector2) Vector3.zero;
    if (instant)
    {
      this._canvasGroup.alpha = 1f;
    }
    else
    {
      this._canvasGroup.DOFade(1f, 0.33f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutQuart).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
      this.RectTransform.DOShakePosition(0.5f, new Vector3(15f, 0.0f, 0.0f)).SetUpdate<Tweener>(true);
    }
  }

  public override void DoHide(bool instant)
  {
    this._canvasGroup.DOKill();
    this.RectTransform.DOKill();
    this.RectTransform.anchoredPosition = (Vector2) Vector3.zero;
    if (instant)
      this._canvasGroup.alpha = 0.0f;
    else
      this._canvasGroup.DOFade(0.0f, 0.33f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutQuart).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
  }
}
