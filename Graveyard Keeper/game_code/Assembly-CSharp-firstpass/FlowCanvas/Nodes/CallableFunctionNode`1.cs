// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.CallableFunctionNode`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

#nullable disable
namespace FlowCanvas.Nodes;

public abstract class CallableFunctionNode<TResult> : CallableFunctionNodeBase
{
  public TResult result;

  public abstract TResult Invoke();

  public sealed override void OnRegisterPorts(FlowNode node)
  {
    FlowOutput o = node.AddFlowOutput(" ");
    node.AddValueOutput<TResult>("Value", (ValueHandler<TResult>) (() => this.result));
    node.AddFlowInput(" ", (FlowHandler) (f =>
    {
      this.result = this.Invoke();
      o.Call(f);
    }));
  }
}
