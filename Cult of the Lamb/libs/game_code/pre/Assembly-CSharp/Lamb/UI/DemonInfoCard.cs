// Decompiled with JetBrains decompiler
// Type: Lamb.UI.DemonInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class DemonInfoCard : UIInfoCardBase<FollowerInfo>
{
  [SerializeField]
  private TextMeshProUGUI _followerName;
  [SerializeField]
  private SkeletonGraphic _followerSpine;
  [SerializeField]
  private SkeletonGraphic _demonSpine;
  [SerializeField]
  private TextMeshProUGUI _demonName;
  [SerializeField]
  private TMP_Text _description;
  [SerializeField]
  private GameObject _effectsContainer;
  [SerializeField]
  private TextMeshProUGUI _effectsText;
  [SerializeField]
  private TextMeshProUGUI _icon;
  [SerializeField]
  private TextMeshProUGUI _iconNumber;
  [SerializeField]
  private RectTransform _redOutline;
  private FollowerInfo _followerInfo;

  public FollowerInfo FollowerInfo => this._followerInfo;

  public RectTransform RedOutline => this._redOutline;

  public override void Configure(FollowerInfo followerInfo)
  {
    if (this._followerInfo == followerInfo)
      return;
    this._followerInfo = followerInfo;
    int demonType = DemonModel.GetDemonType(followerInfo);
    this._followerName.text = $"{followerInfo.Name} {followerInfo.XPLevel.ToNumeral()}";
    this._followerSpine.ConfigureFollower(followerInfo);
    this._icon.text = DemonModel.GetDemonIcon(demonType);
    this._iconNumber.text = $"+{followerInfo.XPLevel}";
    this._effectsContainer.SetActive(followerInfo.XPLevel > 1);
    this._effectsText.text = DemonModel.GetDemonUpgradeDescription(demonType);
    this._demonSpine.Skeleton.SetSkin(Interaction_DemonSummoner.DemonSkins[demonType] + (this._followerInfo.XPLevel > 1 ? "+" : ""));
    this._demonName.text = DemonModel.GetDemonName(demonType);
    this._description.text = DemonModel.GetDescription(demonType);
  }
}
