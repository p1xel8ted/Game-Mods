// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_CameraShake
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Com.LuisPedroFonseca.ProCamera2D;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Camera Shake", 0)]
[Category("Game Actions")]
[Description("Camera Shake")]
public class Flow_CameraShake : MyFlowNode
{
  public string shake_preset = "shake_small";
  public float vibro_time = 0.5f;
  public float vibro_intensity = 1f;

  public override void RegisterPorts()
  {
    ValueInput<bool> in_mute_shake_sound = this.AddValueInput<bool>("Mute sound?");
    FlowOutput flow_immediate = this.AddFlowOutput("Immediate");
    FlowOutput flow_finished = this.AddFlowOutput("Finished");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      int num = in_mute_shake_sound.value ? 1 : 0;
      ProCamera2DShake shaker = MainGame.me.GetComponent<ProCamera2DShake>();
      LazyInput.Vibrate(this.vibro_intensity, this.vibro_time);
      if (num == 0)
        DarkTonic.MasterAudio.MasterAudio.PlaySound("fall_boulder");
      if ((UnityEngine.Object) shaker != (UnityEngine.Object) null)
      {
        shaker.OnShakeCompleted = (System.Action) (() =>
        {
          shaker.OnShakeCompleted = (System.Action) null;
          flow_finished.Call(f);
        });
        shaker.Shake(Resources.Load<ShakePreset>("Shake/" + this.shake_preset));
        flow_immediate.Call(f);
      }
      else
      {
        flow_immediate.Call(f);
        flow_finished.Call(f);
      }
    }));
  }
}
