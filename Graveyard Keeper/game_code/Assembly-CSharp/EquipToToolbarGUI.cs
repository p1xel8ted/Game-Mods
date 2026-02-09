// Decompiled with JetBrains decompiler
// Type: EquipToToolbarGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class EquipToToolbarGUI : BaseGUI
{
  public GameObject dark_back;
  public BaseItemCellGUI cursor_item;
  public Camera _gui_cam;
  public ToolbarGUI _toolbar;
  public bool _update_icon;
  public Transform _cursor_tf;
  public BaseItemCellGUI _prev_cell_gui;

  public BaseItemCellGUI[] keyboard_cells => this._toolbar.keyboard.cells;

  public override void Init()
  {
    this._toolbar = this.GetComponentInChildren<ToolbarGUI>(true);
    this._toolbar.Init();
    this.cursor_item.interaction_enabled = false;
    this.cursor_item.x1.tooltip.available = false;
    this._cursor_tf = this.cursor_item.transform;
    base.Init();
  }

  public void Open(Item item, bool from_inventory)
  {
    this.Open();
    this._toolbar.Redraw();
    this._prev_cell_gui = (BaseItemCellGUI) null;
    if (item.is_equipped_to_toolbar && !from_inventory)
    {
      int toolbarIndex = item.toolbar_index;
      if (toolbarIndex != -1)
        (BaseGUI.for_gamepad ? this._toolbar.gamepad : this._toolbar.keyboard).cells[toolbarIndex].DrawEmpty();
    }
    if (BaseGUI.for_gamepad)
      this.button_tips.PrintBack();
    this.dark_back.Activate();
    this._update_icon = !BaseGUI.for_gamepad;
    if (this._update_icon)
    {
      this._gui_cam = MainGame.me.gui_cam;
      this.cursor_item.Activate<BaseItemCellGUI>();
      this.cursor_item.DrawItem(item.id, 1, false);
      Cursor.visible = false;
    }
    else
      this.cursor_item.Deactivate<BaseItemCellGUI>();
  }

  public override void Update()
  {
    base.Update();
    for (int index = 0; index < 4; ++index)
    {
      if (LazyInput.GetKeyDown(LazyInput.toolbar_keys[index]))
      {
        GUIElements.me.inventory.OnToolbarClicked(index);
        return;
      }
    }
    if (BaseGUI.for_gamepad)
      return;
    Collider2D[] collidersUnderMouse = NGUIExtensionMethods.GetCollidersUnderMouse(MainGame.me.gui_cam);
    BaseItemCellGUI cell_gui = (BaseItemCellGUI) null;
    int index1 = -1;
    foreach (Component component in collidersUnderMouse)
    {
      cell_gui = component.transform.parent.GetComponent<BaseItemCellGUI>();
      if (!((Object) cell_gui == (Object) null))
      {
        index1 = this.GetToolbarIndexByGUI(cell_gui);
        if (index1 == -1)
          cell_gui = (BaseItemCellGUI) null;
        else
          break;
      }
    }
    if ((Object) cell_gui != (Object) this._prev_cell_gui)
    {
      if ((Object) this._prev_cell_gui != (Object) null)
        this._prev_cell_gui.SetVisualyOveredState(false, false);
      if ((Object) cell_gui != (Object) null)
        cell_gui.SetVisualyOveredState(true, false);
      this._prev_cell_gui = cell_gui;
    }
    if (!Input.GetMouseButtonUp(0))
      return;
    if (index1 == -1)
      this.OnClickedBack();
    else
      GUIElements.me.inventory.OnToolbarClicked(index1);
  }

  public int GetToolbarIndexByGUI(BaseItemCellGUI cell_gui)
  {
    for (int toolbarIndexByGui = 0; toolbarIndexByGui < 4; ++toolbarIndexByGui)
    {
      if (!((Object) cell_gui != (Object) this.keyboard_cells[toolbarIndexByGui]))
        return toolbarIndexByGui;
    }
    return -1;
  }

  public new void LateUpdate()
  {
    if (!this._update_icon)
      return;
    this._cursor_tf.position = this._gui_cam.ScreenToWorldPoint(Input.mousePosition) with
    {
      z = this._cursor_tf.position.z
    };
  }

  public void OnClickedBack() => GUIElements.me.inventory.OnEquipmentBackClicked();

  public override bool OnPressedBack()
  {
    this.Hide(true);
    return true;
  }

  public override void Hide(bool play_hide_sound = true)
  {
    foreach (BaseItemCellGUI keyboardCell in this.keyboard_cells)
      keyboardCell.SetVisualyOveredState(false, BaseGUI.for_gamepad);
    base.Hide(play_hide_sound);
    Cursor.visible = true;
    Sounds.PlaySound("gui_item_put");
  }
}
