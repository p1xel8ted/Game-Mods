// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.DoctrineInfoCard
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

public class DoctrineInfoCard : UIInfoCardBase<int>
{
  [SerializeField]
  private TextMeshProUGUI _itemHeader;
  [SerializeField]
  private TextMeshProUGUI _itemDescription;
  [SerializeField]
  private DoctrineFragmentsItem _doctrineFragmentsItem;

  public override void Configure(int pieces)
  {
    this._itemHeader.text = LocalizationManager.GetTranslation("UI/PauseScreen/Player/DoctrineFragments");
    this._itemDescription.text = string.Format(LocalizationManager.GetTranslation("UI/PauseScreen/Player/DoctrineFragments/Description"), (object) pieces, (object) 3);
    this._doctrineFragmentsItem.Configure(pieces);
  }
}
