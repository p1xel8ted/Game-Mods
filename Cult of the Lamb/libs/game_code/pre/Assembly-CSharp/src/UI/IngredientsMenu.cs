// Decompiled with JetBrains decompiler
// Type: src.UI.IngredientsMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI;
using src.Extensions;
using src.UINavigator;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace src.UI;

public class IngredientsMenu : UISubmenuBase
{
  public const int kMaxIngredients = 3;
  public Action<InventoryItem.ITEM_TYPE> OnIngredientChosen;
  [SerializeField]
  private RectTransform _contentContainer;
  [SerializeField]
  private IngredientItem _inventoryItemTemplate;
  [SerializeField]
  private GameObject _noIngredientsText;
  [SerializeField]
  private MMScrollRect _scrollRect;
  private StructuresData _kitchenData;
  private List<IngredientItem> _items = new List<IngredientItem>();

  public void Configure(StructuresData kitchenData) => this._kitchenData = kitchenData;

  protected override void OnShowStarted()
  {
    this._scrollRect.enabled = false;
    if (this._items.Count == 0)
    {
      List<InventoryItem> inventoryItemList = new List<InventoryItem>();
      foreach (InventoryItem.ITEM_TYPE allFood in CookingData.GetAllFoods())
      {
        if (Inventory.GetItemQuantity(allFood) > 0)
        {
          inventoryItemList.Add(Inventory.GetItemByType(allFood));
        }
        else
        {
          foreach (InventoryItem inventoryItem in this._kitchenData.Inventory)
          {
            if ((InventoryItem.ITEM_TYPE) inventoryItem.type == allFood)
            {
              inventoryItemList.Add(new InventoryItem(allFood, 0));
              break;
            }
          }
        }
      }
      foreach (InventoryItem inventoryItem in inventoryItemList)
      {
        IngredientItem newItem = this._inventoryItemTemplate.Instantiate<IngredientItem>((Transform) this._contentContainer);
        newItem.Configure((InventoryItem.ITEM_TYPE) inventoryItem.type, false, true);
        newItem.Button.onClick.AddListener((UnityAction) (() => this.OnIngredientClicked(newItem)));
        this._items.Add(newItem);
      }
    }
    this._scrollRect.normalizedPosition = (Vector2) Vector3.one;
    this._scrollRect.enabled = true;
    this._noIngredientsText.SetActive(this._items.Count == 0);
  }

  private void OnIngredientClicked(IngredientItem item)
  {
    if (this._kitchenData.Inventory.Count < 3 && Inventory.GetItemQuantity(item.Type) > 0)
    {
      Action<InventoryItem.ITEM_TYPE> ingredientChosen = this.OnIngredientChosen;
      if (ingredientChosen == null)
        return;
      ingredientChosen(item.Type);
    }
    else
      item.Shake();
  }

  public IMMSelectable ProvideFirstSelectable()
  {
    return this._items.Count > 0 ? (IMMSelectable) this._items[0].Button : (IMMSelectable) null;
  }

  public IMMSelectable ProvideSelectable()
  {
    return (IMMSelectable) this._items.LastElement<IngredientItem>().Button;
  }

  public void UpdateQuantities()
  {
    foreach (IngredientItem ingredientItem in this._items)
    {
      if (ingredientItem.Quantity == 0 && Inventory.GetItemQuantity(ingredientItem.Type) > 0)
        ingredientItem.Configure(Inventory.GetItemByType(ingredientItem.Type));
      else
        ingredientItem.UpdateQuantity();
    }
  }
}
