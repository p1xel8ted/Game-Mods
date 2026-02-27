// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.CrownAbilityInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using TMPro;
using UnityEngine;

#nullable disable
namespace src.UI.InfoCards;

public class CrownAbilityInfoCard : UIInfoCardBase<UpgradeSystem.Type>
{
  [SerializeField]
  private bool _ignoreLockedState;
  [SerializeField]
  private bool _showCost;
  [SerializeField]
  private TextMeshProUGUI _headerText;
  [SerializeField]
  private TextMeshProUGUI _descriptionText;
  [SerializeField]
  private CrownAbilityItem _crownAbilityItem;
  [SerializeField]
  private GameObject _costHeader;
  [SerializeField]
  private GameObject _costContainer;
  [SerializeField]
  private TextMeshProUGUI _costText;
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
