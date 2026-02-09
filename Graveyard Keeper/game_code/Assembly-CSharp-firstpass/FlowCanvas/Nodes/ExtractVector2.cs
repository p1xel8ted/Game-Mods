// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ExtractVector2
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

public class ExtractVector2 : ExtractorNode<Vector2, float, float>
{
  public override void Invoke(Vector2 vector, out float x, out float y)
  {
    x = vector.x;
    y = vector.y;
  }
}
