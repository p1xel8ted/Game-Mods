// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_RemoveItemFromInventory
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Remove item from inventory", 0)]
[Description("Remove item from inventory")]
public class Flow_RemoveItemFromInventory : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> in_wgo = this.AddValueInput<WorldGameObject>("WGO");
    ValueInput<Item> in_item = this.AddValueInput<Item>("Item");
    ValueInput<string> in_item_id = this.AddValueInput<string>("Item ID");
    ValueInput<int> in_item_count = this.AddValueInput<int>("Items Count");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject worldGameObject = this.WGOParamOrSelf(in_wgo);
      if ((Object) worldGameObject == (Object) null)
        Debug.LogError((object) "WGO is null!");
      else if (in_item.value == null && in_item_id.isDefaultValue)
        Debug.LogError((object) "Item is null!");
      else if (in_item.value != null && !in_item.value.IsEmpty())
      {
        worldGameObject.data.RemoveItem(in_item.value);
        flow_out.Call(f);
      }
      else if (!string.IsNullOrEmpty(in_item_id.value) && in_item_count.value > 0)
      {
        worldGameObject.data.RemoveItem(new Item(in_item_id.value, in_item_count.value));
        flow_out.Call(f);
      }
      else
        flow_out.Call(f);
    }));
  }
}
