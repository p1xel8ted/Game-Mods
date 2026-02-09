// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.CallableActionNode`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

#nullable disable
namespace FlowCanvas.Nodes;

public abstract class CallableActionNode<T1> : CallableActionNodeBase
{
  public abstract void Invoke(T1 a);

  public sealed override void OnRegisterPorts(FlowNode node)
  {
    FlowOutput o = node.AddFlowOutput(" ");
    ValueInput<T1> p1 = node.AddValueInput<T1>(this.parameters[0].Name);
    node.AddFlowInput(" ", (FlowHandler) (f =>
    {
      this.Invoke(p1.value);
      o.Call(f);
    }));
  }
}
