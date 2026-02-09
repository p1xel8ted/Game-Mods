// Decompiled with JetBrains decompiler
// Type: Stick
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Rewired;
using UnityEngine;

#nullable disable
public class Stick
{
  public const float NEW_DIR_DELAY = 0.05f;
  public const float MIN_MAGNITUDE = 0.1f;
  public const float OPPOSITE_DIR_MIN_MAGNITUDE = 0.5f;
  public Vector2 direction;
  public int _h_axis_id;
  public int _v_axis_id;
  public float _prev_h;
  public float _prev_v;
  public float _current_h;
  public float _current_v;
  public float _stick_delay;

  public bool has_direction => (double) this.direction.magnitude > 0.0;

  public Stick(int horizontal_axis_id, int vertical_axis_id)
  {
    this._h_axis_id = horizontal_axis_id;
    this._v_axis_id = vertical_axis_id;
  }

  public void Update(Player player)
  {
    this._current_h = player.GetAxis(this._h_axis_id);
    this._current_v = player.GetAxis(this._v_axis_id);
    this.direction = new Vector2(this._current_h, this._current_v);
    if ((double) this.direction.magnitude < 0.10000000149011612)
    {
      this.direction = Vector2.zero;
      this._current_h = this._current_v = 0.0f;
    }
    this._stick_delay -= Time.deltaTime;
    if ((double) this.direction.magnitude > 0.0)
    {
      if ((double) this._prev_h < 0.0 && (double) this._current_h > 0.0 || (double) this._prev_h > 0.0 && (double) this._current_h < 0.0 || (double) this._prev_v < 0.0 && (double) this._current_v > 0.0 || (double) this._prev_v > 0.0 && (double) this._current_v < 0.0)
        this._stick_delay = 0.05f;
    }
    else
      this._stick_delay = 0.0f;
    if ((double) this._stick_delay > 0.0 && (double) this.direction.magnitude < 0.5)
    {
      this.direction = Vector2.zero;
      this._current_h = this._current_v = 0.0f;
    }
    this._prev_h = this.direction.x;
    this._prev_v = this.direction.y;
  }
}
