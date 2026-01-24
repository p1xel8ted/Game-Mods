// Decompiled with JetBrains decompiler
// Type: Structures_BerryBush
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Structures_BerryBush : StructureBrain, ITaskProvider
{
  public System.Action OnRegrowTree;
  public System.Action OnTreeProgressChanged;
  public Action<bool> OnTreeComplete;
  public bool IsCrop = true;
  public int CropID = -1;

  public bool BerryPicked => (double) this.Data.Progress >= (double) this.Data.ProgressTarget;

  public void PickBerries(float berryDamage = 1f, bool dropLoot = true)
  {
    if (this.BerryPicked)
      return;
    this.Data.Progress += berryDamage;
    System.Action treeProgressChanged = this.OnTreeProgressChanged;
    if (treeProgressChanged != null)
      treeProgressChanged();
    if (!this.BerryPicked)
      return;
    Action<bool> onTreeComplete = this.OnTreeComplete;
    if (onTreeComplete != null)
      onTreeComplete(dropLoot);
    this.Data.Watered = WeatherSystemController.Instance.IsRaining;
    if (PlayerFarming.Location == this.Data.Location)
      return;
    StructureManager.GetStructureByID<Structures_FarmerPlot>(this.CropID)?.Harvest();
    this.Remove();
  }

  public FollowerTask GetOverrideTask(FollowerBrain brain) => (FollowerTask) null;

  public bool CheckOverrideComplete() => true;

  public void GetAvailableTasks(ScheduledActivity activity, SortedList<float, FollowerTask> tasks)
  {
    if (activity != ScheduledActivity.Work || this.ReservedForTask || this.BerryPicked || this.IsCrop)
      return;
    FollowerTask_Forage followerTaskForage = new FollowerTask_Forage(this.Data.ID);
    tasks.Add(followerTaskForage.Priorty, (FollowerTask) followerTaskForage);
  }

  public void DropBerries(Vector3 Position, bool fromCrop = false, InventoryItem.ITEM_TYPE seedType = InventoryItem.ITEM_TYPE.NONE)
  {
    UnityEngine.Random.Range(0, 100);
    foreach (InventoryItem.ITEM_TYPE berry in this.GetBerries())
      InventoryItem.Spawn(berry, 1, Position);
  }

  public void AddBerriesToChest(FollowerLocation Location)
  {
    Structures_CollectedResourceChest result;
    if (Location == PlayerFarming.Location || !StructureManager.TryGetFirstStructureOfType<Structures_CollectedResourceChest>(out result, in Location))
      return;
    foreach (InventoryItem.ITEM_TYPE berry in this.GetBerries())
      result.AddItem(berry, 1);
  }

  public List<InventoryItem.ITEM_TYPE> GetBerries()
  {
    List<InventoryItem.ITEM_TYPE> berries = new List<InventoryItem.ITEM_TYPE>();
    int num1 = UnityEngine.Random.Range(this.Data.LootCountToDropRange.x, this.Data.LootCountToDropRange.y);
    if (this.IsCrop)
    {
      num1 = UnityEngine.Random.Range(this.Data.CropLootCountToDropRange.x, this.Data.CropLootCountToDropRange.y);
      Structures_FarmerPlot structureById = StructureManager.GetStructureByID<Structures_FarmerPlot>(this.CropID);
      if (structureById != null && structureById.HasFertilized() && this.Data.LootToDrop != InventoryItem.ITEM_TYPE.SNOW_FRUIT)
        num1 += UnityEngine.Random.Range(1, 3);
      if (structureById != null && structureById.GetFertilizer() != null && structureById.GetFertilizer().type == 142)
      {
        InventoryItem.ITEM_TYPE itemType = StructureManager.GetAllStructuresOfType<Structures_Refinery>().Count > 0 ? InventoryItem.ITEM_TYPE.GOLD_REFINED : InventoryItem.ITEM_TYPE.BLACK_GOLD;
        int num2 = itemType == InventoryItem.ITEM_TYPE.BLACK_GOLD ? UnityEngine.Random.Range(5, 11) * (DataManager.Instance.BossesCompleted.Count + 1) : UnityEngine.Random.Range(1, 3);
        for (int index = 0; index < num2; ++index)
          berries.Add(itemType);
      }
    }
    for (int index = 0; index < num1; ++index)
      berries.Add(this.Data.MultipleLootToDrop[0]);
    if (this.Data.MultipleLootToDrop.Count > 1)
    {
      if (!this.IsCrop)
      {
        berries.Add(this.Data.MultipleLootToDrop[1]);
        if ((double) UnityEngine.Random.value < 0.75)
          berries.Add(this.Data.MultipleLootToDrop[1]);
      }
      else if (this.IsCrop && (double) UnityEngine.Random.value < 0.800000011920929)
        berries.Add(this.Data.MultipleLootToDrop[1]);
    }
    return berries;
  }
}
