// Decompiled with JetBrains decompiler
// Type: FollowerTask_SiloSeedPorter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class FollowerTask_SiloSeedPorter(
  int logisticsStructure,
  int rootStructure,
  int targetStructure) : FollowerTask_LogisticsPorter(logisticsStructure, rootStructure, targetStructure)
{
  public override List<InventoryItem> CollectItemsFromStructure(StructureBrain structureBrain)
  {
    List<InventoryItem> inventoryItemList = new List<InventoryItem>();
    for (int index = 0; index < structureBrain.Data.Inventory.Count; ++index)
    {
      if (InventoryItem.AllSeeds.Contains((InventoryItem.ITEM_TYPE) structureBrain.Data.Inventory[index].type) && structureBrain.Data.Inventory[index].type != 35)
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
      if ((double) structureBrain.Data.Inventory.Count < (double) ((Structures_SiloSeed) structureBrain).Capacity)
      {
        structureBrain.DepositItem((InventoryItem.ITEM_TYPE) this.heldItems[index].type, this.heldItems[index].quantity);
        this.heldItems.RemoveAt(index);
      }
    }
    return this.heldItems;
  }

  public override void Loop(List<InventoryItem> leftovers) => base.Loop(leftovers);
}
