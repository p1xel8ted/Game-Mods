// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Cache`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Utility")]
[ExposeAsDefinition]
[Description("Caches the value only when the node is called.")]
[Name("Cache", 9)]
public class Cache<T> : CallableFunctionNode<T, T>
{
  public override T Invoke(T value) => value;
}
