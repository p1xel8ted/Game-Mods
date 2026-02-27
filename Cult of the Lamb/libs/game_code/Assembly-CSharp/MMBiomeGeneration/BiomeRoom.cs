// Decompiled with JetBrains decompiler
// Type: MMBiomeGeneration.BiomeRoom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMRoomGeneration;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace MMBiomeGeneration;

[Serializable]
public class BiomeRoom
{
  public static List<BiomeRoom> BiomeRooms = new List<BiomeRoom>();
  public BiomeRoom.Direction CriticalPathDirection;
  public bool IsCustom;
  public bool HasWeapon;
  public bool RegionSet;
  public int Region;
  public int Distance = -1;
  public int x;
  public int y;
  public int Seed;
  public System.Random RandomSeed;
  public RoomConnection N_Room;
  public RoomConnection E_Room;
  public RoomConnection S_Room;
  public RoomConnection W_Room;
  public bool Active;
  public bool Completed;
  public bool Discovered;
  public bool Hidden;
  public bool Visited;
  public bool Generated;
  public GameObject GameObject;
  public string GameObjectPath;
  public bool IsRespawnRoom;
  public bool IsDeathCatRoom;
  public bool IsYngyaRoom;
  public bool IsMysticShopRoom;
  public bool IsBoss;
  public GenerateRoom _generateRoom;

  public List<Vector2Int> EmptyNeighbourPositions
  {
    get
    {
      List<Vector2Int> neighbourPositions = new List<Vector2Int>();
      if (BiomeRoom.GetRoom(this.x, this.y - 1) == null)
        neighbourPositions.Add(new Vector2Int(this.x, this.y - 1));
      if (BiomeRoom.GetRoom(this.x, this.y + 1) == null)
        neighbourPositions.Add(new Vector2Int(this.x, this.y + 1));
      if (BiomeRoom.GetRoom(this.x - 1, this.y) == null)
        neighbourPositions.Add(new Vector2Int(this.x - 1, this.y));
      if (BiomeRoom.GetRoom(this.x + 1, this.y) == null)
        neighbourPositions.Add(new Vector2Int(this.x + 1, this.y));
      return neighbourPositions;
    }
  }

  public List<Vector2Int> EmptyNeighboursWithEmptyNeighbours
  {
    get
    {
      List<Vector2Int> withEmptyNeighbours = new List<Vector2Int>();
      if (BiomeRoom.GetRoom(this.x, this.y - 1) == null && BiomeRoom.EmptyNeighboursOfPosition(this.x, this.y - 1, true).Count > 0)
        withEmptyNeighbours.Add(new Vector2Int(this.x, this.y - 1));
      if (BiomeRoom.GetRoom(this.x, this.y + 1) == null && BiomeRoom.EmptyNeighboursOfPosition(this.x, this.y + 1, true).Count > 0)
        withEmptyNeighbours.Add(new Vector2Int(this.x, this.y + 1));
      if (BiomeRoom.GetRoom(this.x + 1, this.y) == null && BiomeRoom.EmptyNeighboursOfPosition(this.x + 1, this.y, true).Count > 0)
        withEmptyNeighbours.Add(new Vector2Int(this.x + 1, this.y));
      if (BiomeRoom.GetRoom(this.x - 1, this.y) == null && BiomeRoom.EmptyNeighboursOfPosition(this.x - 1, this.y, true).Count > 0)
        withEmptyNeighbours.Add(new Vector2Int(this.x - 1, this.y));
      return withEmptyNeighbours;
    }
  }

  public bool DoAnyOfMyEmptyNeighboursHaveEmptyNeighbours()
  {
    foreach (Vector2Int vector2Int in this.EmptyNeighbourPositionsIgnoreSouth)
    {
      if (BiomeRoom.EmptyNeighboursOfPosition(vector2Int.x, vector2Int.y, true).Count > 0)
        return true;
    }
    return false;
  }

  public static List<Vector2Int> EmptyNeighboursOfPosition(int x, int y, bool IgnoreSouth)
  {
    List<Vector2Int> vector2IntList = new List<Vector2Int>();
    if (!IgnoreSouth && BiomeRoom.GetRoom(x, y - 1) == null)
      vector2IntList.Add(new Vector2Int(x, y - 1));
    if (BiomeRoom.GetRoom(x, y + 1) == null)
      vector2IntList.Add(new Vector2Int(x, y + 1));
    if (BiomeRoom.GetRoom(x - 1, y) == null)
      vector2IntList.Add(new Vector2Int(x - 1, y));
    if (BiomeRoom.GetRoom(x + 1, y) == null)
      vector2IntList.Add(new Vector2Int(x + 1, y));
    return vector2IntList;
  }

