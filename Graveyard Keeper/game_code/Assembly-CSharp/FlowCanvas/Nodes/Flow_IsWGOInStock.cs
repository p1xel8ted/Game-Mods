// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_IsWGOInStock
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Is WGO In Stock", 0)]
[Category("Game Actions")]
[Description("If WGO is null, then self")]
public class Flow_IsWGOInStock : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> par_wgo = this.AddValueInput<WorldGameObject>("WGO");
    ValueInput<string> in_gd_point_tag = this.AddValueInput<string>("GD Point Tag");
    FlowOutput flow_yes = this.AddFlowOutput("Yes");
    FlowOutput flow_no = this.AddFlowOutput("No");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject worldGameObject = this.WGOParamOrSelf(par_wgo);
      if ((Vector2) WorldMap.GetGDPointByGDTag(!string.IsNullOrEmpty(in_gd_point_tag.value) ? in_gd_point_tag.value : "default_destroy_point").transform.position == (Vector2) worldGameObject.transform.position)
        flow_yes.Call(f);
      else
        flow_no.Call(f);
    }));
  }
}
