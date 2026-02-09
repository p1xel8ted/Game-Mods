// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.PureFunctionNode`2
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

#nullable disable
namespace FlowCanvas.Nodes;

public abstract class PureFunctionNode<TResult, T1> : PureFunctionNodeBase
{
  public abstract TResult Invoke(T1 a);

  public sealed override void OnRegisterPorts(FlowNode node)
  {
    ValueInput<T1> p1 = node.AddValueInput<T1>(this.GetParameterName(0));
    node.AddValueOutput<TResult>("Value", (ValueHandler<TResult>) (() => this.Invoke(p1.value)));
  }
}
