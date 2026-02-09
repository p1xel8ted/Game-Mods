// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Vector3Invert
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Inverts the input ( value = value * -1 )")]
[Name("Invert", 0)]
[Category("Logic Operators/Vector3")]
public class Vector3Invert : PureFunctionNode<Vector3, Vector3>
{
  public override Vector3 Invoke(Vector3 value) => value * -1f;
}
