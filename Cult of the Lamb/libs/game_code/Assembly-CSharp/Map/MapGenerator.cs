// Decompiled with JetBrains decompiler
// Type: Map.MapGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMBiomeGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
namespace Map;

public static class MapGenerator
{
  public static MapConfig config;
  public static List<NodeType> RandomNodes;
  public static List<float> layerDistances;
  public static List<List<Point>> paths;
  public static List<List<Node>> nodes = new List<List<Node>>();
  public static List<Node> RewardNodes = new List<Node>();
  public static int blankNodes = 0;
  public static List<NodeType> nonOverrideNodes = new List<NodeType>()
  {
    NodeType.Follower_Beginner,
    NodeType.Follower_Easy,
    NodeType.Follower_Medium,
    NodeType.MarketplaceRelics,
    NodeType.Follower_Hard,
    NodeType.Negative_PreviousMiniboss,
    NodeType.MiniBossFloor,
    NodeType.Boss,
    NodeType.FinalBoss,
    NodeType.MarketPlaceCat,
    NodeType.Special_FindRelic,
    NodeType.Executioner,
    NodeType.Magma_Stone,
    NodeType.Special_DepositFollower,
    NodeType.MarketPlaceClothes,
    NodeType.Special_HappyFollower,
    NodeType.Special_DepositInfectedPet,
    NodeType.Special_Healing
  };

  public static List<List<Node>> Nodes => MapGenerator.nodes;

  public static void Clear()
  {
    MapGenerator.config = (MapConfig) null;
    MapGenerator.RandomNodes?.Clear();
    MapGenerator.layerDistances?.Clear();
    MapGenerator.paths?.Clear();
    MapGenerator.nodes?.Clear();
    MapGenerator.RewardNodes?.Clear();
    MapGenerator.blankNodes = 0;
  }

  public static Map.Map GetMap(MapConfig conf)
  {
    if ((UnityEngine.Object) conf == (UnityEngine.Object) null)
    {
      Debug.LogWarning((object) "Config was null in MapGenerator.Generate()");
      return (Map.Map) null;
    }
    UnityEngine.Random.InitState(BiomeGenerator.Instance.Seed);
    if (conf.RegenerateOnGenerateMap)
      conf.ResetLayer();
    MapGenerator.config = conf;
    MapGenerator.RandomNodes = new List<NodeType>();
    foreach (NodeBlueprint nodeBlueprint in MapGenerator.config.nodeBlueprints)
      MapGenerator.RandomNodes.Add(nodeBlueprint.nodeType);
    MapGenerator.nodes.Clear();
    MapGenerator.RewardNodes.Clear();
    MapGenerator.GenerateLayerDistances();
    for (int layerIndex = 0; layerIndex < conf.layers.Count; ++layerIndex)
      MapGenerator.PlaceLayer(layerIndex, conf);
    if (PlayerFarming.Location == FollowerLocation.Dungeon1_5 && DataManager.Instance.CurrentDLCNodeType == DungeonWorldMapIcon.NodeType.Dungeon5_GhostNPC && !DataManager.Instance.OnboardedRotstoneDungeon || PlayerFarming.Location == FollowerLocation.Dungeon1_5 && DataManager.Instance.CurrentDLCNodeType == DungeonWorldMapIcon.NodeType.Dungeon5_MiniBoss && !DataManager.Instance.OnboardedLightningShardDungeon || PlayerFarming.Location == FollowerLocation.Dungeon1_6 && DataManager.Instance.CurrentDLCNodeType == DungeonWorldMapIcon.NodeType.Dungeon6_MiniBoss && !DataManager.Instance.OnboardedYewCursedDungeon)
    {
      for (int index = 3; index >= 1; --index)
      {
        MapGenerator.nodes[0].RemoveAt(index);
        MapGenerator.nodes[1].RemoveAt(index);
        MapGenerator.nodes[2].RemoveAt(index);
      }
      MapGenerator.nodes[0][0].outgoing.Add(MapGenerator.nodes[1][0].point);
      MapGenerator.nodes[1][0].incoming.Add(MapGenerator.nodes[0][0].point);
      MapGenerator.nodes[1][0].outgoing.Add(MapGenerator.nodes[2][0].point);
      MapGenerator.nodes[2][0].incoming.Add(MapGenerator.nodes[1][0].point);
    }
    else
    {
      MapGenerator.GeneratePaths();
      MapGenerator.SetUpConnections();
      MapGenerator.RemoveCrossConnections();
      if (DungeonSandboxManager.Active)
        MapGenerator.FixBossNodes();
      MapGenerator.ReconnectEmptyNodes();
    }
    List<Node> list = MapGenerator.nodes.SelectMany<List<Node>, Node>((Func<List<Node>, IEnumerable<Node>>) (n => (IEnumerable<Node>) n)).Where<Node>((Func<Node, bool>) (n => n.incoming.Count > 0 || n.outgoing.Count > 0)).ToList<Node>();
    if (conf.EnsureMinimumRewards)
      MapGenerator.EnsureMinimumRewards(conf, list);
    MapGenerator.EnsureCostNodes(conf, list);
    if (MapGenerator.config.NumOfCombatNodes > 0)
      MapGenerator.SetCombatNodes();
    MapGenerator.SetNodePositions();
    if (DungeonSandboxManager.Active)
    {
      MapGenerator.EnsureNodesFixed();
      MapGenerator.SetMiniBossNodes();
      MapGenerator.EvenWeaponTarotRelicRooms();
    }
    return new Map.Map(conf.name, list, new List<Point>());
  }

