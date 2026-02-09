// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Identity`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Use this for organization. It returns exactly what is provided in the input.")]
[Name("Identity", 10)]
[ExposeAsDefinition]
[Category("Utility")]
public class Identity<T> : PureFunctionNode<T, T>
{
  public override string name => (string) null;

  public override T Invoke(T value) => value;
}
