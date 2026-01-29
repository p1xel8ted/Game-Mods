// Decompiled with JetBrains decompiler
// Type: SetInventoryIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
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
