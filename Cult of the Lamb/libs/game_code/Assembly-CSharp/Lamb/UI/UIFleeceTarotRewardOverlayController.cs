// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIFleeceTarotRewardOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using src.UINavigator;
using System.Collections;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class UIFleeceTarotRewardOverlayController : UIMenuBase
{
  [Header("Cards")]
  [SerializeField]
  public UITrinketCards _uiCard1;
  [SerializeField]
  public UITrinketCards _uiCard2;
  [SerializeField]
  public UITrinketCards _uiCard3;
  [SerializeField]
  public UITrinketCards _uiCard4;
  [Header("Misc")]
  [SerializeField]
  public GameObject _controlPrompts;
  public TarotCards.TarotCard _card1;
  public TarotCards.TarotCard _card2;
  public TarotCards.TarotCard _card3;
  public TarotCards.TarotCard _card4;

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

  public override IEnumerator DoShowAnimation()
  {
    if (this._card1 != null)
    {
      this._uiCard1.enabled = true;
      this._uiCard1.Play(this._card1);
      yield return (object) new WaitForSecondsRealtime(0.2f);
    }
    if (this._card2 != null)
    {
      this._uiCard2.enabled = true;
      this._uiCard2.Play(this._card2);
      yield return (object) new WaitForSecondsRealtime(0.2f);
    }
    if (this._card3 != null)
    {
      this._uiCard3.enabled = true;
      this._uiCard3.Play(this._card3);
      yield return (object) new WaitForSecondsRealtime(0.2f);
    }
    if (this._card4 != null)
    {
      this._uiCard4.enabled = true;
      this._uiCard4.Play(this._card4);
      yield return (object) new WaitForSecondsRealtime(1.5f);
    }
  }

  public override void OnShowCompleted() => this.StartCoroutine((IEnumerator) this.RunMenu());

  public IEnumerator RunMenu()
  {
    UIFleeceTarotRewardOverlayController overlayController = this;
    overlayController._controlPrompts.SetActive(true);
    while (!InputManager.UI.GetAcceptButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer) && !InputManager.UI.GetCancelButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
      yield return (object) null;
    overlayController._controlPrompts.SetActive(false);
    overlayController.Hide();
  }

  public override IEnumerator DoHideAnimation()
  {
    if (this._card1 != null)
    {
      this.HideCard(this._uiCard1);
      yield return (object) new WaitForSecondsRealtime(0.15f);
    }
    if (this._card2 != null)
    {
      this.HideCard(this._uiCard2);
      yield return (object) new WaitForSecondsRealtime(0.15f);
    }
    if (this._card3 != null)
    {
      this.HideCard(this._uiCard3);
      yield return (object) new WaitForSecondsRealtime(0.15f);
    }
    if (this._card4 != null)
    {
      this.HideCard(this._uiCard4);
      yield return (object) new WaitForSecondsRealtime(0.5f);
    }
  }

  public void HideCard(UITrinketCards card)
  {
    card.transform.DOScale(0.0f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
  }

  public override void OnHideCompleted() => Object.Destroy((Object) this.gameObject);
}
