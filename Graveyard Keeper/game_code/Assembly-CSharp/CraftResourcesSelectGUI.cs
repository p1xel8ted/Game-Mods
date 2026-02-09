// Decompiled with JetBrains decompiler
// Type: CraftResourcesSelectGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CraftResourcesSelectGUI : BaseGUI
{
  public InventoryPanelGUI _inventory_panel;
  public CraftResourcesSelectGUI.ResourceSelectResultDelegate _result_delegate;
  public Item _selected_result;

  public BaseItemCellGUI current_item_gui => this._inventory_panel.selected_item_gui;

  public Item current_item => this._inventory_panel.selected_item;

  public bool current_item_is_selectable
  {
    get
    {
      return (Object) this.current_item_gui != (Object) null && !this.current_item_gui.is_inactive_state && this.current_item != null && this.current_item.IsNotEmpty();
    }
  }

  public override void Init()
  {
    this._inventory_panel = this.GetComponentInChildren<InventoryPanelGUI>(true);
    this._inventory_panel.Init();
    this._inventory_panel.SetCallbacks(new GJCommons.VoidDelegate(this.OnItemOver), (GJCommons.VoidDelegate) null, new GJCommons.VoidDelegate(this.OnItemSelect), new GJCommons.VoidDelegate(this.OnItemOver));
    this._selected_result = (Item) null;
    base.Init();
  }

  public void Open(
    WorldGameObject obj,
    InventoryWidget.ItemFilterDelegate filter_delegate,
    CraftResourcesSelectGUI.ResourceSelectResultDelegate result_delegate,
    bool force_ignore_toolbelt = false)
  {
    if (this.is_shown)
    {
      Debug.LogError((object) "CraftResourcesSelectGUI is already opened");
    }
    else
    {
      this.Open();
      this._selected_result = (Item) null;
      this._result_delegate = result_delegate;
      MultiInventory multiInventory = obj.GetMultiInventory(sortWGOS: true, include_bags: true);
      bool flag = false;
      if (GlobalCraftControlGUI.is_global_control_active && !obj.is_player)
      {
        WorldZone zoneOfObject = WorldZone.GetZoneOfObject(obj);
        if ((Object) zoneOfObject != (Object) null && !zoneOfObject.IsPlayerInZone())
          flag = true;
      }
      if (obj.is_player && multiInventory.all.Count > 0 && !flag && !force_ignore_toolbelt)
      {
        Item data = new Item()
        {
          inventory_size = 7,
          inventory = obj.data.secondary_inventory
        };
        multiInventory.all.Insert(1, new Inventory(data, "Instruments"));
      }
      this._inventory_panel.Open(multiInventory);
      this._inventory_panel.FilterItems(filter_delegate);
      this._inventory_panel.SetInactiveStateToEmptyCells();
      if (!BaseGUI.for_gamepad)
        return;
      this._inventory_panel.InitGamepad(this.gamepad_controller);
    }
  }

  public void OnItemOver()
  {
    if (!BaseGUI.for_gamepad)
    {
      if (!this._inventory_panel.selected_item_is_empty)
        return;
      this._inventory_panel.selected_item_gui.SetVisualyOveredState(false, false);
    }
    else
      this.button_tips.Print(GameKeyTip.Select(this.current_item_is_selectable), GameKeyTip.Back());
  }

  public void OnItemSelect()
  {
    if (!this.current_item_is_selectable || LazyInput.GetKeyDown(GameKey.RightClick))
      return;
    this._selected_result = this.current_item;
    this.OnPressedBack();
  }

  public override void Hide(bool play_hide_sound = true)
  {
    this._inventory_panel.Hide();
    base.Hide(play_hide_sound);
    if (this._result_delegate == null)
      return;
    this._result_delegate(this._selected_result);
  }

  public void ClearResultDelegate()
  {
    this._result_delegate = (CraftResourcesSelectGUI.ResourceSelectResultDelegate) null;
  }

  public override bool OnPressedBack()
  {
    this.OnClosePressed();
    return true;
  }

  public void OnCloseBtnPressed() => this.OnPressedBack();

  public delegate void ResourceSelectResultDelegate(Item item);
}
