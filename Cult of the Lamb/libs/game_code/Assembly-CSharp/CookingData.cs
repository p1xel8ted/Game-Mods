// Decompiled with JetBrains decompiler
// Type: CookingData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;

#nullable disable
public sealed class CookingData
{
  public const int MaxIngredientSlots = 3;
  public static Action<InventoryItem.ITEM_TYPE> OnRecipeDiscovered;
  public const float RecipeBalanceValue = 2.25f;
  public static bool REQUIRES_LOC;
  public const int SATIATION_PER_DAY = 60;

  public static bool CanMakeMeal(InventoryItem.ITEM_TYPE mealType)
  {
    List<List<InventoryItem>> recipe = CookingData.GetRecipe(mealType);
    for (int index1 = 0; index1 < recipe.Count; ++index1)
    {
      bool flag = true;
      for (int index2 = recipe[index1].Count - 1; index2 >= 0; --index2)
      {
        if (Inventory.GetItemQuantity(recipe[index1][index2].type) < recipe[index1][index2].quantity)
        {
          flag = false;
          break;
        }
      }
      if (flag)
        return true;
    }
    return false;
  }

  public static bool CanMakeMealUsingRecipe(List<InventoryItem> recipe)
  {
    bool flag = true;
    for (int index = recipe.Count - 1; index >= 0; --index)
    {
      if (Inventory.GetItemQuantity(recipe[index].type) < recipe[index].quantity)
      {
        flag = false;
        break;
      }
    }
    return flag;
  }

  public static bool CanMakeMeals(List<InventoryItem> ingredients)
  {
    for (int index = 0; index < CookingData.GetAllMeals().Length; ++index)
    {
      if (CookingData.GetAllMeals()[index] != InventoryItem.ITEM_TYPE.MEAL_GRASS && CookingData.GetCookableRecipeAmount(CookingData.GetAllMeals()[index], ingredients) > 0)
        return true;
    }
    return false;
  }

  public static bool CanMakeDrinks(List<InventoryItem> ingredients)
  {
    for (int index = 0; index < CookingData.GetAllDrinks().Length; ++index)
    {
      if (CookingData.GetCookableRecipeAmount(CookingData.GetAllDrinks()[index], ingredients) > 0)
        return true;
    }
    return false;
  }

  public static List<InventoryItem> GetDrinksFromIngredients(
    List<InventoryItem> ingredients,
    out List<InventoryItem> resultingIngredients)
  {
    resultingIngredients = new List<InventoryItem>();
    List<InventoryItem> ingredients1 = new List<InventoryItem>();
    for (int index = 0; index < ingredients.Count; ++index)
      ingredients1.Add(new InventoryItem((InventoryItem.ITEM_TYPE) ingredients[index].type, ingredients[index].quantity));
    List<InventoryItem> drinksFromIngredients = new List<InventoryItem>();
    List<InventoryItem.ITEM_TYPE> itemTypeList = new List<InventoryItem.ITEM_TYPE>();
    itemTypeList.AddRange((IEnumerable<InventoryItem.ITEM_TYPE>) CookingData.GetAllDrinks());
    for (int index1 = 0; index1 < itemTypeList.Count; ++index1)
    {
      InventoryItem.ITEM_TYPE itemType = itemTypeList[index1];
      int cookableRecipeAmount = CookingData.GetCookableRecipeAmount(itemType, ingredients1);
      for (int index2 = 0; index2 < cookableRecipeAmount; ++index2)
      {
        List<InventoryItem> recipeSimplified = CookingData.GetRecipeSimplified(itemType);
        for (int index3 = 0; index3 < recipeSimplified.Count; ++index3)
          resultingIngredients.Add(new InventoryItem((InventoryItem.ITEM_TYPE) recipeSimplified[index3].type, recipeSimplified[index3].quantity));
        CookingData.RemoveIngredientsFromRecipe(ingredients1, recipeSimplified);
        drinksFromIngredients.Add(new InventoryItem(itemType, 1));
      }
    }
    return drinksFromIngredients;
  }

  public static List<InventoryItem> GetGoodOrBetterMealsFromIngredients(
    List<InventoryItem> ingredients,
    out List<InventoryItem> resultingIngredients)
  {
    resultingIngredients = new List<InventoryItem>();
    List<InventoryItem> ingredients1 = new List<InventoryItem>();
    for (int index = 0; index < ingredients.Count; ++index)
      ingredients1.Add(new InventoryItem((InventoryItem.ITEM_TYPE) ingredients[index].type, ingredients[index].quantity));
    List<InventoryItem> mealsFromIngredients = new List<InventoryItem>();
    List<InventoryItem.ITEM_TYPE> itemTypeList = new List<InventoryItem.ITEM_TYPE>();
    itemTypeList.AddRange((IEnumerable<InventoryItem.ITEM_TYPE>) CookingData.GetAllMeals());
    for (int index1 = 0; index1 < itemTypeList.Count; ++index1)
    {
      InventoryItem.ITEM_TYPE itemType = itemTypeList[index1];
      if (itemType != InventoryItem.ITEM_TYPE.MEAL_GRASS)
      {
        int cookableRecipeAmount = CookingData.GetCookableRecipeAmount(itemType, ingredients1);
        for (int index2 = 0; index2 < cookableRecipeAmount; ++index2)
        {
          List<InventoryItem> recipeSimplified = CookingData.GetRecipeSimplified(itemType);
          for (int index3 = 0; index3 < recipeSimplified.Count; ++index3)
            resultingIngredients.Add(new InventoryItem((InventoryItem.ITEM_TYPE) recipeSimplified[index3].type, recipeSimplified[index3].quantity));
          CookingData.RemoveIngredientsFromRecipe(ingredients1, recipeSimplified);
          mealsFromIngredients.Add(new InventoryItem(itemType, 1));
        }
      }
    }
    return mealsFromIngredients;
  }

  public static void RemoveIngredientsFromRecipe(
    List<InventoryItem> ingredients,
    List<InventoryItem> recipe)
  {
    for (int index1 = recipe.Count - 1; index1 >= 0; --index1)
    {
      for (int index2 = ingredients.Count - 1; index2 >= 0; --index2)
      {
        if (recipe[index1].type == ingredients[index2].type)
        {
          ingredients[index2].quantity -= recipe[index1].quantity;
          recipe[index1].quantity = (int) MathF.Abs((float) ingredients[index2].quantity);
          if (ingredients[index2].quantity <= 0)
            ingredients.RemoveAt(index2);
          if (recipe[index1].quantity <= 0)
          {
            recipe.RemoveAt(index1);
            break;
          }
        }
      }
    }
  }

  public static int GetCookableRecipeAmount(
    InventoryItem.ITEM_TYPE mealType,
    List<InventoryItem> ingredients)
  {
    List<List<InventoryItem>> recipe = CookingData.GetRecipe(mealType);
    int cookableRecipeAmount = 0;
    using (List<List<InventoryItem>>.Enumerator enumerator = recipe.GetEnumerator())
    {
      if (enumerator.MoveNext())
      {
        List<InventoryItem> current = enumerator.Current;
        Dictionary<int, int> dictionary = new Dictionary<int, int>();
        foreach (InventoryItem ingredient in ingredients)
        {
          if (dictionary.ContainsKey(ingredient.type))
            dictionary[ingredient.type] += ingredient.quantity;
          else
            dictionary.Add(ingredient.type, ingredient.quantity);
        }
        while (recipe.Count != 0)
        {
          foreach (InventoryItem inventoryItem in current)
          {
            bool flag = false;
            int num;
            if (dictionary.TryGetValue(inventoryItem.type, out num))
            {
              flag = true;
              if (num < inventoryItem.quantity)
                return cookableRecipeAmount;
              dictionary[inventoryItem.type] -= inventoryItem.quantity;
            }
            if (!flag)
              return cookableRecipeAmount;
          }
          ++cookableRecipeAmount;
        }
        return 0;
      }
    }
    return cookableRecipeAmount;
  }

  public static int BalanceValue(int value) => Mathf.CeilToInt((float) value * 2.25f);

  public static bool IsRecipeDLC(InventoryItem.ITEM_TYPE mealType)
  {
    switch (mealType)
    {
      case InventoryItem.ITEM_TYPE.MEAL_SPICY:
      case InventoryItem.ITEM_TYPE.MEAL_SNOW_FRUIT:
      case InventoryItem.ITEM_TYPE.DRINK_CHILLI:
      case InventoryItem.ITEM_TYPE.MEAL_MILK_BAD:
      case InventoryItem.ITEM_TYPE.MEAL_MILK_GOOD:
      case InventoryItem.ITEM_TYPE.MEAL_MILK_GREAT:
      case InventoryItem.ITEM_TYPE.DRINK_MILKSHAKE:
        return true;
      default:
        return false;
    }
  }

