// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_GetObjectsInZone
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Get Objects In Zone", 0)]
[Category("Game Actions")]
public class Flow_GetObjectsInZone : MyFlowNode
{
  public override void RegisterPorts()
  {
    List<WorldGameObject> result = new List<WorldGameObject>();
    ValueInput<string> in_zone_id = this.AddValueInput<string>("zone_id");
    ValueInput<string> in_obj_id = this.AddValueInput<string>("obj_id");
    this.AddValueOutput<List<WorldGameObject>>("WGOs List", (ValueHandler<List<WorldGameObject>>) (() => result));
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      result = new List<WorldGameObject>();
      if (string.IsNullOrEmpty(in_zone_id.value))
      {
        Debug.LogError((object) "Zone ID can not be null!");
        flow_out.Call(f);
      }
      else
      {
        WorldZone zoneById = WorldZone.GetZoneByID(in_zone_id.value);
        if ((Object) zoneById == (Object) null)
        {
          Debug.LogError((object) $"Not found zone with ID \"{in_zone_id.value}\"");
          flow_out.Call(f);
        }
        else
        {
          if (string.IsNullOrEmpty(in_obj_id.value))
          {
            result = zoneById.GetZoneWGOs();
          }
          else
          {
            string str = in_obj_id.value;
            foreach (WorldGameObject zoneWgO in zoneById.GetZoneWGOs())
            {
              if (!((Object) zoneWgO == (Object) null) && zoneWgO.obj_id == str)
                result.Add(zoneWgO);
            }
          }
          Debug.Log((object) ("Total found WGOs: " + result.Count.ToString()));
          flow_out.Call(f);
        }
      }
    }));
  }
}
