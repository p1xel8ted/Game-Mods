// Decompiled with JetBrains decompiler
// Type: Structures_Bed
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Structures_Bed : StructureBrain
{
  public static Structures_Bed.BedEvent OnBedCollapsedStatic;
  public static Structures_Bed.BedEvent OnBedRebuiltStatic;
  public int SoulMax = 10;
  public Action<int> OnSoulsGained;

  public virtual int SlotCount => 1;

  public bool IsCollapsed
  {
    get => this.Data.IsCollapsed;
    set => this.Data.IsCollapsed = value;
  }

  public event Structures_Bed.BedEvent OnBedCollapsed;

  public virtual int Level => 1;

  public virtual float ChanceToCollapse => WeatherController.isRaining ? 0.2f : 0.15f;

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

  public virtual StructureBrain.TYPES CollapsedType => StructureBrain.TYPES.BED_1_COLLAPSED;

  public override void OnAdded()
  {
    TimeManager.OnNewDayStarted += new System.Action(this.OnNewDayStarted);
    if (FollowerInfo.GetInfoByID(this.Data.FollowerID) != null)
      return;
    this.Data.Claimed = false;
    this.Data.FollowerID = -1;
  }

  public override void OnRemoved()
  {
    TimeManager.OnNewDayStarted -= new System.Action(this.OnNewDayStarted);
  }

  private void OnNewDayStarted()
  {
    ++this.Data.Age;
    if (this.Data.Age <= 2 || (double) UnityEngine.Random.value >= (double) this.ChanceToCollapse)
      return;
    this.Collapse();
  }

  public void Collapse()
  {
    if (this.Data.FollowerID != -1)
    {
      FollowerInfo infoById = FollowerInfo.GetInfoByID(this.Data.FollowerID);
      if (infoById != null && FollowerBrain.GetOrCreateBrain(infoById).CurrentTaskType == FollowerTaskType.SleepBedRest)
        return;
    }
    CultFaithManager.AddThought(Thought.BedCollapsed);
    this.IsCollapsed = true;
    Structures_Bed.BedEvent onBedCollapsed = this.OnBedCollapsed;
    if (onBedCollapsed != null)
      onBedCollapsed();
    Structures_Bed.BedEvent bedCollapsedStatic = Structures_Bed.OnBedCollapsedStatic;
    if (bedCollapsedStatic == null)
      return;
    bedCollapsedStatic();
  }

  public void Rebuild()
  {
    this.IsCollapsed = false;
    Structures_Bed.BedEvent bedRebuiltStatic = Structures_Bed.OnBedRebuiltStatic;
    if (bedRebuiltStatic == null)
      return;
    bedRebuiltStatic();
  }

  public void CheckForAndClearDuplicateBeds()
  {
    List<int> intList = new List<int>();
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      Dwelling.DwellingAndSlot dwellingAndSlot = allBrain.GetDwellingAndSlot();
      if (dwellingAndSlot != null && dwellingAndSlot.ID == this.Data.ID)
      {
        if (intList.Contains(dwellingAndSlot.dwellingslot))
          allBrain.ClearDwelling();
        else
          intList.Add(dwellingAndSlot.dwellingslot);
      }
    }
  }

  public bool CheckIfSlotIsOccupied(int slot)
  {
    bool flag = false;
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      Dwelling.DwellingAndSlot dwellingAndSlot = allBrain.GetDwellingAndSlot();
      if (dwellingAndSlot != null && dwellingAndSlot.ID == this.Data.ID && dwellingAndSlot.dwellingslot == slot)
      {
        flag = true;
        break;
      }
    }
    return flag;
  }

  public delegate void BedEvent();
}
