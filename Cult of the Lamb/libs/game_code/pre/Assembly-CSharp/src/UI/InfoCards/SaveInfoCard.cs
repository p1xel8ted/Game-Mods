// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.SaveInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace src.UI.InfoCards;

public class SaveInfoCard : UIInfoCardBase<MetaData>
{
  [Header("Headers")]
  [SerializeField]
  private Image _difficultyIcon;
  [SerializeField]
  private TextMeshProUGUI _cultNameHeader;
  [SerializeField]
  private TextMeshProUGUI _quoteHeader;
  [Header("Stats")]
  [SerializeField]
  private TextMeshProUGUI _playtime;
  [SerializeField]
  private TextMeshProUGUI _dayText;
  [SerializeField]
  private TextMeshProUGUI _followerCount;
  [SerializeField]
  private TextMeshProUGUI _structureCount;
  [SerializeField]
  private TextMeshProUGUI _deathCount;
  [SerializeField]
  private TextMeshProUGUI _percentageCompleted;
  [Header("Images")]
  [SerializeField]
  private Sprite _easySprite;
  [SerializeField]
  private Sprite _mediumSprite;
  [SerializeField]
  private Sprite _hardSprite;
  [SerializeField]
  private Sprite _extraHardSprite;
  [Header("Bishops")]
  [SerializeField]
  private GameObject _cross1;
  [SerializeField]
  private GameObject _cross2;
  [SerializeField]
  private GameObject _cross3;
  [SerializeField]
  private GameObject _cross4;
  [SerializeField]
  private GameObject _cross5;

  public override void Configure(MetaData metaData)
  {
    switch (metaData.Difficulty)
    {
      case 0:
        this._difficultyIcon.sprite = this._easySprite;
        break;
      case 1:
        this._difficultyIcon.sprite = this._mediumSprite;
        break;
      case 2:
        this._difficultyIcon.sprite = this._hardSprite;
        break;
      case 3:
        this._difficultyIcon.sprite = this._extraHardSprite;
        break;
    }
    if (string.IsNullOrEmpty(metaData.CultName))
      this._cultNameHeader.text = ScriptLocalization.NAMES_Place.Cult;
    else
      this._cultNameHeader.text = metaData.CultName;
    this._percentageCompleted.text = "";
    TimeSpan timeSpan = new TimeSpan(0, 0, 0, (int) metaData.PlayTime);
    if (timeSpan.TotalHours < 1.0)
      this._playtime.text = $"{timeSpan.Minutes}m";
    else if (timeSpan.TotalDays < 1.0)
      this._playtime.text = $"{timeSpan.Hours}h {timeSpan.Minutes}m";
    else
      this._playtime.text = $"{timeSpan.Days}d {timeSpan.Hours}h {timeSpan.Minutes}m";
    this._dayText.text = string.Format(ScriptLocalization.UI.DayNumber, (object) metaData.Day);
    this._followerCount.text = $"{"<sprite name=\"icon_Followers\">"} {metaData.FollowerCount}";
    this._structureCount.text = $"{"<sprite name=\"icon_House\">"} {metaData.StructureCount}";
    this._deathCount.text = $"{"<sprite name=\"icon_Dead\">"} {metaData.DeathCount}";
    this._cross1.SetActive(metaData.Dungeon1Completed);
    this._cross2.SetActive(metaData.Dungeon2Completed);
    this._cross3.SetActive(metaData.Dungeon3Completed);
    this._cross4.SetActive(metaData.Dungeon4Completed);
    this._cross5.SetActive(metaData.GameBeaten);
  }
}
