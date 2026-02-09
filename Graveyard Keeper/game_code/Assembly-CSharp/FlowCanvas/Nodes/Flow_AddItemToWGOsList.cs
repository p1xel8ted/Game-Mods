// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_AddItemToWGOsList
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Add Item To WGOs List", 0)]
[Category("Game Actions")]
[Description("Add Item To WGO")]
public class Flow_AddItemToWGOsList : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<List<WorldGameObject>> in_wgo = this.AddValueInput<List<WorldGameObject>>("WGOs List");
    ValueInput<Item> in_item = this.AddValueInput<Item>("Item");
    this.AddValueInput<bool>("One Hand Slot");
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
            worldGameObject.AddToInventory(in_item.value);
            worldGameObject.Redraw();
          }
        }
        flow_out.Call(f);
      }
    }));
  }
}
