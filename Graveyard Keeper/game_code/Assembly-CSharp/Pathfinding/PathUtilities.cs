// Decompiled with JetBrains decompiler
// Type: Pathfinding.PathUtilities
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Pathfinding;

public static class PathUtilities
{
  public static Queue<GraphNode> BFSQueue;
  public static Dictionary<GraphNode, int> BFSMap;

  public static bool IsPathPossible(GraphNode n1, GraphNode n2)
  {
    return n1.Walkable && n2.Walkable && (int) n1.Area == (int) n2.Area;
  }

  public static bool IsPathPossible(List<GraphNode> nodes)
  {
    if (nodes.Count == 0)
      return true;
    uint area = nodes[0].Area;
    for (int index = 0; index < nodes.Count; ++index)
    {
      if (!nodes[index].Walkable || (int) nodes[index].Area != (int) area)
        return false;
    }
    return true;
  }

  public static bool IsPathPossible(List<GraphNode> nodes, int tagMask)
  {
    if (nodes.Count == 0)
      return true;
    if ((tagMask >> (int) nodes[0].Tag & 1) == 0 || !PathUtilities.IsPathPossible(nodes))
      return false;
    List<GraphNode> reachableNodes = PathUtilities.GetReachableNodes(nodes[0], tagMask);
    bool flag = true;
    for (int index = 1; index < nodes.Count; ++index)
    {
      if (!reachableNodes.Contains(nodes[index]))
      {
        flag = false;
        break;
      }
    }
    ListPool<GraphNode>.Release(reachableNodes);
    return flag;
  }

  public static List<GraphNode> GetReachableNodes(GraphNode seed, int tagMask = -1)
  {
    Stack<GraphNode> stack = StackPool<GraphNode>.Claim();
    List<GraphNode> list = ListPool<GraphNode>.Claim();
    HashSet<GraphNode> map = new HashSet<GraphNode>();
    GraphNodeDelegate del = tagMask != -1 ? (GraphNodeDelegate) (node =>
    {
      if (!node.Walkable || (tagMask >> (int) node.Tag & 1) == 0 || !map.Add(node))
        return;
      list.Add(node);
      stack.Push(node);
    }) : (GraphNodeDelegate) (node =>
    {
      if (!node.Walkable || !map.Add(node))
        return;
      list.Add(node);
      stack.Push(node);
    });
    del(seed);
    while (stack.Count > 0)
      stack.Pop().GetConnections(del);
    StackPool<GraphNode>.Release(stack);
    return list;
  }

  public static List<GraphNode> BFS(GraphNode seed, int depth, int tagMask = -1)
  {
    PathUtilities.BFSQueue = PathUtilities.BFSQueue ?? new Queue<GraphNode>();
    Queue<GraphNode> que = PathUtilities.BFSQueue;
    PathUtilities.BFSMap = PathUtilities.BFSMap ?? new Dictionary<GraphNode, int>();
    Dictionary<GraphNode, int> map = PathUtilities.BFSMap;
    que.Clear();
    map.Clear();
    List<GraphNode> result = ListPool<GraphNode>.Claim();
    int currentDist = -1;
    GraphNodeDelegate del = tagMask != -1 ? (GraphNodeDelegate) (node =>
    {
      if (!node.Walkable || (tagMask >> (int) node.Tag & 1) == 0 || map.ContainsKey(node))
        return;
      map.Add(node, currentDist + 1);
      result.Add(node);
      que.Enqueue(node);
    }) : (GraphNodeDelegate) (node =>
    {
      if (!node.Walkable || map.ContainsKey(node))
        return;
      map.Add(node, currentDist + 1);
      result.Add(node);
      que.Enqueue(node);
    });
    del(seed);
    while (que.Count > 0)
    {
      GraphNode key = que.Dequeue();
      currentDist = map[key];
      if (currentDist < depth)
        key.GetConnections(del);
      else
        break;
    }
    que.Clear();
    map.Clear();
    return result;
  }

