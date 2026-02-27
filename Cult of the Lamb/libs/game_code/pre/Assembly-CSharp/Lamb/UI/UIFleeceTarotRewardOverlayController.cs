// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIFleeceTarotRewardOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class UIFleeceTarotRewardOverlayController : UIMenuBase
{
  [Header("Cards")]
  [SerializeField]
  private UITrinketCards _uiCard1;
  [SerializeField]
  private UITrinketCards _uiCard2;
  [SerializeField]
  private UITrinketCards _uiCard3;
  [SerializeField]
  private UITrinketCards _uiCard4;
  [Header("Misc")]
  [SerializeField]
  private GameObject _controlPrompts;
  private TarotCards.TarotCard _card1;
  private TarotCards.TarotCard _card2;
  private TarotCards.TarotCard _card3;
  private TarotCards.TarotCard _card4;

  public void Show(
    TarotCards.TarotCard card1,
    TarotCards.TarotCard card2,
    TarotCards.TarotCard card3,
    TarotCards.TarotCard card4,
    bool instant = false)
  {
    this._card1 = card1;
    this._card2 = card2;
    this._card3 = card3;
    this._card4 = card4;
    this._uiCard1.enabled = false;
    this._uiCard2.enabled = false;
    this._uiCard3.enabled = false;
    this._uiCard4.enabled = false;
    this._controlPrompts.SetActive(false);
    this.Show(instant);
  }

  protected override IEnumerator DoShowAnimation()
  {
    this._uiCard1.enabled = true;
    this._uiCard1.Play(this._card1);
    yield return (object) new WaitForSecondsRealtime(0.2f);
    this._uiCard2.enabled = true;
    this._uiCard2.Play(this._card2);
    yield return (object) new WaitForSecondsRealtime(0.2f);
    this._uiCard3.enabled = true;
    this._uiCard3.Play(this._card3);
    yield return (object) new WaitForSecondsRealtime(0.2f);
    this._uiCard4.enabled = true;
    this._uiCard4.Play(this._card4);
    yield return (object) new WaitForSecondsRealtime(1.5f);
  }

  protected override void OnShowCompleted() => this.StartCoroutine((IEnumerator) this.RunMenu());

  private IEnumerator RunMenu()
  {
    UIFleeceTarotRewardOverlayController overlayController = this;
    overlayController._controlPrompts.SetActive(true);
    while (!InputManager.UI.GetAcceptButtonDown() && !InputManager.UI.GetCancelButtonDown())
      yield return (object) null;
    overlayController._controlPrompts.SetActive(false);
    overlayController.Hide();
  }

  protected override IEnumerator DoHideAnimation()
  {
    this.HideCard(this._uiCard1);
    yield return (object) new WaitForSecondsRealtime(0.15f);
    this.HideCard(this._uiCard2);
    yield return (object) new WaitForSecondsRealtime(0.15f);
    this.HideCard(this._uiCard3);
    yield return (object) new WaitForSecondsRealtime(0.15f);
    this.HideCard(this._uiCard4);
    yield return (object) new WaitForSecondsRealtime(0.5f);
  }

  private void HideCard(UITrinketCards card)
  {
    card.transform.DOScale(0.0f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
  }

  protected override void OnHideCompleted() => Object.Destroy((Object) this.gameObject);
}
