// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIRecipeConfirmationOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using src.UI.InfoCards;
using src.UINavigator;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class UIRecipeConfirmationOverlayController : UIMenuBase
{
  public System.Action OnConfirm;
  [Header("General")]
  [SerializeField]
  public RecipeInfoCard _infoCard;
  [SerializeField]
  public UIMenuControlPrompts _controlPrompts;
  [Header("Confirm Recipe")]
  [SerializeField]
  public UIHoldInteraction _holdInteraction;
  [SerializeField]
  public GameObject _background;
  [Header("New Recipe")]
  [SerializeField]
  public RectTransform _newRecipeHeaderRect;
  [SerializeField]
  public CanvasGroup _newRecipeCanvasGroup;
  [SerializeField]
  public GameObject _blurBackground;
  public bool _newRecipe;
  public InventoryItem.ITEM_TYPE _recipe;
  public Vector2 _newRecipeOrigin;

  public override void Awake()
  {
    base.Awake();
    this._canvasGroup.alpha = 0.0f;
  }

  public void Show(InventoryItem.ITEM_TYPE recipe, bool newRecipe = false, bool instant = false)
  {
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    this._controlPrompts.HideAcceptButton();
    this._controlPrompts.HideCancelButton();
    this._newRecipe = newRecipe;
    this._recipe = recipe;
    if (!newRecipe)
      this.ShowCard();
    else
      this._canvasGroup.alpha = 0.0f;
    this._newRecipeOrigin = this._newRecipeHeaderRect.anchoredPosition;
    this._newRecipeHeaderRect.anchoredPosition = (Vector2) Vector3.zero;
    this._newRecipeCanvasGroup.alpha = 0.0f;
    this._holdInteraction.gameObject.SetActive(!this._newRecipe);
    this._newRecipeHeaderRect.gameObject.SetActive(newRecipe);
    this._blurBackground.SetActive(newRecipe);
    this._background.SetActive(!newRecipe);
    this.Show(instant);
  }

  public void ShowCard()
  {
    this._infoCard.Show(this._recipe, false);
    if (this._newRecipe)
      this._controlPrompts.ShowAcceptButton();
    else
      this._controlPrompts.ShowCancelButton();
  }

  public override void OnShowCompleted()
  {
    if (!this._newRecipe)
      this.StartCoroutine((IEnumerator) this.RunMenuConfirmation());
    else
      this.StartCoroutine((IEnumerator) this.RunMenuNewRecipe());
  }

  public IEnumerator RunMenuConfirmation()
  {
    UIRecipeConfirmationOverlayController overlayController = this;
    bool cancel = false;
    yield return (object) overlayController._holdInteraction.DoHoldInteraction((Action<float>) (progress =>
    {
      float num = (float) (1.0 + 0.25 * (double) progress);
      this._infoCard.RectTransform.localScale = new Vector3(num, num, num);
      this._infoCard.RectTransform.localPosition = (Vector3) (UnityEngine.Random.insideUnitCircle * progress * this._holdInteraction.HoldTime * 2f);
      MMVibrate.RumbleContinuous(progress * 0.2f, progress * 0.2f);
    }), (System.Action) (() =>
    {
      cancel = true;
      MMVibrate.StopRumble();
    }));
    MMVibrate.StopRumble();
    overlayController._infoCard.RectTransform.DOScale(Vector3.zero, 0.3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.3f);
    if (cancel)
    {
      System.Action onCancel = overlayController.OnCancel;
      if (onCancel != null)
        onCancel();
    }
    else
    {
      System.Action onConfirm = overlayController.OnConfirm;
      if (onConfirm != null)
        onConfirm();
    }
    overlayController.Hide();
  }

  public IEnumerator RunMenuNewRecipe()
  {
    UIRecipeConfirmationOverlayController overlayController = this;
    UIManager.PlayAudio("event:/ui/map_location_appear");
    overlayController._newRecipeHeaderRect.localScale = Vector3.one * 4f;
    overlayController._newRecipeHeaderRect.DOScale(Vector3.one * 2f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    overlayController._newRecipeCanvasGroup.DOFade(1f, 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(1f);
    overlayController._newRecipeHeaderRect.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    overlayController._newRecipeHeaderRect.DOAnchorPos(overlayController._newRecipeOrigin, 0.5f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.5f);
    overlayController.ShowCard();
    while (!InputManager.UI.GetAcceptButtonDown())
      yield return (object) null;
    overlayController.Hide();
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable || this._newRecipe)
      return;
    this.Hide();
  }

  public override void OnHideStarted()
  {
    UIManager.PlayAudio("event:/unlock_building/selection_flash");
  }

  public override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
}
