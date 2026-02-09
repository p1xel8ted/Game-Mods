// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_GetIdlePoint
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Get Idle Point", 0)]
[Category("Game Actions")]
[Description("Get Idle Point")]
public class Flow_GetIdlePoint : MyFlowNode
{
  public List<string> personal_points = new List<string>();
  public const bool LOG_STOCK_POINTS_MANAGEMENT = true;
  public string idle_point_name = string.Empty;

  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> in_stock_wgo = this.AddValueInput<WorldGameObject>("Stock WGO");
    ValueInput<string> in_points_prefix = this.AddValueInput<string>("Points Prefix");
    ValueInput<WorldGameObject> in_npc_wgo = this.AddValueInput<WorldGameObject>("NPC");
    ValueInput<int> in_free_point_name = this.AddValueInput<int>("Free Point Num");
    ValueInput<WorldGameObject> par_wgo = this.AddValueInput<WorldGameObject>("WGO");
    WorldGameObject _wgo = (WorldGameObject) null;
    this.AddValueOutput<string>("Point Name", (ValueHandler<string>) (() => this.idle_point_name));
    FlowOutput flow_yes = this.AddFlowOutput("Found");
    FlowOutput flow_no = this.AddFlowOutput("Not Found");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      _wgo = this.WGOParamOrSelf(par_wgo);
      if (!in_stock_wgo.HasValue<WorldGameObject>())
      {
        Debug.LogError((object) "Stock WGO not connected!");
        flow_no.Call(f);
      }
      else if (string.IsNullOrEmpty(in_points_prefix.value))
      {
        Debug.LogError((object) "Points prefix is null!");
        flow_no.Call(f);
      }
      else
      {
        WorldGameObject worldGameObject = in_stock_wgo.value;
        int paramInt = worldGameObject.GetParamInt("max_idle_points");
        if (paramInt < 1)
        {
          Debug.LogError((object) $"Max idle points with prefix \"{in_points_prefix.value}\", stored in [{worldGameObject.name}::{worldGameObject.obj_id}] is wrong: {paramInt.ToString()}");
        }
        else
        {
          string empty = string.Empty;
          if ((Object) in_npc_wgo.value != (Object) null && in_npc_wgo.value.cur_gd_point.StartsWith(in_points_prefix.value))
          {
            string curGdPoint = in_npc_wgo.value.cur_gd_point;
          }
          else if (in_free_point_name.value > 0)
          {
            string str = in_points_prefix.value + in_free_point_name.value.ToString();
          }
          List<int> intList = new List<int>();
          for (int index = 1; index <= paramInt; ++index)
          {
            if (worldGameObject.data.GetParamInt(in_points_prefix.value + index.ToString()) == 0)
              intList.Add(index);
          }
          bool flag = false;
          if (this.personal_points.Count > 0)
          {
            List<string> stringList = new List<string>();
            for (int index = 0; index < this.personal_points.Count; ++index)
            {
              if ((double) _wgo.data.GetParam(this.personal_points[index]) == 0.0)
                stringList.Add(this.personal_points[index]);
            }
            if (stringList.Count > 0)
            {
              int num = UnityEngine.Random.Range(1, stringList.Count / (intList.Count + stringList.Count));
              if (stringList.Count * 2 >= num)
              {
                flag = true;
                int index = UnityEngine.Random.Range(0, stringList.Count - 1);
                this.idle_point_name = stringList[index];
                _wgo.SetParam(this.idle_point_name, 1f);
                if ((Object) _wgo != (Object) null)
                {
                  if (_wgo.cur_gd_point.StartsWith(in_points_prefix.value))
                  {
                    if (worldGameObject.data.GetParamInt(_wgo.cur_gd_point) == 1)
                      worldGameObject.data.SetParam(_wgo.cur_gd_point, 0.0f);
                  }
                  else if (_wgo.cur_gd_point.StartsWith("personal_") && _wgo.data.GetParamInt(_wgo.cur_gd_point) == 1)
                    _wgo.data.SetParam(_wgo.cur_gd_point, 0.0f);
                }
                flow_yes.Call(f);
              }
            }
          }
          if (flag)
            return;
          switch (intList.Count)
          {
            case 0:
              flow_no.Call(f);
              break;
            case 1:
              this.idle_point_name = in_points_prefix.value + intList[0].ToString();
              worldGameObject.data.SetParam(this.idle_point_name, 1f);
              Debug.Log((object) $"#ipm# [{worldGameObject.obj_id}]: lock idle point {{{this.idle_point_name}}}");
              if ((Object) _wgo != (Object) null)
              {
                if (_wgo.cur_gd_point.StartsWith(in_points_prefix.value))
                {
                  if (worldGameObject.data.GetParamInt(_wgo.cur_gd_point) == 1)
                    worldGameObject.data.SetParam(_wgo.cur_gd_point, 0.0f);
                }
                else if (_wgo.cur_gd_point.StartsWith("personal_") && _wgo.data.GetParamInt(_wgo.cur_gd_point) == 1)
                  _wgo.data.SetParam(_wgo.cur_gd_point, 0.0f);
              }
              flow_yes.Call(f);
              break;
            default:
              int index1 = UnityEngine.Random.Range(0, intList.Count - 1);
              this.idle_point_name = in_points_prefix.value + intList[index1].ToString();
              worldGameObject.data.SetParam(this.idle_point_name, 1f);
              Debug.Log((object) $"#ipm# [{worldGameObject.obj_id}]: lock idle point {{{this.idle_point_name}}}");
              if ((Object) _wgo != (Object) null)
              {
                if (_wgo.cur_gd_point.StartsWith(in_points_prefix.value))
                {
                  if (worldGameObject.data.GetParamInt(_wgo.cur_gd_point) == 1)
                    worldGameObject.data.SetParam(_wgo.cur_gd_point, 0.0f);
                }
                else if (_wgo.cur_gd_point.StartsWith("personal_") && _wgo.data.GetParamInt(_wgo.cur_gd_point) == 1)
                  _wgo.data.SetParam(_wgo.cur_gd_point, 0.0f);
              }
              flow_yes.Call(f);
              break;
          }
        }
      }
    }));
  }
}
