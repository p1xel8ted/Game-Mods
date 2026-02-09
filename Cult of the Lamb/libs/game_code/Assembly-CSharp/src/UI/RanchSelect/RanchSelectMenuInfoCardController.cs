// Decompiled with JetBrains decompiler
// Type: src.UI.RanchSelect.RanchSelectMenuInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using UnityEngine.UI;

#nullable disable
namespace src.UI.RanchSelect;

public class RanchSelectMenuInfoCardController : 
  UIInfoCardController<RanchSelectMenuInfoCard, RanchSelectItem>
{
  public override bool IsSelectionValid(Selectable selectable, out RanchSelectItem showParam)
  {
    showParam = (RanchSelectItem) null;
    RanchMenuItem component;
    if (!selectable.TryGetComponent<RanchMenuItem>(out component))
      return false;
    showParam = (RanchSelectItem) component;
    return showParam.AnimalInfo.Type != InventoryItem.ITEM_TYPE.NONE;
  }
}
