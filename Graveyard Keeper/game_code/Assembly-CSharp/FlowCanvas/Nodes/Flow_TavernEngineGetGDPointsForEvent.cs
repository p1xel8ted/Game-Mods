// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_TavernEngineGetGDPointsForEvent
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Tavern Engine Get GDPoints For Event", 0)]
[Category("Game Actions")]
public class Flow_TavernEngineGetGDPointsForEvent : MyFlowNode
{
  public List<GDPoint> _out_gd_points;

  public override void RegisterPorts()
  {
    ValueInput<string> in_event_id = this.AddValueInput<string>("event id");
    ValueInput<List<GDPoint>> in_gd_points = this.AddValueInput<List<GDPoint>>("available GDPoints");
    this.AddValueOutput<List<GDPoint>>("points for event", (ValueHandler<List<GDPoint>>) (() => this._out_gd_points));
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      this._out_gd_points = new List<GDPoint>();
      if (string.IsNullOrEmpty(in_event_id.value))
      {
        Debug.LogError((object) "Flow_TavernEngineGetGDPointsForEvent error: in_event_id is empty!");
      }
      else
      {
        TavernEventDefinition data = GameBalance.me.GetData<TavernEventDefinition>(in_event_id.value);
        if (data == null)
        {
          Debug.LogError((object) "Flow_TavernEngineGetGDPointsForEvent error: event_definition is NULL!");
        }
        else
        {
          int num = Mathf.FloorToInt(data.visitors_count.EvaluateFloat(character: MainGame.me.player));
          if (in_gd_points.value == null || in_gd_points.value.Count == 0)
          {
            Debug.LogError((object) "Flow_TavernEngineGetGDPointsForEvent error: input gd_points is null or empty!");
            flow_out.Call(f);
          }
          else if (num >= in_gd_points.value.Count)
          {
            this._out_gd_points = new List<GDPoint>();
            this._out_gd_points.AddRange((IEnumerable<GDPoint>) in_gd_points.value);
            flow_out.Call(f);
          }
          else
          {
            List<GDPoint> gdPointList = new List<GDPoint>();
            gdPointList.AddRange((IEnumerable<GDPoint>) in_gd_points.value);
            do
            {
              int index = 0;
              if (!data.idle_points_whitelist.Contains(gdPointList[0].gd_tag))
                index = UnityEngine.Random.Range(0, gdPointList.Count);
              this._out_gd_points.Add(gdPointList[index]);
              gdPointList.RemoveAt(index);
            }
            while (num-- > 0);
            flow_out.Call(f);
          }
        }
      }
    }));
  }
}
