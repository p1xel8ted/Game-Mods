// Decompiled with JetBrains decompiler
// Type: DungeonPreset
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DungeonGenerator;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[CreateAssetMenu(fileName = "DungeonPreset", menuName = "DungeonPreset", order = 3)]
public class DungeonPreset : ScriptableObject
{
  public const string DUNGEON_PRESET_PATH = "Dungeon/DungeonPresets/";
  public const int MAX_ITERATIONS = 50000;
  public int room_borders = 2;
  public Tileset tileset;
  public int dungeon_level = 1;
  public SerializedWalker main_walker = new SerializedWalker();
  public List<SerializedWalker> sub_walkers = new List<SerializedWalker>();
  public EnvironmentPreset environment_preset;

  public Tileset GetTileset()
  {
    return (Object) this.tileset == (Object) null ? Resources.Load<Tileset>("Dungeon/Tilesets/DungeonTileset") : this.tileset;
  }

  public Dungeon GenerateDungeon(SavedDungeon dungeon_save, bool need_log = false)
  {
    Dungeon.SetRandomSeed(dungeon_save.seed, dungeon_save.random_calls_count);
    Debug.Log((object) $"#dgen# Started generating dungeon with {{preset={this.name}; seed={dungeon_save.seed.ToString()}; random_calls_count={dungeon_save.random_calls_count.ToString()}}}");
    int num = 0;
    Dungeon.Init();
    DungeonPattern.InitPatternsCache();
    Dungeon t_dunge;
    Dungeon dungeon;
    do
    {
      int randomCallsCount = Dungeon.GetRandomCallsCount();
      t_dunge = new Dungeon(t_enter: new IntVector2(128 /*0x80*/, 128 /*0x80*/), room_borders: this.room_borders);
      bool flag = false;
      int t_thickness;
      if (this.main_walker.thickness % 2 == 1)
      {
        t_thickness = (this.main_walker.thickness + 1) / 2;
      }
      else
      {
        t_thickness = (this.main_walker.thickness + 2) / 2;
        flag = true;
      }
      List<DungeonRoom> dungeonRoomList = new List<DungeonRoom>();
      foreach (SerializedRoom room1 in this.main_walker.rooms)
      {
        DungeonRoom room2 = room1.GetRoom(dungeonRoomList);
        room2.given_biom_type = room1.biom_type;
        room2.given_room_type = room1.room_type;
        room2.given_room_size = room1.room_size;
        room2.given_room_interior_name = room1.preset_name;
        dungeonRoomList.Add(room2);
      }
      new DungeonWalker(true, t_dunge, dungeonRoomList, t_thickness, this.main_walker.step_length, this.main_walker.action_chances.Copy(), this.main_walker.min_length, this.main_walker.max_length, this.main_walker.max_steps_between_rooms).real_thickness = this.main_walker.thickness;
      foreach (SerializedWalker subWalker in this.sub_walkers)
      {
        List<DungeonRoom> t_rooms = new List<DungeonRoom>();
        foreach (SerializedRoom room3 in subWalker.rooms)
        {
          DungeonRoom room4 = room3.GetRoom(dungeonRoomList);
          room4.given_biom_type = room3.biom_type;
          room4.given_room_type = room3.room_type;
          room4.given_room_size = room3.room_size;
          room4.given_room_interior_name = room3.preset_name;
          t_rooms.Add(room4);
        }
        new DungeonWalker(false, t_dunge, t_rooms, t_thickness, subWalker.step_length, subWalker.action_chances.Copy(), subWalker.min_length, subWalker.max_length, subWalker.max_steps_between_rooms).real_thickness = this.main_walker.thickness;
      }
      if (num == 1357)
        Debug.Log((object) "#dgen# I'm Here");
      if (t_dunge.TryGenerateDungeon())
      {
        dungeon = t_dunge;
        if (flag)
        {
          Debug.Log((object) "#dgen# Seems like I need postproduction.");
          dungeon.MakePostproduction();
        }
        dungeon_save.random_calls_count = randomCallsCount;
        Debug.Log((object) ("#dgen# Finished Dungeon Generator. Iterations = " + (num + 1).ToString()));
        goto label_27;
      }
      ++num;
    }
    while (num <= 50000);
    Debug.LogError((object) $"Stopped Dungeon Generation By Iterator! (iterations: {num.ToString()})");
    dungeon = t_dunge;
label_27:
    return dungeon;
  }
}
