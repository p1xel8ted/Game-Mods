// Decompiled with JetBrains decompiler
// Type: FollowerTask_KitchenPorter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.KitchenMenu;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerTask_KitchenPorter(
  int logisticsStructure,
  int rootStructure,
  int targetStructure) : FollowerTask_LogisticsPorter(logisticsStructure, rootStructure, targetStructure)
{
  public override int MaxItems => UIFollowerKitchenMenuController.kMaxItems;

  public override List<InventoryItem> CollectItemsFromStructure(StructureBrain structureBrain)
  {
    List<InventoryItem> ingredients = structureBrain.Data.Inventory;
    if (structureBrain is Structures_Scarecrow structuresScarecrow)
    {
      if (structuresScarecrow.HasBird)
      {
        int num = Random.Range(1, 3);
        for (int index = 0; index < num * 2; ++index)
          ingredients.Add(new InventoryItem(InventoryItem.ITEM_TYPE.MEAT));
        structuresScarecrow.EmptyTrap();
      }
    }
    else if (structureBrain.Data.Type == StructureBrain.TYPES.WOLF_TRAP && structureBrain.Data.HasBird)
    {
      ingredients = new List<InventoryItem>();
      int num = Random.Range(2, 6);
      for (int index = 0; index < num; ++index)
        ingredients.Add(new InventoryItem(InventoryItem.ITEM_TYPE.MEAT));
      structureBrain.Data.HasBird = false;
      structureBrain.Data.Inventory.Clear();
      foreach (Interaction_WolfTrap trap in Interaction_WolfTrap.Traps)
        trap.UpdateVisuals();
    }
    List<InventoryItem> resultingIngredients;
    CookingData.GetGoodOrBetterMealsFromIngredients(ingredients, out resultingIngredients);
    for (int index = 0; index < resultingIngredients.Count; ++index)
      structureBrain.RemoveItems((InventoryItem.ITEM_TYPE) resultingIngredients[index].type, resultingIngredients[index].quantity);
    return resultingIngredients;
  }

  public override List<InventoryItem> DepositItems(StructureBrain structureBrain)
  {
    List<InventoryItem> resultingIngredients = new List<InventoryItem>();
    List<InventoryItem> mealsFromIngredients = CookingData.GetGoodOrBetterMealsFromIngredients(this.heldItems, out resultingIngredients);
    List<InventoryItem> inventoryItemList = new List<InventoryItem>();
    for (int index = 0; index < mealsFromIngredients.Count; ++index)
    {
      List<InventoryItem> recipeSimplified = CookingData.GetRecipeSimplified((InventoryItem.ITEM_TYPE) mealsFromIngredients[index].type);
      if (structureBrain.Data.QueuedMeals.Count < this.MaxItems)
      {
        structureBrain.Data.QueuedMeals.Add(new Interaction_Kitchen.QueuedMeal()
        {
          MealType = (InventoryItem.ITEM_TYPE) mealsFromIngredients[index].type,
          CookingDuration = CookingData.GetMealCookDuration((InventoryItem.ITEM_TYPE) mealsFromIngredients[index].type),
          CookedTime = 0.0f,
          Ingredients = recipeSimplified
        });
        foreach (Interaction_FollowerKitchen followerKitchen in Interaction_FollowerKitchen.FollowerKitchens)
        {
          if (followerKitchen.structure.Brain == structureBrain)
          {
            followerKitchen.UpdateCurrentMeal();
            break;
          }
        }
      }
      else
        inventoryItemList.AddRange((IEnumerable<InventoryItem>) recipeSimplified);
    }
    return inventoryItemList;
  }

  public override void Loop(List<InventoryItem> leftovers) => this.End();
}
