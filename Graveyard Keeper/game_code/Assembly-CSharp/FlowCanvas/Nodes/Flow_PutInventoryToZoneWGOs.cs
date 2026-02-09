// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_PutInventoryToZoneWGOs
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Put Inventory To Zone WGOs", 0)]
[Category("Game Actions")]
[Description("Put Inventory To Zone WGOs")]
public class Flow_PutInventoryToZoneWGOs : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> in_wgo = this.AddValueInput<WorldGameObject>("WGO");
    ValueInput<bool> in_drop_tail = this.AddValueInput<bool>("drop tail");
    ValueInput<bool> in_clear_inventory = this.AddValueInput<bool>("clear inventory");
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
        List<Item> inventory = in_wgo.value?.data?.inventory;
        if (inventory == null)
        {
          Debug.LogError((object) "Flow_PutInventoryToZoneWGOs error: Items list is null!");
          flow_out.Call(f);
        }
        else if (inventory.Count == 0)
        {
          flow_out.Call(f);
        }
        else
        {
          WorldZone myWorldZone = in_wgo.value.GetMyWorldZone();
          if ((Object) myWorldZone != (Object) null)
          {
            List<Item> cant_insert;
            myWorldZone.PutToAllPossibleInventoriesSmart(inventory, out cant_insert);
            if (in_drop_tail.value && cant_insert != null && cant_insert.Count > 0)
              in_wgo.value.DropItems(cant_insert);
          }
          if (in_clear_inventory.value)
            in_wgo.value.data.inventory = new List<Item>();
          flow_out.Call(f);
        }
      }
    }));
  }
}
