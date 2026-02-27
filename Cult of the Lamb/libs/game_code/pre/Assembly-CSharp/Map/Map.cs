// Decompiled with JetBrains decompiler
// Type: Map.Map
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
