// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Legacy.ReflectedActionNode`3
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;
using System.Reflection;

#nullable disable
namespace FlowCanvas.Nodes.Legacy;

public sealed class ReflectedActionNode<T1, T2, T3> : ReflectedMethodNode
{
  public ReflectedMethodNode.ActionCall<T1, T2, T3> call;
  public T1 instance;

  public void Call(T1 a, T2 b, T3 c)
  {
    this.instance = a;
    this.call(a, b, c);
  }

  public override void RegisterPorts(
    FlowNode node,
    MethodInfo method,
    ReflectedMethodRegistrationOptions options)
  {
    this.call = method.RTCreateDelegate<ReflectedMethodNode.ActionCall<T1, T2, T3>>((object) null);
    ValueInput<T1> p1 = node.AddValueInput<T1>(this.GetName(method, 0));
    ValueInput<T2> p2 = node.AddValueInput<T2>(this.GetName(method, 1));
    ValueInput<T3> p3 = node.AddValueInput<T3>(this.GetName(method, 2));
    FlowOutput o = node.AddFlowOutput(" ");
    node.AddFlowInput(" ", (FlowHandler) (f =>
    {
      this.Call(p1.value, p2.value, p3.value);
      o.Call(f);
    }));
    if (method.IsStatic)
      return;
    node.AddValueOutput<T1>(this.GetName(method, 0), (ValueHandler<T1>) (() => this.instance));
  }
}
