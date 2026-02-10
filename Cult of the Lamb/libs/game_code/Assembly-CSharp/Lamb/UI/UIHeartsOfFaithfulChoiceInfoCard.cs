// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIHeartsOfFaithfulChoiceInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class UIHeartsOfFaithfulChoiceInfoCard : 
  UIChoiceInfoCard<UIHeartsOfTheFaithfulChoiceMenuController.Types>
{
  [SerializeField]
  public TextMeshProUGUI _levelText;
  [SerializeField]
  public TextMeshProUGUI _currentStat;
  [SerializeField]
  public TextMeshProUGUI _nextStat;

  public override void ConfigureImpl(
    UIHeartsOfTheFaithfulChoiceMenuController.Types info)
  {
    if (info != UIHeartsOfTheFaithfulChoiceMenuController.Types.Hearts)
    {
      if (info != UIHeartsOfTheFaithfulChoiceMenuController.Types.Strength)
        return;
      this._currentStat.text = ((float) (1.0 + 0.25 * (double) DataManager.Instance.PLAYER_DAMAGE_LEVEL)).ToString();
      this._nextStat.text = ((float) (1.0 + 0.25 * (double) DataManager.Instance.PLAYER_DAMAGE_LEVEL + 0.25)).ToString();
      this._levelText.text = (DataManager.Instance.PLAYER_DAMAGE_LEVEL + 1).ToNumeral();
    }
    else
    {
      this._currentStat.text = (PlayerFarming.Instance.health.PLAYER_TOTAL_HEALTH / 2f).ToString();
      this._nextStat.text = ((float) ((double) PlayerFarming.Instance.health.PLAYER_TOTAL_HEALTH / 2.0 + 0.5)).ToString();
      this._levelText.text = (DataManager.Instance.PLAYER_HEARTS_LEVEL + 1).ToNumeral();
    }
  }
}
