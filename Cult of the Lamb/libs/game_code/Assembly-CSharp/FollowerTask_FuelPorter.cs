// Decompiled with JetBrains decompiler
// Type: FollowerTask_FuelPorter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerTask_FuelPorter(
  int logisticsStructure,
  int rootStructure,
  int targetStructure) : FollowerTask_LogisticsPorter(logisticsStructure, rootStructure, targetStructure)
{
  public InventoryItem.ITEM_TYPE fuel = InventoryItem.ITEM_TYPE.LOG;

  public override List<InventoryItem> CollectItemsFromStructure(StructureBrain structureBrain)
  {
    List<InventoryItem> inventoryItemList = new List<InventoryItem>();
    StructureBrain structureById = StructureManager.GetStructureByID<StructureBrain>(this.targetStucture);
    int num = Mathf.Max(structureById.Data.MaxFuel - structureById.Data.Fuel, 0);
    for (int index1 = 0; index1 < structureBrain.Data.Inventory.Count; ++index1)
    {
      for (int index2 = 0; index2 < structureBrain.Data.Inventory[index1].quantity && num > inventoryItemList.Count * InventoryItem.FuelWeight(this.fuel); ++index2)
      {
        if ((InventoryItem.ITEM_TYPE) structureBrain.Data.Inventory[index1].type == this.fuel)
          inventoryItemList.Add(new InventoryItem((InventoryItem.ITEM_TYPE) structureBrain.Data.Inventory[index1].type, 1));
      }
    }
    for (int index = 0; index < inventoryItemList.Count; ++index)
      structureBrain.RemoveItems((InventoryItem.ITEM_TYPE) inventoryItemList[index].type, inventoryItemList[index].quantity);
    return inventoryItemList;
  }

  public override List<InventoryItem> DepositItems(StructureBrain structureBrain)
  {
    for (int index = this.heldItems.Count - 1; index >= 0; --index)
    {
      structureBrain.Data.Fuel = Mathf.Clamp(structureBrain.Data.Fuel + InventoryItem.FuelWeight((InventoryItem.ITEM_TYPE) this.heldItems[index].type), 0, structureBrain.Data.MaxFuel);
      this.heldItems.RemoveAt(index);
      if (structureBrain.Data.MaxFuel - structureBrain.Data.Fuel <= 0)
      {
        structureBrain.Data.FullyFueled = true;
        break;
      }
    }
    return this.heldItems;
  }
}
