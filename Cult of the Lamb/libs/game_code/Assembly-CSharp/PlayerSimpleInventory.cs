// Decompiled with JetBrains decompiler
// Type: PlayerSimpleInventory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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

  public new void Start() => this.ItemImage.SetImage(this.Item);

  public void DepositItem()
  {
    Inventory.AddItem((int) this.Item, 1);
    this.RemoveItem();
  }
}
