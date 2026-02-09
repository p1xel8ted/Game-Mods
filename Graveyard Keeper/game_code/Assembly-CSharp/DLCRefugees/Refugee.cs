// Decompiled with JetBrains decompiler
// Type: DLCRefugees.Refugee
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace DLCRefugees;

[Serializable]
public class Refugee
{
  [NonSerialized]
  public WorldGameObject _world_game_object;
  [SerializeField]
  public long _unique_id;
  [SerializeField]
  public string _home_gd_point_tag;

  public WorldGameObject world_game_object => this._world_game_object;

  public long unique_id => this._unique_id;

  public string home_gd_point_tag
  {
    get => this._home_gd_point_tag;
    set => this._home_gd_point_tag = value;
  }

  public Refugee()
  {
  }

  public Refugee(WorldGameObject world_game_object)
  {
    this._world_game_object = world_game_object;
    this._unique_id = world_game_object.unique_id;
  }

  public void Init()
  {
    this._world_game_object = WorldMap.GetWorldGameObjectByUniqueId(this._unique_id);
  }
}
