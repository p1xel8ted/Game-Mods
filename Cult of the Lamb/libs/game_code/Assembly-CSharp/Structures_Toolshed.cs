// Decompiled with JetBrains decompiler
// Type: Structures_Toolshed
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class Structures_Toolshed : StructureBrain, ITaskProvider
{
  public int Capacity => 20;

  public bool CheckOverrideComplete() => false;

  public void GetAvailableTasks(
    ScheduledActivity activity,
    SortedList<float, FollowerTask> sortedTasks)
  {
    if (activity != ScheduledActivity.Work || this.ReservedForTask)
      return;
    List<StructureBrain> ts = new List<StructureBrain>();
    foreach (StructureBrain allBrain in StructureBrain.AllBrains)
    {
      if (allBrain.Data.IsCollapsed || allBrain.Data.Exhausted)
      {
        if (this.HasEnoughResources(new List<InventoryItem>()
        {
          new InventoryItem(StructuresData.GetBuildRubbleType(allBrain.Data.Type, true), 3)
        }))
          ts.Add(allBrain);
      }
    }
    if (ts.Count <= 0)
      return;
    ts.Shuffle<StructureBrain>();
    FollowerTask_Handyman followerTaskHandyman = new FollowerTask_Handyman(this.Data.ID, ts[0].Data.ID);
    sortedTasks.Add(followerTaskHandyman.Priorty, (FollowerTask) followerTaskHandyman);
  }

  public FollowerTask GetOverrideTask(FollowerBrain brain) => (FollowerTask) null;

  public bool HasEnoughResources(List<InventoryItem> cost)
  {
    List<InventoryItem> inventoryItemList = new List<InventoryItem>((IEnumerable<InventoryItem>) cost);
    for (int index1 = 0; index1 < this.Data.Inventory.Count; ++index1)
    {
      for (int index2 = 0; index2 < inventoryItemList.Count; ++index2)
      {
        if (inventoryItemList[index2].type == this.Data.Inventory[index1].type && inventoryItemList[index2].quantity > 0)
          --inventoryItemList[index2].quantity;
      }
    }
    for (int index = 0; index < inventoryItemList.Count; ++index)
    {
      if (inventoryItemList[index].quantity > 0)
        return false;
    }
    return true;
  }

  public void RemoveItems(List<InventoryItem> cost)
  {
    List<InventoryItem> inventoryItemList = new List<InventoryItem>((IEnumerable<InventoryItem>) cost);
    for (int index1 = 0; index1 < inventoryItemList.Count; ++index1)
    {
      for (int index2 = 0; index2 < inventoryItemList[index1].quantity; ++index2)
      {
        this.RemoveItem((InventoryItem.ITEM_TYPE) inventoryItemList[index1].type);
        --inventoryItemList[index1].quantity;
      }
    }
  }

  public void RemoveItem(InventoryItem.ITEM_TYPE itemType)
  {
    for (int index = 0; index < this.Data.Inventory.Count; ++index)
    {
      if ((InventoryItem.ITEM_TYPE) this.Data.Inventory[index].type == itemType)
      {
        this.Data.Inventory.RemoveAt(index);
        break;
      }
    }
  }
}
