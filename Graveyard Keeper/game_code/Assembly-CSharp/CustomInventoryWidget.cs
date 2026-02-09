// Decompiled with JetBrains decompiler
// Type: CustomInventoryWidget
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CustomInventoryWidget : BaseInventoryWidget
{
  public CustomInventoryItem[] _custom_items;
  public PanelAutoScroll _panel_auto_scroll;
  public GJCommons.VoidDelegate _on_over;
  public UIGrid _grid;
  public bool hide_empty_items;

  public override void Init()
  {
    this._custom_items = this.GetComponentsInChildren<CustomInventoryItem>(true);
    foreach (CustomInventoryItem customItem in this._custom_items)
      customItem.Init(this);
    this._panel_auto_scroll = this.GetComponent<PanelAutoScroll>();
    foreach (Tooltip componentsInChild in this.GetComponentsInChildren<Tooltip>(true))
      componentsInChild.Init();
    this._grid = this.GetComponentInChildren<UIGrid>(true);
  }

  public void Open(
    Inventory inventory,
    bool for_gamepad,
    int navigation_group = 0,
    int navigation_sub_group = 0,
    GJCommons.VoidDelegate on_over = null)
  {
    this.Open(inventory, for_gamepad, BaseInventoryWidget.InventoryType.Custom);
    this._on_over = on_over;
    foreach (CustomInventoryItem customItem in this._custom_items)
    {
      customItem.gamepad_item.group = navigation_group;
      customItem.gamepad_item.sub_group = navigation_sub_group;
    }
    this.Redraw();
  }

  public override void Redraw()
  {
    foreach (CustomInventoryItem customItem in this._custom_items)
    {
      customItem.Redraw(this.inventory);
      if (this.hide_empty_items)
        customItem.gameObject.SetActive(customItem.total_count != 0);
    }
    if (!this.hide_empty_items || !((Object) this._grid != (Object) null))
      return;
    this._grid.Reposition();
    this._grid.repositionNow = true;
  }

  public void OnItemOver(string item_id)
  {
    this._panel_auto_scroll.Perform();
    this._on_over.TryInvoke();
  }

  public override GamepadNavigationItem GetFirstNavigationItem(Direction dir)
  {
    return this._custom_items[0].gamepad_item;
  }
}
