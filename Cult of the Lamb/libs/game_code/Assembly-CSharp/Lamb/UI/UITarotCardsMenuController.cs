// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UITarotCardsMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using src.Extensions;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UITarotCardsMenuController : UIMenuBase
{
  public const string kUnlockAnimationState = "Unlock";
  public const string kShowUnlockAnimationState = "Show Unlock";
  [Header("Tarot Cards Menu")]
  [SerializeField]
  public TarotCardItem_Unlocked _tarotCardItemTemplate;
  [SerializeField]
  public MMScrollRect _scrollRect;
  [Header("Containers")]
  [SerializeField]
  public RectTransform _contentContainer;
  [SerializeField]
  public RectTransform _contentContainerCoOp;
  [SerializeField]
  public RectTransform _contentContainerDLC;
  [SerializeField]
  public GameObject _dlcHeader;
  [SerializeField]
  public TextMeshProUGUI _collectedText;
  [SerializeField]
  public TarotInfoCardController _tarotInfoCardController;
  [SerializeField]
  public UIMenuControlPrompts _controlPrompts;
  [SerializeField]
  public GameObject _soloTitleHeader;
  [SerializeField]
  public GameObject _coopTitleHeader;
  [Header("Reveal Sequence")]
  [SerializeField]
  public CanvasGroup _unlockHeaderCanvasGroup;
  [SerializeField]
  public RectTransform _front;
  public TarotCards.TarotCard _showCard = new TarotCards.TarotCard(TarotCards.Card.Count, 0);
  public TarotCards.Card[] _showCards = new TarotCards.Card[0];
  public List<TarotCardItem_Unlocked> _items = new List<TarotCardItem_Unlocked>();
  public int _tarotCount;

  public void SetCoopTitleHeader()
  {
    this._soloTitleHeader.SetActive(false);
    this._coopTitleHeader.SetActive(true);
  }

  public int tarotsTotalCount
  {
    get
    {
      return !DataManager.Instance.PlayerFoundTrinkets.Contains(TarotCards.Card.CoopBetterApart) ? DataManager.AllTrinkets.Count - TarotCards.CoopCards.Length : DataManager.AllTrinkets.Count;
    }
  }

  public void Show(TarotCards.Card newCard, bool instant = false)
  {
    this._showCard = new TarotCards.TarotCard(newCard, 0);
    this._showCards = new TarotCards.Card[1]
    {
      this._showCard.CardType
    };
    TarotCards.UnlockTrinket(newCard);
    TarotCardAnimator tarotCard = this._tarotInfoCardController.Card1.TarotCard;
    tarotCard.Configure(this._showCard);
    tarotCard.SetStaticBack();
    this._controlPrompts.HideCancelButton();
    this.Show(instant);
  }

  public void Show(TarotCards.Card[] cards, bool instant = false)
  {
    TarotCards.UnlockTrinkets(cards);
    this._showCards = cards;
    this._controlPrompts.HideCancelButton();
    this.Show(instant);
  }

  public override void OnShowStarted()
  {
    UIManager.PlayAudio("event:/ui/open_menu");
    this._controlPrompts.HideAcceptButton();
    this._unlockHeaderCanvasGroup.alpha = 0.0f;
    this._scrollRect.normalizedPosition = Vector2.one;
    List<TarotCards.Card> cardList = new List<TarotCards.Card>((IEnumerable<TarotCards.Card>) DataManager.AllTrinkets);
    for (int index = cardList.Count - 1; index >= 0; --index)
    {
      if (!DataManager.Instance.PlayerFoundTrinkets.Contains(cardList[index]) && !CoopManager.CoopActive && TarotCards.CoopCards.Contains<TarotCards.Card>(cardList[index]))
        cardList.RemoveAt(index);
    }
    foreach (TarotCards.Card card in cardList)
    {
      if (TarotCards.GetTarotIsDLC(card))
      {
        TarotCardItem_Unlocked cardItemUnlocked = this._tarotCardItemTemplate.Instantiate<TarotCardItem_Unlocked>((Transform) this._contentContainerDLC);
        cardItemUnlocked.Configure(card);
        this._items.Add(cardItemUnlocked);
      }
      else if (TarotCards.GetTarotIsCoOp(card))
      {
        TarotCardItem_Unlocked cardItemUnlocked = this._tarotCardItemTemplate.Instantiate<TarotCardItem_Unlocked>((Transform) this._contentContainerCoOp);
        cardItemUnlocked.Configure(card);
        this._items.Add(cardItemUnlocked);
      }
      else
      {
        TarotCardItem_Unlocked cardItemUnlocked = this._tarotCardItemTemplate.Instantiate<TarotCardItem_Unlocked>((Transform) this._contentContainer);
        cardItemUnlocked.Configure(card);
        this._items.Add(cardItemUnlocked);
      }
    }
    if (this._showCards.Length == 0)
    {
      this._tarotCount = Mathf.Max(DataManager.Instance.PlayerFoundTrinkets.Count, 0);
      this.OverrideDefault((Selectable) this._items[0].Selectable);
      this.ActivateNavigation();
    }
    else
      this._tarotCount = Mathf.Max(DataManager.Instance.PlayerFoundTrinkets.Count - this._showCards.Length, 0);
    string str = LocalizeIntegration.ReverseText($"{this._tarotCount}/{this.tarotsTotalCount}");
    this._collectedText.text = string.Format(LocalizationManager.GetTranslation("UI/Collected"), (object) str);
    if (GameManager.AuthenticateMajorDLC())
      return;
    this._contentContainerDLC.gameObject.SetActive(false);
    this._dlcHeader.gameObject.SetActive(false);
  }

  public override IEnumerator DoShowAnimation()
  {
    if (this._showCard.CardType != TarotCards.Card.Count)
      yield return (object) this.ShowCardSingle();
    else if (this._showCards.Length != 0)
      yield return (object) this.ShowCardsMultiple();
    else
      yield return (object) this.\u003C\u003En__0();
  }

  public IEnumerator ShowCardSingle()
  {
    UITarotCardsMenuController cardsMenuController = this;
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
    while (!InputManager.UI.GetAcceptButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
      yield return (object) null;
    cardsMenuController._controlPrompts.HideAcceptButton();
    infoCard.RectTransform.SetParent(cardsMenuController._tarotInfoCardController.transform, true);
    infoCard.RectTransform.DOAnchorPos(Vector2.zero, 0.66f).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true);
    cardsMenuController._unlockHeaderCanvasGroup.DOFade(0.0f, 0.66f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    AudioManager.Instance.PlayOneShot("event:/sermon/scroll_sermon_menu", cardsMenuController.gameObject);
    yield return (object) cardsMenuController._animator.YieldForAnimation("Show Unlock");
    yield return (object) new WaitForSecondsRealtime(0.25f);
    cardsMenuController.OverrideDefaultOnce((Selectable) target.Selectable);
    AudioManager.Instance.PlayOneShot("event:/sermon/scroll_sermon_menu", cardsMenuController.gameObject);
    yield return (object) cardsMenuController._scrollRect.DoScrollTo(target.RectTransform);
    RectTransform tarotCardContainer = target.TarotCard.RectTransform.parent as RectTransform;
    target.TarotCard.RectTransform.SetParent(cardsMenuController._scrollRect.viewport.parent, true);
    target.TarotCard.RectTransform.SetSiblingIndex(target.TarotCard.transform.parent.childCount);
    ++cardsMenuController._tarotCount;
    UIManager.PlayAudio("event:/player/new_item_sequence_close");
    yield return (object) target.TarotCard.YieldForReveal();
    string str = LocalizeIntegration.ReverseText($"{cardsMenuController._tarotCount}/{cardsMenuController.tarotsTotalCount}");
    cardsMenuController._collectedText.text = string.Format(LocalizationManager.GetTranslation("UI/Collected"), (object) str);
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
  }

  public IEnumerator ShowCardsMultiple()
  {
    UITarotCardsMenuController cardsMenuController = this;
    cardsMenuController._canvasGroup.interactable = false;
    cardsMenuController._tarotInfoCardController.enabled = false;
    cardsMenuController._scrollRect.ScrollSpeedModifier = 2f;
    cardsMenuController._controlPrompts.HideCancelButton();
    cardsMenuController.SetActiveStateForMenu(false);
    List<TarotCardItem_Unlocked> tarotItems = new List<TarotCardItem_Unlocked>();
    foreach (TarotCardItem_Unlocked cardItemUnlocked in cardsMenuController._items)
    {
      TarotCardItem_Unlocked tarotItem = cardItemUnlocked;
      if (cardsMenuController._showCards.Contains<TarotCards.Card>(tarotItem.Card.CardType))
      {
        tarotItems.Add(tarotItem);
        tarotItem.TarotCard.SetStaticBack();
      }
      if (!TarotCards.IsUnlocked(tarotItem.Card.CardType))
        tarotItem.ForceIncognitoMode();
      tarotItem.Selectable.OnDeselected += (System.Action) (() => tarotItem.Alert.gameObject.SetActive(false));
    }
    tarotItems.Sort((Comparison<TarotCardItem_Unlocked>) ((a, b) => a.RectTransform.GetSiblingIndex().CompareTo(b.RectTransform.GetSiblingIndex())));
    AudioManager.Instance.PlayOneShot("event:/sermon/scroll_sermon_menu", cardsMenuController.gameObject);
    yield return (object) cardsMenuController._animator.YieldForAnimation("Show");
    yield return (object) new WaitForSecondsRealtime(0.1f);
    for (int i = 0; i < tarotItems.Count; ++i)
    {
      float timeScale = (float) (2.0 + (double) Mathf.Floor((float) i / 3f) * 0.10000000149011612);
      ++cardsMenuController._tarotCount;
      cardsMenuController._scrollRect.ScrollSpeedModifier = timeScale;
      AudioManager.Instance.PlayOneShot("event:/sermon/scroll_sermon_menu", cardsMenuController.gameObject);
      yield return (object) cardsMenuController._scrollRect.DoScrollTo(tarotItems[i].RectTransform);
      string str = LocalizeIntegration.ReverseText($"{cardsMenuController._tarotCount}/{cardsMenuController.tarotsTotalCount}");
      cardsMenuController._collectedText.text = string.Format(LocalizationManager.GetTranslation("UI/Collected"), (object) str);
      RectTransform tarotCardContainer = tarotItems[i].TarotCard.RectTransform.parent as RectTransform;
      tarotItems[i].TarotCard.RectTransform.SetParent(cardsMenuController._scrollRect.viewport.parent, true);
      tarotItems[i].TarotCard.RectTransform.SetSiblingIndex(tarotItems[i].TarotCard.transform.parent.childCount);
      UIManager.PlayAudio("event:/player/new_item_sequence_close");
      tarotItems[i].AnimateIncognitoOut();
      yield return (object) tarotItems[i].TarotCard.YieldForReveal(timeScale - 1f);
      tarotItems[i].TarotCard.RectTransform.SetParent((Transform) tarotCardContainer, true);
      tarotItems[i].TarotCard.RectTransform.SetSiblingIndex(0);
      tarotCardContainer = (RectTransform) null;
    }
    for (int index = 0; index < tarotItems.Count; ++index)
      cardsMenuController.StartCoroutine((IEnumerator) tarotItems[index].ShowAlert());
    yield return (object) new WaitForSecondsRealtime(0.1f);
    cardsMenuController._scrollRect.ScrollSpeedModifier = 1f;
    tarotItems.LastElement<TarotCardItem_Unlocked>().Selectable.OnSelected = (System.Action) null;
    cardsMenuController.OverrideDefault((Selectable) tarotItems.LastElement<TarotCardItem_Unlocked>().Selectable);
    cardsMenuController._tarotInfoCardController.enabled = true;
    cardsMenuController.SetActiveStateForMenu(true);
    cardsMenuController._controlPrompts.ShowCancelButton();
    cardsMenuController._canvasGroup.interactable = true;
  }

  public override void OnHideStarted() => UIManager.PlayAudio("event:/ui/close_menu");

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    this.Hide();
  }

  public override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

  [CompilerGenerated]
  [DebuggerHidden]
  public IEnumerator \u003C\u003En__0() => base.DoShowAnimation();
}
