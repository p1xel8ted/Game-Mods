// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.CrownAbilityInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI;
using UnityEngine.UI;

#nullable disable
namespace src.UI.InfoCards;

public class CrownAbilityInfoCardController : 
  UIInfoCardController<CrownAbilityInfoCard, UpgradeSystem.Type>
{
  protected override bool IsSelectionValid(Selectable selectable, out UpgradeSystem.Type showParam)
  {
    showParam = UpgradeSystem.Type.Combat_ExtraHeart1;
    CrownAbilityItem component1;
    if (selectable.TryGetComponent<CrownAbilityItem>(out component1))
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
