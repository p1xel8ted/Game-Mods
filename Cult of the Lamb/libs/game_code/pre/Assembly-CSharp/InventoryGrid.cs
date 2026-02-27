// Decompiled with JetBrains decompiler
// Type: InventoryGrid
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class InventoryGrid : BaseMonoBehaviour
{
  public GameObject IconPrefab;
  public Transform Container;

  private void OnEnable()
  {
    Inventory.OnInventoryUpdated += new Inventory.InventoryUpdated(this.PopulateFromInventory);
  }

  private void OnDisable()
  {
    Inventory.OnInventoryUpdated -= new Inventory.InventoryUpdated(this.PopulateFromInventory);
  }

  private void Start() => this.PopulateFromInventory();

  private void PopulateFromInventory() => this.Populate(Inventory.items);

  private void FakePopulate()
  {
    this.Populate(new List<InventoryItem>()
    {
      new InventoryItem(InventoryItem.ITEM_TYPE.BERRY),
      new InventoryItem(InventoryItem.ITEM_TYPE.FISH),
      new InventoryItem(InventoryItem.ITEM_TYPE.BLACK_GOLD),
      new InventoryItem(InventoryItem.ITEM_TYPE.BLACK_GOLD),
      new InventoryItem(InventoryItem.ITEM_TYPE.BLACK_GOLD),
      new InventoryItem(InventoryItem.ITEM_TYPE.BLACK_GOLD),
      new InventoryItem(InventoryItem.ITEM_TYPE.BLACK_GOLD),
      new InventoryItem(InventoryItem.ITEM_TYPE.BLACK_GOLD),
      new InventoryItem(InventoryItem.ITEM_TYPE.BLACK_GOLD),
      new InventoryItem(InventoryItem.ITEM_TYPE.BLACK_GOLD),
      new InventoryItem(InventoryItem.ITEM_TYPE.BLACK_GOLD),
      new InventoryItem(InventoryItem.ITEM_TYPE.BLACK_GOLD),
      new InventoryItem(InventoryItem.ITEM_TYPE.BLACK_GOLD),
      new InventoryItem(InventoryItem.ITEM_TYPE.BLACK_GOLD),
      new InventoryItem(InventoryItem.ITEM_TYPE.BLACK_GOLD),
      new InventoryItem(InventoryItem.ITEM_TYPE.BLACK_GOLD),
      new InventoryItem(InventoryItem.ITEM_TYPE.BLACK_GOLD)
    });
  }

  public void Populate(List<InventoryItem> Items)
  {
    int childCount = this.Container.childCount;
    while (--childCount >= 0)
      Object.Destroy((Object) this.Container.GetChild(childCount).gameObject);
    foreach (InventoryItem inventoryItem in Items)
    {
      GameObject gameObject = Object.Instantiate<GameObject>(this.IconPrefab, this.Container);
      gameObject.SetActive(true);
      gameObject.GetComponent<HUD_InventoryIcon>().InitFromType((InventoryItem.ITEM_TYPE) inventoryItem.type);
    }
  }
}
