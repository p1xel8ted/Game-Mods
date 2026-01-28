// Decompiled with JetBrains decompiler
// Type: LoreSystem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using Unify;
using UnityEngine;

#nullable disable
public class LoreSystem : MonoBehaviour
{
  public static int LoreTotalLoreRoom = 9;
  public static List<int> listOfLore = new List<int>();
  public static List<int> loreBlacklist = new List<int>()
  {
    10,
    11,
    12,
    13,
    14,
    15,
    16 /*0x10*/,
    17,
    18,
    19,
    20,
    21,
    22,
    23,
    24,
    25,
    26,
    27
  };
  public static int WoolhavenLoreStartsFromID = 15;

  public static int LoreCount => SeasonsManager.Active ? 28 : 15;

  public static List<int> LoreList(bool includeBlacklist = false)
  {
    if (LoreSystem.listOfLore.Count != 0)
      return LoreSystem.listOfLore;
    List<int> intList = new List<int>();
    for (int index = 0; index < LoreSystem.LoreCount; ++index)
    {
      if (includeBlacklist || !LoreSystem.loreBlacklist.Contains(index))
        intList.Add(index);
    }
    return intList;
  }

  public static bool IsDLCLore(int lore) => lore >= LoreSystem.WoolhavenLoreStartsFromID;

  public static List<int> GetUnlockedLoreList() => DataManager.Instance.LoreUnlocked;

  public static List<int> GetLockedLoreList()
  {
    List<int> lockedLoreList = new List<int>();
    foreach (int lore in LoreSystem.LoreList(true))
    {
      if (!DataManager.Instance.LoreUnlocked.Contains(lore))
        lockedLoreList.Add(lore);
    }
    return lockedLoreList;
  }

  public static bool LoreAvailable()
  {
    return DataManager.Instance.LoreUnlocked.Count < LoreSystem.LoreCount;
  }

  public static bool LoreAvailable(int loreID)
  {
    foreach (int num in DataManager.Instance.LoreUnlocked)
    {
      if (loreID == num)
        return true;
    }
    return false;
  }

  public static void UnlockLore(int i)
  {
    DataManager.Instance.LoreUnlocked.Add(i);
    DataManager.Instance.LoreStonesOnboarded = true;
    int num = 0;
    for (int index = 0; index < DataManager.Instance.LoreUnlocked.Count; ++index)
    {
      if (DataManager.Instance.LoreUnlocked[index] < 15)
        ++num;
    }
    if (num < 15)
      return;
    AchievementsWrapper.UnlockAchievement(Achievements.Instance.Lookup("ALL_LORE"));
  }

  public static int GetUnlockedLore()
  {
    if (!LoreSystem.LoreAvailable())
      return -1;
    List<int> intList = new List<int>();
    foreach (int lore in LoreSystem.LoreList())
    {
      bool flag = true;
      foreach (int num in DataManager.Instance.LoreUnlocked)
      {
        if (lore == num)
          flag = false;
      }
      if (flag)
        intList.Add(lore);
    }
    if (intList.Count > 1 && intList.Contains(14))
      intList.Remove(14);
    return intList[Random.Range(0, intList.Count)];
  }
}
