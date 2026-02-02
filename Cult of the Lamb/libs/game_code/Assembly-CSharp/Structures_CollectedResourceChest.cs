// Decompiled with JetBrains decompiler
// Type: Structures_CollectedResourceChest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class Structures_CollectedResourceChest : StructureBrain
{
  public void AddItem(InventoryItem.ITEM_TYPE ItemType, int Quantity)
  {
    foreach (InventoryItem inventoryItem in this.Data.Inventory)
    {
      if ((InventoryItem.ITEM_TYPE) inventoryItem.type == ItemType)
      {
        inventoryItem.quantity += Quantity;
        return;
      }
    }
    this.Data.Inventory.Add(new InventoryItem(ItemType, Quantity));
    System.Action onItemDeposited = this.OnItemDeposited;
    if (onItemDeposited == null)
      return;
    onItemDeposited();
  }
}
