// Decompiled with JetBrains decompiler
// Type: Pathfinding.RichFunnel
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Pathfinding;

public class RichFunnel : RichPathPart
{
  public List<Vector3> left;
  public List<Vector3> right;
  public List<TriangleMeshNode> nodes;
  public Vector3 exactStart;
  public Vector3 exactEnd;
  public NavGraph graph;
  public int currentNode;
  public Vector3 currentPosition;
  public int checkForDestroyedNodesCounter;
  public RichPath path;
  public int[] triBuffer = new int[3];
  public RichFunnel.FunnelSimplification funnelSimplificationMode = RichFunnel.FunnelSimplification.Iterative;

  public RichFunnel()
  {
    this.left = ListPool<Vector3>.Claim();
    this.right = ListPool<Vector3>.Claim();
    this.nodes = new List<TriangleMeshNode>();
    this.graph = (NavGraph) null;
  }

  public RichFunnel Initialize(RichPath path, NavGraph graph)
  {
    if (graph == null)
      throw new ArgumentNullException(nameof (graph));
    this.graph = this.graph == null ? graph : throw new InvalidOperationException("Trying to initialize an already initialized object. " + graph?.ToString());
    this.path = path;
    return this;
  }

  public override void OnEnterPool()
  {
    this.left.Clear();
    this.right.Clear();
    this.nodes.Clear();
    this.graph = (NavGraph) null;
    this.currentNode = 0;
    this.checkForDestroyedNodesCounter = 0;
  }

  public void BuildFunnelCorridor(List<GraphNode> nodes, int start, int end)
  {
    this.exactStart = (nodes[start] as MeshNode).ClosestPointOnNode(this.exactStart);
    this.exactEnd = (nodes[end] as MeshNode).ClosestPointOnNode(this.exactEnd);
    this.left.Clear();
    this.right.Clear();
    this.left.Add(this.exactStart);
    this.right.Add(this.exactStart);
    this.nodes.Clear();
    if (this.graph is IRaycastableGraph graph && this.funnelSimplificationMode != RichFunnel.FunnelSimplification.None)
    {
      List<GraphNode> graphNodeList = ListPool<GraphNode>.Claim(end - start);
      switch (this.funnelSimplificationMode)
      {
        case RichFunnel.FunnelSimplification.Iterative:
          this.SimplifyPath(graph, nodes, start, end, graphNodeList, this.exactStart, this.exactEnd);
          break;
        case RichFunnel.FunnelSimplification.RecursiveBinary:
          RichFunnel.SimplifyPath2(graph, nodes, start, end, graphNodeList, this.exactStart, this.exactEnd);
          break;
        case RichFunnel.FunnelSimplification.RecursiveTrinary:
          RichFunnel.SimplifyPath3(graph, nodes, start, end, graphNodeList, this.exactStart, this.exactEnd);
          break;
      }
      if (this.nodes.Capacity < graphNodeList.Count)
        this.nodes.Capacity = graphNodeList.Count;
      for (int index = 0; index < graphNodeList.Count; ++index)
      {
        if (graphNodeList[index] is TriangleMeshNode triangleMeshNode)
          this.nodes.Add(triangleMeshNode);
      }
      ListPool<GraphNode>.Release(graphNodeList);
    }
    else
    {
      if (this.nodes.Capacity < end - start)
        this.nodes.Capacity = end - start;
      for (int index = start; index <= end; ++index)
      {
        if (nodes[index] is TriangleMeshNode node)
          this.nodes.Add(node);
      }
    }
    for (int index = 0; index < this.nodes.Count - 1; ++index)
      this.nodes[index].GetPortal((GraphNode) this.nodes[index + 1], this.left, this.right, false);
    this.left.Add(this.exactEnd);
    this.right.Add(this.exactEnd);
  }

