// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Dummy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Use for organization.")]
[Name("Identity", 100)]
public class Dummy : FlowControlNode
{
  public override string name => (string) null;

  public override void RegisterPorts()
  {
    FlowOutput fOut = this.AddFlowOutput(" ", "Out");
    this.AddFlowInput(" ", "In", (FlowHandler) (f => fOut.Call(f)));
  }
}
