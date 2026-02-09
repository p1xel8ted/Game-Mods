// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.StartEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("On Start", 9)]
[Category("Events/Graph")]
[Description("Called only once and the first time the Graph is enabled.\nThis is called immediate.")]
public class StartEvent : EventNode
{
  public FlowOutput start;
  public bool called;

  public override void OnGraphStarted()
  {
    if (this.called)
      return;
    this.called = true;
    this.start.Call(new Flow());
  }

  public override void RegisterPorts() => this.start = this.AddFlowOutput("Once");
}
