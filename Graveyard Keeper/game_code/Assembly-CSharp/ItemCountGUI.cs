// Decompiled with JetBrains decompiler
// Type: ItemCountGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class ItemCountGUI : BaseGUI
{
  public UILabel header_label;
  public DialogButtonsGUI _dialog_buttons;
  public BaseItemCellGUI _item_gui;
  public SmartSlider _slider;
  public Action<int> _on_confirm;
  public UILabel price;
  public UIWidget window;
  public int height_with_price = 144 /*0x90*/;
  public int height_without_price = 116;
  public ItemCountGUI.PriceCalculateDelegate _price_calculate_delegate;

  public override void Init()
  {
    this._dialog_buttons = this.GetComponentInChildren<DialogButtonsGUI>(true);
    this._dialog_buttons.Init();
    this._item_gui = this.GetComponentInChildren<BaseItemCellGUI>();
    this._slider = this.GetComponentInChildren<SmartSlider>(true);
    this._slider.Init();
    base.Init();
  }

  public void Open(
    string item_id,
    int min,
    int max,
    Action<int> on_confirm,
    int slider_step_for_keyboard = 1,
    ItemCountGUI.PriceCalculateDelegate price_calculate_delegate = null)
  {
    ItemDefinition data = GameBalance.me.GetData<ItemDefinition>(item_id);
    this._price_calculate_delegate = price_calculate_delegate;
    if (data == null)
    {
      Debug.LogError((object) "Cannot open ItemCountGUI because of null definition", (UnityEngine.Object) this);
    }
    else
    {
      this.Open();
      bool flag = price_calculate_delegate != null;
      this._item_gui.DrawItem(item_id, 1);
      this._item_gui.interaction_enabled = false;
      this.header_label.text = data.GetItemName();
      this.window.height = flag ? this.height_with_price : this.height_without_price;
      this._slider.Open(flag ? 1 : max, min, max, (Action<int>) (n => this.RedrawPrice()), true, true);
      this._on_confirm = on_confirm;
      this._dialog_buttons.Set("ok", new GJCommons.VoidDelegate(this.OnConfirm), BaseGUI.for_gamepad ? "back" : (string) null, (GJCommons.VoidDelegate) (() => this.OnPressedBack()));
      this.RedrawPrice();
    }
  }

  public void RedrawPrice()
  {
    this.price.text = this._price_calculate_delegate == null ? "" : $"{GJL.L("price_dialog_total")}\n{Trading.FormatMoney(this._price_calculate_delegate(this._slider.value))}";
  }

  public void OnConfirm()
  {
    if (this._on_confirm != null)
      this._on_confirm(this._slider.value);
    this.Hide();
  }

  public override bool OnPressedBack()
  {
    this.OnClosePressed();
    return true;
  }

  [CompilerGenerated]
  public void \u003COpen\u003Eb__12_0(int n) => this.RedrawPrice();

  [CompilerGenerated]
  public void \u003COpen\u003Eb__12_1() => this.OnPressedBack();

  public delegate float PriceCalculateDelegate(int amount);
}
