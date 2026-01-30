// Decompiled with JetBrains decompiler
// Type: DifficultyManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMBiomeGeneration;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class DifficultyManager
{
  [CompilerGenerated]
  public static DifficultyManager.Difficulty \u003CPrimaryDifficulty\u003Ek__BackingField = DifficultyManager.Difficulty.Medium;
  [CompilerGenerated]
  public static DifficultyManager.Difficulty \u003CSecondaryDifficulty\u003Ek__BackingField = DifficultyManager.Difficulty.Medium;
  public static DifficultyData currentDifficultyData;
  public static DifficultyData[] difficulties = new DifficultyData[0];

  public static DifficultyManager.Difficulty PrimaryDifficulty
  {
    get => DifficultyManager.\u003CPrimaryDifficulty\u003Ek__BackingField;
    set => DifficultyManager.\u003CPrimaryDifficulty\u003Ek__BackingField = value;
  }

  public static DifficultyManager.Difficulty SecondaryDifficulty
  {
    get => DifficultyManager.\u003CSecondaryDifficulty\u003Ek__BackingField;
    set => DifficultyManager.\u003CSecondaryDifficulty\u003Ek__BackingField = value;
  }

  public static bool AssistModeEnabled => true;

  public static DifficultyData CurrentDifficultyData
  {
    get
    {
      if ((UnityEngine.Object) DifficultyManager.currentDifficultyData == (UnityEngine.Object) null)
        DifficultyManager.LoadCurrentDifficulty();
      return DifficultyManager.currentDifficultyData;
    }
  }

  public static void LoadCurrentDifficulty()
  {
    if (DifficultyManager.difficulties.Length == 0)
      DifficultyManager.difficulties = Resources.LoadAll<DifficultyData>("Data/Difficulty Data");
    DifficultyManager.Difficulty currentDifficulty = DifficultyManager.DetermineCurrentDifficulty();
    DifficultyManager.currentDifficultyData = DifficultyManager.GetDifficultyData(DifficultyManager.PrimaryDifficulty, currentDifficulty);
    DifficultyManager.SecondaryDifficulty = currentDifficulty;
  }

  public static void ForceDifficulty(int difficulty)
  {
    DifficultyManager.ForceDifficulty(DifficultyManager.AllAvailableDifficulties()[Mathf.Clamp(difficulty, 0, DifficultyManager.AllAvailableDifficulties().Length - 1)]);
  }

  public static void ForceDifficulty(DifficultyManager.Difficulty difficulty)
  {
    Debug.Log((object) $"Force Difficulty - {difficulty}".Colour(Color.yellow));
    DifficultyManager.PrimaryDifficulty = difficulty;
    DifficultyManager.LoadCurrentDifficulty();
    bool flag = !GameManager.IsDungeon(PlayerFarming.Location) || (UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null && BiomeGenerator.Instance.CurrentRoom != null && BiomeGenerator.Instance.RoomEntrance != null && BiomeGenerator.Instance.RoomEntrance == BiomeGenerator.Instance.CurrentRoom && (double) DataManager.Instance.PlayerDamageReceivedThisRun <= 0.0 && (double) DataManager.Instance.PlayerDamageDealtThisRun <= 0.0;
    if (!((bool) (UnityEngine.Object) PlayerFarming.Instance & flag))
      return;
    for (int index = 0; index < PlayerFarming.playersCount; ++index)
    {
      PlayerFarming player = PlayerFarming.players[index];
      if (GameManager.IsDungeon(PlayerFarming.Location) && DataManager.Instance.PlayerFleece == 2)
      {
        player.RedHeartsTemporarilyRemoved = 0;
        player.RedHeartsTemporarilyRemoved += (player.health.PLAYER_STARTING_HEALTH + DataManager.Instance.PLAYER_HEARTS_LEVEL) / 2;
      }
      player.health.PLAYER_TOTAL_HEALTH = (float) (player.health.PLAYER_STARTING_HEALTH + DataManager.Instance.PLAYER_HEARTS_LEVEL + DataManager.Instance.PLAYER_HEALTH_MODIFIED - player.RedHeartsTemporarilyRemoved) * PlayerFleeceManager.GetHealthMultiplier();
      player.health.PLAYER_STARTING_HEALTH_CACHED = (float) player.health.PLAYER_STARTING_HEALTH;
      player.health.PLAYER_HEALTH = player.health.PLAYER_TOTAL_HEALTH;
      player.health.InitHP();
    }
  }

  public static DifficultyManager.Difficulty DetermineCurrentDifficulty()
  {
    if (!DifficultyManager.AssistModeEnabled)
      return DifficultyManager.Difficulty.Medium;
    DifficultyManager.Difficulty currentDifficulty = DifficultyManager.Difficulty.Medium;
    double playerSkillValue = (double) PlayerSkillManager.GetPlayerSkillValue();
    float playerTotal = PlayerSkillManager.GetPlayerTotal();
    if (playerSkillValue < 10.0 && (double) playerTotal > 100.0)
      currentDifficulty = DifficultyManager.Difficulty.Easy;
    if (playerSkillValue > 50.0 && (double) playerTotal > 500.0)
      currentDifficulty = DifficultyManager.Difficulty.Hard;
    return currentDifficulty;
  }

  public static DifficultyData GetDifficultyData(
    DifficultyManager.Difficulty primaryDifficulty,
    DifficultyManager.Difficulty secondaryDifficulty)
  {
    foreach (DifficultyData difficulty in DifficultyManager.difficulties)
    {
      if (difficulty.PrimaryDifficulty == primaryDifficulty && CoopManager.CoopActive && difficulty.IsCOOP || difficulty.PrimaryDifficulty == primaryDifficulty && difficulty.SecondaryDifficulty == secondaryDifficulty)
        return difficulty;
    }
    return DifficultyManager.difficulties[0];
  }

  public static float GetHealthDropsMultiplier()
  {
    return DifficultyManager.CurrentDifficultyData.HealthDropsMultiplier;
  }

  public static float GetDungeonRoomsMultiplier()
  {
    return DifficultyManager.CurrentDifficultyData.DungeonRoomMultiplier;
  }

  public static float GetChanceOfNegatingDeath()
  {
    return DifficultyManager.CurrentDifficultyData.ChanceOfNegatingDeath;
  }

  public static float GetPlayerDamageMultiplier()
  {
    return DifficultyManager.CurrentDifficultyData.PlayerDamageMultiplier;
  }

  public static float GetInvincibleTimeMultiplier()
  {
    return DifficultyManager.CurrentDifficultyData.InvincibleTimeMultiplier;
  }

  public static float GetEnemyHealthMultiplier()
  {
    return DifficultyManager.CurrentDifficultyData.EnemyHealthMultiplier;
  }

  public static float GetLuckMultiplier() => DifficultyManager.CurrentDifficultyData.LuckMultiplier;

  public static float GetTimeBetweenDissentingMultiplier()
  {
    return DifficultyManager.CurrentDifficultyData.TimeBetweenDissentingMultiplier;
  }

  public static float GetTimeBetweenFreezingMultiplier()
  {
    return DifficultyManager.CurrentDifficultyData.TimeBetweenFreezingMultiplier;
  }

  public static int GetDeathPeneltyPercentage()
  {
    return DifficultyManager.CurrentDifficultyData.DeathPeneltyPercentage;
  }

  public static float GetDripMultiplier() => DifficultyManager.CurrentDifficultyData.DripMultiplier;

  public static int GetEnemyRoundsScoreOffset()
  {
    return DifficultyManager.CurrentDifficultyData.EnemyRoundsScoreOffset;
  }

  public static int GetEscapedPeneltyPercentage()
  {
    return DifficultyManager.CurrentDifficultyData.EscapedPeneltyPercentage;
  }

  public static float GetTimeBetweenDeathMultiplier()
  {
    return DifficultyManager.CurrentDifficultyData.TimeBetweenDeathMultiplier;
  }

  public static float GetTimeBetweenIllnessMultiplier()
  {
    return DifficultyManager.CurrentDifficultyData.TimeBetweenIllnessMultiplier;
  }

  public static float GetTimeBetweenOldAgeMultiplier()
  {
    return DifficultyManager.CurrentDifficultyData.TimeBetweenOldAgeMultiplier;
  }

  public static float GetHungerDepletionMultiplier()
  {
    return DifficultyManager.CurrentDifficultyData.HungerDepletionMultiplier;
  }

  public static float GetIllnessDepletionMultiplier()
  {
    return DifficultyManager.CurrentDifficultyData.IllnessDepletionMultiplier;
  }

  public static float GetDissenterDepletionMultiplier()
  {
    return DifficultyManager.CurrentDifficultyData.DissenterDepletionMultiplier;
  }

  public static float GetFreezingDepletionMultiplier()
  {
    return DifficultyManager.CurrentDifficultyData.FreezingDepletionMultiplier;
  }

  public static DifficultyManager.Difficulty[] AllAvailableDifficulties()
  {
    return new DifficultyManager.Difficulty[4]
    {
      DifficultyManager.Difficulty.Easy,
      DifficultyManager.Difficulty.Medium,
      DifficultyManager.Difficulty.Hard,
      DifficultyManager.Difficulty.ExtraHard
    };
  }

  public static string[] GetDifficultyLocalisation()
  {
    DifficultyManager.Difficulty[] difficultyArray = DifficultyManager.AllAvailableDifficulties();
    string[] difficultyLocalisation = new string[difficultyArray.Length];
    for (int index = 0; index < difficultyArray.Length; ++index)
      difficultyLocalisation[index] = DifficultyManager.GetDifficultyLocalisation(difficultyArray[index]);
    return difficultyLocalisation;
  }

  public static string GetDifficultyLocalisation(DifficultyManager.Difficulty difficulty)
  {
    return $"UI/Settings/Game/Difficulty/{difficulty}";
  }

  [Serializable]
  public enum Difficulty
  {
    Easy = 0,
    Medium = 50, // 0x00000032
    Hard = 100, // 0x00000064
    ExtraHard = 101, // 0x00000065
  }
}
