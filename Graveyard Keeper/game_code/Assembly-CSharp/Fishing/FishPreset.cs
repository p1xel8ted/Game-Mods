// Decompiled with JetBrains decompiler
// Type: Fishing.FishPreset
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Fishing;

[CreateAssetMenu(fileName = "FishPreset", menuName = "Mini Games/Fish Preset", order = 1)]
public class FishPreset : ScriptableObject
{
  public float catch_time = 0.8f;
  public float progress_k_in_zone = 3f;
  public float progress_k_out_of_zone = 1.5f;
  public float zero_progress_fail_time = 1f;
  public float _t;
  public float _ta;
  public float _tb;
  [Header("Curve setup")]
  [Space(10f)]
  public FishPreset.BehaviourType behaviour_type;
  public float drop_k = 1f;
  public float frequency = 1f;
  [Space(10f)]
  [Header("Amplitude setup")]
  public float amplitude = 1f;
  [Range(0.0f, 10f)]
  public float amp_offset_x;
  [Range(-1f, 1f)]
  public float amp_offset_y;
  [Range(0.0f, 10f)]
  public float amp_freq = 1f;
  [Range(0.0f, 10f)]
  public float amp_mod;
  [Range(0.0f, 10f)]
  [Space(10f)]
  public float amp_offset_2_x;
  [Range(-1f, 1f)]
  public float amp_offset_2_y;
  [Range(0.0f, 10f)]
  public float amp_freq_2 = 1f;
  [Range(0.0f, 10f)]
  public float amp_mod_2;
  [Space(10f)]
  [Header("View")]
  public float inspector_period = 1f;
  public float inspector_shift;
  public float max_amp = 1f;

  public void InitTimeCalculation(float shift = 0.0f)
  {
    this._ta = shift;
    this._tb = shift;
    this._t = shift;
  }

  public float CalculateFishPos(float delta_time, bool normalize = false)
  {
    this._t += delta_time * this.frequency;
    this._ta += delta_time * this.amp_freq * this.frequency;
    this._tb += delta_time * this.amp_freq_2 * this.frequency;
    float f = (float) ((double) Mathf.Sin(this._t) * (double) this.amplitude + ((double) this.amp_offset_y + (double) Mathf.Sin(this._ta + this.amp_offset_x) * (double) this.amp_mod) + ((double) this.amp_offset_2_y + (double) Mathf.Sin(this._tb + this.amp_offset_2_x) * (double) this.amp_mod_2));
    if ((double) Mathf.Abs(f) > (double) this.max_amp)
      this.max_amp = Mathf.Abs(f);
    if (normalize)
      f = (float) (((double) f / (double) this.max_amp + 1.0) / 2.0);
    return f;
  }

  public enum BehaviourType
  {
    Default,
    FastDown,
    FastUp,
  }
}
