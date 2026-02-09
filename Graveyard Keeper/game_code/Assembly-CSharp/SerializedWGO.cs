// Decompiled with JetBrains decompiler
// Type: SerializedWGO
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DungeonGenerator;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class SerializedWGO
{
  public string id = "";
  public Vector2 coordinates = Vector2.zero;
  public int divider = 1;
  public float floor_line;
  public Vector3 local_scale = Vector3.zero;
  public SerializedWGO.WorldObjectType obj_type;
  public List<string> s_params = new List<string>();
  public List<float> f_params = new List<float>();
  public int variation;
  public int variation_2;
  public DungeonObjectChance dungeon_object_chance = new DungeonObjectChance();
  public Item data;

  public enum WorldObjectType
  {
    Unknown,
    WGO,
    WSO,
    MobSpawner,
  }
}
