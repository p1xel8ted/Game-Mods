// Decompiled with JetBrains decompiler
// Type: Pathfinding.GridNode
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding.Serialization;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Pathfinding;

public class GridNode(AstarPath astar) : GridNodeBase(astar)
{
  public static GridGraph[] _gridGraphs = new GridGraph[0];
  public const int GridFlagsConnectionOffset = 0;
  public const int GridFlagsConnectionBit0 = 1;
  public const int GridFlagsConnectionMask = 255 /*0xFF*/;
  public const int GridFlagsEdgeNodeOffset = 10;
  public const int GridFlagsEdgeNodeMask = 1024 /*0x0400*/;

  public static GridGraph GetGridGraph(uint graphIndex) => GridNode._gridGraphs[(int) graphIndex];

  public static void SetGridGraph(int graphIndex, GridGraph graph)
  {
    if (GridNode._gridGraphs.Length <= graphIndex)
    {
      GridGraph[] gridGraphArray = new GridGraph[graphIndex + 1];
      for (int index = 0; index < GridNode._gridGraphs.Length; ++index)
        gridGraphArray[index] = GridNode._gridGraphs[index];
      GridNode._gridGraphs = gridGraphArray;
    }
    GridNode._gridGraphs[graphIndex] = graph;
  }

  public ushort InternalGridFlags
  {
    get => this.gridFlags;
    set => this.gridFlags = value;
  }

  public bool GetConnectionInternal(int dir) => ((int) this.gridFlags >> dir & 1) != 0;

  public void SetConnectionInternal(int dir, bool value)
  {
    this.gridFlags = (ushort) ((int) this.gridFlags & ~(1 << dir) | (value ? 1 : 0) << dir);
  }

  public void SetAllConnectionInternal(int connections)
  {
    this.gridFlags = (ushort) ((int) this.gridFlags & -256 | connections);
  }

  public void ResetConnectionsInternal() => this.gridFlags &= (ushort) 65280;

  public bool EdgeNode
  {
    get => ((uint) this.gridFlags & 1024U /*0x0400*/) > 0U;
    set => this.gridFlags = (ushort) ((int) this.gridFlags & -1025 | (value ? 1024 /*0x0400*/ : 0));
  }

  public int XCoordinateInGrid
  {
    get => this.nodeInGridIndex % GridNode.GetGridGraph(this.GraphIndex).width;
  }

  public int ZCoordinateInGrid
  {
    get => this.nodeInGridIndex / GridNode.GetGridGraph(this.GraphIndex).width;
  }

  public override void ClearConnections(bool alsoReverse)
  {
    if (alsoReverse)
    {
      GridGraph gridGraph = GridNode.GetGridGraph(this.GraphIndex);
      for (int dir = 0; dir < 8; ++dir)
        gridGraph.GetNodeConnection(this, dir)?.SetConnectionInternal(dir < 4 ? (dir + 2) % 4 : 7, false);
    }
    this.ResetConnectionsInternal();
    base.ClearConnections(alsoReverse);
  }

  public override void GetConnections(GraphNodeDelegate del)
  {
    GridGraph gridGraph = GridNode.GetGridGraph(this.GraphIndex);
    int[] neighbourOffsets = gridGraph.neighbourOffsets;
    GridNode[] nodes = gridGraph.nodes;
    for (int dir = 0; dir < 8; ++dir)
    {
      if (this.GetConnectionInternal(dir))
      {
        GridNode node = nodes[this.nodeInGridIndex + neighbourOffsets[dir]];
        if (node != null)
          del((GraphNode) node);
      }
    }
    base.GetConnections(del);
  }

  public Vector3 ClosestPointOnNode(Vector3 p)
  {
    GridGraph gridGraph = GridNode.GetGridGraph(this.GraphIndex);
    p = gridGraph.inverseMatrix.MultiplyPoint3x4(p);
    float num1 = (float) this.position.x - 0.5f;
    float num2 = (float) this.position.z - 0.5f;
    int num3 = this.nodeInGridIndex % gridGraph.width;
    int num4 = this.nodeInGridIndex / gridGraph.width;
    float y = gridGraph.inverseMatrix.MultiplyPoint3x4(p).y;
    Vector3 point = new Vector3(Mathf.Clamp(num1, (float) num3 - 0.5f, (float) num3 + 0.5f) + 0.5f, y, Mathf.Clamp(num2, (float) num4 - 0.5f, (float) num4 + 0.5f) + 0.5f);
    return gridGraph.matrix.MultiplyPoint3x4(point);
  }

