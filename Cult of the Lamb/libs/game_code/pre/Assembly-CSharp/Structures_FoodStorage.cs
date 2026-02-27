// Decompiled with JetBrains decompiler
// Type: Structures_FoodStorage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Structures_FoodStorage : StructureBrain
{
  public int Level;
  public System.Action OnFoodWithdrawn;

  public float Capacity => this.Level == 0 ? 5f : 15f;

  public float Range => this.Level != 0 ? 12f : 7f;

  public Structures_FoodStorage(int level) => this.Level = level;

  public int GetUnreservedItemCount()
  {
    int unreservedItemCount = 0;
    foreach (InventoryItem inventoryItem in this.Data.Inventory)
    {
      Debug.Log((object) $"item: {(object) (InventoryItem.ITEM_TYPE) inventoryItem.type}   {(object) inventoryItem.quantity}   {(object) inventoryItem.UnreservedQuantity}");
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

  public static Structures_FoodStorage GetAvailableFoodStorage(
    Vector3 fromPosition,
    FollowerLocation location)
  {
    List<Structures_FoodStorage> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_FoodStorage>(location);
    List<Structures_FoodStorage> structuresFoodStorageList = new List<Structures_FoodStorage>();
    foreach (Structures_FoodStorage structuresFoodStorage in structuresOfType)
    {
      BoxCollider2D boxCollider2D = GameManager.GetInstance().GetComponent<BoxCollider2D>();
      if ((UnityEngine.Object) boxCollider2D == (UnityEngine.Object) null)
      {
        boxCollider2D = GameManager.GetInstance().gameObject.AddComponent<BoxCollider2D>();
        boxCollider2D.isTrigger = true;
      }
      boxCollider2D.size = Vector2.one * structuresFoodStorage.Range;
      boxCollider2D.transform.position = structuresFoodStorage.Data.Position;
      boxCollider2D.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -45f));
      if ((double) structuresFoodStorage.Data.Inventory.Count < (double) structuresFoodStorage.Capacity && boxCollider2D.OverlapPoint((Vector2) fromPosition))
        structuresFoodStorageList.Add(structuresFoodStorage);
    }
    return structuresFoodStorageList.Count <= 0 ? (Structures_FoodStorage) null : structuresFoodStorageList[UnityEngine.Random.Range(0, structuresFoodStorageList.Count)];
  }
}
