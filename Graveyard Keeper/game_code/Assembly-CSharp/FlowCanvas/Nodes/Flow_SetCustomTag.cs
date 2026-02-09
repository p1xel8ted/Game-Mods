// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_SetCustomTag
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[ParadoxNotion.Design.Icon("Cube", false, "")]
[Description("If WGO is null, then self")]
[Category("Game Actions")]
[Name("Set Custom Tag", 0)]
public class Flow_SetCustomTag : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> par_wgo = this.AddValueInput<WorldGameObject>("WGO");
    ValueInput<string> in_custom_tag = this.AddValueInput<string>("custom_tag");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject worldGameObject = this.WGOParamOrSelf(par_wgo);
      if ((Object) worldGameObject == (Object) null)
      {
        Debug.LogError((object) "Flow_SetCustomTag error: WGO is null");
      }
      else
      {
        worldGameObject.custom_tag = in_custom_tag.value;
        flow_out.Call(f);
      }
    }));
  }
}
