// Decompiled with JetBrains decompiler
// Type: DungeonGenerator.DungeonRoom
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace DungeonGenerator;

public class DungeonRoom
{
  public DungeonRoomInterior.BiomType given_biom_type = DungeonRoomInterior.BiomType.Unknown;
  public string given_room_type = string.Empty;
  public DungeonRoomInterior.RoomSize given_room_size = DungeonRoomInterior.RoomSize.Unknown;
  public string given_room_interior_name = string.Empty;
  public int room_width;
  public int room_height;
  public DungeonRoomInterior room_interior;
  public List<DungeonRoomInterior> wrong_room_interiors = new List<DungeonRoomInterior>();
  public bool is_placed;
  public IntVector2 coords;
  public List<IntVector2> enters_coords = new List<IntVector2>();

  public DungeonRoom(DungeonRoomInterior room_interior)
  {
    if ((Object) room_interior == (Object) null)
    {
      Debug.LogError((object) "Can not create DungeonRoom: Room interor is null!");
    }
    else
    {
      this.room_height = room_interior.room_height;
      this.room_width = room_interior.room_width;
      this.room_interior = room_interior;
    }
  }

  public DungeonRoom Copy() => new DungeonRoom(this.room_interior);

  public bool IsCorrectRoom()
  {
    return this.room_width >= 3 && this.room_height >= 3 && !((Object) this.room_interior == (Object) null);
  }

  public bool TryChangeRoomInterior(
    DungeonWalker.Direction dir,
    int cor_diam,
    List<DungeonRoom> blacklist)
  {
    if (this.is_placed || this.coords != null && this.coords != new IntVector2(0, 0) || this.enters_coords.Count != 0)
      return false;
    List<DungeonRoomInterior> dungeonRoomInteriorList = new List<DungeonRoomInterior>();
    if (blacklist != null && blacklist.Count > 0)
    {
      foreach (DungeonRoom dungeonRoom in blacklist)
      {
        if ((Object) dungeonRoom.room_interior != (Object) null)
          dungeonRoomInteriorList.Add(dungeonRoom.room_interior);
      }
    }
    this.wrong_room_interiors.Add(this.room_interior);
    DungeonRoomsContainer me = DungeonRoomsContainer.me;
    if ((Object) me == (Object) null)
    {
      Debug.LogError((object) "Rooms Container is null!");
      return false;
    }
    if (!string.IsNullOrEmpty(this.given_room_interior_name))
      return false;
    List<SerializedRoom> rooms = me.GetRooms(this.given_biom_type, this.given_room_type, this.given_room_size);
    if (rooms == null || rooms.Count == 0)
      return false;
    for (int index1 = 0; index1 < 25; ++index1)
    {
      int index2 = Dungeon.RandomRange(0, rooms.Count);
      DungeonRoomInterior roomInterior = DungeonRoomInterior.GetRoomInterior(rooms[index2]);
      if ((Object) roomInterior == (Object) null)
        return false;
      if (roomInterior.GetPossibleEnters(dir, cor_diam).Count != 0 && !dungeonRoomInteriorList.Contains(roomInterior) && !this.wrong_room_interiors.Contains(roomInterior))
      {
        this.room_interior = roomInterior;
        this.room_height = this.room_interior.room_height;
        this.room_width = this.room_interior.room_width;
        return true;
      }
    }
    return false;
  }
}
