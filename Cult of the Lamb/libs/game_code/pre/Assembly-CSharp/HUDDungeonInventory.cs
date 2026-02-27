// Decompiled with JetBrains decompiler
// Type: HUDDungeonInventory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class HUDDungeonInventory : BaseMonoBehaviour
{
  public List<HUD_InventoryIcon> Icons = new List<HUD_InventoryIcon>();

  private void OnEnable()
  {
    this.PopulateInventory();
    Inventory.OnItemAddedToDungeonInventory += new Inventory.ItemAddedToDungeonInventory(this.ItemAddedToInventory);
  }

  private void OnDisable()
  {
    Inventory.OnItemAddedToDungeonInventory -= new Inventory.ItemAddedToDungeonInventory(this.ItemAddedToInventory);
  }

  private void ItemAddedToInventory(InventoryItem.ITEM_TYPE ItemType) => this.PopulateInventory();

  private void PopulateInventory()
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
