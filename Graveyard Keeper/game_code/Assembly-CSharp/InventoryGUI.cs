// Decompiled with JetBrains decompiler
// Type: InventoryGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class InventoryGUI : BaseGameGUI
{
  public UILabel character_info_label;
  public Transform head_pos_tf;
  public InventoryPanelGUI _inventory_panel;
  public MultiInventory _inventory;
  public ToolbarGUI _toolbar;
  public InventoryGUI.EquipmentState _equipment_state;
  public Item _equipping_item;
  public BaseItemCellGUI _context_menu_target_item;
  public PerkBuffItemGUI perk_buff_item_prefab;
  public GameObject perk_buff_separator_prefab;
  public GameObject go_no_buffs;
  public GameObject go_no_perks;
  public GameObject go_hdr_buffs;
  public GameObject go_hdr_perks;
  public UITable table_perks_buffs;
  public InventoryWidget toolbelt_widget;
  public InventoryWidget bag_inventory_widget;
  public const int SIBLING_INDEX_BUFFS = 0;
  public const int SIBLING_INDEX_PERKS = 500;
  public InventoryGUI.InventoryGUIState _inventory_state;
  public Item _current_open_bag;
  public InventoryPanelGUI _last_selected_panel;
  public InventoryPanelGUI bag_panel;
  public MultiInventory _bag_inventory;
  public List<InventoryGUI.ToolbeltItemDescriptionGUI> toolbelt_items = new List<InventoryGUI.ToolbeltItemDescriptionGUI>();

  public Item selected_item
  {
    get
    {
      if (this._inventory_state == InventoryGUI.InventoryGUIState.BagIsOpen)
      {
        InventoryPanelGUI last = InventoryPanelGUI.last;
        if ((UnityEngine.Object) last != (UnityEngine.Object) null)
          return last.selected_item;
      }
      return this._inventory_panel.selected_item;
    }
  }

  public Vector3 head_pos
  {
    get
    {
      return MainGame.me.world_cam.ScreenToWorldPoint(MainGame.me.gui_cam.WorldToScreenPoint(this.head_pos_tf.position));
    }
  }

  public override void Init()
  {
    this._inventory_panel = this.GetComponentInChildren<InventoryPanelGUI>();
    this._inventory_panel.Init();
    this._inventory_panel.SetCallbacks(new GJCommons.VoidDelegate(this.OnItemOver), new GJCommons.VoidDelegate(this.OnItemOut), new GJCommons.VoidDelegate(this.OnItemPressed), new GJCommons.VoidDelegate(this.OnItemOver));
    this.bag_panel.Init();
    this.bag_panel.SetCallbacks(new GJCommons.VoidDelegate(this.OnItemOverInBag), new GJCommons.VoidDelegate(this.OnItemOutInBag), new GJCommons.VoidDelegate(this.OnItemPressedInBag));
    this.bag_panel.Hide();
    this._toolbar = this.GetComponentInChildren<ToolbarGUI>(true);
    this._toolbar.Init();
    this.toolbelt_widget.Init();
    this.toolbelt_widget.SetCallbacks(new InventoryWidget.ItemDelegate(this.OnToolbeltItemOver), new InventoryWidget.ItemDelegate(this.OnToolbeltItemOut), new InventoryWidget.ItemDelegate(this.OnItemGUIPressed));
    this.perk_buff_item_prefab.SetActive(false);
    this.perk_buff_separator_prefab.SetActive(false);
    this.go_no_buffs.SetActive(false);
    this.go_no_perks.SetActive(false);
    this.go_hdr_buffs.transform.SetSiblingIndex(0);
    this.go_no_buffs.transform.SetSiblingIndex(1);
    this.go_hdr_perks.transform.SetSiblingIndex(500);
    this.go_no_perks.transform.SetSiblingIndex(501);
    int num = 0;
    foreach (BaseItemCellGUI cell in this._toolbar.keyboard.cells)
    {
      int index = num++;
      cell.interaction_enabled = true;
      cell.SetCallbacks((GJCommons.VoidDelegate) null, (GJCommons.VoidDelegate) null, (GJCommons.VoidDelegate) (() => this.OnToolbarClicked(index)));
    }
    base.Init();
  }

  public override void OpenFromGameGUI() => this.Open();

  public override void Open()
  {
    base.Open();
    this._inventory_state = InventoryGUI.InventoryGUIState.Standart;
    this._current_open_bag = (Item) null;
    this._inventory = MainGame.me.player.GetMultiInventory(sortWGOS: true, include_bags: true);
    this._inventory_panel.Open(this._inventory, is_debt_show: true);
    this.IniGamepadAndTooltips();
    this.RedrawPlayerInfoAndToolbelt();
    this._toolbar.SetActive(!BaseGUI.for_gamepad);
    this.RedrawBuffsAndPerks();
    if (BaseGUI.for_gamepad)
      return;
    this._toolbar.Redraw();
    int num = 0;
    foreach (BaseItemCellGUI keyboardCell in GUIElements.me.equip_to_toolbar.keyboard_cells)
    {
      int index = num++;
      keyboardCell.interaction_enabled = true;
      keyboardCell.SetCallbacks((GJCommons.VoidDelegate) null, (GJCommons.VoidDelegate) null, (GJCommons.VoidDelegate) (() => this.OnToolbarClicked(index)));
    }
  }

  public void RedrawBuffsAndPerks()
  {
    foreach (PerkBuffItemGUI componentsInChild in this.GetComponentsInChildren<PerkBuffItemGUI>())
    {
      componentsInChild.transform.SetParent((Transform) null, true);
      UnityEngine.Object.Destroy((UnityEngine.Object) componentsInChild.gameObject);
    }
    foreach (SeparatorGUI componentsInChild in this.GetComponentsInChildren<SeparatorGUI>())
    {
      componentsInChild.transform.SetParent((Transform) null, true);
      UnityEngine.Object.Destroy((UnityEngine.Object) componentsInChild.gameObject);
    }
    int num1 = 0;
    int num2 = 1;
    foreach (PlayerBuff buff in MainGame.me.save.buffs)
    {
      if (!buff.definition.is_hidden)
      {
        if (num1 > 0)
          this.perk_buff_separator_prefab.Copy().transform.SetSiblingIndex(num2++);
        PerkBuffItemGUI perkBuffItemGui = this.perk_buff_item_prefab.Copy<PerkBuffItemGUI>();
        perkBuffItemGui.transform.SetSiblingIndex(num2++);
        perkBuffItemGui.Draw(buff);
        ++num1;
      }
    }
    this.go_no_buffs.SetActive(num1 == 0);
    int num3 = 0;
    int num4 = 501;
    foreach (string unlockedPerk in MainGame.me.save.unlocked_perks)
    {
      PerkDefinition data = GameBalance.me.GetData<PerkDefinition>(unlockedPerk);
      if (data.show)
      {
        if (num3 > 0)
          this.perk_buff_separator_prefab.Copy().transform.SetSiblingIndex(num4++);
        PerkBuffItemGUI perkBuffItemGui = this.perk_buff_item_prefab.Copy<PerkBuffItemGUI>();
        perkBuffItemGui.transform.SetSiblingIndex(num4++);
        perkBuffItemGui.Draw(data);
        ++num3;
      }
    }
    this.go_no_perks.SetActive(num3 == 0);
    this.table_perks_buffs.Reposition();
    this.table_perks_buffs.repositionNow = true;
  }

  public override void OnAboveWindowClosed()
  {
    this.Redraw();
    this.InitPlatformDependentStuff();
    this.IniGamepadAndTooltips();
    this._equipping_item = (Item) null;
    this._equipment_state = InventoryGUI.EquipmentState.None;
    if (BaseGUI.for_gamepad)
      return;
    this._toolbar.Redraw();
  }

  public void IniGamepadAndTooltips()
  {
    if (BaseGUI.for_gamepad)
      this._inventory_panel.InitGamepad(this.gamepad_controller);
    TooltipBubbleGUI.ChangeAvaibility(true);
  }

  public override void CloseFromGameGUI()
  {
    if (this._inventory_state == InventoryGUI.InventoryGUIState.BagIsOpen)
      this.CloseBag();
    this._inventory_panel.Hide();
    base.CloseFromGameGUI();
    TooltipBubbleGUI.ChangeAvaibility(false);
  }

  public void OnItemOver()
  {
    if (!BaseGUI.for_gamepad)
      return;
    this.UpdateButtonTips();
  }

  public void OnItemOut()
  {
    if (BaseGUI.for_gamepad || (UnityEngine.Object) this._context_menu_target_item == (UnityEngine.Object) null)
      return;
    this._context_menu_target_item.SetVisualyOveredState(true, false);
  }

  public void OnItemPressed()
  {
    if (this._inventory_panel.selected_widget_is_not_main)
    {
      if (this._inventory_state == InventoryGUI.InventoryGUIState.BagIsOpen && ((UnityEngine.Object) InventoryPanelGUI.last != (UnityEngine.Object) this._inventory_panel || this._inventory_panel.selected_widget.inventory_data == this._current_open_bag))
        return;
      if (this._inventory_state == InventoryGUI.InventoryGUIState.Standart)
      {
        Item inventoryData = this._inventory_panel.selected_widget.inventory_data;
        if (inventoryData == null || !inventoryData.is_bag)
          return;
      }
    }
    this.OnItemPressedItem(this.selected_item);
  }

  public void OnToolbeltItemOver(BaseItemCellGUI item)
  {
    if (!BaseGUI.for_gamepad)
      return;
    this.UpdateButtonTips();
  }

  public void OnToolbeltItemOut(BaseItemCellGUI item)
  {
  }

  public void OnItemGUIPressed(BaseItemCellGUI item)
  {
    if (this._inventory_panel.selected_widget_is_not_main)
    {
      if (this._inventory_state == InventoryGUI.InventoryGUIState.BagIsOpen && ((UnityEngine.Object) InventoryPanelGUI.last != (UnityEngine.Object) this._inventory_panel || this._inventory_panel.selected_widget.inventory_data == this._current_open_bag))
        return;
      if (this._inventory_state == InventoryGUI.InventoryGUIState.Standart && (UnityEngine.Object) item.GetComponent<ToolbeltItemGUI>() == (UnityEngine.Object) null)
      {
        Item inventoryData = this._inventory_panel.selected_widget?.inventory_data;
        if (inventoryData == null || !inventoryData.is_bag)
          return;
      }
    }
    this.OnItemPressedItem(item.item);
  }

  public bool ItemIsEquipped(Item item)
  {
    return item.is_equipped || MainGame.me.player.data.secondary_inventory.Contains(item);
  }

  public void OnItemPressedItem(Item item)
  {
    if (item == null || item.IsEmpty())
      return;
    if (this._inventory_state == InventoryGUI.InventoryGUIState.BagIsOpen)
    {
      if (BaseGUI.for_gamepad)
      {
        if (item.is_bag)
        {
          if (this._current_open_bag != item)
          {
            this.CloseBag();
            GJTimer.AddTimer(0.01f, (GJTimer.VoidDelegate) (() => this.OpenBag(item)));
          }
          else
            this.CloseBag();
        }
        else
        {
          int num = (UnityEngine.Object) InventoryPanelGUI.last == (UnityEngine.Object) this._inventory_panel ? 1 : 0;
          Item from_not_open_bag = (Item) null;
          if (num != 0 && this._inventory_panel.selected_widget_is_not_main)
            from_not_open_bag = this._inventory_panel.selected_widget.inventory_data;
          if (LazyInput.GetKeyDown(GameKey.Action))
          {
            this.MoveItemToBag(item, 1, from_not_open_bag: from_not_open_bag);
          }
          else
          {
            Debug.Log((object) $"#BAG# Gamepad: Moving [{item.id}] items from inventory to bag.");
            int maxMoveCount = this.GetMaxMoveCount(item, true, from_not_open_bag: from_not_open_bag);
            if (item.definition.stack_count == 1 || maxMoveCount == 1)
              this.MoveItemToBag(item, 1, from_not_open_bag: from_not_open_bag);
            else if (item.definition.stack_count > 1 && LazyInput.GetKeyDown(GameKey.MoveAllStack))
            {
              this.MoveItemToBag(item, maxMoveCount, from_not_open_bag: from_not_open_bag);
            }
            else
            {
              this.button_tips.Deactivate<ButtonTipsStr>();
              GUIElements.me.item_count.Open(item.id, 1, maxMoveCount, (Action<int>) (chosen_count => this.MoveItemToBag(item, chosen_count, from_not_open_bag: from_not_open_bag)));
              GUIElements.me.item_count.SetOnHide((GJCommons.VoidDelegate) (() =>
              {
                if (!BaseGUI.for_gamepad)
                  return;
                this.button_tips.Activate<ButtonTipsStr>();
                this.gamepad_controller.Enable(GamepadNavigationController.OpenMethod.GetAll);
                this.gamepad_controller.SetFocusedItem(this._inventory_panel.selected_item_gui.gamepad_item);
              }));
            }
          }
        }
      }
      else if (item.is_bag)
      {
        Debug.Log((object) $"#BAG# Mouse: Open/close Bag \"{item.id}\"");
        if (this._current_open_bag != item)
        {
          this.CloseBag();
          GJTimer.AddTimer(0.01f, (GJTimer.VoidDelegate) (() => this.OpenBag(item)));
        }
        else
          this.CloseBag();
      }
      else
      {
        int num = (UnityEngine.Object) InventoryPanelGUI.last == (UnityEngine.Object) this._inventory_panel ? 1 : 0;
        Item from_not_open_bag = (Item) null;
        if (num != 0 && this._inventory_panel.selected_widget_is_not_main)
          from_not_open_bag = this._inventory_panel.selected_widget.inventory_data;
        if (LazyInput.GetKeyDown(GameKey.RightClick))
        {
          Debug.Log((object) $"#BAG# Mouse: Moving 1 [{item.id}] item from Inventory to bag.");
          this.MoveItemToBag(item, 1, from_not_open_bag: from_not_open_bag);
        }
        else
        {
          Debug.Log((object) $"#BAG# Mouse: Moving [{item.id}] items from inventory to bag.");
          int maxMoveCount = this.GetMaxMoveCount(item, true, from_not_open_bag: from_not_open_bag);
          if (item.definition.stack_count == 1 || maxMoveCount == 1)
            this.MoveItemToBag(item, 1, from_not_open_bag: from_not_open_bag);
          else if (item.definition.stack_count > 1 && LazyInput.GetKeyDown(GameKey.MoveAllStack))
          {
            this.MoveItemToBag(item, maxMoveCount, from_not_open_bag: from_not_open_bag);
          }
          else
          {
            this.button_tips.Deactivate<ButtonTipsStr>();
            GUIElements.me.item_count.Open(item.id, 1, maxMoveCount, (Action<int>) (chosen_count => this.MoveItemToBag(item, chosen_count, from_not_open_bag: from_not_open_bag)));
          }
        }
      }
    }
    else if (BaseGUI.for_gamepad || !LazyInput.GetKeyDown(GameKey.RightClick))
    {
      if (item.is_bag)
      {
        Debug.Log((object) $"#BAG# Opening Bag \"{item.id}\"");
        this.OpenBag(item);
      }
      else
        this.OnItemEquip(item, false);
    }
    else
    {
      int num = MainGame.me.player.data.secondary_inventory.Contains(item) ? 1 : 0;
      BubbleWidgetDataOptions options_data = new BubbleWidgetDataOptions();
      bool enabled = item.definition.can_be_used;
      if (enabled && item.GetGrayedCooldownPercent() > 0)
        enabled = false;
      if (num == 0)
      {
        if (item.is_bag)
        {
          if (this._inventory_state == InventoryGUI.InventoryGUIState.Standart)
            options_data.AddOption("open", (System.Action) (() => this.OpenBag(item)));
          else if (this._current_open_bag == item)
            options_data.AddOption("close", new System.Action(this.CloseBag));
          else
            options_data.AddOption("open", (System.Action) (() =>
            {
              this.CloseBag();
              GJTimer.AddTimer(0.01f, (GJTimer.VoidDelegate) (() => this.OpenBag(item)));
            }));
        }
        else
          options_data.AddOption("use", new System.Action(this.UseItem), enabled);
      }
      if (this.CanBeEquipped(item))
        options_data.AddOption(this.ItemIsEquipped(item) ? "unequip" : "equip", (System.Action) (() => this.OnItemEquip(item, true)));
      else
        options_data.AddOption("equip", (System.Action) null, false);
      if (!item.definition.player_cant_throw_out)
        options_data.AddOption("destroy", new System.Action(this.OnDestroyItem));
      this._context_menu_target_item = this._inventory_panel.selected_item_gui;
      ContextMenuBubbleGUI.Show(options_data, (Vector2) Input.mousePosition, (GJCommons.VoidDelegate) (() =>
      {
        if ((UnityEngine.Object) this._context_menu_target_item != (UnityEngine.Object) null)
          this._context_menu_target_item.SetVisualyOveredState(false, false);
        this._context_menu_target_item = (BaseItemCellGUI) null;
      }));
    }
  }

  public void OnToolbarClicked(int index)
  {
    switch (this._equipment_state)
    {
      case InventoryGUI.EquipmentState.None:
        string equippedItem = MainGame.me.save.equipped_items[index];
        if (string.IsNullOrEmpty(equippedItem))
          break;
        this.StartEquipment(new Item(equippedItem, 1), false);
        break;
      case InventoryGUI.EquipmentState.StartedFromInventory:
      case InventoryGUI.EquipmentState.StartedFromToolbar:
        if (this._equipping_item == null)
          break;
        this.EquipCurrentItem(index);
        GUIElements.me.equip_to_toolbar.Hide(true);
        break;
    }
  }

  public void OnEquipmentBackClicked()
  {
    if (this._equipment_state == InventoryGUI.EquipmentState.None)
      return;
    if (this._equipment_state == InventoryGUI.EquipmentState.StartedFromToolbar)
      MainGame.me.save.UnEquip(this._equipping_item.id);
    GUIElements.me.equip_to_toolbar.Hide(true);
  }

  public void OnItemEquip(Item item, bool from_context_menu)
  {
    if (!MainGame.me.player.data.secondary_inventory.Contains(item))
    {
      Item inventoryData = this._inventory_panel.selected_widget.inventory_data;
      if (this._inventory_panel.selected_widget_is_not_main && (inventoryData == null || !inventoryData.is_bag))
        return;
    }
    if (item == null || item.IsEmpty() || item.durability_state == Item.DurabilityState.Broken)
      return;
    if (item.definition.can_be_used && !item.definition.cooldown.has_expression)
    {
      this.StartEquipment(item, true);
    }
    else
    {
      if (item.definition.equipment_type == ItemDefinition.EquipmentType.None)
        return;
      int num = this.ItemIsEquipped(item) ? 1 : 0;
      GamepadNavigationItem focusedItem = BaseGUI.for_gamepad ? this.gamepad_controller?.focused_item : (GamepadNavigationItem) null;
      Sounds.OnToolEquip(num == 0);
      if (num != 0)
      {
        MainGame.me.player.UnEquipItem(item);
      }
      else
      {
        Item inventoryData = this._inventory_panel.selected_widget.inventory_data;
        Item try_from_bag = (Item) null;
        if (inventoryData != null && inventoryData.is_bag)
          try_from_bag = inventoryData;
        MainGame.me.player.EquipItem(item, try_from_bag: try_from_bag);
      }
      this.Redraw();
      if (BaseGUI.for_gamepad)
        this.UpdateButtonTips();
      if (this.IsToolbeltItemFocused())
      {
        if (!((UnityEngine.Object) focusedItem != (UnityEngine.Object) null))
          return;
        this.gamepad_controller.SetFocusedItem(focusedItem);
      }
      else
        this._inventory_panel?.selected_item_gui?.OnOver(BaseGUI.for_gamepad);
    }
  }

  public void StartEquipment(Item item, bool from_inventory)
  {
    this._equipping_item = item;
    this._equipment_state = from_inventory ? InventoryGUI.EquipmentState.StartedFromInventory : InventoryGUI.EquipmentState.StartedFromToolbar;
    Sounds.PlaySound("gui_item_pickup");
    GUIElements.me.equip_to_toolbar.Open(item, from_inventory);
  }

  public void EquipCurrentItem(int toolbar_index = -1, Item try_from_bag = null)
  {
    if (this._equipping_item == null)
      return;
    MainGame.me.player.EquipItem(this._equipping_item, toolbar_index, try_from_bag);
    this.Redraw();
    this._equipping_item = (Item) null;
    this._equipment_state = InventoryGUI.EquipmentState.None;
  }

  public override bool OnPressedSelect()
  {
    if (this._inventory_state == InventoryGUI.InventoryGUIState.BagIsOpen)
    {
      bool to_bag = (UnityEngine.Object) InventoryPanelGUI.last == (UnityEngine.Object) this._inventory_panel;
      Item from_not_open_bag = (Item) null;
      if (to_bag && this._inventory_panel.selected_widget_is_not_main)
      {
        from_not_open_bag = this._inventory_panel.selected_widget.inventory_data;
        if (!from_not_open_bag.is_bag || from_not_open_bag == this._current_open_bag)
          return false;
      }
      Item item = this.selected_item;
      if (item == null || item.IsEmpty())
        return false;
      if (item.is_bag)
      {
        if (this._inventory_state == InventoryGUI.InventoryGUIState.BagIsOpen)
        {
          if (this._current_open_bag == item)
          {
            this.CloseBag();
          }
          else
          {
            this.CloseBag();
            GJTimer.AddTimer(0.01f, (GJTimer.VoidDelegate) (() => this.OpenBag(item)));
          }
        }
        else
          this.OpenBag(item);
        return true;
      }
      if (!item.CanBeInsertedInBag(this._current_open_bag))
        return false;
      int maxMoveCount = this.GetMaxMoveCount(item, to_bag, from_not_open_bag: from_not_open_bag);
      if (item.definition.stack_count == 1 || maxMoveCount == 1)
      {
        this.MoveItemToBag(item, 1, !to_bag, from_not_open_bag);
        return true;
      }
      if (item.definition.stack_count > 1 && LazyInput.GetKeyDown(GameKey.MoveAllStack))
      {
        this.MoveItemToBag(item, maxMoveCount, !to_bag, from_not_open_bag);
        return true;
      }
      this.button_tips.Deactivate<ButtonTipsStr>();
      GUIElements.me.item_count.Open(item.id, 1, maxMoveCount, (Action<int>) (chosen_count => this.MoveItemToBag(item, chosen_count, !to_bag, from_not_open_bag)));
      GUIElements.me.item_count.SetOnHide((GJCommons.VoidDelegate) (() =>
      {
        if (!BaseGUI.for_gamepad)
          return;
        this.button_tips.Activate<ButtonTipsStr>();
        this.gamepad_controller.Enable(GamepadNavigationController.OpenMethod.GetAll);
        if (to_bag)
        {
          if (this._inventory_panel.selected_item_is_empty)
            this.gamepad_controller.ReinitItems(false);
          else
            this.gamepad_controller.SetFocusedItem(this._inventory_panel.selected_item_gui.gamepad_item);
        }
        else if (this.bag_panel.selected_item_is_empty)
          this.gamepad_controller.ReinitItems(false);
        else
          this.gamepad_controller.SetFocusedItem(this.bag_panel.selected_item_gui.gamepad_item);
      }));
      return true;
    }
    if (BaseGUI.for_gamepad && (UnityEngine.Object) InventoryPanelGUI.last != (UnityEngine.Object) this._inventory_panel || this._inventory_panel.selected_widget_is_not_main && !this._inventory_panel.selected_widget.inventory_data.is_bag)
      return false;
    this.UseItem();
    return true;
  }

  public override bool OnPressedBack()
  {
    if (this._inventory_state == InventoryGUI.InventoryGUIState.BagIsOpen)
      this.CloseBag();
    else
      GUIElements.me.game_gui.Hide(true);
    return true;
  }

  public bool IsToolbeltItemFocused() => this.IsToolbeltItemFocused(out Item _);

  public bool IsToolbeltItemFocused(out Item toolbelt_focused_item)
  {
    bool flag = (UnityEngine.Object) this.gamepad_controller?.focused_item?.GetComponent<ToolbeltItemGUI>() != (UnityEngine.Object) null;
    toolbelt_focused_item = flag ? this.gamepad_controller.focused_item.GetComponent<BaseItemCellGUI>()?.item : (Item) null;
    return flag;
  }

  public override bool OnPressedOption1()
  {
    if (this._inventory_state == InventoryGUI.InventoryGUIState.Standart)
    {
      Item toolbelt_focused_item;
      if (this.IsToolbeltItemFocused(out toolbelt_focused_item))
      {
        GamepadNavigationItem focusedItem = this.gamepad_controller.focused_item;
        this.OnItemPressedItem(toolbelt_focused_item);
        this.gamepad_controller.SetFocusedItem(focusedItem);
        focusedItem.UnFocus();
        focusedItem.Focus();
      }
      else
        this.OnItemPressed();
    }
    else
    {
      bool flag = (UnityEngine.Object) InventoryPanelGUI.last == (UnityEngine.Object) this._inventory_panel;
      Item from_not_open_bag = (Item) null;
      if (flag && this._inventory_panel.selected_widget_is_not_main)
      {
        from_not_open_bag = this._inventory_panel.selected_widget.inventory_data;
        if (!from_not_open_bag.is_bag || from_not_open_bag == this._current_open_bag)
          return false;
      }
      Item selectedItem = this.selected_item;
      if (!selectedItem.CanBeInsertedInBag(this._current_open_bag))
        return false;
      GamepadNavigationItem focusedItem = this.gamepad_controller.focused_item;
      if ((UnityEngine.Object) focusedItem != (UnityEngine.Object) null && selectedItem != null && !selectedItem.IsEmpty() && !selectedItem.is_bag)
      {
        if (this.IsToolbeltItemFocused(out Item _))
          Debug.LogError((object) "TODO: Toolbelt Item Pressed Option 1");
        else if (flag)
        {
          Debug.Log((object) $"#BAG# Gamepad: moving 1 [{selectedItem.id}] item from Inventory to Bag", (UnityEngine.Object) focusedItem);
          this.MoveItemToBag(selectedItem, 1, from_not_open_bag: from_not_open_bag);
        }
        else
        {
          Debug.Log((object) $"#BAG# Gamepad: moving 1 [{selectedItem.id}] item from Bag to Inventory", (UnityEngine.Object) focusedItem);
          this.MoveItemToBag(selectedItem, 1, true);
        }
      }
    }
    return true;
  }

  public override bool OnPressedOption2()
  {
    if (this._inventory_state == InventoryGUI.InventoryGUIState.BagIsOpen)
      return false;
    this.OnDestroyItem();
    return true;
  }

  public void OnDestroyItem()
  {
    if (this._inventory_panel.selected_widget_is_not_main)
    {
      Item inventoryData = this._inventory_panel.selected_widget.inventory_data;
      if (inventoryData == null || !inventoryData.is_bag)
        return;
    }
    if (this.selected_item == null || this.selected_item.IsEmpty() || this.selected_item.definition.player_cant_throw_out)
      return;
    if (BaseGUI.for_gamepad)
    {
      this.button_tips.Deactivate<ButtonTipsStr>();
      TooltipBubbleGUI.ChangeAvaibility(false);
    }
    string v1 = this.selected_item.definition.GetItemName();
    if (this.selected_item.value > 1)
      v1 = $"{v1} ({this.selected_item.value.ToString()})";
    GUIElements.me.dialog.OpenYesNo(GJL.L("destroy_question", v1), new GJCommons.VoidDelegate(this.DestroyItem));
  }

  public void DestroyItem()
  {
    if ((UnityEngine.Object) this._inventory_panel.selected_widget == (UnityEngine.Object) null)
      return;
    if (this._inventory_panel.selected_widget_is_not_main)
    {
      Item inventoryData = this._inventory_panel.selected_widget.inventory_data;
      if (inventoryData == null || !inventoryData.is_bag)
        return;
    }
    if (this.selected_item == null || this.selected_item.IsEmpty() || this.selected_item.definition.player_cant_throw_out)
      return;
    this._inventory_panel.selected_widget.inventory_data.RemoveItem(this.selected_item);
    this.Redraw();
    if (!BaseGUI.for_gamepad)
      return;
    this._inventory_panel.selected_item_gui.OnOver(true);
  }

  public void UseItem()
  {
    Item use_from_bag = (Item) null;
    if (this._inventory_panel.selected_widget_is_not_main)
    {
      use_from_bag = this._inventory_panel.selected_widget.inventory_data;
      if (use_from_bag == null || !use_from_bag.is_bag)
        return;
    }
    Item item = this.selected_item;
    if (item == null || item.IsEmpty())
      return;
    if (item.is_bag)
    {
      Debug.Log((object) $"#BAG# Item \"{item.id}\" is used!");
      if (this._inventory_state == InventoryGUI.InventoryGUIState.BagIsOpen)
      {
        if (this._current_open_bag == item)
        {
          this.CloseBag();
        }
        else
        {
          this.CloseBag();
          GJTimer.AddTimer(0.01f, (GJTimer.VoidDelegate) (() => this.OpenBag(item)));
        }
      }
      else
        this.OpenBag(item);
    }
    else
    {
      if (!item.definition.can_be_used)
        return;
      if (item.definition.close_inv_on_use)
      {
        GUIElements.me.game_gui.Hide(true);
        MainGame.me.player.UseItemFromInventory(item, use_from_bag: use_from_bag);
      }
      else
      {
        MainGame.me.player.UseItemFromInventory(item, new Vector3?(this.head_pos), use_from_bag);
        this.Redraw();
        this._inventory_panel.selected_item_gui.OnOver(BaseGUI.for_gamepad);
      }
    }
  }

  public void Redraw()
  {
    this._inventory_panel.Redraw();
    this.UpdateButtonTips();
    this.RedrawPlayerInfoAndToolbelt();
    if (this._inventory_state == InventoryGUI.InventoryGUIState.BagIsOpen)
      this.UpdateBagInventoryWidget();
    this.UpdateFiltering();
  }

  public void UpdateFiltering()
  {
    if (this._inventory_state == InventoryGUI.InventoryGUIState.Standart)
    {
      this._inventory_panel.FilterItems((InventoryWidget.ItemFilterDelegate) ((item, widget) => InventoryWidget.ItemFilterResult.Active));
    }
    else
    {
      if (this._inventory_state != InventoryGUI.InventoryGUIState.BagIsOpen)
        return;
      this._inventory_panel.FilterItems((InventoryWidget.ItemFilterDelegate) ((item, widget) => (UnityEngine.Object) widget != (UnityEngine.Object) null && widget.inventory_data != null && (!widget.IsMain() && !widget.inventory_data.is_bag || widget.inventory_data == this._current_open_bag) || item == null || item.IsEmpty() || item != this._current_open_bag && !item.CanBeInsertedInBag(this._current_open_bag) ? InventoryWidget.ItemFilterResult.Inactive : InventoryWidget.ItemFilterResult.Active));
    }
  }

  public void RedrawToolbelt()
  {
    if (this.toolbelt_items == null)
      return;
    foreach (InventoryGUI.ToolbeltItemDescriptionGUI toolbeltItem in this.toolbelt_items)
      toolbeltItem.item.DrawItem(MainGame.me.player.GetItemFromToolbelt(toolbeltItem.type));
    this.toolbelt_widget.UpdateItemsCallbacksAndStuff(1);
  }

  public void RedrawPlayerInfoAndToolbelt() => this.RedrawToolbelt();

  public bool CanBeEquipped(Item item)
  {
    if (item == null || item.IsEmpty())
      return false;
    ItemDefinition definition = item.definition;
    return (definition.IsWeapon() || definition.IsEquipment()) && item.durability_state != 0;
  }

  public void UpdateButtonTips()
  {
    if (!BaseGUI.for_gamepad)
      return;
    this.button_tips.Activate<ButtonTipsStr>();
    if (this._inventory_state == InventoryGUI.InventoryGUIState.BagIsOpen && !this.IsToolbeltItemFocused())
    {
      if (this.selected_item == null)
      {
        this.button_tips.PrintClose();
      }
      else
      {
        InventoryPanelGUI last = InventoryPanelGUI.last;
        this._last_selected_panel = last;
        bool to_bag = (UnityEngine.Object) last == (UnityEngine.Object) this._inventory_panel;
        if (((UnityEngine.Object) last == (UnityEngine.Object) null || last.selected_widget_is_not_main) && (to_bag && last.selected_widget.inventory_data == null || !last.selected_widget.inventory_data.is_bag))
        {
          this.button_tips.PrintClose();
        }
        else
        {
          bool selectedItemIsEmpty = last.selected_item_is_empty;
          if (last.selected_item.is_bag)
          {
            this.button_tips.Print(GameKeyTip.Select(last.selected_item == this._current_open_bag ? "close" : "open"), GameKeyTip.Close());
          }
          else
          {
            int num = 0;
            if (!selectedItemIsEmpty)
            {
              Item from_not_open_bag = (Item) null;
              if (to_bag && this._inventory_panel.selected_widget_is_not_main)
                from_not_open_bag = this._inventory_panel.selected_widget.inventory_data;
              num = this.GetMaxMoveCount(last.selected_item, to_bag, from_not_open_bag: from_not_open_bag);
            }
            if (num <= 1)
              this.button_tips.Print(GameKeyTip.Select(to_bag ? "put" : "take", !selectedItemIsEmpty && num > 0), GameKeyTip.Close());
            else
              this.button_tips.Print(GameKeyTip.Select(to_bag ? "put" : "take", !selectedItemIsEmpty), GameKeyTip.Option1(GJL.L(to_bag ? "put" : "take") + " 1", !selectedItemIsEmpty), GameKeyTip.Close());
          }
        }
      }
    }
    else if (this.selected_item == null)
    {
      this.button_tips.PrintClose();
    }
    else
    {
      Item toolbelt_focused_item = (Item) null;
      if (this.selected_item.is_bag)
      {
        this.button_tips.Print(GameKeyTip.Select("open"), GameKeyTip.Close());
      }
      else
      {
        int num = this.IsToolbeltItemFocused(out toolbelt_focused_item) ? 1 : 0;
        if (toolbelt_focused_item == null)
          toolbelt_focused_item = this.selected_item;
        Item inventoryData = this._inventory_panel.selected_widget.inventory_data;
        bool flag1 = inventoryData != null && inventoryData.is_bag;
        if (num == 0 && this._inventory_panel.selected_widget_is_not_main && !flag1)
        {
          this.button_tips.PrintClose();
        }
        else
        {
          bool flag2 = toolbelt_focused_item == null || toolbelt_focused_item.IsEmpty();
          bool flag3 = !flag2 && toolbelt_focused_item.definition.can_be_used && toolbelt_focused_item.durability_state != 0;
          bool flag4 = !flag2 && this.CanBeEquipped(toolbelt_focused_item) | flag3;
          if (flag3 && toolbelt_focused_item.GetGrayedCooldownPercent() > 0)
            flag3 = false;
          this.button_tips.Print("\n", GameKeyTip.Select("use", !flag2 & flag3), GameKeyTip.Option1(flag2 || !flag4 || this.ItemIsEquipped(toolbelt_focused_item) ? "unequip" : "equip", !flag2 & flag4), GameKeyTip.Option2("destroy", !flag2 && !toolbelt_focused_item.definition.player_cant_throw_out));
        }
      }
    }
  }

  public void OpenBag(Item bag_item)
  {
    if (bag_item == null || bag_item.IsEmpty() || !bag_item.is_bag)
    {
      this.CloseBag();
    }
    else
    {
      this._current_open_bag = bag_item;
      this._inventory_state = InventoryGUI.InventoryGUIState.BagIsOpen;
      this._bag_inventory = new MultiInventory(new Inventory(this._current_open_bag, this._current_open_bag.id));
      this.bag_panel.Open(this._bag_inventory, navigation_sub_group: 1, custom_line_length: this._current_open_bag.definition.bag_size_x);
      this.bag_panel.SetCallbacks(new GJCommons.VoidDelegate(this.OnItemOverInBag), new GJCommons.VoidDelegate(this.OnItemOutInBag), new GJCommons.VoidDelegate(this.OnItemPressedInBag));
      Sounds.PlaySound("bag_open");
      this.UpdateFiltering();
      this.UpdateBagInventoryWidget();
      this.gamepad_controller.ReinitItems(false);
      this.UpdateButtonTips();
    }
  }

  public void CloseBag()
  {
    this._current_open_bag = (Item) null;
    this._inventory_state = InventoryGUI.InventoryGUIState.Standart;
    this.bag_panel.Hide();
    Sounds.PlaySound("bag_close");
    this.gamepad_controller.ReinitItems(false);
    this.UpdateFiltering();
    this.UpdateButtonTips();
  }

  public void UpdateBagInventoryWidget()
  {
    if (this._inventory_state == InventoryGUI.InventoryGUIState.Standart)
      this.CloseBag();
    else
      this.bag_panel.Redraw();
  }

  public void OnItemOverInBag()
  {
    if (!BaseGUI.for_gamepad)
      return;
    this.UpdateButtonTips();
  }

  public void OnItemOutInBag()
  {
    if (BaseGUI.for_gamepad || (UnityEngine.Object) this._context_menu_target_item == (UnityEngine.Object) null)
      return;
    this._context_menu_target_item.SetVisualyOveredState(true, false);
  }

  public void OnItemPressedInBag()
  {
    Item item = this.selected_item;
    if (item == null || item.IsEmpty())
      return;
    if (BaseGUI.for_gamepad && LazyInput.GetKeyDown(GameKey.Action) || LazyInput.GetKeyDown(GameKey.RightClick))
    {
      Debug.Log((object) $"#BAG# Moving 1 item \"{item.id}\" from Bag to Inventory");
      this.MoveItemToBag(item, 1, true);
    }
    else
    {
      Debug.Log((object) $"#BAG# Moving items \"{item.id}\" from Bag to Inventory");
      int maxMoveCount = this.GetMaxMoveCount(item, false);
      if (item.definition.stack_count == 1 || maxMoveCount == 1)
        this.MoveItemToBag(item, 1, true);
      else if (item.definition.stack_count > 1 && LazyInput.GetKeyDown(GameKey.MoveAllStack))
      {
        this.MoveItemToBag(item, maxMoveCount, true);
      }
      else
      {
        this.button_tips.Deactivate<ButtonTipsStr>();
        GUIElements.me.item_count.Open(item.id, 1, maxMoveCount, (Action<int>) (chosen_count => this.MoveItemToBag(item, chosen_count, true)));
        GUIElements.me.item_count.SetOnHide((GJCommons.VoidDelegate) (() =>
        {
          if (!BaseGUI.for_gamepad)
            return;
          this.button_tips.Activate<ButtonTipsStr>();
          this.gamepad_controller.Enable(GamepadNavigationController.OpenMethod.GetAll);
          this.gamepad_controller.SetFocusedItem(this.bag_panel.selected_item_gui.gamepad_item);
        }));
      }
    }
  }

  public int GetMaxMoveCount(Item item, bool to_bag, bool count_in_bags = false, Item from_not_open_bag = null)
  {
    if (from_not_open_bag != null && from_not_open_bag == this._current_open_bag || to_bag && !item.CanBeInsertedInBag(this._current_open_bag))
      return 0;
    MultiInventory multiInventory1 = to_bag ? this._inventory : this._bag_inventory;
    MultiInventory multiInventory2 = to_bag ? this._bag_inventory : this._inventory;
    if (from_not_open_bag != null)
      multiInventory1 = new MultiInventory(new Inventory(from_not_open_bag));
    return Mathf.Min(multiInventory1.GetTotalCount(item.id, MultiInventory.DestinationType.OnlyFirst, count_in_bags), multiInventory2.CanAddCount(item.id));
  }

  public void MoveItemToBag(Item item, int count, bool from_bag = false, Item from_not_open_bag = null)
  {
    if (this._inventory_state != InventoryGUI.InventoryGUIState.BagIsOpen || this._bag_inventory == null || item == null || item.IsEmpty())
      return;
    Sounds.PlaySound("item_put");
    MultiInventory multiInventory = from_bag ? this._bag_inventory : this._inventory;
    MultiInventory another_inventory = from_bag ? this._inventory : this._bag_inventory;
    InventoryPanelGUI inventoryPanelGui = from_bag ? this.bag_panel : this._inventory_panel;
    int num = inventoryPanelGui.selected_item.value;
    if (!from_bag && from_not_open_bag != null)
      multiInventory = new MultiInventory(new Inventory(from_not_open_bag));
    if (!multiInventory.MoveItemTo(another_inventory, item, count, true, false))
      return;
    this.Redraw();
    this.bag_panel.Redraw();
    inventoryPanelGui.UpdateSelection();
    if (count < num)
      return;
    TooltipsManager.Redraw();
  }

  [CompilerGenerated]
  public InventoryWidget.ItemFilterResult \u003CUpdateFiltering\u003Eb__62_1(
    Item item,
    InventoryWidget widget)
  {
    return (UnityEngine.Object) widget != (UnityEngine.Object) null && widget.inventory_data != null && (!widget.IsMain() && !widget.inventory_data.is_bag || widget.inventory_data == this._current_open_bag) || item == null || item.IsEmpty() || item != this._current_open_bag && !item.CanBeInsertedInBag(this._current_open_bag) ? InventoryWidget.ItemFilterResult.Inactive : InventoryWidget.ItemFilterResult.Active;
  }

  public enum EquipmentState
  {
    None,
    StartedFromInventory,
    StartedFromToolbar,
  }

  public enum InventoryGUIState
  {
    Standart,
    BagIsOpen,
  }

  [Serializable]
  public struct ToolbeltItemDescriptionGUI
  {
    public BaseItemCellGUI item;
    public ItemDefinition.EquipmentType type;
  }
}
