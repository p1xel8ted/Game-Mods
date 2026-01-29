// Decompiled with JetBrains decompiler
// Type: FollowerTask_MedicPorter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class FollowerTask_MedicPorter(
  int logisticsStructure,
  int rootStructure,
  int targetStructure) : FollowerTask_LogisticsPorter(logisticsStructure, rootStructure, targetStructure)
{
  public override List<InventoryItem> CollectItemsFromStructure(StructureBrain structureBrain)
  {
    List<InventoryItem> inventoryItemList = new List<InventoryItem>();
    for (int index = 0; index < structureBrain.Data.Inventory.Count; ++index)
    {
      if (structureBrain.Data.Inventory[index].type == 55)
        inventoryItemList.Add(new InventoryItem((InventoryItem.ITEM_TYPE) structureBrain.Data.Inventory[index].type, structureBrain.Data.Inventory[index].quantity));
    }
    for (int index = 0; index < inventoryItemList.Count; ++index)
      structureBrain.RemoveItems((InventoryItem.ITEM_TYPE) inventoryItemList[index].type, inventoryItemList[index].quantity);
    return inventoryItemList;
  }

  public override List<InventoryItem> DepositItems(StructureBrain structureBrain)
  {
    for (int index1 = this.heldItems.Count - 1; index1 >= 0; --index1)
    {
      if (structureBrain.Data.Inventory.Count < ((Structures_Medic) structureBrain).Capacity)
      {
        for (int index2 = 0; index2 < this.heldItems[index1].quantity; ++index2)
          structureBrain.DepositItemUnstacked((InventoryItem.ITEM_TYPE) this.heldItems[index1].type);
        this.heldItems.RemoveAt(index1);
      }
    }
    return this.heldItems;
  }

  public override void Loop(List<InventoryItem> leftovers) => base.Loop(leftovers);
}
