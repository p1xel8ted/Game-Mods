// Decompiled with JetBrains decompiler
// Type: TutorialWindowsGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TutorialWindowsGUI : BaseTutorialGUI
{
  public GamepadNavigationItem _stored_focus;

  public override void Open()
  {
    base.Open();
    if (!BaseGUI.for_gamepad)
      return;
    this.gamepad_controller.ReinitItems(false);
    this.gamepad_controller.RestoreFocus();
    if ((Object) this._stored_focus != (Object) null)
      this.gamepad_controller.SetFocusedItem(this._stored_focus);
    this._stored_focus = (GamepadNavigationItem) null;
  }

  public override void OnAboveWindowClosed() => this.Open();

  public override bool OnPressedBack()
  {
    this.OnClosePressed();
    return true;
  }

  public void OnPressedTutorialWindowOpen()
  {
    if (BaseGUI.for_gamepad)
      this._stored_focus = this.gamepad_controller.focused_item;
    this.gameObject.Deactivate();
  }

  public void OnBackBtnClicked() => this.OnClosePressed();

  public void RemoveNotAvailableTutorialItem(TutorialItemGUI tutorial_item)
  {
    TutorialItemGUI[] items = this.items;
    this.items = new TutorialItemGUI[this.items.Length - 1];
    int index1 = 0;
    for (int index2 = 0; index2 < items.Length; ++index2)
    {
      if (!((Object) items[index2] == (Object) tutorial_item))
      {
        this.items[index1] = items[index2];
        ++index1;
      }
    }
    Object.Destroy((Object) tutorial_item.gameObject);
  }
}
