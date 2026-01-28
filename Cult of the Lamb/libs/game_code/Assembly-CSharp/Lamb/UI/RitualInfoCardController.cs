// Decompiled with JetBrains decompiler
// Type: Lamb.UI.RitualInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.Rituals;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class RitualInfoCardController : UIInfoCardController<RitualInfoCard, UpgradeSystem.Type>
{
  public override bool IsSelectionValid(Selectable selectable, out UpgradeSystem.Type showParam)
  {
    showParam = UpgradeSystem.Type.Combat_ExtraHeart1;
    RitualItem component1;
    if (selectable.TryGetComponent<RitualItem>(out component1))
    {
      if (!component1.Locked)
      {
        showParam = component1.RitualType;
        return true;
      }
    }
    else
    {
      NotificationDynamicRitual component2;
      if (selectable.TryGetComponent<NotificationDynamicRitual>(out component2))
      {
        showParam = component2.Ritual;
        return true;
      }
    }
    return false;
  }

  public override UpgradeSystem.Type DefaultShowParam() => UpgradeSystem.Type.Ritual_Blank;

  public void ShakeLimitedTimeText() => this.CurrentCard.ShakeLimitedTimeText();
}
