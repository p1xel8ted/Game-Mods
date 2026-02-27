// Decompiled with JetBrains decompiler
// Type: Structures_Shrine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Structures_Shrine : StructureBrain, ITaskProvider
{
  public float DURATION_PER_DEVOTION;
  public List<FollowerTask_Pray> Prayers = new List<FollowerTask_Pray>();
  public Action<int> OnSoulsGained;
  public int[] assignedSeats = new int[10];
  private DayPhase OverridePhase = DayPhase.None;

  public virtual int SoulMax
  {
    get
    {
      switch (this.Data.Type)
      {
        case StructureBrain.TYPES.SHRINE:
          return 50;
        case StructureBrain.TYPES.SHRINE_II:
          return 70;
        case StructureBrain.TYPES.SHRINE_III:
          return 90;
        case StructureBrain.TYPES.SHRINE_IV:
          return 175;
        default:
          return 0;
      }
    }
  }

  public virtual int PrayersMax
  {
    get
    {
      switch (this.Data.Type)
      {
        case StructureBrain.TYPES.SHRINE:
          return 4;
        case StructureBrain.TYPES.SHRINE_II:
          return 6;
        case StructureBrain.TYPES.SHRINE_III:
          return 8;
        case StructureBrain.TYPES.SHRINE_IV:
          return 10;
        default:
          return 0;
      }
    }
  }

  public int SoulCount
  {
    get => this.Data.SoulCount;
    set
    {
      int soulCount = this.SoulCount;
      this.Data.SoulCount = Mathf.Clamp(value, 0, this.SoulMax);
      if (this.SoulCount <= soulCount)
        return;
      Action<int> onSoulsGained = this.OnSoulsGained;
      if (onSoulsGained == null)
        return;
      onSoulsGained(this.SoulCount - soulCount);
    }
  }

  public float DevotionSpeedMultiplier
  {
    get
    {
      if (this.Data.FullyFueled && UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Shrine_Flame))
        return 1.2f;
      return this.Data.FullyFueled && UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Shrine_FlameII) ? 1.4f : 1f;
    }
  }

  public void AddPrayer(FollowerTask_Pray prayer)
  {
    if (this.Prayers.Contains(prayer))
      return;
    this.Prayers.Add(prayer);
  }

  public void RemovePrayer(FollowerTask_Pray prayer)
  {
    if (!this.Prayers.Contains(prayer))
      return;
    this.Prayers.Remove(prayer);
  }

  public Vector3 GetPrayerPosition(FollowerBrain follower)
  {
    Vector3 position = this.Data.Position;
    BuildingShrine shrine = this.GetShrine();
    if (!(bool) (UnityEngine.Object) shrine)
      return position;
    for (int index = 0; index < this.assignedSeats.Length; ++index)
    {
      if (index < this.PrayersMax && this.assignedSeats[index] == follower.Info.ID)
        return shrine.SpawnPositions[index].transform.position;
    }
    for (int index = 0; index < this.assignedSeats.Length; ++index)
    {
      if (index < this.PrayersMax)
      {
        FollowerInfo infoById = FollowerInfo.GetInfoByID(this.assignedSeats[index]);
        if (infoById == null || this.assignedSeats[index] == 0 || !(FollowerBrain.GetOrCreateBrain(infoById).CurrentTask is FollowerTask_Pray))
        {
          this.assignedSeats[index] = follower.Info.ID;
          return shrine.SpawnPositions[index].transform.position;
        }
      }
    }
    follower.CurrentTask.Abort();
    return position;
  }

  private BuildingShrine GetShrine()
  {
    foreach (BuildingShrine shrine in BuildingShrine.Shrines)
    {
      if (shrine.StructureBrain == this)
        return shrine;
    }
    return (BuildingShrine) null;
  }

  public void GetAvailableTasks(ScheduledActivity activity, SortedList<float, FollowerTask> tasks)
  {
    if (activity != ScheduledActivity.Work || this.SoulCount >= this.SoulMax)
      return;
    for (int index = 0; index < this.PrayersMax - this.Prayers.Count; ++index)
    {
      FollowerTask_Pray followerTaskPray = new FollowerTask_Pray(this.Data.ID);
      tasks.Add(followerTaskPray.Priorty, (FollowerTask) followerTaskPray);
    }
  }

  public virtual FollowerTask GetOverrideTask(FollowerBrain brain)
  {
    Debug.Log((object) nameof (GetOverrideTask));
    return (FollowerTask) new FollowerTask_Pray(this.Data.ID);
  }

  public bool CheckOverrideComplete() => false;

  public override void OnAdded()
  {
    TimeManager.OnNewPhaseStarted += new System.Action(this.OnNewPhaseStarted);
  }

  public override void OnRemoved()
  {
    TimeManager.OnNewPhaseStarted -= new System.Action(this.OnNewPhaseStarted);
  }

  private void OnNewPhaseStarted() => this.UpdateFuel();
}
