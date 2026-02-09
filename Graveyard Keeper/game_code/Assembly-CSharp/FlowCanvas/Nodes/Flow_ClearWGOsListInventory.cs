// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_ClearWGOsListInventory
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Clear WGOs List Inventory", 0)]
[Description("Clear WGOs List Inventory")]
[Category("Game Actions")]
public class Flow_ClearWGOsListInventory : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<List<WorldGameObject>> in_wgo = this.AddValueInput<List<WorldGameObject>>("WGOs List");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if (in_wgo.value == null)
      {
        Debug.LogError((object) "WGO is null");
      }
      else
      {
        foreach (WorldGameObject worldGameObject in in_wgo.value)
        {
          if (!((Object) worldGameObject == (Object) null))
          {
            worldGameObject.data.inventory.Clear();
            worldGameObject.Redraw();
          }
        }
        flow_out.Call(f);
      }
    }));
  }
}
