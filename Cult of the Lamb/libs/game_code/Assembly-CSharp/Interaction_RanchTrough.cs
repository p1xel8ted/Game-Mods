// Decompiled with JetBrains decompiler
// Type: Interaction_RanchTrough
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using MMTools;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class Interaction_RanchTrough : Interaction
{
  public static List<Interaction_RanchTrough> Troughs = new List<Interaction_RanchTrough>();
  public Structure Structure;
  public Structures_RanchTrough _StructureInfo;
  public Canvas CapacityIndicatorCanvas;
  public Image CapacityIndicator;
  public GameObject DepositIndicatorPrefab;
  public List<GameObject> FullStates;
  public List<InventoryItem> _itemsInTheAir;

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public Structures_RanchTrough StructureBrain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_RanchTrough;
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

  public void OnBrainAssigned()
  {
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    Interaction_RanchTrough.Troughs.Add(this);
    this.Structure.Brain.OnItemDeposited += new System.Action(this.UpdateCapacityIndicators);
    for (int index = this.Structure.Brain.Data.Inventory.Count - 1; index >= 0; --index)
    {
      if (index >= this.StructureBrain.Capacity)
        this.Structure.Brain.Data.Inventory.RemoveAt(index);
    }
    this.UpdateCapacityIndicators();
  }

  public override void OnDisableInteraction()
  {
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    base.OnDisableInteraction();
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if ((UnityEngine.Object) this.Structure != (UnityEngine.Object) null && this.Structure.Brain != null)
      this.Structure.Brain.OnItemDeposited -= new System.Action(this.UpdateCapacityIndicators);
    Interaction_RanchTrough.Troughs.Remove(this);
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.Interactable = false;
    state.CURRENT_STATE = StateMachine.State.InActive;
    state.facingAngle = Utils.GetAngle(state.transform.position, this.transform.position);
    this.StructureBrain.ReservedByPlayer = true;
    UIItemSelectorOverlayController itemSelector = MonoSingleton<UIManager>.Instance.ShowItemSelector(this.playerFarming, InventoryItem.AllAnimalFood, new ItemSelector.Params()
    {
      Key = "grass",
      Context = ItemSelector.Context.Add,
      Offset = new Vector2(0.0f, 250f),
      RequiresDiscovery = true,
      HideOnSelection = false,
      ShowEmpty = true
    }, true);
    itemSelector.OnItemChosen += (System.Action<InventoryItem.ITEM_TYPE>) (chosenItem =>
    {
      Debug.Log((object) $"ItemToDeposit {chosenItem}".Colour(Color.yellow));
      this._itemsInTheAir.Add(new InventoryItem(chosenItem, 1));
      Inventory.ChangeItemQuantity((int) chosenItem, -1);
      AudioManager.Instance.PlayOneShot("event:/material/footstep_grass", this.transform.position);
      ResourceCustomTarget.Create(this.gameObject, this.playerFarming.transform.position, chosenItem, new System.Action(this.DepositItem));
      this.Interactable = this.GetCompostAndAirCount() < this.StructureBrain.Capacity;
      if (this.Interactable)
        return;
      itemSelector.Hide();
      this.HasChanged = true;
    });
    UIItemSelectorOverlayController overlayController = itemSelector;
    overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() =>
    {
      state.CURRENT_STATE = LetterBox.IsPlaying || MMConversation.isPlaying ? StateMachine.State.InActive : StateMachine.State.Idle;
      itemSelector = (UIItemSelectorOverlayController) null;
      this.Interactable = true;
      this.HasChanged = true;
      this.StructureBrain.ReservedByPlayer = false;
    });
  }

  public override void GetLabel()
  {
    if (Interaction_WolfBase.WolvesActive)
    {
      this.Interactable = false;
      this.Label = "";
    }
    else
    {
      this.Interactable = this.GetCompostAndAirCount() < this.StructureBrain.Capacity;
      if (LocalizeIntegration.IsArabic())
        this.Label = this.GetCompostAndAirCount() >= this.StructureBrain.Capacity ? $"{ScriptLocalization.Interactions.Full} ){LocalizeIntegration.FormatCurrentMax(this.GetCompostAndAirCount().ToString(), this.StructureBrain.Capacity.ToString())}(" : ScriptLocalization.Interactions_Bank.Deposit;
      else
        this.Label = this.GetCompostAndAirCount() >= this.StructureBrain.Capacity ? $"{ScriptLocalization.Interactions.Full} ({LocalizeIntegration.FormatCurrentMax(this.GetCompostAndAirCount().ToString(), this.StructureBrain.Capacity.ToString())})" : ScriptLocalization.Interactions_Bank.Deposit;
    }
  }

  public override void OnBecomeCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeCurrent(playerFarming);
    this.CapacityIndicatorCanvas.gameObject.SetActive(true);
    this.UpdateCapacityIndicators();
  }

  public override void OnBecomeNotCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeNotCurrent(playerFarming);
    this.CapacityIndicatorCanvas.gameObject.SetActive(false);
  }

  public void DepositItem()
  {
    this.Structure.DepositInventory(this._itemsInTheAir[0]);
    this._itemsInTheAir.RemoveAt(0);
    this.UpdateCapacityIndicators();
  }

  public void UpdateCapacityIndicators()
  {
    float num1 = Mathf.Clamp01((float) this.GetCompostCount() / (float) this.StructureBrain.Capacity);
    this.CapacityIndicator.fillAmount = num1;
    int num2 = -1;
    if (this.GetCompostCount() > 0)
      num2 = Mathf.FloorToInt(num1 * (float) (this.FullStates.Count - 1));
    for (int index = 0; index < this.FullStates.Count; ++index)
      this.FullStates[index].SetActive(index == num2);
  }

  public int GetCompostCount() => this.StructureInfo.Inventory.Count;

  public int GetCompostAndAirCount() => this.GetCompostCount() + this._itemsInTheAir.Count;
}
