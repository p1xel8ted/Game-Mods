// Decompiled with JetBrains decompiler
// Type: DungeonGenerator.DungeonWalker
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using LinqTools;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace DungeonGenerator;

public class DungeonWalker
{
  public Dungeon dunge;
  public bool is_main_walker = true;
  public bool is_finished;
  public bool is_correct;
  public int min_path_length;
  public int max_path_length;
  public int max_steps_between_rooms;
  public int steps_after_placed_room;
  public int failed_steps_after_placed_room;
  public int cur_path;
  public int thickness;
  public int real_thickness;
  public DungeonPattern _pattern;
  public int step_length;
  public List<DungeonRoom> rooms;
  public List<DungeonRoom> placed_rooms;
  public DungeonWalker.ActionChances chances;
  public List<DungeonWalker.ActionChances.ActionType> last_tick_action_type_list = new List<DungeonWalker.ActionChances.ActionType>();
  public DungeonWalker.Direction cur_direction = DungeonWalker.Direction.Up;
  public IntVector2 cur_position = new IntVector2();

  public DungeonWalker.ActionChances.ActionType last_tick_action_type
  {
    get
    {
      return this.last_tick_action_type_list == null || this.last_tick_action_type_list.Count == 0 ? DungeonWalker.ActionChances.ActionType.Finish : this.last_tick_action_type_list[this.last_tick_action_type_list.Count - 1];
    }
    set
    {
      if (this.last_tick_action_type_list == null)
        this.last_tick_action_type_list = new List<DungeonWalker.ActionChances.ActionType>();
      this.last_tick_action_type_list.Add(value);
      if (this.last_tick_action_type_list.Count <= 20)
        return;
      this.last_tick_action_type_list.RemoveAt(0);
    }
  }

  public DungeonWalker(
    bool t_is_main,
    Dungeon t_dunge,
    List<DungeonRoom> t_rooms,
    int t_thickness = 1,
    int t_step_length = 1,
    DungeonWalker.ActionChances t_chances = null,
    int t_min_length = 128 /*0x80*/,
    int t_max_length = 256 /*0x0100*/,
    int t_max_steps_between_rooms = 3)
  {
    this.is_main_walker = t_is_main;
    this.dunge = t_dunge;
    this.rooms = t_rooms;
    this.placed_rooms = new List<DungeonRoom>();
    if (t_chances == null)
      t_chances = new DungeonWalker.ActionChances();
    this.chances = t_chances;
    if (this.is_main_walker)
    {
      this.cur_position = this.dunge.enter_to_dunge.Copy();
      this.dunge.main_walker = this;
    }
    else
    {
      this.cur_position = new IntVector2(-1, -1);
      this.dunge.sub_walkers.Add(this);
    }
    this.min_path_length = t_min_length;
    this.max_path_length = t_max_length;
    this.thickness = t_thickness;
    this._pattern = DungeonPattern.GetPattern(this.thickness);
    this.step_length = t_step_length;
    this.max_steps_between_rooms = t_max_steps_between_rooms;
  }

  public bool TryMarkStepPattern()
  {
    if (this.IsWrongPosition())
      return false;
    int num1 = this.cur_position.x - this.thickness + 1;
    int num2 = this.cur_position.y - this.thickness + 1;
    for (int index1 = 0; index1 <= this._pattern.ymax; ++index1)
    {
      int index2 = index1 << 8;
      int y = num2 + index1;
      int x = num1;
      for (int index3 = 0; index3 <= this._pattern.xmax; ++index3)
      {
        if (this._pattern.step_pattern[index2] && this.dunge.GetCellType(x, y) != Dungeon.CellType.Room)
          this.dunge.TrySetCellType(x, y, Dungeon.CellType.Corridor);
        ++index2;
        ++x;
      }
    }
    return true;
  }

