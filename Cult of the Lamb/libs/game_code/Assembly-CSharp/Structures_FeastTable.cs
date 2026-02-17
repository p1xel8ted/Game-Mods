// Decompiled with JetBrains decompiler
// Type: Structures_FeastTable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class Structures_FeastTable : StructureBrain, ITaskProvider
{
  public const int MAX_SLOT_COUNT = 40;
  public bool[] SlotReserved = new bool[40];

  public bool HasAvailableSlot()
  {
    bool flag = false;
    for (int index = 0; index < 40; ++index)
    {
      if (!this.SlotReserved[index])
      {
        flag = true;
        break;
      }
    }
    return flag;
  }

  public bool TryClaimSlot(ref int slotIndex)
  {
    bool flag = false;
    if (slotIndex >= 0 && !this.SlotReserved[slotIndex])
    {
      this.SlotReserved[slotIndex] = true;
      flag = true;
    }
    if (!flag)
    {
      for (int index = 0; index < 40; ++index)
      {
        if (!this.SlotReserved[index])
        {
          slotIndex = index;
          this.SlotReserved[index] = true;
          flag = true;
          break;
        }
      }
    }
    if (!flag)
      slotIndex = -1;
    return flag;
  }

  public int SlotsReserved()
  {
    int num = 0;
    foreach (bool flag in this.SlotReserved)
    {
      if (flag)
        ++num;
    }
    return num;
  }

  public void ReleaseSlot(int slotIndex) => this.SlotReserved[slotIndex] = false;

  public FollowerTask GetOverrideTask(FollowerBrain brain) => (FollowerTask) null;

  public bool CheckOverrideComplete() => true;

  public void GetAvailableTasks(ScheduledActivity activity, SortedList<float, FollowerTask> tasks)
  {
    if (!this.Data.IsGatheringActive)
      return;
    for (int index = 0; index < 40; ++index)
    {
      FollowerTask_EatFeastTable taskEatFeastTable = new FollowerTask_EatFeastTable(this.Data.ID);
      tasks.Add(taskEatFeastTable.Priorty, (FollowerTask) taskEatFeastTable);
    }
  }
}
