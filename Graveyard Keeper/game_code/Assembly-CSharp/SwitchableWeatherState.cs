// Decompiled with JetBrains decompiler
// Type: SwitchableWeatherState
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class SwitchableWeatherState : WeatherStateBase
{
  public string preset_name;
  public bool do_dec_now;
  public float start_removing_time;
  public float t_dec;

  public static List<SwitchableWeatherState> GetStatesFromPreset(
    float start_time,
    WeatherPreset preset)
  {
    List<SwitchableWeatherState> statesFromPreset = new List<SwitchableWeatherState>();
    foreach (WeatherPresetAtom presetAtom in preset.preset_atoms)
    {
      List<SwitchableWeatherState> switchableWeatherStateList = statesFromPreset;
      SwitchableWeatherState switchableWeatherState = new SwitchableWeatherState();
      switchableWeatherState.preset_name = preset.preset_name;
      switchableWeatherState.t_start = start_time;
      switchableWeatherState.lut_texture = presetAtom.lut_texture;
      switchableWeatherState.type = presetAtom.type;
      switchableWeatherState.value = presetAtom.value;
      switchableWeatherState.t_atk = presetAtom.t_atk;
      switchableWeatherState.do_dec_now = false;
      switchableWeatherState.start_removing_time = -1f;
      switchableWeatherState.t_dec = 0.0f;
      switchableWeatherStateList.Add(switchableWeatherState);
    }
    return statesFromPreset;
  }

  public override string ToString()
  {
    return $"{this.type.ToString()}={this.value.ToString()}({this.t_atk.ToString()})";
  }

  public bool HasRemoveCommand()
  {
    return this.do_dec_now || (double) this.start_removing_time > (double) MainGame.game_time;
  }
}
