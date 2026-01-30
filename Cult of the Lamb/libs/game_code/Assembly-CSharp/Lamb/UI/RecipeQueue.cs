// Decompiled with JetBrains decompiler
// Type: Lamb.UI.RecipeQueue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using src.Extensions;
using src.UINavigator;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace Lamb.UI;

public class RecipeQueue : BaseMonoBehaviour
{
  public Func<IMMSelectable> RequestSelectable;
  public Action<InventoryItem.ITEM_TYPE, int> OnRecipeRemoved;
  [SerializeField]
  public RectTransform _contentContainer;
  [SerializeField]
  public RecipeItem _inventoryItemTemplate;
  [SerializeField]
  public TextMeshProUGUI _count;
  [SerializeField]
  public GameObject[] _vacantSlots;
  public StructuresData _kitchenData;
  public List<RecipeItem> _items = new List<RecipeItem>();

  public void Configure(StructuresData kitchenData)
  {
    this._kitchenData = kitchenData;
    for (int index = 0; index < this._vacantSlots.Length; ++index)
      this._vacantSlots[index].SetActive(index < this.RecipeLimit());
    foreach (Interaction_Kitchen.QueuedMeal queuedMeal in kitchenData.QueuedMeals)
      this.AddRecipe(queuedMeal.MealType);
    this.UpdateCount();
  }

  public void AddRecipe(InventoryItem.ITEM_TYPE recipe)
  {
    this.MakeRecipe(recipe);
    this.UpdateCount();
  }

  public RecipeItem MakeRecipe(InventoryItem.ITEM_TYPE recipe)
  {
    RecipeItem newItem = this._inventoryItemTemplate.Instantiate<RecipeItem>((Transform) this._contentContainer);
    newItem.Button.onClick.AddListener((UnityAction) (() => this.RemoveRecipe(newItem)));
    Vector3 localScale = newItem.RectTransform.localScale;
    newItem.RectTransform.localScale = Vector3.one * 1.2f;
    newItem.RectTransform.DOScale(localScale, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    newItem.Configure(recipe, false, true);
    this._vacantSlots[this._items.Count].SetActive(false);
    this._items.Add(newItem);
    newItem.transform.SetSiblingIndex(this._items.Count - 1);
    return newItem;
  }

  public void RemoveRecipe(RecipeItem item)
  {
    Action<InventoryItem.ITEM_TYPE, int> onRecipeRemoved = this.OnRecipeRemoved;
    if (onRecipeRemoved != null)
      onRecipeRemoved(item.Type, this._items.IndexOf(item));
    IMMSelectable selectableOnRight = item.Button.FindSelectableOnRight() as IMMSelectable;
    IMMSelectable selectableOnLeft = item.Button.FindSelectableOnLeft() as IMMSelectable;
    if (this._items.IndexOf(item) < this._items.Count - 1 && selectableOnRight != null)
      MonoSingleton<UINavigatorNew>.Instance.NavigateToNew(selectableOnRight);
    else if (this._items.IndexOf(item) > 0 && selectableOnLeft != null)
      MonoSingleton<UINavigatorNew>.Instance.NavigateToNew(selectableOnLeft);
    else if (this._items.Count - 1 > 0)
      MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this._items[this._items.Count - 2].Button);
    else
      MonoSingleton<UINavigatorNew>.Instance.NavigateToNew(this.RequestSelectable());
    this._items.Remove(item);
    UnityEngine.Object.Destroy((UnityEngine.Object) item.gameObject);
    this._vacantSlots[this._items.Count].SetActive(true);
    this.UpdateCount();
  }

  public void UpdateCount()
  {
    Color colour = this._kitchenData.QueuedMeals.Count > 0 ? StaticColors.GreenColor : StaticColors.RedColor;
    this._count.text = $"{this._kitchenData.QueuedMeals.Count}/{this.RecipeLimit()}".Colour(colour);
    this._count.isRightToLeftText = false;
  }

  public int RecipeLimit()
  {
    return 12 + (this._kitchenData.Type == StructureBrain.TYPES.KITCHEN_II ? 5 : 0);
  }
}
