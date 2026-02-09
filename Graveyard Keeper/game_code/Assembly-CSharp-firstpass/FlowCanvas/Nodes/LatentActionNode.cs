// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.LatentActionNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections;
using System.Runtime.CompilerServices;

#nullable disable
namespace FlowCanvas.Nodes;

public abstract class LatentActionNode : LatentActionNodeBase
{
  public abstract IEnumerator Invoke();

  public sealed override void OnRegisterDerivedPorts(FlowNode node)
  {
    node.AddFlowInput("In", (FlowHandler) (f => this.Begin(this.Invoke(), f)));
  }

  [CompilerGenerated]
  public void \u003COnRegisterDerivedPorts\u003Eb__1_0(Flow f) => this.Begin(this.Invoke(), f);
}
