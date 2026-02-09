// Decompiled with JetBrains decompiler
// Type: Pathfinding.ABPath
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Text;
using UnityEngine;

#nullable disable
namespace Pathfinding;

public class ABPath : Path
{
  public bool recalcStartEndCosts = true;
  public GraphNode startNode;
  public GraphNode endNode;
  public GraphNode startHint;
  public GraphNode endHint;
  public Vector3 originalStartPoint;
  public Vector3 originalEndPoint;
  public Vector3 startPoint;
  public Vector3 endPoint;
  public Int3 startIntPoint;
  public bool calculatePartial;
  public PathNode partialBestTarget;
  public int[] endNodeCosts;

  public virtual bool hasEndPoint => true;

  public static ABPath Construct(Vector3 start, Vector3 end, OnPathDelegate callback = null)
  {
    ABPath path = PathPool.GetPath<ABPath>();
    path.Setup(start, end, callback);
    return path;
  }

  public void Setup(Vector3 start, Vector3 end, OnPathDelegate callbackDelegate)
  {
    this.callback = callbackDelegate;
    this.UpdateStartEnd(start, end);
  }

  public void UpdateStartEnd(Vector3 start, Vector3 end)
  {
    this.originalStartPoint = start;
    this.originalEndPoint = end;
    this.startPoint = start;
    this.endPoint = end;
    this.startIntPoint = (Int3) start;
    this.hTarget = (Int3) end;
  }

  public override uint GetConnectionSpecialCost(GraphNode a, GraphNode b, uint currentCost)
  {
    if (this.startNode != null && this.endNode != null)
    {
      if (a == this.startNode)
      {
        Int3 int3 = this.startIntPoint - (b == this.endNode ? this.hTarget : b.position);
        double costMagnitude1 = (double) int3.costMagnitude;
        double num1 = (double) currentCost * 1.0;
        int3 = a.position - b.position;
        double costMagnitude2 = (double) int3.costMagnitude;
        double num2 = num1 / costMagnitude2;
        return (uint) (costMagnitude1 * num2);
      }
      if (b == this.startNode)
      {
        Int3 int3 = this.startIntPoint - (a == this.endNode ? this.hTarget : a.position);
        double costMagnitude3 = (double) int3.costMagnitude;
        double num3 = (double) currentCost * 1.0;
        int3 = a.position - b.position;
        double costMagnitude4 = (double) int3.costMagnitude;
        double num4 = num3 / costMagnitude4;
        return (uint) (costMagnitude3 * num4);
      }
      if (a == this.endNode)
        return (uint) ((double) (this.hTarget - b.position).costMagnitude * ((double) currentCost * 1.0 / (double) (a.position - b.position).costMagnitude));
      if (b == this.endNode)
        return (uint) ((double) (this.hTarget - a.position).costMagnitude * ((double) currentCost * 1.0 / (double) (a.position - b.position).costMagnitude));
    }
    else
    {
      if (a == this.startNode)
        return (uint) ((double) (this.startIntPoint - b.position).costMagnitude * ((double) currentCost * 1.0 / (double) (a.position - b.position).costMagnitude));
      if (b == this.startNode)
        return (uint) ((double) (this.startIntPoint - a.position).costMagnitude * ((double) currentCost * 1.0 / (double) (a.position - b.position).costMagnitude));
    }
    return currentCost;
  }

  public override void Reset()
  {
    base.Reset();
    this.startNode = (GraphNode) null;
    this.endNode = (GraphNode) null;
    this.startHint = (GraphNode) null;
    this.endHint = (GraphNode) null;
    this.originalStartPoint = Vector3.zero;
    this.originalEndPoint = Vector3.zero;
    this.startPoint = Vector3.zero;
    this.endPoint = Vector3.zero;
    this.calculatePartial = false;
    this.partialBestTarget = (PathNode) null;
    this.startIntPoint = new Int3();
    this.hTarget = new Int3();
    this.endNodeCosts = (int[]) null;
  }

  public override void Prepare()
  {
    this.nnConstraint.tags = this.enabledTags;
    NNInfo nearest1 = AstarPath.active.GetNearest(this.startPoint, this.nnConstraint, this.startHint);
    if (this.nnConstraint is PathNNConstraint nnConstraint)
      nnConstraint.SetStart(nearest1.node);
    this.startPoint = nearest1.clampedPosition;
    this.startIntPoint = (Int3) this.startPoint;
    this.startNode = nearest1.node;
    if (this.hasEndPoint)
    {
      NNInfo nearest2 = AstarPath.active.GetNearest(this.endPoint, this.nnConstraint, this.endHint);
      this.endPoint = nearest2.clampedPosition;
      this.hTarget = (Int3) this.endPoint;
      this.endNode = nearest2.node;
      this.hTargetNode = this.endNode;
    }
    if (this.startNode == null && this.hasEndPoint && this.endNode == null)
    {
      this.Error();
      this.LogError("Couldn't find close nodes to the start point or the end point");
    }
    else if (this.startNode == null)
    {
      this.Error();
      this.LogError("Couldn't find a close node to the start point");
    }
    else if (this.endNode == null && this.hasEndPoint)
    {
      this.Error();
      this.LogError("Couldn't find a close node to the end point");
    }
    else if (!this.startNode.Walkable)
    {
      this.Error();
      this.LogError("The node closest to the start point is not walkable");
    }
    else if (this.hasEndPoint && !this.endNode.Walkable)
    {
      this.Error();
      this.LogError("The node closest to the end point is not walkable");
    }
    else
    {
      if (!this.hasEndPoint || (int) this.startNode.Area == (int) this.endNode.Area)
        return;
      this.Error();
      this.LogError($"There is no valid path to the target (start area: {this.startNode.Area.ToString()}, target area: {this.endNode.Area.ToString()})");
    }
  }

