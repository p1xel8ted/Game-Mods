// Decompiled with JetBrains decompiler
// Type: UIDoctrineChoiceInfoBox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private Image _icon;
  [SerializeField]
  private TextMeshProUGUI _unlockName;
  [SerializeField]
  private TextMeshProUGUI _unlockDescription;
  [SerializeField]
  private TextMeshProUGUI _unlockType;
  [SerializeField]
  private TextMeshProUGUI _unlockTypeIcon;

  protected override void ConfigureImpl(DoctrineResponse info)
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
