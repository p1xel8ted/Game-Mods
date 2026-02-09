// Decompiled with JetBrains decompiler
// Type: InventoryPanelGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DG.Tweening;
using DLCRefugees;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class InventoryPanelGUI : MonoBehaviour
{
  public UILabel money_label;
  public UILabel panel_title;
  public GameObject bottom_bar;
  public bool draw_equipped_icons;
  public bool dont_show_empty_rows;
  public UI2DSprite spr_head;
  public UIScrollView _scroll_view;
  public UITable _widgets_table;
  public InventoryWidget _inventory_widget_prefab;
  public BagInventoryWidget _bag_inventory_widget_prefab;
  public SoulContainerInventoryWidget _soul_container_widget;
  public Dictionary<string, CustomInventoryWidget> _custom_widgets_presets = new Dictionary<string, CustomInventoryWidget>();
  public BaseInventoryWidget _selected_widget;
  public BaseItemCellGUI _selected_item_gui;
  public Item _selected_item;
  public MultiInventory _multi_inventory;
  public GJCommons.VoidDelegate _on_over;
  public GJCommons.VoidDelegate _on_out;
  public GJCommons.VoidDelegate _on_press;
  public GJCommons.VoidDelegate _on_custom_widget_over;
  public List<InventoryWidget> _widgets = new List<InventoryWidget>();
  public List<UIWidget> _separators = new List<UIWidget>();
  public List<CustomInventoryWidget> _custom_widgets = new List<CustomInventoryWidget>();
  public bool _waiting_for_reposition;
  [SerializeField]
  public GameObject bottom_bar_with_debt;
  [SerializeField]
  public UILabel money_label_2;
  [SerializeField]
  public UILabel debt_label;
  [CompilerGenerated]
  public static InventoryPanelGUI \u003Clast\u003Ek__BackingField;

  public BaseInventoryWidget selected_widget => this._selected_widget;

  public static InventoryPanelGUI last
  {
    get => InventoryPanelGUI.\u003Clast\u003Ek__BackingField;
    set => InventoryPanelGUI.\u003Clast\u003Ek__BackingField = value;
  }

  public MultiInventory multi_inventory => this._multi_inventory;

  public BaseItemCellGUI selected_item_gui
  {
    get => this._selected_item_gui;
    set
    {
      this._selected_item_gui = value;
      this._selected_item = (UnityEngine.Object) this._selected_item_gui == (UnityEngine.Object) null ? (Item) null : this._selected_item_gui.item;
    }
  }

  public Item selected_item => this._selected_item;

  public bool selected_item_is_empty
  {
    get => this._selected_item == null || this._selected_item.IsEmpty();
  }

  public bool selected_widget_is_not_main
  {
    get => (UnityEngine.Object) this._selected_widget == (UnityEngine.Object) null || !this._selected_widget.IsMain();
  }

  public void Init()
  {
    this._inventory_widget_prefab = this.GetComponentInChildren<InventoryWidget>(true);
    this._inventory_widget_prefab.Init();
    this._bag_inventory_widget_prefab = this.GetComponentInChildren<BagInventoryWidget>(true);
    if ((UnityEngine.Object) this._bag_inventory_widget_prefab != (UnityEngine.Object) null)
      this._bag_inventory_widget_prefab.Init();
    this._soul_container_widget = this.GetComponentInChildren<SoulContainerInventoryWidget>(true);
    this._soul_container_widget?.Init();
    this._scroll_view = this.GetComponentInChildren<UIScrollView>(true);
    this._widgets_table = this._scroll_view.GetComponentInChildren<UITable>(true);
    foreach (CustomInventoryWidget componentsInChild in this.GetComponentsInChildren<CustomInventoryWidget>(true))
    {
      componentsInChild.Init();
      this._custom_widgets_presets.Add(componentsInChild.name, componentsInChild);
      componentsInChild.Deactivate<CustomInventoryWidget>();
    }
    this.gameObject.SetActive(false);
  }

  public void Open(
    MultiInventory multi_inventory,
    int navigation_group = 0,
    int navigation_sub_group = 0,
    bool clear_name = false,
    int custom_line_length = -1,
    bool is_debt_show = false)
  {
    this.gameObject.SetActive(true);
    this.DoOpening(multi_inventory, navigation_group, navigation_sub_group, clear_name, custom_line_length, is_debt_show);
  }

  public void DoOpening(
    MultiInventory multi_inventory,
    int navigation_group = 0,
    int navigation_sub_group = 0,
    bool clear_name = false,
    int custom_line_length = -1,
    bool is_debt_show = false)
  {
    this._multi_inventory = multi_inventory;
    BaseInventoryWidget widget = (BaseInventoryWidget) null;
    int num = -1;
    Transform parent = this._inventory_widget_prefab.transform.parent;
    bool flag1 = (UnityEngine.Object) this._bag_inventory_widget_prefab != (UnityEngine.Object) null;
    bool flag2 = multi_inventory.all.Find((Predicate<Inventory>) (i => i.data == MainGame.me.player.data)) == null && GlobalCraftControlGUI.is_global_control_active;
    bool flag3 = (UnityEngine.Object) this._soul_container_widget != (UnityEngine.Object) null;
    if (flag2)
    {
      clear_name = false;
      flag1 = false;
    }
    foreach (Inventory inventory in this._multi_inventory.all)
    {
      if (inventory != null && (inventory.data.inventory_size > 0 || !GlobalCraftControlGUI.is_global_control_active))
      {
        if (++num == 0 & clear_name)
          inventory.ClearName();
        if (this._widgets.Count > 0 || this._custom_widgets.Count > 0)
        {
          GameObject gameObject = new GameObject("separator");
          gameObject.transform.SetParent(parent, false);
          UIWidget uiWidget = gameObject.AddComponent<UIWidget>();
          uiWidget.height = 6;
          this._separators.Add(uiWidget);
        }
        string key = inventory.preset;
        if (!string.IsNullOrEmpty(key) && !this._custom_widgets_presets.ContainsKey(key))
        {
          if (key != "soul_container_widget")
            Debug.LogError((object) ("No custom preset: " + key));
          key = "";
        }
        if (string.IsNullOrEmpty(key))
        {
          InventoryWidget inventoryWidget;
          if (flag3 && inventory.data != null && inventory.preset == "soul_container_widget")
          {
            SoulContainerInventoryWidget cont_w = this._soul_container_widget.Copy<SoulContainerInventoryWidget>();
            cont_w.SetActive(true);
            cont_w.Open(inventory, BaseGUI.for_gamepad, navigation_group, navigation_sub_group++, this.dont_show_empty_rows, custom_line_length);
            cont_w.SetCallbacks((InventoryWidget.ItemDelegate) (i => this.OnItemOver((InventoryWidget) cont_w, i)), new InventoryWidget.ItemDelegate(this.OnItemOut), (InventoryWidget.ItemDelegate) (i => this.OnItemPress((InventoryWidget) cont_w, i)));
            inventoryWidget = (InventoryWidget) cont_w;
          }
          else if (flag1 && inventory.data != null && inventory.data.is_bag)
          {
            BagInventoryWidget bag_w = this._bag_inventory_widget_prefab.Copy<BagInventoryWidget>();
            bag_w.SetActive(true);
            bag_w.Open(inventory, BaseGUI.for_gamepad, navigation_group, navigation_sub_group++, this.dont_show_empty_rows, custom_line_length);
            bag_w.SetCallbacks((InventoryWidget.ItemDelegate) (i => this.OnItemOver((InventoryWidget) bag_w, i)), new InventoryWidget.ItemDelegate(this.OnItemOut), (InventoryWidget.ItemDelegate) (i => this.OnItemPress((InventoryWidget) bag_w, i)));
            inventoryWidget = (InventoryWidget) bag_w;
          }
          else
          {
            InventoryWidget w = this._inventory_widget_prefab.Copy<InventoryWidget>();
            w.Open(inventory, BaseGUI.for_gamepad, navigation_group, navigation_sub_group++, this.dont_show_empty_rows, custom_line_length);
            w.SetCallbacks((InventoryWidget.ItemDelegate) (i => this.OnItemOver(w, i)), new InventoryWidget.ItemDelegate(this.OnItemOut), (InventoryWidget.ItemDelegate) (i => this.OnItemPress(w, i)));
            inventoryWidget = w;
          }
          if (this._widgets.Count == 0 && !flag2)
            inventoryWidget.SetMain();
          this._widgets.Add(inventoryWidget);
          if ((UnityEngine.Object) widget != (UnityEngine.Object) null && widget.IsCustom())
            inventoryWidget.SetCustomNavigationTarget(widget, Direction.Up);
          widget = (BaseInventoryWidget) inventoryWidget;
        }
        else
        {
          CustomInventoryWidget w = this._custom_widgets_presets[key].Copy<CustomInventoryWidget>();
          w.Open(inventory, BaseGUI.for_gamepad, navigation_group, navigation_sub_group++, (GJCommons.VoidDelegate) (() => this.OnCustomItemOver(w)));
          this._custom_widgets.Add(w);
          if ((UnityEngine.Object) widget != (UnityEngine.Object) null && !widget.IsCustom())
            widget.SetCustomNavigationTarget((BaseInventoryWidget) w, Direction.Down);
          widget = (BaseInventoryWidget) w;
        }
      }
    }
    this._widgets_table.Reposition();
    this._widgets_table.repositionNow = true;
    this._waiting_for_reposition = true;
    this._scroll_view.transform.localPosition = Vector3.zero;
    this._scroll_view.RestrictWithinBounds(false);
    if (this.draw_equipped_icons)
      this.DrawEquippedIcons();
    this.RedrawMoney();
    if ((UnityEngine.Object) this.bottom_bar_with_debt != (UnityEngine.Object) null & is_debt_show)
    {
      float playersDebt = RefugeesCampEngine.GetPlayersDebt();
      if (!playersDebt.Equals(0.0f))
        this.RedrawMoneyWithDebt(playersDebt);
    }
    this._selected_widget = (BaseInventoryWidget) null;
  }

  public void FullRedraw(
    MultiInventory multi_inventory,
    int navigation_group = 0,
    int navigation_sub_group = 0,
    bool clear_name = false,
    int custom_line_length = -1)
  {
    this.Clear();
    this._scroll_view.StopScrolling();
    this._scroll_view.transform.DOKill();
    this._scroll_view.RestrictWithinBounds(false);
    this._scroll_view.transform.localPosition = Vector3.zero;
    this.DoOpening(multi_inventory, navigation_group, navigation_sub_group, clear_name, custom_line_length);
  }

  public void SetGrayToNotMainWidgets(bool do_not_gray_bags = false)
  {
    foreach (InventoryWidget widget in this._widgets)
    {
      if (!widget.IsMain() && (!do_not_gray_bags || !widget.inventory_data.is_bag))
        widget.GetComponent<UIWidget>().alpha = 0.5f;
    }
    foreach (Component customWidget in this._custom_widgets)
      customWidget.GetComponent<UIWidget>().alpha = 0.5f;
  }

  public void LateUpdate()
  {
    if (this._waiting_for_reposition)
    {
      this._widgets_table.Reposition();
      this._waiting_for_reposition = false;
    }
    if ((UnityEngine.Object) this._scroll_view == (UnityEngine.Object) null || !this._scroll_view.RestrictWithinBounds(false) || !DOTween.IsTweening((object) this._scroll_view.transform))
      return;
    this._scroll_view.transform.DOKill();
  }

  public void InitGamepad(GamepadNavigationController controller)
  {
    if ((UnityEngine.Object) controller == (UnityEngine.Object) null)
      return;
    controller.ReinitItems(false);
    if (this.selected_item == null)
    {
      controller.FocusOnFirstActive();
    }
    else
    {
      BaseItemCellGUI itemCellGuiForItem = this.GetItemCellGuiForItem(this.selected_item);
      controller.SetFocusedItem((UnityEngine.Object) itemCellGuiForItem == (UnityEngine.Object) null ? (GamepadNavigationItem) null : itemCellGuiForItem.gamepad_item, false);
    }
  }

  public void Redraw()
  {
    foreach (BaseInventoryWidget widget in this._widgets)
      widget.Redraw();
    foreach (BaseInventoryWidget customWidget in this._custom_widgets)
      customWidget.Redraw();
    if (this.draw_equipped_icons)
      this.DrawEquippedIcons();
    this.RedrawMoney();
    this._widgets_table.Reposition();
    this._widgets_table.repositionNow = true;
  }

  public void DrawEquippedIcons()
  {
    foreach (InventoryWidget widget in this._widgets)
      widget.DrawEquippedIcons();
  }

  public void RedrawMoney()
  {
    if (!((UnityEngine.Object) this.money_label != (UnityEngine.Object) null))
      return;
    this.money_label.text = Trading.FormatMoney(this._multi_inventory.money, true);
  }

  public void RedrawMoneyWithDebt(float debt_value)
  {
    this.bottom_bar_with_debt.SetActive(true);
    if ((UnityEngine.Object) this.money_label_2 != (UnityEngine.Object) null)
      this.money_label_2.text = Trading.FormatMoney(this._multi_inventory.money, true);
    if (!((UnityEngine.Object) this.debt_label != (UnityEngine.Object) null))
      return;
    this.debt_label.text = GJL.L("debt_label") + ": ";
    this.debt_label.text += Trading.FormatMoney(debt_value, true);
  }

  public void OnItemOver(InventoryWidget widget, BaseItemCellGUI item_gui)
  {
    if (GUIElements.me.context_menu_bubble.is_shown || GUIElements.me.dialog.is_shown)
      return;
    InventoryPanelGUI.last = this;
    this._selected_widget = (BaseInventoryWidget) widget;
    this.selected_item_gui = item_gui;
    this._on_over.TryInvoke();
  }

  public void OnItemOut(BaseItemCellGUI item_gui) => this._on_out.TryInvoke();

  public void OnCustomItemOver(CustomInventoryWidget widget)
  {
    this._selected_widget = (BaseInventoryWidget) widget;
    this.selected_item_gui = (BaseItemCellGUI) null;
    this._on_custom_widget_over.TryInvoke();
  }

  public void OnItemPress(InventoryWidget widget, BaseItemCellGUI item_gui)
  {
    InventoryPanelGUI.last = this;
    this._selected_widget = (BaseInventoryWidget) widget;
    this.selected_item_gui = item_gui;
    this._on_press.TryInvoke();
  }

  public void SetCallbacks(
    GJCommons.VoidDelegate on_over,
    GJCommons.VoidDelegate on_out,
    GJCommons.VoidDelegate on_press,
    GJCommons.VoidDelegate on_custom_widget_over = null)
  {
    this._on_over = on_over;
    this._on_out = on_out;
    this._on_press = on_press;
    this._on_custom_widget_over = on_custom_widget_over;
  }

  public void Hide()
  {
    this.Clear();
    this._scroll_view.StopScrolling();
    this._scroll_view.transform.DOKill();
    this._scroll_view.RestrictWithinBounds(false);
    this._scroll_view.transform.localPosition = Vector3.zero;
    if ((UnityEngine.Object) this.bottom_bar_with_debt != (UnityEngine.Object) null)
      this.bottom_bar_with_debt.SetActive(false);
    this.gameObject.SetActive(false);
  }

  public void Clear()
  {
    foreach (InventoryWidget widget in this._widgets)
    {
      widget.Deactivate<InventoryWidget>();
      widget.DestroyGO<InventoryWidget>();
    }
    foreach (CustomInventoryWidget customWidget in this._custom_widgets)
    {
      customWidget.Deactivate<CustomInventoryWidget>();
      customWidget.DestroyGO<CustomInventoryWidget>();
    }
    foreach (UIWidget separator in this._separators)
    {
      separator.Deactivate<UIWidget>();
      separator.DestroyGO<UIWidget>();
    }
    this._widgets.Clear();
    this._custom_widgets.Clear();
    this._separators.Clear();
    this._widgets_table.Reposition();
    this._scroll_view.ResetPosition();
  }

  public void UpdateSelection(bool after_item_count_gui = false)
  {
    if (!((UnityEngine.Object) this.selected_item_gui != (UnityEngine.Object) null) || this.selected_item_gui.id_empty)
      return;
    if (BaseGUI.for_gamepad)
    {
      this.selected_item_gui.OnOver(true);
    }
    else
    {
      if (after_item_count_gui)
        return;
      this.selected_item_gui.OnOver(false);
    }
  }

  public BaseItemCellGUI GetItemCellGuiForItem(Item item)
  {
    if (item == null || this._widgets.Count == 0)
      return (BaseItemCellGUI) null;
    foreach (InventoryWidget widget in this._widgets)
    {
      BaseItemCellGUI itemCellGuiForItem = widget.GetItemCellGuiForItem(item);
      if ((UnityEngine.Object) itemCellGuiForItem != (UnityEngine.Object) null)
        return itemCellGuiForItem;
    }
    return (BaseItemCellGUI) null;
  }

  public void UpdatePrices(InventoryWidget.ItemPriceDelegate price_delegate, int count_modificator)
  {
    if (price_delegate == null)
      return;
    foreach (InventoryWidget widget in this._widgets)
      widget.UpdatePrices(price_delegate, count_modificator);
  }

  public void FilterItems(InventoryWidget.ItemFilterDelegate filter_delegate)
  {
    foreach (InventoryWidget widget in this._widgets)
      widget.FilterItems(filter_delegate);
  }

  public void SetInactiveStateToEmptyCells()
  {
    foreach (InventoryWidget widget in this._widgets)
      widget.SetInactiveStateToEmptyCells();
  }

  public void ClearSelection() => this.selected_item_gui = (BaseItemCellGUI) null;
}
