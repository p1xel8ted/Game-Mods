// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.PureFunctionNode`11
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

#nullable disable
namespace FlowCanvas.Nodes;

public abstract class PureFunctionNode<TResult, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : 
  PureFunctionNodeBase
{
  public abstract TResult Invoke(
    T1 a,
    T2 b,
    T3 c,
    T4 d,
    T5 e,
    T6 f,
    T7 g,
    T8 h,
    T9 i,
    T10 j);

  public sealed override void OnRegisterPorts(FlowNode node)
  {
    ValueInput<T1> p1 = node.AddValueInput<T1>(this.GetParameterName(0));
    ValueInput<T2> p2 = node.AddValueInput<T2>(this.GetParameterName(1));
    ValueInput<T3> p3 = node.AddValueInput<T3>(this.GetParameterName(2));
    ValueInput<T4> p4 = node.AddValueInput<T4>(this.GetParameterName(3));
    ValueInput<T5> p5 = node.AddValueInput<T5>(this.GetParameterName(4));
    ValueInput<T6> p6 = node.AddValueInput<T6>(this.GetParameterName(5));
    ValueInput<T7> p7 = node.AddValueInput<T7>(this.GetParameterName(6));
    ValueInput<T8> p8 = node.AddValueInput<T8>(this.GetParameterName(7));
    ValueInput<T9> p9 = node.AddValueInput<T9>(this.GetParameterName(8));
    ValueInput<T10> p10 = node.AddValueInput<T10>(this.GetParameterName(9));
    node.AddValueOutput<TResult>("Value", (ValueHandler<TResult>) (() => this.Invoke(p1.value, p2.value, p3.value, p4.value, p5.value, p6.value, p7.value, p8.value, p9.value, p10.value)));
  }
}
