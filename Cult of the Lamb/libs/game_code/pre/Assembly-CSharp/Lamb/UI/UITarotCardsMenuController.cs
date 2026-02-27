// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UITarotCardsMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using src.Extensions;
using src.UINavigator;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UITarotCardsMenuController : UIMenuBase
{
  private const string kUnlockAnimationState = "Unlock";
  private const string kShowUnlockAnimationState = "Show Unlock";
  [Header("Tarot Cards Menu")]
  [SerializeField]
  private TarotCardItem_Unlocked _tarotCardItemTemplate;
  [SerializeField]
  private MMScrollRect _scrollRect;
  [SerializeField]
  private RectTransform _contentContainer;
  [SerializeField]
  private TextMeshProUGUI _collectedText;
  [SerializeField]
  private TarotInfoCardController _tarotInfoCardController;
  [SerializeField]
  private UIMenuControlPrompts _controlPrompts;
  [Header("Reveal Sequence")]
  [SerializeField]
  private CanvasGroup _unlockHeaderCanvasGroup;
  [SerializeField]
  private RectTransform _front;
  private TarotCards.TarotCard _showCard = new TarotCards.TarotCard(TarotCards.Card.Count, 0);
  private List<TarotCardItem_Unlocked> _items = new List<TarotCardItem_Unlocked>();

  public void Show(TarotCards.Card newCard, bool instant = false)
  {
    this._showCard = new TarotCards.TarotCard(newCard, 0);
    TarotCards.UnlockTrinket(newCard);
    TarotCardAnimator tarotCard = this._tarotInfoCardController.Card1.TarotCard;
    tarotCard.Configure(this._showCard);
    tarotCard.SetStaticBack();
    this._controlPrompts.HideCancelButton();
    this.Show(instant);
  }

  protected override void OnShowStarted()
  {
    UIManager.PlayAudio("event:/ui/open_menu");
    this._controlPrompts.HideAcceptButton();
    this._scrollRect.normalizedPosition = Vector2.one;
    foreach (TarotCards.Card allTrinket in DataManager.AllTrinkets)
    {
      TarotCardItem_Unlocked cardItemUnlocked = this._tarotCardItemTemplate.Instantiate<TarotCardItem_Unlocked>((Transform) this._contentContainer);
      cardItemUnlocked.Configure(allTrinket);
      this._items.Add(cardItemUnlocked);
    }
    this._collectedText.text = string.Format(LocalizationManager.GetTranslation("UI/Collected"), (object) $"{DataManager.Instance.PlayerFoundTrinkets.Count}/{DataManager.AllTrinkets.Count}");
    this.OverrideDefault((Selectable) this._items[0].Selectable);
    this.ActivateNavigation();
  }

  protected override IEnumerator DoShowAnimation()
  {
    UITarotCardsMenuController cardsMenuController = this;
    if (cardsMenuController._showCard.CardType != TarotCards.Card.Count)
    {
      cardsMenuController._scrollRect.vertical = false;
      TarotCardItem_Unlocked target = cardsMenuController._items[0];
      foreach (TarotCardItem_Unlocked cardItemUnlocked in cardsMenuController._items)
      {
        if (cardItemUnlocked.Type == cardsMenuController._showCard.CardType)
        {
          target = cardItemUnlocked;
          break;
        }
      }
      target.TarotCard.SetStaticBack();
      target.Alert.TryRemoveAlert();
      cardsMenuController._tarotInfoCardController.enabled = false;
      MonoSingleton<UINavigatorNew>.Instance.Clear();
      cardsMenuController.SetActiveStateForMenu(false);
      TarotInfoCard infoCard = cardsMenuController._tarotInfoCardController.Card1;
      infoCard.Configure(cardsMenuController._showCard);
      infoCard.Hide(true);
      infoCard.CanvasGroup.alpha = 0.0f;
      infoCard.RectTransform.SetParent((Transform) cardsMenuController._front);
      infoCard.RectTransform.anchoredPosition = Vector2.zero;
      TarotCardAnimator tarotCardAnimator = infoCard.TarotCard;
      tarotCardAnimator.Configure(cardsMenuController._showCard);
      tarotCardAnimator.SetStaticBack();
      tarotCardAnimator.RectTransform.SetParent((Transform) cardsMenuController._front);
      tarotCardAnimator.RectTransform.anchoredPosition = Vector2.zero;
      Vector2 localScale = (Vector2) tarotCardAnimator.RectTransform.localScale;
      tarotCardAnimator.RectTransform.localScale = (Vector3) (localScale * 0.25f);
      tarotCardAnimator.RectTransform.DOScale((Vector3) localScale, 0.3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      yield return (object) cardsMenuController._animator.YieldForAnimation("Unlock");
      yield return (object) new WaitForSecondsRealtime(0.15f);
      UIManager.PlayAudio("event:/tarot/tarot_card_reveal");
      yield return (object) tarotCardAnimator.YieldForReveal();
      yield return (object) new WaitForSecondsRealtime(0.5f);
      tarotCardAnimator.RectTransform.DOAnchorPos((Vector2) cardsMenuController._front.InverseTransformPoint(infoCard.CardContainer.TransformPoint((Vector3) Vector2.zero)), 0.5f).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true);
      yield return (object) new WaitForSecondsRealtime(0.15f);
      infoCard.CanvasGroup.DOFade(1f, 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
      yield return (object) new WaitForSecondsRealtime(0.4f);
      tarotCardAnimator.RectTransform.SetParent((Transform) infoCard.CardContainer, true);
      cardsMenuController._controlPrompts.ShowAcceptButton();
      while (!InputManager.UI.GetAcceptButtonDown())
        yield return (object) null;
      cardsMenuController._controlPrompts.HideAcceptButton();
      infoCard.RectTransform.SetParent(cardsMenuController._tarotInfoCardController.transform, true);
      infoCard.RectTransform.DOAnchorPos(Vector2.zero, 0.66f).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true);
      cardsMenuController._unlockHeaderCanvasGroup.DOFade(0.0f, 0.66f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
      yield return (object) cardsMenuController._animator.YieldForAnimation("Show Unlock");
      yield return (object) new WaitForSecondsRealtime(0.25f);
      cardsMenuController.OverrideDefaultOnce((Selectable) target.Selectable);
      yield return (object) cardsMenuController._scrollRect.DoScrollTo(target.RectTransform);
      RectTransform tarotCardContainer = target.TarotCard.RectTransform.parent as RectTransform;
      target.TarotCard.RectTransform.SetParent(cardsMenuController._scrollRect.viewport.parent, true);
      target.TarotCard.RectTransform.SetSiblingIndex(target.TarotCard.transform.parent.childCount);
      UIManager.PlayAudio("event:/player/new_item_sequence_close");
      yield return (object) target.TarotCard.YieldForReveal();
      target.TarotCard.RectTransform.SetParent((Transform) tarotCardContainer, true);
      target.TarotCard.RectTransform.SetSiblingIndex(0);
      target.Alert.gameObject.SetActive(true);
      target.Selectable.OnDeselected += (System.Action) (() => target.Alert.gameObject.SetActive(false));
      infoCard.Show(true);
      cardsMenuController._tarotInfoCardController.ForceCurrentCard(infoCard, cardsMenuController._showCard);
      cardsMenuController._tarotInfoCardController.enabled = true;
      cardsMenuController.SetActiveStateForMenu(true);
      cardsMenuController._controlPrompts.ShowCancelButton();
      cardsMenuController._scrollRect.vertical = true;
      infoCard = (TarotInfoCard) null;
      tarotCardAnimator = (TarotCardAnimator) null;
      tarotCardContainer = (RectTransform) null;
    }
    else
    {
      // ISSUE: reference to a compiler-generated method
      yield return (object) cardsMenuController.\u003C\u003En__0();
    }
  }

  protected override void OnHideStarted() => UIManager.PlayAudio("event:/ui/close_menu");

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    this.Hide();
  }

  protected override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
}
