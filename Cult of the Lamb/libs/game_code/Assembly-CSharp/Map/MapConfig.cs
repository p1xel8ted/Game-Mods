// Decompiled with JetBrains decompiler
// Type: Map.MapConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Map;

[CreateAssetMenu]
public class MapConfig : ScriptableObject
{
  public bool RegenerateOnGenerateMap = true;
  public bool EnsureMinimumRewards = true;
  public bool RanomExtraRewardLayers = true;
  public List<NodeBlueprint> nodeBlueprints;
  public List<NodeType> MinimumRewards = new List<NodeType>();
  public NodeBlueprint FirstFloorBluePrint;
  public NodeBlueprint SecondFloorBluePrint;
  public NodeBlueprint MiniBossFloorBluePrint;
  public NodeBlueprint TreasureBluePrint;
  public NodeBlueprint LeaderFloorBluePrint;
  public IntMinMax numOfPreBossNodes;
  public IntMinMax numOfStartingNodes;
  public int NumOfCombatNodes;
  public int NumOfBlankNodes;
  public int NumOfMiniBossNodes;
  public float ChanceForBlank = 0.5f;
  [HideInInspector]
  public int MinStone;
  [HideInInspector]
  public int MinWood;
  [HideInInspector]
  public int MinFood;
  [Space]
  public NodeType[] FirstLayerBlacklist = new NodeType[0];
  public NodeType[] LastLayerBlacklist = new NodeType[0];
  public static MapLayer Treasure = new MapLayer()
  {
    nodeType = NodeType.Treasure,
    distanceFromPreviousLayer = new FloatMinMax()
    {
      min = 1f,
      max = 2f
    },
    nodesApartDistance = 2f,
    randomizePosition = 0.542f,
    randomizeNodes = 1f
  };
  public static MapLayer FirstFloor = new MapLayer()
  {
    nodeType = NodeType.FirstFloor,
    distanceFromPreviousLayer = new FloatMinMax()
    {
      min = 1f,
      max = 2f
    },
    nodesApartDistance = 2f,
    randomizePosition = 0.0f,
    randomizeNodes = 0.0f
  };
  public static MapLayer SecondFloor = new MapLayer()
  {
    nodeType = NodeType.DungeonFloor,
    distanceFromPreviousLayer = new FloatMinMax()
    {
      min = 1f,
      max = 2f
    },
    nodesApartDistance = 2f,
    randomizePosition = 0.542f,
    randomizeNodes = 0.0f
  };
  public static MapLayer MiniBoss = new MapLayer()
  {
    nodeType = NodeType.MiniBossFloor,
    distanceFromPreviousLayer = new FloatMinMax()
    {
      min = 1f,
      max = 2f
    },
    nodesApartDistance = 2f,
    randomizePosition = 0.542f,
    randomizeNodes = 0.0f
  };
  public static MapLayer Boss = new MapLayer()
  {
    nodeType = NodeType.Boss,
    distanceFromPreviousLayer = new FloatMinMax()
    {
      min = 1f,
      max = 2f
    },
    nodesApartDistance = 2f,
    randomizePosition = 0.542f,
    randomizeNodes = 0.0f
  };
  public int DungeonLength = 5;
  public int MaxDungeonLength = 5;
  public List<MapLayer> layers = new List<MapLayer>();

  public int GridWidth => Mathf.Max(this.numOfPreBossNodes.max, this.numOfStartingNodes.max);

  public static void Clear()
  {
    MapConfig.Treasure.BluePrint = (NodeBlueprint) null;
    MapConfig.FirstFloor.BluePrint = (NodeBlueprint) null;
    MapConfig.SecondFloor.BluePrint = (NodeBlueprint) null;
    MapConfig.MiniBoss.BluePrint = (NodeBlueprint) null;
    MapConfig.Boss.BluePrint = (NodeBlueprint) null;
  }

  public int TotalDungeonLength
  {
    get
    {
      return PlayerFarming.Location == FollowerLocation.Dungeon1_5 && (DataManager.Instance.CurrentDLCNodeType == DungeonWorldMapIcon.NodeType.Dungeon5_GhostNPC && !DataManager.Instance.OnboardedRotstoneDungeon || !DataManager.Instance.RancherSpokeAboutBrokenShop) || PlayerFarming.Location == FollowerLocation.Dungeon1_5 && DataManager.Instance.CurrentDLCNodeType == DungeonWorldMapIcon.NodeType.Dungeon5_MiniBoss && !DataManager.Instance.OnboardedLightningShardDungeon || PlayerFarming.Location == FollowerLocation.Dungeon1_6 && DataManager.Instance.CurrentDLCNodeType == DungeonWorldMapIcon.NodeType.Dungeon6_MiniBoss && !DataManager.Instance.OnboardedYewCursedDungeon ? 3 : Mathf.Clamp(this.DungeonLength + GameManager.CurrentDungeonLayer - 1, 1, this.MaxDungeonLength);
    }
  }

  public void ResetLayer()
  {
    Debug.Log((object) "RESETTING LAYER!");
    this.layers = new List<MapLayer>();
    int num = -1;
    while (++num < this.TotalDungeonLength)
    {
      if (num == 0)
      {
        MapConfig.FirstFloor.BluePrint = this.FirstFloorBluePrint;
        this.layers.Add(MapConfig.FirstFloor);
      }
      else if (num == this.TotalDungeonLength - 1)
      {
        MapConfig.MiniBoss.BluePrint = this.MiniBossFloorBluePrint;
        if (DataManager.Instance.DungeonBossFight && (bool) (Object) this.LeaderFloorBluePrint)
          MapConfig.MiniBoss.BluePrint = this.LeaderFloorBluePrint;
        this.layers.Add(MapConfig.MiniBoss);
      }
      else
      {
        MapConfig.Treasure.BluePrint = this.MiniBossFloorBluePrint;
        this.layers.Add(MapConfig.Treasure);
      }
    }
  }
}
