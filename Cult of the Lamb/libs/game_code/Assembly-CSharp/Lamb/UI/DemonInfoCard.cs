// Decompiled with JetBrains decompiler
// Type: Lamb.UI.DemonInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Spine.Unity;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class DemonInfoCard : UIInfoCardBase<FollowerInfo>
{
  [SerializeField]
  public TextMeshProUGUI _followerName;
  [SerializeField]
  public SkeletonGraphic _followerSpine;
  [SerializeField]
  public SkeletonGraphic _demonSpine;
  [SerializeField]
  public TextMeshProUGUI _demonName;
  [SerializeField]
  public TMP_Text _description;
  [SerializeField]
  public GameObject _effectsContainer;
  [SerializeField]
  public TextMeshProUGUI _effectsText;
  [SerializeField]
  public TextMeshProUGUI _icon;
  [SerializeField]
  public TextMeshProUGUI _iconNumber;
  [SerializeField]
  public RectTransform _redOutline;
  public FollowerInfo _followerInfo;

  public FollowerInfo FollowerInfo => this._followerInfo;

  public RectTransform RedOutline => this._redOutline;

  public override void Configure(FollowerInfo followerInfo)
  {
    if (this._followerInfo == followerInfo)
      return;
    this._followerInfo = followerInfo;
    int demonType = DemonModel.GetDemonType(followerInfo);
    this._followerName.text = followerInfo.GetNameFormatted();
    this._followerSpine.ConfigureFollower(followerInfo);
    this._icon.text = DemonModel.GetDemonIcon(demonType);
    int demonLevel = followerInfo.GetDemonLevel();
    this._iconNumber.text = $"+{demonLevel}";
    this._effectsContainer.SetActive(demonLevel > 1);
    this._effectsText.text = DemonModel.GetDemonUpgradeDescription(demonType);
    this._demonSpine.Skeleton.SetSkin(Interaction_DemonSummoner.DemonSkins[demonType] + (demonLevel <= 1 || demonType >= 6 ? "" : "+"));
    this._demonName.text = DemonModel.GetDemonName(demonType);
    this._description.text = DemonModel.GetDescription(demonType);
    if (this._followerInfo.Necklace != InventoryItem.ITEM_TYPE.Necklace_Demonic)
      return;
    TMP_Text description = this._description;
    description.text = $"{description.text}<br><sprite name=\"icon_GoodTrait\"> {string.Format(LocalizationManager.GetTranslation("UI/DemonScreen/ReasonsForLevel/DemonicNecklace"), (object) this._followerInfo.Name)}\n";
  }
}
