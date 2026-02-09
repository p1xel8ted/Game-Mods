// Decompiled with JetBrains decompiler
// Type: DungeonGenerator.SavedDungeonObject
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace DungeonGenerator;

[Serializable]
public class SavedDungeonObject
{
  [SerializeField]
  public string name = "";
  [SerializeField]
  public Vector2 local_position = new Vector2(-1f, -1f);
  [SerializeField]
  public SavedDungeonObject.SavedDungeonObjectType type = SavedDungeonObject.SavedDungeonObjectType.Unknown;
  [SerializeField]
  public bool _mob_is_alive = true;

  public bool mob_is_alive
  {
    get
    {
      if (this.type == SavedDungeonObject.SavedDungeonObjectType.Mob)
        return this._mob_is_alive;
      Debug.LogError((object) (this.me + "Only Mob can be alive!"));
      return false;
    }
    set
    {
      if (this.type == SavedDungeonObject.SavedDungeonObjectType.Mob)
        this._mob_is_alive = value;
      else
        Debug.LogError((object) (this.me + "Only Mob can be alive!"));
    }
  }

  public string me => $" [{this.local_position.ToString()}] {this.name}";

  public enum SavedDungeonObjectType
  {
    Unknown = -1, // 0xFFFFFFFF
    Mob = 0,
    WGO = 1,
  }
}
