// Decompiled with JetBrains decompiler
// Type: Lamb.UI.CultUpgradeInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.Rituals;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class CultUpgradeInfoCardController : 
  UIInfoCardController<CultUpgradeInfoCard, CultUpgradeData.TYPE>
{
  public override bool IsSelectionValid(Selectable selectable, out CultUpgradeData.TYPE showParam)
  {
    showParam = CultUpgradeData.TYPE.None;
    CultUpgradeItem component;
    if (!selectable.TryGetComponent<CultUpgradeItem>(out component) || component.selectionAlwaysInvalid)
      return false;
    showParam = component.CultUpgradeType;
    return true;
  }
}
