// Decompiled with JetBrains decompiler
// Type: Interaction_FoodStorage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private Structures_FoodStorage _StructureInfo;
  public Canvas CapacityIndicatorCanvas;
  public Image CapacityIndicator;
  public GameObject FoodIndicatorPrefab;
  public SpriteRenderer RangeSprite;
  public InventoryItemDisplay[] itemDisplays;
  private LayerMask playerMask;
  private bool showing;
  private GameObject _player;
  private List<InventoryItem> FoodInTheAir;
  private Color FadeOut = new Color(1f, 1f, 1f, 0.0f);
  private float DistanceRadius = 1f;
  private float Distance = 1f;
  private int FrameIntervalOffset;
  private int UpdateInterval = 2;
  private bool distanceChanged;
  private Vector3 _updatePos;
  private float CurrentCapacity;

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

  private void Start()
  {
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    if (this.Structure.Brain != null)
      this.OnBrainAssigned();
    this.playerMask = (LayerMask) ((int) this.playerMask | 1 << LayerMask.NameToLayer("Player"));
  }

  protected override void OnDestroy()
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
    if (this.StructureInfo == null)
      return;
    this.UpdateFoodDisplayed();
  }

  public override void OnBecomeCurrent()
  {
    base.OnBecomeCurrent();
    this.UpdateCapacityIndicator();
  }

  private void OnBrainAssigned()
  {
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    Structures_FoodStorage structureBrain = this.StructureBrain;
    structureBrain.OnItemDeposited = structureBrain.OnItemDeposited + new System.Action(this.OnFoodWithdrawn);
    this.StructureBrain.OnFoodWithdrawn += new System.Action(this.OnFoodWithdrawn);
    this.UpdateFoodDisplayed();
    this.RangeSprite.size = new Vector2(this.StructureBrain.Range, this.StructureBrain.Range);
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

  private void OnFoodWithdrawn() => this.UpdateCapacityIndicator();

  private new void Update()
  {
    if ((Time.frameCount + this.FrameIntervalOffset) % this.UpdateInterval != 0 || (UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
      return;
    if (!GameManager.overridePlayerPosition)
    {
      this._updatePos = PlayerFarming.Instance.transform.position;
      this.DistanceRadius = 1f;
    }
    else
    {
      this._updatePos = PlacementRegion.Instance.PlacementPosition;
      this.DistanceRadius = this.StructureBrain.Range;
    }
    if ((double) Vector3.Distance(this._updatePos, this.transform.position) < (double) this.DistanceRadius)
    {
      this.RangeSprite.gameObject.SetActive(true);
      this.RangeSprite.DOKill();
      this.RangeSprite.DOColor(StaticColors.OffWhiteColor, 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      this.distanceChanged = true;
      this.UpdateCapacityIndicator();
      this.CapacityIndicatorCanvas.gameObject.SetActive(true);
    }
    else
    {
      if (!this.distanceChanged)
        return;
      this.RangeSprite.DOKill();
      this.RangeSprite.DOColor(this.FadeOut, 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      this.distanceChanged = false;
      this.CapacityIndicatorCanvas.gameObject.SetActive(false);
    }
  }

  private void UpdateCapacityIndicator()
  {
    this.CurrentCapacity = 0.0f;
    foreach (InventoryItem inventoryItem in this.StructureInfo.Inventory)
      this.CurrentCapacity += (float) inventoryItem.quantity;
    this.CapacityIndicator.fillAmount = this.CurrentCapacity / this.StructureBrain.Capacity;
    this.UpdateFoodDisplayed();
  }

  private float GetFoodCount()
  {
    float num = 0.0f;
    foreach (InventoryItem inventoryItem in this.StructureInfo.Inventory)
      ++num;
    return num / 10f;
  }

  private float GetFoodAndAirCount()
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
      if (index < this.itemDisplays.Length)
      {
        this.itemDisplays[index].gameObject.SetActive(true);
        this.itemDisplays[index].SetImage((InventoryItem.ITEM_TYPE) this.StructureInfo.Inventory[index].type);
      }
    }
  }
}
