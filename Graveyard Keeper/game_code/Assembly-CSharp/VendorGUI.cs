// Decompiled with JetBrains decompiler
// Type: VendorGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class VendorGUI : BaseGUI
{
  public const int OFFER_SIZE = 6;
  public UILabel result_money;
  public InventoryPanelGUI player_panel;
  public InventoryPanelGUI vendor_panel;
  public InventoryWidget player_offer_widget;
  public InventoryWidget vendor_offer_widget;
  public List<InventoryPanelGUI> _panels;
  public List<InventoryWidget> _widgets;
  public MultiInventory _player;
  public MultiInventory _vendor;
  public MultiInventory _vendor_real;
  public MultiInventory _player_offer;
  public MultiInventory _vendor_offer;
  public InventoryPanelGUI _selected_panel;
  public InventoryWidget _selected_widget;
  public BaseItemCellGUI _selected_item_gui;
  public Item _selected_item;
  public Trading trading;
  public WorldGameObject _last_vendor_obj;
  public WorldGameObject _vendor_obj;
  public UIButton btn_confirm;
  public UIButton btn_cancel;
  public bool _enable_time_after_close = true;

  public override void Init()
  {
    this._panels = new List<InventoryPanelGUI>()
    {
      this.player_panel,
      this.vendor_panel
    };
    this._widgets = new List<InventoryWidget>()
    {
      this.player_offer_widget,
      this.vendor_offer_widget
    };
    this.player_panel.Init();
    this.player_offer_widget.Init();
    this.vendor_panel.Init();
    this.vendor_offer_widget.Init();
    this.player_panel.SetCallbacks((GJCommons.VoidDelegate) (() => this.OnItemOver((BaseItemCellGUI) null, true, false)), (GJCommons.VoidDelegate) null, new GJCommons.VoidDelegate(this.OnItemSelect), new GJCommons.VoidDelegate(this.OnCustomItemOver));
    this.player_offer_widget.SetCallbacks((InventoryWidget.ItemDelegate) (item => this.OnItemOver(item, true, true)), (InventoryWidget.ItemDelegate) null, (InventoryWidget.ItemDelegate) (item => this.OnItemSelect()));
    this.vendor_panel.SetCallbacks((GJCommons.VoidDelegate) (() => this.OnItemOver((BaseItemCellGUI) null, false, false)), (GJCommons.VoidDelegate) null, new GJCommons.VoidDelegate(this.OnItemSelect), new GJCommons.VoidDelegate(this.OnCustomItemOver));
    this.vendor_offer_widget.SetCallbacks((InventoryWidget.ItemDelegate) (item => this.OnItemOver(item, false, true)), (InventoryWidget.ItemDelegate) null, (InventoryWidget.ItemDelegate) (item => this.OnItemSelect()));
    base.Init();
  }

  public void Open(WorldGameObject vendor_obj, GJCommons.VoidDelegate on_hide)
  {
    this.SetOnHide(on_hide);
    this.Open();
    this.trading = new Trading(vendor_obj);
    if (this.trading.trader == null)
    {
      Debug.LogError((object) $"Can not open VendorGUI: vendor of {vendor_obj.obj_id} is null!");
    }
    else
    {
      this._player = this.trading.player_inventory;
      this._vendor = this.trading.trader.drawing_inventory;
      this._vendor_real = this.trading.trader.inventory;
      this._vendor_obj = vendor_obj;
      if ((UnityEngine.Object) this._vendor_obj != (UnityEngine.Object) null)
        SmartAudioEngine.me.OnStartNPCInteraction(this._vendor_obj.obj_def);
      Item playerOffer = this.trading.player_offer;
      playerOffer.SetInventorySize(6);
      Item curOffer = this.trading.trader.cur_offer;
      curOffer.SetInventorySize(6);
      this._player_offer = new MultiInventory(new Inventory(playerOffer, ">>> Your offer >>>"));
      this._vendor_offer = new MultiInventory(new Inventory(curOffer, "<<< Vendor's offer <<<"));
      this.player_panel.Open(this._player, 1);
      this.vendor_panel.Open(this._vendor, 2, clear_name: true);
      this.player_panel.SetGrayToNotMainWidgets(true);
      this.player_offer_widget.Open(this._player_offer.all[0], BaseGUI.for_gamepad, 3);
      this.vendor_offer_widget.Open(this._vendor_offer.all[0], BaseGUI.for_gamepad, 3);
      this.vendor_panel.panel_title.text = GJL.L(vendor_obj.obj_id);
      this.player_panel.panel_title.text = GJL.L("player_inventory");
      this.player_panel.spr_head.sprite2D = EasySpritesCollection.GetSprite("000_hed_down");
      this.vendor_panel.spr_head.sprite2D = vendor_obj.GetHeadSprite();
      this.Redraw();
      if ((UnityEngine.Object) this._last_vendor_obj != (UnityEngine.Object) vendor_obj)
      {
        this._selected_panel = (InventoryPanelGUI) null;
        this._selected_item = (Item) null;
      }
      this._last_vendor_obj = vendor_obj;
      if (BaseGUI.for_gamepad)
      {
        this.gamepad_controller.ReinitItems(false);
        if ((UnityEngine.Object) this._selected_panel == (UnityEngine.Object) null || this._selected_item == null)
        {
          this.gamepad_controller.FocusOnFirstActive();
        }
        else
        {
          BaseItemCellGUI itemCellGuiForItem = this._selected_panel.GetItemCellGuiForItem(this._selected_item);
          this.gamepad_controller.SetFocusedItem((UnityEngine.Object) itemCellGuiForItem == (UnityEngine.Object) null ? (GamepadNavigationItem) null : itemCellGuiForItem.gamepad_item);
        }
      }
      this._enable_time_after_close = EnvironmentEngine.me.auto_adjust_time;
      if (!this._enable_time_after_close)
        return;
      EnvironmentEngine.me.EnableTime(false);
    }
  }

  public void OnItemOver(BaseItemCellGUI item_gui, bool players, bool offer)
  {
    if (offer)
    {
      this.player_panel.ClearSelection();
      this.vendor_panel.ClearSelection();
      this._selected_panel = (InventoryPanelGUI) null;
      this._selected_widget = players ? this.player_offer_widget : this.vendor_offer_widget;
      this._selected_item_gui = item_gui;
      this._selected_item = this._selected_item_gui.item;
    }
    else
    {
      (players ? this.vendor_panel : this.player_panel).ClearSelection();
      this._selected_panel = players ? this.player_panel : this.vendor_panel;
      this._selected_widget = (InventoryWidget) null;
      this._selected_item_gui = this._selected_panel.selected_item_gui;
      this._selected_item = this._selected_panel.selected_item;
    }
    if (!BaseGUI.for_gamepad)
      return;
    this.UpdateTips("put");
  }

  public void OnCustomItemOver() => this.PrintTipForCustomWidget();

  public void PrintTipForCustomWidget()
  {
    this.button_tips.Print(GameKeyTip.Option2("finish offer", !this.OfferIsEmpty()), GameKeyTip.Close());
  }

  public int GetMaxMoveCount(Item item, MultiInventory from, MultiInventory to)
  {
    return Mathf.Min(from.GetTotalCount(item.id), to.CanAddCount(item.id, true));
  }

  public void UpdateTips(string tip)
  {
    List<GameKeyTip> tips = new List<GameKeyTip>();
    int num = this._selected_item == null ? 1 : (this._selected_item.IsEmpty() ? 1 : 0);
    bool active = num == 0 && !this._selected_item_gui.is_inactive_state;
    MultiInventory from;
    MultiInventory to;
    if ((UnityEngine.Object) this._selected_panel != (UnityEngine.Object) null)
    {
      from = this._selected_panel.multi_inventory;
      to = (UnityEngine.Object) this._selected_panel == (UnityEngine.Object) this.player_panel ? this._player_offer : this._vendor_offer;
    }
    else
    {
      from = (UnityEngine.Object) this._selected_widget == (UnityEngine.Object) this.player_offer_widget ? this._player_offer : this._vendor_offer;
      to = (UnityEngine.Object) this._selected_widget == (UnityEngine.Object) this.player_offer_widget ? this._player : this._vendor;
    }
    int maxMoveCount = num != 0 ? 0 : this.GetMaxMoveCount(this._selected_item, from, to);
    if (maxMoveCount <= 1)
    {
      tips.Add(GameKeyTip.Select(tip, active && maxMoveCount > 0));
      tips.Add(GameKeyTip.Option1(GJL.L(tip) + " 1", active && maxMoveCount > 0, translate: false));
    }
    else if (this._selected_item != null)
    {
      tips.Add(GameKeyTip.Select(tip, active));
      tips.Add(GameKeyTip.Option1(GJL.L(tip) + " 1", active, translate: false));
    }
    tips.Add(GameKeyTip.Option2("finish offer", !this.OfferIsEmpty()));
    tips.Add(GameKeyTip.Close());
    this.button_tips.Print(tips);
  }

  public void MoveItem(int count)
  {
    if ((UnityEngine.Object) this._selected_panel == (UnityEngine.Object) null && (UnityEngine.Object) this._selected_widget == (UnityEngine.Object) null || this._selected_item == null || this._selected_item.IsEmpty() || (UnityEngine.Object) this._selected_item_gui == (UnityEngine.Object) null || this._selected_item_gui.is_inactive_state)
      return;
    MultiInventory multiInventory1 = (MultiInventory) null;
    MultiInventory multiInventory2 = (MultiInventory) null;
    MultiInventory from;
    MultiInventory multiInventory3;
    if ((UnityEngine.Object) this._selected_panel != (UnityEngine.Object) null)
    {
      if ((UnityEngine.Object) this._selected_panel == (UnityEngine.Object) this.player_panel)
      {
        from = this._player;
        multiInventory3 = this._player_offer;
      }
      else
      {
        from = this._vendor;
        multiInventory1 = this._vendor_real;
        multiInventory3 = this._vendor_offer;
      }
    }
    else if ((UnityEngine.Object) this._selected_widget == (UnityEngine.Object) this.player_offer_widget)
    {
      from = this._player_offer;
      multiInventory3 = this._player;
    }
    else
    {
      from = this._vendor_offer;
      multiInventory3 = this._vendor;
      multiInventory2 = this._vendor_real;
    }
    int maxMoveCount = this.GetMaxMoveCount(this._selected_item, from, multiInventory3);
    if (count == 0)
      count = maxMoveCount;
    int num = this._selected_item_gui.item.value;
    bool flag = true;
    if (count == 0 || maxMoveCount == 0)
      return;
    if (LazyInput.GetKeyDown(GameKey.MoveAllStack))
    {
      if (multiInventory3 == this._vendor)
      {
        int index = this._selected_item.definition.product_tier - 1;
        if (index >= this.trading.trader.max_tier)
          index = this.trading.trader.max_tier;
        from.MoveItemTo(multiInventory3.all[index].data, this._selected_item, maxMoveCount);
      }
      else if (!from.MoveItemTo(multiInventory3, this._selected_item, maxMoveCount))
        return;
      if (multiInventory1 != null)
        multiInventory1.RemoveItem(this._selected_item, maxMoveCount, MultiInventory.DestinationType.AllFromFirst);
      else
        multiInventory2?.AddItem(this._selected_item.id, maxMoveCount);
    }
    else if (count == 1 || maxMoveCount == 1)
    {
      if (multiInventory3 == this._vendor)
      {
        int index = this._selected_item.definition.product_tier - 1;
        if (index >= this.trading.trader.max_tier)
          index = this.trading.trader.max_tier;
        from.MoveItemTo(multiInventory3.all[index].data, this._selected_item, 1);
      }
      else if (!from.MoveItemTo(multiInventory3, this._selected_item, 1))
        return;
      if (multiInventory1 != null)
        multiInventory1.RemoveItem(this._selected_item, 1, MultiInventory.DestinationType.AllFromFirst);
      else
        multiInventory2?.AddItem(this._selected_item.id, 1);
    }
    else
    {
      this.OpenItemCountWidnow(this._selected_item.id, count, (UnityEngine.Object) this._selected_panel != (UnityEngine.Object) null, (UnityEngine.Object) this._selected_panel == (UnityEngine.Object) this.player_panel || (UnityEngine.Object) this._selected_widget == (UnityEngine.Object) this.player_offer_widget);
      flag = false;
    }
    if (flag)
      Sounds.PlaySound("item_put");
    this.Redraw();
    this._selected_item_gui.OnOver(BaseGUI.for_gamepad);
    if (count < num)
      return;
    TooltipsManager.Redraw();
  }

  public void OpenItemCountWidnow(
    string item_id,
    int can_move,
    bool from_inventory,
    bool from_player)
  {
    this.button_tips.Deactivate<ButtonTipsStr>();
    GUIElements.me.item_count.Open(item_id, 1, can_move, (Action<int>) (chosen =>
    {
      MultiInventory multiInventory1 = (MultiInventory) null;
      MultiInventory multiInventory2 = (MultiInventory) null;
      MultiInventory multiInventory3;
      MultiInventory another_inventory;
      if (from_inventory)
      {
        if (from_player)
        {
          multiInventory3 = this._player;
          another_inventory = this._player_offer;
        }
        else
        {
          multiInventory3 = this._vendor;
          multiInventory1 = this._vendor_real;
          another_inventory = this._vendor_offer;
        }
      }
      else if (from_player)
      {
        multiInventory3 = this._player_offer;
        another_inventory = this._player;
      }
      else
      {
        multiInventory3 = this._vendor_offer;
        another_inventory = this._vendor;
        multiInventory2 = this._vendor_real;
      }
      if (another_inventory == this._vendor)
      {
        int index = this._selected_item.definition.product_tier - 1;
        if (index >= this.trading.trader.max_tier)
          index = this.trading.trader.max_tier;
        multiInventory3.MoveItemTo(another_inventory.all[index].data, this._selected_item, chosen);
      }
      else
        multiInventory3.MoveItemTo(another_inventory, this._selected_item, chosen);
      if (multiInventory1 != null)
        multiInventory1.RemoveItem(this._selected_item, chosen, MultiInventory.DestinationType.AllFromFirst);
      else
        multiInventory2?.AddItem(this._selected_item.id, chosen);
    }), price_calculate_delegate: (ItemCountGUI.PriceCalculateDelegate) (amount =>
    {
      float num1 = 0.0f;
      for (int index = 0; index < amount; ++index)
      {
        float num2 = from_player ? this.trading.GetSingleItemCostInPlayerInventory(item_id, from_inventory ? index + 1 : -index) : this.trading.GetSingleItemCostInTraderInventory(item_id, from_inventory ? -index : index + 1);
        num1 += Mathf.Round(num2 * 100f) / 100f;
      }
      return num1;
    }));
    GUIElements.me.item_count.SetOnHide((GJCommons.VoidDelegate) (() =>
    {
      if (BaseGUI.for_gamepad)
        this.button_tips.Activate<ButtonTipsStr>();
      this.Redraw();
      this.RestoreFocusAfterDialog();
      TooltipsManager.Redraw();
    }));
  }

  public void OnItemSelect()
  {
    if (BaseGUI.for_gamepad)
      return;
    this.MoveItem(LazyInput.GetKeyDown(GameKey.RightClick) ? 1 : 0);
  }

  public void RefreshButtonsState()
  {
    bool flag1 = !this.OfferIsEmpty() && this.trading.CanAcceptOffer();
    bool flag2 = !this.OfferIsEmpty();
    foreach (UIButton componentsInChild in this.btn_confirm.gameObject.GetComponentsInChildren<UIButton>(true))
    {
      componentsInChild.isEnabled = flag1;
      if ((UnityEngine.Object) componentsInChild != (UnityEngine.Object) this.btn_confirm)
        componentsInChild.SetState(flag1 ? UIButtonColor.State.Pressed : UIButtonColor.State.Disabled, true);
    }
    foreach (UIButton componentsInChild in this.btn_cancel.gameObject.GetComponentsInChildren<UIButton>(true))
    {
      componentsInChild.isEnabled = flag2;
      if ((UnityEngine.Object) componentsInChild != (UnityEngine.Object) this.btn_cancel)
        componentsInChild.SetState(flag2 ? UIButtonColor.State.Pressed : UIButtonColor.State.Disabled, true);
    }
  }

  public void FinishOffer()
  {
    if (this.trading.CanAcceptOffer())
    {
      this.trading.DoAcceptOffer(false);
      this.vendor_panel.Hide();
      this.vendor_panel.Open(this._vendor, 2, clear_name: true);
      this.Redraw();
      TooltipsManager.Redraw();
      if (BaseGUI.for_gamepad)
      {
        this.gamepad_controller.ReinitItems(true);
        this._selected_item_gui.OnOver(BaseGUI.for_gamepad);
      }
      Sounds.PlaySound("coins_sound");
    }
    else
    {
      GUIElements.me.dialog.OpenOK("cant_accept_offer", new GJCommons.VoidDelegate(this.RestoreFocusAfterDialog));
      Debug.Log((object) "Can not accept offer!");
    }
  }

  public void Redraw()
  {
    this.player_panel.Redraw();
    this.player_panel.FilterItems(new InventoryWidget.ItemFilterDelegate(this.trading.BuyableItemsFilter));
    this.vendor_panel.Redraw();
    this.vendor_panel.FilterItems(new InventoryWidget.ItemFilterDelegate(this.trading.SellableItemsFilter));
    this.player_offer_widget.Redraw();
    this.vendor_offer_widget.Redraw();
    this.player_panel.UpdatePrices(new InventoryWidget.ItemPriceDelegate(this.trading.GetSingleItemCostInPlayerInventory), 1);
    this.player_offer_widget.UpdatePrices(new InventoryWidget.ItemPriceDelegate(this.trading.GetSingleItemCostInPlayerInventory), 0);
    this.vendor_panel.UpdatePrices(new InventoryWidget.ItemPriceDelegate(this.trading.GetSingleItemCostInTraderInventory), 0);
    this.vendor_offer_widget.UpdatePrices(new InventoryWidget.ItemPriceDelegate(this.trading.GetSingleItemCostInTraderInventory), 1);
    Trading.DrawMoneyOnLabel(this.player_panel.money_label, this.trading.player_money, true);
    Trading.DrawMoneyOnLabel(this.vendor_panel.money_label, this.trading.trader.cur_money, true);
    Trading.DrawMoneyOnLabel(this.result_money, this.trading.GetTotalBalance());
    this.RefreshButtonsState();
  }

  public void RestoreFocusAfterDialog()
  {
    if (!BaseGUI.for_gamepad)
      return;
    this.gamepad_controller.Enable(GamepadNavigationController.OpenMethod.GetAll);
    this.gamepad_controller.SetFocusedItem((UnityEngine.Object) this._selected_item_gui != (UnityEngine.Object) null ? this._selected_item_gui.gamepad_item : (GamepadNavigationItem) null);
  }

  public override void Hide(bool play_hide_sound = true)
  {
    this.ResetOrder();
    this._selected_widget = (InventoryWidget) null;
    this._selected_item_gui = (BaseItemCellGUI) null;
    foreach (InventoryPanelGUI panel in this._panels)
      panel.Hide();
    foreach (InventoryWidget widget in this._widgets)
      widget.Hide();
    base.Hide(play_hide_sound);
    if ((UnityEngine.Object) this._vendor_obj != (UnityEngine.Object) null)
      SmartAudioEngine.me.OnEndNPCInteraction(this._vendor_obj.obj_def);
    EnvironmentEngine.me.EnableTime(this._enable_time_after_close);
  }

  public override bool OnPressedSelect()
  {
    this.MoveItem(0);
    return true;
  }

  public override bool OnPressedOption1()
  {
    this.MoveItem(1);
    return true;
  }

  public override bool OnPressedOption2()
  {
    this.FinishOffer();
    return true;
  }

  public override bool OnPressedBack()
  {
    this.OnClosePressed();
    return true;
  }

  public override void OnClosePressed()
  {
    if (this.OfferIsEmpty())
      base.OnClosePressed();
    else
      GUIElements.me.dialog.OpenYesNo("cancel_offer", (GJCommons.VoidDelegate) (() => this.Hide(true)), new GJCommons.VoidDelegate(this.RestoreFocusAfterDialog));
  }

  public bool OfferIsEmpty()
  {
    return this.trading.player_offer.inventory.Count == 0 && this.trading.trader.cur_offer.inventory.Count == 0;
  }

  public void ResetOrder()
  {
    if (this._player_offer != null)
    {
      this._player.AddItems(this._player_offer.all[0].data.inventory, true);
      this._player_offer.all[0].data.inventory.Clear();
    }
    if (this._vendor_offer != null)
    {
      this._vendor_real.AddItems(this._vendor_offer.all[0].data.inventory);
      this._vendor.AddItems(this._vendor_offer.all[0].data.inventory);
      this._vendor_offer.all[0].data.inventory.Clear();
    }
    if (this.trading == null || this.trading.trader == null)
      return;
    this.trading.trader.FillDrawingMultiInventory();
    this.vendor_panel.Hide();
    this.vendor_panel.Open(this._vendor, 2, clear_name: true);
    this.Redraw();
    TooltipsManager.Redraw();
    if (!BaseGUI.for_gamepad)
      return;
    this.gamepad_controller.ReinitItems(false);
    this._selected_item_gui.OnOver(BaseGUI.for_gamepad);
  }

  public void ResetOrderAndRedraw()
  {
    Sounds.OnGUIClick();
    this.ResetOrder();
    this.Redraw();
  }

  [CompilerGenerated]
  public void \u003CInit\u003Eb__23_0() => this.OnItemOver((BaseItemCellGUI) null, true, false);

  [CompilerGenerated]
  public void \u003CInit\u003Eb__23_1(BaseItemCellGUI item) => this.OnItemOver(item, true, true);

  [CompilerGenerated]
  public void \u003CInit\u003Eb__23_2(BaseItemCellGUI item) => this.OnItemSelect();

  [CompilerGenerated]
  public void \u003CInit\u003Eb__23_3() => this.OnItemOver((BaseItemCellGUI) null, false, false);

  [CompilerGenerated]
  public void \u003CInit\u003Eb__23_4(BaseItemCellGUI item) => this.OnItemOver(item, false, true);

  [CompilerGenerated]
  public void \u003CInit\u003Eb__23_5(BaseItemCellGUI item) => this.OnItemSelect();

  [CompilerGenerated]
  public void \u003COnClosePressed\u003Eb__42_0() => this.Hide(true);
}
