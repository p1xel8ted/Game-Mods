// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_StopFollowWGO
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("If WGO is null, then self")]
[Category("Game Actions")]
[Name("Stop Following WGO", 0)]
[ParadoxNotion.Design.Icon("CubeArrowStraight", false, "")]
public class Flow_StopFollowWGO : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> par_wgo = this.AddValueInput<WorldGameObject>("WGO");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject worldGameObject = this.WGOParamOrSelf(par_wgo);
      if ((Object) worldGameObject == (Object) null)
      {
        Debug.LogError((object) "FollowWGO error: WGO #1 is null");
      }
      else
      {
        worldGameObject.components.character.StopTargetFollowing();
        flow_out.Call(f);
      }
    }));
  }
}
