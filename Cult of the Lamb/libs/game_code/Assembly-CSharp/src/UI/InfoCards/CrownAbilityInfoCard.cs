// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.CrownAbilityInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using TMPro;
using UnityEngine;

#nullable disable
namespace src.UI.InfoCards;

public class CrownAbilityInfoCard : UIInfoCardBase<UpgradeSystem.Type>
{
  [SerializeField]
  public bool _ignoreLockedState;
  [SerializeField]
  public bool _showCost;
  [SerializeField]
  public TextMeshProUGUI _headerText;
  [SerializeField]
  public TextMeshProUGUI _descriptionText;
  [SerializeField]
  public CrownAbilityItem _crownAbilityItem;
  [SerializeField]
  public GameObject _costHeader;
  [SerializeField]
  public GameObject _costContainer;
  [SerializeField]
  public TextMeshProUGUI _costText;
  public GameObject _redOutline;

  public override void Configure(UpgradeSystem.Type type)
  {
    this._redOutline.SetActive(false);
    this._crownAbilityItem.Configure(type);
    this._costText.text = StructuresData.ItemCost.GetCostStringWithQuantity(UpgradeSystem.GetCost(type));
    if (UpgradeSystem.GetUnlocked(type) || this._ignoreLockedState)
    {
      this._headerText.text = UpgradeSystem.GetLocalizedName(type);
      this._descriptionText.text = UpgradeSystem.GetLocalizedDescription(type);
      this._costHeader.SetActive(this._ignoreLockedState && this._showCost && !UpgradeSystem.GetUnlocked(type));
      this._costContainer.SetActive(this._ignoreLockedState && this._showCost && !UpgradeSystem.GetUnlocked(type));
    }
    else
    {
      this._headerText.text = LocalizationManager.GetTranslation("UI/PauseScreen/Player/AbilityLocked");
      this._descriptionText.text = LocalizationManager.GetTranslation("UI/PauseScreen/Player/AbilityLocked/Description");
      this._costText.text = StructuresData.ItemCost.GetCostStringWithQuantity(UpgradeSystem.GetCost(type));
      if (!this._showCost)
        return;
      this._costHeader.SetActive(true);
      this._costContainer.SetActive(true);
    }
  }
}
