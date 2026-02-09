// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_SetWeatherImmediate
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Color("000000")]
[Description("Set Weather Now. Use only when time is disabled")]
[Category("Game Actions")]
[Name("Set Weather Immediate", 0)]
public class Flow_SetWeatherImmediate : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<float> in_rain = this.AddValueInput<float>("Rain");
    ValueInput<float> in_wind = this.AddValueInput<float>("Wind");
    ValueInput<float> in_fog = this.AddValueInput<float>("Fog");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      foreach (SmartWeatherState state in EnvironmentEngine.me.states)
      {
        if (!((UnityEngine.Object) state == (UnityEngine.Object) null))
        {
          switch (state.type)
          {
            case SmartWeatherState.WeatherType.Rain:
              state.SetValueImmediate(in_rain.value);
              continue;
            case SmartWeatherState.WeatherType.Fog:
              state.SetValueImmediate(in_fog.value);
              continue;
            case SmartWeatherState.WeatherType.Wind:
              state.SetValueImmediate(in_wind.value);
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
