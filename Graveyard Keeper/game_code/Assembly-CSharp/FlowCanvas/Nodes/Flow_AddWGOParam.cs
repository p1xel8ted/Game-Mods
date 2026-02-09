// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_AddWGOParam
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Add WGO Param", 0)]
[Category("Game Actions")]
[Description("Add WGO Param")]
public class Flow_AddWGOParam : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> in_wgo = this.AddValueInput<WorldGameObject>("WGO");
    ValueInput<string> in_param_name = this.AddValueInput<string>("Param name");
    ValueInput<float> in_value = this.AddValueInput<float>("Value");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject worldGameObject = this.WGOParamOrSelf(in_wgo);
      if ((Object) worldGameObject != (Object) null && !string.IsNullOrEmpty(in_param_name.value))
        worldGameObject.AddToParams(in_param_name.value, in_value.value);
      flow_out.Call(f);
    }));
  }
}
