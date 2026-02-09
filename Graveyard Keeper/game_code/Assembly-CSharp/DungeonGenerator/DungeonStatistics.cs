// Decompiled with JetBrains decompiler
// Type: DungeonGenerator.DungeonStatistics
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;

#nullable disable
namespace DungeonGenerator;

[Serializable]
public class DungeonStatistics
{
  public int finish_main_walker_by_iterator;
  public int main_walker_is_not_correct;
  public int finish_sub_walker_by_iterator;
  public int sub_walker_is_not_correct;
  public int is_wrong_direction;
  public int wrong_position_while_plasing_room;
  public int pre_placed_all_rooms;
  public int pre_placing_not_last_room;
  public int pre_no_possible_enters;
  public int pre_not_found_proper_enter;
  public int finish_walker_but_not_placed_all_rooms;
  public int walker_can_not_place_last_room;
  public int not_correctly_placed_sub_walker_after_room_placing;
  public int not_enough_exits_from_room;
  public int can_not_mark_step_pattern_after_room_placing;
  public int walker_is_cycled;
  public int can_not_mark_step_pattern_after_step;
  public int touched_borders;
  public int touched_borders_1;
  public int touched_borders_2;
  public int touched_borders_3;
  public int touched_borders_4;
  public int touched_borders_5;
  public int touched_borders_6;
  public int touched_borders_7;
  public int touched_borders_8;
  public int touched_borders_9;

  public void SetDefault()
  {
    this.finish_main_walker_by_iterator = 0;
    this.main_walker_is_not_correct = 0;
    this.finish_sub_walker_by_iterator = 0;
    this.sub_walker_is_not_correct = 0;
    this.is_wrong_direction = 0;
    this.wrong_position_while_plasing_room = 0;
    this.pre_placed_all_rooms = 0;
    this.pre_placing_not_last_room = 0;
    this.pre_no_possible_enters = 0;
    this.pre_not_found_proper_enter = 0;
    this.finish_walker_but_not_placed_all_rooms = 0;
    this.walker_can_not_place_last_room = 0;
    this.not_correctly_placed_sub_walker_after_room_placing = 0;
    this.not_enough_exits_from_room = 0;
    this.can_not_mark_step_pattern_after_room_placing = 0;
    this.walker_is_cycled = 0;
    this.can_not_mark_step_pattern_after_step = 0;
    this.touched_borders = 0;
    this.touched_borders_1 = 0;
    this.touched_borders_2 = 0;
    this.touched_borders_3 = 0;
    this.touched_borders_4 = 0;
    this.touched_borders_5 = 0;
    this.touched_borders_6 = 0;
    this.touched_borders_7 = 0;
    this.touched_borders_8 = 0;
    this.touched_borders_9 = 0;
  }

  public override string ToString()
  {
    return $"#dgen# Dungeon statistics: \n-Global: \n finish_main_walker_by_iterator={this.finish_main_walker_by_iterator.ToString()}; \n main_walker_is_not_correct={this.main_walker_is_not_correct.ToString()}; \n finish_sub_walker_by_iterator={this.finish_sub_walker_by_iterator.ToString()}; \n sub_walker_is_not_correct={this.sub_walker_is_not_correct.ToString()}; \n-Local: \n is_wrong_direction={this.is_wrong_direction.ToString()}; \n wrong_position_while_plasing_room={this.wrong_position_while_plasing_room.ToString()}; \n - pre_placed_all_rooms={this.pre_placed_all_rooms.ToString()}; \n - pre_placing_not_last_room={this.pre_placing_not_last_room.ToString()}; \n - pre_no_possible_enters={this.pre_no_possible_enters.ToString()}; \n - pre_not_found_proper_enter={this.pre_not_found_proper_enter.ToString()}; \n finish_walker_but_not_placed_all_rooms={this.finish_walker_but_not_placed_all_rooms.ToString()};\n walker_can_not_place_last_room={this.walker_can_not_place_last_room.ToString()};\n not_correctly_placed_sub_walker_after_room_placing={this.not_correctly_placed_sub_walker_after_room_placing.ToString()}; \n not_enough_exits_from_room={this.not_enough_exits_from_room.ToString()};\n can_not_mark_step_pattern_after_room_placing={this.can_not_mark_step_pattern_after_room_placing.ToString()};\n walker_is_cycled={this.walker_is_cycled.ToString()};\n can_not_mark_step_pattern_after_step={this.can_not_mark_step_pattern_after_step.ToString()};\n";
  }
}
