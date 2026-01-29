// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.FleeceInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using UnityEngine.UI;

#nullable disable
namespace src.UI.InfoCards;

public class FleeceInfoCardController : UIInfoCardController<FleeceInfoCard, int>
{
  public override bool IsSelectionValid(Selectable selectable, out int showParam)
  {
    showParam = 0;
    if (selectable.TryGetComponent<FleeceItem>(out FleeceItem _))
    {
      showParam = DataManager.Instance.PlayerFleece;
      return true;
    }
    FleeceItemBuyable component;
    if (!selectable.TryGetComponent<FleeceItemBuyable>(out component))
      return false;
    showParam = component.Fleece;
    return true;
  }

  public override int DefaultShowParam() => -1;
}
