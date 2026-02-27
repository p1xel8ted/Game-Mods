// Decompiled with JetBrains decompiler
// Type: SimpleInventoryWorshipper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
