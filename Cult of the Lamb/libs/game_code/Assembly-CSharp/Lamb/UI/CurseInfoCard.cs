// Decompiled with JetBrains decompiler
// Type: Lamb.UI.CurseInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using src.UINavigator;
using System;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class CurseInfoCard : UIInfoCardBase<EquipmentType>
{
  [SerializeField]
  public CurseItem _curseItem;
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

  public override void Configure(EquipmentType equipmentType)
  {
    this._curseItem.Configure(equipmentType);
    PlayerFarming inputOnlyFromPlayer = MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer;
    if (equipmentType != EquipmentType.None)
    {
      this._itemHeader.text = $"{LocalizationManager.GetTranslation($"UpgradeSystem/{equipmentType}/Name")} {inputOnlyFromPlayer.currentCurseLevel.ToNumeral()}";
      this._itemLore.text = LocalizationManager.GetTranslation($"UpgradeSystem/{equipmentType}/Lore");
      this._itemDescription.text = LocalizationManager.GetTranslation($"UpgradeSystem/{equipmentType}/Description");
      this._levelText.text = inputOnlyFromPlayer.currentCurseLevel.ToNumeral();
      this._statsHeader.SetActive(true);
      this._statsContainer.SetActive(true);
      this._damageText.text = $"{ScriptLocalization.UI_WeaponSelect.Damage}: <color=#FFD201>{Math.Round((double) EquipmentManager.GetCurseData(inputOnlyFromPlayer.currentCurse).Damage * (double) PlayerSpells.GetCurseDamageMultiplier(inputOnlyFromPlayer), 1).ToString()}";
    }
    else
    {
      this._itemHeader.text = LocalizationManager.GetTranslation("UI/PauseScreen/Player/NoCurse");
      this._itemLore.gameObject.SetActive(false);
      this._itemDescription.text = LocalizationManager.GetTranslation("UI/PauseScreen/Player/NoCurse/Description");
      this._levelText.text = "";
      this._statsHeader.SetActive(false);
      this._statsContainer.SetActive(false);
    }
  }
}
