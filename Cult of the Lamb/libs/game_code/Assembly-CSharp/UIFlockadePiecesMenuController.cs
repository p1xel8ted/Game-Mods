// Decompiled with JetBrains decompiler
// Type: UIFlockadePiecesMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Flockade;
using I2.Loc;
using Lamb.UI;
using src.Extensions;
using src.UI.InfoCards;
using src.UI.Items;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UIFlockadePiecesMenuController : UIMenuBase
{
  public const string kUnlockAnimationState = "Unlock";
  public const string kShowUnlockAnimationState = "Show Unlock";
  [SerializeField]
  public MMScrollRect _scrollRect;
  [SerializeField]
  public RectTransform _contentContainer;
  [SerializeField]
  public TMP_Text _collectedText;
  [SerializeField]
  public UIMenuControlPrompts _controlPrompts;
  [SerializeField]
  public FlockadeInfoCardController _infoCardController;
  [Header("Reveal Sequence")]
  [SerializeField]
  public CanvasGroup _unlockHeaderCanvasGroup;
  [SerializeField]
  public RectTransform _front;
  [SerializeField]
  public GameObject _soloTitleHeader;
  [SerializeField]
  public GameObject _coopTitleHeader;
  public List<FlockadePieceItem> _items = new List<FlockadePieceItem>();
  public FlockadeGamePieceConfiguration _showPiece;
  public List<FlockadeGamePieceConfiguration> _showPieces;
  public int _piecesCount;
  public PlayerFarming playerFarming;
  public const int piecesTotalCount = 36;

  public void SetCoopTitleHeader()
  {
    this._soloTitleHeader.SetActive(false);
    this._coopTitleHeader.SetActive(true);
  }

  public void Show(FlockadePieceType pieceType, PlayerFarming playerFarming, bool instant = false)
  {
    this.playerFarming = playerFarming;
    this._showPiece = FlockadePieceManager.GetPiecesPool().GetPiece(pieceType);
    this._piecesCount = Mathf.Clamp(FlockadePieceManager.GetUnlockedPiecesCount(), 0, int.MaxValue);
    this.Show(instant);
  }

  public void Show(List<FlockadePieceType> pieceTypes, PlayerFarming playerFarming, bool instant = false)
  {
    this.playerFarming = playerFarming;
    this.Show(FlockadePieceManager.GetPiecesPool().GetPieces((ICollection<FlockadePieceType>) pieceTypes).ToList<FlockadeGamePieceConfiguration>(), instant);
  }

  public void Show(List<FlockadeGamePieceConfiguration> pieceData, bool instant = false)
  {
    this._showPieces = pieceData;
    this._piecesCount = Mathf.Clamp(FlockadePieceManager.GetUnlockedPiecesCount() - this._showPieces.Count, 0, int.MaxValue);
    this.Show(instant);
  }

  public override void OnShowStarted()
  {
    UIManager.PlayAudio("event:/ui/open_menu");
    this._controlPrompts.HideAcceptButton();
    this._scrollRect.normalizedPosition = Vector2.one;
    List<FlockadeGamePieceConfiguration> pieceConfigurationList = new List<FlockadeGamePieceConfiguration>(FlockadePieceManager.GetPiecesPool().GetAllPieces());
    pieceConfigurationList.Sort(new Comparison<FlockadeGamePieceConfiguration>(this.ComparePieceType));
    foreach (FlockadeGamePieceConfiguration data in pieceConfigurationList)
    {
      FlockadePieceItem flockadePieceItem = MonoSingleton<UIManager>.Instance.FlockadePieceItemTemplate.Instantiate<FlockadePieceItem>((Transform) this._contentContainer);
      flockadePieceItem.Configure(data);
      this._items.Add(flockadePieceItem);
    }
    if ((UnityEngine.Object) this._showPiece == (UnityEngine.Object) null && this._showPieces == null)
    {
      this._piecesCount = Mathf.Clamp(FlockadePieceManager.GetUnlockedPiecesCount(), 0, int.MaxValue);
      this.OverrideDefault((Selectable) this._items[0].Selectable);
      this.ActivateNavigation();
    }
    this._collectedText.text = string.Format(LocalizationManager.GetTranslation("UI/Collected"), (object) $"{this._piecesCount}/{36}");
  }

  public override IEnumerator DoShowAnimation()
  {
    if ((UnityEngine.Object) this._showPiece != (UnityEngine.Object) null)
      yield return (object) this.DoUnlockAnimationSingle();
    else if (this._showPieces != null && this._showPieces.Count > 0)
      yield return (object) this.DoUnlockAnimationMulti();
    else
      yield return (object) this.\u003C\u003En__0();
  }

  public IEnumerator DoUnlockAnimationSingle()
  {
    UIFlockadePiecesMenuController piecesMenuController = this;
    piecesMenuController._canvasGroup.interactable = false;
    piecesMenuController._scrollRect.vertical = false;
    piecesMenuController._controlPrompts.HideCancelButton();
    FlockadePieceItem target = piecesMenuController._items[0];
    foreach (FlockadePieceItem flockadePieceItem in piecesMenuController._items)
    {
      if (flockadePieceItem.Data.Type == piecesMenuController._showPiece.Type)
      {
        target = flockadePieceItem;
        target.ForceLockedState();
        target.Alert.TryRemoveAlert();
      }
      else
        flockadePieceItem.ForceIncognitoState();
    }
    piecesMenuController._infoCardController.enabled = false;
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    piecesMenuController.SetActiveStateForMenu(false);
    FlockadePieceInfoCard infoCard = piecesMenuController._infoCardController.Card1;
    Vector2 originalInfoCardPosition = infoCard.RectTransform.anchoredPosition;
    infoCard.Configure(piecesMenuController._showPiece);
    infoCard.Hide(true);
    infoCard.Animator.enabled = false;
    infoCard.CanvasGroup.alpha = 0.0f;
    infoCard.RectTransform.SetParent((Transform) piecesMenuController._front);
    infoCard.RectTransform.anchoredPosition = Vector2.zero;
    Transform originalParent = infoCard.GamePiece.parent;
    RectTransform iconContainer = infoCard.GamePiece;
    CanvasGroup iconCanvasGroup = infoCard.GamePieceContainer;
    Vector2 originalScale = (Vector2) iconContainer.localScale;
    Vector2 originalPosition = (Vector2) iconContainer.localPosition;
    iconContainer.SetParent((Transform) piecesMenuController._front);
    iconCanvasGroup.alpha = 0.0f;
    iconContainer.localScale = (Vector3) (originalScale * 3f);
    iconContainer.localPosition = (Vector3) Vector2.zero;
    yield return (object) piecesMenuController._animator.YieldForAnimation("Unlock");
    yield return (object) new WaitForSecondsRealtime(0.15f);
    iconContainer.DOScale((Vector3) (originalScale * 5f), 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    iconCanvasGroup.DOFade(1f, 0.5f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutBack).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/new_piece_reveal_single");
    yield return (object) new WaitForSecondsRealtime(0.75f);
    iconContainer.DOLocalMove((Vector3) (Vector2) piecesMenuController._front.InverseTransformPoint(originalParent.TransformPoint((Vector3) originalPosition)), 0.65f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
    iconContainer.DOScale((Vector3) originalScale, 0.65f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
    yield return (object) new WaitForSecondsRealtime(0.15f);
    infoCard.CanvasGroup.DOFade(1f, 0.75f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.55f);
    iconContainer.SetParent(originalParent);
    piecesMenuController._controlPrompts.ShowAcceptButton();
    while (!InputManager.UI.GetAcceptButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
      yield return (object) null;
    piecesMenuController._controlPrompts.HideAcceptButton();
    AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/new_piece_accept_single");
    infoCard.RectTransform.SetParent(piecesMenuController._infoCardController.transform, true);
    infoCard.RectTransform.DOAnchorPos(originalInfoCardPosition, 0.66f).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true);
    piecesMenuController._unlockHeaderCanvasGroup.DOFade(0.0f, 0.66f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    yield return (object) piecesMenuController._animator.YieldForAnimation("Show Unlock");
    yield return (object) new WaitForSecondsRealtime(0.25f);
    piecesMenuController.OverrideDefaultOnce((Selectable) target.Selectable);
    yield return (object) piecesMenuController._scrollRect.DoScrollTo(target.RectTransform);
    ++piecesMenuController._piecesCount;
    UIManager.PlayAudio("event:/player/new_item_sequence_close");
    yield return (object) target.DoUnlock();
    piecesMenuController._collectedText.text = string.Format(LocalizationManager.GetTranslation("UI/Collected"), (object) $"{piecesMenuController._piecesCount}/{36}");
    target.Alert.gameObject.SetActive(true);
    target.Selectable.OnDeselected += (System.Action) (() => target.Alert.gameObject.SetActive(false));
    infoCard.Animator.enabled = true;
    infoCard.Show(true);
    piecesMenuController._infoCardController.ForceCurrentCard(infoCard, piecesMenuController._showPiece);
    piecesMenuController._infoCardController.enabled = true;
    piecesMenuController.SetActiveStateForMenu(true);
    piecesMenuController._controlPrompts.ShowCancelButton();
    piecesMenuController._scrollRect.vertical = true;
    piecesMenuController._canvasGroup.interactable = true;
  }

  public override void OnHideStarted()
  {
    base.OnHideStarted();
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    UIManager.PlayAudio("event:/ui/close_menu");
  }

  public IEnumerator DoUnlockAnimationMulti()
  {
    UIFlockadePiecesMenuController piecesMenuController = this;
    piecesMenuController._canvasGroup.interactable = false;
    piecesMenuController._infoCardController.enabled = false;
    piecesMenuController._scrollRect.ScrollSpeedModifier = 2f;
    piecesMenuController._controlPrompts.HideCancelButton();
    List<FlockadePieceItem> pieceItems = new List<FlockadePieceItem>();
    foreach (FlockadePieceItem flockadePieceItem in piecesMenuController._items)
    {
      FlockadePieceItem item = flockadePieceItem;
      item.Selectable.Interactable = false;
      if (piecesMenuController._showPieces.Contains(item.Data))
      {
        pieceItems.Add(item);
        item.ForceLockedState();
      }
      else
        item.ForceIncognitoState();
      item.Selectable.OnDeselected += (System.Action) (() => item.Alert.gameObject.SetActive(false));
    }
    pieceItems.Sort((Comparison<FlockadePieceItem>) ((a, b) => a.RectTransform.GetSiblingIndex().CompareTo(b.RectTransform.GetSiblingIndex())));
    yield return (object) piecesMenuController._animator.YieldForAnimation("Show");
    yield return (object) new WaitForSecondsRealtime(0.1f);
    for (int i = 0; i < pieceItems.Count; ++i)
    {
      float num = (float) (2.0 + (double) Mathf.Floor((float) i / 3f) * 0.30000001192092896);
      piecesMenuController._scrollRect.ScrollSpeedModifier = num;
      ++piecesMenuController._piecesCount;
      yield return (object) piecesMenuController._scrollRect.DoScrollTo(pieceItems[i].RectTransform);
      piecesMenuController._collectedText.text = string.Format(LocalizationManager.GetTranslation("UI/Collected"), (object) $"{piecesMenuController._piecesCount}/{36}");
      yield return (object) pieceItems[i].Flash();
    }
    for (int index = 0; index < pieceItems.Count; ++index)
      piecesMenuController.StartCoroutine((IEnumerator) pieceItems[index].ShowAlert());
    yield return (object) new WaitForSecondsRealtime(0.1f);
    piecesMenuController._scrollRect.ScrollSpeedModifier = 1f;
    piecesMenuController.OverrideDefault((Selectable) pieceItems.LastElement<FlockadePieceItem>().Selectable);
    piecesMenuController._infoCardController.enabled = true;
    piecesMenuController.SetActiveStateForMenu(true);
    piecesMenuController._controlPrompts.ShowCancelButton();
    piecesMenuController._canvasGroup.interactable = true;
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    this.Hide();
  }

  public override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

  public int ComparePieceType(
    FlockadeGamePieceConfiguration piece0,
    FlockadeGamePieceConfiguration piece1)
  {
    if (piece0.Type > piece1.Type)
      return 1;
    return piece0.Type < piece1.Type ? -1 : 0;
  }

  [CompilerGenerated]
  [DebuggerHidden]
  public IEnumerator \u003C\u003En__0() => base.DoShowAnimation();
}
