// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Refugees.Flow_FeedRefugees
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DLCRefugees;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes.Refugees;

[Category("Game Actions/Refugees")]
[Name("Feed Refugee", 0)]
public class Flow_FeedRefugees : MyFlowNode
{
  public FlowInput @in;
  public FlowOutput @out;

  public override void RegisterPorts()
  {
    ValueInput<float> time_in_days = this.AddValueInput<float>("Time in days");
    this.@in = this.AddFlowInput("In", (FlowHandler) (flow =>
    {
      if ((double) time_in_days.value <= 0.0 || (double) time_in_days.value > 1.0)
        Debug.LogError((object) ("Wrong CampEngine refugees feeding time: " + time_in_days.value.ToString()));
      RefugeesCampEngine.instance.FeedRefugeeForCycle(time_in_days.value);
      this.@out.Call(flow);
    }));
    this.@out = this.AddFlowOutput("Out");
  }
}
