// Decompiled with JetBrains decompiler
// Type: MetaData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
using RTLTMPro;
using System;
using UnityEngine;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public struct MetaData
{
  [Key(0)]
  public string CultName;
  [Key(1)]
  public int FollowerCount;
  [Key(2)]
  public int StructureCount;
  [Key(3)]
  public int DeathCount;
  [Key(4)]
  public int Day;
  [Key(5)]
  public int Difficulty;
  [Key(6)]
  public float PlayTime;
  [Key(7)]
  public bool Dungeon1Completed;
  [Key(8)]
  public bool Dungeon1NGPCompleted;
  [Key(9)]
  public bool Dungeon2Completed;
  [Key(10)]
  public bool Dungeon2NGPCompleted;
  [Key(11)]
  public bool Dungeon3Completed;
  [Key(12)]
  public bool Dungeon3NGPCompleted;
  [Key(13)]
  public bool Dungeon4Completed;
  [Key(14)]
  public bool Dungeon4NGPCompleted;
  [Key(15)]
  public bool GameBeaten;
  [Key(16 /*0x10*/)]
  public bool SandboxBeaten;
  [Key(17)]
  public bool DeathCatRecruited;
  [Key(18)]
  public bool WolfBeaten;
  [Key(19)]
  public bool YngyaBeaten;
  [Key(20)]
  public bool ExecutionerBeaten;
  [Key(21)]
  public int RottingFollowerCount;
  [Key(22)]
  public int LambGhostsCount;
  [Key(23)]
  public int WinterCount;
  [Key(24)]
  public int PercentageCompleted;
  [Key(25)]
  public int DLCPercentageCompleted;
  [Key(26)]
  public bool Permadeath;
  [Key(27)]
  public bool QuickStart;
  [Key(28)]
  public bool Penitence;
  [Key(29)]
  public string Version;
  [Key(30)]
  public bool ActivatedMajorDLC;

  public static MetaData Default(DataManager dataManager)
  {
    return new MetaData()
    {
      CultName = dataManager.CultName,
      FollowerCount = dataManager.Followers.Count,
      StructureCount = StructureManager.GetTotalHomesCount(),
      DeathCount = dataManager.Followers_Dead.Count,
      Day = dataManager.CurrentDayIndex,
      GameBeaten = dataManager.DeathCatBeaten,
      SandboxBeaten = dataManager.CompletedSandbox,
      PlayTime = dataManager.TimeInGame,
      Dungeon1Completed = dataManager.DungeonCompleted(FollowerLocation.Dungeon1_1),
      Dungeon2Completed = dataManager.DungeonCompleted(FollowerLocation.Dungeon1_2),
      Dungeon3Completed = dataManager.DungeonCompleted(FollowerLocation.Dungeon1_3),
      Dungeon4Completed = dataManager.DungeonCompleted(FollowerLocation.Dungeon1_4),
      Dungeon1NGPCompleted = dataManager.DungeonCompleted(FollowerLocation.Dungeon1_1, true),
      Dungeon2NGPCompleted = dataManager.DungeonCompleted(FollowerLocation.Dungeon1_2, true),
      Dungeon3NGPCompleted = dataManager.DungeonCompleted(FollowerLocation.Dungeon1_3, true),
      Dungeon4NGPCompleted = dataManager.DungeonCompleted(FollowerLocation.Dungeon1_4, true),
      DeathCatRecruited = dataManager.HasDeathCatFollower(),
      Difficulty = DifficultyManager.AllAvailableDifficulties().IndexOf<DifficultyManager.Difficulty>(DifficultyManager.Difficulty.Medium),
      PercentageCompleted = 0,
      DLCPercentageCompleted = 0,
      Permadeath = false,
      QuickStart = false,
      Penitence = false,
      Version = Application.version,
      ActivatedMajorDLC = dataManager.SeasonsActive,
      RottingFollowerCount = dataManager.GetRottingFollowers(),
      LambGhostsCount = dataManager.TotalShrineGhostJuice,
      WinterCount = dataManager.WintersOccured,
      ExecutionerBeaten = DataManager.Instance.BeatenExecutioner,
      WolfBeaten = DataManager.Instance.BeatenWolf,
      YngyaBeaten = DataManager.Instance.BeatenYngya
    };
  }

  public override string ToString()
  {
    if (!(LocalizationManager.CurrentLanguage == "Arabic"))
      return $"( {string.Format(ScriptLocalization.UI.DayNumber, (object) this.Day)} | {this.FollowerCount} x {"<sprite name=\"icon_Followers\">"} )";
    FastStringBuilder fastStringBuilder = new FastStringBuilder(this.Day.ToString());
    fastStringBuilder.Reverse();
    string str1 = fastStringBuilder.ToString();
    fastStringBuilder.Clear();
    fastStringBuilder.SetValue(this.FollowerCount.ToString());
    fastStringBuilder.Reverse();
    string str2 = fastStringBuilder.ToString();
    return $") {string.Format(ScriptLocalization.UI.DayNumber, (object) str1)} | <sprite name=\"icon_Followers\"> x {str2} (";
  }
}
