// Decompiled with JetBrains decompiler
// Type: Lamb.UI.ItemInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class ItemInfoCardController : UIInfoCardController<ItemInfoCard, InventoryItem.ITEM_TYPE>
{
  public override bool IsSelectionValid(
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
