// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_SpawnZombieWorkerFromStock
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Spawn Zombie Worker From Stock", 0)]
[Category("Game Actions")]
public class Flow_SpawnZombieWorkerFromStock : MyFlowNode
{
  public WorldGameObject out_o;

  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> par_workbench_wgo = this.AddValueInput<WorldGameObject>("Workbench");
    ValueInput<Item> par_worker_item = this.AddValueInput<Item>("worker item");
    this.AddValueOutput<WorldGameObject>("WGO", (ValueHandler<WorldGameObject>) (() => this.out_o));
    FlowOutput flow_success = this.AddFlowOutput("Succeed");
    FlowOutput flow_fail = this.AddFlowOutput("Failed");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject o = (WorldGameObject) null;
      bool is_success;
      string message = WorldMap.SpawnZombieWorkerFromStock(par_workbench_wgo.value, par_worker_item.value, out o, out is_success);
      if (!string.IsNullOrEmpty(message))
        Debug.LogError((object) message);
      this.out_o = o;
      if (is_success)
        flow_success.Call(f);
      else
        flow_fail.Call(f);
    }));
  }
}
