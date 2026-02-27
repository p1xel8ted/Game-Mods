// Decompiled with JetBrains decompiler
// Type: Interaction_SiloFertilizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class Interaction_SiloFertilizer : Interaction
{
  public static List<Interaction_SiloFertilizer> SiloFertilizers = new List<Interaction_SiloFertilizer>();
  public Structure Structure;
  private Structures_SiloFertiliser _StructureInfo;
  public Canvas CapacityIndicatorCanvas;
  public Image CapacityIndicator;
  public GameObject DepositIndicatorPrefab;
  public List<GameObject> FullStates;
  private bool _activating;
  private float _delay;
  private int _currentlyDepositing;

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public Structures_SiloFertiliser StructureBrain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_SiloFertiliser;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.CapacityIndicatorCanvas.gameObject.SetActive(false);
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
  }

  private void OnBrainAssigned()
  {
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this.UpdateCapacityIndicators();
    Interaction_SiloFertilizer.SiloFertilizers.Add(this);
  }

  protected override void OnDestroy()
  {
    base.OnDestroy();
    Interaction_SiloFertilizer.SiloFertilizers.Remove(this);
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (this.AtCapacity())
      return;
    state.CURRENT_STATE = StateMachine.State.InActive;
    state.facingAngle = Utils.GetAngle(state.transform.position, this.transform.position);
    UIManager instance = MonoSingleton<UIManager>.Instance;
    List<InventoryItem.ITEM_TYPE> items = new List<InventoryItem.ITEM_TYPE>();
    items.Add(InventoryItem.ITEM_TYPE.POOP);
    ItemSelector.Params parameters = new ItemSelector.Params()
    {
      Key = "fertiliser",
      Context = ItemSelector.Context.Add,
      Offset = new Vector2(0.0f, 250f),
      ShowEmpty = true
    };
    UIItemSelectorOverlayController itemSelector = instance.ShowItemSelector(items, parameters);
    itemSelector.OnItemChosen += (System.Action<InventoryItem.ITEM_TYPE>) (chosenItem => AddFertilizer((System.Action) (() => itemSelector.Hide())));
    UIItemSelectorOverlayController overlayController = itemSelector;
    overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() =>
    {
      state.CURRENT_STATE = StateMachine.State.Idle;
      itemSelector = (UIItemSelectorOverlayController) null;
      this.Interactable = !this.AtCapacity() && this.HasRequiredItem();
      this.HasChanged = true;
      this.GetLabel();
    });
    Interaction_SiloFertilizer interactionSiloFertilizer;

    void AddFertilizer(System.Action onFull)
    {
      ++interactionSiloFertilizer._currentlyDepositing;
      InventoryItem.ITEM_TYPE itemType = InventoryItem.ITEM_TYPE.POOP;
      InventoryItem item = new InventoryItem(itemType, 1);
      ResourceCustomTarget.Create(interactionSiloFertilizer.gameObject, PlayerFarming.Instance.transform.position, itemType, (System.Action) (() => interactionSiloFertilizer.DepositItem(item)));
      Inventory.ChangeItemQuantity((int) itemType, -1);
      if (!interactionSiloFertilizer.AtCapacity() || onFull == null)
        return;
      onFull();
    }
  }

  private string GetAffordColor()
  {
    return Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.POOP) > 0 ? "<color=#f4ecd3>" : "<color=red>";
  }

  public override void GetLabel()
  {
    if (this.AtCapacity())
      this.Label = ScriptLocalization.Interactions.Full;
    else
      this.Label = string.Join(" ", ScriptLocalization.Interactions_Bank.Deposit, CostFormatter.FormatCost(InventoryItem.ITEM_TYPE.POOP, 1));
  }

  private bool AtCapacity()
  {
    return (double) (this.Structure.Inventory.Count + this._currentlyDepositing) >= (double) this.StructureBrain.Capacity;
  }

  private bool HasRequiredItem() => Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.POOP) > 0;

  public override void OnBecomeCurrent()
  {
    base.OnBecomeCurrent();
    this.CapacityIndicatorCanvas.gameObject.SetActive(true);
    this.UpdateCapacityIndicators();
    this.Interactable = !this.AtCapacity() && this.HasRequiredItem();
  }

  public override void OnBecomeNotCurrent()
  {
    base.OnBecomeNotCurrent();
    this.CapacityIndicatorCanvas.gameObject.SetActive(false);
  }

  private void DepositItem(InventoryItem item)
  {
    --this._currentlyDepositing;
    this.Structure.DepositInventory(item);
    this.UpdateCapacityIndicators();
  }

  public void UpdateCapacityIndicators()
  {
    float num1 = (float) this.GetCompostCount() / this.StructureBrain.Capacity;
    this.CapacityIndicator.fillAmount = num1;
    int num2 = -1;
    if (this.GetCompostCount() > 0)
      num2 = Mathf.FloorToInt(num1 * (float) (this.FullStates.Count - 1));
    for (int index = 0; index < this.FullStates.Count; ++index)
      this.FullStates[index].SetActive(index == num2);
  }

  private int GetCompostCount()
  {
    int compostCount = 0;
    foreach (InventoryItem inventoryItem in this.StructureInfo.Inventory)
      compostCount += inventoryItem.quantity;
    return compostCount;
  }
}
