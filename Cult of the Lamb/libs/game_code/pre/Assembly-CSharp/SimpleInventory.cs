// Decompiled with JetBrains decompiler
// Type: SimpleInventory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class SimpleInventory : BaseMonoBehaviour
{
  public InventoryItemDisplay ItemImage;
  public InventoryItem.ITEM_TYPE inventoryitem;
  private static List<SimpleInventory> simpleInventories = new List<SimpleInventory>();
  private static int _TotalItemsActive;

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

  private void OnEnable() => SimpleInventory.simpleInventories.Add(this);

  private void OnDisable() => SimpleInventory.simpleInventories.Remove(this);

  private void Start()
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
