// Decompiled with JetBrains decompiler
// Type: TimeOfDay
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class TimeOfDay : MonoBehaviour
{
  public Color WARMING_LIGHT = new Color(0.09671455f, 0.06246148f, 0.0f);
  public const float MORNING = 0.15f;
  public const float DAYTIME = 0.35f;
  public const float EVENING = 0.7f;
  public const float NIGHT = 0.0f;
  public Gradient light_grd = new Gradient();
  public Gradient ambient_grd = new Gradient();
  public Gradient light_sprites = new Gradient();
  public AnimationCurve lut_amount = new AnimationCurve();
  public AnimationCurve shadow_alpha = new AnimationCurve();
  public AnimationCurve light_intensity = new AnimationCurve();
  public AnimationCurve grain_intensity = new AnimationCurve();
  public static Color light_sprites_color = Color.white;
  public List<float> lut_times = new List<float>();
  public List<Texture> lut_textures = new List<Texture>();
  [NonSerialized]
  public static TimeOfDay _me = (TimeOfDay) null;
  [NonSerialized]
  public static bool _me_is_set = false;
  [Range(-1f, 1f)]
  public float time_of_day;
  public float _prev_time_of_day;
  public static float shadow_alpha_k = 1f;
  public static float light_intensity_k = 1f;
  public static float global_shadows_alpha = 1f;
  public const float SECONDS_IN_DAY = 450f;

  public TimeOfDay.TimeOfDayEnum time_of_day_enum
  {
    get
    {
      float timeK = this.GetTimeK();
      if ((double) timeK < 0.15000000596046448)
        return TimeOfDay.TimeOfDayEnum.Night;
      if ((double) timeK < 0.34999999403953552)
        return TimeOfDay.TimeOfDayEnum.Morning;
      return (double) timeK < 0.699999988079071 ? TimeOfDay.TimeOfDayEnum.Day : TimeOfDay.TimeOfDayEnum.Evening;
    }
  }

  public bool is_night
  {
    get
    {
      float timeK = this.GetTimeK();
      return (double) timeK < 0.25 || (double) timeK > 0.75;
    }
  }

  public static TimeOfDay me
  {
    get
    {
      if (TimeOfDay._me_is_set)
        return TimeOfDay._me;
      TimeOfDay._me = UnityEngine.Object.FindObjectOfType<TimeOfDay>();
      TimeOfDay._me_is_set = true;
      return TimeOfDay._me;
    }
  }

  public float GetTimeK() => (float) (((double) this.time_of_day + 1.0) / 2.0);

  public void SetTimeK(float k)
  {
    this.time_of_day = (float) ((double) k * 2.0 - 1.0);
    EnvironmentEngine.SetTime(this.time_of_day);
  }

  public AmplifyColorEffect lut_effect => EnvironmentEngine.me.lut_effect_timeofday;

  public void Update()
  {
    if ((UnityEngine.Object) EnvironmentEngine.me == (UnityEngine.Object) null || !MainGame.game_started)
      return;
    float num = this.GetTimeK();
    TimeOfDay.global_shadows_alpha = 1f;
    Gradient lightGrd = this.light_grd;
    Gradient ambientGrd = this.ambient_grd;
    Gradient lightSprites = this.light_sprites;
    EnvironmentPreset curPreset = EnvironmentEngine.cur_preset;
    if ((UnityEngine.Object) curPreset != (UnityEngine.Object) null)
    {
      if (curPreset.light_override)
        lightGrd = curPreset.light_grd;
      if (curPreset.ambient_light_override)
        ambientGrd = curPreset.ambient_grd;
      if (curPreset.light_sprites_override)
        lightSprites = curPreset.light_sprites;
      if (curPreset.force_static_time)
        num = curPreset.static_time_value;
      if (curPreset.force_global_shadows_alpha)
        TimeOfDay.global_shadows_alpha = curPreset.global_shadows_alpha;
    }
    lightGrd.Evaluate(num);
    Color color = ambientGrd.Evaluate(num);
    TimeOfDay.light_sprites_color = lightSprites.Evaluate(num);
    Color warmingLight = this.WARMING_LIGHT;
    RenderSettings.ambientLight = color + warmingLight;
    TimeOfDay.shadow_alpha_k = this.shadow_alpha.Evaluate(num);
    TimeOfDay.light_intensity_k = this.light_intensity.Evaluate(num);
    if (Application.isPlaying && (UnityEngine.Object) MainGame.me.grain_fx_component != (UnityEngine.Object) null)
      MainGame.me.grain_fx_component.intensityMultiplier = this.grain_intensity.Evaluate(!((UnityEngine.Object) curPreset != (UnityEngine.Object) null) || !curPreset.force_light_intensity ? num : 0.5f);
    if ((UnityEngine.Object) curPreset != (UnityEngine.Object) null)
    {
      if (curPreset.force_shadows_alpha)
        TimeOfDay.shadow_alpha_k = curPreset.shadows_alpha;
      if (curPreset.force_light_intensity)
        TimeOfDay.light_intensity_k = curPreset.light_intensity;
    }
    if (!((UnityEngine.Object) this.lut_effect != (UnityEngine.Object) null))
      return;
    this.CalculateLutBlend(num, out this.lut_effect.BlendAmount, out this.lut_effect.LutTexture, out this.lut_effect.LutBlendTexture);
  }

  public void CalculateLutBlend(float time_k, out float blend, out Texture t1, out Texture t2)
  {
    blend = 0.0f;
    t1 = t2 = (Texture) null;
    if (this.lut_times.Count == 0)
      return;
    for (int index = 0; index < this.lut_times.Count; ++index)
    {
      float lutTime = this.lut_times[index];
      t1 = this.lut_textures[index];
      float num;
      if (index == this.lut_times.Count - 1)
      {
        num = 1f;
        t2 = this.lut_textures[0];
      }
      else
      {
        num = this.lut_times[index + 1];
        t2 = this.lut_textures[index + 1];
      }
      if ((double) Math.Abs(num - lutTime) >= 0.001 && (double) time_k >= (double) lutTime - 0.001 && (double) time_k < (double) num)
      {
        if ((double) Math.Abs(time_k - lutTime) < 0.001)
        {
          blend = 0.0f;
          return;
        }
        blend = (float) (((double) time_k - (double) lutTime) / ((double) num - (double) lutTime));
        return;
      }
    }
    blend = 1f;
  }

  public static float FromTimeKToSeconds(float time_in_time_k) => time_in_time_k * 450f;

  public static float FromSecondsToTimeK(float time_in_secs) => time_in_secs / 450f;

  public TimeOfDay.SerializedTimeOfDay ToSerialized()
  {
    return new TimeOfDay.SerializedTimeOfDay()
    {
      time_of_day = this.time_of_day,
      prev_time_of_day = this._prev_time_of_day
    };
  }

  public void FromSerialized(TimeOfDay.SerializedTimeOfDay data)
  {
    if (data == null)
      return;
    this.time_of_day = data.time_of_day;
    this._prev_time_of_day = data.prev_time_of_day;
  }

  public float GetSecondsToTheMidnight() => (float) ((1.0 - (double) this.GetTimeK()) * 450.0);

  public float GetSecondsToTheMorning()
  {
    float num = this.GetTimeK() - 0.15f;
    return (double) num >= 0.0 ? (float) ((1.0 - (double) this.GetTimeK() + 0.15000000596046448) * 450.0) : (float) ((double) num * -1.0 * 450.0);
  }

  public enum TimeOfDayEnum
  {
    Night,
    Morning,
    Day,
    Evening,
  }

  [Serializable]
  public class SerializedTimeOfDay
  {
    public float time_of_day;
    public float prev_time_of_day;
  }
}
