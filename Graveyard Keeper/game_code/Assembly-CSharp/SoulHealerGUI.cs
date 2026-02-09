// Decompiled with JetBrains decompiler
// Type: SoulHealerGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class SoulHealerGUI : BaseGUI
{
  [SerializeField]
  public UniversalObjectInfoGUI _universal_info;
  [SerializeField]
  public SoulHealingWidget _soul_healing_widget;
  [SerializeField]
  [Space]
  public List<SinItem> _sin_items = new List<SinItem>();
  public WorldGameObject _wgo;
  public BaseItemCellGUI _current_gui_item;
  public SinItem _current_sin_item;
  public float sins_heal_rate;

  public SoulHealingWidget soul_healing_widget => this._soul_healing_widget;

  public override void Init()
  {
    this._soul_healing_widget.Init();
    this._soul_healing_widget.SetActive(true);
    this._soul_healing_widget.ON_GUI_ITEM_CHANGE += new System.Action(this.Redraw);
    this._soul_healing_widget.ON_FOCUS_GAMEPAD_ITEM += new Action<GamepadNavigationItem>(this.FocusGamepadItem);
    base.Init();
  }

  public void Open(WorldGameObject obj)
  {
    this.Open();
    this._wgo = obj;
    for (int index = 0; index < this._sin_items.Count; ++index)
    {
      if (!this._sin_items[index].IsSinUnlocked(this._wgo))
      {
        this._sin_items[index].gameObject.SetActive(false);
      }
      else
      {
        this._sin_items[index].SetSinValuesInSkulls();
        this._sin_items[index].item_cell_gui.SetCallbacks(new BaseItemCellGUI.OnItemAction(this.OnItemSinOver), (BaseItemCellGUI.OnItemAction) null, new BaseItemCellGUI.OnItemAction(this.OnItemSinPress));
        this._sin_items[index].item_cell_gui.InitInputBehaviour();
        this._sin_items[index].item_cell_gui.InitTooltips();
        this._sin_items[index].item_cell_gui.DrawItem(obj.data.GetItemByIndex(index));
      }
    }
    this.DrawLabelsAndCalculateHealRate();
    this._soul_healing_widget.sins_heal_rate = this.sins_heal_rate;
    this._soul_healing_widget.Draw(obj, this._sin_items);
    this._universal_info.Draw(obj.GetUniversalObjectInfo());
    if (!BaseGUI.for_gamepad)
      return;
    this.button_tips.Activate<ButtonTipsStr>();
    this.gamepad_controller.Enable();
    this.gamepad_controller.ReinitItems(true);
  }

  public override void Hide(bool play_hide_sound = true)
  {
    this._wgo = (WorldGameObject) null;
    this._current_gui_item = (BaseItemCellGUI) null;
    this._current_sin_item = (SinItem) null;
    if (GlobalCraftControlGUI.is_global_control_active)
      GUIElements.me.global_craft_control_gui.Open();
    base.Hide(play_hide_sound);
  }

  public override bool OnPressedBack()
  {
    this.OnClosePressed();
    return true;
  }

  public override void OnClosePressed()
  {
    if (GlobalCraftControlGUI.is_global_control_active)
      GUIElements.me.global_craft_control_gui.Open();
    base.OnClosePressed();
  }

  public void Redraw()
  {
    this.DrawLabelsAndCalculateHealRate();
    this._soul_healing_widget.sins_heal_rate = this.sins_heal_rate;
    this._soul_healing_widget.Redraw();
  }

  public void DrawLabelsAndCalculateHealRate()
  {
    this.sins_heal_rate = 0.0f;
    Item insertedItem = this._soul_healing_widget.inserted_item;
    bool flag = insertedItem != null && !insertedItem.IsEmpty();
    int white_skulls_organ = 0;
    int red_skulls_organ = 0;
    int num = 0;
    for (int index = 0; index < this._sin_items.Count; ++index)
    {
      if (this._sin_items[index].IsSinUnlocked(this._wgo))
      {
        this.SetParamAccordingSinPart(this._sin_items[index], 0.0f);
        this._sin_items[index].SetSinValuesInSkulls();
        Item obj = this._sin_items[index].item_cell_gui.item;
        if (obj != null && !obj.IsEmpty())
        {
          white_skulls_organ = obj.GetWhiteSkullsValue();
          red_skulls_organ = obj.GetRedSkullsValue();
          if (white_skulls_organ < 0)
            white_skulls_organ = 0;
          if (red_skulls_organ < 0)
            red_skulls_organ = 0;
        }
        if (flag)
        {
          Item itemByTypeFromItem = SinItem.GetSinItemByTypeFromItem(this._sin_items[index].item_type, insertedItem);
          if (itemByTypeFromItem != null && !itemByTypeFromItem.IsEmpty())
          {
            this._sin_items[index].SetSinValuesInSkulls(true, itemByTypeFromItem.GetRedSkullsValue(), itemByTypeFromItem.GetWhiteSkullsValue(), red_skulls_organ, white_skulls_organ);
            ++num;
            this.SetParamAccordingSinPart(this._sin_items[index], 1f);
          }
          else
            this._sin_items[index].SetSinValuesInSkulls(red_skulls_organ: red_skulls_organ, white_skulls_organ: white_skulls_organ);
        }
        else
          this._sin_items[index].SetSinValuesInSkulls(red_skulls_organ: red_skulls_organ, white_skulls_organ: white_skulls_organ);
        white_skulls_organ = 0;
        red_skulls_organ = 0;
        this.sins_heal_rate += this._sin_items[index].GetHealRate();
      }
    }
    if (num <= 0)
      return;
    this.sins_heal_rate /= (float) num;
  }

  public void OnItemSinPress(BaseItemCellGUI item_gui)
  {
    this._current_gui_item = item_gui;
    SinItem component;
    if (item_gui.TryGetComponent<SinItem>(out component))
    {
      this._current_sin_item = component;
      if (item_gui.item != null && !item_gui.item.IsEmpty())
      {
        this._wgo.data.RemoveItemByIndex(this._sin_items.FindIndex((Predicate<SinItem>) (x => (UnityEngine.Object) x == (UnityEngine.Object) this._current_sin_item)));
        List<Item> items = new List<Item>();
        if (!GlobalCraftControlGUI.is_global_control_active)
        {
          WorldGameObject player = MainGame.me.player;
          List<Item> items_to_insert = new List<Item>();
          items_to_insert.Add(item_gui.item);
          ref List<Item> local = ref items;
          player.TryPutToInventory(items_to_insert, out local);
        }
        else
        {
          WorldGameObject wgo = this._wgo;
          List<Item> drop_list = new List<Item>();
          drop_list.Add(item_gui.item);
          ref List<Item> local = ref items;
          wgo.PutToAllPossibleInventories(drop_list, out local);
        }
        if (items.Count > 0)
          this._wgo.DropItems(items);
        this.SetParamIsOrganInserted(0.0f);
        this._current_gui_item.DrawEmpty();
        this.Redraw();
        if (!BaseGUI.for_gamepad)
          return;
        this.gamepad_controller.Enable(GamepadNavigationController.OpenMethod.GetAll);
        this.gamepad_controller.SetFocusedItem(this._current_gui_item.gamepad_item);
      }
      else
      {
        string organ_item_id = SinItem.GetOrganIdBySin(component);
        if (string.IsNullOrEmpty(organ_item_id))
          return;
        WorldGameObject worldGameObject = MainGame.me.player;
        if (GlobalCraftControlGUI.is_global_control_active && (UnityEngine.Object) this._wgo != (UnityEngine.Object) null)
          worldGameObject = this._wgo;
        GUIElements.me.resource_picker.Open(worldGameObject, (InventoryWidget.ItemFilterDelegate) ((item, widget) =>
        {
          if (item == null || item.IsEmpty())
            return InventoryWidget.ItemFilterResult.Hide;
          if (item.definition.type != ItemDefinition.ItemType.BodyUniversalPart)
            return InventoryWidget.ItemFilterResult.Inactive;
          string id = item.id;
          if (id.Contains(":"))
            id = id.Split(':')[0];
          return id == organ_item_id ? InventoryWidget.ItemFilterResult.Active : InventoryWidget.ItemFilterResult.Inactive;
        }), new CraftResourcesSelectGUI.ResourceSelectResultDelegate(this.OnItemSinForInsertionPicked));
      }
    }
    else
      Debug.LogError((object) "SinItem component not found", (UnityEngine.Object) this);
  }

  public void OnItemSinOver(BaseItemCellGUI item_gui)
  {
    if (!BaseGUI.for_gamepad)
      return;
    this.button_tips.Print(GameKeyTip.Select(item_gui.item.IsEmpty() ? "select" : "change"), GameKeyTip.Close());
  }

  public void OnItemSinForInsertionPicked(Item selected_item)
  {
    if (selected_item == null || selected_item.IsEmpty())
    {
      if (!BaseGUI.for_gamepad)
        return;
      this.gamepad_controller.Enable(GamepadNavigationController.OpenMethod.GetAll);
      this.gamepad_controller.SetFocusedItem(this._current_gui_item.gamepad_item);
    }
    else
    {
      Item i = new Item(selected_item)
      {
        value = 1,
        equipped_as = ItemDefinition.EquipmentType.None
      };
      this._current_gui_item.DrawItem(i);
      MultiInventory.PlayerMultiInventory player_mi = MultiInventory.PlayerMultiInventory.DontChange;
      if (GlobalCraftControlGUI.is_global_control_active && !WorldZone.GetZoneOfObject(this._wgo).IsPlayerInZone())
        player_mi = MultiInventory.PlayerMultiInventory.ExcludePlayer;
      MainGame.me.player.GetMultiInventory(player_mi: player_mi, sortWGOS: true).RemoveItem(i);
      this._wgo.data.AddItemByIndex(i, this._sin_items.FindIndex((Predicate<SinItem>) (x => (UnityEngine.Object) x == (UnityEngine.Object) this._current_sin_item)));
      this.SetParamIsOrganInserted(1f);
      this.Redraw();
      if (!BaseGUI.for_gamepad)
        return;
      this.gamepad_controller.Enable(GamepadNavigationController.OpenMethod.GetAll);
      this.gamepad_controller.SetFocusedItem(this._current_gui_item.gamepad_item);
    }
  }

  public void SetParamIsOrganInserted(float value)
  {
    this._wgo.SetParam($"is_{SinItem.GetOrganIdBySin(this._current_sin_item)}_inserted", value);
  }

  public void SetParamAccordingSinPart(SinItem sin_item, float value)
  {
    this._wgo.SetParam(sin_item.item_type.ToString().ToLower(), value);
  }

  public void FocusGamepadItem(GamepadNavigationItem gamepad_item)
  {
    this.gamepad_controller.Enable(GamepadNavigationController.OpenMethod.GetAll);
    this.gamepad_controller.SetFocusedItem(gamepad_item);
  }

  [CompilerGenerated]
  public bool \u003COnItemSinPress\u003Eb__16_0(SinItem x)
  {
    return (UnityEngine.Object) x == (UnityEngine.Object) this._current_sin_item;
  }

  [CompilerGenerated]
  public bool \u003COnItemSinForInsertionPicked\u003Eb__18_0(SinItem x)
  {
    return (UnityEngine.Object) x == (UnityEngine.Object) this._current_sin_item;
  }
}
