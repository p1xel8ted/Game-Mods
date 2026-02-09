// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Finish
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Stops and cease execution of the FlowSript")]
public class Finish : FlowControlNode
{
  public override void RegisterPorts()
  {
    ValueInput<bool> c = this.AddValueInput<bool>("Success");
    this.AddFlowInput("In", (FlowHandler) (f => this.graph.Stop(c.value)));
  }
}
