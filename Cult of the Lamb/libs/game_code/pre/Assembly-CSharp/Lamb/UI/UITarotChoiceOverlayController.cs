// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UITarotChoiceOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
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
  private MMButton _button1;
  [SerializeField]
  private MMButton _button2;
  [Header("Cards")]
  [SerializeField]
  private UITrinketCards _uiCard1;
  [SerializeField]
  private UITrinketCards _uiCard2;
  private TarotCards.TarotCard _card1;
  private TarotCards.TarotCard _card2;
  private TarotCards.TarotCard _chosenCard;
  private UITrinketCards _chosenUICard;

  public void Show(TarotCards.TarotCard card1, TarotCards.TarotCard card2, bool instant = false)
  {
    this._card1 = card1;
    this._card2 = card2;
    this._uiCard1.Play(this._card1);
    this._uiCard2.Play(this._card2);
    this.Show(instant);
  }

  private UITrinketCards GetOtherCard(UITrinketCards card)
  {
    return (UnityEngine.Object) card == (UnityEngine.Object) this._uiCard1 ? this._uiCard2 : this._uiCard1;
  }

  protected override void OnShowStarted()
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

  protected override IEnumerator DoShowAnimation()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    UITarotChoiceOverlayController overlayController = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      overlayController.OverrideDefault((Selectable) overlayController._button1);
      overlayController.SetActiveStateForMenu(true);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSecondsRealtime(1.5f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private void FinalizeSelection(TarotCards.TarotCard card)
  {
    MMVibrate.Haptic(MMVibrate.HapticTypes.Success);
    this._chosenCard = card;
    this.Hide();
  }

  protected override IEnumerator DoHideAnimation()
  {
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

  protected override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
}
