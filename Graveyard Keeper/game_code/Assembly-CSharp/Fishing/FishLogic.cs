// Decompiled with JetBrains decompiler
// Type: Fishing.FishLogic
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Fishing;

public class FishLogic
{
  public const float OFFSET = 5f;
  public FishPreset _preset;
  public float _fish_pos = 5f;
  public float _progress;
  public float _fail_time;
  public bool _moving_up;
  public const string _FISH_PULL_MLTPLR_BUFF = "buff_pulling_fish_mltplr";

  public FishLogic(FishPreset preset)
  {
    this._preset = preset;
    this._moving_up = true;
    this._preset.InitTimeCalculation();
  }

  public FishLogic.Result CalculateFishPos(float pos, float rod_zone_size)
  {
    bool flag = (double) this._fish_pos >= (double) pos && (double) this._fish_pos <= (double) pos + (double) rod_zone_size;
    float f = MainGame.me.player.data.GetParam("buff_pulling_fish_mltplr");
    float num = (double) Mathf.Abs(f) > 0.0099999997764825821 ? f : 1f;
    this._fish_pos = this._preset.CalculateFishPos(Time.deltaTime, true) * 100f;
    this._progress += (flag ? this._preset.progress_k_in_zone : -this._preset.progress_k_out_of_zone / num) * Time.deltaTime;
    if ((double) this._progress <= 0.0)
      this._progress = 0.0f;
    else if ((double) this._progress > 1.0)
      this._progress = 1f;
    this._fail_time = this._progress.EqualsTo(0.0f) ? this._fail_time + Time.deltaTime : 0.0f;
    return new FishLogic.Result()
    {
      fish_pos = this._fish_pos,
      progress = this._progress,
      success = (double) this._progress >= 1.0,
      fail = (double) this._fail_time >= (double) this._preset.zero_progress_fail_time
    };
  }

  public struct Result
  {
    public float progress;
    public float fish_pos;
    public bool fail;
    public bool success;
  }
}
