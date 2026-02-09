// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_StartCampLive
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DLCRefugees;
using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Start Camp Live", 0)]
[Category("Game Actions")]
public class Flow_StartCampLive : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<bool> start_or_stop = this.AddValueInput<bool>("state");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if (start_or_stop.value)
        RefugeesCampEngine.instance.StartCampLive();
      else
        RefugeesCampEngine.instance.StopCampLive();
      flow_out.Call(f);
    }));
  }

  public override string name
  {
    get
    {
      return this.GetInputValuePort<bool>("state").value ? "<color=#FFFF50>Start Camp Live</color>" : "<color=#30FF30>Stop Camp Live</color>";
    }
    set => base.name = value;
  }
}
