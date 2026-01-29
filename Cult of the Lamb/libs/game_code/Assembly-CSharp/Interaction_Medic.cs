// Decompiled with JetBrains decompiler
// Type: Interaction_Medic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using MMTools;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class Interaction_Medic : Interaction
{
  public static List<Interaction_Medic> Medics = new List<Interaction_Medic>();
  public Structure Structure;
  public Structures_Medic _StructureInfo;
  public Canvas CapacityIndicatorCanvas;
  public Image CapacityIndicator;
  public GameObject DepositIndicatorPrefab;
  public List<GameObject> FullStates;
  public List<InventoryItem> _itemsInTheAir;

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public Structures_Medic StructureBrain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_Medic;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }

  public void Start() => Interaction_Medic.Medics.Add(this);

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
    for (int index = this.Structure.Brain.Data.Inventory.Count - 1; index >= 0; --index)
    {
      if (index >= this.StructureBrain.Capacity)
        this.Structure.Brain.Data.Inventory.RemoveAt(index);
    }
    this.Structure.Brain.OnItemDeposited += new System.Action(this.UpdateCapacityIndicators);
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
    Interaction_Medic.Medics.Remove(this);
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.Interactable = false;
    state.CURRENT_STATE = StateMachine.State.InActive;
    state.facingAngle = Utils.GetAngle(state.transform.position, this.transform.position);
    this.StructureBrain.ReservedByPlayer = true;
    UIManager instance = MonoSingleton<UIManager>.Instance;
    PlayerFarming playerFarming = this.playerFarming;
    List<InventoryItem.ITEM_TYPE> items = new List<InventoryItem.ITEM_TYPE>();
    items.Add(InventoryItem.ITEM_TYPE.FLOWER_RED);
    items.Add(InventoryItem.ITEM_TYPE.CRYSTAL);
    items.Add(InventoryItem.ITEM_TYPE.MUSHROOM_SMALL);
    ItemSelector.Params parameters = new ItemSelector.Params()
    {
      Key = "medic",
      Context = ItemSelector.Context.Add,
      Offset = new Vector2(0.0f, 250f),
      RequiresDiscovery = true,
      HideOnSelection = false,
      ShowEmpty = true
    };
    UIItemSelectorOverlayController itemSelector = instance.ShowItemSelector(playerFarming, items, parameters);
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
    this.Interactable = this.GetCompostAndAirCount() < this.StructureBrain.Capacity;
    if (LocalizeIntegration.IsArabic())
    {
      string str;
      if (this.GetCompostAndAirCount() < this.StructureBrain.Capacity)
      {
        str = ScriptLocalization.Interactions_Bank.Deposit;
      }
      else
      {
        string[] strArray = new string[6]
        {
          ScriptLocalization.Interactions.Full,
          " )",
          null,
          null,
          null,
          null
        };
        int num = this.StructureBrain.Capacity;
        strArray[2] = LocalizeIntegration.ReverseText(num.ToString());
        strArray[3] = "/";
        num = this.GetCompostAndAirCount();
        strArray[4] = LocalizeIntegration.ReverseText(num.ToString());
        strArray[5] = "(";
        str = string.Concat(strArray);
      }
      this.Label = str;
    }
    else
    {
      string str;
      if (this.GetCompostAndAirCount() < this.StructureBrain.Capacity)
        str = ScriptLocalization.Interactions_Bank.Deposit;
      else
        str = $"{ScriptLocalization.Interactions.Full} ({this.GetCompostAndAirCount().ToString()}/{this.StructureBrain.Capacity.ToString()})";
      this.Label = str;
    }
  }

  public override void OnBecomeCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeCurrent(playerFarming);
    int num1 = 0;
    int num2 = 0;
    int num3 = 0;
    for (int index = 0; index < this.StructureBrain.Data.Inventory.Count; ++index)
    {
      if (this.StructureBrain.Data.Inventory[index].type == 55)
        ++num1;
      else if (this.StructureBrain.Data.Inventory[index].type == 89)
        ++num2;
      else if (this.StructureBrain.Data.Inventory[index].type == 29)
        ++num3;
    }
    string text = $"{FontImageNames.GetIconByType(InventoryItem.ITEM_TYPE.FLOWER_RED)}{$"({num1})".Colour(Color.gray)} | {FontImageNames.GetIconByType(InventoryItem.ITEM_TYPE.CRYSTAL)}{$"({num2})".Colour(Color.gray)} | {FontImageNames.GetIconByType(InventoryItem.ITEM_TYPE.MUSHROOM_SMALL)}{$"({num3})".Colour(Color.gray)}";
    playerFarming.indicator.ShowTopInfo(text);
    this.CapacityIndicatorCanvas.gameObject.SetActive(true);
    this.UpdateCapacityIndicators();
  }

  public override void OnBecomeNotCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeNotCurrent(playerFarming);
    playerFarming.indicator.HideTopInfo();
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
