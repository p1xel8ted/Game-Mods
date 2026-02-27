// Decompiled with JetBrains decompiler
// Type: Lamb.UI.WeaponInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class WeaponInfoCard : UIInfoCardBase<EquipmentType>
{
  [SerializeField]
  private WeaponItem _weaponItem;
  [SerializeField]
  private TextMeshProUGUI _itemHeader;
  [SerializeField]
  private TextMeshProUGUI _itemLore;
  [SerializeField]
  private TextMeshProUGUI _itemDescription;
  [SerializeField]
  private TextMeshProUGUI _levelText;
  [SerializeField]
  private GameObject _statsHeader;
  [SerializeField]
  private GameObject _statsContainer;
  [SerializeField]
  private TextMeshProUGUI _damageText;
  [SerializeField]
  private TextMeshProUGUI _speedText;

  public override void Configure(EquipmentType equipmentType)
  {
    this._weaponItem.Configure(equipmentType);
    if (equipmentType != EquipmentType.None)
    {
      this._itemHeader.text = $"{LocalizationManager.GetTranslation($"UpgradeSystem/{equipmentType}/Name")} {DataManager.Instance.CurrentWeaponLevel.ToNumeral()}";
      this._itemLore.text = LocalizationManager.GetTranslation($"UpgradeSystem/{equipmentType}/Lore");
      this._itemDescription.text = LocalizationManager.GetTranslation($"UpgradeSystem/{equipmentType}/Description");
      this._statsHeader.SetActive(true);
      this._statsContainer.SetActive(true);
      this._levelText.text = DataManager.Instance.CurrentWeaponLevel.ToNumeral();
      this._damageText.text = $"{ScriptLocalization.UI_WeaponSelect.Damage}: <color=#FFD201>{(object) PlayerFarming.Instance.playerWeapon.GetAverageWeaponDamage(equipmentType, DataManager.Instance.CurrentWeaponLevel)}";
      this._speedText.text = $"{ScriptLocalization.UI_WeaponSelect.Speed}: <color=#FFD201>{(object) PlayerFarming.Instance.playerWeapon.GetWeaponSpeed(equipmentType)}";
    }
    else
    {
      this._itemHeader.text = LocalizationManager.GetTranslation("UI/PauseScreen/Player/NoWeapon");
      this._itemLore.gameObject.SetActive(false);
      this._itemDescription.text = LocalizationManager.GetTranslation("UI/PauseScreen/Player/NoWeapon/Description");
      this._levelText.text = "";
      this._statsHeader.SetActive(false);
      this._statsContainer.SetActive(false);
    }
  }
}
