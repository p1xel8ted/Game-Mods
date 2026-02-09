// Decompiled with JetBrains decompiler
// Type: Pathfinding.GraphUpdateObject
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Pathfinding;

public class GraphUpdateObject
{
  public Bounds bounds;
  public int only_specific_graph = -1;
  public bool requiresFloodFill = true;
  public bool updatePhysics = true;
  public bool resetPenaltyOnPhysics = true;
  public bool updateErosion = true;
  public NNConstraint nnConstraint = NNConstraint.None;
  public int addPenalty;
  public bool modifyWalkability;
  public bool setWalkability;
  public bool modifyTag;
  public int setTag;
  public bool trackChangedNodes;
  public List<GraphNode> changedNodes;
  public List<uint> backupData;
  public List<Int3> backupPositionData;
  public GraphUpdateShape shape;

  public virtual void WillUpdateNode(GraphNode node)
  {
    if (!this.trackChangedNodes || node == null)
      return;
    if (this.changedNodes == null)
    {
      this.changedNodes = ListPool<GraphNode>.Claim();
      this.backupData = ListPool<uint>.Claim();
      this.backupPositionData = ListPool<Int3>.Claim();
    }
    this.changedNodes.Add(node);
    this.backupPositionData.Add(node.position);
    this.backupData.Add(node.Penalty);
    this.backupData.Add(node.Flags);
    if (!(node is GridNode gridNode))
      return;
    this.backupData.Add((uint) gridNode.InternalGridFlags);
  }

  public virtual void RevertFromBackup()
  {
    if (!this.trackChangedNodes)
      throw new InvalidOperationException("Changed nodes have not been tracked, cannot revert from backup");
    if (this.changedNodes == null)
      return;
    int index1 = 0;
    for (int index2 = 0; index2 < this.changedNodes.Count; ++index2)
    {
      this.changedNodes[index2].Penalty = this.backupData[index1];
      int index3 = index1 + 1;
      this.changedNodes[index2].Flags = this.backupData[index3];
      index1 = index3 + 1;
      if (this.changedNodes[index2] is GridNode changedNode)
      {
        changedNode.InternalGridFlags = (ushort) this.backupData[index1];
        ++index1;
      }
      this.changedNodes[index2].position = this.backupPositionData[index2];
    }
    ListPool<GraphNode>.Release(this.changedNodes);
    ListPool<uint>.Release(this.backupData);
    ListPool<Int3>.Release(this.backupPositionData);
  }

  public virtual void Apply(GraphNode node)
  {
    if (this.shape != null && !this.shape.Contains(node))
      return;
    node.Penalty = (uint) ((ulong) node.Penalty + (ulong) this.addPenalty);
    if (this.modifyWalkability)
      node.Walkable = this.setWalkability;
    if (!this.modifyTag)
      return;
    node.Tag = (uint) this.setTag;
  }

  public GraphUpdateObject()
  {
  }

  public GraphUpdateObject(Bounds b) => this.bounds = b;
}
