// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_IsWGOInRange
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Is WGO in Range", 0)]
[Description("If WGO is null, then self")]
[Category("Game Actions")]
public class Flow_IsWGOInRange : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> par_wgo1 = this.AddValueInput<WorldGameObject>("WGO #1");
    ValueInput<WorldGameObject> par_wgo2 = this.AddValueInput<WorldGameObject>("WGO #2");
    ValueInput<float> par_range = this.AddValueInput<float>("Range");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    FlowOutput flow_yes = this.AddFlowOutput("In range");
    FlowOutput flow_no = this.AddFlowOutput("Out of range");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject worldGameObject = this.WGOParamOrSelf(par_wgo1);
      if ((Object) worldGameObject == (Object) null)
        return;
      if ((Object) par_wgo2.value == (Object) null)
      {
        Debug.LogError((object) "Flow_IsWGOInRange: WGO #2 is not set");
      }
      else
      {
        if (worldGameObject.IsInRange(par_wgo2.value, par_range.value))
          flow_yes.Call(f);
        else
          flow_no.Call(f);
        flow_out.Call(f);
      }
    }));
  }
}
