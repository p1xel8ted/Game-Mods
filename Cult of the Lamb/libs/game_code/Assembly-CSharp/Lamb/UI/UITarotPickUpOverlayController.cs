// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UITarotPickUpOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class UITarotPickUpOverlayController : UIMenuBase
{
  [SerializeField]
  public UITrinketCards _uiCard;
  public TarotCards.TarotCard _card;

  public void Show(TarotCards.TarotCard card, bool instant = false)
  {
    this._card = card;
    this._uiCard.Play(this._card);
    this.Show(instant);
  }

  public override void OnShowStarted() => this.StartCoroutine((IEnumerator) this.UpdateLoop());

  public override void OnHideCompleted() => Object.Destroy((Object) this.gameObject);

  public IEnumerator UpdateLoop()
  {
    UITarotPickUpOverlayController overlayController = this;
    while (!InputManager.UI.GetAcceptButtonDown() && !InputManager.UI.GetCancelButtonDown())
      yield return (object) null;
    overlayController.Hide();
  }
}
