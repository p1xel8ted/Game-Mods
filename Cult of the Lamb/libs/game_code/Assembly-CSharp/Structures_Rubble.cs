// Decompiled with JetBrains decompiler
// Type: Structures_Rubble
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class Structures_Rubble : StructureBrain, ITaskProvider
{
  public int availableSlotCount;
  public int RockSize;
  public Action<int> OnRemovalProgressChanged;
  public System.Action OnRemovalComplete;

  public float RemovalDurationInGameMinutes => this.RockSize == 0 ? 30f : 1000f;

  public int RubbleDropAmount
  {
    get
    {
      switch (this.Data.LootToDrop)
      {
        case InventoryItem.ITEM_TYPE.STONE:
          return this.RockSize != 0 ? 10 : 1;
        case InventoryItem.ITEM_TYPE.GOLD_NUGGET:
          return UnityEngine.Random.Range(2, 4);
        case InventoryItem.ITEM_TYPE.BLOOD_STONE:
          return UnityEngine.Random.Range(3, 5);
        case InventoryItem.ITEM_TYPE.MAGMA_STONE:
          return !ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.LightFurnace) || Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.MAGMA_STONE) > 0 ? UnityEngine.Random.Range(1, 2) : 3;
        default:
          return 0;
      }
    }
  }

  public int AvailableSlotCount
  {
    get => this.availableSlotCount;
    set => this.availableSlotCount = value;
  }

  public float RemovalProgress
  {
    get => this.Data.Progress;
    set
    {
      if (this.ProgressFinished)
        return;
      this.Data.Progress = value;
      if (!this.ProgressFinished)
        return;
      this.Remove();
      System.Action onRemovalComplete = this.OnRemovalComplete;
      if (onRemovalComplete == null)
        return;
      onRemovalComplete();
    }
  }

  public void UpdateProgress(int followerID = -1)
  {
    Action<int> removalProgressChanged = this.OnRemovalProgressChanged;
    if (removalProgressChanged == null)
      return;
    removalProgressChanged(followerID);
  }

  public Structures_Rubble(int rockSize)
  {
    this.RockSize = rockSize;
    this.AvailableSlotCount = this.RockSize == 0 ? 1 : 5;
  }

  public bool ProgressFinished
  {
    get => (double) this.RemovalProgress >= (double) this.RemovalDurationInGameMinutes;
  }

  public FollowerTask GetOverrideTask(FollowerBrain brain) => (FollowerTask) null;

  public bool CheckOverrideComplete() => true;

  public void GetAvailableTasks(ScheduledActivity activity, SortedList<float, FollowerTask> tasks)
  {
    if (activity != ScheduledActivity.Work || this.ProgressFinished)
      return;
    for (int index = 0; index < this.AvailableSlotCount; ++index)
    {
      FollowerTask_ClearRubble followerTaskClearRubble = new FollowerTask_ClearRubble(this.Data.ID);
      tasks.Add(followerTaskClearRubble.Priorty, (FollowerTask) followerTaskClearRubble);
    }
  }
}
