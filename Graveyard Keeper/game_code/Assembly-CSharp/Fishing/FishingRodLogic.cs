// Decompiled with JetBrains decompiler
// Type: Fishing.FishingRodLogic
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Fishing;

public class FishingRodLogic
{
  public FishingRodPreset _rod_preset;
  public float _pos;
  public float _max_height;
  public float _screen_k;
  public float _t;
  public float _y0;
  public float _v0;
  public float _v;
  public bool _pulling_fish;

  public float rect_size => (float) this._rod_preset.rect_size;

  public FishingRodLogic(FishingRodPreset preset)
  {
    this._rod_preset = preset;
    this._max_height = 100f - (float) this._rod_preset.rect_size;
    this._pulling_fish = false;
    this._t = this._y0 = this._v0 = this._v = 0.0f;
  }

  public float CalculateRodPos()
  {
    bool key = LazyInput.GetKey(GameKey.MiniGameAction);
    if (LazyInput.GetKeyDown(GameKey.MiniGameAction))
      Sounds.PlaySound("fishing_reel_short");
    float num = -this._rod_preset.gravity;
    if (key)
    {
      num += this._rod_preset.force / this._rod_preset.mass;
      if (!this._pulling_fish)
        this._v += this._rod_preset.impulse / this._rod_preset.mass;
    }
    if ((double) this._pos > 0.0)
      this._v += num * Time.deltaTime;
    this._pos += this._v * Time.deltaTime;
    if ((double) this._pos > (double) this._max_height)
    {
      this._v = 0.0f;
      this._pos = this._max_height;
    }
    else if ((double) this._pos < 0.0)
    {
      this._pos = 0.0f;
      this._v = (float) (-(double) this._v / 4.0);
      if ((double) Mathf.Abs(this._v) < 0.05000000074505806 * (double) this._rod_preset.impulse / (double) this._rod_preset.mass)
        this._v = 0.0f;
    }
    this._pulling_fish = key;
    return this._pos;
  }
}
