// Decompiled with JetBrains decompiler
// Type: Lamb.UI.RanchMenuInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class RanchMenuInfoCardController : UIInfoCardController<RanchInfoCard, RanchMenuItem>
{
  public override bool IsSelectionValid(Selectable selectable, out RanchMenuItem showParam)
  {
    showParam = (RanchMenuItem) null;
    RanchMenuItem component;
    if (!selectable.TryGetComponent<RanchMenuItem>(out component))
      return false;
    showParam = component;
    return showParam.AnimalInfo != null && showParam.AnimalInfo.Type != InventoryItem.ITEM_TYPE.NONE;
  }
}