  public static void SimplifyPath3(
    IRaycastableGraph rcg,
    List<GraphNode> nodes,
    int start,
    int end,
    List<GraphNode> result,
    Vector3 startPoint,
    Vector3 endPoint,
    int depth = 0)
  {
    if (start == end)
      result.Add(nodes[start]);
    else if (start + 1 == end)
    {
      result.Add(nodes[start]);
      result.Add(nodes[end]);
    }
    else
    {
      int count = result.Count;
      if (!rcg.Linecast(startPoint, endPoint, nodes[start], out GraphHitInfo _, result) && result[result.Count - 1] == nodes[end])
        return;
      result.RemoveRange(count, result.Count - count);
      int num1 = 0;
      float num2 = 0.0f;
      for (int index = start + 1; index < end - 1; ++index)
      {
        float num3 = VectorMath.SqrDistancePointSegment(startPoint, endPoint, (Vector3) nodes[index].position);
        if ((double) num3 > (double) num2)
        {
          num1 = index;
          num2 = num3;
        }
      }
      int num4 = (num1 + start) / 2;
      int num5 = (num1 + end) / 2;
      if (num4 == num5)
      {
        RichFunnel.SimplifyPath3(rcg, nodes, start, num4, result, startPoint, (Vector3) nodes[num4].position);
        result.RemoveAt(result.Count - 1);
        RichFunnel.SimplifyPath3(rcg, nodes, num4, end, result, (Vector3) nodes[num4].position, endPoint, depth + 1);
      }
      else
      {
        RichFunnel.SimplifyPath3(rcg, nodes, start, num4, result, startPoint, (Vector3) nodes[num4].position, depth + 1);
        result.RemoveAt(result.Count - 1);
        RichFunnel.SimplifyPath3(rcg, nodes, num4, num5, result, (Vector3) nodes[num4].position, (Vector3) nodes[num5].position, depth + 1);
        result.RemoveAt(result.Count - 1);
        RichFunnel.SimplifyPath3(rcg, nodes, num5, end, result, (Vector3) nodes[num5].position, endPoint, depth + 1);
      }
    }
  }

  public static void SimplifyPath2(
    IRaycastableGraph rcg,
    List<GraphNode> nodes,
    int start,
    int end,
    List<GraphNode> result,
    Vector3 startPoint,
    Vector3 endPoint)
  {
    int count = result.Count;
    if (end <= start + 1)
    {
      result.Add(nodes[start]);
      result.Add(nodes[end]);
    }
    else
    {
      if (!rcg.Linecast(startPoint, endPoint, nodes[start], out GraphHitInfo _, result) && result[result.Count - 1] == nodes[end])
        return;
      result.RemoveRange(count, result.Count - count);
      int num1 = -1;
      float num2 = float.PositiveInfinity;
      for (int index = start + 1; index < end; ++index)
      {
        float num3 = VectorMath.SqrDistancePointSegment(startPoint, endPoint, (Vector3) nodes[index].position);
        if (num1 == -1 || (double) num3 < (double) num2)
        {
          num1 = index;
          num2 = num3;
        }
      }
      RichFunnel.SimplifyPath2(rcg, nodes, start, num1, result, startPoint, (Vector3) nodes[num1].position);
      result.RemoveAt(result.Count - 1);
      RichFunnel.SimplifyPath2(rcg, nodes, num1, end, result, (Vector3) nodes[num1].position, endPoint);
    }
  }

