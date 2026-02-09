// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_AddLocalWeatherState
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Add Local Weather State", 0)]
[Category("Game Actions")]
[Description("Add Local Weather State")]
public class Flow_AddLocalWeatherState : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<string> in_preset_name = this.AddValueInput<string>("preset name");
    ValueInput<float> in_start_time = this.AddValueInput<float>("start time (in days)");
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
        foreach (SwitchableWeatherState l_state in SwitchableWeatherState.GetStatesFromPreset((double) Mathf.Abs(in_start_time.value) < 9.9999997473787516E-05 ? MainGame.game_time : in_start_time.value, preset))
          EnvironmentEngine.me.AddLocalWeatherState(l_state);
        flow_out.Call(f);
      }
    }));
  }
}
