// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_SetWGOsListParam
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Set WGOs List Param")]
[Category("Game Actions")]
[Name("Set WGOs List Param", 0)]
public class Flow_SetWGOsListParam : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<List<WorldGameObject>> in_wgos = this.AddValueInput<List<WorldGameObject>>("WGOs List");
    ValueInput<string> in_param_name = this.AddValueInput<string>("Param name");
    ValueInput<float> in_value = this.AddValueInput<float>("Value");
    ValueInput<bool> in_need_redraw = this.AddValueInput<bool>("Need redraw");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      List<WorldGameObject> worldGameObjectList = in_wgos.value;
      if (worldGameObjectList == null)
      {
        Debug.LogError((object) "WGOs list is null!");
      }
      else
      {
        foreach (WorldGameObject worldGameObject in worldGameObjectList)
        {
          if ((Object) worldGameObject != (Object) null && !string.IsNullOrEmpty(in_param_name.value))
          {
            worldGameObject.SetParam(in_param_name.value, in_value.value);
            if (in_need_redraw.value)
              worldGameObject.Redraw();
          }
        }
        flow_out.Call(f);
      }
    }));
  }
}
