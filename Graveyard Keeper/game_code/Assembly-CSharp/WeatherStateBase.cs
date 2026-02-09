// Decompiled with JetBrains decompiler
// Type: WeatherStateBase
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class WeatherStateBase
{
  public float t_start;
  public float t_atk;
  public SmartWeatherState.WeatherType type;
  public float value;
  public Texture2D _lut_texture;
  [SerializeField]
  public string _lut_texture_name;
  public string _cached_lut_texture_name = string.Empty;

  public Texture2D lut_texture
  {
    get
    {
      if (this._cached_lut_texture_name != this._lut_texture_name)
      {
        this._cached_lut_texture_name = this._lut_texture_name;
        this._lut_texture = string.IsNullOrEmpty(this._lut_texture_name) ? (Texture2D) null : Resources.Load<Texture2D>(this._lut_texture_name);
      }
      return this._lut_texture;
    }
    set
    {
      this._lut_texture = value;
      this._cached_lut_texture_name = this._lut_texture_name = (UnityEngine.Object) value == (UnityEngine.Object) null ? string.Empty : value.name;
    }
  }
}
