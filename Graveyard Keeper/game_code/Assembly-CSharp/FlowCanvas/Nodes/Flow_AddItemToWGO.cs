// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_AddItemToWGO
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Add Item To WGO")]
[Category("Game Actions")]
[Name("Add Item To WGO", 0)]
public class Flow_AddItemToWGO : MyFlowNode
{
  public bool item_was_added;

  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> in_wgo = this.AddValueInput<WorldGameObject>("WGO");
    ValueInput<Item> in_item = this.AddValueInput<Item>("Item");
    this.AddValueInput<bool>("One Hand Slot");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddValueOutput<bool>("Item was added?", (ValueHandler<bool>) (() => this.item_was_added));
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if ((Object) in_wgo.value == (Object) null)
      {
        Debug.LogError((object) "WGO is null");
      }
      else
      {
        this.item_was_added = in_wgo.value.AddToInventory(in_item.value);
        in_wgo.value.Redraw();
        flow_out.Call(f);
      }
    }));
  }
}
