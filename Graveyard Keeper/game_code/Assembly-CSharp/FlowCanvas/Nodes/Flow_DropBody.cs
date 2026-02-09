// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_DropBody
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Color("857fff")]
[ParadoxNotion.Design.Icon("ArrowDown", false, "")]
[Description("If WGO is null, then self")]
[Category("Game Actions")]
[Name("Drop Body", 0)]
public class Flow_DropBody : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> par_wgo = this.AddValueInput<WorldGameObject>("WGO to drop");
    ValueInput<Direction> par_direction = this.AddValueInput<Direction>("Direction");
    ValueInput<int> par_tier_min = this.AddValueInput<int>("Tier min");
    ValueInput<int> par_tier_max = this.AddValueInput<int>("Tier max");
    ValueInput<int> par_tier_soul_min = this.AddValueInput<int>("Tier min(soul)");
    ValueInput<int> par_tier_soul_max = this.AddValueInput<int>("Tier max(soul)");
    ValueInput<float> par_dec_durability = this.AddValueInput<float>("dec durability [0..100]");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject worldGameObject = this.WGOParamOrSelf(par_wgo);
      if ((Object) worldGameObject == (Object) null)
      {
        Debug.LogError((object) "WGO is null!");
      }
      else
      {
        Item body = MainGame.me.save.GenerateBody(par_tier_min.value, par_tier_max.value, par_tier_soul_min.value, par_tier_soul_max.value);
        if ((double) par_dec_durability.value > 0.1)
          body.durability = (float) (1.0 - (double) par_dec_durability.value / 100.0);
        worldGameObject.DropItem(body, par_direction.value, force: par_direction.value == Direction.None ? 1f : 3f, check_walls: par_direction.value == Direction.None);
        flow_out.Call(f);
      }
    }));
  }
}
