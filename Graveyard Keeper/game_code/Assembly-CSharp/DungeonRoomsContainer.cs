// Decompiled with JetBrains decompiler
// Type: DungeonRoomsContainer
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DungeonGenerator;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DungeonRoomsContainer : ScriptableObject
{
  public const string ASSET_NAME = "Dungeon/Rooms/DungeonRoomsContainer";
  public List<SerializedRoom> rooms_list;
  public Dictionary<DungeonRoomInterior.BiomType, Dictionary<string, Dictionary<DungeonRoomInterior.RoomSize, List<string>>>> _rooms_dictionary;
  public static DungeonRoomsContainer _me;

  public static DungeonRoomsContainer me
  {
    get
    {
      if ((Object) DungeonRoomsContainer._me == (Object) null)
        DungeonRoomsContainer._me = Resources.Load<DungeonRoomsContainer>("Dungeon/Rooms/DungeonRoomsContainer");
      return DungeonRoomsContainer._me;
    }
  }

  public void FillRoomsDictionary()
  {
    this._rooms_dictionary = new Dictionary<DungeonRoomInterior.BiomType, Dictionary<string, Dictionary<DungeonRoomInterior.RoomSize, List<string>>>>();
    foreach (SerializedRoom rooms in this.rooms_list)
    {
      Dictionary<string, Dictionary<DungeonRoomInterior.RoomSize, List<string>>> dictionary1 = (Dictionary<string, Dictionary<DungeonRoomInterior.RoomSize, List<string>>>) null;
      if (!this._rooms_dictionary.TryGetValue(rooms.biom_type, out dictionary1))
      {
        dictionary1 = new Dictionary<string, Dictionary<DungeonRoomInterior.RoomSize, List<string>>>();
        this._rooms_dictionary.Add(rooms.biom_type, dictionary1);
      }
      Dictionary<DungeonRoomInterior.RoomSize, List<string>> dictionary2 = (Dictionary<DungeonRoomInterior.RoomSize, List<string>>) null;
      if (!dictionary1.TryGetValue(rooms.room_type, out dictionary2))
      {
        dictionary2 = new Dictionary<DungeonRoomInterior.RoomSize, List<string>>();
        dictionary1.Add(rooms.room_type, dictionary2);
      }
      List<string> stringList = (List<string>) null;
      if (!dictionary2.TryGetValue(rooms.room_size, out stringList))
      {
        stringList = new List<string>();
        dictionary2.Add(rooms.room_size, stringList);
      }
      if (stringList.Contains(rooms.preset_name))
        Debug.LogError((object) ("Found dublicate room name: " + rooms.preset_name));
      else
        stringList.Add(rooms.preset_name);
    }
    Debug.Log((object) "Filled Rooms Dictionary.");
  }

  public static void FillRoomsDict()
  {
    DungeonRoomsContainer me = DungeonRoomsContainer.me;
    if ((Object) me == (Object) null)
      Debug.LogError((object) "Can not fill rooms dictionary: Container is null!!!");
    else
      me.FillRoomsDictionary();
  }

  public bool NeedFillDict() => this._rooms_dictionary == null;

  public static bool NeedFillDictionary()
  {
    DungeonRoomsContainer me = DungeonRoomsContainer.me;
    if (!((Object) me == (Object) null))
      return me.NeedFillDict();
    Debug.LogError((object) "Can not fill rooms dictionary: Container is null!!!");
    return true;
  }

  public List<SerializedRoom> GetRooms(
    DungeonRoomInterior.BiomType t_biom_type,
    string t_room_type,
    DungeonRoomInterior.RoomSize t_room_size)
  {
    if (t_biom_type == DungeonRoomInterior.BiomType.Unknown)
      return (List<SerializedRoom>) null;
    List<SerializedRoom> rooms = new List<SerializedRoom>();
    Dictionary<string, Dictionary<DungeonRoomInterior.RoomSize, List<string>>> dictionary1;
    if (!this._rooms_dictionary.TryGetValue(t_biom_type, out dictionary1))
      return (List<SerializedRoom>) null;
    if (string.IsNullOrEmpty(t_room_type))
    {
      foreach (KeyValuePair<string, Dictionary<DungeonRoomInterior.RoomSize, List<string>>> keyValuePair1 in dictionary1)
      {
        foreach (KeyValuePair<DungeonRoomInterior.RoomSize, List<string>> keyValuePair2 in keyValuePair1.Value)
        {
          foreach (string str in keyValuePair2.Value)
            rooms.Add(new SerializedRoom()
            {
              biom_type = t_biom_type,
              room_type = keyValuePair1.Key,
              room_size = keyValuePair2.Key,
              preset_name = str
            });
        }
      }
      return rooms;
    }
    Dictionary<DungeonRoomInterior.RoomSize, List<string>> dictionary2;
    if (!dictionary1.TryGetValue(t_room_type, out dictionary2))
      return (List<SerializedRoom>) null;
    if (t_room_size == DungeonRoomInterior.RoomSize.Unknown)
    {
      foreach (KeyValuePair<DungeonRoomInterior.RoomSize, List<string>> keyValuePair in dictionary2)
      {
        foreach (string str in keyValuePair.Value)
          rooms.Add(new SerializedRoom()
          {
            biom_type = t_biom_type,
            room_type = t_room_type,
            room_size = keyValuePair.Key,
            preset_name = str
          });
      }
      return rooms;
    }
    List<string> stringList;
    if (!dictionary2.TryGetValue(t_room_size, out stringList))
      return (List<SerializedRoom>) null;
    foreach (string str in stringList)
      rooms.Add(new SerializedRoom()
      {
        biom_type = t_biom_type,
        room_type = t_room_type,
        room_size = t_room_size,
        preset_name = str
      });
    return rooms;
  }

  public SerializedRoom GetRandomRoomName(
    DungeonRoomInterior.BiomType t_biom_type,
    string t_room_type,
    DungeonRoomInterior.RoomSize t_room_size)
  {
    if (t_biom_type == DungeonRoomInterior.BiomType.Unknown)
      return this.GetAbsolutelyRandomRoomName();
    List<SerializedRoom> rooms = this.GetRooms(t_biom_type, t_room_type, t_room_size);
    if (rooms == null || rooms.Count == 0)
      return (SerializedRoom) null;
    int index = Dungeon.RandomRange(0, rooms.Count);
    return rooms[index];
  }

  public SerializedRoom GetRandomRoomName(
    DungeonRoomInterior.BiomType t_biom_type,
    string t_room_type)
  {
    return this.GetRandomRoomName(t_biom_type, t_room_type, DungeonRoomInterior.RoomSize.Unknown);
  }

  public SerializedRoom GetRandomRoomName(DungeonRoomInterior.BiomType t_biom_type)
  {
    return this.GetRandomRoomName(t_biom_type, (string) null, DungeonRoomInterior.RoomSize.Unknown);
  }

  public SerializedRoom GetAbsolutelyRandomRoomName()
  {
    return this.rooms_list[Dungeon.RandomRange(0, this.rooms_list.Count)];
  }
}
