// Decompiled with JetBrains decompiler
// Type: FollowerTask_TroughPorter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class FollowerTask_TroughPorter(
  int logisticsStructure,
  int rootStructure,
  int targetStructure) : FollowerTask_LogisticsPorter(logisticsStructure, rootStructure, targetStructure)
{
  public override List<InventoryItem> CollectItemsFromStructure(StructureBrain structureBrain)
  {
    List<InventoryItem> inventoryItemList = new List<InventoryItem>();
    for (int index = 0; index < structureBrain.Data.Inventory.Count; ++index)
    {
      if (structureBrain.Data.Inventory[index].type == 35)
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
      if (structureBrain.Data.Inventory.Count < ((Structures_RanchTrough) structureBrain).Capacity)
      {
        structureBrain.DepositItem((InventoryItem.ITEM_TYPE) this.heldItems[index].type, this.heldItems[index].quantity);
        this.heldItems.RemoveAt(index);
      }
    }
    return this.heldItems;
  }

  public override void Loop(List<InventoryItem> leftovers) => base.Loop(leftovers);
}
