// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_FreeAllIdlePoints
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Free All Idle Points", 0)]
[Category("Game Actions")]
[Description("Free All Idle Points")]
public class Flow_FreeAllIdlePoints : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<GDPoint.IdlePointPrefix> in_idle_points_prefix = this.AddValueInput<GDPoint.IdlePointPrefix>("prefix");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if (in_idle_points_prefix.value == GDPoint.IdlePointPrefix.None)
      {
        Debug.LogError((object) ("Wrong IdlePointPrefix: " + in_idle_points_prefix.value.ToString()));
        flow_out.Call(f);
      }
      else
      {
        string idlePrefix = GDPoint.GetIdlePrefix(in_idle_points_prefix.value);
        WorldGameObject objectByCustomTag = WorldMap.GetWorldGameObjectByCustomTag(idlePrefix + "stock");
        if ((Object) objectByCustomTag == (Object) null)
        {
          Debug.LogError((object) $"Not found stock WGO with custom tag \"{idlePrefix}stock\"!");
          flow_out.Call(f);
        }
        else
        {
          int paramInt = objectByCustomTag.GetParamInt("max_idle_points");
          if (paramInt < 1)
          {
            Debug.LogError((object) $"Max idle points with prefix \"{idlePrefix}\", stored in [{objectByCustomTag.name}::{objectByCustomTag.obj_id}] is wrong: {paramInt.ToString()}");
            flow_out.Call(f);
          }
          else
          {
            for (int index = 1; index <= paramInt; ++index)
            {
              if (objectByCustomTag.data.GetParamInt(idlePrefix + index.ToString()) == 1)
              {
                objectByCustomTag.data.SetParam(idlePrefix + index.ToString(), 0.0f);
                Debug.Log((object) $"#ipm# [{objectByCustomTag.obj_id}]: free idle point {{{idlePrefix}{index.ToString()}}}");
              }
            }
            flow_out.Call(f);
          }
        }
      }
    }));
  }
}
