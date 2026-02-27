// Decompiled with JetBrains decompiler
// Type: MMBiomeGeneration.BiomeGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using FMODUnity;
using I2.Loc;
using Lamb.UI;
using Map;
using MMRoomGeneration;
using MMTools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
namespace MMBiomeGeneration;

public class BiomeGenerator : BaseMonoBehaviour
{
  public int TotalFloors = 3;
  public const int MAX_ENDLESS_LEVELS = 3;
  public bool TestStartingLayer;
  public int StartingLayer = 1;
  public int PostMiniBossDoorFollowerCount = 3;
  public int GoldToGive = 1;
  public FollowerLocation DungeonLocation;
  [EventRef(MigrateTo = "MusicToTrigger")]
  public string biomeMusicPath;
  public EventReference MusicToTrigger;
  [EventRef(MigrateTo = "AtmosToTrigger")]
  public string biomeAtmosPath;
  public EventReference AtmosToTrigger;
  private FMOD.Studio.EventInstance biomeAtmosInstance;
  public bool stopCurrentMusicOnLoad;
  [HideInInspector]
  public static List<string> UsedEncounters = new List<string>();
  public static BiomeGenerator.GetKey OnGetKey;
  public static BiomeGenerator.GetKey OnUseKey;
  [SerializeField]
  private bool _HasKey;
  private bool ReuseGeneratorRoom;
  public static BiomeGenerator Instance;
  public static Vector2Int BossCoords = new Vector2Int(0, 999);
  public static Vector2Int RespawnRoomCoords = new Vector2Int(-2147483647 /*0x80000001*/, -2147483647 /*0x80000001*/);
  private System.Random RandomSeed;
  [HideInInspector]
  public List<BiomeRoom> Rooms;
  public int Seed;
  public int NumberOfRooms = 20;
  public bool ForceResource;
  public List<RandomResource.Resource> Resources = new List<RandomResource.Resource>();
  [TermsPopup("")]
  public string DisplayName;
  public GameObject GeneratorRoomPrefab;
  public string EntranceRoomPath;
  public bool LockAndKey = true;
  public string LockedRoomPath;
  public string KeyRoomPath;
  public string BossRoomPath;
  public string LeaderRoomPath;
  public string EndOfFloorRoomPath;
  public string PostBossRoomPath;
  public string BossDoorRoomPath;
  public bool StartWithBossRoomDoor;
  public string KeyPiecePath;
  public string EntranceDoorRoomPath;
  public string RespawnRoomPath;
  public string DeathCatRoomPath;
  public List<BiomeGenerator.ListOfStoryRooms> StoryRooms = new List<BiomeGenerator.ListOfStoryRooms>();
  public List<BiomeGenerator.ListOfCustomRoomPrefabs> CustomDynamicRooms = new List<BiomeGenerator.ListOfCustomRoomPrefabs>();
  public List<BiomeGenerator.FixedRoom> CustomRooms = new List<BiomeGenerator.FixedRoom>();
  public bool OverrideRandomWalk;
  public List<BiomeGenerator.OverrideRoom> OverrideRooms = new List<BiomeGenerator.OverrideRoom>();
  private BiomeRoom lastRoom;
  [Space]
  [SerializeField]
  private bool spawnDemons = true;
  private bool spawnedDemons;
  private static bool WeaponAtEnd;
  [HideInInspector]
  public BiomeRoom PostBossBiomeRoom;
  [HideInInspector]
  public BiomeRoom RoomEntrance;
  [HideInInspector]
  public BiomeRoom RoomExit;
  private List<BiomeRoom> CriticalPath;
  private BiomeRoom RespawnRoom;
  private BiomeRoom DeathCatRoom;
  private List<BiomeGenerator.ListOfStoryRooms> RandomiseStoryOrder;
  [HideInInspector]
  public List<BiomeGenerator.ListOfCustomRoomPrefabs> RandomiseOrder;
  private List<BiomeRoom> AvailableRooms;
  public List<GeneraterDecorations> BiomeDecorationSet;
  private List<AsyncOperationHandle<GameObject>> loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();
  public int CurrentX;
  public int CurrentY = -1;
  [HideInInspector]
  public bool IsTeleporting;
  [HideInInspector]
  public bool FirstArrival = true;
  public bool ShowDisplayName = true;
  public GameObject Player;
  private StateMachine PlayerState;
  private int PrevCurrentX = -2147483647 /*0x80000001*/;
  private int PrevCurrentY = -2147483647 /*0x80000001*/;
  private int StartX;
  private int StartY;
  public bool DoFirstArrivalRoutine = true;
  [Space]
  public float HumanoidHealthMultiplier = 1f;
  [HideInInspector]
  public BiomeRoom CurrentRoom;
  private BiomeRoom PrevRoom;
  [HideInInspector]
  public Door North;
  [HideInInspector]
  public Door East;
  [HideInInspector]
  public Door South;
  [HideInInspector]
  public Door West;

  private void InitMusic()
  {
    if (this.stopCurrentMusicOnLoad)
      AudioManager.Instance.StopCurrentMusic();
    AudioManager.Instance.PlayMusic(this.biomeMusicPath, false);
    AudioManager.Instance.PlayAtmos(this.biomeAtmosPath);
    AudioManager.Instance.SetMusicCombatState(false);
  }

  public bool HasKey
  {
    get => this._HasKey;
    set
    {
      this._HasKey = value;
      if (this._HasKey)
      {
        BiomeGenerator.GetKey onGetKey = BiomeGenerator.OnGetKey;
        if (onGetKey != null)
          onGetKey();
      }
      if (this._HasKey)
        return;
      BiomeGenerator.GetKey onUseKey = BiomeGenerator.OnUseKey;
      if (onUseKey == null)
        return;
      onUseKey();
    }
  }

  public static event BiomeGenerator.BiomeAction OnBiomeGenerated;

  public static event BiomeGenerator.BiomeAction OnBiomeChangeRoom;

  public static event BiomeGenerator.BiomeAction OnBiomeLeftRoom;

  public static event BiomeGenerator.BiomeAction OnRoomActive;

  public void RandomiseSeed() => this.Seed = UnityEngine.Random.Range(-2147483647 /*0x80000001*/, int.MaxValue);

  private void DailyRun()
  {
    DateTime now = DateTime.Now;
    this.Seed = int.Parse((now.Day < 10 ? (object) "0" : (object) "").ToString() + (object) now.Day + (now.Month < 10 ? (object) "0" : (object) "") + (object) now.Month + (object) now.Year);
  }

  public int RoomsVisited { get; private set; }

  private void Awake() => BiomeGenerator.Instance = this;

  private void OnEnable()
  {
    BiomeGenerator.Instance = this;
    if (!this.TestStartingLayer || !Application.isEditor)
      return;
    GameManager.CurrentDungeonLayer = this.StartingLayer;
    DataManager.Instance.DungeonBossFight = this.StartingLayer == 4;
    if (GameManager.CurrentDungeonLayer == 5)
    {
      GameManager.CurrentDungeonLayer = 4;
      DataManager.Instance.BossesCompleted.Add(this.DungeonLocation);
    }
    DataManager.SetNewRun();
  }

  private void Start()
  {
    if (DataManager.UseDataManagerSeed)
      this.Seed = DataManager.RandomSeed.Next(-2147483647 /*0x80000001*/, int.MaxValue);
    this.StartCoroutine((IEnumerator) this.GenerateRoutine());
  }

  public static bool EncounterAlreadyUsed(string EncounterName)
  {
    return BiomeGenerator.UsedEncounters.Contains(EncounterName);
  }

  public static void SetEncounterAsUsed(string EncounterName)
  {
    BiomeGenerator.UsedEncounters.Add(EncounterName);
  }

  public static void RemoveEncounterAsUsed(string EncounterName)
  {
    BiomeGenerator.UsedEncounters.Remove(EncounterName);
  }

  private IEnumerator GenerateRoutine()
  {
    BiomeGenerator biomeGenerator = this;
    Stopwatch stopwatch = new Stopwatch();
    stopwatch.Start();
    float Progress = -1f;
    float Total = 18f;
    biomeGenerator.transform.position = Vector3.zero;
    biomeGenerator.RandomSeed = new System.Random(biomeGenerator.Seed);
    biomeGenerator.CreateRandomWalk();
    GenerateRoom component = biomeGenerator.GeneratorRoomPrefab.GetComponent<GenerateRoom>();
    biomeGenerator.BiomeDecorationSet = new List<GeneraterDecorations>((IEnumerable<GeneraterDecorations>) component.DecorationSetList);
    MMTransition.UpdateProgress(++Progress / Total, ScriptLocalization.Interactions.GeneratingDungeon + " 1");
    if ((double) stopwatch.ElapsedMilliseconds > 0.01666666753590107)
    {
      stopwatch.Reset();
      yield return (object) null;
    }
    MMTransition.UpdateProgress(++Progress / Total, ScriptLocalization.Interactions.GeneratingDungeon + " 2");
    biomeGenerator.SetNeighbours();
    MMTransition.UpdateProgress(++Progress / Total, ScriptLocalization.Interactions.GeneratingDungeon + " 3");
    if ((double) stopwatch.ElapsedMilliseconds > 0.01666666753590107)
    {
      stopwatch.Reset();
      yield return (object) null;
    }
    MMTransition.UpdateProgress(++Progress / Total, ScriptLocalization.Interactions.GeneratingDungeon + " 4");
    biomeGenerator.PlaceEntranceAndExit();
    MMTransition.UpdateProgress(++Progress / Total, ScriptLocalization.Interactions.GeneratingDungeon + " 5");
    if ((double) stopwatch.ElapsedMilliseconds > 0.01666666753590107)
    {
      stopwatch.Reset();
      yield return (object) null;
    }
    MMTransition.UpdateProgress(++Progress / Total, ScriptLocalization.Interactions.GeneratingDungeon + " 6");
    biomeGenerator.GetCriticalPath();
    if (biomeGenerator.LockAndKey)
      biomeGenerator.PlaceLockAndKey();
    if (!string.IsNullOrEmpty(biomeGenerator.RespawnRoomPath))
      biomeGenerator.PlaceRespawnRoom();
    if (!string.IsNullOrEmpty(biomeGenerator.DeathCatRoomPath))
      biomeGenerator.PlaceDeathCatRoom();
    MMTransition.UpdateProgress(++Progress / Total, ScriptLocalization.Interactions.GeneratingDungeon + " 7");
    if ((double) stopwatch.ElapsedMilliseconds > 0.01666666753590107)
    {
      stopwatch.Reset();
      yield return (object) null;
    }
    MMTransition.UpdateProgress(++Progress / Total, ScriptLocalization.Interactions.GeneratingDungeon + " 8");
    biomeGenerator.PlaceFixedCustomRooms();
    MMTransition.UpdateProgress(++Progress / Total, ScriptLocalization.Interactions.GeneratingDungeon + " 9");
    if ((double) stopwatch.ElapsedMilliseconds > 0.01666666753590107)
    {
      stopwatch.Reset();
      yield return (object) null;
    }
    MMTransition.UpdateProgress(++Progress / Total, ScriptLocalization.Interactions.GeneratingDungeon + " 10");
    yield return (object) biomeGenerator.StartCoroutine((IEnumerator) biomeGenerator.PlaceStoryRooms());
    if ((double) stopwatch.ElapsedMilliseconds > 0.01666666753590107)
    {
      stopwatch.Reset();
      yield return (object) null;
    }
    MMTransition.UpdateProgress(++Progress / Total, ScriptLocalization.Interactions.GeneratingDungeon + " 11");
    yield return (object) biomeGenerator.StartCoroutine((IEnumerator) biomeGenerator.PlaceDynamicCustomRooms());
    MMTransition.UpdateProgress(++Progress / Total, ScriptLocalization.Interactions.GeneratingDungeon + " 12");
    if ((double) stopwatch.ElapsedMilliseconds > 0.01666666753590107)
    {
      stopwatch.Reset();
      yield return (object) null;
    }
    MMTransition.UpdateProgress(++Progress / Total, ScriptLocalization.Interactions.GeneratingDungeon + " 13");
    yield return (object) biomeGenerator.StartCoroutine((IEnumerator) biomeGenerator.InstantiatePrefabs());
    biomeGenerator.InitMusic();
    MMTransition.UpdateProgress(++Progress / Total, ScriptLocalization.Interactions.GeneratingDungeon + " 14");
    if ((double) stopwatch.ElapsedMilliseconds > 0.01666666753590107)
    {
      stopwatch.Reset();
      yield return (object) null;
    }
    MMTransition.UpdateProgress(++Progress / Total, ScriptLocalization.Interactions.GeneratingDungeon + " 15");
    while (LocationManager.GetLocationState(biomeGenerator.DungeonLocation) != LocationState.Active && !GameManager.SandboxDungeonEnabled)
      yield return (object) null;
    biomeGenerator.SetRoom(biomeGenerator.StartX, biomeGenerator.StartY);
    MMTransition.UpdateProgress(++Progress / Total, ScriptLocalization.Interactions.GeneratingDungeon + " 16");
    if ((double) stopwatch.ElapsedMilliseconds > 0.01666666753590107)
    {
      stopwatch.Reset();
      yield return (object) null;
    }
    MMTransition.UpdateProgress(++Progress / Total, ScriptLocalization.Interactions.GeneratingDungeon + " 17");
    MMTransition.UpdateProgress(++Progress / Total, ScriptLocalization.Interactions.GeneratingDungeon + " 18");
    if ((double) stopwatch.ElapsedMilliseconds > 0.01666666753590107)
    {
      stopwatch.Reset();
      yield return (object) null;
    }
    MMTransition.UpdateProgress(++Progress / Total, ScriptLocalization.Interactions.GeneratingDungeon + " 19");
    while (!biomeGenerator.CurrentRoom.generateRoom.GeneratedDecorations)
      yield return (object) null;
    BiomeGenerator.BiomeAction onBiomeGenerated = BiomeGenerator.OnBiomeGenerated;
    if (onBiomeGenerated != null)
      onBiomeGenerated();
    EnableRecieveShadowsSpriteRenderer.UpdateSpriteShadows();
    MMTransition.ResumePlay();
    SimulationManager.UnPause();
    stopwatch.Stop();
  }

