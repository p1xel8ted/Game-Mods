// Decompiled with JetBrains decompiler
// Type: SavedDungeonsList
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DungeonGenerator;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class SavedDungeonsList
{
  [SerializeField]
  public List<SavedDungeon> _saved_dungeons = new List<SavedDungeon>();
  [SerializeField]
  public int _global_seed = -1;

  public int GetDungeonSeed(int dungeon_level)
  {
    int dungeonSeed = -1;
    if (dungeon_level < 1)
      return dungeonSeed;
    if (this._global_seed == -1)
    {
      dungeonSeed = UnityEngine.Random.Range(0, 10000);
    }
    else
    {
      System.Random random = new System.Random(this._global_seed);
      for (int index = 0; index < dungeon_level; ++index)
        dungeonSeed = random.Next(0, 100000);
    }
    return dungeonSeed;
  }

  public SavedDungeon GetSavedDungeon(int dungeon_level)
  {
    if (this._saved_dungeons.Count < dungeon_level)
    {
      for (int count = this._saved_dungeons.Count; count < dungeon_level; ++count)
        this._saved_dungeons.Add(new SavedDungeon());
    }
    return this._saved_dungeons[dungeon_level - 1];
  }

  public void SetGlobalSeed(int new_global_seed)
  {
    if (new_global_seed < 0)
      return;
    Debug.Log((object) $"Dungeon Generation: Changed global seed [{this._global_seed.ToString()} => {new_global_seed.ToString()}]");
    this._global_seed = new_global_seed;
  }
}
