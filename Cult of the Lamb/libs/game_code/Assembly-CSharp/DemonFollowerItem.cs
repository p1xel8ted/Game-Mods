// Decompiled with JetBrains decompiler
// Type: DemonFollowerItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Spine.Unity;
using TMPro;
using UnityEngine;

#nullable disable
public class DemonFollowerItem : FollowerSelectItem
{
  [SerializeField]
  public TextMeshProUGUI _followerName;
  [SerializeField]
  public SkeletonGraphic _demonSpine;
  [SerializeField]
  public TextMeshProUGUI _demonIcon;
  [SerializeField]
  public TextMeshProUGUI _iconNumber;
  [SerializeField]
  public TextMeshProUGUI _demonName;
  [SerializeField]
  public TextMeshProUGUI _demonDescription;
  [SerializeField]
  public TextMeshProUGUI _effectsText;
  [SerializeField]
  public GameObject _effectsContainer;

  public override void ConfigureImpl()
  {
    this._followerName.text = this._followerInfo.GetNameFormatted();
    int demonType = DemonModel.GetDemonType(this._followerInfo);
    string demonSkin = Interaction_DemonSummoner.DemonSkins[demonType];
    int demonLevel = this._followerInfo.GetDemonLevel();
    this._demonSpine.Skeleton.SetSkin(demonSkin + (demonLevel <= 1 || demonType >= 6 ? "" : "+"));
    this._demonIcon.text = DemonModel.GetDemonIcon(demonType);
    this._iconNumber.text = $"+{demonLevel}";
    this._demonName.text = DemonModel.GetDemonName(demonType);
    this._demonDescription.text = DemonModel.GetDescription(demonType);
    this._effectsContainer.SetActive(demonLevel > 1);
    this._effectsText.text = DemonModel.GetDemonUpgradeDescription(demonType);
    if (this._followerInfo.Necklace != InventoryItem.ITEM_TYPE.Necklace_Demonic)
      return;
    TextMeshProUGUI demonDescription = this._demonDescription;
    demonDescription.text = $"{demonDescription.text}<br><sprite name=\"icon_GoodTrait\"><sprite name=\"icon_GoodTrait\"> {string.Format(LocalizationManager.GetTranslation("UI/DemonScreen/ReasonsForLevel/DemonicNecklace"), (object) this._followerInfo.Name)}\n";
  }
}
