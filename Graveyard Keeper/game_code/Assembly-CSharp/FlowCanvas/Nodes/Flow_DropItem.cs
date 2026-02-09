// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_DropItem
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Drop Item", 0)]
[Description("If WGO is null, then self")]
[Color("857fff")]
[ParadoxNotion.Design.Icon("ArrowDown", false, "")]
public class Flow_DropItem : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> par_wgo = this.AddValueInput<WorldGameObject>("WGO to drop");
    ValueInput<string> par_res = this.AddValueInput<string>("Res_id");
    ValueInput<Item> in_item = this.AddValueInput<Item>("Item");
    ValueInput<int> par_amount = this.AddValueInput<int>("Amount");
    ValueInput<Direction> par_direction = this.AddValueInput<Direction>("Direction");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    par_amount.serializedValue = (object) 1;
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject worldGameObject = this.WGOParamOrSelf(par_wgo);
      if ((Object) worldGameObject == (Object) null)
      {
        Debug.LogError((object) "WGO is null!");
      }
      else
      {
        Item obj;
        if (in_item.value == null || in_item.value.IsEmpty())
        {
          obj = new Item(par_res.value, par_amount.value);
        }
        else
        {
          obj = in_item.value;
          if (par_amount.value > 0)
            obj.value = par_amount.value;
        }
        Debug.Log((object) $"Drop item {obj.id}, n = {obj.value.ToString()}");
        worldGameObject.DropItem(obj, par_direction.value, force: par_direction.value == Direction.None ? 1f : 3f, check_walls: par_direction.value == Direction.None);
        flow_out.Call(f);
      }
    }));
  }
}
