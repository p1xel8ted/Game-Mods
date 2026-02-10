// Decompiled with JetBrains decompiler
// Type: Structures_Waste
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class Structures_Waste : StructureBrain, ITaskProvider
{
  public Action<int> OnRemovalProgressChanged;
  public System.Action OnRemovalBegin;
  public System.Action OnRemovalComplete;

  public float RemovalDurationInGameMinutes => 15f;

  public int DropAmount
  {
    get
    {
      if (this.Data.LootToDrop != InventoryItem.ITEM_TYPE.SOOT)
        return 0;
      return !ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.LightFurnace) || Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.SOOT) > 0 ? UnityEngine.Random.Range(1, 3) : 5;
    }
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
      System.Action onRemovalBegin = this.OnRemovalBegin;
      if (onRemovalBegin != null)
        onRemovalBegin();
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

  public bool ProgressFinished
  {
    get => (double) this.RemovalProgress >= (double) this.RemovalDurationInGameMinutes;
  }

  public FollowerTask GetOverrideTask(FollowerBrain brain) => (FollowerTask) null;

  public bool CheckOverrideComplete() => true;

  public void GetAvailableTasks(ScheduledActivity activity, SortedList<float, FollowerTask> tasks)
  {
    if (activity != ScheduledActivity.Work || this.ReservedForTask || this.Data.Type == StructureBrain.TYPES.TOXIC_WASTE)
      return;
    FollowerTask_CleanWaste followerTaskCleanWaste = new FollowerTask_CleanWaste(this.Data.ID);
    tasks.Add(followerTaskCleanWaste.Priorty, (FollowerTask) followerTaskCleanWaste);
  }
}
