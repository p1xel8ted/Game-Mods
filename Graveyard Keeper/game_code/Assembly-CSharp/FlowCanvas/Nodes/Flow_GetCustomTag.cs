// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_GetCustomTag
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Get Custom Tag", 0)]
[Category("Game Actions")]
[Description("If WGO is null, then self")]
[ParadoxNotion.Design.Icon("Cube", false, "")]
public class Flow_GetCustomTag : MyFlowNode
{
  public string custom_tag = "";

  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> par_wgo = this.AddValueInput<WorldGameObject>("WGO");
    this.AddValueOutput<string>("Custom tag", (ValueHandler<string>) (() => this.custom_tag));
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject worldGameObject = this.WGOParamOrSelf(par_wgo);
      if ((Object) worldGameObject == (Object) null)
      {
        Debug.LogError((object) "FollowWGO error: WGO is null");
      }
      else
      {
        this.custom_tag = worldGameObject.custom_tag;
        flow_out.Call(f);
      }
    }));
  }
}
