// Decompiled with JetBrains decompiler
// Type: Pathfinding.MultiTargetPath
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding.Util;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

#nullable disable
namespace Pathfinding;

public class MultiTargetPath : ABPath
{
  public OnPathDelegate[] callbacks;
  public GraphNode[] targetNodes;
  public int targetNodeCount;
  public bool[] targetsFound;
  public Vector3[] targetPoints;
  public Vector3[] originalTargetPoints;
  public List<Vector3>[] vectorPaths;
  public List<GraphNode>[] nodePaths;
  public bool pathsForAll = true;
  public int chosenTarget = -1;
  public int sequentialTarget;
  public MultiTargetPath.HeuristicMode heuristicMode = MultiTargetPath.HeuristicMode.Sequential;
  public bool inverted = true;

  public static MultiTargetPath Construct(
    Vector3[] startPoints,
    Vector3 target,
    OnPathDelegate[] callbackDelegates,
    OnPathDelegate callback = null)
  {
    MultiTargetPath multiTargetPath = MultiTargetPath.Construct(target, startPoints, callbackDelegates, callback);
    multiTargetPath.inverted = true;
    return multiTargetPath;
  }

  public static MultiTargetPath Construct(
    Vector3 start,
    Vector3[] targets,
    OnPathDelegate[] callbackDelegates,
    OnPathDelegate callback = null)
  {
    MultiTargetPath path = PathPool.GetPath<MultiTargetPath>();
    path.Setup(start, targets, callbackDelegates, callback);
    return path;
  }

  public void Setup(
    Vector3 start,
    Vector3[] targets,
    OnPathDelegate[] callbackDelegates,
    OnPathDelegate callback)
  {
    this.inverted = false;
    this.callback = callback;
    this.callbacks = callbackDelegates;
    this.targetPoints = targets;
    this.originalStartPoint = start;
    this.startPoint = start;
    this.startIntPoint = (Int3) start;
    if (targets.Length == 0)
    {
      this.Error();
      this.LogError("No targets were assigned to the MultiTargetPath");
    }
    else
    {
      this.endPoint = targets[0];
      this.originalTargetPoints = new Vector3[this.targetPoints.Length];
      for (int index = 0; index < this.targetPoints.Length; ++index)
        this.originalTargetPoints[index] = this.targetPoints[index];
    }
  }

  public override void OnEnterPool()
  {
    if (this.vectorPaths != null)
    {
      for (int index = 0; index < this.vectorPaths.Length; ++index)
      {
        if (this.vectorPaths[index] != null)
          ListPool<Vector3>.Release(this.vectorPaths[index]);
      }
    }
    this.vectorPaths = (List<Vector3>[]) null;
    this.vectorPath = (List<Vector3>) null;
    if (this.nodePaths != null)
    {
      for (int index = 0; index < this.nodePaths.Length; ++index)
      {
        if (this.nodePaths[index] != null)
          ListPool<GraphNode>.Release(this.nodePaths[index]);
      }
    }
    this.nodePaths = (List<GraphNode>[]) null;
    this.path = (List<GraphNode>) null;
    base.OnEnterPool();
  }

  public void ChooseShortestPath()
  {
    this.chosenTarget = -1;
    if (this.nodePaths == null)
      return;
    uint num = (uint) int.MaxValue;
    for (int index = 0; index < this.nodePaths.Length; ++index)
    {
      List<GraphNode> nodePath = this.nodePaths[index];
      if (nodePath != null)
      {
        uint g = this.pathHandler.GetPathNode(nodePath[this.inverted ? 0 : nodePath.Count - 1]).G;
        if (this.chosenTarget == -1 || g < num)
        {
          this.chosenTarget = index;
          num = g;
        }
      }
    }
  }

  public void SetPathParametersForReturn(int target)
  {
    this.path = this.nodePaths[target];
    this.vectorPath = this.vectorPaths[target];
    if (this.inverted)
    {
      this.startNode = this.targetNodes[target];
      this.startPoint = this.targetPoints[target];
      this.originalStartPoint = this.originalTargetPoints[target];
    }
    else
    {
      this.endNode = this.targetNodes[target];
      this.endPoint = this.targetPoints[target];
      this.originalEndPoint = this.originalTargetPoints[target];
    }
  }

