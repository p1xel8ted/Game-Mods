// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.DoctrineInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public TextMeshProUGUI _itemHeader;
  [SerializeField]
  public TextMeshProUGUI _itemDescription;
  [SerializeField]
  public DoctrineFragmentsItem _doctrineFragmentsItem;

  public override void Configure(int pieces)
  {
    this._itemHeader.text = LocalizationManager.GetTranslation("UI/PauseScreen/Player/DoctrineFragments");
    if (DoctrineUpgradeSystem.TrySermonsStillAvailable())
      this._itemDescription.text = string.Format(LocalizationManager.GetTranslation("UI/PauseScreen/Player/DoctrineFragments/Description"), (object) pieces, (object) 3);
    else
      this._itemDescription.text = LocalizationManager.GetTranslation("UI/PauseScreen/Player/DoctrineFragments/Description/Max");
    this._doctrineFragmentsItem.Configure(pieces);
  }
}
