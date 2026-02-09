// Decompiled with JetBrains decompiler
// Type: DungeonGenerator.Tileset
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace DungeonGenerator;

[CreateAssetMenu(fileName = "Tileset", menuName = "Tileset", order = 2)]
public class Tileset : ScriptableObject
{
  public const string DEFAULT_TILESET_NAME = "DungeonTileset";
  public const string TILESET_PATH = "Dungeon/Tilesets/";
  public WSOList wall_up;
  public WSOList wall_down;
  public WSOList wall_left;
  public bool wall_right_mirror;
  public WSOList wall_right;
  public WSOList wall_corner_out_up_left;
  public bool wall_corner_out_up_right_mirror;
  public WSOList wall_corner_out_up_right;
  public WSOList wall_corner_out_down_left;
  public bool wall_corner_out_down_right_mirror;
  public WSOList wall_corner_out_down_right;
  public WSOList wall_corner_in_up_left;
  public bool wall_corner_in_up_right_mirror;
  public WSOList wall_corner_in_up_right;
  public WSOList wall_corner_in_down_left;
  public bool wall_corner_in_down_right_mirror;
  public WSOList wall_corner_in_down_right;
  public WSOList floor;

  public WorldSimpleObject GetTilePrefab(Tileset.WallType t_wall_type, out bool need_mirror)
  {
    need_mirror = false;
    WSOList wsoList;
    switch (t_wall_type)
    {
      case Tileset.WallType.Unknown:
        return (WorldSimpleObject) null;
      case Tileset.WallType.Up:
        wsoList = this.wall_up;
        break;
      case Tileset.WallType.Down:
        wsoList = this.wall_down;
        break;
      case Tileset.WallType.Left:
        wsoList = this.wall_left;
        break;
      case Tileset.WallType.Right:
        wsoList = this.wall_right_mirror ? this.wall_left : this.wall_right;
        need_mirror = this.wall_right_mirror;
        break;
      case Tileset.WallType.CornerOutUpLeft:
        wsoList = this.wall_corner_out_up_left;
        break;
      case Tileset.WallType.CornerOutUpRight:
        wsoList = this.wall_corner_out_up_right_mirror ? this.wall_corner_out_up_left : this.wall_corner_out_up_right;
        need_mirror = this.wall_corner_out_up_right_mirror;
        break;
      case Tileset.WallType.CornerOutDownLeft:
        wsoList = this.wall_corner_out_down_left;
        break;
      case Tileset.WallType.CornerOutDownRight:
        wsoList = this.wall_corner_out_down_right_mirror ? this.wall_corner_out_down_left : this.wall_corner_out_down_right;
        need_mirror = this.wall_corner_out_down_right_mirror;
        break;
      case Tileset.WallType.CornerInUpLeft:
        wsoList = this.wall_corner_in_up_left;
        break;
      case Tileset.WallType.CornerInUpRight:
        wsoList = this.wall_corner_in_up_right_mirror ? this.wall_corner_in_up_left : this.wall_corner_in_up_right;
        need_mirror = this.wall_corner_in_up_right_mirror;
        break;
      case Tileset.WallType.CornerInDownLeft:
        wsoList = this.wall_corner_in_down_left;
        break;
      case Tileset.WallType.CornerInDownRight:
        wsoList = this.wall_corner_in_down_right_mirror ? this.wall_corner_in_down_left : this.wall_corner_in_down_right;
        need_mirror = this.wall_corner_in_down_right_mirror;
        break;
      case Tileset.WallType.Floor:
        wsoList = this.floor;
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof (t_wall_type), (object) t_wall_type, (string) null);
    }
    return (UnityEngine.Object) wsoList == (UnityEngine.Object) null ? (WorldSimpleObject) null : wsoList.GetRandomWSO();
  }

  public enum WallType
  {
    Unknown,
    Up,
    Down,
    Left,
    Right,
    CornerOutUpLeft,
    CornerOutUpRight,
    CornerOutDownLeft,
    CornerOutDownRight,
    CornerInUpLeft,
    CornerInUpRight,
    CornerInDownLeft,
    CornerInDownRight,
    Floor,
  }
}