  public override void ReturnPath()
  {
    if (this.error)
    {
      if (this.callbacks != null)
      {
        for (int index = 0; index < this.callbacks.Length; ++index)
        {
          if (this.callbacks[index] != null)
            this.callbacks[index]((Path) this);
        }
      }
      if (this.callback == null)
        return;
      this.callback((Path) this);
    }
    else
    {
      bool flag = false;
      if (this.inverted)
      {
        this.endPoint = this.startPoint;
        this.endNode = this.startNode;
        this.originalEndPoint = this.originalStartPoint;
      }
      for (int target = 0; target < this.nodePaths.Length; ++target)
      {
        if (this.nodePaths[target] != null)
        {
          this.CompleteState = PathCompleteState.Complete;
          flag = true;
        }
        else
          this.CompleteState = PathCompleteState.Error;
        if (this.callbacks != null && this.callbacks[target] != null)
        {
          this.SetPathParametersForReturn(target);
          this.callbacks[target]((Path) this);
          this.vectorPaths[target] = this.vectorPath;
        }
      }
      if (flag)
      {
        this.CompleteState = PathCompleteState.Complete;
        this.SetPathParametersForReturn(this.chosenTarget);
      }
      else
        this.CompleteState = PathCompleteState.Error;
      if (this.callback == null)
        return;
      this.callback((Path) this);
    }
  }

  public void FoundTarget(PathNode nodeR, int i)
  {
    nodeR.flag1 = false;
    this.Trace(nodeR);
    this.vectorPaths[i] = this.vectorPath;
    this.nodePaths[i] = this.path;
    this.vectorPath = ListPool<Vector3>.Claim();
    this.path = ListPool<GraphNode>.Claim();
    this.targetsFound[i] = true;
    --this.targetNodeCount;
    if (!this.pathsForAll)
    {
      this.CompleteState = PathCompleteState.Complete;
      this.targetNodeCount = 0;
    }
    else if (this.targetNodeCount <= 0)
      this.CompleteState = PathCompleteState.Complete;
    else
      this.RecalculateHTarget(false);
  }

  public void RebuildOpenList()
  {
    BinaryHeapM heap = this.pathHandler.GetHeap();
    for (int i = 0; i < heap.numberOfItems; ++i)
    {
      PathNode node = heap.GetNode(i);
      node.H = this.CalculateHScore(node.node);
      heap.SetF(i, node.F);
    }
    this.pathHandler.RebuildHeap();
  }

  public override void Prepare()
  {
    this.nnConstraint.tags = this.enabledTags;
    NNInfo nearest1 = AstarPath.active.GetNearest(this.startPoint, this.nnConstraint, this.startHint);
    this.startNode = nearest1.node;
    if (this.startNode == null)
    {
      this.LogError("Could not find start node for multi target path");
      this.Error();
    }
    else if (!this.startNode.Walkable)
    {
      this.LogError("Nearest node to the start point is not walkable");
      this.Error();
    }
    else
    {
      if (this.nnConstraint is PathNNConstraint nnConstraint)
        nnConstraint.SetStart(nearest1.node);
      this.vectorPaths = new List<Vector3>[this.targetPoints.Length];
      this.nodePaths = new List<GraphNode>[this.targetPoints.Length];
      this.targetNodes = new GraphNode[this.targetPoints.Length];
      this.targetsFound = new bool[this.targetPoints.Length];
      this.targetNodeCount = this.targetPoints.Length;
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      for (int index = 0; index < this.targetPoints.Length; ++index)
      {
        NNInfo nearest2 = AstarPath.active.GetNearest(this.targetPoints[index], this.nnConstraint);
        this.targetNodes[index] = nearest2.node;
        this.targetPoints[index] = nearest2.clampedPosition;
        if (this.targetNodes[index] != null)
        {
          flag3 = true;
          this.endNode = this.targetNodes[index];
        }
        bool flag4 = false;
        if (nearest2.node != null && nearest2.node.Walkable)
          flag1 = true;
        else
          flag4 = true;
        if (nearest2.node != null && (int) nearest2.node.Area == (int) this.startNode.Area)
          flag2 = true;
        else
          flag4 = true;
        if (flag4)
        {
          this.targetsFound[index] = true;
          --this.targetNodeCount;
        }
      }
      this.startPoint = nearest1.clampedPosition;
      this.startIntPoint = (Int3) this.startPoint;
      if (this.startNode == null || !flag3)
      {
        this.LogError($"Couldn't find close nodes to either the start or the end (start = {(this.startNode != null ? "found" : "not found")} end = {(flag3 ? "at least one found" : "none found")})");
        this.Error();
      }
      else if (!this.startNode.Walkable)
      {
        this.LogError("The node closest to the start point is not walkable");
        this.Error();
      }
      else if (!flag1)
      {
        this.LogError("No target nodes were walkable");
        this.Error();
      }
      else if (!flag2)
      {
        this.LogError("There are no valid paths to the targets");
        this.Error();
      }
      else
        this.RecalculateHTarget(true);
    }
  }

