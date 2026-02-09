// Decompiled with JetBrains decompiler
// Type: Pathfinding.EuclideanEmbedding
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Pathfinding;

[Serializable]
public class EuclideanEmbedding
{
  public HeuristicOptimizationMode mode;
  public int seed;
  public Transform pivotPointRoot;
  public int spreadOutCount = 1;
  [NonSerialized]
  public bool dirty;
  public uint[] costs = new uint[8];
  public int maxNodeIndex;
  public int pivotCount;
  public GraphNode[] pivots;
  public const uint ra = 12820163;
  public const uint rc = 1140671485 /*0x43FD43FD*/;
  public uint rval;
  public object lockObj = new object();

  public uint GetRandom()
  {
    this.rval = (uint) (12820163 * (int) this.rval + 1140671485 /*0x43FD43FD*/);
    return this.rval;
  }

  public void EnsureCapacity(int index)
  {
    if (index <= this.maxNodeIndex)
      return;
    lock (this.lockObj)
    {
      if (index <= this.maxNodeIndex)
        return;
      if (index >= this.costs.Length)
      {
        uint[] numArray = new uint[Math.Max(index * 2, this.pivots.Length * 2)];
        for (int index1 = 0; index1 < this.costs.Length; ++index1)
          numArray[index1] = this.costs[index1];
        this.costs = numArray;
      }
      this.maxNodeIndex = index;
    }
  }

  public uint GetHeuristic(int nodeIndex1, int nodeIndex2)
  {
    nodeIndex1 *= this.pivotCount;
    nodeIndex2 *= this.pivotCount;
    if (nodeIndex1 >= this.costs.Length || nodeIndex2 >= this.costs.Length)
      this.EnsureCapacity(nodeIndex1 > nodeIndex2 ? nodeIndex1 : nodeIndex2);
    uint heuristic = 0;
    for (int index = 0; index < this.pivotCount; ++index)
    {
      uint num = (uint) Math.Abs((int) this.costs[nodeIndex1 + index] - (int) this.costs[nodeIndex2 + index]);
      if (num > heuristic)
        heuristic = num;
    }
    return heuristic;
  }

  public void GetClosestWalkableNodesToChildrenRecursively(Transform tr, List<GraphNode> nodes)
  {
    foreach (Transform tr1 in tr)
    {
      NNInfo nearest = AstarPath.active.GetNearest(tr1.position, NNConstraint.Default);
      if (nearest.node != null && nearest.node.Walkable)
        nodes.Add(nearest.node);
      this.GetClosestWalkableNodesToChildrenRecursively(tr1, nodes);
    }
  }

  public void PickNRandomNodes(int count, List<GraphNode> buffer)
  {
    int n = 0;
    foreach (NavGraph graph in AstarPath.active.graphs)
      graph.GetNodes((GraphNodeDelegateCancelable) (node =>
      {
        if (!node.Destroyed && node.Walkable)
        {
          ++n;
          if ((long) this.GetRandom() % (long) n < (long) count)
          {
            if (buffer.Count < count)
              buffer.Add(node);
            else
              buffer[(int) ((long) this.GetRandom() % (long) buffer.Count)] = node;
          }
        }
        return true;
      }));
  }

  public GraphNode PickAnyWalkableNode()
  {
    NavGraph[] graphs = AstarPath.active.graphs;
    GraphNode first = (GraphNode) null;
    for (int index = 0; index < graphs.Length; ++index)
      graphs[index].GetNodes((GraphNodeDelegateCancelable) (node =>
      {
        if (node == null || !node.Walkable)
          return true;
        first = node;
        return false;
      }));
    return first;
  }

