// Decompiled with JetBrains decompiler
// Type: Structures_DancingFirePit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Structures_DancingFirePit : StructureBrain, ITaskProvider
{
  private const int MAX_SLOT_COUNT = 40;
  public bool[] SlotReserved = new bool[40];

  public override void OnAdded() => TimeManager.OnNewPhaseStarted += new System.Action(this.OnNewPhase);

  public override void OnRemoved() => TimeManager.OnNewPhaseStarted -= new System.Action(this.OnNewPhase);

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

  public void ReleaseSlot(int slotIndex) => this.SlotReserved[slotIndex] = false;

  public Vector3 GetDancePosition(int followerId)
  {
    int index1 = 0;
    for (int index2 = 0; index2 < FollowerBrain.AllBrains.Count; ++index2)
    {
      if (FollowerBrain.AllBrains[index2].Info.ID == followerId)
      {
        index1 = index2;
        break;
      }
    }
    return index1 < Interaction_FireDancePit.Instance.Positions.Length ? Interaction_FireDancePit.Instance.Positions[index1].position : Interaction.interactions[0].transform.position + Vector3.down * 2f + (Vector3) UnityEngine.Random.insideUnitCircle * UnityEngine.Random.Range(2f, 4f);
  }

  public FollowerTask GetOverrideTask(FollowerBrain brain) => (FollowerTask) null;

  public bool CheckOverrideComplete() => true;

  public void GetAvailableTasks(ScheduledActivity activity, SortedList<float, FollowerTask> tasks)
  {
    if (!this.Data.IsGatheringActive)
      return;
    for (int index = 0; index < 40; ++index)
    {
      FollowerTask_DanceFirePit taskDanceFirePit = new FollowerTask_DanceFirePit(this.Data.ID);
      tasks.Add(taskDanceFirePit.Priorty, (FollowerTask) taskDanceFirePit);
    }
  }

  private void OnNewPhase()
  {
    if (this.Data.IsGatheringActive || this.Data.GatheringEndPhase == -1)
      return;
    this.Data.GatheringEndPhase = -1;
    this.Data.Fuel = 0;
  }
}
