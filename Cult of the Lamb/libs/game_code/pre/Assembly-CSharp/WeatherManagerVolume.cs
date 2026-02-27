// Decompiled with JetBrains decompiler
// Type: WeatherManagerVolume
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class WeatherManagerVolume
{
  public WeatherManagerVolume.ShaderTypes types;
  public WeatherManagerVolume.WeatherEffect weatherEffect;
  public float valueToGoTo;
  public Vector2 Vector2ToGoTo;
  public float StartvalueToGoTo;
  public Vector2 StartVector2ToGoTo;

  public WeatherManagerVolume(
    WeatherManagerVolume.ShaderTypes types,
    WeatherManagerVolume.WeatherEffect weatherEffect,
    Vector2 valueToGoTo)
  {
    this.types = types;
    this.weatherEffect = weatherEffect;
    this.Vector2ToGoTo = valueToGoTo;
  }

  public WeatherManagerVolume(
    WeatherManagerVolume.ShaderTypes types,
    WeatherManagerVolume.WeatherEffect weatherEffect,
    float valueToGoTo)
  {
    this.types = types;
    this.weatherEffect = weatherEffect;
    this.valueToGoTo = valueToGoTo;
  }

  public enum ShaderTypes
  {
    Float,
    Vector2,
  }

  public enum WeatherEffect
  {
    Null,
    WindDensity,
    WindSpeed,
    WindDirection,
    RainIntensity,
  }
}
