// Decompiled with JetBrains decompiler
// Type: Structures_SiloSeed
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Structures_SiloSeed : StructureBrain
{
  public float Capacity => this.Data.Type == StructureBrain.TYPES.SEED_BUCKET ? 250f : 15f;

  public static Structures_SiloSeed GetClosestSeeder(
    Vector3 fromPosition,
    FollowerLocation location,
    InventoryItem.ITEM_TYPE prioritisedSeedType = InventoryItem.ITEM_TYPE.NONE,
    List<Structures_SiloSeed> silos = null)
  {
    if (silos == null)
      StructureManager.TryGetAllStructuresOfType<Structures_SiloSeed>(ref silos, location);
    Structures_SiloSeed closestSeeder = (Structures_SiloSeed) null;
    foreach (Structures_SiloSeed silo in silos)
    {
      if (silo.Data.Type != StructureBrain.TYPES.SEED_BUCKET && !silo.Data.IsCollapsed && !silo.Data.IsSnowedUnder)
      {
        if (prioritisedSeedType != InventoryItem.ITEM_TYPE.NONE)
        {
          foreach (InventoryItem inventoryItem in silo.Data.Inventory)
          {
            if ((InventoryItem.ITEM_TYPE) inventoryItem.type == prioritisedSeedType && inventoryItem.UnreservedQuantity > 0)
              return silo;
          }
        }
        else if (silo.Data.Inventory.Count > 0 && silo.Data.Inventory[0].quantity > 0 && (closestSeeder == null || (double) Vector3.Distance(silo.Data.Position, fromPosition) < (double) Vector3.Distance(closestSeeder.Data.Position, fromPosition)))
          closestSeeder = silo;
      }
    }
    return closestSeeder;
  }

  public int GetCompostCount()
  {
    int compostCount = 0;
    foreach (InventoryItem inventoryItem in this.Data.Inventory)
      compostCount += inventoryItem.quantity;
    return compostCount;
  }

  public List<InventoryItem.ITEM_TYPE> GetCompost(int amount)
  {
    List<InventoryItem.ITEM_TYPE> compost = new List<InventoryItem.ITEM_TYPE>();
    for (int index = 0; index < Mathf.Min(amount, this.Data.Inventory.Count); ++index)
      compost.Add((InventoryItem.ITEM_TYPE) this.Data.Inventory[index].type);
    return compost;
  }

  public void RemoveCompost(List<InventoryItem.ITEM_TYPE> items)
  {
    for (int index = this.Data.Inventory.Count - 1; index >= 0; --index)
    {
      if (items.Contains((InventoryItem.ITEM_TYPE) this.Data.Inventory[index].type))
      {
        items.Remove((InventoryItem.ITEM_TYPE) this.Data.Inventory[index].type);
        this.Data.Inventory.RemoveAt(index);
      }
    }
  }
}
