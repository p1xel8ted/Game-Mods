// Decompiled with JetBrains decompiler
// Type: DungeonRoomInterior
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DungeonGenerator;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DungeonRoomInterior : ScriptableObject
{
  public const string DOORS_CONTAINS_THIS_WORDS = "dungeon_exit";
  public const int TINY_ROOM = 8;
  public const int SMALL_ROOM = 11;
  public const int MEDIUM_ROOM = 14;
  public const int BIG_ROOM = 17;
  public const int HUGE_ROOM = 20;
  public string preset_name = "RoomInterior";
  public int room_height = 5;
  public int room_width = 5;
  public List<SerializedWGO> interior_objects = new List<SerializedWGO>();
  [SerializeField]
  public List<IntVectors> possible_enters_3 = new List<IntVectors>();
  [SerializeField]
  public List<IntVectors> possible_enters_4 = new List<IntVectors>();
  [SerializeField]
  public List<IntVectors> possible_enters_5 = new List<IntVectors>();
  [SerializeField]
  public List<IntVectors> possible_enters_6 = new List<IntVectors>();
  [SerializeField]
  public List<IntVectors> possible_enters_7 = new List<IntVectors>();

  public void RecalcHeightAndWidth()
  {
    int num1 = 0;
    int num2 = 0;
    foreach (SerializedWGO interiorObject in this.interior_objects)
    {
      if ((double) interiorObject.coordinates.y + 1.0 > (double) num1)
        num1 = (int) Math.Ceiling((double) interiorObject.coordinates.y) + 1;
      if ((double) interiorObject.coordinates.x + 1.0 > (double) num2)
        num2 = (int) Math.Ceiling((double) interiorObject.coordinates.x) + 1;
    }
    this.room_width = num2;
    this.room_height = num1;
  }

  public static DungeonRoomInterior GetRoomInterior(SerializedRoom t_serialized_room)
  {
    DungeonRoomInterior roomInterior = Resources.Load<DungeonRoomInterior>(t_serialized_room.GetFilename());
    if ((UnityEngine.Object) roomInterior == (UnityEngine.Object) null)
      Debug.LogError((object) $"Failed to load room: {{biom: {t_serialized_room.biom_type.ToString()}, type: {t_serialized_room.room_type}, size: {t_serialized_room.room_size.ToString()}, name: {t_serialized_room.preset_name}}}");
    return roomInterior;
  }

  public List<IntVector2> GetPossibleEnters(
    DungeonWalker.Direction t_direction,
    int corridor_diameter)
  {
    if (corridor_diameter < 3 || corridor_diameter > 7)
      return (List<IntVector2>) null;
    List<IntVectors> intVectorsList = (List<IntVectors>) null;
    switch (corridor_diameter)
    {
      case 3:
        intVectorsList = this.possible_enters_3;
        break;
      case 4:
        intVectorsList = this.possible_enters_4;
        break;
      case 5:
        intVectorsList = this.possible_enters_5;
        break;
      case 6:
        intVectorsList = this.possible_enters_6;
        break;
      case 7:
        intVectorsList = this.possible_enters_7;
        break;
      default:
        Debug.LogError((object) "WHAT THE FUCK???!!!!");
        break;
    }
    if (intVectorsList != null)
      return intVectorsList[(int) t_direction].list;
    Debug.LogError((object) "Possible enters is null!");
    return (List<IntVector2>) null;
  }

  public void SetPossibleEnters(
    List<List<IntVector2>> possible_enters_list_of_lists,
    int t_corridor_width)
  {
    if (t_corridor_width < 3 || t_corridor_width > 7)
      return;
    List<IntVectors> intVectorsList = this.possible_enters_3;
    switch (t_corridor_width)
    {
      case 3:
        this.possible_enters_3 = new List<IntVectors>();
        intVectorsList = this.possible_enters_3;
        break;
      case 4:
        this.possible_enters_4 = new List<IntVectors>();
        intVectorsList = this.possible_enters_4;
        break;
      case 5:
        this.possible_enters_5 = new List<IntVectors>();
        intVectorsList = this.possible_enters_5;
        break;
      case 6:
        this.possible_enters_6 = new List<IntVectors>();
        intVectorsList = this.possible_enters_6;
        break;
      case 7:
        this.possible_enters_7 = new List<IntVectors>();
        intVectorsList = this.possible_enters_7;
        break;
      default:
        Debug.LogError((object) "WHAT THE FUCK???!!!!");
        break;
    }
    if (possible_enters_list_of_lists == null)
      return;
    for (int index = 0; index < 4; ++index)
      intVectorsList.Add(new IntVectors(possible_enters_list_of_lists[index]));
  }

  public bool HaveAnyPossibleEnter()
  {
    foreach (IntVectors intVectors in this.possible_enters_3)
    {
      if (intVectors != null && intVectors.list.Count != 0)
        return true;
    }
    foreach (IntVectors intVectors in this.possible_enters_4)
    {
      if (intVectors != null && intVectors.list.Count != 0)
        return true;
    }
    foreach (IntVectors intVectors in this.possible_enters_5)
    {
      if (intVectors != null && intVectors.list.Count != 0)
        return true;
    }
    foreach (IntVectors intVectors in this.possible_enters_6)
    {
      if (intVectors != null && intVectors.list.Count != 0)
        return true;
    }
    foreach (IntVectors intVectors in this.possible_enters_7)
    {
      if (intVectors != null && intVectors.list.Count != 0)
        return true;
    }
    return false;
  }

  public bool HaveAnyPossibleEnter(int t_corridor_width)
  {
    List<IntVectors> intVectorsList = this.possible_enters_3;
    switch (t_corridor_width)
    {
      case 3:
        intVectorsList = this.possible_enters_3;
        break;
      case 4:
        intVectorsList = this.possible_enters_4;
        break;
      case 5:
        intVectorsList = this.possible_enters_5;
        break;
      case 6:
        intVectorsList = this.possible_enters_6;
        break;
      case 7:
        intVectorsList = this.possible_enters_7;
        break;
      default:
        Debug.LogError((object) "WHAT THE FUCK???!!!!");
        break;
    }
    foreach (IntVectors intVectors in intVectorsList)
    {
      if (intVectors != null && intVectors.list.Count != 0)
        return true;
    }
    return false;
  }

  public void DrawRoom(
    Transform parent,
    Vector2 offset,
    List<IntVector2> enters_coords,
    int enter_thickness,
    Tileset tileset,
    out List<MobSpawner> spawners,
    List<SavedDungeonObject> saved_objs,
    bool is_first_room = true)
  {
    Debug.Log((object) $"#dgen# Drawing room {this.name}[{this.preset_name}];\n offset = {offset.ToString()};\n{(enters_coords != null ? " enter_coords.Count = " + enters_coords.Count.ToString() : " enter_coords is NULL!")};\n{(saved_objs == null ? " saved_objs is NULL!" : " saved_objs.Count = " + saved_objs.Count.ToString())}");
    if ((UnityEngine.Object) tileset == (UnityEngine.Object) null && enters_coords != null && enters_coords.Count > 0)
    {
      tileset = Resources.Load<Tileset>("Dungeon/Tilesets/DungeonTileset");
      Debug.LogError((object) "Tileset is null!");
    }
    bool flag1 = true;
    if (saved_objs == null)
      flag1 = false;
    else if (saved_objs.Count == 0)
      flag1 = false;
    spawners = new List<MobSpawner>();
    foreach (SerializedWGO interiorObject in this.interior_objects)
    {
      bool flag2 = false;
      if (interiorObject.dungeon_object_chance != null && (double) interiorObject.dungeon_object_chance.chance < 1.0)
      {
        if (Application.isPlaying)
        {
          if ((Dungeon.RandomIsInitialized() ? (double) Dungeon.RandomRange(0.0f, 1f) : (double) UnityEngine.Random.Range(0.0f, 1f)) > (double) interiorObject.dungeon_object_chance.chance)
            continue;
        }
        else
          flag2 = true;
      }
      switch (interiorObject.obj_type)
      {
        case SerializedWGO.WorldObjectType.Unknown:
          Debug.LogError((object) $"WorldObjectType of {interiorObject.id} is Unknown!");
          continue;
        case SerializedWGO.WorldObjectType.WGO:
          string str = "";
          if (interiorObject.id.Contains("dungeon_exit"))
          {
            str = interiorObject.id;
            if (is_first_room)
            {
              if (str[str.Length - 1] == '2')
                str = str.Remove(str.Length - 1);
              if (Application.isPlaying)
                MainGame.me.dungeon_root.enter_to_dunge.transform.localPosition = (Vector3) (interiorObject.coordinates + offset);
            }
            else if (str[str.Length - 1] != '2')
              str += "2";
          }
          else if (flag1)
          {
            bool flag3 = true;
            foreach (SavedDungeonObject savedObj in saved_objs)
            {
              if (savedObj.type == SavedDungeonObject.SavedDungeonObjectType.WGO && !(savedObj.name != interiorObject.id))
              {
                Vector2 vector2 = interiorObject.coordinates + offset - savedObj.local_position;
                if ((double) Mathf.Abs(vector2.x) <= 0.10000000149011612 && (double) Mathf.Abs(vector2.y) <= 0.10000000149011612)
                {
                  flag3 = false;
                  break;
                }
              }
            }
            if (flag3)
              continue;
          }
          WorldGameObject worldGameObject = WorldMap.SpawnWGO(parent, string.IsNullOrEmpty(str) ? interiorObject.id : str);
          worldGameObject.transform.localPosition = (Vector3) (interiorObject.coordinates + offset);
          worldGameObject.transform.localScale = interiorObject.local_scale;
          RoundAndSortComponent roundAndSort = worldGameObject.round_and_sort;
          roundAndSort.grid_divider = interiorObject.divider;
          roundAndSort.floor_line = interiorObject.floor_line;
          worldGameObject.variation = interiorObject.variation;
          worldGameObject.variation_2 = interiorObject.variation_2;
          for (int index = 0; index < interiorObject.s_params.Count; ++index)
            worldGameObject.AddToInventory(interiorObject.s_params[index], Mathf.RoundToInt(interiorObject.f_params[index]));
          if (flag2)
            worldGameObject.gameObject.AddComponent<DungeonObjectChanceInspector>().dungeon_object_chance = interiorObject.dungeon_object_chance == null ? new DungeonObjectChance() : interiorObject.dungeon_object_chance;
          worldGameObject.Redraw();
          continue;
        case SerializedWGO.WorldObjectType.WSO:
          WorldSimpleObject original1 = Resources.Load<WorldSimpleObject>("objects/WorldSimpleObjects/" + interiorObject.id);
          if ((UnityEngine.Object) original1 == (UnityEngine.Object) null)
          {
            Debug.LogError((object) ("Can not Load WSO Prefab: " + interiorObject.id));
            continue;
          }
          WorldSimpleObject worldSimpleObject1 = original1;
          bool need_mirror = false;
          IntVector2 intVector2 = new IntVector2(Mathf.FloorToInt(interiorObject.coordinates.x), Mathf.FloorToInt(interiorObject.coordinates.y));
          if (original1.wso_type == WorldSimpleObject.WSOType.WallStraight || original1.wso_type == WorldSimpleObject.WSOType.WallCorner)
          {
            if (intVector2.x == 0)
            {
              foreach (IntVector2 entersCoord in enters_coords)
              {
                if (entersCoord.x == 0 && intVector2.y >= entersCoord.y && intVector2.y <= entersCoord.y + enter_thickness - 1)
                {
                  original1 = intVector2.y != entersCoord.y ? (intVector2.y != entersCoord.y + enter_thickness - 1 ? (WorldSimpleObject) null : tileset.GetTilePrefab(Tileset.WallType.CornerInDownRight, out need_mirror)) : tileset.GetTilePrefab(Tileset.WallType.CornerInUpRight, out need_mirror);
                  break;
                }
              }
            }
            else if (intVector2.x == this.room_width - 1)
            {
              foreach (IntVector2 entersCoord in enters_coords)
              {
                if (entersCoord.x == this.room_width - 1 && intVector2.y >= entersCoord.y && intVector2.y <= entersCoord.y + enter_thickness - 1)
                {
                  original1 = intVector2.y != entersCoord.y ? (intVector2.y != entersCoord.y + enter_thickness - 1 ? (WorldSimpleObject) null : tileset.GetTilePrefab(Tileset.WallType.CornerInDownLeft, out need_mirror)) : tileset.GetTilePrefab(Tileset.WallType.CornerInUpLeft, out need_mirror);
                  break;
                }
              }
            }
            else if (intVector2.y == 0)
            {
              foreach (IntVector2 entersCoord in enters_coords)
              {
                if (entersCoord.y == 0 && intVector2.x >= entersCoord.x && intVector2.x <= entersCoord.x + enter_thickness - 1)
                {
                  original1 = intVector2.x != entersCoord.x ? (intVector2.x != entersCoord.x + enter_thickness - 1 ? (WorldSimpleObject) null : tileset.GetTilePrefab(Tileset.WallType.CornerInUpLeft, out need_mirror)) : tileset.GetTilePrefab(Tileset.WallType.CornerInUpRight, out need_mirror);
                  break;
                }
              }
            }
            else if (intVector2.y == this.room_height - 1)
            {
              foreach (IntVector2 entersCoord in enters_coords)
              {
                if (entersCoord.y == this.room_height - 1 && intVector2.x >= entersCoord.x && intVector2.x <= entersCoord.x + enter_thickness - 1)
                {
                  original1 = intVector2.x != entersCoord.x ? (intVector2.x != entersCoord.x + enter_thickness - 1 ? (WorldSimpleObject) null : tileset.GetTilePrefab(Tileset.WallType.CornerInDownLeft, out need_mirror)) : tileset.GetTilePrefab(Tileset.WallType.CornerInDownRight, out need_mirror);
                  break;
                }
              }
            }
          }
          if (!((UnityEngine.Object) original1 == (UnityEngine.Object) null))
          {
            WorldSimpleObject worldSimpleObject2 = (WorldSimpleObject) null;
            if (Application.isPlaying)
              worldSimpleObject2 = UnityEngine.Object.Instantiate<WorldSimpleObject>(original1, parent, false);
            if ((UnityEngine.Object) worldSimpleObject2 == (UnityEngine.Object) null)
            {
              Debug.LogError((object) "Can not Instantiate new WSO!");
              continue;
            }
            RoundAndSortComponent component = worldSimpleObject2.gameObject.GetComponent<RoundAndSortComponent>();
            component.grid_divider = interiorObject.divider;
            component.floor_line = interiorObject.floor_line;
            worldSimpleObject2.transform.localPosition = (Vector3) (interiorObject.coordinates + offset);
            if (need_mirror)
            {
              Vector3 localScale = worldSimpleObject2.transform.localScale;
              localScale.x *= -1f;
              worldSimpleObject2.transform.localScale = localScale;
            }
            else if ((UnityEngine.Object) worldSimpleObject1 == (UnityEngine.Object) original1)
              worldSimpleObject2.transform.localScale = interiorObject.local_scale;
            if (flag2)
            {
              worldSimpleObject2.gameObject.AddComponent<DungeonObjectChanceInspector>().dungeon_object_chance = interiorObject.dungeon_object_chance == null ? new DungeonObjectChance() : interiorObject.dungeon_object_chance;
              continue;
            }
            continue;
          }
          continue;
        case SerializedWGO.WorldObjectType.MobSpawner:
          MobSpawner original2 = Resources.Load<MobSpawner>("objects/WorldSimpleObjects/" + interiorObject.id);
          if ((UnityEngine.Object) original2 == (UnityEngine.Object) null)
          {
            Debug.LogError((object) ("Can not Load MobSpawner Prefab: " + interiorObject.id));
            continue;
          }
          MobSpawner mobSpawner = (MobSpawner) null;
          if (Application.isPlaying)
            mobSpawner = UnityEngine.Object.Instantiate<MobSpawner>(original2, parent, false);
          if ((UnityEngine.Object) mobSpawner == (UnityEngine.Object) null)
          {
            Debug.LogError((object) "Can not Instantiate new MobSpawner!");
            continue;
          }
          RoundAndSortComponent component1 = mobSpawner.gameObject.GetComponent<RoundAndSortComponent>();
          component1.grid_divider = interiorObject.divider;
          component1.floor_line = interiorObject.floor_line;
          mobSpawner.transform.localPosition = (Vector3) (interiorObject.coordinates + offset);
          if (interiorObject.f_params != null && interiorObject.f_params.Count > 0)
            mobSpawner.chance_to_spawn = interiorObject.f_params[0];
          if (interiorObject.s_params != null && interiorObject.s_params.Count > 0)
            mobSpawner.spawner_id = interiorObject.s_params[0];
          spawners.Add(mobSpawner);
          continue;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }

  public static DungeonRoomInterior.RoomSize GetRoomSize(int room_width, int room_height)
  {
    if (room_height < 8 && room_width < 8)
      return DungeonRoomInterior.RoomSize.Tiny;
    if (room_height < 11 && room_width < 11)
      return DungeonRoomInterior.RoomSize.Small;
    if (room_height < 14 && room_width < 14)
      return DungeonRoomInterior.RoomSize.Medium;
    if (room_height < 17 && room_width < 17)
      return DungeonRoomInterior.RoomSize.Big;
    if (room_height >= 20)
      return DungeonRoomInterior.RoomSize.Huge;
    return DungeonRoomInterior.RoomSize.Huge;
  }

  public enum BiomType
  {
    Unknown = -1, // 0xFFFFFFFF
    Simple = 0,
    Mine = 1,
    Cave = 2,
    Stony = 3,
  }

  public enum RoomSize
  {
    Unknown = -1, // 0xFFFFFFFF
    Tiny = 0,
    Small = 1,
    Medium = 2,
    Big = 3,
    Huge = 4,
  }
}
