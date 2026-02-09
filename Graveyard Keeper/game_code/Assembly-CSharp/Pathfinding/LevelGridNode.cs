// Decompiled with JetBrains decompiler
// Type: Pathfinding.LevelGridNode
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding.Serialization;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Pathfinding;

public class LevelGridNode(AstarPath astar) : GridNodeBase(astar)
{
  public static LayerGridGraph[] _gridGraphs = new LayerGridGraph[0];
  public uint gridConnections;
  public static LayerGridGraph[] gridGraphs;
  public const int NoConnection = 255 /*0xFF*/;
  public const int ConnectionMask = 255 /*0xFF*/;
  public const int ConnectionStride = 8;
  public const int MaxLayerCount = 255 /*0xFF*/;

  public static LayerGridGraph GetGridGraph(uint graphIndex)
  {
    return LevelGridNode._gridGraphs[(int) graphIndex];
  }

  public static void SetGridGraph(int graphIndex, LayerGridGraph graph)
  {
    if (LevelGridNode._gridGraphs.Length <= graphIndex)
    {
      LayerGridGraph[] layerGridGraphArray = new LayerGridGraph[graphIndex + 1];
      for (int index = 0; index < LevelGridNode._gridGraphs.Length; ++index)
        layerGridGraphArray[index] = LevelGridNode._gridGraphs[index];
      LevelGridNode._gridGraphs = layerGridGraphArray;
    }
    LevelGridNode._gridGraphs[graphIndex] = graph;
  }

  public void ResetAllGridConnections() => this.gridConnections = uint.MaxValue;

  public bool HasAnyGridConnections() => this.gridConnections != uint.MaxValue;

  public void SetPosition(Int3 position) => this.position = position;

  public override void ClearConnections(bool alsoReverse)
  {
    if (alsoReverse)
    {
      LayerGridGraph gridGraph = LevelGridNode.GetGridGraph(this.GraphIndex);
      int[] neighbourOffsets = gridGraph.neighbourOffsets;
      LevelGridNode[] nodes = gridGraph.nodes;
      for (int dir = 0; dir < 4; ++dir)
      {
        int connectionValue = this.GetConnectionValue(dir);
        if (connectionValue != (int) byte.MaxValue)
          nodes[this.NodeInGridIndex + neighbourOffsets[dir] + gridGraph.lastScannedWidth * gridGraph.lastScannedDepth * connectionValue]?.SetConnectionValue((dir + 2) % 4, (int) byte.MaxValue);
      }
    }
    this.ResetAllGridConnections();
    base.ClearConnections(alsoReverse);
  }

  public override void GetConnections(GraphNodeDelegate del)
  {
    int nodeInGridIndex = this.NodeInGridIndex;
    LayerGridGraph gridGraph = LevelGridNode.GetGridGraph(this.GraphIndex);
    int[] neighbourOffsets = gridGraph.neighbourOffsets;
    LevelGridNode[] nodes = gridGraph.nodes;
    for (int dir = 0; dir < 4; ++dir)
    {
      int connectionValue = this.GetConnectionValue(dir);
      if (connectionValue != (int) byte.MaxValue)
      {
        LevelGridNode node = nodes[nodeInGridIndex + neighbourOffsets[dir] + gridGraph.lastScannedWidth * gridGraph.lastScannedDepth * connectionValue];
        if (node != null)
          del((GraphNode) node);
      }
    }
    base.GetConnections(del);
  }

  public override void FloodFill(Stack<GraphNode> stack, uint region)
  {
    int nodeInGridIndex = this.NodeInGridIndex;
    LayerGridGraph gridGraph = LevelGridNode.GetGridGraph(this.GraphIndex);
    int[] neighbourOffsets = gridGraph.neighbourOffsets;
    LevelGridNode[] nodes = gridGraph.nodes;
    for (int dir = 0; dir < 4; ++dir)
    {
      int connectionValue = this.GetConnectionValue(dir);
      if (connectionValue != (int) byte.MaxValue)
      {
        LevelGridNode levelGridNode = nodes[nodeInGridIndex + neighbourOffsets[dir] + gridGraph.lastScannedWidth * gridGraph.lastScannedDepth * connectionValue];
        if (levelGridNode != null && (int) levelGridNode.Area != (int) region)
        {
          levelGridNode.Area = region;
          stack.Push((GraphNode) levelGridNode);
        }
      }
    }
    base.FloodFill(stack, region);
  }

  public bool GetConnection(int i)
  {
    return ((int) (this.gridConnections >> i * 8) & (int) byte.MaxValue) != (int) byte.MaxValue;
  }

  public void SetConnectionValue(int dir, int value)
  {
    this.gridConnections = (uint) ((int) this.gridConnections & ~((int) byte.MaxValue << dir * 8) | value << dir * 8);
  }

  public int GetConnectionValue(int dir)
  {
    return (int) (this.gridConnections >> dir * 8) & (int) byte.MaxValue;
  }

