// Decompiled with JetBrains decompiler
// Type: DrinkRecipeInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using UnityEngine.UI;

#nullable disable
public class DrinkRecipeInfoCardController : 
  UIInfoCardController<DrinkRecipeInfoCard, InventoryItem.ITEM_TYPE>
{
  public override bool IsSelectionValid(
    Selectable selectable,
    out InventoryItem.ITEM_TYPE showParam)
  {
    showParam = InventoryItem.ITEM_TYPE.NONE;
    RecipeItem component1;
    if (selectable.TryGetComponent<RecipeItem>(out component1) && CookingData.HasRecipeDiscovered(component1.Type))
    {
      showParam = component1.Type;
      return true;
    }
    FinalizeRecipeButton component2;
    if (!selectable.TryGetComponent<FinalizeRecipeButton>(out component2))
      return false;
    showParam = component2.Recipe;
    return showParam != 0;
  }
}
