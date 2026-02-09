// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.LatentActionNode`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections;

#nullable disable
namespace FlowCanvas.Nodes;

public abstract class LatentActionNode<T1> : LatentActionNodeBase
{
  public abstract IEnumerator Invoke(T1 a);

  public sealed override void OnRegisterDerivedPorts(FlowNode node)
  {
    ValueInput<T1> p1 = node.AddValueInput<T1>(this.parameters[0].Name);
    node.AddFlowInput("In", (FlowHandler) (f => this.Begin(this.Invoke(p1.value), f)));
  }
}
