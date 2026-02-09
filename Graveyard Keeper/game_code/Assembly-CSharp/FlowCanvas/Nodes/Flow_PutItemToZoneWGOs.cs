// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_PutItemToZoneWGOs
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Put Item To Zone WGOs")]
[Category("Game Actions")]
[Name("Put Item To Zone WGOs", 0)]
public class Flow_PutItemToZoneWGOs : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> in_wgo = this.AddValueInput<WorldGameObject>("WGO");
    ValueInput<Item> in_item = this.AddValueInput<Item>("item");
    ValueInput<bool> in_drop_tail = this.AddValueInput<bool>("drop tail");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if ((Object) in_wgo.value == (Object) null)
      {
        Debug.LogError((object) "WGO is null");
        flow_out.Call(f);
      }
      else
      {
        List<Item> drop_list = new List<Item>()
        {
          in_item.value
        };
        if (drop_list == null)
        {
          Debug.LogError((object) "Flow_PutInventoryToZoneWGOs error: Items list is null!");
          flow_out.Call(f);
        }
        else if (drop_list.Count == 0)
        {
          flow_out.Call(f);
        }
        else
        {
          WorldZone myWorldZone = in_wgo.value.GetMyWorldZone();
          if ((Object) myWorldZone != (Object) null)
          {
            List<Item> cant_insert;
            myWorldZone.PutToAllPossibleInventoriesSmart(drop_list, out cant_insert);
            if (in_drop_tail.value && cant_insert != null && cant_insert.Count > 0)
              in_wgo.value.DropItems(cant_insert);
          }
          flow_out.Call(f);
        }
      }
    }));
  }
}
