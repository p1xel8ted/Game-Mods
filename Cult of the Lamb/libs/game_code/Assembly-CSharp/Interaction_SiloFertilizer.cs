// Decompiled with JetBrains decompiler
// Type: Interaction_SiloFertilizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using MMTools;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class Interaction_SiloFertilizer : Interaction
{
  public static List<Interaction_SiloFertilizer> SiloFertilizers = new List<Interaction_SiloFertilizer>();
  public Structure Structure;
  public Structures_SiloFertiliser _StructureInfo;
  public Canvas CapacityIndicatorCanvas;
  public Image CapacityIndicator;
  public GameObject DepositIndicatorPrefab;
  public List<GameObject> FullStates;
  public bool _activating;
  public float _delay;
  public int _currentlyDepositing;
  public bool DepositAllEnabled;

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
    this.Structure.OnBrainRemoved += new System.Action(this.OnBrainRemoved);
  }

  public override void OnDisableInteraction()
  {
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    base.OnDisableInteraction();
  }

  public void OnBrainAssigned()
  {
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    if (this.Structure.Type == global::StructureBrain.TYPES.POOP_BUCKET)
    {
      this.DepositAllEnabled = true;
      this.HasSecondaryInteraction = true;
    }
    Interaction_SiloFertilizer.SiloFertilizers.Add(this);
    Structures_SiloFertiliser structureBrain1 = this.StructureBrain;
    structureBrain1.OnItemDeposited = structureBrain1.OnItemDeposited + new System.Action(this.OnItemQuantityChange);
    Structures_SiloFertiliser structureBrain2 = this.StructureBrain;
    structureBrain2.OnItemRemoved = structureBrain2.OnItemRemoved + new System.Action(this.OnItemQuantityChange);
    for (int index = this.Structure.Brain.Data.Inventory.Count - 1; index >= 0; --index)
    {
      if ((double) index >= (double) this.StructureBrain.Capacity)
        this.Structure.Brain.Data.Inventory.RemoveAt(index);
    }
    this.UpdateCapacityIndicators();
  }

  public void OnBrainRemoved()
  {
    this.Structure.OnBrainRemoved -= new System.Action(this.OnBrainRemoved);
    Structures_SiloFertiliser structureBrain1 = this.StructureBrain;
    structureBrain1.OnItemDeposited = structureBrain1.OnItemDeposited - new System.Action(this.OnItemQuantityChange);
    Structures_SiloFertiliser structureBrain2 = this.StructureBrain;
    structureBrain2.OnItemRemoved = structureBrain2.OnItemRemoved - new System.Action(this.OnItemQuantityChange);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    Interaction_SiloFertilizer.SiloFertilizers.Remove(this);
    if (this.StructureBrain == null)
      return;
    Structures_SiloFertiliser structureBrain1 = this.StructureBrain;
    structureBrain1.OnItemDeposited = structureBrain1.OnItemDeposited - new System.Action(this.OnItemQuantityChange);
    Structures_SiloFertiliser structureBrain2 = this.StructureBrain;
    structureBrain2.OnItemRemoved = structureBrain2.OnItemRemoved - new System.Action(this.OnItemQuantityChange);
  }

  public override void OnInteract(StateMachine state)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    Interaction_SiloFertilizer.\u003C\u003Ec__DisplayClass21_0 cDisplayClass210 = new Interaction_SiloFertilizer.\u003C\u003Ec__DisplayClass21_0();
    // ISSUE: reference to a compiler-generated field
    cDisplayClass210.state = state;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass210.\u003C\u003E4__this = this;
    // ISSUE: reference to a compiler-generated field
    base.OnInteract(cDisplayClass210.state);
    if (this.AtCapacity())
      return;
    state.CURRENT_STATE = StateMachine.State.InActive;
    state.facingAngle = Utils.GetAngle(state.transform.position, this.transform.position);
    this.StructureBrain.ReservedByPlayer = true;
    UIItemSelectorOverlayController itemSelector = MonoSingleton<UIManager>.Instance.ShowItemSelector(this.playerFarming, InventoryItem.AllPoops, new ItemSelector.Params()
    {
      Key = "fertiliser",
      Context = ItemSelector.Context.Add,
      Offset = new Vector2(0.0f, 250f),
      ShowEmpty = true,
      RequiresDiscovery = true
    });
    // ISSUE: reference to a compiler-generated method
    itemSelector.OnItemChosen += (System.Action<InventoryItem.ITEM_TYPE>) (chosenItem => cDisplayClass210.\u003COnInteract\u003Eg__AddFertilizer\u007C0(chosenItem, (System.Action) (() => itemSelector.Hide())));
    UIItemSelectorOverlayController overlayController = itemSelector;
    overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() =>
    {
      state.CURRENT_STATE = LetterBox.IsPlaying || MMConversation.isPlaying ? StateMachine.State.InActive : StateMachine.State.Idle;
      itemSelector = (UIItemSelectorOverlayController) null;
      this.Interactable = !this.AtCapacity() && this.HasRequiredItem();
      this.GetLabel();
      this.HasChanged = true;
      this.StructureBrain.ReservedByPlayer = false;
    });
  }

  public string GetAffordColor()
  {
    return Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.POOP) > 0 ? "<color=#f4ecd3>" : "<color=red>";
  }

  public override void GetLabel()
  {
    if (this.AtCapacity())
    {
      if (LocalizeIntegration.IsArabic())
        this.Label = $"{ScriptLocalization.Interactions.Full} ){LocalizeIntegration.FormatCurrentMax((this.Structure.InventoryTotalCount + this._currentlyDepositing).ToString(), this.StructureBrain.Capacity.ToString())}(";
      else
        this.Label = $"{ScriptLocalization.Interactions.Full} ({LocalizeIntegration.FormatCurrentMax((this.Structure.InventoryTotalCount + this._currentlyDepositing).ToString(), this.StructureBrain.Capacity.ToString())})";
    }
    else
      this.Label = string.Join(" ", ScriptLocalization.Interactions_Bank.Deposit, this.FormatCost(1));
  }

  public override void GetSecondaryLabel()
  {
    if (!this.DepositAllEnabled)
      return;
    this.SecondaryInteractable = !this.AtCapacity();
    string str;
    if (!this.AtCapacity())
      str = string.Join(" ", ScriptLocalization.Interactions.DepositAllPoop, this.FormatCost(this.GetAllPoopQuanitity()));
    else
      str = "";
    this.SecondaryLabel = str;
  }

  public override void OnSecondaryInteract(StateMachine state)
  {
    if (!this.DepositAllEnabled || !this.HasRequiredItem() || this.AtCapacity())
      return;
    base.OnSecondaryInteract(state);
    foreach (InventoryItem.ITEM_TYPE allPoop in InventoryItem.AllPoops)
    {
      int itemQuantity = Inventory.GetItemQuantity(allPoop);
      if (itemQuantity > 0)
      {
        int a = Mathf.Min((int) this.StructureBrain.Capacity - this.Structure.InventoryTotalCount, itemQuantity);
        if (a > 0)
        {
          Inventory.ChangeItemQuantity(allPoop, -a);
          for (int index = 0; index < a; ++index)
            this.Structure.Inventory.Add(new InventoryItem(allPoop, 1));
          int message = Mathf.Min(a, 10);
          while (message-- > 0)
          {
            Debug.Log((object) message);
            ResourceCustomTarget.Create(this.gameObject, this.playerFarming.transform.position, allPoop, (System.Action) (() =>
            {
              this.UpdateCapacityIndicators();
              this.HasChanged = true;
              this.Interactable = !this.AtCapacity() && this.HasRequiredItem();
              this.GetLabel();
            }));
          }
        }
      }
    }
  }

  public bool AtCapacity()
  {
    return (double) (this.Structure.InventoryTotalCount + this._currentlyDepositing) >= (double) this.StructureBrain.Capacity;
  }

  public bool HasRequiredItem()
  {
    foreach (InventoryItem.ITEM_TYPE allPoop in InventoryItem.AllPoops)
    {
      if (Inventory.GetItemQuantity(allPoop) > 0)
        return true;
    }
    return false;
  }

  public int GetAllPoopQuanitity()
  {
    return Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.POOP) + Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.POOP_DEVOTION) + Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.POOP_GLOW) + Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.POOP_GOLD) + Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.POOP_RAINBOW) + Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.POOP_ROTSTONE);
  }

  public string FormatCost(int cost)
  {
    string str1 = $"{FontImageNames.GetIconByType(InventoryItem.ITEM_TYPE.POOP)} {(object) cost}".Bold();
    string str2 = !LocalizeIntegration.IsArabic() ? $"({(object) this.GetAllPoopQuanitity()})".Size(-2) : $"){LocalizeIntegration.ReverseText(this.GetAllPoopQuanitity().ToString())}(".Size(-2);
    return cost > this.GetAllPoopQuanitity() ? $"{str1.Colour("#FD1D03")} {str2.Colour("#BA1400")}" : $"{str1} {str2.Colour(StaticColors.GreyColor)}";
  }

  public override void OnBecomeCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeCurrent(playerFarming);
    this.CapacityIndicatorCanvas.gameObject.SetActive(true);
    this.UpdateCapacityIndicators();
    this.Interactable = !this.AtCapacity() && this.HasRequiredItem();
  }

  public override void OnBecomeNotCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeNotCurrent(playerFarming);
    this.CapacityIndicatorCanvas.gameObject.SetActive(false);
  }

  public void DepositItem(InventoryItem item)
  {
    --this._currentlyDepositing;
    this.StructureBrain.DepositItem((InventoryItem.ITEM_TYPE) item.type);
    this.UpdateCapacityIndicators();
  }

  public void UpdateCapacityIndicators()
  {
    if (this.StructureBrain == null || (UnityEngine.Object) this.CapacityIndicator == (UnityEngine.Object) null)
      return;
    float num1 = Mathf.Clamp01((float) this.GetCompostCount() / this.StructureBrain.Capacity);
    this.CapacityIndicator.fillAmount = num1;
    int num2 = -1;
    if (this.GetCompostCount() > 0)
      num2 = Mathf.FloorToInt(num1 * (float) (this.FullStates.Count - 1));
    for (int index = 0; index < this.FullStates.Count; ++index)
      this.FullStates[index].SetActive(index == num2);
  }

  public int GetCompostCount()
  {
    int compostCount = 0;
    foreach (InventoryItem inventoryItem in this.StructureInfo.Inventory)
      compostCount += inventoryItem.quantity;
    return compostCount;
  }

  public void OnItemQuantityChange()
  {
    this.HasChanged = true;
    this.UpdateCapacityIndicators();
  }

  [CompilerGenerated]
  public void \u003COnSecondaryInteract\u003Eb__25_0()
  {
    this.UpdateCapacityIndicators();
    this.HasChanged = true;
    this.Interactable = !this.AtCapacity() && this.HasRequiredItem();
    this.GetLabel();
  }
}
