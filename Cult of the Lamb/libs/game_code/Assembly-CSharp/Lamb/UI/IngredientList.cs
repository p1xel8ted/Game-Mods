// Decompiled with JetBrains decompiler
// Type: Lamb.UI.IngredientList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using src.Extensions;
using src.UINavigator;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace Lamb.UI;

public class IngredientList : BaseMonoBehaviour
{
  public Action<InventoryItem.ITEM_TYPE, int> OnIngredientRemoved;
  public Func<IMMSelectable> RequestSelectable;
  [SerializeField]
  public RectTransform _contentContainer;
  [SerializeField]
  public IngredientItem _inventoryItemTemplate;
  [SerializeField]
  public TextMeshProUGUI _count;
  [SerializeField]
  public GameObject[] _vacantSlots;
  public StructuresData _kitchenData;
  public List<IngredientItem> _items = new List<IngredientItem>();

  public void Configure(StructuresData kitchenData)
  {
    this._kitchenData = kitchenData;
    foreach (InventoryItem inventoryItem in kitchenData.Inventory)
      this.AddIngredient((InventoryItem.ITEM_TYPE) inventoryItem.type);
    this.UpdateCount();
  }

  public void AddIngredient(InventoryItem.ITEM_TYPE ingredient)
  {
    this.MakeIngredient(ingredient);
    this.UpdateCount();
  }

  public void Clear()
  {
    foreach (Component component in this._items)
      UnityEngine.Object.Destroy((UnityEngine.Object) component.gameObject);
    this._items.Clear();
    this.UpdateCount();
  }

  public void RemoveIngredient(IngredientItem item)
  {
    IMMSelectable selectableOnRight = item.Button.FindSelectableOnRight() as IMMSelectable;
    IMMSelectable selectableOnLeft = item.Button.FindSelectableOnLeft() as IMMSelectable;
    if (this._items.IndexOf(item) < this._items.Count - 1 && selectableOnRight != null)
      MonoSingleton<UINavigatorNew>.Instance.NavigateToNew(selectableOnRight);
    else if (this._items.IndexOf(item) > 0 && selectableOnLeft != null)
      MonoSingleton<UINavigatorNew>.Instance.NavigateToNew(selectableOnLeft);
    else
      MonoSingleton<UINavigatorNew>.Instance.NavigateToNew(this.RequestSelectable());
    Action<InventoryItem.ITEM_TYPE, int> ingredientRemoved = this.OnIngredientRemoved;
    if (ingredientRemoved != null)
      ingredientRemoved(item.Type, this._items.IndexOf(item));
    this._items.Remove(item);
    UnityEngine.Object.Destroy((UnityEngine.Object) item.gameObject);
    this._vacantSlots[this._items.Count].SetActive(true);
    this.UpdateCount();
  }

  public IngredientItem MakeIngredient(InventoryItem.ITEM_TYPE ingredient)
  {
    IngredientItem newItem = this._inventoryItemTemplate.Instantiate<IngredientItem>((Transform) this._contentContainer);
    newItem.Configure(ingredient, true, false);
    newItem.Button.onClick.AddListener((UnityAction) (() => this.RemoveIngredient(newItem)));
    this._vacantSlots[this._items.Count].SetActive(false);
    this._items.Add(newItem);
    newItem.transform.SetSiblingIndex(this._items.Count - 1);
    return newItem;
  }

  public void UpdateCount()
  {
    this._count.text = $"{this._kitchenData.Inventory.Count}/{3}".Colour(this._kitchenData.Inventory.Count >= 3 ? StaticColors.GreenColor : StaticColors.RedColor);
  }

  public IMMSelectable ProvideFirstSelectable()
  {
    return this._items.Count > 0 ? (IMMSelectable) this._items[0].Button : (IMMSelectable) null;
  }
}
