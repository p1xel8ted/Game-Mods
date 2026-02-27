// Decompiled with JetBrains decompiler
// Type: Interaction_FoodStorage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class Interaction_FoodStorage : Interaction
{
  public static List<Interaction_FoodStorage> FoodStorages = new List<Interaction_FoodStorage>();
  public Structure Structure;
  public Structures_FoodStorage _StructureInfo;
  public Canvas CapacityIndicatorCanvas;
  public Image CapacityIndicator;
  public GameObject FoodIndicatorPrefab;
  public SpriteRenderer RangeSprite;
  public InventoryItemDisplay[] itemDisplays;
  public bool showing;
  public GameObject _player;
  public List<InventoryItem> FoodInTheAir;
  public Color FadeOut = new Color(1f, 1f, 1f, 0.0f);
  public float DistanceRadius = 1f;
  public float Distance = 1f;
  public int FrameIntervalOffset;
  public int UpdateInterval = 2;
  public bool distanceChanged;
  public Vector3 _updatePos;
  public float CurrentCapacity;

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public Structures_FoodStorage StructureBrain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_FoodStorage;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    Interaction_FoodStorage.FoodStorages.Remove(this);
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    if ((UnityEngine.Object) this.GetComponentInParent<PlacementObject>() == (UnityEngine.Object) null)
      this.RangeSprite.DOColor(this.FadeOut, 0.0f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    this.UpdateLocalisation();
    this.CapacityIndicatorCanvas.gameObject.SetActive(false);
    foreach (Component itemDisplay in this.itemDisplays)
      itemDisplay.gameObject.SetActive(false);
    this.FoodInTheAir = new List<InventoryItem>();
    Interaction_FoodStorage.FoodStorages.Add(this);
    if (this.StructureInfo != null)
      this.UpdateFoodDisplayed();
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    if (this.Structure.Brain == null)
      return;
    this.OnBrainAssigned();
  }

  public override void OnBecomeCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeCurrent(playerFarming);
    this.UpdateCapacityIndicator();
  }

  public void OnBrainAssigned()
  {
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    Structures_FoodStorage structureBrain = this.StructureBrain;
    structureBrain.OnItemDeposited = structureBrain.OnItemDeposited + new System.Action(this.OnFoodWithdrawn);
    this.StructureBrain.OnFoodWithdrawn += new System.Action(this.OnFoodWithdrawn);
    this.UpdateFoodDisplayed();
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    if (this.StructureBrain == null)
      return;
    Structures_FoodStorage structureBrain = this.StructureBrain;
    structureBrain.OnItemDeposited = structureBrain.OnItemDeposited - new System.Action(this.OnFoodWithdrawn);
    this.StructureBrain.OnFoodWithdrawn -= new System.Action(this.OnFoodWithdrawn);
  }

  public void OnFoodWithdrawn() => this.UpdateCapacityIndicator();

  public void UpdateCapacityIndicator()
  {
    this.CurrentCapacity = 0.0f;
    foreach (InventoryItem inventoryItem in this.StructureInfo.Inventory)
      this.CurrentCapacity += (float) inventoryItem.quantity;
    this.CapacityIndicator.fillAmount = this.CurrentCapacity / (float) this.StructureBrain.Capacity;
    this.UpdateFoodDisplayed();
  }

  public float GetFoodCount()
  {
    float num = 0.0f;
    foreach (InventoryItem inventoryItem in this.StructureInfo.Inventory)
      ++num;
    return num / 10f;
  }

  public float GetFoodAndAirCount()
  {
    this.CurrentCapacity = 0.0f;
    foreach (InventoryItem inventoryItem in this.StructureInfo.Inventory)
      this.CurrentCapacity += (float) inventoryItem.quantity;
    return this.CurrentCapacity + (float) this.FoodInTheAir.Count;
  }

  public void UpdateFoodDisplayed()
  {
    foreach (Component itemDisplay in this.itemDisplays)
      itemDisplay.gameObject.SetActive(false);
    for (int index = 0; index < this.StructureInfo.Inventory.Count; ++index)
    {
      if (index < this.itemDisplays.Length && this.StructureInfo.Inventory[index] != null)
      {
        this.itemDisplays[index].gameObject.SetActive(true);
        this.itemDisplays[index].SetImage((InventoryItem.ITEM_TYPE) this.StructureInfo.Inventory[index].type, false);
      }
    }
  }

  public void UpdateFoodDisplayed(InventoryItem item)
  {
    for (int index = 0; index < this.StructureInfo.Inventory.Count; ++index)
    {
      if (index < this.itemDisplays.Length && this.StructureInfo.Inventory[index] == item)
      {
        this.itemDisplays[index].gameObject.SetActive(true);
        this.itemDisplays[index].SetImage((InventoryItem.ITEM_TYPE) this.StructureInfo.Inventory[index].type, false);
      }
    }
  }
}
