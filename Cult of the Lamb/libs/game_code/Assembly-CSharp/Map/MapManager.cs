// Decompiled with JetBrains decompiler
// Type: Map.MapManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using Lamb.UI;
using MMBiomeGeneration;
using MMRoomGeneration;
using src.UINavigator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace Map;

public class MapManager : MonoBehaviour
{
  public System.Action OnMapShown;
  public MapConfig DungeonConfig;
  [CompilerGenerated]
  public Map.Map \u003CCurrentMap\u003Ek__BackingField;
  public static MapManager Instance;
  [CompilerGenerated]
  public bool \u003CMapGenerated\u003Ek__BackingField;
  public bool _canShuffle = true;
  public UIAdventureMapOverlayController _adventureMapInstance;
  public EventInstance _loopedSound;
  public float clipPlane;
  public Camera mainCamera;

  public Map.Map CurrentMap
  {
    get => this.\u003CCurrentMap\u003Ek__BackingField;
    set => this.\u003CCurrentMap\u003Ek__BackingField = value;
  }

  public Node CurrentNode
  {
    get => this.CurrentMap != null ? this.CurrentMap.GetCurrentNode() : (Node) null;
  }

  public int CurrentLayer
  {
    get
    {
      if (this.CurrentMap == null)
        this.CurrentMap = MapGenerator.GetMap(this.DungeonConfig);
      Node currentNode = this.CurrentMap.GetCurrentNode();
      for (int index = 0; index < MapGenerator.Nodes.Count; ++index)
      {
        if (MapGenerator.Nodes[index].Contains(currentNode))
          return index;
      }
      return 0;
    }
  }

  public bool MapGenerated
  {
    get => this.\u003CMapGenerated\u003Ek__BackingField;
    set => this.\u003CMapGenerated\u003Ek__BackingField = value;
  }

  public bool CanShuffle
  {
    get
    {
      return this._canShuffle && TrinketManager.HasTrinket(TarotCards.Card.ShuffleNode, MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer);
    }
    set => this._canShuffle = value;
  }

  public void Awake()
  {
    this.mainCamera = Camera.main;
    MapManager.Instance = this;
  }

  public void OnDestroy()
  {
    AudioManager.Instance.StopLoop(this._loopedSound);
    MapManager.Instance = (MapManager) null;
  }

