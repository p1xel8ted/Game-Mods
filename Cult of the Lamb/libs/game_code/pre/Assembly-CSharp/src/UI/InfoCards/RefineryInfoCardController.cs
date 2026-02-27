// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.RefineryInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI;
using UnityEngine.UI;

#nullable disable
namespace src.UI.InfoCards;

public class RefineryInfoCardController : 
  UIInfoCardController<RefineryInfoCard, InventoryItem.ITEM_TYPE>
{
  protected override bool IsSelectionValid(
    Selectable selectable,
    out InventoryItem.ITEM_TYPE showParam)
  {
    showParam = InventoryItem.ITEM_TYPE.NONE;
    RefineryItem component;
    if (!selectable.TryGetComponent<RefineryItem>(out component))
      return false;
    showParam = component.Type;
    return true;
  }
}
