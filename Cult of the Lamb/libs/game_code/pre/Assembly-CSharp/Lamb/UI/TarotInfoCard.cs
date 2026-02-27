// Decompiled with JetBrains decompiler
// Type: Lamb.UI.TarotInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class TarotInfoCard : UIInfoCardBase<TarotCards.TarotCard>
{
  [SerializeField]
  private CanvasGroup _canvasGroup;
  [SerializeField]
  private TarotCardAnimator _tarotCard;
  [SerializeField]
  private TextMeshProUGUI _itemHeader;
  [SerializeField]
  private TextMeshProUGUI _itemLore;
  [SerializeField]
  private TextMeshProUGUI _itemDescription;
  [SerializeField]
  private RectTransform _cardContainer;

  public TarotCardAnimator TarotCard => this._tarotCard;

  public RectTransform CardContainer => this._cardContainer;

  public override void Configure(TarotCards.TarotCard card)
  {
    this._tarotCard.Configure(card);
    this._itemHeader.text = TarotCards.LocalisedName(card.CardType);
    this._itemLore.text = LocalizationManager.GetTranslation($"TarotCards/{card.CardType}/Lore");
    this._itemDescription.text = TarotCards.LocalisedDescription(card.CardType);
  }

  protected override void DoShow(bool instant)
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

  protected override void DoHide(bool instant)
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
