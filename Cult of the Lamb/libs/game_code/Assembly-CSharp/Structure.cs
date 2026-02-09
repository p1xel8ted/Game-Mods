// Decompiled with JetBrains decompiler
// Type: Structure
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class Structure : BaseMonoBehaviour
{
  public static Material noShadowsMaterial;
  public StructureBrain.TYPES Type;
  public int VariantIndex;
  public StructureBrain _brain;
  public System.Action OnBrainAssigned;
  public System.Action OnBrainRemoved;
  public List<KeyValuePair<GameObject, bool>> children = new List<KeyValuePair<GameObject, bool>>();
  public static List<Structure> Structures = new List<Structure>();
  public static List<AsyncOperationHandle<GameObject>> loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();
  public static Structure.StructureInventoryChanged OnItemDeposited;
  public UnityEvent OnProgressCompleted;
  public Health health;

  public static Material NoShadowsMaterial
  {
    get
    {
      if ((UnityEngine.Object) Structure.noShadowsMaterial == (UnityEngine.Object) null)
      {
        AsyncOperationHandle<Material> asyncOperationHandle = Addressables.LoadAssetAsync<Material>((object) "Assets/Art/Shaders/AmplifyShaderEditor/Environment/Sprites-Default_NoShadow.mat");
        asyncOperationHandle.WaitForCompletion();
        Structure.noShadowsMaterial = asyncOperationHandle.Result;
      }
      return Structure.noShadowsMaterial;
    }
  }

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

  public int InventoryTotalCount
  {
    get
    {
      int inventoryTotalCount = 0;
      foreach (InventoryItem inventoryItem in this.Inventory)
        inventoryTotalCount += inventoryItem.quantity;
      return inventoryTotalCount;
    }
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

  public void OnEnable()
  {
    if (this.Structure_Info != null && this.Structure_Info.Destroyed)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }
    else
    {
      StructureManager.OnStructureRemoved += new StructureManager.StructureChanged(this.OnStructureRemoved);
      if (this.Type == StructureBrain.TYPES.WEEDS)
        return;
      Structure.Structures.Add(this);
    }
  }

  public void OptimizeShadows()
  {
    int num = 200;
    SpriteRenderer[] componentsInChildren = this.gameObject.GetComponentsInChildren<SpriteRenderer>(true);
    for (int index = 0; index < componentsInChildren.Length; ++index)
    {
      if (!((UnityEngine.Object) componentsInChildren[index].sprite == (UnityEngine.Object) null))
      {
        Rect rect1 = componentsInChildren[index].sprite.rect;
        if (componentsInChildren[index].shadowCastingMode == ShadowCastingMode.Off && (UnityEngine.Object) componentsInChildren[index].sharedMaterial != (UnityEngine.Object) null && componentsInChildren[index].sharedMaterial.name == "Sprites-Default")
          componentsInChildren[index].sharedMaterial = Structure.NoShadowsMaterial;
        Rect rect2 = componentsInChildren[index].sprite.rect;
        if ((double) rect2.size.x < (double) num)
        {
          rect2 = componentsInChildren[index].sprite.rect;
          if ((double) rect2.size.y < (double) num && (UnityEngine.Object) componentsInChildren[index].sharedMaterial != (UnityEngine.Object) null && componentsInChildren[index].sharedMaterial.name == "Sprites-Default")
            componentsInChildren[index].sharedMaterial = Structure.NoShadowsMaterial;
        }
      }
    }
  }

  public void OnDisable()
  {
    StructureManager.OnStructureRemoved -= new StructureManager.StructureChanged(this.OnStructureRemoved);
    Structure.Structures.Remove(this);
  }

  public void Start()
  {
    this.health = this.GetComponent<Health>();
    if ((UnityEngine.Object) this.health != (UnityEngine.Object) null)
    {
      this.health.OnHit += new Health.HitAction(this.OnHit);
      this.health.OnDie += new Health.DieAction(this.OnDie);
    }
    this.OnBrainAssigned += new System.Action(this.BrainAssigned);
    if (this.Brain == null)
      return;
    this.BrainAssigned();
  }

  public void BrainAssigned()
  {
    this.Brain.OnCollapse += new System.Action(this.OnCollapse);
    this.Brain.OnSnowedUnder += new System.Action(this.OnSnowedUnder);
    this.Brain.OnRepaired += new System.Action(this.OnRepaired);
    this.Brain.OnDefrosted += new System.Action(this.OnRepaired);
    this.Brain.OnAflamed += new System.Action(this.OnAflamed);
    this.Brain.OnExtinguished += new System.Action(this.OnExtinguished);
    if (this.Brain.Data.IsCollapsed && this.Brain.Data.Type != StructureBrain.TYPES.BED && this.Brain.Data.Type != StructureBrain.TYPES.BED_2)
      this.Collapse();
    else if (this.Brain.Data.IsAflame)
    {
      this.Aflame();
    }
    else
    {
      if (!this.Brain.Data.IsSnowedUnder)
        return;
      this.SnowedUnder();
    }
  }

  public void OnCollapse()
  {
    if (this.Brain.Data.Type == StructureBrain.TYPES.BED || this.Brain.Data.Type == StructureBrain.TYPES.BED_2)
      return;
    this.Collapse();
  }

  public void OnSnowedUnder() => this.SnowedUnder();

  public void OnRepaired() => this.Repair();

  public void OnAflamed() => this.Aflame();

  public void OnExtinguished() => this.Extinguished();

  public void OnStructureRemoved(StructuresData structure)
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
    if ((UnityEngine.Object) this.health != (UnityEngine.Object) null)
    {
      this.health.OnDie -= new Health.DieAction(this.OnDie);
      this.health.OnHit -= new Health.HitAction(this.OnHit);
    }
    this.OnBrainAssigned -= new System.Action(this.BrainAssigned);
    if (this.Brain == null)
      return;
    this.Brain.OnCollapse -= new System.Action(this.OnCollapse);
    this.Brain.OnSnowedUnder -= new System.Action(this.OnSnowedUnder);
    this.Brain.OnRepaired -= new System.Action(this.OnRepaired);
    this.Brain.OnDefrosted -= new System.Action(this.OnRepaired);
    this.Brain.OnAflamed -= new System.Action(this.OnAflamed);
    this.Brain.OnExtinguished -= new System.Action(this.OnExtinguished);
  }

  public void Collapse()
  {
    for (int index = 0; index < this.transform.childCount; ++index)
    {
      this.children.Add(new KeyValuePair<GameObject, bool>(this.transform.GetChild(index).gameObject, this.transform.GetChild(index).gameObject.activeSelf));
      this.transform.GetChild(index).gameObject.SetActive(false);
    }
    Addressables_wrapper.InstantiateAsync((object) $"Assets/Prefabs/Base/Destroyed_Structure_Rubble {this.Brain.Data.Bounds.x}.prefab", this.Brain.Data.Position, Quaternion.identity, this.transform, (Action<AsyncOperationHandle<GameObject>>) (obj => obj.Result.GetComponent<StructureRubble>().Configure(StructuresData.GetBuildRubbleType(this.Brain.Data.Type))));
    foreach (Behaviour componentsInChild in this.GetComponentsInChildren<Interaction>())
      componentsInChild.enabled = false;
    this.gameObject.AddComponent<Interaction_RepairStructure>().Configure(this, StructuresData.GetBuildRubbleType(this.Brain.Data.Type), this.Brain.Data.Bounds.x * 3);
  }

  public void SnowedUnder()
  {
    Addressables_wrapper.InstantiateAsync((object) "Assets/Prefabs/Base/Snowed_Under_Rubble.prefab", this.Brain.Data.Position, Quaternion.identity, this.transform, (Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      if (!this.Brain.Data.IsSnowedUnder)
      {
        UnityEngine.Object.Destroy((UnityEngine.Object) obj.Result);
      }
      else
      {
        obj.Result.transform.localScale = Vector3.zero;
        obj.Result.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
        if (this.Brain.Data.Bounds.x < 3)
          return;
        obj.Result.GetComponent<StructureRubble>().SetBig();
      }
    }));
    foreach (Behaviour componentsInChild in this.GetComponentsInChildren<Interaction>())
      componentsInChild.enabled = false;
    this.gameObject.AddComponent<Interaction_RepairStructure>().Configure(this, InventoryItem.ITEM_TYPE.MAGMA_STONE, this.Brain.Data.Bounds.x * 3);
  }

  public void Repair()
  {
    for (int index = this.transform.childCount - 1; index >= 0; --index)
    {
      if (this.transform.GetChild(index).CompareTag("Structure_Hindrance"))
        UnityEngine.Object.Destroy((UnityEngine.Object) this.transform.GetChild(index).gameObject);
      else if (this.children.Count > 0)
        this.transform.GetChild(index).gameObject.SetActive(this.children[index].Value);
    }
    foreach (Behaviour componentsInChild in this.GetComponentsInChildren<Interaction>())
      componentsInChild.enabled = true;
    Interaction_RepairStructure component = this.GetComponent<Interaction_RepairStructure>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) component);
    this.children.Clear();
  }

  public void Aflame()
  {
    int x = -1;
    while (++x < this.Brain.Data.Bounds.x)
    {
      int y = -1;
      while (++y < this.Brain.Data.Bounds.y)
        Addressables.InstantiateAsync((object) "Assets/Prefabs/Base/Structure_Fire.prefab", Utils.RotatePointAroundPivot(this.Brain.Data.Position + new Vector3((float) x, (float) y), this.Brain.Data.Position, new Vector3(0.0f, 0.0f, 45f)), Quaternion.Euler(-30f, 180f, 0.0f), this.transform).Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
        {
          obj.Result.transform.localScale = Vector3.zero;
          obj.Result.transform.DOScale(2f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBounce);
        });
    }
    foreach (Behaviour componentsInChild in this.GetComponentsInChildren<Interaction>())
      componentsInChild.enabled = false;
    this.gameObject.AddComponent<Interaction_Extinguish>();
    NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("Notifications/StructureAflamed", this.Brain.Data.GetLocalizedName());
  }

  public void Extinguished()
  {
    for (int index = this.transform.childCount - 1; index >= 0; --index)
    {
      if (this.transform.GetChild(index).tag == "Structure_Hindrance")
      {
        BiomeConstants.Instance.EmitSmokeExplosionVFX(this.transform.GetChild(index).transform.position);
        UnityEngine.Object.Destroy((UnityEngine.Object) this.transform.GetChild(index).gameObject);
      }
    }
    foreach (Behaviour componentsInChild in this.GetComponentsInChildren<Interaction>())
      componentsInChild.enabled = true;
    Interaction_Extinguish component = this.GetComponent<Interaction_Extinguish>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) component);
  }

  [CompilerGenerated]
  public void \u003CCollapse\u003Eb__54_0(AsyncOperationHandle<GameObject> obj)
  {
    obj.Result.GetComponent<StructureRubble>().Configure(StructuresData.GetBuildRubbleType(this.Brain.Data.Type));
  }

  [CompilerGenerated]
  public void \u003CSnowedUnder\u003Eb__55_0(AsyncOperationHandle<GameObject> obj)
  {
    if (!this.Brain.Data.IsSnowedUnder)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) obj.Result);
    }
    else
    {
      obj.Result.transform.localScale = Vector3.zero;
      obj.Result.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
      if (this.Brain.Data.Bounds.x < 3)
        return;
      obj.Result.GetComponent<StructureRubble>().SetBig();
    }
  }

  public delegate void StructureInventoryChanged(Structure structure, InventoryItem item);
}
