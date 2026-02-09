// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_CameraCinematics
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("If WGO is null, then self")]
[Category("Game Actions")]
[Name("Camera Cinematics", 0)]
public class Flow_CameraCinematics : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> par_wgo = this.AddValueInput<WorldGameObject>("WGO");
    ValueInput<float> par_duration = this.AddValueInput<float>("Hold duration");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    FlowOutput flow_finished = this.AddFlowOutput("On Finished");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      CameraTools.PlayCinematics(this.WGOParamOrSelf(par_wgo), hold_duration: par_duration.value, on_ended: (GJCommons.VoidDelegate) (() => flow_finished.Call(f)));
      flow_out.Call(f);
    }));
  }
}
