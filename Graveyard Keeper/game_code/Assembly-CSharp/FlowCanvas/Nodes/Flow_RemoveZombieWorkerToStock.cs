// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_RemoveZombieWorkerToStock
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Remove Zombie Worker To Stock", 0)]
[Category("Game Actions")]
public class Flow_RemoveZombieWorkerToStock : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> in_wgo = this.AddValueInput<WorldGameObject>("WGO");
    ValueInput<string> in_gd_point_tag = this.AddValueInput<string>("GD Point Tag");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject worker = this.WGOParamOrSelf(in_wgo);
      if ((Object) worker == (Object) null)
      {
        Debug.LogError((object) "WGO is null!");
      }
      else
      {
        string stock = WorldMap.RemoveZombieWorkerToStock(worker, in_gd_point_tag.value);
        if (!string.IsNullOrEmpty(stock))
          Debug.LogError((object) stock);
        flow_out.Call(f);
      }
    }));
  }
}
