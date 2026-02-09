// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Vector3Add
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("+", 0)]
[Category("Logic Operators/Vector3")]
public class Vector3Add : PureFunctionNode<Vector3, Vector3, Vector3>
{
  public override Vector3 Invoke(Vector3 a, Vector3 b) => a + b;
}
