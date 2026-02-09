// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_StartCraftWGOsList
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Start Craft WGOs List", 0)]
[Category("Game Actions")]
[Description("Start Craft WGOs List")]
public class Flow_StartCraftWGOsList : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<List<WorldGameObject>> in_wgo = this.AddValueInput<List<WorldGameObject>>("WGOs List");
    ValueInput<Item> in_item = this.AddValueInput<Item>("Item");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      List<WorldGameObject> worldGameObjectList = in_wgo.value;
      if (worldGameObjectList == null)
      {
        Debug.LogError((object) "WGOs List is null!");
      }
      else
      {
        foreach (WorldGameObject worldGameObject in worldGameObjectList)
        {
          if (!((Object) worldGameObject == (Object) null))
            worldGameObject.components.craft.Craft(GameBalance.me.GetData<CraftDefinition>("set_" + in_item.value.id));
        }
        flow_out.Call(f);
      }
    }));
  }
}