  public static List<List<InventoryItem>> GetRecipe(InventoryItem.ITEM_TYPE mealType)
  {
    switch (mealType)
    {
      case InventoryItem.ITEM_TYPE.MEAL:
        return new List<List<InventoryItem>>()
        {
          new List<InventoryItem>()
          {
            new InventoryItem(InventoryItem.ITEM_TYPE.PUMPKIN, 4)
          }
        };
      case InventoryItem.ITEM_TYPE.MEAL_GRASS:
        return new List<List<InventoryItem>>()
        {
          new List<InventoryItem>()
          {
            new InventoryItem(InventoryItem.ITEM_TYPE.GRASS, 5)
          }
        };
      case InventoryItem.ITEM_TYPE.MEAL_MEAT:
        return new List<List<InventoryItem>>()
        {
          new List<InventoryItem>()
          {
            new InventoryItem(InventoryItem.ITEM_TYPE.MEAT, 2)
          }
        };
      case InventoryItem.ITEM_TYPE.MEAL_GREAT:
        return new List<List<InventoryItem>>()
        {
          new List<InventoryItem>()
          {
            new InventoryItem(InventoryItem.ITEM_TYPE.BEETROOT, 6),
            new InventoryItem(InventoryItem.ITEM_TYPE.PUMPKIN, 2),
            new InventoryItem(InventoryItem.ITEM_TYPE.CAULIFLOWER, 2)
          }
        };
      case InventoryItem.ITEM_TYPE.MEAL_GOOD_FISH:
        return new List<List<InventoryItem>>()
        {
          new List<InventoryItem>()
          {
            new InventoryItem(InventoryItem.ITEM_TYPE.FISH, 3)
          }
        };
      case InventoryItem.ITEM_TYPE.MEAL_FOLLOWER_MEAT:
        return new List<List<InventoryItem>>()
        {
          new List<InventoryItem>()
          {
            new InventoryItem(InventoryItem.ITEM_TYPE.FOLLOWER_MEAT, 3),
            new InventoryItem(InventoryItem.ITEM_TYPE.BONE, 5)
          }
        };
      case InventoryItem.ITEM_TYPE.MEAL_POOP:
        return new List<List<InventoryItem>>()
        {
          new List<InventoryItem>()
          {
            new InventoryItem(InventoryItem.ITEM_TYPE.POOP, 3)
          }
        };
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_FISH:
        return new List<List<InventoryItem>>()
        {
          new List<InventoryItem>()
          {
            new InventoryItem(InventoryItem.ITEM_TYPE.FISH_SQUID, 1),
            new InventoryItem(InventoryItem.ITEM_TYPE.FISH_OCTOPUS, 1),
            new InventoryItem(InventoryItem.ITEM_TYPE.FISH_BLOWFISH, 1),
            new InventoryItem(InventoryItem.ITEM_TYPE.FISH_SWORDFISH, 1)
          }
        };
      case InventoryItem.ITEM_TYPE.MEAL_BAD_FISH:
        return new List<List<InventoryItem>>()
        {
          new List<InventoryItem>()
          {
            new InventoryItem(InventoryItem.ITEM_TYPE.FISH_SMALL, 3)
          }
        };
      case InventoryItem.ITEM_TYPE.MEAL_BERRIES:
        return new List<List<InventoryItem>>()
        {
          new List<InventoryItem>()
          {
            new InventoryItem(InventoryItem.ITEM_TYPE.BERRY, 6)
          }
        };
      case InventoryItem.ITEM_TYPE.MEAL_MEDIUM_VEG:
        return new List<List<InventoryItem>>()
        {
          new List<InventoryItem>()
          {
            new InventoryItem(InventoryItem.ITEM_TYPE.CAULIFLOWER, 4)
          }
        };
      case InventoryItem.ITEM_TYPE.MEAL_BAD_MIXED:
        return new List<List<InventoryItem>>()
        {
          new List<InventoryItem>()
          {
            new InventoryItem(InventoryItem.ITEM_TYPE.BERRY, 4),
            new InventoryItem(InventoryItem.ITEM_TYPE.FISH_SMALL, 2),
            new InventoryItem(InventoryItem.ITEM_TYPE.MEAT_MORSEL, 2)
          }
        };
      case InventoryItem.ITEM_TYPE.MEAL_MEDIUM_MIXED:
        return new List<List<InventoryItem>>()
        {
          new List<InventoryItem>()
          {
            new InventoryItem(InventoryItem.ITEM_TYPE.PUMPKIN, 4),
            new InventoryItem(InventoryItem.ITEM_TYPE.FISH, 2),
            new InventoryItem(InventoryItem.ITEM_TYPE.MEAT, 2)
          }
        };
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_MIXED:
        return new List<List<InventoryItem>>()
        {
          new List<InventoryItem>()
          {
            new InventoryItem(InventoryItem.ITEM_TYPE.BEETROOT, 4),
            new InventoryItem(InventoryItem.ITEM_TYPE.FISH_BIG, 2),
            new InventoryItem(InventoryItem.ITEM_TYPE.MEAT, 2)
          }
        };
      case InventoryItem.ITEM_TYPE.MEAL_DEADLY:
        return new List<List<InventoryItem>>()
        {
          new List<InventoryItem>()
          {
            new InventoryItem(InventoryItem.ITEM_TYPE.FOLLOWER_MEAT, 1),
            new InventoryItem(InventoryItem.ITEM_TYPE.POOP, 1),
            new InventoryItem(InventoryItem.ITEM_TYPE.GRASS, 1)
          }
        };
      case InventoryItem.ITEM_TYPE.MEAL_BAD_MEAT:
        return new List<List<InventoryItem>>()
        {
          new List<InventoryItem>()
          {
            new InventoryItem(InventoryItem.ITEM_TYPE.MEAT_MORSEL, 3)
          }
        };
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_MEAT:
        return new List<List<InventoryItem>>()
        {
          new List<InventoryItem>()
          {
            new InventoryItem(InventoryItem.ITEM_TYPE.MEAT, 4),
            new InventoryItem(InventoryItem.ITEM_TYPE.FISH_CRAB, 1),
            new InventoryItem(InventoryItem.ITEM_TYPE.FISH_LOBSTER, 1)
          }
        };
      case InventoryItem.ITEM_TYPE.MEAL_SPICY:
        return new List<List<InventoryItem>>()
        {
          new List<InventoryItem>()
          {
            new InventoryItem(InventoryItem.ITEM_TYPE.CHILLI, 5)
          }
        };
      case InventoryItem.ITEM_TYPE.DRINK_BEER:
        return new List<List<InventoryItem>>()
        {
          new List<InventoryItem>()
          {
            new InventoryItem(InventoryItem.ITEM_TYPE.HOPS, 3)
          }
        };
      case InventoryItem.ITEM_TYPE.DRINK_WINE:
        return new List<List<InventoryItem>>()
        {
          new List<InventoryItem>()
          {
            new InventoryItem(InventoryItem.ITEM_TYPE.GRAPES, 5)
          }
        };
      case InventoryItem.ITEM_TYPE.DRINK_COCKTAIL:
        return new List<List<InventoryItem>>()
        {
          new List<InventoryItem>()
          {
            new InventoryItem(InventoryItem.ITEM_TYPE.HOPS, 2),
            new InventoryItem(InventoryItem.ITEM_TYPE.GRAPES, 4),
            new InventoryItem(InventoryItem.ITEM_TYPE.FLOWER_RED, 4)
          }
        };
      case InventoryItem.ITEM_TYPE.DRINK_EGGNOG:
        return new List<List<InventoryItem>>()
        {
          new List<InventoryItem>()
          {
            new InventoryItem(InventoryItem.ITEM_TYPE.YOLK, 1),
            new InventoryItem(InventoryItem.ITEM_TYPE.HOPS, 5)
          }
        };
      case InventoryItem.ITEM_TYPE.DRINK_POOP_JUICE:
        return new List<List<InventoryItem>>()
        {
          new List<InventoryItem>()
          {
            new InventoryItem(InventoryItem.ITEM_TYPE.POOP, 10),
            new InventoryItem(InventoryItem.ITEM_TYPE.HOPS, 3)
          }
        };
      case InventoryItem.ITEM_TYPE.DRINK_GIN:
        return new List<List<InventoryItem>>()
        {
          new List<InventoryItem>()
          {
            new InventoryItem(InventoryItem.ITEM_TYPE.BERRY, 6),
            new InventoryItem(InventoryItem.ITEM_TYPE.GRAPES, 6),
            new InventoryItem(InventoryItem.ITEM_TYPE.GRASS, 3)
          }
        };
      case InventoryItem.ITEM_TYPE.DRINK_MUSHROOM_JUICE:
        return new List<List<InventoryItem>>()
        {
          new List<InventoryItem>()
          {
            new InventoryItem(InventoryItem.ITEM_TYPE.MUSHROOM_SMALL, 5)
          }
        };
      case InventoryItem.ITEM_TYPE.MEAL_EGG:
        return new List<List<InventoryItem>>()
        {
          new List<InventoryItem>()
          {
            new InventoryItem(InventoryItem.ITEM_TYPE.YOLK, 1)
          }
        };
      case InventoryItem.ITEM_TYPE.MEAL_SNOW_FRUIT:
        return new List<List<InventoryItem>>()
        {
          new List<InventoryItem>()
          {
            new InventoryItem(InventoryItem.ITEM_TYPE.SNOW_FRUIT, 1)
          }
        };
      case InventoryItem.ITEM_TYPE.DRINK_CHILLI:
        return new List<List<InventoryItem>>()
        {
          new List<InventoryItem>()
          {
            new InventoryItem(InventoryItem.ITEM_TYPE.CHILLI, 1),
            new InventoryItem(InventoryItem.ITEM_TYPE.HOPS, 4)
          }
        };
      case InventoryItem.ITEM_TYPE.DRINK_LIGHTNING:
        return new List<List<InventoryItem>>()
        {
          new List<InventoryItem>()
          {
            new InventoryItem(InventoryItem.ITEM_TYPE.LIGHTNING_SHARD, 3),
            new InventoryItem(InventoryItem.ITEM_TYPE.GRAPES, 4)
          }
        };
      case InventoryItem.ITEM_TYPE.DRINK_SIN:
        return new List<List<InventoryItem>>()
        {
          new List<InventoryItem>()
          {
            new InventoryItem(InventoryItem.ITEM_TYPE.PLEASURE_POINT, 3)
          }
        };
      case InventoryItem.ITEM_TYPE.DRINK_GRASS:
        return new List<List<InventoryItem>>()
        {
          new List<InventoryItem>()
          {
            new InventoryItem(InventoryItem.ITEM_TYPE.GRASS, 25),
            new InventoryItem(InventoryItem.ITEM_TYPE.HOPS, 5)
          }
        };
      case InventoryItem.ITEM_TYPE.MEAL_MILK_BAD:
        return new List<List<InventoryItem>>()
        {
          new List<InventoryItem>()
          {
            new InventoryItem(InventoryItem.ITEM_TYPE.MILK, 1)
          }
        };
      case InventoryItem.ITEM_TYPE.MEAL_MILK_GOOD:
        return new List<List<InventoryItem>>()
        {
          new List<InventoryItem>()
          {
            new InventoryItem(InventoryItem.ITEM_TYPE.MILK, 1),
            new InventoryItem(InventoryItem.ITEM_TYPE.PUMPKIN, 3)
          }
        };
      case InventoryItem.ITEM_TYPE.MEAL_MILK_GREAT:
        return new List<List<InventoryItem>>()
        {
          new List<InventoryItem>()
          {
            new InventoryItem(InventoryItem.ITEM_TYPE.MILK, 1),
            new InventoryItem(InventoryItem.ITEM_TYPE.CAULIFLOWER, 3),
            new InventoryItem(InventoryItem.ITEM_TYPE.BEETROOT, 3)
          }
        };
      case InventoryItem.ITEM_TYPE.DRINK_MILKSHAKE:
        return new List<List<InventoryItem>>()
        {
          new List<InventoryItem>()
          {
            new InventoryItem(InventoryItem.ITEM_TYPE.MILK, 2),
            new InventoryItem(InventoryItem.ITEM_TYPE.SNOW_FRUIT, 1),
            new InventoryItem(InventoryItem.ITEM_TYPE.POOP, 1)
          }
        };
      default:
        return new List<List<InventoryItem>>();
    }
  }

  public static List<InventoryItem> GetRecipeSimplified(InventoryItem.ITEM_TYPE meal)
  {
    List<List<InventoryItem>> recipe = CookingData.GetRecipe(meal);
    List<InventoryItem> recipeSimplified = new List<InventoryItem>();
    foreach (InventoryItem inventoryItem1 in recipe[0])
    {
      bool flag = false;
      foreach (InventoryItem inventoryItem2 in recipeSimplified)
      {
        if (inventoryItem2.type == inventoryItem1.type)
        {
          ++inventoryItem2.quantity;
          flag = true;
          break;
        }
      }
      if (!flag)
        recipeSimplified.Add(new InventoryItem((InventoryItem.ITEM_TYPE) inventoryItem1.type, inventoryItem1.quantity));
    }
    return recipeSimplified;
  }

  public static bool HasRecipeDiscovered(InventoryItem.ITEM_TYPE meal)
  {
    return DataManager.Instance.RecipesDiscovered.Contains(meal);
  }

  public static bool TryDiscoverRecipe(InventoryItem.ITEM_TYPE meal)
  {
    if (DataManager.Instance.RecipesDiscovered.Contains(meal))
      return false;
    DataManager.Instance.RecipesDiscovered.Add(meal);
    Action<InventoryItem.ITEM_TYPE> recipeDiscovered = CookingData.OnRecipeDiscovered;
    if (recipeDiscovered != null)
      recipeDiscovered(meal);
    return true;
  }

  public static string GetLocalizedName(InventoryItem.ITEM_TYPE mealType)
  {
    return LocalizationManager.GetTranslation($"CookingData/{mealType}/Name");
  }

  public static string GetLocalizedDescription(InventoryItem.ITEM_TYPE mealType)
  {
    return LocalizationManager.GetTranslation($"CookingData/{mealType}/Description");
  }

  public static InventoryItem.ITEM_TYPE GetIngredientFromInventory(
    CookingData.IngredientType ingredientType)
  {
    foreach (InventoryItem inventoryItem in Inventory.items)
    {
      if (CookingData.GetIngredientType((InventoryItem.ITEM_TYPE) inventoryItem.type) == ingredientType)
        return (InventoryItem.ITEM_TYPE) inventoryItem.type;
    }
    return InventoryItem.ITEM_TYPE.NONE;
  }

  public static InventoryItem.ITEM_TYPE RecipeTypeFromFollowerCommand(
    FollowerCommands followerCommand)
  {
    switch (followerCommand)
    {
      case FollowerCommands.Meal:
        return InventoryItem.ITEM_TYPE.MEAL;
      case FollowerCommands.MealGrass:
        return InventoryItem.ITEM_TYPE.MEAL_GRASS;
      case FollowerCommands.MealPoop:
        return InventoryItem.ITEM_TYPE.MEAL_POOP;
      case FollowerCommands.MealGoodFish:
        return InventoryItem.ITEM_TYPE.MEAL_GOOD_FISH;
      case FollowerCommands.MealFollowerMeat:
        return InventoryItem.ITEM_TYPE.MEAL_FOLLOWER_MEAT;
      case FollowerCommands.MealGreat:
        return InventoryItem.ITEM_TYPE.MEAL_GREAT;
      case FollowerCommands.MealMushrooms:
        return InventoryItem.ITEM_TYPE.MEAL_MUSHROOMS;
      case FollowerCommands.MealMeat:
        return InventoryItem.ITEM_TYPE.MEAL_MEAT;
      case FollowerCommands.MealGreatFish:
        return InventoryItem.ITEM_TYPE.MEAL_GREAT_FISH;
      case FollowerCommands.MealBadFish:
        return InventoryItem.ITEM_TYPE.MEAL_BAD_FISH;
      case FollowerCommands.MealBerries:
        return InventoryItem.ITEM_TYPE.MEAL_BERRIES;
      case FollowerCommands.MealMediumVeg:
        return InventoryItem.ITEM_TYPE.MEAL_MEDIUM_VEG;
      case FollowerCommands.MealMixedLow:
        return InventoryItem.ITEM_TYPE.MEAL_BAD_MIXED;
      case FollowerCommands.MealMixedMedium:
        return InventoryItem.ITEM_TYPE.MEAL_MEDIUM_MIXED;
      case FollowerCommands.MealMixedHigh:
        return InventoryItem.ITEM_TYPE.MEAL_GREAT_MIXED;
      case FollowerCommands.MealDeadly:
        return InventoryItem.ITEM_TYPE.MEAL_DEADLY;
      case FollowerCommands.MealMeatLow:
        return InventoryItem.ITEM_TYPE.MEAL_BAD_MEAT;
      case FollowerCommands.MealMeatHigh:
        return InventoryItem.ITEM_TYPE.MEAL_GREAT_MEAT;
      case FollowerCommands.MealBurnt:
        return InventoryItem.ITEM_TYPE.MEAL_BURNED;
      case FollowerCommands.MealEgg:
        return InventoryItem.ITEM_TYPE.MEAL_EGG;
      case FollowerCommands.DrinkBeer:
        return InventoryItem.ITEM_TYPE.DRINK_BEER;
      case FollowerCommands.DrinkGin:
        return InventoryItem.ITEM_TYPE.DRINK_GIN;
      case FollowerCommands.DrinkCocktail:
        return InventoryItem.ITEM_TYPE.DRINK_COCKTAIL;
      case FollowerCommands.DrinkWine:
        return InventoryItem.ITEM_TYPE.DRINK_WINE;
      case FollowerCommands.DrinkMushroomJuice:
        return InventoryItem.ITEM_TYPE.DRINK_MUSHROOM_JUICE;
      case FollowerCommands.DrinkPoopJuice:
        return InventoryItem.ITEM_TYPE.DRINK_POOP_JUICE;
      case FollowerCommands.DrinkEggnog:
        return InventoryItem.ITEM_TYPE.DRINK_EGGNOG;
      case FollowerCommands.MealSpicy:
        return InventoryItem.ITEM_TYPE.MEAL_SPICY;
      case FollowerCommands.MealSnowFruit:
        return InventoryItem.ITEM_TYPE.MEAL_SNOW_FRUIT;
      case FollowerCommands.DrinkChilli:
        return InventoryItem.ITEM_TYPE.DRINK_CHILLI;
      case FollowerCommands.DrinkLightning:
        return InventoryItem.ITEM_TYPE.DRINK_LIGHTNING;
      case FollowerCommands.DrinkSin:
        return InventoryItem.ITEM_TYPE.DRINK_SIN;
      case FollowerCommands.DrinkGrass:
        return InventoryItem.ITEM_TYPE.DRINK_GRASS;
      case FollowerCommands.MealMilkBad:
        return InventoryItem.ITEM_TYPE.MEAL_MILK_BAD;
      case FollowerCommands.MealMilkGood:
        return InventoryItem.ITEM_TYPE.MEAL_MILK_GOOD;
      case FollowerCommands.MealMilkGreat:
        return InventoryItem.ITEM_TYPE.MEAL_MILK_GREAT;
      case FollowerCommands.DrinkMilkshake:
        return InventoryItem.ITEM_TYPE.DRINK_MILKSHAKE;
      default:
        return InventoryItem.ITEM_TYPE.NONE;
    }
  }

