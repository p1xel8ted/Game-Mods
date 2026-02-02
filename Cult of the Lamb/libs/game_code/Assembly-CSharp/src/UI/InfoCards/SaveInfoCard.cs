// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.SaveInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using RTLTMPro;
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
  public Image _difficultyIcon;
  [SerializeField]
  public TextMeshProUGUI _cultNameHeader;
  [SerializeField]
  public TextMeshProUGUI _quoteHeader;
  [Header("Modifiers")]
  [SerializeField]
  public GameObject _permadeathContainer;
  [SerializeField]
  public GameObject _quickStartContainer;
  [SerializeField]
  public GameObject _penitenceContainer;
  [Header("Stats")]
  [SerializeField]
  public TextMeshProUGUI _playtime;
  [SerializeField]
  public TextMeshProUGUI _dayText;
  [SerializeField]
  public TextMeshProUGUI _followerCount;
  [SerializeField]
  public TextMeshProUGUI _structureCount;
  [SerializeField]
  public TextMeshProUGUI _deathCount;
  [SerializeField]
  public TextMeshProUGUI _percentageCompleted;
  [Header("Images")]
  [SerializeField]
  public Sprite _easySprite;
  [SerializeField]
  public Sprite _mediumSprite;
  [SerializeField]
  public Sprite _hardSprite;
  [SerializeField]
  public Sprite _extraHardSprite;
  [Header("Bishops")]
  [SerializeField]
  public GameObject _cross1;
  [SerializeField]
  public CanvasGroup _recruited1;
  [SerializeField]
  public GameObject _mystery1;
  [SerializeField]
  public GameObject _cross2;
  [SerializeField]
  public CanvasGroup _recruited2;
  [SerializeField]
  public GameObject _mystery2;
  [SerializeField]
  public GameObject _cross3;
  [SerializeField]
  public CanvasGroup _recruited3;
  [SerializeField]
  public GameObject _mystery3;
  [SerializeField]
  public GameObject _cross4;
  [SerializeField]
  public CanvasGroup _recruited4;
  [SerializeField]
  public GameObject _mystery4;
  [SerializeField]
  public GameObject _cross5;
  [SerializeField]
  public GameObject _deathCatKilled;
  [SerializeField]
  public GameObject _deathCatRecruited;
  [SerializeField]
  public GameObject _newGamePlusContainer;
  [Header("Major DLC")]
  [SerializeField]
  public GameObject _majorDLC;
  [SerializeField]
  public TMP_Text percentageText;
  [SerializeField]
  public TMP_Text lambGhostText;
  [SerializeField]
  public TMP_Text rottingFollowersText;
  [SerializeField]
  public TMP_Text wintersOccuredText;
  [SerializeField]
  public Image yngya;
  [SerializeField]
  public GameObject yngyaCross;
  [SerializeField]
  public Image wolf;
  [SerializeField]
  public GameObject wolfCross;
  [SerializeField]
  public Image executioner;
  [SerializeField]
  public GameObject executionerCross;
  [SerializeField]
  public GameObject backUpSaveBreaker;
  [SerializeField]
  public GameObject backUpSaveContainer;
  public MetaData metaData;

  public override void Configure(MetaData metaData)
  {
    this.metaData = metaData;
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
    this._playtime.isRightToLeftText = false;
    if (LocalizationManager.CurrentLanguage == "Arabic")
      this._cultNameHeader.isRightToLeftText = TextUtils.IsRTLInput(this._cultNameHeader.text);
    else
      this._cultNameHeader.isRightToLeftText = false;
    string minutes = "m";
    string hours = "h";
    string days = "d";
    this.GetLocalizedPlayTime(ref minutes, ref hours, ref days);
    TimeSpan timeSpan = new TimeSpan(0, 0, 0, (int) metaData.PlayTime);
    if (timeSpan.TotalHours < 1.0)
      this._playtime.text = $"{timeSpan.Minutes}{minutes}";
    else if (timeSpan.TotalDays < 1.0)
      this._playtime.text = $"{timeSpan.Hours}{hours} {timeSpan.Minutes}{minutes}";
    else
      this._playtime.text = $"{timeSpan.Days}{days} {timeSpan.Hours}{hours} {timeSpan.Minutes}{minutes}";
    this._dayText.text = string.Format(ScriptLocalization.UI.DayNumber, (object) LocalizeIntegration.ReverseText(metaData.Day.ToString()));
    this._followerCount.text = $"{"<sprite name=\"icon_Followers\">"} {metaData.FollowerCount}";
    this._structureCount.text = $"{"<sprite name=\"icon_House\">"} {metaData.StructureCount}";
    this._deathCount.text = $"{"<sprite name=\"icon_Dead\">"} {metaData.DeathCount}";
    this._cross1.SetActive(metaData.Dungeon1Completed);
    this._cross2.SetActive(metaData.Dungeon2Completed);
    this._cross3.SetActive(metaData.Dungeon3Completed);
    this._cross4.SetActive(metaData.Dungeon4Completed);
    this._cross5.SetActive(metaData.GameBeaten);
    this._majorDLC.SetActive(metaData.ActivatedMajorDLC);
    if (metaData.ActivatedMajorDLC)
    {
      this.percentageText.text = $"{metaData.DLCPercentageCompleted}%";
      this.rottingFollowersText.text = $"{"<sprite name=\"icon_Trait_Mutated\">"} <size=20>{metaData.RottingFollowerCount}";
      this.wintersOccuredText.text = $"{"<sprite name=\"icon_Winter\">"} <size=20>{metaData.WinterCount}";
      this.lambGhostText.text = $"{"<sprite name=\"icon_YngyaGhost\">"} <size=20>{metaData.LambGhostsCount}";
      this.wolfCross.SetActive(metaData.WolfBeaten);
      this.yngyaCross.SetActive(metaData.YngyaBeaten);
      this.executionerCross.SetActive(metaData.ExecutionerBeaten);
      if (metaData.WolfBeaten)
        this.wolf.color = Color.white;
      if (metaData.YngyaBeaten)
        this.yngya.color = Color.white;
      if (metaData.ExecutionerBeaten)
        this.executioner.color = Color.white;
    }
    if (metaData.Version != null)
    {
      string[] strArray = metaData.Version.Split('.', StringSplitOptions.None);
      int result;
      if (strArray.Length >= 2 && int.TryParse(strArray[1], out result))
      {
        if (result >= 2)
        {
          int num = metaData.Dungeon1NGPCompleted || metaData.Dungeon2NGPCompleted || metaData.Dungeon3NGPCompleted ? 1 : (metaData.Dungeon4NGPCompleted ? 1 : 0);
          this._newGamePlusContainer.SetActive(metaData.GameBeaten);
          if (num != 0)
          {
            this._mystery1.SetActive(false);
            this._mystery2.SetActive(false);
            this._mystery3.SetActive(false);
            this._mystery4.SetActive(false);
            this._recruited1.alpha = metaData.Dungeon1NGPCompleted ? 1f : 0.25f;
            this._recruited2.alpha = metaData.Dungeon2NGPCompleted ? 1f : 0.25f;
            this._recruited3.alpha = metaData.Dungeon3NGPCompleted ? 1f : 0.25f;
            this._recruited4.alpha = metaData.Dungeon4NGPCompleted ? 1f : 0.25f;
          }
          else
          {
            this._recruited1.alpha = 0.0f;
            this._recruited2.alpha = 0.0f;
            this._recruited3.alpha = 0.0f;
            this._recruited4.alpha = 0.0f;
          }
          this._deathCatKilled.SetActive(metaData.GameBeaten && !metaData.DeathCatRecruited);
          this._deathCatRecruited.SetActive(metaData.GameBeaten && metaData.DeathCatRecruited);
        }
        if (result >= 3)
          this._percentageCompleted.text = $"{metaData.PercentageCompleted}%";
      }
    }
    else
      this._newGamePlusContainer.SetActive(false);
    this._permadeathContainer.SetActive(metaData.Permadeath);
    this._quickStartContainer.SetActive(metaData.QuickStart);
    this._penitenceContainer.SetActive(metaData.Penitence);
  }

  public void ShowBackUpPrompt()
  {
    this.backUpSaveBreaker.gameObject.SetActive(this.metaData.Permadeath || this.metaData.QuickStart || this.metaData.Penitence);
    this.backUpSaveContainer.gameObject.SetActive(true);
  }

  public void HideBackUpPrompt()
  {
    this.backUpSaveBreaker.gameObject.SetActive(false);
    this.backUpSaveContainer.gameObject.SetActive(false);
  }

  public void GetLocalizedPlayTime(ref string minutes, ref string hours, ref string days)
  {
    switch (LocalizationManager.CurrentLanguage)
    {
      case "Dutch":
        minutes = "m";
        hours = "u";
        days = "d";
        break;
      case "Italian":
        minutes = "m";
        hours = "h";
        days = "g";
        break;
      case "French":
        minutes = "m";
        hours = "h";
        days = "j";
        break;
      case "Turkish":
        minutes = "d";
        hours = "s";
        days = "g";
        break;
      case "Arabic":
        minutes = "د";
        hours = "س";
        days = "ي";
        break;
    }
  }
}
