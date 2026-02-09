// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.PureFunctionNode`5
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

#nullable disable
namespace FlowCanvas.Nodes;

public abstract class PureFunctionNode<TResult, T1, T2, T3, T4> : PureFunctionNodeBase
{
  public abstract TResult Invoke(T1 a, T2 b, T3 c, T4 d);

  public sealed override void OnRegisterPorts(FlowNode node)
  {
    ValueInput<T1> p1 = node.AddValueInput<T1>(this.GetParameterName(0));
    ValueInput<T2> p2 = node.AddValueInput<T2>(this.GetParameterName(1));
    ValueInput<T3> p3 = node.AddValueInput<T3>(this.GetParameterName(2));
    ValueInput<T4> p4 = node.AddValueInput<T4>(this.GetParameterName(3));
    node.AddValueOutput<TResult>("Value", (ValueHandler<TResult>) (() => this.Invoke(p1.value, p2.value, p3.value, p4.value)));
  }
}
