// Decompiled with JetBrains decompiler
// Type: Structures_OfferingShrine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Structures_OfferingShrine : StructureBrain, ITaskProvider
{
  public Action<Vector3> OnCompleteOfferingShrine;
  public const float PRAY_DURATION = 30f;
  public List<InventoryItem.ITEM_TYPE> Offerings = new List<InventoryItem.ITEM_TYPE>()
  {
    InventoryItem.ITEM_TYPE.BLACK_GOLD,
    InventoryItem.ITEM_TYPE.GOLD_NUGGET,
    InventoryItem.ITEM_TYPE.MEAT,
    InventoryItem.ITEM_TYPE.FISH_BIG,
    InventoryItem.ITEM_TYPE.FISH,
    InventoryItem.ITEM_TYPE.LOG,
    InventoryItem.ITEM_TYPE.STONE
  };
  public List<InventoryItem.ITEM_TYPE> RareOfferings = new List<InventoryItem.ITEM_TYPE>()
  {
    InventoryItem.ITEM_TYPE.GOLD_REFINED,
    InventoryItem.ITEM_TYPE.LOG_REFINED,
    InventoryItem.ITEM_TYPE.STONE_REFINED
  };

  public FollowerTask GetOverrideTask(FollowerBrain brain) => (FollowerTask) null;

  public bool CheckOverrideComplete() => true;

  public void GetAvailableTasks(ScheduledActivity activity, SortedList<float, FollowerTask> tasks)
  {
    if (activity != ScheduledActivity.Work || this.ReservedForTask || this.Data.Inventory.Count > 0 || (double) TimeManager.TotalElapsedGameTime - (double) this.Data.LastPrayTime <= 360.0)
      return;
    FollowerTask_OfferingShrine taskOfferingShrine = new FollowerTask_OfferingShrine(this.Data.ID);
    tasks.Add(taskOfferingShrine.Priorty, (FollowerTask) taskOfferingShrine);
  }

  public void CollectOffering() => this.Data.LastPrayTime = TimeManager.TotalElapsedGameTime;

  public void Complete(Vector3 FollowerPosition)
  {
    this.Data.Progress = 0.0f;
    InventoryItem.ITEM_TYPE type;
    int quantity;
    if ((double) UnityEngine.Random.value <= 0.30000001192092896 && UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Economy_Refinery))
    {
      type = this.RareOfferings[UnityEngine.Random.Range(0, this.RareOfferings.Count)];
      quantity = UnityEngine.Random.Range(2, 5);
    }
    else
    {
      type = this.Offerings[UnityEngine.Random.Range(0, this.Offerings.Count)];
      quantity = UnityEngine.Random.Range(3, 5);
      if (type == InventoryItem.ITEM_TYPE.BLACK_GOLD)
        quantity = UnityEngine.Random.Range(5, 9);
      if (type == InventoryItem.ITEM_TYPE.GOLD_NUGGET)
        quantity = UnityEngine.Random.Range(7, 14);
    }
    this.DepositItem(type, quantity);
    Action<Vector3> completeOfferingShrine = this.OnCompleteOfferingShrine;
    if (completeOfferingShrine == null)
      return;
    completeOfferingShrine(FollowerPosition);
  }
}
