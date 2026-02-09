// Decompiled with JetBrains decompiler
// Type: CustomInventoryItem
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class CustomInventoryItem : MonoBehaviour
{
  public string item_id;
  [NonSerialized]
  public int total_count;
  public GameObject gamepad_frame;
  public UILabel _counter_label;
  public Tooltip _tooltip;
  public CustomInventoryWidget _inventory_widget;
  public GamepadNavigationItem _gamepad_item;

  public GamepadNavigationItem gamepad_item => this._gamepad_item;

  public void Init(CustomInventoryWidget inventory_widget)
  {
    this._inventory_widget = inventory_widget;
    this._counter_label = this.GetComponentInChildren<UILabel>(true);
    this._tooltip = this.GetComponentInChildren<Tooltip>();
    this.gamepad_frame.Deactivate();
    ItemDefinition data = GameBalance.me.GetData<ItemDefinition>(this.item_id);
    if (data != null && (UnityEngine.Object) this._tooltip != (UnityEngine.Object) null)
      this._tooltip.SetText(data.GetItemName());
    this._gamepad_item = this.GetComponent<GamepadNavigationItem>();
    this._gamepad_item.SetCallbacks(new GJCommons.VoidDelegate(this.OnOver), new GJCommons.VoidDelegate(((ExtentionTools) this.gamepad_frame).Deactivate), (GJCommons.VoidDelegate) null);
  }

  public void Redraw(Inventory inventory)
  {
    this.total_count = inventory.data.GetTotalCount(this.item_id);
    this._counter_label.text = "x" + this.total_count.ToString();
  }

  public void OnOver()
  {
    this.gamepad_frame.Activate();
    this._inventory_widget.OnItemOver(this.item_id);
  }
}