  public static CookingData.IngredientType GetIngredientType(InventoryItem.ITEM_TYPE ingredient)
  {
    switch (ingredient)
    {
      case InventoryItem.ITEM_TYPE.MEAT:
        return CookingData.IngredientType.MEAT_HIGH;
      case InventoryItem.ITEM_TYPE.BERRY:
        return CookingData.IngredientType.FRUIT_LOW;
      case InventoryItem.ITEM_TYPE.FISH:
      case InventoryItem.ITEM_TYPE.FISH_BIG:
      case InventoryItem.ITEM_TYPE.FISH_CRAB:
      case InventoryItem.ITEM_TYPE.FISH_BLOWFISH:
      case InventoryItem.ITEM_TYPE.FISH_PIKE:
        return CookingData.IngredientType.FISH_MEDIUM;
      case InventoryItem.ITEM_TYPE.FISH_SMALL:
      case InventoryItem.ITEM_TYPE.FISH_CATFISH:
        return CookingData.IngredientType.FISH_LOW;
      case InventoryItem.ITEM_TYPE.GRASS:
        return CookingData.IngredientType.SPECIAL_GRASS;
      case InventoryItem.ITEM_TYPE.POOP:
        return CookingData.IngredientType.SPECIAL_POOP;
      case InventoryItem.ITEM_TYPE.PUMPKIN:
        return CookingData.IngredientType.VEGETABLE_MEDIUM;
      case InventoryItem.ITEM_TYPE.FOLLOWER_MEAT:
      case InventoryItem.ITEM_TYPE.FOLLOWER_MEAT_ROTTEN:
        return CookingData.IngredientType.SPECIAL_FOLLOWER_MEAT;
      case InventoryItem.ITEM_TYPE.FISH_LOBSTER:
      case InventoryItem.ITEM_TYPE.FISH_OCTOPUS:
      case InventoryItem.ITEM_TYPE.FISH_SQUID:
      case InventoryItem.ITEM_TYPE.FISH_SWORDFISH:
      case InventoryItem.ITEM_TYPE.FISH_COD:
        return CookingData.IngredientType.FISH_HIGH;
      case InventoryItem.ITEM_TYPE.BEETROOT:
        return CookingData.IngredientType.VEGETABLE_HIGH;
      case InventoryItem.ITEM_TYPE.CAULIFLOWER:
        return CookingData.IngredientType.VEGETABLE_MEDIUM;
      case InventoryItem.ITEM_TYPE.MEAT_MORSEL:
        return CookingData.IngredientType.MEAT_LOW;
      default:
        return CookingData.IngredientType.NONE;
    }
  }

  public static CookingData.IngredientType GetIngredientCategory(
    CookingData.IngredientType ingredientType)
  {
    switch (ingredientType)
    {
      case CookingData.IngredientType.FRUIT_LOW:
        return CookingData.IngredientType.FRUIT;
      case CookingData.IngredientType.VEGETABLE_LOW:
      case CookingData.IngredientType.VEGETABLE_MEDIUM:
      case CookingData.IngredientType.VEGETABLE_HIGH:
        return CookingData.IngredientType.VEGETABLE;
      case CookingData.IngredientType.MEAT_LOW:
      case CookingData.IngredientType.MEAT_MEDIUM:
      case CookingData.IngredientType.MEAT_HIGH:
        return CookingData.IngredientType.MEAT;
      case CookingData.IngredientType.FISH_LOW:
      case CookingData.IngredientType.FISH_MEDIUM:
      case CookingData.IngredientType.FISH_HIGH:
        return CookingData.IngredientType.FISH;
      case CookingData.IngredientType.SPECIAL_FOLLOWER_MEAT:
      case CookingData.IngredientType.SPECIAL_GRASS:
      case CookingData.IngredientType.SPECIAL_POOP:
        return CookingData.IngredientType.SPECIAL;
      default:
        return CookingData.IngredientType.NONE;
    }
  }

  public static int GetIngredientTypeWeight(CookingData.IngredientType ingredientType)
  {
    switch (ingredientType)
    {
      case CookingData.IngredientType.FRUIT_LOW:
      case CookingData.IngredientType.VEGETABLE_LOW:
      case CookingData.IngredientType.MEAT_LOW:
      case CookingData.IngredientType.FISH_LOW:
        return 4;
      case CookingData.IngredientType.VEGETABLE_MEDIUM:
      case CookingData.IngredientType.MEAT_MEDIUM:
      case CookingData.IngredientType.FISH_MEDIUM:
        return 6;
      case CookingData.IngredientType.VEGETABLE_HIGH:
      case CookingData.IngredientType.MEAT_HIGH:
      case CookingData.IngredientType.FISH_HIGH:
        return 10;
      default:
        return 0;
    }
  }

  public static CookingData.IngredientType DetermineIngredientTypeLevel(
    CookingData.IngredientType ingredientType,
    int value)
  {
    switch (ingredientType)
    {
      case CookingData.IngredientType.MEAT:
        if ((double) value >= 7.5)
          return CookingData.IngredientType.MEAT_HIGH;
        return (double) value > 4.0 ? CookingData.IngredientType.MEAT_MEDIUM : CookingData.IngredientType.MEAT_LOW;
      case CookingData.IngredientType.VEGETABLE:
        if ((double) value >= 7.5)
          return CookingData.IngredientType.VEGETABLE_HIGH;
        return (double) value > 4.0 ? CookingData.IngredientType.VEGETABLE_MEDIUM : CookingData.IngredientType.VEGETABLE_LOW;
      case CookingData.IngredientType.FISH:
        if ((double) value >= 7.5)
          return CookingData.IngredientType.FISH_HIGH;
        return (double) value > 4.0 ? CookingData.IngredientType.FISH_MEDIUM : CookingData.IngredientType.FISH_LOW;
      case CookingData.IngredientType.FRUIT:
        return CookingData.IngredientType.FRUIT_LOW;
      case CookingData.IngredientType.MIXED:
        if ((double) value >= 7.5)
          return CookingData.IngredientType.MIXED_HIGH;
        return (double) value > 4.0 ? CookingData.IngredientType.MIXED_MEDIUM : CookingData.IngredientType.MIXED_LOW;
      default:
        return CookingData.IngredientType.NONE;
    }
  }

  public static int GetIllnessAmount(InventoryItem.ITEM_TYPE meal, FollowerBrain _brain)
  {
    switch (meal)
    {
      case InventoryItem.ITEM_TYPE.GRASS:
        if (!_brain.HasTrait(FollowerTrait.TraitType.GrassEater))
          return 5;
        break;
      case InventoryItem.ITEM_TYPE.POOP:
        return 20;
      case InventoryItem.ITEM_TYPE.MEAL_ROTTEN:
      case InventoryItem.ITEM_TYPE.MEAL_BAD_FISH:
        return 10;
      case InventoryItem.ITEM_TYPE.MEAL_FOLLOWER_MEAT:
        if (!_brain.HasTrait(FollowerTrait.TraitType.Cannibal))
          return 10;
        break;
    }
    return 0;
  }

  public static int GetSatationAmount(InventoryItem.ITEM_TYPE meal)
  {
    switch (meal)
    {
      case InventoryItem.ITEM_TYPE.NONE:
      case InventoryItem.ITEM_TYPE.DRINK_BEER:
      case InventoryItem.ITEM_TYPE.DRINK_WINE:
      case InventoryItem.ITEM_TYPE.DRINK_COCKTAIL:
      case InventoryItem.ITEM_TYPE.DRINK_EGGNOG:
      case InventoryItem.ITEM_TYPE.DRINK_POOP_JUICE:
      case InventoryItem.ITEM_TYPE.DRINK_GIN:
      case InventoryItem.ITEM_TYPE.DRINK_MUSHROOM_JUICE:
      case InventoryItem.ITEM_TYPE.DRINK_CHILLI:
      case InventoryItem.ITEM_TYPE.DRINK_LIGHTNING:
      case InventoryItem.ITEM_TYPE.DRINK_GRASS:
        return 0;
      case InventoryItem.ITEM_TYPE.MUSHROOM_SMALL:
        return 10;
      case InventoryItem.ITEM_TYPE.MEAL_MEAT:
      case InventoryItem.ITEM_TYPE.MEAL_GOOD_FISH:
      case InventoryItem.ITEM_TYPE.MEAL_MEDIUM_VEG:
      case InventoryItem.ITEM_TYPE.MEAL_MEDIUM_MIXED:
      case InventoryItem.ITEM_TYPE.MEAL_SPICY:
      case InventoryItem.ITEM_TYPE.MEAL_MILK_GOOD:
        return 75;
      case InventoryItem.ITEM_TYPE.MEAL_GREAT:
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_FISH:
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_MIXED:
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_MEAT:
      case InventoryItem.ITEM_TYPE.MEAL_EGG:
      case InventoryItem.ITEM_TYPE.MEAL_SNOW_FRUIT:
      case InventoryItem.ITEM_TYPE.DRINK_SIN:
      case InventoryItem.ITEM_TYPE.MEAL_MILK_GREAT:
        return 100;
      default:
        return 60;
    }
  }

  public static float GetTummyRating(InventoryItem.ITEM_TYPE meal)
  {
    switch (meal)
    {
      case InventoryItem.ITEM_TYPE.MEAL_MEAT:
      case InventoryItem.ITEM_TYPE.MEAL_GOOD_FISH:
      case InventoryItem.ITEM_TYPE.MEAL_MEDIUM_VEG:
      case InventoryItem.ITEM_TYPE.MEAL_MEDIUM_MIXED:
      case InventoryItem.ITEM_TYPE.MEAL_SPICY:
      case InventoryItem.ITEM_TYPE.MEAL_MILK_GOOD:
        return 0.5f;
      case InventoryItem.ITEM_TYPE.MEAL_GREAT:
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_FISH:
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_MIXED:
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_MEAT:
      case InventoryItem.ITEM_TYPE.MEAL_EGG:
      case InventoryItem.ITEM_TYPE.MEAL_SNOW_FRUIT:
      case InventoryItem.ITEM_TYPE.MEAL_MILK_GREAT:
        return 1f;
      default:
        return 0.25f;
    }
  }

  public static int GetSatationAmountPlayer(InventoryItem.ITEM_TYPE meal)
  {
    int satationLevel = CookingData.GetSatationLevel(meal);
    if (meal == InventoryItem.ITEM_TYPE.MEAL_FOLLOWER_MEAT)
      return 100;
    int num = 25;
    if (TimeManager.CurrentDay >= 10)
      num = 20;
    else if (TimeManager.CurrentDay >= 20)
      num = 15;
    return num * satationLevel;
  }

  public static int GetSatationLevel(InventoryItem.ITEM_TYPE meal)
  {
    switch (meal)
    {
      case InventoryItem.ITEM_TYPE.NONE:
      case InventoryItem.ITEM_TYPE.MEAL_GRASS:
      case InventoryItem.ITEM_TYPE.MEAL_POOP:
      case InventoryItem.ITEM_TYPE.DRINK_MUSHROOM_JUICE:
        return 0;
      case InventoryItem.ITEM_TYPE.MEAL_MEAT:
      case InventoryItem.ITEM_TYPE.MEAL_GOOD_FISH:
      case InventoryItem.ITEM_TYPE.MEAL_MEDIUM_VEG:
      case InventoryItem.ITEM_TYPE.MEAL_MEDIUM_MIXED:
      case InventoryItem.ITEM_TYPE.MEAL_SPICY:
      case InventoryItem.ITEM_TYPE.DRINK_WINE:
      case InventoryItem.ITEM_TYPE.DRINK_GIN:
      case InventoryItem.ITEM_TYPE.DRINK_CHILLI:
      case InventoryItem.ITEM_TYPE.DRINK_LIGHTNING:
      case InventoryItem.ITEM_TYPE.DRINK_GRASS:
      case InventoryItem.ITEM_TYPE.MEAL_MILK_GOOD:
      case InventoryItem.ITEM_TYPE.DRINK_MILKSHAKE:
        return 2;
      case InventoryItem.ITEM_TYPE.MEAL_GREAT:
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_FISH:
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_MIXED:
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_MEAT:
      case InventoryItem.ITEM_TYPE.DRINK_COCKTAIL:
      case InventoryItem.ITEM_TYPE.DRINK_EGGNOG:
      case InventoryItem.ITEM_TYPE.MEAL_EGG:
      case InventoryItem.ITEM_TYPE.MEAL_SNOW_FRUIT:
      case InventoryItem.ITEM_TYPE.DRINK_SIN:
        return 3;
      case InventoryItem.ITEM_TYPE.DRINK_BEER:
      case InventoryItem.ITEM_TYPE.DRINK_POOP_JUICE:
        return 1;
      case InventoryItem.ITEM_TYPE.MEAL_MILK_GREAT:
        return 4;
      default:
        return 1;
    }
  }

  public static int GetFaithAmount(InventoryItem.ITEM_TYPE meal)
  {
    switch (meal)
    {
      case InventoryItem.ITEM_TYPE.MEAL_MEAT:
      case InventoryItem.ITEM_TYPE.MEAL_GOOD_FISH:
      case InventoryItem.ITEM_TYPE.MEAL_MEDIUM_VEG:
      case InventoryItem.ITEM_TYPE.MEAL_MEDIUM_MIXED:
        return 0;
      case InventoryItem.ITEM_TYPE.MEAL_GREAT:
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_FISH:
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_MIXED:
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_MEAT:
      case InventoryItem.ITEM_TYPE.MEAL_EGG:
        return 3;
      default:
        return 0;
    }
  }

