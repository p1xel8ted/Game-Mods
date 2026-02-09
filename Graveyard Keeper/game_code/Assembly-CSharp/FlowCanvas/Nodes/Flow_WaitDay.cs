// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_WaitDay
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Wait Day", 0)]
public class Flow_WaitDay : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<float> par_speed = this.AddValueInput<float>("speed");
    FlowOutput flow_out = this.AddFlowOutput("Immediate");
    FlowOutput flow_done = this.AddFlowOutput("Finished");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      EnvironmentEngine.me.EnableTime(false);
      float start_time = TimeOfDay.me.time_of_day;
      bool looped = false;
      GJTimer.AddConditionalChecker((GJTimer.BoolDelegate) (() => looped && (double) TimeOfDay.me.time_of_day >= (double) start_time), (GJTimer.VoidDelegate) (() =>
      {
        TimeOfDay.me.time_of_day += Time.deltaTime * par_speed.value;
        if ((double) TimeOfDay.me.time_of_day <= 1.0)
          return;
        TimeOfDay.me.time_of_day -= 2f;
        looped = true;
      }), (GJTimer.VoidDelegate) (() => flow_done.Call(f)));
      flow_out.Call(f);
    }));
  }
}
