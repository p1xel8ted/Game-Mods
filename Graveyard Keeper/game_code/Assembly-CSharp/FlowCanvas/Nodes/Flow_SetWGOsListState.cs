// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_SetWGOsListState
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[ParadoxNotion.Design.Icon("CubeArrowStraight", false, "")]
[Description("Set WGOs List State")]
[Category("Game Actions")]
[Name("Set WGOs List State", 0)]
public class Flow_SetWGOsListState : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<List<WorldGameObject>> par_wgo = this.AddValueInput<List<WorldGameObject>>("WGOs List");
    ValueInput<bool> in_state = this.AddValueInput<bool>("State");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      List<WorldGameObject> worldGameObjectList = par_wgo.value;
      if (worldGameObjectList == null)
      {
        Debug.LogError((object) "FollowWGO error: WGOs List is null");
      }
      else
      {
        foreach (WorldGameObject worldGameObject in worldGameObjectList)
        {
          if (!((Object) worldGameObject == (Object) null))
          {
            worldGameObject.gameObject.SetActive(in_state.value);
            worldGameObject.Redraw();
          }
        }
        flow_out.Call(f);
      }
    }));
  }
}
