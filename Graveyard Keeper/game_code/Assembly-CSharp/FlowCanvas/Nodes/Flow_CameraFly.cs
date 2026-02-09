// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_CameraFly
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Camera Fly", 0)]
[Category("Game Actions")]
public class Flow_CameraFly : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<GameObject> par_wgo = this.AddValueInput<GameObject>("Target GO");
    ValueInput<bool> par_move_cam_inst = this.AddValueInput<bool>("Move instantly?");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    FlowOutput flow_finished = this.AddFlowOutput("On Finished");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      float duration = par_move_cam_inst.value ? 0.0f : 0.7f;
      if (!par_wgo.HasValue<GameObject>())
      {
        Debug.Log((object) "Call CameraFlyBack");
        CameraTools.CameraFlyBack((GJCommons.VoidDelegate) (() => flow_finished.Call(f)), duration);
      }
      else
      {
        Debug.Log((object) $"Call CameraFlyTo \"{par_wgo.value.name}\"");
        CameraTools.CameraFlyTo(par_wgo.value.transform, (GJCommons.VoidDelegate) (() => flow_finished.Call(f)), duration);
      }
      flow_out.Call(f);
    }));
  }

  public override string name
  {
    get
    {
      return !this.GetInputValuePort<GameObject>("Target GO").isConnected ? "Camera Fly (back)" : base.name;
    }
    set => base.name = value;
  }
}
