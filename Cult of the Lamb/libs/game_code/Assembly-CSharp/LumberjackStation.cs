// Decompiled with JetBrains decompiler
// Type: LumberjackStation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class LumberjackStation : Interaction
{
  public static List<LumberjackStation> LumberjackStations = new List<LumberjackStation>();
  public Structure Structure;
  public GameObject NormalBuilding;
  public GameObject ExhaustedBuilding;
  public float holdingDuration;
  public Structures_LumberjackStation _StructureInfo;
  public GameObject FollowerPosition;
  public GameObject ChestPosition;
  public GameObject ChestOpen;
  public GameObject ChestClosed;
  public GameObject ItemIndicator;
  public string sString;
  public Vector3 PunchScale = new Vector3(0.1f, 0.1f, 0.1f);
  public bool Activating;
  public GameObject Player;
  public float Delay;
  public float DistanceToTriggerDeposits = 5f;

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public Structures_LumberjackStation StructureBrain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_LumberjackStation;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }

  public void Start()
  {
    this.UpdateLocalisation();
    this.ContinuouslyHold = true;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sString = ScriptLocalization.Interactions.ReceiveDevotion;
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    LumberjackStation.LumberjackStations.Add(this);
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    if (this.StructureBrain == null)
      return;
    this.OnBrainAssigned();
  }

  public void OnBrainAssigned()
  {
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this.StructureBrain.OnExhauted += new System.Action(this.OnExhausted);
    Structures_LumberjackStation structureBrain1 = this.StructureBrain;
    structureBrain1.OnRepaired = structureBrain1.OnRepaired - new System.Action(this.OnRepaired);
    Structures_LumberjackStation structureBrain2 = this.StructureBrain;
    structureBrain2.OnRepaired = structureBrain2.OnRepaired + new System.Action(this.OnRepaired);
    Structures_LumberjackStation structureBrain3 = this.StructureBrain;
    structureBrain3.OnItemRemoved = structureBrain3.OnItemRemoved - new System.Action(this.UpdateChest);
    Structures_LumberjackStation structureBrain4 = this.StructureBrain;
    structureBrain4.OnItemRemoved = structureBrain4.OnItemRemoved + new System.Action(this.UpdateChest);
    if (this.StructureBrain.Data.Exhausted)
      this.OnExhausted();
    this.UpdateChest();
  }

  public void DepositItem()
  {
    this.ChestPosition.transform.DOKill();
    this.ChestPosition.transform.localScale = Vector3.one;
    this.ChestPosition.transform.DOPunchScale(this.PunchScale, 1f);
    if ((UnityEngine.Object) this.Structure != (UnityEngine.Object) null && (this.Structure.Type == global::StructureBrain.TYPES.BLOODSTONE_MINE || this.Structure.Type == global::StructureBrain.TYPES.BLOODSTONE_MINE_2))
      AudioManager.Instance.PlayOneShot("event:/building/place_building_spot", this.transform.position);
    else
      AudioManager.Instance.PlayOneShot("event:/cooking/add_wood", this.transform.position);
    this.UpdateChest();
  }

  public void UpdateChest()
  {
    if (this.StructureBrain.Data.Inventory.Count > 0)
    {
      this.ItemIndicator.SetActive(true);
      this.ChestOpen.SetActive(true);
      this.ChestClosed.SetActive(false);
    }
    else
    {
      if (!this.ChestClosed.activeSelf)
        AudioManager.Instance.PlayOneShot("event:/chests/chest_small_land", this.transform.position);
      this.ItemIndicator.SetActive(false);
      this.ChestOpen.SetActive(false);
      this.ChestClosed.SetActive(true);
    }
  }

  public void MakeExhausted()
  {
    this.StructureInfo.Age = 500;
    this.StructureBrain.IncreaseAge();
  }

  public void OnExhausted()
  {
    this.NormalBuilding.SetActive(false);
    this.ExhaustedBuilding.SetActive(true);
    this.ExhaustedBuilding.transform.DOPunchScale(new Vector3(0.3f, 0.1f), 0.5f, 5, 0.5f);
    if ((UnityEngine.Object) this.Structure != (UnityEngine.Object) null && (this.Structure.Type == global::StructureBrain.TYPES.BLOODSTONE_MINE || this.Structure.Type == global::StructureBrain.TYPES.BLOODSTONE_MINE_2))
      NotificationCentre.Instance.PlayGenericNotification("Notifications/BuildingOutOfResources_Stonemine");
    else if ((UnityEngine.Object) this.Structure != (UnityEngine.Object) null && (this.Structure.Type == global::StructureBrain.TYPES.ROTSTONE_MINE || this.Structure.Type == global::StructureBrain.TYPES.ROTSTONE_MINE_2))
      NotificationCentre.Instance.PlayGenericNotification("Notifications/BuildingOutOfResources_RotburnMine");
    else
      NotificationCentre.Instance.PlayGenericNotification("Notifications/BuildingOutOfResources_Lumberjack");
    if (!((UnityEngine.Object) this.gameObject.GetComponent<Interaction_RepairStructure>() == (UnityEngine.Object) null))
      return;
    Interaction_RepairStructure interactionRepairStructure = this.gameObject.AddComponent<Interaction_RepairStructure>();
    interactionRepairStructure.Configure(this.Structure, StructuresData.GetBuildRubbleType(this.StructureBrain.Data.Type, true), this.StructureBrain.Data.Bounds.x * 3);
    interactionRepairStructure.ActivateDistance = 1.5f;
    interactionRepairStructure.ActivatorOffset.y = 1f;
    this.enabled = false;
  }

  public void OnRepaired()
  {
    this.StructureBrain.Data.Age = 0;
    this.StructureBrain.Data.Exhausted = false;
    this.ExhaustedBuilding.SetActive(false);
    this.NormalBuilding.SetActive(true);
    this.NormalBuilding.transform.DOPunchScale(new Vector3(0.3f, 0.1f), 0.5f, 5, 0.5f);
  }

  public override void GetLabel()
  {
    if (this.StructureBrain == null)
    {
      this.Label = "";
    }
    else
    {
      int count = this.StructureInfo.Inventory.Count;
      this.Interactable = count > 0;
      this.Label = string.Join(" ", this.sString, CostFormatter.FormatCost(this.StructureInfo.LootToDrop, count, ignoreAffordability: true));
    }
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    if ((bool) (UnityEngine.Object) this.Structure)
      this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    if (this.StructureBrain != null)
    {
      this.StructureBrain.OnExhauted -= new System.Action(this.OnExhausted);
      Structures_LumberjackStation structureBrain = this.StructureBrain;
      structureBrain.OnItemRemoved = structureBrain.OnItemRemoved - new System.Action(this.UpdateChest);
    }
    LumberjackStation.LumberjackStations.Remove(this);
  }

  public new void OnDestroy()
  {
    if (this.StructureBrain == null)
      return;
    Structures_LumberjackStation structureBrain = this.StructureBrain;
    structureBrain.OnRepaired = structureBrain.OnRepaired - new System.Action(this.OnRepaired);
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.Activating)
      return;
    this.Player = state.gameObject;
    base.OnInteract(state);
    this.Activating = true;
  }

  public override void Update()
  {
    base.Update();
    if ((UnityEngine.Object) this.Player == (UnityEngine.Object) null)
      return;
    if (this.Activating)
    {
      this.holdingDuration += Time.deltaTime;
      if (this.StructureInfo.Inventory.Count <= 0 || InputManager.Gameplay.GetInteractButtonUp() || (double) Vector3.Distance(this.transform.position, this.Player.transform.position) > (double) this.DistanceToTriggerDeposits)
        this.Activating = false;
    }
    else
      this.holdingDuration = 0.0f;
    if ((double) (this.Delay -= Time.deltaTime) >= 0.0 || !this.Activating)
      return;
    if ((double) this.holdingDuration > 2.0)
    {
      AudioManager.Instance.PlayOneShot("event:/relics/follower_impact", this.state.gameObject);
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
    this.ChestPosition.transform.DOKill();
    this.ChestPosition.transform.localScale = Vector3.one;
    this.ChestPosition.transform.DOPunchScale(this.PunchScale, 1f);
    this.UpdateChest();
    this.Delay = 0.1f;
    this.ExhaustedCheck();
  }

  public void ExhaustedCheck()
  {
    if (this.StructureInfo.Inventory.Count > 0 || !this.StructureInfo.Exhausted)
      return;
    Vector3 vector3 = this.transform.position + Vector3.up * 2f;
    BiomeConstants.Instance.EmitSmokeExplosionVFX(vector3);
    if (this.Structure.Type == global::StructureBrain.TYPES.BLOOD_STONE || this.Structure.Type == global::StructureBrain.TYPES.BLOODSTONE_MINE_2)
      AudioManager.Instance.PlayOneShot("event:/material/stone_break", vector3);
    else
      AudioManager.Instance.PlayOneShot("event:/material/wood_break", vector3);
    MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact, this.playerFarming);
    this.StructureBrain.Remove();
  }

  public void GiveItem(InventoryItem.ITEM_TYPE type) => Inventory.AddItem((int) type, 1);

  public void SpawnItem()
  {
    InventoryItem.ITEM_TYPE itemType = (InventoryItem.ITEM_TYPE) this.StructureInfo.Inventory[0].type;
    ResourceCustomTarget.Create(this.Player.gameObject, this.transform.position, itemType, (System.Action) (() => this.GiveItem(itemType)));
    AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", this.transform.position);
    this.StructureInfo.Inventory.RemoveAt(0);
  }
}
