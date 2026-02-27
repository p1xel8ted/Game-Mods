// Decompiled with JetBrains decompiler
// Type: Structures_WoolyShack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class Structures_WoolyShack : StructureBrain, ITaskProvider
{
  public bool ScarfAvailable()
  {
    for (int index = 0; index < this.Data.Inventory.Count; ++index)
    {
      if (this.Data.Inventory[index].UnreservedQuantity > 0)
        return true;
    }
    return false;
  }

  public void ReserveScarf()
  {
    for (int index = 0; index < this.Data.Inventory.Count; ++index)
    {
      if (this.Data.Inventory[index].UnreservedQuantity > 0)
      {
        ++this.Data.Inventory[index].QuantityReserved;
        break;
      }
    }
  }

  public void UnreserveScarf()
  {
    for (int index = 0; index < this.Data.Inventory.Count; ++index)
    {
      if (this.Data.Inventory[index].QuantityReserved > 0)
      {
        --this.Data.Inventory[index].QuantityReserved;
        break;
      }
    }
  }

  public void RemoveScarf()
  {
    if (this.Data.Inventory.Count <= 0)
      return;
    this.Data.Inventory.RemoveAt(0);
  }

  public FollowerTask GetOverrideTask(FollowerBrain brain) => (FollowerTask) null;

  public bool CheckOverrideComplete() => true;

  public void GetAvailableTasks(
    ScheduledActivity activity,
    SortedList<float, FollowerTask> sortedTasks)
  {
    for (int index = 0; index < this.Data.Inventory.Count; ++index)
    {
      if (this.Data.Inventory[index].UnreservedQuantity > 0 && SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter)
      {
        FollowerTask_CollectScarf taskCollectScarf = new FollowerTask_CollectScarf();
        sortedTasks.Add(taskCollectScarf.Priorty, (FollowerTask) taskCollectScarf);
      }
    }
  }
}
