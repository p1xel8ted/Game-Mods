// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.FleeceInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI;
using UnityEngine.UI;

#nullable disable
namespace src.UI.InfoCards;

public class FleeceInfoCardController : UIInfoCardController<FleeceInfoCard, int>
{
  protected override bool IsSelectionValid(Selectable selectable, out int showParam)
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

  protected override int DefaultShowParam() => -1;
}