  public static List<Vector3> GetSpiralPoints(int count, float clearance)
  {
    List<Vector3> spiralPoints = ListPool<Vector3>.Claim(count);
    float a = clearance / 6.28318548f;
    float t1 = 0.0f;
    spiralPoints.Add(PathUtilities.InvoluteOfCircle(a, t1));
    for (int index = 0; index < count; ++index)
    {
      Vector3 vector3 = spiralPoints[spiralPoints.Count - 1];
      float num1 = (float) (-(double) t1 / 2.0) + Mathf.Sqrt((float) ((double) t1 * (double) t1 / 4.0 + 2.0 * (double) clearance / (double) a));
      float num2 = t1 + num1;
      float t2 = t1 + 2f * num1;
      while ((double) t2 - (double) num2 > 0.0099999997764825821)
      {
        float t3 = (float) (((double) num2 + (double) t2) / 2.0);
        if ((double) (PathUtilities.InvoluteOfCircle(a, t3) - vector3).sqrMagnitude < (double) clearance * (double) clearance)
          num2 = t3;
        else
          t2 = t3;
      }
      spiralPoints.Add(PathUtilities.InvoluteOfCircle(a, t2));
      t1 = t2;
    }
    return spiralPoints;
  }

  public static Vector3 InvoluteOfCircle(float a, float t)
  {
    return new Vector3(a * (Mathf.Cos(t) + t * Mathf.Sin(t)), 0.0f, a * (Mathf.Sin(t) - t * Mathf.Cos(t)));
  }

  public static void GetPointsAroundPointWorld(
    Vector3 p,
    IRaycastableGraph g,
    List<Vector3> previousPoints,
    float radius,
    float clearanceRadius)
  {
    if (previousPoints.Count == 0)
      return;
    Vector3 zero = Vector3.zero;
    for (int index = 0; index < previousPoints.Count; ++index)
      zero += previousPoints[index];
    Vector3 vector3 = zero / (float) previousPoints.Count;
    for (int index = 0; index < previousPoints.Count; ++index)
      previousPoints[index] -= vector3;
    PathUtilities.GetPointsAroundPoint(p, g, previousPoints, radius, clearanceRadius);
  }

  public static void GetPointsAroundPoint(
    Vector3 p,
    IRaycastableGraph g,
    List<Vector3> previousPoints,
    float radius,
    float clearanceRadius)
  {
    if (g == null)
      throw new ArgumentNullException(nameof (g));
    if (!(g is NavGraph navGraph))
      throw new ArgumentException("g is not a NavGraph");
    NNInfo nearestForce = navGraph.GetNearestForce(p, NNConstraint.Default);
    p = nearestForce.clampedPosition;
    if (nearestForce.node == null)
      return;
    radius = Mathf.Max(radius, 1.4142f * clearanceRadius * Mathf.Sqrt((float) previousPoints.Count));
    clearanceRadius *= clearanceRadius;
    for (int index1 = 0; index1 < previousPoints.Count; ++index1)
    {
      Vector3 vector3_1 = previousPoints[index1];
      float magnitude = vector3_1.magnitude;
      if ((double) magnitude > 0.0)
        vector3_1 /= magnitude;
      float a = radius;
      vector3_1 *= a;
      bool flag = false;
      int num1 = 0;
      do
      {
        Vector3 end = p + vector3_1;
        GraphHitInfo hit;
        if (g.Linecast(p, end, nearestForce.node, out hit))
          end = hit.point;
        for (float num2 = 0.1f; (double) num2 <= 1.0; num2 += 0.05f)
        {
          Vector3 vector3_2 = (end - p) * num2 + p;
          flag = true;
          for (int index2 = 0; index2 < index1; ++index2)
          {
            if ((double) (previousPoints[index2] - vector3_2).sqrMagnitude < (double) clearanceRadius)
            {
              flag = false;
              break;
            }
          }
          if (flag)
          {
            previousPoints[index1] = vector3_2;
            break;
          }
        }
        if (!flag)
        {
          if (num1 > 8)
          {
            flag = true;
          }
          else
          {
            clearanceRadius *= 0.9f;
            vector3_1 = (UnityEngine.Random.onUnitSphere * Mathf.Lerp(a, radius, (float) (num1 / 5))) with
            {
              y = 0.0f
            };
            ++num1;
          }
        }
      }
      while (!flag);
    }
  }

