// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.AnyLessThan
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Logic Operators/Any")]
[Name("<", 0)]
public class AnyLessThan : PureFunctionNode<bool, IComparable, IComparable>
{
  public override bool Invoke(IComparable a, IComparable b) => a.CompareTo((object) b) == -1;
}
