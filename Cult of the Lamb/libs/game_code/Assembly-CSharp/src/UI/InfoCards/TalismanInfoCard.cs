// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.TalismanInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public TextMeshProUGUI _itemHeader;
  [SerializeField]
  public TextMeshProUGUI _itemDescription;
  [SerializeField]
  public TalismanPiecesItem _talismanPiecesItem;

  public override void Configure(int pieces)
  {
    this._itemHeader.text = LocalizationManager.GetTranslation("UI/PauseScreen/Player/TalismanPieces");
    this._itemDescription.text = string.Format(LocalizationManager.GetTranslation("UI/PauseScreen/Player/TalismanPieces/Description"), (object) pieces, (object) 4);
    this._talismanPiecesItem.Configure(pieces);
  }
}
