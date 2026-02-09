// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ExtractColor
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

public class ExtractColor : ExtractorNode<Color, float, float, float, float>
{
  public override void Invoke(Color color, out float r, out float g, out float b, out float a)
  {
    r = color.r;
    g = color.g;
    b = color.b;
    a = color.a;
  }
}