  public void RecalculatePivots()
  {
    if (this.mode == HeuristicOptimizationMode.None)
    {
      this.pivotCount = 0;
      this.pivots = (GraphNode[]) null;
    }
    else
    {
      this.rval = (uint) this.seed;
      List<GraphNode> graphNodeList = ListPool<GraphNode>.Claim();
      switch (this.mode)
      {
        case HeuristicOptimizationMode.Random:
          this.PickNRandomNodes(this.spreadOutCount, graphNodeList);
          break;
        case HeuristicOptimizationMode.RandomSpreadOut:
          if ((UnityEngine.Object) this.pivotPointRoot != (UnityEngine.Object) null)
            this.GetClosestWalkableNodesToChildrenRecursively(this.pivotPointRoot, graphNodeList);
          if (graphNodeList.Count == 0)
          {
            GraphNode graphNode = this.PickAnyWalkableNode();
            if (graphNode != null)
            {
              graphNodeList.Add(graphNode);
            }
            else
            {
              Debug.LogError((object) "Could not find any walkable node in any of the graphs.");
              ListPool<GraphNode>.Release(graphNodeList);
              return;
            }
          }
          int num = this.spreadOutCount - graphNodeList.Count;
          for (int index = 0; index < num; ++index)
            graphNodeList.Add((GraphNode) null);
          break;
        case HeuristicOptimizationMode.Custom:
          if ((UnityEngine.Object) this.pivotPointRoot == (UnityEngine.Object) null)
            throw new Exception("heuristicOptimizationMode is HeuristicOptimizationMode.Custom, but no 'customHeuristicOptimizationPivotsRoot' is set");
          this.GetClosestWalkableNodesToChildrenRecursively(this.pivotPointRoot, graphNodeList);
          break;
        default:
          throw new Exception("Invalid HeuristicOptimizationMode: " + this.mode.ToString());
      }
      this.pivots = graphNodeList.ToArray();
      ListPool<GraphNode>.Release(graphNodeList);
    }
  }

  public void RecalculateCosts()
  {
    if (this.pivots == null)
      this.RecalculatePivots();
    if (this.mode == HeuristicOptimizationMode.None)
      return;
    this.pivotCount = 0;
    for (int index = 0; index < this.pivots.Length; ++index)
    {
      if (this.pivots[index] != null && (this.pivots[index].Destroyed || !this.pivots[index].Walkable))
        throw new Exception("Invalid pivot nodes (destroyed or unwalkable)");
    }
    if (this.mode != HeuristicOptimizationMode.RandomSpreadOut)
    {
      for (int index = 0; index < this.pivots.Length; ++index)
      {
        if (this.pivots[index] == null)
          throw new Exception("Invalid pivot nodes (null)");
      }
    }
    Debug.Log((object) "Recalculating costs...");
    this.pivotCount = this.pivots.Length;
    Action<int> startCostCalculation = (Action<int>) null;
    int numComplete = 0;
    OnPathDelegate onComplete = (OnPathDelegate) (path =>
    {
      ++numComplete;
      if (numComplete != this.pivotCount)
        return;
      Debug.Log((object) "Grid graph special case!");
      this.ApplyGridGraphEndpointSpecialCase();
    });
    startCostCalculation = (Action<int>) (k =>
    {
      GraphNode pivot = this.pivots[k];
      FloodPath fp = (FloodPath) null;
      fp = FloodPath.Construct(pivot, onComplete);
      fp.immediateCallback = (OnPathDelegate) (_p =>
      {
        _p.Claim((object) this);
        MeshNode meshNode = pivot as MeshNode;
        uint costOffset = 0;
        if (meshNode != null && meshNode.connectionCosts != null)
        {
          for (int index = 0; index < meshNode.connectionCosts.Length; ++index)
            costOffset = Math.Max(costOffset, meshNode.connectionCosts[index]);
        }
        NavGraph[] graphs = AstarPath.active.graphs;
        for (int index = graphs.Length - 1; index >= 0; --index)
          graphs[index].GetNodes((GraphNodeDelegateCancelable) (node =>
          {
            int index2 = node.NodeIndex * this.pivotCount + k;
            this.EnsureCapacity(index2);
            PathNode pathNode = fp.pathHandler.GetPathNode(node);
            this.costs[index2] = costOffset <= 0U ? ((int) pathNode.pathID == (int) fp.pathID ? pathNode.G : 0U) : ((int) pathNode.pathID != (int) fp.pathID || pathNode.parent == null ? 0U : Math.Max(pathNode.parent.G - costOffset, 0U));
            return true;
          }));
        if (this.mode == HeuristicOptimizationMode.RandomSpreadOut && k < this.pivots.Length - 1)
        {
          if (this.pivots[k + 1] == null)
          {
            int nodeIndex3 = -1;
            uint num3 = 0;
            int num4 = this.maxNodeIndex / this.pivotCount;
            for (int nodeIndex4 = 1; nodeIndex4 < num4; ++nodeIndex4)
            {
              uint val1 = 1073741824 /*0x40000000*/;
              for (int index = 0; index <= k; ++index)
                val1 = Math.Min(val1, this.costs[nodeIndex4 * this.pivotCount + index]);
              GraphNode node = fp.pathHandler.GetPathNode(nodeIndex4).node;
              if ((val1 > num3 || nodeIndex3 == -1) && node != null && !node.Destroyed && node.Walkable)
              {
                nodeIndex3 = nodeIndex4;
                num3 = val1;
              }
            }
            if (nodeIndex3 == -1)
            {
              Debug.LogError((object) "Failed generating random pivot points for heuristic optimizations");
              return;
            }
            this.pivots[k + 1] = fp.pathHandler.GetPathNode(nodeIndex3).node;
          }
          startCostCalculation(k + 1);
        }
        _p.Release((object) this);
      });
      AstarPath.StartPath((Path) fp, true);
    });
    if (this.mode != HeuristicOptimizationMode.RandomSpreadOut)
    {
      for (int index = 0; index < this.pivots.Length; ++index)
        startCostCalculation(index);
    }
    else
      startCostCalculation(0);
    this.dirty = false;
  }

