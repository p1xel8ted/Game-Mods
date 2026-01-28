// Decompiled with JetBrains decompiler
// Type: Structures_Bed
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Structures_Bed : StructureBrain
{
  public static Structures_Bed.BedEvent OnBedCollapsedStatic;
  public static Structures_Bed.BedEvent OnBedRebuiltStatic;

  public virtual int SlotCount => 1;

  public bool IsCollapsed
  {
    get => this.Data.IsCollapsed || this.Data.IsAflame || this.Data.IsSnowedUnder;
    set => this.Data.IsCollapsed = value;
  }

  public event Structures_Bed.BedEvent OnBedCollapsed;

  public virtual int Level => 1;

  public override void Init(StructuresData data)
  {
    base.Init(data);
    for (int index = this.Data.MultipleFollowerIDs.Count - 1; index >= 0; --index)
    {
      if (FollowerInfo.GetInfoByID(this.Data.MultipleFollowerIDs[index]) == null)
        this.Data.MultipleFollowerIDs.RemoveAt(index);
    }
    FollowerInfo infoById = FollowerInfo.GetInfoByID(this.Data.FollowerID);
    if (infoById != null)
      infoById.DwellingSlot = Mathf.Clamp(infoById.DwellingSlot, 0, this.SlotCount - 1);
    else
      this.Data.FollowerID = -1;
  }

  public virtual float ChanceToCollapse
  {
    get => WeatherSystemController.Instance.IsRaining ? 0.2f : 0.15f;
  }

  public virtual StructureBrain.TYPES CollapsedType => StructureBrain.TYPES.BED_1_COLLAPSED;

  public override void OnAdded()
  {
    base.OnAdded();
    TimeManager.OnNewDayStarted += new System.Action(this.OnNewDayStarted);
    for (int index = 0; index < this.Data.MultipleFollowerIDs.Count; ++index)
    {
      FollowerInfo infoById = FollowerInfo.GetInfoByID(this.Data.MultipleFollowerIDs[index]);
      if (infoById != null && infoById.DwellingID != this.Data.ID)
        infoById.DwellingID = this.Data.ID;
    }
    for (int index = this.Data.FollowersClaimedSlots.Count - 1; index >= 0; --index)
    {
      if (FollowerInfo.GetInfoByID(this.Data.FollowersClaimedSlots[index]) == null)
      {
        if (this.Data.FollowerID == this.Data.FollowersClaimedSlots[index])
          this.Data.FollowerID = -1;
        this.Data.MultipleFollowerIDs.Remove(this.Data.FollowersClaimedSlots[index]);
        this.Data.FollowersClaimedSlots.Remove(this.Data.FollowersClaimedSlots[index]);
      }
    }
  }

  public override void OnRemoved()
  {
    base.OnRemoved();
    TimeManager.OnNewDayStarted -= new System.Action(this.OnNewDayStarted);
  }

  public void OnNewDayStarted()
  {
    ++this.Data.Age;
    if (this.Data.Age <= 2 || (double) UnityEngine.Random.value >= (double) this.ChanceToCollapse || this.Data.FollowerID == -1 || FollowerManager.FollowerLocked(in this.Data.FollowerID))
      return;
    this.Collapse(true, true, false);
  }

  public override bool Collapse(
    bool showNotifications = true,
    bool refreshFollowerTasks = true,
    bool struckByLightning = false)
  {
    if (this.Data.ClaimedByPlayer)
      return false;
    base.Collapse(showNotifications);
    if (this.Data.FollowerID != -1)
    {
      FollowerInfo infoById = FollowerInfo.GetInfoByID(this.Data.FollowerID);
      if (infoById != null && FollowerBrain.GetOrCreateBrain(infoById).CurrentTaskType == FollowerTaskType.SleepBedRest)
        return false;
    }
    this.IsCollapsed = true;
    Structures_Bed.BedEvent onBedCollapsed = this.OnBedCollapsed;
    if (onBedCollapsed != null)
      onBedCollapsed();
    Structures_Bed.BedEvent bedCollapsedStatic = Structures_Bed.OnBedCollapsedStatic;
    if (bedCollapsedStatic != null)
      bedCollapsedStatic();
    return true;
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

  public static bool WithinFreezingRadius(Vector3 pos)
  {
    bool flag = false;
    List<StructureBrain> structureBrainList = new List<StructureBrain>();
    structureBrainList.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType<Structures_Snow_Pile>());
    structureBrainList.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType<Structures_DeadWorshipper>());
    float num = 3.5f;
    foreach (StructureBrain structureBrain1 in structureBrainList)
    {
      StructureBrain structureBrain2 = (StructureBrain) null;
      if (structureBrain1 is Structures_DeadWorshipper)
      {
        FollowerInfo infoById = FollowerInfo.GetInfoByID(structureBrain1.Data.FollowerID);
        if (infoById != null && infoById.FrozeToDeath)
          structureBrain2 = structureBrain1;
      }
      else
        structureBrain2 = structureBrain1;
      if (structureBrain2 != null && (double) Vector3.Distance(structureBrain2.Data.Position, pos) <= (double) num)
      {
        flag = true;
        break;
      }
    }
    return flag;
  }

  public delegate void BedEvent();
}
