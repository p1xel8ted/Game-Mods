// Decompiled with JetBrains decompiler
// Type: Lamb.UI.ItemInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class ItemInfoCardController : UIInfoCardController<ItemInfoCard, InventoryItem.ITEM_TYPE>
{
  protected override bool IsSelectionValid(
    Selectable selectable,
    out InventoryItem.ITEM_TYPE showParam)
  {
    showParam = InventoryItem.ITEM_TYPE.NONE;
    GenericInventoryItem component;
    if (!selectable.TryGetComponent<GenericInventoryItem>(out component))
      return false;
    showParam = component.Type;
    return true;
  }
}
