// Decompiled with JetBrains decompiler
// Type: Structures_Medic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class Structures_Medic : StructureBrain, ITaskProvider
{
  public int Capacity => 100;

  public bool CheckOverrideComplete() => false;

  public void GetAvailableTasks(
    ScheduledActivity activity,
    SortedList<float, FollowerTask> sortedTasks)
  {
    if (activity != ScheduledActivity.Work || this.ReservedForTask)
      return;
    List<Structures_HealingBay> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_HealingBay>();
    List<int> targetFollowers = FollowerTask_Medic.GetTargetFollowers();
    if (targetFollowers.Count <= 0 || structuresOfType.Count <= 0)
      return;
    for (int index = 0; index < targetFollowers.Count; ++index)
    {
      FollowerBrain brain = FollowerBrain.GetOrCreateBrain(FollowerInfo.GetInfoByID(targetFollowers[index]));
      if (brain != null && brain.CurrentTaskType != FollowerTaskType.ManualControl && this.HasEnoughResources(Interaction_HealingBay.GetCost(brain, structuresOfType[0].Data.Type == StructureBrain.TYPES.HEALING_BAY_2)) && !structuresOfType[0].ReservedByPlayer)
      {
        FollowerTask_Medic followerTaskMedic = new FollowerTask_Medic(this.Data.ID, targetFollowers[index]);
        sortedTasks.Add(followerTaskMedic.Priorty, (FollowerTask) followerTaskMedic);
        break;
      }
    }
  }

  public FollowerTask GetOverrideTask(FollowerBrain brain) => (FollowerTask) null;

  public bool HasEnoughResources(List<InventoryItem> cost)
  {
    List<InventoryItem> inventoryItemList = new List<InventoryItem>();
    for (int index = 0; index < cost.Count; ++index)
      inventoryItemList.Add(new InventoryItem((InventoryItem.ITEM_TYPE) cost[index].type, cost[index].quantity));
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
