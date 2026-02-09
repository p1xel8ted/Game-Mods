// Decompiled with JetBrains decompiler
// Type: Pathfinding.NodeLink3Node
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Pathfinding;

public class NodeLink3Node(AstarPath active) : PointNode(active)
{
  public NodeLink3 link;
  public Vector3 portalA;
  public Vector3 portalB;

  public override bool GetPortal(
    GraphNode other,
    List<Vector3> left,
    List<Vector3> right,
    bool backwards)
  {
    if (this.connections.Length < 2)
      return false;
    if (this.connections.Length != 2)
      throw new Exception("Invalid NodeLink3Node. Expected 2 connections, found " + this.connections.Length.ToString());
    if (left != null)
    {
      left.Add(this.portalA);
      right.Add(this.portalB);
    }
    return true;
  }

  public GraphNode GetOther(GraphNode a)
  {
    if (this.connections.Length < 2)
      return (GraphNode) null;
    if (this.connections.Length != 2)
      throw new Exception("Invalid NodeLink3Node. Expected 2 connections, found " + this.connections.Length.ToString());
    return a != this.connections[0] ? (this.connections[0] as NodeLink3Node).GetOtherInternal((GraphNode) this) : (this.connections[1] as NodeLink3Node).GetOtherInternal((GraphNode) this);
  }

  public GraphNode GetOtherInternal(GraphNode a)
  {
    if (this.connections.Length < 2)
      return (GraphNode) null;
    return a != this.connections[0] ? this.connections[0] : this.connections[1];
  }
}