  public void RecalculateHTarget(bool firstTime)
  {
    if (!this.pathsForAll)
    {
      this.heuristic = Heuristic.None;
      this.heuristicScale = 0.0f;
    }
    else
    {
      switch (this.heuristicMode)
      {
        case MultiTargetPath.HeuristicMode.None:
          this.heuristic = Heuristic.None;
          this.heuristicScale = 0.0f;
          break;
        case MultiTargetPath.HeuristicMode.Average:
          if (!firstTime)
            return;
          goto case MultiTargetPath.HeuristicMode.MovingAverage;
        case MultiTargetPath.HeuristicMode.MovingAverage:
          Vector3 zero = Vector3.zero;
          int num1 = 0;
          for (int index = 0; index < this.targetPoints.Length; ++index)
          {
            if (!this.targetsFound[index])
            {
              zero += (Vector3) this.targetNodes[index].position;
              ++num1;
            }
          }
          if (num1 == 0)
            throw new Exception("Should not happen");
          this.hTarget = (Int3) (zero / (float) num1);
          break;
        case MultiTargetPath.HeuristicMode.Midpoint:
          if (!firstTime)
            return;
          goto case MultiTargetPath.HeuristicMode.MovingMidpoint;
        case MultiTargetPath.HeuristicMode.MovingMidpoint:
          Vector3 rhs1 = Vector3.zero;
          Vector3 rhs2 = Vector3.zero;
          bool flag = false;
          for (int index = 0; index < this.targetPoints.Length; ++index)
          {
            if (!this.targetsFound[index])
            {
              if (!flag)
              {
                rhs1 = (Vector3) this.targetNodes[index].position;
                rhs2 = (Vector3) this.targetNodes[index].position;
                flag = true;
              }
              else
              {
                rhs1 = Vector3.Min((Vector3) this.targetNodes[index].position, rhs1);
                rhs2 = Vector3.Max((Vector3) this.targetNodes[index].position, rhs2);
              }
            }
          }
          this.hTarget = (Int3) ((rhs1 + rhs2) * 0.5f);
          break;
        case MultiTargetPath.HeuristicMode.Sequential:
          if (!firstTime && !this.targetsFound[this.sequentialTarget])
            return;
          float num2 = 0.0f;
          for (int index = 0; index < this.targetPoints.Length; ++index)
          {
            if (!this.targetsFound[index])
            {
              float sqrMagnitude = (this.targetNodes[index].position - this.startNode.position).sqrMagnitude;
              if ((double) sqrMagnitude > (double) num2)
              {
                num2 = sqrMagnitude;
                this.hTarget = (Int3) this.targetPoints[index];
                this.sequentialTarget = index;
              }
            }
          }
          break;
      }
      if (firstTime)
        return;
      this.RebuildOpenList();
    }
  }

  public override void Initialize()
  {
    PathNode pathNode = this.pathHandler.GetPathNode(this.startNode);
    pathNode.node = this.startNode;
    pathNode.pathID = this.pathID;
    pathNode.parent = (PathNode) null;
    pathNode.cost = 0U;
    pathNode.G = this.GetTraversalCost(this.startNode);
    pathNode.H = this.CalculateHScore(this.startNode);
    for (int i = 0; i < this.targetNodes.Length; ++i)
    {
      if (this.startNode == this.targetNodes[i])
        this.FoundTarget(pathNode, i);
      else if (this.targetNodes[i] != null)
        this.pathHandler.GetPathNode(this.targetNodes[i]).flag1 = true;
    }
    if (this.targetNodeCount <= 0)
    {
      this.CompleteState = PathCompleteState.Complete;
    }
    else
    {
      this.startNode.Open((Path) this, pathNode, this.pathHandler);
      ++this.searchedNodes;
      if (this.pathHandler.HeapEmpty())
      {
        this.LogError("No open points, the start node didn't open any nodes");
        this.Error();
      }
      else
        this.currentR = this.pathHandler.PopNode();
    }
  }

  public override void Cleanup()
  {
    this.ChooseShortestPath();
    this.ResetFlags();
  }

  public void ResetFlags()
  {
    if (this.targetNodes == null)
      return;
    for (int index = 0; index < this.targetNodes.Length; ++index)
    {
      if (this.targetNodes[index] != null)
        this.pathHandler.GetPathNode(this.targetNodes[index]).flag1 = false;
    }
  }

  public override void CalculateStep(long targetTick)
  {
    int num = 0;
    while (this.CompleteState == PathCompleteState.NotCalculated)
    {
      ++this.searchedNodes;
      if (this.currentR.flag1)
      {
        for (int i = 0; i < this.targetNodes.Length; ++i)
        {
          if (!this.targetsFound[i] && this.currentR.node == this.targetNodes[i])
          {
            this.FoundTarget(this.currentR, i);
            if (this.CompleteState != PathCompleteState.NotCalculated)
              break;
          }
        }
        if (this.targetNodeCount <= 0)
        {
          this.CompleteState = PathCompleteState.Complete;
          break;
        }
      }
      this.currentR.node.Open((Path) this, this.currentR, this.pathHandler);
      if (this.pathHandler.HeapEmpty())
      {
        this.CompleteState = PathCompleteState.Complete;
        break;
      }
      this.currentR = this.pathHandler.PopNode();
      if (num > 500)
      {
        if (DateTime.UtcNow.Ticks >= targetTick)
          break;
        num = 0;
      }
      ++num;
    }
  }

