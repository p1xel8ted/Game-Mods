// Decompiled with JetBrains decompiler
// Type: MixedCraftGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Sirenix.Utilities;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MixedCraftGUI : BaseCraftGUI
{
  public UIWidget frame;
  public Dictionary<string, MixedCraftPresetGUI> _presets = new Dictionary<string, MixedCraftPresetGUI>();
  public GamepadSelectableButton _craft_button;
  public MixedCraftPresetGUI _current_preset;
  public bool _allow_empty;
  public InventoryWidget.ItemFilterDelegate _filter;
  public BaseItemCellGUI _current_item_gui;
  public int _current_item_gui_n = -1;
  public MultiInventory _multi_inventory;

  public override void Init()
  {
    this._craft_button = this.GetComponentInChildren<GamepadSelectableButton>(true);
    this._craft_button.Init();
    foreach (MixedCraftPresetGUI componentsInChild in this.GetComponentsInChildren<MixedCraftPresetGUI>(true))
      this._presets.Add(componentsInChild.name, componentsInChild);
    foreach (MixedCraftPresetGUI mixedCraftPresetGui in this._presets.Values)
      mixedCraftPresetGui.Init(this, new BaseItemCellGUI.OnItemAction(this.OnItemOver), (BaseItemCellGUI.OnItemAction) null, new BaseItemCellGUI.OnItemAction(this.OnItemSelect));
    base.Init();
  }

  public void Open(
    WorldGameObject craftery_wgo,
    string preset_name,
    bool allow_empty,
    InventoryWidget.ItemFilterDelegate filter)
  {
    if (!this._presets.ContainsKey(preset_name))
    {
      Debug.LogError((object) ("No MixCraftGUI preset for preset_name = " + preset_name));
    }
    else
    {
      this.CommonOpen(craftery_wgo, CraftDefinition.CraftType.MixedCraft);
      GUIElements.me.resource_picker.ClearResultDelegate();
      GUIElements.me.resource_picker.Hide(true);
      this._current_preset = this._presets[preset_name];
      this._allow_empty = allow_empty;
      this._filter = filter;
      MultiInventory.PlayerMultiInventory player_mi = MultiInventory.PlayerMultiInventory.DontChange;
      if (GlobalCraftControlGUI.is_global_control_active && !WorldZone.GetZoneOfObject(craftery_wgo).IsPlayerInZone())
        player_mi = MultiInventory.PlayerMultiInventory.ExcludePlayer;
      this._multi_inventory = MainGame.me.player.GetMultiInventory(player_mi: player_mi, sortWGOS: true);
      foreach (MixedCraftPresetGUI mixedCraftPresetGui in this._presets.Values)
      {
        if ((Object) mixedCraftPresetGui != (Object) this._current_preset)
          mixedCraftPresetGui.Hide();
      }
      this._current_preset.Open(BaseGUI.for_gamepad);
      this._craft_button.SetCallbacks(new GJCommons.VoidDelegate(this.OnCraftPressed), new GJCommons.VoidDelegate(this.OnCraftButtonOver));
      this.frame.width = this._current_preset.ui_widget.width;
      this.frame.height = this._current_preset.ui_widget.height;
      if (BaseGUI.for_gamepad)
        this.gamepad_controller.ReinitItems(true);
      this._craft_button.SetEnabled(false);
    }
  }

  public void OpenAsAlchemy(WorldGameObject craftery_wgo, string preset_name)
  {
    this.Open(craftery_wgo, preset_name, false, new InventoryWidget.ItemFilterDelegate(this.AlchemyItemPickerFilter));
  }

  public InventoryWidget.ItemFilterResult AlchemyItemPickerFilter(Item item, InventoryWidget widget)
  {
    if (item == null || item.definition == null)
      return InventoryWidget.ItemFilterResult.Hide;
    if (item.definition.alch_type != ItemDefinition.AlchemyType.Universal && item.definition.alch_type != (ItemDefinition.AlchemyType) (this._current_item_gui_n + 1))
      return InventoryWidget.ItemFilterResult.Inactive;
    foreach (Item selectedItem in this._current_preset.GetSelectedItems())
    {
      if (selectedItem?.id == item?.id)
        return InventoryWidget.ItemFilterResult.Inactive;
    }
    return InventoryWidget.ItemFilterResult.Active;
  }

  public bool IsCraftAllowed()
  {
    int num = 0;
    List<Item> selectedItems = this._current_preset.GetSelectedItems();
    foreach (Item obj in selectedItems)
    {
      if (obj.IsEmpty())
        ++num;
    }
    return num != selectedItems.Count && (this._allow_empty || num <= 0);
  }

  public void OnCraftPressed()
  {
    if (!this.IsCraftAllowed())
      return;
    bool do_override_needs;
    CraftDefinition craftDefinition = this.GetCraftDefinition(false, out do_override_needs);
    bool flag = craftDefinition != null;
    if (!flag)
      craftDefinition = this.GetCraftDefinition(true, out do_override_needs);
    if (craftDefinition == null)
    {
      Debug.LogError((object) "no mixed craft definition for this situation", (Object) this.gameObject);
    }
    else
    {
      List<Item> override_needs = flag | do_override_needs ? new List<Item>() : (List<Item>) null;
      this._current_preset?.ClearItems();
      WorldGameObject other_obj_override = (WorldGameObject) null;
      if (GlobalCraftControlGUI.is_global_control_active && (Object) this.craftery_wgo != (Object) null)
      {
        WorldZone myWorldZone = this.craftery_wgo.GetMyWorldZone();
        if ((Object) myWorldZone != (Object) null && !myWorldZone.IsPlayerInZone())
          other_obj_override = this.craftery_wgo;
      }
      this.OnCraft(craftDefinition, override_needs: override_needs, other_obj_override: other_obj_override);
      if (!this.IsASubWindow())
        return;
      GUIElements.me.craft.Hide(false);
    }
  }

  public string GetCraftDefinitionId(bool for_empty, out bool do_override_needs)
  {
    string craftDefinitionId = "mix:" + this.craftery_wgo.obj_id;
    do_override_needs = false;
    List<Item> selectedItems = this._current_preset.GetSelectedItems();
    string str1 = string.Empty;
    string str2 = string.Empty;
    if (for_empty)
    {
      List<CraftDefinition> craftDefinitionList = new List<CraftDefinition>();
      int count = selectedItems.Count;
      foreach (CraftDefinition craftDefinition in GameBalance.me.craft_data)
      {
        if (craftDefinition.craft_type == CraftDefinition.CraftType.MixedCraft && craftDefinition.needs.Count == count)
        {
          for (int index = 0; index < count; ++index)
          {
            if (craftDefinition.needs[index]?.id == selectedItems[index]?.id)
            {
              craftDefinitionList.Add(craftDefinition);
              break;
            }
          }
        }
      }
      if (craftDefinitionList.Count == 0)
      {
        string str3 = string.Empty;
        foreach (Item obj in selectedItems)
        {
          if (!string.IsNullOrEmpty(str3))
            str3 += ", ";
          str3 = $"{str3}\"{obj.id}\"";
        }
        Debug.LogWarning((object) $"Not found any proper alchemy ingridient for set {{{str3}}}");
      }
      else
      {
        int index1 = Random.Range(0, craftDefinitionList.Count);
        CraftDefinition craftDefinition = craftDefinitionList[index1];
        Debug.Log((object) ("Selected alchemy goo craft: " + craftDefinition.id));
        int index2;
        for (index2 = 0; index2 < selectedItems.Count && !(selectedItems[index2].id == craftDefinition.needs[index2].id); ++index2)
        {
          if (index2 == selectedItems.Count - 1)
            Debug.LogError((object) "Wrong shit happen while trying to find propper alchemy craft! Call Bulat, it's his shitcode!");
        }
        for (int index3 = 0; index3 < craftDefinition.needs.Count; ++index3)
        {
          if (index3 != index2)
          {
            if (string.IsNullOrEmpty(str1))
              str1 = ItemDefinition.GetGooFromAlchemyIngridient(craftDefinition.needs[index3]?.id);
            else if (string.IsNullOrEmpty(str2))
            {
              str2 = ItemDefinition.GetGooFromAlchemyIngridient(craftDefinition.needs[index3]?.id);
              break;
            }
          }
        }
      }
    }
    bool flag1 = string.IsNullOrEmpty(str1);
    bool flag2 = string.IsNullOrEmpty(str2);
    foreach (Item obj in selectedItems)
    {
      if (for_empty && !flag1)
      {
        flag1 = true;
        do_override_needs = true;
        craftDefinitionId = $"{craftDefinitionId}:{str1}";
      }
      else if (for_empty && !flag2)
      {
        flag2 = true;
        craftDefinitionId = $"{craftDefinitionId}:{str2}";
      }
      else
        craftDefinitionId = $"{craftDefinitionId}:{(for_empty ? "_" : (obj.IsEmpty() ? "" : obj.id))}";
    }
    for (int count = selectedItems.Count; count < 4; ++count)
      craftDefinitionId += ":";
    return craftDefinitionId;
  }

  public CraftDefinition GetCraftDefinition(bool for_empty, out bool do_override_needs)
  {
    string craftDefinitionId = this.GetCraftDefinitionId(for_empty, out do_override_needs);
    foreach (CraftDefinition craft in this.crafts)
    {
      if (craft.id == craftDefinitionId)
        return craft;
    }
    return (CraftDefinition) null;
  }

  public void ReturnItemsToInventory()
  {
    if (!((Object) this._current_preset != (Object) null))
      return;
    List<Item> cant_insert1 = new List<Item>();
    MainGame.me.player.TryPutToInventory(this._current_preset.GetSelectedItems(), out cant_insert1);
    if (!cant_insert1.IsNullOrEmpty<Item>())
    {
      List<Item> cant_insert2;
      this.craftery_wgo.PutToAllPossibleInventories(cant_insert1, out cant_insert2);
      if (!cant_insert2.IsNullOrEmpty<Item>())
        this.craftery_wgo.DropItems(cant_insert2);
    }
    this._current_preset.ClearItems();
  }

  public void OnItemOver(BaseItemCellGUI item_gui)
  {
    this._current_item_gui = item_gui;
    if (!BaseGUI.for_gamepad)
      return;
    this.UpdateGamepadTips(item_gui.item.IsEmpty() ? "select" : "change", true);
  }

  public void OnItemSelect(BaseItemCellGUI item_gui)
  {
    this._current_item_gui = item_gui;
    this._current_item_gui_n = -1;
    for (int index = 0; index < this._current_preset.items.Length; ++index)
    {
      if ((Object) this._current_preset.items[index] == (Object) this._current_item_gui)
      {
        this._current_item_gui_n = index;
        break;
      }
    }
    Debug.Log((object) ("OnItemSelect, n = " + this._current_item_gui_n.ToString()), (Object) item_gui);
    WorldGameObject worldGameObject = MainGame.me.player;
    if (GlobalCraftControlGUI.is_global_control_active && (Object) this.craftery_wgo != (Object) null)
      worldGameObject = this.craftery_wgo;
    GUIElements.me.resource_picker.Open(worldGameObject, this._filter, new CraftResourcesSelectGUI.ResourceSelectResultDelegate(this.OnResourcePickerClosed));
  }

  public void OnCraftButtonOver()
  {
    this._current_item_gui = (BaseItemCellGUI) null;
    if (!BaseGUI.for_gamepad)
      return;
    this.UpdateGamepadTips("select", this.IsCraftAllowed());
  }

  public void OnResourcePickerClosed(Item item)
  {
    if ((Object) this._current_item_gui == (Object) null)
      return;
    Item i = Item.empty;
    if (item != null)
    {
      i = new Item(item)
      {
        value = 1,
        equipped_as = ItemDefinition.EquipmentType.None
      };
      if (!this._current_item_gui.item.IsEmpty())
        this._multi_inventory.AddItem(this._current_item_gui.item);
    }
    if (!i.IsEmpty())
    {
      this._multi_inventory.RemoveItem(i);
      this._current_item_gui.DrawItem(i);
    }
    this._craft_button.SetEnabled(this.IsCraftAllowed());
    if (!BaseGUI.for_gamepad)
      return;
    this.gamepad_controller.Enable(GamepadNavigationController.OpenMethod.GetAll);
    this.gamepad_controller.SetFocusedItem(this.IsCraftAllowed() ? this._craft_button.navigation_item : this._current_item_gui.gamepad_item);
  }

  public void UpdateGamepadTips(string select_text, bool active)
  {
    this.button_tips.Print(GameKeyTip.Select(select_text, active), GameKeyTip.Close());
  }

  public override void Hide(bool play_hide_sound = true)
  {
    this.ReturnItemsToInventory();
    this._current_item_gui = (BaseItemCellGUI) null;
    if ((Object) this._current_preset != (Object) null)
      this._current_preset.Hide();
    base.Hide(play_hide_sound);
  }

  public bool IsASubWindow() => GUIElements.me.craft.is_shown;

  public override bool OnPressedBack()
  {
    this.OnClosePressed();
    return true;
  }

  public override void OnClosePressed()
  {
    if (this.IsASubWindow())
      GUIElements.me.craft.Hide(false);
    base.OnClosePressed();
  }

  public override bool OnPressedPrevTab()
  {
    return this.IsASubWindow() ? GUIElements.me.craft.PressPrevTab() : base.OnPressedPrevTab();
  }

  public override bool OnPressedNextTab()
  {
    return this.IsASubWindow() ? GUIElements.me.craft.PressNextTab() : base.OnPressedPrevTab();
  }
}
