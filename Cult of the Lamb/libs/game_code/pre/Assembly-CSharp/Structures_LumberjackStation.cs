// Decompiled with JetBrains decompiler
// Type: Structures_LumberjackStation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Structures_LumberjackStation : StructureBrain, ITaskProvider
{
  public System.Action OnExhauted;

  public int DURATION_GAME_MINUTES
  {
    get
    {
      switch (this.Data.Type)
      {
        case StructureBrain.TYPES.LUMBERJACK_STATION:
          return 120;
        case StructureBrain.TYPES.LUMBERJACK_STATION_2:
          return 75;
        case StructureBrain.TYPES.BLOODSTONE_MINE:
          return 120;
        case StructureBrain.TYPES.BLOODSTONE_MINE_2:
          return 75;
        default:
          return 0;
      }
    }
  }

  public int ResourceMax => 9999;

  public int LifeSpawn
  {
    get
    {
      switch (this.Data.Type)
      {
        case StructureBrain.TYPES.LUMBERJACK_STATION:
          return 50;
        case StructureBrain.TYPES.LUMBERJACK_STATION_2:
          return 100;
        case StructureBrain.TYPES.BLOODSTONE_MINE:
          return 50;
        case StructureBrain.TYPES.BLOODSTONE_MINE_2:
          return 100;
        default:
          return 0;
      }
    }
  }

  public FollowerTask GetOverrideTask(FollowerBrain brain) => (FollowerTask) null;

  public bool CheckOverrideComplete() => true;

  public void IncreaseAge()
  {
    ++this.Data.Age;
    if (this.Data.Age < this.LifeSpawn)
      return;
    this.Data.Exhausted = true;
    this.Data.Progress = 0.0f;
    System.Action onExhauted = this.OnExhauted;
    if (onExhauted == null)
      return;
    onExhauted();
  }

  public void GetAvailableTasks(ScheduledActivity activity, SortedList<float, FollowerTask> tasks)
  {
    if (activity != ScheduledActivity.Work || this.ReservedForTask || this.Data.Exhausted || this.Data.Inventory.Count >= this.ResourceMax)
      return;
    FollowerTask_ResourceStation taskResourceStation = new FollowerTask_ResourceStation(this.Data.ID);
    tasks.Add(taskResourceStation.Priorty, (FollowerTask) taskResourceStation);
  }

  public Structures_Tree GetNextTree()
  {
    Structures_Tree nextTree = (Structures_Tree) null;
    float num1 = 30f;
    foreach (Structures_Tree structuresTree in StructureManager.GetAllStructuresOfType<Structures_Tree>(this.Data.Location))
    {
      if (!structuresTree.ReservedForTask && !structuresTree.TreeChopped)
      {
        float num2 = Vector3.Distance(this.Data.Position, structuresTree.Data.Position);
        if ((double) num2 < (double) num1)
        {
          nextTree = structuresTree;
          num1 = num2;
        }
      }
    }
    return nextTree;
  }

  public Structures_LumberMine GetNextMine()
  {
    Structures_LumberMine nextMine = (Structures_LumberMine) null;
    float num1 = 30f;
    foreach (Structures_LumberMine structuresLumberMine in StructureManager.GetAllStructuresOfType<Structures_LumberMine>(this.Data.Location))
    {
      if (structuresLumberMine.RemainingLumber > 0)
      {
        float num2 = Vector3.Distance(this.Data.Position, structuresLumberMine.Data.Position);
        if ((double) num2 < (double) num1)
        {
          nextMine = structuresLumberMine;
          num1 = num2;
        }
      }
    }
    return nextMine;
  }
}
