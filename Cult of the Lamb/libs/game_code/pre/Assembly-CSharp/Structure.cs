// Decompiled with JetBrains decompiler
// Type: Structure
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Structure : BaseMonoBehaviour
{
  public StructureBrain.TYPES Type;
  public int VariantIndex;
  private StructureBrain _brain;
  public System.Action OnBrainAssigned;
  public System.Action OnBrainRemoved;
  public static List<Structure> Structures = new List<Structure>();
  public static Structure.StructureInventoryChanged OnItemDeposited;
  public UnityEvent OnProgressCompleted;
  private Health health;

  public StructuresData Structure_Info => this.Brain?.Data;

  public StructureBrain Brain
  {
    get => this._brain;
    set
    {
      if (this._brain != null)
        this.transform.position -= this.Structure_Info.Offset;
      this._brain = value;
      System.Action onBrainAssigned = this.OnBrainAssigned;
      if (onBrainAssigned != null)
        onBrainAssigned();
      this.transform.position += this.Structure_Info.Offset;
    }
  }

  public List<InventoryItem> Inventory
  {
    get => this.Structure_Info.Inventory;
    set => this.Structure_Info.Inventory = value;
  }

  public static int CountStructuresOfType(StructureBrain.TYPES structureType)
  {
    int num = 0;
    foreach (Structure structure in Structure.Structures)
    {
      if (structure.Type == structureType)
        ++num;
    }
    return num;
  }

  private void OnEnable()
  {
    if (this.Structure_Info != null && this.Structure_Info.Destroyed)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }
    else
    {
      StructureManager.OnStructureRemoved += new StructureManager.StructureChanged(this.OnStructureRemoved);
      Structure.Structures.Add(this);
    }
  }

  private void OnDisable()
  {
    StructureManager.OnStructureRemoved -= new StructureManager.StructureChanged(this.OnStructureRemoved);
    Structure.Structures.Remove(this);
  }

  private void Start()
  {
    this.health = this.GetComponent<Health>();
    if (!((UnityEngine.Object) this.health != (UnityEngine.Object) null))
      return;
    this.health.OnHit += new Health.HitAction(this.OnHit);
    this.health.OnDie += new Health.DieAction(this.OnDie);
  }

  private void OnStructureRemoved(StructuresData structure)
  {
    if (structure != this.Structure_Info)
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public void CreateStructure(
    FollowerLocation location,
    Vector3 position,
    bool emitParticles = true,
    bool save = true)
  {
    this.CreateStructure(location, position, Vector2Int.one, emitParticles, save);
  }

  public void CreateStructure(
    FollowerLocation location,
    Vector3 position,
    Vector2Int bounds,
    bool emitParticles = true,
    bool save = true)
  {
    StructuresData infoByType = StructuresData.GetInfoByType(this.Type, this.VariantIndex);
    if (infoByType == null)
      return;
    infoByType.CreateStructure(location, position, bounds);
    this.Brain = StructureManager.AddStructure(location, infoByType, emitParticles, save);
  }

  public void CreateStructure(
    FollowerLocation location,
    Vector3 position,
    Vector2Int bounds,
    StructureBrain.TYPES ToBuildType)
  {
    StructuresData infoByType = StructuresData.GetInfoByType(this.Type, this.VariantIndex);
    infoByType.ToBuildType = ToBuildType;
    if (infoByType == null)
      return;
    infoByType.CreateStructure(location, position, bounds);
    this.Brain = StructureManager.AddStructure(location, infoByType);
  }

  public void RemoveStructure()
  {
    if (this.Structure_Info.ToBuildType == StructureBrain.TYPES.SURVEILLANCE)
      DataManager.Instance.HasBuiltSurveillance = false;
    if (this.Type == StructureBrain.TYPES.SURVEILLANCE)
      DataManager.Instance.HasBuiltSurveillance = false;
    this.Brain.Remove();
    System.Action onBrainRemoved = this.OnBrainRemoved;
    if (onBrainRemoved == null)
      return;
    onBrainRemoved();
  }

  public static bool TypeExists(StructureBrain.TYPES Type)
  {
    foreach (Structure structure in Structure.Structures)
    {
      if (structure.Type == Type)
        return true;
    }
    return false;
  }

  public bool IsType(StructureBrain.TYPES Type) => this.Type == Type;

  public static Structure GetOfType(StructureBrain.TYPES Type)
  {
    foreach (Structure structure in Structure.Structures)
    {
      if (structure.Type == Type)
        return structure;
    }
    return (Structure) null;
  }

  public static List<Structure> GetListOfType(StructureBrain.TYPES Type)
  {
    List<Structure> listOfType = new List<Structure>();
    foreach (Structure structure in Structure.Structures)
    {
      if (structure.Type == Type)
        listOfType.Add(structure);
    }
    return listOfType;
  }

  public static int GetTypeCount(StructureBrain.TYPES Type)
  {
    int typeCount = 0;
    foreach (Structure structure in Structure.Structures)
    {
      if (structure.Type == Type)
        ++typeCount;
    }
    return typeCount;
  }

  public void DepositInventory(InventoryItem item)
  {
    this.Inventory.Add(item);
    Structure.StructureInventoryChanged onItemDeposited = Structure.OnItemDeposited;
    if (onItemDeposited == null)
      return;
    onItemDeposited(this, item);
  }

  public bool HasInventoryType(InventoryItem.ITEM_TYPE Type)
  {
    foreach (InventoryItem inventoryItem in this.Inventory)
    {
      if ((InventoryItem.ITEM_TYPE) inventoryItem.type == Type)
        return true;
    }
    return false;
  }

  public int GetInventoryTypeCount(InventoryItem.ITEM_TYPE Type)
  {
    int inventoryTypeCount = 0;
    foreach (InventoryItem inventoryItem in this.Inventory)
    {
      if ((InventoryItem.ITEM_TYPE) inventoryItem.type == Type)
        ++inventoryTypeCount;
    }
    return inventoryTypeCount;
  }

  public void RemoveInventoryByType(InventoryItem.ITEM_TYPE Type)
  {
    foreach (InventoryItem inventoryItem in this.Inventory)
    {
      if ((InventoryItem.ITEM_TYPE) inventoryItem.type == Type)
      {
        this.Inventory.Remove(inventoryItem);
        break;
      }
    }
  }

  public void ProgressCompleted()
  {
    if (this.OnProgressCompleted == null)
      return;
    this.OnProgressCompleted.Invoke();
  }

  public virtual void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    CameraManager.shakeCamera(0.5f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
    if (!this.Structure_Info.RemoveOnDie)
      return;
    this.RemoveStructure();
  }

  public virtual void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    CameraManager.shakeCamera(0.1f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
  }

  public virtual void OnDestroy()
  {
    if (!((UnityEngine.Object) this.health != (UnityEngine.Object) null))
      return;
    this.health.OnDie -= new Health.DieAction(this.OnDie);
    this.health.OnHit -= new Health.HitAction(this.OnHit);
  }

  public delegate void StructureInventoryChanged(Structure structure, InventoryItem item);
}
