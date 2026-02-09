// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_CameraCinematicFly
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Camera Cinematic Fly", 0)]
[Category("Game Actions")]
[Description("Duration 0 -- default")]
public class Flow_CameraCinematicFly : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<GameObject> par_wgo = this.AddValueInput<GameObject>("Target GO");
    ValueInput<float> par_duration = this.AddValueInput<float>("Duration");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    FlowOutput flow_finished = this.AddFlowOutput("On Finished");
    float set_duration = 0.7f;
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if (!Mathf.Approximately(par_duration.value, 0.0f))
        set_duration = par_duration.value;
      if (!par_wgo.HasValue<GameObject>())
      {
        Debug.Log((object) "Call CameraFlyBack");
        CameraTools.CameraFlyBack((GJCommons.VoidDelegate) (() => flow_finished.Call(f)));
      }
      else
      {
        Debug.Log((object) $"Call CameraCinematicFly \"{par_wgo.value.name}\"");
        CameraTools.CameraCinematicFly(par_wgo.value.transform, (GJCommons.VoidDelegate) (() => flow_finished.Call(f)), set_duration);
      }
      flow_out.Call(f);
    }));
  }

  public override string name
  {
    get
    {
      return !this.GetInputValuePort<GameObject>("Target GO").isConnected ? "Camera Cinematic Fly (back)" : base.name;
    }
    set => base.name = value;
  }
}
