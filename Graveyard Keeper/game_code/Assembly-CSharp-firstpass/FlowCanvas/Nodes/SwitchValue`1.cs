// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.SwitchValue`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[ExposeAsDefinition]
[Name("Switch", 8)]
[Category("Utility")]
[Description("Returns either one of the two inputs, based on the boolean condition")]
public class SwitchValue<T> : PureFunctionNode<T, bool, T, T>
{
  public override T Invoke(bool condition, T isTrue, T isFalse) => !condition ? isFalse : isTrue;
}
