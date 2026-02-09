// Decompiled with JetBrains decompiler
// Type: DungeonGenerator.SavedDungeon
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace DungeonGenerator;

[Serializable]
public class SavedDungeon
{
  [SerializeField]
  public bool is_empty = true;
  [SerializeField]
  public string dungeon_preset_name = "";
  [SerializeField]
  public int seed = -1;
  [SerializeField]
  public int random_calls_count;
  [SerializeField]
  public List<SavedDungeonObject> objects = new List<SavedDungeonObject>();
  [SerializeField]
  public bool is_completed;
}
