// Decompiled with JetBrains decompiler
// Type: Room
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Room : BaseMonoBehaviour
{
  public int IslandChoice = -1;
  public int EnemyChoice = -1;
  public int x;
  public int y;
  public Room N_Room;
  public Room E_Room;
  public Room S_Room;
  public Room W_Room;
  public Room N_Link;
  public Room E_Link;
  public Room S_Link;
  public Room W_Link;
  public bool regionSet;
  public int region;
  public bool isBase;
  public bool isHome;
  public bool isEntranceHall;
  public bool pointOfInterest;
  public Room.SpecificRoom CurrentSpecificRoom;
  public int[,] RoomGrid;
  public int[,] StructuresOld;
  public float[,] PerlinNoise;
  public float[,] PerlinNoiseRock;
  public Vector2 NorthDoor;
  public Vector2 EastDoor;
  public Vector2 SouthDoor;
  public Vector2 WestDoor;
  public int Width;
  public int Height;
  public bool visited;
  public bool cleared;
  public bool activeTeleporter;
  public string PrefabDir;
  public string prefabName;
  public bool ResourcesPlaced;
  public List<StructuresData> Structures;
  public List<Vector3> RecruitPositions;
  public static int PointOfInterestCount = -1;
  public int Seed;
  public static List<int> Combatrooms = new List<int>();

  public void NewPointOfInterestRoom()
  {
    switch (this.CurrentSpecificRoom)
    {
      case Room.SpecificRoom.CultDoor:
        this.prefabName = "Special/Goat/Cult Door/Cult Door";
        break;
      case Room.SpecificRoom.Village:
        this.prefabName = "Special/Villages/Horse Village";
        break;
      case Room.SpecificRoom.KeyShrine_1:
        this.prefabName = "Special/Key Shrine/Key Shrine 1";
        break;
      case Room.SpecificRoom.KeyShrine_2:
        this.prefabName = "Special/Key Shrine/Key Shrine 2";
        break;
      case Room.SpecificRoom.KeyShrine_3:
        this.prefabName = "Special/Key Shrine/Key Shrine 3";
        break;
      case Room.SpecificRoom.Follower_Rescue:
        this.prefabName = "Special/Follower Rescue/Follower Rescue";
        break;
      case Room.SpecificRoom.Tutorial_0:
        this.prefabName = "Intro/Intro 1";
        break;
      case Room.SpecificRoom.Tutorial_1:
        this.prefabName = "Intro/Intro 2";
        break;
      case Room.SpecificRoom.Tutorial_2:
        this.prefabName = "Intro/Intro 3";
        break;
      case Room.SpecificRoom.Goat_Spider:
        this.prefabName = "Special/Goat/Spider/Spider Room";
        break;
      case Room.SpecificRoom.Goat_Tentacle:
        this.prefabName = "Special/Goat/Tentacle/Tentacle Room";
        break;
      default:
        ++Room.PointOfInterestCount;
        if (Room.PointOfInterestCount > 1)
          Room.PointOfInterestCount = 0;
        switch (Room.PointOfInterestCount)
        {
          case 0:
            this.prefabName = "Special/Goat/First Meeting/Room 0";
            break;
          case 1:
            this.prefabName = "Special/Healing Room/Room_HealingRoom";
            break;
        }
        break;
    }
    this.PrefabDir = "_Rooms/Production/" + this.prefabName;
    this.cleared = true;
  }

  public void NewBaseRoom()
  {
    this.isBase = true;
    this.activeTeleporter = true;
    this.prefabName = "Entrance/Entrance Room";
    this.PrefabDir = "_Rooms/Production/" + this.prefabName;
    this.cleared = true;
    this.visited = true;
  }

  public void NewHomeRoom()
  {
    this.activeTeleporter = true;
    this.prefabName = "Entrance/Entrance Room";
    this.PrefabDir = "_Rooms/Production/" + this.prefabName;
    this.cleared = true;
  }

  public void NewRoom(int Seed)
  {
    this.cleared = true;
    this.prefabName = "Generated/Generated Room";
    this.PrefabDir = "_Rooms/Production/" + this.prefabName;
    this.Seed = Seed;
  }

  public void Clear() => this.Structures.Clear();

  public enum SpecificRoom
  {
    None,
    RandomSpecialinterest,
    CultDoor,
    Village,
    KeyShrine_1,
    KeyShrine_2,
    KeyShrine_3,
    Follower_Rescue,
    Tutorial_0,
    Tutorial_1,
    Tutorial_2,
    Goat_Spider,
    Goat_Tentacle,
  }
}
