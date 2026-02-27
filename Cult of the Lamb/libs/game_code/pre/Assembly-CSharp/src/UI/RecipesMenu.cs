// Decompiled with JetBrains decompiler
// Type: src.UI.RecipesMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private RectTransform _contentContainer;
  [SerializeField]
  private RecipeItem _recipeItemTemplate;
  [SerializeField]
  private GameObject _noRecipesText;
  private StructuresData _kitchenData;
  private List<RecipeItem> _items = new List<RecipeItem>();

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

  private void OnRecipeClicked(RecipeItem item)
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

  private int RecipeLimit()
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
