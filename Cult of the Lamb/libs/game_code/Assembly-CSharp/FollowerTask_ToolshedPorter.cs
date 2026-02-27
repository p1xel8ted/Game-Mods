// Decompiled with JetBrains decompiler
// Type: FollowerTask_ToolshedPorter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerTask_ToolshedPorter(
  int logisticsStructure,
  int rootStructure,
  int targetStructure) : FollowerTask_LogisticsPorter(logisticsStructure, rootStructure, targetStructure)
{
  public override List<InventoryItem> CollectItemsFromStructure(StructureBrain structureBrain)
  {
    List<InventoryItem> inventoryItemList = new List<InventoryItem>();
    Structures_Toolshed structureById = StructureManager.GetStructureByID<Structures_Toolshed>(this.targetStucture);
    int num = Mathf.Max(structureById.Capacity - structureById.Data.Inventory.Count, 0);
    for (int index1 = 0; index1 < structureBrain.Data.Inventory.Count; ++index1)
    {
      for (int index2 = 0; index2 < structureBrain.Data.Inventory[index1].quantity && num > inventoryItemList.Count; ++index2)
      {
        if (structureBrain.Data.Inventory[index1].type == 1)
          inventoryItemList.Add(new InventoryItem((InventoryItem.ITEM_TYPE) structureBrain.Data.Inventory[index1].type, 1));
        else if (structureBrain.Data.Inventory[index1].type == 2)
          inventoryItemList.Add(new InventoryItem((InventoryItem.ITEM_TYPE) structureBrain.Data.Inventory[index1].type, 1));
      }
    }
    for (int index = 0; index < inventoryItemList.Count; ++index)
      structureBrain.RemoveItems((InventoryItem.ITEM_TYPE) inventoryItemList[index].type, inventoryItemList[index].quantity);
    return inventoryItemList;
  }

  public override List<InventoryItem> DepositItems(StructureBrain structureBrain)
  {
    for (int index1 = this.heldItems.Count - 1; index1 >= 0 && structureBrain.Data.Inventory.Count < ((Structures_Toolshed) structureBrain).Capacity; --index1)
    {
      for (int index2 = 0; index2 < this.heldItems[index1].quantity; ++index2)
        structureBrain.DepositItemUnstacked((InventoryItem.ITEM_TYPE) this.heldItems[index1].type);
      this.heldItems.RemoveAt(index1);
    }
    return this.heldItems;
  }

  public override void Loop(List<InventoryItem> leftovers) => base.Loop(leftovers);
}
