// Decompiled with JetBrains decompiler
// Type: Dwelling
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class Dwelling : BaseMonoBehaviour
{
  public static int NO_HOME = 0;
  public List<DwellingSlot> Positions = new List<DwellingSlot>();
  public global::StructureBrain.TYPES Type;
  private bool init;
  public static List<Dwelling> dwellings = new List<Dwelling>();
  public Structure Structure;
  private Structures_Bed _StructureInfo;

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

  private void Start()
  {
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    if (this.Structure.Brain == null)
      return;
    this.OnBrainAssigned();
  }

  private void OnEnable()
  {
    Dwelling.dwellings.Add(this);
    this.InitImages();
    FollowerBrain.OnDwellingAssigned += new FollowerBrain.DwellingAssignmentChanged(this.OnDwellingAssignmentChanged);
    FollowerBrain.OnDwellingCleared += new FollowerBrain.DwellingAssignmentChanged(this.OnDwellingAssignmentChanged);
  }

  private void OnDisable()
  {
    Dwelling.dwellings.Remove(this);
    FollowerBrain.OnDwellingAssigned -= new FollowerBrain.DwellingAssignmentChanged(this.OnDwellingAssignmentChanged);
    FollowerBrain.OnDwellingCleared -= new FollowerBrain.DwellingAssignmentChanged(this.OnDwellingAssignmentChanged);
  }

  private void OnDestroy() => this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);

  private void OnBrainAssigned()
  {
    if (this.Structure.Brain.Data.Claimed)
    {
      bool flag = false;
      foreach (FollowerInfo follower in DataManager.Instance.Followers)
      {
        if (follower.DwellingID == this.Structure.Structure_Info.ID)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
      {
        this.Structure.Brain.Data.FollowerID = -1;
        this.Structure.Brain.Data.Claimed = false;
      }
    }
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this.StructureBrain.CheckForAndClearDuplicateBeds();
    this.InitImages();
  }

  private void OnDwellingAssignmentChanged(int followerID, Dwelling.DwellingAndSlot d)
  {
    if (!((UnityEngine.Object) this.Structure != (UnityEngine.Object) null) || this.Structure.Structure_Info == null || d.ID != this.Structure.Structure_Info.ID)
      return;
    this.InitImages();
  }

  private void InitImages()
  {
    for (int slot = 0; slot < this.Positions.Count; ++slot)
      this.SetBedImage(slot, Dwelling.SlotState.UNCLAIMED);
    if ((UnityEngine.Object) this.Structure == (UnityEngine.Object) null || this.Structure.Structure_Info == null)
      return;
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
    {
      if (follower.DwellingID == this.Structure.Structure_Info.ID)
        this.SetBedImage(follower.DwellingSlot, Dwelling.SlotState.CLAIMED);
    }
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
