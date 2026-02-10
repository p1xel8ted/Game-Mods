// Decompiled with JetBrains decompiler
// Type: Structures_Research
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public abstract class Structures_Research : StructureBrain, ITaskProvider
{
  public abstract bool[] SlotReserved { get; }

  public bool HasAvailableSlot()
  {
    bool flag = false;
    for (int index = 0; index < this.SlotReserved.Length; ++index)
    {
      if (!this.SlotReserved[index])
      {
        flag = true;
        break;
      }
    }
    return flag;
  }

  public int GetAvailableSlotCount()
  {
    int availableSlotCount = 0;
    for (int index = 0; index < this.SlotReserved.Length; ++index)
    {
      if (!this.SlotReserved[index])
        ++availableSlotCount;
    }
    return availableSlotCount;
  }

  public int GetReservedSlotCount()
  {
    int reservedSlotCount = 0;
    for (int index = 0; index < this.SlotReserved.Length; ++index)
    {
      if (this.SlotReserved[index])
        ++reservedSlotCount;
    }
    return reservedSlotCount;
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
      for (int index = 0; index < this.SlotReserved.Length; ++index)
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

  public void ReleaseSlot(int slotIndex) => this.SlotReserved[slotIndex] = false;

  public abstract Vector3 GetResearchPosition(int slotIndex);

  public FollowerTask GetOverrideTask(FollowerBrain brain) => (FollowerTask) null;

  public bool CheckOverrideComplete() => true;

  public void GetAvailableTasks(ScheduledActivity activity, SortedList<float, FollowerTask> tasks)
  {
    if (activity != ScheduledActivity.Work || !StructuresData.GetAnyResearchExists())
      return;
    int num = 0;
    for (int index = 0; index < this.SlotReserved.Length; ++index)
    {
      if (!this.SlotReserved[index])
      {
        ++num;
        FollowerTask_Research followerTaskResearch = new FollowerTask_Research(this.Data.ID, num == this.SlotReserved.Length ? 15f : 8f);
        tasks.Add(followerTaskResearch.Priorty, (FollowerTask) followerTaskResearch);
      }
    }
  }
}
