// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.NewRect
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Obsolete]
[Category("Utilities/Constructors")]
public class NewRect : PureFunctionNode<Rect, float, float, float, float>
{
  public override Rect Invoke(float left, float top, float width, float height)
  {
    return new Rect(left, top, width, height);
  }
}
