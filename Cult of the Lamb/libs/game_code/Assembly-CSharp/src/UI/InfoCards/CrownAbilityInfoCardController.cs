// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.CrownAbilityInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using UnityEngine.UI;

#nullable disable
namespace src.UI.InfoCards;

public class CrownAbilityInfoCardController : 
  UIInfoCardController<CrownAbilityInfoCard, UpgradeSystem.Type>
{
  public override bool IsSelectionValid(Selectable selectable, out UpgradeSystem.Type showParam)
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
