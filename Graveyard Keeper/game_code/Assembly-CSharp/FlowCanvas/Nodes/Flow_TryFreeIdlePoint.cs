// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_TryFreeIdlePoint
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Try Free Idle Point", 0)]
[Description("Try Free Idle Point")]
[Category("Game Actions")]
public class Flow_TryFreeIdlePoint : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> in_stock_wgo = this.AddValueInput<WorldGameObject>("Stock WGO");
    ValueInput<string> in_points_prefix = this.AddValueInput<string>("Points Prefix");
    ValueInput<WorldGameObject> in_npc_wgo = this.AddValueInput<WorldGameObject>("NPC");
    ValueInput<int> in_free_point_num = this.AddValueInput<int>("Free Point Num");
    FlowOutput flow_yes = this.AddFlowOutput("Done");
    FlowOutput flow_no = this.AddFlowOutput("Error");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if (!in_stock_wgo.HasValue<WorldGameObject>())
      {
        Debug.LogError((object) "Stock WGO not connected!");
        flow_no.Call(f);
      }
      WorldGameObject worldGameObject = in_stock_wgo.value;
      string param_name;
      if (in_npc_wgo.HasValue<WorldGameObject>())
      {
        if (in_npc_wgo.value.cur_gd_point.StartsWith(in_points_prefix.value))
        {
          param_name = in_npc_wgo.value.cur_gd_point;
        }
        else
        {
          if (in_npc_wgo.value.cur_gd_point.StartsWith("personal_"))
          {
            if (in_npc_wgo.value.data.GetParamInt(in_npc_wgo.value.cur_gd_point) != 1)
              return;
            in_npc_wgo.value.data.SetParam(in_npc_wgo.value.cur_gd_point, 0.0f);
            return;
          }
          Debug.LogError((object) "Can not free idle point: npc.gd_point is not idle point!");
          flow_no.Call(f);
          return;
        }
      }
      else if (in_free_point_num.value > 0)
      {
        param_name = in_points_prefix.value + in_free_point_num.value.ToString();
      }
      else
      {
        Debug.LogError((object) "Can not free idle point: point to free not set!");
        flow_no.Call(f);
        return;
      }
      if (worldGameObject.data.GetParamInt(param_name) == 1)
      {
        worldGameObject.data.SetParam(param_name, 0.0f);
        flow_yes.Call(f);
      }
      else
      {
        Debug.LogError((object) "Can not free idle point: point is already free or not set!");
        flow_no.Call(f);
      }
    }));
  }
}
