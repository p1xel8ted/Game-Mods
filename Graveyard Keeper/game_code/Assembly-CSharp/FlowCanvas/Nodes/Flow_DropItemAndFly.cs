// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_DropItemAndFly
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Drop Item and Fly", 0)]
[Category("Game Actions")]
[Description("If WGO is null, then self")]
[ParadoxNotion.Design.Icon("ArrowDown", false, "")]
[Color("857fff")]
public class Flow_DropItemAndFly : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> par_wgo = this.AddValueInput<WorldGameObject>("WGO to drop");
    ValueInput<string> par_res = this.AddValueInput<string>("Res_id");
    ValueInput<int> par_amount = this.AddValueInput<int>("Amount");
    ValueInput<string> par_gdp = this.AddValueInput<string>("GDP tag");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      GDPoint gdPointByName = WorldMap.GetGDPointByName(par_gdp.value);
      WorldGameObject worldGameObject = this.WGOParamOrSelf(par_wgo);
      if ((Object) worldGameObject == (Object) null)
      {
        Debug.LogError((object) "WGO is null!");
      }
      else
      {
        if ((Object) gdPointByName != (Object) null)
        {
          worldGameObject.DropItemAndFly(new Item(par_res.value, par_amount.value), (Vector2) gdPointByName.pos);
        }
        else
        {
          Debug.LogError((object) $"Trying to drop {par_res.value} to the null GD point");
          worldGameObject.DropItem(new Item(par_res.value, par_amount.value));
        }
        flow_out.Call(f);
      }
    }));
  }
}
