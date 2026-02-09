// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_IsDLCAvailable
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Color("20c1ae")]
[Description("Returns bool")]
[Category("Game Actions")]
[Name("Is DLC Available", 0)]
public class Flow_IsDLCAvailable : MyFlowNode
{
  public override void RegisterPorts()
  {
    bool is_available = false;
    this.AddValueOutput<bool>("Is Available", (ValueHandler<bool>) (() => is_available));
    ValueInput<DLCEngine.DLCVersion> dlc_version = this.AddValueInput<DLCEngine.DLCVersion>("DLC Version");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    FlowOutput flow_yes = this.AddFlowOutput("True");
    FlowOutput flow_no = this.AddFlowOutput("False");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      is_available = DLCEngine.IsDLCAvailable(dlc_version.value);
      if (is_available)
        flow_yes.Call(f);
      else
        flow_no.Call(f);
      flow_out.Call(f);
    }));
  }
}
