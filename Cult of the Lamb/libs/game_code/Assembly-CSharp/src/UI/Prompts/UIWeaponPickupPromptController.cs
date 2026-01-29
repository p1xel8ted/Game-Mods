// Decompiled with JetBrains decompiler
// Type: src.UI.Prompts.UIWeaponPickupPromptController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using TMPro;
using UnityEngine;

#nullable disable
namespace src.UI.Prompts;

public class UIWeaponPickupPromptController : UIPromptBase
{
  [Header("Content")]
  [SerializeField]
  public InfoCardOutlineRenderer _outlineRenderer;
  [SerializeField]
  public TextMeshProUGUI _title;
  [SerializeField]
  public TextMeshProUGUI _lore;
  [SerializeField]
  public TextMeshProUGUI _description;
  [Header("Stats")]
  [SerializeField]
  public TextMeshProUGUI _damageText;
  [SerializeField]
  public TextMeshProUGUI _speedText;
  [Header("Stats")]
  [SerializeField]
  public TextMeshProUGUI _charge;
  [SerializeField]
  public GameObject _fragileIcon;
  public EquipmentType _weaponType;
  public RelicType _relicType;
  public float _damage;
  public float _speed;
  public int _weaponLevel;
  public PlayerFarming _playerFarming;

  public bool _isWeapon => this._weaponType < EquipmentType.Tentacles;

  public void Show(
    PlayerFarming playerFarming,
    EquipmentType weaponType,
    float damage,
    float speed,
    int weaponLevel,
    bool instant = false)
  {
    this._playerFarming = playerFarming;
    this.Show(instant);
    this._weaponType = weaponType;
    this._damage = damage;
    this._speed = speed;
    this._weaponLevel = weaponLevel;
    this._damage = Mathf.Round(damage * 100f) / 100f;
    this._damageText.gameObject.SetActive(this._isWeapon);
    this._speedText.gameObject.SetActive(this._isWeapon);
    if (this._isWeapon)
      this._outlineRenderer.BadgeVariant = 6;
    else
      this._outlineRenderer.BadgeVariant = 7;
  }

  public void Show(PlayerFarming playerFarming, RelicType relicType, bool instant = false)
  {
    this._playerFarming = playerFarming;
    this.Show(instant);
    this._relicType = relicType;
    this._weaponType = EquipmentType.None;
    this._damageText.gameObject.SetActive(false);
    this._speedText.gameObject.SetActive(false);
    this._outlineRenderer.BadgeVariant = 7;
  }

  public override void Localize()
  {
    if (this._relicType == RelicType.None)
    {
      this._title.text = $"{EquipmentManager.GetEquipmentData(this._weaponType).GetLocalisedTitle()} {(this._weaponLevel > 0 ? this._weaponLevel.ToNumeral() : "")}";
      this._lore.text = EquipmentManager.GetEquipmentData(this._weaponType).GetLocalisedLore();
      this._description.text = EquipmentManager.GetEquipmentData(this._weaponType).GetLocalisedDescription();
    }
    else
    {
      this._title.text = RelicData.GetTitleLocalisation(this._relicType);
      this._lore.text = RelicData.GetLoreLocalization(this._relicType);
      this._description.text = RelicData.GetDescriptionLocalisation(this._relicType);
      RelicData relicData = EquipmentManager.GetRelicData(this._relicType);
      this._fragileIcon.gameObject.SetActive(relicData.InteractionType == RelicInteractionType.Fragile || TrinketManager.AreRelicsFragile(this.playerFarming));
      RelicChargeCategory chargeCategory = RelicData.GetChargeCategory(relicData);
      this._charge.gameObject.SetActive(true);
      if (relicData.InteractionType == RelicInteractionType.Fragile || TrinketManager.AreRelicsFragile(this.playerFarming))
        this._charge.text = ScriptLocalization.UI.Fragile;
      else
        this._charge.text = $"{LocalizationManager.GetTranslation("UI/Charge")} <b>{RelicData.GetChargeCategoryColor(chargeCategory)}{LocalizationManager.GetTranslation($"UI/{chargeCategory}")}";
    }
    if (!this._isWeapon)
      return;
    float averageWeaponDamage = this._playerFarming.playerWeapon.GetAverageWeaponDamage(this._playerFarming.currentWeapon, this._playerFarming.currentWeaponLevel);
    float weaponSpeed = this._playerFarming.playerWeapon.GetWeaponSpeed(this._playerFarming.currentWeapon);
    string damage = ScriptLocalization.UI_WeaponSelect.Damage;
    string str1 = "";
    string str2 = "<color=#F5EDD5>";
    if ((double) averageWeaponDamage > (double) this._damage)
    {
      str1 = "<sprite name=\"icon_FaithDown\">";
      str2 = "<color=#FF1C1C>";
    }
    else if ((double) averageWeaponDamage < (double) this._damage)
    {
      str1 = "<sprite name=\"icon_FaithUp\">";
      str2 = "<color=#2DFF1C>";
    }
    string speed = ScriptLocalization.UI_WeaponSelect.Speed;
    string str3 = "";
    string str4 = "<color=#F5EDD5>";
    if ((double) weaponSpeed > (double) this._speed)
    {
      str3 = "<sprite name=\"icon_FaithDown\">";
      str4 = "<color=#FF1C1C>";
    }
    else if ((double) weaponSpeed < (double) this._speed)
    {
      str3 = "<sprite name=\"icon_FaithUp\">";
      str4 = "<color=#2DFF1C>";
    }
    string str5 = LocalizeIntegration.ReverseText(this._damage.ToString());
    string str6 = LocalizeIntegration.ReverseText(this._speed.ToString());
    this._damageText.text = string.Format($"{damage}: {str1}{{0}}{{1}}</color>", (object) str2, (object) str5);
    this._speedText.text = string.Format($"{speed}: {str3}{{0}}{{1}}</color>", (object) str4, (object) str6);
  }

  public override void UpdateSortingOrder() => this._canvas.sortingOrder = -1;
}
