// Decompiled with JetBrains decompiler
// Type: Structures_FoodStorage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Structures_FoodStorage : StructureBrain
{
  public System.Action OnFoodWithdrawn;

  public int Capacity => 10;

  public Structures_FoodStorage(int level)
  {
  }

  public int GetUnreservedItemCount()
  {
    int unreservedItemCount = 0;
    foreach (InventoryItem inventoryItem in this.Data.Inventory)
    {
      Debug.Log((object) $"item: {((InventoryItem.ITEM_TYPE) inventoryItem.type).ToString()}   {inventoryItem.quantity.ToString()}   {inventoryItem.UnreservedQuantity.ToString()}");
      unreservedItemCount += inventoryItem.UnreservedQuantity;
    }
    return unreservedItemCount;
  }

  public bool TryClaimFoodReservation(out InventoryItem.ITEM_TYPE itemType)
  {
    foreach (InventoryItem inventoryItem in this.Data.Inventory)
    {
      Debug.Log((object) (InventoryItem.ITEM_TYPE) inventoryItem.type);
      if (inventoryItem.UnreservedQuantity > 0)
      {
        ++inventoryItem.QuantityReserved;
        itemType = (InventoryItem.ITEM_TYPE) inventoryItem.type;
        return true;
      }
    }
    itemType = InventoryItem.ITEM_TYPE.NONE;
    return false;
  }

  public bool TryClaimFoodReservation(InventoryItem.ITEM_TYPE itemType)
  {
    foreach (InventoryItem inventoryItem in this.Data.Inventory)
    {
      if ((InventoryItem.ITEM_TYPE) inventoryItem.type == itemType && inventoryItem.UnreservedQuantity > 0)
      {
        ++inventoryItem.QuantityReserved;
        itemType = (InventoryItem.ITEM_TYPE) inventoryItem.type;
        return true;
      }
    }
    return false;
  }

  public void ReleaseFoodReservation(InventoryItem.ITEM_TYPE itemType)
  {
    bool flag = false;
    foreach (InventoryItem inventoryItem in this.Data.Inventory)
    {
      if ((InventoryItem.ITEM_TYPE) inventoryItem.type == itemType && inventoryItem.QuantityReserved > 0)
      {
        --inventoryItem.QuantityReserved;
        flag = true;
        break;
      }
    }
    int num = flag ? 1 : 0;
  }

  public bool TryEatReservedFood(InventoryItem.ITEM_TYPE itemType)
  {
    for (int index = this.Data.Inventory.Count - 1; index >= 0; --index)
    {
      if ((InventoryItem.ITEM_TYPE) this.Data.Inventory[index].type == itemType && this.Data.Inventory[index].quantity > 0 && this.Data.Inventory[index].QuantityReserved > 0)
      {
        --this.Data.Inventory[index].QuantityReserved;
        --this.Data.Inventory[index].quantity;
        if (this.Data.Inventory[index].quantity == 0)
          this.Data.Inventory.RemoveAt(index);
        System.Action onFoodWithdrawn = this.OnFoodWithdrawn;
        if (onFoodWithdrawn != null)
          onFoodWithdrawn();
        return true;
      }
    }
    return false;
  }
}
