// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Legacy.ReflectedFunctionNode`7
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;
using System.Reflection;

#nullable disable
namespace FlowCanvas.Nodes.Legacy;

public sealed class ReflectedFunctionNode<T1, T2, T3, T4, T5, T6, TResult> : ReflectedMethodNode
{
  public ReflectedMethodNode.FunctionCall<T1, T2, T3, T4, T5, T6, TResult> call;
  public TResult returnValue;
  public T1 instance;

  public TResult Call(T1 a, T2 b, T3 c, T4 d, T5 e, T6 f)
  {
    this.instance = a;
    return this.returnValue = this.call(a, b, c, d, e, f);
  }

  public override void RegisterPorts(
    FlowNode node,
    MethodInfo method,
    ReflectedMethodRegistrationOptions options)
  {
    this.call = method.RTCreateDelegate<ReflectedMethodNode.FunctionCall<T1, T2, T3, T4, T5, T6, TResult>>((object) null);
    ValueInput<T1> p1 = node.AddValueInput<T1>(this.GetName(method, 0));
    ValueInput<T2> p2 = node.AddValueInput<T2>(this.GetName(method, 1));
    ValueInput<T3> p3 = node.AddValueInput<T3>(this.GetName(method, 2));
    ValueInput<T4> p4 = node.AddValueInput<T4>(this.GetName(method, 3));
    ValueInput<T5> p5 = node.AddValueInput<T5>(this.GetName(method, 4));
    ValueInput<T6> p6 = node.AddValueInput<T6>(this.GetName(method, 5));
    if (options.callable)
    {
      FlowOutput o = node.AddFlowOutput(" ");
      node.AddFlowInput(" ", (FlowHandler) (f =>
      {
        this.Call(p1.value, p2.value, p3.value, p4.value, p5.value, p6.value);
        o.Call(f);
      }));
      if (!method.IsStatic)
        node.AddValueOutput<T1>(this.GetName(method, 0), (ValueHandler<T1>) (() => this.instance));
    }
    node.AddValueOutput<TResult>("Value", (ValueHandler<TResult>) (() => !options.callable ? this.Call(p1.value, p2.value, p3.value, p4.value, p5.value, p6.value) : this.returnValue));
  }
}
