// Decompiled with JetBrains decompiler
// Type: Pathfinding.IRaycastableGraph
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Pathfinding;

public interface IRaycastableGraph
{
  bool Linecast(Vector3 start, Vector3 end);

  bool Linecast(Vector3 start, Vector3 end, GraphNode hint);

  bool Linecast(Vector3 start, Vector3 end, GraphNode hint, out GraphHitInfo hit);

  bool Linecast(
    Vector3 start,
    Vector3 end,
    GraphNode hint,
    out GraphHitInfo hit,
    List<GraphNode> trace);
}
