// Decompiled with JetBrains decompiler
// Type: Interaction_DepositInventoryItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_DepositInventoryItem : Interaction
{
  public Structure structure;
  public SimpleInventory PlayerInventory;
  public List<InventoryItem.ITEM_TYPE> AllowedItemTypes = new List<InventoryItem.ITEM_TYPE>();

  public void Start() => this.structure = this.GetComponent<Structure>();

  public override void GetLabel()
  {
    this.Label = "";
    if (!((Object) this.PlayerInventory == (Object) null))
      return;
    GameObject withTag = GameObject.FindWithTag("Player");
    if (!((Object) withTag != (Object) null))
      return;
    this.PlayerInventory = withTag.GetComponent<SimpleInventory>();
  }

  public override void OnInteract(StateMachine state)
  {
    SimpleInventory component = state.gameObject.GetComponent<SimpleInventory>();
    if ((Object) component != (Object) null && this.AllowedItemTypes.Contains(component.GetItemType()))
    {
      InventoryItem inventoryItem = new InventoryItem();
      inventoryItem.Init((int) component.GetItemType(), 1);
      this.structure.DepositInventory(inventoryItem);
      component.RemoveItem();
    }
    base.OnInteract(state);
  }
}