  public override bool GetPortal(
    GraphNode other,
    List<Vector3> left,
    List<Vector3> right,
    bool backwards)
  {
    if (backwards)
      return true;
    LayerGridGraph gridGraph = LevelGridNode.GetGridGraph(this.GraphIndex);
    int[] neighbourOffsets = gridGraph.neighbourOffsets;
    LevelGridNode[] nodes = gridGraph.nodes;
    int nodeInGridIndex = this.NodeInGridIndex;
    for (int dir = 0; dir < 4; ++dir)
    {
      int connectionValue = this.GetConnectionValue(dir);
      if (connectionValue != (int) byte.MaxValue && other == nodes[nodeInGridIndex + neighbourOffsets[dir] + gridGraph.lastScannedWidth * gridGraph.lastScannedDepth * connectionValue])
      {
        Vector3 vector3_1 = (Vector3) (this.position + other.position) * 0.5f;
        Vector3 vector3_2 = Vector3.Cross(gridGraph.collision.up, (Vector3) (other.position - this.position));
        vector3_2.Normalize();
        Vector3 vector3_3 = vector3_2 * (gridGraph.nodeSize * 0.5f);
        left.Add(vector3_1 - vector3_3);
        right.Add(vector3_1 + vector3_3);
        return true;
      }
    }
    return false;
  }

  public override void UpdateRecursiveG(Path path, PathNode pathNode, PathHandler handler)
  {
    handler.PushNode(pathNode);
    this.UpdateG(path, pathNode);
    LayerGridGraph gridGraph = LevelGridNode.GetGridGraph(this.GraphIndex);
    int[] neighbourOffsets = gridGraph.neighbourOffsets;
    LevelGridNode[] nodes = gridGraph.nodes;
    int nodeInGridIndex = this.NodeInGridIndex;
    for (int dir = 0; dir < 4; ++dir)
    {
      int connectionValue = this.GetConnectionValue(dir);
      if (connectionValue != (int) byte.MaxValue)
      {
        LevelGridNode node = nodes[nodeInGridIndex + neighbourOffsets[dir] + gridGraph.lastScannedWidth * gridGraph.lastScannedDepth * connectionValue];
        PathNode pathNode1 = handler.GetPathNode((GraphNode) node);
        if (pathNode1 != null && pathNode1.parent == pathNode && (int) pathNode1.pathID == (int) handler.PathID)
          node.UpdateRecursiveG(path, pathNode1, handler);
      }
    }
    base.UpdateRecursiveG(path, pathNode, handler);
  }

  public override void Open(Path path, PathNode pathNode, PathHandler handler)
  {
    LayerGridGraph gridGraph = LevelGridNode.GetGridGraph(this.GraphIndex);
    int[] neighbourOffsets = gridGraph.neighbourOffsets;
    uint[] neighbourCosts = gridGraph.neighbourCosts;
    LevelGridNode[] nodes = gridGraph.nodes;
    int nodeInGridIndex = this.NodeInGridIndex;
    for (int dir = 0; dir < 4; ++dir)
    {
      int connectionValue = this.GetConnectionValue(dir);
      if (connectionValue != (int) byte.MaxValue)
      {
        GraphNode node = (GraphNode) nodes[nodeInGridIndex + neighbourOffsets[dir] + gridGraph.lastScannedWidth * gridGraph.lastScannedDepth * connectionValue];
        if (path.CanTraverse(node))
        {
          PathNode pathNode1 = handler.GetPathNode(node);
          if ((int) pathNode1.pathID != (int) handler.PathID)
          {
            pathNode1.parent = pathNode;
            pathNode1.pathID = handler.PathID;
            pathNode1.cost = neighbourCosts[dir];
            pathNode1.H = path.CalculateHScore(node);
            node.UpdateG(path, pathNode1);
            handler.PushNode(pathNode1);
          }
          else
          {
            uint num = neighbourCosts[dir];
            if (pathNode.G + num + path.GetTraversalCost(node) < pathNode1.G)
            {
              pathNode1.cost = num;
              pathNode1.parent = pathNode;
              node.UpdateRecursiveG(path, pathNode1, handler);
            }
            else if (pathNode1.G + num + path.GetTraversalCost((GraphNode) this) < pathNode.G)
            {
              pathNode.parent = pathNode1;
              pathNode.cost = num;
              this.UpdateRecursiveG(path, pathNode, handler);
            }
          }
        }
      }
    }
    base.Open(path, pathNode, handler);
  }

  public override void SerializeNode(GraphSerializationContext ctx)
  {
    base.SerializeNode(ctx);
    ctx.SerializeInt3(this.position);
    ctx.writer.Write(this.gridFlags);
    ctx.writer.Write(this.gridConnections);
  }

  public override void DeserializeNode(GraphSerializationContext ctx)
  {
    base.DeserializeNode(ctx);
    this.position = ctx.DeserializeInt3();
    this.gridFlags = ctx.reader.ReadUInt16();
    this.gridConnections = ctx.reader.ReadUInt32();
  }
}
