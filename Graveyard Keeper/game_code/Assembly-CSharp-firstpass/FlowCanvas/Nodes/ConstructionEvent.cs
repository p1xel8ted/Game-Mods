// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ConstructionEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Events/Graph")]
[Description("Called only once and the first time the Graph is enabled.\nUse this only for initialization of this graph.")]
[Name("On Awake", 10)]
public class ConstructionEvent : EventNode
{
  public FlowOutput awake;
  public bool called;

  public override void OnGraphStarted()
  {
    if (this.called)
      return;
    this.called = true;
    this.awake.Call(new Flow());
  }

  public override void RegisterPorts() => this.awake = this.AddFlowOutput("Once");
}
