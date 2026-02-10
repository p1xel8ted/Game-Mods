// Decompiled with JetBrains decompiler
// Type: src.Alerts.AchievementAlerts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using src.Managers;
using System;

#nullable disable
namespace src.Alerts;

public class AchievementAlerts : AlertCategory<string>
{
  public AchievementAlerts()
  {
    AchievementsWrapper.OnAchievementUnlocked += new Action<string>(this.OnAchievementUnlocked);
  }

  void object.Finalize()
  {
    try
    {
      AchievementsWrapper.OnAchievementUnlocked -= new Action<string>(this.OnAchievementUnlocked);
    }
    finally
    {
      // ISSUE: explicit finalizer call
      base.Finalize();
    }
  }

  public void OnAchievementUnlocked(string id)
  {
    if (!this.AddOnce(id))
      return;
    PersistenceManager.Save();
  }
}
