// Decompiled with JetBrains decompiler
// Type: DifficultyData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[CreateAssetMenu(menuName = "COTL/Difficulty Data")]
public class DifficultyData : ScriptableObject
{
  public DifficultyManager.Difficulty PrimaryDifficulty;
  public DifficultyManager.Difficulty SecondaryDifficulty;
  public bool IsCOOP;
  [Header("Healing")]
  public float HealthDropsMultiplier = 1f;
  public float ChanceOfNegatingDeath;
  public float PlayerDamageMultiplier = 1f;
  public float InvincibleTimeMultiplier = 1f;
  public float EnemyHealthMultiplier = 1f;
  public int EnemyRoundsScoreOffset;
  public float LuckMultiplier = 1f;
  public float DungeonRoomMultiplier = 1f;
  public int DeathPeneltyPercentage = 30;
  public int EscapedPeneltyPercentage = 30;
  public float DripMultiplier = 1f;
  public float HungerDepletionMultiplier = 1f;
  public float IllnessDepletionMultiplier = 1f;
  public float DissenterDepletionMultiplier = 1f;
  public float FreezingDepletionMultiplier = 1f;
  public float TimeBetweenDissentingMultiplier = 1f;
  public float TimeBetweenDeathMultiplier = 1f;
  public float TimeBetweenIllnessMultiplier = 1f;
  public float TimeBetweenOldAgeMultiplier = 1f;
  public float TimeBetweenFreezingMultiplier = 1f;
}
