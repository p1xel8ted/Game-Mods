// Decompiled with JetBrains decompiler
// Type: WeatherState
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class WeatherState : MonoBehaviour
{
  public WeatherState.WeatherType type;
  public WeatherState.State _state;
  public float _time_left;
  public float _time_total = 10f;
  public float _cur_stay_time;
  public const float FADE_IN_TIME = 10f;
  public const float FADE_OUT_TIME = 10f;
  public const float STAY_TIME = 10f;
  public float _amount;

  public void StateUpdate(float delta_time)
  {
    this._time_left -= delta_time;
    float a = 0.0f;
    if ((double) this._time_total > 0.0)
      a = this._time_left / this._time_total;
    switch (this._state)
    {
      case WeatherState.State.Off:
        break;
      case WeatherState.State.FadeIn:
        if ((double) this._time_left <= 0.0)
        {
          this._state = WeatherState.State.On;
          this.SetWeatherAmount(1f);
          this._time_left = this._time_total = this._cur_stay_time;
          break;
        }
        this.SetWeatherAmount(1f - a);
        break;
      case WeatherState.State.On:
        if ((double) this._time_left > 0.0)
          break;
        this.DoFadeOut(10f);
        break;
      case WeatherState.State.FadeOut:
        if ((double) this._time_left <= 0.0)
        {
          this._state = WeatherState.State.Off;
          this.SetWeatherAmount(0.0f);
          break;
        }
        this.SetWeatherAmount(a);
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  public void DoFadeIn(float fade_time)
  {
    this._time_left = this._time_total = fade_time;
    this._state = WeatherState.State.FadeIn;
  }

  public void DoFadeOut(float fade_time)
  {
    this._time_left = this._time_total = fade_time;
    this._state = WeatherState.State.FadeOut;
  }

  public void Set(WeatherState.SetMethod method = WeatherState.SetMethod.Fade)
  {
    this._cur_stay_time = 10f;
    if (method != WeatherState.SetMethod.ImmidiateOff)
    {
      if (method == WeatherState.SetMethod.ImmidiateOn)
      {
        this.SetWeatherAmount(1f);
        this._state = WeatherState.State.On;
      }
      else
      {
        switch (this._state)
        {
          case WeatherState.State.Off:
            this.DoFadeIn(10f);
            break;
          case WeatherState.State.FadeIn:
            break;
          case WeatherState.State.On:
            this._time_left = 10f;
            break;
          case WeatherState.State.FadeOut:
            float num = 1f - Mathf.Clamp01(this._time_left / this._time_total);
            this.DoFadeIn(10f);
            this._time_left = 10f * num;
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
    }
    else
    {
      this.SetWeatherAmount(0.0f);
      this._state = WeatherState.State.Off;
    }
  }

  public void SetWeatherAmount(float a)
  {
    this._amount = a;
    this.WeatherAmountDelegate(Mathf.Clamp01(a));
  }

  public virtual void WeatherAmountDelegate(float a)
  {
  }

  public float amount => this._amount;

  public enum WeatherType
  {
    Rain,
    Fog,
    Wind,
  }

  public enum State
  {
    Off,
    FadeIn,
    On,
    FadeOut,
  }

  public enum SetMethod
  {
    Fade,
    ImmidiateOff,
    ImmidiateOn,
  }
}
