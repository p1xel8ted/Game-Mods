// Decompiled with JetBrains decompiler
// Type: Interaction_SiloSeeder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using MMTools;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class Interaction_SiloSeeder : Interaction
{
  public static List<Interaction_SiloSeeder> SiloSeeders = new List<Interaction_SiloSeeder>();
  public Structure Structure;
  public Structures_SiloSeed _StructureInfo;
  public Canvas CapacityIndicatorCanvas;
  public Image CapacityIndicator;
  public GameObject DepositIndicatorPrefab;
  public List<GameObject> FullStates;
  public int _itemsInTheAir;
  public float _delay;
  public bool DepositAllEnabled;

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
    if (this.Structure.Type == global::StructureBrain.TYPES.SEED_BUCKET)
    {
      this.DepositAllEnabled = true;
      this.HasSecondaryInteraction = true;
    }
    this.CapacityIndicatorCanvas.gameObject.SetActive(false);
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
  }

  public void OnBrainAssigned()
  {
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    Interaction_SiloSeeder.SiloSeeders.Add(this);
    for (int index = this.Structure.Brain.Data.Inventory.Count - 1; index >= 0; --index)
    {
      if ((double) index >= (double) this.StructureBrain.Capacity)
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
    Interaction_SiloSeeder.SiloSeeders.Remove(this);
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.Interactable = false;
    state.CURRENT_STATE = StateMachine.State.InActive;
    state.facingAngle = Utils.GetAngle(state.transform.position, this.transform.position);
    this.StructureBrain.ReservedByPlayer = true;
    UIItemSelectorOverlayController itemSelector = MonoSingleton<UIManager>.Instance.ShowItemSelector(this.playerFarming, InventoryItem.AllSeeds, new ItemSelector.Params()
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
      InventoryItem item = new InventoryItem(chosenItem, 1);
      Inventory.ChangeItemQuantity((int) chosenItem, -1);
      AudioManager.Instance.PlayOneShot("event:/material/footstep_grass", this.transform.position);
      ++this._itemsInTheAir;
      ResourceCustomTarget.Create(this.gameObject, this.playerFarming.transform.position, chosenItem, (System.Action) (() => this.DepositItem(item)));
      this.Interactable = (double) this.GetCompostAndAirCount() < (double) this.StructureBrain.Capacity;
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
    this.Interactable = (double) this.GetCompostAndAirCount() < (double) this.StructureBrain.Capacity;
    if (LocalizeIntegration.IsArabic())
      this.Label = (double) this.GetCompostAndAirCount() >= (double) this.StructureBrain.Capacity ? $"{ScriptLocalization.Interactions.Full} ){LocalizeIntegration.FormatCurrentMax(this.GetCompostAndAirCount().ToString(), this.StructureBrain.Capacity.ToString())}(" : ScriptLocalization.Interactions_Bank.Deposit;
    else
      this.Label = (double) this.GetCompostAndAirCount() >= (double) this.StructureBrain.Capacity ? $"{ScriptLocalization.Interactions.Full} ({LocalizeIntegration.FormatCurrentMax(this.GetCompostAndAirCount().ToString(), this.StructureBrain.Capacity.ToString())})" : ScriptLocalization.Interactions_Bank.Deposit;
  }

  public override void GetSecondaryLabel()
  {
    if (!this.DepositAllEnabled)
      return;
    this.SecondaryInteractable = (double) this.GetCompostAndAirCount() < (double) this.StructureBrain.Capacity;
    this.SecondaryLabel = (double) this.GetCompostAndAirCount() >= (double) this.StructureBrain.Capacity ? "" : ScriptLocalization.Interactions.DepositAllSeeds;
  }

  public override void OnSecondaryInteract(StateMachine state)
  {
    if (!this.DepositAllEnabled || (double) this.GetCompostAndAirCount() >= (double) this.StructureBrain.Capacity)
      return;
    int num = 0;
    foreach (InventoryItem.ITEM_TYPE allSeed in InventoryItem.AllSeeds)
      num += Inventory.GetItemQuantity(allSeed);
    if (num <= 0)
      return;
    base.OnSecondaryInteract(state);
    foreach (InventoryItem.ITEM_TYPE allSeed in InventoryItem.AllSeeds)
    {
      int itemQuantity = Inventory.GetItemQuantity(allSeed);
      if (itemQuantity > 0)
      {
        int a = Mathf.Min((int) this.StructureBrain.Capacity - this.Structure.InventoryTotalCount, itemQuantity);
        if (a > 0)
        {
          Inventory.ChangeItemQuantity(allSeed, -a);
          for (int index = 0; index < a; ++index)
            this.Structure.Inventory.Add(new InventoryItem(allSeed, 1));
          int message = Mathf.Min(a, 4);
          while (message-- > 0)
          {
            Debug.Log((object) message);
            ResourceCustomTarget.Create(this.gameObject, this.playerFarming.transform.position, allSeed, (System.Action) (() =>
            {
              this.UpdateCapacityIndicators();
              this.HasChanged = true;
              this.GetLabel();
            }));
          }
        }
      }
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

  public void DepositItem(InventoryItem item)
  {
    --this._itemsInTheAir;
    this.Structure.DepositInventory(item);
    this.UpdateCapacityIndicators();
  }

  public void UpdateCapacityIndicators()
  {
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

  public int GetCompostAndAirCount() => this.GetCompostCount() + this._itemsInTheAir;

  [CompilerGenerated]
  public void \u003COnSecondaryInteract\u003Eb__22_0()
  {
    this.UpdateCapacityIndicators();
    this.HasChanged = true;
    this.GetLabel();
  }
}