  public List<Vector2Int> EmptyNeighbourPositionsIgnoreSouth
  {
    get
    {
      List<Vector2Int> positionsIgnoreSouth = new List<Vector2Int>();
      if (BiomeRoom.GetRoom(this.x, this.y + 1) == null)
        positionsIgnoreSouth.Add(new Vector2Int(this.x, this.y + 1));
      if (BiomeRoom.GetRoom(this.x - 1, this.y) == null)
        positionsIgnoreSouth.Add(new Vector2Int(this.x - 1, this.y));
      if (BiomeRoom.GetRoom(this.x + 1, this.y) == null)
        positionsIgnoreSouth.Add(new Vector2Int(this.x + 1, this.y));
      return positionsIgnoreSouth;
    }
  }

  public bool IsNorthEmpty => BiomeRoom.GetRoom(this.x, this.y + 1) == null;

  public int NumConnections
  {
    get
    {
      int numConnections = 0;
      if (this.N_Room != null && this.N_Room.Connected)
        ++numConnections;
      if (this.E_Room != null && this.E_Room.Connected)
        ++numConnections;
      if (this.S_Room != null && this.S_Room.Connected)
        ++numConnections;
      if (this.W_Room != null && this.W_Room.Connected)
        ++numConnections;
      return numConnections;
    }
  }

  public GenerateRoom generateRoom
  {
    get
    {
      if ((UnityEngine.Object) this._generateRoom == (UnityEngine.Object) null && (UnityEngine.Object) this.GameObject != (UnityEngine.Object) null)
        this._generateRoom = this.GameObject.GetComponent<GenerateRoom>();
      return this._generateRoom;
    }
  }

  public BiomeRoom(int x, int y, int Seed, GameObject GameObject)
  {
    this.x = x;
    this.y = y;
    this.Seed = Seed;
    this.GameObject = GameObject;
    BiomeRoom.BiomeRooms.Add(this);
    this.RandomSeed = new System.Random(Seed);
  }

  public void Activate(BiomeRoom PrevRoom, bool ReuseGeneratorRoom)
  {
    if (PrevRoom != null && (bool) (UnityEngine.Object) PrevRoom.GameObject && (UnityEngine.Object) PrevRoom.GameObject != (UnityEngine.Object) this.GameObject)
      PrevRoom.GameObject.SetActive(false);
    if ((UnityEngine.Object) this.generateRoom != (UnityEngine.Object) null)
      this.generateRoom.SpawnHeavyAssets();
    if (!this.GameObject.activeSelf)
      this.GameObject.SetActive(true);
    if (ReuseGeneratorRoom)
    {
      if ((UnityEngine.Object) this.generateRoom != (UnityEngine.Object) null)
        this.generateRoom.Generate(this.Seed, this.N_Room.ConnectionType, this.E_Room.ConnectionType, this.S_Room.ConnectionType, this.W_Room.ConnectionType);
    }
    else
    {
      if ((UnityEngine.Object) this.generateRoom != (UnityEngine.Object) null)
      {
        if (!this.Generated)
        {
          this.generateRoom.Generate(this.Seed, this.N_Room.ConnectionType, this.E_Room.ConnectionType, this.S_Room.ConnectionType, this.W_Room.ConnectionType);
        }
        else
        {
          this.generateRoom.GeneratedDecorations = true;
          this.generateRoom.SetColliderAndUpdatePathfinding();
          this.generateRoom.CreateBackgroundSpriteShape();
        }
      }
      this.Generated = true;
    }
    this.Discovered = true;
    this.Visited = true;
  }

  public void Clear()
  {
    BiomeRoom.BiomeRooms.Remove(this);
    this.N_Room = this.E_Room = this.S_Room = this.W_Room = (RoomConnection) null;
  }

  public static BiomeRoom GetRoom(int x, int y)
  {
    foreach (BiomeRoom biomeRoom in BiomeRoom.BiomeRooms)
    {
      if (biomeRoom.x == x && biomeRoom.y == y)
        return biomeRoom;
    }
    return (BiomeRoom) null;
  }

  public RoomConnection GetOppositeConnection(RoomConnection RoomConnection)
  {
    if (RoomConnection == this.N_Room)
      return this.N_Room.Room.S_Room;
    if (RoomConnection == this.E_Room)
      return this.E_Room.Room.W_Room;
    if (RoomConnection == this.S_Room)
      return this.S_Room.Room.N_Room;
    return RoomConnection == this.W_Room ? this.W_Room.Room.E_Room : (RoomConnection) null;
  }

  public enum Direction
  {
    None,
    North,
    East,
    South,
    West,
  }
}
