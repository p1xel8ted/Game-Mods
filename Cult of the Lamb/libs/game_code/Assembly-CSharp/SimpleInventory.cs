// Decompiled with JetBrains decompiler
// Type: SimpleInventory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class SimpleInventory : BaseMonoBehaviour
{
  public InventoryItemDisplay ItemImage;
  public InventoryItem.ITEM_TYPE inventoryitem;
  public static List<SimpleInventory> simpleInventories = new List<SimpleInventory>();
  public static int _TotalItemsActive;

  public virtual InventoryItem.ITEM_TYPE Item
  {
    get => this.inventoryitem;
    set
    {
      this.inventoryitem = value;
      this.ItemImage.SetImage(this.Item);
    }
  }

  public static int TotalItemsActive
  {
    get
    {
      SimpleInventory._TotalItemsActive = 0;
      foreach (SimpleInventory simpleInventory in SimpleInventory.simpleInventories)
      {
        if (simpleInventory.Item != InventoryItem.ITEM_TYPE.NONE)
          ++SimpleInventory._TotalItemsActive;
      }
      return SimpleInventory._TotalItemsActive;
    }
  }

  public void OnEnable() => SimpleInventory.simpleInventories.Add(this);

  public void OnDisable() => SimpleInventory.simpleInventories.Remove(this);

  public void Start()
  {
    this.Item = InventoryItem.ITEM_TYPE.NONE;
    this.ItemImage.SetImage(this.Item);
  }

  public InventoryItem.ITEM_TYPE GetItemType() => this.Item;

  public void GiveItem(InventoryItem.ITEM_TYPE ItemType)
  {
    this.DropItem();
    this.Item = ItemType;
    this.ItemImage.SetImage(this.Item);
  }

  public void RemoveItem()
  {
    this.Item = InventoryItem.ITEM_TYPE.NONE;
    this.ItemImage.SetImage(this.Item);
  }

  public void DropItem()
  {
    if (this.Item == InventoryItem.ITEM_TYPE.NONE)
      return;
    InventoryItem.Spawn(this.Item, 1, this.ItemImage.transform.position);
    this.RemoveItem();
  }
}
