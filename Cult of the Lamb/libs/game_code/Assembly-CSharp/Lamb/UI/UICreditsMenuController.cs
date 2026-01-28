// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UICreditsMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class UICreditsMenuController : UIMenuBase
{
  [SerializeField]
  public MMScrollRect _scrollRect;

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    this.Hide();
  }

  public override void OnShowStarted()
  {
    base.OnShowStarted();
    UIManager.PlayAudio("event:/ui/open_menu");
    this._scrollRect.normalizedPosition = (Vector2) Vector3.one;
  }

  public override void OnHideStarted()
  {
    base.OnHideStarted();
    UIManager.PlayAudio("event:/ui/close_menu");
    this._scrollRect.enabled = false;
  }

  public override void OnHideCompleted() => Object.Destroy((Object) this.gameObject);
}
