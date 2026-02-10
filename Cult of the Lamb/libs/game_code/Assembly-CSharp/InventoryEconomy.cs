// Decompiled with JetBrains decompiler
// Type: InventoryEconomy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class InventoryEconomy : BaseMonoBehaviour
{
  public List<InventoryItem.ITEM_TYPE> InventoryDisplay = new List<InventoryItem.ITEM_TYPE>()
  {
    InventoryItem.ITEM_TYPE.SOUL,
    InventoryItem.ITEM_TYPE.SEEDS,
    InventoryItem.ITEM_TYPE.INGREDIENTS,
    InventoryItem.ITEM_TYPE.MEALS,
    InventoryItem.ITEM_TYPE.LOG,
    InventoryItem.ITEM_TYPE.STONE,
    InventoryItem.ITEM_TYPE.GRASS,
    InventoryItem.ITEM_TYPE.POOP,
    InventoryItem.ITEM_TYPE.BLACK_GOLD
  };
  public List<InventoryEconomyItem> EconomyItems = new List<InventoryEconomyItem>();
  public GameObject IconPrefab;
  public Transform Container;
  public UI_Transitions UITransitions;
  public bool populatedList;
  public bool FoundOne;

  public void OnEnable()
  {
    Inventory.OnInventoryUpdated += new Inventory.InventoryUpdated(this.PopulateFromInventory);
  }

  public void OnDisable()
  {
    Inventory.OnInventoryUpdated -= new Inventory.InventoryUpdated(this.PopulateFromInventory);
  }

  public void Start()
  {
    this.ManualPopulate(Inventory.items);
    foreach (int type in this.InventoryDisplay)
    {
      if (Inventory.GetItemQuantity(type) > 0)
      {
        this.FoundOne = true;
        break;
      }
    }
    if (this.FoundOne)
      return;
    this.UITransitions.hideBar();
    this.StartCoroutine((IEnumerator) this.CheckResources());
  }

  public void PopulateFromInventory() => this.ManualUpdateResource();

  public void ManualPopulate(List<InventoryItem> Items)
  {
    foreach (InventoryItem.ITEM_TYPE type in this.InventoryDisplay)
    {
      GameObject gameObject = Object.Instantiate<GameObject>(this.IconPrefab, this.Container);
      gameObject.SetActive(true);
      InventoryEconomyItem component = gameObject.GetComponent<InventoryEconomyItem>();
      component.Init(type);
      this.EconomyItems.Add(component);
    }
    this.IconPrefab.SetActive(false);
    this.populatedList = true;
  }

  public void ManualUpdateResource()
  {
    foreach (InventoryEconomyItem economyItem in this.EconomyItems)
      economyItem.UpdateResources();
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

  public IEnumerator CheckResources()
  {
    while (this.UITransitions.Hidden)
    {
      yield return (object) new WaitForSeconds(1f);
      foreach (int type in this.InventoryDisplay)
      {
        if (Inventory.GetItemQuantity(type) > 0)
        {
          this.UITransitions.StartCoroutine((IEnumerator) this.UITransitions.MoveBarIn());
          break;
        }
      }
    }
  }
}
