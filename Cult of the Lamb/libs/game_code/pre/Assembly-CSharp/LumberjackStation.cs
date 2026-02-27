// Decompiled with JetBrains decompiler
// Type: LumberjackStation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private Structures_LumberjackStation _StructureInfo;
  public GameObject FollowerPosition;
  public GameObject ChestPosition;
  public GameObject ChestOpen;
  public GameObject ChestClosed;
  public GameObject ItemIndicator;
  private string sString;
  private Vector3 PunchScale = new Vector3(0.1f, 0.1f, 0.1f);
  private bool Activating;
  private GameObject Player;
  private float Delay;
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

  private void Start()
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

  private void OnBrainAssigned()
  {
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this.StructureBrain.OnExhauted += new System.Action(this.OnExhausted);
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

  private void UpdateChest(bool playSFX = true)
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

  private void MakeExhausted()
  {
    this.StructureInfo.Age = 500;
    this.StructureBrain.IncreaseAge();
  }

  private void OnExhausted()
  {
    this.NormalBuilding.SetActive(false);
    this.ExhaustedBuilding.SetActive(true);
    this.ExhaustedBuilding.transform.DOPunchScale(new Vector3(0.3f, 0.1f), 0.5f, 5, 0.5f);
    if ((UnityEngine.Object) this.Structure != (UnityEngine.Object) null && (this.Structure.Type == global::StructureBrain.TYPES.BLOODSTONE_MINE || this.Structure.Type == global::StructureBrain.TYPES.BLOODSTONE_MINE_2))
      NotificationCentre.Instance.PlayGenericNotification("Notifications/BuildingOutOfResources_Stonemine");
    else
      NotificationCentre.Instance.PlayGenericNotification("Notifications/BuildingOutOfResources_Lumberjack");
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
      this.StructureBrain.OnExhauted -= new System.Action(this.OnExhausted);
    LumberjackStation.LumberjackStations.Remove(this);
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.Activating)
      return;
    base.OnInteract(state);
    this.Activating = true;
  }

  private new void Update()
  {
    if ((UnityEngine.Object) (this.Player = GameObject.FindWithTag("Player")) == (UnityEngine.Object) null)
      return;
    this.GetLabel();
    if (this.Activating && (this.StructureInfo.Inventory.Count <= 0 || InputManager.Gameplay.GetInteractButtonUp() || (double) Vector3.Distance(this.transform.position, this.Player.transform.position) > (double) this.DistanceToTriggerDeposits))
      this.Activating = false;
    if ((double) (this.Delay -= Time.deltaTime) >= 0.0 || !this.Activating)
      return;
    InventoryItem.ITEM_TYPE itemType = (InventoryItem.ITEM_TYPE) this.StructureInfo.Inventory[0].type;
    ResourceCustomTarget.Create(this.Player.gameObject, this.transform.position, itemType, (System.Action) (() => this.GiveItem(itemType)));
    AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", this.transform.position);
    this.StructureInfo.Inventory.RemoveAt(0);
    this.ChestPosition.transform.DOKill();
    this.ChestPosition.transform.localScale = Vector3.one;
    this.ChestPosition.transform.DOPunchScale(this.PunchScale, 1f);
    this.UpdateChest();
    this.Delay = 0.1f;
    if (this.StructureInfo.Inventory.Count > 0 || !this.StructureInfo.Exhausted)
      return;
    Vector3 vector3 = this.transform.position + Vector3.up * 2f;
    BiomeConstants.Instance.EmitSmokeExplosionVFX(vector3);
    if (this.Structure.Type == global::StructureBrain.TYPES.BLOOD_STONE || this.Structure.Type == global::StructureBrain.TYPES.BLOODSTONE_MINE_2)
      AudioManager.Instance.PlayOneShot("event:/material/stone_break", vector3);
    else
      AudioManager.Instance.PlayOneShot("event:/material/wood_break", vector3);
    MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
    this.StructureBrain.Remove();
  }

  private void GiveItem(InventoryItem.ITEM_TYPE type) => Inventory.AddItem((int) type, 1);
}
