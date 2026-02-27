// Decompiled with JetBrains decompiler
// Type: AchievementsWrapper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unify;
using UnityEngine;

#nullable disable
public class AchievementsWrapper : MonoBehaviour
{
  private static List<int> unlockedAchievements;
  public static MonoBehaviour instance;
  public static Unify.Achievements.PlatformAchievementsStatusDelegate XBAchievementsCheck = new Unify.Achievements.PlatformAchievementsStatusDelegate(AchievementsWrapper.XBGetAchievementProgress);
  public static List<string> Achievements = new List<string>()
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

  private void Awake()
  {
    AchievementsWrapper.instance = (MonoBehaviour) this;
    string str = Unify.PlayerPrefs.GetString("unlockedAchievements", "");
    try
    {
      AchievementsWrapper.unlockedAchievements = ((IEnumerable<string>) str.Split(',')).Select<string, int>(new Func<string, int>(int.Parse)).ToList<int>();
    }
    catch
    {
      AchievementsWrapper.unlockedAchievements = new List<int>();
    }
    AchievementsWrapper.compareAchievements();
  }

  public static void UnlockAchievement(Achievement achievementId)
  {
    SessionManager.instance.UnlockAchievement(achievementId);
    if (AchievementsWrapper.unlockedAchievements.Contains(achievementId.id))
      return;
    AchievementsWrapper.unlockedAchievements.Add(achievementId.id);
    Unify.PlayerPrefs.SetString("unlockedAchievements", AchievementsWrapper.unlockedAchievements.ToString());
    AchievementsWrapper.compareAchievements();
  }

  public static void XBGetAchievementProgress(List<AchievementProgress> result)
  {
    if (result == null || result.Count == 0)
      return;
    foreach (AchievementProgress achievementProgress in result)
    {
      if (achievementProgress.progress >= 100 && !AchievementsWrapper.unlockedAchievements.Contains(achievementProgress.id))
      {
        AchievementsWrapper.unlockedAchievements.Add(achievementProgress.id);
        Unify.PlayerPrefs.SetString("unlockedAchievements", AchievementsWrapper.unlockedAchievements.ToString());
      }
    }
  }

  public static void compareAchievements()
  {
    int num = 0;
    foreach (string achievement in AchievementsWrapper.Achievements)
    {
      bool flag = false;
      ref bool local = ref flag;
      SteamUserStats.GetAchievement(achievement, out local);
      if (flag)
        ++num;
    }
    if (num < AchievementsWrapper.Achievements.Count - 1)
      return;
    AchievementsWrapper.instance.StartCoroutine((IEnumerator) AchievementsWrapper.UnlockPlatinum());
  }

  private static IEnumerator UnlockPlatinum()
  {
    yield return (object) new WaitForSeconds(1f);
    Achievement achievement = Unify.Achievements.Instance.Lookup("platinum");
    SessionManager.instance.UnlockAchievement(achievement);
    yield return (object) null;
  }
}
