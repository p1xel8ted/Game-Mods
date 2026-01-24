// Decompiled with JetBrains decompiler
// Type: AchievementsWrapper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public static Action<string> OnAchievementUnlocked;
  public static List<int> unlockedAchievements;
  public static MonoBehaviour instance;
  public static Unify.Achievements.PlatformAchievementsStatusDelegate AchievementsCheck = new Unify.Achievements.PlatformAchievementsStatusDelegate(AchievementsWrapper.GetAchievementProgress);
  public static List<string> Achievements = new List<string>()
  {
    "GODHOOD",
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
    "ALL_CURSES_UNLOCKED",
    "ALL_RELICS_UNLOCKED",
    "BEAT_UP_MIDAS",
    "RETURN_BAAL_AYM",
    "COMPLETE_CHALLENGE_ROW",
    "ALL_LEADER_FOLLOWERS",
    "ALL_LORE",
    "ALL_OUTFITS",
    "DISCIPLES_5",
    "FULLY_UPGRADE_RANKING",
    "SOZO_QUEST",
    "MATING_5",
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
  public static List<string> DLCAchievements = new List<string>()
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
  public static int UnlockAchivementCount;
  public static bool CheckedForAchievementMismatch = false;
  public static bool SyncedWithBackend = false;

  public void Awake()
  {
    AchievementsWrapper.instance = (MonoBehaviour) this;
    AchievementsWrapper.LoadAchievementData();
  }

  public static void LoadAchievementData()
  {
    Debug.Log((object) "####AchievementsWrapper: LoadAchievementData");
    string str = Unify.PlayerPrefs.GetString("unlockedAchievements", "");
    Debug.Log((object) ("####AchievementsWrapper: " + str));
    AchievementsWrapper.CheckedForAchievementMismatch = false;
    try
    {
      AchievementsWrapper.unlockedAchievements = ((IEnumerable<string>) str.Split(',', StringSplitOptions.None)).Select<string, int>(new Func<string, int>(int.Parse)).ToList<int>();
    }
    catch
    {
      AchievementsWrapper.unlockedAchievements = new List<int>();
    }
    AchievementsWrapper.compareAchievements();
  }

  public static void UnlockAchievement(Achievement achievementId)
  {
    ++AchievementsWrapper.UnlockAchivementCount;
    Debug.Log((object) $"####AchievementsWrapper: attempt Unlock for Avhievement {achievementId.label} ID{achievementId.id.ToString()}");
    if (AchievementsWrapper.unlockedAchievements.Contains(achievementId.id))
      return;
    Debug.Log((object) $"####AchievementsWrapper: Unlock call for Avhievement {achievementId.label} ID{achievementId.id.ToString()}");
    SessionManager.instance.UnlockAchievement(achievementId);
    AchievementsWrapper.unlockedAchievements.Add(achievementId.id);
    Unify.PlayerPrefs.SetString("unlockedAchievements", string.Join<int>(", ", (IEnumerable<int>) AchievementsWrapper.unlockedAchievements.ToArray()));
    Action<string> achievementUnlocked = AchievementsWrapper.OnAchievementUnlocked;
    if (achievementUnlocked != null)
      achievementUnlocked(achievementId.label);
    Unify.PlayerPrefs.Save();
    if (SteamAPI.Init())
      SteamUserStats.SetAchievement(achievementId.steamId);
    if (!(achievementId != Unify.Achievements.Instance.Lookup("platinum")))
      return;
    AchievementsWrapper.compareAchievements();
  }

  public static bool UnlockedAchievement(Achievement achievementId)
  {
    return AchievementsWrapper.unlockedAchievements.Contains(achievementId.id);
  }

  public static void DoAchievementsMismatchCheck()
  {
    if (!AchievementsWrapper.CheckedForAchievementMismatch)
      return;
    foreach (int unlockedAchievement in AchievementsWrapper.unlockedAchievements)
      AchievementsWrapper.AttemptUnlockOnPlatform(Unify.Achievements.Instance.Get(unlockedAchievement));
  }

  public static void GetAchievementProgress(List<AchievementProgress> result)
  {
    if (result == null || result.Count == 0)
      return;
    foreach (AchievementProgress achievementProgress in result)
    {
      if (achievementProgress.progress >= 100 && !AchievementsWrapper.unlockedAchievements.Contains(achievementProgress.id))
      {
        AchievementsWrapper.unlockedAchievements.Add(achievementProgress.id);
        Unify.PlayerPrefs.SetString("unlockedAchievements", string.Join<int>(", ", (IEnumerable<int>) AchievementsWrapper.unlockedAchievements.ToArray()));
        Action<string> achievementUnlocked = AchievementsWrapper.OnAchievementUnlocked;
        if (achievementUnlocked != null)
          achievementUnlocked(achievementProgress.name);
      }
    }
    Unify.PlayerPrefs.Save();
  }

  public static void compareAchievements()
  {
    try
    {
      if (UnifyManager.platform != UnifyManager.Platform.Switch && (UnityEngine.Object) SessionManager.instance != (UnityEngine.Object) null && SessionManager.instance.HasStarted && !AchievementsWrapper.SyncedWithBackend)
      {
        AchievementsWrapper.SyncedWithBackend = true;
        Debug.Log((object) "####AchievementsWrapper: GetPlatformAchievementStatus");
        UserHelper.Instance.GetPlatformAchievementStatus(AchievementsWrapper.AchievementsCheck);
      }
      int num = 0;
      foreach (string achievement in AchievementsWrapper.Achievements)
      {
        if (!AchievementsWrapper.DLCAchievements.Contains(achievement))
        {
          bool pbAchieved = false;
          if (SteamAPI.Init())
            SteamUserStats.GetAchievement(achievement, out pbAchieved);
          if (pbAchieved)
            ++num;
        }
      }
      if (num < AchievementsWrapper.Achievements.Count - 1 - AchievementsWrapper.DLCAchievements.Count)
        return;
      AchievementsWrapper.instance.StartCoroutine((IEnumerator) AchievementsWrapper.UnlockPlatinum());
    }
    catch (Exception ex)
    {
      Debug.Log((object) ("####AchievementsWrapper: compareAchievements " + ex.Message));
    }
  }

  public static bool CheckBaseGameAchievementsUnlocked()
  {
    bool flag = true;
    for (int index = 1; index <= 36; ++index)
    {
      if (!AchievementsWrapper.unlockedAchievements.Contains(index))
      {
        flag = false;
        break;
      }
    }
    return flag;
  }

  public static IEnumerator UnlockPlatinum()
  {
    yield return (object) new WaitForSeconds(1f);
    AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("platinum"));
    yield return (object) null;
  }

  public static void AttemptUnlockOnPlatform(Achievement achievement)
  {
    Debug.Log((object) $"####AchievementsWrapper: attempt Unlock for Avhievement {achievement.label} ID{achievement.id.ToString()}");
    Debug.Log((object) $"####AchievementsWrapper: Unlock call for Achievement {achievement.label} ID{achievement.id.ToString()}");
    SessionManager.instance.UnlockAchievement(achievement);
    Action<string> achievementUnlocked = AchievementsWrapper.OnAchievementUnlocked;
    if (achievementUnlocked != null)
      achievementUnlocked(achievement.label);
    if (!(achievement != Unify.Achievements.Instance.Lookup("platinum")))
      return;
    AchievementsWrapper.compareAchievements();
  }

  public static class Tags
  {
    public static string ALL_LEGENDARY_WEAPONS = nameof (ALL_LEGENDARY_WEAPONS);
    public static string RATAU_END = nameof (RATAU_END);
    public static string INDOCTRINATE_MIDAS = nameof (INDOCTRINATE_MIDAS);
    public static string ALL_FLOCKADE = nameof (ALL_FLOCKADE);
    public static string ALL_DEPOSITFOLLOWER = nameof (ALL_DEPOSITFOLLOWER);
    public static string WOOLHAVEN_COMPLETE = nameof (WOOLHAVEN_COMPLETE);
    public static string BEAT_EXECUTIONER = nameof (BEAT_EXECUTIONER);
    public static string BEAT_WOLF = nameof (BEAT_WOLF);
    public static string BEAT_YNGYA = nameof (BEAT_YNGYA);
    public static string BEAT_YNGYA_NOATTACK = nameof (BEAT_YNGYA_NOATTACK);
  }
}
