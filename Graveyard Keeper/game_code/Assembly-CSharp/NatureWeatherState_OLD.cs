// Decompiled with JetBrains decompiler
// Type: NatureWeatherState_OLD
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class NatureWeatherState_OLD : WeatherStateBase
{
  public string preset_name;
  public float v_zero;
  public bool was_activated;

  public static List<NatureWeatherState_OLD> GetStatesFromPreset(
    float start_time,
    WeatherPreset preset)
  {
    List<NatureWeatherState_OLD> statesFromPreset = new List<NatureWeatherState_OLD>();
    foreach (WeatherPresetAtom presetAtom in preset.preset_atoms)
    {
      List<NatureWeatherState_OLD> natureWeatherStateOldList = statesFromPreset;
      NatureWeatherState_OLD natureWeatherStateOld = new NatureWeatherState_OLD();
      natureWeatherStateOld.preset_name = preset.preset_name;
      natureWeatherStateOld.t_start = start_time;
      natureWeatherStateOld.lut_texture = presetAtom.lut_texture;
      natureWeatherStateOld.type = presetAtom.type;
      natureWeatherStateOld.value = presetAtom.value;
      natureWeatherStateOld.t_atk = presetAtom.t_atk;
      natureWeatherStateOldList.Add(natureWeatherStateOld);
    }
    return statesFromPreset;
  }

  public override string ToString()
  {
    return $"{this.type.ToString()}={this.value.ToString()}({this.t_atk.ToString()})";
  }
}
