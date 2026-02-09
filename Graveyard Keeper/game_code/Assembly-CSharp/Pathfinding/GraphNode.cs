// Decompiled with JetBrains decompiler
// Type: Pathfinding.GraphNode
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Pathfinding;

public abstract class GraphNode
{
  public int nodeIndex;
  public uint flags;
  public uint penalty;
  public Int3 position;
  public const int FlagsWalkableOffset = 0;
  public const uint FlagsWalkableMask = 1;
  public const int FlagsAreaOffset = 1;
  public const uint FlagsAreaMask = 262142;
  public const int FlagsGraphOffset = 24;
  public const uint FlagsGraphMask = 4278190080 /*0xFF000000*/;
  public const uint MaxAreaIndex = 131071 /*0x01FFFF*/;
  public const uint MaxGraphIndex = 255 /*0xFF*/;
  public const int FlagsTagOffset = 19;
  public const uint FlagsTagMask = 16252928 /*0xF80000*/;

  public GraphNode(AstarPath astar)
  {
    this.nodeIndex = astar != null ? astar.GetNewNodeIndex() : throw new Exception("No active AstarPath object to bind to");
    astar.InitializeNode(this);
  }

  public void Destroy()
  {
    if (this.Destroyed)
      return;
    this.ClearConnections(true);
    if ((UnityEngine.Object) AstarPath.active != (UnityEngine.Object) null)
      AstarPath.active.DestroyNode(this);
    this.nodeIndex = -1;
  }

  public bool Destroyed => this.nodeIndex == -1;

  public int NodeIndex => this.nodeIndex;

  public uint Flags
  {
    get => this.flags;
    set => this.flags = value;
  }

  public uint Penalty
  {
    get => this.penalty;
    set
    {
      if (value > 16777215U /*0xFFFFFF*/)
        Debug.LogWarning((object) ("Very high penalty applied. Are you sure negative values haven't underflowed?\nPenalty values this high could with long paths cause overflows and in some cases infinity loops because of that.\nPenalty value applied: " + value.ToString()));
      this.penalty = value;
    }
  }

  public bool Walkable
  {
    get => (this.flags & 1U) > 0U;
    set => this.flags = (uint) ((int) this.flags & -2 | (value ? 1 : 0));
  }

  public uint Area
  {
    get => (this.flags & 262142U) >> 1;
    set => this.flags = (uint) ((int) this.flags & -262143 | (int) value << 1);
  }

  public uint GraphIndex
  {
    get => (this.flags & 4278190080U /*0xFF000000*/) >> 24;
    set => this.flags = (uint) ((int) this.flags & 16777215 /*0xFFFFFF*/ | (int) value << 24);
  }

  public uint Tag
  {
    get => (this.flags & 16252928U /*0xF80000*/) >> 19;
    set => this.flags = (uint) ((int) this.flags & -16252929 | (int) value << 19);
  }

  public void UpdateG(Path path, PathNode pathNode)
  {
    pathNode.G = pathNode.parent.G + pathNode.cost + path.GetTraversalCost(this);
  }

  public virtual void UpdateRecursiveG(Path path, PathNode pathNode, PathHandler handler)
  {
    this.UpdateG(path, pathNode);
    handler.PushNode(pathNode);
    this.GetConnections((GraphNodeDelegate) (other =>
    {
      PathNode pathNode1 = handler.GetPathNode(other);
      if (pathNode1.parent != pathNode || (int) pathNode1.pathID != (int) handler.PathID)
        return;
      other.UpdateRecursiveG(path, pathNode1, handler);
    }));
  }

  public virtual void FloodFill(Stack<GraphNode> stack, uint region)
  {
    this.GetConnections((GraphNodeDelegate) (other =>
    {
      if ((int) other.Area == (int) region)
        return;
      other.Area = region;
      stack.Push(other);
    }));
  }

  public abstract void GetConnections(GraphNodeDelegate del);

  public abstract void AddConnection(GraphNode node, uint cost);

  public abstract void RemoveConnection(GraphNode node);

  public abstract void ClearConnections(bool alsoReverse);

  public virtual bool ContainsConnection(GraphNode node)
  {
    bool contains = false;
    this.GetConnections((GraphNodeDelegate) (neighbour => contains |= neighbour == node));
    return contains;
  }

  public virtual void RecalculateConnectionCosts()
  {
  }

  public virtual bool GetPortal(
    GraphNode other,
    List<Vector3> left,
    List<Vector3> right,
    bool backwards)
  {
    return false;
  }

  public abstract void Open(Path path, PathNode pathNode, PathHandler handler);

  public virtual void SerializeNode(GraphSerializationContext ctx)
  {
    ctx.writer.Write(this.Penalty);
    ctx.writer.Write(this.Flags);
  }

  public virtual void DeserializeNode(GraphSerializationContext ctx)
  {
    this.Penalty = ctx.reader.ReadUInt32();
    this.Flags = ctx.reader.ReadUInt32();
    this.GraphIndex = ctx.graphIndex;
  }

  public virtual void SerializeReferences(GraphSerializationContext ctx)
  {
  }

  public virtual void DeserializeReferences(GraphSerializationContext ctx)
  {
  }
}
