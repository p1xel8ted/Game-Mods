// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_RemoveItemFromItemsInventory
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Remove item from Item's inventory", 0)]
[Description("Remove item from item's inventory")]
[Category("Game Actions")]
public class Flow_RemoveItemFromItemsInventory : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<Item> in_item_where = this.AddValueInput<Item>("Item (where to remove)");
    ValueInput<Item> in_item = this.AddValueInput<Item>("Item");
    ValueInput<string> in_item_id = this.AddValueInput<string>("Item ID");
    ValueInput<int> in_item_count = this.AddValueInput<int>("Items Count");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if (in_item_where.value == null || in_item_where.value.IsEmpty())
        Debug.LogError((object) "Item (where to remove) is null!");
      else if (in_item.value == null && in_item_id.isDefaultValue)
        Debug.LogError((object) "Item is null!");
      else if (in_item.value != null && !in_item.value.IsEmpty())
      {
        in_item_where.value.RemoveItem(in_item.value);
        flow_out.Call(f);
      }
      else if (!string.IsNullOrEmpty(in_item_id.value) && in_item_count.value > 0)
      {
        in_item_where.value.RemoveItem(new Item(in_item_id.value, in_item_count.value));
        flow_out.Call(f);
      }
      else
        flow_out.Call(f);
    }));
  }
}
