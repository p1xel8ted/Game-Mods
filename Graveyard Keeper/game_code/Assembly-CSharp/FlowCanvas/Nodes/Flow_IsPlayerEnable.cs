// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_IsPlayerEnable
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Color("4155be")]
[Name("Is Player Enable", 0)]
[Category("Game Actions")]
public class Flow_IsPlayerEnable : MyFlowNode
{
  public bool control_out_value;

  public override void RegisterPorts()
  {
    FlowOutput flow_yes = this.AddFlowOutput("Yes");
    FlowOutput flow_no = this.AddFlowOutput("No");
    this.AddValueOutput<bool>("is enable?", (ValueHandler<bool>) (() => this.control_out_value));
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      this.control_out_value = GS.IsPlayerEnable();
      if (this.control_out_value)
        flow_yes.Call(f);
      else
        flow_no.Call(f);
    }));
  }
}
