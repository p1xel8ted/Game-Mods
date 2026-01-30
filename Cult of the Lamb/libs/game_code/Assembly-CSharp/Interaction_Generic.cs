// Decompiled with JetBrains decompiler
// Type: Interaction_Generic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_Generic : Interaction
{
  [SerializeField]
  [TermsPopup("")]
  public string primaryLabelText = "";
  [SerializeField]
  public bool disablePrimaryInteractionOnInteract;
  [SerializeField]
  public bool requiresItemToInteract;
  [SerializeField]
  public bool usePrimaryCost;
  [SerializeField]
  public Interaction_Generic.CostType costType;
  [SerializeField]
  public InventoryItem.ITEM_TYPE costItem;
  [SerializeField]
  public int primaryCost = 1;
  public UnityEvent OnPrimaryInteractEvent;
  [SerializeField]
  [TermsPopup("")]
  public string secondaryLabelText = "";
  [SerializeField]
  public bool disableSecondaryInteractionOnInteract;
  public UnityEvent OnSecondaryInteractEvent;
  public UnityEvent OnInteractable;
  public UnityEvent OnNonInteractable;

  public string PrimaryLabelLOC => LocalizationManager.GetTranslation(this.primaryLabelText);

  public string SecondaryLabelLOC => LocalizationManager.GetTranslation(this.secondaryLabelText);

  public string GetCostTypeString(Interaction_Generic.CostType costType)
  {
    switch (costType)
    {
      case Interaction_Generic.CostType.Give:
        return ScriptLocalization.UI_ItemSelector_Context.Give;
      case Interaction_Generic.CostType.Plant:
        return ScriptLocalization.UI_ItemSelector_Context.Plant;
      case Interaction_Generic.CostType.Add:
        return ScriptLocalization.UI_ItemSelector_Context.Add;
      case Interaction_Generic.CostType.Buy:
        return ScriptLocalization.UI_ItemSelector_Context.Buy;
      case Interaction_Generic.CostType.Sell:
        return ScriptLocalization.UI_ItemSelector_Context.Sell;
      default:
        return "";
    }
  }

  public void Start()
  {
    if (this.requiresItemToInteract && Inventory.GetItemQuantity(this.costItem) <= 0)
      this.Interactable = false;
    if (this.Interactable)
      this.OnInteractable?.Invoke();
    else
      this.OnNonInteractable?.Invoke();
  }

  public override void OnEnable()
  {
    base.OnEnable();
    if (this.Interactable)
      this.OnInteractable?.Invoke();
    else
      this.OnNonInteractable?.Invoke();
  }

  public override void OnDisable()
  {
    base.OnDisable();
    this.OnNonInteractable?.Invoke();
  }

  public override void GetLabel()
  {
    if (!this.Interactable)
      this.Label = "";
    else if (this.usePrimaryCost)
    {
      string str = CostFormatter.FormatCost(this.costItem, this.primaryCost);
      this.Label = string.Format(this.GetCostTypeString(this.costType), (object) str);
    }
    else
      this.Label = this.PrimaryLabelLOC;
  }

  public override void GetSecondaryLabel()
  {
    this.secondaryLabel = this.SecondaryInteractable ? this.SecondaryLabelLOC : "";
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (this.usePrimaryCost)
    {
      if (Inventory.GetItemQuantity(this.costItem) >= this.primaryCost)
      {
        if (this.disablePrimaryInteractionOnInteract)
        {
          this.OnNonInteractable?.Invoke();
          this.Interactable = false;
        }
        Inventory.ChangeItemQuantity(this.costItem, -this.primaryCost);
        this.OnPrimaryInteractEvent.Invoke();
      }
      else
        this.playerFarming.indicator.PlayShake();
    }
    else
    {
      if (this.disablePrimaryInteractionOnInteract)
      {
        this.Interactable = false;
        this.OnNonInteractable?.Invoke();
      }
      this.OnPrimaryInteractEvent.Invoke();
    }
  }

  public override void OnSecondaryInteract(StateMachine state)
  {
    base.OnSecondaryInteract(state);
    if (this.disableSecondaryInteractionOnInteract)
      this.Interactable = false;
    this.OnSecondaryInteractEvent.Invoke();
  }

  public enum CostType
  {
    Give,
    Plant,
    Add,
    Buy,
    Sell,
  }
}
