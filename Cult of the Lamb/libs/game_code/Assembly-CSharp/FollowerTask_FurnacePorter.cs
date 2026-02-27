// Decompiled with JetBrains decompiler
// Type: FollowerTask_FurnacePorter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerTask_FurnacePorter : FollowerTask_FuelPorter
{
  public FollowerTask_FurnacePorter(int logisticsStructure, int rootStructure, int targetStructure)
    : base(logisticsStructure, rootStructure, targetStructure)
  {
    this.fuel = InventoryItem.ITEM_TYPE.MAGMA_STONE;
  }

  public override List<InventoryItem> DepositItems(StructureBrain structureBrain)
  {
    int spawnAmount = 0;
    for (int index = this.heldItems.Count - 1; index >= 0; --index)
    {
      structureBrain.Data.Fuel = Mathf.Clamp(structureBrain.Data.Fuel + InventoryItem.FuelWeight((InventoryItem.ITEM_TYPE) this.heldItems[index].type), 0, structureBrain.Data.MaxFuel);
      this.heldItems.RemoveAt(index);
      ++spawnAmount;
      if (structureBrain.Data.MaxFuel - structureBrain.Data.Fuel <= 0)
      {
        structureBrain.Data.FullyFueled = true;
        break;
      }
    }
    if (PlayerFarming.Location == FollowerLocation.Base)
    {
      foreach (Interaction_DLCFurnace furnace in Interaction_DLCFurnace.Furnaces)
      {
        if (furnace.Structure.Brain == structureBrain && spawnAmount >= 1)
        {
          furnace.AnimateRoutine((float) spawnAmount);
          break;
        }
      }
    }
    else if (spawnAmount >= 1)
    {
      if (structureBrain.Data.Type == StructureBrain.TYPES.FURNACE_3)
      {
        for (int index = 0; index < Random.Range(6, 10); ++index)
          structureBrain.DepositItemUnstacked(InventoryItem.ITEM_TYPE.SOOT);
      }
      else
      {
        double num = (double) Random.value;
      }
    }
    return this.heldItems;
  }
}
