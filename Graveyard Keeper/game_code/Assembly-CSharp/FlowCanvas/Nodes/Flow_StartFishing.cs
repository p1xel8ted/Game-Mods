// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_StartFishing
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Start Fishing", 0)]
[Category("Game Actions")]
public class Flow_StartFishing : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> in_fishing_spot_wgo = this.AddValueInput<WorldGameObject>("Fishing Spot WGO");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if ((Object) this.WGOParamOrSelf(in_fishing_spot_wgo) == (Object) null)
      {
        Debug.LogError((object) "Can not start fishing: Fishing spot is null!");
      }
      else
      {
        GUIElements.me.fishing.Open(in_fishing_spot_wgo.value);
        flow_out.Call(f);
      }
    }));
  }
}
