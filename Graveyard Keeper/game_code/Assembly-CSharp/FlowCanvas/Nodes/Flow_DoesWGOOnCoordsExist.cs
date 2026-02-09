// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_DoesWGOOnCoordsExist
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("If WGO not found, return false")]
[Category("Game Actions")]
[Name("Does WGO On Coords Exist", 0)]
public class Flow_DoesWGOOnCoordsExist : MyFlowNode
{
  public override void RegisterPorts()
  {
    bool is_found = false;
    this.AddValueOutput<bool>("Is Found", (ValueHandler<bool>) (() => is_found));
    ValueInput<string> par_obj_id = this.AddValueInput<string>("WGO obj_id");
    ValueInput<float> x_coord = this.AddValueInput<float>("'X' coordinate");
    ValueInput<float> y_coord = this.AddValueInput<float>("'Y' coordinate");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    FlowOutput flow_yes = this.AddFlowOutput("True");
    FlowOutput flow_no = this.AddFlowOutput("False");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if (par_obj_id == null)
        return;
      is_found = WorldMap.DoesWGOOnCoordsExist(par_obj_id.value, new Vector2(x_coord.value, y_coord.value));
      if (is_found)
        flow_yes.Call(f);
      else
        flow_no.Call(f);
      flow_out.Call(f);
    }));
  }
}
