// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Legacy.ReflectedActionNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;
using System.Reflection;

#nullable disable
namespace FlowCanvas.Nodes.Legacy;

public sealed class ReflectedActionNode : ReflectedMethodNode
{
  public ReflectedMethodNode.ActionCall call;

  public void Call() => this.call();

  public override void RegisterPorts(
    FlowNode node,
    MethodInfo method,
    ReflectedMethodRegistrationOptions options)
  {
    this.call = method.RTCreateDelegate<ReflectedMethodNode.ActionCall>((object) null);
    FlowOutput o = node.AddFlowOutput(" ");
    node.AddFlowInput(" ", (FlowHandler) (f =>
    {
      this.Call();
      o.Call(f);
    }));
  }
}