  public static void SetMiniBossNodes()
  {
    int numOfMiniBossNodes = MapGenerator.config.NumOfMiniBossNodes;
    int num = 0;
    while (num++ < 100 && numOfMiniBossNodes > 0)
    {
      int index = UnityEngine.Random.Range(0, MapGenerator.nodes.Count);
      Node node = MapGenerator.nodes[index][UnityEngine.Random.Range(0, MapGenerator.nodes[index].Count)];
      if (node.nodeType == NodeType.DungeonFloor && node.incoming.Where<Point>((Func<Point, bool>) (x => MapGenerator.GetNode(x).nodeType == NodeType.MiniBossFloor)).FirstOrDefault<Point>() == null && node.outgoing.Where<Point>((Func<Point, bool>) (x => MapGenerator.GetNode(x).nodeType == NodeType.MiniBossFloor)).FirstOrDefault<Point>() == null)
      {
        node.nodeType = NodeType.MiniBossFloor;
        node.DungeonLocation = MapGenerator.config.layers[index].BluePrint.ForcedDungeon;
        node.blueprint = MapGenerator.config.layers[index].OtherBluePrints.Length != 0 ? MapGenerator.config.layers[index].OtherBluePrints[UnityEngine.Random.Range(0, MapGenerator.config.layers[index].OtherBluePrints.Length)] : MapManager.Instance.DungeonConfig.MiniBossFloorBluePrint;
        node.Hidden = false;
        node.Modifier = DungeonModifier.GetModifier();
        --numOfMiniBossNodes;
      }
    }
  }

  public static void EvenWeaponTarotRelicRooms()
  {
    List<NodeType> nodeTypeList1 = new List<NodeType>()
    {
      NodeType.MarketplaceRelics,
      NodeType.MarketPlaceWeapons,
      NodeType.Special_Healing,
      NodeType.MarketPlaceCat,
      NodeType.Tarot
    };
    List<NodeType> nodeTypeList2 = new List<NodeType>();
    if (PlayerFleeceManager.FleecePreventTarotCards())
      nodeTypeList1.Remove(NodeType.Tarot);
    if (DungeonSandboxManager.Active && PlayerFleeceManager.FleecePreventsHealthPickups())
      nodeTypeList1.Remove(NodeType.MarketPlaceCat);
    while (nodeTypeList1.Count > 0)
    {
      int index = UnityEngine.Random.Range(0, nodeTypeList1.Count);
      nodeTypeList2.Add(nodeTypeList1[index]);
      nodeTypeList1.RemoveAt(index);
    }
    List<Node> nodeList1 = new List<Node>();
    for (int index1 = MapGenerator.Nodes.Count - 1; index1 >= 0; --index1)
    {
      for (int index2 = MapGenerator.Nodes[index1].Count - 1; index2 >= 0; --index2)
      {
        Node node = MapGenerator.Nodes[index1][index2];
        if (nodeTypeList2.Contains(node.nodeType))
        {
          Debug.Log((object) ("Add to list! " + node.nodeType.ToString()));
          nodeList1.Add(node);
        }
      }
    }
    List<Node> nodeList2 = new List<Node>();
    while (nodeList1.Count > 0)
    {
      int index = UnityEngine.Random.Range(0, nodeList1.Count);
      nodeList2.Add(nodeList1[index]);
      nodeList1.RemoveAt(index);
    }
    int index3 = 0;
    foreach (Node node in nodeList2)
    {
      NodeType Type = nodeTypeList2[index3];
      Debug.Log((object) $"{"Random: ".Colour(Color.green)} {index3.ToString()}  {Type.ToString()}");
      node.nodeType = Type;
      node.blueprint = MapGenerator.config.nodeBlueprints.FirstOrDefault<NodeBlueprint>((Func<NodeBlueprint, bool>) (B => B.nodeType == Type));
      index3 = (index3 + 1) % nodeTypeList2.Count;
    }
  }

