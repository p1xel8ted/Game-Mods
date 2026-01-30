// Decompiled with JetBrains decompiler
// Type: Lamb.UI.RanchAssignMenuInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
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
