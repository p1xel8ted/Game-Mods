// Decompiled with JetBrains decompiler
// Type: Pathfinding.GraphHitInfo
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Pathfinding;

public struct GraphHitInfo(Vector3 point)
{
  public Vector3 origin = Vector3.zero;
  public Vector3 point = point;
  public GraphNode node = (GraphNode) null;
  public Vector3 tangentOrigin = Vector3.zero;
  public Vector3 tangent = Vector3.zero;

  public float distance => (this.point - this.origin).magnitude;
}
