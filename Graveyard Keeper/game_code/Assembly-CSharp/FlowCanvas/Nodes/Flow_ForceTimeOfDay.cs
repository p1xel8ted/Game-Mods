// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_ForceTimeOfDay
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Force Time Of Day", 0)]
public class Flow_ForceTimeOfDay : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<Flow_ForceTimeOfDay.TimeOfDayValue> in_time = this.AddValueInput<Flow_ForceTimeOfDay.TimeOfDayValue>("Time");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      switch (in_time.value)
      {
        case Flow_ForceTimeOfDay.TimeOfDayValue.Night:
          TimeOfDay.me.time_of_day = 0.0f;
          break;
        case Flow_ForceTimeOfDay.TimeOfDayValue.Day:
          TimeOfDay.me.time_of_day = 1f;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      MainGame.me.gui_elements.hud.Update();
      flow_out.Call(f);
    }));
  }

  public enum TimeOfDayValue
  {
    Night,
    Day,
  }
}
