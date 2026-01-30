// Decompiled with JetBrains decompiler
// Type: Structures_OfferingShrine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public List<InventoryItem.ITEM_TYPE> SinOfferings = new List<InventoryItem.ITEM_TYPE>()
  {
    InventoryItem.ITEM_TYPE.COTTON,
    InventoryItem.ITEM_TYPE.HOPS,
    InventoryItem.ITEM_TYPE.GRAPES,
    InventoryItem.ITEM_TYPE.SILK_THREAD
  };
  public List<InventoryItem.ITEM_TYPE> DLCOfferings = new List<InventoryItem.ITEM_TYPE>()
  {
    InventoryItem.ITEM_TYPE.MAGMA_STONE,
    InventoryItem.ITEM_TYPE.LIGHTNING_SHARD,
    InventoryItem.ITEM_TYPE.WOOL,
    InventoryItem.ITEM_TYPE.YEW_CURSED,
    InventoryItem.ITEM_TYPE.FLOWER_WHITE,
    InventoryItem.ITEM_TYPE.FLOWER_PURPLE
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
    if ((double) UnityEngine.Random.value <= 0.20000000298023224 && UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Economy_Refinery))
    {
      type = this.RareOfferings[UnityEngine.Random.Range(0, this.RareOfferings.Count)];
      quantity = UnityEngine.Random.Range(1, 3);
    }
    else
    {
      type = this.Offerings[UnityEngine.Random.Range(0, this.Offerings.Count)];
      quantity = UnityEngine.Random.Range(3, 5);
      if (type == InventoryItem.ITEM_TYPE.BLACK_GOLD)
        quantity = UnityEngine.Random.Range(5, 9);
      if (type == InventoryItem.ITEM_TYPE.GOLD_NUGGET)
        quantity = UnityEngine.Random.Range(7, 14);
      if (DataManager.Instance.PleasureEnabled && (double) UnityEngine.Random.value <= 0.33000001311302185)
      {
        type = this.SinOfferings[UnityEngine.Random.Range(0, this.SinOfferings.Count)];
        quantity = type != InventoryItem.ITEM_TYPE.SILK_THREAD ? UnityEngine.Random.Range(5, 9) : UnityEngine.Random.Range(1, 3);
      }
      if (DataManager.Instance.MAJOR_DLC && DataManager.Instance.OnboardedRotstone && (double) UnityEngine.Random.value <= 0.33000001311302185 && SeasonsManager.Active)
      {
        List<InventoryItem.ITEM_TYPE> itemTypeList = new List<InventoryItem.ITEM_TYPE>((IEnumerable<InventoryItem.ITEM_TYPE>) this.DLCOfferings);
        if (!DataManager.Instance.OnboardedWool)
          itemTypeList.Remove(InventoryItem.ITEM_TYPE.WOOL);
        if (!DataManager.Instance.RancherOnboardedHolyYew)
          itemTypeList.Remove(InventoryItem.ITEM_TYPE.LIGHTNING_SHARD);
        if (!DataManager.Instance.RancherOnboardedHolyYew)
          itemTypeList.Remove(InventoryItem.ITEM_TYPE.YEW_CURSED);
        type = itemTypeList[UnityEngine.Random.Range(0, itemTypeList.Count)];
        switch (type)
        {
          case InventoryItem.ITEM_TYPE.FLOWER_WHITE:
          case InventoryItem.ITEM_TYPE.FLOWER_PURPLE:
            quantity = UnityEngine.Random.Range(3, 9);
            break;
          case InventoryItem.ITEM_TYPE.LIGHTNING_SHARD:
            quantity = UnityEngine.Random.Range(5, 9);
            break;
          case InventoryItem.ITEM_TYPE.WOOL:
            quantity = 1;
            break;
          case InventoryItem.ITEM_TYPE.MAGMA_STONE:
            quantity = UnityEngine.Random.Range(2, 5);
            break;
        }
      }
    }
    this.DepositItem(type, quantity);
    Action<Vector3> completeOfferingShrine = this.OnCompleteOfferingShrine;
    if (completeOfferingShrine == null)
      return;
    completeOfferingShrine(FollowerPosition);
  }
}
