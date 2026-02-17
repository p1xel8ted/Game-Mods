// Decompiled with JetBrains decompiler
// Type: Dwelling
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Dwelling : BaseMonoBehaviour
{
  public static int NO_HOME = 0;
  public List<DwellingSlot> Positions = new List<DwellingSlot>();
  public global::StructureBrain.TYPES Type;
  public bool init;
  public static List<Dwelling> dwellings = new List<Dwelling>();
  public Structure Structure;
  public Structures_Bed _StructureInfo;

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public Structures_Bed StructureBrain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_Bed;
      return this._StructureInfo;
    }
  }

  public void Start()
  {
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    if (this.Structure.Brain == null)
      return;
    this.OnBrainAssigned();
  }

  public void OnEnable()
  {
    Dwelling.dwellings.Add(this);
    this.InitImages();
    FollowerBrain.OnDwellingAssigned += new FollowerBrain.DwellingAssignmentChanged(this.OnDwellingAssignmentChanged);
    FollowerBrain.OnDwellingCleared += new FollowerBrain.DwellingAssignmentChanged(this.OnDwellingAssignmentChanged);
  }

  public void OnDisable()
  {
    Dwelling.dwellings.Remove(this);
    FollowerBrain.OnDwellingAssigned -= new FollowerBrain.DwellingAssignmentChanged(this.OnDwellingAssignmentChanged);
    FollowerBrain.OnDwellingCleared -= new FollowerBrain.DwellingAssignmentChanged(this.OnDwellingAssignmentChanged);
  }

  public void OnDestroy() => this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);

  public void OnBrainAssigned()
  {
    if (this.Structure.Brain.Data.FollowersClaimedSlots.Count > 0)
    {
      bool flag = false;
      foreach (FollowerInfo follower in DataManager.Instance.Followers)
      {
        if (follower.DwellingID == this.Structure.Structure_Info.ID || this.Structure.Structure_Info.MultipleFollowerIDs.Contains(follower.DwellingID))
        {
          flag = true;
          break;
        }
      }
      if (!flag)
      {
        this.Structure.Brain.Data.MultipleFollowerIDs.Remove(this.Structure.Brain.Data.FollowerID);
        this.Structure.Brain.Data.FollowerID = -1;
        this.Structure.Brain.Data.FollowersClaimedSlots.Clear();
      }
    }
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this.StructureBrain.CheckForAndClearDuplicateBeds();
    this.InitImages();
  }

  public Vector3 GetDwellingSlotPosition(int slotID)
  {
    return slotID < this.Positions.Count ? this.Positions[slotID].BedOccupied.transform.position : this.Positions[0].BedOccupied.transform.position;
  }

  public void OnDwellingAssignmentChanged(int followerID, Dwelling.DwellingAndSlot d)
  {
    if (!((UnityEngine.Object) this.Structure != (UnityEngine.Object) null) || this.Structure.Structure_Info == null || d.ID != this.Structure.Structure_Info.ID)
      return;
    this.InitImages();
  }

  public void InitImages()
  {
    for (int slot = 0; slot < this.Positions.Count; ++slot)
      this.SetBedImage(slot, Dwelling.SlotState.UNCLAIMED);
    if ((UnityEngine.Object) this.Structure == (UnityEngine.Object) null || this.Structure.Structure_Info == null)
      return;
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
    {
      if (follower.DwellingID == this.Structure.Structure_Info.ID && this.StructureBrain.Data.FollowersClaimedSlots.Contains(follower.ID))
        this.SetBedImage(follower.DwellingSlot, Dwelling.SlotState.CLAIMED);
    }
    if (!this.StructureBrain.Data.ClaimedByPlayer)
      return;
    this.SetBedImage(0, Dwelling.SlotState.CLAIMED);
  }

  public void SetBedImage(int slot, Dwelling.SlotState slotState)
  {
    switch (slotState)
    {
      case Dwelling.SlotState.UNCLAIMED:
        this.Positions[slot].BedOccupied.SetActive(false);
        this.Positions[slot].BedUnoccupied.SetActive(false);
        this.Positions[slot].BedUnclaimed.SetActive(true);
        break;
      case Dwelling.SlotState.CLAIMED:
        this.Positions[slot].BedOccupied.SetActive(false);
        this.Positions[slot].BedUnoccupied.SetActive(true);
        this.Positions[slot].BedUnclaimed.SetActive(false);
        break;
      case Dwelling.SlotState.IN_USE:
        this.Positions[slot].BedOccupied.SetActive(true);
        this.Positions[slot].BedUnoccupied.SetActive(false);
        this.Positions[slot].BedUnclaimed.SetActive(false);
        break;
    }
  }

  public static Dwelling GetDwellingByID(int ID)
  {
    foreach (Dwelling dwelling in Dwelling.dwellings)
    {
      if (dwelling.Structure.Structure_Info.ID == ID)
        return dwelling;
    }
    return (Dwelling) null;
  }

  public class DwellingAndSlot
  {
    public int ID;
    public int dwellingslot;
    public int dwellingLevel;

    public DwellingAndSlot(int ID, int dwellingslot, int dwellingLevel)
    {
      this.ID = ID;
      this.dwellingslot = dwellingslot;
      this.dwellingLevel = dwellingLevel;
    }
  }

  public enum SlotState
  {
    UNCLAIMED,
    CLAIMED,
    IN_USE,
  }
}
