// Decompiled with JetBrains decompiler
// Type: Interaction_Outhouse
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_Outhouse : Interaction
{
  public static List<Interaction_Outhouse> Outhouses = new List<Interaction_Outhouse>();
  public Structure _structure;
  public Transform OutsideFollowerPosition;
  public Transform InsideFollowerPosition;
  public Transform WaitingFollowerPosition;
  public GameObject DoorOpen;
  public GameObject DoorClosed;
  public ItemGauge ItemGauge;
  public Structures_Outhouse _StructureInfo;
  public string sString;
  public bool Activating;
  public float Delay;
  public float DistanceToTriggerDeposits = 5f;
  public GameObject FullOuthouse;
  public GameObject NormalOuthouse;
  public bool givenOutfit;

  public StructuresData StructureInfo => this._structure.Structure_Info;

  public Structures_Outhouse StructureBrain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this._structure.Brain as Structures_Outhouse;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    Interaction_Outhouse.Outhouses.Add(this);
    if (this.StructureInfo == null || this.StructureInfo.FollowerID != -1)
      return;
    this.DoorClosed.SetActive(true);
    this.DoorOpen.SetActive(false);
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    Interaction_Outhouse.Outhouses.Remove(this);
  }

  public void Start()
  {
    this.UpdateLocalisation();
    this._structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    if (this._structure.Brain == null)
      return;
    this.OnBrainAssigned();
  }

  public void OnBrainAssigned()
  {
    this._structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this._structure.Brain.OnItemDeposited += new System.Action(this.OnItemDeposited);
    this._structure.Brain.OnItemRemoved += new System.Action(this.OnItemDeposited);
    this.StructureInfo.FollowerID = -1;
    this.OnItemDeposited();
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sString = ScriptLocalization.Interactions.ReceiveDevotion;
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if (this._structure.Brain == null)
      return;
    this._structure.Brain.OnItemDeposited -= new System.Action(this.OnItemDeposited);
    this._structure.Brain.OnItemRemoved -= new System.Action(this.OnItemDeposited);
  }

  public override void GetLabel()
  {
    if (this.StructureBrain == null)
    {
      this.Label = "";
    }
    else
    {
      int poopCount = this.StructureBrain.GetPoopCount();
      this.Interactable = poopCount > 0;
      this.Label = string.Join(" ", this.sString, CostFormatter.FormatCost(InventoryItem.ITEM_TYPE.POOP, poopCount, ignoreAffordability: true));
    }
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.Activating)
      return;
    base.OnInteract(state);
    this.Activating = true;
  }

  public bool IsFull => this.StructureBrain != null && this.StructureBrain.IsFull;

  public override void Update()
  {
    base.Update();
    if ((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null || this.StructureBrain == null)
      return;
    if (this.Activating && (this.StructureBrain.GetPoopCount() <= 0 || InputManager.Gameplay.GetInteractButtonUp(this.playerFarming) || (double) Vector3.Distance(this.transform.position, this.playerFarming.transform.position) > (double) this.DistanceToTriggerDeposits))
      this.Activating = false;
    if (this.StructureBrain.IsFull && !this.FullOuthouse.activeSelf)
    {
      this.FullOuthouse.SetActive(true);
      this.NormalOuthouse.SetActive(false);
    }
    else if (!this.StructureBrain.IsFull && this.FullOuthouse.activeSelf)
    {
      this.FullOuthouse.SetActive(false);
      this.NormalOuthouse.SetActive(true);
    }
    if ((double) (this.Delay -= Time.deltaTime) >= 0.0 || !this.Activating)
      return;
    InventoryItem inventoryItem = this.StructureInfo.Inventory[0];
    InventoryItem.ITEM_TYPE itemType = (InventoryItem.ITEM_TYPE) inventoryItem.type;
    AudioManager.Instance.PlayOneShot("event:/followers/pop_in", this.transform.position);
    ResourceCustomTarget.Create(this.state.gameObject, this.transform.position, itemType, (System.Action) (() => this.GiveItem(itemType)));
    if (--inventoryItem.quantity <= 0)
      this.StructureInfo.Inventory.RemoveAt(0);
    ++DataManager.Instance.itemsCleaned;
    this.CheckItemsCleaned();
    this.GetLabel();
    this.HasChanged = true;
    this.Delay = 0.2f;
  }

  public void CheckItemsCleaned()
  {
    if (DataManager.Instance.itemsCleaned <= DataManager.itemsCleanedNeeded || this.givenOutfit || DataManager.Instance.UnlockedClothing.Contains(FollowerClothingType.Special_4) || !DataManager.Instance.TailorEnabled)
      return;
    InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FOUND_ITEM_OUTFIT, 1, this.transform.position).GetComponent<FoundItemPickUp>().clothingType = FollowerClothingType.Special_4;
    this.givenOutfit = true;
  }

  public void OnItemDeposited() => this.UpdateGauge();

  public void UpdateGauge()
  {
    this.ItemGauge.SetPosition((float) this.StructureBrain.GetPoopCount() / (float) Structures_Outhouse.Capacity(this.StructureInfo.Type));
  }

  public void GiveItem(InventoryItem.ITEM_TYPE type)
  {
    Inventory.AddItem((int) type, 1);
    this.OnItemDeposited();
    this._playerFarming.playerChoreXPBarController.AddChoreXP(this.playerFarming);
  }
}
