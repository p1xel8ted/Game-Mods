// Decompiled with JetBrains decompiler
// Type: FallingPart
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DG.Tweening;
using UnityEngine;

#nullable disable
public class FallingPart : MonoBehaviour
{
  public Transform target_point;
  public InteractionAnimationPreset preset;
  public SpriteRenderer _spr;

  public void StartAnimation(GJCommons.VoidDelegate on_complete)
  {
    if ((Object) this.target_point == (Object) null || (Object) this.preset == (Object) null)
      return;
    this._spr = this.GetComponent<SpriteRenderer>();
    float duration = this.preset.duration + Random.Range(0.0f, this.preset.duration_random);
    Tweener t = this.transform.DOMoveY(this.target_point.transform.position.y, duration).OnComplete<Tweener>((TweenCallback) (() =>
    {
      if (on_complete == null)
        return;
      on_complete();
    }));
    this.transform.DORotate(new Vector3(0.0f, 0.0f, Random.Range(-this.preset.rotation_a, this.preset.rotation_a)), duration);
    t.OnUpdate<Tweener>((TweenCallback) (() =>
    {
      if (this.preset.alpha_curve == null || !((Object) this._spr != (Object) null))
        return;
      this._spr.color = this._spr.color with
      {
        a = this.preset.alpha_curve.Evaluate(t.ElapsedPercentage())
      };
    }));
  }
}
