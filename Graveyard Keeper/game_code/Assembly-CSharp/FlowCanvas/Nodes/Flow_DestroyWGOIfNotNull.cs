// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_DestroyWGOIfNotNull
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Destroy WGO if not null", 0)]
[Category("Game Actions")]
[ParadoxNotion.Design.Icon("Cross", false, "")]
[Description("If WGO is null, then do nothing")]
public class Flow_DestroyWGOIfNotNull : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> par_wgo = this.AddValueInput<WorldGameObject>("WGO");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if ((Object) par_wgo.value != (Object) null)
        par_wgo.value.DestroyMe();
      flow_out.Call(f);
    }));
  }
}
