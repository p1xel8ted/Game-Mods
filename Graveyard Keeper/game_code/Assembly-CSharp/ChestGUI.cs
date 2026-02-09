// Decompiled with JetBrains decompiler
// Type: ChestGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ChestGUI : BaseGUI
{
  public InventoryPanelGUI player_panel;
  public InventoryPanelGUI chest_panel;
  public MultiInventory _player_inventory;
  public MultiInventory _chest_inventory;
  public InventoryPanelGUI _last_selected_panel;
  public WorldGameObject _chest_obj;

  public override void Init()
  {
    this.player_panel.Init();
    this.player_panel.SetCallbacks(new GJCommons.VoidDelegate(this.OnItemOver), (GJCommons.VoidDelegate) null, new GJCommons.VoidDelegate(this.OnItemSelect), new GJCommons.VoidDelegate(this.OnCustomItemOver));
    this.chest_panel.Init();
    this.chest_panel.SetCallbacks(new GJCommons.VoidDelegate(this.OnItemOver), (GJCommons.VoidDelegate) null, new GJCommons.VoidDelegate(this.OnItemSelect), new GJCommons.VoidDelegate(this.OnCustomItemOver));
    base.Init();
  }

  public void Open(WorldGameObject chest_obj)
  {
    if ((UnityEngine.Object) chest_obj == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Cannot open chest gui for null obj");
    }
    else
    {
      this.Open();
      this._chest_obj = chest_obj;
      this._player_inventory = MainGame.me.player.GetMultiInventory(new List<WorldGameObject>()
      {
        this._chest_obj
      }, sortWGOS: true, include_bags: true);
      this._chest_inventory = this._chest_obj.GetMultiInventoryOfWGOWithoutWorldZone(true);
      this.player_panel.Open(this._player_inventory, 1);
      this.chest_panel.Open(this._chest_inventory, 2);
      this.player_panel.SetGrayToNotMainWidgets(true);
      if (BaseGUI.for_gamepad)
      {
        this.gamepad_controller.ReinitItems(false);
        if ((UnityEngine.Object) this._last_selected_panel == (UnityEngine.Object) null || this._last_selected_panel.selected_item == null)
        {
          this.gamepad_controller.FocusOnFirstActive();
        }
        else
        {
          BaseItemCellGUI itemCellGuiForItem = this._last_selected_panel.GetItemCellGuiForItem(this._last_selected_panel.selected_item);
          this.gamepad_controller.SetFocusedItem((UnityEngine.Object) itemCellGuiForItem == (UnityEngine.Object) null ? (GamepadNavigationItem) null : itemCellGuiForItem.gamepad_item);
        }
      }
      MainGame.SetPausedMode(true);
    }
  }

  public void FullRedrawPanels(int possible_index_for_gamepad = -1)
  {
    this._player_inventory = MainGame.me.player.GetMultiInventory(new List<WorldGameObject>()
    {
      this._chest_obj
    }, sortWGOS: true, include_bags: true);
    this._chest_inventory = this._chest_obj.GetMultiInventoryOfWGOWithoutWorldZone(true);
    this.player_panel.FullRedraw(this._player_inventory, 1);
    this.chest_panel.FullRedraw(this._chest_inventory, 2);
    if (!BaseGUI.for_gamepad)
      return;
    this.gamepad_controller.ReinitItems(false);
    if (possible_index_for_gamepad != -1)
      this.gamepad_controller.SetFocusedItem(possible_index_for_gamepad);
    else if ((UnityEngine.Object) this._last_selected_panel == (UnityEngine.Object) null || this._last_selected_panel.selected_item == null)
    {
      this.gamepad_controller.FocusOnFirstActive();
    }
    else
    {
      BaseItemCellGUI itemCellGuiForItem = this._last_selected_panel.GetItemCellGuiForItem(this._last_selected_panel.selected_item);
      this.gamepad_controller.SetFocusedItem((UnityEngine.Object) itemCellGuiForItem == (UnityEngine.Object) null ? (GamepadNavigationItem) null : itemCellGuiForItem.gamepad_item);
    }
  }

  public void OnCustomItemOver()
  {
    if (!BaseGUI.for_gamepad)
      return;
    this.button_tips.PrintClose();
    this._last_selected_panel = (InventoryPanelGUI) null;
  }

  public void OnItemOver()
  {
    if (!BaseGUI.for_gamepad)
      return;
    InventoryPanelGUI last = InventoryPanelGUI.last;
    this._last_selected_panel = last;
    Item inventoryData = last.selected_widget.inventory_data;
    if ((UnityEngine.Object) last == (UnityEngine.Object) null || last.selected_widget_is_not_main && (inventoryData == null || !inventoryData.is_bag))
    {
      this.button_tips.PrintClose();
    }
    else
    {
      Item from_bag = (Item) null;
      if (inventoryData != null && inventoryData.is_bag)
        from_bag = inventoryData;
      bool flag = (UnityEngine.Object) last == (UnityEngine.Object) this.player_panel;
      bool selectedItemIsEmpty = last.selected_item_is_empty;
      if ((selectedItemIsEmpty ? 0 : this.GetMaxMoveCount(last.selected_item, (UnityEngine.Object) last == (UnityEngine.Object) this.player_panel, from_bag)) <= 1)
        this.button_tips.Print(GameKeyTip.Select(flag ? "put" : "take", !selectedItemIsEmpty), GameKeyTip.Close());
      else
        this.button_tips.Print(GameKeyTip.Select(flag ? "put" : "take", !selectedItemIsEmpty), GameKeyTip.Option1(GJL.L(flag ? "put" : "take") + " 1", !selectedItemIsEmpty), GameKeyTip.Close());
    }
  }

  public void OnItemSelect()
  {
    InventoryPanelGUI last = InventoryPanelGUI.last;
    Item inventoryData = last.selected_widget.inventory_data;
    if (last.selected_widget_is_not_main && (inventoryData == null || !inventoryData.is_bag) || last.selected_item_is_empty)
      return;
    BaseItemCellGUI item_gui = last.selected_item_gui;
    Item item = last.selected_item;
    bool to_chest = (UnityEngine.Object) last == (UnityEngine.Object) this.player_panel;
    Item from_bag = (Item) null;
    if (inventoryData != null && inventoryData.is_bag)
      from_bag = inventoryData;
    int maxMoveCount = this.GetMaxMoveCount(last.selected_item, (UnityEngine.Object) last == (UnityEngine.Object) this.player_panel, from_bag);
    if (item.definition.stack_count > 1 && LazyInput.GetKeyDown(GameKey.MoveAllStack))
      this.MoveItem(item, maxMoveCount, to_chest, from_bag);
    else if (item.definition.stack_count == 1 || LazyInput.GetKeyDown(GameKey.RightClick) || maxMoveCount == 1)
    {
      this.MoveItem(item, 1, to_chest, from_bag);
    }
    else
    {
      this.button_tips.Deactivate<ButtonTipsStr>();
      GUIElements.me.item_count.Open(item.id, 1, maxMoveCount, (Action<int>) (chosen_count => this.MoveItem(item, chosen_count, to_chest, from_bag, true)));
      GUIElements.me.item_count.SetOnHide((GJCommons.VoidDelegate) (() =>
      {
        if (!BaseGUI.for_gamepad)
          return;
        this.button_tips.Activate<ButtonTipsStr>();
        this.gamepad_controller.Enable(GamepadNavigationController.OpenMethod.GetAll);
        this.gamepad_controller.SetFocusedItem(item_gui.gamepad_item);
      }));
    }
  }

  public int GetMaxMoveCount(Item item, bool to_chest, Item from_bag = null, bool count_in_bags = false)
  {
    MultiInventory multiInventory1 = to_chest ? this._player_inventory : this._chest_inventory;
    MultiInventory multiInventory2 = to_chest ? this._chest_inventory : this._player_inventory;
    if (from_bag != null)
      multiInventory1 = new MultiInventory(new Inventory(from_bag));
    return Mathf.Min(multiInventory1.GetTotalCount(item.id, MultiInventory.DestinationType.OnlyFirst, count_in_bags), multiInventory2.CanAddCount(item.id, true));
  }

  public void MoveItem(int count)
  {
    InventoryPanelGUI last = InventoryPanelGUI.last;
    Item inventoryData = last.selected_widget.inventory_data;
    if ((UnityEngine.Object) last == (UnityEngine.Object) null || last.selected_widget_is_not_main && (inventoryData == null || !inventoryData.is_bag) || last.selected_item_is_empty)
      return;
    Item from_bag = (Item) null;
    bool to_chest = (UnityEngine.Object) last == (UnityEngine.Object) this.player_panel;
    if (inventoryData != null && inventoryData.is_bag)
      from_bag = inventoryData;
    this.MoveItem(last.selected_item, count, to_chest, from_bag);
  }

  public void MoveItem(
    Item item,
    int count,
    bool to_chest,
    Item from_bag = null,
    bool after_item_count_gui = false)
  {
    if (item == null || item.IsEmpty())
      return;
    Sounds.PlaySound("item_put");
    MultiInventory multiInventory1 = to_chest ? this._player_inventory : this._chest_inventory;
    MultiInventory another_inventory1 = to_chest ? this._chest_inventory : this._player_inventory;
    InventoryPanelGUI inventoryPanelGui = to_chest ? this.player_panel : this.chest_panel;
    int num1 = inventoryPanelGui.selected_item.value;
    if (from_bag != null)
    {
      int itemsCount = from_bag.GetItemsCount(item.id);
      if (itemsCount > 0)
      {
        MultiInventory multiInventory2 = new MultiInventory(new Inventory(from_bag));
        int num2 = itemsCount > count ? count : itemsCount;
        MultiInventory another_inventory2 = another_inventory1;
        Item obj = item;
        int count1 = num2;
        if (multiInventory2.MoveItemTo(another_inventory2, obj, count1, true))
          count -= num2;
      }
    }
    int possible_index_for_gamepad = -1;
    if (item.is_bag && BaseGUI.for_gamepad)
    {
      BaseItemCellGUI itemCellGuiForItem = this._last_selected_panel.GetItemCellGuiForItem(this._last_selected_panel.selected_item);
      if ((UnityEngine.Object) itemCellGuiForItem != (UnityEngine.Object) null)
        possible_index_for_gamepad = this.gamepad_controller.GetFocusedItemIndex(itemCellGuiForItem.gamepad_item);
      if (possible_index_for_gamepad != -1 && !to_chest)
        possible_index_for_gamepad += item.inventory_size;
    }
    if (count > 0 && !multiInventory1.MoveItemTo(another_inventory1, item, count, true))
      return;
    if (item.is_bag)
    {
      this.FullRedrawPanels(possible_index_for_gamepad);
    }
    else
    {
      this.player_panel.Redraw();
      this.chest_panel.Redraw();
      if (count >= num1)
        TooltipsManager.Redraw();
      if (!to_chest)
        MainGame.me.player.TryEquipPickupedDrop(item, false);
      inventoryPanelGui.UpdateSelection(after_item_count_gui);
    }
  }

  public override void Hide(bool play_hide_sound = true)
  {
    this.player_panel.Hide();
    this.chest_panel.Hide();
    base.Hide(play_hide_sound);
    MainGame.SetPausedMode(false);
  }

  public override void Update()
  {
    if (this.is_shown_and_top && Input.GetKeyDown(KeyCode.Tab))
      this.OnPressedBack();
    base.Update();
  }

  public override bool OnPressedOption1()
  {
    this.MoveItem(1);
    return true;
  }

  public override bool OnPressedBack()
  {
    this.OnClosePressed();
    return true;
  }
}
