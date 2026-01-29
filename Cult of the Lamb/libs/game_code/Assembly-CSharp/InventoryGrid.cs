// Decompiled with JetBrains decompiler
// Type: InventoryGrid
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class InventoryGrid : BaseMonoBehaviour
{
  public GameObject IconPrefab;
  public Transform Container;

  public void OnEnable()
  {
    Inventory.OnInventoryUpdated += new Inventory.InventoryUpdated(this.PopulateFromInventory);
  }

  public void OnDisable()
  {
    Inventory.OnInventoryUpdated -= new Inventory.InventoryUpdated(this.PopulateFromInventory);
  }

  public void Start() => this.PopulateFromInventory();

  public void PopulateFromInventory() => this.Populate(Inventory.items);

  public void FakePopulate()
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
