// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.AnyNotEqual
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("≠", 0)]
[Category("Logic Operators/Any")]
public class AnyNotEqual : PureFunctionNode<bool, object, object>
{
  public override bool Invoke(object a, object b) => !object.Equals(a, b);
}
