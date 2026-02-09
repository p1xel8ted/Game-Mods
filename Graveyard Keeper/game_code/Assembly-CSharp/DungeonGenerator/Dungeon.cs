// Decompiled with JetBrains decompiler
// Type: DungeonGenerator.Dungeon
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace DungeonGenerator;

public class Dungeon
{
  public const int MIN_DUNGE_HEIGHT = 16 /*0x10*/;
  public const int MIN_DUNGE_WIDTH = 16 /*0x10*/;
  public int dungeon_width;
  public int dungeon_height;
  public int room_borders;
  public IntVector2 enter_to_dunge;
  public DungeonWalker main_walker;
  public List<DungeonWalker> sub_walkers;
  public static int[] _dunge_matrix = new int[65536 /*0x010000*/];
  public static int[] _clean_matrix = new int[65536 /*0x010000*/];
  public static System.Random _rnd_float = (System.Random) null;
  public static int _rnd_float_calls_count = 0;

  public bool is_correst
  {
    get
    {
      if (!this.main_walker.is_correct)
        return false;
      foreach (DungeonWalker subWalker in this.sub_walkers)
      {
        if (!subWalker.is_finished || !subWalker.is_correct)
          return false;
      }
      return true;
    }
  }

  public List<DungeonRoom> placed_rooms
  {
    get
    {
      List<DungeonRoom> placedRooms = new List<DungeonRoom>();
      placedRooms.AddRange((IEnumerable<DungeonRoom>) this.main_walker.placed_rooms);
      foreach (DungeonWalker subWalker in this.sub_walkers)
        placedRooms.AddRange((IEnumerable<DungeonRoom>) subWalker.placed_rooms);
      return placedRooms;
    }
  }

  public static void Init()
  {
    for (int index = 0; index < Dungeon._clean_matrix.Length; ++index)
      Dungeon._clean_matrix[index] = 0;
  }

  public Dungeon(int t_width = 256 /*0x0100*/, int t_height = 256 /*0x0100*/, IntVector2 t_enter = null, int room_borders = 2)
  {
    this.dungeon_width = t_width;
    this.dungeon_height = t_height;
    this.room_borders = room_borders > 0 ? room_borders : 2;
    this.enter_to_dunge = t_enter ?? new IntVector2(this.dungeon_width / 2, this.dungeon_height / 2);
    this.sub_walkers = new List<DungeonWalker>();
  }

  public bool TryGenerateDungeon()
  {
    this.FillMatrix();
    int num1 = 0;
    while (!this.main_walker.is_finished)
    {
      this.main_walker.CalculateTick();
      ++num1;
      if (num1 > 10000)
      {
        ++MainGame.me.dungeon_root.statistics.finish_main_walker_by_iterator;
        return false;
      }
    }
    if (!this.main_walker.is_correct)
    {
      ++MainGame.me.dungeon_root.statistics.main_walker_is_not_correct;
      return false;
    }
    foreach (DungeonWalker subWalker in this.sub_walkers)
    {
      if (subWalker.cur_position.x != -1 && subWalker.cur_position.y != -1)
      {
        int num2 = 0;
        while (!subWalker.is_finished)
        {
          subWalker.CalculateTick();
          ++num2;
          if (num2 > 2000)
          {
            ++MainGame.me.dungeon_root.statistics.finish_sub_walker_by_iterator;
            return false;
          }
        }
      }
      if (!subWalker.is_correct)
      {
        ++MainGame.me.dungeon_root.statistics.sub_walker_is_not_correct;
        return false;
      }
    }
    return true;
  }

  public void FillMatrix()
  {
    Array.Copy((Array) Dungeon._clean_matrix, (Array) Dungeon._dunge_matrix, Dungeon._dunge_matrix.Length);
  }

  public bool TrySetCellType(int x, int y, Dungeon.CellType t_cell_type)
  {
    if (x < 0 || x >= this.dungeon_width || y < 0 || y >= this.dungeon_height)
    {
      ++MainGame.me.dungeon_root.statistics.touched_borders_1;
      return false;
    }
    Dungeon._dunge_matrix[x + (y << 8)] = (int) t_cell_type;
    return true;
  }

  public Dungeon.CellType GetCellType(int x, int y)
  {
    return (Dungeon.CellType) Dungeon._dunge_matrix[x + (y << 8)];
  }

  public bool TryGetCellType(int x, int y, out Dungeon.CellType cell_type)
  {
    cell_type = Dungeon.CellType.Nothing;
    if (x < 0 || x >= this.dungeon_width || y < 0 || y >= this.dungeon_height)
    {
      ++MainGame.me.dungeon_root.statistics.touched_borders_2;
      return false;
    }
    cell_type = (Dungeon.CellType) Dungeon._dunge_matrix[x + (y << 8)];
    return true;
  }

  public bool IsEmptyCell(int x, int y)
  {
    if (x >= 0 && x < this.dungeon_width && y >= 0 && y < this.dungeon_height)
      return Dungeon._dunge_matrix[x + (y << 8)] == 0;
    ++MainGame.me.dungeon_root.statistics.touched_borders_3;
    return false;
  }

  public int[,] GetMatrix()
  {
    int[,] matrix = new int[256 /*0x0100*/, 256 /*0x0100*/];
    for (int index1 = 0; index1 < 256 /*0x0100*/; ++index1)
    {
      int index2 = index1 << 8;
      for (int index3 = 0; index3 < 256 /*0x0100*/; ++index3)
      {
        matrix[index3, index1] = Dungeon._dunge_matrix[index2];
        ++index2;
      }
    }
    return matrix;
  }

  public void MakePostproduction()
  {
    for (int index1 = 0; index1 < this.dungeon_width - 1; ++index1)
    {
      for (int index2 = 0; index2 < this.dungeon_height - 1; ++index2)
      {
        if (Dungeon._dunge_matrix[index1 + (index2 << 8)] == 1 && (Dungeon._dunge_matrix[index1 + (index2 + 1 << 8)] == 0 || Dungeon._dunge_matrix[index1 + 1 + (index2 << 8)] == 0 || Dungeon._dunge_matrix[index1 + 1 + (index2 + 1 << 8)] == 0))
          Dungeon._dunge_matrix[index1 + (index2 << 8)] = 0;
      }
    }
  }

  public bool IsCorrectDungeon()
  {
    return this.dungeon_width >= 16 /*0x10*/ && this.dungeon_height >= 16 /*0x10*/;
  }

  public static float RandomRange(float min, float max)
  {
    ++Dungeon._rnd_float_calls_count;
    return (float) Dungeon._rnd_float.NextDouble() * (max - min) + min;
  }

  public static int RandomRange(int min, int max)
  {
    ++Dungeon._rnd_float_calls_count;
    int num = Mathf.FloorToInt((float) Dungeon._rnd_float.NextDouble() * (float) (max - min) + (float) min);
    if (num == max)
      --num;
    return num;
  }

  public static void SetRandomSeed(int seed, int random_calls_count)
  {
    Dungeon._rnd_float = new System.Random(seed);
    for (int index = 0; index < random_calls_count; ++index)
      Dungeon._rnd_float.NextDouble();
    Dungeon._rnd_float_calls_count = random_calls_count;
  }

  public static int GetRandomCallsCount() => Dungeon._rnd_float_calls_count;

  public static bool RandomIsInitialized() => Dungeon._rnd_float != null;

  public enum CellType
  {
    Nothing,
    Corridor,
    Room,
  }
}
