// Decompiled with JetBrains decompiler
// Type: Lamb.UI.IngredientInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class IngredientInfoCardController : 
  UIInfoCardController<IngredientInfoCard, InventoryItem.ITEM_TYPE>
{
  public override bool IsSelectionValid(
    Selectable selectable,
    out InventoryItem.ITEM_TYPE showParam)
  {
    showParam = InventoryItem.ITEM_TYPE.NONE;
    IngredientItem component;
    if (!selectable.TryGetComponent<IngredientItem>(out component))
      return false;
    showParam = component.Type;
    return true;
  }
}
