// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.CallableActionNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

#nullable disable
namespace FlowCanvas.Nodes;

public abstract class CallableActionNode : CallableActionNodeBase
{
  public abstract void Invoke();

  public sealed override void OnRegisterPorts(FlowNode node)
  {
    FlowOutput o = node.AddFlowOutput(" ");
    node.AddFlowInput(" ", (FlowHandler) (f =>
    {
      this.Invoke();
      o.Call(f);
    }));
  }
}
