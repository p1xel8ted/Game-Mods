// Decompiled with JetBrains decompiler
// Type: WeatherPresetAtom
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class WeatherPresetAtom
{
  public SmartWeatherState.WeatherType type;
  public Texture2D lut_texture;
  public float value;
  public float t_atk_in_secs;

  public float t_atk => TimeOfDay.FromSecondsToTimeK(this.t_atk_in_secs);

  public override string ToString()
  {
    return $"{this.type.ToString()}={this.value.ToString()}({this.t_atk_in_secs.ToString()} sec)";
  }
}
