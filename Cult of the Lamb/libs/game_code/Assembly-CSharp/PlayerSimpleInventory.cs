// Decompiled with JetBrains decompiler
// Type: PlayerSimpleInventory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
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
