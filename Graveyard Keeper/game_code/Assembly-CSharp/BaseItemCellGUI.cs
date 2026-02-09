// Decompiled with JetBrains decompiler
// Type: BaseItemCellGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using LinqTools;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class BaseItemCellGUI : MonoBehaviour
{
  public GJCommons.VoidDelegate _on_over_void;
  public GJCommons.VoidDelegate _on_out_void;
  public GJCommons.VoidDelegate _on_select_void;
  public BaseItemCellGUI.OnItemAction _on_over;
  public BaseItemCellGUI.OnItemAction _on_out;
  public BaseItemCellGUI.OnItemAction _on_action;
  public BaseItemCellElements x1;
  public BaseItemCellElements x2;
  public ItemCellColors colors;
  public UILabel item_name;
  public UILabel item_description;
  public UILabel price;
  public UI2DSprite additional_icon;
  public UI2DSprite progress;
  public UI2DSprite quality_icon;
  [HideInInspector]
  [NonSerialized]
  public BaseItemCellElements container;
  [HideInInspector]
  public GamepadNavigationItem gamepad_item;
  public ItemDefinition _item_definition;
  public bool _interaction_enabled = true;
  public bool _is_inactive_state;
  public Item _item;
  public string _multiquality_id = "";
  public bool _mouse_overed;
  public UIEventTrigger additional_button;
  public UI2DSprite radial_dim;
  public float _update_period;
  public Item _last_drawitem_item;
  public bool _last_drawitem_tooltip = true;
  public string _last_drawitem_multiquality = "";

  public UIWidget widget => this.GetComponent<UIWidget>();

  public string item_id
  {
    get
    {
      if (this._item == null)
        return "";
      return !string.IsNullOrEmpty(this._multiquality_id) ? this._multiquality_id : this._item.id;
    }
  }

  public Item item => this._item;

  public bool is_inactive_state => this._is_inactive_state;

  public bool id_empty => this._item == null || this._item.IsEmpty();

  public bool interaction_enabled
  {
    get => this._interaction_enabled;
    set
    {
      this._interaction_enabled = value;
      if (this.container == null)
        this.container = this.x1;
      this.container.collider.enabled = value;
    }
  }

  public void DrawEmpty() => this.DrawItem(Item.empty);

  public void DrawItem(
    Item i,
    bool init_tooltip = true,
    string multiquality_id = "",
    bool try_optimize_redraw = false,
    bool infinity_counter = false)
  {
    if (i == null)
      i = Item.empty;
    if (try_optimize_redraw && this._last_drawitem_item != null && i?.id == this._last_drawitem_item?.id)
    {
      int? nullable1 = i?.value;
      int? nullable2 = this._last_drawitem_item?.value;
      if (nullable1.GetValueOrDefault() == nullable2.GetValueOrDefault() & nullable1.HasValue == nullable2.HasValue && this._last_drawitem_multiquality == multiquality_id && this._last_drawitem_tooltip == init_tooltip)
        return;
    }
    this._last_drawitem_item = i;
    this._last_drawitem_multiquality = multiquality_id;
    this._last_drawitem_tooltip = init_tooltip;
    this._item = i;
    if ((UnityEngine.Object) this.radial_dim != (UnityEngine.Object) null)
      this.radial_dim.gameObject.SetActive(false);
    this._multiquality_id = this._item.is_multiquality ? (string.IsNullOrEmpty(multiquality_id) ? this._item.multiquality_items[0] : multiquality_id) : "";
    string str = this.item_id;
    if (str == "" || str == null)
      str = "empty";
    int num = this._item.value;
    this.gameObject.SetActive(true);
    this._item_definition = (ItemDefinition) null;
    if (str != "empty" && !TechDefinition.TECH_POINTS.Contains(str))
    {
      this._item_definition = GameBalance.me.GetDataOrNull<ItemDefinition>(str);
      if (this._item_definition == null)
      {
        List<string> itemsOfBaseName = GameBalance.me.GetItemsOfBaseName(str);
        if (itemsOfBaseName.Count > 0)
          this._item_definition = GameBalance.me.GetData<ItemDefinition>(itemsOfBaseName[0]);
      }
    }
    bool flag1 = this._item_definition == null || this._item_definition.is_small;
    bool flag2 = str == "empty" || this._item_definition == null;
    if ((UnityEngine.Object) this.x1.container == (UnityEngine.Object) null)
    {
      Debug.LogException(new Exception("No container for item " + this.name), (UnityEngine.Object) this);
    }
    else
    {
      this.x1.container.SetActive(flag1);
      if (this.x2 != null && (UnityEngine.Object) this.x2.container != (UnityEngine.Object) null)
        this.x2.container.SetActive(!flag1);
      this.container = flag1 ? this.x1 : this.x2;
      if (this.container == null && str == "body")
      {
        this.container = this.x1;
        this.x1.container.SetActive(true);
      }
      this.container.icon.sprite2D = !flag2 ? EasySpritesCollection.GetSprite(this._item_definition.GetIcon()) : (UnityEngine.Sprite) null;
      if ((UnityEngine.Object) this.container.empty_item_gfx != (UnityEngine.Object) null)
        this.container.empty_item_gfx.SetActive(flag2);
      this.container.icon.color = this._item.durability_state == Item.DurabilityState.Broken ? this.colors.broken : this.colors.normal;
      if ((UnityEngine.Object) this.container.collider != (UnityEngine.Object) null)
        this.container.collider.enabled = this.interaction_enabled;
      if ((UnityEngine.Object) this.item_name != (UnityEngine.Object) null)
        this.item_name.text = flag2 ? "" : this._item_definition.GetItemName();
      if ((UnityEngine.Object) this.item_description != (UnityEngine.Object) null)
        this.item_description.text = flag2 ? "" : this._item_definition.GetItemDescription();
      if ((bool) (UnityEngine.Object) this.container.counter)
        this.container.counter.text = !infinity_counter ? (num <= 1 | flag2 ? "" : num.ToString()) : "∞";
      if ((UnityEngine.Object) this.price != (UnityEngine.Object) null)
        this.price.text = "";
      if ((UnityEngine.Object) this.container.tooltip != (UnityEngine.Object) null)
        this.container.tooltip.SetData(!init_tooltip || flag2 ? (List<BubbleWidgetData>) null : this._item_definition.GetTooltipData(i));
      this.container.selection.Deactivate<UI2DSprite>();
      this.container.gamepad_frame.Deactivate<UI2DSprite>();
      if ((UnityEngine.Object) this.progress != (UnityEngine.Object) null)
      {
        if (!flag2 && this._item?.definition != null && this._item.definition.has_durability)
        {
          if (this._item.durability_state == Item.DurabilityState.Full)
          {
            this.progress.Deactivate<UI2DSprite>();
          }
          else
          {
            this.progress.Activate<UI2DSprite>();
            this.progress.fillAmount = this._item.durability;
          }
        }
        else
          this.progress.Deactivate<UI2DSprite>();
      }
      if ((UnityEngine.Object) this.additional_icon != (UnityEngine.Object) null)
        this.additional_icon.Deactivate<UI2DSprite>();
      this.DrawQualityIcon(false);
      this.RedrawCooldown();
      this._update_period = 0.0f;
      this.DrawCapIcon(false);
      this.DrawGratitudeIcon(false);
    }
  }

  public void DrawEquippedIcons()
  {
    if ((UnityEngine.Object) this.additional_icon == (UnityEngine.Object) null)
      return;
    if ((this.item == null || this.item.IsEmpty() ? (ItemDefinition) null : this.item.definition) == null)
    {
      this.additional_icon.sprite2D = (UnityEngine.Sprite) null;
    }
    else
    {
      string sprite_name = "";
      if (this._item.equipped_as == ItemDefinition.EquipmentType.None)
      {
        if (this._item.definition.equipment_type == ItemDefinition.EquipmentType.None)
          ;
      }
      else
        sprite_name = $"icon_{this._item.equipped_as.ToString().ToLower()}_equipped";
      this.additional_icon.sprite2D = sprite_name == "" ? (UnityEngine.Sprite) null : EasySpritesCollection.GetSprite(sprite_name);
    }
    this.additional_icon.SetActive((UnityEngine.Object) this.additional_icon.sprite2D != (UnityEngine.Object) null);
  }

  public void DrawQualityIcon(bool force_disable)
  {
    if ((UnityEngine.Object) this.quality_icon == (UnityEngine.Object) null)
      return;
    if (force_disable || this._item_definition == null || this._item_definition.quality_type == ItemDefinition.QualityType.Default)
    {
      this.quality_icon.sprite2D = (UnityEngine.Sprite) null;
      this.quality_icon.gameObject.SetActive(false);
    }
    else
      this._item_definition.TryDrawQualityOrDisableGameObject(this.quality_icon);
  }

  public void DrawItem(
    string id,
    int value,
    bool init_tooltip = true,
    bool try_optimize_redraw = false,
    bool infinity_counter = false)
  {
    this.DrawItem(new Item(id, value), init_tooltip, try_optimize_redraw: try_optimize_redraw, infinity_counter: infinity_counter);
  }

  public void RedrawItem() => this.DrawItem(this._item);

  public void DrawIngredient(
    Item item,
    MultiInventory multi_inventory,
    bool deactivate_colliders = false,
    bool init_tooltip = false,
    string multiquality_id = "",
    Item additional_inventory = null,
    List<Item> used_items = null,
    bool item_is_a_group_of_multiquality = false)
  {
    this.gameObject.SetActive(item != null);
    if (item == null)
      return;
    if (additional_inventory != null && multi_inventory != null)
    {
      foreach (Inventory inventory in multi_inventory.all)
      {
        if (inventory != null && inventory.data == additional_inventory)
        {
          additional_inventory = (Item) null;
          Debug.Log((object) "Additional inventory ignored: duplicate in multi_inventory");
          break;
        }
      }
    }
    this.DrawItem(item, init_tooltip, multiquality_id);
    if (deactivate_colliders)
      this.container.collider.enabled = false;
    if (item.value == 0)
    {
      this.container.counter.text = "";
    }
    else
    {
      int num = 0;
      if (item.id == "gratitude_as_item")
      {
        num = (int) MainGame.me.player.gratitude_points;
      }
      else
      {
        if (item_is_a_group_of_multiquality)
        {
          foreach (string multiqualityItem in item.multiquality_items)
          {
            if (multi_inventory != null)
              num += multi_inventory.GetTotalCount(multiqualityItem);
            if (additional_inventory != null)
              num += additional_inventory.GetItemsCount(multiqualityItem);
          }
        }
        else
        {
          if (multi_inventory != null)
            num += multi_inventory.GetTotalCount(this.item_id);
          if (additional_inventory != null)
            num += additional_inventory.GetItemsCount(this.item_id);
        }
        if (used_items != null)
        {
          foreach (Item usedItem in used_items)
          {
            if (usedItem.id == this.item_id)
              num -= usedItem.value;
          }
          if (num < 0)
            num = 0;
        }
      }
      this.container.counter.text = $"{num.ToString()}/{item.value.ToString()}";
      this.container.counter.color = num >= item.value ? this.colors.enough_res : this.colors.not_enough_res;
    }
    if (!((UnityEngine.Object) this.item_name != (UnityEngine.Object) null))
      return;
    this.item_name.text = item.GetItemName();
  }

  public void DrawIcon(string icon, bool draw_back = true, bool hide_quality_icon = true)
  {
    if (this.x1 != null && (UnityEngine.Object) this.x1.container != (UnityEngine.Object) null)
      this.x1.container.SetActive(true);
    if (this.x2 != null && (UnityEngine.Object) this.x2.container != (UnityEngine.Object) null)
      this.x2.container.SetActive(false);
    this.container = this.x1;
    if (this.container == null)
    {
      Debug.LogError((object) "Container is null", (UnityEngine.Object) this);
    }
    else
    {
      this.DrawCapIcon(false);
      this.DrawGratitudeIcon(false);
      this.container.icon.sprite2D = EasySpritesCollection.GetSprite(icon);
      if ((UnityEngine.Object) this.container.empty_item_gfx != (UnityEngine.Object) null)
        this.container.empty_item_gfx.SetActive(string.IsNullOrEmpty(icon));
      if ((UnityEngine.Object) this.container.gamepad_frame != (UnityEngine.Object) null)
        this.container.gamepad_frame.gameObject.SetActive(false);
      if ((UnityEngine.Object) this.container.back != (UnityEngine.Object) null)
        this.container.back.gameObject.SetActive(draw_back);
      if ((UnityEngine.Object) this.price != (UnityEngine.Object) null)
        this.price.text = "";
      if ((UnityEngine.Object) this.additional_icon != (UnityEngine.Object) null & hide_quality_icon)
        this.additional_icon.gameObject.SetActive(false);
      if (this.container == null || !((UnityEngine.Object) this.container.counter != (UnityEngine.Object) null))
        return;
      this.container.counter.text = "";
    }
  }

  public void HideBack() => this.container.back.gameObject.SetActive(false);

  public void ResizeIconByContent() => this.container.icon.ResizeByContent();

  public void InitInputBehaviour(int navigation_group = 0, int navigation_sub_group = 0)
  {
    if ((UnityEngine.Object) this.gamepad_item == (UnityEngine.Object) null)
      this.gamepad_item = this.GetComponent<GamepadNavigationItem>();
    if ((UnityEngine.Object) this.gamepad_item == (UnityEngine.Object) null)
      return;
    this.gamepad_item.group = navigation_group;
    this.gamepad_item.sub_group = navigation_sub_group;
    this.gamepad_item.active = BaseGUI.for_gamepad;
    this._mouse_overed = false;
    if (!BaseGUI.for_gamepad)
      return;
    this.gamepad_item.SetCallbacks((GJCommons.VoidDelegate) (() => this.OnOver(true)), (GJCommons.VoidDelegate) (() => this.OnOut(true)), (GJCommons.VoidDelegate) (() => this.OnPressed(true)));
  }

  public void InitTooltips()
  {
    if ((UnityEngine.Object) this.x1.tooltip != (UnityEngine.Object) null)
      this.x1.tooltip.Init();
    if (!((UnityEngine.Object) this.x2.tooltip != (UnityEngine.Object) null))
      return;
    this.x2.tooltip.Init();
  }

  public void OnMouseOvered()
  {
    if (this._mouse_overed)
      return;
    this._mouse_overed = true;
    if (BaseGUI.for_gamepad)
      return;
    this.OnOver(false);
  }

  public void OnMouseOut()
  {
    this._mouse_overed = false;
    if (BaseGUI.for_gamepad)
      return;
    this.OnOut(false);
  }

  public void ForceMouseOut() => this._mouse_overed = false;

  public void OnMousePress()
  {
    if (BaseGUI.for_gamepad)
      return;
    this.OnPressed(false);
  }

  public void OnOver(bool by_gamepad)
  {
    if (!this._interaction_enabled)
      return;
    if (GUIElements.me.context_menu_bubble.is_shown)
      return;
    try
    {
      if ((UnityEngine.Object) this.gameObject == (UnityEngine.Object) null)
        return;
    }
    catch (MissingReferenceException ex)
    {
      return;
    }
    this.SetVisualyOveredState(true, by_gamepad);
    if (this._on_over != null)
      this._on_over(this);
    this._on_over_void.TryInvoke();
    try
    {
      if (by_gamepad == BaseGUI.for_gamepad)
      {
        if (this.container != null)
        {
          if ((by_gamepad ? (Component) this.container.gamepad_frame : (Component) this.container.selection).gameObject.activeSelf)
            Sounds.OnGUIHover(Sounds.ElementType.ItemCell);
        }
      }
    }
    catch (MissingReferenceException ex)
    {
      Debug.LogError((object) ("Missing ref: " + ex?.ToString()));
    }
    TooltipsManager.Redraw();
  }

  public void OnOut(bool by_gamepad)
  {
    if (!this._interaction_enabled)
      return;
    this.SetVisualyOveredState(false, by_gamepad);
    if (this._on_out != null)
      this._on_out(this);
    this._on_out_void.TryInvoke();
  }

  public void OnPressed(bool by_gamepad)
  {
    if (!this._interaction_enabled || this._is_inactive_state)
      return;
    this._on_select_void.TryInvoke();
    if (this._on_action != null)
      this._on_action(this);
    if (Sounds.WasAnySoundPlayedThisFrame() || !(by_gamepad ? (Component) this.container.gamepad_frame : (Component) this.container.selection).gameObject.activeSelf)
      return;
    Sounds.OnGUIClick();
  }

  public void SetVisualyOveredState(bool overed, bool by_gamepad)
  {
    if (this.container == null || (UnityEngine.Object) this.container.icon == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "icon is null", (UnityEngine.Object) this);
    }
    else
    {
      this.container.icon.ChangeColor(this._item == null || this.item.durability_state != Item.DurabilityState.Broken ? (overed ? (by_gamepad ? this.colors.gamepad_overed : this.colors.mouse_overed) : this.colors.normal) : this.colors.broken, 0.0f, ignore_alpha: true);
      (by_gamepad ? (MonoBehaviour) this.container.gamepad_frame : (MonoBehaviour) this.container.selection).SetActive(overed);
    }
  }

  public void SetGrayState(bool set_inactive = true) => this.SetInactiveState(set_inactive);

  public void SetInactiveState(bool set_inactive = true)
  {
    this._is_inactive_state = set_inactive;
    this.widget.alpha = this._is_inactive_state ? this.colors.inactive.a : 1f;
  }

  public void DrawUnknown() => this.DrawItem(new Item("unknown", 1));

  public void SetCallbacks(
    GJCommons.VoidDelegate on_over,
    GJCommons.VoidDelegate on_out,
    GJCommons.VoidDelegate on_action)
  {
    this._on_over_void = on_over;
    this._on_out_void = on_out;
    this._on_select_void = on_action;
  }

  public void SetCallbacks(
    BaseItemCellGUI.OnItemAction on_over,
    BaseItemCellGUI.OnItemAction on_out,
    BaseItemCellGUI.OnItemAction on_action)
  {
    this._on_over = on_over;
    this._on_out = on_out;
    this._on_action = on_action;
  }

  public void SetItemPrice(float price)
  {
    if ((UnityEngine.Object) this.price == (UnityEngine.Object) null)
      Debug.LogError((object) "Item has no price field", (UnityEngine.Object) this);
    else
      this.price.text = Trading.FormatMoney(price);
  }

  public void ClearPrice()
  {
    if ((UnityEngine.Object) this.price == (UnityEngine.Object) null)
      Debug.LogError((object) "Item has no price field", (UnityEngine.Object) this);
    else
      this.price.text = "";
  }

  public static void DrawIngredients(
    BaseItemCellGUI[] ingredients,
    List<Item> items,
    MultiInventory multi_inventory,
    List<string> multiquality_ids = null,
    int amount = 1)
  {
    List<Item> used_items = new List<Item>();
    for (int index = 0; index < ingredients.Length; ++index)
    {
      Item obj = index >= items.Count ? (Item) null : items[index];
      ingredients[index].gameObject.SetActive(obj != null);
      if (obj != null)
      {
        if (amount > 1)
        {
          List<string> multiqualityItems = obj.multiquality_items;
          obj = new Item(obj.id, obj.value * amount)
          {
            multiquality_items = multiqualityItems
          };
        }
        string item_id = obj.id;
        // ISSUE: explicit non-virtual call
        if ((multiquality_ids == null || __nonvirtual (multiquality_ids[index]) == null) && obj.multiquality_items.Count > 1)
        {
          ingredients[index].DrawIngredient(obj, multi_inventory, init_tooltip: true, multiquality_id: obj.multiquality_items.FirstOrDefault<string>(), used_items: used_items, item_is_a_group_of_multiquality: true);
          ingredients[index].quality_icon.sprite2D = (UnityEngine.Sprite) null;
        }
        else
          ingredients[index].DrawIngredient(obj, multi_inventory, init_tooltip: true, multiquality_id: multiquality_ids?[index], used_items: used_items);
        if (!string.IsNullOrEmpty(ingredients[index]._multiquality_id))
          item_id = ingredients[index]._multiquality_id;
        used_items.Add(new Item(item_id, obj.value * amount));
      }
    }
  }

  public void RedrawCooldown()
  {
    if (this._item == null || this._item_definition == null || !this._item_definition.can_be_used || !this._item_definition.cooldown.has_expression || !((UnityEngine.Object) this.radial_dim != (UnityEngine.Object) null))
      return;
    this.radial_dim.gameObject.SetActive(true);
    this.radial_dim.fillAmount = (float) this._item.GetGrayedCooldownPercent() / 100f;
  }

  public void DrawGratitudeIcon(bool vis, bool enough = true)
  {
    if (this.container == null || !((UnityEngine.Object) this.container.gratitude_craft_label != (UnityEngine.Object) null))
      return;
    this.container.gratitude_craft_label.gameObject.SetActive(vis);
    this.container.gratitude_craft_label.sprite2D = EasySpritesCollection.GetSprite(enough ? "techpoint_drop_smile" : "techpoint_drop_smile_not_enough");
  }

  public void DrawCapIcon(bool vis)
  {
    if (!((UnityEngine.Object) this.container.icon_cap_limit != (UnityEngine.Object) null))
      return;
    this.container.icon_cap_limit.gameObject.SetActive(vis);
  }

  [CompilerGenerated]
  public void \u003CInitInputBehaviour\u003Eb__53_0() => this.OnOver(true);

  [CompilerGenerated]
  public void \u003CInitInputBehaviour\u003Eb__53_1() => this.OnOut(true);

  [CompilerGenerated]
  public void \u003CInitInputBehaviour\u003Eb__53_2() => this.OnPressed(true);

  public delegate void OnItemAction(BaseItemCellGUI item);
}
