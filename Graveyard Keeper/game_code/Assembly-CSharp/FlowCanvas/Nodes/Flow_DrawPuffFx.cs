// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_DrawPuffFx
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Puff effect on WGO")]
[Category("Game Actions")]
[Name("Puff FX", 0)]
public class Flow_DrawPuffFx : MyFlowNode
{
  public override void RegisterPorts()
  {
    FlowOutput flow_out = this.AddFlowOutput("Out");
    ValueInput<WorldGameObject> par_wgo = this.AddValueInput<WorldGameObject>("WGO");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject worldGameObject = this.WGOParamOrSelf(par_wgo);
      if (!((Object) worldGameObject != (Object) null))
        return;
      worldGameObject.DrawPuffFX();
      flow_out.Call(f);
    }));
  }
}
