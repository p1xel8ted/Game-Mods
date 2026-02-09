// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_RefugeeParams
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Set Refugee Params", 0)]
[Category("Game Actions")]
[Description("set all necessary refugee params")]
public class Flow_RefugeeParams : MyFlowNode
{
  public List<string> custom_param_list = new List<string>();

  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> in_wgo = this.AddValueInput<WorldGameObject>("Refugee WGO");
    WorldGameObject _wgo = (WorldGameObject) null;
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddValueOutput<WorldGameObject>("Refugee WGO", (ValueHandler<WorldGameObject>) (() => _wgo));
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject worldGameObject = this.WGOParamOrSelf(in_wgo);
      _wgo = worldGameObject;
      if ((Object) worldGameObject != (Object) null)
      {
        foreach (string customParam in this.custom_param_list)
          worldGameObject.SetParam(customParam, 2f);
        worldGameObject.SetParam("on_the_way_now", 1f);
        worldGameObject.SetParam("refugee_unrollable", 0.0f);
        worldGameObject.SetParam("first_roll", 0.0f);
        worldGameObject.SetParam("roll_percent", 0.7f);
      }
      flow_out.Call(f);
    }));
  }
}
