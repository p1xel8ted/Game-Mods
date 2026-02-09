// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.DisableEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("On Disable", 7)]
[Description("Called when the Graph is Disabled")]
[Category("Events/Graph")]
public class DisableEvent : EventNode
{
  public FlowOutput disable;

  public override void OnGraphStoped() => this.disable.Call(new Flow());

  public override void RegisterPorts() => this.disable = this.AddFlowOutput("Out");
}
