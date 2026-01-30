// Decompiled with JetBrains decompiler
// Type: FollowerTask_WoolyShackPorter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerTask_WoolyShackPorter(
  int logisticsStructure,
  int rootStructure,
  int targetStructure) : FollowerTask_LogisticsPorter(logisticsStructure, rootStructure, targetStructure)
{
  public override List<InventoryItem> CollectItemsFromStructure(StructureBrain structureBrain)
  {
    List<InventoryItem> inventoryItemList = new List<InventoryItem>();
    for (int index = 0; index < structureBrain.Data.Inventory.Count; ++index)
    {
      if (structureBrain.Data.Inventory[index].type == 165)
        inventoryItemList.Add(new InventoryItem((InventoryItem.ITEM_TYPE) structureBrain.Data.Inventory[index].type, structureBrain.Data.Inventory[index].quantity));
    }
    for (int index = 0; index < inventoryItemList.Count; ++index)
      structureBrain.RemoveItems((InventoryItem.ITEM_TYPE) inventoryItemList[index].type, inventoryItemList[index].quantity);
    return inventoryItemList;
  }

  public override List<InventoryItem> DepositItems(StructureBrain structureBrain)
  {
    for (int index = this.heldItems.Count - 1; index >= 0; --index)
    {
      while (this.heldItems[index].quantity >= 5)
      {
        if ((Object) Interaction_WoolyShack.Instance != (Object) null)
          Interaction_WoolyShack.Instance.Craft();
        else
          structureBrain.DepositItem(InventoryItem.ITEM_TYPE.NONE);
        this.heldItems[index].quantity -= 5;
        if (this.heldItems[index].quantity <= 0)
        {
          this.heldItems.RemoveAt(index);
          break;
        }
      }
    }
    return this.heldItems;
  }

  public override void Loop(List<InventoryItem> leftovers) => base.Loop(leftovers);
}
