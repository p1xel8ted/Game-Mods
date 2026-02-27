// Decompiled with JetBrains decompiler
// Type: MetaData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System;

#nullable disable
[Serializable]
public struct MetaData : IEquatable<MetaData>
{
  public string CultName;
  public int FollowerCount;
  public int StructureCount;
  public int DeathCount;
  public int Day;
  public int Difficulty;
  public float PlayTime;
  public bool Dungeon1Completed;
  public bool Dungeon2Completed;
  public bool Dungeon3Completed;
  public bool Dungeon4Completed;
  public bool GameBeaten;
  public int PercentageCompleted;

  public static MetaData Default(DataManager dataManager)
  {
    return new MetaData()
    {
      CultName = dataManager.CultName,
      FollowerCount = dataManager.Followers.Count,
      StructureCount = StructureManager.GetTotalHomesCount(),
      DeathCount = DataManager.Instance.Followers_Dead.Count,
      Day = dataManager.CurrentDayIndex,
      GameBeaten = dataManager.DeathCatBeaten,
      PlayTime = dataManager.TimeInGame,
      Dungeon1Completed = dataManager.DungeonCompleted(FollowerLocation.Dungeon1_1),
      Dungeon2Completed = dataManager.DungeonCompleted(FollowerLocation.Dungeon1_2),
      Dungeon3Completed = dataManager.DungeonCompleted(FollowerLocation.Dungeon1_3),
      Dungeon4Completed = dataManager.DungeonCompleted(FollowerLocation.Dungeon1_4),
      Difficulty = DifficultyManager.AllAvailableDifficulties().IndexOf<DifficultyManager.Difficulty>(DifficultyManager.Difficulty.Medium),
      PercentageCompleted = 0
    };
  }

  public override string ToString()
  {
    return $"( {string.Format(ScriptLocalization.UI.DayNumber, (object) this.Day)} | {this.FollowerCount} x {"<sprite name=\"icon_Followers\">"} )";
  }

  public bool Equals(MetaData other)
  {
    return this.CultName == other.CultName && this.FollowerCount == other.FollowerCount && this.StructureCount == other.StructureCount && this.DeathCount == other.DeathCount && this.Day == other.Day && this.Difficulty == other.Difficulty && this.PlayTime.Equals(other.PlayTime) && this.Dungeon1Completed == other.Dungeon1Completed && this.Dungeon2Completed == other.Dungeon2Completed && this.Dungeon3Completed == other.Dungeon3Completed && this.Dungeon4Completed == other.Dungeon4Completed && this.GameBeaten == other.GameBeaten && this.PercentageCompleted == other.PercentageCompleted;
  }

  public override bool Equals(object obj) => obj is MetaData other && this.Equals(other);

  public override int GetHashCode()
  {
    return ((((((((((((this.CultName != null ? this.CultName.GetHashCode() : 0) * 397 ^ this.FollowerCount) * 397 ^ this.StructureCount) * 397 ^ this.DeathCount) * 397 ^ this.Day) * 397 ^ this.Difficulty) * 397 ^ this.PlayTime.GetHashCode()) * 397 ^ this.Dungeon1Completed.GetHashCode()) * 397 ^ this.Dungeon2Completed.GetHashCode()) * 397 ^ this.Dungeon3Completed.GetHashCode()) * 397 ^ this.Dungeon4Completed.GetHashCode()) * 397 ^ this.GameBeaten.GetHashCode()) * 397 ^ this.PercentageCompleted;
  }
}
