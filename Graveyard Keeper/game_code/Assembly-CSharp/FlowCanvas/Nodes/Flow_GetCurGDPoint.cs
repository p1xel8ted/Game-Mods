// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_GetCurGDPoint
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Get Current GDPoint", 0)]
[Category("Game Actions")]
[Description("If WGO is null, then self")]
[ParadoxNotion.Design.Icon("Cube", false, "")]
public class Flow_GetCurGDPoint : MyFlowNode
{
  public string cur_gd_point = "";

  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> par_wgo = this.AddValueInput<WorldGameObject>("WGO");
    this.AddValueOutput<string>("GDPoint", (ValueHandler<string>) (() => this.cur_gd_point));
    FlowOutput flow_not_empty = this.AddFlowOutput("Not Empty");
    FlowOutput flow_empty = this.AddFlowOutput("Empty");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject worldGameObject = this.WGOParamOrSelf(par_wgo);
      if ((Object) worldGameObject == (Object) null)
      {
        Debug.LogError((object) "GetCurGDPoint error: WGO is null");
      }
      else
      {
        this.cur_gd_point = worldGameObject.cur_gd_point;
        if (string.IsNullOrEmpty(this.cur_gd_point))
          flow_empty.Call(f);
        else
          flow_not_empty.Call(f);
      }
    }));
  }
}