  public static void EnsureMinimumRewards(MapConfig conf, List<Node> NodesList)
  {
    List<Node> nodeList = new List<Node>((IEnumerable<Node>) MapGenerator.RewardNodes);
    List<NodeType> nodeTypeList = new List<NodeType>((IEnumerable<NodeType>) conf.MinimumRewards);
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      if (objective.Type == Objectives.TYPES.FIND_FOLLOWER && ((Objectives_FindFollower) objective).TargetLocation == BiomeGenerator.Instance.DungeonLocation)
      {
        nodeTypeList.Add(NodeType.Follower_Beginner);
        nodeTypeList.Add(NodeType.Follower_Easy);
        nodeTypeList.Add(NodeType.Follower_Medium);
        nodeTypeList.Add(NodeType.Follower_Hard);
        break;
      }
    }
    foreach (NodeBlueprint nodeBlueprint in MapGenerator.config.nodeBlueprints)
    {
      if (nodeBlueprint.HasEnsuredConditions)
      {
        bool flag = true;
        foreach (BiomeGenerator.VariableAndCondition conditionalVariable in nodeBlueprint.EnsuredConditionalVariables)
        {
          if (DataManager.Instance.GetVariable(conditionalVariable.Variable) != conditionalVariable.Condition)
            flag = false;
        }
        if (flag)
          nodeTypeList.Add(nodeBlueprint.nodeType);
      }
    }
    foreach (NodeType nodeType in nodeTypeList)
    {
      NodeType minimumReward = nodeType;
      if (NodesList.Where<Node>((Func<Node, bool>) (x => x.nodeType == minimumReward)).FirstOrDefault<Node>() == null)
      {
        string blueprintName = MapGenerator.config.nodeBlueprints.Where<NodeBlueprint>((Func<NodeBlueprint, bool>) (b => b.nodeType == minimumReward)).ToList<NodeBlueprint>().Random<NodeBlueprint>().name;
        NodeBlueprint nodeBlueprint = MapGenerator.config.nodeBlueprints.FirstOrDefault<NodeBlueprint>((Func<NodeBlueprint, bool>) (n => n.name == blueprintName));
        if ((nodeBlueprint.nodeType != NodeType.Wood || Inventory.GetItemQuantity(1) <= 5) && (nodeBlueprint.nodeType != NodeType.Stone || Inventory.GetItemQuantity(2) < 5) && (nodeBlueprint.nodeType != NodeType.Cotton || Inventory.GetItemQuantity(133) < 5) && (nodeBlueprint.nodeType != NodeType.Grapes || Inventory.GetItemQuantity(151) < 5) && (nodeBlueprint.nodeType != NodeType.Hops || Inventory.GetItemQuantity(150) < 5) && (nodeBlueprint.nodeType != NodeType.Food || Inventory.GetFoodAmount() <= 10))
        {
          if (nodeBlueprint.RequireCondition && !nodeBlueprint.IgnoreRequiredConditions)
          {
            bool flag = true;
            foreach (BiomeGenerator.VariableAndCondition conditionalVariable in nodeBlueprint.ConditionalVariables)
            {
              if (DataManager.Instance.GetVariable(conditionalVariable.Variable) != conditionalVariable.Condition)
                flag = false;
            }
            if (!flag)
              continue;
          }
          int index = UnityEngine.Random.Range(0, nodeList.Count);
          int num = 100;
          while (--num > 0 && index < nodeList.Count && (nodeList[index].nodeType == NodeType.Wood || nodeList[index].nodeType == NodeType.Food || nodeList[index].nodeType == NodeType.Stone || nodeList[index].nodeType == NodeType.None || !NodesList.Contains(nodeList[index]) || MapGenerator.nonOverrideNodes.Contains(nodeList[index].nodeType)))
            index = UnityEngine.Random.Range(0, nodeList.Count);
          if (nodeList.Count > 0)
          {
            nodeList[index].nodeType = minimumReward;
            nodeList[index].blueprint = nodeBlueprint;
            nodeList[index].Hidden = false;
            nodeList[index].CanBeHidden = false;
            nodeList.RemoveAt(index);
          }
        }
      }
    }
  }

  public static void EnsureCostNodes(MapConfig conf, List<Node> NodesList)
  {
    foreach (NodeBlueprint nodeBlueprint in MapGenerator.config.nodeBlueprints)
    {
      if (nodeBlueprint.HasCost && (double) UnityEngine.Random.Range(0.0f, 1f) <= (double) nodeBlueprint.Probability)
      {
        bool flag = false;
        if (nodeBlueprint.HasEnsuredConditions)
        {
          flag = true;
          foreach (BiomeGenerator.VariableAndCondition conditionalVariable in nodeBlueprint.EnsuredConditionalVariables)
          {
            if (DataManager.Instance.GetVariable(conditionalVariable.Variable) != conditionalVariable.Condition)
              flag = false;
          }
        }
        if (!flag)
        {
          flag = true;
          if (nodeBlueprint.RequireCondition)
          {
            foreach (BiomeGenerator.VariableAndCondition conditionalVariable in nodeBlueprint.ConditionalVariables)
            {
              if (DataManager.Instance.GetVariable(conditionalVariable.Variable) != conditionalVariable.Condition)
                flag = false;
            }
          }
        }
        if (flag)
        {
          List<Node> nodeList = new List<Node>();
          for (int i = 0; i < NodesList.Count; i++)
          {
            if (!MapGenerator.nonOverrideNodes.Contains(NodesList[i].nodeType) && NodesList[i] != MapGenerator.GetFirstNodeOnLayer(0) && NodesList[i].point != MapGenerator.GetFinalNode() && NodesList[i].incoming.Count == 1 && NodesList[i].outgoing.Count == 1 && NodesList.FirstOrDefault<Node>((Func<Node, bool>) (n => n.point.Equals(NodesList[i].incoming[0]))).outgoing.Count >= 2)
              nodeList.Add(NodesList[i]);
          }
          if (nodeList.Count > 0)
          {
            Node node = nodeList[UnityEngine.Random.Range(0, nodeList.Count)];
            node.nodeType = nodeBlueprint.nodeType;
            node.blueprint = nodeBlueprint;
            node.Hidden = false;
            node.CanBeHidden = false;
          }
        }
      }
    }
  }

  public static void GenerateLayerDistances()
  {
    MapGenerator.layerDistances = new List<float>();
    foreach (MapLayer layer in MapGenerator.config.layers)
    {
      float num = layer.distanceFromPreviousLayer.GetValue();
      MapGenerator.layerDistances.Add(num);
    }
  }

  public static float GetDistanceToLayer(int layerIndex)
  {
    return layerIndex < 0 || layerIndex > MapGenerator.layerDistances.Count ? 0.0f : MapGenerator.layerDistances.Take<float>(layerIndex + 1).Sum();
  }

  public static void PlaceLayer(int layerIndex, MapConfig mapConfig)
  {
    MapLayer layer = MapGenerator.config.layers[layerIndex];
    List<Node> nodesOnThisLayer = new List<Node>();
    float offset = (float) ((double) layer.nodesApartDistance * (double) MapGenerator.config.GridWidth / 2.0);
    int gridWidth = MapGenerator.config.GridWidth;
    List<NodeBlueprint> nodeBlueprintList = layer.OtherBluePrints == null || layer.OtherBluePrints.Length == 0 ? new List<NodeBlueprint>() : new List<NodeBlueprint>((IEnumerable<NodeBlueprint>) layer.OtherBluePrints);
    for (int i = 0; i < gridWidth; ++i)
    {
      if (nodeBlueprintList.Count > 0)
      {
        NodeBlueprint nodeBlueprint = nodeBlueprintList[UnityEngine.Random.Range(0, nodeBlueprintList.Count)];
        layer.BluePrint = nodeBlueprint;
        nodeBlueprintList.Remove(nodeBlueprint);
      }
      bool randomiseReward = (double) UnityEngine.Random.Range(0.0f, 1f) < (double) layer.randomizeNodes;
      Node nodeBasedOnLayer = MapGenerator.GetNodeBasedOnLayer(layer, mapConfig, nodesOnThisLayer, layerIndex, i, offset, randomiseReward);
      if (nodeBasedOnLayer.blueprint.ForcedDungeon == FollowerLocation.None)
        nodeBasedOnLayer.DungeonLocation = layer.BluePrint.ForcedDungeon;
      if (PlayerFarming.Location == FollowerLocation.Dungeon1_5 && layerIndex == 1 && DataManager.Instance.CurrentDLCNodeType == DungeonWorldMapIcon.NodeType.Dungeon5_GhostNPC && !DataManager.Instance.OnboardedRotstoneDungeon || PlayerFarming.Location == FollowerLocation.Dungeon1_5 && layerIndex == 1 && DataManager.Instance.CurrentDLCNodeType == DungeonWorldMapIcon.NodeType.Dungeon5_MiniBoss && !DataManager.Instance.OnboardedLightningShardDungeon || PlayerFarming.Location == FollowerLocation.Dungeon1_6 && layerIndex == 1 && DataManager.Instance.CurrentDLCNodeType == DungeonWorldMapIcon.NodeType.Dungeon6_MiniBoss && !DataManager.Instance.OnboardedYewCursedDungeon)
        randomiseReward = false;
      nodesOnThisLayer.Add(nodeBasedOnLayer);
      if (randomiseReward)
        MapGenerator.RewardNodes.Add(nodeBasedOnLayer);
    }
    MapGenerator.nodes.Add(nodesOnThisLayer);
  }

  public static Node GetNodeBasedOnLayer(
    MapLayer layer,
    MapConfig mapConfig,
    List<Node> nodesOnThisLayer,
    int layerIndex,
    int i,
    float offset,
    bool randomiseReward)
  {
    if (PlayerFarming.Location == FollowerLocation.Dungeon1_5 && layerIndex == 1 && DataManager.Instance.CurrentDLCNodeType == DungeonWorldMapIcon.NodeType.Dungeon5_GhostNPC && !DataManager.Instance.OnboardedRotstoneDungeon)
      return new Node(NodeType.Magma_Stone, MapGenerator.config.nodeBlueprints.FirstOrDefault<NodeBlueprint>((Func<NodeBlueprint, bool>) (n => n.nodeType == NodeType.Magma_Stone)), new Point(i, layerIndex))
      {
        position = new Vector2((float) (-(double) offset + (double) i * (double) layer.nodesApartDistance), MapGenerator.GetDistanceToLayer(layerIndex))
      };
    if (PlayerFarming.Location == FollowerLocation.Dungeon1_5 && layerIndex == 1 && DataManager.Instance.CurrentDLCNodeType == DungeonWorldMapIcon.NodeType.Dungeon5_MiniBoss && !DataManager.Instance.OnboardedLightningShardDungeon)
      return new Node(NodeType.Lightning_Shard, MapGenerator.config.nodeBlueprints.FirstOrDefault<NodeBlueprint>((Func<NodeBlueprint, bool>) (n => n.nodeType == NodeType.Lightning_Shard)), new Point(i, layerIndex))
      {
        position = new Vector2((float) (-(double) offset + (double) i * (double) layer.nodesApartDistance), MapGenerator.GetDistanceToLayer(layerIndex))
      };
    if (PlayerFarming.Location == FollowerLocation.Dungeon1_6 && layerIndex == 1 && DataManager.Instance.CurrentDLCNodeType == DungeonWorldMapIcon.NodeType.Dungeon6_MiniBoss && !DataManager.Instance.OnboardedYewCursedDungeon)
      return new Node(NodeType.Yew_Cursed, MapGenerator.config.nodeBlueprints.FirstOrDefault<NodeBlueprint>((Func<NodeBlueprint, bool>) (n => n.nodeType == NodeType.Yew_Cursed)), new Point(i, layerIndex))
      {
        position = new Vector2((float) (-(double) offset + (double) i * (double) layer.nodesApartDistance), MapGenerator.GetDistanceToLayer(layerIndex))
      };
    AnimationCurve.EaseInOut(0.0f, 1f, 1f, 0.75f);
    NodeType nodeType = NodeType.None;
    Node nodeBasedOnLayer;
    if (randomiseReward)
    {
      Node node1;
      Node node2;
      do
      {
        do
        {
          nodeType = (double) UnityEngine.Random.Range(0.0f, 1f) > (double) mapConfig.ChanceForBlank || MapGenerator.blankNodes >= mapConfig.NumOfBlankNodes ? MapGenerator.GetRandomNode(mapConfig) : NodeType.None;
        }
        while (layerIndex == 1 && mapConfig.FirstLayerBlacklist.Contains<NodeType>(nodeType) || layerIndex >= mapConfig.layers.Count - 2 && mapConfig.LastLayerBlacklist.Contains<NodeType>(nodeType));
        node1 = MapGenerator.GetNode(new Point(i, layerIndex - 1));
        node2 = MapGenerator.GetNode(new Point(i, layerIndex + 1));
      }
      while (nodeType == NodeType.None && node1 != null && node1.nodeType == NodeType.None || node2 != null && node2.nodeType == NodeType.None || nodesOnThisLayer.FirstOrDefault<Node>((Func<Node, bool>) (x => x.nodeType != NodeType.None)) == null && nodeType == NodeType.None);
      string blueprintName = MapGenerator.config.nodeBlueprints.Where<NodeBlueprint>((Func<NodeBlueprint, bool>) (b => b.nodeType == nodeType)).ToList<NodeBlueprint>().Random<NodeBlueprint>().name;
      NodeBlueprint blueprint = MapGenerator.config.nodeBlueprints.FirstOrDefault<NodeBlueprint>((Func<NodeBlueprint, bool>) (n => n.name == blueprintName));
      if (blueprint.OnlyOne && !DungeonSandboxManager.Active)
      {
        Debug.Log((object) "ONLY ONE!");
        MapGenerator.RandomNodes.Remove(nodeType);
      }
      if (nodeType == NodeType.None)
        ++MapGenerator.blankNodes;
      nodeBasedOnLayer = new Node(nodeType, blueprint, new Point(i, layerIndex))
      {
        position = new Vector2((float) (-(double) offset + (double) i * (double) layer.nodesApartDistance), MapGenerator.GetDistanceToLayer(layerIndex))
      };
    }
    else
    {
      if (nodesOnThisLayer.FirstOrDefault<Node>((Func<Node, bool>) (x => x.nodeType == NodeType.Boss)) != null)
        return new Node(NodeType.None, MapGenerator.config.nodeBlueprints.FirstOrDefault<NodeBlueprint>((Func<NodeBlueprint, bool>) (b => b.nodeType == NodeType.None)), new Point(i, layerIndex))
        {
          position = new Vector2((float) (-(double) offset + (double) i * (double) layer.nodesApartDistance), MapGenerator.GetDistanceToLayer(layerIndex))
        };
      nodeType = layer.nodeType;
      nodeBasedOnLayer = new Node(nodeType, layer.BluePrint, new Point(i, layerIndex))
      {
        position = new Vector2((float) (-(double) offset + (double) i * (double) layer.nodesApartDistance), MapGenerator.GetDistanceToLayer(layerIndex))
      };
    }
    return nodeBasedOnLayer;
  }

  public static void SetNodePositions()
  {
    float num1 = 0.0f;
    for (int index = 0; index < MapGenerator.nodes.Count; ++index)
    {
      List<Node> node1 = MapGenerator.nodes[index];
      MapLayer layer = MapGenerator.config.layers[index];
      float num2 = index + 1 >= MapGenerator.layerDistances.Count ? 0.0f : MapGenerator.layerDistances[index + 1];
      float layerDistance = MapGenerator.layerDistances[index];
      foreach (Node node2 in node1)
      {
        double num3 = (double) UnityEngine.Random.Range(-0.5f, 0.5f);
        float num4 = UnityEngine.Random.Range(-0.75f, 0.75f);
        double nodesApartDistance = (double) layer.nodesApartDistance;
        float num5 = (float) (num3 * nodesApartDistance / 2.0);
        float y = (double) num4 < 0.0 ? (float) ((double) layerDistance * (double) num4 / 2.0) : (float) ((double) num2 * (double) num4 / 2.0);
        node2.position += new Vector2(num5 + num1, y) * layer.randomizePosition;
      }
      num1 = Mathf.Lerp(-1f, 1f, UnityEngine.Random.Range(0.0f, 1f));
    }
  }

  public static void SetUpConnections()
  {
    int num = 0;
    foreach (List<Point> path in MapGenerator.paths)
    {
      for (int index = 0; index < path.Count; ++index)
      {
        Node node1 = MapGenerator.GetNode(path[index]);
        if (index > 0)
        {
          Node node2 = MapGenerator.GetNode(path[index - 1]);
          node2.AddIncoming(node1.point);
          node1.AddOutgoing(node2.point);
        }
        if (index < path.Count - 1)
        {
          Node node3 = MapGenerator.GetNode(path[index + 1]);
          node3.AddOutgoing(node1.point);
          node1.AddIncoming(node3.point);
        }
      }
      ++num;
    }
  }

  public static void EnsureNodesFixed()
  {
    for (int index1 = MapGenerator.Nodes.Count - 1; index1 >= 0; --index1)
    {
      for (int index2 = MapGenerator.Nodes[index1].Count - 1; index2 >= 0; --index2)
      {
        Node node = MapGenerator.Nodes[index1][index2];
        if (node.outgoing.Count == 0 && node.nodeType != NodeType.FinalBoss && node.nodeType != NodeType.MiniBossFloor && node.nodeType != NodeType.Boss)
        {
          for (int index3 = 0; index3 < node.incoming.Count; ++index3)
            MapGenerator.GetNode(node.incoming[index3]).RemoveOutgoing(node.point);
          for (int index4 = 0; index4 < node.outgoing.Count; ++index4)
            MapGenerator.GetNode(node.outgoing[index4]).RemoveIncoming(node.point);
        }
        for (int index5 = node.incoming.Count - 1; index5 >= 0; --index5)
        {
          if (!MapGenerator.GetNode(node.incoming[index5]).outgoing.Contains(node.point))
            node.RemoveIncoming(node.incoming[index5]);
        }
      }
    }
  }

  public static void FixBossNodes()
  {
    foreach (List<Point> path in MapGenerator.paths)
    {
      for (int index1 = 0; index1 < path.Count; ++index1)
      {
        Node node1 = MapGenerator.GetNode(path[index1]);
        if (MapGenerator.config.layers[MapGenerator.GetNodeLayer(node1)].nodeType == NodeType.Boss || MapGenerator.config.layers[MapGenerator.GetNodeLayer(node1)].nodeType == NodeType.FinalBoss)
        {
          int layer = MapGenerator.GetNodeLayer(node1);
          Node node2 = MapGenerator.Nodes[layer].FirstOrDefault<Node>((Func<Node, bool>) (n => n.nodeType == MapGenerator.config.layers[layer].nodeType));
          List<Node> nodeList1 = new List<Node>((IEnumerable<Node>) MapGenerator.Nodes[layer]);
          if (nodeList1.Count > 1)
          {
            node2.incoming.Clear();
            node2.outgoing.Clear();
            for (int index2 = 0; index2 < nodeList1.Count; ++index2)
            {
              if (nodeList1[index2].nodeType == NodeType.None)
              {
                for (int index3 = 0; index3 < nodeList1[index2].incoming.Count; ++index3)
                {
                  MapGenerator.GetNode(nodeList1[index2].incoming[index3]).outgoing.Clear();
                  MapGenerator.GetNode(nodeList1[index2].incoming[index3]).outgoing.Add(node2.point);
                  if (!node2.incoming.Contains(nodeList1[index2].incoming[index3]))
                    node2.incoming.Add(nodeList1[index2].incoming[index3]);
                }
                for (int index4 = 0; index4 < nodeList1[index2].outgoing.Count; ++index4)
                {
                  MapGenerator.GetNode(nodeList1[index2].outgoing[index4]).incoming.Clear();
                  MapGenerator.GetNode(nodeList1[index2].outgoing[index4]).incoming.Add(node2.point);
                  if (!node2.outgoing.Contains(nodeList1[index2].outgoing[index4]))
                    node2.outgoing.Add(nodeList1[index2].outgoing[index4]);
                }
                nodeList1[index2].incoming.Clear();
                nodeList1[index2].outgoing.Clear();
              }
            }
            if (layer + 1 < MapGenerator.Nodes.Count)
            {
              List<Node> nodeList2 = new List<Node>((IEnumerable<Node>) MapGenerator.Nodes[layer + 1]);
              for (int index5 = 0; index5 < nodeList2.Count; ++index5)
              {
                nodeList2[index5].incoming.Clear();
                nodeList2[index5].incoming.Add(node2.point);
                if (!node2.outgoing.Contains(nodeList2[index5].point))
                  node2.outgoing.Add(nodeList2[index5].point);
              }
            }
          }
        }
      }
    }
  }

  public static void ReconnectEmptyNodes()
  {
    foreach (List<Point> path in MapGenerator.paths)
    {
      for (int index1 = 0; index1 < path.Count; ++index1)
      {
        Node node1 = MapGenerator.GetNode(path[index1]);
        if (node1.nodeType == NodeType.None)
        {
          for (int index2 = 0; index2 < node1.incoming.Count; ++index2)
          {
            for (int index3 = 0; index3 < node1.outgoing.Count; ++index3)
            {
              Node node2 = MapGenerator.GetNode(node1.incoming[index2]);
              Node node3 = MapGenerator.GetNode(node1.outgoing[index3]);
              node2.RemoveOutgoing(node1.point);
              node3.RemoveIncoming(node1.point);
              if (node2.outgoing.Count == 0 || node3.incoming.Count == 0)
              {
                node2.AddOutgoing(node3.point);
                node3.AddIncoming(node2.point);
              }
            }
          }
          node1.incoming.Clear();
          node1.outgoing.Clear();
        }
      }
    }
  }

  public static void SetCombatNodes()
  {
    AnimationCurve animationCurve = AnimationCurve.EaseInOut(0.0f, 1f, 1f, 0.75f);
    int num1 = GameManager.CurrentDungeonLayer - 1;
    int num2 = Mathf.RoundToInt((float) MapManager.Instance.DungeonConfig.NumOfCombatNodes * ((float) num1 * animationCurve.Evaluate((float) num1 / 3f)));
    int num3 = 0;
    bool flag = PlayerFarming.Location == FollowerLocation.Dungeon1_5 || PlayerFarming.Location == FollowerLocation.Dungeon1_6;
    for (int index1 = 0; index1 < 50; ++index1)
    {
      for (int index2 = 0; index2 < MapGenerator.nodes.Count; ++index2)
      {
        for (int index3 = 0; index3 < MapGenerator.nodes[index2].Count && num3 < num2; ++index3)
        {
          if ((MapGenerator.nodes[index2][index3].outgoing.Count > 0 && MapGenerator.nodes[index2][index3].incoming.Count > 0 && MapGenerator.nodes[index2][index3].outgoing.Where<Point>((Func<Point, bool>) (node => MapGenerator.GetNode(node).nodeType == NodeType.DungeonFloor)).FirstOrDefault<Point>() == null && MapGenerator.nodes[index2][index3].incoming.Where<Point>((Func<Point, bool>) (node => MapGenerator.GetNode(node).nodeType == NodeType.DungeonFloor)).FirstOrDefault<Point>() == null && (flag || index2 > 1) || DungeonSandboxManager.Active) && (double) UnityEngine.Random.value > 0.75 && !MapGenerator.nonOverrideNodes.Contains(MapGenerator.nodes[index2][index3].nodeType))
          {
            int nodeLayer = MapGenerator.GetNodeLayer(MapGenerator.nodes[index2][index3]);
            if (MapGenerator.config.layers[nodeLayer].CanBeReplacedWithCombatNode)
            {
              MapGenerator.nodes[index2][index3].nodeType = NodeType.DungeonFloor;
              MapGenerator.nodes[index2][index3].DungeonLocation = MapGenerator.config.layers[nodeLayer].BluePrint.ForcedDungeon;
              MapGenerator.nodes[index2][index3].blueprint = MapManager.Instance.DungeonConfig.SecondFloorBluePrint;
              MapGenerator.nodes[index2][index3].Hidden = false;
              MapGenerator.nodes[index2][index3].Modifier = DungeonModifier.GetModifier();
              ++num3;
            }
          }
        }
      }
      if (num3 >= num2)
        break;
    }
  }

  public static void RemoveCrossConnections()
  {
    for (int x = 0; x < MapGenerator.config.GridWidth - 1; ++x)
    {
      for (int y = 0; y < MapGenerator.config.layers.Count - 1; ++y)
      {
        Node node1 = MapGenerator.GetNode(new Point(x, y));
        if (node1 != null && !node1.HasNoConnections())
        {
          Node node2 = MapGenerator.GetNode(new Point(x + 1, y));
          if (node2 != null && !node2.HasNoConnections())
          {
            Node top = MapGenerator.GetNode(new Point(x, y + 1));
            if (top != null && !top.HasNoConnections())
            {
              Node topRight = MapGenerator.GetNode(new Point(x + 1, y + 1));
              if (topRight != null && !topRight.HasNoConnections() && node1.outgoing.Any<Point>((Func<Point, bool>) (element => element.Equals(topRight.point))) && node2.outgoing.Any<Point>((Func<Point, bool>) (element => element.Equals(top.point))))
              {
                node1.AddOutgoing(top.point);
                top.AddIncoming(node1.point);
                node2.AddOutgoing(topRight.point);
                topRight.AddIncoming(node2.point);
                double num = (double) UnityEngine.Random.Range(0.0f, 1f);
                if (num < 0.33000001311302185)
                {
                  node1.RemoveOutgoing(topRight.point);
                  topRight.RemoveIncoming(node1.point);
                  node2.RemoveOutgoing(top.point);
                  top.RemoveIncoming(node2.point);
                }
                if (num < 0.6600000262260437)
                {
                  node1.RemoveOutgoing(topRight.point);
                  topRight.RemoveIncoming(node1.point);
                }
                else
                {
                  node2.RemoveOutgoing(top.point);
                  top.RemoveIncoming(node2.point);
                }
              }
            }
          }
        }
      }
    }
  }

  public static void ResolveHangingNodes()
  {
    for (int x = MapGenerator.config.GridWidth - 2; x >= 1; --x)
    {
      for (int y = MapGenerator.config.layers.Count - 2; y >= 1; --y)
      {
        Node node = MapGenerator.GetNode(new Point(x, y));
        if (node.nodeType != NodeType.None && (node.incoming.Count == 0 || node.incoming.Count == 1 && MapGenerator.GetNode(node.incoming[0]).nodeType == NodeType.None))
        {
          foreach (Point p in node.outgoing)
            MapGenerator.GetNode(p).RemoveIncoming(node.point);
          node.outgoing.Clear();
          node.nodeType = NodeType.None;
        }
        if (node.nodeType != NodeType.None && y < MapGenerator.config.layers.Count - 1 && (node.outgoing.Count == 0 || node.outgoing.Count == 1 && MapGenerator.GetNode(node.outgoing[0]).nodeType == NodeType.None))
        {
          foreach (Point p in node.incoming)
            MapGenerator.GetNode(p).RemoveOutgoing(node.point);
          node.incoming.Clear();
          node.nodeType = NodeType.None;
        }
      }
    }
    for (int x = 0; x < MapGenerator.config.GridWidth - 1; ++x)
    {
      for (int y = 1; y < MapGenerator.config.layers.Count - 1; ++y)
      {
        Node node = MapGenerator.GetNode(new Point(x, y));
        if (node.nodeType != NodeType.None && node.outgoing.Count == 0)
        {
          Node closestNodeOfNextLayer = MapGenerator.GetClosestNodeOfNextLayer(node.point);
          node.AddOutgoing(closestNodeOfNextLayer.point);
          closestNodeOfNextLayer.AddIncoming(node.point);
        }
      }
    }
    for (int x = 0; x < MapGenerator.config.GridWidth - 1; ++x)
    {
      for (int y = 1; y < MapGenerator.config.layers.Count - 1; ++y)
      {
        Node node = MapGenerator.GetNode(new Point(x, y));
        if (node.nodeType != NodeType.None && node.incoming.Count == 0)
        {
          Node nodeOfPreviousLayer = MapGenerator.GetClosestNodeOfPreviousLayer(node.point);
          node.AddIncoming(nodeOfPreviousLayer.point);
          nodeOfPreviousLayer.AddOutgoing(node.point);
        }
      }
    }
  }

  public static Node GetNode(Point p)
  {
    if (p.y >= MapGenerator.nodes.Count)
      return (Node) null;
    return p.x >= MapGenerator.nodes[p.y].Count ? (Node) null : MapGenerator.nodes[p.y][p.x];
  }

  public static Node GetNode(int y, int x)
  {
    return y >= 0 && y < MapGenerator.nodes.Count - 1 && x >= 0 && x < MapGenerator.nodes[y].Count - 1 ? MapGenerator.nodes[y][x] : (Node) null;
  }

  public static Node GetClosestNodeOfNextLayer(Point p)
  {
    Node closestNodeOfNextLayer = (Node) null;
    foreach (Node node in MapGenerator.nodes[p.y + 1])
    {
      if (node.nodeType != NodeType.None && (closestNodeOfNextLayer == null || Mathf.Abs(node.point.x - p.x) < Mathf.Abs(closestNodeOfNextLayer.point.x - p.x)))
        closestNodeOfNextLayer = node;
    }
    return closestNodeOfNextLayer;
  }

  public static Node GetClosestNodeOfPreviousLayer(Point p)
  {
    Node nodeOfPreviousLayer = (Node) null;
    foreach (Node node in MapGenerator.nodes[p.y - 1])
    {
      if (node.nodeType != NodeType.None && (nodeOfPreviousLayer == null || Mathf.Abs(node.point.x - p.x) < Mathf.Abs(nodeOfPreviousLayer.point.x - p.x)))
        nodeOfPreviousLayer = node;
    }
    return nodeOfPreviousLayer;
  }

  public static List<Node> GetAllFutureNodes(int currentLayer)
  {
    List<Node> allFutureNodes = new List<Node>();
    for (int index = currentLayer; index < MapGenerator.nodes.Count; ++index)
      allFutureNodes.AddRange((IEnumerable<Node>) MapGenerator.nodes[index]);
    return allFutureNodes;
  }

  public static Point GetFinalNode()
  {
    int y = MapGenerator.config.layers.Count - 1;
    if (MapGenerator.config.GridWidth % 2 == 1)
      return new Point(MapGenerator.config.GridWidth / 2, y);
    return UnityEngine.Random.Range(0, 2) != 0 ? new Point(MapGenerator.config.GridWidth / 2 - 1, y) : new Point(MapGenerator.config.GridWidth / 2, y);
  }

  public static void GeneratePaths()
  {
    Point finalNode = MapGenerator.GetFinalNode();
    MapGenerator.paths = new List<List<Point>>();
    int n = MapGenerator.config.numOfStartingNodes.GetValue();
    int count = MapGenerator.config.numOfPreBossNodes.GetValue();
    List<int> intList = new List<int>();
    for (int index = 0; index < MapGenerator.config.GridWidth; ++index)
      intList.Add(index);
    intList.Shuffle<int>();
    List<Point> list = intList.Take<int>(count).Select<int, Point>((Func<int, Point>) (x => new Point(x, finalNode.y - 1))).ToList<Point>();
    int num = 0;
    foreach (Point from in list)
    {
      List<Point> pointList = MapGenerator.Path(from, 0, MapGenerator.config.GridWidth);
      if (pointList != null)
      {
        pointList.Insert(0, finalNode);
        MapGenerator.paths.Add(pointList);
      }
      ++num;
    }
    for (; !MapGenerator.PathsLeadToAtLeastNDifferentPoints((IEnumerable<List<Point>>) MapGenerator.paths, n) && num < 100; ++num)
    {
      List<Point> pointList = MapGenerator.Path(list[UnityEngine.Random.Range(0, list.Count)], 0, MapGenerator.config.GridWidth);
      pointList.Insert(0, finalNode);
      MapGenerator.paths.Add(pointList);
    }
  }

  public static bool PathsLeadToAtLeastNDifferentPoints(IEnumerable<List<Point>> paths, int n)
  {
    return paths.Select<List<Point>, int>((Func<List<Point>, int>) (path => path[path.Count - 1].x)).Distinct<int>().Count<int>() >= n;
  }

  public static List<Point> Path(Point from, int toY, int width, bool firstStepUnconstrained = false)
  {
    if (from.y == toY)
    {
      Debug.LogError((object) "Points are on same layers, return");
      return (List<Point>) null;
    }
    int num = from.y > toY ? -1 : 1;
    List<Point> pointList = new List<Point>() { from };
    while (pointList[pointList.Count - 1].y != toY)
    {
      Point point1 = pointList[pointList.Count - 1];
      List<int> intList = new List<int>();
      if (point1.y == 1)
        intList.Add(MapGenerator.config.GridWidth / 2);
      else if (firstStepUnconstrained && point1.Equals(from))
      {
        for (int index = 0; index < width; ++index)
          intList.Add(index);
      }
      else
      {
        Node node = MapGenerator.GetNode(new Point(point1.x, point1.y - 1));
        if (node != null && node.nodeType == NodeType.MiniBossFloor)
        {
          intList.Add(MapGenerator.config.GridWidth / 2);
        }
        else
        {
          intList.Add(point1.x);
          if (point1.x - 1 >= 0)
            intList.Add(point1.x - 1);
          if (point1.x + 1 < width)
            intList.Add(point1.x + 1);
        }
      }
      Point point2 = new Point(intList[UnityEngine.Random.Range(0, intList.Count)], point1.y + num);
      pointList.Add(point2);
    }
    return pointList;
  }

  public static NodeType GetRandomNode(MapConfig mapConfig)
  {
    int num = 0;
    while (num++ < 100)
    {
      NodeType RandomNode = MapGenerator.RandomNodes[UnityEngine.Random.Range(0, MapGenerator.RandomNodes.Count)];
      NodeBlueprint blueprint = MapManager.GetBlueprint(RandomNode, mapConfig);
      float probability = blueprint.Probability;
      switch (blueprint.nodeType)
      {
        case NodeType.Wood:
          if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.LOG) >= 250)
          {
            probability *= 0.5f;
            break;
          }
          break;
        case NodeType.Stone:
          if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.STONE) >= 250)
          {
            probability *= 0.5f;
            break;
          }
          break;
      }
      if ((double) UnityEngine.Random.Range(0.0f, 1f) <= (double) probability && !blueprint.HasCost)
      {
        if (blueprint.RequireCondition && !DungeonSandboxManager.Active)
        {
          bool flag = true;
          foreach (BiomeGenerator.VariableAndCondition conditionalVariable in blueprint.ConditionalVariables)
          {
            if (DataManager.Instance.GetVariable(conditionalVariable.Variable) != conditionalVariable.Condition)
              flag = false;
          }
          if (!flag)
            continue;
        }
        if ((RandomNode == NodeType.Special_Healing || RandomNode == NodeType.Special_HealthChoice) && PlayerFleeceManager.FleecePreventsHealthPickups())
          Debug.Log((object) "Node type can't be a health type with 'no health' fleece");
        else if ((RandomNode != NodeType.MarketPlaceCat || !DungeonSandboxManager.Active || !PlayerFleeceManager.FleecePreventsHealthPickups()) && (RandomNode != NodeType.Tarot || !PlayerFleeceManager.FleecePreventTarotCards()) && (RandomNode != NodeType.MarketplaceBlacksmith || DataManager.Instance.PleasureEnabled && DataManager.Instance.PalworldEggsCollected < 4 && !DataManager.Instance.PalworldSkinsGivenLocations.Contains(PlayerFarming.Location)))
        {
          MapGenerator.config.nodeBlueprints.FirstOrDefault<NodeBlueprint>((Func<NodeBlueprint, bool>) (n => n.nodeType == RandomNode));
          return RandomNode;
        }
      }
    }
    return MapGenerator.RandomNodes[UnityEngine.Random.Range(0, MapGenerator.RandomNodes.Count)];
  }

  public static Node GetRandomNodeOnLayer(int layer)
  {
    List<Node> nodeList = new List<Node>();
    if (layer >= MapGenerator.nodes.Count - 1)
    {
      nodeList.Add(MapManager.Instance.CurrentMap.GetBossNode());
    }
    else
    {
      foreach (Node node in MapGenerator.nodes[layer])
      {
        if (node.nodeType != NodeType.None)
          nodeList.Add(node);
      }
    }
    return nodeList.Count <= 0 ? MapGenerator.GetNode(MapGenerator.GetFinalNode()) : nodeList[UnityEngine.Random.Range(0, nodeList.Count)];
  }

  public static Node GetFirstNodeOnLayer(int layer)
  {
    foreach (Node firstNodeOnLayer in MapGenerator.nodes[layer])
    {
      if (firstNodeOnLayer.nodeType != NodeType.None && (layer != 0 || firstNodeOnLayer.outgoing.Count > 0))
        return firstNodeOnLayer;
    }
    return (Node) null;
  }

  public static List<Node> GetNodesOnLayer(int layer) => MapGenerator.nodes[layer];

  public static int GetNodeLayer(Node node)
  {
    for (int index = 0; index < MapGenerator.nodes.Count; ++index)
    {
      if (MapGenerator.nodes[index].Contains(node))
        return index;
    }
    return -1;
  }

  public static Node GetRandomWeightedNode(int minLayer, int maxLayer)
  {
    List<KeyValuePair<Node, float>> keyValuePairList = new List<KeyValuePair<Node, float>>();
    for (int index = minLayer; index < maxLayer; ++index)
    {
      float num = (float) (1.0 - ((double) index / (double) MapGenerator.nodes.Count - 1.0));
      foreach (Node key in MapGenerator.nodes[index])
        keyValuePairList.Add(new KeyValuePair<Node, float>(key, num));
    }
    if (keyValuePairList.Count > 0)
    {
      int num = 0;
      while (num++ < 32 /*0x20*/)
      {
        KeyValuePair<Node, float> keyValuePair = keyValuePairList[UnityEngine.Random.Range(0, keyValuePairList.Count)];
        if ((double) UnityEngine.Random.Range(0.0f, 1f) <= (double) keyValuePair.Value)
          return keyValuePair.Key;
      }
    }
    return (Node) null;
  }
}
