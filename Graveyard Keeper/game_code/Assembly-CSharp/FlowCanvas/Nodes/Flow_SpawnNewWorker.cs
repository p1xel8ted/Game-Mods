// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_SpawnNewWorker
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Spawn New Worker", 0)]
public class Flow_SpawnNewWorker : MyFlowNode
{
  public Item out_item;
  public WorldGameObject out_wgo;

  public override void RegisterPorts()
  {
    ValueInput<string> in_worker_id = this.AddValueInput<string>("worker_id");
    ValueInput<GDPoint> in_spawn_point = this.AddValueInput<GDPoint>("spawn_point");
    ValueInput<Item> in_base_body = this.AddValueInput<Item>("base_body");
    this.AddValueOutput<WorldGameObject>("worker_wgo", (ValueHandler<WorldGameObject>) (() => this.out_wgo));
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if (string.IsNullOrEmpty(in_worker_id.value))
      {
        Debug.LogError((object) "Flow_SpawnNewWorker error: worker_id is null!");
      }
      else
      {
        WorkerDefinition data = GameBalance.me.GetData<WorkerDefinition>(in_worker_id.value);
        if (data == null)
          Debug.LogError((object) "Flow_SpawnNewWorker error: worker_definition is null!");
        else if ((Object) in_spawn_point.value == (Object) null)
        {
          Debug.LogError((object) "Flow_SpawnNewWorker error: spawn_point is null!");
        }
        else
        {
          Item body = in_base_body.value;
          if (body == null || body.IsEmpty())
          {
            body = MainGame.me.save.GenerateBody(1, 3);
            Debug.LogError((object) "While spawning new zombie worker created new body: base_body was null or empty!");
          }
          if (body == null || body.IsEmpty())
            Debug.LogError((object) "Flow_SpawnNewWorker error: base_body is null or empty!");
          else if (body.definition.type != ItemDefinition.ItemType.Body)
          {
            Debug.LogError((object) "Flow_SpawnNewWorker error: base_body is NOT a body!");
          }
          else
          {
            WorldGameObject zombie_wgo = WorldMap.SpawnWGO(MainGame.me.world_root, data.worker_wgo, new Vector3?(in_spawn_point.value.pos));
            MainGame.me.save.workers.CreateNewWorker(zombie_wgo, in_worker_id.value, body);
            this.out_wgo = zombie_wgo;
            flow_out.Call(f);
          }
        }
      }
    }));
  }
}
