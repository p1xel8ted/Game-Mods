// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_AddForcedWeatherState
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Add Forced Weather State")]
[Category("Game Actions")]
[Name("Add Forced Weather State", 0)]
public class Flow_AddForcedWeatherState : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<SmartWeatherState.WeatherType> in_type = this.AddValueInput<SmartWeatherState.WeatherType>("type");
    ValueInput<Texture2D> in_lut_texture = this.AddValueInput<Texture2D>("lut_texture");
    ValueInput<float> in_value = this.AddValueInput<float>("value");
    ValueInput<float> in_start_time = this.AddValueInput<float>("start_time (Time K)");
    ValueInput<float> in_t_atk_in_seconds = this.AddValueInput<float>("atk_time (sec)");
    ValueInput<float> in_t_flat_in_seconds = this.AddValueInput<float>("flat_time (sec)");
    ValueInput<float> in_t_dec_in_seconds = this.AddValueInput<float>("dec_time (sec)");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if ((double) in_value.value < 0.0 || (double) in_value.value > 5.0)
        Debug.LogError((object) "Forced weather :: wrong value.");
      else if ((double) in_start_time.value < 1.0)
      {
        Debug.LogError((object) "Forced weather :: wrong start time.");
      }
      else
      {
        EnvironmentEngine.me.AddForcedWeatherState(new ForcedWeatherState(in_type.value, in_lut_texture.value, in_value.value, in_start_time.value, TimeOfDay.FromSecondsToTimeK(in_t_atk_in_seconds.value), TimeOfDay.FromSecondsToTimeK(in_t_flat_in_seconds.value), TimeOfDay.FromSecondsToTimeK(in_t_dec_in_seconds.value)));
        flow_out.Call(f);
      }
    }));
  }
}
