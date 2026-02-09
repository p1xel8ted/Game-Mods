// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_CharWait
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Char Wait", 0)]
public class Flow_CharWait : MyFlowNode
{
  public override void RegisterPorts()
  {
    FlowOutput flow_out = this.AddFlowOutput("Out");
    FlowOutput on_started_waiting = this.AddFlowOutput("On Started Waiting");
    FlowOutput on_ended_waiting = this.AddFlowOutput("On Ended Waiting");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      GUIElements.me.waiting_gui.Open((GJCommons.VoidDelegate) (() => on_started_waiting.Call(f)), (GJCommons.VoidDelegate) (() => on_ended_waiting.Call(f)));
      flow_out.Call(f);
    }));
  }
}
