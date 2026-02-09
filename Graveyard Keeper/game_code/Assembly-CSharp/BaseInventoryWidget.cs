// Decompiled with JetBrains decompiler
// Type: BaseInventoryWidget
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BaseInventoryWidget : MonoBehaviour
{
  public GameObject header_container;
  public UIWidget table_widget;
  public UIWidget table_back_widget;
  public UILabel header_label;
  public Inventory inventory;
  public bool initialized;
  public bool for_gamepad;
  public BaseInventoryWidget.InventoryType type;
  public int _opened_at_frame;

  public Item inventory_data => this.inventory == null ? (Item) null : this.inventory.data;

  public bool just_opened => Time.frameCount - this._opened_at_frame <= 1;

  public virtual void Init()
  {
  }

  public virtual void Open(
    Inventory inventory,
    bool for_gamepad,
    BaseInventoryWidget.InventoryType type = BaseInventoryWidget.InventoryType.None)
  {
    if (!this.initialized)
      this.Init();
    this._opened_at_frame = Time.frameCount;
    this.inventory = inventory;
    this.for_gamepad = for_gamepad;
    this.type = type;
    if (!((Object) this.header_label != (Object) null))
      return;
    this.header_label.text = inventory.name;
  }

  public virtual void Redraw()
  {
  }

  public virtual void SetCustomNavigationTarget(BaseInventoryWidget widget, Direction direction)
  {
  }

  public virtual GamepadNavigationItem GetFirstNavigationItem(Direction dir)
  {
    return this.GetComponentInChildren<GamepadNavigationItem>();
  }

  public void SetMain() => this.type = BaseInventoryWidget.InventoryType.Main;

  public bool IsMain() => this.type == BaseInventoryWidget.InventoryType.Main;

  public bool IsCustom() => this.type == BaseInventoryWidget.InventoryType.Custom;

  public enum InventoryType
  {
    None,
    Custom,
    Main,
  }
}
