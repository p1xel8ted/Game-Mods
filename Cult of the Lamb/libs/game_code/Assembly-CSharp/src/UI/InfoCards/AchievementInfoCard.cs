// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.AchievementInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using src.UI.Menus.Achievements_Menu;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace src.UI.InfoCards;

public class AchievementInfoCard : UIInfoCardBase<AchievementItem>
{
  [SerializeField]
  public Image _achievementIcon;
  [SerializeField]
  public GameObject _lockIcon;
  [SerializeField]
  public Image _lockStateOverlay;
  [SerializeField]
  public TMP_Text _achievementHeader;
  [SerializeField]
  public TMP_Text _achievementDescription;

  public override void Configure(AchievementItem config)
  {
    this._achievementIcon.sprite = config.AchievementSprite;
    if (!config.IsHidden || config.Unlocked)
    {
      this._lockStateOverlay.color = new Color(0.0f, 0.0f, 0.0f, 0.25f);
      this._achievementHeader.text = LocalizationManager.GetTranslation("Achievements/" + config.AchievementID);
      this._achievementDescription.text = LocalizationManager.GetTranslation($"Achievements/{config.AchievementID}/Description");
    }
    else
    {
      this._lockStateOverlay.color = new Color(0.1f, 0.1f, 0.1f, 1f);
      this._achievementHeader.text = LocalizationManager.GetTranslation("UI/AchievementsMenu/Hidden");
      this._achievementDescription.text = LocalizationManager.GetTranslation("UI/AchievementsMenu/Hidden/Description");
    }
    if (!config.Unlocked)
      this._achievementIcon.color = new Color(0.0f, 1f, 1f, 1f);
    else
      this._achievementIcon.color = new Color(1f, 1f, 1f, 1f);
    this._lockIcon.SetActive(!config.Unlocked);
  }
}
