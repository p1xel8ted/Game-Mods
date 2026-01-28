// Decompiled with JetBrains decompiler
// Type: FollowerTask_PubPorter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class FollowerTask_PubPorter(int logisticsStructure, int rootStructure, int targetStructure) : 
  FollowerTask_LogisticsPorter(logisticsStructure, rootStructure, targetStructure)
{
  public override List<InventoryItem> CollectItemsFromStructure(StructureBrain structureBrain)
  {
    List<InventoryItem> resultingIngredients;
    CookingData.GetDrinksFromIngredients(structureBrain.Data.Inventory, out resultingIngredients);
    for (int index = 0; index < resultingIngredients.Count; ++index)
      structureBrain.RemoveItems((InventoryItem.ITEM_TYPE) resultingIngredients[index].type, resultingIngredients[index].quantity);
    return resultingIngredients;
  }

  public override List<InventoryItem> DepositItems(StructureBrain structureBrain)
  {
    List<InventoryItem> resultingIngredients = new List<InventoryItem>();
    List<InventoryItem> drinksFromIngredients = CookingData.GetDrinksFromIngredients(this.heldItems, out resultingIngredients);
    List<InventoryItem> inventoryItemList = new List<InventoryItem>();
    for (int index = 0; index < drinksFromIngredients.Count; ++index)
    {
      List<InventoryItem> recipeSimplified = CookingData.GetRecipeSimplified((InventoryItem.ITEM_TYPE) drinksFromIngredients[index].type);
      if (structureBrain.Data.QueuedMeals.Count < ((Structures_Pub) structureBrain).MaxQueue)
      {
        structureBrain.Data.QueuedMeals.Add(new Interaction_Kitchen.QueuedMeal()
        {
          MealType = (InventoryItem.ITEM_TYPE) drinksFromIngredients[index].type,
          CookingDuration = CookingData.GetMealCookDuration((InventoryItem.ITEM_TYPE) drinksFromIngredients[index].type),
          CookedTime = 0.0f,
          Ingredients = recipeSimplified
        });
        foreach (Interaction_Pub pub in Interaction_Pub.Pubs)
        {
          if (pub.Structure.Brain == structureBrain)
          {
            pub.UpdateCurrentDrink();
            break;
          }
        }
      }
      else
        inventoryItemList.AddRange((IEnumerable<InventoryItem>) recipeSimplified);
    }
    return inventoryItemList;
  }

  public override void Loop(List<InventoryItem> leftovers)
  {
    List<StructureBrain> structuresOfType = StructureManager.GetAllStructuresOfType(FollowerLocation.Base, StructureBrain.TYPES.FARM_STATION_II);
    bool flag = false;
    foreach (StructureBrain structureBrain in structuresOfType)
    {
      if (CookingData.CanMakeDrinks(structureBrain.Data.Inventory))
      {
        this.rootstructure = structureBrain.Data.ID;
        flag = true;
        break;
      }
    }
    if (flag)
    {
      this.targetState = FollowerTask_LogisticsPorter.TargetState.Root;
      this.ClearDestination();
      this.SetState(FollowerTaskState.GoingTo);
    }
    else
      this.End();
  }
}
