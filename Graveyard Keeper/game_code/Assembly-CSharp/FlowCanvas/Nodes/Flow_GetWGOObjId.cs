// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_GetWGOObjId
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Get WGO's Obj ID", 0)]
[Category("Game Actions")]
public class Flow_GetWGOObjId : MyFlowNode
{
  public string _out_str = string.Empty;

  public override void RegisterPorts()
  {
    FlowOutput flow_out = this.AddFlowOutput("Out");
    ValueInput<WorldGameObject> in_wgo = this.AddValueInput<WorldGameObject>("WGO");
    this.AddValueOutput<string>("value", (ValueHandler<string>) (() => this._out_str));
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject worldGameObject = this.WGOParamOrSelf(in_wgo);
      if ((Object) worldGameObject == (Object) null)
      {
        Debug.LogError((object) "Flow_GetWGOObjId: WGO is null");
        flow_out.Call(f);
      }
      else
      {
        this._out_str = worldGameObject.obj_id;
        flow_out.Call(f);
      }
    }));
  }
}
