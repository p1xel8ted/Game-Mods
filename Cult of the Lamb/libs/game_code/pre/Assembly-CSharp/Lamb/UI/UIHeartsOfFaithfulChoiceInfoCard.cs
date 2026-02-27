// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIHeartsOfFaithfulChoiceInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class UIHeartsOfFaithfulChoiceInfoCard : 
  UIChoiceInfoCard<UIHeartsOfTheFaithfulChoiceMenuController.Types>
{
  [SerializeField]
  private TextMeshProUGUI _levelText;
  [SerializeField]
  private TextMeshProUGUI _currentStat;
  [SerializeField]
  private TextMeshProUGUI _nextStat;

  protected override void ConfigureImpl(
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
      this._currentStat.text = (DataManager.Instance.PLAYER_TOTAL_HEALTH / 2f).ToString();
      this._nextStat.text = ((float) ((double) DataManager.Instance.PLAYER_TOTAL_HEALTH / 2.0 + 0.5)).ToString();
      this._levelText.text = (DataManager.Instance.PLAYER_HEARTS_LEVEL + 1).ToNumeral();
    }
  }
}
