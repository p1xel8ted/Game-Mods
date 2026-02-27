// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.TalismanInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using Lamb.UI.PauseDetails;
using TMPro;
using UnityEngine;

#nullable disable
namespace src.UI.InfoCards;

public class TalismanInfoCard : UIInfoCardBase<int>
{
  [SerializeField]
  private TextMeshProUGUI _itemHeader;
  [SerializeField]
  private TextMeshProUGUI _itemDescription;
  [SerializeField]
  private TalismanPiecesItem _talismanPiecesItem;

  public override void Configure(int pieces)
  {
    this._itemHeader.text = LocalizationManager.GetTranslation("UI/PauseScreen/Player/TalismanPieces");
    this._itemDescription.text = string.Format(LocalizationManager.GetTranslation("UI/PauseScreen/Player/TalismanPieces/Description"), (object) pieces, (object) 4);
    this._talismanPiecesItem.Configure(pieces);
  }
}