  public static ThoughtData ThoughtDataForMeal(InventoryItem.ITEM_TYPE meal)
  {
    switch (meal)
    {
      case InventoryItem.ITEM_TYPE.MEAL_GRASS:
        return DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.GrassEater) ? FollowerThoughts.GetData(Thought.Cult_AteGrassMealTrait) : FollowerThoughts.GetData(Thought.Cult_AteGrassMeal);
      case InventoryItem.ITEM_TYPE.MEAL_GREAT:
        return FollowerThoughts.GetData(Thought.Cult_AteGreatMeal);
      case InventoryItem.ITEM_TYPE.MEAL_FOLLOWER_MEAT:
        return !DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.Cannibal) ? FollowerThoughts.GetData(Thought.Cult_AteFollowerMeat) : FollowerThoughts.GetData(Thought.Cult_AteFollowerMeatTrait);
      case InventoryItem.ITEM_TYPE.MEAL_POOP:
        return FollowerThoughts.GetData(Thought.AtePoopMeal);
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_FISH:
        return FollowerThoughts.GetData(Thought.Cult_AteGreatFishMeal);
      default:
        return new ThoughtData();
    }
  }

  public static InventoryItem.ITEM_TYPE[] GetAllFoods()
  {
    List<InventoryItem.ITEM_TYPE> itemTypeList = new List<InventoryItem.ITEM_TYPE>();
    itemTypeList.Add(InventoryItem.ITEM_TYPE.GRASS);
    itemTypeList.Add(InventoryItem.ITEM_TYPE.POOP);
    foreach (InventoryItem.ITEM_TYPE lowQualityFood in CookingData.GetLowQualityFoods())
      itemTypeList.Add(lowQualityFood);
    foreach (InventoryItem.ITEM_TYPE mediumQualityFood in CookingData.GetMediumQualityFoods())
      itemTypeList.Add(mediumQualityFood);
    foreach (InventoryItem.ITEM_TYPE highQualityFood in CookingData.GetHighQualityFoods())
      itemTypeList.Add(highQualityFood);
    return itemTypeList.ToArray();
  }

  public static InventoryItem.ITEM_TYPE[] GetLowQualityFoods()
  {
    return new InventoryItem.ITEM_TYPE[3]
    {
      InventoryItem.ITEM_TYPE.MEAT_MORSEL,
      InventoryItem.ITEM_TYPE.FISH_SMALL,
      InventoryItem.ITEM_TYPE.BERRY
    };
  }

  public static InventoryItem.ITEM_TYPE[] GetMediumQualityFoods()
  {
    return new InventoryItem.ITEM_TYPE[9]
    {
      InventoryItem.ITEM_TYPE.MEAT,
      InventoryItem.ITEM_TYPE.FOLLOWER_MEAT,
      InventoryItem.ITEM_TYPE.FOLLOWER_MEAT_ROTTEN,
      InventoryItem.ITEM_TYPE.FISH,
      InventoryItem.ITEM_TYPE.FISH_BIG,
      InventoryItem.ITEM_TYPE.FISH_CRAB,
      InventoryItem.ITEM_TYPE.FISH_BLOWFISH,
      InventoryItem.ITEM_TYPE.CAULIFLOWER,
      InventoryItem.ITEM_TYPE.PUMPKIN
    };
  }

  public static InventoryItem.ITEM_TYPE[] GetHighQualityFoods()
  {
    return new InventoryItem.ITEM_TYPE[5]
    {
      InventoryItem.ITEM_TYPE.FISH_SWORDFISH,
      InventoryItem.ITEM_TYPE.FISH_SQUID,
      InventoryItem.ITEM_TYPE.FISH_LOBSTER,
      InventoryItem.ITEM_TYPE.FISH_OCTOPUS,
      InventoryItem.ITEM_TYPE.BEETROOT
    };
  }

  public static CookingData.IngredientType GetSpecialType(List<InventoryItem> ingredients)
  {
    List<InventoryItem.ITEM_TYPE> itemTypeList = new List<InventoryItem.ITEM_TYPE>();
    foreach (InventoryItem ingredient in ingredients)
      itemTypeList.Add((InventoryItem.ITEM_TYPE) ingredient.type);
    if (itemTypeList.Contains(InventoryItem.ITEM_TYPE.POOP) && itemTypeList.Contains(InventoryItem.ITEM_TYPE.FOLLOWER_MEAT))
      return CookingData.IngredientType.SPECIAL_DEADLY;
    if (itemTypeList.Contains(InventoryItem.ITEM_TYPE.POOP))
      return CookingData.IngredientType.SPECIAL_POOP;
    if (itemTypeList.Contains(InventoryItem.ITEM_TYPE.FOLLOWER_MEAT))
      return CookingData.IngredientType.SPECIAL_FOLLOWER_MEAT;
    if (itemTypeList.Contains(InventoryItem.ITEM_TYPE.FOLLOWER_MEAT_ROTTEN))
      return CookingData.IngredientType.SPECIAL_DEADLY;
    if (itemTypeList.Contains(InventoryItem.ITEM_TYPE.GRASS))
    {
      int num = 0;
      foreach (InventoryItem ingredient in ingredients)
      {
        if (ingredient.type == 35)
          num += ingredient.quantity;
      }
      if (num >= 2)
        return CookingData.IngredientType.SPECIAL_GRASS;
    }
    return CookingData.IngredientType.NONE;
  }

  public static List<InventoryItem.ITEM_TYPE> GetDiscoveredMealTypes()
  {
    return DataManager.Instance.RecipesDiscovered;
  }

  public static Sprite GetIcon(InventoryItem.ITEM_TYPE mealType)
  {
    SpriteAtlas spriteAtlas = Resources.Load<SpriteAtlas>("Atlases/MealIcons");
    string lower = mealType.ToString().ToLower();
    char c1 = lower[0];
    string name = lower.Remove(0, 1).Insert(0, char.ToUpper(c1).ToString());
    for (int index = 0; index < name.Length; ++index)
    {
      if (name[index] == '_')
      {
        char c2 = name[index + 1];
        name = name.Remove(index + 1, 1).Insert(index + 1, char.ToUpper(c2).ToString());
      }
    }
    return spriteAtlas.GetSprite(name);
  }

  public static float GetMealCookDuration(StructureBrain.TYPES mealType)
  {
    return CookingData.GetMealCookDuration(CookingData.GetMealFromStructureType(mealType));
  }

  public static float GetMealCookDuration(InventoryItem.ITEM_TYPE mealType)
  {
    switch (mealType)
    {
      case InventoryItem.ITEM_TYPE.MEAL_GRASS:
      case InventoryItem.ITEM_TYPE.MEAL_ROTTEN:
      case InventoryItem.ITEM_TYPE.MEAL_FOLLOWER_MEAT:
      case InventoryItem.ITEM_TYPE.MEAL_POOP:
      case InventoryItem.ITEM_TYPE.MEAL_BAD_FISH:
      case InventoryItem.ITEM_TYPE.MEAL_BERRIES:
      case InventoryItem.ITEM_TYPE.MEAL_BAD_MIXED:
      case InventoryItem.ITEM_TYPE.MEAL_DEADLY:
      case InventoryItem.ITEM_TYPE.MEAL_BAD_MEAT:
        return 30f;
      case InventoryItem.ITEM_TYPE.MEAL_MEAT:
      case InventoryItem.ITEM_TYPE.MEAL_GOOD_FISH:
      case InventoryItem.ITEM_TYPE.MEAL_MEDIUM_VEG:
      case InventoryItem.ITEM_TYPE.MEAL_MEDIUM_MIXED:
      case InventoryItem.ITEM_TYPE.MEAL_SPICY:
      case InventoryItem.ITEM_TYPE.MEAL_MILK_GOOD:
        return 36f;
      case InventoryItem.ITEM_TYPE.MEAL_GREAT:
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_FISH:
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_MIXED:
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_MEAT:
      case InventoryItem.ITEM_TYPE.MEAL_EGG:
      case InventoryItem.ITEM_TYPE.MEAL_SNOW_FRUIT:
      case InventoryItem.ITEM_TYPE.MEAL_MILK_GREAT:
        return 42f;
      default:
        return 42f;
    }
  }

  public static void CookedMeal(InventoryItem.ITEM_TYPE mealType)
  {
    if (mealType == InventoryItem.ITEM_TYPE.MEAL_FOLLOWER_MEAT)
      AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("FEED_FOLLOWER_MEAT"));
    CookingData.TryDiscoverRecipe(mealType);
    if (mealType <= InventoryItem.ITEM_TYPE.MEAL_POOP)
    {
      if (mealType <= InventoryItem.ITEM_TYPE.MEAL)
      {
        if (mealType != InventoryItem.ITEM_TYPE.NONE)
        {
          if (mealType != InventoryItem.ITEM_TYPE.MEAL)
            return;
          ++DataManager.Instance.VEGETABLE_LOW_MEALS_COOKED;
        }
        else
          ++DataManager.Instance.MealsCooked;
      }
      else
      {
        switch (mealType - 57)
        {
          case InventoryItem.ITEM_TYPE.NONE:
            ++DataManager.Instance.GRASS_MEALS_COOKED;
            break;
          case InventoryItem.ITEM_TYPE.LOG:
            ++DataManager.Instance.MEAT_MEDIUM_COOKED;
            break;
          case InventoryItem.ITEM_TYPE.STONE:
            ++DataManager.Instance.VEGETABLE_HIGH_MEALS_COOKED;
            break;
          case InventoryItem.ITEM_TYPE.ROCK2:
            ++DataManager.Instance.FISH_MEDIUM_MEALS_COOKED;
            break;
          case InventoryItem.ITEM_TYPE.ROCK3:
            break;
          case InventoryItem.ITEM_TYPE.SEED_SWORD:
            break;
          case InventoryItem.ITEM_TYPE.MEAT:
            break;
          case InventoryItem.ITEM_TYPE.WHEAT:
            break;
          case InventoryItem.ITEM_TYPE.SEED:
            ++DataManager.Instance.FOLLOWER_MEAT_MEALS_COOKED;
            break;
          default:
            if (mealType != InventoryItem.ITEM_TYPE.MEAL_POOP)
              break;
            ++DataManager.Instance.POOP_MEALS_COOKED;
            break;
        }
      }
    }
    else if (mealType <= InventoryItem.ITEM_TYPE.MEAL_BAD_FISH)
    {
      if (mealType != InventoryItem.ITEM_TYPE.MEAL_GREAT_FISH)
      {
        if (mealType != InventoryItem.ITEM_TYPE.MEAL_BAD_FISH)
          return;
        ++DataManager.Instance.FISH_LOW_MEALS_COOKED;
      }
      else
        ++DataManager.Instance.FISH_HIGH_MEALS_COOKED;
    }
    else
    {
      switch (mealType - 106)
      {
        case InventoryItem.ITEM_TYPE.NONE:
          ++DataManager.Instance.FRUIT_LOW_MEALS_COOKED;
          break;
        case InventoryItem.ITEM_TYPE.LOG:
          ++DataManager.Instance.VEGETABLE_MEDIUM_MEALS_COOKED;
          break;
        case InventoryItem.ITEM_TYPE.STONE:
          break;
        case InventoryItem.ITEM_TYPE.ROCK2:
          break;
        case InventoryItem.ITEM_TYPE.ROCK3:
          break;
        case InventoryItem.ITEM_TYPE.SEED_SWORD:
          ++DataManager.Instance.DEADLY_MEALS_COOKED;
          break;
        case InventoryItem.ITEM_TYPE.MEAT:
          ++DataManager.Instance.MEAT_LOW_COOKED;
          break;
        case InventoryItem.ITEM_TYPE.WHEAT:
          ++DataManager.Instance.MEAT_HIGH_COOKED;
          break;
        default:
          if (mealType != InventoryItem.ITEM_TYPE.MEAL_EGG)
            break;
          ++DataManager.Instance.EGG_MEALS_COOKED;
          break;
      }
    }
  }

  public static int GetCookedMeal(InventoryItem.ITEM_TYPE mealType)
  {
    switch (mealType)
    {
      case InventoryItem.ITEM_TYPE.NONE:
        return DataManager.Instance.MealsCooked;
      case InventoryItem.ITEM_TYPE.MEAL:
        return DataManager.Instance.VEGETABLE_LOW_MEALS_COOKED;
      case InventoryItem.ITEM_TYPE.MEAL_GRASS:
        return DataManager.Instance.GRASS_MEALS_COOKED;
      case InventoryItem.ITEM_TYPE.MEAL_MEAT:
        return DataManager.Instance.MEAT_MEDIUM_COOKED;
      case InventoryItem.ITEM_TYPE.MEAL_GREAT:
        return DataManager.Instance.VEGETABLE_HIGH_MEALS_COOKED;
      case InventoryItem.ITEM_TYPE.MEAL_GOOD_FISH:
        return DataManager.Instance.FISH_MEDIUM_MEALS_COOKED;
      case InventoryItem.ITEM_TYPE.MEAL_FOLLOWER_MEAT:
        return DataManager.Instance.FOLLOWER_MEAT_MEALS_COOKED;
      case InventoryItem.ITEM_TYPE.MEAL_POOP:
        return DataManager.Instance.POOP_MEALS_COOKED;
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_FISH:
        return DataManager.Instance.FISH_HIGH_MEALS_COOKED;
      case InventoryItem.ITEM_TYPE.MEAL_BAD_FISH:
        return DataManager.Instance.FISH_LOW_MEALS_COOKED;
      case InventoryItem.ITEM_TYPE.MEAL_BERRIES:
        return DataManager.Instance.FRUIT_LOW_MEALS_COOKED;
      case InventoryItem.ITEM_TYPE.MEAL_MEDIUM_VEG:
        return DataManager.Instance.VEGETABLE_MEDIUM_MEALS_COOKED;
      case InventoryItem.ITEM_TYPE.MEAL_DEADLY:
        return DataManager.Instance.DEADLY_MEALS_COOKED;
      case InventoryItem.ITEM_TYPE.MEAL_BAD_MEAT:
        return DataManager.Instance.MEAT_LOW_COOKED;
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_MEAT:
        return DataManager.Instance.MEAT_HIGH_COOKED;
      case InventoryItem.ITEM_TYPE.MEAL_EGG:
        return DataManager.Instance.EGG_MEALS_COOKED;
      default:
        return 0;
    }
  }

  public static StructureBrain.TYPES GetStructureFromMealType(InventoryItem.ITEM_TYPE mealType)
  {
    switch (mealType)
    {
      case InventoryItem.ITEM_TYPE.MEAL:
        return StructureBrain.TYPES.MEAL;
      case InventoryItem.ITEM_TYPE.MEAL_GRASS:
        return StructureBrain.TYPES.MEAL_GRASS;
      case InventoryItem.ITEM_TYPE.MEAL_MEAT:
        return StructureBrain.TYPES.MEAL_MEAT;
      case InventoryItem.ITEM_TYPE.MEAL_GREAT:
        return StructureBrain.TYPES.MEAL_GREAT;
      case InventoryItem.ITEM_TYPE.MEAL_GOOD_FISH:
        return StructureBrain.TYPES.MEAL_GOOD_FISH;
      case InventoryItem.ITEM_TYPE.MEAL_ROTTEN:
        return StructureBrain.TYPES.MEAL_ROTTEN;
      case InventoryItem.ITEM_TYPE.MEAL_FOLLOWER_MEAT:
        return StructureBrain.TYPES.MEAL_FOLLOWER_MEAT;
      case InventoryItem.ITEM_TYPE.MEAL_POOP:
        return StructureBrain.TYPES.MEAL_POOP;
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_FISH:
        return StructureBrain.TYPES.MEAL_GREAT_FISH;
      case InventoryItem.ITEM_TYPE.MEAL_BAD_FISH:
        return StructureBrain.TYPES.MEAL_BAD_FISH;
      case InventoryItem.ITEM_TYPE.MEAL_BERRIES:
        return StructureBrain.TYPES.MEAL_BERRIES;
      case InventoryItem.ITEM_TYPE.MEAL_MEDIUM_VEG:
        return StructureBrain.TYPES.MEAL_MEDIUM_VEG;
      case InventoryItem.ITEM_TYPE.MEAL_BAD_MIXED:
        return StructureBrain.TYPES.MEAL_BAD_MIXED;
      case InventoryItem.ITEM_TYPE.MEAL_MEDIUM_MIXED:
        return StructureBrain.TYPES.MEAL_MEDIUM_MIXED;
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_MIXED:
        return StructureBrain.TYPES.MEAL_GREAT_MIXED;
      case InventoryItem.ITEM_TYPE.MEAL_DEADLY:
        return StructureBrain.TYPES.MEAL_DEADLY;
      case InventoryItem.ITEM_TYPE.MEAL_BAD_MEAT:
        return StructureBrain.TYPES.MEAL_BAD_MEAT;
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_MEAT:
        return StructureBrain.TYPES.MEAL_GREAT_MEAT;
      case InventoryItem.ITEM_TYPE.MEAL_BURNED:
        return StructureBrain.TYPES.MEAL_BURNED;
      case InventoryItem.ITEM_TYPE.MEAL_SPICY:
        return StructureBrain.TYPES.MEAL_SPICY;
      case InventoryItem.ITEM_TYPE.DRINK_BEER:
        return StructureBrain.TYPES.DRINK_BEER;
      case InventoryItem.ITEM_TYPE.DRINK_WINE:
        return StructureBrain.TYPES.DRINK_WINE;
      case InventoryItem.ITEM_TYPE.DRINK_COCKTAIL:
        return StructureBrain.TYPES.DRINK_COCKTAIL;
      case InventoryItem.ITEM_TYPE.DRINK_EGGNOG:
        return StructureBrain.TYPES.DRINK_EGGNOG;
      case InventoryItem.ITEM_TYPE.DRINK_POOP_JUICE:
        return StructureBrain.TYPES.DRINK_POOPJUICE;
      case InventoryItem.ITEM_TYPE.DRINK_GIN:
        return StructureBrain.TYPES.DRINK_GIN;
      case InventoryItem.ITEM_TYPE.DRINK_MUSHROOM_JUICE:
        return StructureBrain.TYPES.DRINK_MUSHROOMJUICE;
      case InventoryItem.ITEM_TYPE.MEAL_EGG:
        return StructureBrain.TYPES.MEAL_EGG;
      case InventoryItem.ITEM_TYPE.MEAL_SNOW_FRUIT:
        return StructureBrain.TYPES.MEAL_SNOW_FRUIT;
      case InventoryItem.ITEM_TYPE.DRINK_CHILLI:
        return StructureBrain.TYPES.DRINK_CHILLI;
      case InventoryItem.ITEM_TYPE.DRINK_LIGHTNING:
        return StructureBrain.TYPES.DRINK_LIGHTNING;
      case InventoryItem.ITEM_TYPE.DRINK_SIN:
        return StructureBrain.TYPES.DRINK_SIN;
      case InventoryItem.ITEM_TYPE.DRINK_GRASS:
        return StructureBrain.TYPES.DRINK_GRASS;
      case InventoryItem.ITEM_TYPE.MEAL_MILK_BAD:
        return StructureBrain.TYPES.MEAL_MILK_BAD;
      case InventoryItem.ITEM_TYPE.MEAL_MILK_GOOD:
        return StructureBrain.TYPES.MEAL_MILK_GOOD;
      case InventoryItem.ITEM_TYPE.MEAL_MILK_GREAT:
        return StructureBrain.TYPES.MEAL_MILK_GREAT;
      case InventoryItem.ITEM_TYPE.DRINK_MILKSHAKE:
        return StructureBrain.TYPES.DRINK_MILKSHAKE;
      default:
        return StructureBrain.TYPES.NONE;
    }
  }

  public static InventoryItem.ITEM_TYPE GetMealFromStructureType(StructureBrain.TYPES structureType)
  {
    switch (structureType)
    {
      case StructureBrain.TYPES.MEAL:
        return InventoryItem.ITEM_TYPE.MEAL;
      case StructureBrain.TYPES.MEAL_MEAT:
        return InventoryItem.ITEM_TYPE.MEAL_MEAT;
      case StructureBrain.TYPES.MEAL_GREAT:
        return InventoryItem.ITEM_TYPE.MEAL_GREAT;
      case StructureBrain.TYPES.MEAL_GRASS:
        return InventoryItem.ITEM_TYPE.MEAL_GRASS;
      case StructureBrain.TYPES.MEAL_GOOD_FISH:
        return InventoryItem.ITEM_TYPE.MEAL_GOOD_FISH;
      case StructureBrain.TYPES.MEAL_FOLLOWER_MEAT:
        return InventoryItem.ITEM_TYPE.MEAL_FOLLOWER_MEAT;
      case StructureBrain.TYPES.MEAL_POOP:
        return InventoryItem.ITEM_TYPE.MEAL_POOP;
      case StructureBrain.TYPES.MEAL_ROTTEN:
        return InventoryItem.ITEM_TYPE.MEAL_ROTTEN;
      case StructureBrain.TYPES.MEAL_GREAT_FISH:
        return InventoryItem.ITEM_TYPE.MEAL_GREAT_FISH;
      case StructureBrain.TYPES.MEAL_BAD_FISH:
        return InventoryItem.ITEM_TYPE.MEAL_BAD_FISH;
      case StructureBrain.TYPES.MEAL_BERRIES:
        return InventoryItem.ITEM_TYPE.MEAL_BERRIES;
      case StructureBrain.TYPES.MEAL_MEDIUM_VEG:
        return InventoryItem.ITEM_TYPE.MEAL_MEDIUM_VEG;
      case StructureBrain.TYPES.MEAL_BAD_MIXED:
        return InventoryItem.ITEM_TYPE.MEAL_BAD_MIXED;
      case StructureBrain.TYPES.MEAL_MEDIUM_MIXED:
        return InventoryItem.ITEM_TYPE.MEAL_MEDIUM_MIXED;
      case StructureBrain.TYPES.MEAL_GREAT_MIXED:
        return InventoryItem.ITEM_TYPE.MEAL_GREAT_MIXED;
      case StructureBrain.TYPES.MEAL_DEADLY:
        return InventoryItem.ITEM_TYPE.MEAL_DEADLY;
      case StructureBrain.TYPES.MEAL_BAD_MEAT:
        return InventoryItem.ITEM_TYPE.MEAL_BAD_MEAT;
      case StructureBrain.TYPES.MEAL_GREAT_MEAT:
        return InventoryItem.ITEM_TYPE.MEAL_GREAT_MEAT;
      case StructureBrain.TYPES.MEAL_BURNED:
        return InventoryItem.ITEM_TYPE.MEAL_BURNED;
      case StructureBrain.TYPES.MEAL_EGG:
        return InventoryItem.ITEM_TYPE.MEAL_EGG;
      case StructureBrain.TYPES.DRINK_BEER:
        return InventoryItem.ITEM_TYPE.DRINK_BEER;
      case StructureBrain.TYPES.DRINK_WINE:
        return InventoryItem.ITEM_TYPE.DRINK_WINE;
      case StructureBrain.TYPES.DRINK_COCKTAIL:
        return InventoryItem.ITEM_TYPE.DRINK_COCKTAIL;
      case StructureBrain.TYPES.DRINK_EGGNOG:
        return InventoryItem.ITEM_TYPE.DRINK_EGGNOG;
      case StructureBrain.TYPES.DRINK_MUSHROOMJUICE:
        return InventoryItem.ITEM_TYPE.DRINK_MUSHROOM_JUICE;
      case StructureBrain.TYPES.DRINK_POOPJUICE:
        return InventoryItem.ITEM_TYPE.DRINK_POOP_JUICE;
      case StructureBrain.TYPES.DRINK_GIN:
        return InventoryItem.ITEM_TYPE.DRINK_GIN;
      case StructureBrain.TYPES.MEAL_SPICY:
        return InventoryItem.ITEM_TYPE.MEAL_SPICY;
      case StructureBrain.TYPES.MEAL_SNOW_FRUIT:
        return InventoryItem.ITEM_TYPE.MEAL_SNOW_FRUIT;
      case StructureBrain.TYPES.DRINK_CHILLI:
        return InventoryItem.ITEM_TYPE.DRINK_CHILLI;
      case StructureBrain.TYPES.DRINK_LIGHTNING:
        return InventoryItem.ITEM_TYPE.DRINK_LIGHTNING;
      case StructureBrain.TYPES.DRINK_SIN:
        return InventoryItem.ITEM_TYPE.DRINK_SIN;
      case StructureBrain.TYPES.DRINK_GRASS:
        return InventoryItem.ITEM_TYPE.DRINK_GRASS;
      case StructureBrain.TYPES.MEAL_MILK_BAD:
        return InventoryItem.ITEM_TYPE.MEAL_MILK_GOOD;
      case StructureBrain.TYPES.MEAL_MILK_GOOD:
        return InventoryItem.ITEM_TYPE.MEAL_MILK_GOOD;
      case StructureBrain.TYPES.MEAL_MILK_GREAT:
        return InventoryItem.ITEM_TYPE.MEAL_MILK_GREAT;
      case StructureBrain.TYPES.DRINK_MILKSHAKE:
        return InventoryItem.ITEM_TYPE.DRINK_MILKSHAKE;
      default:
        return InventoryItem.ITEM_TYPE.NONE;
    }
  }

  public static InventoryItem.ITEM_TYPE GetMealFromIngredientType(
    CookingData.IngredientType ingredientType)
  {
    switch (ingredientType)
    {
      case CookingData.IngredientType.FRUIT_LOW:
        return InventoryItem.ITEM_TYPE.MEAL_BERRIES;
      case CookingData.IngredientType.VEGETABLE_LOW:
        return InventoryItem.ITEM_TYPE.MEAL;
      case CookingData.IngredientType.VEGETABLE_MEDIUM:
        return InventoryItem.ITEM_TYPE.MEAL_MEDIUM_VEG;
      case CookingData.IngredientType.VEGETABLE_HIGH:
        return InventoryItem.ITEM_TYPE.MEAL_GREAT;
      case CookingData.IngredientType.MEAT_LOW:
        return InventoryItem.ITEM_TYPE.MEAL_BAD_MEAT;
      case CookingData.IngredientType.MEAT_MEDIUM:
        return InventoryItem.ITEM_TYPE.MEAL_MEAT;
      case CookingData.IngredientType.MEAT_HIGH:
        return InventoryItem.ITEM_TYPE.MEAL_GREAT_MEAT;
      case CookingData.IngredientType.FISH_LOW:
        return InventoryItem.ITEM_TYPE.MEAL_BAD_FISH;
      case CookingData.IngredientType.FISH_MEDIUM:
        return InventoryItem.ITEM_TYPE.MEAL_GOOD_FISH;
      case CookingData.IngredientType.FISH_HIGH:
        return InventoryItem.ITEM_TYPE.MEAL_GREAT_FISH;
      case CookingData.IngredientType.MIXED_LOW:
        return InventoryItem.ITEM_TYPE.MEAL_BAD_MIXED;
      case CookingData.IngredientType.MIXED_MEDIUM:
        return InventoryItem.ITEM_TYPE.MEAL_MEDIUM_MIXED;
      case CookingData.IngredientType.MIXED_HIGH:
        return InventoryItem.ITEM_TYPE.MEAL_GREAT_MIXED;
      case CookingData.IngredientType.SPECIAL_FOLLOWER_MEAT:
        return InventoryItem.ITEM_TYPE.MEAL_FOLLOWER_MEAT;
      case CookingData.IngredientType.SPECIAL_GRASS:
        return InventoryItem.ITEM_TYPE.MEAL_GRASS;
      case CookingData.IngredientType.SPECIAL_POOP:
        return InventoryItem.ITEM_TYPE.MEAL_POOP;
      case CookingData.IngredientType.SPECIAL_DEADLY:
        return InventoryItem.ITEM_TYPE.MEAL_DEADLY;
      default:
        return InventoryItem.ITEM_TYPE.NONE;
    }
  }

  public static CookingData.IngredientType GetIngredientFromMealType(
    InventoryItem.ITEM_TYPE mealType)
  {
    switch (mealType)
    {
      case InventoryItem.ITEM_TYPE.MEAL:
        return CookingData.IngredientType.VEGETABLE_LOW;
      case InventoryItem.ITEM_TYPE.MEAL_GRASS:
        return CookingData.IngredientType.SPECIAL_GRASS;
      case InventoryItem.ITEM_TYPE.MEAL_MEAT:
        return CookingData.IngredientType.MEAT_MEDIUM;
      case InventoryItem.ITEM_TYPE.MEAL_GREAT:
        return CookingData.IngredientType.VEGETABLE_HIGH;
      case InventoryItem.ITEM_TYPE.MEAL_GOOD_FISH:
        return CookingData.IngredientType.FISH_MEDIUM;
      case InventoryItem.ITEM_TYPE.MEAL_FOLLOWER_MEAT:
        return CookingData.IngredientType.SPECIAL_FOLLOWER_MEAT;
      case InventoryItem.ITEM_TYPE.MEAL_POOP:
        return CookingData.IngredientType.SPECIAL_POOP;
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_FISH:
        return CookingData.IngredientType.FISH_HIGH;
      case InventoryItem.ITEM_TYPE.MEAL_BAD_FISH:
        return CookingData.IngredientType.FISH_LOW;
      case InventoryItem.ITEM_TYPE.MEAL_BERRIES:
        return CookingData.IngredientType.FRUIT_LOW;
      case InventoryItem.ITEM_TYPE.MEAL_MEDIUM_VEG:
        return CookingData.IngredientType.VEGETABLE_MEDIUM;
      case InventoryItem.ITEM_TYPE.MEAL_BAD_MIXED:
        return CookingData.IngredientType.MIXED_LOW;
      case InventoryItem.ITEM_TYPE.MEAL_MEDIUM_MIXED:
        return CookingData.IngredientType.MIXED_MEDIUM;
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_MIXED:
        return CookingData.IngredientType.MIXED_HIGH;
      case InventoryItem.ITEM_TYPE.MEAL_DEADLY:
        return CookingData.IngredientType.SPECIAL_DEADLY;
      case InventoryItem.ITEM_TYPE.MEAL_BAD_MEAT:
        return CookingData.IngredientType.MEAT_LOW;
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_MEAT:
        return CookingData.IngredientType.MEAT_HIGH;
      default:
        return CookingData.IngredientType.NONE;
    }
  }

  public static InventoryItem.ITEM_TYPE[] GetAllMeals()
  {
    return new InventoryItem.ITEM_TYPE[23]
    {
      InventoryItem.ITEM_TYPE.MEAL_GREAT_MEAT,
      InventoryItem.ITEM_TYPE.MEAL_GREAT_FISH,
      InventoryItem.ITEM_TYPE.MEAL_GREAT,
      InventoryItem.ITEM_TYPE.MEAL_GREAT_MIXED,
      InventoryItem.ITEM_TYPE.MEAL_EGG,
      InventoryItem.ITEM_TYPE.MEAL_MEAT,
      InventoryItem.ITEM_TYPE.MEAL_GOOD_FISH,
      InventoryItem.ITEM_TYPE.MEAL_MEDIUM_VEG,
      InventoryItem.ITEM_TYPE.MEAL_MEDIUM_MIXED,
      InventoryItem.ITEM_TYPE.MEAL_SPICY,
      InventoryItem.ITEM_TYPE.MEAL_FOLLOWER_MEAT,
      InventoryItem.ITEM_TYPE.MEAL_BAD_FISH,
      InventoryItem.ITEM_TYPE.MEAL_BAD_MEAT,
      InventoryItem.ITEM_TYPE.MEAL_BAD_MIXED,
      InventoryItem.ITEM_TYPE.MEAL,
      InventoryItem.ITEM_TYPE.MEAL_BERRIES,
      InventoryItem.ITEM_TYPE.MEAL_GRASS,
      InventoryItem.ITEM_TYPE.MEAL_POOP,
      InventoryItem.ITEM_TYPE.MEAL_DEADLY,
      InventoryItem.ITEM_TYPE.MEAL_SNOW_FRUIT,
      InventoryItem.ITEM_TYPE.MEAL_MILK_BAD,
      InventoryItem.ITEM_TYPE.MEAL_MILK_GOOD,
      InventoryItem.ITEM_TYPE.MEAL_MILK_GREAT
    };
  }

  public static InventoryItem.ITEM_TYPE[] GetAllGreatMeals()
  {
    return new InventoryItem.ITEM_TYPE[6]
    {
      InventoryItem.ITEM_TYPE.MEAL_GREAT_MEAT,
      InventoryItem.ITEM_TYPE.MEAL_GREAT_FISH,
      InventoryItem.ITEM_TYPE.MEAL_GREAT,
      InventoryItem.ITEM_TYPE.MEAL_GREAT_MIXED,
      InventoryItem.ITEM_TYPE.MEAL_EGG,
      InventoryItem.ITEM_TYPE.MEAL_MILK_GREAT
    };
  }

  public static InventoryItem.ITEM_TYPE[] GetAllGoodMeals()
  {
    return new InventoryItem.ITEM_TYPE[6]
    {
      InventoryItem.ITEM_TYPE.MEAL_GOOD_FISH,
      InventoryItem.ITEM_TYPE.MEAL_MEAT,
      InventoryItem.ITEM_TYPE.MEAL_MEDIUM_MIXED,
      InventoryItem.ITEM_TYPE.MEAL_MEDIUM_VEG,
      InventoryItem.ITEM_TYPE.MEAL_SPICY,
      InventoryItem.ITEM_TYPE.MEAL_MILK_GOOD
    };
  }

  public static InventoryItem.ITEM_TYPE[] GetAllDrinks()
  {
    return new InventoryItem.ITEM_TYPE[11]
    {
      InventoryItem.ITEM_TYPE.DRINK_BEER,
      InventoryItem.ITEM_TYPE.DRINK_COCKTAIL,
      InventoryItem.ITEM_TYPE.DRINK_GIN,
      InventoryItem.ITEM_TYPE.DRINK_WINE,
      InventoryItem.ITEM_TYPE.DRINK_EGGNOG,
      InventoryItem.ITEM_TYPE.DRINK_POOP_JUICE,
      InventoryItem.ITEM_TYPE.DRINK_CHILLI,
      InventoryItem.ITEM_TYPE.DRINK_LIGHTNING,
      InventoryItem.ITEM_TYPE.DRINK_SIN,
      InventoryItem.ITEM_TYPE.DRINK_GRASS,
      InventoryItem.ITEM_TYPE.DRINK_MILKSHAKE
    };
  }

  public static InventoryItem.ITEM_TYPE[] GettAllWinterDrinks()
  {
    return new InventoryItem.ITEM_TYPE[3]
    {
      InventoryItem.ITEM_TYPE.DRINK_CHILLI,
      InventoryItem.ITEM_TYPE.DRINK_LIGHTNING,
      InventoryItem.ITEM_TYPE.DRINK_MILKSHAKE
    };
  }

  public static FollowerBrain.PleasureActions GetPleasure(InventoryItem.ITEM_TYPE item)
  {
    switch (item)
    {
      case InventoryItem.ITEM_TYPE.DRINK_BEER:
        return FollowerBrain.PleasureActions.Drink_Beer;
      case InventoryItem.ITEM_TYPE.DRINK_WINE:
        return FollowerBrain.PleasureActions.Drink_Wine;
      case InventoryItem.ITEM_TYPE.DRINK_COCKTAIL:
        return FollowerBrain.PleasureActions.Drink_Cocktail;
      case InventoryItem.ITEM_TYPE.DRINK_EGGNOG:
        return FollowerBrain.PleasureActions.Drink_Eggnog;
      case InventoryItem.ITEM_TYPE.DRINK_POOP_JUICE:
        return FollowerBrain.PleasureActions.Drink_Poop_Juice;
      case InventoryItem.ITEM_TYPE.DRINK_GIN:
        return FollowerBrain.PleasureActions.Drink_Gin;
      case InventoryItem.ITEM_TYPE.DRINK_MUSHROOM_JUICE:
        return FollowerBrain.PleasureActions.Drink_Mushroom_Juice;
      case InventoryItem.ITEM_TYPE.CHILLI:
        return FollowerBrain.PleasureActions.Drink_Chilli;
      case InventoryItem.ITEM_TYPE.DRINK_LIGHTNING:
        return FollowerBrain.PleasureActions.Drink_Lightning;
      case InventoryItem.ITEM_TYPE.DRINK_SIN:
        return FollowerBrain.PleasureActions.Drink_Sin;
      case InventoryItem.ITEM_TYPE.DRINK_GRASS:
        return FollowerBrain.PleasureActions.Drink_Grass;
      default:
        return FollowerBrain.PleasureActions.Drink_Beer;
    }
  }

  public static CookingData.MealEffect[] GetMealEffects(InventoryItem.ITEM_TYPE mealType)
  {
    if (CookingData.REQUIRES_LOC)
      return new CookingData.MealEffect[0];
    switch (mealType)
    {
      case InventoryItem.ITEM_TYPE.MEAL:
        return new CookingData.MealEffect[1]
        {
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.CausesIllPoopy,
            Chance = 5
          }
        };
      case InventoryItem.ITEM_TYPE.MEAL_GRASS:
        return new CookingData.MealEffect[1]
        {
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.CausesIllness,
            Chance = 25
          }
        };
      case InventoryItem.ITEM_TYPE.MEAL_MEAT:
        return new CookingData.MealEffect[2]
        {
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.DropLoot,
            Chance = 25
          },
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.InstantlyPoop,
            Chance = 15
          }
        };
      case InventoryItem.ITEM_TYPE.MEAL_GREAT:
        return new CookingData.MealEffect[1]
        {
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.IncreasesLoyalty,
            Chance = 50
          }
        };
      case InventoryItem.ITEM_TYPE.MEAL_GOOD_FISH:
        return new CookingData.MealEffect[2]
        {
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.DropLoot,
            Chance = 25
          },
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.InstantlyVomit,
            Chance = 15
          }
        };
      case InventoryItem.ITEM_TYPE.MEAL_FOLLOWER_MEAT:
        CookingData.MealEffect[] mealEffects1 = new CookingData.MealEffect[3];
        mealEffects1[0] = new CookingData.MealEffect()
        {
          MealEffectType = CookingData.MealEffectType.CausesIllness,
          Chance = 75
        };
        CookingData.MealEffect mealEffect1 = new CookingData.MealEffect();
        mealEffect1.MealEffectType = CookingData.MealEffectType.IncreasesLoyalty;
        mealEffect1.Chance = 25;
        mealEffects1[1] = mealEffect1;
        mealEffect1 = new CookingData.MealEffect();
        mealEffect1.MealEffectType = CookingData.MealEffectType.RemovesDissent;
        mealEffect1.Chance = 40;
        mealEffects1[2] = mealEffect1;
        return mealEffects1;
      case InventoryItem.ITEM_TYPE.MEAL_POOP:
        return new CookingData.MealEffect[1]
        {
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.CausesIllPoopy,
            Chance = 50
          }
        };
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_FISH:
        return new CookingData.MealEffect[2]
        {
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.DropLoot,
            Chance = 25
          },
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.RemovesIllness,
            Chance = 30
          }
        };
      case InventoryItem.ITEM_TYPE.MEAL_BAD_FISH:
        return new CookingData.MealEffect[1]
        {
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.CausesIllness,
            Chance = 10
          }
        };
      case InventoryItem.ITEM_TYPE.MEAL_BERRIES:
        return new CookingData.MealEffect[1]
        {
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.InstantlyPoop,
            Chance = 15
          }
        };
      case InventoryItem.ITEM_TYPE.MEAL_MEDIUM_VEG:
        return new CookingData.MealEffect[2]
        {
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.DropLoot,
            Chance = 25
          },
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.CausesIllness,
            Chance = 5
          }
        };
      case InventoryItem.ITEM_TYPE.MEAL_BAD_MIXED:
        return new CookingData.MealEffect[1]
        {
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.IncreasesLoyalty,
            Chance = 10
          }
        };
      case InventoryItem.ITEM_TYPE.MEAL_MEDIUM_MIXED:
        return new CookingData.MealEffect[1]
        {
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.IncreasesLoyalty,
            Chance = 20
          }
        };
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_MIXED:
        return new CookingData.MealEffect[2]
        {
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.IncreasesLoyalty,
            Chance = 100
          },
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.RemovesDissent,
            Chance = 100
          }
        };
      case InventoryItem.ITEM_TYPE.MEAL_DEADLY:
        return new CookingData.MealEffect[2]
        {
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.InstantlyDie,
            Chance = 75
          },
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.DropLoot,
            Chance = 100
          }
        };
      case InventoryItem.ITEM_TYPE.MEAL_BAD_MEAT:
        return new CookingData.MealEffect[1]
        {
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.CausesExhaustion,
            Chance = 10
          }
        };
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_MEAT:
        return new CookingData.MealEffect[1]
        {
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.DropLoot,
            Chance = 75
          }
        };
      case InventoryItem.ITEM_TYPE.MEAL_SPICY:
        return new CookingData.MealEffect[1]
        {
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.RemoveFreezing,
            Chance = 75
          }
        };
      case InventoryItem.ITEM_TYPE.DRINK_BEER:
        return new CookingData.MealEffect[1]
        {
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.CausesDrunk,
            Chance = 75
          }
        };
      case InventoryItem.ITEM_TYPE.DRINK_WINE:
        return new CookingData.MealEffect[2]
        {
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.CausesDrunk,
            Chance = 75
          },
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.IncreasesLoyalty,
            Chance = 25
          }
        };
      case InventoryItem.ITEM_TYPE.DRINK_COCKTAIL:
        return new CookingData.MealEffect[1]
        {
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.CausesDrunk,
            Chance = 75
          }
        };
      case InventoryItem.ITEM_TYPE.DRINK_EGGNOG:
        CookingData.MealEffect[] mealEffects2 = new CookingData.MealEffect[3];
        mealEffects2[0] = new CookingData.MealEffect()
        {
          MealEffectType = CookingData.MealEffectType.CausesDrunk,
          Chance = 75
        };
        CookingData.MealEffect mealEffect2 = new CookingData.MealEffect();
        mealEffect2.MealEffectType = CookingData.MealEffectType.DropLoot;
        mealEffect2.Chance = 100;
        mealEffects2[1] = mealEffect2;
        mealEffect2 = new CookingData.MealEffect();
        mealEffect2.MealEffectType = CookingData.MealEffectType.IncreasesLoyalty;
        mealEffect2.Chance = 100;
        mealEffects2[2] = mealEffect2;
        return mealEffects2;
      case InventoryItem.ITEM_TYPE.DRINK_POOP_JUICE:
        return new CookingData.MealEffect[2]
        {
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.CausesDrunk,
            Chance = 75
          },
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.CausesIllPoopy,
            Chance = 75
          }
        };
      case InventoryItem.ITEM_TYPE.DRINK_GIN:
        return new CookingData.MealEffect[2]
        {
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.CausesDrunk,
            Chance = 75
          },
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.DropLoot,
            Chance = 25
          }
        };
      case InventoryItem.ITEM_TYPE.DRINK_MUSHROOM_JUICE:
        return new CookingData.MealEffect[2]
        {
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.CausesDrunk,
            Chance = 75
          },
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.RemovesDissent,
            Chance = 50
          }
        };
      case InventoryItem.ITEM_TYPE.MEAL_EGG:
        return new CookingData.MealEffect[1]
        {
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.OldFollowerYoung,
            Chance = 100
          }
        };
      case InventoryItem.ITEM_TYPE.MEAL_SNOW_FRUIT:
        return new CookingData.MealEffect[1]
        {
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.GivesWarmBloodedTrait,
            Chance = 100
          }
        };
      case InventoryItem.ITEM_TYPE.DRINK_CHILLI:
        return new CookingData.MealEffect[2]
        {
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.CausesDrunk,
            Chance = 75
          },
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.RemoveFreezing,
            Chance = 50
          }
        };
      case InventoryItem.ITEM_TYPE.DRINK_LIGHTNING:
        return new CookingData.MealEffect[2]
        {
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.CausesDrunk,
            Chance = 75
          },
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.RemoveMutation,
            Chance = 25
          }
        };
      case InventoryItem.ITEM_TYPE.DRINK_SIN:
        return new CookingData.MealEffect[2]
        {
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.CausesDrunk,
            Chance = 100
          },
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.RemoveMajorNegativeStates,
            Chance = 100
          }
        };
      case InventoryItem.ITEM_TYPE.DRINK_GRASS:
        return new CookingData.MealEffect[2]
        {
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.CausesDrunk,
            Chance = 75
          },
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.AddRandomTrait,
            Chance = 30
          }
        };
      case InventoryItem.ITEM_TYPE.MEAL_MILK_BAD:
        return new CookingData.MealEffect[1]
        {
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.CausesIllness,
            Chance = 30
          }
        };
      case InventoryItem.ITEM_TYPE.MEAL_MILK_GOOD:
        return new CookingData.MealEffect[1]
        {
          new CookingData.MealEffect()
          {
            MealEffectType = CookingData.MealEffectType.CausesIllness,
            Chance = 10
          }
        };
      case InventoryItem.ITEM_TYPE.DRINK_MILKSHAKE:
        CookingData.MealEffect[] mealEffects3 = new CookingData.MealEffect[3];
        mealEffects3[0] = new CookingData.MealEffect()
        {
          MealEffectType = CookingData.MealEffectType.CausesIllPoopy,
          Chance = 10
        };
        CookingData.MealEffect mealEffect3 = new CookingData.MealEffect();
        mealEffect3.MealEffectType = CookingData.MealEffectType.GivesWarmBloodedTrait;
        mealEffect3.Chance = 75;
        mealEffects3[1] = mealEffect3;
        mealEffect3 = new CookingData.MealEffect();
        mealEffect3.MealEffectType = CookingData.MealEffectType.IncreasesLoyalty;
        mealEffect3.Chance = 100;
        mealEffects3[2] = mealEffect3;
        return mealEffects3;
      default:
        return new CookingData.MealEffect[0];
    }
  }

  public static void DoMealEffect(InventoryItem.ITEM_TYPE meal, FollowerBrain follower)
  {
    if (CookingData.REQUIRES_LOC || meal == InventoryItem.ITEM_TYPE.MEAL_GRASS && DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.GrassEater))
      return;
    CookingData.MealEffect[] mealEffects = CookingData.GetMealEffects(meal);
    if (meal == InventoryItem.ITEM_TYPE.MEAL_DEADLY)
    {
      if (UnityEngine.Random.Range(0, 100) < 75)
        CookingData.InstantlyDie(follower);
      else
        CookingData.DropLoot(follower);
    }
    else
    {
      foreach (CookingData.MealEffect mealEffect in mealEffects)
      {
        int num = CookingData.DrawEffectChance(mealEffect, follower);
        Debug.Log((object) $"{mealEffect.MealEffectType.ToString()}  chance: {num.ToString()} / {mealEffect.Chance.ToString()}  {(num < mealEffect.Chance).ToString()}");
        FollowerTask task;
        if (num < mealEffect.Chance)
        {
          switch (mealEffect.MealEffectType)
          {
            case CookingData.MealEffectType.InstantlyPoop:
              if ((double) IllnessBar.IllnessNormalized > 0.05000000074505806)
              {
                GameManager.GetInstance().StartCoroutine((IEnumerator) CookingData.FrameDelay((System.Action) (() =>
                {
                  task = (FollowerTask) new FollowerTask_InstantPoop();
                  follower.HardSwapToTask(task);
                })));
                continue;
              }
              continue;
            case CookingData.MealEffectType.InstantlyVomit:
              if ((double) IllnessBar.IllnessNormalized > 0.05000000074505806)
              {
                GameManager.GetInstance().StartCoroutine((IEnumerator) CookingData.FrameDelay((System.Action) (() =>
                {
                  task = (FollowerTask) new FollowerTask_Vomit();
                  follower.HardSwapToTask(task);
                })));
                continue;
              }
              continue;
            case CookingData.MealEffectType.InstantlyDie:
              CookingData.InstantlyDie(follower);
              return;
            case CookingData.MealEffectType.RemovesDissent:
              CookingData.RemoveDissent(follower);
              continue;
            case CookingData.MealEffectType.RemovesIllness:
              CookingData.RemoveIllness(follower);
              continue;
            case CookingData.MealEffectType.CausesIllness:
              if ((meal != InventoryItem.ITEM_TYPE.MEAL_FOLLOWER_MEAT || !DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.Cannibal)) && (DataManager.Instance.OnboardedSickFollower || TimeManager.CurrentDay >= 10))
              {
                follower.MakeSick();
                continue;
              }
              continue;
            case CookingData.MealEffectType.CausesExhaustion:
              GameManager.GetInstance().StartCoroutine((IEnumerator) CookingData.FrameDelay((System.Action) (() => follower.MakeExhausted())));
              continue;
            case CookingData.MealEffectType.CausesDissent:
              follower.MakeDissenter();
              continue;
            case CookingData.MealEffectType.CausesIllnessOrDissent:
              if (UnityEngine.Random.Range(0, 100) > 50)
              {
                if (DataManager.Instance.OnboardedDissenter || TimeManager.CurrentDay >= 10)
                {
                  follower.MakeDissenter();
                  continue;
                }
                continue;
              }
              if (DataManager.Instance.OnboardedSickFollower || TimeManager.CurrentDay >= 10)
              {
                follower.MakeSick();
                continue;
              }
              continue;
            case CookingData.MealEffectType.CausesIllPoopy:
              if (DataManager.Instance.OnboardedSickFollower || TimeManager.CurrentDay >= 10)
              {
                follower.MakeSick();
                follower._directInfoAccess.CursedStateVariant = 1;
                GameManager.GetInstance().StartCoroutine((IEnumerator) CookingData.FrameDelay((System.Action) (() =>
                {
                  task = (FollowerTask) new FollowerTask_IllPoopy(true);
                  follower.HardSwapToTask(task);
                })));
                continue;
              }
              continue;
            case CookingData.MealEffectType.DropLoot:
              CookingData.DropLoot(follower);
              continue;
            case CookingData.MealEffectType.IncreasesLoyalty:
              follower.AddAdoration(FollowerBrain.AdorationActions.BigGift, (System.Action) null);
              continue;
            case CookingData.MealEffectType.HealsIllness:
              follower.Stats.Illness = 0.0f;
              FollowerBrainStats.StatStateChangedEvent illnessStateChanged = FollowerBrainStats.OnIllnessStateChanged;
              if (illnessStateChanged != null)
              {
                illnessStateChanged(follower.Info.ID, FollowerStatState.Off, FollowerStatState.On);
                continue;
              }
              continue;
            case CookingData.MealEffectType.CausesDrunk:
              follower.MakeDrunk();
              continue;
            case CookingData.MealEffectType.OldFollowerYoung:
              follower.Info.Age = Mathf.Max(Mathf.RoundToInt((float) follower.Info.Age / 2f), 20);
              follower.DiedOfOldAge = false;
              follower.RemoveCurseState(Thought.OldAge);
              follower.CompleteCurrentTask();
              Follower followerById1 = FollowerManager.FindFollowerByID(follower.Info.ID);
              if ((UnityEngine.Object) followerById1 != (UnityEngine.Object) null)
              {
                followerById1.SimpleAnimator.ResetAnimationsToDefaults();
                FollowerBrain.SetFollowerCostume(followerById1.Spine.Skeleton, follower._directInfoAccess, forceUpdate: true);
                continue;
              }
              continue;
            case CookingData.MealEffectType.RemoveFreezing:
              CookingData.RemoveFreezing(follower);
              continue;
            case CookingData.MealEffectType.GivesWarmBloodedTrait:
              follower.AddTrait(FollowerTrait.TraitType.WarmBlooded, true);
              continue;
            case CookingData.MealEffectType.RemoveMutation:
              if (follower.HasTrait(FollowerTrait.TraitType.Mutated))
              {
                follower.RemoveTrait(FollowerTrait.TraitType.Mutated, true);
                Follower followerById2 = FollowerManager.FindFollowerByID(follower.Info.ID);
                if ((UnityEngine.Object) followerById2 != (UnityEngine.Object) null)
                {
                  FollowerBrain.SetFollowerCostume(followerById2.Spine.Skeleton, follower._directInfoAccess, forceUpdate: true);
                  continue;
                }
                continue;
              }
              continue;
            case CookingData.MealEffectType.RemoveStarvation:
              CookingData.RemoveStarvation(follower);
              continue;
            case CookingData.MealEffectType.RemoveMajorNegativeStates:
              CookingData.RemoveFreezing(follower);
              CookingData.RemoveDissent(follower);
              CookingData.RemoveIllness(follower);
              CookingData.RemoveStarvation(follower);
              continue;
            case CookingData.MealEffectType.AddRandomTrait:
              if (follower._directInfoAccess.Traits.Count < 6)
              {
                List<FollowerTrait.TraitType> traitTypeList = follower._directInfoAccess.RandomisedTraits(UnityEngine.Random.Range(-2147483647 /*0x80000001*/, int.MaxValue));
                if (traitTypeList.Count > 0)
                {
                  follower.AddTrait(traitTypeList[0], true);
                  continue;
                }
                continue;
              }
              continue;
            default:
              continue;
          }
        }
      }
    }
  }

  public static void RemoveStarvation(FollowerBrain follower)
  {
    if (follower.Info.CursedState != Thought.BecomeStarving)
      return;
    NotificationCentre.Instance.PlayFollowerNotification(NotificationCentre.NotificationType.NoLongerStarving, follower.Info, NotificationFollower.Animation.Happy);
    follower.Stats.Starvation = 0.0f;
    FollowerBrainStats.StatStateChangedEvent starvationStateChanged = FollowerBrainStats.OnStarvationStateChanged;
    if (starvationStateChanged == null)
      return;
    starvationStateChanged(follower.Info.ID, FollowerStatState.Off, FollowerStatState.On);
  }

  public static void RemoveIllness(FollowerBrain follower)
  {
    if (follower.Info.CursedState != Thought.Ill)
      return;
    NotificationCentre.Instance.PlayFollowerNotification(NotificationCentre.NotificationType.NoLongerIll, follower.Info, NotificationFollower.Animation.Happy);
    follower.Stats.Illness = 0.0f;
    FollowerBrainStats.StatStateChangedEvent illnessStateChanged = FollowerBrainStats.OnIllnessStateChanged;
    if (illnessStateChanged == null)
      return;
    illnessStateChanged(follower.Info.ID, FollowerStatState.Off, FollowerStatState.On);
  }

  public static void RemoveDissent(FollowerBrain follower)
  {
    if (follower.Info.CursedState != Thought.Dissenter)
      return;
    NotificationCentre.Instance.PlayFollowerNotification(NotificationCentre.NotificationType.StopBeingDissenter, follower.Info, NotificationFollower.Animation.Happy);
    if ((follower.Info.ID != 99996 || !DataManager.Instance.SozoNoLongerBrainwashed) && follower.Info.ID == 99996)
      return;
    follower.Stats.Reeducation = 0.0f;
  }

  public static void RemoveFreezing(FollowerBrain follower)
  {
    if ((double) follower.Stats.Freezing > 0.0)
      NotificationCentre.Instance.PlayFollowerNotification(NotificationCentre.NotificationType.NoLongerFreezing, follower.Info, NotificationFollower.Animation.Happy);
    follower.Stats.Freezing = 0.0f;
    FollowerBrainStats.StatStateChangedEvent freezingStateChanged = FollowerBrainStats.OnFreezingStateChanged;
    if (freezingStateChanged == null)
      return;
    freezingStateChanged(follower.Info.ID, FollowerStatState.Off, FollowerStatState.On);
  }

  public static void InstantlyDie(FollowerBrain follower)
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) CookingData.FrameDelay((System.Action) (() =>
    {
      if ((UnityEngine.Object) FollowerManager.FindFollowerByID(follower.Info.ID) != (UnityEngine.Object) null)
        FollowerManager.FindFollowerByID(follower.Info.ID).Die(NotificationCentre.NotificationType.DiedFromDeadlyMeal);
      else
        follower.Die(NotificationCentre.NotificationType.DiedFromDeadlyMeal);
    })));
  }

  public static void DropLoot(FollowerBrain follower)
  {
    List<InventoryItem.ITEM_TYPE> itemTypeList = new List<InventoryItem.ITEM_TYPE>();
    for (int index = 0; index < UnityEngine.Random.Range(7, 13); ++index)
    {
      int num = UnityEngine.Random.Range(0, 100);
      if (num < 33)
        itemTypeList.Add(InventoryItem.ITEM_TYPE.BLACK_GOLD);
      else if (num < 66)
        itemTypeList.Add(InventoryItem.ITEM_TYPE.LOG);
      else
        itemTypeList.Add(InventoryItem.ITEM_TYPE.STONE);
    }
    if (PlayerFarming.Location == FollowerLocation.Base && (UnityEngine.Object) BaseLocationManager.Instance != (UnityEngine.Object) null)
    {
      foreach (InventoryItem.ITEM_TYPE type in itemTypeList)
      {
        PickUp pickUp = InventoryItem.Spawn(type, 1, follower.LastPosition);
        pickUp.transform.parent = BaseLocationManager.Instance.UnitLayer.transform;
        pickUp.SetInitialSpeedAndDiraction(4f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
      }
    }
    else
    {
      List<Structures_CollectedResourceChest> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_CollectedResourceChest>(FollowerLocation.Base);
      if (structuresOfType.Count <= 0)
        return;
      foreach (InventoryItem.ITEM_TYPE ItemType in itemTypeList)
        structuresOfType[0].AddItem(ItemType, 1);
    }
  }

  public static int DrawEffectChance(CookingData.MealEffect mealEffect, FollowerBrain follower)
  {
    int num = UnityEngine.Random.Range(0, 100);
    return follower.HasTrait(FollowerTrait.TraitType.Heavyweight) && mealEffect.MealEffectType == CookingData.MealEffectType.CausesDrunk && num < mealEffect.Chance && (double) num > (double) mealEffect.Chance * 0.34999999403953552 ? 100 : num;
  }

  public static IEnumerator FrameDelay(System.Action callback)
  {
    yield return (object) new WaitForEndOfFrame();
    System.Action action = callback;
    if (action != null)
      action();
  }

  public static string GetEffectDescription(
    CookingData.MealEffect mealEffect,
    InventoryItem.ITEM_TYPE mealType)
  {
    string str1 = LocalizeIntegration.ReverseText(mealEffect.Chance.ToString());
    string str2 = string.Format(LocalizationManager.GetTranslation($"CookingData/{mealEffect.MealEffectType}/Description"), (object) str1);
    if (mealType == InventoryItem.ITEM_TYPE.MEAL_FOLLOWER_MEAT && DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.Cannibal) && mealEffect.MealEffectType == CookingData.MealEffectType.CausesIllness)
    {
      Debug.Log((object) ("r: " + str2.Colour(Color.yellow)));
      str2 = $"{str2.Insert(31 /*0x1F*/, "<s>")}</s> \n \n<sprite name=\"icon_Trait_Cannibal\"> <color=#FFD201>{FollowerTrait.GetLocalizedTitle(FollowerTrait.TraitType.Cannibal)}</color> \n \n";
    }
    if (mealType == InventoryItem.ITEM_TYPE.MEAL_GRASS && DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.GrassEater))
      str2 = $"{str2.Insert(31 /*0x1F*/, "<s>")}</s> \n \n<sprite name=\"icon_Trait_GrassEater\"> <color=#FFD201>{FollowerTrait.GetLocalizedTitle(FollowerTrait.TraitType.GrassEater)}";
    return str2;
  }

  public static string GetMealSkin(StructureBrain.TYPES mealType)
  {
    switch (mealType)
    {
      case StructureBrain.TYPES.MEAL:
        return "Meals/Meal";
      case StructureBrain.TYPES.MEAL_MEAT:
        return "Meals/MealGood";
      case StructureBrain.TYPES.MEAL_GREAT:
        return "Meals/MealGreat";
      case StructureBrain.TYPES.MEAL_GRASS:
        return "Meals/MealGrass";
      case StructureBrain.TYPES.MEAL_GOOD_FISH:
        return "Meals/MealGoodFish";
      case StructureBrain.TYPES.MEAL_FOLLOWER_MEAT:
        return "Meals/MealFollowerMeat";
      case StructureBrain.TYPES.MEAL_MUSHROOMS:
        return "Meals/MealMushrooms";
      case StructureBrain.TYPES.MEAL_POOP:
        return "Meals/MealPoop";
      case StructureBrain.TYPES.MEAL_ROTTEN:
        return "Meals/MealRotten";
      case StructureBrain.TYPES.MEAL_GREAT_FISH:
        return "Meals/MealGreatFish";
      case StructureBrain.TYPES.MEAL_BAD_FISH:
        return "Meals/MealBadFish";
      case StructureBrain.TYPES.MEAL_BERRIES:
        return "Meals/MealBerries";
      case StructureBrain.TYPES.MEAL_MEDIUM_VEG:
        return "Meals/MealMediumVeg";
      case StructureBrain.TYPES.MEAL_BAD_MIXED:
        return "Meals/MealBadMixed";
      case StructureBrain.TYPES.MEAL_MEDIUM_MIXED:
        return "Meals/MealMediumMixed";
      case StructureBrain.TYPES.MEAL_GREAT_MIXED:
        return "Meals/MealGreatMixed";
      case StructureBrain.TYPES.MEAL_DEADLY:
        return "Meals/MealDeadly";
      case StructureBrain.TYPES.MEAL_BAD_MEAT:
        return "Meals/MealBadMeat";
      case StructureBrain.TYPES.MEAL_GREAT_MEAT:
        return "Meals/MealGreatMeat";
      case StructureBrain.TYPES.MEAL_BURNED:
        return "Meals/MealBurnt";
      case StructureBrain.TYPES.MEAL_EGG:
        return "Meals/MealEgg";
      case StructureBrain.TYPES.MEAL_SPICY:
        return "Meals/MealSpicy";
      case StructureBrain.TYPES.MEAL_SNOW_FRUIT:
        return "Meals/MealSnowFruit";
      case StructureBrain.TYPES.MEAL_MILK_BAD:
        return "Meals/MealMilkBad";
      case StructureBrain.TYPES.MEAL_MILK_GOOD:
        return "Meals/MealMilkGood";
      case StructureBrain.TYPES.MEAL_MILK_GREAT:
        return "Meals/MealMilkGreat";
      default:
        return "Meals/Meal";
    }
  }

  public static string GetDrinkSkin(InventoryItem.ITEM_TYPE drinkType)
  {
    switch (drinkType)
    {
      case InventoryItem.ITEM_TYPE.DRINK_WINE:
        return "Drinks/Wine";
      case InventoryItem.ITEM_TYPE.DRINK_COCKTAIL:
        return "Drinks/Cocktail";
      case InventoryItem.ITEM_TYPE.DRINK_EGGNOG:
        return "Drinks/Eggnog";
      case InventoryItem.ITEM_TYPE.DRINK_POOP_JUICE:
        return "Drinks/Poop";
      case InventoryItem.ITEM_TYPE.DRINK_GIN:
        return "Drinks/Gin";
      case InventoryItem.ITEM_TYPE.DRINK_MUSHROOM_JUICE:
        return "Drinks/Mushroom";
      case InventoryItem.ITEM_TYPE.DRINK_CHILLI:
        return "Drinks/Chilli";
      case InventoryItem.ITEM_TYPE.DRINK_LIGHTNING:
        return "Drinks/Charged";
      case InventoryItem.ITEM_TYPE.DRINK_SIN:
        return "Drinks/Sin";
      case InventoryItem.ITEM_TYPE.DRINK_GRASS:
        return "Drinks/Smoothie";
      case InventoryItem.ITEM_TYPE.DRINK_MILKSHAKE:
        return "Drinks/Milkshake";
      default:
        return "Drinks/Beer";
    }
  }

  public static float GetTotalHunger() => HungerBar.Count + HungerBar.ReservedSatiation;

  public static int GetDaysOfFood(List<InventoryItem> inventory, int followerCount)
  {
    if (followerCount <= 0)
      return inventory.Count <= 0 ? 0 : int.MaxValue;
    List<InventoryItem> resultingIngredients = (List<InventoryItem>) null;
    CookingData.GetGoodOrBetterMealsFromIngredients(inventory, out resultingIngredients);
    int num1 = 0;
    foreach (InventoryItem inventoryItem in resultingIngredients)
    {
      int satationAmount = CookingData.GetSatationAmount((InventoryItem.ITEM_TYPE) inventoryItem.type);
      num1 += satationAmount;
    }
    int num2 = followerCount * 60;
    return num1 / num2;
  }

  public static List<InventoryItem> GetFoodForDays(
    List<InventoryItem> inventory,
    int followerCount,
    int targetDays)
  {
    if (followerCount <= 0 || targetDays <= 0)
      return new List<InventoryItem>((IEnumerable<InventoryItem>) inventory);
    List<InventoryItem> mealsFromIngredients = CookingData.GetGoodOrBetterMealsFromIngredients(inventory, out List<InventoryItem> _);
    mealsFromIngredients.Shuffle<InventoryItem>();
    int num1 = followerCount * 60 * targetDays;
    int num2 = 0;
    List<InventoryItem> foodForDays = new List<InventoryItem>();
    List<InventoryItem.ITEM_TYPE> list = ((IEnumerable<InventoryItem.ITEM_TYPE>) CookingData.GetAllMeals()).ToList<InventoryItem.ITEM_TYPE>();
    foreach (InventoryItem inventoryItem in mealsFromIngredients)
    {
      if (list.Contains((InventoryItem.ITEM_TYPE) inventoryItem.type))
      {
        int satationAmount = CookingData.GetSatationAmount((InventoryItem.ITEM_TYPE) inventoryItem.type);
        if (num2 < num1)
        {
          num2 += satationAmount;
          List<InventoryItem> recipeSimplified = CookingData.GetRecipeSimplified((InventoryItem.ITEM_TYPE) inventoryItem.type);
          if (recipeSimplified != null && recipeSimplified.Count > 0)
            foodForDays.Add(recipeSimplified[0]);
        }
      }
    }
    return foodForDays;
  }

  public static List<InventoryItem> GetLeftoverFoodAfterDays(
    List<InventoryItem> inventory,
    int followerCount,
    int targetDays)
  {
    if (followerCount <= 0)
      return new List<InventoryItem>((IEnumerable<InventoryItem>) inventory);
    List<InventoryItem> mealsFromIngredients = CookingData.GetGoodOrBetterMealsFromIngredients(inventory, out List<InventoryItem> _);
    int num1 = followerCount * 60 * targetDays;
    int num2 = 0;
    List<InventoryItem> leftoverFoodAfterDays = new List<InventoryItem>();
    List<InventoryItem.ITEM_TYPE> list = ((IEnumerable<InventoryItem.ITEM_TYPE>) CookingData.GetAllMeals()).ToList<InventoryItem.ITEM_TYPE>();
    foreach (InventoryItem inventoryItem in mealsFromIngredients)
    {
      if (list.Contains((InventoryItem.ITEM_TYPE) inventoryItem.type))
      {
        int satationAmount = CookingData.GetSatationAmount((InventoryItem.ITEM_TYPE) inventoryItem.type);
        if (num2 < num1)
        {
          num2 += satationAmount;
        }
        else
        {
          List<InventoryItem> recipeSimplified = CookingData.GetRecipeSimplified((InventoryItem.ITEM_TYPE) inventoryItem.type);
          if (recipeSimplified != null && recipeSimplified.Count > 0)
            leftoverFoodAfterDays.Add(recipeSimplified[0]);
        }
      }
    }
    return leftoverFoodAfterDays;
  }

  public enum IngredientType
  {
    NONE = 0,
    FRUIT_LOW = 1,
    VEGETABLE_LOW = 100, // 0x00000064
    VEGETABLE_MEDIUM = 101, // 0x00000065
    VEGETABLE_HIGH = 102, // 0x00000066
    MEAT_LOW = 200, // 0x000000C8
    MEAT_MEDIUM = 201, // 0x000000C9
    MEAT_HIGH = 202, // 0x000000CA
    FISH_LOW = 300, // 0x0000012C
    FISH_MEDIUM = 301, // 0x0000012D
    FISH_HIGH = 302, // 0x0000012E
    MIXED_LOW = 400, // 0x00000190
    MIXED_MEDIUM = 401, // 0x00000191
    MIXED_HIGH = 402, // 0x00000192
    SPECIAL_FOLLOWER_MEAT = 500, // 0x000001F4
    SPECIAL_GRASS = 501, // 0x000001F5
    SPECIAL_POOP = 502, // 0x000001F6
    SPECIAL_DEADLY = 503, // 0x000001F7
    MEAT = 1000, // 0x000003E8
    VEGETABLE = 1001, // 0x000003E9
    FISH = 1002, // 0x000003EA
    FRUIT = 1003, // 0x000003EB
    MIXED = 1004, // 0x000003EC
    SPECIAL = 1005, // 0x000003ED
  }

  [Serializable]
  public struct MealEffect
  {
    public CookingData.MealEffectType MealEffectType;
    public int Chance;
  }

  public enum MealEffectType
  {
    None,
    InstantlyPoop,
    InstantlyVomit,
    InstantlyDie,
    RemovesDissent,
    RemovesIllness,
    CausesIllness,
    CausesExhaustion,
    CausesDissent,
    CausesIllnessOrDissent,
    CausesIllPoopy,
    DropLoot,
    IncreasesLoyalty,
    HealsIllness,
    CausesDrunk,
    CausesWorkAllNight,
    OldFollowerYoung,
    RemoveFreezing,
    GivesWarmBloodedTrait,
    RemoveMutation,
    RemoveStarvation,
    RemoveMajorNegativeStates,
    AddRandomTrait,
  }
}
