// Decompiled with JetBrains decompiler
// Type: Pathfinding.GridNodeBase
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Pathfinding;

public abstract class GridNodeBase : GraphNode
{
  public const int GridFlagsWalkableErosionOffset = 8;
  public const int GridFlagsWalkableErosionMask = 256 /*0x0100*/;
  public const int GridFlagsWalkableTmpOffset = 9;
  public const int GridFlagsWalkableTmpMask = 512 /*0x0200*/;
  public int nodeInGridIndex;
  public ushort gridFlags;
  public GraphNode[] connections;
  public uint[] connectionCosts;
  public static Version VERSION_3_8_3 = new Version(3, 8, 3);

  public GridNodeBase(AstarPath astar)
    : base(astar)
  {
  }

  public int NodeInGridIndex
  {
    get => this.nodeInGridIndex;
    set => this.nodeInGridIndex = value;
  }

  public bool WalkableErosion
  {
    get => ((uint) this.gridFlags & 256U /*0x0100*/) > 0U;
    set => this.gridFlags = (ushort) ((int) this.gridFlags & -257 | (value ? 256 /*0x0100*/ : 0));
  }

  public bool TmpWalkable
  {
    get => ((uint) this.gridFlags & 512U /*0x0200*/) > 0U;
    set => this.gridFlags = (ushort) ((int) this.gridFlags & -513 | (value ? 512 /*0x0200*/ : 0));
  }

  public override void FloodFill(Stack<GraphNode> stack, uint region)
  {
    if (this.connections == null)
      return;
    for (int index = 0; index < this.connections.Length; ++index)
    {
      GraphNode connection = this.connections[index];
      if ((int) connection.Area != (int) region)
      {
        connection.Area = region;
        stack.Push(connection);
      }
    }
  }

  public override void ClearConnections(bool alsoReverse)
  {
    if (alsoReverse && this.connections != null)
    {
      for (int index = 0; index < this.connections.Length; ++index)
        this.connections[index].RemoveConnection((GraphNode) this);
    }
    this.connections = (GraphNode[]) null;
    this.connectionCosts = (uint[]) null;
  }

  public override bool ContainsConnection(GraphNode node)
  {
    if (this.connections != null)
    {
      for (int index = 0; index < this.connections.Length; ++index)
      {
        if (this.connections[index] == node)
          return true;
      }
    }
    return false;
  }

  public override void GetConnections(GraphNodeDelegate del)
  {
    if (this.connections == null)
      return;
    for (int index = 0; index < this.connections.Length; ++index)
      del(this.connections[index]);
  }

  public override void UpdateRecursiveG(Path path, PathNode pathNode, PathHandler handler)
  {
    ushort pathId = handler.PathID;
    if (this.connections == null)
      return;
    for (int index = 0; index < this.connections.Length; ++index)
    {
      GraphNode connection = this.connections[index];
      PathNode pathNode1 = handler.GetPathNode(connection);
      if (pathNode1.parent == pathNode && (int) pathNode1.pathID == (int) pathId)
        connection.UpdateRecursiveG(path, pathNode1, handler);
    }
  }

  public override void Open(Path path, PathNode pathNode, PathHandler handler)
  {
    ushort pathId = handler.PathID;
    if (this.connections == null)
      return;
    for (int index = 0; index < this.connections.Length; ++index)
    {
      GraphNode connection = this.connections[index];
      if (path.CanTraverse(connection))
      {
        PathNode pathNode1 = handler.GetPathNode(connection);
        uint connectionCost = this.connectionCosts[index];
        if ((int) pathNode1.pathID != (int) pathId)
        {
          pathNode1.parent = pathNode;
          pathNode1.pathID = pathId;
          pathNode1.cost = connectionCost;
          pathNode1.H = path.CalculateHScore(connection);
          connection.UpdateG(path, pathNode1);
          handler.PushNode(pathNode1);
        }
        else if (pathNode.G + connectionCost + path.GetTraversalCost(connection) < pathNode1.G)
        {
          pathNode1.cost = connectionCost;
          pathNode1.parent = pathNode;
          connection.UpdateRecursiveG(path, pathNode1, handler);
        }
        else if (pathNode1.G + connectionCost + path.GetTraversalCost((GraphNode) this) < pathNode.G && connection.ContainsConnection((GraphNode) this))
        {
          pathNode.parent = pathNode1;
          pathNode.cost = connectionCost;
          this.UpdateRecursiveG(path, pathNode, handler);
        }
      }
    }
  }

  public override void AddConnection(GraphNode node, uint cost)
  {
    if (node == null)
      throw new ArgumentNullException();
    if (this.connections != null)
    {
      for (int index = 0; index < this.connections.Length; ++index)
      {
        if (this.connections[index] == node)
        {
          this.connectionCosts[index] = cost;
          return;
        }
      }
    }
    int length = this.connections != null ? this.connections.Length : 0;
    GraphNode[] graphNodeArray = new GraphNode[length + 1];
    uint[] numArray = new uint[length + 1];
    for (int index = 0; index < length; ++index)
    {
      graphNodeArray[index] = this.connections[index];
      numArray[index] = this.connectionCosts[index];
    }
    graphNodeArray[length] = node;
    numArray[length] = cost;
    this.connections = graphNodeArray;
    this.connectionCosts = numArray;
  }

  public override void RemoveConnection(GraphNode node)
  {
    if (this.connections == null)
      return;
    for (int index1 = 0; index1 < this.connections.Length; ++index1)
    {
      if (this.connections[index1] == node)
      {
        int length = this.connections.Length;
        GraphNode[] graphNodeArray = new GraphNode[length - 1];
        uint[] numArray = new uint[length - 1];
        for (int index2 = 0; index2 < index1; ++index2)
        {
          graphNodeArray[index2] = this.connections[index2];
          numArray[index2] = this.connectionCosts[index2];
        }
        for (int index3 = index1 + 1; index3 < length; ++index3)
        {
          graphNodeArray[index3 - 1] = this.connections[index3];
          numArray[index3 - 1] = this.connectionCosts[index3];
        }
        this.connections = graphNodeArray;
        this.connectionCosts = numArray;
        break;
      }
    }
  }

  public override void SerializeReferences(GraphSerializationContext ctx)
  {
    if (this.connections == null)
    {
      ctx.writer.Write(-1);
    }
    else
    {
      ctx.writer.Write(this.connections.Length);
      for (int index = 0; index < this.connections.Length; ++index)
      {
        ctx.SerializeNodeReference(this.connections[index]);
        ctx.writer.Write(this.connectionCosts[index]);
      }
    }
  }

  public override void DeserializeReferences(GraphSerializationContext ctx)
  {
    if (ctx.meta.version < GridNodeBase.VERSION_3_8_3)
      return;
    int length = ctx.reader.ReadInt32();
    if (length == -1)
    {
      this.connections = (GraphNode[]) null;
      this.connectionCosts = (uint[]) null;
    }
    else
    {
      this.connections = new GraphNode[length];
      this.connectionCosts = new uint[length];
      for (int index = 0; index < length; ++index)
      {
        this.connections[index] = ctx.DeserializeNodeReference();
        this.connectionCosts[index] = ctx.reader.ReadUInt32();
      }
    }
  }
}
