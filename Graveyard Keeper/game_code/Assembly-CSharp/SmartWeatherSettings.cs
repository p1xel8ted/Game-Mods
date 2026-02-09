// Decompiled with JetBrains decompiler
// Type: SmartWeatherSettings
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[CreateAssetMenu(fileName = "SmartWeatherSettings", menuName = "SmartWeatherSettings", order = 1)]
public class SmartWeatherSettings : ScriptableObject
{
  public List<SmartWeatherSettingsAtom> weather_settings;

  public static SmartWeatherSettings GetSettingsPreset(string preset_name)
  {
    if (string.IsNullOrEmpty(preset_name))
    {
      Debug.LogError((object) "Preset name is empty!");
      return (SmartWeatherSettings) null;
    }
    SmartWeatherSettings settingsPreset = Resources.Load<SmartWeatherSettings>("Weather/Settings/" + preset_name);
    if (!((Object) settingsPreset == (Object) null))
      return settingsPreset;
    Debug.LogError((object) $"Failed to load SmartWeatherSettings Weather/Settings/{preset_name}.asset");
    return settingsPreset;
  }

  public WeatherPreset GetWeatherPreset()
  {
    if (this.weather_settings == null || this.weather_settings.Count == 0)
    {
      Debug.LogError((object) $"SmartWeatherPreset {this.name} is FUCKING BROKEN!!!");
      return (WeatherPreset) null;
    }
    if (this.weather_settings.Count == 1)
      return this.weather_settings[0].weather_preset;
    float maxInclusive = 0.0f;
    foreach (SmartWeatherSettingsAtom weatherSetting in this.weather_settings)
      maxInclusive += weatherSetting.weight;
    float num1 = Random.Range(0.0f, maxInclusive);
    float num2 = 0.0f;
    foreach (SmartWeatherSettingsAtom weatherSetting in this.weather_settings)
    {
      num2 += weatherSetting.weight;
      if ((double) num2 > (double) num1)
        return weatherSetting.weather_preset;
    }
    Debug.LogError((object) $"Some weird stuff happen! [rand = {num1.ToString()}], [max = {maxInclusive.ToString()}]");
    return (WeatherPreset) null;
  }
}
