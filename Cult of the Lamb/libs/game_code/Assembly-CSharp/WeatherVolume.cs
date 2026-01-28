// Decompiled with JetBrains decompiler
// Type: WeatherVolume
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMBiomeGeneration;
using UnityEngine;

#nullable disable
public class WeatherVolume : MonoBehaviour
{
  [SerializeField]
  public WeatherSystemController.WeatherType weatherType;
  [SerializeField]
  public WeatherSystemController.WeatherStrength weatherStrength;
  [SerializeField]
  public float transition = 2f;
  public bool triggered;
  public WeatherSystemController.WeatherType cachedWeatherType;
  public WeatherSystemController.WeatherStrength cachedWeatherStrength;

  public void OnTriggerEnter2D(Collider2D other)
  {
    if ((Object) BiomeGenerator.Instance == (Object) null || !BiomeGenerator.Instance.CurrentRoom.Active || this.triggered)
      return;
    this.triggered = true;
    this.cachedWeatherType = WeatherSystemController.Instance.CurrentWeatherType;
    this.cachedWeatherStrength = WeatherSystemController.Instance.CurrentWeatherStrength;
    this.SetWeather(this.weatherType, this.weatherStrength);
  }

  public void OnTriggerExit2D(Collider2D collision)
  {
    if (this.triggered)
      this.SetWeather(this.cachedWeatherType, this.cachedWeatherStrength);
    this.triggered = false;
  }

  public void OnDestroy()
  {
    if (this.triggered)
      this.SetWeather(this.cachedWeatherType, this.cachedWeatherStrength);
    this.triggered = false;
  }

  public void SetWeather(
    WeatherSystemController.WeatherType type,
    WeatherSystemController.WeatherStrength strength)
  {
    if (type == WeatherSystemController.WeatherType.None)
      WeatherSystemController.Instance.StopCurrentWeather(this.transition);
    else
      WeatherSystemController.Instance.SetWeather(type, strength, this.transition);
  }
}
