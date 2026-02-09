// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ExtractRect
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

public class ExtractRect : ExtractorNode<Rect, Vector2, float, float, float, float>
{
  public override void Invoke(
    Rect rect,
    out Vector2 center,
    out float xMin,
    out float xMax,
    out float yMin,
    out float yMax)
  {
    center = rect.center;
    xMin = rect.xMin;
    xMax = rect.xMax;
    yMin = rect.yMin;
    yMax = rect.yMax;
  }
}
