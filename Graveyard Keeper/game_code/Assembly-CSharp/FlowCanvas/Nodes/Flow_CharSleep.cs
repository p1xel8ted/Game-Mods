// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_CharSleep
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("CharSleep", 0)]
[Category("Game Actions")]
public class Flow_CharSleep : MyFlowNode
{
  public override void RegisterPorts()
  {
    FlowOutput flow_out = this.AddFlowOutput("Out");
    FlowOutput on_doesnt_need_sleep = this.AddFlowOutput("On Doesnt Need Sleep");
    FlowOutput on_appeared = this.AddFlowOutput("On Appeared");
    FlowOutput flow_after_save = this.AddFlowOutput("After save");
    FlowOutput on_wake_up = this.AddFlowOutput("On Wake Up");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      GUIElements.me.sleep_gui.Open((GJCommons.VoidDelegate) (() => on_appeared.Call(f)), (GJCommons.VoidDelegate) (() => on_wake_up.Call(f)), (GJCommons.VoidDelegate) (() => on_doesnt_need_sleep.Call(f)), (GJCommons.VoidDelegate) (() => flow_after_save.Call(f)));
      flow_out.Call(f);
    }));
  }
}
