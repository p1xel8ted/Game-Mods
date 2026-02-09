// Decompiled with JetBrains decompiler
// Type: CanGoTransparent
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class CanGoTransparent : MonoBehaviour
{
  public SpriteRenderer[] _sprs;
  public float _a = 1f;
  public float _target_a = 1f;
  public bool _animating;
  public const float ALPHA_CHANGE_SPEED = 10f;
  public const float MIN_X_TRANSPARENT_DIST = 100f;
  public const float MIN_Z_TRANSPARENT_DIST = 50f;

  public void SetAlpha(float a)
  {
    if ((double) Math.Abs(this._target_a - a) < 0.01)
      return;
    this._target_a = a;
    this._animating = true;
  }

  public void Update()
  {
    if (!this._animating)
      return;
    float f = this._target_a - this._a;
    if ((double) Math.Abs(f) < 0.01)
    {
      this._animating = false;
    }
    else
    {
      this._a += (float) ((double) f * (double) Time.deltaTime * 10.0);
      if ((int) Mathf.Sign(this._target_a - this._a) != (int) Mathf.Sign(f))
      {
        this._a = this._target_a;
        this._animating = false;
      }
      this.SetSpritesAlpha(this._a);
    }
  }

  public void SetSpritesAlpha(float a)
  {
    if (this._sprs == null)
      this._sprs = this.gameObject.GetComponentsInChildren<SpriteRenderer>();
    foreach (SpriteRenderer spr in this._sprs)
    {
      Color color = spr.color with { a = a };
      spr.color = color;
    }
  }
}
