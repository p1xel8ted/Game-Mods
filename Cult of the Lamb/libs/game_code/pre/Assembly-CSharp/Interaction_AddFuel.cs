// Decompiled with JetBrains decompiler
// Type: Interaction_AddFuel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_AddFuel : Interaction
{
  [SerializeField]
  private Structure structure;
  [SerializeField]
  private List<InventoryItem.ITEM_TYPE> fuel = new List<InventoryItem.ITEM_TYPE>();
  [SerializeField]
  public string fuelKey = "ordinary_fuel";
  public int MaxFuel = 10;
  [Space]
  [SerializeField]
  private bool onlyDepleteWhenFullyFueled;
  [SerializeField]
  private bool hideIfEmpty;
  public Interaction OtherInteraction;
  [Space]
  [SerializeField]
  private UnityEvent onFireOn;
  [SerializeField]
  private UnityEvent onFireOff;
  [SerializeField]
  private UnityEvent onFireFullyFueld;
  [SerializeField]
  private Vector3 fuelBarOffset;
  [SerializeField]
  private GameObject noFuelIcon;
  private Coroutine addFuelRoutine;
  private UIAddFuel fuelUI;
  private bool beingMoved;
  private bool activiating;
  private bool firstPress;
  private float delay;
  private int oldFuelAmount = -1;
  public bool StringForHealingBay;

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

  private void OnBrainAssigned()
  {
    this.structure.Structure_Info.onlyDepleteWhenFullyFueled = this.onlyDepleteWhenFullyFueled;
    this.structure.Structure_Info.MaxFuel = this.MaxFuel;
    this.structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this.structure.Brain.OnFuelModified += new System.Action<float>(this.OnBrainFuelModified);
  }

  private void OnBrainFuelModified(float Delta)
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

  protected override void Update()
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

  private void OnBuildingBeganMoving(int structureID)
  {
    int num = structureID;
    int? id = this.structure?.Structure_Info?.ID;
    int valueOrDefault = id.GetValueOrDefault();
    if (!(num == valueOrDefault & id.HasValue) || !(bool) (UnityEngine.Object) this.fuelUI)
      return;
    this.fuelUI.Hide();
    this.beingMoved = true;
  }

  private void OnBuildingPlaced(int structureID)
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
    if (!this.structure.Structure_Info.FullyFueled)
    {
      if (this.StringForHealingBay)
        this.Label = ScriptLocalization.Interactions.AddIngredients;
      else if (this.fuel.Count == 1)
        this.Label = $"{ScriptLocalization.Interactions.AddFuel} {CostFormatter.FormatCost(this.fuel[0], 1)}";
      else
        this.Label = ScriptLocalization.Interactions.AddFuel;
    }
    else
      this.Label = ScriptLocalization.Interactions.Full;
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.structure.Structure_Info.FullyFueled)
      return;
    state.CURRENT_STATE = StateMachine.State.InActive;
    state.facingAngle = Utils.GetAngle(state.transform.position, this.transform.position);
    UIItemSelectorOverlayController itemSelector = MonoSingleton<UIManager>.Instance.ShowItemSelector(this.fuel, new ItemSelector.Params()
    {
      Key = this.fuelKey,
      Context = ItemSelector.Context.Add,
      Offset = new Vector2(0.0f, 150f),
      ShowEmpty = true
    });
    itemSelector.OnItemChosen += (System.Action<InventoryItem.ITEM_TYPE>) (chosenItem => AddFuel(chosenItem, (System.Action) (() => itemSelector.Hide())));
    UIItemSelectorOverlayController overlayController = itemSelector;
    overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() =>
    {
      state.CURRENT_STATE = StateMachine.State.Idle;
      itemSelector = (UIItemSelectorOverlayController) null;
      this.Interactable = !this.structure.Structure_Info.FullyFueled;
      this.HasChanged = true;
    });

    void AddFuel(InventoryItem.ITEM_TYPE itemType, System.Action onFull)
    {
      Debug.Log((object) $"Deposit {itemType} to add fuel".Colour(Color.yellow));
      if (itemType == InventoryItem.ITEM_TYPE.LOG)
        AudioManager.Instance.PlayOneShot("event:/cooking/add_wood", this.transform.position);
      else
        AudioManager.Instance.PlayOneShot("event:/material/footstep_bush", this.transform.position);
      ResourceCustomTarget.Create(this.gameObject, PlayerFarming.Instance.transform.position, itemType, (System.Action) null);
      Inventory.ChangeItemQuantity((int) itemType, -1);
      this.structure.Structure_Info.Fuel = Mathf.Clamp(this.structure.Structure_Info.Fuel + InventoryItem.FuelWeight(itemType), 0, this.structure.Structure_Info.MaxFuel);
      Interaction_AddFuel.FuelEvent onFuelModified1 = this.OnFuelModified;
      if (onFuelModified1 != null)
        onFuelModified1((float) this.structure.Structure_Info.Fuel / (float) this.structure.Structure_Info.MaxFuel);
      if (this.structure.Structure_Info.Fuel < this.structure.Structure_Info.MaxFuel)
        return;
      AudioManager.Instance.PlayOneShot("event:/cooking/fire_start", this.transform.position);
      this.structure.Structure_Info.FullyFueled = true;
      this.onFireFullyFueld?.Invoke();
      Interaction_AddFuel.FuelEvent onFuelModified2 = this.OnFuelModified;
      if (onFuelModified2 != null)
        onFuelModified2(1f);
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.AddFuelToCookingFire);
      if (onFull == null)
        return;
      onFull();
    }
  }

  public override void OnBecomeCurrent()
  {
    base.OnBecomeCurrent();
    this.Interactable = !this.structure.Structure_Info.FullyFueled;
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

  public override void OnBecomeNotCurrent()
  {
    base.OnBecomeNotCurrent();
    if (!this.fuelUI.IsShowing)
      return;
    this.fuelUI.Hide();
    if (!(bool) (UnityEngine.Object) this.noFuelIcon || this.Structure.Brain.Data.FullyFueled)
      return;
    this.noFuelIcon.transform.DOKill();
    this.noFuelIcon.transform.localScale = Vector3.zero;
    this.noFuelIcon.gameObject.SetActive(true);
    this.noFuelIcon.transform.DOScale(1f, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
  }

  public delegate void FuelEvent(float fuel);
}
