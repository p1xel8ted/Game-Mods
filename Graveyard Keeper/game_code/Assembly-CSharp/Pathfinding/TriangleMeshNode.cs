// Decompiled with JetBrains decompiler
// Type: Pathfinding.TriangleMeshNode
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Pathfinding;

public class TriangleMeshNode(AstarPath astar) : MeshNode(astar)
{
  public int v0;
  public int v1;
  public int v2;
  public static INavmeshHolder[] _navmeshHolders = new INavmeshHolder[0];

  public static INavmeshHolder GetNavmeshHolder(uint graphIndex)
  {
    return TriangleMeshNode._navmeshHolders[(int) graphIndex];
  }

  public static void SetNavmeshHolder(int graphIndex, INavmeshHolder graph)
  {
    if (TriangleMeshNode._navmeshHolders.Length <= graphIndex)
    {
      INavmeshHolder[] navmeshHolderArray = new INavmeshHolder[graphIndex + 1];
      for (int index = 0; index < TriangleMeshNode._navmeshHolders.Length; ++index)
        navmeshHolderArray[index] = TriangleMeshNode._navmeshHolders[index];
      TriangleMeshNode._navmeshHolders = navmeshHolderArray;
    }
    TriangleMeshNode._navmeshHolders[graphIndex] = graph;
  }

  public void UpdatePositionFromVertices()
  {
    INavmeshHolder navmeshHolder = TriangleMeshNode.GetNavmeshHolder(this.GraphIndex);
    this.position = (navmeshHolder.GetVertex(this.v0) + navmeshHolder.GetVertex(this.v1) + navmeshHolder.GetVertex(this.v2)) * 0.333333f;
  }

  public int GetVertexIndex(int i)
  {
    if (i == 0)
      return this.v0;
    return i != 1 ? this.v2 : this.v1;
  }

  public int GetVertexArrayIndex(int i)
  {
    INavmeshHolder navmeshHolder = TriangleMeshNode.GetNavmeshHolder(this.GraphIndex);
    int index;
    switch (i)
    {
      case 0:
        index = this.v0;
        break;
      case 1:
        index = this.v1;
        break;
      default:
        index = this.v2;
        break;
    }
    return navmeshHolder.GetVertexArrayIndex(index);
  }

  public override Int3 GetVertex(int i)
  {
    return TriangleMeshNode.GetNavmeshHolder(this.GraphIndex).GetVertex(this.GetVertexIndex(i));
  }

  public override int GetVertexCount() => 3;

  public override Vector3 ClosestPointOnNode(Vector3 p)
  {
    INavmeshHolder navmeshHolder = TriangleMeshNode.GetNavmeshHolder(this.GraphIndex);
    return Polygon.ClosestPointOnTriangle((Vector3) navmeshHolder.GetVertex(this.v0), (Vector3) navmeshHolder.GetVertex(this.v1), (Vector3) navmeshHolder.GetVertex(this.v2), p);
  }

  public override Vector3 ClosestPointOnNodeXZ(Vector3 p)
  {
    INavmeshHolder navmeshHolder = TriangleMeshNode.GetNavmeshHolder(this.GraphIndex);
    Int3 vertex1 = navmeshHolder.GetVertex(this.v0);
    Int3 vertex2 = navmeshHolder.GetVertex(this.v1);
    Int3 vertex3 = navmeshHolder.GetVertex(this.v2);
    Vector2 vector2 = Polygon.ClosestPointOnTriangle(new Vector2((float) vertex1.x * (1f / 1000f), (float) vertex1.z * (1f / 1000f)), new Vector2((float) vertex2.x * (1f / 1000f), (float) vertex2.z * (1f / 1000f)), new Vector2((float) vertex3.x * (1f / 1000f), (float) vertex3.z * (1f / 1000f)), new Vector2(p.x, p.z));
    return new Vector3(vector2.x, p.y, vector2.y);
  }

  public override bool ContainsPoint(Int3 p)
  {
    INavmeshHolder navmeshHolder = TriangleMeshNode.GetNavmeshHolder(this.GraphIndex);
    Int3 vertex1 = navmeshHolder.GetVertex(this.v0);
    Int3 vertex2 = navmeshHolder.GetVertex(this.v1);
    Int3 vertex3 = navmeshHolder.GetVertex(this.v2);
    return (long) (vertex2.x - vertex1.x) * (long) (p.z - vertex1.z) - (long) (p.x - vertex1.x) * (long) (vertex2.z - vertex1.z) <= 0L && (long) (vertex3.x - vertex2.x) * (long) (p.z - vertex2.z) - (long) (p.x - vertex2.x) * (long) (vertex3.z - vertex2.z) <= 0L && (long) (vertex1.x - vertex3.x) * (long) (p.z - vertex3.z) - (long) (p.x - vertex3.x) * (long) (vertex1.z - vertex3.z) <= 0L;
  }