  public override void Trace(PathNode node)
  {
    base.Trace(node);
    if (!this.inverted)
      return;
    int num = this.path.Count / 2;
    for (int index = 0; index < num; ++index)
    {
      GraphNode graphNode = this.path[index];
      this.path[index] = this.path[this.path.Count - index - 1];
      this.path[this.path.Count - index - 1] = graphNode;
    }
    for (int index = 0; index < num; ++index)
    {
      Vector3 vector3 = this.vectorPath[index];
      this.vectorPath[index] = this.vectorPath[this.vectorPath.Count - index - 1];
      this.vectorPath[this.vectorPath.Count - index - 1] = vector3;
    }
  }

  public override string DebugString(PathLog logMode)
  {
    if (logMode == PathLog.None || !this.error && logMode == PathLog.OnlyErrors)
      return "";
    StringBuilder debugStringBuilder = this.pathHandler.DebugStringBuilder;
    debugStringBuilder.Length = 0;
    this.DebugStringPrefix(logMode, debugStringBuilder);
    if (!this.error)
    {
      debugStringBuilder.Append("\nShortest path was ");
      StringBuilder stringBuilder1 = debugStringBuilder;
      int num;
      string str1;
      if (this.chosenTarget != -1)
      {
        num = this.nodePaths[this.chosenTarget].Count;
        str1 = num.ToString();
      }
      else
        str1 = "undefined";
      stringBuilder1.Append(str1);
      debugStringBuilder.Append(" nodes long");
      if (logMode == PathLog.Heavy)
      {
        debugStringBuilder.Append("\nPaths (").Append(this.targetsFound.Length).Append("):");
        Vector3 endPoint;
        for (int index = 0; index < this.targetsFound.Length; ++index)
        {
          debugStringBuilder.Append("\n\n\tPath ").Append(index).Append(" Found: ").Append(this.targetsFound[index]);
          if (this.nodePaths[index] != null)
          {
            debugStringBuilder.Append("\n\t\tLength: ");
            debugStringBuilder.Append(this.nodePaths[index].Count);
            if (this.nodePaths[index][this.nodePaths[index].Count - 1] != null)
            {
              PathNode pathNode = this.pathHandler.GetPathNode(this.endNode);
              if (pathNode != null)
              {
                debugStringBuilder.Append("\n\t\tEnd Node");
                debugStringBuilder.Append("\n\t\t\tG: ");
                debugStringBuilder.Append(pathNode.G);
                debugStringBuilder.Append("\n\t\t\tH: ");
                debugStringBuilder.Append(pathNode.H);
                debugStringBuilder.Append("\n\t\t\tF: ");
                debugStringBuilder.Append(pathNode.F);
                debugStringBuilder.Append("\n\t\t\tPoint: ");
                StringBuilder stringBuilder2 = debugStringBuilder;
                endPoint = this.endPoint;
                string str2 = endPoint.ToString();
                stringBuilder2.Append(str2);
                debugStringBuilder.Append("\n\t\t\tGraph: ");
                debugStringBuilder.Append(this.endNode.GraphIndex);
              }
              else
                debugStringBuilder.Append("\n\t\tEnd Node: Null");
            }
          }
        }
        debugStringBuilder.Append("\nStart Node");
        debugStringBuilder.Append("\n\tPoint: ");
        StringBuilder stringBuilder3 = debugStringBuilder;
        endPoint = this.endPoint;
        string str3 = endPoint.ToString();
        stringBuilder3.Append(str3);
        debugStringBuilder.Append("\n\tGraph: ");
        debugStringBuilder.Append(this.startNode.GraphIndex);
        debugStringBuilder.Append("\nBinary Heap size at completion: ");
        StringBuilder stringBuilder4 = debugStringBuilder;
        string str4;
        if (this.pathHandler.GetHeap() != null)
        {
          num = this.pathHandler.GetHeap().numberOfItems - 2;
          str4 = num.ToString();
        }
        else
          str4 = "Null";
        stringBuilder4.AppendLine(str4);
      }
    }
    this.DebugStringSuffix(logMode, debugStringBuilder);
    return debugStringBuilder.ToString();
  }

  public enum HeuristicMode
  {
    None,
    Average,
    MovingAverage,
    Midpoint,
    MovingMidpoint,
    Sequential,
  }
}
