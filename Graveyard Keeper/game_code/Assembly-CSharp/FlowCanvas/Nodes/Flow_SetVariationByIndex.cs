// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_SetVariationByIndex
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Set Variation by Index", 0)]
public class Flow_SetVariationByIndex : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> in_wgo = this.AddValueInput<WorldGameObject>("WGO");
    ValueInput<int> variation_index = this.AddValueInput<int>("Variation Index");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject worldGameObject = this.WGOParamOrSelf(in_wgo);
      if ((Object) worldGameObject == (Object) null)
      {
        Debug.LogError((object) "Flow_SetVariationByIndex error: WGO is null!");
        flow_out.Call(f);
      }
      else
      {
        Debug.Log((object) ("Set variation for WGO by index:" + variation_index.value.ToString()));
        worldGameObject.SetVariationByIndex(variation_index.value);
        flow_out.Call(f);
      }
    }));
  }
}
