// Decompiled with JetBrains decompiler
// Type: Pathfinding.RichSpecial
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Pathfinding;

public class RichSpecial : RichPathPart
{
  public NodeLink2 nodeLink;
  public Transform first;
  public Transform second;
  public bool reverse;

  public override void OnEnterPool() => this.nodeLink = (NodeLink2) null;

  public RichSpecial Initialize(NodeLink2 nodeLink, GraphNode first)
  {
    this.nodeLink = nodeLink;
    if (first == nodeLink.startNode)
    {
      this.first = nodeLink.StartTransform;
      this.second = nodeLink.EndTransform;
      this.reverse = false;
    }
    else
    {
      this.first = nodeLink.EndTransform;
      this.second = nodeLink.StartTransform;
      this.reverse = true;
    }
    return this;
  }
}
