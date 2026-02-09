// Decompiled with JetBrains decompiler
// Type: SmartWeatherState
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class SmartWeatherState : MonoBehaviour
{
  public SmartController controller;
  [Range(0.0f, 5f)]
  public float value;
  [Range(0.0f, 5f)]
  public float forced_value;
  public float nature_value;
  [SerializeField]
  public float _cur_amount;
  public float speed = 0.25f;
  public float max = 5f;
  public SmartWeatherState.WeatherType type;
  [SerializeField]
  public bool _enabled = true;
  [SerializeField]
  public bool _previously_enabled = true;

  public void Update()
  {
    if ((UnityEngine.Object) this.controller == (UnityEngine.Object) null)
      return;
    if ((double) this.value > (double) this.max)
      this.value = this.max;
    double f1 = (double) this.value - (double) this._cur_amount;
    int num = (int) Mathf.Sign((float) f1);
    if ((double) Mathf.Abs((float) f1) < 0.01)
    {
      if (this._previously_enabled == this._enabled)
        return;
      num = 0;
    }
    this._cur_amount += this.speed * Time.deltaTime * (float) num;
    float f2 = this.value - this._cur_amount;
    if (num != (int) Mathf.Sign(f2))
      this._cur_amount = this.value;
    this._previously_enabled = this._enabled;
    if (!this._enabled)
    {
      this.controller.value = 0.0f;
      this.UpdateWeatherVolume(this.controller.value);
    }
    else
    {
      this.controller.value = this._cur_amount;
      this.UpdateWeatherVolume(this.controller.value);
    }
  }

  public void SetValueImmediate(float v)
  {
    this.controller.value = this._cur_amount = this.value = v;
    this.UpdateWeatherVolume(this.controller.value);
  }

  public void SetEnabled(bool state_enabled)
  {
    this._enabled = state_enabled;
    this.SetWeatherSoundEnable(this._enabled);
    this.Update();
  }

  public SmartWeatherState.SerializedWeatherState Serialize()
  {
    return new SmartWeatherState.SerializedWeatherState()
    {
      value = this.value,
      name = this.name,
      type = this.type,
      enabled = this._enabled,
      max = this.max,
      controller_value = this.controller.value,
      cur_amount = this._cur_amount,
      forced_value = this.forced_value,
      nature_value = this.nature_value,
      previously_enabled = this._previously_enabled,
      speed = this.speed
    };
  }

  public void Deserialize(SmartWeatherState.SerializedWeatherState state)
  {
    this.value = state.value;
    this.name = state.name;
    this.type = state.type;
    this._enabled = state.enabled;
    this.max = state.max;
    this.controller.value = state.controller_value;
    this._cur_amount = state.cur_amount;
    this.forced_value = state.forced_value;
    this.nature_value = state.nature_value;
    this._previously_enabled = state.previously_enabled;
    this.speed = state.speed;
  }

  public void SetWeatherSoundEnable(bool is_enable)
  {
    string weatherMusicId = this.GetWeatherMusicId();
    if (string.IsNullOrEmpty(weatherMusicId))
      return;
    if (is_enable)
    {
      SmartAudioEngine.me.SetSoundVolume(weatherMusicId, 0.0f);
      SmartAudioEngine.me.SetSoundWeight(weatherMusicId, 1f);
      SmartAudioEngine.me.PlaySound(weatherMusicId);
    }
    else
      SmartAudioEngine.me.StopSoundWithFade(weatherMusicId);
  }

  public void UpdateWeatherVolume(float weather_value)
  {
    if (!this._enabled)
      return;
    string weatherMusicId = this.GetWeatherMusicId();
    if (string.IsNullOrEmpty(weatherMusicId))
      return;
    switch (this.type)
    {
      case SmartWeatherState.WeatherType.Rain:
      case SmartWeatherState.WeatherType.Wind:
        float volume = weather_value / this.max;
        SmartAudioEngine.me.SetSoundVolume(weatherMusicId, volume);
        break;
    }
  }

  public string GetWeatherMusicId()
  {
    string weatherMusicId = string.Empty;
    switch (this.type)
    {
      case SmartWeatherState.WeatherType.Rain:
        weatherMusicId = "rain_environment";
        break;
      case SmartWeatherState.WeatherType.Wind:
        weatherMusicId = "wind_environment";
        break;
    }
    return weatherMusicId;
  }

  [Serializable]
  public enum WeatherType
  {
    Rain,
    Fog,
    Wind,
    LUT,
  }

  [Serializable]
  public struct SerializedWeatherState
  {
    public string name;
    public float controller_value;
    public float value;
    public float forced_value;
    public float nature_value;
    public float cur_amount;
    public float speed;
    public float max;
    public SmartWeatherState.WeatherType type;
    public bool enabled;
    public bool previously_enabled;
  }
}
