// Decompiled with JetBrains decompiler
// Type: PlayerSimpleInventory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class PlayerSimpleInventory : SimpleInventory
{
  public override InventoryItem.ITEM_TYPE Item
  {
    get => DataManager.Instance.SimpleInventoryItem;
    set
    {
      this.inventoryitem = value;
      DataManager.Instance.SimpleInventoryItem = this.inventoryitem;
    }
  }

  private void Start() => this.ItemImage.SetImage(this.Item);

  public void DepositItem()
  {
    Inventory.AddItem((int) this.Item, 1);
    this.RemoveItem();
  }
}
