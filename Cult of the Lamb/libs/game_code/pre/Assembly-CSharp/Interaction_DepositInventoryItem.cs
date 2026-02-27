// Decompiled with JetBrains decompiler
// Type: Interaction_DepositInventoryItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_DepositInventoryItem : Interaction
{
  private Structure structure;
  private SimpleInventory PlayerInventory;
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