  public override bool GetPortal(
    GraphNode other,
    List<Vector3> left,
    List<Vector3> right,
    bool backwards)
  {
    if (backwards)
      return true;
    GridGraph gridGraph = GridNode.GetGridGraph(this.GraphIndex);
    int[] neighbourOffsets = gridGraph.neighbourOffsets;
    GridNode[] nodes = gridGraph.nodes;
    for (int dir = 0; dir < 4; ++dir)
    {
      if (this.GetConnectionInternal(dir) && other == nodes[this.nodeInGridIndex + neighbourOffsets[dir]])
      {
        Vector3 vector3_1 = (Vector3) (this.position + other.position) * 0.5f;
        Vector3 vector3_2 = Vector3.Cross(gridGraph.collision.up, (Vector3) (other.position - this.position));
        vector3_2.Normalize();
        vector3_2 *= gridGraph.nodeSize * 0.5f;
        left.Add(vector3_1 - vector3_2);
        right.Add(vector3_1 + vector3_2);
        return true;
      }
    }
    for (int dir = 4; dir < 8; ++dir)
    {
      if (this.GetConnectionInternal(dir) && other == nodes[this.nodeInGridIndex + neighbourOffsets[dir]])
      {
        bool flag1 = false;
        bool flag2 = false;
        if (this.GetConnectionInternal(dir - 4))
        {
          GridNode gridNode = nodes[this.nodeInGridIndex + neighbourOffsets[dir - 4]];
          if (gridNode.Walkable && gridNode.GetConnectionInternal((dir - 4 + 1) % 4))
            flag1 = true;
        }
        if (this.GetConnectionInternal((dir - 4 + 1) % 4))
        {
          GridNode gridNode = nodes[this.nodeInGridIndex + neighbourOffsets[(dir - 4 + 1) % 4]];
          if (gridNode.Walkable && gridNode.GetConnectionInternal(dir - 4))
            flag2 = true;
        }
        Vector3 vector3_3 = (Vector3) (this.position + other.position) * 0.5f;
        Vector3 vector3_4 = Vector3.Cross(gridGraph.collision.up, (Vector3) (other.position - this.position));
        vector3_4.Normalize();
        vector3_4 *= gridGraph.nodeSize * 1.4142f;
        left.Add(vector3_3 - (flag2 ? vector3_4 : Vector3.zero));
        right.Add(vector3_3 + (flag1 ? vector3_4 : Vector3.zero));
        return true;
      }
    }
    return false;
  }

  public override void FloodFill(Stack<GraphNode> stack, uint region)
  {
    GridGraph gridGraph = GridNode.GetGridGraph(this.GraphIndex);
    int[] neighbourOffsets = gridGraph.neighbourOffsets;
    GridNode[] nodes = gridGraph.nodes;
    for (int dir = 0; dir < 8; ++dir)
    {
      if (this.GetConnectionInternal(dir))
      {
        GridNode gridNode = nodes[this.nodeInGridIndex + neighbourOffsets[dir]];
        if (gridNode != null && (int) gridNode.Area != (int) region)
        {
          gridNode.Area = region;
          stack.Push((GraphNode) gridNode);
        }
      }
    }
    base.FloodFill(stack, region);
  }

  public override void UpdateRecursiveG(Path path, PathNode pathNode, PathHandler handler)
  {
    GridGraph gridGraph = GridNode.GetGridGraph(this.GraphIndex);
    int[] neighbourOffsets = gridGraph.neighbourOffsets;
    GridNode[] nodes = gridGraph.nodes;
    this.UpdateG(path, pathNode);
    handler.PushNode(pathNode);
    ushort pathId = handler.PathID;
    for (int dir = 0; dir < 8; ++dir)
    {
      if (this.GetConnectionInternal(dir))
      {
        GridNode node = nodes[this.nodeInGridIndex + neighbourOffsets[dir]];
        PathNode pathNode1 = handler.GetPathNode((GraphNode) node);
        if (pathNode1.parent == pathNode && (int) pathNode1.pathID == (int) pathId)
          node.UpdateRecursiveG(path, pathNode1, handler);
      }
    }
    base.UpdateRecursiveG(path, pathNode, handler);
  }

  public override void Open(Path path, PathNode pathNode, PathHandler handler)
  {
    GridGraph gridGraph = GridNode.GetGridGraph(this.GraphIndex);
    ushort pathId = handler.PathID;
    int[] neighbourOffsets = gridGraph.neighbourOffsets;
    uint[] neighbourCosts = gridGraph.neighbourCosts;
    GridNode[] nodes = gridGraph.nodes;
    for (int dir = 0; dir < 8; ++dir)
    {
      if (this.GetConnectionInternal(dir))
      {
        GridNode node = nodes[this.nodeInGridIndex + neighbourOffsets[dir]];
        if (path.CanTraverse((GraphNode) node))
        {
          PathNode pathNode1 = handler.GetPathNode((GraphNode) node);
          uint num = neighbourCosts[dir];
          if ((int) pathNode1.pathID != (int) pathId)
          {
            pathNode1.parent = pathNode;
            pathNode1.pathID = pathId;
            pathNode1.cost = num;
            pathNode1.H = path.CalculateHScore((GraphNode) node);
            node.UpdateG(path, pathNode1);
            handler.PushNode(pathNode1);
          }
          else if (pathNode.G + num + path.GetTraversalCost((GraphNode) node) < pathNode1.G)
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
    base.Open(path, pathNode, handler);
  }

  public override void SerializeNode(GraphSerializationContext ctx)
  {
    base.SerializeNode(ctx);
    ctx.SerializeInt3(this.position);
    ctx.writer.Write(this.gridFlags);
  }

  public override void DeserializeNode(GraphSerializationContext ctx)
  {
    base.DeserializeNode(ctx);
    this.position = ctx.DeserializeInt3();
    this.gridFlags = ctx.reader.ReadUInt16();
  }
}
