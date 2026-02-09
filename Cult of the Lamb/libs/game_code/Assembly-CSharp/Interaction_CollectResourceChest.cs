// Decompiled with JetBrains decompiler
// Type: Interaction_CollectResourceChest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_CollectResourceChest : Interaction
{
  public static List<Interaction_CollectResourceChest> Chests = new List<Interaction_CollectResourceChest>();
  public GameObject ChestOpen;
  public GameObject ChestClosed;
  public Structure Structure;
  public StructureBrain _StructureInfo;
  public float holdingDuration;
  public string sString;
  public Vector3 PunchScale = new Vector3(0.1f, 0.1f, 0.1f);
  public bool Activating;
  public float Delay;
  public float DistanceToTriggerDeposits = 5f;
  public float delayBetweenChecks;
  public const float DELAY_DELTA = 0.4f;

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public StructureBrain StructureBrain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }

  public void Start()
  {
    this.UpdateLocalisation();
    Interaction_CollectResourceChest.Chests.Add(this);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    Interaction_CollectResourceChest.Chests.Remove(this);
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    this.ContinuouslyHold = true;
    if (this.StructureBrain != null)
    {
      this.StructureBrain.OnItemDeposited -= new System.Action(this.DepositItem);
      this.StructureBrain.OnItemRemoved -= new System.Action(this.UpdateChest);
      this.StructureBrain.OnItemDeposited += new System.Action(this.DepositItem);
      this.StructureBrain.OnItemRemoved += new System.Action(this.UpdateChest);
    }
    this.UpdateChest();
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    if (this.StructureBrain == null)
      return;
    this.StructureBrain.OnItemDeposited -= new System.Action(this.DepositItem);
    this.StructureBrain.OnItemRemoved -= new System.Action(this.UpdateChest);
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sString = ScriptLocalization.Interactions.ReceiveDevotion;
  }

  public void OnBrainAssigned()
  {
    this.StructureBrain.OnItemDeposited -= new System.Action(this.DepositItem);
    this.StructureBrain.OnItemRemoved -= new System.Action(this.UpdateChest);
    this.StructureBrain.OnItemDeposited += new System.Action(this.DepositItem);
    this.StructureBrain.OnItemRemoved += new System.Action(this.UpdateChest);
    this.UpdateChest();
  }

  public override void GetLabel()
  {
    if (this.StructureBrain == null)
      this.Label = "";
    else if (this.StructureInfo.Inventory.Count <= 0)
    {
      this.Interactable = false;
      this.Label = "";
    }
    else
    {
      this.Interactable = true;
      this.Label = string.Join(" ", this.sString, CostFormatter.FormatCosts(this.StructureInfo.Inventory, ignoreAffordability: true));
    }
  }

  public void DepositItem()
  {
    this.transform.DOKill();
    this.transform.localScale = Vector3.one;
    this.transform.DOPunchScale(new Vector3(0.2f, 0.1f), 1f, 5);
    if (!this.ChestOpen.activeSelf)
      AudioManager.Instance.PlayOneShot("event:/chests/chest_small_open");
    this.UpdateChest();
  }

  public void UpdateChest()
  {
    if (this.StructureBrain != null && this.StructureBrain.Data != null && this.StructureBrain.Data.Inventory != null && this.StructureBrain.Data.Inventory.Count > 0)
    {
      this.ChestOpen.SetActive(true);
      this.ChestClosed.SetActive(false);
    }
    else
    {
      this.ChestOpen.SetActive(false);
      this.ChestClosed.SetActive(true);
    }
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.Activating || this.StructureBrain.Data.Inventory.Count <= 0)
      return;
    base.OnInteract(state);
    this.Activating = true;
  }

  public override void Update()
  {
    base.Update();
    if ((double) (this.delayBetweenChecks -= Time.deltaTime) >= 0.0 && !InputManager.Gameplay.GetInteractButtonHeld())
    {
      this.Activating = false;
    }
    else
    {
      this.delayBetweenChecks = 0.4f;
      if ((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null)
        return;
      if (this.Activating)
      {
        this.holdingDuration += Time.deltaTime;
        if (this.StructureInfo.Inventory.Count <= 0 || InputManager.Gameplay.GetInteractButtonUp(this.playerFarming) || (double) Vector3.Distance(this.transform.position, this.playerFarming.transform.position) > (double) this.DistanceToTriggerDeposits)
          this.Activating = false;
      }
      else
        this.holdingDuration = 0.0f;
      if ((double) (this.Delay -= Time.deltaTime) >= 0.0 || !this.Activating)
        return;
      if ((double) this.holdingDuration > 4.0)
      {
        AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", this.transform.position);
        int count = this.StructureInfo.Inventory.Count;
        int num1 = Mathf.Min(10, count);
        int num2 = count - num1;
        for (int index = 0; index < num1; ++index)
          this.SpawnItem();
        foreach (InventoryItem inventoryItem in this.StructureInfo.Inventory)
          Inventory.ChangeItemQuantity(inventoryItem.type, 1);
        this.StructureInfo.Inventory.Clear();
      }
      else
        this.SpawnItem();
      this.transform.DOKill();
      this.transform.localScale = Vector3.one;
      this.transform.DOPunchScale(this.PunchScale, 1f);
      if (this.StructureBrain.Data.Inventory.Count <= 0 && this.ChestOpen.activeSelf)
        AudioManager.Instance.PlayOneShot("event:/chests/chest_small_land");
      this.UpdateChest();
      this.Delay = 0.1f;
      if (this.StructureInfo.Inventory.Count > 0 || !this.StructureInfo.Exhausted)
        return;
      this.StructureBrain.Remove();
    }
  }

  public void SpawnItem()
  {
    InventoryItem inventoryItem = this.StructureInfo.Inventory[0];
    InventoryItem.ITEM_TYPE type = (InventoryItem.ITEM_TYPE) inventoryItem.type;
    AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", this.transform.position);
    for (int index = 0; index < Mathf.Min(inventoryItem.quantity, 5); ++index)
      ResourceCustomTarget.Create(this.playerFarming.gameObject, this.transform.position, type, (System.Action) null);
    if (inventoryItem.type == 231)
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.RefineChargedRostone);
    this.GiveItem(type, inventoryItem.quantity);
    this.StructureInfo.Inventory.RemoveAt(0);
  }

  public void GiveItem(InventoryItem.ITEM_TYPE type, int amount)
  {
    Inventory.AddItem((int) type, amount);
  }
}
