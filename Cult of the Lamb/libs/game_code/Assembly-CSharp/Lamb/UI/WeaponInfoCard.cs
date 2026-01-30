// Decompiled with JetBrains decompiler
// Type: Lamb.UI.WeaponInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using src.UINavigator;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class WeaponInfoCard : UIInfoCardBase<EquipmentType>
{
  [SerializeField]
  public WeaponItem _weaponItem;
  [SerializeField]
  public TextMeshProUGUI _itemHeader;
  [SerializeField]
  public TextMeshProUGUI _itemLore;
  [SerializeField]
  public TextMeshProUGUI _itemDescription;
  [SerializeField]
  public TextMeshProUGUI _levelText;
  [SerializeField]
  public GameObject _statsHeader;
  [SerializeField]
  public GameObject _statsContainer;
  [SerializeField]
  public TextMeshProUGUI _damageText;
  [SerializeField]
  public TextMeshProUGUI _speedText;

  public override void Configure(EquipmentType equipmentType)
  {
    PlayerFarming inputOnlyFromPlayer = MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer;
    this._weaponItem.Configure(equipmentType);
    if (equipmentType != EquipmentType.None)
    {
      this._itemHeader.text = $"{EquipmentManager.GetEquipmentData(equipmentType).GetLocalisedTitle()} {inputOnlyFromPlayer.currentWeaponLevel.ToNumeral()}";
      this._itemLore.text = EquipmentManager.GetEquipmentData(equipmentType).GetLocalisedLore();
      this._itemDescription.text = EquipmentManager.GetEquipmentData(equipmentType).GetLocalisedDescription();
      this._statsHeader.SetActive(true);
      this._statsContainer.SetActive(true);
      this._levelText.text = inputOnlyFromPlayer.currentWeaponLevel.ToNumeral();
      TextMeshProUGUI damageText = this._damageText;
      string damage = ScriptLocalization.UI_WeaponSelect.Damage;
      float num = PlayerFarming.Instance.playerWeapon.GetAverageWeaponDamage(equipmentType, inputOnlyFromPlayer.currentWeaponLevel);
      string str1 = num.ToString();
      string str2 = $"{damage}: <color=#FFD201>{str1}";
      damageText.text = str2;
      TextMeshProUGUI speedText = this._speedText;
      string speed = ScriptLocalization.UI_WeaponSelect.Speed;
      num = PlayerFarming.Instance.playerWeapon.GetWeaponSpeed(equipmentType);
      string str3 = num.ToString();
      string str4 = $"{speed}: <color=#FFD201>{str3}";
      speedText.text = str4;
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
