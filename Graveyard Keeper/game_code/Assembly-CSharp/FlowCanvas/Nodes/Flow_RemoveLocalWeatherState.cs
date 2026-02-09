// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_RemoveLocalWeatherState
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Remove Local Weather State", 0)]
[Category("Game Actions")]
[Description("Remove Local Weather State")]
public class Flow_RemoveLocalWeatherState : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<string> in_preset_name = this.AddValueInput<string>("preset name");
    ValueInput<float> in_start_time = this.AddValueInput<float>("start time (in days)");
    ValueInput<float> in_dec_time_in_seconds = this.AddValueInput<float>("dec time (sec)");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if ((Object) WeatherPreset.GetPreset(in_preset_name.value) == (Object) null)
      {
        Debug.LogError((object) "Weather preset is null!");
      }
      else
      {
        float start_removing_time = (double) Mathf.Abs(in_start_time.value) < 9.9999997473787516E-05 ? MainGame.game_time : in_start_time.value;
        float dec_time = TimeOfDay.FromSecondsToTimeK(in_dec_time_in_seconds.value);
        if ((double) dec_time < 0.0099999997764825821)
          dec_time = 0.1f;
        EnvironmentEngine.me.TryRemoveLocalWeatherState(in_preset_name.value, start_removing_time, dec_time);
        flow_out.Call(f);
      }
    }));
  }
}
