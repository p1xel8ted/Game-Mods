// Decompiled with JetBrains decompiler
// Type: UIDoctrineChoiceInfoBox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using MMTools;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class UIDoctrineChoiceInfoBox : 
  UIChoiceInfoCard<DoctrineResponse>,
  ISelectHandler,
  IEventSystemHandler,
  IDeselectHandler
{
  [SerializeField]
  public Image _icon;
  [SerializeField]
  public TextMeshProUGUI _unlockName;
  [SerializeField]
  public TextMeshProUGUI _unlockDescription;
  [SerializeField]
  public TextMeshProUGUI _unlockType;
  [SerializeField]
  public TextMeshProUGUI _unlockTypeIcon;

  public override void ConfigureImpl(DoctrineResponse info)
  {
    DoctrineUpgradeSystem.DoctrineType sermonReward = DoctrineUpgradeSystem.GetSermonReward(info.SermonCategory, info.RewardLevel, info.isFirstChoice);
    this._icon.sprite = DoctrineUpgradeSystem.GetIcon(sermonReward);
    this._unlockName.text = DoctrineUpgradeSystem.GetLocalizedName(sermonReward);
    this._unlockDescription.text = DoctrineUpgradeSystem.GetLocalizedDescription(sermonReward);
    this._unlockType.text = DoctrineUpgradeSystem.GetDoctrineUnlockString(sermonReward);
    if (string.IsNullOrEmpty(this._unlockType.text))
      this._unlockType.gameObject.SetActive(false);
    this._unlockTypeIcon.text = DoctrineUpgradeSystem.GetDoctrineUnlockIcon(sermonReward);
  }
}
