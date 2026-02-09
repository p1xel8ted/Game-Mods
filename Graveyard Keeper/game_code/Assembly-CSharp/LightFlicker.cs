// Decompiled with JetBrains decompiler
// Type: LightFlicker
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class LightFlicker : MonoBehaviour
{
  public int fps = 30;
  public List<LightFlicker.LightFlickerStep> steps = new List<LightFlicker.LightFlickerStep>();
  public float _last_time;
  public float _dt;
  public int _cur_frame_counter;
  public int _cur_step;
  public int _cur_step_len;
  public Light _light;
  public float trans_time;
  public bool _is_first = true;
  public bool _is_transing;
  public float _trans_t0;
  public Color c0;
  public Color c1;

  public void Start()
  {
    this._last_time = Time.realtimeSinceStartup;
    this._dt = 0.0f;
    this._cur_frame_counter = 0;
    this._cur_step = 0;
    this._light = this.GetComponent<Light>();
    this._is_first = true;
    this.GenerateStep();
  }

  public void GenerateStep()
  {
    int count = this.steps.Count;
    if (count == 0)
      return;
    if (this._cur_step >= count)
      this._cur_step = 0;
    LightFlicker.LightFlickerStep step = this.steps[this._cur_step];
    this._cur_step_len = step.len2 != 0 ? UnityEngine.Random.Range(step.len, step.len2) : step.len;
    this._cur_step_len += Mathf.RoundToInt(this.trans_time * (float) this.fps);
    this._trans_t0 = 0.0f;
    if (this._is_first)
    {
      this._light.color = step.c;
      this._is_first = false;
    }
    else
    {
      this._is_transing = true;
      this.c1 = step.c;
      this.c0 = this._light.color;
    }
  }

  public void Update()
  {
    if (this.steps.Count == 0)
      return;
    float num1 = Time.realtimeSinceStartup - this._last_time;
    this._trans_t0 += Time.deltaTime;
    if (this._is_transing)
    {
      float t = this._trans_t0 / this.trans_time;
      this._light.color = new Color(Mathf.Lerp(this.c0.r, this.c1.r, t), Mathf.Lerp(this.c0.g, this.c1.g, t), Mathf.Lerp(this.c0.b, this.c1.b, t));
      if ((double) this._trans_t0 > (double) this.trans_time)
      {
        this._is_transing = false;
        this._is_first = false;
      }
    }
    this._last_time = Time.realtimeSinceStartup;
    this._dt += num1;
    float num2 = 1f / (float) this.fps;
    if ((double) this._dt <= (double) num2)
      return;
    this._dt -= num2;
    ++this._cur_frame_counter;
    if (this._cur_step >= this.steps.Count)
    {
      this._cur_step = 0;
    }
    else
    {
      if (this._cur_frame_counter < this._cur_step_len)
        return;
      ++this._cur_step;
      this._cur_frame_counter = 0;
      this.GenerateStep();
    }
  }

  [Serializable]
  public class LightFlickerStep
  {
    public int len;
    public int len2;
    public Color c;
  }
}
