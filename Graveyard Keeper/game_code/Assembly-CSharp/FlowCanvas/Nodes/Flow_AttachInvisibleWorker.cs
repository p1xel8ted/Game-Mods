// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_AttachInvisibleWorker
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Attach Invisible Worker", 0)]
public class Flow_AttachInvisibleWorker : MyFlowNode
{
  public WorldGameObject out_o;

  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> par_workbench_wgo = this.AddValueInput<WorldGameObject>("Workbench");
    this.AddValueOutput<WorldGameObject>("WGO", (ValueHandler<WorldGameObject>) (() => this.out_o));
    FlowOutput flow_success = this.AddFlowOutput("Succeed");
    FlowOutput flow_fail = this.AddFlowOutput("Failed");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if (WorldMap.AttachInvisibleWorker(this.WGOParamOrSelf(par_workbench_wgo), out this.out_o))
        flow_success.Call(f);
      else
        flow_fail.Call(f);
    }));
  }
}