  public virtual void CompletePathIfStartIsValidTarget()
  {
    if (!this.hasEndPoint || this.startNode != this.endNode)
      return;
    this.Trace(this.pathHandler.GetPathNode(this.startNode));
    this.CompleteState = PathCompleteState.Complete;
  }

  public override void Initialize()
  {
    if (this.startNode != null)
      this.pathHandler.GetPathNode(this.startNode).flag2 = true;
    if (this.endNode != null)
      this.pathHandler.GetPathNode(this.endNode).flag2 = true;
    PathNode pathNode = this.pathHandler.GetPathNode(this.startNode);
    pathNode.node = this.startNode;
    pathNode.pathID = this.pathHandler.PathID;
    pathNode.parent = (PathNode) null;
    pathNode.cost = 0U;
    pathNode.G = this.GetTraversalCost(this.startNode);
    pathNode.H = this.CalculateHScore(this.startNode);
    this.CompletePathIfStartIsValidTarget();
    if (this.CompleteState == PathCompleteState.Complete)
      return;
    this.startNode.Open((Path) this, pathNode, this.pathHandler);
    ++this.searchedNodes;
    this.partialBestTarget = pathNode;
    if (this.pathHandler.HeapEmpty())
    {
      if (this.calculatePartial)
      {
        this.CompleteState = PathCompleteState.Partial;
        this.Trace(this.partialBestTarget);
      }
      else
      {
        this.Error();
        this.LogError("No open points, the start node didn't open any nodes");
        return;
      }
    }
    this.currentR = this.pathHandler.PopNode();
  }

  public override void Cleanup()
  {
    if (this.startNode != null)
      this.pathHandler.GetPathNode(this.startNode).flag2 = false;
    if (this.endNode == null)
      return;
    this.pathHandler.GetPathNode(this.endNode).flag2 = false;
  }

  public override void CalculateStep(long targetTick)
  {
    int num = 0;
    while (this.CompleteState == PathCompleteState.NotCalculated)
    {
      ++this.searchedNodes;
      if (this.currentR.node == this.endNode)
      {
        this.CompleteState = PathCompleteState.Complete;
        break;
      }
      if (this.currentR.H < this.partialBestTarget.H)
        this.partialBestTarget = this.currentR;
      this.currentR.node.Open((Path) this, this.currentR, this.pathHandler);
      if (this.pathHandler.HeapEmpty())
      {
        this.Error();
        this.LogError("Searched whole area but could not find target");
        return;
      }
      this.currentR = this.pathHandler.PopNode();
      if (num > 500)
      {
        if (DateTime.UtcNow.Ticks >= targetTick)
          return;
        num = 0;
        if (this.searchedNodes > 1000000)
          throw new Exception("Probable infinite loop. Over 1,000,000 nodes searched");
      }
      ++num;
    }
    if (this.CompleteState == PathCompleteState.Complete)
    {
      this.Trace(this.currentR);
    }
    else
    {
      if (!this.calculatePartial || this.partialBestTarget == null)
        return;
      this.CompleteState = PathCompleteState.Partial;
      this.Trace(this.partialBestTarget);
    }
  }

  public void ResetCosts(Path p)
  {
  }

  public override string DebugString(PathLog logMode)
  {
    if (logMode == PathLog.None || !this.error && logMode == PathLog.OnlyErrors)
      return "";
    StringBuilder text = new StringBuilder();
    this.DebugStringPrefix(logMode, text);
    if (!this.error && logMode == PathLog.Heavy)
    {
      text.Append("\nSearch Iterations " + this.searchIterations.ToString());
      Vector3 vector3;
      if (this.hasEndPoint && this.endNode != null)
      {
        PathNode pathNode = this.pathHandler.GetPathNode(this.endNode);
        text.Append("\nEnd Node\n\tG: ");
        text.Append(pathNode.G);
        text.Append("\n\tH: ");
        text.Append(pathNode.H);
        text.Append("\n\tF: ");
        text.Append(pathNode.F);
        text.Append("\n\tPoint: ");
        StringBuilder stringBuilder = text;
        vector3 = this.endPoint;
        string str = vector3.ToString();
        stringBuilder.Append(str);
        text.Append("\n\tGraph: ");
        text.Append(this.endNode.GraphIndex);
      }
      text.Append("\nStart Node");
      text.Append("\n\tPoint: ");
      StringBuilder stringBuilder1 = text;
      vector3 = this.startPoint;
      string str1 = vector3.ToString();
      stringBuilder1.Append(str1);
      text.Append("\n\tGraph: ");
      if (this.startNode != null)
        text.Append(this.startNode.GraphIndex);
      else
        text.Append("< null startNode >");
    }
    this.DebugStringSuffix(logMode, text);
    return text.ToString();
  }

  public Vector3 GetMovementVector(Vector3 point)
  {
    if (this.vectorPath == null || this.vectorPath.Count == 0)
      return Vector3.zero;
    if (this.vectorPath.Count == 1)
      return this.vectorPath[0] - point;
    float num1 = float.PositiveInfinity;
    int num2 = 0;
    for (int index = 0; index < this.vectorPath.Count - 1; ++index)
    {
      float sqrMagnitude = (VectorMath.ClosestPointOnSegment(this.vectorPath[index], this.vectorPath[index + 1], point) - point).sqrMagnitude;
      if ((double) sqrMagnitude < (double) num1)
      {
        num1 = sqrMagnitude;
        num2 = index;
      }
    }
    return this.vectorPath[num2 + 1] - point;
  }
}
