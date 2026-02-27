// Decompiled with JetBrains decompiler
// Type: Interaction_CookedFoodSilo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using UnityEngine;

#nullable disable
public class Interaction_CookedFoodSilo : Interaction
{
  private Structure structure;
  private SimpleInventory PlayerInventory;
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
