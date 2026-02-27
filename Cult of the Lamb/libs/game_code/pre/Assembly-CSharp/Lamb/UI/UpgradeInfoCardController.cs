// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UpgradeInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UpgradeInfoCardController : UIInfoCardController<UpgradeInfoCard, UpgradeSystem.Type>
{
  protected override bool IsSelectionValid(Selectable selectable, out UpgradeSystem.Type showParam)
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
