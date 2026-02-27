// Decompiled with JetBrains decompiler
// Type: Map.MapGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private static List<NodeType> RandomNodes;
  private static List<float> layerDistances;
  private static List<List<Point>> paths;
  private static readonly List<List<Node>> nodes = new List<List<Node>>();
  private static readonly List<Node> RewardNodes = new List<Node>();
  private static int blankNodes = 0;

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
    MapGenerator.GeneratePaths();
    MapGenerator.SetUpConnections();
    MapGenerator.RemoveCrossConnections();
    MapGenerator.ReconnectEmptyNodes();
    List<Node> list = MapGenerator.nodes.SelectMany<List<Node>, Node>((Func<List<Node>, IEnumerable<Node>>) (n => (IEnumerable<Node>) n)).Where<Node>((Func<Node, bool>) (n => n.incoming.Count > 0 || n.outgoing.Count > 0)).ToList<Node>();
    if (conf.EnsureMinimumRewards)
      MapGenerator.EnsureMinimumRewards(conf, list);
    MapGenerator.SetCombatNodes();
    MapGenerator.SetNodePositions();
    return new Map.Map(conf.name, list, new List<Point>());
  }

  private static void EnsureMinimumRewards(MapConfig conf, List<Node> NodesList)
  {
    List<Node> nodeList = new List<Node>((IEnumerable<Node>) MapGenerator.RewardNodes);
    List<NodeType> nodeTypeList = new List<NodeType>((IEnumerable<NodeType>) conf.MinimumRewards);
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      if (objective.Type == Objectives.TYPES.FIND_FOLLOWER && ((Objectives_FindFollower) objective).TargetLocation == BiomeGenerator.Instance.DungeonLocation)
      {
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
        if ((nodeBlueprint.nodeType != NodeType.Wood || Inventory.GetItemQuantity(1) <= 5) && (nodeBlueprint.nodeType != NodeType.Stone || Inventory.GetItemQuantity(2) < 5) && (nodeBlueprint.nodeType != NodeType.Food || Inventory.GetFoodAmount() <= 10))
        {
          if (nodeBlueprint.RequireCondition)
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
          while (--num > 0 && (nodeList[index].nodeType == NodeType.Wood || nodeList[index].nodeType == NodeType.Food || nodeList[index].nodeType == NodeType.Stone || nodeList[index].nodeType == NodeType.None || !NodesList.Contains(nodeList[index])))
            index = UnityEngine.Random.Range(0, nodeList.Count);
          nodeList[index].nodeType = minimumReward;
          nodeList[index].blueprint = nodeBlueprint;
          nodeList[index].Hidden = false;
          nodeList[index].CanBeHidden = false;
          nodeList.RemoveAt(index);
        }
      }
    }
  }

  private static void GenerateLayerDistances()
  {
    MapGenerator.layerDistances = new List<float>();
    foreach (MapLayer layer in MapGenerator.config.layers)
    {
      float num = layer.distanceFromPreviousLayer.GetValue();
      MapGenerator.layerDistances.Add(num);
    }
  }

  private static float GetDistanceToLayer(int layerIndex)
  {
    return layerIndex < 0 || layerIndex > MapGenerator.layerDistances.Count ? 0.0f : MapGenerator.layerDistances.Take<float>(layerIndex + 1).Sum();
  }

  private static void PlaceLayer(int layerIndex, MapConfig mapConfig)
  {
    MapLayer layer = MapGenerator.config.layers[layerIndex];
    List<Node> nodesOnThisLayer = new List<Node>();
    float offset = (float) ((double) layer.nodesApartDistance * (double) MapGenerator.config.GridWidth / 2.0);
    for (int i = 0; i < MapGenerator.config.GridWidth; ++i)
    {
      bool randomiseReward = (double) UnityEngine.Random.Range(0.0f, 1f) < (double) layer.randomizeNodes;
      Node node = !GameManager.SandboxDungeonEnabled || layerIndex == 0 || layerIndex == MapGenerator.config.layers.Count - 1 ? MapGenerator.GetNodeBasedOnLayer(layer, mapConfig, nodesOnThisLayer, layerIndex, i, offset, randomiseReward) : MapGenerator.GetCompleteRandomNode(layer, mapConfig, layerIndex, i, offset);
      nodesOnThisLayer.Add(node);
      if (randomiseReward)
        MapGenerator.RewardNodes.Add(node);
    }
    MapGenerator.nodes.Add(nodesOnThisLayer);
  }

  private static Node GetNodeBasedOnLayer(
    MapLayer layer,
    MapConfig mapConfig,
    List<Node> nodesOnThisLayer,
    int layerIndex,
    int i,
    float offset,
    bool randomiseReward)
  {
    AnimationCurve animationCurve = AnimationCurve.EaseInOut(0.0f, 1f, 1f, 0.75f);
    NodeType nodeType = NodeType.None;
    Node nodeBasedOnLayer;
    if (randomiseReward)
    {
      do
      {
        float time = (float) MapGenerator.blankNodes / (float) mapConfig.NumOfBlankNodes;
        float num = animationCurve.Evaluate(time);
        nodeType = (double) UnityEngine.Random.Range(0.0f, 1f) > 0.5 || (double) MapGenerator.blankNodes >= (double) mapConfig.NumOfBlankNodes * (double) num ? MapGenerator.GetRandomNode(mapConfig) : NodeType.None;
      }
      while (layerIndex == 1 && mapConfig.FirstLayerBlacklist.Contains<NodeType>(nodeType) || layerIndex >= mapConfig.layers.Count - 2 && mapConfig.LastLayerBlacklist.Contains<NodeType>(nodeType) || nodesOnThisLayer.FirstOrDefault<Node>((Func<Node, bool>) (x => x.nodeType != NodeType.None)) == null && nodeType == NodeType.None);
      string blueprintName = MapGenerator.config.nodeBlueprints.Where<NodeBlueprint>((Func<NodeBlueprint, bool>) (b => b.nodeType == nodeType)).ToList<NodeBlueprint>().Random<NodeBlueprint>().name;
      NodeBlueprint blueprint = MapGenerator.config.nodeBlueprints.FirstOrDefault<NodeBlueprint>((Func<NodeBlueprint, bool>) (n => n.name == blueprintName));
      if (blueprint.OnlyOne)
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
      nodeType = layer.nodeType;
      nodeBasedOnLayer = new Node(nodeType, layer.BluePrint, new Point(i, layerIndex))
      {
        position = new Vector2((float) (-(double) offset + (double) i * (double) layer.nodesApartDistance), MapGenerator.GetDistanceToLayer(layerIndex))
      };
    }
    return nodeBasedOnLayer;
  }

  private static Node GetCompleteRandomNode(
    MapLayer layer,
    MapConfig mapConfig,
    int layerIndex,
    int i,
    float offset)
  {
    FollowerLocation location = FollowerLocation.None;
    NodeType node;
    if ((double) UnityEngine.Random.Range(0.0f, 1f) < 0.5)
    {
      node = NodeType.DungeonFloor;
      if (GameManager.SandboxDungeonEnabled)
      {
        int num = UnityEngine.Random.Range(0, 4);
        if (num == 0)
          location = FollowerLocation.Dungeon1_1;
        if (num == 1)
          location = FollowerLocation.Dungeon1_2;
        if (num == 2)
          location = FollowerLocation.Dungeon1_3;
        if (num == 3)
          location = FollowerLocation.Dungeon1_4;
      }
    }
    else
      node = MapGenerator.GetRandomNode(mapConfig);
    if ((double) UnityEngine.Random.Range(0.0f, 1f) > 0.75)
      node = NodeType.None;
    if (node == NodeType.DungeonFloor)
    {
      if ((double) UnityEngine.Random.Range(0.0f, 1f) < 0.10000000149011612)
        node = NodeType.MiniBossFloor;
      return new Node(node, node == NodeType.DungeonFloor ? mapConfig.SecondFloorBluePrint : mapConfig.MiniBossFloorBluePrint, new Point(i, layerIndex), location)
      {
        position = new Vector2((float) (-(double) offset + (double) i * (double) layer.nodesApartDistance), MapGenerator.GetDistanceToLayer(layerIndex))
      };
    }
    string blueprintName = MapGenerator.config.nodeBlueprints.Where<NodeBlueprint>((Func<NodeBlueprint, bool>) (b => b.nodeType == node)).ToList<NodeBlueprint>().Random<NodeBlueprint>().name;
    NodeBlueprint blueprint = MapGenerator.config.nodeBlueprints.FirstOrDefault<NodeBlueprint>((Func<NodeBlueprint, bool>) (n => n.name == blueprintName));
    return new Node(node, blueprint, new Point(i, layerIndex), location)
    {
      position = new Vector2((float) (-(double) offset + (double) i * (double) layer.nodesApartDistance), MapGenerator.GetDistanceToLayer(layerIndex))
    };
  }

  private static void SetNodePositions()
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

  private static void SetUpConnections()
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

  private static void ReconnectEmptyNodes()
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

  private static void SetCombatNodes()
  {
    AnimationCurve animationCurve = AnimationCurve.EaseInOut(0.0f, 1f, 1f, 0.75f);
    int num1 = GameManager.CurrentDungeonLayer - 1;
    int num2 = Mathf.RoundToInt((float) MapManager.Instance.DungeonConfig.NumOfCombatNodes * ((float) num1 * animationCurve.Evaluate((float) num1 / 3f)));
    int num3 = 0;
    for (int index1 = 0; index1 < 50; ++index1)
    {
      for (int index2 = 0; index2 < MapGenerator.nodes.Count; ++index2)
      {
        for (int index3 = 0; index3 < MapGenerator.nodes[index2].Count; ++index3)
        {
          if (MapGenerator.nodes[index2][index3].outgoing.Count > 0 && MapGenerator.nodes[index2][index3].incoming.Count > 0 && MapGenerator.nodes[index2][index3].outgoing.Where<Point>((Func<Point, bool>) (node => MapGenerator.GetNode(node).nodeType == NodeType.DungeonFloor)).FirstOrDefault<Point>() == null && MapGenerator.nodes[index2][index3].incoming.Where<Point>((Func<Point, bool>) (node => MapGenerator.GetNode(node).nodeType == NodeType.DungeonFloor)).FirstOrDefault<Point>() == null && (double) UnityEngine.Random.value > 0.75 && index2 > 1 && MapGenerator.nodes[index2][index3].nodeType != NodeType.Follower_Beginner && MapGenerator.nodes[index2][index3].nodeType != NodeType.Follower_Easy && MapGenerator.nodes[index2][index3].nodeType != NodeType.Follower_Medium && MapGenerator.nodes[index2][index3].nodeType != NodeType.Follower_Hard && MapGenerator.nodes[index2][index3].nodeType != NodeType.Negative_PreviousMiniboss)
          {
            MapGenerator.nodes[index2][index3].nodeType = NodeType.DungeonFloor;
            MapGenerator.nodes[index2][index3].blueprint = MapManager.Instance.DungeonConfig.SecondFloorBluePrint;
            MapGenerator.nodes[index2][index3].Hidden = false;
            MapGenerator.nodes[index2][index3].Modifier = DungeonModifier.GetModifier();
          }
        }
      }
      if (num3 >= num2)
        break;
    }
  }

  private static void RemoveCrossConnections()
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

  private static void ResolveHangingNodes()
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

  private static Node GetNode(Point p)
  {
    if (p.y >= MapGenerator.nodes.Count)
      return (Node) null;
    return p.x >= MapGenerator.nodes[p.y].Count ? (Node) null : MapGenerator.nodes[p.y][p.x];
  }

  private static Node GetNode(int y, int x)
  {
    return y >= 0 && y < MapGenerator.nodes.Count - 1 && x >= 0 && x < MapGenerator.nodes[y].Count - 1 ? MapGenerator.nodes[y][x] : (Node) null;
  }

  private static Node GetClosestNodeOfNextLayer(Point p)
  {
    Node closestNodeOfNextLayer = (Node) null;
    foreach (Node node in MapGenerator.nodes[p.y + 1])
    {
      if (node.nodeType != NodeType.None && (closestNodeOfNextLayer == null || Mathf.Abs(node.point.x - p.x) < Mathf.Abs(closestNodeOfNextLayer.point.x - p.x)))
        closestNodeOfNextLayer = node;
    }
    return closestNodeOfNextLayer;
  }

  private static Node GetClosestNodeOfPreviousLayer(Point p)
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

  private static Point GetFinalNode()
  {
    int y = MapGenerator.config.layers.Count - 1;
    if (MapGenerator.config.GridWidth % 2 == 1)
      return new Point(MapGenerator.config.GridWidth / 2, y);
    return UnityEngine.Random.Range(0, 2) != 0 ? new Point(MapGenerator.config.GridWidth / 2 - 1, y) : new Point(MapGenerator.config.GridWidth / 2, y);
  }

  private static void GeneratePaths()
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
      pointList.Insert(0, finalNode);
      MapGenerator.paths.Add(pointList);
      ++num;
    }
    for (; !MapGenerator.PathsLeadToAtLeastNDifferentPoints((IEnumerable<List<Point>>) MapGenerator.paths, n) && num < 100; ++num)
    {
      List<Point> pointList = MapGenerator.Path(list[UnityEngine.Random.Range(0, list.Count)], 0, MapGenerator.config.GridWidth);
      pointList.Insert(0, finalNode);
      MapGenerator.paths.Add(pointList);
    }
  }

  private static bool PathsLeadToAtLeastNDifferentPoints(IEnumerable<List<Point>> paths, int n)
  {
    return paths.Select<List<Point>, int>((Func<List<Point>, int>) (path => path[path.Count - 1].x)).Distinct<int>().Count<int>() >= n;
  }

  private static List<Point> Path(Point from, int toY, int width, bool firstStepUnconstrained = false)
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
      if ((double) UnityEngine.Random.Range(0.0f, 1f) <= (double) blueprint.Probability)
      {
        if (blueprint.RequireCondition)
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
        MapGenerator.config.nodeBlueprints.FirstOrDefault<NodeBlueprint>((Func<NodeBlueprint, bool>) (n => n.nodeType == RandomNode));
        return RandomNode;
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
      if (firstNodeOnLayer.nodeType != NodeType.None)
        return firstNodeOnLayer;
    }
    return (Node) null;
  }

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
