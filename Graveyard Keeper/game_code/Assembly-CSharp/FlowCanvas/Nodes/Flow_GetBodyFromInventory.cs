// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_GetBodyFromInventory
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Get body from inventory", 0)]
[Category("Game Actions")]
[Description("True if wgo has item with this ID")]
public class Flow_GetBodyFromInventory : MyFlowNode
{
  public Item body_item;

  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> in_wgo = this.AddValueInput<WorldGameObject>("WGO");
    this.AddValueOutput<Item>("body_item", (ValueHandler<Item>) (() => this.body_item));
    FlowOutput flow_yes = this.AddFlowOutput("Yes");
    FlowOutput flow_no = this.AddFlowOutput("No");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject worldGameObject = this.WGOParamOrSelf(in_wgo);
      if ((Object) worldGameObject == (Object) null)
      {
        Debug.LogError((object) "WGO is null!");
        flow_no.Call(f);
      }
      else if (worldGameObject.data?.inventory == null)
      {
        Debug.LogError((object) "WGO inventory is null!");
        flow_no.Call(f);
      }
      else if (worldGameObject.data.inventory.Count == 0)
      {
        flow_no.Call(f);
      }
      else
      {
        foreach (Item obj in worldGameObject.data.inventory)
        {
          if (!obj.IsEmpty() && obj.definition.type == ItemDefinition.ItemType.Body)
          {
            this.body_item = obj;
            flow_yes.Call(f);
            return;
          }
        }
        flow_no.Call(f);
      }
    }));
  }
}
