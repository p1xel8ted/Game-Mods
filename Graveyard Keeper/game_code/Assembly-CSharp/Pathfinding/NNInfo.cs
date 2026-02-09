// Decompiled with JetBrains decompiler
// Type: Pathfinding.NNInfo
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Pathfinding;

public struct NNInfo
{
  public GraphNode node;
  public GraphNode constrainedNode;
  public Vector3 clampedPosition;
  public Vector3 constClampedPosition;

  public NNInfo(GraphNode node)
  {
    this.node = node;
    this.constrainedNode = (GraphNode) null;
    this.clampedPosition = Vector3.zero;
    this.constClampedPosition = Vector3.zero;
    this.UpdateInfo();
  }

  public void SetConstrained(GraphNode constrainedNode, Vector3 clampedPosition)
  {
    this.constrainedNode = constrainedNode;
    this.constClampedPosition = clampedPosition;
  }

  public void UpdateInfo()
  {
    this.clampedPosition = this.node != null ? (Vector3) this.node.position : Vector3.zero;
    this.constClampedPosition = this.constrainedNode != null ? (Vector3) this.constrainedNode.position : Vector3.zero;
  }

  public static explicit operator Vector3(NNInfo ob) => ob.clampedPosition;

  public static explicit operator GraphNode(NNInfo ob) => ob.node;

  public static explicit operator NNInfo(GraphNode ob) => new NNInfo(ob);
}
