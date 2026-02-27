// Decompiled with JetBrains decompiler
// Type: Structures_Refinery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class Structures_Refinery : StructureBrain, ITaskProvider
{
  public System.Action OnCompleteRefining;

  public float RefineryDuration(InventoryItem.ITEM_TYPE ItemType)
  {
    float num = this.Data.Type == StructureBrain.TYPES.REFINERY ? 1f : 0.75f;
    return ItemType == InventoryItem.ITEM_TYPE.BLACK_GOLD ? (float) (480.0 / (5.0 * (double) num) * 0.5) : (float) (480.0 / (5.0 * (double) num));
  }

  public static List<StructuresData.ItemCost> GetCost(InventoryItem.ITEM_TYPE Item, int variant = 0)
  {
    switch (Item)
    {
      case InventoryItem.ITEM_TYPE.BLACK_GOLD:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GOLD_NUGGET, 7)
        };
      case InventoryItem.ITEM_TYPE.FLOWER_RED:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FLOWER_PURPLE, 1),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.FLOWER_WHITE, 1)
        };
      case InventoryItem.ITEM_TYPE.LOG_REFINED:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LOG, 3)
        };
      case InventoryItem.ITEM_TYPE.STONE_REFINED:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 3)
        };
      case InventoryItem.ITEM_TYPE.ROPE:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.GRASS, 10)
        };
      case InventoryItem.ITEM_TYPE.GOLD_REFINED:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, 10)
        };
      case InventoryItem.ITEM_TYPE.SILK_THREAD:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.SPIDER_WEB, 3)
        };
      case InventoryItem.ITEM_TYPE.MAGMA_STONE:
        if (variant != 0)
          return new List<StructuresData.ItemCost>()
          {
            new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LIGHTNING_SHARD, 1),
            new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.STONE, 1)
          };
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.SOOT, 7)
        };
      case InventoryItem.ITEM_TYPE.YEW_HOLY:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.LIGHTNING_SHARD, 2),
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.YEW_CURSED, 2)
        };
      default:
        return new List<StructuresData.ItemCost>()
        {
          new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.NONE, 0)
        };
    }
  }

  public static int GetAmount(InventoryItem.ITEM_TYPE item)
  {
    return item == InventoryItem.ITEM_TYPE.BLACK_GOLD ? 5 : 1;
  }

  public FollowerTask GetOverrideTask(FollowerBrain brain) => (FollowerTask) null;

  public bool CheckOverrideComplete() => true;

  public void GetAvailableTasks(ScheduledActivity activity, SortedList<float, FollowerTask> tasks)
  {
    if (activity != ScheduledActivity.Work || this.ReservedForTask || this.Data.QueuedResources.Count <= 0)
      return;
    FollowerTask_Refinery followerTaskRefinery = new FollowerTask_Refinery(this.Data.ID);
    tasks.Add(followerTaskRefinery.Priorty, (FollowerTask) followerTaskRefinery);
  }

  public void RefineryDeposit()
  {
    if (this.Data.QueuedResources[0] == InventoryItem.ITEM_TYPE.YEW_HOLY)
      DataManager.Instance.RefinedElectrifiedRotstone = true;
    this.Data.Progress = 0.0f;
    this.DepositItem(this.Data.QueuedResources[0], Structures_Refinery.GetAmount(this.Data.QueuedResources[0]));
    this.Data.QueuedResources.RemoveAt(0);
    if (this.Data.QueuedRefineryVariants.Count > 0)
      this.Data.QueuedRefineryVariants.RemoveAt(0);
    System.Action completeRefining = this.OnCompleteRefining;
    if (completeRefining == null)
      return;
    completeRefining();
  }
}
