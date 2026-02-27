// Decompiled with JetBrains decompiler
// Type: Interaction_CollectResourceChest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using UnityEngine;

#nullable disable
public class Interaction_CollectResourceChest : Interaction
{
  public GameObject ChestOpen;
  public GameObject ChestClosed;
  public Structure Structure;
  private StructureBrain _StructureInfo;
  private string sString;
  private Vector3 PunchScale = new Vector3(0.1f, 0.1f, 0.1f);
  private bool Activating;
  private GameObject Player;
  private float Delay;
  public float DistanceToTriggerDeposits = 5f;

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

  private void Start() => this.UpdateLocalisation();

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    this.ContinuouslyHold = true;
    if (this.StructureBrain != null)
      this.StructureBrain.OnItemDeposited += new System.Action(this.DepositItem);
    this.UpdateChest();
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    if (this.StructureBrain == null)
      return;
    this.StructureBrain.OnItemDeposited -= new System.Action(this.DepositItem);
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sString = ScriptLocalization.Interactions.ReceiveDevotion;
  }

  private void OnBrainAssigned()
  {
    this.StructureBrain.OnItemDeposited += new System.Action(this.DepositItem);
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

  private void UpdateChest()
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

  protected override void Update()
  {
    base.Update();
    if ((UnityEngine.Object) (this.Player = GameObject.FindWithTag("Player")) == (UnityEngine.Object) null)
      return;
    this.GetLabel();
    if (this.Activating && (this.StructureInfo.Inventory.Count <= 0 || InputManager.Gameplay.GetInteractButtonUp() || (double) Vector3.Distance(this.transform.position, this.Player.transform.position) > (double) this.DistanceToTriggerDeposits))
      this.Activating = false;
    if ((double) (this.Delay -= Time.deltaTime) >= 0.0 || !this.Activating)
      return;
    InventoryItem inventoryItem = this.StructureInfo.Inventory[0];
    InventoryItem.ITEM_TYPE type = (InventoryItem.ITEM_TYPE) inventoryItem.type;
    AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", this.transform.position);
    for (int index = 0; index < Mathf.Min(inventoryItem.quantity, 5); ++index)
      ResourceCustomTarget.Create(this.Player.gameObject, this.transform.position, type, (System.Action) null);
    this.GiveItem(type, inventoryItem.quantity);
    this.StructureInfo.Inventory.RemoveAt(0);
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

  private void GiveItem(InventoryItem.ITEM_TYPE type, int amount)
  {
    Inventory.AddItem((int) type, amount);
  }
}
