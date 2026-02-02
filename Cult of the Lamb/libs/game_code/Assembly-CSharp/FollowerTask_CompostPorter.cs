// Decompiled with JetBrains decompiler
// Type: FollowerTask_CompostPorter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerTask_CompostPorter(
  int logisticsStructure,
  int rootStructure,
  int targetStructure) : FollowerTask_LogisticsPorter(logisticsStructure, rootStructure, targetStructure)
{
  public override List<InventoryItem> CollectItemsFromStructure(StructureBrain structureBrain)
  {
    List<InventoryItem> inventoryItemList = new List<InventoryItem>();
    for (int index = 0; index < structureBrain.Data.Inventory.Count; ++index)
    {
      if (structureBrain.Data.Inventory[index].type == 35 && inventoryItemList.Count < 50)
        inventoryItemList.Add(new InventoryItem((InventoryItem.ITEM_TYPE) structureBrain.Data.Inventory[index].type, Mathf.Min(structureBrain.Data.Inventory[index].quantity, 50)));
    }
    for (int index = 0; index < inventoryItemList.Count; ++index)
      structureBrain.RemoveItems((InventoryItem.ITEM_TYPE) inventoryItemList[index].type, Mathf.Min(inventoryItemList[index].quantity, 50));
    return inventoryItemList;
  }

  public override List<InventoryItem> DepositItems(StructureBrain structureBrain)
  {
    for (int index = this.heldItems.Count - 1; index >= 0; --index)
    {
      if (structureBrain.Data.Inventory.Count < ((Structures_CompostBin) structureBrain).CompostCost)
      {
        structureBrain.DepositItem((InventoryItem.ITEM_TYPE) this.heldItems[index].type, this.heldItems[index].quantity);
        this.heldItems.RemoveAt(index);
      }
    }
    structureBrain.Data.Progress = TimeManager.TotalElapsedGameTime;
    ((Structures_CompostBin) structureBrain).UpdateCompostState();
    return this.heldItems;
  }

  public override void Loop(List<InventoryItem> leftovers) => base.Loop(leftovers);
}
