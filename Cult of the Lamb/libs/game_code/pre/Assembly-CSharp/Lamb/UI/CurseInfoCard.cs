// Decompiled with JetBrains decompiler
// Type: Lamb.UI.CurseInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class CurseInfoCard : UIInfoCardBase<EquipmentType>
{
  [SerializeField]
  private CurseItem _curseItem;
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

  public override void Configure(EquipmentType equipmentType)
  {
    this._curseItem.Configure(equipmentType);
    if (equipmentType != EquipmentType.None)
    {
      this._itemHeader.text = $"{LocalizationManager.GetTranslation($"UpgradeSystem/{equipmentType}/Name")} {DataManager.Instance.CurrentCurseLevel.ToNumeral()}";
      this._itemLore.text = LocalizationManager.GetTranslation($"UpgradeSystem/{equipmentType}/Lore");
      this._itemDescription.text = LocalizationManager.GetTranslation($"UpgradeSystem/{equipmentType}/Description");
      this._levelText.text = DataManager.Instance.CurrentCurseLevel.ToNumeral();
      this._statsHeader.SetActive(true);
      this._statsContainer.SetActive(true);
      this._damageText.text = $"{ScriptLocalization.UI_WeaponSelect.Damage}: <color=#FFD201>{(object) Math.Round((double) EquipmentManager.GetCurseData(DataManager.Instance.CurrentCurse).Damage * (double) PlayerSpells.GetCurseDamageMultiplier(), 1)}";
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