  public void SimplifyPath(
    IRaycastableGraph graph,
    List<GraphNode> nodes,
    int start,
    int end,
    List<GraphNode> result,
    Vector3 startPoint,
    Vector3 endPoint)
  {
    if (graph == null)
      throw new ArgumentNullException(nameof (graph));
    int num1 = start <= end ? start : throw new ArgumentException("start >= end");
    int num2 = 0;
    while (num2++ <= 1000)
    {
      if (start == end)
      {
        result.Add(nodes[end]);
        return;
      }
      int count = result.Count;
      int num3 = end + 1;
      int index1 = start + 1;
      bool flag = false;
      while (num3 > index1 + 1)
      {
        int index2 = (num3 + index1) / 2;
        Vector3 start1 = start == num1 ? startPoint : (Vector3) nodes[start].position;
        Vector3 end1 = index2 == end ? endPoint : (Vector3) nodes[index2].position;
        if (graph.Linecast(start1, end1, nodes[start], out GraphHitInfo _))
        {
          num3 = index2;
        }
        else
        {
          flag = true;
          index1 = index2;
        }
      }
      if (!flag)
      {
        result.Add(nodes[start]);
        start = index1;
      }
      else
      {
        Vector3 start2 = start == num1 ? startPoint : (Vector3) nodes[start].position;
        Vector3 end2 = index1 == end ? endPoint : (Vector3) nodes[index1].position;
        graph.Linecast(start2, end2, nodes[start], out GraphHitInfo _, result);
        long num4 = 0;
        long num5 = 0;
        for (int index3 = start; index3 <= index1; ++index3)
          num4 += (long) nodes[index3].Penalty + ((UnityEngine.Object) this.path.seeker != (UnityEngine.Object) null ? (long) this.path.seeker.tagPenalties[(int) nodes[index3].Tag] : 0L);
        for (int index4 = count; index4 < result.Count; ++index4)
          num5 += (long) result[index4].Penalty + ((UnityEngine.Object) this.path.seeker != (UnityEngine.Object) null ? (long) this.path.seeker.tagPenalties[(int) result[index4].Tag] : 0L);
        if ((double) num4 * 1.4 * (double) (index1 - start + 1) < (double) (num5 * (long) (result.Count - count)) || result[result.Count - 1] != nodes[index1])
        {
          result.RemoveRange(count, result.Count - count);
          result.Add(nodes[start]);
          ++start;
        }
        else
        {
          result.RemoveAt(result.Count - 1);
          start = index1;
        }
      }
    }
    Debug.LogError((object) "!!!");
  }

  public void UpdateFunnelCorridor(int splitIndex, TriangleMeshNode prefix)
  {
    if (splitIndex > 0)
    {
      this.nodes.RemoveRange(0, splitIndex - 1);
      this.nodes[0] = prefix;
    }
    else
      this.nodes.Insert(0, prefix);
    this.left.Clear();
    this.right.Clear();
    this.left.Add(this.exactStart);
    this.right.Add(this.exactStart);
    for (int index = 0; index < this.nodes.Count - 1; ++index)
      this.nodes[index].GetPortal((GraphNode) this.nodes[index + 1], this.left, this.right, false);
    this.left.Add(this.exactEnd);
    this.right.Add(this.exactEnd);
  }

  public Vector3 Update(
    Vector3 position,
    List<Vector3> buffer,
    int numCorners,
    out bool lastCorner,
    out bool requiresRepath)
  {
    lastCorner = false;
    requiresRepath = false;
    Int3 p = (Int3) position;
    if (this.nodes[this.currentNode].Destroyed)
    {
      requiresRepath = true;
      lastCorner = false;
      buffer.Add(position);
      return position;
    }
    if (this.nodes[this.currentNode].ContainsPoint(p))
    {
      if (this.checkForDestroyedNodesCounter >= 10)
      {
        this.checkForDestroyedNodesCounter = 0;
        int index = 0;
        for (int count = this.nodes.Count; index < count; ++index)
        {
          if (this.nodes[index].Destroyed)
          {
            requiresRepath = true;
            break;
          }
        }
      }
      else
        ++this.checkForDestroyedNodesCounter;
    }
    else
    {
      bool flag = false;
      int index1 = this.currentNode + 1;
      for (int index2 = Math.Min(this.currentNode + 3, this.nodes.Count); index1 < index2 && !flag; ++index1)
      {
        if (this.nodes[index1].Destroyed)
        {
          requiresRepath = true;
          lastCorner = false;
          buffer.Add(position);
          return position;
        }
        if (this.nodes[index1].ContainsPoint(p))
        {
          this.currentNode = index1;
          flag = true;
        }
      }
      int index3 = this.currentNode - 1;
      for (int index4 = Math.Max(this.currentNode - 3, 0); index3 > index4 && !flag; --index3)
      {
        if (this.nodes[index3].Destroyed)
        {
          requiresRepath = true;
          lastCorner = false;
          buffer.Add(position);
          return position;
        }
        if (this.nodes[index3].ContainsPoint(p))
        {
          this.currentNode = index3;
          flag = true;
        }
      }
      if (!flag)
      {
        int index5 = 0;
        int closestIsNeighbourOf = 0;
        float closestDist = float.PositiveInfinity;
        bool closestIsInPath = false;
        TriangleMeshNode closestNode = (TriangleMeshNode) null;
        int containingIndex = this.nodes.Count - 1;
        this.checkForDestroyedNodesCounter = 0;
        int index6 = 0;
        for (int count = this.nodes.Count; index6 < count; ++index6)
        {
          if (this.nodes[index6].Destroyed)
          {
            requiresRepath = true;
            lastCorner = false;
            buffer.Add(position);
            return position;
          }
          float sqrMagnitude = (this.nodes[index6].ClosestPointOnNode(position) - position).sqrMagnitude;
          if ((double) sqrMagnitude < (double) closestDist)
          {
            closestDist = sqrMagnitude;
            index5 = index6;
            closestNode = this.nodes[index6];
            closestIsInPath = true;
          }
        }
        Vector3 posCopy = position;
        GraphNodeDelegate del = (GraphNodeDelegate) (node =>
        {
          if (containingIndex > 0 && node == this.nodes[containingIndex - 1] || containingIndex < this.nodes.Count - 1 && node == this.nodes[containingIndex + 1] || !(node is TriangleMeshNode triangleMeshNode2))
            return;
          float sqrMagnitude = (triangleMeshNode2.ClosestPointOnNode(posCopy) - posCopy).sqrMagnitude;
          if ((double) sqrMagnitude >= (double) closestDist)
            return;
          closestDist = sqrMagnitude;
          closestIsNeighbourOf = containingIndex;
          closestNode = triangleMeshNode2;
          closestIsInPath = false;
        });
        for (; containingIndex >= 0; containingIndex--)
          this.nodes[containingIndex].GetConnections(del);
        if (closestIsInPath)
        {
          this.currentNode = index5;
          position = this.nodes[index5].ClosestPointOnNodeXZ(position);
        }
        else
        {
          position = closestNode.ClosestPointOnNodeXZ(position);
          this.exactStart = position;
          this.UpdateFunnelCorridor(closestIsNeighbourOf, closestNode);
          this.currentNode = 0;
        }
      }
    }
    this.currentPosition = position;
    if (this.FindNextCorners(position, this.currentNode, buffer, numCorners, out lastCorner))
      return position;
    Debug.LogError((object) "Oh oh");
    buffer.Add(position);
    return position;
  }

