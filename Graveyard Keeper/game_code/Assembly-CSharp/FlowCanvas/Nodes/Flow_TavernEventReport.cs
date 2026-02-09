// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_TavernEventReport
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[ParadoxNotion.Design.Icon("CubePlus", false, "")]
[Category("Game Actions")]
[Name("Tavern event report", 0)]
public class Flow_TavernEventReport : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> in_barmen = this.AddValueInput<WorldGameObject>("barmen");
    ValueInput<string> in_event = this.AddValueInput<string>("event ID");
    FlowOutput flow_out = this.AddFlowOutput("out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if ((Object) in_barmen.value == (Object) null)
      {
        Debug.LogError((object) "Flow_TavernEventReport error: barmen WGO is null!");
        flow_out.Call(f);
      }
      else
      {
        TavernEventDefinition data = GameBalance.me.GetData<TavernEventDefinition>(in_event.value);
        if (data == null)
        {
          Debug.LogError((object) "Flow_TavernEventReport error: event_definition is null!");
          flow_out.Call(f);
        }
        else
        {
          GUIElements.me.tavern_event_report.OpenPlayersTavernEventResult(in_barmen.value, data);
          flow_out.Call(f);
        }
      }
    }));
  }
}
