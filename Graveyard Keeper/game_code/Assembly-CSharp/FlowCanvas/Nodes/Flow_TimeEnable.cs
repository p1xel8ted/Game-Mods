// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_TimeEnable
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Time Enable", 0)]
[Icon("Clock", false, "")]
public class Flow_TimeEnable : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<bool> time_enable = this.AddValueInput<bool>("Time Enable");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      EnvironmentEngine.me.EnableTime(time_enable.value);
      flow_out.Call(f);
    }));
  }

  public override string name
  {
    get => !this.GetInputValuePort<bool>("Time Enable").value ? "Time Disable" : base.name;
    set => base.name = value;
  }
}
