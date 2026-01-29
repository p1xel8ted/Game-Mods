// Decompiled with JetBrains decompiler
// Type: Structures_SiloFertiliser
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Structures_SiloFertiliser : StructureBrain
{
  public float Capacity => this.Data.Type == StructureBrain.TYPES.POOP_BUCKET ? 250f : 15f;

  public static Structures_SiloFertiliser GetClosestFertiliser(
    Vector3 fromPosition,
    FollowerLocation location)
  {
    List<Structures_SiloFertiliser> result = new List<Structures_SiloFertiliser>();
    StructureManager.TryGetAllStructuresOfType<Structures_SiloFertiliser>(ref result, location);
    Structures_SiloFertiliser closestFertiliser = (Structures_SiloFertiliser) null;
    foreach (Structures_SiloFertiliser structuresSiloFertiliser in result)
    {
      if (structuresSiloFertiliser.Data.Type != StructureBrain.TYPES.POOP_BUCKET && !structuresSiloFertiliser.Data.IsCollapsed && !structuresSiloFertiliser.Data.IsSnowedUnder && structuresSiloFertiliser.Data.Inventory.Count > 0 && structuresSiloFertiliser.Data.Inventory[0].quantity > 0 && (closestFertiliser == null || (double) Vector3.Distance(structuresSiloFertiliser.Data.Position, fromPosition) < (double) Vector3.Distance(closestFertiliser.Data.Position, fromPosition)))
      {
        if (SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter)
        {
          for (int index = 0; index < structuresSiloFertiliser.Data.Inventory.Count; ++index)
          {
            if (structuresSiloFertiliser.Data.Inventory[index].type == 187 && structuresSiloFertiliser.Data.Inventory[index].quantity > 0)
            {
              closestFertiliser = structuresSiloFertiliser;
              break;
            }
          }
        }
        else
          closestFertiliser = structuresSiloFertiliser;
      }
    }
    return closestFertiliser;
  }

  public int GetCompostCount()
  {
    int compostCount = 0;
    foreach (InventoryItem inventoryItem in this.Data.Inventory)
      compostCount += inventoryItem.quantity;
    return compostCount;
  }

  public List<InventoryItem> RemoveCompostAmount(int amount)
  {
    List<InventoryItem> inventoryItemList = new List<InventoryItem>();
    for (int index = this.Data.Inventory.Count - 1; index >= 0 && amount != 0; --index)
    {
      if (this.Data.Inventory[index].quantity <= amount)
      {
        inventoryItemList.Add(this.Data.Inventory[index]);
        amount -= this.Data.Inventory[index].quantity;
        this.Data.Inventory.RemoveAt(index);
      }
      else
      {
        InventoryItem inventoryItem = new InventoryItem((InventoryItem.ITEM_TYPE) this.Data.Inventory[index].type, amount);
        inventoryItemList.Add(inventoryItem);
        this.Data.Inventory[index].quantity -= amount;
        amount = 0;
      }
    }
    return inventoryItemList;
  }
}
