// Decompiled with JetBrains decompiler
// Type: Interaction_FixBridge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_FixBridge : Interaction
{
  public BaseBridge baseBridge;
  public Structure structure;
  public SimpleInventory PlayerInventory;
  public List<InventoryItem.ITEM_TYPE> AllowedItemTypes = new List<InventoryItem.ITEM_TYPE>();

  public void Start() => this.structure = this.GetComponent<Structure>();

  public override void GetLabel()
  {
    if (this.structure.Inventory.Count < 3)
    {
      this.HoldToInteract = false;
      this.Interactable = false;
      if ((Object) this.PlayerInventory == (Object) null)
      {
        GameObject withTag = GameObject.FindWithTag("Player");
        if (!((Object) withTag != (Object) null))
          return;
        this.PlayerInventory = withTag.GetComponent<SimpleInventory>();
      }
      else
      {
        if (!this.AllowedItemTypes.Contains(this.PlayerInventory.GetItemType()))
          return;
        this.Interactable = true;
      }
    }
    else
    {
      this.Interactable = true;
      this.HoldToInteract = true;
    }
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.Label == "Fix Bridge")
    {
      base.OnInteract(state);
    }
    else
    {
      SimpleInventory component = state.gameObject.GetComponent<SimpleInventory>();
      if (!((Object) component != (Object) null) || !this.AllowedItemTypes.Contains(component.GetItemType()))
        return;
      InventoryItem inventoryItem = new InventoryItem();
      inventoryItem.Init((int) component.GetItemType(), 1);
      this.structure.DepositInventory(inventoryItem);
      component.RemoveItem();
    }
  }
}
