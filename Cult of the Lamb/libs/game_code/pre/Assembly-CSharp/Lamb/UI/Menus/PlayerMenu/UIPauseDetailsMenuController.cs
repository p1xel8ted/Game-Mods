// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Menus.PlayerMenu.UIPauseDetailsMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Lamb.UI.Menus.PlayerMenu;

public class UIPauseDetailsMenuController : UIMenuBase
{
  public void Update()
  {
    if (this._canvasGroup.interactable && InputManager.Gameplay.GetMenuButtonDown())
      this.OnCancelButtonInput();
    Time.timeScale = 0.0f;
  }

  protected override void OnShowStarted()
  {
    base.OnShowStarted();
    AudioManager.Instance.PauseActiveLoops();
    UIManager.PlayAudio("event:/ui/pause");
  }

  protected override void OnHideStarted()
  {
    base.OnHideStarted();
    AudioManager.Instance.ResumeActiveLoops();
    UIManager.PlayAudio("event:/ui/unpause");
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    this.Hide();
  }

  protected override void OnHideCompleted() => Object.Destroy((Object) this.gameObject);
}
