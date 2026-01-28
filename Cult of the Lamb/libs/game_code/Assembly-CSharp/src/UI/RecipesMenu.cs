// Decompiled with JetBrains decompiler
// Type: src.UI.RecipesMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using src.Extensions;
using src.UINavigator;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace src.UI;

public class RecipesMenu : BaseMonoBehaviour
{
  public const int kMaxRecipes = 12;
  public Action<InventoryItem.ITEM_TYPE> OnRecipeChosen;
  [SerializeField]
  public RectTransform _contentContainer;
  [SerializeField]
  public RecipeItem _recipeItemTemplate;
  [SerializeField]
  public GameObject _noRecipesText;
  public StructuresData _kitchenData;
  public List<RecipeItem> _items = new List<RecipeItem>();

  public void Configure(StructuresData kitchenData)
  {
    this._kitchenData = kitchenData;
    if (this._items.Count == 0)
    {
      List<InventoryItem.ITEM_TYPE> itemTypeList = new List<InventoryItem.ITEM_TYPE>();
      foreach (InventoryItem.ITEM_TYPE allMeal in CookingData.GetAllMeals())
      {
        if ((allMeal != InventoryItem.ITEM_TYPE.MEAL_POOP || DataManager.Instance.RecipesDiscovered.Contains(allMeal)) && (allMeal != InventoryItem.ITEM_TYPE.MEAL_GRASS || DataManager.Instance.RecipesDiscovered.Contains(allMeal)))
        {
          if (CookingData.CanMakeMeal(allMeal))
            CookingData.TryDiscoverRecipe(allMeal);
          if (!DataManager.Instance.MAJOR_DLC && CookingData.IsRecipeDLC(allMeal))
            Debug.Log((object) $"Drink  {allMeal.ToString()} is DLC only, skipping");
          else
            itemTypeList.Add(allMeal);
        }
      }
      itemTypeList.Sort((Comparison<InventoryItem.ITEM_TYPE>) ((a, b) => CookingData.HasRecipeDiscovered(b).CompareTo(CookingData.HasRecipeDiscovered(a))));
      foreach (InventoryItem.ITEM_TYPE type in itemTypeList)
      {
        RecipeItem recipeItem = this._recipeItemTemplate.Instantiate<RecipeItem>((Transform) this._contentContainer);
        recipeItem.Configure(type, true, false);
        recipeItem.OnRecipeChosen += new Action<RecipeItem>(this.OnRecipeClicked);
        this._items.Add(recipeItem);
      }
    }
    else
      this.UpdateQuantities();
    this._noRecipesText.SetActive(this._items.Count == 0);
  }

  public void OnRecipeClicked(RecipeItem item)
  {
    if (this._kitchenData.QueuedMeals.Count >= this.RecipeLimit())
      return;
    Action<InventoryItem.ITEM_TYPE> onRecipeChosen = this.OnRecipeChosen;
    if (onRecipeChosen == null)
      return;
    onRecipeChosen(item.Type);
  }

  public void UpdateQuantities()
  {
    foreach (UIInventoryItem uiInventoryItem in this._items)
      uiInventoryItem.UpdateQuantity();
  }

  public IMMSelectable ProvideFirstSelectable()
  {
    return this._items.Count > 0 ? (IMMSelectable) this._items[0].Button : (IMMSelectable) null;
  }

  public IMMSelectable ProvideSelectable()
  {
    return (IMMSelectable) this._items.LastElement<RecipeItem>().Button;
  }

  public int RecipeLimit()
  {
    return 12 + (this._kitchenData.Type == StructureBrain.TYPES.KITCHEN_II ? 5 : 0);
  }

  public int ReadilyAvailableMeals()
  {
    int num = 0;
    foreach (RecipeItem recipeItem in this._items)
      num += recipeItem.Quantity;
    return num;
  }
}
