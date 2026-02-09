// Decompiled with JetBrains decompiler
// Type: AchievementsSystem
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Steamworks;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class AchievementsSystem
{
  [SerializeField]
  public List<string> _completed_fully = new List<string>();
  [SerializeField]
  public List<string> _completed = new List<string>();
  [SerializeField]
  public List<int> _completed_n = new List<int>();

  public void CheckKeyQuests(string key, int increment = 1)
  {
    foreach (AchievementDefinition ach in GameBalance.me.achievements_data)
    {
      if (ach.start_key.Contains(key) && !this._completed_fully.Contains(ach.id) && ach.IsSucceed())
      {
        if (!this._completed.Contains(ach.id))
        {
          this._completed.Add(ach.id);
          this._completed_n.Add(increment);
        }
        else
          this._completed_n[this._completed.IndexOf(ach.id)] += increment;
        if (this._completed_n[this._completed.IndexOf(ach.id)] >= ach.counter)
        {
          this._completed_fully.Add(ach.id);
          PlatformSpecific.OnAchievementComplete(ach);
        }
      }
    }
  }

  public void VerifyAndSetMissedAchievements()
  {
    if (!SteamManager.Initialized)
      return;
    foreach (AchievementDefinition ach in GameBalance.me.achievements_data)
    {
      if (this._completed_fully.Contains(ach.id))
      {
        bool pbAchieved;
        SteamUserStats.GetAchievement(ach.id, out pbAchieved);
        Debug.Log((object) $"#MSA# Ach:[{ach.id}], counter:[{ach.counter}], achieved:[{pbAchieved}]");
        if (!pbAchieved)
          PlatformSpecific.OnAchievementComplete(ach);
      }
    }
  }
}
