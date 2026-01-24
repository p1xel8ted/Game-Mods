// Decompiled with JetBrains decompiler
// Type: Interaction_CookedFoodSilo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using UnityEngine;

#nullable disable
public class Interaction_CookedFoodSilo : Interaction
{
  public Structure structure;
  public SimpleInventory PlayerInventory;
  public string ItemLabel;

  public void Start()
  {
    this.UpdateLocalisation();
    this.structure = this.GetComponent<Structure>();
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.ItemLabel = ScriptLocalization.Inventory.MEAT;
  }

  public override void GetLabel()
  {
    this.Label = "";
    if ((Object) this.PlayerInventory == (Object) null)
    {
      GameObject withTag = GameObject.FindWithTag("Player");
      if (!((Object) withTag != (Object) null))
        return;
      this.PlayerInventory = withTag.GetComponent<SimpleInventory>();
    }
    else
    {
      if (this.structure.Inventory.Count <= 0)
        return;
      this.Label = this.ItemLabel;
    }
  }

  public override void OnInteract(StateMachine state)
  {
    if ((Object) this.PlayerInventory != (Object) null && this.structure.Inventory.Count > 0)
    {
      this.PlayerInventory.GiveItem((InventoryItem.ITEM_TYPE) this.structure.Inventory[0].type);
      this.structure.Inventory.RemoveAt(0);
    }
    base.OnInteract(state);
  }
}
