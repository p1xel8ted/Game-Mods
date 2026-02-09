// Decompiled with JetBrains decompiler
// Type: Lamb.UI.CultUpgradeInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
