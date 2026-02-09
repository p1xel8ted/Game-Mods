// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ExtractBounds
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

public class ExtractBounds : ExtractorNode<Bounds, Vector3, Vector3, Vector3, Vector3, Vector3>
{
  public override void Invoke(
    Bounds bounds,
    out Vector3 center,
    out Vector3 extents,
    out Vector3 max,
    out Vector3 min,
    out Vector3 size)
  {
    center = bounds.center;
    extents = bounds.extents;
    max = bounds.max;
    min = bounds.min;
    size = bounds.size;
  }
}
