// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.LoreInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using Lamb.UI.PauseDetails;
using UnityEngine.UI;

#nullable disable
namespace src.UI.InfoCards;

public class LoreInfoCardController : UIInfoCardController<LoreInfoCard, int>
{
  public override bool IsSelectionValid(Selectable selectable, out int showParam)
  {
    LoreItem component;
    if (selectable.TryGetComponent<LoreItem>(out component))
    {
      showParam = component.LoreId;
      if (LoreSystem.LoreAvailable(showParam))
        return true;
    }
    showParam = -1;
    return false;
  }

  public override int DefaultShowParam() => -1;
}
