// Decompiled with JetBrains decompiler
// Type: Lamb.UI.InventoryMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using src.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class InventoryMenu : UISubmenuBase
{
  [Header("Inventory Menu")]
  [SerializeField]
  private MMScrollRect _scrollRect;
  [Header("Currencies")]
  [SerializeField]
  private RectTransform _currenciesContainer;
  [SerializeField]
  private GameObject _noCurrencyText;
  [Header("Food")]
  [SerializeField]
  private RectTransform _foodContainer;
  [SerializeField]
  private GameObject _noFoodText;
  [Header("Items")]
  [SerializeField]
  private RectTransform _itemsContainer;
  [SerializeField]
  private GameObject _noItemsText;
  [Header("Templates")]
  [SerializeField]
  private GenericInventoryItem _inventoryItemTemplate;
  private List<GenericInventoryItem> _currencies = new List<GenericInventoryItem>();
  private List<GenericInventoryItem> _food = new List<GenericInventoryItem>();
  private List<GenericInventoryItem> _items = new List<GenericInventoryItem>();
  private List<InventoryItem.ITEM_TYPE> _currencyFilter = new List<InventoryItem.ITEM_TYPE>()
  {
    InventoryItem.ITEM_TYPE.BLACK_GOLD,
    InventoryItem.ITEM_TYPE.BONE,
    InventoryItem.ITEM_TYPE.MONSTER_HEART,
    InventoryItem.ITEM_TYPE.LOG,
    InventoryItem.ITEM_TYPE.STONE,
    InventoryItem.ITEM_TYPE.KEY,
    InventoryItem.ITEM_TYPE.TALISMAN
  };

  protected override void OnShowStarted()
  {
    this._scrollRect.enabled = false;
    if (this._items.Count + this._food.Count + this._currencies.Count == 0)
    {
      this.Populate(Inventory.items, this._currencies, this._currenciesContainer, this._noCurrencyText, this._currencyFilter);
      this.Populate(Inventory.items, this._food, this._foodContainer, this._noFoodText, ((IEnumerable<InventoryItem.ITEM_TYPE>) CookingData.GetAllFoods()).ToList<InventoryItem.ITEM_TYPE>());
      List<InventoryItem.ITEM_TYPE> currencyFilter = this._currencyFilter;
      currencyFilter.AddRange((IEnumerable<InventoryItem.ITEM_TYPE>) CookingData.GetAllFoods());
      currencyFilter.Add(InventoryItem.ITEM_TYPE.INGREDIENTS);
      currencyFilter.Add(InventoryItem.ITEM_TYPE.SEEDS);
      this.Populate(Inventory.items, this._items, this._itemsContainer, this._noItemsText, ignore: currencyFilter);
      if (this._currencies.Count > 0)
        this.OverrideDefault((Selectable) this._currencies[0].Button);
      else if (this._food.Count > 0)
        this.OverrideDefault((Selectable) this._food[0].Button);
      else if (this._items.Count > 0)
        this.OverrideDefault((Selectable) this._items[0].Button);
      this.ActivateNavigation();
    }
    this._scrollRect.enabled = true;
    this._scrollRect.normalizedPosition = Vector2.one;
  }

  private void Populate(
    List<InventoryItem> items,
    List<GenericInventoryItem> destination,
    RectTransform container,
    GameObject noText,
    List<InventoryItem.ITEM_TYPE> filter = null,
    List<InventoryItem.ITEM_TYPE> ignore = null)
  {
    foreach (InventoryItem inventoryItem in items)
    {
      if ((filter == null || filter.Contains((InventoryItem.ITEM_TYPE) inventoryItem.type)) && (ignore == null || !ignore.Contains((InventoryItem.ITEM_TYPE) inventoryItem.type)))
      {
        GenericInventoryItem genericInventoryItem = this._inventoryItemTemplate.Instantiate<GenericInventoryItem>((Transform) container);
        genericInventoryItem.Button.Confirmable = false;
        genericInventoryItem.Configure((InventoryItem.ITEM_TYPE) inventoryItem.type);
        destination.Add(genericInventoryItem);
      }
    }
    noText.SetActive(destination.Count == 0);
  }
}
