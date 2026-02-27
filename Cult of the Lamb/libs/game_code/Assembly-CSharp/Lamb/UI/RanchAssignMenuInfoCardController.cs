// Decompiled with JetBrains decompiler
// Type: Lamb.UI.RanchAssignMenuInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class RanchAssignMenuInfoCardController : 
  UIInfoCardController<RanchAssignInfoCard, RanchAssignMenuItem>
{
  public override bool IsSelectionValid(Selectable selectable, out RanchAssignMenuItem showParam)
  {
    showParam = (RanchAssignMenuItem) null;
    RanchAssignMenuItem component;
    if (!selectable.TryGetComponent<RanchAssignMenuItem>(out component))
      return false;
    showParam = component;
    return showParam.AnimalType != InventoryItem.ITEM_TYPE.NONE;
  }
}
