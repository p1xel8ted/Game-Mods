// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.RecipeInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using UnityEngine.UI;

#nullable disable
namespace src.UI.InfoCards;

public class RecipeInfoCardController : UIInfoCardController<RecipeInfoCard, InventoryItem.ITEM_TYPE>
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
