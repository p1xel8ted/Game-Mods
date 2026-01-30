// Decompiled with JetBrains decompiler
// Type: FollowerTask_RefineryPorter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerTask_RefineryPorter(
  int logisticsStructure,
  int rootStructure,
  int targetStructure) : FollowerTask_LogisticsPorter(logisticsStructure, rootStructure, targetStructure)
{
  public override List<InventoryItem> CollectItemsFromStructure(StructureBrain structureBrain)
  {
    List<InventoryItem> inventoryItemList = new List<InventoryItem>();
    if (structureBrain.Data.Inventory.Count == 0)
      return inventoryItemList;
    StructureBrain structureById = StructureManager.GetStructureByID<StructureBrain>(this.targetStucture);
    foreach (InventoryItem inventoryItem in structureBrain.Data.Inventory)
    {
      InventoryItem.ITEM_TYPE itemType;
      if (inventoryItem.type == 20)
        itemType = InventoryItem.ITEM_TYPE.GOLD_REFINED;
      else if (inventoryItem.type == 1)
        itemType = InventoryItem.ITEM_TYPE.LOG_REFINED;
      else if (inventoryItem.type == 2)
        itemType = InventoryItem.ITEM_TYPE.STONE_REFINED;
      else if (inventoryItem.type == 90)
        itemType = InventoryItem.ITEM_TYPE.SILK_THREAD;
      else if (inventoryItem.type == 186)
        itemType = InventoryItem.ITEM_TYPE.MAGMA_STONE;
      else
        continue;
      int costValue = Structures_Refinery.GetCost(itemType)[0].CostValue;
      if (structureById != null)
      {
        int num1 = Mathf.Max((structureById.Data.Type == StructureBrain.TYPES.REFINERY ? 5 : 10) - structureBrain.Data.QueuedResources.Count, 0);
        for (int index1 = 0; index1 < structureBrain.Data.Inventory.Count; ++index1)
        {
          if (inventoryItemList.Count == 0 || (double) inventoryItemList.Count / (double) costValue < (double) num1)
          {
            for (int index2 = 0; index2 < structureBrain.Data.Inventory[index1].quantity; ++index2)
            {
              if (structureBrain.Data.Inventory[index1].type == inventoryItem.type)
                inventoryItemList.Add(new InventoryItem((InventoryItem.ITEM_TYPE) structureBrain.Data.Inventory[index1].type, 1));
            }
          }
        }
        int num2 = inventoryItemList.Count % costValue;
        for (int index = 0; index < num2; ++index)
          inventoryItemList.RemoveAt(inventoryItemList.Count - 1);
        for (int index = 0; index < inventoryItemList.Count; ++index)
          structureBrain.RemoveItems((InventoryItem.ITEM_TYPE) inventoryItemList[index].type, inventoryItemList[index].quantity);
      }
      return inventoryItemList;
    }
    return new List<InventoryItem>();
  }

  public override List<InventoryItem> DepositItems(StructureBrain structureBrain)
  {
    InventoryItem.ITEM_TYPE itemType = InventoryItem.ITEM_TYPE.LOG_REFINED;
    if (this.heldItems[0].type == 2)
      itemType = InventoryItem.ITEM_TYPE.STONE_REFINED;
    else if (this.heldItems[0].type == 90)
      itemType = InventoryItem.ITEM_TYPE.SILK_THREAD;
    else if (this.heldItems[0].type == 186)
      itemType = InventoryItem.ITEM_TYPE.MAGMA_STONE;
    else if (this.heldItems[0].type == 20)
      itemType = InventoryItem.ITEM_TYPE.GOLD_REFINED;
    int costValue = Structures_Refinery.GetCost(itemType)[0].CostValue;
    while (this.heldItems.Count >= costValue && structureBrain.Data.QueuedResources.Count < (structureBrain.Data.Type == StructureBrain.TYPES.REFINERY ? 5 : 10))
    {
      structureBrain.Data.QueuedResources.Add(itemType);
      for (int index = 0; index < costValue; ++index)
        this.heldItems.RemoveAt(this.heldItems.Count - 1);
    }
    foreach (Interaction_Refinery refinery in Interaction_Refinery.Refineries)
      refinery.CheckPhase();
    return this.heldItems;
  }

  public override void Loop(List<InventoryItem> leftovers) => base.Loop(leftovers);
}
