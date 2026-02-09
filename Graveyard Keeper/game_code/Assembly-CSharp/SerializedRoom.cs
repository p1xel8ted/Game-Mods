// Decompiled with JetBrains decompiler
// Type: SerializedRoom
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DungeonGenerator;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

#nullable disable
[Serializable]
public class SerializedRoom
{
  [SerializeField]
  public string room_type = "";
  [SerializeField]
  public DungeonRoomInterior.RoomSize room_size = DungeonRoomInterior.RoomSize.Unknown;
  [SerializeField]
  public DungeonRoomInterior.BiomType biom_type = DungeonRoomInterior.BiomType.Unknown;
  [SerializeField]
  public string preset_name = "";
  public static Dictionary<string, Dictionary<int, Dictionary<int, Dictionary<string, string>>>> _names_hash = new Dictionary<string, Dictionary<int, Dictionary<int, Dictionary<string, string>>>>();
  public static StringBuilder path_sb = new StringBuilder();

  public DungeonRoom GetRoom(List<DungeonRoom> blacklist)
  {
    DungeonRoomsContainer me = DungeonRoomsContainer.me;
    if ((UnityEngine.Object) me == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Rooms Container is null!");
      return (DungeonRoom) null;
    }
    DungeonRoom room = this.biom_type != DungeonRoomInterior.BiomType.Unknown ? (!string.IsNullOrEmpty(this.room_type) ? (this.room_size != DungeonRoomInterior.RoomSize.Unknown ? (!string.IsNullOrEmpty(this.preset_name) ? new DungeonRoom(DungeonRoomInterior.GetRoomInterior(this)) : new DungeonRoom(DungeonRoomInterior.GetRoomInterior(me.GetRandomRoomName(this.biom_type, this.room_type, this.room_size)))) : new DungeonRoom(DungeonRoomInterior.GetRoomInterior(me.GetRandomRoomName(this.biom_type, this.room_type)))) : new DungeonRoom(DungeonRoomInterior.GetRoomInterior(me.GetRandomRoomName(this.biom_type)))) : new DungeonRoom(DungeonRoomInterior.GetRoomInterior(me.GetAbsolutelyRandomRoomName()));
    if (room != null)
    {
      room.given_biom_type = this.biom_type;
      room.given_room_type = this.room_type;
      room.given_room_size = this.room_size;
      room.given_room_interior_name = this.preset_name;
    }
    return room;
  }

  public string GetFilename()
  {
    Dictionary<int, Dictionary<int, Dictionary<string, string>>> dictionary1;
    if (!SerializedRoom._names_hash.TryGetValue(this.room_type, out dictionary1))
    {
      dictionary1 = new Dictionary<int, Dictionary<int, Dictionary<string, string>>>();
      SerializedRoom._names_hash.Add(this.room_type, dictionary1);
    }
    Dictionary<int, Dictionary<string, string>> dictionary2;
    if (!dictionary1.TryGetValue((int) this.room_size, out dictionary2))
    {
      dictionary2 = new Dictionary<int, Dictionary<string, string>>();
      dictionary1.Add((int) this.room_size, dictionary2);
    }
    Dictionary<string, string> dictionary3;
    if (!dictionary2.TryGetValue((int) this.biom_type, out dictionary3))
    {
      dictionary3 = new Dictionary<string, string>();
      dictionary2.Add((int) this.biom_type, dictionary3);
    }
    string filename;
    if (!dictionary3.TryGetValue(this.preset_name, out filename))
    {
      SerializedRoom.path_sb.Length = 0;
      SerializedRoom.path_sb.Append("Dungeon/Rooms/");
      SerializedRoom.path_sb.Append((object) this.biom_type);
      SerializedRoom.path_sb.Append("/");
      SerializedRoom.path_sb.Append(this.room_type);
      SerializedRoom.path_sb.Append("/");
      SerializedRoom.path_sb.Append((object) this.room_size);
      SerializedRoom.path_sb.Append("/");
      SerializedRoom.path_sb.Append(this.preset_name);
      filename = SerializedRoom.path_sb.ToString();
      dictionary3.Add(this.preset_name, filename);
    }
    return filename;
  }
}
