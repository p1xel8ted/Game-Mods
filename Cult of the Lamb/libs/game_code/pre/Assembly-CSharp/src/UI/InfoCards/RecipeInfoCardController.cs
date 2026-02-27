// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.RecipeInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI;
using UnityEngine.UI;

#nullable disable
namespace src.UI.InfoCards;

public class RecipeInfoCardController : UIInfoCardController<RecipeInfoCard, InventoryItem.ITEM_TYPE>
{
  protected override bool IsSelectionValid(
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
