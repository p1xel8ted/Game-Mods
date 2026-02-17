// Decompiled with JetBrains decompiler
// Type: Lamb.UI.DoctrineDetailsPage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class DoctrineDetailsPage : UISubmenuBase
{
  [Header("Category Info")]
  [SerializeField]
  public Image _unlockIcon;
  [SerializeField]
  public TextMeshProUGUI _unlockTitle;
  [SerializeField]
  public TextMeshProUGUI _unlockType;
  [SerializeField]
  public TextMeshProUGUI _unlockTypeIcon;
  [SerializeField]
  public TextMeshProUGUI _unlockDescription;

  public void UpdateDetails(DoctrineUpgradeSystem.DoctrineType type)
  {
    this._unlockTitle.text = DoctrineUpgradeSystem.GetLocalizedName(type).StripHtml();
    this._unlockDescription.text = DoctrineUpgradeSystem.GetLocalizedDescription(type).StripColourHtml();
    this._unlockIcon.sprite = DoctrineUpgradeSystem.GetIcon(type);
    this._unlockTypeIcon.text = DoctrineUpgradeSystem.GetDoctrineUnlockIcon(type);
    this._unlockType.text = DoctrineUpgradeSystem.GetDoctrineUnlockString(type);
  }
}