  public void ApplyGridGraphEndpointSpecialCase()
  {
    foreach (NavGraph graph in AstarPath.active.graphs)
    {
      if (graph is GridGraph gridGraph)
      {
        GridNode[] nodes = gridGraph.nodes;
        int num1 = gridGraph.neighbours == NumNeighbours.Four ? 4 : (gridGraph.neighbours == NumNeighbours.Eight ? 8 : 6);
        for (int index1 = 0; index1 < gridGraph.depth; ++index1)
        {
          for (int index2 = 0; index2 < gridGraph.width; ++index2)
          {
            GridNode gridNode = nodes[index1 * gridGraph.width + index2];
            if (!gridNode.Walkable)
            {
              int num2 = gridNode.NodeIndex * this.pivotCount;
              for (int index3 = 0; index3 < this.pivotCount; ++index3)
                this.costs[num2 + index3] = uint.MaxValue;
              for (int index4 = 0; index4 < num1; ++index4)
              {
                int num3;
                int num4;
                if (gridGraph.neighbours == NumNeighbours.Six)
                {
                  num3 = index2 + gridGraph.neighbourXOffsets[GridGraph.hexagonNeighbourIndices[index4]];
                  num4 = index1 + gridGraph.neighbourZOffsets[GridGraph.hexagonNeighbourIndices[index4]];
                }
                else
                {
                  num3 = index2 + gridGraph.neighbourXOffsets[index4];
                  num4 = index1 + gridGraph.neighbourZOffsets[index4];
                }
                if (num3 >= 0 && num4 >= 0 && num3 < gridGraph.width && num4 < gridGraph.depth)
                {
                  GridNode node = gridGraph.nodes[num4 * gridGraph.width + num3];
                  if (node.Walkable)
                  {
                    for (int index5 = 0; index5 < this.pivotCount; ++index5)
                    {
                      uint val2 = this.costs[node.NodeIndex * this.pivotCount + index5] + gridGraph.neighbourCosts[index4];
                      this.costs[num2 + index5] = Math.Min(this.costs[num2 + index5], val2);
                      Debug.DrawLine((Vector3) gridNode.position, (Vector3) node.position, Color.blue, 1f);
                    }
                  }
                }
              }
              for (int index6 = 0; index6 < this.pivotCount; ++index6)
              {
                if (this.costs[num2 + index6] == uint.MaxValue)
                  this.costs[num2 + index6] = 0U;
              }
            }
          }
        }
      }
    }
  }

  public void OnDrawGizmos()
  {
    if (this.pivots == null)
      return;
    for (int index = 0; index < this.pivots.Length; ++index)
    {
      Gizmos.color = new Color(0.623529434f, 0.368627459f, 0.7607843f, 0.8f);
      if (this.pivots[index] != null && !this.pivots[index].Destroyed)
        Gizmos.DrawCube((Vector3) this.pivots[index].position, Vector3.one);
    }
  }
}
