// Decompiled with JetBrains decompiler
// Type: Lamb.UI.RanchAssignMenuInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
