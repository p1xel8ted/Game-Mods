// Decompiled with JetBrains decompiler
// Type: InventoryWidget
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class InventoryWidget : BaseInventoryWidget
{
  public Item inventory_item;
  public UIGrid items_table;
  public BaseItemCellGUI item_prefab;
  public List<BaseItemCellGUI> items = new List<BaseItemCellGUI>();
  public InventoryWidget.ItemDelegate on_over;
  public InventoryWidget.ItemDelegate on_out;
  public InventoryWidget.ItemDelegate on_press;
  public bool auto_height = true;
  public bool auto_width;
  public bool dont_show_empty_rows;
  public bool custom_size;
  public PanelAutoScroll header_auto_scroll;
  public PanelAutoScroll bottom_auto_scroll;
  public bool _interaction_enabled = true;
  public const int HEADER_HEIGHT = 46;
  public bool _moved_contents;
  public int _default_table_y;
  public int _default_back_anchor_y;
  public UIWidget inventory_lock_widget;
  public UIWidget inventory_lock_progressbar_w;
  public UIProgressBar lock_progress_bar;
  public UILabel lock_descr_text;
  public UILabel lock_tier_1;
  public UILabel lock_tier_2;
  public bool can_show_2h_items;
  public bool sort = true;
  public bool show_empty_as_invisible;
  public int auto_height_offset;
  public int auto_width_offset;

  public bool interaction_enabled
  {
    get => this._interaction_enabled;
    set
    {
      this._interaction_enabled = value;
      foreach (BaseItemCellGUI baseItemCellGui in this.items)
        baseItemCellGui.interaction_enabled = this._interaction_enabled;
    }
  }

  public override void Init()
  {
    if (this.initialized)
      return;
    this.item_prefab = this.GetComponentInChildren<BaseItemCellGUI>(true);
    this.items.AddRange((IEnumerable<BaseItemCellGUI>) this.GetComponentsInChildren<BaseItemCellGUI>(true));
    this.custom_size = this.items.Count > 1;
    if (!this.custom_size)
    {
      this.items.Clear();
      this.item_prefab.Deactivate<BaseItemCellGUI>();
    }
    this.FindItemsTable();
    if ((Object) this.header_label != (Object) null)
      this.header_auto_scroll = this.header_label.transform.parent.GetComponentInChildren<PanelAutoScroll>();
    Transform transform = this.transform.Find("background");
    this.bottom_auto_scroll = (Object) transform != (Object) null ? transform.GetComponentInChildren<PanelAutoScroll>() : this.GetComponentInChildren<PanelAutoScroll>();
    this.Hide();
    this.initialized = true;
    if (!((Object) this.inventory_lock_widget != (Object) null))
      return;
    this.inventory_lock_widget.gameObject.SetActive(false);
  }

  public virtual void Open(
    Inventory inventory,
    bool for_gamepad,
    int navigation_group = 0,
    int navigation_sub_group = 0,
    bool dont_show_empty_rows = false,
    int custom_line_length = -1)
  {
    if (!this.initialized)
      this.Init();
    this.gameObject.SetActive(true);
    if (inventory == null)
      inventory = new Inventory(MainGame.me.player);
    this.Open(inventory, for_gamepad, BaseInventoryWidget.InventoryType.None);
    this.inventory_item = inventory.data;
    this.dont_show_empty_rows = dont_show_empty_rows;
    if (custom_line_length > 0)
    {
      if ((Object) this.items_table == (Object) null)
        this.FindItemsTable();
      this.items_table.maxPerLine = custom_line_length;
      this.auto_width = true;
    }
    if (!this.custom_size)
    {
      int size = inventory.size;
      while (size-- > 0)
      {
        BaseItemCellGUI baseItemCellGui = this.item_prefab.Copy<BaseItemCellGUI>();
        baseItemCellGui.interaction_enabled = this.interaction_enabled;
        this.items.Add(baseItemCellGui);
      }
    }
    foreach (BaseItemCellGUI baseItemCellGui in this.items)
    {
      baseItemCellGui.SetCallbacks(new BaseItemCellGUI.OnItemAction(this.OnItemOver), new BaseItemCellGUI.OnItemAction(this.OnItemOut), new BaseItemCellGUI.OnItemAction(this.OnItemPress));
      baseItemCellGui.InitInputBehaviour(navigation_group, navigation_sub_group);
      baseItemCellGui.InitTooltips();
    }
    this.Redraw();
  }

  public void UpdateItemsCallbacksAndStuff(int navigation_group = 0, int navigation_sub_group = 0)
  {
    foreach (BaseItemCellGUI baseItemCellGui in this.items)
    {
      baseItemCellGui.SetCallbacks(new BaseItemCellGUI.OnItemAction(this.OnItemOver), new BaseItemCellGUI.OnItemAction(this.OnItemOut), new BaseItemCellGUI.OnItemAction(this.OnItemPress));
      baseItemCellGui.InitInputBehaviour(navigation_group, navigation_sub_group);
      baseItemCellGui.InitTooltips();
    }
  }

  public override void Redraw()
  {
    if ((Object) this.inventory_lock_widget != (Object) null)
    {
      this.inventory_lock_widget.gameObject.SetActive(this.inventory.is_locked);
      this.lock_progress_bar.gameObject.SetActive(this.inventory.vendor_tier_info != null);
      this.lock_descr_text.gameObject.SetActive(this.inventory.vendor_tier_info != null);
      if (this.inventory.vendor_tier_info == null)
      {
        this.lock_tier_1.text = this.lock_tier_2.text = "";
      }
      else
      {
        this.inventory_lock_progressbar_w.gameObject.SetActive(this.inventory.vendor_tier_info.progressbar_visible);
        this.lock_progress_bar.value = this.inventory.vendor_tier_info.progress;
        this.lock_tier_1.text = $"(tr{this.inventory.vendor_tier_info.tier_1.ToString()})";
        this.lock_tier_2.text = $"(tr{this.inventory.vendor_tier_info.tier_2.ToString()})";
        this.lock_descr_text.text = GJL.L((double) this.lock_progress_bar.value >= 1.0 ? "vendor_tier_lock_next" : "vendor_tier_lock_info", GJCommons.GetRomeNumber(this.inventory.vendor_tier_info.tier_2));
        this.inventory_lock_widget.GetComponentInChildren<UITableOrGrid>().Reposition();
      }
    }
    if (this.inventory_item == null)
      return;
    if (this.sort)
      this.inventory_item.Sort();
    int index1 = 0;
    foreach (Item i in this.inventory_item.inventory)
    {
      ItemDefinition definition = i.definition;
      if ((definition == null || !definition.is_big || this.can_show_2h_items) && index1 < this.items.Count)
      {
        this.items[index1].DrawItem(i);
        if (this.show_empty_as_invisible)
          this.items[index1].container.container.gameObject.SetActive(!i.IsEmpty());
        ++index1;
      }
    }
    for (int index2 = index1; index2 < this.items.Count; ++index2)
      this.items[index2].DrawItem(Item.empty);
    if (this.custom_size)
      return;
    this.RecalculateWidgetSize();
  }

  public void DrawEquippedIcons()
  {
    foreach (BaseItemCellGUI baseItemCellGui in this.items)
      baseItemCellGui.DrawEquippedIcons();
  }

  [ContextMenu("Recalculate widget size")]
  public void RecalculateWidgetSize()
  {
    if ((Object) this.items_table == (Object) null)
      this.FindItemsTable();
    int num1 = 0;
    int num2 = 0;
    int num3 = 0;
    foreach (BaseItemCellGUI baseItemCellGui in this.items)
    {
      if (baseItemCellGui.gameObject.activeSelf)
      {
        ++num1;
        if (!baseItemCellGui.id_empty)
          num2 += baseItemCellGui.item.definition.item_size;
      }
      else if (!baseItemCellGui.id_empty)
        ++num3;
    }
    int num4 = Mathf.CeilToInt((float) num1 / (float) this.items_table.maxPerLine);
    if (this.dont_show_empty_rows)
    {
      num4 = Mathf.CeilToInt((float) num2 / (float) this.items_table.maxPerLine);
      if (num4 == 0)
        num4 = 1;
      int num5 = num4 * this.items_table.maxPerLine + num3;
      for (int index = 0; index < this.items.Count; ++index)
      {
        if (index < num5)
        {
          if (this.items[index].id_empty)
            this.items[index].Activate<BaseItemCellGUI>();
        }
        else
          this.items[index].Deactivate<BaseItemCellGUI>();
      }
    }
    this.items_table.Reposition();
    if (!this.auto_height)
      return;
    int num6 = 0;
    UIWidget component = this.GetComponent<UIWidget>();
    component.height = 29 + num4 * 42 - num6 + this.auto_height_offset;
    if (!this.auto_width)
      return;
    component.width = 10 + this.items_table.maxPerLine * 42 + this.auto_width_offset;
    foreach (UIRect componentsInChild in this.GetComponentsInChildren<UIWidget>())
      componentsInChild.ResetAndUpdateAnchors();
  }

  public void FindItemsTable()
  {
    foreach (UIGrid componentsInChild in this.GetComponentsInChildren<UIGrid>(true))
    {
      if ((Object) componentsInChild != (Object) null)
        this.items_table = componentsInChild;
    }
    if (!((Object) this.items_table == (Object) null))
      return;
    Debug.LogError((object) "Couldn't find items table (UIGrid)", (Object) this);
  }

  public BaseItemCellGUI ItemAt(int index)
  {
    return index > this.items.Count ? (BaseItemCellGUI) null : this.items[index];
  }

  public void FilterItems(InventoryWidget.ItemFilterDelegate filter_delegate)
  {
    foreach (BaseItemCellGUI behaviour in this.items)
    {
      switch (filter_delegate(behaviour.item, this))
      {
        case InventoryWidget.ItemFilterResult.Active:
          behaviour.SetGrayState(false);
          continue;
        case InventoryWidget.ItemFilterResult.Inactive:
          behaviour.SetGrayState();
          continue;
        case InventoryWidget.ItemFilterResult.Hide:
          behaviour.Deactivate<BaseItemCellGUI>();
          continue;
        case InventoryWidget.ItemFilterResult.Unknown:
          behaviour.DrawUnknown();
          continue;
        default:
          continue;
      }
    }
    if (this.custom_size)
      return;
    this.RecalculateWidgetSize();
  }

  public void SetInactiveStateToEmptyCells()
  {
    foreach (BaseItemCellGUI baseItemCellGui in this.items)
    {
      if (baseItemCellGui.id_empty)
        baseItemCellGui.SetInactiveState();
    }
  }

  public void UpdatePrices(InventoryWidget.ItemPriceDelegate price_delegate, int count_modificator)
  {
    if (price_delegate == null)
      return;
    foreach (BaseItemCellGUI baseItemCellGui in this.items)
    {
      if (baseItemCellGui.id_empty || baseItemCellGui.is_inactive_state)
        baseItemCellGui.ClearPrice();
      else
        baseItemCellGui.SetItemPrice(price_delegate(baseItemCellGui.item, count_modificator));
    }
  }

  public void InitForCraft(List<string> item_types)
  {
    foreach (BaseItemCellGUI baseItemCellGui in this.items)
    {
      if (!item_types.Contains(baseItemCellGui.item_id))
        baseItemCellGui.widget.alpha = baseItemCellGui.colors.inactive.a;
    }
  }

  public void SetCallbacks(
    InventoryWidget.ItemDelegate on_over,
    InventoryWidget.ItemDelegate on_out,
    InventoryWidget.ItemDelegate on_press)
  {
    this.on_over = on_over;
    this.on_out = on_out;
    this.on_press = on_press;
  }

  public void Hide()
  {
    if (this.custom_size)
    {
      foreach (BaseItemCellGUI baseItemCellGui in this.items)
        baseItemCellGui.DrawEmpty();
    }
    else
    {
      this.Clear();
      this.gameObject.SetActive(false);
    }
  }

  public virtual void Clear()
  {
    foreach (BaseItemCellGUI component in this.items)
      component.DestroyGO<BaseItemCellGUI>();
    this.items.Clear();
  }

  public BaseItemCellGUI GetItemCellGuiForItem(Item item)
  {
    if (item == null)
      return (BaseItemCellGUI) null;
    foreach (BaseItemCellGUI itemCellGuiForItem in this.items)
    {
      if (itemCellGuiForItem.item == item)
        return itemCellGuiForItem;
    }
    return (BaseItemCellGUI) null;
  }

  public void OnItemOver(BaseItemCellGUI item_gui)
  {
    if (this.on_over != null)
      this.on_over(item_gui);
    if (!this.for_gamepad)
      return;
    int maxPerLine = this.items_table.maxPerLine;
    int num1 = this.items.IndexOf(item_gui) / maxPerLine + 1;
    int num2 = this.items.Count / maxPerLine;
    if (this.items.Count % maxPerLine > 0)
      ++num2;
    if (num1 == 1)
    {
      if (!((Object) this.header_auto_scroll != (Object) null))
        return;
      this.header_auto_scroll.Perform(!this.just_opened);
    }
    else
    {
      if (num1 != num2 || !((Object) this.bottom_auto_scroll != (Object) null))
        return;
      this.bottom_auto_scroll.Perform(!this.just_opened);
    }
  }

  public void OnItemOut(BaseItemCellGUI item_gui)
  {
    if (this.on_out == null)
      return;
    this.on_out(item_gui);
  }

  public void OnItemPress(BaseItemCellGUI item_gui)
  {
    if (this.on_press == null)
      return;
    this.on_press(item_gui);
  }

  public override void SetCustomNavigationTarget(BaseInventoryWidget widget, Direction direction)
  {
    GamepadNavigationItem firstNavigationItem = widget.GetFirstNavigationItem(direction);
    int maxPerLine = this.items_table.maxPerLine;
    if (this.items.Count <= maxPerLine)
    {
      foreach (BaseItemCellGUI baseItemCellGui in this.items)
        baseItemCellGui.gamepad_item.SetCustomDirectionItem(firstNavigationItem, direction);
    }
    else
    {
      int num1 = this.items.Count / maxPerLine;
      if (this.items.Count % maxPerLine == 0)
        --num1;
      int num2 = direction == Direction.Up ? 0 : num1 * maxPerLine;
      int num3 = direction == Direction.Up ? maxPerLine : this.items.Count;
      for (int index = num2; index < num3; ++index)
        this.items[index].gamepad_item.SetCustomDirectionItem(firstNavigationItem, direction);
    }
  }

  public void ClearItems() => this.Clear();

  public enum ItemFilterResult
  {
    Active,
    Inactive,
    Hide,
    Unknown,
  }

  public delegate void ItemDelegate(BaseItemCellGUI item_gui);

  public delegate InventoryWidget.ItemFilterResult ItemFilterDelegate(
    Item item,
    InventoryWidget widget);

  public delegate float ItemPriceDelegate(Item item, int count_modificator);
}
