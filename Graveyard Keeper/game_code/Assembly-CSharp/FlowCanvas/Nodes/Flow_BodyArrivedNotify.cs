// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_BodyArrivedNotify
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Body Arrived Notify", 0)]
[Category("Game Actions")]
public class Flow_BodyArrivedNotify : MyFlowNode
{
  public override void RegisterPorts()
  {
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      GUIElements.me.body_arrived_gui.Display();
      flow_out.Call(f);
    }));
  }
}
