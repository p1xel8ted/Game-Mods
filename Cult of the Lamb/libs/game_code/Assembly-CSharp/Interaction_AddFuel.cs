// Decompiled with JetBrains decompiler
// Type: Interaction_AddFuel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using MMTools;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_AddFuel : Interaction
{
  [SerializeField]
  public Structure structure;
  [SerializeField]
  public List<InventoryItem.ITEM_TYPE> fuel = new List<InventoryItem.ITEM_TYPE>();
  [SerializeField]
  public string fuelKey = "ordinary_fuel";
  public int MaxFuel = 10;
  [Space]
  [SerializeField]
  public bool onlyDepleteWhenFullyFueled;
  [SerializeField]
  public bool hideIfEmpty;
  [SerializeField]
  public bool winterOnly;
  [SerializeField]
  public bool canAddWhenNotFull;
  public Interaction OtherInteraction;
  [Space]
  [SerializeField]
  public UnityEvent onFireOn;
  [SerializeField]
  public UnityEvent onFireOff;
  [SerializeField]
  public UnityEvent onFireFullyFueld;
  [SerializeField]
  public Vector3 fuelBarOffset;
  [SerializeField]
  public GameObject noFuelIcon;
  public Coroutine addFuelRoutine;
  public UIAddFuel fuelUI;
  public bool beingMoved;
  public bool activiating;
  public bool firstPress;
  public float delay;
  public int oldFuelAmount = -1;
  public bool StringForHealingBay;
  public UIItemSelectorOverlayController itemSelector;

  public Structure Structure => this.structure;

  public event Interaction_AddFuel.FuelEvent OnFuelModified;

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    if ((UnityEngine.Object) this.fuelUI == (UnityEngine.Object) null)
      this.fuelUI = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/UI/UI Add Fuel"), GameObject.FindGameObjectWithTag("Canvas").transform).GetComponent<UIAddFuel>();
    this.fuelUI.offset = this.fuelBarOffset;
    PlacementRegion.OnBuildingBeganMoving += new PlacementRegion.BuildingEvent(this.OnBuildingBeganMoving);
    PlacementRegion.OnBuildingPlaced += new PlacementRegion.BuildingEvent(this.OnBuildingPlaced);
    this.structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
  }

  public virtual void OnBrainAssigned()
  {
    this.structure.Structure_Info.onlyDepleteWhenFullyFueled = this.onlyDepleteWhenFullyFueled;
    this.structure.Structure_Info.MaxFuel = this.MaxFuel;
    this.structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this.structure.Brain.OnFuelModified += new System.Action<float>(this.OnBrainFuelModified);
  }

  public void OnBrainFuelModified(float Delta)
  {
    Interaction_AddFuel.FuelEvent onFuelModified = this.OnFuelModified;
    if (onFuelModified == null)
      return;
    onFuelModified(Delta);
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    PlacementRegion.OnBuildingBeganMoving -= new PlacementRegion.BuildingEvent(this.OnBuildingBeganMoving);
    PlacementRegion.OnBuildingPlaced -= new PlacementRegion.BuildingEvent(this.OnBuildingPlaced);
    if ((bool) (UnityEngine.Object) this.structure)
      this.structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    if ((bool) (UnityEngine.Object) this.structure && this.structure.Brain != null)
      this.structure.Brain.OnFuelModified -= new System.Action<float>(this.OnBrainFuelModified);
    this.activiating = false;
    this.fuelUI?.Hide();
  }

  public override void Update()
  {
    base.Update();
    if (!((UnityEngine.Object) this.structure != (UnityEngine.Object) null) || this.structure.Structure_Info == null || this.structure.Structure_Info.Fuel == this.oldFuelAmount)
      return;
    if (this.structure.Structure_Info.Fuel > 0)
    {
      this.onFireOn?.Invoke();
    }
    else
    {
      this.structure.Structure_Info.FullyFueled = false;
      this.onFireOff?.Invoke();
      this.Interactable = true;
    }
    if (this.structure.Structure_Info.FullyFueled)
      this.onFireFullyFueld?.Invoke();
    this.oldFuelAmount = this.structure.Structure_Info.Fuel;
  }

  public void OnBuildingBeganMoving(int structureID)
  {
    int num = structureID;
    int? id = this.structure?.Structure_Info?.ID;
    int valueOrDefault = id.GetValueOrDefault();
    if (!(num == valueOrDefault & id.HasValue) || !(bool) (UnityEngine.Object) this.fuelUI)
      return;
    this.fuelUI.Hide();
    this.beingMoved = true;
  }

  public void OnBuildingPlaced(int structureID)
  {
    int num = structureID;
    int? id = this.structure?.Structure_Info?.ID;
    int valueOrDefault = id.GetValueOrDefault();
    if (!(num == valueOrDefault & id.HasValue) || !(bool) (UnityEngine.Object) this.fuelUI)
      return;
    this.beingMoved = false;
  }

  public override void GetLabel()
  {
    base.GetLabel();
    if (this.winterOnly && SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter)
      this.Label = "";
    else if (!this.structure.Structure_Info.FullyFueled || this.canAddWhenNotFull && this.structure.Structure_Info.Fuel < this.structure.Structure_Info.MaxFuel)
    {
      this.Interactable = true;
      if (this.StringForHealingBay)
        this.Label = ScriptLocalization.Interactions.AddIngredients;
      else if (this.fuel.Count == 1)
        this.Label = $"{ScriptLocalization.Interactions.AddFuel} {CostFormatter.FormatCost(this.fuel[0], 1)}";
      else
        this.Label = ScriptLocalization.Interactions.AddFuel;
    }
    else
    {
      this.Label = ScriptLocalization.Interactions.Full;
      this.Interactable = false;
    }
  }

  public override void OnInteract(StateMachine state)
  {
    PlayerFarming component = state.GetComponent<PlayerFarming>();
    if ((!this.canAddWhenNotFull || this.structure.Structure_Info.Fuel >= this.structure.Structure_Info.MaxFuel) && this.structure.Structure_Info.FullyFueled)
      return;
    state.CURRENT_STATE = StateMachine.State.InActive;
    state.facingAngle = Utils.GetAngle(state.transform.position, this.transform.position);
    this.itemSelector = MonoSingleton<UIManager>.Instance.ShowItemSelector(component, this.fuel, new ItemSelector.Params()
    {
      Key = this.fuelKey,
      Context = ItemSelector.Context.Add,
      Offset = new Vector2(0.0f, 150f),
      ShowEmpty = true
    });
    this.itemSelector.OnItemChosen += (System.Action<InventoryItem.ITEM_TYPE>) (chosenItem => this.AddFuel(chosenItem, onFull: (System.Action) (() => this.itemSelector.Hide())));
    UIItemSelectorOverlayController itemSelector = this.itemSelector;
    itemSelector.OnHidden = itemSelector.OnHidden + (System.Action) (() => this.OnItemSelectorHidden(state));
  }

  public virtual void OnItemSelectorHidden(StateMachine state)
  {
    state.CURRENT_STATE = LetterBox.IsPlaying || MMConversation.isPlaying ? StateMachine.State.InActive : StateMachine.State.Idle;
    this.itemSelector = (UIItemSelectorOverlayController) null;
    this.Interactable = !this.structure.Structure_Info.FullyFueled || this.canAddWhenNotFull && this.structure.Structure_Info.Fuel < this.structure.Structure_Info.MaxFuel;
    this.HasChanged = true;
  }

  public virtual void AddFuel(
    InventoryItem.ITEM_TYPE itemType,
    bool changeItemQuantity = true,
    System.Action onFull = null,
    bool playSfx = true)
  {
    Debug.Log((object) $"Deposit {itemType} to add fuel".Colour(Color.yellow));
    if (itemType == InventoryItem.ITEM_TYPE.LOG)
    {
      if (playSfx)
        AudioManager.Instance.PlayOneShot("event:/cooking/add_wood", this.transform.position);
    }
    else if (playSfx)
      AudioManager.Instance.PlayOneShot("event:/material/footstep_bush", this.transform.position);
    this.SpawnFuelItem(itemType);
    if (changeItemQuantity)
      Inventory.ChangeItemQuantity((int) itemType, -1);
    this.structure.Structure_Info.Fuel = Mathf.Clamp(this.structure.Structure_Info.Fuel + InventoryItem.FuelWeight(itemType), 0, this.structure.Structure_Info.MaxFuel);
    Interaction_AddFuel.FuelEvent onFuelModified1 = this.OnFuelModified;
    if (onFuelModified1 != null)
      onFuelModified1((float) this.structure.Structure_Info.Fuel / (float) this.structure.Structure_Info.MaxFuel);
    this.structure.Structure_Info.PhaseAddedFuel = TimeManager.CurrentPhase;
    if (this.structure.Structure_Info.Fuel < this.structure.Structure_Info.MaxFuel)
      return;
    if (playSfx)
      AudioManager.Instance.PlayOneShot("event:/cooking/fire_start", this.transform.position);
    this.structure.Structure_Info.FullyFueled = true;
    this.onFireFullyFueld?.Invoke();
    Interaction_AddFuel.FuelEvent onFuelModified2 = this.OnFuelModified;
    if (onFuelModified2 != null)
      onFuelModified2(1f);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.AddFuelToCookingFire);
    if (this.structure.Type != StructureBrain.TYPES.FURNACE_1 && this.structure.Type != StructureBrain.TYPES.FURNACE_2 && this.structure.Type != StructureBrain.TYPES.FURNACE_3)
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.AddFuelToCookingFire);
    if (onFull == null)
      return;
    onFull();
  }

  public virtual void SpawnFuelItem(InventoryItem.ITEM_TYPE itemType)
  {
    ResourceCustomTarget.Create(this.gameObject, this.playerFarming.transform.position, itemType, (System.Action) null);
  }

  public override void OnBecomeCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeCurrent(playerFarming);
    this.Interactable = !this.structure.Structure_Info.FullyFueled || this.canAddWhenNotFull && this.structure.Structure_Info.Fuel < this.structure.Structure_Info.MaxFuel;
    if (this.fuelUI.IsShowing || !this.Interactable)
      return;
    this.fuelUI.Show(this);
    if (!(bool) (UnityEngine.Object) this.noFuelIcon)
      return;
    this.noFuelIcon.transform.DOKill();
    if (!(this.noFuelIcon.transform.localScale != Vector3.zero))
      return;
    this.noFuelIcon.transform.localScale = Vector3.one;
    this.noFuelIcon.transform.DOScale(0.0f, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.noFuelIcon.gameObject.SetActive(false)));
  }

  public override void OnBecomeNotCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeNotCurrent(playerFarming);
    if (!this.fuelUI.IsShowing || !((UnityEngine.Object) this.itemSelector == (UnityEngine.Object) null) && !this.itemSelector.IsHiding)
      return;
    this.fuelUI.Hide();
    if (!(bool) (UnityEngine.Object) this.noFuelIcon || this.Structure.Brain.Data.FullyFueled)
      return;
    this.noFuelIcon.transform.DOKill();
    this.noFuelIcon.transform.localScale = Vector3.zero;
    this.noFuelIcon.gameObject.SetActive(true);
    this.noFuelIcon.transform.DOScale(1f, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
  }

  [CompilerGenerated]
  public void \u003COnBecomeCurrent\u003Eb__41_0() => this.noFuelIcon.gameObject.SetActive(false);

  public delegate void FuelEvent(float fuel);
}
