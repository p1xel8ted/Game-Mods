// Decompiled with JetBrains decompiler
// Type: Map.Map
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Map;

public class Map
{
  public List<Node> nodes;
  public List<Point> path;
  public string configName;

  public Map(string configName, List<Node> nodes, List<Point> path)
  {
    this.configName = configName;
    this.nodes = nodes;
    this.path = path;
  }

  public Node GetBossNode()
  {
    return this.nodes.LastOrDefault<Node>((Func<Node, bool>) (n => n.nodeType == NodeType.MiniBossFloor));
  }

  public Node GetLeaderNode()
  {
    return this.nodes.LastOrDefault<Node>((Func<Node, bool>) (n => n.nodeType == NodeType.Boss));
  }

  public Node GetFinalBossNode()
  {
    return this.nodes.LastOrDefault<Node>((Func<Node, bool>) (n => n.nodeType == NodeType.FinalBoss)) ?? this.nodes.LastOrDefault<Node>((Func<Node, bool>) (n => n.nodeType == NodeType.Boss)) ?? this.nodes.LastOrDefault<Node>((Func<Node, bool>) (n => n.nodeType == NodeType.MiniBossFloor));
  }

  public Node GetCurrentNode()
  {
    return this.path.Count > 0 ? this.GetNode(this.path.LastElement<Point>()) : (Node) null;
  }

  public float DistanceBetweenFirstAndLastLayers()
  {
    Node bossNode = this.GetBossNode();
    Node node = this.nodes.FirstOrDefault<Node>((Func<Node, bool>) (n => n.point.y == 0));
    return bossNode == null || node == null ? 0.0f : bossNode.position.y - node.position.y;
  }

  public Node GetFirstNode()
  {
    return this.nodes.First<Node>((Func<Node, bool>) (n => n.point.y.Equals(0)));
  }

  public Node GetNode(Point point)
  {
    return this.nodes.FirstOrDefault<Node>((Func<Node, bool>) (n => n.point.Equals(point)));
  }

  public Node GetNextNode(Point currentPoint)
  {
    Node node = this.GetNode(currentPoint);
    return node.outgoing.Count > 0 ? this.GetNode(node.outgoing[UnityEngine.Random.Range(0, node.outgoing.Count)]) : node;
  }
}
