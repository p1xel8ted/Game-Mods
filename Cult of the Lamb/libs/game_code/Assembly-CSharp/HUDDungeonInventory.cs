// Decompiled with JetBrains decompiler
// Type: HUDDungeonInventory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class HUDDungeonInventory : BaseMonoBehaviour
{
  public List<HUD_InventoryIcon> Icons = new List<HUD_InventoryIcon>();

  public void OnEnable()
  {
    this.PopulateInventory();
    Inventory.OnItemAddedToDungeonInventory += new Inventory.ItemAddedToDungeonInventory(this.ItemAddedToInventory);
  }

  public void OnDisable()
  {
    Inventory.OnItemAddedToDungeonInventory -= new Inventory.ItemAddedToDungeonInventory(this.ItemAddedToInventory);
  }

  public void ItemAddedToInventory(InventoryItem.ITEM_TYPE ItemType) => this.PopulateInventory();

  public void PopulateInventory()
  {
    int index = -1;
    while (++index < this.Icons.Count)
    {
      if (index > Inventory.itemsDungeon.Count - 1)
      {
        this.Icons[index].gameObject.SetActive(false);
      }
      else
      {
        this.Icons[index].gameObject.SetActive(true);
        this.Icons[index].Init(Inventory.itemsDungeon[index]);
      }
    }
  }
}
