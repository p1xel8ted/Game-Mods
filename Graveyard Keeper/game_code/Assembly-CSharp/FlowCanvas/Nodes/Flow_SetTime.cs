// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_SetTime
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Set Time", 0)]
[Category("Game Actions")]
[Description("Set Time")]
public class Flow_SetTime : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<float> in_time = this.AddValueInput<float>("Time");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      TimeOfDay.me.SetTimeK(in_time.value);
      MainGame.me.gui_elements.hud.Update();
      TimeOfDay.me.Update();
      flow_out.Call(f);
    }));
  }
}
