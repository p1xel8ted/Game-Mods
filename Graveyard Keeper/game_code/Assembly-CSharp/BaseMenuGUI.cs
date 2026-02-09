// Decompiled with JetBrains decompiler
// Type: BaseMenuGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BaseMenuGUI : BaseGUI
{
  public MenuItemGUI[] items;
  public UITable ui_table;
  public SimpleUITable simple_ui_table;
  public string back_btn_text = "back";
  [Tooltip("gui elemets which should be hidden when any window is opened above this window")]
  public List<GameObject> hideable_controls;
  public List<bool> controlls_states = new List<bool>();

  public override void Init()
  {
    this.items = this.GetComponentsInChildren<MenuItemGUI>(true);
    foreach (MenuItemGUI menuItemGui in this.items)
      menuItemGui.Init(this);
    this.ui_table = this.GetComponentInChildren<UITable>(true);
    this.simple_ui_table = this.GetComponentInChildren<SimpleUITable>(true);
    base.Init();
  }

  public override void Open()
  {
    base.Open();
    MenuItemGUI menuItemGui1 = (MenuItemGUI) null;
    MenuItemGUI menuItemGui2 = (MenuItemGUI) null;
    foreach (MenuItemGUI menuItemGui3 in this.items)
    {
      menuItemGui3.Show();
      if (menuItemGui3.gameObject.activeSelf)
      {
        if ((Object) menuItemGui1 == (Object) null)
          menuItemGui1 = menuItemGui3;
        menuItemGui2 = menuItemGui3;
      }
    }
    if ((Object) menuItemGui1 != (Object) null && (Object) menuItemGui2 != (Object) null)
    {
      menuItemGui1.gamepad_item.SetCustomDirectionItem(menuItemGui2.gamepad_item, Direction.Up);
      menuItemGui2.gamepad_item.SetCustomDirectionItem(menuItemGui1.gamepad_item, Direction.Down);
    }
    foreach (MenuItemGUI menuItemGui4 in this.items)
    {
      if (!((Object) menuItemGui4.gamepad_frame == (Object) null))
        menuItemGui4.gamepad_frame.Deactivate<UIWidget>();
    }
    this.Reposition();
    this.UpdatePixelPerfect();
    if (!this.is_shown || !BaseGUI.for_gamepad)
      return;
    this.gamepad_controller.ReinitItems(true);
  }

  public override void Hide(bool play_sound = true)
  {
    foreach (MenuItemGUI menuItemGui in this.items)
      menuItemGui.OnOut(false);
    this.Reposition();
    base.Hide(play_sound);
  }

  public void Reposition()
  {
    if ((Object) this.simple_ui_table != (Object) null)
      this.simple_ui_table.Reposition();
    if (!((Object) this.ui_table != (Object) null))
      return;
    this.ui_table.Reposition();
  }

  public virtual void UpdatTip(bool select_active)
  {
    if (!BaseGUI.for_gamepad)
      return;
    this.button_tips.Print(GameKeyTip.Select(), GameKeyTip.Back(this.back_btn_text));
  }

  public void SetControllsActive(bool active)
  {
    this.hideable_controls.RemoveUnityNulls<GameObject>();
    if (active)
    {
      if (this.controlls_states.Count == 0)
        return;
      for (int index = 0; index < this.hideable_controls.Count; ++index)
        this.hideable_controls[index].SetActive(this.controlls_states[index]);
    }
    else
    {
      this.controlls_states.Clear();
      foreach (GameObject hideableControl in this.hideable_controls)
      {
        this.controlls_states.Add(hideableControl.activeSelf);
        hideableControl.Deactivate();
      }
    }
  }

  public override void OnAboveWindowClosed()
  {
    this.InitPlatformDependentStuff();
    if (!this.is_shown || !BaseGUI.for_gamepad)
      return;
    this.gamepad_controller.ReinitItems(false);
    this.gamepad_controller.RestoreFocus();
  }
}
