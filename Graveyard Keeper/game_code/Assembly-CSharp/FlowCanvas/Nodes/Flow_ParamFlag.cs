// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_ParamFlag
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Param flag", 0)]
[Category("Game Actions")]
[ParadoxNotion.Design.Icon("Flag", false, "")]
[Description("If WGO is null, then self")]
public class Flow_ParamFlag : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> par_wgo = this.AddValueInput<WorldGameObject>("WGO");
    ValueInput<string> par_param = this.AddValueInput<string>("param");
    FlowOutput flow_eq0 = this.AddFlowOutput("<color=red>✘</color>", "== 0");
    FlowOutput flow_moreeq1 = this.AddFlowOutput("<color=green>✔</color>", ">= 1");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject worldGameObject = this.WGOParamOrSelf(par_wgo);
      if ((Object) worldGameObject == (Object) null)
        return;
      int paramInt = worldGameObject.GetParamInt(par_param.value);
      if (paramInt == 0)
        flow_eq0.Call(f);
      if (paramInt < 1)
        return;
      flow_moreeq1.Call(f);
    }));
  }
}