  public void FindWalls(List<Vector3> wallBuffer, float range)
  {
    this.FindWalls(this.currentNode, wallBuffer, this.currentPosition, range);
  }

  public void FindWalls(int nodeIndex, List<Vector3> wallBuffer, Vector3 position, float range)
  {
    if ((double) range <= 0.0)
      return;
    bool flag1 = false;
    bool flag2 = false;
    range *= range;
    position.y = 0.0f;
    int num = 0;
    while (!flag1 || !flag2)
    {
      if (!(num < 0 & flag1) && !(num > 0 & flag2))
      {
        if (num < 0 && nodeIndex + num < 0)
          flag1 = true;
        else if (num > 0 && nodeIndex + num >= this.nodes.Count)
        {
          flag2 = true;
        }
        else
        {
          TriangleMeshNode node1 = nodeIndex + num - 1 < 0 ? (TriangleMeshNode) null : this.nodes[nodeIndex + num - 1];
          TriangleMeshNode node2 = this.nodes[nodeIndex + num];
          TriangleMeshNode node3 = nodeIndex + num + 1 >= this.nodes.Count ? (TriangleMeshNode) null : this.nodes[nodeIndex + num + 1];
          if (node2.Destroyed)
            break;
          if ((double) (node2.ClosestPointOnNodeXZ(position) - position).sqrMagnitude > (double) range)
          {
            if (num < 0)
              flag1 = true;
            else
              flag2 = true;
          }
          else
          {
            for (int index = 0; index < 3; ++index)
              this.triBuffer[index] = 0;
            for (int index1 = 0; index1 < node2.connections.Length; ++index1)
            {
              if (node2.connections[index1] is TriangleMeshNode connection)
              {
                int index2 = -1;
                for (int i1 = 0; i1 < 3; ++i1)
                {
                  for (int i2 = 0; i2 < 3; ++i2)
                  {
                    if (node2.GetVertex(i1) == connection.GetVertex((i2 + 1) % 3) && node2.GetVertex((i1 + 1) % 3) == connection.GetVertex(i2))
                    {
                      index2 = i1;
                      i1 = 3;
                      break;
                    }
                  }
                }
                if (index2 != -1)
                  this.triBuffer[index2] = connection == node1 || connection == node3 ? 2 : 1;
              }
            }
            for (int i = 0; i < 3; ++i)
            {
              if (this.triBuffer[i] == 0)
              {
                wallBuffer.Add((Vector3) node2.GetVertex(i));
                wallBuffer.Add((Vector3) node2.GetVertex((i + 1) % 3));
              }
            }
          }
        }
      }
      num = num < 0 ? -num : -num - 1;
    }
  }

