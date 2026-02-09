// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_SetPlayerParam
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Set Player Param")]
[Name("Set Player Param", 0)]
[Category("Game Actions")]
public class Flow_SetPlayerParam : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<string> in_param_name = this.AddValueInput<string>("Param name");
    ValueInput<float> in_value = this.AddValueInput<float>("Value");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      MainGame.me.player.SetParam(in_param_name.value, in_value.value);
      flow_out.Call(f);
    }));
  }
}
