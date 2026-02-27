// Decompiled with JetBrains decompiler
// Type: Map.MapManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using Lamb.UI;
using MMBiomeGeneration;
using MMRoomGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
namespace Map;

public class MapManager : MonoBehaviour
{
  public System.Action OnMapShown;
  public MapConfig DungeonConfig;
  public static MapManager Instance;
  private UIAdventureMapOverlayController _adventureMapInstance;
  private EventInstance _loopedSound;
  private float clipPlane;
  private Camera mainCamera;

  public Map.Map CurrentMap { get; private set; }

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

  public bool MapGenerated { get; set; }

  private void Start()
  {
    this.mainCamera = Camera.main;
    MapManager.Instance = this;
  }

  public void OnDestroy() => MapManager.Instance = (MapManager) null;

  public UIAdventureMapOverlayController ShowMap(bool disableInput = false)
  {
    SimulationManager.Pause();
    if (!this.MapGenerated)
      this.GenerateNewMap();
    this._adventureMapInstance = MonoSingleton<UIManager>.Instance.ShowAdventureMap(this.CurrentMap, disableInput);
    UIAdventureMapOverlayController adventureMapInstance1 = this._adventureMapInstance;
    adventureMapInstance1.OnShow = adventureMapInstance1.OnShow + (System.Action) (() =>
    {
      if ((double) this.mainCamera.farClipPlane != 0.019999999552965164)
        this.clipPlane = this.mainCamera.farClipPlane;
      this.mainCamera.farClipPlane = 0.02f;
      System.Action onMapShown = this.OnMapShown;
      if (onMapShown != null)
        onMapShown();
      this._loopedSound = AudioManager.Instance.CreateLoop("event:/atmos/forest/temple_door_hum", PlayerFarming.Instance.gameObject, true);
    });
    UIAdventureMapOverlayController adventureMapInstance2 = this._adventureMapInstance;
    adventureMapInstance2.OnHide = adventureMapInstance2.OnHide + (System.Action) (() =>
    {
      if ((double) this.clipPlane != 0.0)
        this.mainCamera.farClipPlane = this.clipPlane;
      AudioManager.Instance.StopLoop(this._loopedSound);
    });
    return this._adventureMapInstance;
  }

  public void CloseMap()
  {
    SimulationManager.UnPause();
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
    Debug.Log((object) ("EnterNode " + (object) mapNode.nodeType));
    switch (mapNode.nodeType)
    {
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
        if (!flag && mapNode.blueprint.RoomPrefabs.Length >= 3)
        {
          float num = UnityEngine.Random.value;
          if ((double) num < 0.20000000298023224)
            roomPrefab = mapNode.blueprint.RoomPrefabs[1];
          else if ((double) num < 0.40000000596046448 && DataManager.Instance.BeatenFirstMiniBoss && !DataManager.Instance.DissentingFolllowerRooms.Contains(PlayerFarming.Location))
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
        if (GameManager.DungeonEndlessLevel <= 1)
          Inteaction_DoorRoomDoor.GetFloor(PlayerFarming.Location, true);
        BiomeGenerator.Instance.OverrideRandomWalk = false;
        break;
      case NodeType.DungeonFloor:
      case NodeType.MiniBossFloor:
        Debug.Log((object) ("AAA " + (object) mapNode.nodeType));
        ++GameManager.CurrentDungeonFloor;
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
        break;
    }
    DataManager.Instance.dungeonVisitedRooms.Add(mapNode.nodeType);
    DataManager.Instance.FollowersRecruitedInNodes.Add(DataManager.Instance.FollowersRecruitedThisNode);
    DataManager.Instance.FollowersRecruitedThisNode = 0;
    BiomeGenerator.Instance.Regenerate((System.Action) (() =>
    {
      if (!((UnityEngine.Object) this._adventureMapInstance != (UnityEngine.Object) null))
        return;
      this._adventureMapInstance.Hide();
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
    return type == config.LeaderFloorBluePrint.nodeType ? config.LeaderFloorBluePrint : blueprint;
  }
}
