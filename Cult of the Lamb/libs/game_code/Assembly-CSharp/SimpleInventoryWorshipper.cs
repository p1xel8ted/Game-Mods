// Decompiled with JetBrains decompiler
// Type: SimpleInventoryWorshipper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class SimpleInventoryWorshipper : SimpleInventory
{
  public SimpleSpineAnimator SpineAnimator;

  public override InventoryItem.ITEM_TYPE Item
  {
    get => this.inventoryitem;
    set
    {
      InventoryItem.ITEM_TYPE inventoryitem = this.inventoryitem;
      this.inventoryitem = value;
      this.ItemImage.SetImage(this.Item);
      if (value == inventoryitem)
        return;
      if (this.inventoryitem == InventoryItem.ITEM_TYPE.NONE)
      {
        this.SpineAnimator.ChangeStateAnimation(StateMachine.State.Idle, "idle");
        this.SpineAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");
      }
      else
      {
        this.SpineAnimator.ChangeStateAnimation(StateMachine.State.Idle, "idle-item");
        this.SpineAnimator.ChangeStateAnimation(StateMachine.State.Moving, "walk-item");
      }
    }
  }
}
