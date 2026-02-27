// Decompiled with JetBrains decompiler
// Type: CookingData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private const float RecipeBalanceValue = 2.25f;
  public static bool REQUIRES_LOC;

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

  public static int GetCookableRecipeAmount(InventoryItem.ITEM_TYPE mealType)
  {
    List<List<InventoryItem>> recipe = CookingData.GetRecipe(mealType);
    int cookableRecipeAmount = 0;
    using (List<List<InventoryItem>>.Enumerator enumerator = recipe.GetEnumerator())
    {
      if (enumerator.MoveNext())
      {
        List<InventoryItem> current = enumerator.Current;
        Dictionary<int, int> source = new Dictionary<int, int>();
        foreach (InventoryItem inventoryItem in Inventory.items)
        {
          if (source.ContainsKey(inventoryItem.type))
            source[inventoryItem.type] += inventoryItem.quantity;
          else
            source.Add(inventoryItem.type, inventoryItem.quantity);
        }
        while (recipe.Count != 0)
        {
          foreach (InventoryItem inventoryItem in current)
          {
            bool flag = false;
            for (int index = 0; index < source.Count; ++index)
            {
              if (inventoryItem.type == source.ElementAt<KeyValuePair<int, int>>(index).Key)
              {
                flag = true;
                if (source.ElementAt<KeyValuePair<int, int>>(index).Value < inventoryItem.quantity)
                  return cookableRecipeAmount;
                source[inventoryItem.type] -= inventoryItem.quantity;
              }
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

  private static int BalanceValue(int value) => Mathf.CeilToInt((float) value * 2.25f);

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
      default:
        return new List<List<InventoryItem>>();
    }
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
        return CookingData.IngredientType.FISH_MEDIUM;
      case InventoryItem.ITEM_TYPE.FISH_SMALL:
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
        return 0;
      case InventoryItem.ITEM_TYPE.MEAL_MEAT:
      case InventoryItem.ITEM_TYPE.MEAL_GOOD_FISH:
      case InventoryItem.ITEM_TYPE.MEAL_MEDIUM_VEG:
      case InventoryItem.ITEM_TYPE.MEAL_MEDIUM_MIXED:
        return 75;
      case InventoryItem.ITEM_TYPE.MEAL_GREAT:
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_FISH:
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_MIXED:
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_MEAT:
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
        return 0.5f;
      case InventoryItem.ITEM_TYPE.MEAL_GREAT:
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_FISH:
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_MIXED:
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_MEAT:
        return 1f;
      default:
        return 0.25f;
    }
  }

  public static int GetSatationLevel(InventoryItem.ITEM_TYPE meal)
  {
    switch (meal)
    {
      case InventoryItem.ITEM_TYPE.MEAL_MEAT:
      case InventoryItem.ITEM_TYPE.MEAL_GOOD_FISH:
      case InventoryItem.ITEM_TYPE.MEAL_MEDIUM_VEG:
      case InventoryItem.ITEM_TYPE.MEAL_MEDIUM_MIXED:
        return 2;
      case InventoryItem.ITEM_TYPE.MEAL_GREAT:
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_FISH:
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_MIXED:
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_MEAT:
        return 3;
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

  private static CookingData.IngredientType GetSpecialType(List<InventoryItem> ingredients)
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
        return 60f;
      case InventoryItem.ITEM_TYPE.MEAL_MEAT:
      case InventoryItem.ITEM_TYPE.MEAL_GOOD_FISH:
      case InventoryItem.ITEM_TYPE.MEAL_MEDIUM_VEG:
      case InventoryItem.ITEM_TYPE.MEAL_MEDIUM_MIXED:
        return 120f;
      case InventoryItem.ITEM_TYPE.MEAL_GREAT:
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_FISH:
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_MIXED:
      case InventoryItem.ITEM_TYPE.MEAL_GREAT_MEAT:
        return 180f;
      default:
        return 180f;
    }
  }

  public static void CookedMeal(InventoryItem.ITEM_TYPE mealType)
  {
    if (mealType == InventoryItem.ITEM_TYPE.MEAL_FOLLOWER_MEAT)
      AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("FEED_FOLLOWER_MEAT"));
    CookingData.TryDiscoverRecipe(mealType);
    if (mealType <= InventoryItem.ITEM_TYPE.MEAL_FOLLOWER_MEAT)
    {
      if (mealType != InventoryItem.ITEM_TYPE.NONE)
      {
        if (mealType != InventoryItem.ITEM_TYPE.MEAL)
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
            case InventoryItem.ITEM_TYPE.SEED:
              ++DataManager.Instance.FOLLOWER_MEAT_MEALS_COOKED;
              break;
          }
        }
        else
          ++DataManager.Instance.VEGETABLE_LOW_MEALS_COOKED;
      }
      else
        ++DataManager.Instance.MealsCooked;
    }
    else if (mealType <= InventoryItem.ITEM_TYPE.MEAL_GREAT_FISH)
    {
      if (mealType != InventoryItem.ITEM_TYPE.MEAL_POOP)
      {
        if (mealType != InventoryItem.ITEM_TYPE.MEAL_GREAT_FISH)
          return;
        ++DataManager.Instance.FISH_HIGH_MEALS_COOKED;
      }
      else
        ++DataManager.Instance.POOP_MEALS_COOKED;
    }
    else if (mealType != InventoryItem.ITEM_TYPE.MEAL_BAD_FISH)
    {
      switch (mealType - 106)
      {
        case InventoryItem.ITEM_TYPE.NONE:
          ++DataManager.Instance.FRUIT_LOW_MEALS_COOKED;
          break;
        case InventoryItem.ITEM_TYPE.LOG:
          ++DataManager.Instance.VEGETABLE_MEDIUM_MEALS_COOKED;
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
      }
    }
    else
      ++DataManager.Instance.FISH_LOW_MEALS_COOKED;
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
    return new InventoryItem.ITEM_TYPE[17]
    {
      InventoryItem.ITEM_TYPE.MEAL_GREAT_MEAT,
      InventoryItem.ITEM_TYPE.MEAL_GREAT_FISH,
      InventoryItem.ITEM_TYPE.MEAL_GREAT,
      InventoryItem.ITEM_TYPE.MEAL_GREAT_MIXED,
      InventoryItem.ITEM_TYPE.MEAL_MEAT,
      InventoryItem.ITEM_TYPE.MEAL_GOOD_FISH,
      InventoryItem.ITEM_TYPE.MEAL_MEDIUM_VEG,
      InventoryItem.ITEM_TYPE.MEAL_MEDIUM_MIXED,
      InventoryItem.ITEM_TYPE.MEAL_FOLLOWER_MEAT,
      InventoryItem.ITEM_TYPE.MEAL_BAD_FISH,
      InventoryItem.ITEM_TYPE.MEAL_BAD_MEAT,
      InventoryItem.ITEM_TYPE.MEAL_BAD_MIXED,
      InventoryItem.ITEM_TYPE.MEAL,
      InventoryItem.ITEM_TYPE.MEAL_BERRIES,
      InventoryItem.ITEM_TYPE.MEAL_GRASS,
      InventoryItem.ITEM_TYPE.MEAL_POOP,
      InventoryItem.ITEM_TYPE.MEAL_DEADLY
    };
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
        CookingData.MealEffect[] mealEffects = new CookingData.MealEffect[3];
        mealEffects[0] = new CookingData.MealEffect()
        {
          MealEffectType = CookingData.MealEffectType.CausesIllness,
          Chance = 75
        };
        CookingData.MealEffect mealEffect = new CookingData.MealEffect();
        mealEffect.MealEffectType = CookingData.MealEffectType.IncreasesLoyalty;
        mealEffect.Chance = 25;
        mealEffects[1] = mealEffect;
        mealEffect = new CookingData.MealEffect();
        mealEffect.MealEffectType = CookingData.MealEffectType.RemovesDissent;
        mealEffect.Chance = 40;
        mealEffects[2] = mealEffect;
        return mealEffects;
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
        int num = UnityEngine.Random.Range(0, 100);
        Debug.Log((object) $"{(object) mealEffect.MealEffectType}  chance: {(object) num} / {(object) mealEffect.Chance}  {(num < mealEffect.Chance).ToString()}");
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
              if (follower.Info.CursedState == Thought.Dissenter)
              {
                follower.Stats.Reeducation = 0.0f;
                continue;
              }
              continue;
            case CookingData.MealEffectType.RemovesIllness:
              if (follower.Info.CursedState == Thought.Ill)
              {
                follower.Stats.Illness = 0.0f;
                FollowerBrainStats.StatStateChangedEvent illnessStateChanged = FollowerBrainStats.OnIllnessStateChanged;
                if (illnessStateChanged != null)
                {
                  illnessStateChanged(follower.Info.ID, FollowerStatState.Off, FollowerStatState.On);
                  continue;
                }
                continue;
              }
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
              FollowerBrainStats.StatStateChangedEvent illnessStateChanged1 = FollowerBrainStats.OnIllnessStateChanged;
              if (illnessStateChanged1 != null)
              {
                illnessStateChanged1(follower.Info.ID, FollowerStatState.Off, FollowerStatState.On);
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

  private static void InstantlyDie(FollowerBrain follower)
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) CookingData.FrameDelay((System.Action) (() =>
    {
      if ((UnityEngine.Object) FollowerManager.FindFollowerByID(follower.Info.ID) != (UnityEngine.Object) null)
        FollowerManager.FindFollowerByID(follower.Info.ID).Die(NotificationCentre.NotificationType.DiedFromDeadlyMeal);
      else
        follower.Die(NotificationCentre.NotificationType.DiedFromDeadlyMeal);
    })));
  }

  private static void DropLoot(FollowerBrain follower)
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

  private static IEnumerator FrameDelay(System.Action callback)
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
    string str = string.Format(LocalizationManager.GetTranslation($"CookingData/{mealEffect.MealEffectType}/Description"), (object) mealEffect.Chance);
    if (mealType == InventoryItem.ITEM_TYPE.MEAL_FOLLOWER_MEAT && DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.Cannibal) && mealEffect.MealEffectType == CookingData.MealEffectType.CausesIllness)
    {
      Debug.Log((object) ("r: " + str.Colour(Color.yellow)));
      str = $"{str.Insert(31 /*0x1F*/, "<s>")}</s> \n \n<sprite name=\"icon_Trait_Cannibal\"> <color=#FFD201>{FollowerTrait.GetLocalizedTitle(FollowerTrait.TraitType.Cannibal)}</color> \n \n";
    }
    if (mealType == InventoryItem.ITEM_TYPE.MEAL_GRASS && DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.GrassEater))
      str = $"{str.Insert(31 /*0x1F*/, "<s>")}</s> \n \n<sprite name=\"icon_Trait_GrassEater\"> <color=#FFD201>{FollowerTrait.GetLocalizedTitle(FollowerTrait.TraitType.GrassEater)}";
    return str;
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
      default:
        return "Meals/Meal";
    }
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
  }
}
