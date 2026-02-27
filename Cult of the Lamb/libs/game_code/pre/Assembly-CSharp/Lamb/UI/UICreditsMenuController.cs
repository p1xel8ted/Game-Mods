// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UICreditsMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class UICreditsMenuController : UIMenuBase
{
  [SerializeField]
  private MMScrollRect _scrollRect;

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    this.Hide();
  }

  protected override void OnShowStarted()
  {
    base.OnShowStarted();
    UIManager.PlayAudio("event:/ui/open_menu");
    this._scrollRect.normalizedPosition = (Vector2) Vector3.one;
  }

  protected override void OnHideStarted()
  {
    base.OnHideStarted();
    UIManager.PlayAudio("event:/ui/close_menu");
    this._scrollRect.enabled = false;
  }

  protected override void OnHideCompleted() => Object.Destroy((Object) this.gameObject);
}
