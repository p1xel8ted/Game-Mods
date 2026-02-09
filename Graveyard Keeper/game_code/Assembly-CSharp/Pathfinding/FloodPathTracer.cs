// Decompiled with JetBrains decompiler
// Type: Pathfinding.FloodPathTracer
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Pathfinding;

public class FloodPathTracer : ABPath
{
  public FloodPath flood;

  public override bool hasEndPoint => false;

  public static FloodPathTracer Construct(Vector3 start, FloodPath flood, OnPathDelegate callback = null)
  {
    FloodPathTracer path = PathPool.GetPath<FloodPathTracer>();
    path.Setup(start, flood, callback);
    return path;
  }

  public void Setup(Vector3 start, FloodPath flood, OnPathDelegate callback)
  {
    this.flood = flood;
    if (flood == null || flood.GetState() < PathState.Returned)
      throw new ArgumentException("You must supply a calculated FloodPath to the 'flood' argument");
    this.Setup(start, flood.originalStartPoint, callback);
    this.nnConstraint = (NNConstraint) new FloodPathConstraint(flood);
  }

  public override void Reset()
  {
    base.Reset();
    this.flood = (FloodPath) null;
  }

  public override void Initialize()
  {
    if (this.startNode != null && this.flood.HasPathTo(this.startNode))
    {
      this.Trace(this.startNode);
      this.CompleteState = PathCompleteState.Complete;
    }
    else
    {
      this.Error();
      this.LogError("Could not find valid start node");
    }
  }

  public override void CalculateStep(long targetTick)
  {
    if (this.IsDone())
      return;
    this.Error();
    this.LogError("Something went wrong. At this point the path should be completed");
  }

  public void Trace(GraphNode from)
  {
    GraphNode node = from;
    int num = 0;
    while (node != null)
    {
      this.path.Add(node);
      this.vectorPath.Add((Vector3) node.position);
      node = this.flood.GetParent(node);
      ++num;
      if (num > 1024 /*0x0400*/)
      {
        Debug.LogWarning((object) "Inifinity loop? >1024 node path. Remove this message if you really have that long paths (FloodPathTracer.cs, Trace function)");
        break;
      }
    }
  }
}
