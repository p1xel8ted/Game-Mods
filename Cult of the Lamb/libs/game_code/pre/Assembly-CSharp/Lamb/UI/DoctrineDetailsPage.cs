// Decompiled with JetBrains decompiler
// Type: Lamb.UI.DoctrineDetailsPage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class DoctrineDetailsPage : UISubmenuBase
{
  [Header("Category Info")]
  [SerializeField]
  private Image _unlockIcon;
  [SerializeField]
  private TextMeshProUGUI _unlockTitle;
  [SerializeField]
  private TextMeshProUGUI _unlockType;
  [SerializeField]
  private TextMeshProUGUI _unlockTypeIcon;
  [SerializeField]
  private TextMeshProUGUI _unlockDescription;

  public void UpdateDetails(DoctrineUpgradeSystem.DoctrineType type)
  {
    this._unlockTitle.text = DoctrineUpgradeSystem.GetLocalizedName(type).StripHtml();
    this._unlockDescription.text = DoctrineUpgradeSystem.GetLocalizedDescription(type).StripColourHtml();
    this._unlockIcon.sprite = DoctrineUpgradeSystem.GetIcon(type);
    this._unlockTypeIcon.text = DoctrineUpgradeSystem.GetDoctrineUnlockIcon(type);
    this._unlockType.text = DoctrineUpgradeSystem.GetDoctrineUnlockString(type);
  }
}
