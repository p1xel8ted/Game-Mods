// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_AddItemToItemsInventory
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Add Item To Item's inventory", 0)]
public class Flow_AddItemToItemsInventory : MyFlowNode
{
  public bool item_was_added;

  public override void RegisterPorts()
  {
    ValueInput<Item> in_item_where = this.AddValueInput<Item>("Item (where to add)");
    ValueInput<Item> in_item = this.AddValueInput<Item>("Item");
    this.AddValueInput<bool>("One Hand Slot");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddValueOutput<bool>("Item was added?", (ValueHandler<bool>) (() => this.item_was_added));
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if (in_item_where.value == null || in_item_where.value.IsEmpty())
      {
        Debug.LogError((object) "Item (where to add) null or empty");
        flow_out.Call(f);
      }
      else
      {
        this.item_was_added = in_item_where.value.AddItem(in_item.value);
        flow_out.Call(f);
      }
    }));
  }
}
