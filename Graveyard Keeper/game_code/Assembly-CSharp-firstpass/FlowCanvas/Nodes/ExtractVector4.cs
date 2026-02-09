// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ExtractVector4
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

public class ExtractVector4 : ExtractorNode<Vector4, float, float, float, float>
{
  public override void Invoke(Vector4 vector, out float x, out float y, out float z, out float w)
  {
    x = vector.x;
    y = vector.y;
    z = vector.z;
    w = vector.w;
  }
}
