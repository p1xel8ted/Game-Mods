// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_AddNatureWeatherState
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Add Nature Weather State")]
[Category("Game Actions")]
[Name("Add Nature Weather State", 0)]
public class Flow_AddNatureWeatherState : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<string> in_preset_name = this.AddValueInput<string>("preset name");
    ValueInput<float> in_start_time = this.AddValueInput<float>("start time (in days)");
    ValueInput<float> in_remove_time = this.AddValueInput<float>("remove time (in days)");
    ValueInput<float> in_dec_time = this.AddValueInput<float>("dec time (sec)");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WeatherPreset preset = WeatherPreset.GetPreset(in_preset_name.value);
      if ((Object) preset == (Object) null)
      {
        Debug.LogError((object) "Weather preset is null!");
      }
      else
      {
        float start_time = (double) Mathf.Abs(in_start_time.value) < 9.9999997473787516E-05 ? MainGame.game_time : in_start_time.value;
        if ((double) in_remove_time.value < (double) start_time)
        {
          Debug.LogError((object) $"Wrong remove time for preset {preset.preset_name}: {{{start_time.ToString()}, {in_remove_time.value.ToString()}}}");
        }
        else
        {
          float timeK = TimeOfDay.FromSecondsToTimeK(in_dec_time.value);
          if ((double) timeK < 0.00050000002374872565)
          {
            Debug.LogError((object) $"Wrong dec_time for preset {preset.preset_name}: {in_dec_time.value.ToString()}}}");
          }
          else
          {
            foreach (SwitchableWeatherState n_state in SwitchableWeatherState.GetStatesFromPreset(start_time, preset))
            {
              EnvironmentEngine.me.AddNatureWeatherState(n_state);
              EnvironmentEngine.me.TryRemoveNatureWeatherState(preset.preset_name, in_remove_time.value, timeK);
            }
            flow_out.Call(f);
          }
        }
      }
    }));
  }
}
