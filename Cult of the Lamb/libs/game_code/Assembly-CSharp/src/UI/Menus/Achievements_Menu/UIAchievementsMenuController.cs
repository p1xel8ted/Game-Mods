// Decompiled with JetBrains decompiler
// Type: src.UI.Menus.Achievements_Menu.UIAchievementsMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using src.Managers;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
namespace src.UI.Menus.Achievements_Menu;

public class UIAchievementsMenuController : UIMenuBase
{
  [SerializeField]
  public TMP_Text _baseGameCount;
  [SerializeField]
  public Transform _baseGameContent;
  [SerializeField]
  public TMP_Text _relicsCount;
  [SerializeField]
  public Transform _relicsContent;
  [SerializeField]
  public TMP_Text _sinsCount;
  [SerializeField]
  public Transform _sinsContent;
  [SerializeField]
  public TMP_Text _dlcCount;
  [SerializeField]
  public Transform _dlcContent;
  [SerializeField]
  public AchievementItem _achievementItemTemplate;
  [SerializeField]
  public MMScrollRect _scrollRect;
  [SerializeField]
  public List<AchievementItem> _achievementItems = new List<AchievementItem>();
  public string[] _baseGameAchievements = new string[37]
  {
    "platinum",
    "ALL_SKINS_UNLOCKED",
    "ALL_TAROTS_UNLOCKED",
    "FULLY_UPGRADED_SHRINE",
    "FEED_FOLLOWER_MEAT",
    "FIND_ALL_LOCATIONS",
    "UPGRADE_ALL_SERMONS",
    "KILL_BOSS_5",
    "KILL_BOSS_4",
    "KILL_BOSS_3",
    "KILL_BOSS_2",
    "KILL_BOSS_1",
    "UNLOCK_TUNIC",
    "UNLOCK_ALL_TUNICS",
    "FIRST_FOLLOWER",
    "GAIN_FIVE_FOLLOWERS",
    "TEN_FOLLOWERS",
    "TWENTY_FOLLOWERS",
    "TAKE_CONFESSION",
    "FISH_ALL_TYPES",
    "WIN_KNUCKLEBONES",
    "WIN_KNUCKLEBONES_ALL",
    "FIX_LIGHTHOUSE",
    "FIRST_RITUAL",
    "FIRST_SACRIFICE",
    "SACRIFICE_FOLLOWERS",
    "DEAL_WITH_THE_DEVIL",
    "666_GOLD",
    "KILL_FIRST_BOSS",
    "KILL_BOSS_1_NODAMAGE",
    "KILL_BOSS_2_NODAMAGE",
    "KILL_BOSS_3_NODAMAGE",
    "KILL_BOSS_4_NODAMAGE",
    "DELIVER_FIRST_SERMON",
    "FIRST_DEATH",
    "ALL_WEAPONS_UNLOCKED",
    "ALL_CURSES_UNLOCKED"
  };
  public string[] _relicsAchievements = new string[5]
  {
    "ALL_RELICS_UNLOCKED",
    "BEAT_UP_MIDAS",
    "RETURN_BAAL_AYM",
    "COMPLETE_CHALLENGE_ROW",
    "ALL_LEADER_FOLLOWERS"
  };
  public string[] _sinsAchievements = new string[6]
  {
    "ALL_LORE",
    "ALL_OUTFITS",
    "DISCIPLES_12",
    "FULLY_UPGRADE_RANKING",
    "SOZO_QUEST",
    "MATING_5"
  };
  public string[] _dlcAchievements = new string[9]
  {
    AchievementsWrapper.Tags.ALL_LEGENDARY_WEAPONS,
    AchievementsWrapper.Tags.RATAU_END,
    AchievementsWrapper.Tags.INDOCTRINATE_MIDAS,
    AchievementsWrapper.Tags.ALL_FLOCKADE,
    AchievementsWrapper.Tags.BEAT_EXECUTIONER,
    AchievementsWrapper.Tags.BEAT_WOLF,
    AchievementsWrapper.Tags.BEAT_YNGYA,
    AchievementsWrapper.Tags.BEAT_YNGYA_NOATTACK,
    AchievementsWrapper.Tags.WOOLHAVEN_COMPLETE
  };

  public override void OnHideStarted()
  {
    base.OnHideStarted();
    UIManager.PlayAudio("event:/ui/close_menu");
    this._scrollRect.enabled = false;
  }

  public override void OnShowStarted()
  {
    base.OnShowStarted();
    UIManager.PlayAudio("event:/ui/open_menu");
    this._scrollRect.normalizedPosition = (Vector2) Vector3.one;
    int num1 = 0;
    int num2 = 0;
    int num3 = 0;
    int num4 = 0;
    foreach (AchievementItem achievementItem in this._achievementItems)
    {
      achievementItem.Configure();
      if (achievementItem.Unlocked)
      {
        if (this._baseGameAchievements.Contains<string>(achievementItem.AchievementID))
          ++num1;
        else if (this._relicsAchievements.Contains<string>(achievementItem.AchievementID))
          ++num2;
        else if (this._sinsAchievements.Contains<string>(achievementItem.AchievementID))
          ++num3;
        else if (this._dlcAchievements.Contains<string>(achievementItem.AchievementID))
          ++num4;
      }
    }
    this._baseGameCount.text = $"{(object) num1}/{(object) this._baseGameAchievements.Length}";
    this._relicsCount.text = $"{(object) num2}/{(object) this._relicsAchievements.Length}";
    this._sinsCount.text = $"{(object) num3}/{(object) this._sinsAchievements.Length}";
    this._dlcCount.text = $"{(object) num4}/{(object) this._dlcAchievements.Length}";
    this._baseGameCount.isRightToLeftText = false;
    this._relicsCount.isRightToLeftText = false;
    this._sinsCount.isRightToLeftText = false;
    this._dlcCount.isRightToLeftText = false;
    this._achievementItems.Sort((Comparison<AchievementItem>) ((a, b) => a.IsHidden.CompareTo(b.IsHidden)));
    for (int index = 0; index < this._achievementItems.Count; ++index)
      this._achievementItems[index].transform.SetSiblingIndex(index);
  }

  public override void OnCancelButtonInput()
  {
    base.OnCancelButtonInput();
    if (!this._canvasGroup.interactable)
      return;
    this.Hide();
    this._scrollRect.enabled = false;
  }

  public override void OnHideCompleted()
  {
    PersistenceManager.Save();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }
}