  public void CalculateTick()
  {
    if (this.IsWrongPosition())
    {
      this.is_finished = true;
    }
    else
    {
      bool use_finish = this.cur_path > this.min_path_length;
      DungeonWalker.ActionChances.ActionType actionType;
      if (this.is_main_walker && this.placed_rooms.Count == 0)
        actionType = DungeonWalker.ActionChances.ActionType.PlaceRoom;
      else if (!this.is_main_walker && this.placed_rooms.Count == 0 && this.steps_after_placed_room == 0)
      {
        if (this.IsWrongDirection())
        {
          ++MainGame.me.dungeon_root.statistics.is_wrong_direction;
          this.is_correct = false;
          this.is_finished = true;
          return;
        }
        actionType = DungeonWalker.ActionChances.ActionType.DoStep;
      }
      else
        actionType = this.cur_path > this.max_path_length || this.IsDeadlock() ? DungeonWalker.ActionChances.ActionType.Finish : (!this.IsWrongDirection() ? (this.placed_rooms.Count >= this.rooms.Count || this.steps_after_placed_room < this.max_steps_between_rooms ? (this.last_tick_action_type == DungeonWalker.ActionChances.ActionType.DoStep ? this.chances.GetAction(use_finish) : DungeonWalker.ActionChances.ActionType.DoStep) : (this.placed_rooms.Count == this.rooms.Count - 1 ? DungeonWalker.ActionChances.ActionType.Finish : DungeonWalker.ActionChances.ActionType.PlaceRoom)) : this.chances.GetTurn());
      switch (actionType)
      {
        case DungeonWalker.ActionChances.ActionType.DoStep:
          if (!this.TryDoStep())
          {
            Debug.LogError((object) "Can not do step!");
            break;
          }
          ++this.cur_path;
          ++this.steps_after_placed_room;
          break;
        case DungeonWalker.ActionChances.ActionType.TurnLeft:
          this.TurnLeft();
          if (this.IsWrongDirection())
          {
            this.TurnRight();
            break;
          }
          if ((double) Dungeon.RandomRange(0.0f, 1f) < 0.25)
          {
            using (List<DungeonWalker>.Enumerator enumerator = this.dunge.sub_walkers.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                DungeonWalker current = enumerator.Current;
                if (current.cur_position.x == -1 || current.cur_position.y == -1)
                {
                  current.cur_position = this.cur_position.Copy();
                  break;
                }
              }
              break;
            }
          }
          break;
        case DungeonWalker.ActionChances.ActionType.TurnRight:
          this.TurnRight();
          if (this.IsWrongDirection())
          {
            this.TurnLeft();
            break;
          }
          if ((double) Dungeon.RandomRange(0.0f, 1f) < 0.25)
          {
            using (List<DungeonWalker>.Enumerator enumerator = this.dunge.sub_walkers.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                DungeonWalker current = enumerator.Current;
                if (current.cur_position.x == -1 || current.cur_position.y == -1)
                {
                  current.cur_position = this.cur_position.Copy();
                  break;
                }
              }
              break;
            }
          }
          break;
        case DungeonWalker.ActionChances.ActionType.PlaceRoom:
          bool flag = false;
          for (int index = 0; index < 3; ++index)
          {
            if (this.TryPlaceRoom())
            {
              flag = false;
              break;
            }
            flag = true;
            if (!this.rooms[this.placed_rooms.Count].TryChangeRoomInterior(this.cur_direction, 2 * this.thickness - 1, this.placed_rooms))
              break;
          }
          if (flag)
          {
            if (this.failed_steps_after_placed_room > this.max_steps_between_rooms / 2)
            {
              ++MainGame.me.dungeon_root.statistics.wrong_position_while_plasing_room;
              this.is_finished = true;
              this.is_correct = false;
              break;
            }
            --this.steps_after_placed_room;
            ++this.failed_steps_after_placed_room;
            break;
          }
          break;
        case DungeonWalker.ActionChances.ActionType.Finish:
          this.is_finished = true;
          if (this.placed_rooms.Count != this.rooms.Count - 1)
          {
            ++MainGame.me.dungeon_root.statistics.finish_walker_but_not_placed_all_rooms;
            this.is_correct = false;
            break;
          }
          if (this.TryPlaceRoom(true))
          {
            this.is_correct = true;
            break;
          }
          this.TurnLeft();
          if (this.TryPlaceRoom(true))
          {
            this.is_correct = true;
            break;
          }
          this.TurnRight();
          this.TurnRight();
          this.is_correct = this.TryPlaceRoom(true);
          if (!this.is_correct)
          {
            ++MainGame.me.dungeon_root.statistics.walker_can_not_place_last_room;
            break;
          }
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      this.last_tick_action_type = actionType;
    }
  }

  public bool TryPlaceRoom(bool last_room = false)
  {
    if (this.placed_rooms.Count >= this.rooms.Count)
    {
      ++MainGame.me.dungeon_root.statistics.pre_placed_all_rooms;
      return false;
    }
    bool flag = this.is_main_walker && this.placed_rooms.Count == 0;
    DungeonRoom room = this.rooms[this.placed_rooms.Count];
    IntVector2 intVector2_1 = new IntVector2(-1, -1);
    int num1 = this.cur_position.y - this.thickness + 1;
    int num2 = this.cur_position.x - this.thickness + 1;
    if (this.placed_rooms.Count == this.rooms.Count - 1 && !last_room)
    {
      ++MainGame.me.dungeon_root.statistics.pre_placing_not_last_room;
      return false;
    }
    DungeonWalker.Direction t_direction = this.cur_direction > DungeonWalker.Direction.Up ? this.cur_direction - 2 : this.cur_direction + 2;
    List<IntVector2> possibleEnters = room.room_interior.GetPossibleEnters(t_direction, 2 * this.thickness - 1);
    if (possibleEnters.Count == 0)
    {
      ++MainGame.me.dungeon_root.statistics.pre_no_possible_enters;
      return false;
    }
    foreach (IntVector2 intVector2_2 in possibleEnters)
    {
      switch (this.cur_direction)
      {
        case DungeonWalker.Direction.Left:
          intVector2_1 = new IntVector2(this.cur_position.x - room.room_width, num1 - intVector2_2.y);
          break;
        case DungeonWalker.Direction.Up:
          intVector2_1 = new IntVector2(num2 - intVector2_2.x, this.cur_position.y + 1);
          break;
        case DungeonWalker.Direction.Right:
          intVector2_1 = new IntVector2(this.cur_position.x + 1, num1 - intVector2_2.y);
          break;
        case DungeonWalker.Direction.Down:
          intVector2_1 = new IntVector2(num2 - intVector2_2.x, this.cur_position.y - room.room_height);
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      if (this.RoomCanBePlaced(room, intVector2_1, this.cur_direction))
      {
        if (!flag)
        {
          room.enters_coords.Add(intVector2_2.Copy());
          break;
        }
        break;
      }
      if (possibleEnters.IndexOf(intVector2_2) == possibleEnters.Count - 1)
      {
        ++MainGame.me.dungeon_root.statistics.pre_not_found_proper_enter;
        return false;
      }
    }
    if (intVector2_1.x <= 1 || intVector2_1.y <= 1 || intVector2_1.x + room.room_width >= this.dunge.dungeon_width || intVector2_1.y + room.room_height >= this.dunge.dungeon_height)
    {
      ++MainGame.me.dungeon_root.statistics.touched_borders_4;
      return false;
    }
    this.steps_after_placed_room = 0;
    this.failed_steps_after_placed_room = 0;
    room.is_placed = true;
    room.coords = intVector2_1;
    for (int index1 = 0; index1 < room.room_width; ++index1)
    {
      for (int index2 = 0; index2 < room.room_height; ++index2)
        this.dunge.TrySetCellType(intVector2_1.x + index1, intVector2_1.y + index2, Dungeon.CellType.Room);
    }
    if ((double) Dungeon.RandomRange(0.0f, 1f) < 0.15000000596046448)
    {
      foreach (DungeonWalker subWalker in this.dunge.sub_walkers)
      {
        if (subWalker.cur_position.x == -1 || subWalker.cur_position.y == -1)
        {
          if (subWalker.SetNewPositionAfterRoomPlaced(room, intVector2_1))
          {
            if (!subWalker.TryMarkStepPattern())
            {
              ++MainGame.me.dungeon_root.statistics.not_correctly_placed_sub_walker_after_room_placing;
              this.is_correct = false;
              this.is_finished = true;
              Debug.LogError((object) $"Wrong placed sub walker: sub_walker={subWalker.cur_position?.ToString()}; place_for_room={intVector2_1?.ToString()}");
              return false;
            }
            break;
          }
          break;
        }
      }
    }
    if (!last_room && !this.SetNewPositionAfterRoomPlaced(room, intVector2_1))
    {
      ++MainGame.me.dungeon_root.statistics.not_enough_exits_from_room;
      return false;
    }
    if (!this.TryMarkStepPattern())
    {
      ++MainGame.me.dungeon_root.statistics.can_not_mark_step_pattern_after_room_placing;
      this.is_correct = false;
      this.is_finished = true;
      return false;
    }
    this.placed_rooms.Add(room);
    if (!this.is_main_walker)
      return true;
    int count = this.placed_rooms.Count;
    return true;
  }

  public bool SetNewPositionAfterRoomPlaced(DungeonRoom placed_room, IntVector2 room_place)
  {
    List<IntVector2>[] intVector2ListArray = new List<IntVector2>[4];
    int max = 0;
    for (int t_direction = 0; t_direction < 4; ++t_direction)
    {
      intVector2ListArray[t_direction] = this.GetEntersWithoutIntersections(placed_room, (DungeonWalker.Direction) t_direction);
      max += intVector2ListArray[t_direction].Count;
    }
    if (max == 0)
      return false;
    int index1 = Dungeon.RandomRange(0, max);
    IntVector2 intVector2_1 = (IntVector2) null;
    int num1 = -1;
    for (int index2 = 0; index2 < 4; ++index2)
    {
      if (index1 >= intVector2ListArray[index2].Count)
      {
        index1 -= intVector2ListArray[index2].Count;
      }
      else
      {
        intVector2_1 = intVector2ListArray[index2][index1].Copy();
        num1 = index2;
      }
    }
    if (intVector2_1 == null)
    {
      Debug.LogError((object) "FATAL ERROR! exit_coords == null!");
      return false;
    }
    if (num1 == -1)
    {
      Debug.LogError((object) "FATAL ERROR! exit_direction == -1");
      return false;
    }
    IntVector2 intVector2_2 = intVector2_1 + room_place;
    this.cur_direction = (DungeonWalker.Direction) num1;
    int num2;
    int num3;
    switch (this.cur_direction)
    {
      case DungeonWalker.Direction.Left:
        num2 = intVector2_2.x - 1;
        num3 = intVector2_2.y + this.thickness - 1;
        break;
      case DungeonWalker.Direction.Up:
        num2 = intVector2_2.x + this.thickness - 1;
        num3 = intVector2_2.y + 1;
        break;
      case DungeonWalker.Direction.Right:
        num2 = intVector2_2.x + 1;
        num3 = intVector2_2.y + this.thickness - 1;
        break;
      case DungeonWalker.Direction.Down:
        num2 = intVector2_2.x + this.thickness - 1;
        num3 = intVector2_2.y - 1;
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
    this.cur_position.x = num2;
    this.cur_position.y = num3;
    placed_room.enters_coords.Add(intVector2_1);
    return true;
  }

  public List<IntVector2> GetEntersWithoutIntersections(
    DungeonRoom placed_room,
    DungeonWalker.Direction t_direction)
  {
    int corridor_diameter = 2 * this.thickness - 1;
    List<IntVector2> possibleEnters = placed_room.room_interior.GetPossibleEnters(t_direction, corridor_diameter);
    List<IntVector2> entersCoords = placed_room.enters_coords;
    List<IntVector2> withoutIntersections = new List<IntVector2>();
    for (int index = 0; index < possibleEnters.Count; ++index)
    {
      IntVector2 intVector2_1 = possibleEnters[index];
      bool flag = false;
      foreach (IntVector2 intVector2_2 in entersCoords)
      {
        if (intVector2_1.x == intVector2_2.x)
        {
          if (Mathf.Abs(intVector2_1.y - intVector2_2.y) <= corridor_diameter)
          {
            flag = true;
            break;
          }
        }
        else if (intVector2_1.y == intVector2_2.y && Mathf.Abs(intVector2_1.x - intVector2_2.x) <= corridor_diameter)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        withoutIntersections.Add(intVector2_1);
    }
    return withoutIntersections;
  }

  public bool RoomCanBePlaced(
    DungeonRoom room,
    IntVector2 place,
    DungeonWalker.Direction direction)
  {
    if (room.room_width < 2 * this.thickness - 1 || room.room_height < 2 * this.thickness - 1)
    {
      Debug.LogError((object) $"Wrong thickness! room.name=\"{room.room_interior.name}\"; [width, height]=[{room.room_width.ToString()}, {room.room_height.ToString()}]; thickness={(2 * this.thickness - 1).ToString()}");
      return false;
    }
    for (int index = 0; index < this.dunge.room_borders; ++index)
    {
      if (!this.CheckBorders(room.room_width + 2 * index, room.room_height + 2 * index, new IntVector2(place.x - index, place.y - index), direction))
        return false;
    }
    return true;
  }

  public bool CheckBorders(
    int room_width,
    int room_height,
    IntVector2 place,
    DungeonWalker.Direction direction)
  {
    if (place.x < 1 || place.y < 1 || place.x + room_width >= this.dunge.dungeon_width - 1 || place.y + room_height >= this.dunge.dungeon_height - 1)
    {
      ++MainGame.me.dungeon_root.statistics.touched_borders_5;
      return false;
    }
    int num1 = this.cur_position.y + this.thickness - 1;
    int num2 = this.cur_position.y - this.thickness + 1;
    int num3 = this.cur_position.x + this.thickness - 1;
    int num4 = this.cur_position.x - this.thickness + 1;
    switch (direction)
    {
      case DungeonWalker.Direction.Left:
        for (int index = 0; index < room_height + 2; ++index)
        {
          if (!this.dunge.IsEmptyCell(place.x - 1, place.y - 1 + index) || (place.y - 1 + index > num1 || place.y - 1 + index < num2) && !this.dunge.IsEmptyCell(place.x + room_height, place.y - 1 + index))
            return false;
        }
        for (int index = 0; index < room_width + 1; ++index)
        {
          if (!this.dunge.IsEmptyCell(place.x + index, place.y - 1) || !this.dunge.IsEmptyCell(place.x + index, place.y + room_height))
            return false;
        }
        break;
      case DungeonWalker.Direction.Up:
        for (int index = 0; index < room_width + 2; ++index)
        {
          if (!this.dunge.IsEmptyCell(place.x - 1 + index, place.y + room_height) || (place.x - 1 + index > num3 || place.x - 1 + index < num4) && !this.dunge.IsEmptyCell(place.x - 1 + index, place.y - 1))
            return false;
        }
        for (int index = -1; index < room_height; ++index)
        {
          if (!this.dunge.IsEmptyCell(place.x - 1, place.y + index) || !this.dunge.IsEmptyCell(place.x + room_width, place.y + index))
            return false;
        }
        break;
      case DungeonWalker.Direction.Right:
        for (int index = 0; index < room_height + 2; ++index)
        {
          if (!this.dunge.IsEmptyCell(place.x + room_width, place.y - 1 + index) || (place.y - 1 + index > num1 || place.y - 1 + index < num2) && !this.dunge.IsEmptyCell(place.x - 1, place.y - 1 + index))
            return false;
        }
        for (int index = -1; index < room_width; ++index)
        {
          if (!this.dunge.IsEmptyCell(place.x + index, place.y - 1) || !this.dunge.IsEmptyCell(place.x + index, place.y + room_height))
            return false;
        }
        break;
      case DungeonWalker.Direction.Down:
        for (int index = 0; index < room_width + 2; ++index)
        {
          if (!this.dunge.IsEmptyCell(place.x - 1 + index, place.y - 1) || (place.x - 1 + index > num3 || place.x - 1 + index < num4) && !this.dunge.IsEmptyCell(place.x - 1 + index, place.y + room_height))
            return false;
        }
        for (int index = 0; index < room_height + 1; ++index)
        {
          if (!this.dunge.IsEmptyCell(place.x - 1, place.y + index) || !this.dunge.IsEmptyCell(place.x + room_width, place.y + index))
            return false;
        }
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof (direction), (object) direction, (string) null);
    }
    return true;
  }

  public bool IsDeadlock()
  {
    if (this.last_tick_action_type_list.Count >= 20)
    {
      int num = 0;
      foreach (DungeonWalker.ActionChances.ActionType lastTickActionType in this.last_tick_action_type_list)
      {
        switch (lastTickActionType)
        {
          case DungeonWalker.ActionChances.ActionType.TurnLeft:
          case DungeonWalker.ActionChances.ActionType.TurnRight:
            ++num;
            continue;
          default:
            continue;
        }
      }
      if (num > 10)
      {
        ++MainGame.me.dungeon_root.statistics.walker_is_cycled;
        this.is_correct = false;
        this.is_finished = true;
        return true;
      }
    }
    bool flag = true;
    for (int index = 0; index < 4; ++index)
    {
      this.TurnLeft();
      flag = flag && this.IsWrongDirection();
    }
    return flag;
  }

  public bool TryDoStep()
  {
    if (this.IsWrongPosition() || this.IsWrongDirection())
      return false;
    IntVector2 intVector2;
    switch (this.cur_direction)
    {
      case DungeonWalker.Direction.Left:
        intVector2 = new IntVector2(-1);
        break;
      case DungeonWalker.Direction.Up:
        intVector2 = new IntVector2(t_y: 1);
        break;
      case DungeonWalker.Direction.Right:
        intVector2 = new IntVector2(1);
        break;
      case DungeonWalker.Direction.Down:
        intVector2 = new IntVector2(t_y: -1);
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
    for (int index = 0; index < this.step_length; ++index)
    {
      this.cur_position += intVector2;
      if (!this.TryMarkStepPattern())
      {
        ++MainGame.me.dungeon_root.statistics.can_not_mark_step_pattern_after_step;
        this.is_correct = false;
        this.is_finished = true;
        return false;
      }
    }
    return true;
  }

  public void TurnRight()
  {
    switch (this.cur_direction)
    {
      case DungeonWalker.Direction.Left:
      case DungeonWalker.Direction.Up:
      case DungeonWalker.Direction.Right:
        ++this.cur_direction;
        break;
      case DungeonWalker.Direction.Down:
        this.cur_direction = DungeonWalker.Direction.Left;
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  public void TurnLeft()
  {
    switch (this.cur_direction)
    {
      case DungeonWalker.Direction.Left:
        this.cur_direction = DungeonWalker.Direction.Down;
        break;
      case DungeonWalker.Direction.Up:
      case DungeonWalker.Direction.Right:
      case DungeonWalker.Direction.Down:
        --this.cur_direction;
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  public bool IsWrongPosition()
  {
    if (this.dunge == null || this.thickness < 1)
      return true;
    if (this.cur_position.x >= this.thickness - 1 && this.cur_position.x <= this.dunge.dungeon_width - this.thickness && this.cur_position.y >= this.thickness - 1 && this.cur_position.y <= this.dunge.dungeon_height - this.thickness)
      return false;
    ++MainGame.me.dungeon_root.statistics.touched_borders_6;
    return true;
  }

  public bool IsWrongDirection()
  {
    if (this.dunge == null)
      return false;
    if (this.cur_direction == DungeonWalker.Direction.Left)
    {
      if (this.cur_position.x - this.thickness - this.step_length < 0)
      {
        ++MainGame.me.dungeon_root.statistics.touched_borders_7;
        return true;
      }
      if (!this.dunge.IsEmptyCell(this.cur_position.x - this.thickness - this.step_length + 1, this.cur_position.y))
        return true;
      for (int index1 = 1; index1 <= this.step_length; ++index1)
      {
        for (int index2 = 0; index2 < 2 * this.thickness + 1; ++index2)
        {
          for (int index3 = 0; index3 < 2 * this.thickness + 1; ++index3)
          {
            if (this._pattern.hit_pattern[index3 + (index2 << 8)])
            {
              if (!this.dunge.IsEmptyCell(this.cur_position.x - index1 - this.thickness + index3, this.cur_position.y - this.thickness + index2))
                return true;
              break;
            }
          }
        }
      }
    }
    if (this.cur_direction == DungeonWalker.Direction.Up)
    {
      if (this.cur_position.y + this.thickness + this.step_length >= this.dunge.dungeon_height)
      {
        ++MainGame.me.dungeon_root.statistics.touched_borders_7;
        return true;
      }
      if (!this.dunge.IsEmptyCell(this.cur_position.x, this.cur_position.y + this.thickness + this.step_length - 1))
        return true;
      for (int index4 = 1; index4 <= this.step_length; ++index4)
      {
        for (int index5 = 0; index5 < 2 * this.thickness + 1; ++index5)
        {
          for (int index6 = 2 * this.thickness; index6 > 0; --index6)
          {
            if (this._pattern.hit_pattern[index5 + (index6 << 8)])
            {
              if (!this.dunge.IsEmptyCell(this.cur_position.x - this.thickness + index5, this.cur_position.y + index4 - this.thickness + index6))
                return true;
              break;
            }
          }
        }
      }
    }
    if (this.cur_direction == DungeonWalker.Direction.Right)
    {
      if (this.cur_position.x + this.thickness + this.step_length >= this.dunge.dungeon_width)
      {
        ++MainGame.me.dungeon_root.statistics.touched_borders_7;
        return true;
      }
      if (!this.dunge.IsEmptyCell(this.cur_position.x + this.thickness + this.step_length - 1, this.cur_position.y))
        return true;
      for (int index7 = 1; index7 <= this.step_length; ++index7)
      {
        for (int index8 = 0; index8 < 2 * this.thickness + 1; ++index8)
        {
          for (int index9 = 2 * this.thickness; index9 > 0; --index9)
          {
            if (this._pattern.hit_pattern[index9 + (index8 << 8)])
            {
              if (!this.dunge.IsEmptyCell(this.cur_position.x + index7 - this.thickness + index9, this.cur_position.y - this.thickness + index8))
                return true;
              break;
            }
          }
        }
      }
    }
    if (this.cur_direction == DungeonWalker.Direction.Down)
    {
      if (this.cur_position.y - this.thickness - this.step_length + 1 < 1)
      {
        ++MainGame.me.dungeon_root.statistics.touched_borders_7;
        return true;
      }
      if (!this.dunge.IsEmptyCell(this.cur_position.x, this.cur_position.y - this.thickness - this.step_length + 1))
        return true;
      for (int index10 = 1; index10 <= this.step_length; ++index10)
      {
        for (int index11 = 0; index11 < 2 * this.thickness + 1; ++index11)
        {
          for (int index12 = 0; index12 < 2 * this.thickness + 1; ++index12)
          {
            if (this._pattern.hit_pattern[index11 + (index12 << 8)])
            {
              if (!this.dunge.IsEmptyCell(this.cur_position.x - this.thickness + index11, this.cur_position.y - index10 - this.thickness + index12))
                return true;
              break;
            }
          }
        }
      }
    }
    return false;
  }

  public bool IsWrongWalker()
  {
    foreach (DungeonRoom room in this.rooms)
    {
      if (room.room_width < 2 * this.thickness || room.room_height < 2 * this.thickness)
        return true;
    }
    return false;
  }

  public enum Direction
  {
    Left,
    Up,
    Right,
    Down,
  }

  [Serializable]
  public class ActionChances
  {
    public static int max_actions = (int) (Enum.GetValues(typeof (DungeonWalker.ActionChances.ActionType)).Cast<DungeonWalker.ActionChances.ActionType>().Max<DungeonWalker.ActionChances.ActionType>() + 1);
    [SerializeField]
    public float[] chances = new float[DungeonWalker.ActionChances.max_actions];

    public ActionChances()
    {
      this.chances[0] = 0.5f;
      this.chances[1] = 0.225f;
      this.chances[2] = 0.225f;
      this.chances[3] = 0.05f;
      this.chances[4] = 0.0f;
    }

    public ActionChances(
      float do_step_chance,
      float turn_left_chance,
      float turn_right_chance,
      float place_room_chance)
    {
      this.chances[0] = do_step_chance;
      this.chances[1] = turn_left_chance;
      this.chances[2] = turn_right_chance;
      this.chances[3] = place_room_chance;
      this.chances[4] = 0.0f;
    }

    public DungeonWalker.ActionChances.ActionType GetTurn()
    {
      DungeonWalker.ActionChances.ActionType turn = (double) this.chances[1] >= (double) this.chances[2] ? DungeonWalker.ActionChances.ActionType.TurnLeft : DungeonWalker.ActionChances.ActionType.TurnRight;
      this.chances[1] += turn == DungeonWalker.ActionChances.ActionType.TurnLeft ? -0.1f : 0.1f;
      this.chances[2] += turn == DungeonWalker.ActionChances.ActionType.TurnRight ? -0.1f : 0.1f;
      return turn;
    }

    public DungeonWalker.ActionChances.ActionType GetAction(bool use_finish = false)
    {
      DungeonWalker.ActionChances.ActionType action = DungeonWalker.ActionChances.ActionType.Finish;
      float max = 0.0f;
      for (int index = 0; index < DungeonWalker.ActionChances.max_actions; ++index)
      {
        if (use_finish || index != 4)
          max += this.chances[index];
      }
      float num1 = Dungeon.RandomRange(0.0f, max);
      float num2 = 0.0f;
      for (int index = 0; index < DungeonWalker.ActionChances.max_actions; ++index)
      {
        num2 += this.chances[index];
        if ((double) num2 > (double) num1)
        {
          action = (DungeonWalker.ActionChances.ActionType) index;
          break;
        }
      }
      switch (action)
      {
        case DungeonWalker.ActionChances.ActionType.DoStep:
          this.ChangeChance(DungeonWalker.ActionChances.ActionType.DoStep, -0.05f);
          this.ChangeChance(DungeonWalker.ActionChances.ActionType.PlaceRoom, 0.05f);
          goto case DungeonWalker.ActionChances.ActionType.Finish;
        case DungeonWalker.ActionChances.ActionType.TurnLeft:
          this.ChangeChance(DungeonWalker.ActionChances.ActionType.TurnLeft, -0.1f);
          this.ChangeChance(DungeonWalker.ActionChances.ActionType.TurnRight, 0.1f);
          goto case DungeonWalker.ActionChances.ActionType.Finish;
        case DungeonWalker.ActionChances.ActionType.TurnRight:
          this.ChangeChance(DungeonWalker.ActionChances.ActionType.TurnLeft, 0.1f);
          this.ChangeChance(DungeonWalker.ActionChances.ActionType.TurnRight, -0.1f);
          goto case DungeonWalker.ActionChances.ActionType.Finish;
        case DungeonWalker.ActionChances.ActionType.PlaceRoom:
          this.ChangeChance(DungeonWalker.ActionChances.ActionType.DoStep, this.chances[3]);
          this.chances[3] = 0.0f;
          goto case DungeonWalker.ActionChances.ActionType.Finish;
        case DungeonWalker.ActionChances.ActionType.Finish:
          return action;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public void ChangeChance(DungeonWalker.ActionChances.ActionType type, float delta)
    {
      this.chances[(int) type] = (double) this.chances[(int) type] + (double) delta < 0.0 ? 0.0f : ((double) this.chances[(int) type] + (double) delta > 1.0 ? 1f : this.chances[(int) type] + delta);
    }

    public override string ToString()
    {
      string str = string.Empty;
      foreach (DungeonWalker.ActionChances.ActionType index in Enum.GetValues(typeof (DungeonWalker.ActionChances.ActionType)))
        str = $"{str}\n{index.ToString()}  \t= {this.chances[(int) index].ToString()}";
      return str;
    }

    public DungeonWalker.ActionChances Copy()
    {
      return new DungeonWalker.ActionChances(this.chances[0], this.chances[1], this.chances[2], this.chances[3]);
    }

    public enum ActionType
    {
      DoStep,
      TurnLeft,
      TurnRight,
      PlaceRoom,
      Finish,
    }
  }
}
