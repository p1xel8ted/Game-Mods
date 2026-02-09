// Decompiled with JetBrains decompiler
// Type: NavigationStick
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class NavigationStick
{
  public const float MIN_DEAD_ZONE = 0.4f;
  public const float DELTA = 0.2f;
  public bool _vertical;
  public GamePadController _controller;
  public float _max_value;
  public float _min_value;
  public bool _wait_for_press;

  public NavigationStick(bool vertical, GamePadController controller)
  {
    this._vertical = vertical;
    this._controller = controller;
  }

  public void Update(Vector2 dir)
  {
    float f = this._vertical ? dir.y : dir.x;
    float num = Mathf.Abs(f);
    if ((double) num <= 0.40000000596046448)
    {
      this._min_value = this._max_value = 0.0f;
      this._wait_for_press = false;
    }
    else
    {
      GameKey key = this.GetKey(f);
      if (this._max_value.EqualsTo(0.0f))
      {
        this._max_value = num;
        this._min_value = this._max_value - 0.2f;
        this._wait_for_press = false;
        this._controller.AddPressed(key);
        this._controller.AddHolded(key);
      }
      else if ((double) num < (double) this._min_value)
      {
        this._wait_for_press = true;
        this._min_value = num;
        this._max_value = this._min_value + 0.2f;
      }
      else
      {
        if ((double) num < (double) this._min_value + 0.10000000149011612)
          return;
        if (this._wait_for_press)
        {
          this._controller.AddPressed(key);
          this._controller.AddHolded(key);
        }
        if ((double) num > (double) this._min_value + 0.20000000298023224)
        {
          this._max_value = num;
          this._min_value = this._max_value - 0.2f;
        }
        if (this._wait_for_press)
          this._wait_for_press = false;
        else
          this._controller.AddHolded(key);
      }
    }
  }

  public GameKey GetKey(float value)
  {
    return !this._vertical ? ((double) value <= 0.0 ? GameKey.Left : GameKey.Right) : ((double) value <= 0.0 ? GameKey.Down : GameKey.Up);
  }
}
