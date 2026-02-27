// Decompiled with JetBrains decompiler
// Type: Lamb.UI.RitualInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI.Rituals;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class RitualInfoCardController : UIInfoCardController<RitualInfoCard, UpgradeSystem.Type>
{
  protected override bool IsSelectionValid(Selectable selectable, out UpgradeSystem.Type showParam)
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

  protected override UpgradeSystem.Type DefaultShowParam() => UpgradeSystem.Type.Ritual_Blank;
}
