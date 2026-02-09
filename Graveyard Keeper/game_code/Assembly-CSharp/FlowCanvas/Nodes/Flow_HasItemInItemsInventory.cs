// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_HasItemInItemsInventory
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("True if item has another item with this ID")]
[Name("Has item in Item's inventory", 0)]
[Category("Game Actions")]
public class Flow_HasItemInItemsInventory : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<Item> in_item_where = this.AddValueInput<Item>("Item (where to check?)");
    ValueInput<Item> in_item = this.AddValueInput<Item>("Item");
    ValueInput<string> in_item_id = this.AddValueInput<string>("Item ID");
    FlowOutput flow_yes = this.AddFlowOutput("Yes");
    FlowOutput flow_no = this.AddFlowOutput("No");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      string item_id = (string) null;
      if (in_item_where.value == null || in_item_where.value.IsEmpty())
      {
        Debug.LogError((object) "Item (where to check) is null");
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
      else if (in_item_where.value.HasItemInInventory(item_id))
        flow_yes.Call(f);
      else
        flow_no.Call(f);
    }));
  }
}
