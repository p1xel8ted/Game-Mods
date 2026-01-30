// Decompiled with JetBrains decompiler
// Type: Structures_LumberjackStation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        case StructureBrain.TYPES.ROTSTONE_MINE:
          return 250;
        case StructureBrain.TYPES.ROTSTONE_MINE_2:
          return 150;
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
        case StructureBrain.TYPES.ROTSTONE_MINE:
          return 35;
        case StructureBrain.TYPES.ROTSTONE_MINE_2:
          return 70;
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
    Structures_Tree result;
    StructureManager.TryGetMinValueStructureOfType<Structures_Tree>(out result, in this.Data.Location, (Func<Structures_Tree, float>) (t => Vector3.Distance(this.Data.Position, t.Data.Position)), (Func<Structures_Tree, bool>) (t => !t.ReservedForTask && !t.TreeChopped), 30f);
    return result;
  }

  public Structures_LumberMine GetNextMine()
  {
    Structures_LumberMine result;
    StructureManager.TryGetMinValueStructureOfType<Structures_LumberMine>(out result, in this.Data.Location, (Func<Structures_LumberMine, float>) (m => Vector3.Distance(this.Data.Position, m.Data.Position)), (Func<Structures_LumberMine, bool>) (m => m.RemainingLumber > 0), 30f);
    return result;
  }

  [CompilerGenerated]
  public float \u003CGetNextTree\u003Eb__11_0(Structures_Tree t)
  {
    return Vector3.Distance(this.Data.Position, t.Data.Position);
  }

  [CompilerGenerated]
  public float \u003CGetNextMine\u003Eb__12_0(Structures_LumberMine m)
  {
    return Vector3.Distance(this.Data.Position, m.Data.Position);
  }
}
