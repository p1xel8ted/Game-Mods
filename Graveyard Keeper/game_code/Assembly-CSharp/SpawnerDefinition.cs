// Decompiled with JetBrains decompiler
// Type: SpawnerDefinition
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class SpawnerDefinition : BalanceBaseObject
{
  public string spawner_id;
  public int dungeon_level;
  public List<SpawnerDefinition.MobDefinition> mobs;

  public SpawnerDefinition.MobDefinition GetMobToSpawn()
  {
    if (this.mobs == null || this.mobs.Count == 0)
      return (SpawnerDefinition.MobDefinition) null;
    int maxInclusive = 0;
    foreach (SpawnerDefinition.MobDefinition mob in this.mobs)
      maxInclusive += mob.weight;
    float num1 = UnityEngine.Random.Range(0.0f, (float) maxInclusive);
    float num2 = 0.0f;
    SpawnerDefinition.MobDefinition mobToSpawn = (SpawnerDefinition.MobDefinition) null;
    foreach (SpawnerDefinition.MobDefinition mob in this.mobs)
    {
      num2 += (float) mob.weight;
      if ((double) num1 <= (double) num2)
      {
        mobToSpawn = mob;
        break;
      }
    }
    if (mobToSpawn == null)
      Debug.LogError((object) "OMG WHAT THE FUCK???!!!");
    return mobToSpawn;
  }

  [Serializable]
  public class MobDefinition
  {
    public string mob_name;
    public int weight;
    public int mobs_count;
    public string craft_name;
  }
}
