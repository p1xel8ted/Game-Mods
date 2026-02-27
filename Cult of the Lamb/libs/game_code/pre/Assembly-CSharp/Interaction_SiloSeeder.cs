// Decompiled with JetBrains decompiler
// Type: Interaction_SiloSeeder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class Interaction_SiloSeeder : Interaction
{
  public static List<Interaction_SiloSeeder> SiloSeeders = new List<Interaction_SiloSeeder>();
  public Structure Structure;
  private Structures_SiloSeed _StructureInfo;
  public Canvas CapacityIndicatorCanvas;
  public Image CapacityIndicator;
  public GameObject DepositIndicatorPrefab;
  public List<GameObject> FullStates;
  private List<InventoryItem> _itemsInTheAir;
  private float _delay;

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public Structures_SiloSeed StructureBrain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_SiloSeed;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.ActivateDistance = 2f;
    this.CapacityIndicatorCanvas.gameObject.SetActive(false);
    this._itemsInTheAir = new List<InventoryItem>();
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
  }

  private void OnBrainAssigned()
  {
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this.UpdateCapacityIndicators();
    Interaction_SiloSeeder.SiloSeeders.Add(this);
  }

  public override void OnDisableInteraction()
  {
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    base.OnDisableInteraction();
  }

  protected override void OnDestroy()
  {
    base.OnDestroy();
    Interaction_SiloSeeder.SiloSeeders.Remove(this);
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.Interactable = false;
    state.CURRENT_STATE = StateMachine.State.InActive;
    state.facingAngle = Utils.GetAngle(state.transform.position, this.transform.position);
    UIItemSelectorOverlayController itemSelector = MonoSingleton<UIManager>.Instance.ShowItemSelector(InventoryItem.AllSeeds, new ItemSelector.Params()
    {
      Key = "plant_seeds",
      Context = ItemSelector.Context.Add,
      Offset = new Vector2(0.0f, 250f),
      RequiresDiscovery = true,
      HideOnSelection = false,
      ShowEmpty = true
    });
    itemSelector.OnItemChosen += (System.Action<InventoryItem.ITEM_TYPE>) (chosenItem =>
    {
      Debug.Log((object) $"ItemToDeposit {chosenItem}".Colour(Color.yellow));
      this._itemsInTheAir.Add(new InventoryItem(chosenItem, 1));
      Inventory.ChangeItemQuantity((int) chosenItem, -1);
      AudioManager.Instance.PlayOneShot("event:/material/footstep_grass", this.transform.position);
      ResourceCustomTarget.Create(this.gameObject, PlayerFarming.Instance.transform.position, chosenItem, new System.Action(this.DepositItem));
      this.Interactable = (double) this.GetCompostAndAirCount() < (double) this.StructureBrain.Capacity;
      if (this.Interactable)
        return;
      itemSelector.Hide();
      this.HasChanged = true;
    });
    UIItemSelectorOverlayController overlayController = itemSelector;
    overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() =>
    {
      state.CURRENT_STATE = StateMachine.State.Idle;
      itemSelector = (UIItemSelectorOverlayController) null;
      this.Interactable = true;
      this.HasChanged = true;
    });
  }

  public override void GetLabel()
  {
    this.Interactable = (double) this.GetCompostAndAirCount() < (double) this.StructureBrain.Capacity;
    this.Label = (double) this.GetCompostAndAirCount() >= (double) this.StructureBrain.Capacity ? ScriptLocalization.Interactions.Full : ScriptLocalization.Interactions_Bank.Deposit;
  }

  public override void OnBecomeCurrent()
  {
    base.OnBecomeCurrent();
    this.CapacityIndicatorCanvas.gameObject.SetActive(true);
    this.UpdateCapacityIndicators();
  }

  public override void OnBecomeNotCurrent()
  {
    base.OnBecomeNotCurrent();
    this.CapacityIndicatorCanvas.gameObject.SetActive(false);
  }

  private void DepositItem()
  {
    this.Structure.DepositInventory(this._itemsInTheAir[0]);
    this._itemsInTheAir.RemoveAt(0);
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

  private int GetCompostAndAirCount() => this.GetCompostCount() + this._itemsInTheAir.Count;
}
