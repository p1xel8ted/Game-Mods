// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_ExhumeBodyFromWGO
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Description("Remove body from inventory and drop it")]
[Name("Exhume body from WGO", 0)]
public class Flow_ExhumeBodyFromWGO : MyFlowNode
{
  public override void RegisterPorts()
  {
    Item item = new Item();
    ValueInput<WorldGameObject> in_wgo = this.AddValueInput<WorldGameObject>("WGO");
    this.AddValueOutput<Item>("Item", (ValueHandler<Item>) (() => item));
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject context = this.WGOParamOrSelf(in_wgo);
      if ((Object) context == (Object) null)
      {
        Debug.LogError((object) "WGO is null!");
      }
      else
      {
        Item bodyFromInventory = context.GetBodyFromInventory();
        item = bodyFromInventory;
        if (bodyFromInventory == null)
        {
          Debug.LogError((object) "Body not found in WGO inventory", (Object) context);
        }
        else
        {
          context.DropItem(bodyFromInventory);
          context.data.RemoveItem(bodyFromInventory);
          flow_out.Call(f);
        }
      }
    }));
  }
}
