// Decompiled with JetBrains decompiler
// Type: SmartAudioSound
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DarkTonic.MasterAudio;
using System;
using UnityEngine;

#nullable disable
[Serializable]
public class SmartAudioSound
{
  public string id;
  [SoundGroup]
  public string sound_group = "[None]";
  public SmartAudioSound.TimeLimitation time_limitation;
  public float _cur_weight = 1f;
  public float _target_weight;
  public float _volume = 1f;
  public bool _weight_animating;
  public float fade_speed = 0.3f;
  public float fade_out_speed_k = 2f;

  public void SetSoundWeight(float weight, bool is_weight_animate = true)
  {
    this._target_weight = weight;
    this._weight_animating = is_weight_animate;
  }

  public void CustomUpdate(float delta_time)
  {
    if (this._weight_animating)
    {
      float f = this._target_weight - this._cur_weight;
      float num = delta_time * this.fade_speed;
      if ((double) f < 0.0)
        num *= this.fade_out_speed_k;
      if ((double) Mathf.Abs(f) < (double) num)
      {
        this._cur_weight = this._target_weight;
        this._weight_animating = false;
      }
      else
      {
        this._cur_weight += num * Mathf.Sign(f);
        if ((int) Mathf.Sign(this._target_weight - this._cur_weight) != (int) Mathf.Sign(f))
        {
          this._cur_weight = this._target_weight;
          this._weight_animating = false;
        }
      }
    }
    else
      this._cur_weight = this._target_weight;
    this.OnWeightChanged();
  }

  public void OnWeightChanged()
  {
    float num1 = 1f;
    float num2 = Mathf.Abs(TimeOfDay.me.time_of_day);
    switch (this.time_limitation)
    {
      case SmartAudioSound.TimeLimitation.OnlyAtNight:
        num1 *= num2;
        break;
      case SmartAudioSound.TimeLimitation.OnlyAtDay:
        num1 *= 1f - num2;
        break;
    }
    if (this.sound_group == "[None]")
      return;
    DarkTonic.MasterAudio.MasterAudio.SetGroupVolume(this.sound_group, this._cur_weight * num1 * this._volume);
  }

  public void Play() => DarkTonic.MasterAudio.MasterAudio.PlaySound(this.sound_group);

  public void PlayWithFade()
  {
    DarkTonic.MasterAudio.MasterAudio.PlaySound(this.sound_group);
    this.SetSoundWeight(1f);
  }

  public void StopWithFade() => this.SetSoundWeight(0.0f);

  public void Stop() => this.SetSoundWeight(0.0f, false);

  public void SetSoundVolume(float volume)
  {
    this._volume = volume;
    this.CustomUpdate(Time.deltaTime);
  }

  public enum TimeLimitation
  {
    AllDayLong,
    OnlyAtNight,
    OnlyAtDay,
  }
}
