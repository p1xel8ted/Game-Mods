// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_FollowWGO
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Follow WGO", 0)]
[Description("If WGO is null, then self")]
[ParadoxNotion.Design.Icon("CubeArrowStraight", false, "")]
[Category("Game Actions")]
public class Flow_FollowWGO : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> par_wgo = this.AddValueInput<WorldGameObject>("WGO");
    ValueInput<WorldGameObject> par_wgo2 = this.AddValueInput<WorldGameObject>("WGO to follow");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    FlowOutput flow_came = this.AddFlowOutput("Came to dest");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject worldGameObject = this.WGOParamOrSelf(par_wgo);
      if ((Object) worldGameObject == (Object) null)
        Debug.LogError((object) "FollowWGO error: WGO #1 is null");
      else if ((Object) par_wgo2.value == (Object) null)
      {
        Debug.LogError((object) "FollowWGO error: WGO #2 is null");
      }
      else
      {
        worldGameObject.components.character.FollowTarget(par_wgo2.value, 1.2f, (GJCommons.VoidDelegate) (() => flow_came.Call(f)));
        flow_out.Call(f);
      }
    }));
  }
}