  public bool FindNextCorners(
    Vector3 origin,
    int startIndex,
    List<Vector3> funnelPath,
    int numCorners,
    out bool lastCorner)
  {
    lastCorner = false;
    if (this.left == null)
      throw new Exception("left list is null");
    if (this.right == null)
      throw new Exception("right list is null");
    if (funnelPath == null)
      throw new ArgumentNullException(nameof (funnelPath));
    if (this.left.Count != this.right.Count)
      throw new ArgumentException("left and right lists must have equal length");
    int count = this.left.Count;
    if (count == 0)
      throw new ArgumentException("no diagonals");
    if (count - startIndex < 3)
    {
      funnelPath.Add(this.left[count - 1]);
      lastCorner = true;
      return true;
    }
    while (this.left[startIndex + 1] == this.left[startIndex + 2] && this.right[startIndex + 1] == this.right[startIndex + 2])
    {
      ++startIndex;
      if (count - startIndex <= 3)
        return false;
    }
    Vector3 p = this.left[startIndex + 2];
    if (p == this.left[startIndex + 1])
      p = this.right[startIndex + 2];
    while (VectorMath.IsColinearXZ(origin, this.left[startIndex + 1], this.right[startIndex + 1]) || VectorMath.RightOrColinearXZ(this.left[startIndex + 1], this.right[startIndex + 1], p) == VectorMath.RightOrColinearXZ(this.left[startIndex + 1], this.right[startIndex + 1], origin))
    {
      ++startIndex;
      if (count - startIndex < 3)
      {
        funnelPath.Add(this.left[count - 1]);
        lastCorner = true;
        return true;
      }
      p = this.left[startIndex + 2];
      if (p == this.left[startIndex + 1])
        p = this.right[startIndex + 2];
    }
    Vector3 a = origin;
    Vector3 b1 = this.left[startIndex + 1];
    Vector3 b2 = this.right[startIndex + 1];
    int num1 = startIndex + 1;
    int num2 = startIndex + 1;
    for (int index = startIndex + 2; index < count; ++index)
    {
      if (funnelPath.Count >= numCorners)
        return true;
      if (funnelPath.Count > 2000)
      {
        Debug.LogWarning((object) "Avoiding infinite loop. Remove this check if you have this long paths.");
        break;
      }
      Vector3 c1 = this.left[index];
      Vector3 c2 = this.right[index];
      if ((double) VectorMath.SignedTriangleAreaTimes2XZ(a, b2, c2) >= 0.0)
      {
        if (a == b2 || (double) VectorMath.SignedTriangleAreaTimes2XZ(a, b1, c2) <= 0.0)
        {
          b2 = c2;
          num1 = index;
        }
        else
        {
          funnelPath.Add(b1);
          a = b1;
          int num3 = num2;
          b1 = a;
          b2 = a;
          num2 = num3;
          num1 = num3;
          index = num3;
          continue;
        }
      }
      if ((double) VectorMath.SignedTriangleAreaTimes2XZ(a, b1, c1) <= 0.0)
      {
        if (a == b1 || (double) VectorMath.SignedTriangleAreaTimes2XZ(a, b2, c1) >= 0.0)
        {
          b1 = c1;
          num2 = index;
        }
        else
        {
          funnelPath.Add(b2);
          a = b2;
          int num4 = num1;
          b1 = a;
          b2 = a;
          num2 = num4;
          num1 = num4;
          index = num4;
        }
      }
    }
    lastCorner = true;
    funnelPath.Add(this.left[count - 1]);
    return true;
  }

  public enum FunnelSimplification
  {
    None,
    Iterative,
    RecursiveBinary,
    RecursiveTrinary,
  }
}
