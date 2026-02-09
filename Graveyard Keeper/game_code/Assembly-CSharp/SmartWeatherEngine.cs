// Decompiled with JetBrains decompiler
// Type: SmartWeatherEngine
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class SmartWeatherEngine
{
  public static SmartWeatherEngine _me;
  public SmartWeatherSettings night;
  public SmartWeatherSettings morning;
  public SmartWeatherSettings day;
  public SmartWeatherSettings evening;

  public static SmartWeatherEngine me
  {
    get
    {
      if (SmartWeatherEngine._me == null)
      {
        SmartWeatherEngine._me = new SmartWeatherEngine();
        SmartWeatherEngine._me.Init();
      }
      return SmartWeatherEngine._me;
    }
  }

  public void Init()
  {
    this.night = SmartWeatherSettings.GetSettingsPreset("night");
    this.morning = SmartWeatherSettings.GetSettingsPreset("morning");
    this.day = SmartWeatherSettings.GetSettingsPreset("day");
    this.evening = SmartWeatherSettings.GetSettingsPreset("evening");
  }

  public void UpdateWeather()
  {
    int day = MainGame.me.save.day;
    for (int index = 0; index < 4; ++index)
    {
      float num1 = (float) day;
      SmartWeatherSettings smartWeatherSettings;
      float num2;
      if (index == 0)
      {
        smartWeatherSettings = this.night;
        num2 = num1 + 0.0f;
      }
      else if (index == 1)
      {
        smartWeatherSettings = this.morning;
        num2 = num1 + 0.15f;
      }
      else if (index == 2)
      {
        smartWeatherSettings = this.day;
        num2 = num1 + 0.35f;
      }
      else if (index == 3)
      {
        smartWeatherSettings = this.evening;
        num2 = num1 + 0.7f;
      }
      else
      {
        Debug.LogError((object) "HOW THE FUCK THIS CAN HAPEN???!!!");
        break;
      }
      List<string> natureWithoutRemoves = EnvironmentEngine.me.FindNatureWithoutRemoves();
      if (natureWithoutRemoves != null && natureWithoutRemoves.Count > 0)
      {
        foreach (string preset_name in natureWithoutRemoves)
          EnvironmentEngine.me.TryRemoveNatureWeatherState(preset_name, num2, TimeOfDay.FromSecondsToTimeK(10f));
      }
      WeatherPreset weatherPreset = smartWeatherSettings.GetWeatherPreset();
      if ((UnityEngine.Object) weatherPreset == (UnityEngine.Object) null)
      {
        Debug.LogError((object) $"SmartWeatherEngine::{smartWeatherSettings.name} weather_preset is null!");
        break;
      }
      Debug.Log((object) $"#weather#Weather for {smartWeatherSettings.name}: [{num2.ToString()}:{weatherPreset.name}]");
      foreach (SwitchableWeatherState n_state in SwitchableWeatherState.GetStatesFromPreset(num2, weatherPreset))
        EnvironmentEngine.me.AddNatureWeatherState(n_state);
    }
  }
}