  public static List<Vector3> GetPointsOnNodes(
    List<GraphNode> nodes,
    int count,
    float clearanceRadius = 0.0f)
  {
    if (nodes == null)
      throw new ArgumentNullException(nameof (nodes));
    if (nodes.Count == 0)
      throw new ArgumentException("no nodes passed");
    System.Random random = new System.Random();
    List<Vector3> pointsOnNodes = ListPool<Vector3>.Claim(count);
    clearanceRadius *= clearanceRadius;
    if (nodes[0] is TriangleMeshNode || nodes[0] is GridNode)
    {
      List<float> list = ListPool<float>.Claim(nodes.Count);
      float num1 = 0.0f;
      for (int index = 0; index < nodes.Count; ++index)
      {
        if (nodes[index] is TriangleMeshNode node2)
        {
          float num2 = (float) Math.Abs(VectorMath.SignedTriangleAreaTimes2XZ(node2.GetVertex(0), node2.GetVertex(1), node2.GetVertex(2)));
          num1 += num2;
          list.Add(num1);
        }
        else if (nodes[index] is GridNode node1)
        {
          GridGraph gridGraph = GridNode.GetGridGraph(node1.GraphIndex);
          float num3 = gridGraph.nodeSize * gridGraph.nodeSize;
          num1 += num3;
          list.Add(num1);
        }
        else
          list.Add(num1);
      }
      for (int index1 = 0; index1 < count; ++index1)
      {
        int num4 = 0;
        int num5 = 10;
        bool flag = false;
        while (!flag)
        {
          flag = true;
          if (num4 >= num5)
          {
            clearanceRadius *= 0.8f;
            num5 += 10;
            if (num5 > 100)
              clearanceRadius = 0.0f;
          }
          float num6 = (float) random.NextDouble() * num1;
          int index2 = list.BinarySearch(num6);
          if (index2 < 0)
            index2 = ~index2;
          if (index2 >= nodes.Count)
          {
            flag = false;
          }
          else
          {
            Vector3 vector3;
            if (nodes[index2] is TriangleMeshNode node4)
            {
              float num7;
              float num8;
              do
              {
                num7 = (float) random.NextDouble();
                num8 = (float) random.NextDouble();
              }
              while ((double) num7 + (double) num8 > 1.0);
              vector3 = (Vector3) (node4.GetVertex(1) - node4.GetVertex(0)) * num7 + (Vector3) (node4.GetVertex(2) - node4.GetVertex(0)) * num8 + (Vector3) node4.GetVertex(0);
            }
            else if (nodes[index2] is GridNode node3)
            {
              GridGraph gridGraph = GridNode.GetGridGraph(node3.GraphIndex);
              float num9 = (float) random.NextDouble();
              float num10 = (float) random.NextDouble();
              vector3 = (Vector3) node3.position + new Vector3(num9 - 0.5f, 0.0f, num10 - 0.5f) * gridGraph.nodeSize;
            }
            else
            {
              pointsOnNodes.Add((Vector3) nodes[index2].position);
              break;
            }
            if ((double) clearanceRadius > 0.0)
            {
              for (int index3 = 0; index3 < pointsOnNodes.Count; ++index3)
              {
                if ((double) (pointsOnNodes[index3] - vector3).sqrMagnitude < (double) clearanceRadius)
                {
                  flag = false;
                  break;
                }
              }
            }
            if (flag)
            {
              pointsOnNodes.Add(vector3);
              break;
            }
            ++num4;
          }
        }
      }
      ListPool<float>.Release(list);
    }
    else
    {
      for (int index = 0; index < count; ++index)
        pointsOnNodes.Add((Vector3) nodes[random.Next(nodes.Count)].position);
    }
    return pointsOnNodes;
  }
}