  public override void UpdateRecursiveG(Path path, PathNode pathNode, PathHandler handler)
  {
    this.UpdateG(path, pathNode);
    handler.PushNode(pathNode);
    if (this.connections == null)
      return;
    for (int index = 0; index < this.connections.Length; ++index)
    {
      GraphNode connection = this.connections[index];
      PathNode pathNode1 = handler.GetPathNode(connection);
      if (pathNode1.parent == pathNode && (int) pathNode1.pathID == (int) handler.PathID)
        connection.UpdateRecursiveG(path, pathNode1, handler);
    }
  }

  public override void Open(Path path, PathNode pathNode, PathHandler handler)
  {
    if (this.connections == null)
      return;
    bool flag2 = pathNode.flag2;
    for (int index = this.connections.Length - 1; index >= 0; --index)
    {
      GraphNode connection = this.connections[index];
      if (path.CanTraverse(connection))
      {
        PathNode pathNode1 = handler.GetPathNode(connection);
        if (pathNode1 != pathNode.parent)
        {
          uint currentCost = this.connectionCosts[index];
          if (flag2 || pathNode1.flag2)
            currentCost = path.GetConnectionSpecialCost((GraphNode) this, connection, currentCost);
          if ((int) pathNode1.pathID != (int) handler.PathID)
          {
            pathNode1.node = connection;
            pathNode1.parent = pathNode;
            pathNode1.pathID = handler.PathID;
            pathNode1.cost = currentCost;
            pathNode1.H = path.CalculateHScore(connection);
            connection.UpdateG(path, pathNode1);
            handler.PushNode(pathNode1);
          }
          else if (pathNode.G + currentCost + path.GetTraversalCost(connection) < pathNode1.G)
          {
            pathNode1.cost = currentCost;
            pathNode1.parent = pathNode;
            connection.UpdateRecursiveG(path, pathNode1, handler);
          }
          else if (pathNode1.G + currentCost + path.GetTraversalCost((GraphNode) this) < pathNode.G && connection.ContainsConnection((GraphNode) this))
          {
            pathNode.parent = pathNode1;
            pathNode.cost = currentCost;
            this.UpdateRecursiveG(path, pathNode, handler);
          }
        }
      }
    }
  }

  public int SharedEdge(GraphNode other)
  {
    int aIndex;
    this.GetPortal(other, (List<Vector3>) null, (List<Vector3>) null, false, out aIndex, out int _);
    return aIndex;
  }

  public override bool GetPortal(
    GraphNode _other,
    List<Vector3> left,
    List<Vector3> right,
    bool backwards)
  {
    return this.GetPortal(_other, left, right, backwards, out int _, out int _);
  }

