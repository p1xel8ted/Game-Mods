// Decompiled with JetBrains decompiler
// Type: ForcedWeatherState
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class ForcedWeatherState : WeatherStateBase
{
  public float t_flat;
  public float t_dec;

  public ForcedWeatherState(
    SmartWeatherState.WeatherType type,
    Texture2D lut_texture,
    float value,
    float t_start,
    float t_atk,
    float t_flat,
    float t_dec)
  {
    this.type = type;
    this.lut_texture = lut_texture;
    this.value = value;
    this.t_start = t_start;
    this.t_atk = t_atk;
    this.t_flat = t_flat;
    this.t_dec = t_dec;
  }
}
