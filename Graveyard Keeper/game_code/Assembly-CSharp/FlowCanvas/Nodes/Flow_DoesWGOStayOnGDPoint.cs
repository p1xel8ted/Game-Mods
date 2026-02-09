// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_DoesWGOStayOnGDPoint
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Description("If WGO is null, then self")]
[Name("Does WGO Stay On GDPoint", 0)]
public class Flow_DoesWGOStayOnGDPoint : MyFlowNode
{
  public bool is_wgo_stay;

  public override void RegisterPorts()
  {
    FlowOutput out_flow = this.AddFlowOutput("Out");
    ValueInput<WorldGameObject> par_wgo = this.AddValueInput<WorldGameObject>("WGO");
    ValueInput<string> gd_tag = this.AddValueInput<string>("GD tag");
    this.AddValueOutput<bool>("stay?", (ValueHandler<bool>) (() => this.is_wgo_stay));
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject worldGameObject = this.WGOParamOrSelf(par_wgo);
      GDPoint gdPointByGdTag = WorldMap.GetGDPointByGDTag(gd_tag.value);
      if ((Object) worldGameObject != (Object) null && (Object) gdPointByGdTag != (Object) null)
        this.is_wgo_stay = (Vector2) worldGameObject.transform.position == (Vector2) gdPointByGdTag.transform.position;
      out_flow.Call(f);
    }));
  }
}
