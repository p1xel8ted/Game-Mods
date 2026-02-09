// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.EnableEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Called when the Graph is enabled")]
[Category("Events/Graph")]
[Name("On Enable", 8)]
public class EnableEvent : EventNode
{
  public FlowOutput enable;

  public override void OnGraphStarted() => this.enable.Call(new Flow());

  public override void RegisterPorts() => this.enable = this.AddFlowOutput("Out");
}
