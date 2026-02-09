// Decompiled with JetBrains decompiler
// Type: SoulHealingWidget
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SoulHealingWidget : BaseGUI
{
  public System.Action ON_GUI_ITEM_CHANGE;
  public Action<GamepadNavigationItem> ON_FOCUS_GAMEPAD_ITEM;
  public const string INSERT_SOUL_BUTTON_LABEL = "insert_soul";
  public const string TAKE_OUT_SOUL_BUTTON_LABEL = "takeout_soul";
  public const int SOUL_ITEM_IN_INVENTORY_INDEX = 7;
  public const int SIN_SHARD_BONUS_MULTIPLIER = 1;
  [SerializeField]
  public BaseItemCellGUI _soul_item_gui;
  [SerializeField]
  public GameObject _heal_button_object;
  [SerializeField]
  public SoulExtractorPanelBarGUI _bar_gui;
  [Space]
  [SerializeField]
  public GamepadSelectableButton _heal_button_gamepad;
  public WorldGameObject _wgo;
  public int _ordinal_number;
  public List<SinItem> _sin_items;
  public float _sins_heal_rate;
  public bool is_heal_button_interactive;

  public Item inserted_item => this._soul_item_gui.item;

  public float sins_heal_rate
  {
    get => this._sins_heal_rate;
    set => this._sins_heal_rate = value;
  }

  public GamepadSelectableButton heal_button_gamepad => this._heal_button_gamepad;

  public new void Init()
  {
    this._heal_button_gamepad.Init();
    this._heal_button_gamepad.SetCallbacks(new GJCommons.VoidDelegate(this.OnStartHealButtonPressed), new GJCommons.VoidDelegate(this.OnHealButtonOver));
    this._soul_item_gui.SetCallbacks(new BaseItemCellGUI.OnItemAction(this.OnItemOver), (BaseItemCellGUI.OnItemAction) null, new BaseItemCellGUI.OnItemAction(this.OnItemPress));
    base.Init();
  }

  public void Draw(WorldGameObject container_obj, List<SinItem> sin_items)
  {
    this._wgo = container_obj;
    this._sin_items = sin_items;
    this._bar_gui.SetActive(false);
    this._soul_item_gui.InitInputBehaviour();
    this._soul_item_gui.InitTooltips();
    this._soul_item_gui.DrawItem(container_obj.data.GetItemByIndex(7));
    int num = this.IsSoulItemInserted() ? 1 : 0;
    this.UpdateHealButtonInteraction();
    if (num == 0)
      return;
    this._bar_gui.SetActive(true);
    this._bar_gui.SetData(this._soul_item_gui.item.durability, max_corruption_chance: this.sins_heal_rate.EqualsTo(-1f) ? 0.0f : (float) (0.5 * (1.0 - (double) this.sins_heal_rate)));
    this._bar_gui.Redraw();
  }

  public void OnStartHealButtonPressed()
  {
    CraftDefinition dataOrNull = GameBalance.me.GetDataOrNull<CraftDefinition>($"{this._wgo.obj_id}:{this.inserted_item.id}");
    if (dataOrNull == null)
      return;
    GUIElements.me.soul_healer_gui.Hide(true);
    Item itemOfType = this._wgo.GetItemOfType(ItemDefinition.ItemType.Soul);
    this._wgo.data.RemoveItem(itemOfType);
    Item obj1 = new Item("soul_healed", 1)
    {
      durability = itemOfType.durability - (this.sins_heal_rate.EqualsTo(-1f) ? 0.0f : (float) (0.5 * (1.0 - (double) this.sins_heal_rate)))
    };
    double num1 = (double) obj1.GetParam("durability");
    double num2 = 1.0 / ((double) this._wgo.GetParam("sin_drop_max_count") + 1.0);
    int paramInt = itemOfType.GetParamInt("sins_count");
    int num3 = num1 > 0.0 ? paramInt : 0;
    Item obj2 = new Item("sin_shard", num3 + MainGame.me.player.GetParamInt("increase_sin_shard_drop") * num3);
    obj1.AddToParams("sins_count", (float) paramInt);
    dataOrNull.output = new List<Item>() { obj1, obj2 };
    foreach (Item obj3 in dataOrNull.output)
    {
      obj3.min_value = new SmartExpression();
      obj3.max_value = new SmartExpression();
      obj3.self_chance = new SmartExpression()
      {
        default_value = 1f
      };
    }
    this._wgo.components.craft.Craft(dataOrNull);
  }

  public static InventoryWidget.ItemFilterResult SoulItemsFilter(Item item)
  {
    if (item == null || item.IsEmpty())
      return InventoryWidget.ItemFilterResult.Hide;
    return item.definition.type != ItemDefinition.ItemType.Soul || item.id == "soul_healed" ? InventoryWidget.ItemFilterResult.Inactive : InventoryWidget.ItemFilterResult.Active;
  }

  public void OnItemPicked(Item item)
  {
    if (item == null || item.IsEmpty())
    {
      if (!BaseGUI.for_gamepad)
        return;
      Action<GamepadNavigationItem> focusGamepadItem = this.ON_FOCUS_GAMEPAD_ITEM;
      if (focusGamepadItem == null)
        return;
      focusGamepadItem(this._soul_item_gui.gamepad_item);
    }
    else
    {
      this._wgo.data.AddItemByIndex(item, 7);
      WorldZone myWorldZone = this._wgo.GetMyWorldZone();
      WorldGameObject worldGameObject = this._wgo;
      if ((UnityEngine.Object) myWorldZone != (UnityEngine.Object) null && myWorldZone.IsPlayerInZone())
        worldGameObject = MainGame.me.player;
      worldGameObject.GetMultiInventory(new List<WorldGameObject>()
      {
        this._wgo
      }, sortWGOS: true, include_bags: true).RemoveItem(item);
      this.Redraw();
      if (BaseGUI.for_gamepad)
      {
        Action<GamepadNavigationItem> focusGamepadItem = this.ON_FOCUS_GAMEPAD_ITEM;
        if (focusGamepadItem != null)
          focusGamepadItem(this._heal_button_gamepad.navigation_item);
      }
      System.Action onGuiItemChange = this.ON_GUI_ITEM_CHANGE;
      if (onGuiItemChange == null)
        return;
      onGuiItemChange();
    }
  }

  public void Redraw() => this.Draw(this._wgo, this._sin_items);

  public void UpdateHealButtonInteraction()
  {
    bool flag = this.IsSoulItemInserted();
    for (int index = 0; index < this._sin_items.Count; ++index)
    {
      if (flag)
      {
        if (SinItem.GetSinItemByTypeFromItem(this._sin_items[index].item_type, this._soul_item_gui.item) != null)
        {
          if (this._sin_items[index].item_cell_gui.item == null)
          {
            this.SetHealButtonInteractionState(false);
            return;
          }
          if (this._sin_items[index].item_cell_gui.item.IsEmpty())
          {
            this.SetHealButtonInteractionState(false);
            return;
          }
        }
      }
      else
      {
        this.SetHealButtonInteractionState(false);
        return;
      }
    }
    this.SetHealButtonInteractionState(true);
  }

  public void ClearSoulItemInGUI() => this._soul_item_gui.DrawEmpty();

  public void SetHealButtonInteractionState(bool is_active)
  {
    foreach (UIButtonColor componentsInChild in this._heal_button_object.GetComponentsInChildren<UIButton>(true))
    {
      componentsInChild.isEnabled = is_active;
      this.is_heal_button_interactive = is_active;
    }
  }

  public void OnItemOver(BaseItemCellGUI item_gui)
  {
    if (item_gui.item == null || item_gui.item.IsEmpty())
    {
      if (!BaseGUI.for_gamepad)
        return;
      this.button_tips.Print(GameKeyTip.Select("select"), GameKeyTip.Close());
    }
    else
    {
      if (!BaseGUI.for_gamepad)
        return;
      this.button_tips.Print(GameKeyTip.Select("take"), GameKeyTip.Close());
    }
  }

  public void OnItemPress(BaseItemCellGUI item_gui)
  {
    if (this._soul_item_gui.item != null && !this._soul_item_gui.item.IsEmpty())
    {
      WorldZone myWorldZone = this._wgo.GetMyWorldZone();
      if ((UnityEngine.Object) myWorldZone != (UnityEngine.Object) null && !myWorldZone.IsPlayerInZone())
      {
        MultiInventory multiInventory = this._wgo.GetMultiInventory();
        if (multiInventory != null && multiInventory.CanAddItem(this._soul_item_gui.item))
          multiInventory.AddItem(this._soul_item_gui.item);
        else
          this._wgo.DropItem(this._soul_item_gui.item);
      }
      else if (MainGame.me.player.data.CanAddItem(this._soul_item_gui.item))
        MainGame.me.player.data.AddItem(this._soul_item_gui.item);
      else
        this._wgo.DropItem(this._soul_item_gui.item, Direction.ToPlayer);
      this._soul_item_gui.DrawEmpty();
      this._wgo.data.RemoveItemByIndex(7);
      this.Draw(this._wgo, this._sin_items);
      if (BaseGUI.for_gamepad)
      {
        Action<GamepadNavigationItem> focusGamepadItem = this.ON_FOCUS_GAMEPAD_ITEM;
        if (focusGamepadItem != null)
          focusGamepadItem(item_gui.gamepad_item);
      }
      System.Action onGuiItemChange = this.ON_GUI_ITEM_CHANGE;
      if (onGuiItemChange == null)
        return;
      onGuiItemChange();
    }
    else
    {
      WorldGameObject worldGameObject = MainGame.me.player;
      if (GlobalCraftControlGUI.is_global_control_active && (UnityEngine.Object) this._wgo != (UnityEngine.Object) null)
        worldGameObject = this._wgo;
      GUIElements.me.resource_picker.Open(worldGameObject, (InventoryWidget.ItemFilterDelegate) ((itm, widget) => SoulHealingWidget.SoulItemsFilter(itm)), new CraftResourcesSelectGUI.ResourceSelectResultDelegate(this.OnItemPicked));
    }
  }

  public bool IsSoulItemInserted()
  {
    return this._soul_item_gui.item != null && !this._soul_item_gui.item.IsEmpty() && this._soul_item_gui.item.definition.type == ItemDefinition.ItemType.Soul;
  }

  public void OnHealButtonOver()
  {
    if (!BaseGUI.for_gamepad)
      return;
    this.button_tips.Print(GameKeyTip.Select(this.is_heal_button_interactive), GameKeyTip.Close());
  }
}
