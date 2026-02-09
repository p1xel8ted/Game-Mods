// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_TransformedGetWorker
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Get Transformed Worker", 0)]
[Category("Game Actions")]
public class Flow_TransformedGetWorker : MyFlowNode
{
  public Item out_item;
  public WorldGameObject out_wgo;

  public override void RegisterPorts()
  {
    ValueInput<Worker.WorkerTransformationType> in_transform_type = this.AddValueInput<Worker.WorkerTransformationType>("transform_type");
    ValueInput<WorldGameObject> in_zombie_wgo = this.AddValueInput<WorldGameObject>("worker_wgo");
    ValueInput<Item> in_overhead_worker_item = this.AddValueInput<Item>("overhead_item");
    ValueInput<Item> in_on_ground_worker_item = this.AddValueInput<Item>("on_ground_item");
    this.AddValueOutput<Item>("worker_item", (ValueHandler<Item>) (() => this.out_item));
    this.AddValueOutput<WorldGameObject>("worker_wgo", (ValueHandler<WorldGameObject>) (() => this.out_wgo));
    FlowOutput flow_success = this.AddFlowOutput("Success");
    FlowOutput flow_fail = this.AddFlowOutput("Fail");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      this.out_item = (Item) null;
      this.out_wgo = (WorldGameObject) null;
      Worker.WorkerState from_state;
      Worker.WorkerState to_state;
      Item in_item;
      switch (in_transform_type.value)
      {
        case Worker.WorkerTransformationType.FromOverheadToWGO:
          from_state = Worker.WorkerState.ItemOverhead;
          to_state = Worker.WorkerState.WGO;
          in_item = in_overhead_worker_item.value;
          break;
        case Worker.WorkerTransformationType.FromOverheadToOnGround:
          from_state = Worker.WorkerState.ItemOverhead;
          to_state = Worker.WorkerState.ItemOnGround;
          in_item = in_overhead_worker_item.value;
          break;
        case Worker.WorkerTransformationType.FromOnGroundToOverhead:
          from_state = Worker.WorkerState.ItemOnGround;
          to_state = Worker.WorkerState.ItemOverhead;
          in_item = in_on_ground_worker_item.value;
          break;
        case Worker.WorkerTransformationType.FromOnGroundToWGO:
          from_state = Worker.WorkerState.ItemOnGround;
          to_state = Worker.WorkerState.WGO;
          in_item = in_on_ground_worker_item.value;
          break;
        case Worker.WorkerTransformationType.FromWGOToOverhead:
          from_state = Worker.WorkerState.WGO;
          to_state = Worker.WorkerState.ItemOverhead;
          in_item = (Item) null;
          break;
        case Worker.WorkerTransformationType.FromWGOToOnGround:
          from_state = Worker.WorkerState.WGO;
          to_state = Worker.WorkerState.ItemOnGround;
          in_item = (Item) null;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      string message = Worker.TransformWorker(from_state, in_item, in_zombie_wgo.value, to_state, out this.out_item, out this.out_wgo);
      if (!string.IsNullOrEmpty(message))
      {
        Debug.LogError((object) message);
        flow_fail.Call(f);
      }
      else
        flow_success.Call(f);
    }));
  }
}
