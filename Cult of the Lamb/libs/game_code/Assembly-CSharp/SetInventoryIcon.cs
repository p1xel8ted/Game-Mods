// Decompiled with JetBrains decompiler
// Type: SetInventoryIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class SetInventoryIcon : BaseMonoBehaviour
{
  public Inventory_Icon inventoryIcon;
  public InventoryItem.ITEM_TYPE Item;

  public void Start()
  {
    InventoryItem itemByType = Inventory.GetItemByType((int) this.Item);
    if (itemByType == null)
      this.inventoryIcon.SetImage((int) this.Item, 0);
    else
      this.inventoryIcon.SetItem(itemByType);
  }
}