  public bool GetPortal(
    GraphNode _other,
    List<Vector3> left,
    List<Vector3> right,
    bool backwards,
    out int aIndex,
    out int bIndex)
  {
    aIndex = -1;
    bIndex = -1;
    if ((int) _other.GraphIndex != (int) this.GraphIndex)
      return false;
    TriangleMeshNode other = _other as TriangleMeshNode;
    int tileIndex1 = this.GetVertexIndex(0) >> 12 & 524287 /*0x07FFFF*/;
    int tileIndex2 = other.GetVertexIndex(0) >> 12 & 524287 /*0x07FFFF*/;
    if (tileIndex1 != tileIndex2 && TriangleMeshNode.GetNavmeshHolder(this.GraphIndex) is RecastGraph)
    {
      for (int index = 0; index < this.connections.Length; ++index)
      {
        if ((int) this.connections[index].GraphIndex != (int) this.GraphIndex && this.connections[index] is NodeLink3Node connection && connection.GetOther((GraphNode) this) == other && left != null)
        {
          connection.GetPortal((GraphNode) other, left, right, false);
          return true;
        }
      }
      INavmeshHolder navmeshHolder = TriangleMeshNode.GetNavmeshHolder(this.GraphIndex);
      int x1;
      int z1;
      navmeshHolder.GetTileCoordinates(tileIndex1, out x1, out z1);
      int x2;
      int z2;
      navmeshHolder.GetTileCoordinates(tileIndex2, out x2, out z2);
      int i1;
      if (Math.Abs(x1 - x2) == 1)
        i1 = 0;
      else if (Math.Abs(z1 - z2) == 1)
        i1 = 2;
      else
        throw new Exception($"Tiles not adjacent ({x1.ToString()}, {z1.ToString()}) ({x2.ToString()}, {z2.ToString()})");
      int vertexCount1 = this.GetVertexCount();
      int vertexCount2 = other.GetVertexCount();
      int i2 = -1;
      int i3 = -1;
      Int3 vertex1;
      for (int i4 = 0; i4 < vertexCount1; ++i4)
      {
        vertex1 = this.GetVertex(i4);
        int num1 = vertex1[i1];
        for (int i5 = 0; i5 < vertexCount2; ++i5)
        {
          int num2 = num1;
          vertex1 = other.GetVertex((i5 + 1) % vertexCount2);
          int num3 = vertex1[i1];
          if (num2 == num3)
          {
            vertex1 = this.GetVertex((i4 + 1) % vertexCount1);
            int num4 = vertex1[i1];
            vertex1 = other.GetVertex(i5);
            int num5 = vertex1[i1];
            if (num4 == num5)
            {
              i2 = i4;
              i3 = i5;
              i4 = vertexCount1;
              break;
            }
          }
        }
      }
      aIndex = i2;
      bIndex = i3;
      if (i2 != -1)
      {
        Int3 vertex2 = this.GetVertex(i2);
        Int3 vertex3 = this.GetVertex((i2 + 1) % vertexCount1);
        int i6 = i1 == 2 ? 0 : 2;
        int num6 = Math.Min(vertex2[i6], vertex3[i6]);
        int num7 = Math.Max(vertex2[i6], vertex3[i6]);
        int val1_1 = num6;
        vertex1 = other.GetVertex(i3);
        int val1_2 = vertex1[i6];
        vertex1 = other.GetVertex((i3 + 1) % vertexCount2);
        int val2_1 = vertex1[i6];
        int val2_2 = Math.Min(val1_2, val2_1);
        int num8 = Math.Max(val1_1, val2_2);
        int val1_3 = num7;
        vertex1 = other.GetVertex(i3);
        int val1_4 = vertex1[i6];
        vertex1 = other.GetVertex((i3 + 1) % vertexCount2);
        int val2_3 = vertex1[i6];
        int val2_4 = Math.Max(val1_4, val2_3);
        int num9 = Math.Min(val1_3, val2_4);
        if (vertex2[i6] < vertex3[i6])
        {
          vertex2[i6] = num8;
          vertex3[i6] = num9;
        }
        else
        {
          vertex2[i6] = num9;
          vertex3[i6] = num8;
        }
        if (left != null)
        {
          left.Add((Vector3) vertex2);
          right.Add((Vector3) vertex3);
        }
        return true;
      }
    }
    else if (!backwards)
    {
      int i7 = -1;
      int num = -1;
      int vertexCount3 = this.GetVertexCount();
      int vertexCount4 = other.GetVertexCount();
      for (int i8 = 0; i8 < vertexCount3; ++i8)
      {
        int vertexIndex = this.GetVertexIndex(i8);
        for (int i9 = 0; i9 < vertexCount4; ++i9)
        {
          if (vertexIndex == other.GetVertexIndex((i9 + 1) % vertexCount4) && this.GetVertexIndex((i8 + 1) % vertexCount3) == other.GetVertexIndex(i9))
          {
            i7 = i8;
            num = i9;
            i8 = vertexCount3;
            break;
          }
        }
      }
      aIndex = i7;
      bIndex = num;
      if (i7 != -1)
      {
        if (left != null)
        {
          left.Add((Vector3) this.GetVertex(i7));
          right.Add((Vector3) this.GetVertex((i7 + 1) % vertexCount3));
        }
      }
      else
      {
        for (int index = 0; index < this.connections.Length; ++index)
        {
          if ((int) this.connections[index].GraphIndex != (int) this.GraphIndex && this.connections[index] is NodeLink3Node connection && connection.GetOther((GraphNode) this) == other && left != null)
          {
            connection.GetPortal((GraphNode) other, left, right, false);
            return true;
          }
        }
        return false;
      }
    }
    return true;
  }

  public override void SerializeNode(GraphSerializationContext ctx)
  {
    base.SerializeNode(ctx);
    ctx.writer.Write(this.v0);
    ctx.writer.Write(this.v1);
    ctx.writer.Write(this.v2);
  }

  public override void DeserializeNode(GraphSerializationContext ctx)
  {
    base.DeserializeNode(ctx);
    this.v0 = ctx.reader.ReadInt32();
    this.v1 = ctx.reader.ReadInt32();
    this.v2 = ctx.reader.ReadInt32();
  }
}
