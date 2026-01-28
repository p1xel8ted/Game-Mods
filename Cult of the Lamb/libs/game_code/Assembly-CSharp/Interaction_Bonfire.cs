// Decompiled with JetBrains decompiler
// Type: Interaction_Bonfire
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_Bonfire : Interaction
{
  public SimpleInventory PlayerInventory;
  public List<InventoryItem.ITEM_TYPE> AllowedItemTypesToCook = new List<InventoryItem.ITEM_TYPE>();
  public GameObject FoodDispenser;
  public GameObject FoodDispenserPosition;
  public Structure structure;
  public int MaxWood = 10;
  public GameObject firePitOn;
  public GameObject firePitOff;
  public float FireUsageTime;
  public float _FireUsageTime;
  public int amountOfWood;
  public string sAddFuel;
  public float Delay;
  public GameObject Player;
  public bool Activating;
  public float DistanceToTriggerDeposits = 5f;
  public List<InventoryItem> ToDeposit = new List<InventoryItem>();

  public override void OnEnableInteraction()
  {
    this.ContinuouslyHold = true;
    base.OnEnableInteraction();
    this.updateFire();
  }

  public override void OnDisableInteraction() => base.OnDisableInteraction();

  public override void GetLabel()
  {
    if (Inventory.GetItemQuantity(1) > 0 && this.structure.Inventory.Count < this.MaxWood)
      this.Label = this.sAddFuel;
    else
      this.Label = "";
  }

  public void Start()
  {
    this._FireUsageTime = this.FireUsageTime;
    this.UpdateLocalisation();
  }

  public void updateFire()
  {
    if (this.structure.Inventory.Count > 0)
      this.firePitOn.SetActive(true);
    else
      this.firePitOn.SetActive(false);
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sAddFuel = ScriptLocalization.Interactions.AddFuel;
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.Activating)
      return;
    base.OnInteract(state);
    this.Activating = true;
  }

  public override void Update()
  {
    base.Update();
    this.amountOfWood = this.structure.Inventory.Count;
    if ((Object) (this.Player = GameObject.FindWithTag("Player")) == (Object) null)
      return;
    this.updateFire();
    if (this.Activating && (this.structure.Inventory.Count + this.ToDeposit.Count >= this.MaxWood || InputManager.Gameplay.GetInteractButtonUp() || (double) Vector3.Distance(this.transform.position, this.Player.transform.position) > (double) this.DistanceToTriggerDeposits))
      this.Activating = false;
    if ((double) (this.Delay -= Time.deltaTime) < 0.0 && this.Activating)
      this.Delay = 0.2f;
    if (this.structure.Inventory.Count <= 0 || (double) (this._FireUsageTime -= Time.deltaTime) >= 0.0)
      return;
    this.consumeWood();
    this.updateFire();
    this._FireUsageTime = this.FireUsageTime;
  }

  public void DepositItem()
  {
    this.structure.DepositInventory(this.ToDeposit[0]);
    this.ToDeposit.RemoveAt(0);
  }

  public void consumeWood() => this.structure.Inventory.RemoveAt(0);
}
