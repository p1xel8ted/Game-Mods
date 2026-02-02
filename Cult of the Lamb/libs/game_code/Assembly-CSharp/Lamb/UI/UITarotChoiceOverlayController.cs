// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UITarotChoiceOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using src.UINavigator;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UITarotChoiceOverlayController : UIMenuBase
{
  public Action<TarotCards.TarotCard> OnTarotCardSelected;
  [Header("Buttons")]
  [SerializeField]
  public MMButton _button1;
  [SerializeField]
  public MMButton _button2;
  [Header("Cards")]
  [SerializeField]
  public UITrinketCards _uiCard1;
  [SerializeField]
  public UITrinketCards _uiCard2;
  public TarotCards.TarotCard _card1;
  public TarotCards.TarotCard _card2;
  public TarotCards.TarotCard _chosenCard;
  public UITrinketCards _chosenUICard;
  [SerializeField]
  public CoopIndicatorIcon coopIndicator_card1;
  [SerializeField]
  public CoopIndicatorIcon coopIndicator_card2;

  public void Show(TarotCards.TarotCard card1, TarotCards.TarotCard card2, bool instant = false)
  {
    this._card1 = card1;
    this._card2 = card2;
    this._uiCard1.Play(this._card1);
    this._uiCard2.Play(this._card2);
    this.coopIndicator_card1.gameObject.SetActive(false);
    this.coopIndicator_card2.gameObject.SetActive(false);
    this._uiCard1.OnDeselected += (System.Action) (() =>
    {
      if (!((UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer != (UnityEngine.Object) null))
        return;
      this.coopIndicator_card1.SetIcon(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer.isLamb ? CoopIndicatorIcon.CoopIcon.Goat : CoopIndicatorIcon.CoopIcon.Lamb);
    });
    this._uiCard1.OnSelected += (System.Action) (() => this.coopIndicator_card1.SetIcon((UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer == (UnityEngine.Object) null || MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer.isLamb ? CoopIndicatorIcon.CoopIcon.Lamb : CoopIndicatorIcon.CoopIcon.Goat));
    this._uiCard2.OnDeselected += (System.Action) (() =>
    {
      if (!((UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer != (UnityEngine.Object) null))
        return;
      this.coopIndicator_card2.SetIcon(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer.isLamb ? CoopIndicatorIcon.CoopIcon.Goat : CoopIndicatorIcon.CoopIcon.Lamb);
    });
    this._uiCard2.OnSelected += (System.Action) (() => this.coopIndicator_card2.SetIcon((UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer == (UnityEngine.Object) null || MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer.isLamb ? CoopIndicatorIcon.CoopIcon.Lamb : CoopIndicatorIcon.CoopIcon.Goat));
    this.Show(instant);
  }

  public UITrinketCards GetOtherCard(UITrinketCards card)
  {
    return (UnityEngine.Object) card == (UnityEngine.Object) this._uiCard1 ? this._uiCard2 : this._uiCard1;
  }

  public override void OnShowStarted()
  {
    this._button1.onClick.AddListener((UnityAction) (() =>
    {
      this._chosenUICard = this._uiCard1;
      this.FinalizeSelection(this._card1);
    }));
    this._button2.onClick.AddListener((UnityAction) (() =>
    {
      this._chosenUICard = this._uiCard2;
      this.FinalizeSelection(this._card2);
    }));
    this._button1.Interactable = false;
    this._button2.Interactable = false;
  }

  public override IEnumerator DoShowAnimation()
  {
    UITarotChoiceOverlayController overlayController = this;
    yield return (object) new WaitForSecondsRealtime(1.5f);
    overlayController.coopIndicator_card1.SetIcon((UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer == (UnityEngine.Object) null || MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer.isLamb ? CoopIndicatorIcon.CoopIcon.Lamb : CoopIndicatorIcon.CoopIcon.Goat);
    overlayController.coopIndicator_card2.SetIcon((UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer == (UnityEngine.Object) null || MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer.isLamb ? CoopIndicatorIcon.CoopIcon.Goat : CoopIndicatorIcon.CoopIcon.Lamb);
    overlayController.coopIndicator_card1.gameObject.SetActive(true);
    overlayController.coopIndicator_card2.gameObject.SetActive(true);
    overlayController.coopIndicator_card1.transform.DOPunchScale(Vector3.one * 0.25f, 0.25f).SetUpdate<Tweener>(true);
    overlayController.coopIndicator_card2.transform.DOPunchScale(Vector3.one * 0.15f, 0.25f).SetUpdate<Tweener>(true);
    overlayController.OverrideDefault((Selectable) overlayController._button1);
    overlayController.SetActiveStateForMenu(true);
  }

  public void FinalizeSelection(TarotCards.TarotCard card)
  {
    MMVibrate.Haptic(MMVibrate.HapticTypes.Success);
    this._chosenCard = card;
    this.Hide();
  }

  public override IEnumerator DoHide()
  {
    UITarotChoiceOverlayController overlayController = this;
    if (overlayController._addToActiveMenus && (UnityEngine.Object) overlayController._canvas != (UnityEngine.Object) null && UIMenuBase.ActiveMenus.Count == 0)
    {
      System.Action onFinalMenuHide = UIMenuBase.OnFinalMenuHide;
      if (onFinalMenuHide != null)
        onFinalMenuHide();
    }
    System.Action onHide = overlayController.OnHide;
    if (onHide != null)
      onHide();
    overlayController.SetActiveStateForMenu(false);
    yield return (object) overlayController.DoHideAnimation();
    if (overlayController._addToActiveMenus && (UnityEngine.Object) overlayController._canvas != (UnityEngine.Object) null && UIMenuBase.ActiveMenus.Count == 0)
    {
      System.Action onFinalMenuHidden = UIMenuBase.OnFinalMenuHidden;
      if (onFinalMenuHidden != null)
        onFinalMenuHidden();
    }
    overlayController.gameObject.SetActive(false);
    System.Action onHidden = overlayController.OnHidden;
    if (onHidden != null)
      onHidden();
    overlayController.OnHideCompleted();
    overlayController.IsHiding = false;
  }

  public override IEnumerator DoHideAnimation()
  {
    this.coopIndicator_card1.gameObject.SetActive(false);
    this.coopIndicator_card2.gameObject.SetActive(false);
    UIManager.PlayAudio("event:/ui/close_menu");
    this._chosenUICard.GetComponentInChildren<UIWeaponCard>().InformationBox.DOFade(0.0f, 0.2f);
    this._chosenUICard.transform.DOScale(0.0f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.25f);
    UITrinketCards otherCard = this.GetOtherCard(this._chosenUICard);
    otherCard.GetComponentInChildren<UIWeaponCard>().InformationBox.DOFade(0.0f, 0.2f);
    otherCard.transform.DOScale(0.0f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.5f);
    Action<TarotCards.TarotCard> tarotCardSelected = this.OnTarotCardSelected;
    if (tarotCardSelected != null)
      tarotCardSelected(this._chosenCard);
  }

  public override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

  [CompilerGenerated]
  public void \u003CShow\u003Eb__11_0()
  {
    if (!((UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer != (UnityEngine.Object) null))
      return;
    this.coopIndicator_card1.SetIcon(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer.isLamb ? CoopIndicatorIcon.CoopIcon.Goat : CoopIndicatorIcon.CoopIcon.Lamb);
  }

  [CompilerGenerated]
  public void \u003CShow\u003Eb__11_1()
  {
    this.coopIndicator_card1.SetIcon((UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer == (UnityEngine.Object) null || MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer.isLamb ? CoopIndicatorIcon.CoopIcon.Lamb : CoopIndicatorIcon.CoopIcon.Goat);
  }

  [CompilerGenerated]
  public void \u003CShow\u003Eb__11_2()
  {
    if (!((UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer != (UnityEngine.Object) null))
      return;
    this.coopIndicator_card2.SetIcon(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer.isLamb ? CoopIndicatorIcon.CoopIcon.Goat : CoopIndicatorIcon.CoopIcon.Lamb);
  }

  [CompilerGenerated]
  public void \u003CShow\u003Eb__11_3()
  {
    this.coopIndicator_card2.SetIcon((UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer == (UnityEngine.Object) null || MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer.isLamb ? CoopIndicatorIcon.CoopIcon.Lamb : CoopIndicatorIcon.CoopIcon.Goat);
  }

  [CompilerGenerated]
  public void \u003COnShowStarted\u003Eb__13_0()
  {
    this._chosenUICard = this._uiCard1;
    this.FinalizeSelection(this._card1);
  }

  [CompilerGenerated]
  public void \u003COnShowStarted\u003Eb__13_1()
  {
    this._chosenUICard = this._uiCard2;
    this.FinalizeSelection(this._card2);
  }
}
