// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UpgradeInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UpgradeInfoCardController : UIInfoCardController<UpgradeInfoCard, UpgradeSystem.Type>
{
  public override bool IsSelectionValid(Selectable selectable, out UpgradeSystem.Type showParam)
  {
    showParam = UpgradeSystem.Type.Combat_ExtraHeart1;
    UpgradeShopItem component1;
    if (selectable.TryGetComponent<UpgradeShopItem>(out component1))
    {
      showParam = component1.Type;
      return true;
    }
    CrownAbilityItemBuyable component2;
    if (!selectable.TryGetComponent<CrownAbilityItemBuyable>(out component2))
      return false;
    showParam = component2.Type;
    return true;
  }
}
