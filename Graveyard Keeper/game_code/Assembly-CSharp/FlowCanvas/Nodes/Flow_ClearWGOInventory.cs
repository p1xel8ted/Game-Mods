// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_ClearWGOInventory
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Clear WGO Inventory", 0)]
[Category("Game Actions")]
[Description("Clear WGO Inventory")]
public class Flow_ClearWGOInventory : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> in_wgo = this.AddValueInput<WorldGameObject>("WGO");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject worldGameObject = this.WGOParamOrSelf(in_wgo);
      if ((Object) worldGameObject == (Object) null)
      {
        Debug.LogError((object) "WGO is null");
      }
      else
      {
        worldGameObject.data.inventory.Clear();
        worldGameObject.Redraw();
        flow_out.Call(f);
      }
    }));
  }
}
