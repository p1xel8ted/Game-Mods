// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_HasItemInInventory
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Has item in inventory", 0)]
[Category("Game Actions")]
[Description("True if wgo has item with this ID")]
public class Flow_HasItemInInventory : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> in_wgo = this.AddValueInput<WorldGameObject>("WGO");
    ValueInput<Item> in_item = this.AddValueInput<Item>("Item");
    ValueInput<string> in_item_id = this.AddValueInput<string>("Item ID");
    FlowOutput flow_yes = this.AddFlowOutput("Yes");
    FlowOutput flow_no = this.AddFlowOutput("No");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject worldGameObject = this.WGOParamOrSelf(in_wgo);
      string item_id = (string) null;
      if ((Object) worldGameObject == (Object) null)
      {
        Debug.LogError((object) "WGO is null!");
        flow_no.Call(f);
      }
      else if ((in_item.value == null || in_item.value.IsEmpty()) && in_item_id.isDefaultValue)
      {
        Debug.LogError((object) "Item is null!");
        flow_no.Call(f);
      }
      else if (in_item.value != null && !in_item.value.IsEmpty())
        item_id = in_item.value.id;
      else if (!string.IsNullOrEmpty(in_item_id.value))
        item_id = in_item_id.value;
      if (string.IsNullOrEmpty(item_id))
      {
        Debug.LogError((object) "Item id not set!");
        flow_no.Call(f);
      }
      else if (worldGameObject.data.HasItemInInventory(item_id))
        flow_yes.Call(f);
      else
        flow_no.Call(f);
    }));
  }
}
