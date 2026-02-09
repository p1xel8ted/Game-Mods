// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.NewColor
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Utilities/Constructors")]
[Obsolete]
public class NewColor : PureFunctionNode<Color, float, float, float, float>
{
  public override Color Invoke(float r, float g, float b, float a = 1f) => new Color(r, g, b, a);
}
