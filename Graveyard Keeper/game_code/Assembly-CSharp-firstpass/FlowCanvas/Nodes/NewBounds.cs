// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.NewBounds
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
public class NewBounds : PureFunctionNode<Bounds, Vector3, Vector3>
{
  public override Bounds Invoke(Vector3 center, Vector3 size) => new Bounds(center, size);
}
