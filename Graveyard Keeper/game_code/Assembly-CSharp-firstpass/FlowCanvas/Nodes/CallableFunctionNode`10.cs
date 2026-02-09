// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.CallableFunctionNode`10
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

#nullable disable
namespace FlowCanvas.Nodes;

public abstract class CallableFunctionNode<TResult, T1, T2, T3, T4, T5, T6, T7, T8, T9> : 
  CallableFunctionNodeBase
{
  public TResult result;

  public abstract TResult Invoke(T1 a, T2 b, T3 c, T4 d, T5 e, T6 f, T7 g, T8 h, T9 i);

  public sealed override void OnRegisterPorts(FlowNode node)
  {
    FlowOutput o = node.AddFlowOutput(" ");
    ValueInput<T1> p1 = node.AddValueInput<T1>(this.parameters[0].Name);
    ValueInput<T2> p2 = node.AddValueInput<T2>(this.parameters[1].Name);
    ValueInput<T3> p3 = node.AddValueInput<T3>(this.parameters[2].Name);
    ValueInput<T4> p4 = node.AddValueInput<T4>(this.parameters[3].Name);
    ValueInput<T5> p5 = node.AddValueInput<T5>(this.parameters[4].Name);
    ValueInput<T6> p6 = node.AddValueInput<T6>(this.parameters[5].Name);
    ValueInput<T7> p7 = node.AddValueInput<T7>(this.parameters[6].Name);
    ValueInput<T8> p8 = node.AddValueInput<T8>(this.parameters[7].Name);
    ValueInput<T9> p9 = node.AddValueInput<T9>(this.parameters[8].Name);
    node.AddValueOutput<TResult>("Value", (ValueHandler<TResult>) (() => this.result));
    node.AddFlowInput(" ", (FlowHandler) (f =>
    {
      this.result = this.Invoke(p1.value, p2.value, p3.value, p4.value, p5.value, p6.value, p7.value, p8.value, p9.value);
      o.Call(f);
    }));
  }
}
