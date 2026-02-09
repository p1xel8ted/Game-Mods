// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.NewVector2
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
public class NewVector2 : PureFunctionNode<Vector2, float, float>
{
  public override Vector2 Invoke(float x, float y) => new Vector2(x, y);
}
