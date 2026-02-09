// Decompiled with JetBrains decompiler
// Type: Pathfinding.XPath
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Pathfinding;

public class XPath : ABPath
{
  public PathEndingCondition endingCondition;

  public static XPath Construct(Vector3 start, Vector3 end, OnPathDelegate callback = null)
  {
    XPath path = PathPool.GetPath<XPath>();
    path.Setup(start, end, callback);
    path.endingCondition = (PathEndingCondition) new ABPathEndingCondition((ABPath) path);
    return path;
  }

  public override void Reset()
  {
    base.Reset();
    this.endingCondition = (PathEndingCondition) null;
  }

  public override void CompletePathIfStartIsValidTarget()
  {
    PathNode pathNode = this.pathHandler.GetPathNode(this.startNode);
    if (!this.endingCondition.TargetFound(pathNode))
      return;
    this.endNode = pathNode.node;
    this.endPoint = (Vector3) this.endNode.position;
    this.Trace(pathNode);
    this.CompleteState = PathCompleteState.Complete;
  }

  public override void CalculateStep(long targetTick)
  {
    int num = 0;
    while (this.CompleteState == PathCompleteState.NotCalculated)
    {
      ++this.searchedNodes;
      if (this.endingCondition.TargetFound(this.currentR))
      {
        this.CompleteState = PathCompleteState.Complete;
        break;
      }
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
    if (this.CompleteState != PathCompleteState.Complete)
      return;
    this.endNode = this.currentR.node;
    this.endPoint = (Vector3) this.endNode.position;
    this.Trace(this.currentR);
  }
}
