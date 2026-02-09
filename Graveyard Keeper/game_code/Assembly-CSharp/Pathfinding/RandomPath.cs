// Decompiled with JetBrains decompiler
// Type: Pathfinding.RandomPath
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Pathfinding;

public class RandomPath : ABPath
{
  public int searchLength;
  public int spread = 5000;
  public float aimStrength;
  public PathNode chosenNodeR;
  public PathNode maxGScoreNodeR;
  public int maxGScore;
  public Vector3 aim;
  public int nodesEvaluatedRep;
  public System.Random rnd = new System.Random();

  public override bool FloodingPath => true;

  public override bool hasEndPoint => false;

  public override void Reset()
  {
    base.Reset();
    this.searchLength = 5000;
    this.spread = 5000;
    this.aimStrength = 0.0f;
    this.chosenNodeR = (PathNode) null;
    this.maxGScoreNodeR = (PathNode) null;
    this.maxGScore = 0;
    this.aim = Vector3.zero;
    this.nodesEvaluatedRep = 0;
  }

  public RandomPath()
  {
  }

  [Obsolete("This constructor is obsolete. Please use the pooling API and the Construct methods")]
  public RandomPath(Vector3 start, int length, OnPathDelegate callback = null)
  {
    throw new Exception("This constructor is obsolete. Please use the pooling API and the Setup methods");
  }

  public static RandomPath Construct(Vector3 start, int length, OnPathDelegate callback = null)
  {
    RandomPath path = PathPool.GetPath<RandomPath>();
    path.Setup(start, length, callback);
    return path;
  }

  public RandomPath Setup(Vector3 start, int length, OnPathDelegate callback)
  {
    this.callback = callback;
    this.searchLength = length;
    this.originalStartPoint = start;
    this.originalEndPoint = Vector3.zero;
    this.startPoint = start;
    this.endPoint = Vector3.zero;
    this.startIntPoint = (Int3) start;
    return this;
  }

  public override void ReturnPath()
  {
    if (this.path != null && this.path.Count > 0)
    {
      this.endNode = this.path[this.path.Count - 1];
      this.endPoint = (Vector3) this.endNode.position;
      this.originalEndPoint = this.endPoint;
      this.hTarget = this.endNode.position;
    }
    if (this.callback == null)
      return;
    this.callback((Path) this);
  }

  public override void Prepare()
  {
    this.nnConstraint.tags = this.enabledTags;
    NNInfo nearest = AstarPath.active.GetNearest(this.startPoint, this.nnConstraint, this.startHint);
    this.startPoint = nearest.clampedPosition;
    this.endPoint = this.startPoint;
    this.startIntPoint = (Int3) this.startPoint;
    this.hTarget = (Int3) this.aim;
    this.startNode = nearest.node;
    this.endNode = this.startNode;
    if (this.startNode == null || this.endNode == null)
    {
      this.LogError("Couldn't find close nodes to the start point");
      this.Error();
    }
    else if (!this.startNode.Walkable)
    {
      this.LogError("The node closest to the start point is not walkable");
      this.Error();
    }
    else
      this.heuristicScale = this.aimStrength;
  }

  public override void Initialize()
  {
    PathNode pathNode = this.pathHandler.GetPathNode(this.startNode);
    pathNode.node = this.startNode;
    if (this.searchLength + this.spread <= 0)
    {
      this.CompleteState = PathCompleteState.Complete;
      this.Trace(pathNode);
    }
    else
    {
      pathNode.pathID = this.pathID;
      pathNode.parent = (PathNode) null;
      pathNode.cost = 0U;
      pathNode.G = this.GetTraversalCost(this.startNode);
      pathNode.H = this.CalculateHScore(this.startNode);
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

  public override void CalculateStep(long targetTick)
  {
    int num = 0;
    while (this.CompleteState == PathCompleteState.NotCalculated)
    {
      ++this.searchedNodes;
      if ((long) this.currentR.G >= (long) this.searchLength)
      {
        if ((long) this.currentR.G <= (long) (this.searchLength + this.spread))
        {
          ++this.nodesEvaluatedRep;
          if (this.rnd.NextDouble() <= 1.0 / (double) this.nodesEvaluatedRep)
            this.chosenNodeR = this.currentR;
        }
        else
        {
          if (this.chosenNodeR == null)
            this.chosenNodeR = this.currentR;
          this.CompleteState = PathCompleteState.Complete;
          break;
        }
      }
      else if ((long) this.currentR.G > (long) this.maxGScore)
      {
        this.maxGScore = (int) this.currentR.G;
        this.maxGScoreNodeR = this.currentR;
      }
      this.currentR.node.Open((Path) this, this.currentR, this.pathHandler);
      if (this.pathHandler.HeapEmpty())
      {
        if (this.chosenNodeR != null)
        {
          this.CompleteState = PathCompleteState.Complete;
          break;
        }
        if (this.maxGScoreNodeR != null)
        {
          this.chosenNodeR = this.maxGScoreNodeR;
          this.CompleteState = PathCompleteState.Complete;
          break;
        }
        this.LogError("Not a single node found to search");
        this.Error();
        break;
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
    if (this.CompleteState != PathCompleteState.Complete)
      return;
    this.Trace(this.chosenNodeR);
  }
}
