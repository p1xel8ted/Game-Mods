// Decompiled with JetBrains decompiler
// Type: OrganEnhancerGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Sirenix.Utilities;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class OrganEnhancerGUI : BaseGUI, CraftInterface
{
  public const string TOTAL_WHITE_SKULLS_WGO_RES = "total_white_skulls";
  public const string TOTAL_RED_SKULLS_WGO_RES = "total_red_skulls";
  public const int MAX_ORGAN_ENHANCING_CUP = 3;
  public UIWidget frame;
  [SerializeField]
  public BaseItemCellGUI _base_item_cell;
  [SerializeField]
  public GamepadSelectableButton _choose_craft_button;
  public InventoryWidget.ItemFilterDelegate _filter;
  public BaseItemCellGUI _current_item_gui;
  public MultiInventory _multi_inventory;
  public WorldGameObject _craftery_wgo;

  public override void Init()
  {
    this._choose_craft_button.Init();
    this._base_item_cell.SetCallbacks(new BaseItemCellGUI.OnItemAction(this.OnItemOver), (BaseItemCellGUI.OnItemAction) null, new BaseItemCellGUI.OnItemAction(this.OnItemSelect));
    this._choose_craft_button.SetCallbacks(new GJCommons.VoidDelegate(this.OnChooseButtonPress), new GJCommons.VoidDelegate(this.OnCraftButtonOver));
    base.Init();
  }

  public void Open(WorldGameObject craftery_wgo)
  {
    this._craftery_wgo = craftery_wgo;
    this._base_item_cell.DrawEmpty();
    this._base_item_cell.InitInputBehaviour();
    MultiInventory.PlayerMultiInventory player_mi = MultiInventory.PlayerMultiInventory.DontChange;
    WorldGameObject worldGameObject;
    if (GlobalCraftControlGUI.is_global_control_active)
    {
      if (!WorldZone.GetZoneOfObject(craftery_wgo).IsPlayerInZone())
        player_mi = MultiInventory.PlayerMultiInventory.ExcludePlayer;
      worldGameObject = this._craftery_wgo;
    }
    else
      worldGameObject = MainGame.me.player;
    this._multi_inventory = worldGameObject.GetMultiInventory(player_mi: player_mi, sortWGOS: true);
    this.Open();
    if (!BaseGUI.for_gamepad)
      return;
    this.button_tips.Activate<ButtonTipsStr>();
    this.gamepad_controller.Enable();
    this.gamepad_controller.ReinitItems(true);
  }

  public void Hide(bool play_hide_sound = true, bool return_item_back = false)
  {
    if (return_item_back)
      this.ReturnItemToInventory();
    this.Hide(play_hide_sound);
  }

  public bool CanCraft(
    CraftDefinition craft,
    List<string> multiquality_ids = null,
    int amount = 1,
    List<Item> override_needs = null)
  {
    return this._multi_inventory.IsEnoughItems(craft.needs) && craft.condition.EvaluateBoolean(this._craftery_wgo, MainGame.me.player);
  }

  public bool OnCraft(
    CraftDefinition craft,
    Item try_use_particular_item = null,
    List<string> multiquality_ids = null,
    int amount = 1,
    List<Item> override_needs = null,
    WorldGameObject other_obj_override = null)
  {
    this._craftery_wgo.components.craft.Craft(craft, override_needs: craft.needs, ignore_crafts_list: true);
    this._current_item_gui = (BaseItemCellGUI) null;
    GUIElements.me.craft.Hide(false);
    if (GlobalCraftControlGUI.is_global_control_active)
      GUIElements.me.global_craft_control_gui.Open();
    this.Hide(true, false);
    return true;
  }

  public void OnCraftButtonOver()
  {
    if (!BaseGUI.for_gamepad)
      return;
    this.button_tips.Print(GameKeyTip.Select(!((UnityEngine.Object) this._current_item_gui == (UnityEngine.Object) null) && !this._current_item_gui.item.IsEmpty()), GameKeyTip.Close());
  }

  public new void OnRightClick() => base.OnRightClick();

  public void OnChooseButtonPress()
  {
    if ((UnityEngine.Object) this._current_item_gui == (UnityEngine.Object) null || this._current_item_gui.item.IsEmpty())
      return;
    WorldGameObject crafteryWgo = this._craftery_wgo;
    Item obj = this._current_item_gui.item;
    this.Hide(true, false);
    GUIElements.me.craft.OpenAsOrganEnhancer(crafteryWgo, obj, new System.Action(this.ReturnItemToInventory));
  }

  public static int GetCraftIndex(WorldGameObject wgo, string skulls_res)
  {
    if (!((UnityEngine.Object) wgo != (UnityEngine.Object) null))
      return 0;
    int num1 = wgo.GetParamInt(skulls_res);
    if (num1 < 0)
      num1 = 0;
    int num2;
    return num1 == 3 ? 3 : (num2 = num1 + 1);
  }

  public static Item TryGetModifiedVersionOfItem(Item item)
  {
    if (item.definition.stack_count <= 1)
      return item;
    Item modifiedVersionOfItem = new Item(item);
    modifiedVersionOfItem.SetItemID(item.id + "_mod");
    return modifiedVersionOfItem;
  }

  public static Item GetModifiedItemForCraftOutput(Item item, bool is_white_skull_change)
  {
    string str = item.id.Split('_')[0];
    if (!str.Contains(":"))
      str = $"{item.id}:{item.id}";
    int whiteSkullsValue = item.GetWhiteSkullsValue();
    int redSkullsValue = item.GetRedSkullsValue();
    if (is_white_skull_change)
    {
      if (whiteSkullsValue < 3)
        ++whiteSkullsValue;
    }
    else if (redSkullsValue < 3)
      ++redSkullsValue;
    return new Item(str + $"_{redSkullsValue}_{whiteSkullsValue}");
  }

  public override bool OnPressedBack()
  {
    this.Hide(true, true);
    if (GUIElements.me.craft.is_shown && BaseGUI.for_gamepad)
    {
      GUIElements.me.craft.gamepad_controller.Enable();
      GUIElements.me.craft.gamepad_controller.ReinitItems(true);
    }
    return true;
  }

  public override void OnClosePressed() => this.Hide(true, true);

  public void OnItemOver(BaseItemCellGUI item_gui)
  {
    this._current_item_gui = item_gui;
    if (!BaseGUI.for_gamepad)
      return;
    this.button_tips.Print(GameKeyTip.Select(item_gui.item.IsEmpty() ? "select" : "change"), GameKeyTip.Close());
  }

  public void OnItemSelect(BaseItemCellGUI item_gui)
  {
    if ((UnityEngine.Object) this._current_item_gui != (UnityEngine.Object) null && !this._current_item_gui.item.IsEmpty())
      this.ReturnItemToInventory();
    this._current_item_gui = item_gui;
    WorldGameObject worldGameObject = MainGame.me.player;
    if (GlobalCraftControlGUI.is_global_control_active && (UnityEngine.Object) this._craftery_wgo != (UnityEngine.Object) null)
      worldGameObject = this._craftery_wgo;
    GUIElements.me.resource_picker.Open(worldGameObject, (InventoryWidget.ItemFilterDelegate) ((item, widget) =>
    {
      if (item == null || item.IsEmpty() || item.definition.type != ItemDefinition.ItemType.BodyUniversalPart)
        return InventoryWidget.ItemFilterResult.Hide;
      return item.id != "skull" && item.id != "bone" && item.id != "surgeon_mistake" && !item.id.EndsWith("_dark") && item.id != "tr_bellas_ring" && (item.GetRedSkullsValue() < 3 || item.GetWhiteSkullsValue() < 3) ? InventoryWidget.ItemFilterResult.Active : InventoryWidget.ItemFilterResult.Inactive;
    }), new CraftResourcesSelectGUI.ResourceSelectResultDelegate(this.OnResourcePickerClosed), true);
  }

  public void OnResourcePickerClosed(Item item)
  {
    if ((UnityEngine.Object) this._current_item_gui == (UnityEngine.Object) null)
      return;
    if (BaseGUI.for_gamepad)
    {
      this.button_tips.Activate<ButtonTipsStr>();
      this.gamepad_controller.Enable();
      this.gamepad_controller.ReinitItems(true);
    }
    if (item != null && !item.IsEmpty())
    {
      if (item.value > 1)
        item = new Item(item) { value = 1 };
      this._multi_inventory.RemoveItem(item, 1);
      this._current_item_gui.DrawItem(item);
    }
    this.SetValuesToCrafteryWGO(item);
    this._choose_craft_button.SetEnabled(true);
    if (!BaseGUI.for_gamepad)
      return;
    this.gamepad_controller.Enable(GamepadNavigationController.OpenMethod.GetAll);
    this.gamepad_controller.SetFocusedItem(this._choose_craft_button.navigation_item);
  }

  public void ReturnItemToInventory()
  {
    if ((UnityEngine.Object) this._current_item_gui == (UnityEngine.Object) null)
      return;
    List<Item> objList = new List<Item>();
    if (!GlobalCraftControlGUI.is_global_control_active)
    {
      WorldGameObject player = MainGame.me.player;
      List<Item> items_to_insert = new List<Item>();
      items_to_insert.Add(this._current_item_gui.item);
      ref List<Item> local = ref objList;
      player.TryPutToInventory(items_to_insert, out local);
    }
    else if (WorldZone.GetZoneOfObject(this._craftery_wgo).IsPlayerInZone())
    {
      WorldGameObject player = MainGame.me.player;
      List<Item> items_to_insert = new List<Item>();
      items_to_insert.Add(this._current_item_gui.item);
      ref List<Item> local = ref objList;
      player.TryPutToInventory(items_to_insert, out local);
    }
    else
    {
      WorldGameObject crafteryWgo = this._craftery_wgo;
      List<Item> drop_list = new List<Item>();
      drop_list.Add(this._current_item_gui.item);
      ref List<Item> local = ref objList;
      crafteryWgo.PutToAllPossibleInventories(drop_list, out local);
    }
    if (!objList.IsNullOrEmpty<Item>())
    {
      List<Item> cant_insert;
      this._craftery_wgo.PutToAllPossibleInventories(objList, out cant_insert);
      if (!cant_insert.IsNullOrEmpty<Item>())
        this._craftery_wgo.DropItems(cant_insert);
    }
    this._current_item_gui.DrawEmpty();
  }

  public void SetValuesToCrafteryWGO(Item organ_item)
  {
    if (organ_item == null || organ_item.IsEmpty() || !((UnityEngine.Object) this._craftery_wgo != (UnityEngine.Object) null) || organ_item.definition.type != ItemDefinition.ItemType.BodyUniversalPart)
      return;
    this._craftery_wgo.SetParam("total_white_skulls", (float) organ_item.GetWhiteSkullsValue());
    this._craftery_wgo.SetParam("total_red_skulls", (float) organ_item.GetRedSkullsValue());
  }

  public void ResetValuesFromCrafteryWGO()
  {
    if (!((UnityEngine.Object) this._craftery_wgo != (UnityEngine.Object) null))
      return;
    this._craftery_wgo.SetParam("total_white_skulls", 0.0f);
    this._craftery_wgo.SetParam("total_red_skulls", 0.0f);
  }
}
