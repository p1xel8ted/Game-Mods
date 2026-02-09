// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_TutorialWindow
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Tutorial window", 0)]
public class Flow_TutorialWindow : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<string> in_id = this.AddValueInput<string>("id");
    FlowOutput flow_on_closed = this.AddFlowOutput("   On Closed", "On Closed");
    this.AddFlowInput("In", (FlowHandler) (f => GUIElements.me.tutorial.Open(in_id.value, (GJCommons.VoidDelegate) (() => flow_on_closed.Call(f)))));
  }
}