  public void Generate()
  {
    this.transform.position = Vector3.zero;
    this.RandomSeed = new System.Random(this.Seed);
    this.CreateRandomWalk();
    this.SetNeighbours();
    this.PlaceEntranceAndExit();
    this.GetCriticalPath();
    if (this.LockAndKey)
      this.PlaceLockAndKey();
    this.PlaceStoryRooms();
    this.PlaceDynamicCustomRooms();
    this.PlaceFixedCustomRooms();
    this.StartCoroutine((IEnumerator) this.InstantiatePrefabs());
    LocationManager.LocationManagers[this.DungeonLocation].SpawnFollowers();
    this.SetRoom(this.StartX, this.StartY);
    BiomeGenerator.BiomeAction onBiomeGenerated = BiomeGenerator.OnBiomeGenerated;
    if (onBiomeGenerated == null)
      return;
    onBiomeGenerated();
  }

  private void CreateRandomWalk()
  {
    this.Rooms = new List<BiomeRoom>();
    int num1 = this.NumberOfRooms * Mathf.RoundToInt(DifficultyManager.GetDungeonRoomsMultiplier());
    int x1 = 0;
    int y1 = 0;
    BiomeRoom NewRoom;
    if (this.OverrideRandomWalk)
    {
      this.StartX = 0;
      this.StartY = 0;
      foreach (BiomeGenerator.OverrideRoom overrideRoom in this.OverrideRooms)
      {
        NewRoom = new BiomeRoom(overrideRoom.x, overrideRoom.y, this.RandomSeed.Next(-2147483647 /*0x80000001*/, int.MaxValue), this.GeneratorRoomPrefab);
        this.Rooms.Add(NewRoom);
        if (overrideRoom.North != GenerateRoom.ConnectionTypes.False)
          NewRoom.N_Room = new RoomConnection(overrideRoom.North);
        if (overrideRoom.East != GenerateRoom.ConnectionTypes.False)
          NewRoom.E_Room = new RoomConnection(overrideRoom.East);
        if (overrideRoom.South != GenerateRoom.ConnectionTypes.False)
          NewRoom.S_Room = new RoomConnection(overrideRoom.South);
        if (overrideRoom.West != GenerateRoom.ConnectionTypes.False)
          NewRoom.W_Room = new RoomConnection(overrideRoom.West);
        NewRoom.Active = overrideRoom.RoomActive;
        NewRoom.IsCustom = true;
        NewRoom.Generated = overrideRoom.Generated == BiomeGenerator.FixedRoom.Generate.DontGenerate;
        Addressables.LoadAssetAsync<GameObject>((object) overrideRoom.PrefabPath).Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
        {
          this.loadedAddressableAssets.Add(obj);
          NewRoom.GameObject = obj.Result;
        });
        NewRoom.GameObjectPath = overrideRoom.PrefabPath;
      }
      foreach (BiomeRoom room1 in this.Rooms)
      {
        BiomeRoom room2;
        if (room1.N_Room != null && (room2 = BiomeRoom.GetRoom(room1.x, room1.y + 1)) != null)
        {
          room1.N_Room.SetConnectionAndRoom(room2, room1.N_Room.ConnectionType);
          room2.S_Room = new RoomConnection(room1);
          room2.S_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
        }
        BiomeRoom room3;
        if (room1.E_Room != null && (room3 = BiomeRoom.GetRoom(room1.x + 1, room1.y)) != null)
        {
          room1.E_Room.SetConnectionAndRoom(room3, room1.E_Room.ConnectionType);
          room3.W_Room = new RoomConnection(room1);
          room3.W_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
        }
        BiomeRoom room4;
        if (room1.S_Room != null && (room4 = BiomeRoom.GetRoom(room1.x, room1.y - 1)) != null)
        {
          room1.S_Room.SetConnectionAndRoom(room4, room1.S_Room.ConnectionType);
          room4.N_Room = new RoomConnection(room1);
          room4.N_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
        }
        BiomeRoom room5;
        if (room1.W_Room != null && (room5 = BiomeRoom.GetRoom(room1.x - 1, room1.y)) != null)
        {
          room1.W_Room.SetConnectionAndRoom(room5, room1.W_Room.ConnectionType);
          room5.E_Room = new RoomConnection(room1);
          room5.E_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
        }
      }
    }
    else
    {
      this.Rooms.Add(new BiomeRoom(x1, y1, this.RandomSeed.Next(-2147483647 /*0x80000001*/, int.MaxValue), this.GeneratorRoomPrefab));
      int num2 = 0;
      while (num1 > 0)
      {
        BiomeRoom room = this.Rooms[this.RandomSeed.Next(0, this.Rooms.Count)];
        int x2 = room.x;
        int y2 = room.y;
        int num3 = this.RandomSeed.Next(0, 4);
        NewRoom = (BiomeRoom) null;
        switch (num3)
        {
          case 0:
            if (BiomeRoom.GetRoom(x2, y2 + 1) == null)
            {
              NewRoom = new BiomeRoom(x2, y2 + 1, this.RandomSeed.Next(-2147483647 /*0x80000001*/, int.MaxValue), this.GeneratorRoomPrefab);
              this.Rooms.Add(NewRoom);
              room.N_Room = new RoomConnection(NewRoom);
              NewRoom.S_Room = new RoomConnection(room);
              room.N_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
              NewRoom.S_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
              break;
            }
            break;
          case 1:
            if (BiomeRoom.GetRoom(x2 + 1, y2) == null)
            {
              NewRoom = new BiomeRoom(x2 + 1, y2, this.RandomSeed.Next(-2147483647 /*0x80000001*/, int.MaxValue), this.GeneratorRoomPrefab);
              this.Rooms.Add(NewRoom);
              room.E_Room = new RoomConnection(NewRoom);
              NewRoom.W_Room = new RoomConnection(room);
              room.E_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
              NewRoom.W_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
              break;
            }
            break;
          case 2:
            if (BiomeRoom.GetRoom(x2, y2 - 1) == null)
            {
              NewRoom = new BiomeRoom(x2, y2 - 1, this.RandomSeed.Next(-2147483647 /*0x80000001*/, int.MaxValue), this.GeneratorRoomPrefab);
              this.Rooms.Add(NewRoom);
              room.S_Room = new RoomConnection(NewRoom);
              NewRoom.N_Room = new RoomConnection(room);
              room.S_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
              NewRoom.N_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
              break;
            }
            break;
          case 3:
            if (BiomeRoom.GetRoom(x2 - 1, y2) == null)
            {
              NewRoom = new BiomeRoom(x2 - 1, y2, this.RandomSeed.Next(-2147483647 /*0x80000001*/, int.MaxValue), this.GeneratorRoomPrefab);
              this.Rooms.Add(NewRoom);
              room.W_Room = new RoomConnection(NewRoom);
              NewRoom.E_Room = new RoomConnection(room);
              room.W_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
              NewRoom.E_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
              break;
            }
            break;
        }
        if (NewRoom != null)
          --num1;
        else
          ++num2;
      }
    }
  }

  private void ResetFloodDistance()
  {
    foreach (BiomeRoom room in this.Rooms)
      room.Distance = int.MaxValue;
  }

  private void FloodFillDistance(BiomeRoom r, int Distance)
  {
    if (r == null || r.Distance < Distance + 1)
      return;
    r.Distance = ++Distance;
    if (r.N_Room.Connected)
      this.FloodFillDistance(r.N_Room.Room, Distance);
    if (r.E_Room.Connected)
      this.FloodFillDistance(r.E_Room.Room, Distance);
    if (r.S_Room.Connected)
      this.FloodFillDistance(r.S_Room.Room, Distance);
    if (!r.W_Room.Connected)
      return;
    this.FloodFillDistance(r.W_Room.Room, Distance);
  }

  private void PlaceEntranceAndExit()
  {
    if (this.OverrideRandomWalk)
      return;
    List<BiomeGenerator.RoomEntranceExit> roomEntranceExitList1 = new List<BiomeGenerator.RoomEntranceExit>();
    int num1 = 0;
    int Layer = -1;
    while (++Layer <= 4)
    {
      if (DataManager.HasKeyPieceFromLocation(this.DungeonLocation, Layer))
        ++num1;
    }
    bool flag1 = num1 < 4;
    bool flag2 = false;
    foreach (BiomeRoom room in this.Rooms)
    {
      if (flag2)
      {
        if (room.NumConnections == 1 && room.N_Room.Connected && room.EmptyNeighbourPositions.Count > 0 && room.DoAnyOfMyEmptyNeighboursHaveEmptyNeighbours())
          roomEntranceExitList1.Add(new BiomeGenerator.RoomEntranceExit(room, false));
        if (room.NumConnections == 1 && room.E_Room.Connected && room.EmptyNeighbourPositions.Count > 0 && room.DoAnyOfMyEmptyNeighboursHaveEmptyNeighbours())
          roomEntranceExitList1.Add(new BiomeGenerator.RoomEntranceExit(room, false));
        if (room.NumConnections == 1 && room.S_Room.Connected && room.EmptyNeighbourPositions.Count > 0 && room.DoAnyOfMyEmptyNeighboursHaveEmptyNeighbours())
          roomEntranceExitList1.Add(new BiomeGenerator.RoomEntranceExit(room, false));
        if (room.NumConnections == 1 && room.W_Room.Connected && room.EmptyNeighbourPositions.Count > 0 && room.DoAnyOfMyEmptyNeighboursHaveEmptyNeighbours())
          roomEntranceExitList1.Add(new BiomeGenerator.RoomEntranceExit(room, false));
      }
      else
      {
        if (room.NumConnections == 1 && room.N_Room.Connected && room.EmptyNeighbourPositionsIgnoreSouth.Count > 0)
          roomEntranceExitList1.Add(new BiomeGenerator.RoomEntranceExit(room, false));
        if (room.NumConnections == 1 && room.E_Room.Connected && room.EmptyNeighbourPositionsIgnoreSouth.Count > 0)
          roomEntranceExitList1.Add(new BiomeGenerator.RoomEntranceExit(room, false));
        if (room.NumConnections == 1 && room.S_Room.Connected && room.EmptyNeighbourPositionsIgnoreSouth.Count > 0)
          roomEntranceExitList1.Add(new BiomeGenerator.RoomEntranceExit(room, false));
        if (room.NumConnections == 1 && room.W_Room.Connected && room.EmptyNeighbourPositionsIgnoreSouth.Count > 0)
          roomEntranceExitList1.Add(new BiomeGenerator.RoomEntranceExit(room, false));
      }
    }
    List<BiomeGenerator.RoomEntranceExit> roomEntranceExitList2 = new List<BiomeGenerator.RoomEntranceExit>();
    foreach (BiomeRoom room in this.Rooms)
    {
      if (room.NumConnections == 1 && room.N_Room.Connected)
        roomEntranceExitList2.Add(new BiomeGenerator.RoomEntranceExit(room, false));
      if (room.NumConnections == 1 && room.E_Room.Connected)
        roomEntranceExitList2.Add(new BiomeGenerator.RoomEntranceExit(room, false));
      if (room.NumConnections == 1 && room.S_Room.Connected)
        roomEntranceExitList2.Add(new BiomeGenerator.RoomEntranceExit(room, false));
      if (room.NumConnections == 1 && room.W_Room.Connected)
        roomEntranceExitList2.Add(new BiomeGenerator.RoomEntranceExit(room, false));
    }
    BiomeGenerator.RoomEntranceExit ConnectionRoom = (BiomeGenerator.RoomEntranceExit) null;
    BiomeGenerator.RoomEntranceExit roomEntranceExit1 = (BiomeGenerator.RoomEntranceExit) null;
    int num2 = 0;
    foreach (BiomeGenerator.RoomEntranceExit roomEntranceExit2 in roomEntranceExitList1)
    {
      this.ResetFloodDistance();
      this.FloodFillDistance(roomEntranceExit2.Room, 0);
      foreach (BiomeGenerator.RoomEntranceExit roomEntranceExit3 in roomEntranceExitList2)
      {
        if (num2 < roomEntranceExit3.Room.Distance && roomEntranceExit2 != roomEntranceExit3)
        {
          num2 = roomEntranceExit3.Room.Distance;
          ConnectionRoom = roomEntranceExit2;
          roomEntranceExit1 = roomEntranceExit3;
        }
      }
    }
    BiomeGenerator.WeaponAtEnd = GameManager.CurrentDungeonFloor > 1 && this.RandomSeed.NextDouble() < 0.64999997615814209 && MapManager.Instance.CurrentNode.nodeType != NodeType.MiniBossFloor;
    if ((UnityEngine.Object) MapManager.Instance != (UnityEngine.Object) null && MapManager.Instance.CurrentNode != null && MapManager.Instance.CurrentNode.nodeType == NodeType.MiniBossFloor && !DataManager.Instance.DungeonBossFight || GameManager.SandboxDungeonEnabled)
    {
      ConnectionRoom.Room.IsCustom = true;
      ConnectionRoom.Room.GameObjectPath = this.BossRoomPath;
      if (ConnectionRoom.Room.N_Room.Connected)
        ConnectionRoom.Room.N_Room.Room.S_Room.SetConnection(GenerateRoom.ConnectionTypes.Boss);
      if (ConnectionRoom.Room.E_Room.Connected)
        ConnectionRoom.Room.E_Room.Room.W_Room.SetConnection(GenerateRoom.ConnectionTypes.Boss);
      if (ConnectionRoom.Room.S_Room.Connected)
        ConnectionRoom.Room.S_Room.Room.N_Room.SetConnection(GenerateRoom.ConnectionTypes.Boss);
      if (ConnectionRoom.Room.W_Room.Connected)
        ConnectionRoom.Room.W_Room.Room.E_Room.SetConnection(GenerateRoom.ConnectionTypes.Boss);
      ConnectionRoom.Room.IsBoss = true;
      ConnectionRoom.Room.Generated = false;
      Vector2Int vector2Int = new Vector2Int(ConnectionRoom.Room.x, ConnectionRoom.Room.y);
      this.RoomExit = ConnectionRoom.Room;
      if (!string.IsNullOrEmpty(this.KeyPiecePath))
      {
        BiomeRoom Room = this.PlacePostBossRoom(ConnectionRoom.Room.EmptyNeighbourPositions, this.KeyPiecePath, ConnectionRoom);
        this.PlacePostBossRoom(Room.EmptyNeighbourPositionsIgnoreSouth, this.PostBossRoomPath, new BiomeGenerator.RoomEntranceExit(Room, false)).N_Room.SetConnection(GenerateRoom.ConnectionTypes.LeaderBoss);
      }
      else if (!GameManager.SandboxDungeonEnabled && MapManager.Instance.CurrentNode == MapManager.Instance.CurrentMap.GetBossNode())
      {
        BiomeRoom biomeRoom = this.PlacePostBossRoom(ConnectionRoom.Room.EmptyNeighbourPositionsIgnoreSouth, DataManager.Instance.DungeonCompleted(this.DungeonLocation) ? this.PostBossRoomPath : this.BossDoorRoomPath, ConnectionRoom);
        if (GameManager.DungeonEndlessLevel >= 3 && DataManager.Instance.DungeonCompleted(this.DungeonLocation))
          biomeRoom.N_Room.SetConnection(GenerateRoom.ConnectionTypes.False);
        else
          biomeRoom.N_Room.SetConnection(DataManager.Instance.DungeonCompleted(this.DungeonLocation) ? GenerateRoom.ConnectionTypes.NextLayer : GenerateRoom.ConnectionTypes.LeaderBoss);
        this.lastRoom = biomeRoom;
      }
      else
      {
        BiomeRoom biomeRoom = this.PlacePostBossRoom(ConnectionRoom.Room.EmptyNeighbourPositionsIgnoreSouth, this.EndOfFloorRoomPath, ConnectionRoom);
        biomeRoom.N_Room.SetConnection(GenerateRoom.ConnectionTypes.NextLayer);
        this.lastRoom = biomeRoom;
      }
    }
    else
    {
      GenerateRoom.ConnectionTypes ConnectionType = GenerateRoom.ConnectionTypes.NextLayer;
      string RoomToPlace = this.EndOfFloorRoomPath;
      if ((UnityEngine.Object) MapManager.Instance != (UnityEngine.Object) null && MapManager.Instance.CurrentNode != null && MapManager.Instance.CurrentNode.nodeType == NodeType.MiniBossFloor && DataManager.Instance.DungeonBossFight)
      {
        ConnectionRoom.Room.GameObjectPath = this.BossDoorRoomPath;
        ConnectionType = GenerateRoom.ConnectionTypes.LeaderBoss;
        RoomToPlace = this.BossDoorRoomPath;
      }
      ConnectionRoom.Room.IsCustom = false;
      ConnectionRoom.Room.Generated = false;
      this.RoomExit = ConnectionRoom.Room;
      BiomeRoom biomeRoom = this.PlacePostBossRoom(ConnectionRoom.Room.EmptyNeighbourPositionsIgnoreSouth, RoomToPlace, ConnectionRoom);
      biomeRoom.N_Room.SetConnection(ConnectionType);
      biomeRoom.HasWeapon = BiomeGenerator.WeaponAtEnd;
      this.lastRoom = biomeRoom;
    }
    if (!string.IsNullOrEmpty(this.EntranceRoomPath))
    {
      roomEntranceExit1.Room.IsCustom = true;
      roomEntranceExit1.Room.GameObjectPath = this.EntranceRoomPath;
    }
    else
    {
      roomEntranceExit1.Room.IsCustom = false;
      roomEntranceExit1.Room.GameObject = this.GeneratorRoomPrefab;
    }
    this.StartX = roomEntranceExit1.Room.x;
    this.StartY = roomEntranceExit1.Room.y;
    this.RoomEntrance = roomEntranceExit1.Room;
    roomEntranceExit1.Room.Active = false;
    if (!roomEntranceExit1.Room.S_Room.Connected)
      roomEntranceExit1.Room.S_Room.SetConnection(GenerateRoom.ConnectionTypes.Entrance);
    else if (!roomEntranceExit1.Room.W_Room.Connected)
      roomEntranceExit1.Room.W_Room.SetConnection(GenerateRoom.ConnectionTypes.Entrance);
    else if (!roomEntranceExit1.Room.E_Room.Connected)
      roomEntranceExit1.Room.E_Room.SetConnection(GenerateRoom.ConnectionTypes.Entrance);
    else if (!roomEntranceExit1.Room.N_Room.Connected)
      roomEntranceExit1.Room.N_Room.SetConnection(GenerateRoom.ConnectionTypes.Entrance);
    if (this.StartWithBossRoomDoor && GameManager.CurrentDungeonFloor == 1)
    {
      BiomeRoom biomeRoom = new BiomeRoom(-999, -999, this.RandomSeed.Next(-2147483647 /*0x80000001*/, int.MaxValue), this.GeneratorRoomPrefab);
      this.Rooms.Add(biomeRoom);
      biomeRoom.GameObjectPath = this.BossDoorRoomPath;
      biomeRoom.Active = true;
      biomeRoom.IsCustom = true;
      biomeRoom.Generated = false;
      this.StartX = biomeRoom.x;
      this.StartY = biomeRoom.y;
      biomeRoom.N_Room = new RoomConnection(GenerateRoom.ConnectionTypes.Exit);
      biomeRoom.E_Room = new RoomConnection(GenerateRoom.ConnectionTypes.DungeonFirstRoom);
      biomeRoom.S_Room = new RoomConnection(GenerateRoom.ConnectionTypes.Entrance);
      biomeRoom.W_Room = new RoomConnection(GenerateRoom.ConnectionTypes.False);
    }
    if (string.IsNullOrEmpty(this.LeaderRoomPath) || !((UnityEngine.Object) MapManager.Instance != (UnityEngine.Object) null) || MapManager.Instance.CurrentNode == null)
      return;
    BiomeRoom biomeRoom1 = this.PlacePostBossRoom(new List<Vector2Int>()
    {
      BiomeGenerator.BossCoords
    }, this.LeaderRoomPath, ConnectionRoom, false);
    biomeRoom1.S_Room.SetConnectionAndRoom(ConnectionRoom.Room, GenerateRoom.ConnectionTypes.False);
    biomeRoom1.N_Room.ConnectionType = GenerateRoom.ConnectionTypes.DoorRoom;
    biomeRoom1.Generated = true;
    biomeRoom1.IsBoss = true;
  }

  private BiomeRoom PlacePostBossRoom(
    List<Vector2Int> EmptyNeighbourPositions,
    string RoomToPlace,
    BiomeGenerator.RoomEntranceExit ConnectionRoom,
    bool setConnections = true)
  {
    Vector2Int neighbourPosition = EmptyNeighbourPositions[this.RandomSeed.Next(0, EmptyNeighbourPositions.Count)];
    BiomeRoom b = new BiomeRoom(neighbourPosition.x, neighbourPosition.y, this.RandomSeed.Next(-2147483647 /*0x80000001*/, int.MaxValue), (GameObject) null);
    this.Rooms.Add(b);
    this.SetNeighbours(b);
    b.IsCustom = true;
    b.Generated = false;
    b.GameObjectPath = RoomToPlace;
    if (setConnections)
    {
      if (neighbourPosition.y > ConnectionRoom.Room.y)
      {
        b.S_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
        b.S_Room.Room.N_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
        b.S_Room.Room.N_Room.Room = b;
      }
      else if (neighbourPosition.y < ConnectionRoom.Room.y)
      {
        b.N_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
        b.N_Room.Room.S_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
        b.N_Room.Room.S_Room.Room = b;
      }
      else if (neighbourPosition.x > ConnectionRoom.Room.x)
      {
        b.W_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
        b.W_Room.Room.E_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
        b.W_Room.Room.E_Room.Room = b;
      }
      else if (neighbourPosition.x < ConnectionRoom.Room.x)
      {
        b.E_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
        b.E_Room.Room.W_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
        b.E_Room.Room.W_Room.Room = b;
      }
    }
    return b;
  }

  public Vector2Int GetBossRoom()
  {
    foreach (BiomeRoom room in this.Rooms)
    {
      if (room.IsBoss)
        return new Vector2Int(room.x, room.y);
    }
    return new Vector2Int(0, 0);
  }

  public Vector2Int GetLastRoom() => new Vector2Int(this.lastRoom.x, this.lastRoom.y);

  private void GetCriticalPath()
  {
    if (this.OverrideRandomWalk)
      return;
    this.ResetFloodDistance();
    this.FloodFillDistance(this.RoomEntrance, 0);
    int distance = this.RoomExit.Distance;
    BiomeRoom biomeRoom = this.RoomExit;
    this.CriticalPath = new List<BiomeRoom>();
    this.CriticalPath.Add(biomeRoom);
    while (--distance > 0)
    {
      if (biomeRoom.N_Room.Connected && biomeRoom.N_Room.Room.Distance == distance)
      {
        biomeRoom = biomeRoom.N_Room.Room;
        biomeRoom.CriticalPathDirection = BiomeRoom.Direction.South;
      }
      if (biomeRoom.E_Room.Connected && biomeRoom.E_Room.Room.Distance == distance)
      {
        biomeRoom = biomeRoom.E_Room.Room;
        biomeRoom.CriticalPathDirection = BiomeRoom.Direction.West;
      }
      if (biomeRoom.S_Room.Connected && biomeRoom.S_Room.Room.Distance == distance)
      {
        biomeRoom = biomeRoom.S_Room.Room;
        biomeRoom.CriticalPathDirection = BiomeRoom.Direction.North;
      }
      if (biomeRoom.W_Room.Connected && biomeRoom.W_Room.Room.Distance == distance)
      {
        biomeRoom = biomeRoom.W_Room.Room;
        biomeRoom.CriticalPathDirection = BiomeRoom.Direction.East;
      }
      this.CriticalPath.Add(biomeRoom);
    }
  }

  private void PlaceLockAndKey()
  {
    if (string.IsNullOrEmpty(this.LockedRoomPath) || string.IsNullOrEmpty(this.KeyRoomPath) || this.OverrideRandomWalk)
      return;
    this.ResetFloodDistance();
    this.FloodFillDistance(this.RoomEntrance, 0);
    int distance1 = this.RoomExit.Distance;
    BiomeRoom biomeRoom1 = this.RoomExit;
    this.CriticalPath = new List<BiomeRoom>();
    this.CriticalPath.Add(biomeRoom1);
    while (--distance1 > 0)
    {
      if (biomeRoom1.N_Room.Connected && biomeRoom1.N_Room.Room.Distance == distance1)
      {
        biomeRoom1 = biomeRoom1.N_Room.Room;
        biomeRoom1.CriticalPathDirection = BiomeRoom.Direction.South;
      }
      if (biomeRoom1.E_Room.Connected && biomeRoom1.E_Room.Room.Distance == distance1)
      {
        biomeRoom1 = biomeRoom1.E_Room.Room;
        biomeRoom1.CriticalPathDirection = BiomeRoom.Direction.West;
      }
      if (biomeRoom1.S_Room.Connected && biomeRoom1.S_Room.Room.Distance == distance1)
      {
        biomeRoom1 = biomeRoom1.S_Room.Room;
        biomeRoom1.CriticalPathDirection = BiomeRoom.Direction.North;
      }
      if (biomeRoom1.W_Room.Connected && biomeRoom1.W_Room.Room.Distance == distance1)
      {
        biomeRoom1 = biomeRoom1.W_Room.Room;
        biomeRoom1.CriticalPathDirection = BiomeRoom.Direction.East;
      }
      this.CriticalPath.Add(biomeRoom1);
    }
    float num = (float) int.MinValue;
    BiomeRoom biomeRoom2 = (BiomeRoom) null;
    foreach (BiomeRoom room in this.Rooms)
    {
      if (!room.IsCustom && (double) room.Distance > (double) num && !this.CriticalPath.Contains(room))
      {
        biomeRoom2 = room;
        num = (float) room.Distance;
      }
    }
    biomeRoom2.IsCustom = true;
    biomeRoom2.GameObjectPath = this.KeyRoomPath;
    BiomeRoom biomeRoom3 = biomeRoom2;
    int distance2 = biomeRoom3.Distance;
    bool flag = true;
    while (flag && !this.CriticalPath.Contains(biomeRoom3))
    {
      if (biomeRoom3.N_Room.Connected && biomeRoom3.N_Room.Room.Distance < distance2)
      {
        biomeRoom3 = biomeRoom3.N_Room.Room;
        distance2 = biomeRoom3.Distance;
      }
      else if (biomeRoom3.E_Room.Connected && biomeRoom3.E_Room.Room.Distance < distance2)
      {
        biomeRoom3 = biomeRoom3.E_Room.Room;
        distance2 = biomeRoom3.Distance;
      }
      else if (biomeRoom3.S_Room.Connected && biomeRoom3.S_Room.Room.Distance < distance2)
      {
        biomeRoom3 = biomeRoom3.S_Room.Room;
        distance2 = biomeRoom3.Distance;
      }
      else if (biomeRoom3.W_Room.Connected && biomeRoom3.W_Room.Room.Distance < distance2)
      {
        biomeRoom3 = biomeRoom3.W_Room.Room;
        distance2 = biomeRoom3.Distance;
      }
    }
    int index = this.RandomSeed.Next(1, this.CriticalPath.IndexOf(biomeRoom3));
    this.CriticalPath[index].IsCustom = true;
    this.CriticalPath[index].GameObjectPath = this.LockedRoomPath;
  }

  private void PlaceRespawnRoom()
  {
    if (this.RespawnRoom != null)
      return;
    this.RespawnRoom = new BiomeRoom(BiomeGenerator.RespawnRoomCoords.x, BiomeGenerator.RespawnRoomCoords.y, this.RandomSeed.Next(-2147483647 /*0x80000001*/, int.MaxValue), (GameObject) null);
    this.Rooms.Add(this.RespawnRoom);
    this.RespawnRoom.IsCustom = true;
    this.RespawnRoom.GameObjectPath = this.RespawnRoomPath;
    this.RespawnRoom.Generated = true;
    this.RespawnRoom.IsRespawnRoom = true;
  }

  private void PlaceDeathCatRoom()
  {
    if (this.DeathCatRoom != null)
      return;
    this.DeathCatRoom = new BiomeRoom(int.MaxValue, int.MaxValue, this.RandomSeed.Next(-2147483647 /*0x80000001*/, int.MaxValue), (GameObject) null);
    this.Rooms.Add(this.DeathCatRoom);
    this.DeathCatRoom.IsCustom = true;
    this.DeathCatRoom.GameObjectPath = this.DeathCatRoomPath;
    this.DeathCatRoom.Generated = true;
    this.DeathCatRoom.IsDeathCatRoom = true;
  }

  private IEnumerator PlaceStoryRooms()
  {
    if (!this.OverrideRandomWalk)
    {
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      this.RandomiseStoryOrder = new List<BiomeGenerator.ListOfStoryRooms>();
      int count = this.StoryRooms.Count;
      while (this.RandomiseStoryOrder.Count < count)
      {
        int index = this.RandomSeed.Next(0, this.StoryRooms.Count);
        this.RandomiseStoryOrder.Add(this.StoryRooms[index]);
        this.StoryRooms.Remove(this.StoryRooms[index]);
      }
      this.StoryRooms = new List<BiomeGenerator.ListOfStoryRooms>((IEnumerable<BiomeGenerator.ListOfStoryRooms>) this.RandomiseStoryOrder);
      int i = -1;
      while (++i < this.StoryRooms.Count)
      {
        BiomeGenerator.ListOfStoryRooms storyRoom = this.StoryRooms[i];
        if (GameManager.CurrentDungeonLayer - 1 == DataManager.Instance.GetVariableInt(storyRoom.StoryVariable) && DataManager.Instance.GetVariableInt(storyRoom.LastRun) < DataManager.Instance.dungeonRun && !DataManager.Instance.GetVariable(storyRoom.DungeonBeaten) && DataManager.Instance.GetVariableInt(storyRoom.StoryVariable) < storyRoom.Rooms.Count && !string.IsNullOrEmpty(storyRoom.Rooms[DataManager.Instance.GetVariableInt(storyRoom.StoryVariable)].RoomPath))
        {
          int num1 = this.RandomSeed.Next(0, 10);
          int variableInt = DataManager.Instance.GetVariableInt(storyRoom.StoryVariable);
          if (GameManager.CurrentDungeonFloor != 1 || num1 >= 5 && storyRoom.Rooms[variableInt].FloorOne || !storyRoom.Rooms[variableInt].FloorTwo && !storyRoom.Rooms[variableInt].FloorThree)
          {
            string roomPath = storyRoom.Rooms[DataManager.Instance.GetVariableInt(storyRoom.StoryVariable)].RoomPath;
            this.AvailableRooms = new List<BiomeRoom>();
            if (storyRoom.PutOnCriticalPath)
            {
              foreach (BiomeRoom biomeRoom in this.CriticalPath)
              {
                if (!biomeRoom.IsCustom)
                  this.AvailableRooms.Add(biomeRoom);
              }
            }
            if (!storyRoom.PutOnCriticalPath || storyRoom.PutOnCriticalPath && this.AvailableRooms.Count <= 0)
            {
              int num2 = 0;
              while (this.AvailableRooms.Count <= 0 && ++num2 <= 4)
              {
                foreach (BiomeRoom room in this.Rooms)
                {
                  if (!room.IsCustom && room.NumConnections == num2)
                    this.AvailableRooms.Add(room);
                }
              }
            }
            BiomeRoom availableRoom = this.AvailableRooms[this.RandomSeed.Next(0, this.AvailableRooms.Count)];
            availableRoom.IsCustom = true;
            availableRoom.GameObjectPath = roomPath;
            MMTransition.UpdateProgress((float) i / (float) this.CustomDynamicRooms.Count, ScriptLocalization.Interactions.GeneratingDungeon);
            if ((double) stopwatch.ElapsedMilliseconds > 0.01666666753590107)
            {
              stopwatch.Reset();
              yield return (object) null;
            }
          }
        }
      }
      stopwatch.Stop();
    }
  }

  private IEnumerator PlaceDynamicCustomRooms()
  {
    bool spawnedWeaponRoom = false;
    Stopwatch stopwatch = new Stopwatch();
    stopwatch.Start();
    if (!this.OverrideRandomWalk)
    {
      this.RandomiseOrder = new List<BiomeGenerator.ListOfCustomRoomPrefabs>();
      int count = this.CustomDynamicRooms.Count;
      while (this.RandomiseOrder.Count < count)
      {
        int index = this.RandomSeed.Next(0, this.CustomDynamicRooms.Count);
        this.RandomiseOrder.Add(this.CustomDynamicRooms[index]);
        this.CustomDynamicRooms.Remove(this.CustomDynamicRooms[index]);
      }
      this.CustomDynamicRooms = new List<BiomeGenerator.ListOfCustomRoomPrefabs>((IEnumerable<BiomeGenerator.ListOfCustomRoomPrefabs>) this.RandomiseOrder);
      int i = -1;
      while (++i < this.CustomDynamicRooms.Count)
      {
        BiomeGenerator.ListOfCustomRoomPrefabs customDynamicRoom = this.CustomDynamicRooms[i];
        bool flag1 = false;
        foreach (BiomeGenerator.VariableAndCondition conditionalVariable in customDynamicRoom.ConditionalVariables)
        {
          if (DataManager.Instance.GetVariable(conditionalVariable.Variable) != conditionalVariable.Condition)
            flag1 = true;
        }
        if (!flag1)
        {
          bool flag2 = false;
          foreach (FollowerLocation followerLocation in customDynamicRoom.LocationIsUndiscovered)
          {
            if (DataManager.Instance.DiscoveredLocations.Contains(followerLocation))
              flag2 = true;
          }
          if (!flag2)
          {
            bool flag3 = false;
            if (customDynamicRoom.MinimumRun && DataManager.Instance.dungeonRun < customDynamicRoom.MinimumRunNumber)
              flag3 = true;
            if (!flag3)
            {
              bool flag4 = false;
              if (customDynamicRoom.MaximumRun && DataManager.Instance.dungeonRun > customDynamicRoom.MaximumRunNumber)
                flag4 = true;
              if (!flag4)
              {
                bool flag5 = false;
                if (customDynamicRoom.MinimumFollowerCount && DataManager.Instance.Followers.Count < customDynamicRoom.MinimumFollowerCountNumber)
                  flag5 = true;
                if (!flag5)
                {
                  bool flag6 = false;
                  if (customDynamicRoom.MaximumFollowerCount && DataManager.Instance.Followers.Count > customDynamicRoom.MaximumFollowerCountNumber)
                    flag6 = true;
                  if (!flag6 && this.RandomSeed.NextDouble() <= (double) customDynamicRoom.Probability && customDynamicRoom.AvailableOnLayer() && customDynamicRoom.AvailableOnFoor())
                  {
                    if (customDynamicRoom.UseStaticRoomList)
                    {
                      customDynamicRoom.RoomPaths = customDynamicRoom.RoomPaths.Union<string>((IEnumerable<string>) BiomeGenerator.ListOfCustomRoomPrefabs.StaticRoomList).ToList<string>();
                      if (DoctrineUpgradeSystem.TrySermonsStillAvailable() && DoctrineUpgradeSystem.TryGetStillDoctrineStone() && DataManager.Instance.FirstDoctrineStone)
                        customDynamicRoom.RoomPaths.Add("Assets/_Rooms/Reward Doctrine Room.prefab");
                      if (!DataManager.Instance.HaroConversationCompleted)
                        customDynamicRoom.RoomPaths.Add("Assets/_Rooms/Lore Haro.prefab");
                    }
                    string roomPath = customDynamicRoom.RoomPaths[this.RandomSeed.Next(0, customDynamicRoom.RoomPaths.Count)];
                    this.AvailableRooms = new List<BiomeRoom>();
                    if (customDynamicRoom.PutOnCriticalPath)
                    {
                      foreach (BiomeRoom biomeRoom in this.CriticalPath)
                      {
                        if (!biomeRoom.IsCustom)
                          this.AvailableRooms.Add(biomeRoom);
                      }
                    }
                    if (!customDynamicRoom.PutOnCriticalPath || customDynamicRoom.PutOnCriticalPath && this.AvailableRooms.Count <= 0)
                    {
                      int num = 0;
                      while (this.AvailableRooms.Count <= 0 && ++num <= 4)
                      {
                        foreach (BiomeRoom room in this.Rooms)
                        {
                          if (customDynamicRoom.RemoveIfNoNonCriticalPath)
                          {
                            if (!room.IsCustom && room.NumConnections == num && !this.CriticalPath.Contains(room))
                              this.AvailableRooms.Add(room);
                          }
                          else if (!room.IsCustom && room.NumConnections == num)
                            this.AvailableRooms.Add(room);
                        }
                      }
                    }
                    if (this.AvailableRooms.Count > 0)
                    {
                      BiomeRoom availableRoom = this.AvailableRooms[this.RandomSeed.Next(0, this.AvailableRooms.Count)];
                      availableRoom.GameObjectPath = roomPath;
                      if (availableRoom.GameObjectPath == "Assets/_Rooms/Reward Room Tarot.prefab")
                      {
                        if (GameManager.CurrentDungeonFloor > 1 && this.RandomSeed.Next(0, 100) >= 65 && DataManager.Instance.WeaponPool.Count + DataManager.Instance.CursePool.Count > 3 && !spawnedWeaponRoom)
                        {
                          spawnedWeaponRoom = true;
                          availableRoom.GameObjectPath = "Assets/_Rooms/Marketplace Weapons.prefab";
                          customDynamicRoom.ConnectionType = GenerateRoom.ConnectionTypes.WeaponShop;
                          UnityEngine.Debug.Log((object) "2".Colour(Color.yellow));
                          foreach (BiomeRoom room in this.Rooms)
                            room.HasWeapon = false;
                        }
                        else if (DataManager.Instance.PlayerFleece != 4)
                        {
                          customDynamicRoom.ConnectionType = GenerateRoom.ConnectionTypes.Tarot;
                          availableRoom.GameObjectPath = roomPath;
                        }
                        else
                          continue;
                      }
                      availableRoom.IsCustom = true;
                      if (customDynamicRoom.SetCustomConnectionType)
                      {
                        UnityEngine.Debug.Log((object) $"{customDynamicRoom.SetCustomConnectionType.ToString()}  {(object) customDynamicRoom.ConnectionType}");
                        if (availableRoom.N_Room.Connected)
                          availableRoom.N_Room.Room.S_Room.SetConnection(customDynamicRoom.ConnectionType);
                        if (availableRoom.E_Room.Connected)
                          availableRoom.E_Room.Room.W_Room.SetConnection(customDynamicRoom.ConnectionType);
                        if (availableRoom.S_Room.Connected)
                          availableRoom.S_Room.Room.N_Room.SetConnection(customDynamicRoom.ConnectionType);
                        if (availableRoom.W_Room.Connected)
                          availableRoom.W_Room.Room.E_Room.SetConnection(customDynamicRoom.ConnectionType);
                      }
                    }
                    MMTransition.UpdateProgress((float) i / (float) this.CustomDynamicRooms.Count, ScriptLocalization.Interactions.GeneratingDungeon);
                    if ((double) stopwatch.ElapsedMilliseconds > 0.01666666753590107)
                    {
                      stopwatch.Reset();
                      yield return (object) null;
                    }
                  }
                }
              }
            }
          }
        }
      }
      List<BiomeRoom> biomeRoomList = new List<BiomeRoom>();
      foreach (BiomeRoom room in this.Rooms)
      {
        if (!room.IsCustom)
          biomeRoomList.Add(room);
      }
      if (!BiomeGenerator.WeaponAtEnd && GameManager.CurrentDungeonFloor > 1 && biomeRoomList.Count > 0 && PlayerFarming.Location != FollowerLocation.IntroDungeon)
        biomeRoomList[this.RandomSeed.Next(0, biomeRoomList.Count)].HasWeapon = true;
      stopwatch.Stop();
    }
  }

  private void PlaceFixedCustomRooms()
  {
    if (this.OverrideRandomWalk)
      return;
    foreach (BiomeGenerator.FixedRoom customRoom in this.CustomRooms)
    {
      bool flag1 = false;
      foreach (BiomeGenerator.VariableAndCondition conditionalVariable in customRoom.ConditionalVariables)
      {
        if (DataManager.Instance.GetVariable(conditionalVariable.Variable) != conditionalVariable.Condition)
          flag1 = true;
      }
      if (!flag1 && this.RandomSeed.NextDouble() <= (double) customRoom.Probability)
      {
        this.AvailableRooms = new List<BiomeRoom>();
        foreach (BiomeRoom room in this.Rooms)
        {
          bool flag2 = true;
          if (room.IsCustom)
            flag2 = false;
          if (room.N_Room.Connected && customRoom.North == BiomeGenerator.FixedRoom.Connection.ForceOff)
            flag2 = false;
          if (room.E_Room.Connected && customRoom.East == BiomeGenerator.FixedRoom.Connection.ForceOff)
            flag2 = false;
          if (room.S_Room.Connected && customRoom.South == BiomeGenerator.FixedRoom.Connection.ForceOff)
            flag2 = false;
          if (room.W_Room.Connected && customRoom.West == BiomeGenerator.FixedRoom.Connection.ForceOff)
            flag2 = false;
          if (!room.N_Room.Connected && customRoom.North == BiomeGenerator.FixedRoom.Connection.ForceOn)
            flag2 = false;
          if (!room.E_Room.Connected && customRoom.East == BiomeGenerator.FixedRoom.Connection.ForceOn)
            flag2 = false;
          if (!room.S_Room.Connected && customRoom.South == BiomeGenerator.FixedRoom.Connection.ForceOn)
            flag2 = false;
          if (!room.W_Room.Connected && customRoom.West == BiomeGenerator.FixedRoom.Connection.ForceOn)
            flag2 = false;
          if (flag2)
            this.AvailableRooms.Add(room);
        }
        if (this.AvailableRooms.Count <= 0 && !customRoom.HasForcedOn && customRoom.HasOptional)
        {
          foreach (BiomeRoom room in this.Rooms)
          {
            int num1 = room.IsCustom ? 1 : 0;
            if (room.N_Room.Connected)
            {
              int north = (int) customRoom.North;
            }
            if (room.E_Room.Connected)
            {
              int east = (int) customRoom.East;
            }
            if (room.S_Room.Connected)
            {
              int south = (int) customRoom.South;
            }
            if (room.W_Room.Connected)
            {
              int west = (int) customRoom.West;
            }
            int num2 = 0;
            if (customRoom.North == BiomeGenerator.FixedRoom.Connection.Optional && room.N_Room.Connected)
              ++num2;
            if (customRoom.East == BiomeGenerator.FixedRoom.Connection.Optional && room.E_Room.Connected)
              ++num2;
            if (customRoom.South == BiomeGenerator.FixedRoom.Connection.Optional && room.S_Room.Connected)
              ++num2;
            if (customRoom.West == BiomeGenerator.FixedRoom.Connection.Optional && room.W_Room.Connected)
              ++num2;
            if (num2 > 0)
              this.AvailableRooms.Add(room);
          }
        }
        if (this.AvailableRooms.Count <= 0)
        {
          if (customRoom.RequiredConnections == 1)
          {
            int num = 0;
            while (this.AvailableRooms.Count <= 0 && ++num <= 3)
            {
              foreach (BiomeRoom room in this.Rooms)
              {
                if (!room.IsCustom && room.NumConnections == num && (customRoom.North != BiomeGenerator.FixedRoom.Connection.ForceOff && room.S_Room.Room == null || customRoom.East != BiomeGenerator.FixedRoom.Connection.ForceOff && room.W_Room.Room == null || customRoom.South != BiomeGenerator.FixedRoom.Connection.ForceOff && room.N_Room.Room == null || customRoom.West != BiomeGenerator.FixedRoom.Connection.ForceOff && room.E_Room.Room == null))
                  this.AvailableRooms.Add(room);
              }
            }
            if (this.AvailableRooms.Count > 0)
            {
              UnityEngine.Debug.Log((object) ("CREATE ! " + customRoom.prefab.name));
              BiomeRoom availableRoom = this.AvailableRooms[this.RandomSeed.Next(0, this.AvailableRooms.Count)];
              if (customRoom.North != BiomeGenerator.FixedRoom.Connection.ForceOff)
                this.CreateCustomRoom(availableRoom.x, availableRoom.y - 1, customRoom.prefab, BiomeGenerator.Direction.North, customRoom.GenerateRoom == BiomeGenerator.FixedRoom.Generate.DontGenerate);
              else if (customRoom.East != BiomeGenerator.FixedRoom.Connection.ForceOff)
                this.CreateCustomRoom(availableRoom.x - 1, availableRoom.y, customRoom.prefab, BiomeGenerator.Direction.East, customRoom.GenerateRoom == BiomeGenerator.FixedRoom.Generate.DontGenerate);
              else if (customRoom.South != BiomeGenerator.FixedRoom.Connection.ForceOff)
                this.CreateCustomRoom(availableRoom.x, availableRoom.y + 1, customRoom.prefab, BiomeGenerator.Direction.South, customRoom.GenerateRoom == BiomeGenerator.FixedRoom.Generate.DontGenerate);
              else if (customRoom.West != BiomeGenerator.FixedRoom.Connection.ForceOff)
                this.CreateCustomRoom(availableRoom.x + 1, availableRoom.y, customRoom.prefab, BiomeGenerator.Direction.West, customRoom.GenerateRoom == BiomeGenerator.FixedRoom.Generate.DontGenerate);
            }
          }
        }
        else
        {
          UnityEngine.Debug.Log((object) ("WORKS! " + customRoom.prefab.name));
          BiomeRoom availableRoom = this.AvailableRooms[this.RandomSeed.Next(0, this.AvailableRooms.Count)];
          availableRoom.IsCustom = true;
          availableRoom.GameObject = customRoom.prefab;
          availableRoom.Generated = customRoom.GenerateRoom == BiomeGenerator.FixedRoom.Generate.DontGenerate;
        }
      }
    }
  }

  private void CreateCustomRoom(
    int x,
    int y,
    GameObject Prefab,
    BiomeGenerator.Direction direction,
    bool Generated)
  {
    BiomeRoom b = new BiomeRoom(x, y, this.RandomSeed.Next(-2147483647 /*0x80000001*/, int.MaxValue), Prefab);
    b.IsCustom = true;
    b.Generated = Generated;
    this.Rooms.Add(b);
    UnityEngine.Debug.Log((object) $"NEW ROOM CREATED: {Prefab.name}  {b.Generated.ToString()}");
    switch (direction)
    {
      case BiomeGenerator.Direction.North:
        this.SetNeighbours(b);
        b.N_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
        b.N_Room.Room.S_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
        break;
      case BiomeGenerator.Direction.East:
        this.SetNeighbours(b);
        b.E_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
        b.E_Room.Room.W_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
        break;
      case BiomeGenerator.Direction.South:
        this.SetNeighbours(b);
        b.S_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
        b.S_Room.Room.N_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
        break;
      case BiomeGenerator.Direction.West:
        this.SetNeighbours(b);
        b.W_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
        b.W_Room.Room.E_Room.SetConnection(GenerateRoom.ConnectionTypes.True);
        break;
    }
  }

  private void SetNeighbours(BiomeRoom b)
  {
    if (b.N_Room == null)
      b.N_Room = new RoomConnection(BiomeRoom.GetRoom(b.x, b.y + 1));
    if (b.E_Room == null)
      b.E_Room = new RoomConnection(BiomeRoom.GetRoom(b.x + 1, b.y));
    if (b.S_Room == null)
      b.S_Room = new RoomConnection(BiomeRoom.GetRoom(b.x, b.y - 1));
    if (b.W_Room != null)
      return;
    b.W_Room = new RoomConnection(BiomeRoom.GetRoom(b.x - 1, b.y));
  }

  private void SetNeighbours()
  {
    foreach (BiomeRoom room in this.Rooms)
    {
      if (room.N_Room == null)
        room.N_Room = new RoomConnection(BiomeRoom.GetRoom(room.x, room.y + 1));
      if (room.E_Room == null)
        room.E_Room = new RoomConnection(BiomeRoom.GetRoom(room.x + 1, room.y));
      if (room.S_Room == null)
        room.S_Room = new RoomConnection(BiomeRoom.GetRoom(room.x, room.y - 1));
      if (room.W_Room == null)
        room.W_Room = new RoomConnection(BiomeRoom.GetRoom(room.x - 1, room.y));
    }
  }

  private IEnumerator InstantiatePrefabs()
  {
    BiomeGenerator biomeGenerator = this;
    Stopwatch stopwatch = new Stopwatch();
    stopwatch.Start();
    int i = -1;
    foreach (BiomeRoom room in biomeGenerator.Rooms)
    {
      BiomeRoom r = room;
      if (r.IsCustom)
      {
        if (!string.IsNullOrEmpty(r.GameObjectPath))
        {
          bool loaded = false;
          Addressables.InstantiateAsync((object) r.GameObjectPath, biomeGenerator.transform).Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
          {
            r.GameObject = obj.Result;
            if ((UnityEngine.Object) r.GameObject != (UnityEngine.Object) null)
              r.GameObject.SetActive(false);
            loaded = true;
          });
          while (!loaded)
            yield return (object) null;
        }
        else
        {
          r.GameObject = UnityEngine.Object.Instantiate<GameObject>(r.GameObject, biomeGenerator.transform);
          r.GameObject.SetActive(false);
        }
      }
      else if (!biomeGenerator.ReuseGeneratorRoom)
      {
        r.GameObject = UnityEngine.Object.Instantiate<GameObject>(biomeGenerator.GeneratorRoomPrefab, biomeGenerator.transform);
        r.GameObject.SetActive(false);
      }
      if (r.IsRespawnRoom)
      {
        r.GameObject.transform.parent = biomeGenerator.transform.parent;
        r.GameObject.gameObject.GetComponent<RespawnRoomManager>().Init(biomeGenerator);
      }
      if (r.IsDeathCatRoom)
      {
        r.GameObject.transform.parent = biomeGenerator.transform.parent;
        r.GameObject.gameObject.GetComponent<DeathCatRoomManager>().Init(biomeGenerator);
      }
      MMTransition.UpdateProgress((float) ((double) ++i / (double) biomeGenerator.Rooms.Count), $"{ScriptLocalization.Interactions.GeneratingDungeon} {ScriptLocalization.UI.PlacingRooms} {(object) i}/{(object) biomeGenerator.Rooms.Count}");
      yield return (object) new WaitForEndOfFrame();
    }
    if (!biomeGenerator.ReuseGeneratorRoom)
      biomeGenerator.GeneratorRoomPrefab.SetActive(false);
    stopwatch.Stop();
  }

  private void OnDestroy()
  {
    this.Clear();
    this.StopMusicAndAtmos();
    if (this.loadedAddressableAssets != null)
    {
      foreach (AsyncOperationHandle<GameObject> addressableAsset in this.loadedAddressableAssets)
        Addressables.Release((AsyncOperationHandle) addressableAsset);
      this.loadedAddressableAssets.Clear();
    }
    if (!((UnityEngine.Object) BiomeGenerator.Instance == (UnityEngine.Object) this))
      return;
    BiomeGenerator.Instance = (BiomeGenerator) null;
  }

  private void Clear()
  {
    foreach (BiomeRoom room in this.Rooms)
      room.Clear();
    this.Rooms.Clear();
  }

  private void StopMusicAndAtmos()
  {
    AudioManager.Instance.StopCurrentMusic();
    AudioManager.Instance.StopLoop(this.biomeAtmosInstance);
  }

  public void Regenerate(System.Action Callback)
  {
    MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 0.5f, "", (System.Action) (() =>
    {
      this.Clear();
      ObjectPool.DestroyAll();
      this.PrevRoom = (BiomeRoom) null;
      this.FirstArrival = true;
      for (int index = this.transform.childCount - 1; index > 0; --index)
      {
        if ((UnityEngine.Object) this.transform.GetChild(index).gameObject != (UnityEngine.Object) this.GeneratorRoomPrefab)
          UnityEngine.Object.Destroy((UnityEngine.Object) this.transform.GetChild(index).gameObject);
      }
      this.Seed = DataManager.RandomSeed.Next(-2147483647 /*0x80000001*/, int.MaxValue);
      this.StartCoroutine((IEnumerator) this.GenerateRoutine());
      System.Action action = Callback;
      if (action == null)
        return;
      action();
    }));
  }

  private void SetRoom(int x, int y)
  {
    this.CurrentX = x;
    this.CurrentY = y;
    this.StartCoroutine((IEnumerator) this.ChangeRoomRoutine(BiomeRoom.GetRoom(this.CurrentX, this.CurrentY)));
  }

  public static void ChangeRoom(int X, int Y)
  {
    BiomeGenerator.Instance.CurrentX = X;
    BiomeGenerator.Instance.CurrentY = Y;
    BiomeGenerator.Instance.StartCoroutine((IEnumerator) BiomeGenerator.Instance.ChangeRoomRoutine(BiomeRoom.GetRoom(BiomeGenerator.Instance.CurrentX, BiomeGenerator.Instance.CurrentY)));
  }

  public static void ChangeRoom(Vector2Int Direction)
  {
    BiomeGenerator.Instance.CurrentX += Direction.x;
    BiomeGenerator.Instance.CurrentY += Direction.y;
    BiomeGenerator.Instance.StartCoroutine((IEnumerator) BiomeGenerator.Instance.ChangeRoomRoutine(BiomeRoom.GetRoom(BiomeGenerator.Instance.CurrentX, BiomeGenerator.Instance.CurrentY)));
  }

  private IEnumerator ChangeRoomRoutine(BiomeRoom CurrentRoom)
  {
    BiomeGenerator.BiomeAction onBiomeLeftRoom = BiomeGenerator.OnBiomeLeftRoom;
    if (onBiomeLeftRoom != null)
      onBiomeLeftRoom();
    this.CurrentRoom = CurrentRoom;
    if (!CurrentRoom.Visited)
      this.RoomsVisited++;
    CurrentRoom.Activate(this.PrevRoom, this.ReuseGeneratorRoom);
    yield return (object) new WaitForEndOfFrame();
    this.GetDoors();
    while (!CurrentRoom.generateRoom.GeneratedDecorations)
      yield return (object) null;
    CurrentRoom.generateRoom.SetColliderAndUpdatePathfinding();
    this.PlacePlayer();
    BiomeGenerator.BiomeAction onBiomeChangeRoom = BiomeGenerator.OnBiomeChangeRoom;
    if (onBiomeChangeRoom != null)
      onBiomeChangeRoom();
    if (this.PrevRoom == null)
    {
      AudioManager.Instance.SetMusicRoomID(CurrentRoom.generateRoom.roomMusicID);
      AudioManager.Instance.StartMusic();
    }
    else
    {
      if (this.PrevRoom.generateRoom.roomMusicID == SoundConstants.RoomID.NoMusic && CurrentRoom.generateRoom.roomMusicID != SoundConstants.RoomID.NoMusic)
        AudioManager.Instance.PlayMusic(this.biomeMusicPath);
      AudioManager.Instance.SetMusicRoomID(CurrentRoom.generateRoom.roomMusicID);
    }
    string soundPath = CurrentRoom.generateRoom.biomeAtmosOverridePath != string.Empty ? CurrentRoom.generateRoom.biomeAtmosOverridePath : this.biomeAtmosPath;
    if (!AudioManager.Instance.CurrentEventIsPlayingPath(this.biomeAtmosInstance, soundPath))
      AudioManager.Instance.PlayAtmos(soundPath);
    bool flag = false;
    foreach (Health allUnit in Health.allUnits)
    {
      if (allUnit.team == Health.Team.Team2 && (double) allUnit.HP > 0.0)
      {
        flag = true;
        break;
      }
    }
    if (!flag)
      AudioManager.Instance.SetMusicCombatState(false);
    this.PrevRoom = CurrentRoom;
  }

  public static void SpawnBombsInRoom(int amount)
  {
    LayerMask layerMask = (LayerMask) ((int) (LayerMask) ((int) new LayerMask() | 1 << LayerMask.NameToLayer("Island")) | 1 << LayerMask.NameToLayer("Obstacles Player Ignore"));
    float x = Physics2D.Raycast((Vector2) Vector3.zero, (Vector2) Vector3.right, 100f, (int) layerMask).point.x;
    float y = Physics2D.Raycast((Vector2) Vector3.zero, (Vector2) Vector3.up, 100f, (int) layerMask).point.y;
    float delay = 0.0f;
    float num = 0.25f;
    for (int index = 0; index < amount; ++index)
    {
      Vector3 position = new Vector3(UnityEngine.Random.Range(-x, x), UnityEngine.Random.Range(-y, y));
      GameManager.GetInstance().StartCoroutine((IEnumerator) SpawnBomb(position, BiomeGenerator.Instance.CurrentRoom.generateRoom.transform, delay));
      delay += num;
    }

    static IEnumerator SpawnBomb(Vector3 position, Transform parent, float delay)
    {
      yield return (object) new WaitForSeconds(delay);
      Bomb.CreateBomb(position, (Health) null, parent);
    }
  }

  private IEnumerator DelayEndConversation()
  {
    yield return (object) new WaitForSeconds(0.3f);
    GameManager.GetInstance().OnConversationEnd(false);
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    GameManager.GetInstance().AddPlayerToCamera();
  }

  private IEnumerator DelayPlayerGoToAndStop(Vector3 TargetPosition)
  {
    BiomeGenerator biomeGenerator = this;
    PlayerFarming.Instance.GoToAndStop(TargetPosition, IdleOnEnd: true, DisableCollider: true, GoToCallback: (System.Action) (() =>
    {
      this.PlayerState.facingAngle = Utils.GetAngle(Door.GetEntranceDoor().transform.position, TargetPosition);
      this.StartCoroutine((IEnumerator) this.DelayEndConversation());
      if (this.CurrentRoom == this.RoomEntrance)
        GenerateRoom.Instance.LockEntranceBehindPlayer = true;
      Door entranceDoor = Door.GetEntranceDoor();
      if (GenerateRoom.Instance.LockEntranceBehindPlayer)
      {
        entranceDoor.RoomLockController.gameObject.SetActive(true);
        entranceDoor.RoomLockController.DoorUp();
        foreach (PlayerDistanceMovement componentsInChild in entranceDoor.GetComponentsInChildren<PlayerDistanceMovement>())
        {
          componentsInChild.ForceReset();
          componentsInChild.enabled = false;
        }
        entranceDoor.VisitedIcon.SetActive(false);
      }
      entranceDoor.PlayerFinishedEnteringDoor();
      this.StartCoroutine((IEnumerator) this.DelayActivateRoom(this.DungeonLocation != FollowerLocation.Boss_5));
    }), maxDuration: -1f);
    yield return (object) new WaitForSeconds(0.5f);
    if (biomeGenerator.ShowDisplayName)
    {
      if (DataManager.Instance.dungeonRun == 1 && PlayerFarming.Location == FollowerLocation.Dungeon1_1)
      {
        yield return (object) new WaitForSeconds(1f);
        UIHeartsIntro uiHeartsIntro = UnityEngine.Object.Instantiate<UIHeartsIntro>(UnityEngine.Resources.Load<UIHeartsIntro>("Prefabs/UI/UI Hearts Intro"), GameObject.FindGameObjectWithTag("Canvas").transform);
        yield return (object) biomeGenerator.StartCoroutine((IEnumerator) uiHeartsIntro.HeartRoutine());
        DataManager.Instance.PlayerHasBeenGivenHearts = true;
      }
      HUD_DisplayName.Play(biomeGenerator.DisplayName, 3, HUD_DisplayName.Positions.Centre);
      yield return (object) new WaitForSeconds(1f);
    }
    biomeGenerator.ShowDisplayName = false;
    DataManager.Instance.FirstTimeInDungeon = true;
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.GoToDungeon);
  }

  public void RoomBecameActive()
  {
    BiomeGenerator.BiomeAction onRoomActive = BiomeGenerator.OnRoomActive;
    if (onRoomActive == null)
      return;
    onRoomActive();
  }

  public IEnumerator DelayActivateRoom(bool applyModifiers)
  {
    BiomeGenerator biomeGenerator = this;
    if (applyModifiers)
    {
      yield return (object) new WaitForEndOfFrame();
      while (PlayerFarming.Instance.GoToAndStopping)
        yield return (object) null;
      if (GameManager.InitialDungeonEnter)
      {
        if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ability_BlackHeart))
          PlayerFarming.Instance.health.BlackHearts += 2f;
        yield return (object) biomeGenerator.StartCoroutine((IEnumerator) biomeGenerator.ApplyCurrentFleeceModifiersIE());
        yield return (object) biomeGenerator.StartCoroutine((IEnumerator) biomeGenerator.ApplyCurrentDemonModifiersIE());
      }
      yield return (object) biomeGenerator.StartCoroutine((IEnumerator) biomeGenerator.ApplyCurrentDungeonModifiersIE());
    }
    yield return (object) new WaitForSeconds(1f);
    GameManager.InitialDungeonEnter = false;
    biomeGenerator.CurrentRoom.Active = true;
    BiomeGenerator.BiomeAction onRoomActive = BiomeGenerator.OnRoomActive;
    if (onRoomActive != null)
      onRoomActive();
    biomeGenerator.CurrentRoom.Completed = true;
  }

  public GameObject PlacePlayer()
  {
    if ((UnityEngine.Object) this.Player == (UnityEngine.Object) null)
    {
      if (GameManager.SandboxDungeonEnabled)
      {
        foreach (KeyValuePair<FollowerLocation, LocationManager> locationManager in LocationManager.LocationManagers)
        {
          if ((UnityEngine.Object) locationManager.Value != (UnityEngine.Object) null)
          {
            this.Player = locationManager.Value.PlacePlayer();
            break;
          }
        }
      }
      else
        this.Player = LocationManager.LocationManagers[this.DungeonLocation].PlacePlayer();
      this.PlayerState = this.Player.GetComponent<StateMachine>();
    }
    else
    {
      System.Action playerLocationSet = LocationManager.OnPlayerLocationSet;
      if (playerLocationSet != null)
        playerLocationSet();
    }
    if (this.FirstArrival && this.DoFirstArrivalRoutine)
    {
      GameManager.GetInstance().OnConversationNew(SnapLetterBox: true);
      GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 6f);
      GameManager.GetInstance().CameraSetZoom(6f);
      GameManager.GetInstance().CameraSetOffset(new Vector3(0.0f, 0.0f, -1f));
      Door entranceDoor = Door.GetEntranceDoor();
      this.Player.transform.position = entranceDoor.PlayerPosition.position;
      this.StartCoroutine((IEnumerator) this.DelayPlayerGoToAndStop(this.Player.transform.position + entranceDoor.GetDoorDirection() * 7f));
      this.FirstArrival = false;
      if (!this.spawnedDemons && this.spawnDemons)
      {
        this.SpawnDemons();
        this.spawnedDemons = true;
      }
    }
    else
    {
      if (this.IsTeleporting)
        this.Player.transform.position = (UnityEngine.Object) Interaction_Chest.Instance == (UnityEngine.Object) null ? Vector3.zero : Interaction_Chest.Instance.transform.position;
      else if (this.PrevCurrentX < this.CurrentX && (bool) (UnityEngine.Object) this.West)
      {
        this.Player.transform.position = this.West.PlayerPosition.position;
        this.PlayerState.facingAngle = 0.0f;
      }
      else if (this.PrevCurrentX > this.CurrentX && (bool) (UnityEngine.Object) this.East)
      {
        this.Player.transform.position = this.East.PlayerPosition.position;
        this.PlayerState.facingAngle = 180f;
      }
      else if (this.PrevCurrentY > this.CurrentY && (bool) (UnityEngine.Object) this.North)
      {
        this.Player.transform.position = this.North.PlayerPosition.position;
        this.PlayerState.facingAngle = 270f;
      }
      else if (this.PrevCurrentY < this.CurrentY && (bool) (UnityEngine.Object) this.South)
      {
        this.Player.transform.position = this.South.PlayerPosition.position;
        this.PlayerState.facingAngle = 90f;
      }
      else if ((UnityEngine.Object) this.South != (UnityEngine.Object) null)
      {
        UnityEngine.Debug.Log((object) "NO WHERE TO PLACE YOU - so put you south!");
        this.Player.transform.position = this.South.PlayerPosition.position;
        this.PlayerState.facingAngle = 90f;
      }
      else
        UnityEngine.Debug.Log((object) "NO WHERE TO PLACE YOU!");
      if (!this.CurrentRoom.Completed)
      {
        bool flag = false;
        foreach (UnitObject componentsInChild in this.CurrentRoom.generateRoom.GetComponentsInChildren<UnitObject>())
        {
          if (componentsInChild.health.team == Health.Team.Team2)
          {
            flag = true;
            break;
          }
        }
        if (TrinketManager.HasTrinket(TarotCards.Card.Arrows) & flag)
          PlayerFarming.Instance.GetBlackSoul(Mathf.RoundToInt(FaithAmmo.Total - FaithAmmo.Ammo), false);
        if (flag && Door.Doors.Count > 0)
        {
          Vector3 vector2 = (Vector3) Utils.DegreeToVector2(this.PlayerState.facingAngle);
          PlayerFarming.Instance?.GoToAndStop(this.Player.transform.position + vector2 * 3f, IdleOnEnd: true, GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.DelayActivateRoom())));
        }
        else
        {
          Vector3 vector2 = (Vector3) Utils.DegreeToVector2(this.PlayerState.facingAngle);
          PlayerFarming.Instance?.GoToAndStop(this.Player.transform.position + vector2, IdleOnEnd: true, GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.DelayActivateRoom())));
        }
      }
      else
      {
        Vector3 vector2 = (Vector3) Utils.DegreeToVector2(this.PlayerState.facingAngle);
        PlayerFarming.Instance?.GoToAndStop(this.Player.transform.position + vector2, IdleOnEnd: true, GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.DelayActivateRoom())));
      }
    }
    this.IsTeleporting = false;
    this.PrevCurrentX = this.CurrentX;
    this.PrevCurrentY = this.CurrentY;
    GameManager.GetInstance().CameraSnapToPosition(this.Player.transform.position);
    GameManager.GetInstance().AddPlayerToCamera();
    return this.Player;
  }

  private IEnumerator DelayActivateRoom()
  {
    yield return (object) new WaitForSeconds(0.5f);
    BiomeGenerator.Instance.CurrentRoom.Active = true;
  }

  public void SpawnDemons()
  {
    int index = -1;
    while (++index < DataManager.Instance.Followers_Demons_IDs.Count)
      this.SpawnDemon(DataManager.Instance.Followers_Demons_Types[index]);
  }

  public void SpawnDemon(int type)
  {
    Addressables.InstantiateAsync((object) new List<string>()
    {
      "Assets/Prefabs/Enemies/Demons/Demon_Shooty.prefab",
      "Assets/Prefabs/Enemies/Demons/Demon_Chomp.prefab",
      "Assets/Prefabs/Enemies/Demons/Demon_Arrows.prefab",
      "Assets/Prefabs/Enemies/Demons/Demon_Collector.prefab",
      "Assets/Prefabs/Enemies/Demons/Demon_Exploder.prefab",
      "Assets/Prefabs/Enemies/Demons/Demon_Spirit.prefab"
    }[type], this.Player.transform.position, Quaternion.identity, this.Player.transform.parent).Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      obj.Result.transform.position = this.Player.transform.position;
      obj.Result.GetComponent<Demon>().Init(type);
    });
  }

  public void ApplyCurrentDungeonModifiers()
  {
    this.StartCoroutine((IEnumerator) this.ApplyCurrentDungeonModifiersIE());
  }

  private IEnumerator ApplyCurrentDungeonModifiersIE()
  {
    BiomeGenerator biomeGenerator = this;
    HealthPlayer health = PlayerFarming.Instance.health as HealthPlayer;
    if (DungeonModifier.HasNeutralModifier(DungeonNeutralModifier.LoseRedGainBlackHeart))
    {
      DataManager.Instance.RedHeartsTemporarilyRemoved += 2;
      health.totalHP -= 2f;
      health.BlackHearts += 4f;
    }
    else if (DungeonModifier.HasNeutralModifier(DungeonNeutralModifier.LoseRedGainTarot))
    {
      DataManager.Instance.RedHeartsTemporarilyRemoved += 2;
      health.totalHP -= 2f;
      yield return (object) biomeGenerator.StartCoroutine((IEnumerator) biomeGenerator.DoTarotRoutine(Vector3.zero, Vector3.one));
    }
  }

  private IEnumerator ApplyCurrentFleeceModifiersIE()
  {
    if (DataManager.Instance.PlayerFleece == 5)
    {
      DataManager.Instance.RedHeartsTemporarilyRemoved += (int) DataManager.Instance.PLAYER_HEALTH;
      PlayerFarming.Instance.health.totalHP -= DataManager.Instance.PLAYER_HEALTH;
      PlayerFarming.Instance.health.BlueHearts += DataManager.Instance.PLAYER_HEALTH * 1.5f;
    }
    else if (DataManager.Instance.PlayerFleece == 2)
    {
      DataManager.Instance.RedHeartsTemporarilyRemoved += (DataManager.Instance.PLAYER_STARTING_HEALTH + DataManager.Instance.PLAYER_HEARTS_LEVEL) / 2;
      PlayerFarming.Instance.health.totalHP -= (float) ((DataManager.Instance.PLAYER_STARTING_HEALTH + DataManager.Instance.PLAYER_HEARTS_LEVEL) / 2);
    }
    if (PlayerFleeceManager.GetFreeTarotCards() > 0)
      yield return (object) this.DoFleeceTarotRoutine();
  }

  private IEnumerator ApplyCurrentDemonModifiersIE()
  {
    foreach (GameObject demon in Demon_Arrows.Demons)
    {
      if ((bool) (UnityEngine.Object) demon.GetComponent<Demon_Spirit>())
      {
        Demon_Spirit component = demon.GetComponent<Demon_Spirit>();
        (PlayerFarming.Instance.health as HealthPlayer).TotalSpiritHearts += Mathf.Ceil((float) component.Level / 2f);
      }
    }
    yield return (object) new WaitForSeconds(0.5f);
  }

  private IEnumerator DoFleeceTarotRoutine()
  {
    BiomeGenerator biomeGenerator = this;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.Instance.state.facingAngle = -90f;
    GameManager.GetInstance().CamFollowTarget.DisablePlayerLook = true;
    GameManager.GetInstance().CameraSetTargetZoom(4f);
    yield return (object) new WaitForSecondsRealtime(0.35f);
    HUD_Manager.Instance.Hide(false, 0);
    LetterBox.Show(false);
    AudioManager.Instance.PlayOneShot("event:/tarot/tarot_card_pull", biomeGenerator.gameObject);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("cards/cards-start", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("cards/cards-loop", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().CameraSetTargetZoom(6f);
    TarotCards.TarotCard drawnCard1 = TarotCards.DrawRandomCard();
    DataManager.Instance.PlayerRunTrinkets.Add(drawnCard1);
    TarotCards.TarotCard drawnCard2 = TarotCards.DrawRandomCard();
    DataManager.Instance.PlayerRunTrinkets.Add(drawnCard2);
    TarotCards.TarotCard drawnCard3 = TarotCards.DrawRandomCard();
    DataManager.Instance.PlayerRunTrinkets.Add(drawnCard3);
    TarotCards.TarotCard drawnCard4 = TarotCards.DrawRandomCard();
    DataManager.Instance.PlayerRunTrinkets.Add(drawnCard4);
    UIFleeceTarotRewardOverlayController tarotRewardOverlay = MonoSingleton<UIManager>.Instance.ShowFleeceTarotReward(drawnCard1, drawnCard2, drawnCard3, drawnCard4);
    UIFleeceTarotRewardOverlayController overlayController = tarotRewardOverlay;
    overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() => tarotRewardOverlay = (UIFleeceTarotRewardOverlayController) null);
    while ((UnityEngine.Object) tarotRewardOverlay != (UnityEngine.Object) null)
      yield return (object) null;
    LetterBox.Hide();
    HUD_Manager.Instance.Show(0);
    AudioManager.Instance.PlayOneShot("event:/tarot/tarot_card_close", biomeGenerator.gameObject);
    GameManager.GetInstance().CameraResetTargetZoom();
    GameManager.GetInstance().CamFollowTarget.DisablePlayerLook = false;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Idle;
    yield return (object) null;
    PlayerFarming.Instance.simpleSpineAnimator.Animate("cards/cards-stop-seperate", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(0.2f);
    TrinketManager.AddTrinket(drawnCard1);
    TrinketManager.AddTrinket(drawnCard2);
    TrinketManager.AddTrinket(drawnCard3);
    TrinketManager.AddTrinket(drawnCard4);
  }

  private IEnumerator DoTarotRoutine(Vector3 position, Vector3 scale)
  {
    BiomeGenerator biomeGenerator = this;
    HUD_Manager.Instance.Hide(false, 0);
    GameManager.GetInstance().CameraSetTargetZoom(4f);
    LetterBox.Show(false);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.Instance.state.facingAngle = -90f;
    GameManager.GetInstance().CamFollowTarget.DisablePlayerLook = true;
    AudioManager.Instance.PlayOneShot("event:/tarot/tarot_card_pull", biomeGenerator.gameObject);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("cards/cards-start", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("cards/cards-loop", 0, true, 0.0f);
    TarotCards.TarotCard drawnCard = TarotCards.DrawRandomCard();
    DataManager.Instance.PlayerRunTrinkets.Add(drawnCard);
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().CameraSetTargetZoom(6f);
    bool waiting = true;
    GameObject gameObject = UITrinketCards.Play(drawnCard, (System.Action) (() => waiting = false));
    if ((bool) (UnityEngine.Object) gameObject.GetComponent<MMButton>())
      gameObject.GetComponent<MMButton>().enabled = false;
    gameObject.transform.localScale = scale;
    ((RectTransform) gameObject.transform).anchoredPosition = (Vector2) position;
    while (waiting)
      yield return (object) null;
    biomeGenerator.StartCoroutine((IEnumerator) biomeGenerator.BackToIdleRoutine(drawnCard));
  }

  private IEnumerator BackToIdleRoutine(TarotCards.TarotCard card)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    BiomeGenerator biomeGenerator = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      PlayerFarming.Instance.simpleSpineAnimator.Animate("cards/cards-stop-seperate", 0, false);
      PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
      GameManager.GetInstance().StartCoroutine((IEnumerator) biomeGenerator.DelayEffectsRoutine(card));
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    LetterBox.Hide();
    HUD_Manager.Instance.Show(0);
    AudioManager.Instance.PlayOneShot("event:/tarot/tarot_card_close", biomeGenerator.gameObject);
    GameManager.GetInstance().CameraResetTargetZoom();
    GameManager.GetInstance().CamFollowTarget.DisablePlayerLook = false;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Idle;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) null;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private IEnumerator DelayEffectsRoutine(TarotCards.TarotCard card)
  {
    yield return (object) new WaitForSeconds(0.2f);
    TrinketManager.AddTrinket(card);
  }

  private void GetDoors()
  {
    this.North = GameObject.FindGameObjectWithTag("North Door")?.GetComponent<Door>();
    this.East = GameObject.FindGameObjectWithTag("East Door")?.GetComponent<Door>();
    this.South = GameObject.FindGameObjectWithTag("South Door")?.GetComponent<Door>();
    this.West = GameObject.FindGameObjectWithTag("West Door")?.GetComponent<Door>();
    if (this.CurrentRoom.N_Room != null)
      this.North?.Init(this.CurrentRoom.N_Room.ConnectionType);
    if (this.CurrentRoom.E_Room != null)
      this.East?.Init(this.CurrentRoom.E_Room.ConnectionType);
    if (this.CurrentRoom.S_Room != null)
      this.South?.Init(this.CurrentRoom.S_Room.ConnectionType);
    if (this.CurrentRoom.W_Room == null)
      return;
    this.West?.Init(this.CurrentRoom.W_Room.ConnectionType);
  }

  public void Left() => BiomeGenerator.ChangeRoom(new Vector2Int(-1, 0));

  public void Right() => BiomeGenerator.ChangeRoom(new Vector2Int(1, 0));

  public void Up() => BiomeGenerator.ChangeRoom(new Vector2Int(0, 1));

  public void Down() => BiomeGenerator.ChangeRoom(new Vector2Int(0, -1));

  public delegate void GetKey();

  public delegate void BiomeAction();

  [Serializable]
  public class OverrideRoom
  {
    public int x;
    public int y;
    public GenerateRoom.ConnectionTypes North;
    public GenerateRoom.ConnectionTypes East;
    public GenerateRoom.ConnectionTypes South;
    public GenerateRoom.ConnectionTypes West;
    public BiomeGenerator.FixedRoom.Generate Generated = BiomeGenerator.FixedRoom.Generate.DontGenerate;
    public string PrefabPath;
    public bool RoomActive = true;
  }

  private class RoomEntranceExit
  {
    public BiomeRoom Room;

    public RoomEntranceExit(BiomeRoom Room, bool Create) => this.Room = Room;
  }

  [Serializable]
  public class ListOfStoryRooms
  {
    public List<BiomeGenerator.RoomAndStoryPosition> Rooms = new List<BiomeGenerator.RoomAndStoryPosition>();
    public DataManager.Variables StoryVariable;
    public DataManager.Variables LastRun;
    public DataManager.Variables DungeonBeaten;
    public bool PutOnCriticalPath = true;
  }

  [Serializable]
  public class RoomAndStoryPosition
  {
    public int StoryCountRequirement;
    public string RoomPath;
    public bool FloorOne = true;
    public bool FloorTwo = true;
    public bool FloorThree = true;
  }

  [Serializable]
  public class ListOfCustomRoomPrefabs
  {
    public List<string> RoomPaths;
    public bool UseStaticRoomList;
    public static List<string> StaticRoomList = new List<string>()
    {
      "Assets/_Rooms/Reward Room Gold Rare.prefab",
      "Assets/_Rooms/Special Blood Sacrafice.prefab",
      "Assets/_Rooms/Special Coin Gamble.prefab",
      "Assets/_Rooms/Special Free Tarot Cards.prefab",
      "Assets/_Rooms/Special Free Health.prefab",
      "Assets/_Rooms/Special Secret Room 1.prefab"
    };
    public bool PutOnCriticalPath;
    public bool RemoveIfNoNonCriticalPath;
    [Range(0.0f, 1f)]
    public float Probability = 1f;
    public bool Redecorate = true;
    public List<BiomeGenerator.VariableAndCondition> ConditionalVariables = new List<BiomeGenerator.VariableAndCondition>();
    public bool MinimumRun;
    public int MinimumRunNumber;
    public bool MaximumRun;
    public int MaximumRunNumber;
    public bool MinimumFollowerCount;
    public int MinimumFollowerCountNumber;
    public bool MaximumFollowerCount;
    public int MaximumFollowerCountNumber;
    public bool SetCustomConnectionType;
    public GenerateRoom.ConnectionTypes ConnectionType;
    public List<FollowerLocation> LocationIsUndiscovered = new List<FollowerLocation>();
    public bool LayerOne;
    public bool LayerTwo;
    public bool LayerThree;
    public bool LayerFour;
    public bool LayerFive;
    public bool LayerSix;
    public bool FloorOne = true;
    public bool FloorTwo = true;
    public bool FloorThree = true;

    internal bool AvailableOnFoor()
    {
      switch (GameManager.CurrentDungeonFloor)
      {
        case 1:
          return this.FloorOne;
        case 2:
          return this.FloorTwo;
        case 3:
          return this.FloorThree;
        default:
          return false;
      }
    }

    internal bool AvailableOnLayer()
    {
      switch (GameManager.CurrentDungeonLayer)
      {
        case 1:
          return this.LayerOne;
        case 2:
          return this.LayerTwo;
        case 3:
          return this.LayerThree;
        case 4:
          return this.LayerFour;
        case 5:
          return this.LayerFive;
        case 6:
          return this.LayerSix;
        default:
          return false;
      }
    }
  }

  [Serializable]
  public class VariableAndCondition
  {
    public DataManager.Variables Variable;
    public bool Condition = true;
  }

  [Serializable]
  public class VariableAndCount
  {
    public DataManager.Variables Variable;
    public int Count;
  }

  [Serializable]
  public class FixedRoom
  {
    public GameObject prefab;
    public BiomeGenerator.FixedRoom.Generate GenerateRoom;
    public BiomeGenerator.FixedRoom.Connection North;
    public BiomeGenerator.FixedRoom.Connection East;
    public BiomeGenerator.FixedRoom.Connection South;
    public BiomeGenerator.FixedRoom.Connection West;
    [Range(0.0f, 1f)]
    public float Probability = 1f;
    public List<BiomeGenerator.VariableAndCondition> ConditionalVariables = new List<BiomeGenerator.VariableAndCondition>();

    public bool HasOptional
    {
      get
      {
        return this.North == BiomeGenerator.FixedRoom.Connection.Optional || this.East == BiomeGenerator.FixedRoom.Connection.Optional || this.South == BiomeGenerator.FixedRoom.Connection.Optional || this.West == BiomeGenerator.FixedRoom.Connection.Optional;
      }
    }

    public bool HasForcedOn
    {
      get
      {
        return this.North == BiomeGenerator.FixedRoom.Connection.ForceOn || this.East == BiomeGenerator.FixedRoom.Connection.ForceOn || this.South == BiomeGenerator.FixedRoom.Connection.ForceOn || this.West == BiomeGenerator.FixedRoom.Connection.ForceOn;
      }
    }

    public int RequiredConnections
    {
      get
      {
        int requiredConnections = 0;
        if (this.North != BiomeGenerator.FixedRoom.Connection.ForceOff)
          ++requiredConnections;
        if (this.East != BiomeGenerator.FixedRoom.Connection.ForceOff)
          ++requiredConnections;
        if (this.South != BiomeGenerator.FixedRoom.Connection.ForceOff)
          ++requiredConnections;
        if (this.West != BiomeGenerator.FixedRoom.Connection.ForceOff)
          ++requiredConnections;
        return requiredConnections;
      }
    }

    public enum Generate
    {
      GenerateOnLoad,
      DontGenerate,
      GenerateCustomOnLoad,
    }

    public enum Connection
    {
      ForceOn,
      Optional,
      ForceOff,
    }
  }

  private enum Direction
  {
    North,
    East,
    South,
    West,
  }
}
