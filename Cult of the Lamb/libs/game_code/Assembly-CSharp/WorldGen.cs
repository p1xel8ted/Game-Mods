// Decompiled with JetBrains decompiler
// Type: WorldGen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class WorldGen : BaseMonoBehaviour
{
  public int Seed;
  public System.Random RandomSeed;
  public static bool WorldGenerated;
  public int width;
  public int height;
  public static int WIDTH;
  public static int HEIGHT;
  public int roomDist = 30;
  public GameObject room;
  public GameObject link;
  public static List<Room> rooms;
  public bool TutorialRooms;
  public bool StartAtCrossRoad = true;
  public WorldGen.StartingRoom StartRoom;
  public bool InstantiatePrefabs = true;
  public int NumRoomsOfInterest = 2;
  public static Room startRoom;
  public static WorldGen Instance;
  public int TreeCount;
  public int RockCount;
  public int SeedFlowerCount;
  public int CottonPlantCount;
  public int FollowerCount;
  [HideInInspector]
  public int NumRegions;

  public event WorldGen.WorldGeneratedAction OnWorldGenerated;

  public void Awake()
  {
    if ((UnityEngine.Object) WorldGen.Instance != (UnityEngine.Object) null)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }
    else
    {
      WorldGen.Instance = this;
      UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) this.gameObject);
    }
  }

  public void OnDestroy() => WorldGen.Instance = (WorldGen) null;

  public static void ClearGeneratedWorld()
  {
    WorldGen.ResetGeneratedWorldData();
    WorldGen.WorldGenerated = false;
    if ((UnityEngine.Object) WorldGen.Instance != (UnityEngine.Object) null)
    {
      int index = -1;
      while (++index < WorldGen.Instance.transform.childCount)
        UnityEngine.Object.Destroy((UnityEngine.Object) WorldGen.Instance.transform.GetChild(index).gameObject);
    }
    if (WorldGen.rooms == null)
      return;
    WorldGen.rooms.Clear();
  }

  public static void ResetGeneratedWorldData()
  {
    RoomManager.CurrentX = -1;
    RoomManager.CurrentY = -1;
    RoomManager.PrevCurrentX = -1;
    RoomManager.PrevCurrentY = -1;
    Room.PointOfInterestCount = -1;
    RoomManager.r = (Room) null;
  }

  public void CreateTutorialRooms()
  {
    WorldGen.WIDTH = 3;
    WorldGen.HEIGHT = 1;
    WorldGen.rooms = new List<Room>();
    int num1 = -1;
    int num2 = 0;
    while (++num1 < 3)
    {
      GameObject gameObject = new GameObject();
      gameObject.transform.position = new Vector3((float) (num1 * this.roomDist - this.width) + 0.5f, 0.0f, (float) (num2 * this.roomDist - this.height) + 0.5f);
      Room room = gameObject.AddComponent<Room>();
      room.x = num1;
      room.y = 0;
      room.Structures = new List<StructuresData>();
      gameObject.transform.parent = this.gameObject.transform;
      gameObject.name = $"Room: {num1.ToString()}_{num2.ToString()}";
      room.CurrentSpecificRoom = (Room.SpecificRoom) (8 + num1);
      room.NewPointOfInterestRoom();
      WorldGen.rooms.Add(room);
    }
    WorldGen.startRoom = WorldGen.rooms[0];
    WorldGen.rooms[0].E_Link = WorldGen.rooms[1];
    WorldGen.rooms[0].E_Room = WorldGen.rooms[1];
    WorldGen.rooms[1].W_Link = WorldGen.rooms[0];
    WorldGen.rooms[1].W_Room = WorldGen.rooms[0];
    WorldGen.rooms[1].E_Link = WorldGen.rooms[2];
    WorldGen.rooms[1].E_Room = WorldGen.rooms[2];
    WorldGen.rooms[2].W_Link = WorldGen.rooms[1];
    WorldGen.rooms[2].W_Room = WorldGen.rooms[1];
    WorldGen.rooms[2].E_Link = WorldGen.rooms[0];
    WorldGen.rooms[2].E_Room = WorldGen.rooms[0];
  }

  public static void GenerateNewWorld()
  {
    if (!((UnityEngine.Object) WorldGen.Instance != (UnityEngine.Object) null))
      return;
    WorldGen.Instance.Start();
  }

  public void Start()
  {
    this.RandomSeed = new System.Random(this.Seed);
    if (!DataManager.Instance.Tutorial_Rooms_Completed && DataManager.Instance.Create_Tutorial_Rooms)
    {
      WorldGen.ResetGeneratedWorldData();
      this.CreateTutorialRooms();
    }
    else if (!WorldGen.WorldGenerated)
    {
      WorldGen.ResetGeneratedWorldData();
      WorldGen.WIDTH = this.width;
      WorldGen.HEIGHT = this.height;
      this.CreateAndConnectRooms();
    }
    WorldGen.WorldGenerated = true;
    if (this.OnWorldGenerated == null)
      return;
    this.OnWorldGenerated();
  }

  public void Clear()
  {
    foreach (Room room in WorldGen.rooms)
      room.Clear();
    WorldGen.rooms = new List<Room>();
  }

  public void CreateResources()
  {
    this.TreeCount = 50;
    this.RockCount = 30;
    this.SeedFlowerCount = 20;
    this.CottonPlantCount = 20;
    this.FollowerCount = 5;
  }

  public void CreateAndConnectRooms()
  {
    WorldGen.rooms = new List<Room>();
    for (int index1 = 0; index1 < this.width; ++index1)
    {
      for (int index2 = 0; index2 < this.height; ++index2)
      {
        Room room;
        if (this.InstantiatePrefabs)
        {
          room = UnityEngine.Object.Instantiate<GameObject>(this.room, new Vector3((float) (index1 * this.roomDist - this.width) + 0.5f, 0.0f, (float) (index2 * this.roomDist - this.height) + 0.5f), Quaternion.identity).GetComponent<Room>();
        }
        else
        {
          GameObject gameObject = new GameObject();
          gameObject.transform.position = new Vector3((float) (index1 * this.roomDist - this.width) + 0.5f, 0.0f, (float) (index2 * this.roomDist - this.height) + 0.5f);
          room = gameObject.AddComponent<Room>();
          gameObject.transform.parent = this.gameObject.transform;
          gameObject.name = $"Room: {index1.ToString()}_{index2.ToString()}";
        }
        room.x = index1;
        room.y = index2;
        WorldGen.rooms.Add(room);
      }
    }
    this.findNeighbours();
    switch (this.StartRoom)
    {
      case WorldGen.StartingRoom.Center:
        this.setCrossRoad(this.width / 2, this.height / 2);
        break;
      case WorldGen.StartingRoom.North:
        this.setCrossRoad(this.width / 2, this.height - 1);
        break;
      case WorldGen.StartingRoom.East:
        this.setCrossRoad(this.width - 1, this.height / 2);
        break;
      case WorldGen.StartingRoom.South:
        this.setCrossRoad(this.width / 2, 0);
        break;
      case WorldGen.StartingRoom.West:
        this.setCrossRoad(0, this.height / 2);
        break;
    }
    this.setLinks();
    Room room1 = (Room) null;
    foreach (Room room2 in WorldGen.rooms)
    {
      if (room2.isHome)
      {
        room1 = room2;
        room2.NewHomeRoom();
      }
      else if (room2.isEntranceHall)
        room2.NewRoom(this.RandomSeed.Next(0, int.MaxValue));
      else if (room2.pointOfInterest)
        room2.NewPointOfInterestRoom();
      else
        room2.NewRoom(this.RandomSeed.Next(0, int.MaxValue));
    }
    Room room3 = new Room();
    room3.NewBaseRoom();
    room3.x = WorldGen.WIDTH / 2;
    room3.y = -1;
    room3.N_Link = room1;
    room1.S_Link = room3;
    WorldGen.rooms.Add(room3);
  }

  public void findNeighbours()
  {
    for (int _x = 0; _x < this.width; ++_x)
    {
      for (int _y = 0; _y < this.height; ++_y)
      {
        Room room = WorldGen.getRoom(_x, _y);
        if (_x + 1 < this.width)
          room.E_Room = WorldGen.getRoom(_x + 1, _y);
        if (_x - 1 >= 0)
          room.W_Room = WorldGen.getRoom(_x - 1, _y);
        if (_y + 1 < this.height)
          room.N_Room = WorldGen.getRoom(_x, _y + 1);
        if (_y - 1 >= 0)
          room.S_Room = WorldGen.getRoom(_x, _y - 1);
      }
    }
  }

  public void setCrossRoad(int i, int j)
  {
    Room room1 = WorldGen.getRoom(i, j);
    room1.isHome = true;
    if (this.StartAtCrossRoad)
      WorldGen.startRoom = room1;
    if ((UnityEngine.Object) room1 != (UnityEngine.Object) null)
    {
      if ((UnityEngine.Object) room1.N_Room != (UnityEngine.Object) null)
      {
        this.addLink(WorldGen.dir.north, room1);
        room1.N_Room.S_Link = room1;
        room1.N_Link = room1.N_Room;
      }
      if ((UnityEngine.Object) room1.E_Room != (UnityEngine.Object) null && (UnityEngine.Object) room1.E_Room.W_Room == (UnityEngine.Object) room1 && (UnityEngine.Object) room1.E_Room.W_Link == (UnityEngine.Object) null)
      {
        this.addLink(WorldGen.dir.east, room1);
        room1.E_Room.W_Link = room1;
        room1.E_Link = room1.E_Room;
      }
      if ((UnityEngine.Object) room1.S_Room != (UnityEngine.Object) null && (UnityEngine.Object) room1.S_Room.N_Room == (UnityEngine.Object) room1 && (UnityEngine.Object) room1.S_Room.N_Link == (UnityEngine.Object) null)
      {
        this.addLink(WorldGen.dir.south, room1);
        room1.S_Room.N_Link = room1;
        room1.S_Link = room1.S_Room;
      }
      if ((UnityEngine.Object) room1.W_Room != (UnityEngine.Object) null && (UnityEngine.Object) room1.W_Room.E_Room == (UnityEngine.Object) room1 && (UnityEngine.Object) room1.W_Room.E_Link == (UnityEngine.Object) null)
      {
        this.addLink(WorldGen.dir.west, room1);
        room1.W_Room.E_Link = room1;
        room1.W_Link = room1.W_Room;
      }
    }
    if (!this.TutorialRooms)
      return;
    for (int index = 1; index < j + 1; ++index)
    {
      Room room2 = WorldGen.getRoom(i - index, j);
      room2.isEntranceHall = true;
      if (!this.StartAtCrossRoad)
        WorldGen.startRoom = room2;
      if ((UnityEngine.Object) room2.E_Room.W_Room == (UnityEngine.Object) room2 && (UnityEngine.Object) room2.E_Room.W_Link == (UnityEngine.Object) null)
      {
        this.addLink(WorldGen.dir.east, room2);
        room2.E_Room.W_Link = room2;
        room2.E_Link = room2.E_Room;
      }
    }
  }

  public void setLinks()
  {
    int num = 1;
    for (int _x = 0; _x < this.width; ++_x)
    {
      for (int _y = 0; _y < this.height; ++_y)
      {
        Room room = WorldGen.getRoom(_x, _y);
        if (!room.isEntranceHall)
        {
          if ((UnityEngine.Object) room.N_Room != (UnityEngine.Object) null && this.RandomSeed.Next(0, 10) < num && !room.N_Room.isEntranceHall && (UnityEngine.Object) room.N_Room.S_Room == (UnityEngine.Object) room && (UnityEngine.Object) room.N_Room.S_Link == (UnityEngine.Object) null)
          {
            this.addLink(WorldGen.dir.north, room);
            room.N_Room.S_Link = room;
            room.N_Link = room.N_Room;
          }
          if ((UnityEngine.Object) room.E_Room != (UnityEngine.Object) null && this.RandomSeed.Next(0, 10) < num && !room.E_Room.isEntranceHall && (UnityEngine.Object) room.E_Room.W_Room == (UnityEngine.Object) room && (UnityEngine.Object) room.E_Room.W_Link == (UnityEngine.Object) null)
          {
            this.addLink(WorldGen.dir.east, room);
            room.E_Room.W_Link = room;
            room.E_Link = room.E_Room;
          }
          if ((UnityEngine.Object) room.S_Room != (UnityEngine.Object) null && this.RandomSeed.Next(0, 10) < num && !room.S_Room.isEntranceHall && (UnityEngine.Object) room.S_Room.N_Room == (UnityEngine.Object) room && (UnityEngine.Object) room.S_Room.N_Link == (UnityEngine.Object) null)
          {
            this.addLink(WorldGen.dir.south, room);
            room.S_Room.N_Link = room;
            room.S_Link = room.S_Room;
          }
          if ((UnityEngine.Object) room.W_Room != (UnityEngine.Object) null && this.RandomSeed.Next(0, 10) < num && !room.W_Room.isEntranceHall && (UnityEngine.Object) room.W_Room.E_Room == (UnityEngine.Object) room && (UnityEngine.Object) room.W_Room.E_Link == (UnityEngine.Object) null)
          {
            this.addLink(WorldGen.dir.west, room);
            room.W_Room.E_Link = room;
            room.W_Link = room.W_Room;
          }
        }
      }
    }
    this.checkLonely();
    this.applyIslands();
    while (this.NumRegions > 1)
      this.connectIsland();
  }

  public void checkLonely()
  {
    ArrayList arrayList1 = new ArrayList();
    for (int _x = 0; _x < this.width; ++_x)
    {
      for (int _y = 0; _y < this.height; ++_y)
      {
        Room room = WorldGen.getRoom(_x, _y);
        if ((UnityEngine.Object) room.E_Link == (UnityEngine.Object) null && (UnityEngine.Object) room.W_Link == (UnityEngine.Object) null && (UnityEngine.Object) room.S_Link == (UnityEngine.Object) null && (UnityEngine.Object) room.N_Link == (UnityEngine.Object) null && !room.isEntranceHall)
          arrayList1.Add((object) room);
      }
    }
    foreach (Room r in arrayList1)
    {
      ArrayList arrayList2 = new ArrayList();
      if ((UnityEngine.Object) r.N_Room != (UnityEngine.Object) null && (UnityEngine.Object) r.N_Room.S_Link == (UnityEngine.Object) null && !r.N_Room.isEntranceHall)
        arrayList2.Add((object) r.N_Room);
      if ((UnityEngine.Object) r.E_Room != (UnityEngine.Object) null && (UnityEngine.Object) r.E_Room.W_Link == (UnityEngine.Object) null && !r.E_Room.isEntranceHall)
        arrayList2.Add((object) r.E_Room);
      if ((UnityEngine.Object) r.S_Room != (UnityEngine.Object) null && (UnityEngine.Object) r.S_Room.N_Link == (UnityEngine.Object) null && !r.S_Room.isEntranceHall)
        arrayList2.Add((object) r.S_Room);
      if ((UnityEngine.Object) r.W_Room != (UnityEngine.Object) null && (UnityEngine.Object) r.W_Room.E_Link == (UnityEngine.Object) null && !r.W_Room.isEntranceHall)
        arrayList2.Add((object) r.W_Room);
      if (arrayList2.Count > 0)
      {
        Room room = (Room) arrayList2[this.RandomSeed.Next(0, arrayList2.Count)];
        if ((UnityEngine.Object) room == (UnityEngine.Object) r.N_Room)
        {
          r.N_Room.S_Link = r;
          r.N_Link = r.N_Room;
          this.addLink(WorldGen.dir.north, r);
        }
        if ((UnityEngine.Object) room == (UnityEngine.Object) r.E_Room)
        {
          r.E_Room.W_Link = r;
          r.E_Link = r.E_Room;
          this.addLink(WorldGen.dir.east, r);
        }
        if ((UnityEngine.Object) room == (UnityEngine.Object) r.S_Room)
        {
          r.S_Room.N_Link = r;
          r.S_Link = r.S_Room;
          this.addLink(WorldGen.dir.south, r);
        }
        if ((UnityEngine.Object) room == (UnityEngine.Object) r.W_Room)
        {
          r.W_Room.E_Link = r;
          r.W_Link = r.W_Room;
          this.addLink(WorldGen.dir.west, r);
        }
      }
    }
  }

  public void connectIsland()
  {
    Room room = WorldGen.rooms[this.RandomSeed.Next(0, WorldGen.rooms.Count)];
    if ((UnityEngine.Object) room.N_Room != (UnityEngine.Object) null && room.N_Room.region != room.region && !room.isEntranceHall && !room.N_Room.isEntranceHall)
    {
      room.N_Room.S_Link = room;
      room.N_Link = room.N_Room;
      this.addLink(WorldGen.dir.north, room);
      this.applyIslands();
    }
    else if ((UnityEngine.Object) room.E_Room != (UnityEngine.Object) null && room.E_Room.region != room.region && !room.isEntranceHall && !room.E_Room.isEntranceHall)
    {
      room.E_Room.W_Link = room;
      room.E_Link = room.E_Room;
      this.addLink(WorldGen.dir.east, room);
      this.applyIslands();
    }
    else if ((UnityEngine.Object) room.S_Room != (UnityEngine.Object) null && room.S_Room.region != room.region && !room.isEntranceHall && !room.S_Room.isEntranceHall)
    {
      room.S_Room.N_Link = room;
      room.S_Link = room.S_Room;
      this.addLink(WorldGen.dir.south, room);
      this.applyIslands();
    }
    else
    {
      if (!((UnityEngine.Object) room.W_Room != (UnityEngine.Object) null) || room.W_Room.region == room.region || room.isEntranceHall || room.W_Room.isEntranceHall)
        return;
      room.W_Room.E_Link = room;
      room.W_Link = room.W_Room;
      this.addLink(WorldGen.dir.west, room);
      this.applyIslands();
    }
  }

  public void resetRegions()
  {
    this.NumRegions = 0;
    for (int _x = 0; _x < this.width; ++_x)
    {
      for (int _y = 0; _y < this.height; ++_y)
        WorldGen.getRoom(_x, _y).regionSet = false;
    }
  }

  public void applyIslands()
  {
    this.resetRegions();
    for (int _x = 0; _x < this.width; ++_x)
    {
      for (int _y = 0; _y < this.height; ++_y)
      {
        Room room = WorldGen.getRoom(_x, _y);
        if (!room.regionSet)
        {
          Color randomColor = UnityEngine.Random.ColorHSV();
          this.floodFillColour(room, randomColor);
          ++this.NumRegions;
        }
      }
    }
  }

  public void setRoomsOfInterest()
  {
    List<Room> roomList = new List<Room>();
    for (int _x = 0; _x < this.width; ++_x)
    {
      for (int _y = 0; _y < this.height; ++_y)
      {
        Room room = WorldGen.getRoom(_x, _y);
        if (!room.isEntranceHall)
        {
          int num = 0;
          if ((UnityEngine.Object) room.N_Link != (UnityEngine.Object) null)
            ++num;
          if ((UnityEngine.Object) room.E_Link != (UnityEngine.Object) null)
            ++num;
          if ((UnityEngine.Object) room.W_Link != (UnityEngine.Object) null)
            ++num;
          if ((UnityEngine.Object) room.S_Link != (UnityEngine.Object) null)
            ++num;
          if (num == 1)
            roomList.Add(room);
        }
      }
    }
    int num1 = this.NumRoomsOfInterest + 1;
    while (--num1 > 0 && roomList.Count > 0)
    {
      int index = this.RandomSeed.Next(0, roomList.Count);
      roomList[index].pointOfInterest = true;
      roomList.RemoveAt(index);
    }
    int num2 = num1 * 5;
    while (--num2 > 0 && num1 > 0)
    {
      int index = this.RandomSeed.Next(0, WorldGen.rooms.Count);
      Room room = WorldGen.rooms[index];
      if (!room.isHome && !room.pointOfInterest)
      {
        room.pointOfInterest = true;
        --num1;
      }
    }
  }

  public Room GetSpecialRoom()
  {
    List<Room> roomList = new List<Room>();
    for (int _x = 0; _x < this.width; ++_x)
    {
      for (int _y = 0; _y < this.height; ++_y)
      {
        Room room = WorldGen.getRoom(_x, _y);
        if (!room.isHome && !room.pointOfInterest)
        {
          int num = 0;
          if ((UnityEngine.Object) room.N_Link != (UnityEngine.Object) null)
            ++num;
          if ((UnityEngine.Object) room.E_Link != (UnityEngine.Object) null)
            ++num;
          if ((UnityEngine.Object) room.W_Link != (UnityEngine.Object) null)
            ++num;
          if ((UnityEngine.Object) room.S_Link != (UnityEngine.Object) null)
            ++num;
          if (num == 1)
            roomList.Add(room);
        }
      }
    }
    if (roomList.Count > 0)
    {
      int index = this.RandomSeed.Next(0, roomList.Count);
      return roomList[index];
    }
    int num1 = 500;
    while (--num1 > 0)
    {
      int index = this.RandomSeed.Next(0, WorldGen.rooms.Count);
      Room room = WorldGen.rooms[index];
      if (!room.isHome && !room.pointOfInterest)
        return room;
    }
    return (Room) null;
  }

  public Room SetSpecificRoom(bool N, bool E, bool S, bool W, int minX, int minY)
  {
    for (int _x = minX; _x < this.width; ++_x)
    {
      for (int _y = minY; _y < this.height; ++_y)
      {
        Room room = WorldGen.getRoom(_x, _y);
        if (room.pointOfInterest && room.CurrentSpecificRoom == Room.SpecificRoom.None && (N ? ((UnityEngine.Object) room.N_Link != (UnityEngine.Object) null ? 1 : 0) : ((UnityEngine.Object) room.N_Link == (UnityEngine.Object) null ? 1 : 0)) != 0 && (E ? ((UnityEngine.Object) room.E_Link != (UnityEngine.Object) null ? 1 : 0) : ((UnityEngine.Object) room.E_Link == (UnityEngine.Object) null ? 1 : 0)) != 0 && (W ? ((UnityEngine.Object) room.W_Link != (UnityEngine.Object) null ? 1 : 0) : ((UnityEngine.Object) room.W_Link == (UnityEngine.Object) null ? 1 : 0)) != 0 && (S ? ((UnityEngine.Object) room.S_Link != (UnityEngine.Object) null ? 1 : 0) : ((UnityEngine.Object) room.S_Link == (UnityEngine.Object) null ? 1 : 0)) != 0)
          return room;
      }
    }
    for (int _x = minX; _x < this.width; ++_x)
    {
      for (int _y = minY; _y < this.height; ++_y)
      {
        Room room = WorldGen.getRoom(_x, _y);
        if (!room.pointOfInterest && (N ? ((UnityEngine.Object) room.N_Link != (UnityEngine.Object) null ? 1 : 0) : ((UnityEngine.Object) room.N_Link == (UnityEngine.Object) null ? 1 : 0)) != 0 && (E ? ((UnityEngine.Object) room.E_Link != (UnityEngine.Object) null ? 1 : 0) : ((UnityEngine.Object) room.E_Link == (UnityEngine.Object) null ? 1 : 0)) != 0 && (W ? ((UnityEngine.Object) room.W_Link != (UnityEngine.Object) null ? 1 : 0) : ((UnityEngine.Object) room.W_Link == (UnityEngine.Object) null ? 1 : 0)) != 0 && (S ? ((UnityEngine.Object) room.S_Link != (UnityEngine.Object) null ? 1 : 0) : ((UnityEngine.Object) room.S_Link == (UnityEngine.Object) null ? 1 : 0)) != 0)
          return room;
      }
    }
    for (int _x = minX; _x < this.width; ++_x)
    {
      for (int _y = minY; _y < this.height; ++_y)
      {
        Room room = WorldGen.getRoom(_x, _y);
        int num = 0;
        if ((UnityEngine.Object) room.N_Link != (UnityEngine.Object) null)
          ++num;
        if ((UnityEngine.Object) room.E_Link != (UnityEngine.Object) null)
          ++num;
        if ((UnityEngine.Object) room.W_Link != (UnityEngine.Object) null)
          ++num;
        if ((UnityEngine.Object) room.S_Link != (UnityEngine.Object) null)
          ++num;
        if (num == 1)
        {
          room.N_Link = N ? room.N_Room : (Room) null;
          if ((UnityEngine.Object) room.N_Room != (UnityEngine.Object) null)
            room.N_Room.S_Link = N ? room : (Room) null;
          room.E_Link = E ? room.E_Room : (Room) null;
          if ((UnityEngine.Object) room.E_Room != (UnityEngine.Object) null)
            room.E_Room.W_Link = E ? room : (Room) null;
          room.W_Link = W ? room.W_Room : (Room) null;
          if ((UnityEngine.Object) room.W_Room != (UnityEngine.Object) null)
            room.W_Room.E_Link = W ? room : (Room) null;
          room.S_Link = S ? room.S_Room : (Room) null;
          if ((UnityEngine.Object) room.S_Room != (UnityEngine.Object) null)
            room.S_Room.N_Link = S ? room : (Room) null;
          return room;
        }
      }
    }
    return (Room) null;
  }

  public void floodFillColour(Room r, Color randomColor)
  {
    if ((UnityEngine.Object) r == (UnityEngine.Object) null || r.regionSet)
      return;
    r.regionSet = true;
    r.region = this.NumRegions;
    this.floodFillColour(r.N_Link, randomColor);
    this.floodFillColour(r.E_Link, randomColor);
    this.floodFillColour(r.S_Link, randomColor);
    this.floodFillColour(r.W_Link, randomColor);
  }

  public static Room getRoom(int _x, int _y)
  {
    foreach (Room room in WorldGen.rooms)
    {
      if (room.x == _x && room.y == _y)
        return room;
    }
    return (Room) null;
  }

  public void addLink(WorldGen.dir _dir, Room r)
  {
    if (!this.InstantiatePrefabs)
      return;
    Vector3 vector3 = new Vector3();
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.link, new Vector3((float) (r.x * this.roomDist - this.width) + 0.5f, 0.0f, (float) (r.y * this.roomDist - this.height) + 0.5f) + Vector3.down, Quaternion.identity);
    switch (_dir)
    {
      case WorldGen.dir.north:
        vector3 = Vector3.forward * (float) this.roomDist / 2f;
        gameObject.transform.localScale = new Vector3(4f, 1f, (float) this.roomDist);
        break;
      case WorldGen.dir.east:
        vector3 = Vector3.right * (float) this.roomDist / 2f;
        gameObject.transform.localScale = new Vector3((float) this.roomDist, 1f, 4f);
        break;
      case WorldGen.dir.south:
        vector3 = Vector3.back * (float) this.roomDist / 2f;
        gameObject.transform.localScale = new Vector3(4f, 1f, (float) this.roomDist);
        break;
      case WorldGen.dir.west:
        vector3 = Vector3.left * (float) this.roomDist / 2f;
        gameObject.transform.localScale = new Vector3((float) this.roomDist, 1f, 4f);
        break;
    }
    gameObject.transform.position += vector3;
  }

  public enum StartingRoom
  {
    Center,
    North,
    East,
    South,
    West,
  }

  public delegate void WorldGeneratedAction();

  public enum dir
  {
    north,
    east,
    south,
    west,
  }
}
