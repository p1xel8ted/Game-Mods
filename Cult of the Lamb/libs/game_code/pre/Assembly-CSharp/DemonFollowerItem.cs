// Decompiled with JetBrains decompiler
// Type: DemonFollowerItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using TMPro;
using UnityEngine;

#nullable disable
public class DemonFollowerItem : FollowerSelectItem
{
  [SerializeField]
  private TextMeshProUGUI _followerName;
  [SerializeField]
  private SkeletonGraphic _demonSpine;
  [SerializeField]
  private TextMeshProUGUI _demonIcon;
  [SerializeField]
  private TextMeshProUGUI _iconNumber;
  [SerializeField]
  private TextMeshProUGUI _demonName;
  [SerializeField]
  private TextMeshProUGUI _demonDescription;
  [SerializeField]
  private TextMeshProUGUI _effectsText;
  [SerializeField]
  private GameObject _effectsContainer;

  protected override void ConfigureImpl()
  {
    this._followerName.text = this._followerInfo.GetNameFormatted();
    int demonType = DemonModel.GetDemonType(this._followerInfo);
    this._demonSpine.Skeleton.SetSkin(Interaction_DemonSummoner.DemonSkins[demonType] + (this._followerInfo.XPLevel > 1 ? "+" : ""));
    this._demonIcon.text = DemonModel.GetDemonIcon(demonType);
    this._iconNumber.text = $"+{this._followerInfo.XPLevel}";
    this._demonName.text = DemonModel.GetDemonName(demonType);
    this._demonDescription.text = DemonModel.GetDescription(demonType);
    this._effectsContainer.SetActive(this._followerInfo.XPLevel > 1);
    this._effectsText.text = DemonModel.GetDemonUpgradeDescription(demonType);
  }
}
