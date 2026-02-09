// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.TalismanInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using Lamb.UI.PauseDetails;
using UnityEngine.UI;

#nullable disable
namespace src.UI.InfoCards;

public class TalismanInfoCardController : UIInfoCardController<TalismanInfoCard, int>
{
  public override bool IsSelectionValid(Selectable selectable, out int showParam)
  {
    showParam = 0;
    if (!selectable.TryGetComponent<TalismanPiecesItem>(out TalismanPiecesItem _) || Inventory.KeyPieces + Inventory.TempleKeys <= 0 && !DataManager.Instance.HadFirstTempleKey)
      return false;
    showParam = Inventory.KeyPieces;
    return true;
  }

  public override int DefaultShowParam() => -1;
}
