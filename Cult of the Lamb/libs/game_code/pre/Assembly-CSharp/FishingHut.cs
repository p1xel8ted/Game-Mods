// Decompiled with JetBrains decompiler
// Type: FishingHut
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FishingHut : Interaction
{
  public static List<FishingHut> FishingHuts = new List<FishingHut>();
  public Structure Structure;
  private Structures_FishingHut _StructureInfo;
  public GameObject FollowerPosition;
  private string sString;
  private bool Activating;
  private GameObject Player;
  private float Delay;
  public float DistanceToTriggerDeposits = 5f;

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public Structures_FishingHut StructureBrain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_FishingHut;
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
    FishingHut.FishingHuts.Add(this);
  }

  public override void GetLabel()
  {
    int count = this.StructureInfo.Inventory.Count;
    this.Interactable = count > 0;
    this.Label = $"{this.sString} <sprite name=\"icon_Fish\"> x{(object) count}";
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    FishingHut.FishingHuts.Remove(this);
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
    AudioManager.Instance.PlayOneShot("event:/followers/pop_in", this.gameObject);
    ResourceCustomTarget.Create(this.state.gameObject, this.transform.position, itemType, (System.Action) (() => this.GiveItem(itemType)));
    this.StructureInfo.Inventory.RemoveAt(0);
    this.Delay = 0.2f;
  }

  private void GiveItem(InventoryItem.ITEM_TYPE type) => Inventory.AddItem((int) type, 1);
}
