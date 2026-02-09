// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_SetWeatherForced
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Set Weather Forced", 0)]
[Color("000000")]
[Description("Set Weather Now. Use only when time is disabled")]
public class Flow_SetWeatherForced : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<float> in_rain = this.AddValueInput<float>("Rain");
    ValueInput<float> in_wind = this.AddValueInput<float>("Wind");
    ValueInput<float> in_fog = this.AddValueInput<float>("Fog");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      Debug.Log((object) nameof (Flow_SetWeatherForced));
      foreach (SmartWeatherState state in EnvironmentEngine.me.states)
      {
        if (!((UnityEngine.Object) state == (UnityEngine.Object) null))
        {
          switch (state.type)
          {
            case SmartWeatherState.WeatherType.Rain:
              state.forced_value = in_rain.value;
              continue;
            case SmartWeatherState.WeatherType.Fog:
              state.forced_value = in_fog.value;
              continue;
            case SmartWeatherState.WeatherType.Wind:
              state.forced_value = in_wind.value;
              continue;
            case SmartWeatherState.WeatherType.LUT:
              state.SetValueImmediate(0.0f);
              Debug.LogError((object) "WHAT??? LUT??? Call Bulat!");
              continue;
            default:
              throw new ArgumentOutOfRangeException();
          }
        }
      }
      flow_out.Call(f);
    }));
  }
}