  public UIAdventureMapOverlayController ShowMap(bool disableInput = false)
  {
    SimulationManager.Pause();
    if (!this.MapGenerated)
      this.GenerateNewMap();
    GameObject go = (UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null ? PlayerFarming.Instance.gameObject : this.gameObject;
    AudioManager.Instance.StopLoop(this._loopedSound);
    this._loopedSound = AudioManager.Instance.CreateLoop("event:/atmos/forest/temple_door_hum", go, true);
    this._adventureMapInstance = MonoSingleton<UIManager>.Instance.ShowAdventureMap(this.CurrentMap, disableInput);
    UIAdventureMapOverlayController adventureMapInstance1 = this._adventureMapInstance;
    adventureMapInstance1.OnShow = adventureMapInstance1.OnShow + (System.Action) (() =>
    {
      if ((double) this.mainCamera.farClipPlane != 0.019999999552965164)
        this.clipPlane = this.mainCamera.farClipPlane;
      this.mainCamera.farClipPlane = 0.02f;
      System.Action onMapShown = this.OnMapShown;
      if (onMapShown == null)
        return;
      onMapShown();
    });
    UIAdventureMapOverlayController adventureMapInstance2 = this._adventureMapInstance;
    adventureMapInstance2.OnHide = adventureMapInstance2.OnHide + (System.Action) (() =>
    {
      if ((double) this.clipPlane == 0.0)
        return;
      this.mainCamera.farClipPlane = this.clipPlane;
    });
    UIAdventureMapOverlayController adventureMapInstance3 = this._adventureMapInstance;
    adventureMapInstance3.OnDisabled = adventureMapInstance3.OnDisabled + (System.Action) (() => AudioManager.Instance.StopLoop(this._loopedSound));
    UIAdventureMapOverlayController adventureMapInstance4 = this._adventureMapInstance;
    adventureMapInstance4.OnDestroyed = adventureMapInstance4.OnDestroyed + (System.Action) (() => AudioManager.Instance.StopLoop(this._loopedSound));
    return this._adventureMapInstance;
  }

  public void CloseMap()
  {
    SimulationManager.UnPause();
    AudioManager.Instance.StopLoop(this._loopedSound);
    this._adventureMapInstance.Hide();
  }

  public Map.Map GenerateNewMap()
  {
    BiomeGenerator.Instance.RandomiseSeed();
    this.CurrentMap = MapGenerator.GetMap(this.DungeonConfig);
    this.MapGenerated = true;
    return this.CurrentMap;
  }

  public void AddNodeToPath(Node mapNode)
  {
    this.CurrentMap.path.Add(mapNode.point);
    DungeonModifier.SetActiveModifier(mapNode.Modifier);
    if (!mapNode.nodeType.ShouldIncrementRandomRoomsEncountered())
      return;
    ++DataManager.Instance.MinimumRandomRoomsEncounteredAmount;
  }

  public void EnterNode(Node mapNode)
  {
    if (DungeonSandboxManager.Active)
    {
      ++GameManager.DungeonEndlessLevel;
      if (mapNode.DungeonLocation != FollowerLocation.None)
        DungeonSandboxManager.Instance.SetDungeonType(mapNode.DungeonLocation);
      else if (mapNode.blueprint.ForcedDungeon == FollowerLocation.None)
        DungeonSandboxManager.Instance.UpdateDungeonType();
      else
        DungeonSandboxManager.Instance.SetDungeonType(mapNode.blueprint.ForcedDungeon);
    }
    Debug.Log((object) ("EnterNode " + mapNode.nodeType.ToString()));
    switch (mapNode.nodeType)
    {
      case NodeType.Boss:
      case NodeType.DungeonFloor:
      case NodeType.MiniBossFloor:
      case NodeType.FinalBoss:
        Debug.Log((object) ("AAA " + mapNode.nodeType.ToString()));
        if (!GameManager.InitialDungeonEnter)
          ++GameManager.CurrentDungeonFloor;
        BiomeGenerator.Instance.OverrideRandomWalk = false;
        if (DungeonSandboxManager.Active && (mapNode.nodeType == NodeType.FinalBoss && mapNode.blueprint.nodeType == NodeType.MiniBossFloor || mapNode.nodeType == NodeType.MiniBossFloor || mapNode.nodeType == NodeType.Boss || mapNode.blueprint.RoomPrefabs.Length != 0))
        {
          BiomeGenerator.Instance.OverrideRooms = new List<BiomeGenerator.OverrideRoom>()
          {
            new BiomeGenerator.OverrideRoom()
            {
              North = GenerateRoom.ConnectionTypes.DoorRoom,
              South = GenerateRoom.ConnectionTypes.Entrance,
              Generated = BiomeGenerator.FixedRoom.Generate.DontGenerate,
              PrefabPath = BiomeGenerator.Instance.LeaderRoomPath
            }
          };
          if ((mapNode.nodeType == NodeType.MiniBossFloor || mapNode.blueprint.nodeType == NodeType.FinalBoss || mapNode.nodeType != NodeType.Boss && mapNode.blueprint.nodeType == NodeType.MiniBossFloor) && mapNode.blueprint.RoomPrefabs.Length != 0)
          {
            BiomeGenerator.Instance.OverrideRooms = new List<BiomeGenerator.OverrideRoom>()
            {
              new BiomeGenerator.OverrideRoom()
              {
                North = GenerateRoom.ConnectionTypes.DoorRoom,
                South = GenerateRoom.ConnectionTypes.Entrance,
                Generated = mapNode.blueprint.nodeType != NodeType.FinalBoss ? BiomeGenerator.FixedRoom.Generate.GenerateOnLoad : BiomeGenerator.FixedRoom.Generate.DontGenerate,
                PrefabPath = mapNode.blueprint.RoomPrefabs[UnityEngine.Random.Range(0, mapNode.blueprint.RoomPrefabs.Length)],
                x = 0,
                y = 0
              }
            };
            if (MapManager.Instance.CurrentMap.GetFinalBossNode() == MapManager.Instance.CurrentNode)
            {
              BiomeGenerator.Instance.OverrideRooms[0].North = GenerateRoom.ConnectionTypes.True;
              BiomeGenerator.Instance.OverrideRooms.Insert(0, new BiomeGenerator.OverrideRoom()
              {
                North = GenerateRoom.ConnectionTypes.False,
                South = GenerateRoom.ConnectionTypes.True,
                Generated = BiomeGenerator.FixedRoom.Generate.GenerateOnLoad,
                PrefabPath = BiomeGenerator.Instance.EndOfFloorRoomPath,
                x = 0,
                y = 1
              });
            }
          }
          if (MapGenerator.GetNodesOnLayer(1).Contains(mapNode) && DungeonSandboxManager.Instance.CurrentScenarioType == DungeonSandboxManager.ScenarioType.BossRushMode)
          {
            BiomeGenerator.Instance.OverrideRooms[0].y = 1;
            BiomeGenerator.Instance.OverrideRooms[0].South = GenerateRoom.ConnectionTypes.True;
            BiomeGenerator.Instance.OverrideRooms.Insert(0, new BiomeGenerator.OverrideRoom()
            {
              North = GenerateRoom.ConnectionTypes.True,
              South = GenerateRoom.ConnectionTypes.Entrance,
              Generated = BiomeGenerator.FixedRoom.Generate.GenerateOnLoad,
              PrefabPath = BiomeGenerator.Instance.EntranceRoomPath,
              x = 0,
              y = 0
            });
          }
          BiomeGenerator.Instance.DoFirstArrivalRoutine = true;
          BiomeGenerator.Instance.OverrideRandomWalk = true;
          break;
        }
        break;
      case NodeType.Follower:
      case NodeType.Follower_Beginner:
      case NodeType.Follower_Easy:
      case NodeType.Follower_Medium:
      case NodeType.Follower_Hard:
        string roomPrefab = mapNode.blueprint.RoomPrefabs[0];
        bool flag = false;
        foreach (ObjectivesData objective in DataManager.Instance.Objectives)
        {
          if (objective.Type == Objectives.TYPES.FIND_FOLLOWER && ((Objectives_FindFollower) objective).TargetLocation == BiomeGenerator.Instance.DungeonLocation)
          {
            flag = true;
            break;
          }
        }
        if (!flag && mapNode.blueprint.RoomPrefabs.Length > 1)
        {
          float num = UnityEngine.Random.value;
          if ((double) num < 0.20000000298023224 || DataManager.Instance.GiveExecutionerFollower)
            roomPrefab = mapNode.blueprint.RoomPrefabs[1];
          else if (mapNode.blueprint.RoomPrefabs.Length > 2 && (double) num < 0.40000000596046448 && DataManager.Instance.BeatenFirstMiniBoss && !DataManager.Instance.DissentingFolllowerRooms.Contains(PlayerFarming.Location))
          {
            roomPrefab = mapNode.blueprint.RoomPrefabs[2];
            DataManager.Instance.DissentingFolllowerRooms.Add(PlayerFarming.Location);
          }
        }
        BiomeGenerator.Instance.OverrideRooms = new List<BiomeGenerator.OverrideRoom>()
        {
          new BiomeGenerator.OverrideRoom()
          {
            North = GenerateRoom.ConnectionTypes.NextLayer,
            South = GenerateRoom.ConnectionTypes.Entrance,
            Generated = BiomeGenerator.FixedRoom.Generate.GenerateOnLoad,
            PrefabPath = roomPrefab
          }
        };
        BiomeGenerator.Instance.OverrideRandomWalk = true;
        break;
      case NodeType.FirstFloor:
        GameManager.CurrentDungeonFloor = 1;
        if (GameManager.DungeonEndlessLevel <= 1 && !DungeonSandboxManager.Active)
          Inteaction_DoorRoomDoor.GetFloor(PlayerFarming.Location, true);
        BiomeGenerator.Instance.OverrideRandomWalk = false;
        break;
      case NodeType.Intro_TeleportHome:
        BiomeGenerator.Instance.OverrideRooms = new List<BiomeGenerator.OverrideRoom>()
        {
          new BiomeGenerator.OverrideRoom()
          {
            North = GenerateRoom.ConnectionTypes.False,
            South = GenerateRoom.ConnectionTypes.Entrance,
            Generated = BiomeGenerator.FixedRoom.Generate.GenerateOnLoad,
            PrefabPath = mapNode.blueprint.RoomPrefabs[UnityEngine.Random.Range(0, mapNode.blueprint.RoomPrefabs.Length)]
          }
        };
        BiomeGenerator.Instance.OverrideRandomWalk = true;
        break;
      default:
        Debug.Log((object) "OVERRIDE!");
        BiomeGenerator.Instance.OverrideRooms = new List<BiomeGenerator.OverrideRoom>()
        {
          new BiomeGenerator.OverrideRoom()
          {
            North = GenerateRoom.ConnectionTypes.NextLayer,
            South = GenerateRoom.ConnectionTypes.Entrance,
            Generated = BiomeGenerator.FixedRoom.Generate.GenerateOnLoad,
            PrefabPath = mapNode.blueprint.RoomPrefabs[UnityEngine.Random.Range(0, mapNode.blueprint.RoomPrefabs.Length)]
          }
        };
        BiomeGenerator.Instance.OverrideRandomWalk = true;
        if (DungeonSandboxManager.Active && mapNode.nodeType == NodeType.FinalBoss)
        {
          BiomeGenerator.Instance.DoFirstArrivalRoutine = false;
          BiomeGenerator.Instance.OverrideRooms[0].Generated = BiomeGenerator.FixedRoom.Generate.DontGenerate;
        }
        if (this._adventureMapInstance.NodeFromPoint(mapNode.point).NodeBlueprint.nodeType == NodeType.Executioner)
        {
          BiomeGenerator.Instance.OverrideRooms[0].North = GenerateRoom.ConnectionTypes.False;
          BiomeGenerator.Instance.OverrideRooms[0].East = GenerateRoom.ConnectionTypes.NextLayer;
          BiomeGenerator.Instance.OverrideRooms[0].Generated = BiomeGenerator.FixedRoom.Generate.DontGenerate;
          break;
        }
        break;
    }
    DataManager.Instance.dungeonVisitedRooms.Add(mapNode.nodeType);
    DataManager.Instance.dungeonLocationsVisited.Add(PlayerFarming.Location);
    DataManager.Instance.FollowersRecruitedInNodes.Add(DataManager.Instance.FollowersRecruitedThisNode);
    DataManager.Instance.FollowersRecruitedThisNode = 0;
    BiomeGenerator.Instance.Regenerate((System.Action) (() =>
    {
      if ((UnityEngine.Object) this._adventureMapInstance != (UnityEngine.Object) null)
        this._adventureMapInstance.Hide();
      BiomeConstants.Instance.CreatePools();
    }));
  }

  public static NodeBlueprint GetBlueprint(NodeType type, MapConfig config)
  {
    NodeBlueprint blueprint = config.nodeBlueprints.FirstOrDefault<NodeBlueprint>((Func<NodeBlueprint, bool>) (n => n.nodeType == type));
    if ((UnityEngine.Object) blueprint != (UnityEngine.Object) null)
      return blueprint;
    if (type == config.FirstFloorBluePrint.nodeType)
      return config.FirstFloorBluePrint;
    if (type == config.SecondFloorBluePrint.nodeType)
      return config.SecondFloorBluePrint;
    if (type == config.MiniBossFloorBluePrint.nodeType)
      return config.MiniBossFloorBluePrint;
    if (type == config.TreasureBluePrint.nodeType)
      return config.TreasureBluePrint;
    if (type == config.LeaderFloorBluePrint.nodeType)
      return config.LeaderFloorBluePrint;
    foreach (MapLayer layer in config.layers)
    {
      if (layer.nodeType == type)
        return layer.BluePrint;
    }
    return blueprint;
  }

  [CompilerGenerated]
  public void \u003CShowMap\u003Eb__23_0()
  {
    if ((double) this.mainCamera.farClipPlane != 0.019999999552965164)
      this.clipPlane = this.mainCamera.farClipPlane;
    this.mainCamera.farClipPlane = 0.02f;
    System.Action onMapShown = this.OnMapShown;
    if (onMapShown == null)
      return;
    onMapShown();
  }

  [CompilerGenerated]
  public void \u003CShowMap\u003Eb__23_1()
  {
    if ((double) this.clipPlane == 0.0)
      return;
    this.mainCamera.farClipPlane = this.clipPlane;
  }

  [CompilerGenerated]
  public void \u003CShowMap\u003Eb__23_2() => AudioManager.Instance.StopLoop(this._loopedSound);

  [CompilerGenerated]
  public void \u003CShowMap\u003Eb__23_3() => AudioManager.Instance.StopLoop(this._loopedSound);

  [CompilerGenerated]
  public void \u003CEnterNode\u003Eb__29_0()
  {
    if ((UnityEngine.Object) this._adventureMapInstance != (UnityEngine.Object) null)
      this._adventureMapInstance.Hide();
    BiomeConstants.Instance.CreatePools();
  }
}
